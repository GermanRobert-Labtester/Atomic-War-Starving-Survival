# Plan 122 — Moral Band Coverage & Ending Matrix

> **Authority:** `MoralChoiceSystem.cs` & `MoralPathBand.cs`
> **Target Catalog:** `Assets/StreamingAssets/Data/military_faction_branch.json`

---

## 1. Canonical Moral Band Definitions

ASHFALL defines seven canonical moral path bands mapped to the player's continuous ethical score (-200 to +200):

| Moral Path Band Token | Enum Value | Score Interval | Semantic Interpretation |
|---|---|---|---|
| `very_evil` | `MoralPathBand.VeryEvil` | `[-200, -100]` | Uncompromising cruelty, predatory exploitation, total disregard for life. |
| `evil` | `MoralPathBand.Evil` | `[-99, -50]` | Ruthless self-interest, callous pragmatism, willing sacrifice of others. |
| `slightly_evil` | `MoralPathBand.SlightlyEvil` | `[-49, -1]` | Cynical compromise, minor embezzlement, low empathy under pressure. |
| `neutral` | `MoralPathBand.Neutral` | `[0, 0]` | Equidistant survival, strict transactional balance, no moral commitment. |
| `slightly_positive` | `MoralPathBand.SlightlyPositive` | `[1, 49]` | Cautious decency, occasional charity, reluctance to cause unnecessary harm. |
| `positive` | `MoralPathBand.Positive` | `[50, 99]` | Principled humanitarianism, communal loyalty, active defense of vulnerable. |
| `very_positive` | `MoralPathBand.VeryPositive` | `[100, 200]` | Self-sacrificing altruism, radical forgiveness, unyielding moral conviction. |

---

## 2. Branch Entry Eligibility Histogram

Entry bands determine which branches a survivor can commit to. Expansion from 8 to 15 branches broadens access across all ethical alignments:

| Moral Path Band | Baseline Eligible Branches (8) | New Eligible Branches (7) | Total Eligible Branches (15) |
|---|:---:|:---:|:---:|
| `very_evil` | 4 (`mil_3`, `mil_5`, `mil_8`) *Note: `mil_5` stops at `evil` | +2 (`mil_9`, `mil_12`) | **6** |
| `evil` | 6 (`mil_1`*, `mil_2`, `mil_3`, `mil_5`, `mil_7`, `mil_8`) | +4 (`mil_9`, `mil_11`, `mil_12`, `mil_14`) | **10** |
| `slightly_evil` | 5 (`mil_1`, `mil_2`, `mil_3`, `mil_7`, `mil_8`) | +5 (`mil_9`, `mil_10`, `mil_11`, `mil_12`, `mil_14`) | **10** |
| `neutral` | 5 (`mil_2`, `mil_3`, `mil_6`, `mil_7`, `mil_8`) | +7 (`mil_9`, `mil_10`, `mil_11`, `mil_12`, `mil_13`, `mil_14`, `mil_15`) | **12** |
| `slightly_positive` | 6 (`mil_1`, `mil_2`, `mil_3`, `mil_6`, `mil_7`, `mil_8`) | +7 (`mil_9`, `mil_10`, `mil_11`, `mil_12`, `mil_13`, `mil_14`, `mil_15`) | **13** |
| `positive` | 6 (`mil_2`, `mil_3`, `mil_4`, `mil_6`, `mil_7`, `mil_8`) | +7 (`mil_9`, `mil_10`, `mil_11`, `mil_12`, `mil_13`, `mil_14`, `mil_15`) | **13** |
| `very_positive` | 4 (`mil_3`, `mil_4`, `mil_6`, `mil_8`) | +6 (`mil_9`, `mil_10`, `mil_11`, `mil_12`, `mil_13`, `mil_15`) | **10** |

Every moral band has between 6 and 13 eligible branches, ensuring that no playstyle is locked out of compelling military character arcs.

---

## 3. Ending Partition Truth Tables (Branches 9–15)

Every new branch implements an exhaustive, non-overlapping partition of all 7 moral bands:
- **Ending A:** High moral alignment (`positive` .. `very_positive`)
- **Ending B:** Middle / ambiguous alignment (`slightly_evil` .. `slightly_positive`)
- **Ending C:** Low / ruthless alignment (`very_evil` .. `evil`)

### Branch 9: The Quartermaster (`branch_mil_9_quartermaster`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_9c_black_ledger` | The Black Ledger |
| `evil` | `ending_mil_9c_black_ledger` | The Black Ledger |
| `slightly_evil` | `ending_mil_9b_civilian_quartermaster` | The Civilian Quartermaster |
| `neutral` | `ending_mil_9b_civilian_quartermaster` | The Civilian Quartermaster |
| `slightly_positive` | `ending_mil_9b_civilian_quartermaster` | The Civilian Quartermaster |
| `positive` | `ending_mil_9a_keeper_of_stores` | The Keeper of Stores |
| `very_positive` | `ending_mil_9a_keeper_of_stores` | The Keeper of Stores |

