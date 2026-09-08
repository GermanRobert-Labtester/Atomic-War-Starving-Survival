# Moral Flag Regression Matrix

| Area | Coverage | Result |
| --- | --- | --- |
| Catalog parse/count | 25 records, unique IDs, populated labels | PASS |
| Static ID synchronization | Catalog IDs are contained in `MoralChoiceIds.AllFlags`; external messenger marker retained | PASS |
| Producer coverage | All 15 new IDs found in loaded source options | PASS |
| Resolution write | `set_flag` maps through base, distress, branch, and expansion loaders | PASS |
| One-shot behavior | A resolved quest cannot replay or duplicate its flag | PASS |
| Same-incident exclusivity | Raider and distress alternative outcomes remain exclusive | PASS |
| Old-state behavior | New IDs remain unset when absent from an old `MoralChoiceState` | PASS |
| Save authority | Existing `MoralChoiceState` / `MoralChoiceSaveStore` retained | PASS |
| Echo predicates | Current schema has no flag predicate | STAGED |
| PONR predicates | Current branch systems have no arbitrary moral-flag predicate | STAGED |
| Faction reactions | Current catalog is threshold-event keyed | STAGED |
| Gossip predicates | Current runtime is plain band/section strings | STAGED |
| Epilogue predicates | Current selector is score/empathy/resolution based | AUDITED / STAGED |
| Data integrity | 0 errors across 298 catalogs | PASS |
| Content utilization | 581 catalogs, 0 orphaned | PASS |
| Moral-choice targeted tests | 154 passed | PASS |
| Full unit tests | 9,969 passed, 0 failed | PASS |
| Core/Godot host build | 0 warnings, 0 errors | PASS |
| Exported packaged parity | 1,218 catalogs byte-identical and parseable | PASS |
| Packaged data-integrity selftest | Exit 0 on freshly exported executable | PASS |

The staged rows are deliberate architecture boundaries, not claims of unsupported wiring.
