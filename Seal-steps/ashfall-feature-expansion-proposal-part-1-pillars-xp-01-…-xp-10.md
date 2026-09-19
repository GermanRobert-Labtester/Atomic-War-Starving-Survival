# ASHFALL — PURPOSED FEATURES EXPANSION · PART 1

**Document type:** Feature expansion proposal (spec-form, not prose)
**Scope:** Pillars XP-01 … XP-10 — systems that currently lack features or are decision-blocked, evidenced against the live repository state
**Companion:** Part 2 (XP-11 … XP-20) will follow in a second document
**Conventions honored:** Godot 4.7.1 .NET host · engine-agnostic `Ashfall.Core.<Domain>` · `snake_case` ids · `IEventBus` state changes · `CaptureState`/`RestoreState` persistence · `ISeededRng` only (no `System.Random`/`Guid.NewGuid()`) · `Assets/StreamingAssets/Data/` is the sole data authority · checksummed save envelopes with version migration · catalog selftests as acceptance gates

---

## 0 · HOW TO READ THIS DOCUMENT

Every pillar uses the same record structure so it can be lifted directly into `Next-steps-plans/` plan files or an `INTEGRATION_PLANS.md` batch:

| Field | Meaning |
|---|---|
| `GAP-ID` | Stable identifier, referenced by Part 2 and by proposed debt rows |
| `Current state` | What exists today, with file/evidence pointers |
| `Missing features` | Enumerated feature list, each with its own id (`-F1`, `-F2`, …) |
| `Design` | Mechanics, formulas, data shapes — tables and code blocks, never prose paragraphs |
| `Integration points` | Exact existing systems/seams the feature binds to |
| `Data catalogs` | New/amended JSON under `Assets/StreamingAssets/Data/`, with `schema_version` |
| `Persistence` | Save section or additive fields, migration path |
| `Presentation` | Panel strips, a11y rules, journal lines |
| `Balance derivation` | Why the numbers are what they are |
| `Acceptance` | Test-suite names + selftest flags that must pass |
| `Risks / exploits` | Degenerate strategies and guards |
| `Foreman decisions required` | The named decisions that currently block the debt row |

Part 1 pillars are chosen because each maps to a live, documented gap:

| Pillar | Title | Evidence source |
|---|---|---|
| XP-01 | Campaign Difficulty & Completion Chronicle Authority | `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` (DECISION-BLOCKED) |
| XP-02 | Graph-Native Travel & Route Topology | `DEBT-PLAN32-GRAPH-TRAVEL` (DECISION-BLOCKED) + `DEBT-PLAN32-MAP-ORPHANS` (PROMOTED) + flooded-route topology tags (handoff §5) |
| XP-03 | Autonomous World Consequence Layer | `DEBT-PLAN30-RUNTIME-CLOCK` + `DEBT-PLAN30-CONSEQUENCE-REACH` (both DECISION-BLOCKED) |
| XP-04 | Economy Legs: Funds Authority, Black Market, Restock Priority | Handoff §2/§5 deferred items |
| XP-05 | Fuel & Energy Consumption Completeness (SOFC seam) | Handoff §4 "highest-value follow-up" |
| XP-06 | Body Integrity: Handedness, Limb Requirements, Prosthetics | `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` (equipment half BLOCKED) |
| XP-07 | Item Lore & Provenance | Plan 190 mapped-but-unsigned |
| XP-08 | Trade Routes & Seasonal Migration | Plans 192/199 (unmapped, product decision pending) |
| XP-09 | Radio Presenter Skill Tree | Plan 173 Phase 3 closeout ("presenter skill tree remains deferred") |
| XP-10 | Phobia & Trait Growth Layer | Plan 177/179 closeout ("phobia growth stays DEFERRED to traits/catalog") |

---

# XP-01 · CAMPAIGN DIFFICULTY & COMPLETION CHRONICLE AUTHORITY

## XP-01.0 · Gap ID and status

- **GAP-ID:** `XP-01-DIFFICULTY-CHRONICLE`
- **Debt row addressed:** `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` (DECISION-BLOCKED, C2[12])
- **Blocking condition today:** "no canonical campaign difficulty ID/preset authority and no plan-defined completion-history chronicle consumer"; storing a display string or adding a second ending/panel authority would violate C2[12]

## XP-01.1 · Current state (evidence)

| Item | State |
|---|---|
| Completion history | Wave 11 B3 sealed user-level append-only completion history under `DEC-20` — exists, observation-only |
| Difficulty authority | None — no canonical difficulty ID, no preset catalog, no single owner |
| Chronicle consumer | None — no read surface consumes the completion history |

## XP-01.2 · Missing features

| Id | Feature | Notes |
|---|---|---|
| XP-01-F1 | Canonical `DifficultyPreset` catalog (`difficulty_presets.json`) | Single authority; ids like `difficulty_standard` |
| XP-01-F2 | `DifficultyDirector` core system | Reads preset scalars, exposes them to consuming systems through one typed surface |
| XP-01-F3 | Preset-conditional tuning seams in 6 consuming systems | Bounded, additive, provider-unset = legacy byte-identical |
| XP-01-F4 | Completion chronicle read-model (`CompletionChronicleProjection`) | Observation-only consumer of the sealed history |
| XP-01-F5 | Campaign start binding | Difficulty selected at new-campaign creation, persisted in campaign header, immutable |
| XP-01-F6 | Journal/panel surface for the chronicle | One panel strip; no second ending authority |

## XP-01.3 · Design

### Catalog shape (`difficulty_presets.json`, `schema_version: 1`)

```json
{
  "schema_version": 1,
  "presets": [
    {
      "id": "difficulty_sparing",
      "display_name": "SPARING",
      "description_key": "difficulty_sparing_desc",
      "scalars": {
        "hunger_rate_mult": 0.75,
        "thirst_rate_mult": 0.75,
        "radiation_gain_mult": 0.75,
        "disease_onset_mult": 0.8,
        "hostile_encounter_mult": 0.75,
        "market_price_mult": 0.9,
        "equipment_decay_mult": 0.8,
        "crisis_deadline_mult": 1.25
      },
      "starting_bonus_item_ids": ["item_canned_rations", "item_iodine"]
    },
    {
      "id": "difficulty_standard",
      "display_name": "STANDARD",
      "scalars": { "hunger_rate_mult": 1.0, "thirst_rate_mult": 1.0,
        "radiation_gain_mult": 1.0, "disease_onset_mult": 1.0,
        "hostile_encounter_mult": 1.0, "market_price_mult": 1.0,
        "equipment_decay_mult": 1.0, "crisis_deadline_mult": 1.0 },
      "starting_bonus_item_ids": []
    },
    {
      "id": "difficulty_austere",
      "display_name": "AUSTERE",
      "scalars": {
        "hunger_rate_mult": 1.35, "thirst_rate_mult": 1.35,
        "radiation_gain_mult": 1.3, "disease_onset_mult": 1.25,
        "hostile_encounter_mult": 1.35, "market_price_mult": 1.15,
        "equipment_decay_mult": 1.25, "crisis_deadline_mult": 0.8
      },
      "starting_bonus_item_ids": []
    },
    {
      "id": "difficulty_dirge",
      "display_name": "DIRGE",
      "scalars": {
        "hunger_rate_mult": 1.75, "thirst_rate_mult": 1.75,
        "radiation_gain_mult": 1.6, "disease_onset_mult": 1.5,
        "hostile_encounter_mult": 1.75, "market_price_mult": 1.3,
        "equipment_decay_mult": 1.5, "crisis_deadline_mult": 0.65
      },
      "starting_bonus_item_ids": []
    }
  ],
  "default_preset_id": "difficulty_standard"
}
```

### Rules

| Rule | Value |
|---|---|
| Preset selection point | New-campaign creation only; stored in campaign header; never editable mid-run |
| Unknown preset id on load | Load fails closed → campaign refuses to start with catalog-integrity error (matches malformed-envelope rejection posture) |
| Scalar consumption | Multiplicative on the consuming system's existing base value; preset absent (legacy saves) → scalar 1.0, behavior byte-identical |
| RNG | Preset id may seed an `ISeededRng` sub-stream name (`difficulty`) but never changes stream counts |
| C2[12] compliance | Difficulty is a **producer of scalars and a consumer of completion history only**; it owns no ending text, no panel authority beyond one strip, no second history store |

### Consuming seams (bounded set — one provider port per system)

| System | Seam | Effect |
|---|---|---|
| Needs/hunger-thirst tick | `DifficultyScalarsProvider.NeedsMult` | Multiplies daily deprivation increments |
| `RadiationSystem` | `DifficultyScalarsProvider.RadMult` | Multiplies dose gain per exposure event |
| `DiseaseSystem` | `DifficultyScalarsProvider.DiseaseMult` | Multiplies onset probability rolls (clamped ≤ 0.95) |
| Encounter arbitration (expeditions/caravan) | `DifficultyScalarsProvider.HostileMult` | Scales hostile-encounter weight band |
| `EconomyMarketRumorRules` / market pricing | `DifficultyScalarsProvider.PriceMult` | Multiplies buy/sell spreads symmetrically |
| `EquipmentConditionSystem` | `DifficultyScalarsProvider.DecayMult` | Multiplies condition loss per use-day |
| Crisis/deadline owners (rescue signals, faction ultimatums) | `DifficultyScalarsProvider.DeadlineMult` | Multiplies deadline windows |

