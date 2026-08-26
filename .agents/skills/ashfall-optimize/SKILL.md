---
name: ashfall-optimize
description: "ASHFALL prompt compiler: turns rough requests into execution-ready prompts for coding agents and creative expansion LLMs."
---

# ASHFALL Prompt Optimizer

> **CRITICAL ARCHITECTURE DIRECTIVE**: Godot is authoritative. Unity is deprecated legacy architecture. Never introduce or restore Unity dependencies. Existing Unity references are migration artifacts to remove, port, or isolate—not architectural guidance.


## Purpose

This skill is a project-specific prompt compiler for **ASHFALL: Atomic War - Starving Survival**.

It does not merely make prompts longer or more polished. It converts rough ASHFALL requests into high-quality task specifications that preserve the game's canon, architecture, migration direction, data model, design pillars, cross-system dependencies, verification requirements, and current uncertainties.

The skill supports two primary jobs and one combined job:

1. **CODING OPTIMIZATION** — manufacture prompts for coding agents that inspect the real repository, reason about dependencies, make bounded implementation changes, preserve deterministic/save-safe architecture, and verify results.
2. **CREATIVE EXPANSION OPTIMIZATION** — manufacture prompts for ASHFALL-specific worldbuilding, systems expansion, quests, factions, characters, locations, encounters, narrative, content, balance, UI concepts, and expansion design without drifting into generic survival-game ideas.
3. **HYBRID EXPANSION + IMPLEMENTATION** — design creative expansion content and then translate it into an implementation-aware execution plan or implementation prompt for the repository.

This skill is **not generic**. Never repurpose it for another game.

---

# 1. Mandatory Reference

The bundled file `references/skillcontext.md` is the ASHFALL project intelligence dossier generated from the repository.

Use it as the initial project-context authority for:

- game identity and design pillars;
- repository layout;
- Godot/Core/Unity migration boundaries;
- system status;
- data authority;
- persistence rules;
- existing factions, characters, locations, quests, expansions, items, and catalogs;
- content gaps;
- integration risks;
- development priorities;
- task-to-context routing;
- known uncertainties.

## Authority order

When optimizing a prompt, use this precedence:

1. **Explicit current user instruction** for the requested task.
2. **Current repository evidence**, if the target agent will have repository access.
3. **Current `AGENTS.md` and repository-local engineering rules**.
4. **Authoritative Core/data/schema/test files**.
5. **`references/skillcontext.md`** as the high-density project map.
6. Current migration/design documentation.
7. Historical plans, archived test reports, legacy README material.
8. Inference.

Never instruct the target model to treat the dossier as more authoritative than the current repository.

If the target is a web LLM without repository access, use `skillcontext.md` as the working project truth but preserve its `OBSERVED`, `INFERRED`, `UNCERTAIN`, and `RECOMMENDED CONTEXT NOTE` distinctions.

---

# 2. ASHFALL Non-Negotiable Project Truths

Unless the current repository explicitly proves otherwise, preserve these constraints in optimized prompts.

## 2.1 Active architecture

- The active migration target is **Godot 4.7+ with C#/.NET**.
- `Assets/Ashfall.Core` is intended to be the engine-agnostic simulation authority.
- `src` is the active Godot host/presentation/integration layer.
- `Assets/_Game` is the large Unity legacy implementation and migration reference.
- `Assets/StreamingAssets/Data` is the intended content/data authority.
- Unity ScriptableObjects are not a second canonical content authority.
- New gameplay rules should not be duplicated inside Godot Nodes or Unity MonoBehaviours when they belong in Core.
- Unity must not be launched unless the user explicitly requests it.

## 2.2 Determinism

Optimized coding prompts should normally require:

- seeded RNG through existing ports;
- stable ordering;
- invariant culture;
- deterministic instance IDs;
- same-seed behavioral tests for random systems;
- avoidance of uncontrolled `System.Random`, `Guid.NewGuid`, Unity randomness, wall-clock dependence, or order-dependent collections in authoritative simulation.

## 2.3 Persistence

Stateful additions should normally require:

- explicit serializable DTO/state;
- `CaptureState()` / `RestoreState()` or the nearest established pattern;
- versioned save envelope when appropriate;
- deterministic ordering for checksums;
- old-version migration/default behavior;
- checksum compatibility;
- dirty-save/event integration;
- round-trip tests;
- reference/ID validation.

## 2.4 Data authority

New authored content should normally:

- reuse existing catalog conventions;
- use existing `snake_case` IDs or discover the authoritative vocabulary before adding IDs;
- follow the nearest schema rather than inventing a parallel format;
- include a typed Core loader/record or reuse an existing one;
- validate required fields and cross-references;
- avoid silent parse failure where possible;
- integrate with runtime selectors/registries rather than exist as orphan JSON.

## 2.5 Host boundary

Use the pattern:

`JSON/catalog → Core rule/state/events → host session/adapter → Godot UI/presentation`

Presentation should stay thin. Domain logic should remain host-neutral when practical.

## 2.6 Integration over replacement

Default preference:

**EXTEND → INTEGRATE → PORT → REFACTOR → REPLACE**

Do not instruct an agent to rebuild a mature system merely because greenfield code would be easier.

