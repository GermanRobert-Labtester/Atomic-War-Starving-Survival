# Feature / Task Plan: The Drowned Coast — a maritime campaign: boats, harbours and dives

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_drowned_coast_plan.md`. Family index: `docs/expansions/expansion_world_moves_without_you_index.md`.
> Not a claim. Sits **on top of** `PLAN-MARITIME-DEEPWATER-27` (PROPOSED, unclaimed) and Expansion 09 — see DEC-DC-01.

## 1. Goal & Outcome
- **Goal:** Turn the existing water machinery (5 hulls, tide, 14 dive sites, Flotilla standing, District 8 dock) into a campaign: persisted owned vessels, harbours with dues/berths/standing, tide-scheduled voyages, a deterministic waterline that drowns berths, and five authored movements.
- **Outcome (observable):** on a fixed seed a player-owned skiff leaves a held berth on a tide-legal day, spends fuel, rolls a seeded piracy check, arrives, dives an authored site whose window is open, returns; the vessel, its hull/fuel/port, and the berth's waterline state survive save/load.
- **Non-Goals:** no new map/expedition/dive system; **no restoration of the retired `MaritimeExplorationSystem` or `maritime_zones.json`** (archived under `docs/archive/retired-data/`); no new save section; no new routed panel; no change to District 8's four decisions; no real nautical data; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | 5 vessel defs with fuel, crew, hull, corrosion, draft, combat rating, repair materials, tags. | `naval_vessels.json`; `Expeditions/ExpeditionNavalSystem.cs` | LIVE |
| E2 | `NavalVesselInstance` (hull, fuel, `isDocked`, `currentPortId`); **no Capture/Restore on `ExpeditionNavalSystem`**. | `ExpeditionNavalSystem.cs` L38–47 | GAP / VERIFY |
| E3 | Two naval holders: `Main._navalSystem` (`EnsureNavalSystem`) and `ExpeditionHostSession._naval`. | `src/Main.NavalExpeditions.Integration.cs`; `src/Host/ExpeditionHostSession.cs` L360 | VERIFY |
| E4 | `EstimateRoute(vessel, route, freezeState, weatherFactor)`, `PiracyRisk`, `RollPiracyEncounter(estimate, ISeededRng)`, `ApplyWaterCorrosion`. | `ExpeditionNavalSystem.cs` | LIVE |
| E5 | 14 dive sites incl. `tide_window`, `location_id`, `discovery`, `keeper_thread_id`. | `dive_sites.json` | LIVE |
| E6 | Tide = 4-day cycle; six windows; `IsWindowOpen`, `DaysUntilOpen`. | `Maritime/TideCalendar.cs` | LIVE |
| E7 | `MaritimeHostSession` + `maritime` save section (`maritime_save.json`); `MaritimeAtlasPanel` reads tide. | `src/Host/MaritimeHostSession.cs`; `Save/SaveSectionRegistry.cs` L74, L453; `src/UI/MaritimeAtlasPanel.cs` | LIVE |
| E8 | `MaritimeDiveSystem` has no `src/` reference; `DiveInstanceRunner` is host-constructed. | grep; `ExpeditionHostSession.cs` L40, L1641 | VERIFY |
| E9 | District 8 Deep Coast: stages 0–4, 4 decisions, integrity, contamination; HoldfastSave-owned. | `District8DeepCoastSystem.cs`; `src/Host/DeepCoastHostSession.cs` | LIVE |
| E10 | Flotilla standing authority + host + selftests. | `Maritime/BlackFlotillaStanding.cs`; `--black-flotilla-standing-selftest` | LIVE |
| E11 | Retired: `MaritimeExplorationSystem` + Plan207 test + `maritime_zones.json` (archived). | `WORKTREE_OWNERSHIP.md` L11866–11874 | LIVE; deletions uncommitted in worktree |
| E12 | `PLAN-MARITIME-DEEPWATER-27` still lists the retired system/data as existing machinery. | `docs/plans/EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-MARITIME-DEEPWATER-27.md` §1 | Stale premise |
| E13 | No waterline/drowned-berth concept. | grep; `world_evolution_seeds.json` | GAP |
| E14 | Selftests: `--maritime-selftest`, `--black-flotilla-selftest`, `--deep-coast-selftest`, `--deep-coast-host-selftest`, `--deep-coast-route-selftest`, `--deep-coast-playthrough`. | `src/Host/HostCli*.cs` | LIVE (VERIFY args) |
| E15 | Freeze state / ice road exist. | `YearOfAsh/YearOfAshIceRoadSystem.cs`; `weather_seasons.json` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Vessel definitions & route estimates | `ExpeditionNavalSystem` | Capture/Restore of instances (P1), once ownership (E3) is resolved |
| Vessel persistence | `maritime` section (`MaritimeHostSave`) **or** expedition save — decide in P0 | nested `vessels[]` (additive) — **DEC-DC-03** |
| Dives | live dive authority per P0 (E8) | gating only (boat + tide + berth + chart) |
| Tide | `TideCalendar` | read-only |
| Flotilla standing | `BlackFlotillaStanding` | read-only |
| District 8 | `District8DeepCoastSystem` | read-only; decision unchanged |
| Harbour berths/dues | — | small `HarbourLedger` (pure Core), nested in `maritime` |
| Waterline | — | pure deterministic function of season + storms; only *lost-berth latch* is stored |
| Charts (knowledge) | existing knowledge/field-guide owner (VERIFY) | consumer; chart ids in data |
| Ownership of coast locations | `LocationEvolutionSystem` | read-only |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Maritime/HarbourLedger.cs` (new), `Maritime/WaterlineModel.cs` (new, pure), `Maritime/CoastVoyagePlanner.cs` (new, pure: tide-legal departure + route estimate composition), `Expeditions/ExpeditionNavalSystem.cs` (additive Capture/Restore), `Maritime/DiveSiteCatalog.cs` (read-only), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream ids (`INT`)
**Data:** `coast_harbours.json` (6 harbours, dues, berth rules, chart ids), `coast_waterline.json` (per-harbour curve, storm steps), `coast_movements.json`, `coast_lines.json`
**Host:** `src/Host/MaritimeHostSession.cs`, `src/Host/MaritimeSaveStore.cs`, `src/Main.NavalExpeditions.Integration.cs` (`INT`), `src/Host/ExpeditionHostSession.cs` (`INT`, only the naval holder line)
**Presentation:** `src/UI/MaritimeAtlasPanel.cs` (existing) — harbours, next open window for *charted* sites
**Tests:** `Ashfall.Core.Tests/Maritime/HarbourLedgerTests.cs`, `WaterlineModelTests.cs`, `CoastVoyagePlannerTests.cs`, `Ashfall.Core.Tests/Save/DrownedCoastSaveTests.cs`

