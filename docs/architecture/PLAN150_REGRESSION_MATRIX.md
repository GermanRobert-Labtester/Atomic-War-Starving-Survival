# PLAN 150 REGRESSION MATRIX

Comprehensive test matrix for Plan 150 Personal Letters & Unsent Correspondence Activation.

---

## 1. Unit Test Matrix (`Ashfall.Core.Tests/PersonalLetterCatalogTests.cs`)

| Test ID | Test Name | Scope & Assertion | Expected Result |
|---|---|---|---|
| `TEST-LTR-01` | `PersonalLetters_LoadsAll25AuthoredLetters` | Loads `letters_expansion.json`, asserts 25 entries, validates `letter_01` (Mother's coat) and `letter_25` (One sentence), verifies types and tags. | PASS |
| `TEST-LTR-02` | `PersonalLetters_AllEntriesHaveValidFieldsAndUniqueIds` | Asserts all entries have non-empty IDs, valid days (> 0), valid senders, recipients, locations, letter types (`unsent` or `delivered`), and content. | PASS |
| `TEST-LTR-03` | `PersonalLetters_Load_IsIdempotent` | Calls `catalog.Load()` multiple times with identical JSON, asserts `AllLetters.Count` remains exactly 25. | PASS |
| `TEST-LTR-04` | `PersonalLetters_LoadFromDirectory_ResolvesCanonicalFile` | Tests static factory `PersonalLetterCatalog.LoadFromDirectory()` using `IFileIO` and `IJsonSerializer`. | PASS |
| `TEST-LTR-05` | `PersonalLetters_GetByType_FiltersDeliveredAndUnsent` | Verifies `GetByType("delivered")` returns exactly 9 letters, and `GetByType("unsent")` returns exactly 16 letters. | PASS |
| `TEST-LTR-06` | `PersonalLetters_GetBySenderAndRecipient_CaseInsensitive` | Verifies queries by sender (e.g. "a_daughter", "the_quartermaster") and recipient ("Pavel", "M.", "Tomas"). | PASS |
| `TEST-LTR-07` | `PersonalLetters_GetBySearch_FindsKeywords` | Queries text across ID, sender, recipient, location, themes, content, and tags (e.g. "coat", "watch", "flour", "potato"). | PASS |
| `TEST-LTR-08` | `PersonalLetters_Projection_MapsCanonicalRooms` | Tests `PersonalLetterProjection.ResolveRoomForLetter()` on all 25 letters, asserting valid canonical `room_*` identifiers. | PASS |
| `TEST-LTR-09` | `PersonalLetters_Projection_ClassifiesTruthAndProvenance` | Tests `PersonalLetterProjection.ResolveTruthClass()` on all 25 letters, asserting valid `LetterTruthClass` enum values. | PASS |
| `TEST-LTR-10` | `PersonalLetters_Projection_ZeroMutationContract` | Verifies querying letters and projections leaves mock system states completely unchanged. | PASS |
| `TEST-LTR-11` | `PersonalLetters_DeterministicOrdering` | Asserts letters can be ordered by day ascending/descending with ordinal secondary sorting. | PASS |
| `TEST-LTR-12` | `PersonalLetters_Clear_ResetsCatalog` | Calls `catalog.Clear()`, asserts count is 0, subsequent reload works cleanly. | PASS |

---

## 2. Full CI Verification Matrix (Rule 5)

| Gate Command | Requirement | Pass Criteria |
|---|---|---|
| `dotnet test Ashfall.Core.Tests` | Required | Exit code 0, 0 failed |
| `godot --headless --path . -- --content-utilization-selftest` | Required | Exit code 0, CI gate PASS |
| `godot --headless --path . -- --data-integrity-selftest` | Required | Exit code 0, 0 errors |
| `godot --headless --path . -- --scene-binding-selftest` | Required | Exit code 0, 25/25 passed |
| `python3 scripts/ci/scene-lint.py` | Required | Exit code 0, 0 errors |
