#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for Plan 134: Dynamic Faction Territory & Supply Line Control.
    /// Manages territorial nodes, contested sites, garrisons, and supply lines across the wasteland.
    /// Follows single-authority rule: territory state lives in <see cref="TerritoryControlSystem"/>,
    /// persistence lives in <see cref="TerritoryControlSaveStore"/>.
    /// </summary>
    public sealed class TerritoryControlHostSession : HostSessionBase
    {
        public const string CatalogTerritoriesFile = "faction_territory.json";
        public const string CatalogSupplyLinesFile = "supply_lines.json";

        public TerritoryControlSystem System { get; }
        public bool CatalogLoaded { get; private set; }

        public TerritoryControlHostSession(TerritoryControlSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnTerritoryControlChangedSeam += (loc, oldF, newF) => RaiseStateChanged();
            System.OnTerritoryContestedSeam += (loc, oldF, newF) => RaiseStateChanged();
            System.OnSupplyLineStatusChangedSeam += (line, st) => RaiseStateChanged();
            System.OnSupplyLineDeliveredSeam += (line, amt) => RaiseStateChanged();
            System.OnLocationFortifiedSeam += (loc, lvl) => RaiseStateChanged();
        }

        /// <summary>
        /// Loads both authored catalogs (faction_territory.json and supply_lines.json)
        /// into a live TerritoryControlHostSession.
        /// </summary>
        public static TerritoryControlHostSession Load(string dataDirectory, IFileIO files)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));

            string terrPath = Path.Combine(dataDirectory, CatalogTerritoriesFile);
            if (!files.FileExists(terrPath))
            {
                throw new FileNotFoundException(
                    $"TerritoryControlHostSession: {CatalogTerritoriesFile} not found at '{terrPath}'.");
            }

            string supplyPath = Path.Combine(dataDirectory, CatalogSupplyLinesFile);
            if (!files.FileExists(supplyPath))
            {
                throw new FileNotFoundException(
                    $"TerritoryControlHostSession: {CatalogSupplyLinesFile} not found at '{supplyPath}'.");
            }

            string terrJson = files.ReadAllText(terrPath);
            string supplyJson = files.ReadAllText(supplyPath);

            var system = TerritoryControlSystem.FromJson(terrJson, supplyJson);
            return new TerritoryControlHostSession(system) { CatalogLoaded = true };
        }

        public TerritoryCensus ReadCensus() => System.ReadCensus();

        public TerritoryControlSaveState CaptureState() => System.CaptureState();

        public bool RestoreState(TerritoryControlSaveState? state)
        {
            bool ok = System.RestoreState(state);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public bool FortifyLocation(string locationId, int levelDelta = 1) =>
            System.FortifyLocation(locationId, levelDelta);

        public bool AssignGarrison(string locationId, int garrisonDelta) =>
            System.AssignGarrison(locationId, garrisonDelta);

        public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0) =>
            System.ContestLocation(locationId, attackingFactionId, attackPower, rng, currentDay);

        public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng) =>
            System.RaidSupplyLine(supplyLineId, raidIntensity, rng);

        public bool RestoreSupplyLine(string supplyLineId) =>
            System.RestoreSupplyLine(supplyLineId);

        public void TickDay(int currentDay, ISeededRng? rng = null) =>
            System.TickDay(currentDay, rng);
    }

    /// <summary>
    /// Checksummed save store for the territory_control save section (Plan 134).
    /// </summary>
    public static class TerritoryControlSaveStore
    {
        public const string FileName = "territory_control_save.json";
        public const string SectionName = "territory_control";

        private static readonly SaveStore<TerritoryControlSaveState> s_store =
            SaveStoreHub.Checksummed<TerritoryControlSaveState>(FileName, nameof(TerritoryControlSaveStore));

        public static bool TrySave(TerritoryControlSaveState state) => s_store.TrySave(state);
        public static TerritoryControlSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(TerritoryControlSaveState state) => s_store.CapturePersisted(state);
        public static TerritoryControlSaveState? TryRestore(string json) => s_store.RestoreEnvelope(json);
        public static TerritoryControlSaveState? TryRestoreBare(string json) => s_store.RestoreBare(json);
    }
}
