using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;


namespace WebParser.App
{
    public class RequestSpaceObjectImagesToFilesByXLSInteractor : IInteractor<XLSFileRequestDTO>
    {
        private readonly IPresentier<SpaceObjectImagesSaveToFilesResponseDTO> _presentier;
        private readonly IValidator<XLSFileRequestDTO> _validator;
        private readonly IXLSRepository _xlsRepository;
        private readonly IWebRepository _webRepository;



        public RequestSpaceObjectImagesToFilesByXLSInteractor(IPresentier<SpaceObjectImagesSaveToFilesResponseDTO> presentier, IXLSRepository xlsRepository, IWebRepository webRepository, IValidator<XLSFileRequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _xlsRepository = xlsRepository;
            _webRepository = webRepository;
        }

        public async Task Execute(XLSFileRequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO) == false) throw new ArgumentException(_validator.GetValidationResultString(requestDTO));

            //var fileSaver = new BitmapImageHelper();
            XLSColumnsToListDTO tempResults = await _xlsRepository.QueryGetDataFromColumnUntilEmpty(requestDTO);

            foreach (var res in tempResults.SearchResultsList)
            {
                SpaceObjectImageResponseDTO imageDTO = await _webRepository.QueryFindSpaceObjectImageByName(res.ElementAtOrDefault(1));
                if (imageDTO.SpaceObjectImage != null) BitmapImageHelper.SaveImage(imageDTO.SpaceObjectImage, requestDTO.TargetFolder, res.ElementAtOrDefault(0) + "_" + res.ElementAtOrDefault(1));

            }
            _presentier.Handle(new SpaceObjectImagesSaveToFilesResponseDTO { Result = true });

        }

    }

    static class BitmapImageHelper
    {
      

        static public void SaveImage(BitmapImage spaceObjectImage, string folderName, string spaceObjectName)
        {
            var fileName = Regex.Replace(spaceObjectName, "[^a-zA-Z0-9-_]", String.Empty) + ".jpg";
          

            if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);

            var localFilePath = Path.Combine(folderName, fileName);


            if (File.Exists(localFilePath)) return;

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(spaceObjectImage));
            using (var filestream = new FileStream(localFilePath, FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }
    }
}
