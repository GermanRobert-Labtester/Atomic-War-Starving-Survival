# UNBLOCK-OLDEST-BATCH5-PLANS — Plans 55 + 58 Host Integration Plan (READY)

**Role:** implementation-readiness plan produced for the foreman/user.
**Status:** READY — not executed, **not claimed**. This document authorises no
edit. It is the premise audit, authority map, file list, acceptance and
verification package for the fifth batch of the oldest-partial queue, per
`docs/plans/OLDEST_PARTIAL_PLANS_AUDIT_20_2026-09-23.md` and
`INTEGRATION_PLANS.md` ("Next batch (oldest remaining partials): Plans 55, 58").

**Date:** 2026-09-23 · **Queue:** `UNBLOCK-OLDEST-BATCH5-PLANS` · **Plans:**
55 (The Long Haul — retention/save budgeting) and 58 (The Continuation —
outposts/waystations/second holdfast).

This plan obeys the established batch-1…4 shape: host-integrate the *existing*
Core authority with canonical owners and a registered save section. It does
**not** re-implement the full `Plan_55`/`Plan_58` source documents.

---

## 1. Premise audit (current source, 2026-09-23)

| Claim | Current evidence | Verdict |
|---|---|---|
| Plan 55 Core authority exists | `Assets/Ashfall.Core/Records/RetentionPolicy.cs` — `RetentionAction`, `RetentionPolicyDefinition`, `RollingLog<T>`, `RetentionAuditReport`, `RetentionPolicyCatalog` (5 growing + 4 protected policies hardcoded in `RegisterDefaultPolicies`) | **TRUE** |
| Plan 55 is host-unreachable | `grep -rl RetentionPolicy src/` → 0; `grep -rl RollingLog src/` → 0 | **TRUE** |
| Plan 55 focused tests exist | `Ashfall.Core.Tests/Records/Plan55RetentionPolicyIntegrationTests.cs` | **TRUE** |
| Plan 58 Core authority exists | `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` — `OutpostDef`, `OutpostInstance`, lifecycle `EstablishOutpost`/`AssignGarrison`/`RelieveGarrison`/`SupplyOutpost`/`TickDay`/`SimulateRisk`/`AbandonOutpost`; 6 delegate seams; `FromJson` | **TRUE** |
| Plan 58 is host-unreachable | `grep -rl OutpostSettlement src/` → 0 | **TRUE** |
| Plan 58 data exists | `Assets/StreamingAssets/Data/outposts.json` — 4 authored outposts (`outpost_north_watch`, `outpost_rail_depot`, `outpost_relay_tower`, `outpost_quarry_camp`) | **TRUE** |
| Plan 58 focused tests exist | `Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs` | **TRUE** |
| Plan 58 persistence | `OutpostSettlementSystem.cs` has **no** `CaptureState`/`RestoreState` and **no** state DTO | **FALSE → real blocker to seal in-batch** |
| Plan 58 custody | `WaystationSystem` owns save section `waystation` (`SaveSectionRegistry.cs:113`); `ColonySystem` owns `colony`; `SettlementCatalog` owns the world-settlement catalog. `ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` §2 rules them distinct and defers `OutpostSettlementSystem` on the missing state DTO | **decision required** (see §3) |

**Conclusion.** Plans 55 and 58 are the correct queue head. Plan 55 is additive
and low-risk. Plan 58 is a genuine High-difficulty plan whose first slice
(`CaptureState`/`RestoreState` + custody) is the bounded in-batch blocker. The
batch must therefore seal Plan 58's state DTO and custody decision, not defer
it again.

---

## 2. Authority boundaries (one authority per concern)

