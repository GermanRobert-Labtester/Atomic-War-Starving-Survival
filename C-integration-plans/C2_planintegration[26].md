# C2 — Flagship Integration Plan [26]: Hidden Agendas, Evidence-Driven Betrayal Arcs, and Persistent Survivor Secrets

> **Deliverable:** `C2_planintegration[26].md`
> **Source scope:** Plan 132 — *Survivor Hidden Agendas & Betrayal Arc*
> **Primary objective:** create a persistent, deterministic hidden-agenda layer for shelter survivors where secret motivations, loyalties, theft, sabotage, escape intent, and protective concealment evolve over time; generate fair but ambiguous behavioral clues; support investigation and confrontation; and route resolutions into the existing relations, factions, quests, morality, epilogue, save, and event authorities without creating a parallel social simulation.
> **Required execution order:** **132A Foundation/System Contract → 132B Agenda Content & Branching → 132C Consequences, Save/CI, and Cross-System Closure**
> **Hard dependencies:** Plan 40A authored identity; Plan 42 voice delivery if survivor speech is used; Plan 44 relation effects/history; Plan 31 semantic event vocabulary/cause IDs; Plan 36 port-contract discipline; Plan 39 save durability; Plan 45 content acceptance ladder.
> **Scope discipline:** no omniscient player-facing agenda labels before discovery, no agenda consequence invented only in prose, no reset-by-save/load exploit, no random agenda generation outside `ISeededRng`, no agenda that bypasses canonical inventory/faction/power/quest systems, no unlimited simultaneous agendas, and no second relationship or faction-standing authority.

---

# 0. Executive Intent

ASHFALL’s survivor simulation already contains the ingredients for distrust:

- authored beliefs,
- professions and identities,
- ideological friction,
- relation affinity/trust/resentment,
- mental-health crises,
- faction standings,
- moral-choice consequences,
- confession secrets,
- survivor voice,
- pair history,
- save/load continuity.

What it does not yet have is a persistent **hidden-motive layer**.

The current model is largely transparent:

```text
survivor state
→ player can inspect
→ player knows roughly what is wrong
```

The target model is:

```text
authored identity + context + pressure
              │
              ▼
      HiddenAgendaSystem
              │
      ┌───────┼────────┐
      ▼       ▼        ▼
   motive    clues   escalation
      │       │        │
      └───────┼────────┘
              ▼
       player investigation
              │
              ▼
          evidence state
              │
              ▼
         confrontation
              │
              ▼
     branch resolution outcome
              │
      ┌───────┼───────────────┬─────────────┐
      ▼       ▼               ▼             ▼
  relations  factions       quests       morality
      │       │               │             │
      └───────┼───────────────┴─────────────┘
              ▼
       permanent campaign flags
              │
              ▼
          epilogue/legacy
```

The strongest player-facing outcome is:

> **The player can suspect a survivor without knowing the truth, investigate with real opportunity cost, confront with incomplete evidence, make a consequential decision, and later discover whether they were right—without the game cheating or the player being able to reset the arc through save/load.**

---

# 1. Source Diagnosis

The source establishes:

- confession secrets exist, but only as one-shot forgiveness/grudge events,
- survivor relations exist, but do not model evolving secret motives,
- ideological friction exists, but does not become multi-step narrative arcs,
- mental-health crises exist, but not secret loyalties/contacts,
- no system tracks covert resource loss, sabotage intent, faction contact, or escape preparation,
- the plan proposes five agenda families:
  - faction loyalty,
  - theft,
  - sabotage,
  - escape,
  - protection,
- the player interaction loop is:
  - clues,
  - investigation,
  - confrontation,
  - branch resolution,
  - delayed consequences,
- deterministic agenda selection is explicitly required,
- old-save compatibility requires empty agenda state that can populate later,
- a suspicion/investigation UI is required,
- cross-system consequences include morality, factions, quests, relations, and endings.

The key architectural implication is:

```text
hidden agenda ≠ hidden parallel game
```

Instead:

```text
hidden agenda = state-backed interpretation and delayed consequence
```

that reuses existing authoritative systems for actual effects.

---

# 2. Program-Level Success Criteria

C2[26] closes only when all of the following are true.

## 2.1 Hidden agendas persist

Active/resolved agendas survive save/load.

## 2.2 Agenda generation is deterministic

Same seed + same state → same selected agendas and clue schedule.

## 2.3 Agenda eligibility is state-backed

Triggers derive from authored identity, relations, faction standing, need/stress, and canonical events.

## 2.4 No agenda invents consequences outside existing authorities

Theft uses inventory.
Sabotage uses actual shelter systems.
Faction loyalty uses faction standing/intel.
Escape uses survivor/roster/location state.

## 2.5 Clues are fair

Observable but ambiguous.

## 2.6 Investigation costs something

Labor/time/opportunity/risk.

## 2.7 False accusation is mechanically possible

And consequences are state-backed.

## 2.8 Evidence thresholds are authored per agenda type

No one magic threshold.

## 2.9 Confrontation cannot occur before minimum evidence

Except for explicit emergency cases.

## 2.10 Save/load cannot reroll clues or outcomes

Idempotency and seeded state prevent exploitation.

## 2.11 Simultaneous agenda pressure is capped

2–3 active agendas maximum unless design proves otherwise.

## 2.12 Agenda escalation is bounded

Ignored agendas progress but do not instantly jump from clue to catastrophe without authored stages.

## 2.13 Every resolution writes persistent flags/history

Later systems can read them.

## 2.14 Relations change through canonical relation APIs

No direct duplicate trust variable.

## 2.15 Factions change through canonical standing APIs

No agenda-specific standing ledger.

## 2.16 Quest unlocks use canonical quest runtime

No hidden-agenda-only quest manager.

## 2.17 Moral consequences use MoralChoiceSystem

No parallel morality counter.

## 2.18 Ending impact uses canonical epilogue/completion record path

No custom ending evaluator.

## 2.19 Old saves load safely

No agenda state required in legacy saves.

## 2.20 Headless/CI coverage proves the complete lifecycle

Generate → clue → investigate → confront → resolve → consequence.

---

# 3. Architectural Invariants

## 3.1 Authored identity is the trigger substrate

Use Plan 40 identity.
Do not infer new personality truths heuristically.

