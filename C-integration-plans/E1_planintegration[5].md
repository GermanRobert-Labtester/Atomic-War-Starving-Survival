---
PLAN_ID: E1-5
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 5
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 140 — Generational Legacy & Campaign Inheritance"
SEQUENCE_FILENAME: "E1_planintegration[5].md"
PREVIOUS_FILENAME: "E1_planintegration[4].md"
NEXT_FILENAMES:
  - "E1_planintegration[6].md"
  - "E1_planintegration[7].md"
CATEGORY: LINK+LEGACY+CONTINUATION+SAVE_BOUNDARY
PRIMARY_INTENT: "Create a bounded multi-generational legacy layer that carries records, lineage, memory, and optional narrative conditions across campaigns without silently converting ASHFALL into a power-stacking New Game+."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
RECORDS_BEFORE_BONUSES: true
CLEAN_START_FIRST_CLASS: true
PRIOR_SAVE_LIVE_DEPENDENCY_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: VERY_HIGH
SCOPE_RISK: VERY_HIGH
---

# E1 Plan Integration [5] — Generational Legacy, Campaign Memory, Lineage, and Bounded Inheritance

> **Sequence rule:** this file is `E1_planintegration[5].md`.
> The next files are `E1_planintegration[6].md`, `E1_planintegration[7].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan expands Plan 140 into an implementation-grade legacy and continuation programme while reconciling a
major architectural tension: the source plan proposes persistent stat bonuses, shelter upgrades, faction
standing modifiers, starting resources, and multi-campaign trait stacking, while the preceding continuation
architecture establishes a safer principle — **records and remembered world conditions cross campaign
boundaries; mechanical inheritance does not happen by default**.

The player-value goal remains valid and strong: campaigns should matter after their endings. Survivors should
leave names, relationships, lineages, remembered choices, unresolved obligations, places, reputations, and
stories that later campaigns can encounter. Generational succession should connect to those records. A child
or successor may meaningfully belong to a lineage. A faction may remember an old alliance or grievance. A
ruined outpost may still be known. An ending can shape the next campaign's historical context.

However, long-term meaning does not require automatic power inflation. A +20% resource start, persistent
fortification, stacking stat bonuses, inherited recipes, or five generations of cumulative multipliers can
turn a survival-management game into a meta-progression treadmill and invalidate campaign balance.

Therefore E1-5 defines four layers:

1. **Campaign record** — immutable summary of what happened.
2. **Lineage and historical memory** — identity, names, relationships, sites, obligations, faction memory.
3. **Continuation seed** — bounded starting conditions derived once when a new campaign is created.
4. **Optional mechanical inheritance** — prohibited by default and admitted only through explicit balance
   decisions, caps, and parity tests.

## 1. Source Plan Intent Preserved

The source plan asks to deepen `GenerationalSuccessionEngine`, integrate `CohortSystem`, endings,
`EpilogueMatrixRuntime`, faction relationships, shelter state, a cross-campaign save layer, legacy UI,
journaling, trait catalogs, deterministic inheritance, old-save compatibility, and CI validation.

It also proposes:

- survivor traits inherited probabilistically;
- shelter improvements persisting;
- faction memory affecting starting standing;
- ending-specific modifiers;
- trait synergies/conflicts;
- trait evolution over several campaigns;
- legacy quests/endings;
- campaign history;
- fresh-start option.

E1-5 keeps the narrative and continuity ambitions but subjects every mechanical inheritance rule to explicit
authority, balance, and campaign-isolation gates.

## 2. Architectural Thesis

A campaign ending should create a **record**, not a live service.

```text
Active campaign
    |
    v
Ending / extinction resolution
    |
    v
Immutable CampaignLegacyRecord
    |
    +--> lineage records
    +--> memorial/site records
    +--> faction historical memory
    +--> obligations / unresolved commitments
    +--> ending/epilogue evidence
    |
    v
Bounded cross-campaign legacy store
    |
    v
New campaign creation
    |
    +--> clean start OR continued-world start
    |
    v
ContinuationSeed
    |
    v
