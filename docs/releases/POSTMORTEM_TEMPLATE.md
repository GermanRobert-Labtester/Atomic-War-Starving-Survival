# ASHFALL Post-Mortem Template

> **Usage:** Copy for each post-incident analysis. Fill in all sections.
> **Authority:** Plan 48 / C2[21] Release Craft
> **Principle:** "Which gate would have caught this?"

---

## Incident: [Short Description]

**Date:** [YYYY-MM-DD]
**Severity:** S[1-5] — [Critical / High / Medium / Low]
**Version affected:** v[X.Y.Z]
**Hotfix version (if issued):** v[X.Y.Z+1] — [link to tag/PR]
**Reporter(s):** [GitHub issue / Discord / internal]

---

## Timeline

| Time | Event |
|---|---|
| [HH:MM UTC] | Issue first reported |
| [HH:MM UTC] | Confirmed reproducible |
| [HH:MM UTC] | Root cause identified |
| [HH:MM UTC] | Hotfix branch created |
| [HH:MM UTC] | Hotfix tagged and pushed |
| [HH:MM UTC] | Post-mortem written |

---

## What Happened

[2-4 paragraph description of the incident. What broke? How was it discovered?
What was the user impact? Were saves affected?]

---

## Root Cause

[Technical root cause. Be specific: which file, which function, which condition
triggered the bug. Do NOT just describe symptoms.]

**Root cause category:**

- [ ] Logic error
- [ ] Missing guard / boundary check
- [ ] Save migration error
- [ ] Data authority mismatch (JSON ↔ C# mismatch)
- [ ] Regression introduced by another change
- [ ] External dependency (engine, library)
- [ ] Configuration/deployment error
- [ ] Other: ___

---

## Impact

**Players affected:** [Estimated count or "unknown"]
**Save files affected:** [Yes / No / Unknown]
**Data loss:** [Yes / No]
**Workaround available:** [Yes — describe / No]

---

## Fix

[Describe the fix. Link the commit(s) and PR(s).]

**Fix commit:** `[SHA]`
**PR:** [#N]
**Iron rule check (if hotfix):** [PASS / N/A]

---

## Which Gate Would Have Caught This?

This is the core question. Be honest.

| Gate | Would it have caught this? | Notes |
|---|---|---|
| `verify-fast.sh` (all 53 fast gates) | [Yes / No / Partially] | |
| `save_support_window` gate | [Yes / No / Partially] | |
| `version_gate` | [Yes / No / N/A] | |
| `release-gate.sh` | [Yes / No / Partially] | |
| `compiler_warning_baseline` | [Yes / No / N/A] | |
| `data_integrity_selftest` | [Yes / No / N/A] | |
| Specific test that should exist | [Test name] | MISSING |

---

## Action Items

| # | Action | Owner | Due |
|---|---|---|---|
| 1 | [Add gate / test / guard that would catch this] | | |
| 2 | [Update FIXTURE_POLICY or historical corpus if saves affected] | | |
| 3 | [Document in SUPPORT.md if triage flow needs updating] | | |

---

## Prevention

[1-3 sentences on what structural change (gate, test, policy, architecture)
would prevent this class of bug from reaching a release tag in the future.]

---

## Sign-off

**Written by:** [Name]
**Date:** [YYYY-MM-DD]
**Linked issue:** [GitHub issue URL]
