# PLAN-AGENT-WORKFLOW-GOVERNANCE-59 — Appendix A: Skills Inventory

**Generated:** 2026-09-21 from `.agents/skills/**` and `~/.pi/agent/skills/**`
(98 skills with SKILL.md).
**Use:** AG-59C — every skill needs a valid frontmatter (name, description,
location), a trigger condition, and a catalogue row; dead or duplicate skills
are retired. The generated catalogue gate keeps this inventory true.

## Skills

| Skill dir | Source | Description (≤110) |
|---|---|---|
| `ashfall-agents-sync` | `.agents/skills` | Detects drift across ASHFALL's many AI-agent rule files (AGENTS.md, CLAUDE.md, CODEX.md, CRUSH.md, GOOSE.md, Q |
| `ashfall-analyze` | `.agents/skills` | Evidence-first, read-only forensic analysis of a specific ASHFALL feature, subsystem, bug, design area, integr |
| `ashfall-audio-qa` | `.agents/skills` | Audits ASHFALL's audio pipeline — cue catalog, event bridge, AudioManager wiring, orphan detection, loudness n |
| `ashfall-balance-sim` | `.agents/skills` | Runs seeded headless simulations and parameter sweeps over ASHFALL's data-driven systems (economy, radiation/d |
| `ashfall-ci-migrate` | `.agents/skills` | Migrates and maintains ASHFALL GitHub Actions from the stale Unity pipeline to the canonical dotnet + godot -- |
| `ashfall-codehealth-sweep` | `.agents/skills` | Runs a CodeScene-style structural health sweep for Core/src — god classes (Main.cs 6.5k, GameBootstrap 82 part |
| `ashfall-coverage-gate` | `.agents/skills` | Gates xUnit coverage via coverlet for Ashfall.Core, enforcing save round-trip and determinism coverage for H10 |
| `ashfall-data-add` | `.agents/skills` | Generates new item/quest/location JSON with correct schema_version, snake_case ID, and CatalogIntegrityValidat |
| `ashfall-data-schema` | `.agents/skills` | Sweeps ASHFALL's ~280 data-authority JSON files to add missing schema_version, normalize camelCase to snake_ca |
| `ashfall-design` | `.agents/skills` | End-to-end ASHFALL visual production orchestrator. Forensically audits the game for missing, placeholder, inco |
| `ashfall-determinism-guard` | `.agents/skills` | Runs paired seeded replays for new systems, flags System.Random/Guid.NewGuid usage, and pins SaveChecksum cult |
| `ashfall-docs-atlas` | `.agents/skills` | Maps, dedupes, and archives ASHFALL's accumulated planning/audit documentation so agents read current truth —  |
| `ashfall-expand` | `.agents/skills` | High-quality canon-aware creative writer and systemic game expansion designer for ASHFALL quests, factions, lo |
| `ashfall-export-build` | `.agents/skills` | Runs headless Godot exports for ASHFALL's configured presets (Linux/Windows), verifies the PCK includes the JS |
| `ashfall-foundry` | `.agents/skills` | Linux-first ASHFALL technical visual-production skill for procedural textures, materials, shaders, shadows, am |
| `ashfall-harden` | `.agents/skills` | Aggressively probes ASHFALL architecture for fragility, hidden coupling, ownership ambiguity, migration debt,  |
| `ashfall-implement` | `.agents/skills` | Executes an approved ASHFALL integration plan conservatively, phase by phase, with pre-change verification, mi |
| `ashfall-localize` | `.agents/skills` | Bootstraps ASHFALL localization — inventories user-facing strings across UI, data JSON, and diegetic text, sca |
| `ashfall-narrative-continuity` | `.agents/skills` | Cross-file canon and flag consistency auditor for ASHFALL's 199 narrative JSON files — quests, encounters, ech |
| `ashfall-plan` | `.agents/skills` | Builds evidence-grounded, dependency-ordered ASHFALL integration plans from forensic findings without modifyin |
| `ashfall-release-captain` | `.agents/skills` | Coordinates ASHFALL releases — version bump via prepare-release.sh, changelog generation, branch/tag/PR discip |
| `ashfall-repair` | `.agents/skills` | Deeply validates ASHFALL bug findings, determines root cause and blast radius, designs a minimal evidence-back |
| `ashfall-repo-hygiene` | `.agents/skills` | Audits and safely quarantines repository junk in ASHFALL (Unity-era test XMLs, audit dumps, stray root binarie |
| `ashfall-save-fuzz` | `.agents/skills` | Stress-tests every ASHFALL save store and codec with round-trips, checksum mutation, legacy-envelope fallbacks |
| `ashfall-scan` | `.agents/skills` | Deep forensic ASHFALL audit for unimplemented gaps, silent failures, unwired or unreachable code, incomplete m |
| `ashfall-scene-port` | `.agents/skills` | Migrates remaining Unity-era assets (Assets/art ~2000 files, sprites, ui, audio/radio) into the Godot root ass |
| `ashfall-seal` | `.agents/skills` | Validates ASHFALL implementation-gap findings, designs the missing behavior and wiring procedure, seals unimpl |
| `ashfall-seed-replay` | `.agents/skills` | Proves ASHFALL determinism by running paired same-seed simulations across hosts, saves, and code paths, hashin |
| `ashfall-snapshot-diff` | `.agents/skills` | Re-renders ASHFALL's 69 golden UI snapshot panels and diffs them against snapshots/ to catch visual regression |
| `ashfall-task-frame` | `.agents/skills` | Generates the 2-line goal, file list, and verification checklist for any task. For when the AI already knows t |
| `ashfall-test-gap` | `.agents/skills` | Maps ASHFALL systems to their test coverage, finds missing save/round-trip/determinism tests (e.g. H10 NeedsSy |
| `ashfall-tune` | `.agents/skills` | Deep forensic ASHFALL performance-engineering and code-debloating skill. Profiles CPU, GPU, memory, allocation |
| `ashfall-tutorial-review` | `.agents/skills` | Audits ASHFALL's first-hour onboarding — TutorialPanel, initial state, early resource pressure, and teach-vs-d |
| `ashfall-ui-access` | `.agents/skills` | Audits ASHFALL's fixed 1920x1080 UI for accessibility and usability — contrast, overflow, scaling, keyboard na |
| `ashfall-write` | `.agents/skills` | High-quality ASHFALL diegetic writing specialist for item descriptions, notes, journals, radio transmissions,  |
| `ashfall-agents-sync` | `../../../.pi/agent/skills` | Detects drift across ASHFALL's many AI-agent rule files (AGENTS.md, CLAUDE.md, CODEX.md, CRUSH.md, GOOSE.md, Q |
| `ashfall-asset-counter` | `../../../.pi/agent/skills` | >- |
| `ashfall-audio-qa` | `../../../.pi/agent/skills` | Audits ASHFALL's audio pipeline — cue catalog, event bridge, AudioManager wiring, orphan detection, loudness n |
| `ashfall-balance-sim` | `../../../.pi/agent/skills` | Runs seeded headless simulations and parameter sweeps over ASHFALL's data-driven systems (economy, radiation/d |
| `ashfall-bug-predictor` | `../../../.pi/agent/skills` | >- |
| `ashfall-build-validator` | `../../../.pi/agent/skills` | >- |
| `ashfall-ci-migrate` | `../../../.pi/agent/skills` | Migrates and maintains ASHFALL GitHub Actions from the stale Unity pipeline to the canonical dotnet + godot -- |
| `ashfall-code-assistant` | `../../../.pi/agent/skills` | >- |
| `ashfall-content-generator` | `../../../.pi/agent/skills` | >- |
| `ashfall-data-add` | `../../../.pi/agent/skills` | Generates new item/quest/location JSON with correct schema_version, snake_case ID, and CatalogIntegrityValidat |
| `ashfall-data-integrity-auditor` | `../../../.pi/agent/skills` | >- |
| `ashfall-data-schema` | `../../../.pi/agent/skills` | Sweeps ASHFALL's ~280 data-authority JSON files to add missing schema_version, normalize camelCase to snake_ca |
| `ashfall-debloater-forensic` | `../../../.pi/agent/skills` | >- |
| `ashfall-determinism-guard` | `../../../.pi/agent/skills` | Runs paired seeded replays for new systems, flags System.Random/Guid.NewGuid usage, and pins SaveChecksum cult |
| `ashfall-docs-atlas` | `../../../.pi/agent/skills` | Maps, dedupes, and archives ASHFALL's accumulated planning/audit documentation so agents read current truth —  |
| `ashfall-export-build` | `../../../.pi/agent/skills` | Runs headless Godot exports for ASHFALL's configured presets (Linux/Windows), verifies the PCK includes the JS |
| `ashfall-localize` | `../../../.pi/agent/skills` | Bootstraps ASHFALL localization — inventories user-facing strings across UI, data JSON, and diegetic text, sca |
| `ashfall-narrative-continuity` | `../../../.pi/agent/skills` | Cross-file canon and flag consistency auditor for ASHFALL's 199 narrative JSON files — quests, encounters, ech |
| `ashfall-performance-analyzer` | `../../../.pi/agent/skills` | >- |
| `ashfall-problem-identifier` | `../../../.pi/agent/skills` | >- |
| `ashfall-refactoring-engine` | `../../../.pi/agent/skills` | >- |
| `ashfall-release-captain` | `../../../.pi/agent/skills` | Coordinates ASHFALL releases — version bump via prepare-release.sh, changelog generation, branch/tag/PR discip |
| `ashfall-repo-hygiene` | `../../../.pi/agent/skills` | Audits and safely quarantines repository junk in ASHFALL (Unity-era test XMLs, audit dumps, stray root binarie |
| `ashfall-save-fuzz` | `../../../.pi/agent/skills` | Stress-tests every ASHFALL save store and codec with round-trips, checksum mutation, legacy-envelope fallbacks |
| `ashfall-scene-port` | `../../../.pi/agent/skills` | Migrates remaining Unity-era assets (Assets/art ~2000 files, sprites, ui, audio/radio) into the Godot root ass |
| `ashfall-seed-replay` | `../../../.pi/agent/skills` | Proves ASHFALL determinism by running paired same-seed simulations across hosts, saves, and code paths, hashin |
| `ashfall-silent` | `../../../.pi/agent/skills` | >- |
| `ashfall-silent-reaudit` | `../../../.pi/agent/skills` | >- |
| `ashfall-silent-repair` | `../../../.pi/agent/skills` | >- |
| `ashfall-snapshot-diff` | `../../../.pi/agent/skills` | Re-renders ASHFALL's 69 golden UI snapshot panels and diffs them against snapshots/ to catch visual regression |
| `ashfall-task-frame` | `../../../.pi/agent/skills` | Generates the 2-line goal, file list, and verification checklist for any task. For when the AI already knows t |
| `ashfall-test-gap` | `../../../.pi/agent/skills` | Maps ASHFALL systems to their test coverage, finds missing save/round-trip/determinism tests (e.g. H10 NeedsSy |
| `ashfall-time-travel-debugger` | `../../../.pi/agent/skills` | >- |
| `ashfall-tutorial-review` | `../../../.pi/agent/skills` | Audits ASHFALL's first-hour onboarding — TutorialPanel, initial state, early resource pressure, and teach-vs-d |
| `ashfall-ui-access` | `../../../.pi/agent/skills` | Audits ASHFALL's fixed 1920x1080 UI for accessibility and usability — contrast, overflow, scaling, keyboard na |
| `automate` | `../../../.pi/agent/skills` | Use this skill to create Codex Automations. |
| `autopilot` | `../../../.pi/agent/skills` | >- |
| `canvas` | `../../../.pi/agent/skills` | >- |
| `create-hook` | `../../../.pi/agent/skills` | >- |
| `create-rule` | `../../../.pi/agent/skills` | >- |
| `create-skill` | `../../../.pi/agent/skills` | >- |
| `create-subagent` | `../../../.pi/agent/skills` | >- |
| `gameforge-prompt-optimizer` | `../../../.pi/agent/skills` | "Master game-development prompt optimizer: converts rough, incomplete, or overly broad game-dev requests into  |
| `generation-prompt-builder` | `../../../.pi/agent/skills` | Build or optimize high-quality generation prompts from copy, product information, scripts, rough ideas, refere |
| `gpt-image-assistant` | `../../../.pi/agent/skills` | GPT Image workflow assistant for text-to-image, reference-image editing, inpainting instructions, posters, typ |
| `loop` | `../../../.pi/agent/skills` | >- |
| `migrate-to-skills` | `../../../.pi/agent/skills` | >- |
| `neon` | `../../../.pi/agent/skills` | >- |
| `neon-postgres` | `../../../.pi/agent/skills` | >- |
| `prompt-optimizer` | `../../../.pi/agent/skills` | >- |
| `prompt-preflight-advice` | `../../../.pi/agent/skills` | Provide concise, goal-aligned suggestions before generating or optimizing image, video, audio, GPT Image, or s |
| `rename-chat` | `../../../.pi/agent/skills` | >- |
| `review` | `../../../.pi/agent/skills` | Review code changes with the Bugbot or Security Review subagent. |
| `review-bugbot` | `../../../.pi/agent/skills` | Review code changes with Bugbot subagent. |
| `review-security` | `../../../.pi/agent/skills` | Review code changes with Security Review subagent. |
| `sdk` | `../../../.pi/agent/skills` | >- |
| `sentry-cli` | `../../../.pi/agent/skills` | Guide for using the Sentry CLI to interact with Sentry from the command line. Use when the user asks about vie |
| `shell` | `../../../.pi/agent/skills` | >- |
| `split-to-prs` | `../../../.pi/agent/skills` | >- |
| `statusline` | `../../../.pi/agent/skills` | >- |
| `storyboard-prompt-assistant` | `../../../.pi/agent/skills` | Convert concepts, scripts, ad copy, product ideas, rough stories, or vague video ideas into high-quality story |
| `update-cli-config` | `../../../.pi/agent/skills` | >- |
| `update-cursor-settings` | `../../../.pi/agent/skills` | >- |
