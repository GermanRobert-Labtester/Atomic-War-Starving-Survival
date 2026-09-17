# C1 — Flagship Integration Plan [24]: Per-NPC Memory & Relationship Depth

> **Output:** `C1_planintegration[24].md`
>
> **Source baseline:** Plan 147 — Per-NPC Memory & Relationship Depth
>
> **Primary mission:** give named NPCs persistent, personal memory of specific player actions so their dialogue, quest access, trade posture, encounter behavior, and long-horizon relationship history can diverge from faction-level standing without creating a second reputation, faction, rumor, quest, trade, or combat system.
>
> **Primary architectural rule:** per-NPC memory is a relationship/history layer, not a universal social simulation. It records personal facts and derives personal relationship posture; every downstream effect must be applied through an existing system authority.
>
> **Primary information rule:** memories may be private to an NPC, known to the shelter, shared through existing rumor/gossip channels, or become faction knowledge. These are different information states and must not collapse into global omniscience.
>
> **Mandatory execution order:** 147A authority audit + memory event contract → 147B personal relationship projection + dialogue/quest/trade adapters → 147C combat/encounter, gossip and forgiveness integration where real systems exist → 147D persistence, decay policy, UI, determinism, balance, reachability and CI.
>
> **Critical re-baseline rule:** before creating `NpcMemorySystem`, inspect `HoldfastNpcCatalog`, `FactionStanceEngine`, `VerdictNpcSystem`, `DoorEncounterSystem`, `CampaignConsequenceLedger`, Plan-131 rumor/intelligence, Plan-44 survivor pair history, Plan-139 combat→faction consequences, quest prerequisites, trade stance, and any current NPC-identity/arc systems. Reuse existing event/history storage wherever it already owns the fact.
>
> **Guardrails:** no second faction trust; no global “reputation” meter; no per-NPC combat AI state machine unless combat already supports allegiance/assist/hostility adapters; no gossip network if Plan 131 already owns information propagation; no one-way trust↔faction feedback loop that double counts the same action; no arbitrary 1%/day memory erosion by default; no RNG for deterministic memory recording; no permanent full-detail memory list without retention policy; no personal-memory state for procedurally anonymous NPCs unless stable identity exists; no quest unlock invented solely because a memory exists; no trade price arithmetic in the memory system; no attack-on-sight rule implemented inside memory code; no new tutorial engine.

---

# 0. Mission

ASHFALL already has named NPCs, faction trust, flag-gated availability, door encounters, quest logic, trade sessions, and campaign consequence history.

What it does not yet have is a durable answer to a simple character question:

> What does this specific person remember about what the player did to them?

The source baseline describes the current gap:

```text
PLAYER ACTION
   │
   ├── faction standing changes
   ├── world flag changes
   └── quest state changes
        │
        ▼
NPC returns later
        │
        └── reacts mostly to faction trust / flags
```

That produces characters who behave more like interfaces than people.

The intended target is:

```text
PLAYER ACTION / INCIDENT
        │
        ▼
PERSONAL MEMORY EVENT
        │
        ├── npc_id
        ├── source event
        ├── subject/target
        ├── action type
        ├── day
        ├── salience
        ├── valence
        ├── visibility
        └── lifecycle
        │
        ▼
NpcRelationshipMemory
        │
        ├── personal trust projection
        ├── grudge projection
        ├── favor/debt projection
        ├── salient remembered facts
        └── unresolved interaction hooks
        │
        ▼
EXISTING SYSTEMS
        │
        ├────────► dialogue predicates
        ├────────► quest prerequisites/branches
        ├────────► trade stance/discount adapters
        ├────────► encounter availability
        ├────────► combat allegiance only if real
        ├────────► faction standing only via explicit rule
        ├────────► rumor/intelligence if memory is shared
        ├────────► journal/history
        └────────► epilogue/recurring NPC arcs
```

The system should make two NPCs from the same faction respond differently because of what happened between the player and each individual.

It should not make every remembered action a new stat, event, or quest.

---

# 1. Source-Evidence Interpretation

## 1.1 Named NPC identity already exists

The source points to `HoldfastNpcCatalog.cs` with 10 NPCs.

The first requirement is therefore stable identity:
- canonical NPC ID;
- lifecycle;
- survival/availability.

Per-NPC memory is only meaningful if those IDs persist.

## 1.2 Faction trust already exists and must remain separate

`FactionStanceEngine` currently supplies faction-level trust.

Personal memory should not merely mirror that value.

The relationship model needs to distinguish:

```text
faction thinks about player
vs
NPC personally thinks about player
```

## 1.3 Existing flags are useful but too coarse

`VerdictNpcSystem` and `CampaignConsequenceLedger` may already record some one-off facts.

Memory should consume or reference those facts where appropriate rather than duplicating all flags as prose history.

## 1.4 `hasGrudgeAgainstLeader` proves concept but not persistence

The source notes `DoorEncounterSystem` has a grudge flag.

Plan 147 should either:
- source that flag from canonical NPC memory;
- or retire the duplicate if memory supersedes it.

## 1.5 Fixed daily decay is not automatically desirable

The source proposes 1% per day.

That creates strange results:
- betrayal forgotten in months;
- saved life nearly meaningless later.

A memory system should distinguish:
- salience decay;
- relationship recovery;
- unresolved grievance;
- permanent milestone memory.

## 1.6 RNG is not needed to record memory

The source proposes `ISeededRng` for triggers.

Recording:
- helped;
- refused;
- betrayed;
- saved life
should be deterministic if the source event is known.

RNG may later select among dialogue lines, but not decide whether the NPC remembers a direct event.

## 1.7 Gossip should reuse the information layer

If Plan 131 already implements rumor propagation:
- NPC memory sharing should produce rumor/intel records there.

Do not create a second gossip simulation.

---

# 2. Non-Negotiable NPC Memory Invariants

## INV-147.1 — Stable NPC identity required

Memory attaches only to canonical NPC IDs.

## INV-147.2 — One personal-memory owner

Per-NPC memory/history has one authority.

## INV-147.3 — Faction trust remains separate

Personal trust does not replace faction standing.

## INV-147.4 — No automatic double counting

The same event cannot:
- directly change faction trust;
- change personal trust;
- then feed personal trust back into faction trust
unless explicitly authored as a separate later consequence.

## INV-147.5 — Memory events are source-attributed

