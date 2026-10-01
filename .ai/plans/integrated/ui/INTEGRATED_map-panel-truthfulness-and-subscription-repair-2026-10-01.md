# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user 2026-10-01 — task T11 of the repair checklist; integrated and archived in the same session.)

# T11 — MapDetailPanel fabricated hazards + MapPanel double-subscribe

> **STATUS: APPROVED BY USER**
> (User-authorized task T11 of the repair checklist, 2026-10-01: "Fix
> `MapDetailPanel` fabricated hazards + `MapPanel` double-subscribe — flagged
> in state; `MapPanel` fog gate already fixed.")

## Goal

Repair the two remaining map-panel defects flagged in `.ai/state.md` after the
task 14/15 integration (line 252 flag list):

1. **`MapDetailPanel` fabricated hazard rows (display-only)** — the detail
   panel presents fabricated content as authoritative survey fact.
2. **`MapPanel.Bind` `OnMarkersChanged` double-subscribe leak** — `Bind`
   subscribes `RefreshView` to five event sources without removing previous
   subscriptions, and `Unbind` never removes `OnMarkersChanged` at all.

## Evidence (premise audit, read-only)

- `MapPanel.Bind` is invoked on **every panel open** (`OpenMapPanel` in
  `src/Main.UiHandlers.cs:150` and the `PanelRegistry` `bindAction` in
  `src/Main.PlayerSurfaces.cs:352`), so each open adds another
  `StateChanged`/`OnMarkersChanged` subscription; after N opens, one world
  event fires `RefreshView` N+ times. `Unbind()` also never removes the
  `OnMarkersChanged` subscription and never nulls session refs.
- `MapDetailPanel.Bind` hazard card renders two advisory rows no system backs:
  "Required Protective Gear" (implies a gear-requirement system) and
  "Transit Stance Advice" (implies a wired stance gate; the stance authority
  is host-unbound).
- Sub-Layouts and Salvage sections render **fully fabricated catalog rows**
  whenever `subLayouts`/`lootCategories` are null — and every current caller
  passes null (no data source exists: `locations.json` has no
  `sub_layouts`/`loot_categories` fields; both `Bind` overloads hardcode
  `subLayouts: null, lootCategories: null`).
- Task 14 established (and the foreman ratified) that showing hazard numbers
  for fog-Unknown sectors is fabrication: `MapPanel` now shows
  `[UNCHARTED] · no survey data`. But the INSPECT button still opens
  `MapDetailPanel`, which displays full `Threat Tier`/`Ambient Radiation
  Rate` numbers for those same uncharted sectors — inconsistent with the
  ratified fog gate.

## Root cause

- **Double-subscribe:** `Bind` mutated bindings additively; the house idiom
  (see `ArchiveDeskPanel.Bind`) is unsubscribe-old → assign → subscribe.
- **Fabrication:** panel-authored fallback prose filled the gap where no
  authority exists, violating "a panel exposes truthful current state."

## Repair

1. `src/UI/MapPanel.cs`
   - `Bind` becomes idempotent: call `Unbind()` first, then assign refs and
     subscribe (house pattern).
   - `Unbind` removes **all five** subscriptions (adds the missing
     `_world.WastelandMap.OnMarkersChanged -= RefreshView`) and nulls the
     session refs.
2. `src/UI/MapDetailPanel.cs`
   - Remove the fabricated "Required Protective Gear" and "Transit Stance
     Advice" rows (authored catalog rows "Threat Tier" / "Ambient Radiation
     Rate" stay — they are real data for charted sectors).
   - Add `bool uncharted = false` parameter; when uncharted, the hazard card
     reports "UNCHARTED — no survey data" instead of hazard numbers (same
     predicate and wording family as the ratified `MapPanel` fog gate).
   - Replace fabricated sub-layout/salvage fallback rows with truthful
     "no survey on record" rows; keep the optional real-data seam.
3. `src/Main.UiHandlers.cs` (`OpenMapDetailPanel`, the single funnel for both
   `MapPanel` and `MapAtlasPanel` inspect requests)
   - Compute `uncharted` from the canonical map with the same predicate as
     `MapPanel` (`GetNode != null && !IsDiscovered && GetFogState ==
     Unknown`) and pass it to the detail panel. The host stays the authority;
     the panel stays presentation-only.

## Invariants

- No Core change; no new catalog, save section, or parallel state.
- Charted sectors render exactly the same authored numbers as before.
- Event order/count otherwise unchanged; `RefreshView` is still called once
  per `Bind` and once per state change while bound (but exactly once per
  event regardless of re-bind count).
- No l10n key changes (panel prose is hardcoded English today, like the
  existing fog-gate string).

## Non-goals

- `MapPanel` static fabricated route rows (separate flag item — follow-up).
- `WorkshopPanel`/`PharmaLabPanel` discarded `ActionResult`s (separate flag).
- No commit of foreign dirty work; full suite not run.

## Test plan

- New `[Fact]` in `Ashfall.Core.Tests/UI/PanelSubscriptionHygieneTests.cs`:
  `MapPanel` re-bind must be subscription-safe (Bind calls Unbind before the
  first subscribe; Unbind removes all four host sources incl.
  `OnMarkersChanged`).
- New `Ashfall.Core.Tests/UI/MapPanelTruthfulnessGateTests.cs`:
  - no fabricated hazard/advisory/survey literals in `MapDetailPanel.cs`;
  - hazard numbers gated behind `uncharted` with "no survey data" wording;
  - the `OpenMapDetailPanel` funnel passes a canonical-map-derived
    `uncharted` flag.
- Scoped runs via `bin/run-scoped-tests`; host build 0 errors; focused
  headless checks (`--scene-binding-selftest`, `--ui-layout-selftest`) since
  panel chrome/tscn is untouched but panel code changed.

## Definition of done

Gates green, build clean, plan marked FULLY INTEGRATED and archived,
`.ai/state.md` updated, claim released.

## Integration record (2026-10-01)

- Changed files: `src/UI/MapPanel.cs` (idempotent `Bind` via unsubscribe-first;
  `Unbind` now removes all five subscriptions incl. the leaked
  `OnMarkersChanged` and nulls session refs), `src/UI/MapDetailPanel.cs`
  (fabricated protective-gear/stance advisory rows removed; hazard numbers
  fog-gated behind `uncharted` with "no survey data" wording; fabricated
  sub-layout/salvage fallbacks replaced with truthful empty states;
  `bool uncharted = false` threaded through both `Bind` overloads),
  `src/Main.UiHandlers.cs` (`OpenMapDetailPanel` computes `uncharted` from
  the canonical map with the same Plan 32 predicate as `MapPanel`),
  `Ashfall.Core.Tests/UI/MapPanelTruthfulnessGateTests.cs` (new, 3 facts),
  `Ashfall.Core.Tests/UI/PanelSubscriptionHygieneTests.cs` (new
  `MapPanelBindIsResubscriptionSafe` fact).
- Verification: `scripts/run_test.sh` on both gate files → 3/3 and 2/2 PASS;
  `dotnet build Ashfall.csproj` 0 errors / 0 warnings; headless
  `--scene-binding-selftest` 25/25 PASS (incl. `MapDetailPanel.tscn`);
  `--ui-layout-selftest` PASS. No commit; full suite not run; foreign dirty
  work untouched.
- Observation left for a future pass (not T11 scope): `MapPanel` adds the
  trapping card into the overview card's box (`ovBox.AddChild(trapCard)`)
  instead of `_overviewContainer` — nested-card oddity, renders fine.
