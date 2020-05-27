using Dapper;
using Newtonsoft.Json.Bson;
using NVenter.Core;
using NVenter.Domain;
using NVenter.Sqlite.Core;
using NVenter.Sqlite.Domain;
using System;
using System.Collections.Generic;
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
FROM event 
WHERE StreamName = @StreamName
ORDER BY StreamPosition";

            await connectionFactory.NewConnection().QueryAsync<int>("select count(*) from event");
            var repository = new AggregateRootRepository<IDictionary<string, object>, ShoppingCart>(
                new AggregateRootEventStream<ShoppingCart>(
                    new ReadForwardEventStreamSettings { Sql = sql },
                    connectionFactory),
                new AggregateRootEventStreamSqlParametersFactory<ShoppingCart>(),
                new EventWriter(connectionFactory),
                new DefaultAggregateRootStreamNameBuilder());

            var addItemStateRepo = new AgggregateRootMessageStateRepository<AddItem, ShoppingCart>(repository);
            var createCartStateRepo = new AgggregateRootMessageStateRepository<CreateCart, ShoppingCart>(repository);

            var addItemOrchestrator = new StatefulMessageHandlerOrchestrator<AddItem, ShoppingCart>(addItemStateRepo, new AddItemCommandHandler());
            var createCartOrchestrator = new StatefulMessageHandlerOrchestrator<CreateCart, ShoppingCart>(createCartStateRepo, new CreateCartCommandHandler());

            var commandId = Guid.NewGuid();

            await createCartOrchestrator.Handle(
                new CreateCart {
                    AggregateId = Guid.NewGuid() 
                }, 
                new MessageContext(commandId, commandId, commandId, DateTimeOffset.UtcNow));
        }
    }

    public class ShoppingCart : AggregateRoot {
        public void AddItem(int itemId) {
            AddEvents(new ItemAdded { ItemId = itemId });
        }

        public void CreateCart(Guid cartId) {
            AddEvents(new CartCreated { CartId = cartId });
        }
    }

    public class CreateCart : IAggregateCreationCommand {
        public Guid AggregateId { get; set; }
    }

    public class AddItem : ICommand {
        public Guid AggregateId { get; set; }
        public int ItemId { get; set; }
    }

    public class ItemAdded : IEvent {
        public Guid CartId { get; set; }
        public int ItemId { get; set; }
    }

    public class CartCreated : IEvent {
        public Guid CartId { get; set; }
    }

    public class AddItemCommandHandler : IStatefulMessageHandler<AddItem, ShoppingCart> {
        public Task<ShoppingCart> Handle(AddItem message, ShoppingCart state, MessageContext context) {

            state.AddItem(message.ItemId);

            return Task.FromResult(state);
        }
    }

    public class CreateCartCommandHandler : IStatefulMessageHandler<CreateCart, ShoppingCart> {
        public Task<ShoppingCart> Handle(CreateCart message, ShoppingCart state, MessageContext context) {

            return Task.FromResult(state);
        }
    }
}
