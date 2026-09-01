# Plan 150 — Romance & Family Dynamics System

## Goal

Create a romance and family dynamics system where survivors can form romantic relationships, establish family units, and experience generational storytelling. Currently `CohortSystem` tracks children with parent IDs but there are no romantic relationships, no courtship, no marriage/partnership mechanics, no family interactions, and no inter-generational social dynamics. The `bondType` field in `RelationshipEntry` can store "mentor" or "caregiver" but no code sets or reads romantic/family bond types. This plan adds meaningful romantic and family gameplay that deepens survivor relationships and creates multi-generational stories.

## Why

**Repository evidence:** The survivor social agent confirmed: "There is no romance system. No romantic relationship formation between survivors. No marriage/partnership mechanics. No spousal bonds distinct from friendship/rivalry. No family unit mechanics. No courtship or relationship progression beyond numerical affinity." `CohortSystem.cs` (174 lines) tracks children with `parentIds` but this is data lineage, not living family dynamics. `GenerationalLineageExtension` tracks parent/child/mentor lineage with `inheritedTraitIds` but no code populates inherited traits from parent traits. The `bondType` field exists but is never set to romantic/family types.

**What is missing:** Survivors can't fall in love. Families don't exist as social units. Children grow up without knowing their parents' stories. There's no courtship, no partnership, no spousal bond, no family interaction, no generational storytelling. The social system tracks affinity and trust but not love or family.

**Why existing plans don't solve it:** Plan 12 (social/shelter life) mentions generational arcs but not romance. Plan 30 (ritual/faith/meaning) covers folklore/rituals but not family dynamics. Plan 144 (survivor autonomy) adds autonomous behavior but not romance. Plan 140 (legacy) adds cross-campaign inheritance but not in-campaign family. No plan addresses romance or family mechanics.

**Player value:** Creates emotional investment (survivors form meaningful bonds), adds depth to shelter management (family units affect morale and decisions), generates emergent stories (love triangles, family conflicts, generational drama), and makes the shelter feel like a community, not just a workplace.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationship data
- `Assets/Ashfall.Core/CohortSystem.cs` — children/parent tracking
- `Assets/Ashfall.Core/GenerationalLineageExtension.cs` — lineage tracking
- `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` — caregiving bonds
- `Assets/StreamingAssets/Data/survivors.json` — survivor definitions
- NEW: `Assets/Ashfall.Core/Survivors/RomanceSystem.cs`
- NEW: `Assets/Ashfall.Core/Survivors/FamilySystem.cs`

## Main Task 1 — Foundation / System Contract

1. Create `RomanceSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Create `FamilySystem.cs` in `Assets/Ashfall.Core/Survivors/`
3. Define `RomanticRelationship` DTO: `survivorA`, `survivorB`, `relationshipStage` (attraction/courtship/partnership/bonded), `romanceScore` (0-100), `compatibility` (0-100), `startDay`, `lastInteractionDay`
4. Define `FamilyUnit` DTO: `familyId`, `parentIds` (list), `childIds` (list), `extendedFamily` (list of related survivor IDs), `familyBond` (0-100), `familyReputation` (string)
5. Define `RomanceState` DTO: list of romantic relationships, list of family units, list of courtship events
6. Define `FamilyState` DTO: list of family units, list of family events, list of inherited traits
7. Implement `CaptureState/RestoreState` with schema versioning for both systems
8. Define romance stages:
   - **Attraction** (0-25): survivors notice each other, flirtation begins
   - **Courtship** (25-50): dating, getting to know each other, compatibility testing
   - **Partnership** (50-75): committed relationship, cohabitation, mutual support
   - **Bonded** (75-100): deep bond, marriage-equivalent, family formation possible
9. Define compatibility factors:
   - Age compatibility (similar age preferred)
   - Belief compatibility (similar beliefs preferred, opposite can work)
   - Personality compatibility (complementary traits)
   - Background compatibility (shared experiences)
10. Create romance trigger rules:
    - High affinity (>50) + compatible traits → attraction chance
    - Successful courtship events → romance score increases
    - Partnership requires mutual consent (both survivors agree)
    - Bonded requires sustained high romance score (75+ for 30 days)
11. Implement family formation:
    - Bonded partners can form family unit
    - Family unit can adopt or have children (CohortSystem integration)
    - Children inherit traits from parents (GenerationalLineageExtension integration)
    - Extended family includes parents, siblings, children
12. Add deterministic seeding: romance triggers use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupRomance`, `SetupFamily`, `TickRomance`, `TickFamily`, `SaveRomance`, `SaveFamily`
14. Create `RomanceEventCatalogLoader` for courtship events
15. Implement romance/family logging: all events recorded for UI/journal

## Main Task 2 — Implementation / Courtship / Family / Generational

1. Implement courtship mechanics:
   - Courtship events: shared meals, walks, conversations, gifts
   - Each event increases romance score if compatibility is high
   - Rejection possible if compatibility is low or affinity is negative
   - Courtship can fail (survivors decide not to pursue)
   - Successful courtship leads to partnership
2. Implement partnership mechanics:
   - Partners share housing (room assignment bonus)
   - Partners support each other (morale bonus when together)
   - Partners cooperate on tasks (work efficiency bonus)
   - Partners defend each other (protection in conflicts)
   - Partnership can end (breakup) if affinity drops too low
3. Implement bonded mechanics:
   - Bonded partners have deep emotional connection
   - Bonded partners gain "Soulmate" trait (permanent morale bonus)
   - Bonded partners can form family unit
   - Death of bonded partner causes severe grief (mental health crisis)
   - Bonded partners inherit each other's traits on death
