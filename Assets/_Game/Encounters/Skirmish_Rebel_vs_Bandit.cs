using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class RebelVsBanditState
    {
        public string id = "skirmish_rebel_vs_bandit";
        public string locationId;
        public float karmaDropOnInterveneAgainstRebels = 30f;
        public bool isLootingBanditsStealing = true;
    }

    /// <summary>
    /// Prompt #343: Skirmish: Rebels vs. Bandits.
    /// Rebels are executing thieves. Intervening against Rebels incurs a severe Karma penalty (-30).
    /// Looting dead Bandits is tagged as Stealing in the eyes of the Rebels.
    /// </summary>
    public class Skirmish_Rebel_vs_Bandit
    {
        private RebelVsBanditState _state = new RebelVsBanditState();

        public event Action<RebelVsBanditState, float> OnIntervenedAgainstRebels;
        public event Action<RebelVsBanditState> OnLootedDeadBanditAsStealing;

        public RebelVsBanditState State => _state;

        public Skirmish_Rebel_vs_Bandit(string locationId)
        {
            _state.locationId = locationId;
        }

        public float InterveneAgainstRebels()
        {
            OnIntervenedAgainstRebels?.Invoke(_state, _state.karmaDropOnInterveneAgainstRebels);
            return _state.karmaDropOnInterveneAgainstRebels;
        }

        public void LootDeadBandits(ref bool isCaughtStealing)
        {
            if (_state.isLootingBanditsStealing)
            {
                isCaughtStealing = true;
                OnLootedDeadBanditAsStealing?.Invoke(_state);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RebelVsBanditState CaptureState() => _state;

        public void RestoreState(RebelVsBanditState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
