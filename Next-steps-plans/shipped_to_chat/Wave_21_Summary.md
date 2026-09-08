# Wave 21 — Summary (Plans 166–170)

## Wave Overview

Five non-duplicative, implementation-ready plans covering community identity, underground exploration, information warfare, atmospheric immersion, and temporal rhythm. Each plan addresses a verified gap — areas with zero existing systems or only superficial coverage.

**Post-recon corrections:** Plans 167 and 168 were initially written as Safe Cracking and Companion Animals, but deep recon revealed `SafeCrackingSystem.cs` (532 lines) already exists in `Assets/Ashfall.Core/Maritime/` and Plan 151 already covers working animals & companions. Both were replaced with genuinely non-overlapping alternatives.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 166 — Shelter Identity & Naming | Player names shelter, chooses origin story, builds community reputation. Shelter becomes a named entity in the wasteland. | Plan 156 (shelter expansion) adds rooms but not identity. Plan 159 (governance) adds politics but not name. Plan 162 (archive) records history but doesn't name the community. Verified: ZERO matches for `ShelterName`, `shelter_identity` anywhere in codebase. | LOW | OnboardingJourney, JournalSystem, FactionBranchCoordinator, ShelterArchiveSystem, GovernanceSystem |
| 167 — Underground Tunnel Network | Discover, explore, map, and maintain subterranean passages connecting bunkers, resources, and hidden locations. | Plan 133 (expedition consequences) covers surface discovery. Plan 153 (espionage) covers infiltration. Plan 155 (black market) covers underground economy but not physical tunnels. Plan 163 (cartography) covers surface mapping. Verified: only `SaltMineExtractionSystem.cs` (resource extraction) and narrative flavor text — no tunnel network gameplay system. | MEDIUM | ExpeditionSystem, LocationEvolution, SkillProgression, SaltMineExtraction, WeatherSystem, ShelterThermal |
| 168 — Propaganda & Morale Warfare | Create/distribute propaganda, broadcast messages, conduct psychological operations, influence faction morale. | Plan 131 (rumor network) covers inter-survivor rumors but not faction-level propaganda. Plan 153 (espionage) covers covert ops but not psyops. Plan 157 (communications) covers infrastructure but not content creation. Verified: only `StencilPropagandaSmearEntry` data type (narrative) — no player-driven propaganda system. | MEDIUM | FactionBranchCoordinator, FactionStanceEngine, PaperPrintingCatalog, VerdictRadioSystem, MoralChoice |
| 169 — Adaptive Audio & Dynamic Music | Soundtrack responds to game state — mood, danger, time, season — with crossfading music layers and ambient soundscapes. | No plan addresses adaptive audio. Existing `AudioManager.cs`/`AudioCueCatalog.cs` are cue-based (specific triggers → specific sounds). Verified: ZERO matches for `adaptive`, `dynamic_music`, `MusicDirector`, `music_layer` in src/. Dual-player crossfade infrastructure exists but is not driven by game state. | MEDIUM | NeedsSystem, WeatherSystem, ExpeditionSystem, TacticalCombatSystem, CampaignCalendar, NuclearWinterSystem |
| 170 — Seasonal Events & Celebrations | Holidays, anniversaries, festivals, and traditions mark time passing. Celebrations boost morale, build community, create traditions. | No plan addresses seasonal events. Verified: only 2 feedback hint strings ("Small celebrations boost morale", "Survivors need a celebration") in `FeedbackMessageCatalogLoader.cs` — hints for a system that was never built. ZERO matches for `SeasonalEvent`, `HolidaySystem`, `FestivalSystem`. | LOW | NeedsSystem, CampaignCalendar, MemorialSystem, ShelterIdentitySystem, ShelterArchiveSystem, SurvivorRelationsSystem |

## Strongest Plan to Implement First

