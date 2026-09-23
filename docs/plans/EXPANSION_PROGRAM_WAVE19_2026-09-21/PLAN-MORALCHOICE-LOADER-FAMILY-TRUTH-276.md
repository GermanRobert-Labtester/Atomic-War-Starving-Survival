# PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276 — Loader & Data Integrity for Choices

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MORAL-CHOICE-TRUTH-136, PLAN-DATA-SCHEMA-COVERAGE-90, PLAN-REFERENCE-INTEGRITY-34.
**Non-goals:** no choice semantics change (Plan 136 owns them); this is the
loader/data layer beneath it.

## 1. Outcome
**11 `MoralChoice/` files** are referenced by no plan — all loaders and data
types (`MoralChoiceBranchQuestCatalogLoader`, `…ChainCatalogLoader`,
`…ExpansionQuestCatalogLoader`, `…FactionReactionsCatalogLoader`,
`…FlagCatalogLoader`, `…GossipCatalogLoader`, `MoralChoiceChainData`,
`MoralChoiceFactionReactionsData`, plus ids). Plan 136 owns semantics; the
loaders must reject malformed data **before** semantics can misbehave.

| Deliverable | Detail |
|---|---|
| Loader coverage | every loader has a valid/invalid fixture pair |
| Flag/chain data | ids resolve; unknown references fail typed |
| Faction reactions | data maps to Plan 29's standing ids (Plan 34 families) |
| Gossip data | gossip rows reference real subjects and expire per Plan 136's rule |
| Schema tie-in | any loader gaining a schema uses Plan 90's stage |

## 2. Evidence
- 11 `MoralChoice/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 136 names the semantics; this plan is its data gate.
- Plan 34's id families cover chain/flag references.

## 3. Packages
- **MLF-276A** loader fixture pairs (valid/invalid per loader).
- **MLF-276B** id resolution tests.
- **MLF-276C** faction reaction mapping tests.
- **MLF-276D** gossip subject/expiry data tests.

## 4. Acceptance & verification
- Every loader rejects its invalid fixture with the field named; ids resolve.
- `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/`.

## 5. Risks
Semantics duplication → Plan 136 owns them; this plan is data-only.
Silent defaults → invalid fixtures require typed failures.

---

## 6. Expanded census (16 family files · 2,211 lines)

Scope: files under `Assets/Ashfall.Core/MoralChoice/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Loader 7 · DTO/Type 5 · Support 3 · System 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `MoralChoiceBranchQuestCatalogLoader.cs` | 72 | Loader | 0 | 0 | 0 |
| `MoralChoiceCatalogLoader.cs` | 125 | Loader | 0 | 0 | 0 |
| `MoralChoiceChainCatalogLoader.cs` | 189 | Loader | 0 | 0 | 0 |
| `MoralChoiceChainData.cs` | 78 | DTO/Type | 0 | 0 | 0 |
| `MoralChoiceExpansionQuestCatalogLoader.cs` | 71 | Loader | 0 | 0 | 0 |
| `MoralChoiceFactionReactionsCatalogLoader.cs` | 96 | Loader | 0 | 0 | 0 |
| `MoralChoiceFactionReactionsData.cs` | 34 | DTO/Type | 0 | 0 | 0 |
| `MoralChoiceFlagCatalogLoader.cs` | 60 | Loader | 0 | 0 | 0 |
| `MoralChoiceFlagDefinitions.cs` | 21 | Support | 0 | 0 | 0 |
| `MoralChoiceGossipCatalogLoader.cs` | 145 | Loader | 0 | 0 | 0 |
| `MoralChoiceGossipData.cs` | 59 | DTO/Type | 0 | 0 | 0 |
| `MoralChoiceGossipRuntime.cs` | 185 | Support | 0 | 0 | 0 |
| `MoralChoiceIds.cs` | 260 | DTO/Type | 0 | 0 | 0 |
| `MoralChoiceState.cs` | 77 | DTO/Type | 0 | 0 | 0 |
| `MoralChoiceSystem.cs` | 688 | System | 0 | 0 | 2 |
| `MoralResolveResult.cs` | 51 | Support | 0 | 0 | 0 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 1 files with capture/restore methods.

## 7. Expanded data & state surface

No catalog in this family's name space; the family is code/support, so no data binding is implied.

**State surfaces (capture/restore present):**

