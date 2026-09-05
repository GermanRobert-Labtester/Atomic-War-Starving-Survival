# Foundry Treaty Faction War Handoff Contract

**Target System:** `Assets/Ashfall.Core/FactionWar/` / `FactionWarSystem.cs`
**Host Hook:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Treaty Breach as a War Pretext

In ASHFALL, regional warfare between major wasteland factions is mediated through tension meters, border demilitarization pacts, and treaty compliance.

When a consequence policy with outcome `violated` fires:
1. `standing_delta` severely lowers trust (e.g. `-10.0` to `-12.0`).
2. The host session passes the breach record to `FactionWarSystem`:
   ```csharp
   _factionWar.RecordTreatyBreach(record.treatyId, record.factionId, record.appliedDay);
   ```
3. The affected faction gains a diplomatic *casus belli* (war pretext) against the breaching signatory.

---

## 2. Specific Treaty Breach Triggers

| Treaty ID | Breaching Signatory | Affected Signatories | War Consequence |
|---|---|---|---|
| `treaty_garrison_grain_tithe_compact` | `faction_central_garrison` / default | `faction_rebuilders` | Eastern Arterial Road blockaded; Central Garrison sets up armed perimeter checkpoints, seizing civilian grain carts. |
| `treaty_switchback_fuel_and_passage_accord` | `faction_ash_sign` / default | `faction_forward_roster` | Mountain rockfalls initiated; Forward Roster patrols clash with Ash Sign sentries along the snowline. |
| `treaty_deep_coast_aquifer_protection_treaty` | `faction_the_fleet` / default | `faction_rebuilders` | Toxic bilge discharge triggers naval blockade of Flotilla coastal anchorages by Rebuilder militia. |
| `treaty_cluster_labour_schedule` | `faction_silent_foundry` | `faction_the_cutters`, `faction_the_office` | General strike and lockout on the charging floor sparks street riots in District 8. |

---

## 3. De-escalation Mechanism

Fulfilling treaty obligations (`outcome == "met"`) over consecutive assessment cycles restores standing and gradually reduces Faction War tension metrics, preventing total regional collapse.
