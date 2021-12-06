using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMServer_WPF.MVVM.Model
{
    interface IObservable
    {
        void AddObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers(Type type);
    }

    interface IObserver
    {
        void UpdateProperty(Type type);
    }
}
