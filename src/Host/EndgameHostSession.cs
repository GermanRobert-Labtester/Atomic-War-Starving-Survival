// SPDX-License-Identifier: MIT
// ASHFALL campaign endgame & epilogue host session (Plan 84 / Task B25).

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side session manager for campaign endgame closure, evaluation, and sealing.
    /// </summary>
    public sealed class EndgameHostSession : HostSessionBase
    {
        private readonly EndgameSystem _system;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileIO _fileIO;
        private readonly string _dataDir;

        public EndgameSystem System => _system;
        public EndgamePhase Phase => _system.Phase;
        public bool IsSealed => _system.IsSealed;
        public CampaignEpilogueReport? EpilogueReport => _system.State.epilogueReport;

        public EndgameHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir, ISeededRng? rng = null, ILog? log = null)
        {
            _jsonSerializer = jsonSerializer;
            _fileIO = fileIO;
            _dataDir = dataDir;
            _system = new EndgameSystem(rng, log);

            _system.OnEndingTriggered += (_, _) => RaiseStateChanged();
            _system.OnCampaignSealed += _ => RaiseStateChanged();

            LoadCatalog();
        }

        public static EndgameHostSession Create(string dataDir, ISeededRng? rng = null, ILog? log = null)
        {
            var serializer = new SystemTextJsonSerializer();
            var fileIO = new FileSystemIO();
            return new EndgameHostSession(serializer, fileIO, dataDir, rng, log);
        }

        private void LoadCatalog()
        {
            string path = Path.Combine(_dataDir, "endings.json");
            if (_fileIO.FileExists(path))
            {
                string json = _fileIO.ReadAllText(path);
                _system.LoadCatalog(json, _jsonSerializer);
            }
        }

        public bool TriggerEnding(CampaignEvaluationContext ctx)
        {
            return _system.TriggerEnding(ctx);
        }

        public bool SealCampaign(int day)
        {
            return _system.SealCampaign(day);
        }

        public EndgameSaveState CaptureState() => _system.CaptureState();

        public string TryCapturePersisted() => EndgameSaveStore.TryCapturePersisted(_system.CaptureState());

        public void RestoreState(EndgameSaveState state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }
    }
}
