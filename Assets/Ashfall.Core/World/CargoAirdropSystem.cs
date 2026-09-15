// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    // ─────────────────────────────────────────────────────────────────
    // Plan 205 — cargo airdrop & emergency supply recovery.
    // Drops are scheduled from resolved radio contacts, descend through a
    // quantized altitude-band ladder, drift on the single weather
    // authority's wind vector, land at integer grid coordinates, and
    // become time-limited expedition recovery targets. Crate contents are
    // generated exactly once at scheduling and persist across save/load —
    // never rerolled. Recovery flows through the standard expedition
    // sortie lifecycle; capacity is enforced by the expedition authority.
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class AirdropProfileDef
    {
        public string drop_profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        /// <summary>Resolved-signal outcome types that can schedule this profile.</summary>
        public List<string> trigger_signal_outcomes { get; set; } = new List<string>();
        public string cargo_pool_id { get; set; } = string.Empty;
        /// <summary>Km of horizontal drift per kph of wind per descent band.</summary>
        public float wind_sensitivity { get; set; } = 1f;
        public float base_impact_kph { get; set; } = 22f;
        public float wind_impact_factor_kph { get; set; } = 0.6f;
        public float light_impact_kph { get; set; } = 24f;
        public float heavy_impact_kph { get; set; } = 40f;
        public int integrity_loss_light_pct { get; set; } = 15;
        public int integrity_loss_heavy_pct { get; set; } = 45;
        public int beacon_duration_days { get; set; } = 4;
        public int initial_interception_risk_bp { get; set; } = 250;
        public int interception_rate_per_day_bp { get; set; } = 900;
        public List<string> terrain_recovery_tags { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class AirdropCargoEntryDef
    {
        public string item_id { get; set; } = string.Empty;
        public int quantity { get; set; }
        public float weight_kg_per_unit { get; set; }
    }

    [Serializable]
    public sealed class AirdropCargoPoolDef
    {
        public string cargo_pool_id { get; set; } = string.Empty;
        public List<AirdropCargoEntryDef> entries { get; set; } = new List<AirdropCargoEntryDef>();
    }

    [Serializable]
    public sealed class CargoAirdropCatalog
    {
        public int schema_version { get; set; } = 1;
        public int descent_bands { get; set; } = 4;
        public int max_active_drops { get; set; } = 3;
        public AirdropInterceptionDef interception { get; set; } = new AirdropInterceptionDef();
        public List<AirdropProfileDef> drop_profiles { get; set; } = new List<AirdropProfileDef>();
        public List<AirdropCargoPoolDef> cargo_pools { get; set; } = new List<AirdropCargoPoolDef>();
    }

    [Serializable]
    public sealed class AirdropInterceptionDef
    {
        public int rate_per_day_bp { get; set; } = 900;
        public int expired_grace_days { get; set; } = 3;
    }

    [Serializable]
    public sealed class AirdropCrateEntryState
    {
        public string item_id { get; set; } = string.Empty;
        public int quantity { get; set; }
        public float weight_kg_per_unit { get; set; }
    }

    [Serializable]
    public sealed class AirdropEventState
    {
        public string event_id { get; set; } = string.Empty;
        public string drop_profile_id { get; set; } = string.Empty;
        public string source_signal_id { get; set; } = string.Empty;
        /// <summary>scheduled | descending | landed | recovered | intercepted | expired</summary>
        public string phase { get; set; } = "scheduled";
        public int release_day { get; set; }
        public int current_band { get; set; }
        // Target release point (quantized grid units).
        public int target_x { get; set; }
        public int target_y { get; set; }
        // Persisted wind snapshot at release — landing never recomputes after load.
        public float wind_direction_deg { get; set; }
        public float wind_speed_kph { get; set; }
        public int landing_x { get; set; }
        public int landing_y { get; set; }
        public int landing_day { get; set; }
        public int cargo_integrity_pct { get; set; } = 100;
        public bool beacon_active { get; set; }
        public int beacon_expires_day { get; set; }
        public int interception_progress_bp { get; set; }
        public List<AirdropCrateEntryState> crate_contents { get; set; } = new List<AirdropCrateEntryState>();
        public List<AirdropCrateEntryState> remaining_contents { get; set; } = new List<AirdropCrateEntryState>();
    }

    [Serializable]
    public sealed class CargoAirdropState
    {
        public int schema_version { get; set; } = 1;
        public List<AirdropEventState> drops { get; set; } = new List<AirdropEventState>();
        public int next_event_number { get; set; } = 1;
        public int total_recovered { get; set; }
        public int total_intercepted { get; set; }
        public int total_expired { get; set; }
    }

    /// <summary>Typed failure codes — the Godot host formats these.</summary>
    public static class AirdropFailures
    {
        public const string DropUnavailable = "airdrop.profile_invalid";
        public const string RadioContactMissing = "airdrop.signal_missing";
        public const string MaxActiveDrops = "airdrop.max_active";
        public const string EventNotFound = "airdrop.event_not_found";
        public const string WrongPhase = "airdrop.wrong_phase";
        public const string BeaconUnavailable = "airdrop.beacon_unavailable";
    }

    public sealed class CargoAirdropSystem
    {
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private CargoAirdropCatalog _catalog = new CargoAirdropCatalog();
        private CargoAirdropState _state = new CargoAirdropState();

        /// <summary>Host-injected campaign day provider (deterministic).</summary>
        public Func<int>? DayProvider { get; set; }

        public CargoAirdropState State => _state;
        public CargoAirdropCatalog Catalog => _catalog;

        public event Action<AirdropEventState>? OnDropScheduled;
        public event Action<AirdropEventState>? OnDropLanded;
        public event Action<AirdropEventState>? OnDropRecovered;
        public event Action<AirdropEventState>? OnDropIntercepted;
        public event Action<AirdropEventState>? OnDropExpired;
        public event Action<AirdropEventState, string, int>? OnCrateCollected;
        public event Action<string>? OnEventRaised;

        public CargoAirdropSystem(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(2050);
            _log = log ?? NullLog.Instance;
        }

        public void BindCatalog(CargoAirdropCatalog? catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        public AirdropProfileDef? FindProfile(string profileId)
        {
            foreach (var p in _catalog.drop_profiles)
                if (p.drop_profile_id == profileId) return p;
            return null;
        }

        private int CurrentDay() => DayProvider?.Invoke() ?? 0;

        // ── Scheduling ──────────────────────────────────────────────────

        /// <summary>
        /// Schedules a drop from a resolved radio contact. Crate contents are
        /// rolled from the pool exactly ONCE here and persist for the event's
        /// lifetime — never rerolled after save/load (roadmap Trap H).
        /// Landing coordinates are computed from the persisted wind snapshot at
        /// release and quantized to the integer grid (Trap I).
        /// </summary>
        public ActionResult ScheduleDrop(string profileId, string signalId, int targetX, int targetY)
        {
            var profile = FindProfile(profileId);
            if (profile == null)
                return ActionResult.Failed(AirdropFailures.DropUnavailable, "airdrop.unknown_profile");
            if (string.IsNullOrEmpty(signalId))
                return ActionResult.Failed(AirdropFailures.RadioContactMissing, "airdrop.missing_signal");
            int active = 0;
            foreach (var d in _state.drops)
            {
                if (d.phase == "scheduled" || d.phase == "descending" || d.phase == "landed") active++;
            }
            if (active >= _catalog.max_active_drops)
                return ActionResult.Blocked(AirdropFailures.MaxActiveDrops, "airdrop.too_many_active");

            var pool = FindPool(profile.cargo_pool_id);
            if (pool == null || pool.entries.Count == 0)
                return ActionResult.Failed(AirdropFailures.DropUnavailable, "airdrop.empty_pool");

            int day = CurrentDay();
            var ev = new AirdropEventState
            {
                event_id = $"drop_{_state.next_event_number}",
                drop_profile_id = profileId,
                source_signal_id = signalId,
                phase = "descending",
                release_day = day,
                current_band = Math.Max(1, _catalog.descent_bands),
                target_x = targetX,
                target_y = targetY,
                wind_direction_deg = WindDirectionDeg?.Invoke() ?? 0f,
                wind_speed_kph = WindSpeedKph?.Invoke() ?? 0f,
                landing_day = day + Math.Max(1, _catalog.descent_bands)
            };
            _state.next_event_number++;

            // Crate contents: generated once from the authored pool (deterministic —
            // no reroll on load; authored quantities are the contract).
            var contents = new List<AirdropCrateEntryState>();
            foreach (var entry in pool.entries)
            {
                if (entry.quantity <= 0) continue;
                contents.Add(new AirdropCrateEntryState
                {
                    item_id = entry.item_id,
                    quantity = entry.quantity,
                    weight_kg_per_unit = entry.weight_kg_per_unit
                });
            }
            ev.crate_contents = contents;
            ev.remaining_contents = new List<AirdropCrateEntryState>(contents);

            // Deterministic drift: quantized to integer grid units at schedule time.
            double driftKm = ev.wind_speed_kph * profile.wind_sensitivity * Math.Max(1, _catalog.descent_bands);
            double rad = ev.wind_direction_deg * Math.PI / 180.0;
            ev.landing_x = (int)Math.Round(ev.target_x + driftKm * Math.Cos(rad));
            ev.landing_y = (int)Math.Round(ev.target_y + driftKm * Math.Sin(rad));
            ev.cargo_integrity_pct = ComputeLandingIntegrity(profile, ev.wind_speed_kph);

            _state.drops.Add(ev);
            OnDropScheduled?.Invoke(ev);
            Raise("airdrop.scheduled");
            _log.Info($"[Airdrop] {ev.event_id} scheduled from signal {signalId}: lands at ({ev.landing_x},{ev.landing_y}) day {ev.landing_day}.");
            return ActionResult.Success("airdrop.scheduled");
        }

        /// <summary>Host-injected live wind projections from the weather authority.</summary>
        public Func<float>? WindDirectionDeg { get; set; }
        public Func<float>? WindSpeedKph { get; set; }

        private AirdropCargoPoolDef? FindPool(string poolId)
        {
            foreach (var p in _catalog.cargo_pools)
                if (p.cargo_pool_id == poolId) return p;
            return null;
        }

        private int ComputeLandingIntegrity(AirdropProfileDef profile, float windSpeedKph)
        {
            float impact = profile.base_impact_kph + windSpeedKph * profile.wind_impact_factor_kph;
            if (impact >= profile.heavy_impact_kph)
                return Math.Max(0, 100 - profile.integrity_loss_heavy_pct);
            if (impact >= profile.light_impact_kph)
                return Math.Max(0, 100 - profile.integrity_loss_light_pct);
            return 100;
        }

        // ── Daily tick ──────────────────────────────────────────────────

        public void TickDay(int day)
        {
            for (int i = 0; i < _state.drops.Count; i++)
            {
                var ev = _state.drops[i];

                if (ev.phase == "descending")
                {
                    ev.current_band--;
                    if (ev.current_band <= 0)
                        Land(ev, day);
                    continue;
                }

                if (ev.phase == "landed")
                {
                    // Beacon expiry.
                    if (ev.beacon_active && day > ev.beacon_expires_day)
                    {
                        ev.beacon_active = false;
                        Raise("airdrop.beacon_expired");
                    }

                    // Hostile interception race (deterministic daily accrual).
                    var profile = FindProfile(ev.drop_profile_id);
                    int rate = profile?.interception_rate_per_day_bp
                               ?? _catalog.interception.rate_per_day_bp;
                    ev.interception_progress_bp = Math.Min(10000, ev.interception_progress_bp + rate);
                    if (ev.interception_progress_bp >= 10000)
                    {
                        ev.phase = "intercepted";
                        _state.total_intercepted++;
                        OnDropIntercepted?.Invoke(ev);
                        Raise("airdrop.intercepted");
                        _log.Warn($"[Airdrop] {ev.event_id} intercepted by hostiles.");
                    }
                    continue;
                }

                // Expired: landed but beacon long dead and grace elapsed.
                if (ev.phase == "landed_expired")
                {
                    if (day > ev.beacon_expires_day + _catalog.interception.expired_grace_days)
                    {
                        ev.phase = "expired";
                        _state.total_expired++;
                        OnDropExpired?.Invoke(ev);
                        Raise("airdrop.expired");
                    }
                }
            }
        }

        private void Land(AirdropEventState ev, int day)
        {
            ev.phase = "landed";
            ev.landing_day = day;
            ev.beacon_active = true;
            ev.beacon_expires_day = day + BeaconDuration(ev);
            // Damage: integrity loss removes a deterministic share of each entry.
            int lossPct = 100 - ev.cargo_integrity_pct;
            if (lossPct > 0)
            {
                foreach (var entry in ev.remaining_contents)
                {
                    int destroyed = entry.quantity * lossPct / 100;
                    entry.quantity -= destroyed;
                }
                ev.remaining_contents.RemoveAll(e => e.quantity <= 0);
            }
            OnDropLanded?.Invoke(ev);
            Raise("airdrop.landed");
            _log.Info($"[Airdrop] {ev.event_id} landed at ({ev.landing_x},{ev.landing_y}) — beacon active {ev.beacon_expires_day - day}d, integrity {ev.cargo_integrity_pct}%.");
        }

        private int BeaconDuration(AirdropEventState ev)
        {
            var profile = FindProfile(ev.drop_profile_id);
            return profile?.beacon_duration_days ?? 4;
        }

        /// <summary>
        /// Expedited recovery marker: the beacon can be reactivated by a radio
        /// check (free action while the crate is intact and on schedule).
        /// </summary>
        public ActionResult ReactivateBeacon(string eventId)
        {
            var ev = FindEvent(eventId);
            if (ev == null)
                return ActionResult.Failed(AirdropFailures.EventNotFound, "airdrop.event_not_found");
            if (ev.phase != "landed")
                return ActionResult.Blocked(AirdropFailures.WrongPhase, "airdrop.not_landed");
            if (ev.beacon_active)
                return ActionResult.Blocked(AirdropFailures.BeaconUnavailable, "airdrop.beacon_active");

            ev.beacon_active = true;
            Raise("airdrop.beacon_reactivated");
            return ActionResult.Success("airdrop.beacon_reactivated");
        }

        // ── Recovery (through the expedition authority) ─────────────────

        public AirdropEventState? FindEvent(string eventId)
        {
            foreach (var d in _state.drops)
                if (d.event_id == eventId) return d;
            return null;
        }

        /// <summary>
        /// Transfers crate contents into the recovering sortie through the
        /// host-provided grant delegate (the expedition authority's
        /// TryGrantLoot — capacity enforced there, per item). Partial recovery:
        /// whatever does not fit stays in the crate (retryable, never lost,
        /// never duplicated). An emptied crate marks the target recovered.
        /// </summary>
        public int CollectCrate(string eventId, Func<string, float, int, bool> tryGrantItem)
        {
            var ev = FindEvent(eventId);
            if (ev == null || ev.phase != "landed") return 0;

            int totalCollected = 0;
            for (int i = ev.remaining_contents.Count - 1; i >= 0; i--)
            {
                var entry = ev.remaining_contents[i];
                if (tryGrantItem(entry.item_id, entry.weight_kg_per_unit, entry.quantity))
                {
                    totalCollected += entry.quantity;
                    OnCrateCollected?.Invoke(ev, entry.item_id, entry.quantity);
                    ev.remaining_contents.RemoveAt(i);
                }
            }

            if (ev.remaining_contents.Count == 0)
            {
                ev.phase = "recovered";
                _state.total_recovered++;
                OnDropRecovered?.Invoke(ev);
                Raise("airdrop.recovered");
                _log.Info($"[Airdrop] {ev.event_id} fully recovered ({totalCollected} units).");
            }
            return totalCollected;
        }

        /// <summary>Grid coordinate of an active drop target for map projection.</summary>
        public AirdropEventState? FindLandedAt(string locationId)
        {
            foreach (var d in _state.drops)
                if (d.phase == "landed" && $"airdrop_{d.event_id}" == locationId) return d;
            return null;
        }

        // ── Persistence ─────────────────────────────────────────────────

        public CargoAirdropState CaptureState()
        {
            var clone = new CargoAirdropState
            {
                schema_version = _state.schema_version,
                drops = new List<AirdropEventState>(_state.drops),
                next_event_number = _state.next_event_number,
                total_recovered = _state.total_recovered,
                total_intercepted = _state.total_intercepted,
                total_expired = _state.total_expired
            };
            return clone;
        }

        public void RestoreState(CargoAirdropState? state)
        {
            if (state == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(state);
            _state = s.Deserialize<CargoAirdropState>(json) ?? new CargoAirdropState();
            if (_state.drops == null) _state.drops = new List<AirdropEventState>();
            if (_state.schema_version < 1 || _state.schema_version > 1)
                _state.schema_version = 1;
        }

        private void Raise(string eventId) => OnEventRaised?.Invoke(eventId);
    }
}
