# C2 — Flagship Integration Plan [41]: Intelligence Networks, Informants, Evidence Quality, Rumor Operations, and Counter-Intelligence

> **Deliverable:** `C2_planintegration[41].md`
> **Source scope:** Plan 203 — *Intelligence & Rumor Network System*
> **Primary objective:** create a deterministic player-managed intelligence layer built around informants, intelligence operations, reports, reliability, verification, counter-intelligence, and actionable information; connect rumor planting/countering to the existing information-propagation and propaganda authorities rather than duplicating them; and turn information quality into a strategic resource that can influence planning without replacing the systems that own factions, combat, trade, radio interception, expeditions, espionage, propaganda, or world-information diffusion.
> **Required execution order:** **203A Intelligence Evidence / Informant Contract → 203B Operations, Reports, Verification, Rumor Requests, Counter-Intel & UI → 203C Cross-System Integration, Save/CI, Exploit Control, Balance, and Intelligence-Quality Closure**
> **Hard dependencies:** `FactionStanceEngine`; `FactionBranchCoordinator`; `SignalTriangulationSystem`; `ExpeditionSystem`; `HoldfastTradeSession`; combat/defense intelligence consumers; Plan 31 semantic event vocabulary; Plan 36 port-contract discipline; Plan 39 save durability; Plan 55 retention; Plan 131 information/rumor propagation; Plan 153 espionage/sabotage; Plan 157 radio interception/encryption/jamming; Plan 168 propaganda/disinformation truth/effects; Plan 132 hidden agendas where shelter-side infiltrators overlap; Plan 162 archive for landmark intelligence history.
> **Scope discipline:** no second rumor-propagation graph, no second propaganda engine, no second faction stance ledger, no second radio interception model, no duplicate expedition scheduler, no duplicate combat model, no duplicate espionage sabotage/assassination executor, no intelligence report that becomes authoritative world truth merely because its reliability is high, no player-visible exact “truth percentage” unless explicitly justified, no report rerolls via save/load, no free passive information from unpaid/inactive informants, and no information flood that requires the player to read every low-value report manually.

---

# 0. Executive Intent

ASHFALL already contains multiple systems that produce or consume information:

```text
Plan 131
→ world information flow
→ rumor/news propagation

Plan 153
→ espionage / sabotage operations

Plan 157
→ interception / encryption / jamming / delivery

Plan 168
→ propaganda / disinformation effects

Faction systems
→ faction state / branches / stance

ExpeditionSystem
→ physical reconnaissance

Trade
→ contacts / market information

Combat
→ tactical outcomes
```

What is still missing is a coherent **intelligence evidence and network-management layer**.

The player should be able to:

```text
recruit a source
→ understand what that source can access
→ pay/protect/contact the source
→ receive an uncertain report
→ compare multiple reports
→ verify or disprove intelligence
→ decide whether it is actionable
→ launch a surveillance/infiltration/recruitment/extraction operation
→ feed confirmed intelligence into faction/trade/combat/expedition decisions
→ plant or counter a rumor through existing propagation/propaganda systems
```

The target architecture is:

```text
informants / operations / intercepts / expeditions / trade contacts
                         │
                         ▼
               IntelligenceNetworkSystem
                         │
             ┌───────────┼────────────┐
             ▼           ▼            ▼
          reports     verification  source/cover
             │           │            │
             └───────────┼────────────┘
                         ▼
               intelligence evidence
                         │
         ┌───────────────┼─────────────────┐
         ▼               ▼                 ▼
 faction planning    expedition/combat    trade
         │
         ▼
 rumor/disinformation request
         │
         ├────────► Plan 131 propagation
         └────────► Plan 168 effect truth
```

The strongest product outcome is:

> **The player never simply “knows” enemy plans because an intelligence stat is high. They possess reports of different provenance, freshness, reliability, and corroboration; they can invest in verification, choose whether to act under uncertainty, protect or extract sources, and deliberately weaponize information through existing rumor/propaganda channels.**

---

# 1. Source Diagnosis

The source establishes:

- no dedicated `IntelligenceSystem`, `RumorSystem`, `InformantSystem`, or equivalent currently exists,
- `FactionStanceEngine.cs` tracks faction stance/trust,
- `SignalTriangulationSystem.cs` handles radio signal analysis,
- `ExpeditionSystem.cs` can support field intelligence gathering,
- `HoldfastTradeSession.cs` can support trade contacts,
- Plan 131 mentions information/rumor flow,
- Plan 153 mentions faction espionage/informants,
- Plan 168 mentions propaganda/rumor campaigns,
- the source proposes:
  - informant networks,
  - intelligence reports,
  - rumors,
  - intelligence operations,
  - counter-intelligence,
  - reliability,
  - verification,
  - 5+ informant types,
  - 7+ report types,
  - rumor planting/countering,
  - surveillance/interception/infiltration/extraction/recruitment,
  - enemy intelligence threats,
  - 10+ informant templates,
  - 20+ report templates,
  - UI, events, quest hooks, deterministic seeding, save/load, and `--intelligence-network-selftest`.

However, the broader C2 architecture creates three non-negotiable overlap rules:

```text
Plan 131 owns propagation of information/rumors
Plan 153 owns espionage/sabotage action execution
Plan 168 owns propaganda/disinformation effect resolution
```

Therefore C2[41] treats “rumor” as:

```text
an intelligence-originated information object / operation request
```

while Plan 131 owns:

```text
how it spreads through settlements/factions
```

and Plan 168 owns:

```text
persuasive/disinformation effects
```

Likewise, C2[41] may plan:

```text
surveillance
infiltration
recruitment
extraction
```

but assassination/sabotage should remain external execution types unless Plan 153 explicitly delegates them.

---

# 2. Program-Level Success Criteria

C2[41] closes only when all of the following are true.

1. `IntelligenceNetworkSystem.cs` exists.
2. One authoritative intelligence evidence/report store exists.
3. Informants have stable identities, coverage, access, reliability, motive, cost, and cover state.
4. Informant reliability is not equivalent to report truth.
5. Report truth remains separate from player belief/confidence.
6. Verification can improve or reduce confidence without rewriting the source event.
7. Reports expire or become stale according to report type.
8. Actionable intelligence is a derived assessment, not a free boolean manually toggled by UI.
9. Intelligence operations are deterministic under `ISeededRng`.
10. Operation outcomes persist before consequences are applied.
11. Save/load cannot reroll success/failure/compromise.
12. Surveillance works as low-risk evidence generation.
13. Radio interception consumes Plan 157 results rather than implementing a second interception model.
14. Infiltration creates/maintains intelligence access without becoming a second sabotage executor.
15. Extraction rescues/withdraws compromised sources through canonical expedition/roster/faction systems.
16. Recruitment turns an eligible contact/agent into an informant through a canonical relationship/faction/espionage handoff.
17. Assassination and sabotage are not duplicated locally if Plan 153/combat owns them.
18. Rumor planting creates a propagation/disinformation request rather than a second rumor-spread engine.
19. Counter-rumor operations consume Plan 131/168 outputs rather than decrementing local spread percentages.
20. Counter-intelligence detects and assesses hostile intelligence threats.
21. Counter-intelligence does not create a second hidden-agenda/spy identity authority.
22. Hostile sabotage/assassination consequences route to their canonical systems.
23. Trade contacts can become legitimate informants.
24. Expedition reconnaissance can generate legitimate intelligence reports.
25. Signal triangulation/interception can generate legitimate intelligence reports.
26. Faction branch intelligence reveals only information justified by report confidence/access.
27. Combat intelligence modifies planning/estimation, not enemy truth itself.
28. Old saves load with no network and no reports.
29. No-network play remains valid.
30. Extensive-network play remains bounded and manageable.
31. Low-value reports are grouped/filtered rather than spamming the player.
32. 5+ source informant archetypes are supported.
33. 7+ source report types are supported.
34. 10+ informant templates and 20+ report templates validate.
35. All references to factions, locations, survivors, operations, and rumor profiles resolve.
36. UI distinguishes source reliability, report confidence, verification, freshness, and actionability.
37. Player does not see hidden exact truth unless verification genuinely proves it.
38. Same seed + same operations + same sources produce the same intelligence history.
39. `--intelligence-network-selftest` validates the whole evidence lifecycle.
40. Full verification remains green with no duplicate authority.

---

# 3. Architectural Invariants

## 3.1 Intelligence owns evidence, not world truth

An intelligence report says:

```text
"Source A reports Faction X is moving toward Region Y."
```

It does not set:

```text
FactionX.ActualDestination = RegionY
```

The faction system remains authoritative.

## 3.2 Reliability and truth are distinct

Source reliability:

```text
how often this source tends to be accurate
```

Report confidence:

```text
how strongly the player should believe this report
```

World truth:

```text
what actually happened
```

Never collapse these into one number.

## 3.3 Verification is evidence aggregation

Verification may use:

- second source,
- interception,
- expedition observation,
- trade corroboration,
- later world event.

It does not simply increase reliability by spending points.

## 3.4 Plan 131 owns propagation

Intelligence may create:

```text
RumorCampaignRequest
CounterRumorRequest
```

Plan 131 owns spread topology/state.

## 3.5 Plan 168 owns propaganda effects

If rumor/disinformation intends to alter belief/trust/morale:

- Plan 168 resolves effect.

Intelligence records outcome references.

## 3.6 Plan 153 owns hostile covert execution where overlapping

Intelligence can identify/plan/target.

Do not duplicate sabotage/assassination runtime.

## 3.7 Plan 157 owns signal interception mechanics

Intelligence consumes the resulting intercepted content/evidence.

## 3.8 FactionStanceEngine owns faction standing

Intelligence may emit reason-coded consequences.

## 3.9 Informant cover is intelligence-owned

Informant relationship/contract + cover status belong here if no Plan 153 authority already owns them.

If Plan 153 owns agent cover:

- reuse it.

## 3.10 Player knowledge must remain explainable

Every actionable report should expose:

- source class,
- freshness,
- verification,
- confidence reason.

---

# 4. Information Architecture

Use four distinct concepts:

```text
Raw Evidence
→ Intelligence Report
→ Assessment
→ Action / Propagation Request
```

### Raw evidence

Examples:

- intercepted message,
- eyewitness report,
- patrol observation,
- trade rumor,
- informant statement.

### Intelligence report

Structured interpretation of evidence.

### Assessment

Player-facing confidence/actionability.

### Action

- expedition,
- faction preparation,
- rumor campaign,
- counter-intel investigation,
- covert operation.

This separation avoids reports becoming direct world state.

---

# 5. Truth / Knowledge Separation

For test fixtures, the engine may know:

```text
ground_truth
```

but production `IntelligenceReport` should not persist a player-visible truth flag.

Recommended internal/test model:

```text
IntelligenceEvidenceTruthLink
```

only available to:

- report generation,
- verification resolution,
- CI/testing,
- AI/faction logic where appropriate.

Player-facing report stores:

```text
confidence
verification_state
freshness
```

not secret truth.

---

# 6. Workstream 203A — Foundation / Intelligence Evidence Contract

## Goal

Create one intelligence network authority for sources, reports, operations, verification, and counter-intelligence while explicitly delegating rumor propagation, propaganda effects, espionage execution, radio interception, faction truth, combat truth, and trade truth.

