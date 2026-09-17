---
PLAN_ID: E1-25
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 25
STATUS: READY_FOR_EXECUTION_WHEN_VISITOR_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 214 — Visitor Integration & Housing System"
SEQUENCE_FILENAME: "E1_planintegration[25].md"
PREVIOUS_FILENAME: "E1_planintegration[24].md"
NEXT_FILENAMES:
  - "E1_planintegration[26].md"
  - "E1_planintegration[27].md"
CATEGORY: LINK+VISITORS+HOUSING+ADMISSION+RECRUITMENT+SECURITY
PRIMARY_INTENT: "Create a temporary-occupant orchestration layer for admitted visitors, temporary housing, service eligibility, administrative processing, departure, and recruitment handoff without duplicating SurvivorCatalog, Recruitment, Needs, Security, Inventory, Diplomacy, Trade, or shelter topology."
PREMISE_VERIFICATION_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
AUTHORITATIVE_INTEGRATION_PROGRESS_SCORE_FORBIDDEN: true
SECOND_SURVIVOR_REGISTRY_FORBIDDEN: true
SECOND_RECRUITMENT_PIPELINE_FORBIDDEN: true
SECOND_SECURITY_SUSPICION_SYSTEM_FORBIDDEN: true
SECOND_HOUSING_TOPOLOGY_FORBIDDEN: true
SECOND_INVENTORY_LEDGER_FORBIDDEN: true
RNG_FOR_ROUTINE_VISITOR_PROCESSING_FORBIDDEN: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
MICROMANAGEMENT_RISK: VERY_HIGH
---

# E1 Plan Integration [25] — Visitor Admission, Temporary Residency, Housing, Service Access, Departure, and Recruitment Handoff

> **Sequence rule:** this file is `E1_planintegration[25].md`. The next file is `E1_planintegration[26].md`.

## 0. Mission

Plan 214 identifies the missing middle between Airlock admission and permanent SurvivorCatalog membership.
`AirlockSecuritySystem` can decide whether somebody enters, while recruitment systems can create a permanent
resident. The absent layer is temporary shelter occupancy: an admitted person needs a stable identity,
housing, resource access, medical/security processing, mission-specific restrictions, a planned or emergent
departure, and—only where appropriate—a clean handoff into recruitment.

E1-25 therefore defines a **Visitor Stay** rather than a second survivor aggregate.

The central rule is:

**Airlock decides entry; E1-25 manages the temporary stay; canonical shelter services support the visitor;
Security/Medical/Trade/Diplomacy own their own concerns; Recruitment alone turns a visitor into a survivor.**

The visitor layer may own:

- temporary stay identity and provenance;
- visitor purpose;
- administrative lifecycle;
- processing requirements and evidence references;
- temporary housing assignment references;
- service-entitlement policy;
- visitor access-policy reference;
- planned departure and departure receipts;
- recruitment-candidate/handoff references;
- bounded historical stay records.

It may not own:

- permanent survivor skills/health/needs/relationships;
- shelter-room condition or bed capacity;
- inventory item state;
- suspicion/threat findings;
- market cargo/prices;
- diplomatic mission state;
- faction standing;
- permanent survivor registration.

## 1. Source Intent Preserved

The source requests refugees, traders, defectors, guests, envoys, deserters/exiles; processing; housing;
integration; resource consumption; monitoring; departure; recruitment; UI; persistence; events; quests; and
Airlock/Survivor/Schedule/Needs/Security/Communication integration.

E1-25 preserves that intent but replaces duplicated data with canonical references and typed handoffs.

## 2. Core Architecture

```text
AirlockSecurity admission
        |
        v
VisitorAdmissionCommitted
        |
        v
VisitorStayRegistry
        |
        +--> purpose / provenance
        +--> processing requirement refs
        +--> temporary housing assignment ref
        +--> service entitlement
        +--> access/restriction policy ref
        +--> departure/recruitment refs
        |
        +------------------------------+
        |                              |
        v                              v
Shelter services                  Specialist authorities
Needs/Rationing                   Medical
E1-9 housing                      ShelterSecurity
Inventory                         E1-19 Trade/Market
E1-24 communication               E1-21 Diplomacy
Duty/Schedule                     Recruitment
        |                              |
        +---------------+--------------+
                        v
                 stay outcome
             depart / recruit / close
```

## 3. Architectural Corrections

### 3.1 Do not create a second survivor aggregate

The proposed `Visitor` DTO is too survivor-like. It should not grow copied health, skills, needs, inventory,
relationships, suspicion, or permanent identity fields.

### 3.2 Replace authoritative `integrationProgress`

A 0–100 integration score should not decide whether someone becomes a resident. Use explicit requirements:

- housing stable;
- medical clearance satisfied;
- security clearance satisfied;
- orientation/registration satisfied;
- stay restrictions resolved;
- recruitment owner says eligible;
- visitor consent/request known.

A UI can derive an administrative-readiness band.

### 3.3 Housing condition is not visitor state

Room/bunk capacity belongs to E1-9/topology; facility/room condition belongs to E1-17 and shelter-environment
authorities.

### 3.4 Resource consumption must be canonical

Visitors should register as temporary resource consumers or use an existing generic consumer abstraction.
E1-25 does not decrement food/water itself.

### 3.5 Monitoring remains Security-owned

Visitor administration can reference a Security clearance, investigation, escort requirement, or access
restriction. It must not maintain its own suspicion/escalation meter.

### 3.6 Purpose and lifecycle are distinct

`Refugee`, `Trader`, `Envoy`, `Guest`, `Defector` describe why the person is present. `Processing`,
`TemporarilyHoused`, `AwaitingDecision`, `PreparingDeparture`, `Closed` describe stay lifecycle.

### 3.7 Recruitment is a conversion transaction

The visitor layer requests recruitment; Recruitment/SurvivorCatalog create the permanent resident. The
temporary stay then closes atomically.

### 3.8 Departure items are Inventory transactions

Parting gifts, personal property, issued gear, and recovered items are canonical inventory operations. A
departure record may cite transaction IDs but may not own copied item lists.

