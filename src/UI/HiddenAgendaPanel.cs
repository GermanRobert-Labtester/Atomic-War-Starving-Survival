// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class HiddenAgendaPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _agendaListContainer = null!;
        private VBoxContainer _detailContainer = null!;
        private Label _detailText = null!;
        private Label _cluesText = null!;
        private Button _investigateBtn = null!;
        private Button _reconcileBtn = null!;
        private Button _exploitBtn = null!;
        private Button _expelBtn = null!;
        private Button _betrayBtn = null!;

        private HiddenAgendaHostSession? _host;
        private string? _selectedAgendaId;

        public bool IsBound => _host != null;

        public void Bind(HiddenAgendaHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Survivor Intrigue // Hidden Agendas & Betrayals", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active", "Active Agendas", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("clues", "Known Clues", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("exposures", "Exposures", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("betrayals", "Betrayals", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Left column: Scrollable list of active agendas
            var leftScroll = new ScrollContainer();
            leftScroll.CustomMinimumSize = new Vector2(420, 380);
            leftScroll.SizeFlagsHorizontal = SizeFlags.Fill;
            leftScroll.SizeFlagsVertical = SizeFlags.ExpandFill;

            _agendaListContainer = new VBoxContainer();
            _agendaListContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _agendaListContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_agendaListContainer);
            _splitBody.AddChild(leftScroll);

            // Right column: Detailed view & Actions
            _detailContainer = new VBoxContainer();
            _detailContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _detailContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _detailContainer.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _detailContainer.AddChild(_detailText);

            _cluesText = new Label();
            _cluesText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _detailContainer.AddChild(_cluesText);

            var actionsTitle = new Label();
            actionsTitle.Text = "CONFRONTATION & COUNTER-INTEL ACTIONS";
            _detailContainer.AddChild(actionsTitle);

            var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 8);

            _investigateBtn = AshfallUiHelpers.MakeButton("Investigate", () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedAgendaId))
                {
                    var agenda = _host.System.GetAllAgendas().FirstOrDefault(a => a.AgendaId == _selectedAgendaId);
                    if (agenda != null)
                    {
                        _host.Investigate(agenda.SurvivorId, 25f, 1);
                    }
                }
            });
            _investigateBtn.CustomMinimumSize = new Vector2(110, 34);
            buttonRow.AddChild(_investigateBtn);

            _reconcileBtn = AshfallUiHelpers.MakeButton("Reconcile", () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedAgendaId))
                {
                    _host.Confront(_selectedAgendaId, AgendaResolution.Reconciled);
                }
            });
            _reconcileBtn.CustomMinimumSize = new Vector2(100, 34);
            buttonRow.AddChild(_reconcileBtn);

            _exploitBtn = AshfallUiHelpers.MakeButton("Exploit", () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedAgendaId))
                {
                    _host.Confront(_selectedAgendaId, AgendaResolution.Exploited);
                }
            });
            _exploitBtn.CustomMinimumSize = new Vector2(90, 34);
            buttonRow.AddChild(_exploitBtn);

            _expelBtn = AshfallUiHelpers.MakeButton("Expel", () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedAgendaId))
                {
                    _host.Confront(_selectedAgendaId, AgendaResolution.Expelled);
                }
            });
            _expelBtn.CustomMinimumSize = new Vector2(90, 34);
            buttonRow.AddChild(_expelBtn);

            _betrayBtn = AshfallUiHelpers.MakeButton("Betray", () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedAgendaId))
                {
                    _host.Confront(_selectedAgendaId, AgendaResolution.Betrayed);
                }
            });
            _betrayBtn.CustomMinimumSize = new Vector2(90, 34);
            buttonRow.AddChild(_betrayBtn);

            _detailContainer.AddChild(buttonRow);
            _splitBody.AddChild(_detailContainer);

            _contentStack.AddChild(_splitBody);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null)
            {
                if (_detailText != null)
                {
                    _detailText.Text = "Hidden Agenda system is offline or uninitialized.";
                }
                if (_cluesText != null) _cluesText.Text = "";
                DisableAllButtons();
                return;
            }

            var agendas = _host.System.GetAllAgendas();
            var clues = _host.System.GetAllClues();
            int activeCount = agendas.Count(a => !a.IsResolved);
            int exposedCount = agendas.Count(a => a.IsDiscovered);
            int betrayedCount = agendas.Count(a => a.Resolution == AgendaResolution.Betrayed);

            _statusRail.Set("active", activeCount.ToString(), activeCount > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("clues", clues.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("exposures", exposedCount.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("betrayals", betrayedCount.ToString(), betrayedCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            // Rebuild agenda list
            foreach (Node child in _agendaListContainer.GetChildren())
            {
                child.QueueFree();
            }

            if (agendas.Count == 0)
            {
                var emptyLabel = new Label();
                emptyLabel.Text = "No active hidden agendas detected among shelter survivors.";
                _agendaListContainer.AddChild(emptyLabel);
                _selectedAgendaId = null;
            }
            else
            {
                if (string.IsNullOrEmpty(_selectedAgendaId) || agendas.All(a => a.AgendaId != _selectedAgendaId))
                {
                    _selectedAgendaId = agendas[0].AgendaId;
                }

                foreach (var agenda in agendas)
                {
                    var itemBtn = new Button();
                    string statusTag = agenda.IsResolved
                        ? $"[{agenda.Resolution.ToString().ToUpperInvariant()}]"
                        : (agenda.IsDiscovered ? "[EXPOSED]" : "[SECRET]");

                    itemBtn.Text = $"{statusTag} Survivor: {agenda.SurvivorId}\nType: {agenda.Type} | Progress: {agenda.DiscoveryProgress:F0}%";
                    itemBtn.Alignment = HorizontalAlignment.Left;
                    itemBtn.CustomMinimumSize = new Vector2(380, 44);

                    string capturedId = agenda.AgendaId;
                    itemBtn.Pressed += () =>
                    {
                        _selectedAgendaId = capturedId;
                        RefreshSelectedAgendaDetails();
                    };

                    _agendaListContainer.AddChild(itemBtn);
                }
            }

            RefreshSelectedAgendaDetails();
        }

        private void RefreshSelectedAgendaDetails()
        {
            if (_host == null)
            {
                DisableAllButtons();
                return;
            }

            if (string.IsNullOrEmpty(_selectedAgendaId))
            {
                _detailText.Text = "Select an agenda from the roster to inspect dossier, clues, and confront survivor.";
                _cluesText.Text = "";
                DisableAllButtons();
                return;
            }

            var agenda = _host.System.GetAllAgendas().FirstOrDefault(a => a.AgendaId == _selectedAgendaId);
            if (agenda == null)
            {
                _detailText.Text = "Selected agenda is no longer active or has been resolved.";
                _cluesText.Text = "";
                DisableAllButtons();
                return;
            }

            var agendaClues = _host.System.GetCluesForAgenda(agenda.AgendaId);

            _detailText.Text = $"=== DOSSIER: {agenda.AgendaId} ===\n" +
                               $"Survivor: {agenda.SurvivorId}\n" +
                               $"Agenda Type: {agenda.Type}\n" +
                               $"Target Faction: {(string.IsNullOrEmpty(agenda.TargetFactionId) ? "None" : agenda.TargetFactionId)}\n" +
                               $"Target Survivor: {(string.IsNullOrEmpty(agenda.TargetSurvivorId) ? "None" : agenda.TargetSurvivorId)}\n" +
                               $"Discovery Progress: {agenda.DiscoveryProgress:F0}%\n" +
                               $"Discovered: {(agenda.IsDiscovered ? "YES" : "NO")}\n" +
                               $"Confronted: {(agenda.IsConfronted ? "YES" : "NO")}\n" +
                               $"Resolved: {(agenda.IsResolved ? $"YES ({agenda.Resolution})" : "NO")}\n" +
                               $"Start Day: {agenda.StartDay}\n" +
                               $"Notes: {agenda.Notes}";

            if (agendaClues.Count == 0)
            {
                _cluesText.Text = "Discovered Clues: None yet. Conduct investigations to uncover behavioral anomalies.";
            }
            else
            {
                _cluesText.Text = "Discovered Clues:\n" + string.Join("\n", agendaClues.Select(c => $"- Day {c.Day}: {c.Description} (Strength: {c.ClueStrength:F0})"));
            }

            bool canInvestigate = !agenda.IsDiscovered && !agenda.IsResolved;
            bool canConfront = agenda.IsDiscovered && !agenda.IsResolved;

            if (_investigateBtn != null) _investigateBtn.Disabled = !canInvestigate;
            if (_reconcileBtn != null) _reconcileBtn.Disabled = !canConfront;
            if (_exploitBtn != null) _exploitBtn.Disabled = !canConfront;
            if (_expelBtn != null) _expelBtn.Disabled = !canConfront;
            if (_betrayBtn != null) _betrayBtn.Disabled = !canConfront;
        }

        private void DisableAllButtons()
        {
            if (_investigateBtn != null) _investigateBtn.Disabled = true;
            if (_reconcileBtn != null) _reconcileBtn.Disabled = true;
            if (_exploitBtn != null) _exploitBtn.Disabled = true;
            if (_expelBtn != null) _expelBtn.Disabled = true;
            if (_betrayBtn != null) _betrayBtn.Disabled = true;
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
