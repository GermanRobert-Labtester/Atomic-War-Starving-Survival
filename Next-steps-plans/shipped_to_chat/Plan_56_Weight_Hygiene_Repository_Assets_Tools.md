# Plan 56 — Weight & Hygiene: A Repository That Doesn't Fight Its Own Tools

> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window*
> **Depends on:** 29A (green doc gates), 50A (asset manifest — the difference between art and
> artifacts), 55B (LFS policy for the save corpus), 48B (release artifacts).
>
> **Theme:** the working copy carries roughly **1.34 GB of non-source weight** (`.godot` 962 MB,
> `.claude` 143 MB, `.crush` 96 MB, `.mimocode` 58 MB, `Ashfall.Core.Tests` 28 MB,
> `snapshot-capture` 2 MB, `artifacts` 1.8 MB), **~14 AI-tool config directories**, **13 rulebook
> copies**, **11 stray root files** — including a **Unity playmode test-results XML** in a project
> whose first rule is "Unity is not a target editor" — and **122 design mockups inside
> `assets/ui/`** (62 PNG + 60 HTML, 1.4 MB + 6.5 MB) that **zero lines of code reference**, several
> named after the exact 30 consoles Wave 1 found to be fake.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Working-copy weight | `du -sh`: `.godot` **962M** · `.claude` **143M** · `.crush` **96M** · `.mimocode` **58M** · `Ashfall.Core.Tests` 28M · `snapshot-capture` 2M · `artifacts` 1.8M · `.cursor` 1.3M · `semantic-review` 80K |
| 2 | ~14 agent/tool config dirs, 13 rulebooks | dot-dirs: `.agents .aider .claude .codex .commandcode .composio .crush .cursor .kiro .mimocode .mistral .qlty .qwen .zcode` + rulebooks `CLAUDE CODEX CRUSH GOOSE MIMOCODE OPENSETUP QWEN VIBE ANTIGRAVITY GEMINI .clinerules .cursorrules .windsurfrules` + `AGENTS.md` — Wave 8's 50B/29A already found one (GEMINI.md) holding another client's rules |
| 3 | Root stray files | `art-wiring-results.xml`, **`batch20-playmode-results.xml`** (Unity playmode results: `clr-version 4.0.30319.42000`, suite `"Atomic War"`, dated 2026-08-12), `export_code.py`, `fix_queuefree.py`, `fix_syntax.py`, `fix_using.py`, `safe_fix.py`, `generate_master_doc.py`, `test_parse.py`, `UI_StyleReference_01.jpg`, `icon.svg` |
| 4 | **122 unreferenced design mockups shipped as game art** | `assets/ui/Screens/` 62 PNG (6.5 MB) + `assets/ui/HtmlBundles/` 60 HTML (1.4 MB); `grep -rn "HtmlBundles\|ui/Screens" src/ Assets/Ashfall.Core` → **0 references**; names map to console mockups: `01_…subterranean_mining_geological_excavation_terminal`, `02_…long-range_radio_intercept_morse_decryption_array`, `03_…atmospheric_air_filtration_carbon_scrubber_termina`, `04_…expedition_return_decontamination_terminal` |
| 5 | The mockups are the *design half* of Wave 1's fake consoles | 30 routed consoles with no authority (BUG-UI-002); several of these screen names correspond to those consoles — art direction was produced for systems that don't exist |
| 6 | Ignore policy split between two mechanisms | `.claude/worktrees/` excluded via `.git/info/exclude` (local, unshared) not `.gitignore` — Wave 1's 19C step 4; plus `git check-ignore` behaviour differs per clone |
| 7 | README states something that is no longer true | `README.md`: "A legacy-engine (Unity) tree is still present as a migration artifact and is being removed" — `Assets/art|sprites|ui|audio` now contain one `.gdignore` each (Wave 8's 50B step 11) |
| 8 | Generated/QA trees mix with source | `snapshots/` (30 goldens), `snapshot-capture/` (working captures), `artifacts/` (reports, `.gdignore`d), `semantic-review/`, `TestResults/results.trx` inside the test project — no documented retention or regeneration path |
| 9 | Test-project weight | `Ashfall.Core.Tests` 28 MB includes `obj/bin`, `.trx` runs, and (per Wave 5) unreferenced fixtures; `scripts/maintenance/` holds one-off migration scripts (`add_p11_methods_v2.py`, `cleanup_p11_hostsessions.py`, `fix_event_leaks.py`) with no lifecycle marker |
| 10 | The project already has the right tools | `ashfall-repo-hygiene` (dry-run quarantine + LFS verification), `ashfall-lfs-gate`, `lfs-health-check.sh`, `git-object-inventory.sh`, `repo-hygiene-report.sh`, `no-whitespace-churn.sh`, `scripts/ci/godot-asset-gate.sh` — this plan runs them and closes what they report, rather than inventing new tooling |

---

## Task 56A — Classify everything at the root and in `assets/`, and act

**Goal:** no file is where it isn't supposed to be; design artifacts, generated output, and shipped
assets are three different places with three different rules.

**Files:** root `*.py`/`*.xml`/`*.jpg`/`*.svg` files, `assets/ui/Screens/`, `assets/ui/HtmlBundles/`,
`snapshot-capture/`, `artifacts/`, `semantic-review/`, `docs/archive/`,
`scripts/maintenance/`, `.gitignore`, `.gitattributes`, `README.md`,
`docs/tools/TOOLING_CLASSIFICATION_AND_LIFECYCLE.md`, `ashfall-repo-hygiene` + `ashfall-lfs-gate`.

### Substeps

1. **Run the existing report first** (`repo-hygiene-report.sh`, `git-object-inventory.sh`,
   `lfs-health-check.sh`, `asset-orphan-sweep.sh`) and paste the numbers — the plan starts from
   measurement, not from this table.
2. **Quarantine the Unity-era artifacts**: `batch20-playmode-results.xml` and `art-wiring-results.xml`
   are engine-era test output in a project whose rule #1 forbids that editor — archive under
   `docs/archive/unity-era/` (never "delete the evidence", never leave it looking current).
3. **Move the one-off scripts** (`fix_*.py`, `safe_fix.py`, `test_parse.py`, `export_code.py`,
   `generate_master_doc.py`) into `scripts/maintenance/` with a lifecycle marker, or delete them if
   the migration they served is done; root scripts are how an agent runs a six-week-old patch by
   accident.
4. **Relocate design artifacts**: the 60 HTML + 62 PNG mockups are *design*, not *game assets* — move
   to `docs/design/mockups/` (or `docs/archive/`) and keep them referenced by the panel work they
   inform, so they stop riding in every exported PCK.
5. **Decide the mockups' fate against 16A's verdict list**: where a mockup belongs to a *shelved*
   console, link it from that console's entry (it is the design spec for when the rail exists); where
   it belongs to a *live* panel, it becomes an approval reference; nothing gets silently deleted.
6. **Shrink the exported artifact**: after step 4, assert `include_filter`/`export_filter` don't pack
   design files, and record the PCK size delta (Wave 3's 26B).
7. **Fix the README** stale Unity claim and its "Legacy migration surface" section (with the tooling
   doc as the citation), then regenerate the rulebooks (29A) if the text is mirrored there.
8. **Unify ignore policy**: anything currently in `.git/info/exclude` that is a *project* concern
   moves to `.gitignore` (`.claude/worktrees/`, generated trees); anything machine-local gets a
   documented reason.
9. **Define retention** for generated output: `artifacts/`, `snapshot-capture/`, `TestResults/*.trx`,
   `semantic-review/` — which are ephemeral (gitignored), which are golden (tracked), which are
   released evidence (archived per 55B/48B).
10. **Prune the test project's committed weight**: `.trx` runs and build output out; fixtures stay
    (and move to LFS if they're binary — 55B step 4).
11. **Bound the tool-config sprawl**: one documented statement of which agent clients are supported,
    which dirs are machine-local, and how the rulebook set is generated (29A/50B's sync contract) —
    ~14 config dirs is a workflow problem wearing a filesystem costume.
12. **Add a hygiene gate**: a Tier-2 check that fails on (a) new root-level `*.xml/*.py` results,
    (b) design files under `assets/` with no manifest row (50A), (c) tracked ephemeral artifacts.
13. **Tests/receipt**: before/after `du` of the working copy, clone, and PCK — the numbers go in the
    wave ledger (29C).

**DoD:** the repo contains only files whose category is knowable from their location, and the clone
got measurably lighter.

---

## Task 56B — Import, LFS, and build-weight policy for the asset tree

**Goal:** the 114 MB of real art and 10 MB of audio are tracked, imported, and packed deliberately —
with a size budget that fails on drift.

**Files:** `.gitattributes`, `assets/**/*.import`, `export_presets.cfg`, `scripts/ci/lfs-health-check.sh`,
`scripts/ci/asset-orphan-sweep.sh`, 50A's `asset_registry.json`, `docs/visual/ASSET_GALLERY.md`
(generated), `scripts/pipeline/import_approved_assets.py`, `docs/hygiene/`, new
`scripts/ci/asset-budget-gate.sh`.

### Substeps

1. **Publish the asset budget** per family (`art/`, `sprites/`, `ui/`, `audio/`, `fonts/`): tracked
   size, packed size, import cache size — then fail on growth beyond a stated tolerance without a
   review entry (the ratchet pattern used throughout Waves 1–7).
2. **Verify LFS policy matches intent** (images/fonts via LFS, `*.wav/mp3/ogg` plain binary, per
   `AGENTS.md`) and that nothing newly added violates it — `lfs-health-check.sh` already exists; give
   it teeth with a size-delta report.
3. **Import settings as a contract**: filter/mipmap/compression per family (the Unity→Godot port
   explicitly required porting import settings); assert via `ashfall-shader-material-lint` that new
   PNGs conform, so "it looks wrong on the Steam Deck" isn't discovered by a review unit.
4. **Compression/size review for the packed PCK**: mipmaps off where UI-only, ETC2/ASTC config per
   `import_etc2_astc=false` in `project.godot` (a mobile-compat toggle in a desktop project — confirm
   it's intentional and record why).
5. **Dedup by hash before anything else ships**: identical bytes across `assets/art` and
   `assets/ui/Icons` (50B step 10) reduce both clone and PCK.
6. **Split store-facing from game-facing art** (used by 57): capsules, screenshots, and press kits do
   not belong in the shipped PCK.
7. **Regenerate the gallery** so per-asset provenance, size, and usage are visible (50A step 8).
8. **Kill the duplicate trees** created by earlier generation runs (`snapshot-capture/` vs
   `snapshots/`, `AI_Generated/` under multiple parents) with a documented single home per kind.
9. **Budget the import step**: CI's `godot --headless --import` time is part of the dev experience
   (Wave 3's 26C); measure and cap, with a cached-import strategy if the tree keeps growing.
10. **Audio-specific policy**: `assets/audio` 73 files / 10 MB, formats mixed (`.wav`/`.mp3`/`.ogg`) —
    declare the canonical runtime format and the conversion step so the catalog doesn't grow a fourth.
11. **Tests**: budget gate self-proof (an oversized fixture fails), LFS/attributes conformance,
    import-preset conformance, duplicate-hash detection.
12. **Docs**: `docs/visual/ASSET_BUDGETS.md` + pointer from `docs/hygiene/`.
13. **Run the checklist** + asset gate + export smoke.

**DoD:** asset weight is a budgeted, gated property, and every byte in `assets/` has a family, a
preset, and a reason.

---

## Task 56C — Instrument the working environment: agents, tools, and CI parity

**Goal:** stop the environment itself from producing drift — the split ignore rules, red doc gates,
and multi-tool rulebooks are how seven waves kept rediscovering the same facts.

**Files:** `.github/workflows/ci.yml`, `scripts/ci/verify-fast.sh`,
`scripts/ci/run-gates.py`, `docs/ci/CI_GATE_MANIFEST.json`, `docs/CI.md`,
`scripts/ci/sync-agent-rulebooks.py`, `docs/agents/AGENT_SKILLS_INDEX.md`,
`docs/tools/TOOLING_CLASSIFICATION_AND_LIFECYCLE.md`, `.gitignore`, `setup-repo.sh`,
`docs/CURRENT_AUTHORITY.md`.

### Substeps

1. **Prove CI runs the gate list it claims** — 46 gates in the manifest vs what `ci.yml` executes
   (Wave 3's 29A step 9): if the workflow doesn't run `run-gates.py --tier fast`, the manifest is a
   wish list and every green check is a partial claim.
2. **Fix the three red doc gates** (29A) and keep them green as a precondition of everything else in
   this plan.
3. **Make `setup-repo.sh` the single bootstrap**: `core.ignorecase=false`, LFS install, and now also
   the ignore-policy consolidation from 56A step 8 — one command, one expected state.
4. **Add a doctor command** (`scripts/ci/doctor.sh`): reports missing tooling (godot/dotnet versions,
   LFS), stale `.godot/` cache, unignored local dirs, red gates, and dirty-tree size — the thing a
   new contributor (or a new agent) runs first instead of guessing.
5. **Declare supported clients** explicitly (56A step 11): the rulebook set generated from
   `AGENTS.md`, plus a stated list of which tool directories are machine-local and must never be
   committed.
6. **Standardise where generated QA output lives** — `artifacts/`, `snapshots/`,
   `snapshot-capture/`, `semantic-review/`, `TestResults/` — with a documented owner and regeneration
   command per tree (and add `--check` to each generator that lacks one).
7. **Give the maintenance scripts a lifecycle** (`scripts/maintenance/*`): run once → move to
   `scripts/archive/` with a note; `ashfall-repo-hygiene`'s dry-run-first rule applies to them.
8. **Reduce the audit/plan document weight** now that the register exists (53A): 255+ plan documents
   is itself a hygiene problem; executed plans move to archive with a completion commit (29C step 7).
9. **Parity rule for agents**: every automated author must run the same fast tier a human does
   (`verify-fast.sh`) and paste the result; the reason Wave 3's gates were red is that they were run
   by nobody for a while.
10. **Time-box CI**: fast tier under a stated budget (it mirrors 14+ gates today; add the new slice,
    corpus, and budget gates to nightly tiers per 55B/54A), so a slow CI is not a reason to skip it.
11. **Tests**: a smoke job that runs the doctor, and a check that the manifest, workflow, and
    `verify-fast.sh` agree on the gate list (three sources, one truth — the recurring lesson).
12. **Docs**: one page — `docs/CI.md`'s status table — showing gate list, tier, last-green date, and
    owner, regenerated not typed (29A).
13. **Run the checklist** + `verify-fast.sh` twice (idempotence) + the doctor on a clean clone.

**DoD:** a clean clone plus `setup-repo.sh` plus the doctor reproduces a fully green gate set — on
any machine, for any tool.

---

## Cross-Task Dependencies

```
29A (green docs gates) ──► 56C step 2 ──► 56C step 11
50A (manifest) ──► 56A step 4, 56B step 7      55B (LFS policy) ◄──► 56B step 2
16A (console verdicts) ──► 56A step 5           48B (release artifacts) ──► 56A step 9
        56A (classify & move) ──► 56B (budget & policy) ──► 56C (environment & CI parity)
        Plans 57 (store art) and 58 (continuation) both need 56A's separation of game vs design assets
```

**Execution order:** 29A → 56A → 56B → 56C. Wave 9 order: **55B step 1 salvage** → 56A → 57A →
56B → 58A → 55A → 56C → 57B → 55B → 57C → 55C → 58B → 59 → 58C.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/repo-hygiene-report.sh && bash scripts/ci/git-object-inventory.sh
7. bash scripts/ci/lfs-health-check.sh && bash scripts/ci/asset-orphan-sweep.sh
8. bash scripts/ci/asset-budget-gate.sh                          # (56B)
9. bash scripts/ci/doctor.sh                                     # (56C) on a fresh clone
10. bash scripts/ci/godot-asset-gate.sh && bash scripts/ci/export-smoke-boot.sh   # PCK delta
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Files moved | Gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 56A | 0 | ~145 (root + mockups) | 1 | 2–4 | Low–Med | **MEDIUM — never move a file an import or export filter references (step 6 proves it)** |
| 56B | 0 | 0 (policy) | 1 | 4–6 | Medium | LOW–MED (import-setting changes can alter rendering → 50C snapshots catch it) |
| 56C | 0 | 0 | workflow parity | 3–5 | Low–Med | LOW |

**Guardrails:** dry-run before any move (the hygiene skill's own rule); archive rather than delete for
anything with history value; no deletion of design mockups that are the only spec for a shelved
console; never weaken LFS policy to make a check pass; no force-added ignores to hide a problem the
doctor should report; and no claim of hygiene without a before/after byte count.