### 3.9 Escape and forced removal are not visitor RNG

Escape is a ShelterSecurity incident. Forced removal is a Governance/Security action. Routine stay processing
uses no RNG.

### 3.10 Trader and envoy stays are wrappers around existing specialist systems

E1-19 owns trade; E1-21 owns diplomacy. E1-25 adds temporary shelter hosting only.

## 4. Non-Negotiable Rules

- AirlockSecurity remains admission owner.
- SurvivorCatalog remains permanent-survivor owner.
- Recruitment remains resident-conversion owner.
- Needs/Rationing owns food/water consumption.
- Medical owns health, diagnosis, treatment, contagiousness, and clearance.
- ShelterSecurity owns suspicion, monitoring findings, investigation, escort enforcement, detention, and escape.
- E1-9 owns room/bed/bunk topology and capacity.
- E1-17 owns maintainable housing/facility condition.
- Inventory owns visitor possessions, issued gear, gifts, and transfers.
- E1-19/Market owns trader cargo and commerce.
- E1-21 owns envoy/diplomatic mission state.
- E1-24 owns visitor-related notices/messages.
- QuestSystem owns quests and rewards.
- VisitorStay owns temporary lifecycle and references only.
- No visitor-local health/needs/skills/relationships.
- No visitor-local room condition.
- No visitor-local suspicion meter.
- No visitor-local item list as authority.
- No authoritative integration percentage.
- No automatic recruitment after elapsed days.
- No automatic hostility from visitor category.
- No RNG for housing, processing, departure scheduling, or recruitment handoff.
- No double resource consumption after recruitment.
- No visitor remains active after successful permanent conversion.
- Old saves receive no fabricated guests or housing.
- First release proves Refugee/Guest + Trader/Envoy + departure/recruitment before broad template expansion.

## 5. Acceptance Slices

### Slice A — Temporary identity
Admitted visitor opens one stable stay exactly once.

### Slice B — Housing and services
Visitor occupies a canonical housing slot and consumes canonical shelter resources.

### Slice C — Processing
Medical/Security/orientation requirements complete through owner-system evidence.

### Slice D — Departure and recruitment
One visitor leaves; another converts through Recruitment/SurvivorCatalog.

### Slice E — Specialized visitor paths
Trader, envoy, defector, mass arrivals, and richer social behavior follow only after A–D pass.

---

## E1-25A — Premise verification and authority audit

1. Read `AirlockSecuritySystem.cs` end-to-end; document admission outcomes, visitor payload, quarantine/inspection state, emitted events, and save ownership.
2. Inspect SurvivorCatalog, Recruitment/Plan 204, E1-9 occupancy, Needs/Rationing, Medical, ShelterSecurity, E1-19, E1-21, Inventory, E1-24, Duty/Schedule, QuestSystem, and save registry.
3. Search for guest/refugee/envoy/trader/asylum/visitor/temporary resident/housing/departure/escort/detention code.
4. Determine whether non-survivor resource consumers, health entities, possessions, and room occupancy already exist.
5. Create `docs/systems/VISITOR_STAY_AUTHORITY_MAP.md` and set premise verification to current HEAD.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25B — Visitor-stay ownership ADR

1. Compare `VisitorIntegrationSystem`, `VisitorStayRegistry` + adapters, and Airlock extension.
2. Define stay-owned facts: stay ID, source identity ref, purpose, lifecycle, requirements, housing ref, service/access policy, departure/recruitment refs, history.
3. Explicitly exclude copied survivor, room, inventory, security, market, and diplomacy state.
4. Define transition ownership and rollback flags.
5. Require architecture review before implementation.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25C — Stable visitor identity

1. Use an existing encounter/faction/person ID where possible.
2. Create one stay ID per visit, separate from person identity.
3. Store source provenance, display-name snapshot/reference, origin/faction ref where known, purpose, admission event ID, and current site.
4. Define anonymous and repeat-visitor behavior.
5. Add uniqueness and duplicate-admission tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25D — Visitor purpose catalog

1. Start with purpose policies such as Refugee, Trader, Guest, Envoy, Defector/AsylumSeeker only where distinct behavior exists.
2. Purpose defines default requirements, entitlements, access, recruitment eligibility, and expected-stay policy.
3. Do not encode health or threat judgment in purpose.
4. Merge categories with identical behavior.
5. Validate all referenced owner policies.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25E — Stay lifecycle state machine

1. Define explicit states such as Processing, TemporarilyHoused, ActiveGuest, AwaitingDecision, PreparingDeparture, RecruitmentHandoff, Closed.
2. Represent quarantine/detention as owner-system restrictions rather than duplicate lifecycle truth when possible.
3. Do not retain `Recruited` as a live visitor after conversion.
4. Define transition owner and terminal state rules.
5. Add invalid-transition tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25F — Airlock admission handoff

1. Consume committed Airlock admission event ID exactly once.
2. Require admitted/conditional-admission result.
3. Open visitor stay without rerunning inspection or recruitment.
4. Reference current quarantine/restriction policy.
5. Emit `VisitorStayOpened` after commit.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25G — Conditional admission and quarantine

1. Medical/Airlock owns quarantine and clearance.
2. E1-25 limits access/housing according to the current restriction.
3. Processing requirement waits for canonical clearance receipt.
4. Do not locally decide quarantine duration or completion.
5. Test admitted+quarantine, cleared, and rejected-after-assessment paths.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25H — Processing requirement schema

1. Represent MedicalClearance, SecurityClearance, Housing, Orientation, MissionRegistration, TradeRegistration, AccessCredential, RecruitmentInterview as typed requirements.
2. Each requirement names its owning subsystem/action and stores evidence ref.
3. Use Pending/InProgress/Satisfied/Failed/Waived.
4. Do not store generic progress percentages for discrete requirements.
5. Validate every owner/action reference.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25I — Derived readiness read model