New campaign envelope
```

Once the new campaign is created, it must not depend on the prior campaign save.

## 3. Non-Negotiable Rules

- `GenerationalSuccessionEngine` remains the authority for within-campaign generational succession where it
  already owns that behavior.
- `CohortSystem` remains the authority for children/maturation.
- Legacy records do not duplicate live survivor state.
- Endings remain owned by ending/epilogue authorities.
- Faction standing remains owned by faction/standing systems.
- Shelter improvements remain owned by shelter systems.
- The legacy layer records historical facts and derives bounded seed inputs; it does not become a second
  faction/shelter/survivor system.
- No prior campaign is read after new-campaign initialization completes.
- Clean start remains fully supported and narratively complete.
- Mechanical bonuses are opt-in design decisions, not automatic consequences of recording history.
- No stacking bonus may grow without a hard cap.
- No trait may be inherited simply because its name sounds hereditary; source semantics must justify it.
- Learned expertise, moral reputation, family identity, and genetic/biological inheritance are distinct
  concepts and must not be conflated into one trait list.
- Death/retirement records are immutable historical evidence.
- Legacy quests reference records; they do not reopen or mutate old campaigns.
- Every legacy record and seed schema is versioned.
- All random inheritance decisions use deterministic RNG.
- Old saves without legacy state load safely.
- A reset/clean-start option must not delete history unless the player explicitly requests deletion; it should
  normally choose not to consume legacy for the new campaign.

## 4. Delivery Slices

### Slice A — Records only
End campaign → immutable record → history UI → clean/continued campaign selection.

### Slice B — Lineage and memory
Successor/mentor/family links, faction memory, graves/sites, unresolved obligations.

### Slice C — Narrative continuation
Legacy quest hooks, epilogue references, lineage-aware dialogue/event conditions.

### Slice D — Bounded seed effects
Only starting conditions already justified by continuation architecture; no raw power stacking.

### Slice E — Optional mechanical inheritance
Only after explicit decision records, balance simulations, clean-start parity, and caps.

A perfectly successful implementation may stop after Slice C or D.


---

## E1-5A — Premise verification and legacy-authority audit

**Goal:** Verify all existing succession, cohort, ending, epilogue, legacy-ledger, save, faction-memory, shelter, and continuation rails before creating new legacy state.

### Required substeps

1. Inspect `GenerationalSuccessionEngine`, `DwellerGenerationRecord`, `CohortSystem`, `HoldfastEndings`, `EpilogueMatrixRuntime`, epilogue chronicle data, faction standing/memory systems, shelter improvement state, memorial/death records, commitments, save-slot architecture, and any existing legacy ledger or cross-campaign record.
2. Search for prior plans/ADRs governing campaign continuation, records-only inheritance, campaign seed derivation, clean-start parity, and previous-save isolation.
3. Map each proposed Plan 140 field to an existing authority: ending, days survived, survivor count, deaths, faction standings, shelter improvements, traits, flags, completion day.
4. Mark fields that should be captured as summary evidence versus fields that should never be copied wholesale.
5. Verify whether campaign IDs already exist and are stable.
6. Verify whether retired/dead/mentor/lineage identities have stable survivor IDs suitable for cross-campaign records.
7. Verify whether faction standings have an accepted historical-memory representation distinct from live standing.
8. Verify whether shelter state can be summarized as notable historical facts without copying entire upgrades.
9. Create `docs/systems/LEGACY_AUTHORITY_MAP.md`.
10. Create duplicate-search/intake evidence and register the plan as SYSTEM/LINK according to what remains genuinely new.
11. Set `PREMISE_VERIFIED_AT` to current HEAD.
12. Stop if another accepted continuation/legacy authority already owns the cross-campaign record.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5B — Legacy ADR: records, lineage, seed, and mechanical inheritance boundary

**Goal:** Write the architectural decision that prevents narrative continuity from silently becoming unlimited meta-progression.

### Required substeps

1. Define four layers explicitly: CampaignLegacyRecord, LineageRecord, ContinuationSeed, OptionalMechanicalInheritance.
2. State that CampaignLegacyRecord is immutable after finalization except migration/repair.
3. State that ContinuationSeed is created once and copied into a new campaign initializer.
4. State that the active new campaign cannot query the previous save.
5. Define clean start as consuming no legacy seed while keeping the historical archive available unless the user deletes it.
6. List mechanical inheritance proposals from Plan 140 and classify each as PROHIBITED_DEFAULT, NARRATIVE_ONLY, STARTING_CONDITION, or CANDIDATE_BONUS.
7. Require an explicit decision record for every CANDIDATE_BONUS.
8. Define hard caps for any future accepted bonus category.
9. Require balance/parity tests for accepted mechanical effects.
10. Define no-bonus baseline as the default launch target.
11. Have a second tool review the ADR using only pillars, prior continuation constraints, and repository evidence.
12. Publish the ADR before code.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5C — Immutable campaign legacy record schema

**Goal:** Create the smallest cross-campaign record that can explain what happened without copying live campaign state.

### Required substeps

1. Define stable campaign ID, schema version, start/end timestamps or campaign days, ending ID, extinction/completion reason, days survived, survivor summary counts, named notable people, named deaths, key sites, major commitments, faction-history summaries, major world flags, and epilogue outcome references.
2. Represent survivors by stable identity snapshots appropriate for history: name, identity ID, generation index, relationships/mentor links, notable role tags, death/retirement state, but not full mutable stats.
3. Represent faction history as bounded relationship events or summary categories rather than full standing maps unless an accepted seed policy requires a snapshot.
4. Represent shelter history as named accomplishments/conditions rather than raw upgrade objects by default.
5. Represent major locations/outposts/memorials as bounded references.
6. Record source evidence for each notable legacy fact where useful for debugging.
7. Define deterministic ordering of collections.
8. Define retention/cap rules for named people, sites, events, and flags.
9. Define record finalization semantics so the same campaign cannot finalize twice.
10. Add schema invariants and integrity tests.
11. Version from first release.
12. Document which live fields are intentionally not stored.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5D — Campaign finalization transaction

**Goal:** Produce exactly one immutable legacy record when a campaign reaches an ending or extinction.

### Required substeps

1. Identify the authoritative end-of-campaign event and finalization boundary.
2. Generate record data through read-only queries into canonical authorities.
3. Do not let legacy code decide the ending.
4. Capture final survivor/death/retirement summaries after canonical ending resolution reaches a stable state.
5. Capture epilogue context/result references without duplicating epilogue logic.
6. Capture faction historical summaries after final standing/consequence processing.
7. Capture site/outpost/memorial records from their canonical owners.
8. Capture unresolved commitments according to continuation policy.
9. Assign a stable finalization operation ID.
10. Append the record to cross-campaign storage atomically.
11. Make finalization idempotent across save/reload/retry.
12. Add tests for normal ending, extinction, repeated finalize call, crash/reload during finalization, and missing optional authorities.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5E — Cross-campaign legacy store and save isolation

**Goal:** Persist campaign records outside individual live campaign envelopes without creating a service dependency from active campaigns.

### Required substeps

1. Identify canonical product-level/profile/slot storage boundary appropriate for records spanning campaigns.
2. Keep cross-campaign legacy data separate from active campaign state while respecting existing save root conventions.
3. Version the store and each campaign record.
4. Use checksums/atomic writes if current save architecture provides them.
5. Define migration from no legacy store.
6. Define handling of corrupt/missing individual records.
7. Prevent active gameplay systems from receiving a mutable handle to historical campaign stores.
8. Allow campaign-creation UI/initializer to read the store.
9. After seed creation, sever the dependency.
10. Add tests proving a continued campaign loads even if the previous campaign save is removed after initialization.
11. Add tests proving modifying the legacy archive later does not mutate already-created campaigns.
12. Document ownership in `docs/saves/SAVE_MODEL.md`.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5F — Lineage identity model

**Goal:** Represent generations, ancestry, mentorship, family identity, and successor links without pretending every gameplay trait is genetic.

### Required substeps

1. Audit actual parent/child/mentor relationships represented by cohort/succession systems.
2. Define lineage edges separately: biological parent, guardian, mentor, designated successor, household/family, ideological successor if needed.
3. Require stable identity IDs across historical records.
4. Define whether names/family names persist and how generated successors reference ancestors.
5. Keep survivor mutable stats out of lineage records.
6. Define notable lineage tags such as founder, child-of, mentored-by, successor-of, memorialized.
7. Define generation index semantics consistently with `DwellerGenerationRecord`.
8. Define what happens when ancestry is unknown.
9. Cap lineage depth materialized in active UI while retaining bounded historical links.
10. Add tests for parent-child, mentor-only, no-parent successor, multiple siblings, death before maturation, adoption/guardian if supported, and record migration.
11. Document difference between lineage identity and inherited gameplay effects.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5G — GenerationalSuccessionEngine integration

**Goal:** Connect cross-campaign records to existing succession without moving succession rules into the legacy layer.

### Required substeps

1. Expose read-only succession outcome events/records suitable for legacy capture.
2. Use succession authority to identify retirements, deaths, generation changes, mentors, and successors.
3. Do not let CampaignLegacySystem age or retire survivors.
4. Define which succession facts become historical records.
5. Allow a new campaign seed to reference a prior lineage only through initialization contracts.
6. Define deterministic successor-selection input if continuation creates a descendant/lineage-starting character.
7. Add tests proving succession behavior is unchanged when legacy recording is disabled.
8. Add tests proving record capture does not double-fire succession.
9. Add save/reload tests around retirement and campaign ending.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5H — Cohort and maturation integration

**Goal:** Use CohortSystem as the authority for children/maturation and carry only bounded lineage evidence across campaigns.

### Required substeps

1. Identify cohort child identity, parent links, maturation day, and maturation outcome APIs.
2. Record matured descendants as historical lineage entries when relevant.
3. Do not create cross-campaign child simulation inside the legacy layer.
4. Define how a continued campaign may start with a descendant archetype or named historical lineage reference without cloning an old survivor.
5. Keep biological trait inheritance out of this task unless a real genetics/trait framework exists.
6. Add tests for matured child, child death, no-child campaign, multiple generations, and continuation seed lineage references.
7. Ensure old campaign cohort state is never loaded into a new active campaign.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5I — Trait taxonomy: genetic, learned, reputational, institutional, and narrative

**Goal:** Replace the source plan's single LegacyTrait bucket with semantically distinct categories.

### Required substeps

1. Define `LineageIdentityTrait` for family/heritage descriptors with no mechanical effect by default.
2. Define `LearnedLegacy` for recipes/knowledge/techniques that may be transmitted only through an accepted knowledge authority.
3. Define `HistoricalReputation` for faction/world memory of past actions.
4. Define `InstitutionalLegacy` for remembered shelter institutions or doctrines, not copied physical upgrades.
5. Define `EndingLegacy` for narrative/epilogue conditions.
6. Define `CandidateMechanicalLegacy` as a separate gated type requiring balance approval.
7. Prohibit generic `effect: stat bonus` in the base trait schema.
8. Require every trait category to name its canonical consumer.
9. Add schema validation preventing a narrative-only trait from containing gameplay modifier fields.
10. Create migration path if an earlier prototype used a flat trait list.
11. Document why 'Leader's Blood' and 'Medic's Knowledge' are not the same inheritance mechanism.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5J — Deterministic lineage inheritance policy

**Goal:** Implement only lineage/identity inheritance that is semantically justified, and gate stochastic mechanics behind explicit policy.

### Required substeps

1. Reject a universal 50% per-trait rule as a default because many traits are learned, reputational, institutional, or narrative.
2. Define inheritance policy by trait category.
3. Biological/heritable traits require an actual survivor trait framework and explicit heritability metadata.
4. Learned knowledge requires mentor/training/knowledge transfer events, not RNG genetics.
5. Faction reputation transfers as historical memory through faction seed policy, not parent-child inheritance.
6. Institutional history transfers through campaign record, not bloodline.
7. Ending legacy transfers as narrative condition, not random chance.
8. Use deterministic RNG only for categories that legitimately involve stochastic inheritance.
9. Key RNG by campaign seed + lineage IDs + trait ID to ensure replay stability.
10. Add tests for category-correct inheritance and rejected invalid combinations.
11. Allow first release to have zero stochastic mechanical inheritance.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5K — Historical faction memory

**Goal:** Carry faction memory as bounded history while leaving live starting standing under canonical faction policy.

### Required substeps

1. Capture major faction relationship milestones rather than blindly persisting final numeric standings.
2. Define memory events such as allied, betrayed, treaty honored, treaty broken, aided, attacked, saved, abandoned, or unresolved debt where canonical data supports them.
3. Cap history per faction.
4. Create a faction-seed adapter that may turn approved memory into starting context.
5. Default to narrative tags/dialogue/availability rather than +20/-20 numeric standing.
6. If numeric starting-standing offsets are proposed, require explicit balance decision, clamp, decay, and clean-start parity testing.
7. Prevent faction memory from stacking without bound across ten campaigns.
8. Add tests for remembered alliance, remembered betrayal, neutral history, faction missing/removed in later data, and cap behavior.
9. Route all actual standing changes through faction authority.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5L — Shelter legacy and physical improvement boundary

**Goal:** Record shelter accomplishments without copying physical shelter upgrades into every new campaign by default.

### Required substeps

1. Capture notable shelter milestones such as survived winter, maintained power, expanded quarters, fortified perimeter, preserved archive, or maintained clinic if authoritative systems expose them.
2. Represent them as history/institutional legacy tags.
3. Do not copy exact upgrade objects or levels across campaigns by default.
4. If a continuation fiction says the same physical holdfast persists, route that through the same-campaign/multi-site continuation model rather than generic New Game+.
5. Require an ADR for any inherited physical facility.
6. Require resource-equivalence tests for any accepted starting infrastructure.
7. Prevent shelter legacy from bypassing build costs silently.
8. Add tests that no legacy tag changes shelter stats unless an explicitly approved adapter exists.
9. Document same-site continuation versus new-campaign remembered-world distinction.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5M — Ending and epilogue integration

**Goal:** Make endings create legacy evidence and allow epilogues to reference history without letting legacy traits own ending logic.

### Required substeps

1. Map every canonical ending ID to a legacy record descriptor.
2. Capture `EpilogueMatrixRuntime` output references/context fingerprints as historical evidence.
3. Do not hard-code ending effects in the legacy store.
4. Allow future epilogue text to reference prior campaign records.
5. Define whether extinction creates a distinct legacy category.
6. Create tests for all existing endings and extinction.
7. Ensure ending evaluation remains unchanged when legacy recording is disabled.
8. Ensure finalization occurs after ending/epilogue context is stable.
9. Add data-integrity validation that recorded ending IDs resolve.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5N — Continuation seed derivation

**Goal:** Derive a bounded new-campaign seed from selected records while preserving previous-save isolation.

### Required substeps

1. Define a pure deterministic `DeriveContinuationSeed(records, policy, newCampaignSeed)` function.
2. Select only approved record categories.
3. Include lineage/history identifiers, bounded faction memory, unresolved obligations, notable sites/memorials, and ending context as permitted.
4. Exclude full inventory, survivor stats, shelter upgrade objects, active jobs, active faction simulation state, and old campaign runtime references.
5. Version the seed schema independently.
6. Keep seed payload compact.
7. Add deterministic tests for identical inputs.
8. Add tests for missing/corrupt optional record entries.
9. Add clean-start seed as empty/default.
10. Add no-live-reference tests.
11. Document every seed field's consumer.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5O — Clean-start parity contract

**Goal:** Ensure a player can choose a fresh campaign without losing content quality or being treated as playing the inferior mode.

### Required substeps

1. Define clean start and continued start through one campaign initializer with different seed inputs.
2. Ensure all core systems initialize in both modes.
3. Ensure clean start has a complete narrative intro.
4. Ensure legacy-only quests/endings are additive rather than required for base completion.
5. Define mechanical parity metrics such as starting resource budget, survivor stat budget, shelter capability, recipe access, and progression speed.
6. Require any mechanical divergence to be explicitly approved.
7. Add automated parity tests.
8. Add UI copy that explains 'continue world' versus 'start clean' without framing clean as punishment.
9. Ensure achievements/progression do not force legacy mode if not intended.
10. Test switching between legacy-consuming and clean campaigns.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5P — Optional mechanical inheritance gate

**Goal:** Create a formal process for considering bonuses without putting them into the default architecture.

### Required substeps

1. Define candidate effect classes: starting-standing offset, starting resource grant, survivor stat modifier, recipe unlock, facility upgrade, capacity increase, production modifier, research modifier, travel/combat modifier.
2. Mark all candidate classes disabled by default.
3. For each proposed effect require a design rationale tied to a pillar.
4. Require a source record and semantic explanation.
5. Require a hard cap and decay/reset rule.
6. Require clean-start parity and difficulty analysis.
7. Require exploit/farming analysis.
8. Require save/versioning behavior.
9. Require a decision record and review.
10. Require feature flags/data toggles so effects can be removed without destroying history.
11. Add tests that historical records remain readable when all mechanical adapters are disabled.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5Q — Legacy trait interactions and conflict model

**Goal:** Keep synergies/conflicts in the narrative/condition layer unless mechanical interactions are separately approved.

### Required substeps

1. Define interaction rules between historical tags for dialogue, quests, epilogues, and world conditions.
2. Allow combinations such as respected military lineage + remembered betrayal to unlock narrative variants.
3. Do not automatically create hidden stacked stat formulas from tag combinations.
4. Represent interactions in data with explicit consumers.
5. Define conflict precedence and deterministic resolution.
6. Cap the number of simultaneously active narrative interaction tags.
7. Add integrity validation for unknown trait/tag IDs.
8. Add tests for synergy, conflict, missing trait, disabled consumer, and deterministic ordering.
9. Keep 'Warlord's Legacy'-style mechanical synergy out of first release.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5R — Trait evolution across campaigns

**Goal:** Model evolution as historical milestone state before considering power escalation.

### Required substeps

1. Define milestones based on number of campaigns, repeated lineage themes, fulfilled obligations, or notable outcomes.
2. Represent evolution as new historical/narrative tags.
3. Do not transform +10% bonuses into +20% bonuses by campaign count.
4. Allow evolved tags to unlock narrative events, epilogue variants, titles, or quest options.
5. Define milestone counters from immutable records.
6. Cap counters and handle deleted/corrupt records deterministically.
7. Add tests for one, three, five, and ten campaign histories.
8. Ensure milestone calculation is recomputable from records where practical.
9. Keep mechanical evolution behind E1-5P gate.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5S — Legacy quest hooks

**Goal:** Create quests that consume historical records without mutating past campaigns or requiring mechanical trait bonuses.

### Required substeps

1. Define quest predicates over lineage, sites, memorials, unresolved commitments, faction memories, and ending records.
2. Implement 'Ancestral Duty' as a current-campaign quest inspired by a recorded unfinished commitment, not a reopened old quest instance.
3. Implement 'Family Heirloom' only if an item/location record can be represented without resurrecting old inventory state.
4. Implement 'Old Enemy' as a current-world narrative/faction hook, not a direct copy of old NPC runtime state.
5. Implement 'Legacy Location' through knowledge/location rails.
6. Route all quest progress through canonical quest authority.
7. Add stable provenance linking the quest to legacy record IDs.
8. Prevent duplicate quest spawning from repeated seed derivation/reload.
9. Add tests for eligibility, duplicate guard, missing historical target, clean start, and quest completion.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5T — Legacy ending conditions

**Goal:** Allow multi-campaign history to unlock special ending variants without invalidating base endings.

### Required substeps

1. Define ending predicates over record counts/tags/lineage milestones.
2. Keep canonical ending authority responsible for eligibility/evaluation.
3. Add legacy conditions as inputs, not a parallel ending engine.
4. Ensure every base ending remains reachable without legacy mode unless design explicitly changes that.
5. Define examples such as long-running lineage, repeated fulfilled commitments, or historically reconciled factions.
6. Do not require cumulative mechanical traits.
7. Add epilogue content references for legacy variants.
8. Add tests for exact threshold, clean-start absence, corrupted history, and no duplicate ending IDs.
9. Validate ending references in data integrity.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5U — Legacy UI: history, lineage, and campaign selection

**Goal:** Present records and continuation choices without exposing implementation details or implying guaranteed bonuses.

### Required substeps

1. Audit existing campaign selection, epilogue, chronicle, journal, and lineage UI surfaces.
2. Prefer extending an existing chronicle/legacy/campaign-select surface.
3. Show completed campaigns as immutable cards with ending, days survived, notable survivors, deaths, faction memories, sites, and epilogue summary.
4. Show lineage tree/graph with bounded depth.
5. Show continuation seed preview in player-facing terms.
6. Clearly distinguish narrative memory from mechanical effects.
7. Show any approved mechanical inheritance explicitly and quantitatively.
8. Provide Start Clean and Continue World as equal choices.
9. Provide reset/ignore-legacy option without deleting archive by default.
10. Add keyboard navigation and scalable history lists.
11. Add snapshot tests for zero history, one campaign, many campaigns, corrupt/missing optional data, lineage, and continued-start preview.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5V — Legacy journal and chronicle integration

**Goal:** Use one canonical historical presentation layer rather than a second independent journal database.

### Required substeps

1. Audit epilogue chronicle and journal systems.
2. Store history facts in the legacy record; journal/chronicle surfaces project from it.
3. Do not duplicate the same campaign summary in multiple independently persisted files.
4. Define automatic entries for campaign finalization, lineage milestone, faction memory, legacy quest completion, and continuation creation.
5. Add links from current campaign to relevant historical records where UI supports it.
6. Add retention/pagination for long histories.
7. Add tests for deterministic ordering and no duplicate entries after reload.
8. Keep AI-generated saga prose as a future presentation layer over records, not part of the authoritative save.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5W — Legacy catalog/data authority

**Goal:** Create data-driven definitions for narrative legacy tags and optional mechanical adapters without hard-coding effects.

### Required substeps

1. Define schema for legacy tag ID, category, name/localization key, description, source criteria, consumers, evolution prerequisites, conflicts/synergies, and optional mechanical adapter reference.
2. Do not put arbitrary stat formulas directly into every narrative tag.
3. Separate narrative tag catalog from optional mechanical effect catalog if needed.
4. Validate source criteria against known ending/faction/location/trait IDs.
5. Create an initial corpus only after source events are verifiable.
6. Start with approximately 20 narrative tags if content demand supports it.
7. Ensure data is localization-ready.
8. Add duplicate ID, unknown source, unknown consumer, invalid evolution, circular dependency, and invalid mechanical adapter tests.
9. Document authoring rules.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5X — Old-save compatibility and migration

**Goal:** Introduce legacy recording without manufacturing fake history for campaigns that predate the system.

### Required substeps

1. Old active saves receive no synthetic completed-campaign records.
2. Existing campaign saves can begin recording legacy facts prospectively after migration.
3. Do not infer historical deaths/endings that are not actually stored.
4. Default missing legacy store to empty.
5. Version store and seed schema.
6. Define migration for any prior prototype legacy files.
7. Add tests for no legacy store, active old campaign, completed old campaign lacking evidence, partially written record, and unsupported version.
8. Make migration idempotent.
9. Ensure clean start remains available even if legacy store is corrupt.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5Y — Exploit, farming, and power-creep audit

**Goal:** Prevent repeated campaign completion, save manipulation, and cumulative traits from creating uncontrolled advantage.

### Required substeps

1. Make campaign record finalization idempotent by campaign ID.
2. Prevent the same campaign from being imported twice.
3. Prevent deleting/recreating a continuation from duplicating one-time rewards if any exist.
4. Prevent save rollback from generating multiple legacy records.
5. Prevent cyclic lineage references.
6. Prevent trait/evolution thresholds from counting duplicate records.
7. Test 10+ campaign histories for cap behavior.
8. Test max accepted mechanical adapter caps if any are enabled.
9. Test clean-start parity after large legacy histories.
10. Add property tests over random campaign histories if infrastructure permits.
11. Measure cumulative hard-power delta across generations and assert zero by default.
12. Block release if mechanical power grows without an explicit bounded policy.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-5Z — Release gate and closure

**Goal:** Ship a legacy system that makes history visible and consequential without requiring New Game+ power stacking.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run `--campaign-legacy-selftest` if introduced.
4. Run campaign finalization idempotency tests.
5. Run legacy-store migration/corruption tests.
6. Run continuation-seed determinism tests.
7. Run previous-save isolation tests.
8. Run clean-start parity tests.
9. Run lineage/cohort/succession integration tests.
10. Run faction-memory and legacy-quest tests.
11. Run 10+ campaign soak/history retention tests.
12. Verify all optional mechanical adapters are disabled unless separately approved.
13. Capture player-value metrics and completion evidence.
14. Mark DONE only when campaigns leave perceivable history and the next campaign can consume that history safely.

### Legacy boundary invariants

- Historical records never become mutable live world state.
- The prior campaign save is not a runtime dependency of the new campaign.
- Lineage identity is distinct from gameplay stat inheritance.
- Faction/shelter/survivor authorities remain canonical.
- Clean start remains valid and complete.
- Mechanical inheritance is disabled unless explicitly approved.
- All cross-campaign state is versioned and bounded.
- All stochastic inheritance uses deterministic RNG.

### Negative tests

- A campaign finalizes twice and creates duplicate records.
- A new campaign changes when the old save is edited after initialization.
- A narrative trait silently applies a stat bonus.
- Faction historical memory directly mutates standing without faction authority.
- A shelter legacy tag recreates an old physical upgrade without approval.
- Deleting one historical record breaks unrelated active campaigns.
- Ten campaigns create unbounded stacked power.
- Clean start lacks a base-game route available to continued start.

### Acceptance evidence

- [ ] Deterministic tests pass.
- [ ] Save/migration behavior is covered.
- [ ] Previous-save isolation is proven.
- [ ] Authority map remains correct.
- [ ] Clean-start parity is measured.
- [ ] History is player-visible in at least one runtime surface.
- [ ] Plan metadata and premise SHA are updated.

---

# 5. Canonical Legacy Authority Matrix

| Fact | Canonical owner | Legacy stores? | Rule |
|---|---|---:|---|
| Survivor live stats | Survivor aggregate | No | Historical snapshot may contain descriptive summary only |
| Age/retirement | Succession engine | Record outcome | Never re-simulate in legacy |
| Child maturation | Cohort system | Record outcome | No cross-campaign child simulation |
| Parent/mentor links | Succession/cohort/relations | Bounded references | Lineage identity |
| Death | Health/death/memorial authority | Historical record | Immutable |
| Ending | Ending authority | Ending ID/result | Legacy does not decide ending |
| Epilogue | Epilogue runtime | Context/result reference | Presentation/history |
| Faction standing | Faction/standing authority | History/seed input only | No direct live writes |
| Shelter upgrades | Shelter authority | Historical tag only by default | Physical inheritance gated |
| Inventory | Inventory authority | Never | No inherited raw inventory by default |
| Quests | Quest authority | Historical provenance / seed hooks | No reopened old quest instance |
| Commitments | Commitment authority | Bounded unresolved-history refs | New campaign creates new obligation instances |
| Sites/outposts | World/site authority | Historical references | Continuation seed may project ruins/memorials |
| Legacy tag | Legacy catalog | Yes | Narrative/condition data |
| Mechanical modifier | Canonical target authority | Adapter only if approved | Disabled by default |
| Campaign history | Cross-campaign legacy store | Yes | Immutable records |
| Continuation seed | Campaign initializer | Creation-time payload | No prior-save handle afterward |

---

# 6. Campaign Record Example

```yaml
schema_version: 1
campaign_id: "campaign_00017"
finalized: true
ending_id: "ending_stand_up"
completion_reason: "ending"
days_survived: 143