**Plan 166 — Shelter Identity & Naming.** It has the lowest risk, smallest scope, and immediate player value (the shelter gets a name and identity). It integrates naturally with onboarding, journal, factions, and archive systems. It transforms the shelter from a generic container into a named community — the foundation for everything else (archive records the named shelter's history, traditions belong to the named shelter, reputation is attached to the named shelter).

## Dependencies Between the 5 Plans

- **Plan 166 (Identity) is foundational** — shelter name referenced by archive (162), traditions (170), reputation affects factions.
- **Plan 167 (Tunnel Network) is standalone** — connects to expeditions, locations, mining, but not other plans in this wave.
- **Plan 168 (Propaganda) is standalone** — connects to faction systems, radio, moral choice, but not other plans in this wave.
- **Plan 169 (Adaptive Audio) is standalone** — reads game state from many systems but doesn't depend on other plans in this wave. Can integrate with Plan 170 (celebration music).
- **Plan 170 (Seasonal Events) integrates with 166** — traditions become part of shelter identity. Also integrates with 162 (archive records memorable celebrations) and 169 (celebration music).

## Recommended Implementation Order

1. **Plan 166** — Shelter Identity & Naming (community identity, lowest risk, foundational)
2. **Plan 170** — Seasonal Events & Celebrations (temporal rhythm, low risk, integrates with identity)
3. **Plan 167** — Underground Tunnel Network (exploration depth, medium risk, standalone)
4. **Plan 168** — Propaganda & Morale Warfare (information warfare, medium risk, standalone)
5. **Plan 169** — Adaptive Audio & Dynamic Music (atmospheric immersion, medium risk, standalone)

## Post-Recon Corrections

Two plans were rewritten after deep recon revealed existing systems:

| Original Plan | Replacement | Reason |
| --- | --- | --- |
| 167 — Safe Cracking & Lockpicking | 167 — Underground Tunnel Network | `SafeCrackingSystem.cs` (532 lines) already exists in `Assets/Ashfall.Core/Maritime/` with full safe definitions, tumblers, difficulty, noise/alarm, loot tables, `CaptureState`/`RestoreState`. `SafeCrackModal.cs` (213 lines) is functional UI. The empty `SafeCrackModalContent.cs` was just a Godot scene-script binding anchor. |
| 168 — Companion Animal System | 168 — Propaganda & Morale Warfare | Plan 151 (Working Animals & Companion System, Wave 18) already covers animal companions (dog/cat/horse/bird), bonding, training, tasks, morale, defense, expedition support. Plan 168 would have been significant overlap. |

## Rejected Candidates (Considered but Not Selected)

- **Tutorial/Onboarding** — Already fully implemented: `OnboardingJourney.cs` (452 lines), `OnboardingSaveState.cs` (103 lines), `TutorialPanel.cs` (204 lines), `OnboardingHintPanel.cs` (357 lines), `Main.Onboarding.cs` (176 lines), `OnboardingSaveStore.cs`, `--onboarding-journey-selftest` verb. Not a gap.
- **Chemical Dependency** — Already fully implemented: `ChemicalDependencySystem.cs` (533 lines) with 4 substance kinds, dependency formation, managed detox, cold-turkey withdrawal, stress-driven relapse, crafting/combat penalties, affliction pipeline integration. Not a gap.
- **Water Treatment** — Already fully implemented: `WaterTreatmentSystem.cs` (634 lines) with 4 water types, 5 treatment modes, contamination tracking, filter degradation, disease pipeline integration. Not a gap.
- **Barter/Trade Depth** — `MarketSystem.cs` (404 lines) has `Barter()` method, `HoldfastTradeSession.cs` (682 lines) has trade sessions, `LedgerDebtSystem.cs` has debt renegotiation. Minor gap (no haggle mechanic) but too thin for a full plan.
- **Safe Cracking** — Already exists as noted above.
- **Companion Animals** — Plan 151 already covers this.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with functional systems into one with personality and depth: a shelter that has a name and reputation (not just a container), an underground world waiting to be explored (not just surface destinations), an information warfare layer where words are weapons (not just combat and trade), audio that breathes with the game's emotional state (not just sound effects on triggers), and time that is marked by celebrations and traditions (not just an incrementing day counter). This is the wave that gives ASHFALL its soul — the small, human details and the hidden depths that make players care about their shelter, their survivors, and their story.

## Cumulative Wave Themes (Waves 14–21)

| Wave | Theme | Plans |
| ---- | ----- | ----- |
| 14 | Information flow & hidden knowledge | 131–135 |
| 15 | Dead-end fixes & cross-system bridges | 136–140 |
| 16 | Research, clothing, medical, autonomy, endings | 141–145 |
| 17 | Radiation, memory, friction, achievements, romance | 146–150 |
| 18 | Animals, vehicles, espionage, education, black market | 151–155 |
| 19 | Shelter, communications, disasters, governance, colonies | 156–160 |
| 20 | Hobbies, archive, cartography, nuclear winter, modding | 161–165 |
| **21** | **Identity, tunnels, propaganda, audio, celebrations** | **166–170** |

**Total: 40 plans across 8 waves (131–170), plus 8 wave summaries.**
