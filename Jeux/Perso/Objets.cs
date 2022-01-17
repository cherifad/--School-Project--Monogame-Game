using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using Jeux;
using Microsoft.Xna.Framework.Graphics;

namespace Jeux.Perso
{
    public class Objets : Sprite
    {
        private enum TypeAnimation { animCoeur, fullVie, noLife };

        public Objets(AnimatedSprite texture) : base(texture)
        {
        }

        public void Mort (AnimatedSprite joueur, GameTime gametime)
        {
            //si le perso tombe
            if (joueur.TextureRegion.Y > 1000)
            {
                Animation = (Perso.TypeAnimation)TypeAnimation.animCoeur; //marche pas
            }
        }

    }
}