1. Derive readiness from mandatory requirements, housing validity, restrictions, recruitment eligibility, and visitor intent.
2. Use labels such as Processing, Restricted, StableGuest, Candidate, ReadyForDecision.
3. If UI requires percentage, derive it from checklist weights and label it administrative readiness.
4. Do not persist or use readiness as Recruitment truth.
5. Add consistency tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25J — Orientation

1. Treat orientation as a light administrative action, not social assimilation.
2. Use E1-24 briefing/notice where suitable.
3. Use Duty/Schedule only if staff time materially matters.
4. Record acknowledgement, not obedience.
5. Do not directly change morale, trust, or relationship values.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25K — Medical clearance

1. Create a Medical assessment request/reference.
2. Medical owns findings, contagiousness, treatment, privacy, and clearance.
3. VisitorStay stores requirement status/evidence only.
4. Do not duplicate diagnoses or medicine consumption.
5. Test cleared, treatment-needed, quarantined, and refused cases.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25L — Security clearance

1. Create a ShelterSecurity screening/investigation reference.
2. Security owns findings, threat, escort, access restriction, detention, and escalation.
3. VisitorStay stores status/evidence only.
4. Do not create a monitoring level or suspicion score.
5. Test cleared, restricted, investigated, and unresolved cases.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25M — Trader stay path

1. Keep Market/E1-19 as owner of cargo, prices, trade session, agreements, and settlement.
2. E1-25 owns temporary housing/access/services only.
3. Reference active market/trade session.
4. Do not make trader labor/recruitment default behavior.
5. Close stay only after commercial/departure constraints resolve.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25N — Diplomatic envoy stay path

1. Keep E1-21 as owner of mission, treaty proposal, negotiation, and diplomatic obligations.
2. E1-25 owns hosting/housing/access/services.
3. Reference envoy mission and intended meeting/departure.
4. Do not duplicate mission timers.
5. Apply Security/privacy rules to access and communication.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25O — Refugee/asylum stay path

1. Allow extended temporary stay without automatic permanent conversion.
2. Use canonical Medical/Security processing and Rationing services.
3. Recruitment is an optional separate decision.
4. Do not treat low resources as automatic deportation.
5. Test temporary stay, candidate, declined recruitment, and voluntary departure.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25P — Defector/deserter boundary

1. Faction systems own origin/defection consequences; ShelterSecurity owns actual risk.
2. Do not hard-code defector/deserter as suspicious.
3. Use explicit intelligence/security evidence for restrictions.
4. Recruitment remains separate.
5. Feature-gate if faction content is absent.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25Q — Guest stay path

1. Use invitation/hosting provenance.
2. Apply lighter processing than refugee/defector where policy permits.
3. Assign temporary accommodation and service entitlement.
4. Respect planned departure/mission.
5. Do not force recruitment eligibility.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25R — Housing assignment transaction

1. Reference canonical site/room/bed/bunk capacity.
2. Validate capacity, access, quarantine, and site restrictions.
3. Reserve destination occupancy and commit exactly once.
4. Store assignment ID + room/slot ref + dates + housing policy.
5. Do not store room condition.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25S — Housing policy types

1. Model TemporaryBunk, SharedQuarter, PrivateRoom, GuestSuite as allocation policies over canonical rooms/slots.
2. Map privacy/amenity requirements to real room capabilities.
3. Do not invent independent capacity values.
4. Start with 2–3 policies.
5. Add policy-to-room compatibility tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25T — Housing quality read model

1. Derive quality from crowding, privacy, thermal/environmental service, sanitation, safety, bed type, and real room condition where available.
2. Do not persist quality as visitor state.
3. Do not directly alter diplomacy or morale unless owner policies explicitly consume factors.
4. Expose contributing reasons.
5. Add derived-state tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25U — Housing reassignment

1. Validate new slot, reserve destination, then release old occupancy on commit.
2. Respect quarantine/security constraints.
3. Use stable operation ID.
4. Handle room removal/deactivation.
5. Test save/load during reassignment.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25V — E1-9 shelter expansion integration

1. New rooms expose real hosting capacity only when installed/activated.
2. VisitorStay revalidates housing on room removal or topology change.
3. Do not auto-create guest housing in every new room.
4. Support site ID for future outposts.
5. Add expansion/renovation tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25W — E1-17 condition boundary

1. Beds, guest facilities, locks, sanitation, and other maintainable assets query E1-17.
2. Broken accommodation may require reassignment.
3. Repairs remain E1-17-owned.
4. Do not store condition or maintenance dates in visitor state.
5. Keep maintenance burden low.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25X — Service entitlement policy

1. Define eligibility for food, water, heat, hygiene, medical care, communication, market access, and other shelter services.
2. Entitlement is policy, not a resource ledger.
3. Use purpose, admission terms, treaty/guest agreement, and governance policy.
4. Canonical service systems perform allocation.
5. Add policy conflict tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25Y — Temporary food/water consumer

1. Prefer an existing generic non-survivor consumer contract.
2. If absent, add one shared resource-consumer abstraction rather than visitor-local daily rates.
3. Register one consumer per stay when entitled.
4. Needs/Rationing owns quantities and shortages.
5. Remove/convert registration atomically on departure/recruitment.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25Z — Medical resource use

1. Treatment uses Medical/Inventory transactions.
2. VisitorStay references treatment outcome only.
3. Do not store daily medicine consumption.
4. Entitlement controls access, not result.
5. Test no-supply/refusal/treatment paths.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AA — Temporary work boundary

1. Use Duty only if non-survivor workers are genuinely supported.
2. Trader/envoy should not become generic labor.
3. Refugee/guest work should be opt-in/policy-based.
4. Production/Duty owns output and compensation.
5. Do not increase integration readiness per work hour.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AB — Schedule integration

1. Use ShelterSchedule for appointments such as orientation, medical check, interview, meeting, or planned departure.
2. Do not use schedule room assignments as housing truth.
3. Use canonical due events.
4. Handle reschedule/time skip deterministically.
5. Keep simple stays low-bureaucracy.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AC — Access control

1. Store a reference to an allowed/restricted-area policy.
2. ShelterSecurity/topology enforces it.
3. Purpose/clearance may select policy.
4. Do not maintain a second per-room security map.
5. Add restricted-area violation tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AD — Escort requirement