### Branch 10: The Combat Medic (`branch_mil_10_combat_medic`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_10c_hardened_triage` | The Hardened Triage |
| `evil` | `ending_mil_10c_hardened_triage` | The Hardened Triage |
| `slightly_evil` | `ending_mil_10b_unit_outcast` | The Unit Outcast |
| `neutral` | `ending_mil_10b_unit_outcast` | The Unit Outcast |
| `slightly_positive` | `ending_mil_10b_unit_outcast` | The Unit Outcast |
| `positive` | `ending_mil_10a_field_healer` | The Field Healer |
| `very_positive` | `ending_mil_10a_field_healer` | The Field Healer |

### Branch 11: The Conscript Parent (`branch_mil_11_conscript_parent`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_11c_permanent_fugitive` | The Permanent Fugitive |
| `evil` | `ending_mil_11c_permanent_fugitive` | The Permanent Fugitive |
| `slightly_evil` | `ending_mil_11b_divided_household` | The Divided Household |
| `neutral` | `ending_mil_11b_divided_household` | The Divided Household |
| `slightly_positive` | `ending_mil_11b_divided_household` | The Divided Household |
| `positive` | `ending_mil_11a_community_protector` | The Community Protector |
| `very_positive` | `ending_mil_11a_community_protector` | The Community Protector |

### Branch 12: The Intelligence Officer (`branch_mil_12_intelligence_officer`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_12c_silenced_liability` | The Silenced Liability |
| `evil` | `ending_mil_12c_silenced_liability` | The Silenced Liability |
| `slightly_evil` | `ending_mil_12b_trusted_analyst` | The Trusted Analyst |
| `neutral` | `ending_mil_12b_trusted_analyst` | The Trusted Analyst |
| `slightly_positive` | `ending_mil_12b_trusted_analyst` | The Trusted Analyst |
| `positive` | `ending_mil_12a_public_witness` | The Public Witness |
| `very_positive` | `ending_mil_12a_public_witness` | The Public Witness |

### Branch 13: The Peacekeeper (`branch_mil_13_peacekeeper`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_13c_overrun` | The Overrun Sentinel |
| `evil` | `ending_mil_13c_overrun` | The Overrun Sentinel |
| `slightly_evil` | `ending_mil_13b_disarmed_mediator` | The Disarmed Mediator |
| `neutral` | `ending_mil_13b_disarmed_mediator` | The Disarmed Mediator |
| `slightly_positive` | `ending_mil_13b_disarmed_mediator` | The Disarmed Mediator |
| `positive` | `ending_mil_13a_community_shield` | The Community Shield |
| `very_positive` | `ending_mil_13a_community_shield` | The Community Shield |

### Branch 14: The Fugitive Deserter (`branch_mil_14_fugitive_deserter`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_14c_armed_defector` | The Armed Defector |
| `evil` | `ending_mil_14c_armed_defector` | The Armed Defector |
| `slightly_evil` | `ending_mil_14b_hunted_fugitive` | The Hunted Fugitive |
| `neutral` | `ending_mil_14b_hunted_fugitive` | The Hunted Fugitive |
| `slightly_positive` | `ending_mil_14b_hunted_fugitive` | The Hunted Fugitive |
| `positive` | `ending_mil_14a_integrated_civilian` | The Integrated Civilian |
| `very_positive` | `ending_mil_14a_integrated_civilian` | The Integrated Civilian |

### Branch 15: The Dissident Officer (`branch_mil_15_dissident_officer`)
| Evaluated Band | Active Ending ID | Ending Title |
|---|---|---|
| `very_evil` | `ending_mil_15c_mutineer` | The Mutineer |
| `evil` | `ending_mil_15c_mutineer` | The Mutineer |
| `slightly_evil` | `ending_mil_15b_broken_officer` | The Broken Officer |
| `neutral` | `ending_mil_15b_broken_officer` | The Broken Officer |
| `slightly_positive` | `ending_mil_15b_broken_officer` | The Broken Officer |
| `positive` | `ending_mil_15a_reform_command` | The Reform Commander |
| `very_positive` | `ending_mil_15a_reform_command` | The Reform Commander |

---

## 4. Verification Evidence
- **Total Branches:** 15
- **Total Endings:** 45 (exactly 3 per branch)
- **Coverage:** 100% of the 7 moral bands covered across all 15 branches with zero gaps and zero ambiguous overlaps. Verified by `Ashfall.Core.Tests.MilitaryBranchExpansionTests`.
