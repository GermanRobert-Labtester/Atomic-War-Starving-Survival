# C2 — Flagship Integration Plan [18]: Deterministic Survivor Voice, Delivery Contracts, and Social Speech That Reflects State

> **Deliverable:** `C2_planintegration[18].md`
> **Source scope:** Plan 42 — *A Voice for Each of Them: Survivors Who Say Things*
> **Wave:** Continuity Wave 6 — *The People In It*
> **Primary objective:** create a deterministic, data-authored survivor voice system keyed to identity, state, place, and moment; route selected lines through existing UI/journal/audio surfaces under strict attention budgets; then close the social loop so what survivors say reflects relationships, policy, grief, leadership, and player-caused events without creating a dialogue engine.
> **Required execution order:** **40A → 31A → 42A → 42B → 42C**
> **Hard prerequisites:** 40A authored survivor identity; 31A semantic event kinds/cause IDs; 36A port-contract enforcement for delivery sinks; 25A/25C localization key layer; 41A memory/grief for later speech-about-people scenarios.
> **Scope discipline:** no dialogue tree, no chat system, no conversational AI, no new panel, no new audio family, no hardcoded survivor prose in C#, no voice line as the sole carrier of a mechanical warning, and no line selected from information the speaker could not plausibly know.

---

# 0. Executive Intent

ASHFALL already has many ingredients of a living cast:

- 129 survivor definitions with biography/profession/traits,
- authored belief/keepsake/phantom-background identity from Plan 40,
- relation and friction systems,
- needs, fatigue, grief, trauma, and leadership state,
- semantic day events,
- journal, briefing, survivor detail, HUD/status surfaces,
- radio voice-register discipline,
- authored roster bark/inspect sentence precedent,
- audio event infrastructure.

What is missing is not a conversation framework.

What is missing is a **deterministic speaker-selection and line-delivery layer**.

The correct model is:

```text
who is the speaker?
+ what just happened?
+ what does this survivor know?
+ where are they?
+ what state are they in?
+ what register fits them?
→ deterministic candidate set
→ deterministic weighted pick
→ cooldown/repeat suppression
→ declared delivery route
→ journal record
→ optional existing cue
```

The player-facing result should be:

> **Survivors speak because something happened to them, around them, or because of the player — and the player can trace why the line fired.**

The system must never become ambient chatter divorced from state.

---

# 1. Source Diagnosis

The source establishes:

- 118 authored radio broadcasts but no survivor-specific voice system.
- Duty-roster marks already prove authored sentence selection can work in data.
- No equivalent survivor bark/quip selection system exists.
- Identity inputs exist after 40A.
- State/event inputs exist across day events, needs, fitness, relations, grief, leadership, trauma.
- Faction voice-register design already exists.
- Multiple UI surfaces can receive lines without inventing a new panel.
- Dialogue trees are not required and are explicitly out of scope.
- New prose must use the localization/key system.

The architectural conclusion is:

```text
voice is a deterministic projection of existing state
```

not:

```text
voice creates state
```

and not:

```text
voice is a parallel conversation simulation
```

---

# 2. Program-Level Success Criteria

C2[18] closes only when:

1. A canonical survivor voice catalog exists.
2. Every line uses a stable ID and localization key.
3. Every speaker selector/reference resolves.
4. Every trigger resolves to canonical semantic event vocabulary.
5. Every place/flag/reference resolves.
6. Every survivor resolves to at least a default voice register.
7. Selection is deterministic for same seed/state.
8. Per-survivor cooldown and no-immediate-repeat are enforced.
9. Knowledge constraints suppress lines the speaker could not know.
10. Every selected line has a declared delivery sink.
11. Unbound delivery fails port validation instead of silently dropping.
12. Delivery never interrupts a decision.
13. Journal archives delivered survivor lines with speaker/day/trigger.
14. Survivor lines and radio remain distinct channels.
15. Mechanical warnings never rely on voice alone.
16. Rebinding/session swap cannot double-deliver lines.
17. Grievances, friction, policy reactions, and leadership state can produce speech.
18. Player-caused speech is attributable through cause/event IDs.
19. A 200-day seeded utilization report identifies dead/unused authored lines.
20. No survivor prose is hardcoded in C#.

---

# 3. Architectural Invariants

## 3.1 Voice is data-authored

Core/host stores selection logic and contracts.
Text lives behind localization keys.

## 3.2 Identity comes from Plan 40

Speaker register/profile must use canonical authored survivor identity.
No new inference shim.

## 3.3 Triggers come from Plan 31

Do not invent a second event vocabulary.

## 3.4 Selection is deterministic

