using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace code
{
    class ViewingFrustum
    {
        float view_field_angle;
        float view_field_width;
        float view_field_height;
        float far_plane_distance;
        float near_plane_distance;

        Camera camera;
        List<Polygon> planes;

        public ViewingFrustum(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance, Camera camera)
        {
            this.camera = camera;
            this.view_field_width = view_field_width;
            this.view_field_height = view_field_height;
            this.far_plane_distance = far_plane_distance;
            this.near_plane_distance = near_plane_distance;

            InitializeViewFieldAngle(view_field_height, near_plane_distance);
            InitializePlanes(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera);
        }

        #region Initialize

        private void InitializeViewFieldAngle(float view_field_height, float near_plane_distance)
        {
            view_field_angle = 2 * (float)Math.Atan(view_field_height / (2 * near_plane_distance));
            view_field_angle *= 180 / (float)Math.PI;
            //view_field_angle = 90;
        }

        private void InitializePlanes(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance, Camera camera)
        {
            planes = new List<Polygon>(6);

            InitializeNearPlane(view_field_width, view_field_height, near_plane_distance, camera);
            InitializeFarPlane(view_field_width, view_field_height, far_plane_distance, camera);
            InitializeOtherPlanes();
        }

        private void InitializeNearPlane(float view_field_width, float view_field_height, float near_plane_distance, Camera camera)
        {
            // Calculate near plane's normal vector
            Vector3D near_plane_normal = camera.Direction;

            // Calculate near plane's distance from camera's position
            float near_plane_distance_from_camera = near_plane_distance;

            // Calculate near plane's left, right, top, and bottom points
            float near_plane_left = -view_field_width / 2;
            float near_plane_right = view_field_width / 2;
            float near_plane_top = view_field_height / 2;
            float near_plane_bottom = -view_field_height / 2;

            // Calculate points in camera's coordinate system
            Vector3D near_plane_left_top = near_plane_left * camera.Right + near_plane_top * camera.Up + near_plane_distance_from_camera * near_plane_normal + camera.Position;
            Vector3D near_plane_right_top = near_plane_right * camera.Right + near_plane_top * camera.Up + near_plane_distance_from_camera * near_plane_normal + camera.Position;
            Vector3D near_plane_left_bottom = near_plane_left * camera.Right + near_plane_bottom * camera.Up + near_plane_distance_from_camera * near_plane_normal + camera.Position;
            Vector3D near_plane_right_bottom = near_plane_right * camera.Right + near_plane_bottom * camera.Up + near_plane_distance_from_camera * near_plane_normal + camera.Position;

            planes.Add(new Polygon(
                near_plane_left_top.ToPoint(),
                near_plane_right_top.ToPoint(),
                near_plane_right_bottom.ToPoint(),
                near_plane_left_bottom.ToPoint()
                ).NormalizedCopy());
        }

        private void InitializeFarPlane(float view_field_width, float view_field_height, float far_plane_distance, Camera camera)
        {
            // Calculate far plane's normal vector
            Vector3D far_plane_normal = camera.Direction;

            // Calculate far plane's distance from camera's position
            float far_plane_distance_from_camera = far_plane_distance;

            // Calculate far plane's left, right, top, and bottom points
            float far_plane_left = -view_field_width / 2;
            float far_plane_right = view_field_width / 2;
            float far_plane_top = view_field_height / 2;
            float far_plane_bottom = -view_field_height / 2;

            // Calculate points in camera's coordinate system
            Vector3D far_plane_left_top = far_plane_left * camera.Right + far_plane_top * camera.Up + far_plane_distance_from_camera * far_plane_normal + camera.Position;
            Vector3D far_plane_right_top = far_plane_right * camera.Right + far_plane_top * camera.Up + far_plane_distance_from_camera * far_plane_normal + camera.Position;
            Vector3D far_plane_left_bottom = far_plane_left * camera.Right + far_plane_bottom * camera.Up + far_plane_distance_from_camera * far_plane_normal + camera.Position;
            Vector3D far_plane_right_bottom = far_plane_right * camera.Right + far_plane_bottom * camera.Up + far_plane_distance_from_camera * far_plane_normal + camera.Position;

            planes.Add(new Polygon(
                far_plane_left_top.ToPoint(),
                far_plane_right_top.ToPoint(),
                far_plane_right_bottom.ToPoint(),
                far_plane_left_bottom.ToPoint()
                ).NormalizedCopy());
        }

        private void InitializeOtherPlanes()
        {
            Point3D near_plane_left_top = planes[0].Points[0];
            Point3D near_plane_right_top = planes[0].Points[1];
            Point3D near_plane_left_bottom = planes[0].Points[3];
            Point3D near_plane_right_bottom = planes[0].Points[2];

            Point3D far_plane_left_top = planes[1].Points[0];
            Point3D far_plane_right_top = planes[1].Points[1];
            Point3D far_plane_left_bottom = planes[1].Points[3];
            Point3D far_plane_right_bottom = planes[1].Points[2];

            // top plane
            planes.Add(new Polygon(
                far_plane_left_top,
                far_plane_right_top,
                near_plane_right_top,
                near_plane_left_top
                ).NormalizedCopy());

            // bottom plane
            planes.Add(new Polygon(
                far_plane_left_bottom,
                far_plane_right_bottom,
                near_plane_right_bottom,
                near_plane_left_bottom
                ).NormalizedCopy());

            //right plane
            planes.Add(new Polygon(
                far_plane_right_top,
                far_plane_right_bottom,
                near_plane_right_bottom,
                near_plane_right_top
                ).NormalizedCopy());

            // left plane
            planes.Add(new Polygon(
                far_plane_left_top,
                far_plane_left_bottom,
                near_plane_left_bottom,
                near_plane_left_top
                ).NormalizedCopy());
        }

        #endregion

        #region Matrix

        public TransformationMatrix ViewMatrix()
        {
            return ViewTransformation().matrix;
        }

        private Transformation ViewTransformation()
        {
            Transformation rotate = new Transformation();
            Transformation move = new Transformation();

            move.ToIdentity();
            move[0, 3] = -camera.Position.X;
            move[1, 3] = -camera.Position.Y;
            move[2, 3] = -camera.Position.Z;

            rotate[3, 3] = 1;
            for (int i = 0; i < 3; i++)
            {
                rotate[i, 0] = camera.Right[i];
                rotate[i, 1] = camera.Up[i];
                rotate[i, 2] = camera.Direction[i];
            }

            return move * rotate;
        }

        public TransformationMatrix ProjectionMatrix()
        {
            return ProjectionTransformation().matrix;
        }

        private Transformation ProjectionTransformation()
        {
            Transformation projection = new Transformation();

            float F = far_plane_distance;
            float N = near_plane_distance;
            float R = view_field_width / view_field_height;

            float zoom_y = 1 / (float)Math.Tan( (view_field_angle / 2) * Math.PI / 180 );
            float zoom_x = zoom_y / R;

            projection[0, 0] = zoom_x;
            projection[1, 1] = zoom_y;
            projection[2, 2] = -(F + N) / (F - N);
            projection[2, 3] = -2 * F * N / (F - N);
            projection[3, 2] = -1;

            return projection;
        }

        public TransformationMatrix WorldMatrix(Point3D point)
        {
            return WorldTransformation(point).matrix;
        }

        private Transformation WorldTransformation(Point3D point)
        {
            return new Move(point.X, point.Y, point.Z);
        }

        private Matrix<float> PointMatrix(Point3D point)
        {
            return new Matrix<float>(point);
        }

        #endregion

        #region Processing

        public void ProcessModels(List<Model> models, Graphics gr)
        {
            foreach (Model model in models)
            {
                ProcessModel(model, gr);
            }
        }

        public void ProcessModel(Model model, Graphics gr)
        {
            foreach (Polygon p in model.Polygons)
            {
                ProcessPolygon(p, gr);
            }
        }

        private void ProcessPolygon(Polygon polygon, Graphics gr)
        {
            //Polygon clippedPolygon = Clipping.ClipPolygon(planes, polygon);

            foreach (Edge edge in polygon.Edges)
            {
                Point start = ViewPortPoint(edge.start);
                Point end = ViewPortPoint(edge.end);

                gr.DrawLine(new Pen(Color.Black), start, end);
            }
        }

        public bool ViewingFrustumPointIsClipped(Point3D worldPoint)
        {
            Matrix<float> mtr = ViewingFrustumPointMatrix(worldPoint);

            return ( Math.Abs(mtr[0, 0]) > mtr[3, 0] ) || 
                   ( Math.Abs(mtr[1, 0]) > mtr[3, 0] ) || 
                   ( mtr[2, 0] < 0 || Math.Abs(mtr[2, 0]) > mtr[3, 0] );
        }

        public Point ViewPortPoint(Point3D worldPoint)
        {
            Point3D p = ViewingFrustumPoint(worldPoint);

            int u = (int)( (p.X * 0.5 + 0.5) * view_field_width );
            int v = (int)( (p.Y * 0.5 + 0.5) * view_field_height );

            u = (int)Math.Max(0, Math.Min(u, view_field_width - 1));
            v = (int)Math.Max(0, Math.Min(v, view_field_height - 1));

            return new Point(u, v);
        }

        private Point3D ViewingFrustumPoint(Point3D worldPoint)
        {
            Matrix<float> mtr = ViewingFrustumPointMatrix(worldPoint);

            return new Point3D(mtr[0, 0], mtr[1, 0], mtr[2, 0]);
        }

        private Matrix<float> ViewingFrustumPointMatrix(Point3D worldPoint)
        {
            Matrix<float> mtr = ProjectionMatrix() * (ViewMatrix() * (WorldMatrix(worldPoint).Transpose() * PointMatrix(worldPoint).Transpose()));

            for (int i = 0; i < 4; i++)
            {
                mtr[i, 0] /= mtr[3, 0];
            }

            return mtr;
        }

        #endregion
    }
}
