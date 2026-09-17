# C1 — Flagship Integration Plan [35]: Item Identification, Appraisal, Evidence & Discovery

> **Output:** `C1_planintegration[35].md`
>
> **Source baseline:** Plan 191 — Item Identification & Appraisal System
>
> **Primary mission:** introduce a discovery layer for expedition loot so genuinely uncertain finds can arrive with incomplete information and require visual inspection, testing, specialist appraisal, or research before the shelter fully understands what they are, what they do, whether they are safe, and what they are worth.
>
> **Primary architectural rule:** identification owns **knowledge about an item instance**. It does not own the item itself. Inventory remains authoritative for possession and stack/instance identity; item catalogs remain authoritative for canonical item definitions; contamination/condition authorities remain authoritative for hazardous physical state; MarketSystem remains authoritative for trade valuation; SkillProgression remains authoritative for survivor expertise; Plan 190/lore remains authoritative for history/lore; crafting remains authoritative for recipe use.
>
> **Primary modeling correction:** the source proposes an `UnidentifiedItem` DTO that duplicates `baseItemId`, a separate appraisal-skill state, generic progress/accuracy percentages, and deliberate incorrect gameplay properties. The flagship narrows this to a per-instance **identification knowledge record** referencing the canonical inventory instance, uses existing skills/certifications where possible, and treats uncertainty as **known/unknown/confidence/evidence** rather than fabricating a second false item truth.
>
> **Primary safety rule:** misidentification may change the survivor/player's *belief* about an item, but never mutates the canonical item's real properties. Dangerous unknown use is permitted only through explicit risk-aware actions and only where the item domain already has safe effect semantics.
>
> **Primary pacing rule:** common, obvious, staple survival items should auto-identify. Identification exists to make rare, ambiguous, damaged, contaminated, military, medical, technical, encrypted, counterfeit, prototype, or lore-bearing finds interesting—not to force the player to inspect every can of food and scrap part.
>
> **Mandatory execution order:** 191A item/inventory/skill/market/lore authority audit → 191B instance knowledge-state contract → 191C difficulty/evidence/method data → 191D analysis job execution through existing time/tools/work systems → 191E reveal/confidence/misidentification semantics → 191F SkillProgression/appraiser role integration → 191G inventory/crafting/market/expedition/lore integration → 191H UI, persistence, old-save migration, balance, determinism and CI → 191I advanced counterfeits/traps/services only after the core is proven.
>
> **Critical re-baseline rule:** before creating `ItemIdentificationSystem.cs`, inspect `ProceduralScavengeSystem`, `ExpeditionSystem`, `Inventory`, item instance identity, item definition/catalog schema, condition/durability, contamination/decontamination, `SkillProgressionSystem`, Plan-180 certification if relevant, `ResearchSystem`, `CraftingSystem`, `MarketSystem`, Plan-190 item lore/history, Plan-155 black market, duty/work scheduling, lab/workbench equipment systems, and save orchestration.
>
> **Guardrails:** no shadow inventory; no duplicated `baseItemId` truth if inventory already stores it; no separate appraisal skill if existing skill taxonomy can express appraisal/medicine/weapons/electronics expertise; no identification-owned market price; no identification-owned crafting recipe unlock unless crafting/knowledge authority says so; no fake `alien artifacts` unless ASHFALL canon explicitly supports them; no all-items-unknown mode as normal gameplay; no hidden property that changes actual item behavior without canonical item definition; no wrong item property written into item state; no accidental destructive testing unless explicitly authored; no arbitrary one-hour/one-day duration if the work/time systems use different units; no per-frame analysis progress; no `Guid.NewGuid`; no wall clock; no unseeded RNG; no XP farming from repeatedly re-inspecting the same known object; no inspection events/journal spam for mundane items.

---

# 0. Mission

ASHFALL's scavenging already generates interesting physical loot state.

The source baseline reports `ProceduralScavengeSystem.cs` already accounts for:
- weighted Poisson loot distribution;
- world-phase degradation;
- per-location visit counts;
- container state;
- contamination;
- decontamination.

But every generated item is immediately and perfectly understood.

Current shape:

```text
EXPEDITION LOOT
     │
     ▼
INVENTORY INSTANCE
     │
     ├── exact name known
     ├── exact use known
     ├── exact value known
     ├── exact condition known
     ├── all relevant properties known
     └── lore/history known if surfaced
```

Target shape:

```text
EXPEDITION / SALVAGE
      │
      ▼
CANONICAL INVENTORY INSTANCE
      │
      ├── true item definition
      ├── true condition
      ├── true contamination
      └── true instance provenance
              │
              ▼
IdentificationKnowledge
      │
      ├── category known?
      ├── identity known?
      ├── condition known?
      ├── hazard known?
      ├── functionality known?
      ├── market class known?
      ├── lore known?
      ├── evidence collected
      └── confidence
              │
              ▼
ANALYSIS METHOD
      │
      ├── visual inspection
      ├── basic testing
      ├── specialist testing
      ├── advanced analysis
      └── expert appraisal
              │
              ▼
REVEALED KNOWLEDGE
      │
      ├────────► Inventory presentation
      ├────────► Crafting eligibility/readability
      ├────────► Market appraisal confidence
      ├────────► Safe-use gating
      ├────────► Research / lore
      └────────► Quest / archive hooks
```

The identification system should answer:

> What does the shelter currently know about this real item instance, how was that knowledge obtained, how confident is it, and what analysis could reveal more?

It should not answer:

> What is the item's actual canonical identity?
> What is its real contamination?
> What is its true condition?
> What market price is currently offered?
> What survivor skill level exists?
> What recipe uses it?

Those remain other authorities.

---

# 1. Source-Evidence Interpretation

## 1.1 Identification is genuinely absent

The source reports zero Core matches for:
- `UnidentifiedItem`;
- `ItemAnalysis`;
- `Appraise`;
- `AnalyzeItem`;
- `ItemAppraisal`;
- `IdentifyItem`;
- `ItemIdentification`.

A knowledge layer is justified.

## 1.2 Procedural scavenging already creates the right provenance seam

Generated loot already knows:
- location;
- degradation;
- contamination;
- container history.

That is ideal evidence/provenance input.

Do not duplicate those facts into a second item DTO unless needed as immutable source references.

## 1.3 SkillProgression already owns survivor expertise

The source proposes a standalone `AppraisalSkill` DTO.

Default decision:
- **reject duplicate skill state**.

Prefer:
- existing skill IDs;
- specializations/certifications;
- knowledge tags;
- survivor capability queries.

If appraisal is truly missing:
- add it to the canonical skill taxonomy, not to ItemIdentificationState.

## 1.4 ResearchSystem may already own some knowledge unlocks

`knowledge_scavenge_efficiency` indicates research/knowledge rails exist.

Audit whether:
- technical analysis;
- material identification;
- medical knowledge
should use Research/Education knowledge instead of a new generic appraisal level.

## 1.5 “Legendary” and “alien artifact” require canon audit

The source uses:
- legendary;
- experimental tech;
- alien artifacts.

Do not insert alien/sci-fi content unless existing ASHFALL canon supports it.

Use neutral categories:
- pre-war military;
- prototype;
- encrypted;
- anomalous only if canonically real.

## 1.6 Misidentification must separate belief from truth

The canonical item never becomes:
- wrong value;
- wrong use;
- wrong property.

Only the knowledge/presentation layer can hold:
- provisional classification;
- uncertain estimate;
- suspected use.

## 1.7 Identification progress and accuracy may be redundant

A single `0–100 progress` plus `0–100 accuracy` can become opaque.

Prefer:
- evidence coverage;
- knowledge fields revealed;
- confidence band.

Use numeric score only where the underlying analysis model genuinely needs it.

---

# 2. Non-Negotiable Identification Invariants

## INV-191.1 — One inventory authority

Identification never owns possession/quantity.

## INV-191.2 — One item-definition authority

Identification never rewrites canonical definition.

## INV-191.3 — One instance identity

Knowledge record references canonical `instance_id`.

## INV-191.4 — Real physical state remains canonical

Condition, contamination, charge, durability, defects remain domain-owned.

## INV-191.5 — Knowledge can be incomplete or wrong; truth cannot

## INV-191.6 — Common obvious items can auto-identify

## INV-191.7 — Unknown does not mean unusable in all cases

Use gating is item-/risk-specific.

## INV-191.8 — Dangerous use is explicit and risk-aware

## INV-191.9 — MarketSystem owns final price

Identification may provide:
- appraisal confidence;
- perceived category;
- information asymmetry.

## INV-191.10 — SkillProgression owns expertise

## INV-191.11 — Lore authority owns lore

Identification can reveal/unlock lore access.

## INV-191.12 — Crafting authority owns recipe acceptance

## INV-191.13 — Analysis methods use real tools/workstations/time

## INV-191.14 — No free re-roll inspection exploit

Committed evidence/outcome uses stable identity/seed.

## INV-191.15 — Repeating the same method cannot farm XP/evidence indefinitely

## INV-191.16 — Restore never re-runs analysis completion

## INV-191.17 — Old saves remain fully usable

Existing items default identified unless evidence says otherwise.

## INV-191.18 — Identification is instance-aware where required

Two damaged/counterfeit/contaminated instances may reveal differently.

## INV-191.19 — Stack semantics are explicit

Do not mix differently identified instances into one indistinguishable stack if knowledge differs materially.

## INV-191.20 — Headless analysis is deterministic

---

# 3. Definition of Done

Plan 191 closes only when:

- inventory/item-instance authority is documented;
- item-definition authority is documented;
- condition/durability authority is documented;
- contamination/decontamination authority is documented;
- SkillProgression ownership is documented;
- MarketSystem valuation ownership is documented;
- CraftingSystem use/restriction ownership is documented;
- Plan-190 lore ownership is documented;
- one per-instance identification-knowledge authority exists;
- no shadow inventory exists;
- no duplicate appraisal-skill state exists;
- difficulty/evidence profiles are data-driven;
- common obvious items can auto-identify;
- medium/hard/expert-like tiers are mapped to actual item taxonomy;
- every analysis method has real tool/workstation/time requirements or is removed;
- visual inspection provides deterministic low-cost evidence;
- analysis jobs route through canonical work/time scheduling where possible;
- evidence sources are stable and idempotent;
- knowledge reveal is field-based;
- provisional/misidentified beliefs never alter canonical truth;
- uncertainty is presented honestly;
- safe-use gating is item/domain-specific;
- crafting checks canonical item identity plus player/survivor knowledge requirements separately;
- market can distinguish true price from seller/buyer appraisal knowledge where supported;
- SkillProgression receives bounded XP exactly once per meaningful evidence gain;
- Plan 190 lore becomes revealable through appropriate expertise/evidence;
- expedition returns preserve unknown-state provenance;
- old saves default existing inventory to identified;
- future newly scavenged items follow identification rules;
- save/load preserves knowledge/evidence exactly;
- stacked-item behavior is tested;
- analysis cannot be restarted to re-roll outcome;
- 30/120/180-day simulations show discovery value without inventory chore overload;
- `--item-identification-selftest` exists or equivalent;
- content acceptance proves every identification profile and method has a reachable consumer.

