# Roadmap 86 — Batch 5: Exploration, Investigation & Progression Catalogs (Plans 76–85)

> **Scope:** Ten focused execution plans that expand ASHFALL's thinnest
> exploration, investigation, and progression catalogs — expedition destinations,
> duty roster seasons, archive inks, autopsy procedures, library manuals, dose
> locations, Verdict investigation sites, weather season windows, muster
> witnesses, and damaged map zones. Every target system is fully implemented
> and wired, but its data catalog is starved (1–4 entries). This batch fills
> those catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems, zero
> save changes. Every plan extends an existing JSON catalog.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `expeditions.json` | 2 destinations | 15 destinations | 76 |
| `duty_roster_seasons.json` | 1 season | 8 seasons | 77 |
| `archive_inks.json` | 3 inks | 12 inks | 78 |
| `autopsy_procedures.json` | 3 procedures | 12 procedures | 79 |
| `library_manuals.json` | 3 manuals | 15 manuals | 80 |
| `dose_locations.json` | 3 locations | 12 locations | 81 |
| `verdict_locations.json` | 4 sites | 15 sites | 82 |
| `weather_seasons.json` | 3 windows | 10 windows | 83 |
| `muster_witnesses.json` | 3 witnesses | 15 witnesses | 84 |
| `damaged_map_zones.json` | 3 zones | 12 zones | 85 |

All target systems confirmed live in `Assets/Ashfall.Core/` via `find`/`grep`:
`ExpeditionCatalogLoader.cs`, `DutyRosterCatalog.cs`, `ArchiveInkCatalogLoader.cs`,
`AutopsySystem.cs`, `LibraryManualCatalogLoader.cs`, `DoseContentCatalog.cs`,
`VerdictCatalogLoader.cs`, `WeatherSystem.cs`, `WitnessCatalog.cs`, plus
`ContentUtilizationScanner.cs` for damaged map zones.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 76 | `76-expedition-destinations-expansion.md` | Surface exploration | `ExpeditionCatalogLoader` | 13 destinations (2 → 15) | P1 | LOW |
| 77 | `77-duty-roster-seasons-expansion.md` | Campaign rhythm | `DutyRosterCatalog` | 7 seasons (1 → 8) | P2 | LOW |
| 78 | `78-archive-inks-expansion.md` | Document preservation | `ArchiveInkCatalogLoader` | 9 inks (3 → 12) | P3 | LOW |
| 79 | `79-autopsy-procedures-expansion.md` | Medical investigation | `AutopsySystem` | 9 procedures (3 → 12) | P2 | LOW |
| 80 | `80-library-manuals-expansion.md` | Knowledge progression | `LibraryStudyHostSession` | 12 manuals (3 → 15) | P2 | LOW |
| 81 | `81-dose-locations-expansion.md` | Radiation cartography | `DoseContentCatalog` | 9 locations (3 → 12) | P2 | LOW |
| 82 | `82-verdict-locations-expansion.md` | Investigation arcs | `VerdictCatalogLoader` | 11 sites (4 → 15) | P2 | LOW |
| 83 | `83-weather-seasons-expansion.md` | Weather progression | `WeatherSystem` | 7 windows (3 → 10) | P2 | LOW |
| 84 | `84-muster-witnesses-expansion.md` | Testimony network | `WitnessCatalog` | 12 witnesses (3 → 15) | P2 | LOW |
| 85 | `85-damaged-map-zones-expansion.md` | Cartographic discovery | Damaged map system | 9 zones (3 → 12) | P2 | LOW |

---

## Dependency graph

