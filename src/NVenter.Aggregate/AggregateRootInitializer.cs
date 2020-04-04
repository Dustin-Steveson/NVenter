using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NVenter.Domain
{
    public static class AggregateRootInitializer
    {
        public static void Initialize(params Assembly[] assemblies)
        {
            Func<MethodInfo, bool> isEventApplier = (mi) =>
                mi.GetParameters().Count() == 1 &&
                typeof(IEvent).IsAssignableFrom(mi.GetParameters().Single().ParameterType);

            AggregateRoot.EventMethods = assemblies.SelectMany(ass => ass.GetTypes())
                .Where(t => typeof(AggregateRoot).IsAssignableFrom(t))
                .ToDictionary(t => t, t => GetEventApplicationMethods(t, isEventApplier));
        }


        internal delegate void DynamicMethodDelegate(object target, IEvent argument);

        private static IDictionary<Type, DynamicMethodDelegate> GetEventApplicationMethods(Type aggregateType, Func<MethodInfo, bool> eligibleMethods)
        {
            return aggregateType
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(mi => mi.DeclaringType != typeof(AggregateRoot))
                .Where(eligibleMethods)
                .Select(mi =>
                {
                    var instanceParameter = Expression.Parameter(typeof(object), "target");
                    var argumentsParameter = Expression.Parameter(typeof(IEvent), "arguments");

                    var eventType = mi.GetParameters().First().ParameterType;

                    var call = Expression.Call(
                    Expression.Convert(instanceParameter, aggregateType),
                    mi,
                    Expression.Convert(argumentsParameter, eventType));

                    var lambda = Expression.Lambda<DynamicMethodDelegate>(
                        call,
                        instanceParameter,
                        argumentsParameter);

                    return (EventType: eventType, Lambda: lambda.Compile());
                })
                .ToDictionary(x => x.EventType, x => x.Lambda);
        }
    }
}