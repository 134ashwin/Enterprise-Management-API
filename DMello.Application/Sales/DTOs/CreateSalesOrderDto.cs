using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Application.Sales.DTOs
{
    public class CreateSalesOrderDto
    {
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string MainSku { get; set; } = string.Empty;
        public string SubSku { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
