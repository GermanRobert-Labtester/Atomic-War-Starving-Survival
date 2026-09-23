# PLAN-NARRATIVE-GRAPH-18 — Flag Reachability, Quest DAG Integrity & Prose Hygiene

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (graph gate + pipeline) with Writers/
Builders per story family.
**Depends on:** PLAN-DATA-AUTHORITY-14 (catalog state), PLAN-UNBLOCK-03 U3
(string freeze), PLAN-VERTICAL-CULTURE-04 (new narrative surfaces).
**Expanded appendix:** [`PLAN-NARRATIVE-GRAPH-18_APPENDIX-A_FLAG_WORKLIST.md`](PLAN-NARRATIVE-GRAPH-18_APPENDIX-A_FLAG_WORKLIST.md)
— the complete **set-but-never-read flag work list (829 flags)** extracted from
the continuity artifact, each with the suggested NG-18A disposition (wire a
reader · bookkeep · delete after premise check).
**Non-goals:** no retcon of shipped prose, no real-world references, no new
narrative engine, no second journal/archive.

---

## 1. Outcome

The narrative corpus is large and structurally sound (0 hard errors) but
functionally wasteful: the current continuity report finds **832 flags set and
only 8 flags read** in the scanned graph set, with **882 warnings**. That means
most authored consequences can never fire because nothing reads the flag they
set — a silent-content gap exactly like an orphan system, one layer up.

Deliverables:

1. **flag reachability reconciliation** — every flag classified: consumed,
   bookkeeping (with a named consumer), or removed;
2. a **CI continuity gate** with hard-error = 0 and a warning budget;
3. **quest DAG validation** (acyclic, terminal reachable, choices resolve,
   grants exist);
4. an **authoring pipeline** that makes new content land readable and
   verifiable;
5. **prose hygiene**: duplication/near-duplication detection, variation rules,
   and a text-volume budget per surface;
6. a **localization-ready extraction hook** for the U3 freeze.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Narrative files scanned | 21 | `artifacts/narrative-continuity.md` |
| Graphs / nodes / edges | 21 / 1,303 / 759 | same |
| Flags **set** | 832 | same |
| Flags **read** | 8 | same |
| Hard errors / warnings | 0 / 882 | same |
| Set-but-never-read examples | `flag_crossing_kael_asylum_granted`, `flag_crossing_kael_extradited`, `flag_crossing_mattis_redeemed`, `flag_crossing_medicine_confiscated` | same |
| Quest catalogs | 27 | `ls Assets/StreamingAssets/Data \| grep -c quest` |
| Encounter/echo/radio catalogs | 7 / 1 / 10 | same |
| Largest narrative catalogs | `moral_choice_quests_branching.json` 340 KB, `quests_massive_expansion_200.json` 223 KB, `quests_faction_branching.json` 194 KB | size listing |
| Prose catalogs | `item_description_texts.json` 273 KB, `medical_texts.json` 222 KB, `events.json` 241 KB | same |
| Continuity tooling | audit artifact + skill (`ashfall-narrative-continuity`) | artifacts + `.agents/skills` |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Story runtime | `QuestlineSystem`, `NarrativeQuestlineSystem`, `MoralChoice*`, `HoldfastQuestSystem`, `PersonalQuestSystem`, `DynamicQuest*` |
| Flags | the existing flag store (`FlagSystem`/campaign flags) and moral-choice flag catalogs |
| Journal/feedback | `JournalSystem` (single feedback strip), `Codex` catalogs |
| Continuity audit | the existing narrative-continuity script/skill output |
| Data | catalogs under `Data/`, `Data/narrative/`, `Data/documents/` |
| Extraction | `scripts/ci/extract_l10n_inventory.py` (U3) |

---

## 4. Packages

### NG-18A — Flag reachability reconciliation
- Extend the continuity auditor to distinguish: `read` (a consumer exists in
  source or a catalog condition), `bookkeeping` (set for telemetry/history with
  a named reader), `unread`.
- Sweep the unread set: for each flag, choose (a) wire a reader (a quest gate,
  gossip line, ending condition, radio mention), (b) reclassify as bookkeeping
  with a named archive consumer, (c) delete the setter and its prose.
- **Acceptance:** unread flags reduced from 820 to 0 with a per-flag verdict;
  no prose orphaned (a deleted setter removes or rewrites its line).
- **Verify:** `python3 scripts/ci/narrative-continuity.py --check`
  (productionized from the artifact).

### NG-18B — Continuity gate in CI
- Add the auditor to the fast tier: hard errors = 0 (fails), warnings ≤ budget
  (starts at the post-sweep number, ratchets down).