---

# 7. 203A Phase A — Create `IntelligenceNetworkSystem`

Path:

```text
Assets/Ashfall.Core/Intelligence/IntelligenceNetworkSystem.cs
```

Responsibilities:

- register/manage informants,
- receive raw evidence,
- create intelligence reports,
- maintain report freshness/confidence/verification,
- schedule intelligence operations,
- resolve operation outcomes,
- maintain counter-intelligence assessments,
- request rumor/counter-rumor operations through external authorities,
- expose intelligence read models,
- capture/restore intelligence-owned state.

---

# 8. 203A Phase B — Avoid One Giant `IntelligenceNetwork` DTO

The source proposes one DTO containing:

- informants,
- reports,
- rumors,
- reputation,
- counter-intelligence,
- settings.

Prefer a normalized model:

```text
InformantRecord
IntelligenceReport
IntelligenceEvidenceRef
IntelligenceAssessment
IntelligenceOperationRecord
CounterIntelligenceCase
RumorOperationLink
IntelligenceNetworkState
```

---

# 9. 203A Phase C — Network Identity

If only one player network exists:

```text
network_id = shelter_intelligence
```

Do not create arbitrary multiple networks unless gameplay needs them.

---

# 10. 203A Phase D — Network Reputation

Source proposes:

```text
networkReputation 0–100
```

Clarify what it means.

Recommended:

```text
NetworkCredibility
```

derived from:

- verified report accuracy history,
- source management,
- false-report rate,
- compromised operations.

Avoid a free-floating number manually modified by every event.

---

# 11. 203A Phase E — Counter-Intelligence Level

Source proposes:

```text
counterIntelligenceLevel 0–100
```

Prefer derived readiness from:

- investigators,
- security systems,
- known threats,
- procedures,
- recent compromises,
- Plan 132/153 shelter infiltration state.

If a numeric score is retained:

- derive it,
- do not persist as independent truth.

---

# 12. 203A Phase F — Informant Record

Recommended:

```text
informant_id
identity_ref
informant_type
coverage_scope
reliability_profile
access_profile
cover_state
motive
contact_state
payment_contract
recruitment_source
last_contact_day
last_report_day
handler_id optional
```

---

# 13. 203A Phase G — Informant Identity Ref

An informant may be:

```text
survivor_id
faction_npc_id
contact_id
trade_contact_id
```

Use typed union/reference.

Do not overload one string field without type.

---

# 14. 203A Phase H — Informant Types

Preserve source five:

```text
FieldAgent
PlacedSpy
TradeContact
LocalSource
Defector
```

Additional authored types allowed only if distinct.

---

# 15. 203A Phase I — Source Requirement vs Template Requirement

Source asks:

```text
10+ informant types
```

later, while Foundation says 5+.

Resolve as:

- **5 canonical informant archetypes**
- **10+ informant templates/profiles**

Do not invent 10 enum values if five behaviors cover the design.

Example templates:

```text
black_market_trader
border_driver
disgruntled_guard
refugee_witness
rail_worker
medical_contact
faction_clerk
radio_technician
deserter
smuggler
```

mapped onto the five archetypes.

---

# 16. 203A Phase J — Reliability Model

Avoid exact truth-like display.

Internal 0–100 can exist.

Player presentation can be:

```text
Unproven
Questionable
UsuallyReliable
Reliable
HighlyReliable
```

depending design.

---

# 17. 203A Phase K — Reliability Evolution

Reliability updates from:

- later verified reports,
- known falsehoods,
- motive conflict,
- compromise,
- deliberate deception.

Do not update based solely on whether the player liked the report.

---

# 18. 203A Phase L — Access Level

Internal 0–100 allowed.

Represents:

```text
depth / privilege / proximity to target information
```

Not accuracy.

High access can still provide false intel.

---

# 19. 203A Phase M — Coverage Scope

Typed:

```text
Faction
Location
Region
TradeNetwork
RadioDomain
Shelter
```

One informant may have multiple scopes only if authored.

---

# 20. 203A Phase N — Cover State

Source:

```text
Intact
Suspicious
Compromised
Blown
```

Possible extension:

```text
Withdrawn
Missing
```

if needed.

State transitions must be explicit.

---

# 21. 203A Phase O — Informant Motive

Preserve:

```text
Money
Ideology
Coercion
Revenge
```

Potential:

```text
Protection
PersonalLoyalty
```

only if content needs them.

Motive affects:

- payment,
- defection risk,
- willingness,
- report bias.

---

# 22. 203A Phase P — Coercion Guardrail

Coercion should create:

- low loyalty,
- exposure risk,
- moral consequence,
- potential false reporting.

Do not model coerced source as simply cheaper.

---

# 23. 203A Phase Q — Payment Contract

Use actual resources/currency.

Store:

```text
payment type
amount
cadence
arrears
```

No magic “paymentPerDay” if economy uses different contract cadence.

---

# 24. 203A Phase R — Missed Payment

Possible effects:

- source pauses reporting,
- reliability/loyalty risk,
- source defects,
- blackmail/exposure event.

Deterministic + authored thresholds.

---

# 25. 203A Phase S — Contact Cadence

Informants do not necessarily report daily.

Use:

- access,
- coverage activity,
- communication availability,
- report opportunity.

---

# 26. 203A Phase T — Informant Activity State

Typed:

```text
Active
Dormant
Unavailable
Compromised
Blown
Extracting
Retired
Lost
```

---

# 27. 203A Phase U — Intelligence Report Types

Preserve source seven:

```text
FactionMovement
MilitaryStrength
ResourceCache
LeadershipChange
PlotDiscovered
TerrainIntel
TradeIntel
```

Possible additional:

```text
DiplomaticIntent
InfrastructureStatus
InternalDissent
```

only after producer audit.

---

# 28. 203A Phase V — Intelligence Report Schema

Recommended:

```text
report_id
report_type
subject_ref
source_refs
evidence_refs
received_day
observation_day
freshness_policy
confidence_band
verification_state
actionability
content_template_key
content_parameters
known_uncertainties
```

Do not persist rendered prose as sole authority.

---

# 29. 203A Phase W — Reliability vs Confidence

Report confidence derives from:

```text
source reliability
+ access
+ evidence quality
+ corroboration
+ freshness
+ known contradictions
```

---

# 30. 203A Phase X — Verification States

Source:

```text
Unverified
PartiallyVerified
Verified
False
```

Add only if needed:

```text
Disputed
Stale
```

Freshness should preferably be separate from verification.

---

# 31. 203A Phase Y — False Report Semantics

`False` means:

- sufficiently disproven by canonical evidence,
- not merely “low confidence.”

---

# 32. 203A Phase Z — Permanent vs Expiring Reports

Use report-type freshness policy.

Examples:

```text
leadership change → persistent until superseded
faction movement → short-lived
resource cache → stale when location changes/depletes
terrain intel → stale through location evolution
trade intel → market-duration limited
```

---

# 33. 203A Phase AA — Actionability Assessment

Do not persist a manually mutable bool if derivable.

Derived from:

- subject relevance,
- freshness,
- confidence,
- available response,
- time window.

UI label:

```text
Actionable
PotentiallyActionable
Background
Expired
```

---

# 34. 203A Phase AB — Evidence Model

Create:

```text
IntelligenceEvidenceRef
```

Fields:

```text
evidence_id
source_type
source_ref
subject_ref
observed_day
content_signature
confidence contribution
```

---

# 35. 203A Phase AC — Evidence Sources

Types:

```text
Informant
Intercept
ExpeditionObservation
TradeContact
FactionContact
PublicRumor
Document
CapturedMaterial
```

---

# 36. 203A Phase AD — Multiple Sources

Verification requires independent evidence.

Do not count:

```text
same rumor repeated by three people
```

as three independent sources if Plan 131 identifies same origin.

---

# 37. 203A Phase AE — Source Independence

Add:

```text
origin_chain_id
```

or provenance graph reference.

Use Plan 131 information provenance if available.

---

# 38. 203A Phase AF — Rumor Duplication Guard

If three settlements repeat one original rumor:

- evidence diversity remains 1 source lineage,
- spread may be high,
- verification stays weak.

This distinction is central.

---

# 39. 203A Phase AG — Intelligence Operation Record

Recommended:

```text
operation_id
operation_type
target_ref
assigned_agent_ids
source/informant refs
planned_day
started_day
expected_duration
resource_commitments
risk_profile
status
outcome_ref
compromise_state
```

---

# 40. 203A Phase AH — Operation Types

Baseline owned by IntelligenceNetworkSystem:

```text
Surveillance
Infiltration
Extraction
Recruitment
Analysis
SourceContact
```

`Interception` should be a Plan 157-backed adapter.

---

# 41. 203A Phase AI — Assassination / Sabotage Scope Correction

The source DTO lists:

```text
assassination
sabotage
```

but Plan 153 and combat/faction systems overlap.

Do not implement those as intelligence-owned executions.

Instead:

```text
confirmed intelligence
→ covert_action_targeting_request
→ Plan 153 / combat owner
```

---

# 42. 203A Phase AJ — Surveillance

Purpose:

- observe target,
- produce evidence,
- low/moderate exposure risk.

Can be:

- field agent,
- local source,
- expedition reconnaissance.

---

# 43. 203A Phase AK — Interception

Intelligence does not calculate interception.

Flow:

```text
Plan 157 interception result
→ IntelligenceEvidenceRef
→ report/assessment
```

---

# 44. 203A Phase AL — Infiltration

Purpose:

- establish/upgrade a placed spy/source.

Outcome:

- access,
- cover state,
- future evidence opportunities.

Does not automatically sabotage target.

---

# 45. 203A Phase AM — Extraction

Purpose:

- recover compromised informant/agent.

Execution may use:

- ExpeditionSystem,
- faction exchange,
- covert action system.

IntelligenceNetworkSystem owns operation intent/outcome reference.

---

# 46. 203A Phase AN — Recruitment

Purpose:

- convert eligible contact into source.

Inputs:

- access,
- motive,
- relationship/trust,
- leverage,
- faction state.

No arbitrary “successChance” independent of those facts.

---

# 47. 203A Phase AO — Analysis Operation

Recommended addition.

Purpose:

- spend analyst time to correlate reports,
- discover contradictions,
- improve confidence without field exposure.

This makes intelligence quality strategic rather than only more spying.

---

# 48. 203A Phase AP — Operation Success Model

Do not persist only a precomputed `successChance`.

Use:

```text
OperationAssessment
```

Inputs:

- agent skill,
- access,
- target security,
- cover,
- resources,
- route,
- intelligence support,
- counter-intelligence pressure.

At commit time:

- calculate deterministic outcome,
- persist result.

---

# 49. 203A Phase AQ — Risk Model

Separate:

```text
mission success
source compromise
agent injury/capture
information quality
```

One operation can:

- succeed but compromise source,
- fail but remain covert,
- produce partial intelligence.

---

# 50. 203A Phase AR — Operation Outcomes

Typed:

```text
Success
PartialSuccess
Failure
Compromised
Aborted
```

Compromise can coexist as secondary outcome.

---

# 51. 203A Phase AS — Deterministic RNG

Dedicated stream:

```text
intelligence
```

Stable keys:

```text
campaign seed
operation ID
source ID
target ID
phase
```

Sort candidates.

