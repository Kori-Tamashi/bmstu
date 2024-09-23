using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Camera
    {
        Vector3D direction;
        Point3D position;

        const int moveStep = 15;

        public Camera(Vector3D direction, Point3D position)
        {
            this.direction = direction;
            this.position = position;
        }

        public int MoveStep
        {
            get { return moveStep; }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    MoveUp();
                    break;
                case Direction.Down:
                    MoveDown(); 
                    break;
                case Direction.Left:
                    MoveLeft();
                    break;
                case Direction.Right:
                    MoveRight();
                    break;
                case Direction.LeftUp:
                    MoveLeftUp();
                    break;
                case Direction.RightUp:
                    MoveRightUp();
                    break;
                case Direction.LeftDown:
                    MoveLeftDown();
                    break;
                case Direction.RightDown:
                    MoveRightDown();
                    break;
                default:
                    break;
            }
        }

        public void MoveUp()
        {
            Move move = new Move(0, -moveStep, 0);
            code.Move.Transform(move, position);
        }

        public void MoveDown()
        {
            Move move = new Move(0, moveStep, 0);
            code.Move.Transform(move, position);
        }

        public void MoveLeft()
        {
            Move move = new Move(-moveStep, 0, 0);
            code.Move.Transform(move, position);
        }

        public void MoveRight()
        {
            Move move = new Move(moveStep, 0, 0);
            code.Move.Transform(move, position);
        }

        public void MoveLeftUp()
        {
            MoveLeft();
            MoveUp();
        }

        public void MoveLeftDown()
        {
            MoveLeft();
            MoveDown();
        }

        public void MoveRightUp()
        {
            MoveRight();
            MoveUp();
        }

        public void MoveRightDown()
        {
            MoveRight();
            MoveDown();
        }

        public void Move(Move move)
        {
            code.Move.Transform(move, position);
        }

        public void Rotate(Rotate rotate)
        {
            code.Rotate.Transform(rotate, position);
        }
    }
}
