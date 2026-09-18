# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 3.1
// SPDX-License-Identifier: MIT

**Document role:** execution-grade first half of Generation Wave 12 Part 3, derived from the verified Antigravity-authored Part 3 source and bounded to the four fully specified E-series tasks.

**Scope of Part 3.1**
1. **Task E1 — C1[24] Per-NPC Personal Memory & Relationship Depth (Plan 147A/147B/147C/147D)**
2. **Task E2 — C1[25] Working Animals & Companion Operating Roles (Plan 151A/151B/151C/151D)**
3. **Task E3 — C1[26] Black Market Contraband, Risk & Enforcement Waves (Plan 155A/155B/155C/155D)**
4. **Task E4 — C1[27] Shelter Governance, Legitimacy & Policy Decrees (Plan 159A/159B/159C/159D)**

**Reserved for Part 3.2:** F1–F4 from the Part 3 execution set:
- C1[28] Shelter Identity, Naming, Origin & Community Projection;
- C2[23] Stateful Ambience, Mix Discipline & Sparse Musical Arc;
- C2[24] Retention Policies, Save Corpus Archaeology & 400-Year Scale;
- C2[25] Retrospective Standing Gates & Plan-Layer Compression.

**Part 3.1 purpose:** deepen long-term character/community continuity without fragmenting authority. The four E-series tasks must connect existing systems rather than invent parallel ones:

- named NPC history → personal stance/dialogue/trade;
- persistent companions → bounded operational roles;
- black-market transactions → contraband risk/heat/enforcement;
- shelter management → declarative governance policy/legitimacy/unrest.

**Primary architectural danger:** each task sits adjacent to already-live authorities. A weak implementation would create duplicate reputation, roster, economy, faction, social, or policy systems. This plan therefore treats every bridge as an authority-routing problem first and a feature problem second.

---

# 0. OPERATING CONTRACT

## 0.1 Allowed terminal states

Every task in Part 3.1 must finish in exactly one recognized state:

- **IMPLEMENTED** — the missing mechanism, bridge, read model, consumer, or policy adapter is landed through canonical owners and all required focused gates pass.
- **DECIDED-DEFERRED** — a product/architecture/balance policy choice requires signature; the decision record states exactly what remains deferred and what event reopens it.
- **RETIRED** — duplicate, speculative, stale, or obsolete mechanism/data path is removed and unregistered with reference proof.
- **VERIFIED-RESOLVED** — current repository `HEAD` already satisfies the plan clause; redundant work is skipped and evidence captured.
- **ROUTED-REPAIR** — investigation reveals a real production defect outside this bounded task; a repair package with characterization evidence is registered.

The following are not terminal states:
- “mostly implemented”;
- “needs polish”;
- “future”;
- “manual follow-up”;
- “likely complete”;
- “TBD”.

## 0.2 Mandatory non-negotiable rules

1. **Godot is authoritative; Unity remains retired.**
2. **Core remains engine-free.** `Assets/Ashfall.Core/` stays free of Godot/engine namespaces and lifecycle types.
3. **JSON/data authority remains canonical.** Live schema/data at repository `HEAD` overrides draft examples.
4. **One authority per concern.** Do not duplicate personal memory, faction standing, survivor relationships, companion identity, inventory, black-market settlement, faction enforcement, governance policy, roster, needs, or medical state.
5. **Deterministic replay.** No unseeded randomness, wall-clock simulation logic, or hash-iteration-dependent ordering in Core.
6. **Claims before edits.**
7. **Exactly 20 procedural substeps per task.**
8. **Exactly four mini-substeps per named mini-task.**
9. **Focused verification first.**
10. **Build warning baseline may not worsen.**
11. **No hidden balance policy.** Draft values such as memory cap 10, 0.75x/1.50x trade clamps, +25 kg cargo, 75% spoilage reduction, +2 morale/day, 60% concealment, heat +5–20/-2/day, heat >=80, legitimacy 0–100, and unrest thresholds are examples until Plan 147/151/155/159 or current signed data confirms them.
12. **Derived state stays derived.** Do not add save fields for values that can be reconstructed from canonical persisted state.
13. **Exactly-once side effects are explicit.** Gifts, memory creation, heat escalation, raids, decree application, and unrest transitions may not duplicate on reload/re-entry.
14. **Presentation never owns simulation.**
15. **Host adapters route commands/events; Core owns domain rules.**
16. **UI warnings, badges, bars, and forecasts are projections only.**
17. **Current repository names win.** If a draft class/file was renamed or never created, map to the current owner instead of reconstructing stale architecture.
18. **Generated artifacts are regenerated, never hand-patched.**
19. **Every task produces an implementation log and updates census/ledger truth.**
20. **Part 3.2 must consume the final merged Part 3.1 handoff rather than reusing this document’s assumptions.**

## 0.3 Universal pre-edit evidence bundle

Before changing production/data:

- current commit/HEAD;
- worktree clean/dirty state;
- active claim table;
- source-plan revision;
- current census row;
- current authority map;
- current focused test baseline;
- current save DTO/store owners;
- current data-integrity state;
- current docs/generated index state.

## 0.4 Universal stop conditions

Stop and route if:

- a required path is actively claimed;
- the live owner differs materially from the historical plan;
- the feature requires a second manager/store rather than an adapter to an existing owner;
- a numeric clamp, penalty, decay, capacity, threshold, or probability is not source-authorized;
- a new memory/event record duplicates a canonical event ledger;
- a companion role requires a second roster or combat AI;
- contraband semantics require a new parallel economy/faction system;
- governance policy directly mutates operational systems instead of using existing command/modifier seams;
- a new save field is derivable from existing persisted state.

---

# 1. DEPENDENCY GRAPH

## 1.1 Recommended execution order

**E1 → E2 → E3 → E4**

Rationale:

- **E1** establishes personal memory/stance semantics that later community/governance work must not duplicate.
- **E2** extends persistent companion entities into operational roles while reusing roster/expedition/needs/morale rails.
- **E3** deepens the black-market/economy layer and depends on already-stable faction/travel enforcement seams.
- **E4** adds the broadest policy layer and therefore should consume final workforce, needs, medical, social, and economy authority boundaries rather than race them.

The current dependency DAG may override this sequence.

## 1.2 Required prior rails

Reverify before task start:

- B3 authored survivor identity;
- D1 relationship/social effect authority;
- C3 faction standing consequences;
- C2 needs-performance projection;
- C4 affliction capability gating;
- existing black-market settlement service from earlier waves;
- companion animal persistence/kennel owner;
- current duty/roster/needs/medical systems;
- DayEventVocabulary/SemanticKind;
- save durability/recovery contract;
- panel lifecycle/a11y gates.

Any regression becomes a separate repair package.

---

# 2. TASK E1 — C1[24] PER-NPC PERSONAL MEMORY & RELATIONSHIP DEPTH

**Source:** `C-integration-plans/C1_planintegration[24].md` — Plan 147A/147B/147C/147D\
**Blocker class:** PERSONAL MEMORY GAP / LACK OF NPC CONTINUITY\
**Canonical ownership:** current Narrative + Social + NPC catalog/trade/door-encounter owners\
**Primary acceptance:** named NPCs can remember significant player interactions through one deterministic, bounded personal-history authority that affects only individual interaction semantics and never duplicates faction standing.

## 2.1 Objective

Introduce or complete per-NPC continuity so named NPCs can remember relevant player actions and use those memories in later:

- greetings/dialogue;
- personal willingness to trade;
- return visits;
- gifts/intelligence;
- personal trust/grudge.

The memory layer is **personal history**, not:
- faction reputation;
- survivor-pair relationship;
- quest state;
- world knowledge;
- arbitrary narrative text cache.

## 2.2 Exact 20 procedural substeps

1. Reverify `HoldfastNpcCatalog`, current door-encounter/NPC/verdict systems, trade session owner, faction standing, and any existing personal-memory/event-history structures.
2. Confirm the invariant that per-NPC memory is an individual interaction history layer and may not replace or duplicate faction standing, quest state, or survivor relationship systems.
3. Claim the current NPC memory/social stance files, NPC catalog/data, dialogue adapter, trade integration, save owner, tests, UI, and documentation paths.
4. Implement/adapt a canonical engine-free personal-memory ledger keyed by stable NPC ID and immutable significant memory records.
5. Define only Plan 147/current data-authorized memory kinds; treat draft kinds (`DebtOwed`, `Betrayal`, `LifeSaved`, `ResourceGift`, `BrokenPromise`, `WitnessedCrime`) as candidate vocabulary until verified.
6. Represent information visibility/provenance only if the current narrative/intel architecture needs it, reusing SemanticKind/day-event or intel concepts rather than creating a parallel rumor system.
7. Implement/adapt a personal stance projection that derives individual trust/willingness from active memories and authored NPC identity without mutating faction standing.
8. Route memory-aware dialogue selection through the existing dialogue/bark selector using stable line IDs and current localization/content data.
9. Route personal trade effects through the current trade quote pipeline after canonical regional/faction pricing, using only Plan 147/current signed clamps and formulas.
10. Route return-visit/gift/intelligence consequences through existing door encounter, quest, intel, inventory, and scheduling owners; no duplicate reward scheduler.
11. Implement decay/retention only according to Plan 147/current memory policy; draft linear decay, permanent memories, and max-cap values are not automatic defaults.
12. Make memory creation deterministic and exactly-once for a given canonical source event/choice, including replay/save-load re-entry.
13. Extend NPC detail/read-model presentation with explainable player-known memory summaries without exposing hidden private/rumor state the player should not know.
14. Verify that personal stance and faction standing remain separately inspectable in code/tests and do not apply the same consequence twice.
15. Add focused Plan 147 tests for memory creation, duplicate prevention, stance projection, dialogue selection, trade effects, retention, and return encounters.
16. Persist only the minimal authoritative personal-memory state through the current NPC/narrative save owner; do not duplicate derived net affinity if it can be recomputed.
17. Prove save/restore restores the exact memory set/order/retention state present at the save point and does not replay source events.
18. Run data-integrity validation for NPC IDs, memory reason IDs, dialogue line IDs, trade references, and any visibility/event vocabulary.
19. Run focused narrative/social/door-encounter/trade/save tests touched by E1.
20. Update the NPC memory architecture document and hand off with memory taxonomy, personal-vs-faction boundary, stance formula source, retention policy, save/replay evidence, and zero-warning build.

## 2.3 Mini-task E1.1 — Memory Event Schema & Ledger Architecture

### E1.1.a
Define/use an immutable memory record with stable event/source ID, NPC ID, memory kind, campaign sequence/day, authored magnitude/category where required, and reason/content key.

### E1.1.b
Provide deterministic indexed queries such as current memories for NPC and derived personal stance inputs, while keeping derived totals out of persistence unless required.

### E1.1.c
Apply the **actual** Plan 147 retention/cap policy; treat the draft “max 10 active memories, evict low significance first” as an example until verified.

### E1.1.d
Add tests proving memory insertion, deterministic ordering, duplicate-source suppression, retention/eviction, and derived stance inputs.

## 2.4 Mini-task E1.2 — Dialogue & Greeting Variation Adapter

### E1.2.a
Implement/reuse a memory-aware dialogue adapter that selects from authored dialogue candidates based on current active memories without generating text dynamically.

