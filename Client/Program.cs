using Grpc.Net.Client;
using Server;

namespace CRUD_gRPC;

public class Program
{
    public static void Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7157");
        var client = new Greeter.GreeterClient(channel);


        Console.WriteLine("******  Starting of the Client Application  ******");

        Console.WriteLine(" Enter the name and press enter to greet the server listening ");

        var name = Console.ReadLine();

        var response = client.SayGreetings(new HelloRequest { Name = name });

        Console.WriteLine(response.Message);
    }
}