Use `ISeededRng` / campaign stream IDs.
No `System.Random`.
No UI-time randomness.

## 3.5 Knowledge gates apply before selection

A matching line is not eligible if the speaker could not know the referenced fact.

## 3.6 Voice never owns consequences

If a line implies refusal, grief, shortage, policy reaction, etc., the corresponding state must already exist.

## 3.7 Delivery is a port

Every selected line routes to a declared sink.
No silent discard.

## 3.8 Journal is durable archive

Murmurs can be ephemeral on screen, but delivered survivor lines worth retaining are archived according to policy.

## 3.9 Radio and survivor speech are separate presentation lanes

World voice ≠ shelter voice.

## 3.10 Attention is budgeted

No interruption-class chatter.
No uncontrolled per-frame barks.
No crisis-time spam.

---

# 4. Dependency Graph

```text
40A authored identity
     │
     ▼
31A semantic events + causeId
     │
     ▼
42A line bank + deterministic selection
     │
     ▼
42B delivery ports + existing surfaces
     │
     ▼
42C social/policy speech loops

36A port contract ─────────► delivery must bind
25A/25C localization ──────► all text keys
41A memory/grief ──────────► speech-about-people
24A fitness / 43 leadership► register/state inputs
32C knowledge model ───────► speaker knows/doesn't know
```

Required sequence:

```text
40A → 31A → 42A → 42B → 42C
```

---

# 5. Baseline Capture

Before implementation, record:

- survivor count,
- radio broadcast count,
- existing roster mark/bark mechanism,
- available semantic event kinds,
- authored identity coverage,
- journal/briefing delivery APIs,
- current audio cue validation,
- existing voice-register metadata,
- current localization/new-string gate status.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

Capture a representative populated-cast save and social-state snapshot.

---

# 6. Workstream 42A — The Line Bank

## 6.1 Objective

Create a schema-driven survivor line catalog with deterministic matching, register discipline, knowledge constraints, cooldown, and utilization reporting.

---

# 7. 42A Phase A — Write the Voice Line Spec First

Create:

```text
docs/voice/VOICE_LINE_SPEC.md
```

Before writing content.

Define stable schema fields.

Recommended conceptual shape:

```json
{
  "id": "voice_survivor_perished_medic_01",
  "speaker": {
    "survivor_id": null,
    "belief_profile_id": null,
    "archetype_id": null
  },
  "trigger": "survivor_perished",
  "conditions": {
    "need_bands": {},
    "affinity": {},
    "location_id": null,
    "fitness_band": null,
    "day_min": null,
    "day_max": null,
    "knowledge_requirements": []
  },
  "register": "clipped",
  "lexicon_tags": ["medical"],
  "text_key": "voice.survivor_perished.medic.01",
  "weight": 1.0,
  "cooldown_days": 7,
  "delivery_class": "event"
}
```

Use actual project naming conventions.

---

# 8. 42A Phase B — Stable Line Identity

Every line ID must be:

- globally unique,
- stable across localization,
- independent of text wording,
- snake_case if matching project convention.

Never key save/cooldown state by localized text.

---

# 9. 42A Phase C — Speaker Selector Contract

Support selectors in precedence order.

Potential:

1. exact survivor ID,
2. belief profile,
3. profession/archetype,
4. default survivor register.

Do not allow ambiguous selector semantics.

Document specificity ordering.

---

# 10. 42A Phase D — Trigger Contract

Every trigger must resolve to canonical Plan 31 event kind.

Do not store arbitrary event strings.

Validation fails unknown kinds.

---

# 11. 42A Phase E — Condition Vocabulary

Conditions may include only canonical state dimensions.

Examples:

- NeedKind band,
- affinity band,
- location ID,
- fitness band,
- day window,
- grief state,
- leadership stress,
- age class,
- known-fact requirement.

Each condition family must define:

- authority,
- query interface,
- default behavior.

---

# 12. 42A Phase F — Voice Register Vocabulary

Extend/reuse existing faction register design.

Create an individual voice-register vocabulary such as:

```text
clipped
bureaucratic
devotional
pragmatic
weary
clinical
guarded
```

Actual set should be small and reviewed.

Do not create dozens of one-off registers.

---

# 13. 42A Phase G — Register Assignment

Each survivor resolves to a register through authored identity.

Inputs may include:

- profession,
- belief profile,
- age class,
- explicit authored override.

Never infer register from arbitrary personality heuristics if an authored mapping can own it.

---

# 14. 42A Phase H — Lexicon Tags

Use tags to vary phrasing without changing state semantics.

Examples:

```text
medical
mechanical
religious
bureaucratic
military
domestic
```

A fitter and medic can discuss the same storm differently.

