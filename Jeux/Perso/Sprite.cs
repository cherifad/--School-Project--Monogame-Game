using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;

namespace Jeux.Perso
{
    public class Sprite
    {
        protected AnimatedSprite _texture;

        //position
        public Vector2 Position;

        //velocité vitesse
        public Vector2 Velocity = Vector2.Zero;
        public float Speed;

        // public Input Input;

        public bool IsRemoved = false;

        //animation
        private TypeAnimation _animation;
        public enum TypeAnimation
        {
            walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb,
            enemyWalkLeft, enemyWalkRight, enemyHitLeft, enemyHitRight,
            witchWalkLeft, witchWalkRight, witchHitLeft, witchHitRight
        };
        public TypeAnimation Animation { get => this._animation; set => this._animation = value; }


        //vie
        private int _health;


        public Vector2 Gravity
        {
            get
            {
                return new Vector2(0, 600f);
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.TextureRegion.Width, _texture.TextureRegion.Height);
            }
        }

        //ajout de ce que adlen a fait

        /*public List<Rectangle> HealhtBar
{
    get
    {
        return HealhtBar;
    }
    set
    {
        HealhtBar = new List<Rectangle>();
        for (int i = 0; i < Health; i++)
        {
            HealhtBar.Add(new Rectangle(Rectangle.X + i * 5, Rectangle.Y - 1, 5, 5));
        }
    }
}*/

        public Sprite(AnimatedSprite texture)
        {
            _texture = texture;
        }

        //vie
        public int Health { get => this._health; protected set => this._health = value; }

        public virtual void Update(GameTime gameTime, TiledMap _map, string layerCollision, string layerClimb, GraphicsDevice graphicsDevice, Sprite player)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position);
        }
    }
}