Every entry records:
- source event/action;
- day;
- target/subject where relevant.

## INV-147.6 — Direct memories are deterministic

No RNG to decide whether an NPC remembers something they directly experienced.

## INV-147.7 — Derived relationship state is bounded

Personal trust, grudge, favor/debt use explicit ranges.

## INV-147.8 — Memory detail is bounded

Long campaigns cannot accumulate unbounded full-fidelity entries.

## INV-147.9 — Important memories can persist

Not every memory decays to zero.

## INV-147.10 — Forgiveness is not deletion

A forgiven betrayal may remain remembered but no longer active as grievance.

## INV-147.11 — Dialogue queries memory; memory does not own dialogue

## INV-147.12 — Quest runtime owns quest state

Memory provides predicates.

## INV-147.13 — Trade system owns price and trade availability

Memory provides personal posture/modifier inputs.

## INV-147.14 — Combat system owns hostility/allegiance

Memory may contribute if a real seam exists.

## INV-147.15 — Gossip uses existing rumor/intelligence

No second propagation engine.

## INV-147.16 — UI never mutates memory by opening

## INV-147.17 — Save/load never replays memories

Restoring history cannot apply trust/grudge twice.

## INV-147.18 — Dead/retired NPCs keep historical memory only where epilogue/archive requires

No active daily processing for unavailable NPCs.

---

# 3. Definition of Done

Plan 147 closes only when:

- stable NPC identity/lifecycle is audited;
- current faction trust vs personal relationship ownership is documented;
- existing per-NPC flags/grudge state are reconciled;
- memory events have stable IDs and source attribution;
- one per-NPC memory authority exists only if current history systems cannot provide it;
- personal trust/grudge/favor are derived or updated exactly once;
- no direct-memory RNG exists;
- decay policy distinguishes salience, forgiveness and milestone persistence;
- memory retention is bounded;
- dialogue can condition on personal memory through canonical predicates;
- quest availability/branches can condition on memory without memory owning quest state;
- trade can consume personal relationship posture without memory owning prices;
- Verdict and door NPCs can query the same memory authority;
- combat hostility/aid integration occurs only if current combat/encounter APIs support it;
- faction standing is affected only through explicit authored bridge rules;
- rumor/gossip uses existing Plan-131 information rails;
- 30 authored memory-triggered dialogue templates exist only after predicate and localization seams are real;
- old saves receive empty/default per-NPC memory;
- save/load round trip is idempotent;
- dead NPC histories remain available for journal/epilogue where appropriate;
- no unbounded memory growth occurs in 400-year/long-run retention tests;
- deterministic replay passes;
- `--npc-memory-selftest` exists or equivalent;
- all NPC IDs and memory-template references validate;
- UI shows personal vs faction relationship distinctly;
- forgiveness/reconciliation is reachable if current dialogue/choice systems support it;
- personal memory does not become a global reputation substitute;
- no second gossip/faction/trade/combat/quest system is introduced.

---

# 4. Phase P0 — Forensic Authority & Identity Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
Holdfast NPC count
canonical NPC IDs
NPC lifecycle/death/availability state
FactionStanceEngine trust range/API
FactionBranchCoordinator standing API
VerdictNpcSystem flags
DoorEncounterSystem grudge flag
CampaignConsequenceLedger event/flag model
quest predicate APIs
dialogue predicate APIs
trade stance/price APIs
combat allegiance/hostility APIs
rumor/intelligence APIs
journal/history APIs
save sections
```

## P0.2 Build NPC relationship authority matrix

Create:

`docs/npc/NPC_RELATIONSHIP_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
personal/faction/global
memory role
status
```

Rows:
- NPC identity;
- NPC availability;
- personal trust;
- grudge;
- favor/debt;
- faction trust;
- faction standing;
- dialogue condition;
- quest condition;
- trade stance;
- hostility;
- rumor;
- historical event;
- epilogue memory.

## P0.3 Audit NPC stability

For every named NPC:
- stable ID;
- no runtime-generated replacement identity;
- save stability.

## P0.4 Audit `hasGrudgeAgainstLeader`

Decide:
- migrate into canonical memory;
- derive from memory;
- retain as unrelated encounter-specific state.

Document.

## P0.5 Audit Plan 44 pair-history overlap

Survivor-to-survivor history is not NPC-player memory.

But reuse:
- event/retention patterns;
- reason IDs;
- pair-history architecture
if applicable.

## P0.6 Audit Plan 131 rumor overlap

Determine:
- can memory be published as rumor?
- can NPC receive rumor knowledge?
- can information source/provenance be retained?

## P0.7 Audit Plan 139 combat consequences

Do not double-record combat memory if combat-faction bridge already emits canonical political incident.

Memory may reference that incident.

## P0.8 Baseline no-memory reproduction

Demonstrate:
- help NPC;
- reload/return later;
- no personal behavior difference
beyond faction/flags.

---

# TASK 147A — Personal Memory Event Contract & Relationship State

# 147A.0 Goal

Create a stable, bounded, deterministic representation of personal memory and relationship posture.

## 147A.1 Prefer existing history store if sufficient

Before adding `NpcMemorySystem`, inspect:
- consequence ledger;
- character history;
- relationship history.

If one can be extended cleanly:
- reuse.

## 147A.2 Suggested file

If new:

`Assets/Ashfall.Core/NPC/NpcMemorySystem.cs`

## 147A.3 Memory entry DTO

Use compact fields:

```text
id
npc_id
event_kind
source_event_id
subject_id
target_id
day
valence
salience
tags[]
resolved_state
visibility
```

Avoid free-form `playerAction` strings.

## 147A.4 Stable memory ID

Preferred:

```text
npc_id + source_event_id + event_kind
```

No GUID.

## 147A.5 Event kinds

Initial typed vocabulary:

```text
helped
refused
ignored
betrayed
saved_life
harm_to_ally
kept_promise
broke_promise
paid_reparation
showed_mercy
```

Only implement events with real producers.

## 147A.6 Do not create producers for taxonomy completeness

No event kind without a source seam.

## 147A.7 Valence

Use bounded:
- negative / neutral / positive;
or numeric contribution.

Do not duplicate trust delta if relationship projector can derive from rule.

## 147A.8 Salience

Represents how strongly memory remains behaviorally relevant.

Not identical to trust.

## 147A.9 Resolution state

Possible:

```text
active
forgiven
repaid
resolved
historical
```

## 147A.10 Visibility

Possible:

```text
private
shelter_known
shared
public
```

Use current information model.

## 147A.11 Relationship summary

Suggested:

```text
npc_id
personal_trust
grudge
favor_owed
last_interaction_day
salient_memory_ids[]
```

## 147A.12 Personal trust range

Use -100..100 only if consistent with project style.

Otherwise use current relation bands.

## 147A.13 Grudge range

Do not need a second numeric 0..100 if active negative memory/salience can derive it.

Audit before adding.

## 147A.14 Favor/debt

Similarly:
- consider discrete owed-favor records instead of a 0..100 meter.

Recommended:
- bounded explicit favor tokens/records if existing commitment/debt systems support.

## 147A.15 Avoid three redundant meters

Trust + grudge + favor can become overlapping truth.

ADR required:

`ADR_NPC_PERSONAL_RELATIONSHIP_STATE.md`

## 147A.16 Recommended model

Potential:

```text
personal_trust: -100..100
active_grievances: memory refs
owed_favors: explicit memory/debt refs
```

No separate grudge/favor numeric meter unless evidence requires.

## 147A.17 Memory trigger rules

Move to data:

`npc_memory_rules.json`

## 147A.18 Rule fields

Suggested:

```text
event_kind
trust_delta
salience
creates_grievance
creates_favor
decay_policy
forgiveness_policy
tags
```

## 147A.19 Direct event determinism

Rule application:
- pure;
- no RNG.

## 147A.20 Exactly-once trigger

Source event ID prevents duplicate memory.

## 147A.21 Daily processing

Only if decay/aging requires it.

Prefer:
- derive effective salience from day difference
rather than ticking every entry daily.

## 147A.22 No per-day mutation if derivable

Example:

```text
effective_salience =
  base_salience × decay(day - created_day)
