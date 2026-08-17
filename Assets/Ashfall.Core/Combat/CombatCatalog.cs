using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Combat
{
    /// <summary>Data-driven weapon definition. snake_case ids per the master list.</summary>
    [Serializable]
    public class CombatWeaponDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public float accuracy = 0.6f;        // base chance to connect
        public float damage = 12f;
        public float range = 1f;             // range modifier (1 = standard)
        public string caliber = string.Empty;// ammo id the weapon takes
        public int burst = 1;                // rounds per trigger pull
        public bool isJuryRigged;            // pipe_ / improvised firearm
        public bool isSuppressionCapable;    // rifle/LMG can lay suppressive fire
        public float degradePerShot = 0.015f;// condition loss per shot fired
        public float jamBase = 0.04f;        // base jam chance at pristine condition
        public int scrapRepairCost = 3;      // scrap metal to field-repair to full
        public float conditionThreshold = 0.25f; // below this, jam risk rises steeply
    }

    /// <summary>Data-driven ammunition definition.</summary>
    [Serializable]
    public class CombatAmmoDefinition
    {
        public string id = string.Empty;     // caliber id
        public string displayName = string.Empty;
        public float damageMod = 1f;
        public float rangeMod = 1f;
        public bool isMilitaryTier;          // military ammo in a jury weapon risks burst failure
    }

    /// <summary>Data-driven cover / armor / barrier material properties.</summary>
    [Serializable]
    public class CombatMaterialDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string kind = "cover";        // cover | armor | barrier
        public float armorReduction;         // fraction of damage absorbed
        public float ricochetChance;         // chance to deflect a hit
        public float ricochetEnergyRetained = 0.6f;
    }

    /// <summary>
    /// Registry of combat definitions so the core stays data-free while hosts can
    /// register JSON-loaded catalogs. Mirrors ExpeditionDefinitionRegistry.
    /// </summary>
    public static class CombatCatalog
    {
        private static readonly Dictionary<string, CombatWeaponDefinition> s_weapons =
            new Dictionary<string, CombatWeaponDefinition>(StringComparer.Ordinal);
        private static readonly Dictionary<string, CombatAmmoDefinition> s_ammo =
            new Dictionary<string, CombatAmmoDefinition>(StringComparer.Ordinal);
        private static readonly Dictionary<string, CombatMaterialDefinition> s_materials =
            new Dictionary<string, CombatMaterialDefinition>(StringComparer.Ordinal);

        public static void SeedDefaults()
        {
            // Data authority is JSON (Assets/StreamingAssets/Data/combat_catalog.json).
            // This populates the registry from that file so code never owns the
            // values (Invariant #6). Idempotent: a host that already loaded the
            // JSON is never clobbered.
            if (s_weapons.Count > 0 || s_ammo.Count > 0 || s_materials.Count > 0)
                return;

            string dataDir = null;
            if (!CatalogLocator.TryFindDataDirectory(Environment.CurrentDirectory, out dataDir)
                && !CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir))
                dataDir = null;

            if (!string.IsNullOrEmpty(dataDir))
                CombatCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Crash-avoidance fallback for environments with no data directory
            // (the committed JSON is the authority and is always found in a repo
            // checkout; this exists only to avoid a null weapon set in odd hosts).
            if (s_weapons.Count == 0 && s_ammo.Count == 0 && s_materials.Count == 0)
                SeedMinimalFallback();
        }

        private static void SeedMinimalFallback()
        {
            Register(new CombatWeaponDefinition
            {
                id = "weapon_pipe_rifle", displayName = "Pipe Rifle",
                accuracy = 0.46f, damage = 12f, caliber = "ammo_357", burst = 1,
                isJuryRigged = true, isSuppressionCapable = false,
                degradePerShot = 0.022f, jamBase = 0.055f, scrapRepairCost = 3,
                conditionThreshold = 0.30f
            });
            Register(new CombatAmmoDefinition { id = "ammo_357", displayName = ".357", damageMod = 1.0f, rangeMod = 1.0f, isMilitaryTier = false });
            Register(new CombatMaterialDefinition { id = "material_wood", displayName = "Rotted Wood", kind = "cover", armorReduction = 0.30f, ricochetChance = 0.05f, ricochetEnergyRetained = 0.5f });
        }

        public static void Register(CombatWeaponDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_weapons[def.id] = def;
        }

        public static void Register(CombatAmmoDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_ammo[def.id] = def;
        }

        public static void Register(CombatMaterialDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_materials[def.id] = def;
        }

        public static CombatWeaponDefinition GetWeapon(string id)
        {
            return !string.IsNullOrEmpty(id) && s_weapons.TryGetValue(id, out var d) ? d : null;
        }

        public static CombatAmmoDefinition GetAmmo(string id)
        {
            return !string.IsNullOrEmpty(id) && s_ammo.TryGetValue(id, out var d) ? d : null;
        }

        public static CombatMaterialDefinition GetMaterial(string id)
        {
            return !string.IsNullOrEmpty(id) && s_materials.TryGetValue(id, out var d) ? d : null;
        }

        public static bool HasWeapon(string id) => GetWeapon(id) != null;
        public static bool HasAmmo(string id) => GetAmmo(id) != null;
        public static bool HasMaterial(string id) => GetMaterial(id) != null;

        public static IReadOnlyCollection<string> WeaponIds => s_weapons.Keys;
        public static IReadOnlyCollection<string> AmmoIds => s_ammo.Keys;
        public static IReadOnlyCollection<string> MaterialIds => s_materials.Keys;

        public static void Clear()
        {
            s_weapons.Clear();
            s_ammo.Clear();
            s_materials.Clear();
        }
    }

    // ---------------------------------------------------------------------
    // Combat data authority loader — reads Assets/StreamingAssets/Data/combat_catalog.json
    // ---------------------------------------------------------------------

    [Serializable]
    internal sealed class CombatWeaponJson
    {
        public string id;
        public string display_name;
        public float accuracy;
        public float damage;
        public float range;
        public string caliber;
        public int burst;
        public bool is_jury_rigged;
        public bool is_suppression_capable;
        public float degrade_per_shot;
        public float jam_base;
        public int scrap_repair_cost;
        public float condition_threshold;
    }

    [Serializable]
    internal sealed class CombatAmmoJson
    {
        public string id;
        public string display_name;
        public float damage_mod;
        public float range_mod;
        public bool is_military_tier;
    }

    [Serializable]
    internal sealed class CombatMaterialJson
    {
        public string id;
        public string display_name;
        public string kind;
        public float armor_reduction;
        public float ricochet_chance;
        public float ricochet_energy_retained;
    }

    [Serializable]
    internal sealed class CombatCatalogRoot
    {
        public int schema_version = 1;
        public string collection_id = "combat_catalog";
        public List<CombatWeaponJson> weapons = new List<CombatWeaponJson>();
        public List<CombatAmmoJson> ammo = new List<CombatAmmoJson>();
        public List<CombatMaterialJson> materials = new List<CombatMaterialJson>();
    }

    /// <summary>
    /// Engine-agnostic loader for the Combat data authority
    /// (combat_catalog.json, snake_case). Maps onto the camelCase runtime
    /// definitions. Returns false (leaving the registry untouched) when the
    /// file is absent; surfaces parse/schema errors as exceptions so a bad
    /// catalog never silently slips in.
    /// </summary>
    public static class CombatCatalogLoader
    {
        public const string FileName = "combat_catalog.json";
        public const int CurrentSchemaVersion = 1;

        /// <summary>
        /// Load + validate the combat catalog. Following Invariant #6 and the
        /// save-migration convention: a future schema throws (never silently
        /// guessed), while canonical-id and cross-reference violations throw so
        /// a malformed catalog can never slip into the simulation.
        /// </summary>
        public static bool Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            string path = files.Combine(dataDirectory, FileName);
            if (!files.FileExists(path)) return false;

            var root = json.Deserialize<CombatCatalogRoot>(files.ReadAllText(path));
            if (root == null) return false;

            if (root.schema_version > CurrentSchemaVersion)
                throw new System.IO.InvalidDataException(
                    $"{FileName} schema {root.schema_version} is newer than supported {CurrentSchemaVersion}.");

            CombatCatalog.Clear();

            if (root.weapons != null)
            {
                for (int i = 0; i < root.weapons.Count; i++)
                {
                    var w = root.weapons[i];
                    if (w == null || string.IsNullOrEmpty(w.id)) continue;
                    CombatCatalog.Register(new CombatWeaponDefinition
                    {
                        id = w.id,
                        displayName = w.display_name ?? string.Empty,
                        accuracy = w.accuracy,
                        damage = w.damage,
                        range = w.range,
                        caliber = w.caliber ?? string.Empty,
                        burst = w.burst,
                        isJuryRigged = w.is_jury_rigged,
                        isSuppressionCapable = w.is_suppression_capable,
                        degradePerShot = w.degrade_per_shot,
                        jamBase = w.jam_base,
                        scrapRepairCost = w.scrap_repair_cost,
                        conditionThreshold = w.condition_threshold
                    });
                }
            }

            if (root.ammo != null)
            {
                for (int i = 0; i < root.ammo.Count; i++)
                {
                    var a = root.ammo[i];
                    if (a == null || string.IsNullOrEmpty(a.id)) continue;
                    CombatCatalog.Register(new CombatAmmoDefinition
                    {
                        id = a.id,
                        displayName = a.display_name ?? string.Empty,
                        damageMod = a.damage_mod,
                        rangeMod = a.range_mod,
                        isMilitaryTier = a.is_military_tier
                    });
                }
            }

            if (root.materials != null)
            {
                for (int i = 0; i < root.materials.Count; i++)
                {
                    var m = root.materials[i];
                    if (m == null || string.IsNullOrEmpty(m.id)) continue;
                    CombatCatalog.Register(new CombatMaterialDefinition
                    {
                        id = m.id,
                        displayName = m.display_name ?? string.Empty,
                        kind = m.kind ?? "cover",
                        armorReduction = m.armor_reduction,
                        ricochetChance = m.ricochet_chance,
                        ricochetEnergyRetained = m.ricochet_energy_retained
                    });
                }
            }

            ValidateRegistered(root, CombatCatalog.WeaponIds, CombatCatalog.AmmoIds, CombatCatalog.MaterialIds);
            return true;
        }

        /// <summary>Enforce canonical snake_case id prefixes and ammo cross-references.</summary>
        private static void ValidateRegistered(
            CombatCatalogRoot root,
            System.Collections.Generic.IReadOnlyCollection<string> weaponIds,
            System.Collections.Generic.IReadOnlyCollection<string> ammoIds,
            System.Collections.Generic.IReadOnlyCollection<string> materialIds)
        {
            var errors = new System.Collections.Generic.List<string>();
            var ammoSet = new HashSet<string>(ammoIds, StringComparer.Ordinal);
            var weaponSet = new HashSet<string>(weaponIds, StringComparer.Ordinal);
            var materialSet = new HashSet<string>(materialIds, StringComparer.Ordinal);

            if (root.weapons != null)
            for (int i = 0; i < root.weapons.Count; i++)
            {
                var w = root.weapons[i];
                if (w == null) continue;
                if (string.IsNullOrEmpty(w.id) || !w.id.StartsWith("weapon_", StringComparison.Ordinal))
                    errors.Add("weapon#" + i + " id must be canonical (weapon_*): " + (w.id ?? "<null>"));
                else if (!string.IsNullOrEmpty(w.caliber) && !ammoSet.Contains(w.caliber))
                    errors.Add("weapon " + w.id + " references unknown caliber " + w.caliber);
            }
            if (root.ammo != null)
            for (int i = 0; i < root.ammo.Count; i++)
            {
                var a = root.ammo[i];
                if (a == null) continue;
                if (string.IsNullOrEmpty(a.id) || !a.id.StartsWith("ammo_", StringComparison.Ordinal))
                    errors.Add("ammo#" + i + " id must be canonical (ammo_*): " + (a.id ?? "<null>"));
            }
            if (root.materials != null)
            for (int i = 0; i < root.materials.Count; i++)
            {
                var m = root.materials[i];
                if (m == null) continue;
                bool canon = !string.IsNullOrEmpty(m.id)
                    && (m.id.StartsWith("material_", StringComparison.Ordinal)
                        || m.id.StartsWith("armor_", StringComparison.Ordinal));
                if (!canon)
                    errors.Add("material#" + i + " id must be canonical (material_*/armor_*): " + (m.id ?? "<null>"));
            }

            if (errors.Count > 0)
                throw new FormatException("combat_catalog.json failed validation:\n" + string.Join("\n", errors));
        }
    }
}
