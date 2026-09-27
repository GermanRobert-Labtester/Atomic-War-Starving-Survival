// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : SurvivorLetterDeliveryHostSession
// Core System  : Ashfall.Core.Narrative.SurvivorLetterDeliverySystem
// Host Caller  : Main.SurvivorLetterDelivery
// Purpose      : ORPHAN-SEAL follow-up — binds the authored dead-letter catalog
//                to the letter delivery lifecycle (found → addressed →
//                delivered/withheld) and keeps the morale consequence on the
//                owning survivor-needs authority instead of a local counter.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class SurvivorLetterDeliveryHostSession : HostSessionBase
    {
        public const string CatalogFile = "narrative/survivor_letters_lost_kin.json";

        public SurvivorLetterDeliverySystem System { get; }
        public SurvivorLetterCatalog? Catalog { get; private set; }
        public int LettersInCatalog => Catalog?.AllLetters.Count ?? 0;

        /// <summary>Survivor ids whose letters were delivered (morale applied once per letter).</summary>
        public event Action<string, string, float>? LetterDelivered;

        /// <summary>Host-side dirty hook so the caller marks its save section.</summary>
        public Action? StateChangedHook { get; set; }

        public SurvivorLetterDeliveryHostSession(SurvivorLetterDeliverySystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public static SurvivorLetterDeliveryHostSession Create(IFileIO files, IJsonSerializer serializer)
        {
            var system = new SurvivorLetterDeliverySystem();
            var session = new SurvivorLetterDeliveryHostSession(system);
            BindCatalog(system, session, files, serializer);
            return session;
        }

        /// <summary>
        /// Loads the authored dead-letter catalog through the shared data-path
        /// authority. Returns false when the catalog is absent so the caller can
        /// report an empty collection instead of silently pretending otherwise.
        /// </summary>
        public static bool BindCatalog(
            SurvivorLetterDeliverySystem system,
            SurvivorLetterDeliveryHostSession session,
            IFileIO files,
            IJsonSerializer serializer)
        {
            try
            {
                string path = CatalogPath.ResolveCatalog(CatalogFile);
                if (!files.FileExists(path)) return false;
                var catalog = new SurvivorLetterCatalog();
                catalog.Load(files.ReadAllText(path), serializer);
                system.BindCatalog(catalog);
                session.Catalog = catalog;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SurvivorLetterRecordState? GetRecord(string letterId) => System.GetRecord(letterId);

        public IReadOnlyList<SurvivorLetterRecordState> GetAllRecords() => System.GetAllRecords();

        public IReadOnlyList<SurvivorLetterRecordState> GetRecordsByState(string state) =>
            System.GetRecordsByState(state);

        public bool MarkFound(string letterId, int day) => System.MarkFound(letterId, day);

        /// <summary>
        /// Attempts automatic addressing against the live shelter roster. The
        /// roster is the survivor authority; nothing here caches dweller state.
        /// </summary>
        public bool TryAddressToSurvivor(string letterId, SurvivorsHostSession roster)
        {
            if (roster == null) return false;
            return System.TryAddressToSurvivor(letterId, BuildDwellerCandidates(roster));
        }

        public bool AssignRecipientExplicit(string letterId, string survivorId) =>
            System.AssignRecipientExplicit(letterId, survivorId);

        /// <summary>
        /// Delivers a letter and routes the morale consequence through the
        /// owning needs authority. Returns false when the record is not in a
        /// deliverable state (partial state is never mutated).
        /// </summary>
        public bool Deliver(string letterId, int day, SurvivorsHostSession roster)
        {
            bool delivered = System.Deliver(
                letterId,
                day,
                applyMorale: roster == null
                    ? null
                    : (survivorId, delta) => roster.Needs.Modify(survivorId, NeedKind.Morale, delta));
            if (delivered)
            {
                var rec = System.GetRecord(letterId);
                LetterDelivered?.Invoke(letterId, rec?.matched_survivor_id ?? string.Empty,
                    rec?.morale_delta_applied ?? 0f);
            }
            return delivered;
        }

        public bool Withhold(string letterId, int day, SurvivorsHostSession roster) =>
            System.Withhold(
                letterId,
                day,
                applyMorale: roster == null
                    ? null
                    : (survivorId, delta) => roster.Needs.Modify(survivorId, NeedKind.Morale, delta));

        public bool MarkUnanswered(string letterId, int day) => System.MarkUnanswered(letterId, day);

        /// <summary>
        /// Census for panels and probes: catalog size, per-state counts, and the
        /// resolved vs unresolved split.
        /// </summary>
        public (int Catalog, int Found, int Addressed, int Delivered, int Withheld, int Unanswered) GetCensus()
        {
            int found = 0, addressed = 0, delivered = 0, withheld = 0, unanswered = 0;
            foreach (var r in System.GetAllRecords())
            {
                switch (r.delivery_state)
                {
                    case LetterDeliveryStates.Found: found++; break;
                    case LetterDeliveryStates.Addressed: addressed++; break;
                    case LetterDeliveryStates.Delivered: delivered++; break;
                    case LetterDeliveryStates.Withheld: withheld++; break;
                    case LetterDeliveryStates.Unanswered: unanswered++; break;
                }
            }
            return (LettersInCatalog, found, addressed, delivered, withheld, unanswered);
        }

        /// <summary>
        /// Living dweller address candidates read straight from the roster.
        /// The roster carries no display-name authority, so the survivor id is
        /// the only truthful identity offered; authored display names would be
        /// invented data.
        /// </summary>
        internal static List<DwellerAddressCandidate> BuildDwellerCandidates(SurvivorsHostSession roster)
        {
            var result = new List<DwellerAddressCandidate>();
            if (roster?.RosterState == null) return result;
            foreach (var survivor in roster.RosterState)
            {
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                result.Add(new DwellerAddressCandidate
                {
                    SurvivorId = survivor.Id,
                    Name = survivor.Id,
                    Role = string.Empty,
                    IsAlive = survivor.IsAliveState,
                });
            }
            return result;
        }
    }
}