Do not let lexicon tags become unvalidated free text.

---

# 15. 42A Phase I — First Vertical Slice

Author narrowly:

```text
trigger = survivor_perished
× 4 registers
× limited condition set
```

Target approximately 12–20 strong lines.

Do not mass-author hundreds before schema and delivery are proven.

---

# 16. 42A Phase J — Core Voice Catalog Loader

Create:

```text
Assets/Ashfall.Core/Voice/VoiceLineCatalogLoader.cs
```

Responsibilities:

- parse catalog,
- validate shape,
- normalize ordering,
- surface diagnostics,
- remain engine-free.

---

# 17. 42A Phase K — `SurvivorVoiceSystem`

Create engine-free Core system.

Responsibilities:

```text
MatchCandidates
FilterKnowledge
FilterCooldown
ApplyRegisterSpecificity
DeterministicWeightedPick
RecordSelectionCooldown
Return VoiceLineSelection
```

No UI knowledge.
No Godot types.

---

# 18. 42A Phase L — Deterministic Candidate Ordering

Before RNG selection, sort candidates by stable line ID.

Weighted selection runs over canonical order.

Same seed + state → same selection.

---

# 19. 42A Phase M — RNG Stream Discipline

Define dedicated campaign stream ID.

Example:

```text
survivor_voice
```

or event-derived substream.

Do not consume unrelated global RNG sequence in a way that perturbs simulation.

Prefer:

```text
event identity + speaker + day
→ stable fork
```

if existing RNG infrastructure supports it.

---

# 20. 42A Phase N — Cooldown State

Track per survivor:

```text
line_id
last_fired_day
```

or compact equivalent.

Cooldown state is dynamic and must persist if it affects future selection deterministically across save/load.

---

# 21. 42A Phase O — No Immediate Repeat

Even if cooldown is zero/small, enforce no immediate repeat where alternatives exist.

Test single-candidate behavior explicitly.

---

# 22. 42A Phase P — Knowledge Constraint

Before candidate eligibility:

```text
Does speaker know the referenced fact?
```

Examples:

- location discovered to speaker?
- event occurred in their presence?
- rumor delivered?
- household grievance directly observed?

Integrate existing knowledge/intel/memory queries where available.

---

# 23. 42A Phase Q — No Invented Interiority

Line conditions may refer to:

- authored identity,
- known state,
- observed events,
- saved relationships.

Do not generate a hidden motivation not represented in state.

---

# 24. 42A Phase R — Child/Elder Register Rules

Age class influences:

- eligible registers,
- topic restrictions,
- tone.

Source constraint:

```text
child should not narrate material beyond plausible perspective
```

Add explicit content-policy validation where possible.

---

# 25. 42A Phase S — Localization Contract

Every line stores:

```text
text_key
```

not prose.

English source value lives in localization resources.

No raw line text in C#.

---

# 26. 42A Phase T — Catalog Integrity Tier

Validate:

- line ID unique,
- speaker reference valid,
- trigger valid,
- location valid,
- flag valid,
- register valid,
- lexicon tag valid,
- text key resolves,
- weight > 0,
- cooldown >= 0,
- delivery class valid,
- optional cue ID valid if declared.

---

# 27. 42A Phase U — Survivor Coverage

Every survivor must resolve to at least:

- default register,
- at least one possible generic line family in mature content.

For initial slice, coverage may be register-only while line-family coverage remains staged, but the gate must report the gap.

---

# 28. 42A Phase V — Unused-Line Report

Run 200-day seeded scenario.

Output:

```text
line_id
eligible_count
selected_count
delivered_count
blocked_by_cooldown
blocked_by_knowledge
blocked_by_surface_budget
```

This distinguishes dead content from merely rare content.

---

# 29. 42A Phase W — Tier-2 Utilization Gate

Do not necessarily fail every unused line immediately.

Start with report/ratchet:

```text
new unreachable line → fail
rare-but-reachable line → report
```

Set deliberate threshold later.

---

# 30. 42A Tests

- schema valid,
- malformed schema diagnostics,
- unknown speaker,
- unknown trigger,
- unknown location,
- register resolution,
- candidate specificity,
- deterministic weighted pick,
- cooldown,
- no-repeat,
- knowledge block,
- child-topic restriction,
- localization key resolution,
- save/load cooldown determinism,
- unused-line report shape.

---

# 31. 42A Definition of Done

