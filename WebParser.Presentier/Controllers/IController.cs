namespace WebParser.PresentierController
{
    public interface IController
    {
        void Handle(object sender, MyEventArgs e);
    }
}