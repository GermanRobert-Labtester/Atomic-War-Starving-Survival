# C1 — Flagship Integration Plan [11]: The Year Turns — Calendar Authority, Seasonal Consequences & Deadlines With Teeth

> **Output:** `C1_planintegration[11].md`
>
> **Source baseline:** Plan 38 — The Year Turns: Seasons, Deadlines, and a Clock With Teeth
>
> **Wave:** Continuity Wave 5 — *The Human Interface*
>
> **Depends on:** Plan 20C weather effects; Plan 31 semantic day events; Plan 35 delivery/storage/perishability; Plan 30 autonomous world; Plan 32 travel/routes; Plan 23 power/thermal; Plan 24 labour; Plan 19 ending-state derivation.
>
> **Mandatory execution order:** 38A → 38B → 38C.
>
> **Wave sequencing constraint:** 36A → 35A → 38A → 37A → 38B → 39A → 38C. Seasonal integrations should land only after producer/port seams are enforceable.
>
> **Primary architectural rule:** extend/use the existing `ICampaignCalendar`. Do not create a rival calendar, duplicate year counter, or per-system season arithmetic.
>
> **Primary gameplay rule:** time must become context. The same day number should consistently determine season, year, ambient baseline, seasonal modifiers, and deadline windows across every consumer.
>
> **Guardrails:** no hidden seasonal multipliers, no per-system `day % N` logic, no season mechanic that exists only in UI, no obligation without a warning window, no impossible deadline by construction, and no invented seasonal dependency where the system genuinely does not care.

---

# 0. Mission

ASHFALL already counts time.

It has:
- campaign days;
- season profiles;
- weather windows;
- chapters;
- years;
- multi-year continuity;
- generational systems;
- tribute due dates;
- ambient temperature in one expansion;
- greenhouse light-hour data;
- wildlife migration;
- shelter thermal state;
- travel routes;
- spoilage;
- disease vectors;
- caravans;
- deadlines and obligations scattered across specific systems.

But those facts do not yet behave as one clock.

The current failure is not "the game has no date."

The failure is:

```text
day counter
   │
   ├── weather notices season
   ├── panel prints season
   ├── generational UI prints chapter/year
   └── almost nothing else cares
```

A survival game about nuclear winter cannot treat winter as a label.

Plan 38 turns day progression into a single simulation context:

```text
Campaign Day
    │
    ▼
ICampaignCalendar
    │
    ├── day
    ├── season
    ├── season progress
    ├── days to change
    ├── year
    ├── chapter
    ├── ambient baseline temperature
    └── deterministic seasonal severity context
    │
    ├────────► weather selection bias
    ├────────► greenhouse light/growth
    ├────────► thermal/fuel demand
    ├────────► spoilage/preservation
    ├────────► wildlife migration
    ├────────► travel/ice roads/mud
    ├────────► disease vectors
    ├────────► caravan rhythm
    └────────► commitments/deadlines
```

The second half of the plan turns scattered obligations into one visible ledger:

```text
obligation authored
    │
    ▼
CommitmentSystem
    │
    ├── due window
    ├── counterparty
    ├── amount/item
    ├── settlement
    ├── shortfall
    ├── escalation
    └── remission / renegotiation
    │
    ▼
warning ladder
    │
    ▼
player choice
    │
    ▼
real existing consequence
    │
    ▼
briefing / faction / debt / ending history
```

The game should be able to answer, from one authority:

> What time of year is it, what is about to change, what do I owe, when is it due, and what will happen if I fail?

---

# 1. Source-Evidence Interpretation

## 1.1 A real season model already exists

The source plan identifies `SeasonProfileDef`, seasonal windows, weather weights, and `GetSeasonForDay`.

Therefore:
- reuse this seam;
- do not add a `SeasonSystem`;
- promote season output to a campaign-wide read model.

## 1.2 Only weather and display currently consume it

This is the key integration defect.

A season is not real until non-weather systems read it.

## 1.3 Year/chapter state is currently cosmetic

Generational chapter/year values appear in status strings.

38A must make chapter/year part of the shared calendar context rather than UI-only labels.

## 1.4 Ambient temperature already exists privately

One expansion derives ambient temperature for deep-freeze/radon behavior.

This is a strong signal to promote a shared baseline temperature rather than compute another one.

## 1.5 A real deadline pattern already exists

Warlord tribute already has:
- due day;
- settlement amount;
- next due;
- paid/short/refused;
- downstream consequences.

38C generalizes this pattern instead of creating an unrelated quest timer system.

---

# 2. Non-Negotiable Time Invariants

## INV-38.1 — One calendar authority

All systems obtain:
- day;
- season;
- year;
- chapter;
- ambient seasonal context
from `ICampaignCalendar` / its canonical implementation.

## INV-38.2 — Calendar is deterministic

Same:
- seed;
- day;
- season profile
must yield identical calendar read model.

## INV-38.3 — Calendar state is pure/derivable where possible

Do not persist duplicate values that can be recomputed safely from:
- day;
- seed;
- profile;
- generational authority.

Persist only what is necessary for versioning/catch-up continuity.

## INV-38.4 — Seasons bias weather selection; weather defines consequences

Season does not directly apply hidden "storm damage +20%" if that belongs to weather effects.

## INV-38.5 — Ambient temperature has one source

Thermal, deep-freeze, radon, greenhouse/environment, and other consumers use the same baseline.

## INV-38.6 — No local season arithmetic

