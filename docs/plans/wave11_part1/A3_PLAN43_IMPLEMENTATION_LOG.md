# WAVE 11 PART 1 — TASK A3 IMPLEMENTATION LOG
## C1[13] Plan 43 — Governing Together: The Shelter Decides

### 1. Overview & Verification Summary
Task A3 executes the governance, leadership policy cadence, typed crew consent, and work refusal mechanics without introducing parallel political subsystems or hidden loyalty stats:
1. **Typed Crew Consent & Refusal**: Implemented `CrewConsentVerdict` (`Accepted`, `AcceptedWithWarning`, `Refused(reason)`), wired into `DutyRosterAssignmentEngine` and `DutyRosterSystem`.
2. **Assignment Preflight & Refusal**: Duty assignment commands preflight consent; refused shifts return explicit typed `ActionResult.Blocked(reason)`. Auto-assignment (`AutoAssignDefaults`) deterministically excludes refusing survivors.
3. **Thin Policy Authority (`PolicySystem`)**: Implemented `Assets/Ashfall.Core/Governance/PolicySystem.cs` with authored `policies.json` covering initial scopes (`rations`, `curfew`, `emergency_override`).
4. **Leadership & Proposer Enforcement**: `leader_only` policies require the proposer to be the designated leader from `LeadershipSystem`. State capture/restore preserves active policies and decision records.

### 2. Implementation Matrix Centerpiece
| Clause | Current owner | Existing state | New delta | Typed result | Consequence owner | Save | Test |
|---|---|---|---|---|---|---|---|
| Policy Cadence & Authority | `PolicySystem` | Ad-hoc / scattered switches | Unified `PolicySystem` + `policies.json` catalog | `ActionResult.Success` / `Blocked("already_active")` | `SurvivorSocialCoordinator` / Schedule / Needs | `PolicySystemState` | `Plan43GoverningTogetherTests.PolicySystem_SetPolicy_UpdatesStateAndHistory` |
| Leader-Only Policy Enactment | `PolicySystem` + `LeadershipSystem` | Leader state unreferenced by policy | `proposer_rules == "leader_only"` requires designated leader | `ActionResult.Blocked("leader_only_policy")` | `LeadershipSystem` | `PolicySystemState` + `LeadershipSaveState` | `Plan43GoverningTogetherTests.PolicySystem_LeaderOnlyPolicy_EnforcedByLeadershipSystem` |
| Typed Crew Consent / Shift Refusal | `DutyRosterSystem` + `DutyRosterAssignmentEngine` | Fitness verdict only; no consent seam | `EvaluateCrewConsent` seam + `PreviewCrewConsent` | `CrewConsentVerdict.Refused(reason)` | `DutyRoster` (vacant shift) | Transient evaluation + Roster save | `Plan43GoverningTogetherTests.DutyRoster_CrewConsent_RefusalBlocksAssignment` |
| Auto-Assign Refusal Exclusion | `DutyRosterAssignmentEngine` | Utility auto-assign picks by fitness only | `IsConsentAllowed` prefilter in `PickEligible` | Bounded eligible candidate pool | `DutyRoster` | Deterministic seed | `Plan43GoverningTogetherTests.DutyRoster_AutoAssign_ExcludesRefusingSurvivors` |

### 3. Test Evidence
- `Ashfall.Core.Tests/Governance/Plan43GoverningTogetherTests.cs`: 6/6 PASS
- `Ashfall.Core.Tests/DutyRosterSystemTests.cs`: 32/32 PASS
- `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`: 10/10 PASS
