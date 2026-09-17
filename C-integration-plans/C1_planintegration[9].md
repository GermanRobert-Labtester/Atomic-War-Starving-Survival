# C1 — Flagship Integration Plan [9]: Intel Has To Be Worth Something — Radio, Traces, Reliability & Comms Infrastructure

> **Output:** `C1_planintegration[9].md`
>
> **Source baseline:** Plan 33 — Intel Has To Be Worth Something: Radio, Traces, and Secrets
>
> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
>
> **Depends on:** Plan 31 semantic event layer; Plan 32A/32C for world nodes/reveal/travel; Plan 23 power authority; Plan 21 equipment condition; Plan 24 labour/fitness/skill; Plan 26A shipped data resolution; Plan 30A/30B for autonomous factions and counter-information.
>
> **Mandatory execution order:** 33A → 33B → 33C.
>
> **Wave order constraint:** 31A → 32A → 30A → 33A. Do not implement signal outcomes before there are real places to reveal, and do not implement false intelligence before there are autonomous actors capable of producing it.
>
> **Primary principle:** intelligence must change what the player can know, where the player can go, what the player can risk, or what the player can gain. Listening that produces only labels is not gameplay.
>
> **Guardrails:** no new radio mechanic beyond authored data and existing system seams; no new radio audio batch; no opaque procedural signal RNG; no "knowledge currency" disconnected from existing progression; no undetectable lies; no free listening; no new event bus.

---

# 0. Mission

ASHFALL already owns most of the pieces required for a compelling information economy:

- 118 authored broadcasts across multiple catalogs;
- a live tuner;
- signal triangulation;
- persisted device battery/calibration;
- weather and EMP states;
- existing radio cues;
- faction broadcasts;
- distress signals with frequency, source, outcome type, trace days, clarity-graded fragments;
- `OnLocationRevealed`;
- expedition targeting;
- world nodes;
- power;
- labour/skills;
- equipment condition;
- journal/briefing event reporting.

Yet those pieces currently do not form a complete loop.

The reported failures are structural:

```text
radio_distress_signals.json
    ↓
no loader
    ↓
no live system
    ↓
no playable objective

OnLocationRevealed
    ↓
label
    ↓
no travel unlock

knowledge_points
    ↓
authored data only
    ↓
no consumer

battery/calibration/weather/operator
    ↓
separate facts
    ↓
no reception-quality function

tropospheric_radio_relay
    ↓
routed console
    ↓
no authority
```

Plan 33 turns the radio layer into a scarce, fallible, actionable intelligence system.

The target architecture is:

```text
AUTHORED BROADCAST / DISTRESS DATA
             │
             ▼
      Radio Catalog Authority
             │
             ▼
      Radio / Distress System
             │
      ┌──────┼───────────────┐
      ▼      ▼               ▼
 detect   trace progress   resolve/lost
      │      │               │
      └──────┼───────────────┘
             ▼
     DayEventKinds (Plan 31)
             │
             ▼
      Briefing / Journal / Diagnostics
             │
             ▼
   Real world knowledge / route unlock
             │
             ▼
         expedition choice
```

Reception quality must become:

```text
device condition
× battery
× calibration
× antenna/relay coverage
× weather / EMI
× operator skill / fatigue
= reception quality [0..1]
```

And long-range infrastructure must become:

```text
relay graph node
   │
   ├── coverage
   ├── condition
   ├── upkeep
   ├── power
   ├── territory risk
   └── repair duty
```

The player should be able to answer:
- What did I hear?
- How sure am I?
- Why is the signal bad?
- What would improve it?
- Is tracing worth the power and labour?
- If I trust it, where can I go?
- If I build relays, what do I gain?
- If a hostile faction lies, how can I prove it?

---

# 1. Source-Evidence Interpretation

## 1.1 Radio infrastructure is real

The existing radio/tuner/triangulation stack is not a prototype-only concept.

Therefore:
- keep current tuner;
- reuse current `OnLocationRevealed`;
- reuse current cues;
- reuse current persistence seams;
- do not build a replacement "intel framework".

## 1.2 The distress catalog is authored but unreachable

`radio_distress_signals.json` reportedly includes:
- frequency ID;
- MHz;
- source;
- outcome type;
- trace duration;
- per-day fragments;
- clarity values.

This is enough to define the mechanic.

The implementation should conform to the data, not rewrite the data around new mechanics.

## 1.3 Location reveal does not currently unlock travel

The reveal signal is semantically incomplete until it changes world/expedition state.

Plan 33A must integrate with Plan 32's world node visibility/known-location contract.

## 1.4 Listening has no cost

In a scarcity game:
- time;
- duty;
- power;
- battery;
are opportunity costs.

Radio intel becomes gameplay only when attention competes with shelter priorities.

## 1.5 Reliability factors already exist separately

Battery, calibration, weather, EMP, condition, and operator skill are already represented or adjacent.

33B should multiply/compose them in one transparent function.

## 1.6 The relay console is currently a false affordance

33C has only two acceptable outcomes:

A. give it a real `CommsNetworkSystem` authority and at least one mutating action;
B. remove/shelve the route and keep the concept on backlog.

A routed fake console is not acceptable.

---

# 2. Non-Negotiable Intel Invariants

## INV-33.1 — Authored distress data is the authority

Trace duration, fragments, frequencies, source names, outcome types, and clarity values come from authored data.

Do not replace them with C# constants.

## INV-33.2 — Every authored distress signal has a playable chain

Every `freq_distress_*` definition must resolve to:

```text
detect
→ trace
→ maintain/loss
→ resolve
→ outcome
```

No dead definitions.

## INV-33.3 — Trace state persists

Save/load mid-trace resumes the same:
- signal;
- day progress;
- clarity;
- outcome state.

## INV-33.4 — Reveals change real world knowledge

A resolved location reveal must update the same authority used by:
- map;
- expedition targeting;
- travel unlock.

