# Plan 215 — Crisis Rationing Overlay Completion — Full Host Integration

**STATUS: APPROVED BY USER**
**Authorized by:** user directive in session ("find another plan to fully integrate! and also mark as integrated!") — 2026-09-26.
**Claim:** `claim-plan215-rationing-overlay-completion-2026-09-26`
**Signed design authority:** `DEC-200` (Shelter Resource Rationing & Crisis Management System, SIGNED — Core SEALED) + the plan's 2026-09-24 integration revision ("Complete the already live rationing overlay").

## Premise verification (evidence-first)

- Census/candidates re-drained before selection: Plans 42, 46 (Batch 2, 2026-09-23), 135/136 (B8), 137/140 (B9), 141/145 (B10), 159 (DEC-320), 165/166 (DEC-321), 149, 181 (difficulty hosted), 151/155 (hosted) are all already integrated — census rows stale. Plan 217 (genealogy) rejected: `RomanceFamilySystem` (hosted, Plan 150) already owns family units, and Plan 191 requires an item-instance identity architecture. Orphan scan survivors are decision-blocked (Barter/ItemLore/Chronic/Diplomacy/Emergency) or retired (`ShelterPrisonerSystem` → `PrisonerSystem` single authority).
- Plan 215's own revision named the exact missing arrows: no src load of `rationing_protocols.json`; a restore-order/duplicate-restore premise in `Main.SetupEconomy`; no player `ApplyProtocol` route; panel readout distinguishing policy from stock.
- Verified live: the consumption loop was already closed (`InventoryHostSession.RationingAuthorizer` → `AuthorizeAllocation`, callers check `AllocatedUnits`); the rationing snapshot is nested in `MarketState.rationing` under the existing `economy` section (no new save section per DEC-200); `RestoreState` does not re-fire tier events.
- The lenient `LoadCatalog(string)` could never read the snake_case rows (case-insensitive camelCase DTO matching breaks on underscores), which is why the catalog was orphan data — a strict snake_case loader is required, mirroring `RetentionPolicyCatalogLoader`.

## Files changed

- **Core:** new `Assets/Ashfall.Core/Economy/RationingProtocolCatalogLoader.cs` (strict snake_case loader; schema_version, unique ids, known tiers, finite non-negative morale scale; errors clear the accepted set); additive read accessors on `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` (`RationTargets`, `EventCount`). No gameplay-decision changes: `ApplyProtocol`, `SetRationTier`, `AuthorizeAllocation`, capture/restore semantics untouched.
- **Host:** `src/Host/EconomyHostSession.cs` — `Create` loads the authored protocol table through the strict loader BEFORE the saved `ActiveProtocolId` is restored (optional file: missing/invalid → legacy path, never a hard failure); additive `ApplyRationingProtocol(protocolId, currentDay)` forwarder (the one lawful player route) + `RationingProtocols` read accessor + private `TargetedResourceIds()` (applies the protocol to existing ration targets only; invents no resource ids).
- **Main:** `src/Main.Economy.cs` — duplicate `_economy.Market.RestoreState(save)` removed (session `Create` already restores market + nested rationing; dirty-flag reset retained); `ApplyRationingProtocolCommand` (validates through the canonical owner, reports outcome, dirties the existing economy save); panel command binding.
- **UI:** `src/Economy/EconomyMarketPanel.cs` — RATIONING POLICY section: protocol OptionButton + one explicit APPLY button, active protocol line (named definition or honest "legacy, definition not loaded"), per-resource tier + multiplier rows, active-crisis count. Read-only on refresh; distinguishes policy from stock (stock stays with the inventory owner).
- **CLI:** `HostCliAction.RationingSelfTest` in both registries (`Assets/Ashfall.Core/HostCliRegistry.cs` + `src/Host/HostCli.cs`), parse/help lines, dispatch in `src/Main.Application.cs`, 12-check probe `src/Host/HostCli.Rationing.cs`.
- **Tests:** new `Ashfall.Core.Tests/Economy/Plan215RationingOverlayCompletionTests.cs` (8 tests).
- **Generated:** architecture map (economy node core list + loader; 272 subsystems), CLI catalog (277), selftest manifest (213), docs index.

## Acceptance (all verified)

1. Focused xUnit: new overlay tests + existing `Plan215ResourceRationingIntegrationTests` (6/6) + `ResourceRationingSystemTests` (8/8) — 22/22.
2. Adjacent gates: Plan14A, Plan212, Plan155, Plan213, Plan192, Plan92_99, RegionalSupplyPrice, SaveSectionRegistry, HostCliActionParityGate, HostCliHelpContract, MainTriadDriftGate, Plan218, Plan195 — 142/142 combined.
3. Headless Godot: `--rationing-selftest` 12/12; `--data-integrity-selftest` 427/427 (0 errors); `--player-panels-uitest` PASS; `--7-day-smoke-selftest` PASS.
4. Builds: Core, tests, host 0 errors.
5. Generators: architecture map `--check` OK (272; the concurrent clothing_warmth transient has since been cleared by that lane), CLI catalog, selftest manifest, docs index current.

## Explicit non-goals (per the plan revision)

- No separate rationing save section; the snapshot stays nested in `MarketState.rationing`.
- No automatic stock debiting: `AuthorizeAllocation` stays a bounded non-mutating decision; the consumer (Inventory) spends stock through its owner.
- No automatic crisis declaration day tick (no real producer exists for auto-declared crises; declaring one would fabricate state).
- No UI-side ration multiplier; the panel renders canonical policy only.
- No blanket rationing of other consumers (their authorization hooks are unaudited).
