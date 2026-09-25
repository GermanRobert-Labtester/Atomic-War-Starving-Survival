// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using AtomicWar.GodotApp;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>Shared dashboard chrome for the three Codex Luna 6 player boards.</summary>
    public abstract partial class PfglOctetBoardPanel : Control
    {
        protected VBoxContainer Content = null!;
        protected Label Feedback = null!;
        protected string FeedbackText = string.Empty;
        private AshfallDashboardShell _shell = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            MouseFilter = MouseFilterEnum.Stop;
            Visible = false;

            _shell = new AshfallDashboardShell(BoardTitle, minWidth: 1120, minHeight: 720);
            _shell.SetAnchorsPreset(LayoutPreset.FullRect);
            _shell.AttachHeaderCloseButton("CLOSE · ESC", Close);

            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            Content = new VBoxContainer();
            Content.AddThemeConstantOverride("separation", 10);
            Content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            Content.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(Content);
            _shell.SetContent(scroll);
            AddChild(_shell);
            RefreshView();
        }

        protected abstract string BoardTitle { get; }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
        }

        public override void _ExitTree() => Unbind();

        protected virtual void Unbind() { }

        public abstract void RefreshView();

        protected void ResetContent()
        {
            if (Content != null)
                AshfallUiHelpers.EmptyChildren(Content);
        }

        protected void AddFeedback()
        {
            Feedback = new Label
            {
                Text = FeedbackText,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            Feedback.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            Content.AddChild(Feedback);
        }

        public void ReportFeedback(string message)
        {
            FeedbackText = message ?? string.Empty;
            if (Feedback != null && GodotObject.IsInstanceValid(Feedback))
                Feedback.Text = FeedbackText;
        }

        protected static Label Body(string text)
        {
            var label = AshfallUiHelpers.MakeBody(text);
            label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            return label;
        }

        public override void _UnhandledKeyInput(InputEvent @event)
        {
            if (Visible && @event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    /// <summary>Read-only overview over the existing romance and family owner.</summary>
    public partial class RomanceFamilyBoardPanel : PfglOctetBoardPanel
    {
        private RomanceFamilyHostSession? _host;
        protected override string BoardTitle => "Romance & Family Board // Bonds and Kin";

        public void Bind(RomanceFamilyHostSession? host)
        {
            if (ReferenceEquals(_host, host))
            {
                RefreshView();
                return;
            }
            Unbind();
            _host = host;
            if (_host != null) _host.StateChanged += RefreshView;
            RefreshView();
        }

        protected override void Unbind()
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void RefreshView()
        {
            if (Content == null) return;
            ResetContent();
            if (_host == null)
            {
                Content.AddChild(Body("The romance and family host is not available."));
                return;
            }

            var system = _host.System;
            var census = system.GetCensus();
            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("CURRENT HOUSEHOLD BONDS"));
            Content.AddChild(Body($"Relationships: {census.TotalRelationships} · partnered: {census.PartnershipCount} · bonded: {census.BondedCount} · families: {census.TotalFamilies} · children recorded: {census.TotalChildren}"));

            foreach (var relationship in system.Relationships
                         .OrderBy(r => r.StartDay)
                         .ThenBy(r => r.SurvivorA, StringComparer.Ordinal)
                         .ThenBy(r => r.SurvivorB, StringComparer.Ordinal))
            {
                string soulmate = relationship.IsSoulmate ? " · soulmate bond" : string.Empty;
                Content.AddChild(Body($"{relationship.SurvivorA} ↔ {relationship.SurvivorB} · {relationship.Stage} · score {relationship.RomanceScore}/100 · compatibility {relationship.Compatibility:0}%{soulmate}"));
            }

            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("FAMILY UNITS"));
            foreach (var family in system.FamilyUnits.OrderBy(f => f.FamilyName, StringComparer.Ordinal))
            {
                string parents = family.ParentIds.Count == 0 ? "none recorded" : string.Join(", ", family.ParentIds);
                string children = family.ChildIds.Count == 0 ? "none recorded" : string.Join(", ", family.ChildIds);
                Content.AddChild(Body($"{family.FamilyName} · bond {family.FamilyBond:0}/100 · parents: {parents} · children: {children}"));
            }

            if (system.Relationships.Count == 0 && system.FamilyUnits.Count == 0)
                Content.AddChild(Body("No romance or family records are currently on file. The board reflects the campaign record and does not create relationships."));
            Content.AddChild(Body("Relationship formation, courtship, family changes, and day progression remain with the existing campaign host."));
            AddFeedback();
        }
    }

    /// <summary>Colony status and authored establishment/supply-line controls over ColonySystem.</summary>
    public partial class ColonyOperationsBoardPanel : PfglOctetBoardPanel
    {
        private ColonySystem? _system;
        private string[] _knownLocations = Array.Empty<string>();
        private int _currentDay = 1;
        public event Action<string, string, ColonyType>? EstablishRequested;
        public event Action<string, SupplyLineStatus>? SupplyLineStatusRequested;
        protected override string BoardTitle => "Colony Operations // Forward Settlements";

        public void Bind(ColonySystem? system, IEnumerable<string>? knownLocations, int currentDay)
        {
            _system = system;
            _knownLocations = (knownLocations ?? Array.Empty<string>())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(id => id, StringComparer.Ordinal)
                .ToArray();
            _currentDay = Math.Max(1, currentDay);
            RefreshView();
        }

        public override void RefreshView()
        {
            if (Content == null) return;
            ResetContent();
            if (_system == null)
            {
                Content.AddChild(Body("The colony host is not available."));
                return;
            }

            ColonyState state = _system.CaptureState();
            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("NETWORK STATUS"));
            Content.AddChild(Body($"Settlements: {_system.TotalColonyCount} · active supply lines: {_system.ActiveSupplyLineCount}"));

            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("ESTABLISH AT A KNOWN EXPEDITION LOCATION"));
            var freeLocations = _knownLocations
                .Where(id => !state.Colonies.Any(c => string.Equals(c.LocationId, id, StringComparison.OrdinalIgnoreCase)))
                .ToArray();
            if (freeLocations.Length == 0)
            {
                Content.AddChild(Body("No unoccupied known expedition locations are available."));
            }
            else
            {
                var row = new HBoxContainer();
                var location = new OptionButton { CustomMinimumSize = new Vector2(260, 36), SizeFlagsHorizontal = SizeFlags.ExpandFill };
                foreach (string id in freeLocations) location.AddItem(id);
                var name = new LineEdit { PlaceholderText = "Settlement name", CustomMinimumSize = new Vector2(240, 36), SizeFlagsHorizontal = SizeFlags.ExpandFill };
                var type = new OptionButton { CustomMinimumSize = new Vector2(220, 36) };
                var types = _system.AuthoredTypes;
                foreach (var definition in types)
                {
                    if (!TryParseColonyType(definition.TypeId, out var parsed)) continue;
                    type.AddItem(definition.DisplayName);
                    type.SetItemMetadata(type.ItemCount - 1, (int)parsed);
                }
                if (type.ItemCount > 0) type.Select(0);
                row.AddChild(location);
                row.AddChild(name);
                row.AddChild(type);
                row.AddChild(AshfallUiHelpers.MakeButton("ESTABLISH", () =>
                {
                    if (location.Selected < 0 || type.Selected < 0) return;
                    string locationId = location.GetItemText(location.Selected);
                    string colonyName = name.Text.Trim();
                    var colonyType = (ColonyType)(int)type.GetItemMetadata(type.Selected);
                    EstablishRequested?.Invoke(locationId, colonyName, colonyType);
                }));
                Content.AddChild(row);
                Content.AddChild(Body("The current colony command uses its authored Core defaults for initial supplies. This board does not debit shelter inventory or claim material costs that the current command does not enforce."));
            }

            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("SETTLEMENTS"));
            foreach (var colony in state.Colonies.OrderBy(c => c.EstablishedDay).ThenBy(c => c.ColonyId, StringComparer.Ordinal))
            {
                Content.AddChild(Body($"{colony.Name} · {colony.LocationId} · {colony.Type} · {(colony.IsActive ? "ACTIVE" : "INACTIVE")} · supplies {colony.StoredSupplies:0.0} · morale {colony.MoraleRating:0} · defense {colony.TotalDefense:0} · population {colony.PopulationIds.Count}/{colony.TotalCapacity}"));
                foreach (var building in colony.Buildings.OrderBy(b => b.ConstructionDay).ThenBy(b => b.BuildingId, StringComparer.Ordinal))
                    Content.AddChild(Body($"   {building.Name} · condition {building.Condition:0}% · capacity +{building.Capacity} · defense +{building.DefenseBonus:0}"));
            }
            if (state.Colonies.Count == 0)
                Content.AddChild(Body("No settlements have been established."));

            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("SUPPLY LINES"));
            foreach (var line in state.SupplyLines.OrderBy(s => s.LineId, StringComparer.Ordinal))
            {
                var row = new HBoxContainer();
                row.AddChild(Body($"{line.LineId}: {line.OriginId} → {line.DestinationId} · {line.Status} · {line.DailyFlow:0.0}/day · last supply day {line.LastSupplyDay}"));
                var nextStatus = line.Status == SupplyLineStatus.Active ? SupplyLineStatus.Disrupted : SupplyLineStatus.Active;
                string label = line.Status == SupplyLineStatus.Active ? "DISRUPT" : "RESTORE";
                row.AddChild(AshfallUiHelpers.MakeButton(label, () => SupplyLineStatusRequested?.Invoke(line.LineId, nextStatus)));
                Content.AddChild(row);
            }
            if (state.SupplyLines.Count == 0)
                Content.AddChild(Body("No supply lines are recorded."));
            Content.AddChild(Body($"Campaign day: {_currentDay}. Colony supply flow, consumption, and morale continue to be advanced by the existing colony day owner."));
            AddFeedback();
        }

        private static bool TryParseColonyType(string id, out ColonyType type)
        {
            type = id?.ToLowerInvariant() switch
            {
                "outpost" => ColonyType.Outpost,
                "settlement" => ColonyType.Settlement,
                "fortress" => ColonyType.Fortress,
                "trading_post" => ColonyType.TradingPost,
                "farming_commune" => ColonyType.FarmingCommune,
                _ => (ColonyType)(-1)
            };
            return Enum.IsDefined(typeof(ColonyType), type);
        }
    }

    /// <summary>Explicit mediation choices over unresolved ideological Core events.</summary>
    public partial class IdeologicalMediationDeskPanel : PfglOctetBoardPanel
    {
        private IdeologicalFrictionHostSession? _host;
        public event Action<string, IdeologicalMediationChoice>? MediationRequested;
        protected override string BoardTitle => "Ideological Mediation Desk // Shelter Disputes";

        public void Bind(IdeologicalFrictionHostSession? host)
        {
            if (ReferenceEquals(_host, host))
            {
                RefreshView();
                return;
            }
            Unbind();
            _host = host;
            if (_host != null) _host.StateChanged += RefreshView;
            RefreshView();
        }

        protected override void Unbind()
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void RefreshView()
        {
            if (Content == null) return;
            ResetContent();
            if (_host == null)
            {
                Content.AddChild(Body("The ideological friction host is not available."));
                return;
            }

            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("OPEN CONFRONTATIONS"));
            var open = _host.RecentEvents
                .Where(e => !e.isResolved && (e.eventType == IdeologicalEventType.Confrontation || e.eventType == IdeologicalEventType.MediationQuest))
                .OrderBy(e => e.day)
                .ThenBy(e => e.instanceId, StringComparer.Ordinal)
                .ToArray();
            foreach (var incident in open)
            {
                Content.AddChild(Body($"Day {incident.day} · {incident.title}\n{incident.actorId} ({incident.actorBelief}) / {incident.targetId} ({incident.targetBelief})\n{incident.description}"));
                var row = new HBoxContainer();
                AddChoice(row, "SIDE WITH ACTOR", incident.instanceId, IdeologicalMediationChoice.SideWithActor);
                AddChoice(row, "SIDE WITH TARGET", incident.instanceId, IdeologicalMediationChoice.SideWithTarget);
                AddChoice(row, "STAY NEUTRAL", incident.instanceId, IdeologicalMediationChoice.StayNeutral);
                AddChoice(row, "BROKER COMPROMISE", incident.instanceId, IdeologicalMediationChoice.BrokerCompromise);
                Content.AddChild(row);
            }
            if (open.Length == 0)
                Content.AddChild(Body("There are no unresolved confrontation or mediation events."));

            Content.AddChild(AshfallUiHelpers.MakeSectionHeader("FACTIONS"));
            foreach (var faction in _host.ActiveFactions.OrderBy(f => f.beliefId, StringComparer.Ordinal))
                Content.AddChild(Body($"{faction.beliefId.Replace('_', ' ')} · {faction.memberIds.Count} members · leader {faction.leaderId}"));
            if (_host.ActiveFactions.Count == 0)
                Content.AddChild(Body("No active ideological factions are recorded."));
            AddFeedback();
        }

        private void AddChoice(HBoxContainer row, string label, string instanceId, IdeologicalMediationChoice choice)
        {
            row.AddChild(AshfallUiHelpers.MakeButton(label, () => MediationRequested?.Invoke(instanceId, choice)));
        }
    }
}
