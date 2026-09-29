// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private JusticeSystem? _justice;

        // ── Plan 193: Wasteland Justice & Tribal Law ─────────────────────

        public JusticeSystem EnsureJustice()
        {
            if (_justice != null) return _justice;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("justice") : new SeededRng(193);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs;

            _justice = new JusticeSystem(rng, inv, needs, _politics, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("wasteland_laws.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<WastelandLawsCatalog>(json);
                        if (catalog?.laws != null)
                        {
                            foreach (var l in catalog.laws)
                                _justice.RegisterLaw(l);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Justice] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = JusticeSaveStore.TryLoad();
            if (saved != null)
            {
                _justice.RestoreState(saved);
            }

            _justice.OnTrialConcluded += (incidentId, verdict, punishment) =>
            {
                _journal?.TryAddRawEntry("trial_concluded", $"Tribal tribunal reached verdict for {incidentId}: {verdict} (Sentence: {punishment}).", null!, _simDay);
            };

            _justice.OnBanishment += (survivorId, incidentId) =>
            {
                _journal?.TryAddRawEntry("survivor_banished", $"{survivorId} was formally banished from the shelter following tribunal proceedings.", null!, _simDay);
            };

            _justice.OnExecution += (survivorId, incidentId) =>
            {
                _journal?.TryAddRawEntry("execution_carried_out", $"Capital punishment carried out on {survivorId}.", null!, _simDay);
            };

            _justice.OnVigilanteOutbreak += (incidentId, accusedId) =>
            {
                _journal?.TryAddRawEntry("vigilante_mob", $"Shelter unrest boiled over! Vigilante mob enacted street justice on {accusedId}.", null!, _simDay);
            };

            return _justice;
        }

        private void SetupJustice()
        {
            EnsureJustice();
        }

        private void SaveJustice()
        {
            if (_justice != null)
            {
                CaptureSection("wasteland_justice", JusticeSaveStore.TryCapturePersisted(_justice.CaptureState()));
            }
        }

        private void HandleJusticeAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseJusticeTribunalPanel(); return; }
            if (_justiceTribunalPanel == null || _justice == null) return;

            switch (action)
            {
                case "report":
                {
                    var parts = param.Split(':');
                    if (parts.Length != 2 || string.IsNullOrEmpty(parts[0])) break;
                    if (!System.Enum.TryParse<CrimeType>(parts[1], out var crime)) break;
                    string incidentId = $"inc_{parts[0]}_{parts[1]}_{_simDay}";
                    var inc = _justice.ReportCrime(incidentId, crime, parts[0], victimId: null, _simDay);
                    _justiceTribunalPanel.ShowFeedback(
                        inc != null ? $"Report filed. The case joins the docket for the tribunal's day."
                                    : "The report could not be filed.",
                        inc == null);
                    break;
                }
            }
            _justiceTribunalPanel.RefreshView();
        }
        private void CloseJusticeTribunalPanel() { _justiceTribunalPanel?.Visible = false; }

    }
}
