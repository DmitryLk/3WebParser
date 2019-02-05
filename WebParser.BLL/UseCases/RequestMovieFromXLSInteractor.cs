using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;

namespace WebParser.App
{
    public class RequestMovieFromXLSInteractor : IInteractor<XLSFileRequestDTO> 
    {
        private readonly IPresentier<MovieInfoResponseDTO> _presentier;
        private readonly IValidator<XLSFileRequestDTO> _validator;
        private readonly IXLSRepository _repository;



        public RequestMovieFromXLSInteractor(IPresentier<MovieInfoResponseDTO> presentier, IXLSRepository repository, IValidator<XLSFileRequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _repository = repository;
        }

        public async Task Execute(XLSFileRequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO) ==false) throw new ArgumentException(_validator.GetValidationResultString(requestDTO));
            var movieInfo = await _repository.QueryGetMovieData(requestDTO);
            _presentier.Handle(movieInfo);
        }
    }
}
