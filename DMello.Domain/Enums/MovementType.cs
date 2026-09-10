using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Domain.Enums
{
    public enum MovementType
    {
        SupplierToRawMaterial = 1,
        SupplierToFinishedProduct = 2,
        RawMaterialToProduction = 3,
        ProductToProduction = 4,        // Sending back for Printing/Dyeing/Washing
        ProductionToFinishedProduct = 5,
        RawMaterialToSales = 6,
        FinishedProductToSales = 7
    }
}
