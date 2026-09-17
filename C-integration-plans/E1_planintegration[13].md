---
PLAN_ID: E1-13
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 13
STATUS: READY_FOR_EXECUTION_WHEN_BIOLOGY_AND_AUTHORITY_RAILS_PASS
SOURCE_PLAN: "Plan 172 — Radiation Mutation System"
SEQUENCE_FILENAME: "E1_planintegration[13].md"
PREVIOUS_FILENAME: "E1_planintegration[12].md"
NEXT_FILENAMES:
  - "E1_planintegration[14].md"
  - "E1_planintegration[15].md"
CATEGORY: LINK+RADIATION+SURVIVOR_TRAITS+LONG_TERM_CONSEQUENCES+PRESENTATION
PRIMARY_INTENT: "Add a bounded fictional radiation-mutation layer that consumes canonical exposure history and projects persistent survivor traits through existing health, lifecycle, disease, reproduction, social, research, and presentation authorities without replacing them."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
FICTIONAL_BIOLOGY_LABEL_REQUIRED: true
SECOND_RADIATION_LEDGER_FORBIDDEN: true
SECOND_SURVIVOR_STAT_SYSTEM_FORBIDDEN: true
SECOND_GENETICS_SYSTEM_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: HIGH
BALANCE_RISK: VERY_HIGH
CONTENT_RISK: VERY_HIGH
---

# E1 Plan Integration [13] — Radiation Mutation, Persistent Survivor Change, Inheritance Boundaries, Social Consequences, and Fictional Biology

> **Sequence rule:** this file is `E1_planintegration[13].md`.
> The next files are `E1_planintegration[14].md`, `E1_planintegration[15].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 172 into an implementation-grade mutation programme suitable for ASHFALL while
separating speculative game fiction from the biological systems that already exist.

The source plan proposes long-term radiation exposure causing persistent "mutations" that may be harmful,
neutral, mixed, or beneficial; some can be visible, progress over time, affect survivor capabilities and
social treatment, and possibly appear in descendants. This can create strong emergent stories and a genuine
risk/reward layer around contaminated zones.

It also contains several assumptions that should not become architecture without correction:

- Ionizing radiation is overwhelmingly associated with DNA damage, cancer risk, tissue injury, reproductive
  harm, and stochastic deleterious effects. A predictable menu of beneficial human adaptations such as
  enhanced strength or night vision is speculative fiction, not realistic radiation biology.
- Inbreeding does not generally "increase the mutation rate." It increases homozygosity and the probability
  that harmful recessive inherited variants are expressed.
- Chelation does not erase radiation dose already absorbed by tissue. Some decorporation treatments can
  reduce internal contamination from specific radionuclides, thereby reducing future dose.
- Gene therapy that stabilizes a broad set of radiation-induced survivor mutations is highly speculative.
- A single cumulative-dose threshold is not enough to model all radiation outcomes realistically.

ASHFALL may intentionally embrace mutation fiction. If so, it should do so consciously, with terminology and
data contracts that prevent the simulation from conflating **radiation dose**, **radiation injury**,
**contamination**, **disease**, **heritable lineage traits**, and **fictional mutation traits**.

The implementation standard is:

**radiation authority reports exposure history; mutation policy decides fictional trait eligibility; canonical
survivor systems own the consequences.**

## 1. Source Intent Preserved

Plan 172 asks for:

- persistent mutation traits;
- exposure-triggered manifestation;
- deterministic rolls;
- harmful/neutral/mixed/beneficial categories;
- progression;
- visibility and diagnosis;
- medical management;
- social consequences;
- inheritance;
- events and quests;
- research;
- UI;
- save/load;
- old-save compatibility;
- data-driven traits;
- headless selftest.

E1-13 preserves these gameplay goals while imposing stricter authority and biological-semantics boundaries.

## 2. Core Architecture Thesis

```text
RadiationSystem / exposure authority
        |
        +--> cumulative dose / dose-band events
        +--> contamination/internal radionuclide state
        +--> exposure provenance
        |
        v
Mutation eligibility policy
        |
        +--> fictional mutation profile rules
        +--> deterministic threshold crossings
        +--> manifestability / incompatibility
        |
        v
