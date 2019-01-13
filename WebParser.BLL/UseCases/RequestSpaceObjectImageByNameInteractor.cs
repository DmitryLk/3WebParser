using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    public class RequestSpaceObjectImageByNameInteractor : IInteractor<SpaceObjectNameRequestDTO>
    {
        private readonly IPresentier _presentier;
        private readonly IValidator<SpaceObjectNameRequestDTO> _validator;
        private readonly IRepository _repository;



        public RequestSpaceObjectImageByNameInteractor(IPresentier presentier, IRepository repository, IValidator<SpaceObjectNameRequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _repository = repository;
        }

        public void Execute(SpaceObjectNameRequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO) == false) throw new ArgumentException(_validator.GetValidationResultString(requestDTO));
            var imdb = _repository.QueryFindImdbByFilmName(requestDTO.SpaceObjectName);
            _presentier.Handle(new ImdbRatingResponseDTO { ImdbRating = imdb });
        }

    }
}
