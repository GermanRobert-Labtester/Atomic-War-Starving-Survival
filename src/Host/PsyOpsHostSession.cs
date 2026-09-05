// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host session over <see cref="PsyOpsSystem"/> (Flagship XI — Plan 157).
    /// Forwards player commands, carries LastEvent/StateChanged, and exposes the
    /// read model for the relay panel. No gameplay logic lives here.
    /// </summary>
    public sealed class PsyOpsHostSession : HostSessionBase
    {
        public PsyOpsSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public PsyOpsHostSession(PsyOpsSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnCampaignStarted += (campaignId, day) =>
            {
                LastEvent = $"Campaign on air: {DisplayName(campaignId)}.";
                RaiseStateChanged();
            };
            System.OnCampaignExpired += (campaignId, day) =>
            {
                LastEvent = $"The {DisplayName(campaignId)} run has ended.";
                RaiseStateChanged();
            };
            System.OnBroadcastIntercepted += (campaignId, factionId, confidence, day) =>
            {
                LastEvent = "Someone answered the broadcast. We heard them listening.";
                BroadcastIntercepted?.Invoke(campaignId, factionId, confidence, day);
                RaiseStateChanged();
            };
            System.OnJammingStarted += (factionId, strength, day) =>
            {
                LastEvent = $"Static now blankets {factionId} airtime.";
                RaiseStateChanged();
            };
            System.OnCounterPropagandaStarted += (campaignId, day) =>
            {
                LastEvent = $"Counter-programming against {DisplayName(campaignId)} is running.";
                RaiseStateChanged();
            };
        }

        private string DisplayName(string campaignId) =>
            System.CatalogDisplay(campaignId);

        /// <summary>Host-side intercept bridge (intel/narrative systems subscribe).</summary>
        public event Action<string, string, float, int>? BroadcastIntercepted;

        // ------------------------------------------------------------ commands

        public string StartCampaignById(string campaignId, int day)
        {
            bool ok = System.StartCampaign(campaignId, day);
            RaiseStateChanged();
            return ok ? $"The {DisplayName(campaignId)} broadcast is on the schedule."
                      : "That campaign cannot start (unknown, already running, or its target is taken).";
        }

        public string StartJammingOn(string factionId, float strength, int days, int day)
        {
            bool ok = System.StartJamming(factionId, strength, days, day);
            RaiseStateChanged();
            return ok ? $"Static goes out over {factionId} frequencies."
                      : "The jammers need a target, strength, and days to run.";
        }

        public string CounterPropagandaOn(string campaignId, int days, int day)
        {
            bool ok = System.StartCounterPropaganda(campaignId, days, day);
            RaiseStateChanged();
            return ok ? "Counter-programming is on the air."
                      : "No such campaign is running to counter.";
        }

        // ---------------------------------------------------------------- save

        public PsyOpsSaveState CaptureSave() => PsyOpsSaveCodec.ToSaveState(System.CaptureState());

        public void RestoreSave(PsyOpsSaveState save)
        {
            if (save == null) return;
            System.RestoreState(PsyOpsSaveCodec.FromSaveState(save));
            LastEvent = "Broadcast schedules restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            PsyOpsSaveStore.TrySave(CaptureSave());
        }
    }
}
