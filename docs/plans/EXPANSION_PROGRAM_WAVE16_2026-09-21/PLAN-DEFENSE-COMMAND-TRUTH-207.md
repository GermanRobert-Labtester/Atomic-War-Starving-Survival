# PLAN-DEFENSE-COMMAND-TRUTH-207 — Readiness, Postures & Command Decisions

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BASE-DEFENSE-RAIDS-61, PLAN-PERIMETER-DEFENSE-TRUTH-165, PLAN-DUTY-ROSTER-TRUTH-101, PLAN-NOISE-DISCIPLINE-TRUTH-116.
**Non-goals:** no raid resolution (Plan 61), no perimeter segment condition
(Plan 165), no roster (Plan 101).

## 1. Outcome
`Defense/DefenseSystem.cs` (**571 lines**) is reachable and unaddressed. Three
layers now exist on paper: Plan 165 owns perimeter condition, Plan 116 the
detection inputs, Plan 61 the raid resolution. Between them sits **command** —
readiness postures, who stands watch, and what a posture changes — with no
stated owner. Without it, the layers do not compose.

| Deliverable | Detail |
|---|---|
| Posture model | postures (stand-down, watch, alert, lockdown) with the conditions and costs of each |
| Readiness derivation | readiness from roster coverage (Plan 101), perimeter state (165), and detection (116); one owner per input |
| Command decisions | which decisions a posture permits (evacuate, muster, arm) with their owners |
| Transition rules | posture changes on documented triggers; no oscillation without a cooldown |
| Save truth | posture and readiness restore; a load never changes posture silently |

## 2. Evidence
- `Assets/Ashfall.Core/Defense/DefenseSystem.cs` (571 lines; unaddressed — Wave 16 audit).
- Plan 165 supplies perimeter/breach state; Plan 116 detection; Plan 101 coverage.
- Plan 61 consumes posture/readiness as raid inputs.
- Plan 80 owns emergency protocols — boundary: posture is military readiness, protocols are disaster response.

## 3. Packages
- **DCT-207A** posture model + cost table.
- **DCT-207B** readiness derivation tests (per input).
- **DCT-207C** command decision table + owner routing.
- **DCT-207D** transition/cooldown fixtures (no oscillation).
- **DCT-207E** save round-trip; no silent posture change.

## 4. Acceptance & verification
- Readiness equals its derived inputs; a scripted night shows the documented posture effect.
- Transitions fire once per trigger with cooldown; save/load preserves posture.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/`.

## 5. Risks
Layer duplication → command consumes, never re-derives, perimeter/detection state.
Oscillation → cooldown is a fixture.

---

## 6. Expanded census (3 files · 1,519 lines)

Scope: `Assets/Ashfall.Core/Defense/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DefenseSystem.cs` | 571 | System | **yes** | 0 | 0 | 2 |
| `PerimeterDefenseCatalog.cs` | 152 | Catalog | — | 0 | 0 | 0 |
| `PerimeterDefenseSystem.cs` | 796 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `defenses.json` | object[2 keys] |
| `sky_defense_ordnance.json` | object[2 keys] |
| `perimeter_defenses.json` | object[2 keys] |

**State surfaces:** `DefenseSystem.cs`, `PerimeterDefenseSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Defense/` |
| Test references | 23 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-BASE-DEFENSE-RAIDS-61` | 3 |
| `PLAN-PERIMETER-DEFENSE-TRUTH-165` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DCT-207A` | no name match — resolve at claim time |
| `DCT-207B` | no name match — resolve at claim time |
| `DCT-207C` | no name match — resolve at claim time |
| `DCT-207D` | no name match — resolve at claim time |
| `DCT-207E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **11** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/DefenseHostSession.cs`, `src/Host/HostCli.Plans162_165.cs`, `src/Main.AdvancedShelterSystems.cs`, `src/Main.Companion.cs`, `src/Main.Plans162_165.cs` |
| Tests (`Ashfall.Core.Tests/`) | 11 | `Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs`, `Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs`, `Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs`, `Ashfall.Core.Tests/DefensePersistenceTests.cs`, `Ashfall.Core.Tests/DefenseSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `DefenseSystem` | `PerimeterDefenseSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `perimeter_defense` |
| `sky_defense_battery` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--defense-selftest` |
| `--sky-defense-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/perimeter_defenses.json` |
| `Assets/StreamingAssets/Data/sky_defense_ordnance.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (5 files, 37 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Defense` | 4 | 36 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 37 cases sit under matching regions — run those first (`Defense`, `PlayerCommand`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **9**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/DefenseHostSession.cs` |
| `src/Host/DefenseSaveStore.cs` |
| `src/Host/HostCli.SkyDefense.cs` |
| `src/Host/PerimeterDefenseSaveStore.cs` |
| `src/Host/SkyDefenseBatterySaveStore.cs` |
| `src/Main.SkyDefense.cs` |
| `src/UI/ChemWarfareDefensePanel.cs` |
| `src/UI/DefenseGridPanel.cs` |
| `src/UI/SkyDefenseBatteryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `perimeter_defense` | no |
| `sky_defense_battery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `defense_capture` |
| `defense_damage` |
| `defense_targeting` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `perimeter_defenses.json` | GAMEPLAY_CONSUMED |
| `sky_defense_ordnance.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 2 (laddered 0) · RNG streams 3 · host files 12 · catalogs 4 · test regions 2 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DEFENSE-COMMAND-TRUTH-207
wave: 16
status: PROPOSED — foreman claim required
packages: DCT-207A, DCT-207B, DCT-207C, DCT-207D, DCT-207E
claim paths:
  - src/Host/DefenseHostSession.cs  # §19 candidate host surface
  - src/Host/DefenseSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.SkyDefense.cs  # §19 candidate host surface
  - src/Host/PerimeterDefenseSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/perimeter_defenses.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/sky_defense_ordnance.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Defense/
  - godot --headless --path . -- --defense-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
