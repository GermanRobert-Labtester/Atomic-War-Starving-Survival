---
PLAN_ID: E1-21
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 21
STATUS: READY_FOR_EXECUTION_WHEN_FACTION_AND_REGIONAL_TREATY_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 197 — Faction Diplomacy & Treaty System"
SEQUENCE_FILENAME: "E1_planintegration[21].md"
PREVIOUS_FILENAME: "E1_planintegration[20].md"
NEXT_FILENAMES:
  - "E1_planintegration[22].md"
  - "E1_planintegration[23].md"
CATEGORY: LINK+FACTIONS+DIPLOMACY+TREATIES+MISSIONS+GEOPOLITICS
PRIMARY_INTENT: "Create formal diplomacy, treaty negotiation, diplomatic missions, obligation tracking, and treaty enforcement by composing existing FactionStanceEngine, FactionBranchCoordinator, RegionalTreatySystem, HoldfastTradeSession, Combat, Expedition, E1-3 information, E1-19 trade-route, debt, and survivor-role authorities without introducing a second standing/reputation graph, duplicate treaty owner, combat gate, or economic settlement engine."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
REGIONAL_TREATY_DUPLICATION_FORBIDDEN: true
SECOND_FACTION_STANDING_SYSTEM_FORBIDDEN: true
SECOND_DIPLOMATIC_REPUTATION_GRAPH_FORBIDDEN_BY_DEFAULT: true
SECOND_COMBAT_PERMISSION_ENGINE_FORBIDDEN: true
SECOND_TRADE_TERMS_ENGINE_FORBIDDEN: true
SECOND_INFORMATION_SYSTEM_FORBIDDEN: true
SECOND_DEBT_LEDGER_FORBIDDEN: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
GEOPOLITICAL_RISK: VERY_HIGH
EXPLOIT_RISK: VERY_HIGH
CONTENT_RISK: HIGH
---

# E1 Plan Integration [21] — Faction Diplomacy, Treaty Negotiation, Envoys, Obligations, Violations, Alliances, and Geopolitical State

> **Sequence rule:** this file is `E1_planintegration[21].md`.
> The next files are `E1_planintegration[22].md`, `E1_planintegration[23].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 197 into an implementation-grade faction-diplomacy programme.

The source plan correctly identifies that faction relations need more structure than a single trust value.
ASHFALL already appears to have `FactionStanceEngine` for trust and trade stance,
`FactionBranchCoordinator` for branch-level standing/trust and political progression,
`HoldfastTradeSession` for faction-gated commerce, and—critically—an existing
`RegionalTreatySystem`.

That last point changes the implementation strategy. The first question is not "how do we build a treaty
system?" It is:

**what treaty authority already exists, what does it own, and what is actually missing around player-driven
negotiation, missions, obligations, and enforcement?**

E1-21 therefore starts with an authority audit of `RegionalTreatySystem` and does not permit a second treaty
ledger to appear merely because Plan 197 proposes `FactionDiplomacySystem`.

The intended end state is:

```text
FactionStanceEngine / BranchCoordinator
        |
        +--> standing / trust / trade stance / branch state
        |
        v
Diplomacy orchestration
        |
        +--> proposal eligibility
        +--> envoy mission
        +--> negotiation session
        +--> treaty proposal / counterproposal
        |
        v
Canonical treaty authority
        |
        +--> treaty identity
        +--> active terms
        +--> obligation schedule
        +--> status / expiry / renunciation / violation
        |
        v
Canonical domain consumers
        |
        +--> Combat / hostility policy
        +--> Market / HoldfastTradeSession
        +--> Expedition / passage
        +--> E1-3 information sharing
        +--> E1-19 trade agreements/routes
        +--> Debt / tribute
        +--> Defense / military aid
        +--> Faction branches / geopolitical consequences
