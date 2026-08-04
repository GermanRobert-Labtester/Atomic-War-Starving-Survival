using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Cartography Table (Prompt #67). Map knowledge is a physical resource.
    /// IntelNodes must be processed at this table using Pencils and Paper to
    /// chart nodes onto the map. Only charted/surveyed nodes appear on the
    /// Expedition UI. Uncharting a node costs a Pencil durability point.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class CartographySystem
    {
        public const string CartographyModuleId = "cartography_table";
        public const string PencilItemId = "pencil";
        public const string PaperItemId = "paper";

        /// <summary>Pencil uses before it's consumed (one per chart action).</summary>
        public const int DefaultPencilDurability = 8;

        /// <summary>Paper sheets consumed per chart action.</summary>
        public const int PaperPerChart = 1;

        /// <summary>Hours required to chart one IntelNode at the table.</summary>
        public const float ChartHoursPerNode = 2f;

        /// <summary>Science skill multiplies chart speed (1 + skill bonus).</summary>
        public const float ScienceSkillSpeedBonus = 0.5f;

        /// <summary>Set of node ids that have been charted onto the map.</summary>
        private readonly HashSet<string> _chartedNodeIds = new HashSet<string>();

        /// <summary>Current pencil durability remaining.</summary>
        private int _pencilDurability;

        /// <summary>Paper sheets currently stocked in the table.</summary>
        private int _paperStock;

        private Func<Shelter> _getShelter;
        private Func<string, int> _countInventoryItem;
        private Action<string, int> _consumeInventoryItem;

        // -- Events --
        public event Action<string> OnNodeCharted;        // nodeId
        public event Action OnSuppliesDepleted;
        public event Action OnStateChanged;

        public IReadOnlyCollection<string> ChartedNodeIds => _chartedNodeIds;
        public int PencilDurability => _pencilDurability;
        public int PaperStock => _paperStock;
        public bool HasSupplies => _pencilDurability > 0 && _paperStock > 0;
        public bool HasModule => _getShelter?.Invoke()?.GetModule(CartographyModuleId)?.IsOperational ?? false;

        public CartographySystem() { }

        public void Bind(
            Func<Shelter> getShelter,
            Func<string, int> countInventoryItem = null,
            Action<string, int> consumeInventoryItem = null)
        {
            _getShelter = getShelter;
            _countInventoryItem = countInventoryItem;
            _consumeInventoryItem = consumeInventoryItem;
        }

        /// <summary>Whether a node has been charted and should appear on the Expedition UI.</summary>
        public bool IsCharted(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _chartedNodeIds.Contains(nodeId);
        }

        /// <summary>
        /// Restock the table from inventory. Transfers one pencil (8 uses)
        /// and any available paper (up to 20 sheets).
        /// </summary>
        public bool RestockSupplies()
        {
            if (_countInventoryItem == null || _consumeInventoryItem == null) return false;

            // Only restock pencil if current one is depleted.
            if (_pencilDurability <= 0 && _countInventoryItem(PencilItemId) > 0)
            {
                _consumeInventoryItem(PencilItemId, 1);
                _pencilDurability = DefaultPencilDurability;
            }

            // Top up paper.
            int available = _countInventoryItem(PaperItemId);
            int space = Mathf.Max(0, 20 - _paperStock);
            int toTransfer = Mathf.Min(available, space);
            if (toTransfer > 0)
            {
                _consumeInventoryItem(PaperItemId, toTransfer);
                _paperStock += toTransfer;
            }

            OnStateChanged?.Invoke();
            return HasSupplies;
        }

        /// <summary>
        /// Chart a node from an IntelNode. Requires cartography table module,
        /// pencil durability, paper, and time.
        /// Returns true if the node was successfully charted.
        /// </summary>
        public bool ChartNode(string nodeId, float scienceSkill = 0f)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            if (!HasModule) return false;
            if (_chartedNodeIds.Contains(nodeId)) return true; // Already charted.

            if (!ConsumeChartSupplies()) return false;

            _chartedNodeIds.Add(nodeId);
            OnNodeCharted?.Invoke(nodeId);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Time required to chart a node, accounting for Science skill.
        /// </summary>
        public float GetChartHours(float scienceSkill)
        {
            float bonus = 1f + scienceSkill * ScienceSkillSpeedBonus;
            return ChartHoursPerNode / Mathf.Max(0.1f, bonus);
        }

        private bool ConsumeChartSupplies()
        {
            if (_pencilDurability <= 0 || _paperStock <= 0)
            {
                // Try to auto-restock from inventory first.
                RestockSupplies();
                if (_pencilDurability <= 0 || _paperStock <= 0)
                {
                    OnSuppliesDepleted?.Invoke();
                    return false;
                }
            }

            _pencilDurability--;
            _paperStock--;
            OnStateChanged?.Invoke();
            return true;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public CartographySave CaptureState()
        {
            var ids = new string[_chartedNodeIds.Count];
            _chartedNodeIds.CopyTo(ids);
            return new CartographySave
            {
                ChartedNodeIds = ids,
                PencilDurability = _pencilDurability,
                PaperStock = _paperStock
            };
        }

        public void RestoreState(CartographySave save)
        {
            _chartedNodeIds.Clear();
            _pencilDurability = 0;
            _paperStock = 0;
            if (save == null) return;
            _pencilDurability = Mathf.Max(0, save.PencilDurability);
            _paperStock = Mathf.Max(0, save.PaperStock);
            if (save.ChartedNodeIds != null)
                for (int i = 0; i < save.ChartedNodeIds.Length; i++)
                    if (!string.IsNullOrEmpty(save.ChartedNodeIds[i]))
                        _chartedNodeIds.Add(save.ChartedNodeIds[i]);
        }
    }

    [Serializable]
    public class CartographySave
    {
        public string[] ChartedNodeIds;
        public int PencilDurability;
        public int PaperStock;
    }
}