summary:
  survivors_alive: 11
  deaths_recorded: 7
  generations_reached: 2

notable_people:
  - historical_person_id: "hist_anna_01"
    display_name: "Anna"
    status: "retired"
    generation: 0
    roles:
      - "founder"
      - "medic"
    mentor_links:
      - "hist_..."

memorials:
  - memorial_id: "mem_..."
    person_id: "hist_..."
    location_id: "loc_..."

faction_history:
  - faction_id: "faction_..."
    tags:
      - "treaty_honored"
      - "aid_received"

institutional_history:
  - "clinic_preserved"
  - "winter_grid_maintained"

unresolved_commitments:
  - record_id: "commit_hist_..."

epilogue:
  context_fingerprint: "..."
  result_id: "..."

legacy_tags:
  - "legacy_community_reputation"
```

This record is historical evidence. It is not an active campaign state dump.

---

# 7. Legacy Trait Taxonomy

## 7.1 Lineage identity

Examples:

- founder's line;
- child of a named survivor;
- mentored by a renowned medic;
- successor to a shelter leader;
- family linked to a memorial site.

Default effect: narrative identity and conditions.

## 7.2 Learned legacy

Examples:

- a recipe taught by a mentor;
- a medical doctrine preserved in records;
- a survival technique transmitted through training.

Default effect: knowledge unlock only when the canonical knowledge/skill system explicitly supports
transmission.

## 7.3 Historical reputation

Examples:

- remembered ally;
- remembered betrayal;
- treaty honored;
- settlement rescued;
- faction convoy attacked.

Default effect: narrative/faction policy input. Numeric standing offset requires separate approval.

## 7.4 Institutional legacy

Examples:

- shelter known for strict duty schedules;
- renowned clinic tradition;
- archive-preserving community;
- hardened defensive culture.

Default effect: narrative/event/quest conditions. Physical/stat bonuses require approval.

## 7.5 Ending legacy

Examples:

- ending category;
- political/social character of the ending;
- unresolved consequences.

Default effect: epilogue/continuation narrative inputs.

## 7.6 Candidate mechanical legacy

Examples from the source plan:

- +10% morale;
- +10% scavenging;
- +20% defense;
- -10% resource consumption;
- starting items;
- +20 faction standing;
- trade discounts;
- research speed;
- expedition speed.

All disabled by default. Each requires the mechanical inheritance gate.

---

# 8. Mechanical Inheritance Decision Template

```markdown
## Mechanical Legacy Decision