## 3.2 One agenda authority

`HiddenAgendaSystem` owns:

- agenda lifecycle,
- clue progress,
- confrontation eligibility,
- agenda-stage transition,
- resolution status.

It does not own:

- inventory,
- factions,
- relations,
- shelter durability,
- survivor roster,
- quest state,
- morality.

## 3.3 Consequences are routed, not duplicated

Every agenda effect uses existing system seams.

## 3.4 Hidden information has separate truth and knowledge

System truth:

```text
agenda exists
```

Player knowledge:

```text
suspected / partial / confirmed
```

These are not the same state.

## 3.5 Investigation modifies knowledge, not truth

Assigning an investigator changes evidence/discovery progress.

It does not alter the underlying agenda unless the agenda rules explicitly react to being watched.

## 3.6 Agenda state is deterministic

All random selection/timing comes from `ISeededRng`.

## 3.7 Agenda consequences are idempotent

Resolution effects apply once.

## 3.8 Save/load cannot rewind discovery

Persistent progression/cooldowns block reroll abuse.

## 3.9 Agenda count is capped

Prevent hidden-state overload.

## 3.10 UI never reveals raw hidden agenda type before sufficient evidence

Use suspicion/clue abstractions.

---

# 4. Dependency Graph

```text
40A authored survivor identity ───┐
44 pair history / relation effects├──► 132A HiddenAgendaSystem
31 semantic events / cause IDs ───┤
36 port contracts ────────────────┘
                                   │
                                   ▼
                          132B content/templates
                                   │
             ┌─────────────────────┼─────────────────────┐
             ▼                     ▼                     ▼
          factions              inventory             shelter systems
             │                     │                     │
             └─────────────────────┼─────────────────────┘
                                   ▼
                           132C consequences
                                   │
             ┌─────────────────────┼─────────────────────┐
             ▼                     ▼                     ▼
          relations             quests               morality
                                   │
                                   ▼
                              epilogue/record
```

---

# 5. Baseline Capture

Before implementation, record:

- current survivor count,
- authored identity coverage,
- relation system APIs,
- faction-standing APIs,
- inventory consume/grant APIs,
- shelter degradation/repair APIs,
- mental-health crisis triggers,
- confession secrets schema,
- current semantic event kinds,
- current quest unlock/accept APIs,
- current save section registry,
- current panel registry/routes.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture one representative populated-cast save.

---

# 6. Workstream 132A — Foundation / System Contract

## Goal

Create one deterministic hidden-agenda lifecycle and knowledge model without yet exploding content scope.

---

# 7. 132A Phase A — Define the Core Agenda Model

Create:

```text
Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs
```

and supporting DTOs.

Recommended split:

```text
HiddenAgendaDefinition
HiddenAgendaInstance
HiddenAgendaState
HiddenAgendaClue
HiddenAgendaInvestigation
HiddenAgendaResolution
```

Avoid a single giant DTO.

---

# 8. 132A Phase B — Definition vs Instance

## Definition

Authored template:

```text
agenda_type
trigger requirements
clue families
evidence thresholds
escalation stages
allowed resolutions
consequence descriptors
```

## Instance

Runtime survivor-specific state:

```text
agenda_id
survivor_id
target ids
start day
stage
discovery progress
player knowledge state
confronted?
resolved?
resolution
cooldowns
applied consequence flags
```

This separation is critical.

---

# 9. 132A Phase C — Agenda Type Vocabulary

Initial five canonical types:

```text
faction_loyalty
resource_theft
sabotage
escape_plan
protection
```

Do not use free-text type strings.

---

# 10. 132A Phase D — Agenda Knowledge States

Recommended:

```text
Unknown
Suspected
Investigating
EvidenceWeak
EvidenceStrong
Confirmed
Resolved
```

This is player knowledge.

Underlying agenda stage remains separate.

---

# 11. 132A Phase E — Agenda Lifecycle States

Recommended:

```text
Dormant
Active
Escalating
Imminent
Triggered
Resolved
Failed
Cancelled
```

Not every agenda needs every stage.

---

# 12. 132A Phase F — Trigger Sources

Implement `IHiddenAgendaSource` or equivalent.

Candidate producers:

- ideological friction,
- faction standing,
- severe unmet need,
- mental-health crisis,
- relation rupture,
- prior betrayal/secret history.

Do not let UI trigger agendas.

---

# 13. 132A Phase G — Eligibility Query

A survivor can only receive agenda when all template prerequisites resolve.

Checks may include:

- identity profile,
- profession,
- faction relation,
- affinity/trust,
- current needs,
- campaign day,
- existing agenda count,
- immunity flags,
- target availability.

---

# 14. 132A Phase H — Immunity / Suppression

Source includes loyalty-trait immunity and high-trust reduction.

Implement as authored modifiers.

Examples:

```text
agenda immunity tag
agenda weight multiplier
minimum trust requirement
disallowed agenda types
```

Avoid hardcoded survivor IDs.

---

# 15. 132A Phase I — Global Active Agenda Cap

Initial design cap:

```text
2–3 active agendas shelter-wide
```

Choose a canonical config value.

Also cap:

- one active agenda per survivor unless explicit nested/protection case,
- one high-severity agenda at a time if balance requires.

---

# 16. 132A Phase J — Deterministic Agenda Selection

Use `ISeededRng`.

Stable selection inputs:

```text
campaign seed
day
survivor id
agenda generation cycle
```

Candidate list sorted by stable template ID before weighted choice.

---

# 17. 132A Phase K — Agenda Generation Cadence

Do not roll every frame/day without policy.

Define:

```text
eligible evaluation cadence
cooldown after resolution
minimum day before first agenda
```

This reduces spam and save/load reroll surface.

---

# 18. 132A Phase L — HiddenAgendaState

Persist:

```text
active agendas
resolved agenda summaries
discovery/evidence state
agenda-generation cooldowns
informant state if needed
idempotency keys
```

Avoid unbounded full-resolution histories; integrate Plan 55 retention policy where later appropriate.

---

# 19. 132A Phase M — Capture / Restore

Implement:

```text
CaptureState()
RestoreState()
```

Requirements:

- deterministic ordering,
- schema version,
- missing legacy section → empty state,
- restored agendas do not reroll.

---