4. Implement family unit mechanics:
   - Family units share resources more efficiently
   - Family units provide morale bonus to members
   - Family units protect children (child safety bonus)
   - Family units can adopt orphaned children
   - Family reputation affects shelter social dynamics
5. Implement parent-child dynamics:
   - Parents teach children skills (mentorship bonus)
   - Parents protect children (safety bonus)
   - Children inherit parent traits (skill, belief, personality)
   - Parent-child relationship affects child development
   - Orphaned children adopted by families
6. Implement sibling dynamics:
   - Siblings have affinity bonus
   - Siblings compete for resources (friction chance)
   - Siblings support each other (morale bonus)
   - Sibling relationships affect family bond
7. Create romance events:
   - "First Sight" — survivors notice each other, attraction begins
   - "The Date" — courtship event, romance score increases
   - "The Proposal" — partnership formed, commitment made
   - "The Wedding" — bonded status achieved, celebration
   - "The Breakup" — partnership ends, affinity penalty
   - "The Loss" — partner dies, grief crisis
   - "The Reunion" — separated partners reconnect
   - "The Rivalry" — two survivors compete for same person
8. Create family events:
   - "The Birth" — child born/adopted, family expands
   - "The Graduation" — child matures into adult survivor
   - "The Reunion" — extended family members reunite
   - "The Inheritance" — traits passed from parent to child
   - "The Feud" — family conflict, bond reduced
   - "The Tradition" — family establishes shelter tradition
9. Add romance quest hooks:
   - "The Matchmaker" — help two survivors find love
   - "The Rival" — compete with another survivor for romance
   - "The Proposal" — plan the perfect partnership proposal
   - "The Family" — help a family in crisis
   - "The Orphan" — find a family for an orphaned child
   - "The Legacy" — ensure family traits pass to next generation
10. Add UI: romance panel showing relationships and family trees
11. Create romance/family journal: automatic log of relationship events
12. Implement romance tutorial: first attraction explains system
13. Add romance tooltips: hover over survivor shows relationship status
14. Create 20 romance events and 15 family events in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SurvivorRelationsSystem`: romantic relationships use relationship data
2. Connect to `CohortSystem`: children integrate with family units
3. Integrate with `GenerationalLineageExtension`: trait inheritance from parents
4. Connect to `CaregivingSystem`: caregiving can lead to romance
5. Wire into `MentalHealthCrisisSystem`: partner death causes grief crisis
6. Connect to `DutyRosterSystem`: partners prefer same shifts
7. Implement old-save compatibility: existing saves get empty romance/family state
8. Add deterministic seeding: romance triggers use `ISeededRng`
9. Create exploit prevention: romance has natural progression, can't be rushed
10. Add tests: romance progression, family formation, trait inheritance, save round-trip
11. Verify catalog integrity: all survivor IDs resolve
12. Test edge cases: no romance (no families), all survivors bonded (max families)
13. Verify headless behavior: romance/family process correctly without UI
14. Add data-integrity-selftest: romance/family events validate against survivor catalog
15. Create `--romance-family-selftest` verb for CI validation

## State / System Interaction Model

```text
Survivor relationship development
├─ Attraction stage (affinity > 50, compatibility check)
│  ├─ Courtship events: shared activities
│  ├─ Romance score increases
│  └─ Compatibility tested
├─ Courtship stage (romance 25-50)
│  ├─ Dating events: deeper connection
│  ├─ Mutual consent required for partnership
│  └─ Can fail (incompatibility discovered)
├─ Partnership stage (romance 50-75)
│  ├─ Committed relationship
│  ├─ Shared housing, morale bonus
│  ├─ Work efficiency bonus
│  └─ Can form family unit
├─ Bonded stage (romance 75-100)
│  ├─ Deep emotional bond
│  ├─ "Soulmate" trait gained
│  ├─ Family formation possible
│  └─ Partner death causes severe grief
├─ Family unit formed
│  ├─ Parents and children
│  ├─ Trait inheritance
│  ├─ Family bond and reputation
│  └─ Extended family network
└─ Generational storytelling
   ├─ Children inherit parent traits
   ├─ Family traditions established
   ├─ Legacy carried forward
   └─ Epilogue includes family outcomes
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --romance-family-selftest
```

## Risk

**MEDIUM** — Romance system can feel forced or unrealistic if progression is too mechanical. Risk of family dynamics adding complexity without player value. Mitigation: keep romance progression natural (driven by affinity/compatibility), make family optional (not required for gameplay), and focus on emotional storytelling rather than mechanical optimization.

## Definition of Done

- `RomanceSystem.cs` and `FamilySystem.cs` exist with full `CaptureState/RestoreState`
- 4 romance stages implemented (attraction, courtship, partnership, bonded)
- Compatibility system functional
- Courtship events and mechanics working
- Partnership and bonded mechanics functional
- Family unit system working (parents, children, extended family)
- Parent-child and sibling dynamics implemented
- Trait inheritance from parents to children
- Romance and family events
- Save/load round-trip tested
- Deterministic romance triggers verified
- Old saves load without error
- 20 romance events + 15 family events in data authority
- UI panel shows relationships and family trees
- Cross-system integration (relations, cohort, lineage, caregiving, mental health, duty roster)

## Follow-On Opportunities

- Romance jealousy system (rivalry between survivors)
- Family dynasty system (multi-generational family legacy)
- Romance legacy (relationships carry to New Game+)
- Family quests (family-specific quest chains)
- Romance epilogue (relationship outcomes in ending)