Provider pattern mirrors the sealed `RoomPowerProvider` precedent: additive port, unset → legacy byte-identical, tested.

### Completion chronicle projection (XP-01-F4)

- New Core class `CompletionChronicleProjection` — **read-only** over the sealed append-only history.
- Produces per-preset aggregates: `{runs_started, runs_completed, endings_seen[], best_survival_days, shelters_fallen}`.
- Deterministic: pure function of history + preset ids; no RNG, no clock.
- Single consumer: one "CAMPAIGN LEDGER" strip on the campaign/endings panel; words never color-only (a11y gate).

## XP-01.4 · Integration points

- `Main.cs` campaign creation flow → preset selection + header write
- Save envelope: additive `difficulty_preset_id` field on the campaign header section; legacy saves default `difficulty_standard`; no version bump required if additive-optional, else V+1 with migration mapping missing → default
- `CatalogIntegrityValidator`: preset id uniqueness, scalar ranges (0.25–2.5), default exists
- Panel: campaign-start overlay preset list + chronicle strip

## XP-01.5 · Acceptance

| Gate | Requirement |
|---|---|
| `DifficultyPresetCatalogTests` | Catalog loads, ids unique, ranges valid, default present |
| `DifficultyDirectorTests` | Scalar resolution, legacy default, fail-closed unknown id |
| `DifficultyScalarSeamTests` | Each of the 7 seams: provider unset → byte-identical legacy result |
| `CompletionChronicleProjectionTests` | Deterministic aggregation, empty history, multi-run history |
| `--data-integrity-selftest` | Stays green with new catalog |
| Save round-trip | `ComprehensiveSaveStore…` section count updated once, additive field migrates cleanly |

## XP-01.6 · Balance derivation

- Presets are **multiplier envelopes, not new content**: every scalar exists to shorten or widen decision windows, never to add new failure modes.
- `difficulty_austere` targets: same median time-to-first-crisis as standard but ~30% tighter resource slack (starvation margin), so experienced players feel pressure without learning new rules.
- `difficulty_dirge` targets: a run where a stable shelter is achievable but a *comfortable* shelter is not — decay and crisis deadlines dominate late game.
- Symmetric price multiplier (buy and sell) prevents difficulty preset from becoming an arbitrage lever.

## XP-01.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Save-file preset editing for easier mid-run game | Header is checksummed; mutation invalidates envelope |
| Preset scalars stacking with future event multipliers | Scalars compose multiplicatively through the single provider; consuming systems must not double-apply (seam test asserts single application) |
| Chronicle becomes a second stats authority | Projection is read-only over DEC-20 history; no writes, no separate save section |

## XP-01.8 · Foreman decisions required

1. Sign `difficulty_presets.json` as the single difficulty authority (C2[12]-compliant producer/consumer split).
2. Sign the 7-seam consumer list (any system outside the list stays untouched).
3. Sign additive header field vs. save-version bump.

---

# XP-02 · GRAPH-NATIVE TRAVEL & ROUTE TOPOLOGY

## XP-02.0 · Gap ID and status

- **GAP-ID:** `XP-02-GRAPH-TRAVEL`
- **Debt rows addressed:** `DEBT-PLAN32-GRAPH-TRAVEL` (DECISION-BLOCKED), `DEBT-PLAN32-MAP-ORPHANS` (PROMOTED — ten `loc_*` nodes without `locations.json` records), flooded-route topology tags (handoff §5, "map owner's authored edges")
- **Blocking condition today:** "Expedition and caravan do not consume `WastelandMapSystem`; graph-native travel would touch expedition, aviation, naval, caravan, and save/multiplier composition"

## XP-02.1 · Current state (evidence)

| Item | State |
|---|---|
| `WastelandMapSystem` | Exists as the canonical graph of `loc_*` nodes and route edges |
| Expedition routing | Distance/danger estimates computed without the graph (legacy multiplier composition) |
| Caravan routing | Same — parallel, non-graph distance logic |
| Aviation / naval | Separate traversal models, not graph-bound |
| Map hygiene | Ten orphan `loc_*` nodes with edges but no canonical `locations.json` record (D9 signed choices pending) |
| Flooded-route tags | Authored edges absent; traversal reachability (Plan 125) cannot express flood closure |

## XP-02.2 · Missing features

| Id | Feature |
|---|---|
| XP-02-F1 | `RouteTopology` edge vocabulary: `condition` (clear, degraded, flooded, irradiated, ash-choked, blocked), `maintenance_day` stamp, `passage_class` (foot, vehicle, boat, air) |
| XP-02-F2 | `GraphTravelPlanner` — single Core route planner (Dijkstra on edge cost) consumed by expedition, caravan, and future legs |
| XP-02-F3 | Edge-cost model with condition/weather modifiers feeding the existing travel-time multiplier composition |
| XP-02-F4 | Orphan-node resolution: authored stub records or loader gate for the ten `loc_*` nodes |
| XP-02-F5 | Dynamic edge closure: sump flood, storm, and war-front events can set/clear edge conditions |
| XP-02-F6 | Geographic-knowledge gating: routes the player has not surveyed cost-estimate with uncertainty bands |
| XP-02-F7 | Aviation/naval binding as `passage_class` filters, not separate routers |

## XP-02.3 · Design

### Edge vocabulary (additive fields on the map catalog, `schema_version` bump)

```json
{
  "from": "loc_quarry_road_gate",
  "to": "loc_river_crossing",
  "distance_km": 14,
  "base_danger": 0.35,
  "condition": "degraded",
  "passage_class": ["foot", "vehicle"],
  "surveyed_by_default": false
}
```

| `condition` | Cost multiplier | Encounter band shift | Notes |
|---|---|---|---|
| clear | 1.00 | — | baseline |
| degraded | 1.15 | +5% hostile weight | unmaintained roads |
| flooded | 1.60 | +10%, vehicle blocked | set by `SumpFloodingSystem`/river state |
| irradiated | 1.40 | dosimeter advisory | ties to `RadiationSystem` zone data |
| ash-choked | 1.25 | visibility penalties | ties to Weather/Year-of-Ash |
| blocked | ∞ (impassable) | — | war-front, collapse events |

### Planner contract

```csharp
public sealed class GraphTravelPlanner
{
    // Single Core authority. Deterministic; no RNG.
    public RoutePlan Plan(string originId, string destinationId,
        PassageClass requested, TravelContext ctx);
    // RoutePlan: ordered edges, total cost, per-edge condition report,
    // blocked-reason list, fallback origin-adjacency plan if unreachable.
}
```

- Edge cost = `distance_km × condition_mult × weather_mult(ctx) × party_speed_mult`.
- `party_speed_mult` consumes the sealed amputation multiplier (`ExpeditionState.survivorSpeedMultiplier`) — no second movement authority.
- Unreachable destination returns a **plan with reasons**, never an exception; expedition dispatch preflight already speaks advisory language.

### Multiplier composition order (the signed decision this pillar proposes)

`estimated_days = graph_cost / (vehicle_mult × party_speed_mult) × difficulty_deadline_independent`
— i.e. graph cost first, then the two existing multipliers, difficulty never touches travel time (XP-01 deliberately excludes travel seams).

### Dynamic closure (XP-02-F5)

| Event producer | Edge effect | Clearance |
|---|---|---|
| `SumpFloodingSystem` node flooded | Incident adjacent edges → `flooded` | Manual: pump-out restores prior condition |
| Storm/weather cascade | `ash-choked` for N days | Auto-expires via `maintenance_day` |
| Faction war front movement | `blocked` corridor | Front retreat event re-opens |

All set/clear goes through `IEventBus` + `CaptureState`/`RestoreState`; restore never replays events (mirrors the market-rumor band rule).

### Survey/knowledge gating (XP-02-F6)

| Survey state | Estimate shown |
|---|---|
| Unsurveyed edge | Cost band `±35%`, danger shown as "UNMAPPED" |
| Once-surveyed | `±12%` |
| Twice+ or scouted by beacon | Exact |

Surveyed-state rides the existing field-guide/collectibles discovery persistence pattern; no new save section.

## XP-02.4 · Integration points

- `ExpeditionSystem` dispatch estimate + runtime pathing consume `GraphTravelPlanner`
- `CaravanPatrol` route selection consumes the same planner with `PassageClass.Vehicle`
- Aviation: filter edges to `passage_class: air` synthetic edges; naval: `boat`
- `WastelandMapSystem` remains node/edge owner; planner is a stateless read-model
- `DosimeterCalibrationSystem`/piezometer advisories feed `irradiated`/`flooded` conditions

## XP-02.5 · Acceptance

| Gate | Requirement |
|---|---|
| `GraphTravelPlannerTests` | Shortest path, condition costs, unreachable-with-reasons, determinism |
| `RouteTopologyCatalogTests` | Edge vocabulary validity, no orphan `loc_*` without records (loader gate variant of D9) |
| `ExpeditionGraphTravelTests` | Estimate parity with legacy for all-clear maps (byte-identical baseline), divergence only when conditions non-clear |
| `CaravanGraphTravelTests` | Vehicle-blocked flooded edges reroute or fail with advisory |
| Reload-replay | Continuous vs mid-reload route equality (Plan 166–169 pattern) |

