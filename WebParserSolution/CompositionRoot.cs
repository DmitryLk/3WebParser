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
    class CompositionRoot
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

            builder.RegisterType<ImdbRatingPresentier>().WithParameter("view", _window);
            builder.RegisterType<MovieFromXLSPresentier>().WithParameter("view", _window);
            builder.RegisterType<SpaceObjectImagePresentier>().WithParameter("view", _window);


            //builder.Register<IPresentier>((c, p) =>
            //        {
            //            var typeName = p.Named<string>("typeName");
            //            if (typeName == "ImdbRatingByFilmName") return c.Resolve<ImdbRatingPresentier>(new NamedParameter("view", _window));
            //            if (typeName == "SpaceObjectImageByName") return c.Resolve<SpaceObjectImagePresentier>(new NamedParameter("view", _window));
            //            throw new Exception("Type specified is incorrect");
            //        }
            //     );


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

                throw new Exception("Type specified is incorrect");
            });




            builder.RegisterType<WebRepository>().As<IWebRepository>();
            builder.RegisterType<XLSRepository>().As<IXLSRepository>();
            builder.RegisterGeneric(typeof(Validator<>)).As(typeof(IValidator<>));
            builder.RegisterType<RequestMovieInfoByMovieNameInteractor>();
            builder.RegisterType<RequestSpaceObjectImageByNameInteractor>();
            builder.RegisterType<RequestMovieFromXLSInteractor>();
            builder.RegisterType<RequestImdbByFilmNameController>();
            builder.RegisterType<RequestMovieFromXLSController>();
            builder.RegisterType<RequestSpaceObjectImageByNameController>();
            _container = builder.Build();
        }

        public void Resolve()
        {

            var requestImdbByFilmNameController = _container.Resolve<IController>(new NamedParameter("typeName", "ImdbRatingByFilmName"));
            var requestSpaceObjectImageByNameController = _container.Resolve<IController>(new NamedParameter("typeName", "SpaceObjectImageByName"));
            var requestMovieFromXLSController = _container.Resolve<IController>(new NamedParameter("typeName", "MovieFromXLS"));



            _window.ImdbRequestUIEvent += requestImdbByFilmNameController.Handle;
            _window.SpaceObjectImageRequestUIEvent += requestSpaceObjectImageByNameController.Handle;
            _window.MovieFromXLSUIEvent += requestMovieFromXLSController.Handle;




        }

        public void Release()
        {
            _container.Dispose();
        }

    }
}


//var requestSpaceObjectImageByNameInteractor = _container.Resolve<RequestSpaceObjectImageByNameInteractor>();
//var RequestSpaceObjectImageByNameController = _container.Resolve<RequestImdbByFilmNameController>(new NamedParameter("interactor", requestSpaceObjectImageByNameInteractor));

//var requestImdbByFilmNamePresentier = _container.Resolve<IPresentier>(new NamedParameter("typeName", "ImdbRatingByFilmName"));
//var requestImdbByFilmNameInteractor = _container.Resolve<RequestImdbByFilmNameInteractor>(new NamedParameter("presentier", requestImdbByFilmNamePresentier));
//var requestImdbByFilmNameController = _container.Resolve<RequestImdbByFilmNameController>(new NamedParameter("interactor", requestImdbByFilmNameInteractor));

//Controller controller = new Controller(_window, new RequestImdbByFilmNameInteractor(new ImdbRatingPresentier(_window), new Repository(), new MessageService(new MessageServiceUI()), new FilmNameValidator()));
