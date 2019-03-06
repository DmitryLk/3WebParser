using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.PresentierController;
using WebParser.App;
using System.IO;

namespace WebParser.PresentierController
{
    public class RequestSpaceObjectImagesToFilesFromXLSController : IController
    {
        private readonly IInteractor<XLSFileRequestDTO> _interactor;
        private readonly IMessageServiceUI _messageServiceUI;

        public RequestSpaceObjectImagesToFilesFromXLSController(IInteractor<XLSFileRequestDTO> interactor, IMessageServiceUI messageServiceUI)
        {
            _interactor = interactor;
            _messageServiceUI = messageServiceUI;
        }


        public async void Handle(object sender, MyEventArgs e)
        {
            try
            {
                await _interactor.Execute(new XLSFileRequestDTO { FileName = "Solar system.xlsx", ListName = "Лист1", ColumnsNumber = new int[] { 1, 11 }, TopRowNumber = 3, TargetFolder= $@"{Directory.GetCurrentDirectory()}\PlanetImages\" });
            }
            catch (Exception ex)
            {
                _messageServiceUI.ShowError(ex.Message);
            }
        }

    }
}
