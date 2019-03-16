using OpenTK;
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

        private Vector3 cameraUp;
        public Vector3 cameraPos;
        private Vector3 cameraFront;

        public Vector3 CameraFront { get => cameraFront; }

        public Camera(float screenWidth, float screenHeight)
        {
            cameraPos = new Vector3(0, 0, 10f);
            cameraFront = new Vector3(0, 0, -1f);
            cameraUp = new Vector3(0, 1, 0);

            projection = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(55.0f), screenWidth / screenHeight,
                0.1f, 100f);
            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);
        }

        

        public void Draw()
        {
            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);
        }
    }
}
