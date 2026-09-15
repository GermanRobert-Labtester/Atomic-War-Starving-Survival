# ASHFALL AI Agent Workflow

This file governs agent roles, evidence, handoffs, and escalation. `AGENTS.md`
holds universal architecture rules; the current batch and path claims live in
`INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`.

## Read order

1. `AGENTS.md`
2. `INTEGRATION_PLANS.md`
3. `WORKTREE_OWNERSHIP.md`
4. `TEST_POLICY.md`
5. `KNOWN_DEBT.md`

Read domain documentation only after the batch identifies the authoritative
system, data, and host path.

## Roles

| Role | Typical model class | May do | Must not do |
|---|---|---|---|
| Foreman | Strong reasoning model | Verify premise, form packages, claim paths, accept handoffs | Routine implementation outside integration seams |
| Builder | Capable coding model | Implement one owned package and focused checks | Touch another claim or invent scope |
| Sweep | Cheap/fast model | Read-only search, log triage, stale API and duplicate detection | Edit code or promote unproven findings |
| Reviewer | Different cheap/mid model | Check a diff against its contract | Race the builder with another rewrite |
| Integrator | Strong coding/reasoning model | Join accepted packages and own shared seams | Add unrelated features |

Examples of sweep-capable models include Stepfun Flash, GLM Flash, Luna, and
their future equivalents. Choose by capability and cost, not by a hard-coded
vendor name.

## Package protocol

Before editing, a builder must have a package containing:

- outcome and explicit non-goals;
- premise evidence from current source/data;
- exact owned paths and any integrator-only shared paths;
- existing authority, save owner, and event/host seam when applicable;
- 3-6 observable acceptance criteria;
- exact focused verification commands.

If current evidence disproves the premise, stop and return `STALE_PLAN` or
`STALE_TEST` to the foreman. Do not adapt a deprecated API merely to keep a
plan moving.

## Handoff format

```text
Package:
Outcome:
Files changed:
Current contract used:
Verification commands and results:
Tests reused / added / aggregated:
Known limitation or debt:
Shared files intentionally untouched:
Ready for sweep: yes/no
```

Reasoning transcripts are not handoff artifacts. The diff, current contract,
commands, results, and remaining limitation are.

## Sweep protocol

Sweep agents are read-only. Every finding must include:

```text
finding_id | severity | confidence | path:line | current evidence | expected contract | proposed owner
```

Permitted checks: build errors, stale symbol use, unbound host/event/save
paths, duplicate authority, invalid catalog references, missing lifecycle
cleanup, accidental Unity use, and focused-test failures. A finding without
path-level evidence is discarded rather than implemented.

## Escalation

Stop and return the package when it needs a new authority, overlaps a live
claim, changes multiple composition roots unexpectedly, cannot reproduce its
failure, restores retired architecture, or exceeds its stated outcome.

Only the foreman edits the integration ledger and ownership ledger. Only the
integrator edits a shared path during integration.