No:
- `day % 90`;
- `if day < 60 winter`;
- ad hoc year division;
outside calendar implementation/tests.

## INV-38.7 — Seasonal modifiers are visible

If season changes:
- growth;
- heating;
- spoilage;
- migration;
- travel;
- disease;
- trade
the UI must expose the same modifiers the simulation uses.

## INV-38.8 — Decorative dependencies remain explicit

A system with no meaningful seasonal behavior is marked `DECORATIVE`.

Do not invent fake modifiers just to fill a matrix.

## INV-38.9 — Deadlines use one obligation vocabulary

Tribute, debt, treaty, filing, quota, and delivery windows share one commitment model where their semantics overlap.

## INV-38.10 — Deadlines are achievable by design

Authored obligations must be satisfiable under the intended difficulty/preset with reasonable preparation.

## INV-38.11 — Deadlines warn before punishment

No hidden due-day ambush.

## INV-38.12 — Consequences reuse existing authorities

Missed obligations trigger:
- raid;
- foreclosure;
- stance loss;
- gate closure;
- other existing consequence systems.

No bespoke punishment switch per type.

---

# 3. Definition of Done

Plan 38 is complete only when:

- one `CampaignCalendar` implementation exists behind `ICampaignCalendar`;
- day, season, progress, days-to-end, year, chapter, ambient baseline, and deterministic severity context are available from one read model;
- seasonal profile data supports the campaign's multi-season/multi-year length;
- weather selection consumes seasonal weights but weather effects remain separate;
- ambient temperature is shared with thermal/power and Year-of-Ash consumers;
- all day owners observe one calendar snapshot for the day;
- save/load/catch-up preserves the same calendar;
- HUD/briefing/forecast present day + season + year consistently;
- manual ice-road naming/logic is reconciled;
- source-scan gate forbids local season arithmetic;
- a seasonal matrix exists for every target system;
- greenhouse reads day length/season;
- thermal/power reads ambient temperature;
- spoilage/preservation reads seasonal conditions;
- wildlife migration/trapping reads seasonal bias;
- travel edges can gain/lose real ice-road/thaw states;
- disease vector bias is seasonal where authored;
- caravan rhythm changes seasonally;
- every non-identity modifier is visible/forecastable;
- balance sim demonstrates survivable but meaningfully different seasons;
- seasonal transitions emit Plan-31 events;
- `CommitmentSystem` exists;
- commitments are authored in data;
- one commitments ledger aggregates obligations;
- due windows and warning ladders are visible;
- partial payment/refusal/renegotiation are real decisions where supported;
- consequences integrate existing systems;
- obligations can couple to season and autonomous world state;
- commitments persist/checksum correctly;
- satisfiability gate verifies authored obligations;
- epilogue/faction/story systems can read obligation outcomes;
- all relevant CI/selftests pass.

---

