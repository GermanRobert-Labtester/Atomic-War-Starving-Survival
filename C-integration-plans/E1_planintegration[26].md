---
PLAN_ID: E1-26
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 26
STATUS: READY_FOR_EXECUTION_WHEN_ARCHIVE_INVENTORY_MEMORIAL_AND_CULTURE_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 218 — Shelter Museum & Historical Archive System"
SEQUENCE_FILENAME: "E1_planintegration[26].md"
PREVIOUS_FILENAME: "E1_planintegration[25].md"
NEXT_FILENAMES:
  - "E1_planintegration[27].md"
  - "E1_planintegration[28].md"
CATEGORY: LINK+CULTURE+MUSEUM+ARCHIVE+ARTIFACTS+MEMORIAL
PRIMARY_INTENT: "Create physical historical curation and exhibition over canonical Inventory items, Plan 162 archive records, Memorial/E1-23 death history, E1-9 rooms/display fixtures, E1-17 condition, E1-20 staffing, Needs/Psychology, Art & Culture, Time Capsules, and Genealogy without duplicating item ownership, historical truth, morale, preservation condition, or survivor identity."
PREMISE_VERIFICATION_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_INVENTORY_LEDGER_FORBIDDEN: true
SECOND_ARCHIVE_HISTORY_SYSTEM_FORBIDDEN: true
SECOND_MEMORIAL_SYSTEM_FORBIDDEN: true
SECOND_ITEM_CONDITION_SYSTEM_FORBIDDEN: true
SECOND_MORALE_SYSTEM_FORBIDDEN: true
AUTHORITATIVE_HISTORICAL_SIGNIFICANCE_SCORE_FORBIDDEN_BY_DEFAULT: true
RNG_FOR_ROUTINE_CURATION_OR_VISITS_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: HIGH
PROVENANCE_RISK: VERY_HIGH
ITEM_CONSERVATION_RISK: VERY_HIGH
CONTENT_SCALE_RISK: HIGH
UI_SCALE_RISK: HIGH
---

# E1 Plan Integration [26] — Shelter Museum, Historical Curation, Artifact Provenance, Exhibition Space, Preservation, and Public Memory

> **Sequence rule:** this file is `E1_planintegration[26].md`. The next file is `E1_planintegration[27].md`.

## 0. Mission

Plan 218 aims to give the shelter a physical cultural memory: significant tools, weapons, documents, clothing,
personal effects, prototypes, specimens, and records can be preserved and displayed so that the shelter's
history becomes visible in the world rather than existing only in logs.

The goal is strong, but the source design risks duplicating several existing authorities:

- Inventory already owns real items and their instance state.
- Plan 162 owns archival historical records.
- MemorialSystem and E1-23 own remembrance and death provenance.
- Plan 178 owns art exhibitions.
- E1-9 owns shelter rooms, topology, and construction.
- E1-17 owns maintainable asset condition.
- E1-20 owns formal survivor appointments such as curator/historian if such a role is added.
- Needs/Psychology owns morale and emotional consequences.
- Time Capsule and Genealogy systems, if implemented, own their own records and lineage/time-capsule truth.

E1-26 therefore defines the museum as a **curation and physical-display layer**.

The museum can decide that a real Inventory item is accessioned into the collection, place that item into a
canonical museum storage/display container, attach provenance/history references, assign it to a display slot,
compose an exhibition from real artifacts and archive records, expose that exhibition to survivors/visitors,
and publish a historical presentation.

It does not clone the item.
It does not rewrite historical truth.
It does not decide cause of death.
It does not own morale.
It does not invent a second item-condition meter.
It does not turn every item into a universal 0–100 historical-score object.

The architectural standard is:

**Inventory owns objects; Archive/Memorial/History systems own facts; E1-26 curates those objects and facts
into physical collections and exhibitions.**

## 1. Source Intent Preserved

Plan 218 asks for:

- museum collections;
- historical artifacts;
- artifact types;
- origin stories;
- significance;
- exhibitions;
- curators;
- visitors;
- morale value;
- physical museum space;
- preservation;
- UI;
- events and quests;
- deterministic behavior;
- persistence;
- integration with Memorial, Inventory, SurvivorFate, PersonalBelongings, Time Capsules, and Genealogy.

E1-26 preserves all useful player-facing goals while correcting ownership and conservation boundaries.

## 2. Core Architecture

```text
canonical object / historical fact
        |
        +--> Inventory item instance
        +--> Plan 162 archive record
        +--> E1-23 death/memorial record
        +--> Time Capsule opened record
        +--> Genealogy/family history
        +--> Quest/event/history provenance
        |
        v
Museum accession
        |
        +--> museum accession ID
        +--> item/record reference
        +--> provenance bundle refs
        +--> curator notes / presentation metadata
        +--> preservation/display policy
        |
        v
canonical storage / display
        |
        +--> museum storage Inventory container
        +--> E1-9 display fixture / room slot
        +--> E1-17 display-case condition
        |
        v
Exhibition
        |
        +--> theme
        +--> accession refs
        +--> archive/memorial refs
        +--> start/end
        +--> visitor/read-model state
        |
        v
canonical cultural consequence
        |
        +--> Needs/Psychology / social event
        +--> Journal/Chronicle
        +--> E1-24 communication
        +--> Quest hooks
```

## 3. Architectural Corrections

### 3.1 `MuseumArtifact` must not duplicate the item

A museum artifact record should reference a stable Inventory item instance or archival record. It may snapshot
a label for historical UI resilience, but item condition, modifications, ammunition, durability, stack
quantity, and ownership remain canonical Inventory state.

### 3.2 Origin stories need provenance references

`originStory` should not be a free-form authoritative history string. Use references to campaign events,
archive records, combat incidents, survivor records, gifts, crafting provenance, expeditions, or manually
authored curator notes clearly distinguished from factual provenance.

### 3.3 Historical significance should be explainable, preferably derived

A universal 0–100 score based on age + donor importance + rarity invites opaque meta-gaming. Prefer typed
significance reasons: Founding, MajorDefense, SurvivorLegacy, TechnologicalMilestone, Diplomatic, Medical,
Cultural, RarePreWar, FamilyHistory, etc. If a score is retained for sorting, derive it from explicit evidence
and do not persist it as historical truth.

### 3.4 Museum item condition belongs to Inventory/E1-17

