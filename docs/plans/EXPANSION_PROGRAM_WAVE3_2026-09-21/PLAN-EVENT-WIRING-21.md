# PLAN-EVENT-WIRING-21 — Dead Event, Callback & Producer Sealing

**Wave:** 3 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-INTEGRATION-KIT-02 (gate), PLAN-ORPHAN-SEAL-01 (host paths).
**Expanded appendix:** [`PLAN-EVENT-WIRING-21_APPENDIX-A_EVENT_INVENTORY.md`](PLAN-EVENT-WIRING-21_APPENDIX-A_EVENT_INVENTORY.md)
— the full wiring inventory for all **478 Core events**: subscriber count and
files, producer evidence, and a verdict (`WIRED` · `DEAD_CONSUMER` ·
`DEAD_BOTH`) plus the complete non-wired work list. EW-21B packages pick
their rows directly from it.
**Non-goals:** no new event bus, no global pub/sub framework, no behavior change
for events that already have consumers.

---

## 1. Outcome

The ledger already contains two repair waves for exactly this class of bug
(`OnMethaneIgnition`, `OnSectorFlooded`, `OnFilterBreakthrough`, recon wind
gate, audio bridge disposal). This plan makes the repair systematic instead of
incident-driven.

**Finding (2026-09-21):** Core declares **478 events**; **84 have zero
`+=` subscribers anywhere** in Core, host, or tests. Each is a promise the
code makes and never keeps: the game state changes and nothing observable
happens. Twelve examples from the inventory:

| Event | Declaring file |
|---|---|
| `OnCensusUpdated` | `CensusClaimSystem.cs` |
| `OnDecisionMade`, `OnSalvageRolled`, `OnNarrativeMarker` | `District8DeepCoastSystem.cs` |
| `OnDoseCorrected`, `OnLedgerCalibrated` | `DoseLedgerSystem.cs` |
| `OnRelationsChanged` | `SurvivorRelationsSystem.cs` |
| `OnQuestStageAdvanced` | `DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnMarkSet`, `OnMarkCleared` | `DutyRoster/MoraleMarkSystem.cs` |
| `OnCampSuppliesReserved`, `OnCampEncounterResolved` | `Expeditions/ExpeditionSystem.cs` |
| `OnStatusLost` | `Radiation/RadiationSystem.cs` |
| `OnTriangulationCompleted`, `OnFrequencyLocked`, … | `Radio/SignalTriangulationSystem.cs` |
| `OnHidePreserved` | `WildlifeTrappingSystem.cs` |
| `OnAccessSoftened` | `VouchAccessSystem.cs` |

Deliverables: a generated event inventory, triage of all 84, a gate that stops
new dead events, and the same audit extended to host callbacks.

---

## 2. Evidence

| Fact | Value | Method |
|---|---:|---|
| Core event declarations | 478 | regex over `public event` in `Assets/Ashfall.Core/**` |
| Zero-subscriber events | 84 | `\bName\s*\+=` across Core+src+tests |
| Prior repairs | ≥ 10 events sealed 2026-09-18/20 | INTEGRATION_PLANS |
| Naming collisions | many `OnStateChanged` (per-system) | inventory groups by declaring type |

**Audit script (reproducible):** parse declarations, count `+=` with an
identifier-boundary regex, then check producers (`Invoke`/raise sites). Groups
with zero subscribers are dead; groups with subscribers but no producer are
also dead in the other direction (e.g. the pre-repair methane case).

---

## 3. Packages

### EW-21A — Event inventory + gate
- Generate `docs/architecture/EVENT_WIRING_INVENTORY.md`:
  `event | declaring type | subscribers (files) | producers (call sites) |
  status`. Status: `WIRED`, `DEAD_CONSUMER`, `DEAD_PRODUCER`, `FDX_ONLY`
  (fire-and-forget diagnostics with a named log consumer).
- Gate `scripts/ci/event-wiring-gate.py --check` fails on new
  `DEAD_CONSUMER`/`DEAD_PRODUCER` events unless allowlisted with a reason.
