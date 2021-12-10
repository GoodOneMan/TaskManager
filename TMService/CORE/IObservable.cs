using System;

namespace TMService.CORE
{
    enum FlagAccess
    {
        all,
        view,
        service
    }

    interface IObservable
    {
        void AddObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers(Type type, FlagAccess flag);
    }
}
