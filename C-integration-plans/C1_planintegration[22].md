# C1 — Flagship Integration Plan [22]: Combat → Faction Standing Bridge — Political Consequences for Violence

> **Output:** `C1_planintegration[22].md`
>
> **Source baseline:** Plan 139 — Combat → Faction Standing Bridge
>
> **Primary mission:** make faction-tagged combat politically meaningful by converting authoritative combat outcomes into exactly-once faction-standing/trust consequences, then letting existing diplomacy, economy, quest, bounty, embargo, rumor, and patrol systems react through their own authorities.
>
> **Primary architectural rule:** do not create a second reputation or diplomacy simulation. Combat records consequences; `FactionBranchCoordinator` / `FactionStanceEngine` / existing faction authorities remain owners of standing, trust, hostility, embargo, bounty, alliance, quest gating, patrol behavior, and diplomatic state.
>
> **Primary information rule:** who learns about a combat incident matters. Witnessed, reported, intercepted, concealed, and later-discovered incidents must use the project's existing information/rumor seams where available; the bridge must not grant every faction omniscient combat knowledge.
>
> **Mandatory execution order:** 139A authority audit + exactly-once combat consequence contract → 139B standing/trust application + context/witness handling → 139C faction reactions and diplomacy adapters → 139D UI/history, save/load, determinism, anti-farming, reachability and CI.
>
> **Critical scope rule:** the source plan proposes new bounty, envoy, alliance, reputation, deception, "faction killer" labels, war-crime-like morality, heroism, and tutorial/event systems. These are not automatically part of the bridge. Each may ship only if a canonical existing authority already provides the mechanic. Otherwise the plan records a follow-on rather than inventing a parallel subsystem.
>
> **Guardrails:** no second faction-standing ledger; no bridge-owned trust score; no new global reputation meter; no hidden `+/- standing` arithmetic in combat UI; no double-application to both standing and trust if those systems already compose; no standing change for factionless combatants; no incident farm through repeated encounter/reset; no random witness roll when actual witness/radio/location information exists; no wall-clock/GUID/unordered iteration; no consequence on reload; no fabricated faction reaction unsupported by current systems; no new “Combat Record” panel if faction detail + journal + encounter report can carry the information.

---

# 0. Mission

ASHFALL already knows which combatants belong to factions.

The source baseline identifies:
- `faction_id` on combatants in the combat catalog;
- a real tactical combat system;
- a faction standing summary;
- a per-faction trust/stance engine;
- expedition combat triggers.

But combat currently ends as an isolated tactical event:

```text
COMBAT
  │
  ├── shots fired
  ├── combatants killed/defeated
  └── encounter resolves
         │
         └── faction state unchanged
```

This means:
- killing a Garrison patrol does not anger the Garrison;
- helping rebels against a rival does not earn rebel favor;
- defending a faction convoy does not improve relations;
- routine violence has no political memory.

The target architecture is:

```text
TACTICAL COMBAT AUTHORITY
        │
        ▼
Combat Outcome Record
        │
        ├── encounter ID
        ├── combatants / faction IDs
        ├── kills / defeats / assists
        ├── context
        ├── self-defense / aggression
        ├── witnesses / information channels
        └── location / day
        │
        ▼
CombatFactionConsequence Projector
        │
        ├── determine affected factions
        ├── determine political context
        ├── determine information reach
        ├── calculate bounded consequence
        └── produce exactly-once consequence IDs
        │
        ▼
CANONICAL FACTION AUTHORITIES
        │
        ├────────► FactionBranchCoordinator / standing
        ├────────► FactionStanceEngine / trust, only if contract requires
        ├────────► Rumor / intelligence awareness
        ├────────► Diplomacy / protest
        ├────────► Bounty / enforcement
        ├────────► Economy / embargo
        ├────────► Quest availability
        ├────────► Patrol / encounter weighting
        └────────► Journal / epilogue history
```

The bridge should answer:

> What politically significant combat happened, which factions learned about it, what standing/trust change was justified, and which existing systems should now react?

It should not answer:
- how factions trade;
- how bounties work;
- how alliances work;
- how quests work;
- how patrols spawn;
- how diplomacy is simulated.

Those remain existing systems.

---

# 1. Source-Evidence Interpretation

## 1.1 Faction identity already exists in combat content

The source reports `faction_id` in combatant definitions.

Therefore the bridge begins from authoritative combatant identity.

No new faction-tagging layer.

## 1.2 Standing and trust may be overlapping truths

The source names both:
- `FactionBranchCoordinator.FactionStandingSummary`;
- `FactionStanceEngine` trust -100..100.

The highest-priority audit is to determine:
- whether standing and trust are the same concept;
- whether one derives from the other;
- whether both should change;
- whether one is historical/aggregate and one operational.

The bridge must not blindly apply the same delta twice.

## 1.3 Combat context matters more than kill count

The source proposes:
- patrol attack;
- raid assistance;
- rebellion support;
- self-defense;
- accidental/allied kill;
- crossfire.

A kill-only formula would be wrong.

The bridge must use encounter context where that context is already knowable.

## 1.4 Witness information should reuse existing information systems

The source suggests settlement/wilderness/radio/companions.

Plan 131 and related work likely introduced rumor/intelligence paths.

Witness/reputation spread should use those systems rather than a second “combat witness simulator.”

## 1.5 Reactions already belong elsewhere

Protest, bounty, embargo, alliance and quest blocking are downstream faction-system behaviors.

139 should emit the political fact and call existing APIs.

If a reaction does not exist:
- defer it.

## 1.6 Anti-farming should be incident identity first, cooldown second

A 7-day blanket cooldown is crude.

Exactly-once incident identity prevents duplicate application.

A cooldown may additionally limit repeated low-value farming encounters, but it must not suppress genuinely distinct politically important incidents.

---

# 2. Non-Negotiable Combat-Politics Invariants

## INV-139.1 — One canonical combat outcome

