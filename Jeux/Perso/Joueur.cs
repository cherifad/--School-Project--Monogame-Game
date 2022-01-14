using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System.Diagnostics;

namespace Jeux.Perso
{
    public enum TypeAnimation { walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight };
    class Joueur
    {
        private string _animatedSprite;

        private Vector2 _position;

        private AnimatedSprite _joueurP;

        private TypeAnimation _animation;

        Vector2 deplacement = Vector2.Zero;

        private int coef, speed = 5;

        private bool idleRight;

        protected Vector2 velocity;

        protected const float gravity = 50f;

        protected float moveSpeed = 500f;

        protected float jumpSpeed = 1000f;

        protected bool jump = false;

        private Game1 _game1;

        TiledMapTileLayer layer;


        public Joueur(string animatedSprite, Vector2 position)
        {
            this.AnimatedSprite = animatedSprite;
            this.Position = position;
        }

        public string AnimatedSprite { get => this._animatedSprite; set => this._animatedSprite = value; }
        public Vector2 Position { get => this._position; set => this._position = value; }

        public TypeAnimation Animation
        {
            get
            {
                return this._animation;
            }

            set
            {
                this._animation = value;
            }
        }

        public AnimatedSprite JoueurP { get => this._joueurP; set => this._joueurP = value; }

        public void Create(Game screen)
        {
            SpriteSheet animation = screen.Content.Load<SpriteSheet>(this.AnimatedSprite + ".sf", new JsonContentLoader());
            JoueurP = new AnimatedSprite(animation);
        }

        public void Move(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb)
        {
            TiledMapTile? tile;

            float walkSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 100;

            KeyboardState keyboardState = Keyboard.GetState();

            float positionColonnePerso = (Position.X / _map.TileWidth);

            float positionLignePerso = (Position.Y / _map.TileHeight);

            ushort x = (ushort)(Position.X / _map.TileHeight);

            ushort y = (ushort)(Position.Y / _map.TileWidth);

            velocity.X = 0;

          //  layer.TryGetTile(x, y, out tile);

            bool collision;


            //Console.WriteLine("position en pixel du perso :" + _game1.PositionPerso.X + "," + _game1.PositionPerso.Y);
            //Console.WriteLine("position dans la matrice du perso :" + positionColonnePerso + "," + positionLignePerso);

            if (idleRight)
                Animation = TypeAnimation.idleRight;
            else
                Animation = TypeAnimation.idleLeft;

            bool toucheBordFenetre = false;

            if (keyboardState.IsKeyDown(Keys.Up) && IsCollision(positionColonnePerso, positionLignePerso, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = _game1.PositionPerso.Y - _game1.Perso.TextureRegion.Height / 2 <= 0;
                collision = IsCollision(positionColonnePerso, positionLignePerso - 1, layerCollision, _map);
                deplacement = -Vector2.UnitY;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && IsCollision(positionColonnePerso, positionLignePerso, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = _game1.PositionPerso.Y + _game1.Perso.TextureRegion.Height / 2 >= _game1.GraphicsDevice.Viewport.Height;
                collision = IsCollision(positionColonnePerso, positionLignePerso + 1, layerCollision, _map);
                deplacement = 2 * Vector2.UnitY;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                Animation = TypeAnimation.walkLeft;
                toucheBordFenetre = _game1.PositionPerso.X - _game1.Perso.TextureRegion.Width / 2 <= 0;
                collision = IsCollision(positionColonnePerso - 1, positionLignePerso, layerCollision, _map);
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                idleRight = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                Animation = TypeAnimation.walkRight;
                toucheBordFenetre = _game1.PositionPerso.X + _game1.Perso.TextureRegion.Width / 2 >= _game1.GraphicsDevice.Viewport.Width;
                collision = IsCollision(positionColonnePerso + 1, positionLignePerso, layerCollision, _map);
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                idleRight = true;
            }
            else if (keyboardState.IsKeyDown(Keys.Space) && jump)
            {
                velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                jump = false;
            }

           /* if (tile.HasValue && !toucheBordFenetre)
            {
                _game1.PositionPerso += walkSpeed * deplacement;
            }*/

            while (!IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map))
            {
                if (!jump)
                    velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }               

            Position += velocity;

            jump = Position.Y <= _game1.GraphicsDevice.Viewport.Height;

            if (jump)
                Position = new Vector2(_game1.PositionPerso.X, 900);

            Debug.WriteLine($"Viwport : {_game1.GraphicsDevice.Viewport.Width}" +
                $"Perso X : {_game1.PositionPerso.X}");

            deplacement = Vector2.Zero;

            JoueurP.Play(Animation.ToString());
            JoueurP.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private bool IsCollision(float x, float y, string layer, TiledMap _map)
        {
            TiledMapTile? tile;
            TiledMapTileLayer _obstacleLayer = _map.GetLayer<TiledMapTileLayer>("sol");
            _obstacleLayer = _map.GetLayer<TiledMapTileLayer>(layer);
            if (_obstacleLayer.TryGetTile((ushort)x, (ushort)y, out tile) == false)
            {
                return false;
            }
            if (!tile.Value.IsBlank)
            {
                return true;
            }
            return false;
        }

       
    }
}
