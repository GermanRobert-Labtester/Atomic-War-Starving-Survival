# Belief & Pre-War Profession Integration Architecture

## 1. Principles of Integration
In Plan 137, beliefs and pre-war professions serve narrative immersion, survivor distinction, and social friction. They are explicitly constrained by the following design rules:
1. **Beliefs are Narrative Worldviews, Not Morality Meters:** Beliefs govern ideological perspective, interpersonal affinity/tension, and dialog flavor. They do not grant direct combat stat bonuses (+10% damage) or resource generation buffs (+5 food).
2. **Professions are Pre-War Backgrounds, Not Skill Overrides:** A survivor's pre-war occupation explains who they were before the bombs fell. Their active settlement duties and capabilities are dictated by their core skills and assignments in `survivors.json` and `SurvivorWorkShiftSystem`.
3. **No Direct Gameplay Stat Mutation:** Verified by unit tests. Accessing or displaying enrichment data does not mutate `SurvivorDefinition`, `Health`, `Morale`, or `Radiation`.

---

## 2. Belief Profile Matrix & Social Friction

### 2.1 Profiles Summary
| Belief Profile ID | Human-Readable Name | Core Tenet | Natural Allies | Primary Tension |
|---|---|---|---|---|
| `atheist_rationalist` | Atheist Rationalist | Logic, science, empirical proof | `pragmatic_individualism` | `religious_faith`, `superstitious_traditional` |
| `collectivist_solidarity` | Collectivist Solidarity | Shared labor, equal rations, communal good | `pacifist` | `pragmatic_individualism` |
| `military_discipline` | Military Discipline | Hierarchy, orders, perimeter defense | `pragmatic_individualism` | `pacifist`, `superstitious_traditional` |
| `pacifist` | Pacifist | Preservation of life, non-violence | `collectivist_solidarity` | `military_discipline` |
| `pragmatic_individualism` | Pragmatic Individualist | Self-sufficiency, merit, realistic compromises | `atheist_rationalist` | `collectivist_solidarity` |
| `religious_faith` | Religious Faith | Divine providence, prayer, repentance | `superstitious_traditional` | `atheist_rationalist` |
| `superstitious_traditional` | Superstitious Traditionalist | Taboos, omens, protective rituals | `religious_faith` | `atheist_rationalist`, `military_discipline` |

### 2.2 Integration in `SurvivorSocialCoordinator` (`src/Main.SurvivorSocial.cs`)
When evaluating social encounters between survivors during daily shifts or evening rest:
- **Shared Profile:** Survivors with the same belief experience positive cohesion (+affinity).
- **Compatible Worldviews:** Rationalists and individualists find common operational ground.
- **Ideological Friction:** When a religious penitent is assigned to share a bunker bunk or expedition post with an uncompromising rationalist, ideological friction events can occur, reflecting real human coping mechanisms.
- **Unenriched Fallback:** When evaluating a survivor lacking an authored enrichment entry, `InferBeliefProfile(traits)` deterministically derives a compatible profile from their character traits.

---

## 3. Pre-War Professions & UI Presentation

### 3.1 Display Rules (`src/UI/SurvivorDetailPanel.cs`)
- **Enriched Profession:** If `pre_war_profession_id` is defined (e.g. `nurse`, `machinist`), the UI renders:
  `Profession: Pre-War Nurse` or `Profession: Pre-War Machinist`
- **Fallback Profession:** If unassigned in enrichment, the UI displays the canonical `def.profession` formatted cleanly (e.g. `Botanist`, `Scavenger`, `Technician`).
- **Zero Raw Tokens:** Underscores are replaced with spaces, and tokens are Title-Cased.

### 3.2 Immutability Contract
`SurvivorEnrichmentService.GetView(survivorId)` returns a read-only `SurvivorEnrichmentView` struct/class. No setter or mutating method exists that can alter runtime survivor stats.
