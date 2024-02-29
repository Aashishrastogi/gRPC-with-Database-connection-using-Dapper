using Grpc.Core;
using Server;
using Server.Database_Operations;

namespace Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly DatabaseContext _databaseContext;


    public GreeterService(ILogger<GreeterService> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override Task<HelloReply> SayGreetings(HelloRequest request, ServerCallContext context)
    {
        _databaseContext.Query<HelloReply>(
            $"INSERT INTO Greetings" +
            $"(NAME, TIME) " +
            $"VALUES" +
            $" ('{request.Name}','{DateTime.UtcNow.TimeOfDay}')");

        _logger.LogInformation("SayGreetings inserted into database successfully ");
        return Task.FromResult(new HelloReply
        {
            Message = "Greeting from Server " + request.Name
        });
    }
}