```

Diplomacy should orchestrate political agreements, not duplicate every system affected by them.

## 1. Source Intent Preserved

Plan 197 asks for:

- formal treaties;
- envoys and diplomatic missions;
- treaty negotiation;
- accept/reject/counter-proposals;
- treaty obligations;
- treaty expiration and renewal;
- treaty violations;
- non-aggression;
- trade alliances;
- mutual defense;
- intelligence sharing;
- tribute;
- vassalage;
- diplomatic UI;
- events and quests;
- deterministic outcomes;
- old-save compatibility;
- cross-system effects on stance, trade, combat, expeditions, and regional treaties.

E1-21 preserves those goals but imposes stronger ownership rules around trust, regional treaties, combat,
trade, information, and debt.

## 2. Core Architecture Thesis

```text
Player selects faction + diplomatic objective
        |
        v
Proposal eligibility
        |
        +--> canonical faction standing / trust / stance
        +--> branch constraints
        +--> existing treaty compatibility
        +--> envoy availability / qualification
        +--> costs / concessions / obligations
        |
        v
Diplomatic mission / negotiation operation
        |
        +--> travel if required
        +--> proposal terms
        +--> counterproposal rounds
        +--> deterministic evaluation
        |
        v
Canonical treaty commit
        |
        +--> treaty terms
        +--> signatories
        +--> obligations
        +--> duration / expiry
        |
        v
Treaty effect adapters
        |
        +--> faction hostility policy
        +--> trade terms
        +--> military aid
        +--> information access
        +--> tribute/debt
        +--> expedition access
        |
        v
