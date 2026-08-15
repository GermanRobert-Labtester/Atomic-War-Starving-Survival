# ASHFALL — Expansion Design Bible

**Title:** ASHFALL: THE DOSE
**Internal id:** `expansion_the_dose`
**Pack number:** **07** — after 06 (The Muster). This does not reopen the Muster, the coast, the bunker wings, or the Crossing.
**Status:** Design bible for review. No game data has been edited. No C#.
**All new ids below are PROPOSED** unless marked *existing*.
**Tone lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.
**Sister packs:** Exp 1 `expansion_the_holdfast` (the allocated world). Exp 2 `expansion_the_duty_roster` (the unlisted home). Exp 3 `expansion_the_standing_record` (the ground). Exp 4 `expansion_nobodys_charter` (who speaks for whom). Exp 5 `expansion_year_of_ash` (the long months). Exp 6 `expansion_the_muster` (the muster). This pack is **the debt the body owes — and who keeps the books.**

---

# I. EXECUTIVE SUMMARY & SCOPE BOUNDARY

Every survivor carries a lifetime dose. The bunker only measures it when someone bothers a dosimeter against them. Most shelters stop caring after the first counting. This pack makes the dose a **document** — read, recorded, disputed, and finally paid — across the whole long winter, not a death-clock.

### The Accounting approach — how this document gives the player multiple ways to shape the dose

One theme (cumulative radiation) is served by four human registers, and the player may lean on any of them or refuse all four:

| Register | What it is | What the player writes by using it |
|---|---|---|
| **The Dose Ledger** | Cumulative mSv per survivor, corrected at every exposure event | Who counts, and how carefully |
| **The Sick List** | Chronic-illness prognosis bands (fatigue, marrow, lungs) | Who receives the last of the medicine |
| **The Cohort** | Children conceived after the exchange — the second generation's baseline | What the first generation chooses to hand down, rads and all |
| **The Voluntary Register** | Survivors who offer themselves for high-dose surface work | Who is allowed to spend the days they have left |

The four registers are not a mini-game. They are paperwork. Choosing to *not* keep a register is also a choice, and the game records that silence.

### What already exists and is NOT duplicated here

- **Radiation accumulation** is already modelled (`RadiationSystem`, `RadiationPhaseProgression`). This pack does not retune it.
- **Chronic-illness traits** (`ActiveChronicIllness`, `HasChronicIllness`) are already fields. This pack reads them, and gives the host a reason to.
- **Lorekeeper / record-keeping** lives in Journal + Duty Roster + Standing Record. This pack is *not* a fourth journal; it is the medical ledger the other three never opened.
- **Dosimeter / geiger counter** are existing items. This pack connects them to a *specific survivor* and a *running total*, which nothing does today.
- **Iodine pills, anti-rad, rad-away** are existing consumables. This pack gives them a target (a named survivor in a dose band) instead of a generic morale effect.

### What is genuinely new in this document

1. **`DoseLedgerSystem`** — per-survivor cumulative dose, corrected by exposure events and attenuated by shielding/anti-rad, persisted and checksummed.
2. **`SickListSystem`** — prognosis bands from cumulative dose, driving care assignments rather than deaths.
3. **`CohortSystem`** — the second generation's inaccurate, self-contradicting baseline.
4. **`VoluntaryRegisterSystem`** — a named, signed decision that spends high-dose surface labour.
5. **Four quest lines** that never tell the player whether they did the right thing.

---

# II. ANALYSIS — WHY RADIATION IS ALREADY A SPECTRE AND NOT A DOCUMENT

### The gap this pack closes

Today, a survivor's radiation is a number that rises and, occasionally, a phase label. It is not **remembered**. A scanner read on Day 40 and a read on Day 220 are two unconnected facts unless a human writes them in the same ledger. The bunker has journals (personal), a roster (home), a gazetteer (ground), a charter (voice). It has **no dose ledger** — the one document radiation makes possible and *demands*.

Theatrically, this is ASHFALL's strongest asset: **paper is scarce, ink is scarce, but the body keeps taking hits whether or not anyone writes them down.** The player is not asked to stop the rads. The player is asked to *keep count*, and to decide what a count is worth when the next exposure is a child's or a willing volunteer's.

### Previous pack debt that this pack discharges

