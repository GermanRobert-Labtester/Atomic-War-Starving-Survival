// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : CompanionAnimalHostSession
// Core Source  : Ashfall.Core.Ecology.CompanionAnimalSystem (Plan 151 / 174)
// Purpose      : Thin Godot adapter for companion animals, working beasts,
//                roles (guard/pack/morale), feeding, and veterinary care.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public sealed class CompanionCensus
    {
        public int AuthoredSpeciesCount { get; init; }
        public int TotalCompanionsCount { get; init; }
        public int AliveCompanionsCount { get; init; }
        public int GuardCount { get; init; }
        public int PackCount { get; init; }
        public int MoraleCount { get; init; }
        public int SickCount { get; init; }
    }

    public sealed class CompanionAnimalHostSession : HostSessionBase
    {
        public CompanionAnimalSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public CompanionCensus Census => new CompanionCensus
        {
            AuthoredSpeciesCount = System.Profiles.Count,
            TotalCompanionsCount = System.State.companions.Count,
            AliveCompanionsCount = System.State.companions.Count(c => c != null && c.alive),
            GuardCount = System.State.companions.Count(c => c != null && c.alive && (CompanionRole)c.role == CompanionRole.Guard),
            PackCount = System.State.companions.Count(c => c != null && c.alive && (CompanionRole)c.role == CompanionRole.Pack),
            MoraleCount = System.State.companions.Count(c => c != null && c.alive && (CompanionRole)c.role == CompanionRole.Morale),
            SickCount = System.State.companions.Count(c => c != null && c.alive && c.sickness != (int)CompanionSicknessState.Healthy)
        };

        public CompanionAnimalHostSession(CompanionAnimalSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnCompanionRegistered += state =>
            {
                LastEvent = $"Companion registered: {state.name} ({state.species_id}).";
                RaiseStateChanged();
            };

            System.OnRoleChanged += (state, role) =>
            {
                LastEvent = $"Role updated: {state.name} assigned to {role}.";
                RaiseStateChanged();
            };

            System.OnSicknessChanged += (state, sickness) =>
            {
                LastEvent = $"Sickness change: {state.name} diagnosed with {sickness}.";
                RaiseStateChanged();
            };

            System.OnCompanionRecovered += state =>
            {
                LastEvent = $"Recovery: {state.name} has recovered.";
                RaiseStateChanged();
            };

            System.OnHungerChanged += (state, hunger) =>
            {
                if (hunger >= CompanionAnimalSystem.HungerCritical)
                {
                    LastEvent = $"Warning: {state.name} is starving (hunger {hunger}).";
                    RaiseStateChanged();
                }
            };

            System.OnCompanionDied += state =>
            {
                LastEvent = $"Tragedy: Companion {state.name} perished.";
                RaiseStateChanged();
            };
        }

        public CompanionAssignResult RegisterCompanion(string companionId, string speciesId, int tamedDay, string? name = null)
        {
            var res = System.RegisterCompanion(companionId, speciesId, tamedDay, name);
            if (res.Success) RaiseStateChanged();
            return res;
        }

        public CompanionAssignResult AssignRole(string companionId, string survivorId, CompanionRole role, Func<string, bool>? survivorAlive = null)
        {
            var res = System.Assign(companionId, survivorId, role, survivorAlive);
            if (res.Success) RaiseStateChanged();
            return res;
        }

        public CompanionFeedResult FeedCompanion(string companionId, int day, IReadOnlyDictionary<string, string>? preferredTagMap = null)
        {
            var c = System.Companion(companionId);
            if (c == null) return new CompanionFeedResult { ReasonCode = "unknown_companion" };
            var profile = System.Profile(c.species_id);
            if (profile == null) return new CompanionFeedResult { ReasonCode = "unknown_species" };

            var res = System.Feed(c, profile, day, preferredTagMap);
            if (res.Fed) RaiseStateChanged();
            return res;
        }

        public CompanionAssignResult TreatSickness(string companionId, string itemId)
        {
            var res = System.TreatSickness(companionId, itemId);
            if (res.Success) RaiseStateChanged();
            return res;
        }

        public float GetGuardModifierTotal() => System.GetGuardModifierTotal();

        public float GetPackCapacityBonusForSurvivor(string survivorId) => System.GetPackCapacityBonusForSurvivor(survivorId);

        public int GetMoraleSupportBp(string companionId) => System.GetMoraleSupportBp(companionId);

        public int GetGriefMoraleDeltaBp(string companionId) => System.GetGriefMoraleDeltaBp(companionId);

        public CompanionSystemState CaptureSave() => System.CaptureState();

        public void RestoreSave(CompanionSystemState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Companion animal records restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            CompanionSaveStore.TrySave(CaptureSave());
        }

        public static CompanionAnimalHostSession Create(
            string dataDir,
            ISeededRng rng,
            Func<string, int> foodCount,
            Action<string, int> foodConsume,
            Func<string, bool>? knownSpeciesCheck = null)
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loadResult = CompanionAnimalCatalogLoader.Load(dataDir, fileIO, json);
            var profiles = CompanionAnimalCatalogLoader.ToProfiles(loadResult);

            var system = new CompanionAnimalSystem(profiles);
            system.BindFoodPort(foodCount, foodConsume);
            system.KnownSpeciesCheck = knownSpeciesCheck ?? (_ => true);
            system.SicknessRoll = () => rng.NextDouble();

            return new CompanionAnimalHostSession(system);
        }
    }
}
