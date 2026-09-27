# Plan Quad Package M — Portable Relative-Link Sweep (Gate Proven)

> **STATUS: APPROVED BY USER**

## Bounded Outcome

Close `DocLinkValidationGateTests.ZeroMachineSpecificFileUris_InDocumentation`, which
currently reports **206 machine-specific documentation links**, by rewriting each to a
portable repository-relative path.

## Scope (measured, not estimated)

A full-repository scan (the gate's own filter: every `*.md` excluding `.git/`, `bin/`,
`obj/`) found exactly **206 matches**, and they are remarkably uniform:

- **One single link target**, repeated 206 times across **168 distinct files**:
  `../newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
- **All 206 are `/home/…` absolute paths**; there are **zero `file:///` URIs**.
- The target file **exists** (635,647 bytes, `docs/newest-…-volumes-1-57.md`), so the
  links are live, not dangling — they only need to become portable.
- **No match carries an anchor or query string** — each is exactly
  `[label](<absolute path>)`.

That uniformity makes this a single deterministic transformation rather than 206
individual judgements.

## Safety checks performed before editing

- **All 168 affected files are clean in git** (`git diff --quiet` passes on every one),
  so no concurrent lane has uncommitted work in any file this sweep touches (Rule 6).
- The transformation is **idempotent**: a rewritten relative path does not contain
  `/home/`, so re-running the sweep cannot double-convert.
- Only the link target substring is replaced; no other content is touched, so this is not
  a mass format.

## Method

For each affected file, compute `os.path.relpath(target_file, dirname(doc))` with forward
slashes, and substitute it for the absolute path:

| Doc location | Rewritten to |
|---|---|
| `docs/foo.md` | `newest-ashfall-…-volumes-1-57.md` |
| `docs/expansions/foo.md` | `../newest-ashfall-…-volumes-1-57.md` |
| `README.md` | `docs/newest-ashfall-…-volumes-1-57.md` |

## Non-Goals

- No changes to the linked authority document itself.
- No other link kinds touched (`http(s)`, `#anchor`, `mailto`, already-relative links).
- Not a commit; not a full-suite run.

## Files

- 168 markdown documents under `docs/`, `piagentsplans/`, `.ai/`, and repository roots.
- `docs/plans/PLAN_QUAD_PACKAGE_M_PORTABLE_RELATIVE_LINKS.md` — this document.

## Verification (bounded)

- Post-sweep scan: **0** machine-specific links (the gate's exact regex and filter).
- Every rewritten link resolves to an existing file (belt-and-braces check beyond the gate).
- `DocLinkValidationGateTests.ZeroMachineSpecificFileUris_InDocumentation`: **green**.
- `DocLinkValidationGateTests.AuthorityDocs_RelativeLinksResolveToExistingFiles`: still **green**.
- One adjacent sweep; no full suite.
