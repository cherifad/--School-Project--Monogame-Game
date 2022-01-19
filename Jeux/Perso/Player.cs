﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Perso
{
    public class Player : Sprite
    {
        bool idleRight = true, jump = true;
        public Player(AnimatedSprite texture) 
            : base(texture)
        {
            Health = 10;
        }

        public void Move(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice)
        {

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float walkSpeed = elapsedTime * 300, walkSpeedVirtuel = elapsedTime * 300;

            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 deplacement = Vector2.Zero;

            Vector2 positionVirtuelle = Vector2.Zero;

            float positionColonnePerso = (Position.X / _map.TileWidth);

            float positionLignePerso = ((Position.Y + _texture.TextureRegion.Height / 2) / _map.TileHeight);

            Velocity.X = 0;

            bool toucheBordFenetre = false;

            //animation idle
            if (idleRight)
                Animation = TypeAnimation.idleRight;
            else
                Animation = TypeAnimation.idleLeft;

          /*  if (IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map)
                && !IsCollision(positionColonnePerso, positionLignePerso - 1, layerCollision, _map))
                Animation = TypeAnimation.idleClimb;*/

            //si le joueur est frappé


            //touche du haut + echelle
            if (keyboardState.IsKeyDown(Keys.Up) && IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map))
            {
                Animation = TypeAnimation.climb;
                toucheBordFenetre = Position.Y - _texture.TextureRegion.Height / 2 <= 0;
                //Collision = IsCollision(positionColonnePerso, positionLignePerso - 1);
                deplacement = -Vector2.UnitY;
            } // touche du bas + echelle
            else if (keyboardState.IsKeyDown(Keys.Down) && IsCollision(positionLignePerso, positionColonnePerso + 1, layerClimb, _map))
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
            } //touche de gauche + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Right) && jump)
            {
                Animation = TypeAnimation.walkRight;
                toucheBordFenetre = Position.X + _texture.TextureRegion.Width / 2 >= Game1.ScreenWidth;
                //Collision = IsCollision(positionColonnePerso + 1, positionLignePerso);
                deplacement = Vector2.UnitX;
                idleRight = true;

            } // saut + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Space) && jump)
            {
                Animation = TypeAnimation.jumpLeft;
                deplacement.Y = -200f;
                jump = false;
            }
            else if (keyboardState.IsKeyDown(Keys.X))
            {
                if (idleRight) Animation = TypeAnimation.hitRight;
                else Animation = TypeAnimation.hitLeft;
            }

            if (//!IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map)
            IsCollision(positionColonnePerso, positionLignePerso + 1, layerClimb, _map))
            {
                Animation = TypeAnimation.idleClimb;
                deplacement.X = 0;
            }

            //deplacement
            if (IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map)
                || !toucheBordFenetre
                || IsCollision(positionColonnePerso, positionLignePerso - _map.TileHeight, layerClimb, _map)
                || IsCollision(positionColonnePerso, positionLignePerso + 1, layerClimb, _map))
            {
                Position += walkSpeed * deplacement;
            }

            /*      if (!IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map)
               || IsCollision(positionColonnePerso, positionLignePerso + 1, layerClimb, _map))
                deplacement.X = 0;*/


            if (IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map) && keyboardState.IsKeyUp(Keys.Space))
                jump = true;

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
            {
                Health = 0;
                Position = Vector2.Zero;
            }


            Velocity.X = 0;

            Position += Velocity * elapsedTime;

            Console.WriteLine(IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map));

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