Replacement requires evidence that the existing authority is obsolete, broken, contradictory, or architecturally unsalvageable.

---

# 3. ASHFALL Creative Identity Lock

All creative prompts must remain recognizably ASHFALL.

## 3.1 Emotional register

Favor:

- cold;
- exhausted;
- human;
- materially specific;
- restrained;
- morally uncomfortable;
- administrative;
- survival through maintenance;
- scarcity expressed through choices and degraded capacity;
- consequences that remain visible later.

Avoid generic apocalypse spectacle.

## 3.2 Narrative language

ASHFALL repeatedly uses:

- records;
- ledgers;
- radio fragments;
- letters;
- manifests;
- census work;
- standing records;
- duty rosters;
- evidence;
- procedural paperwork;
- technical maintenance logs;
- rationing decisions;
- institutional remnants.

Use these as narrative mechanisms where appropriate, not decorative exposition.

## 3.3 Core creative rule

A strong ASHFALL addition should make the player decide some combination of:

- what to preserve;
- who receives scarce capacity;
- what truth becomes actionable;
- what institution survives;
- which relationship is damaged;
- which future risk is accepted;
- which immediate resource is sacrificed;
- which obligation is created;
- what evidence is believed, hidden, traded, or destroyed.

Content that only gives more combat power, loot, enemies, or spectacle is usually weak ASHFALL expansion unless it is embedded in scarcity, logistics, consequence, and narrative cost.

## 3.4 Creative prohibitions

Unless explicitly overridden by the user and consistent with repository rules:

- no fantasy or magic;
- no superhero/power-fantasy framing;
- no glorified violence;
- no gore-first design;
- no copied names, art, UI, text, characters, mechanics descriptions, or code from other games;
- no direct real-country, real-war, or real-person framing as canon;
- no generic neon/cyberpunk tonal drift;
- no lore invention presented as established canon without evidence.

---

# 4. Skill Activation and Scope Guard

Use this skill only when the task is about ASHFALL.

Examples that should activate it:

- “Optimize this prompt for expanding ASHFALL medicine.”
- “Make this Qwen prompt better for adding a faction questline.”
- “Turn this idea into a Codex prompt that implements it.”
- “Write a Gemini prompt for auditing the Godot UI.”
- “Expand Year of Ash and make the agent wire it into the game.”
- “Prompt optimize the next coding steps.”
- “Make a prompt for Black Flotilla integration.”
- “Create a creative expansion prompt for Kimi.”
- “Make this ASHFALL idea implementation-ready.”

Do not use it for unrelated games or generic coding.

If a user requests another game, do not adapt ASHFALL assumptions to it.

---

# 5. Input Interpretation

The user may provide a complete prompt or only a fragment.

Possible inputs:

- rough idea;
- desired feature;
- bug report;
- prior agent result;
- implementation summary;
- expansion concept;
- quest/faction/location idea;
- UI problem;
- desired target model;
- desired execution environment;
- desired scope or intensity.

Do not require a rigid input form.

Infer missing noncritical fields from context.

## Optional control fields

Recognize natural-language equivalents of:

- **Target model:** Codex / ChatGPT / Claude / Gemini / Qwen / MiniMax / DeepSeek / Kimi / MiMo / GLM / Mistral / model-neutral.
- **Mode:** CODING / CREATIVE / HYBRID / DEBUG / AUDIT / PLAN / VERIFY.
- **Execution permission:** analyze-only / plan-only / modify repository / full autonomous implementation.
- **Expansion scale:** 1–6.
- **Risk tolerance:** conservative / normal / aggressive experimentation.
- **Output style:** concise / standard / exhaustive.

If the user does not specify them, choose sensible defaults from the task.

---

# 6. First Step: Classify the Task

Before rewriting anything, classify the request internally.

A task may belong to multiple classes.

## 6.1 Engineering classes

- repository analysis;
- implementation;
- debugging;
- migration/porting;
- architecture;
- refactoring;
- technical debt;
- optimization/performance;
- data/schema;
- persistence/save;
- testing/verification;
- CI/tooling;
- UI implementation;
- asset integration;
- audio integration.

## 6.2 Creative classes

- worldbuilding;
- lore;
- faction;
- character/NPC;
- questline;
- encounter/event;
- location;
- expansion;
- item/content;
- survival mechanic;
- economy/balance;
- narrative branching;
- epilogue/endgame;
- environmental storytelling;
- radio/journal/records;
- UI/UX concept.

## 6.3 Hybrid classes

Examples:

- new faction + standing system + quests + UI + persistence;
- medical expansion + new treatments + narrative consequences + coding;
- Year of Ash content expansion + Core integration + host panel + save migration;
- location cluster + encounters + loot + travel + journal + faction control;
- new shelter subsystem + resource economy + events + UI + tests.

For a hybrid task, never separate creativity from implementation reality.

---

# 7. Determine the Prompt's Actual Outcome

Identify what the target model must do.

Use one or more of:

- `ANALYZE_ONLY`
- `BRAINSTORM_ONLY`
- `DESIGN_ONLY`
- `PLAN_ONLY`
- `AUDIT`
- `DEBUG`
- `IMPLEMENT`
- `EXPAND`
- `REFACTOR`
- `PORT`
- `VERIFY`
- `ITERATE_AND_REPAIR`