- **Acceptance:** a dangling `triggered_by`, unreachable node, or unread flag
  fails the gate with the exact file and node; the budget is committed and
  monotonic.
- **Verify:** `python3 scripts/ci/narrative-continuity.py --check`;
  `agent-fast-verify.py`.

### NG-18C — Quest DAG validation
- Validate every questline catalog: acyclic (topological check), terminal
  stages reachable, every choice's outcome resolves to a real node, every item
  reward/objective references an existing item id, every location references
  an existing location id, day windows ordered (`min_days_after ≤
  max_days`).
- **Acceptance:** zero hard validation failures across the 27 quest catalogs;
  the validator runs as a focused gate, aggregated per-row.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`;
  extended `--data-integrity-selftest`.

### NG-18D — Authoring pipeline and volumes
- Document the path: outline → node graph → JSON → continuity/validation →
  consumer binding → registry row → journal/echo/radio surfaces. Include the
  tone rules (fictional, restrained, human; no real countries/people/art).
- Volume targets for the culture vertical: +6 questlines, +12 echoes, +10 radio
  segments, +8 documents, all with at least one reader per flag.
- **Acceptance:** new content passes all gates; every flag introduced has a
  reader in the same package.
- **Verify:** continuity + catalog + focused narrative suites.

### NG-18E — Prose hygiene
- Near-duplicate detection across text catalogs (shingle similarity), repeated
  sentence detection, placeholder detection (`TODO`, `lorem`, `xxx`), and a
  budget per surface (e.g. ≤ N characters per event description).
- **Acceptance:** report committed; duplicates either varied or justified;
  placeholder count 0; no copied external text (authoring rule).
- **Verify:** `python3 scripts/ci/prose-hygiene.py --check` (new) +
  `extract_l10n_inventory.py`.

### NG-18F — Extraction hook
- Ensure all narrative strings are reachable by the l10n extractor (no string
  built by concatenation, no format holes without a resource, no embedded
  punctuation that breaks extraction).
- **Acceptance:** the U3 inventory includes narrative catalogs; no hardcoded
  runtime prose outside catalogs.
- **Verify:** `python3 scripts/ci/extract_l10n_inventory.py --check`.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Wiring 800+ flags becomes a content avalanche | batch by graph (21 files), one package per family; bookkeeping reclassification is cheap and honest |
| Gate blocks in-flight writing | warning budget starts at current value; hard errors only for structural faults |
| Deleting flags removes intended sequels | default is wire-or-bookkeep; deletion requires the authoring owner to confirm |
| Prose budgets encourage terse bad writing | budgets are per-surface ranges, not minimums; quality review by the narrative owner |
| Duplicate detection false positives on deliberate motifs | waiver list with reasons |

## 6. Verification summary

```bash
python3 scripts/ci/narrative-continuity.py --check
python3 scripts/ci/prose-hygiene.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/
godot --headless --path . -- --data-integrity-selftest
python3 scripts/ci/extract_l10n_inventory.py --check
```

## 7. Change control

Narrative catalogs are authored authority. No runtime system writes them. Flags
are campaign state and follow the save governance. Every content package names
its flag readers and its journal/radio/echo surfaces before merge.

---

## 6. Expanded census (11 files · 5,089 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Demo 1 · Loader 1 · Support 2 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CipherQuestChainEngine.cs` | 192 | System | — | 0 | 0 | 2 |
| `NarrativeContinuityAllowlist.cs` | 63 | Support | — | 0 | 0 | 0 |
| `NarrativeContinuityEngine.cs` | 671 | System | — | 0 | 0 | 0 |
| `NarrativeContinuityModel.cs` | 185 | Support | — | 0 | 0 | 0 |
| `NarrativeArcEventSystem.cs` | 1200 | System | — | 0 | 0 | 2 |
| `NarrativeBatchCatalog.cs` | 150 | Catalog | — | 0 | 0 | 0 |
| `NarrativeDiscoveryCatalog.cs` | 1552 | Catalog | — | 0 | 0 | 0 |
| `NarrativeEncounterSystem.cs` | 560 | System | — | 0 | 0 | 5 |
| `NarrativeHeadlessDemo.cs` | 93 | Demo | — | 0 | 0 | 5 |
| `NarrativeProgressionCatalogLoader.cs` | 58 | Loader | — | 0 | 0 | 0 |
| `ProceduralNarrativeSystem.cs` | 365 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `camouflage_gear.json` | object[2 keys] |
| `dose_quests.json` | object[2 keys] |
| `duty_roster_quests.json` | object[2 keys] |
| `dynamic_questlines.json` | object[2 keys] |
| `holdfast_quests.json` | object[2 keys] |
| `narrative_encounters_expansion.json` | object[2 keys] |

