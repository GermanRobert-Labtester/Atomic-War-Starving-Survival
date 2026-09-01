# Roadmap 108 — Batch 7: Factions, Economy, Moral Choice & Dose-Ledger (Plans 98–107)

> **Scope:** Ten focused execution plans that expand ASHFALL's faction
> ecosystems, dynamic economy, moral-choice reactions, dose-ledger quests,
> Foundry diplomacy, survivor questlines, trade professions, dose equipment,
> and radio distress signals. Every target system is fully implemented and
> wired, but its data catalog is thin (1–6 entries). This batch fills those
> catalogs.
>
> **Bias:** 100% data-authority work. Zero new Core code, zero new systems,
> zero save changes. Every plan extends an existing JSON catalog.

---

## Evidence base (verified 2026-08-30)

| Catalog | Current count | Target | Plan |
|---|---|---|---|
| `standing_record_factions.json` | 1 faction | 8 factions | 98 |
| `hardcore_economy_tuning.json` | 2 tiers, 1 faction pref, 1 shock | 8 tiers, 8 prefs, 6 shocks | 99 |
| `moral_choice_faction_reactions.json` | 1 threshold event | 6 threshold events | 100 |
| `dose_quests.json` | 4 questlines | 12 questlines | 101 |
| `foundry_accords.json` | 4 treaties | 10 treaties | 102 |
| `foundry_treaty_consequences.json` | 6 policies | 15 policies | 103 |
| `narrative_questlines.json` | 4 questlines | 12 questlines | 104 |
| `trade_specialties.json` | 4 professions | 12 professions | 105 |
| `dose_items.json` | 5 items | 15 items | 106 |
| `radio_distress_signals.json` | 5 signals | 20 signals | 107 |

All target systems confirmed live in `Assets/Ashfall.Core/` via `find`/`grep`:
`StandingRecordCatalog.cs`, `HardcoreEconomyTuning.cs`,
`MoralChoiceFactionReactionsCatalogLoader.cs`, `DoseQuestMigration.cs`,
`SilentFoundryTypes.cs`, `SilentFoundryConsequencePolicy.cs`,
`ContentUtilizationScanner.cs`, `TradeSpecialtySystem.cs`,
`ItemCatalogLoader.cs`, plus `ContentUtilizationScanner.cs` for narrative
questlines and radio distress signals.

---

## Plan index

| # | File | Theme | System fed | Content added | Priority | Risk |
|---|---|---|---|---|---|---|
| 98 | `98-standing-record-factions-expansion.md` | Faction ecosystem | `StandingRecordCatalog` | 7 factions (1 → 8) | P1 | LOW |
| 99 | `99-hardcore-economy-tuning-expansion.md` | Dynamic economy | `HardcoreEconomyTuning` | 6 tiers + 7 prefs + 5 shocks | P2 | LOW |
| 100 | `100-moral-choice-faction-reactions-expansion.md` | Moral feedback | `MoralChoiceFactionReactions` | 5 threshold events (1 → 6) | P2 | LOW |
| 101 | `101-dose-quests-expansion.md` | Dose-ledger quests | `DoseQuestMigration` | 8 questlines (4 → 12) | P2 | LOW |
| 102 | `102-foundry-accords-expansion.md` | Faction diplomacy | `SilentFoundryTypes` | 6 treaties (4 → 10) | P2 | LOW |
| 103 | `103-foundry-treaty-consequences-expansion.md` | Treaty enforcement | `SilentFoundryConsequencePolicy` | 9 policies (6 → 15) | P2 | LOW |
| 104 | `104-narrative-questlines-expansion.md` | Personal quests | Narrative questline system | 8 questlines (4 → 12) | P2 | LOW |
| 105 | `105-trade-specialties-expansion.md` | Profession progression | `TradeSpecialtySystem` | 8 professions (4 → 12) | P2 | LOW |
| 106 | `106-dose-items-expansion.md` | Dose equipment | `ItemCatalogLoader` | 10 items (5 → 15) | P3 | LOW |
| 107 | `107-radio-distress-signals-expansion.md` | Radio interception | Radio distress system | 15 signals (5 → 20) | P2 | LOW |

---

## Dependency graph

