using System;
using System.Text;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.Endgame;
using Ashfall.Core.Legacy;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host wrapper for the expansion surfaces that were selftest-only:
    /// Waystation (Holdfast S2 vitals), Standing Record (Exp 03 layouts),
    /// Crossing gate (Exp 04 vouch), and Greenhouse (Exp 05 plots).
    /// No gameplay rules here — everything delegates to Ashfall.Core.
    /// </summary>
    public sealed class ExpansionHostSession
    {
        public const int DefaultSeed = 1117; // greenhouse + vouch demo seed

        public WaystationSystem Waystation { get; }
        public LocationLayoutSystem Layouts { get; }
        public LocationMemorySystem Memory { get; }
        public SiteEncounterSystem SiteEncounters { get; }
        public StandingRecordCatalog RecordQuests { get; }
        public VouchAccessSystem Vouch { get; }
        public GreenhouseSystem Greenhouse { get; }
        public CrossingArbitrationSystem Arbitration { get; }
        public LedgerDebtSystem Ledger { get; }
        public CrossingQuestSystem CrossingQuests { get; }
        public GenerationalSuccessionEngine Generational { get; }
        public EpilogueMatrixRuntime Epilogue { get; }
        public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
        public Ashfall.Core.Foundry.SilentFoundryCatalog FoundryData { get; private set; }
        public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
        public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }

        public ExpansionHostSession(
            WaystationSystem waystation,
            LocationLayoutSystem layouts,
            LocationMemorySystem memory,
            SiteEncounterSystem siteEncounters,
            StandingRecordCatalog recordQuests,
            VouchAccessSystem vouch,
            GreenhouseSystem greenhouse)
        {
            Waystation = waystation ?? new WaystationSystem();
            Layouts = layouts ?? new LocationLayoutSystem(
                new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            Memory = memory ?? new LocationMemorySystem(
                new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            SiteEncounters = siteEncounters ?? new SiteEncounterSystem();
            RecordQuests = recordQuests ?? new StandingRecordCatalog();
            Vouch = vouch ?? new VouchAccessSystem();
            Greenhouse = greenhouse ?? new GreenhouseSystem(DefaultSeed);
            Arbitration = new CrossingArbitrationSystem();
            Ledger = new LedgerDebtSystem();
            CrossingQuests = new CrossingQuestSystem();
            Generational = new GenerationalSuccessionEngine();
            Epilogue = new EpilogueMatrixRuntime();

            // Persistence: any hub-system state change marks the save dirty.
            Waystation.OnStateChanged += _ => StateChanged?.Invoke();
            Layouts.OnStateChanged += _ => StateChanged?.Invoke();
            Memory.OnStateChanged += _ => StateChanged?.Invoke();
            SiteEncounters.OnStateChanged += _ => StateChanged?.Invoke();
            Vouch.OnStateChanged += _ => StateChanged?.Invoke();
            Greenhouse.OnCropPlanted += (_, _, _) => StateChanged?.Invoke();
            Greenhouse.OnCropMatured += (_, _) => StateChanged?.Invoke();
            Greenhouse.OnCropHarvested += _ => StateChanged?.Invoke();
            Greenhouse.OnBlightOutbreak += _ => StateChanged?.Invoke();
            Greenhouse.OnPlotDriedOut += _ => StateChanged?.Invoke();
            Greenhouse.OnCropFailed += _ => StateChanged?.Invoke();
            Arbitration.OnStateChanged += _ => StateChanged?.Invoke();
            CrossingQuests.OnStateChanged += _ => StateChanged?.Invoke();
            // When the opening vouch quest completes, soften the gate automatically
            CrossingQuests.OnOpeningQuestCompleted += () => Vouch.SoftenAccess();
            CrossingQuests.OnStageNarrativeEmitted += evt => OnCrossingStageNarrative?.Invoke(evt);
            Generational.OnDwellerRetired += (_, _) => StateChanged?.Invoke();
            Generational.OnTraitInherited += (_, _, _) => StateChanged?.Invoke();
            Generational.OnChapterAdvanced += _ => StateChanged?.Invoke();
        }

        /// <summary>Raised when any hub-system state changes (save dirty flag).</summary>
        public event Action StateChanged;
        public event Action<CrossingStageNarrativeEvent>? OnCrossingStageNarrative;

        public static ExpansionHostSession Create(string dataDirectory, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? new GodotLog();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var layouts = new LocationLayoutSystem(files, json, log);
            layouts.Load(dataDirectory);
            var memory = new LocationMemorySystem(files, json, log);
            memory.Load(dataDirectory);
            var quests = new StandingRecordCatalogLoader(files, json, log).Load(dataDirectory);
            var crossingQuests = CrossingQuestCatalogLoader.Load(dataDirectory, files, json);

            var session = new ExpansionHostSession(
                new WaystationSystem(),
                layouts,
                memory,
                new SiteEncounterSystem(DefaultSeed),
                quests,
                new VouchAccessSystem(),
                new GreenhouseSystem(DefaultSeed));
            session.CrossingQuests.BindCatalog(crossingQuests);

            // The Silent Foundry (Exp 10): static catalogs + blueprint + treaty anchors.
            var foundryData = new Ashfall.Core.Foundry.SilentFoundryCatalog();
            foundryData.Load(
                Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadProduction(dataDirectory, files, json)!,
                Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadFaction(dataDirectory, files, json)!);
            var foundry = new Ashfall.Core.Foundry.SilentFoundrySystem(log: log);
            int maintenanceCycle = 4;
            var blueprints = new Ashfall.Core.Narrative.BunkerBlueprintCatalog();
            string bpPath = files.Combine(dataDirectory, "narrative", "bunker_blueprints_codex.json");
            if (files.FileExists(bpPath))
            {
                blueprints.Load(files.ReadAllText(bpPath), json);
                var bp = blueprints.GetById(Ashfall.Core.Foundry.SilentFoundryIds.BlueprintRoomId);
                if (bp != null && bp.maintenance_cycle_days > 0) maintenanceCycle = bp.maintenance_cycle_days;
            }
            // District 8 accords (data authority: foundry_accords.json) drive the
            // foundry's treaty clock — campaign-reachable days, Sector 4 canon.
            var ratificationDays = Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadAccordRatificationDays(
                dataDirectory, files, json);
            if (ratificationDays.Count > 0)
                foundry.BindTreaties(ratificationDays);
            foundry.BindCatalog(foundryData, maintenanceCycle);
            session.SilentFoundry = foundry;
            session.FoundryData = foundryData;
            foundry.OnStateChanged += _ => session.StateChanged?.Invoke();

            // Disease Expansion: static catalog + deterministic contagion engine.
            // Bound on the catalog (registered above); always active — outbreaks
            // threaten from day one. No unlock gate, no facility to build.
            var diseaseData = Ashfall.Core.Disease.DiseaseCatalogLoader.Load(dataDirectory, files, json);
            var disease = new Ashfall.Core.Disease.DiseaseSystem(log: log);
            disease.BindCatalog(diseaseData);
            session.Disease = disease;
            session.DiseaseData = diseaseData;
            disease.OnStateChanged += _ => session.StateChanged?.Invoke();
            return session;
        }

        // ---- Cross-host save ----

        /// <summary>Cross-host save envelope. Shape and checksum owned by ExpansionHubSaveCodec.</summary>
        public ExpansionHubSave CaptureSave(int simDay) =>
            ExpansionHubSaveCodec.Capture(simDay, Waystation, Layouts, Memory, SiteEncounters, Vouch, Greenhouse,
                Arbitration, Ledger, CrossingQuests, Generational, SilentFoundry, Disease);

        public void RestoreSave(ExpansionHubSave save) =>
            ExpansionHubSaveCodec.Restore(save, Waystation, Layouts, Memory, SiteEncounters, Vouch, Greenhouse,
                Arbitration, Ledger, CrossingQuests, Generational, SilentFoundry, Disease);

        // ---- Nobody's Charter: Crossing Arbitration (Exp 04) ----

        /// <summary>Loads the standard backer pool used by the headless demo and dev UI.</summary>
        public void LoadDefaultBackerPool()
        {
            Arbitration.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = CrossingIds.NpcOsran, displayName = "Osran Kell", wants = "a sealed contract", willNot = "forge a signature", principled = true },
                new BackerDef { id = CrossingIds.NpcMattis, displayName = "Mattis Cray", wants = "a public record", willNot = "sign a false statement", principled = true },
                new BackerDef { id = "npc_halden_mire", displayName = "Halden Mire", wants = "grain futures", willNot = "lend to a ghost", principled = true },
                new BackerDef { id = "npc_bram_ostrowski", displayName = "Bram Ostrowski", wants = "brass scrap", willNot = "deal with the Garrison directly", principled = false },
                new BackerDef { id = "npc_leva_quist", displayName = "Leva Quist", wants = "information", willNot = "be seen at the Lockup", principled = false }
            });
        }

        public string ArbitrationLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("Arbitration: ").Append(Arbitration.State.rulingsCalled).Append(" called · ")
                .Append(Arbitration.State.rulingsOverturned).Append(" overturned · ")
                .Append(Arbitration.State.standingRepeats).Append(" re-Stood");
            for (int i = 0; i < Arbitration.Rulings.Count; i++)
            {
                var r = Arbitration.Rulings[i];
                if (r == null) continue;
                sb.Append("\n  ").Append(r.topic).Append(": ").Append(r.shape)
                    .Append(" (").Append(r.backers.Count).Append(" backers)");
            }
            return sb.ToString();
        }

        public string LedgerLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("Ledger: ").Append(Ledger.Contracts.Count).Append(" open · ")
                .Append(Ledger.ClosedContracts.Count).Append(" closed · ")
                .Append(Ledger.LedgerTampered ? "TAMPERED" : "clean");
            for (int i = 0; i < Ledger.Contracts.Count; i++)
            {
                var c = Ledger.Contracts[i];
                if (c == null) continue;
                sb.Append("\n  ").Append(c.debtorId).Append(": ").Append(c.principal).Append(" (").Append(c.daysRemaining).Append("d, ")
                    .Append(c.signed ? "signed" : "draft").Append(")");
            }
            return sb.ToString();
        }

        public void UnlockWaystation() => Waystation.Unlock();
        public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
        public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
        public void ResupplyWaystation() => Waystation.Resupply();
        public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);

        public string WaystationLine()
        {
            if (!Waystation.Unlocked) return "Waystation: sealed (unlock to open bunks)";
            return
                $"Waystation: stove {(Waystation.StoveLit ? "lit" : "cold")} · " +
                $"bunks {Waystation.State.bunksOccupied}/{WaystationSystem.MaxBunks} · " +
                $"filter {Waystation.State.filterHealth:0}% · " +
                $"resupply {Waystation.State.daysSinceResupply}d ago · " +
                $"wintering {(Waystation.State.winteringClosedWindow ? "closed-window" : "normal")}";
        }

        // ---- Standing Record (Exp 03) ----

        public void UnlockRecord()
        {
            Layouts.Unlock();
            Memory.Unlock();
            SiteEncounters.Unlock();
        }

        public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);

        public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);

        public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);

        public string RoomLine(string parentId, string roomId)
        {
            var def = Layouts.GetLayout(parentId);
            if (def == null) return "no layout for " + parentId;
            var room = def.GetRoom(roomId);
            if (room == null) return "no room " + roomId;
            string dark = Layouts.IsRoomDark(parentId, roomId) ? " [dark]" : "";
            string recast = Memory.GetActiveRecast(parentId)!;
            var sb = new StringBuilder(room.displayName).Append(dark).Append("\n");
            sb.Append(room.inspect).Append('\n');
            if (!string.IsNullOrEmpty(recast) && !Layouts.IsRoomDark(parentId, roomId))
                sb.Append("NOW: ").Append(recast).Append('\n');
            return sb.ToString().TrimEnd();
        }

        public string StandingRecordLine()
        {
            var sb = new StringBuilder();
            sb.Append("Standing Record: ").Append(Layouts.LayoutCount).Append(" layouts · ")
                .Append(Memory.StratumCount).Append(" strata · ")
                .Append(RecordQuests.Quests.Count).Append(" quests · ");
            sb.Append("Overlay ").Append(SiteEncounters.OverlayAccess ? "access" : "WITHDRAWN")
                .Append(" · plates scraped ").Append(SiteEncounters.PlatesScraped);
            if (Layouts.LayoutCount > 0)
            {
                var def = Layouts.Layouts[0];
                sb.Append(" · first: ").Append(def.parentLocationId)
                    .Append(" (").Append(def.displayName).Append(", ").Append(def.RoomCount).Append(" rooms)");
            }
            return sb.ToString();
        }

        public string RecordQuestLine()
        {
            var sb = new StringBuilder("Record quests:");
            for (int i = 0; i < RecordQuests.Quests.Count && i < 5; i++)
            {
                var q = RecordQuests.Quests[i];
                sb.Append("\n  ").Append(q.id).Append(" → ").Append(q.target_location_id);
            }
            if (RecordQuests.Quests.Count > 5)
                sb.Append("\n  +").Append(RecordQuests.Quests.Count - 5).Append(" more");
            return sb.ToString();
        }

        // ---- Crossing gate (Exp 04) ----

        public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
        public bool BurnVouch() => Vouch.BurnVouch();
        public bool SoftenAccess() => Vouch.SoftenAccess();

        public string CrossingLine()
        {
            string gate = Vouch.HasAccess ? "OPEN" : "CLOSED";
            return
                $"Crossing: gate {gate} · " +
                $"vouch {(string.IsNullOrEmpty(Vouch.VouchedBy) ? "none" : Vouch.VouchedBy)} · " +
                $"burned {Vouch.VouchBurned} · softened {Vouch.AccessSoftened} · " +
                $"last resort {(Vouch.LastResortUsed ? "used" : "available")}";
        }

        // ---- Nobody's Charter: Crossing Quests (Exp 04) ----

        public bool StartCrossingQuest(string questId, int currentDay)
            => CrossingQuests.StartQuest(questId, currentDay);

        /// <summary>
        /// Idempotent daily tick for the Crossing quest auto-start.
        /// Only starts eligible quests once per calendar day; safe to call repeatedly.
        /// </summary>
        public void TickCrossingQuests(int currentDay)
            => CrossingQuests.TickDaily(currentDay, hasVouchAccess: Vouch.HasAccess);

        public int AdvanceCrossingQuestStage(string questId)
            => CrossingQuests.AdvanceStage(questId);

        public bool MakeCrossingChoice(string questId, string choiceId)
            => CrossingQuests.MakeChoice(questId, choiceId);

        public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay)
            => CrossingQuests.GetAvailableQuests(currentDay);

        public bool FailCrossingQuest(string questId)
            => CrossingQuests.FailQuest(questId);

        public bool IsCrossingQuestFailed(string questId)
            => CrossingQuests.IsQuestFailed(questId);

        public bool IsCrossingQuestCompleted(string questId)
            => CrossingQuests.IsQuestCompleted(questId);

        public string CrossingQuestLine()
        {
            var sb = new StringBuilder("Crossing quests:");
            var catalog = CrossingQuests.Catalog;
            int shown = 0;
            for (int i = 0; i < catalog.Count && shown < 5; i++)
            {
                var def = catalog[i];
                if (def == null) continue;
                var progress = CrossingQuests.GetProgress(def.id);
                string status = progress == null ? "available" :
                    progress.completed ? "done" :
                    progress.started ? $"stage {progress.currentStage}/{def.stages.Count}" : "ready";
                sb.Append("\n  ").Append(def.id).Append(" [").Append(status).Append("]");
                shown++;
            }
            if (catalog.Count > 5)
                sb.Append("\n  +").Append(catalog.Count - 5).Append(" more");
            sb.Append(" · flags ").Append(CrossingQuests.State.setFlags.Count);
            return sb.ToString();
        }

        // ---- Greenhouse (Exp 05) ----

        public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
        public bool PlantGreenhouse(int plotIndex, string seedItemId, int day)
            => Greenhouse.Plant(plotIndex, seedItemId, day, out _);
        public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
        public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
        public void TickGreenhouse(int simDay) =>
            Greenhouse.TickDay(simDay, growLightHours: 6f, ashContaminationRate: 0.02f);

        public string GreenhouseLine()
        {
            var sb = new StringBuilder();
            sb.Append("Greenhouse: ").Append(Greenhouse.PlotCount).Append(" plots · ");
            sb.Append(Greenhouse.TotalHarvests).Append(" harvests · ");
            sb.Append(Greenhouse.IsPreWarWheatUnlocked ? "wheat unlocked" : "wheat locked");
            sb.Append(" · [");
            for (int i = 0; i < Greenhouse.PlotCount; i++)
            {
                if (i > 0) sb.Append(" ");
                var p = Greenhouse.State.plots[i];
                string seed = string.IsNullOrEmpty(p.seedItemId) ? "fallow" : p.seedItemId.Replace("item_", "");
                sb.Append(i).Append(":").Append(seed).Append("/").Append(p.stage);
            }
            sb.Append("]");
            return sb.ToString();
        }

        // ---- Generational Succession (Exp 12) ----

        public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0)
            => Generational.RegisterDweller(dwellerId, age, generation);

        public string AdvanceGenerationalTime(int days)
        {
            Generational.AdvanceTime(days);
            return $"Advanced {days}d. Chapter {Generational.CurrentChapterIndex}, " +
                   $"year {Generational.TotalYearsElapsed}.";
        }

        public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId)
        {
            return Generational.FormMentorship(mentorId, apprenticeId, traitId)
                ? $"Mentorship formed: {mentorId} → {apprenticeId} ({traitId})."
                : "Mentorship refused (invalid or deceased).";
        }

        public string GenerationalLine()
        {
            var save = Generational.CaptureState();
            return $"Generational: ch {Generational.CurrentChapterIndex} · " +
                   $"year {Generational.TotalYearsElapsed} · " +
                   $"{save.generationRecords.Count} dwellers";
        }

        public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
        public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);

        // ---- Epilogue Matrix (Endgame) ----

        public string EvaluateEpilogueDemo(int days, int living, int deaths,
            bool treaty, bool tempest, bool burned, bool children)
        {
            var ctx = new EpilogueEvaluationContext
            {
                totalDaysSurvived = days,
                livingDwellerCount = living,
                totalDeathsRecorded = deaths,
                grandTreatySigned = treaty,
                tempestDecommissioned = tempest,
                debtLedgersBurned = burned,
                childrenSurvived = children
            };
            var fate = Epilogue.EvaluateRegionalFate(ctx);
            var demo = Epilogue.EvaluateDemographics(ctx);
            var moral = Epilogue.EvaluateMoralStanding(ctx);
            return $"Epilogue: {fate} / {demo} / {moral}";
        }

        public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx)
            => Epilogue.GenerateEpilogueNarrative(ctx);
    }
}