# 4. Phase P0 — Calendar, Season and Obligation Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
ICampaignCalendar definition
implementations
GetSeasonForDay consumers
season-profile schema
season count/windows
weather weights
ambient-temperature producers
chapter/year producers
manual day arithmetic
ice_road references
tribute/debt/treaty due-day systems
existing obligation-like save state
briefing deadline events
```

---

## P0.2 Build time-authority matrix

Create:

`docs/systems/TIME_AUTHORITY_MATRIX.md`

Columns:

```text
concept
current authority
consumers
persisted?
duplicate computation?
target authority
status
```

Rows:
- campaign day;
- season;
- year;
- chapter;
- ambient temp;
- season progress;
- weather severity;
- due dates;
- next tribute;
- treaty windows;
- debt installments;
- census deadlines.

---

## P0.3 Search for local calendar arithmetic

Audit code for patterns:

```text
% 90
% 365
/ 365
dayOfYear
season =
winter =
spring =
year =
chapter =
```

Classify legitimate:
- calendar implementation;
- tests.

Everything else is migration candidate.

---

## P0.4 Build obligation inventory

Create:

`docs/systems/COMMITMENT_SOURCE_MATRIX.md`

Columns:

```text
obligation
owner
counterparty
due rule
amount/item
settlement
partial allowed?
renegotiate?
shortfall effect
escalation
remission
save state
event
ending impact
```

Include:
- warlord tribute;
- ledger debt;
- treaty terms;
- census/registration;
- delivery quotas/contracts;
- crossing/standing obligations;
- any real current schedule-based promises.

---

# TASK 38A — One Campaign Calendar Authority

# 38A.0 Goal

Make one Core read model answer:

```text
what day is it?
what season?
how far through the season?
how long until change?
what year/chapter?
what ambient baseline applies?
```

---

## 38A.1 Read `ICampaignCalendar` first

Do not create a sibling interface.

Preferred:
- extend existing interface safely;
- or wrap existing implementation behind it if binary compatibility requires.

Document decision.

---

## 38A.2 Create `CampaignCalendar`

File:

`Assets/Ashfall.Core/Calendar/CampaignCalendar.cs`

Responsibilities:
- resolve day context;
- season window;
- repeat/drift;
- year/chapter projection;
- ambient baseline;
- deterministic daily seasonal context.

Engine-free.

---

## 38A.3 Define `CampaignCalendarReadModel`

Recommended fields:

```text
day
season_id
season_display_name
season_index
season_progress_0_1
days_into_season
days_to_season_end
year_since_exchange
chapter
ambient_temperature_c
seasonal_severity
day_length_hours
migration_bias
preservation_bias
```

Only expose modifiers with real consumers.

---

## 38A.4 Pure resolve API

Example:

```csharp
CampaignCalendarReadModel ResolveDay(int day);
```

No mutation required for lookup.

---

## 38A.5 Season profile expansion

Current 3 windows may be too flat for multi-year play.

Expand authored profile to a full annual cycle if design requires:

```text
deep winter
late winter / thaw onset
thaw
short summer / ash summer
autumn/freeze-up
```

But preserve user/source requirement of "4 seasons + repeat-with-drift" if that is the intended design.

Avoid real-world season naming if project canon uses custom labels.

---

## 38A.6 Season repeat model

For multi-year campaigns:

```text
base season windows
+ year drift
```

Drift can affect:
- baseline temperature;
- severity;
- duration;
- weather weights.

Data-driven and bounded.

---

## 38A.7 Drift determinism

Same seed/year => same drift.

No wall-clock RNG.

---

## 38A.8 Weather selection join

Define:

```text
season profile
→ weather kind weights
→ WeatherSystem selection
→ WeatherEffects consequences
```

Season never duplicates weather consequence math.

---

## 38A.9 Weather-effect separation test

Change season weighting:
- weather frequency changes.

Same selected weather kind:
- consequence lookup remains from WeatherEffects.

---

## 38A.10 Ambient temperature authority

Promote existing timeline temperature logic to calendar/shared climate context.

Do not leave `YearOfAshHostSession` with a private value.

---

## 38A.11 Ambient temperature composition

Possible:

```text
season baseline
+ daily deterministic variation
+ selected weather modifier
```

Distinguish:
- baseline calendar temp;
- current weather temp delta.

Do not double-apply.

---

## 38A.12 Expose one daily snapshot

`DayAdvancedEventArgs` or owner context should include the resolved calendar snapshot.

All owners use the same object/value for the day.

---

## 38A.13 No owner recomputation

Owners must not independently call:
- season arithmetic;
- temperature RNG.

They receive/read resolved context.

---

## 38A.14 Day catch-up

When save lags days:
- coordinator catch-up resolves each day in order;
- seasonal transitions emitted once;
- no skipped boundary event.

---

## 38A.15 Persistence strategy

Prefer persist:
- current campaign day;
- profile ID/version;
- seed;
- any non-derivable drift state if truly needed.

Derive:
- season;
- progress;
- days-to-end;
- ambient baseline.

---

## 38A.16 Save mid-season test

Save on:
- first day;
- middle;
- last day.

Reload:
- exact same read model.

---

## 38A.17 Save at boundary

Save one day before season transition, load, advance.

Expected:
- exactly one `season_changed`.

---

## 38A.18 Year/chapter integration

Use existing generational authority.

Calendar should expose, not duplicate:
- chapter index;
- elapsed years.

If calendar derives year directly from day, reconcile with generational rules first.

---

## 38A.19 Chapter/year consistency gate

HUD, generational panel, briefing all display the same values.

---

## 38A.20 HUD format

Localized format example:

```text
Day 143 · Late Thaw · Year 5
```

Do not hardcode English.

---

## 38A.21 Forecast season countdown

Forecast panel shows:
- current season;
- days to transition;
- next season;
- major upcoming seasonal modifiers where known.

---

## 38A.22 Preparation payoff

Season countdown should help player prepare for:
- heating;
- planting;
- travel;
- storage;
- obligations.

Not just decorative text.

---

## 38A.23 Ice-road handler reconciliation

Audit `OnTickIceRoadClicked`.

Either:
- rename to what it actually does;
- or move real ice-road behavior to 38B.

No misleading method name.

---

## 38A.24 Calendar source-scan gate

Fail if non-calendar production code contains known local season arithmetic.

Allowlist tests/calendar implementation only.

---

## 38A.25 Boundary tests

Test:
- first campaign day;
- first/last season day;
- year rollover;
- multi-year repeat;
- drift.

---

## 38A.26 Ambient continuity test

Temperature should not jump impossibly at season boundary unless authored.

Use interpolation/transition rules if needed.

---

## 38A.27 Determinism replay

Paired seed:
- identical 400-day calendar curve.

Different seed:
- only seeded daily variation/drift differs where designed.

---

## 38A.28 Runtime complexity

`ResolveDay(day)` should be O(1) or O(number of season windows) with tiny fixed N.

No scanning campaign history.

### 38A DoD

One calendar authority answers what time of year/year/chapter it is, and every consumer receives the same deterministic daily context.

---

# TASK 38B — Seasonal Consequences Across Existing Systems

# 38B.0 Goal

Make each season measurably change multiple already-built systems while keeping every modifier attributable and forecastable.

---

## 38B.1 Write season-system matrix first

Create:

`docs/systems/SEASON_EFFECT_MATRIX.md`

Rows:
- season/phase.

Columns:
- weather;
- greenhouse;
- thermal;
- power;
- spoilage;
- wildlife;
- travel;
- disease;
- caravans;
- dose if genuinely relevant.

Each cell:
- modifier;
- source data field;
- consumer;
- UI exposure;
- test.

Use `DECORATIVE` for identity/no-effect cells.

---

## 38B.2 Data authority

Season modifiers belong in:
- season profile JSON;
- or referenced seasonal modifier catalog.

No C# switch by season ID.

---

## 38B.3 Greenhouse day length

Use calendar `day_length_hours`.

Greenhouse light requirement:
- natural light contribution;
- artificial lighting shortfall;
- power demand.

---

## 38B.4 Greenhouse growth

Crop growth duration/yield can use:
- season;
- light;
- temperature;
according to existing greenhouse data.

No hidden global crop multiplier if light/temperature already explain it.

---

## 38B.5 Blight seasonal bias

Use authored blight resistance/vector data.

Different seasons affect:
- outbreak pressure;
- resistance.

Keep visible.

---

## 38B.6 Winter power problem

Ambient temperature feeds shelter thermal.

Thermal demand feeds power/heating.

This should increase competition between:
- heating;
- greenhouse lights;
- foundry;
- refrigeration;
- radio;
- other loads.

---

## 38B.7 One fuel competition

If generator/furnace/vehicle systems consume shared fuel:
- use same inventory/economy authority;
- season changes demand, not fuel identity.

---

## 38B.8 Thermal warnings

Forecast can indicate:
- expected heating load;
- low-temperature risk.

---

## 38B.9 Seasonal preservation

Feed calendar preservation bias into Plan 35B spoilage calculation.

Cold season:
- longer shelf life in unheated/cellar storage where physically appropriate.

Warm thaw:
- shorter.

---

## 38B.10 Do not double-count refrigeration

Season affects ambient/storage baseline.

Refrigeration applies its own storage effect.

Avoid:
- winter bonus + same cold bonus twice.

---

## 38B.11 Wildlife migration

Feed seasonal context into `WildlifeMigrationSystem`.

Outputs:
- species presence;
- abundance;
- route/migration state.

---

## 38B.12 Trapping consequence

Post-35A:
- actual quarry availability changes real delivered food.

No cosmetic migration labels.

---

## 38B.13 Ice road state

Add real route-edge seasonal state.

Example:
- frozen/open in deep cold;
- risky during freeze-up;
- closed in thaw.

Use Plan 32 edge authority.

---

## 38B.14 Mud/thaw travel

Thaw can:
- increase travel time;
- fuel cost;
- failure risk;
through route modifiers.

---

## 38B.15 Weather vs route separation

Season biases weather and baseline route state.

Selected weather can further modify route.

One route resolver composes both.

---

## 38B.16 Disease vector seasonality

Audit actual four transmission vectors.

Bias only those with plausible authored relation.

Examples from source:
- water-borne in thaw;
- respiratory in cold.

---

## 38B.17 No hidden vector multiplier

Disease panel/briefing can expose:

```text
Respiratory spread risk elevated — cold season
```

---

## 38B.18 Caravan rhythm

Season affects:
- route availability;
- arrival frequency;
- stock composition;
where existing Plan-30/32 systems support.

---

## 38B.19 Trade scarcity

Winter isolation may reduce:
- caravan arrivals;
- fresh goods;
- certain resources.

Do not directly rewrite market prices in calendar if market system already derives scarcity.

---

## 38B.20 Seasonal production/storage loop

Example:

```text
winter
→ greenhouse lighting demand ↑
→ power competition ↑
→ harvest risk ↑
→ ambient spoilage ↓
→ hunting migration changes
→ caravan access ↓
```

The design works through existing authorities.

---

## 38B.21 Seasonal UI summary

Expose each active non-identity modifier from the same Core read model.

Example:

```text
Late Thaw
• Greenhouse natural light +18%
• Cellar shelf life -12%
• Eastern road muddy: +20% travel time
• Water-borne disease risk elevated
```

---

## 38B.22 Forecast lookahead

Show next-season modifier changes.

No hidden surprise at boundary.

---

## 38B.23 Seasonal event vocabulary

Use Plan 31:

```text
season_changed
first_frost
thaw_began
road_opened
road_closed
migration_shifted
cold_chain_improved/degraded
```

Use only semantic events that have real transitions.

---

## 38B.24 Journal parity

Season transitions persist in journal/event log.

---

## 38B.25 Four-season scripted campaign

Run at least one full annual loop.

Track:
- food produced;
- food spoiled;
- fuel burned;
- indoor temperature;
- power shortfall;
- illness;
- travel time/access;
- wildlife yield;
- caravan arrivals.

---

## 38B.26 Balance sweep

Matrix:
- season severity;
- starting food;
- fuel;
- storage;
- greenhouse capacity.

Acceptance:
- prepared strategy survives;
- unprepared strategy suffers;
- no mathematically unavoidable collapse at intended difficulty.

---

## 38B.27 Seasonal severity tiers

If difficulty modifies seasonal harshness:
- data-driven;
- visible;
- deterministic.

---

## 38B.28 Per-cell tests

For every non-identity matrix cell:
- assert modifier is actually consumed.

This is the regression lock.

---

## 38B.29 No decorative wiring

A system with `DECORATIVE` status:
- gets no dummy callback;
- remains explicit in matrix.

### 38B DoD

Winter, thaw, warm season, and freeze-up produce measurably different survival strategies across food, fuel, storage, illness, travel, and trade.

---

# TASK 38C — Generalized Commitments and Deadline Pressure

# 38C.0 Goal

Turn promises, debts, quotas, treaties, and tributes into one visible schedule of obligations with due dates, choices, consequences, and escalation.

---

## 38C.1 Extract the warlord contract

Read current warlord tribute flow.

Identify shared semantics:

```text
obligation id
counterparty
due day/window
amount/item
settle
partial settle
refuse
shortfall
next due
escalation
remission
collector/response
```

---

## 38C.2 Create `CommitmentSystem`

File:

`Assets/Ashfall.Core/Commitments/CommitmentSystem.cs`

Responsibilities:
- register authored obligations;
- activate;
- compute warnings;
- settle;
- track shortfall;
- escalate;
- expose ledger;
- persist state.

Do not absorb domain-specific consequence systems.

---

## 38C.3 Commitment definition schema

Create:

`commitments.json`

Fields:

```text
schema_version
commitments[]
  id
  type
  counterparty_id
  start_condition
  due_window
  settlement
  partial_policy
  renegotiation_policy
  escalation_stages
  remission_conditions
  seasonal_constraints
  consequence_refs
