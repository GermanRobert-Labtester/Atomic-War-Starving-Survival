# Roadmap 130 — Batch 9: Faction Branches, Crossing Economy, War Overrides & Foundry Production (Plans 120–129)

> **Scope:** Ten focused execution plans that expand ASHFALL's faction
> character-branch arcs (independent, military, rebel), Crossing settlement
> factions and items, faction-war location overrides, moral-choice persistent
> flags, Verdict machine corpus and history ladder, Holdfast faction flavor,
> and Foundry manufacturable products. Every target system is fully
> implemented and wired, but its data catalog is thin (1–11 entries). This
> batch fills those catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems,
> zero save changes. Every plan extends an existing JSON catalog. Three
> plans (121–123) may require minor additions to static `*BranchIds.cs`
> classes — flagged as minor integration, not new systems.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `crossing_factions.json` | 3 factions | 8 factions | 120 |
| `independent_faction_branch.json` | 8 branches | 15 branches | 121 |
| `military_faction_branch.json` | 8 branches | 15 branches | 122 |
| `rebel_faction_branch.json` | 8 branches | 15 branches | 123 |
| `faction_war_location_overrides.json` | 9 overrides | 20 overrides | 124 |
| `moral_choice_flags.json` | 10 flags | 25 flags | 125 |
| `crossing_items.json` | 11 items | 25 items | 126 |
| `verdict_data.json` (corpus + ladder) | 8 corpus + 6 ladder | 25 corpus + 12 ladder | 127 |
| `holdfast_flavor.json` (factions) | 3 factions | 8 factions | 128 |
| `foundry_production.json` | 11 products | 20 products | 129 |

All target systems confirmed live in `Assets/Ashfall.Core/` or `src/` via
`grep -rl`: `CrossingCatalog.cs`, `IndependentBranchCatalog.cs`,
`MilitaryBranchCatalog.cs`, `RebelBranchCatalog.cs`,
`FactionWarContentCatalog.cs`, `MoralChoiceFlagCatalogLoader.cs`,
`ItemCatalogLoader.cs`, `EvidenceLedger.cs`, `MachineLogSystem.cs`,
`HoldfastDispatchLog.cs`, `SilentFoundrySystem.Heat.cs`.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 120 | `120-crossing-factions-expansion.md` | Crossing factions | `CrossingCatalog` | 5 factions (3 → 8) | P2 | LOW |
| 121 | `121-independent-faction-branch-expansion.md` | Independent arcs | `IndependentBranchCatalog` | 7 branches (8 → 15) | P2 | LOW |
| 122 | `122-military-faction-branch-expansion.md` | Military arcs | `MilitaryBranchCatalog` | 7 branches (8 → 15) | P2 | LOW |
| 123 | `123-rebel-faction-branch-expansion.md` | Rebel arcs | `RebelBranchCatalog` | 7 branches (8 → 15) | P2 | LOW |
| 124 | `124-faction-war-location-overrides-expansion.md` | War territory | `FactionWarContentCatalog` | 11 overrides (9 → 20) | P2 | LOW |
| 125 | `125-moral-choice-flags-expansion.md` | Moral memory | `MoralChoiceFlagCatalogLoader` | 15 flags (10 → 25) | P1 | LOW |
| 126 | `126-crossing-items-expansion.md` | Crossing economy | `ItemCatalogLoader` | 14 items (11 → 25) | P2 | LOW |
| 127 | `127-verdict-data-corpus-ladder-expansion.md` | Verdict machine | `EvidenceLedger`/`MachineLogSystem` | 17 corpus + 6 ladder | P2 | LOW |
| 128 | `128-holdfast-flavor-factions-expansion.md` | Holdfast voice | `HoldfastDispatchLog` | 5 factions (3 → 8) | P2 | LOW |
| 129 | `129-foundry-production-expansion.md` | Foundry products | `SilentFoundrySystem` | 9 products (11 → 20) | P2 | LOW |

---

## Dependency graph