## XP-02.6 · Balance derivation

- Condition multipliers are tuned so a flooded detour (typically +40–70% cost) is *cheaper in expectation* than waiting 3 days for pump-out when the cargo is perishable, but more expensive for durable goods — creating a per-mission decision, not a universal rule.
- Uncertainty bands (`±35%`) are wide enough that scouting has material value early, and vanish once the player invests in surveying — an exploration tax that rewards commitment.

## XP-02.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Route oscillation when conditions flip daily | Plan locked at dispatch; re-planning only at waystations |
| Save-scumming survey results | Survey outcomes drawn from a persisted, seeded sub-stream; first result persists (rescue-signal anti-reroll precedent) |
| Planner becoming a second map authority | Planner is stateless; it may not mutate edges or own locations |

## XP-02.8 · Foreman decisions required

1. Sign Plan 32B scope: which legs (expedition, caravan) migrate first; aviation/naval may follow as 32C.
2. Sign the multiplier-composition order above.
3. Sign D9 choice for the ten orphan nodes: this pillar proposes **authored stub records + loader gate (both)**.

---

# XP-03 · AUTONOMOUS WORLD CONSEQUENCE LAYER

## XP-03.0 · Gap ID and status

- **GAP-ID:** `XP-03-WORLD-CONSEQUENCE`
- **Debt rows addressed:** `DEBT-PLAN30-RUNTIME-CLOCK` (DECISION-BLOCKED), `DEBT-PLAN30-CONSEQUENCE-REACH` (DECISION-BLOCKED)
- **Blocking condition today:** war-chain narrative authored for days 480–607 while the live campaign path gates days 180–360; four projection events (`territorial clash`, `decree`, `stage`, `chain`) plus `OnChainResolved` lack player-facing consumers; `OnFactionStandingChanged` is consumed only by `FactionWarMapWidget`

## XP-03.1 · Current state (evidence)

| Item | State |
|---|---|
| War-chain narrative corpus | Authored, days 480–607 |
| Live campaign day window | 180–360 |
| War-chain events emitted | Yes, via `IEventBus` |
| Consumers of territorial clash/decree/stage/chain | None |
| `OnChainResolved` consumer | None |

## XP-03.2 · Missing features

| Id | Feature |
|---|---|
| XP-03-F1 | Runtime-horizon decision implementation: a `WorldClockHorizon` config that re-bases the war chain onto the live day window |
| XP-03-F2 | First consequence route: **economy** — war-front events move market availability/prices via the existing rumor-band bridge |
| XP-03-F3 | Second consequence route: **expedition** — front movement re-routes/blocks map edges (binds XP-02-F5) |
| XP-03-F4 | Third consequence route: **radio** — decree/stage events inject broadcast items through the schedule (radio inject stays radio schedule, per sealed rule) |
| XP-03-F5 | `OnChainResolved` consumer: campaign journal epilogue line + chronicle entry (rides Plan 178 chronicle hook pattern) |
| XP-03-F6 | Faction-war map widget enrichment: standing deltas already consumed; add clash markers with words-not-color a11y |

## XP-03.3 · Design

### Runtime horizon (XP-03-F1) — proposed re-basing contract

```json
{
  "schema_version": 1,
  "horizon": {
    "authored_window": { "first_day": 480, "last_day": 607 },
    "live_window": { "first_day": 180, "last_day": 360 },
    "rebase_policy": "linear_compress",
    "anchor": "chain_start_day"
  }
}
```

| Policy | Behavior |
|---|---|
| `linear_compress` | Authored day *d* maps to `180 + (d-480) × (180/127)`; ordering preserved, gaps compressed |
| `late_anchor` | Chain start pinned to day 300; authored deltas preserved |
| `authoritative_extend` (rejected default) | Live window extended to 607 — rejected because Year-of-Ash gating is a campaign invariant |

Recommended: `linear_compress` with a **storytelling floor** of 2 live days between adjacent chain beats (no double-events on one day; overflow beats push to the next free day, deterministic).

### Consequence route: economy (XP-03-F2)

| War event | Market effect | Mechanism |
|---|---|---|
| territorial_clash(region) | Supply shock on region-tagged goods, ±18–30%, 5–9 day band | New deterministic band kinds in the sealed `EconomyMarketRumorRules` pattern (mirrors `MarketRumor = 6` additive-kind precedent) |
| decree(faction, kind) | Export ban: affected goods unsellable to that faction 10 days | Availability flag on trade sessions |
| chain_stage(major) | Confidence shock: all prices +8% for 3 days | Symmetric multiplier, no arb |
| chain_resolved(outcome) | Regime pricing table applied to war-relevant goods | Outcome-keyed catalog |

- Exactly-once application, persisted fired-set (Plan 167 routing precedent).
- Bands never replay on restore.

### Consequence route: expedition (XP-03-F3)

- Front movement emits `EdgeConditionChanged(blocked|clear)` through XP-02-F5's edge-closure seam.
- Active expeditions crossing a newly blocked edge receive a **waystation decision**: reroute (cost +, days +), hold (supplies drain), or abort (salvage partial). This is a decision event, not an automatic fail — failure stays interesting.

### Consequence route: radio (XP-03-F4)

- Decree/stage events queue one broadcast item each into the radio schedule's inject queue (existing injection owner untouched; war events are just a new producer with priority below emergency alerts).

## XP-03.4 · Integration points

- `FactionWarChainRunner` (event producer) → route adapters (economy band, edge closure, radio inject, journal)
- `EconomyMarketRumorRules` additive band kinds
- `WastelandMapSystem` edge condition owner (via XP-02 seam)
- Radio schedule inject queue
- Chronicle hook (`RecordArchiveChronicle` pattern) for `OnChainResolved`

## XP-03.5 · Acceptance

| Gate | Requirement |
|---|---|
| `WorldClockHorizonTests` | Re-basing monotonic, floor respected, deterministic overflow ordering |
| `WarChainEconomyRouteTests` | Bands applied exactly once, persisted, no restore replay |
| `WarChainExpeditionRouteTests` | Blocked-edge waystation decision; reroute math via planner |
| `WarChainRadioRouteTests` | Inject priority below emergency; one item per event |
| `WarChainResolvedChronicleTests` | Epilogue journal line + chronicle entry exactly once |
| Reload-replay | Continuous vs mid-reload equality for all four routes |

## XP-03.6 · Balance derivation

- Price shocks (18–30%) are sized against the sealed market's existing rumor band magnitude so war becomes the *largest* external price mover without exceeding player-side arbitrage tools — the player cannot cancel a war, but stockpiling before a clash is a legitimate, rewarded read of the war map.
- The 2-day storytelling floor keeps late-campaign pacing readable when 127 authored days compress into 180.

## XP-03.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Compressed chain stacks multiple shocks on one day | Floor + deterministic overflow |
| War-driven price shocks exploited via foreknowledge of authored corpus | Clash day is re-based deterministically but region selection draws from a persisted seeded sub-stream per campaign |
| Blocked edges soft-lock caravans | Planner returns reroute-with-reasons; at least one fallback corridor stays `clear` by authored constraint |

## XP-03.8 · Foreman decisions required

1. Sign the runtime-horizon policy (`linear_compress` recommended).
2. Sign the first consequence route (this pillar proposes economy; expedition/radio follow).
3. Sign the waystation decision vocabulary for in-transit expeditions.

---

# XP-04 · ECONOMY LEGS: FUNDS AUTHORITY, BLACK MARKET, RESTOCK PRIORITY

## XP-04.0 · Gap ID and status

- **GAP-ID:** `XP-04-ECONOMY-LEGS`
- **Debt rows addressed:** handoff §2/§5 deferred items — black-market trade actions (funds/goods legs undefined, "needs a canonical-authority decision") and merchant restock "priority" ("needs a signed ordering design; restock is already day-gated per Plan 147")

## XP-04.1 · Current state (evidence)

| Item | State |
|---|---|
| Black market | Exists as a market surface; **trade actions' funds/goods legs are undefined** — no canonical funds authority |
| Merchant restock | Day-gated (Plan 147 sealed); no priority/ordering design for *what* restocks when capacity is partial |
| Market rumor bands | Sealed additive-kind precedent (`MarketRumor = 6`) available for extension |
| Barter appraisal | Live (Plan 191 retired as already-live) |

## XP-04.2 · Missing features

| Id | Feature |
|---|---|
| XP-04-F1 | `FundsLedger` — canonical player-side fungible-funds authority ( scrip, not a new currency item id) |
| XP-04-F2 | Black-market trade-action legs: buy/sell/fence legs bound to `FundsLedger` + inventory |
| XP-04-F3 | Merchant restock priority ordering: signed, data-driven restock queue |
| XP-04-F4 | Heat/attention mechanic for black-market use (risk leg) |
| XP-04-F5 | Counterfeit/purity tiers for fenced goods |
| XP-04-F6 | Funds-denominated debt/violence consequences (ties to sealed debt-consequence system) |

## XP-04.3 · Design

### Funds authority (XP-04-F1)

