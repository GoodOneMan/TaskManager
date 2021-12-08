using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMService.CORE
{
    interface IObserver
    {
        void UpdateProperty(Type type);
    }
}