- Candidate effect:
- Source historical record:
- Canonical target authority:
- Player-facing rationale:
- Why narrative-only is insufficient:
- Base-game pillar supported:
- Starting power delta:
- Campaign difficulty delta:
- Maximum stack/cap:
- Decay/reset rule:
- Clean-start parity impact:
- Exploit/farming risk:
- Save/versioning impact:
- Feature flag / rollback:
- Tests:
- Reviewer:
- Decision: ACCEPT / REJECT / DEFER
```

No mechanical effect should be implemented without this record.

---

# 9. Continuation Seed Example

```yaml
seed_version: 1
source_campaign_ids:
  - "campaign_00017"

lineage:
  historical_family_ids:
    - "family_anna"

remembered_sites:
  - location_id: "loc_old_waystation"
    memory_kind: "ruined_outpost"

faction_memory:
  - faction_id: "faction_north"
    tags:
      - "old_alliance"

starting_obligations:
  - template_id: "legacy_debt_followup"
    provenance_record_id: "commit_hist_..."

ending_context:
  previous_ending_id: "ending_stand_up"

narrative_tags:
  - "legacy_community_reputation"

mechanical_adapters: []
```

The empty `mechanical_adapters` list is the expected default.

---

# 10. Campaign Boundary Invariants

The following must be tested as architectural guarantees:

1. A continued campaign does not retain a live file handle or repository reference to the previous save.
2. Deleting the previous campaign save after initialization does not break the continued campaign.
3. Modifying the previous campaign after initialization does not alter the continued campaign.
4. A historical record cannot be mutated by active gameplay.
5. A campaign record is finalized once.
6. Clean start uses no legacy seed.
7. Historical archive visibility does not imply legacy bonuses.
8. Legacy quests create new current-campaign quest instances.
9. Faction memory is an input to faction policy, not a direct standing variable.
10. Shelter history is not a physical upgrade object.
11. Lineage identity does not equal stat inheritance.
12. Mechanical adapters can all be disabled without invalidating records.

---

# 11. Source-Plan Mechanical Proposals — Reclassification

| Source proposal | E1-5 classification | Default |
|---|---|---|
| Leader's Blood +10% morale | Candidate mechanical legacy | Disabled |
| Survivor's Instinct +10% scavenging | Candidate mechanical legacy | Disabled |
| Medic's Knowledge recipe unlock | Learned legacy | Possible if knowledge rail exists |
| Warrior's Training +10% combat | Candidate mechanical legacy | Disabled |
| Fortified Walls +20% defense | Physical inheritance | Disabled |
| Efficient Systems -10% resource use | Candidate mechanical legacy | Disabled |
| Expanded Quarters +5 capacity | Physical inheritance | Disabled |
| Hidden Cache starting items | Candidate mechanical legacy | Disabled |
| Old Alliance +20 standing | Historical reputation | Narrative by default |
| Ancient Grudge -20 standing | Historical reputation | Narrative by default |
| Ending resource/efficiency modifiers | Candidate mechanical legacy | Disabled |
| Trait synergies | Narrative interactions first | Allowed narrative-only |
| Trait evolution | Historical milestone first | Allowed narrative-only |
| Legacy quests | Narrative continuation | Allowed |
| Legacy endings | Ending predicates | Allowed |
| Campaign history UI | Presentation | Allowed |
| Fresh-start option | Core requirement | Required |

---

# 12. Lineage Model

Recommended identity graph:

```text
HistoricalPerson
    ├─ biological_parent_of ─> HistoricalPerson
    ├─ guardian_of ──────────> HistoricalPerson
    ├─ mentor_of ────────────> HistoricalPerson
    ├─ successor_of ─────────> HistoricalPerson
    └─ member_of_family ─────> HistoricalFamily
