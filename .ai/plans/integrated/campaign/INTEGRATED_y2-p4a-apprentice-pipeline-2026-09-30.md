# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Year Two P4a — Apprentice pipeline

STATUS: APPROVED BY USER

Acceptance repair authorized by user on 2026-09-30: "fix those 8 errors! so that integration can be complete and full!" Extend to CatalogIntegrityValidator.cs, Endgame/ChapterProfileCatalog.cs, year_two_chapter.json, existing CatalogIntegrityValidatorTests.cs/ChapterProfileTests.cs and owning docs index generator. Correct scoped enum classification, reject unsupported values and correct nonexistent terminal references; do not alter save, ending selection or unrelated data. Done requires focused regressions and global Godot data-integrity PASS before full integration header/archival.

Authorization: user continuation on 2026-09-30; approved Year Two umbrella §5 P4 explicitly defines four sequential subpackages. This executes 4a only, after accepted P2. P3 is blocked by missing lease service owner/parameters; P4b–d remain outside this package.

Outcome: canonical ChildDevelopment Adolescent+ with vocational_apprenticeship can consent to an existing catalog mentorship. Declines persist for the quarter. Mentor death makes the apprentice acting-eligible, never acting. State remains nested in apprenticeship; no new age ladder or save section.

Paths: Assets/Ashfall.Core/ApprenticeshipSystem.cs; src/Host/ApprenticeshipHostSession.cs; src/UI/ApprenticeshipPanel.cs; src/Main.ShelterSocial.cs (apprenticeship setup only); src/Main.SurvivorFate.cs (existing death event handler only); Assets/StreamingAssets/Data/apprenticeship_catalog.json; Ashfall.Core.Tests/Generations/YearTwoApprenticeLadderTests.cs; task state and bounded governance entries.

Done: all five umbrella P4a criteria met, existing mentorship/child/save contracts preserved, focused apprentice tests and host build pass, data validator passes, runtime wiring audited. No commit or full suite.

Premise correction: P2's host failed compilation because its IReadOnlyList property lacked System.Collections.Generic. Include only that import in src/Host/EndgameHostSession.cs to verify the required P2 runtime predecessor. No endgame behavior changed.

## Final acceptance — complete (2026-09-30)

All five P4a criteria are implemented and verified. The user-authorized prerequisite repair classifies the two chapter-profile fields as catalog-scoped enums, rejects unsupported/blank values through the owning catalog, and replaces two nonexistent terminal references with authored tragedy ending IDs. Genuine references remain strict; ending selection, save contracts and deterministic behavior are unchanged.

Final verification: `bin/run-scoped-tests Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs Ashfall.Core.Tests/Endgame/ChapterProfileTests.cs Ashfall.Core.Tests/Endgame/YearTwoPlayOnTests.cs Ashfall.Core.Tests/Generations/YearTwoApprenticeLadderTests.cs` PASS 51/51, zero failed/skipped. `dotnet build Ashfall.csproj --no-restore -v:minimal` PASS, zero warnings/errors. `bin/ashfall-dev validate-json` PASS 714/714. `godot --headless --path . --max-fps 15 -- --data-integrity-selftest` PASS, zero errors across 430 catalogs; five documented distress primary-wins warnings remain. `godot --headless --path . --max-fps 15 -- --year-two-chapter-selftest` PASS 7/7. Scoped whitespace check passed and read-only final audit found no blockers.

Only P4a is fully integrated. P3 and P4b–d remain outside this package. No commit or full suite. The prior blocked handoff below is historical and superseded by this acceptance.

## Historical implementation handoff — superseded

Implemented all apprentice command, persistence and presentation seams. Canonical child birth-day/stage and vocational milestone gate the consent route; ordinary StartPair cannot bypass consent for known children. The existing catalog supplies skill mappings. Quarter declines are retained through opening+90 and reopen on +91. Saved mentor deaths reconcile in both setup orders. Vocational mentor loss records acting eligibility only, preserving adult inheritance behavior. No new save section, age ladder or successor assignment.

Verification: existing apprenticeship targets 11/11 baseline; combined focused apprentice/Plan55 targets 25/25; Go JSON schema 714/714; host build 0 errors/14 unrelated existing warnings; player-panels-uitest PASS (22 lifecycle gates). Runtime gate reports existing deferred-focus diagnostic in Main.PlayerSurfaces.cs after PASS and does not exercise a populated Year Two vocational fixture. Final Core authority correction requires the new 8-test target rerun, recorded in task state.

Required content acceptance remains BLOCKED: `godot --headless --path . --max-fps 15 -- --data-integrity-selftest` fails 8 prerequisite catalog errors, all outside the changed apprenticeship catalog: six enum-token reference errors (`faction_branch` / `ending_resolved`) in chapter_profiles.json, and two unresolved ending IDs (`ending_extinction`, `ending_catastrophic_failure`) in year_two_chapter.json. Five documented distress warnings also reported. No apprenticeship finding. Those antecedent catalog/validator paths were left untouched rather than broadening this package.

At that earlier handoff the plan remained unsealed pending prerequisite catalog repair. The final acceptance above closes this blocker; P3 and full P4 remain unsealed. No commit or full suite.

Final verification: after removing hardcoded vocational target mappings, `bin/run-scoped-tests Ashfall.Core.Tests/Generations/YearTwoApprenticeLadderTests.cs` passed 8/8 (0 failed/skipped). Whitespace check passed across changed production paths.