Do not accidentally turn analysis into implementation.

Do not accidentally turn implementation into endless planning.

If the user's language implies “do it,” “wire it,” “implement it,” “fix it,” or “complete it,” the optimized prompt should require actual repository changes and verification rather than returning only a plan.

If the user asks for ideas/brainstorming only, prohibit code modification.

---

# 8. Context Selection

Do not paste the entire blueprint into every optimized prompt.

Use `references/skillcontext.md` to select only the relevant dependency chain.

## 8.1 Always include high-level context when coding

At minimum, carry forward:

- Godot active target;
- Core simulation authority;
- JSON data authority;
- Unity legacy boundary;
- deterministic/save-safe expectations;
- inspect-current-repository-before-editing rule.

## 8.2 Task-specific routing

Use the task-to-context map in `skillcontext.md`.

Examples:

### Medicine / injury

Trace:

- needs/health;
- radiation;
- medical systems;
- inventory and item consumption;
- survivor schedules/work efficiency;
- caregivers;
- morale/trauma;
- trade/loot;
- quests/events;
- UI condition display;
- persistence;
- tests.

### Factions

Trace:

- faction catalogs and aliases;
- standing/reputation;
- trade/prices/access;
- quests;
- encounters;
- radio/journal;
- locations;
- raids/conflict;
- icons/UI;
- save state;
- epilogue.

### New location

Trace:

- base/expansion location catalogs;
- travel time;
- danger;
- radiation;
- loot/resource identity;
- controlling faction;
- encounters;
- quests;
- radio/journal clues;
- stateful changes;
- asset/map presentation;
- persistence.

### Expansion

Trace:

- nearest existing expansion convention;
- Core DTOs/systems;
- JSON catalogs/IDs;
- host session;
- event/tick integration;
- panel/presentation;
- save envelope/migration/checksum;
- tests;
- data integrity;
- assets;
- radio/journal;
- campaign reachability.

---

# 9. Repository Inspection Contract for Coding Prompts

Every implementation-capable optimized prompt should tell the target agent to inspect before editing.

Use the following logic:

1. Read current `AGENTS.md` and local repository instructions.
2. Inspect current Git/worktree status without reverting user changes.
3. Locate the authoritative implementation rather than assuming a stale path.
4. Read the relevant Core system.
5. Read relevant JSON/data schema/catalog.
6. Read host session/panel/adapter.
7. Read save store/envelope if stateful.
8. Read tests and data validation.
9. Search for duplicate/legacy implementation.
10. Identify current semantic differences between Core, Godot, and Unity.
11. Map downstream dependencies.
12. Only then propose or make edits.

The optimized prompt should explicitly say that file paths listed in context are orientation, not permission to edit blindly.

---

# 10. Preserve Concurrent Work

Coding prompts must instruct agents to:

- inspect `git status` or equivalent;
- preserve unrelated user/concurrent edits;
- never reset, checkout, clean, or overwrite unrelated work;
- avoid broad formatting churn;
- keep the diff bounded to the task;
- report pre-existing failures separately from introduced failures.

The blueprint snapshot itself was taken from a changing worktree, so stale line-level assumptions are especially dangerous.

---

# 11. Creative Expansion Engine

Creative expansion optimization is not “generate more content.”

It must create **new playable consequence structures** that fit ASHFALL.

## 11.1 Expansion priorities

Prefer depth in existing underused systems before inventing unrelated new subsystems.

High-value current surfaces include:

- water treatment / Hydro Baron relationships / contamination / market / aquifer consequences;
- medical care linking injury, illness, radiation, caregiving, work efficiency, morale, items, and scarcity;
- faction standing as a real cross-system graph;
- mid/late game event density;
- data-rich industrial, water, archive, medical, and transport locations;
- Year of Ash campaign continuity;
- Black Flotilla integration;
- Century Seed / generational succession clarification/integration;
- endgame/epilogue reachability;
- survivor relationships and named-character arcs;
- radio/journal/evidence as gameplay feedback;
- full Godot daily-loop integration.

Treat these as opportunities, not mandatory roadmap decisions.

## 11.2 Expansion quality dimensions

A strong expansion prompt should ask the target model to maximize:

- mechanical differentiation;
- thematic fit;
- cross-system interaction;
- scarcity pressure;
- consequence persistence;
- meaningful player choice;
- alternate resolutions;
- faction relevance;
- location relevance;
- survivor relevance;
- progression fit;
- replayability;
- state visibility through UI/radio/journal/records;
- implementation feasibility;
- reuse of established content/data patterns;
- long-term ending/epilogue implications where appropriate.

## 11.3 Depth before quantity

Do not optimize toward arbitrary counts such as “50 quests” unless the user explicitly wants volume.

Instead ask:

- What gap is being filled?
- Which current systems gain new interactions?
- What player decisions become possible?
- What resource trade-offs appear?
- What future consequences become visible?
- What content becomes newly reachable?
- What existing faction, location, item, NPC, radio, journal, or ending material can be reused?
- What new content is truly necessary?

## 11.4 Reuse hierarchy for canon

Prefer:

1. existing established entities;
2. existing underused entities;
3. existing planned/uncertain entities after explicit status labeling;
4. new entities that fill a demonstrated gap;
5. new large canon structures only when justified.

