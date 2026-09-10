using DMello.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Domain.Models
{
    public class InventoryItem //(Master Item Record):
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int QuantityOnHand { get; set; }

        // Core Enum classification
        public ItemCategoryType Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
