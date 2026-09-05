# Utility Action Content Utilization

> **Content Utilization:** Audit against orphaned, unreachable, or dead utility actions.

---

## 1. Action Reachability Audit

Every action in the expanded 20-action catalog is fully reachable under realistic game conditions:

| Action ID | Winning Condition / Scenario |
|---|---|
| `action_weigh_goods` | Depot freight arrival, Osran Kell companion bias active, low fatigue |
| `action_read_contract` | Legal/pact negotiations, The Tally companion bias active |
| `action_canvas_support` | Political faction dispute, Amnesty petition campaign active |
| `action_run_vouch` | Refugee arrival at shelter gate, Standing Record bias |
| `action_audit_inventory` | Post-expedition cargo sorting, routine depot count |
| `action_file_report` | Shift conclusion, quiet bookkeeping in archive alcove |
| `action_repair_equipment` | Damaged workbench tools, skilled mechanic with scrap materials |
| `action_inspect_housing` | Structural settling / minor tremor aftermath |
| `action_treat_wounded` | Medical trauma, survivor injured during expedition or incident |
| `action_seek_treatment` | Wounded survivor moving to clinic triage bed |
| `action_cook_food` | Low cooked ration supply, kitchen powered, cook on duty |
| `action_preserve_food` | Surplus crop harvest, spoilage risk mitigation |
| `action_purify_water` | Low clean water reservoir, water treatment plant powered |
| `action_socialize` | Low morale, idle downtime in mess hall |
| `action_resolve_conflict` | Interpersonal tension between roommates |
| `action_train_skill` | Safe idle downtime, survivor practicing craft |
| `action_teach_skill` | High-skill survivor coaching apprentice in workshop |
| `action_stand_watch` | Elevated perimeter threat, guard sentry post |
| `action_conduct_research` | Active tech project, scientist working at lab terminal |
| `action_rest` | Exhausted survivor (fatigue > 75), recovering in dormitory bunk |

---

## 2. Zero Dead Content

- All 20 actions possess positive `baseScore` and `weight`.
- No action has impossible prerequisite gates or unreachable curves.
- `ContentUtilizationScanner` records `utility_actions.json` as `GAMEPLAY_CONSUMED`.
