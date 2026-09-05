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

        /// <summary>
        /// Raised when a disease outcome resolves to death. Carries the
        /// survivor id and the disease id. The host's SurvivorFateSystem is
        /// the subscriber — the single disease death feed into the pipeline.
        /// </summary>
        public event Action<string, string>? OnSurvivorDied;

        private Func<IReadOnlyList<string>>? _populationProvider;

        /// <summary>Plan 60 / D4 — the campaign's current sim day, so a protocol is
        /// armed with the day it was actually applied and the ward can show how
        /// long each countermeasure still holds.</summary>
        private Func<int>? _dayProvider;

        public void BindDayProvider(Func<int>? dayProvider) => _dayProvider = dayProvider;

        private int Day => _dayProvider?.Invoke() ?? 0;

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
                if (!recovered)
                    OnSurvivorDied?.Invoke(s, d);
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
        }

        /// <summary>The host's live survivor roster becomes the exposure pool.</summary>
        public void BindPopulationProvider(Func<IReadOnlyList<string>> provider)
        {
            _populationProvider = provider;
        }

        /// <summary>
        /// Plan 60 / D3 — bind the single item authority so a treatment dose is spent
        /// like every other consumed thing in the game. The Core engine owns the
        /// clinical decision; it never holds an inventory. An unwired supply channel
        /// makes treatment refuse loudly rather than pretend, so a host that forgets to
        /// bind is caught by its own selftest instead of by a player.
        /// </summary>
        public void BindSupply(Func<string, int, bool>? consume)
        {
            Engine.TryConsumeItem = consume;
        }

        /// <summary>
        /// Treat one patient with one item. Returns a host-readable line; the reason
        /// codes come from Core so the wording and the rule cannot drift apart.
        /// </summary>
        public DiseaseTreatmentResult Treat(string survivorId, string diseaseId, string itemId, int day)
        {
            var result = Engine.TryTreat(survivorId, diseaseId, itemId, day);
            LastEvent = result.Accepted
                ? survivorId + " treated with " + itemId + " (" + result.Role + ")"
                  + (result.Cured ? " — cured" : string.Empty)
                : "Treatment refused (" + result.Reason + "): " + itemId + " for " + survivorId;
            StateChanged?.Invoke();
            return result;
        }

        /// <summary>
        /// Items authorised as treatment for a disease, for the ward UI. Read-only
        /// projection of the catalog — the panel must not keep its own drug list.
        /// </summary>
        public IReadOnlyList<DiseaseTreatment> AuthorizedTreatments(string diseaseId)
        {
            var def = Catalog.GetById(diseaseId);
            if (def == null || def.treatments == null) return System.Array.Empty<DiseaseTreatment>();
            return def.treatments;
        }

        /// <summary>
        /// Plan 60 / D2 — the clinical picture for one patient, assembled by Core
        /// (<see cref="DiseaseTriage.PictureOf"/>) so no surface invents its own
        /// reading of the same person. The bed is where a medic makes the diagnosis,
        /// so the named illness is shown here; surfaces that a layperson reads show
        /// signs without the identification instead.
        /// </summary>
        public DiseaseClinicalPicture? ClinicalPicture(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            var patients = Engine.GetSnapshot()?.patients;
            if (patients == null) return null;

            DiseaseClinicalPicture? worst = null;
            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                if (p == null || p.survivor_id != survivorId) continue;
                var def = Catalog.GetById(p.disease_id);
                if (def == null) continue;
                var picture = DiseaseTriage.PictureOf(
                    def, p.days_sick,
                    Engine.GetEffectiveLethality(survivorId, p.disease_id),
                    p.treatments_applied);
                if (worst == null || (int)picture.Stage > (int)worst.Stage) worst = picture;
            }
            return worst;
        }

        public DiseaseQuarantineCoordinator? Coordinator { get; private set; }

        public void BindCoordinator(DiseaseQuarantineCoordinator coordinator)
        {
            Coordinator = coordinator;
        }

        public void TickDaily(int day)
        {
            Coordinator?.TickDaily(day);
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
            Engine.PurifyWater(Day);
            return "Water stores purified — waterborne vectors blocked.";
        }

        public string SealVents()
        {
            Engine.SealVents(Day);
            return "Ventilators sealed — airborne vectors blocked.";
        }

        public string SterilizeTools()
        {
            Engine.SterilizeTools(Day);
            return "Surgical tools sterilised — bloodborne vectors blocked.";
        }

        public string ToggleAirFiltration(bool active)
        {
            Engine.SetAirFiltration(active, Day);
            return active ? "Air filtration engaged — spore vectors blocked."
                : "Air filtration offline — spore vectors active.";
        }

        /// <summary>Plan 60 / D4 — one protocol readout cell: "ON·2d" with the
        /// days the countermeasure still holds, "ON" when it holds until
        /// disengaged, "off" when the vector is live again.</summary>
        private string ProtocolCell(string vectorName, bool active)
        {
            int left = Engine.ProtocolDaysRemaining(vectorName, Day);
            if (!active || left < 0) return "off";
            if (left == int.MaxValue) return "ON";
            return $"ON·{left}d";
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
            sb.Append("\nProtocols: water ").Append(ProtocolCell(DiseaseVectorNames.Water, State.water_purified))
              .Append(" · vents ").Append(ProtocolCell(DiseaseVectorNames.Air, State.vents_sealed))
              .Append(" · tools ").Append(ProtocolCell(DiseaseVectorNames.Blood, State.tools_sterilized))
              .Append(" · air ").Append(ProtocolCell(DiseaseVectorNames.Spore, State.air_filtration));
            return sb.ToString();
        }

        private DiseaseSystemState State => Engine.State;
    }
}
