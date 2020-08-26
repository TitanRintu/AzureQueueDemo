using AzureQueue.Domain;

using Microsoft.Azure.ServiceBus;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureQueueDemo.Receiver
{

    class Program
    {
        private static string AZURE_SERVICE_BUS_CONNECTIONSTRING = "Endpoint=sb://ordermanagementnamespace.servicebus.windows.net/;SharedAccessKeyName=ReceiverPolicy;SharedAccessKey=HkM/MFoiQsshdq/l2iaxMETa6X09Ddwrlry0BD0vacI=;";
        private static string QUEUE_NAME = "ordermanagementqueue";
        private static IQueueClient client;
        static async Task Main(string[] args)
        {
            await ReceiveMessagesAsync();
        }

        private static  async Task ReceiveMessagesAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                client = new QueueClient(AZURE_SERVICE_BUS_CONNECTIONSTRING, QUEUE_NAME);
                var options = new MessageHandlerOptions(ExceptionMethod)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                client.RegisterMessageHandler(ExecuteMessageProcessing, options);
            });
            Console.Read();

        }

        private static async Task ExecuteMessageProcessing(Message message, CancellationToken arg2)
        {
            var result = JsonConvert.DeserializeObject<OrderInformation>(Encoding.UTF8.GetString(message.Body));
            Console.WriteLine($"Order Id is {result.OrderId}, Order name is {result.OrderName} and quantity is {result.OrderQuantity}");
            await client.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static async Task ExceptionMethod(ExceptionReceivedEventArgs arg)
        {
            await Task.Run(() =>
           Console.WriteLine($"Error occured. Error is {arg.Exception.Message}")
           );
        }
    }
}
