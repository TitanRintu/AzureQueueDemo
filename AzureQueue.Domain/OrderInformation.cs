using System;

namespace AzureQueue.Domain
{
    public class OrderInformation
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
        public int OrderQuantity { get; set; }
    }
}
