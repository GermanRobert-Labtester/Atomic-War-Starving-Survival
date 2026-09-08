# Moral Flag Epilogue Handoff

The current moral ending selector uses moral score, empathy, and resolved-quest count. It does not consume arbitrary historical flags. No ending predicate was added in Plan 125.

The most useful future bounded inputs are:

- `flag_broke_treaty` for historical accord memory;
- `flag_preserved_archive` for institutional continuity;
- `flag_honored_debt` for community obligation;
- `flag_chosen_faction_side` only as “committed to a side at least once,” never as faction identity.

Any future epilogue should combine these with canonical branch, faction, treaty, and current-state data rather than let one boolean dominate ending selection.