### E1.2.b
Add/activate only Plan 147-scoped memory-specific dialogue variants in the existing NPC dialogue catalog, preserving localization IDs and tone rules.

### E1.2.c
Use safe typed tokens/placeholders supported by the current localization/dialogue system; no arbitrary runtime string formatting of hidden data.

### E1.2.d
Add deterministic tests proving the same NPC/memory state resolves the same eligible greeting/branch and that neutral NPCs retain baseline dialogue.

## 2.5 Mini-task E1.3 — Personal Trade Stance Modulation

### E1.3.a
Query personal stance through the canonical NPC memory/stance projection at the current trade quote calculation seam.

### E1.3.b
Apply only Plan-defined personal price/willingness bounds; draft `0.75x` discount and `1.50x` markup are non-authoritative until verified.

### E1.3.c
Compose personal stance **after/beside** canonical faction/regional/economy pricing according to the existing quote contract, preventing double application of faction consequences.

### E1.3.d
Add tests proving identical regional/faction conditions produce only the sourced personal difference and that neutral memory yields exact baseline quote parity.

## 2.6 Mini-task E1.4 — Memory Persistence & Decay Dynamics

### E1.4.a
Implement campaign-time decay/retention only if Plan 147/current policy defines it, using deterministic day/tick progression and no wall clock.

### E1.4.b
Support non-decaying/indelible memories only for explicitly authored categories/flags rather than hardcoded examples such as “saving a child”.

### E1.4.c
Persist authoritative memory records/version data under the current NPC/narrative save section, reusing existing save schema patterns.

### E1.4.d
Test continuous vs restored decay/retention journeys and verify expired memories, permanent memories, and stance recomputation match exactly.

## 2.7 NPC identity contract

Every personal memory references:
- stable canonical NPC ID;
- canonical source event/choice ID where possible.

Never key by:
- display name;
- encounter order;
- runtime object reference.

## 2.8 Memory record vs event ledger

Before adding storage, determine whether a canonical narrative/event ledger already records all required facts.

If the ledger can reconstruct personal memory deterministically:
- use a projection.

If Plan 147 requires independent retention/decay or personal private knowledge:
- dedicated bounded memory state may be justified.

Document the decision.

## 2.9 Memory kinds

Build from live Plan 147/current data:

| Memory kind | Source event | Personal effect | Dialogue consumer | Trade consumer | Retention |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

No uncited kind should change gameplay.

## 2.10 Information-state boundary

Potential knowledge classes:
- NPC-private;
- shelter/player-known;
- public rumor/intel.

Only implement if current game models these states.

Do not create a second rumor/intel simulation.

## 2.11 Exactly-once memory creation

A memory should have a stable source key.

Repeated:
- panel open;
- dialogue re-render;
- save/load;
- event replay
must not create duplicates.

## 2.12 Personal stance derivation

Personal stance may include:
- trust;
- hostility;
- willingness;
- gratitude,
according to Plan 147.

Prefer a typed projection over a single opaque number if consumers need different dimensions.

## 2.13 Faction separation

Example:
- faction standing = neutral;
- merchant remembers rescue = personally favorable.

Personal discount may apply without changing faction standing.

The reverse may also be true:
- high faction standing;
- specific NPC remembers betrayal.

Tests must prove independence.

## 2.14 Trade composition

Document exact quote order:

`base item/economy quote -> regional/faction modifiers -> personal NPC modifier -> other current adjustments`

Actual existing system wins.

No personal modifier should be applied twice in both quote and UI.

## 2.15 Return encounter consequence

If an NPC returns with a gift/intel:
- current scheduling/door encounter owner determines visit;
- inventory/intel owner applies consequence;
- memory serves as eligibility/context.

No memory ledger directly spawns items.

## 2.16 Dialogue selection

Use finite authored lines.

No LLM/runtime text generation.

Selection must:
- be deterministic according to current narrative RNG/ordering policy;
- not consume unrelated RNG during UI redraw.

## 2.17 Retention and decay

Possible policies:
- bounded recent memory;
- intensity decay;
- permanent flags;
- category-specific half-life.

Only current Plan 147 authorizes one.

Do not assume linear decay.

## 2.18 Eviction

If capacity exists:
- stable significance/order;
- deterministic tie-break.

No hash-order eviction.

## 2.19 Save model

Prefer storing:
- memory records;
- source IDs;
- timing/retention metadata.

Derived:
- net affinity;
- best dialogue branch;
- trade multiplier
should usually recompute.

## 2.20 UI explainability

Player-facing summary must:
- show only known memories;
- use localized/diegetic language;
- avoid leaking hidden magnitude unless UX permits.

## 2.21 E1 terminal acceptance

E1 reaches IMPLEMENTED when named NPC memory is singularly owned, significant events record exactly once, stance/dialogue/trade consumers use that memory without duplicating faction standing, retention policy is source-backed, save/replay is stable, and UI explanation respects information visibility.

---

# 3. TASK E2 — C1[25] WORKING ANIMALS & COMPANION OPERATING ROLES

**Source:** `C-integration-plans/C1_planintegration[25].md` — Plan 151A/151B/151C/151D\
**Blocker class:** LIMITED COMPANION ROLES / LACK OF OPERATIONAL VALUE\
**Canonical ownership:** current CompanionAnimal/Ecology + Expedition + Inventory/Needs + Shelter/Defense consumers\
**Primary acceptance:** persistent companion entities can perform Plan-defined operational roles through existing subsystem adapters without becoming inventory items, duplicate workers, or a separate combat AI.

## 3.1 Objective

Turn tamed companions into operationally meaningful persistent entities.

Potential roles from the source include:
- hauling/logistics;
- shelter guarding;
- pest control;
- therapy/bonded morale.

The exact role names, species restrictions, cargo bonuses, spoilage changes, morale bonuses, feed costs, flee/illness behavior, and combat effects are **Plan/data policy**, not safe defaults.

## 3.2 Exact 20 procedural substeps

1. Reverify the current `CompanionAnimalSystem`, companion catalog/data, kennel capacity, save owner, expedition party model, shelter defense, food spoilage, and bonded-survivor state.
2. Confirm the invariant that tamed animals are persistent campaign entities with stable identity and are not modeled as ordinary inventory items.
3. Claim current companion role/assignment, save, expedition, defense, food/spoilage, morale, UI, tests, and documentation paths.
4. Implement/adapt a canonical companion role assignment contract that stores one active role/context per eligible living companion where Plan 151 requires.
5. Implement hauling/pack contribution through the existing expedition cargo-capacity calculation using authored per-species/role values rather than the draft `+25 kg`.
6. Implement guard/security contribution through the existing shelter defense/intrusion/combat modifier system without creating a second combat AI.
7. Implement pest-control contribution through the current spoilage/sanitation/infestation owner using Plan 151-sourced reduction semantics rather than the draft `75%`.
8. Implement therapy/bonded-wellness contribution through the existing morale/stress relationship owner only where Plan 151 defines it.
9. Route daily companion feed demand through the canonical inventory/consumption pipeline using authored species/feed rules.
10. Route underfeeding/starvation consequences through current companion health/needs/state transitions; do not invent flee/illness outcomes without source policy.
11. Route companion injury/death and bonded caretaker grief through existing medical/companion/mourning owners without duplicate morale penalties.
12. Integrate companion role assignment into current duty/kennel presentation as a companion-specific assignment surface, not a duplicate human DutyRoster model.
13. Update kennel/companion UI to assign/unassign roles through typed commands with eligibility/preflight results.
14. Keep guard animals as modifiers/participants according to current combat contract; do not introduce an autonomous second combat simulation.
15. Add focused Plan 151 tests covering role assignment, species compatibility, feed, hauling, guarding, pest control, morale, injury/death, and reassignment.
16. Persist authoritative role assignment, companion health, feed/need state, and bonded identity only through the current companion save owner.
17. Prove deterministic outcomes for any seeded companion recovery/hunting/incident behavior and pure deterministic role modifiers where no RNG is needed.
18. Run data-integrity validation for companion species/archetype IDs, role definitions, feed tags/items, handler refs, and role-specific content.
19. Run focused shelter, companion, inventory/food, expedition, defense/combat, morale, and save suites touched by E2.
20. Update companion working-role documentation and hand off with role taxonomy, species eligibility, modifier-source table, save contract, test evidence, and zero-warning build.

## 3.3 Mini-task E2.1 — Operational Role Assignment Framework

### E2.1.a
Define/use the minimal role vocabulary actually required by Plan 151/current consumers; treat `Unassigned`, `PackHauler`, `FacilityGuard`, `PestControl`, `Therapy` as candidates until verified.

### E2.1.b
Store/reuse role assignment on the canonical companion entity/state owner rather than creating a parallel role manager if the companion system already supports assignments.

### E2.1.c
Validate species/archetype capability using current companion data and return typed assignment refusal reasons.

### E2.1.d
Add tests for assign, unassign, switch, invalid species-role pair, dead/injured companion, save/restore, and immediate consumer refresh.

## 3.4 Mini-task E2.2 — Logistics & Expedition Pack Carry Integration

### E2.2.a
Integrate eligible assigned pack companions into the canonical expedition cargo-capacity/readiness calculation using current role/species values.

### E2.2.b
Apply terrain/endurance travel effects only if Plan 151/current expedition mechanics explicitly define them; do not invent rough-terrain penalties from the draft.

### E2.2.c
Route companion exposure/injury during expedition combat/hazards through current expedition/companion health logic without creating animal-specific combat resolution.

### E2.2.d
Add deterministic expedition fixtures proving the exact authored cargo contribution, dispatch eligibility, injury persistence, and neutral parity without a pack role.

## 3.5 Mini-task E2.3 — Food Storage Pest Control & Sanitation

### E2.3.a
Connect eligible assigned pest-control companions to the current spoilage/infestation/sanitation evaluation seam.

### E2.3.b
Apply the Plan-defined reduction/formula; draft `75% spoilage reduction` is non-authoritative until verified.

### E2.3.c
Emit flavor journal/day-event content only through existing narrative/event rails and only if Plan 151 includes that content scope.

### E2.3.d
Add seeded/deterministic spoilage fixtures proving the exact sourced pest-control contribution without changing unrelated food categories.

## 3.6 Mini-task E2.4 — Companion Feed Drain & Bonded Morale

### E2.4.a
Route daily feed demand through the canonical inventory/consumption transaction using current feed tags/species requirements.

### E2.4.b
Persist/reuse bonded caretaker/handler identity only if the current companion relationship model defines one; do not manufacture a handler link solely for a morale bonus.

### E2.4.c
Apply Plan-defined handler/household morale or stress effect through the existing needs/morale modifier stack; draft `+2/day` is not authoritative.

### E2.4.d
Add tests for fed, underfed, starving, handler absent/dead, companion ill/dead, and restore journeys, including exactly-once grief/distress events.

## 3.7 Companion identity

Stable companion instance ID must survive:
- role assignment;
- expedition;
- injury;
- save/load.

Do not identify by species/display name alone.

## 3.8 Role assignment authority

Prefer role field on companion state or one current companion assignment owner.

Avoid a separate dictionary if the entity model can own it safely.

