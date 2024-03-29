﻿using System;

namespace EventSourcing.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var warehouseProductRepository = new WarehouseProductRepository();
            var projection = new Projection(new ProductDbContext());
            var key = string.Empty;
            while (key != "X")
            {
                Console.WriteLine("R: Receive Inventory");
                Console.WriteLine("S: Ship Inventory");
                Console.WriteLine("A: Inventory Adjustment");
                Console.WriteLine("Q: Quantity On Hand");
                Console.WriteLine("E: Events");
                Console.WriteLine("P: Projection");
                Console.Write("> ");
                key = Console.ReadLine()?.ToUpperInvariant();
                Console.WriteLine();

                var sku = GetSkuFromConsole(); //단순 sku 

                var warehouseProduct = warehouseProductRepository.Get(sku);
                switch (key)
                {
                    case "R":
                        var receiveInput = GetQuantity();
                        if (receiveInput.IsValid)
                        {
                            warehouseProduct.ReceiveProduct(receiveInput.Quantity);
                            Console.WriteLine($"{sku} Received: {receiveInput.Quantity}");
                        }
                        break;
                    case "S":
                        var shipInput = GetQuantity();
                        if (shipInput.IsValid)
                        {
                            warehouseProduct.ShipProduct(shipInput.Quantity);
                            Console.WriteLine($"{sku} Shipped: {shipInput.Quantity}");
                        }
                        break;
                    case "A":
                        var adjustmentInput = GetQuantity();
                        if (adjustmentInput.IsValid)
                        {
                            var reason = GetAdjustmentReason();
                            warehouseProduct.AdjustInventory(adjustmentInput.Quantity, reason);
                            Console.WriteLine($"{sku} Adjusted: {adjustmentInput.Quantity} {reason}");
                        }
                        break;
                    case "Q":
                        var currentQuantityOnHand = warehouseProduct.GetQuantityOnHand();
                        Console.WriteLine($"{sku} Quantity On Hand: {currentQuantityOnHand}");
                        break;
                    case "E":
                        Console.WriteLine($"Events: {sku}");
                        foreach (var evnt in warehouseProduct.GetAllEvents())
                        {
                            switch (evnt)
                            {
                                case ProductShipped shipProduct:
                                    Console.WriteLine($"{shipProduct.DateTime:u} {sku} Shipped: {shipProduct.Quantity}");
                                    break;
                                case ProductReceived receiveProduct:
                                    Console.WriteLine($"{receiveProduct.DateTime:u} {sku} Received: {receiveProduct.Quantity}");
                                    break;
                                case InventoryAdjusted inventoryAdjusted:
                                    Console.WriteLine($"{inventoryAdjusted.DateTime:u} {sku} Adjusted: {inventoryAdjusted.Quantity} {inventoryAdjusted.Reason}");
                                    break;
                            } 
                        }
                        break;
                    case "P":
                        Console.WriteLine($"Projection: {sku}");
                        var productProjection = projection.GetProduct(sku);
                        Console.WriteLine($"{sku} Received: {productProjection.Received}");
                        Console.WriteLine($"{sku} Shipped: {productProjection.Shipped}");
                        break;
                }
                warehouseProductRepository.Save(warehouseProduct);
                Console.ReadLine();
                Console.WriteLine();

            }
        }

        private static (int Ccc,string Ddd) GetDouble(string ss)
        {

            return (ss.Length, ss+"[]");
        }

        private static string GetSkuFromConsole()
        {
            Console.Write("SKU: ");
            return Console.ReadLine();
        }

        private static string GetAdjustmentReason()
        {
            Console.Write("Reason: ");
            return Console.ReadLine();
        }

        /// <summary>
        /// 수량입력
        /// </summary>
        /// <returns></returns>
        private static (int Quantity, bool IsValid) GetQuantity()
        {
            Console.Write("Quantity: ");
            if (int.TryParse(Console.ReadLine(), out var quantity))
            {
                return (quantity, true);
            }
            else
            {
                Console.WriteLine("Invalid Quantity.");
                return (0, false);
            }
        }


    }
}