If display/preservation damages items, the real item condition changes through the item/maintenance authority.
Display cases and climate-control hardware use E1-17. The museum must not keep a parallel `condition=0..100`.

### 3.5 Morale belongs to Needs/Psychology

An exhibition can emit a cultural-visit event with theme/context. Needs/Psychology decides whether and how
that affects survivors. No `moraleBoost` field should be a universal direct mutation.

### 3.6 Visitor counts are secondary telemetry

A count can be retained as bounded exhibition telemetry if it has a real use, but routine individual visits
should not create huge logs. Prefer aggregate attendance and significant-visit events.

### 3.7 Curator should be an E1-20 appointment or Duty context

Do not store a standalone curator survivor ID as a second role system. Query an E1-20 Curator/Historian role
or a temporary Duty assignment.

### 3.8 Art exhibitions and historical exhibitions must not duplicate Plan 178

Plan 178 owns art/cultural exhibition semantics. E1-26 owns historically curated displays. Shared exhibition
infrastructure should be reused where possible.

### 3.9 Memorial artifacts require lawful Inventory ownership

E1-23 estate settlement and MemorialSystem decide what happened to possessions. Museum accession may occur
only after an item legally/canonically becomes available for donation/transfer.

### 3.10 Routine museum mechanics use no RNG

Accession, display placement, exhibition scheduling, preservation checks, and visitor eligibility are
deterministic. Narrative discovery events may use keyed RNG in their owning event systems.

## 4. Non-Negotiable Rules

- Inventory owns real artifact item instances.
- Plan 162/archive authority owns historical record truth.
- E1-23/Memorial own death/memorial provenance.
- E1-9 owns museum room topology, display fixtures, storage rooms, and construction.
- E1-17 owns display-case/environment-control hardware condition.
- Needs/Psychology owns morale, grief, inspiration, stress, and cultural satisfaction.
- E1-20/Duty owns curator staffing.
- Plan 178 owns art exhibition mechanics where overlapping.
- TimeCapsuleSystem owns unopened/opened capsule state.
- Genealogy owns family/lineage records if implemented.
- E1-24 owns museum announcements/notices.
- QuestSystem owns museum quests/rewards.
- Museum owns accession, curation metadata, display assignment, exhibition composition, preservation policy refs, and museum UI/read models only.
- An artifact cannot be both freely usable in inventory and physically displayed unless the design explicitly models accessible display storage.
- Accession moves/locks the real item through Inventory.
- Deaccession returns/transfers the same real item.
- Museum history cannot recreate consumed/destroyed items.
- A museum record may preserve history after an item is destroyed, but must mark the object unavailable/lost.
- No copied item condition.
- No copied survivor fate.
- No copied archive truth.
- No direct morale mutation.
- No universal authoritative historical significance score by default.
- No random visitor-count generation for routine attendance.
- No random artifact discovery inside museum tick.
- No auto-donation of personal items without explicit owner policy/consent.
- No museum claim over estate items before E1-23 settlement.
- No time capsule display before canonical opening.
- No family artifact label without Genealogy/source provenance.
- Old saves receive no fabricated museum, artifacts, exhibitions, or donations.
- First release proves one room/fixture, one accession, one exhibition, one visitor event, and one deaccession before 20+ artifact templates.

## 5. Acceptance Slices

### Slice A — Museum space and accession
Construct/identify a museum room/display slot and move one real Inventory item into museum custody.

### Slice B — Provenance and exhibition
Attach canonical historical references and curate one deterministic exhibition.

### Slice C — Attendance/cultural handoff
One survivor visits; Needs/Psychology receives a cultural event without E1-26 directly changing morale.

### Slice D — Memorial/archive integration
Display one E1-23/Plan 162-linked historical object safely.

### Slice E — Advanced preservation, family/time-capsule/art integration
Only after A–D pass.

---

## E1-26A — Premise verification and museum-authority audit

1. Inspect MemorialSystem, Inventory item/container/instance APIs, SurvivorFate, Plan 162 archive implementation, Plan 178 art/culture exhibitions, E1-23 death/estate, E1-9 topology/construction, E1-17 maintenance, E1-20 roles, Needs/Psychology, TimeCapsule, Genealogy, PersonalBelongings, Journal, E1-24 communication, QuestSystem, and save registry.
2. Search for museum/archive/display/exhibition/artifact/heritage/curator/gallery/showcase/trophy/relic/history/provenance APIs.
3. Determine whether Inventory has stable instance IDs and container-lock/reservation semantics.
4. Determine whether Plan 162 archive records expose stable record IDs and historical event refs.
5. Create `docs/systems/MUSEUM_AUTHORITY_MAP.md` and set premise verification to current HEAD.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26B — Museum ownership ADR

1. Compare one `ShelterMuseumSystem`, collection registry + display service, and extension of shared culture/exhibition infrastructure.
2. Define museum-owned facts: accession IDs, provenance refs, curation metadata, storage/display assignments, exhibition composition/schedule, preservation policy refs, bounded attendance telemetry.
3. Explicitly exclude item state, archive truth, death history, room condition, morale, survivor role, and quest state.
4. Define overlap strategy with Plan 178 art exhibitions.
5. Require architecture review before implementation.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26C — Museum identity and lifecycle

1. Define museum ID/site ID, museum-room refs, established day, enabled/operational state derived from installed capabilities, curator-role query, storage container ref, and active/planned exhibition refs.
2. Do not store copied room condition or power state.
3. Allow no-museum state cleanly.
4. Support later multiple sites only if E1-10 warrants it.
5. Add creation/removal/restore tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26D — Accession identity

1. Define stable accession ID separate from item ID.
2. Reference exactly one Inventory item instance, archive record, or non-item historical record according to accession type.
3. Store accession day, accession source/provenance, acquisition mode, curator note refs, and current disposition.
4. Do not copy condition/durability/quantity.
5. Add uniqueness tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26E — Accession type taxonomy

1. Separate PhysicalItem, DocumentRecord, MemorialRecord, TimeCapsuleOpenedRecord, FamilyRecord, and CompositeDisplay only if real consumers need them.
2. Artifact presentation categories such as Document, Tool, Weapon, Clothing, PersonalEffect, Technological, Natural, Historical are tags, not ownership classes.
3. Do not force eight categories where source data lacks support.
4. Validate category-to-source compatibility.
5. Keep taxonomy data-driven.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26F — Museum collection registry