- `MoralChoiceSystem.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/MoralChoice/` |
| Family files referenced by tests | 80 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(16 files). Other plans referencing those names: **2**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-MORAL-CHOICE-TRUTH-136` | 16 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MLF-276A` | `MoralChoiceBranchQuestCatalogLoader.cs`, `MoralChoiceCatalogLoader.cs`, `MoralChoiceChainCatalogLoader.cs` |
| `MLF-276B` | no name match — resolve at claim time |
| `MLF-276C` | `MoralChoiceFactionReactionsCatalogLoader.cs`, `MoralChoiceFactionReactionsData.cs` |
| `MLF-276D` | `MoralChoiceGossipCatalogLoader.cs`, `MoralChoiceGossipData.cs`, `MoralChoiceGossipRuntime.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 16; intra-domain edges: **12**; isolated files:
**2**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `MoralChoiceCatalogLoader` | `MoralChoiceSystem` |
| `MoralChoiceChainCatalogLoader` | `MoralChoiceChainData` |
| `MoralChoiceFactionReactionsCatalogLoader` | `MoralChoiceFactionReactionsData` |
| `MoralChoiceFlagCatalogLoader` | `MoralChoiceFlagDefinitions` |
| `MoralChoiceGossipCatalogLoader` | `MoralChoiceGossipData` |
| `MoralChoiceGossipRuntime` | `MoralChoiceGossipData` |
| `MoralChoiceGossipRuntime` | `MoralChoiceSystem` |
| `MoralChoiceIds` | `MoralChoiceSystem` |
| `MoralChoiceState` | `MoralChoiceSystem` |
| `MoralChoiceSystem` | `MoralChoiceChainData` |
| `MoralChoiceSystem` | `MoralChoiceState` |
| `MoralChoiceSystem` | `MoralResolveResult` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MoralChoiceSystem` | 4 |
| `MoralChoiceChainData` | 2 |
| `MoralChoiceGossipData` | 2 |
| `MoralChoiceFactionReactionsData` | 1 |
| `MoralChoiceFlagDefinitions` | 1 |
| `MoralChoiceState` | 1 |
| `MoralResolveResult` | 1 |
| `MoralChoiceBranchQuestCatalogLoader` | 0 |
| `MoralChoiceCatalogLoader` | 0 |
| `MoralChoiceChainCatalogLoader` | 0 |

**Class split:** hub 2 · sink 5 · source 7 · isolated 2.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 16. Host files: **8** · Test files: **37** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.MoralChoice.cs`, `src/Host/MoralChoiceSaveStore.cs`, `src/Main.MoralChoice.cs`, `src/UI/FactionsPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 37 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs`, `Ashfall.Core.Tests/CrossingThirdonaryIntegrationTests.cs`, `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/whitelists/plan25_flags.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `encounter_choice` |
| `expansion_quest` |
| `faction_espionage` |
| `moral_choice` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--moral-choice-selftest` |
| `--personal-quest-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnBranchDecided` | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFlagSet` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (4 files, 33 cases).

| Region | Files | Cases |
|---|---:|---:|
| `MoralChoice` | 4 | 33 |

**Verdict:** 33 cases sit under matching regions — run those first (`MoralChoice`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **28**
(10 of them panels/HUD).

| Host file |
|---|
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/HostCli.MoralChoice.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/MoralChoiceSaveStore.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `encounter_choice` | no |
| `expansion_quest` | no |
| `faction_espionage` | no |
| `moral_choice` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `moral_choice` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **30**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 19, OPTIONAL 1, UNRESOLVED 6).

| Catalog | Classification |
|---|---|
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |

**Verdict:** 6 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **25**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_ally` |
| `flag_betrayed_faction` |
| `flag_betrayed_trust` |
| `flag_branch_broken_compact_locked` |
| `flag_branch_iron_way_locked` |
| `flag_branch_listener_locked` |
| `flag_branch_mercy_road_locked` |
| `flag_broke_treaty` |
| `flag_broken_pact` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 4 (laddered 0) · RNG streams 1 · host files 23 · catalogs 22 · test regions 1 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276
wave: 19
status: PROPOSED — foreman claim required
packages: MLF-276A, MLF-276B, MLF-276C, MLF-276D
claim paths:
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/EncounterChoiceSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestHostSession.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/dynamic_quest_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/
  - godot --headless --path . -- --faction-communique-board-selftest
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
