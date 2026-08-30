# Roadmap 64 — Batch 3: Thin-Catalog Expansion & Scaffolding (Plans 54–63)

> **Scope:** Ten focused execution plans that expand the thinnest verified catalogs in
> ASHFALL — combat, crafting, economy, incidents, encounters, questlines, vehicles,
> trade scenarios, trade tell lines, and warlord doctrines. Every target system is fully
> implemented and wired, but its data catalog is starved. This batch fills those catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems, zero save
> changes. Every plan extends an existing JSON catalog.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `combat_catalog.json` | 5 weapons, 0 enemies | 20 weapons, 12 enemies | 54 |
| `recipes.json` | 39 recipes | 80 recipes | 55 |
| `economy_goods.json` | 16 goods | 40 goods | 56 |
| `incidents.json` | 5 incidents | 25 incidents | 57 |
| `narrative_encounters.json` | 3 encounters | 25 encounters | 58 |
| `dynamic_questlines.json` | 4 questlines | 15 questlines | 59 |
| `vehicles.json` | 3 vehicles | 10 vehicles | 60 |
| `trade_screen_scenarios.json` | 3 scenarios | 15 scenarios | 61 |
| `trade_tell_lines.json` | 4 bands, 0 lines | 4 bands, 60 lines | 62 |
| `warlord_doctrines.json` | 12 doctrines | 24 doctrines | 63 |

All target systems confirmed live in `Assets/Ashfall.Core/` via `find`.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 54 | `54-combat-catalog-expansion.md` | Combat content | `TacticalCombatSystem` | 15 weapons + 12 enemies | P2 | LOW |
| 55 | `55-crafting-recipe-expansion.md` | Crafting depth | `CraftingSystem` | 41 recipes (39 → 80) | P2 | LOW |
| 56 | `56-economy-goods-expansion.md` | Trade depth | `EconomySystem` | 24 goods (16 → 40) | P2 | LOW |
| 57 | `57-incident-expansion.md` | Shelter events | Incident system | 20 incidents (5 → 25) | P2 | LOW |
| 58 | `58-narrative-encounter-expansion.md` | Encounter variety | `NarrativeEncounterSystem` | 22 encounters (3 → 25) | P2 | LOW |
| 59 | `59-dynamic-questline-expansion.md` | Quest content | `QuestlineSystem` | 11 questlines (4 → 15) | P2 | LOW |
| 60 | `60-vehicle-expansion.md` | Vehicle progression | Expedition vehicle system | 7 vehicles (3 → 10) | P2 | LOW |
| 61 | `61-trade-screen-scenarios.md` | Trade variety | `TradeScreenPresenter` | 12 scenarios (3 → 15) | P2 | LOW |
| 62 | `62-trade-tell-lines-expansion.md` | Trader voice | `TradeTellEngine` | 60 tell lines (0 → 60) | P2 | LOW |
| 63 | `63-warlord-doctrines-expansion.md` | Warlord behavior | Faction war system | 12 doctrines (12 → 24) | P2 | LOW |

---

## Dependency graph

