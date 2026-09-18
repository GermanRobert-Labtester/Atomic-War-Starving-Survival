# XP W1 Acceptance

**Package:** `XP-WAVE1-DIFFICULTY-AUTHORITY`
**Status:** partial acceptance

| Gate | Result |
|---|---|
| Core catalog/director/migration target | PASS — process exit code 0. |
| Shipped catalog cross-reference target | PASS — process exit code 0. |
| `dotnet build Ashfall.csproj --no-restore --nologo` | PASS — 0 warnings, 0 errors. |
| `godot --headless --path . -- --starting-cohort-lifecycle-selftest` | PASS with isolated Godot user/config paths. |
| Selected preset state | PASS — `difficulty_sparing` saves in `campaign_day` and restores after switching slots. |
| Starter grants | PASS — one `canned_food` and one `iodine_pills` are granted only during fresh initialization. |
| `git diff --check` on owned paths | PASS. |

`bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty` returned a wrapper
failure because its test-host report parser received no output despite the
underlying focused `dotnet test` command exiting 0. This existing runner
reporting issue did not block the direct focused target or the Godot runtime
acceptance check.