| Prior thread | This pack's recast |
|---|---|
| `ActiveChronicIllness` on `Survivor` was read by affliction checks but had no origin story | The Sick List becomes the origin: the dose band is the cause |
| Dosimeters existed as trade goods with a generic effect | The Dose Ledger is the only surface that makes *whose* dosimeter matter |
| Radiation phases (Prodromal→Manifest→Fibrosis) fired with little bookkeeping drama | The Ledger records *when the phase was caught*, and a late catch reads differently from an early one |
| Children existed as a productivity mechanic | The Cohort makes the second generation's *baseline* the moral crux |

---

# III. THE FOUR REGISTERS — SYSTEMS

## 3.1 `DoseLedgerSystem` — who counts

Plain C#, engine-agnostic. State and events below.

`DoseLedgerSystemState`:
```
systemId ; survivorEntries: List<DoseEntry> ; ceilingMsv ; totalReadingsTaken ;
readingsSinceLastCalibration ; calibrationOverdueVersion
```
Per `DoseEntry`:
```
survivorId ; baselineMsv (inherited, never zeroed) ; cumulativeMsv ; readingsHistory: List<DoseReading> ;
radiationPhaseCaught ; shieldingFactor ; lastAntiRadDay
```
Events: `OnDoseCorrected(survivorId, mSv)`, `OnBandReached(survivorId, DoseBand)`, `OnLedgerCalibrated`.

**Rules (from the domain, not invented here):**
- Every exposure event (`RadiationSystem`) may post to the ledger *if the player bothers to record it* (a Dosimeter must be assigned to that survivor). Unread rads are still real — they're just not in the book.
- Anti-rad / iodine applied **after** an exposure reduces the *booked* dose for that reading; applied before reduces the *incoming* dose. The ledger records which.
- Shielding factor (hazmat / shelter level) attenuates what gets booked.
- A `high_energy_event` (fallout storm, EMP surge, vented reactor room) rolls a **flux ambiguity**: ±15% on the reading. The host shows the raw dial; the player books a figure that may be off. The ledger remembers what was written, not what was true.
- Calibration: after 40 readings the dial drifts; a `dosimeter` item refunds the accuracy. Until then, every new reading carries a growing known-error flag.

**Why this is human:** the ledger is never complete, never accurate, and still the only ledger there is. The player books the dose they can stand to write down.

## 3.2 `SickListSystem` — who is named

Reads cumulative dose and emits a prognosis band, *not a death sentence*.

`SickListSystemState`: `bands: List<SickBand>` where each `SickBand` is `{ survivorId, band, diagnosedDay, releaseDay=-1, palliativePlan }`.

Bands (dose → capacity):
```
Green   (< 100 mSv)  — no entry
Amber   (100-300)    — chronic fatigue; occasional weeks off the roster
Red     (300-600)    — marrow strain; needs morphine/palliative to keep working even part-time
Black   (> 600)      — the county knows this name; heavy care, morphine, the good bed
```
A band is *caught* only when someone assigns a dosimeter read against the Sick List. A Black-band survivor is not removed from the game — they remain, use a bed, cost care, and may still *choose* a Voluntary Register entry. That is the tone: the sick are not erased, they are *named and cared for, or named and abandoned* — and both are recorded.

Events: `OnDiagnosed(survivorId, band)`, `OnReleased(survivorId)`, `OnPalliativeAssigned(survivorId, plan)`.

## 3.3 `CohortSystem` — the second generation's baseline

Children born after the exchange inherit a baseline dose from their parents' cumulative totals at conception, plus a postnatal adjustment. The important rule is **the baseline is intractable and largely unknown** — it is recorded as a *guess band* the player chooses, and any two adults in the bunker will dispute it.

`CohortSystemState`: `children: List<CohortChild>` where each is `{ survivorId, parentIds, inheritedGuessBand, birthDay, shieldingExampled, moralityMemory }`.

Rules:
- At birth, the host books only a **Guess Band** (the player chooses "low / medium / high" from an imperfect parental figure). The truth is hidden.
- A dosimeter on the child later **corrects** the guess silently — `CohortSystem` records the correction, `DoseLedgerSystem` may or may not, depending on whether anyone books it.
- The child remembers nothing of the war, but the *band they were told* shapes their later risk decisions. That is `moralityMemory` — the story, not the dose.

