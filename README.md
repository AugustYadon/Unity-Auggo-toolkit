# Unity-Auggo-toolkit

Reusable editor tools and UI pieces for Auggo Doggo Games projects. Mine, not a third-party
package.

## Install

Add to `Packages/manifest.json`:

```json
"com.auggodoggo.toolkit": "https://github.com/AugustYadon/Unity-Auggo-toolkit.git"
```

Git dependencies are **pinned by commit hash** in `Packages/packages-lock.json`. Pushing a fix
here does not update consumers — delete the lock entry and let Unity re-resolve.

## What is in it

| Tool | Menu | What it does |
|---|---|---|
| `StoreScreenshots` | `Tools/Auggo/Capture Store Screenshots` | Renders the shipping scene at App Store pixel sizes |
| `DevicePreview` | `Tools/Auggo/Preview On Devices` | Renders at real device resolutions to check layout without owning the hardware |
| `IOSBatchBuild` | — | Headless iOS build; Unity has no command-line flag for it |

All three work with no configuration: they use the first enabled scene in Build Settings and
write to `Builds/`, which should be gitignored.

## Samples

Showcase pieces live in `Samples~/`, one folder each with its own scene. The tilde keeps Unity
from importing them; Package Manager shows an **Import** button per sample instead, so a project
only pulls in what it wants.

Not everything belongs in a sample. A build script does not need a scene. A wheel picker or a
date-range picker does, because seeing it beats reading about it.

## Adding something

When a script or prefab proves useful in a project:

1. Copy it into `Editor/` (editor tooling) or `Runtime/` (anything shipped in a build)
2. Strip project-specific names and paths — it must work with zero configuration
3. If it is worth *seeing*, add `Samples~/<Name>/` with a scene demonstrating it, and register
   the sample in `package.json`
4. Note it in the table above

## Related

- **[claude-workflows](https://github.com/AugustYadon/claude-workflows)** — release and store-prep notes
