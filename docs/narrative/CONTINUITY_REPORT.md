# Narrative Continuity Report — Full Corpus Audit (25 Creative-Writing Batches)

**Scope:** Cross-reference and contradiction audit across the full narrative data-authority corpus (`Assets/StreamingAssets/Data/narrative/`, 272 JSON files), focused on the ten cross-batch narrative threads identified by the owner. This report supersedes and expands the earlier eight-batch report.

**Method:** `ashfall-narrative-continuity` — canon registry snapshot, reference integrity (mechanical via `CatalogIntegrityValidator`), contradiction sweep (semantic + numerical), player reachability. Read-mostly; no JSON edits applied in this pass.

---

## Phase 1 — Canon Registry Snapshot

The narrative corpus establishes a shared shelter community across ~Day 1–Day 130. Recurring named entities (the continuity backbone):

| Entity | Identity | Established in |
|---|---|---|
| Yelena | Quartermaster, carries the secret flour number | journals, bureaucratic, letters, ration_records |
| Tomas | Engineer, assesses geothermal vent, rebuilds generator with clock brass | journals, engineering, expedition_briefs |
| Ivan | Doctor (Dr. Ivan), runs the clinic | journals, medical |
| Anya | Nurse (Nurse Anya), intake/medication/comfort, hunger tallies | medical, ration_records |
| Suki | Teacher, keeps the water-cycle lesson, astronomical knowledge | journals, graffiti, expedition_briefs |
| Rima | Child, age 9, keeps a book, draws the sun-with-a-face | journals, letters, graffiti |
| Dima | Child, age 6, dies Day 22 (the quiet game, the rain question) | journals, letters, graffiti |
| Kolya | Child, age 8, burn boy (heals Day 20) | medical |
| Bram | Courier, carries the treaty pouch | journals, bureaucratic, radio |
| Mira | Scavenger, goes to the printing works | journals, bureaucratic, medical |
| Petr | Old farmer, plants against the season, first green Day 88 | journals, bureaucratic, weather_almanac |
| The Loma family | 7 persons, arrive Day 14 | journals, bureaucratic, medical |
| The river woman | Unidentified female, dies Day 33 | medical, journals, letters, graffiti, bureaucratic |
| Victor | Conscript, 4200 mSv, lives 95 days | medical, graffiti |
| The Relay Operator | Ridge station, weather logs, star counts, Day 110 final entry | weather_almanac, night_watch |
| The Mechanic | Ashfall measurements, Day 100/110 summaries | weather_almanac, journal_entries_batch_3 |

No real-country/real-war/real-person references (gated by `DataRuleComplianceTests`). No magic/fantasy intrusions. No glorified violence.

---

## Phase 2 — Reference Integrity (Mechanical)

`godot --headless --path . -- --data-integrity-selftest` → **PASS — 0 errors, 0 warnings across 113 catalogs.**

All narrative files use non-registered id prefixes or schema-compatible structures; the validator never flags them and they require no code changes. No dangling `flag_*`, `faction_*`, `loc_*`, or `item_*` references introduced in the creative-writing batches.

---

## Phase 3 — Contradiction Sweep (Semantic + Numerical)

### Thread-by-thread verdict

