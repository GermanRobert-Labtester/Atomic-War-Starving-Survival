# ASHFALL — Expansion Wave 7 Index (Expansions 42–46)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-24
**Purpose:** Index and evidence summary for the five Wave 7 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: reach

Waves 1–6 built the shelter inward: survival, culture, hazards, infrastructure,
crafts, food, the frontier, and care at every scale. Wave 7 measures how far the
community's reach extends. It powers itself from a core it barely understands,
earns knowledge through method, founds a place it must keep alive at a distance,
meets other communities as equals at a table, and learns to read the slow change
of the world it lives in.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 42 · The Core | `NuclearCoreLifecycleSystem` (`InstalledCores`, `SpentCoreStorage`, `OnReactorScrammed`, `OnHeatStateChanged`, `OnRadiationLeak`, `TryInstallCore`, `SetOutputSetting`, `TryRepairShielding`, `TryEmergencyScram`, `TickDay`), `ExtCoreCatalog`, shielding owners | `nuclear_core_profiles.json` = **3,314 B** (three profiles) | procedures, shifts, cooling loops, surveys, criticality, spent fuel, dosimetry, drills |
| 43 · The Question | `ResearchSystem` (62 nodes, eligibility, unlocks, blueprint progress), `PrewarArchiveDecryptionSystem`, `LibraryStudySystem` | `prewar_archives.json` = **4,072 B** | questions, hypotheses, controls, repeats, field studies, review, holdings, method |
| 44 · The Outpost | `ColonySystem` (`ColonyOutpost`, `SupplyLine`, `EstablishColony`, `EstablishSupplyLine`, `TransferSupplies`, `TickDay`), expedition systems | `anomalous_expedition_encounters.json` = **2,267 B** | charters, sites, founding, depots, remote life, defense, radio, rotation, recall |
| 45 · The Envoy | `DiplomaticSummitSystem` (standing/context/skills ports), treaty catalogs, `ShelterReputationSystem` (`ReputationEvidence`), `PropagandaSystem`, `TradeEmbargoSystem` | `diplomatic_treaties.json` = **6,846 B**, `regional_treaties.json` = 3,197 B | envoys, credentials, agendas, terms, protocol, gifts, evidence, messages, access |
| 46 · The Long Change | `LandmarkDegradationSystem` (integrity, burial, collapse, salvaging), `LocationEvolutionSystem` (ownership, contamination, ruin), `EvolvingWorldCatalog` | seed data present (20,728 B) but no player-facing content | observation, registers, thresholds, decisions, reclamation, chronicle |

The strongest authority splits in the wave: the reactor's state machine stays
with its engine and the dose ledger is read-only; knowledge unlocks stay with
the research system while method lives in new content; colony and supply state
stay with `ColonySystem` and only its own writers change them; the envoy never
sets a price and never invents intelligence; and the world changes only inside
its live evolution and landmark systems, with observation as pure record.

---

## The five plans

1. `expansion_42_the_core_plan.md` — 70,403 chars.
   Procedures, shifts, output plans, cooling loops, shielding surveys,
   criticality discipline, spent fuel, dosimetry, drills, and reactor review.
   Extends the core lifecycle and shielding owners.
2. `expansion_43_the_question_plan.md` — 70,611 chars.
   Questions, hypotheses, controls, repeats, field seasons, archives, review
   circles, refutations, holdings, prototypes, and method teaching. Extends the
   research, archive, and study owners.
3. `expansion_44_the_outpost_plan.md` — 70,080 chars.
   Charters, site surveys, founding parties, supply lines, depots, remote life,
   defense, radio windows, mail, rotation, and recall. Extends the colony and
   expedition owners.
4. `expansion_45_the_envoy_plan.md` — 70,422 chars.
   Envoys, credentials, summits, treaty terms, reputation evidence, protocol
   gifts, public messages, and access agreements. Extends the summit, treaty,
   reputation, message, and embargo owners.
5. `expansion_46_the_long_change_plan.md` — 70,034 chars.
   Observation posts, survey rounds, landmark registers, threshold alerts,
   location state decisions, scarcity watches, reclamation, and the world
   chronicle. Extends the evolution and landmark owners.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second core, research, colony, diplomacy,
  world-evolution, or save system is introduced.
- **Deterministic.** Live seeded paths only. Core physics, research days,
  colony ticks, reputation deltas, and world change all resolve through their
  existing owners; new systems record and schedule. Paired replay hashes must
  match.
- **Persistence.** New state is additive inside existing owners
  (`NuclearCoreLifecycleSave`, `ResearchState`, `PrewarArchiveDecryptionState`,
  `ColonyState`, summit/treaty/reputation/message state,
  `LocationEvolutionSaveState`, `LandmarkSaveState`). Legacy saves load neutral;
  the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real reactors, agencies,
  institutions, treaties, or landmarks copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 7

