using CosmosDBDataModel.Config;
using CosmosDBDataModel.Helpers;
using CosmosDBDataModel.Model;
using CosmosDBDataModel.Seeder;
using Microsoft.Azure.Cosmos;

Console.WriteLine("Hello, World!");

//await OrdersSeeder.SeedData();


CosmosTestRun <LineaPedido> lineapedido = await CosmosHelpers.QueryItems<LineaPedido>(databaseName: "cosmospocindi", containerName: "Pedidos_C", query: new QueryDefinition("SELECT * from c where c.type = \"orderline\""));
CosmosTestRun<Pedido> pedido = await CosmosHelpers.QueryItems<Pedido>(databaseName: "cosmospocindi", containerName: "Pedidos_C", query: new QueryDefinition("SELECT * from c where c.type = \"order\""));

Console.WriteLine("Pedido:");

Console.WriteLine("RU:" + pedido._requestsConsumed);
Console.WriteLine("total items: " + pedido._items.Count());


Console.WriteLine("Linea Pedido:");

Console.WriteLine("RU:" + lineapedido._requestsConsumed);
Console.WriteLine("total items: " + lineapedido._items.Count());

