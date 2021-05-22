using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using SingalRServer;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Linq;
using SharedLib;

[assembly: OwinStartup(typeof(Startup))]
namespace SingalRServer
{
    [HubName("MyHub")]
    public class MyHub : Hub
    {
        public long GetID()
        {
            long oldid = GameServer.id;

            GameServer.id++;
            return oldid;
        }

        public Vector2 GetNewPosition(SendableSprite sprite, Keys[] keys)
        {
            Vector2 pos = sprite.Position;
            foreach (var key in keys)
            {
                if (key == Keys.W)
                {
                    pos = new Vector2(pos.X, pos.Y - 10);
                }
                else if (key == Keys.S)
                {
                    pos = new Vector2(pos.X, pos.Y + 10);
                }

                if (key == Keys.D)
                {
                    pos = new Vector2(pos.X + 10, pos.Y);
                }
                else if (key == Keys.A)
                {
                    pos = new Vector2(pos.X - 10, pos.Y);
                }
            }

            GameServer.screenSprites[(long)sprite.Tag] = sprite;
            return pos;
        }

        public List<SendableSprite> GetObjectsInGame()
        {
            return GameServer.screenSprites.Values.ToList();
        }
    }
}