1. Store accession refs and collection groupings, not duplicated artifacts.
2. Use deterministic ordering/filtering.
3. Support active, stored, displayed, loaned, lost/destroyed, deaccessioned states.
4. Archive closed accessions compactly.
5. Do not persist `totalValue` as historical truth.
6. Add registry round-trip tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26G — Inventory accession transaction

1. Validate item exists and is eligible for museum transfer.
2. Check canonical owner/loan/quest/estate restrictions.
3. Move the real item into a museum storage/display container via Inventory.
4. Create accession only after transfer succeeds.
5. Use stable operation ID.
6. Add double-submit/save-load conservation tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26H — Accession eligibility policy

1. Reject quest-critical, faction-owned, borrowed, actively equipped, reserved, unsafe, or otherwise non-transferable items unless explicit policy permits.
2. Allow owner donation, shelter-owned curation, estate release, expedition recovery, or institutional transfer as distinct acquisition sources.
3. Do not auto-donate merely because significance is high.
4. Return explainable blockers.
5. Add policy tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26I — Deaccession transaction

1. Return/sell/transfer an accessioned physical item through Inventory.
2. Remove display assignment first.
3. Preserve accession history and final disposition.
4. Do not recreate item from item definition.
5. Use stable operation ID.
6. Add item-identity conservation tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26J — Museum storage container

1. Use a real Inventory container for physical accessions.
2. Define access/withdrawal policy.
3. Prevent crafting/equipping from museum storage unless deaccessioned/authorized.
4. Do not create a second hidden item list.
5. Handle storage capacity if Inventory models it.
6. Add container access tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26K — Display-slot contract

1. Reference E1-9 room/display fixture/slot IDs.
2. Define accepted artifact size/category/safety constraints.
3. One physical item occupies at most one display slot.
4. Display assignment is separate from item ownership.
5. Add slot uniqueness and compatibility tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26L — E1-9 museum room integration

1. Use canonical room/topology IDs for museum/gallery/archive spaces.
2. Construction/activation belongs to E1-9.
3. Do not create museum rooms in museum save directly.
4. Handle room renovation/deactivation by invalidating display assignments safely.
5. Add expansion/removal tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26M — Display fixture integration

1. Display cases, racks, plaques, cabinets, frames, and archive shelving should be E1-9 infrastructure where gameplay matters.
2. Reference fixture capabilities such as size, security, climate control, visitor access.
3. Do not store duplicate room coordinates.
4. Start with minimal fixture types.
5. Add capability validation.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26N — E1-17 preservation-hardware boundary

1. Climate cabinets, locked cases, archive cabinets, lighting, dehumidifiers, and similar maintainable hardware query E1-17.
2. Museum does not store hardware condition.
3. Failure may reduce preservation/display capability.
4. Repair remains E1-17-owned.
5. Add failed/restored fixture tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26O — Artifact item-condition boundary

1. Real item condition belongs to Inventory/item durability owner.
2. If display exposure causes wear, apply through canonical item-condition API.
3. Do not keep `MuseumArtifact.condition`.
4. Historical record survives even if the physical item degrades or is destroyed.
5. Add degrade/destroyed-item tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26P — Preservation policy

1. Define storage/display requirements by item/material/category only where meaningful.
2. Examples: dry storage, sealed case, low light, secure mount, contamination isolation.
3. Policy affects eligibility/risk through canonical environment/condition systems.
4. Do not invent generic preservation quality percent.
5. Add incompatible-display tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26Q — Hazardous artifact gate

1. Audit weapons, ammunition, chemical, biological, radiological, sharp, explosive, and contaminated items.
2. Require safe display/storage policy from canonical hazard/security authorities.
3. Do not let museum bypass weapon/ammo safety or contamination rules.
4. Disable hazardous artifact categories if no safe-handling owner exists.
5. Add hazard compatibility tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26R — Weapon artifact boundary

1. Accession real weapon instance through Inventory.
2. Define whether ammunition is removed by canonical unload transaction.
3. Display case/security policy controls access.
4. Combat stats remain weapon/item-owned.
5. Do not clone a display-only weapon while retaining usable original unless explicitly a replica item.
6. Add weapon conservation tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26S — Document artifact boundary

1. Physical document item may be accessioned; archival record remains Plan 162-owned.
2. Digital/archive-only record may be exhibited as a non-item display reference.
3. Do not duplicate document text into museum authority unless needed as a snapshot.
4. Respect classified/private information policy.
5. Add physical/digital document tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26T — Personal effect boundary

1. Use PersonalBelongings/Inventory provenance when available.
2. Donation requires canonical ownership/consent or completed E1-23 estate transfer.
3. Do not seize sentimental property automatically.
4. Preserve donor/source relationship refs.
5. Add living-owner/deceased-owner tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26U — Memorial artifact integration

1. E1-23/Memorial owns death, estate, burial, and memorial facts.
2. Museum may accession an item only after estate/owner transfer makes it available.
3. Exhibition can reference death record/memorial record.
4. Do not copy cause-of-death truth into editable museum metadata.
5. Add estate-to-museum provenance tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26V — SurvivorFate boundary

1. Death itself creates no automatic museum donation.
2. SurvivorFate/E1-23 supplies historical identity and estate context.
3. Museum requests/receives legal item transfer after settlement.
4. Do not create relics from every death.
5. Add no-donation-by-default test.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26W — Plan 162 archive integration

1. Use stable archive record IDs as historical provenance and exhibit content.
2. Museum presentation may select excerpts/summary through Archive/localization APIs.
3. Do not rewrite archive truth.
4. Do not duplicate the full archive in museum save.
5. Add missing/retired record tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26X — Provenance bundle schema

1. Allow refs to source event, archive record, survivor, expedition, combat incident, project, treaty, quest, donor, crafted-by, discovered-at, and prior owner only where canonical.
2. Separate verified provenance from curator interpretation.
3. Version provenance bundle.
4. Do not treat free-form curator text as historical fact.
5. Add provenance validation tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26Y — Curator note versus factual history

1. Store curator notes as authored interpretation/presentation.
2. Label them distinctly from verified provenance.
3. Allow revision without mutating historical source records.
4. Do not let notes affect quest/history truth automatically.
5. Add UI labeling tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26Z — Significance-reason taxonomy

