using System.IO;
using UnityEditor;
using UnityEngine;

namespace AuggoToolkit
{
    /// Renders the shipping scene at real device resolutions, so layout can be checked against
    /// an iPad or a tall Android phone without owning either.
    ///
    ///   Unity -batchmode -projectPath . -buildTarget iOS \
    ///         -executeMethod AuggoToolkit.DevicePreview.PreviewBatch -logFile preview.log
    ///
    /// These are canvas renders, not device screenshots: no safe areas, notch or home indicator.
    ///
    /// With a height-matched Canvas Scaler the risky direction is WIDE, not tall — a taller
    /// screen just gives a centred column less width. So the two that matter are the widest
    /// aspect supported (iPad 4:3) and the shortest (16:9). Everything between is safe.
    public static class DevicePreview
    {
        const string OutDir = "Builds/LayoutPreview";

        struct Device
        {
            public string Name; public int Width, Height;
            public Device(string n, int w, int h) { Name = n; Width = w; Height = h; }
        }

        static readonly Device[] Devices =
        {
            new Device("ipad-13-widest",   2064, 2752),   // 4:3   — the wide extreme
            new Device("legacy-16x9",      1080, 1920),   // 16:9  — the short extreme
            new Device("pixel-8-pro",      1344, 2992),
            new Device("iphone-6.9",       1320, 2868),
            new Device("xperia-21x9",      1644, 3840),   // tallest mainstream phone
        };

        [MenuItem("Tools/Auggo/Preview On Devices")]
        public static void PreviewFromMenu()
        {
            if (Run(openScene: false))
                EditorUtility.RevealInFinder(Path.GetFullPath(OutDir));
        }

        public static void PreviewBatch() { EditorApplication.Exit(Run(openScene: true) ? 0 : 1); }

        static bool Run(bool openScene)
        {
            if (openScene && !SceneRenderer.OpenShippingScene()) return false;

            Camera cam = SceneRenderer.FindRenderCamera();
            if (cam == null) { Debug.LogError("[Auggo] No camera found to render from."); return false; }

            foreach (Device d in Devices)
            {
                string path = Path.Combine(OutDir, $"{d.Name}-{d.Width}x{d.Height}.png");
                if (SceneRenderer.Capture(cam, d.Width, d.Height, path) != null)
                    Debug.Log($"[Auggo] {path}");
            }
            return true;
        }
    }
}