| Concern | Sole owner | Batch action |
|---|---|---|
| Retention policy + rolling cap | `RetentionPolicyCatalog` / `RollingLog<T>` (Core) | bind to an authored policy catalog; **apply only**, never move log ownership |
| Kitchen serving log | `KitchenNutritionSystem` (`_state.servingLog`) | expose a policy-apply seam; do not duplicate the list |
| Faction-war decrees | `FactionWarSystem` (`_state.enactedDecrees`) | expose a policy-apply seam |
| Journal entries | `JournalSystem` | expose a policy-apply seam (bounded view over volume-controlled entries only) |
| Machine log | `MachineLogSystem` (`_state.entries`) | expose a policy-apply seam |
| Dose ledger | `DoseLedgerSystem` | expose a policy-apply seam |
| Authored outposts | `OutpostSettlementSystem` (Plan 58) | add `OutpostSettlementState` DTO + `CaptureState`/`RestoreState`; keep sole authority |
| Holdfast S2 waystation | `WaystationSystem` (`waystation` section) | **unchanged** — not merged |
| Player-founded colonies | `ColonySystem` (`colony` section, ORPHAN W1) | **unchanged** — not merged |
| World settlement catalog | `SettlementCatalog` (World) | **unchanged** — read-only graph/bunk reference for outposts |
| Garrison roster | `DutyRosterSystem` / survivor roster | read-only selection; outposts store only survivor ids |
| Rations / build cost | canonical inventory (`Inventory`) | consumed through a `Func<string,int,bool>` cost consumer and a ration provider; **no** second stock |

**Plan 58 custody decision (to be signed by the foreman as the batch premise):**
`OutpostSettlementSystem` is the single authority for **authored** outposts and
secondary positions. `WaystationSystem`, `ColonySystem`, and `SettlementCatalog`
remain separate, already-wired authorities and are **not** merged. The outpost
section stores only outpost state (establishment, condition, garrison **ids**,
supply reserve, starvation/overrun flags); population, food, inventory and
settlement catalog stay with their canonical owners.

---

## 3. Ready-to-paste claim (`WORKTREE_OWNERSHIP.md`)

