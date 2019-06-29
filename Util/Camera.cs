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
        public static Camera main;
        
        
        public Matrix4 view;
        public Matrix4 projection;

        public Vector3 cameraPos;
        private Vector3 cameraUp;
        private Vector3 cameraFront;
        private Vector3 cameraRight;

        private Vector2 previousMousePos;

        private float movementSpeed;
        private float mouseSensitivity;

        private float yaw;
        private float pitch;

        private float fov;

        public Vector3 CameraFront => cameraFront;

        public float aspectRatio;

        public Camera(float aspectRatio)
        {
            cameraPos = new Vector3(0, 0, 10f);
            cameraFront = new Vector3(0, 0, -1f);
            cameraUp = new Vector3(0, 1, 0);

            this.aspectRatio = aspectRatio;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55.0f), aspectRatio,
                0.1f, 100f);
            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);

            previousMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            movementSpeed = 1.5f;

            yaw = pitch = 0.0f;
            mouseSensitivity = .25f;
            fov = 45f;
        }

        public void UpdateFOV(float amount)
        {
            
            if ( fov - amount >= 45f)
                fov = 45f;
            else if (fov - amount <= 1f)
                fov = 1f;
            else
                fov -= amount;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov),
                aspectRatio,
                0.1f, 100f);
        }

        public void SetMousePosition(Vector2 position)
        {
            previousMousePos = position;
        }
        
        private void UpdateCameraLook()
        {
            Vector2 position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if(position != previousMousePos)
            {
                Vector2 deltaPos = previousMousePos - position;

                previousMousePos = position;

                yaw += -deltaPos.X * mouseSensitivity;
                pitch -= -deltaPos.Y * mouseSensitivity;

                if (pitch >= 89f)
                    pitch = 89f;
                else if (pitch <= -89f)
                    pitch = -89f;
            }  
            
            //this is outside of the if statement cause inside it causes a violent motion
            //because of the initial rotation not being 0 i reckon 
            
            cameraFront.X = (float) Math.Cos(MathHelper.DegreesToRadians(pitch)) *
                            (float) Math.Cos(MathHelper.DegreesToRadians(yaw));
            cameraFront.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            cameraFront.Z = (float) Math.Cos(MathHelper.DegreesToRadians(pitch)) *
                            (float) Math.Sin(MathHelper.DegreesToRadians(yaw));
            cameraFront.Normalize();

            cameraRight = (Vector3.Cross(cameraFront, Vector3.UnitY)).Normalized();
            cameraUp = (Vector3.Cross(cameraRight, cameraFront)).Normalized();
        }

        public void Draw()
        {
            if(!Game.isCursorVisible)
                UpdateCameraLook();

            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);
        }

        public void InputUpdate()
        {
            if (MasterInput.IsKeyPressed(Key.Up))
            {
                cameraPos += CameraFront * movementSpeed * Time.deltaTime;
            }
            else if (MasterInput.IsKeyPressed(Key.Down))
            {
                cameraPos -= CameraFront * movementSpeed * Time.deltaTime;
            }

            if (MasterInput.IsKeyPressed(Key.Left))
            {
                cameraPos -= Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * movementSpeed * Time.deltaTime;
            }
            else if (MasterInput.IsKeyPressed(Key.Right))
            {
                cameraPos += Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * movementSpeed * Time.deltaTime;
            }
            
            if (MasterInput.IsKeyPressed(Key.Space))
            {
                cameraPos += cameraUp * movementSpeed * Time.deltaTime;
            }
            else if (MasterInput.IsKeyPressed(Key.ShiftLeft))
            {
                cameraPos -= cameraUp * movementSpeed * Time.deltaTime;
            }
        }
    }
}