Combat outcome comes from `TacticalCombatSystem` / canonical combat resolution.

The bridge does not reconstruct kills from UI or logs.

## INV-139.2 — One consequence per incident/faction/reason

Stable consequence key prevents duplicate standing changes.

## INV-139.3 — No standing for factionless combat

Bandits/animals/unknown actors with no political faction produce no faction standing by default.

## INV-139.4 — Context precedes arithmetic

Self-defense, aggression, assistance, friendly fire and faction hostility must be identified before choosing the delta.

## INV-139.5 — Standing/trust ownership is singular

If trust and standing are linked:
- apply through one canonical mutation seam.

No double political penalty.

## INV-139.6 — Information reach gates reaction

A faction cannot react to an unwitnessed incident it has not learned about.

## INV-139.7 — Direct participants always know

The attacked/assisted faction obviously knows if its surviving forces/reporting chain can communicate according to existing mechanics.

Do not apply arbitrary “unwitnessed 0.5x” when nobody knows yet.

## INV-139.8 — Unknown incidents may become known later

Rumor/intercept/patrol discovery may trigger delayed political consequence if existing information system supports it.

## INV-139.9 — Combat bridge does not own diplomacy

It emits political consequences and requests canonical actions.

## INV-139.10 — Combat bridge does not own economy

Embargo/trade refusal belongs to faction/economy state.

## INV-139.11 — Combat bridge does not own quest gating

Quest availability consumes faction state.

## INV-139.12 — Combat bridge does not own encounter difficulty

Patrol/encounter systems consume hostility/standing.

## INV-139.13 — No hidden reputation meter

Labels like “faction killer” are derived from incident history if ever needed, not a new stat.

## INV-139.14 — Defensive violence is distinguishable

Self-defense may reduce or eliminate political penalty according to authored faction policy.

## INV-139.15 — Friendly-fire/allied violence is explicit

Accidental/hostile allied kills must not be treated like enemy kills.

## INV-139.16 — Consequence magnitude is bounded

No one routine incident should jump an entire political relationship unless explicitly authored as major.

## INV-139.17 — Combat consequences are explainable

UI/history can show:
- incident;
- faction;
- context;
- information source;
- standing/trust effect.

## INV-139.18 — Save/load cannot retrigger consequence

Restoring history never reapplies deltas.

---

# 3. Definition of Done

Plan 139 closes only when:

- combat outcome authority is audited;
- faction standing/trust ownership is documented;
- faction ID reference integrity is enforced;
- one combat-faction consequence projector/bridge exists only if no current event bridge already covers the seam;
- stable incident IDs and consequence IDs exist;
- the bridge receives canonical combat result exactly once;
- context classification distinguishes at least defensive, offensive, allied/friendly-fire, assistance and factionless cases where existing encounter data supports them;
- consequence rules live in data/config rather than scattered switches;
- standing/trust mutation goes through one canonical faction mutation seam;
- witnessed/reported/unknown information state is explicit;
- existing rumor/intelligence systems are reused for delayed awareness;
- factionless combat produces no political consequence;
- anti-farming prevents duplicate/reset abuse without suppressing real distinct incidents;
- expedition combat uses the same bridge;
- shelter defense combat uses the same bridge if it shares tactical resolution;
- protest/bounty/embargo/praise/alliance hooks are adapters into existing systems only;
- no new reputation meter is introduced;
- no new diplomacy system is introduced;
- no new bounty system is introduced if one already exists;
- no reaction is authored if the downstream system does not exist;
- recent faction-combat incidents are visible in faction detail/journal/encounter report;
- pre-combat UI can show combatant faction identity and likely political risk when knowable;
- old saves load with empty/no combat-faction history unless existing event history can derive it;
- capture/restore is versioned only for genuinely new orchestration/history state;
- consequence deltas do not reapply on restore;
- deterministic replay passes;
- 30/180-day simulations show no standing farming or hostility runaway from routine combat;
- all combat catalog faction IDs resolve;
- `--combat-faction-selftest` exists or equivalent;
- headless behavior passes;
- content/data integrity validates pair rules and references;
- all downstream effects are attributable in journal/day diagnostics;
- no duplicate standing/trust authority is introduced.

---

# 4. Phase P0 — Forensic Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
combat result DTO/event
combatant faction_id field
combat encounter context fields
kill/defeat/assist data
self-defense/aggressor data
location context
FactionBranchCoordinator mutation APIs
FactionStanceEngine mutation APIs
standing/trust read models
rumor/intelligence APIs
bounty APIs
embargo/trade APIs
quest gating APIs
patrol/encounter hostility APIs
journal/event history
save sections
```

## P0.2 Build political authority matrix

Create:

`docs/combat/COMBAT_FACTION_AUTHORITY_MATRIX.md`

Columns:

```text
fact
authority
read API
write API
persisted?
combat may write?
bridge role
status
```

Rows:
- combat incident;
- faction standing;
- faction trust;
- hostility;
- awareness;
- protest;
- bounty;
- embargo;
- alliance;
- quest availability;
- patrol aggression;
- journal history;
- epilogue history.

## P0.3 Resolve standing vs trust

Write an ADR:

`docs/architecture/ADR_FACTION_STANDING_VS_TRUST.md`

Answer:

```text
Are standing and trust aliases?
Does one derive from the other?
Does one aggregate branches?
Which mutation API is canonical?
Does combat change both indirectly?
```

No 139B work until this is explicit.

## P0.4 Audit existing semantic events

Look for:
- combat ended;
- faction attacked;
- faction assisted;
- friendly fire;
- shelter raid defended.

Reuse where possible.

## P0.5 Audit information layer

Confirm whether:
- witness;
- rumor;
- radio intercept;
- companion reporting
already has an authority.

Do not create a second information propagation system.

## P0.6 Baseline reproduction

Tests proving current behavior:

```text
kill faction combatant
→ faction standing unchanged

