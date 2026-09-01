# Roadmap 75 — Batch 4: Narrative Depth, Shelter Systems & Campaign Spine (Plans 65–74)

> **Scope:** Ten focused execution plans that deepen ASHFALL's narrative and shelter
> texture — final wishes, guilt sources, cassette sets, wall carvings, grave epitaphs,
> shelter schedules, power-grid rooms, utility-AI actions, faction radio broadcasts, and
> campaign chapters. Every target system is fully implemented and wired, but its data
> catalog is thin or nearly empty. This batch fills those catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems, zero save
> changes. Every plan extends an existing JSON catalog.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `final_wishes.json` | 8 wishes | 30 wishes | 65 |
| `guilt_sources.json` | 20 triggers | 40 triggers | 66 |
| `cassette_sets.json` | 4 sets | 12 sets | 67 |
| `wall_carving_templates.json` | 3 bands, few templates | 3 bands, 60 templates | 68 |
| `wasteland_grave_epitaphs.json` | 8 epitaphs | 30 epitaphs | 69 |
| `shelter_schedules.json` | 3 schedules | 12 schedules | 70 |
| `power_grid.json` | 6 rooms | 18 rooms | 71 |
| `utility_actions.json` | 6 actions | 20 actions | 72 |
| `faction_radio_corpus.json` | ~0 broadcasts (silence only) | 30 broadcasts | 73 |
| `narrative_progression.json` | 5 chapters | 15 chapters | 74 |

All target systems confirmed live in `Assets/Ashfall.Core/` via `find`/`grep`:
`FinalWishSystem.cs`, `MemorialSystem.cs`, `PowerGridSystem.cs`, `CohortSystem.cs`,
`LandmarkDegradationSystem.cs`, `LocationEvolutionSystem.cs`,
`NarrativeEncounterSystem.cs`, `DoorEncounterSystem.cs`, plus `UtilityAI/` in both
Core and Godot host.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 65 | `65-final-wishes-expansion.md` | Death meaning | `FinalWishSystem` | 22 wishes (8 → 30) | P2 | LOW |
| 66 | `66-guilt-sources-expansion.md` | Moral weight | Guilt system | 20 triggers (20 → 40) | P2 | LOW |
| 67 | `67-cassette-sets-expansion.md` | Audio discovery | `VinylMoraleSystem` | 8 sets (4 → 12) | P2 | LOW |
| 68 | `68-wall-carving-templates-expansion.md` | Shelter texture | Shelter-as-character | 57 templates (3 → 60) | P2 | LOW |
| 69 | `69-grave-epitaphs-expansion.md` | Memorial texture | `MemorialSystem` | 22 epitaphs (8 → 30) | P2 | LOW |
| 70 | `70-shelter-schedules-expansion.md` | Shelter rhythm | Duty-roster system | 9 schedules (3 → 12) | P2 | LOW |
| 71 | `71-power-grid-rooms-expansion.md` | Power management | `PowerGridSystem` | 12 rooms (6 → 18) | P2 | LOW |
| 72 | `72-utility-ai-actions-expansion.md` | Survivor autonomy | Utility AI | 14 actions (6 → 20) | P2 | LOW |
| 73 | `73-faction-radio-corpus-expansion.md` | Radio intel | `RadioTuner` / HUD chatter | 30 broadcasts (0 → 30) | P2 | LOW |
| 74 | `74-narrative-progression-chapters.md` | Campaign spine | Narrative progression | 10 chapters (5 → 15) | P2 | LOW |

---

## Dependency graph

