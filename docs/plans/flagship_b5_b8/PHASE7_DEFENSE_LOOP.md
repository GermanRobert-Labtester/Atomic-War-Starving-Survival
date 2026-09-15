# Phase 7 — Defense Loop (B5–B8 / Plan 67) — increment 1 landed

> Owner: `PerimeterDefenseSystem` (Plan 203) — never a new `DefenseSystem`
> (the reconciliation prohibited duplicating it). Warlord/faction pressure
> stays the raid authority; `TacticalCombatSystem` stays the combat resolver.

## Audit results (what was already live)

| Brief assumption | Live truth |
|---|---|
| Raid path integration | `Main.Muster` → `ResolveRaidDefenses` (Plan 162) → `PerimeterDefenseSystem.ResolvePreCombatRaid` → only raiders that breach reach `TacticalCombatSystem`. Warlord doctrine/faction pressure untouched by defense ✓ |
| Ammo | `LoadAmmo` consumes canonical `ammo_9x19`/`ammo_556` from inventory; magazine + barrel wear + jam model live ✓ |
| Wear/repair | barrel wear, weather wear, scrap-metal repair, trap reset/repair — all costed ✓ |
| Turret power coupling | LIVE since Plan 71 via the shared `room_armory_munitions` circuit callback — not per-emplacement registration. Kept (a per-emplacement `RegisterLoadRoom` would double-count draw against the established shared-circuit model); migrated to allocation-aware served state (below) |
| IFF beacon | crafting consumer already live: `integrate_iff_transponder` (gated by `knowledge_iff_transponder_blueprint`) consumes the beacon → `handheld_radio` — an expedition/comms capability, not sentry ammunition |

## Landed in this increment

### 1. Research-gated emplacement construction (§10.5, §15.3)
- Catalog: additive `required_knowledge` field on `PerimeterDefenseDefinition`
  (empty = basic fieldworks, ungated — legacy parity).
- Data: `def_sentry_turret_9mm` → `knowledge_automated_sentry_doctrine`;
  `def_sentry_turret_556` → `knowledge_turret_controller_blueprint`;
  `def_reinforced_outer_gate`/`def_heavy_barricade` →
  `knowledge_fortified_chokepoints`. An unlocked node without a built
  emplacement contributes zero defense — pinned by test.
- Core: `ConstructEmplacement(defenseId, hasRequiredCapability = true)` —
  additive param; blocked `missing_knowledge`, zero mutation on block;
  ungated defs construct freely (pre-gate callers unchanged).
- Real route: the defense grid previously had **no construction surface at
  all** — emplacements existed in catalog/save but no player path could build
  one. `DefenseGridPanel` gains a CONSTRUCT EMPLACEMENT section (picker, cost
  + research display, BUILD button); `Main.Plans162_165` gains the `BUILD`
  case with the live capability query (never cached).

### 2. `item_sentry_targeting_chip` — real dependency (§10.8)
The chip was catalog-only with zero consumers. Both turret rows now include
`item_sentry_targeting_chip: 1` in `build_costs` — a canonical construction
dependency consumed exactly once by the existing atomic build path. Pinned by
an aggregated catalog test with per-row failure output.

### 3. Allocation-aware turret power (§10.14)
`ResolveRaidDefenses`' power callback migrated from the global-outage read
(`!IsBrownout && IsRoomPowered(armory)`) to the Phase 2 allocation-aware
`IsRoomServed("room_armory_munitions")`: during a brownout the armory circuit
stays live while generation covers it and sheds by priority otherwise — the
same migration the sump pump received. Power never modifies attackers; the
freeze flows through the emplacement's own assault semantics.

### 4. IFF classification (§10.9) — documented, not implemented
Classification exercise result: the item's authored semantics ("neutralize
automated sentry guns") target **hostile automated sentry encounters — none
are authored in any encounter catalog**. The live consumer is the crafting
chain above (expedition/comms gear). The automated-encounter consumer is
**deferred** until such an encounter exists; the beacon is NOT wired to
human-faction raids in any form (the flagship's R12 hazard prevented by
construction — no bypass code exists).

## Deliberately deferred

| Item | Target | Reason |
|---|---|---|
| IFF automated-encounter consumer | follow-on | no automated sentry encounter authored; inventing one exceeds Plan 67 scope |
| Raid-defense modifier snapshot typing (§10.11) | Phase 8/follow-on | `ResolvePreCombatRaid` already receives typed inputs (perimeter state, power callback, guard modifier); a formal immutable snapshot type is hardening, not a gap |
| Defense balance harness (§10.17) | Phase 8 | scenario set |
| Per-emplacement load registration | rejected | would double-count the Plan 71 shared-circuit draw; documented decision |

## Verification record (2026-09-13)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs` | 6/6 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs` | 8/8 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs` | 16/16 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | 1148/1148 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/B5B8Phase0SaveFixtureTests.cs` | 6/6 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | 14/14 PASS |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `godot --headless --path . -- --content-utilization-selftest` | PASS — 0 hard failures |
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | PASS |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Defense/PerimeterDefenseCatalog.cs` (additive `required_knowledge`)
- `Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` (capability-gated build)
- `Assets/StreamingAssets/Data/perimeter_defenses.json` (4 gates + 2 chip costs — minimal diff)
- `src/UI/DefenseGridPanel.cs` (CONSTRUCT EMPLACEMENT section)
- `src/Main.Plans162_165.cs` (BUILD route + served-state power migration)
- `Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs` (new, 6 tests)
- `docs/plans/flagship_b5_b8/PHASE7_DEFENSE_LOOP.md` (this file)