assist faction combatant
→ faction standing unchanged
```

Keep until fixed.

---

# TASK 139A — Combat Incident Contract & Exactly-Once Projection

# 139A.0 Goal

Create one stable political incident record from real combat resolution.

## 139A.1 Prefer existing combat result event

Do not modify tactical combat to know faction politics.

It should publish/return combat result.

## 139A.2 Outcome DTO

If existing result lacks required fields, extend minimally:

```text
combat_id
encounter_id
location_id
day
participants[]
  combatant_id
  faction_id
  side
  outcome
  killed_by
  assisted_by
aggressor_side
defender_side
context_tags
```

Only real data.

## 139A.3 Stable incident ID

Use existing combat/encounter identity.

If missing, derive deterministic:

```text
day + encounter_id + location + sequence from authoritative campaign event ID
```

No GUID.

## 139A.4 Political consequence DTO

Suggested:

```text
id
combat_id
faction_id
reason
context
standing_delta
trust_delta if separate/real
severity
information_state
source
day
applied
```

Do not add trust delta if ADR says standing mutation already handles trust.

## 139A.5 Reasons

Initial:

```text
attacked_faction
killed_faction_member
defeated_patrol
assisted_faction
defended_against_faction
friendly_fire
betrayed_faction
```

Only implement supported context.

## 139A.6 Severity

Use discrete:

```text
minor
moderate
major
```

Derived from authored rules.

## 139A.7 Rule catalog

Create/extend:

`combat_faction_consequences.json`

## 139A.8 Rule fields

Suggested:

```text
id
context
actor_relation
victim_relation
base_delta
severity
self_defense_multiplier
aggression_multiplier
information_requirement
cooldown_class
reaction_tags
```

## 139A.9 Faction-pair rules

Only if factions genuinely have asymmetric political expectations.

Default generic rules should work.

## 139A.10 No giant pair matrix by default

Avoid N×N faction tables unless repository already uses them.

## 139A.11 Self-defense semantics

If faction attacks player:
- penalty may be lower or zero;
- authored per faction policy if available.

## 139A.12 Offensive aggression

Player initiates against neutral/allied faction:
- higher penalty.

## 139A.13 Assistance

Player helps faction against enemy:
- positive effect only if the faction learns of assistance.

## 139A.14 Enemy-of-enemy reward

Do not automatically reward every enemy kill.

Require:
- political relationship;
- known assistance;
- authored rule.

## 139A.15 Friendly fire

Different from deliberate aggression if combat engine tracks accident/context.

If not trackable:
- do not pretend.

## 139A.16 Bandit confusion

Faction gear without faction identity:
- no standing effect.

Combatant `faction_id` is authority.

## 139A.17 Civilian crossfire

Only if civilian/faction relation system exists.

Do not invent war-crime subsystem.

## 139A.18 Outcome aggregation

Do not necessarily apply per kill.

Prefer per incident/faction summary.

This prevents:
- 10 kills = 10 stacked political changes
when one patrol engagement is one political incident.

## 139A.19 Count weighting

Rule may consider:
- number killed;
- leader killed;
- patrol destroyed.

Bounded.

## 139A.20 Consequence floor/ceiling

Per incident:
- configured min/max.

## 139A.21 Stable consequence ID

```text
combat_id + faction_id + rule_id
```

## 139A.22 Exactly-once registry

Track applied IDs.

## 139A.23 No blanket 7-day incident suppression

Use incident identity first.

## 139A.24 Optional farming cooldown

Only for repeated generated encounters of same class.

Data-driven and faction-specific.

## 139A.25 Cooldown does not erase major incidents

A second genuine major attack within 7 days still matters.

## 139A.26 Pure projection

Given:
- combat result;
- faction state;
- rule data;
- information state
returns consequences.

## 139A.27 No mutation during calculation

Mutation is later transaction.

## 139A.28 Missing faction

Skip with diagnostic.

## 139A.29 Unknown faction ID

Data-integrity failure if catalog-defined combatant references invalid faction.

## 139A.30 Headless

No UI dependency.

## 139A.31 Unit tests

Cases:
- factionless;
- hostile faction;
- neutral faction;
- allied faction;
- assistance;
- self-defense;
- aggressive attack;
- multiple kills;
- repeated event.

## 139A.32 Generated matrix

Create:

`docs/combat/COMBAT_FACTION_CONSEQUENCE_MATRIX.md`

### 139A DoD

Combat produces one deterministic, explainable political incident summary instead of scattered per-kill reputation math.

---

# TASK 139B — Standing/Trust Application & Information Reach

# 139B.0 Goal

Apply political consequences through one canonical faction mutation path and only when the relevant faction knows the incident.

## 139B.1 Canonical faction mutation

Use ADR result.

Preferred shape:

```text
ApplyStandingDelta(factionId, delta, reason, sourceId)
```

or existing equivalent.

## 139B.2 Trust mutation

Only separately if:
- trust is a different state with explicit contract.

No duplicate mirroring.

## 139B.3 Transaction order

1. verify consequence not applied;
2. verify information requirement;
3. clamp/validate;
4. apply canonical faction mutation;
5. mark consequence applied;
6. emit semantic event;
7. write history;
8. request downstream reactions.

## 139B.4 Mutation idempotence

Domain call should carry source ID if possible.

## 139B.5 Information states

Prefer existing rumor/intel terminology.

Minimal conceptual states:

```text
unknown
known_to_direct_participants
reported
rumored
confirmed
```

## 139B.6 Direct participant knowledge

If faction unit survives/communications exist:
- faction may know.

Use real combat/reporting state.

## 139B.7 Settlement witnesses

Only if:
- location is settlement;
- witness/authority says incident observable.

Do not infer all settlement combat is globally known if project models information asymmetry.

## 139B.8 Companion witnesses

Companions affect:
- shelter knowledge;
not automatically every faction's knowledge.

## 139B.9 Radio intercept

Reuse radio/rumor authority.

## 139B.10 Wilderness incident

Can remain unknown initially.

## 139B.11 Delayed discovery

Rumor/intelligence system may later notify bridge/faction.

## 139B.12 Consequence pending state

If incident occurred but faction doesn't know:
- political consequence record may remain pending.

## 139B.13 Physical vs reputational truth

Combat happened regardless of reputation.

Standing changes only when politically attributed/known.

## 139B.14 Attribution

Faction may know:
- player did it;
- unknown attackers;
- rival blamed.

Deception only if existing diplomacy/intelligence system supports.

## 139B.15 No fake witness RNG

Source proposes `ISeededRng`.

Use RNG only if actual rules call for probabilistic information leakage.

Prefer deterministic world-state witnesses.

## 139B.16 Seeded leak chance

If used:
- seeded;
- data-backed;
- traceable.

## 139B.17 Witness multiplier review

Source proposes 2× witnessed / 0.5× unwitnessed.

Replace with more coherent semantics:

```text
unknown → no standing yet
rumored → smaller/uncertain impact if existing system supports
confirmed → full impact
```

Do not automatically halve an incident nobody knows.

## 139B.18 Severity scaling

Information may scale confidence, not moral severity.

## 139B.19 Standing clamp

Respect canonical -100/+100 or current bounds.

## 139B.20 Faction pair hostility

Killing a faction enemy may improve standing only if:
- faction knows;
- current relations support;
- rule exists.

## 139B.21 Positive farming cap

Repeated assistance should not create infinite standing.

Use:
- diminishing rule;
- cooldown class;
- quest/event uniqueness.

## 139B.22 Negative hostility escalation

Repeated attacks can compound.

Bound:
- standing floor;
- reaction thresholds.

## 139B.23 Incident ledger

Persist compact history if not already handled by world event history:

```text
consequence_id
combat_id
faction
delta
reason
day
information source
```

## 139B.24 No giant full combat log duplication

Reference combat event ID.

## 139B.25 Expedition combat

Expedition uses same tactical result event.

## 139B.26 Shelter defense

If same combat system:
- same bridge.

If separate defense resolution:
- adapter emits same outcome DTO.

## 139B.27 No double expedition/defense application

One source event.

## 139B.28 Tests

- confirmed witness;
- unknown wilderness;
- delayed rumor;
- direct participants;
- self-defense;
- assistance;
- duplicate report;
- cooldown/farming.

### 139B DoD

Standing/trust changes are exactly-once, information-aware, bounded, and applied through one faction authority.

---

# TASK 139C — Downstream Faction Reactions, Diplomacy & World Effects

# 139C.0 Goal

Let existing faction systems react to combat consequences without moving their ownership into the bridge.

## 139C.1 Reaction inventory

Audit actual systems for:
- diplomatic protest;
- bounty;
- embargo;
- praise/reward;
- alliance;
- patrol hostility;
- trade refusal;
- quest gating.

## 139C.2 Reaction disposition table

Create:

`docs/combat/COMBAT_FACTION_REACTION_DISPOSITION.md`

Columns:

```text
reaction
existing authority
trigger API
status
implemented?
deferred?
```

## 139C.3 No new reaction engine by default

If faction authority already maps standing thresholds to reactions:
- bridge does nothing beyond standing change.

## 139C.4 Protest

If existing diplomacy/event system supports:
- trigger from major confirmed negative incident.

## 139C.5 Bounty

Use canonical enforcement/bounty system.

No bridge-owned hunter schedule.

## 139C.6 Embargo

Use economy/faction embargo authority.

No direct `tradeEnabled=false` in bridge.

## 139C.7 Praise/reward

If reward system exists:
- one-time reaction event.

No raw item spawning in bridge.

## 139C.8 Alliance

Alliance offer only through existing faction/treaty system and thresholds.

## 139C.9 Patrol hostility

Encounter/patrol systems consume standing/hostility.

Prefer passive derivation rather than explicit bridge mutation.

## 139C.10 Quest availability

Quest runtime consumes faction standing/prereqs.

No direct quest deletion if hostility changes.

## 139C.11 Trade options

Trade system consumes stance.

No bridge-specific shop filter.

## 139C.12 Reputation labels

Default:
- derive from incident history if UI wants summary.

Do not persist `faction_killer`.

## 139C.13 Diplomatic recovery

Source proposes:
- envoy;
- reparations;
- blame rival;
- accept responsibility.

Disposition individually.

## 139C.14 Reparations

If existing trade/commitment/diplomacy supports:
- create an action/commitment.

## 139C.15 Explanation/envoy

If dialogue/diplomacy route exists:
- reuse.

## 139C.16 Blame rival

Only if:
- deception/rumor attribution mechanic exists.

Otherwise DEFER.

## 139C.17 Accept responsibility

Can be dialogue/narrative if existing system supports.

No hidden morality penalty invented.

## 139C.18 Combat justification

Source proposes pre-combat declaration.

Do not create separate justification state unless:
- self-defense/hostility context already exists.

Prefer derive from:
- who initiated;
- standing;
- encounter type.

## 139C.19 Prior hostility

Hostile faction attacking player:
- lower/zero penalty by rule.

No manual “declare justified” click needed.

## 139C.20 MoralChoice integration

Do not automatically convert every faction kill into MoralChoiceSystem.

Only authored morally significant encounters should use choice system.

## 139C.21 Combat events

"The Patrol", "The Rescue", "The Mistake", etc. are content follow-ons.

Core integration should not require five new event stories.

## 139C.22 10 scenarios

Author 10 data-driven integration scenarios only after:
- rule schema;
- reaction seams;
- actual faction IDs
are verified.

## 139C.23 Scenario classes

Recommended:
1. defend against hostile patrol;
2. ambush neutral patrol;
3. assist ally convoy;
4. attack ally;
5. mixed faction firefight;
6. factionless bandits;
7. shelter defense;
8. expedition rescue;
9. repeated farming attempt;
10. delayed rumor.

## 139C.24 Scenario acceptance

Each proves:
- context;
- info reach;
- delta;
- reaction or deliberate no reaction.

## 139C.25 No reaction without system

A scenario cannot claim bounty/embargo if system absent.

## 139C.26 Downstream idempotence

Reaction request carries source consequence ID.

## 139C.27 Threshold transitions

If standing crosses:
- neutral→hostile;
- hostile→neutral;
emit canonical faction-state transition once.

## 139C.28 No repeated threshold event

Staying hostile does not emit hostility transition every incident.

## 139C.29 Commitment integration

Reparations/protests can use Plan-38 commitment system.

## 139C.30 Rumor integration

Political incident can become rumor.

Do not duplicate consequence after rumor; rumor changes awareness.

## 139C.31 Place memory

Major faction combat may become world-history/place scar through Plan 41.

## 139C.32 Epilogue/history

Use incident/standing history.

No separate combat-reputation save state.

### 139C DoD

Combat consequences can trigger the faction systems ASHFALL already has, while missing reaction mechanics remain explicit follow-ons rather than hidden new subsystems.

---

# TASK 139D — UI, Persistence, Determinism, Balance & CI

# 139D.0 Goal

Make combat politics visible, save-safe, deterministic and resistant to farming or accidental hostility.

## 139D.1 Pre-combat faction identity

Combatant tooltip/encounter preview shows:
- known faction;
- current stance/standing band;
- political risk indicator
if player knows identity.

## 139D.2 Unknown faction identity

Do not reveal hidden affiliation.

## 139D.3 Political risk preview

Examples:
- "Attacking this patrol may damage Garrison standing."
- "Self-defense: reduced political penalty."

Use rule projection.

## 139D.4 No exact hidden delta if context uncertain

Do not promise -17 before witness/attribution is known.

## 139D.5 Post-combat report

Show:
- involved factions;
- confirmed/pending political consequences;
- standing change;
- why.

## 139D.6 Faction detail

Recent politically significant combat incidents.

## 139D.7 Journal

Automatic record for major incidents.

Not every minor hit/kill.

## 139D.8 New Combat Record panel decision

Default:
- NO.

Only add if faction detail/journal cannot satisfy historical review.

## 139D.9 Tooltips

Faction marker on combatants.

Use known information only.

## 139D.10 Tutorial

Only if existing tutorial framework.

First faction-tagged combat can explain:
- political consequences.

## 139D.11 Accessibility

- text + icon;
- no red/green only;
- standing delta signed with label.

## 139D.12 Localization

Reason strings keyed.

## 139D.13 Save principle

If consequences are fully represented in:
- faction state + world event history,
avoid a separate persistent bridge ledger except applied-ID/idempotence metadata.

## 139D.14 Minimal new state

Persist only if needed:

```text
schema_version
processed_consequence_ids
pending_unknown_incidents
compact incident refs
cooldowns if real
```

## 139D.15 Old save

No state:
- empty processed/pending.

No retroactive faction penalties.

## 139D.16 Restore

No mutation during restore.

## 139D.17 Duplicate combat result after load

Still no double delta.

## 139D.18 Mid-awareness save

Incident unknown/rumored:
- reload;
- same pending state.

## 139D.19 Mid-reaction save

If protest/bounty pending:
- source system persists its own reaction.

Bridge doesn't duplicate.

## 139D.20 Determinism fingerprint

Replay:

```text
combat
→ context
→ information
→ faction consequence
→ reaction threshold
```

## 139D.21 RNG scan

No:
- wall clock;
- `Guid.NewGuid`;
- `Random.Shared`;
- unordered dictionary dependence.

## 139D.22 Headless

No UI.

## 139D.23 Catalog integrity

Every combatant faction ID resolves.

## 139D.24 Rule integrity

Every:
- faction ID;
- relation class;
- reaction tag;
- localization key
valid.

## 139D.25 30-day anti-farming simulation

Repeated encounters.

Track:
- standing delta from unique incidents;
- suppressed duplicate/farm incidents;
- hostility transitions.

## 139D.26 180-day faction landscape simulation

Track:
- hostile/allied factions;
- combat incidents;
- positive/negative deltas;
- recovery opportunities;
- embargo/bounty count if real.

## 139D.27 Accidental-hostility guard

Normal defensive play should not automatically make all factions hostile.

## 139D.28 Positive farming guard

Repeated farming of enemy patrols cannot max allies trivially.

## 139D.29 Penalty cap

Routine incident within moderate range.

Major deliberate betrayal can be stronger if authored.

## 139D.30 Recovery path

At least one real recovery route if diplomacy system supports:
- trade;
- reparations;
- quest;
- time/decay
according to faction design.

## 139D.31 No forced recovery feature

If faction hostility is intentionally lasting, document.

## 139D.32 Performance

Projection is incident-time only.

No per-frame work.

## 139D.33 Event budget

No standing event per individual bullet/kill.

Per political incident.

## 139D.34 Selftest

Create:

```text
--combat-faction-selftest
```

## 139D.35 Selftest cases

At least:
- factionless no-op;
- hostile self-defense;
- neutral aggression;
- allied assistance;
- friendly fire/allied attack;
- delayed rumor;
- duplicate event;
- save/load;
- expedition;
- shelter defense if supported.

## 139D.36 Failure fixtures

Each key gate proves it can fail.

## 139D.37 Source-scan authority gate

Detect:
- direct standing mutation in TacticalCombatSystem;
- duplicate trust mutation;
- bridge-owned trade/quest/bounty state.

## 139D.38 Docs

Create/update:
- `COMBAT_FACTION_ARCHITECTURE.md`;
- `COMBAT_FACTION_CONSEQUENCE_MATRIX.md`;
- `COMBAT_FACTION_REACTION_DISPOSITION.md`;
- `ADR_FACTION_STANDING_VS_TRUST.md`.

### 139D DoD

Combat politics is visible, deterministic, save-safe, anti-farm, and governed by the same faction authorities as the rest of the game.

---

# 5. Political Incident State Model

Conceptual:

```text
COMBAT RESOLVED
      │
      ▼
