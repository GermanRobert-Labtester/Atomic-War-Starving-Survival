using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Optional, menu-only cohort selector. It owns no campaign state and
    /// emits only the selected stable profile ID when the player commits.
    /// </summary>
    public sealed class StartingCohortSelection
    {
        public string CohortProfileId { get; }
        public string StartingSuppliesProfileId { get; }

        public StartingCohortSelection(
            string cohortProfileId,
            string startingSuppliesProfileId)
        {
            CohortProfileId = cohortProfileId;
            StartingSuppliesProfileId = startingSuppliesProfileId;
        }
    }

    public partial class StartingCohortSetupPanel : Control
    {
        public event Action<StartingCohortSelection>? OnStartRequested;
        public event Action? OnCancel;

        private VBoxContainer _profileList = null!;
        private VBoxContainer _originList = null!;
        private Label _preview = null!;
        private string _selectedProfileId = StartingCohortCatalog.StandardProfileId;
        private string _selectedOriginId = StartingSuppliesCatalog.StandardProfileId;
        private StartingCohortCatalog? _catalog;
        private StartingSuppliesCatalog? _suppliesCatalog;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        public void Bind(
            StartingCohortCatalog catalog,
            StartingSuppliesCatalog? suppliesCatalog = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _suppliesCatalog = suppliesCatalog ??
                new StartingSuppliesCatalog(
                    new[] { StartingSuppliesCatalog.CreateLegacyFallbackProfile() },
                    StartingSuppliesCatalog.StandardProfileId);
            if (_profileList == null) return;

            AshfallUiHelpers.EmptyChildren(_profileList);
            foreach (var profile in catalog.Profiles)
            {
                var captured = profile.profile_id;
                var button = AshfallUiHelpers.MakeButton(
                    $"{profile.display_name.ToUpperInvariant()}\n" +
                    string.Join("  ·  ", profile.members.Select(m => m.displayName)),
                    () => SelectProfile(captured));
                button.Alignment = HorizontalAlignment.Left;
                button.CustomMinimumSize = new Vector2(0, 62);
                button.TooltipText = profile.description;
                _profileList.AddChild(button);
            }

            AshfallUiHelpers.EmptyChildren(_originList);
            foreach (var profile in _suppliesCatalog.Profiles)
            {
                var captured = profile.id;
                var button = AshfallUiHelpers.MakeButton(
                    $"{profile.display_name.ToUpperInvariant()}\n{profile.description}",
                    () => SelectOrigin(captured));
                button.Alignment = HorizontalAlignment.Left;
                button.CustomMinimumSize = new Vector2(0, 62);
                button.TooltipText = profile.description;
                _originList.AddChild(button);
            }

            if (!catalog.TryGet(_selectedProfileId, out _))
                _selectedProfileId = catalog.DefaultProfileId;
            if (!_suppliesCatalog.TryGet(_selectedOriginId, out _))
                _selectedOriginId = _suppliesCatalog.DefaultProfileId;
            RefreshPreview();
        }

        public void Open()
        {
            if (_catalog != null)
            {
                _selectedProfileId = _catalog.DefaultProfileId;
            }
            if (_suppliesCatalog != null)
                _selectedOriginId = _suppliesCatalog.DefaultProfileId;
            RefreshPreview();
            Visible = true;
            GrabFirstProfileFocus();
        }

        public void Close() => Visible = false;

        private void BuildLayout()
        {
            var dim = new ColorRect
            {
                Color = new Color(0f, 0f, 0f, 0.78f),
                MouseFilter = Control.MouseFilterEnum.Stop
            };
            dim.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(dim);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(860, 620);
            center.AddChild(panel);
            var margin = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margin);
            var root = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            margin.AddChild(root);

            root.AddChild(AshfallUiHelpers.MakeTitle(
                "SELECT STARTING COHORT",
                Ashfall.Core.UI.Theme.FontSizeH2));
            root.AddChild(AshfallUiHelpers.MakeBody(
                "Cohort changes who is present and their day-zero condition. " +
                "It does not grant items, skills, research, or faction standing."));
            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var split = new HBoxContainer
            {
                SizeFlagsVertical = Control.SizeFlags.ExpandFill
            };
            split.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            root.AddChild(split);

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(430, 0),
                SizeFlagsVertical = Control.SizeFlags.ExpandFill
            };
            _profileList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            var profileColumn = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            profileColumn.AddChild(AshfallUiHelpers.MakeSectionHeader("STARTING COHORT"));
            profileColumn.AddChild(_profileList);
            profileColumn.AddChild(AshfallUiHelpers.MakeSectionHeader("STARTING STORES"));
            _originList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            profileColumn.AddChild(_originList);
            scroll.AddChild(profileColumn);
            split.AddChild(scroll);

            var previewPanel = AshfallUiHelpers.MakePanel(360, 0);
            var previewMargin = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingSm);
            previewPanel.AddChild(previewMargin);
            _preview = AshfallUiHelpers.MakeBody(string.Empty);
            _preview.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            previewMargin.AddChild(_preview);
            split.AddChild(previewPanel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());
            var actions = new HBoxContainer
            {
                Alignment = BoxContainer.AlignmentMode.End
            };
            actions.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            var cancel = AshfallUiHelpers.MakeButton(
                "CANCEL",
                () => OnCancel?.Invoke());
            cancel.CustomMinimumSize = new Vector2(140, 40);
            actions.AddChild(cancel);
            var start = AshfallUiHelpers.MakeButton(
                "START CAMPAIGN",
                () => OnStartRequested?.Invoke(
                    new StartingCohortSelection(
                        _selectedProfileId,
                        _selectedOriginId)));
            start.CustomMinimumSize = new Vector2(180, 40);
            actions.AddChild(start);
            root.AddChild(actions);
        }

        private void SelectProfile(string profileId)
        {
            _selectedProfileId = profileId;
            RefreshPreview();
        }

        private void SelectOrigin(string profileId)
        {
            _selectedOriginId = profileId;
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (_preview == null || _catalog == null) return;
            if (!_catalog.TryGet(_selectedProfileId, out var profile)) return;
            var supplies = _suppliesCatalog?.ResolveOrDefault(_selectedOriginId);
            if (supplies == null) return;

            _preview.Text =
                $"{profile.display_name.ToUpperInvariant()}\n\n" +
                $"{profile.description}\n\n" +
                "MEMBERS\n" +
                string.Join("\n", profile.members.Select(m =>
                    $"• {m.displayName} — {DescribeInitialCondition(m)}")) +
                "\n\nSTARTING STORES\n" +
                $"{supplies.display_name.ToUpperInvariant()}\n" +
                supplies.description;
        }

        private static string DescribeInitialCondition(StartingSurvivorDefinition member)
        {
            var notes = new List<string>();
            if (member.acuteRad) notes.Add("acute radiation");
            if (member.health < 88f) notes.Add("injured");
            if (member.hunger >= 36f || member.thirst >= 36f) notes.Add("strained");
            if (notes.Count == 0) notes.Add("stable");
            return string.Join(", ", notes);
        }

        private void GrabFirstProfileFocus()
        {
            if (_profileList == null || _profileList.GetChildCount() == 0) return;
            if (_profileList.GetChild(0) is Control first)
                first.GrabFocus();
        }
    }
}
