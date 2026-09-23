# ASHFALL — Expansion 24 Design Bible
# THE LONG GOODBYE
### Wave 3 · Aging, Palliative Care, Therapy, Mental Health, Legacy, and Dying Well

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Survivors` (FinalWish, Caregiving, PsychologicalArc), `Ashfall.Core.Sanatorium` (PsychologicalSanatorium), `Ashfall.Core.Needs` (SurvivorMentalHealth, Trauma), `Ashfall.Core` (MentalHealthCrisis), `Ashfall.Core.Memorial` (Memorial, Grief)
**Proposed host owner:** `LongGoodbyeHostSession` (extends `CaregivingHostSession` + `PsychologicalSanatoriumSaveStore` + `MentalHealthCrisisHostSession`)
**Existing save sections:** `caregiving`, `mental_health`, `psychology_arc`, `sanatorium`, `memorial`, `dose_ledger` (final wish state)
**Existing CLI verbs:** `--caregiving-selftest`, `--mental-health-selftest`, `--psychology-arc-selftest`, `--sanatorium-selftest`, `--memorial-selftest`, `--final-wish-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already treats the mind and the end of life with real systems, not flavor
text. `FinalWishSystem` opens a terminal survivor's Last Request questline and grants
a permanent morale buff (`WishCompletedMoraleBuff = 15`) or penalty
(`WishFailedMoralePenalty = -10`) under the buff id `their_memory_lives_on`, with a
3–7 day prognosis window. `CaregivingSystem` assigns a healthy survivor to a
bedridden companion, building bond strength, speeding recovery by 30%, and draining
the caregiver. `PsychologicalSanatoriumSystem` runs therapist→patient treatment
through a canonical condition port and only ever writes treatment progress, never
condition state. `PsychologicalTherapyCatalog` authors conditions (combat-rooted
startle sickness, flash-blindness, severe survivor guilt, siege paranoia) with
canonical surfaces and reversibility. `SurvivorMentalHealthSystem`,
`MentalHealthCrisisSystem` (acuity, status, profile, ward capacity), and
`PsychologicalArcSystem` (compulsive stashing, fire fixation, persecutory crisis,
shutdown withdrawal) model crisis and habit. `MemorialSystem` and
`GuiltInsomniaSystem` own grief and guilt. `ConfessionSecretSystem` holds a deep
corpus of things survivors have never said.

But the authored content is thin where it matters most: `mental_arcs.json` is
1.9 KB (four arcs), `psychological_therapies.json` 8 KB,
`psychological_trauma.json` 5.3 KB. A game with this much psychological machinery
has almost no therapy content, no aging, and no palliative layer.

**The Long Goodbye** turns that machinery into the campaign's most human arc: growing
old in a shelter, caring for someone who will not get better, treating what can be
treated, grieving what cannot, and deciding what a person leaves behind.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

ASHFALL is a game about survival. Eventually, survival means someone reaching the end
of their life in a place they helped build.

**The Long Goodbye** is the expansion about that end, and about the long, ordinary
years before it. It adds aging — frailty, memory loss, dependency, and the dignity of
usefulness. It adds palliative care — comfort, pain management, prognosis, and dying
without pain in a place that is not a hospital. It expands therapy from four
condition rows into a real clinical practice with modalities, therapists, setbacks,
and relapse. It adds grief as a journey rather than a mood, and legacy as a system
rather than a text string.

The expansion's hard rule is empathy without exploitation. Death is never a puzzle to
solve or a resource to farm. Grief is never a debuff to remove quickly. A survivor who
can no longer work is not a burden to optimize away; they are the reason the shelter
exists.

### 1.2 The five loops it adds

```
   Aging ──► Frailty ──► Care ──► Palliative ──► Death ──► Legacy
     │          │          │          │            │         │
     ▼          ▼          ▼          ▼            ▼         ▼
   milestones  dependence caregiver  comfort,   memorial  stories,
   decline     adjustments assignment pain,      rites     skills,
   memory      duty       bonds       prognosis            belongings
     │                     │                                  │
     ▼                     ▼                                  ▼
   Therapy ◄──── crisis ────┘                          Grief journey
     │                                                     │
     ▼                                                     ▼
   modalities, therapists, progress, relapse ◄── anniversaries, rituals
```

### 1.3 What the player manages

1. **Aging.** Authored decline curves with individual variation. Some survivors stay
   sharp and slow; others lose memory early. Aging changes roles, not worth.
2. **Frailty and dependency.** Adjustments to rooms, duties, and equipment so an
   elder can keep contributing at a level that fits.
3. **Caregiving.** The live system's assignments and fatigue; the expansion adds
   skill, rotation, burnout, and the cost of caring.
4. **Palliative care.** Comfort, pain management, prognosis, and the last wish. The
   live final-wish system becomes one part of a broader comfort model.
5. **Therapy.** Modalities, sessions, therapists, progress, setbacks, and relapse
   through the live sanatorium and mental-health systems.
6. **Crisis.** Acuity, ward capacity, intervention, restraint, and the ethics of
   both.
7. **Grief and legacy.** Grief stages and anniversaries; legacy as stories, skills,
   belongings, and roles handed on.
8. **Dying well.** The campaign's most human decision: how much intervention, how
   much comfort, and who is in the room.

### 1.4 What it is not

- Not a second mental-health system. All therapy routes through
  `PsychologicalSanatoriumSystem` and the crisis owners.
- Not a second final-wish system. `FinalWishSystem` remains the authority.
- Not a second memorial or grief owner. `MemorialSystem` and `GuiltInsomniaSystem`
  remain owners.
- Not a death-farming loop. No resource is gained by a person's death.
- Not a euphemism machine. Aging and dying are depicted honestly and without
  spectacle.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | Terminal Last Request questline; morale buff/penalty | `LIVE` |
