using System;
using System.Text;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Panel for THE SILENT FOUNDRY (Expansion 10) — smelter-bay production,
    /// repair, maintenance, casting quality, safety, and labor conflict.
    /// Thin presentation only; all rules live in Ashfall.Core.Foundry.
    /// </summary>
    public partial class SilentFoundryPanel : Control
    {
        public event Action? OnClose;

        private SilentFoundryHostSession? _host;
        private Label _lblSummary = null!;
        private Label _lblLastEvent = null!;
        private VBoxContainer _conditions = null!;
        private VBoxContainer _sand = null!;
        private VBoxContainer _production = null!;
        private VBoxContainer _treaties = null!;
        private VBoxContainer _history = null!;
        private int _currentDay = 4;

        public bool IsBound => _host != null;

        public void Bind(SilentFoundryHostSession session, int currentDay)
        {
            _host = session;
            _currentDay = currentDay;
            if (_host != null)
                _host.StateChanged += RefreshView;
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.03f, 0.04f, 0.05f, 0.94f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(1040, 700);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            panel.AddChild(margins);

            var rootVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            margins.AddChild(rootVBox);

            // ── Header ──
            var header = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("THE SILENT FOUNDRY // CUPOLA & CASTING BAY", DesignTheme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            rootVBox.AddChild(header);

            rootVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblSummary = AshfallUiHelpers.MakeMono("FOUNDRY SEALED");
            _lblSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            rootVBox.AddChild(_lblSummary);

            _lblLastEvent = AshfallUiHelpers.MakeSmall("", autowrap: true);
            _lblLastEvent.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            rootVBox.AddChild(_lblLastEvent);

            rootVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var bodyRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            bodyRow.SizeFlagsVertical = SizeFlags.ExpandFill;
            rootVBox.AddChild(bodyRow);

            // ── Left column: conditions, sand, labor ──
            var left = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            left.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            left.SizeFlagsVertical = SizeFlags.ExpandFill;
            bodyRow.AddChild(left);

            var condCard = AshfallUiHelpers.MakeCardFrame("FACILITY CONDITION", "refractory · hearth · sand · structure · exhaust", 460, 0);
            left.AddChild(condCard);
            _conditions = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            condCard.AddChild(_conditions);

            var sandCard = AshfallUiHelpers.MakeCardFrame("GREEN-SAND BED", "moisture · binder · pattern · contamination", 460, 0);
            left.AddChild(sandCard);
            _sand = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            sandCard.AddChild(_sand);

            var laborCard = AshfallUiHelpers.MakeCardFrame("LABOR & SAFETY", "shifts · education · dispute", 460, 0);
            left.AddChild(laborCard);
            var laborBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            laborCard.AddChild(laborBox);
            BuildLaborActions(laborBox);

            // ── Right column: production, treaties, history ──
            var right = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            right.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            right.SizeFlagsVertical = SizeFlags.ExpandFill;
            bodyRow.AddChild(right);

            var prodCard = AshfallUiHelpers.MakeCardFrame("CASTING FLOOR", "charge → heat → tap → cast", 520, 0);
            right.AddChild(prodCard);
            _production = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            prodCard.AddChild(_production);

            var treatyCard = AshfallUiHelpers.MakeCardFrame("ACCORD OBLIGATIONS", "signatory of 4 District 8 accords", 520, 0);
            right.AddChild(treatyCard);
            _treaties = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            treatyCard.AddChild(_treaties);

            var historyCard = AshfallUiHelpers.MakeCardFrame("PRODUCTION & INCIDENT HISTORY", "casts · failures · repairs · incidents", 520, 0);
            right.AddChild(historyCard);
            _history = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            historyCard.AddChild(_history);
        }

        private void BuildLaborActions(VBoxContainer laborBox)
        {
            var overtimeRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnOvertimeOn = AshfallUiHelpers.MakeButton("ORDER OVERTIME", () => Run(() => _host?.SetOvertime(true)));
            var btnOvertimeOff = AshfallUiHelpers.MakeButton("RESCIND OVERTIME", () => Run(() => _host?.SetOvertime(false)));
            overtimeRow.AddChild(btnOvertimeOn);
            overtimeRow.AddChild(btnOvertimeOff);
            laborBox.AddChild(overtimeRow);

            var childRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnChildOn = AshfallUiHelpers.MakeButton("CHILDREN TO FLOOR", () => Run(() => _host?.SetChildLabor(true)));
            var btnChildOff = AshfallUiHelpers.MakeButton("CHILDREN TO LESSONS", () => Run(() => _host?.SetChildLabor(false)));
            childRow.AddChild(btnChildOn);
            childRow.AddChild(btnChildOff);
            laborBox.AddChild(childRow);

            var disputeRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnDispute = AshfallUiHelpers.MakeButton("OPEN LABOR DISPUTE", () => Run(() => _host?.OpenDispute(_currentDay)));
            var btnConcede = AshfallUiHelpers.MakeButton("RESOLVE: CONCEDE SHIFTS", () => Run(() => _host?.ResolveStrike(FoundryStrikeResolution.ConcedeShiftLimits, _currentDay)));
            var btnMediate = AshfallUiHelpers.MakeButton("RESOLVE: MEDIATION", () => Run(() => _host?.ResolveStrike(FoundryStrikeResolution.Mediation, _currentDay)));
            disputeRow.AddChild(btnDispute);
            disputeRow.AddChild(btnConcede);
            disputeRow.AddChild(btnMediate);
            laborBox.AddChild(disputeRow);
        }

        private void Run(Func<string?> action)
        {
            string? msg = action();
            if (msg != null && _host != null)
            {
                _host.LastEvent = msg;
                _lblLastEvent.Text = msg;
            }
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_host == null) return;
            var sys = _host.Engine;
            var s = sys.State;

            string labor = s.laborDispute switch
            {
                FoundryLaborDispute.Tensions => "TENSIONS",
                FoundryLaborDispute.StrikeActive => "STRIKE ACTIVE",
                FoundryLaborDispute.Resolved => "RESOLVED",
                _ => "CALM"
            };
            string maint = !sys.IsUnlocked ? "unscheduled"
                : sys.IsMaintenanceOverdue ? "OVERDUE " + sys.DaysOverdue + "d"
                : s.maintenanceDueDay > 0 ? "due d" + s.maintenanceDueDay : "unscheduled";

            _lblSummary.Text = $"{(s.unlocked ? "OPEN" : "SEALED")} · heat: {sys.HeatStage} · maintenance: {maint} · "
                + $"labor: {labor} · casts: {sys.TotalProductionCount} · failed: {sys.TotalFailedCount} · "
                + $"incidents: {sys.Incidents.Count} · stress {sys.CumulativeStress:F0} / hope {sys.CumulativeHope:F0}";

            if (_host != null && !string.IsNullOrEmpty(_host.LastEvent))
                _lblLastEvent.Text = "» " + _host.LastEvent;

            RebuildConditions(s);
            RebuildSand(s);
            RebuildProduction();
            RebuildTreaties();
            RebuildHistory();
        }

        private static Label MakeBar(string label, float value, (float r, float g, float b, float a) color)
        {
            string v = value.ToString("F0");
            var lbl = AshfallUiHelpers.MakeMono($"{label,-28} {v,3}/100");
            lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(value < 30f ? DesignTheme.Critical : color));
            return lbl;
        }

        private void RebuildConditions(SilentFoundryState s)
        {
            ClearChildren(_conditions);
            _conditions.AddChild(MakeBar("Refractory lining", s.refractoryLining, DesignTheme.Warm));
            _conditions.AddChild(MakeBar("Hearth & tuyeres", s.hearthTuyeres, DesignTheme.Hot));
            _conditions.AddChild(MakeBar("Sand beds", s.sandBeds, DesignTheme.LetheAmber));
            _conditions.AddChild(MakeBar("Structural supports", s.structuralSupports, DesignTheme.Muted));
            _conditions.AddChild(MakeBar("Safety & exhaust", s.safetyExhaust, DesignTheme.Exclusive));

            var warnings = _host!.Engine.GetSafetyWarnings();
            foreach (var w in warnings)
            {
                var warn = AshfallUiHelpers.MakeSmall("⚠ " + w, autowrap: true);
                warn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Entropy));
                _conditions.AddChild(warn);
            }
        }

        private void RebuildSand(SilentFoundryState s)
        {
            ClearChildren(_sand);
            _sand.AddChild(MakeBar("Sand quality", s.sandQuality, DesignTheme.Warm));
            _sand.AddChild(MakeBar("Moisture (target 65)", s.sandMoisture, DesignTheme.Lethe));
            _sand.AddChild(MakeBar("Binder quality", s.binderQuality, DesignTheme.Muted));
            _sand.AddChild(MakeBar("Pattern quality", s.patternQuality, DesignTheme.Hot));
            _sand.AddChild(MakeBar("Compaction", s.compaction, DesignTheme.Warm));
            _sand.AddChild(AshfallUiHelpers.MakeSmall($"Mold reuse: {s.moldReuseCount} · contamination: {s.contamination:F0}% · overtime: {(s.overtimeFlag ? "ON" : "off")} · children: {(s.childLaborUsed ? "FLOOR" : "lessons")}"));

            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            row.AddChild(AshfallUiHelpers.MakeButton("PREPARE SAND (2 sand + 40L water)", () => Run(() => _host?.PrepareSand(40))));
            row.AddChild(AshfallUiHelpers.MakeButton("COMPACT MOLD", () => Run(() => _host?.CompactMold())));
            _sand.AddChild(row);
        }

        private void RebuildProduction()
        {
            ClearChildren(_production);
            var sys = _host!.Engine;
            var s = sys.State;

            if (!s.unlocked)
            {
                _production.AddChild(AshfallUiHelpers.MakeSmall(
                    "The blast furnace is sealed. The blueprint — room_bp_11, Heavy Metallurgy — is catalogued, but no one has signed the charge manifest."));
                var unlockRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var btnUnlock = AshfallUiHelpers.MakeButton("UNLOCK THE FOUNDRY", () => Run(() => _host?.Unlock(_currentDay)));
                unlockRow.AddChild(btnUnlock);
                _production.AddChild(unlockRow);
                return;
            }

            _production.AddChild(AshfallUiHelpers.MakeSmall(
                $"Active: {(_host!.Catalog.GetProduct(s.activeProductId)?.display_name ?? "(none)")} · workers {s.assignedWorkers} · "
                + $"skill {s.workerSkill:F2} · labor {s.laborAccumulated:F0}h · exposure {s.workerExposure:F0}"));

            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnMaintain = AshfallUiHelpers.MakeButton("FULL MAINTENANCE", () => Run(() => _host?.Maintain(_currentDay)));
            var btnTap = AshfallUiHelpers.MakeButton("TAP & CAST", () => Run(() => _host?.Tap(_currentDay)));
            var btnRepairHearth = AshfallUiHelpers.MakeButton("REPAIR HEARTH (10 brick)", () => Run(() => _host?.Repair(FoundryFacilityComponent.HearthTuyeres, _currentDay)));
            var btnRepairLining = AshfallUiHelpers.MakeButton("REPAIR LINING (8 brick)", () => Run(() => _host?.Repair(FoundryFacilityComponent.RefractoryLining, _currentDay)));
            row.AddChild(btnMaintain);
            row.AddChild(btnTap);
            row.AddChild(btnRepairHearth);
            row.AddChild(btnRepairLining);
            _production.AddChild(row);

            foreach (var p in _host!.Catalog.AllProducts)
            {
                var line = AshfallUiHelpers.MakeMono(
                    $"{p.display_name,-24} {Costs(p),-34} → {p.result_amount}× {p.result_item_id.SubstringAfter("item_foundry_")}");
                line.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                var h = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                h.AddChild(line);
                var btn = AshfallUiHelpers.MakeButton("START HEAT", () => Run(() => _host?.StartHeat(p.product_id, 4, 0.6f, _currentDay)));
                h.AddChild(btn);
                _production.AddChild(h);
            }
        }

        private static string Costs(FoundryProductEntry p)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < p.ingredients.Count; i++)
            {
                sb.Append(p.ingredients[i].amount).Append(' ').Append(p.ingredients[i].item_id.SubstringAfter("item_foundry_"));
                if (i < p.ingredients.Count - 1) sb.Append(" + ");
            }
            sb.Append(" · fuel ").Append(p.fuel_units).Append(" · water ").Append(p.water_litres).Append('L');
            return sb.ToString();
        }

        private void RebuildTreaties()
        {
            ClearChildren(_treaties);
            if (_host == null) return;
            var sys = _host.Engine;
            var comps = sys.State.treatyCompliance;
            if (comps == null || comps.Count == 0)
            {
                _treaties.AddChild(AshfallUiHelpers.MakeSmall("No treaty rows bound."));
                return;
            }

            _treaties.AddChild(AshfallUiHelpers.MakeSmall(
                $"Foundry standing: {sys.GuildStanding:F0}/100 · stance: {_host.GuildStance} · "
                + $"consequences applied: {sys.AppliedConsequences.Count}"));

            foreach (var c in comps)
            {
                FoundryTreatyOutcome outcome = sys.GetTreatyOutcome(c.treatyId, _currentDay);
                string status = c.obligation switch
                {
                    "road_iron_quota" => $"road iron {c.quotaFulfilled}/{c.quotaTotal} (met {c.metCount}, missed {c.missedCount})",
                    "brine_pipe_quota" => $"brine pipes {c.quotaFulfilled}/{c.quotaTotal} (met {c.metCount}, missed {c.missedCount})",
                    "labor_shifts" => $"shifts {(sys.State.overtimeFlag || sys.State.childLaborUsed || sys.State.laborDispute == FoundryLaborDispute.StrikeActive ? "VIOLATED" : "upheld")}",
                    "charter_eligibility" => $"charter eligibility: {(c.constitutionEligible ? "clear" : "at risk (incidents on record)")}",
                    _ => c.obligation
                };
                var lbl = AshfallUiHelpers.MakeSmall(
                    $"[{OutcomeLabel(outcome)}] {c.treatyId} — {status}");
                lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(OutcomeColor(outcome, c)));
                _treaties.AddChild(lbl);
            }

            // Consequence ledger: the player must see what changed, when, and why.
            var applied = sys.AppliedConsequences;
            if (applied.Count > 0)
            {
                _treaties.AddChild(AshfallUiHelpers.MakeSubsectionHeader("APPLIED CONSEQUENCES"));
                for (int i = applied.Count - 1; i >= 0; i--)
                {
                    var r = applied[i];
                    if (r == null) continue;
                    var line = AshfallUiHelpers.MakeSmall(
                        $"d{r.appliedDay} {OutcomeLabel(r.outcome)} {r.treatyId} — standing {r.standingDelta:+0;-0;0} · {r.reason}");
                    line.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
                    _treaties.AddChild(line);
                    if (r.modifiers != null)
                    {
                        for (int m = 0; m < r.modifiers.Count; m++)
                        {
                            var mod = r.modifiers[m];
                            if (mod == null) continue;
                            var mline = AshfallUiHelpers.MakeSmall(
                                $"    → {mod.good_id} demand {mod.demand_delta:+0.00;-0.00} ({mod.reason})");
                            mline.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                            _treaties.AddChild(mline);
                        }
                    }
                }
            }
        }

        private static string OutcomeLabel(FoundryTreatyOutcome o)
        {
            switch (o)
            {
                case FoundryTreatyOutcome.NotRatified: return "NOT RATIFIED";
                case FoundryTreatyOutcome.Pending: return "PENDING";
                case FoundryTreatyOutcome.Met: return "MET";
                case FoundryTreatyOutcome.Missed: return "MISSED";
                case FoundryTreatyOutcome.Violated: return "VIOLATED";
                default: return o.ToString();
            }
        }

        private static (float r, float g, float b, float a) OutcomeColor(FoundryTreatyOutcome o, FoundryTreatyCompliance c)
        {
            switch (o)
            {
                case FoundryTreatyOutcome.Violated: return DesignTheme.Critical;
                case FoundryTreatyOutcome.Missed: return DesignTheme.Exclusive;
                case FoundryTreatyOutcome.Met: return DesignTheme.Lethe;
                case FoundryTreatyOutcome.NotRatified: return DesignTheme.Dim;
                default: return c.missedCount > 0 ? DesignTheme.Exclusive : DesignTheme.Muted;
            }
        }

        private void RebuildHistory()
        {
            ClearChildren(_history);
            if (_host == null) return;
            var sys = _host.Engine;
            foreach (var r in sys.CompletedProduction)
                _history.AddChild(AshfallUiHelpers.MakeSmall($"CAST  d{r.completedDay}  {r.displayName} ×{r.amount}  {r.tier}"));
            foreach (var f in sys.FailedCasts)
                _history.AddChild(AshfallUiHelpers.MakeSmall($"FAIL  d{f.failedDay}  {f.displayName}  {f.reason}"));
            foreach (var i in sys.Incidents)
            {
                var lbl = AshfallUiHelpers.MakeSmall($"INCIDENT d{i.day}  [{i.severity}]  {i.summary}  downtime {i.downtimeDays}d");
                lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                _history.AddChild(lbl);
            }
        }

        private static void ClearChildren(Node parent)
        {
            while (parent.GetChildCount() > 0)
                parent.RemoveChild(parent.GetChild(0));
        }
    }

    internal static class FoundryStringExt
    {
        /// <summary>Last segment after the final '_' (item_foundry_plowshare → plowshare).</summary>
        public static string SubstringAfter(this string value, string prefix)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            int idx = value.LastIndexOf(prefix, StringComparison.Ordinal);
            return idx >= 0 ? value.Substring(idx + prefix.Length) : value;
        }
    }
}
