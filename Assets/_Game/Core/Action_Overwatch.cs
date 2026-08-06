using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public sealed class OverwatchState
    {
        public string actionId = "action_overwatch";
        public int rangeNodes = 2;
        public float ambushReduction = 0.6f;

        // Currently stationed sniper -> node.
        public string stationedSniperId = "";
        public string stationedNodeId = "";
    }

    public sealed class Action_Overwatch
    {
        public event Action<string, string> OnOverwatchStationed;      // (sniperId, nodeId)
        public event Action<string, string> OnCoveringFireProvided;    // (sniperId, scavengerId)

        private OverwatchState _state = new OverwatchState();

        // Station a survivor (equipped with a sniper rifle) on a high-elevation node.
        public void Station(string survivorId, string highNode)
        {
            if (string.IsNullOrEmpty(survivorId))
                throw new ArgumentNullException(nameof(survivorId));
            if (string.IsNullOrEmpty(highNode))
                throw new ArgumentNullException(nameof(highNode));

            _state.stationedSniperId = survivorId;
            _state.stationedNodeId = highNode;
            OnOverwatchStationed?.Invoke(survivorId, highNode);
        }

        // Attempt to provide covering fire for a scavenger at a given node.
        // Returns true if the scavenger is within range and covering fire is active
        // (ambush lethality reduced by 60%).
        public bool ProvideCover(string sniperId, string scavengerNodeId, string sniperNodeId)
        {
            if (_state.stationedSniperId != sniperId) return false;
            if (string.IsNullOrEmpty(_state.stationedNodeId)) return false;
            if (!IsInRange(sniperNodeId, scavengerNodeId)) return false;

            // Covering fire is active — caller should apply ambushReduction.
            OnCoveringFireProvided?.Invoke(sniperId, scavengerNodeId);
            return true;
        }

        // Check if two nodes are within overwatch range.
        // Node IDs follow the convention "node_X"; distance is computed from a
        // precomputed adjacency/distance table supplied by the caller, or
        // falling back to a simple numeric-difference heuristic.
        public bool IsInRange(string nodeA, string nodeB)
        {
            if (nodeA == nodeB) return true;

            int dist = NodeDistance(nodeA, nodeB);
            return dist >= 0 && dist <= _state.rangeNodes;
        }

        public float GetAmbushReduction() => _state.ambushReduction;
        public int GetRange() => _state.rangeNodes;

        // --- Node distance helpers -------------------------------------------
        // The caller may register a custom distance function. If none is
        // registered we fall back to parsing numeric suffixes.
        private static Func<string, string, int> _distanceFunc;

        public static void SetDistanceFunction(Func<string, string, int> func)
        {
            _distanceFunc = func;
        }

        private static int NodeDistance(string a, string b)
        {
            if (_distanceFunc != null)
                return _distanceFunc(a, b);

            // Heuristic: parse trailing integer from node ids (e.g. "node_3").
            int ia = ParseTrailingInt(a);
            int ib = ParseTrailingInt(b);
            if (ia < 0 || ib < 0) return -1; // unknown
            return Math.Abs(ia - ib);
        }

        private static int ParseTrailingInt(string s)
        {
            if (string.IsNullOrEmpty(s)) return -1;
            int i = s.Length - 1;
            while (i >= 0 && char.IsDigit(s[i])) i--;
            string num = s.Substring(i + 1);
            return int.TryParse(num, out int v) ? v : -1;
        }

        // --- Save / Load -----------------------------------------------------
        public OverwatchState CaptureState() => new OverwatchState
        {
            actionId = _state.actionId,
            rangeNodes = _state.rangeNodes,
            ambushReduction = _state.ambushReduction,
            stationedSniperId = _state.stationedSniperId,
            stationedNodeId = _state.stationedNodeId
        };

        public void RestoreState(OverwatchState saved)
        {
            _state.actionId = saved.actionId;
            _state.rangeNodes = saved.rangeNodes;
            _state.ambushReduction = saved.ambushReduction;
            _state.stationedSniperId = saved.stationedSniperId;
            _state.stationedNodeId = saved.stationedNodeId;
        }
    }
}
