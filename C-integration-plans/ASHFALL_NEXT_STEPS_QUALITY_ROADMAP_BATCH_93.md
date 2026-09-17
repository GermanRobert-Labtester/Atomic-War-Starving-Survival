# ASHFALL — Quality Roadmap Batch 93

## Theme: Survivor Personality Engine — Emergent Behavior from Trait Combinations

**Priority:** MEDIUM (gameplay depth — survivors feel generic without emergent personality)
**Risk:** Medium — new Core system with cross-system interactions; must not break existing UtilityAI scoring. **Revised per review: Step 6 (personality drift) individually carries higher risk than Medium because it introduces new persistent save state across 7+ bespoke event surfaces — see Review Notes.**
**Batch:** 93
**Depends on:** None (reads existing system states; does not require new data files initially). **Revised: Step 3 depends on the real `UtilityActionScorer.ApplyTraitBiases` hook remaining a no-op passthrough until this batch lands — confirm no other in-flight batch is also targeting that method before starting Step 3.**
**Unlocks:** Personality-driven dialogue, survivor relationship depth, emergent narrative moments, player attachment to individual survivors

---

## Problem Statement

Survivors have traits driven by multiple independent systems: `IdeologicalFrictionSystem`, `MoralBranchingSystem`, `TraumaBondSystem`, `RiskBiasTrait`, `CombatTraumaSystem`, `GuiltInsomniaSystem`, `SkillProgressionSystem`. These systems operate in isolation — they modify stats and fire events but never combine into a coherent behavioral identity. A "cautious + traumatized + skilled medic" survivor should behave differently from a "reckless + idealistic + strong fighter" — refusing dangerous expeditions, prioritizing medical tasks, bonding with patients. Currently they are indistinguishable stat bundles.

**Corrected framing (see Review Notes for full detail):** six of the seven named systems exist under those exact names, but their real APIs do **not** match the fields this document assumes (`PragmaticChoicesMade`, `CombatEncountersFled`, `TraumaLevel`, separate `GuiltLevel`, `PrimarySkill`/`PrimarySkillLevel` — none of these exist). `RiskBiasTrait` is not a personality/AI-bias trait at all; it is a `Journal`-namespace enum (`Paranoid, Cautious, Realist, Reckless, Denialist, Fatalist, Empath, Sociopath`) that exists solely to pick journal-entry flavor text, consumed only by `JournalVoice`. Step 2's derivation formulas as written will not compile against the real systems and must be rewritten against the real field names in Review Notes before Step 2 starts.

The `UtilityAI` system (in `Assets/Ashfall.Core/UtilityAI/`) scores actions for survivors but uses flat stat weights. It has no concept of personality bias. Social systems (`TraumaBondSystem`, `IdeologicalFrictionSystem`) compute compatibility but don't consider holistic personality alignment. Narrative encounters offer choices but don't gate options by personality profile.

The fix is a `PersonalityEngine` that derives a personality vector from existing system states (no new persistent data required) and exposes it as a bias layer consumed by UtilityAI, social systems, and narrative encounters.

---

## Step 1 — Design PersonalityProfile Struct (Trait Vector)

**Goal:** Define a compact personality representation that captures the five core behavioral dimensions derived from existing system states, enabling downstream systems to query "what kind of person is this survivor?" without coupling to each individual source system.

**Implementation:**

- Create `Assets/Ashfall.Core/Personality/PersonalityProfile.cs`:
  ```csharp
  namespace Ashfall.Core.Personality
  {
      /// <summary>
      /// Five-axis personality vector derived from existing system states.
      /// Each axis is normalized to [-1.0, +1.0].
      /// </summary>
      public readonly struct PersonalityProfile
      {
          /// Willingness to face danger; negative = cautious, positive = bold
          public float Courage { get; }

          /// Concern for others' wellbeing; negative = callous, positive = compassionate
          public float Empathy { get; }

          /// Practical vs idealistic decision-making; negative = idealistic, positive = pragmatic
          public float Pragmatism { get; }

          /// Emotional resilience; negative = volatile, positive = steady
          public float Stability { get; }

          /// Openness to new experiences/risks; negative = conservative, positive = exploratory
          public float Curiosity { get; }

          public PersonalityProfile(float courage, float empathy, float pragmatism,
                                     float stability, float curiosity);

          /// Returns the dominant trait (highest absolute value axis)
          public PersonalityAxis DominantAxis { get; }

          /// Computes compatibility score with another profile [-1, +1]
          public float CompatibilityWith(PersonalityProfile other);

          /// Returns a bias multiplier for a given action category [0.2, 2.0]
          public float BiasFor(ActionCategory category);
      }

      public enum PersonalityAxis { Courage, Empathy, Pragmatism, Stability, Curiosity }

      public enum ActionCategory
      {
          CombatAggressive, CombatDefensive, Expedition, Medical, Crafting,
          Social, Leadership, Scavenging, Research, Rest, Guard, Trade
      }
  }
  ```
- Define `PersonalityThresholds` static class:
  - Threshold constants for personality-gated behavior (e.g., `CourageBoldThreshold = 0.6f`)
  - Used by downstream systems to check "is this survivor brave enough to volunteer?"
- `PersonalityProfile` is a value type — computed on demand, not stored (derived from live state)

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles cleanly
```

**Done when (tightened — the original wording had no measurable bar):**
- `PersonalityProfile` struct, `PersonalityAxis` enum, `ActionCategory` enum, and `PersonalityThresholds` compile in `Ashfall.Core.Personality` with zero `Godot.*`/`UnityEngine.*` references (verify via `grep -r "Godot\.\|UnityEngine\." Assets/Ashfall.Core/Personality/` returning no matches, not just "looks clean").
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` exits 0 with this namespace present.
- `CompatibilityWith` and `BiasFor` are provably pure: at least one test calls each twice with identical inputs and asserts identical output (guards against an accidental static/mutable-state regression later, not just "look pure by inspection").
- `DominantAxis` has an explicit, tested tie-breaking rule (e.g. lowest enum ordinal wins) — the original draft says "highest absolute value axis" but never specifies what happens on an exact tie between two axes, which is a real edge case given five independently-computed floats.

---

## Step 2 — Create PersonalityEngine in Core (Derivation from Existing States)

**Goal:** Build the engine that computes a `PersonalityProfile` for any survivor by reading their current state across existing systems — no new persistent data, no new survivor fields, pure derivation from what already exists.

**Implementation — CORRECTED against real system APIs (see Review Notes for the full audit). The struct and formulas below replace the original draft, which referenced fields that do not exist on any real system:**

