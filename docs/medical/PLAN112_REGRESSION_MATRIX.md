# Plan 112 regression matrix

| Gate | Coverage | Result |
|---|---|---|
| JSON parse / shape | schema 3, 20 rows, unique IDs | PASS |
| Vector contract | only water, air, blood, spore | PASS |
| Inventory references | countermeasures and treatments resolve in `items.json` | PASS |
| Existing ID stability | live 16-row prefix unchanged | PASS |
| Runtime registration | all four additions bind and accept `Infect` | PASS |
| Focused disease tests | catalog, depth, expansion, and system tests | PASS, 55 tests |
| Core disease behavior | spread, quarantine, protocols, determinism, outcomes | existing suite |
| Save/checksum | disease envelope, tamper rejection, migration, RNG restore | existing suite |
| Location/weather hooks | no unsupported fields or false integration claims | PASS, deferred |
| Autopsy hooks | existing zoonotic route preserved; new mapping deferred | PASS, deferred |
| Full Core regression | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS, 9,925 tests |
| Godot disease self-test | `--disease-selftest` | PASS, 81/81 |
| Data integrity | `--data-integrity-selftest` | PASS, 0 findings across 298 catalogs |
| Content utilization | `--content-utilization-selftest` | PASS, 0 orphaned catalogs |
| Save-store checksums | `--save-store-checksum-selftest` | PASS, 21/21 |
| Godot host build | `dotnet build Ashfall.csproj` | PASS, 0 errors/warnings |
| Headless boot / bridge | `--quit-after 2` and `--bridge-selftest` | PASS; boot reports existing resource-leak warnings |

The broader canonical dotnet and Godot gates are run after the documentation
and authority diff is complete.
