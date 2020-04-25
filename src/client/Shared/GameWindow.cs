#region Using Statements
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mir.Client.Models;

#endregion

namespace Mir.Client
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWindow : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private AppSettings _appSettings;

        public GameWindow(AppSettings appsettings)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            _appSettings = appsettings ?? throw new ArgumentNullException(nameof(appsettings));
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
            NormalFont = Content.Load<SpriteFont>("fonts/normal");
            //TODO: use this.Content to load your game content here 
        }

        public SpriteFont NormalFont { get; set; }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif
            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            spriteBatch.Begin();

            spriteBatch.DrawString(NormalFont, $"Config: " + _appSettings.Content.Path, new Vector2(10, 10), Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
