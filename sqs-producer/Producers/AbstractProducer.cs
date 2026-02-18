using Amazon;
using Amazon.SQS;

namespace SqsProducer.Producers;

public abstract class AbstractProducer
{
    protected string queueUrl = "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/minha-fila";
    protected abstract Task SendMessageMethod(AmazonSQSClient sqsClient, string[] messages); 

    public async Task SendMessage(string[] messages)
    {
        var region = RegionEndpoint.USEast1;
        var credentials = new Amazon.Runtime.BasicAWSCredentials("ACCESS_KEY", "123456");
        var serviceUrl = "http://localhost:4566";

        var config = new AmazonSQSConfig
        {
            ServiceURL = serviceUrl,
            UseHttp = true
        };

        using var sqsClient = new AmazonSQSClient(credentials, config);

        await SendMessageMethod(sqsClient, messages);        
    }
}