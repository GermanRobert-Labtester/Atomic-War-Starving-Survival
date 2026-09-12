# ASHFALL POTENTIAL CLUTTER, UNWIRED CODE & ANOMALIES
# Read-only sweep output — 2026-09-12
#
# Format per AI_AGENT_WORKFLOW.md sweep protocol:
#   finding_id | severity | confidence | path:line | current evidence | expected contract | proposed owner
#
# Severity: HIGH (likely damage / wasted disk) / MED (review) / LOW (informational)
# Confidence: HIGH (zero references proven) / MED (unclear references) / LOW (needs human judgment)
# Status at sweep time: IDLE batch per INTEGRATION_PLANS.md; no live claims per WORKTREE_OWNERSHIP.md
# This file is a tag-only audit. It does NOT authorize deletion, modification, or promotion.

================================================================================
SECTION A — POTENTIAL CLUTTER (unreferenced / one-off / oversized artifacts)
================================================================================

A1.  BUG_HUNT forensic reports (unreferenced)
    severity: MED     confidence: HIGH
    paths:
      docs/forensics/BUG_HUNT_25_OPTIMIZED_FIX_BATCH_REPORT.md
      docs/forensics/BUG_HUNT_26_SECOND_PASS_FIX_BATCH_REPORT.md
      docs/forensics/BUG_HUNT_27_THIRD_PASS_FIX_BATCH_REPORT.md
      docs/forensics/BUG_HUNT_28_FOURTH_PASS_FIX_BATCH_REPORT.md
      docs/forensics/BUG_HUNT_29_FIFTH_PASS_DEFERRED_SEAL_REPORT.md
    evidence: grep -rn 'BUG_HUNT\|bug_hunt\|forensics/BUG' over AGENTS.md,
              AI_AGENT_WORKFLOW.md, INTEGRATION_PLANS.md, WORKTREE_OWNERSHIP.md,
              KNOWN_DEBT.md, CLAUDE.md, TEST_POLICY.md, docs/CURRENT_AUTHORITY.md,
              scripts/, README.md returned zero matches. No code references either.
    expected contract: forensic reports referenced from KNOWN_DEBT.md or a current
                       governance ledger, or absent.
    proposed owner: Foreman (decide: archive to docs/archive/forensics/<date>/ or
                    delete; record decision in KNOWN_DEBT.md).

A2.  PLANS forensic reports (unreferenced)
    severity: MED     confidence: HIGH
    paths:
      docs/forensics/PLANS_142_145_WAVE0_FORENSIC_REPORT.md
      docs/forensics/PLAN_PORTFOLIO_INTEGRATION_STATUS_FORENSIC_REPORT.md
    evidence: same grep as A1 — no references in governance, code, or scripts.
    expected contract: forensic reports cited by name in INTEGRATION_PLANS.md
                       recent batches or KNOWN_DEBT.md; otherwise archive.
    proposed owner: Foreman.

A3.  Plan authority maps (orphan — not indexed by docs/atlas)
    severity: LOW     confidence: MED
    paths:
      docs/water/PLAN_168_WATER_DELIVERY_AUTHORITY_MAP.md
      docs/factions/PLAN_167_CONSEQUENCE_ROUTING_MAP.md
      docs/shelter/PLANS_126_129_OWNERSHIP_DECISIONS.md
      docs/shelter/PLAN_118_AUTHORITY_MAP.md
      docs/shelter/PLAN_118_FISCHER_TROPSCH_CLOSEOUT.md
      docs/shelter/PLAN_118_SYNTHETIC_LUBE_BALANCE.md
      docs/shelter/PLAN_120_CARBON_COMPOSITES_CLOSEOUT.md
      docs/shelter/PLAN_120_COMPONENT_CONSUMER_MATRIX.md
      docs/shelter/PLAN_120_COMPOSITES_AUTHORITY_MAP.md
      docs/radio/PLAN_119_SENSOR_CHARACTERIZATION.md
      docs/radio/PLAN_119_UV_CORONA_AUTHORITY_MAP.md
      docs/radio/PLAN_119_UV_CORONA_DETECTION_CLOSEOUT.md
      docs/world/PLAN_121_GPR_AUTHORITY_MAP.md
      docs/world/PLAN_121_GPR_CARTOGRAPHY_CLOSEOUT.md
      docs/world/PLAN_121_GPR_CHARACTERIZATION.md
    evidence: same grep as A1 — these authority maps produced by recent closed
              batches (per INTEGRATION_PLANS.md recent closed-batches table) are
              not cross-linked from the governance ledgers themselves. They
              exist as orphans under docs/<domain>/.
    expected contract: docs/CURRENT_AUTHORITY.md or the docs/atlas index should
                       enumerate them, or they belong in docs/archive/.
    proposed owner: Foreman (governance decision; not a code change).

