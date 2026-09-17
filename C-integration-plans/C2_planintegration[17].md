# C2 — Flagship Integration Plan [17]: Authored Survivor Identity, Declared Item Tags, and Diegetic Personality Observability

> **Deliverable:** `C2_planintegration[17].md`
> **Source:** Plan 40 — *Authored Personality, Not Inferred: Wire the Survivor Identity Layer*
> **Wave:** Continuity Wave 6 — *The People In It*
> **Required order:** **36A → 40A → 40B → 40C**
> **Primary objective:** replace inferred survivor identity with authored data, move generic item behavior from literal IDs to validated tags/properties, and expose survivor identity through diegetic consequences rather than omniscient stat sheets.
> **Dependencies:** Plan 36A port contracts; Plan 24A fitness/duty suitability; Plan 25A/25C localization; Plan 31A semantic events; Plan 22A single-consume path.
> **Downstream:** Plans 41–44 and later identity-heavy systems must consume this plumbing instead of introducing new `Infer*` shims.
> **Guardrails:** no new survivor identity facts invented in code; no second profession/belief authority; no numeric compatibility meter; no tag vocabulary understood by only one consumer; do not delete heirloom/eulogy authored intent merely because it is unwired.

---

# 0. Executive Intent

ASHFALL already contains a substantial authored identity layer: survivor belief profiles, professions, keepsakes, phantom backgrounds, item tags, heirloom data, and an unused eulogy engine. The live social system is not missing; the authored data is simply not plumbed into it.

Current defect:

```text
survivor traits
→ InferBeliefProfile()
→ ideological friction
```

Target:

```text
authored survivor identity
→ one Core loader/query authority
→ setup-time registration
→ friction / duty / heirloom / memory / mentoring
→ semantic events
→ player-known identity projection
```

The plan must make one rule structurally true:

> **If survivor identity is authored, runtime code reads it; it never infers a shadow fact.**

The parallel item rule is:

> **Generic item behavior depends on declared properties/tags, not literal item IDs or ID-shape parsing.**

The presentation rule is:

> **Players learn personality from behavior, reasons, memories, refusals, keepsakes, and consequences—not from a compatibility score.**

---

# 1. Starting Evidence and Interpretation

The source establishes that:

- survivor enrichment fields are authored for a large cast;
- `ExpansionEnrichmentCatalog` already exists with survivor queries;
- that catalog has no live consumer;
- host code calls `InferBeliefProfile` and explicitly labels it best-effort mapping;
- ideological friction is live once beliefs are registered;
- base survivor data already owns profession, while enrichment duplicates it;
- keepsake ownership is inferred from item-ID shape;
- the heirloom catalog is test-only;
- the procedural eulogy engine is completely unwired;
- item tags are authored but unused;
- several systems still use hardcoded item-ID lists;
- survivor social coordination and relations UI are otherwise healthy.

The implementation strategy is therefore **plumbing and authority repair**, not a social-system rewrite.

---

# 2. Program-Level Success Criteria

C2[17] is complete only when:

1. Every survivor participating in ideological friction gets belief identity from authored data or an explicit documented default/exclusion rule.
2. `InferBeliefProfile` and equivalent personality heuristics are absent from production source.
3. `profession` has one authority.
4. Enrichment data extends base identity instead of duplicating it.
5. The enrichment catalog is loaded and produces runtime effects.
6. Identity registration happens once during setup and survives New Game/Load correctly.
7. Static authored identity is not redundantly persisted as mutable save state.
8. Item tags are first-class properties available through the canonical item definition/query surface.
9. Generic behavior-driving literal item-ID lists are eliminated where tags/properties apply.
10. Keepsake ownership comes from an explicit survivor→item reference, not string parsing.
11. Tags and identity references are integrity-validated.
12. Relations/duty/journal/briefing surfaces expose reasons and known identity without omniscience.
13. Downstream plans are blocked from introducing bespoke identity derivation paths.

---

# 3. Architectural Invariants

## 3.1 One owner per identity fact

Recommended ownership:

| Fact | Authority |
|---|---|
| survivor ID | base survivor catalog |
| profession | base survivor catalog |
| belief profile | enrichment identity catalog |
| keepsake reference | enrichment identity catalog |
| phantom background | enrichment identity catalog |
| manifesto/stance enrichment | its authored enrichment catalog |

