# PLAN-INVENTORY-FAMILY-TRUTH-271 — Migration, Transactions & Port Contracts

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INVENTORY-CONSERVATION-93, PLAN-SAVE-MIGRATION-CORRIDOR-87, PLAN-PLAYER-COMMAND-TRUTH-131.
**Non-goals:** no inventory redesign; the family is audited for migration and
port correctness.

## 1. Outcome
**17 `Inventory/` files** are referenced by no plan: `InventoryMigrator`,
`InventoryTransaction`, `InventoryProvenance`, `ItemAliases`,
`IEquipmentConditionSink`, `IPlayerInventoryPort`, `DeviceState`, and loaders.
Migration and transaction code is the highest-risk plumbing in the game —
silent failures there corrupt saves.

| Deliverable | Detail |
|---|---|
| Migrator audit | `InventoryMigrator` paths enumerated; each migration is reversible-checkable and tested |
| Transaction integrity | `InventoryTransaction` atomicity: partial application is impossible (fixture) |
| Alias coverage | `ItemAliases` maps legacy ids; unknown aliases fail typed (Plan 34 families) |
| Port contracts | the two port interfaces have one implementer each, named |
| Provenance | `InventoryProvenance` fields trace to owners and survive round-trip |

## 2. Evidence
- 17 `Inventory/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 93 owns conservation; Plan 87 the save ladder the migrator feeds.
- Plan 131's command envelope is the transaction boundary.

## 3. Packages
- **INF-271A** migrator path tests (per migration).
- **INF-271B** transaction atomicity fixture.
- **INF-271C** alias resolution + typed failure.
- **INF-271D** port implementer audit.
- **INF-271E** provenance round-trip.

## 4. Acceptance & verification
- Migrations are idempotent and tested; a partial transaction is impossible; aliases resolve.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/`.

## 5. Risks
Silent corruption → atomicity fixture and typed failures.
Alias sprawl → alias table closed with a failure test.

---

## 6. Expanded census (21 files in scope · 4,948 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Inventory/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
Support 12 · Catalog 3 · System 2 · DTO/Type 2 · Loader 2.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `ClothingWarmthSystem.cs` | 390 | System | 0 | 0 | 2 | since-mentioned |
| `DeviceState.cs` | 136 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `EquipLimbGate.cs` | 104 | Support | 0 | 0 | 0 | since-mentioned |
| `IEquipmentConditionSink.cs` | 19 | Support | 0 | 0 | 0 | since-mentioned |
| `IPlayerInventoryPort.cs` | 24 | Support | 0 | 0 | 0 | since-mentioned |
| `Inventory.cs` | 1353 | Support | 0 | 0 | 4 | since-mentioned |
| `InventoryMigrator.cs` | 61 | Support | 0 | 0 | 0 | since-mentioned |
| `InventoryProvenance.cs` | 44 | Support | 0 | 0 | 0 | since-mentioned |
| `InventoryTransaction.cs` | 388 | Support | 0 | 0 | 1 | since-mentioned |
| `ItemAliases.cs` | 83 | Support | 0 | 0 | 0 | since-mentioned |
| `ItemCatalogLoader.cs` | 783 | Loader | 0 | 0 | 0 | since-mentioned |
| `ItemDefinitions.cs` | 342 | Support | 0 | 0 | 0 | since-mentioned |
| `ItemDescriptionCatalog.cs` | 129 | Catalog | 0 | 0 | 0 | since-mentioned |
| `ItemDescriptionCatalogLoader.cs` | 117 | Loader | 0 | 0 | 0 | since-mentioned |
| `ItemDescriptionEntry.cs` | 129 | Support | 0 | 0 | 0 | since-mentioned |
| `ItemInspectionModel.cs` | 153 | Support | 0 | 0 | 0 | since-mentioned |
| `ItemLoreSystem.cs` | 308 | System | 0 | 0 | 2 | since-mentioned |
| `ItemTagCatalog.cs` | 35 | Catalog | 0 | 0 | 0 | since-mentioned |
| `ItemTypes.cs` | 56 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `ProceduralItemInstance.cs` | 159 | Support | 1 | 0 | 0 | since-mentioned |
| `StartingSuppliesCatalog.cs` | 135 | Catalog | 0 | 0 | 0 | since-mentioned |

**Census totals:** 1 banned nondeterministic references · 0 empty-catch sites · 4 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `black_market_inventory.json` | array[7] |
| `personal_effects_inventory_batch_2.json` | array[15] |

**State surfaces (capture/restore present):**