Persistent mutation trait identity
        |
        +--> visibility/discovery state
        +--> progression state if truly required
        +--> provenance
        |
        v
Canonical consumers
        |
        +--> Survivor aggregate / modifier framework
        +--> Needs / health
        +--> Disease / afflictions
        +--> Lifecycle / fertility / lifespan
        +--> Skills / expedition capability
        +--> Relations / mental health
        +--> Factions / Market
        +--> Research / medicine
        +--> UI / journal / quests
```

The mutation layer owns **which fictional mutation traits exist on a survivor and their mutation-specific
state**. It does not own radiation dose, health, fertility, lifespan, faction standing, prices, morale, or
survivor stats directly.

## 3. Non-Negotiable Rules

- `RadiationSystem` remains the source of radiation exposure/dose truth.
- Mutation state may reference exposure milestones but may not duplicate the dose ledger.
- `NeedsSystem`/health authorities own health consequences.
- `DiseaseSystem` owns disease/affliction behavior.
- `SurvivorLifecycle` or the canonical successor owns lifespan/reproduction lifecycle.
- Survivor-stat/modifier framework owns strength/intelligence/work/combat modifiers.
- Faction systems own faction reactions and standing.
- Market/Economy owns price/trade effects.
- Relations/MentalHealth own social and morale effects.
- Research owns research unlock state.
- Mutation content must distinguish **fictional mutation traits** from medically realistic radiation injury.
- No trait is considered heritable unless the lineage/reproduction authority explicitly supports hereditary
  traits and the trait definition declares a valid inheritance model.
- No universal 50% inheritance rule.
- Inbreeding is not implemented as a mutation-rate multiplier.
- No generic "chelation reduces cumulative dose" operation.
- Any decorporation treatment must target supported internal-contamination state and future dose.
- No universal gene-therapy cure/stabilizer unless the game explicitly treats it as speculative advanced
  technology and the research/medical authorities can represent it.
- Visible mutations do not automatically produce a global social penalty; reactions are contextual and
  authority-owned.
- Hidden mutations are not magically known to the player before diagnosis/reveal.
- Appearance changes are presentation state, not a gameplay-stat owner.
- Mutation rolls must be deterministic and must not reroll on load.
- Trait stacking must have hard compatibility and total-power limits.
- The first release uses a small corpus, preferably 6–8 traits, before 20-trait expansion.
- High-power "beneficial" mutations are gated behind explicit balance review.
- A radiation-free or low-radiation campaign must remain fully playable and narratively complete.
- Old saves get no fabricated mutation history.

## 4. Acceptance Slices

### Slice A — Exposure milestones and persistent trait identity
Radiation crosses a validated milestone → deterministic fictional trait check → one persistent trait.

### Slice B — Harmful/neutral/mixed consequences
Health/social/presentation consequences through canonical systems.

### Slice C — Diagnosis and management
Reveal hidden traits and manage exposure/associated conditions.

### Slice D — Inheritance
Only if reproduction/lineage rails support explicit hereditary traits.

### Slice E — High-speculative beneficial traits
Only after balance and fiction review.

### Slice F — Events/quests/content scale
Expand trait catalog and narrative consumers after the core loop proves useful.

Do not begin with twenty traits, universal inheritance, gene therapy, and faction discrimination at once.


---

## E1-13A — Premise verification and radiation/mutation authority audit

**Goal:** Verify radiation exposure, survivor aggregate, trait/modifier, disease, lifecycle, reproduction, social, research, diagnosis, and save rails before adding mutation state.

### Required substeps

1. Inspect `RadiationSystem`, radiation dose/exposure DTOs, contamination/internal radionuclide logic, `NeedsSystem`, `SurvivorLifecycle`, survivor aggregate/components, disease/affliction systems, trait/perk/modifier systems, reproduction/cohort/lineage systems, mental-health/relations, faction standing, market/economy, research, medical diagnosis/treatment, journal/quest/UI, and save registry.
2. Verify whether cumulative dose is already persisted per survivor and in what units.
3. Verify whether radiation thresholds/events already exist.
4. Verify whether survivor stats accept typed modifiers from traits/components.
5. Verify whether permanent traits/perks already have a canonical owner.
6. Verify whether fertility/lifespan are explicit gameplay variables or merely narrative concepts.
7. Verify whether reproduction and parentage are implemented.
8. Verify whether hidden medical conditions/diagnosis are represented.
9. Search for mutation-prefixed catalog support and any prior prototype data.
10. Map every proposed mutation effect to a canonical owner.
11. Create `docs/systems/RADIATION_MUTATION_AUTHORITY_MAP.md`.
12. Create intake duplicate-search evidence linking Plan 146, radiation bridge work, lineage/legacy work, medical systems, and current survivor trait architecture.
13. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13B — Biology/fantasy ADR

**Goal:** Define exactly where ASHFALL intentionally departs from real radiation biology.

### Required substeps

1. Write an ADR separating realistic radiation injury from fictional mutation mechanics.
2. State that beneficial superhuman adaptations are speculative fiction.
3. State that cumulative dose is one eligibility signal, not a medically faithful genetic-mutation probability model.
4. State that inbreeding affects inherited recessive-risk modeling only if a genetics rail exists; it does not raise the mutation rate.
5. State that decorporation may reduce internal contamination/future dose for certain contaminants but cannot erase absorbed dose.
6. State that advanced gene stabilization is speculative technology if retained.
7. Define in-world terminology: `mutation`, `radiation-altered trait`, `adaptive anomaly`, or equivalent.
8. Define which traits are allowed in grounded versus heightened-fiction content modes if the game supports scenario tuning.
9. Require narrative/medical documentation to avoid presenting game mechanics as real-world treatment guidance.
10. Require second-tool review.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13C — Mutation ownership ADR

**Goal:** Choose the smallest persistent authority for mutation identity and prevent a second survivor-stat system.

### Required substeps

1. Compare a dedicated `MutationSystem`, extension of survivor trait/component store, and a thin mutation eligibility service feeding the canonical trait system.
2. Prefer storing persistent mutation identities in the existing survivor component/trait aggregate if that architecture supports typed persistent components.
3. Define mutation-owned fields: trait ID, manifestation ID/day, source/provenance, severity/progression only if mutation-specific, visibility/discovery state, inherited/acquired origin, and deterministic decision record.
4. Explicitly exclude radiation dose, health, disease, fertility, lifespan, morale, faction standing, trade prices, and derived stats.
5. Define typed adapters from mutation trait to canonical modifier/effect consumers.
6. Define feature flags for inheritance, progression, visible appearance, social response, and high-speculative traits.
7. Require second-tool architecture review.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13D — Mutation trait catalog schema

**Goal:** Define traits as bounded data-driven identities with explicit consumers, compatibility, and fiction classification.

### Required substeps

1. Define trait ID, localization keys, category, fiction level, manifestation eligibility, exposure milestone policy, acquisition mode, visibility, diagnosis requirement, progression policy, incompatibility tags, effect-policy references, inheritance policy reference, and presentation tags.
2. Do not store arbitrary direct stat deltas in a generic `effects` string/list unless the canonical modifier framework already uses that schema.
3. Require each mechanical effect to name a canonical consumer.
4. Define `beneficial`, `harmful`, `neutral`, and `mixed` as player-facing classifications only if useful; do not let them determine mechanics automatically.
5. Define trait exclusivity and stacking caps.
6. Validate all referenced stats/afflictions/presentation/research/inheritance policies.
7. Start with 6–8 traits.
8. Version the catalog.
9. Add integrity tests for duplicate IDs, invalid thresholds, unresolved consumers, illegal inheritance, circular progression, and incompatible category data.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13E — Exposure milestone contract

**Goal:** Trigger mutation eligibility from canonical radiation history without copying the dose ledger.

### Required substeps

1. Identify the radiation authority's stable per-survivor cumulative dose query/event.
2. Define milestone IDs or bands through mutation policy data.
3. Do not persist a second cumulative dose value.
4. Track only which mutation eligibility milestones have already been evaluated if required for idempotency.
5. Define exact boundary crossing behavior.
6. Handle large time/dose jumps crossing several milestones.
7. Define whether acute exposure and chronic exposure are treated differently; defer if radiation authority cannot distinguish them.
8. Add tests for below threshold, exact threshold, multi-threshold jump, repeated query, dose reduction after contamination treatment, and save/load.
9. Keep source 100/200/500/1000/2000 mSv thresholds as tuning hypotheses rather than authoritative medical risk breakpoints.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13F — Deterministic manifestation decision

**Goal:** Make trait acquisition reproducible, idempotent, and independent of save-scumming.

### Required substeps

1. Define stable decision key from campaign seed + survivor ID + milestone ID + trait/pool version.
2. Use `ISeededRng` or existing deterministic keyed RNG.
3. Do not consume unrelated global RNG streams.
4. Persist decision outcome if catalog changes could otherwise change existing saves.
5. Define candidate filtering before random choice.
6. Apply compatibility and max-trait caps.
7. Ensure failed checks are recorded if repeat prevention requires it.
8. Do not reroll after load.
9. Add same-seed, different-survivor, different-milestone, catalog-version, and retry tests.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13G — Trait-pool and rarity policy

**Goal:** Prevent high-radiation exposure from producing an uncontrolled pile of traits.

### Required substeps

1. Define candidate pools by exposure band, scenario, survivor state, and fiction level.
2. Set per-survivor acquired-trait cap.
3. Define rarity/weight data.
4. Define no-result outcome as valid.
5. Define duplicate-trait prevention.
6. Define incompatible-trait exclusion.
7. Define escalating risk without guaranteeing a mutation.
8. Add long-run distribution tests.
9. Measure mutation incidence and category distribution over representative campaigns.
10. Do not force beneficial/harmful parity unless balance evidence supports it.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13H — Persistent trait attachment to survivor aggregate

**Goal:** Attach manifested mutation identity to the canonical survivor representation.

### Required substeps

1. Use existing typed component/trait store if available.
2. Assign stable mutation manifestation ID.
3. Record acquired/inherited origin and day.
4. Register canonical effect-policy adapters.
5. Do not copy survivor stat values into mutation record.
6. Define remove/disable semantics only for migration/debug if mutations are normally permanent.
7. Add save/load tests.
8. Add tests proving the same manifestation cannot attach twice.
9. Ensure survivor deletion/death cleanup keeps history appropriately.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13I — Mechanical modifier integration

**Goal:** Route mutation mechanical effects through the canonical survivor modifier framework.

### Required substeps

1. Audit stat calculation architecture.
2. Define typed modifier policies such as radiation tolerance, carrying capacity, perception/night visibility, disease susceptibility, pain burden, fertility factor, lifespan risk, or appearance tag only where real consumers exist.
3. Do not directly mutate base strength/intelligence values.
4. Use bounded modifier magnitudes.
5. Define stacking order and caps.
6. Define condition-dependent effects if needed.
7. Add tests for apply/remove/restore, stacking, cap, incompatible traits, and save/load.
8. High-impact combat/work bonuses require separate balance approval.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13J — Radiation-resistance trait boundary

**Goal:** If fictional radiation resistance exists, make it modify future exposure handling through the radiation authority rather than rewriting past dose.

### Required substeps

1. Define the exact consumer: external-dose sensitivity, internal uptake, tissue-damage multiplier, or protective physiology abstraction.
2. Do not subtract prior cumulative dose.
3. Do not make resistance complete immunity.
4. Apply the modifier in one canonical radiation/exposure calculation.
5. Add tests for same environmental exposure with/without trait.
6. Ensure dose records remain truthful to the game's abstraction.
7. Document fiction level.
8. Balance against protective equipment so mutation is not mandatory.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13K — Strength/performance trait boundary

**Goal:** If enhanced strength or similar traits are retained, integrate through canonical capacity/performance modifiers.

### Required substeps

1. Audit carrying, melee, work, and performance systems.
2. Choose one or two bounded consumers rather than globally applying `+10% strength` everywhere.
3. Do not duplicate survivor base stats.
4. Define fatigue/food/metabolic tradeoffs if design requires and canonical systems support them.
5. Add tests for actual affected mechanics.
6. Measure power relative to equipment/skill.
7. Feature-gate high-speculative traits.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13L — Sensory/vision trait boundary

**Goal:** If night-vision-like traits exist, route them through perception/visibility systems only if such systems exist.

### Required substeps

1. Audit darkness, perception, expedition visibility, combat visibility, and scouting systems.
2. Do not create a fake trait with no consumer.
3. Define bounded effect and conditions.
4. Do not bypass darkness entirely.
5. Add tests for low-light, daylight, equipment overlap, and save/load.
6. Defer the trait if no canonical visibility system can consume it.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13M — Disease-resistance interaction

**Goal:** Integrate mutation-related disease susceptibility/resistance through DiseaseSystem.

### Required substeps

1. Define disease modifier policy IDs.
2. Do not suppress disease outcomes directly from mutation code.
3. Apply susceptibility/resistance during canonical disease acquisition/progression calculations.
4. Cap effect magnitude.
5. Handle multiple sources of resistance.
6. Add tests for exposure, infection probability/progression, stacking, and save/load.
7. Keep immunity claims bounded and explicit.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13N — Chronic pain and harmful-condition integration

**Goal:** Represent harmful mutations through canonical affliction/needs systems where possible.

### Required substeps

1. Audit chronic pain, fatigue, mobility, cognition, and health-affliction models.
2. Prefer attaching an affliction/policy reference rather than ticking custom damage inside mutation code.
3. Define severity mapping if the affliction system supports it.
4. Route treatment/symptom management through medical authority.
5. Add tests for active symptom, treatment, interruption, save/load, and no duplicate affliction.
6. Keep mutation identity separate from the health condition it causes.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13O — Lifespan and mortality boundary

**Goal:** Do not directly decrement a hidden lifespan counter unless SurvivorLifecycle already models lifespan.

### Required substeps

1. Audit lifecycle/age/mortality mechanics.
2. If lifespan risk exists, route mutation influence through that authority.
3. If not, represent shortened lifespan as narrative/health risk rather than inventing a parallel death timer.
4. Use disease/cancer/organ-damage rails where supported.
5. Add tests only against real lifecycle behavior.
6. Defer mechanical lifespan effect if unsupported.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13P — Fertility and reproduction boundary

**Goal:** Integrate mutation-related fertility only through canonical reproduction/cohort systems.

### Required substeps

1. Audit whether fertility is actually modeled.
2. Define modifier policy if supported.
3. Do not create a mutation-only fertility variable.
4. Keep infertility/reduced fertility as narrative-only if the lifecycle system has no consumer.
5. Add tests for eligible reproduction cases if supported.
6. Treat this as sensitive content in presentation and avoid stigmatizing language.
7. Feature-gate mechanical fertility effects.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13Q — Inheritance model ADR

**Goal:** Only enable inherited mutations when lineage/reproduction architecture can represent heritable traits explicitly.

### Required substeps

1. Audit E1-5 lineage and cohort/reproduction implementations.
2. Separate acquired somatic mutation traits from heritable/germline fictional traits.
3. Require trait-level `inheritance_policy` metadata.
4. Reject universal 50% inheritance.
5. Define deterministic inheritance decision key by parent/child/trait.
6. Allow Mendelian-like, probabilistic, nonheritable, or narrative inheritance policies depending on game abstraction.
7. Do not lower a child's radiation threshold merely because a parent had an acquired mutation unless explicit fictional biology says so.
8. Define two-parent conflict/combination rules.
9. Add tests for nonheritable trait, heritable trait, two-parent case, no-parent case, save/load, and deterministic birth outcome.
10. Keep inheritance disabled in first release if cohort rails are incomplete.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13R — Inbreeding/genetic-risk correction

**Goal:** Prevent the source plan's incorrect 'inbreeding increases mutation risk' rule from becoming a generic mutation multiplier.

### Required substeps

1. Do not modify radiation mutation probability based on relatedness.
2. If lineage genetics later models recessive inherited conditions, represent increased homozygosity/expression risk in that separate genetics authority.
3. Do not infer close relatedness without canonical lineage data.
4. Do not add stigmatizing or sensational presentation.
5. Create a documentation note explaining the distinction.
6. Add a negative test asserting relatedness does not alter acquired radiation-mutation check probability unless an explicit fictional policy says otherwise.
7. Defer inbreeding mechanics entirely unless a dedicated genetics plan exists.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13S — Visibility and appearance contract

**Goal:** Represent visible traits as presentation descriptors attached to canonical survivor appearance state.

### Required substeps

1. Define visibility state: visible-by-default, hidden/diagnosable, context-dependent.
2. Do not equate visibility with social penalty.
3. Expose appearance tags to portrait/description systems.
4. Do not regenerate portraits nondeterministically on every load.
5. Define whether appearance assets actually exist before requiring visual transformation.
6. Add fallback textual description.
7. Add tests for visible, hidden, revealed, and missing-asset cases.
8. Keep appearance changes separate from mechanical effects.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13T — Diagnosis and knowledge-state integration

**Goal:** Ensure hidden traits are unknown until canonical diagnosis/knowledge reveals them.

### Required substeps

1. Audit diagnosis/medical-exam systems.
2. Store survivor-has-trait separately from player-knows-trait where knowledge rails support that distinction.
3. Medical examination uses canonical action/job/resource/time.
4. Reveal only supported information.
5. Do not let UI inspect hidden state directly.
6. Add tests for unknown trait, examination, partial/noisy diagnosis if supported, repeat exam, save/load.
7. Use one canonical knowledge state.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13U — Radiation management and decorporation boundary

**Goal:** Correctly separate reducing contamination/future dose from reversing absorbed radiation dose.

### Required substeps

1. Audit internal contamination and radionuclide treatment mechanics.
2. If supported, define treatment policies for specific contaminants/decorporation.
3. Do not decrease historical absorbed-dose ledger merely because treatment occurs.
4. Treatment may reduce internal burden and future dose accumulation.
5. General supportive care manages injuries/symptoms through medical systems.
6. Add tests for external exposure, internal contamination, treatment, future-dose change, and historical-dose persistence.
7. Use medically neutral in-game terms if the contaminant model is abstract.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13V — Speculative gene-stabilization research gate

**Goal:** Treat gene therapy as optional high-fiction research rather than a baseline medical mechanic.

### Required substeps

1. Require ResearchSystem and advanced medical-treatment authority.
2. Write a fiction-level ADR or data tag.
3. Define what 'stabilize' means mechanically: halt mutation-specific progression, reduce symptom risk, or prevent future fictional manifestations.
4. Do not claim to repair all radiation damage.
5. Do not reverse established trait identity unless explicitly allowed by fiction.
6. Use real resources/time/medical capability.
7. Add tests for locked research, treatment eligibility, stabilization, failure/interruption, and save/load.
8. Keep disabled in grounded/default mode if desired.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13W — Mutation progression model

**Goal:** Only persist progression when it adds player value beyond radiation injury or affliction progression.

### Required substeps

1. Classify each trait as static, exposure-reactive, condition-linked, or progression-capable.
2. Do not make every trait worsen automatically.
3. Use canonical exposure updates as input.
4. Use health/affliction authority for symptom progression where possible.
5. Persist mutation-specific severity only if no canonical consumer owns it.
6. Define monotonic/capped progression rules.
7. Define stabilization semantics.
8. Add tests for static trait, continued exposure, removed exposure, treatment, cap, and save/load.
9. Prevent daily ticking of all traits if event-driven updates suffice.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13X — Mutation permanence and reversibility policy

**Goal:** Clarify which parts are permanent and which consequences are treatable.

### Required substeps

1. Trait identity may be permanent in fiction.
2. Symptoms/afflictions may be treatable.
3. Internal contamination may be reducible.
4. Future exposure can be prevented.
5. Progression may be stabilized.
6. Player knowledge/visibility may change.
7. Appearance may or may not be reversible depending on content.
8. Do not use one `permanent` boolean to govern every consequence.
9. Add tests for each dimension.
10. Document player-facing expectations.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13Y — Social reaction adapter

**Goal:** Make reactions contextual through relations/faction/social systems rather than an automatic stigma scalar.

### Required substeps

1. Define social events such as surprise, fear, curiosity, prejudice, acceptance, exploitation, admiration, or concern as authored/contextual policies.
2. Use survivor traits/beliefs/relationships and faction doctrine if canonical systems expose them.
3. Do not apply universal affinity penalty to all visible mutations.
4. Do not treat appearance difference itself as mechanically harmful by default.
5. Route relationship changes through relations authority.
6. Route morale through mental-health authority.
7. Add tests for accepting friend, fearful stranger, neutral shelter, hostile faction, and no-reaction case.
8. Review content for dehumanizing language.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13Z — Faction interaction boundary

**Goal:** Allow faction-specific reactions without directly writing standing from mutation state.

### Required substeps

1. Audit faction doctrine/standing/branch coordinator.
2. Expose mutation visibility/known status as context to faction interaction policies.
3. Faction authority decides standing/access/trust consequences.
4. Do not encode `visibleMutation = tradePenalty` globally.
5. Allow some factions to be accepting, hostile, exploitative, scientific, or indifferent based on authored doctrine.
6. Add tests for multiple faction policies and no universal reaction.
7. Keep mutation system read-only toward faction state.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AA — Economy/trade boundary

**Goal:** Route any market discrimination or demand through MarketSystem, not mutation-owned price modifiers.

### Required substeps

1. Audit market pricing and faction trader access.
2. Define context tags consumed by market/faction policy if required.
3. Do not store a trade-price modifier in mutation state.
4. Prefer social/faction access effects over arbitrary global price shifts.
5. Add tests for market-neutral, faction-specific restriction, and no mutation knowledge.
6. Feature-gate if economy lacks contextual pricing.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AB — Mental-health and identity consequences

**Goal:** Allow survivor responses to their own changes through canonical mental-health/autonomy systems.

### Required substeps

1. Generate stress, fear, acceptance, body-image concerns, relief, curiosity, or pride contextually.
2. Do not apply automatic morale penalty for visible difference.
3. Use autonomy/personal-goal systems for coping or treatment-seeking if available.
4. Route all state changes through mental-health authority.
5. Add tests for hidden diagnosis, visible change, supportive relations, hostile social environment, and adaptation over time.
6. Keep sensitive narrative language reviewed.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AC — Expedition capability integration

**Goal:** Let mutation traits affect field performance only through real expedition/skill/needs consumers.

### Required substeps

1. Map each supported trait to a canonical capability or modifier.
2. Examples: cold tolerance only if exposure system supports it; carrying capacity only if inventory/encumbrance supports it; radiation tolerance only through radiation authority.
3. Do not create a generic expedition bonus from mutation category.
4. Add tests for each shipped capability.
5. Measure mutation value relative to gear/skill.
6. Keep high-power abilities gated.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AD — Research and scientific-study hooks

**Goal:** Allow mutations to become research subjects without making the mutation system own research progress.

### Required substeps

1. Create research sample/observation events from diagnosed traits.
2. ResearchSystem owns projects/progress/unlocks.
3. Do not consume survivor mutation identity as a resource.
4. Use consent/ethics policy if survivor experimentation exists.
5. Add tests for known trait, hidden trait, research unlock, duplicate observation, and save/load.
6. Keep scientific knowledge separate from treatment availability.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AE — Mutation event contract

**Goal:** Create events from real manifestation/reveal/progression states without duplicating their effects.

### Required substeps

1. Emit `MutationManifested`, `MutationRevealed`, `MutationProgressed`, `MutationStabilized`, and `InheritedTraitObserved` only when those states change.
2. Use stable event IDs.
3. Journal/quest/social systems subscribe.
4. Do not directly apply relation/morale/research rewards in event code.
5. Ensure events fire once across reload.
6. Add tests for manifestation, reveal, progression, stabilization, and duplicate prevention.
7. Use player-facing names only after visibility/diagnosis permits.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AF — Quest hook contract

**Goal:** Add narrative objectives through canonical QuestSystem after the trait loop is stable.

### Required substeps

1. Implement a small subset first: The Changed, The Doctor, The Stigma/Acceptance social arc, and The Gene only if speculative research is enabled.
2. Use manifestation/diagnosis IDs as provenance.
3. Quest authority owns lifecycle/rewards.
4. Do not force reproduction merely to trigger inheritance content.
5. Do not frame mutated survivors as commodities.
6. Allow quest expiry/invalidation on survivor death/leave.
7. Add dedupe/save tests.
8. Review tone for dignity and agency.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AG — Mutation UI and survivor read model

**Goal:** Show known traits, effects, progression, and uncertainty without exposing hidden state.

### Required substeps

1. Extend survivor medical/trait panel where possible.
2. Show diagnosed mutation name/category, known effects, progression status if canonical, management options, and provenance summary.
3. Do not show hidden trait IDs before discovery.
4. Show fictional-biology flavor separately from medical status.
5. Use canonical stat/health projections.
6. Add filter only if enough traits justify it.
7. Add snapshot tests for no trait, hidden trait, diagnosed trait, mixed trait, progression, and managed trait.
8. Keep panel compact.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AH — Appearance/presentation handoff

**Goal:** Allow visible mutation traits to affect portraits/descriptions only through existing presentation pipelines.

### Required substeps

1. Audit portrait layers, survivor descriptors, voice processing, and model variants.
2. Use stable appearance tags.
3. Do not generate visual assets at runtime from simulation state unless current presentation pipeline supports it.
4. Do not alter voice for a trait unless specific content/assets exist.
5. Provide text fallback.
6. Add snapshot/content tests.
7. Keep presentation optional from mechanical trait function.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AI — Mutation journal and historical record

**Goal:** Record major persistent survivor changes without duplicating survivor state.

### Required substeps

1. Use canonical journal/chronicle.
2. Record first manifestation, diagnosis, stabilization, major progression, inheritance observation, and major social event.
3. Do not log every radiation threshold check.
4. Use stable IDs for dedupe.
5. Allow E1-5 legacy to retain notable mutation identity/history narratively.
6. Add retention/order tests.
7. Keep journal read-only toward mutation state.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AJ — Old-save compatibility and migration

**Goal:** Introduce mutations without manufacturing traits from historical radiation exposure unless explicitly chosen.

### Required substeps

1. Default old saves to no manifested mutation state.
2. Decide whether already-crossed radiation milestones should be considered evaluated or eligible prospectively.
3. Prefer no retroactive mass manifestation on first load.
4. Optionally mark past thresholds as evaluated and begin from the next threshold.
5. Preserve canonical dose history.
6. Preserve survivor health/disease state.
7. Version mutation state.
8. Add fixtures at low, medium, high historical dose, no radiation data, and corrupted prototype mutation state.
9. Make migration idempotent.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historical absorbed dose generically.
- A visible mutation automatically applies universal social/trade penalties.
- One acquired somatic trait is inherited through a universal 50% rule.
- Twenty traits can stack without compatibility/power caps.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Determinism/no-reroll tests pass.
- [ ] Save/migration tests pass.
- [ ] Biology/fiction ADR is approved.
- [ ] Trait consumers resolve canonically.
- [ ] Balance/distribution evidence is captured.
- [ ] Content/dignity review passes.

---

## E1-13AK — Save contract and restore ordering

**Goal:** Persist mutation identity and decision/progression metadata without duplicating downstream consequences.

### Required substeps

1. Persist manifestation ID, survivor ID, trait ID, origin, manifested day, discovery state, progression state only if owned, and evaluated milestone/decision records as required.
2. Do not persist radiation dose copies.
3. Do not persist derived stat values.
4. Do not persist faction/trade/morale consequences.
5. Restore survivor aggregate before applying mutation effect adapters.
6. Validate trait IDs/catalog version.
7. Handle removed/changed trait definitions explicitly.
8. Add round-trip tests.
9. Ensure restore cannot re-manifest or double-apply modifiers.

### Mutation integration invariants

- Radiation authority owns dose/exposure truth.
- Mutation authority owns only persistent mutation identity and mutation-specific state.
- Canonical survivor/health/lifecycle/social/economy systems resolve consequences.
- Fictional mutation mechanics are explicitly distinguished from real radiation biology.
- Manifestation decisions are deterministic and cannot reroll on load.
- Inheritance is trait-specific and gated behind lineage/reproduction support.
- Treatment cannot erase historical absorbed dose through mutation code.
- Trait stacking and power are bounded.

### Negative tests

- Mutation state stores a second cumulative radiation dose.
- A mutation directly rewrites base survivor stats.
- Reload rerolls a mutation decision.
- Inbreeding automatically raises radiation mutation probability.
- Chelation subtracts historica

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
