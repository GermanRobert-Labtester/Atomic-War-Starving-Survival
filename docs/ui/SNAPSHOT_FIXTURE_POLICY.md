# ASHFALL — Snapshot Fixture Policy

**Date:** this turn (Phase 14).
**Status:** enforced in code where possible (see `_Ready()` for each Phase 12/13 panel); enforced in audit where the harness cannot tell live from fixture.

A *fixture* is **deterministic test data** that fills a snapshot target when no real host session is bound. It is **visible only in the snapshot harness**, not in normal gameplay. Implementation-time knobs:

- `Bound: true` paths read real Core / host session state.
- `Bound: false` paths render fixture rows + empty-state placeholders.

Every fixture must satisfy both:

1. **Domain scope** — the data is plausible for the runtime.
2. **Harness scope** — the data is deterministic (seeded, hard-coded, or ordered).

## Categories

| Code | Meaning |
|---|---|
| `LIVE_CORE` | Snapshot reads from authoritative Core / host APIs (e.g. `DoseLedgerSystem.GetCumulative`, `FactionStanceEngine.GetStance`, `WeatherSystem.PeekForecast`). |
| `CAPTURED_REAL_STATE` | Snapshot reads from a host session that was seeded deterministically (`Seed1401`-style), so the render is real even though the input is synthetic. |
| `DETERMINISTIC_TEST_FIXTURE` | Snapshot reads hard-coded in-file data constructed specifically for the harness. The fixture is gated by `_session == null`. |
| `MANIFEST_FIXTURE` | Snapshot reads deterministic test data declared in a manifest file (not yet present in Phase 14 — see "Future extension" below). |
| `MIXED` | One target combines LIVE_CORE for some cells and fixture for others (e.g. `faction_matrix_default`). |
| `EMPTY_STATE` | Bind: false. No data yet. Pure empty UI chrome. |

## Allowed

- **Hard-coded fixture rows** for tables and grids when the harness cannot bind.
- **Realistic content IDs** drawn from the canonical master list (see `docs/ui/PHASE13_DATA_AVAILABILITY.md` for ID provenance).
- **Valid catalog IDs** — fixture rows must use IDs that resolve through `AssetRegistry` or survive an `_IconLoader.LoadFor` failure without panicking.
- **Stable ordering** — fixture row order must be deterministic; no `System.Random`, no framework-level iteration ordering.
- **Explicit fixture-only paths** — fixture data is clearly separated: e.g. `BuildFixtureRows()`, `BuildFixtureFactions()`, `BuildForecastFixture()`. These methods are only called when `Bound == false`.
- **Blank fallback textures** — when icons cannot resolve, empty cell text is OK (`"—"`).

## Forbidden

- **Invented production fallback data** — never carry in-file fixture rows into a `Bound == true` path. The brief makes this explicit.
- **Impossible state combinations** — e.g. Survivor with `HP=0` showing `STATUS=STABLE`, or Dose ledger showing `BLACK` band with `0 mSv`. Each cell must reflect the value actually stored.
- **Magic numbers chosen only for visual attractiveness** — fixture thumb values (e.g. trust = `+37`, aggression = `0.42`) come from real Seed1401 host session capture, not invented.
- **Fictional metrics** — no "Market Sentiment Index", "Survivor Happiness Quotient", "Weather Confidence %". These do NOT have Core APIs.
- **UI-only simulation** — do not encapsulate loops that simulate host ticks. The snapshot must show what `RefreshView()` would draw if bound.
- **Silent fallback decoration** — main UI must never depend on fixture data. `RefreshView()` always re-reads from `_session` when bound; fixture is a snapshot-only fallback.

## Enforcement

### Code-side

`AshfallDashboardShell` / `AshfallSidebar` / `AshfallStatusRail` / `AshfallMetricCard` / `AshfallDataGrid` carry **no** fixture data themselves. They are pure presentation primitives. Each panel's `_Ready` builds UI; `RefreshView` is the single path that fetches Core state or fixture data conditionally.

### Audit-side

Each snapshot target classifies its data source explicitly (see `snapshot_manifest.json` → `fixture_source` field).

The brief's "fixture validation" rule is implemented by the manifests themselves: any `BuildFixtureRows()` call exposes hard-coded ids — if any such id drifts out of the canonical list, the `CatalogLocator / StreamingAssets has X ids registered` check downstream will fail with cross-reference noticed — and a snapshot needing the fixture will fall back to `EMPTY_STATE` (still deterministic).

## All Phase 12/13/15+ targets and their fixture policy

