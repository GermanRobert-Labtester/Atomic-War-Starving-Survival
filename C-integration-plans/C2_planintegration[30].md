# C2 — Flagship Integration Plan [30]: Radiation as an Economic and Social Status, Contamination-Aware Trade, and Deterministic Stigma/Trust Projection

> **Deliverable:** `C2_planintegration[30].md`
> **Source scope:** Plan 146 — *Radiation → Economy & Social Bridge*
> **Primary objective:** make radiation and contamination matter beyond health by projecting canonical survivor dose/radiation state and canonical item contamination state into market valuation, trade restrictions, faction reactions, social interactions, quest availability, decontamination choices, and player-facing explanation—without introducing a second persisted price ledger, standing ledger, affinity system, or radiation state.
> **Required execution order:** **146A Foundation/System Contract → 146B Economy/Social/Quest Content → 146C Integration, Save/CI, Fairness, and Player-Facing Closure**
> **Hard dependencies:** `RadiationSystem`, `DoseLedgerSystem`, `DecontaminationSystem`, item contamination ownership, `MarketSystem`, `HoldfastTradeSession`, `FactionStanceEngine`, `SurvivorRelationsSystem`, Plan 31 semantic events, Plan 35/36 effect-port discipline, Plan 39 save durability, Plan 44 relation explainability, Plan 46 balance evidence, Plan 55 retention.
> **Scope discipline:** no duplicate survivor-radiation state, no duplicate item-contamination state, no stored market price modifier that can drift from current contamination, no random discrimination roll where a deterministic rule/existing event system can express the consequence, no standing/affinity mutation outside canonical APIs, no “clean from hot zone” penalty unless provenance/origin is actually tracked, no contaminated-goods exploit based on hidden information without a buyer-knowledge contract, and no social penalty that makes decontamination irrelevant or produces unavoidable permanent lockout.

---

# 0. Executive Intent

ASHFALL already models radiation as a physical problem.

It can:

- accumulate dose,
- track dose history,
- apply health effects,
- degrade worn gear,
- support decontamination,
- expose radiation-related state to the player.

What it does not yet model is the fact that radiation also changes how goods and people are treated by others.

Current conceptual shape:

```text
radiation
→ health
→ gear degradation
```

Target shape:

```text
canonical radiation / contamination facts
              │
              ├────────────► economy projection
              │                ├─ valuation
              │                ├─ restriction
              │                └─ verification / decontamination
              │
              └────────────► social projection
                               ├─ interaction caution
                               ├─ faction policy
                               ├─ relation reactions
                               └─ quest/encounter availability
```

The bridges are deliberately narrow.

They should answer:

```text
Given the current radiation/contamination facts and the counterparty's policy,
what modifier/restriction/reaction applies right now?
```

They should not own:

- dose,
- contamination,
- price history,
- faction standing,
- affinity,
- morality,
- quest state.

The strongest product outcome is:

> **A contaminated shipment can be rejected, discounted, verified clean, decontaminated, or knowingly sold under an ethical risk; an irradiated survivor can face understandable faction/social consequences that improve after decontamination; and every penalty is explainable from current state rather than hidden arbitrary punishment.**

---

# 1. Source Diagnosis

The source establishes:

- `RadiationSystem` changes health and gear but not trade/social behavior,
- `RadiationAfflictionHandlers` already bridge dose into afflictions,
- `DecontaminationSystem` can reduce radiation,
- `DoseLedgerSystem` already tracks survivor dose history,
- no economy path references radiation as a trade modifier,
- no social path queries radiation state,
- proposed economy effects include:
  - contaminated goods discounts,
  - faction-specific refusal,
  - suspicion toward goods from irradiated zones,
  - scarcity premium for verified clean goods,
- proposed social effects include:
  - survivor social penalties by radiation band,
  - trader/faction refusals,
  - faction trust reactions,
  - social events,
- 20 economy modifiers and 15 social modifiers are expected,
- old-save compatibility and deterministic headless testing are required.

The source proposes persisting a `RadiationBridgeState` containing item/survivor modifiers.

That should be rejected unless a field is genuinely stateful.

The preferred architecture is:

```text
current canonical state
+ authored policy
→ pure bridge query
```

Persist only state that represents real historical/knowledge facts, such as:

- a verification certificate,
- knowingly deceptive sale consequence,
- remembered faction incident,
- active temporary sanction,

if those are not already owned elsewhere.

---

# 2. Program-Level Success Criteria

C2[30] closes only when all of the following are true.

1. Market valuation changes when actual item contamination changes.
2. Trade blocking follows buyer/faction policy, not hardcoded UI rules.
3. Decontamination immediately changes downstream trade eligibility/valuation.
4. Survivor radiation state affects social/faction interactions through canonical modifiers.
5. Decontamination can reduce/remove applicable social penalties.
6. No duplicate survivor radiation meter exists in the bridge.
7. No duplicate item contamination field exists in the bridge.
8. No duplicate standing/affinity ledger exists.
9. Modifier calculations are deterministic.
10. UI explains the exact reason for price/restriction/social change.
11. If buyer awareness matters, it is modeled explicitly.
12. Selling contaminated goods deceptively is a moral/relationship/faction action, not a silent arbitrage exploit.
13. “Clean goods from hot zone” is only implemented if item provenance/origin exists.
14. Faction policies are data-authored.
15. 20 economy modifier definitions and 15 social policy/modifier definitions validate.
16. Old saves require no redundant bridge-state migration.
17. Save/load preserves only non-derivable incidents/certifications if introduced.
18. No-radiation baseline produces no modifiers.
19. All-survivors-irradiated stress case remains playable and recoverable.
20. Headless CI proves price, refusal, decontamination, social reaction, and save/load behavior.

