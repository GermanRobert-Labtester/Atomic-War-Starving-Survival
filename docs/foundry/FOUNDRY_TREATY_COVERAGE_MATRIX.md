# Foundry Treaty Consequence Coverage Matrix

**Treaty authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

**Consequence authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

## Final coverage

| Treaty ID | Policies | Outcomes | Coverage note |
|---|---:|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | 2 | `met`, `missed` | existing quota cycle |
| `treaty_cluster_labour_schedule` | 2 | `met`, `violated` | existing labour trigger |
| `treaty_road_iron_charter` | 2 | `met`, `missed` | existing quota cycle |
| `treaty_the_cluster_charter` | 0 | — | finale marker; no policy by design |
| `treaty_saltworks_access` | 2 | `met`, `violated` | Plan 103 data seam; no typed trigger yet |
| `treaty_membrane_repair` | 2 | `met`, `violated` | Plan 103 data seam; no typed trigger yet |
| `treaty_coal_window` | 2 | `met`, `missed` | Plan 103 data seam; no typed trigger yet |
| `treaty_apprentice_exchange` | 0 | — | no trainee-cycle trigger in Core |
| `treaty_crisis_mutual_aid` | 2 | `met`, `violated` | bounded emergency policy rows |
| `treaty_the_incident_book` | 1 | `met` | governance reward; no breach trigger yet |

The eight pre-existing regional records in the same JSON catalog remain
unchanged and have no Foundry consequence rows in this pass.

## Arithmetic note

The final catalog contains 15 rows: 6 preserved baseline rows plus 9 new
rows. Two outcomes for all ten Foundry accords would require 20 rows, so the
catalog does not claim coverage it cannot represent. The two uncovered
contracts are the Apprentice Exchange and Cluster Charter; both need a typed
runtime outcome source before additional policy data becomes executable.

## Outcome semantics

`violated` is the live runtime spelling for the plan's “breached” concept.
`missed` remains a recoverable shortfall, while `violated` is reserved for
active refusal or prohibited conduct.
