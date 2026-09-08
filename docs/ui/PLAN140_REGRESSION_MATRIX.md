# Plan 140 Regression & Verification Matrix

## 1. Test Suite Verification

| Test Target | Purpose | Filter / Command | Acceptance Criteria |
|---|---|---|---|
| Catalog Parity | Asserts 1:1 parity between JSON and loader default container | `dotnet test --filter FeedbackMessageTests` | 0 failed, 200 items matched |
| Safe Formatting | Asserts SafeFormat handles missing/null/excess arguments without exception | `dotnet test --filter SafeFormat` | No FormatException thrown |
| Feedback Service | Unit tests for `FeedbackService`, event resolution, category scoping | `dotnet test --filter FeedbackServiceTests` | 0 failed |
| Deduplication Engine | Verifies threshold transition triggers, cooldowns, and escalation bypass | `dotnet test --filter FeedbackDeduplicationTests` | Repeated tick suppressed, escalation passed |
| Confirmation Contract | Verifies confirm invokes once, cancel zero times, invalidation cancels | `dotnet test --filter ConfirmationContractTests` | 0 failed |
| Full Core Suite | Full regression across all domain systems | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | >= 10,080 passed, 0 failed |
| Data Integrity | Verifies catalog schemas and ID compliance | `godot --headless --path . -- --data-integrity-selftest` | 0 errors |
| Scene Binding | Verifies Godot UI scenes bind cleanly | `godot --headless --path . -- --scene-binding-selftest` | 25/25 passed |
| Scene Lint | Verifies scene syntax and nodes | `python3 scripts/ci/scene-lint.py` | 0 errors |
| Content Utilization | Proves `feedback_messages.json` is consumed in production | `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS |
