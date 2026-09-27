# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-26** under user-authorized package
> `claim-quad-c-155-156-160-28-2026-09-26` (approved plan
> `.ai/plans/quad-c-155-156-160-cloudseeding.md`, STATUS: APPROVED BY USER).
> The premise is constructed, consumed by the host, persisted where stateful,
> and proven by a focused headless probe in current source. Evidence in
> `INTEGRATION_PLANS.md`.

# PLAN-CLOUD-SEEDING-HOST-TRUTH — Weather Seeding Instrument Host Integration

**Kind:** GAP SEALING · **Status:** APPROVED BY USER
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28 (weather authority), PLAN-INVENTORY-CONSERVATION-93.
**Claim:** `claim-quad-c-155-156-160-28-2026-09-26`

## 1. Outcome
`World/CloudSeedingSystem.cs` is host-unreachable: the authored instrument
(install gate, preflight, deterministic deploy, cooldown, partial protection)
is never constructed by the game. This plan binds it to the canonical weather
owner and gives it one persisted state.

| Deliverable | Detail |
|---|---|
| Host owner | `CloudSeedingHostSession` binds `WeatherSystem` (canonical weather owner), inventory, and a seeded RNG fork |
| Save truth | own checksummed `cloud_seeding` section; install day, cooldown, last target, partial protection |
| Daily tick | cooldown and partial-protection decay ride the expanded-shelter day |
| Reachability | probe `--cloud-seeding-selftest` proves install → preflight → deploy → cooldown → save round-trip |

## 2. Evidence
- `Assets/Ashfall.Core/World/CloudSeedingSystem.cs` (353 lines; zero `src/` references).
- `src/Main.World.cs` owns `WeatherHostSession` / `WeatherSystem`.
- `src/Main.CloudSeeding.cs`, `src/Host/CloudSeedingHostSession.cs`, `src/Host/HostCli.CloudSeeding.cs`.

## 3. Non-goals
No second weather owner; no new climate simulation; no authored catalog.