```csharp
public sealed class FundsLedger
{
    // Core, engine-free. Integer scrip units ("chits").
    // Single producer/consumer of fungible funds for ALL trade surfaces.
    public FundsResult TryDebit(int amount, string reasonKey, string sourceId);
    public FundsResult TryCredit(int amount, string reasonKey, string sourceId);
    public int Balance { get; }
    // Capture/Restore; append-only movement log (bounded 128, mirrors MedicalRecordLog rules:
    // day + amount + reason key + counterparty id, never free text).
}
```

| Rule | Value |
|---|---|
| Unit | Integer chits; 1 chit ≈ 1 canned ration at standard difficulty baseline pricing |
| Integration with existing markets | Markets that already clear in goods keep goods-clearing; funds are **additive** as an accepted medium on surfaces that opt in via catalog flag `accepts_funds` |
| Persistence | New `funds_ledger` save section, V+1 migration inserts empty ledger |
| Determinism | No RNG; all flows event-driven |

### Black-market legs (XP-04-F2)

| Action | Legs | Failure states |
|---|---|---|
| `bm_buy` | goods → shelter inventory; chits → merchant | insufficient chits; heat cap |
| `bm_sell` | goods → merchant; chits → player | merchant stockpile cap; heat cap |
| `bm_fence` | flagged/stolen goods → unflagged (fee 20–35%) | purity roll fails; heat spike |
| `bm_contract` | chits escrowed → goods delivered N days later | convoy loss (binds XP-02 war/flood risk); seller default |

### Heat mechanic (XP-04-F4)

| Trigger | Heat delta | Decay |
|---|---|---|
| Any black-market transaction | +1 | −1 per 2 idle days |
| Fencing flagged goods | +2 | same |
| Failed purity roll | +3 | same |

| Heat band | Effect |
|---|---|
| 0–4 | normal prices |
| 5–9 | price premium +10%, fewer stock slots |
| 10+ | market relocates for 7–14 days (drawn from persisted seeded sub-stream; anti-reroll) |

Heat is per-faction-market, persisted, restore never replays.

### Restock priority (XP-04-F3) — proposed signed ordering

```json
{
  "schema_version": 1,
  "merchant_id": "merchant_quartermaster_voss",
  "restock_capacity_per_period": 40,
  "priority_categories": [
    { "category": "medical", "weight": 30, "scarcity_floor": 3 },
    { "category": "calories", "weight": 25, "scarcity_floor": 10 },
    { "category": "fuel_energy", "weight": 15, "scarcity_floor": 5 },
    { "category": "tools", "weight": 12, "scarcity_floor": 2 },
    { "category": "luxury", "weight": 8, "scarcity_floor": 0 },
    { "category": "weapons", "weight": 10, "scarcity_floor": 1 }
  ],
  "scarcity_boost": "weight × 2 when stock < scarcity_floor"
}
```

Ordering algorithm (deterministic, no RNG):

1. Compute per-category effective weight = `weight × (stock < floor ? 2 : 1)`.
2. Allocate capacity proportionally to effective weights, largest-remainder rounding.
3. Within a category, restock item with the lowest `stock / target_par` ratio first.
4. Tie-break by authored `restock_order` field, then by item id (lexicographic, stable).

| Player-visible effect | Where |
|---|---|
| "NEXT SUPPLY" strip on trade panel: category + quantity arriving next period | `TradePanel`/market panel strip, words-not-color |
| Scarcity floors mean sieges/blockades shift restock toward medical/calories automatically | Emergent from weights, no special-case code |

### Purity tiers (XP-04-F5)

| Tier | Purity roll (seeded, persisted per item instance) | Price | Failure effect |
|---|---|---|---|
| sealed | — | 100% | none |
| clean | ≥ 0.75 | 90% | none |
| suspect | 0.40–0.74 | 65% | item condition −20% |
| cut | < 0.40 | 45% | affliction risk on use (binds Disease/Medical pipeline) |

Purity rides the sealed equipment-condition/instance-state pattern; appraisal skill (Plan 191 live) improves *displayed* tier accuracy, never the underlying roll.

## XP-04.4 · Integration points

- `FundsLedger` consumed by: black market, holdfast trade sessions, caravan trade, quest rewards
- Debt-consequence system (sealed) — unpaid `bm_contract` escrow converts to debt rows
- `EconomyMarketRumorRules` — heat/price premia compose with rumor bands multiplicatively
- Panels: funds readout on all opt-in surfaces; NEXT SUPPLY strip; heat as "ATTENTION" wording, never color-only

## XP-04.5 · Acceptance

| Gate | Requirement |
|---|---|
| `FundsLedgerTests` | Debit/credit atomicity, bounded log eviction, restore fidelity |
| `BlackMarketTradeLegTests` | All four actions' legs, failure states, exactly-once |
| `MerchantRestockPriorityTests` | Deterministic allocation, largest-remainder correctness, scarcity boost, tie-breaks |
| `BlackMarketHeatTests` | Band effects, relocation draw anti-reroll, decay |
| `FencePurityTests` | Tier rolls persisted, appraisal affects display only |
| Save round-trip | New section migrates; old saves get empty ledger |

## XP-04.6 · Balance derivation

- Chit/ration parity (1:1 at standard pricing) keeps funds legible against the existing calorie economy — players can price-check everything against food, the survival floor.
- Fence fee 20–35% makes laundering *worse* than honest sale for clean goods but the *only* option for flagged loot — the decision exists only when the player has already taken moral/radiation risk to acquire flagged goods.
- Restock weights sum to 100 and use integer largest-remainder so the allocation is exactly reproducible in tests.

## XP-04.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Funds become infinite money printer via arbitrage across markets | All opt-in surfaces share the single `FundsLedger`; price premia are symmetric; arbitrage probes (existing `EconomyProbeTests`) extended with funds legs |
| Heat ignored by rich players | Relocation hits *access*, not price — chits cannot buy a market that moved |
| Restock scumming by draining one category | Scarcity floor ×2 boost is evaluated at restock time, so draining medical guarantees medical priority — intended (siege logic), bounded by capacity |

## XP-04.8 · Foreman decisions required

1. Sign `FundsLedger` as canonical funds authority (the named blocker).
2. Sign restock ordering design above (or amend weights).
3. Sign which surfaces opt into funds in wave 1 (proposal: black market + caravan only).

---

# XP-05 · FUEL & ENERGY CONSUMPTION COMPLETENESS (SOFC SEAM)

## XP-05.0 · Gap ID and status

- **GAP-ID:** `XP-05-FUEL-CONSUMPTION`
- **Debt row addressed:** handoff §4 "highest-value surfaced follow-up: SOFC `FuelConsumer` placeholder (`FuelConsumer = units => true` — the SOFC consumes no inventory fuel until the inventory owner binds the real check)"

## XP-05.1 · Current state (evidence)

| Item | State |
|---|---|
| SOFC (solid-oxide fuel cell) | Implemented, powers grid; `FuelConsumer` placeholder returns `true` unconditionally |
| Inventory | Canonical item owner |
| Power grid | `PowerGridSystem.IsRoomPowered` live-feed precedent (Plan 210 sealed) |
| Fuel items | Exist (charcoal, diesel, wood, etc. across crafting/energy catalogs) |

## XP-05.2 · Missing features

| Id | Feature |
|---|---|
| XP-05-F1 | Real `FuelConsumer` binding: SOFC draw against inventory fuel per burn tick |
| XP-05-F2 | Fuel-grade table: which item ids feed the SOFC, at what conversion rate |
| XP-05-F3 | Tank/buffer state: internal fuel reserve with days-remaining telemetry |
| XP-05-F4 | Fuel-starvation behavior: graceful degradation, not hard off |
| XP-05-F5 | Refuel action + preflight advisory (fuel logistics become a planning surface) |
| XP-05-F6 | Cross-consumer fairness: when multiple fuel consumers compete, deterministic rationing order |

## XP-05.3 · Design

### Fuel-grade table (`fuel_grades.json`, `schema_version: 1`)

| Item id | Grade | kWh per unit | Residue (g/units) | Notes |
|---|---|---|---|---|
| `item_charcoal_briquette` | solid | 3.2 | 0.02 | ties to charcoal pyrolysis catalog |
| `item_firewood_bundle` | solid | 2.1 | 0.05 | |
| `item_diesel_can` | liquid | 9.8 | 0.01 | also feeds vehicles (XP-02) |
| `item_methanol_jerrycan` | liquid | 5.4 | 0.02 | fermentation/bio chain |
| `item_propane_tank` | gas | 12.6 | 0.00 | scarce, high value |
| `item_biogas bladder` | gas | 4.0 | 0.03 | bio-fermentation chain |

### Burn model

```
daily_fuel_demand_kwh = load_kwh × consumption_rate / stack_efficiency
units_needed = ceil(daily_fuel_demand_kwh / kWh_per_unit[grade])
```

| Parameter | Value | Derivation |
|---|---|---|
| `stack_efficiency` (SOFC) | 0.62 | electrochemical realism; leaves ~38% as heat → warms room 0.5°C (ties to `room_storage_bay` °C projection) |
| Tank capacity | 24 units | ~3 days autonomy at typical shelter load — forces refuel cadence decisions |
| Auto-grade order | propane → diesel → methanol → biogas → charcoal → firewood | highest density first when tank holds mixed stock; player-override list persisted |

### Starvation behavior (XP-05-F4)