- Create `Assets/Ashfall.Core/Personality/PersonalityEngine.cs`:
  - Constructor: `PersonalityEngine(ILog log)`
  - Core method: `PersonalityProfile ComputeProfile(SurvivorPersonalityInputs inputs)`
  - `SurvivorPersonalityInputs` struct aggregates the read-only state needed. Field names below are the **real getters/fields** on each system, not aspirational ones:
    ```csharp
    public struct SurvivorPersonalityInputs
    {
        // From CombatTraumaSystem (Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs)
        // Real API: GetCombatEncountersSurvived(id), GetHypervigilanceLevel(id).
        // There is NO fled-count and NO generic "TraumaLevel" — hypervigilance is the
        // closest analog and has different semantics (defense buff / false-alarm risk,
        // not a general trauma score). Do not read CombatEncountersFled or TraumaLevel.
        public int CombatEncountersSurvived;
        public float HypervigilanceLevel;   // 0-1, proxy for "combat-worn", not "traumatized"

        // From IdeologicalFrictionSystem (Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs)
        // Real API has no single per-survivor "rigidity" scalar or conflict counter.
        // The only per-pair signal is GetAffinity(a,b) / GetRoommateCompatibilityMultiplier(a,b).
        // Treat rigidity as "how negative this survivor's affinities trend on average"
        // computed by the caller, not a field the system exposes directly.
        public float AverageAffinityWithOthers; // -1..+1, caller-aggregated from GetAffinity

        // From MoralBranchingSystem (Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs)
        // Real API tracks ONE running counter (MoralChoiceCount) and a single, irreversible
        // BranchDirection (Neutral/NumbedResilience/BurdenedCompassion) decided once at the
        // 5th choice. There are NO separate PragmaticChoicesMade/IdealisticChoicesMade tallies.
        public MoralBranchDirection BranchDirection;
        public int MoralChoiceCount;

        // From SkillProgressionSystem (Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs)
        // Real API tracks XP per-discipline across 6 fixed disciplines (medical, crafting,
        // science, combat, scavenging, survival) via GetXp(actorId, disciplineId) — uncapped,
        // NOT normalized 0-1. There is no "PrimarySkill" resolution built in; the caller must
        // pick argmax(GetXp) across the 6 disciplines itself if a "primary" concept is needed.
        public string DominantDisciplineId;      // caller-computed argmax, e.g. "medical"
        public float DominantDisciplineXp;        // raw XP, NOT 0-1 — normalize with a chosen cap

        // From NeedsSystem (note: NeedsSystem currently has NO CaptureState/RestoreState —
        // confirmed absent; morale must be read from whatever live field/property the host
        // session exposes, not a save-state DTO) / GuiltInsomniaSystem
        // Real API: GuiltInsomniaSystem exposes ONE scalar, GetInsomniaSeverity(id) (0-1).
        // "Guilt" is a list of decaying GuiltRecord entries (severity + expiry), not a
        // separate 0-1 float — GetGuiltSourceCount(id) returns a raw count, not 0-1.
        public float CurrentMorale;        // 0-1, source TBD pending host wiring
        public int GuiltSourceCount;        // raw count, NOT 0-1 — normalize with a chosen cap
        public float InsomniaSeverity;      // 0-1, real getter: GetInsomniaSeverity

        // From TraumaBondSystem (Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs)
        // Real API: GetBondStrength(a,b), GetBondCount(id), HasBond(a,b). "BondsBroken" has
        // no backing data — TraumaBondSystem only decays bond strength over time (Tick), it
        // does not track a discrete "broken" event/counter.
        public int BondCount;
        public float AverageBondStrength;

        // From RiskBiasTrait — CORRECTED: this is Ashfall.Core.Journal.RiskBiasTrait, an
        // 8-value enum (Paranoid, Cautious, Realist, Reckless, Denialist, Fatalist, Empath,
        // Sociopath) used exclusively to pick journal flavor text via ISurvivorAuthor. It is
        // NOT a -1..+1 float and has no existing wiring into AI/mechanics. If this input is
        // kept, PersonalityEngine must map the 8 enum values to a numeric bias itself (new
        // mapping table, not a reused existing conversion) and the survivor must implement
        // ISurvivorAuthor for the engine to read it at all.
        public RiskBiasTrait? JournalRiskBias; // enum, not a float; null if no journal author
    }
    ```
  - Derivation formulas (each axis independently computed, then clamped to [-1, +1]) — **rewritten against the real fields above; the original draft's formulas referenced fields that don't exist and would not compile**:
    - **Courage** = `(hypervigilanceLevel considered inversely, since high hypervigilance correlates with combat-worn caution) * -0.4 + (combatEncountersSurvived scaled/capped) * 0.3 + journalRiskBiasNumeric * 0.3`
    - **Empathy** = `(averageBondStrength) * 0.4 + (branchDirection == BurdenedCompassion ? 0.4 : branchDirection == NumbedResilience ? -0.2 : 0) + (dominantDisciplineId == "medical" ? 0.2 : 0)`
    - **Pragmatism** = `(branchDirection == NumbedResilience ? 0.3 : branchDirection == BurdenedCompassion ? -0.1 : 0) + (1 - averageAffinityWithOthers negative-clamped) * 0.3 + (dominantDisciplineXp normalized) * 0.4`
    - **Stability** = `(1 - guiltSourceCount normalized) * 0.3 + (1 - insomniaSeverity) * 0.3 + morale * 0.2 + (1 - hypervigilanceLevel) * 0.2`
    - **Curiosity** = `journalRiskBiasNumeric * 0.3 + (bondCount normalized) * 0.3 + (skill discipline variety, i.e. number of disciplines with nonzero XP, normalized) * 0.4`
  - These formulas are illustrative placeholders, same as the original draft's intent — but they must be finalized against the real getters during implementation, not against the aspirational field names removed above. Formulas use safe division (0/0 = 0). All inputs are bounds-checked.
  - Engine is stateless — recomputes every time. No caching (profile changes as states change).
  - **Open design question flagged, not resolved, by this correction:** several "source" systems above have no built-in normalization (skill XP is uncapped, guilt is a count not a 0-1 float, moral branching is a one-shot irreversible decision not a running tally). The plan's original five formulas implicitly assumed every source system already exposed a clean 0-1 signal; in reality PersonalityEngine will have to do real normalization/aggregation work the plan did not budget for. Treat Step 2 as "design the derivation AND the normalization," not just "design the derivation."

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Personality"
```

**Done when (tightened):**
- `PersonalityEngine.ComputeProfile()` compiles against the real getters listed in `SurvivorPersonalityInputs` above (`GetCombatEncountersSurvived`, `GetHypervigilanceLevel`, `GetAffinity`, `GetInsomniaSeverity`, `GetBondStrength`, `GetXp`, `MoralBranchDirection`) — not the removed aspirational field names.
- All five output axes are asserted `>= -1f && <= 1f` for: all-zero/default inputs, and at least one deliberately-extreme input combination per axis (5 tests minimum, one per axis, not one generic "extreme inputs" test).
- A specific test exists for `journalRiskBias == null` (no `ISurvivorAuthor`) producing the same Courage/Curiosity contribution as `0f`, since that mapping is new code with no prior art to copy.
- A specific test exists proving `DominantDisciplineXp` normalization does not divide by zero when a survivor has zero XP in all 6 disciplines (the real failure mode given `SkillProgressionSystem`'s uncapped-XP model, not the "PrimarySkillLevel already 0-1" assumption the original draft made).

---

## Step 3 — Wire Personality into UtilityAI Scoring

**Goal:** Integrate personality as a bias layer in the existing `UtilityAI` action scoring so survivors with different personalities naturally prefer different actions — cautious survivors avoid expeditions, empathetic survivors prefer medical tasks, bold survivors volunteer for combat.

**Implementation — CORRECTED against the real `UtilityAI` pipeline (see Review Notes). The original draft assumed a pure multiplicative `finalScore = baseScore * personalityBias * urgencyMultiplier` formula and an existing `ActionCategory` concept; neither exists. Both are addressed below:**

- Locate the real scoring pipeline: `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` (`UtilityActionDef`, `AIActionContext`), `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs` (`Score`, `ApplyTraitBiases`), `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` (`SelectAction`).
- The real formula in `UtilityActionScorer.Score` is:
  ```csharp
  float rawScore = action.EvaluateRaw(context);
  float curvedScore = action.Curve.Evaluate(rawScore);
  float score = ApplyTraitBiases((curvedScore + action.basePriority) * action.weight, action, context);
  if (context.IsListless) score -= ListlessScorePenalty;
  return action.isOverrideAction ? Math.Max(0f, score) : Math.Max(0f, Math.Min(1f, score));
  ```
  This is additive-then-multiplicative, not the three-way multiplicative chain the original draft assumed. There is no "urgency multiplier" anywhere in the real pipeline — that would be new state, not a rewire of something existing.
- **The correct integration point already exists and is currently a no-op**: `ApplyTraitBiases(float score, UtilityActionDef action, AIActionContext context)` in `UtilityActionScorer.cs` is called in exactly the right place (after curve + basePriority + weight, before the listless penalty and clamp) and currently just does `return score;` with a comment marking it as a placeholder for future soft biases. Wire personality bias here:
  ```csharp
  private float ApplyTraitBiases(float score, UtilityActionDef action, AIActionContext context)
  {
      // existing hard-veto / trait logic stays here, unchanged
      float personalityBias = _personalityProvider?.GetProfile(context.SurvivorId) is PersonalityProfile p
          ? p.BiasFor(action.Category)
          : 1.0f;
      return score * personalityBias;
  }
  ```
- **`ActionCategory` does not exist on `UtilityActionDef` today.** Categorization today is done via a free-form `string[] tags` field plus a `UtilityTags` constants class (`TagLoudLabor`, `TagWeapon`, `TagMedical`, etc.). Adding a `Category` enum property to `UtilityActionDef` is a small, additive change (mirrors the existing `tags` field) but is new work, not already present — do not describe this as "wiring into an existing category system."
- Each `UtilityAction` gains an `ActionCategory Category` property (new field; map existing action JSON defs to categories as part of this step, not assumed pre-mapped).
- `personalityBias = profile.BiasFor(action.Category)` — returns [0.2, 2.0]
- Implement `PersonalityProfile.BiasFor(ActionCategory)`:
  ```csharp
  public float BiasFor(ActionCategory category) => category switch
  {
      ActionCategory.CombatAggressive => MapBias(Courage, 0.5f, 1.8f),
      ActionCategory.CombatDefensive  => MapBias(-Courage, 0.6f, 1.4f),
      ActionCategory.Expedition       => MapBias(Courage * 0.5f + Curiosity * 0.5f, 0.3f, 1.8f),
      ActionCategory.Medical          => MapBias(Empathy, 0.4f, 1.6f),
      ActionCategory.Crafting         => MapBias(Pragmatism, 0.6f, 1.4f),
      ActionCategory.Social           => MapBias(Empathy * 0.5f + Stability * 0.5f, 0.5f, 1.5f),
      ActionCategory.Leadership       => MapBias(Stability * 0.4f + Courage * 0.3f + Pragmatism * 0.3f, 0.4f, 1.6f),
      ActionCategory.Scavenging       => MapBias(Curiosity * 0.6f + Courage * 0.4f, 0.5f, 1.5f),
      ActionCategory.Research         => MapBias(Curiosity * 0.7f + Pragmatism * 0.3f, 0.5f, 1.5f),
      ActionCategory.Rest             => MapBias(-Stability, 0.6f, 1.4f),  // unstable seeks rest
      ActionCategory.Guard            => MapBias(Courage * 0.5f + Stability * 0.5f, 0.5f, 1.5f),
      ActionCategory.Trade            => MapBias(Pragmatism * 0.6f + Empathy * -0.2f + Curiosity * 0.2f, 0.5f, 1.5f),
      _ => 1.0f
  };

  private static float MapBias(float axis, float minBias, float maxBias)
      => minBias + (axis + 1f) / 2f * (maxBias - minBias);  // maps [-1,1] → [min,max]
  ```
- **Non-breaking:** If no `PersonalityProfile` is provided (null/default), bias = 1.0 for all categories (existing behavior preserved) — this matches how `ApplyTraitBiases` is already a passthrough today, so the non-breaking guarantee holds naturally.
- Add `IPersonalityProvider` interface so UtilityAI doesn't depend on PersonalityEngine directly:
  ```csharp
  public interface IPersonalityProvider
  {
      PersonalityProfile? GetProfile(string survivorId);
  }
  ```
  `UtilityActionScorer` (or whatever constructs it) takes an optional `IPersonalityProvider` — check the real constructor signature before assuming it can be added without touching every call site; `UtilityActionScorer` may currently be constructed as a stateless/static-style helper (`Score` takes `action` and `context` directly with no scorer-level constructor dependencies per the Review Notes audit), in which case the provider more naturally belongs on `AIActionContext` (already carries `SurvivorId`) rather than the scorer itself. Confirm the real constructor/call shape before implementing.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # all existing UtilityAI tests still pass
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Personality"
```

