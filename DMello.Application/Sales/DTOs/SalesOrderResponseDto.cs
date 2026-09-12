using System;
using System.Collections.Generic;
using System.Text;


//Why they exist: Decouples your internal database table structure from the JSON payloads sent over HTTP to Angular.
//What problem it solves: Prevents exposing internal database keys or implementation details directly to the client.

namespace DMello.Application.Sales.DTOs
{
    public class SalesOrderResponseDto
    {
        public Guid Id { get; set; }
        public string OrderDate { get; set; } = string.Empty; // Formatted D/M/Y
        public string OrderNo { get; set; } = string.Empty;
        public string MainSku { get; set; } = string.Empty;
        public string SubSku { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