```
98 (standing record factions) ──► 44 [batch 2] (faction territory — factions control territories)
                               ──► 45 [batch 2] (faction patrols — garrison/cutters patrol)
                               ──► 43 [batch 2] (settlements — factions govern settlements)
                               ──► 99 (economy tuning — faction preferences reference factions)
                               ──► 102 (foundry accords — treaties reference factions)

99 (hardcore economy tuning) ──► 56 [batch 3] (economy goods — goods are affected by tiers)
                             ──► 98 (factions — faction preferences reference factions)
                             ──► 48 [batch 2] (weather gates — weather triggers price shocks)
                             ──► 77 [batch 5] (duty roster seasons — seasons align with tiers)

100 (moral choice reactions) ──► 95 [batch 6] (journal voice — moral events trigger journal entries)
                             ──► 66 [batch 4] (guilt — moral events generate guilt)
                             ──► 88 [batch 6] (confessions — moral events may trigger confessions)
                             ──► 89 [batch 6] (epilogues — moral events determine endings)

101 (dose quests) ──► 90 [batch 6] (dose registers — quests reference bands and care plans)
                  ──► 95 [batch 6] (journal voice — quest outcomes trigger journal entries)
                  ──► 66 [batch 4] (guilt — dose quest choices generate guilt)
                  ──► 106 (dose items — quests grant dose items)

102 (foundry accords) ──► 103 (treaty consequences — treaties have consequence policies)
                      ──► 98 (factions — treaties reference factions)
                      ──► 92 [batch 6] (faction dialogue — treaties are discussed in dialogue)
                      ──► 89 [batch 6] (epilogues — treaty outcomes affect endings)

103 (foundry treaty consequences) ──► 102 (foundry accords — treaties define the pacts)
                                ──► 98 (factions — consequences affect faction standing)
                                ──► 89 [batch 6] (epilogues — treaty outcomes affect endings)

104 (narrative questlines) ──► 52 [batch 2] (NPC arcs — questline survivors recur)
                           ──► 95 [batch 6] (journal voice — quest events trigger entries)
                           ──► 88 [batch 6] (confessions — questline survivors may confess)
                           ──► 65 [batch 4] (final wishes — questline survivors may have wishes)

105 (trade specialties) ──► 33 [batch 1] (skills — trade specialties grant skill bonuses)
                       ──► 80 [batch 5] (library manuals — manuals grant profession XP)
                       ──► 56 [batch 3] (economy goods — profession patterns match goods)
                       ──► 72 [batch 4] (utility AI — profession determines actions)

106 (dose items) ──► 101 (dose quests — quests grant dose items)
                ──► 90 [batch 6] (dose registers — care plans consume dose items)
                ──► 81 [batch 5] (dose locations — dose items are used at locations)
                ──► 55 [batch 3] (recipes — some dose items are craftable)

107 (radio distress signals) ──► 76 [batch 5] (expedition destinations — revealed locations)
                           ──► 50 [batch 2] (radio distress expansion — this IS that expansion)
                           ──► 82 [batch 5] (Verdict locations — signals reference sites)
                           ──► 73 [batch 4] (faction radio — distress signals complement)
                           ──► 84 [batch 5] (muster witnesses — signals corroborate testimony)
```

---

## Execution sequence

### NOW (do first — highest player value, lowest risk)
1. **Plan 98** — standing record factions. The single thinnest faction catalog
   (1 faction). Unblocks faction territory, patrols, and diplomacy. Pure
   data, LOW risk. Feeds Plans 99, 102.
2. **Plan 107** — radio distress signals. The radio-interception pillar; 5 →
   20 signals with diverse outcomes. Pure data, LOW risk. Feeds Plans 76, 82.
3. **Plan 104** — narrative questlines. The personal-quest pillar; 4 → 12
   survivor-specific arcs with moral branching. Pure data, LOW risk.

### NEXT (do after NOW — cross-system + moderate integration)
4. **Plan 99** — hardcore economy tuning. The dynamic-economy pillar; 2 → 8
   tiers, 1 → 8 faction prefs, 1 → 6 price shocks. Depends on Plan 98 for
   faction references. LOW risk.
5. **Plan 101** — dose quests. The dose-ledger quest pillar; 4 → 12 questlines.
   Depends on Plan 90/106 for dose register/item references. LOW risk.
6. **Plan 102** — foundry accords. The diplomatic-treaty pillar; 4 → 10
   treaties. Depends on Plan 98 for faction references. LOW risk.
7. **Plan 103** — foundry treaty consequences. The treaty-enforcement pillar;
   6 → 15 policies. Depends on Plan 102 for treaty references. LOW risk.

### LATER (do last — depend on earlier batches or are self-contained)
8. **Plan 100** — moral choice faction reactions. The moral-feedback pillar;
   1 → 6 threshold events. Depends on the moral choice system's band
   boundaries. LOW risk.
9. **Plan 105** — trade specialties. The profession-progression pillar; 4 → 12
   professions. Depends on Plan 33/80 for skill/manual references. LOW risk.
10. **Plan 106** — dose items. The dose-equipment pillar; 5 → 15 items.
    Depends on Plan 101/90 for quest/register references. LOW risk.