```

Only add fields actual obligations use.

---

## 38C.4 Commitment types

Initial:

```text
tribute
debt_installment
treaty_term
census_filing
delivery_contract
quota
```

Avoid type explosion.

---

## 38C.5 Settlement requirement model

Support:
- currency/ledger value;
- item bill;
- action/flag;
- delivery completion.

Reuse existing bill/effect appliers.

---

## 38C.6 One commitments ledger

Read model:

```text
id
type
counterparty
due_day
days_remaining
required
paid/delivered
status
next_consequence
renegotiation_available
```

---

## 38C.7 Status model

Suggested:

```text
pending
due_soon
due
partially_met
met
missed
escalated
remitted
renegotiated
closed
```

---

## 38C.8 Warning ladder

Data-driven T-minus thresholds:

```text
T-14
T-7
T-3
T-1
due
late
```

Only emit configured thresholds.

---

## 38C.9 Briefing integration

Events:
- `obligation_due`;
- `obligation_met`;
- `obligation_missed`;
- `deadline_escalated`;
- `obligation_renegotiated`.

Use canonical Plan-31 kinds.

---

## 38C.10 Guidance integration

For near-term obligations:
- guidance overlay can surface due date and route.

Do not create modal spam.

---

## 38C.11 Partial payment

If allowed:
- consume/pay actual amount;
- track remaining;
- consequences scale according to data/system.

---

## 38C.12 Refusal

Refusal is explicit action.

It writes:
- stance;
- guilt/flags where authored;
- consequence path;
- ending-readable state.

---

## 38C.13 Renegotiation

Reuse existing choice/effect framework.

Inputs can include:
- faction stance;
- leverage;
- season hardship;
- prior compliance.

---

## 38C.14 Delay

If allowed:
- moves due date;
- increases cost or stance consequence;
according to authored ladder.

---

## 38C.15 Warlord integration strategy

Do not duplicate warlord state.

Preferred:
- commitment read model composes/adapts existing warlord tribute state;
- or migrate shared scheduling state carefully while preserving existing warlord authority.

Document ADR if needed.

---

## 38C.16 Ledger debt integration

Map:
- payment schedule;
- foreclosure stage;
- missed payment
into commitment rows.

Foreclosure remains owned by debt system.

---

## 38C.17 Treaty integration

Treaty obligations:
- delivery;
- access;
- non-aggression condition;
- quota;
as existing treaty terms allow.

Consequence remains in treaty/standing system.

---

## 38C.18 Census/registration integration

If filing/registration has due windows:
- represent in ledger.

Do not invent one if current fiction/system lacks a real obligation.

---

## 38C.19 Delivery-contract integration

Plan 35 output delivery can satisfy item commitments.

Use real delivered quantities, not UI checkbox.

---

## 38C.20 Seasonal coupling

Commitment definitions may:
- start in a season;
- fall due in a season;
- use seasonal difficulty context.

But obligations must remain satisfiable.

---

## 38C.21 Autonomous world coupling

Plan 30 may alter:
- demand;
- remission;
- escalation;
based on counterparty state.

Do not make calendar own faction decisions.

---

## 38C.22 Impossible delivery due to world event

If caravan interception/route closure prevents fulfillment:
- system records cause;
- renegotiation/remission can react if authored.

Player is not omnipotent, but consequence must be traceable.

---

## 38C.23 Satisfiability model

For each authored obligation evaluate:
- starting stock;
- unlock timing;
- production capacity;
- route access;
- reasonable labour/power;
- due window.

---

## 38C.24 Difficulty-specific satisfiability

Check per preset.

Hard may be tight.

None should require impossible content/unlocks.

---

## 38C.25 Satisfiability gate

Data integrity fails when:
- required item unavailable before due;
- amount exceeds theoretical production under intended assumptions;
- counterparty/target missing.

Allow explicit scripted-impossible narrative commitments only if design classifies them as such and consequence is intentional—not hidden accidental impossibility.

---

## 38C.26 Persistence

Register `commitments` section.

Persist:
- active rows;
- due dates;
- paid amounts;
- state;
- escalation stage;
- renegotiation result.

---

## 38C.27 Catch-up behavior

Loading a save after several missed days:
- process warning/due/escalation transitions deterministically;
- do not skip straight to arbitrary final state;
- do not duplicate events.

---

## 38C.28 Deadline event idempotence

Each threshold emits once.

Persist threshold/event state or derive safely.

---

## 38C.29 Ending integration

Commitment outcomes may feed Plan 19:
- reliable;
- defaulted;
- exploitative;
- treaty-breaking;
- debt resolution;
where existing ending matrix has relevant inputs.

Do not invent ending booleans locally.

---

## 38C.30 Memorial/history integration

Major refusals/defaults:
- journal/event history;
- faction standing history.

"Memorialise" here means historical record, not necessarily memorial wall.

---

## 38C.31 Promises & Debts UI

Prefer binding an existing shelved/debt/waystation ledger surface if suitable.

Do not add a fake new panel.

---

## 38C.32 UI layout

Show:
- counterparty;
- requirement;
- due day;
- time remaining;
- progress;
- consequence preview;
- actions.

---

## 38C.33 Calendar cross-link

Click due date/season:
- show calendar/forecast context if a live route exists.

---

## 38C.34 Accessibility

Deadlines:
- text labels;
- numerical days remaining;
- no red-only urgency.

---

## 38C.35 Settlement tests

Per type:
- exact;
- partial;
- overpayment;
- wrong item;
- missing stock.

---

## 38C.36 Escalation tests

Threshold transitions:
- deterministic;
- exactly once;
- existing consequence called.

---

## 38C.37 Renegotiation tests

Choice/effect path updates:
- due date;
- amount;
- stance/flags.

---

## 38C.38 Seasonal interaction tests

Obligation in harsh season:
- visible;
- still satisfiable under designed policy.

---

## 38C.39 Save mid-window

Save at T-3, load, advance.

No duplicate T-3 warning.

---

## 38C.40 Determinism

Same:
- seed;
- calendar;
- counterparty state;
- choices
=> same deadline outcomes.

### 38C DoD

The game schedules pressure the player can see, prepare for, renegotiate, meet, or intentionally fail—and every consequence comes from an existing world system.

---

# 5. Cross-Task Dependency Graph

```text
ICampaignCalendar
      │
      ▼
