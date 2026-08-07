using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TripwireMazeState
    {
        public string id = "encounter_tripwire_maze";
        public string displayName = "The Tripwire Maze";
        public int gridSize = 4;
        public float explosionDamage = 80f;
        public float agilityBypassThreshold = 80f;
        public int correctPathIndex = -1;
        public bool isSolved = false;
    }

    /// <summary>
    /// Prompt #602: Encounter: The Tripwire Maze.
    /// A node heavily fortified by BlackOps with no visible enemies. A grid of paths
    /// is presented; choosing the wrong path triggers an explosion. High Agility bypasses entirely.
    /// </summary>
    public class Encounter_TripwireMaze
    {
        private TripwireMazeState _state = new TripwireMazeState();

        public event Action<TripwireMazeState> OnMazeEntered;
        public event Action<TripwireMazeState, int, bool> OnPathChosen;
        public event Action<TripwireMazeState, float> OnExplosionTriggered;
        public event Action<TripwireMazeState> OnMazeSolved;
        public event Action<TripwireMazeState> OnMazeBypassed;

        public TripwireMazeState State => _state;

        /// <summary>
        /// Generates the maze, selecting the correct path index randomly.
        /// </summary>
        public void GenerateMaze(System.Random rng)
        {
            if (rng == null)
                return;

            _state.correctPathIndex = rng.Next(0, _state.gridSize);
            _state.isSolved = false;

            OnMazeEntered?.Invoke(_state);
        }

        /// <summary>
        /// Attempts a path through the maze.
        /// </summary>
        /// <param name="pathIndex">The chosen path index (0 to gridSize-1).</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>True if the path is correct (safe); false triggers an explosion.</returns>
        public bool TryPath(int pathIndex, System.Random rng)
        {
            if (_state.isSolved || pathIndex < 0 || pathIndex >= _state.gridSize)
                return false;

            bool isCorrect = pathIndex == _state.correctPathIndex;

            OnPathChosen?.Invoke(_state, pathIndex, isCorrect);

            if (isCorrect)
            {
                _state.isSolved = true;
                OnMazeSolved?.Invoke(_state);
                return true;
            }

            OnExplosionTriggered?.Invoke(_state, _state.explosionDamage);
            return false;
        }

        /// <summary>
        /// Attempts to bypass the maze entirely using high Agility.
        /// </summary>
        /// <param name="agility">The survivor's agility stat.</param>
        /// <returns>True if bypass succeeded.</returns>
        public bool TryBypass(float agility)
        {
            if (_state.isSolved)
                return false;

            if (agility >= _state.agilityBypassThreshold)
            {
                _state.isSolved = true;
                OnMazeBypassed?.Invoke(_state);
                return true;
            }

            return false;
        }

        public TripwireMazeState CaptureState() => _state;

        public void RestoreState(TripwireMazeState saved)
        {
            _state = saved ?? new TripwireMazeState();
        }
    }
}
