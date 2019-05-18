using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebParser.App
{
    public class RequestSpaceObjectImageByNameInteractor : IInteractor<SpaceObjectNameRequestDTO>
    {
        private readonly IPresentier<SpaceObjectImageResponseDTO> _presentier;
        private readonly IValidator<SpaceObjectNameRequestDTO> _validator;
        private readonly IWebRepository _repository;



        public RequestSpaceObjectImageByNameInteractor(IPresentier<SpaceObjectImageResponseDTO> presentier, IWebRepository repository, IValidator<SpaceObjectNameRequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _repository = repository;
        }

        public async Task Execute(SpaceObjectNameRequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO, out var validationResult) == false) throw new ArgumentException(validationResult);

            var imageDTO = await _repository.QueryFindSpaceObjectImage(new RequestToWebRepositoryDTO { Number="0", Variants = new List<string> { requestDTO.SpaceObjectName } });
            _presentier.Handle(imageDTO);
        }

    }
}