**State surfaces:** `CipherQuestChainEngine.cs`, `NarrativeArcEventSystem.cs`, `NarrativeEncounterSystem.cs`, `NarrativeHeadlessDemo.cs`, `ProceduralNarrativeSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 42 name references across the test tree |
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

The domain set is the plan's own `.cs` enumeration (11 files).
Other plans referencing those names: **8**.

**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 6 |
| `PLAN-NARRATIVE-CONTINUITY-TRUTH-170` | 3 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |
| `PLAN-BACKSTORY-REVEAL-TRUTH-126` | 1 |
| `PLAN-NARRATIVE-ARC-EVENT-TRUTH-176` | 1 |
| `PLAN-NARRATIVE-ENCOUNTER-TRUTH-185` | 1 |
| `PLAN-PROCEDURAL-NARRATIVE-TRUTH-216` | 1 |
| `PLAN-CIPHER-CHAIN-TRUTH-251` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NG-18A` | no name match — resolve at claim time |
| `NG-18B` | `NarrativeContinuityAllowlist.cs`, `NarrativeContinuityEngine.cs`, `NarrativeContinuityModel.cs` |
| `NG-18C` | `CipherQuestChainEngine.cs` |
| `NG-18D` | no name match — resolve at claim time |
| `NG-18E` | no name match — resolve at claim time |
| `NG-18F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 11; intra-domain edges: **2**; isolated files:
**7**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `NarrativeContinuityEngine` | `NarrativeContinuityAllowlist` |
| `NarrativeHeadlessDemo` | `NarrativeEncounterSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `NarrativeContinuityAllowlist` | 1 |
| `NarrativeEncounterSystem` | 1 |
| `CipherQuestChainEngine` | 0 |
| `NarrativeArcEventSystem` | 0 |
| `NarrativeBatchCatalog` | 0 |
| `NarrativeContinuityEngine` | 0 |
| `NarrativeContinuityModel` | 0 |
| `NarrativeDiscoveryCatalog` | 0 |
| `NarrativeHeadlessDemo` | 0 |
| `NarrativeProgressionCatalogLoader` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 7.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 11. Host files: **12** · Test files: **42** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 12 | `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Host/HostCli.PanelTests.cs` |
| Tests (`Ashfall.Core.Tests/`) | 42 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/BoneHornRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs`, `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `encounter_choice` |
| `expansion_quest` |
| `host_event` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-encounter-bridge-selftest` |
| `--ice-road-tick-demo` |
| `--narrative-selftest` |
| `--patrol-encounter-selftest` |
| `--personal-quest-selftest` |
| `--travel-encounter-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **25**.

| Event | First declaration |
|---|---|
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterSelected` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterTriggered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnExtractionBatchProduced` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_children_folklore_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_wiretap_transcripts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/chemist_lab_notes_batch_1.json` |
| `Assets/StreamingAssets/Data/narrative/childrens_artwork_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/cobalt_liturgies_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/courier_mission_logs_batch_2.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (38 files, 396 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Progression` | 11 | 83 |

**Verdict:** 396 cases sit under matching regions — run those first (`Narrative`, `NarrativeConsequence`, `Progression`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **37**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostEventAdapter.cs` |
| `src/Host/HostEventSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
| `encounter_choice` | no |
| `expansion_quest` | no |
| `host_event` | no |
| `narrative` | no |
| `narrative_questlines` | no |
| `procedural_narrative` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `black_market_debt_event` |
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **286**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 3, OPTIONAL 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `moral_choice_quest_stubs.json` | OPTIONAL |
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |
| `narrative/ammo_hoist_jam_reports.json` | CODEX_ONLY |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/annealing_lehr_birefringence_records.json` | CODEX_ONLY |
| `narrative/antler_horn_sawing_records.json` | CODEX_ONLY |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/aramid_fiber_rot_reports.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |

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
**Surface:** save sections 7 (laddered 0) · RNG streams 2 · host files 14 · catalogs 22 · test regions 3 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NARRATIVE-GRAPH-18
wave: —
status: PROPOSED — foreman claim required
packages: NG-18A, NG-18B, NG-18C, NG-18D, NG-18E, NG-18F
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/discovery_consequences.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/dynamic_quest_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/
  - godot --headless --path . -- --expedition-encounter-bridge-selftest
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