A4.  TestResults/*.trx — large committed test artifacts
    severity: MED     confidence: HIGH
    paths:
      Ashfall.Core.Tests/TestResults/p87.trx                           (15.5 MB)
      Ashfall.Core.Tests/TestResults/results.trx                       ( 6.0 MB)
      Ashfall.Core.Tests/TestResults/test_result.trx
      Ashfall.Core.Tests/TestResults/6b426762-26a7-43e5-a9b1-94eb85b4b8be/Sequence_8c778cccbbac439eb681576dae86f35e.xml ( 1.5 MB)
    evidence: these are Visual Studio / dotnet test TRX and vstest sequence
              outputs. .gitignore covers test-results-*.xml but NOT *.trx.
              They regenerate on every test run and add ~22 MB to the repo.
    expected contract: test outputs either excluded by .gitignore or quarantined
                       under Twin_ASHFall/quarantine/ with hashes (parallel to
                       DEBT-TEST-QUARANTINE-2026-09-12).
    proposed owner: Foreman (decide exclusion pattern; integrator executes).

A5.  .cache/audio_* tracked despite .gitignore rule
    severity: HIGH    confidence: HIGH
    paths (314 tracked files, ~28 MB total):
      .cache/audio_library_pre_remaster/{sfx,radio,ui,ambience,music}/*.wav|.mp3|.ogg
      .cache/audio_preservation_pre_flagship/{sfx,radio,ui,ambience}/*.wav|.mp3
    evidence: .gitignore line 'Local audio/tooling working cache — never commit
              /.cache/' explicitly excludes .cache/. But 314 audio files under
              .cache/audio_library_pre_remaster/ and .cache/audio_preservation_pre_flagship/
              are tracked. Examples: geiger.wav (705 678 B) appears identically
              in three locations; vo_verdict_geophone.wav (628 522 B) and
              vo_verdict_count.wav (567 582 B) each appear in three locations.
              Per CLAUDE.md/.gitattributes, audio must remain plain binary — but
              these are duplicates, not load-bearing assets.
    expected contract: either remove the tracked entries (git rm --cached) or
                       move them to a quarantined archive under Twin_ASHFall/.
                       The active audio source of truth is assets/audio/.
    proposed owner: Foreman + Integrator (history-touching; requires explicit
                    approval per repo-hygiene skill).

A6.  Untracked Python reauthor scripts at repo root
    severity: MED     confidence: HIGH
    paths (TRACKED but deleted in working tree):
      test_truncate.py
      test_thirdonary_more.py
      test_thirdonary_later.py
      test_bio_rewrite.py
      inspect_trade.py
      inspect_trade2.py
      rewrite_survivors.py
      rewrite_epitaphs.py
      replace_chunk13.py
      replace_chunk14.py
      replace_chunk15.py
      reauthor_trade_texts.py
      reauthor_exp06.py
      reauthor_exp05.py
      reauthor_exp05_part2.py
      extract_trade_texts.py
      extract_thirdonary.py
      extract_quests.py
      extract_quests_06.py
      extract_npcs.py
      export_code.py
      dump_thirdonary_samples.py
      dump_remaining.py
      dump_remaining_echoes.py
      count_lines_trade_texts.py
      analyze_trade_texts.py
      check_survivors.py
      generate_master_doc.py
    evidence: git status shows all of the above with ' D' (deleted from working
              tree, still tracked in the index). Their work products (data
              rewrites) appear committed. .gitignore has /fix_*.py, /test_parse.py,
              /safe_fix.py but NOT these script names — so they were committed
              once and are now staged-for-deletion but never `git rm`-ed.
    expected contract: either commit the deletions (git rm) so the index
                       matches the working tree, or quarantine to
                       docs/archive/<date>/reauthor-scripts/ with hashes.
    proposed owner: Foreman (governance) — Integrator executes.

A7.  _migrated/UI_StyleReference_01.jpg — orphan tracked deletion
    severity: LOW     confidence: HIGH
    path: _migrated/UI_StyleReference_01.jpg
    evidence: tracked file deleted from working tree. The same image is now
              tracked at assets/ui/reference/ui_style_reference_01.jpg (2.8 MB).
              _migrated/ appears to be a one-off migration scratch dir.
    expected contract: git rm to clean up the index.
    proposed owner: Integrator (mechanical).

A8.  Untracked artifacts/* playtest outputs (regenerable)
    severity: LOW     confidence: MED
    paths (untracked):
      artifacts/advanced-industrial-recon-60d.json
      artifacts/advanced-industrial-recon-60d.md
      artifacts/expedition-playtest-30d.json
      artifacts/expedition-playtest-30d.md
      artifacts/world-playtest-30d.json
      artifacts/world-playtest-30d.md
    evidence: parallel to artifacts/content-utilization.{json,md} which IS
              tracked per .gitignore allow-rule. These newer sibling reports
              look like regen outputs from the recent plan batches but were
              never committed.
    expected contract: either tracked alongside content-utilization (CI
                       reproducibility) or excluded.
    proposed owner: Foreman.

A9.  Untracked addons/ziva_agent — unused Godot extension
    severity: HIGH    confidence: HIGH
    path: addons/ziva_agent/
    evidence:
      - 'addons/' appears as untracked (??) at root.
      - addons/ziva_agent/ziva_agent.gdextension is a native extension
        binary; .gdextension.uid present.
      - project.godot contains zero references to ziva_agent, gdextension, or
        addons/.
      - 9 sibling files (manifest.json, README.md, LICENSE.md, icon.svg, etc.)
        accompany the binary.
    expected contract: if the project does not load ziva_agent, this addon is
                       dead weight (binary + manifest + assets). If it is a
                       future feature, it belongs under version control and
                       should be enabled in project.godot.
    proposed owner: Foreman (decide: drop or wire). Integrator executes.

A10. Quarantined legacy assets — confirmed large debris, low action urgency
    severity: LOW     confidence: HIGH (already in quarantine by design)
    paths:
      assets/quarantine/legacy_assets/Assets/art        (~2 068 deleted files)
      assets/quarantine/deprecated_sprites/Items       ( ~82 deleted files)
      assets/quarantine/legacy_assets/Assets/ui/Icons  ( ~38 deleted files)
      assets/quarantine/legacy_assets/Assets/ui/Textures/Backgrounds ( 8)
    evidence: the deletions in the working tree are FROM these quarantine
              folders; they were staged-removed by an earlier sweep. Files
              are already excluded from gameplay via the assets/quarantine/
              ignore rule. The deletions still need `git rm` to land.
    expected contract: index clean. Not actionable clutter.
    proposed owner: Integrator (mechanical `git rm`).

================================================================================
SECTION B — POTENTIALLY UNWIRED CODE
================================================================================

B1.  BioFermentationPanel never loaded from a scene
    severity: LOW     confidence: HIGH
    path: src/UI/BioFermentationPanel.cs (untracked)
    evidence:
      - File IS referenced from src/Main.World.cs and src/Main.UiPanels.cs.
      - scenes/ contains only CSharpTest.tscn, HoldfastInterior.tscn,
        Main.tscn, WastelandMap.tscn. None reference BioFermentationPanel.
    expected contract: panels are normally opened programmatically (via
                       Main.UiPanels), so a scene reference is not required.
                       Confirm the Main.UiPanels.open path is reachable.
    proposed owner: Integrator (verify reachability) — NOT clutter.

B2.  BioFermentationHostSession / BioFermentationSaveStore — untracked but wired
    severity: LOW     confidence: HIGH
    paths:
      src/Host/BioFermentationHostSession.cs
      src/Host/BioFermentationSaveStore.cs
    evidence: BioFermentation string referenced from src/Main.{World,UiPanels}.cs
              and src/UI/BioFermentationPanel.cs. These files are part of a
              complete system (engine + catalog + tests + host + save + UI).
    expected contract: integrated through the BioFermentation authority; not
                       clutter. Pending commit only.
    proposed owner: Integrator (no fix needed; record as in-flight).

B3.  Main partials and HostCli partials — untracked but wired (intentional)
    severity: LOW     confidence: HIGH
    paths (untracked but class members of partial declarations):
      src/Host/HostCli.AdvancedIndustrialRecon.cs
      src/Host/HostCli.ExpeditionPlaytest.cs
      src/Host/HostCli.WorldPlaytest.cs
      src/Main.Plans126_129.cs
      src/Main.WorldPlaytest.cs
    evidence:
      - src/Host/HostCli.cs:190 declares 'public static partial class HostCli'.
        All *HostCli.*.cs partials are auto-compiled.
      - src/Main.cs:37 declares 'public partial class Main : Control'.
        All *Main.*.cs partials are auto-compiled.
    expected contract: partial-class members always auto-wire. NOT clutter.
    proposed owner: Integrator (commit only).

B4.  .agents/skills/ mass deletion (41 SKILL.md files)
    severity: LOW     confidence: HIGH
    paths: ' D .agents/skills/<dir>/SKILL.md' for 41 dirs (see git status).
    evidence: the repo retains 35 .agents/skills/<dir>/ directories with their
              SKILL.md. 41 sibling directories had SKILL.md removed in the
              working tree (the dirs themselves remain).
    expected contract: if the dirs are still referenced (e.g., via .qwen
                       extension or .claude skill dispatch), they should keep
                       a SKILL.md; if not, the empty dir should be removed.
    proposed owner: Foreman (governance decision).

B5.  .qwen/skills/* — tracked skill mirrors
    severity: MED     confidence: MED
    paths (109 tracked): .qwen/skills/ashfall-* (e.g. ashfall-10-loop-forensic-bug-hunter,
                         ashfall-agents, ashfall-analyze, ashfall-architecture-hardening-auditor,
                         ashfall-art, ashfall-asset, ashfall-audit, ...)
    evidence: .qwen/settings.json + .qwen/extensions/ashfall-agents/ reference
              these skills from the Qwen harness side, but no governance file
              points at them as authoritative. They are sibling definitions
              of the same skills in .agents/skills/ and .claude/skills/.
    expected contract: only ONE skill catalog per skill name should be the
                       authority; .qwen/skills/ duplicates risk drift.
    proposed owner: Foreman (decide: keep .qwen/ as a generated mirror or
                    consolidate). NOT auto-removable.

================================================================================
SECTION C — ANOMALIES
================================================================================

C1.  .gitignore starts with the Unity .gitignore template
    severity: HIGH    confidence: HIGH
    path: .gitignore (lines 1–14 and onward)
    evidence: the file's header reads '# Unity .gitignore Template' and
              '# Source: https://github.com/github/gitignore/blob/main/Unity.gitignore'.
              The file has been *appended* with Godot/Ashfall-specific rules
              (Python __pycache__, generated_AIassets/, .mistral/,
              tools/agent-skill-manager/target/, etc.), but the template body
              is still Unity's.
    expected contract: per CLAUDE.md rule #1 'Godot is authoritative; Unity
                       is retired', the .gitignore should not be a Unity
                       template. Either swap in a Godot template as the base
                       or strip the Unity comments and rebuild from current
                       rules.
    proposed owner: Foreman (governance) — Integrator executes.

C2.  109 untracked deletions under .qwen/ and assets/quarantine/
    severity: LOW     confidence: HIGH
    paths:
      .qwen/tmp/                                              ( 28 files)
      .qwen/tmp/qwen-review-HoldfastRuntimeSession.cs-plan-prompts/ (18)
      assets/quarantine/legacy_assets/Assets/art              (2 068)
      assets/quarantine/deprecated_sprites/Items              ( 82)
      assets/quarantine/legacy_assets/Assets/ui/Icons         ( 38)
      .cache/audio_library_pre_remaster/sfx                   (106)
      .cache/audio_preservation_pre_flagship/sfx              (100)
      .cache/audio_preservation_pre_flagship/radio            ( 34)
      Assets/StreamingAssets/Data                             ( 55)
      Assets/Ashfall.Core                                     ( 34)
      Ashfall.Core.Tests                                      ( 21)
      Ashfall.Core.Tests/WildlifeTrapping                     ( 10)
      assets/art                                              ( 40)
      (and many smaller groups)
    evidence: ~2 855 'D' entries in git status. These represent staged
              deletions awaiting `git rm` to land in the index. They are not
              clutter per se — they are cleanup work that hasn't been
              finalized.
    expected contract: index = working tree. Each batch owner (per
                       WORKTREE_OWNERSHIP.md recent claims, e.g.
                       DECLUTTER-IGNORED-BUILD-CACHE) is responsible.
    proposed owner: Integrator.

C3.  Massive TRX file in working tree (potential churn)
    severity: MED     confidence: HIGH
    path: Ashfall.Core.Tests/TestResults/p87.trx (15.5 MB)
    evidence: top-1 by size in the entire tracked tree. Generated by test runs;
              not in .gitignore.
    expected contract: regenerated on every test run; should be either ignored
                       (add '*.trx' to .gitignore) or quarantined.
    proposed owner: Foreman (decide) — Integrator executes.

C4.  BarlowCondensed SDF font assets re-dirty on every Unity run
    severity: MED     confidence: HIGH (per project memory)
    paths:
      assets/ui/MainMenu/Fonts/BarlowCondensed-SemiBold_SDF.asset (1.38 MB)
      assets/ui/MainMenu/Fonts/BarlowCondensed-Regular_SDF.asset  (1.16 MB)
      assets/ui/MainMenu/Fonts/BarlowCondensed-Bold_SDF.asset      (0.89 MB)
    evidence: per MEMORY.md 'font-atlas-churn.md' — these *_SDF.asset files
              re-dirty on every Unity run with non-deterministic TMP data and
              should be reverted, never committed. They are tracked but should
              be gitignored.
    expected contract: TMP font assets regenerated by tooling, never committed.
    proposed owner: Foreman (governance — confirm Unity retired status; the
                    project is Godot-only, so the SDF.asset may already be
                    obsolete).

C5.  audio duplication across assets/audio/, .cache/audio_* (3× copies)
    severity: HIGH    confidence: HIGH
    evidence: geiger.wav appears in:
                assets/audio/sfx/geiger.wav                          (705 678 B)
                .cache/audio_preservation_pre_flagship/sfx/geiger.wav (705 644 B)
                .cache/audio_library_pre_remaster/sfx/geiger.wav      (705 644 B)
              vo_verdict_geophone.wav appears in three locations identically.
              vo_verdict_count.wav appears in three locations identically.
    expected contract: one canonical source of truth (assets/audio/). The
                       .cache/ copies violate .gitignore rule.
    proposed owner: Foreman + Integrator (history-touching; requires approval).

C6.  docs/visual/_phase13_* and pixel_signature_stats.json (regenerable)
    severity: LOW     confidence: MED
    paths:
      docs/visual/_phase13_missing_classification.json
      docs/visual/_phase13_wiring_stats.json
      docs/visual/pixel_signature_stats.json
    evidence: files with leading underscore and 'phase13' suggest a one-off
              analysis run. Tracked but not regenerated by any current script
              in scripts/ci/ that I could identify in this sweep.
    expected contract: generated analytics should live in artifacts/ (which has
                       .gdignore) or be excluded.
    proposed owner: Foreman (governance).

C7.  Various generated docs/* without canonical index entry
    severity: LOW     confidence: MED
    paths (untracked, not yet in any index):
      docs/architecture/MAIN_DECOMPOSITION_MAP.md
      docs/architecture/UTILITY_AI_UNIFICATION.md
      docs/architecture/WORN_GEAR_CONSOLIDATION.md
      docs/process/AI_FOREMAN_ACCELERATION_PLAN.md
      docs/EXPEDITION_30_DAY_PLAYTEST_REPORT.md
      docs/EXPEDITION_BALANCE_BASELINE.md
      docs/EXPEDITION_VEHICLE_DOMINANCE_TABLE.md
      docs/MEDICAL_30_DAY_CAPACITY_REPORT.md
      docs/MEDICAL_DOSE_TREATMENT_MATRIX.md
      docs/MEDICAL_PIPELINE_JOURNEY.md
      docs/SHELTER_30_DAY_MAINTENANCE_REPORT.md
      docs/SHELTER_MAINTENANCE_MATRIX.md
      docs/PLANS_118_121_ADVANCED_INDUSTRIAL_RECON_CLOSEOUT.md
      docs/PLANS_118_121_AUTHORITY_MAP.md
      docs/PLANS_51_54_INTEGRATION_REPORT.md
      docs/ACTION_RESULT_SURFACING_MATRIX.md
      docs/ECONOMY_FAIRNESS_AUDIT.md
      docs/ECONOMY_PRICE_FACTOR_MATRIX.md
      docs/gaps/logs/PLANS_146_149_MED_SEAL_LOG.md
      docs/gaps/logs/PLANS_146_149_PLAYER_COMMAND_SEAL_LOG.md
      docs/gaps/logs/PLANS_150_153_NARRATIVE_ACTIVATION_SEAL_LOG.md
      docs/plans/PLANS_142_145_WAVE1_SHARED_CONTRACTS_PLAN.md
    evidence: untracked. None are cross-referenced from docs/CURRENT_AUTHORITY.md
              or INTEGRATION_PLANS.md (grep returns zero).
    expected contract: live docs should be enumerated by the docs atlas index
                       generated by scripts/ci/generate-docs-index.py.
    proposed owner: Foreman (decide: track, archive, or delete) — Integrator
                    runs the index generator.

C8.  scripts/run_test.sh and scripts/ci/run-godot-bounded.sh
    severity: LOW     confidence: HIGH (not an anomaly — confirming presence)
    evidence: both files exist; scripts/run_test.sh is referenced from
              INTEGRATION_PLANS.md and KNOWN_DEBT.md as the canonical
              verification entrypoint.
    expected contract: present and executable. No action.
    proposed owner: — (information only).

================================================================================
SECTION D — ITEMS EXPLICITLY VERIFIED AS NOT-CLUTTER (working-as-intended)
================================================================================

The following items LOOKED like clutter candidates on first inspection but
verified clean:

D1.  23 new Core engines + 9 new JSON catalogs + 12 new tests + 5 host/UI files
     under Assets/Ashfall.Core/, Assets/StreamingAssets/Data/, src/Host/,
     src/UI/, Ashfall.Core.Tests/ — all form coherent Plan-118/119/120/121/
     126-129/142-145/167/168 systems. Each engine has matching catalog +
     tests + host wiring. Verified via grep cross-reference; NOT clutter.

D2.  109 untracked src/Main.*.cs and src/Host/HostCli.*.cs partials are
     auto-wired via 'public partial class Main : Control' (Main.cs:37) and
     'public static partial class HostCli' (HostCli.cs:190).

D3.  assets/quarantine/* — explicitly quarantined by .gitignore
     '/assets/quarantine/'; gameplay cannot reach them.

D4.  .agents/skills/<dir>/ remaining 35 directories — referenced from the
     project memory and .claude/ dispatch.

D5.  artifacts/balance/*.csv — referenced from the balance-sim tooling in
     scripts/ci/. .gdignore at artifacts/balance/ prevents runtime import.

================================================================================
SECTION E — OUT OF SCOPE / NOT INVESTIGATED
================================================================================

The following were intentionally not exhaustively audited in this sweep to
stay within focused-verification scope:

- Per-file review of 239 modified M files in src/, Ashfall.Core.Tests/,
  Assets/Ashfall.Core/, .clinerules, .cursorrules, .windsurfrules,
  AGENTS.md, ANTIGRAVITY.md, etc. (these are intentional work-in-progress;
  per AI_AGENT_WORKFLOW.md a sweep must not modify them).
- The 109-file .qwen/skills/* mirror — recommended consolidation decision is
  for the Foreman; not auto-actionable.
- Build verification (dotnet build Ashfall.csproj, godot --headless ...) was
  not run; this sweep is read-only.

================================================================================
SECTION F — DEEPER FORENSIC FINDINGS (bugs / structural anomalies)
================================================================================

F1.  AI-tool workspaces tracked inconsistently with .gitignore policy
    severity: HIGH    confidence: HIGH
    paths (TRACKED despite .gitignore's intent):
      .qwen/         109 tracked,  1.3M
      .codex/         57 tracked,  1.3M
      .cursor/        59 tracked,  1.3M
      .agents/        80 tracked,  348K
      .kiro/           1 tracked,    4K
      .zcode/          4 tracked,   32K
    paths (gitignored — same family, properly handled):
      .crush/         0 tracked,   12K
      .composio/      0 tracked,    8K
      .mimocode/      0 tracked,  1.3M
      .aider/         0 tracked,  100K
      .commandcode/   0 tracked,   36K
      .qlty/          0 tracked,   24K
    evidence: .gitignore lines declare '.mistral/' and 'generated_AIassets/'
              as excluded 'AI-tool workspaces and generation staging', but
              .qwen/.codex/.cursor/.agents/.kiro/.zcode (same family) remain
              tracked. Confirmed by `scripts/ci/repo-hygiene-report.sh`:
              'consider .gitignore' warnings on each. Confirmed by `du` +
              `git ls-files | wc -l`.
    expected contract: consistent policy — either all AI-tool workspaces are
                       gitignored (matches the .mistral/ rule), or the rule
                       is rewritten to enumerate the kept ones.
    proposed owner: Foreman (governance) — Integrator executes.

F2.  Snapshot PNGs duplicated between snapshots/ and snapshot-capture/
    severity: MED     confidence: HIGH
    paths:
      snapshots/*.png           (31 PNGs, 2.3M,  69 tracked)
      snapshot-capture/*.png    (31 PNGs, 2.3M,  untracked)
    evidence: `diff <(ls snapshots/*.png | xargs -n1 basename | sort)
              <(ls snapshot-capture/*.png | xargs -n1 basename | sort)`
              returned ZERO diff lines — file set is IDENTICAL by name.
              Per .gitignore intent, snapshot-capture/ is the transient
              render target; snapshots/ holds the approved golden set. The
              duplicate 2.3M is wasted working-tree disk and risks being
              committed accidentally.
    expected contract: snapshot-capture/ is gitignored (it is); snapshots/
                       remains the source of truth. Confirm the symlink
                       target (Twin_ASHFall/generated/snapshot-capture/current)
                       is the actual transient location and the local
                       snapshot-capture/ is a stale copy.
    proposed owner: Foreman (decide: rebuild symlink or wipe stale dir).

F3.  Generic `throw new Exception(...)` calls (anti-pattern)
    severity: MED     confidence: HIGH
    paths:
      src/UI/SnapshotOrchestrator.cs:195
        `if (type == null) { throw new Exception($"type-not-found: ..."); }`
      src/UI/SnapshotOrchestrator.cs:217
        `if (hostRoot == null) { throw new Exception("no-SceneTree-root"); }`
      src/Host/SceneBindingSelfTest.cs:312
        `throw new Exception("scene root is not Control");`
    evidence: standard .NET practice is `throw new SpecificException(...)`
              with a stable type that callers can catch (e.g.,
              `InvalidOperationException`, `KeyNotFoundException`,
              `InvalidCastException`). `Exception` is the base type and
              hides intent; downstream `catch (Exception)` blocks (the
              project has 624 catches per catch-policy-gate) cannot
              distinguish these from system failures.
    expected contract: each throw should use the most specific subtype.
    proposed owner: Integrator (mechanical, ~3-line refactor).

F4.  `.gitattributes` declares `*.meta  unity-yaml`
    severity: MED     confidence: HIGH
    path: .gitattributes
    evidence: per CLAUDE.md rule #1 "Godot is authoritative; Unity is retired",
              the `unity-yaml` filter on `*.meta` is obsolete. The current
              Godot 4 `.meta` files are plain INI/UTF-8, not Unity YAML.
              This rule likely survives from the migration era and has been
              inert (Godot doesn't use Unity YAML anyway).
    expected contract: replace with the appropriate Godot filter
                       (`text eol=lf` or just leave default).
    proposed owner: Foreman (governance) — Integrator executes.

F5.  1 223 C# files missing SPDX-License-Identifier header
    severity: HIGH    confidence: HIGH
    paths:
      522 src/*.cs files missing header
      701 Assets/Ashfall.Core/*.cs files missing header
    evidence: `scripts/ci/license-header-check.sh` flags 17 sample files in
              `src/UI/` alone; the same script run over the full repo
              reports 522 + 701 = 1 223 missing headers. The script exits
              0 in warning-only mode but would exit 1 in --strict mode.
    expected contract: `// SPDX-License-Identifier: MIT` (and optional
                       ASHFALL copyright block) at top of every .cs file
                       per scripts/ci/license-header-check.sh policy.
    proposed owner: Foreman (decide: bulk-add headers as a hygiene batch,
                    or accept current state). 1 223 is a large batch;
                    recommend quarantining behind a hygiene claim.

F6.  Orphan .uid files in src/ (Godot import cache debris)
    severity: LOW     confidence: HIGH
    paths (untracked, gitignored by `*.uid` rule — local-only):
      src/Host/HostCli.PanelTests.Campaign.cs.uid
      src/Host/HostCli.PanelTests.Diagnostics.cs.uid
      src/Host/HostCli.PanelTests.Expansion.cs.uid
      src/Host/HostCli.PanelTests.Persistence.cs.uid
      src/Host/HostCli.PanelTests.UI.cs.uid
      src/Host/CampaignServices.cs.uid
      (+ 8 more in Assets/Ashfall.Core/, all untracked)
    evidence: 706 .uid files in src/, all untracked, 6 are orphans (no
              matching .cs file on disk). They exist as Godot's import-
              cache residue; not in git, not blocking, but they ARE
              dirty-tree debris that confuses `git status` filters.
    expected contract: Godot regenerates these on import; safe to ignore.
    proposed owner: — (informational, no action needed).

F7.  111 orphan .meta files (Godot metadata for still-present assets)
    severity: MED     confidence: HIGH
    paths:
      Assets/StreamingAssets/Data/*.json.meta (55)
      Assets/*.meta (29)
      Assets/Ashfall.Core/*.cs.meta (27)
    evidence: `git status --porcelain | grep '^ D.*\.meta$'` returns 111
              entries — tracked .meta files whose underlying asset
              (.cs/.json/.png/etc.) is still in the working tree. The
              Core ones are particularly anomalous: Core is `netstandard2.1`
              (.NET), but 27 .cs.meta files were generated for Core scripts
              when Godot tried to import them as scene scripts. The repo
              hygiene gate confirms .meta is a tracked artifact under
              `.gitattributes` (`*.meta unity-yaml`).
    expected contract: either `git rm` the .meta orphans (Core: never
                       needed; Data: regen on next Godot boot), or restore
                       them and treat the divergence as drift.
    proposed owner: Foreman + Integrator (governance + mechanical).

F8.  82 tracked orphan .import files in .cache/
    severity: HIGH    confidence: HIGH
    paths: .cache/audio_preservation_pre_flagship/**/*.wav.import
           .cache/audio_library_pre_remaster/**/*.wav.import (82 files)
    evidence: same root cause as A5 — .cache/ should be gitignored per
              .gitignore rule, but 314 files are tracked including 82
              .import companions. The .import files are Godot's resource
              import settings for the .wav files. Removing the .wav also
              requires removing its .import.
    expected contract: see A5 — pair removal of .wav + .import.
    proposed owner: same as A5 (Foreman + Integrator, approval-gated).

