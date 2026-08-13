using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Interactable expedition map: reads <see cref="GeneratedMap"/> node data,
    /// shows silhouette/rumoredRad fog-of-war, computes weather-scaled pathing,
    /// and raises an expedition request for Core to fulfill.
    /// </summary>
    public class MapScreenUI : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string SelectedNodeId { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string DetailSummary { get; private set; } = string.Empty;
        public string PathSummary { get; private set; } = string.Empty;

        public IReadOnlyList<MapNodePlayerView> NodeViews => _views;
        public IReadOnlyList<string> SelectedPath => _selectedPath;

        /// <summary>Raised when the player confirms an expedition to the selected node.</summary>
        public event Action<Survivor, string /*nodeId*/, ExpeditionPathRequest> OnExpeditionRequested;

        public event Action OnMapScreenChanged;

        private GeneratedMap _map;
        private Func<WeatherKind> _getWeather;
        private readonly List<MapNodePlayerView> _views = new List<MapNodePlayerView>();
        private readonly List<string> _selectedPath = new List<string>();
        private readonly List<string> _routeBuffer = new List<string>();
        private readonly List<MapPathLine> _pathLines = new List<MapPathLine>();
        private readonly GenericObjectPool<MapPathLine> _pathLinePool = new GenericObjectPool<MapPathLine>(
            () => new MapPathLine(),
            line =>
            {
                line.FromLabel = null;
                line.ToLabel = null;
                line.SegmentHours = 0f;
            });
        private float _selectedTravelHours;
        private WeatherKind _pathWeather = WeatherKind.Clear;

        /// <summary>Pooled per-hop path segments for the selected expedition route (never destroyed, recycled on rebuild).</summary>
        public IReadOnlyList<MapPathLine> PathLines => _pathLines;

        /// <summary>Pool backing <see cref="PathLines"/> (profiling/test hook).</summary>
        public GenericObjectPool<MapPathLine> PathLinePool => _pathLinePool;

        public void Bind(GeneratedMap map, Func<WeatherKind> getWeather = null)
        {
            if (_map != null)
                _map.OnMapChanged -= Refresh;

            _map = map;
            _getWeather = getWeather ?? (() => WeatherKind.Clear);

            if (_map != null)
                _map.OnMapChanged += Refresh;

            Refresh();
        }

        public void Open()
        {
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            IsOpen = false;
            OnMapScreenChanged?.Invoke();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        /// <summary>Select a destination node (or clear with null/empty).</summary>
        public bool SelectNode(string nodeId)
        {
            if (_map == null || string.IsNullOrEmpty(nodeId))
            {
                SelectedNodeId = null;
                _selectedPath.Clear();
                _selectedTravelHours = 0f;
                RebuildDetail();
                OnMapScreenChanged?.Invoke();
                return false;
            }

            var node = _map.GetNode(nodeId);
            if (node == null) return false;
            if (node.IsShelter)
            {
                SelectedNodeId = nodeId;
                _selectedPath.Clear();
                _selectedPath.Add(GeneratedMap.ShelterNodeId);
                _selectedTravelHours = 0f;
                RebuildDetail();
                OnMapScreenChanged?.Invoke();
                return true;
            }

            SelectedNodeId = nodeId;
            RebuildPath();
            RebuildDetail();
            OnMapScreenChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Confirm expedition to the selected node. Core handles StartExpedition;
        /// returns false if selection/path invalid.
        /// </summary>
        /// <summary>
        /// When false, RequestExpedition is a no-op (Prompt #48 hatch seal).
        /// Wired from ExpeditionSystem.IsExpeditionUiEnabled / HatchEntrapmentSystem.
        /// </summary>
        public bool IsExpeditionUiEnabled { get; set; } = true;

        public bool RequestExpedition(Survivor survivor)
        {
            if (!IsExpeditionUiEnabled) return false;
            if (_map == null || survivor == null || !survivor.IsAlive) return false;
            if (string.IsNullOrEmpty(SelectedNodeId)) return false;
            var node = _map.GetNode(SelectedNodeId);
            if (node == null || node.IsShelter) return false;
            if (_selectedPath == null || _selectedPath.Count < 2) return false;

            var request = new ExpeditionPathRequest
            {
                NodeId = SelectedNodeId,
                PathNodeIds = new List<string>(_selectedPath),
                TravelHours = _selectedTravelHours,
                Weather = _pathWeather,
                WeatherMultiplier = GeneratedMap.WeatherTravelMultiplier(_pathWeather),
                DistanceFromShelter = node.DistanceFromShelter,
                LootTableId = node.LootTableId,
                DangerLevel = node.DangerLevel,
                TrueRad = node.TrueRad,
                IsRevealed = node.IsRevealed || node.IsVisited
            };

            OnExpeditionRequested?.Invoke(survivor, SelectedNodeId, request);
            return true;
        }

        public float GetSelectedTravelHours() => _selectedTravelHours;

        public WeatherKind GetPathWeather() => _pathWeather;

        /// <summary>Travel hours for an arbitrary destination under current weather.</summary>
        public float PreviewTravelHours(string nodeId)
        {
            if (_map == null || string.IsNullOrEmpty(nodeId)) return 0f;
            return _map.GetTravelHoursFromShelter(nodeId, CurrentWeather());
        }

        public void Refresh()
        {
            if (_map != null)
                _map.GetAllPlayerViews(_views); // buffer overload: no per-refresh list allocation
            else
                _views.Clear();

            if (!string.IsNullOrEmpty(SelectedNodeId) && _map != null && _map.GetNode(SelectedNodeId) != null)
                RebuildPath();
            else
            {
                _selectedPath.Clear();
                _selectedTravelHours = 0f;
                ReleasePathLines();
            }

            RebuildPanel();
            RebuildDetail();
            OnMapScreenChanged?.Invoke();
        }

        /// <summary>Full debug / OnGUI panel text.</summary>
        public string BuildPanelText()
        {
            Refresh();
            return PanelSummary + "\n" + DetailSummary + "\n" + PathSummary;
        }

        private WeatherKind CurrentWeather()
        {
            return _getWeather != null ? _getWeather() : WeatherKind.Clear;
        }

        private void RebuildPath()
        {
            _selectedPath.Clear();
            _selectedTravelHours = 0f;
            _pathWeather = CurrentWeather();
            ReleasePathLines();
            if (_map == null || string.IsNullOrEmpty(SelectedNodeId)) return;

            if (!_map.TryFindPath(GeneratedMap.ShelterNodeId, SelectedNodeId, _routeBuffer))
                return;
            for (int i = 0; i < _routeBuffer.Count; i++)
                _selectedPath.Add(_routeBuffer[i]);

            // Pooled per-hop path lines: recycle the previous route, acquire one
            // line per hop. Steady-state selection churn allocates nothing.
            for (int i = 0; i < _selectedPath.Count - 1; i++)
            {
                var line = _pathLinePool.Acquire();
                if (line == null) break; // pool capped: degrade gracefully
                var from = _map.GetNode(_selectedPath[i]);
                var to = _map.GetNode(_selectedPath[i + 1]);
                line.FromLabel = from != null ? from.GetDisplayLabel() : _selectedPath[i];
                line.ToLabel = to != null ? to.GetDisplayLabel() : _selectedPath[i + 1];
                var edge = _map.GetPath(_selectedPath[i], _selectedPath[i + 1]);
                line.SegmentHours = edge != null ? edge.BaseTravelHours : 0f;
                _pathLines.Add(line);
            }

            _selectedTravelHours = _map.GetTravelHoursFromShelter(SelectedNodeId, _pathWeather);
        }

        /// <summary>Recycle all active path lines back into the pool (never destroys them).</summary>
        private void ReleasePathLines()
        {
            for (int i = 0; i < _pathLines.Count; i++)
                _pathLinePool.Release(_pathLines[i]);
            _pathLines.Clear();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder();
            sb.Append("WASTELAND MAP");
            if (_map != null)
                sb.Append("  seed=").Append(_map.Seed);
            sb.Append("  weather=").Append(CurrentWeather());
            float mult = GeneratedMap.WeatherTravelMultiplier(CurrentWeather());
            if (mult > 1.001f)
                sb.Append("  (travel x").Append(mult.ToString("0.##")).Append(")");
            sb.AppendLine();

            if (_views.Count == 0)
            {
                sb.Append("No map generated.");
                PanelSummary = sb.ToString();
                return;
            }

            for (int i = 0; i < _views.Count; i++)
            {
                var v = _views[i];
                string mark = v.NodeId == SelectedNodeId ? ">" : " ";
                string ring = v.IsShelter ? "HUB" : v.Ring.ToString();
                string fog = v.IsSilhouette ? "SIL" : (v.IsVisited ? "VIS" : "REV");
                string rad = float.IsNaN(v.DisplayedRad) ? "?" : $"~{v.DisplayedRad:0}";
                sb.Append(mark)
                    .Append(" [").Append(ring).Append("] ")
                    .Append(v.Label)
                    .Append("  dist=").Append(v.DistanceFromShelter.ToString("0.0")).Append("h")
                    .Append("  rad=").Append(rad)
                    .Append("  ").Append(fog)
                    .AppendLine();
            }
            PanelSummary = sb.ToString().TrimEnd();
        }

        private string BuildDetailSummary(MapNodePlayerView view, MapNode node)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECTED: " + view.Label);
            sb.Append("Ring: ").Append(view.Ring);
            if (view.IsSilhouette)
                sb.Append("  (unsurveyed silhouette)");
            sb.AppendLine();
            sb.Append("Base distance: ").Append(view.DistanceFromShelter.ToString("0.0")).Append("h");
            sb.Append("  Weather travel: ").Append(_selectedTravelHours.ToString("0.0")).Append("h");
            sb.Append("  (").Append(_pathWeather).Append(" x")
                .Append(GeneratedMap.WeatherTravelMultiplier(_pathWeather).ToString("0.##")).AppendLine(")");

            if (view.IsSilhouette)
            {
                sb.Append("Rumored rad: ~").Append(view.RumoredRad.ToString("0")).AppendLine(" /h");
                sb.AppendLine("Intel incomplete — radio or visit to reveal loot/encounters.");
            }
            else if (node != null)
            {
                sb.Append("Rad: ").Append(view.DisplayedRad.ToString("0")).AppendLine(" /h");
                sb.Append("Loot: ").Append(string.IsNullOrEmpty(node.LootTableId) ? "—" : node.LootTableId).AppendLine();
                sb.Append("Danger: ").Append(node.DangerLevel.ToString("0.0")).AppendLine();
                if (node.EncounterDeckIds != null && node.EncounterDeckIds.Count > 0)
                {
                    sb.Append("Encounters: ");
                    for (int i = 0; i < node.EncounterDeckIds.Count; i++)
                    {
                        if (i > 0) sb.Append(", ");
                        sb.Append(node.EncounterDeckIds[i]);
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString().TrimEnd();
        }

        private string BuildPathSummary()
        {
            var pathSb = new StringBuilder();
            pathSb.Append("PATH: ");
            if (_selectedPath.Count == 0)
                pathSb.Append("(none)");
            else if (_pathLines.Count > 0)
            {
                // Render from pooled path lines (no node lookups, no allocations
                // beyond the string itself).
                pathSb.Append(_pathLines[0].FromLabel);
                for (int i = 0; i < _pathLines.Count; i++)
                    pathSb.Append(" → ").Append(_pathLines[i].ToLabel);
            }
            else
            {
                for (int i = 0; i < _selectedPath.Count; i++)
                {
                    if (i > 0) pathSb.Append(" → ");
                    var n = _map.GetNode(_selectedPath[i]);
                    pathSb.Append(n != null ? n.GetDisplayLabel() : _selectedPath[i]);
                }
            }
            return pathSb.ToString();
        }

        private void RebuildDetail()
        {
            if (_map == null || string.IsNullOrEmpty(SelectedNodeId))
            {
                DetailSummary = "Select a node for expedition pathing.";
                PathSummary = string.Empty;
                return;
            }

            var view = _map.GetPlayerView(SelectedNodeId);
            var node = _map.GetNode(SelectedNodeId);
            DetailSummary = BuildDetailSummary(view, node);
            PathSummary = BuildPathSummary();
        }
    }

    /// <summary>
    /// One pooled hop of the selected expedition route (rendered as a path line).
    /// Instances live in MapScreenUI's GenericObjectPool — recycled, never destroyed.
    /// </summary>
    public class MapPathLine
    {
        public string FromLabel;
        public string ToLabel;
        public float SegmentHours;
    }

    /// <summary>Pathing payload from MapScreenUI to Core expedition start.</summary>
    [Serializable]
    public class ExpeditionPathRequest
    {
        public string NodeId;
        public List<string> PathNodeIds = new List<string>();
        public float TravelHours;
        public WeatherKind Weather;
        public float WeatherMultiplier = 1f;
        public float DistanceFromShelter;
        public string LootTableId;
        public float DangerLevel;
        public float TrueRad;
        public bool IsRevealed;
    }
}