---

# 3. Architectural Invariants

## 3.1 RadiationSystem owns survivor radiation

Bridge reads it.

Bridge never writes/duplicates it.

## 3.2 Canonical item state owns contamination

If item contamination is not yet canonical, this plan must identify the real owner/prerequisite before implementing trade effects.

## 3.3 MarketSystem owns final price

RadiationEconomyBridge supplies one modifier/restriction term.

## 3.4 HoldfastTradeSession owns transaction enforcement

Bridge says:

```text
allowed / blocked / requires disclosure / adjusted valuation
```

Trade session applies.

## 3.5 FactionStanceEngine owns faction trust/standing

Bridge supplies contextual radiation reaction.

## 3.6 SurvivorRelationsSystem owns affinity/trust/resentment

Bridge supplies a reason/modifier via canonical API.

## 3.7 Decontamination changes source facts

Downstream modifiers recompute.

No manual “clear bridge penalty” mutation.

## 3.8 Pure calculations are preferred

No RNG in baseline modifier projection.

If encounter/dialogue variation later uses RNG, it must be seeded and outside the core modifier.

## 3.9 Knowledge/provenance is separate from contamination truth

Buyer may know or not know an item is contaminated.

That is a knowledge state, not contamination state.

## 3.10 Social consequences must remain moderate and reversible where source state is reversible

No permanent stigma from a temporary radiation reading unless a separate historical incident justifies it.

---

# 4. Dependency Graph

```text
RadiationSystem ───────────────┐
DoseLedger ────────────────────┤
Item contamination authority ─┤
DecontaminationSystem ─────────┤
                               ▼
                 Radiation Bridge Projection
                    ┌──────────┴──────────┐
                    ▼                     ▼
            Economy / Trade          Social / Faction
                    │                     │
          ┌─────────┼────────┐      ┌─────┼─────────┐
          ▼         ▼        ▼      ▼     ▼         ▼
        price     block   disclosure standing relations quests
```

Supporting contracts:

```text
Plan 31 events ─────────► explainable incident attribution
Plan 35/36 ports ───────► bound sinks
Plan 44 relation history ► social reason visibility
Plan 46 balance ────────► penalty bands
Plan 55 retention ──────► bounded incident logs
```

---

# 5. Baseline Capture

Before implementation, record:

- RadiationSystem query APIs,
- dose units and thresholds,
- DoseLedger fields,
- DecontaminationSystem effect path,
- item contamination representation, if any,
- item provenance/origin representation, if any,
- MarketSystem price composition,
- trade-session buy/sell validation,
- faction policy/standing APIs,
- relation modifier APIs,
- quest trigger APIs,
- UI trade price explanation support,
- existing radiation warnings/tooltips.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture baseline for:

```text
clean survivor
moderately irradiated survivor
heavily irradiated survivor
clean item
contaminated item if supported
```

---

# 6. Workstream 146A — Foundation / System Contract

## Goal

Create pure, typed radiation-to-economy and radiation-to-social projections backed by canonical state and authored policy.

---

# 7. 146A Phase A — `RadiationEconomyBridge`

Create:

```text
Assets/Ashfall.Core/Radiation/RadiationEconomyBridge.cs
```

Responsibilities:

- read item contamination,
- read buyer/faction policy,
- read verification/provenance if available,
- calculate trade modifier/restriction,
- return explanation terms.

No final price mutation inside the bridge.

---

# 8. 146A Phase B — `RadiationSocialBridge`

Create:

```text
Assets/Ashfall.Core/Radiation/RadiationSocialBridge.cs
```

Responsibilities:

- read survivor radiation/dose band,
- read counterparty/faction policy,
- produce social/faction reaction modifier,
- produce explanation terms.

No standing/affinity mutation inside the bridge.

---

# 9. 146A Phase C — Economy Result Type

Prefer:

```text
RadiationTradeAssessment
{
    item_id
    contamination_band
    buyer_policy_id
    allowed
    disclosure_required
    price_multiplier
    reason_codes
    verification_state
}
```

No saved modifier map.

---

# 10. 146A Phase D — Social Result Type

Prefer:

```text
RadiationSocialAssessment
{
    survivor_id
    radiation_band
    counterparty_policy_id
    interaction_modifier
    trade_allowed
    trust_modifier
    reason_codes
}
```

Again, derived.

---

# 11. 146A Phase E — Radiation Bands

Source baseline:

```text
<20 mSv
20–50
50–100
>100
```

Before adopting, verify actual unit semantics in `RadiationSystem`.

Do not force source thresholds if repository units differ.

Use canonical bands such as:

```text
None
Low
Moderate
High
Severe
```

with data-authored thresholds.

---

# 12. 146A Phase F — Social Penalty Bands

Source seed values:

```text
low: 0
moderate: ~-10
high: ~-25
severe: ~-50
```

