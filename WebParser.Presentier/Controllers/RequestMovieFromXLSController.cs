using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.PresentierController;
using WebParser.App;

namespace WebParser.PresentierController
{
    public class RequestMovieFromXLSController : IController
    {
        private readonly IInteractor<XLSFileRequestDTO> _interactor;
        private readonly IMessageServiceUI _messageServiceUI;

        public RequestMovieFromXLSController(IInteractor<XLSFileRequestDTO> interactor, IMessageServiceUI messageServiceUI)
        {
            _interactor = interactor;
            _messageServiceUI = messageServiceUI;
        }


        public async void Handle(object sender, MyEventArgs e)
        {
            try
            {
                await _interactor.Execute(new XLSFileRequestDTO { fileName = "Ссылки.xlsx", listName = "flm", columnsNumber = new int[] { 2, 3 }, topRowNumber = 2 });
            }
            catch (Exception ex)
            {
                _messageServiceUI.ShowError(ex.Message);
            }
        }

    }
}
