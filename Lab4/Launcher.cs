using SFML.Graphics;
using SFML.Window;
using System;
using System.Timers;

namespace Lab4
{
    internal class Launcher
    {
        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }        
        static void Main()
        {                      
            RenderWindow MainWindow = new RenderWindow(new VideoMode(1300, 1000), "Yonko Adventures");
            MainWindow.SetFramerateLimit(60);
            MainWindow.Closed += new EventHandler(OnClose);

            View visual = new View();
            visual.Init(MainWindow);            
            Model model = new Model();
            model.Init();

            IControllerLaunch controller = new Controller();
            controller.InitRandom(new CustomRandom(new Random()));
            controller.InitTimer(new Timer(Model.FALLING_OBJECT_SPAWN_TIME));
            controller.Init(visual, model);


            visual.AddController((Controller)controller);
            model.AddController((Controller)controller);
            controller.RenderLevel();            

            while (MainWindow.IsOpen)
            {
                MainWindow.DispatchEvents();
                controller.MovementHandler();
                controller.Update();
                visual.DrawScene();
            }          
        }
    }
}