INCIDENT RECORDED
      │
      ├── unknown to faction
      │       │
      │       └── later rumor/report
      │
      └── known/confirmed
              │
              ▼
       CONSEQUENCE APPLIED
              │
              ▼
       FACTION STATE CHANGES
              │
              ▼
       DOWNSTREAM REACTION
```

No repeated transition.

---

# 6. Standing vs Trust Contract

Until ADR resolves:

```text
DO NOT APPLY BOTH
```

Potential final model examples:

### Model A
Standing is canonical; trust is derived.

### Model B
Trust is local operational; standing is branch aggregate.

Combat applies one, coordinator derives the other.

### Model C
They are genuinely independent.

Then rule data must state which one each incident changes.

The bridge cannot guess.

---

# 7. Combat Context Contract

Minimum context classes:

```text
SELF_DEFENSE
AGGRESSIVE_ATTACK
ASSISTANCE
FRIENDLY_FIRE
BETRAYAL
FACTIONLESS
```

Add:
- patrol;
- raid;
- rebellion
only as tags/subcontexts when real encounter metadata supports them.

---

# 8. Information Reach Contract

Who knows is a separate question from what happened.

```text
combat fact
≠ faction awareness
```

A combat consequence may be:
- pending;
- rumored;
- confirmed.

---

# 9. Witness Contract

Preferred evidence hierarchy:

1. direct faction survivor/reporting unit;
2. known settlement witnesses;
3. radio/intelligence intercept;
4. companion/shelter witness for player-side memory;
5. seeded rumor leak only if authored.

No arbitrary blanket multiplier.

---

# 10. Incident Aggregation Contract

Political consequence should normally aggregate at:

```text
combat_id × faction_id
```

not every kill.

Sub-events may influence severity.

---

# 11. Consequence Rule Contract

Every rule answers:

```text
who was attacked?
who initiated?
what relation existed?
what outcome occurred?
who knows?
what bounded standing/trust effect applies?
what downstream reaction tags are eligible?
```

---

# 12. Anti-Farming Contract

Layers:

```text
exact incident idempotence
+ generated encounter uniqueness
+ optional cooldown/diminishing returns
+ standing bounds
```

Cooldown alone is insufficient.

---

# 13. Reaction Contract

Reaction tags do not own reaction logic.

Example:

```text
major_hostile_incident
→ faction authority decides protest/bounty/embargo
```

---

# 14. Diplomacy Recovery Contract

Combat bridge may create:
- a reason;
- a trigger;
- a commitment request.

It does not implement:
- envoy mechanics;
- deception skill;
- trade reparations
unless those systems already exist.

---

# 15. Quest Contract

Quest runtime asks faction state.

Combat bridge does not toggle quests directly unless canonical prerequisite API is explicitly intended.

---

# 16. Trade Contract

Trade refusal/embargo uses existing faction/economy stance.

No bridge-owned boolean.

---

# 17. Patrol Contract

Encounter difficulty/patrol frequency uses faction hostility/war state.

No bridge-owned encounter multiplier.

---

# 18. Moral Weight Contract

Political consequence is not automatically moral consequence.

MoralChoiceSystem only receives authored morally significant combat choices.

No war-crime system in Plan 139.

---

# 19. UI Contract

## Before combat
Known faction and risk.

## After combat
Political consequence summary.

## Faction detail
Recent incidents.

## Journal
Major history.

No dedicated new panel unless evidence proves need.

---

# 20. Persistence Matrix

| Fact | Owner |
|---|---|
| combat result | combat/event history |
| faction standing | faction |
| trust | stance authority |
| hostility | faction |
| awareness | rumor/intelligence |
| processed consequence IDs | bridge if needed |
| protest/bounty/embargo | owning systems |
| incident history | journal/world event |
| epilogue aggregate | derived |

No duplicate state.

---

# 21. Old-Save Migration

If no bridge state:

```text
processed = []
pending_unknown = []
```

Do not infer old combats and retroactively penalize factions.

---

# 22. Failure Injection Matrix

## N139.1 Same combat result delivered twice
Expected: one standing change.

## N139.2 Factionless bandit kill changes faction standing
Expected: fail.

## N139.3 Neutral patrol attacks player and receives full aggression penalty
Expected: context rule fail.

## N139.4 Unwitnessed wilderness combat instantly affects distant faction
Expected: information gate fail.

## N139.5 Faction standing and trust both independently receive same delta despite ADR saying derived
Expected: authority gate fail.

## N139.6 Expedition and combat bridge both apply consequence
Expected: idempotence/identity fail.

## N139.7 Bounty state stored inside bridge
Expected: architecture gate fail.

## N139.8 Repeated generated patrol farm maxes allied standing
Expected: anti-farm simulation fail.

## N139.9 Save reload reapplies protest trigger
Expected: idempotence fail.

## N139.10 UI reveals hidden faction identity
Expected: knowledge/access test fail.

## N139.11 Rule references nonexistent faction
Expected: integrity fail.

## N139.12 Combat UI directly edits standing
Expected: source-scan fail.

---

# 23. Determinism Contract

Same:

```text
combat outcome
+ faction state
+ encounter context
+ information state
+ consequence rules
+ seed where rumor leakage is probabilistic
```

must yield same:
- incident classification;
- affected factions;
- consequence IDs;
- standing/trust deltas;
- reaction tags;
- delayed-awareness schedule.

---

# 24. Long-Horizon Metrics

Track:

```text
political combat incidents
factionless no-ops
self-defense incidents
aggressive incidents
assistance incidents
standing gained/lost
pending unknown incidents
rumor-confirmed incidents
hostility transitions
embargos/bounties/protests if real
recovery actions
suppressed duplicate/farm attempts
```

---

# 25. Balance Guardrails

Routine combat consequences should be:
- meaningful;
- recoverable where design allows;
- not catastrophic from one misunderstood patrol.

Deliberate betrayal:
- can be severe.

Repeated violence:
- can drive war.

---

# 26. Information-Asymmetry Guardrails

Political reaction should feel caused by:
- who survived;
- who witnessed;
- who reported;
- what was intercepted.

Not omniscient game logic.

---

# 27. Performance Guardrails

Combat consequence projection is:
- end-of-combat only;
- O(participants + affected factions);
- no daily full combat-history scan.

---

# 28. Content/Data Acceptance

Scenario/rule data:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Gameplay scenarios reach effect.

---

# 29. CI / Gate Set

Recommended:

```text
combat_faction_authority_single
combat_faction_rule_integrity
combat_faction_information_gate
combat_faction_idempotence
combat_faction_no_farm
combat_faction_save_matrix
combat_faction_determinism
combat_faction_reaction_disposition
combat_faction_ui_access
```

---

# 30. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --combat-faction-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/verify-fast.sh
```

