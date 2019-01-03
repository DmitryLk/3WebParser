using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.PresentierController;
using WebParser.App;

namespace WebParser.PresentierController
{
    public class Controller
    {
        private readonly IInteractor _interactor;
        private readonly IMessageServiceUI _messageServiceUI;

        public Controller(IInteractor interactor, IMessageServiceUI messageServiceUI)
        {
            _interactor = interactor;
            _messageServiceUI = messageServiceUI;
        }


        public void Handle(object sender, MyEventArgs e)
        {
            try
            {
                _interactor.Execute(new RequestDTO { FilmName = e.MyEventParameter });
            }
            catch (Exception ex)
            {
                _messageServiceUI.ShowError(ex.Message);
            }
        }

    }
}
