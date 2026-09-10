# LETTER CROSS-REFERENCE GRAPH

## 1. Narrative Cluster Architecture
While each letter is an autonomous human artifact, the 25 letters weave deep thematic and historical arcs across the life of the shelter.

Below is the validated cross-reference graph connecting letters into coherent memory clusters.

```
                    ┌─────────────────────────┐
                    │  letter_25 (Day 3)      │ "Take the coat and go... love is the going, go"
                    └───────────┬─────────────┘
                                │ (The Mother's Coat)
                                ▼
                    ┌─────────────────────────┐
                    │  letter_01 (Day 12)     │ "I have your coat. The grey one... I am wearing it"
                    └───────────┬─────────────┘
                                │
        ┌───────────────────────┴───────────────────────┐
        ▼                                               ▼
┌─────────────────────────┐                   ┌─────────────────────────┐
│  letter_07 (Day 5)      │                   │  letter_13 (Day 50)     │
│ "Tell Katerina I took   │                   │ "Take the good boot...  │
│  her coat. I left mine" │                   │  leave the wet coat"    │
└─────────────────────────┘                   └─────────────────────────┘
```

---

## 2. Documented Thematic Clusters

### Cluster 1: The Coat & The Evacuation
- `letter_25_one_sentence` (Day 3): Mother leaves urgent command on door hook to take coat and flee.
- `letter_01_to_mother` (Day 12): Daughter in shelter writes back about wearing the grey coat with velvet collar.
- `letter_07_last_letter` (Day 5): Conscript at forward post apologizes to Katerina for taking her coat.
- `letter_13_to_whoever_finds_this` (Day 50): Dying man in ditch warns finder to take boots, but abandon wet coat.

### Cluster 2: The Bedside Lamp, Watch & Cold Tea
- `letter_04_to_lover_returning` (Day 8): Woman A. leaves lamp filled, watch wound, and cold tea for M.
- `letter_05_to_lover_gone` (Day 44): Man M. returns to find lamp cold and watch stopped; drinks salted cold tea in grief.
- `letter_24_love_without_the_word` (Day 48): Woman A. reflects on the persistent ritual of winding the watch and filling the lamp on double shifts.

### Cluster 3: The Cold Frame & Planting in Dirt
- `letter_06_to_grown_daughter` (Day 33): Grandmother Bubba writes in the dirt by the cold frame about the first green potato sprout.
- `letter_15_child_to_father` (Day 16): 9-year-old Rima draws a smiling sun on the porridge-colored wall and counts dinner portions.
- `letter_22_to_younger_self` (Day 100): Elder reflection on planting in the autumn dirt and weeping at the sprout.

### Cluster 4: The Kitchen, Flour & Secret Rations
- `letter_03_to_sister` (Day 19): Elder sister Y. conceals that the flour reserves are dangerously low.
- `letter_08_confession_theft` (Day 23): Key-holder confesses stealing 2 flour sacks to feed the freezing night watch.
- `letter_16_to_old_friend` (Day 25): Woman A. teases cook Boris about stealing burned Tuesday bread to nourish the growing Loma girl.

### Cluster 5: The Powerhouse Knock & Supply Margin
- `letter_17_to_the_engineer` (Day 24): Quartermaster Yelena writes to Engineer Tomas demanding the honest week margin in the logbook margin. Directly intersects with `bunker_maintenance_glitches.json` (`glitch_07`) and `engineering_logs_expansion.json` (`maint_gen_04_knock`).

### Cluster 6: The Clerk's Census of the Deceased
- `letter_18_the_list_of_names` (Day 62): Formal admissions register connecting multiple off-screen and on-screen tragedies:
  - Thin line: River woman (Day 12, triage from `letter_10`), Conscript (Day 5, `letter_07`), Old man in ditch (Day 50, `letter_13`).
  - Thick line: Dima age 6 (Day 22, `letter_12`), Loma grandfather (Day 30), Night watchman (Day 33).

### Cluster 7: The Printing Works & The Scavengers
- `letter_14_thank_you_to_scavenger` (Day 28): Scavenger takes brass screws and leaves a whole candle on the printing works desk.
- `letter_21_to_the_teacher` (Day 36): Scavenger finds two sticks of chalk in the printing works and gifts them to the shelter teacher.

---

## 3. Reader & Spoiler Boundaries
- Cross-references in the reader UI must never reveal undiscovered letters.
- The existence of linked letters (e.g. `letter_04` and `letter_05`) is discovered organically by exploring rooms and reading correspondence, maintaining narrative surprise.
