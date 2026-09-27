# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-e-expansion21-expansion13-traumabond-xp08f6-2026-09-26`
> **Not committed** (per user direction). See §5 for the closeout evidence.


> **Package:** `EXPANSION-21-THE-GRID-LOAD-SHEDDING` (UNBLOCK Program Wave 10 item 3)
> **Category:** shelter / power
> **Plan type:** host integration of a sealed Core engine. Zero new gameplay authority.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

# PLAN-EXPANSION-21-THE-GRID — Microgrid Load Shedding Host Integration


## 1. Objective

`PowerLoadSheddingEngine` (Expansion 21 "The Grid") is sealed, tested, and
engine-free, but it has **no host seam**: nothing in `src/` reads it, so a
brownout on the live shelter grid never produces a shedding order, a cascading
trip risk, or the authored energy-poverty morale penalty. This package wires it
to the live canonical owners and nothing else.

**Bounded outcome:**

1. A `PowerLoadSheddingHostSession` derives its demand vector from the authored
   `power_subgrid_nodes.json` node definitions held by the live
   `PowerDistributionSubgridSystem` — no second node registry.
2. Available supply is read from the live `PowerGridSystem.AvailableSupplyWatts`
   and grid wear from the power-grid owner's own state — no second power ledger.
3. The engine's verdict (served/shed demand, brownout risk, cascading-trip risk,
   energy-poverty morale penalty, shed consumer list) is published once per
   canonical day as a derived read model.
4. The morale penalty is applied through the canonical morale owner
   (`NeedsSystem.Modify(..., NeedKind.Morale, ...)`) exactly once per canonical
   day per shelter — never as a local morale counter.
5. `--power-load-shedding-selftest` proves catalog binding, live-owner reads,
   priority ordering (Tier0 life support served last-shed), brownout/cascade
   monotonicity, blackout detection, and save-neutral read-model behaviour.

**Non-goals (hard boundaries):** no breaker mutation (opening a breaker is
`PowerDistributionSubgridSystem.SetBreaker`'s command and stays there); no second
power grid; no new save section (the engine is stateless — a save section would
be fabricated state); no RNG; no Unity.

---

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
|---|---|
| Engine is pure/static | `Assets/Ashfall.Core/Shelter/PowerLoadSheddingEngine.cs` — `PowerLoadSheddingEngine.Evaluate(availableGenerationKw, demands, gridWearPermille)` |
| Engine has **zero** host references | `grep -rn PowerLoadSheddingEngine src/` → no hits |
| Engine has 4 passing Core tests | `Ashfall.Core.Tests/Shelter/PowerLoadSheddingEngineTests.cs` |
| Node catalog is authored and bound to the subgrid owner | `power_subgrid_nodes.json` → `PowerSubgridCatalogLoader.Load` → `PowerDistributionSubgridSystem` (`src/Main.AdvancedShelterSystems.cs:155 EnsurePowerSubgrids`) |
| Live supply authority exists | `PowerGridSystem.AvailableSupplyWatts`, `TotalDrawWatts`, `DeficitWatts`, `IsBrownout` |
| Morale authority exists | `NeedsSystem.Modify(string survivorId, NeedKind need, float delta)` |
| Read-model precedent | `NeedsPerformanceHostSession` (pure projection allowlisted in `MainTriadDriftGateTests` Rule 5) |

---

## 3. Files

**New:** `src/Host/PowerLoadSheddingHostSession.cs`,
`src/Host/HostCli.PowerLoadShedding.cs`, `src/Main.PowerLoadShedding.cs`,
`Ashfall.Core.Tests/Shelter/PlanExpansion21PowerLoadSheddingHostIntegrationTests.cs`

**Edited:** `Assets/Ashfall.Core/HostCliRegistry.cs` (enum + descriptor),
`src/Host/HostCli.cs` (alias resolution), `src/Main.Application.cs` (dispatch),
`src/Main.ExpandedShelterSystems.cs` (setup/tick/reset registration).

---

## 4. Acceptance

Host build 0 errors / 0 warnings; `--power-load-shedding-selftest` green;
`MainTriadDriftGateTests` still green (read-model disposition documented);
focused `Shelter` test file green.


---

## 5. Closeout evidence (2026-09-26)

The Core authority named in this plan is now bound to the live canonical owners
and verified headless. Exact commands and results:

1. **Host seam:** `src/Host/PowerLoadSheddingHostSession.cs` derives the demand
   vector from the authored `power_subgrid_nodes.json` node definitions held by the
   live `PowerDistributionSubgridSystem` (breaker-closed, non-blown nodes only),
   reads supply from `PowerGridSystem.AvailableSupplyWatts`, and projects grid wear
   from the canonical `PowerGridSystem.GeneratorCondition`. Priority tiers come from
   the node's own `is_critical` flag with the live `EffectivePriority` room mapping.
2. **Exactly one authority per concern:** no breaker mutation (that stays
   `PowerDistributionSubgridSystem.SetBreaker`), no second power ledger, no new save
   section — the engine is stateless, so a section would be fabricated state. The
   energy-poverty morale penalty routes into the canonical morale owner
   (`NeedsSystem.Modify(..., NeedKind.Morale, ...)`) exactly once per canonical day
   through `Main.PowerLoadShedding.cs`.
3. **CLI probe:** `--power-load-shedding-selftest` (**12/12 PASS** headless) covering
   owner binding, the 12-node live demand vector, priority shedding order, blackout
   honesty, brownout/cascade monotonicity, grid-wear compounding, bounded morale
   mapping, and read-model behaviour.
4. **Tests:** `PlanExpansion21PowerLoadSheddingHostIntegrationTests` **6/6 PASS**;
   pre-existing `PowerLoadSheddingEngineTests` still green.
5. **Lifecycle:** phase-5 day owner `power_load_shedding` (deliberate no-op pre-day
   snapshot — nothing to roll back), setup on both campaign paths, lifecycle reset.
6. **Generated artifacts:** `docs/ci/SELFTEST_MANIFEST.json` (257 tests),
   `docs/cli/HOST_CLI_COMMAND_CATALOG.md` (313 entries),
   `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `docs/architecture/PORT_CONTRACT.md`,
   and the `ARCHITECTURE_GRAPH` entry for `power_load_shedding`.

**Deferred with named reasons:** no UI panel for the shedding order (presentation
follow-on); the `thermal_load` / `filtration_stress` / `power_output` shelter legs
stay reported-only because they already reach the shelter through the canonical
weather path and applying them again would be a second drain.


---


---