```
65 (final wishes) ──► 69 (graves — wish completion can produce a grave epitaph)
                   ──► 52 [batch 2] (NPC arcs — dying NPCs issue final wishes)
                   ──► existing 27C (psychological contamination — unmet wishes → guilt)
                   ──► 66 (guilt — refusing a wish is a guilt source)

66 (guilt) ──► 65 (final wishes — refusing a wish triggers guilt)
           ──► existing 27C (psychological contamination)
           ──► 59 [batch 3] (questlines — moral-branch quests generate guilt)
           ──► 58 [batch 3] (encounters — moral encounters generate guilt)

67 (cassette sets) ──► 47 [batch 2] (collectibles — cassettes are collectible items)
                    ──► existing 06B (vinyl/echo morale — cassette sets feed morale)
                    ──► 46 [batch 2] (scavenging — cassette parts are location loot)
                    ──► 73 (faction radio — cassette lore cross-references broadcasts)

68 (wall carvings) ──► 70 (schedules — schedule affects morale → carving band)
                   ──► existing 29A (shelter-as-character)
                   ──► 71 (power grid — power failure lowers morale → low-band carvings)

69 (grave epitaphs) ──► 65 (final wishes — completed wish → personalized epitaph)
                   ──► 49 [batch 2] (micro-locations — graves are micro-locations)
                   ──► existing 17 (environmental storytelling)

70 (shelter schedules) ──► 41 [batch 1] (shelter rooms — rooms define shift work)
                        ──► 71 (power grid — schedule determines room active hours)
                        ──► existing 12B (duty roster — schedules drive roster)
                        ──► 72 (utility AI — schedule gates action availability)

71 (power grid rooms) ──► 41 [batch 1] (shelter rooms — new rooms need power entries)
                     ──► 70 (schedules — schedule sets room power priority)
                     ──► 57 [batch 3] (incidents — equipment failure incidents)
                     ──► 68 (wall carvings — blackout → low-morale carvings)

72 (utility AI actions) ──► 70 (schedules — schedule gates action timing)
                       ──► 55 [batch 3] (recipes — craft actions reference recipes)
                       ──► existing H10/H11 (needs/journal — actions satisfy needs)
                       ──► 41 [batch 1] (shelter rooms — room-specific actions)

73 (faction radio) ──► 50 [batch 2] (distress signals — broadcasts trigger signals)
                  ──► 45 [batch 2] (patrols — broadcasts reveal patrol locations)
                  ──► 24 (radio signals — faction broadcasts are interceptable)
                  ──► 63 [batch 3] (warlord doctrines — doctrine shapes broadcast tone)
                  ──► 67 (cassette sets — lore cross-references)

74 (campaign chapters) ──► 73 (faction radio — chapter gates broadcast availability)
                       ──► 59 [batch 3] (questlines — chapters gate questline arcs)
                       ──► 44 [batch 2] (faction territory — chapters shift control)
                       ──► existing 15 (endgame meta — late chapters are endgame)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 65** — final wishes. The emotional core of ASHFALL; 8 → 30 makes every death
   meaningful. Pure data, LOW risk. Unblocks Plan 69 graves and Plan 66 guilt.
2. **Plan 66** — guilt sources. The moral-weight layer; 20 → 40 makes more choices
   carry weight. Pure data, LOW risk. Pairs with Plan 65.
3. **Plan 68** — wall carvings. The shelter-texture layer; 3 → 60 templates makes the
   walls always have something new to say. Pure data, LOW risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 69** — grave epitaphs. The memorial-texture layer; 8 → 30 ensures variety
   across hundreds of grave encounters. Depends on Plan 65 for wish-linked epitaphs.
   LOW risk.
5. **Plan 70** — shelter schedules. The shelter-rhythm layer; 3 → 12 makes the shelter
   adapt to campaign phase. Depends on Plan 41 rooms. LOW risk.
6. **Plan 71** — power grid rooms. The power-management layer; 6 → 18 makes shelter
   growth a power challenge. Depends on Plan 41 and 70. LOW risk.
7. **Plan 72** — utility AI actions. The survivor-autonomy layer; 6 → 20 makes the
   shelter feel inhabited. Depends on Plan 70 and 55. LOW risk.

### LATER (do last — depend on earlier batches or are self-contained)
8. **Plan 67** — cassette sets. The audio-discovery layer; 4 → 12 sets. Depends on
   Plan 47 collectibles and 46 scavenging. LOW risk.
9. **Plan 73** — faction radio. The radio-intel layer; 0 → 30 broadcasts. Depends on
   Plan 50/45/63 for signal/patrol/doctrine references. LOW risk.
10. **Plan 74** — campaign chapters. The campaign-spine layer; 5 → 15 chapters. Depends
    on Plan 73/59/44 for chapter-gated content. LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Final wish → guilt → psychological contamination → grave | FinalWishSystem → GuiltSystem → PsychologicalContamination → MemorialSystem | 65 → 66 → 27C → 69 |
| Wall carving → morale → schedule → power grid → incident | ShelterCarvings → NeedsSystem/Morale → DutyRoster → PowerGridSystem → IncidentSystem | 68 → 70 → 71 → 57 |
| Cassette set → collectible → scavenging → faction radio → lore | VinylMoraleSystem → Collectibles → ScavengingTable → RadioTuner | 67 → 47 → 46 → 73 |
| Utility AI action → schedule → recipe → shelter room → needs | UtilityAI → DutyRoster → CraftingSystem → ShelterRoom → NeedsSystem | 72 → 70 → 55 → 41 → H10 |
| Faction radio → distress signal → patrol → warlord doctrine → war | RadioTuner → SignalIntel → FactionPatrol → WarlordDoctrine → FactionWar | 73 → 50 → 45 → 63 → 06C |
| Campaign chapter → questline → faction territory → endgame | NarrativeProgression → QuestlineSystem → FactionTerritory → EndgameMeta | 74 → 59 → 44 → 15 |
| Power grid → shelter room → schedule → wall carving (blackout) | PowerGridSystem → ShelterRoom → DutyRoster → ShelterCarvings | 71 → 41 → 70 → 68 |
| Grave epitaph → final wish → NPC arc → memorial | MemorialSystem → FinalWishSystem → CharacterSystem → EnvironmentalStorytelling | 69 → 65 → 52 → 17 |

---

## Content totals added by this batch

* **+22** final wishes (8 → 30)
* **+20** guilt triggers (20 → 40)
* **+8** cassette sets (4 → 12)
* **+57** wall carving templates (3 → 60)
* **+22** grave epitaphs (8 → 30)
* **+9** shelter schedules (3 → 12)
* **+12** power-grid rooms (6 → 18)
* **+14** utility-AI actions (6 → 20)
* **+30** faction radio broadcasts (0 → 30)
* **+10** campaign chapters (5 → 15)

---

## Combined totals (batches 1–4)

| Batch | Plans | Theme | Key content added |
|---|---|---|---|
| Batch 1 (32–42) | 11 | Scaffolding systems | skill/research/wildlife/excavation/sky-armor/orbital/debt/room catalogs + roadmap |
| Batch 2 (43–53) | 11 | World content | settlements/territory/patrols/scavenging/collectibles/weather/micro-locations/radio/documents/NPCs + roadmap |
| Batch 3 (54–64) | 11 | Thin catalogs | combat/crafting/economy/incidents/encounters/questlines/vehicles/trade/doctrines + roadmap |
| Batch 4 (65–75) | 11 | Narrative depth | wishes/guilt/cassettes/carvings/epitaphs/schedules/power/AI/radio/chapters + roadmap |
| **Total** | **44 plans** | | **~400+ new content entries across 30+ catalogs** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plan 71 power-grid room wiring
```

All plans in this batch are pure data (LOW risk). No cross-tool QA required unless a plan
also touches a Core schema field (none in this batch — all extend existing JSON catalogs
within their current schema). The one integration-adjacent plan is **71** (power-grid
rooms), which must be validated against `--expedition-selftest` if room draw affects
expedition readiness, but the plan itself only adds JSON entries to the existing
`power_grid.json` schema.
