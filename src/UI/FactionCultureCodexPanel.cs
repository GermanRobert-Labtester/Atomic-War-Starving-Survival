// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Faction Culture Codex Panel.
    /// Plan 14 / Plan 25 Follow-Up (Dedicated Faction Culture Codex Integration).
    ///
    /// Presents the everyday customs, codes, and rituals that make the wasteland factions
    /// living societies rather than generic banners. Reads from the authoritative
    /// muster_faction_culture.json loaded by FactionCultureCatalogLoader into MusterHostSession.Culture.
    /// </summary>
    public partial class FactionCultureCodexPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private List<FactionCultureEntry> _entries = new();
        private string _activeFactionFilter = "ALL";

        private VBoxContainer _cardsContainer = null!;
        private HBoxContainer _filterBar = null!;
        private Label _countLabel = null!;
        private Label _titleLabel = null!;
        private ScrollContainer _scroll = null!;

        public bool IsBound => _entries != null && _entries.Count > 0;

        public void Bind(MusterHostSession? muster)
        {
            if (muster?.Culture != null && muster.Culture.Count > 0)
            {
                Bind(muster.Culture);
                return;
            }

            // Fallback load if muster not yet initialized. The panel
            // never resolves a data path itself: CatalogPath is the one data
            // authority (env → executable → project → cwd → pck → fallback),
            // so an exported build and a development checkout resolve to the
            // same catalog instead of each inventing its own root.
            string dataDir = CatalogPath.ResolveDataDir();
            var loaded = FactionCultureCatalogLoader.LoadEntries(dataDir, new Ashfall.Core.FileSystemIO(), new Ashfall.Core.SystemTextJsonSerializer());
            Bind(loaded);
        }

        public void Bind(List<FactionCultureEntry>? entries)
        {
            _entries = entries ?? new List<FactionCultureEntry>();
            BuildFilterBar();
            RefreshCards();
        }

        public void Bind(object? data)
        {
            if (data is MusterHostSession muster)
                Bind(muster);
            else if (data is List<FactionCultureEntry> entries)
                Bind(entries);
        }

        public void Unbind()
        {
            _entries.Clear();
            if (_cardsContainer != null)
                AshfallUiHelpers.EmptyChildren(_cardsContainer);
            if (_filterBar != null)
                AshfallUiHelpers.EmptyChildren(_filterBar);
        }

        public override void _Ready()
        {
            AnchorLeft = 0;
            AnchorTop = 0;
            AnchorRight = 1;
            AnchorBottom = 1;

            // Background dimmer
            var bg = new ColorRect
            {
                Color = new Color(0.04f, 0.05f, 0.07f, 0.96f)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var mainMargin = new MarginContainer();
            mainMargin.SetAnchorsPreset(LayoutPreset.FullRect);
            mainMargin.AddThemeConstantOverride("margin_left", 32);
            mainMargin.AddThemeConstantOverride("margin_right", 32);
            mainMargin.AddThemeConstantOverride("margin_top", 24);
            mainMargin.AddThemeConstantOverride("margin_bottom", 24);
            AddChild(mainMargin);

            var mainBox = AshfallUiHelpers.MakeVBox(16);
            mainBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            mainBox.SizeFlagsVertical = SizeFlags.ExpandFill;
            mainMargin.AddChild(mainBox);

            // ── Top Header Row ──
            var headerRow = AshfallUiHelpers.MakeHBox(16);
            headerRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var titleBox = AshfallUiHelpers.MakeVBox(4);
            titleBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _titleLabel = AshfallUiHelpers.MakeSectionHeader("FACTION CULTURE CODEX // CUSTOMS, CODES & RITUALS");
            _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            titleBox.AddChild(_titleLabel);

            var subLabel = AshfallUiHelpers.MakeBody("Authoritative archives of everyday codes, claim marks, water arithmetic, and parley customs across wasteland factions.");
            subLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            titleBox.AddChild(subLabel);

            headerRow.AddChild(titleBox);

            _countLabel = AshfallUiHelpers.MakeBody("0 ENTRIES");
            _countLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            headerRow.AddChild(_countLabel);

            var closeBtn = AshfallUiHelpers.MakeButton("ESC // CLOSE", () => OnClose?.Invoke());
            closeBtn.CustomMinimumSize = new Vector2(130, 36);
            headerRow.AddChild(closeBtn);

            mainBox.AddChild(headerRow);
            mainBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Faction Filter Bar ──
            _filterBar = AshfallUiHelpers.MakeHBox(8);
            _filterBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            mainBox.AddChild(_filterBar);

            // ── Scrollable Card Content ──
            _scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                VerticalScrollMode = ScrollContainer.ScrollMode.Auto
            };

            _cardsContainer = AshfallUiHelpers.MakeVBox(14);
            _cardsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _scroll.AddChild(_cardsContainer);

            mainBox.AddChild(_scroll);

            Visible = false;

            if (_entries.Count > 0)
            {
                BuildFilterBar();
                RefreshCards();
            }
        }

        private void BuildFilterBar()
        {
            if (_filterBar == null) return;
            AshfallUiHelpers.EmptyChildren(_filterBar);

            var factions = new HashSet<string>(StringComparer.Ordinal);
            foreach (var e in _entries)
            {
                if (!string.IsNullOrEmpty(e.factionId))
                    factions.Add(e.factionId);
            }

            var allBtn = AshfallUiHelpers.MakeButton("ALL FACTIONS", () =>
            {
                _activeFactionFilter = "ALL";
                RefreshCards();
            });
            allBtn.CustomMinimumSize = new Vector2(110, 30);
            _filterBar.AddChild(allBtn);

            foreach (var f in factions.OrderBy(x => x))
            {
                string label = FormatFactionName(f);
                string fid = f;
                var btn = AshfallUiHelpers.MakeButton(label, () =>
                {
                    _activeFactionFilter = fid;
                    RefreshCards();
                });
                btn.CustomMinimumSize = new Vector2(120, 30);
                _filterBar.AddChild(btn);
            }
        }

        private void RefreshCards()
        {
            if (_cardsContainer == null) return;
            AshfallUiHelpers.EmptyChildren(_cardsContainer);

            var filtered = _activeFactionFilter == "ALL"
                ? _entries
                : _entries.Where(e => string.Equals(e.factionId, _activeFactionFilter, StringComparison.OrdinalIgnoreCase)).ToList();

            if (_countLabel != null)
                _countLabel.Text = $"{filtered.Count} ARCHIVED ENTRIES";

            if (filtered.Count == 0)
            {
                var emptyLabel = AshfallUiHelpers.MakeBody("No culture records cataloged for the selected filter.");
                emptyLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _cardsContainer.AddChild(emptyLabel);
                return;
            }

            foreach (var entry in filtered)
            {
                string tag = FormatFactionName(entry.factionId);
                var card = AshfallUiHelpers.MakeCardFrame(entry.title.ToUpperInvariant(), tag);
                var innerBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                // Faction identification header row
                var row = AshfallUiHelpers.MakeHBox(10);
                var emblem = AshfallUiHelpers.MakeFactionEmblem(entry.factionId, 32);
                row.AddChild(emblem);

                var badge = AshfallUiHelpers.MakeBody($"[{tag.ToUpperInvariant()}]  //  ID: {entry.id}");
                badge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                row.AddChild(badge);

                innerBox.AddChild(row);
                innerBox.AddChild(AshfallUiHelpers.MakeSeparator());

                // Body text
                var bodyLabel = AshfallUiHelpers.MakeBody(entry.body);
                bodyLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                bodyLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                bodyLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                innerBox.AddChild(bodyLabel);

                _cardsContainer.AddChild(card);
            }
        }

        private static string FormatFactionName(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return "Unknown";
            string stripped = factionId.StartsWith("faction_", StringComparison.OrdinalIgnoreCase)
                ? factionId.Substring("faction_".Length)
                : factionId;

            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(stripped.Replace('_', ' '));
        }

        public void Open()
        {
            Visible = true;
            if (_entries.Count == 0)
            {
                Bind(null as MusterHostSession);
            }
            else
            {
                RefreshCards();
            }
            QueueRedraw();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
