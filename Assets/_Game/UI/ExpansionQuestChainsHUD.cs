using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class QuestChainSnapshot
    {
        public string questChainId;
        public string displayName;
        public int currentStage;
        public int totalStages;
        public bool isCompleted;
        public bool isFailed;
        public string nextObjectiveText;
    }

    public class ExpansionQuestChainsSnapshot
    {
        public int totalQuestsCompleted;
        public List<QuestChainSnapshot> chains = new List<QuestChainSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Expansion Quest Chains & Wasteland Campaign HUD view-model.
    /// Monitors major campaign quest chains (The Iron Worm, The Radio Dark, The Dead Hand,
    /// The Redaction), objective stage progression, branching outcomes, and reward claims.
    /// </summary>
    public class ExpansionQuestChainsHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedChainIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnExpansionQuestChainsChanged;
        public event Action<string> OnClaimQuestRewardRequested; // (questChainId)

        private Func<ExpansionQuestChainsSnapshot> _getSnapshot;
        private ExpansionQuestChainsSnapshot _snapshot;

        public void Bind(Func<ExpansionQuestChainsSnapshot> getSnapshot)
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

        public bool SelectNextChain()
        {
            if (!IsOpen || _snapshot == null || _snapshot.chains == null || _snapshot.chains.Count == 0)
                return false;
            SelectedChainIndex = (SelectedChainIndex + 1) % _snapshot.chains.Count;
            ReportOutcome("Selected campaign quest: " + GetSelectedChainName());
            return true;
        }

        public bool SelectPreviousChain()
        {
            if (!IsOpen || _snapshot == null || _snapshot.chains == null || _snapshot.chains.Count == 0)
                return false;
            SelectedChainIndex = (SelectedChainIndex - 1 + _snapshot.chains.Count) % _snapshot.chains.Count;
            ReportOutcome("Selected campaign quest: " + GetSelectedChainName());
            return true;
        }

        public bool RequestClaimReward()
        {
            if (!IsOpen || _snapshot == null || _snapshot.chains == null || _snapshot.chains.Count == 0)
            {
                ReportOutcome("No quest chain selected for reward claiming.");
                return false;
            }

            var chain = GetSelectedChain();
            if (chain == null) return false;

            if (!chain.isCompleted)
            {
                ReportOutcome("Quest chain " + chain.displayName + " is not yet completed.");
                return false;
            }

            if (OnClaimQuestRewardRequested == null)
            {
                ReportOutcome("Quest reward dispatcher link offline.");
                return false;
            }

            OnClaimQuestRewardRequested.Invoke(chain.questChainId);
            ReportOutcome("Claiming reward for completed quest chain [" + chain.displayName + "]...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No quest action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnExpansionQuestChainsChanged?.Invoke();
        }

        private QuestChainSnapshot GetSelectedChain()
        {
            if (_snapshot != null && _snapshot.chains != null && SelectedChainIndex >= 0 && SelectedChainIndex < _snapshot.chains.Count)
            {
                return _snapshot.chains[SelectedChainIndex];
            }
            return null;
        }

        private string GetSelectedChainName()
        {
            var c = GetSelectedChain();
            return c != null ? (c.displayName ?? c.questChainId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CAMPAIGN QUEST CHAINS & OBJECTIVES  [J] close  ·  [Tab] cycle  ·  [C] claim quest reward");

            if (_snapshot == null)
            {
                sb.Append("\nCampaign quest journal telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCAMPAIGN STATS: Total Quests Completed: ").Append(_snapshot.totalQuestsCompleted);

            sb.Append("\n\nMAJOR WASTELAND CAMPAIGN QUESTS:");
            if (_snapshot.chains == null || _snapshot.chains.Count == 0)
            {
                sb.Append("\n  No active campaign quest chains.");
            }
            else
            {
                for (int i = 0; i < _snapshot.chains.Count; i++)
                {
                    var chain = _snapshot.chains[i];
                    if (chain == null) continue;

                    bool selected = (i == SelectedChainIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(chain.displayName ?? chain.questChainId)
                      .Append(" — Stage ").Append(chain.currentStage).Append(" / ").Append(chain.totalStages);

                    if (chain.isFailed) sb.Append("  ✖ [FAILED]");
                    else if (chain.isCompleted) sb.Append("  ✔ [COMPLETED — CLAIM REWARD]");
                    else sb.Append("  [IN PROGRESS]");

                    if (!string.IsNullOrEmpty(chain.nextObjectiveText))
                        sb.Append("\n    [OBJECTIVE: ").Append(chain.nextObjectiveText).Append("]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nCAMPAIGN LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