F9.  Skill catalog drift between .agents/skills/ and .qwen/skills/
    severity: HIGH    confidence: HIGH
    paths:
      19 skills in .agents/skills/ have NO mirror in .qwen/skills/:
        agents-sync, audio-qa, balance-sim, ci-migrate, codehealth-sweep,
        coverage-gate, data-schema, docs-atlas, export-build, localize,
        narrative-continuity, release-captain, repo-hygiene, save-fuzz,
        scene-port, seed-replay, snapshot-diff, test-gap, tutorial-review,
        ui-access
      1 skill in .qwen/skills/ has a renamed (not just renamed) form:
        ashfall-agents (qwen)  ↔  ashfall-agents-sync (agents)
      0 skills only in .qwen/skills/.
    evidence: directory listing of `.agents/skills/` (35 dirs) vs
              `.qwen/skills/` (only 16 matching dirs). Per CLAUDE.md /
              AI_AGENT_WORKFLOW.md only one skill catalog should be
              authoritative. .agents/skills/ is the project-canonical
              directory (referenced by .claude/ dispatch).
    expected contract: either remove .qwen/skills/* in favour of
                       .agents/skills/, or document .qwen/skills/ as a
                       legacy mirror with a no-drift check.
    proposed owner: Foreman (governance decision). Integrator executes.

F10. UI partial classes with no actual second partial (dead `partial` keyword)
    severity: LOW     confidence: HIGH
    paths: 43 src/UI/*.cs files declare `public partial class X : Control`
           but are the ONLY file declaring class X.
    evidence: 44 partial-class declarations in src/. 1 is genuine
              (Main.UiTests.cs is a decomposition stub listing 14
              partials). 1 is the legitimate SaveLoadHostSession partial
              in src/Host/. The remaining 43 in src/UI/ are singletons:
              e.g. BestiaryPanel declared at src/UI/BestiaryPanel.cs:20
              with no other partial. The `partial` keyword is dead.
    expected contract: drop `partial` from each singleton declaration;
                       or, if a second partial is planned, document why.
    proposed owner: Integrator (mechanical, low risk).

F11. Initial "orphan type" sweep was a false positive
    severity: INFO   confidence: HIGH
    evidence: my first orphan-detection pass flagged 52 src/ types with
              no external reference. Spot-check of 4 examples revealed
              they are all NESTED or DTO types used only within their
              enclosing class:
                - EcologicalInfestationDiseaseSource (nested sealed class
                  in Main.EcologicalInfestations.cs:186)
                - SevenDayDeterministicSmokeTest (called from
                  HostCli.SelfTests.cs:997)
                - EventsRoot / IncidentsRoot / NarrativeRoot (JSON
                  deserialization targets in EventsHostSession.cs:146/176/182)
    expected contract: trust the false-positive count of 0 and document
                       this as a lesson — naive orphan detection needs a
                       scope-aware scanner (Roslyn or grep-based with
                       namespace qualification) to avoid flagging
                       legitimately-nested classes.
    proposed owner: — (corrective documentation; no code change).

================================================================================
SECTION G — GATE-VERIFIED ARCHITECTURE HEALTH (positive findings)
================================================================================

These gates ran clean during this sweep and are recorded as positive
evidence that the project is fundamentally sound — clutter is at the
periphery, not at the architecture core.

G1.  Forbidden Core API Gate          : PASS  (0 engine namespaces in Core,
                                             0 System.Random, 0 Guid.NewGuid,
                                             0 legacy serializer bypasses,
                                             0 wall-clock drift, 0 Thread.Sleep)
G2.  Triad Drift Gate                 : PASS  (175 Save sections, 174 Save
                                             + 185 Setup methods, all
                                             documented)
G3.  Catch Policy Gate                : PASS  (624 catches checked, 0 empty,
                                             0 undocumented)
G4.  Persistent Filename Gate         : PASS  (244 save files + 2 user
                                             files strictly unique, mapped
                                             to 175 sections)
G5.  Legacy Asset Path Gate           : PASS  (0 prohibited runtime assets
                                             under Assets/art, sprites,
                                             ui, audio)
G6.  NuGet Dependency Gate            : PASS  (6 packages, 4 projects,
                                             CPM clean)
G7.  Scene Lint                       : PASS  (30 production scenes,
                                             0 errors, 0 warnings)
G8.  Godot Asset Gate                 : PASS  (data-integrity + bridge +
                                             survivors + world + economy
                                             + asset-registry selftests
                                             all green)
G9.  JSON Schema Policy Gate          : PASS  (593 files all valid schema;
                                             v1: 586, v2: 5, v3: 2)
G10. Compiler Warning Baseline Gate   : PASS  (0 warnings across Core,
                                             Core.Tests, Godot host)
G11. L10N Drift Gate                  : PASS  (359 keys, 69 pilot refs,
                                             German parity verified)
G12. Doc Link Gate                    : PASS  (2311 files checked, all
                                             portable relative links)
G13. Unity Contamination Scan         : PASS  (single hit is a comment
                                             'Report UnityEngine shim
                                             removal' — historical
                                             cleanup marker, not code)

================================================================================
SECTION H — UPDATED RECOMMENDATIONS (consolidated)
================================================================================

H1.  Foreman should schedule a governance-led hygiene batch to:
      a. Add `.qwen/`, `.codex/`, `.cursor/`, `.kiro/`, `.zcode/` to
         .gitignore (matches the .mistral/ precedent) OR document why
         these four are exempt. (See F1.)
      b. Rewrite .gitattributes: drop `*.meta unity-yaml`; add Godot
         defaults. (See F4.)
      c. Reconcile .qwen/skills/ with .agents/skills/ — the 19-skill gap
         and the renamed 'agents' ↔ 'agents-sync' drift must not silently
         compound. (See F9.)
      d. Decide on the 1 223 missing SPDX headers — bulk add as a
         quarantine batch OR amend the license-header-check.sh policy
         to grandfather pre-existing files. (See F5.)

H2.  Integrator (with explicit approval per repo-hygiene skill) should:
      a. `git rm` the 314 .cache/audio_* entries (with their 82 .import
         companions and the matching .cache files). (See A5 / F8.)
      b. `git rm` the 111 orphan .meta files in Data / Core, after
         Foreman decision. (See F7.)
      c. Replace the 3 `throw new Exception(...)` with specific subtypes.
         (See F3.)
      d. Land the 2 855 staged deletions (the ` D` entries) as `git rm`
         — these are already-cleanup work awaiting index sync.
      e. Strip the dead `partial` keyword from the 43 singleton UI
         classes. (See F10.)

H3.  Read-only / informational only:
      a. The 6 orphan .uid files are gitignored and not blocking.
         (See F6.)
      b. Snapshot duplication (snapshots/ vs snapshot-capture/) is
         a working-tree hygiene issue, not a tracked-data issue. (See F2.)
      c. Untracked `addons/ziva_agent/` is a real anomaly but a
         foreman-level decision is needed before any action. (See A9.)
      d. BUG_HUNT_25..29 and PLANS_*_FORENSIC_REPORT in docs/forensics/
         have zero references in governance or code. Archive decision
         belongs to the foreman. (See A1 / A2.)

================================================================================
END OF DEEPER SWEEP — 2026-09-12
================================================================================
