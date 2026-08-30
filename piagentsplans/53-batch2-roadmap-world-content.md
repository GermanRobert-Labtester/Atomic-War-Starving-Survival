# Roadmap 53 — Batch 2: World Content Additions (Plans 43–52)

> **Scope:** Ten focused execution plans that add playable world content — living
> settlements, faction territory and patrols, location-specific scavenging, collectibles,
> weather-gated routes, micro-locations, radio distress signals, environmental-storytelling
> documents, and recurring NPC arcs. Together with Batch 1 (Plans 32–41), this transforms
> ASHFALL from "a collection of systems" into "a dense, interconnected, reactive world."
>
> **Bias:** ~85% content/data/narrative work, ~15% integration wiring. Every plan reuses
> existing systems; no new systems are invented. Where a system needs a loader or schema
> extension, it is flagged and the change is mechanical.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current state | Plan |
|---|---|---|
| `settlements.json` | **MISSING** (no living communities) | 43 |
| `faction_territory.json` | **MISSING** (19 factions, no territory) | 44 |
| `faction_patrols.json` | **MISSING** (no patrol encounters) | 45 |
| `scavenging_tables.json` | **MISSING** (generic `lootCategories` strings) | 46 |
| `collectibles.json` | **MISSING** (1 vinyl, no culture) | 47 |
| `weather_route_gates.json` | **MISSING** (weather has no world gates) | 48 |
| `micro_locations.json` | **MISSING** (no travel discoveries) | 49 |
| `radio_distress_signals.json` | **5** entries | 50 |
| Environmental documents | thin (272 narrative files, few readable docs) | 51 |
| `characters.json` | **36** NPCs (no temporal arcs) | 52 |

---

## Plan index

| # | File | Theme | Content added | Priority | Risk |
|---|---|---|---|---|---|
| 43 | `43-settlements-catalog.md` | Living communities | 12 settlements | P2 | LOW |
| 44 | `44-faction-territory-map.md` | Faction geography | 19 territories + 5 contested zones | P2 | LOW |
| 45 | `45-faction-patrol-encounters.md` | Faction behavior | 15 patrol templates | P2 | MEDIUM |
| 46 | `46-scavenging-tables.md` | Scavenging depth | 20 location-specific loot tables | P2 | MEDIUM |
| 47 | `47-collectibles-world-culture.md` | World culture | 40 collectibles across 16 categories | P2 | LOW |
| 48 | `48-weather-route-gates.md` | Weather as content | 15 weather-gated routes/locations | P2 | MEDIUM |
| 49 | `49-micro-location-discovery.md` | Travel texture | 25 micro-locations | P2 | MEDIUM |
| 50 | `50-radio-distress-signal-expansion.md` | Radio quests | 25 distress signals (5 → 25) | P2 | LOW |
| 51 | `51-environmental-storytelling-documents.md` | World history | 30 documents + journal unlocks | P2 | LOW |
| 52 | `52-recurring-npc-arcs.md` | NPC continuity | 24 recurring NPCs with temporal arcs (36 → 60) | P2 | MEDIUM |

---

## Dependency graph (within batch + cross-batch links)