- `ClothingWarmthSystem.cs`
- `Inventory.cs`
- `InventoryTransaction.cs`
- `ItemLoreSystem.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Inventory/` |
| Files referenced by tests | 378 name references across the test tree |
| Determinism scan | 1 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Drift | 21 of 21 files became plan-referenced since authoring — re-verify their owners |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every file in scope; no edits.
2. Still-unmentioned files: the original audit target — consumer or ownerless verdict.
3. Since-mentioned files: confirm the new plan's claim actually owns them; no double ownership.
4. Catalogs and loaders: justify or report inert.
5. Systems and saves: one owner per state; keys per Plan 1 Appendix Q.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(21 files). Other plans referencing those names: **19**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-INVENTORY-CONSERVATION-93` | 6 |
| `EVIDENCE` | 2 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 2 |
| `PLAN-COLLECTIBLES-RELICS-67` | 2 |
| `PLAN-THERMAL-EXPOSURE-TRUTH-117` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-UI-SURFACE-15` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `INF-271A` | `InventoryMigrator.cs` |
| `INF-271B` | `InventoryTransaction.cs` |
| `INF-271C` | `ItemAliases.cs` |
| `INF-271D` | no name match — resolve at claim time |
| `INF-271E` | `InventoryProvenance.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 21; intra-domain edges: **43**; isolated files:
**0**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ClothingWarmthSystem` | `Inventory` |
| `DeviceState` | `Inventory` |
| `EquipLimbGate` | `Inventory` |
| `IEquipmentConditionSink` | `Inventory` |
| `IPlayerInventoryPort` | `Inventory` |
| `IPlayerInventoryPort` | `InventoryTransaction` |
| `Inventory` | `DeviceState` |
| `Inventory` | `EquipLimbGate` |
| `Inventory` | `IEquipmentConditionSink` |
| `Inventory` | `IPlayerInventoryPort` |
| `Inventory` | `InventoryTransaction` |
| `Inventory` | `ItemAliases` |
| `InventoryMigrator` | `Inventory` |
| `InventoryMigrator` | `ItemAliases` |
| `InventoryProvenance` | `Inventory` |
| `InventoryProvenance` | `ItemAliases` |
| `InventoryTransaction` | `DeviceState` |
| `InventoryTransaction` | `Inventory` |
| `ItemAliases` | `Inventory` |
| `ItemCatalogLoader` | `Inventory` |
| `ItemCatalogLoader` | `ItemAliases` |
| `ItemCatalogLoader` | `ItemDescriptionCatalog` |
| `ItemCatalogLoader` | `ItemDescriptionCatalogLoader` |
| `ItemCatalogLoader` | `StartingSuppliesCatalog` |
| `ItemDefinitions` | `Inventory` |
| `ItemDefinitions` | `ItemTagCatalog` |
| `ItemDescriptionCatalog` | `Inventory` |
| `ItemDescriptionCatalog` | `ItemAliases` |
| `ItemDescriptionCatalog` | `ItemDescriptionEntry` |
| `ItemDescriptionCatalogLoader` | `Inventory` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `Inventory` | 20 |
| `ItemAliases` | 5 |
| `ItemDescriptionCatalog` | 3 |
| `ItemDescriptionEntry` | 3 |
| `DeviceState` | 2 |
| `InventoryTransaction` | 2 |
| `EquipLimbGate` | 1 |
| `IEquipmentConditionSink` | 1 |
| `IPlayerInventoryPort` | 1 |
| `ItemDefinitions` | 1 |

**Class split:** hub 14 · sink 0 · source 7 · isolated 0.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 21. Host files: **176** · Test files: **269** · Data files: **15**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 176 | `src/Audio/AudioSelfTest.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/AgricultureHostSession.cs`, `src/Host/ArchiveDeskHostSession.cs`, `src/Host/AutopsyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 269 | `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`, `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/AutopsyBridgeTests.cs` |
| Data (`StreamingAssets/Data/`) | 15 | `Assets/StreamingAssets/Data/faction_radio_corpus.json`, `Assets/StreamingAssets/Data/holdfast_quests.json`, `Assets/StreamingAssets/Data/independent_faction_branch.json`, `Assets/StreamingAssets/Data/items.json`, `Assets/StreamingAssets/Data/journal_voice_prose.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `equipment` |
| `equipment_condition` |
| `inventory` |
| `oral_lore` |
| `procedural_narrative` |
| `starting_level` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--port-contract-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **16**.

| Event | First declaration |
|---|---|
| `OnCampSuppliesReserved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnEntryAdded` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |
| `OnEquipmentChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnInspectionCompleted` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/expansion_item_tags.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/hobby_definitions.json` |
| `Assets/StreamingAssets/Data/item_degradation.json` |
| `Assets/StreamingAssets/Data/item_description_texts.json` |
| `Assets/StreamingAssets/Data/lore_archives.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (16 files, 118 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Equipment` | 1 | 4 |
| `Inventory` | 15 | 114 |

**Verdict:** 118 cases sit under matching regions — run those first (`Equipment`, `Inventory`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **23**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.StartingSupplies.cs` |
| `src/Host/InventoryHostSession.cs` |
| `src/Host/InventorySaveSelfTest.cs` |
| `src/Host/InventorySaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/OralLoreHostSession.cs` |
| `src/Host/OralLoreSaveStore.cs` |
| `src/Host/PortContractSelfTest.cs` |
| `src/Host/ProceduralNarrativeHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `equipment` | no |
| `equipment_condition` | no |
| `inventory` | no |
| `oral_lore` | no |
| `procedural_narrative` | no |
| `starting_level` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **17**
(CODEX_ONLY 7, GAMEPLAY_CONSUMED 6, OPTIONAL 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `expansion_item_tags.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `item_degradation.json` | OPTIONAL |
| `item_description_texts.json` | OPTIONAL |
| `lore_archives.json` | GAMEPLAY_CONSUMED |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/deep_lore_texts.json` | CODEX_ONLY |
| `narrative/equipment_failure_logs.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 19
**Surface:** save sections 6 (laddered 0) · RNG streams 0 · host files 12 · catalogs 22 · test regions 2 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INVENTORY-FAMILY-TRUTH-271
wave: 19
status: PROPOSED — foreman claim required
packages: INF-271A, INF-271B, INF-271C, INF-271D, INF-271E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Host/EquipmentConditionHostSession.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/HostCli.StartingSupplies.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/black_market_inventory.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/breaching_equipment_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Equipment/
  - godot --headless --path . -- --inventory-save-selftest
dependencies:
  - coordinate: 19 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
