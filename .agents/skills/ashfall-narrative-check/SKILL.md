---
name: ashfall-narrative-check
description: Reviews ASHFALL narrative content for player reachability, state-machine completeness, tone, fictional-world compliance, and mechanical text/data alignment. Use after narrative edits; use ashfall-narrative-continuity for broad cross-file graph integrity.
---

# ASHFALL Narrative Acceptance Check

## Boundary

`ashfall-narrative-continuity` owns broad ID, flag, contradiction, and graph
audits. This skill focuses on whether a narrative slice is playable, legible,
and consistent with its intended systemic contract.

## Workflow

1. Identify the narrative slice, entry conditions, exit conditions, flags,
   choices, effects, and referenced runtime systems.
2. Trace each branch from an actual reachable entry to a valid terminal state or
   documented continuation. Flag dead ends, impossible conditions, duplicate
   choices, and effects with no consumer.
3. Compare displayed text and labels with current IDs, item quantities,
   locations, faction stances, and timing. Do not rewrite prose to conceal a
   mechanical mismatch.
4. Check the restrained ASHFALL tone and fictional-world rule. Flag real-world
   countries, wars, people, glorified violence, fantasy, or magic as findings
   with exact evidence.
5. Separate `BLOCKING`, `CONTENT_DECISION`, `STYLE`, and `UNVERIFIED` findings.
   Ask for owner decisions where multiple canon resolutions are valid.

## Rules

- Read-only by default; do not rewrite narrative JSON.
- Never add IDs, flags, or branches during an audit.
- Use current data authority and code reachability, not a stale plan.
- Hand approved prose changes to `ashfall-write` or `ashfall-expand`; hand
  approved code/data integration to `ashfall-implement`.
- Run the narrative and data self-tests after approved edits.

## Output

Return the slice map, branch/reachability findings, and mechanical mismatches
with file/line or JSON-path evidence, plus content-rule evidence and owner
decisions needed. If a report is requested, use
`docs/narrative/ACCEPTANCE_<slice>.md` and preserve continuity reports.

## Quality gate

- Every blocking finding includes a reproducible path or exact missing edge.
- Tone judgments identify the relevant rule and quoted context.
- No continuity issue is relabeled as a style preference.
