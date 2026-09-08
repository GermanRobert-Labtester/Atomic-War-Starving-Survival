# Moral Flag Write-Path Matrix

The canonical write path is:

```text
committed moral option → MoralChoiceOption.SetFlag → MoralChoiceSystem.Resolve → SetFlag → MoralChoiceState.activeFlags
```

| Flag | Source quest / option | Status |
| --- | --- | --- |
| `flag_spared_raider` | `quest_moral_share_raider`, option 0, “Give medical supplies” | Live; the discovery explicitly says the unarmed raider requests quarter. |
| `flag_executed_prisoner` | `quest_moral_share_raider`, option 3, “Finish them” | Live; same source incident, which requests quarter before the lethal choice. |
| `flag_shared_rations` | `quest_moral_share_raider`, option 1, “Give food”; also `quest_moral_chain_mercy_01`, option 0 | Live; two legitimate producers. |
| `flag_hoarded_medicine` | `quest_moral_chain_betray_01`, option 1, “Keep the pills for your own people” | Live. |
| `flag_sheltered_refugee` | `quest_moral_env_shelter_refugee`, option 0, “Authorize structural entry” | Live. |
| `flag_expelled_survivor` | `quest_moral_chain_betray_09`, option 1, “Expel Bray from the shelter” | Live. |
| `flag_repaired_infrastructure` | `quest_moral_chain_betray_11`, option 0, “Fix it immediately and ask for nothing” | Live. |
| `flag_sabotaged_rival` | `quest_moral_chain_betray_18`, option 1, “Sabotage the merger...” | Live. |
| `flag_broke_treaty` | `quest_moral_chain_betray_05`, option 2, “Sabotage the talks...” | Live with a documented semantic boundary: this is the existing negotiated-accord rupture; the moral runtime has no formal treaty object. |
| `flag_honored_debt` | `quest_moral_chain_mercy_19`, option 1, community service to work off the debt | Live. |
| `flag_ignored_distress` | `quest_moral_distress_trapped_mechanic`, option 1, “Disregard the transmission” | Live. |
| `flag_responded_distress` | `quest_moral_distress_trapped_mechanic`, option 0, “Dispatch rescue expedition” | Live. |
| `flag_forged_record` | `quest_moral_chain_betray_04`, option 1, planting tools under Kessler's name | Live; the source is institutional evidence falsification rather than a generic lie. |
| `flag_preserved_archive` | `quest_moral_chain_listen_25`, option 1, archive the journal | Live. |
| `flag_chosen_faction_side` | `quest_moral_chain_mercy_15`, option 0, accept the Crossroads Collective alliance on equal terms | Live; this records commitment, not which faction was chosen. |

Writes are idempotent because `activeFlags` is treated as a set and the external ledger is also set-based.