| `Assets/Ashfall.Core/Survivors/FinalWishCatalog.cs` + loader | Wish archetypes and pools | `LIVE` |
| `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | Caregiver assignment, bond, recovery bonus, fatigue | `LIVE` |
| `Assets/Ashfall.Core/Sanatorium/PsychologicalSanatoriumSystem.cs` | Therapist→patient treatment, condition port | `LIVE` |
| `Assets/Ashfall.Core/Sanatorium/PsychologicalTherapyCatalog.cs` | Authored conditions | `LIVE` |
| `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs` | Canonical mental-health state | `LIVE` |
| `Assets/Ashfall.Core/Needs/PsychologicalTraumaCatalog.cs` | Trauma definitions | `LIVE` |
| `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` | Crisis cases, acuity, ward | `LIVE` |
| `Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs` | Habit arcs, stress thresholds, behaviors | `LIVE` |
| `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | Memorials and grief sink | `LIVE` |
| `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | Guilt authority | `LIVE` |
| `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs` | Confession corpus | `LIVE` |
| `src/Host/CaregivingHostSession.cs`, `PsychologicalSanatoriumSaveStore.cs`, `MentalHealthCrisisHostSession.cs` | Host | `LIVE` |
| `src/UI/CaregivingPanel.cs`, `MentalHealthCrisisPanel.cs`, `PsychologyArcPanel.cs` | UI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `final_wishes.json` | **62 KB** | rich wish corpus by archetype |
| `confession_secrets.json` | **95 KB** | rich confession corpus |
| `psychological_therapies.json` | 8 KB | condition rows |
| `psychological_trauma.json` | 5.3 KB | trauma definitions |
| `guilt_sources.json` | 10.4 KB | guilt sources |
| `mental_arcs.json` | **1.9 KB** | four arcs |
| `narcotics.json` | 6.4 KB | substances |
| `memorials_expansion.json` | narrative | memorial corpus |

### 2.3 Confirmed gaps

- **GAP-24-1 — No aging.** No frailty, cognitive decline, dependency, or elder
  content exists in data or systems.
- **GAP-24-2 — No palliative layer.** `FinalWishSystem` is terminal-specific; there is
  no comfort, pain, prognosis, or dying model around it.
- **GAP-24-3 — Four mental arcs.** The arc system supports far more than four rows.
- **GAP-24-4 — Therapy is a condition list.** No modalities, no therapist skill use,
  no session model, no setbacks or relapse beyond the sauna port.
- **GAP-24-5 — No caregiver skill or burnout content.** `CaregivingSystem` has
  fatigue; no competence, rotation, or burnout arc.
- **GAP-24-6 — Grief is a sink, not a journey.** `MemorialSystem` and
  `GuiltInsomniaSystem` exist; no stages, anniversaries, or long-tail content.
- **GAP-24-7 — Legacy is a buff id.** `their_memory_lives_on` is a morale modifier,
  not stories, skills, belongings, or roles.
- **GAP-24-8 — No elder locations or NPCs.**
- **GAP-24-9 — Crisis ward capacity is 2.** No authored ward, intervention, or
  restraint ethics.

### 2.4 Non-duplication statement

This expansion will **not** add a second therapy, crisis, memorial, guilt, final-wish,
or caregiving system. It extends `PsychologicalSanatoriumSystem`,
`PsychologicalTherapyCatalog`, `SurvivorMentalHealthSystem`, `MentalHealthCrisisSystem`,
`PsychologicalArcSystem`, `CaregivingSystem`, `FinalWishSystem`, and `MemorialSystem`
with data and additive subsystems. It routes all morale through `NeedsSystem` and all
condition state through the canonical survivor APIs. Wave 1's Faithful expansion owns
religious rites; this expansion owns secular care, therapy, and grief. Wave 2's Long
Evening owns public culture; this expansion owns private life.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Aging is not decline into uselessness.** An elder who cannot haul can
still teach, remember, mediate, and witness. The expansion changes roles, not worth.

**Pillar 2 — Comfort is treatment.** Palliative care is a legitimate medical plan,
not a failure. The expansion rewards it with peace, not points.

**Pillar 3 — Therapy is work.** Sessions cost hours, therapists get tired, progress
is uneven, and relapse is normal. There is no instant cure and no cure for
everything.

**Pillar 4 — Grief is a journey.** It has stages, anniversaries, and long tails. The
goal is not to remove it but to carry it.

**Pillar 5 — Legacy is concrete.** A story, a skill, a tool, a name, a promise. The
expansion makes inheritance a system and never a stat.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Aging | A slower walk, a kept habit, a story repeated | Comic forgetfulness |
| Palliative | A quiet room, a hand held, a dose on time | Morbid spectacle |
| Therapy | A chair, a question, a long silence | Miracle breakthrough |
| Crisis | Restraint, monitoring, and guilt | Restraint fetish |
| Grief | An anniversary nobody else remembers | Moping stereotype |
| Legacy | A tool with a name on it | Statue ceremony |

### 3.3 Content limits

- No exploitation of dementia, dying, or mental illness for horror or comedy.
- No assisted-death mechanic that is framed as a solution or a convenience.
- Therapy is never depicted as a cure-all or as a sign of weakness.
- Aging is never a punishment for the player and never a resource sink to discard.
- All content is fictional and setting-specific; no real condition is named or
  mocked.

---

## 4. THE LONG GOODBYE WORLD

### 4.1 Interior rooms

- **`room_elder_quarters`** — quieter bunks, hand rails, and familiar objects.
- **`room_hospice_ward`** — comfort care, privacy, and an attended night.
- **`room_therapy_room`** — two chairs, a window if there is one, no clock face.
- **`room_crisis_ward`** — safe monitoring with dignity and light.
- **`room_memory_garden`** — a tended indoor garden for remembering.
- **`room_legacy_store`** — belongings, tools, and letters held for survivors.
- **`room_quiet_room`** — for silence, grief, and confession.
- **`room_long_room`** — where the shelter lays out its dead before burial.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_sanatorium_ruins` | The Old Home | 5 | Pre-war care facility; equipment and records |
| `loc_pharmacy_store` | The Chemist's | 5 | Comfort medicine and narcotics |
| `loc_memory_walk` | The Long Walk | 3 | A secular memorial path |
| `loc_burial_ground` | The Ground | 4 | Burial and memorial |
| `loc_elder_farm` | The Quiet Farm | 4 | Light work for those who need it |
| `loc_letter_archive` | The Letters | 5 | Undelivered letters and recordings |
| `loc_clinic_annex` | The Annexe | 5 | Long-stay care salvage |
| `loc_quiet_overlook` | The Overlook | 4 | A place to sit with a view |
| `loc_hospice_depot` | The Comfort Depot | 5 | Bedding, medicine, and lamps |
| `loc_witness_stones` | The Stones | 4 | Named stones for the dead |

All locations require valid item references and scanner registration.

### 4.3 The care graph

Care is a graph of needs and people: who needs what, who can give it, what it costs,
and how long it lasts. The expansion authors the graph; the live systems (caregiving,
sanatorium, mental health, memorial) execute it. Nothing about care is instant, and
nothing about it is a resource conversion.

---

## 5. MAIN STORYLINE — "WHAT WE CARRY OUT"

### 5.1 Central conflict

The shelter's oldest survivor, **Grandmother Sia**, is forgetting. She remembers a
name nobody recorded — possibly a child who was left outside the door during the
first winter — and the shelter cannot tell whether the memory is true or a kind of
dream. She is also the only person who remembers how the old filtration manifolds
were assembled, and the plant is failing.

At the same time, a younger survivor with severe survivor guilt has stopped eating.
The sanatorium's one therapist is already at capacity, the crisis ward has two beds,
and the physician has three patients who will not recover and one who might, if the
comfort medicine can be found.

The expansion's conflict is not a villain. It is **finite care**. The player must
decide where the hours go: the memory, the therapy, the comfort, or the plant. The
answer changes what the shelter becomes.

### 5.2 Theme (unspoken)

**A shelter is measured by how it treats the people it can no longer use.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_elder_sia` | Grandmother Sia | Elder | Memory, dignity, and the unrecorded name |
| `npc_therapist_dun` | Dun Aal | Therapist | Sessions, limits, and self-care |
| `npc_physician_asa` | Dr. Asa Vell | Physician | Palliative plan and comfort medicine |
| `npc_caregiver_yla` | Yla Brant | Caregiver | Bonds, fatigue, and competence |
| `npc_patient_survivor_koval` | Koval | Patient | Survivor guilt and refusal to eat |
| `npc_nurse_bran` | Bran Osk | Nurse | Night watch and the quiet hours |
| `npc_legacy_tob` | Tob | Legacy keeper | Belongings, letters, and stories |
| `npc_child_witness_pim` | Pim | Child | The next generation's stake in memory |

### 5.4 Story beats (15)

1. **The Name.** Sia says a name nobody recorded.
2. **The Manifold.** The plant fails in a way only Sia remembers how to fix.
3. **The Quiet Patient.** Koval stops eating; therapy begins.
4. **The Waiting Room.** The therapist's list grows; triage of care.
5. **The Ward.** A crisis case needs the second bed.
6. **The Comfort Run.** Medicine must be found before the pain returns.
7. **The Good Day.** Sia is lucid; the shelter asks her the real question.
8. **The Bad Day.** Sia is lost in the past; a caregiver breaks.
9. **The Last Request.** A terminal survivor's wish opens.
10. **The Legacy.** Tob begins recording stories and labeling tools.
11. **The Anniversary.** A grief date arrives; the shelter handles it or does not.
12. **The Ward Choice.** Who gets care when care is finite.
13. **The Long Night.** An attended death in the hospice ward.
14. **The Reckoning.** The shelter counts what it gave and what it withheld.
15. **What We Carry Out.** Final disposition of care, therapy, and memory.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Care priority | elder / therapist / comfort / plant | finite hours |
| Sia's memory | record / test / let it rest | truth vs. comfort |
| Therapy approach | talking / work / group / medication | modality |
| Crisis ward | expand / rotate / restrain | dignity vs. capacity |
| Terminal care | full comfort / limited / trial cure | peace vs. hope |
| Legacy | record / distribute / keep private | memory shape |
| Grief support | formal / informal / none | community |
| Final | care as right / care as resource / care as gift | identity |

### 5.6 Endings (5 + fade)

1. **The Attended Death** — the shelter learns to care well; its culture changes.
2. **The Recorded Name** — Sia's memory is preserved, true or not, as the shelter chooses.
3. **The Empty Ward** — care capacity is never expanded; people die alone.
4. **The Garden** — the memory garden becomes an institution; grief has a place.
5. **The Long Night Kept** — a hard death is done well; the shelter is changed by it.
6. **Fade** — the question is deferred; the care list grows.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_goodbye_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_goodbye_the_name`, `quest_goodbye_manifold`, `quest_goodbye_quiet_patient`,
`quest_goodbye_waiting_room`, `quest_goodbye_the_ward`, `quest_goodbye_comfort_run`,
`quest_goodbye_good_day`, `quest_goodbye_bad_day`, `quest_goodbye_last_request`,
`quest_goodbye_legacy`, `quest_goodbye_anniversary`, `quest_goodbye_ward_choice`,
`quest_goodbye_long_night`, `quest_goodbye_reckoning`, `quest_goodbye_what_we_carry`.

### 6.2 Side quests (30)

