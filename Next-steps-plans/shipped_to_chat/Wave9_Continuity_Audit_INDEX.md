# Continuity Wave 9 — Audit Index (Plans 55–59): *Weight, Durability & the Shop Window*

**Snapshot audited:** `ccac926e` (branch `main`, **0 git tags**) · **Date:** 2026-08-31
**Gates I ran this wave:** `dotnet build Ashfall.csproj` 0/0 · `dotnet test` **5303 passed** ·
`--data-integrity-selftest` **PASS 138 catalogs / 5563 ids** · `--asset-registry-selftest`
**PASS at `checked=50`** (W8) · `triad-drift-gate`, `doc-link-gate`, `warning-baseline-gate` PASS ·
Wave 3's three doc gates still red.

Prior waves: [W1](Wave1_Continuity_Audit_INDEX.md)·[W2](Wave2_Continuity_Audit_INDEX.md)·
[W3](Wave3_Continuity_Audit_INDEX.md)·[W4](Wave4_Continuity_Audit_INDEX.md)·
[W5](Wave5_Continuity_Audit_INDEX.md)·[W6](Wave6_Continuity_Audit_INDEX.md)·
[W7](Wave7_Continuity_Audit_INDEX.md)·[W8](Wave8_Continuity_Audit_INDEX.md).

Waves 1–8 asked whether the game connects, operates, ships, means anything, and can be perceived.
Wave 9 asks the last three questions a project has: **does it survive time, does the repo survive
its own tooling, and can we tell the truth about it in public** — and then, **when do we stop.**

---

## Wave 9 findings: the 10 highest-impact gaps