Use exact current commands if renamed.

---

# 31. Recommended Commit Breakdown

```text
139A-1 authority audit + standing/trust ADR
139A-2 combat result contract / stable incident ID
139A-3 consequence DTO/rule catalog
139A-4 context classifier
139A-5 incident aggregation
139A-6 idempotence / anti-duplicate
139A-7 generated matrix/tests
139A-8 headless/selftest/docs

139B-1 canonical faction mutation adapter
139B-2 information-state model
139B-3 direct participant/reporting integration
139B-4 rumor/intelligence delayed awareness
139B-5 faction-pair/assistance rules
139B-6 anti-farming/cooldown
139B-7 expedition/shelter-defense adapters
139B-8 save/pending-awareness tests

139C-1 reaction inventory/disposition
139C-2 protest/bounty/embargo adapters where real
139C-3 praise/alliance adapters where real
139C-4 patrol/trade/quest indirect integration
139C-5 reparations/commitment recovery where real
139C-6 10 scenario data
139C-7 threshold/reaction idempotence
139C-8 history/epilogue docs

139D-1 pre/post combat UI
139D-2 faction detail/journal history
139D-3 old-save/minimal bridge state
139D-4 deterministic replay
139D-5 30-day anti-farm soak
139D-6 180-day faction landscape
139D-7 CI gates/failure fixtures
139D-8 final ship/no-ship report
```

