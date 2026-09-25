# Task Plan

STATUS: APPROVED BY USER

## 1. High-Level Brief & Goals
- **Objective:** Commit all pending changes across the working tree (brand new files, insertions, and deletions) cleanly in accordance with repo governance, pre-commit gates, and release standards.
- **Non-Goals:** Do not create duplicate systems or rewrite git history.

## 2. Target Files & Claims
- **Target Files:** All working tree changes, including core subsystems, tools/gotools, docs, generated catalogs, art imports, tests, and configurations.
- **Worktree Ownership Check:** Disjoint integrator commit requested explicitly by the user.

## 3. Pre-flight Checks
- [x] Run Go config/save validators (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [x] Whitespace scan (`no-whitespace-churn.sh`, `git diff --check`)
- [x] JSON schema policy gate (`scripts/ci/json-schema-policy-gate.sh`)
- [x] Asset orphan sweep (`scripts/ci/asset-orphan-sweep.sh`)
- [x] CLI and documentation catalogs synchronized with live engine and tools

## 4. Execution Steps
1. Resolve asset sidecars and whitespace hygiene across modified files.
2. Regenerate documentation indices and catalog drifts.
3. Verify pre-commit hooks and release gate compliance.
4. Stage all new, modified, and deleted changes.
5. Create comprehensive git commit.

## 5. Verification & Termination Criteria
- [x] Defined "done" criteria met
- [x] Scoped tests and pre-commit checks pass
- [x] No trailing whitespace or orphan assets
- [x] Clean git status after commit
