// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : IdeologicalFrictionSaveStore
// Core State : Ashfall.Core.Survivors.IdeologicalFrictionEventSaveState
// Host Caller: Main.IdeologicalFriction (SetupIdeologicalFriction / SaveIdeologicalFriction)
// Purpose    : Plan 148 — Ideological friction events and quests: confrontations,
//              conversions, bunker factions, and mediation choices.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class IdeologicalFrictionSaveStore
    {
        public const string FileName = "ideological_friction_save.json";
        public const string SectionName = "ideological_friction";

        private static readonly SaveStore<IdeologicalFrictionEventSaveState> s_store =
            SaveStoreHub.Checksummed<IdeologicalFrictionEventSaveState>(FileName, nameof(IdeologicalFrictionSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(IdeologicalFrictionEventSaveState state) => s_store.CaptureBare(state);
        public static IdeologicalFrictionEventSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(IdeologicalFrictionEventSaveState state) => s_store.TrySave(state);
        public static IdeologicalFrictionEventSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 148 (Ideological Friction → Events & Quests).
    /// Orchestrates interpersonal worldview clashes between shelter dwellers,
    /// tracking roommate sleep quality penalties, belief conversion attempts,
    /// faction schisms, and player mediation choices.
    /// </summary>
    public sealed class IdeologicalFrictionHostSession : HostSessionBase
    {
        private readonly IdeologicalFrictionEvents _events;
        private readonly IdeologicalFrictionSystem _friction;
        private string _lastEvent = string.Empty;

        public IdeologicalFrictionEvents Events => _events;
        public IdeologicalFrictionSystem Friction => _friction;
        public string LastEvent => _lastEvent;

        public IdeologicalFrictionCensus Census => _events.GetCensus();
        public IReadOnlyList<IdeologicalEventInstance> RecentEvents => _events.RecentEvents;
        public IReadOnlyList<IdeologicalFactionState> ActiveFactions => _events.ActiveFactions;
        public IReadOnlyList<IdeologicalEventTemplate> Templates => _events.Templates;

        public IdeologicalFrictionHostSession(
            string? dataDir = null,
            IdeologicalFrictionEvents? events = null,
            IdeologicalFrictionSystem? friction = null)
        {
            _events = events ?? new IdeologicalFrictionEvents();
            _friction = friction ?? new IdeologicalFrictionSystem();

            _events.OnIdeologicalEventTriggeredSeam = evt =>
            {
                _lastEvent = $"Ideological event triggered: {evt.title} ({evt.actorId} vs {evt.targetId})";
                RaiseStateChanged();
            };

            _events.OnBeliefConversionSucceededSeam = (targetId, oldBelief, newBelief) =>
            {
                _friction.RegisterBelief(targetId, newBelief);
                _lastEvent = $"Belief conversion: {targetId} converted to {newBelief}";
                RaiseStateChanged();
            };

            _events.OnIdeologicalMediationResolvedSeam = (evt, choice) =>
            {
                _lastEvent = $"Mediation resolved for {evt.title} with choice: {choice}";
                RaiseStateChanged();
            };

            _events.OnBunkerSplitEscalatedSeam = (beliefId, count) =>
            {
                _lastEvent = $"Bunker faction escalated: {beliefId} with {count} adherents";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir);
            }
        }

        public static IdeologicalFrictionHostSession Create(
            string dataDir,
            IdeologicalFrictionEvents? events = null,
            IdeologicalFrictionSystem? friction = null)
        {
            return new IdeologicalFrictionHostSession(dataDir, events, friction);
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, "ideological_events.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _events.LoadCatalog(json);
                _lastEvent = $"Loaded {_events.Templates.Count} ideological event templates.";
                RaiseStateChanged();
            }
        }

        public void RegisterBelief(string survivorId, string beliefProfileId)
        {
            _friction.RegisterBelief(survivorId, beliefProfileId);
        }

        public string GetBelief(string survivorId) => _friction.GetBelief(survivorId);

        public float GetAffinity(string survivorA, string survivorB) => _friction.GetAffinity(survivorA, survivorB);

        public float GetRoommateCompatibilityMultiplier(string survivorA, string survivorB) =>
            _friction.GetRoommateCompatibilityMultiplier(survivorA, survivorB);

        public void TickRoommates(string survivorA, string survivorB, float gameHours)
        {
            _friction.TickRoommates(survivorA, survivorB, gameHours);
        }

        public IdeologicalEventInstance? CheckDailyFriction(
            string survivorA,
            string beliefA,
            string survivorB,
            string beliefB,
            float pairAffinity,
            int currentDay,
            ISeededRng? rng = null,
            bool forceTrigger = false)
        {
            return _events.CheckDailyFriction(
                survivorA,
                beliefA,
                survivorB,
                beliefB,
                pairAffinity,
                currentDay,
                rng,
                forceTrigger);
        }

        public bool ResolveConfrontation(
            string instanceId,
            IdeologicalMediationChoice choice,
            out float affinityDeltaA,
            out float affinityDeltaB,
            out float moraleDelta)
        {
            return _events.ResolveConfrontation(instanceId, choice, out affinityDeltaA, out affinityDeltaB, out moraleDelta);
        }

        public bool AttemptConversion(
            string actorId,
            string targetId,
            string actorBelief,
            float successProbability,
            ISeededRng rng,
            out string resultMessage)
        {
            return _events.AttemptConversion(actorId, targetId, actorBelief, successProbability, rng, out resultMessage);
        }

        public List<IdeologicalFactionState> UpdateBunkerFactions(Dictionary<string, string> survivorBeliefs)
        {
            return _events.UpdateBunkerFactions(survivorBeliefs);
        }

        public IdeologicalFrictionEventSaveState CaptureState() => _events.CaptureState();

        public void RestoreState(IdeologicalFrictionEventSaveState state)
        {
            _events.RestoreState(state);
            _lastEvent = "Restored ideological friction state.";
            RaiseStateChanged();
        }
    }
}
