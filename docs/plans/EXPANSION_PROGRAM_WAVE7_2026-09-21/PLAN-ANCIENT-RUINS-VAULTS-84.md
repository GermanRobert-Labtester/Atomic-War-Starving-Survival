# PLAN-ANCIENT-RUINS-VAULTS-84 — Sealed Complexes, Cryo Facilities & Relic Sites

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DEEP-STRATA-83, PLAN-ANOMALY-PHANTOM-63,
PLAN-SCIENCE-EDUCATION-38.
**Implementation scaffold:** [`PLAN-ANCIENT-RUINS-VAULTS-84_APPENDIX-A_SCAFFOLD.md`](PLAN-ANCIENT-RUINS-VAULTS-84_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ARCHAEOLOGY-TRUTH-152` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real military/corporate references; fictional installations;
no loot pinata — sites are authored set-pieces with cost and consequence.

## Outcome
Pre-war sites exist across data and code: `deep_lore_locations.json` (25),
`prewar_archives.json`, `verdict_locations.json`, `dive_sites.json`,
`excavation_sites.json` (8), `AbyssalAnomalies` (cryopod failures), vault/cryo
families (`CryoVault`, `CryogenicAirSeparationSystem`, `CryoVault` setup),
`SafeCrackingSystem`, `PrewarArchiveDecryptionSystem`, `UniqueItemClaimRegistry`.
This plan makes **vaults** the deep-content pillar: sealed doors, layered
hazards, documents, decisions.

| Layer | Content | Owner |
|---|---|---|
| Approach | routes, gates, weather/zone hazards | map/weather owners |
| Entry | locks, codes, breaching, power restoration | electronics/breaching |
| Interior | hazards (plume, cold, radiation, structural) | hazard owners |
| Records | archives, terminals, tapes, personal logs | archive/decrypt owner |
| Salvage | unique relics, prototypes, supplies | unique-claim + inventory |
| Inhabitants | survivors, machines, factions present | visitors/combat |
| Decision | seal it, loot it, open it, leave it | narrative + consequences |
| Aftermath | exposure, attention, faction interest | espionage/politics |

## Evidence
- Core: `SafeCrackingSystem`, `PrewarArchiveDecryptionSystem`, `CryoVault`/`CryogenicAirSeparationSystem`, `UniqueItemClaimRegistry`, `VerdictEvidenceChain`, `MachineLogSystem`, `District8DeepCoastSystem` (salvage rolls).
- Data: `deep_lore_locations.json` (25 with loot nodes), `prewar_archives.json`, `verdict_locations.json`, `dive_sites.json`, `excavation_sites.json`.
- Sealed prior: Plan 116 deep lore (4/4), Plan 139 InSAR, Plan 87 relic recipes (39), Plan 82 investigation sites, `--relic-selftest`.
- Contracts: unique items via the claim registry; no duplicate loot tables; records feed archive/codex.

## Packages
- **RV-84A** site model: authored multi-room complexes with state (power, doors, hazards) and a map marker revealed by survey/records.
- **RV-84B** entry puzzles: codes, power, breaching; tools and skills matter; failure is loud/costly.
- **RV-84C** interior hazards: authored per site, using canonical hazard owners.
- **RV-84D** records: terminals/tapes/logs (decryption gated) yielding lore, schematics, and quest hooks.
- **RV-84E** unique salvage: relics/prototypes via `UniqueItemClaimRegistry`; display/study/trade.
- **RV-84F** inhabitants/decisions: survivors, machines, or faction squads; outcomes (rescue, alliance, retreat, seal).
- **RV-84G** content volumes: +6 vaults, +10 rooms, +12 records, +8 uniques; fictional.

## Acceptance & verification
- No duplicate uniques; records all resolve to consumers; hazards canonical; determinism.
- `godot --headless --path . -- --deep-coast-selftest`; `--world-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/`.

## Risks
Loot inflation → unique registry + limited sites + study requirements; sites are one-time but records recur.

---

## 6. Expanded census (4 files · 1,023 lines)

Scope: `Assets/Ashfall.Core/Archaeology/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ArchaeologySystem.cs` | 317 | System | **yes** | 0 | 0 | 2 |
| `BlackProjectsArchiveSystem.cs` | 367 | System | **yes** | 0 | 0 | 2 |
| `IndustrialRuinsCatalog.cs` | 239 | Catalog | — | 0 | 0 | 0 |
| `RelicProvenanceCatalog.cs` | 100 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `relic_recipes.json` | object[2 keys] |
| `architect_vault_audits.json` | array[7] |
| `relic_provenance_dossiers.json` | object[3 keys] |
| `surface_dragline_ruins.json` | array[8] |
| `vault_seal_breach_logs.json` | array[7] |

**State surfaces:** `ArchaeologySystem.cs`, `BlackProjectsArchiveSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Archaeology/` |
| Test references | 7 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 2. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RV-84A` | no name match — resolve at claim time |
| `RV-84B` | no name match — resolve at claim time |
| `RV-84C` | no name match — resolve at claim time |
| `RV-84D` | no name match — resolve at claim time |
| `RV-84E` | no name match — resolve at claim time |
| `RV-84F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **5** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/BlackProjectsArchiveSaveStore.cs`, `src/Main.Plans152.cs`, `src/Main.Plans186_189.cs`, `src/UI/ArchaeologyExcavationPanel.cs`, `src/UI/BlackProjectsArchivePanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`, `Ashfall.Core.Tests/Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs`, `Ashfall.Core.Tests/IndustrialRuinsCatalogTests.cs`, `Ashfall.Core.Tests/Integration/Plans186_189_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **0**; isolated: **4**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archaeology` |
| `archive_desk` |
| `black_market` |
| `black_projects_archive` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `technical_material_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--black-flotilla-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnProvenanceComplete` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json` |
| `Assets/StreamingAssets/Data/narrative/surface_dragline_ruins.json` |
| `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json` |
| `Assets/StreamingAssets/Data/relic_recipes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 4 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Archaeology` | 1 | 4 |

**Verdict:** 4 cases sit under matching regions — run those first (`Archaeology`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **20**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CulturalArchiveSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/LeatherworkArchiveSaveStore.cs` |
| `src/Host/PrewarArchiveSaveStore.cs` |
| `src/Host/TechnicalMaterialArchiveSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **8**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archaeology` | no |
| `archive_desk` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `grain_milling_archive` | no |
| `hydrogeology_archive` | no |
| `leatherwork_archive` | no |
| `technical_material_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `narrative/relic_provenance_dossiers.json` | CODEX_ONLY |
| `narrative/surface_dragline_ruins.json` | CODEX_ONLY |
| `narrative/vinyl_record_archive.json` | CODEX_ONLY |
| `relic_recipes.json` | GAMEPLAY_CONSUMED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 8 (laddered 0) · RNG streams 3 · host files 16 · catalogs 16 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ANCIENT-RUINS-VAULTS-84
wave: 7
status: PROPOSED — foreman claim required
packages: RV-84A, RV-84B, RV-84C, RV-84D, RV-84E, RV-84F, RV-84G
claim paths:
  - src/Host/ArchaeologySaveStore.cs  # §19 candidate host surface
  - src/Host/ArchiveDeskHostSession.cs  # §19 candidate host surface
  - src/Host/BlackMarketHostSession.cs  # §19 candidate host surface
  - src/Host/BlackMarketSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_inks.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
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
