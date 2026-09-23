# E1 Tooling and Rails Implementation Log (Plan 53 / E1D, E1H, E1I)

Date: 2026-09-20
Phase: Plan 53 / E1 Governance Programme Integration

---

## 1. Scope & Delivered Capabilities

This integration seals the tooling and registry foundations for Plan 53 (Ambition Governance, Expansion Intake, and Rails Readiness):

1. **E1D Reference Freshness Verifier (`scripts/ci/verify-plan-freshness.py`):**
   - Implements automated, high-precision scanning of all citations (`path:line`) across all 609 in-scope plans.
   - Categorizes every reference into canonical statuses: `OK`, `MISSING_PATH`, `MISSING_LINE_RANGE`, `RENAMED_CANDIDATE`, `AMBIGUOUS`.
   - Supports CLI flags: `--check`, `--summary`, `--plan <path>`, `--self-test`.
   - Verified on full corpus: 609 plans audited with zero crashes.

2. **E1H Canonical Rails Readiness Registry (`scripts/ci/generate-rails-registry.py`):**
   - Authored canonical definitions in `docs/roadmap/rails.registry.json` covering the 10 core rails: `graph`, `intel`, `identity`, `voice`, `policy`, `relations_outcomes`, `seasons`, `commitments`, `acceptance_ladder`, `port_contract`.
   - Generator creates `docs/roadmap/rails.json` (machine-readable substrate) and `docs/roadmap/RAILS.md` (clean markdown table).
   - Validates readiness states against lifecycle (`NOT_STARTED`, `IN_FLIGHT`, `CODE_READY`, `RUNTIME_VERIFIED`, `PRESENTED`, `DONE`).
   - Strictly enforces on-disk existence of evidence anchors for any rail in `CODE_READY` or higher.
   - Enforces dependency-cycle detection across rail consumers and blocking graphs.

3. **E1I Plan Intake Gate (`scripts/ci/plan-intake-check.py` & `scripts/ci/plan-intake-check.sh`):**
   - Validates front matter structure and metadata schema (`PLAN_ID`, `TITLE`, `STATUS`, `CATEGORY`).
   - Enforces system seam extensions and duplicate prevention rationale on `SYSTEM` plans.
   - Enforces underlying authority and read-model clarity on `PRESENTATION` plans.
   - Rejects unknown required rails against the canonical rails registry (`RAILS_REQUIRED`).
   - Supports fast checks on changed files via git (`--changed`), explicit plan file checks (`--plan`), full corpus sweeps (`--check`), and self-tests (`--self-test`).

4. **Contract Verification (`Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`):**
   - Added xUnit contract test `CanonicalRailsRegistryHasStableSchemaAndEvidenceOnDisk` verifying that all 10 canonical rails have valid schema and verified on-disk evidence paths.
   - Added `PlanGovernanceToolingScriptsExistAndAreConfigured` verifying the existence and configuration of all E1 tooling scripts.

---

## 2. Verification Summary

- `python3 scripts/ci/verify-plan-freshness.py --self-test` — **PASS**
- `python3 scripts/ci/verify-plan-freshness.py --summary` — **PASS** (609 plans audited)
- `python3 scripts/ci/plan-intake-check.py --self-test` — **PASS**
- `python3 scripts/ci/plan-intake-check.py --plan scripts/ci/fixtures/plan_governance/unknown_rail.md` — **PASS** (correctly rejected unknown rail)
- `python3 scripts/ci/generate-rails-registry.py --self-test` — **PASS**
- `python3 scripts/ci/generate-rails-registry.py --write && python3 scripts/ci/generate-rails-registry.py --check` — **PASS** (clean sync)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` — **5/5 PASS**