## 3.9 Role taxonomy

Build:

| Role | Eligible species/archetypes | Primary consumer | Modifier source | Feed/health prerequisites |
|---|---|---|---|---|
|  |  |  |  |  |

Blank modifier source means no implementation.

## 3.10 Human roster separation

Human duty assignments and animal roles may share presentation conventions but not necessarily the same domain model.

Do not force animals into survivor workforce aggregates.

## 3.11 Pack capacity

Cargo contribution belongs to expedition capacity calculation.

Do not mutate item weights or vehicle base capacity.

## 3.12 Expedition speed

Only if Plan 151 says pack animals alter speed/endurance.

Avoid double-counting:
- vehicle;
- terrain;
- C2 survivor needs;
- route hazards;
- animal endurance.

## 3.13 Guard role

Guard contribution may influence:
- detection;
- alert time;
- handler combat modifier;
- perimeter readiness,
according to current defense mechanics.

No autonomous dog AI subsystem unless already existing.

## 3.14 Pest-control role

Pest control should modify the current rodent/spoilage/infestation owner.

Do not directly delete food loss after the fact if the system supports a risk modifier upstream.

## 3.15 Therapy role

Relationship/morale owner computes wellness effect.

Companion role supplies context/modifier.

No independent “animal morale meter” unless current system already owns one.

## 3.16 Feed demand

Use canonical item tags/categories if available.

Do not hardcode raw meat/grain/scraps unless Plan 151/data defines allowed feeds per species.

## 3.17 Starvation consequences

Possible:
- reduced role efficiency;
- illness;
- aggression;
- flee;
- death.

Only source-backed outcomes.

## 3.18 Companion mortality/grief

On death:
- companion entity transitions terminal state;
- role clears;
- handler/bonded grief routed once through mourning/morale owner;
- expedition/shelter consumer removes contribution.

## 3.19 Save policy

Persist only authoritative companion state:
- identity;
- health/alive;
- assignment;
- bond if canonical;
- feeding state if canonical.

Derived role modifiers recompute.

## 3.20 E2 terminal acceptance

E2 reaches IMPLEMENTED when working roles are source-backed, companion identity remains singular/persistent, hauling/guard/pest/morale consumers use existing owners, feed and mortality route through canonical inventory/health/grief rails, no separate combat AI/roster is created, and save/replay tests are green.

---

# 4. TASK E3 — C1[26] BLACK MARKET CONTRABAND, RISK & ENFORCEMENT WAVES

**Source:** `C-integration-plans/C1_planintegration[26].md` — Plan 155A/155B/155C/155D\
**Blocker class:** SHALLOW UNDERGROUND TRADE / LACK OF CONTRABAND RISK\
**Canonical ownership:** current BlackMarket/Economy + ItemTag + Faction/Travel/Enforcement owners\
**Primary acceptance:** contraband profit is coupled to authored jurisdiction risk, detection, heat, confiscation, and enforcement through existing systems without creating a second trade settlement or raid authority.

## 4.1 Objective

Deepen the already-existing black-market transaction system by adding risk and enforcement.

The black market must remain:
- economically authoritative through the existing settlement service;
- item-legality-aware through declared tags/data;
- faction/jurisdiction-aware;
- deterministic under seeded checks;
- save-stable;
- legible to the player.

E3 does **not** reimplement Buy/Sell/Loan/Repay.

## 4.2 Exact 20 procedural substeps

1. Reverify current `BlackMarketSystem`, settlement service, transaction actions, dealer trust, item tags, regional/faction jurisdiction, expedition route traversal, standing, raids, and save owner.
2. Confirm the invariant that contraband risk extends existing black-market settlement and faction enforcement; it does not create parallel pricing, currency, inventory, standing, or raid systems.
3. Claim current contraband-risk, item legality, travel/checkpoint, black-market heat, enforcement/raid adapters, UI, tests, data, save, and documentation paths.
4. Implement/adapt a pure engine-free contraband classification/risk query over canonical inventory item IDs/tags, jurisdiction, route context, dealer/heat state, and concealment inputs.
5. Define contraband categories and jurisdiction rules only from Plan 155/current data; draft categories are candidates, not defaults.
6. Integrate checkpoint/inspection eligibility into the current route/travel event pipeline for faction-controlled segments using authored encounter/checkpoint semantics.
7. Integrate concealment/shielded cargo through existing vehicle/equipment/modifier owners and Plan-defined detection formulas; draft `60% reduction` is not authoritative.
8. Route failed inspection outcomes through current confiscation/inventory, faction-standing, encounter/combat, and choice systems according to signed Plan 155 semantics.
9. Implement/adapt syndicate/regional heat as one canonical black-market state dimension only if current system does not already track equivalent enforcement pressure.
10. Route high heat into the current enforcement/raid/embargo/market-closure owner using authored thresholds and escalation states rather than draft hardcoded values.
11. Implement heat decay only according to Plan 155/current inactivity/legitimate-trade rules and campaign time; draft `-2/day` is not automatic.
12. Emit security warnings/briefing entries through existing SemanticKind/day-event/reporting rails without direct builder-owned calculations.
13. Update black-market presentation to show authoritative heat/risk class or probability only when the player has enough information to know it.
14. Ensure confiscation targets only items illegal in the relevant jurisdiction and never removes legal inventory through broad `contraband` assumptions.
15. Add focused Plan 155 tests covering legality, jurisdiction, concealment, detection, choices, confiscation, standing, heat, decay, escalation, and legal-trade neutrality.
16. Persist only authoritative heat/trust/enforcement state through the current black-market/faction save owner; derived risk percentages recompute.
17. Prove deterministic checkpoint/enforcement behavior for identical seed/state/route/cargo and save/reload schedule.
18. Run data-integrity validation for item tags, jurisdiction/faction/region IDs, concealment upgrades, enforcement event IDs, and black-market content.
19. Run focused economy, faction, expedition/travel, inventory, combat/raid, briefing, and save suites touched by E3.
20. Update contraband/heat documentation and hand off with legality taxonomy, risk formula source, choice/consequence matrix, heat state machine, save/replay evidence, and zero-warning build.

## 4.3 Mini-task E3.1 — Contraband Item Classification & Tagging

### E3.1.a
Audit current item definitions and existing public tags before adding any contraband/jurisdiction tag; classify only Plan 155-scoped items.

### E3.1.b
Represent jurisdiction restrictions through the current item legality/faction/region data model rather than scattered hardcoded faction switches.

### E3.1.c
Expose/reuse a canonical query such as `IsContrabandInJurisdiction(itemId, jurisdictionId)` only if the architecture lacks an equivalent.

### E3.1.d
Add tests proving legal staples remain legal, jurisdiction-specific items differ correctly, unknown tags fail validation, and custom tagged items follow declared rules.

## 4.4 Mini-task E3.2 — Travel Checkpoint Search & Concealment Engine

### E3.2.a
Bind checkpoint eligibility to canonical route-segment/faction-control traversal events rather than polling expedition inventory from UI/host.

### E3.2.b
Resolve detection using the current seeded RNG architecture and Plan-defined patrol vigilance, party/vehicle concealment, and heat inputs.

### E3.2.c
Offer only the player choices actually authored by Plan 155/current encounter system; draft “Bribe / Surrender / Fight” is a candidate set until verified.

### E3.2.d
Add deterministic tests for concealed, detected, legal-only, mixed-cargo, high/low heat, and save/reload cases using sourced concealment effects.

## 4.5 Mini-task E3.3 — Syndicate Heat Accumulation & Decay

### E3.3.a
Define/reuse heat state and transaction contribution from the current black-market system; draft `+5–20` increments are non-authoritative.

### E3.3.b
Implement campaign-time decay only according to Plan 155/current inactivity rules; distinguish inactivity from “legitimate trade” if the design does.

### E3.3.c
Route threshold transitions to current market/dealer/enforcement owners and use authored state/threshold definitions.

### E3.3.d
Add tests for accumulation, exact boundary transitions, cap/floor behavior, decay, no-transaction days, restore, and deterministic repeated schedules.

## 4.6 Mini-task E3.4 — Law Enforcement Raid Dispatch

### E3.4.a
Translate critical heat/enforcement state into the existing raid/enforcement dispatcher rather than creating a new `EnforcementRaidCoordinator` if a canonical raid owner already exists.

### E3.4.b
Use the current perimeter/raid/combat owner for shelter attacks; do not route through `SkyDefenseBatterySystem` unless Plan 155/current architecture explicitly defines that integration.

### E3.4.c
Apply post-raid heat/standing/dealer-state consequences according to Plan 155 rather than the draft assumption that repelling/bribing resets heat to “moderate”.

### E3.4.d
Add integration tests proving the actual escalation path and exactly-once raid scheduling at the authored threshold/state.

## 4.7 Legality taxonomy

Build from live data:

| Item/category | Jurisdiction | Legal status | Concealability | Confiscation? | Standing consequence source |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

No generic global “contraband” assumption when jurisdiction differs.

## 4.8 Contraband category

A semantic category may exist for:
- ordnance;
- narcotics;
- relics;
- toxic chemicals,
but only if current data defines them.

Tags should be declarative and validation-backed.

## 4.9 Jurisdiction owner

A faction/region controls legality.

Do not put faction-specific bans into UI code.

## 4.10 Checkpoint eligibility

Not every route in a faction region must necessarily roll inspection.

Use current:
- patrol event;
- route hazard/event;
- enforcement schedule
contract.

## 4.11 Detection formula

Document:

`base inspection/vigilance -> heat -> cargo visibility -> concealment -> other current modifiers -> seeded resolution`

Actual formula/source wins.

## 4.12 Player knowledge

Show exact inspection percentage only if the game intentionally reveals it.

Otherwise show qualitative risk:
- low;
- elevated;
- severe,
etc.

Do not leak hidden patrol stats.

## 4.13 Choice resolution

Bribe/surrender/fight choices, if present, must route to:
- economy;
- inventory;
- faction;
- combat
owners.

No panel-side mutation.

## 4.14 Confiscation

Filter inventory through jurisdiction legality query.

Confiscate:
- only affected illegal items;
- correct quantity;
- through canonical inventory transaction.

No legal collateral loss unless Plan 155 says.

## 4.15 Heat authority

Heat may be:
- regional;
- dealer-specific;
- global syndicate;
- faction-specific.

Do not assume one scalar until current Plan 155 confirms.

## 4.16 Heat contribution

High-volume/value transactions may increase heat according to:
- value;
- category;
- frequency;
- trust,
if defined.

No guessed +5–20 range.

## 4.17 Heat decay

Campaign time only.

No wall clock.

Decay must not occur twice on save/load or repeated day processing.

## 4.18 Enforcement escalation

Potential states:
- normal;
- scrutiny;
- dealer withdrawal;
- checkpoint intensification;
- embargo/market closure;
- raid,
according to actual plan.

Use state transitions rather than scattered numeric checks where possible.

## 4.19 Faction standing

Contraband enforcement may affect standing.

Standing owner remains canonical.

Avoid duplicating C3 combat-faction consequences when a checkpoint fight occurs:
- combat bridge handles violence;
- contraband incident handles smuggling offense.

Document composition.

## 4.20 Black-market settlement

