// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using Ashfall.Core.IO;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Developer Session: Asset Inspector.
    ///
    /// A read-only diagnostic overlay that enumerates authored content from the
    /// authoritative JSON catalogs in <c>Assets/StreamingAssets/Data</c> across
    /// every asset category (items, portraits, locations, factions) and, for each
    /// entry, whether its art resolves through the <see cref="AssetRegistry"/>
    /// (and to which path) or falls back to a placeholder. Presentation-only: it
    /// reads catalogs + the asset registry to *report* state; it never mutates
    /// game state, never owns data, and never fabricates identifiers (all ids
    /// come from the catalogs).
    ///
    /// Opened from the main menu DEV SESSION entry. Not registered as a
    /// player-navigable route; it is a developer affordance.
    /// </summary>
    public partial class AssetInspectorPanel : Control
    {
        public event Action? OnClose;

        private enum Cat { Items, Portraits, Locations, Factions, Art }

        /// <summary>One inspected entry: identity + resolved-asset status.</summary>
        private readonly struct EntryRow
        {
            public readonly string Id;
            public readonly string DisplayName;
            public readonly string Kind;
            public readonly string ResolvedPath;
            public readonly bool HasArt;

            public EntryRow(string id, string displayName, string kind, string resolvedPath, bool hasArt)
            {
                Id = id;
                DisplayName = displayName;
                Kind = kind;
                ResolvedPath = resolvedPath;
                HasArt = hasArt;
            }
        }

        // Catalog sources per category. Items keep the exact-match glob (all
        // *_items.json); the other categories use the authoritative catalog
        // files so unrelated files that merely carry a *_id field are excluded.
        private static readonly Dictionary<Cat, string[]> CatalogGlobs = new()
        {
            [Cat.Items] = new[] { "*items*.json" },
            [Cat.Portraits] = new[]
            {
                "survivors.json", "year_of_ash_survivors.json", "characters.json", "verdict_npcs.json"
            },
            [Cat.Locations] = new[]
            {
                "locations.json", "crossing_locations.json", "deep_lore_locations.json",
                "dose_locations.json", "duty_roster_locations.json", "holdfast_locations.json",
                "locations_expansion3.json", "verdict_locations.json", "year_of_ash_locations.json"
            },
            [Cat.Factions] = new[]
            {
                "currents.json", "crossing_factions.json", "holdfast_factions.json",
                "standing_record_factions.json", "foundry_faction.json", "faction_lore.json"
            },
        };

        private static readonly (Cat cat, string label)[] Categories =
        {
            (Cat.Items, "Items"),
            (Cat.Portraits, "Portraits"),
            (Cat.Locations, "Locations"),
            (Cat.Factions, "Factions"),
            (Cat.Art, "Art Status"),
        };

        private readonly Dictionary<Cat, List<EntryRow>> _rowsByCat = new();
        private List<EntryRow> _visible = new();
        private Cat _activeCat = Cat.Items;

        private Label _summaryLabel = null!;
        private LineEdit _searchBox = null!;
        private CheckBox _missingOnly = null!;
        private GridContainer _grid = null!;
        private HBoxContainer _catBar = null!;
        private readonly Dictionary<Cat, Button> _catButtons = new();
        private bool _built;

        public override void _Ready()
        {
            Visible = false;
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            _built = true;
            Reload();
        }

        /// <summary>Shows the inspector and re-reads catalog + asset state.</summary>
        public void Open()
        {
            Visible = true;
            Reload();
            QueueRedraw();
        }

        /// <summary>Re-reads authoritative catalogs and resolved asset state.</summary>
        public void Reload()
        {
            if (!_built) return;
            LoadAll();
            RefreshCatButtons();
            ApplyFilter();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }

        // ── Data (read-only; authoritative catalogs + asset registry) ─────

        private void LoadAll()
        {
            _rowsByCat.Clear();
            string dataDir = CatalogPath.ResolveDataDir();
            foreach (var (cat, _) in Categories)
            {
                var rows = new List<EntryRow>();
                if (cat == Cat.Art)
                {
                    LoadArtStatus(rows);
                }
                else if (Directory.Exists(dataDir))
                {
                    var seen = new HashSet<string>(StringComparer.Ordinal);
                    foreach (string glob in CatalogGlobs[cat])
                    {
                        foreach (string file in Directory.GetFiles(dataDir, glob))
                            ReadCatalog(file, cat, rows, seen);
                    }
                }
                rows.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
                _rowsByCat[cat] = rows;
            }
        }

        // Sprite collections tracked by their PLACEHOLDER_MANIFEST.json so the
        // dev can see which art is still placeholder vs baked final.
        private static readonly string[] ArtCollections = { "Characters", "Shelter", "Surface" };

        private static void LoadArtStatus(List<EntryRow> rows)
        {
            string root = ProjectSettings.GlobalizePath("res://");
            foreach (string collection in ArtCollections)
            {
                string manifest = Path.Combine(root, "assets", "sprites", collection, "PLACEHOLDER_MANIFEST.json");
                if (!File.Exists(manifest)) continue;
                ReadManifest(manifest, collection, rows);
            }
        }

        private static void ReadManifest(string file, string collection, List<EntryRow> rows)
        {
            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(File.ReadAllText(file));
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(file, "Art manifest parse", ex_CATDIAG);
                return;
            }
            using (doc)
            {
                if (!doc.RootElement.TryGetProperty("files", out JsonElement arr) ||
                    arr.ValueKind != JsonValueKind.Array)
                    return;
                foreach (JsonElement e in arr.EnumerateArray())
                {
                    if (e.ValueKind != JsonValueKind.Object) continue;
                    string? fname = FirstString(e, "file");
                    if (string.IsNullOrWhiteSpace(fname)) continue;
                    bool placeholder = e.TryGetProperty("placeholder", out JsonElement p) &&
                                       p.ValueKind == JsonValueKind.True;
                    string replacedBy = FirstString(e, "replaced_by", "label_in_image") ?? "(none)";
                    // HasArt == final (not placeholder); Kind carries the collection
                    // so BuildThumb can load res://assets/sprites/{Kind}/{Id}.
                    rows.Add(new EntryRow(fname, collection, collection, replacedBy, !placeholder));
                }
            }
        }

        private static void ReadCatalog(string file, Cat cat, List<EntryRow> rows, HashSet<string> seen)
        {
            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(File.ReadAllText(file));
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(file, "Asset inspector parse", ex_CATDIAG);
                return;
            }

            using (doc)
            {
                foreach (JsonElement it in EnumerateEntries(doc.RootElement))
                {
                    if (it.ValueKind != JsonValueKind.Object) continue;
                    string? id = FirstString(it, "id", "faction_id", "factionId", "survivor_id", "location_id");
                    if (string.IsNullOrWhiteSpace(id)) continue;
                    if (!seen.Add(id)) continue;

                    string name = FirstString(it, "displayName", "display_name", "name", "title") ?? id;
                    string kind = FirstString(it, "type", "category", "profession", "alignment") ?? "";

                    AssetResult res = Resolve(cat, id);
                    bool hasArt = res.IsValid && res.Texture != null;
                    string path = string.IsNullOrEmpty(res.ResolvedPath) ? "(none)" : res.ResolvedPath;
                    rows.Add(new EntryRow(id, name, kind, path, hasArt));
                }
            }
        }

        private static IEnumerable<JsonElement> EnumerateEntries(JsonElement root)
        {
            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement e in root.EnumerateArray()) yield return e;
                yield break;
            }
            if (root.ValueKind != JsonValueKind.Object) yield break;

            // Known list keys first, then the first array-of-objects property.
            string[] listKeys =
            {
                "items", "survivors", "characters", "npcs", "locations",
                "factions", "currents", "entries", "records", "relationships"
            };
            foreach (string k in listKeys)
            {
                if (root.TryGetProperty(k, out JsonElement arr) && arr.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement e in arr.EnumerateArray()) yield return e;
                    yield break;
                }
            }
            foreach (JsonProperty p in root.EnumerateObject())
            {
                if (p.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement e in p.Value.EnumerateArray())
                    {
                        if (e.ValueKind == JsonValueKind.Object) yield return e;
                    }
                    yield break;
                }
            }
        }

        private static AssetResult Resolve(Cat cat, string id) => cat switch
        {
            Cat.Portraits => AssetRegistry.GetPortrait(id),
            Cat.Locations => AssetRegistry.GetLocation(id),
            Cat.Factions  => AssetRegistry.GetFaction(id),
            _             => AssetRegistry.GetItem(id),
        };

        private static string? FirstString(JsonElement el, params string[] keys)
        {
            foreach (string k in keys)
            {
                if (el.TryGetProperty(k, out JsonElement v) && v.ValueKind == JsonValueKind.String)
                {
                    string? s = v.GetString();
                    if (!string.IsNullOrWhiteSpace(s)) return s;
                }
            }
            return null;
        }

        // ── Layout ───────────────────────────────────────────────────────

        private void BuildLayout()
        {
            var scrim = new ColorRect { Color = AshfallUiHelpers.PanelScrim() };
            scrim.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(scrim);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(1180, 720);
            center.AddChild(panel);

            var rootV = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            rootV.CustomMinimumSize = new Vector2(1140, 680);
            panel.AddChild(rootV);

            var title = AshfallUiHelpers.MakeTitle("ASSET INSPECTOR", DesignTheme.FontSizeH2);
            rootV.AddChild(title);

            var subtitle = new Label { Text = "DEVELOPER SESSION — read-only catalog / asset coverage" };
            subtitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
            subtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            rootV.AddChild(subtitle);

            // ── Category bar ──
            _catBar = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            rootV.AddChild(_catBar);
            foreach (var (cat, label) in Categories)
            {
                var btn = AshfallUiHelpers.MakeButton(label, () => SelectCategory(cat));
                btn.CustomMinimumSize = new Vector2(150, 32);
                _catButtons[cat] = btn;
                _catBar.AddChild(btn);
            }

            rootV.AddChild(AshfallUiHelpers.MakeSeparator());

            _summaryLabel = new Label { Text = "—" };
            _summaryLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            _summaryLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            rootV.AddChild(_summaryLabel);

            var filterRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            rootV.AddChild(filterRow);

            _searchBox = new LineEdit
            {
                PlaceholderText = "Filter by id, name, or kind…",
                CustomMinimumSize = new Vector2(420, 34),
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };
            _searchBox.TextChanged += _ => ApplyFilter();
            filterRow.AddChild(_searchBox);

            _missingOnly = new CheckBox { Text = "Missing art only" };
            _missingOnly.Toggled += _ => ApplyFilter();
            filterRow.AddChild(_missingOnly);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(140, 34);
            filterRow.AddChild(btnClose);

            rootV.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                SizeFlagsVertical = Control.SizeFlags.ExpandFill,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };
            rootV.AddChild(scroll);

            _grid = new GridContainer { Columns = 4, SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            _grid.AddThemeConstantOverride("h_separation", DesignTheme.SpacingSm);
            _grid.AddThemeConstantOverride("v_separation", DesignTheme.SpacingSm);
            scroll.AddChild(_grid);
        }

        private void SelectCategory(Cat cat)
        {
            _activeCat = cat;
            RefreshCatButtons();
            ApplyFilter();
        }

        private void RefreshCatButtons()
        {
            foreach (var (cat, _) in Categories)
            {
                if (_catButtons.TryGetValue(cat, out var btn))
                    btn.Disabled = cat == _activeCat;
            }
        }

        // ── Rendering ────────────────────────────────────────────────────

        private void ApplyFilter()
        {
            if (_grid == null) return;
            string q = (_searchBox?.Text ?? "").Trim().ToLowerInvariant();
            bool missingOnly = _missingOnly != null && _missingOnly.ButtonPressed;
            var all = _rowsByCat.TryGetValue(_activeCat, out var list) ? list : new List<EntryRow>();

            _visible = new List<EntryRow>();
            foreach (EntryRow row in all)
            {
                if (missingOnly && row.HasArt) continue;
                if (q.Length > 0 &&
                    !row.Id.ToLowerInvariant().Contains(q) &&
                    !row.DisplayName.ToLowerInvariant().Contains(q) &&
                    !row.Kind.ToLowerInvariant().Contains(q))
                    continue;
                _visible.Add(row);
            }

            RenderGrid();
            UpdateSummary(all);
        }

        private void UpdateSummary(List<EntryRow> all)
        {
            int resolved = 0;
            foreach (EntryRow r in all) if (r.HasArt) resolved++;
            int missing = all.Count - resolved;

            var counts = new List<string>();
            foreach (var (cat, label) in Categories)
            {
                var rows = _rowsByCat.TryGetValue(cat, out var l) ? l : new List<EntryRow>();
                int ok = 0;
                foreach (EntryRow r in rows) if (r.HasArt) ok++;
                counts.Add($"{label} {ok}/{rows.Count}");
            }

            string metric = _activeCat == Cat.Art
                ? $"final: {resolved}   ·   placeholder: {missing}"
                : $"art resolved: {resolved}   ·   fallback / missing: {missing}";
            _summaryLabel.Text =
                $"{_activeCat}: {all.Count}   ·   {metric}   ·   showing: {_visible.Count}\n" +
                "Coverage by category — " + string.Join("   ·   ", counts);
        }

        private void RenderGrid()
        {
            foreach (Node child in _grid.GetChildren())
                child.QueueFree();

            foreach (EntryRow row in _visible)
                _grid.AddChild(BuildCard(row));
        }

        private Control BuildCard(EntryRow row)
        {
            var card = AshfallUiHelpers.MakePanel(260, 0);
            var v = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            v.CustomMinimumSize = new Vector2(250, 0);
            card.AddChild(v);

            var icon = BuildThumb(row);
            icon.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            v.AddChild(icon);

            var name = new Label
            {
                Text = row.DisplayName,
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                ClipText = true
            };
            name.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            name.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            v.AddChild(name);

            var id = new Label { Text = row.Id, ClipText = true };
            id.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
            id.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            id.TooltipText = row.ResolvedPath;
            v.AddChild(id);

            var kind = new Label { Text = string.IsNullOrEmpty(row.Kind) ? "—" : row.Kind, ClipText = true };
            kind.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
            kind.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            v.AddChild(kind);

            // Status line (no Colors.Red / Colors.Green — theme tokens only).
            string statusText = _activeCat == Cat.Art
                ? (row.HasArt ? "FINAL" : "PLACEHOLDER")
                : (row.HasArt ? "ART OK" : "MISSING — fallback");
            var status = new Label { Text = statusText };
            status.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
            status.AddThemeColorOverride(
                "font_color",
                AshfallUiHelpers.ToColor(row.HasArt ? DesignTheme.Lethe : DesignTheme.Warm));
            v.AddChild(status);

            return card;
        }

        /// <summary>Neutral thumbnail: resolved texture, else the placeholder icon.</summary>
        private TextureRect BuildThumb(EntryRow row)
        {
            var rect = new TextureRect
            {
                CustomMinimumSize = new Vector2(72, 72),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize
            };
            if (_activeCat == Cat.Art)
            {
                // Show the actual sprite art so placeholder vs final is visible.
                rect.Texture = AshfallUiHelpers.TryLoadTexture($"res://assets/sprites/{row.Kind}/{row.Id}")
                            ?? AshfallUiHelpers.TryLoadTexture(AssetRegistry.FallbackIconPath);
                return rect;
            }
            AssetResult res = Resolve(_activeCat, row.Id);
            if (res.IsValid && res.Texture != null)
            {
                rect.Texture = res.Texture;
            }
            else
            {
                rect.Texture = AshfallUiHelpers.TryLoadTexture(AssetRegistry.FallbackIconPath);
            }
            return rect;
        }

        public override void _ExitTree()
        {
            _built = false;
            base._ExitTree();
        }
    }
}
