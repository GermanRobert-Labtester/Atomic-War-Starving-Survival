using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Pay the warlord's tribute from the canonical Holdfast inventory.
        /// Consumption happens here (the inventory authority); settlement and
        /// every consequence run in Core. The collector's reply is authored
        /// prose from the catalog, surfaced as the panel note and a log line.
        /// </summary>
        private void PayWarlordTribute(int amount)
        {
            if (_yearOfAsh?.Warlord == null || _holdfastRuntime?.Trade.Inventory == null) return;
            int day = _yearOfAsh.Timeline.CurrentDay;
            var inventory = _holdfastRuntime.Trade.Inventory;
            string item = _yearOfAsh.Warlord.Catalog.Warlord.tribute_currency_item;
            if (!inventory.Items.TryGetValue(item, out int held) || held < amount)
            {
                GD.Print($"[warlord] Tribute refused by shortage: {amount}× {item} needed, {held} on hand.");
                _statusLabel.Text = $"The collector waits. You do not have {amount}× {item} to hand over.";
                return;
            }
            inventory.RemoveItem(item, amount);
            int next;
            bool full = _yearOfAsh.SettleWarlordTribute(amount, day, out next);
            string line = _yearOfAsh.CollectorLine(full ? "paid" : "short", day);
            GD.Print($"[warlord] Tribute paid: {amount}× {item} (day {day}). {line}");
            _statusLabel.Text = line;
            _yearOfAshDirty = true;
        }

        private void RefuseWarlordTribute()
        {
            if (_yearOfAsh?.Warlord == null) return;
            int day = _yearOfAsh.Timeline.CurrentDay;
            int next;
            _yearOfAsh.SettleWarlordTribute(0, day, out next);
            string line = _yearOfAsh.CollectorLine("refused", day);
            GD.Print($"[warlord] Tribute refused (day {day}). Next ask: {next}. {line}");
            _statusLabel.Text = line;
            _yearOfAshDirty = true;
        }

        private void SaveYearOfAsh()
        {
            if (_yearOfAsh == null) return;
            if (YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave()))
            {
                _yearOfAshDirty = false;
                GD.Print("[Ashfall Godot] Year of Ash save written.");
            }
        }

        private void FlushYearOfAshIfDirty()
        {
            if (_yearOfAshDirty) SaveYearOfAsh();
        }

        private void SetupYearOfAsh()
        {
            if (_yearOfAsh != null) return;
            _yearOfAsh = YearOfAshHostSession.Create(_dataDir);
            BuildYearOfAshPanel();

            // Questline progress rides the same save as the rest of Year of Ash, so any
            // resolution marks it dirty exactly like an encounter does.
            _yearOfAsh.Quests.OnQuestlineStarted += def =>
                GD.Print($"[Ashfall Godot] Questline started: {def.questlineId}");
            _yearOfAsh.Quests.OnQuestlineResolved += (id, status) =>
            {
                _yearOfAshDirty = true;
                GD.Print($"[Ashfall Godot] Questline {id} → {status}");
            };
            _yearOfAsh.Quests.OnQuestChoiceTaken += _ => _yearOfAshDirty = true;

            int playable = _yearOfAsh.Quests.GetPlayableQuestlines(_yearOfAsh.Timeline.CurrentDay).Count;
            int withheld = _yearOfAsh.Quests.WithheldQuestlineCount(_yearOfAsh.Timeline.CurrentDay);
            GD.Print($"[Ashfall Godot] Year of Ash ready. Day {_yearOfAsh.Timeline.CurrentDay} · " +
                     $"questlines: {playable} playable, {withheld} withheld (no authored choices)");

            WireWarlordPlayerFacing();
            WireWarlordExpeditionDanger();
            RefreshWarlordTargets();
        }

        /// <summary>
        /// Thin consequence wiring: warlord-controlled/contested ground raises
        /// the encounter chance of real sorties to those locations (the Core
        /// ExpeditionSystem multiplier hook). The warlord system owns the danger
        /// number; this only routes it.
        /// </summary>
        private void WireWarlordExpeditionDanger()
        {
            if (_expeditions == null) return;
            _expeditions.SetEncounterChanceMultiplier(locationId =>
            {
                var w = _yearOfAsh?.Warlord;
                if (w == null) return 1f;
                float mod = w.TravelDangerModifier(locationId);
                return mod > 0f ? 1f + mod : 1f;
            });
        }

        /// <summary>
        /// Registers the warlord territory nodes as expedition targets so the
        /// road-danger consequence is felt on actual sorties (Toll House,
        /// weighbridge, cut substation, convoy apron, grain silo). The encounter
        /// multiplier above supplies the dynamic warlord pressure; these are the
        /// static destination cards.
        /// </summary>
        private void RefreshWarlordTargets()
        {
            if (_yearOfAsh?.Warlord == null) return;
            var catalog = _yearOfAsh.Warlord.Catalog;
            for (int i = 0; i < catalog.Territory.Count; i++)
            {
                var node = catalog.Territory[i];
                if (node == null || string.IsNullOrEmpty(node.location_id)) continue;
                if (ExpeditionDefinitionRegistry.Get(node.location_id) != null) continue;
                ExpeditionDefinitionRegistry.Register(new ExpeditionDefinition
                {
                    id = node.location_id,
                    displayName = node.home ? "The Toll House" : node.location_id,
                    distanceTicks = 10 + node.supply_value,
                    dangerLevel = 5 + node.defense_value,
                    encounterChancePerTick = 0.14f,
                    baseStaminaDrainPerHour = 2.6f + node.supply_value * 0.2f,
                    lootCategories = new System.Collections.Generic.List<string>
                        { "scrap_metal", "canned_food", "fuel" }
                });
            }
        }

        /// <summary>
        /// Thin player-facing consequence wiring for the warlord AI: doctrine
        /// shifts and hostile actions land in the real journal (once-only keys)
        /// and the radio history (RaidWarning intercepts under the canonical
        /// warlords_sector_4 identity). No rules live here — the warlord system
        /// emits the intents; this surfaces them.
        /// </summary>
        private void WireWarlordPlayerFacing()
        {
            var warlord = _yearOfAsh?.Warlord;
            if (warlord == null) return;
            var author = new AtomicWar.Journal.DemoSurvivor("warlords_sector_4", "The Tollman", Ashfall.Core.Journal.RiskBiasTrait.Reckless);
            warlord.OnNarrativeRequested += (journalKey, radioKey) =>
            {
                string text = WarlordNarrativeText(journalKey);
                if (!string.IsNullOrEmpty(text))
                    _journal?.TryAddRawEntry(journalKey, text, author, _yearOfAsh!.Timeline.CurrentDay);
                if (!string.IsNullOrEmpty(radioKey))
                    _radio?.InterceptWarlordWarning(WarlordRadioText(radioKey), _yearOfAsh!.Timeline.CurrentDay);
                _yearOfAshDirty = true;
            };
            warlord.OnTributeDemanded += (amount, item, day) =>
            {
                GD.Print($"[warlord] Collector calls: {amount}× {item} (day {day}).");
                _yearOfAshDirty = true;
            };
            warlord.OnDoctrineChanged += (from, to, reason, day) =>
            {
                GD.Print($"[warlord] Doctrine {from} → {to} (day {day}): {reason}");
                _yearOfAshDirty = true;
            };
        }

        private static string WarlordNarrativeText(string journalKey)
        {
            switch (journalKey)
            {
                case "journal_warlord_toll_doctrine":
                    return "The boom is up and the price is known — pay, pass, and nobody learns your name. The Tollman keeps that contract through two governors and a war that forgot to end. The day he has to explain the price is the day it stops being his to set.";
                case "journal_warlord_consolidation_doctrine":
                    return "Too many fires on the cut this season. Ground the Warlords do not hold is ground they do not have to defend, so they are holding less of it. The checkpoints stay; the ambition goes into a drawer with the maps.";
                case "journal_warlord_annexation_doctrine":
                    return "The weighbridge answers to the Toll House now: the scale, then the road it serves, then the ground under both. They will call it a land grab. He calls it a longer price list, and the rates are posted before the ink dries.";
                case "journal_warlord_withdrawal_doctrine":
                    return "The lamps are out and the door is locked. Not surrender — arithmetic. The weather has a column of its own and it does not pay tolls. The road can wait. He has taught it patience.";
                default:
                    return string.Empty;
            }
        }

        private static string WarlordRadioText(string radioKey)
        {
            switch (radioKey)
            {
                case "radio_warlord_toll_standing":
                    return "This is the Toll House. The boom is up. The price is the price — same as last week, higher if you make it higher. Pay in food, pay in fuel, pay in patience.";
                case "radio_warlord_consolidation":
                    return "Toll House relay. Nothing moving, nothing burning. We hold what we hold and are not interested in what we do not. That is a kindness. Do not test it.";
                case "radio_warlord_annexation":
                    return "Toll House relay. New ground, new checkpoints. The weighbridge answers to the Toll House now. The map is being repainted. Check your chits against the new rates.";
                case "radio_warlord_withdrawal":
                    return "Toll House relay. The boom is down, the lamps are out. The road is yours again, all of it. Enjoy it. It will not be cheap to get back.";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Wires the four Year-of-Ash presentation widgets (faction war map, radio
        /// terminal, geothermal heating, radon ventilation) into the right column.
        /// They were authored but never instantiated — dead presentation code.
        /// Widgets are added to the tree before BindSession so their _Ready has run
        /// and the labels exist when the first RefreshView fires.
        /// </summary>
        private void BuildYearOfAshPanel()
        {
            if (_yearOfAshPanel != null || _rightColumn == null || _yearOfAsh == null) return;

            _yearOfAshPanel = new VBoxContainer();
            _yearOfAshPanel.AddThemeConstantOverride("separation", 8);

            var header = new Label
            {
                Text = "YEAR OF ASH — SYSTEMS (DAYS 180–360)"
            };
            header.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH3);
            header.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            _yearOfAshPanel.AddChild(header);

            _factionWarMap = new FactionWarMapWidget();
            _geothermalWidget = new GeothermalHeatingWidget();
            _radonWidget = new RadonVentilationWidget();
            _radioTerminal = new RadioBroadcastTerminal();

            _yearOfAshPanel.AddChild(_factionWarMap);
            _yearOfAshPanel.AddChild(_geothermalWidget);
            _yearOfAshPanel.AddChild(_radonWidget);
            _yearOfAshPanel.AddChild(_radioTerminal);

            // Enter the tree first so each widget's _Ready has built its labels.
            _rightColumn.AddChild(_yearOfAshPanel);

            _factionWarMap.BindSession(_yearOfAsh);
            _geothermalWidget.BindSession(_yearOfAsh);
            _radonWidget.BindSession(_yearOfAsh);
            _radioTerminal.LoadBroadcasts(_dataDir);
            _radioTerminal.RefreshView(_yearOfAsh.Timeline.CurrentDay);
        }

        private void OnDoorEncounterClicked()
        {
            SetupYearOfAsh();
            int today = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            var eligible = _yearOfAsh!.Encounters.GetEligibleEncounters(today);
            if (eligible.Count == 0)
            {
                _statusLabel.Text = "No door encounters eligible today (one-shots spent or beyond season cap).";
                return;
            }

            var enc = eligible[_doorEncounterIndex % eligible.Count];
            _doorEncounterIndex++;
            _doorModal.DisplayEncounter(enc, _yearOfAsh.DemoRoster);
            _statusLabel.Text = $"Shelter door visitor arrived: {enc.visitorName}.";
        }

        private void OnDoorEncounterChoiceClicked(DoorEncounterEntry encounter, EncounterChoice choice)
        {
            if (_yearOfAsh == null) return;
            var result = _yearOfAsh.Encounters.ResolveChoice(encounter, choice, _yearOfAsh.DemoRoster);
            _doorModal.DisplayResolution(result);
            _statusLabel.Text = $"Encounter resolved: {encounter.visitorName}. Morale: {result.netMoraleDelta:+#;-#;0}, Guilt: {result.netGuiltDelta:+#;-#;0}";
            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());
        }

        /// <summary>
        /// Opens the questline ledger. Resumes the first active questline if one is in
        /// flight, otherwise offers what can be started today.
        /// </summary>
        private void OnQuestlinesClicked()
        {
            SetupYearOfAsh();
            int day = _yearOfAsh.Timeline.CurrentDay;

            var active = _yearOfAsh.Quests.State.active
                .Find(a => a.status == QuestlineStatus.Active);
            if (active != null && ShowQuestlineStage(active.questlineId, day))
            {
                _statusLabel.Text = $"Questline in progress: {active.questlineId} (day {day}).";
                return;
            }

            var offers = _yearOfAsh.Quests.GetPlayableQuestlines(day);
            int withheld = _yearOfAsh.Quests.WithheldQuestlineCount(day);
            _questlineModal.DisplayOffers(offers, day, withheld);
            _statusLabel.Text = withheld > 0
                ? $"{offers.Count} questlines open on day {day}. {withheld} withheld — no authored choices."
                : $"{offers.Count} questlines open on day {day}.";
        }

        /// <summary>Renders the current stage of an active questline. False if it cannot.</summary>
        private bool ShowQuestlineStage(string questlineId, int day)
        {
            var record = _yearOfAsh.Quests.GetActiveRecord(questlineId);
            var def = _yearOfAsh.Quests.FindDefinition(questlineId);
            if (record == null || def == null) return false;

            var stage = def.FindStage(record.currentStageId);
            if (stage == null || stage.choices.Count == 0) return false;

            _questlineModal.DisplayStage(def, stage, day);
            return true;
        }

        private void OnQuestlineChosen(QuestlineDefinition def)
        {
            if (_yearOfAsh == null || def == null) return;
            int day = _yearOfAsh.Timeline.CurrentDay;

            if (!_yearOfAsh.Quests.StartQuestline(def.questlineId, day))
            {
                _statusLabel.Text = $"Could not start {def.questlineId} — already active or unknown.";
                return;
            }

            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());
            ShowQuestlineStage(def.questlineId, day);
            _statusLabel.Text = $"Questline begun: {def.title} (day {day}).";
        }

        private void OnQuestlineChoiceTaken(string questlineId, string choiceId)
        {
            if (_yearOfAsh == null) return;
            int day = _yearOfAsh.Timeline.CurrentDay;

            var result = _yearOfAsh.Quests.TakeChoice(questlineId, choiceId, day);
            if (result == null)
            {
                _statusLabel.Text = $"Choice {choiceId} was refused by {questlineId}.";
                return;
            }

            // A choice that moves a faction moves the actual war model, not just text.
            if (!string.IsNullOrEmpty(result.factionId) && result.factionDelta != 0)
                _yearOfAsh.FactionWar.ModifyStanding(result.factionId, result.factionDelta);

            // Grant rewards into the real inventory surface (previously display-only).
            if (!string.IsNullOrEmpty(result.grantItemId) && result.grantItemQty > 0)
            {
                SetupInventory();
                _inventory.Add(result.grantItemId, result.grantItemQty);
                if (_inventoryPanel != null) _inventoryPanel.RefreshView();

                // Journal Items tab reveals the fragment once it is in hand.
                SetupJournal();
                _journal.UnlockItemSeen(result.grantItemId);

                // evidence_* grants enroll into the Verdict's authoritative evidence ledger.
                if (result.grantItemId.StartsWith("evidence_", StringComparison.Ordinal))
                {
                    SetupVerdict();
                    _verdict.Evidence.Enroll(result.grantItemId, day);
                    UnlockVerdictLore();
                }
            }

            bool ended = result.newQuestStatus != QuestlineStatus.Active;
            _questlineModal.DisplayResolution(result, ended);

            // Persist immediately: questline progress is the one Year of Ash surface a
            // player would most obviously expect to survive a quit.
            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());

            _statusLabel.Text = ended
                ? $"{questlineId} → {result.newQuestStatus}. Morale {result.moraleDelta:+#;-#;0}, guilt {result.guiltDelta:+#;-#;0}."
                : $"{questlineId} advanced to {result.nextStageId}.";

            if (!ended) ShowQuestlineStage(questlineId, day);
        }

        private void OnTickYearOfAshClicked()
        {
            SetupYearOfAsh();
            int targetDay = Math.Min(360, _yearOfAsh.Timeline.CurrentDay + 10);
            _yearOfAsh.TickDay(targetDay);
            // Persist after the day advance too, so a quit between ticks doesn't
            // lose the timeline (encounter resolutions already save on their own).
            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());
            AutoEscalateMuster();
            if (_radioTerminal != null)
                _radioTerminal.RefreshView(_yearOfAsh.Timeline.CurrentDay);
            _statusLabel.Text = _yearOfAsh.GetStatusSummary();
            if (_codexViewer != null)
            {
                _codexViewer.Text = $"=== YEAR OF ASH (DAYS 180-360) ===\n{_yearOfAsh.GetStatusSummary()}\n\n" +
                                   $"Phase: {_yearOfAsh.Timeline.CurrentPhase}\n" +
                                   $"Ambient Temp: {_yearOfAsh.Timeline.AmbientTemperatureCelsius:F1}°C\n" +
                                   $"Caloric Multiplier: {_yearOfAsh.Timeline.CalculateCaloricMultiplier():F2}x\n" +
                                   $"Radon Infiltration: {_yearOfAsh.Timeline.RadonInfiltrationRate * 100:F1}%\n" +
                                   $"War Tension: {_yearOfAsh.FactionWar.WarTension}/100\n" +
                                   $"Dominant Faction: {_yearOfAsh.FactionWar.DominantFactionId}\n" +
                                   $"Encounters Available: {_yearOfAsh.Encounters.Catalog.Count}\n";
        }
    }

    }
}