## 3.2 Authored beats inferred

No trait-to-belief mapping may silently override or substitute authored data.

## 3.3 Static and dynamic identity are distinct

Static/config-like:

- profession,
- belief profile,
- keepsake reference,
- phantom background.

Dynamic/persisted:

- affinity,
- grievances,
- bonds,
- discovered/known identity cues,
- memories.

## 3.4 Restore never clobbers authored identity

Load order must make this explicit.

## 3.5 Tags express generic semantics

Tags describe categories/properties. Ownership and unique entity identity remain explicit references/IDs.

## 3.6 UI consumes known identity, not raw authored truth

Simulation may know a belief before the player does.

---

# 4. Dependency Graph

```text
36A port contract
   │
   ▼
40A authored identity authority
   │
   ├─ belief registration
   ├─ profession canonicalization
   ├─ keepsake reference
   ├─ phantom-background handoff
   └─ identity events
   │
   ▼
40B declared item tags
   │
   ├─ medical categories
   ├─ food categories
   ├─ keepsake/decor behavior
   ├─ decontamination/pharma
   └─ trade/property logic
   │
   ▼
40C diegetic observability
   ├─ relations reasons
   ├─ duty suitability
   ├─ grievances
   ├─ keepsakes
   └─ partial identity discovery
```

Hard execution order:

```text
36A → 40A → 40B → 40C → Plans 41–44
```

---

# 5. Baseline Capture

Before changes, record:

- survivor definition count;
- enrichment survivor count;
- authored belief coverage;
- missing belief IDs;
- duplicate profession fields and mismatches;
- current inferred belief for each survivor;
- ideological-friction registration count;
- item-tag count;
- hardcoded behavior ID-list sites;
- keepsake ID-shape parsing sites;
- heirloom/eulogy runtime references;
- content-utilization status of enrichment/tag catalogs.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
python3 scripts/ci/generate-port-contract.py --check
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Pin one representative social-state save and relations snapshot before migration.

---

# 6. Task 40A — Load Authored Identity and Delete Guesswork

## Goal

Every survivor belief, profession, keepsake, and phantom-background fact comes from one authority, is registered exactly once, and cannot be silently invented by host code.

---

# 7. 40A.1 — Write the Identity Authority ADR First

Create/update:

```text
docs/systems/SURVIVOR_IDENTITY.md
```

Document:

- field owner;
- source catalog;
- required/optional status;
- missing-value policy;
- save behavior;
- downstream consumers;
- player-knowledge behavior.

Add the explicit rule:

```text
authored, not inferred
```

and reference it from AGENTS identity/data rules.

---

# 8. 40A.2 — Resolve the Profession Fork

Source-preferred authority:

```text
profession → survivors.json
```

Procedure:

1. compare base and enrichment profession values;
2. publish mismatch report;
3. preserve canonical base value;
4. migrate any unique enrichment-only correction deliberately;
5. remove/deprecate duplicate enrichment field;
6. add schema/integrity rule preventing duplicate ownership from returning.

Do not silently choose one copy if values disagree.

---

# 9. 40A.3 — Add `ExpansionEnrichmentCatalogLoader`

Create:

```text
Assets/Ashfall.Core/ExpansionEnrichmentCatalogLoader.cs
```

Use established Core loader conventions:

- serializer abstraction;
- deterministic parsing;
- `CatalogDiagnostics.Warn(path, shape, ex)` or repository equivalent;
- explicit optionality policy.

If survivor identity enrichment is production-required, missing file must not become silent-empty behavior.

---

# 10. 40A.4 — Make the Loader the Only Enrichment Ingress

Host code must not directly parse enrichment JSON.

The loader/catalog owns:

- survivor enrichment entries;
- item-tag enrichment entries where appropriate;
- stable query APIs.

Keep one runtime catalog instance per campaign/application authority according to existing composition rules.

---

# 11. 40A.5 — Setup-Time Identity Registration

In `SetupSurvivorSocial` or its manifest-driven equivalent:

```text
load catalog
→ iterate canonical survivor definitions
→ resolve authored belief_profile_id
→ RegisterBelief(survivorId, belief_profile_id)
```

No lazy identity binding from UI.

No first-use initialization.

---

# 12. 40A.6 — Delete `InferBeliefProfile`

