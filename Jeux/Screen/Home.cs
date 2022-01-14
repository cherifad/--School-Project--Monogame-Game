using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Screen
{
    class Home : GameScreen
    {
        private Game1 _game1; // pour récupérer la fenêtre de jeu principale

        private TiledMap _tiledMap;

        private TiledMapRenderer _tiledMapRenderer;

        private SpriteBatch spriteBatch;

        private TiledMapTileLayer _mapLayer;

        public bool start, settings, exit;

        private Rectangle rectangleExit, rectangleStart, rectangleSettings;

        public Home(Game1 game) : base(game)
        {
            _game1 = game;
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _tiledMap = Content.Load<TiledMap>("homeScreen");

            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);


             rectangleExit = new Rectangle(800, 640, 288, 160);
             rectangleStart = new Rectangle(640, 352, 608, 288);
             rectangleSettings = new Rectangle(1728, 0, 160, 160);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState aMouse = Mouse.GetState();

            if (aMouse.LeftButton == ButtonState.Pressed && _game1.mPreviousMouseState.LeftButton == ButtonState.Released)
            {
                //Set the starting location for the selection box to the current location
                //where the Left button was initially clicked.
                _game1.mSelectionBox = new Rectangle(aMouse.X, aMouse.Y, 0, 0);
            }

            //If the user has released the left mouse button, then reset the selection square
            if (aMouse.LeftButton == ButtonState.Released)
            {
                //Reset the selection square to no position with no height and width
                _game1.mSelectionBox = new Rectangle(-1, -1, 0, 0);                
            }

            //Store the previous mouse state
            _game1.mPreviousMouseState = aMouse;

            _tiledMapRenderer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _game1.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            start = rectangleStart.Intersects(_game1.mSelectionBox);

            settings = rectangleSettings.Intersects(_game1.mSelectionBox);

            exit = rectangleExit.Intersects(_game1.mSelectionBox);

            _tiledMapRenderer.Draw();

            spriteBatch.End();
        }

    }
}
