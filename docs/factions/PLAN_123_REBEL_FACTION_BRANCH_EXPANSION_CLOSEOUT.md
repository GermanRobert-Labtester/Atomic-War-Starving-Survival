# Plan 123 — Rebel Faction Branch Expansion Closeout

## Status

Complete for the live branch catalog and shared branch runtime. The catalog
now contains exactly 15 Rebel branches and 45 endings. The original eight
rows remain first and unchanged in content; seven new rows occupy IDs 9–15.

## New branches

| Branch | PoNR flag | Ending IDs |
|---|---|---|
| `branch_rebel_9_bombmaker` | `flag_branch_rebel_9_ponr` | `ending_rebel_9a_repentant_defector`, `ending_rebel_9b_scarred_survivor`, `ending_rebel_9c_unrepentant_zealot` |
| `branch_rebel_10_courier` | `flag_branch_rebel_10_ponr` | `ending_rebel_10a_trusted_network`, `ending_rebel_10b_broken_route`, `ending_rebel_10c_executed_traitor` |
| `branch_rebel_11_propagandist` | `flag_branch_rebel_11_ponr` | `ending_rebel_11a_voice_of_the_people`, `ending_rebel_11b_broadcast_in_ash`, `ending_rebel_11c_silenced_by_their_own` |
| `branch_rebel_12_dissident` | `flag_branch_rebel_12_ponr` | `ending_rebel_12a_reconciled_civilian`, `ending_rebel_12b_unwelcome_witness`, `ending_rebel_12c_killed_in_defection` |
| `branch_rebel_13_protector` | `flag_branch_rebel_13_ponr` | `ending_rebel_13a_community_shield`, `ending_rebel_13b_postkeeper`, `ending_rebel_13c_overwhelmed_guard` |
| `branch_rebel_14_saboteur` | `flag_branch_rebel_14_ponr` | `ending_rebel_14a_resistance_hero`, `ending_rebel_14b_bitter_liberator`, `ending_rebel_14c_starved_freedom` |
| `branch_rebel_15_negotiator` | `flag_branch_rebel_15_ponr` | `ending_rebel_15a_peacemaker`, `ending_rebel_15b_uneasy_accord`, `ending_rebel_15c_marked_collaborator` |

The provisional Plan 123 name `branch_rebel_5_defector` was not used:
`branch_rebel_2_defector` already exists in the shipped catalog. The new
variant is represented by the unique, repository-compatible
`branch_rebel_12_dissident`.

## Runtime and persistence

- `RebelBranchIds` is expanded to 15 branches, 15 PoNR flags, and 45 ending
  constants.
- `RebelBranchSystem` remains the only branch runtime. No per-archetype
  runtime or second branch system was added.
- New rows use the existing inclusive moral-band parser and first-match
  ending resolver. Each new branch has a non-overlapping partition covering
  all seven bands.
- PoNR flags continue to be written to both the runtime ledger and the
  save-durable `setFlags` list.
- The string-based save shape and save version remain unchanged. Existing
  branch saves continue to restore without new fields.

## Cross-plan reconciliation

The current consumers expose compatible, typed handoff points but do not
accept branch predicates or branch-ending mappings in their live schemas.
No unsupported JSON fields or duplicate authority were introduced.

| Plan | Live handoff |
|---|---|
| 124 faction-war overrides | Courier names the Understory relay, Protector names the petition camp, and Saboteur names Bridge Seven. These correspond to `loc_override_understory_transmitter_ambient`, `loc_override_camp_overrun`, and `loc_override_bridge_destroyed`. The override catalog remains a pure `(locationId, day)` presentation query. |
| 89 epilogue matrix | `FactionBranchCoordinator.OnEndingResolved` emits every new ending ID, and `ResolvedEndingId` persists it. The current `EpilogueMatrix` selects its own 25 `ending_key` values and has no branch-ending input, so direct mapping is contract-ready rather than falsely claimed as a live predicate. |
| 125 moral-choice flags | New Rebel PoNR flags are durable ledger flags. Existing moral-history flags `flag_sabotaged_rival` and `flag_broke_treaty` are the documented lead-in candidates for Saboteur and Negotiator content. The live Plan 125 schemas do not accept arbitrary PONR predicates, so the global flag catalog was not polluted with duplicate branch flags. |

Bombmaker, Saboteur, and Propagandist text stays high-level. It describes
consequence and accountability, not construction steps, materials, recipes,
or operational tactics.

## Verification

- Focused Rebel tests: 31 passed.
- JSON parse, ID uniqueness, and seven-band partition checks: passed.
- Authoritative data and `builds/linux` mirror: byte-identical.
- Full Core suite, host build, and Godot data-integrity gates are recorded in
  `docs/plans/PLAN_123_REBEL_BRANCH_IMPLEMENTATION_LOG.md`.
