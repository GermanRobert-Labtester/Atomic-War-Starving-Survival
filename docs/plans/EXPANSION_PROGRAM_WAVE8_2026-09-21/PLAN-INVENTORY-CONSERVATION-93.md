# PLAN-INVENTORY-CONSERVATION-93 — Item Conservation Invariants & Duplication Probes

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-REFERENCE-INTEGRITY-34, PLAN-SAVE-GOVERNANCE-12, PLAN-DATA-CONSUMER-22.
**Implementation scaffold:** [`PLAN-INVENTORY-CONSERVATION-93_APPENDIX-A_SCAFFOLD.md`](PLAN-INVENTORY-CONSERVATION-93_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CRAFT-QUALITY-TRUTH-112` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no second inventory store, no per-item global ledger as
authority, no economy rewrite.

## 1. Outcome
Inventory is an existing authority (`save` section `inventory`, `SaveInventory`
/ `SetupInventory`) and item ids are real catalog ids (Plan 34 Appendix A:
`item_` 332 ids across 112 files). What does not exist is a stated and tested
**conservation contract**: every unit that appears has an owned source, every
unit that disappears has an owned sink, and no transfer path can duplicate on
reload. This plan proves the contract at the existing seam instead of adding a
parallel ledger.

| Deliverable | Detail |
|---|---|
| Conservation model | sources (loot, crafting yield, trade-in) and sinks (consumption, decay, trade-out, death) each mapped to their current owner |
| Audit mode | dev-only operation wrapper that counts by item id before/after and reports deltas with the caller named |
| Reload probes | save → load → transfer → reload cycles for stack, partial, and container paths; count must match |
| Edge cases | over-capacity, partial transfer, dead owner's inventory, container-in-container, zero-quantity remainder |
| Invariant tests | focused xUnit cases over the current public inventory API |

## 2. Evidence
- `SaveSectionRegistry.All` contains `("inventory", "SaveInventory", "SetupInventory", "inventory", …)` — the canonical save seam.
- `Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs` is host-unreachable today (Plan 1 Appendix A); it cannot be the conservation owner.
- Plan 34 Appendix A: `item_` id family (332 ids / 112 files) — the id space to count over.
- Fuzz precedent exists (`DoseCollectibleSaveFuzzTests`), so a focused conservation test block fits the current test layout.

## 3. Packages
- **IC-93A** conservation model doc + owner table (no code change).
- **IC-93B** audit wrapper: counts by id around an operation; opt-in in tests/dev only.
- **IC-93C** reload probes: stack/partial/container cycles comparing counts and checksums.
- **IC-93D** edge-case tests: capacity, partial, dead-owner, nested container, remainder.
- **IC-93E** report: any discovered duplication path filed as a finding for its owner (repair goes through the owner's plan).

## 4. Acceptance & verification
- Audit deltas are zero for a scripted sequence of legal operations.
- Reload probes show identical counts and stable checksums.
- Edge cases return typed failures, never silent drops or duplications.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/`.

## 5. Risks
Audit mode becoming a runtime authority → dev/test only, and the wrapper adds
no state. Duplication finding scope → each finding routes to the owning system;
this plan does not invent a fix path beside it.

---

## 6. Expanded census (5 files · 1,870 lines)

Scope: `Assets/Ashfall.Core/Inventory/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `IPlayerInventoryPort.cs` | 24 | Support | — | 0 | 0 | 0 |
| `Inventory.cs` | 1353 | Support | — | 0 | 0 | 4 |
| `InventoryMigrator.cs` | 61 | Support | **yes** | 0 | 0 | 0 |
| `InventoryProvenance.cs` | 44 | Support | — | 0 | 0 | 0 |
| `InventoryTransaction.cs` | 388 | Support | **yes** | 0 | 0 | 1 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `black_market_inventory.json` | array[7] |
| `personal_effects_inventory_batch_2.json` | array[15] |

**State surfaces:** `Inventory.cs`, `InventoryTransaction.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Inventory/` |
| Test references | 317 name references across the test tree |
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

Domain files: 5. Other plans referencing their names: **14**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-INVENTORY-FAMILY-TRUTH-271` | 5 |
| `EVIDENCE` | 1 |
| `PLAN-UI-SURFACE-15` | 1 |
| `PLAN-ASSET-PIPELINE-19` | 1 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |
| `PLAN-FOOD-CUISINE-39` | 1 |
| `PLAN-LOCALIZATION-READINESS-52` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `IC-93A` | no name match — resolve at claim time |
| `IC-93B` | no name match — resolve at claim time |
| `IC-93C` | no name match — resolve at claim time |
| `IC-93D` | no name match — resolve at claim time |
| `IC-93E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **176** · Test files: **267** · Data files: **15**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 176 | `src/Audio/AudioSelfTest.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/AgricultureHostSession.cs`, `src/Host/ArchiveDeskHostSession.cs`, `src/Host/AutopsyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 267 | `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`, `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/AutopsyBridgeTests.cs` |
| Data (`StreamingAssets/Data/`) | 15 | `Assets/StreamingAssets/Data/faction_radio_corpus.json`, `Assets/StreamingAssets/Data/holdfast_quests.json`, `Assets/StreamingAssets/Data/independent_faction_branch.json`, `Assets/StreamingAssets/Data/items.json`, `Assets/StreamingAssets/Data/journal_voice_prose.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **7**; isolated: **0**.

| From | → To |
|---|---|
| `IPlayerInventoryPort` | `Inventory` |
| `IPlayerInventoryPort` | `InventoryTransaction` |
| `Inventory` | `IPlayerInventoryPort` |
| `Inventory` | `InventoryTransaction` |
| `InventoryMigrator` | `Inventory` |
| `InventoryProvenance` | `Inventory` |
| `InventoryTransaction` | `Inventory` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `inventory` |

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

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnProvenanceComplete` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/narrative/personal_effects_inventory_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (15 files, 114 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Inventory` | 15 | 114 |

**Verdict:** 114 cases sit under matching regions — run those first (`Inventory`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **8**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/InventoryHostSession.cs` |
| `src/Host/InventorySaveSelfTest.cs` |
| `src/Host/InventorySaveStore.cs` |
| `src/Host/PortContractSelfTest.cs` |
| `src/Main.Inventory.cs` |
| `src/Main.UiTests.Inventory.cs` |
| `src/UI/InventoryDetailPanel.cs` |
| `src/UI/InventoryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `inventory` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(CODEX_ONLY 2).

| Catalog | Classification |
|---|---|
| `narrative/personal_effects_inventory_batch_2.json` | CODEX_ONLY |
| `narrative/relic_provenance_dossiers.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 14
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 8 · catalogs 5 · test regions 1 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INVENTORY-CONSERVATION-93
wave: 8
status: PROPOSED — foreman claim required
packages: IC-93A, IC-93B, IC-93C, IC-93D, IC-93E
claim paths:
  - src/Host/InventoryHostSession.cs  # §19 candidate host surface
  - src/Host/InventorySaveSelfTest.cs  # §19 candidate host surface
  - src/Host/InventorySaveStore.cs  # §19 candidate host surface
  - src/Host/PortContractSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/black_market_inventory.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/personal_effects_inventory_batch_2.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/
  - godot --headless --path . -- --inventory-save-selftest
dependencies:
  - coordinate: 14 other plan(s) name these artifacts (§12)
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