| Reserve state | Grid effect | Advisory |
|---|---|---|
| > 2 days equivalent | full output | none |
| 1–2 days | full output | "FUEL RESERVE LOW" panel strip |
| < 1 day | output capped at 70% load; non-critical rooms shed first (existing room priority) | "LOAD SHEDDING" |
| 0 | SOFC offline; grid falls back to other providers (nuclear core, generators) | "SOFC OFFLINE — NO FUEL" |

Graceful degradation guarantees the placeholder's current semantics (`true`) are the *degenerate upper bound* — existing tests that assumed unconstrained fuel keep passing when a full tank is present.

### Rationing order (XP-05-F6)

When multiple fuel consumers (SOFC, kilns, foundry, vehicles) draw the same item pool in one tick, consumption order is: **life-support (water treatment, medical) → food chain (kitchen, fermentation) → security (airlock, comms) → industry (foundry, kilns) → comfort**. Fixed, authored, deterministic — no RNG, no market bidding.

## XP-05.4 · Integration points

- Inventory owner: `TryConsume` via the fuel consumer port — the SOFC never owns fuel items
- `PowerGridSystem`: SOFC reports available output given reserve state
- Crafting/foundry/kiln consumers join the same rationing order
- Panels: SOFC panel strip (reserve %, grade active, days remaining); refuel action with preflight (mirrors rescue preflight advisory pattern)
- `ISeededRng`: none required — burn math is deterministic

## XP-05.5 · Acceptance

| Gate | Requirement |
|---|---|
| `SofcFuelConsumerBindingTests` | Real consumption per burn tick; placeholder semantics preserved with full tank |
| `FuelGradeCatalogTests` | All item ids resolve in item catalog; kWh ranges sane |
| `SofcStarvationTests` | Load-shed thresholds, room priority, offline fallback |
| `FuelRationingOrderTests` | Deterministic multi-consumer ordering, starvation of industry before life support |
| Reload-replay | Reserve state continuous vs mid-reload equal |

## XP-05.6 · Balance derivation

- 3-day tank autonomy means refueling is a *weekly-ish* chore with *daily* visibility — a pressure dial, not a click tax. Propane's 12.6 kWh/unit makes it the strategic reserve: hoarding propane is the correct late-game instinct, and the auto-grade order burning it *first* by density is deliberately wrong for hoarders — hence the persisted player override, a small meaningful decision.
- Rationing order encodes the game's value statement (life before industry) into mechanics rather than UI warnings.

## XP-05.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Fuel-free power via provider stacking | All providers' `FuelConsumer` bindings audited by a new gate; placeholder detectors (`units => true` lambdas) flagged by analyzer |
| Oscillating on/off at 0 reserve | Hysteresis: restart requires 2 units in tank |
| Mixed-stock tank abuse via grade overrides | Overrides only change order, never conversion rates |

## XP-05.8 · Foreman decisions required

1. Sign fuel owner (proposal: inventory, SOFC only reads).
2. Sign kWh/unit table and rationing order.
3. Sign whether vehicles share the pool in wave 1 or defer to XP-02's vehicle legs.

---

# XP-06 · BODY INTEGRITY: HANDEDNESS, LIMB REQUIREMENTS, PROSTHETICS

## XP-06.0 · Gap ID and status

- **GAP-ID:** `XP-06-BODY-INTEGRITY`
- **Debt row addressed:** `DEBT-AMPUTATION-EQUIPMENT-RESTRICTION` (equipment half BLOCKED — "`ItemDefinition`/`EquipSlot` has no handedness/limb-requirement field, so an arm-state equip restriction needs a schema redesign")

## XP-06.1 · Current state (evidence)

| Item | State |
|---|---|
| Amputation system | Live; movement-speed multiplier consumed by expeditions (sealed C2-D3) |
| Equipment schema | No handedness / limb-requirement fields |
| Presentation | Avatar half sealed by deletion (no presentation illusions) |
| Prosthetics | None — no catalog, no crafting, no equip rules |

## XP-06.2 · Missing features

| Id | Feature |
|---|---|
| XP-06-F1 | Schema extension: `limb_requirements` on `ItemDefinition`; `body_state` on survivor |
| XP-06-F2 | Equip-time gating: two-handed weapons unusable with one arm, etc. |
| XP-06-F3 | Prosthetics catalog + crafting chain (hooks: foundry, ceramics, apiculture wax, cordage) |
| XP-06-F4 | Prosthetic tiers with maintenance/condition integration |
| XP-06-F5 | Rehabilitation arc: fitting → adaptation → full function (ties to skill/apprenticeship ticks) |
| XP-06-F6 | Phantom-pain and overuse considerations in the psychological/medical pipeline |

## XP-06.3 · Design

### Schema extension (additive, optional fields — old catalogs deserialize unchanged)

```json
{
  "id": "item_bolt_action_rifle",
  "equip_slot": "weapon",
  "limb_requirements": { "hands": 2 },
  "strength_requirement": 14
}
```

```json
{
  "id": "item_hook_prosthetic",
  "equip_slot": "prosthetic_hand",
  "provides_limb": { "hands": 1, "quality": 0.5 }
}
```

| `body_state` field (survivor) | Type | Values |
|---|---|---|
| `limbs` | map | `left_arm`, `right_arm`, `left_leg`, `right_leg` → `intact`, `amputated`, `prosthetized(prosthetic_item_id)` |

### Equip gating rule (single authority)

```
effective_hands = Σ provides_limb.hands over intact arms + fitted prosthetics
equip allowed ⇔ item.limb_requirements.hands ≤ effective_hands
              AND item.strength_requirement ≤ survivor.strength
```

- Gate runs at equip time and on every `body_state` change (an amputation event unequips violating items automatically, journal line emitted).
- Prosthetic `quality` scales *some* weapon handling characteristics (aim stability), never damage — damage identity stays with the weapon.

### Prosthetics catalog

| Item id | Tier | Crafting chain | Provides | Condition decay | Notes |
|---|---|---|---|---|---|
| `item_hook_prosthetic` | field | cordage + bone carving | hands: 1, q 0.5 | 2%/day | cheap, fast |
| `item_leather_splint_hand` | field | tanning + cordage | hands: 1, q 0.4 | 1.5%/day | |
| `item_pinned_metal_hand` | workshop | foundry + ceramics joint | hands: 1, q 0.75 | 1%/day | needs machinist |
| `item_articulated_hand` | advanced | crucible foundry + apiculture wax bushings + glass bearings | hands: 1, q 0.9 | 0.7%/day | endgame craft |
| `item_peg_leg` | field | carpentry | legs: 1, q 0.5 | 2%/day | movement +0.85 of sealed multiplier |
| `item_sprung_leg_frame` | advanced | foundry + cordage cable | legs: 1, q 0.85 | 0.8%/day | movement 0.95 |

Condition ties into the sealed `EquipmentConditionSystem`; a failed prosthetic at 0% = `amputated` behavior until repaired/replaced.

### Rehabilitation arc (XP-06-F5)

| Phase | Days | Effect | Driver |
|---|---|---|---|
| fitting | 3–5 | provides_limb active at ×0.5 quality | medical pipeline event |
| adaptation | 10–20 (scaled by skill_physical resilience) | quality ramps to full | daily tick beside apprenticeship tick (`TickSharedSkillProgression` owner) |
| mastery | permanent | small handling bonus with this prosthetic type | skill progression |

### Psychological seam (XP-06-F6)

| Event | Pipeline effect |
|---|---|
| Amputation (acute) | trauma entry (existing mental-health crisis vocabulary) |
| Phantom pain nights | insomnia-classified sleep beat variant — extends the sealed `SleepNarrativeProjection` classification, no new DreamSystem |
| First mastery | chronicle milestone via Plan 178 hook |

## XP-06.4 · Integration points

- `ItemDefinition`/`EquipSlot` schema (the named blocker) + `CatalogIntegrityValidator` (requirement ranges, provides-limb sanity)
- `AmputationSystem` remains the arm/leg state owner; equip gate is a read-model over it
- Crafting chains: foundry, ceramics, cordage, bone carving, apiculture — all live catalogs
- `EquipmentConditionSystem` for decay/repair
- `SurvivorDetailPanel`: limb state + prosthetic slots; words-not-color

## XP-06.5 · Acceptance

| Gate | Requirement |
|---|---|
| `LimbRequirementSchemaTests` | Additive deserialization, old catalogs byte-identical |
| `EquipLimbGateTests` | Two-handed/one-handed gating, auto-unequip on amputation event, prosthetic provision |
| `ProstheticsCatalogTests` | Crafting chains resolve, decay rates valid |
| `RehabilitationArcTests` | Phase lengths, skill scaling, determinism |
| `ProstheticConditionTests` | Decay → failed → amputated-equivalent behavior; repair restores |
| Save round-trip | `body_state` additive on survivor records; migration inserts intact limbs |

## XP-06.6 · Balance derivation

- Field-tier prosthetics restore *capability class* (can hold a tool) but not *performance* (q 0.4–0.5) — the shelter keeps a maimed survivor productive, but the player feels the loss and wants the advanced craft. That want is the content hook for the foundry endgame.
- Peg-leg movement 0.85 vs. the sealed amputation multiplier (per-limb) keeps the expedition math authoritative — prosthetics *approach* the sealed value, never exceed intact.
- Rehabilitation length (10–20 days) matches the campaign's existing recovery arcs (disease, chemical dependency) so the medical pipeline has one pacing language.