1. Security declares whether escort is required.
2. Duty assigns eligible escort staff.
3. VisitorStay references escort operation/status.
4. Do not teleport visitor or auto-monitor locally.
5. Handle no-escort availability.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AE — Monitoring boundary

1. Replace proposed VisitorMonitoring state with ShelterSecurity references.
2. Security owns observation level, findings, investigation, and escalation if such concepts exist.
3. VisitorStay may display clearance/restriction summary only.
4. Do not accumulate suspicion over time.
5. Test cleared and active-investigation states.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AF — Suspicious activity handoff

1. Use explicit canonical incidents such as restricted-zone breach, theft, false credentials, sabotage, violence, or hostile communication.
2. ShelterSecurity investigates and resolves.
3. VisitorStay updates restrictions only from owner result.
4. Do not infer malicious intent from origin category.
5. Use stable incident IDs.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AG — Detention/forced custody boundary

1. Use Governance/ShelterSecurity if detention exists.
2. VisitorStay references custody case and adjusted housing/access.
3. Do not store a parallel detained truth.
4. Departure/recruitment respects custody outcome.
5. Feature-gate if detention is absent.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AH — Visitor possessions

1. Use Inventory or source person entity for possessions.
2. Search/inspection produces Inventory/Security evidence.
3. VisitorStay stores container/ref only.
4. Preserve item identity and metadata.
5. Test admission, departure, and recruitment possession continuity.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AI — Shelter-issued items

1. Use Inventory loan/issue transactions.
2. Examples may include visitor badge, bedding, clothing, respirator only if actual items exist.
3. Return on departure unless explicitly gifted.
4. Recruitment converts ownership according to Inventory policy.
5. Prevent duplicate item creation.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AJ — Parting gifts

1. Validate real shelter-owned item and transfer through Inventory.
2. Store transaction ref in departure history.
3. Faction/Relations/Diplomacy owns any consequence.
4. Do not create a local `good_standing` reward.
5. Test retry/save-load idempotency.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AK — Voluntary departure

1. Validate no quarantine/detention/mission restriction blocks exit.
2. Reconcile possessions and issued items.
3. Release housing and temporary resource consumer.
4. Close stay only after canonical exit/departure commits.
5. Preserve external person identity/history.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AL — Invited departure

1. Leadership/Governance owns request to leave.
2. Visitor may comply, negotiate extension, or refuse through canonical social/security policy.
3. VisitorStay stores request/ref and pending state.
4. Do not immediately mark Departed.
5. Use E1-24 communication where useful.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AM — Forced removal

1. Require canonical Governance/Security order.
2. Security owns escort/use-of-force outcome.
3. Inventory owns possessions.
4. Faction/Diplomacy owns political consequences.
5. VisitorStay closes only after successful removal.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AN — Escape

1. Escape attempt/result belongs to ShelterSecurity.
2. VisitorStay consumes confirmed absence/departure.
3. Do not use a daily escape chance.
4. Keep possessions where the incident says they are.
5. Test caught and successful escape.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AO — Planned departure scheduler

1. Store planned departure only where the stay contract has one.
2. Use due queue rather than daily scans.
3. Due date creates a departure opportunity, not teleportation.
4. Revalidate trade/diplomacy/security constraints.
5. Support extensions.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AP — Departure receipt

1. Create immutable record with stay ID, departure type, day, source operation, destination ref if known, housing/resource closure refs, Inventory transaction refs, and recruitment ref if applicable.
2. Do not own copied item lists.
3. Do not own generic final standing.
4. Archive compactly.
5. Add restore tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AQ — Recruitment eligibility handoff

1. Recruitment evaluates candidate eligibility using its own rules.
2. E1-25 exposes administrative readiness and source refs.
3. Do not approve recruitment locally.
4. Do not make every guest/trader a candidate.
5. Use stable candidate/request ID.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AR — Recruitment conversion transaction

1. Call Recruitment/SurvivorCatalog once.
2. Create permanent survivor through normal bootstrap.
3. Transfer possessions through Inventory.
4. Atomically replace temporary resource-consumer identity.
5. Release/convert housing according to permanent housing owner.
6. Close visitor stay with survivor ID ref.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AS — Identity continuity after recruitment

1. Link stay/source visitor ID to permanent survivor ID.
2. Preserve approved background/faction provenance.
3. Do not copy temporary suspicion into permanent traits.
4. Keep visit history read-only.
5. Assert exactly one active person identity.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AT — Visitor death boundary

1. Audit whether Health can own non-survivor person deaths.
2. Close stay only after canonical death outcome.
3. Inventory resolves possessions.
4. E1-23/Memorial integration occurs only if their contracts support visitor identities.
5. Do not create fake SurvivorFate records.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AU — Illness during stay

1. Medical/Disease owns illness, treatment, contagiousness, and quarantine.
2. E1-25 adjusts housing/access requirement refs.
3. Do not make illness an integration penalty.
4. Continue canonical resource service.
5. Respect health privacy.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AV — Resource shortages

1. Governance/Rationing decides visitor service priority.
2. Needs/Rationing determines actual allocation.
3. E1-25 does not eject people automatically for scarcity.
4. Show shortages and policy source.
5. Test changing priorities.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AW — Consent and agency

1. Recruitment, optional work, medical procedures, stay extension, and departure requests should respect existing consent/autonomy policies.
2. E1-25 stores request/outcome refs, not a compliance score.
3. Do not add local random compliance chance.
4. Use authored intent if generic autonomy is unavailable.
5. Keep routine administration deterministic.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AX — Relationship boundary

1. Use Relations only if it supports non-survivor identities.
2. If unsupported, keep visitor relationships narrative-only until a generic person relationship contract exists.
3. Do not store visitor affinity locally.
4. Recruitment may import canonical history only through explicit adapter.
5. Do not invent friend/hostile final status.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AY — E1-24 communication integration

