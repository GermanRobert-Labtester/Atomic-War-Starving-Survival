# Plans 198–201 — Integration Closeout & Follow-Up Register

**Status:** Player-facing integration complete (commits `231595b8` + this follow-up commit).
**Scope delivered:** CBRN hazard warfare (198), communications arrays (199), ceremonies (200), robotics (201) — Core systems, catalogs, save stores, host orchestration, UI bodies, routes, and end-to-end verification.

---

## 1. Delivered Layers

| Layer | Plan 198 | Plan 199 | Plan 200 | Plan 201 |
|---|---|---|---|---|
| Core system | `Combat/ChemWarfareSystem.cs` | `World/CommsArraySystem.cs` | `Narrative/CeremonySystem.cs` | `Crafting/RoboticsSystem.cs` |
| Catalog | `chemical_weapons.json` (5 agents) | `comms_targets.json` (8 targets) | `ceremonies.json` (5 ceremonies) | `robotics.json` (5 archetypes) |
| Save store | `ChemWarfareSaveStore` | `CommsArraySaveStore` | `CeremonySaveStore` | `RoboticsSaveStore` |
| Host wiring | `Main.Plans198_201.cs` (Setup/Save/Tick triad, RNG forks `chem_warfare`/`comms_array`/`ceremony_system`/`robotics`) | | | |
| UI body | `ChemWarfareDefensePanel` | `CommsArrayTransceiverPanel` | `CeremonyFestivalPanel` | `RoboticsWorkshopPanel` |
| Route | `chem_warfare_defense` | `comms_array_transceiver` | `ceremony_ritual` | `robotics_assembly` — all registered **Live** |
| Tests | 47 Core tests + `--plans198-201-uitest` headless gate | | | |

## 2. Verification Evidence

```
--plans198-201-uitest      PASS  (5/5 segments: chem, comms, ceremony, robotics, registry routes)
  route → bind → visible → command → state delta → feedback, exception-free, exit 0
dotnet test                10,760 / 10,760 PASS
--data-integrity-selftest  PASS — 0 errors / 299 catalogs
--content-utilization-selftest PASS — all four catalogs at stage 4 (gameplay-consumed)
--scene-binding-selftest   25 / 25 PASS
--bridge-selftest          PASS
dotnet build Ashfall.csproj  0 errors, 0 warnings
```

## 3. Architectural Compliance

- All mutations delegate: inventory → atomic `InventoryBill` transactions; morale/truce/journal → host-adapter events from Core; power → grid query at tick. Panels are presentation-only (`OnActionRequested` + read-only state queries).
- Determinism: forked `ISeededRng` substreams per system; disaster rolls, orbital windows (`IsInSatelliteWindow`), rogue-AI corruption, EMP recovery are all seed-deterministic.
- Safety boundary (Plan 198): abstract hazard classes, densities, severities, filter-wear fractions only. No chemistry, dosing, dispersal engineering, or construction content anywhere in data or code.
- Persistence: legacy saves default to empty registries (`RestoreState(null)` → fresh state); envelope/checksum via the canonical save-store pattern.
- Display-name discipline: raw IDs never rendered — `Plans198To201Display.cs` is the single prettification authority.

## 4. Resolved Follow-Ups (from the wave handoff)

| Follow-up | Resolution |
|---|---|
| MoralChoiceCatalogTests lone failure | **Resolved upstream by concurrent streams** — full suite now 10,760/10,760 green. |
| Satellite-window display used only persisted lock state | **Resolved:** `CommsArrayTransceiverPanel.SetDisplayClock(day, hour)` (host passes `_simDay` at OPEN); the panel now queries the deterministic `CommsArraySystem.IsInSatelliteWindow` for a live OVERHEAD/CLOSED readout. Core remains the sole calculation owner. |
| Panel verification gap (UI-21-style construction-only tests) | **Resolved:** new `--plans198-201-uitest` proves the full route → bind → visible → command → state delta → feedback contract per panel, fails on any engine exception, and covers blocked paths (unknown ceremony, missing strike code, repair without materials, double-clear). |
| UI-07 / UI-09 register entries | **Closed for the four panels** in `AGENTS.md` with evidence; six UI-07 stubs and nineteen UI-09 IDs remain for other streams. |

## 5. Open Follow-Ups (tracked, not yet scheduled)

1. **Cross-plan scenarios A–F** (festival-under-raid, comms+orbital telemetry, festival diplomacy+off-map trade, robot-assisted toxic cleanup, EMP crisis, rogue-AI contact) — the event seams all exist (journal adapters, hazard/trauma/sanitation handoffs, stance engine, grid); a dedicated integration-plan pass should script the six scenarios as headless multi-system journeys with save/load splits and duplicate-event suppression assertions. Suggested as its own flagship plan.
2. **Lane HEAD standalone build** — pre-existing: concurrent streams' committed call-sites (e.g. `GrainMillingDiscoverySystem.GetAny`, `JournalSystem.UnlockBureaucraticDocument` consumers) reference definitions still uncommitted in their working trees. Not introduced by this wave; resolves when those streams commit their remaining work.
3. **Robotics combat/expedition actor mapping** — `RobotDefinition.compatible_tasks` includes combat/expedition roles; wiring robot units into `TacticalCombatSystem` actor interfaces and `ExpeditionSystem` party composition is the next integration seam (currently labor/grid/EMP scope only).
4. **UI-07/UI-09 remaining six panels** — `AmputationTriage`, `ArchaeologyExcavation`, `JusticeTribunal`, `RailwayTerminal`, `SurvivorDowntime`, `WinterFreeze` (owned per the AGENTS.md register).

## 6. Canonical Verification Commands

```bash
dotnet build Ashfall.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --plans198-201-uitest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --scene-binding-selftest
godot --headless --path . -- --bridge-selftest
```
