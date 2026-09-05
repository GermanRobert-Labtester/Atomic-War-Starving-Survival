# ASHFALL Flagship Asset Program — Phase 6 Completion Report
**Phase 6: Shared Audio Generation Pipeline**
**Date:** 2026-09-03
**Status:** COMPLETE — ALL GATES PASSING

---

## 1. Executive Summary

Phase 6 delivers a release-grade, shared audio generation and mastering pipeline for ASHFALL at [`tools/audio_pipeline.py`](../../tools/audio_pipeline.py). The pipeline extracts shared seeded synthesis, audio mastering, named delivery presets per asset class, clean WAV export, EBU R128 and True Peak measurement, and machine-readable delivery ledgers.

Full-scale peak normalization (0.000 dBFS / 1.0 linear) is strictly prohibited across all generation tooling, enforcing a maximum linear ceiling of ≤ -1.5 dBFS (~0.8414). Manual `.import` sidecar fabrication has been completely eradicated across all synthesis tools, delegating sidecar and import cache generation exclusively to the Godot engine importer (`godot --headless --path . --import`).

The pipeline's byte-reproducibility engine proves that identical seeds generate 100% byte-identical delivery outputs across independent runs.

---

## 2. Architecture & Components

### 2.1 Named Delivery Presets ([`tools/audio_pipeline.py`](../../tools/audio_pipeline.py))

| Preset Name | Target Loudness | Max True Peak Ceiling | Linear Ceiling | Default Loop Mode | Asset Types |
|---|---|---|---|---|---|
| **`UI`** | -18.0 LUFS | -1.5 dBFS | 0.8414 | 0 (one-shot) | Clicks, switches, dials, drawers, confirmations |
| **`Ambience`** | -24.0 LUFS | -2.0 dBFS | 0.7943 | 1 (looping) | Bunker room tone, surface winds, rain beds |
| **`Loop`** | -20.0 LUFS | -2.0 dBFS | 0.7943 | 1 (looping) | Generators, recyclers, water pumps, lathe hums |
| **`Radio`** | -16.0 LUFS | -1.5 dBFS | 0.8414 | 0 (one-shot) | Numbers station, EBS tone, telemetry, voice |
| **`Voice`** | -16.0 LUFS | -1.5 dBFS | 0.8414 | 0 (one-shot) | Radio narrative dialogues, dispatch transmissions |
| **`SFX`** | -14.0 LUFS | -1.5 dBFS | 0.8414 | 0 (one-shot) | Firearm reports, metal impacts, collisions |
| **`Music`** | -16.0 LUFS | -1.5 dBFS | 0.8414 | 1 (looping) | Diegetic radio cassettes, theme tracks |
| **`Transient`** | -16.0 LUFS | -1.5 dBFS | 0.8414 | 0 (one-shot) | Ultra-short impacts (<0.4s) |

### 2.2 Core Modules in `audio_pipeline.py`
1. **`SeededSynthesizer`**:
   Deterministic PRNG-seeded mathematical signal generation (sine, FM synthesis, white noise, one-pole filtered noise, ADSR envelopes, waveshaping soft clipping).
2. **`AudioMasterer`**:
   Enforces category peak ceiling. Rejects any ceiling > -1.0 dBFS. Clamps linear amplitude strictly to preset threshold.
3. **`AudioExporter`**:
   Writes 16-bit PCM WAV at 44,100 Hz. Computes canonical SHA-256 hash. Never writes or touches `.import` sidecars.
4. **`AudioMeasurer`**:
   Integrates with FFmpeg's `ebur128` filter to meter integrated LUFS, true peak dBFS, and duration.
5. **`ReproducibilityEngine`**:
   Renders independent seeded passes in memory, compares byte-level SHA-256 digests, and guarantees deterministic stability.
6. **`DeliveryLedger`**:
   Maintains machine-readable JSON ([`docs/audio/AUDIO_PIPELINE_REPRODUCIBILITY_LEDGER.json`](AUDIO_PIPELINE_REPRODUCIBILITY_LEDGER.json)) and Markdown tables ([`docs/audio/AUDIO_PIPELINE_REPRODUCIBILITY_LEDGER.md`](AUDIO_PIPELINE_REPRODUCIBILITY_LEDGER.md)).