---

# 52. 203A Phase AT — Outcome Commitment

Persist operation outcome before:

- source cover changes,
- report generation,
- faction reaction,
- injury/capture.

Prevents reroll.

---

# 53. 203A Phase AU — Counter-Intelligence Case

Recommended:

```text
counter_case_id
threat_type
evidence_refs
suspected_actor_refs
detected_day
confidence
investigation_state
severity
external_operation_ref
resolution_ref
```

---

# 54. 203A Phase AV — Counter-Intelligence Threat Types

Preserve source:

```text
EnemySpy
InformationLeak
PropagandaCampaign
AssassinationPlot
SabotagePlot
```

The case tracks knowledge of threat.

It does not execute the hostile action.

---

# 55. 203A Phase AW — Counter-Intel State

Source:

```text
Undetected
Suspected
Investigating
Identified
Neutralized
```

`Undetected` cannot normally be stored in player-known case state.

Better:

```text
latent threat owned externally
→ evidence appears
→ Suspected case created
```

---

# 56. 203A Phase AX — Enemy Spy Ownership

If Plan 132/153 owns infiltrator identity:

- counter-intel references that entity.

Do not create a duplicate hidden spy record.

---

# 57. 203A Phase AY — Threat Neutralization

Routes to owner:

- expel/detain via visitor/security,
- arrest via governance/security,
- counter-propaganda via Plan 168,
- sabotage prevention via Plan 153,
- combat response via defense/combat.

Counter-intelligence case records resolution.

---

# 58. 203A Phase AZ — State Persistence

Persist:

```text
informant records
active contracts
report/evidence history
operations
counter-intelligence cases
rumor operation links
idempotency keys
schema version
```

Do not persist:

- faction truth copy,
- Plan 131 propagation state,
- Plan 168 propaganda state,
- Plan 157 radio network state,
- combat truth,
- market truth.

---

# 59. 203A Phase BA — Old Save Compatibility

Source exact policy:

```text
no network
no reports
```

Implement:

- empty informants,
- empty reports,
- empty operations,
- empty counter cases.

No retroactive report generation.

---

# 60. 203A Phase BB — Migration Grace

First days after old-save migration:

- no surprise hostile spy flood due solely to system activation,
- new threat generation follows external owner cadence.

---

# 61. 203A Phase BC — GameBootstrap

Source asks:

```text
SetupIntelligenceNetwork
TickIntelligenceNetwork
SaveIntelligenceNetwork
```

Follow Plan 28 composition conventions.

No manual bootstrap drift.

---

# 62. 203A Phase BD — Tick Model

Avoid global continuous polling.

Use:

- event-driven evidence intake,
- scheduled operation resolution,
- daily informant/payment/contact processing,
- daily report expiry,
- bounded counter-intel reconciliation.

---

# 63. 203A Phase BE — Semantic Events

Candidate kinds:

```text
informant_recruited
informant_contact_lost
informant_compromised
intelligence_report_received
intelligence_report_verified
intelligence_report_disproved
intelligence_operation_started
intelligence_operation_resolved
counter_intelligence_case_opened
counter_intelligence_case_resolved
rumor_operation_requested
```

---

# 64. 203A Phase BF — Port Contract

Mandatory:

- factions,
- survivor/agent availability,
- resources/payment,
- save service,
- semantic events.

Conditional:

- Plan 131,
- Plan 153,
- Plan 157,
- Plan 168,
- expedition,
- trade,
- combat,
- hidden agenda/spy state.

---

# 65. 203A Phase BG — Diagnostics

Expose:

```text
INTEL_INFORMANTS_ACTIVE
INTEL_INFORMANTS_COMPROMISED
INTEL_REPORTS_TOTAL
INTEL_REPORTS_ACTIONABLE
INTEL_REPORTS_VERIFIED
INTEL_REPORTS_DISPROVED
INTEL_OPERATIONS_ACTIVE
INTEL_COUNTER_CASES_ACTIVE
INTEL_RUMOR_REQUESTS_ACTIVE
INTEL_REQUIRED_PORTS_MISSING
```

---

# 66. 203A Tests

- informant type load,
- reliability/access separation,
- report confidence,
- independent-source verification,
- copied rumor not double-verification,
- operation deterministic outcome,
- compromise persistence,
- no local assassination/sabotage executor,
- no local rumor propagation,
- old-save empty state,
- missing-port validation.

---

# 67. 203A Definition of Done

- [ ] IntelligenceNetworkSystem,
- [ ] normalized state model,
- [ ] 5 canonical informant archetypes,
- [ ] 10+ informant templates/profiles,
- [ ] 7+ report types,
- [ ] evidence provenance,
- [ ] reliability/confidence/truth separation,
- [ ] verification states,
- [ ] freshness,
- [ ] actionability projection,
- [ ] operation model,
- [ ] surveillance/infiltration/extraction/recruitment/analysis,
- [ ] Plan 157 interception adapter,
- [ ] no duplicate sabotage/assassination executor,
- [ ] counter-intelligence case model,
- [ ] save/old-save,
- [ ] deterministic RNG,
- [ ] composition root,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 68. Workstream 203B — Informant Network

## Goal

Make sources feel like strategic relationships with access, risk, motive, payment, and uncertainty—not passive report generators.

---

# 69. 203B Phase A — Recruitment Sources

Potential recruitment channels:

- trade contact,
- defector,
- visitor,
- faction contact,
- expedition encounter,
- captured operative,
- local resident,
- survivor field agent.

Each requires a canonical source event.

---

# 70. 203B Phase B — Field Agent

Identity:

- player survivor.

Properties:

- high control,
- direct exposure,
- strong observational evidence,
- unavailable for normal shelter work while deployed.

Use ExpeditionSystem for physical deployment.

---

# 71. 203B Phase C — Placed Spy

Identity:

- survivor or faction contact embedded in target.

Properties:

- high access,
- slow reporting,
- high compromise risk,
- strong long-term evidence.

If Plan 153 owns placed-agent lifecycle:

- reference it instead of duplicating.

---

# 72. 203B Phase D — Trade Contact

Identity:

- trader/contact.

Properties:

- broad rumor/trade information,
- low military access,
- low direct risk,
- payment/trade reciprocity.

---

# 73. 203B Phase E — Local Source

Identity:

- resident/local observer.

Properties:

- high local accuracy,
- low strategic access,
- can report movement/resource/terrain.

---

# 74. 203B Phase F — Defector

Identity:

- former faction member.

Properties:

- high historical/internal access,
- freshness decays after defection,
- variable motive/reliability.

---

# 75. 203B Phase G — Informant Template Data

Create or include in:

```text
Assets/StreamingAssets/Data/intelligence_templates.json
```

Recommended sections:

```text
informant_templates
report_templates
operation_profiles
verification_profiles
counter_intelligence_profiles
rumor_operation_profiles
```

---

# 76. 203B Phase H — Informant Template Count

At least 10 templates.

Each template maps to one of five archetypes.

---

# 77. 203B Phase I — Informant Cost

Cost can be:

- currency,
- food,
- medicine,
- access,
- favors,
- protection,
- trade concession.

Use canonical transactions.

---

# 78. 203B Phase J — Informant Loyalty / Motive

Do not add generic loyalty meter unless needed.

Behavior can derive from:

- motive,
- payment status,
- faction relation,
- coercion,
- exposure.

---

# 79. 203B Phase K — Contact Windows

Some informants only contact:

- every N days,
- during trade visit,
- through radio,
- at specific location.

This creates operational texture.

---

# 80. 203B Phase L — Contact Failure

No contact can mean:

- delayed,
- communication blocked,
- source unavailable,
- source compromised.

Do not always reveal why immediately.

---

# 81. 203B Phase M — Cover Pressure

Cover risk grows from:

- repeated operations,
- high-risk access,
- missed extraction,
- target counter-intelligence,
- suspicious communication pattern.

---

# 82. 203B Phase N — Cover Recovery

Intact/suspicious source may reduce pressure by:

- going dormant,
- lowering contact frequency,
- changing channel.

No instant “pay to reset.”

---

# 83. 203B Phase O — Compromised Source

Player choices:

- extract,
- cut contact,
- continue risky use,
- attempt misinformation if another plan supports it.

Consequences explicit.

---

# 84. 203B Phase P — Blown Source

Source can no longer operate normally.

May:

- flee,
- be captured,
- become double agent,
- disappear.

Actual fate routes through canonical systems.

---

# 85. 203B Phase Q — Double Agent

Source quest hook asks to turn enemy informant.

Do not create a separate double-agent truth if Plan 153/132 owns it.

Intelligence record stores:

```text
source_role_ref
```

and consumes owner state.

---

# 86. 203B Phase R — Handler

Optional:

- survivor assigned to manage source.

Benefits:

- contact reliability,
- cover safety,
- analysis.

Costs:

- survivor time.

Only implement if duty/work system supports.

---

# 87. 203B Phase S — Informant Network Read Model

For each source show:

- type,
- coverage,
- contact status,
- known reliability band,
- access band,
- cover status,
- payment status,
- recent reports.

No hidden exact deception probability.

---

# 88. Workstream 203B — Intelligence Reports and Verification

## Goal

Make information quality the heart of the system.

---

# 89. 203B Phase T — Report Generation

A report is produced when:

```text
evidence opportunity
+ source access
+ operation/contact
→ report template binding
```

Not on fixed daily spam.

---

# 90. 203B Phase U — 20+ Report Templates

Recommended distribution:

```text
FactionMovement: 4
MilitaryStrength: 3
ResourceCache: 3
LeadershipChange: 2
PlotDiscovered: 3
TerrainIntel: 3
TradeIntel: 3
```

= 21 templates.

Adjust based on real systems.

---

# 91. 203B Phase V — Report Template Fields

```text
report_template_id
report_type
eligible evidence sources
subject types
freshness policy
content template key
confidence weights
actionability profile
verification requirements
```

---

# 92. 203B Phase W — Faction Movement

May include:

- patrol concentration,
- troop relocation,
- convoy,
- raid staging,
- border movement.

Truth source remains faction system.

---

# 93. 203B Phase X — Military Strength

Estimate:

- manpower band,
- equipment band,
- readiness,
- defenses.

Do not expose exact hidden combat stats unless fully verified and design permits.

---

# 94. 203B Phase Y — Resource Cache

Can reveal:

- approximate location,
- resource category,
- guarded status.

Use canonical location/loot source.

---

# 95. 203B Phase Z — Leadership Change

May identify:

- new leader,
- faction split,
- succession conflict.

Faction system owns actual leadership state.

---

# 96. 203B Phase AA — Plot Discovered

Examples:

- raid plan,
- sabotage plan,
- embargo,
- assassination plot.

Only if authoritative external system has such threat.

Do not invent plot to fill report quota.

---

# 97. 203B Phase AB — Terrain Intel

Use Plan 32/163 world knowledge.

Can improve:

- route knowledge,
- hazard knowledge,
- location confidence.

No duplicate map state.

---

# 98. 203B Phase AC — Trade Intel

Use market/trade authority.

Can report:

- demand,
- shortages,
- route opening,
- price trend.

Do not set market price locally.

---

# 99. 203B Phase AD — Report Content

Use localized templates with parameters.

Store stable refs.

Avoid frozen English truth text.

---