38A — one calendar
      │
      ▼
38B — seasonal consequences
      │
      ▼
38C — commitments/deadlines
```

Support:

```text
20C WeatherEffects ─────► season selects weather, weather applies consequences
23 power/thermal ───────► winter load
35 production/storage ──► seasonal food and perishability
30 world autonomy ──────► counterparty decisions
32 routes ──────────────► ice roads / caravans
31 events ──────────────► all transitions and deadlines
19 ending ──────────────► long-term obligation outcomes
```

---

# 6. Calendar Read-Model Contract

The read model is a value object for one day.

It should be safe to pass to every day owner.

No owner mutates it.

---

# 7. Seasonal Data Contract

Each season window must define only real shared modifiers:

```text
weather weights
ambient temperature band
day length
migration bias
preservation bias
travel state hints if data-owned
disease vector biases
trade/caravan bias if appropriate
```

Avoid dumping producer-specific formulas into calendar JSON.

---

# 8. Weather/Season Separation

Season answers:

> What weather is more likely and what baseline environment are we in?

WeatherEffects answers:

> What does the weather that actually occurred do?

This separation is mandatory.

---

# 9. Seasonal Modifier Attribution

Every modifier exposed to a system includes a stable source:

```text
season:<season_id>
weather:<weather_kind>
room:<room_id>
power:<state>
```

UI can explain composed effects.

---

# 10. Seasonal Transition Semantics

At boundaries emit:
- transition;
- first frost/thaw/road state when real.

Do not emit daily "season ticked."

---

# 11. Deadline Contract

A commitment is not a quest.

It is a scheduled obligation with:
- creditor/counterparty;
- due window;
- settlement;
- consequence.

Quests may create commitments; commitment system owns timing once created.

---

# 12. Commitment Transaction Semantics

Settlement:

1. validate obligation active;
2. validate requested settlement;
3. preview resource/action effect;
4. consume via authoritative bill/effect path;
5. update paid state;
6. compute met/partial;
7. emit event;
8. invoke consequence/remission if transition;
9. persist.

No double settlement on stale UI request.

---

# 13. Deadline State Machine

```text
inactive
→ pending
→ due_soon
→ due
→ met / partially_met / missed
→ escalated / renegotiated / remitted
→ closed
```

Not every obligation uses every state.

---

# 14. Obligation Warning Policy

Warning thresholds must be:
- visible;
- data-driven;
- localized;
- non-spammy.

The same threshold never emits twice.

---

# 15. Calendar + Deadline UX

Dashboard should answer:

```text
Today
Season
Days until season change
Next 3 obligations
Next major expected seasonal pressure
```

No need for a giant calendar UI if existing dashboard/briefing can carry it.

---

# 16. Balance Strategy

Seasonal harshness and deadlines interact.

Test policy sweeps:

```text
season severity
× food reserve
× fuel reserve
× storage
× obligation load
```

Reject combinations that create unavoidable failure on intended difficulty.

---

# 17. Multi-Year Continuity

Track across 400+ days:
- seasonal repeat/drift;
- generational year/chapter;
- memorial accumulation;
- obligation cycles;
- migration;
- world state.

No drift from save/load or catch-up.

---

# 18. Failure Injection Matrix

## N38.1 Two systems compute different season for same day
Expected: source-scan / integration failure.

## N38.2 Save/load at season boundary
Expected: one transition.

## N38.3 Weather selection ignores season
Expected: weight-distribution test fails.

## N38.4 Selected weather consequence changes by hidden season multiplier
Expected: separation test fails.

## N38.5 Greenhouse ignores day length
Expected: matrix test fails.

## N38.6 Winter power demand not reflected
Expected: thermal/power integration test fails.

## N38.7 Ice road remains text-only
Expected: route-state acceptance fails.

## N38.8 Deadline authored with unavailable item
Expected: satisfiability/data gate fails.

## N38.9 Deadline misses warning window
Expected: warning test fails.

## N38.10 Reload duplicates deadline escalation
Expected: idempotence test fails.

## N38.11 Warlord state forked into duplicate ledger
Expected: authority test fails.

## N38.12 Decorative system receives meaningless season multiplier
Expected: season matrix review/gate rejects unowned field.

---

# 19. Persistence Matrix

| State | Persist? | Authority |
|---|---:|---|
| current day | yes | campaign |
| season | derive | calendar |
| season progress | derive | calendar |
| year | derive/read existing | generational/calendar |
| chapter | existing authority | generational |
| ambient baseline | derive | calendar |
| seasonal weather roll context | derive/seeded | calendar/weather |
| active commitments | yes | CommitmentSystem |
| due dates | yes/derived from activation | commitment |
| payment progress | yes | commitment |
| escalation stage | yes | commitment |
| warnings emitted | yes or derivable idempotently | commitment |

---

# 20. UI Acceptance

## Calendar/HUD
- day;
- season;
- year;
- days to change.

## Seasonal detail
- actual active modifiers;
- next transition.

## Commitments
- due;
- remaining;
- progress;
- consequence;
- actions.

No hidden multiplier.

---

# 21. Accessibility

Use:
- text + icons + color;
- keyboard navigation;
- numerical remaining days;
- screen-reader labels where current framework supports.

---

# 22. Performance Guardrails

Calendar:
- O(1) / tiny fixed lookup;
- no historical scan.

Commitments:
- iterate active obligations only;
- no full catalog reparsing per day.

---

# 23. CI / Static Gates

Recommended:

```text
calendar_single_authority
no_local_season_arithmetic
season_effect_matrix_coverage
commitment_satisfiability
commitment_reference_integrity
season_localization
commitment_event_completeness
```

---

# 24. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/triad-drift-gate.sh
godot --headless --path . -- --runtime-scale-selftest
bash scripts/ci/verify-fast.sh
```

