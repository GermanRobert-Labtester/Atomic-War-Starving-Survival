# Plan Bloat Reduction — Three Unintegrated Plans (2026-09-27)

User-authorized creative bloat analysis ("pick 3 plans, determine what is bloat
and what to cut, immediate reducing") executed by the docs-atlas pass.
No production code, data, or tests were touched.

## 1. Bloat anatomy (evidence)

Each of the three selected plans had ~205,000 lines / 11.4 MB, of which only
~2,300–2,630 lines (~110 KB) were the operative plan. The remaining ~99% was
template-generated volume padding appended after `# SECTION IX`:

- `SECTION XII: DEEP POLISHING PASS — HIGH-VOLUME ARCHIVAL DOSSIERS` —
  hundreds of near-identical "incident record" blocks with mad-libs prose
  ("anomalous resonance was detected across the X interface", "operational
  flux exceeding nominal boundaries by 13%").
- `SECTION XIV: 110 ARCHIVAL INQUEST CHRONICLES & TRIBUNAL DEPOSITIONS` —
  the same narrative template re-skinned per file.
- Dozens of closing "mega-sections" (up to 60.x) that are thematically
  unrelated to the plan topics — e.g., the rail-maintenance/glassworks plan
  ends with liquid-argon cryogenic detector physics and silicon
  photomultiplier dark counts.

Only ~24,000 of ~205,000 lines per file were unique (88% within-file
duplication). The padding is not authored game content: none of the dossier
IDs exist in `Assets/StreamingAssets/Data/`, no code or other document
references them, and the operative plan regions contain zero references to
the padding sections.

## 2. Files cut (keep operative plan verbatim, remove SECTION XII → EOF)

| File | Before | After | Removed |
|---|---:|---:|---:|
| `docs/plans/UNBLOCK_EXPANSION25_29_INTEGRATION_PLAN.md` | 204,970 lines / 11.47 MB | 2,307 lines / 105 KB | 99.1% |
| `docs/plans/UNBLOCK_OLDEST_PLAN181_INTEGRATION_PLAN.md` | 204,974 lines / 11.47 MB | 2,311 lines / 105 KB | 99.1% |
| `docs/plans/EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-CONTENT-ACCEPTANCE-FAMILY-TRUTH-274.md` | 203,535 lines / 11.47 MB | 2,633 lines / 116 KB | 99.0% |

Total: **34.4 MB → 326 KB**. Selection criteria: largest unintegrated plan
files, clean in the worktree (no concurrent-lane edits), unclaimed in
`WORKTREE_OWNERSHIP.md`. The first two describe packages that
`INTEGRATION_PLANS.md` marks COMPLETE (2026-09-24); the third (Plan 274) is a
genuinely pending plan whose operative acceptance content is fully retained.
Each trimmed file carries a provenance note; the full removed text remains
in git history at `241a179fb` (`git show 241a179fb:<path>`).

## 3. Remaining opportunity (foreman decision required)

**695 further unintegrated `docs/plans/` files carry the same `SECTION XII`
padding template, totaling 7.1 GB** — the dominant share of the 29 GB
Markdown corpus that makes the docs-index pre-commit hook take ~50 minutes
per markdown commit on this 6 GB machine (see
`docs/performance/ASHFALL_PERFORMANCE_AUDIT.md`, PERF-06). A batch pass with
the same keep-operative-plan/append-note protocol would remove ~7 GB with no
loss of operative plan content, but it is a large, foreman-gated change and
was not executed here beyond the user-authorized sample of three.
