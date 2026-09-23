# PLAN-THREADING-ASYNCHRONY-72 — Main-Thread Discipline, Async IO & Watchdogs

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RUNTIME-RESILIENCE-57, PLAN-ARCHITECTURE-BOUNDARY-31.
**Implementation scaffold:** [`PLAN-THREADING-ASYNCHRONY-72_APPENDIX-A_SCAFFOLD.md`](PLAN-THREADING-ASYNCHRONY-72_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no task-parallel simulation, no multithreaded world tick.

## Outcome
Godot is single-threaded for scene/state access, but the codebase does file IO,
JSON parsing, and potentially background work. There is no declared threading
contract, no async pattern, and the planned tick watchdog (Plan 57) needs one.
This plan defines and audits thread discipline.

| Area | Rule |
|---|---|
| Simulation | all campaign/day mutation on the main thread |
| File IO | allowed off-thread only through an owned async wrapper that returns data, never mutates state |
| Loading | catalog parse may be backgrounded; publication on the main thread |
| Events | raise on the thread that mutated; no cross-thread UI calls |
| Timers | engine timers for UI; day logic via day owners, never `Task.Delay` |
| Watchdog | overrun aborts the owner on the main thread; no thread kill |
| Signals | Godot signals only from main thread |

## Evidence
- Save/catalog IO is synchronous today (`File.ReadAllText` sites: 487 across Core, PLAN-ARCHITECTURE-BOUNDARY-31).
- `HostCli` runs headless sequentially; no concurrency in the tick path.
- 517 UI files with 229 lifecycle methods (Plan 32) — cross-thread UI risk if async creeps in.
- Catch policy + typed results exist; watchdog planned in Plan 57.

## Packages
- **TH-72A** contract doc: `docs/architecture/THREADING_MODEL.md` with allowed/forbidden patterns and examples.
- **TH-72B** static gate: flag `Task.Run`, `Thread`, `async void` (outside event handlers), `ConfigureAwait(false)` in state paths, and any background mutation of session state.
- **TH-72C** async IO wrapper (if adopted): load-and-return pattern; cancellation; errors as typed results; no partial state on cancel.
- **TH-72D** cross-thread audit of existing UI/session calls; fixes to main-thread dispatch.
- **TH-72E** watchdog integration: owner budget, abort semantics, incident log, state consistency check after abort.

## Acceptance & verification
- Gate green; a synthetic background mutation fails it; watchdog abort leaves campaign loadable and deterministic.
- `python3 scripts/ci/threading-gate.py --check`; `godot --headless --path . -- --safe-mode-selftest`; determinism replay.

## Risks
Async complexity for little gain → default remains synchronous; async only where measured (big catalog loads).

---

## 6. Expanded census (168 files with threading references)

Content-based census: threading is found by usage, not filename. Totals per
primitive below the table.

| File | Total | async | await | Task | Thread | lock | Interlocked | Lines |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| `Flags/CampaignConsequenceLedger.cs` | 14 | 0 | 0 | 0 | 0 | 14 | 0 | 301 |
| `host:Main.Medical.cs` | 9 | 0 | 0 | 9 | 0 | 0 | 0 | 528 |
| `RegionalTreatySystem.cs` | 8 | 0 | 0 | 8 | 0 | 0 | 0 | 389 |
| `WildlifeTrappingSystem.cs` | 7 | 0 | 0 | 7 | 0 | 0 | 0 | 1621 |
| `Medical/MedicalTreatmentCatalog.cs` | 7 | 0 | 0 | 7 | 0 | 0 | 0 | 221 |
| `Medical/MedicalPipelineCoordinator.cs` | 6 | 0 | 0 | 6 | 0 | 0 | 0 | 747 |
| `host:Main.UiHandlers.cs` | 6 | 2 | 3 | 1 | 0 | 0 | 0 | 321 |
| `host:UI/MedicalPanel.cs` | 6 | 0 | 0 | 6 | 0 | 0 | 0 | 939 |
| `host:Main.ShelterSocial.cs` | 5 | 0 | 0 | 5 | 0 | 0 | 0 | 603 |
| `host:Host/HostCli.Collectibles.cs` | 5 | 0 | 0 | 5 | 0 | 0 | 0 | 496 |
| `WildlifeTrappingCatalog.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 389 |
| `Medical/PsychologyAfflictionHandlers.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 262 |
| `Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 460 |
| `Save/SaveEnvelopeHelper.cs` | 4 | 0 | 0 | 1 | 0 | 0 | 3 | 320 |
| `Institutions/InstitutionAssignmentLedger.cs` | 4 | 0 | 0 | 0 | 0 | 4 | 0 | 57 |
| `host:Main.MoralChoice.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 288 |
| `host:Main.CampaignOwners.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 1520 |
| `host:UI/SaveLoadPanel.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 403 |
| `Narrative/BlackProjectsArchiveSystem.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 367 |
| `Narrative/OralLoreCatalog.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 240 |
| `Radio/RadioScheduleCoordinator.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 427 |
| `Survivors/LaborProductivity.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 206 |
| `Feedback/FeedbackMessageCatalogLoader.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 343 |
| `MoralChoice/MoralChoiceIds.cs` | 3 | 0 | 0 | 2 | 1 | 0 | 0 | 260 |
| `Telemetry/PlaySessionRecorder.cs` | 3 | 0 | 0 | 0 | 0 | 3 | 0 | 402 |
| `host:Main.ShelterBatch3.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 468 |
| `host:Main.GameFlow.cs` | 3 | 1 | 1 | 1 | 0 | 0 | 0 | 926 |
| `host:Host/MedicalHostSession.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 305 |
| `host:Host/WildlifeTrappingHostSession.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 644 |
| `host:Host/Phase0HostSession.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 1096 |
| `host:Host/HostCli.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 834 |
| `host:UI/AfflictionsPanel.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 448 |
| `host:UI/ChemicalDependencyPanel.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 413 |
| `host:UI/FeedbackMessages.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 560 |
| `host:UI/Phase0Panel.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 339 |
| `host:World/HoldfastInteriorView.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 635 |
| `Economy/CaravanTradeNetworkSystem.cs` | 2 | 0 | 0 | 2 | 0 | 0 | 0 | 670 |
| `Medical/DiseaseAfflictionHandler.cs` | 2 | 0 | 0 | 2 | 0 | 0 | 0 | 177 |
| `Medical/MedicalWardPipelineBridge.cs` | 2 | 0 | 0 | 2 | 0 | 0 | 0 | 88 |
| `Medical/PatientRecord.cs` | 2 | 0 | 0 | 2 | 0 | 0 | 0 | 233 |

… and 128 more files.

**Files touching raw `Thread`/`Interlocked`:** 8 — the plan's static gate starts from this deny list.

## 7. Expanded surface: the main-thread contract

| Surface | Rule |
|---|---|
| Core simulation | main thread only; no Core call from a worker |
| IO | through the async wrapper with cancellation + timeout |
| Watchdog | stalled work reported, never silently dropped |
| Determinism | async completion order must not change simulation state |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Static gate | scan for `Thread(`/`Task.Run(`/`Interlocked.` outside allowed files |
| Wrapper tests | cancellation and timeout fixtures |
| Watchdog | one fixture per stall class |
| Focused region | `Ashfall.Core.Tests/Performance/` |

## 9. Rollout sequence

1. Census and deny-list (this section) — no edits.
2. Wrapper: cancellation/timeout tests first.
3. Static gate wired with the allow/deny sets.
4. Watchdog integration with a stall fixture.
5. Regression: gate + focused region.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Static gate | fails on an injected violation; allow-list documented |
| Wrapper | cancellation and timeout proven |
| Watchdog | stall produces a report, not silence |
| Determinism | paired runs unchanged under async paths |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not introduce a threading model.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 3. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ECOLOGY-WILDLIFE-26` | 2 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `TH-72A` | no name match — resolve at claim time |
| `TH-72B` | no name match — resolve at claim time |
| `TH-72C` | no name match — resolve at claim time |
| `TH-72D` | no name match — resolve at claim time |
| `TH-72E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **7** · Test files: **36** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/HostCli.EvolvingWorld.cs`, `src/Host/HostCli.Plans162_165.cs`, `src/Host/HostCli.WorldPlaytest.cs`, `src/Host/PanelBindLifecycleSelfTest.cs`, `src/Host/RegionalTreatyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 36 | `Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs`, `Ashfall.Core.Tests/Expeditions/Plan20CConsumerWiringTests.cs`, `Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs`, `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`, `Ashfall.Core.Tests/RegionalTreatyCatalogLoaderTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `WildlifeTrappingCatalog` | `WildlifeTrappingSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `regional_treaty` |
| `wildlife_ecosystem` |
| `wildlife_trapping` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--wildlife-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnTrappingChanged` | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyQuotaMet` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyQuotaMissed` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyStatusChanged` | `Assets/Ashfall.Core/RegionalTreatySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` |
| `Assets/StreamingAssets/Data/narrative/regional_treaty_protocols.json` |
| `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/regional_prices.json` |
| `Assets/StreamingAssets/Data/regional_treaties.json` |
| `Assets/StreamingAssets/Data/treaty_templates.json` |
| `Assets/StreamingAssets/Data/wildlife_ecosystem.json` |
| `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (5 files, 70 cases).

| Region | Files | Cases |
|---|---:|---:|
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 70 cases sit under matching regions — run those first (`WildlifeTrapping`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **9**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/RegionalTreatyHostSession.cs` |
| `src/Host/RegionalTreatySaveStore.cs` |
| `src/Host/WildlifeEcosystemHostSession.cs` |
| `src/Host/WildlifeEcosystemSaveStore.cs` |
| `src/Host/WildlifeTrappingHostSession.cs` |
| `src/Host/WildlifeTrappingSaveStore.cs` |
| `src/UI/AquiferTreatyConcessionPanel.cs` |
| `src/UI/RegionalTreatyPanel.cs` |
| `src/UI/WildlifeTrappingPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `regional_treaty` | no |
| `wildlife_ecosystem` | no |
| `wildlife_trapping` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `foundry_treaty_consequences.json` | GAMEPLAY_CONSUMED |
| `narrative/regional_treaty_protocols.json` | CODEX_ONLY |
| `narrative/wasteland_wildlife_bestiary.json` | CODEX_ONLY |
| `narrative/wildlife_field_encounter_logs.json` | CODEX_ONLY |
| `wildlife_trapping_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_broke_treaty` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 3 (laddered 0) · RNG streams 4 · host files 14 · catalogs 14 · test regions 1 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-THREADING-ASYNCHRONY-72
wave: 7
status: PROPOSED — foreman claim required
packages: TH-72A, TH-72B, TH-72C, TH-72D, TH-72E
claim paths:
  - src/Host/RegionalTreatyHostSession.cs  # §19 candidate host surface
  - src/Host/RegionalTreatySaveStore.cs  # §19 candidate host surface
  - src/Host/WildlifeEcosystemHostSession.cs  # §19 candidate host surface
  - src/Host/WildlifeEcosystemSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/foundry_treaty_consequences.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/regional_treaty_protocols.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrapping/
  - godot --headless --path . -- --wildlife-selftest
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