A label-only reveal is incomplete.

## INV-33.5 — Reception quality is one Core function

No duplicate reliability formulas in:
- panel;
- host;
- triangulation;
- relay.

## INV-33.6 — Reliability is attributable

The player can inspect why quality is high/low.

## INV-33.7 — Wrong intel is deterministic and investigable

Misdirection must:
- derive from visible reliability;
- use seeded RNG if needed;
- be correctable via re-trace/corroboration;
- never be impossible to disprove.

## INV-33.8 — Listening costs something

At least:
- power;
- operator time/duty;
- battery/condition where current systems support them.

## INV-33.9 — Operator identity matters

Skill and fitness from Plan 24 affect detection/reception.

## INV-33.10 — Knowledge is not a new isolated currency

If `knowledge_points` is activated:
- route into existing research/knowledge progression;
- otherwise retire/replace the field.

## INV-33.11 — Relay coverage has real consequences

Coverage affects at least:
- intel reliability;
- expedition contact;
- caravan notification.

## INV-33.12 — Relay state is authoritative and persisted

No panel-local relay state.

---

# 3. Definition of Done

Plan 33 closes only when:

- distress catalog loads through shipped data path;
- malformed catalog produces diagnostics rather than silent empty results;
- new state is registered in save architecture;
- all five authored distress signals are detectable and traceable;
- trace progress uses authored day/clarity data;
- weather/power/equipment/operator factors can stall/degrade trace;
- signal detection/progress/resolution/loss emit Plan-31 semantic kinds;
- each outcome type has a registered handler;
- location outcomes reveal real world nodes;
- loot/cache outcomes feed real expedition/loot authority;
- survivor/community outcomes integrate through existing visitor/census path where appropriate;
- threat outcomes feed actual encounter/world risk;
- listening consumes power and labour;
- radio cues fire from existing cue family;
- reliability is one deterministic 0..1 Core calculation;
- UI shows factor breakdown;
- low reliability can produce ambiguous/wrong-but-correctable results;
- EMP/power/relay failures can remove channels;
- `knowledge_points` is connected or retired;
- false faction intel can be contradicted by independent source;
- corroboration can raise confidence;
- radio device state survives save/load;
- relay console either becomes live through `CommsNetworkSystem` or is removed from player navigation;
- relay graph has coverage, condition, upkeep, repair, power, and territory interaction;
- relay events emit through Plan 31;
- relay state is checksummed/persisted;
- no duplicate campaign authority is constructed at panel bind;
- content utilization records distress catalog as consumed/effect-producing;
- all required CI/selftests pass.

---

# 4. Phase P0 — Re-verify Data, Radio, World & Persistence Seams

## P0.1 Capture repository baseline

Record:

```text
commit SHA
branch
dirty-file count
radio catalog files/counts
distress catalog schema
distress catalog loader callers
SignalTriangulationSystem consumers
OnLocationRevealed subscribers
known-location authority
expedition target validation
radio cue ids
inventory battery/calibration persistence
power authority
weather/EMP authority
equipment condition authority
skill authority
duty roster authority
tropospheric_radio_relay maturity verdict
save section count
content exemption row for distress catalog
```

---

## P0.2 Read all relevant files before design

Read:
- all radio catalogs;
- item/device definitions for radios/antennas;
- `SignalTriangulationSystem`;
- `RadioHostSession`;
- `TriangulationPanel`;
- `RadioPanel`;
- world node/reveal authority from Plan 32;
- expedition known-location validation;
- power grid;
- equipment condition;
- inventory device state;
- weather/EMP;
- skill/fitness;
- duty roster;
- visitor/census systems;
- faction/world autonomy from Plan 30;
- relay panel route;
- waystation network;
- save registry/hub;
- content utilization exemptions.

---

## P0.3 Build broadcast inventory

Create:

```text
catalog
broadcast_id
frequency_id
frequency_mhz
type
audio_cue
distress?
traceable?
outcome_type
content_exempt?
live_loader?
live_consumer?
runtime_effect?
```

Use current measured count, not assumed 118 if source changed.

---

## P0.4 Build distress outcome matrix

For all authored signals:

```text
signal_id
frequency
source
days_to_trace
fragments
outcome_type
expected target id
target exists?
handler exists?
route unlock?
loot/population/threat integration?
```

Any missing target is a data-integrity failure or requires authored correction.

---

# TASK 33A — Distress Signals Become Objectives

# 33A.0 Goal

Turn the authored distress catalog into five complete, stateful, player-actionable signal chains.

---

## 33A.1 Define Core data contracts

Create contracts matching actual JSON:

```text
DistressSignalCatalog
DistressSignalDefinition
DistressSignalFragment
DistressSignalOutcomeDefinition / fields
```

Use exact authored schema naming.

Avoid speculative fields.

---

## 33A.2 Loader implementation

Create:

`Assets/Ashfall.Core/Radio/DistressSignalCatalogLoader.cs`

Requirements:
- `SystemTextJsonSerializer`;
- schema/version validation;
- duplicate ID/frequency validation;
- diagnostics on malformed shape;
- deterministic ordering.

---

## 33A.3 Malformed catalog behavior

Bad catalog:
- does not silently produce empty set;
- diagnostics include logical path and shape;
- production host can fail safe;
- data-integrity selftest fails when required catalog invalid.

---

## 33A.4 Distress signal runtime state

Create state:

```text
signal_id
detected_day
trace_days_completed
current_fragment_day
current_clarity
state
resolved_outcome
last_quality
lost_reason
```

Possible states:
- undiscovered;
- detected;
- tracing;
- stalled;
- lost;
- resolved.

---

## 33A.5 Core `DistressSignalSystem`

Responsibilities:
- load definitions;
- detect available signal;
- start trace;
- advance trace;
- apply reliability input;
- resolve/loss;
- expose state;
- emit domain event/callback.

