using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ContractorRosterSystem.
    /// Wraps the Core contractor pipeline (GenerateOffer → AcceptOffer → Dismiss → TickDay)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class ContractorRosterHostSession
    : HostSessionBase{
        public ContractorRosterSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public ContractorRosterHostSession(
            ContractorRosterSystem system,
            Inventory inventory,
            DutyRosterSystem roster,
            ExpeditionSystem expeditions)
        {
            System = system
                ?? new ContractorRosterSystem(new SeededRng(1986), inventory, roster, expeditions, new GodotLog());

            System.OnContractorStatusChanged += contractor =>
            {
                LastEvent = $"Contractor status changed: {contractor.contractorId}";
                RaiseStateChanged();
            };
            System.OnOfferStatusChanged += offer =>
            {
                LastEvent = $"Offer status changed: {offer.offerId}";
                RaiseStateChanged();
            };
            System.OnRosterChanged += () => RaiseStateChanged();
        }

        public ActionResult GenerateOffer(string candidateId, string role, List<string> requiredSkills, int initialFee, int dailyPay, int termDays)
        {
            var res = System.GenerateOffer(candidateId, role, requiredSkills, initialFee, dailyPay, termDays);
            if (res.IsSuccess)
            {
                LastEvent = $"Offer generated for {candidateId} ({role})";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult AcceptOffer(string offerId)
        {
            var res = System.AcceptOffer(offerId);
            if (res.IsSuccess)
            {
                LastEvent = $"Offer accepted: {offerId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult Dismiss(string contractorId)
        {
            var res = System.Dismiss(contractorId);
            if (res.IsSuccess)
            {
                LastEvent = $"Contractor dismissed: {contractorId}";
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            ContractorRosterSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// Contractor roster save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class ContractorRosterSaveStore
    {
        public const string FileName = "contractor_roster_save.json";
        public const string SectionName = "contractor_roster";

        private static readonly SaveStore<ContractorRosterState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ContractorRosterSaveStore),
            SchemaVersionedEnvelope<ContractorRosterState>.Encode,
            SchemaVersionedEnvelope<ContractorRosterState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(ContractorRosterState state) => s_store.TrySave(state);

        public static ContractorRosterState? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ContractorRosterState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ContractorRosterState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ContractorRosterState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ContractorRosterState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
