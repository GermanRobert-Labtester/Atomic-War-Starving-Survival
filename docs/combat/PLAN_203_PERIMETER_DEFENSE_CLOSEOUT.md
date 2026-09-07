# Plan 203 — Wasteland Perimeter Defensive Grid Extension — Closeout

**Flagship:** Plans 202–205. **Status: IMPLEMENTED & VERIFIED** (commit `17dd8297`, Wave F hardening in the flagship integration log).

## Starting point

`PerimeterDefenseSystem` + `perimeter_defenses.json` already owned construction,
ammo, barrel wear/jam, repair, the deterministic assault simulation, and raid
integration via `DefenseSystem` — extension, not replacement, per the
reconnaissance (creating the proposed second catalog would have forked the
data authority).

## Added contracts

- **Sector topology (§6.4):** canonical graph (north/east/south/west/gate);
  new emplacements auto-assign round-robin; `AssignEmplacementToSector`
  reassignment; sector state is save-carried (`schema_version` 1→2, additive).
- **Alert lifecycle (§6.7):** triggered devices **spend** — stealth denial and
  re-trigger are suppressed until `RESET` (free player action); `ARM/DISARM`
  toggle; suppressed while disarmed.
- **False alarms (§6.8):** seeded → day-derived per-device daily roll
  (`false_alarm_rate_bp`); spends the device, writes the log, never spawns
  combat.
- **Weather wear (§6.11):** severe-weather days (host-projected from the
  single weather authority: ashfall/fallout/blizzard/black-rain/acid-snow/
  black-snow/blood-rain) wear intact emplacements by
  `weather_wear_per_storm`; destroyed stay destroyed; repair flow unchanged.
- **Counterplay (§6.10):** `counter_tags` (cutting_tools/explosives/stealth/
  vehicle_breach/emp) neutralize matching defenses — optional
  `attackerCounterTags` parameter on the assault sim and encounter snapshot;
  existing callers unchanged. The tripwire line counters `stealth`, keeping
  stealth-archetype raiders viable against detection devices.
- **Encounter snapshot (§6.9):** stealth-denied, integrity-scaled movement
  delay, detection initiative, protected sectors, countered list — combat
  consumes context, never writes perimeter state (Trap C).
- **Intrusion log (§6.14):** bounded 32 entries
  (false_alarm/hostile_trigger/breach/repelled).

## Save / migration

Additive fields only; old saves restore with no sectors, no log (legacy
stealth-denial fallback = historical always-on behavior), no free maxed grid.
The `perimeter_defense` campaign section is reused unchanged.

## Verification

15 tests (`PerimeterDefensePlan203Tests`) + Wave F cross-plan replay.
The daily tick and jam rolls are day/sequence-derived (Wave F fresh-seed
pattern) — split-run convergence proven in
`Plans202To205CampaignIntegrationTests` (continuous vs day-15 and
multi-point splits over 30 days).

## Abstraction boundary

Devices remain abstract gameplay hardware (as the existing catalog already
modeled); no trigger construction, no anatomical mechanics — obstacle contact
expresses movement impairment and breach difficulty only (§6.6).
