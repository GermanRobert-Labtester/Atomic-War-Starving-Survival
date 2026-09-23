# BUG-HOLDFAST-INTEGRITY implementation log

## Phase 1 — reproduce

Pre-integration checkpoint: PASS. Existing trade claims are DONE; captured
pre-edit HoldfastTradeSession.cs SHA-256
`7c03a7cedd6329ff9d8f3563f32fedd61496ce2296eef8abf4bc640f4175c5e4` matched before editing.
The initial Godot build passed with 0 warnings / 0 errors.

New HoldfastTradeIntegrityTests failed 13/13 on the original implementation.
Tracing preview parity identified the missing sell embargo check; with that
regression added, the original source failed 14/14. The second baseline run
needed escalation because VSTest's local communication socket was sandbox-blocked.
The successful reproduction log is `/tmp/ashfall-trade-repair.AqoLAz/before.log`.

## Phase 2 — repair

Pre-integration checkpoint: PASS. No save fields, ownership changes, authored
price changes, or RNG consumption required. Changes confined to the claimed
source: checked item-count/stock/wallet headroom, seed bookkeeping after accepted
addition, representable quote checks shared by previews/commands, exact integer
funds totals, explicit rejection of non-finite/out-of-range float conversions,
and the missing sell embargo preview check.

Regression: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/HoldfastTradeIntegrityTests.cs`
14/14 PASS. Existing HoldfastFundsTradeTests 4/4 PASS.
Diff inspected against the pre-edit source; earlier funds API additions preserved.

## Phase 3 — verification

Pre-integration checkpoint: PASS. Adjacent targets run individually via
`bash scripts/run_test.sh <path>`:

| Path | Result |
|---|---|
| `Ashfall.Core.Tests/Economy/HoldfastTradeIntegrityTests.cs` | 14/14 |
| `Ashfall.Core.Tests/Economy/HoldfastFundsTradeTests.cs` | 4/4 |
| `Ashfall.Core.Tests/HoldfastTradeSessionTests.cs` | 3/3 |
| `Ashfall.Core.Tests/HoldfastTradeArbitrageTests.cs` | 8/8 |
| `Ashfall.Core.Tests/TradeCommandTests.cs` | 9/9 |
| `Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` | 18/18 |
| `Ashfall.Core.Tests/Holdfast/HoldfastTradeSaveStoreTests.cs` | 13/13 |
| `Ashfall.Core.Tests/Inventory/UnifiedInventoryOwnershipTests.cs` | 7/7 |

Total: 76/76 PASS. No full suite. Adjacent VSTest runs required the same
local-socket sandbox escalation as the reproduction. Focused diff whitespace
check passed. Final build and bounded Godot runtime verification recorded below.

## Phase 4 — runtime verification correction

Godot and test project builds both passed with 0 warnings / 0 errors.
The first isolated runtime test failed only `quarantine`; all trade, invalid
transaction, save/reload, continued-play, and new-ledger checks passed. A second
verbose run reproduced the same failure. Current source proves the test looked
for a backup/quarantine contract on HoldfastSaveStore, although that contract
belongs to HoldfastTradeSaveStore. No base backup was written; the trade backup
was present.

Pre-integration checkpoint: CHANGED PLAN. Added the exact runtime-test path to
claim BUG-HOLDFAST-RUNTIME-PROBE. Retargeted the checksum-corruption probe to the
trade store, isolated its files per invocation, required changed corrupt bytes,
and distinguished the backup balance (25) from the newer primary (20). Production
save stores remain untouched. Godot rebuild: 0 warnings / 0 errors.

The runtime initially also emitted missing-asset/faction-demand diagnostics and
parentless UI resource leak diagnostics. Follow-up repairs below resolve the
proven UI ownership defects; content diagnostics remain visible.

## Phase 5 — UI follow-up and final verification

Used the repair skill's reproduce-first workflow for each new root cause:

- [Grid lifecycle](BUG-GRID-LIFECYCLE_IMPLEMENTATION_LOG.md): persistent empty
  labels detached while populated (37 of the 100 orphan controls).
- [Panel allocations](BUG-PANEL-ORPHANS_IMPLEMENTATION_LOG.md): five overwritten
  headings plus two discarded/unused containers.
- [Panel inputs](BUG-PANEL-INPUTS_IMPLEMENTATION_LOG.md): two unparented pneumatic
  inputs made maintenance IDs inaccessible; inventory sidebar had no subscription.
- [Slurry cleanup](BUG-SLURRY-CLEANUP_IMPLEMENTATION_LOG.md): reused the existing
  panel instead of leaking its subtree; user explicitly authorized this single
  block inside the active Plan 24 claim. Original close wiring remains intact.

Temporary orphan traces removed. No global orphan cleanup or warning suppression.

Final commands/results:

```text
dotnet build Ashfall.csproj --no-restore -v minimal
  PASS: 0 warnings, 0 errors
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore -v minimal
  PASS: 0 warnings, 0 errors
bash scripts/ci/run-godot-bounded.sh --headless --path . -- --panel-bind-lifecycle-selftest
  PASS: 21/21 gates, exit 0, no engine warnings/errors or shutdown leaks
bash scripts/ci/run-godot-bounded.sh --headless --path . -- --holdfast-runtime-uitest
  PASS: every probe including quarantine and slurryOwned, exit 0,
  no engine warnings/errors or shutdown leaks
python3 scripts/ci/generate-docs-index.py --check
  PASS: 2657 documents
git diff --check -- <owned files>
  PASS
```

Godot runs used isolated XDG_DATA_HOME/CONFIG_HOME/CACHE_HOME under
`/tmp/ashfall-trade-repair.AqoLAz`, 15 FPS and the canonical 180-second cap; live
player saves untouched. Final runtime logs: `final-lifecycle.log` and
`final-holdfast.log` in that evidence directory. Earlier focused xUnit results:
76 trade + [45 warning-target cases](BUG-TEST-WARNINGS_IMPLEMENTATION_LOG.md) =
121/121 PASS. No full test suite.

## Resolution and handoff

Original bugs: phantom inventory grants, count/balance overflow, rounded funds
quotes, preview mismatch, stale recovery probe, UI ownership and input wiring.
Minimal repair: guards on existing trade authority and correct existing UI
parent/subscription ownership. Four permanent UI gates and the slurry bootstrap
assertion complement the 14 new xUnit regressions. Saves and deterministic RNG
unchanged; no new gameplay, catalog, wallet, registry, or save owner.

Adversarial checks cover extreme numeric values, failed-operation non-mutation,
actual backup-state recovery, populated/empty grid transitions, native Ready/Free,
chosen input IDs, filter round-trips, and existing ×100 bind/unbind cycles.

Status: **RESOLVED for the reproduced repair batch**, not a claim that the entire
repository is bug-free. Remaining scope: 12 missing item icons still use their
existing placeholders. Warlord validation emits 46 informational noncanonical
tribute-token notices and 3 intentionally unmerged alias notices, as explicitly
documented in WarlordDoctrineCatalog's validator contract. These are not hidden,
converted into fake gameplay items, or silently canonized. Visual/manual QA and
the full suite were not run.

Files/contracts: exact production/test/doc paths are recorded in the nine closed
`claim-bug-*` rows for this session. Main.UiHandlers has no retained diagnostic
edit. Shared paths intentionally untouched: Main.UiPanels, E1 governance, all
other active Plan 24 regions, authored data, save-store implementations.
Ready for sweep: yes. Existing dirty worktree edits preserved; no commit created.
