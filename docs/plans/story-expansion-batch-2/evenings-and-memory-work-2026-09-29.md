# Feature / Task Plan: The Shelter as a Place II — What We Do With the Evenings (culture, festivals and recreation as survival) & Memory Work (how the shelter mourns and remembers)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Open points are in §18 (Expansion backlog) and §19 (Open Mysteries).

> **Subjects covered (2 of the 16 in this batch):**
> 23. **What We Do With the Evenings** — culture, festivals and recreation as survival. (Prefix `EV`.)
> 24. **Memory Work** — how the shelter mourns and remembers. (Prefix `MK`.)
>
> **Companions that already exist and are extended, not replaced:** the festival, ceremony, holiday, hobby, downtime, artwork, museum, oral-lore and memorial owners listed in §2 and §3; the plan-24 memorial work (`Mourn`, the vigil), and the prose corpus in `docs/expansions/`. Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Sister plan.** `works-below-and-machine-in-the-walls-2026-09-29.md` is the *body* of the shelter; this plan is its *evenings*. The two share one tonal rule: **the people supply the character.**
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, data, ledger or other plan. Paths are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c**, **7b**, **10b** and **19** carry story texture. Sample lines are content candidates for JSON rows, never code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — After the Work, and After the Loss

> *"Nobody survives on food alone. They survive on the hour after the food, when someone
> hums, and on the day after the funeral, when someone sets the table for one fewer and does
> not say so."*

A shelter has a working day and a night. Between them is a small, unclaimed stretch of hours: **the evening.** It is where people decide, without deciding, whether they are a crew or a community. The game already owns every part of that stretch — hobby sessions, a downtime system, festivals, ceremonies, holidays, artworks, a museum, songs, memorial rites and a memorial wall. What it does not yet have is the **shape of it over time**: the sense that a shelter *has evenings*, that some of them have become customs, and that the people who are gone are still part of them.

These two subjects are paired because they are the same question at two tempos. **What We Do With the Evenings** is the *daily* answer to "why go on?" **Memory Work** is the *occasional* answer to "who are we going on for?" A shelter that has good evenings can afford to mourn. A shelter that mourns well has better evenings.

**Tone & register.**

- **Evenings are written in the voice of the table.** A harmonica half-learned, a card game with house rules, a story told wrong on purpose. Small, specific, slightly funny. Nothing is grand.
- **Memory Work is written in the voice of the ledger and the chair.** A name said aloud at muster. A bunk left made. Restraint over sentiment; the grief is in what is *not* said.
- **Never mawkish, never mocking.** The tone is fictional, restrained and human (CLAUDE.md UI/tone rule). No real people, countries, wars, copied text or copied rites. Rites are invented, plain, and physical.

**What the two share.** *Repetition as meaning.* A thing done once is an event. Done five times it is a **custom**. Done for the dead every year it is **memory**. Neither part gives the player a new lever; both reward *keeping something going*.

**The second layer.** Both halves of this plan measure the same thing: the distance between
*surviving* and *continuing*. A shelter can be fed to the calorie and still be only a crew; what
turns it into a place is repetition that nobody mandated — a stool brought three nights running, a
name spoken every year until the speaking is the point. The evening is where a household decides
to exist on purpose, and the memorial is where it agrees to carry its dead without being asked
twice. Neither is efficient. Both are the reason the arithmetic is worth doing. And the evening is the only hour the ledger
refuses to total — which is why the household keeps it.

---

## 1. Goal & Outcome

### 1.1 What We Do With the Evenings (EV)

> *Design intent: at day 200, the player should be able to name three things the shelter does
> in the evenings that nobody scheduled — and know which one it would miss most.*

- **Goal:** Give the existing recreation and culture owners a **shape over time**: an **Evening Ledger** (a derived read of what was done each evening, by whom, with whom), a stored set of **Customs** (recurring practices the household has *made* — a historical fact, earned by repetition), a derived **Hearth Warmth** reading (how alive the evenings are), a derived **Calendar** (which holidays, anniversaries, festivals and ceremonies are due), and a **Thin Larder** rule that makes festivals a real trade-off against survival stores rather than free morale.
- **Outcome (observable):**
  1. The **Evenings surface** (an existing panel — `SurvivorDowntimePanel` or `CeremonyFestivalPanel`, chosen at P0) shows tonight's options drawn from the existing owners, last week's evenings as a strip, and any **Custom** the household has formed.
  2. A **Custom** forms when the same practice (a hobby session type, a holiday observance, a ceremony) has been held on N qualifying evenings inside a window; the household names it (seeded pick or player rename) and the chronicle records it once.
  3. **Hearth Warmth** is a derived 0–100 band (Cold / Thin / Warm / Alight) from real participation, variety and recency — *read only*.
  4. A **Festival** shows its **true cost** (food, fuel, time) against the current larder, with a plain warning when it would leave the shelter under a survival floor; skipping a holiday is recorded, never punished beyond what the owner already applies.
  5. Save/load round-trips; a legacy save loads as "no customs, no ledger history" and every existing recreation, festival and ceremony behaviour equals today's.
- **Non-Goals (EV):** no new morale system (morale stays with its owners); no change to any existing morale, stress or cost value; no new festival, ceremony or hobby engine; no minigames; no new routed panel; no real-world holidays or religions; no Unity.
- **"Done" (EV):** §13 EV acceptance passes via `bin/run-scoped-tests`; existing festival, ceremony, celebration and hobby tests unchanged; handoff lists untouched shared paths.

### 1.2 Memory Work (MK)

> *Design intent: when a survivor dies on day 90, the player should still be able to see, on
> day 400, what the shelter did about it — and what it stopped doing.*

- **Goal:** Give the existing memorial owner a **long tail**: a derived **Grief Season** per death (Raw, Heavy, Settling, Carried) read from days since death and whether the vigil was held; a stored **Remembrance** record per death (which rite was performed, who keeps the name, which anniversaries were observed) — *only what the household did*; a derived **Held / Quiet** state for each name (a name that is said stays Held; a name never said drifts to Quiet, **never deleted**); a **Name Reading** ritual on anniversaries; and **Things Left** (personal effects and heirlooms) tied to the existing heirloom hook.
- **Outcome (observable):**
  1. Each entry on the memorial wall shows its **Grief Season** and **Held/Quiet** state, derived from the existing entry plus the Remembrance record.
  2. On a death the player may choose a **rite** from the existing rite catalog (roll-call, empty bunk, division of effects, work-gang farewell, wall tally, last-wish committal); the choice is recorded once and the existing grief-reduction values apply through their owner.
  3. A **Keeper of the Name** (a survivor) can be assigned to each name; the keeper's own state shapes the tone of the reading line.
  4. On the anniversary of a death and on the shelter's **Founding Day**, a **Name Reading** may be held; a held reading raises Held for the names read.
  5. **Things Left** items appear in a bounded shelf; giving one to a survivor is an existing inventory act and is recorded as a Remembrance fact.
  6. Save/load round-trips; a legacy save loads with no Remembrance records and every existing memorial, vigil and grief behaviour equals today's.
- **Non-Goals (MK):** no new grief or morale arithmetic; no change to `MemorialSystem`, `RelationsGriefSink` or the plan-24 vigil; no forgetting-as-deletion; no afterlife, no ghosts, no supernatural; no new routed panel; no Unity.
- **"Done" (MK):** §13 MK acceptance passes; existing memorial and grief tests unchanged; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**One calendar, one ledger of acts.** The Evening Ledger and the Remembrance record are different *stored* structures but share one **Calendar** read model: an anniversary of a death is an **observance** in the same list as a holiday. A **Name Reading** is an *evening* practice that may become a Custom. Neither part keeps a second calendar or a second "observed" flag (Rule 5).

---

## 1b. Texture, Mystery & Voice

**Evenings: what the shelter does when nobody is making it.**

The game will never tell the player "morale is 71." It will say: *"Third Storytelling night this week. Somebody has started bringing their own stool."* The number stays with its owner; the ledger keeps the **fact** that it happened three times. The Custom is not a bonus; it is a **name**. *"Stool Night."* Once a household has a name for a habit, it defends the habit.

**Thin Larder: the honest cost.**

A festival is not free. A harvest feast burns food the shelter may need in February. The game shows the sum, shows the floor, and lets the player choose. Skipping a holiday is not a sin; it is an entry — *"Midsummer skipped. Nobody said anything. Nobody hummed."*

**Memory Work: what the shelter does when someone is gone.**

A death does not end at the vigil. On day 90 there is a funeral. On day 91 the bunk is made. By day 120 the bunk is somebody else's, and that, too, is written down — gently, once. The **Grief Season** is not a meter to manage; it is a **weather report** for a person the shelter still lives with.

**Held and Quiet.**

A name that is said aloud is *Held*. A name that has not been said for a long time is *Quiet*. **Quiet is not gone.** A Quiet name can be Held again by a single reading. The wall keeps all of them. That is the entire mechanism of forgetting: **attention, recorded.**

**What the player is never told.**

- **Why the harmonica is always half-learned.** The `hobby_harmonica` row is authored; the half-learned tune is not explained.
- **Whether the Founding Day reading includes the names of those who sealed the doors.** The reading begins *after* the first name. What came before is not written.
- **Who leaves the outside crumb.** `ritual_crust_for_the_waste` is optional and unattributed. It stays that way.
- **What is on the last page of the tally.** The wall tally has a final blank line. It is blank on purpose.

