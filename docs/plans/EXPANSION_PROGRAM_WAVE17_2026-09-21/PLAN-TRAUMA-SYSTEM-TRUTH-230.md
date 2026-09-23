# PLAN-TRAUMA-SYSTEM-TRUTH-230 — Somatic Flashbacks & Combat Trauma

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-COMBAT-DEPTH-62, PLAN-SANATORIUM-TRUTH-144, PLAN-PSYCHOLOGICAL-ARC-TRUTH-186.
**Non-goals:** no combat resolver (Plan 62), no clinical model (Plan 64), no
arc stages (Plan 186).

## 1. Outcome
Two reachable systems are unaddressed: `SomaticFlashbackSystem.cs` (**298**)
and `CombatTraumaSystem.cs` (**255**). Trauma exposure and its flashback
consequences are the bridge between combat (Plan 62) and the psychological
model (Plan 64) — and the exact place a hidden debuff appears if unstated.

| Deliverable | Detail |
|---|---|
| Exposure model | trauma accrues from documented events (casualties witnessed, personal injury, near-death) with bands |
| Flashback rule | flashback triggers derive from state + context; frequency is bounded and reduced by care (Plan 144) |
| Effect routing | effects write through Plan 64/186 owners; no private trauma score outside the model |
| Recovery | documented recovery paths per band; therapy (Plan 144) is the strong path |
| Save truth | exposure and pending triggers restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` (298) and `Survivors/CombatTraumaSystem.cs` (255) — both unaddressed (Wave 17 audit).
- Plan 62's outcomes are the exposure source; Plan 64 the value owner.
- Plan 186's arcs may consume bands; Plan 144 treats.
- Plan 138 carries notices where visible.

## 3. Packages
- **TST-230A** exposure table + band tests.
- **TST-230B** flashback trigger/cadence fixtures.
- **TST-230C** effect routing to Plan 64/186 (no private score).
- **TST-230D** recovery paths + therapy strength.
- **TST-230E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Exposure traces to combat events; effects appear in named owners.
- Frequency bounds hold; therapy reduces them per rule.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Hidden debuff → bands and routes are visible and tested.
Overlap with 64/186 → exposure feeds them; values stay there.

---

## 6. Expanded census (3 files · 857 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CombatTraumaSystem.cs` | 255 | System | **yes** | 0 | 0 | 3 |
| `SomaticFlashbackSystem.cs` | 298 | System | **yes** | 0 | 0 | 3 |
| `TraumaBondSystem.cs` | 304 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `psychological_trauma.json` | object[4 keys] |

**State surfaces:** `CombatTraumaSystem.cs`, `SomaticFlashbackSystem.cs`, `TraumaBondSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 11 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 3. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-MENTAL-HEALTH-THERAPY-64` | 1 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TST-230A` | no name match — resolve at claim time |
| `TST-230B` | `SomaticFlashbackSystem.cs` |
| `TST-230C` | no name match — resolve at claim time |
| `TST-230D` | no name match — resolve at claim time |
| `TST-230E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **6** · Test files: **8** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/Phase0HostSession.cs`, `src/Main.Audio.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/CombatTraumaSystemTests.cs`, `Ashfall.Core.Tests/GapTestCoverageTests.cs`, `Ashfall.Core.Tests/InstitutionCanonicalReliefTests.cs`, `Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs`, `Ashfall.Core.Tests/Shelter/Plan12_27SocialAutopsyIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `combat` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--combat-breaching-selftest` |
| `--combat-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnCaregivingBondDeepened` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnFlashbackEnded` | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` |
| `OnFlashbackGrounded` | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` |
| `OnFlashbackSuppressed` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnFlashbackTriggered` | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` |
| `OnTraumaBondDecayed` | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` |
| `OnTraumaBondFormed` | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/psychological_trauma.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (10 files, 84 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |

**Verdict:** 84 cases sit under matching regions — run those first (`Combat`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/UI/CombatDetailPanel.cs` |
| `src/UI/CombatHistoryPanel.cs` |
| `src/UI/CombatHudOverlay.cs` |
| `src/UI/CombatPanel.cs` |
| `src/UI/TraumaBondingCohortPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `combat` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `combat` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `combat_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 1 (laddered 0) · RNG streams 1 · host files 8 · catalogs 4 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TRAUMA-SYSTEM-TRUTH-230
wave: 17
status: PROPOSED — foreman claim required
packages: TST-230A, TST-230B, TST-230C, TST-230D, TST-230E
claim paths:
  - src/Host/CombatHostSession.cs  # §19 candidate host surface
  - src/Host/CombatSaveStore.cs  # §19 candidate host surface
  - src/UI/CombatDetailPanel.cs  # §19 candidate host surface
  - src/UI/CombatHistoryPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/combat_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --combat-breaching-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
