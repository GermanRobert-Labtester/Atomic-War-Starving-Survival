// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Production
{
    /// <summary>
    /// Represents a single item delivery line within a DeliveryBill.
    /// </summary>
    public sealed class DeliveryItem
    {
        public string ItemId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public float Contamination { get; set; }

        public DeliveryItem() { }

        public DeliveryItem(string itemId, int amount, float contamination = 0f)
        {
            ItemId = itemId;
            Amount = amount;
            Contamination = contamination;
        }
    }

    /// <summary>
    /// Delivery bill passed from a producer system to an IOutputSink.
    /// </summary>
    public sealed class DeliveryBill
    {
        public string SourceSystemId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int Day { get; set; }
        public List<DeliveryItem> Items { get; } = new List<DeliveryItem>();

        public DeliveryBill AddItem(string itemId, int amount, float contamination = 0f)
        {
            if (!string.IsNullOrEmpty(itemId) && amount > 0)
                Items.Add(new DeliveryItem(itemId, amount, contamination));
            return this;
        }
    }

    /// <summary>
    /// Categorical delivery outcome status.
    /// </summary>
    public enum DeliveryStatus
    {
        Success = 0,
        Partial = 1,
        StorageFull = 2,
        WeightExceeded = 3,
        UnknownItem = 4,
        Rejected = 5
    }

    /// <summary>
    /// Result returned by IOutputSink.Deliver.
    /// </summary>
    public sealed class DeliveryResult
    {
        public DeliveryStatus Status { get; set; }
        public bool IsSuccess => Status == DeliveryStatus.Success;
        public string Reason { get; set; } = string.Empty;
        public int DeliveredCount { get; set; }
        public int RejectedCount { get; set; }
        public List<DeliveryItem> UndeliveredItems { get; set; } = new List<DeliveryItem>();

        public static DeliveryResult Succeeded(int deliveredCount, string reason = "") =>
            new DeliveryResult
            {
                Status = DeliveryStatus.Success,
                DeliveredCount = deliveredCount,
                Reason = reason
            };

        public static DeliveryResult Partial(int deliveredCount, int rejectedCount, string reason, IEnumerable<DeliveryItem>? undelivered = null)
        {
            var res = new DeliveryResult
            {
                Status = DeliveryStatus.Partial,
                DeliveredCount = deliveredCount,
                RejectedCount = rejectedCount,
                Reason = reason
            };
            if (undelivered != null) res.UndeliveredItems.AddRange(undelivered);
            return res;
        }

        public static DeliveryResult Failed(DeliveryStatus status, string reason, IEnumerable<DeliveryItem>? undelivered = null)
        {
            var res = new DeliveryResult
            {
                Status = status,
                Reason = reason,
                DeliveredCount = 0,
                RejectedCount = undelivered != null ? undelivered.Count() : 0
            };
            if (undelivered != null) res.UndeliveredItems.AddRange(undelivered);
            return res;
        }
    }

    /// <summary>
    /// Canonical output sink interface for all resource and item producers (Plan 35 / REM-007 / R13).
    /// Ensures every accepted yield reaches authoritative storage or remains claimable without loss.
    /// </summary>
    public interface IOutputSink
    {
        DeliveryResult Deliver(DeliveryBill bill);
        bool CanDeliver(DeliveryBill bill, out string reason);
    }
}