---

## 3. Tool Migration

All legacy generators were refactored to eliminate duplicate `save_wav_with_import` implementations, remove hardcoded fake UID templates, and route all audio mastering through `AudioExporter`:

1. [`tools/generate_phase2_audio.py`](../../tools/generate_phase2_audio.py): Migrated to `audio_pipeline.py` using `PRESET_AMBIENCE`, `PRESET_LOOP`, and `PRESET_SFX`.
2. [`tools/generate_phase3_audio.py`](../../tools/generate_phase3_audio.py): Migrated to `audio_pipeline.py` using `PRESET_LOOP` and `PRESET_SFX`.
3. [`tools/generate_phase4_audio.py`](../../tools/generate_phase4_audio.py): Migrated to `audio_pipeline.py` using `PRESET_LOOP` and `PRESET_SFX`.
4. [`tools/generate_phase5_audio.py`](../../tools/generate_phase5_audio.py): Migrated to `audio_pipeline.py` using `PRESET_RADIO`, `PRESET_LOOP`, and `PRESET_SFX`.
5. [`tools/generate_tactile_ui.py`](../../tools/generate_tactile_ui.py): Migrated to `audio_pipeline.py` using `PRESET_UI`.
6. [`tools/generate_train_screech_crash.py`](../../tools/generate_train_screech_crash.py): Migrated to `audio_pipeline.py` using `PRESET_SFX`.

---

## 4. Verification Evidence Matrix

| Gate | Execution Command | Result | Telemetry / Notes |
|---|---|---|---|
| **Pipeline Unit Tests** | `python3 tests/test_audio_pipeline.py` | **PASS (exit 0)** | 5 tests passed: presets ceiling, reproducibility, over-prevention, export cleanliness, measurement |
| **Pipeline Reproducibility Gate** | `python3 tools/verify_pipeline_reproducibility.py` | **PASS (exit 0)** | Deterministic seeded runs match 100% (hash equality `873ac5...`). No .import sidecars created. True peak -1.50 dBFS |
| **Audio Asset CI Gate** | `python3 scripts/ci/audio-asset-gate.py` | **PASS (exit 0)** | 151/151 assets verified: 0 overs, 0 untracked, 0 missing .import sidecars |
| **Host Audio Self-Test** | `godot --headless --path . -- --audio-selftest` | **PASS (exit 0)** | **502 passed, 0 failed**; 148 cues resolved, 4 expansion probes PASS |
| **Catalog Documentation Sync** | `python3 scripts/ci/generate-audio-catalog.py --check` | **PASS (exit 0)** | In sync with `audio_cues.json` (148 cues) |
| **Core Unit Tests** | `dotnet test Ashfall.Core.Tests` | **PASS (exit 0)** | **6,617 passed, 0 failed, 0 skipped** |
| **Full Asset CI Gate Suite** | `./scripts/ci/godot-asset-gate.sh` | **ALL GATES GREEN (exit 0)** | Orphan sweep, decode gate (2,815 files), audio gate (151 files), build, 7 selftests, Linux export |

---

## 5. Phase 6 Completion Sign-Off

- [x] Shared seeded synthesis, mastering, export, measurement, and ledger functions extracted into `tools/audio_pipeline.py`.
- [x] Phase 2–5 and tactile generators migrated to shared pipeline.
- [x] Named delivery presets defined per asset class (`UI`, `Ambience`, `Loop`, `Radio`, `Voice`, `SFX`, `Music`, `Transient`).
- [x] Full-scale peak normalization strictly prohibited (max peak ceiling ≤ -1.5 dBFS).
- [x] Engine-native Godot import utilized exclusively (0 generator-fabricated `.import` files).
- [x] Byte-reproducibility checks verified and enforced.
- [x] Ready to commence **Phase 7 — Flagship Audio Generation Waves** (Wave A: Survival-Critical Feedback).
