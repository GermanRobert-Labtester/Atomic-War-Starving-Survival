// SPDX-License-Identifier: MIT
// ASHFALL campaign endgame & epilogue host session (Plan 84 / Task B25).

using System;
using System.Collections.Generic;
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
        public int ChapterIndex => _system.ChapterIndex;
        public IReadOnlyList<ChapterRecord> Chapters => _system.Chapters;
        public bool HasPlayedOn => _system.HasPlayedOn;
        public bool CanPlayOn => _system.CanPlayOn;
        public CampaignEpilogueReport? EpilogueReport => _system.State.epilogueReport;
        public event Action<CampaignEpilogueReport>? CampaignSealed;
        public event Action<int>? ChapterContinued;

        public EndgameHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir, ISeededRng? rng = null, ILog? log = null)
        {
            _jsonSerializer = jsonSerializer;
            _fileIO = fileIO;
            _dataDir = dataDir;
            _system = new EndgameSystem(rng, log);

            _system.OnEndingTriggered += (_, _) => RaiseStateChanged();
            _system.OnChapterContinued += ch =>
            {
                RaiseStateChanged();
                ChapterContinued?.Invoke(ch);
            };
            _system.OnCampaignSealed += report =>
            {
                RaiseStateChanged();
                CampaignSealed?.Invoke(report);
            };

            LoadCatalog();
        }

        public static EndgameHostSession Create(string dataDir, ISeededRng? rng = null, ILog? log = null)
        {
            var serializer = new SystemTextJsonSerializer();
            var fileIO = new FileSystemIO();
            return new EndgameHostSession(serializer, fileIO, dataDir, rng, log);
        }

        private ChapterProfileCatalog? _chapterProfiles;
        public ChapterProfileCatalog? ChapterProfiles => _chapterProfiles;

        private void LoadCatalog()
        {
            string path = Path.Combine(_dataDir, "endings.json");
            if (_fileIO.FileExists(path))
            {
                string json = _fileIO.ReadAllText(path);
                _system.LoadCatalog(json, _jsonSerializer);
            }

            string profilesPath = Path.Combine(_dataDir, "chapter_profiles.json");
            if (_fileIO.FileExists(profilesPath))
            {
                string profilesJson = _fileIO.ReadAllText(profilesPath);
                _chapterProfiles = ChapterProfileCatalog.LoadFromJson(profilesJson);
            }
        }

        public ChapterProfileDef GetCurrentProfile()
        {
            if (_chapterProfiles != null && _chapterProfiles.TryGetProfile(_system.ProfileId, out var def) && def != null)
                return def;
            return ChapterProfileCatalog.CreateDefaultLegacyProfile();
        }

        public int GetTargetReadingDay()
        {
            if (_system.ChapterIndex >= 2) return 720;
            return GetCurrentProfile().reading_day > 0 ? GetCurrentProfile().reading_day : 360;
        }

        public bool TriggerEnding(CampaignEvaluationContext ctx)
        {
            return _system.TriggerEnding(ctx);
        }

        public bool TriggerEnding(CampaignEvaluationContext ctx, ChapterProfileDef? profile, string? branchEndingId = null)
        {
            return _system.TriggerEnding(ctx, profile, branchEndingId);
        }

        public bool ContinueChapter()
        {
            return _system.ContinueChapter();
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
