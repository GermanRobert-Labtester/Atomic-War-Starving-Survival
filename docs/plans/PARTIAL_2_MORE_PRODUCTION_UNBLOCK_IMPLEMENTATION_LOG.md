# Partial-plan production unblock implementation log — two more plans

Date: 2026-09-19
Authority: direct user request following `PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md`

## Bounded outcome

Implement the next two ranked partial plans through their existing authorities:

1. Plan 215 / C2[44] — Resource Rationing.
2. Plan 183 / D1[13] — Child Development.

Non-goals: no duplicate stock, needs, water, power, cohort, lifecycle, or save
authority; no new JSON catalog; no Unity restoration; no changes to the live
foreman ledgers; no speculative UI panel.

## Authority decisions and files

### Plan 215 — Resource Rationing

- `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs`
- `Assets/Ashfall.Core/Economy/MarketSystem.cs`
- `src/Host/EconomyHostSession.cs`
- `src/Host/InventoryHostSession.cs`
- `src/Main.Economy.cs`
- `src/Main.Inventory.cs`
- `Ashfall.Core.Tests/Economy/ResourceRationingSystemTests.cs`

Rationing is now a policy/protocol owner only. It validates resource IDs when
the host binds the canonical inventory/goods/category catalog, returns a
demand-and-stock-bounded authorization decision, and never mutates inventory,
needs, morale, water, or power. The existing inventory consumer remains the
sole mutator; its consume route calls the economy policy seam when present.

The policy is carried additively in the existing `MarketState`/economy save
envelope. Old saves restore a neutral empty policy, and validator filtering is
applied both at bind time and on later restores.

### Plan 183 — Child Development

- `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs`
- `Assets/Ashfall.Core/Survivors/GenerationalSystem.cs`
- `src/Main.Plans178_181.cs`
- `Ashfall.Core.Tests/Survivors/ChildDevelopmentSystemTests.cs`

`GenerationalSystem` remains the only owner of birth day, growth, caregiver,
education, and adulthood transition state. Plan 183 now exposes a detached
read projection from that record, derives stage from canonical age, emits
forward stage facts after the persisted growth day is established, and routes
the young-adult edge through the existing guarded `ProcessAdulthood` handoff.
Clock rewind is ignored and repeated ticks/save restore do not replay the
handoff.

## Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/ResourceRationingSystemTests.cs` — **8/8 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/ChildDevelopmentSystemTests.cs` — **17/17 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/GenerationalSystemTests.cs` — **6/6 passed**.
- `dotnet build Ashfall.csproj --no-restore -p:BuildInParallel=false -v:minimal` — **0 warnings, 0 errors**.
- `godot --headless --log-file /tmp/ashfall-plan215-economy.log --path . -- --economy-selftest` — **13/13 passed**.
- `godot --headless --log-file /tmp/ashfall-plan215-data-integrity.log --path . -- --data-integrity-selftest` — **337/337 catalogs passed**, 0 errors and 5 documented primary-wins warnings.

## Remaining partial scope

Plan 215 is unblocked for policy and an existing inventory consume seam, but
autonomous kitchen/water/medical/power day loops still need their own typed
consumer adapters before rationing can be called flagship-complete. Crisis
threshold authoring and a dedicated panel remain follow-up work.

Plan 183 is unblocked for canonical age projection and the generational adult
handoff. The legacy `ChildDevelopmentSystem` state remains compatibility-only;
caregiver UI, education/skill effects, and any future catalog-authored stages
must be integrated through the current generational/lifecycle owners rather
than restored as a second child save section.

Unrelated dirty and untracked worktree changes were preserved. `INTEGRATION_PLANS.md`,
`WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, and `KNOWN_DEBT.md` were not edited.
