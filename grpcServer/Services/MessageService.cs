using Grpc.Core;
using grpcMessageServer;
using grpcServer;

namespace grpcServer.Services;

public class MessageService : Message.MessageBase
{
    private readonly ILogger<MessageService> _logger;
    public MessageService(ILogger<MessageService> logger)
    {
        _logger = logger;
    }


    //Bi-directional
    public override async Task SendMessage(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
    {
        var task = Task.Run(async () =>
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                Console.WriteLine($"Message: {requestStream.Current.Message} | Name: {requestStream.Current.Name}");
            }
        });

        for (int i = 0; i < 10; i++) 
        {
            await Task.Delay(1000);
            await responseStream.WriteAsync(new MessageResponse
            {
                Message = "Mesaj" + i
            });
        }
        await task;
    }



    //Clientstreaming
    //public override async Task<MessageResponse> SendMessage(IAsyncStreamReader<MessageRequest> requestStream, ServerCallContext context)
    //{
    //    while(await requestStream.MoveNext(context.CancellationToken))
    //    {
    //        Console.WriteLine($"Message: {requestStream.Current.Message} | Name: {requestStream.Current.Name}");
    //    }

    //    return new MessageResponse
    //    {
    //        Message = "Veri Alınmıştır."
    //    };
    //}



    //Serverstreaming
    //public override async Task SendMessage(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
    //{
    //    Console.WriteLine($"Message: {request.Message} | Name: {request.Name}");
    //    for (int i= 0; i< 10; i++)
    //    {
    //        await Task.Delay(200);
    //        await responseStream.WriteAsync(new MessageResponse
    //        {
    //            Message = $"{i}"
    //        });
    //    }
    //}
}
