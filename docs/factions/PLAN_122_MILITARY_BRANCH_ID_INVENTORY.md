# Plan 122 — Military Branch ID Inventory & Collision Forensics

> **Scope:** Complete namespace and identity audit for 15 Military branches, 15 PoNR flags, and 45 endings.
> **Source Authorities:** `military_faction_branch.json` & `MilitaryBranchIds.cs`

---

## 1. Collision Forensics on Working Draft IDs (Task 122S)

The initial planning draft proposed the following working IDs for new branches:
- `branch_mil_2_deserter`
- `branch_mil_3_reformer`
- `branch_mil_4_quartermaster`
- `branch_mil_5_medic_soldier`
- `branch_mil_6_conscript_father`
- `branch_mil_7_intelligence_officer`
- `branch_mil_8_peacekeeper`

### 1.1 Collision Analysis
An inventory of the existing baseline in `MilitaryBranchIds.cs` and `military_faction_branch.json` proved that numeric tokens `1` through `8` were already occupied:

| Draft Proposal | Existing Baseline ID | Conflict / Hazard Type | Resolution |
|---|---|---|---|
| `branch_mil_2_deserter` | `branch_mil_2_defector` | Numeric ID collision with Defector; semantic collision with Branch 7 (Deserter). | Renumbered to `branch_mil_14_fugitive_deserter`. |
| `branch_mil_3_reformer` | `branch_mil_3_opportunist` | Numeric ID collision with Opportunist; semantic collision with Branch 6 (Reformer). | Renumbered to `branch_mil_15_dissident_officer`. |
| `branch_mil_4_quartermaster` | `branch_mil_4_martyr` | Numeric ID collision with Martyr. | Renumbered to `branch_mil_9_quartermaster`. |
| `branch_mil_5_medic_soldier` | `branch_mil_5_tyrant` | Numeric ID collision with Tyrant. | Renumbered to `branch_mil_10_combat_medic`. |
| `branch_mil_6_conscript_father` | `branch_mil_6_reformer` | Numeric ID collision with Reformer. | Renumbered to `branch_mil_11_conscript_parent`. |
| `branch_mil_7_intelligence_officer` | `branch_mil_7_deserter` | Numeric ID collision with Deserter. | Renumbered to `branch_mil_12_intelligence_officer`. |
| `branch_mil_8_peacekeeper` | `branch_mil_8_broken_chain` | Numeric ID collision with Broken Chain. | Renumbered to `branch_mil_13_peacekeeper`. |

Following the precedent established in Plan 121 (`branch_ind_9` through `branch_ind_15`), Plan 122 utilizes the exact next free numeric sequence: **`branch_mil_9` through `branch_mil_15`**.

---

## 2. Master Identity Inventory (All 15 Branches)

| # | Branch ID | Display Name | PoNR Flag ID | Ending IDs (A, B, C) |
|---|---|---|---|---|
| **1** | `branch_mil_1_loyal_soldier` | The Loyal Soldier | `flag_branch_mil_1_ponr` | `ending_mil_1a_benevolent_dictator`<br>`ending_mil_1b_iron_fist`<br>`ending_mil_1c_survivor_king` |
| **2** | `branch_mil_2_defector` | The Defector | `flag_branch_mil_2_ponr` | `ending_mil_2a_reformer_of_rebels`<br>`ending_mil_2b_warlord`<br>`ending_mil_2c_survivor` |
| **3** | `branch_mil_3_opportunist` | The Opportunist | `flag_branch_mil_3_ponr` | `ending_mil_3a_tyrant`<br>`ending_mil_3b_benevolent_warlord`<br>`ending_mil_3c_survivor_king` |
| **4** | `branch_mil_4_martyr` | The Martyr | `flag_branch_mil_4_ponr` | `ending_mil_4a_saint_of_wasteland`<br>`ending_mil_4b_fallen_hero`<br>`ending_mil_4c_broken_martyr` |
| **5** | `branch_mil_5_tyrant` | The Tyrant | `flag_branch_mil_5_ponr` | `ending_mil_5a_tyrant_king`<br>`ending_mil_5b_benevolent_warlord`<br>`ending_mil_5c_survivor_king` |
| **6** | `branch_mil_6_reformer` | The Reformer | `flag_branch_mil_6_ponr` | `ending_mil_6a_visionary`<br>`ending_mil_6b_reformer`<br>`ending_mil_6c_survivor` |
| **7** | `branch_mil_7_deserter` | The Deserter | `flag_branch_mil_7_ponr` | `ending_mil_7a_lone_survivor`<br>`ending_mil_7b_traitor`<br>`ending_mil_7c_idealist` |
| **8** | `branch_mil_8_broken_chain` | The Broken Chain | `flag_branch_mil_8_ponr` | `ending_mil_8a_new_leader`<br>`ending_mil_8b_survivor`<br>`ending_mil_8c_warlord` |
| **9** | `branch_mil_9_quartermaster` | The Quartermaster | `flag_branch_mil_9_ponr` | `ending_mil_9a_keeper_of_stores`<br>`ending_mil_9b_civilian_quartermaster`<br>`ending_mil_9c_black_ledger` |
| **10** | `branch_mil_10_combat_medic` | The Combat Medic | `flag_branch_mil_10_ponr` | `ending_mil_10a_field_healer`<br>`ending_mil_10b_unit_outcast`<br>`ending_mil_10c_hardened_triage` |
| **11** | `branch_mil_11_conscript_parent` | The Conscript Parent | `flag_branch_mil_11_ponr` | `ending_mil_11a_community_protector`<br>`ending_mil_11b_divided_household`<br>`ending_mil_11c_permanent_fugitive` |
| **12** | `branch_mil_12_intelligence_officer` | The Intelligence Officer | `flag_branch_mil_12_ponr` | `ending_mil_12a_public_witness`<br>`ending_mil_12b_trusted_analyst`<br>`ending_mil_12c_silenced_liability` |
| **13** | `branch_mil_13_peacekeeper` | The Peacekeeper | `flag_branch_mil_13_ponr` | `ending_mil_13a_community_shield`<br>`ending_mil_13b_disarmed_mediator`<br>`ending_mil_13c_overrun` |
| **14** | `branch_mil_14_fugitive_deserter` | The Fugitive Deserter | `flag_branch_mil_14_ponr` | `ending_mil_14a_integrated_civilian`<br>`ending_mil_14b_hunted_fugitive`<br>`ending_mil_14c_armed_defector` |
| **15** | `branch_mil_15_dissident_officer` | The Dissident Officer | `flag_branch_mil_15_ponr` | `ending_mil_15a_reform_command`<br>`ending_mil_15b_broken_officer`<br>`ending_mil_15c_mutineer` |

---

## 3. Uniqueness and Standards Verification
- **Branch IDs:** Exactly 15 unique IDs starting with `branch_mil_`.
- **PoNR Flag IDs:** Exactly 15 unique IDs starting with `flag_branch_mil_`.
- **Ending IDs:** Exactly 45 unique IDs starting with `ending_mil_`.
- **Zero Prefix Clashes:** No collision with `branch_rebel_` or `branch_ind_` prefixes.
- **Verification:** Pinned by `Ashfall.Core.Tests.MilitaryBranchExpansionTests` and `Ashfall.Core.Tests.MilitaryBranchCatalogTests`.
