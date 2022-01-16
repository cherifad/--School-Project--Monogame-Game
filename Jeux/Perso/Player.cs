using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Perso
{
    public enum TypeAnimation { walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb };
    class Player : Sprite
    {
        public Player(AnimatedSprite texture) 
            : base(texture)
        {
        }

        private void Move(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice)
        {
            #region Fields
            
            bool idleRight = true, collision = false, jump = true;
            KeyboardState keyboardState = Keyboard.GetState();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = elapsedTime * 100, jumpSpeed = elapsedTime * 10;

            #endregion

            #region Positions

            //le personnage ne peut sortir du bord de la fenêtre
            Position.X = MathHelper.Clamp(Position.X, 0, Game1.ScreenWidth - Rectangle.Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, Game1.ScreenHeight - Rectangle.Height);

            //position du personnage sur la carte
            float positionColonnePlayer = (Position.X / _map.TileWidth);
            float positionLignePlayer = (Position.Y / _map.TileHeight);

            #endregion

            #region Animation au repos
            //Grimpe
            if (IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerClimb, _map)
               && !IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerCollision, _map))
                Animation = TypeAnimation.idleClimb;

            //debout gauche ou droite
            if (idleRight)
                Animation = TypeAnimation.idleRight;
            else
                Animation = TypeAnimation.idleLeft;
            #endregion

            #region Entree joueur

            //entrées clavier touche du haut
            if (keyboardState.IsKeyDown(Keys.Up) 
                && IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                //toucheBordFenetre = Position.Y - Player.TextureRegion.Height / 2 <= 0;
                collision = IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerCollision, _map);
                Velocity = -Vector2.UnitY;
            } //touche du bas
            else if (keyboardState.IsKeyDown(Keys.Down) 
                && IsCollision(positionColonnePlayer, positionLignePlayer, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                //toucheBordFenetre = Position.Y + Player.TextureRegion.Height / 2 >= graphicsDevice.Viewport.Height;
                collision = IsCollision(positionColonnePlayer, positionLignePlayer + 1, layerCollision, _map);
                Velocity = Vector2.UnitY;
            } //touche gauche
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                Animation = TypeAnimation.walkLeft;
                //toucheBordFenetre = Position.X - Player.TextureRegion.Width / 2 <= 0;
                collision = IsCollision(positionColonnePlayer - 1, positionLignePlayer, layerCollision, _map);
                Velocity = -Vector2.UnitX;
                idleRight = false;
            } //touche droite
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                Animation = TypeAnimation.walkRight;
                //toucheBordFenetre = Position.X + Player.TextureRegion.Width / 2 >= graphicsDevice.Viewport.Width;
                collision = IsCollision(positionColonnePlayer + 1, positionLignePlayer, layerCollision, _map);
                Velocity = Vector2.UnitX;
                idleRight = true;
            } //saut (espace)
            else if (keyboardState.IsKeyDown(Keys.Space) && jump)
            {
                Velocity.Y = -jumpSpeed;
                jump = false;
            }

            #endregion

            #region deplacement

            if (IsCollision(positionColonnePlayer, positionLignePlayer, layerCollision, _map)
               || IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerClimb, _map)
               || IsCollision(positionColonnePlayer, positionLignePlayer + 1, layerClimb, _map))
            {
                Position += walkSpeed * Velocity;
            }

            if ((!jump || !IsCollision(positionColonnePlayer, positionLignePlayer, layerCollision, _map))
                && !IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerClimb, _map)
                && !IsCollision(positionColonnePlayer, positionLignePlayer + 1, layerClimb, _map))
                Velocity.Y += Gravity.Y * elapsedTime;
            else
                Velocity.Y = 0;

            if (jump)
                Position = new Vector2(Position.X, 900);

            #endregion

            Velocity.X = 0;
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