Do not multiply faction names while faction identity mapping is unresolved unless the user's task explicitly requires a new faction and the prompt includes canonical mapping work.

---

# 12. Expansion Scale Controller

When the user asks for an expansion but not its magnitude, infer an appropriate level.

## Scale 1 — Micro

A focused enhancement:

- one event;
- one location interaction;
- one UI feedback loop;
- a few items;
- one narrow quality-of-life system.

No new major architecture.

## Scale 2 — Focused subsystem

A bounded subsystem/content expansion with a small dependency chain.

## Scale 3 — Major feature

A meaningful system or narrative package touching several existing systems and requiring persistence/UI/tests.

## Scale 4 — Interconnected expansion

Multiple mechanics + narrative + content + UI + persistence, with a clear campaign role.

## Scale 5 — Major campaign expansion

Large multi-system content package with quests, factions/characters/locations/events, balance, data, Core logic, host integration, save migration, tests, and endgame consequences.

## Scale 6 — Transformative

Game-wide transformation or long campaign layer. Require explicit architecture decomposition, milestones, compatibility strategy, staged integration, and strict prevention of uncontrolled rewrite.

Scale 6 should not mean “edit everything at once.” It should mean a large vision implemented as bounded vertical slices.

---

# 13. Creative-to-Code Translation

When Mode = HYBRID, every major creative idea must be translated into implementation implications.

For each proposed feature, capture as relevant:

- design purpose;
- player-facing behavior;
- existing system reused;
- Core rule/state required;
- JSON/catalog additions;
- IDs and references;
- events emitted/consumed;
- time/tick integration;
- RNG/determinism requirements;
- save state/migration;
- Godot session/adapter;
- UI/presentation;
- radio/journal/evidence feedback;
- assets/audio if necessary;
- tests;
- balance implications;
- campaign/ending flags;
- failure modes;
- acceptance criteria.

This translation step is mandatory for implementation-oriented expansion prompts.

---

# 14. ASHFALL Expansion Architecture Pattern

When creating or extending a formal expansion, prefer the established pattern:

`Core system + DTOs → JSON catalogs and IDs → host session/panel → event/tick integration → versioned save envelope + migration/checksum → tests/data-integrity/self-test → UI/assets/radio/journal`

The optimized prompt should state what is intentionally out of scope.

Never let an expansion exist only as prose or disconnected JSON if the user requested implementation.

---

# 15. High-Risk Areas

When a task touches these, increase prompt rigor and verification depth.

## 15.1 Save systems

Require full state graph, versioning, checksums, migrations, old files, ordering, host compatibility, dirty-save wiring.

## 15.2 Composition roots

`src/Main.cs`, Unity `GameBootstrap`, bridge lifecycle, and broad wiring are high coupling. Require minimal edits and extraction rather than further monolith growth where possible.

## 15.3 Needs/radiation/weather/shelter

Tick order and thresholds alter global survival outcomes. Require behavioral regression tests.

## 15.4 Inventory/item schema

Changes can affect IDs, stacking, equipment, art, crafting, trade, loot, UI, and saves.

## 15.5 Faction identity

Aliases are unresolved. Do not silently invent a mapping or canonize one.

## 15.6 Narrative catalogs

Eligibility, flags, day/location/stance gates, deterministic selection, journal/radio, future branches, and endings must remain coherent.

## 15.7 Time/RNG

Require stable ordering and deterministic stream use.

## 15.8 Legacy-to-Core ports

Parity is not assumed. Compare semantics explicitly.

---

# 16. Coding Prompt Compiler

For CODING, DEBUG, PORT, REFACTOR, or HYBRID modes, build the optimized prompt in this logical order.

## A. Mission

State one unambiguous outcome.

## B. Repository truths

Include only relevant ASHFALL architecture constraints.

## C. Evidence-first preflight

Tell the agent what to inspect before editing.

## D. Current-state audit

Require the agent to establish what already exists, what is partial, what is legacy, what is data-only, and what is uncertain.

## E. Dependency map

Require explicit dependencies and downstream effects.

## F. Implementation strategy

Prefer bounded vertical slices, existing patterns, deterministic Core logic, data authority, and thin host wiring.

## G. Edit boundaries

State whether the agent may modify code/data/tests/UI/assets and whether Unity execution is forbidden.

## H. Verification

Define exact relevant commands and expected invariants.

## I. Repair loop

If verification fails because of the agent's changes, require diagnosis, repair, and rerun rather than stopping at first failure.

## J. Final evidence report

Require changed files, behavior, tests, failures, unverified areas, and follow-up risks.

---

# 17. Creative Prompt Compiler

For CREATIVE/BRAINSTORM/DESIGN modes, use this logical order.

## A. Creative mission

State what aspect of ASHFALL is being expanded.

## B. Canon anchors

Specify relevant existing factions, characters, locations, systems, expansions, timeline, and design pillars.

## C. Gap to solve

Identify why expansion is useful.

## D. Desired player experience

Define the emotional and strategic pressure.

## E. Cross-system hooks

Require multiple meaningful interactions rather than isolated lore.

## F. Consequence design

Require immediate, delayed, and persistent consequences where relevant.

