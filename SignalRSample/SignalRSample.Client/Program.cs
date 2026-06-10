using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SignalRSample.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.development.json", true)
                .Build();

            var logger = LoggingExtensions.CreateLogger(config).WithContext<Program>();
            var user = $"User{new Random().Next(10)}";

            try
            {
                logger.Information("Starting client with User {User}", user);

                var connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:4321/my-hub")
                    // .WithAutomaticReconnect()
                    .Build();

                connection.Closed += async (error) =>
                {
                    logger.Error("Connection to hub closed. Attempting to reconnect. Error: {Error}", error);
                    await Task.Delay(1000);
                    await connection.StartAsync();
                };

                await connection.StartAsync();

                logger.Information("Connection started.");

                connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    logger.Information("Received message from {User}: {Message}", user, message);
                });

                while (true)
                {
                    var message = Console.ReadLine();
                    await connection.InvokeAsync("SendMessage", user, message);
                    logger.Information("Sent message: {Message}", message);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred in client.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