Remove the production heuristic outright.

Add a narrow source-scan regression check for patterns such as:

```text
InferBelief*
GuessBelief*
TraitToBelief*
BestEffortBelief*
```

The scanner is a guard against reintroducing shadow identity, not a general naming ban.

---

# 13. 40A.7 — Define Missing-Belief Semantics

Never use `string.Empty` as an accidental no-op.

Choose one documented policy:

### A. Canonical neutral/default belief

A named default profile participates normally.

### B. Explicit no-friction participation

The survivor remains valid but is excluded from ideological-friction comparisons until identity is authored.

The policy must be visible in integrity reports and tests.

---

# 14. 40A.8 — Identity Integrity Tier

Extend catalog validation:

- every referenced belief profile exists;
- every profile intended for production is used;
- duplicate profile IDs fail;
- duplicate survivor enrichment rows fail;
- profession duplication fails after migration;
- keepsake references resolve;
- phantom-background IDs resolve if they use a registry;
- required identity fields cannot silently disappear.

---

# 15. 40A.9 — Register Exactly Once

Add a setup/lifecycle assertion:

```text
one survivor
→ zero or one identity registration per session
```

New Game/Load may rebuild a session, but must not duplicate registrations inside one live session.

Coordinate with Plan 28 lifecycle and Plan 36 wiring diagnostics.

---

# 16. 40A.10 — Restore Ordering Contract

Required order:

```text
resolve authored identity
→ construct/register social authorities
→ restore dynamic relations/friction history
```

Restore may update dynamic state but not overwrite belief/profession/keepsake identity.

Add a regression test specifically proving restore does not clobber authored identity.

---

# 17. 40A.11 — Profession Consumers

Route canonical profession into existing consumers:

- duty suitability/readiness;
- apprenticeship/mentor matching;
- player-facing advisory hints in 40C.

No second profession normalization table unless an existing canonical role mapping already owns it.

---

# 18. 40A.12 — Keepsake Handoff

Expose explicit:

```text
survivor_id → keepsake_item_id
```

for:

- shelter decor;
- heirloom logic;
- memorial surfaces;
- Plan 41 memory.

Ownership comes from the survivor identity record, not from the item ID string.

---

# 19. 40A.13 — Phantom Background Handoff

Expose a typed query/interface for Plan 41.

If Plan 41 is not yet live, stop at the seam; do not invent a temporary second memory system.

---

# 20. 40A.14 — Port Contract Alignment

Treat identity sinks as Plan 36 contract examples.

Where `RegisterBelief` or equivalent is host-bound, require:

- stable port ID;
- owner;
- expected caller;
- setup-time binding;
- runtime validation.

The identity layer should be impossible to forget in a new composition root.

---

# 21. 40A.15 — Semantic Events

Use Plan 31 canonical kinds for runtime-significant identity consequences.

Distinguish:

```text
identity registration (diagnostic/internal)
friction ignition (player-significant)
```

Avoid briefing spam from routine setup.

---

# 22. 40A.16 — Save Compatibility

Static authored identity is config, not duplicated save state.

Old save flow:

```text
load current authored identity
→ restore old dynamic social state
→ validate references
```

No new required save field solely to persist data that already lives in catalogs.

---

# 23. 40A Tests

Minimum:

- valid enrichment parse;
- malformed diagnostics;
- missing required catalog failure;
- authored belief matches data;
- explicit missing-belief policy;
- invalid profile integrity failure;
- unused profile rule;
- profession single-authority rule;
- exactly-once registration;
- restore does not clobber identity;
- old save load;
- heuristic source-scan failure fixture;
- port contract completeness;
- profession consumer correctness.

---

# 24. 40A Definition of Done

- [ ] identity authority ADR;
- [ ] profession fork resolved;
- [ ] enrichment loader live;
- [ ] identity loaded at setup;
- [ ] `InferBeliefProfile` deleted;
- [ ] missing-belief rule explicit;
- [ ] identity integrity tier;
- [ ] exactly-once registration;
- [ ] restore ordering proven;
- [ ] profession consumers use canonical field;
- [ ] keepsake handoff;
- [ ] phantom-background handoff;
- [ ] port contract green;
- [ ] old saves load;
- [ ] enrichment catalog produces runtime evidence.

---