1. Publish arrival/orientation/meeting/housing/departure notices as message projections.
2. Keep Medical/Security details restricted.
3. Recruitment notice follows canonical conversion.
4. Messages do not mutate visitor state.
5. Use stay/source event IDs for dedupe.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25AZ — Faction/diplomacy consequence boundary

1. E1-21/Faction consumes hosting, removal, harm, or defection events where relevant.
2. E1-25 does not change standing.
3. Good hosting can become evidence only if diplomacy policy consumes it.
4. Do not store canonical final diplomatic status.
5. Use provenance.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BA — Trader departure settlement

1. Ensure Market/E1-19 session and cargo transactions are resolved before final stay closure where required.
2. Do not copy trader cargo.
3. Release housing/access after settlement/departure.
4. Reference market session result in departure record.
5. Test interrupted trade.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BB — Temporary contribution accounting

1. If visitor work exists, Duty/Production owns output.
2. Economy/Inventory owns compensation.
3. Rationing owns service cost.
4. E1-25 records authorization/evidence only.
5. Prevent free-labor or double-reward loops.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BC — Visitor processing orchestration

1. Do not build a second task executor.
2. Medical requirement calls Medical; Security calls Security; housing calls occupancy service; work calls Duty; interviews call Recruitment.
3. E1-25 tracks administrative requirement state/evidence only.
4. Validate every requirement owner.
5. Fail integrity checks on orphan task types.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BD — Language/cultural orientation gate

1. Audit whether language/culture systems exist.
2. If absent, keep this narrative-only.
3. Do not create an assimilation meter.
4. Do not penalize origin categories culturally by default.
5. Feature-gate until a real consumer exists.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BE — Hosting-conditions read model

1. Derive status from housing, ration delivery, service availability, processing delay, and unmet entitlements.
2. Use explainable factors.
3. Do not persist a universal hosting score.
4. Do not feed it directly to Recruitment/Diplomacy unless explicit policy names factors.
5. Add consistency tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BF — Visitor overview UI

1. Show active visitors, purpose, lifecycle, housing, major requirements, restrictions, and planned departure.
2. Show recruitment candidate state from Recruitment.
3. Link to Medical/Security/Trade/Diplomacy rather than copy detail.
4. Respect privacy.
5. Add empty/full snapshots.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BG — Visitor detail UI

1. Show admission provenance, purpose/origin, processing checklist, housing/services, access policy, departure/recruitment refs.
2. Do not display local suspicion or fake integration percentage as truth.
3. Show owner blockers and links.
4. Use neutral language.
5. Add representative visitor snapshots.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BH — Housing UI

1. Show canonical room/bunk capacity and visitor assignments.
2. Show environmental/service issues from owners.
3. Allow authoritative reassignment.
4. Do not duplicate room condition.
5. Highlight overcapacity and restrictions.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BI — Processing checklist UI

1. Group requirements by Medical, Security, Housing, Orientation, Trade/Diplomacy, Recruitment.
2. Show owner and evidence status.
3. Do not invent local progress for external actions.
4. Link to actionable owner panel.
5. Keep low-value tasks out of the checklist.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BJ — Departure UI

1. Show departure mode, blockers, planned date, possession/issued-item refs, and canonical operation source.
2. Do not directly force-remove from a visitor-only command.
3. Confirm irreversible actions.
4. Use neutral wording.
5. Test cancellation and race conditions.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BK — Security UI boundary

1. Show only clearance/restriction summary in visitor detail.
2. Link full findings to ShelterSecurity.
3. Respect classified information.
4. Do not show 0–100 suspicion.
5. Do not equate visitor purpose with threat.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BL — Visitor lifecycle events

1. Emit events only after committed transitions: StayOpened, HousingAssigned, RequirementSatisfied, DepartureCommitted, VisitorRecruited.
2. ForcedRemoval/Escape cite canonical Security incidents.
3. Do not emit daily random integration events.
4. Use stable IDs.
5. Add replay/dedupe tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BM — Quest hook redesign

1. Prefer first successful hosting, resolving a capacity problem, completing an envoy stay, processing an asylum case, or recruiting one valid candidate.
2. Defer raw counters such as 30 processed/20 housed/15 integrated.
3. QuestSystem owns rewards.
4. Do not incentivize unnecessary surveillance/removal.
5. Use stay/outcome IDs.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BN — Tutorial

1. Start from the first real admitted visitor.
2. Explain temporary versus permanent identity, housing capacity, processing requirements, service cost, departure, and recruitment separation.
3. Do not present every visitor category at once.
4. Support no-recruitment/no-housing fallback.
5. Use localized copy.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BO — Old-save migration

1. Initialize visitor stay registry empty.
2. Preserve Airlock, SurvivorCatalog, housing, security, and inventory unchanged.
3. Do not backfill historical guests.
4. Do not create temporary resource consumers.
5. Version migration and add early/late fixtures.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BP — Existing active visitor migration gate

1. If repository audit finds actual live visitor objects, map them explicitly by stable ID.
2. Map current site, admission state, possessions, trade/diplomacy refs, and occupancy.
3. Do not invent completed requirements.
4. Do not double-register resource consumption.
5. Require a dedicated migration ADR.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BQ — Save contract and restore ordering

1. Persist stay records, purpose/lifecycle, requirement/evidence refs, housing refs, entitlement/access policy, planned departure/recruitment refs, and compact history.
2. Do not persist copied health/needs/skills, room condition, suspicion, inventory contents, market, or diplomacy state.
3. Restore source person/Airlock, topology, Inventory, Medical/Security, Trade/Diplomacy, Recruitment, and services before validating refs.
4. Rebuild derived readiness/hosting summaries.
5. Do not replay admission/resource registration.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BR — Resource-consumer idempotency

1. Use one stable consumer ID per stay.
2. Register once and unregister once.
3. Recruitment atomically converts/replaces it.
4. Do not register visitors whose policy excludes shelter rations.
5. Add double-registration/load tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BS — Time skip

1. Use CampaignCalendar.
2. Do not advance a generic integration percentage.
3. Process due departure/appointment events once.
4. Needs, Medical, Security, Trade, Diplomacy, and Recruitment advance through their own clocks.
5. Add stepped-versus-skip tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BT — Zero-RNG baseline