```

This avoids daily writes.

## 147A.23 Decay classes

Suggested:

```text
ephemeral
normal
persistent
milestone
```

## 147A.24 Ephemeral

Small refusals/ignored requests.

## 147A.25 Normal

Moderate help/conflict.

## 147A.26 Persistent

Major betrayal, life saved, serious harm.

## 147A.27 Milestone

Campaign-defining relationship event.

Does not disappear automatically.

## 147A.28 No blanket 1% daily decay

Decay is authored by class.

## 147A.29 Trust recovery

Can occur through:
- positive later memories;
- forgiveness;
- fulfilled promises.

Not simply forgetting.

## 147A.30 Forgiveness

Changes:
- grievance state;
- active effect.

Does not delete historical entry.

## 147A.31 Compensation

Use existing:
- trade;
- commitment;
- item/resource transfer
if current dialogue choice can request it.

## 147A.32 No generic apology button

Forgiveness requires authored dialogue/action.

## 147A.33 CaptureState

Persist:
- compact memory entries;
- relationship summary only if non-derivable;
- resolution;
- favor/debt refs.

## 147A.34 RestoreState

No rule reapplication.

## 147A.35 Schema version

Old save:
- empty memory map.

## 147A.36 Retention

Use bounded history:
- salient unresolved full entries;
- resolved older entries compacted/archived.

## 147A.37 400-year compatibility

If campaign can simulate long spans:
- no unbounded daily entries.

## 147A.38 Dead NPC retention

Keep:
- major memory refs for epilogue/history.

Stop:
- active decay/query work unless referenced.

## 147A.39 Memory event API

Example:

```text
RecordMemory(NpcMemoryEvent evt)
```

## 147A.40 Query API

Example:

```text
GetRelationship(npcId)
GetSalientMemories(npcId)
HasMemory(npcId, tag/eventKind)
```

## 147A.41 No mutable collection leak

Return read-only projections.

## 147A.42 Diagnostics

Trace:
- source event;
- applied rule;
- trust change;
- active grievance/favor.

## 147A.43 Unit tests

- help;
- refuse;
- betray;
- save life;
- duplicate source;
- forgiveness;
- decay class;
- old save;
- dead NPC retention.

## 147A.44 Generated matrix

Create:

`docs/npc/NPC_MEMORY_RULE_MATRIX.md`

### 147A DoD

Named NPCs can accumulate stable, source-attributed personal memories and relationship posture without duplicating faction trust or creating redundant meters.

---

# TASK 147B — Dialogue, Quest & Trade Reactions

# 147B.0 Goal

Make existing NPC-facing systems read personal memory as a condition, not a new runtime owner.

---

# 147B-D — Dialogue

## 147B.D1 Dialogue authority audit

Identify:
- dialogue fragments;
- trust requirements;
- condition predicates.

## 147B.D2 Personal-memory predicate

Add generic conditions if missing:

```text
npc_personal_trust_at_least
npc_has_memory
npc_has_active_grievance
npc_owes_favor
```

## 147B.D3 No dialogue logic in memory system

Memory returns facts.

Dialogue selects text.

## 147B.D4 Warm greeting

Only authored template.

No generic string concatenation.

## 147B.D5 Cold greeting

Same.

## 147B.D6 Secret sharing

Requires:
- authored dialogue;
- knowledge/quest consequence.

High trust alone does not invent secrets.

## 147B.D7 Threats/refusal

Use dialogue/availability system.

No memory-owned hostility transition unless current system maps it.

## 147B.D8 Past-action references

Template has:
- memory tag/event kind;
- localization key;
- optional min salience.

## 147B.D9 Selection priority

Specific salient memory can outrank generic faction line.

## 147B.D10 No repetitive recall

Cooldown/recent-selected IDs may live in dialogue presentation history if existing.

Do not decay memory solely to prevent repeated dialogue.

## 147B.D11 30 templates

Author only after:
- condition schema;
- localization;
- NPC IDs
are stable.

## 147B.D12 Distribution

At minimum cover:
- positive;
- negative;
- favor;
- reconciliation;
- milestone.

## 147B.D13 No 30 near-duplicates

Semantic variation.

## 147B.D14 Dialogue reachability

Synthetic relationship states prove templates selectable.

---

# 147B-Q — Quest Gating

## 147B.Q1 Quest authority remains canonical

Memory exposes predicates.

## 147B.Q2 Trust gate

Quest can require:
- personal trust threshold.

## 147B.Q3 Grievance block

Quest can block/branch on active grievance.

## 147B.Q4 Favor hook

Quest may become eligible because NPC owes favor.

Only if quest exists.

## 147B.Q5 Second chance

Forgiveness/reconciliation can satisfy branch predicate.

## 147B.Q6 No dynamic quest invention

Memory cannot generate new quest data.

## 147B.Q7 Assignment vs availability

If quest belongs to NPC, memory may affect availability.

If generic faction quest:
- personal memory should not globally hide it.

## 147B.Q8 Faction vs personal trust

Quest schema must clearly state which one is used.

## 147B.Q9 Mixed predicate

Possible:
- faction trust >= X;
- personal trust >= Y.

No implicit conversion.

## 147B.Q10 Quest history

Started quest remains quest-owned even if relationship later worsens unless authored fail condition.

## 147B.Q11 Reachability

No permanent critical-path softlock from one grievance unless alternate exists.

---

# 147B-T — Trade

## 147B.T1 Trade authority audit

Find current:
- price calculation;
- refusal;
- stance.

## 147B.T2 Personal trade posture

Memory may provide:

```text
personal_price_modifier
trade_refusal_reason
special_deal_eligibility
```

as inputs.

## 147B.T3 No price arithmetic in memory system

Final pricing stays trade/economy authority.

## 147B.T4 High trust discount

Source proposes 10%.

Treat as candidate data.

Use bounded authored modifier.

## 147B.T5 Grudge premium

Candidate 20%.

Balance test.

## 147B.T6 Favor special deal

One-time deal must have:
- consumption/idempotence;
- trade system ownership.

## 147B.T7 Betrayal blacklist

Only if trader NPC has authority to refuse.

Do not globally embargo faction.

## 147B.T8 Faction trade policy precedence

Personal discount cannot override:
- faction embargo;
- unavailable goods;
- market rules.

## 147B.T9 Modifier composition

Document order:

```text
base market price
× faction stance
× scarcity
× personal NPC modifier
→ clamp
```

Use actual current order.

## 147B.T10 Anti-arbitrage

Personal discounts cannot generate trivial buy-sell loop.

## 147B.T11 Trade tests

- neutral;
- trust discount;
- grudge premium;
- embargo precedence;
- one-time favor;
- save/load.

---

# 147B-V — Verdict & Door NPCs

## 147B.V1 Verdict NPCs

Use personal memory predicates in existing availability/dialogue path.

## 147B.V2 Door encounters

Replace/derive `hasGrudgeAgainstLeader` if appropriate.

## 147B.V3 No duplicate flags

If persistent memory owns grievance:
- encounter flag becomes projection.

## 147B.V4 One NPC identity

Door NPC and holdfast NPC must resolve same canonical ID if same character.

## 147B.V5 Unknown/procedural NPC

No persistent memory unless stable ID exists.

### 147B DoD

Dialogue, quests, trade, Verdict NPCs and door encounters can react to personal history through their existing authorities.

---

# TASK 147C — Combat, Forgiveness, Gossip & Cross-System Consequences

# 147C.0 Goal

Integrate only those higher-order personal reactions that already have real systems.

---

# 147C-C — Combat Behavior

## 147C.C1 Combat authority audit

Determine whether NPC combat actors can:
- switch allegiance;
- assist;
- become hostile
from pre-combat state.

## 147C.C2 If no seam

DEFER all combat-memory behavior.

Do not rewrite TacticalCombatSystem.

## 147C.C3 Hostility input

If seam exists:
- personal grievance can contribute to encounter hostility.

## 147C.C4 Faction precedence

An allied faction NPC should not attack solely because personal trust is mildly negative.

Use thresholds/policy.

## 147C.C5 Betrayal

Major active grievance may permit hostility only if:
- authored encounter;
- current faction/AI rules allow.

## 147C.C6 Aid

High trust may enable authored aid encounter.

Do not spawn helper magically.

## 147C.C7 No memory-owned AI state

Memory provides condition.

Encounter/combat owns behavior.

## 147C.C8 Plan 139 interaction

Combat against/with NPC:
- Plan 139 handles faction consequence;
- Plan 147 handles personal memory if NPC survives/learns.

## 147C.C9 Same incident source ID

Avoid duplicate history.

---

# 147C-F — Forgiveness & Reconciliation

## 147C.F1 Forgiveness authority

Use dialogue/choice/effect resolver.

## 147C.F2 Apology

Authored choice.

## 147C.F3 Compensation

Existing inventory/trade/commitment effect.

## 147C.F4 Reconciliation outcome

Memory event marks grievance:
- forgiven/resolved.

## 147C.F5 Trust does not instantly reset to max

Forgiveness reduces active hostility, not historical truth.

## 147C.F6 Failed apology

May:
- leave grievance unchanged;
- worsen trust
only if authored.

## 147C.F7 Reconciliation quest

Only if canonical quest exists.

## 147C.F8 Second chance

Can reopen specific quest/dialogue branch.

---

# 147C-G — Gossip / Memory Sharing

## 147C.G1 Use Plan 131

If rumor/intelligence system exists:
- publish a rumor payload.

## 147C.G2 Memory remains personal source

Shared information does not clone the exact same emotional memory into another NPC.

## 147C.G3 Distinguish fact from attitude

NPC A says:
- "player betrayed me."

NPC B may learn:
- "A claims the player betrayed them."

B's trust change depends on:
- B's relationship;
- faction;
- rumor credibility.

Use existing rumor/faction logic.

## 147C.G4 No global personal-memory replication

## 147C.G5 Reputation precedes player

This belongs to rumor/faction/public reputation, not per-NPC memory.

Plan 147 only emits shareable facts.

## 147C.G6 Gossip visibility

Track source/provenance in information system.

## 147C.G7 No per-day all-NPC gossip loop

Use event-driven propagation.

---

# 147C-FS — Personal → Faction Effects

## 147C.FS1 Default: no automatic coupling

Personal trust should not continuously modify faction trust.

## 147C.FS2 Explicit political event

A named influential NPC may:
- advocate;
- complain;
- report
through authored event.

Then faction authority changes.

## 147C.FS3 One-time source ID

No feedback loop.

## 147C.FS4 Influence weighting

Only if NPC/faction role data exists.

## 147C.FS5 Ordinary NPC

Personal relationship remains personal.

---

# 147C-E — Memory Events & Quest Hooks

## 147C.E1 Source examples

- The Grudge;
- The Favor;
- The Reconciliation;
- The Memory;
- The Test;
- Old Debts;
- The Apology;
- Test of Loyalty;
- Shared Enemy.

## 147C.E2 Treat as content backlog

Architecture does not require all nine.

## 147C.E3 Author after rails

Use:
- quest;
- dialogue;
- choice;
- event systems.

## 147C.E4 No event-specific logic in memory system

## 147C.E5 Minimum representative set

Implement 4–6 scenarios:
- positive recall;
- active grievance;
- favor call-in;
- reconciliation;
- shared-history quest;
- rumor spread if supported.

## 147C.E6 Reachability

Every scenario:
- triggerable;
- completable;
- no hidden impossible predicate.

### 147C DoD

Personal memory can influence combat posture, reconciliation, public information, and faction politics only through systems that already own those behaviors.

---

# TASK 147D — UI, Persistence, Determinism, Retention, Balance & CI

# 147D.0 Goal

Make personal relationships understandable, save-safe, bounded and durable over long campaigns.

## 147D.1 NPC detail panel

Show distinct:

```text
Personal relationship
Faction relationship
```

Do not merge.

## 147D.2 Personal relationship summary

Prefer:
- band;
- trust numeric if project style uses;
- active grievance;
- favor owed;
- last significant memory.

## 147D.3 Memory history

Show significant events, not every micro-action.

## 147D.4 Timeline

Entries:
- day;
- concise event;
- outcome/resolution.

## 147D.5 Grudge indicator

If grievance active:
- text + icon.

## 147D.6 Favor indicator

If owed:
- explicit.

## 147D.7 Faction comparison

Example:

```text
Personal: Warm
Faction: Suspicious
```

This is the value of the system.

## 147D.8 Hidden memories

Do not show NPC internal memory if player would not reasonably know.

Default UI shows:
- interaction history player directly participated in;
- known relationship effects.

## 147D.9 Secret NPC knowledge

Not exposed.

## 147D.10 Tooltips

Compact summary only.

## 147D.11 Journal

Log only significant relationship milestones.

## 147D.12 No automatic full-memory dump

## 147D.13 Tutorial

Only if current tutorial framework.

## 147D.14 Save state

Persist only canonical personal memory state.

## 147D.15 Old save

All known NPCs:
- neutral/no personal memory;
or
- derive limited bootstrap from explicitly compatible existing flags only if safe.

Default:
- empty.

## 147D.16 No retroactive guessing

Do not invent history from faction trust.

## 147D.17 Save round trip

Cases:
- positive trust;
- active grievance;
- favor owed;
- forgiven memory;
- dead NPC;
- shared rumor provenance if memory owns any ref.

## 147D.18 Reload idempotence

No:
- trust reapply;
- favor duplication;
- reconciliation rerun.

## 147D.19 Determinism

Direct event application pure.

## 147D.20 Dialogue RNG

Dialogue selection may use seeded RNG per existing narrative system.

Memory state itself does not.

## 147D.21 Decay determinism

Derived from:
- day delta;
- decay class.

## 147D.22 Retention cap

Per NPC:
- bounded active salient entries;
- resolved historical rollups.

## 147D.23 Suggested policy

Example:
- active unresolved: full;
- top N salient resolved: full;
- older resolved: compact summary count/category.

Use Plan 39B/41 retention conventions.

## 147D.24 400-year soak

If campaign supports:
- memory state size bounded.

## 147D.25 NPC death

Memory:
- no active behavior;
- remains historical for epilogue/archive.

## 147D.26 NPC unavailable/left

No daily work.

If returns:
- memory intact.

## 147D.27 New NPC

Neutral personal state.

## 147D.28 Max trust

Bound.

## 147D.29 Max grievance

Bound.

## 147D.30 Forgiveness accessibility

If active grudge can block meaningful content:
- at least one authored repair path where design intends reconciliation.

## 147D.31 No mandatory forgiveness

Some betrayals may remain permanent.

Document.

## 147D.32 Punitive-memory balance

30-day scenario:
- several refusals;
- one betrayal;
- positive repair.

Player must understand recovery route.

## 147D.33 180-day scenario

Track:
- relationship diversity;
- grievance count;
- favor count;
- dialogue variation;
- quest access.

## 147D.34 Anti-farming

Repeated cheap help cannot max trust/favor infinitely.

## 147D.35 Source-event uniqueness

Same quest reward/event:
- one memory.

## 147D.36 Favor farming

One action cannot create repeated favors by reload.

## 147D.37 Trade exploit

Personal discount cannot enable infinite arbitrage.

## 147D.38 Faction feedback exploit

Personal→faction event cannot recursively increase personal trust.

## 147D.39 Content reachability

30 dialogue templates:
- selectable across synthetic states.

## 147D.40 Memory-event reachability

Representative source actions actually fire.

## 147D.41 Performance

Queries:
- O(active salient memories / indexed summary);
- no scan of all campaign events.

## 147D.42 No per-frame updates

## 147D.43 Headless

Full logic works without UI.

## 147D.44 Data integrity

Validate:
- NPC IDs;
- event kinds;
- dialogue keys;
- quest IDs if directly referenced;
- rule bounds;
- decay classes.

## 147D.45 Selftest

Create:

```text
--npc-memory-selftest
```

## 147D.46 Selftest cases

At least:
1. no memories;
2. help;
3. refusal;
4. betrayal;
5. saved life;
6. duplicate source;
7. forgiveness;
8. dialogue predicate;
9. quest predicate;
10. trade posture;
11. rumor sharing if supported;
12. old save;
13. dead NPC history;
14. long-retention compaction.

## 147D.47 Source-scan authority gate

Detect:
- price arithmetic;
- quest mutation;
- faction trust loop;
- TacticalCombat AI state;
- second rumor loop
inside NPC memory namespace.

## 147D.48 Generated docs

Create:
- `NPC_MEMORY_ARCHITECTURE.md`;
- `NPC_MEMORY_RULE_MATRIX.md`;
- `NPC_MEMORY_REACTION_DISPOSITION.md`;
- `ADR_NPC_PERSONAL_RELATIONSHIP_STATE.md`.

### 147D DoD

Personal relationship memory remains distinct from faction trust, persists without duplication, stays bounded over long campaigns, and produces visible NPC-specific behavior through existing systems.

---

# 5. Personal Memory vs Faction Trust Contract

This distinction must be explicit:

```text
FACTION TRUST
"What does the organization think of you?"