**Done when (tightened):**
- Personality bias is wired specifically through `UtilityActionScorer.ApplyTraitBiases` (the real, already-existing hook), confirmed by a code-level check that no other formula shape (e.g. a new parallel `finalScore = base * bias * urgency` path) was introduced alongside it.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — the **full** suite passes with zero regressions (not just UtilityAI-filtered tests), since `ApplyTraitBiases` is a shared code path every existing action score already flows through.
- A test confirms a null/absent `IPersonalityProvider` reproduces byte-for-byte identical scores to the pre-change baseline for a fixed set of `(action, context)` pairs (a real before/after diff, not just "still passes").
- `ActionCategory` mapping for existing action JSON defs is complete: a test enumerates every `UtilityActionDef` loaded from data and asserts none resolve to the `_ => 1.0f` default case silently (i.e. every shipped action has been deliberately categorized, not left as an unmapped fallback).

---

## Step 4 — Wire Personality into Social Systems

**Goal:** Integrate personality compatibility into the social systems so compatible personalities bond faster, conflicting personalities generate friction faster, and the shelter's social dynamics feel emergent rather than stat-driven.

**Implementation — CORRECTED hook points (see Review Notes). `TraumaBondSystem` and `IdeologicalFrictionSystem` do not have fields literally named "bond-strength gain" or "friction gain" — the real accumulating quantities and constants are named differently and are shaped differently than the plan assumed:**

