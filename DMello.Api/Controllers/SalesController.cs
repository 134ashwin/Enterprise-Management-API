namespace DMello.Api.Controllers;

using DMello.Application.Sales.DTOs;
using DMello.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMello.Domain.Models;

[Authorize]
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
}