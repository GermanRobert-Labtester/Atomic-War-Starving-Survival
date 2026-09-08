# Moral Flag Source-Event Matrix

| Flag | Source system | Commit point | Replay behavior |
| --- | --- | --- | --- |
| `flag_spared_raider` | Base moral choice | `Resolve` after option 0 commits | One resolution per source quest. |
| `flag_executed_prisoner` | Base moral choice | `Resolve` after option 3 commits | Mutually exclusive with option 0 in this incident. |
| `flag_shared_rations` | Base/branch moral choice | `Resolve` after the selected sharing option commits | Can occur in separate incidents. |
| `flag_hoarded_medicine` | Broken Compact moral chain | `Resolve` after withholding option commits | One-shot per source quest. |
| `flag_sheltered_refugee` | Base moral choice | `Resolve` after entry authorization commits | One-shot per source quest. |
| `flag_expelled_survivor` | Broken Compact moral chain | `Resolve` after expulsion commits | One-shot per source quest. |
| `flag_repaired_infrastructure` | Broken Compact moral chain | `Resolve` after immediate repair commits | One-shot per source quest. |
| `flag_sabotaged_rival` | Broken Compact moral chain | `Resolve` after merger sabotage commits | One-shot per source quest. |
| `flag_broke_treaty` | Broken Compact moral chain | `Resolve` after accord sabotage commits | Historical boolean; formal treaty identity remains outside this source. |
| `flag_honored_debt` | Mercy Road moral chain | `Resolve` after community-service debt choice commits | One-shot per source quest. |
| `flag_ignored_distress` | Distress moral choice | `Resolve` after deliberate non-response commits | Can coexist with response from another incident. |
| `flag_responded_distress` | Distress moral choice | `Resolve` after rescue choice commits | Can coexist with ignore from another incident. |
| `flag_forged_record` | Broken Compact moral chain | `Resolve` after evidence-planting choice commits | One-shot per source quest. |
| `flag_preserved_archive` | Listener moral chain | `Resolve` after archive choice commits | One-shot per source quest. |
| `flag_chosen_faction_side` | Mercy Road moral chain | `Resolve` after explicit alliance choice commits | Broad “committed at all” history; branch state identifies specifics. |

The Plan 125 test suite loads the authored source catalogs and asserts every new flag has at least one producer.
