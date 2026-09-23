# BUG-PANEL-INPUTS implementation log

## Reproduce

Pre-integration checkpoint PASS. Exact panel paths had no active competing claim.
Gate 20 failed because neither maintenance input exists under the panel. Gate 21
failed with all=True, material=False, food=False, restored=True, proving the
sidebar changes its own selection but not visible inventory items. Gates 1–19
passed. Evidence: /tmp/ashfall-trade-repair.AqoLAz/inputs-before.log.
The new test initially used the wrong Inventory method name; corrected to the
current Add API before this runtime reproduction (no production API change).

## Repair

Pre-integration checkpoint PASS. Added existing capsule/link fields through the
panel's AddRow helper, immediately above their existing command buttons. Assigned
the existing sidebar return value so the already-written filter subscription runs.
No new data, command, save, RNG, or inventory authority. Final lifecycle 21/21,
including both interaction regressions, and holdfast runtime PASS. Host build
0 warnings/errors; no runtime engine/shutdown diagnostics. Status: RESOLVED.
Exact commands and evidence in the holdfast implementation log.
