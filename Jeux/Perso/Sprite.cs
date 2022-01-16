using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System.Collections.Generic;

namespace Jeux.Perso
{
    public class Sprite
    {
        protected AnimatedSprite _texture;

        public Vector2 Position;
        public Vector2 Velocity = Vector2.Zero;
        public float Speed;
        // public Input Input;
        public bool IsRemoved = false;
        private TypeAnimation _animation;

        public TypeAnimation Animation { get => this._animation; set => this._animation = value; }

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

        public Sprite(AnimatedSprite texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position);
        }
    }
}