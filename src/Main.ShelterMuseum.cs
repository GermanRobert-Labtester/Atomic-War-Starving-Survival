// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 218 — Shelter Museum & Historical Archive host wiring.
// DEC-203: the museum owns collection, exhibitions, curator, visits, and
// events. The morale delta from an explicit once-per-day visit is applied
// exactly once through the canonical Needs owner. Daily exhibition expiry
// rides the existing TickPlans46_49 daily orchestration — no second day
// owner, no wall-clock timer, no new day event.
// ============================================================================

using System;
using Ashfall.Core.Culture;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterMuseumHostSession? _shelterMuseum;
        private bool _shelterMuseumDirty;
        private Action? _shelterMuseumStateChangedHandler;
        private Action<string, float>? _shelterMuseumVisitHandler;

        public ShelterMuseumHostSession? ShelterMuseum => _shelterMuseum;

        public void SetupShelterMuseum()
        {
            if (_shelterMuseum != null) return;

            var saved = ShelterMuseumSaveStore.TryLoad();
            _shelterMuseum = ShelterMuseumHostSession.Create(saved);
            _shelterMuseumStateChangedHandler = () => _shelterMuseumDirty = true;
            _shelterMuseum.StateChanged += _shelterMuseumStateChangedHandler;

            // Museum visit morale flows through the canonical Needs owner,
            // applied exactly once per recorded visit.
            _shelterMuseumVisitHandler = (visitorId, moraleGain) =>
            {
                if (string.IsNullOrEmpty(visitorId)) return;
                var survivor = _survivors?.Needs.Get(visitorId);
                if (survivor == null || !survivor.IsAliveState) return;
                _survivors!.Needs.Modify(visitorId, Ashfall.Core.Survivors.NeedKind.Morale, moraleGain);
            };
            _shelterMuseum.System.OnMuseumVisited += _shelterMuseumVisitHandler;

            string catalogPath = CatalogPath.ResolveCatalog("museum_collection_templates.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _shelterMuseum.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        /// <summary>
        /// Explicit once-per-day museum visit for one survivor. Returns the
        /// visitor feedback line; null morale application happens inside the
        /// session/Needs seam, never on panel refresh.
        /// </summary>
        public string VisitShelterMuseum(string survivorId)
        {
            SetupShelterMuseum();
            float? morale = _shelterMuseum!.Visit(survivorId, _simDay);
            if (morale.HasValue) _shelterMuseumDirty = true;
            return _shelterMuseum.LastEvent;
        }

        /// <summary>Appoints the chief curator (host/CLI curation command).</summary>
        public string AppointShelterMuseumCurator(string survivorId)
        {
            SetupShelterMuseum();
            _shelterMuseum!.AppointCurator(survivorId, _simDay);
            _shelterMuseumDirty = true;
            return _shelterMuseum.LastEvent;
        }

        /// <summary>
        /// Accessions a nonphysical historical record from an authored template.
        /// No inventory item is consumed; physical donation remains unavailable
        /// until a transaction-safe custody bridge is signed.
        /// </summary>
        public string AccessionShelterMuseumTemplate(string templateId, string donorId)
        {
            SetupShelterMuseum();
            var artifact = _shelterMuseum!.AccessionTemplateRecord(templateId, donorId, _simDay);
            if (artifact != null) _shelterMuseumDirty = true;
            return _shelterMuseum.LastEvent;
        }

        public ShelterMuseumSnapshot? GetShelterMuseumSnapshot()
        {
            SetupShelterMuseum();
            return _shelterMuseum!.GetSnapshot();
        }

        /// <summary>Daily exhibition expiry through the existing daily orchestration.</summary>
        public void TickShelterMuseum(int day)
        {
            SetupShelterMuseum();
            _shelterMuseum!.TickDay(day);
        }

        public void SaveShelterMuseum()
        {
            if (_shelterMuseum == null) return;
            var state = _shelterMuseum.System.CaptureState();
            if (CaptureSection(
                    ShelterMuseumSaveStore.SectionName,
                    ShelterMuseumSaveStore.TryCapturePersisted(state)))
            {
                _shelterMuseumDirty = false;
            }
        }

        public void FlushShelterMuseumIfDirty()
        {
            if (_shelterMuseumDirty)
            {
                SaveShelterMuseum();
            }
        }

        public void ResetShelterMuseum()
        {
            if (_shelterMuseum != null)
            {
                if (_shelterMuseumStateChangedHandler != null)
                    _shelterMuseum.StateChanged -= _shelterMuseumStateChangedHandler;
                if (_shelterMuseumVisitHandler != null)
                    _shelterMuseum.System.OnMuseumVisited -= _shelterMuseumVisitHandler;
            }

            _shelterMuseum = null;
            _shelterMuseumDirty = false;
            _shelterMuseumStateChangedHandler = null;
            _shelterMuseumVisitHandler = null;
        }
    }
}