- **Acceptance:** inventory generated deterministically; gate in fast tier.
- **Verify:** `python3 scripts/ci/event-wiring-gate.py --check`.

### EW-21B — Consumer sealing (the 84)
Triage each into:
- **WIRE** — a host adapter already has the effect as an explicit call; convert
  it to an event subscription so the effect cannot be bypassed (initiative,
  quest stage, morale mark, expedition camp, triangulation).
- **JOURNAL** — route to `JournalSystem.TryAddRawEntry` with a stable dedup
  key when the fact is player-facing (census, dose correction, relations).
- **RETIRE** — no intended consumer; delete the declaration and its raise site.
- **HOST-EVENT** — promote to a host-facing delegate consumed by panels/audio
  (triangulation, deep coast salvage).
- Rules: no event stays unclassified; a retired event's raise site is deleted
  too; a wired event gets exactly one host adapter path.
- **Acceptance:** 0 unclassified; each wired event has a focused test proving
  the observable effect; each retired event has a `KNOWN_DEBT`-free deletion
  note in the package handoff.
- **Verify:** focused suites per domain (duty roster, expeditions, radio,
  radiation, relations) + the inventory gate.

### EW-21C — Producer sealing (reverse direction)
- For events with subscribers, prove at least one raise site runs on the live
  path (not test-only). Flag `TEST_ONLY_PRODUCER` events.
- **Acceptance:** every subscriber-bearing event names its live producer; the
  two known historical cases stay sealed.
- **Verify:** the inventory + focused tests.

### EW-21D — Host callback sweep
- Extend the same audit to host-side callbacks: `Action`/`Func` fields,
  `Callable` connections, signal-style delegates, and panel button handlers
  with no assignment. Report dead controls (buttons whose handler is
  `Callable`-fixed but never connected).
- **Acceptance:** report committed; dead host callbacks retired; the
  panel-bind lifecycle probe covers the changed surfaces.
- **Verify:** `godot --headless --path . -- --panel-bind-lifecycle-selftest`.

---

## 4. Risk register

| Risk | Mitigation |
|---|---|
| Wiring an event changes ordering | subscriptions run after the state mutation that raised them; no state write inside a handler (adapter only) |
| Retiring an event that a mod/consumer uses | search repo-wide first; retirement gate lists references |
| Duplicate effects (event + explicit call) | WIRE replaces the explicit call in the same package |
| Inventory drifts | generated + gate |

## 5. Verification

```bash
python3 scripts/ci/event-wiring-gate.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/
godot --headless --path . -- --panel-bind-lifecycle-selftest
```

---

## 6. Expanded census (bespoke: event surface)

This plan wires events to consumers, so the census collects event-type names
across Core and host by name pattern.

| Metric | Value |
|---|---:|
| Distinct `*Event` type names | 114 |

**Sample events (first 30):** `AcceptEvent`, `AcknowledgeArcEvent`, `AcknowledgeEvent`, `ActionAddEvent`, `ActionEraseEvent`, `ActiveSeasonalEvent`, `AddEvent`, `AddSilenceEvent`, `ApplyEvent`, `BelongingEvent`, `BlackMarketDebtEvent`, `ButcheryCompletedEvent`, `BycatchOccurredEvent`, `CascadeEvent`, `CloneEvent`, `CombatEvent`, `ConductCourtshipEvent`, `CreatePendingEvent`, `CrossingStageNarrativeEvent`, `DayRecordEvent`, `DayStateChangeEvent`, `DevelopmentMilestoneEvent`, `DispatchCatalogEvent`, `DocumentationEvent`, `DynamicEvent`, `EscalationEvent`, `ExposureEvent`, `FamilyEvent`, `FeedbackEvent`, `FindEvent`

## 7. Expanded surface: wiring contract