## XP-06.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Hot-swap prosthetics to dodge condition decay | Unequip→re-equip during `fitting` phase resets quality ramp; decay persists per item instance |
| Strength + two hooks = dual-wield rifles | `limb_requirements` counts *biological pattern*: `hands: 2` means two hands *of the item's required grip class*; hooks satisfy only `simple` grip items |
| Amputation becomes strategically optimal (less food?) | Calorie need scales with body mass unchanged; amputation adds trauma + rehab cost — never a net survival gain |

## XP-06.8 · Foreman decisions required

1. Sign the `ItemDefinition` schema extension (the named C2 blocker: "equipment: signed schema package").
2. Sign grip-class vocabulary (`simple` vs `full`) — proposal: two classes only.
3. Sign whether prosthetic slots are visible in survivor panel wave 1 or after avatar work.

---

# XP-07 · ITEM LORE & PROVENANCE TRACKING

## XP-07.0 · Gap ID and status

- **GAP-ID:** `XP-07-ITEM-PROVENANCE`
- **Debt row addressed:** Plan 190 (Instance-Lore / Item Provenance) — mapped but not signed; the Plan 178 closeout states "Plan 190 instance-lore stays mapped until separately signed"

## XP-07.1 · Current state (evidence)

| Item | State |
|---|---|
| Chronicle vault | Live — `TryRecordChronicleEntry`, `ArchiveChronicleMilestones`, memorial→chronicle hook (sealed) |
| Cultural archive | Live (collectibles, archive inks, archive desk) |
| Item instance state | Exists for condition/purity-style fields |
| Provenance | None — items carry no origin, ownership, or discovery history |

## XP-07.2 · Missing features

| Id | Feature |
|---|---|
| XP-07-F1 | Provenance record on item instances: origin kind, day, actor, place |
| XP-07-F2 | Named-item system: a bounded set of authored unique items with lore fragments |
| XP-07-F3 | Lore fragment reveal conditions (condition thresholds, appraisal, chronicle milestones) |
| XP-07-F4 | Provenance-aware interactions: heirloom requests, memorial claims, faction recognition of stolen goods |
| XP-07-F5 | Read surface: item detail panel "HISTORY" rows; words-not-color |
| XP-07-F6 | Provenance export to the chronicle on death/inheritance (binds Plan 206 legacy flow) |

## XP-07.3 · Design

### Provenance record (additive optional on item instances)

```json
{
  "origin_kind": "crafted | scavenged | traded | gifted | looted | inherited | recovered",
  "origin_day": 214,
  "origin_actor_id": "survivor_ilse_brandt",
  "origin_place_id": "loc_verity_motel",
  "named_item_id": null
}
```

| Rule | Value |
|---|---|
| Record size cap | One record per instance — no append-only history per item (bounded memory; the chronicle remains the history authority) |
| Migration | Old instances deserialize with `origin_kind: null` → displayed as "HISTORY UNKNOWN" |
| Determinism | Purely event-driven; no RNG |

### Named items (`named_items.json`, `schema_version: 1`)

| Field | Example |
|---|---|
| `id` | `named_watch_inspectors_luminox` |
| `base_item_id` | `item_windup_watch` |
| `display_name` | "The Inspector's Watch" |
| `lore_fragments` | 3 fragments, each with reveal condition |
| `trait` | small mechanical identity, e.g. `reliability_bonus: 0.1` |

Proposed launch set: 24 named items across categories (tools, weapons, instruments, keepsakes), each mechanically distinct but never strictly superior to crafted equivalents (+10–15% in one dimension, −5–10% in another), and each with one quest hook into existing questline masters.

### Reveal conditions (XP-07-F3)

| Condition type | Example | Consumer |
|---|---|---|
| appraisal skill check | `appraisal ≥ 40` reveals fragment 2 | Plan 191 live appraisal |
| condition threshold | item repaired above 90% | `EquipmentConditionSystem` |
| place visit | carried to `loc_*` tied to the item's story | XP-02 survey state |
| chronicle milestone | owner survives a crisis | Plan 178 hook |
| pairing | carried together with another named item | inventory read-model |

### Provenance-aware interactions (XP-07-F4)

| Interaction | Trigger | Effect |
|---|---|---|
| Heirloom claim | Deceased survivor's `gifted`/`crafted` items | Bereaved with high affinity gains item + grief-processing chronicle entry (ties Guilt/Insomnia system) |
| Faction recognition | `looted` item from faction territory shown to that faction | Standing −4; fenceable via XP-04 `bm_fence` only |
| Memorial dedication | Any named item post-humously | Chronically-inscribed memorial (extends memorial→chronicle hook) |

## XP-07.4 · Integration points

- Inventory instance state (additive field), `ItemInspectionModel` → `InventoryDetailPanel` for read surface
- Plan 191 appraisal, `EquipmentConditionSystem`, XP-02 survey state as reveal inputs
- Chronicle hook (Plan 178 pattern) for milestone reveals + inheritance export
- Guilt/Insomnia and relationship systems for heirloom claims
- Questline master catalogs for the 24 hooks

## XP-07.5 · Acceptance

| Gate | Requirement |
|---|---|
| `ItemProvenanceSchemaTests` | Additive field, null migration, no free-text storage (ids only) |
| `NamedItemsCatalogTests` | 24 items, base ids resolve, trait balance envelope (+15/−10 max) |
| `LoreFragmentRevealTests` | Each condition type fires exactly once, persisted |
| `ProvenanceInteractionTests` | Heirloom claim, faction recognition, memorial dedication |
| `ProvenanceChronicleExportTests` | Death/inheritance writes exactly one chronicle entry |
| A11y | HISTORY rows words-not-color; screen-reader labels |

## XP-07.6 · Balance derivation

- Traits capped at +15% single-dimension with a compensating penalty keeps named items *identity*, not upgrades — the choice to carry "The Inspector's Watch" (reliable, but heavy and faction-recognized) is a character decision, not a stat roll.
- Reveal conditions all consume *existing* progression surfaces (appraisal, repair, exploration, chronicle) so lore is a reward rhythm layered on current systems with zero new progression currency.

## XP-07.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Provenance bloat in saves | Single bounded record per instance; ids only; save-size selftest extended |
| Farming heirloom claims via intentional deaths | Grief/insomnia and morale costs scale with repeated loss; affinity gate ≥ high |
| Named-item traits stacking with prosthetics/equipment mods | Trait envelope enforced at catalog validation |

## XP-07.8 · Foreman decisions required

1. Sign Plan 190 scope: provenance-record + named-items subset (above) vs. full instance-lore.
2. Sign launch set size (24 proposed) and trait envelope.
3. Sign whether `looted` flagging applies retroactively to existing theft paths or only new ones.

---

# XP-08 · TRADE ROUTES & SEASONAL MIGRATION

## XP-08.0 · Gap ID and status

- **GAP-ID:** `XP-08-TRADE-MIGRATION`
- **Debt rows addressed:** Plans 192 (Player Trade Route Establishment) and 199 (Seasonal Migration, Human Faction) — the two remaining unmapped plan families ("192/199 remain unmapped pending product decision")

## XP-08.1 · Current state (evidence)

| Item | State |
|---|---|
| Caravans | Live (caravan patrol, caravan-war integration) |
| Holdfast trade | Live (sessions, arbitrage probes) |
| Black market | Surface exists; legs undefined (XP-04) |
| Faction movement | War-chain fronts move; populations do not |
| Player-established trade routes | None |
| Seasonal migration | None |

## XP-08.2 · Missing features

| Id | Feature |
|---|---|
| XP-08-F1 | Player trade route contracts: shelter ↔ holdfast recurring supply runs |
| XP-08-F2 | Route economics: fixed tariff, volume commitments, reliability score |
| XP-08-F3 | Route risk binding to XP-02 edge conditions (floods/war interrupt runs) |
| XP-08-F4 | Reliability→volume growth: dependable routes unlock higher tiers |
| XP-08-F5 | Seasonal migration overlays: human-faction populations move with Year-of-Ash seasons |
| XP-08-F6 | Migration consequences: market relocation, labor availability, territorial friction |

## XP-08.3 · Design

### Trade route contract (`trade_routes.json` authored templates + runtime instances)

```json
{
  "id": "route_shelter_to_krasthold",
  "counterparty_id": "holdfast_krasthold",
  "goods_out": [{ "item_id": "item_iodine", "units_per_run": 6 }],
  "goods_in":  [{ "item_id": "item_grain_sack", "units_per_run": 10 }],
  "cadence_days": 5,
  "tariff_chits": 4,
  "min_reliability_tier": 1,
  "caravan_slots_required": 1
}
```

| Tier | Reliability score needed | Unlock |
|---|---|---|
| 1 · occasional | 0 (any) | base cadence, base volumes |
| 2 · scheduled | 5 successful runs | cadence −1 day, volumes +25% |
| 3 · favored | 12 successful runs | tariff −25%, scarce-goods access flag |
| 4 · exclusive | 20 successful + standing ≥ amicable | one exclusive good id per route |