## 5. Packages

### DC-P0 — Premise audit & reconciliation (Auditor; read-only)
- Resolve E2 (persistence), E3 (two naval holders: same instance or two), E8 (live dive authority); confirm E11's uncommitted-deletion state and that nothing in current source references the retired names; list Plan 27 rows that are stale (E12) for the integrator; foreman signs DEC-DC-01…10.
- **Accept:** E2/E3/E8 each resolved with `path:line`, or a named blocker.

### DC-P1 — Vessel ownership & persistence (Core + host)
- Additive Capture/Restore of `NavalVesselInstance[]`; a single holder (per E3); nested in the owner selected in P0.
- **Accept:** round-trip of hull, fuel, port, docked flag; old saves load with an empty fleet; ship-dark parity for `--maritime-selftest`.

### DC-P2 — Harbour Ledger (Core, pure + data)
- Berths (held/rented/refused), dues (in harbour-valued goods), standing read from existing owners; six authored harbours.
- **Accept:** table-driven; no writes to standing authorities; dues charged only through the inventory/funds owners.

### DC-P3 — Coast Voyage Planner (Core, pure)
- Composes `TideCalendar`, `EstimateRoute`, weather gate, freeze state and berth availability into a departure verdict + fuel/hull/crew cost; piracy check via `ISeededRng` from a `CampaignStreamIds` fork (new id is `INT`).
- **Accept:** same inputs+seed → same verdict/roll; refused departures return a cause; no I/O.