# 20. 132A Phase N — Save Schema Version

Register hidden-agenda save section.

Old save:

```text
missing section
→ initialize empty
→ future agenda generation permitted
```

No immediate retroactive agenda explosion on load.

---

# 21. 132A Phase O — Legacy Grace Window

For legacy saves, consider:

```text
minimum N days after migration before first agenda generation
```

to avoid “load old campaign, betrayal fires instantly.”

If used, author and test.

---

# 22. 132A Phase P — Clue Model

Each clue record should contain:

```text
clue_id
agenda_instance_id
clue_family
day
source_event_id
evidence_value
reliability
player_visible?
resolved/invalidated?
```

Do not store prose as authority.

---

# 23. 132A Phase Q — Clue Families

Source-driven clue concepts:

```text
unusual_absence
inventory_discrepancy
radio_activity
maintenance_pattern
witness_statement
found_stash
route_questions
packing_behavior
informant_tip
```

Use validated IDs.

---

# 24. 132A Phase R — Ambiguity Rule

A clue should not always uniquely identify agenda type.

Example:

```text
unusual absence
```

could support:

- faction contact,
- theft,
- escape.

This is essential for fair uncertainty.

---

# 25. 132A Phase S — Discovery Progress

Discovery progress should be derived from:

```text
clue evidence
investigation results
reliability
countermeasures
```

Prefer explicit evidence accumulation over arbitrary +20/+30 where possible.

If numerical progress is used, thresholds are authored.

---

# 26. 132A Phase T — False Clue / Misinterpretation Policy

Source allows false accusation.

Do not make false clues random lies unless state-backed.

Potential causes:

- unreliable informant,
- coincidental resource discrepancy,
- normal maintenance failure,
- agenda intentionally planting evidence if a later type supports it.

---

# 27. 132A Phase U — Investigation Assignment

Investigation uses existing duty/roster system.

Inputs:

```text
investigator survivor
target suspicion
duration
skill/profession modifier
relation modifier
labor opportunity cost
```

Do not create a parallel labor scheduler.

---

# 28. 132A Phase V — Investigator Suitability

Use:

- profession,
- relation to target,
- fitness,
- trust,
- fatigue.

All from canonical authorities.

---

# 29. 132A Phase W — Investigation Outcome

Possible:

```text
no_new_evidence
weak_clue
strong_clue
false_lead_exposed
target_became_cautious
direct_confirmation
```

Deterministic seeded resolution.

---

# 30. 132A Phase X — Target Awareness

Failed/intrusive investigation may set:

```text
agenda_caution
```

which modifies clue timing/escalation.

Persist this.

---

# 31. 132A Phase Y — Confrontation Eligibility

Each agenda type defines:

```text
minimum evidence threshold
strong evidence threshold
emergency override?
```

UI must show whether evidence is enough.

---

# 32. 132A Phase Z — Confrontation State Machine

Confrontation phases:

```text
open accusation
present evidence
response
player resolution
commit consequences
```

Do not apply consequences before final resolution.

---

# 33. 132A Phase AA — Evidence Strength

Use bands:

```text
weak
credible
strong
overwhelming
```

instead of exposing exact 0–100 if product tone benefits.

Raw values remain diagnostics.

---

# 34. 132A Phase AB — Resolution Types

Canonical resolution vocabulary:

```text
expelled
reconciled
monitored
exploited
forgiven
repaid
allowed_departure
forced_repair
cover_up
double_agent
ignored
false_accusation
```

Not all valid for every agenda.

---

# 35. 132A Phase AC — Consequence Descriptor Layer

Agenda system should output descriptors like:

```text
remove_items
apply_relation_delta
apply_faction_delta
set_world_flag
start_quest
remove_survivor
apply_labor_debt
```

But the actual effect is applied through owning system.

---

# 36. 132A Phase AD — Port Contract

Every required sink becomes a declared port.

Candidate sinks:

```text
inventory
relations
factions
quest runtime
survivor roster
shelter degradation/repair
moral choice
world flags
```

Missing required sink → boot/validation failure.

---

# 37. 132A Phase AE — Idempotency

Each agenda resolution has:

```text
resolution_event_id
consequence_applied set
```

Load/replay cannot double:

- remove items,
- change standing,
- expel survivor,
- write flags.

---

# 38. 132A Phase AF — Headless Tick

Agenda progression must not depend on panel open.

Daily/monthly owner is explicit.

No UI-time ticking.

---

# 39. 132A Phase AG — Event Vocabulary

Emit semantic events for:

- agenda activated (internal/non-player),
- clue observed,
- investigation completed,
- confrontation started,
- agenda resolved,
- betrayal completed,
- false accusation.

Use Plan 31 taxonomy.

---

# 40. 132A Phase AH — Diagnostic View

Provide headless/debug readout:

```text
active agenda count
survivor
type
stage
knowledge state
evidence
next escalation
```

Developer-only; not player UI.

---

# 41. 132A Tests

- deterministic agenda selection,
- eligibility,
- immunity,
- cap,
- save round-trip,
- old-save empty migration,
- clue ambiguity,
- investigation outcome,
- target caution,
- confrontation threshold,
- idempotent resolution,
- missing port failure,
- headless ticking.

---

# 42. 132A Definition of Done

- [ ] HiddenAgendaSystem,
- [ ] definition/instance split,
- [ ] five type vocabulary,
- [ ] player knowledge states,
- [ ] lifecycle states,
- [ ] trigger-source interface,
- [ ] deterministic eligibility/selection,
- [ ] immunity/trust suppression,
- [ ] active cap,
- [ ] save schema,
- [ ] legacy-safe empty state,
- [ ] clue model,
- [ ] investigation integration,
- [ ] confrontation state machine,
- [ ] consequence descriptors,
- [ ] port declarations,
- [ ] idempotency,
- [ ] headless tick,
- [ ] semantic events,
- [ ] diagnostics.

---

# 43. Workstream 132B — Implementation / Content / Branching

## Goal

Implement a narrow but deep first content set for all five agenda families with fair clues, escalation, and branch consequences.

---

# 44. 132B Phase A — `hidden_agendas.json`

Create:

```text
Assets/StreamingAssets/Data/hidden_agendas.json
```

Schema fields:

```text
id
agenda_type
speaker/identity eligibility
target requirements
trigger conditions
clue schedule/families
investigation modifiers
confrontation thresholds
escalation stages
resolution options
consequence descriptors
cooldowns
localization keys
```

---

# 45. 132B Phase B — Template Integrity

Validate:

- unique ID,
- known agenda type,
- known faction/survivor/location refs,
- clue family refs,
- resolution refs,
- localization keys,
- threshold ordering,
- cooldown non-negative,
- stage sequence valid.

---

# 46. 132B Phase C — Initial Content Budget

Source asks for 15 templates.

Implement:

```text
3 per agenda family
```

before expanding.

This keeps coverage balanced.

---

# 47. 132B Phase D — Faction Loyalty Agenda

Canonical underlying actions may include:

```text
secret contact
information leak
covert standing shift
raid/diplomatic pressure enablement
```

Actual faction effects use faction systems.

---

# 48. 132B Phase E — Faction Loyalty Clues

Potential clues:

- unusual radio activity,
- unexplained absence,
- third-party message,
- suspicious standing/intel event,
- faction-coded item/communication if such content exists.

Do not invent visible faction standing changes without canonical cause.

---

# 49. 132B Phase F — Faction Loyalty Evidence Thresholds

Source baseline:

```text
confront around 60
strong evidence around 80
```

Treat as authored initial defaults, not hardcoded constants.

---

# 50. 132B Phase G — Faction Loyalty Resolutions

Initial branches:

```text
expose_and_expel
turn_double_agent
forgive_and_monitor
exploit_for_leverage
```

Each requires explicit consequence matrix.

---

# 51. 132B Phase H — Double-Agent Guardrails

If player turns survivor:

- create explicit flag/state,
- route false intel through existing faction/intel system,
- do not magically grant standing without action,
- cap repeat exploitation.

---

# 52. 132B Phase I — Blackmail / Exploit Guardrail

Source includes coerced labor/loyalty.

Represent through existing labor/relations/morale systems.

No hidden “forced loyalty stat.”

Also treat as severe moral/resentment consequence.

---

# 53. 132B Phase J — Resource Theft Agenda

Underlying mechanics:

```text
scheduled theft attempts
actual inventory removal
hidden stash state
```

Use canonical inventory transaction APIs.

---

# 54. 132B Phase K — Theft Clues

Potential:

- inventory discrepancy,
- found item,
- stash discovery,
- witness,
- unexplained possession.

---

# 55. 132B Phase L — Theft Thresholds

Source baseline:

```text
confront around 50
stash reveal around 70
```

Data-authored.

---

# 56. 132B Phase M — Theft Resolutions

Initial:

```text
recover_and_expel
force_repayment
understand_motive
use_stash_as_leverage
```

Each routes through inventory/relations/labor/morality.

---

# 57. 132B Phase N — Legitimate-Need Motive

If “understand motive” branch exists, the motive must be backed by state.

Examples:

- unmet medicine need,
- dependent child,
- starvation,
- debt.

Do not retroactively invent sympathy reason.

---

# 58. 132B Phase O — Sabotage Agenda

Underlying mechanics:

```text
small degradation events
maintenance failures
delayed system fault
```

Apply through actual durability/power/shelter systems.

---

# 59. 132B Phase P — Sabotage Clues

Potential:

- correlated degradation,
- maintenance anomalies,
- proximity,
- witness,
- tool/part discrepancy.

Never use omniscient “sabotage detected” before evidence threshold.

---

# 60. 132B Phase Q — Sabotage Threshold

Source baseline:

```text
~70
```

Data-authored.

---

# 61. 132B Phase R — Sabotage Resolutions

Initial:

```text
immediate_expulsion
forced_repair
discover_motivation
cover_up
```

Cover-up should preserve risk state and may create later consequences.

---

# 62. 132B Phase S — Cover-Up Consequence

If sabotage is hidden:

- player/public knowledge differs,
- actual system damage remains,
- future discovery can blame player,
- morale effect delayed.

This is a good proof of truth-vs-knowledge separation.

---

# 63. 132B Phase T — Escape Plan Agenda

Underlying mechanics:

```text
resource hoarding
route research
packing
departure attempt
```

Use existing inventory/location/roster APIs.

---

# 64. 132B Phase U — Escape Clues

Potential:

- packed bag,
- route questions,
- missing supplies,
- map interest,
- absence near exit.

---

# 65. 132B Phase V — Escape Threshold

Source baseline:

```text
~40
```

because escape intent is easier to infer and not automatically betrayal.

---

# 66. 132B Phase W — Escape Resolutions

Initial:

```text
allow_departure
convince_to_stay
confiscate_and_expel
join_escape
```

---

# 67. 132B Phase X — Join-Escape Scope Guard

The source calls this campaign-altering.

Do not implement as a trivial branch unless campaign architecture supports player departure.

If current runtime cannot support it safely:

```text
mark deferred/blocking prerequisite
```

rather than fake it.

This is one place where source ambition may need explicit dependency gating.

---

# 68. 132B Phase Y — Protection Agenda

This agenda conceals another survivor’s secret.

Representation:

```text
protector agenda
→ target agenda instance id
```

not duplicate copied secret state.

---

# 69. 132B Phase Z — Nested Secret Guard

Cap nesting depth.

Initial:

```text
1 layer
```

No recursive secret graph explosion.

---

# 70. 132B Phase AA — Protection Discovery

Discovering protector may:

- reveal target suspicion,
- add evidence,
- not necessarily fully expose target agenda.

Keep fairness.

---

# 71. 132B Phase AB — Cross-Agenda Interaction

One agenda may trigger conditions for another.

Examples:

- theft discovered → protection agenda by close ally,
- faction contact exposed → escape agenda,
- false accusation → resentment-driven later agenda eligibility.

Use semantic events.

---

# 72. 132B Phase AC — Agenda Escalation

Each template defines staged progression.

Example theft:

```text
small discrepancy
→ stash grows
→ larger theft
→ major loss
```

No instantaneous catastrophe.

---

# 73. 132B Phase AD — Escalation Delay

Use authored intervals.

Save/load persists next escalation day.

No reroll.

---

# 74. 132B Phase AE — Ignored-Clue Consequences

Ignoring clues may:

- increase agenda confidence,
- reduce future clue rate,
- increase severity,
- trigger completion.

But escalation is deterministic and visible in retrospective evidence.

---

# 75. 132B Phase AF — Informant Mechanic

A survivor may provide a clue about another.

Eligibility uses:

- relationship,
- personality/belief,
- observed knowledge,
- agenda protection conflicts.

---

# 76. 132B Phase AG — Informant Truthfulness

Do not use arbitrary liar RNG.

Truthfulness can be influenced by:

- relation to target,
- relation to player,
- own agenda,
- evidence quality.

Any random component is seeded.

---

# 77. 132B Phase AH — False Accusation

If informant is wrong/unreliable:

- accused survivor relation changes,
- allies may react,
- player trust/morale changes,
- agenda remains if target actually innocent.

This is a separate outcome state.

---

# 78. 132B Phase AI — Prevention by High Trust

High trust/affinity modifies generation probability.

Do not make it absolute unless template says so.

This preserves tension without making trust meaningless.

---

# 79. 132B Phase AJ — Agenda Cooldowns

Per survivor and per type.

Prevent:

```text
resolve theft
→ immediate second theft
```

---

# 80. 132B Phase AK — Content Localization

All:

- clue descriptions,
- suspicion labels,
- investigation results,
- confrontation prompts,
- branch labels

use localization keys.

No raw prose in C#.

---

# 81. 132B Phase AL — Narrative Tone

Tone should be:

- restrained,
- ambiguous,
- non-omniscient,
- concrete.

Avoid thriller melodrama that breaks ASHFALL tone.

---

# 82. 132B Phase AM — Voice Integration

If Plan 42 is live:

- survivors may comment,
- but voice cannot reveal hidden truth without knowledge,
- confrontation lines remain state-backed.

No spoken line as sole evidence.

---

# 83. 132B Phase AN — 15-Template Coverage Matrix

Table:

| Type | Template | Eligibility | Clues | Threshold | Escalation | Resolutions |
|---|---|---|---|---:|---|---|

Require at least 3 templates/type.

---

# 84. 132B Phase AO — Content Utilization

Run 100/200-day seeded scenarios.

Report:

```text
templates eligible
templates activated
clues generated
confrontations reached
resolutions reached
dead templates
```

---

# 85. 132B Phase AP — Dead Template Policy

If a template is never eligible:

- fix trigger,
- mark intentionally rare,
- remove,
- or exempt with reason/expiry.

---

# 86. 132B Definition of Done

- [ ] hidden_agendas.json,
- [ ] schema/integrity,
- [ ] 15 templates,
- [ ] 3 per type,
- [ ] faction loyalty branch set,
- [ ] theft branch set,
- [ ] sabotage branch set,
- [ ] escape branch set,
- [ ] protection/nested secret logic,
- [ ] delayed escalation,
- [ ] cross-agenda interaction,
- [ ] informants,
- [ ] false accusations,
- [ ] trust suppression,
- [ ] cooldowns,
- [ ] localized content,
- [ ] voice-safe integration,
- [ ] utilization report,
- [ ] no unreachable template debt without disposition.

---

# 87. Workstream 132C — Consequences / Integration / Validation

## Goal

Close every agenda branch through canonical systems and prove persistence, fairness, determinism, and player observability.

---

# 88. 132C Phase A — Relations Integration

Every resolution writes through `SurvivorRelationsSystem`.

Potential effects:

- trust loss,
- resentment,
- forgiveness,
- blackmail resentment,
- gratitude,
- false-accusation damage.

Use existing pair history if available.

---

# 89. 132C Phase B — Pair History Attribution

Write reason entries such as:

```text
hidden_agenda_exposed
false_accusation
betrayal_forgiven
blackmail
theft_repaid
```

No unexplained affinity jumps.

---

# 90. 132C Phase C — Faction Integration

Faction loyalty agendas affect standing/intel through faction APIs.

Examples:

- leak discovered,
- agent expelled,
- double agent,
- faction pressure.

No direct number mutation in agenda system.

---

# 91. 132C Phase D — Quest Integration

Agenda discovery can unlock confrontation/aftermath quests.

Use canonical quest runtime.

Quest should reference:

```text
agenda_instance_id
cause_id
survivor_id
```

where appropriate.

---

# 92. 132C Phase E — Moral Choice Integration

Resolutions like:

- forgive,
- exploit,
- expel,
- cover up,
- blackmail

can map to morality consequences.

Use MoralChoiceSystem.

Do not create hidden-agenda morality flags separately.

---

# 93. 132C Phase F — Epilogue / Completion Record

Record permanent outcomes:

```text
major_betrayal
double_agent_used
false_accusation_destroyed_trust
survivor_expelled_for_sabotage
escape_joined
```

through canonical completion/epilogue record.

Do not branch ending logic inside HiddenAgendaSystem.

---

# 94. 132C Phase G — World Flags

Only use world flags for coarse persistent facts consumed elsewhere.

Do not dump every agenda detail into global flags.

---

# 95. 132C Phase H — Inventory Integrity

Theft/recovery/repayment must preserve mass/item correctness.

No phantom item creation/loss.

Use transactional APIs.

---

# 96. 132C Phase I — Shelter Damage Integrity

Sabotage damage uses actual degradation/damage path.

Repair branch uses actual repair/work path.

No “sabotage_damage += 1” parallel state.

---

# 97. 132C Phase J — Survivor Departure

Expulsion/escape uses canonical survivor removal/departure flow.

Ensure:

- roster updates,
- duty assignments cleaned,
- expedition references cleaned,
- relations/memory preserved appropriately,
- save round-trip safe.

---

# 98. 132C Phase K — All-Survivors-Expelled Edge Case

If no valid survivors remain:

- agenda generation stops,
- no invalid target selection,
- game-over/ending path remains canonical.

---

# 99. 132C Phase L — High-Trust Shelter Edge Case

Source requires no-agenda trigger scenario.

Test a shelter where all eligible weights suppress to zero.

Expected:

```text
no agenda generated
```

not fallback-forced drama.

---

# 100. 132C Phase M — No-Valid-Target Edge Case

For protection/contact agendas:

```text
no valid target
→ template ineligible
```

never self-target unless explicitly authored.

---

# 101. 132C Phase N — Save/Load Exploit Prevention