# 25. Task 40B — Item Tags Replace Hardcoded ID Lists

## Goal

Make the authored item-tag layer the generic behavior classification mechanism so adding a new item with the correct tag requires no consumer code change.

---

# 26. 40B.1 — Publish the Hardcoded-Semantics Inventory

Start from source-highlighted sites and scan wider.

Classify each occurrence:

| Site | IDs/pattern | Behavior meaning | Correct authority | Action |
|---|---|---|---|---|

Classes:

- generic behavior category → tag/property;
- ownership/reference → explicit reference;
- presentation-only asset mapping → may remain explicit;
- truly unique item behavior → explicit ID allowed with rationale.

This prevents overcorrecting every ID lookup indiscriminately.

---

# 27. 40B.2 — Define the Tag Vocabulary

Create one reviewed, enumerable vocabulary in data.

Each tag has:

- stable snake_case ID;
- semantic definition;
- optional category;
- expected consumer domains.

No free-text tags.

---

# 28. 40B.3 — Promote Tags Into Canonical Item Definitions

Extend item loading/resolution so consumers can ask:

```csharp
item.HasTag(tagId)
```

or repository-equivalent.

Preferred resolved model:

```text
base item definition
+ enrichment tags
→ canonical runtime ItemDefinition
```

Do not create a parallel runtime item catalog solely for tags.

---

# 29. 40B.4 — Medical Item Migration

Replace literal medical item chains with tag/property queries.

Two mandatory tests:

### Equivalence

Existing authored items behave identically before/after migration.

### Extensibility

A fixture item with the correct tag is recognized with **zero code changes**.

The extensibility assertion is the core value of the task.

---

# 30. 40B.5 — Food/Consume Migration

Replace generic food ID lists where behavior is category-based.

Coordinate with Plan 22A single consume path and existing nutrition categories.

Do not create duplicate `food` semantics if nutrition authority already exposes them.

---

# 31. 40B.6 — Keepsake Migration

Delete behavior based on:

```text
StartsWith("item_personal_keepsake_")
```

Use:

```text
survivor.keepsake_item_id
→ item catalog
```

Optional `tag_keepsake` may describe category, but not owner identity.

---

# 32. 40B.7 — Medical/Decontamination/Pharma Audit

Audit:

- MedicalWardSystem;
- MedicalTreatmentCatalog;
- DecontaminationSystem;
- PharmaLabSystem;
- crafting medical categories.

Generic item families become tags/properties.

Unique mechanics remain explicit IDs only with documented reason.

---

# 33. 40B.8 — Faction Classification Repair

A hardcoded faction-ID list is the same disease but not necessarily an item-tag problem.

Use canonical faction alignment/category data.

Do not misuse item tags as a universal metadata dumping ground.

---

# 34. 40B.9 — Tag Integrity Tier

Validate:

- every referenced tag exists;
- every rule tag exists;
- every defined production tag is used by an item or rule;
- duplicate tags fail;
- deprecated tags are explicit.

Unused tags are removed or intentionally marked—not silently tolerated.

---

# 35. 40B.10 — Literal-ID Regression Gate

Add a narrow source scanner for domains migrated to tags.

Fail new generic behavior checks such as:

```text
item.Id == "bandage"
itemId is "x" or "y"
StartsWith("item_personal_keepsake_")
```

Allow:

- tests;
- presentation asset maps;
- documented unique-item mechanics.

---

# 36. 40B.11 — Extension Fixture

Add a synthetic item using an existing tag and prove all relevant consumers recognize it.

This doubles as future mod-surface proof without implementing the mod wave here.

---

# 37. 40B.12 — Docs/Agent Rules

Update data/agent docs:

```text
generic behavior → tag/property
unique entity behavior → explicit ID with rationale
ownership → explicit reference
```

Prevent future coding agents from recreating literal lists.

---

# 38. 40B.13 — Runtime Content Evidence

Re-run content utilization.

The item-tag catalog should move from merely authored/exempt to runtime consumed/effect produced.

---

# 39. 40B Tests

- tag vocabulary parse;
- unknown-tag rejection;
- unused-tag rejection;
- item-definition tag projection;
- medical equivalence;
- food equivalence;
- keepsake reference behavior;
- decontamination/pharma category behavior;
- extension fixture;
- literal-ID regression gate.

