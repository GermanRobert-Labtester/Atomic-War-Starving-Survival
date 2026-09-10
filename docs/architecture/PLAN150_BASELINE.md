# PLAN 150 BASELINE: Personal Letters & Unsent Correspondence Discovery and Survivor Memory Runtime

## 1. Mission & Scope
Plan 150 activates `Assets/StreamingAssets/Data/narrative/letters_expansion.json` as a player-discoverable correspondence and survivor memory layer.

The core architectural mission is to enable discovery of private and delivered correspondence across shelter rooms, lockers, desks, and expeditions to deepen survivor identity, grief, secrets, absence, and shelter history—**without** treating authored letters as omniscient truth or letting prose directly mutate relationships, deaths, guilt, quests, locations, or inventory.

---

## 2. Problem Statement & Historical Gaps
Prior to Plan 150:
1. **Unactivated Corpus**: `narrative/letters_expansion.json` contained 25 authored letters (`letter_01` to `letter_25`) spanning days 3 to 100, but had no dedicated Core catalog, projection service, or integration into `NarrativeDiscoveryCatalog`.
2. **Risk of Prose-Driven Mutation**: Letters contain dramatic narrative claims (e.g. Pavel leaving for the intake, stolen flour sacks for the night watch, triage decisions in the clinic, conscripts dying at the forward post, generator knocks). Without strict gates, a document reader could accidentally trigger unwarranted deaths, inventory drops, or moral score changes.
3. **Missing Canonical Spatial Projections**: Authored locations were descriptive strings (e.g. `"the cold frame, in the dirt"`, `"the generator room, on the logbook"`, `"the school, in the book"`) needing explicit mapping to canonical shelter rooms (`room_*`).
4. **Distinction between Delivered and Unsent**: 9 letters are delivered correspondence (left on trays, desks, logbooks); 16 are unsent/private artifacts (written to the dead, written in dirt, kept in bedside tins). Privacy and discovery semantics must preserve this distinction.

---

## 3. Core Architectural Invariants
1. **Invariant 1: Simulation Authority**: Authoritative simulation systems own all survivor vital states, inventory counts, faction standing, and quest progression. A letter is a person's testimony, not a command to the simulation.
2. **Invariant 2: Zero Direct Prose Mutation**: Reading a letter never automatically kills a survivor, changes affinity scores, deducts items, or unlocks map locations.
3. **Invariant 3: Zero Engine Coupling**: All Core code (`PersonalLetterCatalog.cs`, `PersonalLetterProjection.cs`) is pure C# (`netstandard2.1`), with zero references to Godot or Unity engines.
4. **Invariant 4: Idempotence & Read Safety**: Reading the same letter any number of times produces zero side effects and identical readouts.
5. **Invariant 5: Save State Neutrality**: Discovered letters are recorded via existing knowledge keys (`KnowledgeKeys.NarrativeDiscovered`) in `JournalSystem`. No new save stores or envelope format changes are introduced. Old saves remain 100% load-compatible.

---

## 4. Implementation Structure
- `Assets/Ashfall.Core/Narrative/PersonalLetterCatalog.cs`: Engine-agnostic loader, indexer, search, and query interface.
- `Assets/Ashfall.Core/Narrative/PersonalLetterProjection.cs`: Spatial mapping to canonical rooms, truth/provenance classification, and pure query helpers.
- `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`: `PersonalLetterSourceAdapter` for Journal integration.
- `src/Main.ShelterInfrastructure.cs`: Host accessor `GetPersonalLetterCatalog()`.
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`: Static utilization tracking.
- `Ashfall.Core.Tests/PersonalLetterCatalogTests.cs`: Comprehensive test coverage.