- [ ] voice-line spec,
- [ ] stable schema,
- [ ] speaker selectors,
- [ ] canonical triggers,
- [ ] condition vocabulary,
- [ ] register vocabulary,
- [ ] lexicon tags,
- [ ] first vertical slice,
- [ ] loader,
- [ ] SurvivorVoiceSystem,
- [ ] dedicated RNG discipline,
- [ ] cooldown/no-repeat,
- [ ] knowledge gating,
- [ ] age-class rules,
- [ ] text keys only,
- [ ] integrity tier,
- [ ] survivor register coverage,
- [ ] 200-day utilization report.

---

# 32. Workstream 42B — Delivery Through Existing Surfaces

## 32.1 Objective

Route selected survivor lines into existing interfaces through declared ports, under strict attention and idempotency budgets.

---

# 33. 42B Phase A — Define Delivery DTO

Create a small record:

```text
VoiceLineDelivery
```

Fields:

```text
line_id
speaker_id
trigger_kind
cause_id
day
delivery_class
text_key
optional_cue_id
journal_policy
```

No resolved localized prose in Core.

---

# 34. 42B Phase B — Delivery Port Contract

Define producer/sink seam using Plan 35A/36A discipline.

Examples:

```text
ISurvivorVoiceSink
VoiceLineProduced
```

Actual API should match existing port conventions.

Requirement:

```text
selected line without bound sink → validation failure
```

Never silent drop.

---

# 35. 42B Phase C — Delivery Classes

Define three attention classes.

## Murmur

- HUD/status/detail panel,
- low priority,
- dismissible,
- never blocks.

## Event

- briefing/journal,
- persistent record,
- moderate priority.

## Interruption

For this plan:

```text
NOT ALLOWED
```

No voice line should interrupt an active decision.

---

# 36. 42B Phase D — Surface Routing

Route classes to existing surfaces.

Potential:

```text
MURMUR → HUD / SurvivorDetail
EVENT → Briefing + Journal
FLASHBACK → Journal + appropriate HUD/detail treatment
```

Do not create a chat panel.

---

# 37. 42B Phase E — Journal Archive

For archived lines record:

- speaker ID,
- day,
- trigger,
- cause ID,
- line ID/text key,
- optional location.

Journal is the durable reading surface.

---

# 38. 42B Phase F — Radio Separation

Do not route survivor lines through faction/radio feeds.

Keep:

```text
radio = outside world
survivor voice = shelter interior
```

Visually and semantically separate.

---

# 39. 42B Phase G — Flashback Voice Class

Integrate existing trauma/flashback trigger machinery.

Constraints:

- high priority within voice,
- no gameplay-alert confusion,
- fragmented/present-tense register,
- not used for mechanical instruction,
- cooldown and anti-spam.

---

# 40. 42B Phase H — Per-Surface Density Budgets

Data/config defines limits.

Examples:

```text
HUD murmurs/day
briefing voice entries/day
journal-only entries/day
flashbacks/day
```

Actual N values require playtest.

---

# 41. 42B Phase I — Per-Speaker Density Budget

Prevent one survivor from dominating.

Track:

```text
lines/day
lines/window
```

with significance exceptions only if justified.

---

# 42. 42B Phase J — Crisis Attenuation

During high-attention decisions/crises:

- suppress murmurs,
- keep important event lines for later journal/briefing delivery.

Do not discard the record; defer low-priority presentation.

---

# 43. 42B Phase K — Mechanical Warning Duplication Rule

If voice references a real warning:

```text
filter clogged
medicine low
raid imminent
```

then canonical warning must also exist in:

- panel,
- briefing,
- status,
- other mechanical surface.

Add test that voice is not sole carrier.

---

# 44. 42B Phase L — Delivery Idempotency Key

Define stable key:

```text
(line_id, speaker_id, event_id/cause_id, day)
```

or canonical equivalent.

Across save/load/session rebind, same event cannot duplicate delivery.

---

# 45. 42B Phase M — Session-Swap Safety

On load/new game:

- rebuild sink binding,
- restore cooldown/delivery history,
- avoid replaying already-delivered entries unless journal projection intentionally re-renders history.

---

# 46. 42B Phase N — Optional Audio Cue

Line may carry existing cue ID.

Validation:

- cue exists,
- cue family allowed,
- no new audio batch required.

Text remains primary semantic carrier.

---

# 47. 42B Phase O — UI Accessibility

Murmur/event line UI must be:

- keyboard readable,
- dismissible where applicable,
- focus-safe,
- high contrast,
- screen-reader named where supported,
- non-essential for mechanical warning comprehension.

---

# 48. 42B Phase P — Localization Timing

Resolve text key at presentation time.

Do not persist localized string if journal can persist key + args safely.

This allows locale changes without corrupting record semantics.

---

# 49. 42B Phase Q — Briefing Integration

Voice entry should appear as a distinct semantic subtype.

Do not allow chatter to swamp system transitions.

