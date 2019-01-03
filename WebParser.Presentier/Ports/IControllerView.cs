using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.PresentierController
{
  
    public interface IControllerView
    {
        event EventHandler<MyEventArgs> ImdbRequestUIEvent;
    }
    
}
