using System;

namespace TMClient.CORE
{
    interface IObservable
    {
        void AddObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers(Type type);
    }
}
