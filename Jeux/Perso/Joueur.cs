using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System.Diagnostics;

namespace Jeux.JoueurP
{
    public enum TypeAnimation { walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb };
    class Joueur
    {
        private Vector2 _position;

        private Rectangle _rectanglePerso;

        private AnimatedSprite _joueurP;

        private TypeAnimation _animation;

        Vector2 deplacement = Vector2.Zero;

        private bool idleRight;

        protected Vector2 velocity;

        protected float moveSpeed = 500f;

        protected float jumpSpeed = 1000f;

        protected bool jump = false;

        private Game1 _game1;

        TiledMapTileLayer layer;

        readonly Vector2 gravity = new Vector2(0, 600f);

        public Joueur(Vector2 position, AnimatedSprite joueurP)
        {
            this.Position = position;
            this.JoueurP = joueurP;
        }

        public Vector2 Position { get => this._position; set => this._position = value; }

        public Rectangle RectanglePerso { get => this._rectanglePerso; private set => this._rectanglePerso = value; }

        public TypeAnimation Animation { get => this._animation; set => this._animation = value; }

        public AnimatedSprite JoueurP { get => this._joueurP; set => this._joueurP = value; }

        public void Update(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice)
        {
            RectanglePerso = new Rectangle((int)Position.X, (int)Position.Y, JoueurP.TextureRegion.Width, JoueurP.TextureRegion.Height);

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float walkSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 100;

            KeyboardState keyboardState = Keyboard.GetState();

            float positionColonneJoueurP = (Position.X / _map.TileWidth);

            float positionLigneJoueurP = (Position.Y / _map.TileHeight);

            Vector2 deplacement = Vector2.Zero;

            /*ushort x = (ushort)(Position.X / _map.TileHeight);

            ushort y = (ushort)(Position.Y / _map.TileWidth);*/

            velocity.X = 0;

            bool collision;

            if (idleRight)
                Animation = TypeAnimation.idleRight;
            else
                Animation = TypeAnimation.idleLeft;

            bool toucheBordFenetre = false;

            //animation grimpe 
            if (IsCollision(positionColonneJoueurP, positionLigneJoueurP - 1, layerClimb, _map)
                && !IsCollision(positionColonneJoueurP, positionLigneJoueurP - 1, layerCollision, _map))
                Animation = TypeAnimation.idleClimb;


            //entrées clavier
            if (keyboardState.IsKeyDown(Keys.Up) && IsCollision(positionColonneJoueurP, positionLigneJoueurP - 1, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = Position.Y - JoueurP.TextureRegion.Height / 2 <= 0;
                collision = IsCollision(positionColonneJoueurP, positionLigneJoueurP - 1, layerCollision, _map);
                deplacement = -Vector2.UnitY;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && IsCollision(positionColonneJoueurP, positionLigneJoueurP, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = Position.Y + JoueurP.TextureRegion.Height / 2 >= graphicsDevice.Viewport.Height;
                collision = IsCollision(positionColonneJoueurP, positionLigneJoueurP + 1, layerCollision, _map);
                deplacement = Vector2.UnitY;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                Animation = TypeAnimation.walkLeft;
                toucheBordFenetre = Position.X - JoueurP.TextureRegion.Width / 2 <= 0;
                collision = IsCollision(positionColonneJoueurP - 1, positionLigneJoueurP, layerCollision, _map);
                deplacement = -Vector2.UnitX;
                idleRight = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                Animation = TypeAnimation.walkRight;
                toucheBordFenetre = Position.X + JoueurP.TextureRegion.Width / 2 >= graphicsDevice.Viewport.Width;
                collision = IsCollision(positionColonneJoueurP + 1, positionLigneJoueurP, layerCollision, _map);
                deplacement = Vector2.UnitX;
                idleRight = true;
            }
            else if (keyboardState.IsKeyDown(Keys.Space) && jump)
            {
                velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                jump = false;
            }

            if (IsCollision(positionColonneJoueurP, positionLigneJoueurP, layerCollision, _map)
               || !toucheBordFenetre
               || IsCollision(positionColonneJoueurP, positionLigneJoueurP - 1, layerClimb, _map)
               || IsCollision(positionColonneJoueurP, positionLigneJoueurP + 1, layerClimb, _map))
            {
                Position += walkSpeed * deplacement;
            }

            if ((!jump || !IsCollision(positionColonneJoueurP, positionLigneJoueurP, layerCollision, _map))
                && !toucheBordFenetre
                && !IsCollision(positionColonneJoueurP, positionLigneJoueurP - 1, layerClimb, _map)
                && !IsCollision(positionColonneJoueurP, positionLigneJoueurP + 1, layerClimb, _map))
                velocity.Y += gravity.Y * elapsedTime;
            else
                velocity.Y = 0;

            if (jump)
                Position = new Vector2(Position.X, 900);

            Debug.WriteLine($"Viwport : {graphicsDevice.Viewport.Width}" +
                $"JoueurP X : {Position.X}");

            deplacement = Vector2.Zero;

            JoueurP.Play(Animation.ToString());
            JoueurP.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private bool IsCollision(float x, float y, string layer, TiledMap _map)
        {
            TiledMapTile? tile;
            TiledMapTileLayer _obstacleLayer = _map.GetLayer<TiledMapTileLayer>(layer);
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