1. Prefer typed reasons such as FoundingEra, MajorDefense, SurvivorLegacy, MedicalBreakthrough, TechnologicalMilestone, Diplomatic, Cultural, FamilyHistory, RarePreWar, ExpeditionDiscovery.
2. Each reason requires provenance evidence or explicit curator/manual classification where allowed.
3. Do not auto-score donor importance as human worth.
4. Validate reason/source combinations.
5. Keep reasons explainable.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AA — Derived significance ranking

1. If sorting needs a score, derive it from reason weights, provenance confidence, rarity metadata, age, uniqueness, and authored policy.
2. Do not persist score as historical truth.
3. Show contributing factors.
4. Do not use score to generate morale directly.
5. Add deterministic ranking tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AB — Rarity boundary

1. Use canonical item rarity/availability when available.
2. Do not assume rare item equals historically important.
3. Do not inflate significance through player hoarding.
4. Treat rarity as one optional factor.
5. Add common-significant and rare-insignificant tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AC — Age/founding-era boundary

1. Use CampaignCalendar and provenance acquisition/creation dates.
2. Old age alone does not make an item important.
3. Founding-era status requires actual early shelter provenance.
4. Do not backdate items without evidence.
5. Add date-boundary tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AD — Museum curator role

1. Prefer E1-20 Curator/Historian formal appointment or an existing culture role.
2. Role/Duty owns staffing/availability.
3. Museum queries current curator capability.
4. Do not store a second permanent curator identity authority.
5. Add absent/changed curator tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AE — Curator responsibility handoff

1. Curator may propose accession review, exhibition planning, preservation inspection, cataloguing, or guided-tour duties.
2. Use Duty/E1-6 Autonomy for actual work.
3. Do not auto-move items from role tick.
4. Skill/certification systems own competence.
5. Add responsibility handoff tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AF — Exhibition identity and lifecycle

1. Define exhibition ID, theme, accession refs, archive/memorial refs, planned/active/completed/cancelled status, start/end, location/display refs, curator refs, and presentation metadata.
2. Do not duplicate item state.
3. Use immutable closed-exhibition history.
4. Add lifecycle tests.
5. Keep active state compact.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AG — Historical exhibition versus art exhibition

1. Audit Plan 178 exhibition infrastructure.
2. Reuse shared scheduling/display/visitor infrastructure where possible.
3. Historical exhibitions require provenance-backed accessions; art exhibitions use art/culture content.
4. Do not maintain two independent visitor/morale pipelines for the same physical gallery.
5. Document shared and distinct contracts.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AH — Exhibition theme policy

1. Start with themes such as Founding, Medical, Defense, Technology, PersonalLives, Memorial, Diplomacy, Expedition, Culture only when provenance supports them.
2. Theme selects eligible provenance reasons, not arbitrary item categories.
3. Do not create factual claims from theme alone.
4. Validate accession/theme compatibility.
5. Keep theme data-driven.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AI — Exhibition planning transaction

1. Validate gallery/display capacity, artifact availability, storage/access restrictions, curator staffing, and dates.
2. Reserve display slots if needed.
3. Do not remove items from storage until activation transaction.
4. Use stable operation ID.
5. Add double-plan/cancel tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AJ — Exhibition activation

1. Move/assign physical accessions to reserved display slots transactionally.
2. Activate archive/digital record displays without cloning items.
3. Revalidate fixture condition/security.
4. Commit active status only after all mandatory displays succeed.
5. Add partial-failure rollback tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AK — Exhibition closure

1. Return physical artifacts to museum storage or next approved display.
2. Release slots.
3. Preserve exhibition history/attendance summary.
4. Do not deaccession items automatically.
5. Add closure/save tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AL — Exhibition cancellation

1. Release reservations and preserve item ownership.
2. Do not emit cultural attendance effects.
3. Handle cancellation after partial setup.
4. Use operation ID.
5. Add cancel/retry tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AM — Visitor/attendance ownership ADR

1. Audit whether survivors have leisure/free-time/visit systems.
2. If yes, museum visit is a canonical leisure/social action.
3. If not, prefer aggregate deterministic attendance model tied to schedule/population rather than fake per-survivor logs.
4. Do not add a full visitor-AI system inside museum.
5. Require ADR before detailed NPC attendance simulation.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AN — Survivor museum visit

1. Use Schedule/Duty/Autonomy/free-time owner to create visit action where supported.
2. Museum validates exhibition open/access.
3. Needs/Psychology receives cultural exposure event after real visit.
4. Do not directly add morale.
5. Add visit/no-exhibition/interrupted tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AO — Aggregate attendance

1. If individual visits are not simulated, compute attendance from open hours, population, accessibility, and canonical leisure policy.
2. Use deterministic formulas.
3. Do not roll random visitor counts daily.
4. Store only bounded totals/interval summaries if needed.
5. Add stepped/time-skip equivalence tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AP — Visitor count telemetry

1. Treat counts as telemetry/read-model data, not cultural value.
2. Aggregate by exhibition/day or milestone.
3. Do not write one log row per routine visitor.
4. Do not unlock content solely by grinding count unless Quest policy explicitly chooses it.
5. Add save-size tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AQ — Cultural consequence handoff

1. Emit typed `MuseumVisit`/`ExhibitionExperienced` event with survivor/exhibition/theme/provenance refs.
2. Needs/Psychology/social systems own morale/inspiration/grief consequences.
3. Different survivors may react differently according to canonical traits/context.
4. Do not keep `moraleBoost` in exhibition authority.
5. Add no-direct-morale test.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AR — Memorial exhibition psychology boundary

1. Memorial-themed exhibitions may trigger grief/remembrance context through E1-23/Psychology.
2. Do not assume memorial display always raises morale.
3. Use relationship/death provenance.
4. Keep sensitive presentation restrained.
5. Add survivor-with/without-relationship tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AS — Security and theft boundary

1. Audit whether museum artifacts can be stolen or vandalized.
2. If supported, ShelterSecurity owns incident/investigation; Inventory owns item movement/loss.
3. Museum updates accession disposition from canonical outcome.
4. Do not create a separate theft roll.
5. Feature-gate if no theft system exists.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AT — Artifact loan boundary

1. Define temporary loan only if another settlement/exhibition system can receive real items.
2. Inventory/transport owns physical movement.
3. Museum stores loan agreement/status ref.
4. Do not clone the artifact for both museums.
5. Add overdue/lost/returned tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AU — Time Capsule integration

