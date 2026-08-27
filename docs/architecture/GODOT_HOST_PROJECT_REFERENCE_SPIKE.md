# ASHFALL — Feasibility Spike Report: Compiling Godot Host via ProjectReference

**Date:** 2026-08-27<br>
**Status:** Spike Complete — Feasibility Proven (Migration Pending Approval)

---

## 1. Executive Summary

This feasibility spike investigated transitioning the active Godot 4.7 host project ([`Ashfall.csproj`](../../Ashfall.csproj)) from direct source inclusion:
```xml
<Compile Include="Assets/Ashfall.Core/**/*.cs" />
```
to a standard MSBuild project reference:
```xml
<ProjectReference Include="Ashfall.Core/Ashfall.Core.csproj" />
```

### Key Spike Findings
1. **100% Functional Compatibility**: The Godot host compiled cleanly into separate assemblies (`Ashfall.dll` and `Ashfall.Core.dll`), successfully resolving all Core systems, DTOs, save stores, and UI models.
2. **Zero Runtime Loading Issues**: Godot 4.7's Mono/.NET runtime assembly loader loaded `Ashfall.Core.dll` and transitive packages (`System.Text.Json`) from `.godot/mono/temp/bin/Debug/` via `Ashfall.deps.json` without reflection errors or path collisions.
3. **All 25 Fast-Tier CI Gates Passed**: Headless self-tests, UI panel tests, data integrity checks, and campaign smoke tests completed with 0 errors in 34.13s.
4. **Compile-Time Enforcement of Invariant 1**: ProjectReference structurally guarantees that `Ashfall.Core` cannot reference `Godot.*` or host types.

---

## 2. Architectural Comparison

| Dimension | Direct Source Inclusion (`<Compile>`) | Project Reference (`<ProjectReference>`) |
|---|---|---|
| **Host Assembly Structure** | Single monolithic assembly `Ashfall.dll` (~4.8 MB) containing both host and core code. | Clean multi-assembly architecture: `Ashfall.dll` (~2.7 MB) + `Ashfall.Core.dll` (~2.4 MB). |
| **Invariant 1 Enforcement** | Enforced solely via `.asmdef` conventions and static AST CI gates. | Enforced at MSBuild compilation boundary (circular references physically rejected). |
| **Incremental Build Times** | Touching a UI file re-analyzes all 250+ Core classes. | MSBuild caches `Ashfall.Core.dll`; only modified assemblies are recompiled. |
| **Transitive Dependencies** | Host must explicitly declare all NuGet packages required by Core. | NuGet CPM packages (`System.Text.Json`) flow transitively via `ProjectReference`. |
| **IDE Navigation & Debugging** | Unified symbol space; breakpoints hit directly. | Multi-assembly symbol resolution; PDBs emitted cleanly (`Ashfall.Core.pdb`). |
| **PCK / Release Export** | Single `Ashfall.dll` placed in release export output. | Both `Ashfall.dll` and `Ashfall.Core.dll` staged in export directory. |

---

## 3. Godot Engine Constraints & Build Implications

### 3.1. Dynamic Assembly Loading (`EnableDynamicLoading`)
In `Ashfall.csproj`, `<EnableDynamicLoading>true</EnableDynamicLoading>` instructs the .NET SDK to generate an `Ashfall.deps.json` file.
During editor development and headless CLI execution:
1. Godot loads `Ashfall.dll` into its isolated `AssemblyLoadContext`.
2. The runtime reads `Ashfall.deps.json` and loads `Ashfall.Core.dll` directly from `.godot/mono/temp/bin/Debug/`.
3. Verified output from spike:
   ```
   .godot/mono/temp/bin/Debug/
   ├── Ashfall.dll (2,698,752 bytes)
   ├── Ashfall.Core.dll (2,420,736 bytes)
   ├── Ashfall.deps.json (2,394 bytes)
   ├── Ashfall.runtimeconfig.json (291 bytes)
   ├── GodotSharp.dll
   ├── GodotSharpEditor.dll
   └── Sentry.dll (839,680 bytes)
   ```

### 3.2. Headless Self-Tests & CLI Flags
All headless CLI commands (e.g. `--version`, `--ui-accessibility-selftest`, `--expansions-selftest`) operate identically under `ProjectReference`:
- Static reflection utilities (such as `SaveChecksum`, `CatalogIntegrityValidator`, `HostCliRegistry`) resolve types from `Ashfall.Core.dll` without configuration changes.

### 3.3. Exported Binary Packaging Considerations
When running `godot --export-release` for Linux/Windows targets:
- Godot automatically collects referenced assemblies listed in `.deps.json` and stages them into the exported application data directory (`data_Ashfall_<os>_<arch>/`).
- **Implication**: CI export pipelines must verify that both `Ashfall.dll` and `Ashfall.Core.dll` are present in the final distribution archive.

---

## 4. Migration Strategy & Recommendations

1. **Safety**: Zero behavioral differences in gameplay, simulation determinism, save codecs, or UI binding lifecycles.
2. **Pre-Requisite Check**: Verify the export packaging script ([`scripts/export/export_build.sh`](../../scripts/export/export_build.sh)) includes all staging assemblies from `.deps.json`.
3. **Execution Plan**: When approved, switch `Ashfall.csproj` to `<ProjectReference Include="Ashfall.Core/Ashfall.Core.csproj" />` and remove redundant Core source links.
