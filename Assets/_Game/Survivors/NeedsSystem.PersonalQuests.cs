using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Personal-quest integration for <see cref="NeedsSystem"/>. Kept separate
    /// from physiological need drift so either concern can be reviewed safely.
    /// </summary>
    public partial class NeedsSystem
    {
        private static readonly string[] SmallUndergroundRoomKeywords =
        {
            "bunker", "cellar", "tunnel", "shaft",
            "deep", "mine", "sublevel", "basement"
        };

        private void ApplyPersonalQuestEffects(Survivor survivor, float gameHours)
        {
            if (_personalQuests == null) return;

            ApplyEmpathyAndMoraleFloor(survivor, gameHours);
            ApplyFatigueCapHungerStrikeAndHypochondria(survivor);
            ApplyBunkerMoraleEffects(survivor, gameHours);
            TryConsumeForcedChem(survivor);
            ApplyStaminaBurdens(survivor, gameHours);
            ApplyRoomAndHygieneEffects(survivor, gameHours);
            ApplyQuestHealthCap(survivor);
            MirrorTwinPartnerNeeds(survivor);
        }

        private void ApplyEmpathyAndMoraleFloor(Survivor survivor, float gameHours)
        {
            // #262 Hyper-Empathetic: morale drifts toward bunker average.
            if (_personalQuests.HasHyperEmpathetic(survivor))
            {
                float averageMorale = ComputeBunkerAverageMorale(survivor);
                _personalQuests.ApplyHyperEmpatheticMorale(survivor, averageMorale, gameHours);
            }

            // #265 Living Saint: permanent Inspired morale floor for the bunker.
            _personalQuests.ApplyLivingSaintMoraleFloor(survivor);
        }

        private void ApplyFatigueCapHungerStrikeAndHypochondria(Survivor survivor)
        {
            // #268 Restless: clamp fatigue to permanent cap.
            float fatigueCap = _personalQuests.GetMaxFatigueCap(survivor);
            if (survivor.Needs.Fatigue > fatigueCap)
                survivor.Needs.Fatigue = fatigueCap;

            // #275 Hunger strike tick (Pacifist Monk).
            _personalQuests.TickHungerStrike(survivor);

            // #269 Hypochondriac: fake illness without placebo → real morale/fatigue hit.
            if (_personalQuests.ShouldGenerateFakeAfflictionAlert(survivor))
                _personalQuests.ApplyHypochondriacPlaceboTick(survivor, givenPlacebo: false);
        }

        private void ApplyBunkerMoraleEffects(Survivor survivor, float gameHours)
        {
            // #278 Moral Compass aura (bunker-wide small morale).
            float compass = _personalQuests.GetMoralCompassBunkerMorale(GetSurvivors());
            if (compass > 0f)
            {
                survivor.Needs.Morale = Mathf.Min(
                    100f,
                    survivor.Needs.Morale + compass * gameHours * 0.1f);
            }

            // #282 Agoraphile: fractional bunker morale hit (day counter advances in TickDaily).
            if (!_personalQuests.HasAgoraphile(survivor) || survivor.IsOnExpedition) return;

            float hit = _personalQuests.GetAgoraphileBunkerMoraleHitPerDay(survivor)
                * (gameHours / 24f);
            if (hit > 0f)
                survivor.Needs.Morale = Mathf.Max(0f, survivor.Needs.Morale - hit);
        }

        private void TryConsumeForcedChem(Survivor survivor)
        {
            // #267 Forced chem: only reset dose clock when host actually drains stock.
            // Missing stock leaves HoursSinceLastDose running.
            if (!_personalQuests.ShouldForceConsumeMedicalChems(survivor)) return;

            bool consumed = ForcedChemConsumeHandler != null
                && ForcedChemConsumeHandler(survivor);
            if (!consumed) return;

            survivor.HoursSinceLastDose = 0f;
            _personalQuests.NotifyChemUsed(survivor);
        }

        private void ApplyStaminaBurdens(Survivor survivor, float gameHours)
        {
            ApplyFailingHeartFatigue(survivor, gameHours);
            ApplyBlackLungFatigue(survivor, gameHours);
        }

        private void ApplyFailingHeartFatigue(Survivor survivor, float gameHours)
        {
            if (!_personalQuests.HasFailingHeart(survivor)) return;

            _personalQuests.TickFailingHeart(survivor, currentDay: survivor.DaysAlive);
            float stamina = _personalQuests.GetFailingHeartStaminaMax(
                survivor,
                survivor.DaysAlive);
            if (stamina >= 100f) return;

            float pressure = (1f - stamina / 100f) * 2f * gameHours;
            survivor.Needs.Fatigue = Mathf.Min(100f, survivor.Needs.Fatigue + pressure);
        }

        private void ApplyBlackLungFatigue(Survivor survivor, float gameHours)
        {
            if (!_personalQuests.HasBlackLung(survivor)) return;

            float staminaMultiplier = _personalQuests.GetBlackLungStaminaMaxMultiplier(survivor);
            if (staminaMultiplier >= 1f) return;

            float pressure = (1f - staminaMultiplier) * 3f * gameHours;
            survivor.Needs.Fatigue = Mathf.Min(100f, survivor.Needs.Fatigue + pressure);
        }

        private void ApplyRoomAndHygieneEffects(Survivor survivor, float gameHours)
        {
            // #284 Claustrophilic: morale gain when already deep/underground.
            if (_personalQuests.HasClaustrophilic(survivor)
                && IsSmallUndergroundRoomId(survivor.CurrentRoomId))
            {
                _personalQuests.ApplyClaustrophilicMorale(
                    survivor,
                    inSmallUndergroundRoom: true,
                    gameHours: gameHours);
            }

            // #287 Neat Freak + #281 Photogenic: hygiene-driven morale pressure.
            float hygiene01 = survivor.Needs.Hygiene / 100f;
            _personalQuests.ApplyNeatFreakHygienePressure(survivor, hygiene01);
            _personalQuests.ApplyPhotogenicHygieneMorale(survivor, hygiene01);
        }

        private void ApplyQuestHealthCap(Survivor survivor)
        {
            // #291 Frail: clamp health to cap.
            float healthCap = _personalQuests.GetMaxHealthCapForQuests(survivor);
            if (survivor.Needs.Health > healthCap)
                survivor.Needs.Health = healthCap;
        }

        private void MirrorTwinPartnerNeeds(Survivor survivor)
        {
            // #305 Twin Beta: mirror Twin Alpha's needs until Hive Healing unlocks independence.
            string twinPartnerId = _personalQuests.GetTwinPartnerId(survivor);
            if (string.IsNullOrEmpty(twinPartnerId)) return;

            Survivor partner = FindSurvivor(twinPartnerId, GetSurvivors());
            if (partner != null)
                _personalQuests.MirrorTwinAlphaNeeds(survivor, partner);
        }

        private IReadOnlyList<Survivor> GetSurvivors()
        {
            return _getSurvivors != null ? _getSurvivors() : _survivors;
        }

        private static Survivor FindSurvivor(
            string survivorId,
            IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return null;

            for (int i = 0; i < survivors.Count; i++)
            {
                Survivor candidate = survivors[i];
                if (candidate != null
                    && string.Equals(candidate.Id, survivorId, StringComparison.Ordinal))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static bool IsSmallUndergroundRoomId(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;

            for (int i = 0; i < SmallUndergroundRoomKeywords.Length; i++)
            {
                if (roomId.IndexOf(
                        SmallUndergroundRoomKeywords[i],
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private float ComputeBunkerAverageMorale(Survivor exclude = null)
        {
            IReadOnlyList<Survivor> survivors = GetSurvivors();
            if (survivors == null || survivors.Count == 0) return 50f;

            float sum = 0f;
            int count = 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                Survivor candidate = survivors[i];
                if (candidate == null || !candidate.IsAlive || candidate.Needs == null) continue;
                if (exclude != null && ReferenceEquals(candidate, exclude)) continue;
                sum += candidate.Needs.Morale;
                count++;
            }

            return count > 0 ? sum / count : 50f;
        }
    }
}
