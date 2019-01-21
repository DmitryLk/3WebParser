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
        private readonly IRepository _repository;



        public RequestSpaceObjectImageByNameInteractor(IPresentier<SpaceObjectImageResponseDTO> presentier, IRepository repository, IValidator<SpaceObjectNameRequestDTO> validator)
        {
            _presentier = presentier;
            _validator = validator;
            _repository = repository;
        }

        public async Task Execute(SpaceObjectNameRequestDTO requestDTO)
        {
            if (_validator.IsValid(requestDTO) == false) throw new ArgumentException(_validator.GetValidationResultString(requestDTO));
            var image = await _repository.QueryFindSpaceObjectImageByName(requestDTO.SpaceObjectName);
            _presentier.Handle(new SpaceObjectImageResponseDTO { SpaceObjectImage = image });
        }

    }
}
