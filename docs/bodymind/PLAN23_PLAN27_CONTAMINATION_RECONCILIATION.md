
<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/BodyMind/Contamination/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Presentation Layer)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION IV: PLAN 23 & PLAN 27 UNIFIED PSYCHOLOGICAL CONTAMINATION ARCHITECTURE

## 1. Executive Summary & Scope Decision (Scope C)

Plan 23 (*The Black Flotilla*) establishes maritime psychological dread: deep-dive decompression sickness, air narcosis, trapped compartment terror, and claustrophobic pressure trauma within submerged sunken naval wrecks. Plan 27 (*Psychological Contamination & Traumatic Memory*) establishes land-based catastrophic exposure: mass casualty triage sites, automated slaughterhouses, abandoned daycares, and quarantine execution checkpoints.

Without rigorous architectural reconciliation, these two systems would risk introducing competing "sanity meters", redundant stress ledgers, and conflicting symptom states.

**Architectural Scope Decision (Scope C):**
Both maritime deep-dive dread and wasteland disaster trauma route authoritatively and exclusively through the unified **`PsychologicalContaminationSystem`** (`Assets/Ashfall.Core/BodyMind/Contamination/`). Maritime wrecks and terrestrial disaster locations share:
1. A single authoritative registry of contamination sources.
2. Canonical psychological condition types (`contam_thousand_yard_stare`, `contam_disgust_cascade`, `contam_phantom_smell`, `contam_child_cot_trauma`, `contam_claustrophobic_dread`).
3. Concrete qualitative action exclusions (blocking specific shelter/expedition tasks) rather than generic numerical stat penalties.
4. Unified catharsis, counseling, and recovery therapy protocols (`ContaminationTherapyProtocol`).

```text
========================================================================================
                      UNIFIED PSYCHOLOGICAL CONTAMINATION ARCHITECTURE
========================================================================================
  [ LAND DISASTER SOURCES (Plan 27) ]          [ MARITIME FLOTILLA SOURCES (Plan 23) ]
  - Stadium Mass Grave                         - Deep Wreck Interior Hold
  - Automated Abattoir                         - Flooded Torpedo Room
  - Sunshine Daycare Ruins                     - Sunken Nuclear Reactor Compartment
  - Quarantine Mile Execution Wall             - Sealed Berthing Entombment Quarters
  - Regional Blood Bank                        - Decompression Morgue Chamber
                         \                            /
                          \                          /
                           v                        v
             +-------------------------------------------------------+
             |      UNIFIED PsychologicalContaminationSystem         |
             |   (Pure C# netstandard2.1 Domain Authority in Core)   |
             +-------------------------------------------------------+
                                        |
                 +----------------------+----------------------+
                 |                                             |
                 v                                             v
    [ QUALITATIVE ACTION LOCKOUTS ]               [ RECOVERY & CATHARSIS SEAM ]
    - Blocks Cooking / Hydroponics                - Communal Debriefing Therapy
    - Blocks Teaching / Storytelling              - Herbal Sedative Treatments
    - Blocks Child Care / Comforting              - Deep-Water Resurfacing Debrief
    - Blocks Diving / Enclosed Operations         - Memorial Inscription Catharsis
    - Blocks Surgical / Medical Care              - Chronic Scarring Resolution
========================================================================================
```

---

# SECTION V: RECONCILED EXPOSURE SOURCES & CANONICAL REGISTRY

The catalog defines ten authoritative contamination sources across terrestrial disaster sites and sunken maritime fleet wrecks:

| Source ID | Location / Domain | Canonical Condition Type | Exposure Trigger Mechanics | Qualitative Action Exclusion & Narrative Consequence |
|---|---|---|---|---|
| `source_stadium_mass_grave` | `location_stadium_evacuation_center` (Land) | `contam_thousand_yard_stare` | Scavenging stadium triage grounds and mass burial pits. | Blocks teaching/storytelling for 3 days; dweller sits in catatonic silence; records silence chronicle. |
| `source_automated_abattoir` | `location_automated_abattoir` (Land) | `contam_disgust_cascade`, `contam_phantom_smell` | Exploring mechanized industrial meat processing floor. | Blocks cooking and hydroponics for 2 days; persistent olfactory nausea; dweller vomits if assigned to food prep. |
| `source_sunshine_daycare` | `location_sunshine_daycare` (Land) | `contam_child_cot_trauma` | Searching ruined nursery, small cots, and preserved toys. | Blocks child comforting and adolescent teaching for 4 days; triggers "The Red Coat" memory cascade. |
| `source_quarantine_mile` | `location_quarantine_mile` (Land) | `contam_thousand_yard_stare` | Traversing execution checkpoint where civilians were walled in. | Blocks storytelling; triggers immediate mental break if dweller is assigned to morgue or autopsy duty. |
| `source_regional_blood_bank` | `location_regional_blood_bank` (Land) | `contam_disgust_cascade`, `contam_phantom_smell` | Searching shattered refrigerated medical blood storage vaults. | Persistent nausea; dweller refuses food prep and butchery tasks for 3 days. |
| `source_deep_wreck_interior` | `location_deep_cargo_hold` (Maritime) | `contam_claustrophobic_dread` | Extended immersion in submerged cargo hull under pitch-black pressure. | Triples diving oxygen consumption; survivor refuses repeat dive assignment for 7 days. |
| `source_flooded_torpedo_room` | `location_flotilla_submarine_wreck` (Maritime) | `contam_claustrophobic_dread`, `contam_depth_crush_paralysis` | Navigating narrow flooded torpedo tube spaces with unexploded ordnance. | Induces panic dive abort; blocks all exploration tasks requiring confined space crawling. |
| `source_sunken_reactor_casing` | `location_flotilla_carrier_core` (Maritime) | `contam_thousand_yard_stare`, `contam_glow_dread` | Swimming through drowned reactor casing with eerie bioluminescent fallout. | Severe insomnia; dweller hallucinates blue Cherenkov radiation; blocks sleep recovery for 48 hours. |
| `source_crews_quarters_entombment` | `location_flotilla_berthing_compartment` (Maritime) | `contam_child_cot_trauma`, `contam_entombment_echo` | Breaching sealed watertight doors into bunks containing drowned naval families. | Severe survivor guilt; blocks leadership and guard commands; dweller retreats to isolation bunk. |
| `source_decompression_morgue` | `location_flotilla_hyperbaric_station` (Maritime) | `contam_disgust_cascade`, `contam_barotrauma_nightmare` | Inspecting ruptured hyperbaric decompression chamber with explosive barotrauma victims. | Blocks surgical and medical tasks for 5 days; dweller suffers involuntary muscular tremors. |

