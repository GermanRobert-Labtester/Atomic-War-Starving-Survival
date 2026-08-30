# Roadmap 119 — Batch 8: Moral Echoes, Disease, Expansion Questlines & Locations (Plans 109–118)

> **Scope:** Ten focused execution plans that expand ASHFALL's moral-choice
> echo layer, ambient gossip, phantom-memory triggers, disease catalog, and
> the four coordinated expansion questline/location catalogs (Verdict, Year
> of Ash, Crossing, Holdfast, Standing Record). Every target system is fully
> implemented and wired, but its data catalog is thin (4–10 entries). This
> batch fills those catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems,
> zero save changes. Every plan extends an existing JSON catalog.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `moral_choice_chains.json` (echo_quests) | 32 echo quests | 60 echo quests | 109 |
| `moral_choice_gossip.json` | 5–10 lines/band (21 arrays) | 20 lines/band (21 arrays) | 110 |
| `phantom_triggers.json` | 7 backgrounds | 20 backgrounds | 111 |
| `disease_catalog.json` | 7 diseases | 20 diseases | 112 |
| `verdict_questlines.json` | 8 questlines | 15 questlines | 113 |
| `year_of_ash_questlines.json` | 8 questlines | 15 questlines | 114 |
| `crossing_encounters.json` | 10 encounters + 5 crises | 25 encounters + 12 crises | 115 |
| `deep_lore_locations.json` | 10 locations | 25 locations | 116 |
| `holdfast_quests.json` | 10 quests | 20 quests | 117 |
| `standing_record_quests.json` | 10 quests | 20 quests | 118 |

All target systems confirmed live in `Assets/Ashfall.Core/` via `grep -rl`:
`MoralChoiceSystem.cs`, `MoralChoiceGossipRuntime.cs`,
`PhantomMemoryEngine.cs`, `DiseaseSystem.cs`, `VerdictQuestCatalogLoader.cs`,
`DoorEncounterSystem.cs`, `CrossingSession.cs`,
`DeepLoreLocationCatalogLoader.cs`, `HoldfastCatalog.cs`,
`StandingRecordCatalog.cs`.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 109 | `109-moral-choice-echo-quests-expansion.md` | Moral echoes | `MoralChoiceSystem` | 28 echo quests (32 → 60) | P2 | LOW |
| 110 | `110-moral-choice-gossip-expansion.md` | Ambient gossip | `MoralChoiceGossipRuntime` | ~250 gossip lines (all bands → 20) | P2 | LOW |
| 111 | `111-phantom-triggers-expansion.md` | Survivor memory | `PhantomMemoryEngine` | 13 backgrounds (7 → 20) | P2 | LOW |
| 112 | `112-disease-catalog-expansion.md` | Disease vectors | `DiseaseSystem` | 13 diseases (7 → 20) | P2 | LOW |
| 113 | `113-verdict-questlines-expansion.md` | Verdict cases | `VerdictQuestCatalogLoader` | 7 questlines (8 → 15) | P2 | MEDIUM |
| 114 | `114-year-of-ash-questlines-expansion.md` | Late crises | `DoorEncounterSystem` | 7 questlines (8 → 15) | P2 | MEDIUM |
| 115 | `115-crossing-encounters-expansion.md` | Charter crises | `CrossingSession` | 15 encounters + 7 crises | P2 | MEDIUM |
| 116 | `116-deep-lore-locations-expansion.md` | Exploration | `DeepLoreLocationCatalogLoader` | 15 locations (10 → 25) | P1 | LOW |
| 117 | `117-holdfast-quests-expansion.md` | Ice-road quests | `HoldfastCatalog` | 10 quests (10 → 20) | P2 | MEDIUM |
| 118 | `118-standing-record-quests-expansion.md` | Survey quests | `StandingRecordCatalog` | 10 quests (10 → 20) | P2 | MEDIUM |

---

## Dependency graph