---

# 4. Phase P0 — Item, Instance, Skill & Market Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
Inventory APIs
item instance identity
stacking semantics
item definition/catalog schema
ProceduralScavengeSystem output DTO
Expedition loot transfer
condition/durability fields
contamination/decontamination fields
item provenance/location history
SkillProgression skill IDs
ResearchSystem knowledge IDs
Plan-180 certification if relevant
CraftingSystem ingredient matching
MarketSystem quote/value APIs
Plan-190 lore/history contract
Plan-155 black-market information asymmetry
work/job scheduler
lab/workbench equipment
campaign time authority
save order
inventory UI/item detail
```

## P0.2 Build identification authority matrix

Create:

`docs/items/ITEM_IDENTIFICATION_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
read API
write API
persisted?
identification role
status
```

Rows:
- item instance ID;
- item definition;
- item category;
- condition;
- contamination;
- defect;
- charge/ammo;
- provenance;
- market value;
- recipe compatibility;
- skill;
- certification;
- knowledge;
- lore;
- perceived identity;
- confidence;
- evidence;
- analysis job;
- reveal history.

## P0.3 Instance/stack ADR

Create:

`docs/architecture/ADR_IDENTIFICATION_INSTANCE_AND_STACKING.md`

Answer:
- when identification is per item definition;
- when it is per instance;
- how stacks behave;
- whether stack merge requires compatible knowledge/condition/provenance.

## P0.4 Misidentification ADR

Create:

`ADR_ITEM_TRUTH_VS_APPRAISAL_BELIEF.md`

Define:
- canonical truth;
- provisional belief;
- false positive;
- uncertainty;
- correction.

## P0.5 Skill ADR

Create:

`ADR_APPRAISAL_SKILL_OWNERSHIP.md`

Decide:
- reuse existing scavenging/research/medicine/weapons/electronics skills;
- add canonical appraisal skill;
- no duplicate state.

## P0.6 Baseline proof

Capture:
- expedition loot is fully identified;
- all item detail visible immediately;
- no analysis task exists.

---

# TASK 191A — Identification Knowledge State

# 191A.0 Goal

Store only what the shelter/survivor knows about a real inventory instance.

## 191A.1 Proposed owner

`Assets/Ashfall.Core/Inventory/ItemIdentificationSystem.cs`

Narrow ownership.

## 191A.2 Knowledge record

Recommended:

```text
IdentificationRecord
  instance_id
  schema_version
  discovery_day
  discovery_location_id optional
  discovered_by_survivor_id optional
  profile_id
  evidence_refs[]
  revealed_fields[]
  confidence_by_field[]
  provisional_classification optional
  full_identity_known
  processed_reveal_events[]
