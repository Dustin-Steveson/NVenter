using Dapper;
using NVenter.Core;
using NVenter.Domain;
using NVenter.Sqlite.Core;
using NVenter.Sqlite.Domain;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Nventer.Sqlite.Example {
    class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("Hello World!");

            var connectionFactory = new ConnectionFactory("Data Source=test.db");


            //var initializer = new DatabaseInitializer(connectionFactory);
            //await initializer.InitializeDatabase();

            var sql = @"
SELECT * 
FROM EVENTS 
WHERE StreamName = @StreamName
ORDER BY StreamPosition";

            await connectionFactory.NewConnection().QueryAsync<int>("select count(*) from event");

            IEventStream<IAggregateRootEventStreamParameters<ShoppingCart>> x = 
                new AggregateRootEventStream<AggregateRootStreamSqlParameters<ShoppingCart>, ShoppingCart>(
                    new ReadForwardEventStreamSettings { Sql = sql },
                    connectionFactory);

            var repository = new AggregateRootRepository<ShoppingCart>(
                x,
                new AggregateRootEventStreamSqlParametersFactory(),
                new EventWriter(connectionFactory),
                new DefaultAggregateRootStreamNameBuilder());

            
        }
    }

    public class ShoppingCart : AggregateRoot {
        public void AddItem(int itemId) {
            AddEvents(new ItemAdded { ItemId = itemId });
        }
    }

    public class ItemAdded : IEvent {
        public int ItemId { get; set; }
    }
}