Do not own:
- Godot UI;
- power system;
- duty roster;
- world map directly.

Use injected callbacks/services.

---

## 33A.6 Trace progression from authored fragments

Each day:
- determine target fragment/day;
- combine authored clarity with reception quality;
- reveal appropriate text/coordinate precision;
- increment trace progress only if maintenance conditions met.

No hidden extra time unless data/rule explicitly supports it.

---

## 33A.7 Maintainable, not automatic

Trace requires:
- power available;
- receiver operational;
- operator time;
- acceptable reception.

If one fails:
- stall;
- degrade;
- or lose signal according to explicit rules.

---

## 33A.8 Lost signal policy

Define:
- temporary loss vs terminal loss;
- recovery window;
- whether trace progress decays;
- whether signal can be reacquired.

Prefer minimal rule consistent with authored `days_to_trace`.

---

## 33A.9 Day event kinds

Use Plan 31 vocabulary.

Required semantic kinds:

```text
signal_detected
trace_progress
signal_resolved
signal_lost
```

If Plan 31 registry uses generalized names, map accordingly.

No string literals.

---

## 33A.10 Event payload semantics

Example:

### `signal_detected`
- PrimaryId: signal ID
- SecondaryId: frequency ID
- Numeric: initial quality

### `trace_progress`
- PrimaryId: signal ID
- SecondaryId: fragment/day
- Numeric: quality/progress

### `signal_resolved`
- PrimaryId: signal ID
- SecondaryId: outcome ID/type
- Numeric: final confidence

### `signal_lost`
- PrimaryId: signal ID
- SecondaryId: reason
- Numeric: last quality

---

## 33A.11 Briefing routing

Map:
- detected/progress → radio/triangulation;
- resolved location → map/expedition;
- lost → radio.

Use only live panel routes.

---

## 33A.12 Outcome-handler registry

Create one registry:

```text
outcome_type → handler
```

Avoid `switch` scattered across host.

Each handler:
- validates target;
- mutates existing authority;
- returns structured result.

---

## 33A.13 Survivor-community outcome

Use existing:
- map reveal;
- visitor/airlock triage;
- voluntary register/census;
where actual data supports population arrival/contact.

Do not automatically add population merely because signal says "community."

Separate:
- discovered community location;
- possible future visitor/population event.

---

## 33A.14 Cache/loot outcome

Resolve:
- world node;
- expedition target;
- loot table.

No direct inventory reward from radio resolution unless authored mechanic explicitly says so.

Radio reveals opportunity; expedition collects it.

---

## 33A.15 Threat outcome

Resolve into existing:
- encounter risk;
- territory/faction threat;
- route hazard;
depending on outcome data.

Do not spawn bespoke scripted enemy system.

---

## 33A.16 World node reveal

Use Plan 32 authority.

On resolution:
- node moves from unknown/hidden to known/revealed;
- map reflects it;
- expedition validation accepts it if otherwise valid.

Reference identity test between reveal authority and expedition-known-location authority.

---

## 33A.17 Travel unlock integration test

Journey:

```text
target node hidden
→ resolve signal
→ node revealed
→ expedition target list contains node
```

This is the minimum "intel pays" test.

---

## 33A.18 Operator duty cost

Tracing must reserve/consume:
- radio operator assignment;
- hours per day;
- duty slot/capacity.

Use Plan 24 roster authority.

No hidden free operator.

---

## 33A.19 Operator fitness

Use Plan 24A verdict.

Blocked:
- incapacitated/quarantined per role requirements.

Impaired:
- lower quality or more hours where data/rules allow.

---

## 33A.20 Power draw

Use Plan 23.

Radio operation consumes:
- watts;
- duration;
- load priority.

No direct boolean `hasPower` only if actual grid supports real draw.

---

## 33A.21 Battery use

If receiver battery is relevant:
- drain according to operating hours;
- persisted inventory device state;
- low battery feeds quality.

Avoid duplicate battery model.

---

## 33A.22 Existing cue reuse

Use:
- tune;
- static;
- signal-lock;
- morse.

No new cue family.

Exactly one cue per semantic transition.

---

## 33A.23 UI trace state

Radio/Triangulation panel shows:
- source/frequency;
- trace day N/D;
- current fragment;
- current clarity;
- operator;
- power cost;
- reliability summary;
- action state.

---

## 33A.24 Save section

Register distress runtime state via:
- `SaveSectionRegistry`;
- `SaveStoreHub` facade;
- checksum/contract matrix.

No unchecksummed ad hoc file.

---

## 33A.25 Mid-trace save test

```text
detect
→ trace 2/4
→ save
→ load
→ continue
```

Expected:
- same signal;
- same progress;
- same fragment;
- same deterministic final outcome.

---

## 33A.26 Content exemption removal

Once runtime consumer produces effect:
- remove distress catalog from exemption registry;
- update generated content policy;
- content-utilization expects `EFFECT_PRODUCED`.

---

## 33A.27 Per-authored-signal test

For every current `freq_distress_*`:
- loader resolves;
- trace starts;
- authored fragment sequence reachable;
- outcome handler registered;
- target valid;
- end-to-end chain playable.

---

## 33A.28 Determinism

Same:
- seed;
- operator;
- power;
- weather;
- device state;
- decisions

=> same:
- fragments;
- quality outcomes;
- loss/resolution;
- final target.

---

## 33A.29 Docs

Create/extend:
`docs/radio/SIGNALS.md`

Include:
- catalog authority;
- trace algorithm;
- reliability dependency seam;
- outcome types;
- persistence;
- events;
- tests/gates.

### 33A DoD

Every authored distress signal becomes a real opportunity the player can hear, maintain, resolve, and act on.

---

# TASK 33B — Reception Reliability, Misreading, Knowledge & Counter-Information

# 33B.0 Goal

Make signal quality a deterministic function of visible physical and human factors.