```
54 (combat) ──► 45 [batch 2] (patrols — enemies populate patrol combat)
             ──► existing 14 (raids — raiders, scavengers, press gangs)
             ──► 37 [batch 1] (excavation — automated turrets as site defense)

55 (recipes) ──► 33 [batch 1] (skills — skill-gated recipes)
              ──► 34 [batch 1] (research — research-gated recipes)
              ──► 37 [batch 1] (excavation — rare materials for advanced recipes)
              ──► 46 [batch 2] (scavenging — location-specific ingredients)

56 (economy goods) ──► 43 [batch 2] (settlements — trade goods/needs)
                    ──► existing 16B (caravans — goods drive caravan profitability)
                    ──► 40 [batch 1] (debt — goods are debt principals)
                    ──► 61 (trade scenarios — scenarios reference goods)

57 (incidents) ──► 45 [batch 2] (patrols — faction patrol incidents)
                ──► 43 [batch 2] (settlements — refugee approach incidents)
                ──► existing 09A/09B (medical — disease/exposure incidents)
                ──► 71 [batch 4] (power grid — equipment failure incidents)

58 (encounters) ──► 32 [batch 1] (expedition — location-specific encounters)
                ──► 45 [batch 2] (patrols — faction patrol encounters)
                ──► 54 (combat — combat encounters use enemy definitions)
                ──► 52 [batch 2] (NPCs — social encounters introduce NPCs)

59 (questlines) ──► 50 [batch 2] (distress signals — 5 questlines begin with a signal)
                ──► 52 [batch 2] (NPC arcs — 3 questlines are NPC objectives)
                ──► 44 [batch 2] (faction territory — faction quests shift control)

60 (vehicles) ──► 55 (recipes — vehicle repair and crafting)
              ──► 48 [batch 2] (weather gates — terrain_type determines route access)
              ──► 45 [batch 2] (patrols — armored vehicles for faction patrols)
              ──► 43 [batch 2] (settlements — vehicle trade)

61 (trade scenarios) ──► 43 [batch 2] (settlements — default scenario per settlement)
                     ──► 45 [batch 2] (patrols — smuggler/black-market scenarios)
                     ──► 40 [batch 1] (debt — debt collector scenario)
                     ──► 62 (tell lines — scenarios reference tell lines)

62 (tell lines) ──► 61 (scenarios — negotiation options consume tell lines)
                ──► 45 [batch 2] (patrols — patrol negotiation uses tell lines)

63 (warlord doctrines) ──► 45 [batch 2] (patrols — doctrine determines patrol type)
                       ──► existing 14 (raids — raid frequency per doctrine)
                       ──► existing 06C (faction war — escalation triggers)
                       ──► 54 (combat — technologist doctrine uses turrets/drones)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 54** — combat catalog. The most underfed live system (5 weapons, 0 enemies).
   Unblocks Plan 45 patrol combat and existing 14 raids. Pure data, LOW risk.
2. **Plan 55** — crafting recipes. Unlocks the crafting-progression spine; depends on
   Plan 33/34 for skill/research gates. Pure data, LOW risk.
3. **Plan 57** — incidents. Unlocks the shelter-tick-content layer; makes every day
   feel different. Pure data, LOW risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 56** — economy goods. Unblocks Plan 43 settlement trade and caravans. LOW risk.
5. **Plan 58** — narrative encounters. Unblocks Plan 32 location-specific encounters.
   LOW risk.
6. **Plan 59** — dynamic questlines. Unlocks the quest-content pillar; depends on Plan
   50/52 for signal/NPC hooks. LOW risk.
7. **Plan 60** — vehicles. Unlocks the vehicle-progression pillar; depends on Plan 55
   for repair recipes. LOW risk.

### LATER (do last — depend on earlier batches or are self-contained)
8. **Plan 61** — trade scenarios. Depends on Plan 43/56 for settlement/good references.
   LOW risk.
9. **Plan 62** — trade tell lines. Depends on Plan 61 for scenario references. LOW risk.
10. **Plan 63** — warlord doctrines. Depends on Plan 45/54 for patrol/combat integration.
    LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Combat → patrol → raid → war escalation | TacticalCombat → FactionPatrol → RaidSystem → FactionWar | 54 → 45 → 14 → 06C |
| Recipe → skill → research → scavenging → excavation | CraftingSystem → SkillProgression → ResearchSystem → ScavengingTable → ExcavationSystem | 55 → 33 → 34 → 46 → 37 |
| Economy good → settlement → caravan → debt | EconomySystem → SettlementSystem → CaravanSystem → LedgerDebt | 56 → 43 → 16B → 40 |
| Incident → patrol → settlement → medical | IncidentSystem → FactionPatrol → SettlementSystem → MedicalSystem | 57 → 45 → 43 → 09A |
| Encounter → expedition → patrol → NPC → quest | NarrativeEncounter → ExpeditionSystem → FactionPatrol → CharacterSystem → QuestSystem | 58 → 32 → 45 → 52 → 59 |
| Vehicle → recipe → weather gate → patrol | VehicleSystem → CraftingSystem → WeatherSystem → FactionPatrol | 60 → 55 → 48 → 45 |
| Trade scenario → tell lines → patrol → settlement | TradeScreen → TradeTellEngine → FactionPatrol → SettlementSystem | 61 → 62 → 45 → 43 |
| Warlord doctrine → patrol → raid → war | WarlordDoctrine → FactionPatrol → RaidSystem → FactionWar | 63 → 45 → 14 → 06C |

---

## Content totals added by this batch

* **+15** weapons + **+12** enemies (combat catalog)
* **+41** crafting recipes (39 → 80)
* **+24** economy goods (16 → 40)
* **+20** shelter incidents (5 → 25)
* **+22** narrative encounters (3 → 25)
* **+11** dynamic questlines (4 → 15)
* **+7** vehicles (3 → 10)
* **+12** trade screen scenarios (3 → 15)
* **+60** trade tell lines (0 → 60)
* **+12** warlord doctrines (12 → 24)

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 60
```

All plans in this batch are pure data (LOW risk). No cross-tool QA required unless a plan
also touches a Core schema field (none in this batch — all extend existing JSON catalogs
within their current schema).