PERSONAL TRUST
"What does this person think of you?"
```

They can disagree.

That disagreement is feature value, not inconsistency.

---

# 6. Recommended Relationship State Model

Avoid:

```text
personalTrust
grudgeLevel
favorOwed
memoryIntensity
reputation
```

all as overlapping meters.

Preferred:

```text
personal_trust
active_grievance_refs[]
owed_favor_refs[]
salient_memories[]
```

This keeps emotional facts explainable.

---

# 7. Memory Entry Contract

Every memory entry answers:

```text
what happened?
who experienced it?
who/what was targeted?
when?
how salient?
is it unresolved?
who knows?
what source event proves it?
```

---

# 8. Memory Rule Contract

Rules are data-driven.

Example conceptual:

```text
helped
  trust +10
  normal salience

refused
  trust -8
  ephemeral/normal

betrayed
  trust -30
  persistent grievance

saved_life
  trust +35
  persistent favor
```

Values are tuning defaults, not immutable source truth.

---

# 9. Decay Contract

Decay affects:
- behavioral salience.

It does **not** erase:
- historical fact.

Example:

```text
refusal
→ salience fades

betrayal
→ grievance remains until forgiveness/resolution

saved life
→ milestone remains referenceable
```

---

# 10. Forgiveness Contract

Forgiveness changes:

```text
active grievance
→ resolved/forgiven
```

It may also adjust trust.

It does not delete memory.

---

# 11. Favor Contract

A favor is best treated as:
- explicit owed obligation;
- one-time/limited;
- consumable by existing quest/trade/dialogue action.

Not a free-floating 0–100 bank.

---

# 12. Dialogue Contract

Dialogue selects based on:
- personal trust;
- active grievance;
- favor;
- salient memory;
- faction trust;
- current quest/world state.

Memory does not select final line itself unless existing dialogue engine requests eligible definitions.

---

# 13. Quest Contract

Memory provides predicates.

Quest runtime owns:
- available;
- active;
- complete;
- failed.

---

# 14. Trade Contract

Memory provides personal stance input.

Trade owns:
- price;
- stock;
- embargo;
- transaction.

---

# 15. Combat Contract

Memory may provide:
- personal hostility/aid eligibility.

Combat/encounter system owns:
- side;
- target selection;
- attack behavior.

If no seam exists:
- defer.

---

# 16. Rumor/Gossip Contract

Personal memory fact may be published:

```text
source NPC
→ rumor/intelligence
→ recipient knowledge
```

Recipient does not automatically inherit source NPC's emotional valence.

---

# 17. Personal→Faction Contract

Default:
- none.

Explicit authored influential-NPC events may translate personal memory into faction politics exactly once.

---

# 18. Memory Visibility Contract

Three different things:

```text
memory exists
player knows NPC remembers
third parties know the event
```

Do not conflate.

---

# 19. Retention Contract

Long-lived campaign:

```text
unresolved salient memories = full
recent resolved = full
old resolved = compact
milestones = retained
```

No unlimited list.

---

# 20. Persistence Matrix

| Fact | Owner |
|---|---|
| personal memory | NPC memory |
| personal trust | NPC memory |
| active grievance | NPC memory |
| favor/debt | NPC memory or commitment if externalized |
| faction trust | faction |
| rumor knowledge | rumor/intel |
| quest state | quest |
| trade state | economy/trade |
| combat hostility state | encounter/combat |
| dialogue history | narrative if needed |
| journal history | journal |

---

# 21. Old-Save Migration

Default:

```text
npc_memory = {}
```

Do not derive personal relationships from:
- faction trust;
- global flags
unless a migration explicitly proves one-to-one personal meaning.

---

# 22. Failure Injection Matrix

## N147.1 Same help event delivered twice
Expected: one memory/trust change.

## N147.2 Betrayal forgotten automatically after fixed 100 days
Expected: persistent-memory policy test fails.

## N147.3 Forgiveness deletes historical memory
Expected: history test fails.

## N147.4 Personal trust continuously feeds faction trust
Expected: feedback-loop gate fails.

## N147.5 Trade price computed inside NpcMemorySystem
Expected: authority source-scan fails.

## N147.6 Memory marks NPC hostile inside TacticalCombatSystem directly
Expected: architecture gate fails.

## N147.7 NPC B gets identical emotional grievance when hearing NPC A's rumor
Expected: gossip semantics test fails.

## N147.8 Old save creates fake personal memories from faction trust
Expected: migration test fails.

## N147.9 400-year run stores millions of entries
Expected: retention gate fails.

## N147.10 UI shows secret memory the player could not know
Expected: visibility test fails.

## N147.11 One cheap help action repeatedly farms favors by reload
Expected: idempotence/anti-farm fails.

## N147.12 Unknown NPC ID in dialogue template
Expected: data-integrity fail.

---

# 23. Determinism Contract

Same:

```text
source memory event
+ NPC current relationship
+ rule data
+ campaign day
```

must yield same:

```text
memory ID
trust delta
grievance/favor state
effective salience
relationship summary
```

No RNG required.

---

# 24. Long-Horizon Metrics

Track:

```text
named NPCs with memories
memories per NPC
active grievances
forgiven grievances
favors owed/consumed
personal trust distribution
dialogue memory references
quests gated/unlocked by personal memory
trade modifiers applied
rumors published
memory state bytes
compacted history count
```

---

# 25. Balance Guardrails

Personal memory should create:
- continuity;
- replayability;
- repairable relationships.

It should not create:
- permanent lockout from one refusal;
- infinite discounts;
- unstoppable grievance cascades;
- unavoidable faction punishment.

---

# 26. UI Acceptance

## NPC Detail
Shows:
- personal relationship;
- faction relationship;
- known significant history;
- active grievance/favor.

## Dialogue
References relevant memory.

## Quest
Shows personal-memory prerequisite where appropriate.

## Trade
Shows personal relationship reason for price/refusal if player can know.

---

# 27. Accessibility

- no color-only trust/grudge;
- timeline keyboard navigable;
- text scaling;
- clear distinction between personal and faction relationship.

---

# 28. Content Acceptance

Memory dialogue/scenario data:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
```

