using System;
using Npgsql;

namespace Discount.Grpc.Extensions;

public class DiscountDbCreator : IHostedService
{
    private readonly IConfiguration configuration;
    private readonly ILogger<DiscountDbCreator> logger;

    public DiscountDbCreator(IConfiguration configuration, ILogger<DiscountDbCreator> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        bool retry = false;
        int retrycount = 0;
        do
        {
            Thread.Sleep(2000);
            try
            {
                logger.LogInformation("Migrating postgresql database.");
                using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();
                using var command = new NpgsqlCommand { Connection = connection };
                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE Coupon(
                                                            Id SERIAL PRIMARY KEY,
                                                            ProductName VARCHAR(24) NOT NULL,
                                                            Description TEXT,
                                                            Amount INT
                                                           )";
                command.ExecuteNonQuery();

                command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) VALUES('IPhone X2','IPhone Discount',150)";
                command.ExecuteNonQuery();
                command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) VALUES('Samsung 102','Samsung Discount',100)";
                command.ExecuteNonQuery();
                logger.LogInformation("Migrated postgresql database.");
                retry = false;
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occured while migrating the postgresql database.");
                retry = true;
                retrycount++;
            }

        } while (retry && retrycount < 50);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

