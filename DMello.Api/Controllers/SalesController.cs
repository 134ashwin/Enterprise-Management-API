namespace DMello.Api.Controllers;

using ExcelDataReader;
using DMello.Application.Sales.DTOs;
using DMello.Domain.Models;
using DMello.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SalesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetSalesOrders()
    {
        var sales = await _context.SalesOrders
            .AsNoTracking()
            .OrderByDescending(s => s.OrderDate)
            .Select(s => new SalesOrderResponseDto
            {
                Id = s.Id,
                OrderDate = s.OrderDate.ToString("dd/MM/yyyy"), // D/M/Y format for UI
                OrderNo = s.OrderNo,
                MainSku = s.MainSku,
                SubSku = s.SubSku,
                Size = s.Size,
                Customer = s.Customer,
                Description = s.Description
            })
            .ToListAsync();

        return Ok(sales);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderDto dto)
    {
        var salesOrder = new SalesOrdersModel
        {
            OrderDate = dto.OrderDate,
            OrderNo = dto.OrderNo,
            MainSku = dto.MainSku,
            SubSku = dto.SubSku,
            Size = dto.Size,
            Customer = dto.Customer,
            Description = dto.Description
        };

        _context.SalesOrders.Add(salesOrder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSalesOrders), new { id = salesOrder.Id }, salesOrder);
    }


    //Why ImportExcel Method exists: Receives the uploaded Excel file(IFormFile),
    //parses the rows into SalesOrder entities, saves them to the database, and returns the newly imported records.
    [HttpPost("import")]
    [Consumes("multipart/form-data")] // Solves Swagger & ASP.NET Core model binding
    public async Task<IActionResult> ImportExcel([FromForm(Name = "file")] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Please upload a valid Excel file." });

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var importedOrders = new List<SalesOrdersModel>();

        using (var stream = file.OpenReadStream())
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true // Uses Row 1 as column headers
                    }
                });

                var table = result.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    var salesOrder = new SalesOrdersModel
                    {
                        //OrderDate = DateTime.TryParse(row["Date_d_m_y"]?.ToString(), out var dt) ? dt : DateTime.UtcNow,
                        //OrderNo = row["Order No"]?.ToString() ?? string.Empty,
                        //MainSku = row["Main SKU"]?.ToString() ?? string.Empty,
                        //SubSku = row["Sub SKU"]?.ToString() ?? string.Empty,
                        //Size = row["Size"]?.ToString() ?? string.Empty,
                        //Customer = row["Customer"]?.ToString() ?? string.Empty,
                        //Description = row["Description"]?.ToString() ?? string.Empty
                        OrderDate = DateTime.TryParse(row[0]?.ToString(), out var dt) ? dt : DateTime.UtcNow, // Column A
                        OrderNo = row[1]?.ToString() ?? string.Empty,                                          // Column B
                        MainSku = row[2]?.ToString() ?? string.Empty,                                          // Column C
                        SubSku = row[3]?.ToString() ?? string.Empty,                                           // Column D
                        Size = row[4]?.ToString() ?? string.Empty,                                             // Column E
                        Customer = row[5]?.ToString() ?? string.Empty,                                         // Column F
                        Description = row[6]?.ToString() ?? string.Empty
                    };

                    if (!string.IsNullOrEmpty(salesOrder.OrderNo))
                    {
                        importedOrders.Add(salesOrder);
                    }
                }
            }
        }

        if (importedOrders.Any())
        {
            await _context.SalesOrders.AddRangeAsync(importedOrders);
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = $"{importedOrders.Count} Sales orders imported successfully." });
    }
}