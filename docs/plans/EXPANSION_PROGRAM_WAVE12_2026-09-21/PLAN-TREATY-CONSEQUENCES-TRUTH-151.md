# PLAN-TREATY-CONSEQUENCES-TRUTH-151 — Treaty Terms, Breach Detection & Consequence Rows

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WARLORDS-DIPLOMACY-29, PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122.
**Implementation scaffold:** [`PLAN-TREATY-CONSEQUENCES-TRUTH-151_APPENDIX-A_SCAFFOLD.md`](PLAN-TREATY-CONSEQUENCES-TRUTH-151_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-WARLORDS-DIPLOMACY-29` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no diplomacy model (Plan 29 owns standing/agreements), no contract
board (Plan 109), no obligation record (Plan 122).

## 1. Outcome
`Treaties/TreatyConsequences.cs` is a single file defining what happens when a
treaty is kept or broken. Plan 29 owns diplomacy; Plan 122 owns personal
obligations. A treaty is a **standing agreement between groups** whose
consequences must be enumerated and routed, or breach becomes an unexplained
standing drop.

| Deliverable | Detail |
|---|---|
| Treaty model | terms typed (passage, trade, defense, tribute) with the owner of each term's state |
| Breach detection | each term defines its breach condition evaluated from existing owners on the canonical clock |
| Consequence rows | per term: keep, breach, expiry outcomes, each routed to Plan 29's standing owner and Plan 96's ledger where value moves |
| Notice path | a breach produces a visible notice through Plan 138's feedback surface |
| Persistence | treaty state rides its existing owner's section; no new section without Plan 87 policy |

## 2. Evidence
- `Assets/Ashfall.Core/Treaties/TreatyConsequences.cs` (whole directory; verified).
- Plan 29 owns faction standing and agreements; this plan is the consequence table for them.
- Plan 96 owns value movement (tribute/trade) rows.
- Plan 122's obligations cover individuals; the boundary is stated.

## 3. Packages
- **TCT-151A** term table (type, state owner, breach condition).
- **TCT-151B** keep/breach/expiry outcome rows wired to owners.
- **TCT-151C** breach fixture per term type + notice path check.
- **TCT-151D** value-movement rows handed to Plan 96 (tribute/trade).
- **TCT-151E** persistence via the owner's section + reload test.

## 4. Acceptance & verification
- Every term maps to exactly one state owner; breach conditions are evaluable from stored state only.
- A breach produces the documented consequence once and a visible notice.
- Value movement reconciles in Plan 96's wrapper.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Treaties/` (create if absent).

## 5. Risks
Overlap with 29 → consequences only; standing math stays there.
Silent expiry → expiry is an outcome row with a notice, never a quiet removal.

---

## 6. Expanded census (1 files · 191 lines)

Scope: `Assets/Ashfall.Core/Treaties/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `TreatyConsequences.cs` | 191 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `foundry_treaty_consequences.json` | object[3 keys] |
| `treaty_templates.json` | object[2 keys] |
| `regional_treaty_protocols.json` | object[3 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Treaties/` (create if absent) |
| Test references | 1 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TCT-151A` | no name match — resolve at claim time |
| `TCT-151B` | no name match — resolve at claim time |
| `TCT-151C` | no name match — resolve at claim time |
| `TCT-151D` | no name match — resolve at claim time |
| `TCT-151E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **1** · Test files: **7** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Foundry/SilentFoundryHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/Diplomacy/Plan197FactionDiplomacyIntegrationTests.cs`, `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs`, `Ashfall.Core.Tests/RegionalTreatyCatalogTests.cs`, `Ashfall.Core.Tests/RegionalTreatyFeedTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **9** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `deep_well` |
| `faction_espionage` |
| `foundry` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `regional_treaty` |
| `silent_foundry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--narrative-selftest` |
| `--selftest-manifest` |
| `--silent-foundry-selftest` |
| `--silent-foundry-uitest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyQuotaMet` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **12** (117 files, 947 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Communication` | 3 | 17 |
| `Diplomacy` | 1 | 6 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Narrative` | 26 | 293 |

**Verdict:** 947 cases sit under matching regions — run those first (`Audio`, `Balance`, `Communication`, `Diplomacy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **182**
(21 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **39**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `bio_fermentation` | no |
| `black_market` | no |
| `black_projects_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **365**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 62, OPTIONAL 5, UNRESOLVED 19).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 19 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **6**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_faction` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 39 (laddered 0) · RNG streams 15 · host files 28 · catalogs 22 · test regions 10 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TREATY-CONSEQUENCES-TRUTH-151
wave: 12
status: PROPOSED — foreman claim required
packages: TCT-151A, TCT-151B, TCT-151C, TCT-151D, TCT-151E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/backstory_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --deep-coast-host-selftest
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
