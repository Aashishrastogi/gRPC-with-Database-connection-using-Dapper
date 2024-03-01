using Server.Services;
using Serilog;
using Server.Database_Operations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json"
        , true
        , true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();


builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json", false);

// Database Configuration
/*builder.Services.AddSingleton<DbConnectionStringBuilder>(provider =>
{
    var dbStringconfiguration = provider.GetRequiredService<IConfiguration>();
    var connectionStringBuilder = new SqlConnectionStringBuilder
    {
        ConnectionString = dbStringconfiguration.GetConnectionString("ImportantDatabase")
       // Password = dbStringconfiguration.GetSection("SecurePasswords").GetValue<string>("ImportantDatabasePassword")
    };
    return connectionStringBuilder;
});*/

// Database Connection
/*builder.Services.AddTransient<IDbConnection>(provider =>
{
    var builder2 = provider.GetRequiredService<DbConnectionStringBuilder>();
   // var providerFactory = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");
   if (!DbProviderFactories.TryGetFactory("Microsoft.Data.SqlClient", out var providerFactory))
   {
      
       throw new InvalidOperationException("Microsoft.Data.SqlClient provider factory is not registered.");
   }
    var conn = providerFactory.CreateConnection();
    conn.ConnectionString = builder2.ConnectionString;
    return conn;
});*/

builder.Services.AddSingleton<DatabaseContext>();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();

var databaseServices = app.Services.GetRequiredService<DatabaseContext>();

var status = databaseServices.Preprocessing_Database(builder.Configuration.GetValue<int>("DatabaseOperations"));

if (status != true) throw new Exception("Database Cleanup not successful");
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();