### DC-P4 — Waterline model (Core, pure)
- Per-harbour curve from season/storms; berth states *usable / awash / lost*; only the **lost latch** persists; announcements two days ahead.
- **Accept:** deterministic; a lost berth never returns; announcement precedes the step; Board line carries the cause.

### DC-P5 — Dive gating & aftercare (Host)
- A dive requires: vessel at a usable berth, tide window open (charted site), crew at surface, chart known. Aftercare uses the existing contamination owner.
- **Accept:** dive refused with cause when any gate fails; nothing added to the dive authority itself.

### DC-P6 — Ice modality (read-only cross-check)
- Winter freeze state changes the voyage verdict into "walkable/road" or "closed" via the existing freeze input.
- **Accept:** no ice-road system changes; verdict text distinguishes water from ice.

### DC-P7 — Movements I–V & chronicle (content + light quest wiring)
- Five movements as quest/journal chains through existing quest authority; District 8 decision is *consumed*, not changed.
- **Accept:** each movement has a start gate, end marker, and journal line; quest-coverage validators pass.

### DC-P8 — Year Two coda (depends on Year Two P1B/P5)
- By Chapter Profile: harbour as candidate second-shelter door **(Year Two owns custody)** or ledger coda.
- **Accept:** legacy profiles unchanged; no custody change from this plan.

### DC-P9 — Content waves W1–W5 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Determinism: identical voyage/piracy/waterline sequences on replay.
3. Save round-trip with a vessel at sea/docked and a berth awash.
4. Ship-dark parity: no coast data → `--maritime-selftest`, `--black-flotilla-selftest`, `--deep-coast-selftest` unchanged.
5. Retired names absent from new code and data (grep gate).
6. No dive/expedition authority modified beyond a single holder line.

## 7. Cross-plan boundaries
- **Plan 27:** this is its campaign layer; DC-P0 supplies corrections; no overlap of claimed paths (foreman verifies).
- **Year Two:** harbour-as-outpost is a Year Two custody decision; this plan only offers the *place*.
- **The Long Line: Freight:** water legs are conditions the freight resolver reads; no writes.
- **The Plague Year:** quarantine anchorage is a harbour state Plague Year may set; the ledger exposes a read-only "closed" flag.
- **The Living Region:** flooded-out waves and `deep_coast` harbours; read-only.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-DC-01 | This plan is the campaign layer over Plan 27; stale Plan 27 rows corrected by the integrator. | governance | Yes |
| DEC-DC-02 | One naval holder (unify `Main._navalSystem` and `ExpeditionHostSession._naval`) — or prove they share state. | architecture | Decide in P0 |
| DEC-DC-03 | Vessel and harbour state nest in `maritime`, no new section. | architecture | Yes |
| DEC-DC-04 | Waterline is a pure function; only the lost-berth latch persists. | design | Yes |
| DEC-DC-05 | Charts are knowledge entries in the existing knowledge owner, not items. | design | Decide in P0 |
| DEC-DC-06 | `MaritimeDiveSystem` fate (live vs duplicate) is **not** decided here. | governance | Foreman |
| DEC-DC-07 | No restoration of retired maritime code/data. | scope | Yes |
| DEC-DC-08 | Two-day announcement before any waterline step. | design | Yes |
| DEC-DC-09 | Year Two coda is optional (profile-driven). | compatibility | Yes |
| DEC-DC-10 | No new routed panel; extend `MaritimeAtlasPanel`. | UI | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Harbour`, `Waterline`, `Chart`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim (especially Plan 27 and the retire-duplicate claim)
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing Maritime/Naval/DeepCoast/BlackFlotilla tests (list from P0 selector)
- [ ] `--maritime-selftest`, `--black-flotilla-selftest`, `--deep-coast-selftest`, `--deep-coast-route-selftest` (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: vessel persistence needs a second store; E3 shows two independent naval authorities and unification touches a claimed path; any task would restore retired maritime code/data; a dive change is needed beyond gating; any path overlaps a live claim.
