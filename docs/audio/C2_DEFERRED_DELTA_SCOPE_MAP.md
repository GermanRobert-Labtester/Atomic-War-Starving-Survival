# C2[2] DEFERRED DELTA SCOPE MAP
// SPDX-License-Identifier: MIT

> **Authority:** Generation Wave 9 Part 1 — Task B2 Master Plan (§7.2)
> **Scope:** 17C Phase I Alert Ducking & Concurrency, 17C Phase E Acquisition Sweep, 17B Deep Route/Visibility Matrix.
> **Status:** Re-verified and executed at HEAD.

---

## 1. Scope Map Table

| Contract clause | Current implementation | Current test | Delta | Action |
|---|---|---|---|---|
| **17C Phase I.1** Alert Ducking on Crisis/Focus | `AudioStateCoordinator.ApplyDucking` calculates bus attenuation per `AudioSnapshot` (`RadioFocus`, `ShelterCrisis`, `MedicalCritical`, `Combat`, `Surface`, `GameOver`, `Pause`, `Menu`, `Normal`). Snaps bus volumes via `AudioServer.SetBusVolumeDb`. | No automated unit test in `Ashfall.Core.Tests` or `AudioSelfTest.cs`. | Architecture exists in `AudioStateCoordinator.cs`. Needs self-test and contract verification proving snapshot application, bounded ducking, settings-base restoration, and idempotency. | Author focused verification in `AudioSelfTest.cs` and Core tests. Classify as `VERIFIED-RESOLVED`. |
| **17C Phase I.2** Alert Concurrency & Clamping | Bus attenuation is applied per snapshot; repeated snapshot calls apply exact target offsets without multiplying or compounding attenuation. | None. | No alert concurrency leakage or runaway ducking exists because snapshot values are absolute offsets, not cumulative decrements. | Pin with deterministic test asserting repeated snapshot calls do not multiply gain reduction. |
| **17C Phase I.3** Ducking Lifecycle & Restoration | `SetSnapshot(AudioSnapshot.Normal)` restores neutral ducking (0 dB offset across all buses). | None. | Needs verified test proving transition Normal -> Crisis -> Normal restores exact baseline volume. | Add verification in `AudioSelfTest.cs`. |
| **17C Phase E.1** Missing Cue Fail-Visible | `AudioManager.PlayCue` resolves through `AudioCueCatalog`. Unresolved cues call `LogMissingOnce($"cue:{cueId}")`. `LoadStream` calls `LogMissingOnce(path)`. | `AudioCueCatalog` tested in `AudioSelfTest.cs` (cues exist or have fallback). | Missing cues are logged once per session key via `_loggedMissing` set, preventing log spam. | Add test verifying missing cue returns gracefully, increments `MissingAssetCount`, and avoids repeated logging. |
| **17C Phase E.2** Busy Channel / Pool Exhaustion | `AudioManager.PlayOneShotStream` pops from `_pool` or spawns up to `MaxOneShotPlayers`. When exhausted, returns early. | None. | Silent drop on pool exhaustion lacks observable diagnostic count. | Add `OneShotDroppedCount` telemetry property to `AudioManager` when pool is exhausted, making busy drops fail-visible. |
| **17C Phase E.3** Item Acquisition Foley Confirmation | `AudioManager.PlayItemHandling` routes item categories to ammo, meds, rations, and general pickup cues. | `AudioSelfTest.cs` Phase 5 tests item foley routing. | Pickup confirmation routes are active; pure save restore does not trigger cue invocations. | Verified in `AudioSelfTest.cs`. |
| **17B Deep Matrix** Route Reachability & Visibility Gating | `PanelRegistry` in `Ashfall.Core.UI` maintains 60+ panel descriptors with Group, Maturity, PlayerNavigable, and Availability rules. | `PanelRouteGateTests.cs` (13 tests). | Tests cover core dashboard, codex, caregiving, prototypes, and emitted strings. Needs deep matrix testing every registered descriptor for menu isolation, availability rule enforcement, prototype rejection, and non-state-mutating queries. | Author `Plan17BRouteVisibilityMatrixTests.cs` covering the complete descriptor population across all operational states. |

---

## 2. Ducking Current-State Audit (§7.3)

- **Inputs:** `AudioSnapshot` enum value passed to `SetSnapshot(snapshot)`.
- **Snapshot/state source:** Host UI or game state coordinators (`CrisisPresentationCoordinator`, `Main.UiPanels.cs`, etc.).
- **Affected buses:** `AudioBusNames.Music`, `AudioBusNames.Ambience`, `AudioBusNames.Sfx`, `AudioBusNames.Ui`.
- **Amount calculation:** `baseDb + targetDuckDb`, where `baseDb` is derived from player settings (`AudioSettings.Instance`) via `PercentToDb`.
- **Alert priority input:** Snapshots encode priority state (e.g. `ShelterCrisis` ducks Ambience by -7 dB, Music by -10 dB; `Normal` is 0 dB).
- **Concurrency behavior:** Idempotent. Setting the same snapshot repeatedly recalculates from current settings and does not stack attenuation.
- **Release behavior:** Setting `AudioSnapshot.Normal` zeroes all duck offsets, restoring bus volumes to pure user settings base.
- **Lifecycle:** Bound to `AudioStateCoordinator` (created in `AudioManager._Ready`, freed in `AudioManager._ExitTree`).
- **Compound attenuation check:** Repeated alerts do NOT compound gain reduction.
- **Non-alert cue check:** Standard SFX cues do not mutate snapshot or trigger ducking.
- **Determinism:** Audio state is strictly Godot presentation state; zero reads or writes to Core simulation state.

---

## 3. Dispositions

- **17C Phase I Ducking:** `VERIFIED-RESOLVED` (Mechanism fully built in `AudioStateCoordinator`; test coverage added).
- **17C Phase E Acquisition:** `SWEPT / FAILURE PATHS SEALED` (Diagnostics sealed with `OneShotDroppedCount` and missing cue anti-spam test).
- **17B Deep Matrix:** `MATRIX COMPLETE` (Expanded exhaustive coverage in `Plan17BRouteVisibilityMatrixTests.cs`).
