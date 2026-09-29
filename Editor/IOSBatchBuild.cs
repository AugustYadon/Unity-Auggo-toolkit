using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AuggoToolkit
{
    /// Headless iOS build. Unity exposes no command-line flag for iOS the way it does for
    /// desktop players, so a build has to go through BuildPipeline.
    ///
    ///   Unity -batchmode -nographics -projectPath . -buildTarget iOS \
    ///         -executeMethod AuggoToolkit.IOSBatchBuild.Build -logFile build.log
    ///
    /// Output lands in Builds/iOS, which should be gitignored. Open the .xcodeproj there to
    /// sign, run on device or archive.
    ///
    /// Two things to expect and ignore, both documented in claude-workflows/GOTCHAS.md:
    /// hundreds of libc++ platform warnings from a hardcoded IL2CPP flag, and a missing dSYM
    /// for UnityRuntime.framework that Unity does not ship.
    public static class IOSBatchBuild
    {
        const string OutputPath = "Builds/iOS";

        public static void Build()
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("[Auggo] No enabled scenes in Build Settings.");
                EditorApplication.Exit(1);
                return;
            }

            Debug.Log($"[Auggo] Building {scenes.Length} scene(s) to {OutputPath}");

            // Unity appends to an existing Xcode project rather than replacing it, which
            // preserves manual Xcode edits but also hides stale-state bugs. Start clean.
            if (Directory.Exists(OutputPath)) Directory.Delete(OutputPath, true);

            BuildSummary summary = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = OutputPath,
                target = BuildTarget.iOS,
                targetGroup = BuildTargetGroup.iOS,
                options = BuildOptions.None,
            }).summary;

            Debug.Log($"[Auggo] result={summary.result} errors={summary.totalErrors} " +
                      $"warnings={summary.totalWarnings} time={summary.totalTime}");

            EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }
    }
}