Treat as balance config, not hardcoded constants.

---

# 13. 146A Phase G — Remove Random “Discrimination Chance” From Core Projection

The source proposes 5/15/30% discrimination chance.

Prefer deterministic policy rules for baseline interaction effects.

If an event/encounter needs probabilistic discriminatory behavior:

- put it in event generation,
- use seeded RNG,
- preserve explicit cause,
- do not make the core social modifier random.

---

# 14. 146A Phase H — Item Contamination Prerequisite Audit

Before writing economy logic, confirm canonical item contamination exists.

Possible outcomes:

## Exists

Use it directly.

## Exists as environment/container contamination

Define adapter.

## Does not exist

Create or depend on one canonical item-contamination authority first.

Do not fake contamination using item location/name.

---

# 15. 146A Phase I — Item Contamination Bands

Suggested:

```text
Clean
Trace
Contaminated
High
Severe
```

Thresholds in data.

---

# 16. 146A Phase J — Item Origin/Provenance Audit

Source includes:

```text
clean goods from irradiated zone → suspicion
rare clean goods from hot zone → premium
```

Only implement if item provenance/origin is actually tracked.

Otherwise:

```text
defer origin-based modifiers
```

Do not infer hot-zone origin from item ID.

---

# 17. 146A Phase K — Buyer Knowledge Model

For contaminated goods, distinguish:

```text
contamination truth
buyer knowledge
seller disclosure
verification
```

This is essential for “sell to unaware trader” morality.

---

# 18. 146A Phase L — Verification State

Potential:

```text
Unverified
Scanned
VerifiedClean
VerifiedContaminated
DisclosedBySeller
```

Authority may belong to trade/item metadata depending architecture.

---

# 19. 146A Phase M — Faction Radiation Policy

Create data-authored policy.

Example fields:

```text
policy_id
faction_id
max_item_contamination
max_survivor_radiation
price_curve
trade_block_rules
verification_requirements
social_reaction_curve
treatment_specialty
```

---

# 20. 146A Phase N — Policy Archetypes

Source concepts:

```text
health_conscious
military_clean_equipment
desperate_acceptance
standing_first
treatment_specialist
```

Prefer policy IDs to hardcoded faction names.

---

# 21. 146A Phase O — `IRadiationEconomySink`

If the project uses ports, expose bridge query to economy/trade.

Do not invert ownership so market calls back into UI.

---

# 22. 146A Phase P — `IRadiationSocialSink`

Expose assessment to faction/social systems.

---

# 23. 146A Phase Q — Explanation Codes

Every modifier returns stable reason codes:

```text
item_contamination
buyer_health_policy
verified_clean
seller_disclosed_contamination
survivor_high_radiation
treatment_faction
```

UI localizes them.

---

# 24. 146A Phase R — Pure Calculation Contract

Assessment is a pure function of:

```text
source radiation/contamination state
counterparty policy
knowledge/verification state
```

No RNG.

---

# 25. 146A Phase S — No Persisted Bridge Cache

Do not persist:

```text
item → price modifier
survivor → social modifier
```

because they can drift.

---

# 26. 146A Phase T — Persist Only Incidents If Needed

Possible stateful records:

- knowingly deceptive contaminated sale,
- faction sanction triggered by incident,
- verification certificate,
- public contamination scandal.

These should live in the owning system where possible.

---

# 27. 146A Phase U — Decontamination Invalidation

When source contamination/radiation changes:

```text
next query reflects new value
```

No explicit bridge reset required.

---

# 28. 146A Phase V — Old Save Compatibility

Because bridges are derived:

```text
old saves need no bridge-state migration
```

Only new stateful incident/certificate fields require defaults.

---

# 29. 146A Phase W — Port Validation

Required consumers:

- MarketSystem,
- HoldfastTradeSession,
- FactionStanceEngine,
- SurvivorRelationsSystem if social effects are mechanical,
- quest/event path if used.

Missing required consumer should fail port contract.

---

# 30. 146A Phase X — Diagnostics

Headless summary:

```text
RADIATION_ECONOMY_POLICIES
RADIATION_SOCIAL_POLICIES
CONTAMINATION_SOURCE_AVAILABLE
ORIGIN_PROVENANCE_AVAILABLE
REQUIRED_PORTS_MISSING
```

---

# 31. 146A Tests

- no radiation,
- contamination bands,
- faction policy lookup,
- verification,
- deterministic assessment,
- decontamination recompute,
- no persisted-cache drift,
- missing policy/reference,
- old-save behavior.

---

# 32. 146A Definition of Done

- [ ] RadiationEconomyBridge,
- [ ] RadiationSocialBridge,
- [ ] derived assessment DTOs,
- [ ] canonical radiation bands,
- [ ] contamination prerequisite resolved,
- [ ] origin/provenance prerequisite audited,
- [ ] buyer knowledge model,
- [ ] verification model,
- [ ] faction policies,
- [ ] pure deterministic queries,
- [ ] no duplicate saved modifiers,
- [ ] ports,
- [ ] explanation codes,
- [ ] diagnostics,
- [ ] old-save compatibility.

---

# 33. Workstream 146B — Economy / Social / Quest Content

## Goal

Author concrete but bounded economy/social responses that make radiation strategically important without turning it into a universal rejection mechanic.

