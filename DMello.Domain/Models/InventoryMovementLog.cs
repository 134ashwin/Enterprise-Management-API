using DMello.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Domain.Models
{
    public class InventoryMovementLog // (Stock Audit Ledger):  
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid InventoryItemId { get; set; }
        public InventoryItem? InventoryItem { get; set; }

        public int Quantity { get; set; }
        public MovementType MovementType { get; set; }
        public ProductionProcessType? ProcessType { get; set; } // Nullable if not in production

        public string ExecutedBy { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
