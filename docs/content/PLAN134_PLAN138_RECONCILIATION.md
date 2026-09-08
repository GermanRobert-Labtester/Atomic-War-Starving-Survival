# Plan 134 / Plan 138 Reconciliation

The repository's Plan 134 is Dynamic Faction Territory & Supply Line Control.
Its implementation is not a day-zero supply/origin profile system. Existing
starting supplies are loaded by `InventoryHostSession` from their own
authority.

Plan 138 therefore keeps two dimensions separate:

- cohort: canonical people and their supported initial personal conditions;
- supplies/origin: existing inventory and starting-level authorities.

No cohort embeds item IDs, supply quantities, recipes, research, or loadout
choices. No Plan 134 system is changed. A future supply-origin selector may
compose with cohorts only through an explicit compatibility matrix and its own
tests.
