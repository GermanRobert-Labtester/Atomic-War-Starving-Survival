using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
#pragma warning disable CS8618

namespace Ashfall.Core.Medical
{
    [Serializable]
    public class NarcoticRecipeInput
    {
        public string item_id = string.Empty;
        public int amount = 1;
    }

    [Serializable]
    public class NarcoticDefinition
    {
        public string chem_id = string.Empty;
        public string name = string.Empty;
        public string category = "Stimulant";
        public List<string> effect_tags = new List<string>();
        public float duration_hours = 6.0f;
        public string onset_class = "Immediate";
        public float tolerance_gain = 0.08f;
        public float dependency_pressure = 18.0f;
        public string withdrawal_profile_id = string.Empty;
        public float toxicity_contribution = 15.0f;
        public List<string> contraindication_tags = new List<string>();
        public int trade_value = 50;
        public int research_tier = 1;
        public List<NarcoticRecipeInput> recipe_inputs = new List<NarcoticRecipeInput>();
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public class NarcoticsCatalog
    {
        public int schema_version = 1;
        public List<NarcoticDefinition> narcotics = new List<NarcoticDefinition>();
    }

    [Serializable]
    public class ActiveChemEffect
    {
        public string chemId = string.Empty;
        public float remainingHours = 0f;
        public float potency = 1f;
    }

    [Serializable]
    public class DependencyRecord
    {
        public string chemId = string.Empty;
        public float tolerance = 0f; // 0..1
        public float dependencyLevel = 0f; // 0..100
        public float hoursSinceLastDose = 0f;
        public bool isWithdrawing = false;
    }

    [Serializable]
    public class SurvivorNarcoticsProfile
    {
        public string survivorId = string.Empty;
        public float bloodToxicity = 0f; // 0..100
        public List<ActiveChemEffect> activeEffects = new List<ActiveChemEffect>();
        public List<DependencyRecord> dependencies = new List<DependencyRecord>();
        public bool inRehabBed = false;
        public float rehabProgressDays = 0f;
    }

    [Serializable]
    public class NarcoticsState
    {
        public string systemId = "narcotics_system";
        public List<SurvivorNarcoticsProfile> survivors = new List<SurvivorNarcoticsProfile>();
        public int totalDosesAdministered = 0;
        public int totalOverdoses = 0;
        public int totalThefts = 0;
        public int totalRehabCompletions = 0;
    }

    public class NarcoticsSystem
    {
        public const string SystemId = "narcotics_system";

        private readonly Dictionary<string, NarcoticDefinition> _definitions = new Dictionary<string, NarcoticDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, SurvivorNarcoticsProfile> _profiles = new Dictionary<string, SurvivorNarcoticsProfile>(StringComparer.Ordinal);

        private int _totalDoses = 0;
        private int _totalOverdoses = 0;
        private int _totalThefts = 0;
        private int _totalRehabs = 0;

        public event Action<string, string>? OnChemBrewed;
        public event Action<string, string>? OnChemAdministered;
        public event Action<string, string>? OnOverdoseEmergency;
        public event Action<string, string>? OnWithdrawalTriggered;
        public event Action<string, string>? OnTheftCommitted;
        public event Action<string>? OnRehabCompleted;

        public IReadOnlyCollection<SurvivorNarcoticsProfile> Profiles => _profiles.Values;
        public int TotalDoses => _totalDoses;
        public int TotalOverdoses => _totalOverdoses;
        public int TotalThefts => _totalThefts;
        public int TotalRehabs => _totalRehabs;

        public void LoadCatalog(string jsonText, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(jsonText) || serializer == null) return;
            try
            {
                var catalog = serializer.Deserialize<NarcoticsCatalog>(jsonText);
                if (catalog?.narcotics != null)
                {
                    _definitions.Clear();
                    foreach (var n in catalog.narcotics)
                    {
                        if (!string.IsNullOrEmpty(n.chem_id))
                            _definitions[n.chem_id] = n;
                    }
                }
            }
            catch
            {
                // Graceful fallback
            }
        }