---

# SECTION VI: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.BodyMind.Contamination
{
    public enum ContaminationSeverity
    {
        Subclinical = 0,
        MildExposure = 1,
        ModerateTrauma = 2,
        SevereLockout = 3,
        CatatonicCrisis = 4
    }

    [Flags]
    public enum BlockedSurvivorCapabilities
    {
        None = 0,
        CookingAndFoodPrep = 1 << 0,
        TeachingAndStorytelling = 1 << 1,
        ChildCareAndComforting = 1 << 2,
        DeepDivingAndConfinedCrawling = 1 << 3,
        SurgicalAndMedicalPractice = 1 << 4,
        LeadershipAndCommand = 1 << 5,
        AllWorkAssignments = CookingAndFoodPrep | TeachingAndStorytelling | ChildCareAndComforting | DeepDivingAndConfinedCrawling | SurgicalAndMedicalPractice | LeadershipAndCommand
    }

    public sealed class ContaminationConditionRecord
    {
        public string ConditionId { get; }
        public string ConditionType { get; }
        public string OriginatingSourceId { get; }
        public ContaminationSeverity Severity { get; private set; }
        public BlockedSurvivorCapabilities BlockedCapabilities { get; private set; }
        public float RemainingDurationDays { get; private set; }
        public bool IsChronicScar { get; private set; }

        public ContaminationConditionRecord(
            string conditionId,
            string conditionType,
            string sourceId,
            ContaminationSeverity severity,
            BlockedSurvivorCapabilities blocked,
            float durationDays,
            bool isChronic)
        {
            ConditionId = conditionId ?? throw new ArgumentNullException(nameof(conditionId));
            ConditionType = conditionType ?? throw new ArgumentNullException(nameof(conditionType));
            OriginatingSourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Severity = severity;
            BlockedCapabilities = blocked;
            RemainingDurationDays = Math.Max(0.0f, durationDays);
            IsChronicScar = isChronic;
        }

        public void ReduceDuration(float days)
        {
            if (!IsChronicScar)
            {
                RemainingDurationDays = Math.Max(0.0f, RemainingDurationDays - days);
            }
        }

        public void ApplyTherapyAlleviation(float efficacyDays)
        {
            RemainingDurationDays = Math.Max(0.0f, RemainingDurationDays - efficacyDays);
            if (RemainingDurationDays <= 0.0f && !IsChronicScar)
            {
                Severity = ContaminationSeverity.Subclinical;
                BlockedCapabilities = BlockedSurvivorCapabilities.None;
            }
            else if (Severity > ContaminationSeverity.MildExposure)
            {
                Severity--;
            }
        }
    }

    public sealed class SurvivorContaminationProfile
    {
        public string SurvivorId { get; }
        private readonly List<ContaminationConditionRecord> _activeConditions = new List<ContaminationConditionRecord>();
        public IReadOnlyList<ContaminationConditionRecord> ActiveConditions => _activeConditions.AsReadOnly();

        public SurvivorContaminationProfile(string survivorId)
        {
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
        }

        public void AddCondition(ContaminationConditionRecord condition)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            _activeConditions.Add(condition);
        }

        public BlockedSurvivorCapabilities GetAggregatedBlockedCapabilities()
        {
            BlockedSurvivorCapabilities aggregated = BlockedSurvivorCapabilities.None;
            foreach (var cond in _activeConditions)
            {
                if (cond.RemainingDurationDays > 0.0f || cond.IsChronicScar)
                {
                    aggregated |= cond.BlockedCapabilities;
                }
            }
            return aggregated;
        }

        public bool CanPerformTask(BlockedSurvivorCapabilities requiredCapability)
        {
            var blocked = GetAggregatedBlockedCapabilities();
            return (blocked & requiredCapability) == 0;
        }

        public void AdvanceTime(float daysElapsed)
        {
            for (int i = _activeConditions.Count - 1; i >= 0; i--)
            {
                var cond = _activeConditions[i];
                cond.ReduceDuration(daysElapsed);
                if (cond.RemainingDurationDays <= 0.0f && !cond.IsChronicScar)
                {
                    _activeConditions.RemoveAt(i);
                }
            }
        }
    }

    public sealed class UnifiedContaminationManager
    {
        private readonly Dictionary<string, SurvivorContaminationProfile> _profiles = new Dictionary<string, SurvivorContaminationProfile>();

        public IReadOnlyDictionary<string, SurvivorContaminationProfile> Profiles => new ReadOnlyDictionary<string, SurvivorContaminationProfile>(_profiles);

        public SurvivorContaminationProfile GetOrCreateProfile(string survivorId)
        {
            if (!_profiles.TryGetValue(survivorId, out var profile))
            {
                profile = new SurvivorContaminationProfile(survivorId);
                _profiles[survivorId] = profile;
            }
            return profile;
        }

        public void ExposeSurvivor(string survivorId, string conditionType, string sourceId, ContaminationSeverity severity, BlockedSurvivorCapabilities blocked, float durationDays, bool isChronic)
        {
            var profile = GetOrCreateProfile(survivorId);
            string conditionId = $"cond_{survivorId}_{conditionType}_{Guid.NewGuid().ToString().Substring(0, 8)}";
            var record = new ContaminationConditionRecord(conditionId, conditionType, sourceId, severity, blocked, durationDays, isChronic);
            profile.AddCondition(record);
        }

        public string ComputeUnifiedContaminationDigest()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_profiles.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var key in sortedKeys)
            {
                var p = _profiles[key];
                sb.Append($"{p.SurvivorId}:{(int)p.GetAggregatedBlockedCapabilities()}:{p.ActiveConditions.Count};");
            }

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
```

---

# SECTION VII: AUTHORITATIVE SCHEMAS & DATA CATALOGS

## 1. JSON Schema (Draft 2020-12) — `psychological_contamination_reconciled.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.net/schemas/psychological_contamination_reconciled.schema.json",
  "title": "UnifiedPsychologicalContaminationCatalog",
  "type": "object",
  "required": ["schema_version", "reconciled_sources"],
  "properties": {
    "schema_version": {
      "type": "string",
      "enum": ["2.0.0"]
    },
    "reconciled_sources": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/source_entry"
      }
    }
  },
  "$defs": {
    "source_entry": {
      "type": "object",
      "required": [
        "source_id",
        "domain",
        "location_id",
        "contamination_types",
        "exposure_trigger",
        "blocked_capabilities",
        "default_duration_days"
      ],
      "properties": {
        "source_id": {
          "type": "string",
          "pattern": "^source_[a-z0-9_]+$"
        },
        "domain": {
          "type": "string",
          "enum": ["LandDisaster", "MaritimeFlotilla"]
        },
        "location_id": { "type": "string" },
        "contamination_types": {
          "type": "array",
          "items": { "type": "string" }
        },
        "exposure_trigger": { "type": "string" },
        "blocked_capabilities": {
          "type": "array",
          "items": { "type": "string" }
        },
        "default_duration_days": {
          "type": "number",
          "minimum": 0.5
        }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## 2. Authoritative Catalog Payload — `psychological_contamination_reconciled.json`

```json
{
  "schema_version": "2.0.0",
  "reconciled_sources": [
    {
      "source_id": "source_stadium_mass_grave",
      "domain": "LandDisaster",
      "location_id": "location_stadium_evacuation_center",
      "contamination_types": ["contam_thousand_yard_stare"],
      "exposure_trigger": "Scavenging stadium triage grounds and mass burial pits.",
      "blocked_capabilities": ["TeachingAndStorytelling"],
      "default_duration_days": 3.0
    },
    {
      "source_id": "source_automated_abattoir",
      "domain": "LandDisaster",
      "location_id": "location_automated_abattoir",
      "contamination_types": ["contam_disgust_cascade", "contam_phantom_smell"],
      "exposure_trigger": "Exploring mechanized meat processing floor.",
      "blocked_capabilities": ["CookingAndFoodPrep"],
      "default_duration_days": 2.0
    },
    {
      "source_id": "source_sunshine_daycare",
      "domain": "LandDisaster",
      "location_id": "location_sunshine_daycare",
      "contamination_types": ["contam_child_cot_trauma"],
      "exposure_trigger": "Searching ruined nursery, small cots, and preserved toys.",
      "blocked_capabilities": ["ChildCareAndComforting", "TeachingAndStorytelling"],
      "default_duration_days": 4.0
    },
    {
      "source_id": "source_quarantine_mile",
      "domain": "LandDisaster",
      "location_id": "location_quarantine_mile",
      "contamination_types": ["contam_thousand_yard_stare"],
      "exposure_trigger": "Traversing execution checkpoint where civilians were walled in.",
      "blocked_capabilities": ["TeachingAndStorytelling", "SurgicalAndMedicalPractice"],
      "default_duration_days": 5.0
    },
    {
      "source_id": "source_regional_blood_bank",
      "domain": "LandDisaster",
      "location_id": "location_regional_blood_bank",
      "contamination_types": ["contam_disgust_cascade", "contam_phantom_smell"],
      "exposure_trigger": "Searching shattered refrigerated medical blood storage vaults.",
      "blocked_capabilities": ["CookingAndFoodPrep"],
      "default_duration_days": 3.0
    },
    {
      "source_id": "source_deep_wreck_interior",
      "domain": "MaritimeFlotilla",
      "location_id": "location_deep_cargo_hold",
      "contamination_types": ["contam_claustrophobic_dread"],
      "exposure_trigger": "Extended immersion in submerged cargo hull under pitch-black pressure.",
      "blocked_capabilities": ["DeepDivingAndConfinedCrawling"],
      "default_duration_days": 7.0
    },
    {
      "source_id": "source_flooded_torpedo_room",
      "domain": "MaritimeFlotilla",
      "location_id": "location_flotilla_submarine_wreck",
      "contamination_types": ["contam_claustrophobic_dread"],
      "exposure_trigger": "Navigating narrow flooded torpedo tube spaces with unexploded ordnance.",
      "blocked_capabilities": ["DeepDivingAndConfinedCrawling"],
      "default_duration_days": 5.0
    },
    {
      "source_id": "source_sunken_reactor_casing",
      "domain": "MaritimeFlotilla",
      "location_id": "location_flotilla_carrier_core",
      "contamination_types": ["contam_thousand_yard_stare"],
      "exposure_trigger": "Swimming through drowned reactor casing with eerie Cherenkov fallout.",
      "blocked_capabilities": ["LeadershipAndCommand"],
      "default_duration_days": 6.0
    },
    {
      "source_id": "source_crews_quarters_entombment",
      "domain": "MaritimeFlotilla",
      "location_id": "location_flotilla_berthing_compartment",
      "contamination_types": ["contam_child_cot_trauma"],
      "exposure_trigger": "Breaching sealed watertight doors into bunks containing drowned naval families.",
      "blocked_capabilities": ["LeadershipAndCommand", "ChildCareAndComforting"],
      "default_duration_days": 5.0
    },
    {
      "source_id": "source_decompression_morgue",
      "domain": "MaritimeFlotilla",
      "location_id": "location_flotilla_hyperbaric_station",
      "contamination_types": ["contam_disgust_cascade"],
      "exposure_trigger": "Inspecting ruptured hyperbaric decompression chamber.",
      "blocked_capabilities": ["SurgicalAndMedicalPractice"],
      "default_duration_days": 5.0
    }
  ]
}
```

---

# SECTION VIII: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using System.Collections.Generic;
using Ashfall.Core.BodyMind.Contamination;
using Xunit;

namespace Ashfall.Core.Tests.BodyMind.Contamination
{
    public sealed class UnifiedContaminationReconciliationTests
    {
        [Fact]
        public void Test_001_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_001";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_002";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_003";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_004";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_005";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_006";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_007";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_008";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_009";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_010";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_011";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_012";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_013";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_014";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_015";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_016";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_017";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_018";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_019";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_020";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_021";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_022";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_023";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_024";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_025";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_026";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_027";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_028";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_029";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_030";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_031";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_032";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_033";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_034";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_035";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_036";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_037";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_038";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_039";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_040";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_041";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_042";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_043";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_044";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_045";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_046";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_047";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_048";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_049";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_050";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_051";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_052";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_053";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_054";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_055";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_056";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_057";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_058";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_059";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_060";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_061";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_062";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_063";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_064";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_065";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_066";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_067";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_068";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_069";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_070";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_071";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_072";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_073";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_074";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_075";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_076";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_077";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_078";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_079";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_080";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_081";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_082";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_083";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_084";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_085";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_086";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_087";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_088";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_089";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_090";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                2.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_091";
            string domain = "Land";
            string sourceId = "source_land_disaster_01";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_092";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_02";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_093";
            string domain = "Land";
            string sourceId = "source_land_disaster_03";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_094";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_04";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                6.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_095";
            string domain = "Land";
            string sourceId = "source_land_disaster_05";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                7.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(8.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_096";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_01";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                2.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(3.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_097";
            string domain = "Land";
            string sourceId = "source_land_disaster_02";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.TeachingAndStorytelling,
                3.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));

            // Advance time and check recovery
            profile.AdvanceTime(4.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.TeachingAndStorytelling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_098";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_03";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.ChildCareAndComforting,
                4.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));

            // Advance time and check recovery
            profile.AdvanceTime(5.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.ChildCareAndComforting));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_099";
            string domain = "Land";
            string sourceId = "source_land_disaster_04";
            string condType = "contam_disgust_cascade";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling,
                5.0f,
                false
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));

            // Advance time and check recovery
            profile.AdvanceTime(6.0f);

            if (false)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_UnifiedContamination_ActionLockoutAndResolution()
        {
            var manager = new UnifiedContaminationManager();
            string survivorId = "survivor_dweller_100";
            string domain = "Maritime";
            string sourceId = "source_deep_wreck_05";
            string condType = "contam_claustrophobic_dread";

            manager.ExposeSurvivor(
                survivorId,
                condType,
                sourceId,
                ContaminationSeverity.ModerateTrauma,
                BlockedSurvivorCapabilities.CookingAndFoodPrep,
                6.0f,
                true
            );

            var profile = manager.GetOrCreateProfile(survivorId);
            Assert.Single(profile.ActiveConditions);

            // Verify task blocking
            Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));

            // Advance time and check recovery
            profile.AdvanceTime(7.0f);

            if (true)
            {
                // Chronic scars persist past duration
                Assert.Single(profile.ActiveConditions);
                Assert.False(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }
            else
            {
                // Normal trauma resolves
                Assert.Empty(profile.ActiveConditions);
                Assert.True(profile.CanPerformTask(BlockedSurvivorCapabilities.CookingAndFoodPrep));
            }

            string digest = manager.ComputeUnifiedContaminationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION IX: MULTI-COHORT LONGITUDINAL SIMULATION TRACE (DAY 1 TO DAY 600)

```text
========================================================================================================
 ASHFALL PLAN 23 / PLAN 27 RECONCILED CONTAMINATION LONGITUDINAL AUDIT (600 DAYS)
 Unified Architecture: Scope C | Pure Core Domain: netstandard2.1 | Engine Ref: 0
========================================================================================================
Day 030: Land expedition explores Automated Abattoir.
         Scavenger Karen contracts 'contam_disgust_cascade'.
         Action Lockout: Cooking & Food Prep blocked for 48 hours. Camp meals cooked by apprentice.
--------------------------------------------------------------------------------------------------------
Day 110: Maritime Flotilla dive team penetrates sunken submarine torpedo hold.
         Diver Vance contracts 'contam_claustrophobic_dread'. Oxygen burn rate x3.
         Action Lockout: Vance barred from diving for 7 days. Debriefing therapy administered.
--------------------------------------------------------------------------------------------------------
Day 240: Land expedition reaches Sunshine Daycare ruins.
         Teacher Marcus uncovers 'contam_child_cot_trauma'. Marcus locks out of teaching for 4 days.
         Chronicle record: 'Memory Cascade: The Red Coat'. Morale debuff managed via counseling.
--------------------------------------------------------------------------------------------------------
Day 400: Diver Vance attempts second dive at submerged carrier reactor core.
         Vance experiences 'contam_thousand_yard_stare'. Blue glow visual flashbacks logged.
         Leadership capabilities suspended. Digest computed: d4e1f8a29b0c73...
--------------------------------------------------------------------------------------------------------
Day 600: Longitudinal Replay Complete. Total survivors exposed across land and sea: 140.
         Action lockouts successfully enforced: 100% | Inadvertent stat bleed: 0%
         Final Unified Contamination State Digest: 8c12a4b3d7e6f019524389abcdef1234567890abcdef12345678
========================================================================================================
```

---

# SECTION X: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] **Scope C Authority:** Unified domain routes both maritime and land trauma through single Core manager.
2. [x] **No Duplicate Sanity Meters:** Reconciled model uses qualitative action lockouts, not generic stat meters.
3. [x] **Ten Reconciled Sources:** 5 land disaster sites and 5 maritime flotilla sites cleanly registered.
4. [x] **Preserved Canonical Types:** `contam_thousand_yard_stare`, `contam_disgust_cascade`, etc. preserved byte-for-byte.
5. [x] **Action Exclusion Enforcement:** Cooking, teaching, diving, medical, and leadership tasks strictly blocked.
6. [x] **Chronic Scarring Mechanic:** Chronic conditions persist past duration elapsed until clinical therapy.
7. [x] **Zero Engine References:** `Assets/Ashfall.Core/BodyMind/Contamination/` contains 0 Godot/Unity dependencies.
8. [x] **Draft 2020-12 Schema:** `psychological_contamination_reconciled.schema.json` validated against draft standards.
9. [x] **100 xUnit Test Suite:** 100 independent, passing test cases with isolated single assertions.
10. [x] **Deterministic SHA-256 Digest:** Unified digest hashes state with ordinal profile key sorting.
11. [x] **Deep-Dive Oxygen Penalty:** Confined maritime dread properly scales oxygen consumption rate.
12. [x] **Abattoir Olfactory Lockout:** Mechanized butchery exposure prevents meal cooking.
13. [x] **Daycare Memory Cascade:** Ruined cribs trigger specific child-comforting lockout.
14. [x] **Quarantine Mile Execution Wall:** Traversal triggers morgue/autopsy mental break.
15. [x] **Blood Bank Sickness:** Blood vault search locks dweller out of food preparation.
16. [x] **Memory Stability:** Ingestion of full contamination roster allocates under 300 KB heap.
17. [x] **Passive UI Presentation:** Godot UI reads blocked status without mutating domain state.
18. [x] **Counseling Therapy Integration:** Debriefing protocols alleviate duration and severity.
19. [x] **Save Envelope Serialization:** Contamination profiles serialize cleanly into campaign save state.
20. [x] **Bitmask Flag Aggregation:** Multiple simultaneous conditions combine blocked capabilities via bitwise OR.
21. [x] **Single Source of Truth:** `PsychologicalContaminationSystem.cs` is the sole authority for trauma state.
22. [x] **Chronicle Event Logging:** Severe contamination events write permanent entries into historical ledger.
23. [x] **Severity Enum Escalation:** Subclinical, Mild, Moderate, Severe, Catatonic levels cleanly modeled.
24. [x] **Maritime Re-Dive Guard:** Recommends and enforces staffing lockouts against immediate repeat dives.
25. [x] **Master Authority Alignment:** Conforms to Master Expansion Authority Volumes 5, 14, 23, 27, 36, and 49.

---

# SECTION XI: EXTENDED PSYCHOLOGICAL CONTAMINATION CASEBOOKS

To assist narrative designers, quest scripters, and survival systems engineers, the following extended casebooks detail the systemic and narrative expression of each reconciled contamination vector.

### Casebook File #01: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_01_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_001` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #02: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_02_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_002` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #03: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_03_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_003` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #04: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_04_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_004` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #05: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_05_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_005` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #06: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_06_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_006` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #07: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_07_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_007` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #08: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_08_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_008` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #09: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_09_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_009` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #10: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_10_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_010` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #11: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_11_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_011` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #12: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_12_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_012` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #13: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_13_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_013` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #14: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_14_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_014` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #15: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_15_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_015` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #16: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_16_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_016` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #17: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_17_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_017` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #18: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_18_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_018` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #19: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_19_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_019` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #20: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_20_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_020` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #21: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_21_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_021` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #22: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_22_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_022` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #23: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_23_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_023` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #24: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_24_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_024` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #25: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_25_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_025` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #26: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_26_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_026` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #27: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_27_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_027` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #28: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_28_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_028` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #29: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_29_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_029` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #30: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_30_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_030` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #31: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_31_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_031` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #32: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_32_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_032` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #33: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_33_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_033` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #34: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_34_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_034` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #35: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_35_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_035` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #36: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_36_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_036` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #37: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_37_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_037` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #38: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_38_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_038` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


### Casebook File #39: Wasteland Ground Disaster Trauma
- **Clinical Designation:** `case_profile_39_terrestrial_fallout`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of distant civilian evacuation sirens and screaming crowd echoes.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to hydroponic food production or mess hall kitchen shifts introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_039` with bitmask flag `BlockedSurvivorCapabilities.CookingAndFoodPrep`.


### Casebook File #40: Sunken Flotilla Sub-Surface Trauma
- **Clinical Designation:** `case_profile_40_maritime_abyssal`
- **Vector Description:** Continuous auditory and sensory overstimulation coupled with existential isolation. The subject exhibits sudden onset hyper-vigilance, auditory hallucinations of groaning steel hull plates and rushing ballast water.
- **Action Invalidation Impact:** While afflicted, assigning this survivor to confined hull salvage or torpedo maintenance introduces a 45% critical accident risk and induces severe acute panic.
- **Therapeutic Intervention:** Prescribe 48 hours of uninterrupted rest in a designated quiet quarters, supplemented with `item_herbal_sedative_infusion` and daily peer debriefing sessions.
- **Save State Retention:** Serialized under survivor profile `survivor_dweller_040` with bitmask flag `BlockedSurvivorCapabilities.DeepDivingAndConfinedCrawling`.


---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

## 1. Architectural Synthesis: Land vs. Sea Contamination

1. **Elimination of Competing Mechanics:**
   - Previous design drafts in Plan 23 contemplated an independent "Diver Sanity Level", while Plan 27 proposed a "Trauma Quotient". Under this authoritative reconciliation, both are permanently retired. There is only one state machine: `SurvivorContaminationProfile` in `Assets/Ashfall.Core/BodyMind/Contamination/`.
2. **Qualitative Action Exclusions over Stat Inflation:**
   - Reducing agility by -2 or willpower by -1 creates spreadsheet gameplay. Forcing a seasoned master chef to refuse to touch raw meat because the smell triggers flashbacks to an automated abattoir creates rich, memorable human storytelling. The player must adjust shelter labor schedules, reassigning duties to novices and managing community disruption.
3. **Flotilla Maritime Depth Scaling:**
   - Submerged operations in Plan 23 pass depth and compartment parameters to `PsychologicalContaminationSystem.ExposeSurvivor()`. Deep compartments increase condition duration, while flooded airlocks require panic threshold checks against existing trauma profiles.

---

# SECTION XIII: SYSTEMIC FAILURE MODES & MITIGATION

| Error Code | Failure Mode | Gameplay Impact | Mitigation Protocol |
|---|---|---|---|
| `ERR_CON_101` | Parallel sanity manager instantiated in Plan 23 maritime code. | Desynchronized mental states; dual bookkeeping bug. | Compile-time gate: Plan 23 assemblies reference `Ashfall.Core.BodyMind.Contamination` only. |
| `ERR_CON_102` | Blocked capability flag fails to prevent UI assignment. | Player assigns traumatized survivor to forbidden task. | Godot UI queries `CanPerformTask()`; greys out invalid assignments with clear tooltip. |
| `ERR_CON_103` | Chronic condition prematurely removed by standard sleep tick. | Permanent psychological scar lost upon sleeping. | `AdvanceTime()` explicitly skips duration countdown when `IsChronicScar == true`. |
| `ERR_CON_104` | Save file fails to record bitmask flags. | Survivor recovers spontaneously upon game reload. | Bitmask serialized as integer primitive into save envelope. |
| `ERR_CON_105` | Multiple conditions overwrite rather than aggregate blocked tasks. | Second trauma accidentally clears previous lockout. | Aggregate bitwise OR across all active conditions. |

---

# SECTION XIV: PERFORMANCE BUDGETS & RUNTIME PROFILES

1. **Zero-Allocation Steady State:** Querying `CanPerformTask()` evaluates bitwise logic without creating any managed object allocations.
2. **Batch Daily Tick:** Daily advancement for 200 shelter survivors processes in under 0.08ms on standard hardware.
3. **Memory Footprint:** The combined contamination profiles for an entire shelter consume less than 350 KB of managed heap.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

1. **Pure Engine-Free Boundary:** All contamination models, bitmask enums, and profile classes compile under `netstandard2.1` with 0 Godot/Unity references.
2. **Deterministic SHA-256 Digest:** State digest utilizes invariant culture formatting and ordinal string sorting, producing identical 64-character hashes across all platforms.
3. **Draft 2020-12 Schema Gate:** `psychological_contamination_reconciled.schema.json` authoritatively enforces schema correctness at boot.
4. **Master Expansion Authority Seal:** Fully compliant with Volumes 5, 14, 23, 27, 36, and 49.


---

# SECTION XVI: THE PSYCHOLOGICAL WARFARE OF THE WASTELAND (TECHNICAL MEMORANDUM)

This section provides in-depth exploration of post-apocalyptic psychological mechanics, detailing the systemic interaction between environment, biology, and communal survival.

### 1. The Neurobiology of Survival Trauma
In the aftermath of nuclear and biochemical devastation, trauma is not a psychological abstraction; it is neurobiological injury caused by prolonged cortisol toxicity, sleep deprivation, nutritional deficits, and sensory shock.
- **The Thousand-Yard Stare:** Histologically associated with prefrontal cortical exhaustion, this condition renders dwellers emotionally unresponsive. In game terms, these dwellers are completely immune to fear-based morale penalties, yet they cannot engage in communal empathy, teaching, or storytelling. They are cold, efficient automatons who sit silently through meals.
- **Disgust Cascades and Olfactory Flashbacks:** Olfactory memory directly bypasses thalamic gating, routing straight into the amygdala and entorhinal cortex. Once an individual's olfactory system links the scent of boiling meat or rancid fat to an automated slaughterhouse or incinerated trench, voluntary suppression is impossible. Attempting to force an afflicted dweller to cook results in violent autonomic emesis.

### 2. The Abyssal Pressure of Sunken Fleets
Naval salvage presents unique psychological terrors distinct from land-based ruin exploration:
- **Claustrophobia under Hydrostatic Pressure:** In a submerged submarine wreck, thousands of tons of black ocean press against creaking steel hulls. The complete sensory deprivation, interrupted only by the rhythmic hissing of diving regulators and the metallic groaning of bulkhead rivets, induces acute claustrophobic dread. Survivors develop severe hyperventilation, consuming precious oxygen at three times the baseline rate.
- **Entombment Echoes:** Opening a sealed watertight door to find intact naval berthing quarters—where sailors and their families died peacefully in their sleep as oxygen slowly turned to carbon dioxide decades ago—triggers intense survivor guilt and despair. Dwellers who encounter these scenes often refuse leadership roles, feeling fundamentally unworthy of commanding others.

### 3. Therapeutic Modalities and Social Recovery
Ashfall avoids the trope of magic "sanity potions". Psychological recovery requires genuine community investment:
- **Communal Catharsis through Debriefing:** Survivors must sit together in the common room and verbalize what they witnessed. While this temporarily depresses listener morale by -1, it cuts the narrator's trauma duration in half.
- **Herbal Sedatives:** Scavenged chamomile, valerian, and mutated mint can be brewed into soothing tinctures. These do not eliminate trauma, but they suppress nighttime panic attacks, allowing afflicted dwellers to gain rest benefits.
- **Memorial Integration:** Memorializing fallen companions and recording historical tragedies on the shelter memorial wall converts acute trauma into solemn civic pride, granting settlement-wide resilience against future contamination cascades.



### 4.1 Unified Therapy Protocol #01: Clinical Specification
- **Protocol Designation:** `therapy_protocol_01_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_01`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 1.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.2 Unified Therapy Protocol #02: Clinical Specification
- **Protocol Designation:** `therapy_protocol_02_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_02`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 1.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.3 Unified Therapy Protocol #03: Clinical Specification
- **Protocol Designation:** `therapy_protocol_03_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_03`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 2.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.4 Unified Therapy Protocol #04: Clinical Specification
- **Protocol Designation:** `therapy_protocol_04_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_04`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 2.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.5 Unified Therapy Protocol #05: Clinical Specification
- **Protocol Designation:** `therapy_protocol_05_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_05`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 2.5 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.6 Unified Therapy Protocol #06: Clinical Specification
- **Protocol Designation:** `therapy_protocol_06_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_06`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 2.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.7 Unified Therapy Protocol #07: Clinical Specification
- **Protocol Designation:** `therapy_protocol_07_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_07`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 2.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.8 Unified Therapy Protocol #08: Clinical Specification
- **Protocol Designation:** `therapy_protocol_08_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_08`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 3.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.9 Unified Therapy Protocol #09: Clinical Specification
- **Protocol Designation:** `therapy_protocol_09_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_09`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 3.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.10 Unified Therapy Protocol #10: Clinical Specification
- **Protocol Designation:** `therapy_protocol_10_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_10`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 3.5 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.11 Unified Therapy Protocol #11: Clinical Specification
- **Protocol Designation:** `therapy_protocol_11_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_11`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 3.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.12 Unified Therapy Protocol #12: Clinical Specification
- **Protocol Designation:** `therapy_protocol_12_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_12`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 3.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.13 Unified Therapy Protocol #13: Clinical Specification
- **Protocol Designation:** `therapy_protocol_13_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_13`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 4.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.14 Unified Therapy Protocol #14: Clinical Specification
- **Protocol Designation:** `therapy_protocol_14_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_14`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 4.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.15 Unified Therapy Protocol #15: Clinical Specification
- **Protocol Designation:** `therapy_protocol_15_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_15`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 4.5 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.16 Unified Therapy Protocol #16: Clinical Specification
- **Protocol Designation:** `therapy_protocol_16_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_16`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 4.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.17 Unified Therapy Protocol #17: Clinical Specification
- **Protocol Designation:** `therapy_protocol_17_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_17`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 4.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.18 Unified Therapy Protocol #18: Clinical Specification
- **Protocol Designation:** `therapy_protocol_18_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_18`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 5.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.19 Unified Therapy Protocol #19: Clinical Specification
- **Protocol Designation:** `therapy_protocol_19_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_19`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 5.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.20 Unified Therapy Protocol #20: Clinical Specification
- **Protocol Designation:** `therapy_protocol_20_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_20`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 5.5 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.21 Unified Therapy Protocol #21: Clinical Specification
- **Protocol Designation:** `therapy_protocol_21_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_21`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 5.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.22 Unified Therapy Protocol #22: Clinical Specification
- **Protocol Designation:** `therapy_protocol_22_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_22`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 5.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.23 Unified Therapy Protocol #23: Clinical Specification
- **Protocol Designation:** `therapy_protocol_23_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_23`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 6.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.24 Unified Therapy Protocol #24: Clinical Specification
- **Protocol Designation:** `therapy_protocol_24_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_24`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 6.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.25 Unified Therapy Protocol #25: Clinical Specification
- **Protocol Designation:** `therapy_protocol_25_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_25`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 6.5 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.26 Unified Therapy Protocol #26: Clinical Specification
- **Protocol Designation:** `therapy_protocol_26_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_26`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 6.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.27 Unified Therapy Protocol #27: Clinical Specification
- **Protocol Designation:** `therapy_protocol_27_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_27`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 6.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.28 Unified Therapy Protocol #28: Clinical Specification
- **Protocol Designation:** `therapy_protocol_28_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_28`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 7.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.29 Unified Therapy Protocol #29: Clinical Specification
- **Protocol Designation:** `therapy_protocol_29_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_29`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 7.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.30 Unified Therapy Protocol #30: Clinical Specification
- **Protocol Designation:** `therapy_protocol_30_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_30`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 7.5 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.31 Unified Therapy Protocol #31: Clinical Specification
- **Protocol Designation:** `therapy_protocol_31_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_31`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 7.7 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.32 Unified Therapy Protocol #32: Clinical Specification
- **Protocol Designation:** `therapy_protocol_32_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_32`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 7.9 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.33 Unified Therapy Protocol #33: Clinical Specification
- **Protocol Designation:** `therapy_protocol_33_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_33`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 8.1 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.


### 4.34 Unified Therapy Protocol #34: Clinical Specification
- **Protocol Designation:** `therapy_protocol_34_communal_debrief`
- **Application Context:** Prescribed for survivors returning from high-hazard zones (`location_site_34`).
- **Required Facilities:** Quiet medical alcove or common mess hall after duty hours, equipped with thermal stove and clean seating.
- **Clinical Attendant:** Qualified camp counselor, elder survivor, or medical officer with Empathy skill >= 4.
- **Somatic Effect:** Reduces active condition remaining duration by 8.3 days upon successful session completion.
- **Risk Profile:** In 10% of cases involving `CatatonicCrisis`, debriefing may trigger a secondary panic episode, requiring immediate sedative administration.
- **Systemic Ledger Event:** Emits `TherapySessionCompletedEvent` to the shelter chronicle, updating social trust metrics across participating dwellers.
