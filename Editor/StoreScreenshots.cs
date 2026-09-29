using System.IO;
using UnityEditor;
using UnityEngine;

namespace AuggoToolkit
{
    /// Renders App Store screenshots at Apple's required pixel sizes without a device. Apple
    /// validates dimensions and content, not what captured them.
    ///
    ///   Unity -batchmode -projectPath . -buildTarget iOS \
    ///         -executeMethod AuggoToolkit.StoreScreenshots.CaptureBatch -logFile shots.log
    ///
    /// Note: no -nographics. Rendering needs a graphics device.
    ///
    /// KNOWN GAP: this captures with the app stopped, so anything only alive in Play mode — a
    /// line renderer's waveform, animations — renders in its inactive state. Play-mode capture
    /// with page navigation is the intended next version.
    public static class StoreScreenshots
    {
        const string OutDir = "Builds/Screenshots";

        struct Size
        {
            public string Name; public int Width, Height;
            public Size(string n, int w, int h) { Name = n; Width = w; Height = h; }
        }

        // Apple scales every smaller iPhone slot down from the largest supplied, so 6.9" alone
        // covers all eight iPhone sizes. iPad has no such fallback and needs its own.
        static readonly Size[] Sizes =
        {
            new Size("iphone-6.9", 1320, 2868),
            new Size("ipad-13",    2064, 2752),   // required only while iPad support ships
        };

        [MenuItem("Tools/Auggo/Capture Store Screenshots")]
        public static void CaptureFromMenu()
        {
            if (Run(openScene: false))
                EditorUtility.RevealInFinder(Path.GetFullPath(OutDir));
        }

        public static void CaptureBatch() { EditorApplication.Exit(Run(openScene: true) ? 0 : 1); }

        static bool Run(bool openScene)
        {
            // From the menu the scene is already loaded, and reopening would discard unsaved work.
            if (openScene && !SceneRenderer.OpenShippingScene()) return false;

            Camera cam = SceneRenderer.FindRenderCamera();
            if (cam == null) { Debug.LogError("[Auggo] No camera found to render from."); return false; }

            foreach (Size s in Sizes)
            {
                string path = Path.Combine(OutDir, $"{s.Name}-{s.Width}x{s.Height}.png");
                if (SceneRenderer.Capture(cam, s.Width, s.Height, path) != null)
                    Debug.Log($"[Auggo] {path}");
            }

            AssetDatabase.Refresh();
            return true;
        }
    }
}
