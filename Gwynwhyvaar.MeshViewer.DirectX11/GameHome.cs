using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gwynwhyvaar.MeshViewer.DirectX11
{
    public class GameHome : Game
    {
        private Model _modelViewerModel;

        private float _aspectRatio;
        private float _modelRotation = 15.5f;

        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

        private Vector3 _modelPostion = Vector3.Up;
        private Vector3 _cameraPostion = new Vector3(100, 0, 600);

        private Effect _effect;

        public GameHome()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;

            _graphics.DeviceCreated += _graphics_DeviceCreated;

            Window.Title = "### Monogame Mesh Viewer ###";
        }

        private void _graphics_DeviceCreated(object sender, System.EventArgs e)
        {
            _graphics.PreferMultiSampling = true;
            _graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            _graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 16;
        }
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            _modelViewerModel = Content.Load<Model>("wizard");

            _aspectRatio = _graphics.GraphicsDevice.Viewport.AspectRatio;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            _modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.025f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            DrawSolid();
            base.Draw(gameTime);
        }
        private void DrawSolid()
        {
            Matrix[] parentTransforms = new Matrix[_modelViewerModel.Bones.Count];

            _modelViewerModel.CopyAbsoluteBoneTransformsTo(parentTransforms);

            foreach (ModelMesh mesh in _modelViewerModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = parentTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(_modelRotation) * Matrix.CreateTranslation(_modelPostion);

                    effect.View = Matrix.CreateLookAt(_cameraPostion, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), _aspectRatio, 0.1f, 1000.0f);

                    effect.AmbientLightColor = Vector3.One;
                    effect.Alpha = 1;
                    effect.SpecularColor = Vector3.Zero;
                    effect.EmissiveColor = Vector3.Zero;
                    if (mesh.Name.ToLowerInvariant().Equals("mesh12"))
                    {
                        effect.EmissiveColor = Color.Violet.ToVector3();
                    }
                    // this part allows drawing of the meshes in solid
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight1.Enabled = false;
                    effect.DirectionalLight2.Enabled = false;
                }
                mesh.Draw();
            }
        }
    }
}
