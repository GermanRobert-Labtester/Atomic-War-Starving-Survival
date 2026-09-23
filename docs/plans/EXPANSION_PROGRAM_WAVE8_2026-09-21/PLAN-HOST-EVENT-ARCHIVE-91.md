# PLAN-HOST-EVENT-ARCHIVE-91 — Durable Bounded Fact-Event History per Save

**Wave 8 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TELEMETRY-PRIVACY-58, PLAN-SILENT-FAILURE-35, PLAN-OBSERVABILITY.
**Implementation scaffold:** [`PLAN-HOST-EVENT-ARCHIVE-91_APPENDIX-A_SCAFFOLD.md`](PLAN-HOST-EVENT-ARCHIVE-91_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-TELEMETRY-PRIVACY-58` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no gameplay authority, no replay-from-archive, no network
upload, no PII, no infinite log.

## 1. Outcome
The Core already exposes facts through its event seams, the journal records
diegetic entries, and `Telemetry/PlaySessionRecorder` writes local JSONL
aggregates (bounded 2000 events, zero network, Plan 58 governs consent). The
gap is a **per-save structured archive of fact events** for post-mortem: what
happened, on which day/hour, to which subject — enough to answer "why did the
colony die on day 12" without reproducing the run.

| Deliverable | Detail |
|---|---|
| Event taxonomy | the graduated fact list (day boundary, death, crisis trigger, migration, trade failure, save/load, mod change) with stable ids |
| Ring writer | bounded file beside the save slot; deterministic order; per-save, not global |
| Replay independence | a guard test proving the simulation never reads the archive; archive absence changes nothing |
| Dump surface | one CLI verb + a diagnostics-bundle section (Plan 58 consent applies) |
| Retention | bounded by count/day; oldest-first eviction, visible in the dump header |

## 2. Evidence
- `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs`, `PlayableMetricsAggregationEngine.cs` (aggregates, JSONL, bounded).
- Core event seams expose facts; host adapters apply presentation/persistence (AGENTS authority rule).
- `Save/SaveStore.cs` + `SaveSlotService.cs` own slot files; the archive is adjacent, not inside the checksummed envelope.
- `PlayableMetricsAggregationEngine` is host-unreachable today (Plan 1 Appendix A).

## 3. Packages
- **HEA-91A** taxonomy: enumerated fact ids with day/hour/subject fields; unknown facts are dropped and counted, not guessed.
- **HEA-91B** writer: ring file per slot, rotation, deterministic ordering, flush on save.
- **HEA-91C** independence guard: focused test toggling archive on/off and comparing checksums.
- **HEA-91D** dump verb + bundle integration under consent.
- **HEA-91E** retention doc + header counters.

## 4. Acceptance & verification
- Archive-on vs archive-off runs produce identical checksums.
- Dump shows the last N events in order after a scripted 3-day run.
- Rotation verified at the boundary count; deleted slot leaves no orphan archive.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Telemetry/` + one headless dump run.

## 5. Risks
Archive becoming an authority → independence guard is a permanent test.
Archive growth → count/day bounds; header shows eviction state.

---

## 6. Expanded census (6 files · 1,477 lines)

Scope: `Assets/Ashfall.Core/Telemetry/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 4 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `JournalCorpus.cs` | 348 | Support | — | 0 | 0 | 0 |
| `JournalEntry.cs` | 34 | Support | — | 0 | 0 | 0 |
| `JournalSystem.cs` | 484 | System | — | 0 | 0 | 4 |
| `JournalVoice.cs` | 59 | Support | — | 0 | 0 | 0 |
| `JournalVoiceProseCatalog.cs` | 150 | Catalog | — | 0 | 0 | 0 |
| `PlaySessionRecorder.cs` | 402 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `faction_war_journal.json` | array[26] |
| `journal_entries_expansion_05.json` | object[2 keys] |
| `journal_voice_prose.json` | object[2 keys] |
| `recon_telemetry_probes.json` | object[2 keys] |
| `awl_saddle_stitch_journals.json` | array[7] |
| `dweller_psychological_journals.json` | array[8] |

**State surfaces:** `JournalSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Telemetry/` |
| Test references | 83 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 8. Other plans referencing them: **15**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-TRIO-FAMILY-TRUTH-280` | 5 |
| `PLAN-TELEMETRY-PRIVACY-58` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |
| `PLAN-NARRATIVE-GRAPH-18` | 1 |
| `PLAN-EVENT-WIRING-21` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `HEA-91A` | no name match — resolve at claim time |
| `HEA-91B` | no name match — resolve at claim time |
| `HEA-91C` | no name match — resolve at claim time |
| `HEA-91D` | no name match — resolve at claim time |
| `HEA-91E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 8; intra-domain edges: **4**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `JournalCorpus` | `JournalVoice` |
| `JournalSystem` | `JournalEntry` |
| `JournalSystem` | `JournalVoice` |
| `JournalVoice` | `JournalVoiceProseCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `JournalVoice` | 2 |
| `JournalEntry` | 1 |
| `JournalVoiceProseCatalog` | 1 |
| `JournalCorpus` | 0 |
| `JournalSystem` | 0 |
| `PlaySessionRecorder` | 0 |
| `PlayableMetricsAggregationEngine` | 0 |
| `SaveSlotService` | 0 |

**Class split:** hub 1 · sink 2 · source 2 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 8. Host files: **32** · Test files: **67** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 32 | `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/ArchiveDeskHostSession.cs`, `src/Host/CodexHostSession.cs`, `src/Host/CollectibleEffectDispatcher.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 67 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/narrative/journal_entries_batch_1.json`, `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `grain_milling_archive` |
| `host_event` |
| `hydrogeology_archive` |
| `journal` |
| `leatherwork_archive` |
| `technical_material_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--day1-playable-selftest` |
| `--journal-save-selftest` |
| `--journal-selftest` |
| `--journal-uitest` |
| `--journal-weather-panel-selftest` |
| `--playable-loop-selftest` |
| `--playable-shell-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEntryAdded` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnJournalTriggered` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/journal_entries_expansion_05.json` |
| `Assets/StreamingAssets/Data/journal_voice_prose.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/eulogy_corpus_batch_1.json` |
| `Assets/StreamingAssets/Data/narrative/journal_entries_batch_1.json` |
| `Assets/StreamingAssets/Data/narrative/journal_entries_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/journal_entries_batch_3.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (3 files, 14 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Voice` | 3 | 14 |

**Verdict:** 14 cases sit under matching regions — run those first (`Voice`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **154**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **8**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `grain_milling_archive` | no |
| `host_event` | no |
| `hydrogeology_archive` | no |
| `journal` | no |
| `leatherwork_archive` | no |
| `technical_material_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `black_market_debt_event` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **15**
(CODEX_ONLY 9, GAMEPLAY_CONSUMED 4, OPTIONAL 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `journal_entries_expansion_05.json` | OPTIONAL |
| `journal_voice_prose.json` | GAMEPLAY_CONSUMED |
| `narrative/education_session_records.json` | CODEX_ONLY |
| `narrative/eulogy_corpus_batch_1.json` | CODEX_ONLY |
| `narrative/journal_entries_batch_1.json` | CODEX_ONLY |
| `narrative/journal_entries_batch_2.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 15
**Surface:** save sections 8 (laddered 0) · RNG streams 1 · host files 14 · catalogs 22 · test regions 1 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HOST-EVENT-ARCHIVE-91
wave: 8
status: PROPOSED — foreman claim required
packages: HEA-91A, HEA-91B, HEA-91C, HEA-91D, HEA-91E
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_inks.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Voice/
  - godot --headless --path . -- --day1-playable-selftest
dependencies:
  - coordinate: 15 other plan(s) name these artifacts (§12)
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
