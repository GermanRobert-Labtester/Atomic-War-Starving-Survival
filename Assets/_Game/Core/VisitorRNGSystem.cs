using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FixedNodeState
    {
        public string locationId;
        public string displayName;
        public string assignedVisitorCardId;
        public string assignedVisitorTitle;
        public string primaryFactionId;
        public string secondaryFactionId;
        public bool isSkirmishActive;
        public string ruinModifier; // Prompt #320
    }

    [Serializable]
    public class FixedMapSaveData
    {
        public int seed;
        public List<FixedNodeState> nodeStates = new List<FixedNodeState>();
    }

    /// <summary>
    /// Prompt #319: System: Fixed Map Nodes & Visitor Decks
    /// Manages 20 permanent map locations and injects random visitor decks at game start.
    /// </summary>
    
    [Serializable]
    public class VisitorRNGSystemSave
    {
        public string systemId = "visitor_rngsystem";

        public List<FixedNodeState> nodes = new List<FixedNodeState>();
    }
/// <summary>DEMOTE-VisitorRNG — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public class VisitorRNGSystem
    {
        public static readonly string[] PermanentLocationIds = new string[20]
        {
            "general_hospital",
            "downtown_precinct",
            "suburban_pharmacy",
            "military_base",
            "city_apartments",
            "subway_station",
            "ruined_church",
            "gas_station",
            "old_bar",
            "military_training_yard",
            "rebel_training_yard",
            "labor_camp",
            "radio_tower",
            "water_treatment_plant",
            "suburban_school",
            "warehouse_district",
            "power_plant",
            "civic_center",
            "subway_depot",
            "railway_yard"
        };

        public static readonly string[] LocationDisplayNames = new string[20]
        {
            "General Hospital",
            "Downtown Precinct",
            "Suburban Pharmacy",
            "Military Base",
            "City Apartments",
            "Subway Station",
            "Ruined Church",
            "Gas Station",
            "Old Bar",
            "Military Training Yard",
            "Rebel Training Yard",
            "Labor Camp",
            "Radio Tower",
            "Water Treatment Plant",
            "Suburban School",
            "Warehouse District",
            "Power Plant",
            "Civic Center",
            "Subway Depot",
            "Railway Yard"
        };

        private readonly Dictionary<string, FixedNodeState> _nodes = new Dictionary<string, FixedNodeState>();
        private readonly Dictionary<string, VisitorCard> _availableCards = new Dictionary<string, VisitorCard>();
        private int _currentSeed;

        public event Action OnFixedNodesInitialized;
        public event Action<string, string> OnNodeVisitorAssigned; // locationId, visitorCardId

        public IReadOnlyDictionary<string, FixedNodeState> Nodes => _nodes;
        public int CurrentSeed => _currentSeed;

        public VisitorRNGSystem()
        {
            RegisterDefaultVisitorCards();
            InitDefaultNodes();
        }

        private void InitDefaultNodes()
        {
            _nodes.Clear();
            for (int i = 0; i < PermanentLocationIds.Length; i++)
            {
                string id = PermanentLocationIds[i];
                string name = LocationDisplayNames[i];
                _nodes[id] = new FixedNodeState
                {
                    locationId = id,
                    displayName = name,
                    assignedVisitorCardId = "visitor_abandoned",
                    assignedVisitorTitle = "Abandoned Ruins",
                    primaryFactionId = "none",
                    secondaryFactionId = "none",
                    isSkirmishActive = false,
                    ruinModifier = "Pristine"
                };
            }
        }

        private void RegisterDefaultVisitorCards()
        {
            _availableCards.Clear();
            AddCard(new VisitorCard("visitor_military_patrol", "Standard Military Patrol", "military"));
            AddCard(new VisitorCard("visitor_conscripts", "The Conscripts", "military"));
            AddCard(new VisitorCard("visitor_spec_ops", "Special Ops Unit", "military"));
            AddCard(new VisitorCard("visitor_black_ops", "Black Ops Ambush", "rebels"));
            AddCard(new VisitorCard("visitor_chem_scientists", "Chem-Weapon Scientists", "military"));
            AddCard(new VisitorCard("visitor_mil_training_yard", "Military Training Yard", "military"));
            AddCard(new VisitorCard("visitor_rebel_snipers", "4x Rebel Sniper Squad", "rebels"));
            AddCard(new VisitorCard("visitor_rebel_militia", "Rebel Militias", "rebels"));
            AddCard(new VisitorCard("visitor_rebel_zealots", "Rebel Zealots", "rebels"));
            AddCard(new VisitorCard("visitor_rebel_moderates", "Rebel Moderates", "rebels"));
            AddCard(new VisitorCard("visitor_rebel_training_yard", "Rebel Training Yard", "rebels"));
            AddCard(new VisitorCard("visitor_collaborators", "The Collaborators Firing Squad", "rebels"));
            AddCard(new VisitorCard("visitor_terrorists", "Terrorist Cell", "terrorists"));
            AddCard(new VisitorCard("visitor_slavers", "Slaver Gang", "terrorists"));
            AddCard(new VisitorCard("visitor_cannibals", "Armed Cannibals", "cannibals"));
            AddCard(new VisitorCard("visitor_lone_psychopath", "The Lone Psychopath", "psychopaths"));
            AddCard(new VisitorCard("visitor_psychopath_pair", "Psychopath Pair Hunters", "psychopaths"));
            AddCard(new VisitorCard("visitor_bandits", "Bandit Highwaymen", "bandits"));
            AddCard(new VisitorCard("visitor_city_residents", "City Residents", "civilians"));
            AddCard(new VisitorCard("visitor_desperate_family", "Desperate Family", "civilians"));
            AddCard(new VisitorCard("visitor_the_parents", "Territorial Parents", "civilians"));
            AddCard(new VisitorCard("visitor_homeless", "Homeless Encampment", "civilians"));
            AddCard(new VisitorCard("visitor_drunks_aggro", "Aggressive Drunks", "civilians"));
            AddCard(new VisitorCard("visitor_addicts_passive", "Passive Addicts", "civilians"));
            AddCard(new VisitorCard("visitor_traveling_couple", "Traveling Couple", "civilians"));
            AddCard(new VisitorCard("visitor_the_old", "Defenseless Elders", "civilians"));
            AddCard(new VisitorCard("visitor_hospital_staff", "Hospital Triage Staff", "civilians"));
            AddCard(new VisitorCard("visitor_hospital_patients", "Bedridden Patients", "civilians"));
            AddCard(new VisitorCard("visitor_church_sanctuary", "Church Sanctuary", "civilians"));
            AddCard(new VisitorCard("visitor_church_hostile", "Church Hostile Takeover", "bandits"));
            AddCard(new VisitorCard("visitor_abandoned", "Abandoned Ruins", "none"));

            // Skirmish cards (Prompt #321 / #340-#344)
            AddCard(new VisitorCard("skirmish_mil_vs_terror", "Mil vs Terrorist Firefight", "military", true));
            AddCard(new VisitorCard("skirmish_mil_vs_rebel", "Mil vs Rebel Firefight", "military", true));
            AddCard(new VisitorCard("skirmish_bandit_vs_terror", "Bandit vs Terrorist Skirmish", "bandits", true));
            AddCard(new VisitorCard("skirmish_rebel_vs_bandit", "Rebel vs Bandit Clash", "rebels", true));
            AddCard(new VisitorCard("skirmish_rebel_vs_terror", "Rebel vs Terrorist Defense", "rebels", true));
        }

        public void AddCard(VisitorCard card)
        {
            if (card != null && !string.IsNullOrEmpty(card.cardId))
                _availableCards[card.cardId] = card;
        }

        public void InitializeMap(int seed)
        {
            _currentSeed = seed;
            var rng = new System.Random(seed);
            List<VisitorCard> pool = new List<VisitorCard>(_availableCards.Values);

            for (int i = 0; i < PermanentLocationIds.Length; i++)
            {
                string locId = PermanentLocationIds[i];
                VisitorCard drawn = pool[rng.Next(0, pool.Count)];
                var state = _nodes[locId];
                state.assignedVisitorCardId = drawn.cardId;
                state.assignedVisitorTitle = drawn.title;
                state.primaryFactionId = drawn.factionId;
                state.isSkirmishActive = drawn.isSkirmish;

                OnNodeVisitorAssigned?.Invoke(locId, drawn.cardId);
            }

            OnFixedNodesInitialized?.Invoke();
        }

        public FixedNodeState GetNodeState(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            _nodes.TryGetValue(locationId, out var state);
            return state;
        }

        public FixedMapSaveData GetSaveData()
        {
            var data = new FixedMapSaveData { seed = _currentSeed };
            foreach (var kvp in _nodes)
            {
                data.nodeStates.Add(kvp.Value);
            }
            return data;
        }

        public void RestoreSaveData(FixedMapSaveData data)
        {
            if (data == null) return;
            _currentSeed = data.seed;
            InitDefaultNodes();
            if (data.nodeStates != null)
            {
                foreach (var state in data.nodeStates)
                {
                    if (state != null && !string.IsNullOrEmpty(state.locationId))
                    {
                        _nodes[state.locationId] = state;
                    }
                }
            }
            OnFixedNodesInitialized?.Invoke();
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public VisitorRNGSystemSave CaptureState() => new VisitorRNGSystemSave
        {
            nodes = SaveMap.Capture(_nodes),
        };

        public void RestoreState(VisitorRNGSystemSave saved) =>
            SaveMap.Restore(_nodes, saved?.nodes, e => e.locationId);

}
}