Existing settlement service continues to own:
- funds;
- goods;
- transaction atomicity.

Risk/heat must never create an alternate buy/sell settlement path.

## 4.21 Save policy

Persist:
- current authoritative heat/enforcement/dealer state as required.

Derived:
- current inspection chance;
- risk display
recompute.

## 4.22 E3 terminal acceptance

E3 reaches IMPLEMENTED when legality is data/jurisdiction-driven, checkpoint risk is deterministic and source-backed, concealment/choices route through current systems, confiscation is precise, heat/escalation use one authority, legal trade remains unaffected, and save/replay cannot duplicate inspections/raids/consequences.



# 5. TASK E4 — C1[27] SHELTER GOVERNANCE, LEGITIMACY & POLICY DECREES

**Source:** `C-integration-plans/C1_planintegration[27].md` — Plan 159A/159B/159C/159D\
**Blocker class:** POLICY GAP / UNSTRUCTURED SHELTER RULES\
**Canonical ownership:** current Shelter + Social + Duty/Needs/Medical operational owners\
**Primary acceptance:** shelter governance becomes a declarative policy layer that modifies existing operations through typed adapters, with one legitimacy model, deterministic council/consultation semantics, explicit unrest routing, and save-stable decree state.

## 5.1 Objective

Introduce or complete governance as a **policy-intent layer** above existing operational systems.

Governance may define:
- active decree/policy IDs;
- legitimacy/acceptance state;
- council consultation/vote outcomes;
- policy modifiers;
- unrest state.

Governance may **not** become:
- a second duty system;
- a second ration/inventory owner;
- a second medical triage system;
- a second social/morale system;
- a second quest/event runtime.

The draft’s legitimacy range, costs, thresholds, ideological labels, decree names, and unrest outcomes are all **policy examples** until verified against Plan 159/current data.

## 5.2 Exact 20 procedural substeps

1. Reverify current shelter policy-like controls, including rationing, duty hours, medical priority, curfew/security, survivor beliefs, social/morale, and any existing council/leadership/legitimacy state.
2. Confirm the invariant that governance owns **policy intent and legitimacy**, while existing operational systems continue to own resources, assignments, needs, treatment, security, and event resolution.
3. Claim current governance/policy owner, decree data, adapters, save store, UI, social/council integration, tests, and documentation paths.
4. Implement/adapt a canonical engine-free governance state containing active policy/decree IDs, legitimacy/acceptance state, and any Plan-defined pending/repeal/cooldown state.
5. Define governance policy domains from current Plan 159/data; draft domains (`RationingProtocol`, `LaborMandates`, `MedicalTriagePriority`, `SecurityCurfew`) are candidates until verified.
6. Implement legitimacy/acceptance using the current Plan 159 model and bounds; draft `0–100` is not authoritative unless source policy confirms it.
7. Implement decree enact/repeal commands through a central validation/transaction path that checks conflicts, costs, prerequisites, council rules, and exactly-once transition semantics.
8. Route labor/work policies into the existing Plan 24/C2 productivity, duty-hours, overwork, and needs modifier seams rather than rewriting worker output in governance.
9. Route rationing policies into existing nutrition/inventory/ration allocation owners rather than directly subtracting food/water from governance.
10. Route medical-priority policies into the existing triage/medical allocation contract only where Plan 159 defines those policy choices and without bypassing canonical medical safety constraints.
11. Implement council consultation/vote mechanics only according to current authored survivor identity/leadership/governance rules; do not invent ideology classes or deterministic voting formulas from the draft.
12. Route low-legitimacy/unrest transitions into existing social/event/duty/inventory/security owners using Plan-defined typed incidents rather than governance directly stealing items or skipping work.
13. Emit/reuse canonical semantic events for decree enact/repeal, legitimacy transitions, consultation outcomes, and unrest only where current event vocabulary requires them.
14. Extend daily briefing/read models to summarize active policies and authoritative community response without calculating policy effects in presentation.
15. Implement the governance UI as a read/command surface that displays current policies, requirements, conflicts, forecasted sourced consequences, legitimacy/acceptance, and confirmation results.
16. Add focused Plan 159 tests for decree validation, conflicts, adapters, legitimacy changes, council outcomes, unrest transitions, repeal, and exactly-once application.
17. Persist authoritative governance state only through the current shelter/governance save owner, with neutral old-save defaults and no duplicated operational state.
18. Prove deterministic policy/council outcomes for identical canonical survivor composition, beliefs, prior events, decree, and seed where RNG is actually part of the Plan.
19. Run focused social, shelter, duty, needs/nutrition, medical, security, event, save, UI lifecycle, and accessibility suites touched by E4.
20. Update governance/law documentation and hand off with decree taxonomy, adapter map, legitimacy model source, conflict rules, council/unrest semantics, save/replay evidence, and zero-warning build.

## 5.3 Mini-task E4.1 — Decree Registry & Effect Adapters

### E4.1.a
Define/extend the authoritative decree schema/catalog only if current architecture uses authored decree data; required fields come from Plan 159 and current data conventions.

### E4.1.b
Implement policy adapters that translate active decree state into named modifiers/constraints consumed by existing productivity, needs, ration, medical, security, or other owners.

### E4.1.c
Define decree conflicts/prerequisites declaratively or through one canonical validation table, using draft pairs such as “Generous Portions vs Starvation Rations” only if actual IDs exist.

### E4.1.d
Add tests proving enact/repeal updates the exact intended adapter contributions immediately and leaves unrelated operations at baseline.

## 5.4 Mini-task E4.2 — Community Legitimacy & Council Consensus

### E4.2.a
Use the actual Plan 159 legitimacy/acceptance model and inputs; do not invent a scalar formula from happiness/food security merely because the draft suggests it.

### E4.2.b
Resolve council opinion from canonical authored survivor beliefs/roles/leadership state using current identity vocabulary rather than draft-only labels such as Pragmatist/Egalitarian/Authoritarian unless verified.

### E4.2.c
Apply consultation/unilateral-enactment consequences according to signed Plan 159 policy and stable reason IDs.

### E4.2.d
Add deterministic tests for representative council compositions, identical-state repeatability, legitimacy transitions, and neutral behavior where no consultation modifier applies.

## 5.5 Mini-task E4.3 — Civil Unrest & Sabotage Event Cascade

### E4.3.a
Evaluate unrest eligibility from the canonical governance/social state at the current daily event seam using the actual Plan 159 threshold/state model rather than hardcoding `<30`.

### E4.3.b
Route Plan-defined unrest consequences into existing duty, inventory, security, or event owners as typed incidents; governance may request an incident but does not directly mutate foreign state.

### E4.3.c
Expose only the player responses authored by current content/Plan 159; draft “Concede / Enforce / Negotiate” is a candidate choice set, not a default.

### E4.3.d
Add tests for unrest entry, no-duplicate incident scheduling, response resolution, legitimacy/social follow-up, restore, and return to normal operations when conditions actually resolve.

## 5.6 Mini-task E4.4 — Governance UI & Decree Enactment Surface

### E4.4.a
Implement/extend the current governance panel route with categorized policies, status, prerequisites, conflicts, sourced consequences, and enact/repeal controls.

### E4.4.b
Show predicted legitimacy/acceptance impact only when the domain read model can provide it; do not fabricate exact forecasts from UI-side arithmetic.

### E4.4.c
Ensure keyboard/controller traversal, text scaling, non-color-only state cues, confirmation modal focus isolation, and back/cancel behavior follow B1 input contracts.

### E4.4.d
Run current UI accessibility, panel lifecycle, input/focus, and intended snapshot gates for the governance surface.

## 5.7 Governance authority split

### Governance owns
- policy/decree state;
- legitimacy/acceptance if Plan-defined;
- consultation/vote state;
- policy conflict/prerequisite validation;
- governance event intent.

### Operational systems own
- food/water allocation;
- duty assignment/hours;
- medical treatment/triage;
- inventory transactions;
- security/combat;
- needs/morale effects;
- actual event side effects.

### Presentation owns
- display;
- forecast/read model;
- commands.

## 5.8 Decree identity

Every decree uses a stable ID.

Do not key by:
- display label;
- panel index;
- localized string.

## 5.9 Policy domains

Build from live Plan/data:

| Policy domain | Decrees | Operational owner(s) | Conflict group | Save state |
|---|---|---|---|---|
|  |  |  |  |  |

No unused domain should be added merely because it appeared in a draft.

## 5.10 Decree schema

Potential fields:
- decree ID;
- domain;
- prerequisites;
- conflicts;
- enact/repeal cost;
- legitimacy/acceptance effect;
- operational modifier references;
- cooldown;
- visibility/unlock.

Actual schema follows Plan 159/current data conventions.

## 5.11 Enactment transaction

A decree command should:
1. resolve current state;
2. validate unlock/prerequisites;
3. validate conflicts;
4. calculate/validate any governance cost;
5. commit active state;
6. emit typed transition result/event;
7. let operational adapters project effects.

Avoid partially applying effects before validation.

## 5.12 Repeal semantics

Plan 159 must define:
- immediate repeal?;
- cost?;
- cooldown?;
- lingering effect?;
- legitimacy consequence?

Do not assume repeal is free/infinite.

## 5.13 Conflict semantics

Mutually exclusive decrees:
- cannot coexist;
- replacement behavior must be explicit.

Possible:
- reject;
- auto-repeal conflicting;
- require explicit repeal.

Current policy wins.

## 5.14 Legitimacy model

Do not hardcode `0–100`.

Possible models:
- bounded scalar;
- tiered state;
- support/opposition groups;
- derived approval.

Use actual Plan 159 representation.

## 5.15 Legitimacy inputs

Potential:
- broken promises;
- crisis handling;
- ration stability;
- deaths;
- ideology alignment;
- consultation.

Only source-backed inputs.

No double-counting with morale unless designed.

## 5.16 Legitimacy vs morale

These are not automatically the same.

- **Morale:** survivor psychological state.
- **Legitimacy:** acceptance of shelter authority/policy.

One may influence the other if Plan 159 defines it.

Do not alias them.

## 5.17 Council membership

Council composition must come from:
- current survivor governance/leadership rules;
- explicit roles;
- authored membership,
not “all survivors” unless Plan says so.

## 5.18 Ideology source

Consume B3 authored belief identity.

Do not infer council ideology from unrelated traits if B3 already removed heuristics.

## 5.19 Vote calculation

If deterministic:
- same members/beliefs/decree/state → same result.

If seeded uncertainty exists:
- registered governance RNG stream;
- stable member order;
- save/replay semantics.

No `System.Random`.

## 5.20 Predicted approval

UI forecast should use the same canonical vote/legitimacy read model.

Never duplicate formula in panel.

## 5.21 Labor policy adapters

A labor decree may affect:
- permitted duty hours;
- productivity modifier;
- overwork risk;
- worker compliance.

Route through Plan 24/C2 owners.

Do not directly add output units.

## 5.22 Ration policy adapters

A ration decree may alter:
- allocation rules;
- target portions;
- priority,
through current ration/nutrition owner.

It must not directly subtract items.

## 5.23 Medical policy adapters

Medical priority is sensitive because the current medical system owns safety/treatment applicability.

Governance may define an allocation priority if Plan 159 allows.

