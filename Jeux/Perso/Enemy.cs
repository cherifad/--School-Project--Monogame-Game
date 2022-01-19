using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeux.Perso
{

    class Enemy : Sprite
    {
        public Enemy(AnimatedSprite texture) 
            : base(texture)
        {
            Health = 5;
        }

        private Vector2 deplacement = Vector2.Zero;

        protected bool hit = false;

        protected bool _visible = true, _spam = false;

        public void Move(GameTime gameTime, TiledMap _map, string layerCollision, GraphicsDevice graphicsDevice, Sprite player)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = elapsedTime * 150;

            float positionColonnePerso = (Position.X / _map.TileWidth);

            float positionLignePerso = ((this.Position.Y + this._texture.TextureRegion.Height / 2) / _map.TileHeight);

            int timer = 1;

            KeyboardState keyboardState = Keyboard.GetState();

            Velocity.X = 0;

            int end = (int)player.Position.Y + 5;
            int start = (int)player.Position.Y - 10;

            if (Enumerable.Range(start, end).Contains(Rectangle.Y))
            {
                walkSpeed = elapsedTime * 200;
                if (Math.Abs(Position.X - player.Position.X) < 2 && keyboardState.IsKeyDown(Keys.X) && !_spam)
                {
                    Health--;
                    deplacement = Vector2.Zero;
                    _spam = true;
                }
                else if (Math.Abs(Position.X - player.Position.X) < 2 && keyboardState.IsKeyUp(Keys.X) && timer > 0)
                    {
                        hit = true;
                        deplacement = Vector2.Zero;
                        timer -= (int)elapsedTime;
                    }
                else if (Position.X > player.Position.X)
                {
                    Animation = TypeAnimation.enemyWalkLeft;
                    deplacement = -Vector2.UnitX;
                }
                else if (Position.X < player.Position.X)
                {
                    deplacement = Vector2.UnitX;
                    Animation = TypeAnimation.enemyWalkRight;
                }
            }
            else
            {
                if (!IsCollision(positionColonnePerso + Rectangle.Width, positionLignePerso, layerCollision, _map))
                    deplacement += -Vector2.UnitX;
                if (!IsCollision(positionColonnePerso - Rectangle.Width, positionLignePerso, layerCollision, _map))
                    deplacement += Vector2.UnitX;
            }

            hit = false;

            if (keyboardState.IsKeyUp(Keys.X))
                _spam = false;

//                    deplacement = -Vector2.UnitX;


            if ((!IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map))
               )
            {
                Velocity.Y += Gravity.Y * elapsedTime;
            }
            else
                Velocity.Y = 0;

           

            Position += Velocity * elapsedTime;

            Position += walkSpeed * deplacement;

          // this._texture.Play(this.Animation.ToString());
          // this._texture.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Update(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice, Sprite player)
        {
            Move(gameTime, _map, layerCollision, graphicsDevice, player);
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
