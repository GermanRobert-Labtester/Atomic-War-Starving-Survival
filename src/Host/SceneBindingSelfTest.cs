// SPDX-License-Identifier: MIT
// ASHFALL — SceneBindingSelfTest.cs (Ticket #125 verb driver).
//
// Implements --scene-binding-selftest, which loads every production scene
// declared in PanelSceneLoader.Load<R>(res://assets/ui/panels/<Name>.tscn)
// where R is one of the migrated detail panels, then resolves each
// scene's typed unique-name node contract via SceneBinder.
//
// Unlike src/UI/SceneBindingHeadlessProbe (whose Register/Run have no callers),
// this verb owns its own scene + contract table, so the whole check lives in one
// self-contained file that the existing ProjectBuild coverage path picks up. The
// RootType column is asserted against the scene root's actual Script — a plain
// unique-name contract check cannot catch a request/root type mismatch, which is
// how a scene could pass here yet throw InvalidCastException in production.

using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp;

public static class SceneBindingSelfTest
{
    /// <summary>
    /// Production-scene contract. Lists each unique name and the typed
    /// SceneBinder.Require<T> the panel class calls. We replicate the same
    /// names with their declared types here so the headless probe checks
    /// the contract without booting the full CampaignCoordinator.
    /// </summary>
    public sealed class SceneCheck
    {
        public string ResPath { get; init; } = string.Empty;
        public Type RootType { get; init; } = typeof(Control);
        public List<(string name, Type type)> Contract { get; } = new();
    }

    private static readonly List<SceneCheck> _checks = new();

    public static void Check(string resPath, Type rootType, params (string, Type)[] contract)
    {
        var sc = new SceneCheck { ResPath = resPath, RootType = rootType };
        sc.Contract.AddRange(contract);
        _checks.Add(sc);
    }

