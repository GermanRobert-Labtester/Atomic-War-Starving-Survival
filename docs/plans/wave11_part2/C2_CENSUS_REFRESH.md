# Wave 11 Part 2 C2 — Census Refresh and Queue Maintenance

**Date:** 2026-09-18\
**Status:** `DEPENDENCY-BLOCKED` — partial actuality refresh completed; final Wave 11 rerank and Wave 12 handoff wait on C1 foreman decisions.

## Re-verified Wave 11 outcomes

| Row / package | Current terminal truth | Evidence / action |
|---|---|---|
| C1[11]–C1[15] | `SEALED` | Part 1 logs and focused tests remain recorded in `WAVE11_PART1_CLOSEOUT.md`. |
| C2[10] / Plan 30 | `PARTIALLY-SEALED` / decision-blocked continuation | Corrected a false five-event claim: `OnFactionStandingChanged` is bound by `FactionWarMapWidget`; five other emitted events remain without `src/` subscribers. No change to the runtime-clock or consequence-reach blocker. |
| C2[11] / Plan 32 | `PARTIALLY-SEALED` | Orphan-node, graph-travel, and geographic-knowledge remainders remain promoted and decision-gated as documented in the Part 1 log. |
| C2[12] / Plan 34 | `PARTIALLY-SEALED` | User-authorized B3 now records canonical epilogue completion facts into checksum-validated, append-only `user://completion_history.json`; campaign saves remain untouched. Canonical difficulty authority (34B) and history/chronicle projection contract (34C) remain unsealed. |
| C2[13] / Plan 36 | `PARTIALLY-SEALED` | Existing 144-row static policy is not the historical whole-contract closure; promoted `DEBT-PLAN36-PORT-CONTRACT-CLOSURE`. |
| C2[14] / Plan 35 | `RECONCILED-DUPLICATE` | C1[10] is the sole executable authority; C2[14] is bannered provenance. |

## Duplicate result

`python3 scripts/ci/detect-corpus-duplicates.py --check` found one exact duplicate: C1[10] and C2[14], both Plan 35. It is classified in the census duplicate registry and the B5 reconciliation log. No other exact cross-corpus source-Plan-ID duplicate was found.

## Measured drains (reproducible definitions)

| Drain | Count | Definition / qualification |
|---|---:|---|
| Corpus queue | **117** | All nonterminal corpus rows in `UNCLAIMED_CORPUS_CENSUS.md`: 111 `AUDIT-PENDING` + 5 `PARTIALLY-SEALED` + 1 `READY-UNCLAIMED`. Superseded/reconciled duplicates are excluded. |
| Decision register | **6 formal deferred rows; final count not certified** | The formal register has six `DEFERRED-WITH-CONDITION` rows. Its source-signature truth conflicts with current closeouts and the 23-item foreman packet, so C1 must complete before publishing a final Wave 11 decision count. |
| Formal CI quarantine registry | **0** | `scripts/ci/quarantine.json` contains an empty `quarantines` list. The 50 project-file `Compile Remove` exclusions are not entries in this formal runtime-quarantine registry and are not counted as K. |

## Wave 12 pre-verification

**No queue head is certified.** The historical candidate C1[16] (Plan 49) declares Plan 42 and Plan 46 prerequisites whose terminal source status is not established by the current census; C2[15] (Plan 37) likewise requires a separate premise/dependency/claim audit. Selecting either would violate the current-evidence rule.

## Final

**Production changes:** none in this refresh.\
**Remaining blocker:** C1 foreman signatures and a resulting reconciled decision register.\
**Required next action:** after C1 is signed, rerun the counts, dependency graph, claims check, and head premise-check at the then-current `HEAD`.
