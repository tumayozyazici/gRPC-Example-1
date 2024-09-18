using Grpc.Core;
using Grpc.Net.Client;
using grpcMessageClient;
using grpcServer;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5035");
        var client = new Message.MessageClient(channel);


        //Unary
        //MessageResponse result = await client.SendMessageAsync(new MessageRequest
        //{
        //    Message = "gRPC öğreniyorum.",
        //    Name = "Tümay"
        //});


        //Serverstreaming
        //var result = client.SendMessage(new MessageRequest
        //{
        //    Message = "gRPC öğreniyorum.",
        //    Name = "Tümay"
        //});

        //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        //while (await result.ResponseStream.MoveNext(cancellationTokenSource.Token))
        //{
        //    Console.WriteLine(result.ResponseStream.Current.Message);
        //}


        //ClientStreaming
        //var result = client.SendMessage();
        //for (int i = 0; i < 10; i++)
        //{
        //    await Task.Delay(1000);
        //    await result.RequestStream.WriteAsync(new MessageRequest
        //    {
        //        Message = $"{i}. Merhaba",
        //        Name = "Tümay"
        //    });
        //}

        ////Stream datanın sonlandığını ifade eder. çünkü dönme mikarı belli ama benim gönderdiklerim acaba bitti mi kod bunu bilmiyor. o yüzden belirtmem lazım...
        //await result.RequestStream.CompleteAsync();
        //Console.WriteLine((await result.ResponseAsync).Message);


        //Bi-directional
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var result = client.SendMessage();
        var task = Task.Run(async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                await result.RequestStream.WriteAsync(new MessageRequest
                {
                    Message = "Selam" + i,
                    Name = "Tümay"
                });
            }
        });

        while (await result.ResponseStream.MoveNext(cancellationTokenSource.Token))
        {
            Console.WriteLine(result.ResponseStream.Current.Message);
        }

        await task;
        await result.RequestStream.CompleteAsync();


        //var client = new Greeter.GreeterClient(channel);

        //HelloReply result =await client.SayHelloAsync(new HelloRequest
        //{
        //    Name = "Tümay"
        //});

        //Console.WriteLine(result.Message);
    }
}