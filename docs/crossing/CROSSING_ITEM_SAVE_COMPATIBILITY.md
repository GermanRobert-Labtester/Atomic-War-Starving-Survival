# Plan 126 Save Compatibility

Plan 126 adds definitions only; it does not add an enum value, save section, migration, or new persistence authority.

Inventory state remains item ID plus quantity under the existing save stores. Existing eleven IDs and their stack behavior are unchanged. New IDs are absent from old saves and become resolvable when acquired after catalog expansion. Unknown/new definitions do not rewrite existing quantities.

The Plan 126 test suite verifies global ID resolution for all fourteen additions. Full save round-trip behavior remains owned by the existing inventory/save tests; no save code was changed for this catalog-only expansion.
