# Plan 126 Regression Matrix

| Area | Evidence | Result |
| --- | --- | --- |
| JSON parse and schema | `CrossingItemsPlan126Tests` | PASS |
| Exact item count | local catalog + global registration | PASS; 25 |
| Original 11 parity | numeric/type assertions | PASS |
| New ID uniqueness | local and all global `*items.json` files | PASS |
| Canonical type values | accepted `ItemType` set | PASS |
| Numeric ranges | stack, weight, value checks | PASS |
| Bread/water effects | global `ItemDefinition` assertions | PASS |
| Medicine health semantics | canonical `healthEffect` assertion | PASS; H0 |
| Lamp Oil semantics | type-only; no fake lamp consumer | PASS; L2 documented |
| Map/pouch semantics | no unsupported fields or runtime | PASS |
| Crossing headless smoke | `--crossing-selftest` | PASS; 40/40 |
| Data integrity | `--data-integrity-selftest` | PASS; 0 errors |
| Content utilization | `--content-utilization-selftest` | PASS; 0 orphaned catalogs |
| Targeted Crossing tests | `dotnet test ... --filter FullyQualifiedName~Crossing` | PASS; 97 |
| Plan 126 tests | `dotnet test ... --filter FullyQualifiedName~CrossingItemsPlan126` | PASS; 7 |
| Host build | `dotnet build Ashfall.csproj --no-restore` | PASS; 0 warnings/errors |
| Fast CI tier | `run-gates.py --tier fast` | BASELINE BLOCKER; unrelated whitespace findings |

The four Plan 120, three Plan 115, and two cross-expansion item-level links requested by the roadmap are not marked green because current live schemas/consumers do not support those exact links without inventing authority.
