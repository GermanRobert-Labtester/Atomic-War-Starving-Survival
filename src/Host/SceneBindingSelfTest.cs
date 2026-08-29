// SPDX-License-Identifier: MIT
// ASHFALL — SceneBindingSelfTest.cs (Ticket #125 verb driver).
//
// Implements --scene-binding-selftest, which loads every production scene
// declared in PanelSceneLoader.Load<R>(res://assets/ui/panels/<Name>.tscn)
// where R is one of the migrated detail panels, then resolves each
// scene's typed unique-name node contract via SceneBinder.
//
// The Godot-side dispatcher is a thin wrapper over the SceneBindingHeadlessProbe
// declarations; we register each scene (+ contract) below so the verb is
// owned by a single self-contained file and the existing ProjectBuild
// coverage path picks it up.

using System;
using System.Collections.Generic;
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
        Check("res://assets/ui/panels/KitchenNutritionPanel.tscn", typeof(KitchenNutritionPanelContent),
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
        Check("res://assets/ui/panels/PharmaLabPanel.tscn", typeof(PharmaLabPanelContent),
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
        Check("res://assets/ui/modals/OpeningProtocolModal.tscn", typeof(OpeningProtocolModalContent),
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
        Check("res://assets/ui/modals/SafeCrackModal.tscn", typeof(SafeCrackModalContent),
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
        Check("res://assets/ui/modals/DailyBriefingModal.tscn", typeof(DailyBriefingModalContent),
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
        foreach (var sc in _checks)
        {
            Node? root = null;
            try
            {
                root = PanelSceneLoader.Load<Node>(sc.ResPath);
                if (!(root is Control c))
                    throw new Exception("scene root is not Control");
                var binder = new SceneBinder(c, sc.RootType);
                foreach (var (name, type) in sc.Contract)
                {
                    binder.Require(type, name);
                }
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
        GD.Print($"[SCENE_BIND] Summary: {passed} passed, {failed} failed (of {_checks.Count})");
        return failed == 0 ? 0 : 1;
    }
}