## G. Content architecture

Specify expected quest/event/location/NPC/item/radio/journal outputs only as necessary.

## H. Canon discipline

Label new content as proposed; preserve uncertainty in existing content.

## I. Implementation-awareness

If no coding is requested, still identify likely systems/data touched so future implementation can be planned.

## J. Quality gate

Reject filler, repetitive morality choices, lore dumps, arbitrary counts, or spectacle without resource/consequence integration.

---

# 18. Hybrid Prompt Compiler

For HYBRID tasks, optimize the target prompt into phases.

## Phase 0 — Inspect and establish truth

- read `AGENTS.md`;
- inspect current implementation/data/tests;
- identify user/concurrent edits;
- verify task-relevant blueprint assumptions.

## Phase 1 — Design expansion

- identify gap;
- generate ASHFALL-specific mechanics/narrative;
- connect to existing systems;
- define player decisions and consequences;
- reuse established content where strong.

## Phase 2 — Canon/data model

- determine IDs;
- schemas;
- catalog changes;
- flags;
- state machines;
- save implications;
- ending implications.

## Phase 3 — Core implementation

- implement simulation in Core;
- deterministic behavior;
- explicit state/events;
- no host-specific domain logic.

## Phase 4 — Host/presentation integration

- session/adapter;
- Godot panel/UI;
- journal/radio/feedback;
- assets/audio only where needed.

## Phase 5 — Persistence and migration

- state DTOs;
- versioning;
- checksums;
- old-version defaults;
- dirty-save integration.

## Phase 6 — Verification

- tests;
- data integrity;
- deterministic same-seed behavior;
- build;
- targeted Godot headless/self-test;
- relevant runtime smoke behavior.

## Phase 7 — Final report

- implementation summary;
- creative content summary;
- files changed;
- tests run;
- PASS/FAIL;
- pre-existing failures;
- remaining uncertainty;
- next highest-value integration step.

---

# 19. Debugging Optimizer

When the user wants debugging, do not optimize into “rewrite the system.”

Require:

1. reproduce or inspect the failure;
2. gather logs/error text/test failure;
3. identify authoritative state owner;
4. trace lifecycle/tick/events/save/data dependencies;
5. compare Core/Godot/legacy semantics if relevant;
6. form ranked hypotheses;
7. make the smallest evidence-backed fix;
8. add or strengthen regression coverage;
9. rerun the narrow test first;
10. run broader relevant verification;
11. report root cause separately from symptom.

For stale or unreproducible failures, require the agent to state that rather than fabricate a root cause.

---

# 20. Refactoring Optimizer

For refactoring requests:

- preserve behavior first;
- define invariants before edits;
- add characterization tests where behavior is poorly covered;
- reduce duplicate authorities;
- move domain logic toward Core;
- keep Godot host thin;
- avoid broad rewrite;
- preserve saves and IDs;
- compare behavior before/after;
- separate structural changes from new features whenever practical.

If the user asks to expand a feature and refactor it, sequence the prompt so the agent first establishes behavior and architecture, then performs the minimal refactor necessary to support the expansion.

---

# 21. Migration/Port Optimizer

For Unity → Core/Godot migration prompts:

1. Treat Unity as behavioral reference, not future architecture.
2. Identify the legacy source of behavior.
3. Identify existing Core partial port.
4. Document semantic differences.
5. Freeze expected behavior with parity/characterization tests.
6. Port simulation rules to Core using ports/events/state.
7. Add deterministic behavior.
8. Add Core tests.
9. Add thin Godot adapter/session/UI.
10. Add save migration/compatibility if stateful.
11. Reduce bridge dependency.
12. Do not launch Unity unless explicitly requested.

A successful bridge compile is not equivalent to a proper migration.

---

# 22. Narrative / Quest Optimizer

For questlines and events, require enough structure to make them gameplay content rather than text blobs.

Each major event/quest beat should define as relevant:

- trigger;
- eligibility;
- day/phase gate;
- location;
- actors/faction;
- prerequisite flags/state;
- player information available at decision time;
- choices;
- resource/state deltas;
- immediate consequence;
- delayed consequence;
- standing/relationship effect;
- journal/radio/evidence feedback;
- future eligibility;
- failure/fallback state;
- persistence;
- ending/epilogue hooks;
- tests/validation if implemented.

Avoid repeated binary “good vs evil” choices. Prefer competing defensible needs under uncertainty.

---

# 23. Faction Optimizer

Because faction identity is currently fragmented, faction prompts must be conservative.

Before adding or extending a faction:

- inspect existing faction IDs and aliases;
- inspect character, quest, radio, icon, location, save, standing, economy, and war references;
- determine whether the task concerns an existing faction under another namespace;
- avoid silent canonical alias decisions;
- if a new faction is genuinely needed, define its systemic role rather than only ideology/lore.

A strong faction expansion connects standing to multiple systems such as:

- access;
- water/resources;
- prices/trade;
- quests;
- encounters;
- shelter pressure;
- survivor relationships;
- evidence/records;
- radio/journal;
- infrastructure;
- raids/defense;
- endings.

---

# 24. Location Optimizer

A strong ASHFALL location should normally have:

- material identity;
- environmental condition;
- controlling/contesting faction where relevant;
- resource identity;
- travel cost;
- radiation/danger;
- unique mechanical pressure;
- encounter pool;
- quest hooks;
- evidence/radio/journal clues;
- stateful changes after player intervention;
- future return variation;
- loot that reflects place rather than generic tables;
- implementation/data hooks if coding is requested.

Prefer expanding underused existing industrial, water, archive, medical, civic, and transit locations before creating dozens of disconnected map nodes.

---

# 25. Survivor / Character Optimizer

For named characters or survivor systems, trace:

- canonical record/archetype;
- profession/skills;
- traits;
- needs;
- injury/illness/radiation;
- work/task eligibility;
- inventory/equipment;
- relationships;
- faction links;
- personal quest/event hooks;
- portrait/UI;
- save state;
- epilogue potential.

Characters should create systemic or narrative pressure, not exist only as dialogue dispensers.

---

# 26. System Expansion Optimizer

For mechanics such as medicine, shelter, weather, expeditions, greenhouse, economy, radiation, or Utility AI:

1. Audit current Core and legacy behavior.
2. Identify the design gap.
3. Preserve existing semantics unless intentionally changed.
4. Define new state variables and ownership.
5. Define event flow.
6. Define interactions with at least the relevant dependency chain.
7. Define data requirements.
8. Define persistence.
9. Define UI feedback.
10. Define balance knobs.
11. Define tests.
12. Define migration/parity implications.

Do not create a parallel “v2” system without proving why extension is insufficient.

---

# 27. UI/UX Prompt Optimizer

ASHFALL UI prompts should distinguish:

- UX/information architecture;
- visual styling;
- host implementation;
- Core state ownership;
- assets;
- input/accessibility;
- persistence refresh.

When coding UI:

- inspect current Godot panel/helper/theme patterns;
- avoid placing domain rules in `Main.cs` or panels;
- prefer reusable state-driven panels;
- preserve near-black / charcoal / rust / amber visual language unless current design sources say otherwise;
- make critical states readable without color alone;
- include tooltips/feedback/failure states;
- verify asset IDs and missing-asset fallback.

---

# 28. Balance Optimizer

Never rebalance from catalog counts alone.

Require analysis of:

- production/source rate;
- consumption/sinks;
- prices;
- loot probability;
- travel time;
- labor opportunity cost;
- treatment time;
- fuel/heat/water demand;
- faction access;
- day/phase gates;
- difficulty modifiers;
- save/retry behavior;
- cross-system feedback loops.

When creative expansion adds resources, items, treatments, or loot, require an economy impact note.

---

# 29. Target-Model Adaptation

After the task specification is correct, adapt its presentation to the target model.

Read `references/model-adapters.md` when a target model is named.

Do not distort ASHFALL requirements to suit a model.

Model adaptation may change:

- section density;
- degree of explicit decomposition;
- milestone structure;
- prompt length;
- redundancy level;
- tool-use instructions;
- review loops;
- output schema.

It must not change:

- architecture constraints;
- canon constraints;
- data authority;
- save/determinism requirements;
- scope;
- acceptance criteria.

---

# 30. Model Capacity Adaptation

Infer whether the target is a high-capability/long-context model or a faster/lower-cost model.

## High-capability / Max / Pro / reasoning model

May receive:

- broader dependency analysis;
- multiple workstreams;
- longer context;
- autonomous audit + design + implementation + verify loops;
- stronger synthesis requirements.

## Plus / balanced model

Use:

- explicit phases;
- bounded workstreams;
- concise but complete context;
- fewer simultaneous architecture changes;
- strong acceptance criteria.

## Flash / Small / fast model

Prefer:

- one bounded task;
- deterministic numbered steps;
- exact file/domain targets after preflight;
- minimal optional brainstorming;
- explicit output schema;
- short verification loop;
- multiple separate passes rather than one giant prompt.

Do not ask a lightweight model to redesign half the game in one pass.

---

# 31. Prompt Output Contract

By default, output **one polished optimized prompt** that is ready to paste into the target LLM.

Do not execute the game-development task unless the user asks you to do that too.

Do not bury the final prompt under a long essay.

If useful, a short preface may state:

- inferred mode;
- target model;
- expansion scale;
- key optimization choices.

Then provide the prompt.

If the user says “only prompt,” output only the optimized prompt.

---

# 32. Default Optimized Prompt Structure — Coding

Use this structure when appropriate; adapt, do not mechanically fill every heading.

```markdown
# ASHFALL — [TASK TITLE]

## Mission
[One precise outcome.]

## Execution Mode
[Analyze / implement / debug / refactor / port / verify.]

## ASHFALL Architecture Constraints
[Relevant Core/Godot/data/legacy/determinism/save rules.]

## Evidence-First Preflight
[What to inspect before edits.]

## Current-State Audit
[What must be established from repository reality.]

## Dependency Chain
[Systems/data/UI/save/tests to trace.]

## Required Work
### 1. ...
### 2. ...

## Creative / Design Requirements
[Only if applicable.]

## Engineering Requirements
[State ownership, events, data, host, save, determinism, migration.]

## Edit Boundaries
[Allowed/prohibited changes.]

## Acceptance Criteria
[Observable completion conditions.]

## Verification
[Targeted tests/build/headless probes.]

## Repair Loop
[Fix introduced failures and rerun.]

## Final Response Contract
[Changed files, behavior, tests, PASS/FAIL, uncertainty, next step.]
```

