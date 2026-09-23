# PLAN-MORAL-CHOICE-TRUTH-136 — Flaggable Choices, Chains, Faction Reactions & Gossip

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132, PLAN-NARRATIVE-GRAPH-18, PLAN-WARLORDS-DIPLOMACY-29, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Implementation scaffold:** [`PLAN-MORAL-CHOICE-TRUTH-136_APPENDIX-A_SCAFFOLD.md`](PLAN-MORAL-CHOICE-TRUTH-136_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new narrative store (Plan 18), no consequence-graph rules
(Plan 132), no prose authoring.

## 1. Outcome
`MoralChoice/` is a 16-file subsystem: catalogs for branch quests, chains,
expansion quests, faction reactions, flags, and gossip; plus `MoralChoiceState`,
`MoralChoiceSystem`, `MoralChoiceIds`, `MoralResolveResult`, and
`MoralChoiceGossipRuntime`. Nothing states how a choice resolves (result
semantics), how flags interact (precedence and mutual exclusion), how faction
reactions map, or how gossip carries a choice outward.

| Deliverable | Detail |
|---|---|
| Resolve semantics | `MoralResolveResult` fields documented; a choice's outcome is derived from state + catalog, not panel logic |
| Flag rules | precedence and mutual exclusion declared per flag family; contradictory combinations fail validation at load |
| Chain integrity | a chain's stages gate correctly; skipping is impossible without the documented bypass |
| Faction reactions | each reaction maps to the existing faction standing owner (Plan 29); no local faction score |
| Gossip runtime | gossip rows use Plan 132's consequence rules and Plan 120's rumor lifecycle where they propagate; expiry is day-based |
| Save truth | `MoralChoiceState` round-trips; loading never re-resolves a choice |

## 2. Evidence
- `Assets/Ashfall.Core/MoralChoice/` file list (16 files, verified); `MoralChoiceGossipRuntime.cs` is the runtime half of gossip.
- Plan 132 owns consequence-graph validation; this plan's flag rules are validator rules there.
- Plan 29 owns faction standing; reactions write through it.
- Save: moral-choice state rides an existing section or an explicit row per Plan 87 policy.

## 3. Packages
- **MCT-136A** resolve-semantics doc + per-field derivation table.
- **MCT-136B** flag precedence/exclusion rules as validator rules + fixtures.
- **MCT-136C** chain stage gating tests (including the documented bypass).
- **MCT-136D** faction reaction wiring tests against Plan 29's owner.
- **MCT-136E** gossip runtime determinism + day-based expiry + save round-trip.

## 4. Acceptance & verification
- Contradictory flag fixtures fail validation with both flags named.
- Chain stages cannot be skipped without the bypass; the bypass is explicit and tested.
- Loading after a choice does not change its resolve result.
- `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/`.

## 5. Risks
Flag sprawl → families and precedence are declared; unregistered flag use fails validation.
Gossip duplication → propagation routes through 120/132 owners; this plan stores only its own rows.

---

## 6. Expanded census (15 files · 2,160 lines)

Scope: `Assets/Ashfall.Core/MoralChoice/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 5 · Loader 7 · Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MoralChoiceBranchQuestCatalogLoader.cs` | 72 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceCatalogLoader.cs` | 125 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceChainCatalogLoader.cs` | 189 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceChainData.cs` | 78 | DTO/Type | — | 0 | 0 | 0 |
| `MoralChoiceExpansionQuestCatalogLoader.cs` | 71 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceFactionReactionsCatalogLoader.cs` | 96 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceFactionReactionsData.cs` | 34 | DTO/Type | — | 0 | 0 | 0 |
| `MoralChoiceFlagCatalogLoader.cs` | 60 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceFlagDefinitions.cs` | 21 | Support | — | 0 | 0 | 0 |
| `MoralChoiceGossipCatalogLoader.cs` | 145 | Loader | — | 0 | 0 | 0 |
| `MoralChoiceGossipData.cs` | 59 | DTO/Type | — | 0 | 0 | 0 |
| `MoralChoiceGossipRuntime.cs` | 185 | Support | — | 0 | 0 | 0 |
| `MoralChoiceIds.cs` | 260 | DTO/Type | — | 0 | 0 | 0 |
| `MoralChoiceState.cs` | 77 | DTO/Type | — | 0 | 0 | 0 |
| `MoralChoiceSystem.cs` | 688 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `MoralChoiceSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/MoralChoice/` |
| Test references | 79 name references across the test tree |
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

Domain files: 15. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276` | 15 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MCT-136A` | no name match — resolve at claim time |
| `MCT-136B` | no name match — resolve at claim time |
| `MCT-136C` | `MoralChoiceChainCatalogLoader.cs`, `MoralChoiceChainData.cs` |
| `MCT-136D` | `MoralChoiceFactionReactionsCatalogLoader.cs`, `MoralChoiceFactionReactionsData.cs` |
| `MCT-136E` | `MoralChoiceGossipRuntime.cs`, `MoralChoiceGossipCatalogLoader.cs`, `MoralChoiceGossipData.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 15; intra-domain edges: **11**; isolated files:
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

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MoralChoiceSystem` | 4 |
| `MoralChoiceChainData` | 2 |
| `MoralChoiceGossipData` | 2 |
| `MoralChoiceFactionReactionsData` | 1 |
| `MoralChoiceFlagDefinitions` | 1 |
| `MoralChoiceState` | 1 |
| `MoralChoiceBranchQuestCatalogLoader` | 0 |
| `MoralChoiceCatalogLoader` | 0 |
| `MoralChoiceChainCatalogLoader` | 0 |
| `MoralChoiceExpansionQuestCatalogLoader` | 0 |

**Class split:** hub 2 · sink 4 · source 7 · isolated 2.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 15. Host files: **8** · Test files: **37** · Data files: **1**.

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

Events whose name shares a domain token: **13**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnBranchDecided` | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFlagSet` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |

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
plan: PLAN-MORAL-CHOICE-TRUTH-136
wave: 11
status: PROPOSED — foreman claim required
packages: MCT-136A, MCT-136B, MCT-136C, MCT-136D, MCT-136E
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
