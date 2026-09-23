# BUG-TEST-WARNINGS implementation log

## Phase 1 — assertion repair

Pre-integration checkpoint: PASS. All five compiler-reported locations still
matched source; exact paths claimed; preceding Plan 176 edits preserved.

Changed three membership assertions to Contains/DoesNotContain, the duplicate
record assertion to DoesNotContain with the same predicate, and reversed one
numeric expected/actual pair. No warning suppression or new tests.

## Phase 2 — verification

Fresh test compilation emitted none of the five earlier analyzer warnings.
Each affected file was run via `bash scripts/run_test.sh <path>`:

| Path | Result |
|---|---|
| `Ashfall.Core.Tests/World/Plan176AnomalyHazardTests.cs` | 21/21 PASS |
| `Ashfall.Core.Tests/Shelter/Plan20BShieldingBalanceSweepTests.cs` | 12/12 PASS |
| `Ashfall.Core.Tests/Collectibles/CollectibleSaveMigrationTests.cs` | 4/4 PASS |
| `Ashfall.Core.Tests/Integration/Expansion4IntegrationTests.cs` | 8/8 PASS |

45/45 PASS. VSTest local sockets required sandbox escalation after the first
sandboxed attempt was denied. Only the affected targets ran. Diff whitespace
check passed; assertion meaning, production behavior, save formats, and RNG
are unchanged. Status: RESOLVED.
