using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the narrative encounter port. Loads the
    /// JSON catalog, offers weighted selection + resolution for the demo, and
    /// persists the resolution history. No rules here — hosts only wire.
    /// </summary>
    public sealed class NarrativeHostSession
    : HostSessionBase{
        public const int DemoSeed = 4242;

        public NarrativeEncounterSystem Engine { get; }
        public NarrativeArcEventSystem Arc { get; }
        public NarrativeArcEventDefinition? PendingArcEvent => Arc.PendingEvent;

        public string LastEvent { get; private set; } = string.Empty;
        public NarrativeHostSession(
            NarrativeEncounterSystem engine = null!,
            NarrativeArcEventSystem? arc = null)
        {
            Engine = engine ?? new NarrativeEncounterSystem();
            Arc = arc ?? new NarrativeArcEventSystem();
            Engine.OnEncounterSelected += def =>
            {
                LastEvent = $"Encounter: {def.title}";
                RaiseStateChanged();
            };
            Engine.OnEncounterResolved += r =>
            {
                LastEvent = $"Resolved {r.encounterId} / {r.choiceId} " +
                            $"(morale {r.moraleDelta:+0;-0;0}, guilt {r.guiltDelta:+0;-0;0}).";
                RaiseStateChanged();
            };
            Engine.OnStateChanged += _ => RaiseStateChanged();
            Arc.OnEventSelected += def =>
            {
                LastEvent = $"Narrative arc offered: {def.Title}.";
                RaiseStateChanged();
            };
            Arc.OnChoiceCommitted += result =>
            {
                LastEvent = result.ChoiceId.Length == 0
                    ? $"Narrative arc acknowledged: {result.EventId}."
                    : $"Narrative choice committed: {result.EventId} / {result.ChoiceId} " +
                      $"(morale {result.MoraleDelta:+0;-0;0}).";
                RaiseStateChanged();
            };
            Arc.OnStateChanged += _ => RaiseStateChanged();
        }

        public static NarrativeHostSession Create(string dataDir)
        {
            var session = new NarrativeHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                session.Engine.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, serializer));

                var arcLoad = NarrativeArcEventCatalogLoader.LoadDetailed(dataDir, fileIO, serializer);
                if (arcLoad.IsSuccess)
                {
                    session.Arc.RegisterRange(arcLoad.Events);
                }
                else if (arcLoad.Errors.Count > 0)
                {
                    session.LastEvent = "Narrative arc catalog disabled: " + arcLoad.Errors[0];
                }
            }
            var save = NarrativeSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.Arc.RestoreState(save.arcState);
                session.LastEvent = "Narrative history restored from save.";
            }
            return session;
        }

        /// <summary>Bind live survivor and cross-system authorities.</summary>
        public void ConfigureArcRuntime(
            Func<string, bool> survivorIsPresent,
            INarrativeArcConsequencePort consequences)
        {
            Arc.SurvivorIsPresent = survivorIsPresent ?? (_ => false);
            Arc.Consequences = consequences ?? NullNarrativeArcConsequencePort.Instance;
        }

        public NarrativeArcEventDefinition? SelectArcForDay(int day, ISeededRng rng)
            => Arc.SelectForDay(day, rng);

        public NarrativeArcChoiceResult CanApplyArcChoice(string eventId, string choiceId, int day)
            => Arc.CanApplyChoice(eventId, choiceId, day);

        public NarrativeArcChoiceResult ResolveArcChoice(string eventId, string choiceId, int day)
            => Arc.CommitChoice(eventId, choiceId, day);

        public NarrativeArcChoiceResult AcknowledgeArcEvent(string eventId, int day)
            => Arc.AcknowledgeEvent(eventId, day);

        // ── Demo actions ─────────────────────────────────────────────

        public string SelectDemo(string stance, float danger, string locationId)
        {
            var picked = Engine.SelectEncounter(stance, danger, locationId, new SeededRng(DemoSeed));
            return picked != null
                ? $"Encounter offered: {picked.title} ({picked.id}) — {picked.choices.Count} choices."
                : "Nothing eligible on this leg.";
        }

        public string ResolveDemo(string encounterId, string choiceId, int day)
        {
            bool ok = Engine.Resolve(encounterId, choiceId, string.Empty, day);
            return ok ? "Resolved." : "Unknown encounter or choice.";
        }

        public string StatusLine()
        {
            return $"Narrative encounters: {Engine.Catalog.Count} in catalog, " +
                   $"{Engine.TotalResolved} resolved " +
                   $"(morale {Engine.State.cumulativeMorale:+0;-0;0}, guilt {Engine.State.cumulativeGuilt:+0;-0;0}); " +
                   $"arc events {Arc.Catalog.Count}, {Arc.State.completedEventIds.Count} completed.";
        }

        // ── Save / Load ──────────────────────────────────────────────

        public NarrativeEncounterState CaptureSave()
        {
            var save = Engine.CaptureState();
            save.arcState = Arc.HasPersistedState ? Arc.CaptureState() : null;
            return save;
        }

        public void RestoreSave(NarrativeEncounterState state)
        {
            Engine.RestoreState(state);
            Arc.RestoreState(state?.arcState);
        }
    }
}
