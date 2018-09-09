using OpenTK;


namespace Overlord
{
    class Program
    {
        static void Main(string[] args)
        {
            //GameWindow
            GameWindow window = new GameWindow(800, 600);
            Game game = new Game(window);

            game.Run();
        }
    }
}
