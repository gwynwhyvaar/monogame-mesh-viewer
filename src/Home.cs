using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gwynwhyvaar.MeshViewer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Home : Game
    {
        Model modelViewerModel;

        float aspectRatio;
        float modelRotation = 5.5f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector3 modelPostion = Vector3.Zero;
        Vector3 cameraPostion = new Vector3(100, 0, 600);

        public Home()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 1080;

            graphics.DeviceCreated += Graphics_DeviceCreated;

            Window.Title = "=== Monogame Asset Viewer ==="; // can change this title
        }

        private void Graphics_DeviceCreated(object sender, System.EventArgs e)
        {
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
            //graphics.PreferMultiSampling = true;
        }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
            GraphicsDevice.PresentationParameters.IsFullScreen = true;
            var info = GraphicsDevice.GraphicsProfile;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // graphics.ApplyChanges();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            modelViewerModel = Content.Load<Model>("yuri");

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
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
            {
                Exit();
            }
            // TODO: Add your update logic here
            modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.015f);

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
            Matrix[] parentTransforms = new Matrix[modelViewerModel.Bones.Count];

            modelViewerModel.CopyAbsoluteBoneTransformsTo(parentTransforms);

            foreach (ModelMesh mesh in modelViewerModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = parentTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation) * Matrix.CreateTranslation(modelPostion);

                    effect.View = Matrix.CreateLookAt(cameraPostion, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 1000.0f);

                    effect.AmbientLightColor = Vector3.One; /*Color.LightSkyBlue.ToVector3();*/

                    effect.SpecularColor = Vector3.Zero;
                    effect.EmissiveColor = Vector3.Zero;
                    // this part allows drawing of the meshes in solid
                    effect.DirectionalLight0.Enabled = false;
                    effect.DirectionalLight1.Enabled = false;
                    effect.DirectionalLight2.Enabled = false;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
