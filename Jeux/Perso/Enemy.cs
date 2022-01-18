using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Perso
{
    class Enemy : Sprite
    {
        public Enemy(AnimatedSprite texture) 
            : base(texture)
        {
        }

        private Vector2 deplacement = Vector2.Zero;

        Random random = new Random();
        public bool _visible = true;

        int _ranX, _ranY;

        public void Move(GameTime gameTime, TiledMap _map, string layerCollision, GraphicsDevice graphicsDevice, Sprite player)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = elapsedTime * 300;

            float positionColonnePerso = (Position.X / _map.TileWidth);

            float positionLignePerso = ((this.Position.Y + this._texture.TextureRegion.Height / 2) / _map.TileHeight);

            Velocity.X = 0;

            if (this.Position.Y > player.Position.Y)
            {
                walkSpeed += 100;
                if (Math.Abs(Position.X - player.Position.X) < 2)
                    //frappe le joueur;
                    deplacement = Vector2.Zero;
                else if (Position.X > player.Position.X)
                    deplacement = -Vector2.UnitX;
                else if (Position.X < player.Position.X)
                    deplacement = Vector2.UnitX;
            }
            else
                deplacement = Vector2.UnitX;

            if ((!IsCollision(positionColonnePerso, positionLignePerso, layerCollision, _map))
               )
            {
                Velocity.Y += Gravity.Y * elapsedTime;
            }
            else
                Velocity.Y = 0;

            Position += Velocity * elapsedTime;

            Position += walkSpeed * deplacement;

            Console.WriteLine((this.Position.Y == player.Position.Y));
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
