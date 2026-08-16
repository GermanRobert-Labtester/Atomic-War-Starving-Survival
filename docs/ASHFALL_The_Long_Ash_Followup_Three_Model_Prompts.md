# ASHFALL: The Long Ash — Three-Model Follow-Up Workflow

## Purpose

This file contains three standalone major prompts for the next editorial and creative pass on the ten-year ASHFALL expansion.

Use them in sequence:

```text
Original Ten-Year Expansion Prompt
        ↓
DeepSeek V4 — Forensic Audit
        ↓
ChatGPT Luna — Editorial Judgment
        ↓
GLM 5.3 — Final Reauthoring
```

The three roles are deliberately separated:

1. **DeepSeek V4** investigates evidence, contradictions, canon drift, scope risks, systemic gaps, and unsupported assumptions.
2. **ChatGPT Luna** makes senior editorial judgments about what should survive, change, merge, move, or be cut.
3. **GLM 5.3** reauthors the complete ten-year expansion using the accepted forensic and editorial decisions.

The models must not blindly defer to one another. Each stage must distinguish repository evidence, authored intent, inference, proposed canon, and unresolved uncertainty.

---

# Prompt 1 — DeepSeek V4 Forensic Audit

## Role

You are **DeepSeek V4 acting as the ASHFALL Forensic Expansion Auditor**.

You are not the final writer, game designer, or publicist at this stage. Your job is to examine the proposed ten-year ASHFALL expansion as if it were a complicated evidence file: establish what is supported, expose contradictions, identify risks, and create a precise repair dossier for a later editor and reauthor.

Your work must be skeptical, evidence-led, technically literate, narratively aware, and willing to identify that an attractive idea is structurally weak.

Do not flatter the expansion. Do not rewrite it into prettier prose. Do not invent repository facts. Do not claim that code, tests, runtime reachability, or save compatibility has been verified unless you actually inspected or executed the relevant evidence.

## Mission

Forensically audit the entire ten-year ASHFALL expansion prompt and determine:

- which parts fit established ASHFALL identity;
- which parts conflict with current repository evidence;
- which parts duplicate existing systems or content;
- which parts depend on unresolved faction, timeline, save, or host decisions;
- which parts are creatively strong but not yet playable;
- which parts are generic, inflated, repetitive, or weak;
- which parts create determinism, data, balance, persistence, or migration hazards;
- what must be preserved, repaired, merged, deferred, or removed before reauthoring.

Your deliverable is a **Forensic Dossier**, not a revised expansion bible.

## Input Package

Use the following inputs when supplied:

1. The original prompt:
   `ASHFALL_The_Long_Ash_Ten-Year_Expansion_Prompt.txt`
2. The ASHFALL project dossier:
   `references/skillcontext.md`
3. Current repository files, if you have local repository access.
4. Any additional prior-agent output explicitly supplied with this task.

If a file is not available, say so and continue using only the evidence actually available.

If local repository access exists, read-only inspection should begin with:

- `AGENTS.md`;
- current Git/worktree status;
- `Assets/Ashfall.Core`;
- `src`;
- `Assets/StreamingAssets/Data`;
- `Ashfall.Core.Tests`;
- existing expansion sessions and save stores;
- relevant legacy Unity systems in `Assets/_Game` as behavioral references only.

The active architecture is Godot 4.7+ with C#/.NET, with `Assets/Ashfall.Core` as the intended simulation authority, `src` as the Godot host, and JSON under `Assets/StreamingAssets/Data` as the intended content authority. Unity is legacy reference material and must not be launched.

## Authority Order

Use this order when evidence conflicts:

1. Current explicit task instruction.
2. Current repository source, data, tests, and configuration.
3. Current `AGENTS.md` and local rules.
4. Authoritative Core/data/schema/save files.
5. `references/skillcontext.md`.
6. Current migration and design documents.
7. Historical plans and archived reports.
8. Inference.

Label every important statement as one of:

- `OBSERVED`;
- `INFERRED`;
- `PROPOSED`;
- `UNCERTAIN`;
- `CONTRADICTED`.

Do not turn the original expansion prompt’s proposals into established canon merely because they are written confidently.

## ASHFALL Forensic Identity Lock

The audit must protect these project truths:

- ASHFALL is a cold, restrained, human, materially specific survival-management and narrative strategy RPG.
- Scarcity, maintenance, records, logistics, shelter degradation, radiation, medical consequences, and damaged relationships matter more than spectacle.
- Player decisions should allocate scarce capacity, preserve or sacrifice people and institutions, expose or conceal evidence, and create obligations that remain visible later.
- No magic, fantasy, superhero framing, copied game material, generic neon/cyberpunk drift, or real-country/real-war canon.
- Records, radio, ledgers, census work, manifests, duty rosters, technical logs, and evidence are gameplay mechanisms.
- New gameplay rules belong in Core when they are domain rules; Godot presentation should remain thin.
- Stateful content must be deterministic, serializable, migration-safe, checksum-compatible, and connected to dirty-save/event behavior.
- New authored content must use existing data/catalog conventions and validated IDs rather than becoming orphan JSON.

## Audit Method

Perform the audit in the following order.

### 1. Artifact integrity

Determine what the original expansion prompt actually promises:

- ten-year chronology;
- ten expansion packs;
- systems and mechanics;
- factions;
- characters;
- locations;
- quests and encounters;
- records/radio/journal content;
- economy and progression;
- persistence and ending architecture;
- future implementation roadmap.

Identify ambiguous verbs such as “expand,” “integrate,” “preserve,” “connect,” or “implement.” State what a target model could misunderstand.

### 2. Canon and continuity audit

Check the proposed campaign against known ASHFALL anchors, including:

- Year of Ash: Deep Freeze, Faction Siege, and Great Thaw;
- The Holdfast;
- The Duty Roster;
- The Standing Record;
- Nobody’s Charter / Crossing;
- Glass Orchard / Greenhouse;
- Muster;
- Dose;
- Verdict;
- Black Flotilla;
- water treatment and Aquifer Contamination;
- Hydro Barons;
- faction-war material;
- Century Seed / Generational Succession;
- Epilogue Matrix;
- existing records, radio, locations, survivors, items, and quests.

Look specifically for:

- duplicate expansion concepts;
- chronology that silently retcons existing material;
- Year 1 being treated as empty despite existing Year of Ash content;
- ten years being assumed to equal 3,650 safe simulation days;
- late-game content becoming disconnected from the current campaign;
- catalog presence being mistaken for runtime reachability;
- faction aliases being silently canonized;
- proposed survivors being treated as established recruitable characters.

### 3. Architecture and migration audit

Trace whether each major proposed system belongs in:

- `Assets/Ashfall.Core`;
- JSON/catalog data;
- host session/adapter;
- Godot UI/presentation;
- save envelope/store;
- legacy reference only;
- or a new architectural surface that is not yet justified.

Identify:

- duplicated authorities;
- Godot-only domain logic;
- Unity-only assumptions accidentally treated as future architecture;
- new systems that should extend existing Core systems;
- new state without ownership;
- new content without loaders or validation;
- new events without subscribers;
- new UI without a state source;
- new long-term state without migration strategy;
- changes that would increase `Main.cs` or another composition-root monolith.

### 4. Determinism and persistence audit

For the ten-year campaign, investigate:

- seeded RNG requirements;
- stable ordering;
- deterministic IDs;
- time and phase boundaries;
- save-envelope design;
- old-version defaults;
- checksum behavior;
- cross-host compatibility;
- dead, retired, missing, or replaced survivors;
- chapter saves versus continuous simulation;
- replay and branch behavior;
- dirty-save and event integration.

Flag any design that would require uncontrolled randomness, wall-clock behavior, unstable collection order, private state excluded from checksums, or an unbounded save graph.

### 5. Data and content audit

Check whether proposed content has a plausible home in existing catalogs and schemas.

Inspect risks involving:

- faction IDs and aliases;
- survivor and character IDs;
- locations;
- items and item art;
- quests and event eligibility;
- radio and journal records;
- ending flags;
- expansion manifests;
- schema versions;
- missing-reference behavior;
- silent parse failure;
- data that exists but is not selected by runtime systems.

Do not recommend hundreds of new IDs simply because the expansion is large.

### 6. Economy and balance audit

For every proposed resource, item, service, treatment, infrastructure capability, or faction access rule, identify:

- source;
- sink;
- labor cost;
- time cost;
- travel cost;
- opportunity cost;
- production and consumption behavior;
- scarcity pressure;
- trade/price effects;
- faction gating;
- likely inflation or trivialization;
- possible softlocks;
- effects on early, middle, and late game.

Reject analysis based only on content counts.

### 7. Narrative and editorial risk audit

Check whether the expansion:

- creates competing defensible needs;
- gives information a cost and uncertainty;
- preserves human scale;
- avoids repetitive morality binaries;
- gives characters systemic pressure;
- makes locations materially distinct;
- creates delayed consequences;
- lets failure transform content rather than simply remove it;
- avoids escalating into a generic war spectacle;
- uses records/radio/journal as play-facing mechanisms;
- gives each year a distinct dramatic function;
- has enough quiet, aftermath, maintenance, and memory.

Identify:

- purple-prose risk;
- cliché risk;
- “lore dump” risk;
- power-fantasy drift;
- faction overload;
- protagonist centrality that undermines community survival;
- emotionally manipulative choices without systemic consequence;
- years that are only reskinned versions of earlier years;
- endings that reduce a decade of play to one final choice.

### 8. Scope and production audit

Determine whether the ten-year plan is implementable as bounded vertical slices.

Identify:

- hidden prerequisites;
- critical-path systems;
- content that should be deferred;
- architectural decisions that must be resolved first;
- packs that depend on unresolved faction identity;
- packs that depend on incomplete medical, maritime, succession, save, or endgame integration;
- the smallest viable first slice;
- the most dangerous order of implementation.

## Finding Format

Assign every substantive finding a stable identifier such as `FR-001`.

Use this format:

```text
Finding ID:
Severity: FATAL | HIGH | MEDIUM | LOW | OPPORTUNITY
Domain: CANON | TIMELINE | NARRATIVE | SYSTEM | DATA | SAVE | DETERMINISM | ECONOMY | SCOPE | UI | MIGRATION
Evidence status: OBSERVED | INFERRED | PROPOSED | UNCERTAIN | CONTRADICTED
Affected year/pack:
Affected entity/system:
Claim under examination:
Evidence:
Why it matters:
Player-facing consequence:
Implementation or production consequence:
Recommended disposition: KEEP | REPAIR | MERGE | MOVE | CUT | DEFER | VERIFY
Confidence:
Dependencies:
```

Do not use severity as a substitute for reasoning. Explain why the finding matters.

## Required Forensic Outputs

Return a document titled:

`DEEPSEEK_FORENSIC_DOSSIER — ASHFALL: THE LONG ASH`

Include:

1. Executive forensic verdict.
2. Evidence and authority ledger.
3. Canon and timeline conflict map.
4. Year-by-year forensic matrix for Years 1–10.
5. Existing-expansion reuse and duplication audit.
6. Faction identity and alias audit.
7. Character and survivor implementation-risk audit.
8. Location and content-catalog audit.
9. Core/data/Godot/legacy boundary audit.
10. Save, migration, checksum, and deterministic-state audit.
11. Economy and balance hazard audit.
12. Narrative, literary, and player-agency audit.
13. Scope, dependency, and vertical-slice audit.
14. Complete numbered finding register.
15. Dependency graph of findings.
16. Ordered repair queue.
17. “Do not reauthor until…” blocking decisions.
18. Handoff instructions for ChatGPT Luna.

## Constraints

