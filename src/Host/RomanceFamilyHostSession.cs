// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RomanceFamilySaveStore
// Core State : Ashfall.Core.Survivors.RomanceFamilySystem (CaptureState/RestoreState)
// Host Caller: Main.RomanceFamily (SetupRomanceFamily / SaveRomanceFamily)
// Purpose    : Plan 150 — Romance & family dynamics: attraction, courtship,
//              partnership, bonded pairs, family units, adoption, and the
//              carrying of a family's story forward.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using FamilyUnit = Ashfall.Core.Survivors.FamilyUnit;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side persistence projection of the romance/family document. The
    /// Core owns the document; the host wraps it so the section travels in the
    /// same checksummed envelope every other integrated system uses. There is
    /// exactly one capture path — the Core's — and this only carries it.
    /// </summary>
    public sealed class RomanceFamilyPersistedState
    {
        public int schema_version { get; set; } = 1;
        public string core_state { get; set; } = string.Empty;
    }

    public static class RomanceFamilySaveStore
    {
        public const string FileName = "romance_family_save.json";
        public const string SectionName = "romance_family";

        private static readonly SaveStore<RomanceFamilyPersistedState> s_store =
            SaveStoreHub.Checksummed<RomanceFamilyPersistedState>(FileName, nameof(RomanceFamilySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(RomanceFamilyPersistedState state) => s_store.CaptureBare(state);
        public static RomanceFamilyPersistedState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RomanceFamilyPersistedState state) => s_store.TrySave(state);
        public static RomanceFamilyPersistedState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session for Plan 150 (Romance &amp; Family Dynamics). Binds the
    /// strict authored courtship table onto the Core authority and exposes the
    /// live relationship and family surface to the campaign day owner.
    /// </summary>
    public sealed class RomanceFamilyHostSession : HostSessionBase
    {
        /// <summary>Authored courtship table in the data authority.</summary>
        public const string CatalogFile = RomanceCourtshipCatalogLoader.FileName;

        private readonly RomanceFamilySystem _system;
        private string _lastEvent = string.Empty;

        public RomanceFamilySystem System => _system;
        public string LastEvent => _lastEvent;
        public RomanceFamilyCensus Census => _system.GetCensus();
        public IReadOnlyList<RomanticRelationship> Relationships => _system.Relationships;
        public IReadOnlyList<FamilyUnit> FamilyUnits => _system.FamilyUnits;
        public RomanceCourtshipCatalog Catalog => _system.Catalog;

        /// <summary>True when the authored table loaded and was bound.</summary>
        public bool UsingAuthoredCatalog { get; private set; }

        /// <summary>Overlay error, when the authored table was rejected.</summary>
        public string? LoadError { get; private set; }

        public RomanceFamilyHostSession(string? dataDir = null, IFileIO? files = null)
        {
            _system = new RomanceFamilySystem();

            _system.OnRomanceStageAdvancedSeam += (a, b, stage) =>
            {
                _lastEvent = $"Romance stage advanced: {a} and {b} reached {stage}.";
                RaiseStateChanged();
            };
            _system.OnFamilyUnitEstablishedSeam += (familyId, parents) =>
            {
                _lastEvent = $"Family unit established: {familyId} ({parents.Count} parents).";
                RaiseStateChanged();
            };
            _system.OnPartnershipDissolvedSeam += (a, b, reason) =>
            {
                _lastEvent = $"Partnership dissolved: {a} and {b} ({reason}).";
                RaiseStateChanged();
            };
            _system.OnChildWelcomedToFamilySeam += (familyId, childId, adopted) =>
            {
                _lastEvent = $"{(adopted ? "Adopted" : "Welcomed")} child {childId} into {familyId}.";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir!, files);
            }
        }

        public static RomanceFamilyHostSession Create(string dataDir, IFileIO? files = null)
            => new RomanceFamilyHostSession(dataDir, files);

        /// <summary>
        /// Load the authored courtship table through the strict loader and bind
        /// only validated rows. A rejected table leaves the contract unbound
        /// rather than falling back to the lenient parser, so an authored typo
        /// can never become a live courtship event.
        /// </summary>
        public void LoadCatalog(string dataDirectory, IFileIO? files = null)
        {
            if (string.IsNullOrEmpty(dataDirectory)) return;

            var loaded = RomanceCourtshipCatalogLoader.Load(dataDirectory, files ?? new FileSystemIO());
            if (loaded.HasErrors)
            {
                UsingAuthoredCatalog = false;
                LoadError = string.Join("; ", loaded.Errors);
                _lastEvent = "Courtship catalog rejected: " + LoadError;
                RaiseStateChanged();
                return;
            }

            _system.Catalog.BindValidatedEvents(loaded.Events);
            UsingAuthoredCatalog = true;
            LoadError = null;
            _lastEvent = $"Bound {loaded.Events.Count} courtship event(s) from the authored table.";
            RaiseStateChanged();
        }

        public float CalculateCompatibility(int ageA, int ageB, string beliefA, string beliefB, bool sharedTrauma)
            => _system.CalculateCompatibility(ageA, ageB, beliefA, beliefB, sharedTrauma);

        public RomanticRelationship? GetRelationship(string survivorA, string survivorB)
            => _system.GetRelationship(survivorA, survivorB);

        public RomanticRelationship? GetRomanticPartner(string survivorId)
            => _system.GetRomanticPartner(survivorId);

        public FamilyUnit? GetFamilyForSurvivor(string survivorId)
            => _system.GetFamilyForSurvivor(survivorId);

        public bool TryInitiateAttraction(
            string survivorA,
            string survivorB,
            float affinity,
            int ageA,
            int ageB,
            string beliefA,
            string beliefB,
            bool sharedTrauma,
            ISeededRng rng,
            int currentDay = 1,
            bool force = false)
            => _system.TryInitiateAttraction(
                survivorA, survivorB, affinity, ageA, ageB, beliefA, beliefB,
                sharedTrauma, rng, currentDay, force);

        public bool ConductCourtshipEvent(
            string survivorA,
            string survivorB,
            string eventId,
            float currentAffinity,
            ISeededRng rng,
            int currentDay,
            bool forceSuccess = false)
            => _system.ConductCourtshipEvent(
                survivorA, survivorB, eventId, currentAffinity, rng, currentDay, forceSuccess);

        public bool DissolvePartnership(string survivorA, string survivorB, string reason = "mutual_drift")
            => _system.DissolvePartnership(survivorA, survivorB, reason);

        public FamilyUnit FormFamilyUnit(string survivorA, string survivorB, string familyName)
            => _system.FormFamilyUnit(survivorA, survivorB, familyName);

        public bool AddChildToFamily(string familyId, string childId, bool isAdopted)
            => _system.AddChildToFamily(familyId, childId, isAdopted);

        public void AdvanceDay(int currentDay) => _system.AdvanceDay(currentDay);

        public string CaptureCoreState() => _system.CaptureState();

        public void RestoreCoreState(string json) => _system.RestoreState(json);

        /// <summary>Project the live Core document into the persisted section shape.</summary>
        public RomanceFamilyPersistedState CapturePersistedState()
            => new RomanceFamilyPersistedState
            {
                schema_version = 1,
                core_state = _system.CaptureState()
            };
    }
}
