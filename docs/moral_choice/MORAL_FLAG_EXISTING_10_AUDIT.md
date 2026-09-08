# Existing 10-Flag Audit

| ID | Display name | Existing role |
| --- | --- | --- |
| `flag_betrayed_ally` | Betrayed an Ally | Legacy narrative vocabulary; no option-level producer in the current moral catalog. |
| `flag_betrayed_faction` | Betrayed a Faction | Legacy narrative vocabulary; used by chain gate data. |
| `flag_betrayed_trust` | Broke Trust | Legacy narrative vocabulary; used by chain gate data. |
| `flag_broken_pact` | Broken Pact | Legacy narrative vocabulary; used by chain gate data. |
| `flag_become_warlord` | Became Warlord | Legacy ending/branch vocabulary. |
| `flag_throne_of_ash` | Throne of Ash | Legacy ending/branch vocabulary. |
| `flag_branch_mercy_road_locked` | Mercy Road Locked | Branch lockout flag written by `MoralChoiceSystem.TrackBranchProgress`. |
| `flag_branch_iron_way_locked` | Iron Way Locked | Branch lockout flag written by `MoralChoiceSystem.TrackBranchProgress`. |
| `flag_branch_listener_locked` | Listener's Thread Locked | Branch lockout flag written by `MoralChoiceSystem.TrackBranchProgress`. |
| `flag_branch_broken_compact_locked` | Broken Compact Locked | Branch lockout flag written by `MoralChoiceSystem.TrackBranchProgress`. |

All 10 IDs and display names were preserved. The existing branch-lock write path remains unchanged.