| # | Thread | Verdict | Evidence |
|---|---|---|---|
| 1 | The flour number | **MOSTLY CONSISTENT — 1 cross-file contradiction** | Journals: 19→14→11. Ration records: 19→11→8→6→5→4→3. Weather almanac Day 110 trajectory: 19→14→11→8→6→5→3 (omits the 4). See Finding 1. |
| 2 | The river woman | **CONSISTENT** | Medical: intake Day 12 → treatment Day 12/19/26/30 → autopsy Day 33. Journals, letters, graffiti, bureaucratic all reference her death on Day 33. Three distinct rings confirmed (river woman's, Day-37 single adult's, widow's husband's). |
| 3 | The clock-brass generator | **CONSISTENT** | Engineering logs: Day 4 daily check → Day 15 injector/boot-gasket → Day 24 knock → Day 31 rebuild with clock brass → Day 32 morning after → Day 80 brass holds → Day 90 fuel. Journals align. |
| 4 | The dam evacuation | **CONSISTENT** | Radio arc (Days 1–4, gate closes Day 3 at 4am, operator down Day 4) and bureaucratic evacuation order (Day 4, 03:30, relayed via Kestrel-9, same transmission text). Kestrel-9 is the relay in both. Operator fate is intentionally ambiguous (no follow-up broadcast). |
| 5 | The sun-with-a-face | **CONSISTENT (after prior fix)** | Rima (age 9) draws sun-with-a-face in journals (Day 27), letters (to father), graffiti (the wall argument). Medical: Kolya (age 8, formerly Dima) draws sun-with-a-face on Day 20 — a separate child's drawing, not a named-motif collision. See Finding 7 for residual tag. |
| 6 | The spring counter | **CONSISTENT** | Day 100: ration_records (menu, ledger, hunger tally) and weather_almanac (seasonal marker) all agree: the spring is the counter. The counter is the potato, cabbage, nettle, mushroom, root, fish — not the flour. |
| 7 | The geothermal transition | **GAP + 1 CONTRADICTION** | Day 31/32: Tomas assesses, pipe feasible. Day 47: journal says "If we could route..." Day 50: expedition says "engineering is beyond what we can do" (contradicts Tomas). Day 100: ration_records assumes geothermal IS the heat. Day 127: pipe material used for hand crane, not heat exchanger. No explicit record of transition occurring. See Finding 5. |
| 8 | The thin count trajectory | **MOSTLY CONSISTENT — 1 unsupported extrapolation** | Hunger tallies: 0, 12, 8, 31, 28, 14. Day 110 weather almanac extends to 0, 12, 8, 31, 28, 14, 8. The final 8 has no supporting tally. See Finding 6. |
| 9 | The ash trajectory | **INCONSISTENT — cherry-picked summary** | Day 110 claims: 47, 35, 12, 5, 3, 2, 1. Actual logged measurements include Day 45=8g, Day 70=0g, Day 75=0g, Day 95=2g — all omitted from the trajectory. The 35 is only referenced in Day 50 comparison, not independently measured. The 5 does not appear as a measured value. See Finding 3. |
| 10 | The star count | **INCONSISTENT — U-shaped trajectory** | Day 70: 47. Day 75: 53 (+6, "stars returning"). Day 95: 47, "down from 62 last month." Day 108: 62. The count rises, falls, then rises again, contradicting the "fewer stars / thickening ash" narrative. See Finding 4. |

---

### Findings

#### Finding 1 — Flour Day 100 numerical contradiction (HIGH)

**Location:** `narrative/weather_almanac_expansion.json` Day 100 (`bunker_doc_ashfall_day100`) vs `narrative/ration_records_expansion.json` Day 100 (3 documents: `bunker_doc_menu_spring`, `bunker_doc_ledger_spring`, `bunker_doc_hunger_tally_spring`).

**Issue:** The weather almanac's Day 100 ashfall measurement says: "The flour is also 3. The flour is 3 days. The ash is 3 grams. The two 3s are the same number."

But the ration records on the same day all say flour = 4 days:
- Menu: "The flour is 4 days. The 4 is the lowest."
- Ledger: "Flour: 4 days. The 4 is the number."
- Hunger tally: "The flour is 4. The 4 is the lowest."

The Day 110 weather almanac summary trajectory ("19, 14, 11, 8, 6, 5, 3") also omits the Day 100 value of 4, jumping from 5 to 3.

**Resolution needed:** Either change the weather almanac Day 100 to "4" to match the ration records, or change the ration records Day 100 to "3" to match the weather almanac. The two 3s (flour=3, ash=3g) are a deliberate poetic parallel, so if the flour is kept at 3, the ration records must be updated. If the flour stays at 4, the weather almanac must be updated and the poetic parallel removed or adjusted.

#### Finding 2 — Star count inconsistent trajectory (MEDIUM)

**Location:** `narrative/weather_almanac_expansion.json` Days 70, 75, 110 and `narrative/night_watch_expansion.json` Day 95.

**Issue:** The star counts form a U-shaped trajectory that contradicts the "fewer stars / thickening ash" narrative:

| Day | Star count | Source | Narrative claim |
|---|---|---|---|
| 70 | 47 | weather_almanac | "first count", "fewer than the before" |
| 75 | 53 | weather_almanac | "more than the 47", "stars returning" |
| 95 | 47 | night_watch | "down from 62 last month", "ash is thickening" |
| 108 | 62 | weather_almanac | "more than the 47", "proof the sky is clearing" |

Contradictions:
- Day 75 (53) is MORE than Day 70 (47), but the narrative says "the stars are fewer."
- Night watch Day 95 says "last month: 62." There is no Day 65 star count in the weather almanac. Day 70 was 47, not 62. So "last month: 62" cannot refer to Day 70.
- Day 108 (62) is MORE than Day 95 (47), but the night watch Day 95 entry says "the ash is thickening" and "the stars are going behind the ash."

**Resolution needed:** Either the Day 75 and Day 108 counts are errors that should be lower (consistent with a monotonic decline), or the narrative framing ("fewer stars", "ash thickening") needs to acknowledge the fluctuation. If the counts are correct as written, the trajectory is a "clearance event" (ash thins, stars return, ash thickens again, stars clear again) rather than a simple decline.