**Aging (5)**
- `quest_goodbye_hand_rails` — make rooms safer
- `quest_goodbye_light_work` — adapt a role
- `quest_goodbye_memory_book` — record recollections
- `quest_goodbye_walking_aid` — build a frame or cane
- `quest_goodbye_familiar_object` — reunite a survivor with a keepsake

**Caregiving (5)**
- `quest_goodbye_care_roster` — build a rotation
- `quest_goodbye_caregiver_burnout` — a caregiver is failing
- `quest_goodbye_night_shift` — cover an attended night
- `quest_goodbye_bedding` — comfort bedding and warmth
- `quest_goodbye_bond` — a bond forms or breaks

**Therapy (5)**
- `quest_goodbye_first_session` — begin treatment
- `quest_goodbye_setback` — a relapse
- `quest_goodbye_group_session` — a shared room
- `quest_goodbye_work_therapy` — purposeful labor
- `quest_goodbye_medication_trial` — a careful trial

**Crisis (5)**
- `quest_goodbye_second_bed` — expand the ward
- `quest_goodbye_restraint_rule` — write the restraint policy
- `quest_goodbye_night_watch` — monitor a crisis
- `quest_goodbye_crisis_discharge` — plan a safe return
- `quest_goodbye_quiet_room` — a place to be alone safely

**Grief and legacy (5)**
- `quest_goodbye_memorial_stone` — name a stone
- `quest_goodbye_anniversary_meal` — mark a date
- `quest_goodbye_letter_kept` — hold a letter
- `quest_goodbye_tool_passed` — pass a tool to a new hand
- `quest_goodbye_story_recorded` — record a voice

**Palliative (5)**
- `quest_goodbye_pain_plan` — set the comfort plan
- `quest_goodbye_comfort_kit` — assemble a kit
- `quest_goodbye_prognosis_talk` — the conversation
- `quest_goodbye_last_meal` — a requested meal
- `quest_goodbye_presence` — sit with someone

### 6.3 Repeatable quests (8)

`quest_goodbye_repeat_session`, `quest_goodbye_repeat_care`,
`quest_goodbye_repeat_comfort`, `quest_goodbye_repeat_watch`,
`quest_goodbye_repeat_record`, `quest_goodbye_repeat_garden`,
`quest_goodbye_repeat_visit`, `quest_goodbye_repeat_memorial`.

### 6.4 Dynamic hooks

Live systems emit final-wish, caregiving, crisis, arc, guilt, and memorial events. The
generator attaches authored follow-ups without a new event bus.

### 6.5 Constraints

- No death may grant resources; legacy is memory, not loot.
- No therapy may cure a permanent condition; only progress and coping.
- No crisis may be resolved by ignoring it; acuity escalates.
- No palliative plan may be forced; the patient's preference matters.
- No content may exploit dementia, dying, or mental illness.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `AgingSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** authored aging milestones, frailty, cognitive change, and role adaptation.
**Consumes:** `SurvivorEntityStore`, `NeedsSystem`, `SkillProgressionSystem`,
`DutyRoster`. **Data:** `aging_profiles.json`, `cognitive_decline.json`.
**Rules:** aging is slow and individual; frailty reduces physical capacity and never
intelligence or worth; roles adapt rather than terminate; aging interacts with dose
and trauma history.

### 7.2 `PalliativeCareSystem` (new, `Ashfall.Core.Medical`)

**Owns:** comfort plans, pain management, prognosis communication, and attended care.
**Consumes:** `FinalWishSystem`, `MedicalPipelineCoordinator`, `Inventory`,
`NeedsSystem`. **Data:** `palliative_protocols.json`, `comfort_items.json`.
**Rules:** comfort is a legitimate plan; pain control consumes real medicine; a
patient's preference is recorded and honored where possible; the final-wish system
remains the terminal questline authority.

### 7.3 `TherapyProgramSystem` (new, `Ashfall.Core.Sanatorium`)

**Owns:** modalities, session scheduling, therapist skill use, progress, setbacks,
and relapse. **Consumes:** `PsychologicalSanatoriumSystem`, `PsychologicalTherapyCatalog`,
`SurvivorMentalHealthSystem`, `SkillProgressionSystem`. **Data:**
`therapy_modalities.json`.
**Rules:** each modality suits some conditions and not others; progress is uneven;
the therapist tires; the sanatorium remains the treatment state owner.

### 7.4 `CrisisWardSystem` (extend `MentalHealthCrisisSystem`)

**Owns:** ward capacity, monitoring, intervention, restraint policy, and discharge.
**Consumes:** `MentalHealthCrisisSystem`, `NeedsSystem`, `MedicalPipelineCoordinator`.
**Data:** `crisis_protocols.json`.
**Rules:** capacity is finite; restraint is a policy with a guilt and trust cost;
discharge planning reduces relapse; the ward never becomes a prison.

### 7.5 `GriefJourneySystem` (new, thin, `Ashfall.Core.Memorial`)

**Owns:** grief stages, anniversaries, long-tail echoes, and communal mourning.
**Consumes:** `MemorialSystem`, `GuiltInsomniaSystem`, `NeedsSystem`. **Data:**
`grief_stages.json`.
**Rules:** grief is a journey, not a debuff; stages advance with time and support;
anniversaries produce authored events; the memorial remains the rite authority.

### 7.6 `LegacySystem` (new, `Ashfall.Core.Survivors`)

**Owns:** stories, skills, tools, letters, and promises handed on; and their delivery
to heirs and the next generation. **Consumes:** `FinalWishSystem`,
`GenerationalSuccessionEngine` (Wave 1), `MemorialSystem`, `Inventory`. **Data:**
`legacy_tokens.json`.
**Rules:** legacy is concrete and durable; a tool keeps its association; a recorded
voice can be replayed; no legacy grants combat power.

### 7.7 `ElderRoleSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** adapted roles for elders: teaching, mediation, records, gardening, child
care, and witness. **Consumes:** `AgingSystem`, `DutyRoster`, `SkillProgressionSystem`.
**Data:** `elder_roles.json`.
**Rules:** adapted roles produce real value; they are chosen with the survivor, not
assigned; the system never treats an elder as idle.

### 7.8 Systems explicitly not added

- No second therapy, crisis, memorial, guilt, or final-wish system.
- No assisted-death minigame; the terminal arc is authored and respectful.
- No death resource loop.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `mental_arcs.json` (extend 4 → 30)

Existing schema preserved (`id`, `display_name`, `stress_threshold`,
`minimum_stress_days`, `behavior`, `behavior_chance`, `behavior_cooldown_days`,
`crisis_behavior_min_stage`, `treatment_tags`, `relapse_cooldown_days`, `tags`).
New arcs include hoarding, fire fixation, persecutory crisis, shutdown withdrawal,
plus: compulsive checking, ration guarding, night waking, noise intolerance, rage
outbursts, self-neglect, ritual rigidity, phone-like listening, hand washing
compulsion, counting fixation, escape planning, attachment to objects, refusal to
sleep in a bed, hoarding of letters, verbal aggression, isolation seeking, food
rituals, door checking, and grief paranoia.

### 8.2 `psychological_therapies.json` (extend)

New conditions with canonical surfaces and reversibility: prolonged grief, moral
injury, caregiver fatigue, survivor guilt variants, isolation paranoia, sensory
grief, numbness, hyperarousal, dissociative episodes, and somatic pain.

### 8.3 `psychological_trauma.json` (extend)

Trauma definitions for new origins: caregiving, terminal care, child loss, rescue
failure, execution duty, and quiet attrition.

### 8.4 `guilt_sources.json` (extend)

New guilt sources for care decisions: withheld medicine, missed night watch,
restraint used, ward refused, last words unsaid.

### 8.5 `final_wishes.json` (extend)

New archetypes and wishes across elder, caregiver, therapist, and child roles. The
existing schema (`id`, `archetype_id`, `wish_type`, `wish_title`, `wish_description`,
`steps[]`, `completion_text`) is preserved.

### 8.6 `aging_profiles.json` (new)

```json
{
  "schema_version": 1,
  "profiles": [
    {
      "profile_id": "aging_steady",
      "display_name": "Steady Decline",
      "onset_year_min": 8,
      "frailty_rate_per_year": 0.08,
      "cognitive_rate_per_year": 0.03,
      "adaptation_bonus": 0.15,
      "role_transitions": ["teacher", "records", "witness"],
      "tags": ["elder", "steady", "valued"]
    }
  ]
}
```

### 8.7 `cognitive_decline.json` (new)

Decline rows: stage, memory effect, orientation effect, care need, safety risk, and
lucid-window rules.

### 8.8 `palliative_protocols.json` (new)

Protocol rows: comfort level, medicine, pain target, monitoring, family presence, and
prognosis communication.

### 8.9 `comfort_items.json` (new)