# 100. 203B Phase AE — Uncertainty Language

Map confidence to wording.

Example:

```text
Low
→ "A source claims..."

Medium
→ "Multiple indications suggest..."

High
→ "Evidence strongly indicates..."

Verified
→ "Confirmed by..."
```

---

# 101. 203B Phase AF — Confidence Reason Codes

Examples:

```text
single_source
trusted_source
deep_access
intercept_corroboration
expedition_corroboration
stale_observation
conflicting_reports
same_origin_rumor
```

---

# 102. 203B Phase AG — Verification Operation

Player can choose:

- assign analyst,
- seek second source,
- intercept communications,
- send reconnaissance,
- question trade contact.

Each uses canonical systems.

---

# 103. 203B Phase AH — Partial Verification

Occurs when:

- some details corroborated,
- core claim unresolved.

Do not flatten to true/false.

---

# 104. 203B Phase AI — Verified

Requires evidence threshold appropriate to report type.

For moving targets, verification can still become stale later.

---

# 105. 203B Phase AJ — False

Triggered when canonical evidence disproves claim sufficiently.

Keep historical false report for source reliability learning.

---

# 106. 203B Phase AK — Contradictory Reports

Show both.

Create assessment:

```text
Conflicting
```

Do not silently overwrite older report.

---

# 107. 203B Phase AL — Supersession

Newer verified report may supersede old one.

Keep history but hide stale by default.

---

# 108. 203B Phase AM — Expiration

At expiry:

- report remains historical,
- loses actionable status.

No hard deletion immediately.

---

# 109. 203B Phase AN — Actionable Queue

Curated queue includes only:

- relevant,
- current,
- high enough confidence,
- response available.

This is the player's main operational surface.

---

# 110. 203B Phase AO — Intelligence Flood Guard

Group:

```text
routine patrol reports
trade chatter
low-confidence rumor repeats
```

into summaries.

Do not notify each.

---

# 111. 203B Phase AP — Daily Intelligence Brief

Optional read model:

```text
Top actionable intelligence
New confirmations
Critical contradictions
Compromised sources
Expiring opportunities
```

No new truth store.

---

# 112. Workstream 203B — Rumor Planting / Countering

## Goal

Let intelligence initiate information operations without reimplementing Plan 131 propagation or Plan 168 persuasion.

---

# 113. 203B Phase AQ — Rumor Object Scope

Do not persist a second full rumor propagation object.

Recommended:

```text
RumorOperationLink
{
    operation_id
    message_id
    target_refs
    propagation_ref
    propaganda_ref
    objective
    status
}
```

---

# 114. 203B Phase AR — Rumor Types

Source types:

```text
FactionRumor
ShelterRumor
PersonRumor
LocationRumor
EventRumor
```

Treat as message classification.

---

# 115. 203B Phase AS — Plant Rumor Flow

```text
choose objective/message
→ validate known/fabricated claim policy
→ select insertion channel/source
→ Plan 168 resolves propaganda/disinformation eligibility/effect model
→ Plan 131 propagates information
→ intelligence records campaign link
```

---

# 116. 203B Phase AT — False Rumor / Disinformation

If player intentionally plants false information:

- mark internally as disinformation operation,
- Plan 168 owns truth/effect mechanics,
- moral/faction consequences route externally.

---

# 117. 203B Phase AU — Rumor Credibility

Do not maintain separate local credibility if Plan 131/168 already owns it.

Intelligence UI reads authoritative credibility/spread from those systems.

---

# 118. 203B Phase AV — Rumor Spread

Source says local → regional → widespread.

That belongs to Plan 131.

Intelligence panel can display it.

---

# 119. 203B Phase AW — Counter Rumor

Counter operation:

```text
identify hostile rumor
→ gather evidence
→ choose rebuttal/counter-message
→ Plan 168 resolves message effect
→ Plan 131 propagates
```

No local `spreadLevel -= 20`.

---

# 120. 203B Phase AX — Counter-Rumor Evidence

Verified intelligence may improve counter-message credibility.

Plan 168 decides final effect.

---

# 121. 203B Phase AY — Enemy Rumors

Detected via:

- Plan 131 propagation,
- Plan 168 propaganda campaign,
- informant report.

Counter-intelligence may open a case.

---

# 122. 203B Phase AZ — Rumor Outcome UI

Display:

- objective,
- target,
- current spread from Plan 131,
- credibility/effect summary from owner,
- discovered counter-effort if known.

---

# 123. Workstream 203B — Intelligence Operations

## Goal

Provide deliberate intelligence activities with real time/resource/risk costs and multiple outcome dimensions.

---

# 124. 203B Phase BA — Operation Planning

Player selects:

- operation type,
- target,
- agents/source,
- supporting intel,
- resources,
- duration/profile.

---

# 125. 203B Phase BB — Agent Eligibility

Use canonical survivor:

- availability,
- skills,
- traits,
- health,
- faction identity/history,
- equipment.

No intelligence-only agent stats unless registered in SkillProgression.

---

# 126. 203B Phase BC — Operation Skill Domains

Potential existing skills:

- social,
- stealth,
- observation,
- radio,
- combat,
- trade,
- analysis.

Audit before adding spy skill.

---

# 127. 203B Phase BD — Surveillance

Low risk.

Outputs:

- observation evidence,
- movement report,
- terrain/faction details.

---

# 128. 203B Phase BE — Interception

Adapter only.

Inputs:

- request/channel target.

Plan 157 returns:

- intercept success,
- content,
- confidence/metadata.

Intelligence turns into evidence/report.

---

# 129. 203B Phase BF — Infiltration

High risk.

Stages:

```text
approach
establish cover
gain access
operate
withdraw/continue
```

If Plan 153 already models this:

- call it.

---

# 130. 203B Phase BG — Extraction

High risk.

Can create:

- expedition,
- trade/faction exchange,
- covert extraction.

Do not teleport source to shelter.

---

# 131. 203B Phase BH — Recruitment

Very high risk.

Potential outcomes:

- recruited,
- refused,
- suspicious,
- compromise,
- double-agent risk.

---

# 132. 203B Phase BI — Analysis

Low physical risk.

Consumes:

- analyst time,
- reports,
- possibly equipment.

Can:

- merge evidence,
- reveal contradiction,
- increase confidence.

---

# 133. 203B Phase BJ — Operation Resource Costs

Use:

- supplies,
- bribes,
- equipment,
- radio time,
- transport,
- safehouse if such system exists.

No abstract “intel points” unless design intentionally adds them.

---

# 134. 203B Phase BK — Information as Resource

The strategic resource is:

```text
verified/fresh/actionable information
```

not necessarily a fungible currency.

Avoid generic `intel_points` unless another economy plan demands it.

---

# 135. 203B Phase BL — Operation Risk Breakdown

Show:

```text
exposure risk
capture/injury risk
source compromise risk
information-quality uncertainty
faction reaction risk
```

No single opaque 73%.

---

# 136. 203B Phase BM — Operation Preview

Player sees bands:

```text
Low
Moderate
High
Extreme
```

with reasons.

Exact deterministic chance may remain hidden depending UI philosophy.

---

# 137. 203B Phase BN — Operation Resolution

Commit:

```text
primary outcome
secondary compromise
evidence generated
costs consumed
injury/capture refs
faction reaction refs
```

once.

---

# 138. 203B Phase BO — Partial Success

Important outcome.

Examples:

- report but source suspicious,
- incomplete evidence,
- target identity unknown,
- route observed but not strength.

---

# 139. 203B Phase BP — Compromise

Can occur even on success.

This creates source-management tension.

---

# 140. 203B Phase BQ — Injury / Capture

Route to:

- expedition/combat/fate/roster system.

Intelligence records outcome ref.

---

# 141. 203B Phase BR — Operation Cancellation

Before start:

- release reserved resources according to policy.

After start:

- partial cost remains,
- potential exposure depends on stage.

---

# 142. Workstream 203B — Counter-Intelligence

## Goal

Let the player detect hostile information threats while preserving Plan 132/153 ownership of hidden actors and covert attacks.

---

# 143. 203B Phase BS — Threat Evidence

Counter-intel cases begin from evidence:

- suspicious access,
- leak pattern,
- intercepted message,
- conflicting logs,
- informant warning,
- hidden agenda clue,
- propaganda campaign.

---

# 144. 203B Phase BT — Threat Confidence

Same evidence-quality model as normal intelligence.

No magic detection bar.

---

# 145. 203B Phase BU — Investigation

Player can:

- assign investigator,
- audit records,
- restrict access,
- interview suspect,
- cross-check reports,
- monitor communications.

Use canonical work/security systems.

---

# 146. 203B Phase BV — Enemy Spy

If external actor exists:

- counter case references actor.
- identity remains hidden until evidence threshold.

---

# 147. 203B Phase BW — Information Leak

Detect through:

- access patterns,
- Plan 131 propagation anomaly,
- faction knowledge the player did not intentionally share.

Do not implement omniscient leak detector.

---

# 148. 203B Phase BX — Propaganda Campaign

Detected via Plan 168/131.

Counter-intelligence decides:

- investigate origin,
- gather rebuttal evidence,
- initiate counter-message.

---

# 149. 203B Phase BY — Assassination Plot

Intelligence can discover the plot.

Security/combat/Plan 153 owns prevention/execution.

---

# 150. 203B Phase BZ — Sabotage Plot

Same boundary.

---

# 151. 203B Phase CA — Neutralization

Counter case resolves when owner system reports:

- source expelled,
- spy detained,
- plot disrupted,
- leak closed,
- counter-message effective.

---

# 152. 203B Phase CB — False Accusation

Counter-intelligence should support:

- wrong suspect,
- uncertain evidence,
- political cost.

Do not guarantee investigator correctness.

---

# 153. 203B Phase CC — Double-Agent Risk

Recruiting hostile source may create:

- true asset,
- false asset,
- ambiguous source.

If Plan 153 owns double agents, consume its result.

---

# 154. Workstream 203B — UI

## Goal

Make uncertain information legible without drowning the player in raw reports.

---

# 155. 203B Phase CD — Intelligence Dashboard

Primary sections:

```text
Actionable
Sources
Reports
Operations
Counter-Intelligence
Rumor Operations
Map
History
```

---

# 156. 203B Phase CE — Actionable First

Default view should show:

- top actionable reports,
- expiring opportunities,
- compromised sources,
- critical contradictions.

Not chronological noise.

---

# 157. 203B Phase CF — Informant Detail

Show:

- archetype,
- coverage,
- access band,
- reliability band,
- cover status,
- payment,
- last contact,
- report history.

---

# 158. 203B Phase CG — Report Detail

Show:

- report type,
- subject,
- source lineage,
- received/observed day,
- freshness,
- confidence,
- verification,
- uncertainties,
- corroboration,
- available actions.

---

# 159. 203B Phase CH — Confidence Visualization

Never color-only.

Use:

- label,
- icon,
- bar/shape,
- tooltip.

Plan 184 accessibility.

---

# 160. 203B Phase CI — Report Comparison

Allow compare:

- conflicting movement reports,
- two strength estimates,
- old vs new trade intel.

---

# 161. 203B Phase CJ — Rumor Panel

Reads Plan 131/168 state.

Shows:

- message/objective,
- spread,
- credibility/effect,
- target,
- response.

