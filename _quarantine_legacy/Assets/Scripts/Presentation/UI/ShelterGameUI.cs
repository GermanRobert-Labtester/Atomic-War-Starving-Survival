using System.Collections.Generic;
using AtomicWar.Core.Events;
using AtomicWar.Core.Save;
using AtomicWar.Core.Services;
using AtomicWar.Data;
using AtomicWar.Runtime.Crafting;
using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.RandomEvents;
using AtomicWar.Runtime.Scavenging;
using AtomicWar.Runtime.Survivors;
using AtomicWar.Runtime.Time;
using UnityEngine;

namespace AtomicWar.Presentation.UI
{
    /// <summary>
    /// Complete prototype HUD binding allowing full interactive testing in Unity:
    /// - Day/Night clock and speed controls
    /// - Survivor stats (Hunger, Fatigue, Health, Sickness, Morale)
    /// - Actions (Eat, Sleep, Guard, Craft Bandage)
    /// - Night Scavenging (Abandoned Pharmacy)
    /// - Save & Load buttons
    /// </summary>
    public class ShelterGameUI : MonoBehaviour
    {
        private GameStateSystem _gameStateSystem;
        private TimeSystem _timeSystem;
        private SurvivorSystem _survivorSystem;
        private LegacyVitalsSystem _legacyVitalsSystem;
        private InventorySystem _inventorySystem;
        private CraftingSystem _craftingSystem;
        private ScavengingSystem _scavengingSystem;
        private SaveSystem _saveSystem;

        private GameBootstrap _bootstrap;
        private readonly List<string> _eventLog = new List<string>();
        private Vector2 _logScroll = Vector2.zero;

        private void OnEnable()
        {
            EventBus.Subscribe<PhaseChangedEvent>(OnPhaseChanged);
            EventBus.Subscribe<HourTickEvent>(OnHourTick);
            EventBus.Subscribe<NightRaidEventTriggered>(OnNightRaid);
            EventBus.Subscribe<ScavengeCompletedEvent>(OnScavengeCompleted);
            EventBus.Subscribe<NeedStatusAlertEvent>(OnNeedAlert);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PhaseChangedEvent>(OnPhaseChanged);
            EventBus.Unsubscribe<HourTickEvent>(OnHourTick);
            EventBus.Unsubscribe<NightRaidEventTriggered>(OnNightRaid);
            EventBus.Unsubscribe<ScavengeCompletedEvent>(OnScavengeCompleted);
            EventBus.Unsubscribe<NeedStatusAlertEvent>(OnNeedAlert);
        }

        private void Start()
        {
            _gameStateSystem = ServiceLocator.Get<GameStateSystem>();
            _timeSystem = ServiceLocator.Get<TimeSystem>();
            _survivorSystem = ServiceLocator.Get<SurvivorSystem>();
            _legacyVitalsSystem = ServiceLocator.Get<LegacyVitalsSystem>();
            _inventorySystem = ServiceLocator.Get<InventorySystem>();
            _craftingSystem = ServiceLocator.Get<CraftingSystem>();
            _scavengingSystem = ServiceLocator.Get<ScavengingSystem>();
            _saveSystem = ServiceLocator.Get<SaveSystem>();

            _bootstrap = GetComponent<GameBootstrap>() ?? FindObjectOfType<GameBootstrap>();

            AddLog("Shelter Scene initialized. Survival cycle started.");
        }

        private void OnPhaseChanged(PhaseChangedEvent e)
        {
            AddLog($"Phase Changed -> Day {e.DayNumber} [{e.NewPhase}]");
        }

        private void OnHourTick(HourTickEvent e)
        {
            // Optional log for hour ticks
        }

        private void OnNightRaid(NightRaidEventTriggered e)
        {
            AddLog($"[NIGHT RAID] {e.RaidMessage}");
        }

        private void OnScavengeCompleted(ScavengeCompletedEvent e)
        {
            AddLog($"[SCAVENGE] {e.SummaryReport}");
        }

        private void OnNeedAlert(NeedStatusAlertEvent e)
        {
            AddLog($"[ALERT] {e.AlertMessage}");
        }

