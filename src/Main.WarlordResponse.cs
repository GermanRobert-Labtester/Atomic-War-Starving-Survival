// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Warlord Tribute Responses — host wiring over the sealed
// Ashfall.Core.Warlords.WarlordResponseActions and the LIVE
// WarlordDoctrineSystem held by YearOfAshHostSession.
//
// Before this wiring, Main.YearOfAsh called SettleWarlordTribute directly with
// no responded-tribute guard, so the same week's ask could be settled (and the
// tribute currency consumed) repeatedly. Every response now goes through the
// idempotent command surface exactly once.
// ============================================================================

using System;
using Ashfall.Core.Warlords;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WarlordResponseHostSession? _warlordResponse;
        private bool _warlordResponseDirty;

        public WarlordResponseHostSession? WarlordResponse => _warlordResponse;

        public void SetupWarlordResponse()
        {
            if (_warlordResponse != null) return;

            var saved = WarlordResponseSaveStore.TryLoad();
            _warlordResponse = new WarlordResponseHostSession(
                () => _yearOfAsh?.Warlord,
                saved);
            _warlordResponse.StateChanged += () => _warlordResponseDirty = true;
        }

        /// <summary>
        /// Records a tribute payment exactly once per canonical tribute id, then
        /// settles the ask through the canonical doctrine owner and consumes the
        /// tribute currency through the canonical inventory. Refusals never mutate.
        /// </summary>
        public string PayWarlordTributeGuarded(int amount)
        {
            SetupWarlordResponse();
            if (_warlordResponse == null || _yearOfAsh?.Warlord == null || _holdfastRuntime?.Trade.Inventory == null)
                return "warlord owner unavailable";

            int day = _core?.Clock.Day ?? 1;
            if (amount <= 0) return "there is nothing to hand over this week.";
            if (_warlordResponse.HasRespondedToCurrentAsk())
                return "The collector already has this week's answer. He will be back.";

            var inventory = _holdfastRuntime.Trade.Inventory;
            string item = _yearOfAsh.Warlord.Catalog.Warlord.tribute_currency_item;
            if (!inventory.Items.TryGetValue(item, out int held) || held < amount)
                return $"The collector waits. You do not have {amount}\u00d7 {item} to hand over.";

            // Record first: a failed settlement leaves the response recorded rather
            // than silently re-offering the same ask, and the currency is untouched.
            var response = _warlordResponse.Pay(amount, day);
            if (response == null || !response.Succeeded)
                return $"The collector will not take that ({response?.ReasonCode ?? "refused"}).";

            inventory.RemoveItem(item, amount);
            int next;
            bool full = _yearOfAsh.SettleWarlordTribute(amount, day, out next);
            string line = _yearOfAsh.CollectorLine(full ? "paid" : "short", day);
            _yearOfAshDirty = true;
            return line;
        }

        /// <summary>Refusal is a response too — recorded exactly once per ask.</summary>
        public string RefuseWarlordTributeGuarded()
        {
            SetupWarlordResponse();
            if (_warlordResponse == null || _yearOfAsh?.Warlord == null)
                return "warlord owner unavailable";

            int day = _core?.Clock.Day ?? 1;
            if (_warlordResponse.HasRespondedToCurrentAsk())
                return "The collector already has this week's answer. He will be back.";

            var response = _warlordResponse.Contest(day);
            if (response == null || !response.Succeeded)
                return $"The collector will not accept that ({response?.ReasonCode ?? "refused"}).";

            int next;
            _yearOfAsh.SettleWarlordTribute(0, day, out next);
            string line = _yearOfAsh.CollectorLine("refused", day);
            _yearOfAshDirty = true;
            return line;
        }

        /// <summary>Submission is the third authored response: hand it all over.</summary>
        public string SubmitToWarlordGuarded()
        {
            SetupWarlordResponse();
            if (_warlordResponse == null || _yearOfAsh?.Warlord == null || _holdfastRuntime?.Trade.Inventory == null)
                return "warlord owner unavailable";

            int day = _core?.Clock.Day ?? 1;
            if (_warlordResponse.HasRespondedToCurrentAsk())
                return "The collector already has this week's answer. He will be back.";

            var inventory = _holdfastRuntime.Trade.Inventory;
            string item = _yearOfAsh.Warlord.Catalog.Warlord.tribute_currency_item;
            int held = inventory.Items.TryGetValue(item, out int onHand) ? onHand : 0;

            var response = _warlordResponse.Submit(day);
            if (response == null || !response.Succeeded)
                return $"The collector will not accept that ({response?.ReasonCode ?? "refused"}).";

            if (held > 0) inventory.RemoveItem(item, held);
            int next;
            _yearOfAsh.SettleWarlordTribute(Math.Max(1, held), day, out next);
            string line = _yearOfAsh.CollectorLine("paid", day);
            _yearOfAshDirty = true;
            return line;
        }

        public string WarlordResponseStatusLine() =>
            _warlordResponse?.StatusLine() ?? "awaiting response";

        public void SaveWarlordResponse()
        {
            if (_warlordResponse == null) return;
            var state = _warlordResponse.CaptureState();
            WarlordResponseSaveStore.TrySave(state);
            if (CaptureSection(WarlordResponseSaveStore.SectionName, WarlordResponseSaveStore.TryCapturePersisted(state)))
                _warlordResponseDirty = false;
        }

        public void FlushWarlordResponseIfDirty()
        {
            if (_warlordResponseDirty) SaveWarlordResponse();
        }

        public void ResetWarlordResponse()
        {
            _warlordResponse = null;
            _warlordResponseDirty = false;
        }
    }
}
