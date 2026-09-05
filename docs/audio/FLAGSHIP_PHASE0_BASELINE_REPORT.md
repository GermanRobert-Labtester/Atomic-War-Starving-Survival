# Flagship Asset Library Program — Phase 0 Baseline & Forensic Report

**Document Version:** 1.0.0
**Date:** 2026-09-03
**Branch:** `feat/asset-pipeline-flagship`
**Execution Target:** Phase 0 (Freeze, Baseline, Audio Preservation, Import Crash Diagnosis)

---

## 1. Phase 0 Objectives & Status

| Milestone Objective | Required Action | Status | Forensic Evidence |
|---|---|---|---|
| **1. Dedicated Branch** | Branch from stable revision to freeze execution baseline | **DONE** | Switched to branch `feat/asset-pipeline-flagship`. |
| **2. Audio Preservation** | Hash and preserve all 82 WAV files (including 52 untracked) before any generator rerun | **DONE** | Saved to `.cache/audio_preservation_pre_flagship/`; manifest in `docs/audio/AUDIO_PRESERVATION_MANIFEST_PRE_FLAGSHIP.json`. |
| **3. Crash Reproduction** | Reproduce the Godot import crash without modifying generator scripts | **DONE** | Reproduced exit code 134 / signal 4 / SIGABRT during `godot --headless --path . --import`. |
| **4. Crash Diagnosis** | Bisect directory tree to identify root cause and owner of the import crash | **DONE** | Isolated to `assets/l10n/strings.en.translation` (corrupted binary `OptimizedTranslation`). |
| **5. Baseline Verification** | Execute full canonical test and verification matrix | **DONE** | All 5 canonical checks passing cleanly. |

---

## 2. Audio Library Preservation & Census

- **Total WAV Sources:** 82 files (30 tracked, 52 untracked from generator passes 2–5).
- **Untracked WAVs Preserved:** 52 files in `assets/audio/ambience/`, `assets/audio/radio/`, `assets/audio/sfx/`, and `assets/audio/ui/`.
- **Preservation Storage:** `.cache/audio_preservation_pre_flagship/` (full directory tree mirrored).
- **Manifest:** `docs/audio/AUDIO_PRESERVATION_MANIFEST_PRE_FLAGSHIP.json` records SHA-256 hash, file size, and backup path for every single source file.
- **Mastering Baseline (Audit F-08):** Confirmed all 52 untracked generated WAV files peak at exactly 0.000 dBFS (unmastered full-scale normalization) and require Phase 4 headroom remastering.

---

## 3. Forensic Diagnosis of Godot Import Crash (Exit 134)

### 3.1 Initial Crash Symptoms
Executing `godot --headless --path . --import` failed with:
```text
[ DONE ] first_scan_filesystem

ERROR: Parameter "mem" is null.
   at: alloc_static (core/os/memory.cpp:104)
ERROR: Parameter "mem_new" is null.
   at: _alloc_exact (./core/templates/cowdata.h:476)
ERROR: FATAL: Index p_index = 0 is out of bounds (((Vector<T> *)(this))->_cowdata.size() = 0).
   at: operator[] (./core/templates/vector.h:54)
ERROR: /root: The caller thread can't call the function `propagate_notification()` on this node. Use `call_deferred()` or `call_deferred_thread_group()` instead.
   at: propagate_notification (scene/main/node.cpp:2578)

================================================================
handle_crash: Program crashed with signal 4
Engine version: Godot Engine v4.7.1.stable.mono.official (a13da4feb8d8aefc283c3763d33a2f170a18d541)
```

### 3.2 Top-Level Directory Bisection
Isolating top-level directories in headless test environments against `project.godot`:
- `scenes/` → exit 0
- `src/` → exit 0
- `Assets/` → exit 0
- `scripts/` → exit 0
- `docs/` → exit 0
- **`assets/` → exit -6 (SIGABRT / exit 134)**

### 3.3 Assets Subdirectory Bisection
Isolating subdirectories within `assets/`:
- `assets/art/` → exit 0
- `assets/audio/` → exit 0
- `assets/fonts/` → exit 0
- `assets/quarantine/` → exit 0
- `assets/sprites/` → exit 0
- `assets/ui/` → exit 0
- **`assets/l10n/` → exit -6 (SIGABRT / exit 134)**

### 3.4 File Bisection in `assets/l10n/`
Testing individual files in `assets/l10n/`:
- `strings.csv` → exit 0
- `strings.csv.import` → exit 0
- `strings.source.translation` → exit 0
- `template.pot` → exit 0
- **`strings.en.translation` → exit -6 (SIGABRT / exit 134)**

### 3.5 Root Cause Proof
- **Corrupted Artifact:** `assets/l10n/strings.en.translation` was an invalid/mismatched binary `OptimizedTranslation` resource (3,259 bytes) with corrupted bucket tables.
- **Godot Memory Failure:** When Godot's `EditorFileSystem` scanned and parsed `strings.en.translation`, the malformed header caused `alloc_static` to return null, leaving internal `Vector<T>` with size 0 and triggering `operator[] (vector.h:54)` out-of-bounds assertion failure.
- **Proof of Fix:** Re-importing `strings.csv` cleanly through Godot 4.7.1 generated a valid 4,148-byte `strings.en.translation`. Re-running `godot --headless --path . --import` on the project with this valid translation completed with **exit code 0** (all 118 asset steps processed, editor layout loaded, zero crashes).

---

## 4. Secondary Pre-Flight Findings Identified

1. **`marker_safe.png` (Audit F-13):**
   - File format: ASCII text (base64 encoded beginning with `iVBORw0KGgoAAA...`).
   - Length: 1,061 bytes (has one trailing invalid ASCII character `A`).
   - Slicing to 1,060 bytes yields a valid 795-byte 32×32 RGBA PNG image.
   - Required action for Phase 1: decode to binary PNG or remove if unreferenced.
2. **`snapshots/` directory:**
   - Contains UI test screenshots (`*.png`).
   - Currently lacks a `.gdignore` file, causing Godot's asset importer to attempt reimporting test snapshots as game textures.
   - Adding `snapshots/.gdignore` prevents unnecessary scanner churn.

---

## 5. Canonical Baseline Verification Results

All tests executed at baseline:
- `dotnet test Ashfall.Core.Tests`: **PASS** (6,616 passed, 0 failed).
- `dotnet build Ashfall.csproj`: **PASS** (0 warnings, 0 errors).
- `godot --headless --path . -- --data-integrity-selftest`: **PASS** (0 findings across 208 catalogs).
- `godot --headless --path . -- --content-utilization-selftest`: **PASS** (CI gate PASS).
- `godot --headless --path . -- --scene-binding-selftest`: **PASS** (22/22 passed).
- `python3 scripts/ci/scene-lint.py`: **PASS** (27 production scenes checked, 0 errors).
- `godot --headless --path . --import`: **PASS** (exits 0 with valid translation artifact).
