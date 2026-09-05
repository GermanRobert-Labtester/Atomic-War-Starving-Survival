using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Combat
{
    public enum ShieldStance
    {
        Stowed,
        Carried,
        Raised,
        Braced,
        Anchored,
        Phalanx,
        Broken
    }

    public enum ShieldDamageResultType
    {
        Blocked,
        Penetrated,
        Glanced,
        ShieldShattered
    }

    [Serializable]
    public sealed class BallisticShieldDef
    {
        public string shield_id = string.Empty;
        public string display_name = string.Empty;
        public float coverage_arc_deg = 90.0f;
        public float frontal_block_rating = 0.70f;
        public float integrity_max = 80.0f;
        public float viewport_clarity = 0.90f;
        public float stamina_cost_per_tick = 2.5f;
        public bool anchor_supported;
        public float movement_multiplier = 0.80f;
        public float suppression_resistance = 0.55f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class BallisticShieldCatalog
    {
        public int schema_version = 1;
        public List<BallisticShieldDef> shields = new List<BallisticShieldDef>();
    }

    [Serializable]
    public sealed class BallisticShieldState
    {
        public string equippedShieldId = string.Empty;
        public ShieldStance stance = ShieldStance.Stowed;
        public float currentIntegrity;
        public float viewportIntegrity = 1.0f;
        public bool isAnchored;
        public int anchorSpikesRemaining = 4;
        public float staminaStrain;
        public int phalanxLinkedCount;
        public int totalShotsBlocked;
        public float totalDamageAbsorbed;
    }

    public sealed class ShieldBlockResult
    {
        public bool success;
        public float absorbedDamage;
        public float penetratingDamage;
        public ShieldDamageResultType resultType;
        public float remainingIntegrity;
        public bool shattered;
    }

    public static class BallisticShieldCatalogLoader
    {
        public const string DefaultFileName = "ballistic_shield_catalog.json";

        public static BallisticShieldCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[BallisticShield] catalog not found at {path}");
                return new BallisticShieldCatalog();
            }

            try
            {
                string text = fileIO.ReadAllText(path);
                var cat = json.Deserialize<BallisticShieldCatalog>(text);
                return cat ?? new BallisticShieldCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[BallisticShield] failed loading catalog: {ex.Message}");
                return new BallisticShieldCatalog();
            }
        }
    }

    public sealed class BallisticShieldEngine
    {
        public const string SystemId = "ballistic_shield";
        public const string ItemAnchorSpikes = "item_hardened_ground_anchor_spikes";
        public const string ItemViewportGlass = "item_laminated_ballistic_viewport_glass";

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog? _log;

        private BallisticShieldCatalog _catalog = new BallisticShieldCatalog();
        private BallisticShieldState _state = new BallisticShieldState();

        public event Action<BallisticShieldState>? OnStateChanged;
        public event Action<ShieldStance>? OnStanceChanged;
        public event Action<ShieldBlockResult>? OnDamageBlocked;
        public event Action<string>? OnShieldBroken;
        public event Action<float>? OnViewportCracked;

        public BallisticShieldState State => _state;
        public BallisticShieldCatalog Catalog => _catalog;

        public BallisticShieldEngine(Inventory.Inventory inventory, ISeededRng rng, ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log;
        }

        public void LoadCatalog(BallisticShieldCatalog catalog)
        {
            _catalog = catalog ?? new BallisticShieldCatalog();
        }

        public ActionResult EquipShield(string shieldId)
        {
            var def = _catalog.shields.FirstOrDefault(s => s.shield_id == shieldId);
            if (def == null)
                return ActionResult.Failed("unknown_shield", $"Shield ID '{shieldId}' not found in catalog.");

            _state.equippedShieldId = shieldId;
            _state.currentIntegrity = def.integrity_max;
            _state.viewportIntegrity = 1.0f;
            _state.stance = ShieldStance.Carried;
            _state.isAnchored = false;
            _state.phalanxLinkedCount = 0;

            _log?.Info($"[BallisticShield] Equipped shield '{def.display_name}'. Integrity: {def.integrity_max}.");
            OnStanceChanged?.Invoke(_state.stance);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("shield_equipped");
        }

        public ActionResult SetStance(ShieldStance newStance)
        {
            if (string.IsNullOrEmpty(_state.equippedShieldId))
                return ActionResult.Blocked("no_shield", "No ballistic shield equipped.");

            if (_state.stance == ShieldStance.Broken && newStance != ShieldStance.Stowed)
                return ActionResult.Blocked("shield_broken", "Shield is shattered and cannot be raised.");

            if (_state.isAnchored && newStance != ShieldStance.Anchored)
            {
                _state.isAnchored = false;
            }

            _state.stance = newStance;
            if (newStance != ShieldStance.Phalanx)
            {
                _state.phalanxLinkedCount = 0;
            }

            _log?.Info($"[BallisticShield] Stance changed to {newStance}.");
            OnStanceChanged?.Invoke(newStance);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("stance_changed");
        }

        public ActionResult AnchorToGround()
        {
            var def = _catalog.shields.FirstOrDefault(s => s.shield_id == _state.equippedShieldId);
            if (def == null)
                return ActionResult.Blocked("no_shield", "No shield equipped.");

            if (!def.anchor_supported)
                return ActionResult.Blocked("not_supported", "This shield model lacks ground anchor spike mounts.");

            if (_state.anchorSpikesRemaining <= 0)
            {
                if (_inventory.CountById(ItemAnchorSpikes) > 0)
                {
                    _inventory.TryConsumeById(ItemAnchorSpikes, 1);
                    _state.anchorSpikesRemaining += 2;
                }
                else
                {
                    return ActionResult.Blocked("missing_spikes", "Ground anchor spikes depleted.");
                }
            }

            _state.anchorSpikesRemaining--;
            _state.isAnchored = true;
            _state.stance = ShieldStance.Anchored;

            _log?.Info("[BallisticShield] Deployed ground anchor spikes. Heavy cover stance locked.");
            OnStanceChanged?.Invoke(_state.stance);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("anchored");
        }

        public ActionResult Unanchor()
        {
            if (!_state.isAnchored)
                return ActionResult.Blocked("not_anchored", "Shield is not anchored.");

            _state.isAnchored = false;
            _state.stance = ShieldStance.Braced;
            _log?.Info("[BallisticShield] Disengaged ground anchors.");
            OnStanceChanged?.Invoke(_state.stance);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("unanchored");
        }

        public ActionResult JoinPhalanx(int allyCount)
        {
            var def = _catalog.shields.FirstOrDefault(s => s.shield_id == _state.equippedShieldId);
            if (def == null)
                return ActionResult.Blocked("no_shield", "No shield equipped.");

            if (!def.tags.Contains("phalanx_capable"))
                return ActionResult.Blocked("not_phalanx_capable", "Shield design cannot interlock into a phalanx formation.");

            _state.phalanxLinkedCount = Math.Clamp(allyCount, 1, 4);
            _state.stance = ShieldStance.Phalanx;
            _log?.Info($"[BallisticShield] Interlocked into phalanx formation with {_state.phalanxLinkedCount} allies.");
            OnStanceChanged?.Invoke(_state.stance);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("phalanx_joined");
        }

        public ShieldBlockResult InterceptDamage(float incomingDamage, float hitAngleDeg, bool isPiercing = false)
        {
            var def = _catalog.shields.FirstOrDefault(s => s.shield_id == _state.equippedShieldId);
            if (def == null || _state.stance == ShieldStance.Stowed || _state.stance == ShieldStance.Broken)
            {
                return new ShieldBlockResult
                {
                    success = false,
                    absorbedDamage = 0f,
                    penetratingDamage = incomingDamage,
                    resultType = ShieldDamageResultType.Penetrated,
                    remainingIntegrity = _state.currentIntegrity,
                    shattered = false
                };
            }

            // Coverage check: hitAngleDeg relative to facing (-180 to +180, 0 is direct frontal)
            float halfArc = def.coverage_arc_deg * 0.5f;
            if (_state.stance == ShieldStance.Phalanx)
            {
                halfArc *= (1.0f + 0.15f * _state.phalanxLinkedCount);
            }

            float absAngle = Math.Abs(hitAngleDeg);
            if (absAngle > halfArc)
            {
                // Flanking hit outside coverage
                return new ShieldBlockResult
                {
                    success = false,
                    absorbedDamage = 0f,
                    penetratingDamage = incomingDamage,
                    resultType = ShieldDamageResultType.Penetrated,
                    remainingIntegrity = _state.currentIntegrity,
                    shattered = false
                };
            }

            // Compute block efficiency based on stance
            float stanceModifier = _state.stance switch
            {
                ShieldStance.Carried => 0.70f,
                ShieldStance.Raised => 1.00f,
                ShieldStance.Braced => 1.15f,
                ShieldStance.Anchored => 1.30f,
                ShieldStance.Phalanx => 1.25f,
                _ => 0.50f
            };

            float effectiveBlockRating = Math.Clamp(def.frontal_block_rating * stanceModifier, 0.20f, 0.95f);
            if (isPiercing)
            {
                effectiveBlockRating *= 0.75f;
            }

            float absorbable = incomingDamage * effectiveBlockRating;
            float actualAbsorbed = Math.Min(_state.currentIntegrity, absorbable);
            float penetrating = incomingDamage - actualAbsorbed;

            _state.currentIntegrity = Math.Max(0f, _state.currentIntegrity - actualAbsorbed);
            _state.totalDamageAbsorbed += actualAbsorbed;
            _state.totalShotsBlocked++;

            // Viewport degradation check
            if (_state.viewportIntegrity > 0.1f && _rng.NextDouble() < 0.25)
            {
                float deg = (float)(0.05 + _rng.NextDouble() * 0.15);
                _state.viewportIntegrity = Math.Max(0f, _state.viewportIntegrity - deg);
                OnViewportCracked?.Invoke(_state.viewportIntegrity);
            }

            bool shattered = _state.currentIntegrity <= 0.001f;
            if (shattered)
            {
                _state.stance = ShieldStance.Broken;
                _state.isAnchored = false;
                _log?.Warn($"[BallisticShield] Shield '{def.display_name}' shattered under heavy ballistic fire!");
                OnShieldBroken?.Invoke(def.shield_id);
                OnStanceChanged?.Invoke(_state.stance);
            }

            var res = new ShieldBlockResult
            {
                success = true,
                absorbedDamage = actualAbsorbed,
                penetratingDamage = penetrating,
                resultType = shattered ? ShieldDamageResultType.ShieldShattered : ShieldDamageResultType.Blocked,
                remainingIntegrity = _state.currentIntegrity,
                shattered = shattered
            };

            OnDamageBlocked?.Invoke(res);
            OnStateChanged?.Invoke(_state);
            return res;
        }

        public ActionResult RepairShield(float amount)
        {
            var def = _catalog.shields.FirstOrDefault(s => s.shield_id == _state.equippedShieldId);
            if (def == null)
                return ActionResult.Blocked("no_shield", "No shield equipped.");

            _state.currentIntegrity = Math.Min(def.integrity_max, _state.currentIntegrity + amount);
            if (_state.stance == ShieldStance.Broken && _state.currentIntegrity > 10f)
            {
                _state.stance = ShieldStance.Carried;
                OnStanceChanged?.Invoke(_state.stance);
            }

            _log?.Info($"[BallisticShield] Repaired shield to {_state.currentIntegrity:F1}/{def.integrity_max}.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("shield_repaired");
        }

        public ActionResult ReplaceViewport()
        {
            if (_state.viewportIntegrity >= 0.95f)
                return ActionResult.Blocked("viewport_intact", "Viewport is already in pristine condition.");

            if (_inventory.CountById(ItemViewportGlass) < 1)
                return ActionResult.Blocked("missing_glass", $"Requires 1x {ItemViewportGlass}.");

            _inventory.TryConsumeById(ItemViewportGlass, 1);
            _state.viewportIntegrity = 1.0f;
            _log?.Info("[BallisticShield] Replaced armored viewport pane.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("viewport_replaced");
        }

        public void TickCombatStamina(float staminaAvailable)
        {
            var def = _catalog.shields.FirstOrDefault(s => s.shield_id == _state.equippedShieldId);
            if (def == null || _state.stance == ShieldStance.Stowed || _state.stance == ShieldStance.Anchored)
            {
                _state.staminaStrain = Math.Max(0f, _state.staminaStrain - 1.0f);
                return;
            }

            float cost = def.stamina_cost_per_tick;
            if (_state.stance == ShieldStance.Braced) cost *= 1.4f;
            if (_state.stance == ShieldStance.Carried) cost *= 0.5f;

            if (staminaAvailable < cost)
            {
                // Fatigue causes wielder to lower shield
                _state.stance = ShieldStance.Carried;
                _log?.Info("[BallisticShield] Operator fatigued; shield lowered to carried stance.");
                OnStanceChanged?.Invoke(_state.stance);
            }
            else
            {
                _state.staminaStrain += cost;
            }

            OnStateChanged?.Invoke(_state);
        }

        public BallisticShieldState CaptureState()
        {
            return new BallisticShieldState
            {
                equippedShieldId = _state.equippedShieldId,
                stance = _state.stance,
                currentIntegrity = _state.currentIntegrity,
                viewportIntegrity = _state.viewportIntegrity,
                isAnchored = _state.isAnchored,
                anchorSpikesRemaining = _state.anchorSpikesRemaining,
                staminaStrain = _state.staminaStrain,
                phalanxLinkedCount = _state.phalanxLinkedCount,
                totalShotsBlocked = _state.totalShotsBlocked,
                totalDamageAbsorbed = _state.totalDamageAbsorbed
            };
        }

        public void RestoreState(BallisticShieldState? state)
        {
            if (state == null) return;
            _state = new BallisticShieldState
            {
                equippedShieldId = state.equippedShieldId,
                stance = state.stance,
                currentIntegrity = state.currentIntegrity,
                viewportIntegrity = state.viewportIntegrity,
                isAnchored = state.isAnchored,
                anchorSpikesRemaining = state.anchorSpikesRemaining,
                staminaStrain = state.staminaStrain,
                phalanxLinkedCount = state.phalanxLinkedCount,
                totalShotsBlocked = state.totalShotsBlocked,
                totalDamageAbsorbed = state.totalDamageAbsorbed
            };
            OnStateChanged?.Invoke(_state);
        }
    }
}
