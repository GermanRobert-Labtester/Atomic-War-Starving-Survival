// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 210 — Waste Management & Sanitation host wire
// Subsystems   : facility catalog, room registration (tag→role), daily
//                population feed, compost→inventory delivery, save section.
// ============================================================================
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SanitationHostSession? _sanitation;
        private bool _sanitationDirty;

        public SanitationHostSession EnsureSanitationSession()
        {
            SetupSanitation();
            return _sanitation!;
        }

        private void SetupSanitation()
        {
            if (_sanitation != null)
            {
                // Plan 210 follow-up — feed the live power grid every visit;
                // the provider is evaluated at facility-tick time, so late
                // grid composition is picked up without re-binding.
                if (_sanitation.System.RoomPowerProvider == null)
                    _sanitation.System.RoomPowerProvider = roomId =>
                        _powerGrid?.System != null && _powerGrid.System.IsRoomPowered(roomId);
                return;
            }

            _sanitation = SanitationHostSession.Create(_dataDir);
            _sanitation.System.RoomPowerProvider = roomId =>
                _powerGrid?.System != null && _powerGrid.System.IsRoomPowered(roomId);
            _sanitation.StateChanged += () =>
            {
                _sanitationDirty = true;
                _economyPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };

            // Register the authored rooms once (EnsureRoom is idempotent).
            // Role mapping follows the canonical shelter_rooms.json tags.
            var rooms = ShelterRoomCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var room in rooms.rooms)
            {
                if (room == null || string.IsNullOrEmpty(room.id)) continue;
                _sanitation.System.EnsureRoom(room.id, RoleForRoom(room));
            }

            // Compost output lands in the canonical inventory authority.
            _sanitation.System.OnCompostReady += DeliverCompostOutput;

            var saved = SanitationSaveStore.TryLoad();
            if (saved != null)
            {
                _sanitation.System.RestoreState(saved);
                _sanitationDirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Sanitation state restored.");
            }
        }

        private static RoomWasteRole RoleForRoom(ShelterRoomDef room)
        {
            var tags = room.tags ?? new System.Collections.Generic.List<string>();
            if (tags.Contains("residential")) return RoomWasteRole.Residential;
            if (tags.Contains("canteen") || tags.Contains("nutrition")) return RoomWasteRole.FoodPrep;
            if (tags.Contains("heavy_industrial") || tags.Contains("crafting") || tags.Contains("power")) return RoomWasteRole.Industrial;
            if (tags.Contains("medical") || tags.Contains("triage") || tags.Contains("surgery") || tags.Contains("quarantine")) return RoomWasteRole.Medical;
            return RoomWasteRole.Other;
        }

        private void DeliverCompostOutput(CompostBatchState batch)
        {
            if (batch == null || string.IsNullOrEmpty(batch.outputItemId) || batch.outputUnits <= 0f) return;
            var inv = _inventory?.Inventory;
            if (inv == null) return;
            int units = (int)Math.Floor(batch.outputUnits);
            if (units <= 0) return;
            if (inv.AddById(batch.outputItemId, units))
            {
                GD.Print($"[Ashfall Godot] Compost delivered: {units} × {batch.outputItemId}");
            }
        }

        private void SaveSanitation()
        {
            if (_sanitation == null) return;
            CaptureSection("sanitation", SanitationSaveStore.TryCapturePersisted(_sanitation.CaptureSave()));
        }

        private void FlushSanitationIfDirty()
        {
            if (_sanitationDirty) SaveSanitation();
        }

        // ── Panels (Plan 210 Phase 9): created hidden; opened via the
        //    expanded-panel route. Presentation only.
        private UI.SanitationPanel? _sanitationPanel;

        private void EnsureSanitationPanel()
        {
            if (_sanitation == null) return;
            if (_sanitationPanel != null) return;
            _sanitationPanel = new UI.SanitationPanel();
            _sanitationPanel.Bind(_sanitation);
            _sanitationPanel.Visible = false;
            AddChild(_sanitationPanel);
        }

        private void OpenSanitationPanel()
        {
            SetupSanitation();
            EnsureSanitationPanel();
            if (_sanitationPanel != null) { _sanitationPanel.Visible = true; _sanitationPanel.RefreshView(); }
        }
    }
}
