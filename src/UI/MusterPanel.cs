using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — The Muster (Expansion 06) dedicated host panel.
    /// Integrates the 15 Sector Currents, Deserter Coalition Camp, The Unsigned
    /// Order witness dossiers, and sector faction outposts.
    /// Thin presentation only: delegates all mutations to MusterHostSession.
    /// </summary>
    public partial class MusterPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, IReadOnlyList<ApproachOption>>? OnApproachModalRequested;

        private MusterHostSession? _muster;
        private int _currentDay = 1;

        // ── UI Nodes ──────────────────────────────────────────────────
        private Label _escalationStatus = null!;
        private VBoxContainer _currentsContainer = null!;
        private VBoxContainer _coalitionContainer = null!;
        private VBoxContainer _witnessContainer = null!;
        private VBoxContainer _factionsContainer = null!;
        private Label _lblAuthorBias = null!;

        public bool IsBound => _muster != null;

        // ── Bind ──────────────────────────────────────────────────────

        public void Bind(MusterHostSession muster, int currentDay)
        {
            if (_muster != null)
                _muster.StateChanged -= OnStateChangedHandler;

            _muster = muster;
            _currentDay = currentDay;

            if (_muster != null)
                _muster.StateChanged += OnStateChangedHandler;

            RefreshView();
        }

        private void OnStateChangedHandler() => RefreshView();

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        // ── Godot Lifecycle ───────────────────────────────────────────

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
            AddChild(scroll);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            center.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(center);

            var rootBox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(760, 0);
            center.AddChild(rootBox);

            // ── Header ─────────────────────────────────────────────
            var header = AshfallUiHelpers.MakeTitle("THE MUSTER // SECTOR ESCALATION & CURRENTS", CoreTheme.FontSizeH1);
            header.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(header);

            var subtitle = AshfallUiHelpers.MakeSmall("Late-stage sector escalation protocols. The currents move. The holding ground waits.");
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
            subtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
            rootBox.AddChild(subtitle);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Escalation status bar ──────────────────────────────
            _escalationStatus = AshfallUiHelpers.MakeMono("ESCALATION: —");
            _escalationStatus.HorizontalAlignment = HorizontalAlignment.Center;
            _escalationStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Entropy));
            rootBox.AddChild(_escalationStatus);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Section 1: Sector Currents ─────────────────────────
            var currentsTitle = AshfallUiHelpers.MakeSectionHeader("SECTOR CURRENTS & FACTION ALIGNMENTS");
            rootBox.AddChild(currentsTitle);

            _currentsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_currentsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Section 2: Deserter Coalition Camp ─────────────────
            var campTitle = AshfallUiHelpers.MakeSectionHeader("DESERTER COALITION // HOLDING GROUND");
            rootBox.AddChild(campTitle);

            _coalitionContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_coalitionContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Section 3: The Unsigned Order Witness Dossiers ─────
            var witnessTitle = AshfallUiHelpers.MakeSectionHeader("THE UNSIGNED ORDER // WITNESS DOSSIERS");
            rootBox.AddChild(witnessTitle);

            _witnessContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_witnessContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Section 4: Sector Faction Outposts ─────────────────
            var factionTitle = AshfallUiHelpers.MakeSectionHeader("SECTOR FACTION OUTPOST STATUS");
            rootBox.AddChild(factionTitle);

            _factionsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_factionsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Footer buttons ──────────────────────────────────────
            var btnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingMd);
            btnRow.Alignment = BoxContainer.AlignmentMode.Center;

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [Esc]", () => OnClose?.Invoke(), false);
            btnClose.CustomMinimumSize = new Vector2(260, 42);
            btnRow.AddChild(btnClose);

            rootBox.AddChild(btnRow);

            var hint = AshfallUiHelpers.MakeSmall("Press [Esc] to return");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
            rootBox.AddChild(hint);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }

        // ── View Refresh ──────────────────────────────────────────────

        public void RefreshView()
        {
            if (_currentsContainer == null) return;

            ClearContainer(_currentsContainer);
            ClearContainer(_coalitionContainer);
            ClearContainer(_witnessContainer);
            ClearContainer(_factionsContainer);

            if (_muster == null)
            {
                _escalationStatus.Text = "Panel not bound to active Muster session.";
                return;
            }

            int day = _currentDay;
            var engine = _muster.Engine;

            // ── Escalation Bar ────────────────────────────────────────
            bool open = engine.MusterTriggered;
            string statusText = open
                ? $"ESCALATION: DAY {day} — THE MUSTER IS OPEN (Holding Ground Active)"
                : $"ESCALATION: DAY {day} — DORMANT (Muster opens Day {MusterSystem.MusterOpeningDay})";

            _escalationStatus.Text = statusText;
            _escalationStatus.AddThemeColorOverride("font_color",
                open
                    ? AshfallUiHelpers.ToColor(CoreTheme.Hot)
                    : AshfallUiHelpers.ToColor(CoreTheme.Warm));

            // ── Render Currents ───────────────────────────────────────
            var currentsCard = AshfallUiHelpers.MakeCardFrame("SECTOR CURRENTS MATRIX", $"{_muster.Roster.Count} REGISTERED BLOCS");
            var currentsBox = currentsCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            foreach (var current in _muster.Roster)
            {
                var row = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
                string state = current.isActive ? "ACTIVE" : "DORMANT";
                Color stateColor = current.isActive
                    ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
                    : AshfallUiHelpers.ToColor(CoreTheme.Dim);

                var nameLbl = AshfallUiHelpers.MakeMono($"{current.displayName}");
                nameLbl.CustomMinimumSize = new Vector2(220, 0);
                nameLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
                row.AddChild(nameLbl);

                var statusLbl = AshfallUiHelpers.MakeSmall($"[{state}] · {current.alignment} · Trust {current.trust:0}");
                statusLbl.AddThemeColorOverride("font_color", stateColor);
                statusLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(statusLbl);

                // If this current has a matching questline in the catalog, show approach button
                var qDef = engine.FindDefinition(current.id);
                if (qDef != null)
                {
                    var rec = engine.FindRecord(qDef.questlineId);
                    if (rec != null && rec.resolved)
                    {
                        var resTag = AshfallUiHelpers.MakeSmall($"[RESOLVED: {rec.selectedApproach}]");
                        resTag.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
                        row.AddChild(resTag);
                    }
                    else if (qDef.approaches != null && qDef.approaches.Count > 0)
                    {
                        string qId = qDef.questlineId;
                        var appList = qDef.approaches;
                        var btnApp = AshfallUiHelpers.MakeButton("APPROACH", () =>
                        {
                            OnApproachModalRequested?.Invoke(qId, appList);
                        });
                        btnApp.CustomMinimumSize = new Vector2(90, 28);
                        row.AddChild(btnApp);
                    }
                }

                currentsBox.AddChild(row);
            }
            _currentsContainer.AddChild(currentsCard);

            // ── Render Coalition Camp ─────────────────────────────────
            var campCard = AshfallUiHelpers.MakeCardFrame("DESERTER COALITION HOLDING GROUND", "SECTION VI.2 STATUS");
            var campBox = campCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            var campState = _muster.Camp.State;
            string formedStatus = campState.formed
                ? $"Formed Day {campState.formedDay} at {campState.holdingGroundId} · Vask {(campState.vaskWithCamp ? "with camp" : "absent")}"
                : "Holding ground not formed (requires Muster opening at Day 260).";

            campBox.AddChild(AshfallUiHelpers.MakeDataRow("Holding Ground", formedStatus, AshfallUiHelpers.ToColor(CoreTheme.Warm)));
            campBox.AddChild(AshfallUiHelpers.MakeDataRow("Members Rallied", $"{campState.membersRallied} fighters", AshfallUiHelpers.ToColor(CoreTheme.Hot)));
            campBox.AddChild(AshfallUiHelpers.MakeDataRow("Campaign Strategy", string.IsNullOrEmpty(campState.chosenStrategy) ? "None chosen" : campState.chosenStrategy, AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            campBox.AddChild(AshfallUiHelpers.MakeDataRow("Garrison Lockout Risk", $"{campState.garrisonLockoutRisk}%", AshfallUiHelpers.ToColor(campState.garrisonLockoutRisk > 50 ? CoreTheme.Critical : CoreTheme.Warm)));

            var campBtnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
            var btnRally = AshfallUiHelpers.MakeButton("RALLY DESERTER", () =>
            {
                _muster.RallyDeserter();
                RefreshView();
            });
            campBtnRow.AddChild(btnRally);

            if (string.IsNullOrEmpty(campState.chosenStrategy) && campState.formed)
            {
                var btnStratA = AshfallUiHelpers.MakeButton("STRATEGY: AMNESTY", () =>
                {
                    _muster.SetStrategy(QuestApproach.A);
                    RefreshView();
                });
                var btnStratB = AshfallUiHelpers.MakeButton("STRATEGY: OPEN MUSTER", () =>
                {
                    _muster.SetStrategy(QuestApproach.B);
                    RefreshView();
                });
                campBtnRow.AddChild(btnStratA);
                campBtnRow.AddChild(btnStratB);
            }
            campBox.AddChild(campBtnRow);

            _coalitionContainer.AddChild(campCard);

            // ── Render Witness Dossiers ───────────────────────────────
            var witCard = AshfallUiHelpers.MakeCardFrame("THE UNSIGNED ORDER (3 ACCOUNTS)", $"RECORDED BY {_muster.AuthorBias.ToString().ToUpperInvariant()} AUTHOR");
            var witBox = witCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            var biasRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
            _lblAuthorBias = AshfallUiHelpers.MakeBody($"Current Recording Bias: {_muster.AuthorBias}");
            _lblAuthorBias.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
            _lblAuthorBias.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            biasRow.AddChild(_lblAuthorBias);

            var btnCycleBias = AshfallUiHelpers.MakeButton("CYCLE AUTHOR BIAS", () =>
            {
                _muster.CycleAuthorBias();
                RefreshView();
            });
            btnCycleBias.CustomMinimumSize = new Vector2(180, 32);
            biasRow.AddChild(btnCycleBias);
            witBox.AddChild(biasRow);
            witBox.AddChild(AshfallUiHelpers.MakeSeparator());

            for (int i = 0; i < _muster.Witnesses.Count; i++)
            {
                var w = _muster.Witnesses[i];
                if (day < w.dayMin) continue;

                string framing = JournalVoice.ComposeFullText(w.knowledgeKey, _muster.AuthorBias, day);
                var entryBox = AshfallUiHelpers.MakeVBox(2);

                var wHeader = AshfallUiHelpers.MakeMono($"{w.witnessName} — {w.locationId} (Min Day {w.dayMin})");
                wHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
                entryBox.AddChild(wHeader);

                var wBody = AshfallUiHelpers.MakeSmall($"{w.body}\n{framing}");
                wBody.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                wBody.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
                entryBox.AddChild(wBody);

                witBox.AddChild(entryBox);
                witBox.AddChild(AshfallUiHelpers.MakeSeparator());
            }

            _witnessContainer.AddChild(witCard);

            // ── Render Sector Faction Outposts ────────────────────────
            var facCard = AshfallUiHelpers.MakeCardFrame("SECTOR OUTPOST STATUS", "SUB-SYSTEM INTEGRITY");
            var facBox = facCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            // Hydro Barons
            var hb = _muster.HydroBarons;
            string hbStatus = hb.PlantSeized ? "PLANT SEIZED" : (hb.AdminReform ? "ADMIN REFORMED" : "EXTRACTION MONOPOLY ACTIVE");
            facBox.AddChild(AshfallUiHelpers.MakeDataRow("Hydro Barons Water Grid", hbStatus, AshfallUiHelpers.ToColor(CoreTheme.Warm)));

            // Cold Count
            var cc = _muster.ColdCount;
            facBox.AddChild(AshfallUiHelpers.MakeDataRow("Cold Count Heating", $"Power: {cc.PowerSuppliedDays}/{ColdCountState.RequiredPowerDays} days · Shielding: {cc.ShieldingDelivered}/{ColdCountState.RequiredShieldingUnits} units", AshfallUiHelpers.ToColor(CoreTheme.Hot)));

            // Provisioned
            var ps = _muster.Provisioned;
            facBox.AddChild(AshfallUiHelpers.MakeDataRow("Provisioned Rations", $"Respect: {ps.RespectScore}/{ProvisionedState.ContactThreshold}", AshfallUiHelpers.ToColor(CoreTheme.Pale)));

            // Iron Raiders
            var ir = _muster.IronRaiders;
            facBox.AddChild(AshfallUiHelpers.MakeDataRow("Iron Raiders Perimeter", $"Aggression: {ir.AggressionLevel:P0} · Raids This Season: {ir.RaidsThisSeason} · Visibility: {ir.State.shelterVisibility:P0}", AshfallUiHelpers.ToColor(CoreTheme.Warm)));

            // Scavenger Guild
            var sg = _muster.ScavengerGuild;
            facBox.AddChild(AshfallUiHelpers.MakeDataRow("Scavenger Guild", $"Claimed Sites: {sg.State.claimedSiteIds.Count} · Trust: {sg.Trust:F1} · Blacklists: {sg.State.blacklistedShelterIds.Count}", AshfallUiHelpers.ToColor(CoreTheme.Pale)));

            // Long Walk
            var lw = _muster.LongWalk;
            facBox.AddChild(AshfallUiHelpers.MakeDataRow("Long Walk Waste Traversal", $"Region: {lw.State.currentRegion} · Departure in {lw.State.daysUntilDeparture}d · Crossings: {lw.State.crossingsCompleted}", AshfallUiHelpers.ToColor(CoreTheme.Warm)));

            _factionsContainer.AddChild(facCard);
        }

        private static void ClearContainer(VBoxContainer container)
        {
            AshfallUiHelpers.EmptyChildren(container);
        }


    public void Unbind()
    {
        if (_muster != null)
            {
                _muster.StateChanged -= OnStateChangedHandler;
            }
    }

    public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