- Extend `TraumaBondSystem` bonding rate:
  - Real hook point: `ProcessBondPair` (private method) applies `bond.BondStrength = Min(1f, bond.BondStrength + BondStrengthPerSharedHazard)` where `BondStrengthPerSharedHazard = 0.30f` is a class constant. `OnSharedHazardEndured(List<string> participantIds, string hazardId)` takes a **list of participants**, not a pair, and applies `ProcessBondPair` to every combination inside a nested loop — a compatibility multiplier must be applied per-pair inside that loop, not as a single top-level scalar as the original draft implied:
    - `bondGain *= 1.0f + profile1.CompatibilityWith(profile2) * 0.5f` — apply this inside the nested loop, once per (survivorA, survivorB) combination, not once per hazard event.
    - Compatible personalities (score > 0.5): bond 25% faster
    - Incompatible personalities (score < -0.5): bond 25% slower (but can still bond through shared trauma)
- Extend `IdeologicalFrictionSystem` friction rate:
  - **There is no scalar "friction" field to modulate.** The real accumulating value is `_affinities[pairKey]` (a signed float), driven by `TickRoommates(survivorA, survivorB, gameHours)`, which applies two constants: `ConflictAffinityDrainPerDay = 2f` (drains affinity — the plan's "friction") and `SynergyAffinityGainPerDay = 1f` (raises affinity — the plan's "compatibility"). Personality compatibility should modulate these two constants directly inside `TickRoommates`, inverted relative to the plan's original framing since affinity is the inverse of friction:
    - `effectiveDrain = ConflictAffinityDrainPerDay * (1.0f - profile1.CompatibilityWith(profile2) * 0.3f)` — compatible pairs drain affinity 30% slower (friction builds slower)
    - `effectiveGain = SynergyAffinityGainPerDay * (1.0f + profile1.CompatibilityWith(profile2) * 0.3f)` — compatible pairs gain affinity 30% faster
  - Belief conflict itself is keyed off a static `ConflictGroups` dictionary lookup, not a numeric score — personality does not change *whether* two belief profiles conflict, only how fast the resulting affinity drain/gain accumulates.
- Implement `PersonalityProfile.CompatibilityWith()`:
  ```csharp
  public float CompatibilityWith(PersonalityProfile other)
  {
      // Complementary model: similar stability/empathy is good,
      // but opposed courage/curiosity can complement
      float similarity = (
          CosineSimilarity(Empathy, other.Empathy) * 0.3f +
          CosineSimilarity(Stability, other.Stability) * 0.3f +
          CosineSimilarity(Pragmatism, other.Pragmatism) * 0.2f +
          Math.Abs(Courage - other.Courage) < 0.5f ? 0.1f : -0.1f +  // moderate courage gap is fine
          Math.Abs(Curiosity - other.Curiosity) < 0.8f ? 0.1f : -0.1f
      );
      return Math.Clamp(similarity, -1f, 1f);
  }
  ```
  **Bug flagged, not fixed, in the original draft's own sample code:** the ternary chain above has a C# operator-precedence bug — `Math.Abs(...) < 0.5f ? 0.1f : -0.1f + Math.Abs(...)` binds `+` before the second ternary's condition is evaluated as intended, because `?:` has lower precedence than `+` on its false-branch operand in this specific chained-without-parens form. Wrap each conditional term in its own parentheses before implementing, and add a unit test asserting the sign of each term independently (this is exactly the kind of defect `CompatibilityWith_SimilarProfiles_Positive`/`CompatibilityWith_OpposedProfiles_Negative` in Step 7 should have caught, but only if the test author checks the actual returned value against a hand-computed expectation rather than just asserting `> 0` / `< 0` loosely).
- Add personality-driven social events:
  - Survivors with high empathy + low stability → more likely to trigger "emotional outburst" events
  - Survivors with high courage + low empathy → more likely to volunteer others for dangerous tasks (conflict source)
  - Survivors with matched high curiosity → "deep conversation" bond event (bonus bonding)
  - **Scope flag:** none of these three "personality-driven social events" exist today in any form (not stubbed, not planned elsewhere) — this bullet is net-new event/encounter design, not a modification of an existing event. Estimate accordingly; this is easily its own sub-step, not a one-line addition to an existing event.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # existing social system tests still pass
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Personality"
```

**Done when (tightened):**
- The `CompatibilityWith` precedence bug flagged above is fixed (parenthesized correctly) and covered by a test with a hand-computed expected value, not just a sign check.
- `TickRoommates` and `ProcessBondPair` modulation is applied per-pair inside their existing loops (confirmed by reading the diff, not just by the top-level test passing) — a single top-level scalar multiply on the caller side would be the wrong integration point given `OnSharedHazardEndured` takes a participant list.
- Full existing test suite (`dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`) passes with zero regressions — `TraumaBondSystem`/`IdeologicalFrictionSystem` already have dedicated test files per the codebase; both must be re-run, not assumed unaffected.
- A test demonstrates the case AGENTS.md determinism invariant requires: identical seed + identical personality inputs produce identical affinity/bond-strength trajectories across two independent runs.

---

## Step 5 — Wire Personality into Narrative Encounters

**Goal:** Make narrative encounter choices personality-aware so certain options only appear (or are more likely to succeed) based on a survivor's personality profile — creating moments where players think "of course she would do that."

**Implementation:**

- Extend encounter choice data in `Assets/StreamingAssets/Data/` encounter definitions:
  - Add optional `personality_gate` field to encounter choices:
    ```json
    {
      "choice_id": "encounter_vault_04_charge",
      "text": "Rush the entrance before they notice",
      "personality_gate": {
        "axis": "courage",
        "min_value": 0.3,
        "label": "bold"
      }
    }
    ```
  - `personality_gate` is optional — choices without it are always available
  - Multiple gates can be specified (all must pass): `"personality_gates": [...]`
- Create `Assets/Ashfall.Core/Personality/PersonalityGateEvaluator.cs`:
  - `bool CanChoose(PersonalityProfile profile, PersonalityGate gate)` — checks threshold
  - `float SuccessModifier(PersonalityProfile profile, PersonalityGate gate)` — scales outcome probability
  - A survivor above the gate threshold: option visible + success bonus (+10-20%)
  - A survivor below but within 0.2 of threshold: option visible but penalty ("against their nature")
  - A survivor far below threshold: option hidden (they wouldn't even consider it)
- Extend encounter resolution:
  - When resolving an encounter with a personality-gated choice selected:
    - Survivors acting "in character" (axis well above threshold): slight success bonus
    - Survivors acting "against character" (axis barely meets threshold): stability penalty (internal conflict)
  - This creates meaningful player decisions: send the cautious medic on a dangerous run (she might succeed but it'll cost her stability)
- Update `CatalogIntegrityValidator` to validate `personality_gate` fields (axis must be a valid `PersonalityAxis` name in snake_case)

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~PersonalityGate"
godot --headless --path . -- --data-integrity-selftest   # validates new gate fields
```

