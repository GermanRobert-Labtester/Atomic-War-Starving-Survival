// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Godot;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.YearOfAsh;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Faction War Communiqué Surface (#133 / #135).
    /// Displays day-gated public diplomatic statements and communiqués issued by wasteland factions.
    /// Pure projection of loaded catalog and current campaign day. Zero simulation mutation.
    /// </summary>
    public partial class FactionCommuniqueBoardPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell? _shell;
        private VBoxContainer _communiqueList = null!;
        private Label _statusLabel = null!;
        private ScrollContainer _scroll = null!;

        private YearOfAshHostSession? _session;
        private int _campaignDay;
        private bool _isBound;

        private static readonly Dictionary<string, string> s_factionDisplayNames = new(StringComparer.Ordinal);
        private static bool s_loreLoaded;

        public bool IsBound => _isBound && _session != null;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.03f, 0.04f, 0.05f, 0.95f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", 32);
            margin.AddThemeConstantOverride("margin_right", 32);
            margin.AddThemeConstantOverride("margin_top", 24);
            margin.AddThemeConstantOverride("margin_bottom", 24);
            AddChild(margin);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 16);
            margin.AddChild(rootVbox);

            // Header Bar
            var headerRow = new HBoxContainer();
            var titleBox = new VBoxContainer();
            var title = AshfallUiHelpers.MakeTitle("DISTRICT WIRE // FACTION COMMUNIQUÉS");
            titleBox.AddChild(title);

            _statusLabel = AshfallUiHelpers.MakeSmall("SECTOR 4 MONITORED WIRE — PUBLIC DIPLOMATIC STATEMENTS");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            titleBox.AddChild(_statusLabel);

            headerRow.AddChild(titleBox);
            titleBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var closeBtn = AshfallUiHelpers.MakeButton("DISCONNECT", () => OnClose?.Invoke());
            closeBtn.CustomMinimumSize = new Vector2(140, 36);
            headerRow.AddChild(closeBtn);
            rootVbox.AddChild(headerRow);

            // Scrollable Communiqué Area
            _scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            rootVbox.AddChild(_scroll);

            _communiqueList = new VBoxContainer();
            _communiqueList.AddThemeConstantOverride("separation", 16);
            _communiqueList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _scroll.AddChild(_communiqueList);

            RefreshView();
        }

        public void Bind(YearOfAshHostSession? session, int campaignDay)
        {
            _session = session;
            _campaignDay = campaignDay;
            _isBound = session != null;
            EnsureFactionLoreLoaded();
            RefreshView();
        }

        public void Unbind()
        {
            _session = null;
            _isBound = false;
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_communiqueList == null) return;

            AshfallUiHelpers.EmptyChildren(_communiqueList);

            if (!IsBound)
            {
                if (_statusLabel != null)
                    _statusLabel.Text = "DISTRICT WIRE: OFFLINE";

                var emptyCard = AshfallUiHelpers.MakeCardFrame("WIRE STATUS", "DISCONNECTED");
                var emptyBox = emptyCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
                var emptyLabel = AshfallUiHelpers.MakeBody("District wire not connected.");
                emptyBox.AddChild(emptyLabel);
                _communiqueList.AddChild(emptyCard);
                return;
            }

            if (_statusLabel != null)
                _statusLabel.Text = $"SECTOR 4 MONITORED WIRE — DAY {_campaignDay} ARCHIVE";

            var catalog = _session?.WarRunner?.Catalog;
            var communiques = catalog?.Communiques;

            if (communiques == null || communiques.Count == 0)
            {
                RenderNoCommuniquesCard($"No communiqués have been issued yet as of day {_campaignDay}.");
                return;
            }

            var visible = communiques
                .Where(c => c.day <= _campaignDay)
                .OrderByDescending(c => c.day)
                .ThenBy(c => c.id)
                .ToList();

            if (visible.Count == 0)
            {
                RenderNoCommuniquesCard($"No communiqués have been issued yet as of day {_campaignDay}.");
                return;
            }

            foreach (var c in visible)
            {
                string factionName = ResolveFactionName(c.factionId);
                var card = AshfallUiHelpers.MakeCardFrame(
                    $"[DAY {c.day:D3}] {factionName.ToUpperInvariant()}",
                    $"\"{c.title}\"");
                card.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
                cardBox.AddThemeConstantOverride("separation", 8);

                var bodyLabel = AshfallUiHelpers.MakeBody(c.body);
                bodyLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                bodyLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                cardBox.AddChild(bodyLabel);

                _communiqueList.AddChild(card);
            }
        }

        private void RenderNoCommuniquesCard(string message)
        {
            var card = AshfallUiHelpers.MakeCardFrame("WIRE ARCHIVE", "NO TRAFFIC");
            var box = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
            var label = AshfallUiHelpers.MakeBody(message);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            box.AddChild(label);
            _communiqueList.AddChild(card);
        }

        private static void EnsureFactionLoreLoaded()
        {
            if (s_loreLoaded) return;
            s_loreLoaded = true;

            string osPath = CatalogPath.ResolveCatalog("faction_lore.json");
            try
            {
                if (!File.Exists(osPath)) return;
                using var stream = File.OpenRead(osPath);
                using var doc = JsonDocument.Parse(stream);
                var root = doc.RootElement;
                JsonElement items = root.ValueKind == JsonValueKind.Array
                    ? root
                    : (root.TryGetProperty("items", out var arr) ? arr : (root.TryGetProperty("factions", out var farr) ? farr : default));

                if (items.ValueKind == JsonValueKind.Array)
                {
                    foreach (var entry in items.EnumerateArray())
                    {
                        string id = entry.TryGetProperty("faction_id", out var fid) ? fid.GetString() ?? "" : "";
                        if (string.IsNullOrEmpty(id)) continue;
                        string display = entry.TryGetProperty("display_name", out var dn) ? dn.GetString() ?? id : id;
                        s_factionDisplayNames[id] = display;
                    }
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(osPath, "faction_lore.json", ex);
            }
        }

        private static string ResolveFactionName(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return "Unknown Dispatch";
            if (s_factionDisplayNames.TryGetValue(factionId, out string? name) && !string.IsNullOrEmpty(name))
                return name;
            return factionId;
        }
    }
}