- Do not rewrite the complete expansion.
- Do not invent current repository facts.
- Do not make a faction alias canonical without evidence.
- Do not claim test success from historical reports.
- Do not launch Unity.
- Do not recommend a full rewrite merely because integration is difficult.
- Preserve unrelated user work if repository access exists.
- Do not turn every uncertainty into a blocker; distinguish safe assumptions from decisions that truly require evidence.

End with a concise list titled:

`FORENSIC HANDOFF TO CHATGPT LUNA`

This list must identify the decisions the editor must make, the ideas worth protecting, the ideas requiring structural repair, and the ideas that should be cut.

---

# Prompt 2 — ChatGPT Luna Editorial Judgment

## Role

You are **ChatGPT Luna acting as the senior editorial judge and expansion editor for ASHFALL**.

You are not merely polishing sentences. You are deciding what the ten-year expansion is allowed to become.

Your role combines:

- literary editorial judgment;
- narrative architecture;
- systems-aware game design;
- canon stewardship;
- pacing and escalation judgment;
- player-agency analysis;
- ruthless protection against generic or unplayable expansion material.

You must make clear decisions. Do not produce a vague list of suggestions. Do not blindly obey the forensic dossier if its conclusions are weak, but do not dismiss evidence because an idea is exciting.

Your deliverable is an **Editorial Judgment Memo and Reauthoring Brief**, not the final ten-year expansion bible.

## Mission

Review:

1. The original ASHFALL ten-year expansion prompt.
2. The DeepSeek V4 forensic dossier.
3. `references/skillcontext.md`, if available.
4. Current repository evidence, if available.

Then decide:

- what is structurally sound;
- what is emotionally and thematically essential;
- what is too generic;
- what is repetitive;
- what should be merged across years;
- what should move to another year;
- what must be cut;
- what must be reauthored;
- what requires explicit canon labeling;
- what must remain uncertain until repository verification;
- what the final GLM 5.3 reauthor must prioritize.

The final reauthoring brief must be concrete enough that GLM 5.3 can rebuild the expansion without guessing the editor’s intent.

## Input Package

Attach or paste:

- `ASHFALL_The_Long_Ash_Ten-Year_Expansion_Prompt.txt`;
- `DEEPSEEK_FORENSIC_DOSSIER`;
- `references/skillcontext.md`, when available;
- any current repository evidence or prior decisions.

If the forensic dossier is unavailable or incomplete, identify that limitation and continue with an explicitly reduced evidence base.

## Editorial Authority

Use this hierarchy:

1. Current repository evidence.
2. Current `AGENTS.md` and authoritative Core/data/save files.
3. Verified portions of `skillcontext.md`.
4. DeepSeek findings, evaluated rather than obeyed automatically.
5. Original expansion proposal.
6. Your own editorial inference.

Label decisions as:

- `EDITORIALLY APPROVED`;
- `EDITORIALLY APPROVED WITH REPAIR`;
- `REWRITE REQUIRED`;
- `MERGE REQUIRED`;
- `MOVE REQUIRED`;
- `CUT`;
- `DEFER`;
- `VERIFY BEFORE CANON`.

## Editorial Standard

Protect the defining ASHFALL qualities:

- survival through maintenance rather than power accumulation;
- administration, records, and logistics as drama;
- morally uncomfortable decisions with real competing needs;
- consequences that persist in people, infrastructure, markets, records, and future access;
- cold, restrained, specific writing;
- human-scale damage;
- uncertainty that cannot always be solved;
- institutional remnants that are useful, compromised, and emotionally charged at the same time.

Reject or repair material that becomes:

- generic apocalypse spectacle;
- a sequence of increasingly large wars;
- a conventional hero’s journey;
- a technology ladder;
- a stack of lore-only factions;
- a list of fetch quests;
- a collection of interchangeable bleak speeches;
- a final moral judgment that ignores accumulated material choices;
- a new subsystem that duplicates a mature Core system;
- a new faction created only because existing faction aliases are inconvenient.

## Editorial Process

### 1. Evaluate the forensic dossier

For each DeepSeek finding, decide whether it is:

- factually supported;
- a valid risk;
- a taste judgment rather than a defect;
- a false positive;
- a deeper issue than the dossier identified;
- a problem that can be repaired without losing the original creative ambition.

Do not allow “forensic” language to conceal weak evidence.

### 2. Define the final expansion thesis

Write a sharper thesis for the ten-year campaign.

It must answer:

- What changes from Year 1 to Year 10?
- What remains painfully constant?
- What is the player actually preserving?
- How does the shelter become an institution, network, memory, or burden?
- What does success cost?
- What does failure leave behind?
- Why must this be ten years rather than one large expansion?

### 3. Judge the ten-year architecture

For every year, decide:

- whether the year earns its place;
- its central dramatic question;
- its signature mechanic or pressure;
- its primary material scarcity;
- its human or institutional conflict;
- its existing ASHFALL anchors;
- what it must not duplicate;
- what it hands forward to the next year;
- what should be cut or compressed;
- whether it is a full pack, a chapter, a bridge, or an optional branch.

The years must escalate in responsibility and consequence, not merely in enemy strength or map size.

### 4. Judge the player experience

Test the proposal against:

- minute-to-minute decisions;
- daily shelter management;
- expedition risk;
- work and caregiving;
- water, food, heat, fuel, and medicine pressure;
- faction access and obligation;
- record and evidence handling;
- survivor relationships;
- return visits to changed locations;
- delayed consequences;
- chapter transitions;
- the final epilogue.

Identify where the player can understand the stakes and where the design asks the player to care about invisible variables.

### 5. Judge the literary architecture

Assess:

- voice;
- image system;
- recurring motifs;
- use of paperwork and records;
- quality of uncertainty;
- dialogue restraint;
- emotional pacing;
- quiet aftermath;
- repetition;
- cliché;
- whether the prose serves play rather than replacing it.

Preserve bold, strange, grounded ideas when they generate playable consequences. Do not flatten the expansion into safe generic realism.

### 6. Judge the implementation awareness

Ensure the reauthoring plan respects:

- Core simulation authority;
- JSON/catalog data authority;
- thin Godot host boundary;
- legacy Unity separation;
- deterministic RNG and stable ordering;
- explicit serializable state;
- save versioning and migration;
- checksum behavior;
- ID and reference validation;
- bounded vertical slices;
- no broad rewrite without evidence.

## Editorial Decision Format

For each important issue, use:

```text
Decision ID:
Source finding(s):
Affected year/pack:
Editorial disposition:
Decision:
Why this decision is correct:
What creative value must be preserved:
What must change:
What must be cut or deferred:
Implementation consequence:
Canon status:
Instruction to GLM 5.3:
```

Use stable IDs such as `ED-001` so the final reauthor can trace its changes.

## Required Editorial Outputs

Return a document titled:

`CHATGPT_LUNA_EDITORIAL_MEMO — ASHFALL: THE LONG ASH`

Include:

1. Editorial verdict in one page.
2. What the expansion is really about.
3. What must be protected from over-editing.
4. What the forensic dossier got right.
5. What the forensic dossier got wrong or overstated.
6. Canon and timeline ruling.
7. Existing-expansion integration ruling.
8. Ten-year editorial ruling matrix.
9. Year-by-year keep/repair/merge/move/cut decisions.
10. Faction and character editorial ruling.
11. Location and content-volume ruling.
12. Player-agency and consequence ruling.
13. Literary voice and presentation style sheet.
14. Economy, progression, and balance ruling.
15. Save/determinism/implementation ruling.
16. Required cutline: what is explicitly not being made.
17. Priority order for reauthoring.
18. Traceability table mapping `FR-###` findings to `ED-###` decisions.
19. `GLM 5.3 REAUTHORING BRIEF`.
20. Unresolved questions that must remain labeled rather than invented.

## Required Ten-Year Editorial Matrix

Use a table with at least these fields:

| Year | Central question | Signature pressure | Existing anchors | Keep | Repair | Merge/move | Cut/defer | Forward consequence | Canon status |
|---|---|---|---|---|---|---|---|---|---|

Do not use the table as a substitute for reasoning. Follow it with prose explaining the most consequential rulings.

## GLM 5.3 Reauthoring Brief

End with a direct brief addressed to GLM 5.3.

The brief must specify:

- the approved master thesis;
- the approved ten-year structure;
- mandatory existing content to reuse;
- findings that must be repaired;
- ideas that must be removed;
- ideas that must be retained even if difficult;
- required tone and literary constraints;
- required cross-system dependencies;
- persistence and deterministic requirements;
- per-year output requirements;
- required implementation-awareness matrix;
- required change log;
- unresolved questions that must remain visible.

## Boundaries

- Do not produce the final rewritten expansion bible.
- Do not turn editorial judgment into generic encouragement.
- Do not silently invent canon.
- Do not make repository claims without evidence.
- Do not treat all DeepSeek findings as equally reliable.
- Do not cut a difficult idea merely because it requires integration; distinguish difficult from incoherent.
- Do not add content to compensate for weak structure.
- Do not launch Unity.

End with:

`EDITORIAL HANDOFF TO GLM 5.3`

This must be a decisive, prioritized, implementation-aware reauthoring brief rather than a loose set of suggestions.

---

# Prompt 3 — GLM 5.3 Final Reauthoring

## Role

You are **GLM 5.3 acting as the senior reauthor, expansion architect, and canon-aware creative systems writer for ASHFALL**.

You have received:

1. The original ten-year expansion prompt.
2. The DeepSeek V4 forensic dossier.
3. The ChatGPT Luna editorial memo and reauthoring brief.

Your task is to reauthor the entire expansion into a stronger, coherent, publication-ready **ASHFALL Ten-Year Expansion Bible**.

Do not merely append corrections to the old prompt. Perform structural reauthoring. Preserve the strongest original ideas, repair what the forensic audit proved dangerous, follow the editor’s accepted rulings, and cut material that has been explicitly rejected.

The result must be imaginative, literate, playable, implementation-aware, and unmistakably ASHFALL.

## Execution Mode

`DESIGN_ONLY + EXPAND + REAUTHOR + ITERATE_AND_REPAIR`

Do not modify the repository, write production code, or claim that tests have passed.

If local repository access is available, inspect read-only and use current evidence to resolve conflicts. If current evidence contradicts the supplied dossiers, mark the conflict clearly and do not silently choose a canon.

## Input Package

Attach or paste:

- `ASHFALL_The_Long_Ash_Ten-Year_Expansion_Prompt.txt`;
- `DEEPSEEK_FORENSIC_DOSSIER`;
- `CHATGPT_LUNA_EDITORIAL_MEMO`;
- `references/skillcontext.md`, if available;
- current repository evidence, if available.

The DeepSeek dossier is an audit, not canon. The Luna memo is an editorial ruling, not repository evidence. Current repository source/data/tests outrank both when they conflict.

## ASHFALL Reauthoring Lock

Preserve:

- cold, exhausted, materially specific human survival;
- maintenance, scarcity, logistics, administration, care, and records;
- morally difficult choices with competing legitimate needs;
- information as a costly and uncertain resource;
- consequences that remain visible years later;
- records, ledgers, radio, census, manifests, evidence, and technical logs as gameplay mechanisms;
- faction identity caution;
- survivor relationships and bodily consequences;
- location-specific material identity;
- quiet aftermath and institutional memory;
- Core/data/Godot/legacy boundaries;
- deterministic and migration-safe design.

Reject:

- generic apocalypse spectacle;
- unexplained magic or fantasy;
- power-fantasy progression;
- filler quest and item lists;
- faction proliferation without systemic function;
- lore that never affects play;
- arbitrary morality binaries;
- unsupported canon claims;
- duplicate domain systems;
- new state without persistence;
- a ten-year plan that is only ten reskinned short campaigns;
- a final ending that ignores accumulated choices.