The player must be able to understand why a reading is unreliable and how to improve it.

---

## 33B.1 Define `ReceptionQuality`

Create a Core calculation with output:

```text
quality: 0..1
factors:
  device_condition
  battery
  calibration
  antenna_relay
  weather_emi
  operator_skill
  operator_fitness
```

---

## 33B.2 Factor normalization

Each factor maps to 0..1.

Clamp explicitly.

No NaN/Infinity.

Document:
- source;
- range;
- neutral value.

---

## 33B.3 Combination formula

Default source plan proposes multiplicative model:

```text
Q = device × battery × calibration × relay × weather × operator
```

Validate whether strict multiplication is too punishing.

If using weighted/geometric variant:
- document;
- data-drive weights where reasonable.

One function only.

---

## 33B.4 Breakdown DTO

Expose:

```text
overall_quality
factor_values
dominant_penalties
human-readable reason ids
```

UI renders reasons.

---

## 33B.5 Device condition

Read from Plan 21 equipment condition.

Broken/degraded radio:
- lower quality;
- potentially unavailable below threshold.

No separate radio-condition field.

---

## 33B.6 Battery factor

Use persisted inventory device battery.

Low battery:
- visible;
- degrades quality;
- can shut receiver off.

---

## 33B.7 Calibration factor

Use existing calibration state.

Mis-calibrated device:
- degraded quality;
- potential frequency offset/ambiguity if current tuner supports it.

Persist across load.

---

## 33B.8 Antenna/relay factor

Before 33C:
- use local antenna state/default coverage.

After 33C:
- query `CommsNetworkSystem`.

No duplicate local relay math.

---

## 33B.9 Weather/EMI factor

Use:
- storm;
- EMP;
- atmospheric conditions;
- interference.

Data-drive effect multipliers.

---

## 33B.10 Operator skill

Use campaign skill authority.

Relevant skill ID must exist in current taxonomy.

No hardcoded "radio skill" if catalog uses another skill.

---

## 33B.11 Operator fitness/fatigue

Use Plan 24 fitness/needs.

Examples:
- fatigue;
- illness;
- stress;
can reduce effective listening.

Do not create radio-only fatigue track.

---

## 33B.12 Quality attribution

UI:

```text
Reception 0.42
- Calibration: 0.70
- Storm interference: 0.65
- Operator fatigue: 0.85
- Battery: 0.95
```

Highlight top penalties.

---

## 33B.13 Clarity mapping

Combine:
- authored fragment clarity;
- reception quality.

Define deterministic mapping to:
- clear;
- partial;
- ambiguous;
- wrong/misaligned.

---

## 33B.14 Wrong resolution

Below threshold:
- wrong coordinate fragment;
- ambiguous wording;
- incorrect confidence;
using authored alternatives where available.

If data lacks wrong variants:
- prefer ambiguity/uncertainty over fabricated narrative.

Do not invent false text that cannot be traced to authored data unless an explicit counter-intel system supplies it.

---

## 33B.15 Seeded misread selection

Use `ISeededRng`.

Same seed/state => same misread.

Save/load does not reroll.

---

## 33B.16 Correction loop

Player can improve:
- calibration;
- antenna;
- operator;
- power;
- timing/weather;
- corroboration.

Then re-trace/relisten.

No save-scumming as intended correction path.

---

## 33B.17 Jam/outage states

Channels may be unavailable during:
- EMP-class weather;
- power-off;
- broken relay;
- severe receiver failure.

UI distinguishes:
- no signal;
- jammed;
- receiver offline;
- out of coverage.

---

## 33B.18 `knowledge_points` audit

Find every authored use.

Decide:

A. integrate into existing knowledge/research progression;
B. migrate to existing field name/system;
C. delete/retire if redundant.

No standalone new currency.

---

## 33B.19 Knowledge reward semantics

If integrated:
- partial decrypt grants knowledge;
- full resolution may grant more;
- research system owns balance;
- radio does not own persistent total.

---

## 33B.20 Cipher arc decision

Audit:
- narrative arc events;
- audio logs;
- environmental texts;
- moral choice stubs.

If a multi-stage cipher chain can be implemented with existing content:
- wire it.

If not:
- downgrade/remove current registry claim;
- leave backlog item.

Do not half-ship.

---

## 33B.21 Cipher chain minimal proof

If implemented:

```text
broadcast A
→ decode clue
→ broadcast B
→ location clue
→ real node reveal
→ outcome
```

No ARG purely in UI text.

---

## 33B.22 Counter-information source

With Plan 30:
- hostile faction may broadcast false/misleading statement.

The lie must correspond to:
- world fact;
- territory state;
- route condition;
- faction intent;
that another channel can check.

---

## 33B.23 No undetectable lies

Every false report must have at least one verification path:
- scout;
- caravan;
- independent radio source;
- map observation;
- expedition outcome.

If none, do not emit false claim.

---

## 33B.24 Confidence model

Intel record may store:

```text
claim_id
subject_id
source_channel
source_id
confidence
corroborators
contradictions
status
```

Use existing knowledge model if one exists.

Do not create broad intelligence database unless required.

---

## 33B.25 Corroboration

Two independent sources agreeing:
- increase confidence;
- show provenance.

Two conflicting:
- show conflict;
- do not auto-resolve without evidence hierarchy.

---

## 33B.26 Source independence

Do not count:
- same faction rebroadcast;
- same original feed on two panels
as independent corroboration.

Track source root where possible.

---

## 33B.27 Cost visibility

Panel shows:
- W draw;
- battery drain;
- operator hours;
- current trace cost;
- estimated remaining trace time.

---

## 33B.28 Reliability tests

Factor-by-factor:
- all 1.0 => quality 1.0;
- one 0 => expected outage/zero;
- boundaries;
- clamping;
- deterministic combination.

---

## 33B.29 Misread determinism test

Same bad state + seed:
- same ambiguous/wrong result.

