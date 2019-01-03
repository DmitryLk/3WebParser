using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    public interface IPresentier
    {
        void Handle(ResponseDTO response);

        void ShowMessage(string message);
        void ShowExclamation(string exclamation);
        void ShowError(string error);
    }
}
