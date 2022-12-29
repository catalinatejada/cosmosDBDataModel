using Bogus;
using CosmosDBDataModel.Helpers;
using CosmosDBDataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBDataModel.Seeder
{
    public static class OrdersSeeder
    {

        public static int Batchsize => 50;
        public static int SeedRounds => 10;
        public static async Task SeedData()
        {
            int i = 0;
            Faker<LineaPedido> FakerOrdersLinesDataGenerator = new Faker<LineaPedido>()
             .RuleFor(p => p.id, f => Guid.NewGuid().ToString())
             .RuleFor(p => p.IdPedido, f => { i++; return i; })
             .RuleFor(p => p.IdLineaPedido, f => { i++; return i; })
             .RuleFor(p => p.Unidades, f => f.PickRandom<int>(0, 1, 5, 6, 7, 9, 11))
             .RuleFor(p => p.UsuarioAlta, f => f.Person.UserName)
             .RuleFor(p => p.FechaAlta, f => f.Date.Between(DateTime.Now.AddYears(-15), DateTime.Now.AddMonths(-3)))
             .RuleFor(p => p.UsuarioModificacion, f => f.Person.UserName)
             .RuleFor(p => p.FechaModificacion, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
             .RuleFor(p => p.CodOla, f => f.Random.Number(1, 500))
             .RuleFor(p => p.CodOrdenTrabajo, f => f.Random.Number(1, int.MaxValue));

            Faker<Pedido> FakeOrderDataGenerator = new Faker<Pedido>()
            .RuleFor(p => p.id, f => Guid.NewGuid().ToString())
            .RuleFor(p => p.CodigoPedido, f => f.PickRandom<string>("ref", "pren", "rol") + new Random().Next(0, 8000))
            .RuleFor(p => p.CodigoSistemaOrigen, f => f.PickRandom<string>("SGA", "EWMS", "XWMS"))
            .RuleFor(p => p.CodigoReserva, f => f.PickRandom<string>("res", "nonres") + new Random().Next(0, 8000))
            .RuleFor(p => p.IdEstadoPedido, f => f.PickRandom<short>(0, 1, 2, 3, 4, 5))
            .RuleFor(p => p.IdCentroDistribucion, f => f.PickRandom<int>(100, 300, 410, 900, 905, 908))
            .RuleFor(p => p.IdCampana, f => f.PickRandom<int>(100, 300, 410, 900, 905, 908))
            .RuleFor(p => p.IdTipoMovimiento, f => f.PickRandom<int>(100, 300, 410, 900, 905, 908))
            .RuleFor(p => p.IdSubtipoMovimiento, f => f.PickRandom<int>(100, 300, 410, 900, 905, 908))
            .RuleFor(p => p.IdTemporada, f => f.PickRandom<short>(0, 1, 2, 3, 4, 5))
            .RuleFor(p => p.UsuarioAlta, f => f.Person.UserName)
            .RuleFor(p => p.FechaAlta, f => f.Date.Between(DateTime.Now.AddYears(-15), DateTime.Now.AddMonths(-3)))
            .RuleFor(p => p.FechaHoraCreacion, f => f.Date.Between(DateTime.Now.AddYears(-15), DateTime.Now.AddMonths(-3)))
            .RuleFor(p => p.UsuarioModificacion, f => f.Person.UserName)
            .RuleFor(p => p.FechaModificacion, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now));

            await CosmosHelpers.CreateContainer("cosmospocindi", "Pedidos_C", "/partitionKey");

            for (var x = 0; x <= SeedRounds; x++)
            {
                var myData = FakeOrderDataGenerator.Generate(Batchsize).ToList();
                await CosmosHelpers.InsertTransacctionalBatch<Pedido>("cosmospocindi", "Pedidos_C", myData);

                Console.Write("Inserted Pedido");

                var myDataLineaPedido = FakerOrdersLinesDataGenerator.Generate(Batchsize).ToList();
                await CosmosHelpers.InsertTransacctionalBatch<LineaPedido>("cosmospocindi", "Pedidos_C", myDataLineaPedido);

                Console.Write("Inserted LineaPedido");

            }
        }
    }
}