Improved state:
- corrected/clear result.

---

## 33B.30 Jam tests

EMP/power/relay failure:
- channel unavailable;
- trace stalls/lost according to rule;
- event emitted.

---

## 33B.31 Knowledge/counter-info tests

- partial decrypt awards existing knowledge;
- false report stored;
- second source contradicts;
- confidence/status updates deterministically.

---

## 33B.32 Save round-trip

Persist:
- device calibration/battery through inventory;
- intel/trace state;
- confidence/corroboration if stateful.

No reroll on load.

### 33B DoD

The radio can be unreliable without feeling arbitrary: the player can see the causes, improve them, and disprove bad information.

---

# TASK 33C — Comms Network as Maintainable Infrastructure

# 33C.0 Goal

Give the relay console a real authority or remove it.

If implemented, comms coverage becomes an asset with:
- graph position;
- coverage;
- power;
- condition;
- upkeep;
- repair;
- territorial vulnerability.

---

## 33C.1 Re-check 16A maturity verdict

Confirm route:
`tropospheric_radio_relay`

If still shelved/unbacked:
- do not mark live until 33C authority lands.

If architecture changed:
- adjust accordingly.

---

## 33C.2 Capacity decision gate

Before implementation answer:

```text
Can we deliver:
authority + persistence + mutation + coverage consumer + tests?
```

If no:
- remove route from player navigation;
- leave backlog entry;
- close false affordance.

This is acceptable completion.

---

## 33C.3 Create `CommsNetworkSystem`

Core responsibilities:
- relay definitions/state;
- graph;
- coverage query;
- condition/upkeep;
- availability;
- territory ownership/suppression input.

Engine-free.

---

## 33C.4 Relay definition data

Create:

`comms_network.json`

Schema:

```text
schema_version
relays[]
  id
  node_id
  base_coverage
  power_draw
  upkeep_interval
  upkeep_bill
  repair_recipe
  failure_thresholds
  terrain_modifiers
  weather_modifiers
```

Only fields actually needed.

---

## 33C.5 Relay graph nodes

Integrate with Plan 32 world graph.

Each relay:
- attaches to world node;
- can cover other nodes/edges;
- respects terrain/LOS approximation already available.

Do not introduce a second map graph.

---

## 33C.6 Coverage calculation

One Core query:

```text
GetCoverage(sourceNode, targetNode, environment)
```

Output:
- covered bool;
- quality multiplier;
- supporting relay IDs.

Deterministic.

---

## 33C.7 Terrain modifier

Use Plan 32 terrain/world data.

No hand-authored per-code route list.

---

## 33C.8 Weather modifier

Use current weather authority.

Severe weather:
- reduces range/quality;
- may disable high-risk links.

---

## 33C.9 Condition ledger

Use Plan 21 condition/wear patterns.

Relay state:
- condition;
- degradation;
- failure threshold.

No separate condition framework.

---

## 33C.10 Upkeep

At intervals:
- consume bill via existing construction/repair bill machinery;
- if missed, degrade or disable.

No free infrastructure.

---

## 33C.11 Repair duty

Repair requires:
- materials;
- worker;
- hours;
- fitness/skill.

Use Plan 24 labour authority.

---

## 33C.12 Power

Local/base relay:
- real watts from Plan 23.

Remote relay/waystation:
- use existing power source model if available;
- otherwise author simple data-backed supply consistent with waystation architecture.

No magic powered relay.

---

## 33C.13 Build path

Use existing:
- construction;
- recipe;
- expedition deployment;
depending on architecture.

Do not add relay-specific crafting framework.

---

## 33C.14 Coverage → intel fidelity

33B queries comms network.

In coverage:
- better signal factor.

Out of coverage:
- degraded or unavailable depending signal.

---

## 33C.15 Coverage → expedition contact

Expedition inside coverage:
- can receive warnings/retask if current expedition system supports it.

Outside:
- cannot be retasked;
- delayed/no warning.

Do not invent remote-control mechanic if expeditions never support retasking; instead gate existing contact affordance.

---

## 33C.16 Coverage → caravan notification

With Plan 30C:
- covered routes/nodes can report caravan arrival earlier/more reliably.

No coverage:
- notification delayed/absent per current event system.

---

## 33C.17 Territory interaction

Plan 30B can:
- suppress;
- seize;
- disable
relay through territory control.

Do not implement bespoke faction combat inside comms.

---

## 33C.18 Seizure state

On hostile takeover:
- relay owner/control changes;
- coverage changes;
- event emitted;
- repair/use blocked or altered.

---

## 33C.19 Events

Plan 31 kinds:

```text
relay_built
relay_degraded
relay_lost
coverage_reduced
relay_repaired
coverage_restored
```

Use actual canonical registry.

---

## 33C.20 Panel binding

`TroposphericRadioRelayPanel` binds to campaign-owned authority.

Required:

```text
ReferenceEquals(panel/network session authority, campaign authority)
```

No `new CommsNetworkSystem()` at bind.

---

## 33C.21 Mutating action

At least one:
- build;
- repair;
- assign upkeep;
- activate/deactivate;
depending actual UX.

A read-only fake map is insufficient.

---

## 33C.22 Liveness gate

Plan 15C runtime/static liveness must recognize:
- route live;
- bind real authority;
- action mutates authority.

---

## 33C.23 Save section

Register:
- relay state;
- condition;
- control;
- active coverage.

Use:
- `SaveSectionRegistry`;
- `SaveStoreHub`;
- save-store contract matrix.

---

## 33C.24 Coverage persistence

Derived coverage:
- recompute after load from persisted relay state + world/weather.

Do not persist duplicate coverage matrix if derivable.

---

## 33C.25 Coverage math tests

Cases:
- no relay;
- one relay;
- overlapping relays;
- weather penalty;
- terrain penalty;
- degraded relay;
- failed relay.