```

## 191A.3 Do not duplicate canonical `baseItemId`

System may query it internally.

It should not be a second persisted truth unless save architecture requires a frozen reference and authority ADR approves.

## 191A.4 Discovery provenance

Use existing expedition/scavenge provenance ID if available.

## 191A.5 Knowledge scope

Decide whether identification is:
- shelter-shared;
- survivor-local;
- mixed.

Recommended MVP:
- shelter-shared item knowledge once an item reaches shelter inventory.

This avoids needless “Alice knows but Bob does not” micromanagement.

## 191A.6 Appraiser attribution

Record who performed analysis for:
- XP;
- history;
- lore.

## 191A.7 Revealed fields

Candidate:

```text
broad_category
specific_identity
condition
contamination_or_hazard
functional_use
compatibility
market_class
rarity
special_properties
provenance
lore
```

Only actual item fields.

## 191A.8 Field-level confidence

Prefer bands:

```text
unknown
suspected
probable
confirmed
```

Numeric internal confidence optional.

## 191A.9 Full identification

Definition:

```text
all gameplay-critical fields required by profile are confirmed
```

Lore may remain separately undiscovered.

## 191A.10 Lore completion

Do not block full gameplay identification merely because flavor lore is unknown.

## 191A.11 Discovery state

Possible:

```text
unknown
partially_identified
identified
```

Derived.

## 191A.12 No separate `isIdentified` truth if derived from fields/profile

## 191A.13 Evidence

Stable IDs.

Examples:
- visual inspection;
- multimeter reading;
- reagent test;
- catalog comparison;
- firing-range test;
- lab scan;
- expert comparison.

## 191A.14 Evidence is append-only per source

Same test/source does not duplicate.

## 191A.15 Correcting belief

New stronger evidence:
- supersedes provisional classification.

Keep source provenance.

## 191A.16 No destructive history rewrite

Diagnostics can show:
- initially suspected X;
- confirmed Y.

## 191A.17 CaptureState

Versioned.

## 191A.18 RestoreState

No reveal events emitted during restore.

## 191A.19 Old save

Existing inventory:
- fully identified;
- no fake appraisal history required.

## 191A.20 New loot after migration

Normal rules.

## 191A.21 Generated docs

Create:

`ITEM_IDENTIFICATION_STATE_CONTRACT.md`

### 191A DoD

Identification persists only evidence and knowledge about canonical item instances, with no shadow copy of inventory or item truth.

---

# TASK 191B — Identification Difficulty & Item Profiles

# 191B.0 Goal

Determine which items require analysis and what must be learned about them.

## 191B.1 Data file

`Assets/StreamingAssets/Data/identification_difficulty.json`

Versioned.

## 191B.2 Prefer profile terminology

Instead of hard coding only:

```text
easy
medium
hard
expert
```

define:

```text
IdentificationProfile
```

with display tier.

## 191B.3 Profile DTO

Suggested:

```text
id
display_tier
item_tags[]
auto_reveal_fields[]
required_confirmed_fields[]
allowed_methods[]
base_complexity
hazard_disclosure_policy
lore_profile optional
```

## 191B.4 Easy tier

Common obvious items:
- scrap;
- basic food;
- obvious common tools.

Default:
- auto-identify.

## 191B.5 Medium

Potential:
- unfamiliar medicine;
- damaged electronics;
- weapon parts;
- technical consumables.

## 191B.6 Hard

Potential:
- advanced electronics;
- specialist medical items;
- encrypted/data items;
- military components;
- prototypes.

## 191B.7 Expert

Only if real content exists:
- rare pre-war military;
- experimental/prototype;
- highly specialized equipment;
- canon-supported artifact.

## 191B.8 No alien content without canon

## 191B.9 Difficulty derives from content metadata

Do not maintain a separate hand list if item tags can select profile.

## 191B.10 Override support

Specific item definition can declare profile.

## 191B.11 Auto-identification policy

Use:
- commonness;
- obvious visual identity;
- survivor/community baseline knowledge.

## 191B.12 Community baseline knowledge

Some items should be universally recognized:
- water;
- common canned food;
- scrap metal;
- obvious tools.

## 191B.13 Specialized ambiguity

An object may be:
- category known;
- exact model/function unknown.

## 191B.14 Hazard-sensitive profile

Medicine/chemicals:
- hazard/identity may require stronger evidence.

## 191B.15 Weapon profile

May reveal:
- category;
- caliber;
- condition;
- hidden defect.

Only actual fields.

## 191B.16 Electronics profile

May reveal:
- voltage/function/compatibility/condition.

## 191B.17 Medical profile

May reveal:
- drug identity;
- contamination/expiry/contraindication
if those fields exist.

## 191B.18 Lore-bearing profile

Gameplay identification separate from lore.

## 191B.19 Data integrity

Every profile:
- referenced method exists;
- required field exists;
- matching item tags exist.

## 191B.20 Generated matrix

`IDENTIFICATION_PROFILE_MATRIX.md`

### 191B DoD

Only genuinely ambiguous content requires identification, with field-specific reveal requirements driven by real item metadata rather than arbitrary rarity labels.

---

# TASK 191C — Analysis Method & Evidence Contract

# 191C.0 Goal

Make analysis methods real activities tied to actual tools, equipment, survivor capability and time.

## 191C.1 Method catalog

`Assets/StreamingAssets/Data/identification_methods.json`

## 191C.2 Method DTO

Suggested:

```text
method_id
display_key
supported_profile_tags[]
tool_requirements[]
workstation_requirement optional
skill_requirements[]
time_cost
evidence_types[]
field_reveal_rules[]
destructive_test_risk optional
```

## 191C.3 Visual inspection

Source:
- free;
- instant.

Recommended:
- low-cost;
- no workstation;
- reveals visually obvious fields.

## 191C.4 Visual inspection should not produce fake exact guesses too aggressively

Use:
- broad category;
- visible markings;
- obvious damage;
- provisional identity.

## 191C.5 Basic test

Only if tools exist.

Potential:
- multimeter;
- test kit;
- magnification.

Do not invent magnifying-glass gameplay item solely for this plan unless it fits item catalog.

## 191C.6 Time cost

Use canonical work/time unit.

Source `1 hour` is tuning candidate.

## 191C.7 Advanced analysis

Requires real:
- lab;
- diagnostic terminal;
- electronics bench;
- medical bench
as appropriate.

Do not force one universal spectrometer.

## 191C.8 Source `1 day`

Tuning candidate.

## 191C.9 Expert appraisal

Means:
- qualified survivor;
- appropriate tools/reference;
- time.

It is not a magical universal 100% action if evidence cannot support certainty.

## 191C.10 Method capability

Different methods reveal different fields.

## 191C.11 Method quality

Can depend on:
- skill;
- tool quality;
- workstation;
- condition of sample;
- reference material.

## 191C.12 No generic progress points unless needed

Prefer:
- evidence accumulation.

## 191C.13 If progress UI needed

Derive:

```text
confirmed_required_fields / total_required_fields
```

rather than arbitrary +20/+40/+30/+10.

## 191C.14 Repeated same method

May add nothing after its evidence exhausted.

## 191C.15 Alternative methods

Allow multiple routes to confirm field.

## 191C.16 Destructive testing

Only explicitly authored.

Example:
- consume reagent sample;
- discharge cell;
- open sealed casing.

## 191C.17 Player confirmation

Destructive action requires explicit warning.

## 191C.18 Analysis job

Prefer use:
- DutyRoster/work queue;
- crafting/research job framework
if available.

## 191C.19 No bespoke ticking job engine if one exists

## 191C.20 Interruption

If analyst:
- becomes sick;
- assigned elsewhere;
- emergency occurs,
job pauses/follows canonical work rules.

## 191C.21 Completion event

Stable job ID.

## 191C.22 No per-frame progress

Campaign-time boundary.

### 191C DoD

Analysis methods become real evidence-producing activities using canonical tools, workstations, survivor capability and time rather than arbitrary percentage buttons.

---

# TASK 191D — Skill, Expertise & Specialization

# 191D.0 Goal

Create a meaningful appraiser role without adding a second skill system.

## 191D.1 Audit SkillProgression

Inventory relevant existing skills:
- scavenging;
- medicine;
- weapons;
- electronics;
- research;
- crafting.

## 191D.2 Appraisal skill decision

Three acceptable options:

### A. Reuse domain skills
Weapons expert identifies weapons.
Medic identifies drugs.
Technician identifies electronics.

### B. Add one canonical appraisal skill
Only if cross-domain identification needs independent progression.

### C. Hybrid
General appraisal + domain expertise.

Choose through ADR.

## 191D.3 Reject `AppraisalSkill DTO` inside identification state

## 191D.4 Source rank labels

```text
novice
apprentice
journeyman
expert
master
```

Use existing skill rank vocabulary if one exists.

Do not create parallel rank names.

## 191D.5 Specializations

Source:
- general;
- weapons;
- medical;
- tech;
- artifact.

Map to real skill/domain tags.

## 191D.6 “Artifact”

Rename to:
- prewar;
- historical;
- prototype
unless artifact is canonical taxonomy.

## 191D.7 Accuracy bonus

Do not store generic +20% if domain model uses skill thresholds/competency.

## 191D.8 Method unlocks

Can be:
- skill threshold;
- certification;
- workstation training.

## 191D.9 Master instant identification

Hard gate.

Recommended:
- instant auto-identify only for items whose profile evidence is genuinely trivial to a master.

Do not instant-confirm hidden contamination/defects merely from rank.

## 191D.10 XP source

Grant only when analysis produces new meaningful evidence.

## 191D.11 No XP for repeated inspection with zero reveal

## 191D.12 XP scales with

- profile complexity;
- new fields confirmed;
- method sophistication.

## 191D.13 No farming known items

Once fully known:
- reappraisal grants no normal XP.

## 191D.14 Teaching

Plan 154/SkillProgression owns mentoring/training.

Source `"The Mentor"` hook is later quest content.

## 191D.15 Certification

If Plan 180 supports certifications:
- specialist appraisal certification can be a future extension.

## 191D.16 Knowledge prerequisites

Some methods may require research unlock.

## 191D.17 Reason trace

UI:
- “Medical 4 confirms pharmaceutical identity.”
- “Electronics 2 insufficient for encrypted controller.”

## 191D.18 Tests

- novice;
- specialist;
- cross-domain;
- master;
- no-XP repeat;
- save/load.

### 191D DoD

Appraisal expertise is expressed through the canonical skill/knowledge system, with meaningful domain specialization and bounded XP from genuinely new discoveries.

---

# TASK 191E — Reveal, Confidence & Misidentification

# 191E.0 Goal

Represent uncertain beliefs without corrupting canonical item truth or misleading the player unfairly.

## 191E.1 Reveal rule

Analysis outputs evidence.

Evidence updates:
- field knowledge;
- confidence;
- provisional classification.

## 191E.2 Confidence bands

Recommended:

```text
unknown
suspected
probable
confirmed
```

## 191E.3 Source accuracy ranges

50–70 / 70–90 / 90–100 are tuning inspirations only.

Do not expose false precision unless needed.

## 191E.4 Basic visible fields

Visual inspection may reveal:
- broad category;
- apparent condition;
- markings.

## 191E.5 Identity

Can be provisional:
- “Likely 9mm pistol component”
- “Unknown antibiotic-class medicine”
- “Probable pre-war controller board.”

## 191E.6 Value estimate

Market knowledge.

Identification can reveal:
- likely value class;
- not final quote.

## 191E.7 Use estimate

Can reveal:
- suspected function.

## 191E.8 Hidden properties

Only actual canonical properties.

Examples:
- defect;
- contamination;
- compatibility;
- special effect;
- authenticity.

## 191E.9 Lore

Separate reveal channel.

## 191E.10 Misidentification

Allowed as **provisional belief**.

Example:

```text
suspected_identity = item_A
confidence = suspected
canonical_identity = item_B
```

## 191E.11 No lying UI at “confirmed”

Confirmed must be correct unless the game has intentional deception/counterfeit evidence.

## 191E.12 Incorrect property

Do not copy false property onto item.

UI may show:
- suspected value/use.

## 191E.13 Correction

Better evidence replaces:
- suspected belief.

## 191E.14 Correction event

Semantic:
- appraisal_corrected
only if meaningful.

## 191E.15 Misidentified use

Source says discovered on use via wrong effect.

This is dangerous as a generic rule.

Only allow blind use if:
- user explicitly accepts risk;
- domain supports real canonical effect.

## 191E.16 Medicine safety

Do not let an unidentified medicine be consumed through a misleading confirmed label.

Use:
- unknown medication;
- explicit risk warning;
- canonical medical effect.

## 191E.17 Weapons safety

Unknown weapon:
- may be unusable until basic functional safety known;
or test-fired at risk if combat/weapons system supports.

## 191E.18 Electronics

Unknown function:
- cannot be installed into incompatible system without canonical compatibility check.

## 191E.19 Trade

Seller can trade unidentified object.
Market/buyer may:
- appraise independently;
- discount uncertainty.

## 191E.20 Player fairness

UI distinguishes:
- confirmed;
- estimate;
- unknown.

Never present a low-confidence guess visually identical to fact.

## 191E.21 Deterministic provisional belief

If probabilistic:
- stable seeded by instance + evidence + analyst.

No reload reroll.

## 191E.22 No random misidentification if evidence model can deterministically produce ambiguity

Prefer deterministic confidence.

### 191E DoD

Identification can be uncertain and occasionally wrong at the belief layer while canonical item truth remains stable, player-facing confidence is explicit, and risky use never happens invisibly.

---

# TASK 191F — Expedition & Scavenging Integration

# 191F.0 Goal

Make scavenging the main entry point for uncertain items without altering loot truth.

## 191F.1 ProceduralScavengeSystem

Continues to generate canonical loot.

## 191F.2 Identification profile selection

After/alongside instance creation:
- profile assigned based on item metadata/provenance.

## 191F.3 No different loot roll

Identification state must not reroll item identity.

## 191F.4 Expedition discovery

Return bundle includes:
- instance IDs;
- known field summary.

## 191F.5 Field identification during expedition

If scavenger skill/equipment supports:
- some visual/basic evidence may be acquired in field.

## 191F.6 No magical expedition auto-identification

Unless survivor expertise/profile says so.

## 191F.7 Scavenger apex

Audit `skill_apex_scavenger`.

Potential:
- improves initial field evidence;
- not universal full identification.

## 191F.8 `knowledge_scavenge_efficiency`

Audit.

Could:
- increase initial classification;
- improve provenance clues.

Do not duplicate.

## 191F.9 Discovery day/location

Use expedition/scavenge provenance.

## 191F.10 Container contamination

Canonical physical contamination.

Identification can hide/reveal its **knowledge**.

## 191F.11 Decontamination

If player decontaminates unknown item:
- physical state changes through canonical system;
- knowledge state may still be uncertain.

## 191F.12 World-phase degradation

Canonical condition.

Appraisal reveals it.

## 191F.13 Visit count

No direct identification effect unless authored.

## 191F.14 Loot summary

End expedition can group:

```text
Known items
Unknown finds
Hazardous/suspected finds
```

## 191F.15 Auto-sort

Do not force immediate modal analysis.

Unknown items enter inventory safely.

## 191F.16 Quarantine

If contamination/hazard system has quarantine:
- suspected hazardous unknowns can route there.

Do not invent duplicate quarantine.

### 191F DoD

Expeditions return real item instances with appropriately incomplete knowledge, while scavenging, contamination, degradation, and provenance remain canonical.

---

# TASK 191G — Inventory, Crafting, Market & Lore Integration

# 191G.0 Goal

Make identification matter across inventory decisions without duplicating downstream authority.

---

# 191G-INV — Inventory

## 191G.I1 Inventory owns item instance

## 191G.I2 Display layer uses identification read model

## 191G.I3 Unknown label

Prefer:
- “Unknown electronic component”
- “Unidentified medicine”
rather than generic “Unknown Item.”

## 191G.I4 Category visibility

Only if category is known.

## 191G.I5 Stack behavior

Do not merge:
- identified;
- unidentified
instances if presentation/provenance would be lost.

## 191G.I6 Knowledge-compatible stack merge

Allowed if:
- same canonical item;
- same relevant physical state;
- same knowledge state
according to inventory stacking contract.

## 191G.I7 Sort/filter

Filter:
- unidentified;
- suspected hazard;
- analysis-ready.

---

# 191G-CRAFT — Crafting

## 191G.C1 Crafting owns ingredient truth

## 191G.C2 Unknown item may be excluded from recipe picker

because player cannot intentionally choose what they do not know.

## 191G.C3 Once identity confirmed

Normal recipe eligibility.

## 191G.C4 Partial knowledge

Could allow category-based recipe only if crafting system supports substitute categories.

## 191G.C5 No identification-owned recipe unlock

## 191G.C6 Lore is not crafting knowledge by default

## 191G.C7 Disassembly

Can be an analysis method if CraftingSystem supports teardown.

Destructive warning.

---

# 191G-MKT — Market

## 191G.M1 MarketSystem owns true quote

## 191G.M2 Information asymmetry

Potential model:

```text
seller knowledge
buyer knowledge
item truth
market demand
→ quote negotiation
```

Only if MarketSystem supports.

## 191G.M3 MVP

Unidentified item:
- lower/uncertain offered price profile.

Market adapter handles.

## 191G.M4 Do not hardcode `price *= 0.5`

## 191G.M5 Skilled trader/appraiser

Could improve:
- valuation confidence;
- detect underpricing.

Use existing trade skills.

## 191G.M6 Black market

Plan 155 can consume:
- unknown/prototype/military item categories.

No duplicate market.

## 191G.M7 Selling unknown item

Player warned:
- potential underpricing;
- unknown hazard.

## 191G.M8 Buying unidentified item

If market supports:
- purchase mystery item;
- own appraisal risk.

Follow-on if not.

---

# 191G-LORE — Plan 190

## 191G.L1 Identification can unlock lore eligibility

## 191G.L2 Full gameplay identity ≠ full lore

## 191G.L3 Lore requirements

Potential:
- expert analysis;
- provenance evidence;
- research knowledge.

## 191G.L4 Plan 190 owns text/history

## 191G.L5 No lore duplicated inside result DTO

Store:
- lore IDs unlocked.

## 191G.L6 Significant reveal event

Can journal/archive:
- rare prototype identified;
- meaningful pre-war provenance.

No mundane item log spam.

### 191G DoD

Inventory presentation, recipe selection, market valuation, and lore discovery all consume the same identification knowledge without ceding their canonical item/economy/content ownership.

---

# TASK 191H — Analysis Jobs, Time, Tools & Work Scheduling

# 191H.0 Goal

Ensure appraisal competes with real shelter time and resources.

## 191H.1 Audit job frameworks

Possible:
- crafting queue;
- research queue;
- DutyRoster task;
- workshop job.

## 191H.2 Reuse one

Do not create:
- `TickIdentificationJobs`
as bespoke parallel scheduler if avoidable.

## 191H.3 Analysis job definition

Suggested:

```text
job_id
instance_id
method_id
analyst_id
workstation_id optional
tool_refs[]
started_day/time
required_work
progress_owner
status
```

Owner may be generic job system.

## 191H.4 Analyst eligibility

Check:
- alive;
- present;
- fit;
- skill;
- not away;
- available.

## 191H.5 Tool reservation

Canonical inventory/workstation reservation.

## 191H.6 Consumables

Tests may consume:
- reagent;
- cartridge;
- battery
only if method declares.

## 191H.7 Non-consumable tools

Reserve, do not consume.

## 191H.8 Work interruption

Canonical.

## 191H.9 Pause/resume

Stable.

## 191H.10 Emergency

May preempt.

## 191H.11 Completion

Produces deterministic evidence package.

## 191H.12 No completion reroll

Evidence result seeded/derived at job start or stable job ID.

## 191H.13 Cancel

Define:
- consumed resources;
- partial evidence;
- reservation release.

## 191H.14 Partial analysis

Can reveal if method supports milestone.
Otherwise no arbitrary progress reward.

## 191H.15 Queue UI

Reuse generic job panel if possible.

## 191H.16 Batch appraisal

Common medium items can be batched if:
- same profile/method;
- workstation capacity.

## 191H.17 Master efficiency

May reduce required work via SkillProgression modifier.

## 191H.18 No instant-global appraisal aura

### 191H DoD

Identification analysis behaves like real shelter work, using existing jobs, time, tools, survivor availability and workstation capacity.

---

# TASK 191I — Auto-Identification & Anti-Chore Design

# 191I.0 Goal

Keep the mechanic focused on discovery rather than inventory friction.

## 191I.1 Auto-identify common items

Default.

## 191I.2 Auto-identify familiar definitions

Consider shelter familiarity:

Once the shelter has fully identified item definition/model many times, future obvious identical instances may begin with more knowledge.

## 191I.3 Instance-specific hazards remain unknown where needed

Even if model is known:
- contamination;
- hidden defect;
- condition
may still require inspection.

## 191I.4 Familiarity cache

Do not duplicate item catalog.

Store:
- known model familiarity
only if it has real benefit.

## 191I.5 Common model fast path

After confirmed model knowledge:
- identity auto-known;
- instance condition/hazard still inspected.

## 191I.6 Analyst automation

Configurable:
- auto visual-inspect new unknowns;
- auto queue easy/basic tests if resources permit.

## 191I.7 No auto-consume rare reagents without player policy

## 191I.8 Auto-identification threshold

Source proposes setting.

Define:
- profile complexity;
- available skill;
- method cost.

## 191I.9 Safe defaults

Do not auto-run destructive tests.

## 191I.10 Notification budget

Notify only:
- rare find;
- hazard;
- significant correction;
- full identification of important item.

## 191I.11 No event per visual inspection

## 191I.12 Batch UI

Select:
- analyze all unknown electronics;
- inspect all expedition returns.

## 191I.13 Analysis recommendations

System suggests:
- cheapest method that can confirm remaining critical fields.

## 191I.14 Stop when gameplay-critical fields confirmed

Lore can be optional deeper analysis.

## 191I.15 Metrics

Track:
- unknown backlog;
- clicks per expedition;
- analysis queue size.

### 191I DoD

Most expeditions require little or no manual appraisal work, while rare ambiguous finds create deliberate moments of discovery and specialist value.

---

# TASK 191J — UI, Confidence & Player Communication

# 191J.0 Goal

Make uncertainty legible without hiding vital safety information unfairly.

## 191J.1 Inventory indicator

`?` icon is acceptable.

Also provide text/status.

## 191J.2 Unknown item card

Show:
- known category;
- visible condition clues;
- suspected hazard;
- provenance;
- evidence count;
- recommended analysis.

## 191J.3 No fake exact stat display

Unknown fields:
- `?`;
- estimate;
- range.

## 191J.4 Confidence styling

Text labels:
- suspected;
- probable;
- confirmed.

No color-only confidence.

## 191J.5 Provisional identity

Visually distinct from confirmed.

## 191J.6 Analysis panel

Show:
- methods;
- requirements;
- analyst eligibility;
- time;
- consumables;
- likely reveal fields;
- destructive risk.

## 191J.7 Appraiser viewer

Prefer existing survivor skill panel.

Do not create separate skill UI unless necessary.

## 191J.8 Discovery log

Source proposes full history.

Default:
- significant discoveries only.

Detailed evidence history in item detail/debug.

## 191J.9 Lore

Separate tab/section.

## 191J.10 Market warning

Selling unknown:
- “Value uncertain.”

## 191J.11 Crafting warning

Unknown ingredient:
- “Identity not confirmed.”

## 191J.12 Use warning

If risky use permitted:
- explicit.

## 191J.13 Accessibility

- text equivalents;
- keyboard;
- controller;
- no hover-only critical information;
- no tiny confidence bar only.

## 191J.14 Screen reader

Read:
- current known fields;
- confidence;
- next method.

## 191J.15 Text scale

Long technical item descriptions wrap.

## 191J.16 Tutorial

First meaningful unidentified item.

Do not trigger for auto-known scrap.

## 191J.17 Tooltips

Explain:
- why unknown;
- what method reveals;
- who can perform it.

### 191J DoD

Players always understand the difference between fact, estimate, uncertainty and hazard, and can decide whether an item is worth deeper analysis without opaque progress grinding.

---

# TASK 191K — Persistence, Migration & Idempotence

# 191K.0 Goal

Keep knowledge state stable across save/load and content updates.

## 191K.1 Persist

Only:

```text
identification records
evidence refs
field confidence/reveal
provisional classification
processed semantic events
familiarity if approved
pending analysis refs if identification owns them
```

## 191K.2 Do not persist duplicate

- canonical item definition;
- condition;
- contamination;
- market price;
- skill level;
- lore text.

## 191K.3 Old saves

All existing inventory instances:
- identified.

## 191K.4 Why

Pre-feature players already had perfect information.

Migration must not remove it.

## 191K.5 Existing contaminated items

Physical state stays canonical.
Knowledge can be marked confirmed to preserve old UI parity.

## 191K.6 New loot

Uses new identification rules.

## 191K.7 Missing record

For old item:
- identified by migration provenance.

For new item:
- create according to profile.

Need save schema/source-version signal to distinguish safely.

## 191K.8 Item removed/sold/consumed

Identification record can:
- remove with instance;
- retain compact discovery history if significant.

## 191K.9 Stack split

Knowledge follows instance/stack semantics.

## 191K.10 Stack merge

Only if knowledge compatibility rules permit.

## 191K.11 Content update changes profile

Existing identified item stays identified.

## 191K.12 Unidentified item definition updated

Knowledge fields migrate by stable field IDs.

## 191K.13 Method version update

Existing evidence remains provenance.

## 191K.14 Restore

No analysis/reveal/XP events replay.

## 191K.15 Stable event IDs

Examples:

```text
evidence:<instance>:<method>:<job>
reveal:<instance>:<field>:confirmed
correction:<instance>:<field>:<sequence>
```

## 191K.16 Analysis job completion

Processed exactly once.

## 191K.17 Policy version

Persist identification schema/catalog version.

### 191K DoD

Existing saves preserve their historical full-information behavior, new unidentified finds remain stable across reload, and no evidence, reveal or appraisal XP can replay.

---

# TASK 191L — Determinism, Exploit Prevention & Balance

# 191L.0 Goal

Make appraisal strategic without save-scumming, XP farming, infinite rerolls, or progression stalls.

## 191L.1 Deterministic evidence

Prefer deterministic method result.

## 191L.2 If uncertainty uses RNG

Seed:

```text
campaign_seed
+ instance_id
+ method_id
+ analyst_id
+ analysis_attempt_sequence
```

## 191L.3 Attempt sequence

Only increments when a genuinely different/valid attempt occurs.

## 191L.4 Reload

Same completed attempt:
- same result.

## 191L.5 Repeated visual inspection

No new evidence after first exhaustive visual pass unless item state changes.

## 191L.6 Repeated basic test

No XP/evidence farm.

## 191L.7 Tool swap

Can improve evidence only if tool quality matters and result is materially better.

## 191L.8 Analyst swap

New analyst can contribute if:
- higher/different expertise;
- not merely reroll.

## 191L.9 No analyst cycling exploit

Evidence benefit depends on capability threshold, not unique survivor count.

## 191L.10 Known item XP

Zero/near-zero.

## 191L.11 High difficulty XP

Bounded.

## 191L.12 Rare item farming

Loot generation already owns rarity.

Identification cannot duplicate instance or respawn.

## 191L.13 Market exploit

Player cannot repeatedly toggle identified/unidentified state.

Knowledge is monotonic toward confirmation.

## 191L.14 Misidentification correction

Does not create repeated full XP.

## 191L.15 Progress monotonicity

Knowledge fields should generally:
- unknown → suspected → probable → confirmed.

Correction may replace belief, not regress evidence arbitrarily.

## 191L.16 Hazard knowledge

Could become outdated if physical item state changes after testing.

Example:
- contamination after analysis.

Then field knowledge may need:
- stale timestamp;
- re-inspection.

Audit whether item states can change.

## 191L.17 Staleness

Do not mark entire identity unknown.
Only mutable fields.

## 191L.18 Analysis backlog budget

Normal campaign should not accumulate hundreds of required unknown items.

## 191L.19 Time-cost balance

Rare appraisal should compete with:
- crafting;
- research;
- medical work.

## 191L.20 Expert value

Expert reduces:
- time;
- uncertainty;
- reagent cost
but does not bypass physical truth.

### 191L DoD

Identification knowledge advances monotonically through stable evidence, cannot be rerolled/farmed, and remains a deliberate tradeoff rather than a progression tax.

---

# TASK 191M — Long-Horizon Simulation & CI

# 191M.0 Goal

Prove discovery depth without harming scavenging flow, economy, crafting access or save stability.

## 191M.1 30-day baseline

Track:
- loot items found;
- auto-identified;
- unknown;
- analysis jobs;
- analyst time;
- backlog.

## 191M.2 120-day normal campaign

Track:
- common vs rare identification burden;
- skill progression;
- market sales of unknowns;
- lore unlocks;
- analysis resource cost.

## 191M.3 180-day expert campaign

Track:
- familiarity acceleration;
- expert appraiser value;
- hard/expert profiles;
- backlog.

## 191M.4 No-appraiser scenario

Game remains playable.

Player can:
- trade unknowns;
- use basic inspection;
- train/research;
- avoid requiring identification for critical basic resources.

## 191M.5 Master appraiser

Does not trivialize:
- hidden contamination;
- mutable condition
without evidence.

## 191M.6 All-common expedition

Near-zero manual work.

## 191M.7 Rare-tech expedition

Creates meaningful analysis queue.

## 191M.8 Medical unknowns

Safety warnings clear.

## 191M.9 Contaminated unknown

Physical hazard remains canonical.

## 191M.10 Old-save migration

No inventory suddenly becomes unknown.

## 191M.11 Large inventory

Performance bounded.

## 191M.12 Market

No duplicated pricing state.

## 191M.13 Crafting

Quest-critical recipes remain reachable.

## 191M.14 Content reachability

Any item requiring expert analysis:
- at least one reachable method/skill path
or it can remain optional/tradable.

## 191M.15 No hard progression lock

Essential campaign items:
- auto-known;
- or basic-identifiable.

---

# 191M-T — Testing & CI

## 191M.T1 Data integrity

Validate:
- profiles;
- methods;
- item tags;
- field IDs;
- tools;
- workstations;
- skills;
- lore IDs;
- localization.

## 191M.T2 Selftest

Create:

```text
--item-identification-selftest
```

## 191M.T3 Selftest cases

At least:

1. common auto-identify;
2. medium unknown;
3. visual inspection;
4. basic test;
5. advanced analysis;
6. expert appraisal;
7. insufficient skill;
8. missing tool;
9. interrupted job;
10. evidence idempotence;
11. provisional misidentification;
12. correction;
13. no canonical truth mutation;
14. contamination reveal;
15. inventory stack behavior;
16. crafting gate;
17. market uncertainty;
18. lore reveal;
19. XP once;
20. old save;
21. save/load;
22. headless.

## 191M.T4 Source-scan authority gate

Detect:
- duplicate item truth in identification state;
- duplicate skill levels;
- direct price field mutation;
- direct recipe state mutation;
- direct contamination mutation;
- per-frame analysis tick;
- unseeded RNG.

## 191M.T5 Content acceptance

Profile ladder:

```text
DISCOVERED
LOADED
REGISTERED
ITEM_MATCHED
METHOD_REACHABLE
EVIDENCE_PRODUCED
FIELD_REVEALED
CONSUMER_OBSERVED
```

## 191M.T6 Dead-profile gate

No profile that matches nothing.

## 191M.T7 Unreachable-method gate

No required method whose tools/workstation/skill cannot be obtained.

## 191M.T8 Golden identification fixtures

Fixed instance + analyst + tools:
- exact evidence/reveal result.

## 191M.T9 Determinism fingerprint

Same fixture:
- same knowledge state.

## 191M.T10 Performance

Lookup:
- O(1) per instance.

No scan of whole inventory per frame.

## 191M.T11 State-size benchmark

Knowledge metadata bounded.

## 191M.T12 Generated docs

Create:
- `ITEM_IDENTIFICATION_ARCHITECTURE.md`;
- `ITEM_IDENTIFICATION_AUTHORITY_MATRIX.md`;
- `ITEM_IDENTIFICATION_STATE_CONTRACT.md`;
- `IDENTIFICATION_PROFILE_MATRIX.md`;
- `IDENTIFICATION_METHOD_MATRIX.md`;
- `IDENTIFICATION_FIELD_REVEAL_MATRIX.md`;
- `APPRAISAL_SKILL_INTEGRATION_MATRIX.md`;
- `IDENTIFICATION_STACKING_MATRIX.md`;
- `IDENTIFICATION_MIGRATION_MATRIX.md`;
- `IDENTIFICATION_BALANCE_REPORT.md`;
- `ADR_IDENTIFICATION_INSTANCE_AND_STACKING.md`;
- `ADR_ITEM_TRUTH_VS_APPRAISAL_BELIEF.md`;
- `ADR_APPRAISAL_SKILL_OWNERSHIP.md`.

### 191M DoD

Identification is deterministic, reachable, low-overhead, save-safe and fully integrated with inventory, scavenging, skills, markets, crafting and lore without duplicate state.

---

# TASK 191N — Events, Quest Hooks & Narrative Restraint

# 191N.0 Goal

Surface important discoveries without turning every analysis into narrative spam.

## 191N.1 Engine events

Use semantic:

```text
item_identification_started
item_field_confirmed
item_fully_identified
item_appraisal_corrected
rare_item_identified
hazard_identified
```

## 191N.2 Source narrative event names

Source proposes:
- The Discovery;
- The Inspection;
- The Analysis;
- The Appraisal;
- The Revelation;
- The Misidentification;
- The Expert;
- The Treasure.

Treat these as authored story/event candidates.

Do not emit all automatically.

## 191N.3 Significant threshold

Journal/event only for:
- rare;
- dangerous;
- lore-significant;
- major correction;
- specialist milestone.

## 191N.4 Skill milestone

SkillProgression owns rank-up event.

Identification may contextualize it.

## 191N.5 Quest hooks

Plan 171 owns dynamic quest generation.

Expose predicates/events:

```text
identified_item_category_count
rare_item_identified
hazard_identified
appraisal_skill_band
lore_item_identified
```

## 191N.6 Source quest ideas

- The Appraiser;
- The Expert;
- The Treasure;
- The Collector;
- The Specialist;
- The Discovery;
- The Mentor.

Treat as content backlog.

## 191N.7 Avoid grind quests by default

“Identify 20 items” risks making the mechanic chore-driven.

Prefer:
- meaningful rare discovery;
- solve a dangerous unknown;
- identify a mission-critical component.

## 191N.8 Mentor quest

Plan 154/171 if actual training exists.

## 191N.9 Collector

Could fit achievement, not necessarily quest.

## 191N.10 Lore integration

Rare pre-war item can open:
- Plan 190 lore;
- narrative hook.

### 191N DoD

Only meaningful identification milestones reach journal, event or quest systems, preserving the discovery fantasy without incentivizing repetitive grind.

---

# TASK 191O — Advanced Follow-Ons: Counterfeits, Traps, Appraisal Services & Unknown Technology

# 191O.0 Goal

Keep advanced appraisal gameplay gated behind real supporting systems.

## 191O.1 Counterfeit items

Requires:
- authenticity property;
- market fraud;
- seller provenance.

Follow-on.

## 191O.2 Trapped items

Requires:
- trap/explosive/mechanical hazard system.

Do not invent generic “cursed item.”

## 191O.3 “Cursed items”

Reject unless canon contains supernatural mechanics.

## 191O.4 Appraisal services

Possible market/service follow-on.

Requires:
- service economy;
- NPC/merchant skill.

## 191O.5 Player sells appraisal service

Requires contracts/jobs/economy support.

## 191O.6 Remote expert consultation

Could use radio/faction relationship if real.

## 191O.7 Reference-library bonus

Research/education follow-on.

## 191O.8 Museum/archive collection

Plan 162/190 if item history supports.

## 191O.9 Authenticity provenance

Could become powerful for:
- military gear;
- medicine;
- historical objects.

## 191O.10 Unknown technology

Only canon-supported.

## 191O.11 Destructive reverse engineering

Research/Crafting authority.

## 191O.12 Blueprints

Identification can reveal blueprint identity.
Crafting/Research owns unlock.

## 191O.13 Intelligence value

Encrypted data can route to Plan 131 if actual item exists.

## 191O.14 Black-market mystery lots

Plan 155 follow-on.

### 191O DoD

Advanced item mystery features remain explicit extensions over authenticity, hazards, services, research, intelligence and market systems rather than speculative mechanics hidden inside identification.

---

# 5. Core State Model

```text
CANONICAL ITEM INSTANCE
      │
      ▼