Test at key lifecycle points:

```text
before clue
after clue
before investigation result
before confrontation
before resolution
after resolution
```

Reload must preserve:

- selected agenda,
- next clue timing,
- evidence,
- target caution,
- resolution outcome,
- consequence idempotency.

---

# 102. 132C Phase O — Determinism Replay

Same seed + same actions:

```text
same agenda assignments
same clue schedule
same investigation outcomes
same confrontation branches
same consequences
```

---

# 103. 132C Phase P — Headless Behavior

No panel required for:

- progression,
- escalation,
- clue generation,
- consequence application.

UI is projection.

---

# 104. 132C Phase Q — Suspicion Board UI

Source asks for a board.

Implement as existing panel architecture.

Player sees:

```text
survivor
suspicion status
known clues
investigation assignment
evidence band
available action
```

Not hidden agenda type unless discovered.

---

# 105. 132C Phase R — No Omniscient Metadata

Never show:

- exact agenda type,
- exact true target,
- exact hidden stage,
- exact next consequence day

before discovery.

Developer diagnostics may.

---

# 106. 132C Phase S — Investigation UX

Before assignment show:

- labor cost,
- investigator suitability,
- known risk,
- expected time,
- uncertainty.

Warn-don’t-block where reasonable.

---

# 107. 132C Phase T — Confrontation UX

Show:

- evidence band,
- clue list,
- risk of false accusation,
- possible response categories.

Do not promise outcome.

---

# 108. 132C Phase U — Accessibility

Suspicion board:

- keyboard navigable,
- controller navigable if supported,
- text labels,
- no color-only evidence state,
- localized.

---

# 109. 132C Phase V — Alert/Briefing Integration

Significant events can appear in briefing:

- clue discovered,
- confrontation ready,
- agenda completed,
- false accusation fallout.

Do not spam every hidden tick.

---

# 110. 132C Phase W — Journal Integration

Journal may archive:

- clue,
- confrontation,
- resolution.

Keep hidden truth out of journal until discovered.

---

# 111. 132C Phase X — Content Integrity Selftest

Validate all template references:

- factions,
- survivors,
- locations,
- items if referenced,
- quest IDs,
- event kinds,
- localization keys.

---

# 112. 132C Phase Y — `--hidden-agendas-selftest`

Selftest scenarios:

1. generate deterministic agenda,
2. clue,
3. investigate,
4. confront,
5. resolve,
6. save/load,
7. verify cross-system consequence,
8. verify old-save empty state,
9. verify cap,
10. verify no-target/high-trust cases.

Expected summary required.

---

# 113. 132C Phase Z — Deliberate Failure Proof

Break:

- target reference,
- port binding,
- save idempotency,
- clue threshold.

Assert relevant gates fail.

---

# 114. 132C Phase AA — 100-Day Fairness Soak

Run representative policies:

```text
investigate_everything
investigate_only_strong_clues
ignore_all
high_trust
low_trust
```

Measure:

- agendas generated,
- false accusations,
- undetected completions,
- player labor spent,
- consequences.

---

# 115. 132C Phase AB — Fairness Targets

Tune toward:

- clues observable but not conclusive,
- investigation materially reduces uncertainty,
- false accusation possible but not arbitrary,
- ignored agendas sometimes complete,
- agenda spam remains low.

Do not overfit exact numbers until playtest evidence exists.

---

# 116. 132C Phase AC — 200-Day Balance Soak

Assert:

- active cap respected,
- no agenda runaway,
- no repeated save exploit,
- no resource theft drains impossible amounts,
- no sabotage dominates all other risk.

---

# 117. 132C Phase AD — Playtest Scenarios

At minimum:

1. correct accusation,
2. false accusation,
3. ignored theft,
4. successful double agent,
5. protected secret/nested reveal.

Observe whether clue fairness feels intelligible.

---

# 118. 132C Phase AE — Documentation

Create:

```text
docs/systems/HIDDEN_AGENDAS.md
```

Include:

- authority,
- lifecycle,
- trigger sources,
- clue model,
- investigation,
- confrontation,
- consequences,
- save schema,
- adding new agenda type.

---

# 119. 132C Phase AF — Extension Contract

To add new agenda type, contributor must supply:

```text
type definition
templates
trigger source
clue family
thresholds
consequence descriptors
tests
utilization evidence
```

No code-only hidden agenda type.

---

# 120. 132C Definition of Done

- [ ] relations integration,
- [ ] pair-history reasons,
- [ ] faction integration,
- [ ] quest integration,
- [ ] moral-choice integration,
- [ ] epilogue/completion integration,
- [ ] inventory/shelter integrity,
- [ ] departure cleanup,
- [ ] old saves,
- [ ] save/load exploit prevention,
- [ ] determinism replay,
- [ ] headless progression,
- [ ] suspicion board,
- [ ] no omniscient UI,
- [ ] accessibility,
- [ ] briefing/journal,
- [ ] content integrity,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] 100-day fairness soak,
- [ ] 200-day balance soak,
- [ ] playtest set,
- [ ] architecture docs.

---

# 121. Branching / Consequence Model — Hardened

```text
Agenda eligibility becomes true
        │
        ▼
deterministic agenda selection
        │
        ▼
agenda active, player unaware
        │
        ├─ staged hidden action
        ├─ clue generation
        ├─ escalation timer
        └─ possible informant event
        │
        ▼
player receives ambiguous evidence
        │
        ├─ ignore
        │    └─ agenda escalates / may complete
        │
        ├─ investigate
        │    ├─ evidence improves
        │    ├─ false lead exposed
        │    └─ target may become cautious
        │
        └─ confront when eligible
             │
             ├─ weak evidence
             │    ├─ denial
             │    ├─ player backs down
             │    └─ false accusation fallout
             │
             └─ strong evidence
                  ├─ confession
                  ├─ partial denial
                  └─ resolution branches
                         │
                         ├─ forgive
                         ├─ expel
                         ├─ monitor
                         ├─ exploit
                         ├─ repair/repay
                         ├─ double agent
                         └─ allow departure
                                │
                                ▼
                         canonical system effects
                                │
                                ▼
                   relations/factions/quests/morality
                                │
                                ▼
                      campaign completion/epilogue
```

---

