using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for THE SILENT FOUNDRY (Expansion 10).
    /// Loads the static catalogs (foundry_production.json, foundry_items.json,
    /// foundry_faction.json), wires the Core SilentFoundrySystem to the shared
    /// inventory and the journal bridge, and exposes thin commands for the UI.
    /// No gameplay rules here — hosts only present.
    /// </summary>
    public sealed class SilentFoundryHostSession
    {
        public const int DefaultSeed = 1009;

        public SilentFoundrySystem Engine { get; }
        public SilentFoundryCatalog Catalog { get; }
        public ItemCatalog FoundryItems { get; }
        public SilentFoundryConsequencePolicyCatalog ConsequencePolicy { get; }
        public FactionStanceEngine GuildStanceEngine { get; }

        public string LastEvent { get; set; } = string.Empty;
        public event Action? StateChanged;

        private readonly JournalSystem? _journal;
        private readonly NarrativeBatchCatalog _journalTemplates = new NarrativeBatchCatalog();
        private readonly InventoryContainer _inventory;
        private readonly ItemCatalog _inventoryCatalog;
        private readonly MarketSystem? _market;

        private SilentFoundryHostSession(
            SilentFoundrySystem engine,
            SilentFoundryCatalog catalog,
            ItemCatalog foundryItems,
            InventoryContainer inventory,
            ItemCatalog inventoryCatalog,
            JournalSystem? journal,
            MarketSystem? market,
            SilentFoundryConsequencePolicyCatalog consequencePolicy,
            ILog log)
        {
            Engine = engine;
            Catalog = catalog;
            FoundryItems = foundryItems;
            _inventory = inventory;
            _inventoryCatalog = inventoryCatalog;
            _journal = journal;
            _market = market;
            ConsequencePolicy = consequencePolicy;
            Engine.BindConsequencePolicy(consequencePolicy);

            // The Foundry Guild is registered with the existing stance engine
            // (no alias, no second standing system). Its trust is derived from
            // the Core consequence ledger; the ledger is the save authority.
            GuildStanceEngine = new FactionStanceEngine();
            GuildStanceEngine.RegisterFaction(new FactionThresholds(
                SilentFoundryIds.FactionId,
                raidThreshold: -50f,
                robThreshold: -20f,
                minTrustToTrade: -40f,
                intelShareThreshold: 40f));
            SyncGuildStanding();

            Engine.OnConsequenceApplied += ApplyConsequenceToSurfaces;

            Engine.BindInventory(
                id => _inventory.CountById(id),
                (id, amount) => _inventoryCatalog.Get(id) != null,
                (id, amount) =>
                {
                    var def = _inventoryCatalog.Get(id);
                    if (def != null) _inventory.Add(def, amount);
                },
                (id, amount) =>
                {
                    var def = _inventoryCatalog.Get(id);
                    if (def != null) _inventory.Remove(def, amount);
                });

            Engine.OnStateChanged += _ => StateChanged?.Invoke();
            Engine.OnProductionCompleted += _ => { LastEvent = "Cast complete. Output stored."; StateChanged?.Invoke(); };
            Engine.OnCastFailed += f => { LastEvent = "Cast failed: " + f.reason; StateChanged?.Invoke(); };
            Engine.OnIncident += i => { LastEvent = "INCIDENT: " + i.summary; StateChanged?.Invoke(); };
            Engine.OnTreatyQuotaMet += c => { LastEvent = "Treaty quota met: " + c.treatyId; StateChanged?.Invoke(); };
            Engine.OnTreatyQuotaMissed += c => { LastEvent = "Treaty quota missed: " + c.treatyId; StateChanged?.Invoke(); };
            Engine.OnJournalTriggered += BridgeJournalTrigger;

            // The authored journal templates stay the source of the narrative text.
            string dataDir = ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data");
            string jrnlPath = Path.Combine(dataDir, "narrative", "jrnl_templates_cycle_d.json");
            if (System.IO.File.Exists(jrnlPath))
            {
                try
                {
                    _journalTemplates.LoadJournalBatch(System.IO.File.ReadAllText(jrnlPath), new SystemTextJsonSerializer());
                }
                catch (Exception e)
                {
                    GD.PrintErr("[SilentFoundry] journal template load failed: " + e.Message);
                }
            }
        }

        public static SilentFoundryHostSession Create(
            string dataDir,
            ExpansionHostSession expansions,
            InventoryHostSession inventory,
            JournalSystem? journal = null,
            MarketSystem? market = null,
            ILog? log = null)
        {
            log = log ?? new GodotLog();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var engine = expansions.SilentFoundry;
            var catalog = expansions.FoundryData;
            if (engine == null || catalog == null)
            {
                // Standalone fallback (tests / hosts that skip the hub): build a
                // fresh engine bound to the static catalogs + blueprint + treaties.
                var built = BuildStandalone(dataDir, files, json, log);
                engine = built.Engine;
                catalog = built.Catalog;
            }

            // Authored consequence policy (data authority: foundry_treaty_consequences.json).
            var policyCatalog = new SilentFoundryConsequencePolicyCatalog();
            policyCatalog.Load(SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json));

            // Foundry item definitions → shared inventory catalog (data authority: foundry_items.json).
            var foundryItems = LoadFoundryItems(dataDir, files, json);
            foreach (string id in foundryItems.Ids)
            {
                var def = foundryItems.Get(id);
                if (def != null) inventory.Catalog.Register(def);
            }
            EnsureChargeMaterials(inventory.Catalog);

            var session = new SilentFoundryHostSession(engine, catalog, foundryItems,
                inventory.Inventory, inventory.Catalog, journal, market, policyCatalog, log);
            SeedFoundrySupplies(inventory);
            return session;
        }

        /// <summary>Expose the current guild stance/trust for presentation.</summary>
        public float GuildTrust => Engine.GuildStanding;
        public TradeStance GuildStance => GuildStanceEngine.GetStance(SilentFoundryIds.FactionId);

        /// <summary>
        /// Mirror the authoritative Core standing into the existing stance engine
        /// (SetTrust, never ModifyTrust on restore — no double counting).
        /// </summary>
        public void SyncGuildStanding()
        {
            GuildStanceEngine.SetTrust(SilentFoundryIds.FactionId, Engine.GuildStanding);
        }

        /// <summary>
        /// Apply a Core consequence record to the real economy surfaces exactly
        /// once: standing into the existing FactionStanceEngine, market/logistics
        /// modifiers into the existing MarketSystem demand path. The Core ledger
        /// already guarantees once-per-cycle; this only mirrors it outward.
        /// </summary>
        private void ApplyConsequenceToSurfaces(FoundryConsequenceRecord record)
        {
            if (record == null) return;

            if (Math.Abs(record.standingDelta) > 1e-6f)
            {
                GuildStanceEngine.ModifyTrust(SilentFoundryIds.FactionId, record.standingDelta);
            }

            if (_market != null && record.modifiers != null)
            {
                for (int i = 0; i < record.modifiers.Count; i++)
                {
                    var m = record.modifiers[i];
                    if (m == null || string.IsNullOrEmpty(m.good_id)) continue;
                    if (_market.FindGood(m.good_id) == null)
                    {
                        GD.PrintErr($"[SilentFoundry] consequence references unknown good '{m.good_id}'; skipped.");
                        continue;
                    }
                    _market.AdjustDemand(m.good_id, m.demand_delta);
                }
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("Consequence: ").Append(record.treatyId).Append(" ")
              .Append(SilentFoundryConsequencePolicyCatalog.OutcomeName(record.outcome))
              .Append(" · standing ").Append(record.standingDelta.ToString("+0;-0;0"))
              .Append(" · guild ").Append(Engine.GuildStanding.ToString("F0"));
            if (record.modifiers != null)
            {
                for (int i = 0; i < record.modifiers.Count; i++)
                {
                    var m = record.modifiers[i];
                    if (m == null) continue;
                    sb.Append(" · ").Append(m.good_id).Append(" demand ").Append(m.demand_delta.ToString("+0.00;-0.00"));
                }
            }
            LastEvent = sb.ToString();
            StateChanged?.Invoke();
        }

        /// <summary>Standalone engine build (used when the expansion hub has none).</summary>
        private static (SilentFoundrySystem Engine, SilentFoundryCatalog Catalog) BuildStandalone(
            string dataDir, IFileIO files, IJsonSerializer json, ILog log)
        {
            var catalog = new SilentFoundryCatalog();
            catalog.Load(
                SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json),
                SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json));

            int maintenanceCycle = 4;
            var blueprints = new BunkerBlueprintCatalog();
            string bpPath = files.Combine(dataDir, "narrative", "bunker_blueprints_codex.json");
            if (files.FileExists(bpPath))
            {
                blueprints.Load(files.ReadAllText(bpPath), json);
                var bp = blueprints.GetById(SilentFoundryIds.BlueprintRoomId);
                if (bp != null && bp.maintenance_cycle_days > 0) maintenanceCycle = bp.maintenance_cycle_days;
            }

            var engine = new SilentFoundrySystem(log: log);
            // District 8 accords (foundry_accords.json) drive the treaty clock.
            var ratificationDays = SilentFoundryCatalogLoader.LoadAccordRatificationDays(dataDir, files, json);
            if (ratificationDays.Count > 0)
                engine.BindTreaties(ratificationDays);
            engine.BindCatalog(catalog, maintenanceCycle);
            return (engine, catalog);
        }

        /// <summary>Register the charge materials the Foundry consumes (canonical items.json ids).</summary>
        private static void EnsureChargeMaterials(ItemCatalog catalog)
        {
            if (catalog.Get(SilentFoundryIds.ItemScrapMetal) == null)
                catalog.Register(new ItemDefinition
                {
                    id = SilentFoundryIds.ItemScrapMetal, displayName = "Scrap Metal",
                    type = ItemType.Material, stackMax = 50, weight = 0.4f, tradeValue = 1f
                });
            if (catalog.Get(SilentFoundryIds.ItemCoal) == null)
                catalog.Register(new ItemDefinition
                {
                    id = SilentFoundryIds.ItemCoal, displayName = "Coal",
                    type = ItemType.Fuel, stackMax = 50, weight = 0.9f, tradeValue = 2f
                });
            if (catalog.Get(SilentFoundryIds.ItemCharcoal) == null)
                catalog.Register(new ItemDefinition
                {
                    id = SilentFoundryIds.ItemCharcoal, displayName = "Charcoal",
                    type = ItemType.Fuel, stackMax = 50, weight = 0.5f, tradeValue = 1f
                });
        }

        /// <summary>Read foundry_items.json into an ItemCatalog (same schema as items.json).</summary>
        private static ItemCatalog LoadFoundryItems(string dataDir, IFileIO files, IJsonSerializer json)
        {
            var catalog = new ItemCatalog();
            string path = files.Combine(dataDir, "foundry_items.json");
            if (!files.FileExists(path)) return catalog;
            try
            {
                var defs = json.Deserialize<List<FoundryItemJson>>(files.ReadAllText(path));
                if (defs == null) return catalog;
                for (int i = 0; i < defs.Count; i++)
                {
                    var d = defs[i];
                    if (d == null || string.IsNullOrEmpty(d.id)) continue;
                    if (!Enum.TryParse(d.type, ignoreCase: true, out ItemType type)) type = ItemType.Material;
                    catalog.Register(new ItemDefinition
                    {
                        id = d.id,
                        displayName = d.displayName,
                        description = d.description,
                        type = type,
                        stackMax = d.stackMax > 0 ? d.stackMax : 1,
                        weight = d.weight,
                        tradeValue = d.tradeValue,
                        durability = d.durability > 0 ? d.durability : 100f
                    });
                }
            }
            catch (Exception e)
            {
                GD.PrintErr("[SilentFoundry] foundry_items.json load failed: " + e.Message);
            }
            return catalog;
        }

        /// <summary>Seed a modest starter stock of charge materials into the shared inventory.</summary>
        private static void SeedFoundrySupplies(InventoryHostSession inventory)
        {
            // Capacity-bounded shared container: keep to a few slots; the rest is
            // gathered through expeditions and trade, like every other material.
            inventory.Add(SilentFoundryIds.ItemScrapMetal, 12);
            inventory.Add(SilentFoundryIds.ItemCoal, 12);
            inventory.Add(SilentFoundryIds.ItemCleanWater, 6);
            inventory.Add(SilentFoundryIds.ItemFlux, 3);
        }

        /// <summary>Bridge a Core journal trigger to the real journal system (once-only via knowledge key).</summary>
        private void BridgeJournalTrigger(FoundryJournalTrigger trigger)
        {
            if (_journal == null) return;
            if (trigger == null) return;

            string body = string.Empty;
            string authorRole = string.Empty;
            var template = _journalTemplates.JournalTemplates.TryGetValue(trigger.TemplateId, out var t) ? t : null;
            if (template != null)
            {
                body = template.body_template ?? string.Empty;
                authorRole = template.author_role ?? string.Empty;
            }
            if (string.IsNullOrEmpty(body))
            {
                body = trigger.TemplateId == SilentFoundryIds.JournalFirstHeat
                    ? "The first successful heat is poured. New iron, not scrap."
                    : "The charging floor has stopped. The strike is real.";
            }

            // Preserve the authored author role in the journal entry (the template
            // remains the text authority; the role becomes the entry's author).
            ISurvivorAuthor? author = null;
            if (!string.IsNullOrEmpty(authorRole))
            {
                author = new FoundryJournalAuthor(trigger.TemplateId, authorRole);
            }

            // The template id doubles as the knowledge key: KnowledgeBase dedupes,
            // so reloading a save can never inject a duplicate entry.
            _journal.TryAddRawEntry(trigger.TemplateId, body, author, trigger.Day);
        }

        /// <summary>Minimal author surface so the journal preserves the authored role.</summary>
        private sealed class FoundryJournalAuthor : ISurvivorAuthor
        {
            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;

            public FoundryJournalAuthor(string templateId, string role)
            {
                Id = templateId;
                DisplayName = char.ToUpperInvariant(role[0]) + role.Substring(1); // foundryman -> Foundryman
            }
        }

        // ---- Thin commands for the UI ----

        public string Unlock(int day) => Engine.Unlock(day) ? "The Silent Foundry is open." : "Already open.";
        public string Repair(FoundryFacilityComponent component, int day) => Engine.StartRepair(component, day);
        public string Maintain(int day) => Engine.PerformMaintenance(day);
        public string PrepareSand(int water) => Engine.PrepareSand(water);
        public string CompactMold() => Engine.CompactMold(0.6f);
        public string StartHeat(string productId, int workers, float skill, int day) => Engine.StartProduction(productId, workers, skill, day);
        public string Tap(int day) => Engine.TapAndCast(day);
        public string SetOvertime(bool on) { Engine.SetOvertime(on); return on ? "Overtime ordered." : "Overtime rescinded."; }
        public string SetChildLabor(bool on) { Engine.SetChildLaborUsed(on); return on ? "Children sent to the charging floor." : "Children returned to lessons."; }
        public string OpenDispute(int day) => Engine.BeginLaborDispute(day);
        public string ResolveStrike(FoundryStrikeResolution resolution, int day) => Engine.ResolveStrike(resolution, day);

        public string StatusLine()
        {
            var s = Engine.State;
            return $"FOUNDRY: {(s.unlocked ? "OPEN" : "SEALED")} · heat {Engine.HeatStage} · "
                + $"hearth {s.hearthTuyeres:F0}/100 · maintenance {(Engine.IsMaintenanceOverdue ? "OVERDUE " + Engine.DaysOverdue + "d" : (s.maintenanceDueDay > 0 ? "due d" + s.maintenanceDueDay : "unscheduled"))} · "
                + $"casts {Engine.TotalProductionCount} · failed {Engine.TotalFailedCount} · "
                + $"labor {Engine.LaborDispute} · hope {Engine.CumulativeHope:F0}";
        }
    }

    /// <summary>foundry_items.json row (same camelCase schema as items.json).</summary>
    public sealed class FoundryItemJson
    {
        public string? id;
        public string? displayName;
        public string? description;
        public string? type;
        public int stackMax = 1;
        public float weight;
        public float tradeValue;
        public float durability;
    }
}
