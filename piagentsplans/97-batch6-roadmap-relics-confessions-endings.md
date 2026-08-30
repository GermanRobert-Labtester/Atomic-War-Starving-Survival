# Roadmap 97 — Batch 6: Relics, Confessions, Endings & Voice (Plans 87–96)

> **Scope:** Ten focused execution plans that expand ASHFALL's relic restoration,
> interpersonal morality, campaign-ending, radiation-management,
> food-production, faction-dialogue, investigation-NPC, machine-radio,
> journal-voice, and ending-presentation catalogs. Every target system is
> fully implemented and wired, but its data catalog is thin (3–18 entries).
> This batch fills those catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems,
> zero save changes. Every plan extends an existing JSON catalog.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `relic_recipes.json` | 6 relics | 15 relics | 87 |
| `confession_secrets.json` | 8 secrets | 20 secrets | 88 |
| `muster_epilogues.json` | 12 epilogues | 25 epilogues | 89 |
| `dose_registers.json` | 4 bands, 3 plans | 12 bands, 8 plans | 90 |
| `greenhouse_items.json` | 14 items | 30 items | 91 |
| `faction_war_dialogue.json` | 18 snippets | 40 snippets | 92 |
| `verdict_npcs.json` | 6 NPCs | 15 NPCs | 93 |
| `verdict_radio.json` | 13 broadcasts | 30 broadcasts | 94 |
| `journal_voice_prose.json` | 3 situation keys | 15 situation keys | 95 |
| `epilogue_chronicle.json` | 5 slides | 20 slides | 96 |

All target systems confirmed live in `Assets/Ashfall.Core/` via `find`/`grep`:
`WorkshopReverseEngineeringSystem.cs`, `EpilogueMatrix.cs`,
`DoseRegistersCatalog.cs`, `ItemCatalogLoader.cs`,
`FactionWarContentCatalog.cs`, `VerdictNpcSystem.cs`,
`VerdictRadioSystem.cs`, `JournalVoiceProseCatalog.cs`,
`EpilogueChronicleBuilder.cs`, plus `ContentUtilizationScanner.cs` for
confession secrets.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 87 | `87-relic-recipes-expansion.md` | Relic restoration | `WorkshopReverseEngineeringSystem` | 9 relics (6 → 15) | P2 | LOW |
| 88 | `88-confession-secrets-expansion.md` | Interpersonal morality | Confession system | 12 secrets (8 → 20) | P2 | LOW |
| 89 | `89-muster-epilogues-expansion.md` | Campaign endings | `EpilogueMatrix` | 13 epilogues (12 → 25) | P2 | LOW |
| 90 | `90-dose-registers-expansion.md` | Radiation management | `DoseRegistersCatalog` | 8 bands + 5 plans | P2 | LOW |
| 91 | `91-greenhouse-items-expansion.md` | Food production | `ItemCatalogLoader` | 16 items (14 → 30) | P3 | LOW |
| 92 | `92-faction-war-dialogue-expansion.md` | Overheard world | `FactionWarContentCatalog` | 22 snippets (18 → 40) | P2 | LOW |
| 93 | `93-verdict-npcs-expansion.md` | Investigation NPCs | `VerdictNpcSystem` | 9 NPCs (6 → 15) | P2 | LOW |
| 94 | `94-verdict-radio-expansion.md` | Machine voice | `VerdictRadioSystem` | 17 broadcasts (13 → 30) | P2 | LOW |
| 95 | `95-journal-voice-prose-expansion.md` | Journal voice | `JournalVoiceProseCatalog` | 12 situation keys | P2 | LOW |
| 96 | `96-epilogue-chronicle-expansion.md` | Ending presentation | `EpilogueChronicleBuilder` | 15 slides (5 → 20) | P3 | LOW |

---

## Dependency graph