---

## Cross-system chains activated by this batch

| Chain | Systems spanned | Plans |
|---|---|---|
| Faction → territory → patrol → treaty → consequence → ending | FactionCatalog → FactionTerritory → FactionPatrol → FoundryAccords → ConsequencePolicy → EpilogueMatrix | 98 → 44 → 45 → 102 → 103 → 89 |
| Economy tier → faction preference → price shock → weather gate → season | HardcoreEconomyTuning → FactionCatalog → WeatherSystem → DutyRoster | 99 → 98 → 48 → 77 |
| Moral event → faction reaction → journal voice → guilt → confession → ending | MoralChoiceReactions → JournalVoice → GuiltSystem → ConfessionSystem → EpilogueMatrix | 100 → 95 → 66 → 88 → 89 |
| Dose quest → dose band → care plan → dose item → autopsy → grave | DoseQuest → DoseRegister → DoseItem → AutopsySystem → MemorialSystem | 101 → 90 → 106 → 79 → 69 |
| Radio signal → revealed location → expedition → Verdict site → witness → ending | RadioDistressSignal → ExpeditionSystem → VerdictLocation → WitnessCatalog → EpilogueMatrix | 107 → 76 → 82 → 84 → 89 |
| Narrative questline → survivor → NPC arc → confession → final wish → journal | NarrativeQuestline → CharacterSystem → ConfessionSystem → FinalWishSystem → JournalVoice | 104 → 52 → 88 → 65 → 95 |
| Trade specialty → skill → library manual → economy good → utility AI | TradeSpecialty → SkillProgression → LibraryStudy → EconomySystem → UtilityAI | 105 → 33 → 80 → 56 → 72 |
| Foundry accord → treaty consequence → faction standing → faction dialogue → epilogue | FoundryAccords → ConsequencePolicy → FactionStanding → FactionWarDialogue → EpilogueMatrix | 102 → 103 → 98 → 92 → 89 |

---

## Content totals added by this batch

* **+7** standing record factions (1 → 8)
* **+6** scarcity tiers (2 → 8) + **+7** faction preferences (1 → 8) + **+5** price shocks (1 → 6)
* **+5** moral choice threshold events (1 → 6)
* **+8** dose quest questlines (4 → 12)
* **+6** foundry accord treaties (4 → 10)
* **+9** foundry treaty consequence policies (6 → 15)
* **+8** narrative questlines (4 → 12)
* **+8** trade specialty professions (4 → 12)
* **+10** dose items (5 → 15)
* **+15** radio distress signals (5 → 20)

---

## Combined totals (batches 1–7)

| Batch | Plans | Theme | Key content added |
|---|---|---|---|
| Batch 1 (32–42) | 11 | Scaffolding systems | skill/research/wildlife/excavation/sky-armor/orbital/debt/room catalogs + roadmap |
| Batch 2 (43–53) | 11 | World content | settlements/territory/patrols/scavenging/collectibles/weather/micro-locations/radio/documents/NPCs + roadmap |
| Batch 3 (54–64) | 11 | Thin catalogs | combat/crafting/economy/incidents/encounters/questlines/vehicles/trade/doctrines + roadmap |
| Batch 4 (65–75) | 11 | Narrative depth | wishes/guilt/cassettes/carvings/epitaphs/schedules/power/AI/radio/chapters + roadmap |
| Batch 5 (76–86) | 11 | Exploration & investigation | expeditions/seasons/inks/autopsies/manuals/dose/verdict/weather/witnesses/maps + roadmap |
| Batch 6 (87–97) | 11 | Relics, confessions & endings | relics/confessions/epilogues/dose-bands/greenhouse/dialogue/verdict-npcs/verdict-radio/journal-voice/epilogue-slides + roadmap |
| Batch 7 (98–108) | 11 | Factions, economy & dose-ledger | sr-factions/economy-tuning/moral-reactions/dose-quests/foundry-accords/treaty-consequences/narrative-questlines/trade-specialties/dose-items/radio-signals + roadmap |
| **Total** | **77 plans** | | **~800+ new content entries across 60+ catalogs** |

---

## Verification (run after each plan, then after the full batch)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest   # for plans 107 (revealed locations)
```

All plans in this batch are pure data (LOW risk). No cross-tool QA required unless a plan
also touches a Core schema field (none in this batch — all extend existing JSON catalogs
within their current schema). The integration-adjacent plans are **98** (factions,
referenced by territory/patrol systems), **99** (economy tuning, referenced by the
economy system), and **107** (radio signals, which reveal expedition-reachable
locations), all validated via `--data-integrity-selftest` and `--expedition-selftest`.