```text
| claim-unblock-oldest-batch5-plans-2026-09-23 | `UNBLOCK-OLDEST-BATCH5-PLANS` | Integrator (user-authorized 2026-09-23) | **Data:** `Assets/StreamingAssets/Data/retention_policies.json` (new); **Core:** `Assets/Ashfall.Core/Records/RetentionPolicyCatalogLoader.cs` (new strict loader), `Assets/Ashfall.Core/Records/RetentionPolicy.cs` (loader bind accessor), `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` (`OutpostSettlementState` DTO + `CaptureState`/`RestoreState`), `Assets/Ashfall.Core/KitchenNutritionSystem.cs`, `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`, `Assets/Ashfall.Core/Journal/JournalSystem.cs`, `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`, `Assets/Ashfall.Core/Dose/DoseLedgerSystem.cs` (additive `ApplyRetention` seams only), `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (new `retention` + `outpost_settlement` sections and filenames), `Assets/Ashfall.Core/HostCliRegistry.cs` (two CLI descriptors); **Host:** `src/Host/RetentionHostSession.cs` (new), `src/Host/OutpostSettlementHostSession.cs` (new), `src/Host/RetentionSaveStore.cs` (new), `src/Host/OutpostSettlementSaveStore.cs` (new), `src/Host/HostCli.Retention.cs` (new), `src/Host/HostCli.OutpostSettlement.cs` (new), `src/Host/HostCli.cs`, `src/Main.Retention.cs` (new), `src/Main.OutpostSettlement.cs` (new), `src/Main.ExpandedShelterSystems.cs`, `src/Main.CampaignOwners.cs` (retention + outpost day owners); **Tests:** `Ashfall.Core.Tests/Campaign/Plan55RetentionHostIntegrationTests.cs` (new), `Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs` (new), `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (section-count pin 220→222); **Generated:** `scripts/ci/generate-architecture-map.py` (section graph entries), `docs/architecture/ARCHITECTURE_TEST_MAP.md`, `docs/architecture/port-contract.json`, `docs/architecture/PORT_CONTRACT.md`, `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `docs/ci/SELFTEST_MANIFEST.json`, `docs/INDEX.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md` | READY — see `docs/plans/UNBLOCK_OLDEST_BATCH5_PLANS_55_58_INTEGRATION_PLAN.md`. |
```

---

## 4. Package A — Plan 55 (retention host integration)

**Bounded outcome:** the existing `RetentionPolicyCatalog` becomes the live
retention authority for the canonical unbounded campaign logs, driven by an
authored policy table, applied on the canonical day tick, with an auditable
`retention` save section and a focused CLI probe. No log ownership moves.

**Core deltas (minimal, additive):**
1. `Assets/StreamingAssets/Data/retention_policies.json` — authored
   `collection_key`, `max_capacity`, `action` (`keep_newest_k` |
   `rollup_summary` | `drop_prose_keep_ids`), `protected_obligation`.
   Snake_case, `schema_version`.
2. `RetentionPolicyCatalogLoader` — strict loader in the established style
   (collected errors, rejects bad keys/actions/capacities; a protected
   obligation may not be authored with a finite cap).
3. `RetentionPolicyCatalog` — add a bind/overlay method that merges authored
   rows over the built-in defaults (authored row wins; defaults preserved for
   unlisted collections). Add an accessor to enumerate active policies for the
   audit report.
4. Owner apply seams (one per owner, no new store):
   - `KitchenNutritionSystem.ApplyRetention(RetentionPolicyCatalog)`
   - `FactionWarSystem.ApplyRetention(RetentionPolicyCatalog)`
   - `JournalSystem.ApplyRetention(RetentionPolicyCatalog)`
   - `MachineLogSystem.ApplyRetention(RetentionPolicyCatalog)`
   - `DoseLedgerSystem.ApplyRetention(RetentionPolicyCatalog)`
   Each calls `catalog.ApplyRetention(key, list, out pruned)` on its own list and
   returns the pruned count. Protected keys are no-ops.

**Host deltas:**
5. `RetentionHostSession` (`src/Host/`) — constructs the catalog, loads the
   authored table (missing/invalid → built-in defaults, never hard-fail), holds
   the canonical owner references, exposes `TickDay()` that applies every owner
   seam and aggregates a `RetentionAuditReport`.
6. `RetentionSaveStore` + `retention` section (owner `records`): persists the
   **audit report only** (totals + pruned/protected keys), not the log contents
   (those remain in their own sections). Registration in `SaveSectionRegistry`
   plus filename map.
7. `Main.Retention.cs` — `SetupRetention()` / `SaveRetention()` campaign twins;
   day owner `retention` (phase 5, after the owners that append) in
   `Main.CampaignOwners.cs`; journal fact `retention_applied` when pruned > 0
   and once per campaign a `retention_audit` summary.
8. CLI probe `--retention-selftest` + `HostCli.Retention.cs`.

**Acceptance:** authored table loaded and overlaid; protected obligations
(`survivor_wills_and_legacies`, `memorial_monuments`, `campaign_deadlines`,
`survivor_grief_markers`) never shrink across a 400-day simulated tick; each
canonical list respects its cap; the audit section round-trips; the CLI probe
passes.

---

## 5. Package B — Plan 58 (outpost settlement host integration)

**Bounded outcome:** the authored `outposts.json` becomes a live, persisted,
host-driven outpost loop: establish (consume canonical build cost), garrison
(from the roster, bunk-capped), supply (draw canonical rations), daily consume /
starve / threat / overrun / abandon — with exactly one new save section and no
second population, food, inventory, or settlement authority.

**Core deltas (minimal, additive):**
1. `OutpostSettlementState` DTO + `CaptureState()` / `RestoreState(state)` on
   `OutpostSettlementSystem`. The DTO carries: definitions are re-derived from
   the catalog; per-instance `outpost_id`, `graph_node_id`, `is_established`,
   `condition_permille`, `garrison_survivor_ids`, `days_since_supply`,
   `is_starving`, `is_overrun`, `ration_reserve`. Legacy/absent state → all
   outposts unestablished (deterministic, never partially invented).
2. Guard `RestoreState` against an unknown `outpost_id` (drop, do not create a
   phantom instance) and against a garrison id list exceeding the authored bunk
   cap (truncate deterministically). No RNG in capture/restore.

**Host deltas:**
3. `OutpostSettlementHostSession` (`src/Host/`) — binds `outposts.json` via
   `FromJson`; provides owner-backed delegates:
   - build-cost consumer → canonical `Inventory` (`ConsumeById`);
   - garrison fitness check → canonical fitness/duty read model;
   - central ration provider → canonical food/ration inventory;
   - risk RNG → a **forked** campaign stream (`outpost_risk`), never
     `System.Random`.
4. `OutpostSettlementSaveStore` + `outpost_settlement` section (owner
   `expedition`): `CaptureState`/`RestoreState` round-trip.
5. `Main.OutpostSettlement.cs` — `SetupOutpostSettlement()` /
   `SaveOutpostSettlement()` twins; day owner `outpost_settlement` (phase 4/5,
   after roster/economy) in `Main.CampaignOwners.cs`; seam routing:
   - `OnOutpostEstablishedSeam` → journal `outpost_established`;
   - `OnOutpostSuppliedSeam` → journal `outpost_supplied`;
   - `OnOutpostStarvingSeam` → journal `outpost_starving`;
   - `OnOutpostOverrunSeam` → journal + `CampaignConsequenceLedger`;
   - `OnGarrisonAssignedSeam`/`OnGarrisonRelievedSeam` → journal facts.
6. CLI probe `--outpost-settlement-selftest` + `HostCli.OutpostSettlement.cs`.
7. Surface note: ship headless-first (journal + CLI). A panel is a separate,
   smaller package, consistent with the ORPHAN W1 precedent.

**Acceptance:** all 4 authored outposts load; establish consumes the authored
build cost through canonical inventory; garrison respects the bunk cap and
fitness gate; supply draws and consumes canonical rations; 30-day seeded tick is
deterministic and `continuous == save/restore` field-by-field; overrun fires at
most once; abandon clears garrison and reserve; the section round-trips; the CLI
probe passes; no new population/food/inventory/settlement store is created.

---

## 6. Slice order (serialised, one builder)

1. **B5-A1** Plan 55 Core: authored catalog + loader + catalog overlay.
2. **B5-A2** Plan 55 owner seams + host session + save section + CLI.
3. **B5-B1** Plan 58 Core: `OutpostSettlementState` + capture/restore + tests.
4. **B5-B2** Plan 58 host session + save section + day owner + CLI.
5. **B5-B3** Joint generated-artifact regeneration + gate sweep + ledger update.

Plan 58 must not start before B5-A1/B5-A2 land if the two packages share the
save-section-count pin; otherwise the two plan files are path-disjoint.

---

## 7. Verification (focused; TEST_POLICY.md)

```bash
# builds
dotnet build Ashfall.csproj
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# Core (existing + new)
bash scripts/run_test.sh Ashfall.Core.Tests/Records/Plan55RetentionPolicyIntegrationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plan55RetentionHostIntegrationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs

# host probes
godot --headless --path . -- --retention-selftest
godot --headless --path . -- --outpost-settlement-selftest
godot --headless --path . -- --real-campaign-journey-selftest
godot --headless --path . -- --7-day-smoke-selftest

# gates
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PersistentFilenameRegistryGateTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/SaveSectionRegistryTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs
python3 scripts/ci/generate-port-contract.py --check
python3 scripts/ci/generate-docs-index.py --check
```

Expected gate deltas: section count 220 → 222; two new `--*-selftest` verbs in
`docs/ci/SELFTEST_MANIFEST.json`; two new architecture-map section edges;
port-contract seam count increases by the number of newly bound seams.

---

## 8. Risks / blockers

| Item | Disposition |
|---|---|
| Plan 58 has no state DTO | **Sealed in-batch** (B5-B1). This is the blocker ORPHAN W1 named; the batch owns it. |
| Plan 58 custody vs `WaystationSystem`/`ColonySystem`/`SettlementCatalog` | **Requires the foreman's one-line signature** on §2 before B5-B2 edits. Recommended: distinct authority, no merge. |
| Plan 55 mutating Core log lists | Mitigated by owner-local `ApplyRetention`, protected-obligation no-op, and a 400-day test that asserts protected keys never shrink. |
| Save-section-count pin shared with other active packages | Pin update (220→222) is integrator-owned; coordinate with any concurrent claim touching `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`. |
| Concurrent dirty worktree | Batch touches only its claimed paths; no mass-format, no shared-path rewrite except the documented generated artifacts and the ledger owned by the integrator. |
| Plan 58 is High difficulty / 3 sub-slices | The batch scopes only the host integration of the existing authority; 58B content and 58C second-holdfast economics remain follow-on. |

---

## 9. Handoff / ledger edits (foreman-owned)

On authorization, the integrator makes exactly these live-ledger changes:
1. Add the §3 claim block to `WORKTREE_OWNERSHIP.md`.
2. Prepend the `UNBLOCK-OLDEST-BATCH5-PLANS` completion entry to
   `INTEGRATION_PLANS.md` with evidence, and update its "Next batch" line.
3. Record the Plan 58 custody decision in
   `docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` §2 (supersede the
   deferral) once B5-B1/B5-B2 land.

**No production code, data, test, or ledger is modified by this readiness
document.**
