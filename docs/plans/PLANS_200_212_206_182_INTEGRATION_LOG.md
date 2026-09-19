# Integration Log: Plans 200, 212, 206, and 182 Full Production Sealing

**Date:** 2026-09-20
**Scope:**
1. **Plan 200 (`C2[40]`): Survivor Personal Quests & Character Arcs**
2. **Plan 212 (`C2[43]`): Time Capsule & Legacy Messages System**
3. **Plan 206 (`E1[23]`): Survivor Death Records, Wills & Estates System**
4. **Plan 182 (`E1[16]`): Relationship Decay & Social Drift System**

---

## 1. Architectural Summary

### Plan 200: Survivor Personal Quests (`C2[40]`)
- **Core Authority:** [`Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`](../../Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs)
- **Host Session & Save Store:** [`src/Host/PersonalQuestHostSession.cs`](../../src/Host/PersonalQuestHostSession.cs), [`src/Host/PersonalQuestSaveStore.cs`](../../src/Host/PersonalQuestSaveStore.cs)
- **UI Surface:** [`src/UI/PersonalQuestPanel.cs`](../../src/UI/PersonalQuestPanel.cs) implementing `IBindablePanel`, wired into [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs) under `PERSONAL QUESTS`.
- **Main Lifecycle:** [`src/Main.PersonalQuests.cs`](../../src/Main.PersonalQuests.cs), daily ticking and envelope capture in `Main.ExpandedShelterSystems.cs`.
- **CLI & Gate:** `--personal-quests-selftest` in `HostCliRegistry.cs` and `HostCli.cs`; integration test [`Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs`](../../Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs).

### Plan 212: Time Capsule & Legacy Messages (`C2[43]`)
- **Core Authority:** [`Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs`](../../Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs)
- **Host Session & Save Store:** [`src/Host/TimeCapsuleHostSession.cs`](../../src/Host/TimeCapsuleHostSession.cs), [`src/Host/TimeCapsuleSaveStore.cs`](../../src/Host/TimeCapsuleSaveStore.cs) under `user://time_capsules_save.json` and campaign save section `time_capsules`.
- **UI Surface:** [`src/UI/TimeCapsulePanel.cs`](../../src/UI/TimeCapsulePanel.cs) implementing `IBindablePanel`, wired into [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs) under `TIME CAPSULE`.
- **Main Lifecycle:** [`src/Main.TimeCapsule.cs`](../../src/Main.TimeCapsule.cs), daily ticking and envelope capture in `Main.ExpandedShelterSystems.cs`.
- **CLI & Gate:** `--time-capsule-selftest` in `HostCliRegistry.cs` and `HostCli.cs`; integration test [`Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs`](../../Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs).

### Plan 206: Survivor Death Records, Wills & Estates (`E1[23]`)
- **Core Authority:** [`Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs)
- **Host Session & Save Store:** [`src/Host/SurvivorDeathLegacyHostSession.cs`](../../src/Host/SurvivorDeathLegacyHostSession.cs), [`src/Host/SurvivorDeathLegacySaveStore.cs`](../../src/Host/SurvivorDeathLegacySaveStore.cs) under `user://death_legacy_save.json` and campaign save section `death_legacy`.
- **UI Surface:** [`src/UI/SurvivorDeathLegacyPanel.cs`](../../src/UI/SurvivorDeathLegacyPanel.cs) implementing `IBindablePanel`, wired into [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs) under `WILLS & LEGACY`.
- **Main Lifecycle:** [`src/Main.SurvivorDeathLegacy.cs`](../../src/Main.SurvivorDeathLegacy.cs), daily ticking and envelope capture in `Main.ExpandedShelterSystems.cs`.
- **CLI & Gate:** `--death-legacy-selftest` in `HostCliRegistry.cs` and `HostCli.cs`; integration test [`Ashfall.Core.Tests/Survivors/Plan206SurvivorDeathLegacyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan206SurvivorDeathLegacyIntegrationTests.cs).

### Plan 182: Relationship Decay & Social Drift (`E1[16]`)
- **Core Authority:** [`Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs`](../../Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs)
- **Host Session & Save Store:** [`src/Host/RelationshipDecayHostSession.cs`](../../src/Host/RelationshipDecayHostSession.cs), [`src/Host/RelationshipDecaySaveStore.cs`](../../src/Host/RelationshipDecaySaveStore.cs) under `user://relationship_decay_save.json` and campaign save section `relationship_decay`.
- **UI Surface:** [`src/UI/RelationshipDecayPanel.cs`](../../src/UI/RelationshipDecayPanel.cs) implementing `IBindablePanel`, wired into [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs) under `SOCIAL BONDS`.
- **Main Lifecycle:** [`src/Main.RelationshipDecay.cs`](../../src/Main.RelationshipDecay.cs), daily ticking and envelope capture in `Main.ExpandedShelterSystems.cs`.
- **CLI & Gate:** `--relationship-decay-selftest` in `HostCliRegistry.cs` and `HostCli.cs`; integration test [`Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs).

---

## 2. Verification Evidence
- `dotnet test Ashfall.Core.Tests` passed (54/54 tests PASS).
- `generate-architecture-map.py` verified 204 subsystems with 100% mechanical evidence.
- `generate-save-store-matrix.py` regenerated matrix with 205 save store classes.
- `generate-port-contract.py` generated 264 seams.
- Census status for `C2[40]`, `C2[43]`, `E1[23]`, `E1[16]` updated to `SEALED`.
