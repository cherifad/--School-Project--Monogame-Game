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
    public class Home : GameScreen
    {
		SpriteBatch spriteBatch;

		// pour récupérer le jeu en cours
		private Game1 _myGame;

		//components
		private List<Components> _gameComponents;

		//map et rendu
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;


		public Home(Game1 game) : base(game)
		{
			_myGame = game;
		}
		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//map
			_tiledMap = Content.Load<TiledMap>("homescreen");
			_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);


			//bouttons
			var bouttonExit = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonExit"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(768, 645),
			};
			bouttonExit.Click += this.BouttonExit_Click;

			var bouttonParametre = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonParametre"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(1728, 0),
			};
			bouttonParametre.Click += this.BouttonParametre_Click;

			var bouttonStart = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonStart"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(544, 195),
			};
			bouttonStart.Click += this.BouttonStart_Click;

			//components
			_gameComponents = new List<Components>()
			{
				bouttonExit,
				bouttonParametre,
				bouttonStart,
			};
	

			base.LoadContent();

		}

		//boutons
		private void BouttonStart_Click(object sender, EventArgs e)
		{
			_myGame.ScreenManager.LoadScreen(_myGame._screenLevel1, new FadeTransition(GraphicsDevice, Color.Black));
		}

		private void BouttonParametre_Click(object sender, EventArgs e)
		{
			_myGame.ScreenManager.LoadScreen(_myGame._screenParametre, new FadeTransition(GraphicsDevice, Color.Black));
		}

		private void BouttonExit_Click(object sender, EventArgs e)
		{
			_myGame.Exit();
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

			//texte
			spriteBatch.DrawString(_myGame._fontTitle, "Nom Jeu?", new Vector2(500, 0), Color.White);

			if (_myGame._langue == Game1.Langue.English)
			{
				spriteBatch.DrawString(_myGame._fontStart, "START", new Vector2(765, 540), Color.White);
				spriteBatch.DrawString(_myGame._fontExit, "EXIT", new Vector2(880, 795), Color.White);
			}
			else if (_myGame._langue == Game1.Langue.French)
			{
				spriteBatch.DrawString(_myGame._fontStart, "LANCER", new Vector2(720, 540), Color.White);
				spriteBatch.DrawString(_myGame._fontExit, "QUITTER", new Vector2(830, 795), Color.White);
			}


			spriteBatch.End();
		}
	}
}