---

# 33. Default Optimized Prompt Structure — Creative Expansion

```markdown
# ASHFALL — [EXPANSION / CREATIVE TASK]

## Creative Mission
[What gap is being expanded and why.]

## Canon Anchors
[Relevant existing content and status.]

## ASHFALL Design Pillars
[Scarcity, maintenance, records, consequence, restraint.]

## Player Experience Target
[Emotional + strategic pressure.]

## Expansion Scope
[Scale and boundaries.]

## Existing Content to Reuse
[Factions, characters, locations, systems, items, records, radio, etc.]

## New Content Only Where Needed
[Justified additions.]

## Cross-System Design
[Mechanic/narrative dependency chains.]

## Consequence Architecture
[Immediate, delayed, persistent.]

## Quest / Event / Location / Character Requirements
[Task-specific.]

## Progression and Balance
[Where it sits in the campaign.]

## Canon Safety
[What is observed vs proposed vs uncertain.]

## Implementation Awareness
[Likely Core/data/host/save/UI/test impact even if not coding yet.]

## Deliverables
[Exact outputs.]

## Quality Gate
[Reject filler, repetition, generic apocalypse, disconnected lore.]
```

---

# 34. Default Optimized Prompt Structure — Hybrid Expansion + Coding

```markdown
# ASHFALL — [FEATURE / EXPANSION] — DESIGN + IMPLEMENTATION

## Mission
Design and implement [feature] as a native ASHFALL vertical slice.

## Non-Negotiable Project Constraints
[Relevant architecture/canon/data/save/determinism rules.]

## Phase 0 — Repository Truth Audit
[Inspect AGENTS, current source/data/tests, dirty worktree, duplicate authorities.]

## Phase 1 — Creative Expansion Design
[Gap, player pressure, choices, consequences, reuse, new content.]

## Phase 2 — System and Data Architecture
[State, IDs, schema, flags, events, RNG, persistence, campaign hooks.]

## Phase 3 — Core Implementation
[Host-neutral simulation.]

## Phase 4 — Godot Host / UI Integration
[Thin adapter and player-facing feedback.]

## Phase 5 — Persistence / Migration
[Versioning/checksum/round-trip.]

## Phase 6 — Content Integration
[Quests/events/radio/journal/items/locations/assets as relevant.]

## Phase 7 — Verification and Repair
[Tests/build/data integrity/headless/self-test.]

## Acceptance Criteria
[Functional + creative + architectural + verification.]

## Final Report
[Evidence-based summary.]
```

---

# 35. Verification Command Library

