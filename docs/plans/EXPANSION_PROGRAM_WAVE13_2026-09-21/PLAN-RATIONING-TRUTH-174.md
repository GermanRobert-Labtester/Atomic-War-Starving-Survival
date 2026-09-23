# PLAN-RATIONING-TRUTH-174 — Allocation Policy, Fairness & Pressure Responses

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FOOD-CUISINE-39, PLAN-MORALE-UNREST-TRUTH-129, PLAN-SHELTER-POLITICS-69, PLAN-INVENTORY-CONSERVATION-93.
**Implementation scaffold:** [`PLAN-RATIONING-TRUTH-174_APPENDIX-A_SCAFFOLD.md`](PLAN-RATIONING-TRUTH-174_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-FOOD-CUISINE-39` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no food catalog (Plan 39), no morale model (Plan 129), no policy
system (Plan 69).

## 1. Outcome
`Economy/ResourceRationingSystem.cs` (631 lines) is reachable and unaddressed.
When supplies tighten, someone decides who eats less; Plan 39 owns food, Plan 93
conservation, Plans 129/69 the collective response. The **allocation rule**
itself — tiers, exceptions, fairness measurement, and escalation — is unowned,
so rationing is either an invisible auto-balance or a hidden punishment.

| Deliverable | Detail |
|---|---|
| Tier model | allocation tiers (full, reduced, minimal, excluded) with the conditions that assign each and the exemptions (children, injured, workers) |
| Fairness truth | a computed fairness index from the actual distribution — displayed, not scored against a hidden target |
| Pressure response | shortage escalates through documented steps; each step is a decision the player can make or accept |
| Conservation | distributions consume inventory through Plan 93's wrapper; no private pile |
| Save truth | tier assignment and escalation state restore; a load never re-deals a day's rations |

## 2. Evidence
- `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` (631 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 93's wrapper verifies distributions.
- Plan 129 receives the collective consequence; Plan 69 is where the policy is chosen.
- Plan 39 owns the food items distributed.

## 3. Packages
- **RAT-174A** tier model + assignment/exemption table.
- **RAT-174B** fairness index computation + display-from-index test.
- **RAT-174C** escalation steps + one fixture per step.
- **RAT-174D** conservation check across a scripted shortage week.
- **RAT-174E** save round-trip; no re-deal on load.

## 4. Acceptance & verification
- Tier assignment matches its conditions; exemptions apply exactly where listed.
- Fairness index equals the actual distribution in the fixture; escalating shortage follows the steps.
- Inventory balances; a reload does not redistribute.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`.

## 5. Risks
Hidden punishment → every tier and exemption is visible and tested.
Overlap with 69 → this plan computes options; the policy choice surface stays there.

---

## 6. Expanded census (3 files · 937 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MigrationConsequenceEngine.cs` | 144 | System | — | 0 | 0 | 2 |
| `ResourceRationingSystem.cs` | 631 | System | **yes** | 0 | 0 | 2 |
| `SeasonalHumanMigrationEngine.cs` | 162 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `desperation_events.json` | object[2 keys] |
| `cryogenic_air_separation.json` | object[3 keys] |
| `electrostatic_filtration_catalog.json` | object[2 keys] |
| `gpr_exploration_catalog.json` | object[5 keys] |
| `espionage_operations.json` | object[3 keys] |
| `shelter_celebrations.json` | object[4 keys] |

**State surfaces:** `MigrationConsequenceEngine.cs`, `ResourceRationingSystem.cs`, `SeasonalHumanMigrationEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 5 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **8**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-NOMADS-CARAVAN-CULTURE-82` | 2 |
| `PLAN-ASYLUM-REFUGEES-85` | 2 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RAT-174A` | no name match — resolve at claim time |
| `RAT-174B` | no name match — resolve at claim time |
| `RAT-174C` | no name match — resolve at claim time |
| `RAT-174D` | no name match — resolve at claim time |
| `RAT-174E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/EconomyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Economy/MigrationConsequenceEngineTests.cs`, `Ashfall.Core.Tests/Economy/Plan215ResourceRationingIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/ResourceRationingSystemTests.cs`, `Ashfall.Core.Tests/Economy/SeasonalHumanMigrationEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `MigrationConsequenceEngine` | `SeasonalHumanMigrationEngine` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/rationing_protocols.json` |
| `Assets/StreamingAssets/Data/seasonal_events.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 20 cases).

| Region | Files | Cases |
|---|---:|---:|
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 20 cases sit under matching regions — run those first (`NarrativeConsequence`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **2**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/UI/BasalRadonMigrationPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `wildlife_migration` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `seasonal_events.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 0 (laddered 0) · RNG streams 1 · host files 3 · catalogs 3 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RATIONING-TRUTH-174
wave: 13
status: PROPOSED — foreman claim required
packages: RAT-174A, RAT-174B, RAT-174C, RAT-174D, RAT-174E
claim paths:
  - src/Host/NarrativeArcConsequenceAdapter.cs  # §19 candidate host surface
  - src/UI/BasalRadonMigrationPanel.cs  # §19 candidate host surface
  - wildlife_migration  # §19 candidate host surface
  - Assets/StreamingAssets/Data/rationing_protocols.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/seasonal_events.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeConsequence/
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
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