---

# 32. Risk Register

## R139.1 Standing and trust are double-mutated

Mitigation:
- ADR before implementation;
- authority tests.

## R139.2 Player gets punished for self-defense

Mitigation:
- context classifier;
- pre-combat political-risk preview.

## R139.3 Hidden factions become omniscient

Mitigation:
- information layer;
- delayed awareness.

## R139.4 Incident farming trivializes alliances

Mitigation:
- incident identity;
- diminishing/cooldown;
- long-horizon simulation.

## R139.5 Bridge becomes diplomacy system

Mitigation:
- reaction disposition table;
- adapters only.

## R139.6 Combat catalog faction IDs drift

Mitigation:
- data-integrity validation.

## R139.7 UI exposes political info player does not know

Mitigation:
- knowledge-gated faction identity and witness state.

## R139.8 Event history bloats saves

Mitigation:
- compact incident refs;
- reuse journal/world-event history.

---

# 33. Acceptance Checklist

## P0

- [ ] combat result authority audited
- [ ] combatant faction IDs audited
- [ ] context fields audited
- [ ] FactionBranchCoordinator mutation APIs audited
- [ ] FactionStanceEngine mutation APIs audited
- [ ] standing vs trust ADR published
- [ ] rumor/intelligence authority audited
- [ ] bounty authority audited
- [ ] embargo/trade authority audited
- [ ] quest gating audited
- [ ] patrol hostility audited
- [ ] journal/history audited
- [ ] save sections audited
- [ ] baseline no-consequence tests captured

