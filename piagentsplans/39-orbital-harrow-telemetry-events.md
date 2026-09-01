# Plan 39 — Orbital Harrow Telemetry Events (system exists, no data)

## Goal (2 lines)
Create `orbital_harrow_events.json` for `OrbitalHarrowTelemetrySystem` — the system is fully
implemented and save-supported but has **no event data** (verified: file missing). Add 12
telemetry events and 8 strike-consequence records that give the player warning time and
meaningful responses to orbital kinetic strikes.

## Why (P2)
- Verified: `OrbitalHarrowTelemetrySystem.cs` exists in Core; no event catalog exists.
- The telemetry system is the early-warning layer: without event data, the player gets no
  warning before strikes hit (Plan 38 armor has nothing to defend against in practice).
- Creates a time-pressure survival loop: detect → interpret → decide (reinforce / evacuate /
  accept damage) → consequence.

## Files to touch
- `Assets/StreamingAssets/Data/orbital_harrow_events.json` (CREATE — 12 events + 8 consequences)
- Read-only: `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` (confirm event schema:
  event id, signal type, detection window, impact coordinates, strike type, severity),
  `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` (confirm how events feed armor)
- Check loader: `grep -rn "orbital_harrow\|OrbitalHarrow\|harrow_event" Assets/Ashfall.Core/`

## Content grammar (per telemetry event)
- snake_case `id` with prefix `event_` or `telemetry_` (confirm accepted prefix).
- signal_type: radar_anomaly / seismic_precursor / radio_interference / thermal_signature /
  dead_hand_ping (the dead-hand system from W17 in roadmap 31).
- detection_window: ticks between detection and impact (the player's response window).
- strike_type: kinetic_rod / cluster / emp_burst / debris_fall (must match Plan 38 threats).
- severity: damage value passed to `SkyLayerArmorSystem`.
- false_positive_chance: some signals are noise — the player must interpret, not react blindly.

## Content grammar (per consequence)
- snake_case `id` with prefix `consequence_` or `event_` (confirm accepted prefix).
- trigger: armor_breach / armor_hold / evacuation / no_response.
- effects: shelter damage, radiation ingress, fire, structural collapse, survivor casualties,
  morale impact, electronics disruption (EMP).
- delayed_effect: some consequences manifest days later (structural weakness, radiation
  sickness, survivor trauma — feeds existing 09B/27C systems).

## Steps
1. Read `OrbitalHarrowTelemetrySystem.cs` end-to-end: confirm the event schema, the
   detection→impact timeline, the false-positive logic, and the save DTO shape.
2. Confirm how events feed into `SkyLayerArmorSystem` (Plan 38) — are they the same catalog or
   separate? Reconcile with Plan 38's threat events to avoid duplication.
3. Confirm loader status; if missing, add a mechanical loader.
4. Author 12 telemetry events: 4 confirmed kinetic-rod strikes (varying detection windows), 2
   cluster-strike warnings, 2 EMP-burst precursors, 2 dead-hand pings (ambiguous — could be
   drill or real), 2 false-positive radar anomalies.
5. Author 8 consequence records: armor_holds (minimal damage), armor_breaches (shelter
   damage + radiation), successful_evacuation (no casualties but shelter takes hit),
   no_response (full damage + casualties), delayed_structural_weakness, delayed_radiation,
   emp_electronics_failure, morale_trauma.
6. Wire 3 events into radio broadcasts (existing 24A schedule) — the player hears the
   dead-hand ping on shortwave before the telemetry system confirms it.
7. Validate: `--data-integrity-selftest`; confirm a detect → interpret → impact → consequence
   loop works in a headless boot; save round-trip for in-progress events.
8. xUnit: detection window fires correctly, false-positive logic is deterministic (seeded),
   consequences apply per trigger type, delayed effects fire on schedule, save round-trip green.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the Plan 38/39 overlap (threat events vs telemetry events) must be reconciled in
step 2. If they're the same catalog, merge; if separate, clearly delineate (armor threats
vs detection signals).

## Definition of Done
- `orbital_harrow_events.json` exists with 12 events + 8 consequences, all ids resolving,
  detect→impact→consequence loop works end-to-end, false-positive determinism pinned, delayed
  effects fire on schedule, save round-trip green, integrity + tests green.

## Follow-on
- Plan 38 (sky armor) — telemetry events are the warning layer; armor is the defense.
- Existing 19B (orbital strikes) — this plan provides the event data.
- W17 in roadmap 31 (dead-hand system) — dead-hand pings as ambiguous telemetry events.
- Existing 24A (radio schedule) — telemetry signals heard on shortwave before confirmation.
