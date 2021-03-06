﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.PresentierController;
using WebParser.App;



namespace WebParser.PresentierController
{
    public class SpaceObjectImagesToFilesFromXLSPresentier : IPresentier<SpaceObjectImagesSaveToFilesResponseDTO>
    {
        private readonly IPresentierView _view;
        private readonly IMessageServiceUI _messageServiceUI;


        public SpaceObjectImagesToFilesFromXLSPresentier(IPresentierView view, IMessageServiceUI messageServiceUI)
        {
            _view = view;
            _messageServiceUI = messageServiceUI;
        }

    

        public void Handle(SpaceObjectImagesSaveToFilesResponseDTO response)
        {
            //_view.SpaceObjectImage = response.SpaceObjectImage;
            //_view.SpaceObjectImageResponse = new SpaceObjectImageResponseViewModel
            //{ SpaceObjectName = response.SpaceObjectName, SpaceObjectImage = response.SpaceObjectImage };
            _view.butSpaceObjectToFileState = true;
        }



        public void ShowMessage(string message) => _messageServiceUI.ShowMessage(message);
        public void ShowExclamation(string exclamation) => _messageServiceUI.ShowExclamation(exclamation);
        public void ShowError(string error) => _messageServiceUI.ShowError(error);

    }
}
