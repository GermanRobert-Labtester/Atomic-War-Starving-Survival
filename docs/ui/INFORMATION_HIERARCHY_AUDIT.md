# ASHFALL — Information Hierarchy, Causality & Decision Clarity Audit

**Audit Reference:** Plan 14 Task 14D
**Focus:** HUD signal competition, severity vocabulary, Glance → Inspect → Act flow, actionable disabled states.

---

## 1. Shared Semantic Severity Vocabulary

All systems map into a unified 5-tier severity model:

| Level | Semantic Meaning | Color Token | Icon Marker | HUD Behavior | Detail Behavior |
|---|---|---|:---:|---|---|
| **Normal** | State is healthy and stable | Pale (`#E6E0D2`) / Success (`#5CD670`) | `[OK]` | Standard telemetry readout | Full statistics displayed |
| **Attention** | Mild strain / trend declining | Warning (`#C97B3A`) | `[▲]` | Muted yellow badge on status rail | Trend warning + causal factor |
| **Dangerous** | Severe deficit / rapid degradation | Radiation (`#D9A026`) | `[!]` | Pulsing indicator; top of status rail | Specific system breakdown alert |
| **Critical** | Lethal condition / immediate loss | Critical (`#E63333`) | `[☠]` | Prominent warning banner + alarm audio | Direct action link to remedy panel |
| **Unavailable** | Action cannot be taken currently | Dim (`#66675F`) | `[X]` | Button disabled | Explicit prerequisite explanation |

---

## 2. Glance → Inspect → Act Navigation Flow

For every core survival pressure:

1. **Glance (HUD):**
   - Single-line status rail shows overall shelter condition, current hazard level, and survivor counts.
   - Distinct badges highlight which system requires immediate attention (e.g. `[RAD 38 mSv Mikhail]`, `[WATER < 3 Days]`).

2. **Inspect (Panel):**
   - Clicking or shortcutting to the panel (e.g. `MedicalPanel` or `InventoryPanel`) sorts endangered elements to the top.
   - Panel explains the root cause: e.g. "Acute Radiation Sickness: +5 HP/h decay from 38 mSv exposure".

3. **Act (Direct Control):**
   - Remedial action (e.g. "Administer Rad-Away", "Run Desalination Membrane") is directly clickable.
   - Disabled actions state why: "Requires 1 Rad-Away in inventory (Available: 0)".

---

## 3. HUD Signal Competition Resolution

- HUD alerts coalesce multiple similar events (e.g. 3 survivors hungry -> "3 Survivors Hungry [Rations Strained]" instead of 3 separate floating popups).
- High-severity alerts supersede low-priority status noise without blocking player view.
