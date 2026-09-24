# UNBLOCK — Plans 167 & 169: Underground Tunnel Network + Audio Accessibility

**Status:** DONE 2026-09-24 (integrator, user-authorized).
**Claim:** `claim-unblock-plans-167-169-2026-09-24`.
**Evidence:** `--tunnel-network-selftest` 12/12, `--audio-accessibility-selftest` 12/12,
host + Core builds 0 errors / 0 warnings.

## Premise (verified in source before editing)

Both plans were listed as partials with **0 host references**.

- **Plan 167** — `TunnelNetworkSystem` existed and was already wrapped by
  `WastelandMapSystem.Tunnels`, so the authority was correct and single. The real
  gaps: the authored `underground_tunnels.json` was **never loaded** (the map
  seeded only a hardcoded two-segment network) and `TunnelNetworkSystem.TickDay`
  was **never called**, so structural wear/collapse never occurred.
- **Plan 169** — `AudioAccessibilityCoordinator` and
  `audio_accessibility_cues.json` existed but had **zero host references**: no
  catalog load, no callback to `AudioManager`, no probe.

## Plan 167 — Underground Tunnel Network

**Core**
- `TunnelNetworkCatalogLoader` (new, strict): rejects unsupported schema,
  empty/duplicate junction or segment ids, empty names, missing or self
  endpoints, `length_hours < 0.5`, `difficulty` outside 1..5, `integrity`
  outside 0..100, unknown hazard tokens, and junction → missing-segment
  references.
- `TunnelNetworkSystem.Clear()`, `GetCensus()`, and a schema-gated
  `RestoreState` (a payload omitting the field is the v1 shape and still loads;
  a newer schema throws).
- `WastelandMapSystem` accepts an optional authored catalog and seeds it in
  place of the built-in canonical network. `WastelandMapCatalogLoader.CreateSystem`
  loads `underground_tunnels.json` through the strict loader. A restored
  campaign replaces the seeded state before any tick, so a player's
  discovered/repaired network survives.

**Host**
- `src/Main.TunnelNetwork.cs`: `SetupTunnelNetwork`, `TickTunnelNetwork`,
  `GetTunnelNetworkCensus`, `ReinforceTunnelSegment`, `ClearTunnelHazard`,
  `EvaluateTunnelBypass` — all routing through the canonical
  `WastelandMapSystem.Tunnels`. No second store: the network persists inside the
  existing `world_map` save section.
- Phase-5 day owner `tunnel_network` with `IPreDaySnapshotRestore`, so the daily
  structural wear runs inside the fail-closed advance and rolls back on failure.
- `tunnel_network_ticked` classified as an internal heartbeat.
- CLI probe `--tunnel-network-selftest` (12 checks).

## Plan 169 — Audio Accessibility & Mix Legibility

**Core**
- `AudioAccessibilityCatalogLoader` (new, strict): rejects unsupported schema,
  empty/duplicate cue or preset ids, empty bus name, empty visual label,
  unknown severity, ducking outside -24..0 dB, negative coalesce window, and
  preset values outside -24..0 dB.
- `AudioAccessibilityCoordinator.BindCatalog(AudioAccessibilityCatalogData)` and
  `GetCensus()`.

**Host**
- `AudioAccessibilityHostSession` binds the coordinator's seams to delegate
  sinks (ducking, mix preset, visual notification) and gates only the visual
  layer on the user preference — the audio cue still plays and ducks.
- `src/Main.AudioAccessibility.cs`: loads the authored catalog, applies the
  persisted preset on setup, and translates canonical day facts
  (`power_critical_deficit`, `power_brownout_began`, `medical_admitted`,
  `radio_intercept`, `radio_intercept_decrypted`, `expedition_milestone`,
  `raid_incoming`) into critical cues through the `CampaignDayCoordinator`
  `OnDayAdvanced` bridge. No gameplay decision lives here.
- `AudioManager.ApplyAccessibilityDucking` / `ApplyAccessibilityMixPreset`:
  ducking attenuates background buses only (alerts, voice, medical, radio stay
  legible); presets apply a bounded master-gain offset.
- `UserSettingsData.VisualAudioAlerts` (+`AudioMixPreset`) persist the
  preference through the single existing settings authority. No second store.
- CLI probe `--audio-accessibility-selftest` (12 checks).

## Deferred with named reasons

- Tunnel UI beyond the existing `MapPanel` tunnel card and a dedicated
  reinforce/clear-hazard panel (presentation).
- Geographic tunnel discovery from surveying is already canonical in
  `WastelandMapSystem`; no new graph was added.
- The audio preset's high-frequency attenuation is recorded but applied as a
  bounded master offset rather than a dedicated shelf filter (mixer effect
  ownership belongs to `AudioManager`).
- Authored feedback-toast copy for audio notifications (presentation; the host
  currently logs the notification and exposes `VisualNotificationSink`).

## Verification

```
host + Core builds: 0 errors / 0 warnings
--tunnel-network-selftest 12/12        --audio-accessibility-selftest 12/12
--data-integrity-selftest PASS         --port-contract-selftest PASS (293 seams)
--7-day-smoke-selftest PASS            --selftest-manifest PASS
Ashfall.Core.Tests/Underground/ 24/24  Ashfall.Core.Tests/Audio/ 47/47
Ashfall.Core.Tests/World/ 544/544      Ashfall.Core.Tests/Settings/ 34/34
```