        private void AddLog(string msg)
        {
            _eventLog.Insert(0, $"[{System.DateTime.Now:HH:mm:ss}] {msg}");
            if (_eventLog.Count > 15) _eventLog.RemoveAt(_eventLog.Count - 1);
        }

        private void OnGUI()
        {
            GUI.skin.box.fontSize = 14;
            GUI.skin.button.fontSize = 13;
            GUI.skin.label.fontSize = 13;

            // Draw Top Day/Night Bar
            GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, 80), GUI.skin.box);
            GUILayout.BeginHorizontal();

            string phaseText = _gameStateSystem != null ? _gameStateSystem.CurrentPhase.ToString() : "Day";
            int dayText = _gameStateSystem != null ? _gameStateSystem.DayNumber : 1;
            int hourText = _timeSystem != null ? (int)_timeSystem.CurrentHour : 6;
            bool isPaused = _timeSystem != null && _timeSystem.IsPaused;

            GUILayout.Label($"<b>DAY {dayText}</b> | <b>PHASE: {phaseText.ToUpper()}</b> | Time: {hourText:00}:00", GUILayout.Width(350));

            if (GUILayout.Button(isPaused ? "RESUME" : "PAUSE", GUILayout.Width(100), GUILayout.Height(30)))
            {
                _timeSystem?.TogglePause();
            }

            if (GUILayout.Button("ADVANCE HOUR", GUILayout.Width(130), GUILayout.Height(30)))
            {
                _timeSystem?.AdvanceHour();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("SAVE GAME", GUILayout.Width(110), GUILayout.Height(30)))
            {
                SaveCurrentGame();
            }

            if (GUILayout.Button("LOAD GAME", GUILayout.Width(110), GUILayout.Height(30)))
            {
                LoadSavedGame();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            // Draw Left Column: Survivors Panel
            GUILayout.BeginArea(new Rect(10, 100, 480, Screen.height - 110), GUI.skin.box);
            GUILayout.Label("<b>SURVIVORS IN SHELTER</b>");
            GUILayout.Space(5);

            if (_survivorSystem != null)
            {
                foreach (var survivor in _survivorSystem.ActiveSurvivors)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label($"<b>{survivor.Data?.CharacterName ?? "Survivor"}</b> - Status: <i>{survivor.CurrentState}</i>");
                    GUILayout.Label($"Health: {survivor.Health:F0}% | Hunger: {survivor.Hunger:F0}% | Fatigue: {survivor.Fatigue:F0}% | Sickness: {survivor.Sickness:F0}% | Morale: {survivor.Morale:F0}%");

                    GUILayout.BeginHorizontal();

                    // Action: Eat
                    if (GUILayout.Button("EAT FOOD", GUILayout.Width(100)))
                    {
                        if (_inventorySystem.HasItemAmount("item_food", 1))
                        {
                            _inventorySystem.RemoveItem(_bootstrap.FoodItem, 1);
                            _legacyVitalsSystem.ModifyHunger(survivor, -40f);
                            AddLog($"{survivor.Data?.CharacterName} ate food (-40 Hunger).");
                        }
                        else
                        {
                            AddLog("No canned food available in inventory!");
                        }
                    }

                    // Action: Sleep
                    if (GUILayout.Button(survivor.CurrentState == SurvivorState.Sleeping ? "WAKE UP" : "SLEEP", GUILayout.Width(90)))
                    {
                        survivor.CurrentState = survivor.CurrentState == SurvivorState.Sleeping ? SurvivorState.Idle : SurvivorState.Sleeping;
                        AddLog($"{survivor.Data?.CharacterName} state set to {survivor.CurrentState}.");
                    }

                    // Action: Guard
                    if (GUILayout.Button(survivor.CurrentState == SurvivorState.Guard ? "STOP GUARD" : "GUARD", GUILayout.Width(100)))
                    {
                        survivor.CurrentState = survivor.CurrentState == SurvivorState.Guard ? SurvivorState.Idle : SurvivorState.Guard;
                        AddLog($"{survivor.Data?.CharacterName} state set to {survivor.CurrentState}.");
                    }

                    // Action: Craft Bandage
                    if (GUILayout.Button("CRAFT BANDAGE", GUILayout.Width(130)))
                    {
                        if (_craftingSystem.CanCraft(_bootstrap.BandageRecipe))
                        {
                            _craftingSystem.StartCrafting(_bootstrap.BandageRecipe, survivor);
                            AddLog($"{survivor.Data?.CharacterName} started crafting Bandage.");
                        }
                        else
                        {
                            AddLog("Requires 2 Rags & Cloth to craft Bandage!");
                        }
                    }

                    GUILayout.EndHorizontal();

                    // Night Scavenging Assignment
                    if (_gameStateSystem != null && (_gameStateSystem.CurrentPhase == DayCyclePhase.Evening || _gameStateSystem.CurrentPhase == DayCyclePhase.Night))
                    {
                        if (GUILayout.Button($"SCAVENGE AT {_bootstrap.PharmacyLocation.LocationName.ToUpper()}", GUILayout.Height(25)))
                        {
                            if (_scavengingSystem.PrepareScavengeRun(survivor, _bootstrap.PharmacyLocation))
                            {
                                AddLog($"{survivor.Data?.CharacterName} dispatched to {_bootstrap.PharmacyLocation.LocationName}!");
                                _scavengingSystem.ResolveNightRun();
                            }
                        }
                    }

                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }
            }