---

## 33C.26 Expedition contact test

```text
sortie within coverage
→ contact feature available

sortie crosses out
→ contact unavailable
```

Use actual expedition capability.

---

## 33C.27 Territory seizure test

```text
relay owned
→ hostile territory takeover
→ relay suppressed/seized
→ coverage drops
→ event emitted
```

---

## 33C.28 Save round-trip

Persist relay state.

After load:
- coverage identical for same weather/world state;
- no duplicate relay;
- condition preserved.

---

## 33C.29 Determinism

Same:
- relay graph;
- weather;
- territory;
- condition

=> same coverage.

---

## 33C.30 Registry/docs update

Update canon/registry row:
- authority file;
- panel route;
- tests;
- runtime evidence;
- confidence status.

If 33C chooses deletion:
- mark recommendation backlog;
- remove live-route claim.

---

## 33C.31 Panel snapshots

Three states:
- healthy network;
- degraded/partial;
- lost/hostile/offline.

Accessibility:
- no coverage encoded by color only.

### 33C DoD

Either the relay console becomes a real infrastructure surface backed by a persisted authority, or it disappears from player navigation. No fake affordance remains.

---

# 5. Cross-Task Dependency Graph

```text
31A semantic kinds
        │
        ▼
32A/32C world nodes / reveal
        │
        ▼
30A autonomous factions
        │
        ▼
33A distress signals
        │
        ▼
33B reliability / misinformation
        │
        ▼
33C comms infrastructure
```

Supporting systems:

```text
23 power ───────────────► 33A/33B/33C
21 condition ───────────► 33B/33C
24 labour/skill/fitness ► 33A/33B/33C
26A shipped data path ─► 33A loader fidelity
30B territory ─────────► 33C seizure/suppression
30C caravan world ─────► 33C notification
```

---

# 6. Information Lifecycle

Every intel fact should move through:

```text
SOURCE
→ CHANNEL
→ RECEPTION
→ INTERPRETATION
→ CONFIDENCE
→ REVEAL
→ ACTION
→ OUTCOME
→ CORROBORATION / CORRECTION
```

A fact that jumps directly from source to omniscient player knowledge bypasses the information economy.

---

# 7. Intel Claim Model

If existing systems do not already provide one, use a minimal record:

```text
claim_id
subject_id
claim_type
source_channel
source_id
observed_day
confidence
status
corroborated_by[]
contradicted_by[]
```

Status examples:
- unverified;
- probable;
- corroborated;
- contradicted;
- resolved.

Only add if necessary for 33B.

---

# 8. Cost Model

Radio usage costs should be legible:

| Cost | Authority |
|---|---|
| operator hours | DutyRoster |
| fitness/skill | Plan 24 |
| power watts | PowerGrid |
| battery | Inventory device |
| condition wear | EquipmentCondition |
| relay upkeep | bill/repair system |
| attention/opportunity | player scheduling |

No hidden abstract "radio points."

---

# 9. Reception Quality Acceptance

For any quality result, UI/debug should answer:

```text
Overall: 0.46
Device condition: 0.88
Battery: 0.93
Calibration: 0.70
Relay coverage: 0.80
Weather/EMI: 0.75
Operator: 0.92
Dominant penalty: calibration
```

This is the difference between interesting uncertainty and arbitrary failure.

---

# 10. Misreading Policy

Allowed:
- missing fragment;
- ambiguous coordinate;
- wrong-but-plausible interpretation;
- low confidence;
- delayed confirmation.

Not allowed:
- invisible random lie with no explanation;
- permanent false coordinate with no correction route;
- reroll on save/load.

---

# 11. Counter-Information Policy

Faction lies must satisfy:

```text
real world fact exists
AND hostile actor has motive
AND broadcast channel exists
AND independent verification route exists
```

The player should be able to learn:
"the radio lied"
through evidence, not developer revelation.

---

# 12. Outcome Handler Contract

Each `outcome_type` handler:

```text
Validate(definition)
Apply(campaign authorities)
Return structured outcome
Emit semantic event
```

Do not allow:
- raw string switch in UI;
- direct arbitrary mutation from catalog loader.

---

# 13. Persistence Matrix

| State | Authority | Persist? |
|---|---|---:|
| distress definitions | data catalog | no |
| detected signals | DistressSignalSystem | yes |
| trace progress | DistressSignalSystem | yes |
| current fragment | derive/save as needed | yes/derived |
| resolved outcomes | DistressSignalSystem/world | yes |
| battery/calibration | Inventory | already |
| knowledge/research | existing knowledge authority | yes |
| relay definitions | data catalog | no |
| relay built/condition/control | CommsNetworkSystem | yes |
| coverage | derived | no |
| intel confidence | existing/new minimal intel authority | yes if gameplay-relevant |

---

# 14. Save/Load Failure Cases

Test:

- save before detection;
- save mid-trace;
- save while jammed;
- save after wrong interpretation;
- save after corroboration;
- save with degraded relay;
- save after relay seizure.

No rerolls or duplicate outcomes on load.

---

# 15. Content Utilization Requirements

For distress catalog:

