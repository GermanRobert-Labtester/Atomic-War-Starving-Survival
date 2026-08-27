using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Maritime fields (GAP-ARCH-01 Phase 1) ──
        private MaritimeHostSession _maritime = null!;
        private bool _maritimeDirty;
        private DeepCoastHostSession _deepCoast = null!;

        private void FlushMaritimeIfDirty()
        {
            if (_maritimeDirty) SaveMaritime();
        }

        /// <summary>
        /// Thin host wiring: shares the CoreDemoSession's District8DeepCoastSystem
        /// (so the HoldfastSave v5 envelope is the single authority), the real
        /// journal, the maritime dive session, and the Holdfast trade inventory.
        /// Also registers the existing Northern Sound Icebreaker Dock as an
        /// expedition target the moment the route reaches dock_accessible — the
        /// route gate (IsNodeAccessible) stays the enforcement, so the dock can
        /// never be dispatched before it is reached.
        /// </summary>
        private void SetupDeepCoast()
        {
            if (_deepCoast != null) return;
            SetupIceRoad();
            SetupJournal();
            SetupMaritime();
            SetupHoldfastRuntime(); // canonical Holdfast trade inventory for the route bills
            _deepCoast = DeepCoastHostSession.Create(
                _core.DeepCoast,
                _journal,
                null!,
                _holdfastRuntime!.Trade.Inventory!,
                _maritime!);
            // Seasonal (Ice Road) + route-stage gate for expedition dispatch.
            if (_expeditions != null)
            {
                _expeditions.ExtraBlocked = locationId =>
                    _core.IceRoad.IsTravelBlocked(locationId)
                    || _deepCoast.IsRouteNodeBlocked(locationId);
            }
            _deepCoast.DeepCoast.OnStateChanged += () =>
            {
                _holdfastDirty = true; // deep-coast state rides in the Holdfast v5 save
                RefreshDeepCoastDockTarget();
            };
            RefreshDeepCoastDockTarget();
            GD.Print("[Ashfall Godot] Deep coast host ready: District 8 route beyond the Shelf.");
        }

        private void RefreshDeepCoastDockTarget()
        {
            if (_deepCoast == null) return;

            // Route-node expedition targets. The breakwater is always offered
            // (the survey trip is the first expedition); everything beyond the
            // boom registers only when its stage opens, so the UI can never
            // dispatch past the route gate.
            RegisterDeepCoastTarget(District8DeepCoastSystem.PerimeterBreakwaterId,
                "The Perimeter Breakwater", 13, 8, 3.0f, true);
            RegisterDeepCoastTarget(District8DeepCoastSystem.ServiceChannelId,
                "The Flooded Service Channel", 14, 8, 3.2f,
                _deepCoast.DeepCoast.IsNodeAccessible(District8DeepCoastSystem.ServiceChannelId));
            RegisterDeepCoastTarget(District8DeepCoastSystem.DeepBerthId,
                "The Deep Berth", 15, 9, 3.5f,
                _deepCoast.DeepCoast.IsNodeAccessible(District8DeepCoastSystem.DeepBerthId));
            RegisterDeepCoastTarget(District8DeepCoastSystem.DockId,
                "Northern Sound Icebreaker Dock", 16, 9, 3.5f,
                _deepCoast.DockExpeditionAvailable);
        }

        private static void RegisterDeepCoastTarget(string id, string displayName, int ticks, int danger, float drain, bool available)
        {
            if (!available) return;
            if (ExpeditionDefinitionRegistry.Get(id) != null) return;
            ExpeditionDefinitionRegistry.Register(new ExpeditionDefinition
            {
                id = id,
                displayName = displayName,
                distanceTicks = ticks,
                dangerLevel = danger,
                encounterChancePerTick = 0.18f,
                baseStaminaDrainPerHour = drain,
                lootCategories = new System.Collections.Generic.List<string>
                    { "scrap_metal", "brass_fittings", "canned_food" }
            });
        }

        private void SetupMaritime()
        {
            if (_maritime != null) return;
            _maritime = MaritimeHostSession.Create(_dataDir);
            _maritime.StateChanged += () => _maritimeDirty = true;
            GD.Print("[Ashfall Godot] Maritime host ready: stealth dive · scavenge · contamination.");
        }

        private void SaveMaritime()
        {
            if (_maritime == null) return;
            if (CaptureSection("maritime", MaritimeSaveStore.TryCapturePersisted(_maritime.CaptureSave())))
            {
                _maritimeDirty = false;
                GD.Print("[Ashfall Godot] Maritime save written.");
            }
        }

        private void OnMaritimeStartDiveClicked()
        {
            SetupMaritime();
            _statusLabel.Text = _maritime.StartDiveDemo("diver_cole", "operator_ren");
        }

        private void OnMaritimeTickDiveClicked()
        {
            SetupMaritime();
            _statusLabel.Text = _maritime.TickDiveDemo(10f);
        }

        private void OnMaritimeScavengeClicked()
        {
            SetupMaritime();
            _statusLabel.Text = _maritime.ScavengeDemo("location_stadium_evacuation_center");
        }

        private void OnMaritimeContaminateClicked()
        {
            SetupMaritime();
            _statusLabel.Text = _maritime.ContaminateDemo("survivor_gunner_mikhail", "location_automated_abattoir");
        }

        private void CloseMaritimePanel()
        {
            if (_maritimePanel != null) _maritimePanel.Visible = false;
        }

        private void CloseDeepCoastPanel()
        {
            if (_deepCoastPanel != null) _deepCoastPanel.Visible = false;
        }

    }
}
