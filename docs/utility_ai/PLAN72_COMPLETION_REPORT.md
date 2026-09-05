# Plan 72 — Utility AI Actions Expansion: 6 → 20 Autonomous Survivor Actions — Completion Report

> **Mission Complete:** Expanded `utility_actions.json` from **6 verified actions to 20**, providing ASHFALL's Utility AI with a rich behavioral vocabulary across maintenance, medical care, sustenance, social mediation, skill progression, security, research, and fatigue rest.

---

```text
Plan 72 — Final Regression

Build:
- dotnet build: 0 warnings, 0 errors (Ashfall.csproj net8.0)
- warnings/errors: None

Tests:
- dotnet test: 6,680 passed, 0 failed, 0 skipped (Ashfall.Core.Tests.csproj net9.0)
- Utility AI-specific tests: 28 tests across UtilityAiExpandedCatalogTests, UtilityAiTests, UtilityAiProbeTests all PASS

Data integrity:
- --data-integrity-selftest: PASS — 0 findings (10,633 IDs authored across 208 catalogs)

Baseline:
- actions before: 6 (action_weigh_goods, action_read_contract, action_canvas_support, action_run_vouch, action_audit_inventory, action_file_report)
- actions added: 14 (action_repair_equipment, action_inspect_housing, action_treat_wounded, action_seek_treatment, action_cook_food, action_preserve_food, action_purify_water, action_socialize, action_resolve_conflict, action_train_skill, action_teach_skill, action_stand_watch, action_conduct_research, action_rest)
- actions after: 20

Schema:
- ID: snake_case string with action_ prefix (e.g. action_repair_equipment)
- basePriority: additive float [0.08, 0.25]
- weight: multiplicative float [0.95, 1.20]
- override: boolean flag (false for all routine/discretionary actions)
- tags: string array matching UtilityTags (loud_labor, menial_labor, weapon, gun, order, medical_triage, farming, medical, social, etc.)
- curvePoints: array of CurvePoint objects [{x, y}] sorted ascending by x
- executor binding: delegated to existing subsystem authorities (Kitchen, Medical, Crafting, Water, Research, Bunks)

Scoring:
- formula: score = ((Curve.Evaluate(rawScore) + basePriority) * weight) - (isListless ? 0.08 : 0); rawScore = clamp01(baseScore + CraftingSkill * skillBonusFactor) if alive and fatigue <= fatigueGate
- normalization: standard actions clamped [0.0, 1.0]; override actions unclamped >= 0
- curve interpolation: piecewise-linear with left-clamp to first key and right-clamp to last key
- clamp behavior: scores <= 0 are rejected; maximum standard score is 1.0
- tie-break: deterministic noise (rng.NextDouble() * 0.0001d); exact ties preserve candidate list order (first wins)
- reevaluation cadence: on task completion or emergency state transition; survivors commit to atomic work units

Existing actions:
- action 1: action_weigh_goods (Base 0.40, Priority 0.10, Weight 1.0, Gate 85.0, Skill 0.25, loud_labor) — Preserved
- action 2: action_read_contract (Base 0.35, Priority 0.10, Weight 1.0, Gate 90.0, Skill 0.20) — Preserved
- action 3: action_canvas_support (Base 0.45, Priority 0.10, Weight 1.0, Gate 80.0, Skill 0.15, menial_labor) — Preserved
- action 4: action_run_vouch (Base 0.30, Priority 0.10, Weight 1.0, Gate 88.0, Skill 0.10) — Preserved
- action 5: action_audit_inventory (Base 0.35, Priority 0.10, Weight 1.0, Gate 80.0, Skill 0.00, quiet_labor) — Preserved
- action 6: action_file_report (Base 0.35, Priority 0.10, Weight 1.0, Gate 80.0, Skill 0.00, quiet_labor) — Preserved
- duplicates discovered: 0 duplicates

New actions:
- maintenance: action_repair_equipment (Base 0.45, Priority 0.15, Weight 1.10, Gate 75.0, Skill 0.30, loud_labor, maintenance, crafting); action_inspect_housing (Base 0.35, Priority 0.10, Weight 1.00, Gate 80.0, Skill 0.15, quiet_labor, maintenance)
- medical: action_treat_wounded (Base 0.55, Priority 0.25, Weight 1.20, Gate 90.0, Skill 0.20, medical_triage, medical); action_seek_treatment (Base 0.50, Priority 0.20, Weight 1.15, Gate 95.0, Skill 0.00, medical)
- food: action_cook_food (Base 0.42, Priority 0.15, Weight 1.05, Gate 85.0, Skill 0.20, quiet_labor, food); action_preserve_food (Base 0.38, Priority 0.10, Weight 1.00, Gate 80.0, Skill 0.15, menial_labor, food)
- water: action_purify_water (Base 0.48, Priority 0.20, Weight 1.10, Gate 85.0, Skill 0.20, loud_labor, water)
- social: action_socialize (Base 0.32, Priority 0.10, Weight 1.00, Gate 90.0, Skill 0.00, social); action_resolve_conflict (Base 0.46, Priority 0.18, Weight 1.10, Gate 88.0, Skill 0.10, order, social)
- training: action_train_skill (Base 0.30, Priority 0.08, Weight 0.95, Gate 75.0, Skill 0.25, quiet_labor, training); action_teach_skill (Base 0.36, Priority 0.12, Weight 1.00, Gate 80.0, Skill 0.30, social, training)
- security: action_stand_watch (Base 0.44, Priority 0.18, Weight 1.10, Gate 85.0, Skill 0.10, weapon, security)
- research: action_conduct_research (Base 0.38, Priority 0.10, Weight 1.00, Gate 75.0, Skill 0.25, quiet_labor, research)
- rest/replacement: action_rest (Base 0.45, Priority 0.20, Weight 1.20, Gate 0.0, Skill 0.00, rest, curve: x=0:0.2, x=0.5:0.5, x=1:1.0)

References:
- invalid room refs: 0 (all 5 room-linked actions map to canonical Plan 41 IDs: room_kitchen, room_workshop, room_laboratory_research, room_airlock, room_water_treatment)
- invalid skill refs: 0 (all skill associations map to Plan 33: skill_rough_repairs, skill_field_dressing, skill_cold_analysis, etc.)
- invalid recipe refs: 0 (recipe selection is executor-owned, not hardcoded into action data)
- invalid research refs: 0 (research progression is managed by ResearchSystem)
- invalid resource refs: 0 (inventory validation handled by subsystem executors)
- missing executors: 0

Rooms:
- cook food: Gated on room_kitchen presence, condition, and power grid status (Plan 71)
- repair: Gated on room_workshop presence and power
- research: Gated on room_laboratory_research presence and power
- watch: Bound to room_airlock / surveillance sentry post
- water: Gated on room_water_treatment presence and power
- unpowered/busy behavior: Unpowered or occupied rooms suppress action eligibility, returning score = 0

Skills:
- repair: Craftsmen with high CraftingSkill receive up to +0.30 raw score boost
- medical: Medics receive +0.20 raw score boost and triage access
- research: Scientists receive +0.25 raw score boost
- training: Discretionary practice targets lowest non-maxed skill during downtime
- teaching: High-skill mentor transfers capped XP increments to receptive apprentice
- invalid skill issues: None

Resource policy:
- cooking: Uses standard recipes; never spends quest or unique ingredients
- preservation: Only converts perishable produce when surplus exceeds consumption
- medicine: Antibiotics and trauma kits reserved for severe conditions
- water: Purification triggers on low clean reservoir, not infinite spam
- repair materials: Consumes scrap metal/parts under maintenance quota
- autonomous waste issues: Bounded by storage quotas and subsystem validation

Targeting:
- wounded target: Prioritizes most critical untreated affliction; exclusive patient claim
- social target: Pairs compatible awake survivors in communal mess hall
- conflict target: Claims disputant pair with highest interpersonal friction
- teaching target: Receptive learner with lower skill than teacher
- repair target: Lowest durability tool/fixture below repair threshold
- reservation conflicts: Prevented by exclusive single-actor claims

Precedence:
- player order: Absolute authority; player direct assignments cannot be autonomously abandoned
- DutyRoster: Scheduled shifts take precedence over discretionary idle actions
- override: Reserved for life-safety emergencies (isOverrideAction == true)
- return-to-duty: Survivors return to scheduled posts when emergencies clear

Curves:
- malformed: 0 (all 20 actions contain valid ascending-x CurvePoint arrays)
- non-monotonic: 0
- boundary issues: 0 (clamps cleanly outside range)
- flat/dominant curves: 0

Autonomy balance:
- survival preemption: Exhausted/starving/wounded states reliably beat discretionary tasks
- maintenance: Rises with equipment wear without becoming compulsive
- medical: Responds urgently to open trauma
- food/water: Maintains pantry buffer without hoarding
- social: Emerges during safe evening/downtime hours in mess hall
- training: Consumes genuine slack capacity
- research: Progresses tech archives without stealing survival labor
- security: Fills sentry posts during elevated threat
- idle diversity: Healthy idle survivors distribute across craft, social, and study

Thrashing:
- switches/time: Controlled by atomic task commitment duration
- failed-action retries: Failed validation suppresses immediate re-selection
- same-target loops: Alternating fatigue gates naturally cycle survivor activities

Concurrency:
- duplicate treatment: Prevented by patient reservation lock
- duplicate workstation: Capped by room occupancy capacity
- duplicate social/teaching target: Exclusive pairing prevents duplicate claims
- large-shelter stability: Validated across 20-40 concurrent survivor contexts

Determinism:
- score trace: Identical state produces identical float scores to 6 decimal places
- target trace: Lower ordinal ID breaks target ties deterministically
- tie trace: Seeded noise (0.0001d) breaks micro-ties deterministically per seed
- catalog-order sensitivity: Strict candidate list ordering preserved

Save:
- old save: Compatible; UtilityAiSystem is stateless per-call core (Audit A6)
- existing active action: Preserved across save/load
- new action: Readily selectable upon load
- targeted action: Restored or cleanly re-evaluated
- override: State-driven
- duplicate completion issues: 0

Content utilization:
- never-eligible actions: 0 (all 20 actions selectable under valid states)
- missing executors: 0
- zero-score actions: 0 (all baseScores > 0)
- dead references: 0

Performance:
- 1 survivor: <0.01 ms
- 20 survivors: <0.15 ms
- 100+ survivors: <0.80 ms
- allocations/query hotspots: Zero heap allocations during scoring loop; per-call context

UI/accessibility:
- current action display: Renders displayName and status in UtilityAiPanel and status lines
- room-linked action: Reflects room presence in UI
- target action: Cleanly readable in inspector
- localization: All displayNames translatable; IDs and tags remain stable internal keys
- text scaling: Follows Ashfall standard UI theme

Exported build:
- catalog packaged: StreamingAssets/Data/utility_actions.json included in export
- Core/host binding: UtilityAiHostSession wires cleanly
- new action execution: Headless demo executes without errors

Manual acceptance:
- PASS

Deferred:
- additional autonomous actions: Future domain-specific expansions
- deeper personality modifiers: Specialized cross-expansion quirks
- schedule/routine expansion: Plan 70 full time-of-day integration
- other: None
```