Comfort rows: bedding, lamp, warmth, familiar object, food, music, and their effects.

### 8.10 `therapy_modalities.json` (new)

Modality rows: name, suitable surfaces, session cost, therapist skill, progress
curve, setback chance, and relapse profile.

### 8.11 `crisis_protocols.json` (new)

Protocol rows: acuity, monitoring, intervention, restraint, discharge conditions, and
guilt/trust costs.

### 8.12 `grief_stages.json` (new)

Stage rows: name, duration, support need, anniversary triggers, and long-tail effects.

### 8.13 `legacy_tokens.json` (new)

Legacy rows: story, skill transfer, tool, letter, promise, and delivery condition.

### 8.14 `elder_roles.json` (new)

Role rows: name, physical demand, value, skill, and adaptation requirements.

### 8.15 Items

New items appended to `items.json`: `item_comfort_blanket`, `item_pain_draught`,
`item_care_log`, `item_memory_book`, `item_walking_frame`, `item_hearing_horn`,
`item_reading_lamp`, `item_legacy_tool`, `item_sealed_letter`, `item_voice_record`,
`item_memorial_stone`, `item_grief_candle`, `item_companion_chair`,
`item_quiet_clock`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing stores: `CaregivingSaveStore`, `PsychologicalSanatoriumSaveStore`,
`SurvivorMentalHealthSaveStore`, `MentalHealthCrisisHostSession`,
`PsychologyArcSaveStore`, `MemorialSaveStore`, `DoseLedgerSaveStore` (final wish).
New sub-objects are additive inside these envelopes. No new save section.

### 9.2 State to persist

- Aging milestones and frailty.
- Cognitive stage and lucid windows.
- Palliative plan, comfort level, and prognosis.
- Therapy modality, progress, setback, relapse.
- Crisis ward occupancy, restraint policy, and discharge plans.
- Grief stage and anniversaries.
- Legacy tokens and delivery.
- Elder role assignments.

### 9.3 Determinism

