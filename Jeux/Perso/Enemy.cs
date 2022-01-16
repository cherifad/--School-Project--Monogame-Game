using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
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


        private Vector2 _position;
        private Vector2 _velocity;
        private AnimatedSprite _texture;
        private Vector2 deplacement = Vector2.Zero;

        Random random = new Random();
        public bool _visible = true;

        int _ranX, _ranY;

        public void Play(Player joueur, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = elapsedTime * 300;

            if (Position.Y == joueur.Position.Y)
            {
                walkSpeed += 100;
                if (Math.Abs(Position.X - joueur.Position.X) == 2)
                    //frappe le joueur;
                    deplacement = Vector2.Zero;
                else if (Position.X > joueur.Position.X)
                    deplacement = -Vector2.UnitX;
                else if (Position.X < joueur.Position.X)
                    deplacement = Vector2.UnitX;
            }
            else
                deplacement = Vector2.UnitX;


            Position += walkSpeed * deplacement;
        }
        
        public Vector2 Position { get => this._position; set => this._position = value; }
        public AnimatedSprite Texture { get => this._texture; set => this._texture = value; }
    }
}