1. TimeCapsuleSystem owns sealed/opened state and contents.
2. Only opened/released contents or opened-capsule object may be accessioned.
3. Do not display hidden capsule contents early.
4. Preserve opening event provenance.
5. Add sealed/opened tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AV — Genealogy integration

1. Genealogy owns lineage/family links.
2. Museum can present family-tree/history refs and accession family-associated artifacts after canonical ownership transfer.
3. Do not infer kinship from item provenance.
4. Do not copy genealogy graph.
5. Add missing/known lineage tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AW — Personal Belongings integration

1. Use belongings provenance to identify personal significance.
2. Donation requires owner consent/transfer or estate settlement.
3. Do not automatically museumize sentimental items.
4. Preserve ownership history.
5. Add donation/revocation-before-transfer tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AX — Natural specimen boundary

1. Only support natural specimens if Inventory/content defines them.
2. Handle biological/chemical/radiological hazards through canonical systems.
3. Do not create arbitrary specimen durability/preservation mechanics locally.
4. Feature-gate unsupported specimens.
5. Add safe/unsafe specimen tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AY — Technological artifact boundary

1. Inventions/prototypes remain canonical crafted/item instances.
2. ResearchSystem owns invention/research milestone truth.
3. Museum may reference project completion provenance.
4. Do not disable usable technology merely by declaring it significant without real accession transfer.
5. Add prototype accession/deaccession tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26AZ — Collection completeness/read model

1. Derived collection summaries may show themes/eras/reasons represented.
2. Do not persist completion percentages as historical truth.
3. Do not require one artifact of every arbitrary category.
4. Use actual content catalog and discovered history.
5. Add dynamic-content tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BA — Museum storage/display security

1. Use ShelterSecurity/access-control policies for restricted artifacts.
2. Curator role does not automatically bypass weapon/classified/hazard rules.
3. Inventory remains owner.
4. Do not maintain a museum-only security score.
5. Add unauthorized access tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BB — Museum environmental services

1. If preservation needs temperature/humidity/light/power, query canonical Thermal/Ventilation/Power/environment systems.
2. Do not store parallel environmental state.
3. Display policy can become blocked/degraded when services fail.
4. Item wear, if modeled, uses canonical item condition.
5. Add outage tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BC — Lighting and power boundary

1. Gallery lighting/electronic displays depend on PowerGrid where implemented.
2. Physical artifacts may remain accessible under no power according to room safety.
3. Do not store `museumPowered` independently.
4. Use E1-9/E1-17 infrastructure.
5. Add brownout tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BD — Museum hours/schedule

1. Use ShelterSchedule for opening hours/tours if gameplay warrants them.
2. Exhibition active state is not the same as open hours.
3. Do not tick visitors when museum is closed.
4. Support emergency closure.
5. Add schedule/time-skip tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BE — Public access and visitor integration

1. If E1-25 guests can visit, use VisitorStay access policy and museum access capability.
2. Do not use museum attendance to change guest diplomacy directly.
3. Security/privacy restrictions still apply.
4. Add guest/envoy/restricted access tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BF — E1-24 communication integration

1. Publish exhibition openings, closures, major accession, and memorial exhibition notices through InternalCommunication.
2. Messages reference museum/exhibition IDs.
3. Deleting notice does not cancel exhibition.
4. Respect classified/restricted exhibit privacy.
5. Add dedupe/privacy tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BG — Journal/Chronicle integration

1. Record only significant museum events: establishment, landmark accession, major exhibition, artifact loss, restoration, famous memorial exhibition.
2. Do not log every visit.
3. Use accession/exhibition operation IDs.
4. Chronicle remains historical record owner.
5. Add dedupe/retention tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BH — Museum event redesign

1. Create events from committed state changes rather than random museum tick.
2. Donation follows real Inventory transfer.
3. ExhibitionOpened follows activation.
4. ArtifactDiscovered follows canonical expedition/world event.
5. Visitor milestone follows deterministic telemetry.
6. CuratorAppointed follows E1-20 role event.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BI — Quest hook redesign

1. Prefer meaningful curation goals: establish first exhibit, preserve a founding artifact, assemble a provenance-backed thematic exhibition, recover a lost accession, open a memorial exhibition.
2. Defer raw `50 artifacts`, `100 visitors`, `10 exhibitions` as primary grind goals.
3. QuestSystem owns rewards.
4. Use accession/exhibition/source IDs.
5. Prevent duplicate milestone rewards.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BJ — Museum tutorial

1. Trigger on first valid accession opportunity or museum construction.
2. Explain transfer versus display, provenance, exhibition, and deaccession.
3. Make clear displayed items are not freely usable unless removed.
4. Do not teach significance as a mysterious score.
5. Support no-museum fallback.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BK — Museum overview UI

1. Show collection size, stored/displayed accessions, active/planned exhibitions, curator role, museum room/fixture status, and major preservation/security blockers.
2. Do not show copied item condition/room condition.
3. Link to canonical item/room systems.
4. Show no-museum state cleanly.
5. Add large-collection snapshots.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BL — Artifact detail UI

1. Show real item reference/label, accession source, verified provenance, curator notes, significance reasons, current location/disposition, preservation requirements, and exhibition history.
2. Distinguish verified facts from interpretation.
3. Show canonical condition by query if permitted.
4. Do not show an opaque score without factors.
5. Add lost/destroyed/deaccessioned states.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BM — Exhibition UI

1. Show theme, dates, gallery, accessions, archival records, curator, display slots, open/closed status, attendance summary, and cultural context.
2. Do not store direct morale bonus.
3. Show missing/broken fixture blockers.
4. Allow authoritative plan/activate/cancel/close commands.
5. Add planned/active/completed snapshots.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BN — Collection search/filter UI

1. Filter by theme, provenance reason, era, source survivor, item category, disposition, exhibition history, and availability.
2. Do not create separate search indexes as authority.
3. Index for performance only.
4. Respect classified/private records.
5. Add large-collection performance tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BO — Provenance confidence/presentation

1. Where Archive/E1-3 or source history distinguishes confirmed versus uncertain claims, preserve that distinction.
2. Do not present rumor as verified museum fact.
3. Curator notes may say 'attributed to' or 'believed to be' when provenance is uncertain.
4. Do not silently upgrade confidence.
5. Add uncertain provenance tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BP — Classified/private historical material

