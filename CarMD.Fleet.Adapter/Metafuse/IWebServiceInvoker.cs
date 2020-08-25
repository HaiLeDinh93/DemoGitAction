using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Adapter.Metafuse
{
    public interface IWebServiceInvoker
    {
        TResult InvokeWebService<TResult>(string serviceOperator, params object[] param);
    }
}
