# Utility Action Existing 6 Audit

> **Forensic Audit:** Detailed analysis of the 6 baseline actions landed in `utility_actions.json`.

---

## 1. Inventory of Baseline Actions

| ID | Display Name | Base Priority | Weight | Override? | Tags | Base Score | Fatigue Gate | Skill Factor |
|---|---|---|---|---|---|---|---|---|
| `action_weigh_goods` | Weigh Goods | 0.1 | 1.0 | No | `["loud_labor"]` | 0.40 | 85.0 | 0.25 |
| `action_read_contract` | Read Contract | 0.1 | 1.0 | No | `[]` | 0.35 | 90.0 | 0.20 |
| `action_canvas_support` | Canvas Support | 0.1 | 1.0 | No | `["menial_labor"]` | 0.45 | 80.0 | 0.15 |
| `action_run_vouch` | Run Vouch | 0.1 | 1.0 | No | `[]` | 0.30 | 88.0 | 0.10 |
| `action_audit_inventory` | Audit Inventory | 0.1 | 1.0 | No | `["quiet_labor"]` | 0.35 | 80.0 | 0.00 |
| `action_file_report` | File Report | 0.1 | 1.0 | No | `["quiet_labor"]` | 0.35 | 80.0 | 0.00 |

---

## 2. Functional Roles & Behavioral Gaps

1. **Crossing Companion Heritage:** The first four actions (`action_weigh_goods`, `action_read_contract`, `action_canvas_support`, `action_run_vouch`) mirror the companion biases for Osran Kell, The Tally, the Amnesty Campaign, and Standing Record.
2. **Administrative Bias:** The last two actions (`action_audit_inventory`, `action_file_report`) represent sedentary depot bookkeeping.
3. **Severe Behavioral Gaps:**
   - **Zero physical shelter maintenance:** No equipment repair, fixture overhaul, or housing checks.
   - **Zero medical care:** No triage response for wounded survivors or clinic check-ins.
   - **Zero sustenance loops:** No cooking, food preservation, or water purification.
   - **Zero social life:** No communal conversation or conflict mediation.
   - **Zero skill growth:** No self-practice or mentoring.
   - **Zero security:** No perimeter watch or sentry duty.
   - **Zero research:** No analytical science or technical study.
   - **Zero rest:** No fatigue recovery in dormitory bunks.

Plan 72 fills these exact gaps with 14 purpose-built actions.
