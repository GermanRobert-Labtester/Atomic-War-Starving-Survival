using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public enum EndgamePathStatus
    {
        Locked,
        InProgress,
        Achieved,
        Failed
    }

    [Serializable]
    public class EndgameVictoryPathData
    {
        public string PathId;
        public string PathName = "BUNKER AUTARKY";
        public float Progress01 = 0.75f;
        public EndgamePathStatus Status = EndgamePathStatus.InProgress;
    }

    public class EndgameVictoryPathTracker : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private int _survivalScore = 8450;
        [SerializeField] private List<EndgameVictoryPathData> _paths = new List<EndgameVictoryPathData>();

        private VisualElement _root;
        private Label _scoreLabel;
        private ScrollView _scroll;

        public event Action<int, List<EndgameVictoryPathData>> OnStateChanged;

        public int SurvivalScore => _survivalScore;
        public IReadOnlyList<EndgameVictoryPathData> Paths => _paths;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("endgame-victory-root") 
                      ?? _document.rootVisualElement.Q("endgame_victory_root");
                Bind();
                InitializeDefaultPathsIfEmpty();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _scoreLabel = _root.Q<Label>("overall_survival_score_label");
            _scroll = _root.Q<ScrollView>("victory_paths_scroll");
        }

        private void InitializeDefaultPathsIfEmpty()
        {
            if (_paths.Count == 0)
            {
                string[] pathNames = new string[]
                {
                    "PATH I: BUNKER AUTARKY (Self-Sufficiency)",
                    "PATH II: EMERGENCY BROADCAST TRANSMISSION",
                    "PATH III: MILITARY REINSTATEMENT DECREE",
                    "PATH IV: SURFACE EVACUATION CONVOY",
                    "PATH V: CULTIST COMMUNION SACRIFICE",
                    "PATH VI: WARLORD TITHE PACIFICATION",
                    "PATH VII: GEOTHERMAL DEEP EXCAVATION",
                    "PATH VIII: FORENSIC DATA EXFILTRATION"
                };

                for (int i = 0; i < 8; i++)
                {
                    _paths.Add(new EndgameVictoryPathData
                    {
                        PathId = $"path_{i + 1}",
                        PathName = pathNames[i],
                        Progress01 = (i + 1) * 0.12f,
                        Status = (EndgamePathStatus)(i % 4)
                    });
                }
            }
        }

        public void SetData(int survivalScore, List<EndgameVictoryPathData> paths)
        {
            _survivalScore = survivalScore;
            _paths = paths ?? new List<EndgameVictoryPathData>();
            RefreshUI();
            OnStateChanged?.Invoke(_survivalScore, _paths);
        }

        public void UpdatePath(string pathId, float progress01, EndgamePathStatus status)
        {
            var p = _paths.Find(x => x.PathId == pathId || x.PathName.Contains(pathId));
            if (p != null)
            {
                p.Progress01 = Mathf.Clamp01(progress01);
                p.Status = status;
                RefreshUI();
                OnStateChanged?.Invoke(_survivalScore, _paths);
            }
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_scoreLabel != null)
                _scoreLabel.text = $"SURVIVAL INDEX: {_survivalScore} PTS";

            if (_scroll != null)
            {
                _scroll.Clear();

                foreach (var path in _paths)
                {
                    VisualElement card = new VisualElement();
                    card.AddToClassList("victory-path-card");

                    VisualElement header = new VisualElement();
                    header.AddToClassList("victory-path-header");

                    Label nameLabel = new Label(path.PathName);
                    nameLabel.AddToClassList("victory-path-name");

                    Label badge = new Label(FormatStatus(path.Status));
                    badge.AddToClassList("status-badge");
                    badge.AddToClassList(GetStatusClass(path.Status));

                    header.Add(nameLabel);
                    header.Add(badge);

                    VisualElement track = new VisualElement();
                    track.AddToClassList("victory-path-track");

                    VisualElement fill = new VisualElement();
                    fill.AddToClassList("victory-path-fill");
                    fill.style.width = Length.Percent(Mathf.Clamp01(path.Progress01) * 100f);

                    track.Add(fill);

                    card.Add(header);
                    card.Add(track);

                    _scroll.Add(card);
                }
            }
        }

        private string FormatStatus(EndgamePathStatus status)
        {
            switch (status)
            {
                case EndgamePathStatus.Locked: return "[LOCKED]";
                case EndgamePathStatus.InProgress: return "[IN PROGRESS]";
                case EndgamePathStatus.Achieved: return "[ACHIEVED]";
                case EndgamePathStatus.Failed: return "[FAILED]";
                default: return status.ToString().ToUpper();
            }
        }

        private string GetStatusClass(EndgamePathStatus status)
        {
            switch (status)
            {
                case EndgamePathStatus.Locked: return "status-locked";
                case EndgamePathStatus.InProgress: return "status-in_progress";
                case EndgamePathStatus.Achieved: return "status-achieved";
                case EndgamePathStatus.Failed: return "status-failed";
                default: return "status-locked";
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
