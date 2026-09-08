# Wave 28 — Summary (Plans 201–205)

## Wave Overview

Five non-duplicative, implementation-ready plans covering environmental hygiene, social dynamics, information warfare, population growth, and stealth acoustics. This wave focuses on **shelter as living organism** — the systems that make the shelter feel like a real place with waste, noise, conflicts, visitors, and external threats that notice it exists.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 201 — Shelter Sanitation & Waste Management | Shelter accumulates waste (sewage, trash, hazardous), requires sanitation infrastructure (latrines, waste processors, water recyclers), suffers disease/morale consequences when sanitation fails. | `VentilationSystem` (270 lines) handles air quality. `SumpFloodingSystem` (298 lines) handles water. But NO sanitation, waste, sewage, or hygiene tracking. Plan 158 mentions "poor sanitation" as disaster trigger but doesn't implement. Verified: ZERO matches for `SanitationSystem`, `WasteManagement`, `HygieneSystem`, `SewageSystem` in Core. | LOW | VentilationSystem, SumpFloodingSystem, DiseaseSystem, NeedsSystem, KitchenNutritionSystem, GreenhouseSystem, PowerGridSystem |
| 202 — Survivor Interpersonal Conflict & Grievances | Survivors argue, hold grudges, clash over resources/fairness/personality — with escalation, mediation, and resolution mechanics. | `IdeologicalFrictionSystem` (158 lines) handles belief-based friction only. Plan 148 adds ideological events. But NO general interpersonal conflict — no arguments over resources, no grudges from unfair treatment, no personality clashes, no disputes, no mediation. Verified: ZERO matches for `InterpersonalConflict`, `GrievanceSystem`, `DisputeSystem` in Core. Distinct from Plan 148 (ideology) — this covers non-ideological social friction. | LOW | IdeologicalFrictionSystem, SurvivorRelationsSystem, NeedsSystem, DutyRosterSystem, TraitSystem, MoralChoiceSystem |
| 203 — Intelligence & Rumor Network | Build informant networks, gather intelligence on factions, spread/counter rumors, run intelligence operations — information as strategic resource. | `FactionStanceEngine` (172 lines) tracks trust. `SignalTriangulationSystem` handles radio. Plan 131/153/168 mention informants/espionage/propaganda in passing. But NO dedicated intelligence system — no informant network, no rumor mechanics, no intelligence operations, no counter-intelligence. Verified: ZERO matches for `IntelligenceSystem`, `RumorSystem`, `InformantSystem` in Core. | LOW | FactionStanceEngine, SignalTriangulationSystem, ExpeditionSystem, HoldfastTradeSession, CombatSystem, FactionBranchCoordinator |
| 204 — Survivor Recruitment & Defection | Actively recruit survivors from factions/wilderness, induce faction defections, offer asylum, trade for survivors — population growth as strategic gameplay. | Survivors arrive through narrative events. Plan 174 generates backstories. Plan 168/153 mention defection as quest hooks. But NO active recruitment system — no recruitment missions, no defection mechanics, no candidate discovery, no structured survivor acquisition. Verified: ZERO matches for `RecruitmentSystem`, `DefectorSystem`, `RecruitSurvivor` in Core. | LOW | FactionStanceEngine, FactionBranchCoordinator, ExpeditionSystem, EventSystem, SurvivorRelationsSystem, SkillProgressionSystem |
| 205 — Shelter Noise Discipline & Acoustic Management | Shelter generates noise from activities/machinery, noise propagates through rooms, excessive noise attracts external threats, soundproofing and quiet hours reduce detection. | `MaritimeDiveSystem`/`SafeCrackingSystem` track domain-specific noise. Plan 138 mentions "noise discipline" in passing. But NO general shelter noise system — no acoustic propagation, no soundproofing, no noise discipline, no sound-based detection. Verified: ZERO matches for `ShelterNoise`, `NoiseDiscipline`, `SoundPropagation`, `AcousticSystem` in Core. | LOW | PowerGridSystem, VentilationSystem, ShelterThermalSystem, ExpeditionSystem, CombatSystem, InterpersonalConflictSystem |

## Strongest Plan to Implement First

**Plan 201 — Shelter Sanitation & Waste Management.** It completes the shelter's environmental systems (air + water + waste), has the clearest survival realism payoff, and integrates naturally with existing disease/kitchen/greenhouse systems. Sanitation is a foundational survival need that the shelter currently ignores — implementing it makes the shelter feel like a real enclosed environment where waste must be managed.

## Dependencies Between the 5 Plans

- **Plan 201 (Sanitation) is standalone** — extends shelter environment with waste layer.
- **Plan 202 (Interpersonal Conflict) is standalone** — adds social friction layer to survivors.
- **Plan 203 (Intelligence) is standalone** — adds information warfare layer.
- **Plan 204 (Recruitment) integrates with Plan 203** — intelligence network discovers recruitment candidates.
- **Plan 205 (Noise) integrates with Plan 202** — arguments generate noise; noise discipline affected by conflicts.

