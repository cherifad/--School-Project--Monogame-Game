using Jeux.Perso;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using Sprite = Jeux.Perso.Sprite;

namespace Jeux.Screen
{
    class Test : GameScreen
    {
        private Game1 _game1; // pour récupérer la fenêtre de jeu principale

        private TiledMap _tiledMap;

        private TiledMapRenderer _tiledMapRenderer;

        private SpriteBatch spriteBatch;

        private TiledMapTileLayer _mapLayer;

        public bool start, settings, exit;

        private Rectangle rectangleExit, rectangleStart, rectangleSettings;

        private int ScreenHeight, ScreenWidth;

        private Random Random;

        GraphicsDeviceManager graphics;

        private List<Sprite> _sprites;

        private bool _hasStarted = false;

        public Test(Game1 game) : base(game)
        {
            _game1 = game;            
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _tiledMap = Content.Load<TiledMap>("map/1Eta");

            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            graphics = _game1.Graphics;

            Random = new Random();

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            Restart();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _hasStarted = true;

            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            _tiledMapRenderer.Draw();

            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void Restart()
        {
            var player = Content.Load<SpriteSheet>("perso.sf", new JsonContentLoader());
            var playerTexture = new AnimatedSprite(player);

            _sprites = new List<Sprite>()
      {
        new Player(playerTexture)
        {
          Position = new Vector2(100, 100),
        }
      };

            _hasStarted = false;
        }

    }
}
