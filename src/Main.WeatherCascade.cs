// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 135 — Weather → Deep Gameplay Cascade, in the running game.
//
// Weather stops being a number the UI prints and becomes a driver: on every
// canonical weather change the cascade evaluates the authored template table
// and routes each effect to the canonical owner that already owns that fact.
// Nothing here owns a weather, shelter, expedition, market, morale or
// accessibility value — only the cascade's own event ledger is persisted.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Records;
using Ashfall.Core.Weather;
using Ashfall.Core.World;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private WeatherCascadeHostSession? _weatherCascade;
        private bool _weatherCascadeDirty;
        private WeatherKind _lastCascadedKind = (WeatherKind)(-1);
        private int _lastCascadedDay = -1;

        public WeatherCascadeHostSession? WeatherCascade => _weatherCascade;

        /// <summary>
        /// Build the cascade session and bind the authored template table. A
        /// missing or invalid table leaves the cascade inactive (logged), so a
        /// campaign never runs a fabricated cascade — the engine's own
        /// generated-effect fallback is deliberately unreachable from the host.
        /// </summary>
        private WeatherCascadeHostSession? EnsureWeatherCascade()
        {
            if (_weatherCascade != null) return _weatherCascade;
            var session = new WeatherCascadeHostSession();
            session.BindEffectsCatalog(_world?.WeatherEffects);
            if (!session.LoadAuthoredTemplates(_dataDir, new FileSystemIO()))
            {
                GD.PrintErr("[WeatherCascade] authored cascade table unavailable: "
                    + string.Join("; ", session.LoadErrors) + "; weather will not cascade.");
            }
            BindWeatherCascadeOwners(session);
            _weatherCascade = session;
            return _weatherCascade;
        }

        private void BindWeatherCascadeOwners(WeatherCascadeHostSession session)
        {
            session.ShelterResilience = _disasterResponse?.System;
            session.Expeditions = _expeditions?.Engine;
            session.Market = _economy?.Market;
            session.Needs = _survivors?.Needs;
        }

        private void SetupWeatherCascade()
        {
            var session = EnsureWeatherCascade();
            if (session == null) return;
            BindWeatherCascadeOwners(session);
            if (_saveLoadHost != null
                && _saveLoadHost.TryGetSectionPayload(WeatherCascadeSaveStore.SectionName, out string cascadePayload))
            {
                RestoreWeatherCascade(cascadePayload);
            }            if (session.UsingAuthoredTemplates)
            {
                GD.Print($"[WeatherCascade] {session.System.Engine.Templates.Count} authored cascade "
                    + "template(s) bound; severity derived from the canonical weather-effects table.");
            }
            _lastCascadedKind = (WeatherKind)(-1);
            _lastCascadedDay = -1;
        }

        private void SaveWeatherCascade()
        {
            var session = _weatherCascade;
            if (session == null) return;
            BindWeatherCascadeOwners(session);
            CaptureSection(
                WeatherCascadeSaveStore.SectionName,
                WeatherCascadeSaveStore.TryCapturePersisted(session.System.CaptureState()));
            _weatherCascadeDirty = false;
        }

        private void FlushWeatherCascadeIfDirty()
        {
            if (_weatherCascadeDirty) SaveWeatherCascade();
        }

        private void RestoreWeatherCascade(string? json)
        {
            var session = EnsureWeatherCascade();
            if (session == null || string.IsNullOrWhiteSpace(json)) return;
            try
            {
                session.System.RestoreState(WeatherCascadeSaveStore.TryRestore(json));
            }
            catch (InvalidOperationException ex)
            {
                // A schema the engine cannot interpret keeps live state rather
                // than half-applying a foreign shape.
                GD.PrintErr("[WeatherCascade] restore rejected: " + ex.Message);
            }
        }

        private void ResetWeatherCascade()
        {
            _weatherCascade?.Dispose();
            _weatherCascade = null;
            _weatherCascadeDirty = false;
            _lastCascadedKind = (WeatherKind)(-1);
            _lastCascadedDay = -1;
        }

        /// <summary>
        /// Canonical weather-change entry point. Called by the weather owner's
        /// change notification (and by the CLI probe); evaluates one cascade per
        /// (kind, day) so a chattering forecast cannot farm effects.
        /// </summary>
        internal void OnWeatherFrontArrived(WeatherKind kind, int day)
        {
            var session = EnsureWeatherCascade();
            if (session == null || !session.UsingAuthoredTemplates) return;
            if (_lastCascadedDay == day && _lastCascadedKind == kind) return;
            _lastCascadedDay = day;
            _lastCascadedKind = kind;

            var outcome = session.TriggerFront(kind, day);
            if (outcome.EffectCount <= 0) return;

            _weatherCascadeDirty = true;
            var parts = new List<string>();
            foreach (var contribution in outcome.Contributions)
                parts.Add($"{contribution.TargetSystem}:{contribution.OwnerName}");
            _journal?.TryAddRawEntry(
                "weather_cascade",
                $"{kind} front (severity {outcome.Severity:0.#}) pressured "
                + $"{outcome.EffectCount} system(s): {string.Join(", ", parts)}.",
                null, Math.Max(1, day));
        }

        /// <summary>
        /// Canonical day tick: expire fronts whose duration ended and withdraw
        /// their contribution from every owner that accepted one.
        /// </summary>
        internal void TickWeatherCascade(int day)
        {
            var session = _weatherCascade;
            if (session == null || !session.UsingAuthoredTemplates) return;
            BindWeatherCascadeOwners(session);

            var expired = session.TickDay(day);
            if (expired.Count > 0)
            {
                _weatherCascadeDirty = true;
                _journal?.TryAddRawEntry(
                    "weather_cascade",
                    $"{expired.Count} weather front(s) expired; shelter, economy and morale "
                    + "contributions withdrawn from their canonical owners.",
                    null, Math.Max(1, day));
            }
        }
    }
}