- Aging and cognitive rates are pure functions of time and authored profiles.
- Therapy progress and setback use the host-forked `ISeededRng`.
- Crisis escalation is deterministic given care and time.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no aging, no palliative plan, no therapy modality, no grief
stage, and no legacy tokens. Existing final-wish, caregiving, crisis, and memorial
state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for progress and risk.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CaregivingPanel` (extend) | Assignments, bonds, fatigue, roster | `LongGoodbyeHostSession` |
| `ElderPanel` (new) | Frailty, roles, adaptations, needs | same |
| `PalliativePanel` (new) | Comfort plan, pain, prognosis, presence | same |
| `TherapyPanel` (new) | Modalities, sessions, progress, relapse | same |
| `MentalHealthCrisisPanel` (extend) | Ward, acuity, intervention, discharge | same |
| `PsychologyArcPanel` (extend) | Arcs, behaviors, treatment | same |
| `GriefPanel` (new) | Stages, anniversaries, support | same |
| `LegacyPanel` (new) | Stories, tools, letters, delivery | same |
| `MemorialPanel` (extend) | Rites and long-tail echoes | `MemorialSystem` host |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Care status is always visible; a neglected patient is never hidden.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Restraint and end-of-life decisions require explicit confirmation with stated
  costs.
- Language is dignified; no euphemism-by-default and no clinical coldness.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a quiet room tone, a chair shifting, a pen,
a night lamp, a held breath, a remembered song. No cue is required; text carries
meaning. The expansion deliberately avoids sad piano.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `FinalWishSystem` | Terminal questline consumed by palliative care |
| `CaregivingSystem` | Skill, rotation, burnout extended |
| `PsychologicalSanatoriumSystem` | Modality and session program extended |
| `PsychologicalTherapyCatalog` | Conditions extended |
| `SurvivorMentalHealthSystem` | Canonical mental-health state |
| `MentalHealthCrisisSystem` | Ward and protocol extended |
| `PsychologicalArcSystem` | Arcs extended |
| `MemorialSystem` | Rites and long-tail grief |
| `GuiltInsomniaSystem` | Care-decision guilt |
| `ConfessionSecretSystem` | Quiet-room content |
| `NeedsSystem` | Morale and appetite |
| `SkillProgressionSystem` | Therapist and elder-role skills |
| `DutyRoster` | Care rosters and adapted roles |
| `MedicalPipelineCoordinator` | Palliative medicine and crises |
| `GenerationalSuccessionEngine` | Legacy delivery to heirs |
| `Inventory` | Comfort items and legacy tools |
| `EpilogueChronicleBuilder` | Care and legacy milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `FinalWishSystem`, `CaregivingSystem`,
`PsychologicalSanatoriumSystem`, `PsychologicalTherapyCatalog`,
`SurvivorMentalHealthSystem`, `MentalHealthCrisisSystem`, `PsychologicalArcSystem`,
`MemorialSystem`, save stores, and panels. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend arc, therapy, trauma, guilt, and final-wish
catalogs; author aging, cognitive decline, palliative, comfort, modalities, crisis
protocols, grief stages, legacy tokens, elder roles. Register validators and scanner.

**Phase 2 — Pure Core.** `AgingSystem`, `PalliativeCareSystem`,
`TherapyProgramSystem`, `CrisisWardSystem`, `GriefJourneySystem`, `LegacySystem`,
`ElderRoleSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `LongGoodbyeHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak including aging, care load, and grief cycles.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Mental arcs | 26 new (4 → 30) |
| Therapy conditions | 20 new |
| Trauma definitions | 15 new |
| Guilt sources | 30 new |
| Final wishes | 12 new archetypes |
| Aging profiles | 10 |
| Cognitive stages | 8 |
| Palliative protocols | 10 |
| Comfort items | 15 |
| Therapy modalities | 10 |
| Crisis protocols | 8 |
| Grief stages | 6 |
| Legacy tokens | 20 |
| Elder roles | 10 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second mental-health system | Critical | Extend live owners |
| Exploitative tone | Critical | Dignity review gate |
| Death becomes a resource | Critical | No death rewards |
| Therapy trivialized | High | Uneven progress and relapse |
| Aging becomes a debuff | High | Role adaptation, not removal |
| Restraint ethics mishandled | High | Explicit policy, guilt, trust |
| Determinism break | Low | Host-forked RNG |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `mental_arcs.json` | +26 | 6,000 |
| `psychological_therapies.json` | +20 | 4,000 |
| `psychological_trauma.json` | +15 | 3,500 |
| `guilt_sources.json` | +30 | 5,000 |
| `final_wishes.json` | +12 | 6,000 |
| `aging_profiles.json` | 10 | 2,500 |
| `cognitive_decline.json` | 8 | 2,000 |
| `palliative_protocols.json` | 10 | 2,500 |
| `comfort_items.json` | 15 | 2,500 |
| `therapy_modalities.json` | 10 | 3,000 |
| `crisis_protocols.json` | 8 | 2,500 |
| `grief_stages.json` | 6 | 2,000 |
| `legacy_tokens.json` | 20 | 4,000 |
| `elder_roles.json` | 10 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~78,000** |trim to the 60–75k target during authoring.

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R24-1 | Second mental-health system | Low | Critical | Extend owners |
| R24-2 | Exploitative tone | Med | Critical | Dignity review |
| R24-3 | Death resource loop | Low | Critical | No rewards |
| R24-4 | Therapy trivial | Med | High | Uneven progress |
| R24-5 | Aging as debuff | Med | High | Role adaptation |
| R24-6 | Restraint mishandled | Med | High | Explicit ethics |
| R24-7 | Determinism | Low | High | Host-forked RNG |
| R24-8 | Content overrun | Med | Med | Budget §13 |
| R24-9 | Grief too heavy | Med | Med | Balanced prose |
| R24-10 | Care load opaque | Med | Med | Explicit roster UI |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can therapy fully restore a survivor?** Recommended: no permanent condition is
   cured; coping and function improve.
2. **Can the player refuse palliative care?** Recommended: yes; the patient's and
   the shelter's preferences both matter.
3. **Is restraint ever necessary?** Recommended: yes, as a last resort with guilt and
   trust costs, never as routine.
4. **Does aging ever reduce a survivor's skills?** Recommended: physical capacity
   only; knowledge and judgment are preserved.
5. **Can legacy tokens be sold?** Recommended: they can be given away, never sold for
   value; selling a named tool is a social event.

---

## 17. APPENDIX D — AGING PROFILE TABLE (10 PROFILES)

| # | Profile | Onset year | Frailty/yr | Cognitive/yr | Adaptation | Roles |
|---|---|---|---|---|---|---|
| 1 | Steady Decline | 8 | 0.08 | 0.03 | high | teacher, records |
| 2 | Slow Decline | 10 | 0.05 | 0.02 | high | witness, garden |
| 3 | Rapid Frailty | 6 | 0.14 | 0.04 | med | records, witness |
| 4 | Early Memory | 6 | 0.07 | 0.09 | low | witness, garden |
| 5 | Late Decline | 12 | 0.04 | 0.02 | high | teacher, mediator |
| 6 | Stubborn Frame | 9 | 0.06 | 0.03 | high | light work |
| 7 | Rad-Worn | 4 | 0.12 | 0.06 | med | witness |
| 8 | Fit Elder | 11 | 0.04 | 0.03 | high | light work, garden |
| 9 | Caregiver-Worn | 7 | 0.10 | 0.04 | med | records |
| 10 | Child of Ash | 9 | 0.07 | 0.05 | high | teacher, witness |

Aging is authored per survivor and never uniform. A survivor with a rad-worn body at
year four and a fit elder at year eleven are both real, and the shelter adapts
differently to each.

---

## 18. APPENDIX E — COGNITIVE DECLINE TABLE (8 STAGES)

| # | Stage | Memory | Orientation | Care need | Safety risk | Lucid windows |
|---|---|---|---|---|---|---|
| 1 | Sharp | full | full | none | none | n/a |
| 2 | Forgetting | names, dates | full | low | none | n/a |
| 3 | Repeating | recent events | mostly | low | low | frequent |
| 4 | Mixing eras | present/past | some | med | low | occasional |
| 5 | Wandering | routes | poor | med | med | occasional |
| 6 | Confusion | people | poor | high | high | rare |
| 7 | Frightened | place | very poor | high | high | rare |
| 8 | Quiet | full | none | total | total | none |

Cognitive decline never reduces the person's worth, and lucid windows are real,
authored, and precious. The system records what a survivor says in a lucid window
verbatim, because those words are the expansion's most valuable resource.

---

## 19. APPENDIX F — PALLIATIVE PROTOCOL TABLE (10 PROTOCOLS)

| # | Protocol | Comfort | Medicine | Pain target | Monitoring | Presence |
|---|---|---|---|---|---|---|
| 1 | Watchful | low | none | tolerable | daily | optional |
| 2 | Comfort Basic | med | draught | mild | daily | family |
| 3 | Comfort Full | high | draught + sedative | none | continuous | family |
| 4 | Terminal Calm | high | sedative | none | continuous | family + staff |
| 5 | Trial Cure | low | experimental | n/a | continuous | staff |
| 6 | Home Ward | med | draught | mild | daily | family |
| 7 | Garden Care | med | draught | mild | daily | garden |
| 8 | Night Attended | high | sedative | none | night staff | one person |
| 9 | Pain Crisis | high | strong | urgent | continuous | staff |
| 10 | Last Wish | med | draught | mild | daily | wish team |

Comfort consumes real medicine and staff hours. A shelter that cannot afford full
comfort is a shelter making a hard choice, and the expansion never pretends that
choice is easy or wrong.

---

## 20. APPENDIX G — COMFORT ITEM TABLE (15 ITEMS)

| # | Item | Effect | Cost | Source |
|---|---|---|---|---|
| 1 | Warm Blanket | warmth, calm | cloth | salvage, craft |
| 2 | Soft Pillow | rest | cloth | craft |
| 3 | Reading Lamp | orientation | power | salvage |
| 4 | Familiar Cup | grounding | none | keepsake |
| 5 | Photograph | memory anchor | none | keepsake |
| 6 | Recorded Voice | calm | power | record |
| 7 | Heated Stone | pain relief | fuel | simple |
| 8 | Herbal Tea | calm | herb | garden |
| 9 | Soft Food | appetite | food | kitchen |
| 10 | Window View | orientation | none | room |
| 11 | Companion Chair | presence | wood | craft |
| 12 | Quiet Clock | orientation | none | craft |
| 13 | Scented Cloth | memory | herb | garden |
| 14 | Letter Kept | meaning | none | archive |
| 15 | Hand Held | meaning | none | presence |

Comfort items are not medicine and do not cure. They are how a shelter makes a room
livable, and their absence is felt.

---

## 21. APPENDIX H — THERAPY MODALITY TABLE (10 MODALITIES)

| # | Modality | Suits | Session cost | Therapist | Progress | Setback |
|---|---|---|---|---|---|---|
| 1 | Talking | grief, guilt | 1 h | 2 | steady | 0.10 |
| 2 | Work Therapy | shutdown, isolation | 3 h | 1 | steady | 0.08 |
| 3 | Group Session | isolation, grief | 1 h each | 2 | shared | 0.12 |
| 4 | Medication | startle, paranoia | med supply | 3 | assisted | 0.05 |
| 5 | Exposure Care | flashback | 1 h | 3 | uneven | 0.20 |
| 6 | Memory Work | grief, decline | 1 h | 2 | gentle | 0.06 |
| 7 | Body Work | tension, pain | 1 h | 2 | steady | 0.07 |
| 8 | Routine Restore | arcs | ongoing | 1 | slow | 0.09 |
| 9 | Witness | moral injury | 1 h | 2 | slow | 0.04 |
| 10 | Quiet Presence | crisis, terminal | 1 h | 1 | soft | 0.03 |

No modality is universal. The therapist's skill, the condition's canonical surface,
and the survivor's willingness all matter, and progress is never linear.

---

## 22. APPENDIX I — THERAPY CONDITION TABLE (20 CONDITIONS)

| # | Condition | Surface | Reversible | Best modality |
|---|---|---|---|---|
| 1 | Combat startle | hypervigilance | no | exposure care |
| 2 | Flash blindness | flashback | yes | exposure care |
| 3 | Severe survivor guilt | guilt insomnia | no | witness |
| 4 | Siege paranoia | paranoia | no | medication |
| 5 | Prolonged grief | grief | no | memory work |
| 6 | Moral injury | guilt | no | witness |
| 7 | Caregiver fatigue | exhaustion | yes | routine restore |
| 8 | Isolation paranoia | paranoia | maybe | group session |
| 9 | Numbness | shutdown | maybe | work therapy |
| 10 | Hyperarousal | hypervigilance | maybe | body work |
| 11 | Dissociative episodes | flashback | maybe | quiet presence |
| 12 | Somatic pain | pain | maybe | body work |
| 13 | Food rituals | arc | maybe | routine restore |
| 14 | Hoarding compulsion | arc | maybe | talking |
| 15 | Night waking | sleep | yes | routine restore |
| 16 | Rage outbursts | conflict | maybe | talking |
| 17 | Self-neglect | shutdown | yes | work therapy |
| 18 | Grief paranoia | paranoia | maybe | group session |
| 19 | Terminal fear | dread | no | quiet presence |
| 20 | Memory grief | grief | no | memory work |

Reversibility is authored and honest. Some conditions improve; some are carried for
life, and the therapy's job is to make carrying possible.

---

## 23. APPENDIX J — MENTAL ARC TABLE (30 ARCS)

| # | Arc | Threshold | Behavior | Treatment |
|---|---|---|---|---|
| 1 | Compulsive Stashing *(LIVE)* | 90 | stash transfer | routine |
| 2 | Fire Fixation *(LIVE)* | 92 | unsafe fire | cognitive |
| 3 | Persecutory Crisis *(LIVE)* | 90 | refuse assignment | cognitive, routine |
| 4 | Shutdown Withdrawal *(LIVE)* | 95 | withdraw self-care | routine, cognitive |
| 5 | Compulsive Checking | 88 | door checks | routine |
| 6 | Ration Guarding | 89 | hide food | routine |
| 7 | Night Waking | 86 | wander at night | routine |
| 8 | Noise Intolerance | 90 | avoid mess | quiet presence |
| 9 | Rage Outbursts | 92 | shout | talking |
| 10 | Self-Neglect | 94 | skip meals | work therapy |
| 11 | Ritual Rigidity | 87 | fixed order | routine |
| 12 | Listening Compulsion | 91 | listen at walls | cognitive |
| 13 | Hand Washing | 88 | scrub raw | routine |
| 14 | Counting Fixation | 87 | count supplies | routine |
| 15 | Escape Planning | 93 | plan leaving | talking |
| 16 | Object Attachment | 85 | guard keepsake | memory work |
| 17 | Refuse Bed | 90 | sleep on floor | routine |
| 18 | Letter Hoarding | 86 | keep mail | memory work |
| 19 | Verbal Aggression | 91 | insult | talking |
| 20 | Isolation Seeking | 89 | avoid mess | group |
| 21 | Food Rituals | 88 | arrange food | routine |
| 22 | Grief Paranoia | 92 | suspect theft | group |
| 23 | Rescue Guilt | 93 | blame self | witness |
| 24 | Caregiver Guilt | 91 | overwork | routine |
| 25 | Death Watch | 94 | sit with dying | quiet presence |
| 26 | Terminal Rage | 95 | refuse care | quiet presence |
| 27 | Music Compulsion | 85 | hum constantly | work |
| 28 | Tool Hugging | 86 | carry tool | work |
| 29 | Silence Vow | 93 | refuse speech | quiet presence |
| 30 | Memory Repeating | 90 | retell story | memory work |

Each arc has a threshold, a behavior, a cooldown, a crisis stage, treatment tags, and
a relapse window. The expansion triples the arc space without changing the system.

---

## 24. APPENDIX K — CRISIS PROTOCOL TABLE (8 PROTOCOLS)

| # | Protocol | Monitoring | Intervention | Restraint | Discharge | Guilt |
|---|---|---|---|---|---|---|
| 1 | Watch | hourly | reassurance | no | when calm | none |
| 2 | Ward Basic | 4 h | medication | no | plan | low |
| 3 | Ward Close | 1 h | medication, therapy | no | plan | low |
| 4 | Night Watch | continuous | presence | no | morning | low |
| 5 | Safe Room | continuous | quiet, light | soft | plan | med |
| 6 | Restraint Short | continuous | physical | yes, hours | review | high |
| 7 | Restraint Long | continuous | physical | yes, days | review | very high |
| 8 | Transfer | continuous | escort | as needed | review | med |

Restraint is available because some crises are dangerous, and it always costs guilt
and trust. The expansion's rule is that restraint must be logged, reviewed, and
released as soon as it is safe, and the player sees the cost.

---

## 25. APPENDIX L — GRIEF STAGE TABLE (6 STAGES)

| # | Stage | Duration | Support need | Anniversary | Effect |
|---|---|---|---|---|---|
| 1 | Acute | 3–7 days | high | yes | morale, sleep |
| 2 | Empty Shift | 7–14 days | med | yes | work drop |
| 3 | Return of Ordinary | 14–30 days | low | no | recovery |
| 4 | Memorial Observance | 1 day | high | yes | communal |
| 5 | Long-Tail Echo | months | low | yes | memory |
| 6 | Carried | permanent | low | yes | identity |

Grief is not removed; it is carried. The final stage is not a debuff but a permanent,
small part of who a survivor is, and the memorial panel shows it with dignity.

---

## 26. APPENDIX M — LEGACY TOKEN TABLE (20 TOKENS)

| # | Token | Kind | Delivery | Effect |
|---|---|---|---|---|
| 1 | Worn Wrench | tool | heir | craft familiarity |
| 2 | Sealed Letter | letter | named person | morale |
| 3 | Recorded Voice | voice | family | grief relief |
| 4 | Recipe Card | skill | kitchen | recipe unlock |
| 5 | Field Manual | skill | student | skill head start |
| 6 | Photograph | memory | family | memory anchor |
| 7 | Carved Spoon | keepsake | friend | comfort |
| 8 | Watch | keepsake | heir | orientation |
| 9 | Map Notes | knowledge | scouts | route hint |
| 10 | Seed Packet | practical | growers | crop line |
| 11 | Promise | obligation | faction | standing |
| 12 | Debt Note | obligation | heir | debt |
| 13 | Story Recording | memory | archive | chronicle |
| 14 | Medal | keepsake | museum | display |
| 15 | Child's Toy | keepsake | next child | morale |
| 16 | Last Words | memory | witness | guilt relief |
| 17 | Tool Roll | tool | apprentice | craft bonus |
| 18 | Ledger Page | knowledge | clerk | records |
| 19 | Garden Stone | memory | garden | memorial |
| 20 | Name List | memory | archivist | canon |

Legacy is concrete and durable. A named tool keeps its association; a recorded voice
can be replayed; a promise can be honored or broken. None of it grants combat power.

---

## 27. APPENDIX N — ELDER ROLE TABLE (10 ROLES)

| # | Role | Physical | Value | Skill | Adaptation |
|---|---|---|---|---|---|
| 1 | Teacher | low | high | teaching | seat, board |
| 2 | Mediator | low | high | rhetoric | quiet room |
| 3 | Records | low | med | literacy | lamp, seat |
| 4 | Garden Keeper | low | med | agriculture | raised bed |
| 5 | Child Care | low | high | caregiving | seat, toys |
| 6 | Witness | none | high | memory | any |
| 7 | Tool Keeper | low | med | craft | bench, seat |
| 8 | Storyteller | none | high | culture | common hall |
| 9 | Advisor | none | high | leadership | any |
| 10 | Quiet Watch | low | med | vigilance | post seat |

Adapted roles produce real value. The expansion's promise is that a shelter which
adapts its rooms and duties keeps its elders, and a shelter that does not loses them
to beds and boredom long before biology finishes the job.

---

## 28. APPENDIX O — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_goodbye_the_name` | 3 | Sia says a name; decide whether to record it |
| `quest_goodbye_manifold` | 5 | Consult Sia; repair the plant from her memory |
| `quest_goodbye_quiet_patient` | 4 | Koval stops eating; begin therapy |
| `quest_goodbye_waiting_room` | 4 | Triage the therapist's list |
| `quest_goodbye_the_ward` | 4 | A crisis case needs the second bed |
| `quest_goodbye_comfort_run` | 5 | Find comfort medicine before the pain returns |
| `quest_goodbye_good_day` | 3 | A lucid window; ask the real question |
| `quest_goodbye_bad_day` | 4 | A caregiver breaks; rebalance care |
| `quest_goodbye_last_request` | 5 | A terminal wish opens; fulfill or miss it |
| `quest_goodbye_legacy` | 4 | Record stories; label tools; write letters |
| `quest_goodbye_anniversary` | 4 | A grief date arrives; mark it or not |
| `quest_goodbye_ward_choice` | 5 | Who gets care when care is finite |
| `quest_goodbye_long_night` | 5 | An attended death; do it well or poorly |
| `quest_goodbye_reckoning` | 4 | Count what was given and withheld |
| `quest_goodbye_what_we_carry` | 3 | Final disposition; epilogue |

