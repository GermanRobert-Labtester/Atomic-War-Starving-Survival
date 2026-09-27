// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Modal Travel Dispatch — derived read model over the sealed
// Ashfall.Core.World.ModalTravelDispatchEngine (L-P32R / UNBLOCK-04 §2.14/§5.12).
//
// The wasteland map owner owns the route graph; the vehicle/garage owner owns
// condition; the inventory owner owns fuel; the weather owner owns the flight
// window. This session is a PURE PROJECTION — it reads those owners and returns
// an immutable verdict. It mutates nothing and persists nothing.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class ModalTravelDispatchHostSession : HostSessionBase
    {
        private readonly Func<WastelandMapSystem?> _mapProvider;
        private readonly Func<int> _vehicleConditionPermille;
        private readonly Func<int> _fuelAvailableUnits;
        private readonly Func<int> _weatherWindowPermille;

        public string LastEvent { get; private set; } = string.Empty;

        public ModalTravelDispatchHostSession(
            Func<WastelandMapSystem?> mapProvider,
            Func<int> vehicleConditionPermille,
            Func<int> fuelAvailableUnits,
            Func<int> weatherWindowPermille)
        {
            _mapProvider = mapProvider ?? throw new ArgumentNullException(nameof(mapProvider));
            _vehicleConditionPermille = vehicleConditionPermille ?? throw new ArgumentNullException(nameof(vehicleConditionPermille));
            _fuelAvailableUnits = fuelAvailableUnits ?? throw new ArgumentNullException(nameof(fuelAvailableUnits));
            _weatherWindowPermille = weatherWindowPermille ?? throw new ArgumentNullException(nameof(weatherWindowPermille));
        }

        public MapRoute? FindRoute(string fromNodeId, string toNodeId)
        {
            var map = _mapProvider();
            if (map == null) return null;
            foreach (var r in map.Routes)
            {
                if (r == null) continue;
                if (string.Equals(r.From, fromNodeId, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(r.To, toNodeId, StringComparison.OrdinalIgnoreCase))
                    return r;
            }
            return null;
        }

        /// <summary>Every route the live map owner currently knows about.</summary>
        public IReadOnlyList<MapRoute> KnownRoutes => _mapProvider()?.Routes ?? new List<MapRoute>();

        /// <summary>
        /// Evaluates one modality against one route. Pure: identical inputs always
        /// produce identical integers, and the route is never written back.
        /// </summary>
        public ModalTravelDispatchResult Evaluate(MapRoute? route, TravelModality modality)
            => ModalTravelDispatchEngine.EvaluateDispatch(
                route,
                modality,
                _vehicleConditionPermille(),
                _fuelAvailableUnits(),
                _weatherWindowPermille());

        public ModalTravelDispatchResult EvaluateRoute(string fromNodeId, string toNodeId, TravelModality modality)
        {
            var route = FindRoute(fromNodeId, toNodeId);
            var result = Evaluate(route, modality);
            LastEvent = result.Summary;
            RaiseStateChanged();
            return result;
        }

        /// <summary>
        /// The full modality matrix for one route — what the player needs to decide
        /// before committing an expedition.
        /// </summary>
        public IReadOnlyList<(TravelModality Modality, ModalTravelDispatchResult Result)> EvaluateAllModalities(
            MapRoute? route)
        {
            var rows = new List<(TravelModality, ModalTravelDispatchResult)>();
            foreach (TravelModality modality in Enum.GetValues(typeof(TravelModality)))
                rows.Add((modality, Evaluate(route, modality)));
            return rows;
        }

        /// <summary>Truthful one-line projection for a status readout.</summary>
        public string StatusLine(string fromNodeId, string toNodeId)
        {
            var route = FindRoute(fromNodeId, toNodeId);
            if (route == null) return $"no route {fromNodeId}->{toNodeId}";
            int viable = 0;
            foreach (var (_, result) in EvaluateAllModalities(route))
                if (result.CanDispatch) viable++;
            return $"{fromNodeId}->{toNodeId} · {route.DistanceKm:0.#} km · {viable}/{Enum.GetValues(typeof(TravelModality)).Length} modality(ies) crossable";
        }
    }
}
