# Narrative Continuity Report — Eight Creative-Writing Batches

**Scope:** Cross-reference and contradiction audit across the eight diegetic-content batches added in commit `cfbf8319` (atmosphere, radio, journals, bureaucratic, letters, medical, engineering, graffiti) and their continuity with the existing canon.

**Method:** `ashfall-narrative-continuity` — canon registry snapshot, reference integrity (mechanical via `CatalogIntegrityValidator`), contradiction sweep (semantic, manual), player reachability. Read-mostly; two mechanical fixes applied with rationale below.

---

## Phase 1 — Canon Registry Snapshot

The eight batches establish a shared shelter community across ~Day 1–100. Recurring named entities (the continuity backbone):

| Entity | Identity | Established in |
|---|---|---|
| Yelena | Quartermaster, carries the secret flour number | journals, bureaucratic, letters |
| Tomas | Engineer, keeps the Meridian K generator | journals, engineering, letters |
| Ivan | Doctor (Dr. Ivan), runs the clinic | journals, medical, bureaucratic |
| Anya | Nurse (Nurse Anya), intake/medication/comfort | medical |
| Suki | Teacher, keeps the water-cycle lesson | journals, graffiti |
| Rima | Child, age 9, keeps a book, draws the sun-with-a-face | journals, letters, graffiti, medical(dose ledger) |
| Dima | Child, age 6, dies Day 22 (the quiet game, the rain question) | journals, letters, graffiti |
| Bram | Courier, carries the treaty pouch | journals, bureaucratic, radio |
| Mira | Scavenger, goes to the printing works | journals, bureaucratic, medical(dose ledger) |
| Petr | Old farmer, plants against the season | journals, bureaucratic |
| The Loma family | 7 persons, arrive Day 14 | journals, bureaucratic, medical |
| Kolya | Child, age 8, burn boy (renamed from Dima — see Fix 2) | medical |
| The river woman | Unidentified female, dies Day 33 | medical, journals, letters, graffiti, bureaucratic |
| Victor | Conscript, 4200 mSv, lives 95 days | medical, graffiti |

No real-country/real-war/real-person references (gated by `DataRuleComplianceTests`; the existing `survivor_letters_lost_kin.json` Leningrad reference was deliberately avoided by using the cleaner `unsent_letters_batch_2` schema). No magic/fantasy intrusions. No glorified violence.

---

## Phase 2 — Reference Integrity (Mechanical)

`godot --headless --path . -- --data-integrity-selftest` → **PASS — 0 errors, 0 warnings across 113 catalogs.**

All eight files use non-registered id prefixes (`atm_`/`freq_distress_`/`journal_`/`bunker_doc_`/`letter_`/`med_`/`maint_`/`graf_`) so the validator never flags them and they require no code changes. No dangling `flag_*`, `faction_*`, `loc_*`, or `item_*` references introduced. The 579 pre-existing deep-walk errors in `wasteland_settlement_gazetteer.json` and other unrelated narrative files are outside this audit's scope.

---

## Phase 3 — Contradiction Sweep (Semantic)

### The five continuity claims from the commit message