| Target | Data source | Fixture source | Notes |
|---|---|---|---|
| medical_default | LIVE_CORE | `DoseLedgerSystem` + `MedicalSystem` | Reads real dose ledger and medical state. |
| shelter_default | LIVE_CORE | `ShelterSystem` | Reads real shelter metrics. |
| journal_default | LIVE_CORE | `JournalSystem` | Reads real journal entries. |
| inventory_default | LIVE_CORE | `InventorySystem` | Reads real inventory state. |
| survivors_default | LIVE_CORE | `SurvivorsHostSession.RosterState` | Reads real survivor roster. |
| radio_default | LIVE_CORE | `RadioSystem` | Reads real radio channels. |
| weather_default | LIVE_CORE | `WeatherSystem` | Reads real weather forecast. |
| verdict_default | LIVE_CORE | `VerdictSystem` | Reads real verdict state. |
| trade_default | LIVE_CORE | `TradeScreenGodotPanel` (INTENTIONAL_CHILD) | Focused child of `CaravanBarterLedgerPanel`. |
| survival_workstation_default | LIVE_CORE | `CraftingSystem` + `RecipeCatalog` | Reads real recipes and workstation state. |
| caravan_barter_default | LIVE_CORE | `CaravanBarterLedgerPanel` | Reads real barter ledger. |
| shelter_hud_default | LIVE_CORE | `ShelterHudPanel` | Reads real shelter HUD metrics. |
| faction_matrix_default | LIVE_CORE | `FactionStanceEngine` | Reads real faction stances. |
| dose_ledger_default | LIVE_CORE | `DoseLedgerSystem` | Reads real dose ledger. |
| verdict_dashboard_default | LIVE_CORE | `VerdictSystem` | Reads real verdict dashboard. |
| weather_dashboard_default | LIVE_CORE | `WeatherSystem` | Reads real weather dashboard. |
| greenhouse_default | LIVE_CORE | `GreenhouseSystem` | Reads real greenhouse state. |
| silent_foundry_default | LIVE_CORE | `SilentFoundrySystem` | Reads real foundry recipes. |
| expedition_radar_default | LIVE_CORE | `ExpeditionHostSession.Active` + `DemoDefinitions` | Reads real expedition radar. |
| skill_matrix_default | LIVE_CORE | `SkillProgressionSystem` + `SurvivorsHostSession.RosterState` | Reads real skill matrix. |
| duty_roster_default | LIVE_CORE | `DutyRosterSystem` | Reads real duty roster. |
| factions_narrative_default | LIVE_CORE | `FactionStanceEngine` (`IFactionStanceProvider`) | Reads real faction narrative. |
| combat_hud_default | LIVE_CORE | `TacticalCombatSystem` via `CombatHostSession` | Reads real combat state. |
| map_atlas_default | LIVE_CORE | `ExpeditionDefinition` via `ExpeditionHostSession.DemoDefinitions` | Reads real map quadrants. |
| maritime_atlas_default | LIVE_CORE | `MaritimeHostSession` | Reads real maritime state. |
| muster_atlas_default | LIVE_CORE | `MusterSystem` via `MusterHostSession` | Reads real muster currents. |
| quests_atlas_default | LIVE_CORE | `QuestsHostSession` | Reads real quests. |
| standing_record_atlas_default | LIVE_CORE | `StandingRecordEngine` via `StandingRecordHostSession` | Reads real standing record layouts. |
| research_atlas_default | DETERMINISTIC_TEST_FIXTURE | `ResearchSystem` via `ResearchHostSession` (null bound) | Renders 15-node fixture knowledge grid + active research row + breakthrough items grid + action bar. All ids canonical (drawn from the 15-node inline catalog). |



A `MANIFEST_FIXTURE` shape could exist alongside `DETERMINISTIC_TEST_FIXTURE`:

```jsonc
// docs/ui/fixtures/faction_matrix_v1.json
{
  "version": 1,
  "rows": [
    { "faction_id": "warlords_sector_4", "trust": -10, "aggression": 0.65, "stance": "Rob" },
    { "faction_id": "iron_garrison",     "trust":  30, "aggression": 0.20, "stance": "Trade" }
  ]
}
```

The activation pipeline would read this file under `--ui-snapshot-uitest --fixture=manifest` and assert that each ID resolves via `AssetRegistry.GetFaction` before populating the dashboard. Phase 14 did not deliver this; Phase 15+ is the natural home for it because it requires extending `SnapshotOrchestrator` to pass the mode flag. **It is explicitly not Phase 14 work** — Phase 14's mission is "audit what's there, not build a new fixture engine".

## Discipline when adding NEW snapshot targets

When a worker adds a new `Target` entry to `SnapshotHarness.cs`:

1. **Document** `data_source` field — never leave it ambiguous.
2. **Bind or empty** — pick one: bind the host session deterministically, OR explicitly empty-state with placeholders.
3. **Verify fixture IDs** — any `BuildFixtureRows()` use must round-trip with `CatalogLocator.Catalog` for id-in-file id validity.
4. **Inspect the resulting PNG**, not just the size. Phase 10 audit policy: PNG PASS does not prove visual correctness.
5. **Update `docs/ui/snapshot_manifest.json`** — never orphan a target.
6. **Update `docs/ui/SNAPSHOT_COVERAGE.md`** — every target updates the row's coverage status.

Failure to follow this policy in Phase 15+ will accumulate untraceable fixtures — the brief calls this out as anti-pattern.
