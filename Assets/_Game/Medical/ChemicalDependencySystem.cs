using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Chemical Dependency Expansion System — extends the base AddictionSystem
    /// with substance-specific dependency tracking for alcohol, pain meds,
    /// stimulants, and sedatives. Supports managed detox programs and
    /// cold-turkey withdrawal with different severity profiles per substance.
    ///
    /// Owns: Survivor.ChemicalDependencies.
    /// Works alongside: AddictionSystem (which handles the Addicted trait and base withdrawal).
    /// </summary>
    public class ChemicalDependencySystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float DependencyThreshold = 0.3f;
        // level at which dependency forms
        public const float DependencyIncreasePerDose = 0.15f;
        public const float DependencyDecayPerDayClean = 0.05f;
        public const float MaxDependencyLevel = 1f;

        // ── Withdrawal constants ───────────────────────────────────────
        public const float ColdTurkeyWithdrawalDurationHours = 72f;
        public const float ManagedDetoxDurationHours = 120f;
        public const float ColdTurkeyTremorCraftingPenalty = 0.40f;
        public const float ColdTurkeyTremorCombatPenalty = 0.30f;
        public const float ColdTurkeyMoraleDrainPerHour = 3f;
        public const float ManagedDetoxMoraleDrainPerHour = 1f;
        public const float DetoxSuccessThresholdHours = 96f;

        // ── Substance-specific severities ──────────────────────────────
        public static readonly Dictionary<ChemicalDependencyKind, float> KindBaseSeverity =
            new Dictionary<ChemicalDependencyKind, float>
            {
                { ChemicalDependencyKind.Opioid, 0.9f },
                { ChemicalDependencyKind.Alcohol, 0.7f },
                { ChemicalDependencyKind.Stimulant, 0.6f },
                { ChemicalDependencyKind.Sedative, 0.5f }
            };

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, string> OnDependencyFormed;
        // sv, itemId
        public event Action<Survivor, string> OnWithdrawalStarted;
        // sv, itemId
        public event Action<Survivor, string> OnDetoxCompleted;
        public event Action<Survivor, string> OnDetoxFailed;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<Survivor, float> ApplyCraftingPenalty;
        public Action<Survivor, float> ApplyCombatPenalty;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Func<float> GetDay;
        public System.Random Rng;

        /// <summary>
        /// Register consumption of a potentially addictive substance.
        /// </summary>
        public void OnSubstanceConsumed(Survivor sv, string itemId,
            ChemicalDependencyKind kind)
        {
            if (sv == null || !sv.IsAlive) return;

            // Find or create dependency entry
            ChemicalDependency? existing = null;
            int existingIdx = -1;
            for (int i = 0; i < sv.ChemicalDependencies.Count; i++)
            {
                if (string.Equals(sv.ChemicalDependencies[i].ItemId, itemId,
                    System.StringComparison.Ordinal))
                {
                    existing = sv.ChemicalDependencies[i];
                    existingIdx = i;
                    break;
                }
            }

            if (existingIdx >= 0)
            {
                var dep = sv.ChemicalDependencies[existingIdx];
                dep.DependencyLevel = Math.Min(MaxDependencyLevel,
                    dep.DependencyLevel + DependencyIncreasePerDose);
                sv.ChemicalDependencies[existingIdx] = dep;

                if (dep.DependencyLevel >= DependencyThreshold && !dep.InManagedDetox)
                {
                    OnDependencyFormed?.Invoke(sv, itemId);
                }
            }
            else
            {
                var dep = new ChemicalDependency(itemId,
                    DependencyIncreasePerDose, kind);
                sv.ChemicalDependencies.Add(dep);
            }
        }

        /// <summary>
        /// Begin a managed detox for a specific substance.
        /// </summary>
        public bool BeginManagedDetox(Survivor sv, string itemId)
        {
            if (sv == null) return false;
            for (int i = 0; i < sv.ChemicalDependencies.Count; i++)
            {
                if (string.Equals(sv.ChemicalDependencies[i].ItemId, itemId,
                    System.StringComparison.Ordinal))
                {
                    var dep = sv.ChemicalDependencies[i];
                    if (dep.DependencyLevel < DependencyThreshold) return false;
                    dep.InManagedDetox = true;
                    dep.DetoxProgressHours = 0f;
                    sv.ChemicalDependencies[i] = dep;
                    OnWithdrawalStarted?.Invoke(sv, itemId);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Begin cold-turkey withdrawal (stop substance immediately).
        /// </summary>
        public bool BeginColdTurkey(Survivor sv, string itemId)
        {
            if (sv == null) return false;
            for (int i = 0; i < sv.ChemicalDependencies.Count; i++)
            {
                if (string.Equals(sv.ChemicalDependencies[i].ItemId, itemId,
                    System.StringComparison.Ordinal))
                {
                    var dep = sv.ChemicalDependencies[i];
                    if (dep.DependencyLevel < DependencyThreshold) return false;
                    dep.InManagedDetox = false;
                    dep.DetoxProgressHours = -1f; // flag: cold turkey active
                    sv.ChemicalDependencies[i] = dep;
                    OnWithdrawalStarted?.Invoke(sv, itemId);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tick — progress detox programs and apply withdrawal effects.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive) return;

            for (int i = sv.ChemicalDependencies.Count - 1; i >= 0; i--)
            {
                var dep = sv.ChemicalDependencies[i];

                if (dep.DetoxProgressHours < 0f)
                {
                    // Cold turkey active
                    float moraleDrain = ColdTurkeyMoraleDrainPerHour * gameHours *
                        KindBaseSeverity.GetValueOrDefault(dep.Kind, 0.5f);
                    ApplyMoraleDelta?.Invoke(sv, -moraleDrain);
                    ApplyCraftingPenalty?.Invoke(sv, ColdTurkeyTremorCraftingPenalty);
                    ApplyCombatPenalty?.Invoke(sv, ColdTurkeyTremorCombatPenalty);

                    // Cold turkey lasts 72h
                    dep.DetoxProgressHours += gameHours;
                    if (dep.DetoxProgressHours >= ColdTurkeyWithdrawalDurationHours)
                    {
                        CompleteDetox(sv, i, ref dep);
                    }
                    sv.ChemicalDependencies[i] = dep;
                }
                else if (dep.InManagedDetox)
                {
                    // Managed detox
                    float moraleDrain = ManagedDetoxMoraleDrainPerHour * gameHours *
                        KindBaseSeverity.GetValueOrDefault(dep.Kind, 0.5f);
                    ApplyMoraleDelta?.Invoke(sv, -moraleDrain);

                    dep.DetoxProgressHours += gameHours;
                    if (dep.DetoxProgressHours >= DetoxSuccessThresholdHours)
                    {
                        CompleteDetox(sv, i, ref dep);
                    }
                    sv.ChemicalDependencies[i] = dep;
                }
                else if (dep.DependencyLevel > 0f)
                {
                    // Natural decay when clean
                    dep.DependencyLevel = Math.Max(0f,
                        dep.DependencyLevel -
                        DependencyDecayPerDayClean * (gameHours / 24f));
                    if (dep.DependencyLevel <= 0f)
                    {
                        sv.ChemicalDependencies.RemoveAt(i);
                        OnDetoxCompleted?.Invoke(sv, dep.ItemId);
                    }
                    else
                    {
                        sv.ChemicalDependencies[i] = dep;
                    }
                }
            }
        }

        private void CompleteDetox(Survivor sv, int index, ref ChemicalDependency dep)
        {
            dep.DependencyLevel = Math.Max(0f, dep.DependencyLevel - 0.5f);
            dep.DetoxProgressHours = 0f;
            dep.InManagedDetox = false;
            ApplyCraftingPenalty?.Invoke(sv, 0f); // clear penalties
            ApplyCombatPenalty?.Invoke(sv, 0f);

            if (dep.DependencyLevel < DependencyThreshold)
            {
                sv.ChemicalDependencies.RemoveAt(index);
                OnDetoxCompleted?.Invoke(sv, dep.ItemId);
            }
            else
            {
                OnDetoxFailed?.Invoke(sv, dep.ItemId);
            }
        }

        /// <summary>
        /// Check if a survivor has active withdrawal from any substance.
        /// </summary>
        public bool HasActiveWithdrawal(Survivor sv)
        {
            if (sv == null) return false;
            for (int i = 0; i < sv.ChemicalDependencies.Count; i++)
            {
                if (sv.ChemicalDependencies[i].DetoxProgressHours != 0f ||
                    sv.ChemicalDependencies[i].InManagedDetox)
                    return true;
            }
            return false;
        }
    }
}
