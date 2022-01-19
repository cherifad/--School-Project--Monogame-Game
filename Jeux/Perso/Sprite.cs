using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;

namespace Jeux.Perso
{
    public class Sprite
    {
        private AnimatedSprite _texture;

        //position
        public Vector2 Position;

        //velocité vitesse
        public Vector2 Velocity = Vector2.Zero;
        public float Speed;

        // public Input Input;

        public bool IsRemoved = false, hit = false;

        //animation
        private TypeAnimationPerso _animationP;
        private TypeAnimationEnnemi _animationE;
        public enum TypeAnimationPerso
        {
            walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb            
        };

        public enum TypeAnimationEnnemi
        {
            enemyWalkLeft, enemyWalkRight, enemyHitLeft, enemyHitRight,
            witchWalkLeft, witchWalkRight, witchHitLeft, witchHitRight
        };


        public TypeAnimationPerso AnimationP { get => this._animationP; set => this._animationP = value; }

        public TypeAnimationEnnemi AnimationE { get => this._animationE; set => this._animationE = value; }



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

        public AnimatedSprite Texture
        {
            get
            {
                return this._texture;
            }
            set
            {
                this._texture = value;
            }
        }

        //ajout de ce que adlen a fait

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