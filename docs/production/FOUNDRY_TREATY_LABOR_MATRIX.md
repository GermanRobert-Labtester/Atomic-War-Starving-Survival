# Foundry Treaty Labor & Accord Matrix

This document maps treaty obligations, signatory quotas, and labor-compliance mechanics between the Silent Foundry and regional wasteland accords (`foundry_accords.json`, `foundry_treaty_consequences.json`, and `SilentFoundrySystem.TreatyLabor.cs`).

---

## 1. Accord Quota Mapping

| Treaty ID | Treaty Title | Signatory Factions | Product ID | Required Quota | Assessment Cycle | Non-Compliance Consequence |
|---|---|---|---|---|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | The Brine Pipe & Iodine Exchange | Silent Foundry, The Office | `foundry_prod_brine_pipe` | 4 units | 30 Days (Day 280) | -6 Office Standing; Iodine medication suspended |
| `treaty_road_iron_charter` | The Road Iron Charter | Silent Foundry, Cutters, Fleet | `foundry_prod_ice_anchor`<br>`foundry_prod_winch_drum` | 60 anchors<br>3 drums | 45 Days (Day 330) | -8 Cutters Standing; Ice road haulage tariff doubled |
| `treaty_cluster_labour_schedule` | The Cluster Labour Schedule | Silent Foundry, Office, Cutters | Clean Water Allocation | Shift rules | Continuous | Forfeits coal convoy window on the next ice |
| `treaty_the_cluster_charter` | The Cluster Charter | Silent Foundry, All Signatories | Open Incident Book | Zero open breaches | Annual (Day 365) | Revocation of official works status |

---

## 2. Gating and Labor Constraints

1. **High-Tier Treaty Lock**: Products carrying an active `treaty_id` cannot be poured if the signatory faction is hostile (`standing < -30`) or if a treaty-backed strike is active.
2. **Emergency Requisition**: Overriding a treaty quota to cast emergency shelter defense plates (`foundry_prod_roof_armor_plate`) triggers an immediate diplomatic protest and requires audit review at the weigh-hut.