            GUILayout.EndArea();

            // Draw Right Column: Inventory Stockpile & Event Log
            GUILayout.BeginArea(new Rect(500, 100, Screen.width - 510, Screen.height - 110), GUI.skin.box);
            GUILayout.Label("<b>SHELTER STOCKPILE INVENTORY</b>");
            GUILayout.Space(5);

            if (_inventorySystem != null)
            {
                int foodCount = _inventorySystem.GetItemCount("item_food");
                int clothCount = _inventorySystem.GetItemCount("item_cloth");
                int bandageCount = _inventorySystem.GetItemCount("item_bandage");

                GUILayout.Label($"• Canned Food Tins: <b>{foodCount}</b>");
                GUILayout.Label($"• Rags & Cloth: <b>{clothCount}</b>");
                GUILayout.Label($"• Sterile Bandages: <b>{bandageCount}</b>");
                GUILayout.Label($"• Total Stockpile Weight: <b>{_inventorySystem.TotalWeight:F1} kg</b>");
            }

            GUILayout.Space(15);
            GUILayout.Label("<b>EVENT LOG & ALERTS</b>");
            _logScroll = GUILayout.BeginScrollView(_logScroll, GUILayout.Height(250));
            foreach (var log in _eventLog)
            {
                GUILayout.Label(log);
            }
            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        private void SaveCurrentGame()
        {
            if (_saveSystem == null) return;
            var saveData = new GameSaveData
            {
                DayNumber = _gameStateSystem.DayNumber,
                CurrentPhase = _gameStateSystem.CurrentPhase.ToString()
            };

            foreach (var survivor in _survivorSystem.ActiveSurvivors)
            {
                saveData.Survivors.Add(new SurvivorSaveData
                {
                    Id = survivor.InstanceId,
                    DefinitionId = survivor.Data?.Id ?? "survivor",
                    Health = survivor.Health,
                    Hunger = survivor.Hunger,
                    Fatigue = survivor.Fatigue,
                    Morale = survivor.Morale,
                    Status = survivor.CurrentState.ToString()
                });
            }

            _saveSystem.Save(saveData);
            AddLog("Game progress saved to JSON file.");
        }

        private void LoadSavedGame()
        {
            if (_saveSystem == null) return;
            var saveData = _saveSystem.Load();
            if (saveData != null)
            {
                _gameStateSystem.RestoreState(new DayCycleSaveData
                {
                    DayNumber = saveData.DayNumber,
                    CurrentPhase = saveData.CurrentPhase
                });

                int i = 0;
                foreach (var sData in saveData.Survivors)
                {
                    if (i < _survivorSystem.ActiveSurvivors.Count)
                    {
                        var survivor = _survivorSystem.ActiveSurvivors[i];
                        survivor.Health = sData.Health;
                        survivor.Hunger = sData.Hunger;
                        survivor.Fatigue = sData.Fatigue;
                        survivor.Morale = sData.Morale;
                    }
                    i++;
                }

                AddLog("Game progress loaded from JSON save file.");
            }
        }
    }
}
