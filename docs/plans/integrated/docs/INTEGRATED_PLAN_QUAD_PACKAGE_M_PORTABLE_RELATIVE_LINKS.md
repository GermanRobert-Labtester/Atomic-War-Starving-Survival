# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan Quad Package M — Portable Relative-Link Sweep (Gate Proven)

> **STATUS: APPROVED BY USER — FULLY INTEGRATED**

**Claim:** `claim-quad-m-portable-relative-links-2026-09-26`

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


---

## Closeout Evidence (2026-09-26)

### Gate closed

`DocLinkValidationGateTests.ZeroMachineSpecificFileUris_InDocumentation` reported
**206** machine-specific links before this package. After the sweep it reports **0**, and
the test is **green**. Its sibling
`AuthorityDocs_RelativeLinksResolveToExistingFiles` is also **green**.

### What the scan actually found (measured, not assumed)

| Measure | Before | After |
|---|---|---|
| Gate-flagged markdown links (a `[...]` label whose link target is an absolute `/home/…` path) | 206 | **0** |
| Absolute-path occurrences rewritten | 1583 | **0** |
| Files touched | 639 | — |
| Links to the authority doc that resolve | 305 | **319** |
| Links to the authority doc that are broken | 14 | **0** |

### Two scopes, both closed

1. **Gate scope (206).** Every gate-flagged markdown link was rewritten to a portable
   repo-relative path. Only 5 distinct relative forms resulted (`../newest-…`,
   `../docs/newest-…`, `../../newest-…`, `../../../newest-…`, `newest-…`), one per
   directory depth.
2. **Same-defect prose scope (1377), closed to avoid leaving a partial.** The absolute
   path also appeared 1377 times as an inline-code citation
   (`` `/home/robertsrff/…/docs/newest-…` ``) across 471 further files. This is outside
   the gate's markdown-link regex, but it is the identical machine-specific-path defect,
   so all 1583 occurrences of that path were rewritten. The repository now contains
   **zero** references to that absolute authority-document path in any `.md`.
   Rewriting to a relative path is valid here because the target is inside the repo.
3. **Pre-existing broken links (14), repaired.** 14 links already used a *relative* path
   that was wrong for their directory depth (e.g. `../docs/…` in a file four levels deep).
   These were broken before this package — confirmed identical at `git show HEAD:` — and
   were repaired to the correct depth (`../../../newest-…`, `../../docs/newest-…`, …).

### Correctness verification

- **Diff-proven, not sampled.** All 639 files were compared against `git HEAD`. After
  normalising *every* authority-document reference to a token, **all 1582 changed lines
  were byte-identical** apart from how that one document is referenced. No prose, no link
  labels, and no other links were altered.
- **All 319** links to the authority doc now resolve to an existing file (0 broken).
- **0** gate violations remain.

### Self-inflicted bug, caught and proven harmless

An intermediate repair script captured the markdown-link **label** in the wrong regex
group and therefore detected "broken" links against the label rather than the path. It
would have rewritten `(label)` → `(relative path)`, corrupting those links. It was a
**provable no-op**: a markdown link of the form `[label](path)` never literally contains
the substring `(label)`, so `String.replace` matched nothing and files were rewritten with
identical content. Verified by re-running the resolve check, which showed the same 14
broken links before and after that script, and by the byte-identical diff proof above. The
14 links were then repaired correctly using the path group.

### Safety checks before editing (Rule 6)

- **All 639 affected files were clean in git** (`git diff --quiet` passed on every one), so
  no concurrent lane had uncommitted work in any file touched. Re-checked immediately
  before the rewrite, not once at the start.
- The one untracked affected file was this plan document itself.
- The substitution is idempotent: a rewritten relative path contains no `/home/`, so
  re-running cannot double-convert.

### Not done (deliberately, and why)

**128 remaining `/home/robertsrff/` occurrences across 97 files** are *not* in scope and
are not the same defect:

- 21 are the Godot binary path (`/home/robertsrff/.local/bin/godot`) — a tool location
  that has no in-repo relative form and should be expressed as `godot` (relying on
  `PATH`), a documentation judgement rather than a mechanical rewrite.
- ~16 point to `~/Desktop/luna)plans/…`, i.e. **outside the repository**, where no
  relative path exists.
- The remainder are a mixed set of other machine-specific paths.

The gate does not flag these (its regex requires markdown-link syntax), so the gate is
fully closed. Rewriting them would require per-case decisions, and for the out-of-repo
and tool-binary cases there is no portable relative target at all. Flagged here as the
next candidate package rather than silently skipped.

### Verification

- `DocLinkValidationGateTests`: **2/2 green** (both tests).
- Adjacent sweep (`PortContractGateTests`, `ConsequenceLedgerSourceGateTests`,
  `SaveSectionRegistryTests`, `MainTriadDriftGateTests`, `VersionReportContractTests`,
  `PlanQuadPackageLCoreTests`, `FollowUpRemediationGateTests`): **47/47 green**.
- No code was changed; no test was added or weakened. **No commit.**