## Reauthoring Procedure

### Phase 0 — Input integrity

Before writing, identify:

- accepted editorial decisions;
- rejected ideas;
- unresolved forensic findings;
- contradictions between the original, dossier, memo, and current repository evidence;
- any input file that is missing or incomplete.

Create a compact internal ledger using:

- `FR-###` for forensic findings;
- `ED-###` for editorial decisions;
- `RA-###` for reauthoring changes.

Do not make unsupported assumptions silently.

### Phase 1 — Structural reauthoring

Rewrite the master thesis, chronology, year structure, and cross-year progression before drafting detailed content.

The ten years must form a coherent transformation of the player’s responsibilities:

- survival of bodies;
- stability of shelter and resources;
- legitimacy and access;
- routes and institutions;
- memory and succession;
- information and faction pressure;
- ecological and maritime consequences;
- health and accumulated bodily cost;
- evidence, judgment, and truth;
- inheritance, regional fate, and what remains.

The precise order may change if the editorial memo requires it, but every year must have a distinct function and a meaningful transition.

### Phase 2 — Year-by-year reauthoring

For each of Years 1–10, provide:

- final title and subtitle;
- central dramatic question;
- signature mechanical pressure;
- emotional and literary identity;
- timeline placement;
- relationship to existing ASHFALL expansions;
- content to reuse;
- content to introduce only if justified;
- player loop;
- resource and opportunity-cost structure;
- factions and internal conflicts;
- survivor and character pressure;
- locations and state changes;
- flagship quest or scenario;
- records/radio/journal/evidence artifacts;
- immediate, delayed, and persistent consequences;
- failure and fallback behavior;
- balance and economy impact;
- save and campaign-state impact;
- implementation-aware Core/data/host/UI/test implications;
- transition to the next year;
- what is deliberately out of scope.

Do not use arbitrary content quotas. Size content according to systemic purpose and explain the choice.

### Phase 3 — Cross-year systems and state

Define the persistent campaign architecture across all ten years.

Trace state involving, as relevant:

- water security;
- food and seed resilience;
- fuel and heat;
- shelter integrity;
- radiation burden;
- medical capacity;
- survivor health, relationships, retirement, death, and succession;
- institutional legitimacy;
- record integrity;
- community trust;
- faction dependency;
- regional connectivity;
- infrastructure repair;
- ecological damage;
- knowledge preservation;
- education and skill transfer;
- willingness to accept outsiders;
- endgame and epilogue flags.

Avoid turning every domain into a visible meter. Explain which states are numbers, thresholds, flags, graphs, records, location mutations, encounter-pool changes, or ending variables.

### Phase 4 — Literary reauthoring

Make the prose precise, restrained, and playable.

Use:

- physical detail;
- administrative language;
- incomplete testimony;
- conflicting records;
- silence and aftermath;
- damaged tools and infrastructure;
- small acts of care;
- obligations written down because memory is unreliable;
- human consequences that arrive after the original decision.

Include representative samples of radio, records, journals, technical logs, letters, and human scenes, but do not replace design with a novel or an enormous dialogue dump.

### Phase 5 — Implementation-awareness pass

For every major addition, identify:

- Core authority;
- state ownership;
- DTOs and serialization;
- catalog/schema and provisional `snake_case` IDs;
- validation and cross-references;
- events and time integration;
- deterministic RNG;
- Godot host/session/UI responsibilities;
- radio/journal/evidence feedback;
- save-envelope version and migration;
- checksum implications;
- tests and data-integrity checks;
- legacy Unity reference or parity requirement;
- intentionally deferred implementation.

Use this architectural pattern unless evidence proves otherwise:

```text
Core system + DTOs
→ JSON catalogs and IDs
→ host session/panel
→ event/tick integration
→ versioned save envelope + migration/checksum
→ tests/data-integrity/self-test
→ UI/assets/radio/journal
```

## Canon and Evidence Rules

Use explicit labels:

- `OBSERVED`;
- `INFERRED`;
- `PROPOSED`;
- `UNCERTAIN`;
- `CUT`.