1. Use archive/security/privacy policy.
2. Not every letter, medical record, or diplomatic document should be public.
3. Museum can display redacted/summary form only if owner provides it.
4. Do not leak private E1-23/E1-21/E1-24 content.
5. Add access-control tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BQ — Auto-donation policy rejection

1. Disable broad `autoDonate` by default.
2. If automation is later desired, restrict it to shelter-owned items explicitly tagged museum-eligible and require policy confirmation.
3. Never auto-donate personal/estate/quest/faction-owned items.
4. Do not create surprise loss of usable equipment.
5. Add negative automation tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BR — Accession rollback/cancellation

1. Before commit, cancel leaves item in original owner/container.
2. After commit, deaccession is a separate transaction.
3. Do not delete provenance on cancellation.
4. Use idempotency keys.
5. Add interrupted transaction tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BS — Destroyed/lost accession handling

1. Canonical Inventory/World outcome marks real item destroyed/lost.
2. Museum updates accession disposition and preserves historical record.
3. Do not resurrect item on load.
4. Exhibition revalidates missing accessions.
5. Add destruction/loss tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BT — Replica and reproduction boundary

1. Replicas must be real canonical items/content if supported.
2. Do not duplicate originals merely for display.
3. Replica display should be labeled as such.
4. Historical provenance links to original.
5. Feature-gate if no replica crafting exists.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BU — Trade/sale boundary

1. Deaccession before sale unless a canonical museum-loan/sale workflow exists.
2. Market/E1-19 owns price/trade settlement.
3. Historical significance does not directly multiply price inside Museum.
4. Record disposition as sold/transferred after canonical transaction.
5. Add sale tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BV — Donation consent and ownership

1. Living survivor donation requires actual owner consent/transaction when personal ownership exists.
2. Shelter-owned item donation requires governance/inventory authority.
3. E1-23 estate donation requires settlement availability.
4. Do not treat curator selection as ownership transfer.
5. Add consent and ownership tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BW — Historical-value exploitation audit

1. Prevent significance farming by repeatedly equipping, renaming, donating, displaying, or transferring the same item.
2. Evidence reasons derive from real unique events/provenance.
3. One event should not generate duplicate significance tags.
4. Deaccession/reaccession preserves accession history where appropriate.
5. Add property tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BX — Inventory conservation audit

1. Count physical accessioned item instances before/after accession, display, storage, deaccession, loan, loss, and sale.
2. Assert exactly one canonical item location.
3. Test unique weapons, stackable documents/resources, modified items, and quest-restricted items.
4. Use Inventory transaction receipts.
5. Block release on duplication/loss.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BY — Morale/psychology boundary audit

1. Search for any museum code directly mutating morale/stress/affinity.
2. Require cultural event handoff instead.
3. Test exhibition with Needs/Psychology disabled/stubbed: museum still functions.
4. Test no-survivor visit means no visit consequence.
5. Prevent duplicate cultural effects from Plan 178 + E1-26.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26BZ — Art & Culture overlap audit

1. Map shared concepts: gallery room, exhibition schedule, attendance, curator, cultural event, UI.
2. Extract/reuse shared exhibition primitives where reasonable.
3. Keep provenance-heavy historical curation distinct from art production/display.
4. Prevent double counting attendance/morale.
5. Document overlap ADR.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CA — Time-capsule overlap audit

1. Ensure sealed capsules remain inaccessible.
2. Opened capsule records/items may be accessioned only through real transfer.
3. Do not duplicate capsule history.
4. Do not count same capsule contents as multiple accession items without real item identity.
5. Add opened/empty/lost capsule tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CB — Genealogy/family artifact overlap audit

1. Family association comes from Genealogy/provenance.
2. Do not infer lineage from donor names.
3. Display may reference family tree read model.
4. Do not copy genealogy graph into museum save.
5. Add family-history consistency tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CC — Old-save migration

1. Initialize museum registry/accessions/exhibitions empty.
2. Do not auto-create museum room.
3. Do not auto-accession notable Inventory items or memorial possessions.
4. Preserve Plan 162 archive, Memorial, Inventory, Art/Culture, Time Capsule, Genealogy unchanged.
5. Version migration and add early/late save fixtures.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CD — Existing archive/memorial discovery UX

1. After feature enable, show eligible historical records/items as recommendations, not automatic accessions.
2. Recommendations are derived and non-authoritative.
3. Do not move items.
4. Allow player/curator to review provenance before accession.
5. Add recommendation consistency tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CE — Save contract and restore ordering

1. Persist museum identity/room refs, accession metadata/provenance refs/disposition, exhibition composition/lifecycle, display assignments, bounded attendance telemetry, and schema version.
2. Do not persist copied item state, archive records, death records, room condition, morale, roles, or power.
3. Restore Inventory/Archive/Memorial/topology before resolving accessions/displays.
4. Rebuild derived significance/quality/availability read models.
5. Do not replay donation/visit/exhibition events.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CF — Zero-RNG baseline

1. Accession, display assignment, exhibition planning, preservation validation, attendance formulas, and archive integration are deterministic.
2. Artifact discoveries belong to world/expedition/event owners.
3. Narrative flavor variation may use keyed RNG outside simulation truth.
4. Do not call shared global RNG from museum tick.
5. Add call-order determinism tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CG — Time-skip semantics

1. Use CampaignCalendar.
2. Advance exhibition start/end, aggregate attendance, preservation/service exposure, and scheduled tours once.
3. Do not generate one visit log per skipped day.
4. Canonical item/environment systems own wear/condition during skip.
5. Add stepped-versus-batch tests.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CH — Museum content scale budget

1. Start with 8–12 strong provenance/display archetypes, not 20+ arbitrary artifact types.
2. Prefer content that references real systems/events.
3. Reject template types with no source items or historical provenance.
4. Measure authoring burden.
5. Add catalog-coverage report.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CI — Player-burden audit

1. Measure accession decisions, exhibition setup actions, inventory transfers, preservation warnings, and archive browsing per campaign month.
2. Support bulk filtering and curator recommendations without auto-seizing items.
3. Do not require constant re-curation for morale optimization.
4. Use long exhibition durations and clear blockers.
5. Set UX burden targets.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CJ — Performance and long-campaign museum soak

