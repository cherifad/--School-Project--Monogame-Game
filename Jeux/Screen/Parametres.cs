using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Jeux.Controles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Screen
{
	public class Parametres : GameScreen
	{
		SpriteBatch spriteBatch;

		// pour récupérer le jeu en cours
		private Game1 _myGame;

		//components
		private List<Components> _gameComponents;

		//map et rendu
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;

		//music
		bool music = true;

		public Parametres(Game1 game) : base(game)
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
			var boutonSon = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonSon"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(550, 250),
			};
			boutonSon.Click += this.BoutonSon_Click;

			var bouttonRules = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonRules"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(550, 450)
			};
			bouttonRules.Click += this.BouttonRules_Click;

			var bouttonLangues = new Boutons(Content.Load<Texture2D>("map/ecranAcceuil/bouttonLangues"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(550, 650)
			};
			bouttonLangues.Click += this.BouttonLangues_Click;

			var boutonNext = new Boutons(Content.Load<Texture2D>("rules/arrow2"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(50, 750),
			};
			boutonNext.Click += this.BoutonNext_Click;

			//components
			_gameComponents = new List<Components>()
			{
				boutonSon,
				bouttonRules,
				bouttonLangues,
				boutonNext
			};

			base.LoadContent();

		}

		//langues
		private void BouttonLangues_Click(object sender, EventArgs e)
		{
			if (_myGame._langue == Game1.Langue.English)
			{
				_myGame._langue = Game1.Langue.French;
			}
			else if (_myGame._langue == Game1.Langue.French)
			{
				_myGame._langue = Game1.Langue.English;
			}
		}

		//instructions
		private void BouttonRules_Click(object sender, EventArgs e)
		{
			_myGame.ScreenManager.LoadScreen(_myGame._screenRules);
		}

		//son
		private void BoutonSon_Click(object sender, EventArgs e)
		{
			if (music == true)
			{
				MediaPlayer.Pause();
				music = false;
			}

			else if (music == false)
			{
				MediaPlayer.Resume();
				music = true;
			}
		}

		//retour
		private void BoutonNext_Click(object sender, EventArgs e)
		{
			_myGame.ScreenManager.LoadScreen(_myGame._screenHome, new FadeTransition(GraphicsDevice, Color.Black));

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
				spriteBatch.DrawString(_myGame._fontTitle, "Settings", new Vector2(550, 0), Color.White);
				spriteBatch.DrawString(_myGame._font2, "Sound", new Vector2(800, 280), Color.White);
				spriteBatch.DrawString(_myGame._font2, "Rules", new Vector2(800, 480), Color.White);
				spriteBatch.DrawString(_myGame._font2, "Language", new Vector2(800, 680), Color.White);

			}
			else if (_myGame._langue == Game1.Langue.French)
			{
				spriteBatch.DrawString(_myGame._fontTitle, "Parametres", new Vector2(450, 0), Color.White);
				spriteBatch.DrawString(_myGame._font2, "Son", new Vector2(800, 280), Color.White);
				spriteBatch.DrawString(_myGame._font2, "Regles", new Vector2(800, 480), Color.White);
				spriteBatch.DrawString(_myGame._font2, "Langue", new Vector2(800, 680), Color.White);
			}


			spriteBatch.End();
		}

	}
}
