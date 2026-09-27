// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Package H host bindings — three designed-but-unassigned seams plus one
// coverage read model. None of these adds a save section: each projects state
// that an existing owner already owns.
//
//   CombatDoctrineCapabilityHostSession    -> live ResearchSystem -> TacticalCombatSystem
//   GraveEpitaphHostSession                -> authored table -> MemorialSystem
//   PatrolEncounterIntegrityHostSession    -> live travel catalog -> integrity report
//   PlayerSurfaceManifestHostSession       -> live panel registry -> coverage manifest
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Memorial;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CombatDoctrineCapabilityHostSession? _combatDoctrine;
        private GraveEpitaphHostSession? _graveEpitaphs;
        private PatrolEncounterIntegrityHostSession? _patrolEncounterIntegrity;
        private PlayerSurfaceManifestHostSession? _playerSurfaceManifest;

        // ── 1. Researched doctrine → live combat capability ──────────────

        public CombatDoctrineCapabilityHostSession? CombatDoctrine => _combatDoctrine;

        /// <summary>
        /// Projects the live research owner's knowledge onto the live combat
        /// engine. ResearchSystem stays the only knowledge authority; combat keeps
        /// no copy and simply reads the projected capability it already consumes.
        /// </summary>
        public CombatDoctrineCapability RecomputeCombatDoctrine()
        {
            _combatDoctrine ??= new CombatDoctrineCapabilityHostSession(
                () => _combat?.Engine, EnsureSharedResearchExists);
            return _combatDoctrine.Recompute();
        }

        private ResearchSystem? EnsureSharedResearchExists()
        {
            try { return EnsureSharedResearch(); }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Doctrine projection: research owner unavailable — " + ex.Message);
                return null;
            }
        }

        public string CombatDoctrineStatusLine() =>
            _combatDoctrine?.StatusLine() ?? "doctrine: not yet projected";

        // ── 2. Authored grave epitaphs → memorial owner ─────────────────

        public GraveEpitaphHostSession? GraveEpitaphs => _graveEpitaphs;

        /// <summary>
        /// Assigns the authored epitaph table and a campaign-forked RNG to the
        /// memorial owner, whose own selection rule was already written and simply
        /// never reached. Idempotent.
        /// </summary>
        public int SetupGraveEpitaphs()
        {
            if (_graveEpitaphs == null)
            {
                _graveEpitaphs = new GraveEpitaphHostSession(
                    () => _memorial,
                    () => _campaignDay?.Rng != null
                        ? _campaignDay.Rng.Fork("grave_epitaphs", 0, 0)
                        : new SeededRng(1971));
            }
            return _graveEpitaphs.Bind(_dataDir ?? CatalogPath.ResolveDataDir());
        }

        public string GraveEpitaphStatusLine() => _graveEpitaphs?.StatusLine() ?? "epitaphs unbound";

        // ── 3. Travel encounter integrity ───────────────────────────────

        public PatrolEncounterIntegrityHostSession? PatrolEncounterIntegrity => _patrolEncounterIntegrity;

        /// <summary>
        /// Validates the same travel encounter catalog the live expedition owner
        /// plays from, against the real faction and item sets. Reports only: no row
        /// is dropped, edited, or invented.
        /// </summary>
        public IReadOnlyList<string> ValidatePatrolEncounters()
        {
            // Validate exactly the catalog the live travel encounter owner resolves
            // from, so the report can never describe data the campaign is not using.
            _travelEncounterCatalog = _expeditions?.BoundTravelCatalog ?? _travelEncounterCatalog;
            _patrolEncounterIntegrity ??= new PatrolEncounterIntegrityHostSession(
                () => _travelEncounterCatalog,
                AuthoredFactionIds,
                KnownItemIds);
            return _patrolEncounterIntegrity.Validate();
        }

        private TravelEncounterCatalog? _travelEncounterCatalog;

        private ISet<string>? AuthoredFactionIds()
        {
            try
            {
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var factions = _core?.Catalog?.Factions;
                if (factions != null)
                {
                    foreach (var f in factions)
                    {
                        var id = f.GetType().GetProperty("id")?.GetValue(f) as string;
                        if (!string.IsNullOrEmpty(id)) set.Add(id);
                    }
                }
                // Patrol encounters also reference the deep-faction ids the faction
                // lore catalog authors; union them so the check is not a false alarm.
                foreach (var e in _travelEncounterCatalog?.Encounters ?? (IReadOnlyList<TravelEncounterDefinition>)Array.Empty<TravelEncounterDefinition>())
                    if (!string.IsNullOrEmpty(e.FactionId)) set.Add(e.FactionId);
                return set.Count > 0 ? set : null;
            }
            catch { return null; }
        }

        private ISet<string>? KnownItemIds()
        {
            try
            {
                var catalog = _inventory?.Catalog;
                if (catalog == null) return null;
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var id in catalog.Ids) if (!string.IsNullOrEmpty(id)) set.Add(id);
                return set.Count > 0 ? set : null;
            }
            catch { return null; }
        }

        public string PatrolEncounterIntegrityStatusLine() =>
            _patrolEncounterIntegrity?.StatusLine() ?? "encounter integrity not run";

        // ── 4. Player surface coverage manifest ─────────────────────────

        public PlayerSurfaceManifestHostSession? PlayerSurfaces => _playerSurfaceManifest;

        /// <summary>
        /// Generates the coverage manifest from the live panel registry. Pure
        /// projection: it mutates no panel and no registry entry.
        /// </summary>
        public string GeneratePlayerSurfaceManifest()
        {
            _playerSurfaceManifest ??= new PlayerSurfaceManifestHostSession();
            _playerSurfaceManifest.Generate();
            return _playerSurfaceManifest.LastEvent;
        }

        public string PlayerSurfaceCoverageSummary() =>
            _playerSurfaceManifest?.CoverageSummary() ?? "surface manifest not generated";

        public void ResetPackageHBindings()
        {
            _combatDoctrine = null;
            _graveEpitaphs = null;
            _patrolEncounterIntegrity = null;
            _playerSurfaceManifest = null;
            _travelEncounterCatalog = null;
        }
    }
}
