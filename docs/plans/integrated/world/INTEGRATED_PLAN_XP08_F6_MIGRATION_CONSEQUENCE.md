# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-e-expansion21-expansion13-traumabond-xp08f6-2026-09-26`
> **Not committed** (per user direction). See §5 for the closeout evidence.
>
> **Independent seal-sweep verification (2026-09-26, no-commit seal session):** `MigrationConsequenceEngine` is fully wired over the LIVE hosted `SeasonalHumanMigrationEngine`: consequences routed through the canonical market owner's idempotent shock seam, `PlanXp08F6MigrationConsequenceHostIntegrationTests` 9/9 and `--migration-consequence-selftest` 11/11 verified 2026-09-26. See `INTEGRATION_PLANS.md` for the full evidence trail.

# PLAN-XP-08-F6-MIGRATION-CONSEQUENCE — Seasonal Migration Consequence Host Integration

> **Package:** `XP-08-F6-MIGRATION-CONSEQUENCE` (UNBLOCK Program Wave 16 item 1)
> **Category:** economy / world
> **Plan type:** host integration of a sealed Core engine over the already-hosted Plan 199 migration owner. No second migration engine.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

`MigrationConsequenceEngine` is sealed and tested and its input authority
(`SeasonalHumanMigrationEngine`, Plan 199) is **already hosted and persisted**,
but the consequence engine itself has **zero host references**. Regional
population movement therefore never moves market demand, labour supply,
territorial friction, or caravan demand.

**Bounded outcome:**

1. `MigrationConsequenceHostSession` is constructed over the **live hosted**
   `SeasonalHumanMigrationEngine` instance — the same object the campaign ticks,
   not a second migration engine.
2. It persists its exactly-once consequence ledger through its own bounded
   `migration_consequence` checksummed save section.
3. Regional market consequences reach the canonical market owner through
   `MarketSystem.ApplyShock` with a source id of
   `migration_<regionId>_<phase>` so the owner's per-source idempotence makes the
   consequence exactly-once across save/load. No price table is copied.
4. Labour-pool, territorial-friction, and caravan-demand projections are exposed
   as a derived read model for the economy and world day reports.
5. `--migration-consequence-selftest` proves shared-instance binding, multiplier
   monotonicity against population weight, exactly-once phase consequences across
   a save/load round-trip, and a save round-trip of the ledger.

**Non-goals (hard boundaries):** no second migration engine; no direct price or
ledger mutation outside `MarketSystem`; no new caravan scheduler (caravan demand
is a projection consumed by the caravan owner); no RNG; no Unity.

---

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
|---|---|
| Consequence engine is sealed with save state | `Assets/Ashfall.Core/Economy/MigrationConsequenceEngine.cs` — `GetMarketDemandMultiplierPermille`, `GetLaborPoolSizeMultiplierPermille`, `GetTerritorialFrictionMultiplierPermille`, `GetCaravanDemandPriority`, `TryApplyPhaseConsequence`, `CaptureState`, `RestoreState` |
| Consequence engine has **zero** host references | `grep -rn MigrationConsequenceEngine src/` → no hits |
| Consequence engine has 5 passing Core tests | `Ashfall.Core.Tests/Economy/MigrationConsequenceEngineTests.cs` |
| Input authority is hosted and persisted | `SeasonalHumanMigrationEngine` via `src/Host/HumanMigrationHostSession.cs`, section `human_migration` |
| Market owner has an idempotent shock seam | `MarketSystem.ApplyShock(...)` (already used source-named by the weather cascade) |
| Labour/friction consumers exist | recruitment pool, territorial friction ledger |

---

## 3. Files

**New:** `src/Host/MigrationConsequenceHostSession.cs`,
`src/Host/MigrationConsequenceSaveStore.cs`,
`src/Host/HostCli.MigrationConsequence.cs`, `src/Main.MigrationConsequence.cs`,
`Ashfall.Core.Tests/Economy/PlanXp08F6MigrationConsequenceHostIntegrationTests.cs`

**Edited:** `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
(`migration_consequence` section + filename, section-count pin bump),
`Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Host/HostCli.cs`,
`src/Main.Application.cs`, `src/Main.CampaignOwners.cs` (phase-5 day owner with
pre-day snapshot rollback), `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`,
`src/Main.HumanMigration.cs` (share the live engine instance),
`docs/architecture/ARCHITECTURE_TEST_MAP.md`.

---

## 4. Acceptance

Host + Core test builds 0 errors / 0 warnings;
`--migration-consequence-selftest` green;
`--human-migration-selftest` still green (shared-instance regression);
section-count pin updated with a ledger comment.


---

## 5. Closeout evidence (2026-09-26)

The Core authority named in this plan is now bound to the live canonical owners
and verified headless. Exact commands and results:

1. **Host seam:** `src/Host/MigrationConsequenceHostSession.cs` +
   `MigrationConsequenceSaveStore` (section `migration_consequence`,
   `migration_consequence_save.json`) are constructed over the **live hosted**
   `SeasonalHumanMigrationEngine` the campaign already ticks. `MigrationConsequenceEngine`
   gained two additive read accessors (`GetRegionPopulationWeight`,
   `MigrationEngine`) so the host can surface the raw weight and prove it is bound to
   the same instance instead of back-computing or building a second engine.
2. **Exactly one authority per concern:** the market leg routes through the canonical
   `MarketSystem.ApplyShock` seam with the source id
   `migration_<regionId>_<phase>`, so the market owner's own clamp and per-source
   idempotence make the consequence exactly-once across save/load. No direct price or
   ledger mutation, no second caravan scheduler (caravan demand is a projection the
   caravan owner consumes).
3. **CLI probe:** `--migration-consequence-selftest` (**11/11 PASS** headless) covering
   shared-instance binding (`ReferenceEquals` against the hosted session's engine),
   food demand tracking population weight (110 -> 1100 permille), engine-owned category
   differentiation (food 1100 / luxury 1050 / labor 950), bounded labour-pool and
   friction multipliers, authored caravan-priority bands, exactly-once phase
   consequences, ledger survival across a save/load round-trip, region/phase-scoped
   market source id, all-region projection, and a reset that leaves the migration owner
   untouched.
4. **Tests:** `PlanXp08F6MigrationConsequenceHostIntegrationTests` **9/9 PASS**;
   pre-existing `MigrationConsequenceEngineTests` still green.
5. **Lifecycle:** registered save section `migration_consequence` (part of the 302 ->
   305 pin bump), phase-5 day owner with `IPreDaySnapshotRestore` pre-day rollback,
   setup on both campaign paths (re-binds to the live migration instance, carrying the
   persisted ledger across the rebind), save mirror, lifecycle reset.
6. **Generated artifacts:** selftest manifest, CLI catalog, save-store matrix, port
   contract, and the `ARCHITECTURE_GRAPH` entry for `migration_consequence`.

**Deferred with named reasons:** labour-pool and territorial-friction projections are
exposed as a derived read model only — no recruitment or territory consumer is bound in
this package, because those owners have their own acceptance criteria. The caravan
demand priority is likewise a projection, not a scheduler command.
