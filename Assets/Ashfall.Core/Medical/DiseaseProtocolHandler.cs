// SPDX-License-Identifier: MIT
// Task #133 P1 — Camp-wide disease vector protocols on the medical pipeline.
using System;
using System.Collections.Generic;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// A camp-wide medical protocol (shelter-wide countermeasure, not a
    /// per-patient treatment). Unlike <see cref="IAfflictionHandler"/> there is
    /// no patient: protocols act on shared shelter state, so the coordinator
    /// routes them through a separate registry and never checks patient
    /// availability. Item costs come from the authored disease catalog
    /// contract (vector countermeasures); the pipeline consumes them through
    /// the authoritative inventory.
    /// </summary>
    public interface IMedicalProtocolHandler
    {
        /// <summary>Stable snake_case protocol id, e.g. <c>protocol_purify_water</c>.</summary>
        string ProtocolId { get; }

        /// <summary>Player-facing label.</summary>
        string DisplayName { get; }

        /// <summary>Inventory cost per application (first engage only).</summary>
        IReadOnlyDictionary<string, int> ItemCosts { get; }

        /// <summary>
        /// Null when the protocol may be applied, otherwise a stable snake_case
        /// reason code ("already_applied", ...). Must not mutate state.
        /// </summary>
        string? Validate();

        /// <summary>
        /// Apply the protocol to domain state. Called only after the pipeline
        /// validated and consumed resources. Must succeed when <see cref="Validate"/>
        /// returned null (the validate-first contract).
        /// </summary>
        bool Apply();
    }

    /// <summary>
    /// The four authored DiseaseSystem vector protocols wired to the pipeline.
    /// One protocol per transmission vector; each consumes the catalog's
    /// canonical countermeasure item exactly once per engage. Re-applying a
    /// live protocol is rejected with <c>already_applied</c> and consumes
    /// nothing. There is no pipeline off-switch: the domain's Reset* methods
    /// stay simulation-internal.
    /// </summary>
    public sealed class DiseaseProtocolHandler : IMedicalProtocolHandler
    {
        private readonly DiseaseSystem _disease;
        private readonly string _protocolId;
        private readonly string _displayName;
        private readonly Action _apply;
        private readonly Func<bool> _isActive;
        private readonly Dictionary<string, int> _costs;

        public DiseaseProtocolHandler(
            DiseaseSystem disease,
            string protocolId,
            string displayName,
            Action apply,
            Func<bool> isActive,
            string itemId,
            int amount = 1)
        {
            _disease = disease ?? throw new ArgumentNullException(nameof(disease));
            _protocolId = string.IsNullOrEmpty(protocolId)
                ? throw new ArgumentException("Protocol id must not be empty.", nameof(protocolId))
                : protocolId;
            _displayName = displayName ?? string.Empty;
            _apply = apply ?? throw new ArgumentNullException(nameof(apply));
            _isActive = isActive ?? throw new ArgumentNullException(nameof(isActive));
            _costs = new Dictionary<string, int>(StringComparer.Ordinal);
            if (!string.IsNullOrEmpty(itemId) && amount > 0)
                _costs[ItemAliases.ToCanonical(itemId)] = amount;
        }

        public string ProtocolId => _protocolId;
        public string DisplayName => _displayName;
        public IReadOnlyDictionary<string, int> ItemCosts => _costs;

        public string? Validate()
        {
            if (_isActive()) return "already_applied";
            return null;
        }

        public bool Apply()
        {
            _apply();
            return true;
        }

        /// <summary>
        /// Register the four authored vector protocols. Returns the count registered.
        /// Plan 60 / D4 — an optional day provider lets the coordinator arm expiry at
        /// the day the protocol is actually applied; without it the protocol re-arms
        /// from the next day tick, which is the legacy-correct fallback.
        /// </summary>
        public static int RegisterAll(MedicalPipelineCoordinator pipeline, DiseaseSystem disease,
            Func<int>? dayProvider = null)
        {
            if (pipeline == null || disease == null) return 0;
            int Day() => dayProvider?.Invoke() ?? 0;
            pipeline.RegisterProtocol(new DiseaseProtocolHandler(
                disease,
                MedicalTreatmentCatalog.ProtocolPurifyWater,
                "Purify Water Stores",
                () => disease.PurifyWater(Day()),
                () => disease.State.water_purified,
                MedicalTreatmentCatalog.ItemCleanWater));
            pipeline.RegisterProtocol(new DiseaseProtocolHandler(
                disease,
                MedicalTreatmentCatalog.ProtocolSealVents,
                "Seal Ventilators",
                () => disease.SealVents(Day()),
                () => disease.State.vents_sealed,
                MedicalTreatmentCatalog.ItemGasMask));
            pipeline.RegisterProtocol(new DiseaseProtocolHandler(
                disease,
                MedicalTreatmentCatalog.ProtocolSterilizeTools,
                "Sterilise Surgical Tools",
                () => disease.SterilizeTools(Day()),
                () => disease.State.tools_sterilized,
                MedicalTreatmentCatalog.ItemAntibiotics));
            pipeline.RegisterProtocol(new DiseaseProtocolHandler(
                disease,
                MedicalTreatmentCatalog.ProtocolAirFiltration,
                "Engage Air Filtration",
                () => disease.SetAirFiltration(true, Day()),
                () => disease.State.air_filtration,
                MedicalTreatmentCatalog.ItemHazmatSuit));
            return 4;
        }
    }
}
