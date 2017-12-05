using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using textInput;
using GameData;

namespace GetCredentialsClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum PlayerDataState { LOGGEDOUT, LOGGEDIN, CHARACTER_ASSIGNED }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        string connectionMessage = string.Empty;
        PlayerData playerData;
        PlayerDataState state = PlayerDataState.LOGGEDOUT;
        // SignalR Client object delarations

        HubConnection serverConnection;
        IHubProxy proxy;

        public bool Connected { get; private set; }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Helpers.GraphicsDevice = GraphicsDevice;
            new GetGameInputComponent(this);
            serverConnection = new HubConnection("http://localhost:30791/");
            serverConnection.StateChanged += ServerConnection_StateChanged;
            proxy = serverConnection.CreateHubProxy("CredentialHub");
            serverConnection.Start();
            base.Initialize();
        }

        private void ServerConnection_StateChanged(StateChange State)
        {
            switch (State.NewState)
            {
                case ConnectionState.Connected:
                    connectionMessage = "Connected......";
                    Connected = true;
                    
                    break;
                case ConnectionState.Disconnected:
                    connectionMessage = "Disconnected.....";
                    if (State.OldState == ConnectionState.Connected)
                        connectionMessage = "Lost Connection....";
                    Connected = false;
                    break;
                case ConnectionState.Connecting:
                    connectionMessage = "Connecting.....";
                    Connected = false;
                    break;

            }
        }

        private void checkLogin()
        {
            if (Connected && GetGameInputComponent.name != string.Empty && GetGameInputComponent.password != string.Empty

                )
            {
                proxy.Invoke<PlayerData>("checkCredentials", 
                    new object[] { GetGameInputComponent.name, GetGameInputComponent.password } )
                    .ContinueWith( // This is an inline delegate pattern that processes the message 
                                   // returned from the async Invoke Call
                            (p) =>
                            { // With p do 
                            if (p.Result == null)
                                    connectionMessage = "Invalid Login";
                                else
                                {
                                    playerData = p.Result;
                                    state = PlayerDataState.LOGGEDIN;
                                }
                            });
            }
         }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch(state)
            {
                case PlayerDataState.LOGGEDOUT:
                    if (Connected)
                        {
                            checkLogin();
                        }
                    break;
                case PlayerDataState.LOGGEDIN:
                    break;
            }
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if(state == PlayerDataState.LOGGEDIN)
            {
                //Draw string player details
            }
            base.Draw(gameTime);
        }
    }
}
