using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SqsConsumer.Options;

namespace SqsConsumer.Workers;

public class SqsConsumeWorker : BackgroundService
{

    private readonly IAmazonSQS _amazonSQS;
    private readonly QueueOptions _awsOptions;

    public SqsConsumeWorker(IAmazonSQS amazonSQS, IOptions<QueueOptions> awsOptions)
    {
        _amazonSQS = amazonSQS;
        _awsOptions = awsOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Read SQS queue...");

        while (!stoppingToken.IsCancellationRequested)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = _awsOptions.QueueUrl,
                MaxNumberOfMessages = 5,
                WaitTimeSeconds = 10
            };

            var response = await _amazonSQS.ReceiveMessageAsync(request);

            if (response.Messages == null) continue;

            foreach (var message in response.Messages)
            {
                Console.WriteLine($"Message received: {message.Body}");

                await _amazonSQS.DeleteMessageAsync(_awsOptions.QueueUrl, message.ReceiptHandle);

                Console.WriteLine("Message has been read and deleted");
            }
        }
    }
}