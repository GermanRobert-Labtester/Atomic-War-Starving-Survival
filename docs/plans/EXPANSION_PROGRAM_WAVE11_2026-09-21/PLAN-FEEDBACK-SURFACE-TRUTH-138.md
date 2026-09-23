# PLAN-FEEDBACK-SURFACE-TRUTH-138 — Message Catalog Coverage, Dedup Windows & Interface Compliance

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SILENT-FAILURE-35, PLAN-PLAYER-COMMAND-TRUTH-131, PLAN-LOCALIZATION-READINESS-52.
**Implementation scaffold:** [`PLAN-FEEDBACK-SURFACE-TRUTH-138_APPENDIX-A_SCAFFOLD.md`](PLAN-FEEDBACK-SURFACE-TRUTH-138_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-HOST-EVENT-ARCHIVE-91` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new notification system, no toast framework, no gameplay
effects from feedback.

## 1. Outcome
`Feedback/` already implements the machinery: `FeedbackEvent`,
`FeedbackMessageCatalog(+Loader)`, `FeedbackMessageTypes`, `FeedbackDeduplicator`,
`FeedbackService` behind `IFeedbackService`, and `ResolvedFeedbackMessage`. The
contract gaps are coverage and discipline: does every refusal/outcome code have
a message, does dedup run in game time (not wall time), and is the interface the
only way UI receives messages?

| Deliverable | Detail |
|---|---|
| Coverage gate | every `PlayerCommandCode` refusal (Plan 131) and every typed outcome maps to a catalog message; a missing message fails the gate |
| Parameter truth | `ResolvedFeedbackMessage` substitution renders from data; no concatenated English in code |
| Dedup window | suppression window measured in game time/ticks, documented; the same event in a new day is not suppressed |
| Interface rule | UI consumes `IFeedbackService` only; a direct catalog read in a panel is a defect |
| Localization link | catalog entries are localizable keys (Plan 52), not literals baked into panels |

## 2. Evidence
- `Assets/Ashfall.Core/Feedback/` file list (verified).
- Plan 131 produces the refusal codes this plan renders.
- Plan 52 owns the extraction/localization pipeline for UI-facing text.
- Plan 35 bans swallowing failures; a missing message must surface loudly in dev builds.

## 3. Packages
- **FST-138A** coverage gate: code set ↔ message set; fail with the missing code named.
- **FST-138B** parameter substitution tests (missing parameter → typed failure, not "null").
- **FST-138C** dedup window tests in game time (OS clock change has no effect).
- **FST-138D** interface scan: panels touching the catalog directly are reported.
- **FST-138E** localization key check against Plan 52's inventory.

## 4. Acceptance & verification
- Coverage gate green; a deliberately unmapped code fails with its name.
- Dedup suppresses within the window and not across a day boundary.
- No panel reads the catalog directly; the scan proves it.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Feedback/`.

## 5. Risks
Message churn → catalog entries are keys; wording changes do not touch code.
Dedup hiding important failures → refusals of a different code are never deduped together; the fixture covers it.

---

## 6. Expanded census (8 files · 885 lines)

Scope: `Assets/Ashfall.Core/Feedback/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Loader 1 · Support 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FeedbackDeduplicator.cs` | 102 | Support | — | 0 | 0 | 0 |
| `FeedbackEvent.cs` | 42 | Support | — | 0 | 0 | 0 |
| `FeedbackMessageCatalog.cs` | 182 | Catalog | — | 0 | 0 | 0 |
| `FeedbackMessageCatalogLoader.cs` | 343 | Loader | — | 0 | 0 | 0 |
| `FeedbackMessageTypes.cs` | 41 | DTO/Type | — | 0 | 0 | 0 |
| `FeedbackService.cs` | 129 | Support | **yes** | 0 | 0 | 0 |
| `IFeedbackService.cs` | 22 | Support | — | 0 | 0 | 0 |
| `ResolvedFeedbackMessage.cs` | 24 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `feedback_messages.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Feedback/` (create if absent) |
| Test references | 6 name references across the test tree |
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

Domain files: 8. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FST-138A` | `FeedbackMessageCatalog.cs`, `FeedbackMessageCatalogLoader.cs`, `FeedbackMessageTypes.cs` |
| `FST-138B` | no name match — resolve at claim time |
| `FST-138C` | `FeedbackDeduplicator.cs` |
| `FST-138D` | `FeedbackMessageCatalog.cs`, `FeedbackMessageCatalogLoader.cs` |
| `FST-138E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 8; intra-domain edges: **10**; isolated files:
**1**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `FeedbackMessageCatalogLoader` | `FeedbackMessageCatalog` |
| `FeedbackService` | `FeedbackDeduplicator` |
| `FeedbackService` | `FeedbackEvent` |
| `FeedbackService` | `FeedbackMessageCatalog` |
| `FeedbackService` | `IFeedbackService` |
| `FeedbackService` | `ResolvedFeedbackMessage` |
| `IFeedbackService` | `FeedbackDeduplicator` |
| `IFeedbackService` | `FeedbackEvent` |
| `IFeedbackService` | `FeedbackMessageCatalog` |
| `IFeedbackService` | `ResolvedFeedbackMessage` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `FeedbackMessageCatalog` | 3 |
| `FeedbackDeduplicator` | 2 |
| `FeedbackEvent` | 2 |
| `ResolvedFeedbackMessage` | 2 |
| `IFeedbackService` | 1 |
| `FeedbackMessageCatalogLoader` | 0 |
| `FeedbackMessageTypes` | 0 |
| `FeedbackService` | 0 |

**Class split:** hub 1 · sink 4 · source 2 · isolated 1.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 8. Host files: **8** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Main.GameFlow.cs`, `src/Main.Medical.cs`, `src/Main.SaveOrchestrator.cs`, `src/Main.SurvivorFate.cs`, `src/Main.UiPanels.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/UI/FeedbackMessageTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `host_event` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

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

Events whose name shares a domain token: **22**.

| Event | First declaration |
|---|---|
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnApproachResolved` | `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs` |
| `OnCampDawnResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampNightSegmentResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnChallengeResolved` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnGuiltResolved` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/feedback_messages.json` |
| `Assets/StreamingAssets/Data/food_types.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **9**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostEventAdapter.cs` |
| `src/Host/HostEventSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/UI/EventDetailPanel.cs` |
| `src/UI/FeedbackMessages.cs` |
| `src/UI/FeedbackPanel.cs` |
| `src/UI/PanelSceneLoader.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `host_event` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `feedback_messages.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

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
**Surface:** save sections 1 (laddered 0) · RNG streams 1 · host files 10 · catalogs 3 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FEEDBACK-SURFACE-TRUTH-138
wave: 11
status: PROPOSED — foreman claim required
packages: FST-138A, FST-138B, FST-138C, FST-138D, FST-138E
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/HostEventAdapter.cs  # §19 candidate host surface
  - src/Host/HostEventSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/feedback_messages.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/food_types.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
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