1. Housing, requirements, entitlement, departure scheduling, and recruitment handoff are deterministic.
2. Escape/investigation incidents belong to Security.
3. Visitor narrative variation may use keyed RNG in content systems.
4. Do not consume shared RNG from visitor tick.
5. Add same-input/call-order tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BU — Conservation and identity audit

1. Test bed occupancy, resource consumer registration, visitor possessions, issued items, gifts, recruitment, and departure together.
2. Assert one active housing slot per stay.
3. Assert one temporary or permanent resource identity—not both.
4. Assert item conservation.
5. Assert exactly one active person identity after recruitment.
6. Add save/load race tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BV — Fairness and dignity review

1. Visitor origin/category is not guilt.
2. Security restriction requires policy/evidence.
3. Illness is not an integration failure.
4. Forced removal uses operational language.
5. Privacy boundaries are explicit.
6. Add content-review sign-off.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BW — Bureaucracy/micromanagement audit

1. Measure manual actions per stay.
2. Auto-create standard requirement sets.
3. Use standing housing/service policies.
4. Remove routine clicks that do not create a decision.
5. Cap alerts and dashboard churn.
6. Set per-purpose complexity targets.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BX — Capacity and balance simulation

1. Simulate low/medium/high visitor counts.
2. Measure housing pressure, food/water load, Medical/Security staff time, processing latency, and departure/recruitment throughput.
3. Ensure one guest is affordable while mass arrivals create recoverable pressure.
4. Use actual campaign population/capacity.
5. Document accepted ranges.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BY — Mass-arrival gate

1. Do not ship group refugee waves before single-stay semantics scale.
2. Batch housing/service allocation.
3. Do not create dozens of modal tasks.
4. Preserve individual identity only where narrative/recruitment needs it.
5. Add batch performance tests.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25BZ — Repeat visitors

1. Reuse stable external person identity when available.
2. Create a new stay ID for each visit.
3. Preserve prior stay history as references.
4. Do not reuse old housing/resource registrations.
5. Test returning trader/envoy.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CA — Outpost/site hosting

1. Use E1-10 site identity if visitors can stay outside the main shelter.
2. Site capacity/services own housing and consumption.
3. Do not charge main-shelter resources automatically.
4. Recruitment transfer to shelter requires real travel.
5. Feature-gate until site hosting exists.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CB — Journal and E1-5 legacy boundary

1. Chronicle may record notable arrival, asylum, envoy, recruitment, forced removal, escape, or famous departure.
2. Do not log every processing step.
3. E1-5 may receive narrative visitor history only.
4. Do not grant future-campaign mechanical bonuses.
5. Use stable stay/outcome IDs.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CC — Exploit and edge-case audit

1. Test duplicate admission, duplicate housing, recruit+depart same tick, gift double-submit, stale resource consumer, trader cargo leakage, envoy recruitment during active mission, quarantine bypass, and repeated callback delivery.
2. Use stable operation IDs.
3. Add property assertions for occupancy, resources, inventory, and identity.
4. Test old-save enablement.
5. Block release on any duplication.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CD — Performance and long-campaign soak

1. Run maximum campaign with overlapping refugees, guests, traders, envoys, repeat visitors, housing changes, shortages, checks, departures, and recruitment.
2. Measure active/closed stays, requirement records, due events, resource registrations, occupancy queries, save size, allocations, and UI read-model cost.
3. Archive closed stays compactly.
4. Do not tick historical visitors.
5. Record median/p95 processing cost.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CE — Headless visitor-integration selftest

1. Admit visitor through Airlock fixture; open stay once; assign canonical housing; register resource consumer.
2. Complete Medical and Security requirements through owner fixtures.
3. Depart visitor and reconcile housing/resources/possessions.
4. Admit second visitor and convert through Recruitment/SurvivorCatalog.
5. Verify one permanent identity and closed visitor stay.
6. Save/reload on admission, housing, departure, and recruitment boundaries.
7. Expose `--visitor-integration-selftest`.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CF — Data-integrity selftest

1. Validate purpose/template IDs, admission mappings, requirement owners, housing policy/capability refs, service entitlements, Security/Medical/Recruitment/Trade/Diplomacy adapters, item rules, and localization.
2. Reject templates that embed copied suspicion/health/resource ledgers.
3. Reject orphan owner/action refs.
4. Fail with actionable diagnostics.
5. Wire into `--data-integrity-selftest`.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CG — Documentation and observability

1. Create `docs/systems/VISITOR_STAYS.md`, `VISITOR_PROCESSING_REQUIREMENTS.md`, `VISITOR_HOUSING.md`, `VISITOR_DEPARTURE.md`, and `VISITOR_RECRUITMENT_HANDOFF.md`.
2. Document Security/Medical/E1-19/E1-21/E1-24 boundaries and zero-RNG baseline.
3. Add debug readout for stay/source IDs, purpose, lifecycle, requirements, housing, entitlements, restrictions, departure/recruitment refs, and blockers.
4. Keep debug mutation dev-only.
5. Update plan register/intake.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

## E1-25CH — Release gate

1. Run .NET build/test and game build.
2. Run data-integrity and visitor-integration selftests.
3. Run Airlock idempotency, housing/resource/inventory conservation, Medical/Security boundary, Trader/Envoy, Recruitment conversion, E1-24 privacy, migration, time-skip, fairness, exploit, and performance tests.
4. Verify Refugee/Guest + Trader/Envoy + departure/recruitment vertical slices before broad templates or mass arrivals.
5. Mark DONE only when visitors can stay temporarily without becoming a second survivor/security/housing/resource system.

**Acceptance gate:** owner boundaries remain singular; transitions are deterministic/idempotent; relevant occupancy/resource/item/identity conservation is tested; save/load cannot replay the operation.

---

# 6. Canonical Authority Matrix