It must not:
- make invalid treatment medically valid;
- bypass contraindications/game constraints;
- consume medicine outside inventory transaction.

## 5.24 Security/curfew adapters

Route to existing:
- shelter access;
- security;
- schedule;
- social
owners.

No new security runtime.

## 5.25 Unrest state

Prefer a typed state machine if Plan 159 is stateful:
- stable;
- strained;
- unrest;
- crisis,
or actual current vocabulary.

Avoid scattered `if legitimacy < 25` checks.

## 5.26 Unrest incident generation

Unrest may request:
- strike;
- theft;
- sabotage;
- protest;
- confrontation,
according to current event catalog.

Each incident uses canonical owner for side effect.

## 5.27 Exactly-once unrest

A threshold transition should not fire the same unrest incident every day unless Plan explicitly defines recurring unrest.

Use:
- state transition;
- cooldown;
- event receipt
as current architecture requires.

## 5.28 Response choices

Player response may alter:
- legitimacy;
- morale;
- security;
- duty;
- faction-like internal groups,
according to content.

Each effect routed to owner.

## 5.29 Save policy

Persist:
- active decree IDs;
- legitimacy/acceptance state if authoritative;
- council/pending state if needed;
- unrest state/cooldown/receipt if authoritative.

Do not persist:
- worker productivity multiplier;
- ration multiplier;
- medical allocation projection,
if derivable.

## 5.30 Old-save neutrality

Missing governance section/fields:
- default no active decrees;
- neutral baseline legitimacy/acceptance according to current migration policy;
- no surprise unrest.

Exact defaults must come from Plan/save policy.

## 5.31 UI command boundary

Panel sends:
- enact;
- repeal;
- consult;
- resolve response,
through host/domain command.

It never writes active decree set directly.

## 5.32 E4 terminal acceptance

E4 reaches IMPLEMENTED when governance has one canonical policy/legitimacy owner, operational systems consume typed adapters rather than duplicated logic, council/unrest rules are source-backed and deterministic, save state is minimal/neutral, exactly-once transitions are proven, and the governance UI is accessible/read-only apart from typed commands.

---

# 6. CROSS-TASK AUTHORITY MAP

| Task | Canonical source state | Bridge/projection | Consuming owners | New persistence expected |
|---|---|---|---|---|
| E1 | named NPC + canonical events | personal memory/stance | dialogue/trade/door encounter | memory records if not derivable |
| E2 | companion entity | work-role projection | expedition/defense/spoilage/morale | assignment/health/bond if authoritative |
| E3 | inventory + jurisdiction + black market | contraband risk/heat | travel/faction/inventory/raid/briefing | heat/enforcement if canonical |
| E4 | governance state | policy adapters | duty/needs/medical/security/social | decree/legitimacy/unrest state |

---

# 7. CROSS-TASK COMPOSITION RULES

## 7.1 E1 + faction standing

Personal memory may modify one NPC’s interaction.

Faction standing remains separate.

Never:
- convert every faction standing change into every NPC memory;
- apply the same betrayal/rescue as both personal and faction effect unless Plan explicitly defines both.

## 7.2 E1 + D1 survivor relationships

E1 is for named external NPC personal memory.

D1 is for survivor-to-survivor pair relationships.

Do not merge their ledgers merely because both contain “affinity”.

## 7.3 E2 + human duty roster

Companion roles may be displayed alongside duties.

Do not include animals in:
- survivor worker headcount;
- human labor hours;
- medical duty eligibility,
unless a specific role adapter says so.

## 7.4 E2 + C2/C4

Companion role effects may modify expedition/shelter outcomes.

Survivor needs/performance and affliction capability remain separate.

Do not let a pack animal “restore” an incapable survivor’s WastelandTravel capability unless Plan 151/143 explicitly defines assistance.

## 7.5 E3 + C3 combat/faction bridge

A checkpoint event may cause combat.

Separate consequences:
- smuggling offense/inspection → E3/faction enforcement;
- violence/casualties → C3 combat-faction incident.

Deduplicate shared standing effects through reason IDs/policy.

## 7.6 E3 + black-market settlement

Existing settlement service owns trade atomicity.

Heat/risk observes committed transactions.

Do not increase heat for a transaction that failed/rolled back unless Plan 155 explicitly counts attempted deals.

## 7.7 E4 + C2 performance

Governance can contribute policy modifiers into the same productivity stack.

Needs projection remains independent.

Named contributors prevent double-counting.

## 7.8 E4 + medical capability

Governance cannot make an incapable survivor medically capable.

Triage priority may affect resource allocation only.

## 7.9 E4 + authored beliefs

Council/vote consumes B3 authored identity.

No reintroduced heuristic ideology inference.

---

# 8. SAVE / PERSISTENCE CONTRACT

## 8.1 E1

Potential persisted state:
- personal memory records;
- retention metadata.

Derived:
- net personal stance;
- trade multiplier;
- greeting selection.

## 8.2 E2

Persist only canonical companion state:
- stable instance;
- role;
- health/alive;
- feeding state if owned;
- bond/handler if canonical.

Derived:
- cargo bonus;
- pest reduction;
- guard modifier;
- morale contribution.

## 8.3 E3

Persist only authoritative:
- heat/enforcement state;
- dealer trust if already current;
- scheduled raid receipt if required.

Derived:
- current inspection risk;
- contraband status from item/jurisdiction data.

## 8.4 E4

Persist:
- active policies;
- legitimacy/approval if authoritative;
- governance transition/cooldown state if needed.

Derived:
- operational modifiers.

---

# 9. DETERMINISM CONTRACT

## E1
Same source events + campaign time → same memory set/order/stance.

## E2
Role modifiers are pure; any hunting/recovery/incident RNG uses registered streams and stable ordering.

## E3
Same route/cargo/jurisdiction/heat/concealment/seed → same inspection/enforcement outcomes.

## E4
Same survivor/council/policy/history state → same deterministic verdict, or same seeded result if Plan explicitly uses uncertainty.

---

# 10. IMPLEMENTATION LOG STANDARD

Every E-series implementation log contains:

1. plan identity;
2. source revision;
3. current census status;
4. active claims;
5. historical premise;
6. current premise;
7. authority map;
8. exact 20-step completion matrix;
9. mini-task completion matrix;
10. data/numeric policy source table;
11. files/data changed;
12. persistence impact;
13. determinism/RNG;
14. exactly-once strategy;
15. focused commands/results;
16. UI/selftest results;
17. terminal state;
18. remaining blocker;
19. census/ledger update;
20. Part 3.2 dependency impact.

---

# 11. ROLLBACK / ROUTING MATRIX

| Task | Finding | Required action |
|---|---|---|
| E1 | current event log fully reconstructs memory | projection; avoid new persistence |
| E1 | trade modifier clamp not sourced | decision-block |
| E1 | memory capacity/decay not sourced | do not invent |
| E1 | personal consequence duplicates faction effect | reconcile owner/reason |
| E2 | role values not sourced | decision/data package |
| E2 | current companion state already stores role | extend, no new manager |
| E2 | guard role requires second combat AI | stop; use modifier/current combat owner |
| E2 | feed consequences unspecified | no invented flee/death behavior |
| E3 | jurisdiction legality model absent | bounded data/decision package |
| E3 | concealment/heat probabilities unsourced | decision/data package |
| E3 | raid owner differs from draft | current raid owner wins |
| E3 | legal trade regresses | block/revert |
| E4 | legitimacy model absent | decision/package |
| E4 | decree directly requires operational rewrite | adapt existing owner, no duplicate |
| E4 | ideology classes differ from draft | authored B3/current vocabulary wins |
| E4 | unrest thresholds/actions unsourced | decision/data package |

---

# 12. PART 3.1 MASTER ACCEPTANCE MATRIX

| Task | Core blocker | Terminal proof |
|---|---|---|
| E1 | named NPCs forget prior interaction | singular memory/stance + deterministic dialogue/trade + save/replay |
| E2 | companions are passive collectibles | source-backed roles + canonical consumers + feed/health/grief/save |
| E3 | black market lacks systemic risk | jurisdiction legality + deterministic inspection + heat/enforcement + legal-trade parity |
| E4 | shelter policy is implicit/unstructured | governance state + adapters + council/legitimacy/unrest + accessible UI |

---

# 13. PART 3.2 HANDOFF CONTRACT

Part 3.2 must consume the final merged E-series evidence.

## 13.1 From E1
- personal memory taxonomy;
- save-state shape;
- personal stance/trade composition order;
- information visibility semantics.

## 13.2 From E2
- companion role taxonomy;
- persistent companion/save shape;
- role modifiers and species constraints;
- feed/grief integration.

## 13.3 From E3
- legality/jurisdiction taxonomy;
- black-market heat/enforcement state machine;
- faction/travel/raid integration;
- any save changes.

## 13.4 From E4
- policy/decree taxonomy;
- legitimacy/council model;
- operational adapter map;
- governance save state;
- unrest transition contract.

These may materially affect:
- shelter identity/community projection;
- ambience state;
- retention policies;
- standing gate compression.

Part 3.2 must reverify them at final `HEAD`.

---

# 14. PART 3.1 CLOSEOUT CHECKLIST

1. E1 NPC catalog/current memory owner reverified.
2. E1 memory persistence justified.
3. E1 memory kinds sourced.
4. E1 retention/decay/cap sourced.
5. E1 personal vs faction separation proven.
6. E1 dialogue/trade deterministic.
7. E1 save/replay green.
8. E2 companion role taxonomy sourced.
9. E2 no second role/roster authority.
10. E2 cargo/guard/pest/morale values sourced.
11. E2 feed/starvation behavior sourced.
12. E2 mortality/grief exactly once.
13. E2 save/replay green.
14. E3 legality jurisdiction model sourced.
15. E3 inspection/concealment formula sourced.
16. E3 heat thresholds/decay sourced.
17. E3 raid/enforcement owner reused.
18. E3 legal trade baseline preserved.
19. E3 save/replay green.
20. E4 decree/policy taxonomy sourced.
21. E4 legitimacy model sourced.
22. E4 council beliefs consume canonical B3 identity.
23. E4 adapters reuse operational owners.
24. E4 unrest exactly-once.
25. E4 save/replay green.
26. All focused suites green.
27. UI lifecycle/a11y green where changed.
28. Build warning baseline preserved.
29. Generated docs/indexes synchronized.
30. Claims/census/ledger handed off.

---

# 15. PART 3.1 NON-GOALS

- no second faction reputation system;
- no runtime-generated dialogue;
- no arbitrary memory decay/cap;
- no second companion roster;
- no autonomous parallel animal combat engine;
- no guessed companion bonuses/feed penalties;
- no second black-market settlement service;
- no guessed contraband heat/raid thresholds;
- no duplicate faction standing consequence for smuggling/combat;
- no second nutrition/medical/duty system inside governance;
- no invented ideology/voting model;
- no hardcoded legitimacy/unrest thresholds from draft examples;
- no broad full-suite churn as substitute for focused evidence.


# APPENDIX A — E1 NPC MEMORY EXECUTION PACKET

## A.1 Current named-NPC census

Before implementation, build:

