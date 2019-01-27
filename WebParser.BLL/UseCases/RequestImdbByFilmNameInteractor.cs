using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;

namespace WebParser.App
{
    public class RequestImdbByFilmNameInteractor : IInteractor<FilmNameRequestDTO> 
    {
        private readonly IPresentier<MovieInfoResponseDTO> _presentier;
        private readonly IValidator<FilmNameRequestDTO> _validator;
        private readonly IRepository _repository;



        public RequestImdbByFilmNameInteractor(IPresentier<MovieInfoResponseDTO> presentier, IRepository repository, IValidator<FilmNameRequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _repository = repository;
        }

        public async Task Execute(FilmNameRequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO) ==false) throw new ArgumentException(_validator.GetValidationResultString(requestDTO));
            var imdb = await _repository.QueryFindImdbByFilmName(requestDTO.FilmName);
            _presentier.Handle(new MovieInfoResponseDTO { });
        }
    }
}
