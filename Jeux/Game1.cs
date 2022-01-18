using Jeux.Perso;
using Jeux.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;

namespace Jeux
{
    public enum Ecran { Home, Level1 };
        public enum TypeAnimation { walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb };
        public class Game1 : Game
        {
            private GraphicsDeviceManager _graphics;

            private SpriteBatch _spriteBatch;

            private AnimatedSprite _perso, _ennemys;

        private List<AnimatedSprite> _ennemy = new List<AnimatedSprite>();

            private TypeAnimation _animation, _animationE;

            private Vector2 _positionPerso, _positionEnemy;

            private Level _screenLevel1;

            private Home _screenHome;

        private Test _screentest;

            private Ecran _currentScreen;
        
            public Rectangle mSelectionBox;

            public MouseState mPreviousMouseState;

        public static int ScreenWidth, ScreenHeight;

      //  private Joueur _joueur;



        private readonly ScreenManager _screenManager;

            public SpriteBatch SpriteBatch
            {
                get
                {
                    return this._spriteBatch;
                }

                set
                {
                    this._spriteBatch = value;
                }
            }

            public AnimatedSprite Perso
            {
                get
                {
                    return this._perso;
                }

                set
                {
                    this._perso = value;
                }
            }

            public Vector2 PositionPerso
            {
                get
                {
                    return this._positionPerso;
                }

                set
                {
                    this._positionPerso = value;
                }
            }

            public TypeAnimation Animation
            {
                get
                {
                    return this._animation;
                }

                set
                {
                    this._animation = value;
                }
            }

            public GraphicsDeviceManager Graphics
            {
                get
                {
                    return this._graphics;
                }

                set
                {
                    this._graphics = value;
                }
            }

            public Game1()
            {
                Graphics = new GraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
                IsMouseVisible = true;
                _screenManager = new ScreenManager();
                Components.Add(_screenManager);

            }

            protected override void Initialize()
            {
            // TODO: Add your initialization logic here
            //        var _positionPerso = Vector2.Zero; //new Vector2(Level1.WIDTH_FENETRE / 2, Level1.HEIGHT_FENETRE / 2);
            var _positionPerso = new Vector2(10, 10);
            PositionE = new Vector2(0, 868);
            _positionEnemy.X = MathHelper.Clamp(PositionE.X, 0, Game1.ScreenWidth);
            _positionEnemy.Y = MathHelper.Clamp(PositionE.Y, 0, Game1.ScreenHeight);
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 992;
            //Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();

            ScreenWidth = Graphics.PreferredBackBufferWidth;
            ScreenHeight = Graphics.PreferredBackBufferHeight;

            mSelectionBox = new Rectangle(-1, -1, 0, 0);
            //Initialize the previous mouse state. This stores the current state of the mouse
            mPreviousMouseState = Mouse.GetState(); 

            base.Initialize();
            }

            protected override void LoadContent()
            {
                SpriteBatch = new SpriteBatch(GraphicsDevice);

                SpriteSheet animation = Content.Load<SpriteSheet>("perso.sf", new JsonContentLoader());
            SpriteSheet animationE = Content.Load<SpriteSheet>("test/enemy.sf", new JsonContentLoader());
                Perso = new AnimatedSprite(animation);
            Enemysolo = new AnimatedSprite(animationE);

                _screenHome = new Home(this);
                _screenLevel1 = new Level(this);
            _screentest = new Test(this);
            //_screenManager.LoadScreen(_screentest, new FadeTransition(GraphicsDevice, Color.Black));
            _screenManager.LoadScreen(_screenHome, new FadeTransition(GraphicsDevice, Color.Black));
                _currentScreen = Ecran.Home;

                // TODO: use this.Content to load your game content here
            }

            protected override void Update(GameTime gameTime)
            {

            if(_screenHome.start)
                _screenManager.LoadScreen(_screenLevel1, new FadeTransition(GraphicsDevice, Color.Black));

            if(_screenHome.exit)
                Exit();

            if(_screenHome.settings)
                _screenManager.LoadScreen(_screentest, new FadeTransition(GraphicsDevice, Color.Black));

            

            base.Update(gameTime);
            }

        public List<AnimatedSprite> Enemy
        {
            get
            {
                return this._ennemy;
            }

            set
            {
                this._ennemy = value;
            }
        }

        public AnimatedSprite Enemysolo
        {
            get
            {
                return this._ennemys;
            }

            set
            {
                this._ennemys = value;
            }
        }

        public Vector2 PositionE
        {
            get
            {
                return this._positionEnemy;
            }

            set
            {
                this._positionEnemy = value;
            }
        }

        public TypeAnimation AnimationE
        {
            get
            {
                return this._animationE;
            }

            set
            {
                this._animationE = value;
            }
        }

        protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(Color.Black);

                // TODO: Add your drawing code here

                base.Draw(gameTime);
            }
        }
}