## Recommended Implementation Order

1. **Plan 201** — Shelter Sanitation & Waste Management (environmental completion, lowest risk, broadest integration)
2. **Plan 205** — Shelter Noise Discipline & Acoustics (stealth layer, low risk, extends shelter environment)
3. **Plan 202** — Survivor Interpersonal Conflict & Grievances (social depth, low risk, extends survivor relations)
4. **Plan 203** — Intelligence & Rumor Network (information warfare, low risk, extends faction interaction)
5. **Plan 204** — Survivor Recruitment & Defection (population growth, low risk, uses intelligence network)

## Rejected Candidates (Considered but Not Selected)

- **Power/Energy/Fuel Logistics** — `PowerGridSystem.cs` already exists (full implementation: rooms, priorities, fuel, tick, save/load, 480+ lines). Not a gap.
- **Air Quality/Ventilation Management** — `VentilationSystem.cs` already exists (270 lines: smoke/soot, CO, filter saturation, duct/valve state, room exposure). Not a gap.
- **Raid/Assault Defense System** — Plan 138 (Shelter Defense & Visitor/Refugee System) covers shelter attacks, defense, visitors. Not a planning gap.
- **Justice/Judicial System** — Plan 159 (Shelter Governance & Political System) includes justice system, courts, trials, punishments. Not a planning gap.
- **Agriculture/Seed Vault System** — `GreenhouseSystem.cs` exists (full farming system). `SeedBankPreservationCatalog.cs` exists (narrative content). Not a gap.
- **Shelter Noise/Acoustics as "Sound Detection"** — considered merging with Plan 138 (shelter defense) but noise discipline is broader (room propagation, soundproofing, quiet hours) and deserves its own plan.

## Post-Recon Corrections

Both recon agents validated all 5 plans as genuine gaps:
- **Plan 201 (Sanitation)**: confirmed ZERO systems. Waste management is entirely absent.
- **Plan 202 (Interpersonal Conflict)**: confirmed distinct from Plan 148 (ideological friction). Plan 148 = belief-based conflicts. Plan 202 = resource/fairness/personality conflicts. Complementary, not duplicative.
- **Plan 203 (Intelligence)**: confirmed ZERO systems. Mentions in Plans 131/153/168 are quest hooks/passing references, not dedicated systems.
- **Plan 204 (Recruitment)**: confirmed ZERO systems. Mentions in Plans 168/153 are quest hooks, not recruitment mechanics.
- **Plan 205 (Noise)**: confirmed ZERO general shelter noise systems. Domain-specific noise (diving, safe cracking, radio) exists but no shelter-wide acoustic management.

## Why This Wave Materially Expands ASHFALL

These five plans transform the shelter from a functional workspace into a living organism that produces waste, generates noise, hosts conflicts, attracts newcomers, and is noticed by the outside world. Sanitation makes the shelter's environmental systems complete (air + water + food + waste). Noise discipline adds a stealth layer where the shelter's acoustic footprint matters. Interpersonal conflicts make survivor social dynamics richer than just affinity numbers. Intelligence gathering turns information into a strategic resource. Recruitment makes population growth a player choice rather than random events. Together, these plans make the shelter feel like a real place in a real world — with all the mess, noise, drama, visitors, and external attention that implies.

## Cumulative Wave Themes (Waves 15–28)

| Wave | Theme | Plans |
| ---- | ----- | ----- |
| 15 | Dead-end fixes & cross-system bridges | 136–140 |
| 16 | Research, clothing, medical, autonomy, endings | 141–145 |
| 17 | Radiation, memory, friction, achievements, romance | 146–150 |
| 18 | Animals, vehicles, espionage, education, black market | 151–155 |
| 19 | Shelter, communications, disasters, governance, colonies | 156–160 |
| 20 | Hobbies, archive, cartography, nuclear winter, modding | 161–165 |
| 21 | Identity, tunnels, propaganda, audio, celebrations | 166–170 |
| 22 | Dynamic quests, mutations, radio, backstories, meta-progression | 171–175 |
| 23 | Aging, dreams, art, psychology, certifications | 176–180 |
| 24 | Difficulty, relationship decay, child development, accessibility, memory decay | 181–185 |
| 25 | Shelter maintenance, bestiary, survivor routines, water sources, item lore | 186–190 |
| 26 | Item identification, trade routes, chronic conditions, emergency alerts, survivor roles | 191–195 |
| 27 | Food types, diplomacy, health records, seasonal migration, personal quests | 196–200 |
| **28** | **Sanitation, interpersonal conflict, intelligence, recruitment, noise discipline** | **201–205** |

**Total: 75 plans across 15 waves (131–205), plus 15 wave summaries.**

## Milestone Note

Wave 28 begins the third century of plans (201–205). The planning journey has expanded from basic systems (Plans 1–50) through content/narrative (51–100), through integration/depth (101–150), through societal complexity (151–200), and now into the shelter as living organism (201+). Each wave builds on the last, creating an increasingly detailed vision of what ASHFALL can become.
