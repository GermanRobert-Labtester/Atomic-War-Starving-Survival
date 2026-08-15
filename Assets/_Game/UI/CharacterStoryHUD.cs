using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class CharacterStoryArcSnapshot
    {
        public string storyId;
        public string characterId;
        public string characterName;
        public string currentStage;
        public string[] availableChoices;
        public string[] unlockedPerks;
    }

    public class CharacterStorySnapshot
    {
        public int totalStoriesCompleted;
        public List<CharacterStoryArcSnapshot> arcs = new List<CharacterStoryArcSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Character Story Arcs & Legacy HUD view-model.
    /// Manages survivor personal narrative quests (The Reporter: The Redaction,
    /// The Plumber: The Arteries, The Defector: The Ashen Mirror), moral decision dilemmas,
    /// perk unlocks (Truth Teller, Iron Stomach, Cold Blooded), and legacy outcomes.
    /// </summary>
    public class CharacterStoryHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedArcIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnCharacterStoryChanged;
        public event Action<string, string> OnMakeStoryChoiceRequested; // (storyId, choiceId)

        private Func<CharacterStorySnapshot> _getSnapshot;
        private CharacterStorySnapshot _snapshot;

        public void Bind(Func<CharacterStorySnapshot> getSnapshot)
        {
            _getSnapshot = getSnapshot;
            Refresh();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public bool SelectNextArc()
        {
            if (!IsOpen || _snapshot == null || _snapshot.arcs == null || _snapshot.arcs.Count == 0)
                return false;
            SelectedArcIndex = (SelectedArcIndex + 1) % _snapshot.arcs.Count;
            ReportOutcome("Selected character story arc: " + GetSelectedArcName());
            return true;
        }

        public bool SelectPreviousArc()
        {
            if (!IsOpen || _snapshot == null || _snapshot.arcs == null || _snapshot.arcs.Count == 0)
                return false;
            SelectedArcIndex = (SelectedArcIndex - 1 + _snapshot.arcs.Count) % _snapshot.arcs.Count;
            ReportOutcome("Selected character story arc: " + GetSelectedArcName());
            return true;
        }

        public bool RequestMakeChoice(string choiceId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.arcs == null || _snapshot.arcs.Count == 0)
            {
                ReportOutcome("No character story arc selected for decision.");
                return false;
            }

            var arc = GetSelectedArc();
            if (arc == null) return false;

            if (OnMakeStoryChoiceRequested == null)
            {
                ReportOutcome("Story decision engine offline.");
                return false;
            }

            OnMakeStoryChoiceRequested.Invoke(arc.storyId, choiceId);
            ReportOutcome("Executed narrative choice [" + choiceId + "] for " + arc.characterName + " (" + arc.storyId + ")");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No story action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnCharacterStoryChanged?.Invoke();
        }

        private CharacterStoryArcSnapshot GetSelectedArc()
        {
            if (_snapshot != null && _snapshot.arcs != null && SelectedArcIndex >= 0 && SelectedArcIndex < _snapshot.arcs.Count)
            {
                return _snapshot.arcs[SelectedArcIndex];
            }
            return null;
        }

        private string GetSelectedArcName()
        {
            var a = GetSelectedArc();
            return a != null ? (a.characterName + " — " + a.storyId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CHARACTER STORY ARCS & LEGACY JOURNAL  [H] close  ·  [Tab] cycle  ·  [1..3] choose outcome");

            if (_snapshot == null)
            {
                sb.Append("\nCharacter story journal telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nJOURNAL STATS: Arcs Completed: ").Append(_snapshot.totalStoriesCompleted);

            sb.Append("\n\nCHARACTER PERSONAL QUEST ARCS:");
            if (_snapshot.arcs == null || _snapshot.arcs.Count == 0)
            {
                sb.Append("\n  No active character story arcs recorded.");
            }
            else
            {
                for (int i = 0; i < _snapshot.arcs.Count; i++)
                {
                    var arc = _snapshot.arcs[i];
                    if (arc == null) continue;

                    bool selected = (i == SelectedArcIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(arc.characterName ?? arc.characterId)
                      .Append(" — Arc: ").Append(arc.storyId)
                      .Append(" (Stage: ").Append(arc.currentStage ?? "Initial").Append(")");

                    if (arc.unlockedPerks != null && arc.unlockedPerks.Length > 0)
                    {
                        sb.Append("\n    [UNLOCKED PERKS: ").Append(string.Join(", ", arc.unlockedPerks)).Append("]");
                    }

                    if (arc.availableChoices != null && arc.availableChoices.Length > 0)
                    {
                        sb.Append("\n    [CHOICES AVAILABLE: ").Append(string.Join(" | ", arc.availableChoices)).Append("]");
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nJOURNAL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