| # | Claim | Verdict | Evidence |
|---|---|---|---|
| 1 | The flour number (QM secret → census correction → sister's letter) | **CONSISTENT** | Journals: 19→14→11 days across Days 9/14/22; bureaucratic incident report (Day 22): "the number is eleven"; bureaucratic census correction (Day 62): headcount 140→147 (separate from flour); letters (sister): "I cannot tell you the number." The number 11 is consistent; the 42 is the protein number consistently; the headcount correction is a separate, consistent thread. |
| 2 | The river woman (death across journals/medical/letters/graffiti) | **CONSISTENT** | Medical: intake Day 12 → treatment Day 12/19/26/30 → autopsy Day 33 → nurse note Day 33; journals (doctor arc) reference her; letters (widow's ring is a *different* ring); graffiti: "RIVER WOMAN. I NAMED HER."; bureaucratic: transfer order to sheeted bay. Death day 33 consistent across all. Three distinct rings (river woman's, the Day-37 single adult's, the widow's dead husband's) — no conflation. |
| 3 | The clock-brass generator (journals/engineering) | **CONSISTENT** | Journals (engineer arc) and engineering logs agree on Day 4 (daily check), Day 15 (injector/boot-gasket), Day 24 (the knock), Day 31 (rebuild with clock brass), Day 32 (morning after). Engineering extends to Day 80 (brass holds) and Day 90 (fuel). Identical prose in both. |
| 4 | The dam evacuation (radio/bureaucratic) | **CONSISTENT** | Radio arc (Days 1–4, gate closes Day 3 at 4am, operator down Day 4) and bureaucratic evacuation order (Day 4, 03:30, relayed via Kestrel-9, same transmission text). Kestrel-9 is the relay in both. |
| 5 | The child's sun-with-a-face (journals/letters/graffiti/medical) | **CONSISTENT (after Fix 2)** | Rima (age 9) draws the sun-with-a-face in journals (Day 27), letters (to father), graffiti (the wall argument — adult corrects "the sun has no face," child draws it back smaller). See Finding 2 for the medical overlap, now resolved. |

### Findings

#### Finding 1 — Rima age contradiction in bureaucratic denial (FIXED)

**Location:** `narrative/bureaucratic_documents_expansion.json`, `bunker_doc_denial_extra_04`, clerk margin note.

**Issue:** The ration denial form's basis field said "infant age 4 months" (Loma M.'s nursing baby), but the clerk margin note read "Infant is, per the roster, named Rima. The infant is not an infant. Rima is four." This conflated the 4-month-old infant with **Rima**, who is established as **age 9** in journals (child arc), letters (grandmother: "You are nine"), medical (dose ledger: "Rima, age 9"), and graffiti ("Rima, age 9"). The note made Rima four, contradicting the canon age of nine.

**Resolution (applied):** Rewrote the clerk margin note to preserve the form-error texture while correcting the conflation: the infant is Loma M.'s (four months, per the form); Rima is the grandmother's granddaughter (age nine), who is *not* the infant. The form conflated the two children; the clerk corrects the conflation without changing Rima's age.

**Evidence:** `Rima, age 9` in medical (`med_dose_36_child_rima`), letters (`letter_06_to_grown_daughter`: "You are nine"), graffiti (`graf_joke_22_sun`, `graf_long_39_sun_face`).

#### Finding 2 — Dima name/fate contradiction across medical vs journals/letters/graffiti (FIXED)

**Location:** `narrative/medical_documents_expansion.json` (4 cases, `med_intake_03_burn_boy`, `med_treat_10_burn_boy_day14`, `med_nurse_26_dima_hand`, `med_medlog_32_honey_dressing`) vs `narrative/journals_expansion.json` (teacher arc), `narrative/letters_expansion.json` (`letter_12_to_the_dead_child`), `narrative/graffiti_expansion.json` (`graf_grief_26_dima`, `graf_grief_31_empty`).

**Issue:** Two different children named **Dima** with contradictory ages and fates:
- **Canon Dima, age 6:** established in journals (teacher arc, "Dima is six," asks about the rain Day 13, plays the quiet game Day 18), letters (`letter_12_to_the_dead_child`: "You were six. You are not six anymore," death Day 22), and graffiti (the memorial candle "for dima," the empty bed). This Dima **dies on Day 22**.
- **Medical Dima, age 8:** the burn boy, scalded hand from a kettle Day 7, heals by Day 20, draws the sun-with-a-face. This Dima **survives and heals**.

The same name with conflicting age (6 vs 8) and fate (dies Day 22 vs heals) is a genuine canon break.

**Resolution (applied):** Renamed the medical burn boy from "Dima, age 8" to **"Kolya, age 8"** (patient_id `dwr_burn_boy_dima` → `dwr_burn_boy_kolya`) across all 4 medical cases and their prose. The burn arc (intake, treatment, healing, honey dressing, nurse keeps the sun drawing) is fully preserved under the new name. The canon Dima (age 6, dies Day 22) is now unambiguous and unique. The sun-with-a-face motif is now Rima's signature in journals/letters/graffiti and a separate child's (Kolya's) drawing in medical — two children drawing suns is plausible and no longer a named-motif collision.

**Rationale for rename over the canon Dima:** The canon Dima (age 6, dies Day 22) is load-bearing across three batches (journals, letters, graffiti) and anchors the teacher's arc and a mother's grief. The medical burn boy is self-contained to the medical batch. Renaming the medical child is the smaller, safer change.

#### Finding 3 — Loma family headcount (CONSISTENT, no fix needed)

The Loma family is consistently **7 persons** across journals ("all seven of them"), bureaucratic (assignment slip "7 persons," denial "7 persons," census correction "+7"), and medical (grandmother intake). No contradiction.

#### Finding 4 — The three rings (CONSISTENT, no fix needed)

Three distinct gold rings appear, none conflated:
- River woman's ring (medical autopsy Day 33: "no claimant"; she was unidentified).
- The Day-37 unidentified single adult's ring (bureaucratic assignment: "retained by occupant," a living person).
- The widow's dead husband's ring (letters Day 40: "Your ring is in the office").

Different people, different fates, different days. No contradiction.

---

## Phase 4 — Player Reachability

All eight batches are **DATA_ONLY** per the forensic taxonomy: pure data-authority JSON, no code wiring, no quest/flag/event hooks. They are loadable as diegetic flavor via their respective catalog loaders (where loaders exist: `WastelandBestiaryCatalog`, `OrphanKnockWhitelist`, etc.) or as raw narrative content for the journal/radio/graffiti systems. No batch is wired to a quest or flag, so "reachability" is "discoverable as world text" rather than "gated by a quest state." No orphaned *mechanical* content (no dangling flags/refs). The content is ambient/diegetic, by design.

---

## Tone & Content Rules

- No magic, no fantasy, no supernatural confirmation. The "supernatural atmosphere" entries in the atmosphere batch and the numbers-loop radio arc are explicitly ambiguous and physically explainable.
- No real countries/wars/people (gated by `DataRuleComplianceTests`).
- No glorified violence; the medical and autopsy content is restrained and clinical, no gore for spectacle.
- No copied IP.

---

## Verification After Fixes

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS (0 errors, 0 warnings) |
| `dotnet test Ashfall.Core.Tests` | PASS (2851/2851) |
| `godot --headless -- --data-integrity-selftest` | PASS (0/0 across 113 catalogs) |
| `godot --headless -- --bridge-selftest` | PASS (exits 0) |

---

## Quality Gate

- ✅ Zero dangling mechanical references (canonical selftest 0/0).
- ✅ Every semantic contradiction has file-level evidence and a resolution (Findings 1 & 2 fixed; 3 & 4 verified consistent).
- ✅ Cross-batch continuity claims from the commit message all resolve consistently after fixes.
- ✅ Tone and content rules upheld.

**Conclusion:** Two semantic contradictions were found and fixed (Rima's age in the bureaucratic denial; the Dima name/fate clash between medical and journals/letters/graffiti). The five continuity claims in the commit message now resolve consistently. The eight batches form a coherent cross-referenced diegetic corpus.
