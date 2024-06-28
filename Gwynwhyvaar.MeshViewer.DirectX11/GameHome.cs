
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gwynwhyvaar.MeshViewer.DirectX11
{
    public class GameHome : Game
    {
        private Model _modelViewerModel;

        private float _aspectRatio;
        private float _modelRotation = 5.5f;

        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

        private Vector3 _modelPostion = Vector3.Zero;
        private Vector3 _cameraPostion = new Vector3(100, 0, 600);

        public GameHome()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;

            _graphics.DeviceCreated += _graphics_DeviceCreated; 

            Window.Title = "### Monogame Mesh Viewer ###"; // can change this title
        }

        private void _graphics_DeviceCreated(object sender, System.EventArgs e)
        {
            _graphics.PreferMultiSampling = true;
            _graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            _graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 16;
            //graphics.PreferMultiSampling = true;        }
        }
        protected override void Initialize()
        {
            base.Initialize();
            GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
            GraphicsDevice.PresentationParameters.IsFullScreen = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _modelViewerModel = Content.Load<Model>("yuri");

            _aspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            // TODO: Add your update logic here
            _modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.015f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Matrix[] parentTransforms = new Matrix[_modelViewerModel.Bones.Count];

            _modelViewerModel.CopyAbsoluteBoneTransformsTo(parentTransforms);

            foreach (ModelMesh mesh in _modelViewerModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = parentTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(_modelRotation) * Matrix.CreateTranslation(_modelPostion);

                    effect.View = Matrix.CreateLookAt(_cameraPostion, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), _aspectRatio, 1.0f, 1000.0f);

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
