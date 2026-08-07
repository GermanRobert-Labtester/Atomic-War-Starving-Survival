using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AirliftState
    {
        public string victoryId = "victory_airlift";
        public bool isActive = false;
        public float defenseTimerSeconds = 1440f; // 24 * 60
        public int currentWaveNumber = 0;
        public float waveIntervalSeconds = 60f;
        public int survivorsOnRoof = 0;
        public bool isExtracted = false;
        public int wavesDefeated = 0;
    }

    /// <summary>
    /// Prompt #562: Endgame: The Airlift (Last Stand).
    /// Helicopter extraction at Skyscraper node. Abandon bunker, move all survivors to roof,
    /// defend for 24 real-time minutes against endless waves.
    /// </summary>
    public class Victory_Airlift
    {
        private AirliftState _state = new AirliftState();

        public event Action<AirliftState, int> OnAirliftInitiated;
        public event Action<AirliftState, int> OnWaveStarted;
        public event Action<AirliftState, int> OnWaveDefeated;
        public event Action<AirliftState> OnDefenseFailed;
        public event Action<AirliftState> OnAirliftExtracted;

        private float _timeSinceLastWave = 0f;
        private bool _defenseFailed = false;

        public AirliftState State => _state;

        public void StartDefense(int survivorCount)
        {
            _state.survivorsOnRoof = survivorCount;
            _state.isActive = true;
            _state.currentWaveNumber = 0;
            _state.wavesDefeated = 0;
            _timeSinceLastWave = 0f;
            _defenseFailed = false;
            OnAirliftInitiated?.Invoke(_state, survivorCount);
        }

        public void TickRealTime(float deltaSeconds)
        {
            if (!_state.isActive || _defenseFailed) return;

            _state.defenseTimerSeconds -= deltaSeconds;
            _timeSinceLastWave += deltaSeconds;

            if (_timeSinceLastWave >= _state.waveIntervalSeconds)
            {
                _timeSinceLastWave -= _state.waveIntervalSeconds;
                _state.currentWaveNumber++;
                OnWaveStarted?.Invoke(_state, _state.currentWaveNumber);
            }

            if (_state.defenseTimerSeconds <= 0f && !_defenseFailed)
            {
                _state.defenseTimerSeconds = 0f;
                _state.isExtracted = true;
                OnAirliftExtracted?.Invoke(_state);
            }
        }

        public void SpawnWave(int waveNumber, System.Random rng)
        {
            if (!_state.isActive) return;

            _state.currentWaveNumber = waveNumber;
            OnWaveStarted?.Invoke(_state, waveNumber);
        }

        public bool ResolveWaveDefense(float defensePower)
        {
            if (!_state.isActive || _defenseFailed) return false;

            // Defense succeeds if power is positive; scaling is handled externally
            if (defensePower > 0f)
            {
                _state.wavesDefeated++;
                OnWaveDefeated?.Invoke(_state, _state.currentWaveNumber);
                return true;
            }

            _defenseFailed = true;
            _state.isActive = false;
            OnDefenseFailed?.Invoke(_state);
            return false;
        }

        public bool IsDefenseComplete()
        {
            return _state.defenseTimerSeconds <= 0f && _state.isExtracted;
        }

        public bool IsVictoryAchieved()
        {
            return _state.isExtracted;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public AirliftState CaptureState()
        {
            return new AirliftState
            {
                victoryId = _state.victoryId,
                isActive = _state.isActive,
                defenseTimerSeconds = _state.defenseTimerSeconds,
                currentWaveNumber = _state.currentWaveNumber,
                waveIntervalSeconds = _state.waveIntervalSeconds,
                survivorsOnRoof = _state.survivorsOnRoof,
                isExtracted = _state.isExtracted,
                wavesDefeated = _state.wavesDefeated,
            };
        }

        public void RestoreState(AirliftState state)
        {
            if (state == null)
            {
                _state = new AirliftState();
                _timeSinceLastWave = 0f;
                _defenseFailed = false;
                return;
            }
            _state = new AirliftState
            {
                victoryId = state.victoryId,
                isActive = state.isActive,
                defenseTimerSeconds = state.defenseTimerSeconds,
                currentWaveNumber = state.currentWaveNumber,
                waveIntervalSeconds = state.waveIntervalSeconds,
                survivorsOnRoof = state.survivorsOnRoof,
                isExtracted = state.isExtracted,
                wavesDefeated = state.wavesDefeated,
            };
            _timeSinceLastWave = 0f;
            _defenseFailed = false;
        }
    }
}
