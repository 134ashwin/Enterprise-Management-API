using System;
using System.Collections.Generic;
using System.Text;


//Why it exists: Represents the SalesOrders table in SQL Server.

//What problem it solves: Stores the exact fields required by Angular UI
//(Date, OrderNo, MainSKU, SubSKU, Size, Customer, Description).

namespace DMello.Domain.Models
{
    public class SalesOrdersModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; }        // Date (D/M/Y)
        public string OrderNo { get; set; } = string.Empty;  // Order No
        public string MainSku { get; set; } = string.Empty;  // Main SKU
        public string SubSku { get; set; } = string.Empty;   // Sub SKU
        public string Size { get; set; } = string.Empty;     // Size
        public string Customer { get; set; } = string.Empty; // Customer
        public string Description { get; set; } = string.Empty; // Description
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


