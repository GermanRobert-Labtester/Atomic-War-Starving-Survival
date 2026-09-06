# ASHFALL — Patrol Dynamic Territory Matrix & Authority Specification

**Milestone:** Flagship Integration Plan VII (F12)
**Authority:** Engine-agnostic Core (`Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs`, `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`)
**Status:** Integrated & Verified

---

## 1. Executive Summary

Wasteland territory is fluid. Factions seize choke points, establish fortifications, and lose perimeter nodes to rivals. Armed patrols do not operate in a vacuum: patrols requiring territorial backing spawn only where their parent faction maintains an active claim or military control.

---

## 2. Core Contract: `ITerritoryAuthority`

```csharp
public interface ITerritoryAuthority
{
    bool IsClaimedBy(string locationId, string factionId);
    TerritoryNodeState GetTerritoryState(string locationId);
    string? GetController(string locationId);
    bool IsClaimant(string locationId, string factionId);
}
```

### Eligibility Rules by Territory Node State

When an encounter defines `required_territory_owner = "faction_x"`:

| Node State | Claim / Control Condition | Eligibility for `faction_x` Patrol |
|---|---|---|
| `None` / `Unclaimed` | No faction presence | **Ineligible** (patrol cannot operate without logistics base). |
| `Claimed` | Faction has staked claim | **Eligible** if `faction_x` is the claimant. |
| `Contested` | Multiple factions disputing | **Eligible** if `faction_x` is one of the competing claimants. |
| `Controlled` | Single faction consolidated | **Eligible ONLY** if `faction_x` is the sole controller. Hostile factions are barred from spawning patrols here. |

---

## 3. Region to Canonical Node Mapping

When encounter checks occur at the travel region level (e.g., standard travel or caravan regional movement), `PatrolTerritoryResolver` maps the region to its canonical strategic choke point:

| Wasteland Travel Region | Canonical Location ID | Primary Strategic Feature | Default Initial Controller |
|---|---|---|---|
| `the_toll` | `loc_toll_house` | Heavily fortified toll bridge and culvert network | `warlords_sector_4` |
| `high_scarp` | `pass_mount_karkov` | Mountain pass guarding eastern approaches | `iron_garrison` |
| `industrial_belt` | `loc_denial_cut_substation` | Power distribution and foundry railway junctions | `faction_ordnance_foundry` |
| `dead_suburbs` | `settlement_13` | Residential ruins with deep aquifer access | `faction_hydro_barons` |

If an encounter check supplies an explicit `locationId` (e.g. caravan arrived at a specific waypoint node), that node ID is evaluated directly without falling back to the regional anchor.

---

## 4. Territory Patrol Matrix

| Encounter ID | Faction | Required Territory Owner | Valid Territory States | Authoring Regions |
|---|---|---|---|---|
| `enc_patrol_warlord_raid` | `warlords_sector_4` | `warlords_sector_4` | Contested, Controlled | `the_toll`, `industrial_belt` |
| `enc_patrol_warlord_advancing` | `warlords_sector_4` | `warlords_sector_4` | Contested, Controlled | `the_toll`, `industrial_belt` |
| `enc_patrol_central_garrison_border` | `faction_central_garrison` | `faction_central_garrison` | Border, Controlled | `high_scarp` |
| `enc_patrol_garrison_checkpoint` | `iron_garrison` | *(unrestricted)* | Controlled | `high_scarp` |
| `enc_patrol_railway_convoy` | `faction_railway_guild` | *(unrestricted)* | Controlled, Contested | `the_toll` |
| `enc_patrol_hydro_escort` | `faction_hydro_barons` | *(unrestricted)* | Controlled | `dead_suburbs` |
| `enc_patrol_supply_corps_convoy` | `faction_supply_corps` | *(unrestricted)* | Controlled | `high_scarp`, `dead_suburbs` |

*Unrestricted patrols represent roaming trade convoys, mutual-aid missions, or scouting units operating beyond strictly claimed sovereign borders.*

---

## 5. Dynamic Reaction & Immediate Invalidation

`DynamicTerritoryAuthority` (and `WarlordDoctrineSystem`) dispatches change notifications (`OnTerritoryChanged`).
When a node flips control (e.g. Garrison forces seize `loc_toll_house` from `warlords_sector_4`):
1. Warlord raid patrols immediately cease appearing in `the_toll`.
2. Garrison border inspections and checkpoints immediately become eligible.
3. Zero latency or stale cache retention. Verified by `PatrolTerritoryIntegrationTests`.
