using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Queue-based decontamination station: processes items (storage slots) one at a
    /// time, consumes clean water or a decon agent per tick, applies imperfect
    /// decontamination (residual floor). Player chooses to route dirty items through
    /// decon before storing them, keeping the shelter rooms clean.
    ///
    /// A decon job references a StorageSlot containing the item to be cleaned. On
    /// completion the slot's contamination is reduced to the station's residual floor
    /// (or as close as the available water/time allow).
    ///
    /// Save/load safe: serializes the queue of slot references as room-id + slot-index
    /// pairs.
    /// </summary>
    public class DecontaminationQueue
    {
        /// <summary>
        /// A pending decontamination job referencing a slot in a room.
        /// </summary>
        [Serializable]
        public class DeconJob
        {
            public string RoomId;
            public int SlotIndex;
            public float InitialContamination;
            public float Progress;
        }

        private readonly Queue<DeconJob> _queue = new Queue<DeconJob>();
        private DeconJob _currentJob;
        private float _timeRemainingOnCurrentJob;

        /// <summary>Rooms used to resolve job references during processing.</summary>
        private readonly List<ShelterRoom> _rooms = new List<ShelterRoom>();

        // ----- Data-driven parameters (from DeconStationModuleSO or defaults) -----

        /// <summary>Decontamination rate (contamination units per hour).</summary>
        public float DeconRatePerHour = 0.5f;

        /// <summary>Water consumed per hour of decon operation.</summary>
        public float WaterCostPerHour = 2f;

        /// <summary>Residual contamination floor: items can never be cleaned below this (0..1).</summary>
        [Range(0f, 1f)]
        public float ResidualFloor = 0.05f;

        /// <summary>Hours to fully process one item (if water is not the bottleneck).</summary>
        public float ProcessTimePerItem = 1f;

        /// <summary>Whether the station is operational (powered, etc.).</summary>
        public bool IsOperational = true;

        /// <summary>Available clean water (consumed during decon).</summary>
        public float AvailableWater;

        /// <summary>Fired when a job completes (item cleaned).</summary>
        public event Action<DeconJob> OnJobCompleted;

        /// <summary>Fired when the queue changes (job added/removed).</summary>
        public event Action OnQueueChanged;

        /// <summary>Current job being processed (null if idle).</summary>
        public DeconJob CurrentJob => _currentJob;

        /// <summary>Number of jobs waiting in the queue.</summary>
        public int QueueLength => _queue.Count;

        /// <summary>Whether the station is currently processing a job.</summary>
        public bool IsBusy => _currentJob != null;

        /// <summary>Register a room so its slots can be resolved during processing.</summary>
        public void RegisterRoom(ShelterRoom room)
        {
            if (room != null && !_rooms.Contains(room))
            {
                _rooms.Add(room);
            }
        }

        /// <summary>Unregister a room.</summary>
        public void UnregisterRoom(ShelterRoom room)
        {
            _rooms.Remove(room);
        }

        /// <summary>
        /// Enqueue a storage slot for decontamination. Returns true if accepted.
        /// The slot must be non-empty and have contamination above the residual floor.
        /// </summary>
        public bool Enqueue(string roomId, int slotIndex)
        {
            var room = FindRoom(roomId);
            if (room == null || slotIndex < 0 || slotIndex >= room.Slots.Count) return false;
            var slot = room.Slots[slotIndex];
            if (slot.IsEmpty || slot.Contamination <= ResidualFloor) return false;

            var job = new DeconJob
            {
                RoomId = roomId,
                SlotIndex = slotIndex,
                InitialContamination = slot.Contamination,
                Progress = 0f
            };

            _queue.Enqueue(job);
            OnQueueChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Advance decontamination over elapsed game hours. Processes the current job
        /// (or starts the next one if idle), consumes water, applies decontamination
        /// with residual floor. Returns the amount of water consumed.
        /// </summary>
        public float Tick(float gameHours)
        {
            if (!IsOperational || gameHours <= 0f) return 0f;

            float waterConsumed = 0f;
            float timeLeft = gameHours;

            while (timeLeft > 0f)
            {
                // Start next job if idle
                if (_currentJob == null)
                {
                    if (_queue.Count == 0) break;
                    _currentJob = _queue.Dequeue();
                    _timeRemainingOnCurrentJob = ProcessTimePerItem;
                    OnQueueChanged?.Invoke();
                }

                // Resolve the slot
                var slot = ResolveSlot(_currentJob);
                if (slot == null || slot.IsEmpty)
                {
                    // Slot was emptied or room lost; discard this job
                    _currentJob = null;
                    OnQueueChanged?.Invoke();
                    continue;
                }

                // Process one time slice
                float timeSlice = Mathf.Min(timeLeft, _timeRemainingOnCurrentJob);
                if (timeSlice <= 0f) break;

                // Check water availability
                float waterNeeded = WaterCostPerHour * timeSlice;
                float waterAvailable = Mathf.Min(waterNeeded, AvailableWater);
                if (waterAvailable <= 0f)
                {
                    // Out of water: pause, return whatever time we didn't use
                    break;
                }

                float actualTimeUsed = waterAvailable / WaterCostPerHour;
                AvailableWater -= waterAvailable;
                waterConsumed += waterAvailable;

                // Apply decontamination
                float contamReduction = DeconRatePerHour * actualTimeUsed;
                float newContam = Mathf.Max(ResidualFloor, slot.Contamination - contamReduction);
                slot.Contamination = newContam;

                _currentJob.Progress += actualTimeUsed;
                _timeRemainingOnCurrentJob -= actualTimeUsed;

                // Job complete?
                if (_timeRemainingOnCurrentJob <= 0f || slot.Contamination <= ResidualFloor)
                {
                    OnJobCompleted?.Invoke(_currentJob);
                    _currentJob = null;
                    OnQueueChanged?.Invoke();
                }

                timeLeft -= actualTimeUsed;

                // If we ran out of water, stop
                if (waterAvailable < waterNeeded) break;
            }

            return waterConsumed;
        }

        /// <summary>
        /// Cancel the current job. Returns the job (item is left with its current
        /// partial contamination in the slot).
        /// </summary>
        public DeconJob CancelCurrentJob()
        {
            var job = _currentJob;
            _currentJob = null;
            OnQueueChanged?.Invoke();
            return job;
        }

        /// <summary>Clear the entire queue (emergency stop).</summary>
        public void ClearQueue()
        {
            _queue.Clear();
            _currentJob = null;
            OnQueueChanged?.Invoke();
        }

        /// <summary>Refill the water supply.</summary>
        public void RefillWater(float amount)
        {
            AvailableWater = Mathf.Max(0f, AvailableWater + amount);
        }

        /// <summary>Get a snapshot of the queue for save/serialization.</summary>
        public List<DeconJob> CaptureQueue()
        {
            var result = new List<DeconJob>(_queue);
            return result;
        }

        /// <summary>Restore the queue from a snapshot.</summary>
        public void RestoreQueue(List<DeconJob> jobs)
        {
            _queue.Clear();
            _currentJob = null;
            if (jobs == null) return;
            foreach (var job in jobs)
            {
                _queue.Enqueue(job);
            }
            OnQueueChanged?.Invoke();
        }

        private ShelterRoom FindRoom(string roomId)
        {
            foreach (var room in _rooms)
            {
                if (room != null && room.RoomId == roomId) return room;
            }
            return null;
        }

        private StorageSlot ResolveSlot(DeconJob job)
        {
            if (job == null) return null;
            var room = FindRoom(job.RoomId);
            if (room == null || job.SlotIndex < 0 || job.SlotIndex >= room.Slots.Count) return null;
            return room.Slots[job.SlotIndex];
        }
    }
}
