using NVenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NVenter.Domain
{
    public class PubliclyExposedEventMethodException : Exception
    {
        public PubliclyExposedEventMethodException(Type aggregateType, MethodInfo method, ParameterInfo eventParameter)
            : base($"Aggregate of type {aggregateType.Name} method {method.Name} taking event {eventParameter.Name} should not be public")
        { }
    }

    internal class AggregateDelegateFactory : IMakeAggregateDelegates
    {
        private Func<MethodInfo, bool> MethodIsEventHandler =
            method => method.GetParameters().Length == 1 &&
                      typeof(IEvent).IsAssignableFrom(method.GetParameters().First().ParameterType);

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<Type, EventMethod>> GetEventApplicationMethods(params Type[] types)
        {
            var delegates = new Dictionary<Type, IReadOnlyDictionary<Type, EventMethod>>();
            var assemblyAggregateTypes = types
                .Where(type => type.IsAssignableTo(typeof(AggregateRoot)) && type.IsAbstract == false);

            EnsureNoPubliclyExposedEventMethods(assemblyAggregateTypes);

            foreach (var aggregateType in assemblyAggregateTypes)
            {
                var eventDelegates = GetEventApplicationMethods(aggregateType);
                delegates.Add(aggregateType, eventDelegates);
            }

            return delegates;
        }

        private void EnsureNoPubliclyExposedEventMethods(IEnumerable<Type> assemblyAggregateTypes)
        {
            var exceptions = assemblyAggregateTypes
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(MethodIsEventHandler)
                .Select(method => new PubliclyExposedEventMethodException(method.DeclaringType, method, method.GetParameters().Single()));

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions.ToArray());
            }
        }

        private IReadOnlyDictionary<Type, EventMethod> GetEventApplicationMethods(Type aggregateType)
        {
            return aggregateType
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(MethodIsEventHandler)
                .Where(method => !method.IsGenericMethod)
                .Select(MakeCompiledEventMethod)
                .ToDictionary(tuple => tuple.EventType, tuple => tuple.EventMethod);
        }

        private static (EventMethod EventMethod, Type EventType) MakeCompiledEventMethod(MethodInfo methodInfo)
        {
            var instanceParameter = Expression.Parameter(typeof(AggregateRoot), "target");
            var argumentsParameter = Expression.Parameter(typeof(IEvent), "arguments");

            var eventType = methodInfo.GetParameters().First().ParameterType;

            var call = Expression.Call(
                Expression.Convert(instanceParameter, methodInfo.DeclaringType),
                methodInfo,
                Expression.Convert(argumentsParameter, eventType));

            var lambda = Expression.Lambda<EventMethod>(
                call,
                instanceParameter,
                argumentsParameter);

            return (lambda.Compile(), eventType);
        }
    }
}