| NPC family | Stable ID source | Dialogue owner | Trade owner | Door/encounter owner | Existing history? |
|---|---|---|---|---|---|
| Holdfast NPCs |  |  |  |  |  |
| Verdict NPCs |  |  |  |  |  |
| Traders |  |  |  |  |  |
| Wanderers |  |  |  |  |  |

The purpose is to prove all memory-bearing NPCs have stable identity and a current consumer.

## A.2 Existing history audit

Search for:
- encounter receipts;
- quest flags;
- trade trust;
- faction incidents;
- door-encounter history;
- NPC-specific flags;
- journal facts.

For each candidate, record whether it:
- already represents the needed memory;
- can be projected;
- is too coarse;
- belongs to another authority.

## A.3 Memory event provenance

Each personal memory must answer:
- what canonical source event created it?
- which NPC experienced/knows it?
- what was the campaign sequence/day?
- what memory kind/reason applies?
- can it be recorded twice?

No anonymous “+10 trust” entry.

## A.4 Source-event key

A stable key may combine:
- event ID;
- NPC ID;
- quest/encounter ID;
- source sequence.

Never use current timestamp.

## A.5 Magnitude/value policy

If memories carry magnitude:
- source from Plan 147/data;
- define unit/range;
- clamp policy sourced.

Do not infer magnitude from dialogue text.

## A.6 Net affinity calculation

If `GetNetAffinity` exists:
- document exact aggregation;
- permanent vs decaying entries;
- clamp;
- neutral baseline.

If consumers need multidimensional trust/hostility rather than scalar:
- do not collapse prematurely.

## A.7 Information visibility

For each memory:
- private to NPC?
- known to player?
- public rumor?
- faction-known?

Presentation only shows player-known.

No hidden-state leak.

## A.8 Dialogue selection ordering

Candidate lines sorted stably.

Selection inputs may include:
- strongest relevant memory;
- authored NPC personality;
- current context;
- semantic event.

No random UI redraw behavior.

## A.9 Trade personal stance

Build a quote breakdown:

| Layer | Source | Example reason ID |
|---|---|---|
| base | item/economy |  |
| regional/faction | economy/faction |  |
| personal NPC | E1 stance |  |
| scarcity/other | current owner |  |

This prevents double-counting.

## A.10 Return visit

A remembered sheltering/rescue event may make a return encounter eligible.

Eligibility is not scheduling.

Scheduling remains owned by encounter/calendar/door system.

## A.11 Gift/intel consequence

On return:
- gift -> inventory transaction;
- intelligence -> intel/world knowledge owner;
- quest -> quest runtime.

Memory is condition, not payout owner.

## A.12 Memory decay boundary

If campaign day advances:
- decay evaluation once;
- no duplicate on load.

Test save before/after day boundary.

## A.13 Permanent memory policy

Permanent memories require explicit authored flag/category.

Do not hardcode thematic examples in Core logic.

## A.14 Capacity pressure

If per-NPC bound is reached:
- deterministic eviction;
- protect permanent records if policy;
- stable tie-break.

Test several equal-significance entries.

## A.15 Old-save behavior

If old save lacks memory section:
- neutral empty memory;
- no synthetic history reconstruction unless migration policy explicitly derives it.

## A.16 NPC removal/death

If named NPC becomes permanently unavailable:
- memory may remain historical or be pruned according to save/content policy.

Do not leave invalid catalog references.

## A.17 E1 failure injection

Test:
- unknown NPC;
- duplicate source event;
- hidden/private memory shown to player;
- capacity overflow;
- identical magnitude tie;
- expired memory;
- permanent memory;
- missing dialogue line;
- trade session after save/restore.

## A.18 E1 merge-readiness

- [ ] NPC identities stable;
- [ ] no duplicate faction reputation;
- [ ] memory source keys stable;
- [ ] retention sourced;
- [ ] personal stance formula sourced;
- [ ] dialogue deterministic;
- [ ] trade composition explicit;
- [ ] return consequences routed;
- [ ] old saves neutral;
- [ ] focused tests green.

---

# APPENDIX B — E2 COMPANION ROLE EXECUTION PACKET

## B.1 Companion entity census

| Field | Current owner | Persisted? | Required by E2? |
|---|---|---|---|
| stable ID |  |  |  |
| species/archetype |  |  |  |
| health |  |  |  |
| alive |  |  |  |
| kennel slot |  |  |  |
| role |  |  |  |
| handler/bond |  |  |  |
| feed state |  |  |  |

## B.2 Species-role capability matrix

| Species/archetype | Pack | Guard | Pest | Therapy | Source |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

No source = not allowed.

## B.3 Assignment validation

Check:
- companion exists;
- alive;
- health/condition sufficient;
- species role allowed;
- not conflicting with expedition/kennel state;
- role slot capacity if any.

Return typed refusal.

## B.4 Assignment transition

1. validate;
2. clear conflicting assignment;
3. set canonical role/context;
4. emit change;
5. consumers recompute.

No consumer directly edits companion role.

## B.5 Pack contribution

Cargo calculation should expose breakdown:
- vehicle/base;
- survivor/equipment;
- companion;
- penalties.

Use actual Plan value.

## B.6 Pack animal presence

If companion is injured mid-expedition:
- cargo limit/overweight consequences follow current expedition policy.

Do not silently delete carried items.

Route overweight state to current expedition owner.

## B.7 Guard contribution

Potential:
- intrusion detection;
- perimeter alert;
- combat handler modifier.

One or more only if Plan 151 defines.

No separate initiative/AI loop unless existing combat supports companion units.

## B.8 Pest-control contribution

Clarify whether it changes:
- infestation probability;
- spoilage probability;
- spoilage amount;
- specific food classes.

Use current sanitation/kitchen owner.

## B.9 Flavor events

Flavor journal line:
- authored content ID;
- optional;
- no gameplay side effect unless separately defined.

Do not generate random prose.

## B.10 Therapy effect

Clarify:
- handler-only;
- all shelter;
- nearby survivors;
- interaction action.

Use Plan 151.

No universal +2 morale assumption.

## B.11 Feed categories

Build:

| Species | Allowed feed tag/item | Units/day | Consequence if insufficient | Source |
|---|---|---:|---|---|
|  |  |  |  |  |

No guessed diet.

## B.12 Feed transaction

Use inventory atomic consume.

If partial feed:
- current policy decides whether partially satisfied.

No direct item count decrement in companion system.

## B.13 Underfeeding state

If companion need state exists:
- update once per day.

Role effectiveness may derive from it.

No repeated stacking.

## B.14 Starvation/flee behavior

Only if authored.

If fleeing:
- companion entity transitions through current departure/removal event;
- role cleared;
- handler reaction routed.

## B.15 Disease/health

Companion disease/injury integrates with current companion health/veterinary system if present.

Do not reuse human medical state blindly.

## B.16 Bonded handler

If canonical:
- stable survivor ID;
- validation on survivor death/removal;
- transfer/rebond semantics if defined.

## B.17 Companion death

Exactly once:
- alive→dead transition;
- clear role;
- emit grief/context;
- save.

No grief every day after death.

## B.18 UI assignment

Kennel panel:
- role list;
- eligibility;
- current contribution;
- feed requirement;
- warnings.

No consumer formula duplication.

## B.19 Expedition UI

Show pack companion contribution through canonical expedition read model.

No extra capacity in UI-only total.

## B.20 E2 failure injection

Test:
- invalid species role;
- dead companion assignment;
- injured pack animal;
- insufficient feed;
- handler death;
- companion death;
- role change same day;
- save/restore;
- expedition overweight after injury.

## B.21 E2 merge-readiness

- [ ] one companion authority;
- [ ] role data sourced;
- [ ] species matrix sourced;
- [ ] no duplicate roster;
- [ ] no second combat AI;
- [ ] feed transaction canonical;
- [ ] grief exactly once;
- [ ] save state minimal;
- [ ] role modifiers derived;
- [ ] focused suites green.

---

# APPENDIX C — E3 CONTRABAND / HEAT EXECUTION PACKET

## C.1 Current black-market architecture census

| Concern | Current owner | Persisted? | E3 action |
|---|---|---|---|
| buy/sell settlement |  |  | reuse |
| dealer trust |  |  |  |
| item tags |  |  | extend/query |
| jurisdiction |  |  |  |
| route control |  |  | consume |
| checkpoint event |  |  |  |
| faction standing |  |  | consume |
| raids |  |  | consume |
| heat |  |  |  |

## C.2 Transaction event contract

Heat should usually react to **committed** black-market transaction result.

Fields:
- transaction ID;
- region/jurisdiction;
- item/value categories;
- amount/value;
- dealer;
- success.

Failed transaction behavior must be sourced.

## C.3 Legality lookup

A query should be pure:

`item + jurisdiction -> legality result`

Possible result:
- legal;
- restricted;
- contraband;
- prohibited;
- unknown.

Use actual vocabulary.

## C.4 Region vs faction

Do not conflate region ID and faction ID.

Jurisdiction may derive from:
- controlling faction;
- law region;
- settlement.

Use current world/faction map.

## C.5 Item tagging

Prefer declared semantic tags/category IDs.

Do not identify contraband via item-name prefix.

## C.6 Legal control items

Maintain explicit tests for ordinary legal items to catch overbroad tagging.

Use current data examples.

## C.7 Checkpoint trigger

Checkpoint event should be tied to:
- route/segment;
- faction control;
- event probability/schedule;
- heat if applicable.

No per-frame or per-UI-open roll.

## C.8 RNG stream

Use current expedition/world/event RNG.

Stable call order.

Do not let opening map or black-market panel consume it.

## C.9 Concealment input

Vehicle/equipment owner supplies:
- concealment/shielding capability.

E3 combines according to Plan 155.

No duplicate vehicle upgrade state.

## C.10 Detection outcome

A successful detection produces typed incident.

It does not directly:
- remove items;
- alter standing;
- start combat.

Those are choice/consequence owner actions.

## C.11 Player choices

Each authored choice maps:

| Choice | Owner(s) | Preconditions | Consequence |
|---|---|---|---|
|  |  |  |  |

No panel-side state changes.

## C.12 Bribe

If present:
- canonical currency owner;
- atomic payment;
- failure if insufficient;
- faction/heat consequence sourced.

## C.13 Surrender

Inventory transaction removes only jurisdiction-illegal goods selected by policy.

No accidental legal loss.

## C.14 Fight

Combat started through current combat encounter owner.

C3 then handles political violence consequences.

Avoid double standing penalty:
- smuggling offense reason;
- combat casualty reason
are separate and policy-driven.

## C.15 Heat scope

Determine:
- global;
- regional;
- faction;
- dealer.

Do not store one scalar if design has multiple jurisdictions.

## C.16 Heat contribution formula

Record:
- transaction volume;
- value;
- category;
- repeat behavior,
as defined.

No arbitrary range.

## C.17 Heat thresholds

Use named states if possible:
- normal;
- attention;
- high;
- critical,
according to Plan.

Consumers respond to transitions.

## C.18 Dealer disappearance/closure

If Plan 155 defines:
- current market/black-market availability owner applies.

Heat system emits state/context.

## C.19 Raid scheduling

One threshold crossing should not schedule multiple identical raids.

Use current raid scheduler receipt/cooldown.

## C.20 Raid outcome

Post-raid heat change is a policy effect.

