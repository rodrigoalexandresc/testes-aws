// See https://aka.ms/new-console-template for more information
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqsConsumer.Options;
using SqsConsumer.Workers;

Console.WriteLine("SQS Consumer Start");

// var quereUrl = "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/minha-fila";
// var credentials = new Amazon.Runtime.BasicAWSCredentials("ACCESS_KEY", "teste1");
// var serviceUrl = "http://localhost:4566";

// var config = new AmazonSQSConfig
// {
//     ServiceURL = serviceUrl,
//     UseHttp = true
// };

// using var sqsClient = new AmazonSQSClient(credentials, config);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddOptions<QueueOptions>()
            .Bind(context.Configuration.GetSection("Queue"))
            .Validate(o => !string.IsNullOrEmpty(o.QueueUrl), "QueueUrl obrigatória")
            .ValidateOnStart();
        services.AddAWSService<Amazon.SQS.IAmazonSQS>();
        services.AddHostedService<SqsConsumeWorker>();
    })
    .Build();   

await host.RunAsync();


