# Epilogue Reachability & Witness Matrix

> **Scope:** All 25 Campaign-Ending Outcomes
> **Rule:** Every ending must have a proven, reachable terminal campaign state witness.

---

## Complete 25-Ending Reachability Matrix

| # | Ending Key | Title | Category | Required Conditions | Exclusions | Witness State in Tests |
|---|---|---|---|---|---|---|
| 1 | `the_open_muster` | The Open Muster | Muster Core | `MusterEndingKey == "the_open_muster"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 2 | `the_amnesty` | The Amnesty | Muster Core | `MusterEndingKey == "the_amnesty"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 3 | `the_corridor` | The Corridor | Muster Core | `MusterEndingKey == "the_corridor"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 4 | `the_blood_price` | The Blood Price | Muster Core | `MusterEndingKey == "the_blood_price"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 5 | `the_rate_card_revised` | The Rate Card, Revised | Hydro Barons | `MusterEndingKey == "the_rate_card_revised"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 6 | `the_administrator` | The Administrator | Hydro Barons | `MusterEndingKey == "the_administrator"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 7 | `the_measured_truth_contested` | The Measured Truth, Contested | Cold Count | `MusterEndingKey == "the_measured_truth_contested"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 8 | `the_measured_truth` | The Measured Truth | Cold Count | `MusterEndingKey == "the_measured_truth"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 9 | `unwritten` | Unwritten | Fallback | All condition fields false / empty | Active quest resolutions | `MusterEpilogueMatrixTests.Evaluate_DefaultInput_ReturnsUnwritten` |
| 10 | `ending_verdict_the_sector_recounts` | The Sector Recounts | The Verdict | `VerdictEndingKey == "ending_verdict_the_sector_recounts"` | `ShelterFallen` | `MusterEpilogueMatrixTests.Evaluate_VerdictEnding_TakesPrecedenceOverGenericFactionOrResource` |
| 11 | `ending_verdict_the_count_is_held` | The Count Is Held | The Verdict | `VerdictEndingKey == "ending_verdict_the_count_is_held"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 12 | `ending_verdict_the_offer_is_a_lease` | The Offer Is a Lease | The Verdict | `VerdictEndingKey == "ending_verdict_the_offer_is_a_lease"` | `ShelterFallen` | `MusterEpilogueMatrixTests.EveryKey_HasProseInLoadedCatalog` |
| 13 | `ending_garrison_absorbs_coalition` | Under Their Watch | Faction | `FactionOutcome == GarrisonAbsorbed` | `ShelterFallen`, Compound | `MusterEpilogueMatrixTests.Evaluate_FactionEndings_SelectsCorrectly` |
| 14 | `ending_rebuilders_joined` | Hands on the Ruins | Faction | `FactionOutcome == RebuildersJoined` | `ShelterFallen`, Compound | `MusterEpilogueMatrixTests.Evaluate_FactionEndings_SelectsCorrectly` |
| 15 | `ending_coalition_independent` | No Banner | Faction | `FactionOutcome == Independent` | `ShelterFallen`, Compound | `MusterEpilogueMatrixTests.Evaluate_FactionEndings_SelectsCorrectly` |
| 16 | `ending_foundry_annexation` | Stamped in Steel | Faction | `FactionOutcome == FoundryAnnexed` | `ShelterFallen`, Compound | `MusterEpilogueMatrixTests.Evaluate_FactionEndings_SelectsCorrectly` |
| 17 | `ending_water_plant_held` | The Last Clean Line | Resource | `WaterPlantHeld == true` | `ShelterFallen`, `MercyPattern`, Faction | `MusterEpilogueMatrixTests.Evaluate_ResourceEndings_SelectsCorrectly` |
| 18 | `ending_grain_silo_captured` | The Grain Count | Resource | `GrainSiloCaptured == true` | `ShelterFallen`, Faction, Water | `MusterEpilogueMatrixTests.Evaluate_ResourceEndings_SelectsCorrectly` |
| 19 | `ending_fuel_depot_burned` | Fire at the Depot | Resource | `FuelDepotBurned == true` | `ShelterFallen`, `IronPattern`, Faction, Water, Grain | `MusterEpilogueMatrixTests.Evaluate_ResourceEndings_SelectsCorrectly` |
| 20 | `ending_mercy_road` | The Mercy Road | Moral | `MercyPattern == true` | `ShelterFallen`, `WaterPlantHeld`, Faction, Resource | `MusterEpilogueMatrixTests.Evaluate_MoralEndings_SelectsCorrectly` |
| 21 | `ending_iron_way` | The Iron Way | Moral | `IronPattern == true` | `ShelterFallen`, `FuelDepotBurned`, Faction, Resource | `MusterEpilogueMatrixTests.Evaluate_MoralEndings_SelectsCorrectly` |
| 22 | `ending_listeners_thread` | The Listener's Thread | Moral | `DiplomacyPattern == true` | `ShelterFallen`, Faction, Resource, Mercy, Iron | `MusterEpilogueMatrixTests.Evaluate_MoralEndings_SelectsCorrectly` |
| 23 | `ending_mercy_water_held` | Water for the Road | Compound | `MercyPattern && WaterPlantHeld` | `ShelterFallen` | `MusterEpilogueMatrixTests.Evaluate_CompoundEnding_MercyAndWaterHeld_BeatsComponentEndings` |
| 24 | `ending_iron_fuel_ash` | Ash in the Tanks | Compound | `IronPattern && FuelDepotBurned` | `ShelterFallen` | `MusterEpilogueMatrixTests.Evaluate_CompoundEnding_IronAndFuelBurned_BeatsComponentEndings` |
| 25 | `ending_shelter_falls` | What They Found | Failure | `ShelterFallen == true` | None (Highest Priority) | `MusterEpilogueMatrixTests.Evaluate_FailurePrecedence_ShelterFallenOverridesEverything` |