```
43 (settlements) ──► 44 (territory — settlements anchor control points)
                ──► 45 (patrols — patrols originate from settlements)
                ──► existing 16B (caravans — settlements are caravan endpoints)

44 (territory) ──► 45 (patrols — patrols operate in territory)
              ──► 40 [batch 1] (debt — default shifts control strength)
              ──► existing 06C (faction war — contested zones are flashpoints)

45 (patrols) ──► 32 [batch 1] (expedition wiring — patrols appear as encounters)
            ──► existing 16B (caravans — escort/supply patrols on routes)
            ──► 33 [batch 1] (skills — patrol negotiation uses skill checks)

46 (scavenging) ──► 32 [batch 1] (expedition wiring — lootCategories reference tables)
               ──► 47 (collectibles — collectibles are rare table entries)
               ──► 51 (documents — documents are rare table entries)
               ──► existing 09A (disease — contaminated loot hazard)

47 (collectibles) ──► 46 (scavenging — collectibles in tables)
                 ──► 34 [batch 1] (research — manuals unlock nodes)
                 ──► 32 [batch 1] (expedition — maps reveal destinations)
                 ──► existing 17C (codex — collectibles unlock entries)

48 (weather gates) ──► 32 [batch 1] (expedition — gates block destinations)
                   ──► 33 [batch 1] (skills — override bypasses gates)
                   ──► existing 16B (caravans — reroute around gates)
                   ──► existing 19A (forecasting — plan around gates)

49 (micro-locations) ──► 32 [batch 1] (expedition — appear on travel routes)
                    ──► 46 (scavenging — micro-location loot)
                    ──► existing 17A (environmental storytelling)

50 (distress signals) ──► 52 (NPCs — rescued senders become recurring)
                     ──► 32 [batch 1] (expedition — rescue missions dispatch)
                     ──► existing 24A (radio — signals on shortwave)
                     ──► existing 11B (cipher — encrypted bursts)

51 (documents) ──► 46 (scavenging — documents in tables)
              ──► existing 17A/B/C (environmental storytelling + codex)
              ──► existing 25A (faction history)
              ──► existing 15B (verdict evidence)

52 (NPC arcs) ──► 50 (distress signals — 4 arcs begin with a signal)
             ──► 33 [batch 1] (skills — NPC skill references)
             ──► 44/45 (faction — NPC affiliation affects patrols)
             ──► existing 12A (generational — child NPC arc)
             ──► existing 30C (belief — priest NPC arc)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 43** — settlements. Gives the world living communities; unblocks 44/45 and
   caravans. Pure data, LOW risk.
2. **Plan 50** — radio distress signals. Extends an existing catalog (5 → 25); unblocks
   52 NPC arcs and rescue missions. Pure data, LOW risk.
3. **Plan 51** — environmental storytelling documents. Core tone delivery; pure data +
   narrative, LOW risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 46** — scavenging tables. Deepens Plan 32 loot; unblocks 47/51 (collectibles
   and documents slot into tables). MEDIUM risk (loot-resolver question).
5. **Plan 47** — collectibles. Depends on 46 (tables) and 34 (research unlocks). LOW risk.
6. **Plan 44** — faction territory. Depends on 43 (settlements anchor control). LOW risk.
7. **Plan 45** — faction patrols. Depends on 44 (territory) and 33 (skill checks). MEDIUM
   risk (encounter-schema question).

### LATER (do last — require integration or schema extension)
8. **Plan 48** — weather route gates. Requires expedition system weather-gating support.
   MEDIUM risk.
9. **Plan 49** — micro-locations. Requires expedition travel-tick encounter support.
   MEDIUM risk.
10. **Plan 52** — recurring NPC arcs. Requires character schema arc-state support; depends
    on 50 (distress signals), 33 (skills), 44/45 (faction). MEDIUM risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Settlement → territory → patrol → encounter → reputation | SettlementSystem → FactionTerritory → FactionPatrol → EncounterSystem → ReputationSystem | 43 → 44 → 45 → 32 → 40 |
| Scavenging table → collectible → research unlock → location reveal | ScavengingTable → Collectible → ResearchSystem → ExpeditionSystem | 46 → 47 → 34 → 32 |
| Weather gate → expedition block → skill override → caravan reroute | WeatherSystem → ExpeditionSystem → SkillSystem → CaravanSystem | 48 → 32 → 33 → 16B |
| Distress signal → radio → expedition → NPC rescue → recurring arc | RadioTuner → SignalIntel → ExpeditionSystem → CharacterSystem | 50 → 24A → 32 → 52 |
| Document → scavenging → journal unlock → faction history → quest hook | ScavengingTable → JournalSystem → FactionSystem → QuestSystem | 51 → 46 → 17C → 25A |
| Micro-location → travel encounter → loot → rumor → quest hook | ExpeditionSystem → EncounterSystem → ItemSystem → QuestSystem | 49 → 32 → 46 → existing |

---

## Content totals added by this batch

* **+12** living settlements (trade posts, strongholds, refugee camps, communities)
* **+19** faction territories + **+5** contested zones
* **+15** faction patrol encounter templates (8 patrol types)
* **+20** location-specific scavenging tables (hospital, rail yard, school, depot, etc.)
* **+40** collectibles across 16 categories (vinyl, photos, books, manuals, maps, etc.)
* **+15** weather-gated routes/locations (blizzard, fog, black rain, EMP, severe cold)
* **+25** micro-locations (roadside memorials, crashed trucks, frozen buses, shrines, etc.)
* **+20** radio distress signals (5 → 25; genuine, stale, trap, false-flag, encrypted)
* **+30** environmental-storytelling documents + journal unlocks
* **+24** recurring NPCs with temporal arcs (36 → 60; 8 designed cross-system arcs)

---

## Combined totals (Batch 1 + Batch 2)

| Category | Batch 1 | Batch 2 | Total |
|---|---|---|---|
| Expedition destinations | +48 | — | +48 |
| Skill definitions | +50 | — | +50 |
| Research nodes | +40 | — | +40 |
| Wildlife migrations | +12 | — | +12 |
| Traps + prey | +10 + 15 | — | +25 |
| Excavation sites | +8 | — | +8 |
| Sky-armor configs + threats | +6 + 10 | — | +16 |
| Telemetry events + consequences | +12 + 8 | — | +20 |
| Debt templates + consequences | +15 + 10 | — | +25 |
| Shelter rooms + rules | +20 + 12 | — | +32 |
| Settlements | — | +12 | +12 |
| Faction territories + contested zones | — | +19 + 5 | +24 |
| Faction patrol templates | — | +15 | +15 |
| Scavenging tables | — | +20 | +20 |
| Collectibles | — | +40 | +40 |
| Weather gates | — | +15 | +15 |
| Micro-locations | — | +25 | +25 |
| Radio distress signals | — | +20 | +20 |
| Environmental documents | — | +30 | +30 |
| Recurring NPCs | — | +24 | +24 |
| **Invariant violations closed** | **8** | — | **8** |
| **New JSON catalogs created** | **9** | **7** | **16** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 46, 48, 49
```