---

## 29. APPENDIX P — NPC DOSSIERS (BRIEF)

**Grandmother Sia** — elder. Remembers a name nobody recorded and the precise torque
of a pipe flange installed forty years ago. Her decline is the expansion's center,
and her lucid windows are the most valuable content in it. She is not a plot device;
she is a person with opinions about her own care.

**Dun Aal** — therapist. Runs sessions, keeps notes, and is one patient away from
burnout. Believes progress is real and slow. Represents the expansion's refusal to
promise cures.

**Dr. Asa Vell** — physician. Runs the palliative plan with clinical honesty and
personal cost. Will tell a patient the truth, and will sit with them after.

**Yla Brant** — caregiver. Good at care and bad at rest. Her arc is burnout and the
awkward, necessary act of accepting help.

**Koval** — patient. Survivor guilt has stopped his appetite. His recovery, if it
comes, is uneven and written honestly.

**Bran Osk** — nurse. Owns the night watch and the quiet hours, where most of the
real care happens.

**Tob** — legacy keeper. Records voices, labels tools, and files letters. Believes
memory is infrastructure and treats the archive accordingly.

**Pim** — child. The reason legacy matters: someone has to receive it.

---

## 30. APPENDIX Q — LOCATION DETAIL

- **The Old Home** — a ruined care facility; bed frames, hoists, and records.
- **The Chemist's** — comfort medicine behind a counter that still locks.
- **The Long Walk** — a secular memorial path with named stones and no sermon.
- **The Ground** — burial; markers, a spade, and a list.
- **The Quiet Farm** — light work for those who need to be useful outdoors.
- **The Letters** — undelivered mail and recorded voices, catalogued by Tob.
- **The Annexe** — long-stay care salvage; screens, rails, and bedding.
- **The Overlook** — a bench with a view; where survivors go to sit with someone.
- **The Comfort Depot** — bedding, lamps, tea, and medicine; accumulated slowly.
- **The Stones** — named stones; the shelter's memory made physical.

---

## 31. APPENDIX R — CARE LOAD WORKED MODEL

| Care need | Staff hours/week | Materials | Bond effect | Burnout rate |
|---|---|---|---|---|
| Elder light | 4 | low | + | low |
| Elder heavy | 12 | med | ++ | med |
| Cognitive care | 20 | med | +++ | high |
| Palliative basic | 15 | med | +++ | med |
| Palliative full | 30 | high | ++++ | high |
| Therapy session | 6 | low | ++ | med |
| Crisis watch | 40 | low | + | very high |
| Night watch | 28 | low | ++ | high |
| Grief support | 6 | low | ++ | low |