---

# 34. 146B Phase A — 20 Economy Modifier Definitions

Build a data set covering:

- food,
- water,
- equipment,
- medicine,
- trade goods,
- radiation gear,
- verified-clean goods,
- high-contamination goods,
- faction-specific policies.

Use categories/tags where possible, not 20 hardcoded item IDs.

---

# 35. 146B Phase B — Modifier Data Schema

Fields:

```text
id
item_tag/category
contamination_band
buyer_policy
price_multiplier
trade_allowed
verification_requirement
disclosure_rule
localization reason
```

---

# 36. 146B Phase C — Contaminated Food/Water

Source suggests strong discount/block.

Policy should distinguish:

- knowingly safe faction refusing,
- desperate buyer accepting,
- verified decontaminated item.

No universal rule.

---

# 37. 146B Phase D — Irradiated Equipment

Equipment contamination affects value/acceptance.

If equipment contamination is not tracked, this content waits on prerequisite.

Do not use survivor dose as proxy for item contamination.

---

# 38. 146B Phase E — Contaminated Trade Goods

Some policies may block.

Others discount.

Standing can modify willingness only through economy/faction system.

---

# 39. 146B Phase F — Origin Suspicion

Implement only if origin provenance exists.

If yes:

```text
hot-zone origin
+ unverified clean status
→ suspicion discount
```

Verification can remove it.

---

# 40. 146B Phase G — Verified Clean Premium

Rare clean goods from dangerous zones may earn premium.

But premium must be limited to actual scarcity/market context.

Do not hardcode unconditional +50%.

---

# 41. 146B Phase H — Sell to Unaware Trader

This requires explicit transaction semantics.

Flow:

```text
seller knows/should know contamination
buyer unaware
seller chooses disclose or conceal
```

If conceal:

- possible moral-choice consequence,
- later faction/relationship consequence if discovered.

Do not simply pay +100% with no risk/state.

---

# 42. 146B Phase I — Disclosure Choice

Trade UI should offer disclosure where relevant.

This is a real player action.

---

# 43. 146B Phase J — Faction Trade Policies

Source examples map to policy archetypes:

- health-conscious,
- military,
- rebel/desperate,
- independent/standing-first.

Use data.

---

# 44. 146B Phase K — Radiation Treatment Faction

Some faction can value decontamination/treatment.

Opportunity:

- services,
- premium clean certification,
- special trade.

Use canonical service/trade system.

---

# 45. 146B Phase L — 15 Social Modifier/Policy Definitions

Build data set across:

- radiation bands,
- faction attitudes,
- settlement policies,
- survivor reactions,
- treatment-specialist exceptions.

---

# 46. 146B Phase M — Survivor Interaction Penalty

Radiation can modify:

- willingness to share space,
- interaction trust,
- faction envoy comfort.

Keep bounded.

Do not directly drain affinity every day merely for being irradiated unless a real social event occurs.

---

# 47. 146B Phase N — Event-Based Social Consequence

Preferred model:

```text
state creates interaction modifier / event eligibility
actual event produces relationship history
```

This is more explainable than passive invisible affinity decay.

---

# 48. 146B Phase O — Faction Envoy Reaction

Envoy may:

- request decontamination,
- refuse meeting,
- apply temporary trust penalty,
- offer treatment.

Driven by policy.

---

# 49. 146B Phase P — Trade Refusal by Irradiated Representative

If trade uses a representative survivor:

- heavily irradiated representative may trigger policy refusal.

If trade has no representative concept:

- do not fake it.

Potential alternative:

```text
shelter radiation/public-health status
```

only if canonically modeled.

---

# 50. 146B Phase Q — Survivor Avoidance

Other survivors may react through authored social events.

Avoid:

```text
every survivor loses affinity daily
```

Prefer event/interaction consequence.

---

# 51. 146B Phase R — Social Event: The Outcast

State:

```text
high radiation
+ strict settlement policy
```

Outcome:

- refused entry/trade,
- decontamination opportunity,
- relation/quest consequence.

---

# 52. 146B Phase S — Social Event: The Clean Trader

Faction offers premium for verified clean goods.

Requires real verification state.

---

# 53. 146B Phase T — Social Event: The Desperate Deal

Faction accepts contaminated goods at deep discount.

Can create moral/health follow-up.

---

# 54. 146B Phase U — Social Event: Decontamination Queue

Multiple irradiated survivors compete for limited decon resources.

Use real decontamination capacity/resources.

---

# 55. 146B Phase V — Quest: Hot Zone Merchant

Trader with rare goods.

Use canonical trade/quest runtime.

Do not require new hot-zone system if location contamination already exists.

---

# 56. 146B Phase W — Quest: Clean Slate

Decontaminate key survivor before diplomatic meeting.

Great proof of social-bridge reversibility.

---

# 57. 146B Phase X — Quest: Contaminated Cargo

Player discovers shipment contamination.

Choices:

- disclose,
- decontaminate,
- discard,
- conceal.

Actual inventory/economy/moral consequences.

---

# 58. 146B Phase Y — Quest: Radiation Refugees

Use visitor/refugee system if Plan 138 is live.

Do not create a second refugee system.

---

# 59. 146B Phase Z — Radiation Social UI

Survivor panel may show:

```text
radiation band
current social/faction consequences
available decontamination mitigation
```

Avoid a generic “social penalty -25” if qualitative explanation is clearer.

---

# 60. 146B Phase AA — Trade Panel

For each item:

- contamination band,
- verification,
- buyer policy,
- price impact,
- blocked reason,
- disclosure state.

---

# 61. 146B Phase AB — Faction Panel

Show policy-specific effects:

```text
this faction refuses high-contamination goods
this faction requires clean envoy
this faction offers radiation treatment
```

No hidden exact faction AI.

---

# 62. 146B Phase AC — Journal

Log significant incidents:

- contaminated-sale scandal,
- clean certification,
- diplomatic refusal,
- successful decontamination before meeting.

Do not log every price calculation.

---

# 63. 146B Phase AD — Tutorial

First radiation-sensitive trade:

Explain:

```text
contamination affects trade
buyers have different policies
decontamination/verification can restore value
```

---

# 64. 146B Phase AE — Tooltips

Use same assessment object as runtime.

No duplicated modifier math.

---

# 65. 146B Phase AF — Content Utilization

Run trade/social scenarios.

Report:

```text
economy policies evaluated
blocked trades
discounted trades
verified clean premiums
social events
quest triggers
decon-driven modifier removals
```

---

# 66. 146B Phase AG — Dead Policy Rule

Never-observed definition:

- fix,
- mark intentionally rare,
- remove,
- exempt with reason.

---

# 67. 146B Definition of Done

- [ ] 20 economy definitions,
- [ ] 15 social definitions,
- [ ] contaminated food/water,
- [ ] equipment/trade-goods handling if contamination exists,
- [ ] buyer policy variation,
- [ ] disclosure choice,
- [ ] verified-clean handling,
- [ ] origin suspicion only if supported,
- [ ] treatment faction,
- [ ] bounded survivor social reactions,
- [ ] faction envoy reactions,
- [ ] four source social events,
- [ ] four quest hooks,
- [ ] trade/survivor/faction UI,
- [ ] journal,
- [ ] tutorial/tooltips,
- [ ] utilization report.

---

# 68. Workstream 146C — Integration / Consequences / Validation

## Goal

Prove bridge assessments affect the real owning systems, update immediately after decontamination, remain deterministic and save-safe, and do not create unfair or exploitable radiation lockouts.

---

# 69. 146C Phase A — MarketSystem Integration

Final price composition should include:

```text
base price
× economy modifiers
× radiation contamination modifier
```

in the canonical order documented by MarketSystem.

Avoid double applying with generic scarcity/quality modifiers.

---

# 70. 146C Phase B — Trade Restriction Integration

`HoldfastTradeSession` checks assessment.

Blocked item:

```text
cannot transact
```

with reason shown.

---

# 71. 146C Phase C — Disclosure Integration

If seller conceal/disclose is supported:

Trade session records the choice.

Concealment is not a UI-only label.

---

# 72. 146C Phase D — Faction Standing Integration

Only actual incidents should write durable standing changes.

Examples:

- caught selling contaminated goods deceptively,
- complied with decon requirement,
- completed radiation-relief quest.

Current radiation level alone may create contextual interaction modifier without permanently changing standing.

---

# 73. 146C Phase E — Survivor Relations Integration

Prefer event-based reasoned changes.

Example:

```text
irradiated survivor avoided during crisis
→ relation history reason
```

Do not continuously mutate every pair from radiation state.

---

# 74. 146C Phase F — Decontamination Integration

After decontamination:

- price assessment recomputes,
- trade block may clear,
- social interaction modifier recomputes,
- faction meeting eligibility may clear.

No stale cached penalties.

---

# 75. 146C Phase G — DoseLedger Role

Dose history remains historical.

Current social/economic modifier should usually use:

- current radiation,
- current contamination,

not lifetime dose unless a policy explicitly cares about history.

---

# 76. 146C Phase H — Historical Incident Distinction

A faction may remember:

```text
deception incident
```

even after radiation is cleaned.

That is faction history, not radiation state.

---

# 77. 146C Phase I — Old Save Compatibility

Derived bridges mean:

```text
no bridge state required
```

If new verification/incidents exist:

- missing → Unverified/no incidents.

---

# 78. 146C Phase J — Save/Load Disclosure State

If player has already disclosed/certified an item:

- reload preserves knowledge/certificate if those are persistent.

Do not re-roll buyer awareness.

---

# 79. 146C Phase K — No Radiation Edge Case

All assessments:

```text
neutral
```

unless other unrelated faction/economy modifiers apply.

---

# 80. 146C Phase L — All Survivors Irradiated Edge Case

System remains playable.

Requirements:

- at least one decontamination/treatment path exists,
- not all factions globally hard-lock forever,
- critical progression can recover.

---

# 81. 146C Phase M — Severe Contamination Edge Case

Some items may be effectively untradeable.

Player still has:

- decontaminate,
- discard,
- use,
- sell to tolerant faction if design supports.

No inventory soft lock.

---

# 82. 146C Phase N — Exploit: Decontaminate After Quote

Trade quote must revalidate at transaction time.

No stale favorable/blocked quote.

---

# 83. 146C Phase O — Exploit: Concealment Farming

Reloading before discovery/consequence may not reroll a seeded incident if the design includes later discovery.