Gameplay quest/trade effects reach:
`EFFECT_PRODUCED`.

---

# 29. Reachability Contract

For every template:

```text
source memory can occur
NPC can be revisited
condition can be true
dialogue/quest/trade surface can select it
```

Unreachable templates:
- fix;
- archive.

---

# 30. Performance Guardrails

- indexed by NPC ID;
- no global scan on every dialogue;
- no daily mutation for derivable decay;
- compact summaries;
- bounded active set.

---

# 31. CI / Gate Set

Recommended:

```text
npc_memory_identity_integrity
npc_memory_rule_integrity
npc_memory_idempotence
npc_memory_no_faction_feedback_loop
npc_memory_dialogue_reachability
npc_memory_trade_authority
npc_memory_quest_authority
npc_memory_retention
npc_memory_determinism
npc_memory_save_matrix
npc_memory_ui_access
```

---

# 32. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --npc-memory-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 33. Recommended Commit Breakdown

```text
147A-1 authority/identity audit
147A-2 relationship-state ADR
147A-3 memory DTO / stable IDs
147A-4 rule catalog
147A-5 relationship projection
147A-6 decay/forgiveness/favor semantics
147A-7 save/retention
147A-8 generated matrix/selftest/docs

147B-1 dialogue predicates
147B-2 memory-triggered dialogue templates
147B-3 quest predicates
147B-4 trade posture adapter
147B-5 Verdict NPC integration
147B-6 DoorEncounter grudge migration
147B-7 reachability
147B-8 integration tests/docs

147C-1 combat/encounter seam audit
147C-2 combat behavior adapter if supported
147C-3 forgiveness/reconciliation choices
147C-4 rumor/gossip integration
147C-5 personal→faction explicit-event adapter
147C-6 representative memory events/quests
147C-7 Plan 139 source-ID integration
147C-8 cross-system idempotence

147D-1 NPC detail/history UI
147D-2 old-save/roundtrip
147D-3 deterministic decay/replay
147D-4 30-day relationship balance
147D-5 180-day relationship diversity
147D-6 400-year retention soak
147D-7 CI gates/failure fixtures
147D-8 final ship/no-ship report
```

