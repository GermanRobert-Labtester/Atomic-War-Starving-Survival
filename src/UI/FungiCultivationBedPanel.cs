// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Farming;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 190–193 / Plan 204: subterranean fungi cultivation workflow.
    /// Presentation-only — every mutation routes through the bound
    /// <see cref="FungiCultivationSystem"/>; feedback mirrors Core results.
    /// Communicates state → blocker → cost → consequence per the UI standard.
    /// </summary>
    public partial class FungiCultivationBedPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private FungiCultivationSystem? _system;
        private ItemList _plotList = null!;
        private VBoxContainer _detail = null!;
        private string _selectedPlotId = string.Empty;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;
        private OptionButton _strainSelect = null!;
        private OptionButton _substrateSelect = null!;

        public bool IsBound => _system != null;

        public void Bind(FungiCultivationSystem system) { _system = system; _selectedPlotId = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("DARK BEDS // SUBTERRANEAN FUNGI CULTIVATION", minWidth: 1000, minHeight: 650);

            _plotList = new ItemList
            {
                CustomMinimumSize = new Vector2(260, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _plotList.ItemSelected += OnPlotSelected;

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_plotList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        private void OnPlotSelected(long index)
        {
            if (_system == null) return;
            var plots = _system.State.plots;
            if (index >= 0 && index < plots.Count)
            {
                _selectedPlotId = plots[(int)index].plotId;
                RefreshView();
            }
        }

        private FungiPlotState? SelectedPlot()
        {
            if (_system == null) return null;
            if (string.IsNullOrEmpty(_selectedPlotId))
                return _system.State.plots.Count > 0 ? _system.State.plots[0] : null;
            return _system.State.plots.Find(p => p.plotId == _selectedPlotId);
        }

        private void SetFeedback(string message, bool failure)
        {
            _feedbackText = message;
            _feedbackIsFailure = failure;
        }

        private Ashfall.Core.ActionResult RunCommand(Func<Ashfall.Core.ActionResult> action, string successText)
        {
            if (_system == null) return Ashfall.Core.ActionResult.Failed("unbound", "fungi.unbound");
            var result = action();
            SetFeedback(result.IsSuccess ? successText : $"Blocked: {DescribeFailure(result.FailureCode)}", !result.IsSuccess);
            RefreshView();
            return result;
        }

        private static string DescribeFailure(string? code) => code switch
        {
            "plot_not_found" => "No bed selected.",
            "plot_occupied" => "Bed is already planted — harvest or clear it first.",
            "plot_quarantined" => "Bed is sealed under quarantine — purge or dispose first.",
            "plot_bloomed" => "Toxic bloom present — dispose of the substrate before preparing.",
            "prep_plot_occupied" => "Substrate can only be prepared on a fallow bed.",
            "prep_plot_bloomed" => "Clear the bloom before preparing substrate.",
            "unknown_strain" => "Unknown strain.",
            "unknown_substrate" => "Unknown substrate.",
            "missing_spores" => "No viable spores in storage.",
            "insufficient_water" => "Not enough clean water.",
            "prep_missing_water" => "Preparation needs 1 clean water.",
            "prep_missing_fuel" => "Heated preparation needs 1 fuel.",
            "insufficient_fuel" => "Not enough fuel.",
            "dispose_missing_fuel" => "Burning contaminated substrate needs 1 fuel.",
            "dispose_missing_sealant" => "Quarantine needs 2 scrap wood for sealing.",
            "dispose_unknown_method" => "Unknown disposal method.",
            "not_ready" => "Bed is not ready to harvest.",
            "no_bloom" => "No bloom to purge.",
            "not_contaminated" => "Bed is not contaminated — disposal not required.",
            "insufficient_clean_water" => "Purging needs 2 clean water.",
            _ => code ?? "unknown error"
        };

        private static string PhaseText(FungiPlotState plot)
        {
            if (plot.isQuarantined) return "QUARANTINED";
            if (plot.hasToxicBloom) return "TOXIC BLOOM";
            if (string.IsNullOrEmpty(plot.strainId)) return plot.substratePreparation == SubstratePreparation.Untreated
                ? "FALLOW (UNTREATED)" : $"FALLOW ({plot.substratePreparation.ToUpperInvariant()})";
            if (plot.isHarvestReady) return "FRUITING — READY";
            return plot.growthStage < 0.5f ? "COLONIZING" : "FRUITING";
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);
            _plotList.Clear();

            var plots = _system.State.plots;

            // Bed list (never raw IDs in prose — index labels only).
            for (int i = 0; i < plots.Count; i++)
            {
                var p = plots[i];
                _plotList.AddItem($"Bed {i + 1} — {PhaseText(p)}", null, false);
                if (p.plotId == _selectedPlotId || (string.IsNullOrEmpty(_selectedPlotId) && i == 0))
                {
                    _plotList.Select(i);
                    if (string.IsNullOrEmpty(_selectedPlotId)) _selectedPlotId = p.plotId;
                }
            }

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader("BED STATUS"));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            var plot = SelectedPlot();
            if (plot == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No cultivation beds excavated. Dig a new dark bed to begin substrate preparation.",
                    title: "NO BEDS EXCAVATED",
                    actionHint: "Use DIG NEW BED below."));
                BuildActions(fallowOnly: true);
                BuildNewBedSection();
                return;
            }

            bool hasStrain = _system!.Strains.TryGetValue(plot.strainId ?? "", out var strain);
            _system.Substrates.TryGetValue(plot.substrateId ?? "", out var substrate);

            int bedIndex = plots.IndexOf(plot);
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Bed", $"Bed {bedIndex + 1}", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("State", PhaseText(plot),
                plot.hasToxicBloom || plot.isQuarantined ? AshfallUiHelpers.ColorCritical : AshfallUiHelpers.ColorText));
            if (hasStrain)
            {
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Strain", strain!.display_name, AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Growth", $"{plot.growthStage:P0}", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Flushes left", plot.remainingFlushes > 0 ? plot.remainingFlushes.ToString() : "1", AshfallUiHelpers.ColorText));
            }
            if (substrate != null)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Substrate", substrate.display_name, AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Preparation", plot.substratePreparation.ToUpperInvariant(),
                plot.substratePreparation == SubstratePreparation.Compromised ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Moisture", $"{plot.moisture:P0}", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Contamination", $"{plot.contamination:P0}",
                plot.contamination >= 0.75f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));

            // Room spore load — the hazard that feeds ventilation/medicine.
            float roomHazard = _system.GetSporeHazardInRoom(plot.roomId);
            if (roomHazard > 0f)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Room spore load", $"{Math.Min(1f, roomHazard):P0}", AshfallUiHelpers.ColorWarning));

            float biolum = _system.GetBioluminescentLightOutput(plot.roomId);
            if (biolum > 0f)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Bioluminescent light", $"{biolum:F0} lux-eq", AshfallUiHelpers.ColorInfo));

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("BED ACTIONS"));
            BuildActions(fallowOnly: false, plot: plot, hasStrain: hasStrain, strain: hasStrain ? strain! : null);

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            BuildNewBedSection();
        }

        private void BuildActions(bool fallowOnly, FungiPlotState? plot = null, bool hasStrain = false, FungusStrainDef? strain = null)
        {
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            if (fallowOnly || plot == null || string.IsNullOrEmpty(plot.strainId))
            {
                row.AddChild(MakeAction("PREPARE SUBSTRATE (COLD)", "Costs 1 clean water. Reduces contamination risk; can fail.", () =>
                    RunCommand(() => _system!.PrepareSubstrate(_selectedPlotId, useHeat: false), "Substrate prepared (cold).")));
                row.AddChild(MakeAction("PREPARE SUBSTRATE (HEATED)", "Costs 1 clean water + 1 fuel. Best preparation, can still fail.", () =>
                    RunCommand(() => _system!.PrepareSubstrate(_selectedPlotId, useHeat: true), "Substrate prepared with heat.")));
            }

            if (plot != null && !string.IsNullOrEmpty(plot.strainId) && !plot.isQuarantined)
            {
                row.AddChild(MakeAction("WATER BED", "Costs 1 clean water. Raises moisture toward the strain band.", () =>
                    RunCommand(() => _system!.WaterPlot(_selectedPlotId), "Bed watered.")));

                if (plot.isHarvestReady)
                {
                    string expected = hasStrain ? $" Expected yield ≈ {strain!.yield_count} units." : string.Empty;
                    row.AddChild(MakeAction("HARVEST FLUSH", $"Collects this flush into storage.{expected}", () =>
                        RunCommand(() => _system!.HarvestPlot(_selectedPlotId), "Flush harvested.")));
                }
            }

            if (plot != null && plot.hasToxicBloom)
            {
                row.AddChild(MakeAction("PURGE (WATER)", "Costs 2 clean water. Clears the bed and resets it.", () =>
                    RunCommand(() => _system!.PurgeToxicBloom(_selectedPlotId), "Bloom purged.")));
                row.AddChild(MakeAction("DISCARD SUBSTRATE", "Free. Contaminated mass is written off.", () =>
                    RunCommand(() => _system!.DisposeInfectedSubstrate(_selectedPlotId, "discard"), "Contaminated substrate discarded.")));
                row.AddChild(MakeAction("BURN SUBSTRATE", "Costs 1 fuel. Destroys the contaminated mass outright.", () =>
                    RunCommand(() => _system!.DisposeInfectedSubstrate(_selectedPlotId, "burn"), "Contaminated substrate burned.")));
                row.AddChild(MakeAction("QUARANTINE BED", "Costs 2 scrap wood. Seals the bed: no growth, no spores, no spread.", () =>
                    RunCommand(() => _system!.DisposeInfectedSubstrate(_selectedPlotId, "quarantine"), "Bed sealed under quarantine.")));
            }

            if (row.GetChildCount() > 0)
                _detail.AddChild(row);
        }

        private void BuildNewBedSection()
        {
            if (_system == null) return;
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("NEW BED"));

            _strainSelect = new OptionButton { CustomMinimumSize = new Vector2(0, 26) };
            foreach (var s in _system.Strains.Values) _strainSelect.AddItem(s.display_name);
            _substrateSelect = new OptionButton { CustomMinimumSize = new Vector2(0, 26) };
            foreach (var s in _system.Substrates.Values) _substrateSelect.AddItem(s.display_name);

            var selectRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            selectRow.AddChild(_strainSelect);
            selectRow.AddChild(_substrateSelect);
            _detail.AddChild(selectRow);

            var digRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            digRow.AddChild(MakeAction("DIG NEW BED", "Costs nothing. New beds sit in the subterranean greenhouse.", () =>
            {
                int n = _system!.State.plots.Count + 1;
                _system.EnsurePlot($"plot_{n}", "room_greenhouse_shelter");
                _selectedPlotId = $"plot_{n}";
                SetFeedback("New bed excavated in the subterranean greenhouse.", false);
                RefreshView();
                return Ashfall.Core.ActionResult.Success("fungi.bed_dug");
            }));
            digRow.AddChild(MakeAction("PLANT SPORES", "Consumes 1 viable spore sample from storage.", () =>
            {
                var strains = _system!.Strains.Values.ToList();
                var subs = _system.Substrates.Values.ToList();
                if (_strainSelect.Selected < 0 || _strainSelect.Selected >= strains.Count)
                    return Ashfall.Core.ActionResult.Failed("no_selection", "fungi.unknown_strain");
                if (_substrateSelect.Selected < 0 || _substrateSelect.Selected >= subs.Count)
                    return Ashfall.Core.ActionResult.Failed("no_selection", "fungi.unknown_substrate");
                return RunCommand(() => _system.CultivateSpores(_selectedPlotId, strains[_strainSelect.Selected].strain_id, subs[_substrateSelect.Selected].substrate_id, _system.State.totalHarvests),
                    "Spores planted. Colonization begins.");
            }));
            _detail.AddChild(digRow);
        }

        private Control MakeAction(string label, string consequence, Func<Ashfall.Core.ActionResult> command)
        {
            var btn = AshfallUiHelpers.MakeButton(label, () => command());
            btn.TooltipText = consequence;
            btn.CustomMinimumSize = new Vector2(0, 30);
            return btn;
        }
    }
}