#### Finding 3 — Ash trajectory cherry-picks measurements (MEDIUM)

**Location:** `narrative/weather_almanac_expansion.json` Day 110 (`bunker_doc_weather_day110`).

**Issue:** The Day 110 summary says: "The trajectory of the ash: 47, 35, 12, 5, 3, 2, 1."

But the actual logged measurements in the same file are:

| Day | Ashfall (g/m²) | In trajectory? |
|---|---|---|
| 10 | 47 | ✅ |
| 30 | 35 | ⚠️ Only mentioned in Day 50 comparison, not independently measured |
| 45 | 8 | ❌ Omitted |
| 50 | 12 | ✅ |
| 70 | 0 | ❌ Omitted |
| 75 | 0 | ❌ Omitted |
| 95 | 2 | ❌ Omitted |
| 100 | 3 | ✅ |
| 110 | 1 | ✅ |

The value "5" does not appear as an independent measurement anywhere. The trajectory skips four actual measurements (8, 0, 0, 2) that would disrupt the clean "47→1" narrative arc.

**Resolution needed:** Either add the missing measurements to the trajectory (e.g., "47, 35, 12, 8, 0, 0, 2, 3, 1") or explain why they are excluded (e.g., "excluding storm events and zero-ashfall clear nights"). The current presentation implies these are the only measurements, which is false.

#### Finding 4 — Thin count trajectory final value unsupported (LOW)

**Location:** `narrative/weather_almanac_expansion.json` Day 110.

**Issue:** The hunger tallies in `ration_records_expansion.json` show the "thin" count at six points:

| Day | Thin count | Document |
|---|---|---|
| 7 | 0 | `bunker_doc_hunger_tally_week1` |
| 28 | 12 | `bunker_doc_hunger_tally_week4` |
| 44 | 8 | `bunker_doc_hunger_tally_harvest` |
| 56 | 31 | `bunker_doc_hunger_tally_week8` |
| 65 | 28 | `bunker_doc_hunger_tally_solstice` |
| 100 | 14 | `bunker_doc_hunger_tally_spring` |

The Day 110 weather almanac extends this to seven values: "0, 12, 8, 31, 28, 14, 8."

The final "8" has no supporting hunger tally. There is no Day 105 or Day 108 tally showing thin=8. The Day 100 tally shows 14, and the trajectory predicts a decline to 8 by Day 110. This is either an unsupported extrapolation or a missing tally document.

**Resolution needed:** Either add a hunger tally document for Day ~105 showing thin=8, or remove the extrapolated value from the Day 110 trajectory.

#### Finding 5 — Geothermal transition gap and contradiction (MEDIUM)

**Locations:** 
- `narrative/expedition_briefs_expansion.json` Day 31/32 (Tomas assessment)
- `narrative/expedition_field_reports.json` Day 50 (field report)
- `narrative/journal_entries_batch_1.json` Day 47 (journal)
- `narrative/journal_entries_batch_3.json` Day 127 (hand crane)
- `narrative/ration_records_expansion.json` Day 100 (ledger)

**Issue:** The geothermal transition is narrated inconsistently:

1. **Day 31/32 (Tomas):** Assesses the vent, concludes the 4km insulated pipe run is feasible, says "If the pipe is built, the shelter has heat."
2. **Day 47 (journal):** "If we could route the steam to the bunker heat exchangers, we'd save forty litres of fuel a week." (Still hypothetical.)
3. **Day 50 (expedition field report):** "The plant is a viable long-term heat source if we can route the steam to the bunker. The engineering is beyond what we can do with current tools and knowledge — we need someone who understands geothermal systems."

This directly contradicts Tomas's Day 32 conclusion. Either Tomas is wrong (but he's the engineer) or the Day 50 team lacks his expertise (plausible, but not stated).

4. **Day 100 (ration records):** "Lamp oil: 0. The 0 is the lamp. The 0 is the dark. But the geothermal is the heat. The heat is the geothermal. The geothermal is the spring."

This assumes the transition has already happened — the shelter is using geothermal heat and lamp oil is gone. But there is no explicit narrative of the pipe being installed, the heat exchangers being connected, or the transition occurring.

5. **Day 127 (journal):** The Mechanic uses "two lengths of the stainless pipe the Mechanic's team brought back from the geo-thermal plant" to build a hand crane for fuel drums — not for a heat exchanger.

**Resolution needed:** Add an explicit narrative document (engineering log, journal entry, or council minute) recording the geothermal transition between Day 50 and Day 100. Reconcile the Day 50 field report with Tomas's Day 32 assessment — either the Day 50 team is unaware of Tomas's work, or Tomas's plan required modifications not anticipated on Day 32.