Apply significance and density cap.

---

# 50. 42B Phase R — Survivor Detail Integration

Detail panel may show:

- recent remark,
- recent archived line,
- known voice context.

Do not show all hidden eligible lines.

---

# 51. 42B Phase S — HUD Murmur Presentation

Murmur should:

- time out,
- be dismissible,
- not steal focus,
- not overlap critical alerts,
- use speaker attribution.

---

# 52. 42B Phase T — Snapshots

Create populated snapshots for:

- survivor detail with recent remark,
- briefing with survivor event lines,
- HUD murmur alongside non-voice status.

---

# 53. 42B Tests

- route binding,
- unbound sink failure,
- delivery-class routing,
- density budget,
- speaker budget,
- crisis suppression/defer,
- journal persistence,
- warning-duplication rule,
- load/rebind idempotency,
- audio cue validation,
- deterministic delivered sequence,
- localization-key persistence.

---

# 54. 42B Definition of Done

- [ ] delivery DTO,
- [ ] port contract,
- [ ] no unbound silent drop,
- [ ] attention classes,
- [ ] existing-surface routing,
- [ ] journal archive,
- [ ] radio separation,
- [ ] flashback class,
- [ ] per-surface budgets,
- [ ] per-speaker budgets,
- [ ] crisis attenuation,
- [ ] warning duplication rule,
- [ ] idempotency,
- [ ] session-swap safety,
- [ ] optional existing cue validation,
- [ ] accessibility,
- [ ] snapshots.

---

# 55. Workstream 42C — What They Say About You and Each Other

## 55.1 Objective

Make speech evidence of world/social state and player decisions rather than ambient filler.

---

# 56. 42C Phase A — Define Speech Objects

Three top-level subject classes:

```text
WORLD
PEOPLE
PLAYER_DECISION
```

## WORLD

- weather,
- shortage,
- contamination,
- territory,
- shelter conditions.

## PEOPLE

- bonds,
- grudges,
- grief,
- mediation,
- conflict.

## PLAYER_DECISION

- ration cuts,
- duty assignments,
- triage decisions,
- policy outcomes,
- refusals.

Store subject type in data.

---

# 57. 42C Phase B — Cause Attribution

For player-decision speech, line selection must receive canonical `causeId`.

This allows:

```text
policy event
→ causeId
→ eligible survivor reactions
→ line
```

No vague “player did something bad” synthetic state.

---

# 58. 42C Phase C — Grievance Speech

Integrate `RationConflictSystem`.

Flow:

```text
ration conflict state change
→ semantic event
→ affected survivor knows
→ grievance line family eligible
→ delivery
```

The grievance exists in state first.

---

# 59. 42C Phase D — Ideological Friction Pairing

When two belief profiles clash:

- each may produce one line,
- escalation can alter register,
- lines should not fire simultaneously if readability suffers,
- paired sequence remains deterministic.

No raw affinity-only presentation.

---

# 60. 42C Phase E — Praise/Blame Eligibility

After player policy:

- determine materially affected survivors,
- apply identity/register filters,
- choose plausible speakers.

Do not make every survivor react to every policy.

---

# 61. 42C Phase F — Direct vs Second-Hand Speech

Model:

```text
direct reaction
second-hand remark
mediated confrontation
```

where systems support it.

A low-standing or hidden grievance may become audible second-hand before direct confrontation.

Ordering must be state-backed.

---

# 62. 42C Phase G — Leadership Voice

Leadership state can influence register.

Inputs may include:

- stress band,
- leadership stability,
- break risk,
- authority state.

Do not create new leadership mechanics.

Voice only projects existing state.

---

# 63. 42C Phase H — Policy Announcement Channel

Where leader is designated speaker:

```text
policy event
→ leader voice line
```

This is a projection of actual policy.
Not a parallel policy system.

---

# 64. 42C Phase I — Rumour Delivery Channel

Coordinate with information-network plans.

Voice may carry a fact the speaker knows.

This plan owns:

```text
speech delivery of known fact
```

It does not own:

```text
full rumor propagation simulation
```

---

# 65. 42C Phase J — Knowledge Provenance

For a rumor/world line, record:

- known fact ID,
- speaker knowledge source,
- event/cause,
- optional location.

Eligibility must confirm the knowledge is already available to speaker.

---

# 66. 42C Phase K — No Voice-Only Causality

If line says:

```text
"I won't work with him."
```

then refusal/relationship state must exist independently.

Voice cannot invent a gameplay consequence.

---

# 67. 42C Phase L — Contradiction Guard

Build state-consistency checks.

Examples:

- do not praise full rations when `ration_short`,
- do not discuss a dead survivor as alive,
- do not claim route knowledge unavailable to speaker,
- do not use calm leader register during broken state if register rules prohibit it.

---

# 68. 42C Phase M — Line-Family Content Budget

Each speech object type gets deliberate initial families.

Avoid broad content explosion.

Suggested phased content:

1. ration grievance,
2. death/grief,
3. ideological friction,
4. player duty/policy response,
5. leadership state,
6. rumor/world knowledge.

---

# 69. 42C Phase N — Tone Rules

Content must be:

- cold,
- tired,
- restrained,
- concrete,
- human,
- non-moralizing.

Avoid:

- exposition,
- jokes,
- omniscient narration,
- tutorial prose disguised as dialogue.

Run `ashfall-write` / narrative checks.

---

# 70. 42C Phase O — Relations Read-Model Integration

Recent relevant speech can be linked from relationship reasons.

Do not make voice history the relationship authority.

Relationship system owns the state.
Voice record explains/illustrates it.

---

# 71. 42C Phase P — Journal Cross-Linking

Journal entries should allow:

```text
speaker
→ survivor detail
trigger
→ source event
cause
→ relevant briefing/system
```

using existing route/click-through architecture.

---

# 72. 42C Phase Q — Repetition Control

Avoid repeated same-topic lines.

Track:

- line cooldown,
- family cooldown,
- per-trigger cooldown,
- speaker density.

A shelter should not repeat one grievance every day.

---

# 73. 42C Phase R — Soak Contradiction Test

Run a long seeded simulation.

Collect every delivered line with state snapshot.

Assert:

```text
line conditions were true at delivery
```

and run targeted contradiction rules.

---

# 74. 42C Phase S — Player-Causality Coverage

Ensure at least representative reactions to:

- ration cut,
- triage refusal,
- duty assignment,
- survivor death,
- major policy/leadership event.

No need for every action to have bespoke lines initially.

---

# 75. 42C Tests

- world speech object,
- people speech object,
- player-decision speech object,
- causeId propagation,
- grievance eligibility,
- paired-friction ordering,
- praise/blame speaker selection,
- second-hand-before-confrontation,
- leader register by stress,
- rumor knowledge constraint,
- no voice-only consequence,
- contradiction guard,
- family cooldown,
- long-soak condition validity.

---

# 76. 42C Definition of Done

- [ ] speech object taxonomy,
- [ ] cause attribution,
- [ ] grievance lines,
- [ ] friction paired lines,
- [ ] policy reactions,
- [ ] direct/second-hand ordering,
- [ ] leadership voice,
- [ ] policy announcements,
- [ ] rumor delivery channel,
- [ ] knowledge provenance,
- [ ] no voice-only causality,
- [ ] contradiction guard,
- [ ] content-budget discipline,
- [ ] tone gate,
- [ ] relations/journal cross-links,
- [ ] repetition control,
- [ ] soak contradiction test.

---

# 77. Integrated Voice Pipeline

```text
Semantic event / state change
        │
        ▼
speaker eligibility
        │
        ├─ identity
        ├─ relation
        ├─ needs/fitness
        ├─ place
        ├─ age
        └─ knowledge
        │
        ▼
candidate line set
        │
        ├─ register
        ├─ lexicon
        ├─ cooldown
        ├─ family cooldown
        └─ delivery class
        │
        ▼
deterministic seeded selection
        │
        ▼
VoiceLineDelivery
        │
        ├─ HUD/detail murmur
        ├─ briefing
        ├─ journal
        └─ optional existing cue
```

---

# 78. Voice Authority Contract

The voice system owns:

- line matching,
- deterministic selection,
- cooldown,
- delivery proposal.

It does not own:

- relationship state,
- grief state,
- policy state,
- warnings,
- knowledge creation,
- audio assets,
- dialogue choices.

---

# 79. Knowledge Contract

A survivor can only speak about:

- what they experienced,
- what a known information channel delivered,
- what the shelter publicly announced,
- what their relationship state plausibly exposes.

No omniscient speech.

---

# 80. Determinism Contract

Same:

```text
seed
campaign state
event sequence
knowledge state
```

must produce the same:

```text
selected line IDs
delivery order
cooldown state
```

across save/load.

---

# 81. Save Contract

Persist only dynamic voice state required for deterministic continuity:

- cooldown history,
- delivered idempotency keys where necessary,
- archived journal references through journal authority.

Do not persist the whole line catalog.

---

# 82. Localization Contract

Persistent identity:

```text
line_id
text_key
```

Presentation resolves current locale.

Do not persist rendered English unless journal architecture requires immutable historical prose; if so, document why and preserve key too.

---