- Reliability score: +1 per on-time run, −2 per failed/late run, floor 0.
- Route risk: each run's caravan rolls against XP-02 edge conditions en route; flooded/blocked edges convert to delay or loss (waystation decisions from XP-03-F3 apply).
- Player may suspend a route (relations dip slightly) or cancel (reliability resets after 30-day cooldown).

### Seasonal migration (XP-08-F5)

| Season (Year of Ash phase) | Migration pattern | Effect |
|---|---|---|
| deep winter | lowland holdfasts → sheltered valleys/shelter vicinity | labor pool +, demand for calories +30% |
| thaw | return movement + river traffic opens | boat passage_class edges reopen (XP-02) |
| growing season | settlement at fields/outposts | expedition encounter mix shifts toward foragers |
| ash storms | migration into deep shelters/metro | markets relocate (ties XP-04 heat relocation pattern) |

Migration is authored per faction as a schedule of population-weight deltas per region, applied by a deterministic daily tick; **no individual NPC agents** — population weights only, matching the economy's aggregate style.

### Migration consequences (XP-08-F6)

| Consequence | Mechanism |
|---|---|
| Market relocation | Region good-availability multipliers follow population weights |
| Labor availability | Apprenticeship/caregiving candidate pool scales with local population |
| Territorial friction | Population overlap in a region raises ideological-friction event weights (live system) |
| Caravan demand | Migrating populations generate escort/passage contracts (quest hooks) |

## XP-08.4 · Integration points

- Caravan dispatch (route runs are scheduled caravan missions), XP-04 `FundsLedger` for tariffs
- `WastelandMapSystem`/XP-02 planner for run pathing and risk
- Faction standing engine (route tier gates, migration friction)
- Ideological friction system (live) for overlap events
- Panels: ROUTES strip on trade panel (cadence, next run, tier, reliability); migration shown on war/region map widget

## XP-08.5 · Acceptance

| Gate | Requirement |
|---|---|
| `TradeRouteContractTests` | Cadence scheduling, tariff legs, tier gating, suspend/cancel rules |
| `RouteReliabilityTests` | Score math, decay/cooldown, tier unlock thresholds |
| `RouteRiskBindingTests` | Flood/war edge → delay/loss outcomes deterministic |
| `SeasonalMigrationTests` | Schedule application, population-weight deltas, day-accuracy |
| `MigrationConsequenceTests` | Market/labor/friction effects, no double-application |
| Reload-replay | Route + migration state continuous vs mid-reload equal |

## XP-08.6 · Balance derivation

- 5-day base cadence with 6-for-10 iodine/grain exchange is calibrated to make routes a *stability* strategy, not an arbitrage engine: per-run value ≈ 2–4 chits of margin at standard pricing — meaningful over 20 runs, useless for get-rich-quick.
- Tier 4 exclusivity (one exclusive good) is the long-term carrot that justifies protecting a route through a war — it makes XP-03's front movements *personal*.
- Migration uses population weights rather than agents specifically to stay within the existing economy's aggregate-determinism envelope and test budget.

## XP-08.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Route spam across all holdfasts | Caravan slots are the scarce resource; slots are bounded by shelter vehicles/drivers |
| Reliability farming with trivial goods | Volume commitments per tier; trivial runs score at tier-1 volumes only |
| Migration oscillation with weather | Season transitions are hysteresis-gated (10-day dwell per phase) |

## XP-08.8 · Foreman decisions required

1. Sign Plan 192 scope: contract model above (routes as scheduled caravans, no new logistics layer).
2. Sign Plan 199 scope: population-weight migration, no agent simulation.
3. Sign exclusive-goods list for tier 4 (requires economy sign-off).

---

# XP-09 · RADIO PRESENTER SKILL TREE & BROADCAST PERSONALITY

## XP-09.0 · Gap ID and status

- **GAP-ID:** `XP-09-PRESENTER-SKILLS`
- **Debt row addressed:** Plan 173 Phase 3 closeout — "presenter skill tree remains deferred"

## XP-09.1 · Current state (evidence)

| Item | State |
|---|---|
| Radio program production | Live: `RadioProgramProductionSystem`, `radio_programs.json`, SlotId binds, prep-cost gates, PsyOps campaign start |
| Program production panel | Live strip (start/cancel/LastEvent) |
| Presenter skills | None — no skill dimension for broadcast quality |
| Signal authenticity | Live skill-driven precedent (`skill_signal_ear`, `skill_watchful`, `skill_cold_analysis`) |

## XP-09.2 · Missing features

| Id | Feature |
|---|---|
| XP-09-F1 | Presenter skill domain: 5 skills with XP accrual from broadcasts |
| XP-09-F2 | Broadcast quality model: skill-weighted quality roll per program, persisted per program instance |
| XP-09-F3 | Voice/personality archetypes chosen at presenter creation |
| XP-09-F4 | Skill-gated program formats (interviews, serials, call-ins need skill thresholds) |
| XP-09-F5 | Audience response loop: quality → listenership → PsyOps effectiveness + morale effect |
| XP-09-F6 | Presenter fatigue/burnout consideration (ties mental health) |

## XP-09.3 · Design

### Skill domain (`presenter_skills.json`)

| Skill id | Grows by | Affects |
|---|---|---|
| `skill_broadcast_voice` | each aired program | baseline quality, listener retention |
| `skill_improvisation` | live/call-in formats, unscripted events | crisis-response broadcasts, authenticity defense |
| `skill_scripting` | serial/documentary formats | prep-cost reduction, message clarity (PsyOps strength) |
| `skill_audience_read` | call-ins, listener mail | targeted morale effect, faction-targeted programs |
| `skill_technical_ops` | equipment prep, repairs | on-air failure rate, equipment wear reduction |

### Quality model

```
program_quality = clamp(0.2
    + 0.08 × voice_level
    + 0.05 × scripting_level (if scripted format)
    + 0.05 × improvisation_level (if live format)
    + 0.04 × audience_read_level (if targeted)
    + archetype_bonus (format-matched)
    − fatigue_penalty, 0.05, 1.0)
```

- Quality roll drawn once per program instance from a persisted seeded sub-stream (`radio_presenter`), anti-reroll (first result persists).
- Quality bands: `< 0.35` dead air (listenership −20%), `0.35–0.7` serviceable, `> 0.7` resonant (morale +, PsyOps ×1.2).

### Archetypes (XP-09-F3)

| Archetype | Bonus format | Penalty format | Signature |
|---|---|---|---|
| `the_warm_hand` | morale programs | war news | +listener retention during crises |
| `the_cold_reader` | news, decrees | morale programs | PsyOps strength +15%, morale − |
| `the_raconteur` | serials, drama | call-ins | listenership +10%, prep cost + |
| `the_switchboard_saint` | call-ins | scripted serials | authenticity-signal rumors gained +1 slot |

Archetype is chosen once per presenter (survivor with radio duty), persisted on the presenter record; never re-choosable (identity, not optimization).

### Fatigue (XP-09-F6)

| Broadcast load (programs/week) | Fatigue delta | Effect |
|---|---|---|
| ≤ 2 | 0 | — |
| 3–4 | +1/day | −0.05 quality each, panel warning "VOICE WEARY" |
| 5+ | +2/day | mental-health stress consideration entry (existing vocabulary) |

Fatigue decays −2 per rest day without broadcasts.

## XP-09.4 · Integration points

- `RadioProgramProductionSystem` (quality roll at air time, prep gates), PsyOps campaign effectiveness multiplier
- Skill progression system (live `SkillProgressionSystem`, host tick precedent)
- Mental-health pipeline for fatigue stress; sleep narrative unaffected
- Morale/mood owner for listener effects
- Panels: RadioPanel presenter strip (skills, archetype, fatigue, next-XP); words-not-color

## XP-09.5 · Acceptance

| Gate | Requirement |
|---|---|
| `PresenterSkillCatalogTests` | 5 skills, XP curve, thresholds |
| `BroadcastQualityTests` | Formula bands, archetype bonuses, persisted anti-reroll roll |
| `PresenterArchetypeTests` | One-time choice, format match/mismatch |
| `AudienceResponseTests` | Listenership/morale/PsyOps effects exactly-once per airing |
| `PresenterFatigueTests` | Accrual, decay, stress entry, panel warning |
| Reload-replay | Skill XP + quality rolls continuous vs mid-reload equal |

## XP-09.6 · Balance derivation

- The quality formula saturates at ~8 combined skill levels, reached after roughly 30–40 aired programs — a full campaign of dedicated radio use, matching the apprenticeship arc's pacing.
- Fatigue thresholds deliberately make a **daily broadcaster impossible**: radio stays a strategic voice, not a morale vending machine; 3–4 programs/week is the sustainable optimum.
- Archetype penalties are format-based (not numeric) so no archetype is strictly best — shelter mood determines which voice matters.

## XP-09.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Rotating presenters to dodge fatigue | Fatigue attaches per presenter, but skill XP does too — rotation spreads thin, diluting quality |
| Quality re-rolled via save/load | Persisted sub-stream roll, rescue-signal anti-reroll pattern |
| PsyOps + morale double-dipping | A program is either morale-typed or PsyOps-typed per its authored format |

## XP-09.8 · Foreman decisions required

1. Sign the 5-skill domain and the archetype list (4 proposed).
2. Sign fatigue coupling depth (warning-only vs. stress entry — proposal: both).
3. Sign whether presenters must be radio-duty survivors only (proposal: yes).

