# Epilogue Ending Binding Matrix (Plan 89 Cross-System Seams)

**Document ID:** `docs/endgame/EPILOGUE_ENDING_BINDING_MATRIX.md`
**Narrative Authority:** `Assets/StreamingAssets/Data/muster_epilogues.json` (Plan 89, 25 outcomes)
**Presentation Authority:** `Assets/StreamingAssets/Data/epilogue_chronicle.json` (Plan 96, 20 slides)

---

## 1. Five Representative Plan 89 Epilogue Integrations

Plan 96 establishes the visual presentation grounding for the 5 major ending categories defined in Plan 89 (`EpilogueMatrix.cs`). While the chronicle sequence maintains a coherent chronological arc, each representative ending highlights specific thematic slides:

| Ending Key (`muster_epilogues.json`) | Category | Highlighted Presentation Slides | Primary Slide Order & Title | Art Token | Narrative Alignment |
|---|---|---|---|---|---|
| `the_open_muster` | Muster Core | 14 (`The Muster`), 15 (`The Resolution`) | Order 14: `The Muster` | `epilogue_coalition_placeholder` | The coalition holds the substation; the rally point becomes an enduring regional trading settlement. |
| `ending_water_plant_held` | Resource / Desalination | 4 (`Water and Heat`), 15 (`The Resolution`) | Order 4: `Water and Heat` | `epilogue_resources_placeholder` | Desalination Unit 4 secured; the shelter controls the valley's primary clean water lifelines. |
| `ending_garrison_absorbs_coalition` | Faction Alignment | 7 (`The Factions`), 14 (`The Muster`) | Order 7: `The Factions` | `epilogue_factions_placeholder` | Central Garrison expands martial law, incorporating the coalition as auxiliary conscripts. |
| `ending_verdict_the_sector_recounts` | Verdict Forensic | 10 (`The Verdict`), 16 (`The Census`) | Order 10: `The Verdict` | `epilogue_investigations_placeholder` | The pre-war machine log count is read publicly at the Grain Exchange and recorded into history. |
| `ending_mercy_road` | Moral Path | 13 (`What We Chose`), 18 (`After Us`) | Order 13: `What We Chose` | `epilogue_key_decisions_placeholder` | Compassion and humanitarian amnesty prevail, setting an ethical precedent for future generations. |

---

## 2. Dynamic Text Binding Workflow

When `EpilogueChronicleBuilder.Build(...)` is invoked:
1. `EndingKey` selects the title via `TitleFor(endingKey)`.
2. The authoritative outcome prose from `muster_epilogues.json` is injected into Slide 15 (`The Resolution`).
3. Survivor memorials from `MemorialSystem` are injected into Slide 6 (`Empty Bunks`).
4. Living survivor fate cards are bound to Slide 5 (`Survivors`).
5. Demographic and days-survived statistics are bound to Slide 16 (`The Census`).
