// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static readonly StringComparer IdentityComparer = StringComparer.OrdinalIgnoreCase;

        private ArchaeologyState _state = new ArchaeologyState();
        private readonly Dictionary<string, LoreArchiveDef> _catalog =
            new Dictionary<string, LoreArchiveDef>(IdentityComparer);
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ResearchSystem? _researchSystem;
        private readonly ILog _log;
        private long _siteCounter;

        public ArchaeologyState State => CaptureState();
        public IReadOnlyList<ExcavationSite> Sites => CaptureState().sites;
        public IReadOnlyList<PreWarArchiveInstance> Archives => CaptureState().archives;

        public event Action<ExcavationSite>? OnExcavationSiteDiscovered;
        public event Action<PreWarArchiveInstance>? OnArchiveRecovered;
        public event Action<PreWarArchiveInstance>? OnDecryptionStarted;
        public event Action<PreWarArchiveInstance>? OnArchiveCorrupted;
        public event Action<PreWarArchiveInstance, int>? OnLoreUnlocked;
        public event Action<PreWarArchiveInstance, float>? OnArchiveSold;

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
            _catalog.Clear();
            string path;
            try
            {
                path = string.IsNullOrWhiteSpace(dataPath)
                    ? Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "lore_archives.json")
                    : Path.Combine(dataPath, "lore_archives.json");
            }
            catch (Exception ex)
            {
                _log.Warn($"[ArchaeologySystem] Invalid catalog path: {ex.Message}");
                return;
            }

            if (!File.Exists(path))
            {
                RegisterArchive(CreateFallbackArchive());
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var container = JsonSerializer.Deserialize<ArchaeologyCatalogContainer>(json);
                if (container?.archives == null) return;
                foreach (var archive in container.archives)
                    RegisterArchive(archive);
            }
            catch (Exception ex)
            {
                // A malformed reload must not leave the previous catalog active.
                _catalog.Clear();
                _log.Warn($"[ArchaeologySystem] Failed to load catalog from {path}: {ex.Message}");
            }
        }

        public void RegisterArchive(LoreArchiveDef def)
        {
            if (def == null || string.IsNullOrWhiteSpace(def.archive_id)) return;
            string id = NormalizeId(def.archive_id);
            _catalog[id] = NormalizeDefinition(def);
        }

        public ExcavationSite? SurveyRuins(string zoneId, float scoutSkill)
        {
            string normalizedZone = NormalizeId(zoneId);
            if (string.IsNullOrWhiteSpace(normalizedZone)) return null;
            _ = SanitizeNonNegative(scoutSkill);

            var existing = FindSiteByZone(normalizedZone);
            if (existing != null) return CloneSite(existing);

            var reserved = new HashSet<string>(IdentityComparer);
            foreach (var archive in _state.archives)
            {
                if (archive != null && !string.IsNullOrWhiteSpace(archive.archiveId))
                    reserved.Add(NormalizeId(archive.archiveId));
            }
            foreach (var knownSite in _state.sites)
            {
                if (knownSite != null && !string.IsNullOrWhiteSpace(knownSite.archiveId))
                    reserved.Add(NormalizeId(knownSite.archiveId));
            }

            string targetArchiveId = _catalog.Keys
                .Where(id => !reserved.Contains(id))
                .OrderBy(id => id, IdentityComparer)
                .FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(targetArchiveId)) return null;

            var site = new ExcavationSite
            {
                siteId = NextSiteId(normalizedZone),
                zoneId = normalizedZone,
                displayName = $"Excavation Ruin Sector {normalizedZone}",
                discovered = true,
                excavationProgress = 0f,
                exhausted = false,
                archiveId = targetArchiveId
            };

            _state.sites.Add(site);
            OnExcavationSiteDiscovered?.Invoke(CloneSite(site));
            return CloneSite(site);
        }

        public PreWarArchiveInstance? ProgressExcavation(string siteId, float laborHours)
        {
            var site = FindSite(siteId);
            if (site == null || site.exhausted) return null;
            if (!IsFinite(laborHours) || laborHours <= 0f) return null;

            site.excavationProgress = AddProgress(site.excavationProgress, laborHours * 10f);
            if (site.excavationProgress < 100f) return null;

            site.exhausted = true;
            var existing = FindArchive(site.archiveId);
            if (existing != null)
            {
                OnArchiveRecovered?.Invoke(CloneArchive(existing));
                return CloneArchive(existing);
            }

            _catalog.TryGetValue(site.archiveId, out var definition);
            var archive = CreateArchive(site.archiveId, definition);
            _state.archives.Add(CloneArchive(archive));
            OnArchiveRecovered?.Invoke(CloneArchive(archive));
            return CloneArchive(archive);
        }

        public ActionResult ProgressDecryption(
            string archiveId,
            float hours,
            float engineerSkill,
            bool hasPower,
            bool hasKeycard = false)
        {
            string normalizedId = NormalizeId(archiveId);
            var archive = FindArchive(normalizedId);
            if (archive == null) return ActionResult.Blocked("archive_not_found", "archaeology.archive_not_found");
            if (archive.corrupted) return ActionResult.Blocked("archive_corrupted", "archaeology.archive_corrupted");
            if (archive.unlocked) return ActionResult.Blocked("already_unlocked", "archaeology.already_unlocked");
            if (!IsFinite(hours) || hours <= 0f || !IsFinite(engineerSkill) || engineerSkill < 0f)
                return ActionResult.Blocked("invalid_work", "archaeology.invalid_work");

            if (!hasPower)
            {
                _catalog.TryGetValue(normalizedId, out var definition);
                float risk = SanitizeProbability(definition?.corruption_risk ?? 0.08f);
                if (_rng.NextDouble() < risk)
                {
                    archive.corrupted = true;
                    OnArchiveCorrupted?.Invoke(CloneArchive(archive));
                    return ActionResult.Blocked("power_loss_corruption", "archaeology.power_loss_corruption");
                }
                return ActionResult.Blocked("no_power", "archaeology.no_power");
            }

            if (archive.decryptionProgress <= 0f)
                OnDecryptionStarted?.Invoke(CloneArchive(archive));

            float keyMultiplier = hasKeycard ? 1.5f : 1f;
            double workStep = (double)hours * engineerSkill * keyMultiplier * 10d;
            archive.decryptionProgress = AddProgress(archive.decryptionProgress, (float)Math.Min(workStep, 100d));

            if (archive.decryptionProgress >= 100f)
            {
                archive.encrypted = false;
                archive.unlocked = true;
                AddUniqueId(_state.unlockedLoreIds, normalizedId);

                if (!archive.researchClaimed)
                {
                    archive.researchClaimed = true;
                    _researchSystem?.UnlockManual(normalizedId);
                    OnLoreUnlocked?.Invoke(CloneArchive(archive), archive.researchPoints);
                }
            }

            return ActionResult.Success("archaeology.decryption_progressed");
        }

        public ActionResult SellArchiveToBroker(string archiveId)
        {
            string normalizedId = NormalizeId(archiveId);
            var archive = FindArchive(normalizedId);
            if (archive == null) return ActionResult.Blocked("archive_not_found", "archaeology.archive_not_found");
            if (!archive.unlocked) return ActionResult.Blocked("not_decrypted", "archaeology.not_decrypted");
            if (archive.sold || ContainsId(_state.soldArchiveIds, normalizedId))
                return ActionResult.Blocked("already_sold", "archaeology.already_sold");

            int payout = ToBrokerPayout(archive.brokerValue);
            if (payout <= 0)
                return ActionResult.Blocked("invalid_broker_value", "archaeology.invalid_broker_value");
            if (!_inventory.AddById("scrap_metal", payout))
                return ActionResult.Blocked("broker_payment_failed", "archaeology.broker_payment_failed");

            archive.sold = true;
            AddUniqueId(_state.soldArchiveIds, normalizedId);
            OnArchiveSold?.Invoke(CloneArchive(archive), payout);
            return ActionResult.Success("archaeology.sold",
                new Dictionary<string, double> { { "broker_value", payout } });
        }

        public ArchaeologyState CaptureState() => NormalizeState(_state);

        public void RestoreState(ArchaeologyState state)
        {
            if (state == null) return;
            _state = NormalizeState(state);
            _siteCounter = DeriveSiteCounter(_state.sites);
        }

        private static PreWarArchiveInstance CreateArchive(string archiveId, LoreArchiveDef? definition)
        {
            return new PreWarArchiveInstance
            {
                archiveId = NormalizeId(archiveId),
                titleKey = NormalizeText(definition?.title_key, archiveId),
                summaryKey = NormalizeText(definition?.summary_key, string.Empty),
                encryptionTier = Math.Max(1, definition?.encryption_tier ?? 1),
                decryptionProgress = 0f,
                encrypted = true,
                corrupted = false,
                unlocked = false,
                sold = false,
                researchClaimed = false,
                researchPoints = Math.Max(0, definition?.research_reward ?? 20),
                brokerValue = SanitizeBrokerValue(definition?.broker_value ?? 100f)
            };
        }

        private static LoreArchiveDef CreateFallbackArchive() => new LoreArchiveDef
        {
            archive_id = "lore_archive_silo_manifest",
            title_key = "Strategic Missile Silo Armament Ledger",
            summary_key = "Pre-war manifest detailing payload yields and launch grids.",
            encryption_tier = 2,
            required_engineering = 3f,
            base_work_hours = 16f,
            power_kw = 2.5f,
            research_reward = 25,
            broker_value = 150f
        };

        private static LoreArchiveDef NormalizeDefinition(LoreArchiveDef source) => new LoreArchiveDef
        {
            archive_id = NormalizeId(source.archive_id),
            title_key = NormalizeText(source.title_key, string.Empty),
            summary_key = NormalizeText(source.summary_key, string.Empty),
            era = NormalizeText(source.era, "PreExchange"),
            topic_tags = source.topic_tags?
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Select(tag => tag.Trim())
                .Distinct(IdentityComparer)
                .ToList() ?? new List<string>(),
            encryption_tier = Math.Max(1, source.encryption_tier),
            required_engineering = SanitizeNonNegative(source.required_engineering),
            base_work_hours = SanitizeNonNegative(source.base_work_hours),
            power_kw = SanitizeNonNegative(source.power_kw),
            corruption_risk = SanitizeProbability(source.corruption_risk),
            required_key_item_id = NormalizeText(source.required_key_item_id, string.Empty),
            research_reward = Math.Max(0, source.research_reward),
            broker_value = SanitizeBrokerValue(source.broker_value),
            unique = source.unique
        };

        private static ArchaeologyState NormalizeState(ArchaeologyState source)
        {
            var normalized = new ArchaeologyState
            {
                systemId = SystemId,
                sites = new List<ExcavationSite>(),
                archives = new List<PreWarArchiveInstance>(),
                unlockedLoreIds = new List<string>(),
                soldArchiveIds = new List<string>()
            };
            var siteIds = new HashSet<string>(IdentityComparer);
            var zones = new HashSet<string>(IdentityComparer);
            var archiveIds = new HashSet<string>(IdentityComparer);

            if (source.sites != null)
            {
                foreach (var site in source.sites)
                {
                    if (site == null) continue;
                    string siteId = NormalizeId(site.siteId);
                    string zoneId = NormalizeId(site.zoneId);
                    string archiveId = NormalizeId(site.archiveId);
                    if (string.IsNullOrWhiteSpace(siteId) || string.IsNullOrWhiteSpace(zoneId)
                        || string.IsNullOrWhiteSpace(archiveId)
                        || !siteIds.Add(siteId) || !zones.Add(zoneId)) continue;
                    float progress = SanitizeProgress(site.excavationProgress);
                    normalized.sites.Add(new ExcavationSite
                    {
                        siteId = siteId,
                        zoneId = zoneId,
                        displayName = NormalizeText(site.displayName, $"Excavation Ruin Sector {zoneId}"),
                        discovered = site.discovered,
                        excavationProgress = progress,
                        exhausted = site.exhausted && progress >= 100f,
                        archiveId = archiveId
                    });
                }
            }

            if (source.archives != null)
            {
                foreach (var archive in source.archives)
                {
                    if (archive == null) continue;
                    string archiveId = NormalizeId(archive.archiveId);
                    if (string.IsNullOrWhiteSpace(archiveId) || !archiveIds.Add(archiveId)) continue;
                    normalized.archives.Add(NormalizeArchive(archive));
                }
            }

            foreach (var archive in normalized.archives)
            {
                if (archive.unlocked) AddUniqueId(normalized.unlockedLoreIds, archive.archiveId);
                if (archive.sold) AddUniqueId(normalized.soldArchiveIds, archive.archiveId);
            }
            return normalized;
        }

        private static PreWarArchiveInstance NormalizeArchive(PreWarArchiveInstance source)
        {
            bool unlocked = source.unlocked || source.sold;
            float progress = SanitizeProgress(source.decryptionProgress);
            if (unlocked) progress = 100f;
            else if (progress >= 100f) progress = 99.9f;
            return new PreWarArchiveInstance
            {
                archiveId = NormalizeId(source.archiveId),
                titleKey = NormalizeText(source.titleKey, string.Empty),
                summaryKey = NormalizeText(source.summaryKey, string.Empty),
                encryptionTier = Math.Max(1, source.encryptionTier),
                decryptionProgress = progress,
                encrypted = !unlocked,
                corrupted = source.corrupted && !unlocked,
                unlocked = unlocked,
                sold = source.sold,
                researchClaimed = unlocked && source.researchClaimed,
                researchPoints = Math.Max(0, source.researchPoints),
                brokerValue = SanitizeBrokerValue(source.brokerValue)
            };
        }

        private static ExcavationSite CloneSite(ExcavationSite source) => new ExcavationSite
        {
            siteId = NormalizeId(source.siteId),
            zoneId = NormalizeId(source.zoneId),
            displayName = NormalizeText(source.displayName, string.Empty),
            discovered = source.discovered,
            excavationProgress = SanitizeProgress(source.excavationProgress),
            exhausted = source.exhausted,
            archiveId = NormalizeId(source.archiveId)
        };

        private static PreWarArchiveInstance CloneArchive(PreWarArchiveInstance source) => new PreWarArchiveInstance
        {
            archiveId = NormalizeId(source.archiveId),
            titleKey = NormalizeText(source.titleKey, string.Empty),
            summaryKey = NormalizeText(source.summaryKey, string.Empty),
            encryptionTier = Math.Max(1, source.encryptionTier),
            decryptionProgress = SanitizeProgress(source.decryptionProgress),
            encrypted = source.encrypted,
            corrupted = source.corrupted,
            unlocked = source.unlocked,
            sold = source.sold,
            researchClaimed = source.researchClaimed,
            researchPoints = Math.Max(0, source.researchPoints),
            brokerValue = SanitizeBrokerValue(source.brokerValue)
        };

        private ExcavationSite? FindSite(string siteId)
        {
            string normalized = NormalizeId(siteId);
            return _state.sites.FirstOrDefault(site =>
                site != null && string.Equals(site.siteId, normalized, StringComparison.OrdinalIgnoreCase));
        }

        private ExcavationSite? FindSiteByZone(string zoneId)
        {
            string normalized = NormalizeId(zoneId);
            return _state.sites.FirstOrDefault(site =>
                site != null && string.Equals(site.zoneId, normalized, StringComparison.OrdinalIgnoreCase));
        }

        private PreWarArchiveInstance? FindArchive(string archiveId)
        {
            string normalized = NormalizeId(archiveId);
            return _state.archives.FirstOrDefault(archive =>
                archive != null && string.Equals(archive.archiveId, normalized, StringComparison.OrdinalIgnoreCase));
        }

        private string NextSiteId(string zoneId)
        {
            long candidate = _siteCounter + 1;
            string siteId;
            do
            {
                siteId = $"site_{candidate}_{zoneId}";
                candidate++;
            }
            while (_state.sites.Any(site => site != null
                && string.Equals(site.siteId, siteId, StringComparison.OrdinalIgnoreCase)));
            _siteCounter = candidate - 1;
            return siteId;
        }

        private static long DeriveSiteCounter(IEnumerable<ExcavationSite> sites)
        {
            long maximum = 0;
            foreach (var site in sites)
            {
                if (site == null || !site.siteId.StartsWith("site_", StringComparison.OrdinalIgnoreCase)) continue;
                string remainder = site.siteId.Substring("site_".Length);
                int separator = remainder.IndexOf('_');
                if (separator <= 0) continue;
                if (long.TryParse(remainder.Substring(0, separator), out long value))
                    maximum = Math.Max(maximum, value);
            }
            return maximum;
        }

        private static void AddUniqueId(List<string> ids, string id)
        {
            string normalized = NormalizeId(id);
            if (string.IsNullOrWhiteSpace(normalized) || ContainsId(ids, normalized)) return;
            ids.Add(normalized);
        }

        private static bool ContainsId(IEnumerable<string> ids, string id)
        {
            string normalized = NormalizeId(id);
            return ids.Any(existing => string.Equals(NormalizeId(existing), normalized, StringComparison.OrdinalIgnoreCase));
        }

        private static float AddProgress(float current, float amount)
        {
            double projected = (double)SanitizeProgress(current) + (double)amount;
            if (!IsFinite((float)Math.Min(projected, 100d))) return 100f;
            return (float)Math.Clamp(projected, 0d, 100d);
        }

        private static float SanitizeProgress(float value) =>
            IsFinite(value) ? Math.Clamp(value, 0f, 100f) : 0f;

        private static float SanitizeNonNegative(float value) =>
            IsFinite(value) ? Math.Max(0f, value) : 0f;

        private static float SanitizeProbability(float value) =>
            IsFinite(value) ? Math.Clamp(value, 0f, 1f) : 0f;

        private static float SanitizeBrokerValue(float value) =>
            IsFinite(value) ? Math.Clamp(value, 0f, int.MaxValue) : 0f;

        private static int ToBrokerPayout(float value)
        {
            float sanitized = SanitizeBrokerValue(value);
            return sanitized >= int.MaxValue ? int.MaxValue : (int)sanitized;
        }

        private static bool IsFinite(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value);

        private static string NormalizeId(string? value) => value?.Trim() ?? string.Empty;

        private static string NormalizeText(string? value, string fallback) =>
            string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    }
}
