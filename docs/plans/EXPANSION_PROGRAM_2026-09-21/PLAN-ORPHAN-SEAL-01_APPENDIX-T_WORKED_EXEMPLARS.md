# PLAN-ORPHAN-SEAL-01 — Appendix T: Worked Seal Exemplars

**Generated:** 2026-09-21. Four fully worked seal specifications assembled from
Appendices A–U (one per archetype), so a package author can copy the shape:
**CommunicationsSystem** (top risk score 7), **BestiarySystem** (island with a
route but no attachment), **CupolaFoundryEngine** (stateful engine with bound
catalogs), **StormForecastReadinessEngine** (small stateless evaluator).
**Note:** each exemplar is a *proposal*; the claim and premise re-check still
belong to the foreman and the executing package.

## Exemplar: `CommunicationsSystem`

- **File:** `Communications/CommunicationsSystem.cs` · **Lines:** 561 · **Tests referencing:** 1: `Communications/Plan157CommunicationsIntegrationTests.cs`
- **Save surface:** `LoadCatalog`, `RestoreState`
- **Candidate catalogs:** `communications_networks.json`, `nvis_communications_catalog.json`
- **Host partial candidates:** none — new attachment or headless-only
- **Route candidates:** none — surface owner decision

**Package shape (proposal):**

1. Premise re-check: file hash vs Appendix I; catalogs vs Appendix R; tests vs Appendix J.
2. Interface package: host adapter (or session method) exposing the members in Appendix K; no new authority.
3. State package (only if `Capture`/`Restore` exists): register through the save owner; key per Appendix Q (all proposals collision-free).
4. Surface package: attach the route above or ship headless-only with a recorded reason.
5. Verification package: focused command from Appendix O; re-run the reachability audit and expect this file to leave the orphan set.

## Exemplar: `BestiarySystem`

- **File:** `Bestiary/BestiarySystem.cs` · **Lines:** 285 · **Tests referencing:** 1: `Bestiary/Plan187BestiaryIntegrationTests.cs`
- **Save surface:** `LoadCatalog`, `RestoreState`
- **Candidate catalogs:** `wasteland_wildlife_bestiary.json`
- **Host partial candidates:** none — new attachment or headless-only
- **Route candidates:** `bestiary`

**Package shape (proposal):**

1. Premise re-check: file hash vs Appendix I; catalogs vs Appendix R; tests vs Appendix J.
2. Interface package: host adapter (or session method) exposing the members in Appendix K; no new authority.
3. State package (only if `Capture`/`Restore` exists): register through the save owner; key per Appendix Q (all proposals collision-free).
4. Surface package: attach the route above or ship headless-only with a recorded reason.
5. Verification package: focused command from Appendix O; re-run the reachability audit and expect this file to leave the orphan set.

## Exemplar: `CupolaFoundryEngine`

- **File:** `Shelter/CupolaFoundryEngine.cs` · **Lines:** 445 · **Tests referencing:** 1: `Shelter/CupolaFoundryEngineTests.cs`
- **Save surface:** `RestoreState`
- **Candidate catalogs:** `cupola_foundry_catalog.json`, `cupola_melting_ratio_audits.json`, `cupola_slag_leaching_records.json`, `foundry_accords.json`
- **Host partial candidates:** `Main.ExpandedShelterSystems.cs.uid`, `Main.ShelterBatch3.cs.uid`, `Main.ShelterInfrastructure.cs.uid`
- **Route candidates:** `shelter`, `shelter_atmosphere`, `shelter_barter`

**Package shape (proposal):**

1. Premise re-check: file hash vs Appendix I; catalogs vs Appendix R; tests vs Appendix J.
2. Interface package: host adapter (or session method) exposing the members in Appendix K; no new authority.
3. State package (only if `Capture`/`Restore` exists): register through the save owner; key per Appendix Q (all proposals collision-free).
4. Surface package: attach the route above or ship headless-only with a recorded reason.
5. Verification package: focused command from Appendix O; re-run the reachability audit and expect this file to leave the orphan set.

## Exemplar: `StormForecastReadinessEngine`

- **File:** `World/StormForecastReadinessEngine.cs` · **Lines:** 276 · **Tests referencing:** 1: `World/StormForecastReadinessEngineTests.cs`
- **Save surface:** none found (decide statefulness in EP-01)
- **Candidate catalogs:** none by name — check literal references (Appendix U)
- **Host partial candidates:** `Main.World.cs.uid`, `Main.EvolvingWorld.cs.uid`, `Main.WorldPlaytest.cs.uid`
- **Route candidates:** none — surface owner decision

**Package shape (proposal):**

1. Premise re-check: file hash vs Appendix I; catalogs vs Appendix R; tests vs Appendix J.
2. Interface package: host adapter (or session method) exposing the members in Appendix K; no new authority.
3. State package (only if `Capture`/`Restore` exists): register through the save owner; key per Appendix Q (all proposals collision-free).
4. Surface package: attach the route above or ship headless-only with a recorded reason.
5. Verification package: focused command from Appendix O; re-run the reachability audit and expect this file to leave the orphan set.

