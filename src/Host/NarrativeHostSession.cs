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

        public string LastEvent { get; private set; } = string.Empty;
        public NarrativeHostSession(NarrativeEncounterSystem engine = null!)
        {
            Engine = engine ?? new NarrativeEncounterSystem();
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
        }

        public static NarrativeHostSession Create(string dataDir)
        {
            var session = new NarrativeHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                session.Engine.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, serializer));
            }
            var save = NarrativeSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Narrative history restored from save.";
            }
            return session;
        }

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
                   $"(morale {Engine.State.cumulativeMorale:+0;-0;0}, guilt {Engine.State.cumulativeGuilt:+0;-0;0}).";
        }

        // ── Save / Load ──────────────────────────────────────────────

        public NarrativeEncounterState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(NarrativeEncounterState state) => Engine.RestoreState(state);
    }
}
