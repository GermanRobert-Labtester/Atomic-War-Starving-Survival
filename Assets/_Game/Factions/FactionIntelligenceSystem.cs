using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// Faction Intelligence System — covert operations, double agents,
    /// propaganda, extortion, hostage negotiation, and faction alliances
    /// for Expansion 4 (#91-100).
    ///
    /// Plain C#, save-safe. Extends existing FactionPressureWiring.
    /// </summary>
    [Serializable]
    public class IntelEntry
    {
        public string IntelId;
        public string FactionId;
        public string IntelType; // incoming_raid, convoy_schedule, internal_feud
        public string Description;
        public float HoursUntilExpiry;
        public bool IsActionable;
    }

    [Serializable]
    public class DoubleAgentState
    {
        public string SurvivorId;
        public string InfiltratedFactionId;
        public float DiscoveryRisk;
        public int DaysDeployed;
        public bool IsCompromised;
        public bool HasReturned;
    }

    [Serializable]
    public class FactionIntelligenceSaveState
    {
        public List<IntelEntry> ActiveIntel = new List<IntelEntry>();
        public List<DoubleAgentState> ActiveAgents = new List<DoubleAgentState>();
        public Dictionary<string, float> TributeDemands = new Dictionary<string, float>();
        public List<string> AlliedFactionIds = new List<string>();
        public bool InformantNetworkActive;
    }

    public class FactionIntelligenceSystem
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<IntelEntry> OnIntelReceived;
        public event Action<DoubleAgentState> OnAgentDeployed;
        public event Action<DoubleAgentState> OnAgentCompromised;
        public event Action<DoubleAgentState> OnAgentReturned;
        public event Action<string, float> OnTributeDemanded;
        // factionId, amount
        public event Action<string> OnPropagandaBroadcast;
        public event Action<string, string> OnAllianceFormed;
        // factionId, allianceType
        public event Action<string> OnAllianceBroken;

        private readonly FactionIntelligenceSaveState _state =
            new FactionIntelligenceSaveState();

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<string, float> GetFactionStanding;
        // factionId → -100..100
        public Func<string, int> GetAvailableResource;
        // resourceType → count
        public Action<string, int> ConsumeResource;
        // resourceType, amount
        public Func<int> GetDay;
        public System.Random Rng;

        // ── Intel Operations ───────────────────────────────────────────

        public bool InterceptRadioFrequency(string frequencyId,
            string factionId, string intelType)
        {
            string description = intelType switch
            {
                "incoming_raid" => "Raid detected — attack vector decoded. 24h advance warning.",
                "convoy_schedule" => "Supply convoy route intercepted. Opportunity for ambush.",
                "internal_feud" => "Internal faction conflict detected. Leadership unstable.",
                _ => "Unknown intelligence received."
            };

            var intel = new IntelEntry
            {
                IntelId = $"intel_{frequencyId}_{GetDay?.Invoke() ?? 1}",
                FactionId = factionId,
                IntelType = intelType,
                Description = description,
                HoursUntilExpiry = 24f,
                IsActionable = true
            };
            _state.ActiveIntel.Add(intel);
            OnIntelReceived?.Invoke(intel);
            return true;
        }

        public bool SendDoubleAgent(Survivor agent, string factionId)
        {
            if (agent == null || !agent.IsAlive) return false;

            var agentState = new DoubleAgentState
            {
                SurvivorId = agent.Id,
                InfiltratedFactionId = factionId,
                DiscoveryRisk = 0.1f,
                DaysDeployed = 0
            };
            _state.ActiveAgents.Add(agentState);
            agent.IsDoubleAgent = true;
            agent.InfiltratedFactionId = factionId;
            OnAgentDeployed?.Invoke(agentState);
            return true;
        }

        /// <summary>Demand tribute from a faction.</summary>
        public void DemandTribute(string factionId, string resourceType,
            int amount)
        {
            _state.TributeDemands[$"{factionId}_{resourceType}"] = amount;
            OnTributeDemanded?.Invoke(factionId, amount);
        }

        /// <summary>Broadcast counter-propaganda against a faction.</summary>
        public bool BroadcastPropaganda(string factionId)
        {
            float standing = GetFactionStanding?.Invoke(factionId) ?? 0f;
            if (standing > -50f) return false; // faction too stable

            OnPropagandaBroadcast?.Invoke(factionId);
            return true;
        }

        /// <summary>Negotiate hostage exchange.</summary>
        public bool NegotiateHostageExchange(string prisonerId,
            string demandType, int demandAmount)
        {
            int available = GetAvailableResource?.Invoke(demandType) ?? 0;
            if (available < demandAmount) return false;
            ConsumeResource?.Invoke(demandType, demandAmount);
            return true;
        }

        /// <summary>Plant sabotaged supplies along enemy patrol routes.</summary>
        public bool PlantSabotagedSupplies(string factionId,
            string supplyType)
        {
            // Weakens faction raiding power — effect applied by host
            return true;
        }

        /// <summary>Instigate defection among enemy troops.</summary>
        public bool InstigateDefection(string factionId, float moraleThreshold)
        {
            float standing = GetFactionStanding?.Invoke(factionId) ?? 0f;
            if (standing > moraleThreshold) return false;
            // Chance of gaining a defector survivor
            return (Rng?.NextDouble() ?? 0.5) < 0.3f;
        }

        /// <summary>Deploy fake signal decoy to redirect raids.</summary>
        public bool DeployFakeSignalDecoy()
        {
            _state.InformantNetworkActive = true;
            return true;
        }

        /// <summary>Establish informant network with alcohol/water bribes.</summary>
        public bool EstablishInformantNetwork(int alcoholCost, int waterCost)
        {
            int alcohol = GetAvailableResource?.Invoke("alcohol") ?? 0;
            int water = GetAvailableResource?.Invoke("clean_water") ?? 0;
            if (alcohol < alcoholCost || water < waterCost) return false;

            ConsumeResource?.Invoke("alcohol", alcoholCost);
            ConsumeResource?.Invoke("clean_water", waterCost);
            _state.InformantNetworkActive = true;
            return true;
        }

        /// <summary>Form a formal alliance with a faction.</summary>
        public bool FormAlliance(string factionId)
        {
            if (_state.AlliedFactionIds.Contains(factionId)) return false;
            float standing = GetFactionStanding?.Invoke(factionId) ?? 0f;
            if (standing < 60f) return false;

            _state.AlliedFactionIds.Add(factionId);
            OnAllianceFormed?.Invoke(factionId, "military_alliance");
            return true;
        }

        public bool IsAlliedWith(string factionId) =>
            _state.AlliedFactionIds.Contains(factionId);

        // ── Tick ───────────────────────────────────────────────────────

        public void Tick(float gameHours)
        {
            // Expire old intel
            for (int i = _state.ActiveIntel.Count - 1; i >= 0; i--)
            {
                _state.ActiveIntel[i].HoursUntilExpiry -= gameHours;
                if (_state.ActiveIntel[i].HoursUntilExpiry <= 0f)
                    _state.ActiveIntel.RemoveAt(i);
            }

            // Progress double agents
            for (int i = _state.ActiveAgents.Count - 1; i >= 0; i--)
            {
                var agent = _state.ActiveAgents[i];
                agent.DaysDeployed++;
                agent.DiscoveryRisk += 0.05f;

                if ((Rng?.NextDouble() ?? 0.5) < agent.DiscoveryRisk)
                {
                    agent.IsCompromised = true;
                    OnAgentCompromised?.Invoke(agent);
                    _state.ActiveAgents.RemoveAt(i);
                }
            }
        }

        public FactionIntelligenceSaveState GetState() => _state;
    }
}