```text
DISCOVERED
LOADED
DESERIALIZED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

At least one real journey must reach `EFFECT_PRODUCED`.

All five authored signals must be reachable by content test.

---

# 16. Day Event Requirements

Minimum event coverage:

## 33A
- signal detected;
- trace progress;
- signal resolved;
- signal lost.

## 33B
- signal degraded/jammed;
- intel corroborated/contradicted where appropriate.

## 33C
- relay built;
- relay degraded;
- relay lost;
- coverage reduced/restored.

Every event:
- canonical kind;
- localization;
- route/info status.

---

# 17. UI Acceptance

## Radio panel
Shows:
- frequency;
- source/unknown;
- quality;
- quality factors;
- power cost;
- operator;
- trace state;
- confidence;
- actionable controls.

## Triangulation
Shows:
- trace progress;
- current fragment;
- estimated location/confidence;
- whether signal maintainable.

## Relay panel
Shows:
- graph/network;
- active relays;
- coverage;
- condition;
- upkeep;
- power;
- hostile/offline state.

No color-only confidence/coverage.

---

# 18. Audio Acceptance

Reuse current family.

Events:
- tune;
- static;
- lock;
- morse.

Rules:
- no duplicate cue;
- no new batch;
- audio state may reflect clarity through existing condition/mix system if supported.

---

# 19. Determinism Contract

Same:

```text
seed
+ signal definition
+ device state
+ weather
+ power
+ operator
+ relay coverage
+ player decisions
```

=> same:
- quality;
- fragment resolution;
- misread;
- trace result;
- outcome;
- emitted events.

---

# 20. Performance Guardrails

Radio calculations are low frequency.

Avoid:
- scanning all 118 broadcasts every frame;
- reparsing catalogs on tune;
- recomputing whole relay graph per UI frame.

Prefer:
- indexed frequency lookup;
- cached definitions;
- recompute coverage on topology/weather/state change;
- event-driven UI refresh.

---

# 21. Failure Injection Matrix

## N33.1 Distress catalog missing/malformed
Expected: diagnostics + integrity failure.

## N33.2 Unknown outcome type
Expected: data validation failure, no silent resolve.

## N33.3 Hidden node target missing
Expected: catalog integrity failure.

## N33.4 Power lost mid-trace
Expected: stall/loss per rule + event.

## N33.5 Operator removed from duty
Expected: trace cannot continue.

## N33.6 Battery depleted
Expected: offline/degraded.

## N33.7 Calibration poor
Expected: quality penalty visible.

## N33.8 Storm/EMP
Expected: quality/outage state visible.

## N33.9 Save/load mid-trace
Expected: same result.

## N33.10 False intel with no verification source
Expected: content/integrity test fails.

## N33.11 Relay panel bound to fresh authority
Expected: liveness/reference test fails.

## N33.12 Relay seized
Expected: coverage changes and event emitted.

## N33.13 Release distress catalog still exempt after wiring
Expected: content policy gate fails.

---

# 22. Test Pyramid

## Tier 1 — Core
- loader/schema;
- trace state;
- reception quality;
- coverage math;
- outcome registry.

## Tier 2 — Host
- power/labour/device inputs;
- world reveal;
- knowledge integration;
- save/persistence.

## Tier 3 — UI
- trace controls;
- reliability breakdown;
- relay state;
- accessibility.

## Tier 4 — Integration
- signal→reveal→expedition;
- false intel→corroboration;
- relay→contact.

## Tier 5 — Journey
- hear→trace→resolve→travel;
- jam/recover;
- hostile misinformation;
- relay degradation/recovery.

---

# 23. Verification Commands

Run per task and at close:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/generate-save-store-matrix.sh --check
bash scripts/ci/verify-fast.sh
```

Narrative/content graph tools:
- `ashfall-dialog-graph-lint`
- `ashfall-narrative-continuity`

Use actual repository commands.

---

# 24. Recommended Commit Breakdown

```text
33A-1 distress data audit + failing loader/reveal tests
33A-2 DistressSignalCatalogLoader
33A-3 DistressSignalSystem + save contract
33A-4 day events + UI trace wiring
33A-5 outcome handlers + world reveal
33A-6 power/labour/device cost
33A-7 authored-signal content tests + exemption removal + docs

33B-1 ReceptionQuality model
33B-2 device/weather/operator factors
33B-3 deterministic ambiguity/misread
33B-4 jamming/outage
33B-5 knowledge_points integration/retirement
33B-6 counter-intel/corroboration
33B-7 UI breakdown + save/tests

33C-1 capacity decision + authority skeleton or route removal
33C-2 comms_network data + graph
33C-3 condition/upkeep/power/repair
33C-4 coverage consumers
33C-5 territory seizure/suppression
33C-6 panel liveness + persistence
33C-7 events/snapshots/docs/registry
```

---

# 25. Risk Register

## R33.1 Radio becomes too expensive to use

Mitigation:
- telemetry;
- power/time tuning;
- low baseline monitoring cost, higher trace cost.

## R33.2 Reliability feels arbitrary

Mitigation:
- factor breakdown;
- deterministic formula;
- correction paths.

## R33.3 False intel frustrates player

Mitigation:
- confidence UI;
- independent verification;
- never undetectable.

## R33.4 Outcome handlers duplicate world logic

Mitigation:
- registry delegates to existing authorities.

## R33.5 Relay network scope expands uncontrollably

Mitigation:
- minimal graph;
- only three proven consumers;
- delete route if capacity insufficient.

## R33.6 Device state duplicates inventory

Mitigation:
- battery/calibration remain inventory-owned.

## R33.7 Knowledge points become a new currency

Mitigation:
- integrate existing progression or retire.

## R33.8 Signal tracing becomes automatic timer

Mitigation:
- power/labour/reliability maintenance.

---

# 26. Acceptance Checklist

## 33A — Distress signals

- [ ] current radio/broadcast inventory generated
- [ ] distress outcome matrix generated
- [ ] contracts match authored schema
- [ ] loader implemented
- [ ] malformed catalog warning test
- [ ] signal runtime state implemented
- [ ] DistressSignalSystem implemented
- [ ] authored fragments drive progression
- [ ] trace can stall/loss
- [ ] Plan-31 event kinds emitted
- [ ] event field semantics documented
- [ ] briefing routes live
- [ ] outcome-handler registry created
- [ ] survivor-community outcome integrated appropriately
- [ ] cache/loot outcome integrates with expedition/loot
- [ ] threat outcome integrates existing world risk
- [ ] location reveal uses Plan-32 authority
- [ ] expedition target unlock verified
- [ ] operator duty cost real
- [ ] fitness checked
- [ ] power draw real
- [ ] battery use real where supported
- [ ] existing cues reused
- [ ] UI trace state complete
- [ ] save section registered
- [ ] mid-trace round-trip passes
- [ ] content exemption removed after consumer lands
- [ ] all authored distress frequencies playable
- [ ] deterministic trace passes
- [ ] radio docs updated

