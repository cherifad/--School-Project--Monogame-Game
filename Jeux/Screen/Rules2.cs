using Jeux;
using Jeux.Controles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Screen
{
	public class Rules2 : GameScreen
	{
		SpriteBatch spriteBatch;

		// pour récupérer le jeu en cours
		private Game1 _myGame;

		//components
		private List<Components> _gameComponents;

		//map et rendu
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;

		//parchemin
		private Texture2D _parchemin;
		private Vector2 _positionParchemin;
		private Texture2D _image1;

		public Rules2(Game1 game) : base(game)
		{
			_myGame = game;
		}


		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//map
			_tiledMap = Content.Load<TiledMap>("homescreen");
			_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);


			//parchemin
			_parchemin = Content.Load<Texture2D>("map/ecranAcceuil/parchemin");
			_positionParchemin = new Vector2(0, 0);

			//images
			_image1 = Content.Load<Texture2D>("rules/image1");

			//bouton
			var boutonNext = new Boutons(Content.Load<Texture2D>("rules/arrow"), Content.Load<SpriteFont>("Font/font"))
			{
				Position = new Vector2(1600, 400),
			};
			boutonNext.Click += this.BoutonNext_Click;

			_gameComponents = new List<Components>()
			{
				boutonNext
			};

			base.LoadContent();

		}

		private void BoutonNext_Click(object sender, EventArgs e)
		{
			_myGame.ScreenManager.LoadScreen(_myGame._screenParametre);
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

			//affichage parchemin
			spriteBatch.Draw(_parchemin, new Rectangle(0, -350, _myGame.Graphics.PreferredBackBufferWidth, _myGame.Graphics.PreferredBackBufferHeight + 800),
			   new Rectangle(0, 0, _parchemin.Width, _parchemin.Height), Color.White);

			spriteBatch.Draw(_image1, new Vector2(820, 300), Color.White);


			//texte
			if (_myGame._langue == Game1.Langue.English)
			{
				spriteBatch.DrawString(_myGame._font2, "How to play ?", new Vector2(610, 148), Color.Black);
				spriteBatch.DrawString(_myGame._fontExit, "Beware of the ennemies you'll \nencounter on your way", new Vector2(550, 700), Color.Black);
			}
			else if (_myGame._langue == Game1.Langue.French)
			{
				spriteBatch.DrawString(_myGame._font2, "Comment jouer ?", new Vector2(500, 150), Color.Black);
				spriteBatch.DrawString(_myGame._fontExit, "Mefie-toi des ennemis que tu \nrencontreras sur ton chemin.", new Vector2(650, 700), Color.Black);

			}

			//components
			foreach (var component in _gameComponents)
				component.Draw(gameTime, spriteBatch);

			spriteBatch.End();
		}

	}
}

