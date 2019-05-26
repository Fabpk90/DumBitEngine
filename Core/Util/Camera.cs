using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumBitEngine.Core.Util
{
    public class Camera : IRenderable
    {
        public Matrix4 view;
        public Matrix4 projection;

        public Vector3 cameraPos;
        private Vector3 cameraUp;
        private Vector3 cameraFront;
        private Vector3 cameraRight;

        private Vector2 previousMousePos;

        private float speed;

        public Vector3 CameraFront => cameraFront;

        public Camera(float screenWidth, float screenHeight)
        {
            cameraPos = new Vector3(0, 0, 10f);
            cameraFront = new Vector3(0, 0, -1f);
            cameraUp = new Vector3(0, 1, 0);

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55.0f), screenWidth / screenHeight,
                0.1f, 100f);
            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);

            previousMousePos.X = Mouse.GetState().X;
            previousMousePos.Y = Mouse.GetState().Y;

            speed = 1.0f;
        }

        private void UpdateCameraLook()
        {
            Vector2 position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if(position != previousMousePos)
            {
                Vector2 deltaPos = previousMousePos - position;
                previousMousePos = position;

                cameraFront.X = (float)(Math.Sin(MathHelper.DegreesToRadians(deltaPos.X)));
                cameraFront.Y = (float)Math.Sin(MathHelper.DegreesToRadians((deltaPos.Y)));
                cameraFront.Normalize();

                cameraRight = (Vector3.Cross(cameraFront, Vector3.UnitY)).Normalized();
                cameraUp = (Vector3.Cross(cameraRight, cameraFront)).Normalized();
            }  
        }

        public void Draw()
        {
            UpdateCameraLook();

            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);
        }

        public void InputUpdate(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Key.Up))
            {
                cameraPos += CameraFront * speed * Time.deltaTime;
            }
            else if (keyboard.IsKeyDown(Key.Down))
            {
                cameraPos -= CameraFront * speed * Time.deltaTime;
            }

            if (keyboard.IsKeyDown(Key.Left))
            {
                cameraPos -= Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * speed * Time.deltaTime;
            }
            else if (keyboard.IsKeyDown(Key.Right))
            {
                cameraPos += Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * speed * Time.deltaTime;
            }
            
            if (keyboard.IsKeyDown(Key.Space))
            {
                cameraPos += cameraUp * speed * Time.deltaTime;
            }
            else if (keyboard.IsKeyDown(Key.ShiftLeft))
            {
                cameraPos -= cameraUp * speed * Time.deltaTime;
            }
        }
    }
}
