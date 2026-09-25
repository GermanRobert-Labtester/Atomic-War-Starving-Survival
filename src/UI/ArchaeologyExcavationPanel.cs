// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Archaeology;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 190–193: pre-war archaeology & decryption workflow.
    /// Presentation-only — decryption and broker-sale commands are emitted via
    /// <see cref="OnActionRequested"/> and resolved by the host through the
    /// bound <see cref="ArchaeologySystem"/>.
    /// </summary>
    public partial class ArchaeologyExcavationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private ArchaeologySystem? _system;
        private ItemList _archiveList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedArchiveIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(ArchaeologySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("BEFORE // PRE-WAR ARCHIVES & DIG SITES", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("sites", "Dig Sites", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
            _statusRail.AddCard("archives", "Archives Held", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("decrypted", "Decrypted", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("research", "Research Value", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _archiveList = new ItemList
            {
                CustomMinimumSize = new Vector2(300, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _archiveList.ItemSelected += index => { _selectedArchiveIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_archiveList);

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
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private List<PreWarArchiveInstance> SortedArchives()
        {
            if (_system == null) return new List<PreWarArchiveInstance>();
            return _system.Archives
                .OrderBy(a => a.archiveId, StringComparer.Ordinal)
                .ToList();
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _archiveList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var archives = SortedArchives();
            _selectedArchiveIndex = Math.Clamp(_selectedArchiveIndex, -1, Math.Max(0, archives.Count - 1));

            int decrypted = archives.Count(a => a.unlocked);
            int research = archives.Where(a => a.unlocked && !a.researchClaimed).Sum(a => a.researchPoints);

            if (_statusRail != null)
            {
                _statusRail.Set("sites", $"{_system.Sites.Count(s => s.discovered)}/{_system.Sites.Count}", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("archives", archives.Count.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("decrypted", decrypted.ToString(),
                    decrypted > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
                _statusRail.Set("research", research > 0 ? $"{research} pts" : "—",
                    research > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
            }

            _archiveList.Clear();
            for (int i = 0; i < archives.Count; i++)
            {
                var a = archives[i];
                string state = a.corrupted ? "CORRUPTED" : a.unlocked ? "DECRYPTED" : a.sold ? "SOLD" : $"encrypted T{a.encryptionTier} — {a.decryptionProgress:0}%";
                _archiveList.AddItem($"{ArchiveTitle(a)} — {state}", null, false);
                if (i == _selectedArchiveIndex)
                    _archiveList.Select(i);
            }
            if (archives.Count == 0)
                _archiveList.AddItem("No archives recovered", null, false);

            var selected = _selectedArchiveIndex >= 0 && _selectedArchiveIndex < archives.Count
                ? archives[_selectedArchiveIndex] : null;

            // ── Dig sites ──
            if (_system.Sites.Count > 0)
            {
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("DIG SITES"));
                foreach (var site in _system.Sites)
                {
                    if (site == null) continue;
                    string state = site.exhausted ? "exhausted" : site.discovered ? $"{site.excavationProgress:0}% excavated" : "undiscovered";
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow(
                        string.IsNullOrEmpty(site.displayName) ? ItemDisplay.Prettify(site.siteId) : site.displayName,
                        state,
                        site.exhausted ? AshfallUiHelpers.ColorDim :
                        site.discovered ? AshfallUiHelpers.ColorText : AshfallUiHelpers.ColorMuted));
                }
            }

            // ── Archive detail ──
            if (selected != null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(AshfallUiHelpers.MakeSectionHeader(ArchiveTitle(selected).ToUpperInvariant()));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Encryption tier", $"T{selected.encryptionTier}", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Decryption", $"{selected.decryptionProgress:0}%",
                    selected.unlocked ? AshfallUiHelpers.ColorSuccess :
                    selected.corrupted ? AshfallUiHelpers.ColorCritical :
                    selected.decryptionProgress > 0 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorDim));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Research value", selected.unlocked ? $"{selected.researchPoints} pts" : "locked",
                    selected.unlocked ? AshfallUiHelpers.ColorInfo : AshfallUiHelpers.ColorDim));

                if (selected.corrupted)
                    _detail.AddChild(AshfallUiHelpers.MakeWarning("The archive degraded past recovery — the data is gone."));

                if (!string.IsNullOrEmpty(_feedbackText))
                {
                    _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                    _detail.AddChild(_feedbackIsFailure
                        ? AshfallUiHelpers.MakeWarning(_feedbackText)
                        : AshfallUiHelpers.MakeSuccess(_feedbackText));
                }

                // ── Actions ──
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

                if (!selected.unlocked && !selected.corrupted && !selected.sold)
                {
                    var decryptBtn = AshfallUiHelpers.MakeButton("RUN DECRYPTION SHIFT", () =>
                        OnActionRequested?.Invoke("decrypt", selected.archiveId));
                    decryptBtn.TooltipText = "Puts an engineer on the archive for a work shift. Higher tiers need power and keycards.";
                    row.AddChild(decryptBtn);
                }
                if (selected.unlocked && !selected.researchClaimed)
                {
                    var sellBtn = AshfallUiHelpers.MakeButton("SELL TO BROKER", () =>
                        OnActionRequested?.Invoke("sell", selected.archiveId));
                    sellBtn.TooltipText = "Hands the archive to a broker for cap-and-lead. The research value is lost.";
                    row.AddChild(sellBtn);
                }

                if (row.GetChildCount() == 0)
                    row.AddChild(AshfallUiHelpers.MakeMetadata(selected.unlocked
                        ? "Decrypted and claimed — the knowledge is already ours."
                        : "Nothing to do for this archive right now."));
                _detail.AddChild(row);
            }
            else
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No pre-war archives in hand. Excavation parties bring data storages back from the digs.",
                    title: "NO ARCHIVES"));
            }
        }

        private static string ArchiveTitle(PreWarArchiveInstance a) =>
            string.IsNullOrEmpty(a.titleKey) ? ItemDisplay.Prettify(a.archiveId) : ItemDisplay.Prettify(a.titleKey);

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
