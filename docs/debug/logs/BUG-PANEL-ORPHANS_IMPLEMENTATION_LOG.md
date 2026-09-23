# BUG-PANEL-ORPHANS implementation log

## Phase 1 — heading reproduction

Pre-integration checkpoint PASS. Exact panels unclaimed; current _Ready and
RefreshView/RefreshDetail paths confirmed. Gate 19 fails for each of the five
panels with exactly one new orphan per Ready/Free. Gates 1–18 pass. Evidence:
/tmp/ashfall-trade-repair.AqoLAz/headings-before.log.

## Phase 2 — heading repair

Pre-integration checkpoint PASS. Removed only the redundant unparented heading
allocation in each panel. Existing parented/refresh headings untouched, with no
gameplay, persistence, event, or RNG change. Five heading probes passed before
the following remaining-container repair.

## Phase 3 — remaining container reproduction and repair

Pre-integration checkpoint PASS. Per-panel Ready traces narrowed the remaining
two containers to MedicalPanel and EconomyDetailPanel; exact claims added.
Extended Gate 19 fails with one orphan for each panel; the five heading probes
now pass. Evidence: /tmp/ashfall-trade-repair.AqoLAz/containers-before.log.
Removed only the discarded temporary medical content and unused initial economy
detail allocation. Real scroll content and on-demand regional details unchanged.
Temporary diagnostic instrumentation removed; no forced global cleanup retained.

## Final verification

Host build 0 warnings/errors. Final lifecycle 21/21, including all seven independent
Ready/Free probes, and holdfast runtime PASS. Neither runtime emitted engine
warnings/errors or shutdown leaks. Debug-only orphan enumeration explicitly
reports Gate 19 skipped in a release engine; the verified run used the debug
editor engine. Status: RESOLVED. Exact commands in the holdfast implementation log.