```

Do not force every relation into parentage.

A successor may inherit a name, obligation, or reputation without inheriting DNA or stats.
A child may inherit biological traits without inheriting a mentor's learned medical knowledge automatically.
A faction may remember a family even when the active survivor is not a direct descendant.

---

# 13. Historical Record Retention

Ten or twenty campaigns can produce enormous records if every daily event is persisted.

Classify retained data:

### Permanent bounded
- campaign summary;
- ending;
- named founder/major survivors;
- named deaths/memorials;
- major faction milestones;
- major site history;
- lineage anchors;
- legacy quest provenance.

### Summarized
- aggregate deaths;
- days survived;
- generation count;
- broad faction relationship category;
- institutional achievements.

### Not retained cross-campaign
- daily needs;
- individual inventory transactions;
- all production jobs;
- all weather ticks;
- every standing change;
- every temporary affliction;
- routine patrol events.

Set hard per-campaign and total archive caps.

---

# 14. Legacy Quest Safety Rules

Legacy quests must:

- reference historical provenance;
- instantiate fresh current-campaign state;
- validate all referenced locations/factions/items;
- tolerate missing or retired content IDs;
- never reopen old save sections;
- never mutate historical records;
- deduplicate by provenance + quest template;
- remain optional unless a pillar explicitly says otherwise;
- have clean-start alternatives where needed for core progression.

---

# 15. Legacy Ending Safety Rules

Legacy endings may:

- require historical tags;
- require lineage milestones;
- require multiple campaign completions;
- reference prior outcomes;
- use new epilogue text.

Legacy endings may not:

- require cumulative stat bonuses;
- invalidate all base endings;
- require the previous save to exist;
- depend on unbounded history traversal during runtime;
- mutate old records.

---

# 16. Clean vs Continued Start Parity Matrix

| Dimension | Clean start | Continued start | Default requirement |
|---|---|---|---|
| Starting food/resources | Baseline | Baseline | Equal |
| Survivor stat budget | Baseline | Baseline | Equal |
| Shelter capabilities | Baseline | Baseline | Equal |
| Recipes | Baseline | Baseline | Equal unless approved learned legacy |
| Faction raw standing | Baseline | Baseline | Equal by default |
| Narrative context | Fresh | Remembered history | Different |
| Legacy quests | None/history-independent | Available where records qualify | Additive |
| Historical sites | Fresh world | May include bounded remembered ruins/memorials | Different |
| Obligations | Baseline | May include approved inherited obligations | Different |
| Difficulty | Baseline | Context-dependent | Not automatically easier |
| Previous-save dependency | None | None after initialization | Equal |

---

# 17. Suggested Metrics

Track:

- campaigns finalized successfully;
- duplicate finalization attempts blocked;
- average record size;
- archive size after 1/3/5/10 campaigns;
- notable people retained per campaign;
- lineage depth;
- legacy tags generated;
- continued-world selection rate;
- clean-start selection rate;
- time to first perceived historical callback;
- legacy quest discovery/completion;
- faction historical-memory usage;
- memorial/site-memory discovery;
- legacy ending eligibility;
- mechanical adapters enabled count;
- hard-power delta clean vs continued;
- previous-save isolation test failures;
- migration/corruption recovery events.

The central player-value metric is whether the player notices and understands that prior campaigns mattered,
not how many bonuses they accumulated.

---

# 18. Selftest Scenario

A headless `--campaign-legacy-selftest` should:

1. create a deterministic campaign state;
2. create one founder, one child, one mentor link, one death, one faction milestone, one notable site;
3. resolve a known ending;
4. finalize exactly one record;
5. save and reload the legacy store;
6. derive a continuation seed;
7. create a new campaign from the seed;
8. assert lineage/history inputs appear;
9. assert no prior-save runtime reference exists;
10. assert no unapproved mechanical modifiers are active;
11. assert clean-start initializer produces baseline power;
12. delete the prior save and reload the new campaign successfully;
13. test one legacy quest predicate;
14. test one legacy ending predicate;
15. verify archive retention/caps.

---

# 19. Exploit Matrix

| Exploit | Guard |
|---|---|
| Finish same campaign repeatedly | Finalization operation/campaign ID |
| Roll back save before ending | Record-store idempotency |
| Duplicate-import old record | Stable campaign ID uniqueness |
| Create/delete continuation for rewards | No one-time reward by default; transaction journal if ever added |
| Stack faction bonuses forever | Disabled by default; cap/decay if approved |
| Stack shelter bonuses forever | Disabled by default |
| Trait lineage cycle | Graph validation |
| Count same ancestor twice | Stable historical-person IDs |
| Remove bad record to improve seed | Seed provenance/versioning; player-controlled deletion policy explicit |
| Corrupt history blocks clean start | Clean start independent |
| Mechanical adapter removed later | Records remain valid without adapter |

---

# 20. Performance and Scale

The legacy layer should not be on the hot gameplay path.

Requirements:

- campaign records loaded on campaign-selection/continuation flows, not every frame;
- lineage/history queries indexed by campaign/person/faction IDs;
- no scanning all historical records on every tick;
- continuation seed derived once;
- active campaign stores only the bounded seed inputs it needs;
- archive UI paginates large histories;
- deterministic stable ordering;
- migration runs once per store version;
- record size bounded by caps.

Run a synthetic 20-campaign archive test and record load time, seed-derivation time, memory, and store size.

---

# 21. Rollback Strategy

If legacy logic causes regressions:

### Records layer
Keep reading existing immutable records even if new recording is temporarily disabled.

### Continuation seed
Disable “Continue World” creation while preserving Clean Start.

### Narrative adapters
Disable legacy quests/endings independently.

### Mechanical adapters
Disable all of them without touching record schemas.

### UI
Fall back to campaign history read-only view.

Never repair a legacy bug by reintroducing live prior-save dependencies.

---

# 22. Follow-On Opportunities

Only after the core records/lineage system is accepted:

## Legacy achievements
Derived from immutable records; no gameplay power required.

## Legacy challenges
Special run modifiers chosen at campaign creation.

## Deeper lineage simulation
Only if cohort/relations data genuinely support it.

## Historical world atlas
Map of prior sites, graves, treaties, and campaign outcomes.

## AI-written saga
Presentation-only generation from bounded records; never authoritative.

## Shared/community legacy
Requires separate privacy/network design; not part of single-player core.

## Modded legacy tags
Requires an explicit modding policy and schema stability; not part of this plan.

---

# 23. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --campaign-legacy-selftest
bash scripts/ci/verify-fast.sh
```

