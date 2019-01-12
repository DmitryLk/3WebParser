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
            if (window == null) throw new ArgumentNullException(nameof(window));
            _window = window;
        }



        public void Register()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<Presentier>().As<IPresentier>().WithParameter("view", _window);
            builder.RegisterType<Repository>().As<IRepository>();
            builder.RegisterType<MessageServiceUI>().As<IMessageServiceUI>();
            builder.RegisterType<Validator<RequestDTO>>().As<IValidator<RequestDTO>>();

            builder.RegisterType<RequestImdbByFilmNameInteractor>();
            builder.RegisterType<Controller>();


            _container = builder.Build();
            
        }

        public void Resolve()
        {
            var requestImdbByFilmNameInteractor = _container.Resolve<RequestImdbByFilmNameInteractor>();

            var requestImdbByFilmNameController = _container.Resolve<Controller>(new NamedParameter("interactor", requestImdbByFilmNameInteractor));


            //Controller controller = _container.Resolve<Controller>();



            _window.ImdbRequestUIEvent += requestImdbByFilmNameController.Handle;

        }

        public void Release()
        {
            _container.Dispose();
        }

    }
}

//Controller controller = new Controller(_window, new RequestImdbByFilmNameInteractor(new Presentier(_window), new Repository(), new MessageService(new MessageServiceUI()), new FilmNameValidator()));