No shadow rumor list with separate truth.

---

# 162. 203B Phase CK — Operations Panel

Show:

- planned,
- active,
- resolved,
- risk,
- agents,
- duration,
- current known status.

---

# 163. 203B Phase CL — Counter-Intel Panel

Show only detected/suspected cases.

Never list latent hidden spies.

---

# 164. 203B Phase CM — Intelligence Map

Overlay:

- source coverage,
- report subjects,
- confidence,
- faction/route intel.

Use canonical map/knowledge system.

No hidden location leak.

---

# 165. 203B Phase CN — Map Confidence

Marker may be:

```text
approximate
confirmed
stale
```

based on report.

Do not turn report into canonical mapped location automatically unless Plan 32/163 transition permits.

---

# 166. 203B Phase CO — Notifications

Notify:

- high-actionability report,
- source compromised,
- operation resolution,
- critical counter-intel case,
- major verification.

Group low-priority reports.

---

# 167. 203B Phase CP — Intelligence Tutorial

First informant:

Explain:

- source reliability,
- access,
- verification,
- freshness,
- uncertainty,
- operations.

---

# 168. 203B Phase CQ — Tooltips / Details Accessibility

Hover is optional.

Details reachable by:

- focus,
- controller,
- keyboard.

---

# 169. 203B Phase CR — Journal / Archive

Journal can log:

- important report,
- failed operation,
- source loss.

Plan 162 archive only landmark intelligence events.

---

# 170. 203B Phase CS — Intelligence Events

Preserve source eight:

```text
The Report
The Informant
The Rumor
The Operation
The Compromise
The Counter
The Verification
The Failure
```

Use semantic/narrative events.

---

# 171. 203B Phase CT — Quest/Achievement Hooks

Source:

```text
The Spymaster
The Analyst
The Puppet Master
The Counter
The Deep Cover
The Intelligence Chief
The Double Agent
```

If Plan 149 owns achievements:

- these become achievement criteria there.

No second achievement engine.

---

# 172. 203B Phase CU — Spymaster

Build network of 10 active informants.

Count active distinct sources.

---

# 173. 203B Phase CV — Analyst

Verify 20 reports.

Do not count repeated verification of same report.

---

# 174. 203B Phase CW — Puppet Master

Source says 5 successful rumors.

Success must consume Plan 131/168 authoritative outcome.

---

# 175. 203B Phase CX — Counter

Neutralize 3 enemy intelligence threats.

Counter case resolution references actual owner outcome.

---

# 176. 203B Phase CY — Deep Cover

Maintain placed spy for 100 days.

Must remain:

- active,
- unblown,
- target-embedded.

---

# 177. 203B Phase CZ — Intelligence Chief

Source says network reputation 90+.

If network credibility is derived:

- achievement reads derived metric.

---

# 178. 203B Phase DA — Double Agent

Turn one eligible hostile source.

Use Plan 153 ownership where present.

---

# 179. 203B Phase DB — Localization

All report templates/UI labels use localization keys.

Source names/locations/factions resolve by ID.

---

# 180. 203B Phase DC — Content Integrity Matrix

Informants:

| Template | Archetype | Coverage | Motive | Cost | Recruitment source | Runtime observed |
|---|---|---|---|---|---|---:|

Reports:

| Template | Type | Evidence sources | Freshness | Verification | Action consumers | Runtime observed |
|---|---|---|---|---|---|---:|

---

# 181. 203B Phase DD — Content Utilization

Run targeted scenario.

Report:

```text
informants recruited
reports received
verified
disproved
expired
operations
compromises
counter cases
rumor operations
actionable reports
```

---

# 182. 203B Phase DE — Dead Content Policy

Never-observed template:

- fix,
- mark intentionally rare,
- remove,
- exempt with reason.

---

# 183. 203B Definition of Done

- [ ] informant recruitment,
- [ ] payment/contact/cover,
- [ ] 5 archetypes,
- [ ] 10+ informant templates,
- [ ] 7+ report types,
- [ ] 20+ report templates,
- [ ] report confidence/verification/freshness,
- [ ] independent-source corroboration,
- [ ] actionable queue,
- [ ] rumor operation adapter,
- [ ] Plan 131 propagation,
- [ ] Plan 168 effect integration,
- [ ] surveillance,
- [ ] interception adapter,
- [ ] infiltration,
- [ ] extraction,
- [ ] recruitment,
- [ ] analysis,
- [ ] counter-intelligence cases,
- [ ] threat investigation,
- [ ] dashboard,
- [ ] informant/report/rumor/operations/counter panels,
- [ ] intelligence map,
- [ ] events/hooks,
- [ ] tutorial/tooltips,
- [ ] localization,
- [ ] utilization report.

---

# 184. Workstream 203C — Faction Stance Integration

## Goal

Turn intelligence outcomes into reason-coded faction consequences without duplicating stance.

---

# 185. 203C Phase A — `FactionStanceEngine`

Intelligence may affect stance through:

- exposed spying,
- valuable shared intelligence,
- false rumor attribution,
- successful counter-intel cooperation,
- betrayal.

Use canonical faction reason APIs.

---

# 186. 203C Phase B — No Direct Trust Mutation

Forbidden:

```text
faction.trust += 10
```

inside intelligence code.

---

# 187. 203C Phase C — Source Exposure

If faction discovers informant:

- stance consequence belongs to faction system.

---

# 188. 203C Phase D — Intelligence Sharing

If player shares verified report:

- faction can evaluate novelty/value.

Do not grant trust for already-known information.

---

# 189. Workstream 203C — Faction Branch Coordinator

## Goal

Use intelligence to reveal plans without changing faction plan truth.

---

# 190. 203C Phase E — Branch Plan Evidence

FactionBranchCoordinator may expose:

```text
intelligence-observable plan facts
```

through interface.

---

# 191. 203C Phase F — Reveal Levels

Intelligence can reveal:

```text
intent
probable branch
confirmed branch
timing window
```

based on access/verification.

---

# 192. 203C Phase G — No Branch Mutation

Intelligence never selects faction branch.

---

# 193. Workstream 203C — Trade Integration

## Goal

Make trade contacts and trade intelligence real.

---

# 194. 203C Phase H — HoldfastTradeSession

Trade contact recruitment originates from:

- existing trade partner/contact.

---

# 195. 203C Phase I — Trade Intel

Can expose:

- demand,
- route risk,
- shortage,
- likely supply.

Market remains authoritative.

---

# 196. 203C Phase J — Intel Trading

Source follow-on suggests intelligence trading.

Baseline can support sharing/selling verified reports if trade system supports intangible goods/contracts.

Otherwise defer.

No fake inventory item needed.

---

# 197. Workstream 203C — Signal Triangulation / Radio

## Goal

Turn intercepted signals into evidence without duplicating radio mechanics.

---

# 198. 203C Phase K — `SignalTriangulationSystem`

Can produce:

- approximate location evidence,
- signal identity,
- movement/relay hints.

---

# 199. 203C Phase L — Plan 157 Interception

Plan 157 owns:

- interception,
- encryption,
- jamming,
- delivery.

Intelligence only consumes result.

---

# 200. 203C Phase M — Decryption

If Plan 157 owns decryption:

- consume decoded content.

If not:

- intelligence analysis may request decryption service but must not duplicate crypto network.

---

# 201. Workstream 203C — Expedition Integration

## Goal

Use physical reconnaissance as high-quality intelligence evidence.

---

# 202. 203C Phase N — Field Agent Expedition

Field agent assignment uses ExpeditionSystem.

No second travel lifecycle.

---

# 203. 203C Phase O — Recon Objective

Expedition objective:

```text
Observe
VerifyReport
ContactSource
ExtractSource
ScoutRoute
```

---

# 204. 203C Phase P — Observation Evidence

Real visited location can corroborate:

- movement,
- cache,
- terrain,
- military strength.

---

# 205. 203C Phase Q — Expedition Risk

Intelligence system does not reduce actual expedition risk unless confirmed intel feeds canonical planning modifier.

---

# 206. Workstream 203C — Combat Integration

## Goal

Make military intelligence useful without changing enemy hidden truth.

---

# 207. 203C Phase R — Combat Intelligence Read Model

Provide:

```text
estimated enemy strength
known equipment
known defenses
known tactics
confidence/freshness
```

---

# 208. 203C Phase S — Planning Benefit

Verified intelligence may improve:

- preview accuracy,
- route/position planning,
- ambush avoidance,
- tactical preparation.

Do not arbitrarily apply:

```text
+20% combat success
```

unless combat system has an explicit intel preparedness input.

---

# 209. 203C Phase T — Wrong Intelligence

False/stale intel can cause:

- misleading estimate,
- bad preparation choice.

It should not secretly alter enemy stats.

---

# 210. Workstream 203C — Plan 131 Integration

## Goal

Use one world information-flow system.

---

# 211. 203C Phase U — Rumor Propagation

All spread:

```text
settlement → settlement
faction → faction
traveler → player
```

belongs to Plan 131.

---

# 212. 203C Phase V — Intelligence Intake

Plan 131 rumor may become:

```text
PublicRumor evidence
```

with low/unknown provenance.

---

# 213. 203C Phase W — Intelligence Output

Player rumor operation creates:

```text
information insertion request
```

into Plan 131.

---

# 214. 203C Phase X — Provenance

Reuse Plan 131 propagation provenance to avoid false corroboration.

---

# 215. Workstream 203C — Plan 168 Integration

## Goal

Use one propaganda/disinformation effect authority.

---

# 216. 203C Phase Y — Rumor Effect

Plan 168 decides:

- belief,
- morale,
- faction influence,
- backlash/resistance.

---

# 217. 203C Phase Z — Verified Evidence Advantage

Intelligence may pass:

- verification quality,
- evidence strength

as Plan 168 inputs if its contract supports them.

Do not multiply outcome locally.

---

# 218. Workstream 203C — Plan 153 Integration

## Goal

Prevent duplicate espionage/sabotage systems.

---

# 219. 203C Phase AA — Espionage Execution

If Plan 153 owns:

- infiltration,
- sabotage,
- assassination,
- double agents,

C2[41] delegates.

Intelligence contributes:

- target,
- evidence,
- access,
- source.

---

# 220. 203C Phase AB — Operation Ownership Matrix

Create:

| Operation | Plan 203 | Plan 153 | Plan 157 | Expedition/Combat |
|---|---|---|---|---|
| Surveillance | owns | may consume | | expedition adapter |
| Interception | consumes | | owns | |
| Infiltration | orchestration/adapter | owns if implemented | | |
| Recruitment | source network | may own covert turning | | |
| Extraction | intent/evidence | may own covert context | | expedition executes |
| Sabotage | targeting only | owns | | |
| Assassination | targeting only | owns/Combat | | combat executes |

---

# 221. Workstream 203C — Hidden Agenda / Shelter Spy Integration

## Goal

Avoid a second infiltrator identity model.

---

# 222. 203C Phase AC — Plan 132

A survivor hidden agenda may create:

- suspicion,
- leak evidence,
- spy case.

But IntelligenceNetworkSystem cannot inspect hidden truth directly for player UI.

---

# 223. 203C Phase AD — Knowledge Boundary

Counter-intel only sees:

- discovered clues,
- behavioral evidence,
- intercepted evidence.

