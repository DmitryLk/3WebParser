using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;


namespace WebParser.App
{
    public class RequestImdbByFilmNameInteractor : IInteractor 
    {
        private readonly IPresentier _presentier;
        private readonly IValidator<RequestDTO> _validator;
        private readonly IRepository _repository;



        public RequestImdbByFilmNameInteractor(IPresentier presentier, IRepository repository, IValidator<RequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _repository = repository;
        }

        public void Execute(RequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO) ==false) throw new ArgumentException(_validator.GetValidationResultString(requestDTO));
            var imdb = _repository.QueryFindImdbByFilmName(requestDTO.FilmName);
            _presentier.Handle(new ResponseDTO { ImdbRating = imdb });
        }
    }
}