# 83. Attention Budget Contract

Every delivery class defines:

```text
max/day/surface
max/day/speaker
crisis suppression
journal retention
```

No ad-hoc limits in individual panels.

---

# 84. Mechanical Warning Contract

Voice may repeat or humanize a warning.

It may never be the only place the warning exists.

Gate/test representative warning topics.

---

# 85. Port Contract Integration

Plan 36 must expose:

```text
voice producer
required sinks
bound sinks
missing sinks
```

Healthy boot:

```text
VOICE_DELIVERY_MISSING=0
```

or equivalent.

---

# 86. Content Utilization Contract

Voice catalog reporting should distinguish:

```text
AUTHORED
ELIGIBLE
SELECTED
DELIVERED
ARCHIVED
```

A line parsed but never eligible is not “consumed.”

---

# 87. Audio Contract

Optional cue IDs use existing audio catalog.

No new audio-family dependency.

Text and state remain complete without cue playback.

---

# 88. Accessibility Contract

Speech surfaces must:

- remain keyboard readable,
- not steal focus,
- provide text equivalent for cue-only emotion,
- respect text scale,
- respect reduce-motion where transitions animate.

---

# 89. Failure Modes

## Hardcoded prose appears in C#

Fail string gate.

## Line trigger is arbitrary text

Fail event-kind validation.

## Candidate line references unknown place/flag

Fail integrity.

## Speaker comments on unknown fact

Fail knowledge test.

## Random line sequence differs across reload

Fix RNG/cooldown persistence.

## Selected line is dropped because no sink

Port validation fails.

## Survivor line overwhelms briefing

Apply attention budget.

## Voice says warning no other system exposes

Fail warning-parity test.

## Load repeats yesterday's line

Fix idempotency key/history.

## Radio and survivor speech mix lanes

Fix routing.

## Line implies refusal that state does not contain

Remove/retarget line; voice never owns consequence.

---

# 90. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| content explosion before schema stabilizes | High | Medium | vertical slice first |
| non-deterministic selection | Medium | High | stable candidate order + seeded stream |
| save/load repeats | Medium | High | cooldown/idempotency persistence |
| invented knowledge | Medium | High | knowledge gate |
| chatter overload | High | Medium | surface/speaker budgets |
| UI warning dependency on voice | Medium | High | warning parity |
| register vocabulary sprawl | Medium | Medium | reviewed small set |
| unused authored lines | High | Medium | 200-day utilization report |
| localization debt | Medium | High | text keys only |
| line contradicts state | Medium | High | contradiction soak |
| voice becomes pseudo-dialogue system | Medium | Medium | strict non-goals |

---

# 91. Commit Strategy

## C2[18].1 — baseline + VOICE_LINE_SPEC

## C2[18].2 — voice catalog schema + loader

## C2[18].3 — SurvivorVoiceSystem matching

## C2[18].4 — deterministic RNG/cooldowns

## C2[18].5 — identity/register/lexicon integration

## C2[18].6 — knowledge/age constraints

## C2[18].7 — integrity tier + first line slice

## C2[18].8 — 200-day utilization report

### Gate: 42A complete

## C2[18].9 — delivery DTO + port contract

## C2[18].10 — briefing/journal routing

## C2[18].11 — HUD/detail murmur routing

## C2[18].12 — flashback class + audio validation

## C2[18].13 — attention budgets + warning parity

## C2[18].14 — idempotency/session-swap safety

## C2[18].15 — snapshots/accessibility

### Gate: 42B complete

## C2[18].16 — speech object taxonomy + causeId

## C2[18].17 — grievance/friction families

## C2[18].18 — player policy reactions

## C2[18].19 — leader/second-hand/rumor delivery

## C2[18].20 — contradiction guard + family cooldown

## C2[18].21 — long-soak voice consistency

### Gate: 42C complete

## C2[18].22 — Wave-6 survivor voice closure

---

# 92. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical:

```text
ashfall-narrative-check
ashfall-write review
200-day authored-line utilization soak
same-seed voice-sequence replay
voice contradiction soak
port-contract validation
snapshot diff for briefing/detail/HUD
```

---

# 93. Flagship Definition of Done

## Prerequisites

- [ ] 40A authored identity live,
- [ ] 31A event kinds/causeId live,
- [ ] 36A delivery-port gate available,
- [ ] 25A/25C localization layer live.

## 42A

- [ ] line schema documented,
- [ ] first vertical slice authored,
- [ ] loader live,
- [ ] SurvivorVoiceSystem live,
- [ ] deterministic candidate selection,
- [ ] dedicated RNG discipline,
- [ ] register/lexicon mapping,
- [ ] cooldown/no-repeat,
- [ ] knowledge constraints,
- [ ] age-class rules,
- [ ] localization keys only,
- [ ] integrity tier,
- [ ] survivor register coverage,
- [ ] unused-line utilization report.

