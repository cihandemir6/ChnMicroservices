using System;
using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Coupon> GetDisCount(string productName)
    {
        using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductNAme = @ProductName",new {ProductName=productName});
        if (coupon == null)
         return new Coupon { ProductName="No Discount",Amount=0,Description ="No Discount Description"};

        return coupon;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("INSERT INTO coupon(productname, description, amount) VALUES( @ProductName, @Description, @Amount); ",
             new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
        return affected > 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("UPDATE  coupon SET  productname=@ProductName, description=@Description, amount=@Amount WHERE id= @Id;",
        new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });
        return affected > 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("DELETE FROM coupon WHERE  productname=@ProductName",
        new { ProductName = productName});
        return affected > 0;
    }

    

   
}

