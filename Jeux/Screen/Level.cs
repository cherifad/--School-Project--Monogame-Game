using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Jeux.Perso;
using System.Collections.Generic;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using Sprite = Jeux.Perso.Sprite;
using MonoGame.Extended.Content;
using Microsoft.Xna.Framework.Input;

namespace Jeux.Screen
{
    public class Level : GameScreen
    {
        // pour récupérer la fenêtre de jeu principale
        private Game1 _game1;

        //map
        private List<TiledMap> _map = new List<TiledMap>(), _parametres = new List<TiledMap>(); 
        private List<TiledMapRenderer> _renduMap = new List<TiledMapRenderer>(), _renduParametres = new List<TiledMapRenderer>();
        public int _mapEnCour;

        //idk
        private List<Rectangle> _start = new List<Rectangle>();
        private List<Sprite> _player, _enemys = new List<Sprite>();
        private AnimatedSprite enemyTexture;

        //kill and lives
        private int _kill = 0;

        public Level(Game1 game) : base(game)
        {
            _game1 = game;
        }

        public override void Initialize()
        {
            // TODO: Add your initialization logic here

            _game1.IsMouseVisible = false;

            _mapEnCour = 0;

            base.Initialize();

        }
        public override void LoadContent()
        {

            //chargement des map
          for (int i = 0; i < 5; i++)
           {
                _map.Add(Content.Load<TiledMap>($"map/{i + 1}Eta"));
                _renduMap.Add(new TiledMapRenderer(GraphicsDevice, _map[i]));
           }


          //cgargement des écran de paramètrages
            for (int i = 0; i < 3; i++)
             {
                 _parametres.Add(Content.Load<TiledMap>($"map/para{i + 1}"));
                 _renduParametres.Add(new TiledMapRenderer(GraphicsDevice, _parametres[i]));
             }
                
                //chargement des textures
            var player = Content.Load<SpriteSheet>("perso.sf", new JsonContentLoader());
            var playerTexture = new AnimatedSprite(player);
            var enemy = Content.Load<SpriteSheet>("test/enemy.sf", new JsonContentLoader());
            enemyTexture = new AnimatedSprite(enemy);

            //creation du joueur
            _player = new List<Sprite>()
           {
               new Player(playerTexture)
               {
                   Position = new Vector2(10, 800),
               },
           };

           

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            //creation des rectangles de début et de fin + spawn des ennemis (en supprimant les ancien). Le tout juste lors des changements de map
            if (_switch)
            {
                _start.Clear();
                _enemys.Clear();
                int i = 2;
                int y = 2;
                while (i > 0)
                {
                    _start.Add(new Rectangle((int)_map[_mapEnCour].ObjectLayers[y].Objects[0].Position.X,
                                             (int)_map[_mapEnCour].ObjectLayers[y].Objects[0].Position.Y,
                                             (int)_map[_mapEnCour].ObjectLayers[y].Objects[0].Size.Width,
                                             (int)_map[_mapEnCour].ObjectLayers[y].Objects[0].Size.Height));
                    y++;
                    i--;
                }

                for (int j = 0; j < _map[_mapEnCour].ObjectLayers[0].Objects.Length; j++) //ajout des ennemis a des point predefinis sur la map
                {
                    _enemys.Add(
                    new Enemy(enemyTexture)
                    {
                        Position = _map[_mapEnCour].ObjectLayers[0].Objects[j].Position,
                    }
                    );
                }

                _switch = false;
            }

            //ennemi supprimer si sa vie est 0
            for (int i = 0; i < _enemys.Count; i++)
            {
                if (_enemys[i].Health == 0)
                {
                    _enemys.RemoveAt(i);
                    i--;
                }
            }

            //vie du joueur est égal à 0 ==> mort
            if (_player[0].Dead == true)
                _dead = true;

            //changement de map quand le joueur atteint le point de fin
            if (_player[0].Rectangle.Intersects(_start[0]))
            {
                _mapEnCour++;
                _player[0].Position = new Vector2(10, 800);
                _switch = true;
            }

            //maj du joueur et des ennemis
            foreach (Sprite sprite in _player)
                sprite.Update(gameTime, _map[_mapEnCour], "sol", "echelles", GraphicsDevice, _player[0]);

            foreach (Sprite enemy in _enemys)
                enemy.Update(gameTime, _map[_mapEnCour], "sol", "echelles", GraphicsDevice, _player[0]);

            //debug chagement de map
            if (keyboardState.IsKeyDown(Keys.E) && !_spam)
            {
                _spam = true;
                _mapEnCour++;
            }

            //debug changement de map sans avoir un changement trop rapide
            if (keyboardState.IsKeyUp(Keys.E))
                _spam = false;
        }

        public override void Draw(GameTime gameTime)
        {
            _game1.GraphicsDevice.Clear(Color.Black);

            _game1.SpriteBatch.Begin();

            //dessin des ennemis et du joueur
            foreach (var sprite in _player)
                sprite.Draw(_game1.SpriteBatch);

            foreach (var sprite in _enemys)
                sprite.Draw(_game1.SpriteBatch);

            //dessin de la map
            if(_mapEnCour < 5)
                _renduMap[_mapEnCour].Draw();
            else
            {
                _mapEnCour = 0;
            }

            //dessin de l'ecran de mort
            if (_dead == true)
            {
                _game1.ScreenManager.LoadScreen(_game1._screenMort);
                _player[0].Lives = 3;
                _dead = false;
            }



            //texte
            if (_game1._langue == Game1.Langue.English)
            {
                _game1.SpriteBatch.DrawString(_game1._fontLevel, $"Kill count : {_kill}", new Vector2(20, 20), Color.Black);
                _game1.SpriteBatch.DrawString(_game1._fontLevel, $"Lives : {_player[0].Lives}", new Vector2(20, 40), Color.Black);
            }
            else if (_game1._langue == Game1.Langue.French)
            {
                _game1.SpriteBatch.DrawString(_game1._fontLevel, $"Ennemis tues : {_kill}", new Vector2(20, 20), Color.Black);
                _game1.SpriteBatch.DrawString(_game1._fontLevel, $"Vies : {_player[0].Lives}", new Vector2(20, 40), Color.Black);
            }

            _game1.SpriteBatch.End();
        }
    }
}
