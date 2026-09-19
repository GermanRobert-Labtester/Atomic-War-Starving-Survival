# ASHFALL Multi-Agent Skills Registry & Taxonomy Index

> **Living Multi-Agent Navigation Guide**: Documents all specialized agent skills in `.agents/skills/` organized across 6 functional capability domains. Used by Antigravity, Claude, Codex, Cline, Cursor, and Windsurf AI agents for instant tool discovery.

**Total Registered Skills:** `35`<br>
**Last Verified:** `2026-09-19`<br>
**Drift Gated:** `python3 scripts/ci/generate-agent-skills-catalog.py --check`

---

## Domain Taxonomy Navigation

- [Forensics & Subsystem Audits](#forensics--subsystem-audits)
- [Architecture & Integration Planning](#architecture--integration-planning)
- [Expansions & Content Systems](#expansions--content-systems)
- [UI, Panels & Visual Presentation](#ui-panels--visual-presentation)
- [Audio, Localization & Asset Pipeline](#audio-localization--asset-pipeline)
- [Determinism, Save Resilience & Hardening](#determinism-save-resilience--hardening)

---

## Forensics & Subsystem Audits

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-analyze`](../../.agents/skills/ashfall-analyze/SKILL.md) | Evidence-first, read-only forensic analysis of a specific ASHFALL feature, subsystem, bug, design area, integration seam, or implementation claim. | `.agents/skills/ashfall-analyze/SKILL.md` |
| [`ashfall-codehealth-sweep`](../../.agents/skills/ashfall-codehealth-sweep/SKILL.md) | Runs a CodeScene-style structural health sweep for Core/src — god classes (Main.cs 6.5k, GameBootstrap 82 partials), duplication (WornGear x2, HoldfastRuntimeSession), bare catch{}, and 0-engine-ref violations. Use when planning refactors or before large merges. | `.agents/skills/ashfall-codehealth-sweep/SKILL.md` |
| [`ashfall-tutorial-review`](../../.agents/skills/ashfall-tutorial-review/SKILL.md) | Audits ASHFALL's first-hour onboarding — TutorialPanel, initial state, early resource pressure, and teach-vs-demand gaps — against what the survival systems actually require from the player. | `.agents/skills/ashfall-tutorial-review/SKILL.md` |
| [`ashfall-scan`](../../.agents/skills/ashfall-scan/SKILL.md) | Deep forensic ASHFALL audit for unimplemented gaps, silent failures, unwired or unreachable code, incomplete migration paths, syntax/API misuse, placeholder logic, stale contracts, missing registrations, dead callbacks, data-consumer gaps, and code that compiles without actually functioning. | `.agents/skills/ashfall-scan/SKILL.md` |
| [`ashfall-test-gap`](../../.agents/skills/ashfall-test-gap/SKILL.md) | Maps ASHFALL systems to their test coverage, finds missing save/round-trip/determinism tests (e.g. H10 NeedsSystem round-trips, H11 JournalSystem), and scaffolds skeletons in Ashfall.Core.Tests. Coverage cartography before coverage writing. | `.agents/skills/ashfall-test-gap/SKILL.md` |

## Architecture & Integration Planning

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-plan`](../../.agents/skills/ashfall-plan/SKILL.md) | Builds evidence-grounded, dependency-ordered ASHFALL integration plans from forensic findings without modifying production code. | `.agents/skills/ashfall-plan/SKILL.md` |
| [`ashfall-task-frame`](../../.agents/skills/ashfall-task-frame/SKILL.md) | Generates the 2-line goal, file list, and verification checklist for any task. For when the AI already knows the target. | `.agents/skills/ashfall-task-frame/SKILL.md` |
| [`ashfall-docs-atlas`](../../.agents/skills/ashfall-docs-atlas/SKILL.md) | Maps, dedupes, and archives ASHFALL's accumulated planning/audit documentation so agents read current truth — identifies superseded plans, stale audits, and duplicates among 40+ root and docs/ markdowns. | `.agents/skills/ashfall-docs-atlas/SKILL.md` |
| [`ashfall-design`](../../.agents/skills/ashfall-design/SKILL.md) | End-to-end ASHFALL visual production orchestrator. Forensically audits the game for missing, placeholder, inconsistent, unwired, or low-quality visual assets; discovers available MCP/AI tools including Composio, Gemini image generation, ChatGPT image generation/editing, Canva, Figma, Google Stitch, and local Linux art tools; generates or redesigns assets through the best available provider; processes, imports, wires, verifies, iterates, and performs visual QA inside the active Godot project. | `.agents/skills/ashfall-design/SKILL.md` |

## Expansions & Content Systems

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-expand`](../../.agents/skills/ashfall-expand/SKILL.md) | High-quality canon-aware creative writer and systemic game expansion designer for ASHFALL quests, factions, locations, encounters, and worldbuilding. | `.agents/skills/ashfall-expand/SKILL.md` |
| [`ashfall-data-add`](../../.agents/skills/ashfall-data-add/SKILL.md) | Generates new item/quest/location JSON with correct schema_version, snake_case ID, and CatalogIntegrityValidator pass. For when the AI already knows the data schema. | `.agents/skills/ashfall-data-add/SKILL.md` |
| [`ashfall-data-schema`](../../.agents/skills/ashfall-data-schema/SKILL.md) | Sweeps ASHFALL's ~280 data-authority JSON files to add missing schema_version, normalize camelCase to snake_case with migration notes, and gate regression through CatalogIntegrityValidator. Data hygiene without forking authority. | `.agents/skills/ashfall-data-schema/SKILL.md` |
| [`ashfall-foundry`](../../.agents/skills/ashfall-foundry/SKILL.md) | Linux-first ASHFALL technical visual-production skill for procedural textures, materials, shaders, shadows, ambient lighting, particles, animation, sprite effects, masks, decals, environmental effects, texture families, and technical game visuals. Uses installed local Linux applications as the production authority while optionally using connected Gemini/Nano Banana or ChatGPT image generation only as references, seeds, or concept sources before reconstructing production-ready assets locally and wiring them into Godot. | `.agents/skills/ashfall-foundry/SKILL.md` |
| [`ashfall-write`](../../.agents/skills/ashfall-write/SKILL.md) | High-quality ASHFALL diegetic writing specialist for item descriptions, notes, journals, radio transmissions, logs, records, letters, notices, environmental text, medical documents, expedition reports, survivor writings, archival fragments, and other low- or high-volume in-game prose. Maintains grounded tone, distinct voices, systemic context, continuity, variation, and implementation-ready formatting. | `.agents/skills/ashfall-write/SKILL.md` |
| [`ashfall-narrative-continuity`](../../.agents/skills/ashfall-narrative-continuity/SKILL.md) | Cross-file canon and flag consistency auditor for ASHFALL's 199 narrative JSON files — quests, encounters, echoes, radio, flags, factions. Finds continuity breaks across the story graph without rewriting prose. | `.agents/skills/ashfall-narrative-continuity/SKILL.md` |

## UI, Panels & Visual Presentation

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-ui-access`](../../.agents/skills/ashfall-ui-access/SKILL.md) | Audits ASHFALL's fixed 1920x1080 UI for accessibility and usability — contrast, overflow, scaling, keyboard navigation, readability — across the 207-file UI tree, with evidence-based findings. | `.agents/skills/ashfall-ui-access/SKILL.md` |
| [`ashfall-snapshot-diff`](../../.agents/skills/ashfall-snapshot-diff/SKILL.md) | Re-renders ASHFALL's 69 golden UI snapshot panels and diffs them against snapshots/ to catch visual regressions before QA, using only headless Godot rendering and image comparison tooling. | `.agents/skills/ashfall-snapshot-diff/SKILL.md` |

## Audio, Localization & Asset Pipeline

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-audio-qa`](../../.agents/skills/ashfall-audio-qa/SKILL.md) | Audits ASHFALL's audio pipeline — cue catalog, event bridge, AudioManager wiring, orphan detection, loudness normalization, and format policy compliance across generated and migrated sound assets. | `.agents/skills/ashfall-audio-qa/SKILL.md` |
| [`ashfall-scene-port`](../../.agents/skills/ashfall-scene-port/SKILL.md) | Migrates remaining Unity-era assets (Assets/art ~2000 files, sprites, ui, audio/radio) into the Godot root assets/ tree with import settings, and ports any scene/prefab found. The standardized Unity-to-Godot asset migration path. | `.agents/skills/ashfall-scene-port/SKILL.md` |
| [`ashfall-localize`](../../.agents/skills/ashfall-localize/SKILL.md) | Bootstraps ASHFALL localization — inventories user-facing strings across UI, data JSON, and diegetic text, scaffolds a Godot translation layer, and gates new hardcoded strings. Prepares the game for store release. | `.agents/skills/ashfall-localize/SKILL.md` |
| [`ashfall-export-build`](../../.agents/skills/ashfall-export-build/SKILL.md) | Runs headless Godot exports for ASHFALL's configured presets (Linux/Windows), verifies the PCK includes the JSON data authority, smoke-boots the binary, and reports size/regressions. The bridge between passing selftests and a shippable build. | `.agents/skills/ashfall-export-build/SKILL.md` |

## Determinism, Save Resilience & Hardening

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-save-fuzz`](../../.agents/skills/ashfall-save-fuzz/SKILL.md) | Stress-tests every ASHFALL save store and codec with round-trips, checksum mutation, legacy-envelope fallbacks, and cross-codec migrations using dotnet tests and godot --headless only. Finds save corruption before players do. | `.agents/skills/ashfall-save-fuzz/SKILL.md` |
| [`ashfall-seed-replay`](../../.agents/skills/ashfall-seed-replay/SKILL.md) | Proves ASHFALL determinism by running paired same-seed simulations across hosts, saves, and code paths, hashing final state, and flagging divergence. Guards Invariant 4 using dotnet + godot --headless only. | `.agents/skills/ashfall-seed-replay/SKILL.md` |
| [`ashfall-determinism-guard`](../../.agents/skills/ashfall-determinism-guard/SKILL.md) | Runs paired seeded replays for new systems, flags System.Random/Guid.NewGuid usage, and pins SaveChecksum culture-invariant formatting. For when the AI already knows the determinism rules. | `.agents/skills/ashfall-determinism-guard/SKILL.md` |
| [`ashfall-coverage-gate`](../../.agents/skills/ashfall-coverage-gate/SKILL.md) | Gates xUnit coverage via coverlet for Ashfall.Core, enforcing save round-trip and determinism coverage for H10/H11 gaps. Use when adding systems, before PR, or to close test-gap findings. | `.agents/skills/ashfall-coverage-gate/SKILL.md` |
| [`ashfall-harden`](../../.agents/skills/ashfall-harden/SKILL.md) | Aggressively probes ASHFALL architecture for fragility, hidden coupling, ownership ambiguity, migration debt, save/determinism risks, weak contracts, runtime islands, lifecycle hazards, and future scalability problems, then ranks evidence-based system-hardening next steps without modifying production code. | `.agents/skills/ashfall-harden/SKILL.md` |
| [`ashfall-seal`](../../.agents/skills/ashfall-seal/SKILL.md) | Validates ASHFALL implementation-gap findings, designs the missing behavior and wiring procedure, seals unimplemented and silent gaps, connects Core/data/Godot/save/test paths carefully, and verifies complete end-to-end functionality without introducing duplicate architecture. | `.agents/skills/ashfall-seal/SKILL.md` |
| [`ashfall-repair`](../../.agents/skills/ashfall-repair/SKILL.md) | Deeply validates ASHFALL bug findings, determines root cause and blast radius, designs a minimal evidence-backed repair plan, then integrates the repair phase by phase with a fresh forensic checkpoint before every change and full regression verification afterward. | `.agents/skills/ashfall-repair/SKILL.md` |
| [`ashfall-release-captain`](../../.agents/skills/ashfall-release-captain/SKILL.md) | Coordinates ASHFALL releases — version bump, changelog from git history, lane/snap discipline, full pre-release gate (tests, data integrity, asset gate, export smoke), and release checklist. Shipping discipline for the Godot era. | `.agents/skills/ashfall-release-captain/SKILL.md` |
| [`ashfall-repo-hygiene`](../../.agents/skills/ashfall-repo-hygiene/SKILL.md) | Audits and safely quarantines repository junk in ASHFALL (Unity-era test XMLs, audit dumps, stray root binaries), verifies Git LFS policy compliance, and keeps clone size healthy. Dry-run by default; never deletes without approval. | `.agents/skills/ashfall-repo-hygiene/SKILL.md` |
| [`ashfall-ci-migrate`](../../.agents/skills/ashfall-ci-migrate/SKILL.md) | Migrates and maintains ASHFALL GitHub Actions from the stale Unity pipeline to the canonical dotnet + godot --headless gate. Detects CI drift, rewrites workflows, and verifies gates run clean. | `.agents/skills/ashfall-ci-migrate/SKILL.md` |
| [`ashfall-balance-sim`](../../.agents/skills/ashfall-balance-sim/SKILL.md) | Runs seeded headless simulations and parameter sweeps over ASHFALL's data-driven systems (economy, radiation/dose, needs, cohorts, trade) to produce evidence-based balance and difficulty reports, using dotnet tests and godot --headless only. | `.agents/skills/ashfall-balance-sim/SKILL.md` |
| [`ashfall-tune`](../../.agents/skills/ashfall-tune/SKILL.md) | Deep forensic ASHFALL performance-engineering and code-debloating skill. Profiles CPU, GPU, memory, allocations, startup, scene loading, UI, physics, rendering, shaders, particles, data access, saves, long-session behavior, and architectural bloat; identifies proven bottlenecks; designs minimal surgical optimizations; removes dead or redundant cost; implements carefully; and verifies stable frame pacing on the lowest practical hardware target without changing intended gameplay. | `.agents/skills/ashfall-tune/SKILL.md` |
| [`ashfall-agents-sync`](../../.agents/skills/ashfall-agents-sync/SKILL.md) | Detects drift across ASHFALL's many AI-agent rule files (AGENTS.md, CLAUDE.md, CODEX.md, CRUSH.md, GOOSE.md, QWEN.md, VIBE.md, MIMOCODE.md, OPENSETUP.md, ANTIGRAVITY.md, .clinerules, .cursorrules, .windsurfrules) and re-syncs them so all agents read one truth. | `.agents/skills/ashfall-agents-sync/SKILL.md` |

## Additional General Skills

| Skill Name | Purpose & Trigger | Location |
|---|---|---|
| [`ashfall-implement`](../../.agents/skills/ashfall-implement/SKILL.md) | Executes an approved ASHFALL integration plan conservatively, phase by phase, with pre-change verification, minimal diffs, and mandatory tests. | `.agents/skills/ashfall-implement/SKILL.md` |