Persist incident identity/outcome.

---

# 84. 146C Phase P — Exploit: Verified-Clean Premium Loop

Certification cost/state must prevent repeated free premium stacking.

Verification should not multiply every visit.

---

# 85. 146C Phase Q — Price Cap/Floor

Radiation modifier bounded.

No negative prices or extreme exploit multiplier.

Source risk suggests moderate ceilings.

Define market-wide safe bounds.

---

# 86. 146C Phase R — Social Penalty Cap

Maximum contextual social penalty bounded.

Source suggests ~-50.

Use balance tests.

---

# 87. 146C Phase S — Policy Diversity Balance

Ensure:

- strict faction,
- tolerant faction,
- treatment faction,
- standing-first faction

produce differentiated but viable strategies.

---

# 88. 146C Phase T — Decontamination Value Test

Compare:

```text
trade/social value before decon
vs after decon
```

Decontamination should have clear strategic payoff beyond health.

---

# 89. 146C Phase U — Fairness Test

Radiation consequences should be:

- visible,
- explainable,
- reversible where source state is reversible,
- not universally punitive.

---

# 90. 146C Phase V — `--radiation-bridges-selftest`

Required scenarios:

1. clean item,
2. contaminated item discount,
3. strict faction block,
4. tolerant faction acceptance,
5. verified-clean value,
6. irradiated survivor social modifier,
7. decontamination clears modifier,
8. deceptive sale incident if supported,
9. old save,
10. no-radiation neutral baseline.

---

# 91. 146C Phase W — Data Integrity

Validate:

- item categories/tags,
- faction IDs,
- policy IDs,
- radiation band thresholds,
- contamination bands,
- modifier bounds,
- quest/event/localization references.

---

# 92. 146C Phase X — Deliberate Failure Proof

Break:

- faction policy ID,
- multiplier > allowed max,
- unresolved item tag/category,
- missing trade sink.

Assert gate fails.

---

# 93. 146C Phase Y — 100-Day Economy/Social Soak

Run radiation-heavy scenario.

Record:

```text
contaminated trades
blocked trades
decontaminations
social refusals
faction reactions
quests
standing incidents
```

---

# 94. 146C Phase Z — Strategy Profiles

Compare:

```text
decontaminate_first
sell_dirty_to_tolerant
disclose_all
conceal_when_profitable
avoid_hot_zones
```

Measure:

- wealth,
- standing,
- morale/relations,
- radiation burden.

No one policy should dominate every dimension.

---

# 95. 146C Phase AA — Same-State Replay

Because core assessment is deterministic:

```text
same state
→ exact same assessment
```

Any incident RNG remains separately seeded.

---

# 96. 146C Phase AB — UI Runtime Parity

Trade panel/faction panel/survivor panel display the same reason codes and modifiers used by runtime.

---

# 97. 146C Phase AC — Accessibility

No color-only contamination or social status.

Provide:

- text band,
- reason,
- action/mitigation.

---

# 98. 146C Phase AD — Headless Behavior

Bridges query/process without UI.

No panel required for trade/faction rule enforcement.

---

# 99. 146C Phase AE — Retention

Significant historical incidents may roll up under Plan 55.

Do not persist every price assessment.

---

# 100. 146C Phase AF — Documentation

Create:

```text
docs/systems/RADIATION_ECONOMY_SOCIAL.md
```

Include:

- authority boundaries,
- assessment contracts,
- buyer knowledge/disclosure,
- faction policy schema,
- decontamination behavior,
- save behavior,
- adding policies/modifiers.

---

# 101. 146C Definition of Done

- [ ] MarketSystem integration,
- [ ] HoldfastTradeSession enforcement,
- [ ] FactionStance integration,
- [ ] relations integration,
- [ ] decontamination recompute,
- [ ] DoseLedger/history distinction,
- [ ] old-save support,
- [ ] disclosure/certification persistence if used,
- [ ] no-radiation edge case,
- [ ] all-irradiated edge case,
- [ ] severe contamination recovery path,
- [ ] quote revalidation,
- [ ] exploit guards,
- [ ] price/social caps,
- [ ] policy diversity,
- [ ] decon strategic-value proof,
- [ ] fairness test,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] 100-day soak,
- [ ] strategy profiles,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] headless behavior,
- [ ] docs.

---

# 102. Integrated Radiation Bridge Pipeline

```text
survivor radiation
item contamination
buyer/faction policy
verification/knowledge
          │
          ▼
RadiationEconomyBridge / RadiationSocialBridge
          │
          ├─ trade assessment
          └─ social assessment
          │
          ▼
existing owners
          │
   ┌──────┼─────────┬─────────┐
   ▼      ▼         ▼         ▼
 market trade   factions   relations
   │      │         │         │
   └──────┴─────────┴─────────┘
          │
          ▼
      quests/events/UI
```

---

# 103. Radiation Truth Contract

Survivor radiation truth is canonical RadiationSystem state.

No copied bridge value.

---

# 104. Contamination Truth Contract

Item contamination truth comes from one item/environment authority.

No inferred contamination from name/origin alone.

---

# 105. Buyer Knowledge Contract

Buyer knowledge is separate from contamination truth.

This enables:

- disclosure,
- verification,
- deception,
- discovery.

---

# 106. Economy Assessment Contract

Assessment returns:

```text
allowed
price multiplier
verification requirement
reason codes
```

Market calculates final price.

---

# 107. Social Assessment Contract

Assessment returns:

```text
interaction modifier
trade eligibility
temporary trust context
reason codes
```

Owning systems apply durable outcomes only when an incident occurs.

---

# 108. Faction Policy Contract

Faction behavior is data-authored.

No giant switch on faction ID.

---

# 109. Decontamination Contract

Decontamination updates source state.

Bridge automatically reflects it.

---

# 110. Price Modifier Contract

Radiation modifier is bounded and composes once with MarketSystem.

---

# 111. Social Penalty Contract

Social penalty is contextual and bounded.

Temporary radiation does not automatically create permanent relationship damage.

---

# 112. Incident Contract

Durable consequences require an actual event:

- deception discovered,
- meeting refused,
- treatment completed,
- contaminated shipment delivered.

---

# 113. Verification Contract

“Verified clean” is a state/credential.

Do not infer it from contamination = 0 if the buyer policy requires proof.

---

# 114. Origin Contract

Hot-zone origin effects only exist if provenance is real.

No fake origin inference.

---

# 115. Old-Save Contract

Derived bridges need no persisted modifier migration.

---

# 116. Save Contract

Persist only true new state:

- verification,
- disclosure,
- incident IDs,

and preferably in the owning trade/faction system.

---

# 117. UI Contract

UI uses the same assessment object as runtime.

No duplicated price/social calculation.

---

# 118. Fairness Contract

The player should be able to understand:

```text
why the trade is blocked
why the price changed
why the faction reacts
how to mitigate it
```

---

# 119. Balance Contract

Radiation becomes strategically important but not universally disabling.

At least one viable recovery path must remain.

---

# 120. Content Acceptance Contract

Policies/modifiers move through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ ASSESSMENT_PRODUCED
→ EFFECT_ENFORCED
→ PLAYER_VISIBLE
```

---

# 121. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| bridge duplicates radiation/contamination state | Medium | High | pure projection only |
| discrimination feels arbitrary | Medium | High | deterministic policy + reason codes |
| permanent stigma from temporary exposure | Medium | High | contextual modifier/event-based durability |
| price modifiers stack twice | Medium | High | one MarketSystem composition point |
| concealment becomes exploit | Medium | High | disclosure/knowledge/incident state |
| origin suspicion inferred falsely | Medium | High | require provenance |
| decontamination does not clear penalty | Medium | High | recompute from source state |
| all factions hard-lock irradiated players | Medium | Critical | policy diversity/recovery test |
| social penalties continuously drain affinity | Medium | High | event-based relation effects |
| verified-clean premium farming | Medium | Medium | certificate/transaction guard |
| item contamination prerequisite missing | Medium | High | audit/gate prerequisite |
| UI quote differs from final transaction | Medium | High | transaction-time revalidation |

---

# 122. Commit Strategy

## 146A — Foundation

### C2[30].1 — baseline + radiation-bridge ADR

### C2[30].2 — contamination/provenance prerequisite audit

### C2[30].3 — economy/social assessment DTOs

### C2[30].4 — faction policy schema

### C2[30].5 — buyer knowledge/verification model

### C2[30].6 — pure bridge calculation

### C2[30].7 — ports/explanation/diagnostics

### Gate: 146A complete

---

## 146B — Content

### C2[30].8 — 20 economy definitions

### C2[30].9 — 15 social definitions

### C2[30].10 — contamination price/block policies

### C2[30].11 — disclosure/verified-clean logic

### C2[30].12 — faction/social reaction content

### C2[30].13 — events/quests

### C2[30].14 — UI/tutorial/tooltips

### C2[30].15 — content-utilization report

### Gate: 146B complete

---

## 146C — Closure

### C2[30].16 — Market/Trade integration

### C2[30].17 — Faction/Relations integration

### C2[30].18 — Decontamination/DoseLedger integration

### C2[30].19 — save/old-save/incident matrix

### C2[30].20 — exploit/cap/revalidation tests

### C2[30].21 — selftest + failure proof

### C2[30].22 — 100-day radiation-economy soak

### C2[30].23 — strategy profiles/fairness

### C2[30].24 — UI parity/accessibility/docs

### Gate: 146C complete

---

# 123. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --radiation-bridges-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
radiation policy content-utilization report
old-save fixture load
same-state assessment replay
100-day radiation-economy/social soak
trade/faction/survivor UI parity snapshot/accessibility check
```

---

# 124. Flagship Definition of Done

## 146A — Foundation

- [ ] RadiationEconomyBridge,
- [ ] RadiationSocialBridge,
- [ ] contamination authority identified,
- [ ] origin/provenance prerequisite resolved,
- [ ] derived assessments,
- [ ] canonical radiation/contamination bands,
- [ ] buyer knowledge,
- [ ] verification,
- [ ] faction policies,
- [ ] deterministic pure functions,
- [ ] no persisted modifier cache,
- [ ] old-save compatibility,
- [ ] ports,
- [ ] explanation codes,
- [ ] diagnostics.

## 146B — Content

