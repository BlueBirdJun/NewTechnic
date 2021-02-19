using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Demo
{
    public interface IEvent { }

    public record ProductShipped(string Sku, int Quantity, DateTime DateTime) : IEvent;

    public record ProductReceived(string Sku, int Quantity, DateTime DateTime) : IEvent;

    public record InventoryAdjusted(string Sku, int Quantity, string Reason, DateTime DateTime) : IEvent;
}