IDENTIFICATION PROFILE
      │
      ▼
INITIAL KNOWLEDGE
      │
      ├── auto-known
      └── incomplete
             │
             ▼
        ANALYSIS EVIDENCE
             │
             ▼
        FIELD CONFIDENCE
             │
             ├── suspected
             ├── probable
             └── confirmed
             │
             ▼
        IDENTIFIED FOR GAMEPLAY
             │
             └── optional deeper lore/provenance analysis
```

---

# 6. Truth vs Knowledge Contract

Canonical truth:

```text
item definition
condition
contamination
compatibility
properties
```

Identification knowledge:

```text
what the shelter believes/has confirmed
```

Never overwrite truth with belief.

---

# 7. Item Instance Contract

Use existing item instance identity.

Knowledge references:
- instance.

No second item lifecycle.

---

# 8. Stack Contract

If all instances in a stack share:

```text
canonical item
physical stack-compatible state
knowledge state
```

merge may be safe.

Otherwise split.

---

# 9. Discovery Contract

Discovery creates:
- knowledge record;
not item.

Scavenge/expedition creates item.

---

# 10. Difficulty Contract

Difficulty means:
- amount/type of evidence needed.

It does not modify:
- item rarity;
- item power.

---

# 11. Evidence Contract

Evidence is:
- method result;
- source-attributed;
- stable;
- idempotent.

---

# 12. Confidence Contract

Confidence describes belief quality.

Confirmed means:
- canonical truth established according to profile.

---

# 13. Misidentification Contract

Misidentification is:

```text
incorrect provisional belief
```

not:
- corrupted item definition.

---

# 14. Visual Inspection Contract

Reveals:
- what can actually be seen.

No omniscient hidden contamination or circuitry knowledge.

---

# 15. Basic Test Contract

Requires:
- reachable tool;
- skill where appropriate;
- time.

---

# 16. Advanced Analysis Contract

Requires:
- real workstation/equipment.

No universal fake “lab.”

---

# 17. Expert Appraisal Contract

Expertise improves:
- interpretation;
- certainty;
- efficiency.

It cannot see physically unknowable properties without evidence.

---

# 18. Skill Contract

Canonical SkillProgression owns:
- rank;
- XP;
- specialization.

Identification only queries and emits bounded XP events.

---

# 19. Research Contract

Research can:
- unlock analysis methods;
- improve interpretation;
- provide reference knowledge.

ResearchSystem owns knowledge unlock.

---

# 20. Inventory Contract

Inventory owns:
- item presence;
- stack;
- remove/add.

Identification modifies presentation only.

---

# 21. Crafting Contract

Crafting uses canonical item identity.

Knowledge determines whether player can intentionally select/use it where appropriate.

---

# 22. Market Contract

Market owns true current quote.

Identification supplies:
- uncertainty/appraisal context.

---

# 23. Lore Contract

Plan 190 owns lore text/content.

Identification supplies:
- reveal trigger.

---

# 24. Contamination Contract

Contamination system owns actual contamination.

Identification owns:
- known/suspected/confirmed hazard information.

---

# 25. Condition Contract

Condition authority owns actual durability/state.

Visual/technical analysis can reveal confidence.

---

# 26. Expedition Contract

Expedition returns:
- item instances.

Identification state attaches after/at discovery.

---

# 27. Work/Time Contract

Analysis consumes:
- real shelter time;
- analyst availability;
- tools/workstations.

---

# 28. Old-Save Contract

Pre-feature items stay fully known.

No retroactive information loss.

---

# 29. Familiarity Contract

Optional community familiarity can reduce repeated model-identification burden.

Instance-specific hazards/condition remain separate.

---

# 30. Persistence Matrix

| Fact | Owner |
|---|---|
| item definition | item catalog |
| instance possession | Inventory |
| instance ID | Inventory |
| condition | condition/item instance |
| contamination | contamination |
| provenance | scavenge/expedition/item history |
| skill | SkillProgression |
| research knowledge | ResearchSystem |
| market quote | MarketSystem |
| crafting recipe | CraftingSystem |
| lore text | Plan 190 |
| evidence | identification |
| field confidence | identification |
| provisional identity | identification |
| full gameplay identification | derived |
| analysis job | generic job/work system where possible |

---

# 31. Old-Save Migration Matrix

## Existing inventory

```text
identified = true
confidence = confirmed for gameplay-critical fields
migration_provenance = pre_feature_full_knowledge
```

## Newly generated loot

Use profile normally.

## Existing stacks

No forced split unless future state changes.

---

# 32. Exactly-Once Identities

Stable IDs:

```text
discovery:<instance>
analysis:<instance>:<method>:<job>
reveal:<instance>:<field>:<evidence>
xp:<analysis_job>:<analyst>
```

No duplicates after reload.

---

# 33. Failure Injection Matrix

## N191.1 Identification state stores a mutable duplicate item definition
Expected: authority gate fails.

## N191.2 Misidentification writes wrong canonical use/value
Expected: truth-vs-belief gate fails.

## N191.3 Re-inspecting same item repeatedly grants XP
Expected: anti-farm gate fails.

## N191.4 Reload changes provisional identity result
Expected: determinism gate fails.

## N191.5 Existing old-save inventory becomes unknown
Expected: migration gate fails.

## N191.6 Unknown common food must be analyzed before eating
Expected: anti-chore/profile gate fails unless canonically ambiguous.

## N191.7 Market price field directly changed by identification
Expected: market-authority gate fails.

## N191.8 Crafting recipe unlock stored in identification state
Expected: crafting-authority gate fails.

## N191.9 Master appraiser visually confirms hidden contamination with no evidence
Expected: evidence-semantics gate fails.

## N191.10 Two knowledge-incompatible instances merge and lose uncertainty
Expected: stacking gate fails.

## N191.11 Analysis job ticks every frame
Expected: performance/source-scan gate fails.

## N191.12 “Alien artifact” profile exists without canonical item matches
Expected: content-integrity/dead-profile gate fails.

---

# 34. Determinism Contract

Same:

```text
campaign seed
+ item instance truth
+ identification profile
+ collected evidence
+ analyst capability
+ tools/workstation
+ job ID
```

must yield same:
- evidence;
- provisional belief;
- confidence;
- field reveals;
- XP award.

---

# 35. Long-Horizon Metrics

Track:

```text
items found
items auto-identified
items partially identified
items fully identified
unknown backlog
analysis jobs
analysis time
analysis reagent cost
visual inspections
basic tests
advanced analyses
expert appraisals
corrections
hazards revealed
lore reveals
XP awarded
unknown items sold
value lost/gained from appraisal
state bytes
analysis CPU time
```

---

# 36. Balance Guardrails

Identification should make:

```text
rare find → curiosity → analysis → revelation
```

not:

```text
every expedition → 30 unknown junk items → mandatory clicks
```

---

# 37. Common-Item Guardrails

At least the majority of:
- staple food;
- water;
- scrap;
- basic materials;
- obvious tools
should be auto-known.

---

# 38. Rare-Item Guardrails

Rare technical/medical/military items:
- may need specialist evidence;
- should justify time cost with meaningful utility/value/lore.

---

# 39. Market Guardrails

Selling unknown items is a strategic option.

The penalty comes from:
- buyer risk/information asymmetry;
not arbitrary fixed half-price in identification code.

---

# 40. Skill Guardrails

Appraiser role should be valuable.

But no shelter should become softlocked because no appraiser spawned.

Provide:
- basic methods;
- training;
- trade/service;
- research;
- optional sale.

---

# 41. Misidentification Guardrails

Misidentification should be:
- rare;
- clear as uncertain;
- recoverable.

Never make the UI confidently lie without design support.

---

# 42. Safety Guardrails

Unknown:
- medicine;
- chemical;
- explosive;
- weapon
requires appropriate warning/use gating.

No surprise lethal effect from a UI that claimed certainty.

---

# 43. UI Acceptance

## Inventory
- unknown indicator;
- category if known;
- confidence.

## Item detail
- known fields;
- unknown fields;
- evidence;
- next analysis.

## Analysis
- method;
- analyst;
- tools;
- time;
- likely reveal.

## Skills
- use canonical skill UI.

## Lore
- separate from gameplay identification.

---

# 44. Accessibility

- no `?` icon as sole status;
- no color-only confidence;
- no hover-only safety warning;
- keyboard/controller analysis flow;
- text-scale support;
- readable technical details.

---

# 45. Localization

Field names, methods, confidence labels:
- localization keys.

Canonical item IDs remain logic keys.

---

# 46. Content Acceptance

Identification content ladder:

```text
DISCOVERED
LOADED
REGISTERED
ITEM_MATCHED
METHOD_AVAILABLE
ANALYSIS_STARTED
EVIDENCE_PRODUCED
FIELD_REVEALED
DOWNSTREAM_CONSUMER_USED
```

No profile/method counts without proof.

---

# 47. Reachability

Each profile:
- deterministic matching item fixture.

Each method:
- reachable analyst + tool/workstation fixture.

Each reveal field:
- at least one consumer/test.

---

# 48. Performance Guardrails

- O(1) instance knowledge lookup;
- no per-frame inventory scan;
- no repeated catalog parse;
- cached profile matching;
- event/job-completion driven.

---

# 49. CI / Gate Set

Recommended:

```text
item_identification_authority_single
item_identification_no_shadow_inventory
item_identification_truth_vs_belief
item_identification_profile_integrity
item_identification_method_integrity
item_identification_skill_authority
item_identification_market_authority
item_identification_crafting_authority
item_identification_contamination_authority
item_identification_stack_integrity
item_identification_evidence_idempotence
item_identification_xp_antifarm
item_identification_old_save
item_identification_determinism
item_identification_reachability
item_identification_long_horizon
item_identification_ui_access
```

---

# 50. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --item-identification-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 51. Recommended Commit Breakdown

```text
191A-1 inventory/item/skill/market/lore authority audit
191A-2 instance-and-stacking ADR
191A-3 truth-vs-belief ADR
191A-4 appraisal-skill ownership ADR
191A-5 identification record/state
191A-6 evidence/reveal model
191A-7 old-save migration skeleton
191A-8 docs/tests