---

# 224. Workstream 203C — Save / Load / Idempotency

## Goal

Make every source/report/operation outcome durable before consequences.

---

# 225. 203C Phase AE — Save Matrix

Test:

```text
informant recruited
payment due
source suspicious
report received
verification in progress
report verified
operation planned
operation active
operation outcome committed
source compromised
counter case suspected
counter case investigating
rumor request active
```

---

# 226. 203C Phase AF — Report Generation Idempotency

Same evidence opportunity:

```text
one report
```

unless template intentionally permits revision/update.

---

# 227. 203C Phase AG — Operation Outcome Idempotency

Same operation ID:

```text
one outcome
```

---

# 228. 203C Phase AH — Verification Idempotency

Same evidence cannot repeatedly grant confidence.

---

# 229. 203C Phase AI — Rumor Operation Idempotency

Same insertion request:

- one Plan 131 propagation object/campaign.

---

# 230. 203C Phase AJ — Counter Case Idempotency

Same hostile operation/evidence cluster:

- one counter case.

---

# 231. 203C Phase AK — Old Save

Empty network.

No fabricated reports.

---

# 232. Workstream 203C — Exploit Prevention

## Goal

Prevent information, money, reputation, verification, and operation reroll exploits.

---

# 233. 203C Phase AL — Save-Scum Operation Reroll

Persist:

- RNG key,
- operation outcome,
- compromise result

before downstream effects.

---

# 234. 203C Phase AM — Source Recruitment Reroll

Recruitment attempt has stable ID.

Reload cannot reroll.

---

# 235. 203C Phase AN — Verification Farming

Same evidence cannot be applied repeatedly.

---

# 236. 203C Phase AO — Rumor Echo Verification Exploit

Same-origin rumor repeated across multiple nodes does not count as independent corroboration.

---

# 237. 203C Phase AP — Trade Contact Farm

Repeatedly opening trade session does not generate new reports.

Evidence opportunity keyed to:

- trade event/day/contact/context.

---

# 238. 203C Phase AQ — Expedition Recon Farm

Repeated same-route visit with unchanged world state:

- diminishing/no new evidence unless observation materially changes.

---

# 239. 203C Phase AR — Informant Payment Exploit

Cannot:

```text
receive daily report
→ avoid payment by save/load timing
```

Payment/contact transaction idempotent.

---

# 240. 203C Phase AS — Source Duplication

Same NPC/contact cannot be recruited twice into parallel identical informant records.

---

# 241. 203C Phase AT — Rumor Reward Farm

Repeated planting of same message at same target:

- no repeated standing/achievement reward.

Plan 131/168 novelty/provenance helps.

---

# 242. 203C Phase AU — Counter-Intel Farm

Cannot manufacture/resolve fake threat cases.

Cases require external threat/evidence.

---

# 243. Workstream 203C — Edge Cases

## Goal

Ensure sparse and dense intelligence campaigns remain playable.

---

# 244. 203C Phase AV — No Network

Expected:

- no reports,
- no operations,
- no errors,
- other game systems function normally.

---

# 245. 203C Phase AW — Extensive Network

Stress:

- 10–30 informants,
- hundreds of reports,
- multiple operations.

UI groups and filters.

---

# 246. 203C Phase AX — All Sources Compromised

Network temporarily collapses.

Player can rebuild.

No permanent campaign soft-lock.

---

# 247. 203C Phase AY — Zero Reliable Sources

Reports remain low-confidence.

Verification through expeditions/intercepts still possible.

---

# 248. 203C Phase AZ — Conflicting High-Quality Sources

Show conflict.

Do not arbitrarily choose one without evidence.

---

# 249. 203C Phase BA — Faction Destroyed

Reports about destroyed faction:

- expire/supersede,
- informants reclassify,
- operations cancel/transform.

---

# 250. 203C Phase BB — Location Destroyed/Changed

Terrain/cache reports stale through LocationEvolution.

---

# 251. 203C Phase BC — Source Dies

Informant becomes lost/dead according to canonical roster/NPC state.

Existing reports remain historical.

---

# 252. 203C Phase BD — Source Defects

Future reliability affected.

Past verified reports remain verified.

---

# 253. 203C Phase BE — False Intelligence Cascade

One false report may influence player decision.

Do not automatically spread falsehood into faction truth.

---

# 254. 203C Phase BF — Counter-Intelligence False Positive

Player may investigate wrong suspect.

Consequences route to relations/morale/faction/security.

---

# 255. Workstream 203C — Determinism

## Goal

Guarantee reproducible operations and report generation.

---

# 256. 203C Phase BG — RNG Stream

Use:

```text
intelligence
```

No wall-clock randomness.

---

# 257. 203C Phase BH — Stable Candidate Ordering

Sort:

- informants,
- targets,
- report templates,
- evidence,
- operations.

---

# 258. 203C Phase BI — Same-Seed Digest

Build:

```text
intelligence_history_digest
```

from:

- source IDs/states,
- report IDs/confidence,
- operation outcomes,
- counter cases,
- rumor links.

---

# 259. 203C Phase BJ — Same Inputs

Same:

```text
campaign seed
informant recruitment
payments
operations
external faction truth
external radio/expedition evidence
```

→ same intelligence history.

---

# 260. Workstream 203C — Data Integrity

## Goal

Fail early on broken intelligence content.

---

# 261. 203C Phase BK — Template Validation

Validate:

- unique IDs,
- valid informant archetype,
- valid report type,
- valid subject type,
- valid operation profile,
- localization key,
- valid freshness policy.

---

# 262. 203C Phase BL — Reference Validation

Validate:

- faction IDs,
- location IDs,
- skill/trait IDs if used,
- item/resource IDs,
- Plan 131 message/rumor profiles,
- Plan 168 propaganda profiles,
- Plan 157 signal/intercept types,
- quest/achievement refs.

---

# 263. 203C Phase BM — Forbidden Ownership Scan

Within intelligence implementation, flag creation of:

```text
FactionStanceEngine duplicate
Broadcast/interception network
RumorPropagationGraph
PropagandaEffectResolver
CombatSystem
SabotageExecutor
AssassinationExecutor
```

unless explicit adapter/test fake.

---

# 264. 203C Phase BN — Rumor Schema Gate

`intelligence_templates.json` must not contain:

- propagation graph,
- settlement spread simulation,
- faction belief engine.

Only message/operation profiles and adapter refs.

---

# 265. 203C Phase BO — Operation Ownership Gate

Fail if Plan 203 directly mutates:

- faction military truth,
- market truth,
- combat stats,
- sabotage state,
- radio range.

---

# 266. Workstream 203C — `--intelligence-network-selftest`

Required scenarios:

1. recruit field agent,
2. recruit trade contact,
3. recruit local source,
4. recruit defector,
5. placed-spy adapter,
6. payment processing,
7. missed payment,
8. report generation,
9. report expiry,
10. independent-source partial verification,
11. full verification,
12. false report,
13. conflicting reports,
14. surveillance,
15. interception via Plan 157 fake,
16. infiltration adapter,
17. extraction,
18. recruitment,
19. analysis,
20. source compromise,
21. counter-intel case,
22. hostile rumor detection,
23. rumor insertion request,
24. counter-rumor request,
25. faction stance effect handoff,
26. expedition recon evidence,
27. combat intelligence read model,
28. save/load,
29. old save,
30. no-network,
31. extensive-network,
32. same-seed replay.

---

# 267. Workstream 203C — Deliberate Failure Proof

Break:

- invalid faction ID,
- invalid location,
- duplicate informant template,
- invalid report type,
- copied-origin evidence counted twice,
- operation reroll key,
- missing Plan 131 adapter,
- local `RumorPropagationSystem`,
- local `PropagandaEffectResolver`.

Assert gate fails.

---

# 268. Workstream 203C — Balance / Information Overload

## Goal

Make intelligence strategic, not an inbox chore.

---

# 269. 203C Phase BP — 200-Day Intelligence Soak

Profiles:

```text
minimal_network
trade_contact_network
field_recon_network
deep_spy_network
counter_intel_heavy
balanced
```

Record:

```text
informants
payments
reports
actionable reports
verified reports
false reports
expired reports
operations
compromises
counter cases
rumor operations
```

---

# 270. 203C Phase BQ — Report Volume Budget

Measure:

```text
reports/day
actionable reports/day
notifications/day
```

Target:

- manageable,
- actionable-first.

No exact target before soak.

---

# 271. 203C Phase BR — Informant Maintenance Budget

Measure:

- resource cost/month,
- handler work,
- contact actions,
- compromised-source events.

Avoid micromanaging 10 daily payments manually.

Automate recurring payments if enabled.

---

# 272. 203C Phase BS — Source Value Profiles

Compare:

- field agent,
- placed spy,
- trade contact,
- local source,
- defector.

Each should have a distinct niche.

---

# 273. 203C Phase BT — Field Agent Value

Best for:

- fresh local observation,
- expedition-linked verification.

Cost:

- survivor exposure/time.

---

# 274. 203C Phase BU — Placed Spy Value

Best for:

- deep faction plans,
- leadership,
- long-term access.

Cost:

- high compromise/political risk.

---

# 275. 203C Phase BV — Trade Contact Value

Best for:

- trade/rumor/economic information.

Cost:

- lower depth.

---

# 276. 203C Phase BW — Local Source Value

Best for:

- terrain/movement/local events.

---

# 277. 203C Phase BX — Defector Value

Best for:

- historical/internal structure,
- one-time deep knowledge.

Freshness decays.

---

# 278. 203C Phase BY — Verification Value

Compare:

```text
act on unverified
verify first
ignore
```

Verification should reduce uncertainty but cost time/resources.

---

# 279. 203C Phase BZ — False Report Frequency

False information should be possible but not so common that intelligence is useless.

Tune by:

- source motive,
- reliability,
- counter-intel,
- propaganda exposure.

---

# 280. 203C Phase CA — Perfect Intelligence Prevention

Even excellent network should not reveal every hidden fact.

Use:

- coverage gaps,
- access limits,
- freshness,
- encryption,
- counter-intelligence.

---

# 281. 203C Phase CB — Counter-Intel Balance

Counter-intelligence should not make shelter immune to espionage.

It should improve:

- detection chance,
- response time,
- damage mitigation.

---

# 282. 203C Phase CC — Rumor Operation Balance

Rumor/disinformation should:

- cost resources/access,
- risk backlash,
- rely on Plan 131/168,
- not trivially rewrite faction stance.

---

# 283. 203C Phase CD — Combat Intel Balance

Intel helps preparation/preview.

It should not erase combat uncertainty.

---

# 284. 203C Phase CE — Network Reputation / Credibility Balance

If derived metric reaches 90+:

- requires long verified track record,
- false/compromised reports can lower it.

No grindable cosmetic bar.

---

# 285. Workstream 203C — UI / Accessibility

## Goal

Make uncertainty understandable and accessible.

---

# 286. 203C Phase CF — Plan 184 Large Text

Report/intelligence panels survive 2× text.

---

# 287. 203C Phase CG — No Color-Only Confidence

Confidence uses:

- text,
- icon,
- shape.

---

# 288. 203C Phase CH — Screen Reader Semantics

Reports announce:

```text
type
subject
confidence
verification
freshness
actionability
```

---