---

# 40. 40B Definition of Done

- [ ] behavior ID-list inventory;
- [ ] canonical tag vocabulary;
- [ ] tags first-class on item definitions;
- [ ] medical generic lists removed;
- [ ] food generic lists removed where appropriate;
- [ ] keepsake ID parsing removed;
- [ ] other generic category hacks migrated;
- [ ] faction classification uses faction authority;
- [ ] tag integrity tier;
- [ ] literal-ID regression gate;
- [ ] extension fixture passes;
- [ ] docs updated;
- [ ] tag catalog runtime-consumed.

---

# 41. Task 40C — Make Identity Observable

## Goal

Make survivor identity discoverable through in-fiction evidence while preserving uncertainty and avoiding an omniscient compatibility dashboard.

---

# 42. 40C.1 — Write the Diegetic Rule First

Add to `SURVIVOR_IDENTITY.md`:

> The player learns personality through what survivors say, refuse, notice, choose, remember, and mourn—not through a numeric compatibility meter.

All later UI and narrative work must satisfy this rule.

---

# 43. 40C.2 — Separate Authored Truth From Player Knowledge

Maintain two layers:

```text
authored/simulation identity
player-known identity
```

The UI must not expose every belief/profile field at campaign start.

---

# 44. 40C.3 — Identity Knowledge States

Use a compact state model appropriate for survivor identity, for example:

```text
Unknown
Suspected
Observed
Known
```

Do not mechanically copy geographic knowledge states if unnecessary.

Persist only if this knowledge is player progression rather than derivable presentation.

---

# 45. 40C.4 — Discovery Sources

Identity knowledge may advance through:

- shared shifts;
- meals;
- ration conflicts;
- crises;
- caregiving;
- grief/memorial events;
- later conversation/memory systems.

Where practical, record the source event/day.

---

# 46. 40C.5 — Relations Read Model Carries Reasons

Audit the existing social read model.

Ensure it can expose player-known reasons such as:

- ideological friction source;
- trauma-bond origin;
- ration grievance;
- same-shift history;
- recent relevant event.

Do not expose hidden causes before discovery.

---

# 47. 40C.6 — Duty Suitability Hints

Profession produces advisory in-world hints, e.g.:

```text
Worked as a fitter before the ash.
```

Requirements:

- advisory only;
- no blocking;
- no raw profession ID;
- no numeric compatibility score;
- localized through Plan 25.

---

# 48. 40C.7 — Friction Attribution

When ideological friction changes morale/relations:

```text
survivor pair
→ authored belief profiles
→ friction event
→ canonical social effects
→ briefing/journal reason when known
```

Use Plan 31 semantic events.

---

# 49. 40C.8 — Grievance Visibility

Ration/social grievances should be traceable where they matter:

- relations read model;
- briefing when significant;
- journal/history.

Avoid silent affinity changes.

---

# 50. 40C.9 — Keepsakes as Identity Objects

A keepsake presentation should resolve:

```text
owner survivor
item display name
known meaning/memory
current role (decor/memorial/etc.)
```

Never show or infer meaning from raw item IDs.

---

# 51. 40C.10 — Heirloom Catalog Decision

`DwellerHeirloomCatalog` is test-only but likely authored intent.

Before deleting:

1. compare content against enrichment keepsakes;
2. identify duplicate facts;
3. preserve unique authored narrative data;
4. choose one canonical reference model;
5. wire useful fields into Plan 41/decor/memorial flow;
6. remove duplicate authority if overlap is exact.

---

# 52. 40C.11 — Procedural Eulogy Engine Decision

`ProceduralEulogyEngine` is unwired and emotionally important.

Treat as an integration candidate for Plan 41/memorial flow.

If activated:

- consume canonical survivor identity;
- use keyed/localized templates;
- route grief/memory through existing authorities;
- do not create new grief state.

If deferred:

- assign explicit plan/owner;
- keep it out of “dead cleanup” sweeps until that decision is made.

---

# 53. 40C.12 — Survivor Detail Projection

Show only identity facts the player has earned or that are naturally public:

- profession/background cues;
- known keepsake;
- known grievances;
- known bonds;
- observed ideological cues.

Forbidden presentation:

```text
Belief: X
Compatibility: 73%
```

---

# 54. 40C.13 — Relations Panel