1. Run maximum campaign with hundreds of accessions/history refs and many completed exhibitions.
2. Exercise accession/deaccession, display, memorial items, time capsules, genealogy refs, room changes, power/maintenance faults, visits, and archive filtering.
3. Measure active accessions, archive refs, save size, UI search/filter cost, attendance processing, and allocations.
4. Do not tick historical records every frame.
5. Record median/p95 costs.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CK — Headless museum selftest

1. Construct/map one museum room/display slot.
2. Accession one real Inventory item and verify conservation.
3. Attach one Plan 162/E1-23 provenance ref.
4. Plan and activate one exhibition.
5. Visit through canonical/aggregate attendance path and verify no direct morale mutation.
6. Close exhibition, deaccession item, and confirm same instance returns.
7. Save/reload at accession/display/exhibition boundaries.
8. Expose `--shelter-museum-selftest`.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CL — Data-integrity selftest

1. Validate museum templates, accession categories, Inventory item refs, provenance owner refs, E1-9 room/fixture refs, E1-17 capability refs, curator role refs, exhibition themes, privacy policies, and localization.
2. Reject duplicate accession of one physical item.
3. Reject orphan archive/memorial/time-capsule/genealogy refs.
4. Reject direct morale or copied condition fields in museum data contracts.
5. Wire into `--data-integrity-selftest`.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CM — Documentation and observability

1. Create `docs/systems/SHELTER_MUSEUM.md`, `MUSEUM_ACCESSION_CONTRACT.md`, `MUSEUM_PROVENANCE.md`, `MUSEUM_EXHIBITIONS.md`, and `MUSEUM_PRESERVATION_BOUNDARIES.md`.
2. Document Plan 162/178/E1-23/E1-9/E1-17/E1-20/Needs boundaries.
3. Document verified provenance versus curator interpretation.
4. Add debug readout for accession/item refs, provenance, display slot, availability, exhibition, preservation blockers, and source-system status.
5. Keep debug mutation dev-only.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

## E1-26CN — Release gate

1. Run .NET build/test and game build.
2. Run data-integrity and shelter-museum selftests.
3. Run Inventory conservation, provenance, archive/memorial boundary, display-slot, E1-17/E1-9, curator, attendance, Needs/Psychology, Plan 178 overlap, migration, time-skip, privacy, exploit, and performance tests.
4. Verify one accession + one exhibition + one visit + one deaccession before broad artifact template expansion.
5. Mark DONE only when the museum curates real historical objects/facts without becoming a second Inventory, Archive, Memorial, or morale system.

**Acceptance gate:** canonical owners remain singular; provenance is explicit; physical-item conservation and save/load idempotency are tested where applicable; derived significance/attendance never become hidden authorities.

---

# 6. Canonical Museum Authority Matrix

| Fact | Canonical owner | E1-26 role |
|---|---|---|
| Physical artifact item | Inventory | Reference/transfer |
| Item condition | Inventory/item durability | Query |
| Historical record truth | Plan 162 Archive | Reference |
| Death/memorial truth | E1-23/Memorial | Reference |
| Museum accession/curation | E1-26 | Own |
| Display slot/room | E1-9 | Reference |
| Display hardware condition | E1-17 | Query |
| Curator role | E1-20/Duty | Query |
| Morale/psychology | Needs/Psychology | Cultural-event consumer |
| Art exhibition | Plan 178 | Shared infrastructure / separate content |
| Time capsule | TimeCapsuleSystem | Reference after opening |
| Family lineage | Genealogy | Reference |
| Announcement | E1-24 | Presentation |
| Quest | QuestSystem | Hook only |

# 7. Suggested Accession Record

```yaml
schema_version: 1
accession_id: "museum_accession_0042"
source_kind: "PHYSICAL_ITEM"
item_instance_id: "item_rifle_022"

accession_day: 88
acquisition_mode: "ESTATE_DONATION"
acquisition_source_ref: "estate_distribution_019"

provenance_refs:
  - "combat_incident_118"
  - "death_record_044"

significance_reasons:
  - "MAJOR_DEFENSE"
  - "SURVIVOR_LEGACY"

disposition: "DISPLAYED"
display_assignment_id: "museum_display_031"
```

No copied condition or combat stats.

# 8. Suggested Exhibition Record

```yaml
exhibition_id: "museum_exhibition_defense_01"
theme_id: "SHELTER_DEFENSE"
status: "ACTIVE"
start_day: 95
end_day: 125
gallery_room_id: "room_museum_gallery"

accession_ids:
  - "museum_accession_0042"
  - "museum_accession_0061"

archive_record_refs:
  - "archive_raid_day_43"

curator_role_holder_ref: "survivor_anna"
```

# 9. Provenance Model

Verified provenance may reference:

- archive record;
- combat incident;
- survivor/death record;
- expedition discovery;
- research/project milestone;
- treaty/diplomatic event;
- crafted-by history;
- donor/estate transfer;
- time-capsule opening;
- genealogy/family record.

Curator interpretation is stored separately and labeled as interpretation.

# 10. Significance Model

Prefer reasons over one magic number.

Examples:

- Founding Era
- Major Defense
- Survivor Legacy
- Medical Breakthrough
- Technological Milestone
- Diplomatic Turning Point
- Cultural Milestone
- Expedition Discovery
- Family History
- Rare Pre-War Artifact

A derived sort score may exist, but must expose its factors and never become historical truth.

# 11. Inventory Conservation

For a physical accession:

```text
original owner/container
-> museum storage/display container
-> display slot/storage
-> deaccession/loan/sale/loss
```

At every point there is exactly one canonical item instance and one canonical location/disposition.

# 12. Memorial Integration

Correct:

```text
E1-23 estate settles item
-> item becomes available to owner/commons
-> donation/accession transaction
-> museum references death/memorial provenance
```

Incorrect:

```text
survivor dies
-> museum silently clones favorite weapon
```

# 13. Plan 162 Archive Boundary

The Archive says what happened.
The Museum decides what to display about it.

A museum presentation can summarize or contextualize a record, but may not rewrite the canonical record.

# 14. Plan 178 Art Boundary

Shared concepts that should be reused where possible:

- gallery rooms;
- exhibition scheduling;
- visitor/free-time actions;
- exhibition UI primitives;
- cultural event handoff.

Historical curation remains provenance-driven and distinct from art creation.

# 15. Attendance Boundary

A real or aggregate museum visit may emit:

```text
ExhibitionExperienced(
  survivor_id,
  exhibition_id,
  theme,
  provenance_refs
)
```

