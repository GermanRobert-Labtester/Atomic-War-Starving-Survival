// SPDX-License-Identifier: MIT
// Unit tests for Year Two Package P2: Play On chapter continuation & dual verb mechanics.

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class YearTwoPlayOnTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            return "Assets/StreamingAssets/Data";
        }

        private EndgameSystem CreateSystem()
        {
            var sys = new EndgameSystem(new SeededRng(84), NullLog.Instance);
            string dataDir = FindDataDir();
            string path = Path.Combine(dataDir, "endings.json");
            if (File.Exists(path))
            {
                sys.LoadCatalog(File.ReadAllText(path), new SystemTextJsonSerializer());
            }
            return sys;
        }

        [Fact]
        public void InitialState_DefaultsToChapterOne_ActiveAndUnsealed()
        {
            var sys = CreateSystem();

            Assert.Equal(1, sys.ChapterIndex);
            Assert.Equal(EndgamePhase.Active, sys.Phase);
            Assert.False(sys.IsSealed);
            Assert.False(sys.HasPlayedOn);
            Assert.Empty(sys.Chapters);
            Assert.False(sys.CanPlayOn);
        }

        [Fact]
        public void TriggerEnding_Day360_LivingSurvivors_EnablesCanPlayOn()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 12,
                DeceasedSurvivors = 2,
                AverageMorale = 65f
            };

            bool triggered = sys.TriggerEnding(ctx);

            Assert.True(triggered);
            Assert.Equal(EndgamePhase.Epilogue, sys.Phase);
            Assert.False(sys.IsSealed);
            Assert.False(sys.IsTerminalEnding());
            Assert.True(sys.CanPlayOn);
        }

        [Fact]
        public void TriggerEnding_TerminalEnding_Extinction_ForbidsPlayOn()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 150,
                LivingSurvivors = 0,
                DeceasedSurvivors = 10,
                ForceExtinction = true
            };

            bool triggered = sys.TriggerEnding(ctx);

            Assert.True(triggered);
            Assert.Equal(EndgamePhase.Epilogue, sys.Phase);
            Assert.True(sys.IsTerminalEnding());
            Assert.False(sys.CanPlayOn);
        }

        [Fact]
        public void TriggerEnding_TerminalEnding_FrozenSilence_ForbidsPlayOn()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 200,
                LivingSurvivors = 8,
                ForceWinterFailure = true
            };

            bool triggered = sys.TriggerEnding(ctx);

            Assert.True(triggered);
            Assert.Equal(EndgamePhase.Epilogue, sys.Phase);
            Assert.True(sys.IsTerminalEnding());
            Assert.False(sys.CanPlayOn);
        }

        [Fact]
        public void ContinueChapter_TransitionsToChapterTwo_RecordsPriorChapter_ClearsEpilogue()
        {
            var sys = CreateSystem();
            int continuedChapter = 0;
            sys.OnChapterContinued += ch => continuedChapter = ch;

            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 10,
                AverageMorale = 70f
            };
            sys.TriggerEnding(ctx);

            bool continued = sys.ContinueChapter();

            Assert.True(continued);
            Assert.Equal(2, continuedChapter);
            Assert.Equal(2, sys.ChapterIndex);
            Assert.True(sys.HasPlayedOn);
            Assert.Equal(EndgamePhase.Active, sys.Phase);
            Assert.False(sys.IsSealed);
            Assert.Empty(sys.State.selectedEndingId);
            Assert.Null(sys.State.epilogueReport);

            Assert.Single(sys.Chapters);
            var ch1 = sys.Chapters[0];
            Assert.Equal(1, ch1.chapterIndex);
            Assert.Equal("Year One: The Long Ash", ch1.chapterTitle);
            Assert.Equal(360, ch1.readingDay);
            Assert.NotNull(ch1.epilogueReport);
            Assert.Equal(360, ch1.epilogueReport.daysSurvived);
            // ChapterRecord carries the profile and the sealed/reading day.
            Assert.Equal(sys.ProfileId, ch1.profileId);
            Assert.Equal(360, ch1.sealedDay);
        }

        [Fact]
        public void ContinueChapter_InChapterTwo_ForbidsFurtherPlayOn()
        {
            var sys = CreateSystem();
            var ctx1 = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 10
            };
            sys.TriggerEnding(ctx1);
            Assert.True(sys.ContinueChapter());
            Assert.Equal(2, sys.ChapterIndex);

            // Trigger Chapter 2 ending
            var ctx2 = new CampaignEvaluationContext
            {
                CurrentDay = 720,
                LivingSurvivors = 8
            };
            sys.TriggerEnding(ctx2);

            Assert.Equal(EndgamePhase.Epilogue, sys.Phase);
            Assert.False(sys.CanPlayOn);
            Assert.False(sys.ContinueChapter());
        }

        [Fact]
        public void SealCampaign_AtChapterOne_SealsWithoutPlayingOn()
        {
            var sys = CreateSystem();
            CampaignEpilogueReport? sealedReport = null;
            sys.OnCampaignSealed += r => sealedReport = r;

            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 15,
                AverageMorale = 80f
            };
            sys.TriggerEnding(ctx);

            bool sealedSuccess = sys.SealCampaign(360);

            Assert.True(sealedSuccess);
            Assert.Equal(EndgamePhase.Sealed, sys.Phase);
            Assert.True(sys.IsSealed);
            Assert.False(sys.CanPlayOn);
            Assert.NotNull(sealedReport);
            Assert.Equal(360, sealedReport.sealedDay);

            // Re-seal is idempotent / rejected
            Assert.False(sys.SealCampaign(360));
        }

        [Fact]
        public void SaveState_RoundTrip_PreservesChapterIndexAndRecords()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 11,
                AverageMorale = 68f
            };
            sys.TriggerEnding(ctx);
            sys.ContinueChapter();

            var captured = sys.CaptureState();
            Assert.Equal(2, captured.chapterIndex);
            Assert.True(captured.hasPlayedOn);
            Assert.Single(captured.chapters);

            var newSys = CreateSystem();
            newSys.RestoreState(captured);

            Assert.Equal(2, newSys.ChapterIndex);
            Assert.True(newSys.HasPlayedOn);
            Assert.Equal(EndgamePhase.Active, newSys.Phase);
            Assert.Single(newSys.Chapters);
            Assert.Equal(1, newSys.Chapters[0].chapterIndex);
            Assert.Equal(360, newSys.Chapters[0].readingDay);
            Assert.Equal("Year One: The Long Ash", newSys.Chapters[0].chapterTitle);
            // The profile + sealed day survive the save round-trip.
            Assert.Equal(sys.ProfileId, newSys.Chapters[0].profileId);
            Assert.Equal(360, newSys.Chapters[0].sealedDay);
        }

        [Fact]
        public void SaveState_LegacySchema1_UpgradesCleanlyToChapterOne()
        {
            var legacy = new EndgameSaveState
            {
                schema_version = 1,
                systemId = "endgame",
                phase = EndgamePhase.Active,
                selectedEndingId = "",
                isSealed = false,
                profileId = "profile_base_v1"
            };

            var sys = CreateSystem();
            sys.RestoreState(legacy);

            Assert.Equal(1, sys.ChapterIndex);
            Assert.False(sys.HasPlayedOn);
            Assert.Empty(sys.Chapters);
            Assert.Equal(EndgamePhase.Active, sys.Phase);
        }
    }
}
