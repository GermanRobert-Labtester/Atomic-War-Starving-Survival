# Bug Hunt Fifth Pass — Deferred Seal Handoff

**Date:** 2026-09-12
**Scope:** Implement every item deferred from BUG_HUNT_28 that the user ordered sealed: ChemWarfare toxic exposure, SaltMine hub persistence, dual Greenhouse unify, StandingRecord/PerimeterDefense deep clones, atlas/caravan/expedition UI wires.
**Authority checked:** `WORKTREE_OWNERSHIP.md` — no active claims.
**Still deferred (prior approval / design):** Plans 146–149 OPEN-only; DEBT-PLAN167/168; theater fake-feedback panels.

---

## Strategy

1. Confirm no ownership races; reuse settled recipes from explore agents.
2. Seal Core save clones first (StandingRecord + PerimeterDefense).
3. Extend ExpansionHubSave to v6 for SaltMine; merge/restore on host foundry path.
4. Bind player `GreenhouseHostSession.System` into expansion hub; remove dual day-tick.
5. Wire ChemWarfare: subscribe health consumer + call `EvaluateActorExposure` from combat `ActionEndTurn`.
6. Wire atlas selection, caravan sidebar (local section focus), expedition radar (open planner, no auto-dispatch).
7. Focused builds + filtered xUnit only.

---

## Bugs fixed

| # | Bug | Fix |
|---|-----|-----|
| 1 | StandingRecord `CaptureState`/`RestoreState` aliased live `State` | Fresh envelope Capture; Restore into new envelope then child Restore |
| 2 | PerimeterDefense sectors/`emplacement_ids`/intrusion_log shallow | Deep-clone sectors + intrusion log on Capture/Restore |
| 3 | SaltMine Capture/Restore unused; no hub field | Hub `CurrentSaveVersion=6` + `saltMine`; frozen V5; Capture/Decode/Restore; Main merge on save; SetupSilentFoundry restore |
| 4 | Dual Greenhouse authorities both day-ticked | `BindGreenhouse` share; day owner ticks player only; World plant/tick retargeted to `_greenhouse` |
| 5 | ChemWarfare `OnToxicExposureResolved` never subscribed | Subscribe in `EnsureChemWarfare` → journal + combat HP / `DamageSurvivor` / Needs.Health |
| 6 | `EvaluateActorExposure` never called in production | `CombatHostSession.ActionEndTurn` evaluates living combatants; mask wear via `RecordWear` |
| 7 | Atlas panels selection events unwired | Map→map detail; Muster→faction detail; Quests→quest detail; Research→open research; StandingRecordAtlas→faction detail (fixture ids) |
| 8 | Caravan sidebar treated section ids as factions | Local `FocusLedgerSection`; only real faction ids hit `SetActiveFaction` / `OnSetActiveFaction` |
| 9 | Expedition radar `OnDispatchRequested` unwired | Opens expeditions planner with status line; no auto-`StartExpedition` on row select |

---

## Files touched

**Core:**
`Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs`
`Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs`
`Assets/Ashfall.Core/ExpansionHubSave.cs`
`src/Host/ExpansionHostSession.cs` (BindGreenhouse — prior mid-edit)

**Host / Main / UI:**
`src/Main.ExpansionHub.cs`
`src/Main.World.cs`
`src/Main.Economy.cs`
`src/Main.Plans198_201.cs`
`src/Main.Expeditions.cs`
`src/Main.CampaignOwners.cs` (dual TickGreenhouse already removed)
`src/Host/CombatHostSession.cs`
`src/Main.UiPanels.cs`
`src/UI/CaravanBarterLedgerPanel.cs`
`src/Economy/TradeScreenGodotPanel.cs`
`src/Main.UiTests.Plans198_201.cs`

**Tests:**
`Ashfall.Core.Tests/StandingRecordEngineTests.cs`
`Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs`
`Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs` (v5→v6 + SaltMine round-trip)

---

## Contracts

### ChemWarfare
- **Call site:** `CombatHostSession.ActionEndTurn` → `EvaluateToxicExposureAfterTurn` for each living combatant.
- **Mask:** player `EquipSlot.Face` gas_mask durability fraction; hostiles evaluated unmasked.
- **Consumer:** `OnToxicExposureResolved` → journal + `8 * severity` HP via combatant Health + `DamageSurvivor`, else `Needs.Modify(..., NeedKind.Health, -hp)`.
- **Non-goals:** DoseLedger / radiation / disease routing.

### SaltMine
- Hub envelope field `saltMine` (v6).
- Capture merge: `SaveExpansionHub` writes `_silentFoundry.SaltMine.CaptureState()`.
- Restore: `SetupSilentFoundry` reloads hub payload into `SaltMine.RestoreState`.
- v1–v5 migrate with empty salt mine.

### Greenhouse
- Single growth authority: `GreenhouseHostSession.System`.
- Hub holds the same reference via `BindGreenhouse` after either setup order.
- Day owner and World demo buttons tick/plant the player session only.

### UI
- Atlas selection opens existing detail/open destinations.
- Caravan sidebar section ids never become faction ids.
- Radar dispatch opens expeditions planner (safe on select-fire).

---

## Verification

| Check | Result |
|-------|--------|
| `dotnet build Ashfall.Core/Ashfall.Core.csproj` | 0 errors, 0 warnings |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Focused xUnit | **61 passed**, 0 failed |

```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj -v q
dotnet build Ashfall.csproj -v q
bash scripts/run_test.sh Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs          # 4
bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecordEngineTests.cs        # 10
bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs  # 16
bash scripts/run_test.sh Ashfall.Core.Tests/ApicultureAndSaltExpansionTests.cs  # 3
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs    # 12
bash scripts/run_test.sh Ashfall.Core.Tests/DiseaseSystemTests.cs               # 16 (hub version regression)
```

Godot headless `--plans198-201-uitest` not run this pass (host UI contract extended statically; Core exposure cases green).

---

## Limitations / intentionally untouched

- Expedition radar still fires on row select; true one-click dispatch with scout guards deferred until an explicit dispatch control exists on the panel.
- StandingRecordAtlas fixture rows still emit `faction_*` ids — wired to faction detail, not layout sites.
- Caravan `FocusLedgerSection` only expands biology drawer; offer/ask scroll targets not present on TradeScreen.
- Shared paths (`INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`) not updated (not foreman/integrator role).
- Plans 146–149, DEBT-PLAN167/168, theater fake-feedback remain deferred.
