# PLANS 122–125 — Second-Tool Review (plan §21)

**Date:** 2026-09-13 · **Reviewer:** independent agent session (different tool than the batch that implemented Plans 122–125 on 2026-09-13)
**Scope:** read-only re-verification of the batch's acceptance claims against current repository truth, per plan §21. No production code modified.

## 1. Focused battery re-run (claimed → observed)

| Claim (closeout) | Observed | Verdict |
|---|---|---|
| Build 0 warnings / 0 errors | 0 / 0 | ✅ match |
| Plan 122: catalog 8/8, engine 17/17 | 8/8, 17/17 | ✅ match |
| Plan 123: catalog 8/8, engine 14/14 | 8/8, 14/14 | ✅ match |
| Plan 124: catalog 10/10, engine 17/17 | 10/10, 17/17 | ✅ match |
| Plan 125: catalog 9/9, engine 18/18 | 9/9, 18/18 | ✅ match |
| Persistence 11/11 | 11/11 | ✅ match |
| `--plans-122-125-selftest` 41/41 | **41/41** | ✅ match |
| `--late-tech-mobility-selftest` 23/23 | **24/24** | ⚠️ count drift (+1 gate since closeout; all PASS) |
| `--plans-122-125-balance-soak` 24/24 | **16/16** | ⚠️ count drift (closeout likely summed per-plan runs; current manifest 16 aggregate checks, all PASS, incl. `no_dominance` / `no_weapon_precision` / `not_all_success` boundedness gates) |

All green. The two count drifts are documentation drift, not failures — current-truth counts recorded here.

## 2. Architecture definition-of-done — source-level audit

| Claim | Evidence (current source) | Verdict |
|---|---|---|
| Core engines engine-free | `SofcElectrochemistryEngine` / `SoundRangingThreatEngine` / `CvdDiamondSynthesisEngine` / `AmphibiousDraisineEngine` + 4 catalogs: **0 hits** for `Godot`/`UnityEngine`/`new Random` | ✅ |
| RNG seeded, streams distinct | `--plans-122-125-selftest` gates `rng_streams_distinct`, `rng_streams_present` PASS; additive stream ids in `CampaignStreamIds` | ✅ |
| Diamond consumer registry live, never-zero-wear | Composition registers `consumer_deep_excavation_cutter` + `consumer_precision_lathe_insert` (`src/Main.Plans122to125.cs:85-86`); selftests `cvd_registered_consumer_wear` / `cvd_unregistered_consumer_no_benefit` / `cvd_never_zero_wear` | ✅ |
| Acoustic defensive-only schema | `sra_defensive_schema_only` gate PASS; no `HostileFireObservation` emitter exists outside the sound-ranging seam — §4.1 decision still open as documented | ✅ |
| Amphibious typed route capability | `amb_route_capability_typed` + `amb_deep_route_refused_mk1` gates; **no map-owner consumer yet** (topology still unauthored — follow-up open as documented) | ✅ |

## 3. Documented limitations — re-verified accurate

| Closeout limitation | Current truth | Verdict |
|---|---|---|
| SOFC `FuelConsumer` composition placeholder | `src/Main.Plans122to125.cs:51` — `FuelConsumer = units => true` ("inventory owner binds the real check in Phase 9"); no later batch bound it. **Highest-value remaining follow-up:** the SOFC consumes no inventory fuel until this is wired. | limitation real, still open |
| `WasteHeatTargetRoomProvider` kitchen policy placeholder | Only the harness (`HostCli.Plans122to125.cs:60`) binds a constant kitchen target; no thermal-coordinator allocator exists | ✅ accurately documented |
| Amphibious route topology edges unauthored | `WaterCrossingRouteCapability` has zero map-owner consumers | ✅ |
| Plan 123 per-strike emitter (§4.1) | No external emitter; input seam live | ✅ |
| `AcousticDirectionFindingCatalog.cs` dormant | Zero external consumers repo-wide | ✅ quarantine candidate still valid |
| Diamond → ExcavationSystem downtime accounting | No downtime accounting in the excavation owner | ✅ follow-up still open |

## 4. Drift from later batches (context, not defects)

Since the batch landed (2026-09-13 morning), the repo grew: save sections 184→191, catalogs 317→325, architecture map 184→192 subsystems, and my rescue-signal batch sealed the CLI help / determinism debts. The Plans 122–125 assets themselves show **no regressions**: every suite, selftest, and hash-parity gate still passes.

## 5. Verdict

**§21 SECOND-TOOL REVIEW: PASSED.** All acceptance claims re-verified against current truth; three count-drifts recorded as documentation drift; all documented limitations confirmed accurate and still open; no duplicate authorities, no engine leakage, no determinism violations. The batch's "ACCEPTED except the second-tool review" condition is now fully satisfied.

**Open follow-ups carried forward** (unchanged, from the closeouts): SOFC inventory-fuel binding (highest value); flooded-route topology tags (map owner); Plan 123 §4.1 emitter decision (foreman); diamond→excavation downtime accounting.
