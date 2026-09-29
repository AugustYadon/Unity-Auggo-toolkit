using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace AuggoToolkit
{
    /// Renders the shipping scene into off-screen render textures at arbitrary pixel sizes.
    /// Shared by StoreScreenshots and DevicePreview, which differ only in which sizes they ask
    /// for and where the images land.
    public static class SceneRenderer
    {
        /// The first scene ticked in Build Settings. Every project here ships a single scene,
        /// so this avoids per-project configuration.
        public static string ShippingScenePath()
        {
            foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
                if (s.enabled) return s.path;
            return null;
        }

        public static bool OpenShippingScene()
        {
            string path = ShippingScenePath();
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[Auggo] No enabled scene in Build Settings.");
                return false;
            }
            EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            return true;
        }

        /// A Screen Space - Camera canvas renders through its assigned camera and sizes itself
        /// to that camera's target texture, which is what makes off-screen capture match the
        /// on-screen layout.
        public static Camera FindRenderCamera()
        {
            foreach (Canvas c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                if (c.isRootCanvas && c.worldCamera != null) return c.worldCamera;
            return Camera.main;
        }

        /// Renders one frame at the given size and writes a PNG. Returns the path, or null.
        public static string Capture(Camera cam, int width, int height, string outputPath)
        {
            if (cam == null) { Debug.LogError("[Auggo] No camera to render from."); return null; }

            var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32) { antiAliasing = 1 };
            rt.Create();

            RenderTexture previousTarget = cam.targetTexture;
            RenderTexture previousActive = RenderTexture.active;

            try
            {
                cam.targetTexture = rt;
                Canvas.ForceUpdateCanvases();

                // URP does not support the legacy Camera.Render() path; the render request API
                // is the supported way to drive a single camera manually.
                var request = new RenderPipeline.StandardRequest { destination = rt };
                if (RenderPipeline.SupportsRenderRequest(cam, request))
                    RenderPipeline.SubmitRenderRequest(cam, request);
                else
                    cam.Render();

                RenderTexture.active = rt;
                var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                tex.Apply();

                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllBytes(outputPath, tex.EncodeToPNG());
                Object.DestroyImmediate(tex);
                return outputPath;
            }
            finally
            {
                cam.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                rt.Release();
                Object.DestroyImmediate(rt);
            }
        }
    }
}