```
87 (relic recipes) ──► 47 [batch 2] (collectibles — relics are collectible)
                   ──► 76 [batch 5] (expedition — rare relics at specific sites)
                   ──► 55 [batch 3] (recipes — relic component crafting)
                   ──► 96 (epilogue slides — relic slides show restored artifacts)

88 (confession secrets) ──► 66 [batch 4] (guilt — confessions trigger guilt)
                         ──► 65 [batch 4] (final wishes — confessed secret as wish)
                         ──► 52 [batch 2] (NPC arcs — confessions deepen relationships)
                         ──► 95 (journal voice — confessions trigger journal entries)

89 (muster epilogues) ──► 96 (epilogue slides — slides reference ending keys)
                       ──► 74 [batch 4] (chapters — late chapters gate endings)
                       ──► 66 [batch 4] (guilt — moral choices determine endings)
                       ──► 44 [batch 2] (faction territory — standing determines endings)

90 (dose registers) ──► 81 [batch 5] (dose locations — locations feed bands)
                    ──► existing 09B (radiation — bands classify exposure)
                    ──► 79 [batch 5] (autopsy — terminal-band patients may die)
                    ──► 83 [batch 5] (weather seasons — fallout pushes into higher bands)

91 (greenhouse items) ──► 55 [batch 3] (recipes — greenhouse tools are craftable)
                      ──► 46 [batch 2] (scavenging — greenhouse supplies scavenged)
                      ──► 71 [batch 4] (power grid — greenhouse room draws power)
                      ──► 76 [batch 5] (expedition — agricultural sites yield supplies)

92 (faction war dialogue) ──► 84 [batch 5] (witnesses — dialogue and testimony)
                          ──► 73 [batch 4] (faction radio — radio and dialogue complement)
                          ──► 44 [batch 2] (faction territory — dialogue is location-gated)
                          ──► 52 [batch 2] (NPC arcs — speakers can recur)

93 (verdict NPCs) ──► 82 [batch 5] (Verdict locations — NPCs are site-linked)
                 ──► 84 [batch 5] (witnesses — 3 NPCs double as witnesses)
                 ──► 94 (Verdict radio — NPCs reference broadcasts)
                 ──► 52 [batch 2] (NPC arcs — Verdict NPCs can recur)

94 (verdict radio) ──► 82 [batch 5] (Verdict locations — broadcasts reference sites)
                  ──► 84 [batch 5] (witnesses — broadcasts corroborate/contradict)
                  ──► 73 [batch 4] (faction radio — separate but complementary)
                  ──► 93 (Verdict NPCs — NPCs reference broadcasts)

95 (journal voice prose) ──► 88 (confessions — confessions trigger journal entries)
                        ──► 66 [batch 4] (guilt — guilt triggers moral_compromise)
                        ──► 65 [batch 4] (final wishes — death triggers death_of_survivor)
                        ──► 57 [batch 3] (incidents — incidents trigger situation keys)

96 (epilogue chronicle) ──► 89 (muster epilogues — slides reference endings)
                       ──► 74 [batch 4] (chapters — late chapters gate slides)
                       ──► 87 (relic recipes — relic slides show artifacts)
                       ──► 82 [batch 5] (Verdict locations — investigation slides)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 89** — muster epilogues. The campaign's payoff; 12 → 25 endings
   creates replayability. Pure data, LOW risk. Feeds Plan 96.
2. **Plan 88** — confession secrets. The interpersonal-morality layer; 8 → 20
   makes more survivors carry meaningful secrets. Pure data, LOW risk.
3. **Plan 92** — faction war dialogue. The overheard-world layer; 18 → 40 makes
   factions feel alive. Pure data, LOW risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 87** — relic recipes. The workshop-restoration layer; 6 → 15 relics.
   Depends on Plan 47/76 for collectible/expedition integration. LOW risk.
5. **Plan 95** — journal voice prose. The journal-voice layer; 12 new situation
   keys. Depends on Plan 88/66/65 for event triggers. LOW risk.
6. **Plan 96** — epilogue chronicle. The ending-presentation layer; 5 → 20
   slides. Depends on Plan 89 for ending references. LOW risk.
7. **Plan 90** — dose registers. The radiation-management layer; 4 → 12 bands.
   Depends on Plan 81 for dose locations. LOW risk.

### LATER (do last — depend on earlier batches or are self-contained)
8. **Plan 93** — Verdict NPCs. Depends on Plan 82 for site locations. LOW risk.
9. **Plan 94** — Verdict radio. Depends on Plan 82/84 for site/witness
   references. LOW risk.
10. **Plan 91** — greenhouse items. Depends on Plan 55/46 for
    recipe/scavenging integration. LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Confession → guilt → journal voice → psychological contamination | ConfessionSystem → GuiltSystem → JournalVoice → PsychologicalContamination | 88 → 66 → 95 → 27C |
| Epilogue ending → chronicle slide → campaign chapter → faction territory | EpilogueMatrix → EpilogueChronicleBuilder → NarrativeProgression → FactionTerritory | 89 → 96 → 74 → 44 |
| Relic restoration → collectible → expedition → workshop → epilogue slide | WorkshopReverseEngineering → Collectibles → ExpeditionSystem → EpilogueChronicle | 87 → 47 → 76 → 96 |
| Dose band → dose location → weather season → autopsy → grave | DoseRegisters → DoseLocations → WeatherSystem → AutopsySystem → MemorialSystem | 90 → 81 → 83 → 79 → 69 |
| Verdict NPC → Verdict location → Verdict radio → muster witness → faction dialogue | VerdictNpcSystem → VerdictLocations → VerdictRadio → WitnessCatalog → FactionWarDialogue | 93 → 82 → 94 → 84 → 92 |
| Greenhouse item → crafting recipe → scavenging → power grid → expedition | ItemCatalog → CraftingSystem → ScavengingTable → PowerGrid → ExpeditionSystem | 91 → 55 → 46 → 71 → 76 |
| Journal voice → confession → final wish → death → grave epitaph | JournalVoice → ConfessionSystem → FinalWishSystem → MemorialSystem | 95 → 88 → 65 → 69 |
| Faction dialogue → muster witness → Verdict radio → epilogue ending | FactionWarDialogue → WitnessCatalog → VerdictRadio → EpilogueMatrix | 92 → 84 → 94 → 89 |

---

## Content totals added by this batch

* **+9** relic recipes (6 → 15)
* **+12** confession secrets (8 → 20)
* **+13** muster epilogues (12 → 25)
* **+8** dose register bands (4 → 12) + **+5** care plans (3 → 8)
* **+16** greenhouse items (14 → 30)
* **+22** faction war dialogue snippets (18 → 40)
* **+9** Verdict NPCs (6 → 15)
* **+17** Verdict radio broadcasts (13 → 30)
* **+12** journal voice situation keys (3 → 15, each with 7 personality variants)
* **+15** epilogue chronicle slides (5 → 20)

---

## Combined totals (batches 1–6)

| Batch | Plans | Theme | Key content added |
|---|---|---|---|
| Batch 1 (32–42) | 11 | Scaffolding systems | skill/research/wildlife/excavation/sky-armor/orbital/debt/room catalogs + roadmap |
| Batch 2 (43–53) | 11 | World content | settlements/territory/patrols/scavenging/collectibles/weather/micro-locations/radio/documents/NPCs + roadmap |
| Batch 3 (54–64) | 11 | Thin catalogs | combat/crafting/economy/incidents/encounters/questlines/vehicles/trade/doctrines + roadmap |
| Batch 4 (65–75) | 11 | Narrative depth | wishes/guilt/cassettes/carvings/epitaphs/schedules/power/AI/radio/chapters + roadmap |
| Batch 5 (76–86) | 11 | Exploration & investigation | expeditions/seasons/inks/autopsies/manuals/dose/verdict/weather/witnesses/maps + roadmap |
| Batch 6 (87–97) | 11 | Relics, confessions & endings | relics/confessions/epilogues/dose-bands/greenhouse/dialogue/verdict-npcs/verdict-radio/journal-voice/epilogue-slides + roadmap |
| **Total** | **66 plans** | | **~650+ new content entries across 50+ catalogs** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 87, 91
```

All plans in this batch are pure data (LOW risk). No cross-tool QA required unless a plan
also touches a Core schema field (none in this batch — all extend existing JSON catalogs
within their current schema). The integration-adjacent plans are **87** (relic recipes,
which may need component items in items.json) and **91** (greenhouse items, which may
need to resolve in the main item catalog), both validated via `--data-integrity-selftest`.
