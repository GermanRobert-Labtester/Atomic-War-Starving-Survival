# Plan 103 — Baseline Reconnaissance & Execution Contract

**Data authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

**Treaty authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

**Runtime policy model:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`

## Verified pre-change state

- Consequence rows: **6**.
- Treaty records: **18** total; **10** explicitly include
  `faction_silent_foundry`.
- Existing policy treaties: brine-pipe exchange, Cluster labour schedule,
  and road-iron charter.
- Existing policy faction: `faction_silent_foundry` for all six rows.
- Existing accepted outcomes: `met`, `missed`, `violated`.
- Existing mechanical channels: `standing_delta` and
  `market_modifiers[]`; there is no `mechanical_effect` field or token
  dispatcher in the live implementation.
- Lookup key: `(treaty_id, outcome)`. `faction_id` is validated as a
  signatory but is not part of the dictionary key.
- Idempotency: `(treaty_id, assessment day)` through
  `SilentFoundryConsequenceState.IsApplied`.
- The current compliance assessor creates live outcome rows only for the
  original four Foundry obligations; the Cluster Charter is a marker and the
  six Plan 102 contracts have no typed assessment trigger yet.

## Plan 103 target

Add exactly nine rows, reaching **15 policies**:

| Treaty | New outcomes | Purpose |
|---|---|---|
| `treaty_saltworks_access` | `met`, `violated` | measured access and maintenance trust |
| `treaty_coal_window` | `met`, `missed` | scheduled ice-road logistics |
| `treaty_membrane_repair` | `met`, `violated` | technical repair acceptance |
| `treaty_crisis_mutual_aid` | `met`, `violated` | bounded emergency cooperation |
| `treaty_the_incident_book` | `met` | reporting and institutional trust |

All nine rows target the Foundry as the affected signatory, matching the
existing ledger and host stance authority. The runtime uses `violated` as the
canonical value for the plan's colloquial “breached” state.

## Coverage reconciliation

Fifteen policies cannot provide two outcomes for all ten Foundry accords.
The honest final distribution is:

- 7 treaties with two policies;
- 1 treaty with one policy;
- 2 treaties with no policy rows: `treaty_apprentice_exchange` and
  `treaty_the_cluster_charter`;
- 8 of 10 Foundry-signatory accords covered.

The two uncovered rows are intentional: the current Core has no trainee-cycle
or charter-outcome trigger. Adding policy records without a trigger would make
them catalog-only promises, not executable consequences.

## Baseline verification

The relevant pre-change policy test path passed **27/27** tests. Existing
workspace data-integrity and build checks were also green before the data
edit. The previously quarantined Plan 103 expansion test file was stale and
excluded from the test project; it is updated and re-enabled as part of this
implementation.