Also:
- `ashfall-seed-replay` 400-day curve;
- `ashfall-balance-sim` seasonal severity × stock;
- expedition selftest for route seasonality;
- expansions selftest for commitments.

---

# 25. Recommended Commit Breakdown

```text
38A-1 time authority audit + local arithmetic gate
38A-2 CampaignCalendar read model
38A-3 season profile expansion
38A-4 weather/ambient integration
38A-5 day-owner snapshot + persistence/catch-up
38A-6 HUD/forecast/ice-road naming
38A-7 deterministic 400-day tests

38B-1 season-effect matrix
38B-2 greenhouse/light/growth
38B-3 thermal/power/fuel
38B-4 preservation/cold chain
38B-5 wildlife/trapping
38B-6 route/ice-road/thaw
38B-7 disease/caravans
38B-8 events/UI/balance sweep

38C-1 obligation source inventory + warlord extraction
38C-2 CommitmentSystem + catalog
38C-3 settlement ledger
38C-4 warning/escalation
38C-5 debt/treaty/census/delivery adapters
38C-6 seasonal/world coupling
38C-7 satisfiability gate
38C-8 UI/persistence/ending integration
```

---

# 26. Risk Register

## R38.1 Seasonal balance shock

Mitigation:
- matrix;
- balance sweep;
- staged wiring.

## R38.2 Temperature authority migration changes many systems

Mitigation:
- baseline curves;
- one source;
- regression tests.

## R38.3 Hidden double modifiers

Mitigation:
- season/weather separation;
- attribution.

## R38.4 Calendar drift over long campaigns

Mitigation:
- 400-day paired-seed replay;
- save/load boundary tests.

## R38.5 Deadline system duplicates domain logic

Mitigation:
- commitment schedules only; domain consequences remain in owners.

## R38.6 Obligations become impossible

Mitigation:
- satisfiability gate by difficulty.

## R38.7 Warning spam

Mitigation:
- configured thresholds;
- idempotent emissions.

## R38.8 UI turns into another fake calendar panel

Mitigation:
- reuse HUD/briefing/existing ledger surfaces when possible.

---

# 27. Acceptance Checklist

## 38A — Calendar

- [ ] ICampaignCalendar re-read
- [ ] no sibling calendar introduced
- [ ] CampaignCalendar implemented
- [ ] read model defined
- [ ] pure ResolveDay API
- [ ] season profile expanded appropriately
- [ ] repeat-with-drift authored
- [ ] deterministic drift
- [ ] weather selection consumes seasonal bias
- [ ] WeatherEffects remains consequence authority
- [ ] single ambient temperature baseline
- [ ] daily snapshot shared by owners
- [ ] no owner local recomputation
- [ ] catch-up crosses boundaries correctly
- [ ] persistence strategy documented
- [ ] mid-season round-trip
- [ ] boundary round-trip
- [ ] generational year/chapter reconciled
- [ ] HUD values consistent
- [ ] forecast countdown
- [ ] preparation payoff visible
- [ ] ice-road handler reconciled
- [ ] local season arithmetic gate
- [ ] boundary tests
- [ ] ambient continuity
- [ ] paired-seed determinism
- [ ] runtime-scale O(1)

## 38B — Seasonal consequences

