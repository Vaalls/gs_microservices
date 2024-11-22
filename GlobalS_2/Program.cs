using GlobalS_2.Repository;
using GlobalS_2.Repository.EnergyConsumptionService.Repositories;
using MongoDB.Driver;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Configura��o do Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configura��o da conex�o com o MongoDB
        builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            // Recupera a string de conex�o do MongoDB a partir de appsettings.json
            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDbConnection");
            return new MongoClient(mongoConnectionString);
        });

        builder.Services.AddScoped(serviceProvider =>
        {
            var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
            return mongoClient.GetDatabase("EnergyConsumptionDb"); // Nome do banco de dados
        });

        // Inje��o de depend�ncia do reposit�rio
        builder.Services.AddScoped<IConsumoRepository, ConsumoRepository>();

        // Adiciona o Swagger
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}