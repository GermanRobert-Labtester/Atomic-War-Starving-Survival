# Foundry Treaty Consequence Faction Matrix

**Policy authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

**Treaty authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

## Faction semantics

`faction_id` is the signatory whose Foundry relationship is affected by the
row. The current host has one authoritative Foundry standing ledger, so all
15 policies use the verified `faction_silent_foundry` identity. This is not a
wildcard and does not broadcast standing to co-signatories.

## Reference matrix

| Policy group | Policy faction | Treaty signatory check |
|---|---|---|
| six preserved baseline rows | `faction_silent_foundry` | valid for all three referenced accords |
| Saltworks Access (2) | `faction_silent_foundry` | valid; Office and Scale remain co-signatories |
| Membrane Repair (2) | `faction_silent_foundry` | valid; Office remains co-signatory |
| Coal Window (2) | `faction_silent_foundry` | valid; Cutters remains co-signatory |
| Crisis Mutual Aid (2) | `faction_silent_foundry` | valid; seven additional explicit signatories remain in treaty data |
| Incident Book (1) | `faction_silent_foundry` | valid; Office and Archivists remain co-signatories |

Automated reference checks report zero invalid treaty IDs, zero invalid faction
IDs, and zero policy rows whose faction is absent from the referenced treaty.
The policy catalog itself does not infer conceptual labels such as “Office,”
“Scale,” or “Cluster.”