Do not reset to moderate without source.

## C.21 Briefing warning

Use typed security/economy event.

Daily briefing presentation remains read-only.

## C.22 Risk display

If exact percentages are hidden by design:
- display qualitative category.

Do not expose internal RNG chance automatically.

## C.23 Save/load

Persist canonical heat/enforcement.

After restore:
- no re-roll of already resolved checkpoint;
- scheduled raid remains once;
- dealer state preserved.

## C.24 E3 failure injection

- legal-only cargo;
- one contraband item;
- mixed jurisdiction;
- concealment upgrade;
- high heat;
- duplicate route event;
- insufficient bribe;
- surrender;
- fight;
- raid threshold;
- save before roll;
- save after roll.

## C.25 E3 merge-readiness

- [ ] black-market settlement unchanged;
- [ ] legality pure/data-backed;
- [ ] jurisdiction explicit;
- [ ] RNG stream stable;
- [ ] concealment sourced;
- [ ] heat scope/formula sourced;
- [ ] escalation exactly once;
- [ ] confiscation precise;
- [ ] combat standing not double-counted;
- [ ] save/replay green.

---

# APPENDIX D — E4 GOVERNANCE EXECUTION PACKET

## D.1 Governance baseline census

| Existing control | Current owner | Policy-like already? | E4 action |
|---|---|---|---|
| rations |  |  |  |
| labor hours |  |  |  |
| medical priority |  |  |  |
| curfew/security |  |  |  |
| leadership/council |  |  |  |
| morale/social |  |  |  |
| unrest events |  |  |  |

## D.2 Governance state minimalism

Before persisting a field ask:
- is this policy intent?
- is this legitimacy/council state?
- or is it operational state already owned elsewhere?

Only the first two categories normally belong in governance save state.

## D.3 Decree registry

| Decree | Domain | Prerequisites | Conflicts | Operational adapter | Legitimacy source |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## D.4 Modifier identity

Every decree adapter uses stable contributor ID.

Example:
`governance:<decree_id>`

This prevents duplicate modifier stacking.

## D.5 Multiple decrees

Define whether multiple decrees in different domains compose.

For same domain:
- conflict rules;
- override rules;
- precedence.

No accidental multiplicative explosion.

## D.6 Enactment validation

Test failure reasons:
- locked;
- conflict;
- insufficient legitimacy/cost;
- council requirement unmet;
- already active;
- cooldown.

No partial mutation.

## D.7 Repeal

Test:
- active only;
- cooldown/cost;
- modifier removal;
- legitimacy consequence;
- save/restore.

## D.8 Policy adapters

Adapters should expose either:
- modifier contribution;
- policy query;
- priority rule.

They should not copy the entire operational algorithm.

## D.9 Extended duty hours

If such decree exists:
- policy may alter allowed/target hours;
- Plan 24 duty/overwork system computes resulting effects.

Governance does not directly add work output.

## D.10 Rationing decree

Policy may alter ration allocation target/priority.

Nutrition/inventory performs consumption.

## D.11 Medical triage decree

Policy may set allowed priority rule only if Plan 159 supports.

Medical applicability/safety remains unchanged.

## D.12 Council identity

Use stable survivor IDs/roles.

Do not persist display-name council membership.

## D.13 Council eligibility

Rules may include:
- leadership role;
- age/status;
- elected/appointed;
- shelter role.

Only source-backed.

## D.14 Belief mapping

Use B3 authored beliefs.

If governance needs a coarser political stance:
- define an explicit mapping in one place;
- do not reintroduce hidden inference everywhere.

## D.15 Vote weights

If one survivor one vote:
- explicit.

If leadership weights differ:
- source.

No guessed weighting.

## D.16 Consensus output

Prefer typed result:
- approve;
- reject;
- split;
- abstention,
or current vocabulary.

UI displays.

## D.17 Legitimacy delta source

Every change uses reason ID and Plan-defined value/formula.

No raw `legitimacy -= 10` scattered.

## D.18 Legitimacy floor/ceiling

Source from Plan.

If scalar, clamp in one authority.

If tiered, no fake 0–100 conversion.

## D.19 Recovery/restoration

Draft says happiness and food stability restore legitimacy.

Only implement if Plan 159 defines passive recovery.

Otherwise legitimacy changes via events/actions only.

## D.20 Unrest eligibility

Use state/rule engine.

No duplicated thresholds in UI and social coordinator.

## D.21 Unrest event catalog

| Unrest event | Preconditions | Owner | Exactly-once/cooldown | Resolution choices |
|---|---|---|---|---|
|  |  |  |  |  |

## D.22 Strike

If shift refusal exists:
- DutyRoster owner receives incident/restriction.

No governance-side assignment deletion.

## D.23 Theft

If medical supplies stolen:
- Inventory owner handles exact transaction.

Event system chooses payload according to authored content.

## D.24 Sabotage

Facility/equipment owner handles damage.

Governance does not own facility durability.

## D.25 Resolution choices

Each choice has typed effects routed to owners.

No UI direct mutation.

## D.26 Daily briefing

Show:
- active policies;
- changed legitimacy/acceptance;
- unrest event;
- council result.

No hidden vote math if not player-visible.

## D.27 Governance UI prediction

Forecast should be:
- domain-computed;
- labeled estimate if uncertainty;
- same rule as commit validation.

Do not calculate differently in UI.

## D.28 Old-save migration

No governance state:
- Plan-defined neutral baseline;
- no active decrees;
- no unrest receipts.

Test legacy fixture.

## D.29 E4 failure injection

- conflicting decrees;
- enact already active;
- repeal inactive;
- council tie;
- absent council member;
- low legitimacy boundary;
- repeated day tick;
- unrest save/restore;
- operational modifier removed on repeal.

## D.30 E4 merge-readiness

- [ ] governance owns policy only;
- [ ] decree schema sourced;
- [ ] conflicts sourced;
- [ ] legitimacy model sourced;
- [ ] council uses authored identity;
- [ ] adapters use current owners;
- [ ] no duplicated operations;
- [ ] unrest exactly once;
- [ ] save state minimal;
- [ ] UI prediction authoritative;
- [ ] a11y/lifecycle green.

---

# APPENDIX E — CROSS-TASK FAILURE-INJECTION REGISTRY

## E.1 E1
- duplicate dialogue choice;
- repeated door encounter;
- private memory visibility leak;
- memory-cap tie;
- expired/permanent memory;
- faction standing changes independently;
- trade quote restore.

## E.2 E2
- species-role mismatch;
- dead/injured animal;
- insufficient feed;
- handler removed;
- pack animal lost mid-expedition;
- pest-control unavailable;
- grief duplicate;
- save/restore.

## E.3 E3
- jurisdiction switch;
- legal goods only;
- hidden cargo;
- duplicate checkpoint;
- failed transaction;
- high heat transition;
- raid scheduling twice;
- combat after checkpoint;
- save before/after inspection.

## E.4 E4
- decree conflict;
- council rejection;
- legitimacy boundary;
- repeated enact;
- repeal;
- unrest recurrence;
- response resolution;
- legacy save;
- UI forecast mismatch.

---

# APPENDIX F — PART 3.1 COMMAND / EVIDENCE LOG TEMPLATE

**Task:**\
**Substep / mini-task:**\
**Command:**\
**HEAD:**\
**Claim state:**\
**Expected:**\
**Actual:**\
**Cases:**\
**Warnings/errors:**\
**Save/replay impact:**\
**Artifact/log:**\
**Disposition:**\

All terminal claims must point to exact evidence.

---

# APPENDIX G — PART 3.1 HANDOFF TEMPLATE

## TASK `<ID>` HANDOFF — `<TITLE>`

### 1. Workspace
- HEAD:
- Worktree:
- Claims:
- Source plan:
- Census:

### 2. Premise Reverification
- Historical blocker:
- Current state:
- Already sealed clauses:
- Stale assumptions corrected:

### 3. Authority
- Source owner:
- Adapter/projection:
- Consumers:
- Duplicate-authority proof:

### 4. Policy/Data
- Numeric values:
- Source:
- Tags/categories:
- Signed decisions:
- Deferred decisions:

### 5. Implementation
- Files:
- Data:
- New types:
- Retired paths:
- Generated artifacts:

### 6. Save / Determinism
- Persisted fields:
- Derived fields:
- Old-save behavior:
- RNG stream:
- Exactly-once key:
- Restore proof:

### 7. Verification
- Focused tests:
- Integrity:
- UI/a11y:
- Build:
- Warnings:

### 8. Governance
- Terminal state:
- Remaining blocker:
- Census/ledger:
- Claim handoff:
- Part 3.2 impact:

---

# APPENDIX H — CROSS-TASK INTEGRATION JOURNEYS

## H.1 NPC memory + trade

1. Named NPC begins neutral.
2. Player completes a source-authored favorable action.
3. One memory is recorded.
4. Faction standing stays unchanged unless separately specified.
5. NPC greeting changes through authored line selection.
6. Trade quote gains exactly one personal contribution.
7. Save.
8. Restore.
9. Same memory/quote/greeting eligibility remains.

## H.2 Companion + expedition

1. Tamed companion assigned eligible pack role.
2. Expedition preflight reads role.
3. Capacity increases by sourced amount.
4. Party departs.
5. Companion becomes injured in a deterministic fixture.
6. Current role effectiveness/capacity responds according to Plan.
7. Save/restore.
8. No duplicate companion/entity created.

## H.3 Contraband + combat consequence

1. Expedition enters controlled jurisdiction with illegal cargo.
2. Checkpoint detection resolves.
3. Player chooses fight if authored.
4. Smuggling incident produces its own faction/legal consequence.
5. Combat resolves.
6. C3 produces casualty/violence consequence.
7. Standing owner combines distinct reasoned effects exactly once.
8. Save/restore does not duplicate either.

## H.4 Governance + work performance

1. Baseline worker team.
2. Enact Plan-defined labor policy.
3. Governance records policy.
4. Plan24/C2 owner receives named adapter contribution.
5. Productivity changes by sourced value.
6. Overwork/needs consequences remain owned by existing systems.
7. Repeal policy.
8. Contribution disappears and baseline returns.

## H.5 Governance + medical allocation

1. Scarce medicine scenario.
2. Policy exists if Plan 159 defines triage priority.
3. Medical owner receives policy priority context.
4. Invalid treatment remains invalid.
5. Inventory transaction remains canonical.
6. UI explains policy effect without calculating it.

---

# APPENDIX I — FINAL REVIEWER QUESTIONS

## I.1 E1
- Is memory distinct from faction standing?
- Is storage necessary, or could existing event history project it?
- Are decay/cap/trade clamps actually sourced?
- Can UI leak private memory?
- Can a saved event record twice after restore?

## I.2 E2
- Are companions persistent entities?
- Are role values sourced?
- Does assignment duplicate the human roster?
- Does guard duty create a second combat AI?
- Are feed/grief effects exactly once?
- Can role modifiers recompute after restore?

## I.3 E3
- Is legality jurisdiction-specific?
- Is heat one canonical state?
- Are detection/concealment values sourced?
- Does checkpoint combat double-count standing?
- Can high heat schedule duplicate raids?
- Does legal trade remain unchanged?

