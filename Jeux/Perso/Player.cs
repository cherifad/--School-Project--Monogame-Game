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
    public class Player : Sprite
    {
        bool idleRight = true, jump = true;
        public Player(AnimatedSprite texture) 
            : base(texture)
        {
        }

        public void Move(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice)
        {
            /*  #region Fields

              bool idleRight = true, collision = false, jump = true;
              KeyboardState keyboardState = Keyboard.GetState();
              float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
              float walkSpeed = elapsedTime * 100, jumpSpeed = elapsedTime * 10;

              #endregion

              #region Positions

              //le personnage ne peut sortir du bord de la fenêtre
              Position = Vector2.Clamp(Position, new Vector2(0 + this.Rectangle.Width, 0 + this.Rectangle.Height), 
                  new Vector2(Game1.ScreenWidth - this.Rectangle.Width, Game1.ScreenHeight - this.Rectangle.Height));
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

             /* if (IsCollision(positionColonnePlayer, positionLignePlayer, layerCollision, _map)
                 || IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerClimb, _map)
                 || IsCollision(positionColonnePlayer, positionLignePlayer + 1, layerClimb, _map))
              {
                  Position += walkSpeed * Velocity;
              }*/

            /* if ((!jump || !IsCollision(positionColonnePlayer, positionLignePlayer, layerCollision, _map))
                 && !IsCollision(positionColonnePlayer, positionLignePlayer - 1, layerClimb, _map)
                 && !IsCollision(positionColonnePlayer, positionLignePlayer + 1, layerClimb, _map))
                 Velocity.Y += Gravity.Y * elapsedTime;
             else
                 Velocity.Y = 0;

             Position += walkSpeed * Velocity;

             //  if (jump)
             //     Position = new Vector2(Position.X, 800);

             #endregion*/

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float walkSpeed = elapsedTime * 300, walkSpeedVirtuel = elapsedTime * 300;

            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 deplacement = Vector2.Zero;

            Vector2 positionVirtuelle = Vector2.Zero;

            //                _mapEnCour ++;

            /*for (int i = 0; i < 5; i++)
            {
                _positions[i] = (Position.X / _map[_mapEnCour].TileWidth);
            }*/

            float positionColonnePerso = (Position.X / _map.TileWidth);

            float positionLignePerso = ((Position.Y + _texture.TextureRegion.Height / 2) / _map.TileHeight);

            Velocity.X = 0;

            bool toucheBordFenetre = false;

            bool ecran = false;

            //animation idle
            if (idleRight)
                Animation = TypeAnimation.idleRight;
            else
                Animation = TypeAnimation.idleLeft;

            if (IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map)
                && !IsCollision(positionColonnePerso, positionLignePerso - 1, layerCollision, _map))
                Animation = TypeAnimation.idleClimb;


            //touche du haut + echelle
            if (keyboardState.IsKeyDown(Keys.Up) && (IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map) || IsCollision(positionColonnePerso, positionLignePerso, layerClimb, _map)))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = Position.Y - _texture.TextureRegion.Height / 2 <= 0;
                //Collision = IsCollision(positionColonnePerso, positionLignePerso - 1);
                deplacement = -Vector2.UnitY;
            } // touche du bas + echelle
            else if (keyboardState.IsKeyDown(Keys.Down) && IsCollision(positionColonnePerso, positionLignePerso , layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = Position.Y + _texture.TextureRegion.Height / 2 >= Game1.ScreenHeight;
                //Collision = IsCollision(positionColonnePerso, positionLignePerso + 1);
                deplacement = Vector2.UnitY;
            }//touche de droite + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Left) && jump)
            {
                Animation = TypeAnimation.walkLeft;
                toucheBordFenetre = Position.X - _texture.TextureRegion.Width / 2 <= 0;
                //Collision = IsCollision(positionColonnePerso - 1, positionLignePerso);
                deplacement = -Vector2.UnitX;
                idleRight = false;
                ecran = true;
            } //touche de gauche + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Right) && jump)
            {
                Animation = TypeAnimation.walkRight;
                toucheBordFenetre = Position.X + _texture.TextureRegion.Width / 2 >= Game1.ScreenWidth;
                //Collision = IsCollision(positionColonnePerso + 1, positionLignePerso);
                deplacement = Vector2.UnitX;
                idleRight = true;
                ecran = true;

            } // saut + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Space) && jump)
            {
                Animation = TypeAnimation.jumpLeft;
                Velocity.Y = -100f;
            }
            else if (keyboardState.IsKeyDown(Keys.X))
            {
                if (idleRight) Animation = TypeAnimation.hitRight;
                else Animation = TypeAnimation.hitLeft;
            }

            /* // if (IsCollision(positionColonnePerso + 1, positionLignePerso, "sol"))
              {
                  deplacement = Vector2.Zero;
              }*/

            //deplacement
            if (IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map)
                || !toucheBordFenetre
                || IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map)
                || IsCollision(positionColonnePerso, positionLignePerso + 1, layerClimb, _map))
            {
                Position += walkSpeed * deplacement;
            }


            // gravité si pas en colision avec le sol et pas de saut
            if ((!jump || !IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map))
                && !toucheBordFenetre
                && !IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map)
                && !IsCollision(positionColonnePerso, positionLignePerso + 1, layerClimb, _map))
            {
                Velocity.Y += Gravity.Y * elapsedTime;
            }
            /*else if (IsCollision(positionColonnePerso, positionLignePerso, "echelles"))
                Velocity.Y = 0;*/
            else
                Velocity.Y = 0;

            if (Position.Y > Game1.ScreenHeight)
                Position = Vector2.Zero;

            //si en colision avec le sol, il peut sauter
            if (IsCollision(positionColonnePerso, positionLignePerso + 1, layerCollision, _map))
                jump = true;

            Velocity.X = 0;

            Position += Velocity * elapsedTime;

            _texture.Play(Animation.ToString());
            _texture.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

       public override void Update(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice, Sprite player)
        {
            Move(gameTime, _map, layerCollision, layerClimb, graphicsDevice);            
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