**Done when (tightened):**
- `personality_gate`/`personality_gates` fields parse from at least one real encounter JSON file under `Assets/StreamingAssets/Data/` (not just a hand-written test fixture) and pass `--data-integrity-selftest` with 0 errors.
- `CatalogIntegrityValidator` rejects a gate referencing an invalid axis name (e.g. `"axs": "couragee"`) with a specific error message identifying the offending choice id — add a negative test, not only a positive one.
- Threshold-boundary behavior is pinned by exact test values: at threshold, at threshold+0.2 (visible-with-penalty band the plan describes), and at threshold-0.01 (hidden) — the "0.2 penalty band" and "far below = hidden" language in the Implementation section needs exact numeric boundaries decided and tested here, not left as prose.
- **Dependency note added:** this step assumes `PersonalityProfile` (Step 1-2) is already computable per survivor at encounter-resolution time. Confirm the Godot host's narrative encounter resolution path actually has a survivor id available to look up a profile before wiring — narrative encounters resolved via `ExpeditionEncounterBridge` (per AGENTS.md) may resolve at a point where a specific survivor identity isn't yet bound; verify this against the real encounter resolution flow before assuming the hook point exists.

---

## Step 6 — Add Personality Drift (Trauma and Experience Shift Personality)

**Goal:** Make personality a living system that evolves over time — traumatic events erode stability, successful expeditions boost courage, acts of kindness increase empathy — so survivors feel like they grow and change rather than being static archetypes.

**Implementation:**

- Create `Assets/Ashfall.Core/Personality/PersonalityDriftSystem.cs`:
  - Implements `CaptureState()` / `RestoreState()` for save/load
  - Persistent state: `Dictionary<string, PersonalityDriftState> _driftBySurvivor`
  - `PersonalityDriftState`:
    ```csharp
    public class PersonalityDriftState
    {
        public float CourageOffset;    // accumulated drift [-0.5, +0.5]
        public float EmpathyOffset;
        public float PragmatismOffset;
        public float StabilityOffset;
        public float CuriosityOffset;
    }
    ```
  - Drift is additive to the computed profile: `finalAxis = computedAxis + driftOffset`, clamped to [-1, +1]
  - Drift accumulates from events:
    - **Traumatic combat** → stability -0.05, courage -0.03
    - **Successful combat** → courage +0.04, stability +0.02
    - **Healing another survivor** → empathy +0.03
    - **Failed expedition** → curiosity -0.04, courage -0.02
    - **Successful expedition discovery** → curiosity +0.05, courage +0.02
    - **Betrayal by bonded survivor** → empathy -0.06, stability -0.04
    - **Long period without crisis** → stability +0.02 per 5 days (natural recovery)
  - Drift is slow and bounded: max ±0.5 per axis. A cautious person can become braver but never reckless.
  - Drift decay: extreme offsets decay toward 0 at 0.005/day (personality resists permanent change)
- Wire drift events:
  - `PersonalityDriftSystem` listens to system events (combat resolved, expedition completed, bond broken)
  - **Corrected (see Review Notes):** there is no single unified event bus to subscribe to for this. AGENTS.md's own Event System section documents two parallel buses (`IEventBus`/`SimpleEventBus`, underused, and the Unity-only `EventBus` static class) plus "Godot: No bus — direct method calls." In practice each source system exposes its **own bespoke C# events** — e.g. `CombatTraumaSystem` has `OnHypervigilanceIncreased`, `OnFalseAlarmTriggered`, `OnShelterFalseAlarm`, `OnStateChanged` (confirmed at `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs:46-49`); `TraumaBondSystem` has `OnTraumaBondFormed`, `OnTraumaBondDecayed`, `OnCoShiftBonusApplied`, `OnStateChanged`. `PersonalityDriftSystem` will need a **separate subscription per source system** (7+ distinct event surfaces, no shared interface), not one generic "listen to events" hookup. Budget Step 6 accordingly — this is closer to writing 7 small adapters than wiring one listener.
  - Note also: `TraumaBondSystem` has no "broken" concept (only formation via `OnSharedHazardEndured`/`ProcessBondPair` and passive decay via `Tick`) — a "betrayal by bonded survivor" drift trigger (listed below) has no existing event to hang off; it is net-new event/mechanic design, not a subscription to something that already fires.
  - Each event type maps to a drift delta via a data table (not hardcoded — load from JSON)
- Create `Assets/StreamingAssets/Data/personality_drift_events.json`:
  ```json
  {
    "schema_version": 1,
    "drift_events": [
      {
        "event_key": "combat_traumatic",
        "deltas": { "stability": -0.05, "courage": -0.03 }
      }
    ]
  }
  ```