# 122. Hidden Truth vs Player Knowledge Contract

For every agenda:

```text
truth_state
```

and:

```text
player_knowledge_state
```

are separate.

This prevents:

- omniscient UI,
- unfair instant discovery,
- save-state leakage.

---

# 123. Evidence Contract

Every clue has:

```text
source
reliability
evidence value
known-to-player flag
```

Confrontation reads evidence state.

It does not read hidden agenda truth directly for UI.

---

# 124. Investigation Contract

Investigation:

```text
costs labor/time
may expose clues
may fail
may alert target
```

It never guarantees truth unless evidence reaches authored confirmation threshold.

---

# 125. False Accusation Contract

False accusation is allowed only when:

```text
player confronts with insufficient/incorrect evidence
```

Consequences:

- relation damage,
- possible morale damage,
- ally reaction,
- future agenda probability effects.

---

# 126. Agenda Cap Contract

Initial cap:

```text
2–3 active shelter-wide
```

This should be config, not magic constant.

Soak/playtest may tune.

---

# 127. Agenda Probability Contract

Probability/weights derive from:

- identity,
- trust,
- ideology,
- need,
- faction state,
- prior history,
- cooldown.

Do not use one flat daily probability.

---

# 128. Nested Secret Contract

Protection agendas may reference another agenda.

Depth capped.

No arbitrary graph cycles.

Validator rejects:

```text
A protects B
B protects A
```

unless explicitly designed and safely handled.

---

# 129. Inventory Theft Contract

A theft must correspond to actual removed inventory and actual stash state.

No “story theft” with no accounting.

---

# 130. Sabotage Contract

A sabotage must correspond to actual system degradation/failure risk.

No narrative-only machine damage.

---

# 131. Faction Loyalty Contract

Information leak affects only faction/intel systems that already exist.

No hidden agenda owns faction knowledge globally.

---

# 132. Escape Contract

Departure uses canonical roster/leave flow.

All references cleaned.

---

# 133. Resolution Idempotency Contract

Each consequence has an application key.

Reapplying same resolution:

```text
no-op
```

---

# 134. Save Contract

Persist:

- active agenda instances,
- resolved summaries,
- clues/evidence,
- timing/cooldowns,
- target caution,
- idempotency state.

Do not persist static template content.

---

# 135. Old Save Contract

Missing hidden-agenda section:

```text
valid
```

Initialize empty.

No breaking migration.

---

# 136. Determinism Contract

Same:

```text
seed
state
player actions
```

→ same agenda history.

---

# 137. Port Contract

Required cross-system sinks are declared.

Healthy report:

```text
HIDDEN_AGENDA_REQUIRED_PORTS_MISSING=0
```

or canonical equivalent.

---

# 138. Content Acceptance Contract

Hidden agenda templates should move through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ CLUE_PRODUCED
→ CONFRONTED
→ CONSEQUENCE_PRODUCED
```

A parsed but unreachable agenda is not complete.

---

# 139. UI Contract

Player-facing suspicion board shows only known facts.

Never:

```text
true agenda type
true target
next hidden stage
hidden RNG
```

before discovery.

---

# 140. Narrative Contract

Clues should be:

- specific,
- concrete,
- ambiguous,
- attributable.

Avoid omniscient exposition.

---

# 141. Fairness Contract

A betrayal is fair if:

- clues existed,
- player had some chance to notice,
- investigation could reduce uncertainty,
- ignored escalation followed authored timing.

Not every agenda must be preventable.

But no catastrophic consequence should be entirely untelegraphed unless explicitly designed and rare.

---

# 142. Save-Scum Guard Contract

Reloading may not:

- reroll agenda type,
- reroll clue timing,
- reroll confrontation response,
- reapply different consequence.

All relevant state persists.

---

# 143. Performance Contract

Agenda system should be event/day driven.

No per-frame scans across every survivor/agenda/template.

---

# 144. Retention Contract

Resolved agenda history may need bounded retention under Plan 55.

Preserve:

- permanent flags,
- landmark betrayals,
- completion-record inputs.

Detailed low-value clues may roll up later.

---

# 145. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| too many agendas overwhelm player | High | High | active cap + cooldown |
| clues too obvious | Medium | Medium | ambiguity + shared clue families |
| clues too hidden | Medium | High | fairness soak + playtest |
| false accusations feel arbitrary | Medium | High | state-backed reliability/evidence |
| save scumming rerolls outcome | Medium | High | persisted RNG/state/idempotency |
| hidden agenda duplicates relation state | Medium | High | canonical relation APIs |
| sabotage creates parallel damage system | Medium | High | existing shelter systems only |
| join-escape branch exceeds current campaign architecture | Medium | High | explicit prerequisite/defer |
| nested protection creates recursion | Medium | High | depth cap/validator |
| agenda spam floods briefing | Medium | Medium | significance filtering |
| old saves suddenly spawn betrayal | Medium | High | legacy grace window |
| blackmail mechanics create unexplained forced labor | Medium | Medium | route through existing labor/relations |

---

# 146. Commit Strategy

## 132A — Foundation

### C2[26].1 — baseline + hidden-agenda ADR

### C2[26].2 — definition/instance/state DTOs

### C2[26].3 — trigger sources + eligibility

### C2[26].4 — deterministic generation/caps/cooldowns

### C2[26].5 — clue/evidence model

### C2[26].6 — investigation workflow

### C2[26].7 — confrontation/resolution state machine

### C2[26].8 — save schema + old-save compatibility

### C2[26].9 — port contract + headless tick

### Gate: 132A complete

---

## 132B — Content

### C2[26].10 — hidden_agendas.json schema

### C2[26].11 — faction loyalty templates

### C2[26].12 — theft templates

### C2[26].13 — sabotage templates

### C2[26].14 — escape templates

### C2[26].15 — protection/nested-secret templates

### C2[26].16 — escalation/cross-agenda/informants

### C2[26].17 — localization/narrative/utilization

### Gate: 132B complete

---

## 132C — Integration

### C2[26].18 — relations/pair history

### C2[26].19 — factions/quests/morality

### C2[26].20 — inventory/shelter/departure integrity

### C2[26].21 — epilogue/completion record

### C2[26].22 — suspicion board/accessibility

### C2[26].23 — save-scum/determinism/headless tests

### C2[26].24 — content integrity/selftest

### C2[26].25 — fairness/balance soaks

### C2[26].26 — playtest/docs

### Gate: 132C complete

---

# 147. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --hidden-agendas-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
content-utilization report
same-seed replay
old-save fixture load
save/load exploit matrix
100-day agenda fairness soak
200-day agenda balance soak
suspicions-board snapshot/accessibility check
```