---

# XP-10 · PHOBIA & TRAIT GROWTH LAYER

## XP-10.0 · Gap ID and status

- **GAP-ID:** `XP-10-PHOBIA-GROWTH`
- **Debt rows addressed:** Plan 177 closeout ("phobia growth stays DEFERRED to traits/catalog"); Plan 179 (Unified Psychology/Phobia) partially sealed — unified projection exists, growth rules do not

## XP-10.1 · Current state (evidence)

| Item | State |
|---|---|
| Psychological arc / mental health | Live (crisis, insomnia, trauma, stress records) |
| Sleep narrative projection | Live, read-only, deterministic |
| Phobias | Authored as static traits in survivor catalogs — no acquisition, no growth, no exposure arc |
| Guilt/insomnia | Live with persisted sources |

## XP-10.2 · Missing features

| Id | Feature |
|---|---|
| XP-10-F1 | Phobia acquisition rules: trauma-triggered, bounded vocabulary, personality-weighted |
| XP-10-F2 | Phobia intensity levels (mild → entrenched) with threshold effects |
| XP-10-F3 | Exposure/recovery arc: repeated safe contact decays intensity |
| XP-10-F4 | Phobia-behavior bridge into UtilityAI considerations (avoidance scoring) |
| XP-10-F5 | Trait growth generally: positive traits earned through repeated behavior |
| XP-10-F6 | Interactions with prosthetics (XP-06 phantom pain), radio fatigue, guilt sources |

## XP-10.3 · Design

### Phobia vocabulary (bounded, closed — 8 entries, catalog-authored)

| Phobia id | Trigger class | Avoids |
|---|---|---|
| `phobia_dark` | metro/cave collapse events | unlit expeditions |
| `phobia_water` | sump flooding, near-drowning | flooded edges, boat legs |
| `phobia_confinement` | airlock lockdown, burial | airlock duty, tunnels |
| `phobia_blood` | amputation, mass-casualty events | medical duty |
| `phobia_storms` | ash storms | surface work during weather alerts |
| `phobia_crowds` | migration influx, riots | crowded rooms, markets |
| `phobia_radiation` | acute dose event | irradiated edges, reactor rooms |
| `phobia_silence` | long comms blackout | solo night shifts |

### Acquisition rule

```
P(acquire) = base 0.25
  × personality_weight (trait catalog, 0.5–2.0)
  × severity_factor of trigger (1–3)
  × (already has phobia? 0.4 : 1.0)
roll from persisted seeded sub-stream (psychology), once per trigger event, anti-reroll
```

### Intensity and thresholds

| Level | Range | Effect |
|---|---|---|
| 1 mild | 1–29 | flavor journal lines only |
| 2 pronounced | 30–59 | UtilityAI avoidance weight +; stress gain in trigger contexts |
| 3 entrenched | 60–89 | refusal behavior possible (survivor declines relevant duty) |
| 4 consuming | 90–100 | crisis-event vulnerability (existing mental-health crisis vocabulary) |

Intensity grows +5–15 per trigger-context exposure *without* safe resolution; decays −3 per safe exposure, −1 per idle week, floor 0 (phobia at 0 for 14 days = removed).

### Exposure/recovery arc (XP-10-F3)

| Exposure kind | Intensity delta | Conditions |
|---|---|---|
| Triggered, uncontrolled | +8 to +15 | event-driven |
| Voluntary, supported | −3 | duty performed with morale > threshold and a trusted partner assigned |
| Therapeutic protocol | −6/day | medical pipeline treatment (new treatment kind, binds existing pipeline) |

### UtilityAI bridge (XP-10-F4)

| Consideration | Input curve |
|---|---|
| `phobia_avoidance` | step function by intensity level on context tags |
| `trusted_partner_present` | −50% avoidance weight when paired with affinity ≥ high |
| `duty_necessity` | existing priority considerations outrank phobia only at crisis alert level |

All considerations are data-driven entries in the live UtilityAI catalog — no hard-coded decision trees.

### Positive trait growth (XP-10-F5)

| Earned trait | Requires | Effect |
|---|---|---|
| `trait_field_hardened` | 10 expeditions without crisis | stress gain −10% |
| `trait_steady_hands` | 20 successful medical assists | medical action failure − |
| `trait_signal_veteran` | 30 aired programs | radio fatigue accrual −25% |
| `trait_unshakable` | recover from 3 crises | phobia acquisition × 0.5 |

Traits granted exactly once via the chronicle-milestone hook pattern; never removed.

## XP-10.4 · Integration points

- Survivor trait catalog (additive earned-traits section), personality weights from existing trait data
- UtilityAI catalog (new considerations, weights authored)
- Medical pipeline (therapeutic protocol treatment kind)
- Mental-health crisis system (level-4 vulnerability), guilt/insomnia (trigger co-occurrence)
- XP-06 phantom pain (co-trigger for `phobia_silence`/insomnia classification)

## XP-10.5 · Acceptance

| Gate | Requirement |
|---|---|
| `PhobiaAcquisitionTests` | Bounded vocabulary, probability formula, anti-reroll, existing-phobia damping |
| `PhobiaIntensityTests` | Level thresholds, growth/decay math, removal rule |
| `PhobiaExposureArcTests` | All three exposure kinds, partner/support modifiers |
| `PhobiaUtilityAiBridgeTests` | Consideration curves, necessity override at crisis only |
| `EarnedTraitTests` | Grant conditions, exactly-once, no removal |
| Reload-replay | Phobia + trait state continuous vs mid-reload equal |

## XP-10.6 · Balance derivation

- Acquisition base 0.25 with severity scaling means a typical campaign sees 1–3 phobias per shelter of 8–12 survivors — present enough to matter, never a plague.
- Voluntary supported exposure (−3) being weaker than growth (+8–15) makes *avoidance* the default strategy and *treatment* an investment — the same asymmetry as the disease system, so players learn one recovery grammar.
- `trait_unshakable`'s × 0.5 acquisition dampening rewards psychological resilience play without making trauma a farm (crises are costly by construction).

## XP-10.7 · Risks / exploits

| Risk | Guard |
|---|---|
| Phobia triggered in a loop to farm journal content | Journal lines knowledge-key deduped per survivor (existing pattern) |
| Intentional duty-refusal used to steer roster | Refusal only at entrenched level; necessity override at crisis |
| UtilityAI pathological avoidance loops | Consideration weights bounded; fallback behavior authored; debug overlay logs arbitration (existing utility-AI debug surface) |

## XP-10.8 · Foreman decisions required

1. Sign the 8-entry closed phobia vocabulary (extensions = catalog version bump only).
2. Sign the earned-trait list (4 proposed) and their grant hooks.
3. Sign therapeutic protocol as a medical-pipeline treatment kind (vs. new psychology system — proposal: pipeline kind).

---

# PART 1 CLOSING · IMPLEMENTATION ORDER & CROSS-PILLAR MAP

## Recommended sequencing (dependency-aware)

| Wave | Pillars | Rationale |
|---|---|---|
| 1 | XP-01, XP-05 | Both unblock other work (difficulty scalars consumed everywhere; fuel seam closes the highest-value follow-up); zero cross-dependency |
| 2 | XP-04 | Funds authority is prerequisite for XP-08 tariffs; black-market legs standalone |
| 3 | XP-02 | Graph travel is the substrate for XP-03 expedition route + XP-08 route risk |
| 4 | XP-03 | Consequence routes consume XP-02 edge seam + XP-04 band pattern |
| 5 | XP-06, XP-09, XP-10 | Independent pillars; XP-10 binds XP-06 phantom-pain co-trigger |
| 6 | XP-07, XP-08 | Consumes XP-02 survey state, XP-04 funds, XP-03 risk vocabulary |

## Cross-pillar dependency edges

| Producer | Consumer | Seam |
|---|---|---|
| XP-02 edge conditions | XP-03-F3, XP-08-F3 | `EdgeConditionChanged` events |
| XP-04 `FundsLedger` | XP-08 tariffs | debit/credit ports |
| XP-04 heat relocation | XP-08-F5 market relocation | shared relocation pattern |
| XP-06 phantom pain | XP-10 insomnia classification | sleep-beat tag |
| XP-07 reveal conditions | XP-02 survey, Plan 191 appraisal, Plan 178 chronicle | read-only inputs |
| XP-01 difficulty scalars | XP-03 war shock magnitude (optional), NOT travel | provider port |

## Shared invariants (all pillars)

1. No new RNG authorities — persisted seeded sub-streams only, anti-reroll where a roll has lasting consequences.
2. Additive-first persistence; every migration leaves legacy behavior byte-identical when the new feature is absent.
3. No presentation-only simulations (C2 rule) and no second authorities for anything that already has an owner.
4. Every catalog ships with `schema_version` and passes `--data-integrity-selftest`; every system ships with reload-replay parity tests.
5. All panel additions: words never color-only, a11y-gated, journal lines for one-time consequences.

**End of Part 1.** Part 2 (XP-11 … XP-20) will cover: romance & family dynamics, vehicle customization/mobile base, hobby & leisure, shelter atmosphere/ambiance, museum & photography, time capsules, exercise/training, internal communication network, mod data contract, and new-game-plus/meta progression.