        public NarcoticDefinition? GetDefinition(string chemId)
        {
            return _definitions.TryGetValue(chemId, out var def) ? def : null;
        }

        public SurvivorNarcoticsProfile GetOrCreateProfile(string survivorId)
        {
            if (!_profiles.TryGetValue(survivorId, out var p))
            {
                p = new SurvivorNarcoticsProfile
                {
                    survivorId = survivorId,
                    bloodToxicity = 0f
                };
                _profiles[survivorId] = p;
            }
            return p;
        }

        public bool CanBrewChem(string chemId, Func<string, int> inventoryCountQuery, out string failureReason)
        {
            if (!_definitions.TryGetValue(chemId, out var def))
            {
                failureReason = "Unknown chemical formula";
                return false;
            }

            foreach (var req in def.recipe_inputs)
            {
                if (inventoryCountQuery(req.item_id) < req.amount)
                {
                    failureReason = $"Insufficient reagent: requires {req.amount}x {req.item_id}";
                    return false;
                }
            }

            failureReason = string.Empty;
            return true;
        }

        public bool BrewChem(string chemId, Func<string, int> countQuery, Action<string, int> removeInventory, Action<string, int> addInventory, out string error)
        {
            if (!CanBrewChem(chemId, countQuery, out error)) return false;

            var def = _definitions[chemId];
            foreach (var req in def.recipe_inputs)
            {
                removeInventory(req.item_id, req.amount);
            }

            // Produced dose item
            string outputItemId = $"item_{chemId}";
            addInventory(outputItemId, 1);

            OnChemBrewed?.Invoke(chemId, outputItemId);
            return true;
        }

        public bool AdministerChem(string survivorId, string chemId, ISeededRng rng, out string outcomeMessage)
        {
            if (!_definitions.TryGetValue(chemId, out var def))
            {
                outcomeMessage = "Chem formula not found";
                return false;
            }

            var profile = GetOrCreateProfile(survivorId);

            // Find or create dependency
            var dep = profile.dependencies.Find(d => d.chemId == chemId);
            if (dep == null)
            {
                dep = new DependencyRecord { chemId = chemId };
                profile.dependencies.Add(dep);
            }

            // Calculate effective potency based on tolerance
            float effectivePotency = Math.Max(0.2f, 1f - (dep.tolerance * 0.7f));
            profile.activeEffects.Add(new ActiveChemEffect
            {
                chemId = chemId,
                remainingHours = def.duration_hours,
                potency = effectivePotency
            });

            // Tolerance and dependency progression
            dep.tolerance = Math.Clamp(dep.tolerance + def.tolerance_gain, 0f, 1f);
            dep.dependencyLevel = Math.Clamp(dep.dependencyLevel + def.dependency_pressure, 0f, 100f);
            dep.hoursSinceLastDose = 0f;
            dep.isWithdrawing = false;

            // Systemic Toxicity
            profile.bloodToxicity = Math.Clamp(profile.bloodToxicity + def.toxicity_contribution, 0f, 100f);
            _totalDoses++;
            OnChemAdministered?.Invoke(survivorId, chemId);

            // Overdose check
            if (profile.bloodToxicity > 60f)
            {
                float overdoseRisk = (profile.bloodToxicity - 60f) * 0.025f;
                if (rng.NextDouble() < overdoseRisk)
                {
                    _totalOverdoses++;
                    OnOverdoseEmergency?.Invoke(survivorId, $"Acute toxic crisis from {def.name}! Medical resuscitation required.");
                    outcomeMessage = "Dose administered but triggered an acute toxic overdose crisis!";
                    return true;
                }
            }

            outcomeMessage = $"Successfully administered {def.name}. Potency: {effectivePotency * 100:F0}%.";
            return true;
        }