Events: `OnChildBooked(survivorId, guessBand)`, `OnBaselineCorrected(survivorId, trueBand)`.

**Why this is the moral core:** the first generation cannot undo what the atmosphere did. They can only decide *what to tell a child about themselves*. The game does not grade this.

## 3.4 `VoluntaryRegisterSystem` — who signs away the front of the days

A survivor may volunteer for high-dose surface work (expeditions into hot zones, vented maintenance, scavenging the reactor corridor). This is not a penalty — the Shelter did not force them. It is a signature.

`VoluntaryRegisterSystemState`: `entries: List<VolunteerEntry>` where each is `{ survivorId, task, acceptedDay, completedDay=-1, doseIncurred, reasonText }`.

Rules:
- A volunteer must be named on the Sick List or explicitly *not* named — the host asks which. Volunteering while unlisted is different from volunteering while named Black.
- The player signs off the dose. There is no way to volunteer *nothing*.
- On completion, the dose is banked into the Ledger, the Sick List may move a band, and a short `reasonText` (the volunteer's own words) is written once and never edited.

Events: `OnVolunteered(survivorId, task)`, `OnVolunteerCompleted(survivorId, dose)`.

---

# IV. QUEST LINES — WHAT THE PLAYER IS ASKED TO WRITE

Four quest lines. None has a "correct" resolution flag. Each resolves by *choosing what is recorded*.

## 4.1 `quest_the_dose_the_first_reading` — Day 40+
**Gate:** a dosimeter exists. One survivor's first reading books an Amber band.
**Objective:** decide whether to start the Ledger at all. Choosing *not* to start it bars the other three registers for a season — the shelter "doesn't keep that kind of book."
**First choice:** start the Ledger, or close it. Either is a resolution.

## 4.2 `quest_the_sick_of_room_seven` — Day 90+ (after a Red band)
**Gate:** a Red-band survivor exists.
**Objective:** the room has two beds and one morphine routine. Choose who is named first, and what the named person is told.
**Choices:** keep the bed honest (both named, split care), hide one name, or draw on the Voluntary Register for a high-dose work rotation that buys care. All three close the quest; none unroots the cause.

## 4.3 `quest_the_childs_number` — Day 150+ (after a birth under Cohort)
**Gate:** a Cohort child exists.
**Objective:** the parents ask the shelter to book the child's baseline. The numbers are a guess.
**Choices:** book low (protects the child's story, risks a shock later), book honest (a grim but maybe-true number), or refuse to book and let the child grow up "uncounted." A London-style refusal is final for this child.

## 4.4 `quest_the_signed_hour` — Day 200+ (any Volunteer entry)
**Gate:** a Volunteer entry is signed.
**Objective:** the volunteer wants to do the task *now*, in the current weather / hazard window, or wait. Choosing to send them now books a worse dose; waiting risks the window closing. The volunteer's `reasonText` is written regardless.

---

# V. NEW NAMED NPCS

Four site-keeper / record-keeper figures. Each owns one register's *pen*, not its numbers.

| Id | Name | Role | Will not |
|---|---|---|---|
| `npc_dr_irina_vel` | Dr. Irina Vel | Radiation registrar; keeps the Ledger pen | forge a reading to comfort |
| `npc_wyn_omah` | Sister Wyn Omah | Sick-room nurse; keeps the Sick List | move a name up the bed order for mercy |
| `npc_piet_abar` | Piet Abar | clockmaker; calibrates dosimeters | lie about the drift |
| `npc_saria_voss` | Saria Voss | midwife; keeps the Cohort | book a guess as a truth |

These four are not your friends. They are your accountants. Each has a `wants` and a `will_not`, mirroring the Crossing backer pattern, so the player negotiates with a pen, not a sword.

---

# VI. NEW & REUSED LOCATIONS

**Reused (no new geography authored):** the vented reactor corridor, the fallout hot zones north of Km 19, the medical bay, room seven, the children's corridor. All *existing* ids gain a `dose_reading` hook, not a new room.

**New (three rooms, all one-screen, node-tick not walker):**
| Id | Parent | What stands there |
|---|---|---|
| `loc_the_dose_room` | bunker | the Ledger table, four chairs, a fan |
| `loc_the_calibration_bench` | bunker | dosimeters in a row, Piet's clock |
| `loc_the_childrens_baseline_board` | bunker | chalk numbers, half-erased guesses |

These are *standing places*, not interiors that render 3D. A levy walks up, the host shows the register.

---

# VII. NEW ITEMS

All new items are *books and tools*, not loot.

| Id | Type | Effect |
|---|---|---|
| `item_dose_ledger` | Quest/story | opens the Ledger surface; consumed when the Ledger is started |
| `item_calibration_key` | Tool | resets dosimeter drift (Piet's key) |
| `item_dosimeter_tag` | Attachment | binds a dosimeter read to a named survivor |
| `item_palliative_morphine` | Medical | moves a Red/Black survivor to a palliative plan (existing mechanics, re-targeted) |
| `item_cohort_first_board` | Quest/story | the children's baseline chalkboard as a carried memory |

Anti-rad / iodine / gas mask / hazmat are reused, now with dose-ledger targets.

---

# VIII. SAVE / WIRING (Godot-native)

Four systems in `Ashfall.Core` (new files):
```
Assets/Ashfall.Core/DoseLedgerSystem.cs
Assets/Ashfall.Core/SickListSystem.cs
Assets/Ashfall.Core/CohortSystem.cs
Assets/Ashfall.Core/VoluntaryRegisterSystem.cs
```
All four: `CaptureState` + `RestoreState` (deep copy), `OnStateChanged`, `ISeededRng` for flux ambiguity, no engine references.

**Save envelope:** one system that rides the existing expansion hub host, or a standalone `DoseLedgerSave.cs` + codec if a fifth expansion hub host session is preferred. Persistent across Godot and Unity via `IJsonSerializer` + `SaveChecksum`, exactly as Holdfast / Duty Roster / Expansion Hub / Year of Ash.

**Host wiring:** a `DoseLedgerHostSession` analogous to `ExpansionHostSession` — constructs the four systems, subscribes `OnStateChanged` → dirty flag → coalesced flush, restores on setup, saves on quit. Buttons + a `--dose-ledger-selftest`.

**Events that feed it:** `RadiationSystem` exposure events post to the Ledger if a dosimeter tag is assigned; `YearOfAshTimelineSystem` deep-freeze / thaw radon spikes are natural "high_energy_event" flux rolls.

---

# IX. VERIFICATION PROTOCOL

- `dotnet test` — new `Ashfall.Core.Tests/DoseLedgerSystemTests.cs`, `SickListSystemTests.cs`, `CohortSystemTests.cs`, `VoluntaryRegisterSystemTests.cs`: per-system roundtrip, band transitions, flux ambiguity determinism, cohort baseline correction, volunteer dose banking.
- `godot --headless -- --dose-ledger-selftest` — the four-system demo: book a first reading, hit a Red band, diagnose, birth + correction, sign a volunteer, then save→reload→restore→verify checksum + tamper.
- Full regression battery: expansions, journal, holdfast save, bridge, year-of-ash, duty-roster, expansion-hub — all must remain green. Godot-only; no Unity run (per project rule).

---

# X. NON-DUPLICATION LEDGER & SELF-REVIEW

| Concern | Answer |
|---|---|
| "Isn't this just RadiationSystem again?" | RadiationSystem models the *physics*; this models the *record*. The dose a scanner shows and the dose a ledger books are the same number only if someone writes it. |
| "Personal journals already track trauma" | Journals are personal voice. This is a *medical* ledger with a pen in someone else's hand, and a named family of accountants. |
| "Chronic illness is already a field" | It had no origin and no *decision point*. The Sick List is the decision, the Voluntary Register is the consequence. |
| "Children already exist" | They existed as labour. The Cohort makes their baseline a contested document. |
| "High-dose work is just expeditions" | Expeditions are a distance mechanic. The Voluntary Register is a *named, signed* act. The difference is the signature. |
| "Too many documents already" | That is the point — ASHFALL's thesis is that the world is paperwork and the body is a cost. The Dose is the last paper it writes on. |

**Tone check:** nothing here glorifies radiation or the sick. The sick are never "managed" off-screen; they are named, cared for, or abandoned, and the ledger remembers which. No death by dose is framed as a victory. The game never tells the player how to feel about a band on a board.