# 289. 203C Phase CI — Cognitive Load Reduction

Reduced-density mode shows:

```text
top claim
confidence
freshness
recommended action
```

Advanced evidence chain collapsible.

---

# 290. 203C Phase CJ — Hidden Information Safety

Accessibility tree must not expose:

- true report value,
- hidden faction plan,
- latent spy identity,
- secret source motive if not known.

---

# 291. 203C Phase CK — Keyboard / Controller

All report comparison/operation planning reachable without mouse.

---

# 292. Workstream 203C — Retention / Archive

## Goal

Keep intelligence history useful without unbounded report accumulation.

---

# 293. 203C Phase CL — Plan 55 Retention

Keep:

- active reports,
- verified landmark reports,
- false-report source history,
- major operations,
- unresolved counter cases.

Roll up:

- expired low-value movement chatter,
- repeated routine trade reports,
- routine contact logs.

---

# 294. 203C Phase CM — Source Accuracy Summary

Instead of retaining every minor historical report forever:

- keep source accuracy/verification summary.

Must be deterministically reconstructible or persisted as roll-up.

---

# 295. 203C Phase CN — Plan 162 Archive

Archive may record:

- major intelligence breakthrough,
- catastrophic intelligence failure,
- famous double agent,
- plot prevented,
- decisive verified report.

Not routine reports.

---

# 296. Workstream 203C — Performance

## Goal

Avoid O(all reports × all factions × all sources) daily processing.

---

# 297. 203C Phase CO — Event-Driven Report Generation

Reports generated from:

- source contact,
- operation resolution,
- intercept,
- expedition,
- trade event,
- external information event.

---

# 298. 203C Phase CP — Expiry Index

Index reports by expiry day.

Do not scan all history daily.

---

# 299. 203C Phase CQ — Active Source Tick

Daily only active sources/contracts.

---

# 300. 203C Phase CR — Counter Case Tick

Only active cases.

---

# 301. 203C Phase CS — UI Search/Filter

Index report fields for:

- subject,
- type,
- source,
- confidence,
- date.

Derived/rebuildable.

---

# 302. 203C Phase CT — Performance Budget

At extensive-network fixture:

- 30 sources,
- 5,000 historical reports,
- 20 active reports,
- 10 active operations.

Measure:

```text
daily tick
report search
dashboard build
save/load
```

---

# 303. Workstream 203C — Human Intelligence Playtest

## Goal

Validate that intelligence creates decisions rather than bookkeeping.

---

# 304. 203C Phase CU — Playtest Questions

```text
Did the player understand what was uncertain?
Did verification feel worth its cost?
Did a false report create a fair story rather than arbitrary punishment?
Did informants feel different?
Was there a meaningful reason to protect a source?
Did the intelligence panel surface the important information first?
Did rumor/counter-rumor operations feel connected to world information flow?
```

---

# 305. 203C Phase CV — Information Overload Test

Give player:

- 15 reports,
- 3 contradictions,
- 2 operations,
- 1 compromised source,
- 1 counter-intel threat.

Can they identify top 3 actions in under a short interaction?

Human review.

---

# 306. 203C Phase CW — Uncertainty Language Review

Avoid false precision.

If player sees:

```text
67% truthful
```

they may optimize mechanically.

Prefer bands + reasons unless exact analysis capability is deliberately unlocked.

---

# 307. Documentation

Create:

```text
docs/systems/INTELLIGENCE_NETWORK.md
```

Include:

- authority boundaries,
- evidence/report distinction,
- informant model,
- verification,
- operations,
- Plan 131 integration,
- Plan 153 integration,
- Plan 157 integration,
- Plan 168 integration,
- counter-intelligence,
- save/idempotency,
- adding report templates.

---

# 308. Intelligence Content Authoring Guide

Create:

```text
docs/content/INTELLIGENCE_TEMPLATE_AUTHORING.md
```

Checklist:

```text
1. identify canonical truth owner
2. define evidence source
3. define report type
4. define freshness
5. define uncertainty
6. define verification paths
7. define action consumers
8. define failure/false-report behavior
9. define localization
10. add targeted fixture
```

---

# 309. Integrated Intelligence Pipeline

```text
canonical world/faction event
         │
         ├─ informant observation
         ├─ radio intercept
         ├─ expedition recon
         ├─ trade contact
         └─ public rumor
         │
         ▼
     raw evidence
         │
         ▼
 IntelligenceNetworkSystem
         │
         ├─ report generation
         ├─ confidence
         ├─ verification
         └─ freshness
         │
         ▼
 intelligence assessment
         │
    ┌────┼────────┬──────────┬─────────────┐
    ▼    ▼        ▼          ▼             ▼
 Faction Expedition Combat  Trade     Rumor operation
                                          │
                                ┌─────────┴─────────┐
                                ▼                   ▼
                         Plan 131 spread      Plan 168 effects
```

---

# 310. Intelligence Authority Contract

Plan 203 owns:

- informant network,
- evidence references,
- intelligence reports,
- report confidence/verification,
- intelligence operation orchestration,
- counter-intelligence cases,
- intelligence history.

---

# 311. Faction Authority Contract

Faction systems own:

- actual faction plans,
- military state,
- leadership,
- standing/trust.

Intelligence observes/estimates.

---

# 312. Rumor Propagation Contract

Plan 131 owns:

- rumor/news propagation,
- spread topology,
- settlement/faction transmission.

Plan 203 requests/observes.

---

# 313. Propaganda Contract

Plan 168 owns:

- disinformation truth,
- persuasion/effect resolution,
- counter-message effect.

Plan 203 provides intelligence/evidence context.

---

# 314. Espionage Contract

Plan 153 owns:

- sabotage/assassination/covert execution where implemented,
- possibly infiltrator lifecycle.

Plan 203 owns evidence/network orchestration.

---

# 315. Radio Contract

Plan 157 owns:

- signal reach,
- interception,
- encryption,
- jamming.

Intelligence receives intercepted evidence.

---

# 316. Expedition Contract

ExpeditionSystem owns:

- party,
- travel,
- risk,
- return,
- survivor outcomes.

Intelligence defines reconnaissance/source objectives.

---

# 317. Trade Contract

Trade owner owns:

- price,
- inventory,
- transaction.

Intelligence may report estimates/opportunities.

---

# 318. Combat Contract

Combat owns:

- enemy stats,
- tactical resolution,
- casualties.

Intelligence may improve preparedness/preview.

---

# 319. Hidden Agenda Contract

Plan 132 owns:

- survivor hidden motivations/infiltration truth.

Counter-intelligence sees only discovered evidence.

---

# 320. Evidence Contract

Evidence is immutable provenance-bearing observation.

It is not equivalent to truth.

---

# 321. Reliability Contract

Reliability belongs to source history.

It is not report truth probability shown as omniscience.

---

# 322. Confidence Contract

Confidence derives from evidence/access/freshness/corroboration.

---

# 323. Verification Contract

Verification consumes independent evidence.

Same-origin rumor echoes do not count independently.

---

# 324. Freshness Contract

Time-sensitive reports become stale without being rewritten as false.

---

# 325. Actionability Contract

Actionability is derived from confidence + freshness + available response.

---

# 326. Informant Contract

Informant state includes:

- identity,
- type,
- access,
- coverage,
- reliability,
- motive,
- payment,
- cover.

No duplicate NPC/survivor state.

---

# 327. Counter-Intelligence Contract

Counter-intelligence owns:

- detection evidence,
- investigation,
- case state.

It does not own hostile attack execution.

---

# 328. Save Contract

Persist:

- sources,
- reports/evidence,
- operations,
- counter cases,
- rumor-operation links,
- idempotency.

Do not persist copies of external truth systems.

---

# 329. Old-Save Contract

Old saves:

```text
empty intelligence network
```

No historical backfill.

---

# 330. Determinism Contract

Same:

```text
seed
source network
operations
external authoritative events
player decisions
```

→ same intelligence history.

---

# 331. Information Overload Contract

Default UI shows actionable intelligence first.

Routine chatter is summarized.

---

# 332. Content Acceptance Contract

Every intelligence template progresses through:

```text
AUTHORED
→ LOADS
→ SOURCE_ELIGIBLE
→ EVIDENCE_PRODUCED
→ REPORT_CREATED
→ CONFIDENCE/VERIFICATION RESOLVED
→ ACTION CONSUMER AVAILABLE
→ PLAYER_VISIBLE
```

---

# 333. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| duplicates Plan 131 rumor propagation | High | Critical | request/adapter only |
| duplicates Plan 168 propaganda | High | Critical | result consumption only |
| duplicates Plan 153 sabotage/assassination | High | Critical | operation ownership matrix |
| reports become world truth | Medium | Critical | evidence/truth separation |
| repeated rumor counts as corroboration | High | High | provenance/origin-chain |
| save/load rerolls operations | Medium | Critical | commit outcome before effects |
| intelligence floods player | High | High | actionable-first + grouping |
| exact reliability becomes omniscient | Medium | Medium | player-facing bands |
| informants become passive free income | Medium | High | cost/contact/access |
| counter-intel reveals hidden spies too easily | Medium | High | evidence threshold |
| wrong intelligence directly alters enemy stats | Medium | High | planning preview only |
| deep network makes all uncertainty disappear | Medium | High | coverage/access/freshness limits |
| trade/intercept/expedition evidence duplicates owner state | Medium | High | evidence refs only |
| old saves retroactively generate reports | High | Medium | empty-state migration |

---

# 334. Commit Strategy

## 203A — Foundation

### C2[41].1 — baseline + intelligence authority ADR

### C2[41].2 — evidence/report/source DTOs

### C2[41].3 — informant archetypes/templates

### C2[41].4 — confidence/reliability/freshness model

### C2[41].5 — verification/provenance/origin-chain

### C2[41].6 — operation model + deterministic RNG

### C2[41].7 — counter-intelligence case model

### C2[41].8 — Plan 131/153/157/168 adapter contracts

### C2[41].9 — save/old-save/idempotency

### C2[41].10 — composition/events/diagnostics

### Gate: 203A complete

---

## 203B — Informants / Reports / Operations / UI

### C2[41].11 — field agent

### C2[41].12 — placed spy adapter

### C2[41].13 — trade contact / local source / defector

### C2[41].14 — payment/contact/cover lifecycle

### C2[41].15 — 20+ report templates

### C2[41].16 — report verification/expiry/conflict

### C2[41].17 — actionable brief/grouping

### C2[41].18 — surveillance/analysis

### C2[41].19 — interception/infiltration adapters

### C2[41].20 — extraction/recruitment

### C2[41].21 — rumor/counter-rumor requests

### C2[41].22 — counter-intelligence investigation

### C2[41].23 — dashboard/map/panels

### C2[41].24 — events/hooks/tutorial/localization

### C2[41].25 — content-utilization report

### Gate: 203B complete

---

## 203C — Integration / Validation

### C2[41].26 — faction stance / branch integration

### C2[41].27 — trade / radio / expedition integration

### C2[41].28 — combat intelligence planning

### C2[41].29 — Plan 131 rumor propagation integration

### C2[41].30 — Plan 168 propaganda integration

### C2[41].31 — Plan 153 covert-operation integration

### C2[41].32 — Plan 132 hidden-spy boundary

### C2[41].33 — save-load/idempotency matrix

