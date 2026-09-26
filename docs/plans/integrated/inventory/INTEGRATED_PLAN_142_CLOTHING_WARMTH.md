# UNBLOCK — Plan 142: Clothing & Warmth Full Host Integration

> **Status:** FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **Package:** `UNBLOCK-PLAN142-CLOTHING-WARMTH`
> **Target:** Plan 142 / C2[29] — Clothing & Warmth Gear Progression (`ClothingWarmthSystem` in Core)
> **Signed design authority:** `DEC-109` (SIGNED 2026-09-20)
> **Role:** Integrator (user-authorized full integration, 2026-09-26)
> **Zero-partials rule:** domain census, persistence, campaign day owner, diagnostic probe, UI readout, tests, generated gates, and governance are all delivered. No partial residue.

---

## 1. Premise audit (current evidence)

`ClothingWarmthSystem` (DEC-109) has **0 references in `src/`**.
`NeedsSystem.ClothingWarmthReductionProvider` is declared at
`Assets/Ashfall.Core/Survivors/NeedsSystem.cs:97` but was assigned only by the
Core test `ClothingWarmthSystemTests.cs:90` — never by the host. Equipped
clothing therefore had **zero gameplay effect** in the live game. There was no
`clothing_warmth` save section, no CLI probe, and no UI readout.

The 8 signed profile item ids do **not** exist in `items.json`. Authoring a
`clothing_warmth_profiles.json` catalog would declare foreign keys that resolve
nowhere and trip the integrity pipeline, so the DEC-109 internal profile table
remains the system's data authority (a catalog is deliberately not invented).

## 2. Delivered

- **Core:** additive `ClothingWarmthCensus` read model + `GetCensus()` in
  `ClothingWarmthSystem.cs`; `clothing_warmth` save section
  (`clothing_warmth_save.json`) in `SaveSectionRegistry`; `clothing_warmth_ticked`
  internal heartbeat in `DayEventVocabulary`; `ClothingWarmthSelfTest` action +
  descriptor in `HostCliRegistry`.
- **Host:** `ClothingWarmthHostSession` + `ClothingWarmthSaveStore`
  (`SaveStoreHub.Checksummed<ClothingWarmthSaveState>`); `Main.ClothingWarmth.cs`
  (setup/save/flush/reset, equip/unequip, wetness/drying/condition advance,
  census, readout). The existing `NeedsSystem.ClothingWarmthReductionProvider`
  is bound to `ClothingWarmthSystem.CalculateColdLossReduction`.
- **Day owner:** phase-5 `ClothingWarmthDayOwner` with `IPreDaySnapshotRestore`;
  it applies precipitation wetness read from the canonical `WeatherSystem`,
  dries gear in the shelter, and accrues one day of wear per equipped layer.
- **Probe:** `HostCliClothingWarmth` 12-check diagnostic (`--clothing-warmth-selftest`).
- **UI:** read-only `Clothing:` row on `SurvivorDetailPanel` via `ClothingWarmthReadout`.
- **Authority boundaries (Rule 5):** `NeedsSystem` owns survivor warmth; `Inventory`
  owns physical items; `WeatherSystem` owns weather. This system owns only
  equipped-layer records, wetness, gear condition, and the reduction fraction.

## 3. Evidence

- Core/`ClothingWarmthSystemTests` 7/7; `Plan142ClothingWarmthHostIntegrationTests` 7/7.
- `--clothing-warmth-selftest` 12/12 headless.
- Save round-trip pin: `ComprehensiveSaveStoreCorruptionAndMigrationTests` 1634/1634
  (sections 272).
- Adjacent gates: `SaveSectionRegistryTests` 5/5, `HostCliActionParityGateTests` 4/4,
  `HostCliHelpContractTests` 2/2, `DayEventParitySourceGateTests` 2/2,
  `MainTriadDriftGateTests` 7/7.
- Generated `--check`: architecture map 272 subsystems, save-store matrix 274,
  selftest manifest 212, CLI catalog 276/427, catalog registry 711.
- Builds 0 errors / 0 warnings.

## 4. Fleet note

The pre-existing `EVENT_SEMANTIC_PARITY_MATRIX.md` was stale from Plan 204
(`recruitment_ticked` missing) and is repaired here alongside the Plan 142 row.
The `AllSaveSections_TotalCountMatchesContractMatrix` pin was stale from the
concurrent section additions and is reconciled to the measured 272.