191B-1 identification profile schema
191B-2 common auto-identify profiles
191B-3 weapons/medical/tech profiles
191B-4 rare/prototype profiles
191B-5 profile matching/integrity
191B-6 generated profile matrix

191C-1 method schema/registry
191C-2 visual inspection
191C-3 basic test
191C-4 advanced domain analysis
191C-5 expert appraisal
191C-6 destructive-test semantics
191C-7 evidence idempotence
191C-8 method docs/tests

191D-1 SkillProgression mapping
191D-2 general-vs-domain appraisal decision
191D-3 method unlocks
191D-4 specialization mapping
191D-5 XP-on-new-evidence
191D-6 master fast-path
191D-7 anti-XP-farm tests
191D-8 docs

191E-1 confidence bands
191E-2 provisional classification
191E-3 field reveal
191E-4 correction semantics
191E-5 hazardous-use warnings
191E-6 deterministic uncertainty
191E-7 player-fairness snapshots
191E-8 tests/docs

191F-1 ProceduralScavenge adapter
191F-2 Expedition loot transfer
191F-3 field evidence/scavenger skill
191F-4 contamination/condition read
191F-5 expedition-return summary
191F-6 quarantine adapter if real
191F-7 tests/docs

191G-1 inventory display/filter
191G-2 stack compatibility
191G-3 crafting knowledge gate
191G-4 market uncertainty adapter
191G-5 black-market adapter
191G-6 Plan-190 lore adapter
191G-7 significant discovery events
191G-8 tests/docs