---

# 34. Risk Register

## R147.1 Personal trust duplicates faction trust

Mitigation:
- explicit authority matrix;
- no automatic coupling.

## R147.2 Memory becomes punitive

Mitigation:
- salience classes;
- forgiveness/reconciliation;
- balance thresholds.

## R147.3 Memory history grows without bound

Mitigation:
- compaction;
- milestone retention;
- 400-year soak.

## R147.4 Gossip becomes second rumor system

Mitigation:
- Plan 131 only.

## R147.5 Trade discounts create arbitrage

Mitigation:
- trade-owned pricing;
- anti-arbitrage tests.

## R147.6 Combat behavior balloons scope

Mitigation:
- adapter only if seam exists;
- otherwise defer.

## R147.7 Old saves get fake histories

Mitigation:
- empty migration default.

## R147.8 Dialogue templates become repetitive

Mitigation:
- semantic diversity;
- selection cooldown/history.

---

# 35. Acceptance Checklist

## P0

- [ ] canonical NPC IDs audited
- [ ] NPC lifecycle audited
- [ ] HoldfastNpcCatalog audited
- [ ] FactionStanceEngine audited
- [ ] FactionBranchCoordinator audited
- [ ] VerdictNpcSystem audited
- [ ] DoorEncounter grudge audited
- [ ] CampaignConsequenceLedger audited
- [ ] Plan 44 overlap audited
- [ ] Plan 131 rumor overlap audited
- [ ] Plan 139 combat overlap audited
- [ ] quest predicates audited
- [ ] dialogue predicates audited
- [ ] trade pricing/stance audited
- [ ] combat allegiance seam audited
- [ ] save sections audited
- [ ] baseline no-memory behavior reproduced

