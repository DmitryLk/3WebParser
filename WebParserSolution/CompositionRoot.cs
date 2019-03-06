using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;
using WebParser.Data;
using WebParser.PresentierController;


namespace WebParser.UI
{
    class CompositionRoot : IDisposable
    {

        private readonly MainWindow _window;

        private IContainer _container;
        

        public CompositionRoot(MainWindow window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

       

        public void Register()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<MessageServiceUI>().As<IMessageServiceUI>();

            // PRESENTIERS
            builder.RegisterType<ImdbRatingPresentier>().WithParameter("view", _window);
            builder.RegisterType<MovieFromXLSPresentier>().WithParameter("view", _window);
            builder.RegisterType<SpaceObjectImagePresentier>().WithParameter("view", _window);
            builder.RegisterType<SpaceObjectImagesToFilesFromXLSPresentier>().WithParameter("view", _window);



            // CONTROLLERS
            builder.RegisterType<RequestImdbByFilmNameController>();
            builder.RegisterType<RequestMovieFromXLSController>();
            builder.RegisterType<RequestSpaceObjectImageByNameController>();
            builder.RegisterType<RequestSpaceObjectImagesToFilesFromXLSController>();

            builder.Register<IController>((c, p) =>
            {
                var typeName = p.Named<string>("typeName");
                if (typeName == "ImdbRatingByFilmName")
                {
                    var requestImdbByFilmNamePresentier = c.Resolve<ImdbRatingPresentier>(new NamedParameter("typeName", typeName));
                    var requestImdbByFilmNameInteractor = c.Resolve<RequestMovieInfoByMovieNameInteractor>(new NamedParameter("presentier", requestImdbByFilmNamePresentier));
                    return c.Resolve<RequestImdbByFilmNameController>(new NamedParameter("interactor", requestImdbByFilmNameInteractor));
                }
                if (typeName == "MovieFromXLS")
                {
                    var requestMovieFromXLSPresentier = c.Resolve<MovieFromXLSPresentier>(new NamedParameter("typeName", typeName));
                    var requestMovieFromXLSInteractor = c.Resolve<RequestMovieFromXLSInteractor>(new NamedParameter("presentier", requestMovieFromXLSPresentier));
                    return c.Resolve<RequestMovieFromXLSController>(new NamedParameter("interactor", requestMovieFromXLSInteractor));
                }
                if (typeName == "SpaceObjectImageByName")
                {
                    var requestSpaceObjectImageByNamePresentier = c.Resolve<SpaceObjectImagePresentier>(new NamedParameter("typeName", typeName));
                    var requestSpaceObjectImageByNameInteractor = c.Resolve<RequestSpaceObjectImageByNameInteractor>(new NamedParameter("presentier", requestSpaceObjectImageByNamePresentier));
                    return c.Resolve<RequestSpaceObjectImageByNameController>(new NamedParameter("interactor", requestSpaceObjectImageByNameInteractor));
                }
                if (typeName == "SpaceObjectImagesToFilesFromXLS")
                {
                    var requestSpaceObjectImagesToFilesFromXLSPresentier = c.Resolve<SpaceObjectImagesToFilesFromXLSPresentier>(new NamedParameter("typeName", typeName));
                    var requestSpaceObjectImagesToFilesFromXLSInteractor = c.Resolve<RequestSpaceObjectImagesToFilesByXLSInteractor>(new NamedParameter("presentier", requestSpaceObjectImagesToFilesFromXLSPresentier));
                    return c.Resolve<RequestSpaceObjectImagesToFilesFromXLSController>(new NamedParameter("interactor", requestSpaceObjectImagesToFilesFromXLSInteractor));
                }

                throw new Exception("Type specified is incorrect");
            });


            // INTERACTORS
            builder.RegisterType<RequestMovieInfoByMovieNameInteractor>();
            builder.RegisterType<RequestSpaceObjectImageByNameInteractor>();
            builder.RegisterType<RequestMovieFromXLSInteractor>();
            builder.RegisterType<RequestSpaceObjectImagesToFilesByXLSInteractor>();


            // INFRASTRUCTURE
            builder.RegisterType<WebRepository>().As<IWebRepository>();
            builder.RegisterType<XLSRepository>().As<IXLSRepository>();
            builder.RegisterGeneric(typeof(Validator<>)).As(typeof(IValidator<>));

            _container = builder.Build();

        }

        public void Resolve()
        {

            var requestImdbByFilmNameController = _container.Resolve<IController>(new NamedParameter("typeName", "ImdbRatingByFilmName"));
            var requestSpaceObjectImageByNameController = _container.Resolve<IController>(new NamedParameter("typeName", "SpaceObjectImageByName"));
            var requestMovieFromXLSController = _container.Resolve<IController>(new NamedParameter("typeName", "MovieFromXLS"));
            var requestSpaceObjectImagesToFilesFromXLSController = _container.Resolve<IController>(new NamedParameter("typeName", "SpaceObjectImagesToFilesFromXLS"));

            



            _window.ImdbRequestUIEvent += requestImdbByFilmNameController.Handle;
            _window.SpaceObjectImageUIEvent += requestSpaceObjectImageByNameController.Handle;
            _window.MovieFromXLSUIEvent += requestMovieFromXLSController.Handle;
            _window.SpaceObjectImagesToFilesFromXLSUIEvent += requestSpaceObjectImagesToFilesFromXLSController.Handle;

            

        }

        public void Release()
        {
            Dispose();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

    }
}

//builder.Register<IPresentier>((c, p) =>
//        {
//            var typeName = p.Named<string>("typeName");
//            if (typeName == "ImdbRatingByFilmName") return c.Resolve<ImdbRatingPresentier>(new NamedParameter("view", _window));
//            if (typeName == "SpaceObjectImageByName") return c.Resolve<SpaceObjectImagePresentier>(new NamedParameter("view", _window));
//            throw new Exception("Type specified is incorrect");
//        }
//     );


//var requestSpaceObjectImageByNameInteractor = _container.Resolve<RequestSpaceObjectImageByNameInteractor>();
//var RequestSpaceObjectImageByNameController = _container.Resolve<RequestImdbByFilmNameController>(new NamedParameter("interactor", requestSpaceObjectImageByNameInteractor));

//var requestImdbByFilmNamePresentier = _container.Resolve<IPresentier>(new NamedParameter("typeName", "ImdbRatingByFilmName"));
//var requestImdbByFilmNameInteractor = _container.Resolve<RequestImdbByFilmNameInteractor>(new NamedParameter("presentier", requestImdbByFilmNamePresentier));
//var requestImdbByFilmNameController = _container.Resolve<RequestImdbByFilmNameController>(new NamedParameter("interactor", requestImdbByFilmNameInteractor));

//Controller controller = new Controller(_window, new RequestImdbByFilmNameInteractor(new ImdbRatingPresentier(_window), new Repository(), new MessageService(new MessageServiceUI()), new FilmNameValidator()));