**Scope risk — flagged per adversarial review, not resolved by this document alone:** Step 6 is the step most likely to grow unbounded. It proposes a system that (a) subscribes to 7+ independently-shaped event surfaces across systems that were never designed to be observed together, (b) invents at least one event that doesn't exist yet (bond "betrayal"/"broken"), (c) introduces persistent per-survivor state (`PersonalityDriftState`) with its own save/load contract on top of an already-94-system save surface, and (d) is explicitly said to feed back into Steps 3-5 (UtilityAI bias, social compatibility, narrative gates) — meaning a bug in drift math silently propagates into three other systems' behavior. Recommend treating Step 6 as **optional / deferrable** relative to Steps 1-5: ship the derived (non-drifting) `PersonalityProfile` first, validate Steps 3-5 against it in isolation, and only add Step 6's drift layer once the static profile is proven stable in playtesting. Doing Step 6 in the same pass as Steps 3-5 makes it much harder to tell whether an observed behavior change (e.g. "survivors seem to avoid medical tasks now") is caused by the bias wiring (Step 3) or by drift silently degrading empathy over time (Step 6) — the two need to be validated independently before being combined.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~PersonalityDrift"
godot --headless --path . -- --data-integrity-selftest   # validates drift events JSON
```

**Done when (tightened):**
- Per the scope-risk note above, Step 6 ships behind a flag or as a separately-mergeable change from Steps 3-5, so a regression can be bisected to drift math specifically rather than to "personality" generically.
- Each of the 7 drift-triggering event subscriptions listed (combat traumatic/successful, healing, failed/successful expedition, betrayal, natural recovery) is backed by a real, currently-firing event **except** "betrayal by bonded survivor," which is flagged above as having no existing event — that trigger is descoped to a follow-up batch unless a new event is designed and reviewed as its own deliverable first.
- `PersonalityDriftState` implements `CaptureState()`/`RestoreState()` following the deep-copy discipline documented elsewhere in this codebase (see `BrineWaterSystem.cs`, `CensusClaimSystem.cs`, `DutyRosterSystem.cs` deep-copy comments) — a test explicitly mutates a restored copy and asserts the original is untouched, per the existing project pattern (e.g. `CoalitionCampSystemTests.CaptureState_ReturnsSnapshotNotLiveState`).
- Bounds test asserts `[-0.5, +0.5]` clamp holds after 100+ synthetic drift events applied to one axis (not just 1-2 events), and decay test asserts the stated `0.005/day` rate empirically, with an exact numeric assertion, not a directional "it decreased" check.

---

## Step 7 — Write Personality Tests (Derivation, Bias, Drift, Integration)

**Goal:** Comprehensive test coverage ensuring personality derivation is deterministic, bias calculations are bounded, drift accumulates and decays correctly, compatibility produces meaningful results, and the system integrates cleanly with UtilityAI and social systems.

**Implementation:**

- Create `Ashfall.Core.Tests/PersonalityProfileTests.cs`:
  - `DefaultInputs_ProducesNeutralProfile` — all-zero inputs → (0,0,0,0,0)
  - `HighCombatExperience_LowFleeRate_IncreaseCourage` — derivation formula
  - `HighEmpathy_BiasesMedicalActions` — bias mapping
  - `LowCourage_ReducesExpeditionBias` — bias suppression
  - `BiasFor_AlwaysWithinBounds` — [0.2, 2.0] invariant for all axes and categories
  - `CompatibilityWith_SimilarProfiles_Positive` — social compatibility
  - `CompatibilityWith_OpposedProfiles_Negative` — social incompatibility
  - `CompatibilityWith_Self_IsMaximal` — identity case
  - `DominantAxis_ReturnsHighestAbsoluteValue` — correct identification

- Create `Ashfall.Core.Tests/PersonalityEngineTests.cs`:
  - `ComputeProfile_NullRiskBias_UsesZero` — optional input handling
  - `ComputeProfile_ZeroDivision_Safe` — no encounters = no crash
  - `ComputeProfile_ExtremeInputs_ClampedToRange` — bounds enforcement
  - `ComputeProfile_DeterministicGivenSameInputs` — reproducibility
  - `ComputeProfile_MedicSkill_BoostsEmpathy` — skill influence
  - `ComputeProfile_HighTrauma_ReducesStability` — trauma influence

- Create `Ashfall.Core.Tests/PersonalityDriftSystemTests.cs`:
  - `ApplyDriftEvent_AccumulatesCorrectly` — basic accumulation
  - `ApplyDriftEvent_AtMaxOffset_Clamped` — bounds enforcement
  - `DriftDecay_OverTime_ReducesOffset` — natural recovery
  - `SaveLoadRoundTrip_PreservesDrift` — persistence
  - `SaveLoadRoundTrip_Checksum_ChangesOnDrift` — integrity
  - `MultipleEvents_SameAxis_Stack` — cumulative drift
  - `Drift_AppliedToProfile_ShiftsAxis` — integration with derivation

- Create `Ashfall.Core.Tests/PersonalityIntegrationTests.cs`:
  - `CautiousSurvivor_UtilityAI_PrefersDefensiveActions` — end-to-end
  - `BoldSurvivor_UtilityAI_PrefersExpeditions` — end-to-end
  - `CompatibleSurvivors_BondFaster` — social integration
  - `IncompatibleSurvivors_FrictionFaster` — social integration
  - `PersonalityGate_BelowThreshold_ChoiceHidden` — narrative integration
  - `PersonalityGate_AboveThreshold_ChoiceVisible` — narrative integration
  - `DriftAfterTrauma_ChangesActionPreference` — full loop

- Create `Ashfall.Core.Tests/PersonalityGateEvaluatorTests.cs`:
  - `CanChoose_WellAboveThreshold_True` — standard pass
  - `CanChoose_WellBelowThreshold_False` — standard fail
  - `CanChoose_NearThreshold_True_WithPenalty` — edge case
  - `SuccessModifier_InCharacter_Bonus` — reward alignment
  - `SuccessModifier_AgainstCharacter_Penalty` — penalize misalignment
  - `MultipleGates_AllMustPass` — compound gates

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Personality"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full suite — no regressions
```