## I.4 E4
- Does governance own policy rather than operations?
- Are legitimacy/council formulas sourced?
- Are B3 beliefs reused?
- Do decree adapters have stable contribution IDs?
- Is unrest stateful/exactly-once?
- Does UI forecast use the same domain rule?

---

# APPENDIX J — FINAL NO-FALSE-CLOSURE RULES

Part 3.1 is not complete if:

- E1 introduces a personal reputation system that also changes faction standing by default;
- E1 hardcodes max 10 memories, linear decay, permanent categories, 0.75x/1.50x trade bounds without authority;
- E1 persists derived affinity unnecessarily;
- E2 treats companions as inventory items;
- E2 hardcodes +25 kg, 75% pest reduction, +2 morale/day, specific feed costs without authority;
- E2 adds a second combat AI or survivor roster;
- E3 reimplements black-market settlement;
- E3 hardcodes 60% concealment, +5–20 heat, -2/day, >=80 raid thresholds without authority;
- E3 routes raids through a draft-named subsystem without verifying current raid ownership;
- E3 removes legal inventory during confiscation;
- E4 hardcodes 0–100 legitimacy, <25/<30 unrest, ideology labels, or decree effects without Plan 159/current data;
- E4 directly edits worker output, food, or treatment state instead of routing policy adapters;
- E4 creates a second social/morale or medical system;
- any task lacks focused evidence;
- warning baseline worsens;
- claims/census/ledger are stale;
- Part 3.2 starts from original draft assumptions rather than final handoff.

**Final invariant:** Part 3.1 must deepen long-term continuity by making named NPCs remember, companions work, illicit trade carry risk, and shelter policy matter—while every operational consequence remains owned by the system that already owns that domain.


# APPENDIX K — FINAL MERGE-READINESS AUDIT

## K.1 E1 merge readiness

E1 is merge-ready only when:

- all named NPC families have stable canonical IDs;
- existing event/quest/trade/faction history was audited before new persistence was introduced;
- each stored memory has a stable source event/reason;
- duplicate recording is impossible across replay/reopen/save-load;
- the retention/cap/decay model cites Plan 147/current policy;
- personal stance and faction standing remain separate;
- dialogue selection is finite/authored/deterministic;
- trade modulation uses the canonical quote pipeline exactly once;
- return visits/gifts/intel use existing scheduling/inventory/intel owners;
- hidden/private memories are not exposed by UI;
- old saves default neutrally;
- focused narrative/social/trade/save tests are green.

## K.2 E2 merge readiness

E2 is merge-ready only when:

- companions remain persistent canonical entities;
- role assignment has one owner;
- species-role compatibility is data/plan-backed;
- all role numeric effects cite current Plan 151/data;
- pack capacity uses the expedition capacity owner;
- guard behavior does not create a second combat simulation;
- pest control modifies the current spoilage/infestation owner;
- therapy/morale routes through existing social/needs rails;
- feed consumption uses canonical inventory transactions;
- starvation/flee/illness outcomes are source-backed;
- death/grief is exactly-once;
- save/restore recreates role-derived effects;
- focused companion/shelter/expedition/inventory/morale tests are green.

## K.3 E3 merge readiness

E3 is merge-ready only when:

- the current black-market settlement service remains the sole transaction owner;
- legality is a pure item+jurisdiction query;
- item/jurisdiction data validate;
- checkpoint eligibility comes from current route/faction control;
- concealment/detection formulas are source-backed;
- all player choices route through canonical owners;
- confiscation affects only jurisdiction-illegal inventory;
- heat scope/formula/decay/thresholds are Plan 155-backed;
- enforcement/raid scheduling uses one existing dispatcher;
- a threshold transition cannot schedule duplicate raids;
- checkpoint combat and C3 violence consequences do not accidentally duplicate standing;
- derived risk percentages are not persisted;
- legal trade remains baseline-identical;
- deterministic save/replay is green.

## K.4 E4 merge readiness

E4 is merge-ready only when:

- governance owns policy intent rather than operational state;
- decree schema and conflict rules are source-backed;
- legitimacy/approval representation is source-backed;
- council membership/voting consumes canonical authored survivor identity;
- no heuristic ideology system was reintroduced;
- policy adapters use stable contribution IDs;
- labor/ration/medical/security systems remain canonical owners;
- enact/repeal is transactional and exactly-once;
- unrest state/threshold/cooldown semantics are source-backed;
- unrest consequences route through current event/duty/inventory/security owners;
- UI forecast uses the same domain calculation as enactment validation;
- old saves remain neutral;
- operational derived values are not separately persisted;
- focused shelter/social/duty/needs/medical/UI tests are green.

---

# APPENDIX L — PART 3.1 RELEASE NOTE TEMPLATE

# ASHFALL Wave 12 Part 3.1 — Closeout

## E1 — Per-NPC Personal Memory
**Terminal state:**\
**NPC families covered:**\
**Memory authority:**\
**Memory taxonomy:**\
**Retention/cap policy:**\
**Personal stance formula source:**\
**Dialogue integration:**\
**Trade integration:**\
**Faction separation:**\
**Save/replay:**\

## E2 — Working Animals
**Terminal state:**\
**Companion authority:**\
**Role taxonomy:**\
**Species restrictions:**\
**Pack contribution:**\
**Guard contribution:**\
**Pest contribution:**\
**Therapy/bond contribution:**\
**Feed model:**\
**Mortality/grief:**\
**Save/replay:**\

## E3 — Contraband / Enforcement
**Terminal state:**\
**Black-market settlement owner:**\
**Legality/jurisdiction model:**\
**Checkpoint trigger:**\
**Concealment formula:**\
**Heat model:**\
**Escalation states:**\
**Raid owner:**\
**Confiscation:**\
**Standing composition:**\
**Save/replay:**\

## E4 — Shelter Governance
**Terminal state:**\
**Governance owner:**\
**Decree domains:**\
**Legitimacy model:**\
**Council/vote model:**\
**Labor adapters:**\
**Ration adapters:**\
**Medical adapters:**\
**Unrest model:**\
**UI:**\
**Save/replay:**\

## Quality gates

**Build:**\
**Warnings:**\
**Data integrity:**\
**UI lifecycle/a11y:**\
**Focused suites:**\
**Docs/index:**\

## Governance

**Claims:**\
**Census:**\
**Ledger:**\
**Decisions deferred:**\
**Repairs routed:**\

## Part 3.2 handoff

**Final merged HEAD:**\
**F1 dependency impact:**\
**F2 dependency impact:**\
**F3 dependency impact:**\
**F4 dependency impact:**\
**Active claims:**\
**Highest-priority unresolved decision:**\

---

# APPENDIX M — PART 3.2 ENTRY FILTER

Do not begin F1–F4 merely because they are the next four numbered entries.

At final Part 3.1 `HEAD`, record:

| Rank | Corpus | Title | Current premise | E1–E4 dependencies | Claims | Decision | Ready? |
|---:|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

Reverify especially:

### F1 — Shelter Identity
- governance identity/policy state from E4;
- companion/community projection from E2;
- named NPC/community memory from E1 if relevant.

### F2 — Stateful Ambience
- governance/unrest state from E4;
- black-market/security pressure from E3;
- companion/shelter state from E2.

### F3 — Retention / 400-Year Scale
- E1 memory-retention policy;
- E2 companion persistence;
- E3 heat/enforcement retention;
- E4 governance history/state.

### F4 — Standing Gate Compression
- E1 personal-vs-faction separation;
- E3 faction enforcement/standing consequences;
- E4 internal legitimacy vs external faction standing.

Part 3.2 must not collapse these distinct concepts merely because “standing” or “history” appears in several systems.

---

# APPENDIX N — FINAL ACTUALITY HEARTBEAT

Immediately before each E-series task begins, answer:

1. Has the source integration plan changed?
2. Has concurrent work already implemented any clause?
3. Are the draft class/file names current?
4. Is a required path actively claimed?
5. Is the numeric policy still current?
6. Did a prior Wave 12 task already establish the needed owner?
7. Can the proposed save state be derived instead?
8. Does current data contradict any draft category or threshold?
9. Will the implementation create a second authority?
10. Is the package still bounded enough for one claim/merge unit?

If any answer changes scope, update the implementation-log premise before editing production code.

---

# APPENDIX O — FINAL IMPLEMENTER SIGN-OFF

- [ ] E1 retains exactly 20 procedural substeps.
- [ ] E1 has exactly four mini-tasks with four mini-substeps each.
- [ ] E2 retains exactly 20 procedural substeps.
- [ ] E2 has exactly four mini-tasks with four mini-substeps each.
- [ ] E3 retains exactly 20 procedural substeps.
- [ ] E3 has exactly four mini-tasks with four mini-substeps each.
- [ ] E4 retains exactly 20 procedural substeps.
- [ ] E4 has exactly four mini-tasks with four mini-substeps each.
- [ ] Godot remains authoritative.
- [ ] Core remains engine-free.
- [ ] No duplicate personal/faction/social authority was created.
- [ ] No companion role values were invented.
- [ ] No second animal combat or human roster system was created.
- [ ] No black-market transaction authority was duplicated.
- [ ] No contraband/heat/raid thresholds were invented.
- [ ] Governance owns policy rather than operations.
- [ ] No legitimacy/vote/unrest policy was invented.
- [ ] All source-backed numeric values are documented.
- [ ] Exactly-once rules are tested.
- [ ] Save/restore journeys are green.
- [ ] Focused tests precede broader gates.
- [ ] Warning baseline is preserved.
- [ ] Generated docs/indexes are synchronized.
- [ ] Census/ledger/claims agree.
- [ ] Part 3.2 will consume final merged evidence.

---

# APPENDIX P — PART 3.1 TRANSITION GATE

After E1–E4 merge, rerun the corpus census and dependency DAG.

Classify remaining work:

### CONTINUE PART 3.2
Use when one or more F1–F4 nodes remain verified-open and dependency-ready.

### REPAIR FIRST
Use when E1–E4 expose production defects that materially block F-series work.

### DECISION PASS
Use when the highest-value F-series nodes are blocked primarily by unresolved product/architecture policy.

### VERIFIED-RESOLVED / REORDER
Use when concurrent work already sealed an F-series node or changed the ready order.

The final queue is determined by repository actuality—not the historical file index.

---

# FINAL EXECUTION NOTE

The corrected Antigravity Part 3 source establishes a new class of ASHFALL continuity work: **individual NPC memory, persistent working companions, underground-economy enforcement, and shelter governance**.

These are particularly vulnerable to accidental architecture duplication because each resembles an existing system:

- personal memory can accidentally become faction reputation;
- companion roles can accidentally become a second workforce/combat framework;
- contraband heat can accidentally become a second economy/faction/raid manager;
- governance can accidentally become a second duty/nutrition/medical/social simulator.

The safe implementation pattern across all four tasks is therefore:

**canonical state → bounded adapter/projection → existing owner consequence → typed read model → focused evidence.**

Draft thematic numbers and examples are not implementation authority. Every cap, multiplier, decay, probability, threshold, role restriction, council rule, and unrest consequence must be reverified in Plan 147/151/155/159 or current signed data before landing.

**End of Wave 12 Part 3.1 implementation-unblocker plan.**