## 139A

- [ ] canonical combat result reused
- [ ] result extended minimally if needed
- [ ] stable incident ID
- [ ] political consequence DTO minimal
- [ ] supported reason taxonomy
- [ ] severity discrete
- [ ] rule catalog versioned
- [ ] self-defense rule
- [ ] aggression rule
- [ ] assistance rule
- [ ] enemy-of-enemy not automatic
- [ ] friendly fire only if context exists
- [ ] factionless no-op
- [ ] civilian crossfire deferred unless authority exists
- [ ] per-incident aggregation
- [ ] bounded count weighting
- [ ] consequence floor/ceiling
- [ ] stable consequence ID
- [ ] exactly-once registry
- [ ] no blanket cooldown suppression
- [ ] optional anti-farm cooldown data-driven
- [ ] pure projection
- [ ] no mutation during calculation
- [ ] invalid faction integrity
- [ ] headless
- [ ] unit tests
- [ ] generated consequence matrix

## 139B

- [ ] one canonical standing/trust mutation path
- [ ] trust separately mutated only if ADR says so
- [ ] transaction order explicit
- [ ] source ID/idempotence
- [ ] information states
- [ ] direct participant knowledge
- [ ] settlement witness rules grounded
- [ ] companion witness semantics correct
- [ ] radio/rumor reused
- [ ] wilderness incidents may stay unknown
- [ ] delayed discovery
- [ ] pending consequence state
- [ ] physical vs reputation separated
- [ ] attribution represented
- [ ] no fake witness RNG
- [ ] seeded leakage only if authored
- [ ] no arbitrary 2x/0.5x omniscient multiplier
- [ ] standing clamp
- [ ] ally reward requires knowledge/rule
- [ ] positive-farm guard
- [ ] negative escalation bounded
- [ ] compact incident ledger
- [ ] no full combat duplication
- [ ] expedition combat same bridge
- [ ] shelter defense same bridge if applicable
- [ ] no double adapter application
- [ ] tests green