```
76 (expedition destinations) ──► 81 (dose locations — expedition sites have dose)
                             ──► 48 [batch 2] (weather gates — 2 destinations gated)
                             ──► 49 [batch 2] (micro-locations — 3 destinations produce discoveries)
                             ──► 82 (Verdict sites — 2 sites are expedition-reachable)
                             ──► 85 (damaged maps — 3 revealed installations are destinations)

77 (duty roster seasons) ──► 70 [batch 4] (schedules — seasons gate schedules)
                         ──► 57 [batch 3] (incidents — seasons modulate frequency)
                         ──► 83 (weather seasons — align for consistent pacing)
                         ──► 74 [batch 4] (chapters — seasons map to chapter transitions)

78 (archive inks) ──► 51 [batch 2] (environmental documents — inks preserve documents)
                  ──► 47 [batch 2] (collectibles — pre-war ink is a rare collectible)
                  ──► 55 [batch 3] (recipes — ink crafting recipes)

79 (autopsy procedures) ──► existing 09 (medical — disease/chemical/pathogen autopsies)
                         ──► 65 [batch 4] (final wishes — autopsy reveals dying condition)
                         ──► 69 [batch 4] (grave epitaphs — cause-of-death links to epitaph)
                         ──► 55 [batch 3] (recipes — autopsy tool crafting)

80 (library manuals) ──► 33 [batch 1] (skills — manuals grant skill XP)
                     ──► 34 [batch 1] (research — manuals unlock research nodes)
                     ──► 71 [batch 4] (power grid — some manuals require powered rooms)
                     ──► 72 [batch 4] (utility AI — study actions reference manuals)

81 (dose locations) ──► 76 (expedition destinations — matching loc_ ids)
                    ──► 48 [batch 2] (weather gates — fallout storms increase dose)
                    ──► 46 [batch 2] (scavenging — high-dose = high-reward)
                    ──► 83 (weather seasons — seasonal fallout shifts dose)

82 (Verdict locations) ──► 76 (exppedition — 2 sites are expedition-reachable)
                       ──► 73 [batch 4] (faction radio — broadcasts reference sites)
                       ──► 84 (witnesses — witnesses at sites provide testimony)
                       ──► 51 [batch 2] (environmental storytelling)

83 (weather seasons) ──► 48 [batch 2] (weather gates — 2 windows block routes)
                    ──► 81 (dose locations — seasonal fallout shifts dose)
                    ──► 77 (duty roster seasons — align for pacing)
                    ──► 74 [batch 4] (chapters — weather marks transitions)

84 (muster witnesses) ──► 82 (Verdict sites — witnesses at investigation sites)
                     ──► 52 [batch 2] (NPC arcs — named witnesses recur)
                     ──► 51 [batch 2] (environmental documents — testimony cross-refs)
                     ──► 73 [batch 4] (faction radio — broadcasts corroborate/contradict)

85 (damaged map zones) ──► 76 (expedition — revealed installations are destinations)
                       ──► 46 [batch 2] (scavenging — fragments are scavenging loot)
                       ──► 47 [batch 2] (collectibles — pre-war maps are collectibles)
                       ──► 51 [batch 2] (environmental storytelling)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 76** — expedition destinations. The single thinnest exploration catalog
   (2 destinations). Unblocks the entire surface-exploration loop. Pure data,
   LOW risk. Feeds Plans 81, 82, 85.
2. **Plan 83** — weather season windows. Fills the mid-campaign weather gap
   (days 61–239 have no window transitions). Pure data, LOW risk. Aligns with
   Plan 77.
3. **Plan 77** — duty roster seasons. The single thinnest campaign-rhythm
   catalog (1 season). Makes the shelter's workload shift across the campaign.
   Pure data, LOW risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 81** — dose locations. Adds surface/expedition radiation tracking.
   Depends on Plan 76 for matching loc_ ids. LOW risk.
5. **Plan 82** — Verdict investigation sites. Deepens the investigation-arc
   pillar. Depends on Plan 76 for 2 expedition-reachable sites. LOW risk.
6. **Plan 84** — muster witnesses. Deepens the testimony-network pillar.
   Depends on Plan 82 for site-linked witnesses. LOW risk.
7. **Plan 85** — damaged map zones. Deepens the cartographic-discovery pillar.
   Depends on Plan 76 for revealed-installation destinations. LOW risk.

### LATER (do last — depend on earlier batches or are self-contained)
8. **Plan 79** — autopsy procedures. Deepens the medical-investigation pillar.
   Depends on existing 09 and Plan 55 for tool crafting. LOW risk.
9. **Plan 80** — library manuals. Deepens the knowledge-progression pillar.
   Depends on Plan 33/34 for skill/research ids. LOW risk.
10. **Plan 78** — archive inks. Deepens the document-preservation pillar.
    Depends on Plan 51/47 for document/collectible integration. LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Expedition destination → dose location → weather gate → scavenging | ExpeditionSystem → DoseLedger → WeatherSystem → ScavengingTable | 76 → 81 → 83 → 46 |
| Verdict site → witness testimony → faction radio → NPC arc → environmental doc | VerdictSystem → WitnessCatalog → RadioTuner → CharacterSystem → EnvironmentalStorytelling | 82 → 84 → 73 → 52 → 51 |
| Damaged map fragment → scavenging → revealed installation → expedition → unique loot | DamagedMapSystem → ScavengingTable → ExpeditionSystem → ItemCatalog | 85 → 46 → 76 → items |
| Weather window → duty roster season → campaign chapter → faction territory | WeatherSystem → DutyRoster → NarrativeProgression → FactionTerritory | 83 → 77 → 74 → 44 |
| Autopsy procedure → medical disease → final wish → grave epitaph | AutopsySystem → MedicalSystem → FinalWishSystem → MemorialSystem | 79 → 09 → 65 → 69 |
| Library manual → skill XP → research unlock → power-gated study room → utility AI | LibraryStudy → SkillProgression → ResearchSystem → PowerGrid → UtilityAI | 80 → 33 → 34 → 71 → 72 |
| Archive ink → document preservation → environmental storytelling → collectible | ArchiveInk → DocumentSystem → EnvironmentalStorytelling → Collectibles | 78 → 51 → 17 → 47 |
| Expedition → Verdict site → witness → faction radio → warlord doctrine | ExpeditionSystem → VerdictSystem → WitnessCatalog → RadioTuner → WarlordDoctrine | 76 → 82 → 84 → 73 → 63 |

---

## Content totals added by this batch

* **+13** expedition destinations (2 → 15)
* **+7** duty roster seasons (1 → 8)
* **+9** archive inks (3 → 12)
* **+9** autopsy procedures (3 → 12)
* **+12** library manuals (3 → 15)
* **+9** dose locations (3 → 12)
* **+11** Verdict investigation sites (4 → 15)
* **+7** weather season windows (3 → 10)
* **+12** muster witnesses (3 → 15)
* **+9** damaged map zones (3 → 12)

---

## Combined totals (batches 1–5)

| Batch | Plans | Theme | Key content added |
|---|---|---|---|
| Batch 1 (32–42) | 11 | Scaffolding systems | skill/research/wildlife/excavation/sky-armor/orbital/debt/room catalogs + roadmap |
| Batch 2 (43–53) | 11 | World content | settlements/territory/patrols/scavenging/collectibles/weather/micro-locations/radio/documents/NPCs + roadmap |
| Batch 3 (54–64) | 11 | Thin catalogs | combat/crafting/economy/incidents/encounters/questlines/vehicles/trade/doctrines + roadmap |
| Batch 4 (65–75) | 11 | Narrative depth | wishes/guilt/cassettes/carvings/epitaphs/schedules/power/AI/radio/chapters + roadmap |
| Batch 5 (76–86) | 11 | Exploration & investigation | expeditions/seasons/inks/autopsies/manuals/dose/verdict/weather/witnesses/maps + roadmap |
| **Total** | **55 plans** | | **~500+ new content entries across 40+ catalogs** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 76, 82, 85
```

All plans in this batch are pure data (LOW risk). No cross-tool QA required unless a plan
also touches a Core schema field (none in this batch — all extend existing JSON catalogs
within their current schema). The integration-adjacent plans are **76** (expedition
destinations), **82** (Verdict sites), and **85** (damaged maps), which must be validated
against `--expedition-selftest` if their locations are dispatched to, but the plans
themselves only add JSON entries to existing schemas.