**Done when (tightened):**
- All new Personality* test files pass, and the **full** suite (`dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` with no filter) passes — report the exact before/after test count (per Batch 76's precedent of citing an exact, re-verified count rather than a stale figure) so regressions are visible as a count delta, not just a green checkmark.
- `dotnet build Ashfall.csproj` reports 0 errors, 0 warnings (per the project's own verification checklist — the original draft's Step 7 verification block omitted the Godot host build entirely, even though Steps 3-5 modify Core code the Godot host depends on).
- `godot --headless --path . -- --data-integrity-selftest` passes with 0 errors if Step 5's JSON gate fields shipped.

---

## Summary Table

| Step | Deliverable | Layer | Key Files | Risk |
|------|-------------|-------|-----------|------|
| 1 | `PersonalityProfile` struct + enums + thresholds | Core | `Assets/Ashfall.Core/Personality/PersonalityProfile.cs` | None |
| 2 | `PersonalityEngine` (derivation from existing states) | Core | `Assets/Ashfall.Core/Personality/PersonalityEngine.cs` | Low |
| 3 | UtilityAI personality bias integration | Core | `Assets/Ashfall.Core/UtilityAI/` (modified), `IPersonalityProvider.cs` | Medium |
| 4 | Social system compatibility modulation | Core | `TraumaBondSystem.cs`, `IdeologicalFrictionSystem.cs` (modified) | Medium |
| 5 | Narrative encounter personality gates | Core + Data | `PersonalityGateEvaluator.cs`, encounter JSON (extended) | Low |
| 6 | Personality drift system (persistent evolution) | Core + Data | `PersonalityDriftSystem.cs`, `personality_drift_events.json` | Medium |
| 7 | Test suite (derivation + bias + drift + integration) | Tests | `Ashfall.Core.Tests/Personality*Tests.cs` | None |

---

## Architecture Notes

- **Engine-agnostic:** All personality logic lives in `Ashfall.Core.Personality`. No `Godot.*` or `UnityEngine.*`. The Godot host only needs to assemble `SurvivorPersonalityInputs` from its runtime sessions and pass them to the engine.
- **Derived, not stored:** The base `PersonalityProfile` is computed on-demand from existing system states. Only `PersonalityDriftState` is persisted (via `CaptureState/RestoreState`). This means personality automatically reflects the survivor's lived experience without manual synchronization.
- **Deterministic:** Same inputs → same profile, always. Uses `ISeededRng` for any stochastic drift events. No `System.Random`.
- **Non-breaking:** All integrations are additive. Null/default personality = 1.0 bias multiplier everywhere. Existing tests pass without modification. Systems that don't have personality support yet continue working as before.
- **Bounded:** All axes are clamped to [-1, +1]. Drift offsets capped at [-0.5, +0.5]. Bias multipliers capped to [0.2, 2.0]. No axis can produce degenerate behavior.
- **Data-driven drift:** Drift event mappings live in JSON (`personality_drift_events.json`), not hardcoded. Designers can tune personality evolution without code changes.

---

## Design Principles

| Principle | Implementation |
|-----------|---------------|
| Emergent, not scripted | Personality is derived from actual experiences, not assigned at spawn |
| Observable, not invisible | Players should eventually see personality reflected in action choices and social dynamics |
| Gradual, not sudden | Drift is slow (±0.05 per event, ±0.5 max). Personality evolves, not flips |
| Reversible, not permanent | Drift decays naturally. Extreme events can shift personality but time heals |
| Complementary, not replacing | Personality biases existing systems, doesn't override them. A starving bold survivor still eats |
| Testable, not fuzzy | Every derivation formula is a pure function. Every threshold is a constant. Every interaction is unit-testable |

---

## Relationship to Existing Systems

**Corrected (see Review Notes below — several of these rows describe interactions with fields that don't exist on the named system; row content updated to match the real APIs verified during review):**

| Existing System | Interaction |
|-----------------|-------------|
| `UtilityAI` (`UtilityActionScorer.ApplyTraitBiases`) | Consumes `BiasFor()` as a multiplier inside the existing (currently no-op) trait-bias hook, not a new top-level formula term |
| `TraumaBondSystem` | Compatibility modulates the `BondStrengthPerSharedHazard` gain applied per-pair inside `ProcessBondPair` |
| `IdeologicalFrictionSystem` | Compatibility modulates `ConflictAffinityDrainPerDay`/`SynergyAffinityGainPerDay` inside `TickRoommates` (affinity is the inverse of "friction") |
| `CombatTraumaSystem` | Source input for Courage (inversely, via `GetHypervigilanceLevel`) and Stability derivation — NOT a direct "trauma level" field, which doesn't exist on this system |
| `GuiltInsomniaSystem` | Source input for Stability derivation via `GetInsomniaSeverity` (0-1) and `GetGuiltSourceCount` (raw count, needs caller-side normalization) — there is no separate 0-1 "GuiltLevel" |
| `MoralBranchingSystem` | Source input for Empathy/Pragmatism derivation via the one-shot `BranchDirection` enum, not running dual choice counters |
| `SkillProgressionSystem` | Source input for Empathy/Pragmatism/Curiosity derivation via per-discipline `GetXp`, with the caller computing a "dominant discipline" itself — there is no built-in `PrimarySkill` concept |
| `RiskBiasTrait` (`Ashfall.Core.Journal`) | Optional source input for Courage/Curiosity, but this is a journal-flavor-text enum with no existing numeric mapping or AI wiring — PersonalityEngine must invent and own that mapping, and the survivor must implement `ISurvivorAuthor` for it to be readable at all |
| `ExpeditionSystem` | Source of drift events (success/failure) — Step 6 only; verify these events exist on `ExpeditionSystem` with the exact names assumed before wiring (not independently verified in this review pass) |
| `NeedsSystem` | Morale as Stability input — **flagged:** `NeedsSystem` has no `CaptureState`/`RestoreState` today (confirmed absent), so the morale read path must go through whatever live host-session field currently exposes it, not a save-state DTO |
| `CatalogIntegrityValidator` | Validates `personality_gate` JSON (drift event JSON in Step 6 needs its own schema validation added — not automatically covered by the existing `scenario_`-style prefix validation) |
| `SaveChecksum` | Drift state covered by existing reflection-based checksum, **provided** `PersonalityDriftState` uses public fields (not properties) for every value that must be hashed — `SaveChecksum`'s `WriteObject` reflects over public fields only (see Batch 94 Review Notes for the concrete failure mode when this assumption is violated with a `Dictionary`-shaped or property-only DTO) |

---

## Review Notes (Corrected)

This section documents the adversarial fact-check performed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and lists every correction applied above.

### Factual corrections applied

1. **Named systems exist, but their APIs don't match the plan's assumptions.** Six of the seven systems named in the Problem Statement exist under those exact names (`IdeologicalFrictionSystem`, `MoralBranchingSystem`, `TraumaBondSystem`, `CombatTraumaSystem`, `GuiltInsomniaSystem`, `SkillProgressionSystem` — all in `Assets/Ashfall.Core/Survivors/`). None of them expose the specific fields the original `SurvivorPersonalityInputs` struct assumed (`CombatEncountersTotal`/`CombatEncountersFled`/`TraumaLevel`, `IdeologicalRigidity`/`ConflictsInitiated`, `PragmaticChoicesMade`/`IdealisticChoicesMade`, `PrimarySkill`/`PrimarySkillLevel`, separate `GuiltLevel`, `BondsFormed`/`BondsBroken`). The struct in Step 2 has been rewritten against the real getters.
2. **`RiskBiasTrait` is not a personality/bias trait.** It is `Ashfall.Core.Journal.RiskBiasTrait`, an 8-value enum (`Paranoid, Cautious, Realist, Reckless, Denialist, Fatalist, Empath, Sociopath`) used exclusively to select journal-entry flavor text via `ISurvivorAuthor`, consumed only by `JournalVoice`. It has zero existing wiring into AI, combat, or social mechanics. Two enum members' doc comments describe *intended future* morale hookups, but no code implements them today. This document now treats it as an optional, newly-mapped input, not a reused existing personality signal.
3. **`NeedsSystem` and `RadiationSystem` are not currently save-capable.** Neither has a `CaptureState`/`RestoreState` pair (confirmed absent by search). Any plan step that reads morale/radiation as a personality input must go through a live host-session field, not a save-state DTO, until/unless those systems gain save support separately.
4. **The UtilityAI scoring formula is additive-then-multiplicative, not the pure multiplicative chain the plan assumed.** The real formula (`UtilityActionScorer.Score`) is `ApplyTraitBiases((curvedScore + basePriority) * weight, ...)`, followed by a listless penalty and a clamp. There is no "urgencyMultiplier" anywhere in the real pipeline. `ApplyTraitBiases` is a genuine, already-wired, currently-no-op hook in exactly the right place in the pipeline — Step 3 has been rewritten to occupy that hook rather than inventing a parallel formula.
5. **`ActionCategory` does not exist today.** UtilityAI categorizes actions via a free-form `string[] tags` field plus a `UtilityTags` constants class, not an enum. Adding `ActionCategory` is real new work (small, additive, but not "already there").
6. **`TraumaBondSystem`/`IdeologicalFrictionSystem` have no fields literally named "bond-strength gain" or "friction gain."** The real constants are `BondStrengthPerSharedHazard` (0.30f, applied in `ProcessBondPair`) and `ConflictAffinityDrainPerDay`/`SynergyAffinityGainPerDay` (2f/1f, applied in `TickRoommates`). Step 4 has been rewritten to modulate these specific, real values, applied per-pair inside the existing loops rather than as a single top-level scalar.
7. **`CompatibilityWith`'s sample code has an operator-precedence bug.** The ternary chain in the original draft binds incorrectly without explicit parentheses around each conditional term. Flagged inline in Step 4; must be fixed before implementation, with a test asserting a hand-computed expected value.
8. **There is no unified event bus for `PersonalityDriftSystem` to subscribe to.** Per AGENTS.md's own Event System section, the codebase has two parallel, mostly-unused buses plus direct method calls in the Godot host. Each source system (`CombatTraumaSystem`, `TraumaBondSystem`, etc.) exposes its own bespoke C# events. Step 6 now documents this as 7+ separate subscriptions, not one listener, and flags that "betrayal by bonded survivor" has no backing event on `TraumaBondSystem` today (only formation and passive decay exist).

### Number correction

No specific system-count figure was stated in the original Batch 93 draft, so there is no "82+" vs "94" correction needed in this file directly — but see the cross-reference below: the same "~94 systems implement CaptureState/RestoreState" figure that Batch 76 independently verified (by counting `public ... CaptureState(` definitions) was re-confirmed during this review (94 files under `Assets/Ashfall.Core/` match `public [A-Za-z].* CaptureState\(`, 93 match `RestoreState\(` — a 1-file asymmetry worth a separate, small investigation, out of scope for this batch). This matters here because Step 6 adds a 95th/96th stateful system (`PersonalityDriftSystem`) to that surface, and its "Done when" criteria have been tightened to require the same deep-copy discipline already documented on ~94 existing systems.

### Scope-creep risk: personality drift (Step 6)

Flagged and addressed inline above. Summary: personality drift is the correct feature to defer or gate separately from Steps 1-5, because it (a) requires bespoke event wiring across 7+ unrelated systems with no shared interface, (b) invents at least one event that doesn't exist, (c) adds new persistent per-survivor state to an already-large save surface, and (d) feeds back into three other systems' runtime behavior (UtilityAI, social compatibility, narrative gates), making regressions hard to attribute. This document now recommends shipping the static/derived `PersonalityProfile` (Steps 1-5) first and validating it in isolation before adding drift.

### Missing risk/rollback notes (added)

The original draft had no explicit rollback plan for any step. Added:
- **Steps 1-2 (pure Core additions):** rollback is trivial — delete the new files, nothing else references them yet. No production risk.
- **Step 3 (UtilityAI wiring):** rollback risk is real because `ApplyTraitBiases` is a shared hook every existing action score already flows through. Rollback = revert the `ApplyTraitBiases` body to `return score;` and remove the `Category` field consumption; because the hook was already a no-op passthrough, a clean revert restores exact prior behavior. Recommend landing Step 3 behind a null/absent `IPersonalityProvider` default (already specified) so it can ship disabled and be enabled per-save or per-build without a code revert.
- **Step 4 (social systems):** higher risk than Step 3 because it changes numeric constants (`ConflictAffinityDrainPerDay` etc.) inside existing tested methods (`TickRoommates`, `ProcessBondPair`). Rollback = revert to the unmodulated constant multiply; recommend a feature flag (e.g. `bool EnablePersonalityCompatibility` defaulting false) rather than relying on a code revert if a balance issue is found post-ship, since these systems drive long-running save-persisted state (`_affinities`, bond strengths) that a later code revert cannot retroactively "unbias."
- **Step 5 (narrative gates):** rollback = the `personality_gate` field is additive/optional in JSON; removing it from data files (not code) is sufficient to disable per-encounter without a code change. Low risk confirmed.
- **Step 6 (drift):** highest rollback risk of all six steps, because it's the only step introducing new persistent save state. If shipped and later found broken, existing saves will already contain `PersonalityDriftState` entries; a rollback must either (a) ship a migration that discards drift state safely (the codebase's existing versioned-migration pattern, e.g. `HoldfastSaveCodec`, is the right precedent to follow) or (b) make drift reading defensive against a missing/stale block indefinitely. This must be decided before Step 6 starts, not discovered after a broken drift build has already been played and saved.

### Ordering check

Steps 1→2→3/4/5→6→7 as originally ordered is logically sound (profile before consumers, consumers before drift, tests last) and this review did not find an illogical ordering issue. The one addition: Step 6 should be reordered to run *after* Steps 3-5 have been validated in production/playtesting in isolation (see scope-risk note), not merely after them in document order — the original document's linear numbering implied a tight sequential dependency chain with no validation gate in between, which this review adds explicitly.

---

## Next Prompt

```
Implement Step 1 of Batch 93: Design PersonalityProfile struct in Assets/Ashfall.Core/Personality/
with the five-axis model (Courage, Empathy, Pragmatism, Stability, Curiosity), PersonalityAxis enum,
ActionCategory enum, and PersonalityThresholds. Follow Invariant 1. Verify with dotnet build.
```
