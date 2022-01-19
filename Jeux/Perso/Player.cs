using Jeux.Screen;
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
    public enum TypeAnimationPerso { walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb };
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
            //deplacement
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = elapsedTime * 300, walkSpeedVirtuel = elapsedTime * 300;
            Vector2 deplacement = Vector2.Zero;

            //clavier
            KeyboardState keyboardState = Keyboard.GetState();

            //position
            Vector2 positionVirtuelle = Vector2.Zero;
            float positionColonnePerso = (Position.X / _map.TileWidth);
            float positionLignePerso = ((Position.Y + Texture.TextureRegion.Height / 2) / _map.TileHeight);

            //velocité
            Velocity.X = 0;

            //bord
            bool toucheBordFenetre = false;


            //AnimationP idle
            if (idleRight)
                AnimationP = TypeAnimationPerso.idleRight;
            else
                AnimationP = TypeAnimationPerso.idleLeft;

          if (IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map)
              && !IsCollision(positionColonnePerso, positionLignePerso - 1, layerCollision, _map))
                AnimationP = TypeAnimationPerso.idleClimb;


            //si le joueur est frappé
            if (hit)
                Health--;

            //touche du haut + echelle
            if (keyboardState.IsKeyDown(Keys.Up) && (IsCollision(positionColonnePerso, positionLignePerso - 1, layerClimb, _map) || IsCollision(positionColonnePerso, positionLignePerso, layerClimb, _map)))
            {
                AnimationP = TypeAnimationPerso.climb;
                toucheBordFenetre = Position.Y - Texture.TextureRegion.Height / 2 <= 0;
                //Collision = IsCollision(positionColonnePerso, positionLignePerso - 1);
                deplacement = -Vector2.UnitY;
            } // touche du bas + echelle
            else if (keyboardState.IsKeyDown(Keys.Down) && IsCollision(positionColonnePerso, positionLignePerso , layerClimb, _map))
            {
                AnimationP = TypeAnimationPerso.climb;
                toucheBordFenetre = Position.Y + Texture.TextureRegion.Height / 2 >= Game1.ScreenHeight;
                //Collision = IsCollision(positionColonnePerso, positionLignePerso + 1);
                deplacement = Vector2.UnitY;
            }//touche de droite + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Left) && jump)
            {
                AnimationP = TypeAnimationPerso.walkLeft;
                toucheBordFenetre = Position.X - Texture.TextureRegion.Width / 2 <= 0;
                //Collision = IsCollision(positionColonnePerso - 1, positionLignePerso);
                deplacement = -Vector2.UnitX;
                idleRight = false;
            } //touche de gauche + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Right) && jump)
            {
                AnimationP = TypeAnimationPerso.walkRight;
                toucheBordFenetre = Position.X + Texture.TextureRegion.Width / 2 >= Game1.ScreenWidth;
                //Collision = IsCollision(positionColonnePerso + 1, positionLignePerso);
                deplacement = Vector2.UnitX;
                idleRight = true;

            } // saut + pas de saut
            else if (keyboardState.IsKeyDown(Keys.Space) && jump)
            {
                AnimationP = TypeAnimationPerso.jumpLeft;
                Velocity.Y = -100f;
            }//hit
            else if (keyboardState.IsKeyDown(Keys.X))
            {
                if (idleRight) AnimationP = TypeAnimationPerso.hitRight;
                else AnimationP = TypeAnimationPerso.hitLeft;
            }

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
            {
                Health = 0;
                Position = Vector2.Zero;
            }

            //si en colision avec le sol, il peut sauter
            if (IsCollision(positionColonnePerso, positionLignePerso + 1, layerCollision, _map))
                jump = true;

            Velocity.X = 0;

            Position += Velocity * elapsedTime;

            if (Position.Y > Game1.ScreenHeight)
                Health = 0;

            //vie 
            if (Lives != 0 && this.Health == 0)
            {
                this.Position = Vector2.Zero;
                this.Health = 10;
                Lives--;
                this.Dead = false;
            }
            if (Lives == 0)
            {
                this.Dead = true;
            }


            //affichage
            Texture.Play(AnimationP.ToString());
            Texture.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
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
