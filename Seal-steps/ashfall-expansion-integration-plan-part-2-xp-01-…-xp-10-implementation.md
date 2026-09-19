# ASHFALL — EXPANSION INTEGRATION PLAN · PART 2

**Document type:** Implementation plan — file-level integration of Part 1 pillars XP-01 … XP-10
**Companion:** Part 1 (feature proposals; this document implements them)
**Orientation:** Part 1 answers *what and why*. This document answers *where, in what order, through which seams, with which tests and gates*. No new feature design appears here; where Part 1 left a choice open, the implementing choice is stated once and marked `[IMPL-CHOICE]`.

---

## 0 · GROUND RULES (BINDING FOR ALL WAVES)

| Rule | Enforcement |
|---|---|
| Godot 4.7.1 .NET host; engine-free Core | All new systems live in `Assets/Ashfall.Core/<Domain>/`; zero `Godot`/`GodotSharp`/`UnityEngine` references; `DeterminismGuardTests` extended per wave |
| `snake_case` ids everywhere | New catalogs validated by `CatalogIntegrityValidator` + `--data-integrity-selftest` |
| State changes via `IEventBus` only | New event types registered in the event-surface architecture test (`EventSurfaceArchitectureTests`) before first use |
| Persistence via `CaptureState`/`RestoreState` | Every new store registers in the comprehensive save-store registry; section count constant updated exactly once per wave |
| `ISeededRng` sub-streams only; anti-reroll for lasting rolls | Sub-stream names registered centrally (`difficulty`, `graph_survey`, `psychology`, `radio_presenter`, `bm_heat`, `war_chain`); `DeterminismGuardTests` asserts no `System.Random`/`Guid.NewGuid()`/`DateTime.UtcNow` in new files |
| Additive-first migrations | Every migration ships with a legacy-parity test asserting byte-identical behavior when the new feature is absent |
| Restore never replays events | Applied to war bands (XP-03), heat relocation (XP-04), quality rolls (XP-09), phobia acquisition (XP-10) |
| No second authorities | Planner stateless; difficulty scalar-provider only; funds single ledger; read-models own nothing |
| a11y | All new panel strips: words never color-only; `--ui-layout-selftest` + `--settings-selftest` extended |
| SPDX headers + `partial` rules | New Godot `Control`/`Node` classes are `partial`; POCOs are not; `license-header-check.sh --strict` in every wave gate |

### Batch/wave bookkeeping (per repo process)

Each wave lands as one registered batch:

1. Register in `INTEGRATION_PLANS.md` (batch id `XP-WAVE<n>-<pillar>`).
2. Claim in `WORKTREE_OWNERSHIP.md`.
3. Premise-check against live source before coding (the repo's standing rule — every Part 1 evidence pointer is re-verified at wave start; if a premise has drifted, the wave log records the correction, the plan is not silently bent).
4. Closeout doc under `docs/plans/xp/<wave>/…_{PREMISE_EVIDENCE,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md`.
5. Debt rows added/retired in `KNOWN_DEBT.md` with evidence pointers.

---

## 1 · TARGET FILE MAP (ALL NEW ARTIFACTS)

### Core (`Assets/Ashfall.Core/…`)

| Pillar | New files |
|---|---|
| XP-01 | `Difficulty/DifficultyPresetCatalog.cs`, `Difficulty/DifficultyDirector.cs`, `Difficulty/DifficultyScalarsProvider.cs`, `Difficulty/CompletionChronicleProjection.cs` |
| XP-02 | `Expeditions/GraphTravelPlanner.cs`, `Expeditions/RouteTopologyModel.cs`, `Expeditions/SurveyKnowledgeStore.cs` |
| XP-03 | `WorldConsequences/WorldClockHorizon.cs`, `WorldConsequences/WarChainEconomyRoute.cs`, `WorldConsequences/WarChainExpeditionRoute.cs`, `WorldConsequences/WarChainRadioRoute.cs`, `WorldConsequences/WarChainChronicleRoute.cs` |
| XP-04 | `Economy/FundsLedger.cs`, `Economy/BlackMarketTradeActions.cs`, `Economy/BlackMarketHeatStore.cs`, `Economy/MerchantRestockPriority.cs`, `Economy/FencePurityRolls.cs` |
| XP-05 | `Energy/FuelGradeCatalog.cs`, `Energy/SofcFuelBinder.cs`, `Energy/FuelRationingQueue.cs` |
| XP-06 | `Survivors/BodyIntegrity/LimbRequirementGate.cs`, `Survivors/BodyIntegrity/ProstheticRehabilitationArc.cs`, `Survivors/BodyIntegrity/BodyStateModel.cs` |
| XP-07 | `Inventory/Provenance/ItemProvenanceModel.cs`, `Inventory/Provenance/NamedItemsCatalog.cs`, `Inventory/Provenance/LoreFragmentRevealRules.cs`, `Inventory/Provenance/ProvenanceInteractions.cs` |
| XP-08 | `Economy/TradeRoutes/TradeRouteContractStore.cs`, `Economy/TradeRoutes/RouteReliabilityLedger.cs`, `WorldConsequences/SeasonalMigrationScheduler.cs`, `WorldConsequences/MigrationConsequenceApplier.cs` |
| XP-09 | `Radio/PresenterSkills/PresenterSkillCatalog.cs`, `Radio/PresenterSkills/BroadcastQualityEvaluator.cs`, `Radio/PresenterSkills/PresenterFatigueStore.cs` |
| XP-10 | `BodyMind/Phobia/PhobiaAcquisitionRules.cs`, `BodyMind/Phobia/PhobiaIntensityStore.cs`, `BodyMind/Phobia/PhobiaUtilityConsiderations.cs`, `BodyMind/EarnedTraitGranter.cs` |

### Data catalogs (`Assets/StreamingAssets/Data/…`)

| File | Pillar | `schema_version` |
|---|---|---|
| `difficulty_presets.json` | XP-01 | 1 |
| `fuel_grades.json` | XP-05 | 1 |
| `named_items.json` | XP-07 | 1 |
| `trade_route_templates.json` | XP-08 | 1 |
| `seasonal_migration.json` | XP-08 | 1 |
| `presenter_skills.json` | XP-09 | 1 |
| `phobias.json` | XP-10 | 1 |
| `earned_traits.json` | XP-10 | 1 |
| `black_market_actions.json` | XP-04 | 1 |
| `merchant_restock_priority.json` | XP-04 | 1 |
| `world_clock_horizon.json` | XP-03 | 1 |
| Amended: `items.json` (additive `limb_requirements`), `prosthetics.json` (new) | XP-06 | items +1 minor |

### Host (`src/…`)

| Pillar | New/edited |
|---|---|
| All | `src/Host/HostCli.cs` — new selftest flags (§6) |
| XP-01 | `src/Host/DifficultyHostSession.cs`; `src/Main.cs` campaign-creation preset bind; `src/UI/CampaignPanel.cs` chronicle strip |
| XP-02 | `src/Host/GraphTravelHostSession.cs`; `src/Main.Expeditions.cs` planner bind; `src/UI/ExpeditionPanel.cs` route report |
| XP-03 | `src/Host/WorldConsequenceHostSession.cs`; `src/Main.Economy.cs` band adapter; `src/UI/FactionWarMapWidget` clash markers |
| XP-04 | `src/Host/FundsHostSession.cs`, `src/Host/BlackMarketHostSession.cs`; `src/UI/TradePanel.cs` funds readout + NEXT SUPPLY strip |
| XP-05 | `src/Host/EnergyHostSession.cs`; `src/UI/SofcPanel.cs` reserve strip + refuel action |
| XP-06 | `src/Host/BodyIntegrityHostSession.cs`; `src/UI/SurvivorDetailPanel.cs` limb/prosthetic slots |
| XP-07 | `src/UI/InventoryDetailPanel.cs` HISTORY rows (edit only — read-model, no new session) |
| XP-08 | `src/Host/TradeRouteHostSession.cs`, `src/Host/SeasonalMigrationHostSession.cs`; `src/UI/TradePanel.cs` ROUTES strip |
| XP-09 | `src/Host/PresenterSkillHostSession.cs`; `src/UI/RadioPanel.cs` presenter strip |
| XP-10 | `src/Host/PhobiaHostSession.cs`; `src/UI/SurvivorDetailPanel.cs` phobia/earned-trait rows |

`Main.<Domain>.cs` partials follow the existing per-domain split; no new god-file. Every new panel surface registers in `PanelRegistryBootstrap` + `Main.PlayerSurfaces` `expandedIds` (the `DEBT-PANEL-REACHABILITY-138` gate enforces switch↔registry parity — do it in the same commit).

---

## 2 · SAVE & MIGRATION MATRIX

One save-envelope version bump **per wave** that adds sections (not per pillar), to keep the `ComprehensiveSaveStoreCorruptionAndMigrationTests` section-count churn minimal and auditable.

| Wave | New save sections | Additive fields | Envelope version | Migration behavior |
|---|---|---|---|---|
| W1 (XP-01, XP-05) | `funds_ledger` deferred to W2; `sofc_fuel` (reserve, grade order override) | campaign header `difficulty_preset_id`; item instances unchanged | V+1 | Missing preset → `difficulty_standard`; missing fuel section → full tank + legacy `FuelConsumer` semantics byte-identical |
| W2 (XP-04) | `funds_ledger`, `black_market` (heat, fired-set) | trade surfaces gain `accepts_funds` (catalog, not save) | V+2 | Empty ledger inserted; heat 0 |
| W3 (XP-02) | `graph_survey` (edge surveyed-state) | — | V+3 | All edges unsurveyed; estimates get `±35%` bands (legacy parity test asserts identical *resolved* costs for all-clear maps) |
| W4 (XP-03) | `world_consequences` (fired-set, band persistence) | — | V+4 | Empty fired-set; no historical replay |
| W5 (XP-06, XP-09, XP-10) | `body_integrity` (limb map, rehab phase), `presenter_skills`, `phobias` (intensity, rolls), `earned_traits` | item instances `limb_requirements` are catalog-side only | V+5 | Survivors get intact limbs; skills level 0; phobias empty |
| W6 (XP-07, XP-08) | `trade_routes`, `seasonal_migration`, provenance additive field on item instances | item instance `provenance` (nullable) | V+6 | Null provenance → "HISTORY UNKNOWN"; routes/migration empty |

Implementation notes:

- All sections use the shared atomic writer + per-store SHA-256 records via a new `SaveLoadHostSession` codec per section, following the existing typed-session pattern (`src/Host/<Name>HostSession.cs` owns Capture/Restore + codec).
- Migration tests live in one new fixture per wave: `Ashfall.Core.Tests/SaveMigrations/Wave<n>MigrationTests.cs`, each asserting: (a) previous-version save loads, (b) absent-feature behavior byte-identical, (c) round-trip V→V+1→V stable, (d) corrupted section rejected fail-closed.
- `SaveEnvelopeDetection` + `SaveChecksum` contracts unchanged — no new envelope kinds.
- The section-count constant in `ComprehensiveSaveStoreCorruptionAndMigrationTests` is updated **once per wave**, in the same commit that registers the codecs (the standing failure mode called out in `DEBT-STALE-TEST-CONTRACTS` — never let it drift across commits).

---

## 3 · WAVE IMPLEMENTATION DETAIL

Each wave below lists: premise checks, exact wiring commits (small, each independently green), and Definition of Done. Commit slicing follows the repo rule: build 0/0, full suite green, at **every** commit boundary.

### WAVE 1 · XP-01 Difficulty + XP-05 Fuel (parallel, zero cross-dependency)

**Premise checks (before any code):**

- [ ] Confirm no difficulty authority appeared since KNOWN_DEBT review (search `DifficultyPreset|difficulty_presets|DifficultyDirector` repo-wide)
- [ ] Confirm SOFC `FuelConsumer` still `units => true` placeholder (grep the lambda)
- [ ] Confirm `RoomPowerProvider` pattern shape in `SanitationSystem` (the provider precedent this wave copies)

**Commit 1.1 — XP-01 catalog + director (Core only, no consumers):**
`difficulty_presets.json`, `DifficultyPresetCatalog` loader, `DifficultyDirector`, `DifficultyScalarsProvider` (7 nullable scalar getters). `DifficultyPresetCatalogTests` + `DifficultyDirectorTests` (fail-closed unknown id, default resolution, scalar ranges 0.25–2.5).

**Commit 1.2 — XP-01 scalar seams (7 systems, one commit each inside the batch):**
For each of the 7 seams (needs, radiation, disease, encounters, market pricing, equipment decay, crisis deadlines):
1. Add optional provider parameter to the consuming tick/rule (appended after existing params — the `DEBT-194` binding precedent: appended-only, priority order unchanged).
2. Multiply base value by provider scalar when set.
3. Add one provider-unset parity test asserting legacy result byte-identical.
Register all 7 in `DifficultyScalarSeamTests` as a table-driven suite so adding an 8th seam later is a row, not a file.

**Commit 1.3 — XP-01 host + chronicle:**
`DifficultyHostSession` (Capture/Restore of header field), campaign-creation preset bind in `src/Main.cs`, `CompletionChronicleProjection` + `CampaignPanel` LEDGER strip. `CompletionChronicleProjectionTests`; `--settings-selftest` unchanged; `PanelRouteGateTests` updated for the new strip.

**Commit 1.4 — XP-05 fuel grades + binder:**
`fuel_grades.json`, `FuelGradeCatalog`, `SofcFuelBinder` replacing the placeholder lambda — the binder lives Core-side; the inventory owner supplies `TryConsume` via an injected `IFuelInventory` port (new port registered in `docs/ci/port_contract_policy.json`, HOST_REQUIRED, with active `src/` caller). `SofcFuelConsumerBindingTests` (full-tank placeholder parity; per-tick consumption math).

**Commit 1.5 — XP-05 starvation + rationing:**
`FuelRationingQueue` (fixed authored order: life-support → food → security → industry → comfort), hysteresis (restart needs 2 units), load-shed thresholds wired to `PowerGridSystem` room priority. `SofcStarvationTests`, `FuelRationingOrderTests`. `EnergyHostSession` + `SofcPanel` strip + refuel preflight (mirrors `RescueDispatchPreflight` advisory shape).

**W1 DoD:** suite green including 5 new suites; `--data-integrity-selftest` green with `difficulty_presets.json` + `fuel_grades.json`; V+1 migration fixture; port contract regenerated (`generate-port-contract.py`, 2 new seams); `KNOWN_DEBT.md`: XP-01 rows propose retirement of `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY`, XP-05 closes the handoff follow-up.

---

### WAVE 2 · XP-04 Economy legs

**Premise checks:**

- [ ] Confirm black-market trade actions still lack funds/goods legs (grep action ids)
- [ ] Confirm Plan 147 day-gating is the only restock constraint
- [ ] Confirm `EconomyMarketRumorRules` additive-kind pattern (`MarketRumor = 6`) for the premium-composition commit

**Commit 2.1 — `FundsLedger` (Core, zero consumers):** ledger + bounded movement log (128, ids-only — copy `MedicalRecordLog` rules verbatim: day + amount + reason key + counterparty, oldest-first eviction, campaign-scoped). `FundsLedgerTests` (atomicity, eviction, restore fidelity).

**Commit 2.2 — black-market legs:** `black_market_actions.json`, `BlackMarketTradeActions` with the four actions (`bm_buy`, `bm_sell`, `bm_fence`, `bm_contract`) each a two-leg transaction: debit/credit `FundsLedger` + inventory move, both-or-neither (transactional helper `TryExecuteLegs` — on any leg failure, no state change; test asserts no partial application). `BlackMarketTradeLegTests` (all failure states from Part 1 table).

**Commit 2.3 — heat + purity:** `BlackMarketHeatStore` (per-faction-market, persisted, relocation draw from `bm_heat` sub-stream, anti-reroll), `FencePurityRolls` (per-instance persisted roll; appraisal affects display only). `BlackMarketHeatTests`, `FencePurityTests`.

**Commit 2.4 — restock priority:** `merchant_restock_priority.json`, `MerchantRestockPriority` implementing largest-remainder allocation exactly as Part 1 §XP-04-F3 (integer math only — no floating point, guaranteeing cross-platform determinism; the tie-break is `restock_order` then lexicographic id). `MerchantRestockPriorityTests` (allocation, scarcity boost, tie-breaks, determinism under category permutation). `TradePanel` NEXT SUPPLY strip.

**Commit 2.5 — funds host + surfaces:** `FundsHostSession` (save section `funds_ledger`), `BlackMarketHostSession`, opt-in flag wiring for black market + caravan trade panels, funds readout, `ATTENTION` heat wording. V+2 migration fixture. Extend `EconomyProbeTests` with funds-leg arbitrage probes (the standing exploit gate).

**W2 DoD:** 4 new suites green; V+2 migration; port contract +2 seams; debt row for the handoff §5 black-market/restock items proposed for retirement.

---

### WAVE 3 · XP-02 Graph travel

**Premise checks:**

- [ ] Enumerate the ten orphan `loc_*` nodes (from D9) and confirm none gained records
- [ ] Confirm expedition + caravan estimate code paths (the multiplier composition sites to replace)
- [ ] Confirm `WastelandMapSystem` edge shape for additive fields

**Commit 3.1 — topology vocabulary + orphan resolution [IMPL-CHOICE: stubs + gate]:** additive `condition`, `passage_class`, `surveyed_by_default` on edges; authored stub records for the ten orphans; loader gate in `CatalogIntegrityValidator` (edge referencing unknown `loc_*` = catalog error — this is the D9 "both" choice, implemented). `RouteTopologyCatalogTests`.

**Commit 3.2 — planner (stateless):** `GraphTravelPlanner` — Dijkstra with integer-scaled costs (`distance_km × 100` scaled condition multipliers; all integer arithmetic end-to-end, no doubles, for determinism). `RoutePlan` carries ordered edges, cost, condition report, blocked-reasons, fallback adjacency plan. `GraphTravelPlannerTests` (shortest path, unreachable-with-reasons, condition costs, permutation determinism).

**Commit 3.3 — survey knowledge:** `SurveyKnowledgeStore` (edge → survey count, persisted section `graph_survey`), uncertainty bands `±35%`/`±12%`/exact as display + planning modifiers; survey roll from `graph_survey` sub-stream, persisted anti-reroll. `SurveyKnowledgeTests`.

**Commit 3.4 — expedition + caravan bind (the DECISION-BLOCKED migration, executed):**
1. `ExpeditionSystem` dispatch estimate consumes planner; **parity guard**: when all edges on the route are `clear` and surveyed, estimated days must equal the legacy formula output — `ExpeditionGraphTravelTests` asserts this equality on a fixture map, so the migration cannot silently rebalance the live game.
2. Runtime pathing consumes the same plan; plan locked at dispatch; re-planning only at waystations.
3. Caravan route selection with `PassageClass.Vehicle`; flooded-edge reroute-or-advisory test.
4. Composition order implemented as Part 1 §XP-02.3 (graph cost → vehicle mult → party speed mult; difficulty excluded).
Aviation/naval stay legacy in W3 (Plan 32C follow-up debt row opened, promotion condition = W4 landing).

**Commit 3.5 — dynamic closure seam:** `EdgeConditionChanged` event (registered in event-surface test first), producers: sump flood (adjacent edges → `flooded`), weather cascade (`ash-choked`, expiry via `maintenance_day`), war front (deferred producer to W4 — the event type + seam land now, front producer lands in W4). `GraphTravelHostSession`; V+3 migration; `ExpeditionPanel` route report (condition words, never color-only).

**W3 DoD:** 4 new suites + parity fixture green; V+3 migration; `DEBT-PLAN32-GRAPH-TRAVEL` and `DEBT-PLAN32-MAP-ORPHANS` proposed for retirement with evidence; `DEBT-XP-32C-AVIATION-NAVAL` opened (bounded, promotion condition stated).

---

### WAVE 4 · XP-03 World consequences

**Premise checks:**

- [ ] Re-verify the war-chain authored window (480–607) vs live window (180–360) — the two DECISION-BLOCKED premises
- [ ] Enumerate the five unconsumed events (`territorial clash`, `decree`, `stage`, `chain`, `OnChainResolved`) and confirm still zero consumers
- [ ] Confirm `EconomyMarketRumorRules` band-kind count (currently 7 with `MarketRumor = 6` — new kinds append `WarShock = 7`, `ExportBan = 8`, `Confidence = 9`, additive, existing values untouched)

**Commit 4.1 — horizon:** `world_clock_horizon.json`, `WorldClockHorizon` implementing `linear_compress` with 2-day storytelling floor and deterministic overflow (beats that would land inside the floor push to the next free day, ordering by authored sequence). `WorldClockHorizonTests` (monotonic, floor, overflow determinism, authored-window boundary cases 480 and 607).

**Commit 4.2 — economy route:** `WarChainEconomyRoute` subscribing to the three re-based events; new band kinds 7–9 in `EconomyMarketRumorRules`; exactly-once fired-set persisted in `world_consequences` section; restore-no-replay (the market-rumor band rule verbatim). `WarChainEconomyRouteTests`.

**Commit 4.3 — expedition route:** war-front producer for `EdgeConditionChanged` (the W3 seam's deferred producer — front movement blocks/clears corridors); in-transit expeditions crossing a newly blocked edge get the waystation decision (reroute/hold/abort) as a decision event on the existing expedition encounter bridge. `WarChainExpeditionRouteTests`.

**Commit 4.4 — radio + chronicle routes:** `WarChainRadioRoute` — decree/stage events queue one inject item each, priority below emergency alerts, into the existing radio schedule inject owner (no new injection authority — sealed rule from `DEBT-194`). `WarChainChronicleRoute` — `OnChainResolved` writes exactly one journal epilogue line + one chronicle entry via the Plan 178 `RecordArchiveChronicle` host command. `WarChainRadioRouteTests`, `WarChainResolvedChronicleTests`.

**Commit 4.5 — widget + host:** `WorldConsequenceHostSession`; `FactionWarMapWidget` clash markers (words + glyph, a11y-gated); reload-replay fixture `WarChainReloadReplayTests` (continuous vs mid-reload equality across all four routes — the Plan 166–169 pattern).

**W4 DoD:** 5 new suites green; V+4 migration; both `DEBT-PLAN30-*` rows proposed for retirement **with the signed horizon choice recorded in the closeout** (the rows are DECISION-BLOCKED — the foreman signs `linear_compress` before 4.1 lands; implementation does not proceed on an unsigned horizon).

---

### WAVE 5 · XP-06 Body integrity + XP-09 Presenter skills + XP-10 Phobias

Three independent pillars; one wave (one envelope bump), three sub-batches landing sequentially.

**XP-06 sub-batch (schema wave — the named C2 blocker):**

1. *Premise:* confirm `ItemDefinition`/`EquipSlot` has no limb fields; confirm the sealed amputation multiplier API shape.
2. Commit 5.1 — schema: additive optional `limb_requirements`, `strength_requirement` on `ItemDefinition`; `provides_limb` on prosthetic items; `body_state` limb map on survivor records (migration: all intact). `prosthetics.json` (6 launch items from Part 1 table). `LimbRequirementSchemaTests` (old catalogs deserialize byte-identical).
3. Commit 5.2 — equip gate: `LimbRequirementGate` as a read-model over `AmputationSystem`; runs at equip time and on every `body_state` change (amputation event auto-unequips violating items + journal line — journal line is a *state consequence*, not presentation-only simulation, C2-safe). Grip classes: `simple`/`full` only [IMPL-CHOICE]. `EquipLimbGateTests`.
4. Commit 5.3 — rehabilitation + condition: `ProstheticRehabilitationArc` (fitting/adaptation/mastery phases, daily tick beside `TickSharedSkillProgression` — same host owner, `ShelterFacilitiesDayOwner` ordering documented); prosthetic decay through `EquipmentConditionSystem`; failed prosthetic = amputated-equivalent. `ProstheticsCatalogTests`, `RehabilitationArcTests`, `ProstheticConditionTests`.
5. `BodyIntegrityHostSession`; `SurvivorDetailPanel` limb slots. The `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` equipment half proposes retirement.

**XP-09 sub-batch:**

1. Commit 5.4 — catalog + skills: `presenter_skills.json` (5 skills), `PresenterSkillCatalog`; XP accrual hooks into the live `SkillProgressionSystem` host tick (no second progression engine). `PresenterSkillCatalogTests`.
2. Commit 5.5 — quality + archetypes: `BroadcastQualityEvaluator` (Part 1 formula; integer-scaled 0–100 internally, converted to band at the boundary — no float persistence); roll from `radio_presenter` sub-stream, persisted per program instance, anti-reroll; 4 archetypes one-time choice. `BroadcastQualityTests`, `PresenterArchetypeTests`.
3. Commit 5.6 — fatigue + audience loop: `PresenterFatigueStore` (accrual/decay per Part 1 table; stress entry via existing mental-health vocabulary at 5+ load); listenership/morale/PsyOps effects exactly-once per airing, persisted fired-set. `PresenterFatigueTests`, `AudienceResponseTests`. `RadioPanel` presenter strip. Radio directory suite count updated once.

**XP-10 sub-batch:**

1. Commit 5.7 — phobia stores: `phobias.json` (8-entry closed vocabulary + personality weights), `PhobiaAcquisitionRules` (roll from `psychology` sub-stream, anti-reroll, existing-phobia damping), `PhobiaIntensityStore` (levels 1–4, growth/decay, 0-for-14-days removal). `PhobiaAcquisitionTests`, `PhobiaIntensityTests`.
2. Commit 5.8 — exposure + AI bridge: exposure arc (triggered/voluntary-supported/therapeutic — therapeutic as a new treatment kind in the existing medical pipeline, no new psychology system [IMPL-CHOICE]); `PhobiaUtilityConsiderations` as authored entries in the live UtilityAI catalog (consideration curves data-driven; crisis-alert necessity override; debug overlay logs arbitration via the existing UtilityAI debug surface). `PhobiaExposureArcTests`, `PhobiaUtilityAiBridgeTests`.
3. Commit 5.9 — earned traits: `earned_traits.json` (4 traits), `EarnedTraitGranter` via the chronicle-milestone hook pattern, exactly-once, never removed. `EarnedTraitTests`.
4. Cross-pillar test: `PhantomPainSleepClassificationTests` — XP-06 phantom-pain events classify as insomnia-class sleep beats through the sealed `SleepNarrativeProjection` (read-only extension of the classification input, no new projection).

**W5 DoD:** 12 new suites green; V+5 migration (4 sections); Plan 177/179 phobia-growth deferral and Plan 173 presenter-skill deferral debt rows proposed for retirement.

---

### WAVE 6 · XP-07 Provenance + XP-08 Trade routes & migration

**XP-07 sub-batch:**

1. Commit 6.1 — provenance model: additive nullable record on item instances (origin kind/day/actor/place — ids only, no free text; save-size probe extended: `SaveSizeBudgetTests` asserts < 2% envelope growth on the fixture campaign). `ItemProvenanceSchemaTests`.
2. Commit 6.2 — named items + reveals: `named_items.json` (24 items, trait envelope +15/−10 enforced at validation); `LoreFragmentRevealRules` consuming appraisal (Plan 191), condition (`EquipmentConditionSystem`), survey state (W3), chronicle milestones (Plan 178 hook), pairing. Each reveal exactly-once, persisted. `NamedItemsCatalogTests`, `LoreFragmentRevealTests`.
3. Commit 6.3 — interactions: heirloom claim (affinity-gated, grief chronicle entry via guilt/insomnia vocabulary), faction recognition of `looted` items (standing −4 via canonical `FactionStanceEngine`), memorial dedication (extends the sealed memorial→chronicle hook). Death/inheritance export writes exactly one chronicle entry — binds the live Plan 206 legacy flow, no new inheritance authority. `ProvenanceInteractionsTests`, `ProvenanceChronicleExportTests`. `InventoryDetailPanel` HISTORY rows (edit only — read-model).

**XP-08 sub-batch:**

1. Commit 6.4 — route contracts: `trade_route_templates.json`, `TradeRouteContractStore` + `RouteReliabilityLedger` (score math, tiers 1–4, suspend/cancel + 30-day cooldown). Runs dispatch as scheduled caravan missions through the existing caravan system (no new logistics layer). Tariffs through `FundsLedger` (W2). `TradeRouteContractTests`, `RouteReliabilityTests`.
2. Commit 6.5 — route risk: run pathing + risk through the W3 planner and `EdgeConditionChanged` seam; delay/loss outcomes deterministic. `RouteRiskBindingTests`.
3. Commit 6.6 — seasonal migration: `seasonal_migration.json`, `SeasonalMigrationScheduler` (population-weight deltas per region, deterministic daily tick, 10-day hysteresis dwell), `MigrationConsequenceApplier` (market availability multipliers, apprenticeship/caregiving candidate pools, ideological-friction event weights, escort/passage quest hooks into questline master). No NPC agents — population weights only [IMPL-CHOICE, cost-bounded]. `SeasonalMigrationTests`, `MigrationConsequenceTests`.
4. Commit 6.7 — reload-replay + panels: `TradeRouteMigrationReloadReplayTests` (both XP-07 persistence and XP-08 state, continuous vs mid-reload); `TradePanel` ROUTES strip; migration shown on the region map widget (same a11y rules).

**W6 DoD:** 8 new suites green; V+6 migration; Plans 190/192/199 mapped-then-implemented; final cross-pillar reload-replay soak (`XpFullStackReloadReplayTests` — one campaign fixture touching all ten pillars, continuous vs mid-reload equality).

---

## 4 · PORT CONTRACT & EVENT SURFACE CHANGES

### New ports (registered in `docs/ci/port_contract_policy.json`, all HOST_REQUIRED with active `src/` callers)

| Port | Wave | Consumer |
|---|---|---|
| `IFuelInventory.TryConsume` | W1 | `SofcFuelBinder` |
| `IFundsSurface.Debit/Credit` | W2 | black market, caravan trade, route tariffs |
| `IGraphEdgeOwner.GetEdges/SetCondition` | W3 | planner (read), closure producers (write via owner only) |
| `IProvenanceSource.GetOrigin` | W6 | reveal rules, interactions |

The planner itself never appears in the port contract as a host seam — it is pure Core, called by Core systems.

### New event types (event-surface registry, registered before first emit)

| Event | Wave | Emitters | Subscribers |
|---|---|---|---|
| `EdgeConditionChanged` | W3 | sump/weather; W4 war front | planner read-model invalidation, XP-08 route risk, panel refresh |
| `BodyStateChanged` | W5 | amputation, prosthetic fit/fail | equip gate, expedition multiplier, panels |
| `ProgramAired` (extends existing radio event if present — premise check first) | W5 | radio production | presenter XP, fatigue, audience loop |
| `PhobiaAcquired` / `TraitEarned` | W5 | phobia/trait stores | UtilityAI consideration refresh, journal, chronicle |

Every subscriber registration gets one row in `EventSurfaceArchitectureTests`; orphan events (emitted, zero subscribers) fail the gate — this codifies the Plan 30 lesson (unconsumed projection events) as a mechanical check for all new work.

---

## 5 · DETERMINISM & RNG SUB-STREAM REGISTRY

| Sub-stream | Wave | Used by | Anti-reroll |
|---|---|---|---|
| `graph_survey` | W3 | survey outcome rolls | Yes — first result persists |
| `bm_heat` | W2 | market relocation draws | Yes |
| `war_chain` | W4 | clash region selection | Yes, per campaign |
| `radio_presenter` | W5 | broadcast quality rolls | Yes, per program instance |
| `psychology` | W5 | phobia acquisition rolls | Yes, per trigger event |

Rules enforced by extended `DeterminismGuardTests`:

1. All new math integer-scaled (×100) end-to-end; floats permitted only at the presentation boundary, never persisted.
2. `DeterminismGuardTests` source-scan extended to the new directories (`WorldConsequences/`, `Energy/`, `BodyIntegrity/`, `TradeRoutes/`, `PresenterSkills/`, `Phobia/`, `Provenance/`).
3. Every persisted roll store round-trips (roll → persist → reload → same value) — asserted in each wave's reload-replay fixture.

---

## 6 · SELFTEST FLAGS & CI GATES

### New host CLI flags (all documented in `HostCli.PrintHelp` — the sealed CLI-help contract rule; add the flag and its help line in the same commit)

| Flag | Wave | Asserts |
|---|---|---|
| `--difficulty-selftest` | W1 | Preset catalog loads; 7 seams parity with provider unset; chronicle projection aggregates |
| `--fuel-selftest` | W1 | Fuel grades resolve; SOFC burn/starve cycle on fixture campaign; rationing order |
| `--funds-selftest` | W2 | Ledger round-trip; black-market leg transactionality; restock allocation determinism |
| `--graph-travel-selftest` | W3 | Planner on fixture map; parity vs legacy estimates; survey bands |
| `--world-consequence-selftest` | W4 | Horizon re-basing; four routes exactly-once; no restore replay |
| `--body-integrity-selftest` | W5 | Equip gate; rehab arc; prosthetic decay cycle |
| `--presenter-selftest` | W5 | Skill accrual; quality bands; fatigue cycle |
| `--phobia-selftest` | W5 | Acquisition formula; intensity arc; AI consideration curves |
| `--provenance-selftest` | W6 | Reveal rules; interactions; chronicle export |
| `--trade-routes-selftest` | W6 | Contract cadence; reliability tiers; migration schedule |

### Existing gates extended

| Gate | Change |
|---|---|
| `--data-integrity-selftest` | New catalogs (12 files) with cross-reference rules (fuel item ids resolve; prosthetic chains resolve; named-item base ids resolve; route counterparty ids resolve; phobia personality weights reference existing traits) |
| `--ui-layout-selftest` | New strips (LEDGER, NEXT SUPPLY, ROUTES, presenter, limb, HISTORY, clash markers) snapshot rows |
| `--settings-selftest` | Colorblind mapping covers new status words |
| `--playable-shell-selftest` | Navigation path touches: preset selection (campaign start), funds readout, SOFC refuel, route panel |
| `CI_GATE_MANIFEST.json` | Each new selftest flag added to the fast gate battery |
| `generate-architecture-map.py` | New registry sections per Core domain directory (the `DEBT-ARCH-MAP-GENERATOR-DRIFT` lesson: update the generator graph **and** the map text in the same commit; `--check` must pass) |
| `generate-port-contract.py` | Regenerated per wave; new seams counted in the fast gate |

### Test-suite ledger (all new fixtures)

W1: 5 · W2: 4 · W3: 5 · W4: 5 · W5: 12 · W6: 9 — **40 new suites**, each with premise-proof, happy-path, failure-state, determinism, and reload-replay cases. Every suite name appears in `ArchitectureTestMapGateTests` coverage (the map cites a fixture per new subsystem).

---

## 7 · REGRESSION RISK REGISTER (WHAT THIS PLAN COULD BREAK)

| Risk | Likelihood | Mitigation (built into the waves) |
|---|---|---|
| Difficulty scalars double-applied where an event already multiplies | Medium | Scalar applied at exactly one site per system (the seam); table-driven seam test asserts single application; seam list is closed at 7 — new consumers need a signed row |
| Graph migration silently rebalances expedition pacing | High if unguarded | W3 parity guard: all-clear + surveyed route ⇒ estimate equals legacy formula, asserted on fixture map; any divergence is a deliberate, signed rebalance |
| Save bloat from provenance + survey + phobia stores | Medium | `SaveSizeBudgetTests` per wave (< 2% growth on fixture campaign); ids-only records; bounded logs |
| Section-count contract drift (the recurring standing failure) | High | One constant bump per wave, same commit as codec registration; `ComprehensiveSaveStoreCorruptionAndMigrationTests` is a W-gate, not a cleanup |
| War bands replaying on reload (Plan 30-class bug) | Medium | Fired-set persisted before effect applied; restore-no-replay asserted in `WarChainReloadReplayTests` |
| New panels unreachable (`DEBT-PANEL-REACHABILITY-138` class) | Medium | Panel registration + switch case + `expandedIds` in one commit; the parity gate catches it mechanically |
| UtilityAI pathological loops from phobia avoidance | Low-Medium | Bounded weights, authored fallback, crisis override; arbitration logged through the existing debug overlay; soak scenario in `PhobiaUtilityAiBridgeTests` |
| Port contract drift (new seams uncalled or unregistered) | Low | Generator in fast gate per wave; HOST_REQUIRED seams must show active `src/` callers |
| EconomyProbe regression from funds legs enabling new arbitrage | Medium | W2 extends the probe battery *before* the panels exposing funds go live |
| Event-surface orphans (Plan 30 lesson) | Medium | New gate rule: emitted-but-unsubscribed new events fail `EventSurfaceArchitectureTests` |

---

## 8 · FOREMAN DECISION GATE (SIGN-OFFS, IN ORDER)

No wave starts before its decisions are signed (the repo's standing rule — DECISION-BLOCKED debts are not improvised around):

| # | Decision | Blocks | Proposal on the table |
|---|---|---|---|
| D1 | Difficulty authority: `difficulty_presets.json` + 7-seam list | W1 | Part 1 XP-01.3 |
| D2 | Fuel owner + kWh table + rationing order | W1 | Part 1 XP-05.3 |
| D3 | `FundsLedger` as canonical funds authority; wave-1 opt-in surfaces | W2 | Part 1 XP-04.3 |
| D4 | Restock ordering design (integer largest-remainder) | W2 | Part 1 XP-04-F3 |
| D5 | Plan 32B scope (expedition+caravan first) + multiplier order + D9 "both" | W3 | Part 1 XP-02 |
| D6 | Runtime horizon policy (`linear_compress`) + first consequence route (economy) | W4 | Part 1 XP-03 |
| D7 | `ItemDefinition` limb schema extension + grip classes | W5 | Part 1 XP-06 |
| D8 | Phobia vocabulary + earned-trait list + therapeutic-as-pipeline-kind | W5 | Part 1 XP-10 |
| D9 | Presenter skill domain + archetypes + fatigue coupling | W5 | Part 1 XP-09 |
| D10 | Plan 190 subset + trait envelope; Plans 192/199 scope (population-weight migration) | W6 | Part 1 XP-07/08 |

Recommended packet format: one `docs/governance/DECISION_PACKET_<date>.md` covering D1–D10 with per-decision evidence pointers to Part 1 sections, mirroring the 2026-09-18 packet structure.

---

## 9 · EXECUTION CALENDAR & EFFORT MODEL

| Wave | New suites | Save bumps | Est. sessions (foreman+integrator) | Exit criterion |
|---|---|---|---|---|
| W1 | 5 | V+1 | 2 | DoD + full suite green + debt rows proposed |
| W2 | 4 | V+2 | 2 | same |
| W3 | 5 | V+3 | 2–3 | parity fixture is the long pole |
| W4 | 5 | V+4 | 2 | horizon decision is the long pole |
| W5 | 12 | V+5 | 3 | three sub-batches, schema review for XP-06 |
| W6 | 9 | V+6 | 2–3 | full-stack reload-replay soak closes the program |

Total: 40 suites, 6 envelope versions, ~14 sessions. Waves are independently shippable: if any wave's decision stalls, subsequent waves re-order around it (only W3→W4→W6-risk and W2→W6-tariff edges are hard dependencies; W1, W5 are fully independent and can start immediately after D1/D2/D7/D8/D9).

**End of Part 2.**