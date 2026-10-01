// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host adapter for the Core WorldIncidentSystem. It owns catalog
    /// loading and persistence only; consequences are applied by the engine
    /// through the consequence port bound by Main.
    /// </summary>
    public sealed class WorldIncidentHostSession : HostSessionBase
    {
        public WorldIncidentSystem Engine { get; }
        public WorldIncidentDefinition? PendingIncident => Engine.PendingIncident;
        public string LastEvent { get; private set; } = string.Empty;

        public WorldIncidentHostSession(WorldIncidentSystem engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Engine.OnIncidentSurfaced += incident =>
            {
                LastEvent = $"World incident surfaced: {incident.Title}.";
                RaiseStateChanged();
            };
            Engine.OnIncidentResolved += result =>
            {
                LastEvent = $"World incident resolved: {result.EventId} / {result.ChoiceId}.";
                RaiseStateChanged();
            };
            Engine.StateChanged += () => RaiseStateChanged();
        }

        public static WorldIncidentHostSession Create(string dataDir)
        {
            var engine = new WorldIncidentSystem();
            var session = new WorldIncidentHostSession(engine);
            if (!string.IsNullOrWhiteSpace(dataDir))
            {
                var load = WorldIncidentCatalogLoader.LoadDetailed(
                    dataDir,
                    new FileSystemIO(),
                    new SystemTextJsonSerializer());
                if (load.IsSuccess)
                    engine.RegisterRange(load.Incidents);
                else if (load.Errors.Count > 0)
                    session.LastEvent = "World incident catalog disabled: " + load.Errors[0];
            }

            var saved = WorldIncidentSaveStore.TryLoad();
            if (saved != null)
            {
                engine.RestoreState(saved);
                session.LastEvent = "World incident history restored from save.";
            }
            return session;
        }

        public WorldIncidentDefinition? SelectForDay(int day, ISeededRng rng)
            => Engine.SelectForDay(day, rng);

        public WorldIncidentResolutionResult Resolve(string eventId, string choiceId, int day)
            => Engine.Resolve(eventId, choiceId, day);

        public WorldIncidentState CaptureSave() => Engine.CaptureState();

        public void RestoreSave(WorldIncidentState state) => Engine.RestoreState(state);
    }
}
