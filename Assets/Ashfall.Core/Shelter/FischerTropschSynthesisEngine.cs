// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class FischerTropschBatchState
    {
        public string batch_id = string.Empty;
        public string reactor_profile_id = string.Empty;
        public int progress_ticks;
        public float operator_skill = 0.5f;
        public float feed_quality = 1f;
        public float process_variation = 1f;
    }

    [Serializable]
    public sealed class SynthesisOutputBatch
    {
        public string batch_id = string.Empty;
        public string reactor_profile_id = string.Empty;
        public int lubricant_units;
        public int wax_units;
        public int light_fraction_units;
        public string lubricant_grade = "standard";
        public int completed_tick;
    }

    [Serializable]
    public sealed class FischerTropschSynthesisState
    {
        public int schema_version = 1;
        public ulong rng_state;
        public float catalyst_condition = 100f;
        public int ticks_since_maintenance;
        public int next_batch_number = 1;
        public int total_batches_completed;
        public FischerTropschBatchState? active_batch;
        public List<SynthesisOutputBatch> output_buffer = new List<SynthesisOutputBatch>();
        public List<string> serviced_consumers = new List<string>();
    }

    [Serializable]
    public sealed class MechanicalLubricantConsumer
    {
        public string consumer_id = string.Empty;
        public List<string> accepted_grades = new List<string>();
        public float wear_multiplier = 1f;
        public int service_units = 1;
    }

    public sealed class SynthesisTickResult
    {
        public bool Progressed { get; set; }
        public bool Paused { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public SynthesisOutputBatch? CompletedBatch { get; set; }
    }

    public static class FischerTropschFailureCodes
    {
        public const string ReactorUnknown = "ft.reactor_unknown";
        public const string FeedstockMissing = "ft.feedstock_missing";
        public const string CatalystSpent = "ft.catalyst_spent";
        public const string CatalystUnavailable = "ft.catalyst_unavailable";
        public const string ReactorFaulted = "ft.reactor_faulted";
        public const string CoolingUnavailable = "ft.cooling_unavailable";
        public const string BatchActive = "ft.batch_active";
        public const string OutputUnavailable = "ft.output_unavailable";
        public const string ConsumerUnknown = "ft.consumer_unknown";
        public const string LubricantMissing = "ft.lubricant_missing";
    }

    /// <summary>
    /// Abstract, catalog-driven synthesis plant. It models feed conversion,
    /// catalyst condition, operating bands, product routing, and registered
    /// lubricant consumers without encoding real process instructions.
    /// </summary>
    public sealed class FischerTropschSynthesisEngine
    {
        private readonly ISeededRng _rng;
        private FischerTropschCatalog _catalog;
        private FischerTropschSynthesisState _state = new FischerTropschSynthesisState();
        private IPlayerInventoryPort? _inventory;
        private Func<string, ItemDefinition?>? _itemLookup;
        private readonly Dictionary<string, MechanicalLubricantConsumer> _consumers =
            new Dictionary<string, MechanicalLubricantConsumer>(StringComparer.Ordinal);
        private int _tick;

        public FischerTropschSynthesisEngine(ISeededRng? rng = null, FischerTropschCatalog? catalog = null)
        {
            _rng = rng ?? new SeededRng(118);
            _catalog = catalog ?? new FischerTropschCatalog(new FischerTropschCatalogDto
            {
                reactor_profiles = new List<FischerTropschReactorProfile>
                {
                    new FischerTropschReactorProfile
                    {
                        reactor_profile_id = "ft_reactor_mk1",
                        feedstock_item_id = "synthetic_fuel_canister",
                        feedstock_units = 2,
                        base_conversion_units = 3f,
                        catalyst_profile_id = "ft_catalyst_standard",
                        catalyst_item_id = "item_bearing_grease"
                    }
                },
                products = new List<FischerTropschProductProfile>
                {
                    new FischerTropschProductProfile { product_id = "ft_lubricant_standard", item_id = "machine_oil", output_kind = "lubricant", grade = "synthetic", split = 0.5f },
                    new FischerTropschProductProfile { product_id = "ft_wax_standard", item_id = "carbon_black_powder", output_kind = "wax", grade = "standard", split = 0.25f },
                    new FischerTropschProductProfile { product_id = "ft_light_fraction", item_id = "fuel_1l", output_kind = "light_fraction", grade = "standard", split = 0.25f }
                },
                catalyst_profiles = new List<FischerTropschCatalystProfile>
                {
                    new FischerTropschCatalystProfile { catalyst_profile_id = "ft_catalyst_standard" }
                }
            });
        }

        public FischerTropschCatalog Catalog => _catalog;
        public FischerTropschSynthesisState State => _state;
        public bool HasActiveBatch => _state.active_batch != null;
        public float CatalystCondition => Math.Clamp(_state.catalyst_condition, 0f, 100f);
        public IReadOnlyDictionary<string, MechanicalLubricantConsumer> Consumers => _consumers;

        public void BindCatalog(FischerTropschCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public void BindInventory(IPlayerInventoryPort inventory, Func<string, ItemDefinition?>? itemLookup = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _itemLookup = itemLookup;
        }

        public void RegisterLubricantConsumer(MechanicalLubricantConsumer consumer)
        {
            if (consumer == null || string.IsNullOrWhiteSpace(consumer.consumer_id))
                throw new ArgumentException("A lubricant consumer needs an id", nameof(consumer));
            if (consumer.wear_multiplier <= 0f || consumer.wear_multiplier > 1f)
                throw new ArgumentOutOfRangeException(nameof(consumer), "wear multiplier must be in (0,1]");
            _consumers[consumer.consumer_id] = consumer;
        }

        public ActionResult StartBatch(string reactorProfileId, float operatorSkill = 0.5f, float feedQuality = 1f)
        {
            var profile = _catalog.FindReactor(reactorProfileId);
            if (profile == null)
                return ActionResult.Blocked(FischerTropschFailureCodes.ReactorUnknown, "ft.reactor_unknown");
            if (_state.active_batch != null)
                return ActionResult.Blocked(FischerTropschFailureCodes.BatchActive, "ft.batch_active");
            if (_state.catalyst_condition <= (_catalog.FindCatalyst(profile.catalyst_profile_id)?.spent_threshold ?? 10f))
                return ActionResult.Blocked(FischerTropschFailureCodes.CatalystSpent, "ft.catalyst_spent");
            if (_inventory == null)
                return ActionResult.Blocked(FischerTropschFailureCodes.FeedstockMissing, "ft.inventory_unbound");
            if (!_inventory.HasSufficient(profile.feedstock_item_id, profile.feedstock_units))
                return ActionResult.Blocked(FischerTropschFailureCodes.FeedstockMissing, "ft.feedstock_missing");

            if (!_inventory.TryConsume(profile.feedstock_item_id, profile.feedstock_units))
                return ActionResult.Blocked(FischerTropschFailureCodes.FeedstockMissing, "ft.feedstock_missing");

            _state.active_batch = new FischerTropschBatchState
            {
                batch_id = $"ft_batch_{_state.next_batch_number++}",
                reactor_profile_id = reactorProfileId,
                operator_skill = Math.Clamp(operatorSkill, 0f, 1f),
                feed_quality = Math.Clamp(feedQuality, 0f, 1f),
                process_variation = 0.95f + (float)_rng.NextDouble() * 0.10f
            };
            return ActionResult.Success("ft.batch_started");
        }

        public ActionResult ReplaceCatalyst(string reactorProfileId)
        {
            var reactor = _catalog.FindReactor(reactorProfileId);
            if (reactor == null) return ActionResult.Blocked(FischerTropschFailureCodes.ReactorUnknown, "ft.reactor_unknown");
            if (_inventory == null || !_inventory.HasSufficient(reactor.catalyst_item_id, reactor.catalyst_units))
                return ActionResult.Blocked(FischerTropschFailureCodes.CatalystUnavailable, "ft.catalyst_unavailable");
            if (!_inventory.TryConsume(reactor.catalyst_item_id, reactor.catalyst_units))
                return ActionResult.Blocked(FischerTropschFailureCodes.CatalystUnavailable, "ft.catalyst_unavailable");

            var catalyst = _catalog.FindCatalyst(reactor.catalyst_profile_id);
            _state.catalyst_condition = catalyst?.replacement_condition ?? 100f;
            _state.ticks_since_maintenance = 0;
            return ActionResult.Success("ft.catalyst_replaced");
        }

        public SynthesisTickResult Tick(
            float thermalState,
            float pressureState,
            float coolingState,
            float operatorSkill = 0.5f)
        {
            _tick++;
            _state.ticks_since_maintenance++;
            var batch = _state.active_batch;
            if (batch == null) return new SynthesisTickResult { StatusCode = "ft.idle" };
            var profile = _catalog.FindReactor(batch.reactor_profile_id);
            if (profile == null)
            {
                _state.active_batch = null;
                return new SynthesisTickResult { Paused = true, StatusCode = FischerTropschFailureCodes.ReactorUnknown };
            }
            var catalyst = _catalog.FindCatalyst(profile.catalyst_profile_id);
            if (_state.catalyst_condition <= (catalyst?.spent_threshold ?? 10f))
                return new SynthesisTickResult { Paused = true, StatusCode = FischerTropschFailureCodes.CatalystSpent };
            if (coolingState < 0.2f)
                return new SynthesisTickResult { Paused = true, StatusCode = FischerTropschFailureCodes.CoolingUnavailable };

            float thermalBand = BandModifier(thermalState, profile.thermal_min, profile.thermal_max, profile.off_band_yield_floor);
            float pressureBand = BandModifier(pressureState, profile.pressure_min, profile.pressure_max, profile.off_band_yield_floor);
            float cooling = Math.Clamp(coolingState, 0f, 1f);
            _state.catalyst_condition = Math.Max(0f, _state.catalyst_condition - profile.catalyst_decay_per_tick * (1.2f - cooling * 0.2f));
            batch.progress_ticks++;
            if (batch.progress_ticks < profile.process_ticks)
                return new SynthesisTickResult { Progressed = true, StatusCode = "ft.progress" };

            float catalystModifier = Math.Clamp(_state.catalyst_condition / 100f, 0.25f, 1f);
            float quality = Math.Clamp(batch.process_variation * batch.feed_quality
                * thermalBand * pressureBand * cooling * catalystModifier
                * (0.9f + Math.Clamp(operatorSkill, 0f, 1f) * 0.1f), 0f, 1f);
            float conversion = Math.Min(profile.feedstock_units, profile.base_conversion_units * quality);
            var output = BuildOutput(profile, batch, conversion);
            _state.output_buffer.Add(output);
            _state.active_batch = null;
            _state.total_batches_completed++;
            return new SynthesisTickResult { Progressed = true, StatusCode = "ft.complete", CompletedBatch = output };
        }

        public ActionResult ClaimOutputs()
        {
            if (_state.output_buffer.Count == 0)
                return ActionResult.Blocked(FischerTropschFailureCodes.OutputUnavailable, "ft.no_output");
            if (_inventory == null)
                return ActionResult.Blocked(FischerTropschFailureCodes.OutputUnavailable, "ft.inventory_unbound");

            var bill = new InventoryBill();
            foreach (var output in _state.output_buffer)
            {
                AddOutputGrants(bill, output);
            }
            if (!_inventory.TryExecuteTransaction(bill, lookup: _itemLookup))
                return ActionResult.Blocked(FischerTropschFailureCodes.OutputUnavailable, "ft.output_unavailable");

            int total = 0;
            foreach (var output in _state.output_buffer)
                total += output.lubricant_units + output.wax_units + output.light_fraction_units;
            _state.output_buffer.Clear();
            return ActionResult.Success("ft.outputs_claimed", new Dictionary<string, double> { ["units"] = total });
        }

        public ActionResult ServiceLubricantConsumer(string consumerId, int tick = 0)
        {
            if (!_consumers.TryGetValue(consumerId, out var consumer))
                return ActionResult.Blocked(FischerTropschFailureCodes.ConsumerUnknown, "ft.consumer_unknown");
            var product = FindLubricantProduct(consumer);
            if (product == null || _inventory == null || !_inventory.HasSufficient(product.item_id, consumer.service_units))
                return ActionResult.Blocked(FischerTropschFailureCodes.LubricantMissing, "ft.lubricant_missing");
            if (!_inventory.TryConsume(product.item_id, consumer.service_units))
                return ActionResult.Blocked(FischerTropschFailureCodes.LubricantMissing, "ft.lubricant_missing");

            if (!_state.serviced_consumers.Contains(consumerId)) _state.serviced_consumers.Add(consumerId);
            return ActionResult.Success("ft.consumer_serviced", new Dictionary<string, double>
            {
                ["wear_multiplier"] = consumer.wear_multiplier,
                ["service_tick"] = tick
            });
        }

        public float GetWearMultiplier(string consumerId)
        {
            if (!_consumers.TryGetValue(consumerId, out var consumer)) return 1f;
            return _state.serviced_consumers.Contains(consumerId) ? consumer.wear_multiplier : 1f;
        }

        public FischerTropschSynthesisState CaptureState()
        {
            var copy = new FischerTropschSynthesisState
            {
                schema_version = _state.schema_version,
                rng_state = _rng is SeededRng seeded ? seeded.PeekState() : 0UL,
                catalyst_condition = _state.catalyst_condition,
                ticks_since_maintenance = _state.ticks_since_maintenance,
                next_batch_number = _state.next_batch_number,
                total_batches_completed = _state.total_batches_completed,
                output_buffer = new List<SynthesisOutputBatch>(),
                serviced_consumers = new List<string>(_state.serviced_consumers)
            };
            foreach (var output in _state.output_buffer)
                copy.output_buffer.Add(Clone(output));
            if (_state.active_batch != null)
                copy.active_batch = new FischerTropschBatchState
                {
                    batch_id = _state.active_batch.batch_id,
                    reactor_profile_id = _state.active_batch.reactor_profile_id,
                    progress_ticks = _state.active_batch.progress_ticks,
                    operator_skill = _state.active_batch.operator_skill,
                    feed_quality = _state.active_batch.feed_quality,
                    process_variation = _state.active_batch.process_variation
                };
            return copy;
        }

        public void RestoreState(FischerTropschSynthesisState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state = state;
            _state.output_buffer ??= new List<SynthesisOutputBatch>();
            _state.serviced_consumers ??= new List<string>();
            _state.catalyst_condition = Math.Clamp(_state.catalyst_condition, 0f, 100f);
            if (_rng is SeededRng seeded && _state.rng_state != 0UL)
                seeded.SeekState(_state.rng_state);
            _tick = Math.Max(_tick, _state.total_batches_completed);
        }

        private SynthesisOutputBatch BuildOutput(FischerTropschReactorProfile profile, FischerTropschBatchState batch, float conversion)
        {
            int lubricant = 0, wax = 0, light = 0;
            string grade = "standard";
            foreach (var product in _catalog.Products)
            {
                int amount = Math.Max(0, (int)Math.Round(conversion * product.split * product.units_per_conversion, MidpointRounding.AwayFromZero));
                if (product.output_kind == "lubricant") { lubricant += amount; grade = product.grade; }
                else if (product.output_kind == "wax") wax += amount;
                else if (product.output_kind == "light_fraction") light += amount;
            }
            return new SynthesisOutputBatch
            {
                batch_id = batch.batch_id,
                reactor_profile_id = profile.reactor_profile_id,
                lubricant_units = lubricant,
                wax_units = wax,
                light_fraction_units = light,
                lubricant_grade = grade,
                completed_tick = _tick
            };
        }

        private void AddOutputGrants(InventoryBill bill, SynthesisOutputBatch output)
        {
            foreach (var product in _catalog.Products)
            {
                int amount = product.output_kind switch
                {
                    "lubricant" => output.lubricant_units,
                    "wax" => output.wax_units,
                    "light_fraction" => output.light_fraction_units,
                    _ => 0
                };
                if (amount > 0) bill.AddGrant(product.item_id, amount);
            }
        }

        private FischerTropschProductProfile? FindLubricantProduct(MechanicalLubricantConsumer consumer)
        {
            foreach (var product in _catalog.Products)
            {
                if (product.output_kind != "lubricant") continue;
                if (consumer.accepted_grades == null || consumer.accepted_grades.Count == 0 || consumer.accepted_grades.Contains(product.grade))
                    return product;
            }
            return null;
        }

        private static float BandModifier(float value, float min, float max, float floor)
        {
            if (value >= min && value <= max) return 1f;
            float distance = value < min ? min - value : value - max;
            return Math.Max(floor, 1f - Math.Min(1f, distance * 2f));
        }

        private static SynthesisOutputBatch Clone(SynthesisOutputBatch source) => new SynthesisOutputBatch
        {
            batch_id = source.batch_id,
            reactor_profile_id = source.reactor_profile_id,
            lubricant_units = source.lubricant_units,
            wax_units = source.wax_units,
            light_fraction_units = source.light_fraction_units,
            lubricant_grade = source.lubricant_grade,
            completed_tick = source.completed_tick
        };
    }
}
