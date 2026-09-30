# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan 53 E1C — Plan Metadata Migration

**STATUS: APPROVED BY USER** (explicit reply: “Yes plan 53!”)

## Bounded outcome

Complete Plan 53 phase E1C: add a deterministic, one-time, dry-run-first migrator that completes front matter for the current 621-path inventory; preserve every Markdown body byte and line ending; record field decisions, unresolved review items, and pre/post hashes; regenerate the canonical register and prove execution-baseline path parity, no duplicate IDs, zero inferred `DONE`, and an idempotent second run. Preserve E1A's 609-path historical snapshot unchanged and document all 8 missing legacy paths and 20 additions.

## Scope

- Tool: `scripts/ci/migrate-plan-metadata.py` (one-time; archive after E1C).
- Fixtures and focused migration tests under `scripts/ci/fixtures/plan_governance/` and `Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` only if compatible with the existing test surface.
- Migration report and execution baseline under `docs/roadmap/e1/`.
- Data corpus: exact relative paths in `docs/roadmap/e1/E1C_EXECUTION_BASELINE.json::plan_file_list`; `e1_baseline.json` remains immutable historical evidence.
- Governance/generated outputs: `WORKTREE_OWNERSHIP.md`, `.ai/state.md`, `INTEGRATION_PLANS.md`, `docs/roadmap/PLAN_REGISTER.{md,json}`, and generated docs index.

## Guardrails

- Preserve unknown front-matter keys and original body bytes; never infer a premise verification date or `DONE` from prose.
- Use stable path-derived IDs when filename identity is ambiguous; record uncertain title/category/status proposals for human review.
- Dry-run is the default. `--write` requires the digest printed by a reviewed dry run.
- User explicitly authorized reassignment of the seven PFGL-held E1A plan documents plus the PFGL-held Plan 41 file newly in the current inventory. Record this handoff in `WORKTREE_OWNERSHIP.md` before editing.
- Do not alter gameplay source/data/save/UI, run the full suite, or commit.

## Definition of done

Focused tests and tool self-tests pass; reviewed dry-run and write proposal sets match; all execution-baseline paths are migrated; second migration is a no-op; report proves byte-identical post-front-matter bodies and exact path parity; regenerated register is clean; no inferred `DONE`; plan is marked FULLY INTEGRATED at the top and immediately archived.