## 33B — Reliability

- [ ] one ReceptionQuality function
- [ ] factors normalized
- [ ] formula documented
- [ ] breakdown DTO exists
- [ ] condition factor wired
- [ ] battery factor wired
- [ ] calibration factor wired
- [ ] relay factor seam wired
- [ ] weather/EMI factor wired
- [ ] operator skill wired
- [ ] operator fatigue/fitness wired
- [ ] UI attribution displays penalties
- [ ] authored clarity combines with quality
- [ ] low quality produces ambiguity/misread
- [ ] seeded misread deterministic
- [ ] correction loop works
- [ ] jam/outage states distinct
- [ ] knowledge_points audited
- [ ] knowledge integrated or field retired
- [ ] cipher-arc decision explicit
- [ ] cipher chain real if implemented
- [ ] hostile counter-information integrates Plan 30
- [ ] no undetectable lies
- [ ] confidence/provenance model minimal
- [ ] corroboration works
- [ ] independent-source rule enforced
- [ ] cost visible in panel
- [ ] factor tests pass
- [ ] misread determinism passes
- [ ] jam tests pass
- [ ] counter-intel tests pass
- [ ] save round-trip passes

## 33C — Comms network

- [ ] 16A maturity verdict rechecked
- [ ] build-vs-delete capacity gate decided
- [ ] CommsNetworkSystem exists if route stays live
- [ ] comms_network.json validated
- [ ] relays use Plan-32 graph nodes
- [ ] one coverage query exists
- [ ] terrain modifier wired
- [ ] weather modifier wired
- [ ] condition ledger reused
- [ ] upkeep real
- [ ] repair consumes labour/materials
- [ ] power requirement real
- [ ] build path reuses construction/recipe system
- [ ] coverage affects intel fidelity
- [ ] coverage affects expedition contact where supported
- [ ] coverage affects caravan notification where supported
- [ ] territory interaction uses Plan 30
- [ ] seizure state real
- [ ] relay events emitted
- [ ] panel binds campaign authority
- [ ] at least one mutating action exists
- [ ] liveness gate sees it
- [ ] save section registered
- [ ] coverage derived after load
- [ ] coverage math tests pass
- [ ] expedition contact test passes
- [ ] seizure test passes
- [ ] save round-trip passes
- [ ] determinism passes
- [ ] canon/registry row updated
- [ ] three relay snapshots accepted

---

# 27. Ship / No-Ship Gate

**SHIP** only if:

```text
distress_catalog_loaded_by_shipped_path == true
AND authored_distress_signals_playable == 100_percent
AND signal_outcomes_registered == 100_percent
AND signal_reveal_unlocks_real_world_node == true
AND expedition_target_accepts_revealed_node == true
AND tracing_consumes_power == true
AND tracing_consumes_labour == true
AND reception_quality_authorities == 1
AND reliability_factor_breakdown_visible == true
AND misread_deterministic == true
AND undetectable_false_intel == 0
AND knowledge_points_or_equivalent_has_real_consumer == true
AND radio_device_state_roundtrip == pass
AND distress_content_exemption_removed == true
AND distress_content_effect_produced == true
AND (
    tropospheric_relay_route_removed == true
    OR (
        comms_network_authorities == 1
        AND relay_panel_campaign_identity == true
        AND relay_mutating_action_live == true
        AND relay_state_roundtrip == pass
        AND coverage_consumers >= 3
    )
)
AND audio_selftest == pass
AND data_integrity_selftest == pass
AND content_utilization_selftest == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 28. Implementer Handoff

1. Read `radio_distress_signals.json` first; do not invent around it.
2. Land 33A before reliability or relay infrastructure.
3. Use the shipped data path from Plan 26A.
4. Persist trace state from birth.
5. Emit Plan-31 semantic kinds for every meaningful trace transition.
6. Make every signal outcome mutate an existing world/campaign authority.
7. Require operator time and power.
8. Keep existing cue family.
9. Implement one reception-quality function.
10. Explain every quality penalty in UI.
11. Use deterministic misreading and real correction paths.
12. Integrate `knowledge_points` into an existing knowledge system or retire it.
13. Never ship a lie the player cannot investigate.
14. Build `CommsNetworkSystem` only if the relay console can become genuinely live; otherwise remove the route.
15. Reuse world graph, condition, power, labour, territory, save, and event authorities.
16. Remove content exemptions only after runtime effect evidence exists.
17. Close with end-to-end signal→reveal→expedition and content-utilization proof.

---

# 29. Final Outcome

When this plan is complete, radio stops being atmospheric wallpaper and becomes a scarce intelligence system.

The player can hear a distress signal, assign someone to work the receiver, pay the power and battery cost, watch clarity improve or collapse under weather and equipment condition, and decide whether the information is reliable enough to act on. A resolved signal reveals something real: a node, a cache, a threat, or a community that the expedition/world systems can actually use.

Signal quality is not a hidden dice roll. The game can explain that the receiver is mis-calibrated, the storm is bad, the operator is exhausted, or relay coverage is weak. Low-quality intelligence can be ambiguous or wrong, but it is deterministic and correctable. Hostile factions can mislead, but never in a way the player cannot disprove through another source.

If the relay network lands, comms become infrastructure: built, powered, maintained, repaired, seized, and lost. Coverage changes what can be heard and what expeditions/caravans can communicate. If that authority cannot be delivered, the fake console disappears.

The result is not a new radio minigame. It is the existing radio, world, expedition, labour, power, condition, faction, and event systems finally turning information into a decision worth paying for.