- [ ] season-system matrix authored
- [ ] modifiers live in data
- [ ] greenhouse day length wired
- [ ] greenhouse growth wired
- [ ] blight bias wired if real
- [ ] thermal consumes ambient
- [ ] power demand reflects heat
- [ ] fuel competition real
- [ ] thermal warnings forecastable
- [ ] preservation bias wired
- [ ] no refrigeration double count
- [ ] wildlife migration seasonal
- [ ] trapping receives migration
- [ ] real ice-road state
- [ ] thaw/mud route state
- [ ] route composes season + weather
- [ ] disease vector bias
- [ ] disease attribution visible
- [ ] caravan rhythm seasonal
- [ ] trade scarcity derived through world
- [ ] seasonal production/storage loop measurable
- [ ] modifier UI shows real values
- [ ] next season forecastable
- [ ] semantic transition events
- [ ] journal parity
- [ ] four-season scripted campaign
- [ ] balance sweep passes
- [ ] severity data-driven
- [ ] per-cell integration tests
- [ ] DECORATIVE rows remain honest

## 38C — Commitments

- [ ] warlord shared contract extracted
- [ ] CommitmentSystem created
- [ ] commitments.json validated
- [ ] type set minimal
- [ ] settlement requirements use existing bills/effects
- [ ] one commitments ledger
- [ ] status model
- [ ] warning ladder
- [ ] briefing events
- [ ] guidance integration
- [ ] partial payment
- [ ] explicit refusal
- [ ] renegotiation
- [ ] delay policy where authored
- [ ] warlord state not duplicated
- [ ] debt adapted
- [ ] treaty adapted
- [ ] census/registration adapted only if real
- [ ] delivery contracts use Plan-35 quantities
- [ ] seasonal coupling
- [ ] autonomous-world coupling
- [ ] impossible-world-event causes attributable
- [ ] satisfiability model
- [ ] difficulty-specific satisfiability
- [ ] satisfiability gate
- [ ] persistence
- [ ] catch-up deterministic
- [ ] deadline events idempotent
- [ ] ending integration uses existing matrix
- [ ] historical/journal recording
- [ ] real Promises & Debts surface
- [ ] due/remaining/progress UI
- [ ] calendar cross-link
- [ ] accessible urgency
- [ ] settlement maths tests
- [ ] escalation tests
- [ ] renegotiation tests
- [ ] seasonal interaction tests
- [ ] mid-window save test
- [ ] determinism

---

# 28. Ship / No-Ship Gate

**SHIP** only if:

```text
campaign_calendar_authorities == 1
AND systems_computing_season_locally == 0
AND calendar_same_day_same_snapshot == true
AND weather_selection_uses_season == true
AND weather_effects_remain_single_authority == true
AND ambient_temperature_authorities == 1
AND season_matrix_nonidentity_cells_tested == 100_percent
AND greenhouse_reads_seasonal_context == true
AND thermal_reads_calendar_ambient == true
AND spoilage_reads_seasonal_context == true
AND wildlife_reads_seasonal_context == true
AND travel_has_real_seasonal_state == true
AND hidden_season_multipliers == 0
AND seasonal_balance_sweep == pass
AND commitment_schedule_authorities == 1
AND authored_commitments_unsatisfiable == 0
AND deadline_warning_windows_missing == 0
AND duplicate_deadline_events == 0
AND warlord_state_forks == 0
AND commitment_save_roundtrip == pass
AND multi_year_seed_replay == pass
AND runtime_scale_selftest == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 29. Implementer Handoff

1. Start from the existing `ICampaignCalendar`.
2. Do not add another calendar or season service.
3. Build one immutable day read model.
4. Keep weather selection and weather effects separate.
5. Promote ambient temperature into shared authority.
6. Pass one daily calendar snapshot to all owners.
7. Add source-scan protection against local season arithmetic.
8. Write the season-system matrix before wiring any consumer.
9. Only add seasonal modifiers that have real mechanics and data.
10. Make every modifier forecastable from the same Core values.
11. Make ice roads a real route state or remove the misleading concept.
12. Generalize the warlord due-date pattern rather than replacing it.
13. Let CommitmentSystem schedule and track; let domain systems punish/reward.
14. Make every deadline warn before consequence.
15. Gate authored obligations for satisfiability.
16. Preserve exact-once deadline transitions across save/load/catch-up.
17. Feed obligation outcomes into existing history/faction/ending state.
18. Close with a 400-day deterministic replay and balance sweep across seasonal severity and reserves.

---

# 30. Final Outcome

When this plan is complete, ASHFALL's clock stops being a label and becomes a governing context.

The same campaign day now means the same season, year, chapter, ambient temperature, and seasonal modifiers everywhere. Weather is selected through the seasonal profile, but weather consequences stay in the WeatherEffects authority. The greenhouse feels short winter days. Heating demand forces the power grid to matter. Cold preserves food while thaw accelerates loss. Wildlife moves. Ice roads open and close. Disease vectors shift. Caravans arrive on a seasonal rhythm.

The player can see these changes coming. The HUD and forecast show not only what season it is, but how long remains and which real systems are about to become easier or harder.

Promises also gain time. Tribute, debt, treaty terms, quotas, deliveries, and other real obligations appear in one ledger with due dates, warning windows, progress, choices, and consequences. A missed promise is not a surprise script; it is a visible decision that lands in an existing faction, debt, raid, standing, or ending system.

Multi-year play finally has a clock the simulation obeys.

The result is not a new calendar minigame. It is the existing weather, thermal, production, storage, travel, disease, caravan, faction, debt, and generational systems finally agreeing about what day it is—and what that day demands.
