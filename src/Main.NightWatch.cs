// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 36 — The Watch
// Campaign composition, canonical-owner bindings, lifecycle, and player route.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private NightWatchHostSession? _nightWatch;
        private NightWatchPanel? _nightWatchPanel;
        private bool _nightWatchDirty;

        public NightWatchHostSession? NightWatch => _nightWatch;

        private void SetupNightWatch()
        {
            if (_nightWatch != null) return;

            SetupInventory();
            SetupDutyRoster();
            SetupWorld();
            SetupPerimeterDefense();
            SetupSoundRanging();
            SetupShelterSecurity();
            SetupTerritoryControl();

            string dataDir = CatalogPath.ResolveDataDir();
            var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
            var load = NightWatchOperationsCatalogLoader.Load(dataDir, fileIO);
            if (!load.Success || load.Catalog == null)
            {
                GD.PushError("[NightWatch] operations catalog unavailable: " + string.Join("; ", load.Errors));
                return;
            }

            var locationErrors = new List<string>();
            ValidateNightWatchLocations(fileIO, dataDir, load.Catalog, locationErrors);
            if (locationErrors.Count > 0)
            {
                GD.PushError("[NightWatch] catalog location integrity failed: " + string.Join("; ", locationErrors));
                return;
            }

            NightWatchCatalog? narrative = null;
            string narrativePath = CatalogPath.ResolveSub("narrative", "night_watch_logbook.json");
            if (fileIO.FileExists(narrativePath))
            {
                narrative = new NightWatchCatalog();
                narrative.Load(fileIO.ReadAllText(narrativePath), new SystemTextJsonSerializer());
            }

            _nightWatch = new NightWatchHostSession(
                _perimeterDefense ?? throw new InvalidOperationException("perimeter owner unavailable"),
                _dutyRoster.Roster,
                _shelterSecurity.System,
                load.Catalog,
                narrative,
                _soundRanging?.System)
            {
                SurvivorFatigueProvider = id =>
                {
                    var state = _survivors?.Find(id);
                    return state == null ? 0 : Math.Clamp((int)(state.Fatigue * 1000f), 0, 1000);
                },
                TerritoryContestedProvider = sector =>
                {
                    string location = sector switch
                    {
                        "north" => "loc_signal_hill_tower",
                        "east" => "loc_river_bend_outpost",
                        "south" => "loc_south_beacon_tower",
                        "west" => "loc_shelter_perimeter",
                        _ => "loc_shelter_gate"
                    };
                    return _territoryControl?.System.GetLocationState(location)?.IsContested ?? false;
                },
                JournalWriter = (key, text, day) =>
                {
                    _journal?.TryAddRawEntry(key, text, null!, day);
                    _journalDirty = true;
                    _nightWatchDirty = true;
                },
                AlarmWriter = text => GD.Print("[NightWatch] " + text)
            };

            _nightWatch.SetCurrentDay(CurrentWatchDay());
            _perimeterDefense!.OnEventRaised += OnNightWatchPerimeterEvent;
            if (_soundRanging != null)
                _soundRanging.OnThreatEstimate += OnNightWatchAcousticEstimate;
            _nightWatch.StateChanged += () => _nightWatchDirty = true;
            _nightWatchDirty = false;
            GD.Print($"[NightWatch] {load.Catalog.posts.Count} posts, {load.Catalog.routes.Count} routes, {load.Catalog.drills.Count} drills loaded.");
        }

        private void ValidateNightWatchLocations(
            IFileIO fileIO,
            string dataDir,
            NightWatchOperationsCatalog catalog,
            List<string> errors)
        {
            string path = fileIO.Combine(dataDir, "locations.json");
            if (!fileIO.FileExists(path)) return;
            try
            {
                using var document = JsonDocument.Parse(fileIO.ReadAllText(path));
                var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (document.RootElement.TryGetProperty("locations", out var rows) && rows.ValueKind == JsonValueKind.Array)
                {
                    foreach (var row in rows.EnumerateArray())
                    {
                        if (row.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String)
                            ids.Add(id.GetString() ?? string.Empty);
                    }
                }
                NightWatchOperationsCatalogLoader.ValidateWorldLocations(catalog, ids, errors);
            }
            catch (Exception ex)
            {
                errors.Add("locations.json could not be validated: " + ex.Message);
            }
        }

        private void OnNightWatchPerimeterEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName)) return;
            if (!eventName.StartsWith("watch.", StringComparison.Ordinal) &&
                !string.Equals(eventName, "acoustic_contact", StringComparison.Ordinal)) return;
            _nightWatchDirty = true;
            _perimeterDefenseDirty = true;
            // Readiness snapshots are derived presentation state. The panel
            // evaluates them during RefreshView; requesting another refresh
            // here would recurse while the panel is being opened.
            if (!string.Equals(eventName, "watch.readiness_evaluated", StringComparison.Ordinal))
                _nightWatch?.RequestPresentationRefresh();
        }

        private void OnNightWatchAcousticEstimate(Ashfall.Core.Combat.SoundRangingThreatEngine.AcousticThreatEstimate estimate)
        {
            _nightWatch?.RecordAcousticContact(estimate, CurrentWatchDay());
        }

        public void TickNightWatch(int day, bool severeWeather)
        {
            SetupNightWatch();
            _nightWatch?.TickDay(day, severeWeather);
            if (_nightWatchDirty) SaveNightWatch();
        }

        public void SaveNightWatch()
        {
            if (_nightWatch == null) return;
            // The watch has no parallel save section. Its posts/routes/drills
            // ride the canonical perimeter save, shifts ride duty_roster, and
            // gate actions ride shelter_security.
            _perimeterDefenseDirty = true;
            _dutyRosterDirty = true;
            _shelterSecurityDirty = true;
            SavePerimeterDefense();
            SaveDutyRoster();
            SaveShelterSecurity();
            _nightWatchDirty = false;
        }

        public void FlushNightWatchIfDirty()
        {
            if (_nightWatchDirty) SaveNightWatch();
        }

        public void SetupNightWatchPanel()
        {
            if (_nightWatchPanel != null && _nightWatchPanel.IsInsideTree()) return;
            SetupNightWatch();
            if (_nightWatch == null) return;
            _nightWatchPanel = new NightWatchPanel();
            _nightWatchPanel.Bind(_nightWatch);
            _nightWatchPanel.OnClose += () => _nightWatchPanel!.Visible = false;
            _nightWatchPanel.Visible = false;
            AddChild(_nightWatchPanel);
        }

        public void ShowNightWatchPanel()
        {
            SetupNightWatchPanel();
            if (_nightWatchPanel == null) return;
            _nightWatchPanel.Visible = true;
            _nightWatchPanel.RefreshView();
        }

        private int CurrentWatchDay()
        {
            if (_campaignDay?.Calendar != null) return _campaignDay.Calendar.CurrentDay;
            return Math.Max(0, _simDay);
        }

        private void ResetNightWatch()
        {
            if (_perimeterDefense != null)
                _perimeterDefense.OnEventRaised -= OnNightWatchPerimeterEvent;
            if (_soundRanging != null)
                _soundRanging.OnThreatEstimate -= OnNightWatchAcousticEstimate;
            if (_nightWatchPanel != null)
            {
                _nightWatchPanel.Unbind();
                if (_nightWatchPanel.IsInsideTree()) RemoveChild(_nightWatchPanel);
                _nightWatchPanel = null;
            }
            _nightWatch?.Dispose();
            _nightWatch = null;
            _nightWatchDirty = false;
        }
    }
}
