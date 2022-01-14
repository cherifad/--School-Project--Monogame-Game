using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Screen
{
    class Level2 : GameScreen
    {
        private Game1 _game1;

        private TiledMap _map;

        private TiledMapRenderer _renduMap;

        public Level2(Game1 game) : base(game)
        {
            _game1 = game;
        }
        public override void LoadContent()
        {

            _map = Content.Load<TiledMap>("level2/2Eta");
            _renduMap = new TiledMapRenderer(GraphicsDevice, _map);

            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