**Voice — sample fragments (content candidates for `evening_lines.json`, `remembrance_lines.json`).**

> "Third night of stories. Sabine brought her own stool. Nobody had asked. That is a custom now." — Evening Ledger (EV)

> "Midsummer skipped. The larder was thin and everyone knew it. Nobody said anything." — Calendar (EV)

> "The feast will cost 40 food and 12 fuel. After it, the larder sits eleven days above the floor." — Festival preview (EV)

> "Roll call this morning. Marek's name went out into the cold concrete and nobody answered, and then somebody did." — rite line (MK)

> "The bunk was made on day 91. On day 118 Ines asked if she could have it. Ambrose's sister said yes." — Remembrance note (MK)

> "Year one. We read the names. It took four minutes. It felt longer than it was." — Founding Day reading (MK)

**Design texture beats.**

- **A custom is a name, not a bonus (EV).** The ledger records a fact; the household supplies the word.
- **The cost is shown, not hidden (EV).** A festival preview states what it takes and what is left.
- **Quiet is not gone (MK).** A name can always be raised again by being said.
- **The rite is a choice with a record (MK).** Which rite was performed is a fact the shelter carries.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §19's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. §7b/
§15b remain the texture sections; this section is the **objects** those sections leave behind.)*

**What the evenings leave lying around.**

> "A stool by the third-storytelling bench. Nobody assigned it. Nobody would now dare move it."

> "Festival preview slip: 40 food, 12 fuel, eleven days above the floor. Somebody has written 'worth it' at the bottom and initialled it."

> "Calendar entry: Midsummer — skipped. The entry is kept. Skipping is an entry."

**What the mourning leaves lying around.**

> "Bunk 7, made on day 91. The making is recorded, not the feeling. Grief is procedural here, and that is the mercy."

> "Wall tally, final line blank. Blank on purpose."

> "Keeper's list: names in an order nobody has explained. The order changes only when a name is read."

**Scenes the player may piece together.**

> "Four minutes for the names. It felt longer than it was. Nobody has ever suggested shortening it."

> "The outside crumb is gone in the morning. `ritual_crust_for_the_waste` is unattributed and will remain so."

**Held silences (texture, not register rows).**

- Who started the first custom. The ledger records the fifth repetition as a name; the first is not recorded and must never be invented. Texture only.
- What the last page of the tally is *for*. It is blank on purpose — never fill it, never explain the purpose, and never let a future pass tidy it away.