## 139C

- [ ] reaction inventory
- [ ] reaction disposition table
- [ ] no new reaction engine
- [ ] protest adapter only if existing
- [ ] bounty adapter only if existing
- [ ] embargo adapter only if existing
- [ ] praise adapter only if existing
- [ ] alliance adapter only if existing
- [ ] patrol hostility consumes faction state
- [ ] quest availability consumes faction state
- [ ] trade consumes faction state
- [ ] no reputation meter
- [ ] envoy/reparation/blame/accept dispositioned
- [ ] reparations reuse commitment/trade
- [ ] blame-rival only if deception/rumor exists
- [ ] self-defense derived, not manual button by default
- [ ] MoralChoice not globally wired
- [ ] event stories follow-on unless already supported
- [ ] 10 scenarios only after seams
- [ ] scenario outputs honest
- [ ] no reaction without authority
- [ ] downstream idempotence
- [ ] threshold transitions once
- [ ] commitments reused
- [ ] rumor reused
- [ ] place memory for major incidents
- [ ] epilogue derives from history

## 139D

- [ ] pre-combat faction identity
- [ ] hidden identity protected
- [ ] political-risk preview
- [ ] no exact hidden delta promise
- [ ] post-combat report
- [ ] faction detail incidents
- [ ] journal major incidents
- [ ] no new combat-record panel unless proven
- [ ] tooltips
- [ ] tutorial only if framework exists
- [ ] accessibility
- [ ] localization
- [ ] minimal persistence
- [ ] old-save default
- [ ] no restore mutation
- [ ] duplicate post-load result no-op
- [ ] mid-awareness save
- [ ] reaction state owned elsewhere
- [ ] deterministic fingerprint
- [ ] forbidden RNG scan
- [ ] headless
- [ ] faction ID integrity
- [ ] rule integrity
- [ ] 30-day anti-farm
- [ ] 180-day landscape
- [ ] accidental-hostility guard
- [ ] positive farming guard
- [ ] routine penalty bounded
- [ ] recovery path assessed
- [ ] performance bounded
- [ ] per-incident event budget
- [ ] selftest
- [ ] failure fixtures
- [ ] authority source-scan
- [ ] docs complete

---

# 34. Ship / No-Ship Gate

**SHIP** only if:

```text
combat_faction_bridges == 1
AND faction_standing_authorities == 1
AND duplicate_standing_trust_application == 0
AND factionless_combat_standing_effects == 0
AND duplicate_combat_incident_effects == 0
AND standing_changes_without_information_channel == 0
AND self_defense_context_supported == true
AND routine_incident_delta_within_budget == true
AND combat_bridge_owned_diplomacy_state == 0
AND combat_bridge_owned_bounty_state == 0
AND combat_bridge_owned_embargo_state == 0
AND combat_bridge_owned_quest_state == 0
AND global_reputation_meter_added == false
AND expedition_combat_double_application == 0
AND shelter_defense_double_application == 0
AND positive_standing_farming_detected == false
AND normal_defensive_play_all_factions_hostile == false
AND combat_faction_old_save == pass
AND combat_faction_save_roundtrip == pass
AND combat_faction_determinism == pass
AND combat_faction_information_gate == pass
AND combat_faction_selftest == pass
AND data_integrity_selftest == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 35. Implementer Handoff

1. Audit standing vs trust before writing a single delta.
2. Reuse the canonical combat-result event instead of teaching combat about politics.
3. Derive one stable combat incident ID.
4. Aggregate political consequence per incident/faction rather than per bullet or per kill.
5. Keep context explicit: self-defense, aggression, assistance, allied violence, factionless.
6. Put consequence rules in data and clamp them.
7. Use incident identity before adding any cooldown.
8. Reuse rumor/intelligence for information propagation.
9. Treat unknown incidents as pending, not half-strength omniscient consequences.
10. Apply standing/trust through one faction mutation authority.
11. Carry source IDs into downstream reactions for idempotence.
12. Let trade, quest, patrol, bounty, embargo and alliance systems consume faction state rather than storing their state in the bridge.
13. Do not create a generic reputation meter or "faction killer" stat.
14. Make expedition and shelter-defense combat feed the same bridge exactly once.
15. Show faction affiliation/political risk before combat when known.
16. Show actual political result after combat and in faction history.
17. Do not reveal hidden faction identity or witness information.
18. Use 10 scenarios to prove context and information differences—not to justify new reaction systems.
19. Add old-save and mid-awareness save fixtures.
20. Run anti-farm and 180-day faction-landscape simulations.
21. Close only when combat is politically meaningful without creating a second diplomacy game.

---

# 36. Final Outcome

When this plan is complete, faction-tagged combat stops being politically invisible.

A patrol fight is no longer just a tactical result. The combat system resolves who fought, who died, who initiated, and where it happened. The combat-faction bridge turns that result into one political incident and asks a separate question: who actually knows?

If the Garrison attacked first and the player defended themselves in the wilderness, the political consequence can be smaller, delayed, or absent until somebody reports it. If the player ambushed a neutral patrol in a settlement, the incident is likely to become a confirmed major grievance. If the player helps a rebel convoy survive an ambush, the rebels can reward that action only if they know the player was responsible.

Standing and trust change through the same faction authorities the rest of the game already uses. Trade restrictions, hostile patrols, bounties, protests, alliance opportunities and quest availability then react through their own systems. The combat bridge does not own any of those mechanics.

Repeated combat cannot be farmed by reloading or replaying one incident. Consequence IDs are stable. Save/load does not reapply penalties. Positive standing from assistance is bounded. Routine self-defense cannot accidentally make every faction hostile.

The player also receives fair information. Known faction affiliation and likely political risk can be shown before committing to violence. After combat, the report explains which faction reacted and why. Major incidents remain in faction history and the journal.

The result is a tactical game that finally has political memory.

Who the player shoots matters.