| Fact | Canonical owner | E1-25 role |
|---|---|---|
| Entry decision | AirlockSecurity | Consume |
| Temporary stay | E1-25 | Own |
| Permanent survivor | SurvivorCatalog | Recruitment handoff |
| Recruitment | Recruitment | Query/commit |
| Health/clearance | Medical/Health | Evidence ref |
| Threat/monitoring | ShelterSecurity | Evidence/restriction ref |
| Room/bed capacity | E1-9 | Housing ref |
| Facility condition | E1-17 | Query |
| Food/water demand | Needs/Rationing | Temporary consumer |
| Possessions/gifts | Inventory | Transaction refs |
| Trader commerce | E1-19/Market | Specialized path |
| Envoy mission | E1-21 | Specialized path |
| Internal notices | E1-24 | Presentation |
| Duty/work | DutyRoster | Optional action |
| Quest | QuestSystem | Hook only |
| Legacy | E1-5 | Narrative only |

# 7. Suggested Visitor Stay Record

```yaml
schema_version: 1
stay_id: "visitor_stay_0042"
visitor_identity_ref: "encounter_person_118"
purpose: "REFUGEE"
admission_event_id: "airlock_admission_0901"
arrival_day: 91
site_id: "main_shelter"
status: "TEMPORARILY_HOUSED"
housing_assignment_id: "visitor_housing_0042"
requirement_ids:
  - "visitor_req_0042_medical"
  - "visitor_req_0042_security"
service_entitlement_policy_id: "visitor_refugee_standard"
access_policy_id: "visitor_cleared_common_areas"
planned_departure_day: null
recruitment_candidate_ref: null
```

No health, suspicion, item list, room condition, or daily-consumption fields.

# 8. Processing Requirement

```yaml
requirement_id: "visitor_req_0042_medical"
stay_id: "visitor_stay_0042"
requirement_type: "MEDICAL_CLEARANCE"
owner_system: "Medical"
status: "SATISFIED"
evidence_ref: "medical_assessment_772"
```

# 9. Housing Assignment

```yaml
housing_assignment_id: "visitor_housing_0042"
stay_id: "visitor_stay_0042"
site_id: "main_shelter"
room_id: "room_guest_quarters"
occupancy_slot_id: "bunk_guest_03"
policy_id: "shared_quarter"
assigned_day: 91
expected_end_day: 100
```

E1-9 owns capacity. E1-17 owns condition.

# 10. Temporary Resource Consumer

```text
stay_id
-> temporary resource consumer registration
-> Needs/Rationing
-> food/water/service allocation
```

On recruitment:

```text
temporary consumer
-> atomic conversion
-> permanent survivor consumer
```

The two must never coexist.

# 11. Security Boundary

Correct:

```text
stay requests clearance
-> ShelterSecurity screens/investigates
-> canonical finding/restriction
-> E1-25 records evidence/ref
```

Incorrect:

```text
visitor.suspicion += 10/day
if suspicion > 80: visitor becomes hostile
```

# 12. Departure Pipeline

```text
departure intent/order
-> validate quarantine/detention/mission
-> reconcile possessions and issued items
-> release occupancy
-> remove temporary consumer
-> canonical exit commits
-> close stay
-> write immutable departure receipt
```

# 13. Recruitment Pipeline

```text
administrative requirements complete
-> Recruitment evaluates candidate
-> visitor consent/decision
-> SurvivorCatalog creates permanent survivor
-> Inventory continuity
-> resource-consumer conversion
-> housing handoff
-> visitor stay closes
```

Exactly one active person identity remains.

# 14. Specialized Visitor Paths

## Refugee / asylum seeker
Extended stay and possible recruitment; no automatic conversion.

## Trader
E1-19/Market owns commerce and cargo.

## Envoy
E1-21 owns mission and negotiations.

## Guest
Low-bureaucracy planned temporary stay.

## Defector / deserter
Faction consequences belong to faction/diplomacy; risk requires Security evidence.

# 15. Old-Save Migration

Default:

```text
visitor stays = empty
housing assignments = empty
requirements = empty
departure history = empty
temporary resource consumers = none
```

Preserve Airlock, permanent survivors, shelter housing, Security, Medical, Inventory, Trade, and Diplomacy.

# 16. Fairness Rules

- origin is not guilt;
- refugee status is not security evidence;
- illness is not integration failure;
- defection does not automatically equal threat;
- restrictions require a canonical policy/reason;
- forced removal is a Governance/Security act;
- recruitment remains separate from temporary hospitality.

# 17. Exploit Matrix

| Failure | Guard |
|---|---|
| Duplicate Airlock event | Admission event ID |
| Two housing slots | Occupancy transaction |
| Free food/water | Temporary consumer registration |
| Double consumption after recruitment | Atomic conversion |
| Trader cargo copied | Market/Inventory boundary |
| Gift duplicated | Inventory transaction ID |
| Recruit + depart race | Lifecycle lock |
| Escape rerolled | Security-owned incident |
| Quarantine cleared locally | Medical/Airlock boundary |
| Monitoring time creates guilt | No local suspicion |
| Old save gains guests | Empty migration |
| Visitor + survivor both active | Identity uniqueness assertion |

# 18. First Release Scope

1. Airlock handoff.
2. Visitor stay registry.
3. Refugee/Guest purpose.
4. Canonical housing slot.
5. Temporary resource consumer.
6. Medical requirement.
7. Security requirement.
8. Voluntary departure.
9. Recruitment conversion.
10. Trader or Envoy specialized path.
11. Visitor UI.
12. Headless selftest.