```
120 (crossing factions) ──► 115 [batch 8] (crossing encounters — factions appear in encounters/crises)
                        ──► 98 [batch 7] (standing record factions — Crossing factions overlap)
                        ──► 126 (crossing items — factions want/offer new items)
                        ──► 102 [batch 7] (foundry accords — Crossing factions sign treaties)

121 (independent branches) ──► 109 [batch 8] (echo quests — ponr triggers fire echoes)
                           ──► 89 [batch 6] (epilogues — branch endings feed epilogue matrix)
                           ──► 125 (moral choice flags — ponr_flags register)
                           ──► 95 [batch 6] (journal voice — ponr moments trigger entries)

122 (military branches) ──► 114 [batch 8] (Year of Ash questlines — crises trigger military ponr)
                       ──► 89 [batch 6] (epilogues — military endings feed epilogue matrix)
                       ──► 125 (moral choice flags — ponr_flags register)
                       ──► 98 [batch 7] (standing record factions — military standing shifts)

123 (rebel branches) ──► 124 (faction war location overrides — rebel actions trigger ponr)
                    ──► 89 [batch 6] (epilogues — rebel endings feed epilogue matrix)
                    ──► 125 (moral choice flags — ponr_flags register)
                    ──► 102 [batch 7] (foundry accords — rebel negotiators sign treaties)

124 (faction war location overrides) ──► 116 [batch 8] (deep lore locations — overridden locations)
                                    ──► 114 [batch 8] (Year of Ash questlines — crises produce overrides)
                                    ──► 115 [batch 8] (crossing encounters — Crossing locations overridden)
                                    ──► 123 (rebel branches — rebel territorial actions trigger)
                                    ──► 85 [batch 5] (damaged map zones — overrides complement zones)

125 (moral choice flags) ──► 109 [batch 8] (echo quests — echoes reference flags)
                       ──► 121/122/123 (faction branches — ponr_flags reference flags)
                       ──► 100 [batch 7] (faction reactions — reactions fire on flags)
                       ──► 110 [batch 8] (gossip — gossip references flags)
                       ──► 89 [batch 6] (epilogues — flag state determines ending eligibility)

126 (crossing items) ──► 120 (crossing factions — factions want/offer items)
                   ──► 115 [batch 8] (crossing encounters — encounters grant/require items)
                   ──► 116 [batch 8] (deep lore locations — Crossing items in loot tables)
                   ──► 99 [batch 7] (hardcore economy tuning — Crossing items get price tiers)
                   ──► 105 [batch 7] (trade specialties — Crossing items match professions)

127 (verdict corpus + ladder) ──► 113 [batch 8] (Verdict questlines — questlines reference ladder)
                            ──► 116 [batch 8] (deep lore locations — ladder sites are deep lore)
                            ──► 94 [batch 6] (Verdict radio — corpus complements broadcasts)
                            ──► 82 [batch 5] (Verdict locations — ladder discovery sites)
                            ──► 89 [batch 6] (epilogues — ladder completion feeds endings)

128 (holdfast flavor factions) ──► 117 [batch 8] (Holdfast quests — quests reference flavored factions)
                             ──► 120 (crossing factions — Holdfast/Crossing overlap)
                             ──► 92 [batch 6] (faction war dialogue — Holdfast dialogue)
                             ──► 95 [batch 6] (journal voice — transactions trigger journal)

129 (foundry production) ──► 102 [batch 7] (foundry accords — treaty-bound products reference treaties)
                      ──► 116 [batch 8] (deep lore locations — foundry products in industrial loot)
                      ──► 55 [batch 3] (recipes — foundry products complement crafting recipes)
                      ──► 99 [batch 7] (hardcore economy tuning — foundry products get price tiers)
                      ──► 105 [batch 7] (trade specialties — foundry products match professions)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 125** — moral choice flags. The persistent-memory layer of the
   moral system and the only P1 in this batch. Unblocks echo quests,
   faction branches, faction reactions, gossip, and epilogues. Pure data,
   LOW risk. Feeds Plans 109, 121–123, 100, 110, 89.
2. **Plan 120** — crossing factions. The thinnest faction catalog (3
   factions). Unlocks crossing encounters and crossing items. Pure data,
   LOW risk. Feeds Plans 115, 126.
3. **Plan 126** — crossing items. The Crossing economy pillar; 11 → 25
   items. Depends on Plan 120 for faction wants/offers. Pure data, LOW
   risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 121** — independent faction branches. The largest survivor pool;
   8 → 15 character arcs. Depends on Plan 125 for ponr_flags. LOW risk
   (minor ids-class edit possible).
5. **Plan 122** — military faction branches. Parallel to 121; 8 → 15.
   Depends on Plan 125 and Plan 114. LOW risk.
6. **Plan 123** — rebel faction branches. Parallel to 121–122; 8 → 15.
   Depends on Plan 125 and Plan 124. LOW risk.
7. **Plan 128** — holdfast flavor factions. The Holdfast voice pillar; 3 →
   8. Depends on Plan 117 for quest references. LOW risk.
8. **Plan 129** — foundry production. The industrial-recovery pillar; 11 →
   20. Depends on Plan 102 for treaty refs. LOW risk.

### LATER (do last — depend on earlier batches or are structurally complex)
9. **Plan 124** — faction war location overrides. The territorial-conflict
   pillar; 9 → 20. Depends on Plan 116 for location refs. LOW risk.
10. **Plan 127** — verdict data corpus + ladder. The Verdict machine
    atmosphere and discovery narrative; 8+6 → 25+12. Depends on Plan 116
    for discovery_location_id refs. LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Moral flag → echo quest → gossip → faction reaction → epilogue | MoralChoiceFlag → EchoQuest → GossipRuntime → FactionReaction → EpilogueMatrix | 125 → 109 → 110 → 100 → 89 |
| Moral flag → faction branch ponr → epilogue | MoralChoiceFlag → IndependentBranch/MilitaryBranch/RebelBranch → EpilogueMatrix | 125 → 121/122/123 → 89 |
| Crossing faction → crossing item → crossing encounter → foundry accord → epilogue | CrossingFaction → CrossingItem → CrossingEncounter → FoundryAccords → EpilogueMatrix | 120 → 126 → 115 → 102 → 89 |
| Faction war override → deep lore location → Year of Ash crisis → rebel branch ponr | FactionWarOverride → DeepLoreLocation → YearOfAshQuest → RebelBranch | 124 → 116 → 114 → 123 |
| Verdict corpus → Verdict questline → deep lore location → Verdict radio → epilogue | VerdictData → VerdictQuest → DeepLoreLocation → VerdictRadio → EpilogueMatrix | 127 → 113 → 116 → 94 → 89 |
| Holdfast flavor → Holdfast quest → crossing faction → foundry accord → epilogue | HoldfastFlavor → HoldfastQuest → CrossingFaction → FoundryAccords → EpilogueMatrix | 128 → 117 → 120 → 102 → 89 |
| Foundry product → foundry accord → deep lore location → economy tier → trade specialty | FoundryProduction → FoundryAccords → DeepLoreLocation → HardcoreEconomy → TradeSpecialty | 129 → 102 → 116 → 99 → 105 |
| Military branch ponr → Year of Ash crisis → faction war override → standing record faction → epilogue | MilitaryBranch → YearOfAshQuest → FactionWarOverride → StandingRecordFaction → EpilogueMatrix | 122 → 114 → 124 → 98 → 89 |

---

## Content totals added by this batch

* **+5** crossing factions (3 → 8)
* **+7** independent faction branches (8 → 15)
* **+7** military faction branches (8 → 15)
* **+7** rebel faction branches (8 → 15)
* **+11** faction war location overrides (9 → 20)
* **+15** moral choice flags (10 → 25)
* **+14** crossing items (11 → 25)
* **+17** Verdict corruption corpus strings (8 → 25) + **+6** world history ladder entries (6 → 12)
* **+5** Holdfast flavor factions (3 → 8)
* **+9** foundry products (11 → 20)

---

## Combined totals (batches 1–9)

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
| Batch 9 (120–130) | 11 | Faction branches, Crossing & foundry | crossing-factions/ind-branches/mil-branches/rebel-branches/war-overrides/moral-flags/crossing-items/verdict-corpus/holdfast-flavor/foundry-production + roadmap |
| **Total** | **99 plans** | | **~1300+ new content entries across 80+ catalogs** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 124, 127 (location refs)
```

All plans in this batch are pure data (LOW risk). Plans **121, 122, 123**
may require minor additions to the static `*BranchIds.cs` classes — confirm
whether these are auto-generated or hand-maintained before editing; if
hand-maintained, the edit is additive (new constants only) and does not
constitute a new system. The integration-adjacent plans are **125** (moral
choice flags, referenced by 5 downstream systems), **124** (faction war
location overrides, whose locationIds must resolve), and **127** (verdict
data, whose knowledge_keys and discovery_location_ids must resolve),
validated via `--data-integrity-selftest` and `--expedition-selftest`.