The numbers are authored so that a shelter cannot care for everyone at full intensity
without giving up something else. That constraint is the expansion's entire point:
finite care is a real, visible, and honorable problem.

---

## 32. APPENDIX S — WORKED 360-DAY CARE SCENARIO

**Days 1–30.** Sia's decline is noticed. Her name story is recorded or dismissed.
The manifold fails and her memory repairs it. Koval's appetite loss is diagnosed as
guilt-linked.

**Days 31–90.** The therapist's list grows to five. The second crisis bed is filled.
A caregiver shows fatigue signs. The comfort plan for a terminal survivor is written
and medicine is sourced.

**Days 91–150.** A bad day breaks a caregiver; the roster rotates. Sia's lucid window
produces a second name. A legacy program begins: voices, tools, letters.

**Days 151–220.** A crisis discharge is planned and fails once, then succeeds. The
memory garden is built. Grief anniversaries are marked for the first time.

**Days 221–300.** A long night occurs: an attended death in the hospice ward. The
shelter learns what presence means. The first legacy token is passed to an heir.

**Days 301–360.** The shelter writes its care charter: who gets hours, who gets
medicine, and who sits with whom. The epiclogue records what was carried out.

---

## 33. APPENDIX T — VIGNETTE (TONE SAMPLE)

> Sia is having a good day, which means she knows the room and the year and the name
> of the person sitting beside her. Dun does not waste it. He asks about the child
> left outside the door, and Sia tells the story the same way she told it the first
> time, and then adds one detail she has never added before, and then stops.
>
> In the next room, Yla is asleep in a chair beside a bed with her hand on the
> blanket, and Bran lets her sleep, because the night was long and the morning is
> someone else's shift.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Caregiver burnout | care quality drops | rotate, rest, reduce load |
| Medicine shortage | pain returns | source, substitute, comfort only |
| Crisis escalation | safety risk | ward, watch, restraint policy |
| Ward full | crisis elsewhere | expand, transfer, prioritize |
| Therapy stall | morale drop | change modality, rest |
| Relapse | progress loss | resume, adjust, support |
| Grief unmarked | long-tail anger | anniversary rite, support |
| Legacy lost | story gone | interview others, archive |
| Unattended death | guilt, trust loss | review, support, policy |
| Elder role lost | meaning loss | adapt role, training |

No failure is a game over. Every failure has a recovery path, and every recovery
costs hours, medicine, or trust. The deepest failure is a shelter that stops trying
because the care list never gets shorter.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] `PsychologicalSanatoriumSystem` remains the treatment authority.
- [ ] `FinalWishSystem` remains the terminal wish authority.
- [ ] `MemorialSystem` and `GuiltInsomniaSystem` remain grief/guilt owners.
- [ ] No death grants resources.
- [ ] No therapy cures a permanent condition.
- [ ] Aging never removes knowledge or worth.
- [ ] Restraint is logged, reviewed, and costly.
- [ ] Palliative medicine is real and finite.
- [ ] Language is dignified, never clinical-cold or exploitative.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 36. APPENDIX W — GLOSSARY

- **Aging profile** — an authored decline curve for a survivor.
- **Cognitive stage** — a memory/orientation stage with care needs.
- **Lucid window** — a real, authored period of clarity.
- **Palliative plan** — comfort level, medicine, and presence.
- **Modality** — a therapy approach suited to certain conditions.
- **Crisis protocol** — monitoring, intervention, restraint, and discharge.
- **Grief stage** — a journey phase with support needs.
- **Legacy token** — a concrete thing handed on.
- **Elder role** — an adapted role for physical limits.
- **Care charter** — the shelter's written care priorities.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `FinalWishSystem` | terminal state | wish state | resources |
| `CaregivingSystem` | assignments | care state | health |
| `PsychologicalSanatoriumSystem` | condition port | progress | conditions |
| `PsychologicalTherapyCatalog` | conditions | — | — |
| `SurvivorMentalHealthSystem` | state | canonical state | — |
| `MentalHealthCrisisSystem` | acuity | crisis state | health |
| `PsychologicalArcSystem` | stress | arc state | — |
| `MemorialSystem` | deaths | memorials | — |
| `GuiltInsomniaSystem` | choices | guilt | — |
| `ConfessionSecretSystem` | secrets | confessions | — |
| `NeedsSystem` | care | morale | — |
| `SkillProgressionSystem` | skills | training | — |
| `DutyRoster` | roles | rosters | — |
| `MedicalPipelineCoordinator` | plans | treatment | — |
| `GenerationalSuccessionEngine` | heirs | legacy | — |
| `Inventory` | items | transfers | — |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`aging_profiles.json`** — `profile_id`, `display_name`, `onset_year_min`,
`frailty_rate_per_year`, `cognitive_rate_per_year`, `adaptation_bonus`,
`role_transitions[]`, `tags`.

**`cognitive_decline.json`** — `stage_id`, `display_name`, `memory_effect`,
`orientation_effect`, `care_need`, `safety_risk`, `lucid_window_rules`, `tags`.

**`palliative_protocols.json`** — `protocol_id`, `display_name`, `comfort_level`,
`medicine[]`, `pain_target`, `monitoring`, `presence`, `prognosis_talk`, `tags`.

**`comfort_items.json`** — `comfort_id`, `display_name`, `effect[]`, `cost[]`,
`source`, `tags`.

**`therapy_modalities.json`** — `modality_id`, `display_name`, `suitable_surfaces[]`,
`session_hours`, `therapist_skill`, `progress_curve`, `setback_chance`,
`relapse_profile`, `tags`.

**`crisis_protocols.json`** — `protocol_id`, `display_name`, `monitoring`,
`intervention[]`, `restraint`, `discharge_condition`, `guilt_cost`,
`trust_cost`, `tags`.

**`grief_stages.json`** — `stage_id`, `display_name`, `duration_days`,
`support_need`, `anniversary_trigger`, `effect[]`, `tags`.

**`legacy_tokens.json`** — `token_id`, `display_name`, `kind`, `delivery_target`,
`effect[]`, `durability`, `tags`.

**`elder_roles.json`** — `role_id`, `display_name`, `physical_demand`, `value`,
`required_skill`, `adaptation[]`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Care hours per week | load | CaregivingSystem |
| Caregiver burnout rate | sustainability | CaringSystem |
| Therapy sessions | access | TherapyProgramSystem |
| Therapy relapse rate | realism | TherapyProgramSystem |
| Crisis occupancy | capacity | CrisisWardSystem |
| Restraint uses | ethics | CrisisWardSystem |
| Good-day windows | value of lucidity | AgingSystem |
| Grief anniversaries marked | community | GriefJourneySystem |
| Legacy tokens delivered | inheritance | LegacySystem |
| Unattended deaths | the measure | PalliativeCareSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether care feels finite and meaningful or
merely punishing.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] No death grants resources.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the dignity review in §35.
- [ ] Phase 7 soak shows aging, care load, grief, and legacy over a full arc.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel therapy, crisis, memorial, or final-wish system exists.

---

## 41. APPENDIX AB — CAMPAIGN ARC TIMELINE

| Phase | Days | Theme | Decision |
|---|---|---|---|
| The name | 1–40 | memory | record or dismiss |
| The list | 41–100 | finite care | triage hours |
| The bad day | 101–160 | burden | rebalance |
| The wish | 161–220 | terminal | fulfill or miss |
| The anniversary | 221–280 | grief | mark or not |
| The long night | 281–330 | presence | how to die |
| What we carry | 331–360 | legacy | what remains |

Each phase changes the shelter's relationship to care, memory, and its own limits.

---

## 42. APPENDIX AC — OPEN QUESTIONS FOR REVIEW

1. Can a survivor recover fully from a reversible condition?
2. Can the player choose a patient's care against their stated wish?
3. Can restraint ever be the correct choice without a guilt cost?
4. Does aging ever progress faster after a radiation event?
5. Can legacy tokens be lost or destroyed?
6. Can the care charter be revised mid-campaign?
7. Should unattended deaths be possible at all?
8. Should therapy progress be visible to the patient's family?

None of these may be decided unilaterally; each changes the expansion's balance and
emotional contract.

---

## 43. APPENDIX AD — CLOSING VIGNETTE

> The long room is dark except for one lamp, and the person in the bed is not
> afraid anymore. Bran sits in the companion chair with her hands folded, and Asa
> checks the dose on time, and the door is open because the dying are not hidden
> here.
>
> Outside, in the garden, Tob has set a stone with a name on it, and Pim has read
> the name aloud twice because the first time was practice.
>
> In the morning the shelter makes breakfast for everyone, including the ones who
> cannot eat, and sets their trays at the table, which is a small thing and the
> whole of it.

