using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion V — Character Story Expansions. Survivors are not just stat blocks.
    /// Their pasts are tied to locations and factions. Completing their personal
    /// quests alters the WorldStateConsequenceSystem.
    ///
    /// Three intersecting arcs:
    /// 1. The Reporter — The Redaction (truth about who fired first)
    /// 2. The Plumber — The Arteries (toxic water valve dilemma)
    /// 3. The Defector — The Ashen Mirror (Cult lover confrontation)
    /// </summary>
    public class CharacterStorySystem
    {
        // ── Character ids ─────────────────────────────────────────────
        public const string Char_Reporter = "the_reporter";
        public const string Char_Plumber = "the_plumber";
        public const string Char_Defector = "the_defector";

        // ── Story ids ─────────────────────────────────────────────────
        public const string Story_Redaction = "story_the_redaction";
        public const string Story_Arteries = "story_the_arteries";
        public const string Story_AshenMirror = "story_the_ashen_mirror";

        // ── Choice ids ────────────────────────────────────────────────
        public const string Choice_BroadcastTruth = "choice_broadcast_truth";
        public const string Choice_BurnDrive = "choice_burn_drive";
        public const string Choice_ForceValve = "choice_force_valve";
        public const string Choice_LeaveValveOpen = "choice_leave_valve_open";
        public const string Choice_AssassinatePriest = "choice_assassinate_priest";
        public const string Choice_JoinCult = "choice_join_cult";

        // ── Perk ids ──────────────────────────────────────────────────
        public const string Perk_TruthTeller = "perk_truth_teller";
        public const string Perk_IronStomach = "perk_iron_stomach";
        public const string Perk_ColdBlooded = "perk_cold_blooded";

        // ── Affliction ids ────────────────────────────────────────────
        public const string Affliction_SurvivorsGuilt = "affliction_survivors_guilt";
        public const string Affliction_PTSD = "affliction_ptsd";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, string> OnStoryStageChanged;   // storyId, stage
        public event Action<string, string> OnChoiceMade;          // storyId, choiceId
        public event Action<string, string> OnPerkGranted;         // characterId, perkId
        public event Action<string, string> OnAfflictionGained;    // characterId, afflictionId
        public event Action<string> OnCharacterDeparted;           // characterId

        private readonly Dictionary<string, StoryState> _stories = new Dictionary<string, StoryState>();

        public CharacterStorySystem()
        {
            _stories[Story_Redaction] = new StoryState { Id = Story_Redaction, CharacterId = Char_Reporter };
            _stories[Story_Arteries] = new StoryState { Id = Story_Arteries, CharacterId = Char_Plumber };
            _stories[Story_AshenMirror] = new StoryState { Id = Story_AshenMirror, CharacterId = Char_Defector };
        }

        // ── Story: The Redaction (Reporter) ───────────────────────────

        /// <summary>Stage 1: Reporter recognizes the frequency from Blacksite Echo.</summary>
        public void TriggerRedaction(int currentDay)
        {
            var story = _stories[Story_Redaction];
            if (story.Stage != StoryProgressStage.NotStarted) return;
            story.Stage = StoryProgressStage.Active;
            OnStoryStageChanged?.Invoke(Story_Redaction, "hook");
        }

        /// <summary>Stage 2: Drive retrieved. Research bench decrypts it.</summary>
        public void RedactionDriveDecrypted()
        {
            var story = _stories[Story_Redaction];
            if (story.Stage != StoryProgressStage.Active) return;
            story.Stage = StoryProgressStage.Climax;
            OnStoryStageChanged?.Invoke(Story_Redaction, "climax");
        }

        /// <summary>Final choice: broadcast the truth or burn the drive.</summary>
        public bool MakeRedactionChoice(string choiceId)
        {
            var story = _stories[Story_Redaction];
            if (story.Stage != StoryProgressStage.Climax) return false;

            story.Choice = choiceId;
            story.Stage = StoryProgressStage.Completed;

            if (choiceId == Choice_BroadcastTruth)
            {
                // Garrison hegemony drops to 0, Militia maxes out
                story.WorldEffect = "Garrison Hegemony → 0. Militia Hegemony → 100.";
                OnPerkGranted?.Invoke(Char_Reporter, Perk_TruthTeller);
                // Garrison sends BlackOps to assassinate the Reporter
            }
            else if (choiceId == Choice_BurnDrive)
            {
                story.WorldEffect = "Garrison remains in power. The war's lie continues.";
                OnAfflictionGained?.Invoke(Char_Reporter, Affliction_SurvivorsGuilt);
            }

            OnChoiceMade?.Invoke(Story_Redaction, choiceId);
            return true;
        }

        // ── Story: The Arteries (Plumber) ─────────────────────────────

        /// <summary>Stage 1: Water purifier failing. Toxic cross-contamination.</summary>
        public void TriggerArteries(int currentDay)
        {
            var story = _stories[Story_Arteries];
            if (story.Stage != StoryProgressStage.NotStarted) return;
            story.Stage = StoryProgressStage.Active;
            OnStoryStageChanged?.Invoke(Story_Arteries, "hook");
        }

        /// <summary>Stage 2: Plumber finds the Homeless guarding the valve.</summary>
        public void ArteriesValveFound()
        {
            var story = _stories[Story_Arteries];
            if (story.Stage != StoryProgressStage.Active) return;
            story.Stage = StoryProgressStage.Climax;
            OnStoryStageChanged?.Invoke(Story_Arteries, "climax");
        }

        /// <summary>Final choice: force the valve or leave it open.</summary>
        public bool MakeArteriesChoice(string choiceId)
        {
            var story = _stories[Story_Arteries];
            if (story.Stage != StoryProgressStage.Climax) return false;

            story.Choice = choiceId;
            story.Stage = StoryProgressStage.Completed;

            if (choiceId == Choice_ForceValve)
            {
                story.WorldEffect = "Water purified. Homeless lose their only source.";
                OnPerkGranted?.Invoke(Char_Plumber, Perk_IronStomach);
                OnAfflictionGained?.Invoke(Char_Plumber, Affliction_PTSD);
            }
            else if (choiceId == Choice_LeaveValveOpen)
            {
                story.WorldEffect = "Bunker water tainted. -5 Health cap. Periodic ZoonoticFlu.";
                // Plumber refuses to work on pipes ever again
            }

            OnChoiceMade?.Invoke(Story_Arteries, choiceId);
            return true;
        }

        // ── Story: The Ashen Mirror (Defector) ────────────────────────

        /// <summary>Stage 1: Cult messenger arrives — the Defector's former lover.</summary>
        public void TriggerAshenMirror(int currentDay)
        {
            var story = _stories[Story_AshenMirror];
            if (story.Stage != StoryProgressStage.NotStarted) return;
            story.Stage = StoryProgressStage.Active;
            OnStoryStageChanged?.Invoke(Story_AshenMirror, "hook");
        }

        /// <summary>Stage 2: Inside the Cathedral. Confront the High Priest.</summary>
        public void AshenMirrorConfrontation()
        {
            var story = _stories[Story_AshenMirror];
            if (story.Stage != StoryProgressStage.Active) return;
            story.Stage = StoryProgressStage.Climax;
            OnStoryStageChanged?.Invoke(Story_AshenMirror, "climax");
        }

        /// <summary>Final choice: assassinate or join.</summary>
        public bool MakeAshenMirrorChoice(string choiceId)
        {
            var story = _stories[Story_AshenMirror];
            if (story.Stage != StoryProgressStage.Climax) return false;

            story.Choice = choiceId;
            story.Stage = StoryProgressStage.Completed;

            if (choiceId == Choice_AssassinatePriest)
            {
                story.WorldEffect = "Cult fractures into warring splinter cells. Map becomes dangerous.";
                OnPerkGranted?.Invoke(Char_Defector, Perk_ColdBlooded);
            }
            else if (choiceId == Choice_JoinCult)
            {
                story.WorldEffect = "Defector leaves bunker. Cult demands monthly tithe.";
                OnCharacterDeparted?.Invoke(Char_Defector);
            }

            OnChoiceMade?.Invoke(Story_AshenMirror, choiceId);
            return true;
        }

        // ── Queries ───────────────────────────────────────────────────

        public StoryState GetStory(string storyId)
        {
            return _stories.TryGetValue(storyId, out var s) ? s : null;
        }

        public bool IsStoryCompleted(string storyId)
        {
            return _stories.TryGetValue(storyId, out var s)
                && s.Stage == StoryProgressStage.Completed;
        }

        public string GetStoryChoice(string storyId)
        {
            return _stories.TryGetValue(storyId, out var s) ? s.Choice : null;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public CharacterStorySave CaptureState()
        {
            var entries = new StoryStateSave[_stories.Count];
            int i = 0;
            foreach (var kv in _stories)
            {
                var s = kv.Value;
                entries[i++] = new StoryStateSave
                {
                    Id = s.Id,
                    CharacterId = s.CharacterId,
                    Stage = s.Stage,
                    Choice = s.Choice,
                    WorldEffect = s.WorldEffect
                };
            }
            return new CharacterStorySave { Stories = entries };
        }

        public void RestoreState(CharacterStorySave save)
        {
            _stories.Clear();
            if (save?.Stories == null) return;
            for (int i = 0; i < save.Stories.Length; i++)
            {
                var e = save.Stories[i];
                if (e == null || string.IsNullOrEmpty(e.Id)) continue;
                _stories[e.Id] = new StoryState
                {
                    Id = e.Id,
                    CharacterId = e.CharacterId,
                    Stage = e.Stage,
                    Choice = e.Choice,
                    WorldEffect = e.WorldEffect
                };
            }
        }
    }

    public enum StoryProgressStage
    {
        NotStarted,
        Active,
        Climax,
        Completed
    }

    public class StoryState
    {
        public string Id;
        public string CharacterId;
        public StoryProgressStage Stage;
        public string Choice;
        public string WorldEffect;
    }

    [Serializable]
    public class CharacterStorySave
    {
        public StoryStateSave[] Stories;
    }

    [Serializable]
    public class StoryStateSave
    {
        public string Id;
        public string CharacterId;
        public StoryProgressStage Stage;
        public string Choice;
        public string WorldEffect;
    }
}