| Rule | Detail |
|---|---|
| Producer | a Core owner publishes a fact; no presentation logic in the event |
| Consumer | at least one host adapter applies the effect; a producer with no consumer is reported |
| Ordering | deterministic publish order within a day (Plan 33) |
| No re-entry | an effect never republishes its own event |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Consumer census | each event has a named consumer or a dead-consumer verdict |
| Ordering | paired same-seed runs publish identically |
| Fixture | one scripted event per class reaches its consumer |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) and consumer mapping.
2. Wire or retire dead-consumer events.
3. Ordering fixtures.
4. Regression: consumer census regenerated.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Event | has a producer fact and a consumer effect |
| Ordering | deterministic within a day |
| Dead event | retired or explicitly recorded |
| Fixture | scripted event reaches its consumer |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not add events.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 6. Other plans referencing them: **8**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 3 |
| `PLAN-ECOLOGY-WILDLIFE-26` | 1 |
| `PLAN-MARITIME-DEEPWATER-27` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-MUTATION-HEREDITY-81` | 1 |
| `PLAN-DEEP-STRATA-83` | 1 |
| `PLAN-ANCIENT-RUINS-VAULTS-84` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `WIRE` | no name match — resolve at claim time |
| `JOURNAL` | no name match — resolve at claim time |
| `RETIRE` | no name match — resolve at claim time |
| `HOST-EVENT` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **0**; isolated files:
**6**.

**Top edges (by source name):**

| From | → To |
|---|---|
| — | no intra-domain references found |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `CensusClaimSystem` | 0 |
| `District8DeepCoastSystem` | 0 |
| `DoseLedgerSystem` | 0 |
| `SurvivorRelationsSystem` | 0 |
| `VouchAccessSystem` | 0 |
| `WildlifeTrappingSystem` | 0 |

**Class split:** hub 0 · sink 0 · source 0 · isolated 6.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **30** · Test files: **82** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 30 | `src/Dose/DoseRegisterSurface.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CoreDemoSession.cs`, `src/Host/DeepCoastHostSession.cs`, `src/Host/DoseLedgerHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 82 | `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **9** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `deep_well` |
| `dose_ledger` |
| `host_event` |
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |
| `wildlife_ecosystem` |
| `wildlife_trapping` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **10** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--census-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--ledger-debt-selftest` |
| `--survivor-death-selftest` |
| `--wildlife-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **19**.

| Event | First declaration |
|---|---|
| `OnAccessSoftened` | `Assets/Ashfall.Core/VouchAccessSystem.cs` |
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnDoseChanged` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnDoseCorrected` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnOverlayAccessChanged` | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnRelationsChanged` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/dose_items.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/dose_registers.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/deep_lore_texts.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json` |

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

Host files (`src/`) whose names share a domain token: **42**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/HostEventAdapter.cs` |
| `src/Host/HostEventSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/SurvivorDeathLegacyHostSession.cs` |
| `src/Host/SurvivorDeathLegacySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **9**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `deep_well` | no |
| `dose_ledger` | yes |
| `host_event` | no |
| `survivor_fate` | no |
| `survivor_mental_health` | no |
| `survivor_relations` | no |
| `survivor_social` | no |
| `wildlife_ecosystem` | no |
| `wildlife_trapping` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `black_market_debt_event` |
| `deep_coast` |
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **16**
(CODEX_ONLY 6, GAMEPLAY_CONSUMED 7, OPTIONAL 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `dose_locations.json` | GAMEPLAY_CONSUMED |
| `dose_quests.json` | GAMEPLAY_CONSUMED |
| `dose_registers.json` | GAMEPLAY_CONSUMED |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 9 (laddered 1) · RNG streams 6 · host files 19 · catalogs 22 · test regions 1 · flags 10

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-EVENT-WIRING-21
wave: —
status: PROPOSED — foreman claim required
packages: EW-21A, EW-21B, EW-21C, EW-21D
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Dose/DoseRegisterSurface.cs  # §19 candidate host surface
  - src/Host/DeepCoastHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_locations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrapping/
  - godot --headless --path . -- --census-selftest
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