**Fourth pass — the distance between surviving and continuing (texture only; §19 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §19's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Repetition is the plan's only magic and it must never be written as
magic. A stool, a name, a four-minute silence — the prose should record each one with the same
flat attention a ledger gives a column, and let the fifth repetition do what the first could not.
Restraint over sentiment: the grief lives in what is not said, and the warmth in what is done
twice.

**What the evenings leave lying around.**

> "Storytelling bench, third seat. The stool is older than the custom, or the custom is older than
> the stool; nobody left would know."

> "Calendar entry: Midsummer — kept this year, in advance. Hope is procedural here too."

**What the mourning leaves lying around.**

> "Four minutes for the names. The clock in that room is the only one nobody has ever suggested
> winding forward."

**Held silences (texture, not register rows).**

- Whether the first custom began as grief. A thing done five times is a custom (§0); what the first
  repetition was for is not recorded and must never be. Texture only.
- What the morning after a festival is for. The preview slip counts what the evening costs; the
  morning is not authored and the accounting stops at the floor.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `ShelterFestivalEngine` owns festival scheduling/commencement/daily tick/cancel and save state (`ShelterFestivalSaveState`, `FestivalPlanSaveState` with planned day, duration, days active, morale boost permille, despair reduction permille, required commodities). Four `FestivalType`s: HarvestCommunion, RemembranceVigil, MidwinterSolstice, FoundingJubilee. | `Culture/ShelterFestivalEngine.cs` L8–322 | LIVE |
| E2 | `CeremonySystem` (data `ceremonies.json`, 5 ceremonies: founding day, remembrance vigil, long-night bonfire, treaty market, ashfall harvest) owns `ScheduleCeremony`, `ContributeResource`, `InviteFaction`, `TickDay`, save state. | `Narrative/CeremonySystem.cs`; `Data/ceremonies.json` | LIVE |
| E3 | `SeasonalCelebrationSystem` (data `shelter_celebrations.json`) owns 5 holidays (new year day 1, spring thaw, midsummer, harvest observance, winter solstice; each with `base_morale_boost`, `food_cost`, `fuel_cost`, `allowed_activities`), 3 anniversary types (founding, fallen, milestone), 3 scales (small/… with `cost_multiplier`, `morale_multiplier`, `is_memorable`); `CheckHolidayForDay`, `TryHoldCelebration`, `TrySkipHoliday` (returns a morale penalty), `CommemorateAnniversary`, save state. | `Events/SeasonalCelebrationSystem.cs`; `Data/shelter_celebrations.json` | LIVE |
| E4 | **Two hobby catalogs exist:** `recreation.json` (6 hobbies: whittling, guitar, harmonica, card games, sketching, storytelling; consumed by `SurvivorDowntimeSystem`) and `hobby_definitions.json` (10: woodcarving, painting, reading, instrument, chess, botany, calisthenics, tailoring, coin collecting, storytelling; consumed by `HobbySystem`). Only `storytelling` overlaps by concept, under different ids. | `Data/recreation.json`, `Data/hobby_definitions.json`; `Recreation/SurvivorDowntimeSystem.cs`; `Survivors/HobbySystem.cs` | LIVE (finding) |
| E5 | `SurvivorDowntimeSystem` owns active hobby sessions (`ActiveHobbySession`: session id, hobby, room, participants, start day), stress relief, morale effect, brawl risk, output items. | `Recreation/SurvivorDowntimeSystem.cs` | LIVE |
| E6 | `CultureCreationSystem` owns artworks (`CreateArtwork`, `DisplayArtwork`, `GetShelterCultureMoraleBonus`, `GetCensus`), art forms catalog; `ShelterMuseumSystem` owns artifacts, exhibitions, curator, `VisitMuseum`; `OralLorePerformanceSystem` owns heard songs and producers. | `Culture/CultureCreationSystem.cs`, `ShelterMuseumSystem.cs`; `Narrative/OralLorePerformanceSystem.cs` | LIVE |
| E7 | `MemorialSystem` owns memorial entries (`Memorialize` idempotent; `Mourn(deceasedId, day)` once-per-death via persisted `MournedDay`; `LatestUnmourned()`; `OnMemorialized`, `OnMourned` events); entry fields include survivor id, cause, day, survived days, epitaph, eulogy, heirloom item and recipient, morale delta, death quality, outcome. Grief fires through `IGriefSink` (`RelationsGriefSink`) on first memorialization only. | `Memorial/MemorialSystem.cs` L148–421; `RelationsGriefSink.cs` | LIVE |
| E8 | `SpiritualMeaningCoordinator.PerformMemorialRite(deceasedId, riteId, day)` exists and records the rite per death; the only live caller (`Main.Campaign.cs` L265) hard-codes `ShelterVigilRiteId`. Six rites in `memorial_rites.json`. | `Spiritual/SpiritualMeaningCoordinator.cs` L91; `src/Main.Campaign.cs` L265 | **RESOLVED (§2b)** |
| E9 | `spiritual_rituals.json` holds 19 small optional rituals with `context_trigger`, `morale_delta`, `friction_flag`, `cooldown_days` (e.g. the two taps on the outer iron; the outside crumb). | `Data/spiritual_rituals.json` | LIVE |
| E10 | `memorials_expansion_05.json` holds authored memorial texts; `GraveEpitaphCatalog` selects epitaphs by cause with a seeded pick; the save section registry has a `memorial` section ("Fallen survivors memorial wall"). | `Data/memorials_expansion_05.json`; `Memorial/GraveEpitaphCatalog.cs`; `Save/SaveSectionRegistry.cs` L106 | LIVE |
| E11 | Host surfaces exist: `CeremonyFestivalPanel`, `SurvivorDowntimePanel`, `Main.Ceremony.Integration.cs`, `Main.Recreation.Integration.cs`. | `src/UI/`; `src/Main.*` | LIVE |
| E12 | Festival engine (`FestivalType.RemembranceVigil`), seasonal celebrations ("Remembrance of the Fallen") and the memorial wall's vigil all cover remembrance; celebrations and festivals are ticked in `TickOrphanSealWave1`. | `Main.OrphanSealWave1.cs` L386–387; `Culture/ShelterFestivalEngine.cs` L11 | **RESOLVED (§2b)** |
| E13 | A shared "evening" concept (a per-day slot with a record of what was done) does not exist; grep for `evening`/`Evening` finds only flavour text. | grep over Core | LIVE (finding) |
| E14 | `CulturalArchiveVaultSystem.TryRecordChronicleEntry` (fixed key) and `JournalSystem.TryAddRawEntry` (free text; used by the existing vigil). | `Culture/CulturalArchiveVaultSystem.cs` L385; `Journal/JournalSystem.cs` L281 | **RESOLVED (§2b)** |
| E15 | Sections `memorial`, `ceremony`, `seasonal_celebration`, `shelter_festival` exist; MK nests in `memorial`, EV in `shelter_festival`. | `Save/SaveSectionRegistry.cs` L106, L189, L264, L273 | **RESOLVED (§2b)** |
| E16 | Difficulty scalars are read at owner read sites; this plan changes no scalar. | `difficulty_presets.json` | LIVE |
| E17 | Heirloom systems exist (`DwellerHeirloomCatalog`, `MemorialInput.HeirloomItemId/RecipientId`). | `Narrative/DwellerHeirloomCatalog.cs`; `MemorialSystem.cs` | LIVE (hook) |

**Four findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E4 — two hobby catalogs with different consumers and mostly different ids.** The Evening Ledger must read *both* owners through one small adapter and never merge the catalogs (merging is a separate debt item, not this plan's job).
2. **E1/E2/E3 — three "remembrance" concepts.** A remembrance vigil festival, a remembrance vigil ceremony and an anniversary of the fallen all exist. MK must present them as **one Calendar list with provenance**, and never add a fourth.
3. **E13 — there is no "evening."** The slot does not exist as a concept. EV *derives* it from timestamps and session records; it does not add a scheduler.
4. **E8 — six memorial rites are authored but the player-choice route is unconfirmed.** If no choice route exists, P0 records it, and MK's rite choice becomes a single additive command on the existing coordinator (not a new system).

---

## 2b. Evidence pass 1 — premises checked against source (2026-09-29)

| # | Open item | Result | Edit made |
|---|---|---|---|
| E8 | Can the player choose a rite per death? | **The coordinator already takes a rite id per death.** `SpiritualMeaningCoordinator.PerformMemorialRite(deceasedId, riteId, day)` validates the rite in the catalog, records `PerformedRiteId`/`RiteCompleted` on the death's mourning arc, and raises `OnMemorialRitePerformed`; `SkipMemorialRite`, `RegisterDeath`, `TickMourning`, `GetMourningArc` sit beside it. **But the only live caller hard-codes one rite**: `Main.Campaign.cs` L265 performs `ShelterVigilRiteId` (`memorial_rite_roll_call_naming`) after the memorial wall's vigil. | §11.2 rewritten: MK adds **no** command; the choice route is a new *caller* of the existing method, and the current hard-coded vigil becomes the default when the player chooses nothing |
| E12 | Which owners run; overlaps | **Two of the four are wired and ticked in a normal campaign**: `SeasonalCelebrationSystem` (`SetupSeasonalCelebration`, ticked by `TickSeasonalCelebration`) and `ShelterFestivalEngine` (`ShelterFestivalHostSession.TickDay`), both in `Main.OrphanSealWave1.cs`; `CeremonySystem` is ensured in `Main.Ceremony.Integration.cs`. **The overlap is real:** the festival engine has `FestivalType.RemembranceVigil`, the seasonal system has an anniversary "Remembrance of the Fallen", and the memorial wall already holds its own vigil (L257–265). Three owners, one idea. | EV/MK **do not add a fourth**. The Remembrance evening is *one* of the three, chosen by DEC-EV-11; the others are told (by their existing hosts) not to double-fire |
| E14 | Chronicle writer | Two writers: `CulturalArchiveVaultSystem.TryRecordChronicleEntry` (fixed `summary_key`, deduped) and `JournalSystem.TryAddRawEntry` (free text, deduped per key — the memorial vigil already uses it: key `memorial_mourned_<id>`). | fixed lines → chronicle keys; composed lines with a name or a day → journal, following the existing vigil precedent (DEC-MK-11) |
| E15 | Save home | Real sections exist: `memorial` (`MemorialSaveStore.SectionName`), `ceremony` (`CeremonySaveStore`), `seasonal_celebration` and `shelter_festival` (both registry-owned, lifecycle group *ExpandedShelter*). | MK nests in `memorial`; EV nests in `shelter_festival`. No new section |
| — | Founding-day source | `ShelterArchiveSystem.FoundingDay` (default 1, captured and restored; `Shelter/ShelterArchiveSystem.cs` L56, L76). `FamilyUnit.foundingDay` in the lineage extension is a *family's*, not the shelter's. | anniversary "founding" reads `ShelterArchiveSystem.FoundingDay` |
| — | Last-wish rite | `memorial_rite_last_wish_committal` has `requires_recovered_body: false`; the coordinator's `RiteType` is `"last_wish_committal"`. Nothing in the catalog *stores* what the wish was. | the rite is offered only when the household knows a wish (a plan-level rule); the wish text is authored per survivor bio, not invented at the rite |
| — | Suggested keeper | `SurvivorRelationsSystem.RelatedIds(id)` plus `TryGetRelationship(a, b, out entry)` (entry carries `affinity` −100..100, `trust`, `grief`) is a real read seam. | the suggestion is the alive `RelatedIds` member with the highest `affinity`; ties broken by id |

**What did not change:** the Remembrance Book, the Evening slate, the keeper role, preview lines that speak of what the household *does* and never of how anyone *feels*, and the rule that rite numbers stay in the catalog.

**One consequence worth stating plainly.** The shelter already mourns — always the same way. Every death gets a roll-call. The plan's oldest promise is therefore small and specific: *let the household choose, and let the choice be remembered*. That is the whole distance between a ritual and a habit.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Hobby sessions, stress relief, output items | `SurvivorDowntimeSystem` (`recreation.json`) and `HobbySystem` (`hobby_definitions.json`) | none |
| Festivals, commodities, morale/despair boosts | `ShelterFestivalEngine` | none |
| Ceremonies, faction invitations, resource contributions | `CeremonySystem` | none |
| Holidays, anniversaries, celebration scale, skip penalty | `SeasonalCelebrationSystem` | none |
| Artworks, cultural morale bonus | `CultureCreationSystem` | none |
| Museum, exhibitions | `ShelterMuseumSystem` | none |
| Songs heard | `OralLorePerformanceSystem` | none |
| Memorial entries, vigil (`Mourn`), grief dispersion | `MemorialSystem`, `RelationsGriefSink` | none |
| Rites, rituals catalogs | `SpiritualCatalogLoader` / `SpiritualMeaningCoordinator` | none |
| Chronicle | Plan 34 / `ArchiveDeskState` | append-only writes |
| What the household made a habit (Customs) | — | `EveningState.Customs` (stored: small historical records) |
| Which evenings had which practice (Ledger) | — | `EveningLedger` (**derived** from owner records; a bounded stored ring only if P0 finds no owner keeps the needed timestamps) |
| Hearth Warmth | — | `HearthWarmth` (pure derivation) |
| Calendar (holidays, anniversaries, festivals, ceremonies, death anniversaries) | — | `ShelterCalendar` (pure read model over the owners) |
| Thin Larder preview | — | `FestivalCostPreview` (pure read of owner costs and stores) |
| Remembrance record (rite chosen, keeper, observances) | — | `RemembranceState` (stored: one record per death) |
| Grief Season, Held/Quiet | — | pure derivations |

**Non-duplication statement.** Every morale, stress, cost and grief number stays with its owner. The plan stores only **what the household did** (a Custom formed; a rite chosen; a keeper named; a reading held) — historical facts. Everything else is derived on read. **No new morale or grief authority is created.**

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (EV):** `Assets/Ashfall.Core/Culture/Evenings/EveningLedger.cs` (new, pure read), `Evenings/EveningCustoms.cs` (new, state + rules), `Evenings/HearthWarmth.cs` (new, pure), `Evenings/ShelterCalendar.cs` (new, read model), `Evenings/FestivalCostPreview.cs` (new, read), `Evenings/EveningCatalogLoader.cs` (new).

**Core (MK):** `Assets/Ashfall.Core/Memorial/Remembrance/RemembranceState.cs` (new), `Remembrance/GriefSeason.cs` (new, pure), `Remembrance/NameHeld.cs` (new, pure), `Remembrance/NameReading.cs` (new), `Remembrance/ThingsLeft.cs` (new), `Remembrance/RemembranceCatalogLoader.cs` (new). `Memorial/MemorialSave.cs` (**additive nested DTO only**, `INT`) — home chosen at P0.

**Data (EV):** `evening_customs.json`, `evening_lines.json`, `evening_calendar_notes.json`.
**Data (MK):** `remembrance_lines.json`, `remembrance_keepers.json`, `remembrance_things_left.json`, `name_reading_scripts.json`.

**Host:** `src/Main.Recreation.Integration.cs` and `src/Main.Ceremony.Integration.cs` (`INT`: ledger/custom credit hooks), `src/Main.SubsystemComposition.cs` (`INT`: day-owner registration), `src/Main.Campaign.cs` (`INT`: briefing line).

**Presentation (both):** extend `SurvivorDowntimePanel` / `CeremonyFestivalPanel` (Evenings, Calendar) and the existing memorial wall surface (Remembrance). No new routed panel (DEC-EV-08, DEC-MK-08).

**Tests:** `Ashfall.Core.Tests/Culture/Evenings/*` (Ledger, Customs, Warmth, Calendar, CostPreview, Save); `Ashfall.Core.Tests/Memorial/Remembrance/*` (GriefSeason, Held, Reading, ThingsLeft, Save); parity guards extend the existing festival, ceremony, celebration, hobby and memorial tests unchanged.


---

# PART ONE — WHAT WE DO WITH THE EVENINGS

## 5. The Evening Ledger (derive, do not store)

### 5.1 What an evening is

An **evening** is not a scheduler slot. It is a **derived record**: for calendar day *d*, the set of *practices* the owners recorded as held that day, each with participants. A **practice** is one of:

| Practice kind | Source owner | Fact read |
|---|---|---|
| `hobby` | `SurvivorDowntimeSystem` / `HobbySystem` | session id, hobby id, room, participants, start day |
| `holiday` | `SeasonalCelebrationSystem` | holiday id, held or skipped, scale, day |
| `festival` | `ShelterFestivalEngine` | festival id, type, days active |
| `ceremony` | `CeremonySystem` | ceremony id, day held, attendance |
| `oral_lore` | `OralLorePerformanceSystem` | song heard, producer, day |
| `museum` | `ShelterMuseumSystem` | visit, exhibition, day |
| `ritual` | spiritual coordinator | ritual id, trigger, day |
| `mourning` | `MemorialSystem` | `MournedDay` = d |
| `reading` | MK `NameReading` | reading held, day |

The ledger adapter (`IEveningSource`, one per owner) exposes `PracticesOn(day)`. **No owner gains a method** unless P0 finds one keeps no per-day record (E5's `ActiveHobbySession.startedDay` gives hobbies; holidays record `WasHolidayOccurrenceHeld`; festivals carry `PlannedDay + DaysActive`). Where an owner keeps no dated record, P0 records the gap and the ledger uses a **bounded stored ring** of the last 60 evenings (`EveningRing`, ≤ 60 entries, ≤ 6 practices each) fed by the host credit hook (DEC-EV-02).

### 5.2 The adapter for two hobby catalogs (finding E4)

`HobbyEveningSource` unions `SurvivorDowntimeSystem` sessions (ids `hobby_whittling`, `hobby_guitar`, `hobby_harmonica`, `hobby_card_games`, `hobby_sketching`, `hobby_storytelling`) and `HobbySystem` sessions (`hobby_woodcarving`, `hobby_painting`, `hobby_reading`, `hobby_instrument`, `hobby_chess`, `hobby_botany`, `hobby_calisthenics`, `hobby_tailoring`, `hobby_coin_collecting`, `hobby_storytelling`). The adapter maps each hobby id to an **evening family** for custom formation:

| Family | Hobby ids mapped |
|---|---|
| `music` | `hobby_guitar`, `hobby_harmonica`, `hobby_instrument` |
| `craft` | `hobby_whittling`, `hobby_woodcarving`, `hobby_tailoring`, `hobby_sketching`, `hobby_painting` |
| `stories` | `hobby_storytelling` (both catalogs) |
| `games` | `hobby_card_games`, `hobby_chess`, `hobby_coin_collecting` |
| `quiet` | `hobby_reading`, `hobby_botany` |
| `body` | `hobby_calisthenics` |

Families are authored in `evening_customs.json` (`family_map`). A hobby id missing from the map is a validator error (E-2), so a newly added hobby cannot silently fall out of the ledger.

### 5.3 The strip

The Evenings surface shows the **last 7 evenings** as a strip: for each day, up to 3 practice icons/labels and a participation count. A day with no practice shows "—" (no shame, no penalty). The strip is a pure projection.

### 5.4 Worked example — one week

| Day | Practices held | Participants |
|---|---|---|
| 40 | storytelling (`stories`) in mess hall | 4 |
| 41 | — | — |
| 42 | harmonica (`music`) in quarters | 2 |
| 43 | storytelling (`stories`) in mess hall | 5 |
| 44 | card games (`games`) | 3 |
| 45 | — | — |
| 46 | storytelling (`stories`) in mess hall | 5 |

Reading: three `stories` evenings in seven days, at the same room, with rising attendance. That is the raw material of a Custom (§6).

---

## 6. Customs (the only stored EV fact)

### 6.1 What a Custom is

A **Custom** is a *practice the household has made its own*. It is stored because it is a **historical fact about the community** (it formed on day *d*, it has this name), not a derivable score.

```csharp
public sealed class EveningState
{
    public int SchemaVersion = 1;
    public List<CustomRecord> Customs = new();
    public List<EveningRingEntry> Ring = new();     // only if P0 finds no owner keeps dated records
    public List<string> ChronicleKeys = new();      // idempotence guard
}
public sealed class CustomRecord
{
    public string CustomId;         // authored template id
    public string Family;           // music | craft | stories | games | quiet | body | holiday | ritual
    public string Name;             // household name; template default, coined, or player
    public string NameSource;       // "template" | "coined" | "player"
    public int    FormedDay;
    public int    TimesKept;        // incremented by real held practices matching the template
    public int    LastKeptDay;
    public bool   Lapsed;           // true after the lapse window; can be revived
    public string RoomId;           // where it happens, if consistent (else null)
}
```

### 6.2 Formation rule

A **template** (authored) says: family, minimum count *N* within a window of *W* days, minimum distinct participants *P*, and optionally a room requirement. When the ledger shows *N* qualifying practices in *W* days with ≥ *P* distinct participants across them, **and no live Custom of that family exists**, a Custom forms.

Default templates (data, tunable, DEC-EV-04):

| Template id | Family | N in W | P | Default name |
|---|---|---|---|---|
| `custom_story_night` | stories | 3 in 10 | 3 | Story Night |
| `custom_tune_hour` | music | 3 in 10 | 2 | The Tune Hour |
| `custom_bench_hour` | craft | 4 in 10 | 2 | Bench Hour |
| `custom_card_table` | games | 3 in 10 | 3 | The Card Table |
| `custom_quiet_corner` | quiet | 4 in 14 | 2 | The Quiet Corner |
| `custom_morning_stretch` | body | 4 in 10 | 3 | Morning Stretch |
| `custom_holiday_table` | holiday | 2 holidays held in a row | 4 | The Long Table |
| `custom_outer_taps` | ritual | 5 ritual triggers in 14 | 2 | The Two Taps |

**Naming.** On formation the household name is the template default, unless a **coined** name is drawn from `coined_names[family]` (3–5 each) through the seeded stream `evening_custom:<family>:<formed_day>`. The player may rename any Custom (≤ 24 chars, trimmed, control characters stripped). A player name is never overwritten. Formation writes one chronicle line, idempotent by key `custom:<family>:<formed_day>`.

### 6.3 Keeping, lapsing, reviving

- A qualifying practice on any day sets `LastKeptDay = d` and `TimesKept++`.
- **Lapse:** no qualifying practice for *L* days (default 21; DEC-EV-05) → `Lapsed = true`; one chronicle line, once (`lapse:<custom_id>:<formed_day>`).
- **Revive:** a qualifying practice on a lapsed Custom clears `Lapsed`, records one chronicle line (`revive:…`), and does **not** reset `TimesKept` or `FormedDay`. Revival is cheap on purpose.
- A Custom never disappears from the record.

### 6.4 What a Custom does (and does not)

Mechanically: **nothing new.** A Custom is a name and a record, and its only downstream effect in v1 is on **Hearth Warmth** (§7) and on **text** (briefing and ledger lines). DEC-EV-03 (unsigned) records the option of a small bounded morale echo routed through the *existing* `CultureCreationSystem.GetShelterCultureMoraleBonus` seam; the default is **no echo**, so morale arithmetic is untouched.

### 6.5 Worked example — Stool Night

Day 46 in the week above: the third `stories` practice inside 10 days with 4, 5 and 5 participants (P = 3 distinct across the set ≥ 3). No live `stories` Custom → forms. Coin draw yields "Stool Night" from the `stories` pool. Chronicle: *"Third story night in ten days. Sabine brought her own stool. It is called Stool Night now."* On day 78 no story has been held for 21 days → lapses; on day 83 Marek tells one → revived: *"Stool Night is back. Someone carried the stool in from the corridor."*

---

## 7. Hearth Warmth (a derived reading)

### 7.1 Definition

Hearth Warmth is a pure function of the last **14 evenings**:

`Warmth = 40*Presence + 25*Variety + 20*Reach + 15*Ritual` (each term 0..1, result 0..100)

| Term | Meaning | Derivation |
|---|---|---|
| Presence | how many of the last 14 evenings had ≥ 1 practice | `evenings_with_practice / 14` |
| Variety | how many distinct families appeared | `min(1, distinct_families / 4)` |
| Reach | how many survivors joined at least one practice | `participants_distinct / max(1, population)` (capped at 1) |
| Ritual | live, unlapsed Customs | `min(1, live_customs / 3)` |

Bands: **Cold** < 25, **Thin** 25–49, **Warm** 50–74, **Alight** ≥ 75. Weights and band cuts are data (`evening_customs.json: warmth`), tunable, DEC-EV-06.

### 7.2 Rules

- **H-1** Warmth is **never stored** and never feeds an owner's arithmetic (DEC-EV-03 default).
- **H-2** Population is read from the existing roster owner; a shelter of 1–2 survivors uses the same formula (Reach dominates).
- **H-3** A day of expedition, illness or crisis where the owners record no practice counts as a gap, not a penalty.
- **H-4** Warmth appears on the Evenings surface as the band name plus a short line from `evening_lines.json`, never as a bare number alone.

### 7.3 Worked example

Population 8. Last 14 evenings: 9 with a practice (Presence 0.64), families seen: stories, music, games (Variety 0.75), 7 distinct participants (Reach 0.875), 2 live Customs (Ritual 0.67).
`Warmth = 40(0.64) + 25(0.75) + 20(0.875) + 15(0.67) = 25.6 + 18.75 + 17.5 + 10.05 = 71.9` → **Warm**. Line: *"Nine evenings in fourteen had somebody at the table. Nobody counted. Somebody did."*

### 7.4 The first ten days (new shelter)

Warmth on day 1–10 uses the days elapsed as the denominator (not 14), so a new shelter is not "Cold" by definition. Below 3 recorded evenings the surface shows **"Too early to say"** instead of a band.

---

## 7b. Texture — the table

**The Calendar as a household object.**

The Calendar (below) is a list, not a planner. It shows what is due: a holiday, a festival in progress, an anniversary of a loss, a Founding Day. Each row carries **provenance** (which owner it came from) and a plain line about what it will cost. The calendar never nags; it *notes*.

**Thin Larder — the festival that costs February.**

Festival previews are honest. *Harvest Communion costs 40 food and 12 fuel; after it, the larder sits eleven days above the floor.* The floor is the existing survival threshold read from the food/fuel owners (`FoodFloorDays`, read only). If the preview would drop the shelter beneath the floor, the line turns plain and grey: *"After this the shelter has four days of food. The feast will be remembered. So will the four days."* The command stays available. The plan never blocks; it *tells*.

**Skipping is a fact, not a sin.** `TrySkipHoliday` returns the owner's own penalty. The ledger records *"Midsummer skipped."* The tone is neutral.

**Small cruelties (kept small).** A festival held while a death is unmourned is legal. The surface shows one line: *"Marek's vigil has not been held."* Nothing else changes. The player sees it and does what they like with it.

---

## 7c. The Calendar (read model)

### 7c.1 Entries

`ShelterCalendar.Upcoming(day, horizon)` merges, with provenance, rows from:

| Source | Row |
|---|---|
| `SeasonalCelebrationSystem.CheckHolidayForDay` | holiday (trigger day, cost, allowed activities) |
| anniversary types | founding (`ShelterArchiveSystem.FoundingDay`, verified §2b), fallen (each death's day + 365k), milestone |
| `ShelterFestivalEngine` | scheduled or active festival |
| `CeremonySystem` | scheduled ceremony |
| MK | death anniversaries; Founding Day Name Reading |
| Customs | "Stool Night is due" is *not* a row — customs do not schedule; the Calendar never becomes a planner for them |

Rows sort by day, then by kind (holiday, anniversary, festival, ceremony, reading).

### 7c.2 One remembrance, three owners (finding 2)

The festival type `RemembranceVigil`, the ceremony `ceremony_remembrance_vigil` and the anniversary `anniv_fallen` overlap in concept. The Calendar shows each **with its provenance tag** (`festival`, `ceremony`, `anniversary`) and a shared **"Remembrance"** family chip. When two rows fall on the same day the Calendar folds them into one row with two provenance tags. **No owner is edited to reconcile them**; if P0 finds the three are meant as one system, that is recorded in `KNOWN_DEBT` by the integrator, not fixed here.

### 7c.3 Cost preview (Thin Larder)

`FestivalCostPreview.For(row)` reads the owner's declared costs (holiday `food_cost`/`fuel_cost` × the chosen scale's `cost_multiplier`; festival `RequiredCommodities`; ceremony item requirements) and the present stores from the existing inventory owner, and returns:

`{ cost_food, cost_fuel, cost_items[], days_above_floor_after, floor_breached, scale, memorable }`

Rules:

- **T-1** the preview never *charges*; charging stays inside `TryHoldCelebration`, `TryCommenceFestival`, `ScheduleCeremony`.
- **T-2** `days_above_floor_after` uses the existing daily consumption read (`Days of food` from the food owner); if unavailable the field is `null` and the line says *"cannot say how long the larder lasts."*
- **T-3** the preview line is data (`evening_calendar_notes.json`), chosen by `floor_breached` and scale.
- **T-4** `is_memorable` scale is shown as a plain note ("this one will be remembered"), not as a bonus figure.

### 7c.4 Worked example — Harvest Observance

Day 190. Holiday `hol_harvest_observance` (authored cost read from the row, e.g. food 6, fuel 3 at scale `small`); the player considers scale `large` (cost_multiplier 2.0 → food 12, fuel 6). Stores: food 96, daily use 8 → 12 days; after: 84 / 8 = 10.5 days. Floor (authored 7 days) not breached. Line: *"The feast will cost 12 food and 6 fuel. After it, the larder lasts ten and a half days, three and a half above the floor."* (Floor and tone lines authored; numbers read.)

---

## 8. Evening activity notes (authored prose layer)

Each practice family has a small bank of **evening lines** (2–4 per family per warmth band) used by the strip and the briefing. They describe what people did, in plain physical detail. Examples (candidates):

- `stories`, Warm: "Somebody told the one about the generator again, wrong on purpose."
- `music`, Thin: "The harmonica got through half a tune and everyone clapped at the right place."
- `games`, Alight: "House rules on the cards are now longer than the original rules."
- `craft`, Warm: "Three people whittling and nobody speaking, which is a kind of talking."
- `quiet`, Cold: "One lamp on, one person reading, everyone else asleep by choice."
- `body`, Warm: "Morning stretch, six of them, and the sixth was only there for the noise."
- `holiday`, Alight: "The long table ran the length of the mess. Someone brought their own chair."
- `ritual`, Thin: "Two taps on the outer iron. Nobody said what for."

**Rule V-1:** lines describe **people and things**, never the shelter's mood as a mind; lines never state a number that the surface already shows.


---

# PART TWO — MEMORY WORK

## 9. Remembrance (one record per death, only what the household did)

### 9.1 Stored shape (additive nested DTO — no new save section)

```csharp
public sealed class RemembranceState
{
    public int SchemaVersion = 1;
    public Dictionary<string, RemembranceRecord> ByDeceased = new();   // survivor id -> record
    public List<ReadingRecord> Readings = new();
    public List<string> ChronicleKeys = new();
}
public sealed class RemembranceRecord
{
    public string RiteId;               // one of memorial_rites.json, or null if none chosen
    public int    RiteDay = -1;
    public string KeeperId;             // survivor keeping the name; null if none
    public int    KeeperSinceDay = -1;
    public int    LastSaidDay = -1;     // last day the name was spoken in a reading or rite
    public int    TimesSaid;
    public List<int> AnniversariesObserved = new();   // year numbers 1, 2, ...
    public List<string> ThingsLeftGiven = new();      // heirloom/effect ids handed on
    public int    BunkReassignedDay = -1;
}
public sealed class ReadingRecord
{
    public int    Day;
    public string Occasion;             // "anniversary" | "founding_day" | "player"
    public List<string> NamesRead = new();
    public string ReaderId;
}
```

Home: nested in `MemorialSave` (or the celebration save) per P0 (DEC-MK-02). A legacy save without the field loads as *no records* and every memorial, vigil and grief behaviour equals today's. **Death facts (cause, day, epitaph, eulogy, morale delta, quality) stay in `MemorialEntry`** and are never copied.

### 9.2 Rules

- **R-1** A record is created lazily, the first time the household *does* something for a name (a rite, a keeper, a reading). A death with no record is valid: it means nothing was done yet.
- **R-2** `RiteId` is set at most once per death (the rite is a choice with a record, DEC-MK-04).
- **R-3** `LastSaidDay` and `TimesSaid` change only through real acts: a rite, a reading, an explicit "say the name" command.
- **R-4** No record is ever deleted. Names cannot be removed from the wall by any code path in this plan.
- **R-5** Survivors who have died can keep a name (their own record persists) but are not eligible as Keeper for new records.

---

## 10. Grief Season and Held/Quiet (derived)

### 10.1 Grief Season

A pure function of `days_since_death`, whether `Mourn` was held (`MournedDay`), and whether a rite was chosen:

| Season | Rule |
|---|---|
| **Raw** | `days_since_death < 7`, **or** unmourned and `days_since_death < 21` |
| **Heavy** | `7 <= days_since_death < 30` (mourned), or unmourned and `21 <= days_since_death < 60` |
| **Settling** | `30 <= days_since_death < 120` |
| **Carried** | `days_since_death >= 120` |

Thresholds are data (`remembrance_lines.json: seasons`), tunable (DEC-MK-05). An **unmourned** death stays a season behind: the wall shows *"the vigil has not been held"* as a plain line, never as a nag. The season is a **weather report**, not a meter; it carries no numeric consequence in this plan (all grief arithmetic remains in `RelationsGriefSink`).

### 10.2 Held and Quiet

Derived from `LastSaidDay`:

| State | Rule |
|---|---|
| **Held** | `TimesSaid >= 1` and `day - LastSaidDay <= 120` |
| **Quiet** | otherwise (never said, or not said for more than 120 days) |

A name that has never been said is **Quiet from the start** — the wall shows it plainly, not as a fault. **Quiet is not gone**: a single act (rite, reading, "say the name") makes it Held again. The 120-day window is data (DEC-MK-06).

### 10.3 Why both

Grief Season answers *"how near is this loss?"* — a function of time. Held/Quiet answers *"is this person still spoken of?"* — a function of what the household chose to do. A death long ago can be **Carried** and **Held**; a recent death can be **Raw** and **Quiet** (nobody has yet been able to say the name). The two states together are the whole of the wall's emotional texture.

### 10.4 Worked example — three names on the wall, day 200

| Name | Died | Mourned | Rite | Last said | Season | State |
|---|---|---|---|---|---|---|
| Marek | day 58 | day 60 | roll-call | day 190 (reading) | Carried (142 days) | Held |
| Ambrose | day 121 | day 123 | empty bunk | day 130 | Settling (79 days) | Held (70 days since said) |
| Ines' brother | day 186 | not held | none | never | Raw (14 days, unmourned) | Quiet |

The third row reads: *"Nobody has said his name. Nobody has tried."* The first two show the point of having both states: Marek is far away in time and still spoken of; Ambrose is near and already being said.

---|---|---|---|---|---|---|
| Marek | 58 | day 60 | roll-call | 190 (reading) | Carried (142 days) | Held |
| Ambrose | 121 | day 123 | empty bunk | 130 | Settling? days=79 → Settling | Quiet (70 days: ≤120 → **Held**) |
| Ines' brother | 186 | not held | none | never | Raw? days=14, unmourned → Raw | Quiet |

Correction of the middle row under the rule: Ambrose died day 121; on day 200, `days = 79` → **Settling**; `LastSaidDay = 130` → 70 days ago → **Held**. The row reads: Settling · Held. The third row, unmourned at day 14, is **Raw** and Quiet: *"Nobody has said his name. Nobody has tried."*

---

## 11. Rites (existing catalog, one additive choice route)

### 11.1 The six authored rites (read from `memorial_rites.json`)

| Rite id | Type | What it is | Body needed |
|---|---|---|---|
| `memorial_rite_roll_call_naming` | roll_call | the name said at morning muster | no |
| `memorial_rite_empty_bunk_night` | empty_bunk | the bunk left untouched for a night | no |
| `memorial_rite_division_of_effects` | effects | personal effects divided among the living | no |
| `memorial_rite_work_gang_farewell` | work_gang | the dead one's crew gives a farewell | no |
| `memorial_rite_wall_tally_engraving` | wall_tally | a mark cut into the wall | no |
| `memorial_rite_last_wish_committal` | last_wish | the last wish carried out | offered only when a wish is known (§2b) |

Each row already carries `grief_reduction_multiplier` and `guilt_relief_amount`; **the numbers stay with the rite catalog and its coordinator** — MK reads them for the preview and applies nothing itself.

### 11.2 The choice route (resolved, §2b)

The coordinator **already exposes** per-death rite selection (`PerformMemorialRite(deceasedId, riteId, day)`, verified §2b). MK therefore adds **no command**: the Memorial Book calls the existing method with the player's chosen rite and subscribes to `OnMemorialRitePerformed` to write `RiteId`/`RiteDay`. The current hard-coded call in `Main.Campaign.cs` (roll-call naming after the wall's vigil) stays as the **default** when the player chooses nothing, so no death ever goes unmarked. If a rite has been performed already for that death the method's arc state is the truth and the Book only reads it.

### 11.3 Rite preview lines

Each rite has a **preview line** shown before choosing (data, `remembrance_lines.json`): one plain sentence about what the household will do, never about how anyone will feel.

- Roll call: "At muster tomorrow his name is read, and the line stands quiet for three seconds."
- Empty bunk: "The bunk stays made for a night. Nobody sleeps in it."
- Division of effects: "What he owned is divided by those who knew what he used."
- Work-gang farewell: "His crew says the work part of it. They are not good at the other part."
- Wall tally: "A mark is cut into the wall where the others are."
- Last wish: "What he asked for is done, if it can be."

### 11.4 Rules

- **RI-1** A rite may be performed at most once per death by the player (R-2); the coordinator's own cooldowns still apply.
- **RI-2** A rite requiring a recovered body is offered only when the death record says a body was recovered (the field exists on the rite).
- **RI-3** The rite choice is **not** gated by Grief Season; the player may choose on day 300 what they did not choose on day 3. The record shows the day it was chosen.

---

## 12. Keepers of the Name

### 12.1 What a keeper is

A **Keeper of the Name** is a survivor the household (or the player) has named to keep a death's memory: to say the name at readings, to tend the bunk or shelf, to carry the last wish. It is a **human act, stored** (`KeeperId`, `KeeperSinceDay`).

### 12.2 Rules

- **K-1** One keeper per name; a survivor may keep up to 3 names (bounded).
- **K-2** The player assigns; a **suggested keeper** is offered from the deceased's relationship data (the alive `RelatedIds` member with the highest `affinity`, via `SurvivorRelationsSystem.RelatedIds` and `TryGetRelationship` — verified §2b; ties by id).
- **K-3** A keeper who dies leaves the name **unkept**, with a chronicle line, and the name shows "no keeper" until reassigned.
- **K-4** A keeper who is far away (expedition) is still the keeper; a reading may be held by someone else.
- **K-5** Keepers add **no mechanics** in v1. They shape the *tone* of the reading line (a bonded keeper gets a warmer line; a stranger a plainer one), through the data selection only.

### 12.3 Keeper lines (candidates)

- Bonded keeper reading: "Ines read her brother's name and did not stop after it. Nobody asked her to."
- Stranger keeper reading: "Tomas read the name evenly. He had not known the man. He read it as if he had."
- No keeper: "The name is on the wall. No one has said it in a long time."

---

## 13. The Name Reading

### 13.1 Occasions

A **Name Reading** is a short ritual that says a set of names aloud. Occasions:

| Occasion | Trigger | Names read |
|---|---|---|
| **Anniversary** | the year-mark of a death (day + 365k) | that name (and any others within 3 days) |
| **Founding Day** | the shelter's founding anniversary (the existing `anniv_founding`) | every name on the wall, oldest first |
| **Player** | the player chooses, any day | any chosen names |

### 13.2 Effect

A held reading sets `LastSaidDay = day` and `TimesSaid++` for each name read (so Quiet names become Held), appends a `ReadingRecord`, writes one chronicle line per reading (`reading:<day>:<occasion>`), and is offered to the Evening Ledger as a `reading` practice (§5.1) so it can help form a Custom. **Morale/grief effects, if any, come from the existing owners** (the anniversary type `anniv_fallen` already has a `base_morale_boost` through `CommemorateAnniversary`; the reading *calls* that owner path, it does not add its own).

### 13.3 Time cost

A reading takes the evening. It costs **no items**, and requires at least one healthy survivor present. A Founding Day reading of *n* names takes `ceil(n/8)` evenings only as a **narration** line ("It took four minutes"); mechanically it is one evening.

### 13.4 Worked example — Founding Day, year 1

Wall has 9 names. Player holds the reading on day 365. Names read oldest first. 4 were Quiet; all 9 become Held. Chronicle: *"Year one. We read the names. It took four minutes. It felt longer than it was."* The `reading` practice joins the ledger for day 365; if a `reading` Custom template exists (DEC-MK-07, optional) and the second reading happens in year 2, *"The Reading"* forms as a Custom.

### 13.5 Reader

The reader is the assigned reader (a survivor) or the player's chosen survivor; default is the survivor with the highest bond to the most names, read from the relationship owner. A reader who cannot attend is replaced by the next; if no one can attend the reading is **not held** and the Calendar row remains due with the note "no one was free."

---

## 14. Things Left

### 14.1 What they are

**Things Left** are the personal effects and heirlooms a death leaves: the existing `MemorialEntry.HeirloomItemId` and `HeirloomRecipientId`, plus the authored effects from `remembrance_things_left.json` (a mug, a folded shirt, a hand-drawn map, a half-finished carving). They sit on a **bounded shelf** (≤ 12 items) in the memorial surface.

### 14.2 Rules

- **TL-1** An effect is *given* through the existing inventory command; MK records the giving (`ThingsLeftGiven`) and writes one chronicle line.
- **TL-2** Giving an effect to the deceased's Keeper or closest bond changes only *text*; any morale echo already exists in the heirloom owner.
- **TL-3** An effect can be **kept on the shelf** indefinitely. Nothing decays.
- **TL-4** A shelf at 12 items refuses new authored effects with a plain line; the *heirloom owner's* items are unaffected.
- **TL-5** No effect names a real place, person or copied text (M-6).

### 14.3 Authored effects (excerpt)

| Id | Effect | Line |
|---|---|---|
| `tl_mug_chipped` | a chipped mug | "A mug with a chip on the handle side. He held it from the other side." |
| `tl_map_pencil` | a pencil map | "A map of the east corridor, drawn from memory, mostly right." |
| `tl_carving_half` | a half-finished carving | "A bird, one wing done. The other is a suggestion." |
| `tl_scarf_grey` | a grey scarf | "Long enough for two turns and a knot he never learned." |
| `tl_ration_card` | an unspent ration card | "Not used. Kept. There is a difference." |
| `tl_key_bent` | a bent key | "A key for a lock nobody has found." |

---

## 15. Bunks, rooms and the ordinary afterward

A death frees a bunk. The **bunk reassignment** is an existing shelter-assignment act; MK records `BunkReassignedDay` when the bunk is reassigned and writes one line — gently, once. If the reassignment happens before the empty-bunk rite is performed, the rite becomes unavailable for that death with a plain reason ("the bunk has a new sleeper"). The plan never forces either order; it *records* the order. That record is the whole of the "ordinary afterward."

Worked line: *"The bunk was made on day 91. On day 118 Ines asked for it. Ambrose's sister said yes."*

---

## 15b. Texture — the wall

**The wall is a place, not a screen.** In the memorial surface each name sits under a small line: *Carried · Held · kept by Ines.* A Quiet name is drawn a shade fainter, never struck through. The Founding Day reading briefly lights every name.

**Small cruelties (kept small).** When a death is unmourned and a festival is held, the memorial surface shows: *"Marek's vigil has not been held."* When a keeper dies and the name has no other keeper: *"No one is keeping his name."* Nothing else happens. The player sees it and does what they like with it.

**What the shelter never does.** It never pressures. It never awards a bonus for mourning "properly". The rites have their own catalog numbers; the player may choose none. A shelter that never reads a name is a shelter — a plainly recorded one.

**The last blank line.** The wall tally engraving has a last, blank line. It is blank on purpose (§19).


---

# PART THREE — DEPTH, CATALOGS, HOOKS, ACCEPTANCE, DELIVERY

## 15c. A year of evenings and losses (one worked timeline)

Fixture: 8 survivors, default difficulty, a shelter founded day 1. All numbers below are *ledger facts*; morale, stress and grief values remain with their owners.

| Day | Event | Stored change | Player sees |
|---|---|---|---|
| 1 | new year holiday held, small scale | ledger: `holiday` on day 1 | "New Year Dawn: quiet, solemn." |
| 12 | storytelling in mess hall (4) | ledger only | strip icon |
| 16 | storytelling (5) | ledger only | strip icon |
| 19 | storytelling (5) | Custom `custom_story_night` forms, coined "Stool Night" | chronicle line |
| 30 | harmonica (2), quarters | ledger only | strip icon |
| 58 | Marek dies | `MemorialEntry` (existing owner) | wall: Marek, Raw, Quiet |
| 60 | player holds the vigil (`Mourn`) | `MournedDay = 60` (existing) | Raw to Heavy path |
| 61 | player chooses roll-call rite | `RiteId`, `RiteDay = 61`, `TimesSaid = 1`, `LastSaidDay = 61` | wall: Held |
| 62 | keeper named: Ines | `KeeperId = ines` | "kept by Ines" |
| 70 | 21 days no story night | Custom lapses (only if 21 days pass) | chronicle line |
| 90 | spring thaw: player skips (thin larder) | celebration owner records skip | Calendar: "skipped" |
| 121 | Ambrose dies | entry | Raw, Quiet |
| 123 | vigil | `MournedDay` | -- |
| 124 | empty-bunk rite | `RiteId`, Held | -- |
| 148 | bunk reassigned | `BunkReassignedDay = 148` | "The bunk was made on day 91..." (variant) |
| 181 | Marek: 120 days since said (61) | derived: Quiet | wall fainter |
| 186 | Marek's anniversary is day 423, not yet | -- | -- |
| 190 | harvest observance held, large | ledger `holiday` | strip |
| 200 | Founding day is day 365 | -- | -- |
| 365 | Founding Day reading | `Readings += 1`, all names `LastSaidDay = 365` | all Held, chronicle line |
| 423 | Marek's anniversary | reading offered; held | Marek: Carried, Held |

(Day numbers are illustrative; P0 fixes the founding-day reference: the existing `anniv_founding` day source.)

---

## 15d. Interaction with existing owners (what reads what)

| Interaction | Direction | Note |
|---|---|---|
| Hobby sessions to Ledger | owner to EV | read `PracticesOn(day)` |
| Holiday held or skipped to Ledger/Calendar | owner to EV | `WasHolidayOccurrenceHeld/Skipped` |
| Festival/ceremony to Calendar | owner to EV | read only |
| `OnMemorialized` to MK | owner event to MK | may prompt a rite choice surface |
| `OnMourned` to MK | owner event to MK | updates Grief Season inputs (derived) |
| Rite performed to MK | coordinator event to MK | writes `RiteId` |
| Reading to Ledger | MK to EV | `reading` practice |
| `CommemorateAnniversary` | MK calls owner | morale via owner, not MK |
| Keeper death | roster event to MK | writes chronicle line once |
| Heirloom given | inventory event to MK | writes `ThingsLeftGiven` |

No owner changes behaviour. Every arrow is a read or an event subscription; the only writes are to the two stored states.

---

## 15e. Edge cases and rules

- **X-1** A survivor dies with no relationships: the wall entry exists; Keeper suggestion is empty; the reading still includes the name.
- **X-2** Two deaths on the same day: both entries exist; a reading names both; rites are chosen separately.
- **X-3** Shelter population 1 (the last survivor): Reach in Warmth is 1.0 or 0 by the formula; the Evenings surface shows *"One at the table."* — no special mechanics.
- **X-4** A festival scheduled while the shelter is under siege or crisis: the owner decides whether it can commence; the Calendar shows the row with the owner's refusal reason.
- **X-5** A survivor leaves (not dies): no memorial entry; no Remembrance record; this plan does nothing.
- **X-6** Difficulty presets never scale Custom thresholds, Warmth weights, season thresholds or reading behaviour.
- **X-7** Save bounds: ≤ 24 Custom records, ≤ 60 ring entries, one Remembrance record per memorial entry, ≤ 40 Reading records (oldest are summarised into a count when exceeded, names retained in `MemorialEntry`).
- **X-8** Determinism: the only randomness is coined names for Customs and epitaph selection (existing); both go through named `CampaignStreamIds` forks.
- **X-9** A death by dissolution or a lost expedition with no body: rites requiring a body are unavailable; the others remain.

---

## 15f. Presentation spec (no new routed panel)

**Evenings tab** (`SurvivorDowntimePanel` or `CeremonyFestivalPanel`, chosen at P0):

```
EVENINGS                                              Day 200
Hearth: WARM     14-evening strip:  S . M S G . S  ...
Customs: Stool Night (kept 9x)   The Tune Hour (lapsed)
CALENDAR
  d205  Harvest Observance   cost food 12, fuel 6   larder after: 10.5 days
  d212  Marek — anniversary (Reading)
TONIGHT   [Hobby session v]  [Hold a reading]  [Skip]
```

**Remembrance tab** (existing memorial wall surface):

```
THE WALL
Marek       Carried · Held    kept by Ines     rite: roll call     [Say the name]
Ambrose     Settling · Held   kept by —        rite: empty bunk    [Assign keeper]
(Name)      Raw · Quiet       vigil not held   [Hold the vigil]  [Choose a rite]
THINGS LEFT (3 of 12)   [Give ...]
```

Every control is keyboard and controller reachable with visible focus; Back closes the tab; controls meet the a11y height floor already in Core; state is never conveyed by colour alone (band names and words are always shown).

---

## 16. Catalog specs and validator rules

| File | Rows | Key fields |
|---|---|---|
| `evening_customs.json` | 8 templates, `family_map` (16 hobby ids), `warmth` weights/bands, `coined_names` (6 families x 3–5) | `template_id`, `family`, `n`, `window`, `min_participants`, `default_name`, `lapse_days` |
| `evening_lines.json` | ~64 (8 families x 4 bands x 2) | `family`, `band`, `text` |
| `evening_calendar_notes.json` | ~24 | `kind`, `floor_breached`, `scale`, `text` |
| `remembrance_lines.json` | ~48 | `kind` (`season`, `held`, `quiet`, `rite_preview`, `keeper`, `reading`, `bunk`), `key`, `text` |
| `remembrance_keepers.json` | ~12 | `bond_band`, `text` |
| `remembrance_things_left.json` | 18 | `id`, `label`, `line` |
| `name_reading_scripts.json` | ~12 | `occasion`, `size_band`, `text` |

**Validator rules:**

- **E-1** every custom template `family` is in the closed family list; `n <= window`; `min_participants >= 1`.
- **E-2** every hobby id in **both** hobby catalogs appears in `family_map` (a new hobby cannot silently fall out).
- **E-3** `coined_names` has 3–5 unique names per family, each 3–24 chars.
- **E-4** warmth weights sum to 100; band cuts strictly increasing.
- **E-5** every evening line has a known family and band; at least 2 per pair.
- **E-6** every calendar note references a known `kind`; both `floor_breached` values covered for every kind.
- **K-1** season thresholds strictly increasing; `held_window_days > 0`.
- **K-2** every rite id in a preview line exists in `memorial_rites.json`; every one of the 6 rites has a preview.
- **K-3** every keeper line has a `bond_band` in {bonded, known, stranger, none}.
- **K-4** things-left ids unique; labels <= 32 chars; lines <= 160 chars.
- **K-5** all text rows <= 200 chars; no placeholder tokens; no real places, factions or people (tone rule).
- **K-6** tone deny-list: no line attributes intent, mood or memory to the *shelter*, a machine or the wall (only to people); a small verb deny-list is checked (`wanted`, `remembered`, `mourned`, `knew`, `forgave`) unless the subject is a survivor.

---

## 17. Cross-plan hooks (ship dark; `Null*` defaults)

| Hook | Direction | Default | Purpose |
|---|---|---|---|
| `IEveningSource` | owners to Ledger | `NullEveningSource` | one adapter per owner; Core stays engine-free |
| `IRemembranceSink` | MK events | no-op | rite, reading and keeper events into the chronicle |
| `IBondReader` | relationship owner to MK | `NullBondReader` | keeper suggestion and reader choice |
| `IFoodFloorReader` | food owner to preview | `NullFoodFloorReader` | Thin Larder |
| Works Below / Machine hooks | Machine in the Walls may cite a keeper's death | none | sister plan; read-only, no shared state |
| Long Siege | a siege year may mark Founding Day reading as impossible | none | owner decides; no new state |
| Year Two chapter profiles | may unlock template sets | none | data only |
| Other Beginnings (plan 8) | a start may pre-seed a Custom or a wall entry | none | additive, guarded |

No hook changes another plan's authority; every hook is optional and the plan is complete with all defaults.

---

## 18. Acceptance criteria

**EV**

- **EV-A1** The ledger, for every fixture week, lists exactly the practices the owners recorded (parity test across the two hobby catalogs, holidays, festivals, ceremonies).
- **EV-A2** Every hobby id in both catalogs maps to a family (validator E-2).
- **EV-A3** Custom formation follows the template rule, is deterministic (coined name stable across calls and load), and is idempotent in the chronicle.
- **EV-A4** Lapse and revive follow §6.3; `FormedDay` and `TimesKept` survive revival.
- **EV-A5** Warmth matches the worked example to one decimal; band edges tested; early-shelter "Too early to say" holds.
- **EV-A6** The Calendar merges overlapping remembrance rows with provenance; no owner is mutated.
- **EV-A7** The cost preview never charges; `floor_breached` and `null` days-above-floor paths tested.
- **EV-A8** Round-trip save; legacy save loads with no Customs; existing festival, ceremony, celebration and hobby tests pass unchanged.

**MK**

- **MK-A1** Grief Season table (§10.1) holds for every boundary, mourned and unmourned (parametrised).
- **MK-A2** Held/Quiet follows §10.2; a Quiet name becomes Held after one act; no code path deletes a record.
- **MK-A3** Rite choice records once per death; a second attempt is refused with an explicit result.
- **MK-A4** Keeper rules K-1..K-5 hold, including keeper death.
- **MK-A5** A reading updates `LastSaidDay`/`TimesSaid` for every name, writes one chronicle line, and calls the existing anniversary owner rather than adding morale.
- **MK-A6** Things Left: giving records once; shelf bound holds.
- **MK-A7** Tone deny-list rejects a crafted bad line (K-6).
- **MK-A8** Round-trip save; legacy save loads with no records; existing memorial, vigil and grief tests pass unchanged.

---

## 19. Open Mysteries and Deliberate Silence

- Who leaves the outside crumb is never stated.
- Why the harmonica is half-learned is never explained.
- Whether the first Founding Day reading includes those who sealed the doors is left unwritten; the reading begins after the first name.
- The last line of the wall tally is blank on purpose and is never filled by code.
- What happens to a Quiet name that nobody ever says again is left to the player; the wall keeps it.
- Whether a Custom belongs to the household or the household belongs to it is never asked.

---

## 20. Packages

| Pkg | Scope | Depends |
|---|---|---|
| P0 | Premise audit: E8, E12, E14, E15; choose save homes; record findings (rite route, three remembrance owners, per-day records) | none |
| P1 | Catalogs and loaders: evening + remembrance JSON, validators E-1..E-6, K-1..K-6 | P0 |
| P2 | `IEveningSource` adapters + `EveningLedger` (two hobby catalogs, holidays, festivals, ceremonies) | P1 |
| P3 | `EveningCustoms` (formation, naming, lapse, revive) + save DTO | P2 |
| P4 | `HearthWarmth` + `ShelterCalendar` + `FestivalCostPreview` | P2 |
| P5 | Evenings surface + briefing line (`INT`) | P3, P4 |
| P6 | `RemembranceState` + Grief Season + Held/Quiet + save DTO | P0, P1 |
| P7 | Rite route (read or one additive command, `INT`) + Keepers | P6 |
| P8 | Name Reading + Things Left + memorial surface (`INT`) | P7, P3 |
| P9 | Save parity, legacy load, prose and tone review pass, handoff | all |

EV (P2..P5) and MK (P6..P7) can proceed in parallel after P1; P8 is the join (readings feed the ledger).

---

## 21. Decision register (all unsigned; foreman/user signature required)

- **DEC-EV-01** The Evening Ledger is derived from owner records. *Recommend yes.*
- **DEC-EV-02** A bounded 60-evening ring is stored only if P0 finds an owner keeps no dated record. *Decide at P0.*
- **DEC-EV-03** Customs and Warmth have **no mechanical echo** in v1 (option: a bounded echo through the existing culture-bonus seam). *Recommend no echo.*
- **DEC-EV-04** Template thresholds (N in W, P). *Tunable data.*
- **DEC-EV-05** Lapse window 21 days. *Tunable data.*
- **DEC-EV-06** Warmth weights 40/25/20/15 and band cuts 25/50/75. *Tunable data.*
- **DEC-EV-07** The two hobby catalogs are not merged by this plan (separate debt item).
- **DEC-EV-08** No new routed panel; extend existing surfaces.
- **DEC-EV-11** Remembrance is one owner's evening, not a fourth: festival engine `RemembranceVigil`, the seasonal "Remembrance of the Fallen" and the memorial wall vigil overlap, so the plan picks one to *host* the evening and asks the others' hosts not to double-fire. *Recommend the festival engine hosts; the wall vigil stays the default rite.*
- **DEC-MK-01** Tone rule: the people supply the character; nothing attributes mind to the shelter or wall. *Binding.*
- **DEC-MK-02** Save home for Remembrance: nest in the `memorial` section. *Resolved at pass 1 (§2b).*
- **DEC-MK-03** No forgetting-as-deletion; Quiet is a display state. *Recommend yes.*
- **DEC-MK-04** Rite choice is once per death. *Recommend yes.*
- **DEC-MK-05** Grief Season thresholds 7/30/120 (unmourned lags). *Tunable data.*
- **DEC-MK-06** Held window 120 days. *Tunable data.*
- **DEC-MK-07** Optional `reading` Custom template. *Recommend yes, off by default.*
- **DEC-MK-08** No new routed panel; extend the memorial wall surface.
- **DEC-MK-09** ~~One additive rite command~~ *Resolved at pass 1: not needed; `PerformMemorialRite` already takes a rite id per death (§2b, §11.2).*
- **DEC-MK-11** Fixed-sentence Book lines use chronicle keys; composed lines use `JournalSystem.TryAddRawEntry`, following the existing vigil precedent.

---

## 22. Test plan (focused; `bin/run-scoped-tests`)

- Ledger: one parametrised parity test over the source owners and both hobby catalogs.
- Family map: every hobby id in both catalogs maps (catalog-level, row failure output).
- Customs: formation, coin determinism across call order, lapse, revive, rename sanitising.
- Warmth: the worked example, band edges, early-shelter path, population 1.
- Calendar: merge of three remembrance owners; cost-preview paths (breach, null).
- Grief Season: parametrised boundaries, mourned and unmourned.
- Held/Quiet: transitions and no-delete guarantee.
- Rite: once per death; requirements; coordinator route (read or additive) as discovered at P0.
- Keeper: assignment, cap of 3, keeper death.
- Reading: names updated, chronicle idempotence, owner call not duplicated.
- Things Left: give once; shelf bound.
- Tone validator: deny-list rejects a crafted line.
- Save: round-trip and legacy load for both DTOs; parity guard runs the existing festival, ceremony, celebration, hobby and memorial tests unchanged.

Target 35–45 focused tests; no duplicate wording variants; one test per behaviour.

---

## 23. Risk register

| Risk | Likelihood | Mitigation |
|---|---|---|
| Three remembrance owners collide in the Calendar | Med | Fold with provenance; debt note to the integrator; never edit owners |
| Two hobby catalogs drift | Med | Validator E-2 covers both; no merge here |
| Customs feel like homework | Med | No schedule, no penalty; lapse is a line, not a loss |
| Mourning feels gamified | Med | No bonuses; tone deny-list; rites carry only catalog numbers |
| Thin Larder reads as a nag | Low | Preview only; command never blocked |
| Save bloat | Low | Bounded lists (X-7) |
| A second "observed" flag appears | Low | One Remembrance record; one Calendar |
| Rite route needs a new authority | Med | Stop condition; one additive command only, else read-only |

---

## 24. Expansion backlog

- Guests at readings (faction visitors, from the ceremony invitation seam).
- Generational customs (a child raised into a Custom).
- Seasonal customs (winter-only).
- A shelter songbook built from heard songs.
- Grief over non-human losses (a dog, a garden).
- The Book of Days as a printed artifact (paper-making chain).
- Memory of the *place* (a sealed corridor mourned).
- Player-authored rites (a small text field, sanitised).
- Cross-shelter memory (a treaty partner reads your names).

---

## 25. Pre-flight, verification and stop conditions

**Pre-flight (before any edit):** read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md`; confirm claims for every path in section 4; complete P0 and record the findings.

**Verification:** run only focused targets via `bin/run-scoped-tests` for the changed files; run `bin/ashfall-dev validate-config` for the new catalogs; a Godot headless check only if a panel or briefing wiring is touched (15 FPS); never the full suite without `RUN FULL TESTS`.

**Stop and report to the foreman if:**

- a path in section 4 is claimed by another owner;
- P0 shows the rite choice can only be delivered by a new authority;
- an owner keeps no dated record *and* the host has no credit hook to feed the ring;
- any change would alter a morale, stress, cost or grief value;
- any line would attribute mind or memory to the shelter, the wall or a machine.

**Handoff:** outcome, files, contract, commands and results, limitations, shared paths intentionally untouched, per `AI_AGENT_WORKFLOW.md`. On integration, mark this file FULLY INTEGRATED at the top (multiple times) and move it to `.ai/plans/integrated/<category>/`.