```
109 (moral echoes) ──► 95 [batch 6] (journal voice — echo resolutions trigger entries)
                   ──► 88 [batch 6] (confessions — echo survivors may confess)
                   ──► 89 [batch 6] (epilogues — echo outcomes feed endings)
                   ──► 110 (gossip — echoes propagate as camp chatter)
                   ──► 100 [batch 7] (faction reactions — echo choices shift standing)

110 (gossip) ──► 109 (echoes — gossip references echo outcomes)
             ──► 100 [batch 7] (faction reactions — gossip references standing shifts)
             ──► 95 [batch 6] (journal voice — gossip may unlock entries)
             ──► 92 [batch 6] (faction dialogue — gossip complements dialogue)
             ──► 89 [batch 6] (epilogues — gossip band influences ending tone)

111 (phantom triggers) ──► 66 [batch 4] (guilt — breakdowns generate guilt)
                       ──► 33 [batch 1] (skills — motivation grants skill bonus)
                       ──► 95 [batch 6] (journal voice — phantom moments trigger entries)
                       ──► 88 [batch 6] (confessions — breakdown survivors confess)
                       ──► 109 (echoes — breakdown may trigger a later echo)

112 (disease catalog) ──► 116 (deep lore locations — locations are disease vectors)
                     ──► 48 [batch 2] (weather gates — weather triggers outbreaks)
                     ──► 79 [batch 5] (autopsy procedures — new diseases need autopsies)
                     ──► 81 [batch 5] (dose locations — radiation sickness overlaps dose)
                     ──► 90 [batch 6] (dose registers — chronic disease bands parallel dose)
                     ──► 115 (crossing — quarantine break crisis ties to disease)

113 (verdict questlines) ──► 82 [batch 5] (Verdict locations — cases reference sites)
                         ──► 94 [batch 6] (Verdict radio — cases reference broadcasts)
                         ──► 93 [batch 6] (Verdict NPCs — cases involve NPCs)
                         ──► 109 (echoes — resolutions may trigger echoes)
                         ──► 89 [batch 6] (epilogues — outcomes feed endings)
                         ──► 116 (deep lore locations — cases reference investigation sites)

114 (year of ash questlines) ──► 98 [batch 7] (standing record factions — crises shift standing)
                             ──► 102 [batch 7] (foundry accords — crises produce/break treaties)
                             ──► 76 [batch 5] (expedition destinations — crises unlock sites)
                             ──► 89 [batch 6] (epilogues — outcomes feed endings)
                             ──► 109 (echoes — resolutions may trigger echoes)

115 (crossing encounters) ──► 98 [batch 7] (standing record factions — crises shift standing)
                        ──► 76 [batch 5] (expedition destinations — encounters reveal locations)
                        ──► 102 [batch 7] (foundry accords — crises may produce treaties)
                        ──► 89 [batch 6] (epilogues — Crossing outcomes feed endings)
                        ──► 112 (disease — quarantine break crisis ties to disease)

116 (deep lore locations) ──► 112 (disease — locations are disease vectors)
                        ──► 48 [batch 2] (weather gates — weather modifies access)
                        ──► 76 [batch 5] (expedition destinations — new expedition targets)
                        ──► 113 (verdict questlines — cases reference sites)
                        ──► 117 (holdfast quests — quests target locations)
                        ──► 118 (standing record quests — quests target locations)

117 (holdfast quests) ──► 76 [batch 5] (expedition destinations — quests target locations)
                    ──► 80 [batch 5] (library manuals — quests unlock manuals)
                    ──► 84 [batch 5] (muster witnesses — quests involve witnesses)
                    ──► 89 [batch 6] (epilogues — outcomes feed endings)
                    ──► 109 (echoes — resolutions may trigger echoes)

118 (standing record quests) ──► 98 [batch 7] (standing record factions — quests shift standing)
                           ──► 76 [batch 5] (expedition destinations — quests target locations)
                           ──► 82 [batch 5] (Verdict locations — quests reference sites)
                           ──► 89 [batch 6] (epilogues — outcomes feed endings)
                           ──► 109 (echoes — resolutions may trigger echoes)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 116** — deep lore locations. The single thinnest exploration catalog
   (10 locations) and the only P1 in this batch. Unlocks expedition targets,
   disease vectors, and Verdict investigation sites. Pure data, LOW risk.
   Feeds Plans 112, 113, 117, 118.
2. **Plan 112** — disease catalog. The disease-system pillar; 7 → 20
   diseases tied to world locations and weather. Pure data, LOW risk.
   Depends on Plan 116 for location vectors.
3. **Plan 109** — moral choice echo quests. The "choices that echo" pillar;
   32 → 60 delayed-payoff callbacks. Pure data, LOW risk. Feeds Plans 95,
   88, 89, 110.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 110** — moral choice gossip. The ambient-texture pillar; all 21
   band arrays to 20 lines. Pure data, LOW risk. Depends on Plan 109 for
   echo references.
5. **Plan 111** — phantom triggers. The survivor-memory pillar; 7 → 20
   backgrounds. Pure data, LOW risk. Feeds Plans 66, 33, 95.
6. **Plan 113** — Verdict questlines. The investigation-pillar questlines; 8 →
   15 cases. MEDIUM risk (nested stages). Depends on Plan 116 for
   investigation sites.
7. **Plan 117** — Holdfast quests. The ice-road pillar; 10 → 20 quests.
   MEDIUM risk. Depends on Plan 116 for target locations.
8. **Plan 118** — Standing Record quests. The survey pillar; 10 → 20 quests.
   MEDIUM risk (mutation resolution). Depends on Plan 116 for target
   locations.

### LATER (do last — depend on earlier batches or are structurally complex)
9. **Plan 114** — Year of Ash questlines. The late-campaign crisis pillar;
   8 → 15 questlines. MEDIUM risk. Depends on Plan 98 for faction refs.
10. **Plan 115** — Crossing encounters & crises. The charter-settlement
    pillar; 10+5 → 25+12. MEDIUM risk (choice DTO). Depends on Plan 98 for
    faction refs and Plan 112 for the quarantine crisis.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Moral choice → echo quest → gossip → journal voice → confession → ending | MoralChoiceSystem → EchoQuest → GossipRuntime → JournalVoice → ConfessionSystem → EpilogueMatrix | 109 → 110 → 95 → 88 → 89 |
| Phantom trigger → guilt → confession → journal voice → echo | PhantomMemoryEngine → GuiltSystem → ConfessionSystem → JournalVoice → EchoQuest | 111 → 66 → 88 → 95 → 109 |
| Disease → location → weather gate → autopsy → dose ledger | DiseaseSystem → DeepLoreLocation → WeatherSystem → AutopsySystem → DoseLedger | 112 → 116 → 48 → 79 → 90 |
| Deep lore location → expedition destination → Verdict case → Verdict radio → Verdict NPC → ending | DeepLoreLocation → ExpeditionSystem → VerdictQuest → VerdictRadio → VerdictNPC → EpilogueMatrix | 116 → 76 → 113 → 94 → 93 → 89 |
| Year of Ash crisis → standing record faction → foundry accord → expedition → ending | DoorEncounter → StandingRecordFaction → FoundryAccords → ExpeditionSystem → EpilogueMatrix | 114 → 98 → 102 → 76 → 89 |
| Crossing crisis → standing record faction → foundry accord → disease quarantine → ending | CrossingSession → StandingRecordFaction → FoundryAccords → DiseaseSystem → EpilogueMatrix | 115 → 98 → 102 → 112 → 89 |
| Holdfast quest → expedition destination → library manual → muster witness → ending | HoldfastQuest → ExpeditionSystem → LibraryStudy → MusterWitness → EpilogueMatrix | 117 → 76 → 80 → 84 → 89 |
| Standing Record quest → faction standing → expedition → Verdict location → ending | StandingRecordQuest → StandingRecordFaction → ExpeditionSystem → VerdictLocation → EpilogueMatrix | 118 → 98 → 76 → 82 → 89 |

---

## Content totals added by this batch

* **+28** moral choice echo quests (32 → 60)
* **+~250** moral choice gossip lines (21 arrays, all → 20 lines each)
* **+13** phantom memory trigger backgrounds (7 → 20)
* **+13** diseases (7 → 20)
* **+7** Verdict questlines (8 → 15)
* **+7** Year of Ash questlines (8 → 15)
* **+15** Crossing encounters (10 → 25) + **+7** Crossing crises (5 → 12)
* **+15** deep lore locations (10 → 25)
* **+10** Holdfast quests (10 → 20)
* **+10** Standing Record quests (10 → 20)

---

## Combined totals (batches 1–8)

| Batch | Plans | Theme | Key content added |
|---|---|---|---|
| Batch 1 (32–42) | 11 | Scaffolding systems | skill/research/wildlife/excavation/sky-armor/orbital/debt/room catalogs + roadmap |
| Batch 2 (43–53) | 11 | World content | settlements/territory/patrols/scavenging/collectibles/weather/micro-locations/radio/documents/NPCs + roadmap |
| Batch 3 (54–64) | 11 | Thin catalogs | combat/crafting/economy/incidents/encounters/questlines/vehicles/trade/doctrines + roadmap |
| Batch 4 (65–75) | 11 | Narrative depth | wishes/guilt/cassettes/carvings/epitaphs/schedules/power/AI/radio/chapters + roadmap |
| Batch 5 (76–86) | 11 | Exploration & investigation | expeditions/seasons/inks/autopsies/manuals/dose/verdict/weather/witnesses/maps + roadmap |
| Batch 6 (87–97) | 11 | Relics, confessions & endings | relics/confessions/epilogues/dose-bands/greenhouse/dialogue/verdict-npcs/verdict-radio/journal-voice/epilogue-slides + roadmap |
| Batch 7 (98–108) | 11 | Factions, economy & dose-ledger | sr-factions/economy-tuning/moral-reactions/dose-quests/foundry-accords/treaty-consequences/narrative-questlines/trade-specialties/dose-items/radio-signals + roadmap |
| Batch 8 (109–119) | 11 | Moral echoes, disease & expansion quests | echo-quests/gossip/phantom-triggers/disease/verdict-quests/yoa-quests/crossing-encounters/deep-lore-locations/holdfast-quests/sr-quests + roadmap |
| **Total** | **88 plans** | | **~1100+ new content entries across 70+ catalogs** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 116 (new expedition targets)
```

All plans in this batch are pure data. Plans **113, 114, 115, 117, 118** are
MEDIUM risk due to nested stage/choice structures and cross-references
(nextStageId chains, faction/item/mutation resolution) — validate
incrementally after each questline rather than after the whole file. The
integration-adjacent plans are **116** (deep lore locations, which become
expedition targets, disease vectors, and Verdict investigation sites) and
**112** (disease catalog, whose countermeasure_item_ids must resolve),
validated via `--data-integrity-selftest` and `--expedition-selftest`.
