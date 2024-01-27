using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonBattle.Engine
{
    public class Camera
    {
        #region Singleton
        private static Camera _singleton;

        public static Camera GetCamera()
        {
            if (_singleton == null)
            {
                _singleton = new Camera();
            }
            return _singleton;
        }
        #endregion

        private Vector2 position;
        private float zoom;
        private float rotation;
        private int viewportWidth;
        private int viewportHeight;
        private List<CameraAnimation> cameraAnimations;

        private Camera()
        {
            zoom = 1.0f;
            viewportWidth = 1080;
            viewportHeight = 720;
            position = new Vector2(viewportWidth >> 1, viewportHeight >> 1);
            rotation = 0f;
            cameraAnimations = new List<CameraAnimation>();
        }

        public int ViewportWidth { get { return viewportWidth; } set { viewportWidth = value; } }
        public int ViewportHeight { get { return viewportHeight; } set { viewportHeight = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public float Zoom { get { return zoom; } set { zoom = value; } }

        public void Update(GameTime gameTime)
        {
            if (cameraAnimations.Count > 0)
            {
                cameraAnimations[0].Update();
                if (cameraAnimations[0].IsCompleted)
                {
                    cameraAnimations.RemoveAt(0);
                }
            }
        }

        // Center of the Viewport which does not account for scale
        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(viewportWidth >> 1, viewportHeight >> 1);
            }
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)position.X,
                   -(int)position.Y, 0) *
                   Matrix.CreateRotationZ(rotation) *
                   Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public void MoveCamera(Vector2 cameraMovement)
        {
            position += cameraMovement;
        }

        public void AdjustZoom(float amount)
        {
            zoom += amount;
        }

        public void AdjustRotation(float amount)
        {
            rotation += amount;
        }

        public void CenterOn(Vector2 position)
        {
            this.position = position;
        }

        public void AddAnimations(List<CameraAnimation> cameraAnimations)
        {
            this.cameraAnimations.AddRange(cameraAnimations);
        }
    }

    public class CameraAnimation
    {
        protected Vector2 toPosition;
        protected float toZoom;
        protected float toRotation;

        protected Vector2 toPositionStep;
        protected float toZoomStep;
        protected float toRotationStep;

        protected int runtime;
        protected bool isCompleted;
        protected bool initialized;

        public CameraAnimation(Vector2 toPosition, float toZoom, float toRotation, int runtime)
        {
            this.runtime = runtime;
            this.toPosition = toPosition;
            this.toZoom = toZoom;
            this.toRotation = toRotation;
            this.isCompleted = false;
            this.initialized = false;

            this.toPositionStep = this.toPosition / runtime;
            this.toRotationStep = this.toRotation / runtime;
        }

        public void Update()
        {
            if (!this.initialized)
            {
                Initialize();
            }

            if (this.runtime == 0)
            {
                this.isCompleted = true;
                return;
            }

            if (toPosition != null)
            {
                Microsoft.Xna.Framework.Game._camera.MoveCamera(toPositionStep);
            }

            if (!float.IsNaN(toZoom))
            {
                Microsoft.Xna.Framework.Game._camera.AdjustZoom(toZoomStep);
            }

            if (!float.IsNaN(toRotation))
            {
                Microsoft.Xna.Framework.Game._camera.AdjustRotation(toRotationStep);
            }

            this.runtime--;
        }

        public void Initialize()
        {
            if (this.toZoom != float.NaN)
            {
                this.toZoomStep = (this.toZoom - Microsoft.Xna.Framework.Game._camera.Zoom) / runtime;
            }

            this.initialized = true;
        }

        public bool IsCompleted { get { return isCompleted; } set { this.isCompleted = value; } }

    }
}
