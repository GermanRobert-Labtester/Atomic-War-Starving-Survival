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
        private void SetupPhantom()
        {
            if (_phantomMemory != null) return;
            _phantomMemory = PhantomMemoryHostSession.Create(_dataDir);
            _phantomMemory.StateChanged += () => SavePhantomMemory();

            var save = PhantomMemorySaveStore.TryLoad();
            if (save != null)
            {
                _phantomMemory.RestoreSave(save);
                GD.Print("[Ashfall Godot] Phantom Memory state restored.");
            }
        }

        private void OnPhantomScavengeClicked()
        {
            SetupPhantom();
            _statusLabel.Text = _phantomMemory.ScavengeItem("survivor_gunner_mikhail", "armour_heavy_military");
        }

        private void OnPhantomTickClicked()
        {
            SetupPhantom();
            _statusLabel.Text = _phantomMemory.TickDemo();
        }

        private void SavePhantomMemory()
        {
            if (_phantomMemory == null) return;
            if (PhantomMemorySaveStore.TrySave(_phantomMemory.CaptureSave()))
                GD.Print("[Ashfall Godot] Phantom Memory save written.");
        }

        private void SetupPhase0()
        {
            if (_phase0 != null) return;
            _phase0 = new Phase0HostSession();
            _phase0.StateChanged += () => _phase0Dirty = true;

            // ── Wire every Phase-0 effect to the REAL gameplay consumer ──
            SetupSurvivors();
            SetupJournal();
            SetupCrafting();
            SetupExpeditions();
            SetupMedical();

            _phase0.Consumers.ApplyMoraleDelta = (sv, delta) =>
            {
                var survivor = _survivors.Find(sv);
                if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
            };
            _phase0.Consumers.ApplyHealthDelta = (sv, delta) =>
            {
                var survivor = _survivors.Find(sv);
                if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Health, delta);
            };
            _phase0.Consumers.ApplyFatigueDelta = (sv, delta) =>
            {
                var survivor = _survivors.Find(sv);
                if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Fatigue, delta);
            };
            // Work efficiency + chemical crafting penalty compose into the real
            // CraftingSystem craft-time multiplier.
            _phase0.Consumers.ApplyWorkEfficiencyMultiplier = (sv, mult) =>
            {
                if (_crafting == null) return;
                _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
                    id == sv ? MathfCompat.Max(0.1f, 1f / MathfCompat.Max(0.1f, mult)) : 1f);
            };
            _phase0.Consumers.ApplyCraftingPenaltyFactor = (sv, factor) =>
            {
                if (_crafting == null) return;
                _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
                    id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
            };
            // Chemical combat penalty feeds the expedition encounter/failure risk by
            // draining stamina faster (tremor). Also exposed via ApplyStaminaDrainMultiplier.
            _phase0.Consumers.ApplyCombatPenaltyFactor = (sv, factor) =>
            {
                if (_expeditions == null) return;
                _expeditions.Engine.SetStaminaDrainMultiplier(id =>
                    id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
            };
            // Respiratory severe cough raises expedition stamina drain.
            _phase0.Consumers.ApplyStaminaDrainMultiplier = (sv, factor) =>
            {
                if (_expeditions == null) return;
                _expeditions.Engine.SetStaminaDrainMultiplier(id =>
                    id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
            };
            // Shelter-wide morale deltas (final wish / moral branching) reach every
            // alive survivor's morale via the authoritative NeedsSystem.
            _phase0.Consumers.ApplyShelterMoraleDelta = delta =>
            {
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s != null && s.IsAliveState)
                        _survivors.Needs.Modify(s, NeedKind.Morale, delta);
                }
            };
            _phase0.Consumers.FireNarrativeEvent = (narrativeId, sv) =>
            {
                int day = _holdfastRuntime?.Day ?? _simDay;
                _journal.TryAddRawEntry(
                    $"{narrativeId}_{sv}_{day}",
                    $"{sv}: {narrativeId.Replace('_', ' ')}.",
                    author: null!,
                    day: day);
            };
            _phase0.Consumers.GrantChronicIllness = (sv, afflictionId) =>
            {
                var rad = _survivors.RadStateFor(sv);
                if (rad != null && !rad.HasChronicIllness)
                {
                    rad.HasChronicIllness = true;
                    SaveSurvivors();
                }
            };
            _phase0.Consumers.ResetRadiationDose = sv =>
            {
                var rad = _survivors.RadStateFor(sv);
                if (rad != null) _survivors.Radiation.SetDose(rad, 0f);
            };

            // Environment signals from the real world/shelter hosts.
            _phase0.CurrentDay = _holdfastRuntime?.Day ?? _simDay;
            _phase0.GetFilterHealth = () =>
            {
                var filter = _expansions?.Waystation?.State != null
                    ? _expansions.Waystation.State.filterHealth : 100f;
                return filter;
            };
            // Host flags: updated each tick from the real world/shelter state.
            _phase0.IsInFalloutStorm = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
            _phase0.IsNightTime = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.BlackRain;

            var ids = new System.Collections.Generic.List<string>();
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var s = _survivors.RosterState[i];
                if (s != null && s.IsAliveState) ids.Add(s.Id);
            }
            _phase0.RegisterSurvivors(ids);

            var save = Phase0SaveStore.TryLoad();
            if (save != null)
            {
                _phase0.RestoreSave(save);
                _phase0Dirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Phase-0 effects restored.");
            }
        }

        private void SavePhase0()
        {
            if (_phase0 == null) return;
            if (Phase0SaveStore.TrySave(_phase0.CaptureSave()))
            {
                _phase0Dirty = false;
                GD.Print("[Ashfall Godot] Phase-0 effects save written.");
            }
        }

        private void FlushPhase0IfDirty()
        {
            if (_phase0Dirty) SavePhase0();
        }

        private void OnPhase0ScavengeClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
        }

        private void OnPhase0NoiseClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.RaiseNoise("siren");
        }

        private void OnPhase0CraftClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.CraftItem("elena_vasquez", "machinist", "wrench_standard");
        }

        private void OnPhase0TickClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.TickHour(6f);
        }

        private void SetupDoseLedger()
        {
            if (_doseLedger != null) return;
            _doseLedger = DoseLedgerHostSession.Create(_dataDir);
            _doseLedger.StateChanged += () => _doseLedgerDirty = true;

            var save = DoseLedgerSaveStore.TryLoad();
            if (save != null)
            {
                _doseLedger.RestoreSave(save);
                _doseLedgerDirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Dose Ledger state restored.");
            }

            if (_doseSurface == null && _rightColumn != null)
            {
                _doseSurface = new DoseRegisterSurface();
                _rightColumn.AddChild(_doseSurface);
            }
            if (_doseSurface != null)
            {
                _doseSurface.BindSession(_doseLedger);
                _doseSurface.RefreshView();
            }
        }

        private void OnDoseRegisterClicked()
        {
            SetupDoseLedger();
            _statusLabel.Text = "The Dose Register is open. Four tabs, four people who keep books.";
        }

        private void OnDoseSealClicked()
        {
            SetupDoseLedger();
            _doseLedger.SealDemoSurvivors();
            _statusLabel.Text = "Dosimeters sealed: Gunner Mikhail (tag_1), Elena Vasquez (tag_2).";
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseScribeClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.ScribeReading(180f, highEnergy: false);
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseDiagnoseClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed);
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseCohortClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.BookDemoChild();
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseVolunteerClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.SignDemoVolunteer();
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void SaveDoseLedger()
        {
            if (_doseLedger == null) return;
            int day = _core != null ? _core.Clock.Day : _simDay;
            if (DoseLedgerSaveStore.TrySave(_doseLedger.CaptureSave(day)))
            {
                _doseLedgerDirty = false;
                GD.Print($"[Ashfall Godot] Dose Ledger save written (day {day}).");
            }
        }

        private void FlushDoseLedgerIfDirty()
        {
            if (_doseLedgerDirty) SaveDoseLedger();
        }

        private void ClosePhase0Panel()
        {
            _phase0Panel.Visible = false;
        }

    }
}
