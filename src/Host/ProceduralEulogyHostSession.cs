// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ProceduralEulogySaveStore
// Core State : Ashfall.Core.Journal.EulogySaveState
// Host Caller: Main.ProceduralEulogy
// Purpose    : Procedural eulogy host session & persistence. The Core engine
//              remains the sole composition authority over a dweller's life
//              summary; the host supplies the roster life record and archives
//              the spoken eulogy through the existing journal owner.
// ============================================================================

using System;
using System.Linq;
using Ashfall.Core.Journal;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ProceduralEulogySaveStore
    {
        public const string FileName = "procedural_eulogy_save.json";
        public const string SectionName = "procedural_eulogy";

        private static readonly SaveStore<EulogySaveState> s_store =
            SaveStoreHub.Checksummed<EulogySaveState>(FileName, nameof(ProceduralEulogySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(EulogySaveState state) => s_store.CaptureBare(state);
        public static EulogySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(EulogySaveState state) => s_store.TrySave(state);
        public static EulogySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="ProceduralEulogyEngine"/>.</summary>
    public sealed class ProceduralEulogyHostSession : HostSessionBase
    {
        private readonly ProceduralEulogyEngine _engine;

        public ProceduralEulogyEngine Engine => _engine;
        public string LastEvent { get; private set; } = string.Empty;

        public ProceduralEulogyHostSession(EulogySaveState? state = null)
        {
            _engine = new ProceduralEulogyEngine();
            if (state != null) _engine.RestoreState(state);
        }

        public static ProceduralEulogyHostSession Create(EulogySaveState? state = null) =>
            new ProceduralEulogyHostSession(state);

        public string ComposeEulogy(DwellerLifeRecord life)
        {
            string text = _engine.ComposeEulogy(life);
            LastEvent = $"Composed eulogy for {life?.dwellerName ?? "an unnamed dweller"}.";
            RaiseStateChanged();
            return text;
        }

        public int ArchivedCount => _engine.CaptureState().archivedEulogyTexts.Count;

        public string? GetMostRecentEulogy()
        {
            var texts = _engine.CaptureState().archivedEulogyTexts;
            return texts.Count > 0 ? texts[texts.Count - 1] : null;
        }

        public EulogySaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(EulogySaveState state)
        {
            _engine.RestoreState(state);
            LastEvent = "Restored eulogy archive.";
            RaiseStateChanged();
        }

        public bool TrySave() => ProceduralEulogySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ProceduralEulogySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
