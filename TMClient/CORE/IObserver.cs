using System;

namespace TMClient.CORE
{
    interface IObserver
    {
        void UpdateProperty(Type type);
    }
}