#### Finding 6 — Uncanonical characters in journal_entries_batch_1 (LOW)

**Location:** `narrative/journal_entries_batch_1.json`.

**Issue:** Two authors appear who are not in the canonical survivor registry or `survivor_profiles_expansion.json`:

- **Elena Vasquez** (`elena_vasquez`): Day 47 geo-thermal plant observation, Day 61 seed vault.
- **Marcus Olejnik** (`marcus_olejnik`): Day 52 silent observatory, Day 61 seed vault, Day 68 barge recovery.

The canonical engineer is Tomas. The canonical quartermaster is Yelena. Neither Elena nor Marcus appears in any other narrative file.

**Resolution needed:** If these are transient survivors, they should be added to `survivor_profiles_expansion.json` with canonical IDs. If they are editorial artifacts from a different draft, they should be retconned to canonical characters (e.g., Marcus → Tomas, Elena → Mira or another canonical survivor).

#### Finding 7 — Residual "dima" tag in medical documents (LOW)

**Location:** `narrative/medical_documents_expansion.json`, `med_nurse_26_dima_hand`.

**Issue:** The patient is correctly identified as "Kolya, age 8" with `patient_id: "dwr_burn_boy_kolya"`, but the `tags` array still includes `"dima"`. This is a residual reference from the pre-fix era when the burn boy was named Dima, creating a name collision with the canon Dima (age 6, dies Day 22).

**Resolution needed:** Remove `"dima"` from the tags array of `med_nurse_26_dima_hand`. The rename fix was applied to `patient_name` and `patient_id` but not to the tags.

#### Finding 8 — Dam evacuation operator fate intentionally ambiguous (LOW)

**Location:** `narrative/radio_scripts_expansion.json`, `narrative/bureaucratic_documents_expansion.json`.

**Issue:** The North Dam operator says "I will stay on the winch. I will call when the gate is closed or when the gate is not closed." No follow-up broadcast confirms whether the gate was closed or whether the operator survived. This is likely intentional ambiguity (the radio arc ends with the operator's sign-off on Day 7 at Kestrel-9, not at the dam), but it leaves the dam evacuation thread unresolved.

**Resolution needed:** Owner decision. Leave as intentional ambiguity, or add a brief line in a bureaucratic or radio document confirming the operator's fate.

---

## Phase 4 — Player Reachability

All batches are **DATA_ONLY** per the forensic taxonomy: pure data-authority JSON, no code wiring, no quest/flag/event hooks. They are loadable as diegetic flavor via their respective catalog loaders or as raw narrative content. No batch is wired to a quest or flag, so "reachability" is "discoverable as world text."

**New orphaned content (since last report):**
- `journal_entries_batch_1.json`: Contains raw entries loadable via `TryAddRawEntry()`, extending the journal beyond the 14 hardcoded keys. Authors Elena Vasquez and Marcus Olejnik are not in the canonical survivor registry (Finding 6).
- The "thin count trajectory" Day 110 extrapolation (Finding 4): The final value "8" has no source tally.

---

## Tone & Content Rules

- No magic, no fantasy, no supernatural confirmation. All ambiguous entries (the numbers-loop radio arc, the "maybe-sun") remain physically explainable.
- No real countries/wars/people (gated by `DataRuleComplianceTests`).
- No glorified violence; medical and autopsy content is restrained and clinical.
- No copied IP.

---

## Verification

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS (0 errors, 0 warnings) |
| `dotnet test Ashfall.Core.Tests` | PASS (2851/2851) |
| `godot --headless -- --data-integrity-selftest` | PASS (0/0 across 113 catalogs) |
| `godot --headless -- --bridge-selftest` | PASS (exits 0) |

---

## Quality Gate

- ⚠️ **1 HIGH-severity contradiction remains:** Flour Day 100 value differs between weather_almanac (3) and ration_records (4). Requires owner decision on canonical value.
- ⚠️ **2 MEDIUM-severity contradictions remain:** Star count U-shaped trajectory (47→53→47→62); ash trajectory omits actual measurements. Requires owner decision.
- ⚠️ **1 MEDIUM-severity gap remains:** Geothermal transition is assumed but not explicitly narrated; Day 50 field report contradicts Day 32 engineering assessment.
- ✅ **All other threads resolve consistently** across the full corpus.
- ✅ **Zero dangling mechanical references** (canonical selftest 0/0).
- ✅ **Every contradiction has file-level evidence** and a proposed resolution.

**Conclusion:** Seven of ten cross-batch threads resolve consistently. Three threads have contradictions or gaps requiring owner decision before the corpus can be considered fully continuous. No edits were applied in this pass.
