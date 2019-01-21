using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    public interface IInteractor<T>
    {
        Task Execute(T requestDTO);
    }


}
