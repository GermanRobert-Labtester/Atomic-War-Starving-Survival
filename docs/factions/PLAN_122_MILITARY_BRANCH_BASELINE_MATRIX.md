# Plan 122 — Military Branch Baseline Matrix & Forensic Audit

> **Catalog Authority:** `Assets/StreamingAssets/Data/military_faction_branch.json`
> **Source Baseline:** Indices 0 through 7 (Branches 1–8)

---

## 1. Existing Eight Branches Baseline Matrix

| # | Branch ID | Display Name | Role / Archetype | PONR Trigger Summary | Entry Band Range | Endings | Ending Band Coverage |
|---|---|---|---|---|---|:---:|---|
| **1** | `branch_mil_1_loyal_soldier` | The Loyal Soldier | Line discipline & obedience | Ordered to act against judgment; comply or refuse for the last time. | `slightly_evil` .. `slightly_positive` | 3 | `positive`..`very_positive` (1a)<br>`very_evil`..`evil` (1b)<br>`slightly_evil`..`slightly_positive` (1c) |
| **2** | `branch_mil_2_defector` | The Defector | Armed faction switching | Formally sworn into rebel ranks; marked as a traitor. | `evil` .. `positive` | 3 | `slightly_positive`..`very_positive` (2a)<br>`very_evil`..`evil` (2b)<br>`slightly_evil`..`neutral` (2c) |
| **3** | `branch_mil_3_opportunist` | The Opportunist | Self-enrichment & betrayal | Betray someone who trusted you for personal advantage. | `very_evil` .. `very_positive` | 3 | `very_evil`..`very_evil` (3a)<br>`positive`..`very_positive` (3b)<br>`evil`..`slightly_positive` (3c) |
| **4** | `branch_mil_4_martyr` | The Martyr | Sacrificial self-destruction | Make an irreversible personal sacrifice for others. | `positive` .. `very_positive` | 3 | `very_positive`..`very_positive` (4a)<br>`positive`..`positive` (4b)<br>`very_evil`..`evil` (4c) |
| **5** | `branch_mil_5_tyrant` | The Tyrant | Authoritarian power grab | Seize unilateral command with no superior left to answer to. | `very_evil` .. `evil` | 3 | `very_evil`..`very_evil` (5a)<br>`slightly_evil`..`evil` (5b)<br>`neutral`..`neutral` (5c) |
| **6** | `branch_mil_6_reformer` | The Reformer | Internal institutional policy change | Force through an administrative policy change that cannot be reversed. | `neutral` .. `very_positive` | 3 | `very_positive`..`very_positive` (6a)<br>`positive`..`positive` (6b)<br>`neutral`..`slightly_positive` (6c) |
| **7** | `branch_mil_7_deserter` | The Deserter | Walking away into the wastes | Walk away from post for the last time; name struck from rolls. | `evil` .. `positive` | 3 | `neutral`..`slightly_positive` (7a)<br>`very_evil`..`evil` (7b)<br>`positive`..`very_positive` (7c) |
| **8** | `branch_mil_8_broken_chain` | The Broken Chain | Systemic organizational collapse | Command structure collapses around you and does not reform. | `very_evil` .. `very_positive` | 3 | `slightly_positive`..`very_positive` (8a)<br>`slightly_evil`..`neutral` (8b)<br>`very_evil`..`evil` (8c) |

---

## 2. Role Duplication & Archetype Differentiation Analysis

The initial planning draft proposed working labels such as "The Deserter" and "The Reformer" for new branches. A forensic audit of the existing baseline revealed that:
1. **Branch 6 is already "The Reformer" (`branch_mil_6_reformer`):** Centered on administrative and systemic policy transformation.
2. **Branch 7 is already "The Deserter" (`branch_mil_7_deserter`):** Centered on a solitary soldier walking away into the wilderness.

To avoid duplicate titles, ambiguous IDs, or redundant narrative arcs, Plan 122 sharpens the 7 new archetypes into distinct institutional roles:

- **The Quartermaster (Branch 9):** Owns physical resources, inventory ledgers, and logistics ethics.
- **The Combat Medic (Branch 10):** Owns battlefield medical ethics, patient status, and unconditional care.
- **The Conscript Parent (Branch 11):** Owns the direct collision between compulsory state military service and family survival in the local shelter.
- **The Intelligence Officer (Branch 12):** Owns knowledge control, source protection, classified archives, and report alteration.
- **The Peacekeeper (Branch 13):** Owns crowd security, de-escalation, and refusal to authorize lethal force.
- **The Fugitive Deserter (Branch 14):** Distinct from Branch 7's solitary wanderer; focuses on a soldier seeking sanctuary inside a civilian shelter while evading military provost recall.
- **The Dissident Officer (Branch 15):** Distinct from Branch 6's administrative policy reformer; focuses on an operational commander issuing a direct, witnessed counter-order halting combat action.

This differentiation ensures that all 15 branches represent non-overlapping institutional dilemmas.
