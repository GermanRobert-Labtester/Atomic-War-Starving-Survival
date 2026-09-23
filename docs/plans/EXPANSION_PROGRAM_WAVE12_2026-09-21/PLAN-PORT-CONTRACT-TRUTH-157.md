# PLAN-PORT-CONTRACT-TRUTH-157 — Dockside Service Boundary: Contracts Before Docks

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MARITIME-DEEPWATER-27, PLAN-TRANSPORT-EXPEDITION-30, PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140.
**Implementation scaffold:** [`PLAN-PORT-CONTRACT-TRUTH-157_APPENDIX-A_SCAFFOLD.md`](PLAN-PORT-CONTRACT-TRUTH-157_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no dock content, no maritime simulation (Plan 27), no trade
arithmetic (Plan 96).

## 1. Outcome
`Ports/PortContract.cs` is a single contracts-only file — the same pattern as
Plan 140's machinery contracts. Contracts without named realizers attract
half-built systems later; this plan makes the port boundary accountable before
anything docks.

| Deliverable | Detail |
|---|---|
| Contract inventory | each interface/type with its members and stated obligations |
| Realizer map | which system will implement each contract (Plan 27 maritime, Plan 30 transport) and which consumer calls it |
| Test double | at least one thin implementation proving the contract is implementable and sufficient |
| Overlap check | no port contract duplicates an existing route/waystation/maritime concept (Plan 95/153/27) |
| Decision | each contract: adopt with a named owner, or retire with a reason |

## 2. Evidence
- `Assets/Ashfall.Core/Ports/PortContract.cs` (whole directory; verified).
- Plan 140 established the same accountability pattern for `AdvancedMachinery/`.
- Plan 27 owns maritime; Plan 153's waystation network is the land-side analogue.
- Plan 95 owns route edges a port would attach to.

## 3. Packages
- **PCT-157A** contract inventory + obligations.
- **PCT-157B** realizer/consumer map with plan ids.
- **PCT-157C** test double + smoke call.
- **PCT-157D** overlap report against 27/95/153 concepts.
- **PCT-157E** adopt/retire decisions with owners.

## 4. Acceptance & verification
- Every contract has a decision row — no blanks.
- The test double compiles and satisfies a smoke call path.
- The overlap report finds no duplicated concept or names the owner who absorbs it.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Ports/` (create if absent).

## 5. Risks
Speculative retention → the window and decision rule from Plan 140 apply.
Premature docks → explicitly out of scope; the realizer plan builds.

---

## 6. Expanded census (1 files · 135 lines)

Scope: `Assets/Ashfall.Core/Ports/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PortContract.cs` | 135 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `ammo_hoist_jam_reports.json` | array[8] |
| `aramid_fiber_rot_reports.json` | array[8] |
| `biochar_cation_exchange_reports.json` | array[7] |
| `bolting_silk_mesh_reports.json` | array[8] |
| `brain_tanning_hide_reports.json` | array[8] |
| `bullet_alloy_assay_reports.json` | array[7] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Ports/` (create if absent) |
| Test references | 2 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PCT-157A` | `PortContract.cs` |
| `PCT-157B` | no name match — resolve at claim time |
| `PCT-157C` | no name match — resolve at claim time |
| `PCT-157D` | no name match — resolve at claim time |
| `PCT-157E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 8. Host files: **0** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `grain_milling_archive` |
| `grain_processing` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--port-contract-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnHidePreserved` | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/grain_processing.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/ammo_hoist_jam_reports.json` |
| `Assets/StreamingAssets/Data/narrative/aramid_fiber_rot_reports.json` |
| `Assets/StreamingAssets/Data/narrative/bark_tanning_vat_logs.json` |
| `Assets/StreamingAssets/Data/narrative/biochar_cation_exchange_reports.json` |
| `Assets/StreamingAssets/Data/narrative/bolting_silk_mesh_reports.json` |
| `Assets/StreamingAssets/Data/narrative/brain_tanning_hide_reports.json` |
| `Assets/StreamingAssets/Data/narrative/bullet_alloy_assay_reports.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/chrome_alum_tanning_assays.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (24 files, 178 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `PlayerCommand` | 1 | 1 |
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 178 cases sit under matching regions — run those first (`Holdfast`, `Integration`, `NarrativeConsequence`, `PlayerCommand`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **16**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/GrainProcessingHostSession.cs` |
| `src/Host/PortContractSelfTest.cs` |
| `src/Host/WildlifeEcosystemHostSession.cs` |
| `src/Host/WildlifeEcosystemSaveStore.cs` |
| `src/Host/WildlifeTrappingHostSession.cs` |
| `src/Host/WildlifeTrappingSaveStore.cs` |
| `src/Main.DebtCredit.cs` |
| `src/UI/CaravanBarterLedgerPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **15**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `collectible_discovery` | no |
| `dose_ledger` | yes |
| `grain_milling_archive` | no |
| `grain_processing` | no |
| `holdfast_trade` | no |
| `hydrogeology_archive` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `black_market_debt_event` |
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **157**
(CODEX_ONLY 139, GAMEPLAY_CONSUMED 10, OPTIONAL 4, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `dose_locations.json` | GAMEPLAY_CONSUMED |
| `dose_quests.json` | GAMEPLAY_CONSUMED |
| `dose_registers.json` | GAMEPLAY_CONSUMED |
| `foundry_treaty_consequences.json` | GAMEPLAY_CONSUMED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 15 (laddered 1) · RNG streams 5 · host files 19 · catalogs 22 · test regions 5 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PORT-CONTRACT-TRUTH-157
wave: 12
status: PROPOSED — foreman claim required
packages: PCT-157A, PCT-157B, PCT-157C, PCT-157D, PCT-157E
claim paths:
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - src/Host/GrainMillingArchiveSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/discovery_consequences.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/grain_processing.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Holdfast/
  - godot --headless --path . -- --port-contract-selftest
dependencies:
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
