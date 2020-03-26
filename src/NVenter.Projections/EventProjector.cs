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
    public class EventProjector
    {
        private readonly IEventStream _eventStream;
        private readonly Func<Type, IEventHandler> _handlerResolver;
        private readonly Func<IEnumerable<IEventHandler>> _allHandlersResolver;
        private readonly IDictionary<Type, Func<EventWrapper, Task>> _processEventDelegateCache;

        public EventProjector(IEventStream eventStream, Func<Type, IEventHandler> handlerResolver, Func<IEnumerable<IEventHandler>> allHandlersResolver)
        {
            _eventStream = eventStream;
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

            await _eventStream.Start(types);
            while (cancellationToken.IsCancellationRequested == false)
            {
                var eventWrappers = await _eventStream.GetEvents();

                foreach (var eventWrapper in eventWrappers)
                {
                    if (_processEventDelegateCache.ContainsKey(eventWrapper.Event.GetType()) == false) continue;

                    await _processEventDelegateCache[eventWrapper.Event.GetType()](eventWrapper);
                }
            }
        }

        private Func<EventWrapper, Task> GetProcessEventDelegate(Type type)
        {
            var methodInfo = GetType()
                .GetMethod(nameof(EventProjector.ProcessEvent), BindingFlags.Instance | BindingFlags.NonPublic)
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
            var handler = _handlerResolver(typeof(IEventHandler<TEvent>)) as IEventHandler<TEvent>;
            await handler.Handle(@event as EventWrapper<TEvent>);
        }
    }
}