Any plan touching ≥2 coupled variables (e.g. Plan 45 encounter schema + faction fields, or
Plan 52 character schema + arc states) requires **cross-tool QA** (implementer ≠ reviewer)
per `AGENTS.md`.

---

## Relationship to the master roadmap (Plan 31)

These 20 plans (32–52) are the **execution-ready decomposition** of the W-plans in
`31-world-content-master-roadmap.md`:

| Batch plan | W-plan(s) in roadmap 31 |
|---|---|
| 32 (expedition wiring) | W32 (scaffolding) |
| 33 (skills) | W12 (underused systems) |
| 34 (research) | W13 (underused systems) |
| 35 (migration) | W14 (underused systems) |
| 36 (trapping) | W14 (underused systems) |
| 37 (excavation) | W15 (underused systems) |
| 38 (sky armor) | W16 (underused systems) |
| 39 (telemetry) | W17 (underused systems) |
| 40 (debt) | W18 (underused systems) |
| 41 (rooms) | W19 (underused systems) |
| 43 (settlements) | W45 (settlements pillar) |
| 44 (territory) | W43 (faction territorialization) |
| 45 (patrols) | W43 (faction territorialization) |
| 46 (scavenging) | W20 (scavenging depth) |
| 47 (collectibles) | W21 (collectibles + world culture) |
| 48 (weather gates) | W48 (weather as content gate) |
| 49 (micro-locations) | W11 (micro-location discovery) |
| 50 (distress signals) | W50 (radio distress expansion) |
| 51 (documents) | W17 (environmental storytelling) |
| 52 (NPC arcs) | W44 (recurring NPC arcs) |

Each batch plan is a standalone, hand-off-ready execution document. The roadmap files
(42, 53) provide the dependency-aware sequencing and cross-system chain visibility.
