using CarSaleApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarSaleApi.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController(IDbConnection connection): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        const string query = "SELECT Id, Name FROM Brand";

        var brands = await connection.QueryAsync<Brand>(query);

        return Ok(brands);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        const string query = "SELECT Id, Name FROM Brand where Id = @id";

        var brand = await connection.QueryFirstOrDefaultAsync<Brand>(query, new { id });

        if (brand is null)
            return NotFound();

        return Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult> Post(Brand brand)
    {
        const string command = @"
            INSERT INTO Brand (Name) 
            OUTPUT INSERTED.Id
            VALUES (@Name)";

        var id = await connection.ExecuteScalarAsync<int>(command, brand);

        return Created($"/api/brands/{id}", new { id });
    }
}