When relevant and still valid in the current repository, optimized coding prompts may ask agents to run:

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expansion-suite
```

Do not blindly require every command for every task.

Before running, verify the current repository instructions and command availability.

Never cite historical test counts as proof of current success.

---

# 36. Acceptance Criteria Generator

Every optimized implementation prompt must have concrete completion criteria.

Choose relevant criteria such as:

- authoritative implementation located and documented;
- no duplicate domain authority introduced;
- new state lives in Core where appropriate;
- existing IDs reused or validated;
- JSON loads successfully;
- missing references fail clearly;
- deterministic same-seed behavior passes;
- save round trip passes;
- old-version state migrates correctly;
- UI reflects state changes;
- journal/radio/evidence feedback is reachable;
- no unrelated files modified;
- target build passes;
- targeted tests pass;
- broader relevant suite passes;
- introduced warnings/errors resolved;
- user/concurrent work preserved;
- unresolved uncertainties explicitly reported.

Creative acceptance criteria can include:

- all content fits ASHFALL tone;
- no disconnected lore;
- no arbitrary filler counts;
- player choices have competing costs;
- existing underused content is leveraged;
- cross-system consequences are explicit;
- progression placement is justified;
- proposed canon is labeled as proposed;
- implementation implications are identified.

---

# 37. Prompt Quality Audit

Before outputting the optimized prompt, silently check:

## Intent

- Does it solve the user's actual request?
- Is execution permission correct?
- Is the target model correct?

## ASHFALL specificity

- Could this prompt be pasted into another survival game unchanged?
- If yes, it is not specific enough.

## Architecture

- Does it preserve Core/data/Godot/legacy boundaries?
- Does it avoid duplicate authority?

## Creativity

- Does expansion create choices, scarcity, consequence, and systemic interaction?
- Does it reuse relevant existing content?

## Coding

- Does it require inspection before editing?
- Are dependencies explicit?
- Are save/determinism/IDs considered?

## Verification

- Are acceptance criteria testable?
- Is there a repair loop?

## Uncertainty

- Does it avoid turning `UNCERTAIN` or `INFERRED` blueprint material into fact?

## Scope

- Is the task bounded enough for the target model?
- If massive, is it decomposed into vertical slices rather than one uncontrolled rewrite?

Only output the prompt after these checks pass.

---

# 38. Anti-Patterns to Remove During Optimization

Rewrite or eliminate prompts that contain:

- “make it better” without defining better;
- “expand massively” with no design target;
- arbitrary content counts with no systemic purpose;
- file-edit instructions based on stale assumptions;
- “rewrite the whole system” without audit;
- direct edits to Godot UI that duplicate simulation rules;
- new JSON without loader/validation/runtime integration;
- new state without save/versioning considerations;
- uncontrolled random behavior;
- new faction IDs without alias/canon audit;
- new quests with no eligibility or consequence model;
- lore presented as canon without evidence;
- giant plans with no implementation permission clarity;
- implementation prompts with no verification;
- “tests passed” claims based on archived reports;
- instructions to reset unrelated repository changes;
- Unity execution without explicit user permission.

---

# 39. Handling Contradictions

If current context contains conflicting authorities:

1. Identify the conflict.
2. Prefer current source/data over stale prose.
3. Do not silently canonize one faction alias or semantic behavior if both are active.
4. Preserve compatibility where possible.
5. Choose the safest reversible path when a decision is not blocking.
6. If an architectural/canon choice is truly necessary before safe implementation, instruct the target agent to surface the decision clearly rather than invent it.

Prompt optimization should reduce ambiguity, not conceal it.

---

# 40. Development-Priority Awareness

When the user asks “what next?” or gives a vague improvement request, use the blueprint's candidate priorities as signals, not mandates.

Consider:

- deterministic RNG/stable IDs;
- duplicate clocks/event buses/WornGear ownership;
- catalog validation failures;
- canonical faction ID mapping;
- extraction from `Main.cs` and other monoliths;
- bounded Unity → Core/Godot ports;
- save migration/round-trip coverage;
- active Godot CI alignment;
- water/radiation/shelter/medicine/work/market/faction integration;
- bounded playable Godot day loop;
- expedition semantic parity;
- richer mid/late event density;
- named-character arcs;
- Black Flotilla / Century Seed / endgame integration;
- state-driven UI;
- item-art coverage;
- Godot audio routing;
- localization/accessibility decisions.

Prioritize based on the user's stated goal and current repository evidence.

---

# 41. Special Rule: “Expand Everything” Requests

If the user requests an enormous game-wide expansion, do not produce a prompt that tells one agent to edit the whole repository in one pass.

Compile it into:

1. repository audit;
2. expansion thesis;
3. dependency graph;
4. prioritized workstreams;
5. vertical slices;
6. implementation order;
7. per-slice acceptance criteria;
8. integration checkpoints;
9. save/data compatibility checkpoints;
10. final cross-system regression pass.

The target agent may work autonomously across slices, but each slice must reach a verified stable state before the next high-risk layer.

---

# 42. Special Rule: Prior Agent Result / Continuation

If the user pastes a prior agent completion summary and asks for the next prompt:

- treat the summary as a claim, not proof;
- preserve completed work unless current repository evidence contradicts it;
- ask the target model to verify claimed files/tests before extending;
- identify the next dependency-safe frontier;
- avoid redoing completed phases;
- convert unresolved caveats into explicit next-step checks;
- maintain continuity with existing expansion architecture and canon.

---

# 43. Special Rule: Web LLM Without Repository Tools

When generating a prompt for a web LLM that cannot inspect the local repository:

- provide the relevant blueprint context inside the prompt;
- state which facts are `OBSERVED`, `INFERRED`, or `UNCERTAIN` where it matters;
- ask for design/analysis rather than pretending to implement local files;
- request implementation artifacts such as schemas, pseudocode, file-change plans, test matrices, or content packs only if useful;
- do not claim repository verification.

If the web LLM can accept uploaded files, instruct the user-facing prompt to use `skillcontext.md` and any relevant current files as source material.

---

# 44. Special Rule: Local Coding Agent

When the target has repository/tool access:

- use the blueprint as orientation;
- make the agent inspect current files;
- require autonomous evidence gathering;
- allow bounded implementation if requested;
- require tests/build/probes;
- require a final evidence report;
- avoid stuffing the full blueprint into the prompt if the agent can read it locally.

---

# 45. Model-Agnostic Fallback

If no target model is specified, create a model-neutral prompt optimized for a capable reasoning/coding LLM.

Use:

- clear hierarchy;
- evidence-first instructions;
- explicit constraints;
- phased execution;
- acceptance criteria;
- verification;
- concise final report contract.

Do not overfit syntax to any vendor.

---

# 46. Minimal User-Facing Behavior

When invoked, the skill should normally:

1. understand the rough ASHFALL request;
2. infer task type and target model;
3. consult relevant `skillcontext.md` sections;
4. compile the prompt;
5. run the quality audit;
6. output the paste-ready optimized prompt.

Do not overwhelm the user with the optimizer's internal decomposition unless they ask for it.

---

# 47. Final Rule

The purpose of every output is to make the receiving LLM act like it understands **this repository, this migration, this canon, this design language, and this development state**.

A successful optimized prompt should reduce the chance of:

- generic survival-game content;
- disconnected lore;
- duplicate systems;
- host-specific domain logic;
- broken saves;
- nondeterminism;
- stale-path edits;
- faction-ID drift;
- unverified implementation;
- huge uncontrolled rewrites;
- creative additions that never become playable consequences.

When in doubt, optimize for **ASHFALL-specific depth, integration, evidence, consequence, and verifiability**.
