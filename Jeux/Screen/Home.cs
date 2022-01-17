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

		//titre
		private Texture2D _titre;

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
				Position = new Vector2(832, 770),
			};
			bouttonExit.Click += this.BouttonExit_Click;

			var bouttonParametre = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonParametre"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(1728, 0),
			};
			bouttonParametre.Click += this.BouttonParametre_Click;

			var bouttonStart = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonStart"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(668, 480),
			};
			bouttonStart.Click += this.BouttonStart_Click;

			//components
			_gameComponents = new List<Components>()
			{
				bouttonExit,
				bouttonParametre,
				bouttonStart,
			};

			//titre
			_titre = Content.Load<Texture2D>("map/ecranAcceuil/titre");

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
			if (_myGame._langue == Game1.Langue.English)
			{
				spriteBatch.DrawString(_myGame._fontStart, "START", new Vector2(775, 540), Color.White);
				spriteBatch.DrawString(_myGame._fontExit, "EXIT", new Vector2(900, 795), Color.White);
			}
			else if (_myGame._langue == Game1.Langue.French)
			{
				spriteBatch.DrawString(_myGame._fontStart, "LANCER", new Vector2(730, 540), Color.White);
				spriteBatch.DrawString(_myGame._fontExit, "QUITTER", new Vector2(850, 795), Color.White);
			}

			//affichage parchemin
			spriteBatch.Draw(_titre, new Vector2(400, 150),Color.White);
			spriteBatch.End();
		}
	}
}
