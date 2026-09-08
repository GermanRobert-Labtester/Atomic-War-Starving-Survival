# Moral Flag PONR Handoff

The live military, rebel, and independent branch systems commit PONR state from branch-specific lock flags plus moral band, standing, and hostility gates. They do not parse arbitrary moral flag predicates.

Plan 125 adds no unsupported PONR fields and claims 0/4 live integrations. The staged candidates are:

- `flag_broke_treaty`
- `flag_sabotaged_rival`
- `flag_forged_record`
- `flag_chosen_faction_side`

`flag_chosen_faction_side` must never substitute for the canonical branch/faction identity. Any future integration must be a downstream read of the existing flag store and must not create a second PONR marker.
