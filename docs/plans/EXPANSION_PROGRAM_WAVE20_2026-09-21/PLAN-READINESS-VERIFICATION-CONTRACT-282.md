# PLAN-READINESS-VERIFICATION-CONTRACT-282 — Canonical verification commands

**Wave 20 · Kind:** CLAIM-READINESS CLOSURE · **Status:** PROPOSED — foreman claim required.
**Depends on:** `PLAN-READINESS-PACKAGE-IDS-281` (format registry),
`PLAN-READINESS-AUDITOR-284` (rule owner), `TEST_POLICY.md` (focused-test rule).
**Non-goals:** no new tests; no change to `TEST_POLICY.md`; no production code;
no broadening of what `scripts/run_test.sh` accepts.

## 1. Outcome

Every plan's §24 claim block carries at least one **runnable** verification
command drawn from a published family list, and the readiness detector stops
reporting a plan as unverified when its command merely uses a different family.
The contract is explicit so a builder never has to invent a command.

## 2. Premise evidence (re-verified 2026-09-21)

Five plans were reported as having no verification command. All five do cite
real commands; the detector only recognised three families.

| Plan | Real command(s) already cited | Family |
|---|---|---|
| `PLAN-RELEASE-OPS-20` | `bash scripts/ci/…` (asset/orphan/audio gates), release scripts | CI shell gate |
| `PLAN-DEBT-DRAIN-24` | `python3 scripts/ci/check-register-truth.py --check`; `python3 scripts/ci/generate-docs-index.py --check` | Python `--check` |
| `PLAN-ORIGINALITY-LICENSING-60` | `bash scripts/ci/license-header-check.sh --strict`; `python3 scripts/ci/generate-asset-registry.py --check` | CI shell gate + Python `--check` |
| `PLAN-HOST-COMPOSITION-GOVERNANCE-71` | `python3 scripts/ci/generate-host-composition.py --check` | Python `--check` |
| `PLAN-DOC-ATLAS-CURRENCY-115` | `python3 scripts/ci/generate-docs-index.py --check` | Python `--check` |

## 3. Packages

- ****282A** — Command-family contract.** Publish the five accepted families in
  one place (owned by Plan 284's contract file):
  1. focused xUnit — `bash scripts/run_test.sh <region-or-file>/`
  2. host selftest — `godot --headless --path . -- --<domain>-selftest`
  3. generated-output check — `python3 scripts/ci/<generator>.py --check`
  4. CI shell gate — `bash scripts/ci/<gate>.sh [--strict]`
  5. release gate — `bash scripts/ci/release-gate.sh` / `scripts/release/*`
- ****282B** — §24 backfill.** Add the explicit command line(s) to the §24 claim
  block of the five plans above, quoting the command already in the body — no
  new command invention, no new work implied.
- ****282C** — Index correction.** Correct the residual-gap table in
  `CLAIM_READINESS_INDEX.md` from "5 plans without verification" to "0 real; 5
  detector false negatives" and link this plan as evidence.
- ****282D** — Regression check.** Extend the readiness auditor's verification
  rule to the five families; a plan counts as unverified only when no family
  matches anywhere in its body.

## 4. Acceptance and verification

| Check | Command | Pass condition |
|---|---|---|
| Families published | `grep -c '^### Family' <contract file>` | 5 |
| Backfill present | `grep -l 'scripts/ci/' <five plan files>` | 5/5 files |
| Auditor agreement | `python3 …/tools/claim-readiness-audit.py --check` | verification gap count `0` |
| Focused runs green | run each cited command once | exit 0 (or documented pre-existing failure, quarantined per `TEST_POLICY.md`) |
| Index honest | `CLAM_READINESS_INDEX.md` §4 corrected | no false claim of missing commands |

## 5. Risks

- **Command rot.** A cited script can be renamed. Mitigation: the auditor
  resolves each command path at check time and fails loudly when a referenced
  script is missing.
- **Treating a gate as a test.** A `--check` proves generated outputs are
  current; it does not prove gameplay behaviour. Plans whose acceptance is
  behavioural must still name a focused xUnit region or a selftest.
- **Copy/paste drift into `TEST_POLICY.md`.** This plan documents families; it
  does not relax the focused-test rule or the 180-second cap.

---

## 12. Cross-plan coupling
Document-artifact incoming edges: **276**.

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-READINESS-AUDITOR-284` | 2 |
| `PLAN-READINESS-HEADER-NORMALISATION-283` | 2 |
| `PLAN-RUNTIME-PERF-16` | 2 |
| `PLAN-SAVE-GOVERNANCE-12` | 2 |
| `PLAN-TEST-WELFARE-17` | 2 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 2 |
| `PLAN-AUTOMATED-QA-CAMPAIGNS-74` | 2 |

---

## 13. Authority binding map
Host files: **4** · Test regions: **0** · Data catalogs: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/CSharpVerificationTest.cs`, `src/Host/PortContractSelfTest.cs`, `src/Host/UniqueClaimSaveStore.cs`, `src/UI/AshfallFocusPolicy.cs` |
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
Matching flags: **1**.

| Flag |
|---|
| `--port-contract-selftest` |

---

## 16. Event-route reachability
Events sharing a scope token: **5**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |

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
Matching host files: **4**.

| Host file |
|---|
| `src/CSharpVerificationTest.cs` |
| `src/Host/PortContractSelfTest.cs` |
| `src/Host/UniqueClaimSaveStore.cs` |
| `src/UI/AshfallFocusPolicy.cs` |

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
**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance/closure · **Coupling:** 276

**Proposed claim block**

```
plan: PLAN-READINESS-VERIFICATION-CONTRACT-282
wave: 20
status: PROPOSED — foreman claim required
packages: author at claim time
claim paths:
  - docs/plans/EXPANSION_PROGRAM_WAVE20_2026-09-21/  # this plan
verification:
  - godot --headless --path . -- --port-contract-selftest
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
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages.
