# PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132 — Consequence Graph Semantics & Static Validation

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-DATA-CONSUMER-22, PLAN-MORAL-CHOICE-TRUTH-136.
**Implementation scaffold:** [`PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132_APPENDIX-A_SCAFFOLD.md`](PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-NARRATIVE-FAMILY-TRUTH-261` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no second narrative store, no prose authoring, no runtime
generation; the graph stays data-driven.

## 1. Outcome
`NarrativeConsequence/` ships a graph, a simulator, and a validator —
`NarrativeConsequenceGraph.cs`, `NarrativeSimulator.cs`, `NarrativeValidator.cs`
— but no stated contract for what the graph guarantees: reachability of every
authored consequence, absence of dead ends, deterministic simulation order, and
what the validator refuses at load.

| Deliverable | Detail |
|---|---|
| Graph rules | node/edge semantics; a consequence with no trigger is reported, not silently inert |
| Reachability pass | every authored consequence is reachable from at least one documented entry, or explicitly marked as a trim/retirement |
| Dead-end policy | terminal nodes are allowed only where intended (endings); an unintended sink fails validation |
| Simulation determinism | the simulator's traversal order is stable; same seed + same graph → same trace |
| Validator coverage | every rule above is a validator rule with a per-row failure message |

## 2. Evidence
- `Assets/Ashfall.Core/NarrativeConsequence/`: `NarrativeConsequenceGraph.cs`, `NarrativeSimulator.cs`, `NarrativeValidator.cs` (verified).
- Plan 18 owns the flag/quest graph; this plan owns the consequence layer's guarantees.
- Plan 136 (MoralChoice) consumes consequences; the boundary is stated there.
- Plan 22 owns consumer gaps; reachability here is consequence-level, not catalog-level.

## 3. Packages
- **NCT-132A** graph rule document + validator mapping.
- **NCT-132B** reachability pass over authored data with a report.
- **NCT-132C** dead-end policy tests (intended ending passes; accidental sink fails).
- **NCT-132D** simulator determinism test (paired same-seed trace).
- **NCT-132E** per-rule failure message check (one bad fixture per rule).

## 4. Acceptance & verification
- Validator rejects one deliberately broken graph per rule with the node named.
- Reachability report covers all authored consequences; unreachable rows are decisions, not silence.
- Paired simulation traces identical.
- `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeConsequence/` (create if absent).

## 5. Risks
Validation becoming authoring → the plan reports; content changes route to owners.
Overlap with Plan 18 → this plan's rules concern consequences of flags, not flag storage.

---

## 6. Expanded census (3 files · 769 lines)

Scope: `Assets/Ashfall.Core/NarrativeConsequence/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `NarrativeConsequenceGraph.cs` | 139 | Support | **yes** | 0 | 0 | 0 |
| `NarrativeSimulator.cs` | 350 | Support | **yes** | 0 | 0 | 0 |
| `NarrativeValidator.cs` | 280 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `foundry_treaty_consequences.json` | object[3 keys] |
| `discovery_consequences.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/NarrativeConsequence/` |
| Test references | 3 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NCT-132A` | `NarrativeConsequenceGraph.cs` |
| `NCT-132B` | no name match — resolve at claim time |
| `NCT-132C` | no name match — resolve at claim time |
| `NCT-132D` | no name match — resolve at claim time |
| `NCT-132E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/NarrativeConsequence/NarrativeConsequenceSimulatorTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **2**; isolated: **0**.

| From | → To |
|---|---|
| `NarrativeSimulator` | `NarrativeConsequenceGraph` |
| `NarrativeValidator` | `NarrativeConsequenceGraph` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/narrative_arc_events.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/narrative_encounters.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/narrative_questlines.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (27 files, 313 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 313 cases sit under matching regions — run those first (`Narrative`, `NarrativeConsequence`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **13**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/NarrativeContinuitySelfTest.cs` |
| `src/Host/NarrativeHostSession.cs` |
| `src/Host/NarrativeQuestlineHostSession.cs` |
| `src/Host/NarrativeQuestlineSaveStore.cs` |
| `src/Host/NarrativeSaveStore.cs` |
| `src/Host/ProceduralNarrativeHostSession.cs` |
| `src/Host/ProceduralNarrativeSaveStore.cs` |
| `src/Main.Narrative.cs` |
| `src/Main.NarrativeQuestlines.cs` |
| `src/Main.SleepNarrative.cs` |
| `src/UI/FactionsNarrativePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `narrative` | no |
| `narrative_questlines` | no |
| `procedural_narrative` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **285**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 3, OPTIONAL 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |
| `narrative/ammo_hoist_jam_reports.json` | CODEX_ONLY |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/annealing_lehr_birefringence_records.json` | CODEX_ONLY |
| `narrative/antler_horn_sawing_records.json` | CODEX_ONLY |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/aramid_fiber_rot_reports.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/armored_locomotive_manifests.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 3 (laddered 0) · RNG streams 1 · host files 13 · catalogs 18 · test regions 2 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132
wave: 11
status: PROPOSED — foreman claim required
packages: NCT-132A, NCT-132B, NCT-132C, NCT-132D, NCT-132E
claim paths:
  - src/Host/NarrativeArcConsequenceAdapter.cs  # §19 candidate host surface
  - src/Host/NarrativeContinuitySelfTest.cs  # §19 candidate host surface
  - src/Host/NarrativeHostSession.cs  # §19 candidate host surface
  - src/Host/NarrativeQuestlineHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative_arc_events.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/
  - godot --headless --path . -- --narrative-selftest
dependencies:
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