        public void AdvanceMedicalTick(float deltaHours, ISeededRng rng)
        {
            foreach (var profile in _profiles.Values)
            {
                // Toxicity metabolic clearance
                profile.bloodToxicity = Math.Max(0f, profile.bloodToxicity - (deltaHours * 2.0f));

                // Active effect decay
                for (int i = profile.activeEffects.Count - 1; i >= 0; i--)
                {
                    var eff = profile.activeEffects[i];
                    eff.remainingHours -= deltaHours;
                    if (eff.remainingHours <= 0f)
                    {
                        profile.activeEffects.RemoveAt(i);
                    }
                }

                // Dependency and withdrawal progression
                foreach (var dep in profile.dependencies)
                {
                    dep.hoursSinceLastDose += deltaHours;

                    // Withdrawal triggers if dependent and dose has lapsed > 24 hours
                    if (dep.dependencyLevel > 30f && dep.hoursSinceLastDose > 24f && !dep.isWithdrawing)
                    {
                        dep.isWithdrawing = true;
                        OnWithdrawalTriggered?.Invoke(profile.survivorId, $"Severe withdrawal symptoms from chemical dependency ({dep.chemId}).");
                    }

                    // Craving theft risk if severely dependent and not in rehab
                    if (dep.dependencyLevel > 50f && dep.isWithdrawing && !profile.inRehabBed)
                    {
                        if (rng.NextDouble() < 0.08 * (deltaHours / 24f))
                        {
                            _totalThefts++;
                            OnTheftCommitted?.Invoke(profile.survivorId, $"Addicted survivor pilfered medicine stores in desperate craving for {dep.chemId}.");
                        }
                    }
                }

                // Rehab bed processing
                if (profile.inRehabBed)
                {
                    profile.rehabProgressDays += (deltaHours / 24f);

                    foreach (var dep in profile.dependencies)
                    {
                        dep.dependencyLevel = Math.Max(0f, dep.dependencyLevel - (deltaHours * 1.5f));
                        dep.tolerance = Math.Max(0f, dep.tolerance - (deltaHours * 0.02f));
                    }

                    if (profile.rehabProgressDays >= 14f)
                    {
                        profile.inRehabBed = false;
                        profile.rehabProgressDays = 0f;
                        foreach (var dep in profile.dependencies)
                        {
                            dep.isWithdrawing = false;
                            dep.dependencyLevel = 0f;
                        }
                        _totalRehabs++;
                        OnRehabCompleted?.Invoke(profile.survivorId);
                    }
                }
            }
        }

        public bool AssignToRehabBed(string survivorId)
        {
            var profile = GetOrCreateProfile(survivorId);
            if (profile.inRehabBed) return false;
            profile.inRehabBed = true;
            profile.rehabProgressDays = 0f;
            return true;
        }

        public bool DischargeFromRehab(string survivorId)
        {
            var profile = GetOrCreateProfile(survivorId);
            if (!profile.inRehabBed) return false;
            profile.inRehabBed = false;
            return true;
        }

        public NarcoticsState CaptureState()
        {
            var state = new NarcoticsState
            {
                systemId = SystemId,
                totalDosesAdministered = _totalDoses,
                totalOverdoses = _totalOverdoses,
                totalThefts = _totalThefts,
                totalRehabCompletions = _totalRehabs
            };
            foreach (var kv in _profiles) state.survivors.Add(kv.Value);
            return state;
        }

        public void RestoreState(NarcoticsState? state)
        {
            _profiles.Clear();
            _totalDoses = 0;
            _totalOverdoses = 0;
            _totalThefts = 0;
            _totalRehabs = 0;

            if (state == null) return;

            _totalDoses = state.totalDosesAdministered;
            _totalOverdoses = state.totalOverdoses;
            _totalThefts = state.totalThefts;
            _totalRehabs = state.totalRehabCompletions;

            if (state.survivors != null)
            {
                foreach (var p in state.survivors)
                {
                    if (!string.IsNullOrEmpty(p.survivorId))
                        _profiles[p.survivorId] = p;
                }
            }
        }
    }
}