Violation / renewal / renunciation
```

The diplomatic layer owns negotiation and treaty lifecycle only to the extent that `RegionalTreatySystem`
does not already own those facts.

## 3. Architectural Corrections to the Source Plan

### 3.1 Audit RegionalTreatySystem before creating any treaty DTO

Plan 197 explicitly lists `RegionalTreatySystem.cs` for inspection. That means the premise “no treaty system
exists” is already partially contradicted by repository evidence.

The implementation must first determine:

- what treaty types RegionalTreatySystem supports;
- whether its treaties involve factions, regions, settlements, or campaign-wide law;
- whether it already persists active treaties;
- whether it already tracks terms, expiry, violation, signatories, or enforcement;
- whether it can be extended to player-faction treaties;
- whether a shared treaty core should be extracted.

No `FactionDiplomacySystem` may create an authoritative treaty list until this is resolved.

### 3.2 Do not create a second diplomatic relation number

`FactionStanceEngine` already tracks trust and trade stance. `FactionBranchCoordinator` tracks standing/trust.
A new `DiplomaticRelation.relationLevel` and a global `diplomaticReputation` risk creating duplicate or
contradictory political scores.

Preferred approach:

- derive Hostile/Unfriendly/Neutral/Friendly/Allied presentation bands from canonical faction state;
- store treaty-specific reliability/history only if it has distinct semantics;
- do not create global diplomatic reputation by default.

### 3.3 Treaty terms must be typed

A free-form `terms`, `benefits`, `obligations`, and `condition` structure can become an unvalidated scripting
engine. Each treaty term should reference a typed policy consumed by one canonical system.

### 3.4 Non-aggression should not "prevent combat" by bypassing CombatSystem

A non-aggression pact should change faction hostility/engagement authorization through the canonical faction
or encounter policy. Combat still resolves combat when a violation, rogue branch, accident, scripted event,
or hostile actor causes an encounter.

### 3.5 Trade alliances use Market/HoldfastTradeSession

Diplomacy records the treaty term. Market/trade policy calculates tariffs, access, stock, prices, and
agreement effects. Diplomacy does not add a local trade multiplier.

### 3.6 Tribute uses real economic obligations

Tribute is a scheduled payment/obligation through canonical inventory/currency/debt systems. It is not a
daily decrement inside diplomacy state.

### 3.7 Mutual defense requires real aid delivery

An alliance does not mean `aid=true`. Defense/expedition/faction military systems must provide actual units,
resources, access, or response operations.

### 3.8 Intelligence sharing routes through E1-3

The treaty grants an information-source entitlement. E1-3 owns claims, propagation, delays, and knowledge.

### 3.9 Negotiation should be deterministic by default

Standing, strategic value, envoy skill, treaty terms, obligations, faction doctrine, and current geopolitical
state can yield a deterministic acceptance/counter/rejection score. If uncertainty remains, it uses a stable
keyed negotiation decision—not a reloadable success roll.

### 3.10 Vassalage is not just a high-tier treaty

Vassalage changes sovereignty/obligation structure and likely affects faction branch, governance, trade,
tribute, military control, quest state, and perhaps endings. It should be feature-gated behind a separate
vassalage ADR and not be in the first vertical slice.

## 4. Non-Negotiable Rules

- `FactionStanceEngine` remains canonical for faction trust/trade stance unless an existing authority says
  otherwise.
- `FactionBranchCoordinator` remains canonical for branch standing/progression/point-of-no-return.
- `RegionalTreatySystem` must be audited before a new treaty owner is introduced.
- Exactly one authoritative treaty record exists for any agreement.
- Diplomacy does not maintain a duplicate faction relation level as simulation truth.
- Diplomacy does not maintain a second global political reputation by default.
- Trade terms are resolved through Market/HoldfastTradeSession/E1-19.
- Intelligence sharing is resolved through E1-3.
- Military aid is resolved through defense/faction/expedition authorities.
- Tribute/debt are resolved through canonical economy/LedgerDebt.
- Non-aggression changes hostility/engagement policy through faction/encounter authority.
- CombatSystem still owns combat resolution.
- Expedition owns travel/movement.
- E1-20 Diplomat role may qualify or improve negotiation through typed diplomacy capability, but does not own
  negotiation state.
- Every diplomatic mission has a stable operation ID.
- Envoys are canonical survivors, not copied DTOs.
- Envoys remain unavailable to incompatible duties while traveling/negotiating.
- Negotiation proposals are immutable value objects.
- Counterproposals are versioned/round-indexed and cannot be duplicated after reload.
- Treaty commitment is transactional.
- Treaty term activation occurs only after treaty commit.
- Treaty obligations are explicit scheduled commitments.
- Obligation fulfillment is proven by canonical transaction/event receipts.
- Violations are not inferred twice by both treaty and domain systems.
- Treaty consequences route through canonical faction/market/combat/debt/quest authorities.
- Renunciation is distinct from accidental violation.
- Treaty expiry is distinct from breach.
- Treaty suspension is distinct from termination.
- Old saves do not receive fabricated treaties.
- Old saves preserve existing faction standing/trust exactly.
- If RegionalTreatySystem already has active treaties, migration must preserve them.
- Auto-renew does not silently spend resources or accept changed terms without a policy.
- No daily scanning of every treaty obligation if due-time scheduling can be used.
- Treaty history is bounded/aggregated.
- First release should prove non-aggression + trade/intelligence treaty before mutual defense/tribute/vassalage.

## 5. Acceptance Slices

### Slice A — Treaty authority reconciliation
Audit/extend RegionalTreatySystem and establish one authoritative treaty model.

### Slice B — Negotiation + Non-Aggression Pact
Envoy proposes terms, faction evaluates, treaty commits, hostility policy consumes it.

### Slice C — Trade or Intelligence treaty
One typed treaty effect routes to Market/E1-3.

### Slice D — Obligations and violation
One scheduled obligation is fulfilled or missed with canonical receipts.

### Slice E — Renewal, defense, tribute, advanced geopolitics
Only after the base treaty lifecycle is stable.

Do not ship six treaty types before the authority audit passes.


---

## E1-21A — Premise verification and diplomacy authority audit

**Goal:** Verify FactionStanceEngine, FactionBranchCoordinator, RegionalTreatySystem, trade, combat, expedition, debt, information, survivor-role, save, and geopolitical authorities before adding diplomacy state.

### Required substeps

1. Read `FactionStanceEngine.cs` end-to-end and document trust/trade stance semantics, ranges, mutation APIs, save ownership, and downstream consumers.
2. Read `FactionBranchCoordinator.cs` end-to-end and document standing, branch relations, point-of-no-return behavior, and faction event hooks.
3. Read `RegionalTreatySystem.cs` end-to-end and document every treaty DTO, term, lifecycle, save field, API, and consumer.
4. Read `HoldfastTradeSession.cs` and current Market trade-term policies.
5. Inspect Combat/Encounter hostility checks, Expedition travel/access, E1-3 information sharing, E1-19 trade agreements, LedgerDebt, shelter defense/military aid, and E1-20 Diplomat role capability.
6. Search for treaty, pact, alliance, ceasefire, embargo, tribute, aid, safe passage, envoy, negotiation, counteroffer, signatory, violation, betrayal, vassalage, diplomatic immunity, and faction agreement code.
7. Identify all existing treaty-like data catalogs and localization.
8. Determine whether RegionalTreatySystem can be extended or should share a lower-level `TreatyRegistry`.
9. Create `docs/systems/DIPLOMACY_TREATY_AUTHORITY_MAP.md`.
10. Create intake duplicate-search evidence linking Plans 131/E1-3, 134 territory, 138 defense, 139 standing bridge, 160 colony, E1-19 trade routes, E1-20 roles, and any governance/debt plans.
11. Set `PREMISE_VERIFIED_AT` to current HEAD and block new treaty DTO implementation until ownership is resolved.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21B — RegionalTreaty reconciliation ADR

**Goal:** Decide whether RegionalTreatySystem becomes the canonical treaty authority, is extended, or yields a shared treaty core.

### Required substeps

1. Compare direct extension of RegionalTreatySystem, shared `TreatyRegistry` used by regional and faction diplomacy, and a narrow diplomacy facade over existing treaty APIs.
2. Define one authoritative treaty ID namespace.
3. Define one treaty status vocabulary.
4. Define one term schema family.
5. Define one obligation/violation ownership path.
6. Define migration of any existing RegionalTreaty saves.
7. Do not keep parallel `regionalTreaties` and `factionTreaties` if the same semantic agreement can exist in both.
8. Require second-tool architecture review.
9. Document why the chosen design avoids duplicate treaty truth.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21C — Faction diplomatic-state boundary

**Goal:** Use canonical faction trust/standing as political state and store only treaty-specific diplomacy facts.

### Required substeps

1. Derive presentation bands such as Hostile/Unfriendly/Neutral/Friendly/Allied from canonical FactionStance/Branch state if useful.
2. Do not persist a second `relationLevel` unless it has unique semantics.
3. Do not copy trust/standing into diplomacy save.
4. Define treaty eligibility queries over canonical state.
5. Define alliance as treaty/relationship status, not simply trust >= 50.
6. Allow high trust with no formal alliance.
7. Allow formal treaty at moderate trust if policy permits.
8. Add consistency tests.
9. Document faction-state query contract.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21D — Diplomatic reliability/reputation ADR

**Goal:** Reject a global diplomatic reputation unless repository evidence proves a distinct need.

### Required substeps

1. Audit all existing reputation/standing/trust/reliability concepts.
2. Prefer treaty-performance history plus faction-specific standing.
3. If factions should know about the player's general treaty reliability, route that knowledge through E1-3 or a narrowly scoped `treaty_reliability` profile with explicit owner/consumers.
4. Do not create a universal 0–100 score by default.
5. Do not reduce reputation because a mission merely failed unless the faction interprets it negatively through canonical policy.
6. Define cross-faction betrayal propagation only through information/knowledge.
7. Add boundary tests.
8. Require ADR amendment before new global reputation state.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21E — Treaty identity and lifecycle

**Goal:** Define stable treaty identity and status transitions shared with RegionalTreatySystem.

### Required substeps

1. Define treaty ID, treaty template/type ID, parties, signed day, effective day, expiry/term policy, status, signatory refs, term refs, obligation refs, negotiation provenance, and history refs.
2. Use parties as canonical shelter/faction/settlement IDs.
3. Define Draft/Proposed/Active/Suspended/Expired/Violated/Renounced/Terminated only if each has real behavior.
4. Do not encode `violated` and `active` ambiguously.
5. Preserve historical treaty identity after expiry/renunciation.
6. Use stable IDs across save/load.
7. Add transition tests.
8. Keep active state minimal.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21F — Treaty template schema

**Goal:** Define treaty types as composable typed terms rather than unvalidated benefit strings.

### Required substeps

1. Define template ID, localization, eligible party types, eligibility policy, allowed/required term policy IDs, duration policy, negotiation policy, violation policy, renewal policy, incompatibility tags, and feature flags.
2. Do not embed direct combat/trade/intel mutations.
3. Validate term-policy consumers.
4. Version template semantics.
5. Start with non-aggression, trade preference, and intelligence sharing.
6. Defer vassalage.
7. Add data-integrity tests.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21G — Treaty term schema

**Goal:** Represent each diplomatic obligation/benefit with a typed consumer-owned policy.

### Required substeps

1. Define term ID, term policy ID, responsible party/beneficiary, effective window, parameters, condition trigger refs, obligation schedule ref if any, and termination behavior.
2. Possible policy classes: non_aggression, market_tariff_policy, trade_access_policy, intelligence_access, military_aid_commitment, tribute_payment, territory_access, safe_passage.
3. Do not use arbitrary `description/value/condition` as simulation authority.
4. Human-readable description is localization/presentation only.
5. Validate all parameter ranges.
6. Add term-contract tests.
7. Reject unknown consumers.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21H — Treaty compatibility matrix

**Goal:** Prevent contradictory treaties and ambiguous geopolitical states.

### Required substeps

1. Define incompatibility rules: non-aggression with active war/hostility state, mutual defense with certain enemy alliances, exclusive trade conflicts, vassalage conflicts, embargo conflicts, branch-specific restrictions.
2. Ask canonical faction/branch systems for hard blockers.
3. Do not resolve contradictions by last-write-wins.
4. Define suspension versus termination behavior when world state changes.
5. Add graph/compatibility tests.
6. Keep treaty template rules data-driven.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21I — Diplomatic mission contract

**Goal:** Represent envoy missions as canonical travel/operation state rather than a parallel survivor copy.

### Required substeps

1. Define mission ID, mission type, target faction/settlement, assigned envoy survivor ID, objective/proposal ID, travel operation ref, negotiation session ref, status, created/start/completion times, and outcome ref.
2. Do not copy envoy stats/health.
3. Use Expedition/Travel for physical movement where required.
4. Use Duty/Autonomy for survivor availability.
5. Define local/remote mission if communications permit.
6. Add save/load and death/interruption tests.
7. Keep mission lifecycle idempotent.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21J — Envoy eligibility and assignment

**Goal:** Use canonical survivor role/skill/certification/availability for diplomatic missions.

### Required substeps

1. Audit social/diplomacy skills and E1-20 Diplomat role.
2. Role can satisfy/modify envoy eligibility through typed capability.
3. Validate health/fatigue/current duty through canonical systems.
4. Do not require Diplomat role if another qualified survivor can negotiate unless design says so.
5. Move envoy into travel/mission state through canonical authority.
6. Prevent double assignment.
7. Add tests for qualified role-holder, skilled non-role survivor, unavailable envoy, injured envoy, and reassignment.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21K — Mission travel boundary

**Goal:** Use Expedition/world travel for envoy movement instead of diplomacy timers.

### Required substeps

1. Resolve route/path through world topology.
2. Expedition/Travel owns travel duration, hazards, arrival, injury, death, and return.
3. Diplomacy owns only mission objective and negotiation state.
4. Do not add arbitrary `durationDays = travel + negotiation` without real travel resolution.
5. Allow escorted envoy if current systems support it.
6. Add tests for safe travel, blocked route, hostile territory, delayed arrival, death, and return.
7. Keep mission pending while travel is in progress.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21L — Negotiation proposal value object

**Goal:** Make treaty proposals immutable, versioned, and auditable.

### Required substeps

1. Define proposal ID, treaty template ID, parties, proposed terms, proposer, envoy, created day, negotiation round, parent proposal/counterproposal ID, and expiry.
2. Do not mutate an accepted historical proposal.
3. Validate terms before evaluation.
4. Use deterministic canonical ordering.
5. Add proposal serialization tests.
6. Support counterproposal chain.
7. Bound negotiation history.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21M — Faction proposal evaluation policy

**Goal:** Evaluate treaties from canonical political state, strategic value, faction doctrine, obligations, and envoy capability.

### Required substeps

1. Inputs: FactionStance trust/stance, BranchCoordinator state, faction doctrine, current wars/allies/treaties, market/resource needs if canonical, territory concerns, treaty reliability evidence if approved, envoy capability, and proposed terms.
2. Return accept/reject/counter plus reason breakdown.
3. Do not mutate faction standing during pure evaluation.
4. Do not use a reloadable generic success chance.
5. Use deterministic score/threshold first.
6. Add golden tests for representative factions.
7. Keep hidden strategic data separate from player-visible estimate.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21N — Keyed uncertainty gate

**Goal:** If negotiations need uncertainty, isolate it behind a deterministic keyed decision.

### Required substeps

1. Use key: campaign seed + mission/proposal ID + faction ID + round + policy version.
2. Persist decision/result if policy changes could affect saves.
3. Do not consume shared global RNG.
4. Do not reroll on reload or reopen negotiation UI.
5. Use uncertainty only around genuinely ambiguous outcomes.
6. Add call-order/no-reroll tests.
7. Document when deterministic threshold versus keyed variation applies.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21O — Counterproposal mechanics

**Goal:** Allow factions to request altered terms without creating an unbounded negotiation minigame.

### Required substeps

1. Define term adjustment rules per faction/template.
2. Counterproposal is a new immutable proposal linked to prior proposal.
3. Limit negotiation rounds or concession budget.
4. Show exact changed terms.
5. Do not let UI alter hidden faction state.
6. Use envoy capability to improve information/available concessions rather than simply adding chance.
7. Add accept/reject/counter/counter-loop tests.
8. Bound history.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21P — Treaty commit transaction

**Goal:** Activate a treaty exactly once after both sides accept validated terms.

### Required substeps

1. Validate latest proposal accepted by both parties.
2. Validate faction/branch state has not become incompatible.
3. Validate any signing cost/concession escrow.
4. Create canonical treaty record.
5. Activate term adapters.
6. Create obligation schedules.
7. Record signatory/envoy provenance.
8. Commit economic costs once.
9. Emit TreatySigned after commit.
10. Add double-submit/reload/idempotency tests.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21Q — Signing concessions and consideration

**Goal:** Route resources, payments, access, or one-time concessions through canonical systems.

### Required substeps

1. Inventory/currency owns one-time gifts/payments.
2. Territory/access authority owns passage/land rights.
3. Debt owns financed concessions.
4. Market owns trade stock/terms.
5. Do not store payment balances in Treaty.
6. Reserve/consume concessions transactionally.
7. Handle failure after reservation.
8. Record canonical transaction IDs.
9. Add conservation tests.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21R — Non-aggression treaty adapter

**Goal:** Implement one non-aggression pact through canonical hostility/encounter policy.

### Required substeps

1. Define which faction actors/branches are covered.
2. Register a treaty authorization/constraint with FactionStance/Encounter policy.
3. Do not disable CombatSystem.
4. Define exceptions: self-defense, rogue actors, scripted breach, third-party disguise only if supported.
5. Attacking treaty party creates a violation from canonical combat/hostility event.
6. Handle branch split/control changes.
7. Add tests for normal encounter, player attack, faction attack, rogue branch, expiry, and renunciation.
8. Use this as first treaty vertical slice.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21S — Trade-alliance treaty adapter

**Goal:** Route preferential trade through Market/HoldfastTradeSession/E1-19 terms.

### Required substeps

1. Define tariff/access/quota/priority policies.
2. Market calculates prices/fees.
3. Do not add a diplomacy-side price multiplier.
4. E1-19 trade routes may consume treaty entitlement.
5. Handle market closure/embargo/standing change.
6. Define exclusivity only if canonical policy supports it.
7. Add tests for active/expired/suspended treaty and trade settlement.
8. Keep Market authoritative.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21T — Intelligence-sharing treaty adapter

**Goal:** Grant information-source permissions through E1-3.

### Required substeps

1. Define what information classes are shared.
2. Define delay/reliability/source identity.
3. Faction treaty grants access; E1-3 owns propagation/claims/knowledge.
4. Do not copy intel into treaty state.
5. Do not grant perfect world truth unless policy explicitly says so.
6. Handle treaty suspension/expiry by disabling future flow while retaining already learned information.
7. Add tests for delayed intel, stale intel, expiry, misinformation if faction system supports it.
8. Use stable provenance.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21U — Mutual-defense treaty gate

**Goal:** Implement only after canonical military-aid dispatch and defense handoff exist.

### Required substeps

1. Audit faction reinforcement/defense/expedition systems.
2. Define aid-request trigger and obligation.
3. Define what counts as attack covered by treaty.
4. Ally chooses/owes response according to treaty and faction capability.
5. Actual aid is a real dispatch/resource/force.
6. Combat/Defense owns battle result.
7. Do not directly add defense strength from treaty.
8. Add tests for available/unavailable ally, delayed aid, partial aid, refusal/violation, and treaty expiry.
9. Feature-gate until rails are real.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21V — Tribute agreement gate

**Goal:** Use scheduled canonical economic obligations and protection terms.

### Required substeps

1. Define tribute item/currency, quantity/formula, cadence, grace period, and beneficiary.
2. Create obligation due events through scheduler.
3. Payment uses Inventory/Economy/LedgerDebt.
4. Protection uses faction/security authority.
5. Do not subtract tribute from diplomacy state directly.
6. Define insufficient funds behavior.
7. Add payment/late/missed/partial tests.
8. Feature-gate until obligation system is stable.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21W — Vassalage ADR gate

**Goal:** Treat vassalage as a high-impact sovereignty system requiring separate approval.

### Required substeps

1. Identify governance, tribute, military command, territory, faction orders, endings, and branch consequences.
2. Do not implement as `tribute + protection` only.
3. Define player agency and exit/renunciation conditions.
4. Define whether vassalage changes shelter sovereignty or faction membership.
5. Require separate ADR and narrative review.
6. Keep template disabled until approved.
7. Add no-op data validation for disabled template.
8. Do not block base diplomacy release on vassalage.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21X — Obligation scheduler

**Goal:** Track treaty commitments by due events instead of scanning every term every day.

### Required substeps

1. Define obligation ID, treaty ID, term ID, responsible party, due window, fulfillment policy, grace period, status, and evidence refs.
2. Use campaign time.
3. Schedule next due time.
4. Only evaluate due obligations.
5. Advance recurrence after fulfillment/miss according to policy.
6. Do not generate duplicate obligations after reload.
7. Add 1/7/30/100-day time-skip tests.
8. Keep due queue indexed.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21Y — Obligation fulfillment receipts

**Goal:** Prove treaty obligations through canonical transaction/action evidence.

### Required substeps

1. Tribute uses economy transaction ID.
2. Military aid uses dispatch/arrival/action ID.
3. Intelligence sharing uses entitlement/service evidence if needed.
4. Trade quota uses Market/E1-19 settlement refs.
5. Territory access may be passive and need no fulfillment receipt.
6. Do not mark fulfilled merely because diplomacy tick says so.
7. Deduplicate evidence.
8. Add tests for exact/late/partial fulfillment.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21Z — Treaty violation detection

**Goal:** Create violations only from explicit failed obligations or prohibited canonical events.

### Required substeps

1. Violation sources: canonical attack event, missed due obligation after grace period, explicit forbidden trade/embargo action, unauthorized territory breach, deliberate renunciation if policy treats it separately, or other typed term breach.
2. Do not infer generic violation from low trust.
3. Use stable violation ID.
4. Record violator/victim/term/evidence.
5. Prevent double detection from combat + diplomacy listeners.
6. Add source-specific tests.
7. Keep violation immutable.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AA — Violation consequence handoff

**Goal:** Route political, trade, debt, military, and narrative consequences through canonical authorities.

### Required substeps

1. FactionStance/BranchCoordinator receives treaty-breach context.
2. Market/TradeStance may revoke access through canonical policy.
3. Debt/payment obligations remain in LedgerDebt.
4. Combat/war escalation occurs through faction/hostility policy.
5. E1-3 propagates betrayal information to other factions if appropriate.
6. Do not subtract a generic reputation score unless approved.
7. Add tests for local-only breach versus widely known breach.
8. Use provenance to prevent duplicate standing changes.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AB — Renunciation contract

**Goal:** Allow voluntary treaty exit without conflating it with accidental breach.

### Required substeps

1. Define renunciation eligibility, notice period, exit cost, treaty-specific penalty, and effective date.
2. Some treaties may permit clean expiry/non-renewal.
3. Renunciation emits its own event.
4. Faction consequences use canonical policy.
5. Do not automatically classify every renunciation as betrayal.
6. Handle active obligations in notice period.
7. Add tests for immediate/notice-period/clean non-renewal.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AC — Expiration and renewal

**Goal:** Handle treaty term endings deterministically and visibly.

### Required substeps

1. Schedule expiry event.
2. Disable future treaty effects at effective expiry.
3. Preserve history.
4. Allow renewal proposal before expiry.
5. Auto-renew only if terms are unchanged or policy explicitly allows automatic acceptance.
6. Do not auto-spend concessions/resources without policy.
7. Handle faction-state incompatibility.
8. Add expiry/renewal/reload tests.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AD — Treaty suspension

**Goal:** Allow temporary inability to perform a treaty without deleting it.

### Required substeps

1. Define suspension reasons: faction civil split, inaccessible territory, temporary embargo, disputed violation, missing authority, war with a third party if template says so.
2. Suspension stops selected effects/obligations according to policy.
3. Do not use suspension to erase debt or breach history.
4. Resume deterministically when condition resolves.
5. Add tests for suspension/resume/expiry while suspended.
6. Keep state explicit.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AE — Faction branch integration

**Goal:** Ensure treaties interact with Military/Rebel/Independent branches through BranchCoordinator rather than flattening factions.

### Required substeps

1. Determine whether treaty binds whole faction or one branch.
2. Define signatory authority.
3. Branch split may invalidate, inherit, contest, or selectively honor treaty according to policy.
4. Point-of-no-return may block negotiations.
5. Do not copy branch standing into diplomacy.
6. Add tests for branch coup/split/rebel takeover and treaty continuity.
7. Document party-scope semantics.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AF — Combat standing bridge integration

**Goal:** Integrate Plan 139 alliance-offer consequences as diplomacy invitations rather than instant hidden alliances.

### Required substeps

1. Combat consequence may generate `TreatyOfferAvailable`/diplomatic opportunity.
2. Do not immediately create alliance unless the faction policy explicitly produces a signed unilateral agreement.
3. Player may send envoy/negotiate terms.
4. Faction standing remains canonical.
5. Add tests for combat-earned offer, ignored offer, expired offer, accepted treaty.
6. Use stable opportunity ID.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit/economy/performance evidence is captured.

---

## E1-21AG — Shelter defense envoy integration

**Goal:** Use Plan 138 diplomatic visits as mission/negotiation entry points.

### Required substeps

1. Incoming faction envoy may open a negotiation session.
2. Do not duplicate visitor/arrival system.
3. Treaty proposal uses same canonical negotiation model.
4. Security/defense controls envoy access if relevant.
5. Add tests for friendly envoy, hostile/suspicious envoy, active treaty renewal visit, and rejected meeting.
6. Keep visitor state external.

### Diplomacy invariants

- FactionStanceEngine and BranchCoordinator remain canonical for faction standing/trust/branch state.
- RegionalTreatySystem is reconciled before any new treaty authority is introduced.
- Exactly one treaty record owns each signed agreement.
- Treaty terms delegate effects to Market, Combat/hostility, E1-3, Debt, Expedition, Defense, or other canonical consumers.
- Envoys remain canonical survivors and travel through canonical movement.
- Negotiation and obligation outcomes are deterministic/idempotent across save/load.
- Treaty violations require explicit evidence and apply consequences once.
- Old saves preserve all existing faction and regional-treaty state.

### Negative tests

- Diplomacy stores copied trust/standing as a second relationship authority.
- FactionDiplomacy and RegionalTreatySystem both persist the same treaty.
- A non-aggression pact disables CombatSystem globally.
- A trade treaty applies a second price multiplier after Market settlement.
- An intel treaty copies world knowledge directly into diplomacy state.
- A tribute tick subtracts currency in diplomacy code and also creates debt/payment transaction.
- Reload rerolls negotiation or repeats a treaty violation.
- Old-save migration resets factions to neutral or fabricates treaties.

### Acceptance evidence

- [ ] Treaty-authority reconciliation tests pass.
- [ ] Faction-state boundary tests pass.
- [ ] Mission/negotiation/save tests pass.
- [ ] Term-adapter integration tests pass.
- [ ] Obligation/violation idempotency passes.
- [ ] Old-save migration passes.
- [ ] Exploit

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
