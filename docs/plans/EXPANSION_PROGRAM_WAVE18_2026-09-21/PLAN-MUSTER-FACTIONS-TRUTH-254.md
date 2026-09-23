# PLAN-MUSTER-FACTIONS-TRUTH-254 — Named Camp Factions: Barons, Guilds & Raiders

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MUSTER-COALITION-TRUTH-130, PLAN-WARLORDS-DIPLOMACY-29, PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-SECURITY (Plan 224).
**Non-goals:** no camp model (Plan 130), no diplomacy model (Plan 29), no
per-faction narrative rewrite — status and interaction only.

## 1. Outcome
Four reachable `Muster/` systems are unaddressed: `HydroBaronsSystem.cs`
(**138**), `ScavengerGuildSystem.cs` (**135**), `IronRaidersSystem.cs` (**111**),
`ProvisionedSystem.cs` (**112**) — the named factions of the muster camp. Plan
130 owns the camp/coalition model; each faction's **standing, demands, and
interaction rules** are unowned, so they are either set dressing or opaque.

| Deliverable | Detail |
|---|---|
| Faction model | per faction: standing with the holdfast, demands, and a stance state, reading Plan 29 relations |
| Interaction rules | trade (Plan 96), passage (Plan 151/30), and aid requests route through existing owners |
| Pressure model | unmet demands adjust standing per a documented rule; effects are visible before rupture |
| Raiders boundary | Iron Raiders' hostile actions route to Plan 61; this plan never resolves combat |
| Save truth | standings/demands restore; no re-roll on load |

## 2. Evidence
- The four files above (unaddressed — Wave 18 audit).
- Plan 130 documents camp state; this plan covers its factions — boundary stated.
- Plan 29 owns relations; Plan 96/151 trade; Plan 61 hostility.

## 3. Packages
- **MFT-254A** faction model + demand table.
- **MFT-254B** interaction routing tests per owner.
- **MFT-254C** pressure/standing rule fixtures.
- **MFT-254D** raider hostility hand-off to Plan 61.
- **MFT-254E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Demands and standings trace to their owners; hostility reaches Plan 61 only.
- Save/load preserves faction state.
- `bash scripts/run_test.sh` on the muster region.

## 5. Risks
Set dressing → demands and standings are mechanics with tests.
Combat duplication → raids stay in Plan 61 by explicit boundary.

---

## 6. Expanded census (4 files · 496 lines)

Scope: `Assets/Ashfall.Core/Muster/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `HydroBaronsSystem.cs` | 138 | System | **yes** | 0 | 0 | 2 |
| `IronRaidersSystem.cs` | 111 | System | **yes** | 0 | 0 | 2 |
| `ProvisionedSystem.cs` | 112 | System | **yes** | 0 | 0 | 2 |
| `ScavengerGuildSystem.cs` | 135 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `scavenger_expedition_route_notes.json` | array[8] |

**State surfaces:** `HydroBaronsSystem.cs`, `IronRaidersSystem.cs`, `ProvisionedSystem.cs`, `ScavengerGuildSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Muster/` (create if absent) |
| Test references | 8 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MUSTER-FAMILY-TRUTH-275` | 4 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MFT-254A` | no name match — resolve at claim time |
| `MFT-254B` | no name match — resolve at claim time |
| `MFT-254C` | no name match — resolve at claim time |
| `MFT-254D` | `IronRaidersSystem.cs` |
| `MFT-254E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/MusterHostSession.cs`, `src/Main.Muster.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Ashfall.Core.Tests/EventTriggerTests.cs`, `Ashfall.Core.Tests/FactionActionBoardTests.cs`, `Ashfall.Core.Tests/MusterCurrentSystemsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **0**; isolated: **4**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `factions` |
| `muster` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--muster-selftest` |
| `--muster-uitest` |

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

Catalog JSON files whose names share a domain token: **11**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/holdfast_factions.json` |
| `Assets/StreamingAssets/Data/muster_camp_scenes.json` |
| `Assets/StreamingAssets/Data/muster_epilogues.json` |
| `Assets/StreamingAssets/Data/muster_faction_actions.json` |
| `Assets/StreamingAssets/Data/muster_faction_culture.json` |
| `Assets/StreamingAssets/Data/muster_witnesses.json` |
| `Assets/StreamingAssets/Data/narrative/iron_gall_ink_acidity_reports.json` |
| `Assets/StreamingAssets/Data/narrative/iron_synod_canons.json` |
| `Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json` |
| `Assets/StreamingAssets/Data/standing_record_factions.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (10 files, 72 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Factions` | 10 | 72 |

**Verdict:** 72 cases sit under matching regions — run those first (`Factions`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **10**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/MusterHostSession.cs` |
| `src/Host/MusterSaveStore.cs` |
| `src/Main.Muster.cs` |
| `src/Main.UiTests.Muster.cs` |
| `src/UI/FactionsNarrativePanel.cs` |
| `src/UI/FactionsPanel.cs` |
| `src/UI/IronCenotaphMemorialPanel.cs` |
| `src/UI/MusterAtlasPanel.cs` |
| `src/UI/MusterPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `factions` | no |
| `muster` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `muster` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **11**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 5, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `muster_camp_scenes.json` | UNRESOLVED |
| `muster_epilogues.json` | GAMEPLAY_CONSUMED |
| `muster_faction_actions.json` | UNRESOLVED |
| `muster_faction_culture.json` | UNRESOLVED |
| `muster_witnesses.json` | GAMEPLAY_CONSUMED |
| `narrative/iron_gall_ink_acidity_reports.json` | CODEX_ONLY |
| `narrative/iron_synod_canons.json` | CODEX_ONLY |
| `narrative/scavenger_expedition_route_notes.json` | CODEX_ONLY |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_branch_iron_way_locked` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 12 · catalogs 21 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MUSTER-FACTIONS-TRUTH-254
wave: 18
status: PROPOSED — foreman claim required
packages: MFT-254A, MFT-254B, MFT-254C, MFT-254D, MFT-254E
claim paths:
  - src/Host/HydroGeologyArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/MusterHostSession.cs  # §19 candidate host surface
  - src/Host/MusterSaveStore.cs  # §19 candidate host surface
  - src/Main.Muster.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/crossing_factions.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/holdfast_factions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Factions/
  - godot --headless --path . -- --muster-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
