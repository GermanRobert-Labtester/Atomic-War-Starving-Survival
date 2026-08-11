using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Player-facing dispatch board for catalog locations. It owns only
    /// selection and presentation; Core handles mission truth, loot, radiation,
    /// events, and save state after receiving a dispatch request.
    /// </summary>
    public class ScavengeDispatchHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string SelectedLocationId { get; private set; }
        public string SelectedSurvivorId { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;
        public IReadOnlyList<LocationDefinitionSO> Locations => _locations;
        public IReadOnlyList<Survivor> Survivors => _survivors;

        /// <summary>Raised when selection, availability, or a mission outcome changes.</summary>
        public event Action OnScavengeDispatchChanged;

        /// <summary>
        /// Raised when the player confirms the selected location. UI does not
        /// reference Core; GameBootstrap validates and starts the real mission
        /// and reports back whether it actually accepted the dispatch.
        /// </summary>
        public event Func<Survivor, LocationDefinitionSO, bool> OnDispatchRequested;

        private readonly List<LocationDefinitionSO> _locations = new List<LocationDefinitionSO>();
        private readonly List<Survivor> _survivors = new List<Survivor>();
        private LocationCatalogSO _catalog;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private Func<Survivor, string> _getDispatchBlockReason;
        private Func<Survivor, string> _getTaskLabel;
        private Func<string> _getMissionRoster;
        private Func<Survivor, LocationDefinitionSO, string> _getPreflightSummary;
        private Func<string, string> _getRadiationPreview;
        private Func<float, string> _getLootPreview;

        /// <summary>
        /// Core supplies availability and player-facing radiation delegates so
        /// cross-system constraints stay in the composition root.
        /// </summary>
        public void Bind(
            LocationCatalogSO catalog,
            Func<IReadOnlyList<Survivor>> getSurvivors,
            Func<Survivor, string> getDispatchBlockReason,
            Func<Survivor, string> getTaskLabel,
            Func<string> getMissionRoster,
            Func<Survivor, LocationDefinitionSO, string> getPreflightSummary,
            Func<string, string> getRadiationPreview,
            Func<float, string> getLootPreview)
        {
            _catalog = catalog;
            _getSurvivors = getSurvivors;
            _getDispatchBlockReason = getDispatchBlockReason;
            _getTaskLabel = getTaskLabel;
            _getMissionRoster = getMissionRoster;
            _getPreflightSummary = getPreflightSummary;
            _getRadiationPreview = getRadiationPreview;
            _getLootPreview = getLootPreview;
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

        /// <summary>Move selection through the imported location catalog.</summary>
        public bool SelectNext()
        {
            return SelectByOffset(1);
        }

        /// <summary>Move selection through the imported location catalog.</summary>
        public bool SelectPrevious()
        {
            return SelectByOffset(-1);
        }

        /// <summary>Cycle to the next currently eligible crew member.</summary>
        public bool SelectNextSurvivor()
        {
            return SelectSurvivorByOffset(1);
        }

        /// <summary>Cycle to the previous currently eligible crew member.</summary>
        public bool SelectPreviousSurvivor()
        {
            return SelectSurvivorByOffset(-1);
        }

        /// <summary>
        /// Request the selected, discrete scavenging action. The request is
        /// handed to Core; this component cannot fabricate loot or time.
        /// </summary>
        public bool DispatchSelected()
        {
            if (!IsOpen || !TryGetSelectedLocation(out var location)) return false;

            var survivor = GetSelectedSurvivor();
            if (!CanDispatch(survivor, out var reason))
            {
                LastOutcome = "HELD: " + reason;
                Refresh();
                return false;
            }

            if (OnDispatchRequested == null)
            {
                LastOutcome = "HELD: dispatch link offline.";
                Refresh();
                return false;
            }

            // The subscriber (Core) reports back whether it actually accepted
            // the dispatch, so a rejection (e.g. hatch sealed, mission system
            // refused) can no longer be masked behind a true return here.
            return OnDispatchRequested.Invoke(survivor, location);
        }

        /// <summary>Live eligibility for the selected location, suitable for tests and future buttons.</summary>
        public bool CanDispatchSelected(out string reason)
        {
            var survivor = GetSelectedSurvivor();
            if (!TryGetSelectedLocation(out _))
            {
                reason = "No scavenge location available.";
                return false;
            }
            return CanDispatch(survivor, out reason);
        }

        public void Refresh()
        {
            SyncSurvivors();
            SyncLocations();
            RebuildPanel();
            OnScavengeDispatchChanged?.Invoke();
        }

        private bool SelectByOffset(int offset)
        {
            SyncLocations();
            if (_locations.Count == 0) return false;

            int currentIndex = IndexOfSelected();
            if (currentIndex < 0) currentIndex = offset < 0 ? 0 : -1;
            int next = (currentIndex + offset) % _locations.Count;
            if (next < 0) next += _locations.Count;
            SelectedLocationId = _locations[next].id;
            Refresh();
            return true;
        }

        private bool SelectSurvivorByOffset(int offset)
        {
            SyncSurvivors();
            if (_survivors.Count == 0) return false;

            int current = IndexOfSelectedSurvivor();
            if (current < 0) current = offset < 0 ? 0 : -1;

            for (int step = 1; step <= _survivors.Count; step++)
            {
                int next = (current + offset * step) % _survivors.Count;
                if (next < 0) next += _survivors.Count;

                var candidate = _survivors[next];
                if (!CanDispatch(candidate, out _)) continue;

                SelectedSurvivorId = candidate.Id;
                Refresh();
                return true;
            }

            Refresh();
            return false;
        }

        private bool CanDispatch(Survivor survivor, out string reason)
        {
            if (survivor == null || !survivor.IsAlive)
            {
                reason = "No living operative available.";
                return false;
            }
            if (_getDispatchBlockReason != null)
            {
                reason = _getDispatchBlockReason(survivor);
                if (!string.IsNullOrEmpty(reason)) return false;
            }

            reason = string.Empty;
            return true;
        }

        private void SyncSurvivors()
        {
            _survivors.Clear();
            var source = _getSurvivors != null ? _getSurvivors() : null;
            if (source != null)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var survivor = source[i];
                    if (survivor != null && survivor.IsAlive && !string.IsNullOrEmpty(survivor.Id))
                        _survivors.Add(survivor);
                }
            }

            if (_survivors.Count == 0)
            {
                SelectedSurvivorId = null;
                return;
            }
            if (IndexOfSelectedSurvivor() >= 0) return;

            for (int i = 0; i < _survivors.Count; i++)
            {
                if (CanDispatch(_survivors[i], out _))
                {
                    SelectedSurvivorId = _survivors[i].Id;
                    return;
                }
            }

            SelectedSurvivorId = _survivors[0].Id;
        }

        private void SyncLocations()
        {
            _locations.Clear();
            if (_catalog?.locations != null)
            {
                for (int i = 0; i < _catalog.locations.Count; i++)
                {
                    var location = _catalog.locations[i];
                    if (location != null && !string.IsNullOrEmpty(location.id))
                        _locations.Add(location);
                }
            }

            if (_locations.Count == 0)
            {
                SelectedLocationId = null;
                return;
            }
            if (IndexOfSelected() < 0)
                SelectedLocationId = _locations[0].id;
        }

        private int IndexOfSelected()
        {
            if (string.IsNullOrEmpty(SelectedLocationId)) return -1;
            for (int i = 0; i < _locations.Count; i++)
            {
                if (string.Equals(_locations[i].id, SelectedLocationId, StringComparison.Ordinal))
                    return i;
            }
            return -1;
        }

        private int IndexOfSelectedSurvivor()
        {
            if (string.IsNullOrEmpty(SelectedSurvivorId)) return -1;
            for (int i = 0; i < _survivors.Count; i++)
            {
                if (string.Equals(_survivors[i].Id, SelectedSurvivorId, StringComparison.Ordinal))
                    return i;
            }
            return -1;
        }

        private Survivor GetSelectedSurvivor()
        {
            int index = IndexOfSelectedSurvivor();
            return index >= 0 && index < _survivors.Count ? _survivors[index] : null;
        }

        private bool TryGetSelectedLocation(out LocationDefinitionSO location)
        {
            location = null;
            int index = IndexOfSelected();
            if (index < 0 || index >= _locations.Count) return false;
            location = _locations[index];
            return location != null;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("SCAVENGE DISPATCH  [G] close  ·  [,/.] site  ·  [TAB] crew  ·  [ENTER] send");

            var survivor = GetSelectedSurvivor();
            if (_locations.Count == 0)
            {
                sb.Append("No scavenge locations are available.");
                PanelSummary = sb.ToString();
                return;
            }

            string availability = CanDispatch(survivor, out var reason)
                ? "READY"
                : "HELD — " + reason;
            sb.Append("OPERATIVE: ").Append(DisplaySurvivorName(survivor));
            if (survivor != null)
            {
                sb.Append("  health ").Append(survivor.Needs.Health.ToString("0"))
                    .Append("  dose ").Append(survivor.RadiationDose.ToString("0"));
            }
            sb.AppendLine();
            sb.AppendLine("STATUS: " + availability);

            sb.AppendLine("--- CREW ROSTER ---");
            if (_survivors.Count == 0)
            {
                sb.AppendLine("No living crew available.");
            }
            else
            {
                for (int i = 0; i < _survivors.Count; i++)
                {
                    var candidate = _survivors[i];
                    bool selected = candidate.Id == SelectedSurvivorId;
                    string candidateStatus = CanDispatch(candidate, out var candidateReason)
                        ? "ready"
                        : "held — " + candidateReason;
                    sb.Append(selected ? "> " : "  ")
                        .Append(DisplaySurvivorName(candidate))
                        .Append("  health ").Append(candidate.Needs.Health.ToString("0"))
                        .Append("  dose ").Append(candidate.RadiationDose.ToString("0"))
                        .Append("  task ").Append(TaskLabel(candidate))
                        .Append("  ").Append(candidateStatus)
                        .AppendLine();
                }
            }

            for (int i = 0; i < _locations.Count; i++)
            {
                var location = _locations[i];
                bool selected = location.id == SelectedLocationId;
                sb.Append(selected ? "> " : "  ")
                    .Append(i + 1).Append(". ")
                    .Append(DisplayLocationName(location))
                    .Append("  ").Append(location.travelHours.ToString("0.#")).Append("h")
                    .Append("  rad ").Append(RadiationLabel(location.id))
                    .Append("  danger ").Append(location.dangerLevel.ToString("0"))
                    .AppendLine();
            }

            if (TryGetSelectedLocation(out var selectedLocation))
            {
                sb.AppendLine("--- SELECTED ---");
                sb.AppendLine(selectedLocation.description ?? string.Empty);
                sb.Append("RISK: ").Append(selectedLocation.travelHours.ToString("0.#"))
                    .Append("h exposed travel · rad ").Append(RadiationLabel(selectedLocation.id))
                    .Append(" · danger ").Append(selectedLocation.dangerLevel.ToString("0.0"))
                    .AppendLine();
                sb.Append("SUPPLY: ").AppendLine(LootPreview(selectedLocation.dangerLevel));

                string preflight = _getPreflightSummary != null
                    ? _getPreflightSummary(survivor, selectedLocation)
                    : null;
                if (!string.IsNullOrEmpty(preflight))
                {
                    sb.AppendLine();
                    sb.AppendLine(preflight.TrimEnd());
                }
            }

            string missionRoster = _getMissionRoster != null ? _getMissionRoster() : null;
            if (!string.IsNullOrEmpty(missionRoster))
            {
                sb.AppendLine();
                sb.AppendLine(missionRoster.TrimEnd());
            }

            if (!string.IsNullOrEmpty(LastOutcome))
                sb.AppendLine("REPORT: " + LastOutcome);
            PanelSummary = sb.ToString().TrimEnd();
        }

        private string LootPreview(float dangerLevel)
        {
            string preview = _getLootPreview != null ? _getLootPreview(dangerLevel) : null;
            return string.IsNullOrEmpty(preview) ? "unknown yield." : preview;
        }

        private string RadiationLabel(string locationId)
        {
            string label = _getRadiationPreview != null ? _getRadiationPreview(locationId) : null;
            return string.IsNullOrEmpty(label) ? "?" : label;
        }

        private string TaskLabel(Survivor survivor)
        {
            string label = _getTaskLabel != null ? _getTaskLabel(survivor) : null;
            if (!string.IsNullOrEmpty(label)) return label;
            return survivor == null ? "—" : survivor.State.ToString().ToLowerInvariant();
        }

        private static string DisplaySurvivorName(Survivor survivor)
        {
            if (survivor == null) return "—";
            return string.IsNullOrEmpty(survivor.DisplayName) ? survivor.Id : survivor.DisplayName;
        }

        private static string DisplayLocationName(LocationDefinitionSO location)
        {
            if (location == null) return "—";
            return string.IsNullOrEmpty(location.displayName) ? location.id : location.displayName;
        }

        /// <summary>Core reports that the requested live mission was accepted.</summary>
        public void ReportMissionStarted(string operativeName, string destinationName, float returnHours)
        {
            LastOutcome = $"DISPATCHED: {operativeName} → {destinationName}. "
                + $"Return window {returnHours:0.#}h.";
            Refresh();
        }

        /// <summary>Core reports a live mission outcome after inventory receives its loot.</summary>
        public void ReportMissionCompleted(string locationName, int recoveredItemCount)
        {
            var sb = new StringBuilder();
            sb.Append("RETURNED: ").Append(locationName).Append(" — ");
            if (recoveredItemCount <= 0)
            {
                sb.Append("nothing recoverable.");
            }
            else
            {
                sb.Append("recovered ").Append(recoveredItemCount).Append(" item")
                    .Append(recoveredItemCount == 1 ? "." : "s.");
            }
            LastOutcome = sb.ToString();
            Refresh();
        }

        /// <summary>
        /// Displays an authoritative Core-formatted after-action report. The UI
        /// receives text only so it cannot alter mission, inventory, or radiation truth.
        /// </summary>
        public void ReportAfterAction(string summary)
        {
            LastOutcome = string.IsNullOrEmpty(summary)
                ? "RETURNED: report link failed."
                : summary;
            Refresh();
        }

        /// <summary>Core reports a rejected request with its authoritative reason.</summary>
        public void ReportDispatchHeld(string reason)
        {
            LastOutcome = "HELD: " + (string.IsNullOrEmpty(reason)
                ? "dispatch rejected by the mission system."
                : reason);
            Refresh();
        }
    }
}
