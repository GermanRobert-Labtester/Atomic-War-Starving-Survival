# PLAN-READINESS-HEADER-NORMALISATION-283 — Wave/kind header contract

**Wave 20 · Kind:** CLAIM-READINESS CLOSURE · **Status:** PROPOSED — foreman claim required.
**Depends on:** `PLAN-READINESS-AUDITOR-284` (rule owner); Wave 3–5 headers as they exist today.
**Non-goals:** no prose rewrite; no change to any plan's scope, packages,
order, or acceptance; no mass-format of shared areas.

## 1. Outcome

The programme states which wave/kind header forms are legal, proves both forms
are in use and equivalent, and fixes the readiness detector so a legal header
never reads as a gap. A single mechanical normalisation pass is offered for
uniformity but is explicitly optional.

## 2. Premise evidence (re-verified 2026-09-21)

Twenty plans were reported as not matching `**Wave N`. Nineteen of them use the
equally valid form below; the twentieth (`PLAN-VERTICAL-BODY-INDUSTRY-05`) is a
Wave 1 plan whose wave line sits in a different header shape.

```
**Wave:** 3 (2026-09-21) · **Kind:** GAP SEALING
```

Affected plans: `05 · 21 · 23 · 24 · 25 · 26 · 31 · 32 · 33 · 34 · 35 · 37 · 41 ·
42 · 44 · 45 · 46 · 47 · 48 · 49`.

Observed kinds in that set: `GAP SEALING`, `EXPANSION`, `MAJOR EXPANSION`.

## 3. Packages

- ****283A** — Header contract.** Declare both forms legal:
  - canonical: `**Wave N · Kind:** <KIND>`
  - accepted: `**Wave:** N (YYYY-MM-DD) · **Kind:** <KIND>`
  Publish the closed set of `KIND` values actually in use (`GAP SEALING`,
  `EXPANSION`, `MAJOR EXPANSION`, `CLAIM-READINESS CLOSURE`).
- ****283B** — Optional uniformity pass (documentation-only). Rewrite the header
  line of the twenty plans to the canonical form, changing nothing else in the
  file. Reviewable as a pure mechanical diff: one changed line per file.
- ****283C** — Directory-vs-header truth check.** Every plan's declared wave must
  equal the wave implied by its directory name
  (`EXPANSION_PROGRAM_WAVE<N>_<date>`), except Wave 1's base directory which
  implies Wave 1. This catches a real class of error — a plan filed in the
  wrong wave directory — that the cosmetic check cannot.
- ****283D** — Index correction.** Update the residual-gap table in
  `CLAIM_READINESS_INDEX.md` to record zero real header gaps and link this plan.

## 4. Acceptance and verification

| Check | Command | Pass condition |
|---|---|---|
| Both forms accepted | `claim-readiness-audit.py --check` | wave gap count `0` |
| Uniformity (if 283B done) | `grep -c '\*\*Wave [0-9]' <twenty files>` | 20/20, and diff shows one line changed per file |
| No other edits | `git diff --stat` on the twenty files | 1 insertion / 1 deletion each |
| Directory truth | auditor's directory-vs-header rule | 0 mismatches across 276 |
| Index honest | §4 corrected | no false claim of malformed headers |

## 5. Risks

- **Mass-edit hazard.** Touching twenty plan files risks unrelated churn.
  Mitigation: single-line edits only; verify with a before/after diff; stop if
  any hunk exceeds one line.
- **Cosmetic churn for no functional gain.** 283B is optional by design. If the
  foreman prefers, skip it — the contract (283A) plus the detector fix (283C)
  already removes the false gap.
- **Directory truth reveals a real mismatch.** If the check finds a plan filed
  in the wrong wave directory, that is a genuine finding: escalate to the
  foreman rather than moving files silently.

---

## 12. Cross-plan coupling
Document-artifact incoming edges: **2**.

| Plan | Mentions |
|---|---:|
| `PLAN-READINESS-AUDITOR-284` | 1 |
| `PLAN-READINESS-VERIFICATION-CONTRACT-282` | 1 |

---

## 13. Authority binding map
Host files: **1** · Test regions: **0** · Data catalogs: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/UniqueClaimSaveStore.cs` |
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
Matching host files: **1**.

| Host file |
|---|
| `src/Host/UniqueClaimSaveStore.cs` |

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
**Readiness:** READY-WITH-NOTES (10/12) · **Class:** governance/closure · **Coupling:** 2

**Proposed claim block**

```
plan: PLAN-READINESS-HEADER-NORMALISATION-283
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