## 42B

- [ ] delivery port bound,
- [ ] no silent drop,
- [ ] murmur/event classes,
- [ ] no interruption chatter,
- [ ] journal archive,
- [ ] briefing route,
- [ ] HUD/detail route,
- [ ] radio separation,
- [ ] flashback route,
- [ ] surface/speaker budgets,
- [ ] warning parity,
- [ ] load/rebind idempotency,
- [ ] existing cue validation,
- [ ] accessibility,
- [ ] snapshots.

## 42C

- [ ] world/people/player-decision subjects,
- [ ] causeId propagation,
- [ ] grievance sentences,
- [ ] friction paired speech,
- [ ] policy praise/blame,
- [ ] second-hand stage,
- [ ] leader register,
- [ ] rumor delivery only from known facts,
- [ ] no voice-only mechanics,
- [ ] contradiction guard,
- [ ] family cooldown,
- [ ] long-soak state consistency.

## Global

- [ ] no dialogue engine,
- [ ] no new voice panel,
- [ ] no hardcoded survivor prose,
- [ ] no omniscient speech,
- [ ] no line as sole warning,
- [ ] no unbound voice sink,
- [ ] deterministic save/load sequence,
- [ ] full verification green.

---

# 94. Closure Report Template

```markdown
## C2[18] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Survivor count:
- Existing radio broadcasts:
- Roster bark definitions:
- Semantic event kinds:
- Voice line count before:
- Voice sinks:
- Localization gate:

### 42A — Line Bank
- Schema:
- Registers:
- Lexicon tags:
- Vertical-slice line count:
- Trigger coverage:
- Survivor register coverage:
- Determinism digest:
- Cooldown:
- Knowledge blocks:
- Integrity:
- 200-day eligible lines:
- 200-day selected lines:
- 200-day unused lines:
- Result:

### 42B — Delivery
- Port contract:
- Bound sinks:
- Missing sinks:
- Journal:
- Briefing:
- HUD:
- Detail panel:
- Flashbacks:
- Density budgets:
- Warning parity:
- Rebind duplicate count:
- Audio cue validation:
- Accessibility:
- Result:

### 42C — Social Speech
- World speech:
- People speech:
- Player-decision speech:
- Grievance:
- Friction:
- Policy reactions:
- Second-hand ordering:
- Leader register:
- Rumour knowledge:
- Contradiction failures:
- Soak results:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Audio:
- Content utilization:
- Narrative check:
- Voice utilization soak:
- Voice sequence replay:
- Verify fast:

### Final Metrics
- VOICE_LINES_AUTHORED:
- VOICE_LINES_ELIGIBLE:
- VOICE_LINES_SELECTED:
- VOICE_LINES_DELIVERED:
- VOICE_LINES_UNUSED:
- VOICE_DELIVERY_MISSING:
- DUPLICATE_DELIVERIES:
- KNOWLEDGE_VIOLATIONS:
- CONTRADICTION_VIOLATIONS:

### Remaining Debt
- Registers:
- Content families:
- Memory integration:
- Leadership:
- Rumour network:
- UI:
```

---

# 95. Final Execution Directive

Execute Plan 42 as a deterministic survivor-voice projection layer.

The critical sequence is:

```text
authored identity first
→ semantic trigger vocabulary
→ schema before content
→ deterministic line matching/selection
→ knowledge filtering
→ delivery ports
→ existing surfaces
→ journal archive
→ social/policy speech
→ contradiction and utilization soak
```

Do not build a dialogue tree.

Do not add a chat panel.

Do not let a line invent facts or consequences.

Do not hardcode prose into host/UI code.

Do not author hundreds of lines before the first narrow slice proves selection, delivery, cooldown, localization, and observability.

The strongest selection rule is:

> **A survivor line is chosen from authored candidates using only canonical identity, canonical state, canonical knowledge, and deterministic RNG.**

The strongest delivery rule is:

> **Every selected line is delivered through a declared sink or fails validation; survivor speech is never silently discarded.**

The strongest narrative rule is:

> **What survivors say must be evidence of what happened, what they know, who they are, and what the player did — not ambient chatter detached from state.**

The flagship acceptance scenario is:

> **Run the same seeded campaign twice through a survivor death, ration conflict, and policy decision. The same eligible survivors must produce the same line IDs in the same order, every line must obey knowledge and cooldown rules, every delivered line must appear in the correct existing surface and journal archive, and no line may contradict the underlying state.**
