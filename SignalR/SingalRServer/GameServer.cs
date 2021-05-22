using Microsoft.Xna.Framework;
using System;
using Microsoft.Owin.Hosting;
using SingalRServer;
using Microsoft.Owin;
using System.Collections.Generic;
using SharedLib;
using Microsoft.Xna.Framework.Input;

[assembly: OwinStartup(typeof(Startup))]
namespace SingalRServer
{
    public class GameServer
    {
        string url = "http://127.0.0.1:8088/";

        IDisposable SignalR;

        public static Dictionary<long, SendableSprite> screenSprites = new Dictionary<long, SendableSprite>();
        public static long id = 0;

        public void Run()
        {
            LoadContent();
            using (SignalR = WebApp.Start(url))
            {
                Console.WriteLine("Server is running, press any key to close it");
                Console.ReadKey();
            }

        }

        public void LoadContent()
        {
            Random random = new Random();

            Vector2 scale = new Vector2(10, 10);

            for (int i = 0; i < 1; i++)
            {
                int width = 800;
                int height = 800;

                var x = random.Next((int)(0 + scale.X / 2), (int)(width - scale.X / 2 ));
                var y = random.Next((int)(0 + scale.Y / 2), (int)(height - scale.Y / 2));

                screenSprites.Add(id, new SendableSprite(new[] { Color.White }, new Vector2(x, y), Color.Black, new Vector2(10, 10), 1));
                screenSprites[id].Tag = id;

                id++;
            }
        }
    }
}