| # | Gap | Category | Severity | Why it matters | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **No retention policy anywhere in the codebase** — `grep -rniE "Retention\|TrimOldest\|maxEntries\|RollingWindow\|Prune"` returns ballistics `RicochetRetention`, timber density, and the briefing's *display* cap; meanwhile `servingLog`, `enactedDecrees`, journal/memorial/census/pair-history lists all grow forever | technical architecture / production | **critical** | A 400-year campaign — which Waves 4 and 6 just made possible — bloats saves, slows the day loop, and eventually reads as a wall of prose | an inventory table, then `RollingLog<T>` + per-collection policy | 39B, 48A | **first** |
| 2 | **The real save corpus is on one developer's disk** — `~/.local/share/godot/app_userdata/ASHFALL…/` holds **7 MB** of dated `holdfast_archive_20260815…` campaign saves; **none in the repo** | testing / production | **critical** | These are the only artifacts that prove the V1→V2→envelope migrations work on genuinely old data — and one `rm -rf` deletes the project's memory | export, sanitise, commit under LFS as fixtures + a load-all gate | 48A, 55B | **salvage today** |
| 3 | **Long-session robustness is prose, not gates** — leak triage lives in `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`; no cycle/resume test, no memory ceiling, no growth slope assertion | testing | **important** | 200-hour players are the audience; every check today runs 7–30 days | a 50× panel-cycle + 400-day soak assertion in the nightly tier | 16B/16C, 39B | during |
| 4 | **1.34 GB of working-copy weight, and the ignore policy is split** — `.godot` 962M, `.claude` 143M, `.crush` 96M, `.mimocode` 58M, `Ashfall.Core.Tests` 28M, `snapshot-capture` 2M; `.claude/worktrees/` lives in `.git/info/exclude` (local, unshared) | production | **important** | Slow clones, slow imports, agents grepping duplicate "source" trees (W1–3 each hit this); and one machine's rules aren't the project's | move project concerns to `.gitignore`, add a `doctor.sh`, document retention per generated tree | 29A | cheap, now |
| 5 | **122 design mockups are shipped as game assets, referenced by nothing** — `assets/ui/Screens` (62 PNG, 6.5 MB) + `assets/ui/HtmlBundles` (60 HTML, 1.4 MB), 0 code references; several named for Wave 1's unbacked consoles; plus **1,189 unreferenced art files** | production / content | **important** | PCK weight, misleading "art coverage", and pictures of systems that don't exist — the fake-console failure mode preserved in image form | move to `docs/design/mockups/`, link to 16A's verdict list, assert PCK file counts | 50A, 26B | immediately |
| 6 | **A Unity-era playmode results XML sits at the repo root** (`clr-version 4.0.30319`, suite `"Atomic War"`, 2026-08-12) beside 6 `fix_*.py`/`test_parse.py` scripts and a stale `README.md` claiming the Unity tree is "still being removed" (it's four `.gdignore` files now) | technical architecture / production | **later** (but 10 minutes) | Rule #1 of this project is "Godot is authoritative"; root clutter is how a forbidden-engine artifact reads as current, and how an agent runs a stale patch script | archive the artifacts, relocate the scripts, fix the README, add a no-root-strays gate | 29A/56A | with 4 |
| 7 | **There is no store-facing anything** — no `screenshots/`, press kit, credits, or support docs; the only rendered images are 30 QA goldens; `docs/AI_DISCLOSURE.md` is a **template**: *"Fill in the bracketed placeholders before submission"* | production | **critical** (launch-blocking) | A launch page built from here can only make claims the build can't support — the fake consoles again, at public scale | generate kit assets from the frozen slice; generate the provenance statement from 50A's manifest | 54A/50A/56B | after the slice |
| 8 | **Accessibility & localization statements would be fiction today** — the gates that would back them (37A/B/C input, focus, pad, scale, reduce-motion, captions; 25's locale layer) are all still plans | UX / production | **important** | Publishing support claims before the gates exist is a promise the store will hold against you | generate `docs/accessibility/STATEMENT.md` from gate results only; a claim with no gate is omitted | 37, 25 | after 37 |
| 9 | **The continuation has rails but no ownership** — `WaystationSystem` is live (`ExpansionHostSession.cs:23,49,219` reports `bunks bunksOccupied/MaxBunks`), camps/routes/commitments exist, but there is no site/slot state model, and `SaveSlotRoot` routes per *slot*, not per site | progression / technical architecture | **later, deliberately** | A second holdfast is the single easiest way to create a second source of truth — the exact bug nine waves kept finding | an ADR whose deliverable is the *ownership table*, then intake approval (53C) | 32A, 38C, 41C, 43C, 34C, 55A | **blocked — queue it** |
| 10 | **The process is the last unclosed loop** — 22 finding classes across nine waves, each avoidable by a gate that didn't exist **or wasn't run** (three doc gates have been red for weeks); the audit itself added to the sprawl (now **122** plans in `Next-steps-plans/`, 81 of them from a parallel series, **133** in `piagentsplans/`) and made its own error (Wave 1's 17A premise, disproved in Wave 4) | technical architecture / production | **critical** | More waves of findings mean the same failures keep happening; a ninth wave that only produces a tenth wave is a process failure | one gate register with owners + self-proofs, deduplicated scanner, `AUDIT_TRIGGERS.md`, and a declared end | all prior waves | **the closing task** |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [55](Plan_55_Long_Haul_Retention_Save_Corpus_Durability.md) | The Long Haul | 1, 2, 3 | Nothing grows without a stated limit, every save format is a committed fixture, and a 200-hour campaign is a tested scenario. |
| [56](Plan_56_Weight_Hygiene_Repository_Assets_Tools.md) | Weight & Hygiene | 4, 5, 6 | Only files whose category is knowable from their location remain, and a clean clone reproduces a green gate set. |
| [57](Plan_57_Shop_Window_Store_Kit_Statements_Launch_Ops.md) | The Shop Window | 7, 8 | Every image and sentence on the store page traces to a build, a metric, or a generated report. |
| [58](Plan_58_Outposts_Waystations_Second_Holdfast.md) | The Continuation | 9 | An ADR that names an owner for every field — and may legitimately conclude the feature should shrink or not happen. |
| [59](Plan_59_Retrospective_Gates_Rules_Then_Stop.md) | Retrospective | 10 | 22 finding classes → gates with owners and self-proofs; the plan folder stops growing; the audit series is closed. |

---

## Nine waves: the whole shape

| Wave | Question | Plans | Root finding |
|---|---|---|---|
| 1 Story machine | Does choosing matter? | 15–19 | ending hardcoded; choices unmakeable; 30 fake consoles |
| 2 Bunker machine | Does doing matter? | 20–24 | dose a literal; gear immortal; eating a no-op; power decorative |
| 3 Ship it intact | Can we build/test/describe it? | 25–29 | 3 red gates; instructions citing a dead class; unbooted artifacts |
| 4 World beyond the gate | Is anything else going on? | 30–34 | war never ticked; 20/27 event kinds dropped; 6-node map |
| 5 Human interface | Can a person run it 200 hours? | 35–39 | 74/147 seams unplugged; hunting yields vanish; no keyboard nav |
| 6 The people in it | Is anybody in it? | 40–44 | personality inferred not authored; eulogy engine unreferenced; affinity read by nobody |
| 7 Rails & measurement | Will it stay fixed? | 45–49 | 452 defs unreachable; balance unattributed; 0 tags |
| 8 The presented game | Can anyone perceive it? | 50–54 | asset gate checks 0.9 %; 0 shaders/motion; no human ever tested it |
| 9 Weight, durability, truth | Does it last, and can we say so? | 55–59 | no retention; real saves uncommitted; 1.34 GB of tool weight; a store page that would have to lie |

**Forty-five plans, 135 tasks, and one finding, restated nine ways:** *the systems exist and the
seams don't* — and the checking apparatus measured presence instead of liveness, at every altitude
from save codecs to store screenshots.

**The program's highest-value 13 tasks:** 19A · 22A · 24A · 29A · 31A · 34B.1 · 36A · 40A · 44A ·
45A · 48A · 50A · 54A. None is a feature. Three (29A, 53B/59A, 54A) exist to make future work
unnecessary or honest.

## Metrics to report at wave close

1. Unbounded persisted collections: **all → 0**, each with a policy and a ceiling; 400-year
   `campaign.json` size known and gated
2. Save fixtures in-repo: **0 → ≥8** formats/versions, load-all + cross-version + fuzz gating
3. Working-copy weight: **~1.34 GB → a documented, budgeted number**; design mockups and one-off
   scripts relocated; 0 forbidden-engine artifacts visible at root
4. Asset gate coverage: **50/5,563 → 100 % of ids in shipped catalogs**, fallbacks failing the strict tier
5. Store kit: **absent + placeholder AI disclosure → generated, dated, claim-checked**; every
   screenshot a captured build
6. Support statements: **none writable → generated from gates**, claims-without-gates dropped
7. Continuation: **queued with `blocked-by`** — an ADR naming every owner, or a decision not to build
8. Gate register: **22 finding classes → each with a gate, an owner, and a self-proof**; duplicated
   detectors merged into one scanner; retired gates counted
9. Process health: plan-folder size **flat or shrinking**; audit series declared closed with written
   re-audit triggers; findings-per-release trend reported

## Off-wave: flagship integration

A user-supplied flagship medical plan (7 → 15 diseases, dependency care, palliative depth) was
integrated as **[Plan 60 — Medicine Made Legible](Plan_60_Medicine_Made_Legible_Plan09_Integrated.md)**.
Its premise was re-baselined against source, which is exactly the operation
`docs/process/AUDIT_TRIGGERS` and the claims-gate idea exist for: the catalog already holds **15**
diseases with all four vectors covered, and two world-triggered outbreak sources are already live
(`src/Host/DiseaseOutbreakHostAdapter.cs:32,46`). What is actually missing is legibility and
binding — authored `guidance`/`source_note` fields with **no runtime consumer**, an
`IGriefSink.ApplyGrief` sink with **no host caller**, a `ReportStress` hook with **no producers**, and
a `VigilStateMachine` with **zero UI/Main callers**. Volume was never the gap; connection was.

## Deferred (explicitly — this is the last wave by design)

Anything still unstarted after Wave 9 enters through **53C's intake gate** with the 45A ladder and the
54C scorecard as its acceptance evidence — including the parallel expansion series (81 plans), the
store-certification tail (57), and the continuation (58). Wave 10 is not scheduled; `docs/process/AUDIT_TRIGGERS.md`
decides whether it exists.