# 19. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --visitor-integration-selftest
```

Targeted suites:

- `VisitorAuthorityBoundaryTests`
- `VisitorAdmissionHandoffTests`
- `VisitorIdentityTests`
- `VisitorLifecycleTests`
- `VisitorProcessingRequirementTests`
- `VisitorMedicalBoundaryTests`
- `VisitorSecurityBoundaryTests`
- `VisitorHousingTests`
- `VisitorOccupancyConservationTests`
- `VisitorResourceConsumerTests`
- `VisitorInventoryBoundaryTests`
- `VisitorTradeIntegrationTests`
- `VisitorDiplomacyIntegrationTests`
- `VisitorDepartureTests`
- `VisitorForcedRemovalBoundaryTests`
- `VisitorEscapeBoundaryTests`
- `VisitorRecruitmentTests`
- `VisitorIdentityConversionTests`
- `VisitorCommunicationTests`
- `VisitorMigrationTests`
- `VisitorTimeSkipTests`
- `VisitorExploitTests`
- `VisitorPerformanceTests`

# 20. Completion Checklist

- [ ] Premise audit completed.
- [ ] Visitor-stay ADR accepted.
- [ ] Airlock remains admission owner.
- [ ] SurvivorCatalog remains permanent-identity owner.
- [ ] Recruitment remains conversion owner.
- [ ] Purpose and lifecycle are distinct.
- [ ] No authoritative integration percentage exists.
- [ ] Requirements are owner-backed.
- [ ] Medical owns medical clearance.
- [ ] ShelterSecurity owns monitoring/findings.
- [ ] Housing uses E1-9 occupancy.
- [ ] E1-17 owns condition.
- [ ] Visitors consume canonical resources.
- [ ] Trader path remains E1-19-owned.
- [ ] Envoy path remains E1-21-owned.
- [ ] Possessions remain Inventory-owned.
- [ ] Voluntary/invited/forced/escaped/recruited outcomes are distinct.
- [ ] Escape is Security-owned.
- [ ] Forced removal is Governance/Security-owned.
- [ ] Recruitment produces one permanent identity.
- [ ] Temporary consumer cannot survive conversion.
- [ ] Old saves receive no fabricated visitors.
- [ ] Routine processing uses no RNG.
- [ ] Conservation and exploit tests pass.
- [ ] Fairness/bureaucracy review passes.
- [ ] Long-campaign soak passes.
- [ ] `E1_planintegration[26].md` is the next sequence filename.

# 21. Scenario Review Bank

For each scenario identify admission provenance, visitor-stay state, housing/service owner, Medical/Security
requirements, possessions, departure/recruitment owner, save/idempotency behavior, and one negative duplicate-
authority assertion.

1. Refugee admitted conditionally into quarantine.
2. Trader stays overnight while Market session remains active.
3. Envoy negotiation ends early and departure is rescheduled.
4. Guest room becomes unusable under E1-17.
5. Food becomes scarce with several visitors present.
6. Medical clearance succeeds while Security investigation remains open.
7. Defector comes from hostile faction but has no actual adverse findings.
8. Visitor refuses recruitment.
9. Recruitment commits on autosave boundary.
10. Voluntary departure and forced-removal order race.
11. Visitor leaves with a shelter-issued respirator.
12. Parting-gift callback fires twice.
13. Visitor dies during stay.
14. Returning trader reuses stable person identity but gets a new stay.
15. Old save loads with a pending Airlock event.
16. Maximum campaign handles many overlapping stays.

# 22. Final Directive

Plan 214 should make admitted visitors feel like real temporary occupants rather than instant recruits or
disposable event objects.

A refugee should need somewhere to sleep and something to eat without immediately becoming a permanent
survivor. A trader should be hosted without copying market cargo. An envoy should live in the shelter for the
duration of a real diplomatic mission. A defector may request asylum while Security investigates evidence
rather than a hidden suspicion meter. A guest should be able to complete a successful stay and leave.

The architectural standard is:

**Airlock decides entry; E1-25 manages the temporary stay; canonical shelter services support the visitor;
Security, Medical, Trade, and Diplomacy resolve their own concerns; Recruitment alone creates the permanent
survivor.**

If `VisitorIntegrationSystem` starts storing copied health, needs, room condition, suspicion, possessions,
market state, diplomatic mission state, or directly creates permanent survivors, stop and restore the
boundary.


---

# 23. Execution Evidence Worksheets

## E1-25A — Premise verification and authority audit

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25B — Visitor-stay ownership ADR

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25C — Stable visitor identity

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25D — Visitor purpose catalog

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25E — Stay lifecycle state machine

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25F — Airlock admission handoff

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25G — Conditional admission and quarantine

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25H — Processing requirement schema

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25I — Derived readiness read model

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25J — Orientation

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25K — Medical clearance

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25L — Security clearance

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25M — Trader stay path

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25N — Diplomatic envoy stay path

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25O — Refugee/asylum stay path

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25P — Defector/deserter boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25Q — Guest stay path

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25R — Housing assignment transaction

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25S — Housing policy types

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25T — Housing quality read model

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25U — Housing reassignment

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25V — E1-9 shelter expansion integration

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25W — E1-17 condition boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25X — Service entitlement policy

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25Y — Temporary food/water consumer

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25Z — Medical resource use

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AA — Temporary work boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AB — Schedule integration

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AC — Access control

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AD — Escort requirement

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AE — Monitoring boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AF — Suspicious activity handoff

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AG — Detention/forced custody boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AH — Visitor possessions

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AI — Shelter-issued items

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AJ — Parting gifts

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AK — Voluntary departure

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AL — Invited departure

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AM — Forced removal

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AN — Escape

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AO — Planned departure scheduler

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AP — Departure receipt

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AQ — Recruitment eligibility handoff

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AR — Recruitment conversion transaction

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AS — Identity continuity after recruitment

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AT — Visitor death boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AU — Illness during stay

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AV — Resource shortages

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AW — Consent and agency

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AX — Relationship boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AY — E1-24 communication integration

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25AZ — Faction/diplomacy consequence boundary

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BA — Trader departure settlement

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BB — Temporary contribution accounting

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BC — Visitor processing orchestration

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BD — Language/cultural orientation gate

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BE — Hosting-conditions read model

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BF — Visitor overview UI

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BG — Visitor detail UI

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.

## E1-25BH — Housing UI

- [ ] Authority-map delta captured.
- [ ] Stable operation/provenance IDs defined.
- [ ] Negative duplicate-owner test added.
- [ ] Save/load idempotency tested.
- [ ] Occupancy/resource/item/identity conservation tested where relevant.
- [ ] UI/privacy/fairness evidence captured where relevant.
