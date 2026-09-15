# Plan 167 — Espionage Consequence Routing Map

**Debt:** `DEBT-PLAN167-CONSEQUENCE-ROUTING`  
**Status:** SEALED 2026-09-12  
**Package:** `DEBT-167-CONSEQUENCE-ROUTING`

## Contract

`EspionageSystem` remains the sole emitter of `EspionageConsequenceIntent`.
A thin `EspionageConsequenceRouter` routes each typed intent once into one
canonical consumer. No parallel consequence ledger.

| Consequence ID | Canonical consumer | Method | Idempotency | Save boundary |
|---|---|---|---|---|
| `consequence_supply_disruption` | `CaravanTradeNetworkSystem` | `TryApplySupplyDisruption(factionId, magnitude, startDay, endDay, sourceId)` | `sourceId = incidentId` | `caravan_trade_network` disruption rows |
| `consequence_communications_disruption` | `PsyOpsSystem` | `StartJamming(factionId, strength=magnitude, days=max(1,expiry-start), day=start)` | Router fired-set by `incidentId` (PsyOps merges by faction) | `psyops` jamming rows + espionage fired-set |
| `consequence_defense_readiness_reduced` | `FactionWarSystem` | `TryApplyDefenseReadinessPressure(factionId, magnitude, startDay, endDay, sourceId)` | `sourceId = incidentId` | `year_of_ash` / FactionWar pressure rows |

## Rejected owners

- `ShelterEspionageSystem` — Plan 51 inbound shelter infiltration, not outbound sabotage
- `PerimeterDefenseSystem` / muster readiness — player shelter defenses
- `FactionEmbargoLedger` — suspends player trade (inverted semantic)
- `CampaignConsequenceLedger` — audit/flags only; optional secondary record, never sole effect

## Fired-incident set

Persisted on `EspionageState.firedConsequenceIncidentIds`. Restore never
re-applies already-fired incidents. Router ignores unknown consequence types
explicitly (structured miss, not silent invent).

## Focused verification

`Ashfall.Core.Tests/Plan167EspionageConsequenceRoutingTests.cs`