    /// <summary>
    /// Walk the migration registry once at boot. The contract for each
    /// panel matches the unique-name keys declared in the corresponding
    /// .tscn. If a scene is regenerated or restructured, the matching
    /// C# class's _Ready block must be re-checked against this list.
    /// </summary>
    public static void RegisterMigratedPanels()
    {
        Check("res://assets/ui/panels/InventoryDetailPanel.tscn", typeof(InventoryDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("Info", typeof(VBoxContainer)),
            ("Stats", typeof(VBoxContainer)),
            ("Actions", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/AfflictionsPanel.tscn", typeof(AfflictionsPanel),
            ("Backdrop", typeof(ColorRect)),
            ("ActiveList", typeof(VBoxContainer)),
            ("ChronicList", typeof(VBoxContainer)),
            ("TreatmentList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/SurvivorDetailPanel.tscn", typeof(SurvivorDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("SurvivorInfo", typeof(VBoxContainer)),
            ("NeedsList", typeof(VBoxContainer)),
            ("TraitsList", typeof(VBoxContainer)),
            ("StatusList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/WeatherDetailPanel.tscn", typeof(WeatherDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("CurrentList", typeof(VBoxContainer)),
            ("ForecastList", typeof(VBoxContainer)),
            ("WindList", typeof(VBoxContainer)),
            ("TrendList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/QuestDetailPanel.tscn", typeof(QuestDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("InfoContainer", typeof(VBoxContainer)),
            ("StagesContainer", typeof(VBoxContainer)),
            ("ChoicesContainer", typeof(VBoxContainer)),
            ("RewardsContainer", typeof(VBoxContainer)),
            ("Title", typeof(Label)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/MapDetailPanel.tscn", typeof(MapDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("InfoContainer", typeof(VBoxContainer)),
            ("HazardsContainer", typeof(VBoxContainer)),
            ("LayoutsContainer", typeof(VBoxContainer)),
            ("SalvageContainer", typeof(VBoxContainer)),
            ("Title", typeof(Label)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/RadiationDetailPanel.tscn", typeof(RadiationDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("CurrentData", typeof(VBoxContainer)),
            ("DosimeterData", typeof(VBoxContainer)),
            ("ProtectionData", typeof(VBoxContainer)),
            ("EventsList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/EconomyDetailPanel.tscn", typeof(EconomyDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("ResourcesList", typeof(VBoxContainer)),
            ("TradeList", typeof(VBoxContainer)),
            ("MarketList", typeof(VBoxContainer)),
            ("DebtList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/CombatDetailPanel.tscn", typeof(CombatDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("BattleInfo", typeof(VBoxContainer)),
            ("TacticsData", typeof(VBoxContainer)),
            ("CasualtyData", typeof(VBoxContainer)),
            ("OutcomesData", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/FactionDetailPanel.tscn", typeof(FactionDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("InfoContainer", typeof(VBoxContainer)),
            ("DiplomacyContainer", typeof(VBoxContainer)),
            ("TradeContainer", typeof(VBoxContainer)),
            ("EventsContainer", typeof(VBoxContainer)),
            ("Title", typeof(Label)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/JournalDetailPanel.tscn", typeof(JournalDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("EntriesList", typeof(VBoxContainer)),
            ("CodexList", typeof(VBoxContainer)),
            ("TabsList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/EventDetailPanel.tscn", typeof(EventDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("EventInfoList", typeof(VBoxContainer)),
            ("HistoryList", typeof(VBoxContainer)),
            ("NarrativeList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/DutyRosterDetailPanel.tscn", typeof(DutyRosterDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("AssignmentsList", typeof(VBoxContainer)),
            ("ShiftsList", typeof(VBoxContainer)),
            ("PerformanceList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/SurvivalDetailPanel.tscn", typeof(SurvivalDetailPanel),
            ("Backdrop", typeof(ColorRect)),
            ("HealthData", typeof(VBoxContainer)),
            ("NeedsData", typeof(VBoxContainer)),
            ("RadiationData", typeof(VBoxContainer)),
            ("StatusData", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/WorkshopPanel.tscn", typeof(WorkshopPanel),
            ("RelicListContainer", typeof(VBoxContainer)),
            ("DetailContainer", typeof(VBoxContainer)),
            ("JobHeader", typeof(Label)),
            ("JobProgressBar", typeof(ProgressBar)),
            ("JobDetails", typeof(Label)),
            ("CancelJobButton", typeof(Button)),
            ("CloseButton", typeof(Button))
        );
        Check("res://assets/ui/panels/RadioIntelligencePanel.tscn", typeof(RadioIntelligencePanel),
            ("CloseButton", typeof(Button)),
            ("ScanButton", typeof(Button)),
            ("FrequencyLabel", typeof(Label)),
            ("BandLabel", typeof(Label)),
            ("SignalMeter", typeof(ProgressBar)),
            ("Oscilloscope", typeof(ColorRect)),
            ("DecryptButton", typeof(Button)),
            ("DecryptProgressBar", typeof(ProgressBar)),
            ("RecordBearingButton", typeof(Button)),
            ("BearingRadar", typeof(Control)),
            ("SosContainer", typeof(VBoxContainer)),
            ("InterceptListContainer", typeof(VBoxContainer)),
            ("StatusLabel", typeof(Label))
        );
        Check("res://assets/ui/panels/ShelterSocialPanel.tscn", typeof(ShelterSocialPanel),
            ("CloseButton", typeof(Button)),
            ("RoomListContainer", typeof(VBoxContainer)),
            ("SurvivorListContainer", typeof(VBoxContainer)),
            ("RelationsContainer", typeof(VBoxContainer)),
            ("DisputeContainer", typeof(VBoxContainer)),
            ("MediateButton", typeof(Button)),
            ("GatheringButton", typeof(Button)),
            ("MemorialContainer", typeof(VBoxContainer)),
            ("HistoryContainer", typeof(VBoxContainer)),
            ("StatusLabel", typeof(Label))
        );
        Check("res://assets/ui/panels/SubterraneanOperationsPanel.tscn", typeof(SubterraneanOperationsPanel),
            ("CloseButton", typeof(Button)),
            ("SectorSelector", typeof(OptionButton)),
            ("MethaneMeter", typeof(ProgressBar)),
            ("FloodMeter", typeof(ProgressBar)),
            ("SporeMeter", typeof(ProgressBar)),
            ("ShoringMeter", typeof(ProgressBar)),
            ("MitigationListContainer", typeof(VBoxContainer)),
            ("BulkheadToggleButton", typeof(Button)),
            ("RescueContainer", typeof(VBoxContainer)),
            ("RescueProgressBar", typeof(ProgressBar)),
            ("RescueLaborButton", typeof(Button)),
            ("StatusLabel", typeof(Label))
        );
        Check("res://assets/ui/panels/CraftingPanel.tscn", typeof(CraftingPanel),
            ("RecipeList", typeof(VBoxContainer)),
            ("QueueList", typeof(VBoxContainer)),
            ("QueueHeader", typeof(Label)),
            ("FilterStatus", typeof(Label)),
            ("CloseButton", typeof(Button)),
            ("FilterAllButton", typeof(Button)),
            ("FilterCraftableButton", typeof(Button)),
            ("RelicWorkshopButton", typeof(Button)),
            ("PharmaLabButton", typeof(Button))
        );
        // ── Designer mirrors (not scene-bound in production) ──────────────
        // KitchenNutritionPanel, OpeningProtocolModal, SafeCrackModal, PharmaLabPanel
        // and DailyBriefingModal are layout mirrors of surfaces whose real UI is built
        // in C#: each constructs its own tree in _Ready and is created with `new`.
        // Their scene roots carry no Script, so the declared RootType is Control.
        // WaterTreatmentPanel.tscn (interleaved below) is the one genuine scene-bound
        // surface: WaterTreatmentPanel._Ready loads it as WaterTreatmentPanelContent
        // and binds ContentStack/DetailText/the four batch buttons via SceneBinder.
        Check("res://assets/ui/panels/KitchenNutritionPanel.tscn", typeof(Control),
            ("RecipeList", typeof(VBoxContainer)),
            ("PrepStation", typeof(VBoxContainer)),
            ("ServiceLogContainer", typeof(VBoxContainer)),
            ("EventLogLabel", typeof(Label))
        );
        Check("res://assets/ui/panels/WaterTreatmentPanel.tscn", typeof(WaterTreatmentPanelContent),
            ("ContentStack", typeof(VBoxContainer)),
            ("DetailText", typeof(Label)),
            ("CharcoalButton", typeof(Button)),
            ("DistillButton", typeof(Button)),
            ("OsmosisButton", typeof(Button)),
            ("ReplaceFilterButton", typeof(Button))
        );
        Check("res://assets/ui/panels/PharmaLabPanel.tscn", typeof(Control),
            ("RecipeListContainer", typeof(VBoxContainer)),
            ("DetailContainer", typeof(VBoxContainer)),
            ("LabStatusHeader", typeof(Label)),
            ("DistillationProgressBar", typeof(ProgressBar)),
            ("PhaseMetricsLabel", typeof(Label)),
            ("CancelBatchButton", typeof(Button)),
            ("CloseButton", typeof(Button)),
            ("CatAll", typeof(Button)),
            ("CatChelator", typeof(Button)),
            ("CatPsychotropic", typeof(Button)),
            ("CatStimulant", typeof(Button)),
            ("CatEmergency", typeof(Button)),
            ("CatAnesthetic", typeof(Button)),
            ("CatAntibiotic", typeof(Button)),
            ("CatAntiseptic", typeof(Button))
        );
        Check("res://assets/ui/modals/OpeningProtocolModal.tscn", typeof(Control),
            ("RationStatus", typeof(Label)),
            ("MaintenanceStatus", typeof(Label)),
            ("RadioStatus", typeof(Label)),
            ("LogList", typeof(VBoxContainer)),
            ("CloseButton", typeof(Button)),
            ("RationStandardButton", typeof(Button)),
            ("RationHalfButton", typeof(Button)),
            ("RationIrradiatedButton", typeof(Button)),
            ("MaintServiceButton", typeof(Button)),
            ("MaintLeadBunkButton", typeof(Button)),
            ("MaintCalibrateButton", typeof(Button)),
            ("RadioAckButton", typeof(Button)),
            ("RadioSilenceButton", typeof(Button)),
            ("RadioBeaconButton", typeof(Button))
        );
        Check("res://assets/ui/modals/SafeCrackModal.tscn", typeof(Control),
            ("HeaderLabel", typeof(Label)),
            ("SafeInfoLabel", typeof(Label)),
            ("DifficultyLabel", typeof(Label)),
            ("AttemptsLabel", typeof(Label)),
            ("NoiseLabel", typeof(Label)),
            ("ToolLabel", typeof(Label)),
            ("FeedbackLabel", typeof(Label)),
            ("LootLabel", typeof(Label)),
            ("Tumbler0", typeof(SpinBox)),
            ("Tumbler1", typeof(SpinBox)),
            ("Tumbler2", typeof(SpinBox)),
            ("Tumbler3", typeof(SpinBox)),
            ("Tumbler4", typeof(SpinBox)),
            ("Tumbler5", typeof(SpinBox)),
            ("AttemptButton", typeof(Button)),
            ("AccessibleButton", typeof(Button)),
            ("TransferLootButton", typeof(Button)),
            ("AbandonButton", typeof(Button))
        );
        Check("res://assets/ui/modals/DailyBriefingModal.tscn", typeof(Control),
            ("TitleLabel", typeof(Label)),
            ("BodyLabel", typeof(RichTextLabel)),
            ("AckLabel", typeof(Label)),
            ("AckButton", typeof(Button)),
            ("SkipButton", typeof(Button)),
            ("Scroll", typeof(ScrollContainer))
        );
    }

    public static int Run()
    {
        RegisterMigratedPanels();
        int passed = 0, failed = 0;
        var report = new StringBuilder();
        report.AppendLine("[SCENE_BIND_REPORT] scene | expected loader type | actual root type | attached script");
        foreach (var sc in _checks)
        {
            Node? root = null;
            try
            {
                root = PanelSceneLoader.Load<Node>(sc.ResPath);
                if (!(root is Control c))
                    throw new InvalidCastException("scene root is not Control");
                string scriptPath = AttachedScriptPath(c);
                report.AppendLine($"[SCENE_BIND_REPORT] {sc.ResPath} | {sc.RootType.FullName} | {c.GetType().FullName} | {scriptPath}");
                // RootType is the type a production PanelSceneLoader.Load<T> call asks
                // this scene for. Asserting assignability is what catches a scene whose
                // root Script does not satisfy the request — the exact mismatch that made
                // DailyBriefingModal.tscn (a plain Control with no Script attached) throw
                // InvalidCastException from Main.Campaign.SetupDailyBriefingModal, while
                // the unique-name contract below still passed.
                if (!sc.RootType.IsInstanceOfType(c))
                    throw new SceneBindingException(
                        sc.ResPath, nameof(SceneBindingSelfTest), "<root>", sc.RootType.Name,
                        actualPath: c.GetType().Name,
                        "scene root does not satisfy the declared binding type. Attach a Script " +
                        "that derives from it, or declare the type the root actually is.",
                        nodeHint: false);
                var binder = new SceneBinder(c, sc.RootType);
                foreach (var (name, type) in sc.Contract)
                {
                    binder.Require(type, name);
                }
                if (sc.RootType.Name.EndsWith("Content", StringComparison.Ordinal))
                    binder.RequireNonEmptySurface(sc.RootType.Name);
                GD.Print($"[SCENE_BIND] PASS {sc.ResPath} ({c.GetChildCount()} children, {sc.Contract.Count} contract entries)");
                passed++;
            }
            catch (Exception e)
            {
                GD.PrintErr($"[SCENE_BIND] FAIL {sc.ResPath}: {e.Message}");
                failed++;
            }
            finally
            {
                if (root != null && GodotObject.IsInstanceValid(root))
                {
                    root.QueueFree();
                    root.Free();
                }
            }
        }
        GD.Print(report.ToString());
        if (!VerifyMismatchDiagnostics()) failed++;
        if (!VerifyDynamicScenePath()) failed++;
        GD.Print($"[SCENE_BIND] Summary: {passed} passed, {failed} failed (of {_checks.Count} scene contracts + 2 runtime probes)");
        return failed == 0 ? 0 : 1;
    }

    private static string AttachedScriptPath(Node root)
    {
        var scriptVariant = root.GetScript();
        if (scriptVariant.VariantType == Variant.Type.Nil) return "<no script attached>";
        var script = scriptVariant.As<Script>();
        return string.IsNullOrWhiteSpace(script?.ResourcePath) ? "<script path unavailable>" : script!.ResourcePath;
    }

    private static bool VerifyMismatchDiagnostics()
    {
        const string scenePath = "res://assets/ui/modals/DailyBriefingModal.tscn";
        try
        {
            var load = typeof(PanelSceneLoader).GetMethod(nameof(PanelSceneLoader.Load));
            if (load == null)
                throw new InvalidOperationException("PanelSceneLoader.Load<T> method was not found");
            load.MakeGenericMethod(typeof(DailyBriefingModal)).Invoke(null, new object[] { scenePath });
            GD.PrintErr("[SCENE_BIND_MISMATCH] FAIL expected a SceneBindingException for the unbound Control root");
            return false;
        }
        catch (System.Reflection.TargetInvocationException wrapper) when (wrapper.InnerException is SceneBindingException ex)
        {
            bool truthful = ex.ScenePath == scenePath &&
                            ex.ExpectedType == nameof(DailyBriefingModal) &&
                            ex.ActualPath != null && ex.ActualPath.Contains("NO Script attached", StringComparison.Ordinal);
            if (truthful)
            {
                GD.Print("[SCENE_BIND_MISMATCH] PASS requested type and actual unbound root are reported");
                return true;
            }
            GD.PrintErr($"[SCENE_BIND_MISMATCH] FAIL incomplete diagnostic: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[SCENE_BIND_MISMATCH] FAIL expected SceneBindingException, got {ex.GetType().Name}: {ex.Message}");
            return false;
        }
    }

    private static bool VerifyDynamicScenePath()
    {
        const string scenePath = "res://assets/ui/panels/WaterTreatmentPanel.tscn";
        WaterTreatmentPanelContent? root = null;
        try
        {
            root = PanelSceneLoader.Load<WaterTreatmentPanelContent>(scenePath);
            var binder = new SceneBinder(root, typeof(WaterTreatmentPanelContent));
            binder.RequireNonEmptySurface(nameof(WaterTreatmentPanelContent));
            GD.Print($"[SCENE_BIND_DYNAMIC] PASS {scenePath} -> {root.GetType().Name} ({AttachedScriptPath(root)})");
            return true;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[SCENE_BIND_DYNAMIC] FAIL {scenePath}: {ex.Message}");
            return false;
        }
        finally
        {
            if (root != null && GodotObject.IsInstanceValid(root)) root.Free();
        }
    }
}
