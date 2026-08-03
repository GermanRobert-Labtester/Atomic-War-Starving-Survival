using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action: clean a contaminated shelter room. Scored by how much
    /// contamination is present above the threshold.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_CleanRoom", menuName = "ASHFALL/AI/Clean Room Action")]
    public class CleanRoomActionSO : SurvivorAction
    {
        [Header("Clean Room Parameters")]
        [Tooltip("Minimum room contamination to consider cleaning (0..1)")]
        public float minContamThreshold = 0.2f;

        [Tooltip("Amount of contamination removed per cleaning action")]
        public float cleanAmount = 0.3f;

        public CleanRoomActionSO()
        {
            id = "action_clean_room";
            displayName = "Clean Room";
            description = "Scrub down contaminated surfaces in the shelter.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context == null || context.Shelter == null) return 0f;
            if (context.Shelter.ContaminationEconomy == null) return 0f;

            // Find the most contaminated room
            float maxContam = 0f;
            var rooms = context.Shelter.ContaminationEconomy.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (room != null && room.AmbientContamination > maxContam)
                {
                    maxContam = room.AmbientContamination;
                }
            }

            // Only clean if contamination is above threshold
            if (maxContam < minContamThreshold) return 0f;

            // Score: higher contamination = higher priority
            // Normalize: threshold = 0 score, 1.0 = 1.0 score
            float normalized = (maxContam - minContamThreshold) / (1f - minContamThreshold);
            return Mathf.Clamp01(normalized);
        }

        public override void Execute(AIContext context)
        {
            if (context == null || context.Shelter == null) return;
            if (context.Shelter.ContaminationEconomy == null) return;

            // Find and clean the most contaminated room
            Shelter.ShelterRoom dirtiestRoom = null;
            float maxContam = 0f;
            var rooms = context.Shelter.ContaminationEconomy.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (room != null && room.AmbientContamination > maxContam)
                {
                    maxContam = room.AmbientContamination;
                    dirtiestRoom = room;
                }
            }

            if (dirtiestRoom != null)
            {
                context.Shelter.ContaminationEconomy.CleanRoom(dirtiestRoom, cleanAmount);
                Debug.Log($"[AI] Cleaned room '{dirtiestRoom.RoomId}', now at {dirtiestRoom.AmbientContamination:F2}");
            }
        }
    }
}
