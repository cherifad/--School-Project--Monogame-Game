using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeux.Perso
{
    public class Enemy : Sprite
    {
        public Enemy(AnimatedSprite texture) 
            : base(texture)
        {
        }
        private Vector2 deplacement = Vector2.Zero;

        Random random = new Random();
        public bool _visible = true;

        int _ranX, _ranY;

        public static void Play(Enemy enemy, AnimatedSprite joueur, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = elapsedTime * 300;

            if (enemy.Position.Y == joueur.TextureRegion.Y)
            {
                walkSpeed += 100;
                if (Math.Abs(enemy.Position.X - joueur.TextureRegion.X) == 2)
                    //frappe le joueur;
                    enemy.deplacement = Vector2.Zero;
                else if (enemy.Position.X > joueur.TextureRegion.X)
                    enemy.deplacement = -Vector2.UnitX;
                else if (enemy.Position.X < joueur.TextureRegion.X)
                    enemy.deplacement = Vector2.UnitX;
            }
            else
                enemy.deplacement = Vector2.UnitX;


            enemy.Position += walkSpeed * enemy.deplacement;
        }


    }
}
