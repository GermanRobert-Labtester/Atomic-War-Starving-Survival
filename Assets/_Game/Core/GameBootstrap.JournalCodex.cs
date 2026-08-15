using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Journal codex wiring (docs/ui/JOURNAL_UI_PLAN.md): builds the JournalCodex
    /// view-model from the catalogs + knowledge base, binds it to the book, and
    /// subscribes the four unlock hooks (item grant, expedition return, survivor
    /// met, event fired). Unlocks are idempotent and save/load safe via
    /// KnowledgeBase, so wiring can run again on load without double-firing.
    /// </summary>
    public partial class GameBootstrap
    {
        private JournalCodex _journalCodex;

        /// <summary>
        /// Build the codex + bind unlock hooks. Safe to run once; called from
        /// WireHUD after EnsureJournalBook. All subscriptions are tracked for
        /// teardown via _subscriptions.
        /// </summary>
        private void WireJournalCodex()
        {
            if (JournalSystem == null || _hud == null) return;

            _journalCodex = new JournalCodex(
                JournalSystem,
                _itemCatalog,
                _locationCatalog,
                _eventCatalog,
                () => Survivors,
                _survivorCatalog);

            var book = _hud.EnsureJournalBook();
            if (book != null)
            {
                book.SetCodexProvider(tab => _journalCodex.BuildRows(tab));
                book.SetUnreadProvider(tab => JournalSystem.HasUnreadForTab(tab));
                book.SwitchTab(JournalSystem.ActiveTab);
                JournalSystem.OnTabChanged -= HandleJournalTabChanged;
                JournalSystem.OnTabChanged += HandleJournalTabChanged;
                _subscriptions.Track(() => JournalSystem.OnTabChanged -= HandleJournalTabChanged);

                // Live refresh while the book is open on a codex tab.
                JournalSystem.OnCodexUnlocked -= HandleJournalCodexUnlocked;
                JournalSystem.OnCodexUnlocked += HandleJournalCodexUnlocked;
                _subscriptions.Track(() => JournalSystem.OnCodexUnlocked -= HandleJournalCodexUnlocked);
            }

            WireJournalCodexUnlocks();
            BackfillExistingUnlocks();
        }

        /// <summary>
        /// Survivors created before JournalSystem existed (the starting crew in
        /// InitFoundation) and items granted in the same window never fired the
        /// hooks. Unlock them now — idempotent, so load-game backfill is safe.
        /// </summary>
        private void BackfillExistingUnlocks()
        {
            if (Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                    UnlockSurvivorMet(Survivors[i]);
            }
            if (Inventory != null && Inventory.Slots != null)
            {
                for (int i = 0; i < Inventory.Slots.Count; i++)
                {
                    var slot = Inventory.Slots[i];
                    if (slot != null && slot.Item != null && !string.IsNullOrEmpty(slot.Item.id))
                        JournalSystem?.UnlockItemSeen(slot.Item.id);
                }
            }
        }

        /// <summary>Keep the book's tab in sync when the system switches tabs.</summary>
        private void HandleJournalTabChanged(int tab)
        {
            var book = _hud != null ? _hud.EnsureJournalBook() : null;
            if (book != null && book.ActiveTab != tab)
                book.SwitchTab(tab);
        }

        /// <summary>Repaint the open book so a fresh unlock row appears immediately.</summary>
        private void HandleJournalCodexUnlocked(string key)
        {
            var book = _hud != null ? _hud.EnsureJournalBook() : null;
            book?.Refresh();
        }

        /// <summary>Player input: [1]-[5] switch the open journal's tab.</summary>
        public void SwitchJournalTab(int tab)
        {
            if (JournalSystem == null) return;
            JournalSystem.SwitchTab(tab);
            var book = _hud != null ? _hud.EnsureJournalBook() : null;
            if (book != null)
                book.SwitchTab(JournalSystem.ActiveTab);
        }

        // -----------------------------------------------------------------
        // Unlock hooks — idempotent via KnowledgeBase, so repeated wiring
        // (load-game hot path) cannot double-fire journal entries.
        // -----------------------------------------------------------------

        private void WireJournalCodexUnlocks()
        {
            if (Inventory != null)
            {
                Inventory.OnItemAdded -= HandleJournalItemAdded;
                Inventory.OnItemAdded += HandleJournalItemAdded;
                _subscriptions.Track(() => Inventory.OnItemAdded -= HandleJournalItemAdded);
            }

            if (ExpeditionSystem != null)
            {
                ExpeditionSystem.OnExpeditionCompleted -= HandleJournalExpeditionCompleted;
                ExpeditionSystem.OnExpeditionCompleted += HandleJournalExpeditionCompleted;
                _subscriptions.Track(() => ExpeditionSystem.OnExpeditionCompleted -= HandleJournalExpeditionCompleted);
            }

            if (EventRunner != null)
            {
                EventRunner.OnEventTriggered -= HandleJournalEventTriggered;
                EventRunner.OnEventTriggered += HandleJournalEventTriggered;
                _subscriptions.Track(() => EventRunner.OnEventTriggered -= HandleJournalEventTriggered);

                // Day-keyed narrative events fire through a separate channel and
                // may not carry a resolved GameEvent (pool miss) — unlock by the
                // scheduled id so the EVENTS tab still records the moment.
                EventRunner.OnScheduledEventFired -= HandleJournalScheduledEventFired;
                EventRunner.OnScheduledEventFired += HandleJournalScheduledEventFired;
                _subscriptions.Track(() => EventRunner.OnScheduledEventFired -= HandleJournalScheduledEventFired);
            }
        }

        private void HandleJournalItemAdded(ItemDefinition item, int amount)
        {
            if (item == null || string.IsNullOrEmpty(item.id)) return;
            JournalSystem?.UnlockItemSeen(item.id);
        }

        private void HandleJournalExpeditionCompleted(ExpeditionState state, List<ItemDefinition> loot)
        {
            if (state == null || string.IsNullOrEmpty(state.TargetLocationId)) return;
            // Only a real return logs the place; deaths/captures do not.
            if (state.Phase != ExpeditionPhase.Completed) return;
            JournalSystem?.UnlockLocationVisited(state.TargetLocationId);
        }

        private void HandleJournalEventTriggered(GameEvent evt, EventContext context)
        {
            if (evt == null || string.IsNullOrEmpty(evt.id)) return;
            JournalSystem?.UnlockEventFired(evt.id);
        }

        private void HandleJournalScheduledEventFired(ScheduledEvent scheduled, GameEvent evt, EventContext context)
        {
            if (evt != null && !string.IsNullOrEmpty(evt.id))
            {
                JournalSystem?.UnlockEventFired(evt.id);
                return;
            }
            if (!string.IsNullOrEmpty(scheduled.EventId))
                JournalSystem?.UnlockEventFired(scheduled.EventId);
        }

        /// <summary>Call at every survivor-add site so recruits unlock their dossier.</summary>
        private void UnlockSurvivorMet(Survivor sv)
        {
            if (sv == null) return;
            string key = string.IsNullOrEmpty(sv.ArchetypeId) ? sv.Id : sv.ArchetypeId;
            JournalSystem?.UnlockSurvivorMet(key);
        }
    }
}