## 147A

- [ ] existing history authority reused if sufficient
- [ ] NpcMemorySystem only if needed
- [ ] compact memory DTO
- [ ] stable deterministic memory ID
- [ ] typed event kinds
- [ ] no taxonomy-only producers
- [ ] valence semantics
- [ ] salience semantics
- [ ] resolution state
- [ ] visibility state
- [ ] relationship summary
- [ ] personal-trust range justified
- [ ] grudge meter avoided unless needed
- [ ] favor meter avoided unless needed
- [ ] relationship ADR
- [ ] memory rules in data
- [ ] direct events deterministic
- [ ] exactly-once source event
- [ ] decay derived where possible
- [ ] decay classes
- [ ] no blanket 1% decay
- [ ] trust recovery via later actions
- [ ] forgiveness preserves history
- [ ] compensation uses existing systems
- [ ] no generic apology button
- [ ] CaptureState
- [ ] RestoreState
- [ ] schema version
- [ ] bounded retention
- [ ] 400-year compatibility
- [ ] dead-NPC history policy
- [ ] RecordMemory API
- [ ] query APIs
- [ ] read-only projections
- [ ] diagnostics
- [ ] unit tests
- [ ] generated rule matrix

## 147B — Dialogue

- [ ] dialogue authority documented
- [ ] personal-memory predicates
- [ ] no dialogue selection ownership in memory system
- [ ] warm/cold/secret/threat templates authored
- [ ] past-action references
- [ ] specificity priority
- [ ] repetition control
- [ ] 30 templates only after rails
- [ ] semantic diversity
- [ ] dialogue reachability

## 147B — Quest

- [ ] quest authority canonical
- [ ] trust threshold predicate
- [ ] active grievance predicate
- [ ] favor predicate
- [ ] forgiveness predicate
- [ ] no dynamic quest invention
- [ ] personal vs faction trust explicit
- [ ] mixed predicates supported if needed
- [ ] started quest remains quest-owned
- [ ] no critical-path softlock

## 147B — Trade

- [ ] trade authority audited
- [ ] memory returns posture only
- [ ] no memory-owned price formula
- [ ] trust discount data-backed
- [ ] grudge premium data-backed
- [ ] one-time favor deal idempotent
- [ ] blacklist only at trader level if supported
- [ ] faction embargo precedence
- [ ] composition order documented
- [ ] anti-arbitrage
- [ ] trade tests

## 147B — Verdict/Door

- [ ] Verdict NPC queries memory
- [ ] DoorEncounter grudge migrated/derived if appropriate
- [ ] no duplicate grievance flags
- [ ] canonical NPC identity reused
- [ ] anonymous NPC memory disallowed without stable ID

## 147C