---

# 148. Flagship Definition of Done

## 132A — Foundation

- [ ] HiddenAgendaSystem exists,
- [ ] definition/instance split,
- [ ] five canonical agenda types,
- [ ] truth vs knowledge separation,
- [ ] deterministic trigger selection,
- [ ] active cap,
- [ ] immunity/trust suppression,
- [ ] clue model,
- [ ] investigation,
- [ ] confrontation,
- [ ] resolution branches,
- [ ] consequence descriptors,
- [ ] save state/version,
- [ ] old-save compatibility,
- [ ] idempotency,
- [ ] headless tick,
- [ ] port contract,
- [ ] semantic events.

## 132B — Content

- [ ] hidden_agendas.json,
- [ ] 15 templates,
- [ ] faction loyalty,
- [ ] theft,
- [ ] sabotage,
- [ ] escape,
- [ ] protection,
- [ ] delayed escalation,
- [ ] cross-agenda interaction,
- [ ] informant mechanic,
- [ ] false accusation,
- [ ] cooldowns,
- [ ] localized text,
- [ ] utilization report,
- [ ] no unreachable template without disposition.

## 132C — Closure

- [ ] relations consequences,
- [ ] pair-history reasons,
- [ ] faction consequences,
- [ ] quest unlocks,
- [ ] morality effects,
- [ ] epilogue/completion inputs,
- [ ] inventory integrity,
- [ ] sabotage integrity,
- [ ] departure cleanup,
- [ ] save/load exploit prevention,
- [ ] same-seed determinism,
- [ ] old saves load,
- [ ] suspicion board,
- [ ] no omniscient UI,
- [ ] accessibility,
- [ ] headless behavior,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] fairness soak,
- [ ] balance soak,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no parallel relationship system,
- [ ] no parallel faction ledger,
- [ ] no hidden agenda without clues,
- [ ] no agenda consequence only in prose,
- [ ] no save-scum reroll,
- [ ] no unlimited agenda spam,
- [ ] full verification green.

---

# 149. Closure Report Template

```markdown
## C2[26] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Survivors:
- Identity coverage:
- Existing secret definitions:
- Relation consumers:
- Faction sinks:
- Quest sinks:
- Save schema:

### 132A — Foundation
- Agenda types:
- Trigger sources:
- Determinism:
- Active cap:
- Immunity/suppression:
- Clue families:
- Investigation:
- Confrontation:
- Save state:
- Old-save migration:
- Port contract:
- Result:

### 132B — Content
- Templates total:
- Loyalty:
- Theft:
- Sabotage:
- Escape:
- Protection:
- Escalation:
- Informant:
- False accusation:
- Unreachable templates:
- Localization:
- Result:

### 132C — Integration
- Relation outcomes:
- Faction outcomes:
- Quest unlocks:
- Moral outcomes:
- Epilogue inputs:
- Inventory accounting:
- Shelter damage:
- Survivor departure:
- Save/load exploit failures:
- Determinism digest:
- UI:
- Accessibility:
- Headless:
- Result:

### Fairness / Balance
- 100-day agendas generated:
- False accusations:
- Ignored agenda completions:
- Average investigation labor:
- Active-cap violations:
- 200-day runaway agendas:
- Resource-loss ceiling:
- Sabotage dominance:
- Playtest notes:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Hidden-agendas selftest:
- Port contract:
- Old-save fixtures:
- Determinism:
- Fairness soak:
- Balance soak:
- Verify fast:

### Final Metrics
- ACTIVE_AGENDA_CAP:
- TEMPLATES_TOTAL:
- TEMPLATES_ELIGIBLE:
- TEMPLATES_ACTIVATED:
- CLUES_GENERATED:
- CONFRONTATIONS:
- RESOLUTIONS:
- FALSE_ACCUSATIONS:
- SAVE_REROLL_VIOLATIONS:
- REQUIRED_PORTS_MISSING:
- CONTENT_EFFECT_PRODUCED:

### Remaining Debt
- Agenda content:
- UI:
- Voice:
- Factions:
- Quest aftermath:
- Epilogue:
```

---

# 150. Final Execution Directive

Execute Plan 132 as a **persistent hidden-motive layer over existing survivor, faction, inventory, quest, and relation authorities**.

The critical sequence is:

```text
build one deterministic agenda lifecycle
→ separate hidden truth from player knowledge
→ generate ambiguous clues
→ make investigation cost labor/time
→ gate confrontation on evidence
→ resolve through canonical systems
→ persist every meaningful state
→ block save/load rerolls
→ cap agenda pressure
→ prove fairness and consequence in long seeded runs
```

Do not make the player omniscient.

Do not hide catastrophes with zero prior clue unless explicitly authored and exceptionally rare.

Do not let a theft exist without inventory accounting.

Do not let sabotage exist without actual system damage.

Do not create agenda-specific trust, morality, faction standing, or quest state.

Do not let save/load reselect an agenda or confrontation outcome.

The strongest secrecy rule is:

> **The system may know the agenda; the player only knows the evidence they have actually earned.**

The strongest integration rule is:

> **Hidden agendas own motive progression and evidence, while every real consequence is applied through the existing authoritative system that already owns that fact.**

The strongest fairness rule is:

> **A betrayal can surprise the player, but it must be retrospectively legible: clues existed, investigation could have reduced uncertainty, and ignored escalation followed deterministic authored rules.**

The flagship acceptance scenario is:

> **Start a seeded campaign with one eligible faction-loyal survivor and one high-trust survivor who should never trigger. Let the first agenda activate, observe ambiguous radio/absence clues, assign an investigator, save/load before the result, confront once evidence is strong, choose a double-agent resolution, and verify the same agenda, clue timing, investigation outcome, relation/faction consequences, persistent flags, and epilogue input reproduce identically across reloads—while the high-trust survivor remains agenda-free and no hidden truth is exposed in the UI before discovery.**
