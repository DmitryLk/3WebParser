using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebParser.PresentierController;



namespace WebParser.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPresentierView, IControllerView
    {

        public event EventHandler<MyEventArgs> ImdbRequestUIEvent = delegate { };
        public event EventHandler<MyEventArgs> SpaceObjectImageRequestUIEvent = delegate { };

        public MainWindow()
        {
            InitializeComponent();

        }

        //public string FilmName
        //{
        //    get { return tbFilmName.Text; }
        //    //set { tbFilmName.Text = value; }
        //}

        public string ImdbFilmRating
        {
            //get { return tbFilmName.Text; }
            set { lbImdbFilmRating.Content = value; }
        }


        private void butIMDB_Click(object sender, RoutedEventArgs e)
        {
            if (ImdbRequestUIEvent!=null) ImdbRequestUIEvent(this, new MyEventArgs() { MyEventParameter = tbFilmName.Text});

        }

        private void butSpaceObject_Click(object sender, RoutedEventArgs e)
        {

            if (SpaceObjectImageRequestUIEvent != null) SpaceObjectImageRequestUIEvent(this, new MyEventArgs() { MyEventParameter = tbFilmName.Text });

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            //image.UriSource = new Uri("//upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg");
            image.UriSource = new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg");
            image.EndInit();
            imgTestImage.Source = image;

        }
    }
}
