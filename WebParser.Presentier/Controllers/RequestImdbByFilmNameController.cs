using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.PresentierController;
using WebParser.App;

namespace WebParser.PresentierController
{
    public class RequestImdbByFilmNameController : IController
    {
        private readonly IInteractor<FilmNameRequestDTO> _interactor;
        private readonly IMessageServiceUI _messageServiceUI;

        public RequestImdbByFilmNameController(IInteractor<FilmNameRequestDTO> interactor, IMessageServiceUI messageServiceUI)
        {
            _interactor = interactor;
            _messageServiceUI = messageServiceUI;
        }


        public void Handle(object sender, MyEventArgs e)
        {
            try
            {
                _interactor.Execute(new FilmNameRequestDTO { FilmName = e.MyEventParameter });
            }
            catch (Exception ex)
            {
                _messageServiceUI.ShowError(ex.Message);
            }
        }

    }
}