191H-1 generic job/work integration
191H-2 analyst/tool/workstation eligibility
191H-3 time/reservation semantics
191H-4 interruption/cancel
191H-5 batch analysis
191H-6 completion idempotence
191H-7 tests/docs

191I-1 anti-chore auto-identification
191I-2 model familiarity
191I-3 instance-specific hazard separation
191I-4 auto-visual-inspection policy
191I-5 batch analysis/recommendations
191I-6 notification budget
191I-7 backlog metrics
191I-8 playtest tuning

191J-1 inventory item card
191J-2 confidence UX
191J-3 analysis panel
191J-4 safety warnings
191J-5 market/crafting warnings
191J-6 accessibility/localization
191J-7 tutorial
191J-8 snapshots

191K-1 save schema
191K-2 old-save full-identification migration
191K-3 stack split/merge persistence
191K-4 item removal cleanup/history
191K-5 version migrations
191K-6 restore idempotence
191K-7 save round-trip tests
191K-8 migration docs

191L-1 stable seeded uncertainty
191L-2 reroll prevention
191L-3 repeated-method anti-farm
191L-4 analyst/tool swap semantics
191L-5 monotonic knowledge
191L-6 mutable-field staleness
191L-7 exploit tests
191L-8 balance docs

191M-1 30-day baseline
191M-2 120-day normal campaign
191M-3 180-day expert campaign
191M-4 no-appraiser scenario
191M-5 large-inventory/performance
191M-6 CI/failure fixtures/goldens
191M-7 generated reports
191M-8 final ship/no-ship report

