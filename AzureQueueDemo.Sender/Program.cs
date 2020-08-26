using AzureQueue.Domain;

using Microsoft.Azure.ServiceBus;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureQueueDemo.Sender
{
    class Program
    {

        static List<OrderInformation> Orders = new List<OrderInformation>()
        {
            new OrderInformation()
            {
                OrderId = Guid.NewGuid(),
                OrderName="Dell Laptop",
                OrderQuantity=10

            },
             new OrderInformation()
            {
                OrderId = Guid.NewGuid(),
                OrderName="Apple Laptop",
                OrderQuantity=10

            },
              new OrderInformation()
            {
                OrderId = Guid.NewGuid(),
                OrderName="Lenovo Laptop",
                OrderQuantity=10

            },
               new OrderInformation()
            {
                OrderId = Guid.NewGuid(),
                OrderName="MI Laptop",
                OrderQuantity=10

            } 

        };
        private static string AZURE_SERVICE_BUS_CONNECTIONSTRING = "Endpoint=sb://ordermanagementnamespace.servicebus.windows.net/;SharedAccessKeyName=SenderPolicy;SharedAccessKey=z6kKhOgRoBuNxoNvrdwrzjEwqB7LdrQh/9KSKIAgqj8=;";
        private static string QUEUE_NAME = "ordermanagementqueue";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Do you want to send Order Information? If Yes , Press Y.");
            var result = Console.ReadLine();
            if (result.Equals("Y"))
            {
                IQueueClient client = new QueueClient(AZURE_SERVICE_BUS_CONNECTIONSTRING, QUEUE_NAME);
                foreach (var item in Orders)
                {
                    var messageBody = JsonConvert.SerializeObject(item);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    await client.SendAsync(message);
                    Console.WriteLine($"Sending Message : {item.OrderName.ToString()} ");

                }
            }
            Console.Read();
        }
    }
}
