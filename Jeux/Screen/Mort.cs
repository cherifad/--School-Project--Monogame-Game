using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using Jeux.Controles;
using MonoGame.Extended.Screens.Transitions;

namespace Jeux.Screen
{
	public class Mort : GameScreen
	{
		SpriteBatch spriteBatch;

		// pour récupérer le jeu en cours
		private Game1 _myGame;

		//components
		private List<Components> _gameComponents;

		//map et rendu
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;


		public Mort(Game1 game) : base(game)
		{
			_myGame = game;
		}
		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//map
			_tiledMap = Content.Load<TiledMap>("map/ecranMort/ecranmortsansboutton");
			_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);


			//bouttons
			var bouttonExit = new Boutons(Content.Load<Texture2D>("map/ecranMort/ImmagebouttonExit"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(600, 500),
			};
			bouttonExit.Click += this.BouttonExit_Click;

			//components
			_gameComponents = new List<Components>()
			{
				bouttonExit,

			};

			base.LoadContent();

		}

		//boutons
		private void BouttonExit_Click(object sender, EventArgs e)
		{
			_myGame.ScreenManager.LoadScreen(_myGame._screenHome);
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var component in _gameComponents)
				component.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			_myGame.GraphicsDevice.Clear(Color.Black);


			spriteBatch.Begin();

			//affichage map
			_tiledMapRenderer.Draw();

			//components
			foreach (var component in _gameComponents)
				component.Draw(gameTime, spriteBatch);


			spriteBatch.End();
		}
	}
}
