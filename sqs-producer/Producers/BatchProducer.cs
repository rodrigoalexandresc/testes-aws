using Amazon.SQS;
using Amazon.SQS.Model;

namespace SqsProducer.Producers;

public class BatchProducer : AbstractProducer
{
    private static BatchProducer? _instance;
    
    public static BatchProducer Instance()
    {
        if (_instance == null)
        {
            _instance = new BatchProducer();
        }
        
        return _instance;
    }

    protected override async Task SendMessageMethod(AmazonSQSClient sqsClient, string[] messages)
    {
        var messageRequest = new SendMessageBatchRequest
        {
            QueueUrl = queueUrl,
            Entries = messages.Select((message, idx) => new SendMessageBatchRequestEntry
            {
                Id = idx.ToString(),
                MessageBody = message,
                DelaySeconds = 0
            }).ToList() 
        };

        try
        {
            var response = await sqsClient.SendMessageBatchAsync(messageRequest);

            Console.WriteLine($"{response.ContentLength} bytes em mensagens enviadas com sucesso!");
            Console.WriteLine($"HTTP Status: {response.HttpStatusCode}");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Erro ao enviar mensagem");
            Console.WriteLine(ex.Message);
            
        }
    }
}