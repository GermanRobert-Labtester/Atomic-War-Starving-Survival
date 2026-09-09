# PLAN 147 REGRESSION MATRIX — risk → test register

All tests live in
`Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs` (28 tests) unless
noted. The pre-existing shape tests
(`Ashfall.Core.Tests/BunkerContrabandCatalogTests.cs`, 3 tests) remain green
and untouched.

## Plan-risk register

| # | Plan risk (§9) / requirement | Guard test(s) |
|---|---|---|
| 1 | Hidden second economy authority | `AuthorizedEffect_TradeValue_IsOwnedByCanonicalItemDefinition` + the runtime has no price/valuation API by construction; `Stash_LoadAndQuery_HasNoSimulationSideEffects` |
| 2 | Direct HP/hunger/morale effects bypassing canonical item use | `AuthorizedEffect_Morale_IsOwnedByCanonicalSugarItem_NotByContrabandCode` (morale only via canonical `sugar.moraleEffect` through the use pipeline; stash grant asserts no other slots granted) |
| 3 | Dependency risk applied twice | No dependency entry activated this slice (morphine deferred); `chemical_dependency_risk` proven non-executed by `Stash_UnknownOrUnactivatedEntry_FailsClosed` (claim on a deferred record never succeeds). Full once-per-consumption pin belongs to the follow-up morphine slice. |
| 4 | Fictitious tribunal-suspicion subsystem | No suspicion code exists; `tribunal_suspicion_rate` is validator-range-checked only (DESCRIPTIVE). No test can assert absence of a subsystem mechanically — enforced by review + the frozen key allowlist rejecting new suspicion-style keys (`Validator_RejectsUnknownMechanicsKey`). |
| 5 | Location strings mistaken for IDs | `CONTRABAND_STASH_LOCATION_MATRIX.md` decision + stash placement owned by activation map; `Validator` never resolves stash strings. |
| 6 | Rerollable unique stashes (save/reload, reopen) | `Stash_IsOnceOnly_NoReclaim`, `Stash_SaveRoundTrip_PreservesOnceOnlyClaims`, `Stash_CapacityBlocked_ClaimIsNotRecorded_AndRetrySucceeds` (blocked claim ≠ destroyed stash ≠ reroll exploit) |
| 7 | Contraband ids duplicated as item ids with conflicting definitions | Cross-ref sweep (ENTRY_MATRIX §cross-refs): zero collisions; `Activation_CanonicalItems_ExistInAuthoritativeItemCatalog` pins the three links to real defs |
| 8 | Trade arbitrage through mixed price semantics | `CONTRABAND_TRADE_AND_ARBITRAGE_AUDIT.md`; once-only acquisition means no buy→sell loop; no scrip conversion exists (by construction). |
| 9 | UI claiming mechanics that don't exist | No UI wired yet; runtime exposes only discoverable/claim facts. Future panel must bind to `ListDiscoverable`/`IsClaimed`, not to mechanics fields. |
| 10 | Scope explosion into crime/law/black-market simulation | No crime/police/suspicion/counterfeit code introduced; runtime is 2 Core files + tests. |

## Task A (validation) register

| Requirement (Task A.15) | Test |
|---|---|
| Reject duplicate ids | `Validator_RejectsDuplicateIds` |
| Reject negative / non-integer prices | `Validator_RejectsNegativeAndNonIntegerPrices` |
| Reject invalid tiers (0, 4) | `Validator_RejectsInvalidTiers` |
| Reject NaN/Infinity | `Validator_RejectsNonFiniteNumbersAtParseLevel` |
| Reject unknown mechanics keys | `Validator_RejectsUnknownMechanicsKey` |
| Reject out-of-range probabilities/multipliers/quantities | `Validator_RejectsOutOfRangeMechanicsValues` (4 cases) |
| Reject bad ids / missing fields | `Validator_RejectsBadIdsAndMissingFields` |
| Authored corpus stays valid | `Validator_AuthoredCatalog_IsValidWith20Entries` |

## Task B (slice) register

| Requirement | Test |
|---|---|
| One low/mid/high tier activated (B.10) | `Activation_CoversOneLowOneMidOneHighTierEntry` |
| Canonical identity (B.5) | `Activation_CanonicalItems_ExistInAuthoritativeItemCatalog`, `Stash_Claim_GrantsExactCanonicalItems` |
| High-tier gated on real campaign state — day (B.9) | `Stash_DayGate_BlocksEarlyClaim_AndOpensAtGate` |
| Fail-closed for deferred records (B.4) | `Stash_UnknownOrUnactivatedEntry_FailsClosed`, `Registration_FailsClosed_ForUnknownEntryOrBadPayload` |
| Once-only, deterministic (B.8, B.14) | `Stash_IsOnceOnly_NoReclaim`, `Stash_Deterministic_IdenticalSystemsProduceIdenticalAvailabilitySequences` |
| Atomic, capacity-respecting transaction (B.13) | `Stash_CapacityBlocked_ClaimIsNotRecorded_AndRetrySucceeds` |
| No side effects on load/query/view (C.5) | `Stash_LoadAndQuery_HasNoSimulationSideEffects` |
| Save/reload stability (C.4, C.11) | `Stash_SaveRoundTrip_PreservesOnceOnlyClaims`, `Stash_OldSave_WithNoContrabandState_ChangesNothing`, `Stash_RestoreState_IsDeepClone_NotSharedReferences` |
| Effects owned by live authorities (B acceptance) | `AuthorizedEffect_Morale_…`, `AuthorizedEffect_Agriculture_…`, `AuthorizedEffect_TradeValue_…` |

## Full-gate runs (session record)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS (0 errors, 0 warnings) |
| `dotnet test` — ContrabandPlan147 filter | PASS 28/28 |
| `dotnet build Ashfall.csproj` (Godot host) | PASS (0 errors, 0 warnings) |
| `godot --headless -- --data-integrity-selftest` | PASS — 299 catalogs, 0 errors, 0 warnings (incl. contraband catalog) |
| `godot --headless -- --bridge-selftest` | PASS exit 0 |
| Full `dotnet test` suite | PASS 10273/10273 (incl. 28 new + 3 pre-existing contraband tests) |