- [ ] 20 economy definitions,
- [ ] 15 social definitions,
- [ ] contaminated-goods valuation,
- [ ] faction trade restrictions,
- [ ] tolerant/strict/treatment policy diversity,
- [ ] disclosure choice,
- [ ] verified-clean behavior,
- [ ] origin suspicion only if supported,
- [ ] survivor/faction reactions,
- [ ] four social events,
- [ ] four quest hooks,
- [ ] radiation trade/survivor/faction UI,
- [ ] tutorial/tooltips,
- [ ] utilization report.

## 146C — Integration

- [ ] MarketSystem,
- [ ] HoldfastTradeSession,
- [ ] FactionStanceEngine,
- [ ] SurvivorRelationsSystem,
- [ ] DecontaminationSystem,
- [ ] DoseLedger distinction,
- [ ] save/load incident state if any,
- [ ] no-radiation neutral case,
- [ ] all-irradiated recoverable case,
- [ ] severe contamination recovery path,
- [ ] quote revalidation,
- [ ] concealment exploit guard,
- [ ] verified-clean exploit guard,
- [ ] price/social caps,
- [ ] policy diversity,
- [ ] decon payoff,
- [ ] fairness test,
- [ ] selftest,
- [ ] failure proof,
- [ ] 100-day soak,
- [ ] strategy profiles,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] headless,
- [ ] docs.

## Global

- [ ] no duplicate radiation truth,
- [ ] no duplicate contamination truth,
- [ ] no duplicate standing/affinity truth,
- [ ] no random opaque discrimination in core projection,
- [ ] no unsupported origin inference,
- [ ] no stale penalty after decontamination,
- [ ] full verification green.

---

# 125. Closure Report Template

```markdown
## C2[30] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Radiation query:
- Dose unit:
- Item contamination authority:
- Item provenance authority:
- Market price composition:
- Trade validation:
- Faction policy source:
- Relation API:

### 146A — Foundation
- Economy bridge:
- Social bridge:
- Radiation bands:
- Contamination bands:
- Buyer knowledge:
- Verification:
- Faction policies:
- Derived-state persistence:
- Missing ports:
- Result:

### 146B — Content
- Economy definitions:
- Social definitions:
- Strict policies:
- Tolerant policies:
- Treatment policies:
- Disclosure:
- Verified clean:
- Origin-based policies:
- Social events:
- Quests:
- UI:
- Unused definitions:
- Result:

### 146C — Integration
- Market:
- Trade:
- Factions:
- Relations:
- Decontamination:
- Dose history:
- Old save:
- Quote revalidation:
- Concealment exploit:
- Certified-clean exploit:
- Max price penalty:
- Max social penalty:
- 100-day soak:
- Strategy profiles:
- UI parity:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Radiation bridges selftest:
- Port contract:
- Content utilization:
- Old-save fixtures:
- Same-state replay:
- 100-day soak:
- Verify fast:

### Final Metrics
- ECONOMY_MODIFIER_DEFINITIONS:
- SOCIAL_POLICY_DEFINITIONS:
- CONTAMINATED_TRADES:
- BLOCKED_TRADES:
- VERIFIED_CLEAN_TRADES:
- DECONTAMINATION_CLEARANCES:
- SOCIAL_REFUSALS:
- FACTION_RADIATION_INCIDENTS:
- UI_RUNTIME_MISMATCHES:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Item contamination:
- Origin/provenance:
- Faction policies:
- Social events:
- Quests:
- UI:
```

---

# 126. Final Execution Directive

Execute Plan 146 as a **deterministic radiation-to-economy/social projection layer**, not as a new radiation, market, or social simulation.

The critical sequence is:

```text
confirm canonical survivor radiation and item contamination truth
→ define buyer/faction radiation policies
→ model buyer knowledge and verification explicitly
→ compute deterministic assessments
→ feed those assessments into Market/Trade/Faction/Relations
→ make decontamination immediately clear source-state penalties
→ persist only real incidents/certifications
→ prove fairness, reversibility, and exploit resistance in long runs
```

Do not save derived price modifiers.

Do not save derived social modifiers.

Do not invent contaminated-item state if a canonical owner is missing.

Do not punish a survivor permanently just because their current dose is high.

Do not infer hot-zone origin if provenance does not exist.

Do not let decontamination leave stale penalties behind.

The strongest authority rule is:

> **Radiation and contamination remain owned by their canonical systems; the bridges only interpret those facts for a specific buyer, faction, or social context.**

The strongest fairness rule is:

> **Every radiation-related economic or social penalty must be explainable, bounded, and tied to a mitigation or recovery path wherever the underlying radiation state itself is reversible.**

The strongest economy rule is:

> **The transaction system must know the difference between contaminated truth, buyer knowledge, seller disclosure, and verified cleanliness—otherwise contaminated trade becomes either trivial or exploitable.**

The flagship acceptance scenario is:

> **Take one contaminated shipment and one heavily irradiated survivor to three counterparties: a strict health-conscious faction, a desperate/tolerant buyer, and a treatment-specialist faction. Verify each receives a different but deterministic trade/social assessment. Decontaminate the survivor and shipment, reopen the same interactions, and confirm restrictions/price/social modifiers recompute immediately from source state. Then test a concealed contaminated sale, save/load before its later consequence, and prove the incident does not reroll or duplicate. The UI must show the same reason codes and modifiers the transaction/faction systems actually enforce.**