191N-1 semantic event/quest hook surface
191N-2 narrative event disposition
191N-3 journal/event budget

191O-1 counterfeits/traps/services/reverse-engineering follow-on disposition
```

---

# 52. Risk Register

## R191.1 Shadow inventory and item truth

Mitigation:
- instance reference only;
- authority matrix.

## R191.2 Appraisal skill duplicates SkillProgression

Mitigation:
- mandatory ADR;
- canonical skill integration.

## R191.3 Identification becomes inventory chore

Mitigation:
- auto-identify common items;
- batch analysis;
- familiarity.

## R191.4 Misidentification feels unfair

Mitigation:
- confidence labels;
- truth/belief separation;
- explicit risk.

## R191.5 Analysis jobs duplicate work scheduler

Mitigation:
- generic job/DutyRoster integration.

## R191.6 Market valuation duplicated

Mitigation:
- market adapter only.

## R191.7 Crafting becomes softlocked

Mitigation:
- basic items auto-known;
- reachability tests;
- multiple analysis routes.

## R191.8 Old saves lose knowledge

Mitigation:
- existing items migrate fully identified.

## R191.9 Stacks lose per-instance knowledge

Mitigation:
- stacking ADR and compatibility gate.

## R191.10 Rare/canon-breaking content sneaks in

Mitigation:
- item-profile reachability/canon validation.

---

# 53. Acceptance Checklist

## P0

- [ ] Inventory authority audited
- [ ] item instance identity audited
- [ ] stacking semantics audited
- [ ] item definition/catalog audited
- [ ] ProceduralScavenge output audited
- [ ] Expedition loot transfer audited
- [ ] condition/durability audited
- [ ] contamination/decontamination audited
- [ ] provenance audited
- [ ] SkillProgression audited
- [ ] ResearchSystem audited
- [ ] Plan 180 certification audited if relevant
- [ ] CraftingSystem audited
- [ ] MarketSystem audited
- [ ] Plan 190 lore audited
- [ ] Plan 155 market asymmetry audited
- [ ] generic work/job framework audited
- [ ] labs/workbenches audited
- [ ] time authority audited
- [ ] save order audited
- [ ] inventory UI audited
- [ ] authority matrix published
- [ ] instance/stack ADR
- [ ] truth-vs-belief ADR
- [ ] appraisal skill ADR
- [ ] baseline fully-known behavior captured

## 191A — State

- [ ] narrow ItemIdentification owner
- [ ] record references canonical instance
- [ ] no duplicate base item truth
- [ ] discovery provenance
- [ ] shelter-vs-survivor knowledge scope decided
- [ ] analyst attribution
- [ ] only real reveal fields
- [ ] confidence bands
- [ ] gameplay full-identification semantics
- [ ] lore separate
- [ ] derived identification state
- [ ] evidence refs stable
- [ ] evidence idempotent
- [ ] stronger evidence corrects belief
- [ ] correction provenance preserved
- [ ] CaptureState
- [ ] RestoreState side-effect free
- [ ] old-save existing inventory fully known
- [ ] state contract docs

## 191B — Profiles

- [ ] versioned difficulty/profile data
- [ ] typed profile DTO
- [ ] easy auto-identifies common items
- [ ] medium mapped to real item tags
- [ ] hard mapped to real item tags
- [ ] expert only for canonical content
- [ ] no alien artifacts without canon
- [ ] metadata-driven matching
- [ ] specific item override
- [ ] community baseline knowledge
- [ ] category-vs-exact identity distinction
- [ ] hazard-sensitive profiles
- [ ] weapon fields real
- [ ] electronics fields real
- [ ] medical fields real
- [ ] lore separate
- [ ] profile integrity
- [ ] generated matrix

## 191C — Methods

- [ ] method catalog
- [ ] typed method DTO
- [ ] visual inspection grounded
- [ ] no aggressive fake guesses
- [ ] basic tools exist or are deferred
- [ ] canonical time unit
- [ ] advanced equipment real
- [ ] no universal fake spectrometer
- [ ] expert appraisal evidence-bound
- [ ] field-specific method capability
- [ ] quality inputs grounded
- [ ] arbitrary progress increments removed
- [ ] progress derived if shown
- [ ] repeat method saturates
- [ ] alternative evidence routes
- [ ] destructive testing explicit
- [ ] destructive warning
- [ ] generic job integration
- [ ] no bespoke tick engine if avoidable
- [ ] interruption
- [ ] stable completion event
- [ ] no per-frame progress

## 191D — Skill

- [ ] existing skills inventoried
- [ ] no AppraisalSkill shadow DTO
- [ ] general/domain appraisal decision
- [ ] existing rank vocabulary reused
- [ ] specializations map to real domains
- [ ] artifact terminology canon-safe
- [ ] no redundant +20% magic bonus if thresholds better
- [ ] method unlocks canonical
- [ ] master fast-path evidence-safe
- [ ] XP only on meaningful evidence
- [ ] no repeat-inspection XP
- [ ] XP bounded by complexity/reveal
- [ ] known item no-farm
- [ ] teaching canonical
- [ ] certification follow-on if supported
- [ ] research prerequisites
- [ ] reason trace
- [ ] tests

## 191E — Reveal/Misidentification

- [ ] evidence updates field knowledge
- [ ] semantic confidence bands
- [ ] source accuracy ranges treated as tuning
- [ ] visual fields only
- [ ] provisional identity
- [ ] value is estimate not quote
- [ ] use is suspected until confirmed
- [ ] hidden fields are real canonical properties
- [ ] lore separate
- [ ] misidentification exists only in belief layer
- [ ] confirmed is correct
- [ ] no false property copied into item
- [ ] correction
- [ ] correction event bounded
- [ ] blind use explicitly risk-aware
- [ ] medicine safety
- [ ] weapon safety
- [ ] electronics compatibility
- [ ] market unknown sale
- [ ] confidence visually clear
- [ ] provisional result deterministic
- [ ] random mis-ID avoided unless needed

## 191F — Scavenge/Expedition

- [ ] ProceduralScavenge remains loot authority
- [ ] profile assigned after canonical item generation
- [ ] identification does not reroll loot
- [ ] expedition bundle references instance
- [ ] field evidence if real
- [ ] no magic field auto-ID
- [ ] apex scavenger audited
- [ ] scavenge knowledge audited
- [ ] provenance reused
- [ ] contamination truth preserved
- [ ] decontamination truth preserved
- [ ] world-phase condition preserved
- [ ] no unnecessary visit-count coupling
- [ ] return summary
- [ ] no forced modal analysis
- [ ] quarantine adapter only if real

## 191G — Inventory

- [ ] Inventory owns instance
- [ ] display uses read model
- [ ] useful unknown labels
- [ ] category hidden when unknown
- [ ] stack knowledge compatibility
- [ ] sort/filter support

## 191G — Crafting

- [ ] Crafting owns ingredient truth
- [ ] unknown intentional selection gated
- [ ] confirmed identity enables normal recipe path
- [ ] category substitutes only if crafting supports
- [ ] no recipe unlock in identification
- [ ] lore separate
- [ ] disassembly destructive analysis only if real

## 191G — Market

- [ ] Market owns quote
- [ ] asymmetry integrated only if supported
- [ ] no fixed 50% hardcode
- [ ] trader/appraiser skill integration grounded
- [ ] black-market adapter
- [ ] unknown-sale warning
- [ ] unknown-buy follow-on if unsupported

## 191G — Lore

- [ ] Plan 190 owns lore
- [ ] gameplay identification separate
- [ ] lore requirement grounded
- [ ] lore text not duplicated
- [ ] significant rare reveal event only

## 191H — Jobs

- [ ] generic job framework reused
- [ ] analyst eligibility
- [ ] tool reservation
- [ ] consumables
- [ ] non-consumable tools
- [ ] interruption
- [ ] pause/resume
- [ ] emergency preemption
- [ ] deterministic completion evidence
- [ ] no reroll
- [ ] cancel semantics
- [ ] partial analysis intentional
- [ ] queue UI reused
- [ ] batch appraisal if appropriate
- [ ] master efficiency bounded
- [ ] no global aura

## 191I — Anti-Chore

- [ ] common items auto-known
- [ ] model familiarity considered
- [ ] instance hazard remains separate
- [ ] familiarity state minimal
- [ ] common model fast path
- [ ] auto visual-inspection policy
- [ ] no destructive auto-analysis
- [ ] auto-ID threshold semantics
- [ ] safe defaults
- [ ] notification budget
- [ ] no inspection event spam
- [ ] batch UI
- [ ] recommended next method
- [ ] stop at gameplay-critical confirmation
- [ ] backlog/click metrics

## 191J — UI

- [ ] icon has text equivalent
- [ ] unknown item card
- [ ] no fake exact stats
- [ ] semantic confidence
- [ ] provisional vs confirmed distinct
- [ ] method requirements shown
- [ ] analyst eligibility shown
- [ ] time/consumables shown
- [ ] destructive risk shown
- [ ] skill viewer reused if possible
- [ ] discovery log significant-only
- [ ] lore separate
- [ ] market warning
- [ ] crafting warning
- [ ] use warning
- [ ] keyboard/controller
- [ ] no hover-only safety
- [ ] screen-reader support
- [ ] text scale
- [ ] meaningful tutorial
- [ ] explanatory tooltips

## 191K — Persistence

- [ ] only identification-specific knowledge persisted
- [ ] no duplicate definition
- [ ] no duplicate condition
- [ ] no duplicate contamination
- [ ] no duplicate price
- [ ] no duplicate skill
- [ ] no lore text duplicate
- [ ] old-save items fully identified
- [ ] contamination knowledge parity preserved
- [ ] new loot uses new rules
- [ ] missing-record migration distinction
- [ ] removed item cleanup
- [ ] stack split knowledge
- [ ] stack merge knowledge
- [ ] content update preserves identified state
- [ ] stable field IDs
- [ ] method evidence preserved
- [ ] restore no side effects
- [ ] stable event IDs
- [ ] XP exactly once
- [ ] schema/policy version

## 191L — Determinism/Exploit

- [ ] deterministic evidence preferred
- [ ] RNG stable if used
- [ ] reload same result
- [ ] repeated visual no new evidence
- [ ] repeated test no farm
- [ ] tool upgrade meaningful
- [ ] analyst swap not reroll
- [ ] analyst cycling prevented
- [ ] known item XP absent
- [ ] high-difficulty XP bounded
- [ ] rare item no duplication
- [ ] cannot toggle identified state
- [ ] correction no full XP repeat
- [ ] knowledge generally monotonic
- [ ] mutable field staleness handled
- [ ] backlog bounded
- [ ] time cost balanced
- [ ] expert value without omniscience

## 191M — Simulation

- [ ] 30-day baseline
- [ ] 120-day normal
- [ ] 180-day expert
- [ ] no-appraiser playable
- [ ] master not omniscient
- [ ] all-common low-overhead
- [ ] rare-tech meaningful
- [ ] medical safety
- [ ] contaminated unknown
- [ ] old-save parity
- [ ] large inventory performance
- [ ] market single authority
- [ ] crafting reachability
- [ ] expert-method reachability
- [ ] no critical softlock

## 191M — CI

- [ ] profile integrity
- [ ] method integrity
- [ ] item-tag integrity
- [ ] field integrity
- [ ] tool/workstation integrity
- [ ] skill integrity
- [ ] lore integrity
- [ ] localization
- [ ] item-identification selftest
- [ ] no shadow inventory gate
- [ ] truth-vs-belief gate
- [ ] skill authority gate
- [ ] market authority gate
- [ ] crafting authority gate
- [ ] contamination authority gate
- [ ] stack integrity
- [ ] evidence idempotence
- [ ] XP anti-farm
- [ ] old-save gate
- [ ] deterministic goldens
- [ ] content acceptance
- [ ] dead-profile gate
- [ ] unreachable-method gate
- [ ] performance
- [ ] state-size benchmark
- [ ] generated docs
- [ ] verify-fast

## 191N/O

- [ ] semantic events bounded
- [ ] source narrative event names treated as content candidates
- [ ] only significant discoveries journaled
- [ ] Plan 171 owns dynamic quests
- [ ] grind quest ideas reviewed critically
- [ ] mentor hook requires training system
- [ ] collector likely achievement not mandatory quest
- [ ] counterfeits follow-on
- [ ] traps require real hazard system
- [ ] cursed items rejected absent canon
- [ ] appraisal services follow market/service system
- [ ] remote consultation follow-on
- [ ] reference library uses Research/Education
- [ ] museum/archive uses Plan 162/190
- [ ] reverse engineering uses Research/Crafting
- [ ] encrypted intelligence uses Plan 131
- [ ] black-market mystery lots use Plan 155

---

# 54. Ship / No-Ship Gate

**SHIP** only if:

```text
inventory_authorities == 1
AND item_definition_authorities == 1
AND identification_shadow_inventory_state == 0
AND identification_duplicate_base_item_truth == 0
AND identification_duplicate_skill_state == 0
AND identification_owned_market_price == false
AND identification_owned_recipe_unlock_state == false
AND identification_owned_contamination_truth == false
AND canonical_item_truth_mutated_by_misidentification == false
AND low_confidence_guess_presented_as_confirmed_fact == false
AND common_obvious_item_chore_rate_within_budget == true
AND required_methods_without_reachable_tools_or_skills == 0
AND identification_xp_repeat_farms == 0
AND analysis_reload_rerolls == 0
AND old_save_items_become_unidentified == false
AND knowledge_incompatible_stack_merges == 0
AND per_frame_analysis_processing == 0
AND unseeded_identification_rng == 0
AND canon_unsupported_identification_profiles == 0
AND item_identification_old_save == pass
AND item_identification_save_roundtrip == pass
AND item_identification_truth_vs_belief == pass
AND item_identification_evidence_idempotence == pass
AND item_identification_skill_integration == pass
AND item_identification_market_integration == pass
AND item_identification_crafting_integration == pass
AND item_identification_contamination_integration == pass
AND item_identification_stack_integrity == pass
AND item_identification_reachability == pass
AND item_identification_30_day_balance == pass
AND item_identification_120_day_balance == pass
AND item_identification_180_day_balance == pass
AND item_identification_large_inventory_performance == pass
AND item_identification_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 55. Implementer Handoff

