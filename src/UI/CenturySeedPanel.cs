using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Century Seed Panel (Expansion 12 — Generational Succession).
    /// Manages multi-generational survival across the 10-year horizon, survivor aging,
    /// mentorship bonds, trait inheritance, elder retirement, and lineage chronicle.
    ///
    /// Presentation only — queries GenerationalSuccessionEngine for state.
    /// </summary>
    public partial class CenturySeedPanel : Control
    {
        public event Action? OnClose;

        private GenerationalSuccessionEngine? _succession;
        private SurvivorsHostSession? _survivors;
        private VBoxContainer _lineageContainer = null!;
        private VBoxContainer _mentorshipContainer = null!;
        private Label _statusLabel = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public void Bind(GenerationalSuccessionEngine? succession, SurvivorsHostSession? survivors)
        {
            _succession = succession;
            _survivors = survivors;
            EnsureDwellersRegistered();
            RefreshView();
        }

        private void EnsureDwellersRegistered()
        {
            if (_succession == null || _survivors == null) return;

            var list = _survivors.RosterState;
            if (list == null) return;

            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null) continue;
                _succession.RegisterDweller(s.Id, 32 + (i * 4), 0);
            }
        }

        public void Open()
        {
            Visible = true;
            EnsureDwellersRegistered();
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        private void BuildLayout()
        {
            var backdrop = new ColorRect
            {
                Color = new Color(0.04f, 0.05f, 0.06f, 0.95f)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
            AddChild(margin);

            var mainVBox = new VBoxContainer();
            mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            margin.AddChild(mainVBox);

            // ── Header Card ──
            var headerCard = AshfallUiHelpers.MakeCardFrame(
                "THE CENTURY SEED // EXP 12: GENERATIONAL SUCCESSION & LINEAGE",
                "Multi-generational shelter succession across the 10-year horizon, survivor aging, mentorship pairings, trait inheritance, and elder retirement."
            );
            mainVBox.AddChild(headerCard);

            // ── Scrollable Body ──
            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var contentBox = new VBoxContainer();
            contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            _lineageContainer = new VBoxContainer();
            _lineageContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_lineageContainer);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("MENTORSHIP BONDS & TRAIT SUCCESSION"));

            _mentorshipContainer = new VBoxContainer();
            _mentorshipContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_mentorshipContainer);

            // ── Bottom Action Bar ──
            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(bottomBar);

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 44);
            bottomBar.AddChild(btnClose);

            _statusLabel = AshfallUiHelpers.MakeMono("Lineage engine active. Assign mentorships to preserve survival knowledge.");
            _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_statusLabel);
        }

        public void RefreshView()
        {
            if (_lineageContainer == null || _mentorshipContainer == null) return;

            ClearContainer(_lineageContainer);
            ClearContainer(_mentorshipContainer);

            if (_succession == null)
            {
                _statusLabel.Text = "Succession engine unavailable.";
                return;
            }

            // ── Chapter Timeline Card ──
            var chapCard = AshfallUiHelpers.MakePanel();
            var chapMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            chapCard.AddChild(chapMargin);

            var cBox = new VBoxContainer();
            cBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            chapMargin.AddChild(cBox);

            cBox.AddChild(AshfallUiHelpers.MakeSectionHeader($"CHAPTER {_succession.CurrentChapterIndex}: THE LONG WINTER"));
            cBox.AddChild(AshfallUiHelpers.MakeDataRow("Chapter Timeline", $"{_succession.DaysElapsedInChapter} / {GenerationalSuccessionEngine.DaysPerChapter} Days Elapsed", AshfallUiHelpers.ToColor(CoreTheme.Hot)));
            cBox.AddChild(AshfallUiHelpers.MakeDataRow("Total Horizon", $"{_succession.TotalYearsElapsed} Years Since Fallout Exchange", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            cBox.AddChild(AshfallUiHelpers.MakeDataRow("Retirement Mandate", "Age 65+ (Withdraws from hazardous exterior sorties to advisory elder role)", AshfallUiHelpers.ToColor(CoreTheme.Pale)));

            _lineageContainer.AddChild(chapCard);

            // ── Survivor Lineage Cards ──
            _lineageContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVIVOR GENERATION & SUCCESSION ROSTER"));

            if (_survivors != null && _survivors.RosterState != null)
            {
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s == null) continue;

                    var rec = _succession.GetRecord(s.Id);
                    int age = rec != null ? rec.inGameAgeYears : 35;
                    int gen = rec != null ? rec.generationIndex : 0;
                    bool isRet = rec != null && rec.isRetired;

                    var sCard = AshfallUiHelpers.MakePanel();
                    var sMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
                    sCard.AddChild(sMargin);

                    var sBox = new VBoxContainer();
                    sBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingXs);
                    sMargin.AddChild(sBox);

                    var top = new HBoxContainer();
                    top.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
                    sBox.AddChild(top);

                    string name = s.Id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant();
                    var lblName = AshfallUiHelpers.MakeSectionHeader($"{name} (Gen {gen})");
                    lblName.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(isRet ? CoreTheme.Pale : CoreTheme.Hot));
                    lblName.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    top.AddChild(lblName);

                    string badgeText = isRet ? "[RETIRED ELDER]" : $"[AGE {age}]";
                    var badgeColor = isRet ? CoreTheme.Pale : (age >= 60 ? CoreTheme.Hot : CoreTheme.Warm);
                    var badge = AshfallUiHelpers.MakeSmall(badgeText);
                    badge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(badgeColor));
                    top.AddChild(badge);

                    string mentorText = rec != null && !string.IsNullOrEmpty(rec.mentorDwellerId)
                        ? $"Mentored by: {rec.mentorDwellerId.Replace("survivor_", "")}"
                        : "No formal mentorship bound";
                    sBox.AddChild(AshfallUiHelpers.MakeDataRow("Mentorship", mentorText, AshfallUiHelpers.ToColor(CoreTheme.Pale)));

                    string traits = rec != null && rec.inheritedTraitIds.Count > 0
                        ? string.Join(", ", rec.inheritedTraitIds)
                        : "None inherited";
                    sBox.AddChild(AshfallUiHelpers.MakeDataRow("Inherited Traits", traits, AshfallUiHelpers.ToColor(CoreTheme.Pale)));

                    _lineageContainer.AddChild(sCard);
                }
            }

            // ── Mentorship Binding Card ──
            var mCard = AshfallUiHelpers.MakePanel();
            var mMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            mCard.AddChild(mMargin);

            var mBox = new VBoxContainer();
            mBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            mMargin.AddChild(mBox);

            mBox.AddChild(AshfallUiHelpers.MakeDataRow("Apprenticeship System", "Pairs veteran survivors with younger generation dwellers to inherit vital perks.", AshfallUiHelpers.ToColor(CoreTheme.Pale)));

            var btnForm = AshfallUiHelpers.MakeButton("PAIR MENTOR & APPRENTICE (SARAH CHEN → ELENA VASQUEZ)", () =>
            {
                bool ok = _succession.FormMentorship("survivor_sarah_chen", "survivor_elena_vasquez", "trait_rad_resilience");
                _statusLabel.Text = ok ? "Mentorship formalized: Dr. Sarah Chen bonded with Elena Vasquez." : "Mentorship could not be established.";
                RefreshView();
            });
            btnForm.CustomMinimumSize = new Vector2(400, 36);
            mBox.AddChild(btnForm);

            _mentorshipContainer.AddChild(mCard);
        }

        private static void ClearContainer(VBoxContainer container)
        {
            AshfallUiHelpers.EmptyChildren(container);
        }
    }
}
