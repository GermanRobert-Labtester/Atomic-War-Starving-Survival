# Utility Action Balance Audit

> **Balance Evaluation:** Urgency tiers, score distributions, and dominant-action prevention across the 20 actions.

---

## 1. Urgency Tiers & Score Spread

| Tier | Category | Actions Included | Expected Score Range |
|---|---|---|---|
| **Tier 1: Acute Emergency** | Medical / Danger | `action_treat_wounded`, `action_seek_treatment` | 0.65 – 1.00 (or >1.0 on override) |
| **Tier 2: Basic Sustenance** | Food / Water / Rest | `action_cook_food`, `action_purify_water`, `action_rest` | 0.45 – 0.75 |
| **Tier 3: Vital Maintenance** | Repair / Security | `action_repair_equipment`, `action_stand_watch`, `action_resolve_conflict` | 0.40 – 0.65 |
| **Tier 4: Administrative / Preserves** | Logistics / Storage | `action_weigh_goods`, `action_canvas_support`, `action_preserve_food`, `action_inspect_housing`, `action_audit_inventory` | 0.30 – 0.50 |
| **Tier 5: Discretionary Slack** | Study / Social | `action_conduct_research`, `action_train_skill`, `action_teach_skill`, `action_socialize`, `action_read_contract`, `action_run_vouch`, `action_file_report` | 0.20 – 0.45 |

---

## 2. Dominance Audit

- **No Single Action Dominates:** Even high-scoring actions like `action_repair_equipment` or `action_treat_wounded` drop to 0 when their prerequisites are satisfied (no wounded patients, no degraded tools, or high fatigue exceeding `fatigueGate`).
- **Fatigue Self-Regulation:** Working survivors steadily gain fatigue. When fatigue surpasses 75–85, work actions gate to 0, leaving `action_rest` (which has `fatigueGate: 0.0`) as the natural victor.