Use:

- reasons;
- text labels;
- icon/shape support;
- recent cause/event.

No color-only affinity meaning.

Keyboard/focus support from Plan 37B applies.

---

# 55. 40C.14 — Briefing and Journal

Surface only significant social changes.

Rate-limit repetitive micro-events.

Prefer:

```text
new conflict
major grievance
relationship reversal
memorial/grief consequence
```

over every tiny affinity delta.

---

# 56. 40C.15 — Tone Gate

Apply narrative checks:

- restrained;
- concrete;
- non-moralizing;
- no omniscient psychoanalysis;
- enough variation to avoid repetition fatigue.

All strings are localization keys/overlays, not new C# prose.

---

# 57. 40C.16 — Accessibility

Belief/grudge/relationship state must use:

- text;
- icon/shape;
- accessible names;
- visible keyboard focus.

Never color alone.

---

# 58. 40C.17 — Snapshot Matrix

Create snapshots/read-model fixtures for:

- stable group;
- ideological conflict pair;
- ration grievance;
- keepsake/memorial case;
- populated survivor detail.

Include pseudo-locale/scale variants where available.

---

# 59. 40C Tests

- relations read model includes reasons;
- hidden cause remains hidden until discovered;
- profession hint is advisory;
- friction event reaches briefing;
- grievance reaches journal/read model;
- keepsake resolves owner/item;
- no `StartsWith("item_personal_keepsake")` meaning in UI;
- no raw belief/profile IDs rendered;
- identity knowledge transitions;
- accessibility labels;
- populated-cast snapshots.

---

# 60. 40C Definition of Done

- [ ] diegetic identity rule documented;
- [ ] authored vs known identity separated;
- [ ] discovery sources defined;
- [ ] relations reasons exposed;
- [ ] profession suitability hints;
- [ ] friction attribution;
- [ ] grievance observability;
- [ ] keepsakes visible by owner/name;
- [ ] heirloom catalog disposition;
- [ ] eulogy engine disposition;
- [ ] survivor detail non-omniscient;
- [ ] journal/briefing integration;
- [ ] no raw identity IDs in UI;
- [ ] no compatibility meter;
- [ ] tone gate;
- [ ] accessibility;
- [ ] snapshot matrix.

---

# 61. Integrated Identity Pipeline

```text
survivors.json
   │
   ├─ profession
   ├─ base traits
   └─ canonical survivor id
   │
identity enrichment
   │
   ├─ belief profile
   ├─ keepsake reference
   └─ phantom background
   │
   ▼
ExpansionEnrichmentCatalog
   │
   ▼
setup-time identity registration
   │
   ├─ ideological friction
   ├─ duty suitability
   ├─ apprenticeship
   ├─ keepsake/decor
   ├─ memory/eulogy handoff
   └─ later inner-life systems
           │
           ▼
      semantic events
           │
           ▼
   player-known identity
           │
      ┌────┼────┐
      ▼    ▼    ▼
relations journal survivor detail
```

---

# 62. Identity Authority Matrix

Every identity field must answer:

```text
Who authors it?
Who loads it?
Who queries it?
Is it static or dynamic?
Is it saved?
Who may present it?
When does the player know it?
```

No identity field may have two conflicting answers.

---

# 63. Static vs Dynamic Save Contract

Static identity:

```text
catalog-backed
not duplicated in save
```

Dynamic social state:

```text
save-backed
restored after identity registration
```

This prevents save files from becoming a stale second personality authority.

---

# 64. Tag Semantics Contract

A tag describes a generic property/category.

Examples:

```text
medical_antidote
food_preservable
keepsake
```

It does **not** encode:

- survivor ownership;
- unique story identity;
- faction identity when a faction catalog already owns that fact.

---

# 65. Explicit-ID Exception Policy

Explicit item IDs are permitted only when:

- behavior is intentionally unique to that exact item;
- a category/property would be misleading;
- rationale is documented.

Generic classes must use tags/properties.

---

# 66. Content Utilization Contract

For identity/tag catalogs track:

```text
authored
→ loaded
→ read
→ effect produced
→ player observed
```

The closure report should show both enrichment and tag catalogs advancing out of inert/exempt status.

---

# 67. Port Contract Integration

Plan 36 should report required identity seams as bound.

Examples conceptually:

```text
survivor_identity.register_belief
survivor_identity.keepsake_reference
survivor_identity.profession_consumer
```

Use actual stable project IDs.

---

# 68. Lifecycle Contract

On New Game/Load:

```text
load identity catalog
→ build survivor identities
→ bind social consumers
→ restore dynamic social state
→ validate ports
→ rebind UI
```

No double registration and no stale identity projection.

---

# 69. Event Contract

Identity consequences should carry stable IDs and causes, never localized prose.

Example:

```text
friction_ignited
survivor_a
survivor_b
cause/profile pair
resulting social effect
```

Presentation resolves the text later.

---

# 70. Failure Modes

## Authored belief exists but heuristic still wins

Delete heuristic; source scan must fail reintroduction.

## Missing identity becomes empty string

Use explicit default/exclusion policy.

## Restore overwrites authored belief

Separate static config from dynamic state.

## Profession remains in two authorities

Fail integrity after migration.

## New tagged item still needs code change

Extension fixture exposes incomplete migration.

## Keepsake owner still parsed from ID

Use explicit reference.

## UI reveals all beliefs immediately

Use known-identity projection.

## Relations UI is only numbers

Add attributable reasons and in-fiction cues.

## Heirloom/eulogy code deleted as dead

Stop and perform authored-intent disposition first.

---

# 71. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| profession mismatches | Medium | Medium | mismatch report + ADR |
| missing identity silently tolerated | Medium | High | required/optional policy |
| restore-order regression | Medium | High | dedicated round-trip test |
| tag vocabulary proliferation | Medium | Medium | reviewed definitions |
| literal-ID scan false positives | Medium | Low | scoped scanner |
| over-tagging unique behavior | Low–Med | Medium | explicit-ID exception policy |
| player omniscience | Medium | High | known-identity layer |
| social event spam | Medium | Medium | significance/rate limiting |
| heirloom catalog duplicates facts | Medium | Medium | canonical comparison |
| downstream plan bypasses identity authority | Medium | High | docs + port/integrity gates |

---

# 72. Commit Strategy

## C2[17].1 — baseline + identity ADR
## C2[17].2 — profession fork resolution
## C2[17].3 — enrichment loader
## C2[17].4 — authored belief registration
## C2[17].5 — remove inference + missing-policy gate
## C2[17].6 — identity integrity + restore-order tests
## C2[17].7 — profession/keepsake/phantom handoffs
## C2[17].8 — port/event/content-utilization closure

### Gate: 40A complete

## C2[17].9 — behavior-ID inventory + tag vocabulary
## C2[17].10 — item-definition tag projection
## C2[17].11 — medical/food migrations
## C2[17].12 — keepsake/reference migration
## C2[17].13 — remaining generic-category migrations
## C2[17].14 — tag integrity + literal-ID gate
## C2[17].15 — extension fixture + docs/utilization

### Gate: 40B complete

## C2[17].16 — diegetic identity knowledge model
## C2[17].17 — relations reasons + duty hints
## C2[17].18 — friction/grievance observability
## C2[17].19 — keepsake/heirloom/eulogy decisions
## C2[17].20 — survivor detail/journal/briefing projection
## C2[17].21 — accessibility/tone/snapshots

### Gate: 40C complete

## C2[17].22 — Wave-6 identity closure

---

# 73. Verification Checklist

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
python3 scripts/ci/generate-port-contract.py --check
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical:

```text
ashfall-narrative-check
identity heuristic source scan
literal-item-ID behavior scan
relations/survivor-detail snapshot diff
```

---

# 74. Flagship Definition of Done

## 40A

- [ ] one identity authority per field;
- [ ] profession fork removed;
- [ ] enrichment loader live;
- [ ] authored belief registration;
- [ ] no personality inference heuristic;
- [ ] explicit missing-belief rule;
- [ ] integrity tiers;
- [ ] exactly-once setup registration;
- [ ] restore cannot clobber identity;
- [ ] keepsake/phantom/profession handoffs;
- [ ] old saves load;
- [ ] port contract green;
- [ ] enrichment catalog effect-produced.

## 40B

