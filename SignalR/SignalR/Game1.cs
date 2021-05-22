using MichaelLibrary;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SharedLib;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace SignalR
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        IHubProxy myHub;

        SendableSprite player;

        List<SendableSprite> spritesOnScreen = new List<SendableSprite>();

        OrthographicCamera camera;

        Texture2D pixel;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            var connection = new HubConnection("http://127.0.0.1:8088/");
            myHub = connection.CreateHubProxy("MyHub");

            connection.Start().Wait();

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            player = new SendableSprite(new[] { Color.White }, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Red, Vector2.One * 50, 1)
            {
                Origin = new Vector2(0.5f)
            };
            player.CreateTexture(GraphicsDevice);
            
            Task<long> t = myHub.Invoke<long>("GetID");
            while(t.IsCompleted == false) { }
            player.Tag = t.Result;

            var adapter = new BoxingViewportAdapter(Window, GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            camera = new OrthographicCamera(adapter);
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected async override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyState = Keyboard.GetState();

            player.Position = await myHub.Invoke<Vector2>("GetNewPosition", player, keyState.GetPressedKeys());
            camera.LookAt(player.Position);

            var m = await myHub.Invoke<List<SendableSprite>>("GetObjectsInGame");
            spritesOnScreen.Clear();
            foreach (var item in m)
            {
                if (item == null || (long)item.Tag == (long)player.Tag) continue;

                if (camera.BoundingRectangle.Contains(item.Position))
                {
                    item.CreateTexture(GraphicsDevice);

                    spritesOnScreen.Add(item);
                }
            }

            Window.Title = $"{spritesOnScreen.Count}";
     
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            player.Draw(spriteBatch);

            spriteBatch.Draw(pixel, camera.BoundingRectangle.ToRectangle(), Color.Red * 0.3f);

            foreach(var item in spritesOnScreen)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