| Plan | Hard contract |
|---|---|
| 42 Core | No weapons, enrichment, or bomb content; no meltdown spectacle or radiation horror; the dose ledger is read-only and authoritative; spent fuel is never dumped; operators are professionals and fatigue is managed |
| 43 Question | No human studies outside consent and the ward; no miracle cures or prophecy; every claim shows method and repeat count; refutations are recorded with dignity, never erased; uncertainty is always displayed |
| 44 Outpost | No colonial framing, conquest, forced relocation, or punishment postings; remote life uses the live needs owners; every outpost has a written recall threshold; closure brings people and records home together |
| 45 Envoy | No bribery or manipulation rewarded; public messages that lie always damage internal trust and are never a winning strategy; no price setting; factions are interest-based, never identity-based; guests are housed decently and never used as leverage |
| 46 Long Change | No real landmarks or heritage sites copied; no doom clock; salvage has records and costs, never looting glee; human sites route through the memory owners; every loss is named and every recovery is counted |

---

## Cross-wave hooks (summary)

- **Wave 7 × Wave 7:** the core powers the outpost and the reclamation works;
  the question studies the world's change; the outpost reports change; the envoy
  negotiates access to places that are changing.
- **Wave 7 × earlier waves:** the grid takes the core's generation; the ward and
  dose ledger receive every radiological fact; the press prints the chronicle,
  the notes, and the treaties; the road and watch carry envoys and convoys; the
  wild and weather supply observation data; the kiln, thread, glass, and wheel
  supply restoration materials; the quiet and the long goodbye receive the
  memory of lost places.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 7 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an
   API or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and
   focused verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then
   content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe
pre-signature work is Phase 1 (schemas and validators), which is additive and
reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 300,000–330,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. Each
  plan recommends additive sub-objects.
- Whether any Wave 7 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending existing verbs.
- Priority order. Recommended: 42 (Core) first, because it is the most
  safety-critical and the most isolated; 43 (Question) second, because its
  method content improves every other plan's quality; 44 (Outpost) third,
  because it extends reach and depends on the road and radio; 45 (Envoy)
  fourth, because it builds on outpost contact; 46 (Long Change) fifth, because
  it is the slowest and longest-lived.

---

## Evidence anchors (file references used across the plans)

- `Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs` —
  `ReactorCoreState` (`outputSetting`, `coolantState`, `heatState`,
  `shieldingIntegrity`, `embrittlementWear`, `isScrammed`, `roomId`),
  `NuclearCoreLifecycleSave` (`installedCores`, `spentCoreStorage`),
  `PowerSourceId = "nuclear_core"`, `TryInstallCore`, `SetOutputSetting`,
  `GetTotalGenerationWatts`, `TryRepairShielding`, `TryEmergencyScram`,
  `TickDay`.
- `Assets/Ashfall.Core/Research/` — `ResearchSystem`, `ResearchKnowledgeDef`,
  `ResearchState` (`unlockedIds`, `blueprintProgress`,
  `researchPointsAvailable`), `ResearchEligibility`,
  `PrewarArchiveDecryptionSystem` (`PrewarArchiveProject`).
- `Assets/Ashfall.Core/Expeditions/ColonySystem.cs` — `ColonyOutpost`,
  `SupplyLine`, `ColonyState`, establish/transfer/tick/capture/restore.
- `Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs` —
  `DiplomaticSummitState`, `IFactionStandingPort`, `IFactionContextPort`,
  `ISurvivorSkillsPort`.
- `Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs` —
  `ReputationEvidence` (`Dimension`, `Delta`, `Confidence`, `Salience`,
  `Medium`).
- `Assets/Ashfall.Core/LandmarkDegradationSystem.cs` —
  `LandmarkStatusRecord` (`structuralIntegrity`, `ashBurialCm`, `isCollapsed`,
  `isScavenged`, `collapseDay`).
- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — `LocationMutationRecord`
  (`currentOwner`, `contaminationLevel`, `lootDepletionFactor`, `isCleared`,
  `isRuined`).
- `Assets/Ashfall.Core/EvolvingWorldCatalog.cs` — `EvolvingWorldSeedContainer`
  (sectors, packs, landmarks, location seeds, scarcity goods).
- Data: `nuclear_core_profiles.json` (3,314 B), `research_knowledge.json`
  (21,516 B / 62 nodes), `prewar_archives.json` (4,072 B),
  `world_evolution_seeds.json` (20,728 B), `damaged_map_zones.json`
  (15,568 B), `settlements.json` (24,974 B),
  `wasteland_settlement_npcs.json` (30,940 B), `diplomatic_treaties.json`
  (6,846 B), `regional_treaties.json` (3,197 B), `propaganda_campaigns.json`
  (4,998 B).
- `docs/expansions/wave1/WAVE1_INDEX.md` through
  `docs/expansions/wave6/WAVE6_INDEX.md`.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`,
  `INTEGRATION_PLANS.md`.