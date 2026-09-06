using System;
using Ashfall.Core;
using Ashfall.Core.Factions;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for the Core espionage authority.</summary>
    public sealed class EspionageHostSession : HostSessionBase
    {
        public EspionageSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public static EspionageHostSession Create(string dataDir, EspionageSystem? system = null)
        {
            var session = new EspionageHostSession(system ?? new EspionageSystem(new GodotLog()));
            if (!string.IsNullOrWhiteSpace(dataDir))
            {
                var missions = EspionageMissionCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                session.System.LoadMissionCatalog(missions);
                session.LastEvent = $"Espionage catalog loaded: {missions.Count} missions";
            }
            return session;
        }

        public EspionageHostSession(EspionageSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnIntelDiscovered += fact =>
            {
                LastEvent = $"Intel confirmed: {fact.factId} ({fact.confidence})";
                RaiseStateChanged();
            };
            System.OnAgentCaptured += agent =>
            {
                LastEvent = $"Agent captured: {agent.agentId}";
                RaiseStateChanged();
            };
            System.OnConsequenceIntent += intent =>
            {
                LastEvent = $"Espionage consequence: {intent.consequenceType}";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        public ActionResult Deploy(string agentId, string factionId, string missionId, int day)
        {
            var result = System.DeployAgent(agentId, factionId, missionId, day);
            LastEvent = result.IsSuccess ? "Agent deployed." : "Deployment blocked: " + result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        public ActionResult Exfiltrate(string networkId, int day)
        {
            var result = System.Exfiltrate(networkId, day);
            LastEvent = result.IsSuccess ? "Agent returned." : "Exfiltration blocked: " + result.FailureCode;
            RaiseStateChanged();
            return result;
        }

        public void AdvanceDay(int day)
        {
            System.Tick(day);
            LastEvent = "Espionage tick @ day " + day;
            RaiseStateChanged();
        }

        public EspionageState CaptureState() => System.CaptureState();
        public void RestoreState(EspionageState state) => System.RestoreState(state);
    }
}
