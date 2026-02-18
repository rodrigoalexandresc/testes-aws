// See https://aka.ms/new-console-template for more information
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsProducer.Producers;

Console.WriteLine("Hello, World!");

var mode = args[0];
var messageOrFile = args[1];

if (mode == "TEXT")
{
    await SimpleProducer.Instance().SendMessage([messageOrFile]);
}
else if (mode == "FILE")
{
    var lines = (await File.ReadAllTextAsync(messageOrFile)).Split(Environment.NewLine);
    foreach (var chunk in lines.Chunk(10))
    {
        await BatchProducer.Instance().SendMessage(chunk);    
    } 
}
else
{
    Console.WriteLine("Opção inválida. Usar [TEXT|FILE] [Message|FilePath]");
}
