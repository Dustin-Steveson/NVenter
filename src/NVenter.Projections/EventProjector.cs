using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NVenter.Projections
{
    public class EventProjector<TEventStreamConfiguration>
    {
        private readonly string _streamName;
        private readonly IEventStream<TEventStreamConfiguration> _eventStream;
        private readonly TEventStreamConfiguration _configuration;
        private readonly IGetProjectionPosition _projectionPositionGetter;
        private readonly Func<Type, IHandleMessages> _handlerResolver;
        private readonly Func<IEnumerable<IHandleMessages>> _allHandlersResolver;
        private readonly IDictionary<Type, Func<EventWrapper, Task>> _processEventDelegateCache;

        public EventProjector(
            string streamName,
            IEventStream<TEventStreamConfiguration> eventStream,
            TEventStreamConfiguration configuration,
            IGetProjectionPosition projectionPositionGetter,
            Func<Type, IHandleMessages> handlerResolver,
            Func<IEnumerable<IHandleMessages>> allHandlersResolver)
        {
            _streamName = streamName;
            _eventStream = eventStream;
            _configuration = configuration;
            _projectionPositionGetter = projectionPositionGetter;
            _handlerResolver = handlerResolver;
            _allHandlersResolver = allHandlersResolver;
            _processEventDelegateCache = new Dictionary<Type, Func<EventWrapper, Task>>();
        }

        public async Task Start(CancellationToken cancellationToken)
        {

            var handlers = _allHandlersResolver();
            var types = handlers.Select(h => h.GetType().GenericTypeArguments.Single());

            foreach(var type in types)
            {
                _processEventDelegateCache.Add(type, GetProcessEventDelegate(type));
            }

            var position = _projectionPositionGetter.GetProjectionPosition();

            while (cancellationToken.IsCancellationRequested == false)
            {
                var eventStreamSlice = await _eventStream.GetEvents(_configuration, position);

                foreach (var eventWrapper in eventStreamSlice.Events)
                {
                    if (_processEventDelegateCache.ContainsKey(eventWrapper.Event.GetType()) == false) continue;

                    await _processEventDelegateCache[eventWrapper.Event.GetType()](eventWrapper);
                }

                position = eventStreamSlice.LastPosition;
            }
        }

        private Func<EventWrapper, Task> GetProcessEventDelegate(Type type)
        {
            var methodInfo = GetType()
                .GetMethod(nameof(EventProjector<object>.ProcessEvent), BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(type);

            var instanceParameter = Expression.Constant(this);
            var argumentsParameter = Expression.Parameter(typeof(object), "arguments");

            var eventType = typeof(EventWrapper<>).MakeGenericType(type);

            var call = Expression.Call(
                instanceParameter,
                methodInfo,
                Expression.Convert(argumentsParameter, eventType));

            return Expression.Lambda<Func<EventWrapper, Task>>(
                    call,
                    argumentsParameter).Compile();
        }

        private async Task ProcessEvent<TEvent>(EventWrapper @event) where TEvent : IEvent
        {
            var handler = _handlerResolver(typeof(IHandleMessages<TEvent>)) as IHandleMessages<TEvent>;
            var eventWrapper = @event as EventWrapper<TEvent>;
            await handler.Handle(eventWrapper.Event, new MessageContext(eventWrapper.Metadata.Id, eventWrapper.Metadata.CausationId, eventWrapper.Metadata.CorrelationId, eventWrapper.Metadata.Created));
        }
    }
}