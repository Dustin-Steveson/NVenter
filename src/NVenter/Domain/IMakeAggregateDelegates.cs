using System;
using System.Collections.Generic;

namespace NVenter.Domain
{
    public interface IMakeAggregateDelegates
    {
        IReadOnlyDictionary<Type, IReadOnlyDictionary<Type, EventMethod>> GetEventApplicationMethods(params Type[] types);
    }
}