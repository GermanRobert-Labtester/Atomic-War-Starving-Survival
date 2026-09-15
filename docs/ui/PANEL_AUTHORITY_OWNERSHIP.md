# Panel Authority Ownership — Live player surfaces and their campaign authorities

> **Plan:** `docs/plans/C1_planintegration[2].md` §16B.1 (Plan 16 — Honest Navigation).
> **Derived from:** current source (revision `87b199b2` + this package's edits, 2026-09-15).
> **Rule:** one authority per fact (INV-16.1). A player-routed panel may only read/write
> the same instance the campaign day loop ticks and the save system captures.
> Panels never construct campaign authorities at bind time; they resolve them
> through the `Ensure*`/`Setup*` composition seams, which fail loudly when the
> campaign owner is absent (INV-16.3).

## Ownership table

| panel_id | authority_type | construction_owner | campaign_field_or_service | tick_owner | save_capture | save_restore | host_session | rebind_on_load |
|---|---|---|---|---|---|---|---|---|
| `fire_incident` | `ShelterFireHazardSystem` (Core) | `Main.SetupShelterFireHazard()` — `src/Main.ShelterInfrastructure.cs:588`, guarded once-only, applies `ShelterFireSaveStore.TryLoad()` on construct | `_shelterFireHazard` / `_shelterFireSession` | `ShelterFire` day owner — `_shelterFireSession!.TickDay(...)` at `src/Main.CampaignOwners.cs:484` | `SaveShelterFire()` → section `shelter_fire` | `ShelterFireSaveStore.ApplyToSystem` at construction; lifecycle restore path | `ShelterFireHostSession` (single, campaign-owned) | Yes — reset region unbinds `_fireIncidentPanel` and nulls the session (`src/Main.Lifecycle.cs:452-456`); bind action re-resolves at next open |
| `faction_matrix` | `FactionStanceEngine` (Core) — the foundry guild engine, never a panel-local instance | `Main.EnsureSharedFactionStance()` — `src/Main.CampaignServices.cs:188`; resolves `_silentFoundry.GuildStanceEngine` (composed by guarded `SetupSilentFoundry()`, `src/Main.Economy.cs:181`); throws a descriptive error when no campaign-owned engine exists | `_silentFoundry` (`SilentFoundryHostSession`) | `GreenhouseFoundryDayOwner` (campaign day loop, phase 2 — `src/Main.CampaignOwners.cs:38,285`) composes/ticks the owning foundry session; stance moves through treaty consequences in that session | section `silent_foundry` (`SaveSilentFoundry` / `SetupSilentFoundry`) | foundry session restore (same section) | `SilentFoundryHostSession` | Yes — both faction panels bind the same `EnsureSharedFactionStance()` instance; identity between `faction_matrix` and `factions_narrative` holds by construction |
| `factions_narrative` | `FactionStanceEngine` (Core) — same instance as `faction_matrix` | `Main.EnsureSharedFactionStance()` (same seam) | `_silentFoundry` | same as `faction_matrix` | same as `faction_matrix` | same as `faction_matrix` | `SilentFoundryHostSession` | Yes — `ReferenceEquals(factionMatrix.Authority, factionsNarrative.Authority)` holds by construction; no duplicate stance engine survives composition |
| `skill_matrix` | `SkillProgressionSystem` (Core) | `Main.EnsureSharedSkillProgression()` — `src/Main.CampaignServices.cs:167`, cached `_sharedSkillProgression`, skills registered from catalog | `_sharedSkillProgression` | `Main.TickSharedSkillProgression(day)` — `src/Main.CampaignServices.cs:150`, called from the shelter-facilities day owner immediately before the apprenticeship tick | rides the `apprenticeship` section — `state.skillProgression = EnsureSharedSkillProgression().CaptureState()` (`src/Main.ShelterSocial.cs:499`) | `appSkills.RestoreState(appState.skillProgression)` (`src/Main.ShelterSocial.cs:482`) | none (plain Core system; progression state persists via the apprenticeship section) | Yes — bind action re-resolves `EnsureSharedSkillProgression()` each open; instance state restored through the apprenticeship section on load |
| `weather_sonde` | `WeatherSystem` (Core) — the world's weather, identical reference to `weather_forecast` / `weather_detail` | `Main.SetupWeatherSonde()` — `src/Main.World.cs:201`, guarded once-only wrapper around `_world?.Weather` (composed by guarded `SetupWorld()`) | `_world.Weather` / `_weatherSondeHost` | world weather day owner (`world_weather` lifecycle participant; weather ticked by the world session in the campaign day loop) | `SaveWorld()` → section `world` | world section restore (world session) | `WeatherHostSession` (sounding catalog + recovery inventory wrapper; **not** a second weather authority) | Yes — `world_weather` participant onReset nulls `_world` and (this package) `_weatherSondeHost`, so the wrapper can never outlive its campaign; next open rebuilds it around the new world's `WeatherSystem`. Identity with `weather_forecast` holds by construction (both bind `_world?.Weather`) |
| `geiger_calibration` | `DosimeterCalibrationSystem` (Core) via `DoseLedgerHostSession.Calibration` | `Main.SetupDoseLedger()` — `src/Main.Phase0.cs:268` (`DoseLedgerHostSession.Create(_dataDir)`), applies `DoseLedgerSaveStore.TryLoad()` on construct | `_doseLedger` | dose ledger day owner (dose lifecycle participant; `dose_ledger` section registered in the save registry) | `SaveDoseLedger` → section `dose_ledger` (`dose_ledger_save.json`) | `DoseLedgerSaveStore.TryLoad()` + `RestoreSave` | `DoseLedgerHostSession` | Yes — dose participant reset nulls `_doseLedger` (`src/Main.Lifecycle.cs:323`); bind action re-resolves. Device selection is live: `ResolveLiveDosimeterTag()` (this package) picks the ordinal-first registered device or an explicit empty no-device state — demo sealing (`SealDemoSurvivors`) is reachable only from the Phase-0 dev surface and selftests, never from the player route |
| `triangulation` | `SignalTriangulationSystem` (Core) via `RadioHostSession.Triangulation` | `Main.SetupRadio()` (guarded radio composition) | `_radio` | radio day owner (radio lifecycle participant; distress/trust/follow-up tick in the campaign day loop) | radio save V6 (`RadioSaveCodec`) incl. distress state | radio section restore (`RadioHostSession.RestoreSave`) | `RadioHostSession` | Yes — radio participant reset nulls `_radio` (`src/Main.Lifecycle.cs:185`); bind action re-resolves. Signal selection is live: `ResolveLiveTriangulationSignalId()` (this package) picks the ordinal-first `DistressSystem.ActiveSignals` entry or an explicit empty no-signal state |
| `expedition_camp` | `ExpeditionSystem` (Core) via `ExpeditionHostSession` | `Main.SetupExpeditions()` (guarded) | `_expeditions` | expedition day owner (`expeditions` lifecycle participant, depends on survivors+inventory) | section `expedition` | expedition section restore | `ExpeditionHostSession` | Yes — expeditions participant reset disposes/nulls `_expeditions` (`src/Main.Lifecycle.cs:103-108`); bind action re-resolves. Survivor selection is live: first roster entry or explicit empty (the `?? "surv_01"` fabricated fallback was removed by this package) |
| `shelter_barter` | `ShelterBarterSystem` (Core) over the campaign `Inventory` | `Main.EnsureShelterBarter()` — `src/Main.Plans147.cs:233`, guarded; composes `SetupInventory()` first (this package) so barter trades through the campaign inventory — the previous `?? new Inventory()` fabricated-substrate fallback is gone (INV-16.3) | `_shelterBarter` / `_inventory.Inventory` | economy/barter day path; RNG from `_campaignDay.Rng.Fork("shelter_barter")` (deterministic campaign stream) | `SaveShelterBarter` → section `shelter_barter` (`shelter_barter_save.json`, owner economy) | `ShelterBarterSaveStore` restore — `_shelterBarter.RestoreState(saved)` (`src/Main.Plans147.cs:257`) | none (plain Core system + caravan registration precedes state restore) | Yes — bind action calls `SetupInventory()` then binds the campaign inventory directly |

## Selection-resolution seams added by this package (INV-16.4)

- `ResolveLiveDosimeterTag()` — `src/Main.PlayerSurfaces.cs`; ordinal-first registered
  calibration device or explicit empty.
- `ResolveLiveTriangulationSignalId()` — `src/Main.PlayerSurfaces.cs`; ordinal-first
  active distress signal or explicit empty.
- Explicit empty states (N16.6): `GeigerCalibrationPanel` renders
  "Device: None registered / Status: No dosimeter devices registered" with all
  actions disabled; `TriangulationPanel` renders "Signal: None under
  direction-finding" with record/triangulate disabled; `ExpeditionCampPanel`
  renders its existing "Phase: Not in camp" state with actions disabled.

## Source-plan fixture-ID verdicts (current evidence)

| Fixture literal | Verdict | Evidence |
|---|---|---|
| `"inc_default"` | Already absent from live source | No occurrence in `src/` or `Assets/Ashfall.Core/UI/`; `FireIncidentPanel.Bind` resolves the active incident from live `Incidents` state |
| `"tag_1"` | Removed from player routes by this package | Was `src/Main.PlayerSurfaces.cs:508` + panel defaults; now live-resolved. Remains only in explicit demo/selftest paths (`DoseLedgerHostSession.SealDemoSurvivors`, Phase-0 dev surface, `PanelBindLifecycleSelfTest` harness params) |
| `"sig_distress"` | Removed from player routes by this package | Was `src/Main.PlayerSurfaces.cs:513` + panel defaults; now live-resolved. Remains only as a selftest harness parameter |
| `"sv_cohort_demo"` | Confined to explicit demo paths | Only in `DoseLedgerHostSession` `*Demo` methods and the unrouted Phase-0 dev surface `src/Dose/DoseRegisterSurface.cs`; no player-routed panel references it |
| fallback `"surv_01"` | Removed from player routes by this package | Was `src/Main.PlayerSurfaces.cs:489` (`expedition_camp` bind fallback); now explicit empty selection |

## Enforcement

`Ashfall.Core.Tests/UI/PlayerSurfaceBindingPurityGateTests.cs` (this package) bans
the exact fixture literals from `src/UI/**` and `src/Main.PlayerSurfaces.cs`, and
bans any Core-type / `*System` / `*Engine` / `*HostSession` construction inside
the player bind configuration. The scan scope deliberately excludes the
sanctioned demo/selftest paths listed above.

`PlayerSurfaceLivenessGateTests` / `PlayerSurfaceCoverageGateTests` /
`PanelRouteGateTests` / `PanelSubscriptionHygieneTests` /
`ProductionUiNoFabricatedFallbackGateTests` (pre-existing) continue to enforce
maturity classification, route↔descriptor parity, and subscription hygiene.

## Known flagged items (foreman decisions needed, not fixed here)

1. **`EnsureShelterBarter` pre-campaign RNG fallback** — `new SeededRng(147)`
   when `_campaignDay` is null (`src/Main.Plans147.cs`). In-game the campaign
   stream is always used; the fallback only fires pre-campaign. Whether barter
   should be constructible before a campaign exists is a product decision.
2. **`stationId` default `"station_alpha"`** on
   `RadioHostSession.RecordObservation` — every player-recorded triangulation
   observation currently carries this synthetic station identity because the
   panel has no station selection. A station-selection seam is a design change,
   not a Plan 16 repair.
3. **Long-lived shared instances across new-game** — `_silentFoundry` /
   `_sharedFactionStance` / `_sharedSkillProgression` are not nulled by any
   lifecycle participant; they survive session swaps and have their state
   restored through their save sections (load path). Whether new-game (as
   opposed to load) fully resets their state should be verified by the owning
   foundry/apprenticeship packages before Plan 16 claims 16B.9 closed for them.