- [ ] combat behavior seam audited
- [ ] all combat memory behavior deferred if no seam
- [ ] hostility input only through encounter/combat API
- [ ] faction precedence
- [ ] betrayal behavior authored
- [ ] aid only through real encounter
- [ ] no memory-owned AI state
- [ ] Plan 139 source IDs reused
- [ ] forgiveness via dialogue/choice
- [ ] apology authored
- [ ] compensation canonical
- [ ] grievance resolved, not deleted
- [ ] forgiveness does not max trust
- [ ] failed apology authored
- [ ] reconciliation quest only if real
- [ ] Plan 131 rumor used
- [ ] fact vs attitude separated
- [ ] no emotional memory cloning
- [ ] no new public reputation system
- [ ] event-driven gossip
- [ ] no automatic personal→faction coupling
- [ ] influential-NPC event explicit
- [ ] source-ID loop protection
- [ ] influence only if role data exists
- [ ] event/quest ideas treated as backlog
- [ ] representative scenario set
- [ ] reachability

## 147D

- [ ] NPC detail personal/faction split
- [ ] relationship summary
- [ ] significant memory history
- [ ] timeline
- [ ] grievance indicator
- [ ] favor indicator
- [ ] faction comparison
- [ ] hidden memories not exposed
- [ ] tooltips
- [ ] bounded journal
- [ ] tutorial only if framework exists
- [ ] canonical state persisted
- [ ] old save empty default
- [ ] no retroactive faction-trust inference
- [ ] save round-trip matrix
- [ ] reload idempotence
- [ ] direct memory determinism
- [ ] dialogue RNG separate
- [ ] deterministic decay
- [ ] retention cap
- [ ] long-run retention policy
- [ ] 400-year soak
- [ ] dead NPC handling
- [ ] returning NPC retains memory
- [ ] new NPC neutral
- [ ] trust/grievance bounds
- [ ] forgiveness path where intended
- [ ] permanent betrayal allowed only intentionally
- [ ] 30-day punitive-memory balance
- [ ] 180-day diversity simulation
- [ ] anti-farming
- [ ] source-event uniqueness
- [ ] favor anti-reload
- [ ] trade exploit test
- [ ] faction feedback-loop test
- [ ] 30-template reachability
- [ ] source-event reachability
- [ ] performance bounded
- [ ] no per-frame update
- [ ] headless
- [ ] integrity
- [ ] npc-memory selftest
- [ ] source-scan authority gate
- [ ] generated docs

---

# 36. Ship / No-Ship Gate

**SHIP** only if:

```text
npc_personal_memory_authorities == 1
AND canonical_named_npc_ids_missing == 0
AND faction_trust_authorities == 1
AND personal_faction_trust_double_count_paths == 0
AND direct_memory_rng_usage == 0
AND duplicate_memory_source_event_application == 0
AND blanket_memory_decay_policy == false
AND forgiven_memories_deleted == false
AND unbounded_memory_history == false
AND memory_owned_dialogue_runtime == false
AND memory_owned_quest_state == false
AND memory_owned_trade_price_state == false
AND memory_owned_combat_ai_state == false
AND duplicate_gossip_systems == 0
AND personal_to_faction_feedback_loops == 0
AND old_save_fake_personal_history == false
AND npc_memory_save_roundtrip == pass
AND npc_memory_reload_idempotence == pass
AND npc_memory_determinism == pass
AND npc_memory_retention_400_year == pass
AND npc_memory_dialogue_reachability == pass
AND npc_memory_quest_reachability == pass
AND npc_memory_trade_integrity == pass
AND data_integrity_selftest == pass
AND npc_memory_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 37. Implementer Handoff

1. Audit stable NPC identity before designing memory.
2. Keep personal memory distinct from faction standing.
3. Reconcile `hasGrudgeAgainstLeader` and any existing personal flags before adding duplicate state.
4. Reuse campaign event IDs as memory provenance.
5. Use deterministic source-event IDs; never RNG to decide whether a direct action is remembered.
6. Prefer one trust value plus explicit grievance/favor records over three overlapping meters.
7. Use authored decay classes, not blanket 1% daily forgetting.
8. Make forgiveness resolve a grievance without deleting history.
9. Bound memory history with compaction/retention from the start.
10. Let dialogue query memory; do not put dialogue behavior inside the memory system.
11. Let quests query memory predicates; do not let memory own quest lifecycle.
12. Let trade consume a personal posture modifier; keep pricing in trade/economy.
13. Integrate combat behavior only if a real allegiance/hostility seam exists.
14. Reuse Plan 131 for gossip and public reputation spread.
15. Do not automatically feed personal trust into faction trust.
16. Use explicit influential-NPC events if personal relationships should become political.
17. Keep known player history separate from secret NPC internal state in UI.
18. Author the 30 dialogue templates only after predicates/localization/reachability rails exist.
19. Add old-save, dead-NPC, forgiveness, duplicate-event and anti-farming fixtures.
20. Run 30-, 180-, and 400-year retention/balance scenarios.
21. Close only when two NPCs from the same faction can credibly react differently to the player without creating a second social simulation.

---

# 38. Final Outcome

When this plan is complete, named NPCs stop behaving like faction-colored quest dispensers.

A person the player helped can remember that specific act. A person who was refused can become cooler without the entire faction suddenly hating the player. A major betrayal can remain a persistent grievance until the player actually repairs the relationship, while the historical fact still remains part of that character's story.

Personal and faction relationships can diverge:

```text
Personal: trusted
Faction: suspicious
```

or:

```text
Personal: resentful
Faction: allied
```

That difference gives individual characters identity.

Dialogue can reference real past actions through existing narrative predicates. Quests can require personal trust or become available after reconciliation. Trade can apply a bounded personal modifier while still respecting faction embargoes and market scarcity. If combat and encounter systems already support personal allegiance, a major grievance can influence those decisions without the memory layer becoming an AI controller.

Shared information also remains coherent. An NPC can tell others what happened through the existing rumor/intelligence system, but another NPC hearing the story does not magically inherit the source NPC's emotional memory. Public reputation and faction politics remain separate systems.

Memory stays durable without becoming an ever-growing log. Minor events fade in salience. Major betrayals and life-saving moments persist. Forgiveness changes whether a grievance is active, not whether history existed. Old resolved events can compact while milestones remain available for recurring arcs and epilogues.

Most importantly, the system remains explainable.

Every personal reaction can be traced to a real player action, a stable NPC identity, and a remembered event.

The result is a world where characters do not merely know what faction the player belongs with.

They remember what the player did to them.