### C2[41].34 — exploit prevention

### C2[41].35 — edge cases

### C2[41].36 — data-integrity / forbidden-ownership gates

### C2[41].37 — `--intelligence-network-selftest`

### C2[41].38 — deliberate failure proof

### C2[41].39 — 200-day intelligence soak

### C2[41].40 — information-overload / balance profiles

### C2[41].41 — accessibility/performance/retention

### C2[41].42 — playtest/docs/release closure

### Gate: 203C complete

---

# 335. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --intelligence-network-selftest
```

Also run repository-canonical equivalents of:

```text
port-contract validation
intelligence template/reference integrity
evidence-origin/corroboration test
Plan 131 rumor ownership audit
Plan 168 propaganda ownership audit
Plan 153 covert-operation ownership audit
Plan 157 interception ownership audit
old-save empty-network fixture
same-seed intelligence digest replay
200-day intelligence soak
large-network performance test
intelligence-panel accessibility/hidden-info test
```

---

# 336. `--intelligence-network-selftest` Acceptance Matrix

| Scenario | Expected |
|---|---|
| Field Agent | real survivor + expedition evidence |
| Placed Spy | canonical espionage adapter |
| Trade Contact | trade-derived evidence |
| Local Source | local coverage |
| Defector | high access, freshness decay |
| Payment | real resource transaction |
| Missed payment | source response |
| Report | stable ID + confidence |
| Verification | independent evidence |
| Same-origin rumor echo | no false corroboration |
| False report | disproven without world mutation |
| Expiration | stale/non-actionable |
| Surveillance | evidence generated |
| Interception | Plan 157 result consumed |
| Infiltration | adapter/cover result |
| Extraction | physical/canonical execution |
| Recruitment | deterministic source outcome |
| Analysis | confidence/contradiction |
| Compromise | persistent |
| Counter-intel | evidence-driven case |
| Rumor plant | Plan 131/168 request |
| Counter rumor | Plan 131/168 request |
| Faction effect | canonical stance API |
| Combat intel | preview/preparedness only |
| Save/load | exact state |
| Old save | empty network |
| No network | valid |
| Extensive network | bounded |
| Same seed | same digest |

---

# 337. Flagship Definition of Done — Foundation

- [ ] `IntelligenceNetworkSystem.cs`,
- [ ] evidence/report/source separation,
- [ ] 5 informant archetypes,
- [ ] 10+ informant templates,
- [ ] 7+ report types,
- [ ] 20+ report templates,
- [ ] source reliability,
- [ ] report confidence,
- [ ] verification,
- [ ] freshness,
- [ ] actionability,
- [ ] provenance/origin chain,
- [ ] operation model,
- [ ] counter-intelligence cases,
- [ ] deterministic RNG,
- [ ] Plan 131/153/157/168 boundaries,
- [ ] save/old-save,
- [ ] events/ports/diagnostics.

---

# 338. Flagship Definition of Done — Implementation / UI

- [ ] source recruitment,
- [ ] payment/contact,
- [ ] cover lifecycle,
- [ ] report generation,
- [ ] contradiction/supersession,
- [ ] surveillance,
- [ ] interception,
- [ ] infiltration,
- [ ] extraction,
- [ ] recruitment,
- [ ] analysis,
- [ ] rumor planting adapter,
- [ ] counter-rumor adapter,
- [ ] enemy rumor detection,
- [ ] counter-intel investigation,
- [ ] intelligence dashboard,
- [ ] informant detail,
- [ ] report detail,
- [ ] rumor panel,
- [ ] operations panel,
- [ ] counter-intel panel,
- [ ] intelligence map,
- [ ] events/hooks,
- [ ] tutorial/tooltips,
- [ ] localization,
- [ ] utilization report.

---

# 339. Flagship Definition of Done — Integration / Validation

- [ ] FactionStanceEngine,
- [ ] FactionBranchCoordinator,
- [ ] HoldfastTradeSession,
- [ ] SignalTriangulationSystem,
- [ ] Plan 157 interception,
- [ ] ExpeditionSystem,
- [ ] combat planning,
- [ ] Plan 131 propagation,
- [ ] Plan 168 propaganda,
- [ ] Plan 153 espionage,
- [ ] Plan 132 hidden-spy boundary,
- [ ] save/load lifecycle,
- [ ] report/verification/operation idempotency,
- [ ] anti-reroll,
- [ ] anti-echo corroboration,
- [ ] anti-payment/recruitment/report farming,
- [ ] no/extensive network edges,
- [ ] compromised-source edges,
- [ ] faction/location/source invalidation,
- [ ] deterministic replay,
- [ ] data integrity,
- [ ] forbidden ownership gates,
- [ ] deliberate failure fixtures,
- [ ] selftest,
- [ ] 200-day soak,
- [ ] report-volume budget,
- [ ] informant maintenance budget,
- [ ] source-type balance,
- [ ] verification/false-report balance,
- [ ] accessibility,
- [ ] performance,
- [ ] retention,
- [ ] archive,
- [ ] human playtest,
- [ ] docs.

---

# 340. Global Definition of Done

- [ ] no second rumor propagation system,
- [ ] no second propaganda effect resolver,
- [ ] no second sabotage/assassination executor,
- [ ] no second radio interception system,
- [ ] no faction truth copied into intelligence state,
- [ ] no report truth equated with confidence,
- [ ] no duplicate source recruitment,
- [ ] no save/load operation reroll,
- [ ] no rumor-echo false corroboration,
- [ ] no hidden spy leakage,
- [ ] no intelligence flood,
- [ ] no perfect-knowledge endgame,
- [ ] full verification green.

---

# 341. Closure Report Template

```markdown
## C2[41] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Ownership Baseline
- Plan 131 information propagation:
- Plan 153 espionage:
- Plan 157 interception:
- Plan 168 propaganda:
- Faction stance:
- Faction branch:
- Expedition:
- Trade:
- Combat:

### 203A — Foundation
- IntelligenceNetworkSystem:
- Informant archetypes:
- Informant templates:
- Report types:
- Report templates:
- Reliability model:
- Confidence model:
- Verification:
- Freshness:
- Provenance:
- Operations:
- Counter-intel:
- Save schema:
- Old-save behavior:
- Missing ports:
- Result:

### 203B — Runtime / UI
- Informants recruited:
- Field agents:
- Placed spies:
- Trade contacts:
- Local sources:
- Defectors:
- Reports received:
- Reports verified:
- Reports disproved:
- Reports expired:
- Operations launched:
- Operations compromised:
- Rumor requests:
- Counter-rumor requests:
- Counter cases:
- UI:
- Unused templates:
- Result:

### 203C — Integration
- Faction stance:
- Faction branch:
- Trade:
- Radio:
- Expedition:
- Combat:
- Plan 131:
- Plan 168:
- Plan 153:
- Plan 132:
- Save-load rerolls:
- Evidence duplication:
- Rumor echo exploit:
- Source duplication:
- Result:

### Balance / Soak
- 200-day reports:
- actionable reports:
- notifications/day:
- verified rate:
- false rate:
- source maintenance cost:
- compromise rate:
- operation success:
- counter-intel cases:
- dominant source type:
- dominant operation:
- information overload findings:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Intelligence selftest:
- Port contract:
- Template integrity:
- Plan 131 ownership audit:
- Plan 153 ownership audit:
- Plan 157 ownership audit:
- Plan 168 ownership audit:
- Old-save fixture:
- Same-seed replay:
- Large-network performance:
- Accessibility:
- Result:

### Final Metrics
- INTEL_INFORMANTS_ACTIVE:
- INTEL_INFORMANTS_COMPROMISED:
- INTEL_REPORTS_TOTAL:
- INTEL_REPORTS_ACTIONABLE:
- INTEL_REPORTS_VERIFIED:
- INTEL_REPORTS_FALSE:
- INTEL_REPORTS_EXPIRED:
- INTEL_OPERATIONS_TOTAL:
- INTEL_OPERATIONS_COMPROMISED:
- COUNTER_INTEL_CASES:
- RUMOR_OPERATION_REQUESTS:
- EVIDENCE_ORIGIN_DUPLICATION_ERRORS:
- OPERATION_REROLL_VIOLATIONS:
- UNAUTHORIZED_PROPAGATION_LOGIC:
- UNAUTHORIZED_PROPAGANDA_LOGIC:
- UNAUTHORIZED_COVERT_EXECUTION_LOGIC:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Informant content:
- Report content:
- Counter-intelligence depth:
- Intelligence trading:
- Analyst specialization:
- UI:
```

---

# 342. Final Execution Directive

Execute Plan 203 as an **intelligence evidence, source-management, verification, and operation-orchestration layer over the existing information, espionage, radio, faction, expedition, trade, and combat systems**.

The critical sequence is:

```text
establish evidence/source/report truth boundaries
→ recruit real informants with access, motive, cost, and cover
→ create reports from provenance-bearing evidence
→ track reliability, confidence, freshness, and verification separately
→ use deterministic operations to gather/verify evidence
→ consume Plan 157 interception instead of duplicating it
→ request Plan 131 rumor propagation instead of simulating spread locally
→ consume Plan 168 propaganda effects instead of recomputing them
→ delegate sabotage/assassination/covert execution to Plan 153/combat
→ feed verified intelligence into faction/trade/expedition/combat planning
→ detect hostile intelligence through evidence-driven counter-intel cases
→ prove save/load idempotency and long-run information-volume bounds
```

Do not turn intelligence reports into world truth.

Do not count repeated rumor echoes as independent verification.

Do not create a second rumor graph.

Do not create a second propaganda resolver.

Do not create a second assassination or sabotage system.

Do not make interception calculations locally.

The strongest authority rule is:

> **Plan 203 owns what the shelter believes it knows, why it believes it, how well-supported that belief is, and which intelligence operations produced the evidence; the world, factions, radio network, rumors, propaganda, covert attacks, combat, and trade remain authoritative in their existing systems.**

The strongest uncertainty rule is:

> **Reliability, confidence, verification, and truth are different things. A trusted source can be wrong, a low-trust source can be right, repeated rumor echoes can still trace to one origin, and only independent evidence should increase verification.**

The strongest integration rule is:

> **Intelligence creates better decisions by improving evidence and preparedness; it does not directly rewrite the systems it observes.**

The flagship acceptance scenario is:

> **Recruit a paid trade contact, a local source, and a survivor field agent. Receive an unverified report that a hostile faction is staging near a route. A repeated public rumor reaches three settlements through Plan 131 but must remain one provenance lineage and therefore not count as three independent confirmations. Send the field agent on a real ExpeditionSystem reconnaissance objective and consume a Plan 157 intercepted transmission as a second independent evidence source. The report becomes verified, updates a faction-branch planning read model, and improves the combat/expedition preparedness preview without changing the faction's actual hidden state. Plant a counter-rumor using the verified evidence: Plan 131 handles spread and Plan 168 handles propaganda effect. Then allow a placed-spy source to become compromised, open a counter-intelligence case from real evidence, and extract the source through the canonical covert/expedition path. Save/load before operation resolution and before rumor insertion; repeated runs must preserve the same report IDs, verification state, operation outcome, compromise state, propagation reference, and intelligence-history digest, with zero duplicate rumor, propaganda, interception, sabotage, or faction-state authorities.**