Targeted suites:

- `CampaignLegacyRecordTests`
- `CampaignLegacyFinalizationTests`
- `LegacyStoreTests`
- `LegacyMigrationTests`
- `LineageIdentityTests`
- `SuccessionLegacyAdapterTests`
- `CohortLegacyAdapterTests`
- `FactionHistoricalMemoryTests`
- `ShelterLegacyBoundaryTests`
- `ContinuationSeedTests`
- `PreviousSaveIsolationTests`
- `CleanStartParityTests`
- `LegacyQuestTests`
- `LegacyEndingTests`
- `LegacyCatalogTests`
- `LegacyExploitTests`
- `LegacyScaleTests`

---

# 24. Completion Checklist

- [ ] Authority audit completed at current HEAD.
- [ ] Legacy ADR accepted.
- [ ] Campaign records immutable and versioned.
- [ ] Campaign finalization idempotent.
- [ ] Cross-campaign legacy store exists at canonical boundary.
- [ ] Previous-save isolation is proven.
- [ ] Lineage identity model distinguishes parent/guardian/mentor/successor.
- [ ] Succession remains owned by `GenerationalSuccessionEngine`.
- [ ] Cohort maturation remains owned by `CohortSystem`.
- [ ] Trait taxonomy separates genetic, learned, reputational, institutional, narrative, and mechanical categories.
- [ ] Universal 50% trait inheritance is not used.
- [ ] Faction memory is historical by default, not automatic standing bonus.
- [ ] Shelter legacy is historical by default, not copied infrastructure.
- [ ] Endings/epilogues generate records without being reimplemented.
- [ ] Continuation seed is deterministic and bounded.
- [ ] Clean start remains first-class.
- [ ] All mechanical adapters are disabled unless separately approved.
- [ ] Legacy quest hooks instantiate new current-campaign quests.
- [ ] Legacy ending predicates preserve base endings.
- [ ] Legacy UI shows history/lineage clearly.
- [ ] Old saves load safely.
- [ ] Duplicate/farming exploits blocked.
- [ ] 10–20 campaign scale tests remain bounded.
- [ ] Player can perceive at least one meaningful historical callback.
- [ ] `E1_planintegration[6].md` is the next sequence filename.

---

# 25. Final Directive

Plan 140's strongest idea is not persistent bonuses. It is that a campaign should leave evidence behind.

ASHFALL can achieve multi-generational meaning through names, descendants, mentors, graves, old alliances,
betrayals, unfinished promises, remembered sites, endings, institutional reputations, and lineage-specific
stories. Those effects can be powerful precisely because they do not automatically make the next campaign
easier.

The implementation standard is therefore:

**record history immutably, derive bounded continuation once, keep clean start equal, and require a separate
explicit decision before history becomes mechanical power.**

If a legacy feature needs to copy live survivor stats, shelter upgrades, full faction standings, inventory,
or active systems into the next run, stop and re-evaluate the campaign boundary.