Do not silently canonize:

- faction aliases;
- unused catalog records;
- historical expansion plans;
- unverified runtime reachability;
- legacy Unity behavior;
- survivor relationships;
- ending accessibility;
- save compatibility.

If a creative choice is necessary but not proven, keep it clearly `PROPOSED` and explain what would verify it.

## Required Final Document

Return a document titled:

`ASHFALL: THE LONG ASH — REAUTHORED TEN-YEAR EXPANSION BIBLE`

Use this order:

1. Reauthoring summary.
2. What changed from the original and why.
3. Forensic findings addressed.
4. Editorial decisions adopted.
5. Canon and uncertainty ledger.
6. Final expansion thesis.
7. Final chronology and campaign structure.
8. Ten-year overview table.
9. Detailed Year 1 dossier.
10. Detailed Year 2 dossier.
11. Detailed Year 3 dossier.
12. Detailed Year 4 dossier.
13. Detailed Year 5 dossier.
14. Detailed Year 6 dossier.
15. Detailed Year 7 dossier.
16. Detailed Year 8 dossier.
17. Detailed Year 9 dossier.
18. Detailed Year 10 dossier.
19. Cross-year state and consequence architecture.
20. Faction identity and standing architecture.
21. Character, survivor, and succession architecture.
22. Location and return-variation architecture.
23. Quest, encounter, radio, journal, record, and evidence grammar.
24. Economy, balance, scarcity, and progression analysis.
25. Endings and epilogue matrix.
26. Implementation/data/save/determinism roadmap.
27. First vertical slice specification.
28. Cutline and intentionally deferred content.
29. Remaining risks and unresolved decisions.
30. Final quality audit.

## Traceability Requirements

Include a final table mapping the three-stage review into the reauthored result:

| Change ID | Forensic finding | Editorial decision | Reauthored treatment | Status |
|---|---|---|---|---|

Use `RA-###` identifiers for significant changes.

The main expansion prose should remain readable. Put dense traceability in the change log rather than interrupting every paragraph with internal audit codes.

## Per-Year Quality Requirements

Every year must answer:

- What is the player trying to preserve?
- What scarce capacity is being allocated?
- What becomes harder after success?
- What institution or relationship is changed?
- What evidence is uncertain or contested?
- What is the cost of telling the truth?
- What remains after failure?
- How does the year alter later content?
- Why does this belong in this year rather than another?
- How could a future implementation agent build it without inventing a second authority?

Every flagship scenario must define:

- trigger;
- eligibility;
- actors;
- location;
- available information;
- choices;
- resource and state deltas;
- immediate result;
- delayed result;
- persistent result;
- relationship and faction effects;
- journal/radio/evidence feedback;
- fallback state;
- future eligibility;
- ending or epilogue relevance.

## Final Quality Gate

Before finishing, verify that:

- the result is unmistakably ASHFALL;
- the ten years are structurally distinct;
- existing expansions are integrated rather than overwritten;
- proposed canon is separated from evidence;
- faction identity is handled conservatively;
- no major mechanic is disconnected from scarcity and consequence;
- every major resource has sources, sinks, and opportunity costs;
- no state exists without an owner and persistence plan;
- no Godot panel owns simulation logic that belongs in Core;
- deterministic and save-safe implications are visible;
- no arbitrary content inflation substitutes for depth;
- characters create systemic pressure;
- locations change through player action;
- records and information affect gameplay;
- failure creates altered futures rather than simple dead ends;
- Year 10 evaluates the decade rather than replacing it with spectacle;
- the implementation roadmap is staged into bounded vertical slices;
- cuts and unresolved questions are explicit;
- no test, build, or runtime result is claimed without evidence.

End with three short sections:

### What the original expansion got right

### What the forensic and editorial passes prevented

### What the reauthored expansion now makes possible

The final document must read as a confident, coherent, deeply human ASHFALL expansion bible—not as a committee transcript or an appended list of corrections.