- [ ] behavior ID-list inventory;
- [ ] reviewed tag vocabulary;
- [ ] tags first-class on item definitions;
- [ ] generic medical/food lists migrated;
- [ ] keepsake string parsing removed;
- [ ] other generic category hacks migrated;
- [ ] tag integrity;
- [ ] literal-ID regression gate;
- [ ] no-code extension fixture;
- [ ] tag catalog effect-produced.

## 40C

- [ ] diegetic identity rule;
- [ ] authored vs known identity separated;
- [ ] discovery channels;
- [ ] relations reasons;
- [ ] advisory duty hints;
- [ ] friction/grievance attribution;
- [ ] keepsake presentation;
- [ ] heirloom/eulogy disposition;
- [ ] non-omniscient survivor detail;
- [ ] journal/briefing integration;
- [ ] no raw identity IDs in UI;
- [ ] no compatibility meter;
- [ ] accessibility;
- [ ] narrative/tone gate;
- [ ] snapshot matrix.

## Global

- [ ] no new inferred identity;
- [ ] no second profession/belief authority;
- [ ] generic behavior is property/tag-driven;
- [ ] downstream inner-life plans use this plumbing;
- [ ] full verification green.

---

# 75. Closure Report Template

```markdown
## C2[17] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Survivor definitions:
- Enrichment survivor entries:
- Authored belief coverage:
- Missing beliefs:
- Duplicate profession rows:
- Heuristic identity sites:
- Item tags:
- Hardcoded generic ID lists:
- Keepsake pattern sites:
- Enrichment utilization:

### 40A — Identity Authority
- Profession owner:
- Loader:
- Belief registration:
- Heuristic removed:
- Missing policy:
- Identity integrity:
- Restore ordering:
- Keepsake handoff:
- Phantom handoff:
- Profession consumers:
- Port contract:
- Save compatibility:
- Result:

### 40B — Item Tags
- Tag vocabulary:
- ItemDefinition integration:
- Medical migration:
- Food migration:
- Keepsake migration:
- Other consumers:
- Tag integrity:
- Literal-ID gate:
- Extension fixture:
- Runtime utilization:
- Result:

### 40C — Observability
- Diegetic rule:
- Knowledge states:
- Relations reasons:
- Duty hints:
- Friction attribution:
- Grievance visibility:
- Keepsakes:
- Heirloom catalog:
- Eulogy engine:
- Survivor detail:
- Journal/briefing:
- Accessibility:
- Tone:
- Snapshots:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Content utilization:
- Port contract:
- Triad gate:
- Narrative check:
- Verify fast:

### Final Metrics
- HEURISTIC_IDENTITY_SITES:
- UNRESOLVED_BELIEF_IDS:
- DUPLICATE_PROFESSION_AUTHORITIES:
- HARD_CODED_GENERIC_ITEM_ID_LISTS:
- UNDEFINED_TAGS:
- UNUSED_TAGS:
- IDENTITY_CATALOG_EFFECT_PRODUCED:
- TAG_CATALOG_EFFECT_PRODUCED:

### Remaining Debt
- Identity:
- Tags:
- Heirlooms:
- Eulogy:
- Memory:
- Relations:
```

---

# 76. Final Execution Directive

Execute Plan 40 as an identity-authority repair.

Critical sequence:

```text
choose one owner for every identity fact
→ load authored enrichment
→ register beliefs at setup
→ delete inference
→ validate coverage/restore ordering
→ make tags first-class
→ replace generic literal-ID behavior
→ expose identity through consequences and memory
→ preserve information scarcity
```

Do not write another `InferBeliefProfile`.

Do not let duplicate `profession` fields survive because they happen to agree today.

Do not parse keepsake ownership from a naming convention.

Do not replace a hardcoded ID list with a subsystem-local tag vocabulary nobody else understands.

The strongest identity rule is:

> **If a survivor fact is authored, runtime systems read it; they do not infer a parallel truth.**

The strongest item rule is:

> **Generic behavior keys off declared properties/tags, not literal IDs.**

The strongest presentation rule is:

> **The player learns who people are through what they do, remember, refuse, value, and mourn—never through an omniscient compatibility meter.**

The flagship acceptance scenario is:

> **Load a populated campaign, place two survivors with authored conflicting beliefs into a shared social/work context, and observe an attributable friction consequence whose cause traces back to authored identity. Then add a fixture medical/food/keepsake item using only canonical tags/references and verify every relevant consumer recognizes it without a code edit.**
