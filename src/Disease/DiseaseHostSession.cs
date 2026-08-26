using System;
using System.Collections.Generic;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Disease;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the DISEASE EXPANSION.
    /// Wraps the Core DiseaseSystem (owned by the expansion hub so it rides the
    /// hub save), binds the live survivor roster as the exposure pool, and
    /// exposes thin commands + a read model for the outbreak ward UI.
    /// No gameplay rules here — hosts only present.
    /// </summary>
    public sealed class DiseaseHostSession
    {
        public DiseaseSystem Engine { get; }
        public DiseaseCatalog Catalog { get; }

        public string LastEvent { get; set; } = string.Empty;
        public event Action? StateChanged;

        private Func<IReadOnlyList<string>>? _populationProvider;

        public DiseaseHostSession(DiseaseSystem engine, DiseaseCatalog catalog)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Catalog = catalog ?? new DiseaseCatalog();
            Engine.OnInfection += (s, d) => { LastEvent = s + " infected with " + d; StateChanged?.Invoke(); };
            Engine.OnQuarantineStarted += (s, d) => { LastEvent = s + " isolated in the ward (" + d + ")"; StateChanged?.Invoke(); };
            Engine.OnQuarantineEnded += (s, d) => { LastEvent = s + " released from the ward (" + d + ")"; StateChanged?.Invoke(); };
            Engine.OnOutbreakDeclared += d => { LastEvent = "OUTBREAK DECLARED: " + d + " — isolate the ward now"; StateChanged?.Invoke(); };
            Engine.OnOutbreakContained += (d, prevented) =>
            {
                LastEvent = "Outbreak contained: " + d + (prevented ? " (no lives lost)" : " (lives lost)");
                StateChanged?.Invoke();
            };
            Engine.OnOutcomeResolved += (s, d, recovered) =>
            {
                LastEvent = (recovered ? s + " recovered from " : s + " died of ") + d;
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
        }

        /// <summary>The host's live survivor roster becomes the exposure pool.</summary>
        public void BindPopulationProvider(Func<IReadOnlyList<string>> provider)
        {
            _populationProvider = provider;
        }

        public void TickDaily(int day)
        {
            Engine.TickDaily(day, _populationProvider?.Invoke()!);
        }

        public DiseaseSnapshot Snapshot => Engine.GetSnapshot();

        // ---- Thin commands for the UI ----

        public string Infect(string survivorId, string diseaseId, int day)
        {
            if (!string.IsNullOrEmpty(survivorId) && string.IsNullOrEmpty(Engine.GetTransmissionVector(diseaseId)))
                return "Unknown disease: " + diseaseId;
            Engine.Infect(survivorId, diseaseId, day);
            return survivorId + " isolated case registered (" + diseaseId + ").";
        }

        public string Quarantine(string survivorId, string diseaseId)
        {
            if (!Engine.IsInfected(survivorId, diseaseId))
                return survivorId + " is not a registered case of " + diseaseId + ".";
            Engine.Quarantine(survivorId, diseaseId);
            return survivorId + " moved to the quarantine ward.";
        }

        public string Release(string survivorId, string diseaseId)
        {
            if (!Engine.IsQuarantined(survivorId, diseaseId))
                return survivorId + " is not quarantined for " + diseaseId + ".";
            Engine.EndQuarantine(survivorId, diseaseId);
            return survivorId + " released from the quarantine ward.";
        }

        public string PurifyWater()
        {
            Engine.PurifyWater();
            return "Water stores purified — waterborne vectors blocked.";
        }

        public string SealVents()
        {
            Engine.SealVents();
            return "Ventilators sealed — airborne vectors blocked.";
        }

        public string SterilizeTools()
        {
            Engine.SterilizeTools();
            return "Surgical tools sterilised — bloodborne vectors blocked.";
        }

        public string ToggleAirFiltration(bool active)
        {
            Engine.SetAirFiltration(active);
            return active ? "Air filtration engaged — spore vectors blocked."
                : "Air filtration offline — spore vectors active.";
        }

        public string StatusLine()
        {
            var s = Engine.GetSnapshot();
            var sb = new StringBuilder("DISEASE WARD: infections " + s.total_infected
                + " · quarantined " + s.total_quarantined
                + " · outbreaks " + s.total_outbreaks
                + " (prevented " + s.total_outbreaks_prevented + ")"
                + " · recovered " + s.total_recovered
                + " · deaths " + s.total_deaths);
            if (s.total_contagious > 0)
                sb.Append("  ★ " + s.total_contagious + " CONTAGIOUS AND UNISOLATED");
            if (!string.IsNullOrEmpty(LastEvent))
                sb.Append("  | " + LastEvent);
            return sb.ToString();
        }

        public string WardReport()
        {
            var s = Engine.GetSnapshot();
            var sb = new StringBuilder("EPIDEMIC WARD — QUARANTINE PROTOCOL\n");
            sb.Append("Active infections: ").Append(s.total_infected)
              .Append(" · Isolated: ").Append(s.total_quarantined)
              .Append(" · Contagious free: ").Append(s.total_contagious)
              .Append(" · Outbreaks: ").Append(s.total_outbreaks)
              .Append(" (prevented ").Append(s.total_outbreaks_prevented).Append(")");

            if (s.patients.Count == 0)
            {
                sb.Append("\n  No active cases in the ward.");
            }
            else
            {
                for (int i = 0; i < s.patients.Count; i++)
                {
                    var p = s.patients[i];
                    if (p == null) continue;
                    sb.Append("\n  ").Append(p.survivor_id)
                      .Append(" — ").Append(p.disease_name ?? p.disease_id)
                      .Append(" (day ").Append(p.days_sick).Append(")")
                      .Append(" · contagion risk ").Append(p.contagion_risk_percent).Append("%")
                      .Append(p.quarantined ? "  ✔ ISOLATED" : (p.contagious ? "  ★ HIGH RISK" : "  (incubating)"));
                }
            }
            sb.Append("\nProtocols: water ").Append(State.water_purified ? "ON" : "off")
              .Append(" · vents ").Append(State.vents_sealed ? "ON" : "off")
              .Append(" · tools ").Append(State.tools_sterilized ? "ON" : "off")
              .Append(" · air ").Append(State.air_filtration ? "ON" : "off");
            return sb.ToString();
        }

        private DiseaseSystemState State => Engine.State;
    }
}