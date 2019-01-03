using Autofac;
using Autofac.Features.ResolveAnything;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebParser.PresentierController;
using WebParser.App;
using WebParser.Data;

namespace WebParser.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            App app = new App();
            MainWindow window = new MainWindow();

            CompositionRoot _compositionRoot = new CompositionRoot(window);

            _compositionRoot.Register();
            _compositionRoot.Resolve();
            _compositionRoot.Release();


            app.Run(window);
        }
    }
}