1. Audit inventory instance identity, stacking, item definition, condition, contamination, skills, crafting, market and Plan-190 lore before writing identification code.
2. Store **knowledge about an instance**, not a second copy of the item.
3. Do not persist a duplicate `baseItemId` truth unless the save architecture explicitly requires it and the ADR approves.
4. Keep all real physical properties canonical.
5. Separate item truth from provisional appraisal belief.
6. Never let misidentification write false gameplay properties into canonical item state.
7. Use confidence labels so players can distinguish estimate from fact.
8. Auto-identify common obvious survival items.
9. Make rare/ambiguous technical, medical, military, prototype and lore-bearing finds the main analysis targets.
10. Audit ASHFALL canon before shipping “artifact,” “legendary,” or alien-like content.
11. Replace arbitrary progress increments with evidence/required-field coverage where possible.
12. Make visual inspection reveal only visually plausible information.
13. Make basic/advanced methods use real tools/workstations and canonical shelter time.
14. Reuse generic work/research/crafting queues rather than creating a bespoke per-frame analysis scheduler.
15. Reuse SkillProgression; do not store a second appraisal rank system.
16. Award XP only for new meaningful evidence.
17. Prevent repeat-inspection, analyst-cycling, tool-cycling and reload reroll exploits.
18. Let Expedition/ProceduralScavenge create items and provenance; identification only attaches knowledge state.
19. Let contamination/degradation remain real even when hidden from the player.
20. Let Inventory own stacks and explicitly define whether differently known instances can merge.
21. Let Crafting own ingredient truth and recipe unlocks.
22. Let MarketSystem own final value/quote; identification only contributes uncertainty/appraisal information.
23. Let Plan 190 own item lore and history text.
24. Migrate every pre-feature old-save item as fully identified to preserve historical player knowledge.
25. Add model familiarity/auto-inspection only if it meaningfully reduces repeated chores.
26. Keep significant-discovery events rare; do not journal every visual inspection.
27. Run no-appraiser, all-common, rare-tech, medical-hazard, master-appraiser and large-inventory scenarios.
28. Close only when appraisal makes salvage feel mysterious and rewarding without turning every expedition into an inventory-processing tax.

---

# 56. Final Outcome

When this plan is complete, ASHFALL's scavenging loop gains a real discovery phase.

Most ordinary survival supplies remain obvious. Scrap is scrap. Water is water. A common tool does not require a laboratory.

But a damaged pre-war electronics module, an unfamiliar pharmaceutical package, a sealed military component, an encrypted data device, or a prototype part can come back from an expedition with incomplete information.

The item itself is already real.

Its identity exists in the item catalog.
Its condition exists in the canonical item instance.
Its contamination exists in the contamination system.
Its provenance exists in the expedition/scavenging record.

What the shelter lacks is knowledge.

A visual inspection may reveal the broad category and visible damage.
A basic electrical or chemical test may confirm likely function.
A technician or medic may recognize domain-specific clues.
A proper workstation can produce stronger evidence.
An experienced appraiser can interpret ambiguous results faster and more reliably.

As evidence accumulates, the interface changes from:

```text
Unknown electronic component
```

to:

```text
Probable pre-war power regulator
```

and finally:

```text
Confirmed: PR-12 power regulator
Condition: damaged
Compatibility: generator control bus
Hazard: no contamination detected
```

If an early appraisal was wrong, the system corrects the **belief**, not the item.

The canonical item never became something else.

That distinction lets ASHFALL use uncertainty without cheating the player.

Market interactions gain information asymmetry: selling an unknown object may mean accepting a cautious price. Crafting becomes more deliberate because the player cannot knowingly install a component whose identity is still uncertain. Lore gains a natural reveal point when a rare object is finally understood. Specialists gain meaningful work without requiring a duplicate skill system.

The mechanic also knows when to get out of the way.

Common items auto-identify.
Repeated familiar models become easier.
Visual inspection can be automated.
Batch analysis reduces clicks.
Only important hazards or discoveries demand attention.

The result is not an “identify every item” chore.

It is a salvage loop in which the wasteland can still surprise the player after the expedition has already returned home—and where knowledge itself becomes one of the shelter's valuable resources.
