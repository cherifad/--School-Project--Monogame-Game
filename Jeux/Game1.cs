using Jeux.Perso;
using Jeux.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;

namespace Jeux
{
        public enum TypeAnimation { walkRight, walkLeft, climb, hitLeft, hitRight, jumpLeft, jumpRight, idleLeft, idleRight, idleClimb };
        public class Game1 : Game
        {
            private GraphicsDeviceManager _graphics;

            private SpriteBatch _spriteBatch;

            private AnimatedSprite _perso;

            private TypeAnimation _animation;

            private Vector2 _positionPerso;

            //
            public enum Ecran { Home, Level1 };
            public Level _screenLevel1;
            public Home _screenHome;
            public Parametres _screenParametre;
            public Rules _screenRules;
            public Rules2 _screenRules2;
            private Test _screentest;

            private Ecran _currentScreen;
        
            public Rectangle mSelectionBox;

            public MouseState mPreviousMouseState;

            public static int ScreenWidth, ScreenHeight;

            //langues
            public enum Langue { French, English };
            public Langue _langue;

            //police
            public SpriteFont _fontTitle;
            public SpriteFont _font2;
            public SpriteFont _fontStart;
            public SpriteFont _fontExit;

            //musique
            public Song _music;

            private readonly ScreenManager _screenManager;

        public Langue Langue1
        {
            get
            {
                return this._langue;
            }

            set
            {
                this._langue = value;
            }
        }
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

        public ScreenManager ScreenManager
        {
            get
            {
                return this._screenManager;
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
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 992;
            //Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();

            ScreenWidth = Graphics.PreferredBackBufferWidth;
            ScreenHeight = Graphics.PreferredBackBufferHeight;

            mSelectionBox = new Rectangle(-1, -1, 0, 0);
            //Initialize the previous mouse state. This stores the current state of the mouse
            mPreviousMouseState = Mouse.GetState();

            //langue
            _langue = Langue.English;

            base.Initialize();
            }

            protected override void LoadContent()
            {
                SpriteBatch = new SpriteBatch(GraphicsDevice);

                SpriteSheet animation = Content.Load<SpriteSheet>("perso.sf", new JsonContentLoader());
                Perso = new AnimatedSprite(animation);
                
            //
                _screenHome = new Home(this);
                _screenLevel1 = new Level(this);
                _screentest = new Test(this);
                _screenParametre = new Parametres(this);
                _screenRules = new Rules(this);
                _screenRules2 = new Rules2(this);

             //  ScreenManager.LoadScreen(_screenLevel1, new FadeTransition(GraphicsDevice, Color.Black));
            // _screenManager.LoadScreen(_screenHome, new FadeTransition(GraphicsDevice, Color.Black));
                _currentScreen = Ecran.Home;

                //musique
                _music = Content.Load<Song>("music");
                MediaPlayer.Play(_music);

            //police
            _fontTitle = Content.Load<SpriteFont>("Font/font");
            _font2 = Content.Load<SpriteFont>("Font/fontPara");
            _fontStart = Content.Load<SpriteFont>("Font/fontStart");
            _fontExit = Content.Load<SpriteFont>("Font/fontExit");

            //loading écran accueil
            ScreenManager.LoadScreen(_screenHome);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            /*
            if(_screenHome.start)
                ScreenManager.LoadScreen(_screenLevel1, new FadeTransition(GraphicsDevice, Color.Black));

            if(_screenHome.exit)
                Exit();
            */
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (/*_ecranEncours != Ecran.Accueil && */ Keyboard.GetState().IsKeyDown(Keys.M))
            {
                ScreenManager.LoadScreen(_screenHome, new FadeTransition(GraphicsDevice, Color.Black));
            }

            base.Update(gameTime);
        }

            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(Color.Black);

                // TODO: Add your drawing code here

                base.Draw(gameTime);
            }
        }
}
