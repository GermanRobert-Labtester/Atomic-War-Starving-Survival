# PLAN-READINESS-PACKAGE-IDS-281 — Package IDs for the Wave 1 prose plans

**Wave 20 · Kind:** CLAIM-READINESS CLOSURE · **Status:** PROPOSED — foreman claim required.
**Depends on:** `PLAN-ORPHAN-SEAL-01` (§3), `PLAN-INTEGRATION-KIT-02` (§3),
`PLAN-UNBLOCK-03` (§4), and the Wave 1 package-format variants already in use.
**Non-goals:** no change to Wave 1 scope, order, or acceptance; no renumbering
of existing package IDs (`C4-*`, `B5-*`, `F6-*`); no production code.

## 1. Outcome

The three Wave 1 plans whose work is organised as prose sections
(`PLAN-ORPHAN-SEAL-01`, `PLAN-INTEGRATION-KIT-02`, `PLAN-UNBLOCK-03`) gain
stable, addressable package IDs, so that a builder can claim *one package* the
way every other plan in the programme allows. The three package formats already
in use across the programme are registered in one place, and the readiness
detector's package rule is widened to accept all three.

## 2. Premise evidence (re-verified 2026-09-21)

| Plan | Current §3 shape | Package headings found |
|---|---|---|
| `PLAN-ORPHAN-SEAL-01` | `## 3. Authority map and shared seams`; work is in `## 5. Waves` (Wave 1–N with prose system lists) | **none** |
| `PLAN-INTEGRATION-KIT-02` | `## 3. Deliverable design` with `### 3.1`–`### 3.5` numbered subsections | **none** (numbered, not ID'd) |
| `PLAN-UNBLOCK-03` | `## 4. Phases` with `### U0`–`### Un` headings | **none** (phase IDs, not package IDs) |

False negatives the detector produced in the same field:

| Plan | Actual heading | Format family |
|---|---|---|
| `PLAN-VERTICAL-CULTURE-04` | `### C4-1 — Culture day owner and calendar loop` | letter+digit prefix |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | `### B5-1 — Clinic Continuum (medical)` | letter+digit prefix |
| `PLAN-LAUNCH-FACE-06` | `### F6-1 — Input, Focus, Controller, Rebinding` | letter+digit prefix |

Three format families exist programme-wide and all are valid:

1. `- **XX-NNA** Description` (bullet form, used by most waves)
2. `### XX-NNA — Title` (heading form)
3. `### C4-1 — Title` (letter+digit prefix, Wave 1 verticals)

## 3. Packages

- ****281A** — Orphan Seal package IDs.** Convert the seal waves in
  `PLAN-ORPHAN-SEAL-01` §5 into addressable packages (`OS1-A`, `OS2-A`, … per
  wave/system group) while preserving the existing wave order and the risk
  balance recorded in Appendix Y/AF. Do not renumber Appendix AF's global seal
  order; add IDs alongside it.
- ****281B** — Integration Kit package IDs.** Give `PLAN-INTEGRATION-KIT-02`
  §3.1–§3.5 package IDs (tool, manifest, selftest, scaffolds, ledger checks) so
  each deliverable is separately claimable.
- ****281C** — Unblock package IDs.** Give `PLAN-UNBLOCK-03` phases `U0`–`Un`
  package IDs that keep the existing phase letters visible.
- ****281D** — Package-format registry and detector rule.** Record the three
  accepted families in the readiness contract (owned by
  `PLAN-READINESS-AUDITOR-284`) and widen the package rule to
  `^###\s+[A-Z][A-Z0-9]{0,5}-\d+[A-Z]?\b` plus the existing bullet and heading
  patterns.

## 4. Acceptance and verification

| Check | Command / evidence | Pass condition |
|---|---|---|
| IDs exist | `grep -cE '^### (OS|IK|UB)[0-9]' docs/plans/EXPANSION_PROGRAM_2026-09-21/PLAN-{ORPHAN-SEAL-01,INTEGRATION-KIT-02,UNBLOCK-03}.md` | ≥3 per plan, ≥9 total |
| Order preserved | diff of §5 wave order before/after | byte-identical except inserted IDs |
| No cross-reference breakage | `grep -n 'Appendix AB\|Appendix AF' PLAN-ORPHAN-SEAL-01.md` | links still resolve |
| Detector agreement | `python3 docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/claim-readiness-audit.py --check` | packages gap count `0` |
| Index truth | `generate-docs-index.py --check` | green |

## 5. Risks

- **Renumbering drift.** Appendix AB (batch→plan links) and Appendix AF (global
  seal order) reference Waves 1–N by order. Mitigation: add IDs, never reorder.
- **Fan-out edits.** Every plan upstream references Orphan Seal by wave, not by
  package; grep for `OS[0-9]`/`seal batch` before writing.
- **Over-claiming.** IDs must map to the existing scope; a new ID is not
  permission to widen a seal batch.

---

## 12. Cross-plan coupling
Document-artifact incoming edges: **0**.

| Plan | Mentions |
|---|---:|
| — | no other plan references these artifacts |

---

## 13. Authority binding map
Host files: **0** · Test regions: **0** · Data catalogs: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Test regions | 0 | — |
| Data catalogs | 0 | — |

---

## 14. Save-section ownership
Matching section keys: **0** (versioned ladders: **0**).

| Section key |
|---|
| — | no section key shares a token with this scope |

---

## 15. CLI and selftest coverage
Matching flags: **0**.

| Flag |
|---|
| — | no CLI flag shares a token with this scope |

---

## 16. Event-route reachability
Events sharing a scope token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this scope |

---

## 17. Data-catalog binding
Matching catalogs: **0**.

| Catalog | Classification |
|---|---|
| — | no catalog shares a token with this scope |

---

## 18. Test-region mapping
Matching regions: **0** (0 files, 0 cases). Root-level files outside regions: 560 files / 5731 cases.

| Region | Files | Cases |
|---|---:|---:|
| — | no region shares a token with this scope |

---

## 19. Host-surface route map
Matching host files: **0**.

| Host file |
|---|
| — | no host filename shares a token with this scope |

---

## 20. Save schema ladder
Matched sections: **0**; laddered: **0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this scope |

---

## 21. Determinism / RNG stream binding
Matching seeded streams: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this scope |

---

## 22. Content-consumption classification
Matching catalogs: **0**.

| Catalog | Classification |
|---|---|
| — | no catalog shares a token with this scope |

---

## 23. Flag wiring
Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this scope |

---

## 24. Claim readiness
**Readiness:** READY-WITH-NOTES (10/12) · **Class:** governance/closure · **Coupling:** 0

**Proposed claim block**

```
plan: PLAN-READINESS-PACKAGE-IDS-281
wave: 20
status: PROPOSED — foreman claim required
packages: author at claim time
claim paths:
  - docs/plans/EXPANSION_PROGRAM_WAVE20_2026-09-21/  # this plan
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve at claim time
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | **no** |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages, verification.
