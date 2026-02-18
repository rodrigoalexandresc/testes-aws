using Amazon.SQS;
using Amazon.SQS.Model;

namespace SqsProducer.Producers;

public class SimpleProducer : AbstractProducer
{
    private static SimpleProducer? _instance;
    
    public static SimpleProducer Instance()
    {
        if (_instance == null)
        {
            _instance = new SimpleProducer();
        }
        
        return _instance;
    }

    protected override async Task SendMessageMethod(AmazonSQSClient sqsClient, string[] messages)
    {
        var messageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = messages[0],
            DelaySeconds = 0
        };

        try
        {
            var response = await sqsClient.SendMessageAsync(messageRequest);

            Console.WriteLine($"Mensagem: {messages[0]} enviada com sucesso!");
            Console.WriteLine($"MessageId: {response.MessageId}");
            Console.WriteLine($"HTTP Status: {response.HttpStatusCode}");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Erro ao enviar mensagem");
            Console.WriteLine(ex.Message);
            
        }
    }
}