Needs/Psychology then decides the emotional consequence.

No museum-local `morale += 10`.

# 16. Hazardous Artifact Rules

Do not display hazardous objects unless canonical safety systems permit them.

Potentially restricted:

- loaded firearms;
- explosives/ammunition;
- contaminated clothing/filters;
- chemical agents;
- radioactive samples;
- biological specimens;
- sharp/unsafe machinery;
- classified documents.

Security/Contamination/Inventory/E1-17 policies decide what is safe.

# 17. Old-Save Migration

Default:

```text
museum = absent/empty
accessions = empty
exhibitions = empty
attendance = empty
```

Do not automatically:

- construct a museum;
- take items from Inventory;
- convert memorial objects;
- expose archive records;
- open time capsules.

Instead, surface eligible recommendations prospectively.

# 18. Exploit Matrix

| Exploit/failure | Guard |
|---|---|
| Accession duplicates item | Inventory transfer + accession op ID |
| Displayed item remains freely equipped | Container/access lock |
| Deaccession recreates item | Preserve instance ID |
| Re-donate farms significance | Provenance/accession history |
| Donor prestige creates arbitrary value | Evidence-based significance |
| Memorial item bypasses estate | E1-23 ownership gate |
| Time capsule revealed early | TimeCapsule owner |
| Museum directly buffs morale | Needs/Psychology handoff |
| Art + museum double attendance | Shared exhibition/visit contract |
| Weapon display bypasses safety | Security/Inventory policy |
| Old save loses items to museum | Empty prospective migration |
| Lost artifact respawns after load | Canonical item disposition |

# 19. First Release Scope

1. one museum/gallery room or mapped capability;
2. one museum storage container;
3. one display fixture;
4. one physical accession;
5. one archive/memorial provenance link;
6. one exhibition;
7. one visitor/attendance handoff;
8. one cultural-event handoff;
9. one deaccession;
10. save/load selftest.

Only then add broad templates, artifact loans, advanced preservation, time capsules, genealogy, and traveling
exhibitions.

# 20. Scenario Review Bank

For each scenario identify the canonical item/history owner, accession state, provenance confidence, physical
location, display capability, preservation/security constraints, cultural consequence owner, save/load behavior,
and one negative duplicate-authority assertion.

1. A survivor donates a personally owned coat.
2. E1-23 estate releases a deceased survivor's weapon to shelter commons.
3. A loaded firearm is proposed for display.
4. A contaminated filter has major historical significance.
5. A Plan 162 archive record has no physical artifact.
6. An artifact's provenance is uncertain or rumor-based.
7. A displayed item loses condition under an environmental failure.
8. The gallery loses power during an active exhibition.
9. A display case fails under E1-17.
10. A memorial exhibition affects a survivor closely related to the deceased.
11. A time capsule remains sealed.
12. A family artifact is associated with an unverified lineage claim.
13. An accessioned item is needed for emergency shelter use.
14. A deaccession and save happen on the same tick.
15. A large old save enables the museum feature with many notable Inventory items.
16. A maximum campaign accumulates hundreds of accessions and exhibitions.

# 21. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-museum-selftest
```

Targeted suites:

- `MuseumAuthorityBoundaryTests`
- `MuseumAccessionTests`
- `MuseumInventoryConservationTests`
- `MuseumStorageTests`
- `MuseumDisplaySlotTests`
- `MuseumProvenanceTests`
- `MuseumSignificanceTests`
- `MuseumArchiveIntegrationTests`
- `MuseumMemorialIntegrationTests`
- `MuseumArtifactConditionBoundaryTests`
- `MuseumPreservationTests`
- `MuseumHazardTests`
- `MuseumCuratorTests`
- `MuseumExhibitionTests`
- `MuseumAttendanceTests`
- `MuseumPsychologyBoundaryTests`
- `MuseumArtCultureOverlapTests`
- `MuseumTimeCapsuleTests`
- `MuseumGenealogyTests`
- `MuseumPrivacyTests`
- `MuseumMigrationTests`
- `MuseumTimeSkipTests`
- `MuseumExploitTests`
- `MuseumPerformanceTests`

# 22. Completion Checklist

- [ ] Premise audit completed.
- [ ] Museum ownership ADR accepted.
- [ ] Inventory remains physical-item owner.
- [ ] Plan 162 remains archive-truth owner.
- [ ] E1-23/Memorial remain remembrance owners.
- [ ] E1-9 owns room/display topology.
- [ ] E1-17 owns hardware condition.
- [ ] E1-20/Duty owns curator staffing.
- [ ] Needs/Psychology owns cultural consequences.
- [ ] Plan 178 overlap is reconciled.
- [ ] Accession uses real item instance.
- [ ] No copied item condition exists.
- [ ] Provenance distinguishes verified fact from interpretation.
- [ ] Historical significance is reason-based/derived.
- [ ] No direct morale boost exists.
- [ ] No random routine attendance exists.
- [ ] Memorial items require estate/ownership transfer.
- [ ] Time capsules cannot display sealed contents.
- [ ] Genealogy is referenced, not copied.
- [ ] Hazardous artifacts obey safety owners.
- [ ] Deaccession preserves item identity.
- [ ] Old saves receive no fabricated museum or donated items.
- [ ] Inventory conservation passes.
- [ ] Long-campaign performance passes.
- [ ] `E1_planintegration[27].md` is the next sequence filename.

# 23. Final Directive

Plan 218 should make the shelter's past physically visible without creating a second version of that past.

A rifle from a decisive defense should be the **same rifle** that existed in Inventory. A medic's coat should
reach the museum only through real ownership or estate transfer. A memorial display should reference the same
death record the shelter already trusts. A time capsule should remain sealed until its own system opens it.
An exhibition can interpret history, but it cannot rewrite it.

The museum's job is to curate.

It turns canonical objects and records into a deliberate public memory through accession, preservation,
display, exhibition, and presentation.

The architectural standard is:

**Inventory owns objects; Archive/Memorial/History systems own facts; E1-26 curates those objects and facts
into physical collections and exhibitions; Needs/Psychology owns the human response.**

If `ShelterMuseumSystem` starts cloning items, storing duplicate item condition, rewriting archive history,
auto-seizing personal belongings, opening time capsules, copying genealogy, directly adding morale, or
maintaining a second art-exhibition runtime, stop and restore the boundary.
