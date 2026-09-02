using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Archaeology
{
    [Serializable]
    public sealed class LoreArchiveDef
    {
        public string archive_id { get; set; } = string.Empty;
        public string title_key { get; set; } = string.Empty;
        public string summary_key { get; set; } = string.Empty;
        public string era { get; set; } = "PreExchange";
        public List<string> topic_tags { get; set; } = new List<string>();
        public int encryption_tier { get; set; } = 1;
        public float required_engineering { get; set; } = 2.0f;
        public float base_work_hours { get; set; } = 12.0f;
        public float power_kw { get; set; } = 2.0f;
        public float corruption_risk { get; set; } = 0.08f;
        public string required_key_item_id { get; set; } = "item_decryption_keycard_prewar";
        public int research_reward { get; set; } = 20;
        public float broker_value { get; set; } = 100.0f;
        public bool unique { get; set; } = true;
    }

    [Serializable]
    public sealed class ArchaeologyCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<LoreArchiveDef> archives { get; set; } = new List<LoreArchiveDef>();
    }

    [Serializable]
    public sealed class ExcavationSite
    {
        public string siteId { get; set; } = string.Empty;
        public string zoneId { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public bool discovered { get; set; }
        public float excavationProgress { get; set; } // 0..100
        public bool exhausted { get; set; }
        public string archiveId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class PreWarArchiveInstance
    {
        public string archiveId { get; set; } = string.Empty;
        public string titleKey { get; set; } = string.Empty;
        public string summaryKey { get; set; } = string.Empty;
        public int encryptionTier { get; set; } = 1;
        public float decryptionProgress { get; set; } // 0..100
        public bool encrypted { get; set; } = true;
        public bool corrupted { get; set; }
        public bool unlocked { get; set; }
        public bool sold { get; set; }
        public bool researchClaimed { get; set; }
        public int researchPoints { get; set; } = 20;
        public float brokerValue { get; set; } = 100.0f;
    }

    [Serializable]
    public sealed class ArchaeologyState
    {
        public string systemId = ArchaeologySystem.SystemId;
        public List<ExcavationSite> sites = new List<ExcavationSite>();
        public List<PreWarArchiveInstance> archives = new List<PreWarArchiveInstance>();
        public List<string> unlockedLoreIds = new List<string>();
        public List<string> soldArchiveIds = new List<string>();
    }

    public sealed class ArchaeologySystem
    {
        public const string SystemId = "archaeology";

        private ArchaeologyState _state = new ArchaeologyState();
        private readonly Dictionary<string, LoreArchiveDef> _catalog = new Dictionary<string, LoreArchiveDef>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ResearchSystem? _researchSystem;
        private readonly ILog _log;
        private int _siteCounter;

        public ArchaeologyState State => _state;
        public IReadOnlyList<ExcavationSite> Sites => _state.sites;
        public IReadOnlyList<PreWarArchiveInstance> Archives => _state.archives;

        public event Action<ExcavationSite>? OnExcavationSiteDiscovered;
        public event Action<PreWarArchiveInstance>? OnArchiveRecovered;
        public event Action<PreWarArchiveInstance>? OnDecryptionStarted;
        public event Action<PreWarArchiveInstance>? OnArchiveCorrupted;
        public event Action<PreWarArchiveInstance, int>? OnLoreUnlocked; // archive, researchPoints
        public event Action<PreWarArchiveInstance, float>? OnArchiveSold; // archive, payout

        public ArchaeologySystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            ResearchSystem? researchSystem = null,
            ILog? log = null,
            string dataPath = "")
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _researchSystem = researchSystem;
            _log = log ?? NullLog.Instance;

            LoadCatalog(dataPath);
        }

        public void LoadCatalog(string dataPath)
        {
            string path = string.IsNullOrEmpty(dataPath)
                ? Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "lore_archives.json")
                : Path.Combine(dataPath, "lore_archives.json");

            if (!File.Exists(path))
            {
                RegisterArchive(new LoreArchiveDef
                {
                    archive_id = "lore_archive_silo_manifest",
                    title_key = "Strategic Missile Silo Armament Ledger",
                    summary_key = "Pre-war manifest detailing payload yields and launch grids.",
                    encryption_tier = 2,
                    required_engineering = 3.0f,
                    base_work_hours = 16.0f,
                    power_kw = 2.5f,
                    research_reward = 25,
                    broker_value = 150.0f
                });
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var container = JsonSerializer.Deserialize<ArchaeologyCatalogContainer>(json);
                if (container?.archives != null)
                {
                    foreach (var a in container.archives)
                        RegisterArchive(a);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[ArchaeologySystem] Failed to load catalog from {path}: {ex.Message}");
            }
        }

        public void RegisterArchive(LoreArchiveDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.archive_id)) return;
            _catalog[def.archive_id] = def;
        }

        public ExcavationSite? SurveyRuins(string zoneId, float scoutSkill)
        {
            if (string.IsNullOrEmpty(zoneId)) return null;

            // Check if site already discovered in zone
            var existing = _state.sites.Find(s => s.zoneId == zoneId);
            if (existing != null) return existing;

            // Select an unrecovered archive from catalog
            string targetArchiveId = string.Empty;
            foreach (var kvp in _catalog)
            {
                if (!_state.archives.Exists(a => a.archiveId == kvp.Key))
                {
                    targetArchiveId = kvp.Key;
                    break;
                }
            }

            if (string.IsNullOrEmpty(targetArchiveId)) return null;

            var site = new ExcavationSite
            {
                siteId = $"site_{++_siteCounter}_{zoneId}",
                zoneId = zoneId,
                displayName = $"Excavation Ruin Sector {zoneId}",
                discovered = true,
                excavationProgress = 0f,
                exhausted = false,
                archiveId = targetArchiveId
            };

            _state.sites.Add(site);
            OnExcavationSiteDiscovered?.Invoke(site);
            return site;
        }

        public PreWarArchiveInstance? ProgressExcavation(string siteId, float laborHours)
        {
            var site = _state.sites.Find(s => s.siteId == siteId);
            if (site == null || site.exhausted) return null;

            site.excavationProgress = Math.Min(100f, site.excavationProgress + laborHours * 10f);

            if (site.excavationProgress >= 100f && !site.exhausted)
            {
                site.exhausted = true;

                _catalog.TryGetValue(site.archiveId, out var def);
                var archive = new PreWarArchiveInstance
                {
                    archiveId = site.archiveId,
                    titleKey = def?.title_key ?? site.archiveId,
                    summaryKey = def?.summary_key ?? string.Empty,
                    encryptionTier = def?.encryption_tier ?? 1,
                    decryptionProgress = 0f,
                    encrypted = true,
                    corrupted = false,
                    unlocked = false,
                    sold = false,
                    researchClaimed = false,
                    researchPoints = def?.research_reward ?? 20,
                    brokerValue = def?.broker_value ?? 100.0f
                };

                _state.archives.Add(archive);
                OnArchiveRecovered?.Invoke(archive);
                return archive;
            }

            return null;
        }

        public ActionResult ProgressDecryption(string archiveId, float hours, float engineerSkill, bool hasPower, bool hasKeycard = false)
        {
            var archive = _state.archives.Find(a => a.archiveId == archiveId);
            if (archive == null) return ActionResult.Blocked("archive_not_found", "archaeology.archive_not_found");
            if (archive.corrupted) return ActionResult.Blocked("archive_corrupted", "archaeology.archive_corrupted");
            if (archive.unlocked) return ActionResult.Blocked("already_unlocked", "archaeology.already_unlocked");

            if (!hasPower)
            {
                // Interruption during decryption carries corruption risk
                _catalog.TryGetValue(archiveId, out var def);
                if (_rng.NextDouble() < (def?.corruption_risk ?? 0.08f))
                {
                    archive.corrupted = true;
                    OnArchiveCorrupted?.Invoke(archive);
                    return ActionResult.Blocked("power_loss_corruption", "archaeology.power_loss_corruption");
                }
                return ActionResult.Blocked("no_power", "archaeology.no_power");
            }

            if (archive.decryptionProgress <= 0f)
            {
                OnDecryptionStarted?.Invoke(archive);
            }

            float keyMultiplier = hasKeycard ? 1.5f : 1.0f;
            float workStep = (hours * engineerSkill * keyMultiplier * 10f);
            archive.decryptionProgress = Math.Min(100f, archive.decryptionProgress + workStep);

            if (archive.decryptionProgress >= 100f)
            {
                archive.encrypted = false;
                archive.unlocked = true;

                if (!_state.unlockedLoreIds.Contains(archiveId))
                {
                    _state.unlockedLoreIds.Add(archiveId);
                }

                // Grant research reward once
                if (!archive.researchClaimed)
                {
                    archive.researchClaimed = true;
                    _researchSystem?.UnlockManual(archiveId);
                    OnLoreUnlocked?.Invoke(archive, archive.researchPoints);
                }
            }

            return ActionResult.Success("archaeology.decryption_progressed");
        }

        public ActionResult SellArchiveToBroker(string archiveId)
        {
            var archive = _state.archives.Find(a => a.archiveId == archiveId);
            if (archive == null) return ActionResult.Blocked("archive_not_found", "archaeology.archive_not_found");
            if (!archive.unlocked) return ActionResult.Blocked("not_decrypted", "archaeology.not_decrypted");
            if (archive.sold || _state.soldArchiveIds.Contains(archiveId))
                return ActionResult.Blocked("already_sold", "archaeology.already_sold");

            // Award broker value in scrap
            _inventory.AddById("scrap_metal", (int)archive.brokerValue);

            archive.sold = true;
            _state.soldArchiveIds.Add(archiveId);

            OnArchiveSold?.Invoke(archive, archive.brokerValue);
            return ActionResult.Success("archaeology.sold");
        }

        public void RestoreState(ArchaeologyState state)
        {
            if (state == null) return;
            _state = state;
            _siteCounter = _state.sites.Count;
        }
    }
}
