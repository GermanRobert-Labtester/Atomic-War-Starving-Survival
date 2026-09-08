# Moral Flag Echo Handoff

Current echo records support `triggered_by`, `triggered_by_choice`, `min_days_after`, and `branch`. `MoralChoiceSystem.FindAvailableEchoQuests` does not read a flag predicate.

Plan 125 therefore adds no unsupported echo fields and claims 0/5 live flag-conditioned echo integrations. The five candidate historical mappings for a future supported predicate are:

1. `flag_spared_raider` → mercy callback.
2. `flag_shared_rations` → scarcity/generosity callback.
3. `flag_sheltered_refugee` → shelter callback.
4. `flag_sabotaged_rival` → betrayal callback.
5. `flag_preserved_archive` → Listener callback.

Each candidate must retain the existing source quest/day/branch checks. A future extension should only use a flag when it adds cross-quest “ever happened” meaning rather than duplicating an exact source-choice check.
