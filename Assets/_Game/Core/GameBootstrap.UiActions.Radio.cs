using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        public void ToggleJournalBook()
        {
            var book = _hud?.EnsureJournalBook();
            if (book == null) return;
            book.Toggle();
            if (JournalSystem != null)
            {
                JournalSystem.HudIsOpen = book.IsOpen;
                if (book.IsOpen)
                {
                    JournalSystem.MarkRead();
                    JournalSystem.MarkTabViewed(JournalSystem.ActiveTab);
                }
            }
        }

        /// <summary>Open journal book and clear unread / ping.</summary>
        public void OpenJournalBook()
        {
            var book = _hud?.EnsureJournalBook();
            book?.Open();
            if (JournalSystem != null)
            {
                JournalSystem.HudIsOpen = true;
                JournalSystem.MarkRead();
                JournalSystem.MarkTabViewed(JournalSystem.ActiveTab);
            }
        }

        /// <summary>
        /// Copy live radio strip presentation into the intercept system so
        /// SaveSystem.CaptureState persists open / unread / tuner index.
        /// </summary>
        public void SnapshotRadioHudToInterceptSystem()
        {
            if (FactionRadioIntercepts == null) return;
            var strip = _hud != null ? _hud.EnsureRadioInterceptHud() : null;
            if (strip == null) return;
            FactionRadioIntercepts.HudIsOpen = strip.IsOpen;
            FactionRadioIntercepts.HudHasUnread = strip.HasUnread;
            FactionRadioIntercepts.HudTunerIndex = strip.TunerIndex;
        }

        public void ConsumeItem(Survivor sv, ItemDefinition item)
        {
            if (sv == null || item == null || !sv.IsAlive) return;

            // Prompt #833 — peek tolerance effectiveness before applying effects
            // (scales how much dose/consume-strength Inventory.Consume applies).
            float therapeuticScale = 1f;
            if (!string.IsNullOrEmpty(item.id) && ChemUseRouter.IsToleranceChem(item.id))
            {
                therapeuticScale = ChemUse?.PeekEffectiveness(sv, item.id) ?? 1f;
            }

            if (Inventory == null
                || !Inventory.Consume(item, sv, RadiationSystem, NeedsSystem, therapeuticScale))
                return;

            // Temporary rad-resistance is granted ONLY by iodine, via
            // AdministerIodine (Inventory.Consume routes iodine_pills -> it),
            // per the radiation contract in RadiationSystem.cs:
            //   "Iodine grants a timed RadResistance status; anti-rad reduces
            //    the current dose directly."
            // A previous block here re-granted RadResistance to ANY tolerance
            // chem with radCleanse > 0 — in practice only anti_rad (items.json:
            // radCleanse 50) — for up to 24h, while iodine is NOT a tolerance
            // chem and never entered this branch (it gets only 6h via
            // AdministerIodine). That made anti_rad strictly dominate iodine at
            // iodine's own job, so the block was removed.

            // Prompt #13 — poisoned iodine looks clean until swallowed.
            SabotagedCacheSystem?.TryApplyPoisonOnConsume(item, sv, MedicalSystem);

            // Direct inventory use: addiction + blood toxicity + polypharmacy + tolerance.
            if (!string.IsNullOrEmpty(item.id))
                ChemUse?.Notify(sv, item.id);
        }

        public void CraftRecipe(Recipe recipe)
        {
            if (recipe == null) return;
            CraftingSystem.StartCraft(recipe);
        }

        public void SelectEventChoice(int choiceIndex)
        {
            // Applies to the most recently triggered event context
            if (EventRunner.ActiveConsequences.Count > 0 || EventRunner.Pool.Count > 0)
            {
                // EventModalUI handles this via its own Bind
            }
        }

        /// <summary>Open the wasteland map screen (UI).</summary>
        public void OpenMapScreen()
        {
            _hud?.MapScreenUI?.Open();
        }

        /// <summary>Open the workbench disassembly / repair / hatch-install screen.</summary>
        public void OpenWorkbench()
        {
            _hud?.WorkbenchUI?.Open();
        }

        /// <summary>Toggle workbench panel (keybind B).</summary>
        public void ToggleWorkbench()
        {
            _hud?.WorkbenchUI?.Toggle();
        }

        /// <summary>Open hatch defense status panel.</summary>
        public void OpenHatchDefense()
        {
            _hud?.HatchDefenseHUD?.Open();
        }

        /// <summary>Toggle hatch defense panel (keybind H).</summary>
        public void ToggleHatchDefense()
        {
            _hud?.HatchDefenseHUD?.Toggle();
        }

        /// <summary>Open the expanded radio intercept log.</summary>
        public void OpenRadioInterceptLog()
        {
            _hud?.EnsureRadioInterceptHud()?.Open();
        }

        /// <summary>Toggle expanded radio intercept log (keybind R).</summary>
        public void ToggleRadioInterceptLog()
        {
            _hud?.EnsureRadioInterceptHud()?.Toggle();
        }

        /// <summary>Cycle radio frequency filter forward (keybind ]).</summary>
        public void CycleRadioTunerNext()
        {
            _hud?.EnsureRadioInterceptHud()?.CycleTunerNext();
        }

        /// <summary>Cycle radio frequency filter backward (keybind [).</summary>
        public void CycleRadioTunerPrev()
        {
            _hud?.EnsureRadioInterceptHud()?.CycleTunerPrev();
        }

        private void PushRadioInterceptToHud(FactionRadioInterceptSystem.InterceptEntry entry)
        {
            if (entry == null || _hud == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            strip?.Push(entry.Message, entry.Kind, entry.FactionId, entry.Day);
        }

        private void PushJournalEntryToHud(JournalEntry entry)
        {
            if (entry == null || _hud == null) return;
            var book = _hud.EnsureJournalBook();
            book?.Push(entry);
        }

        /// <summary>Rebuild journal book from JournalSystem (WireHUD / load).</summary>
        public void SyncJournalBookFromSystem()
        {
            if (_hud == null || JournalSystem == null) return;
            var book = _hud.EnsureJournalBook();
            if (book == null) return;
            book.SetEntries(JournalSystem.Entries);
            book.ApplyUiState(
                JournalSystem.HudIsOpen,
                JournalSystem.HasUnread,
                JournalSystem.NotificationPing,
                JournalSystem.ActiveTab);
        }

        /// <summary>
        /// Bind the intercept strip dial to RadioTunerSystem frequencies so
        /// [ / ] retunes intel extraction and filters faction intercepts together.
        /// Safe to call multiple times (rebinds bands + handler).
        /// </summary>
        public void WireRadioInterceptTuner()
        {
            if (_hud == null || RadioTunerSystem == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            // Push band list (ALL + each registered frequency).
            var coreBands = RadioTunerSystem.BuildTunerBands();
            var uiBands = new System.Collections.Generic.List<RadioInterceptHUD.TunerBand>(coreBands.Count);
            for (int i = 0; i < coreBands.Count; i++)
            {
                var b = coreBands[i];
                uiBands.Add(RadioInterceptHUD.TunerBand.FromParts(
                    b.FrequencyId, b.Label, b.ChannelTag));
            }
            strip.SetTunerBands(uiBands);

            // Avoid stacking handlers if WireHUD / load re-runs.
            strip.OnTunerBandChanged -= HandleRadioHudTunerChanged;
            strip.OnTunerBandChanged += HandleRadioHudTunerChanged;
            _subscriptions.Track(() => strip.OnTunerBandChanged -= HandleRadioHudTunerChanged);
            RadioTunerSystem.OnFrequencyChanged -= HandleRadioTunerFrequencyChanged;
            RadioTunerSystem.OnFrequencyChanged += HandleRadioTunerFrequencyChanged;
            _subscriptions.Track(() => RadioTunerSystem.OnFrequencyChanged -= HandleRadioTunerFrequencyChanged);

            // Align dial with current tuner state (detuned on fresh boot).
            strip.SyncFromFrequencyId(RadioTunerSystem.State?.CurrentFrequencyId);
            PushRadioLiveStateToHud();
        }

        private void HandleRadioHudTunerChanged(string frequencyId, string channelTag)
        {
            if (RadioTunerSystem == null) return;
            if (string.IsNullOrEmpty(frequencyId))
                RadioTunerSystem.Detune();
            else
                RadioTunerSystem.TuneToFrequency(frequencyId);
        }

        private void HandleRadioTunerFrequencyChanged(string frequencyId)
        {
            if (_hud == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            // Sync HUD without re-notifying (would loop into TuneToFrequency).
            strip?.SyncFromFrequencyId(frequencyId);
            PushRadioLiveStateToHud();
        }

        private static DiaryFragmentSO CreateDefaultDiary(in DiarySeed seed)
        {
            var diary = ScriptableObject.CreateInstance<DiaryFragmentSO>();
            diary.id = seed.Id;
            diary.title = seed.Title;
            diary.text = seed.Text;
            diary.authorName = seed.Author;
            diary.foundInRoomId = seed.RoomId;
            diary.warnsAboutSystemId = seed.WarnsSystem;
            diary.pageOrder = seed.Page;
            diary.totalPages = seed.Total;
            return diary;
        }

    }
}
