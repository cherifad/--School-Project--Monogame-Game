using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jeux.Controles
{
    public class Boutons : Components
    {
        #region Fields

        private MouseState _sourisEnCours;

        private SpriteFont _font;

        private bool _isHovering;

        private MouseState _sourisPrecedente;

        private Texture2D _texture;

        //?
        private Rectangle _bouttonRectangle;

        private Texture2D _whiteRectangle;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        //couleur font
        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        //click
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        //constructeur
        public Boutons(Texture2D texture, SpriteFont font)
        {
            _texture = texture;

            _font = font;

            PenColour = Color.Black;
        }

        public Boutons(SpriteFont font)
        {
            _font = font;

            PenColour = Color.White;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            var colour = Color.White;

            //quand la souris passe sur le bouton
            if (_isHovering)
                colour = Color.LightGray;

            spriteBatch.Draw(_texture, Rectangle, colour);

            //si il y a du texte sur le bouton
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }

        }

        public override void Update(GameTime gameTime)
        {
            _sourisPrecedente = _sourisEnCours;
            _sourisEnCours = Mouse.GetState();

            var mouseRectangle = new Rectangle(_sourisEnCours.X, _sourisEnCours.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_sourisEnCours.LeftButton == ButtonState.Released && _sourisPrecedente.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}