---

## 45. APPENDIX AE — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_goodbye_hand_rails` | 3 | Survey, fit, test |
| `quest_goodbye_light_work` | 4 | Assess, adapt, assign, check |
| `quest_goodbye_memory_book` | 4 | Prompt, record, transcribe, bind |
| `quest_goodbye_walking_aid` | 3 | Build, fit, adjust |
| `quest_goodbye_familiar_object` | 4 | Locate, recover, clean, give |
| `quest_goodbye_care_roster` | 4 | List needs, match people, rotate |
| `quest_goodbye_caregiver_burnout` | 4 | Notice, relieve, support, replace |
| `quest_goodbye_night_shift` | 3 | Cover, stay awake, hand over |
| `quest_goodbye_bedding` | 3 | Source, wash, layer |
| `quest_goodbye_bond` | 3 | Pair, work, witness |
| `quest_goodbye_first_session` | 3 | Prepare, hold, schedule next |
| `quest_goodbye_setback` | 3 | Notice, adjust, continue |
| `quest_goodbye_group_session` | 3 | Recruit, seat, facilitate |
| `quest_goodbye_work_therapy` | 4 | Choose task, supervise, review |
| `quest_goodbye_medication_trial` | 5 | Source, dose, observe, decide |
| `quest_goodbye_second_bed` | 4 | Plan, build, staff, certify |
| `quest_goodbye_restraint_rule` | 4 | Draft, debate, publish, log |
| `quest_goodbye_night_watch` | 3 | Sit, record, escalate |
| `quest_goodbye_crisis_discharge` | 4 | Plan, trial, review, return |
| `quest_goodbye_quiet_room` | 3 | Build, furnish, mark safe |
| `quest_goodbye_memorial_stone` | 4 | Choose, carve, set, read |
| `quest_goodbye_anniversary_meal` | 3 | Plan, cook, share |
| `quest_goodbye_letter_kept` | 3 | Receive, hold, deliver |
| `quest_goodbye_tool_passed` | 4 | Choose, clean, teach, hand over |
| `quest_goodbye_story_recorded` | 3 | Prompt, record, archive |
| `quest_goodbye_pain_plan` | 4 | Assess, prescribe, schedule, review |
| `quest_goodbye_comfort_kit` | 4 | Gather, pack, place, label |
| `quest_goodbye_prognosis_talk` | 3 | Prepare, tell the truth, sit |
| `quest_goodbye_last_meal` | 3 | Ask, cook, serve |
| `quest_goodbye_presence` | 3 | Sit, listen, stay |

---

## 46. APPENDIX AF — CARE CHARTER MODEL

The care charter is the shelter's written priority order for finite care. It is
revised through quests and visible to everyone:

| Clause | Options | Effect |
|---|---|---|
| Who gets hours | elders first / crisis first / rotating | fairness |
| Who gets medicine | comfort first / cure first / triage | trust |
| Restraint rule | never / reviewed / as needed | guilt, safety |
| Presence rule | staff only / family / anyone | dignity |
| Ward access | open / scheduled / closed | privacy |
| Legacy rule | recorded / private / none | memory |
| Terminal rule | comfort / trial / family choice | peace |

The charter is not a slider; it is a set of authored commitments with real
consequences. Revising it is possible and costly, and the shelter's culture changes
with it. When the campaign ends, the charter is read into the epilogue verbatim.

---

## 47. APPENDIX AG — LUCID WINDOW AND MEMORY MODEL

Memory is the expansion's most valuable and most fragile resource:

| State | Content available | Value | Risk |
|---|---|---|---|
| Sharp | full recall | high | none |
| Forgetting | names, dates | med | none |
| Repeating | same story | med | fatigue |
| Mixing eras | present and past | high | confusion |
| Lucid window | specific truth | very high | brief |
| Quiet | none | — | grief |

A lucid window is authored, rare, and precious. The system records what is said,
verbatim, because those words are the only source for some of the shelter's history.
The player cannot summon a lucid window; they can only be present when one happens,
which is the expansion's whole lesson about time.

---

## 48. APPENDIX AH — THERAPY OUTCOME TABLE

| Outcome | Meaning | Frequency | Effect |
|---|---|---|---|
| Breakthrough | clear gain | rare | progress jump |
| Steady | small gain | common | progress |
| Plateau | no change | common | none |
| Setback | loss | common | progress loss |
| Relapse | arc return | occasional | arc reset |
| Refusal | stop | occasional | no progress |
| Stable Carrying | condition managed | eventual | quality of life |

Outcomes are authored and weighted honestly. A campaign of therapy is mostly steady
work with setbacks; breakthroughs are rare and earned, and the true success state is
"stable carrying," not cure.

---

## 49. APPENDIX AI — GRIEF SUPPORT TABLE

| Support | Cost | Effect | Requires |
|---|---|---|---|
| Presence | time | grief relief | person |
| Shared meal | food | communal relief | kitchen |
| Memorial stone | labor | named memory | stone |
| Anniversary | food, time | communal relief | calendar |
| Letter kept | none | personal relief | archive |
| Garden work | time | steady relief | garden |
| Story recorded | time | long relief | archive |
| Silence | time | deep relief | quiet room |

Grief support is cheap in materials and expensive in hours. That is the point: the
shelter's care is measured in presence, not production.

---

## 50. APPENDIX AJ — LEGACY DELIVERY TABLE

| Delivery | Requirement | Timing | Effect |
|---|---|---|---|
| In person | survivor present | before death | strongest |
| Sealed letter | archive | after death | strong |
| Tool handoff | heir ready | before/after | practical |
| Voice recording | recorder | after death | strong |
| Public reading | gathering | after death | communal |
| Silent keeping | individual | after death | private |
| Promise kept | faction | any | standing |
| Promise broken | faction | any | standing loss |

Legacy delivery timing matters. A tool handed over in life becomes a lesson; the same
tool found afterward becomes a relic. Both are valid, and the difference is authored.

---

## 51. APPENDIX AK — LORE: THE QUIET HOUSE

The pre-war world had institutions for this and the shelter has only its own habits.
The fiction:

- **The Old Home** was a care facility; its hoists and rails are the shelter's
  starting equipment, and its records are a list of people nobody here knows.
- **The Chemist's** was a pharmacy; comfort medicine is scavenged from behind its
  counter, and the stock is finite.
- **The Long Walk** was a municipal memorial path adopted by the shelter as a secular
  place to remember without a sermon.
- **The Stones** began as a single carved name and became the shelter's memory made
  physical.
- **The Letters** is a mail archive; some of its letters can still be delivered, and
  delivering one is a quest.

No real institution, program, or disease is referenced. The Quiet House is fictional
and exists to explain why the wasteland's care knowledge is partially recoverable.

---

## 52. APPENDIX AL — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do the live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are the systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is care status honest? | lifecycle + a11y tests |
| Tone | Is anything exploitative? | dignity review |
| Balance | Is care finite but fair? | soak results |

The tone gate is mandatory. This expansion touches aging, mental illness, and death;
the review must confirm that nothing is written for shock, comedy, or convenience.

---

## 53. APPENDIX AM — WORKED CARE NUMBERS

| Scenario | Hours/week | Medicine | Effect on shelter |
|---|---|---|---|
| Minimal care | 20 | low | morale falling |
| Balanced care | 60 | med | morale stable |
| Full care | 110 | high | morale rising, output down |
| Crisis-heavy | 150 | high | output down, morale fragile |
| Neglect | 0 | none | deaths, guilt, trust loss |

The expansion's intended arc is that a shelter learns to spend around 60–110 care
hours a week and adjusts its output expectations accordingly. A shelter that refuses
the cost loses something it cannot buy back, which the epilogue records.

---

## 54. CLOSING STATEMENT

ASHFALL already owns the mind and the end of life with real systems: terminal last
wishes with a morale consequence, caregiving with bonds and fatigue, a sanatorium
that treats without overwriting, crisis cases with acuity and a two-bed ward, habit
arcs with treatment tags, memorials, guilt, and a corpus of things people have never
said. What it lacks is the life around those systems: growing old, being cared for,
being treated, and being remembered. The Long Goodbye adds that life without adding a
second anything, and without ever turning death into a resource. It adds a chair, a
night lamp, a recorded voice, and the measure of a shelter: what it does for the
people it can no longer use.

> Wave 3 note: this plan is one of five Wave 3 expansion bibles (22–26). Each is
> self-contained; none requires another to ship. The shared Wave 3 index lives at
> `docs/expansions/wave3/WAVE3_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible.