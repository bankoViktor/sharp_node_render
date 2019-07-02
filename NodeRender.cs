using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NodeInterface
{
    public class Node
    {
        public int NodeID;
        public Point Location;
        public Size Size;
        public Color BackColor; // TODO нод хранит цвет своего фона, для примера

        public virtual void Draw(NodeDrawArgs e)
        {
            // TODO нужно ли освобождать перья и кисти методом Dispose() или нет? 
            Rectangle rect = new Rectangle(Location, Size);
            Color HighlightColor = Color.Red;
            Color SelectedColor = Color.Blue;

            e.Graphics.FillRectangle(new SolidBrush(BackColor), rect);

            e.Graphics.DrawString(
                NodeID.ToString(),
                new Font(FontFamily.GenericMonospace, 20.0f, FontStyle.Regular, GraphicsUnit.Pixel),
                new SolidBrush(Color.Blue),
                new Point(Location.X + 10, Location.Y + 10));

            // Подсветка при наведении
            if (e.HighlightID == NodeID)
                e.Graphics.DrawRectangle(new Pen(HighlightColor), rect);
            else
            {
                if (e.Selected.Contains(this))
                {
                    e.Graphics.DrawRectangle(new Pen(SelectedColor), rect);
                }
            }
        }
    }

    public class NodeDrawArgs
    {
        public Graphics Graphics;
        public int HighlightID;
        public List<Node> Selected;
    }

    public partial class NodeRender : Control
    {
        [Category("Сетка"), Description("Включает отрисовку сетки на фоне")]
        public bool Grid { get; set; } = true;
        [Category("Сетка"), Description("Задает шаг сетки")]
        public int GridStep { get; set; } = 25;
        [Category("Сетка"), Description("Задает ширину линий сетки")]
        public float GridWidth { get; set; } = 2f;
        [Category("Сетка"), Description("Задает цвет линий сетки")]
        public Color GridColor { get; set; } = Color.LightGray;

        public int Highlight { get; private set; } = -1;
        public List<Node> Selected { get; private set; } = new List<Node>(); // TODO хранить ID выделенных нодов? 
        public List<Node> Nodes { get; private set; } = new List<Node>();

        public int deb_paint = 0; // TODO 

        private Color SelectingColorToRight = Color.LightBlue;
        private Color SelectingColorToLeft = Color.LightGreen;
        private int SelectingAlpha = 100;
        private Color SelectingBorderColor = Color.BlueViolet;
        private float SelectingBorderWidth = 2f;

        private bool IsLeftPressed; // флаг нажатия ЛКМ
        private Point LastMousePos; // Последняя позиция щелчка мыши
        private Point NewMousePos;  // Новая позиция курсора выделения
                                    // меняется каждый выхов MouseMove для перерисовки прямоугольника выделения
                                    //private Node TopNode;       // Верхний нод 
                                    // TODO последний выделенный нод отрисовывать поверх всех (последним) 


        public NodeRender()
        {
            DoubleBuffered = true;

            MouseMove += NodeRender_MouseMove;
            MouseDown += NodeRender_MouseDown;
            MouseUp += NodeRender_MouseUp;
        }

        private Node GetNodeUnderCursor(int Cursor_X, int Cursor_Y)
        {
            Node node = null;

            foreach (Node n in Nodes)
            {
                if (PointIntoRectangle(new Point(Cursor_X, Cursor_Y),
                    new Rectangle(n.Location, n.Size)))
                {
                    node = n;
                    break;
                }
            }

            return node;
        }

        private bool PointIntoRectangle(Point point, Rectangle rectangle)
        {
            if (point.X >= rectangle.Left && point.X < rectangle.Right &&
                point.Y >= rectangle.Top && point.Y < rectangle.Bottom)
                return true;
            else
                return false;
        }

        private Rectangle GetSelectingRect()
        {
            Point point;
            Size size;

            if (NewMousePos.X > LastMousePos.X)
            {
                // Вправо
                if (NewMousePos.Y > LastMousePos.Y)
                {
                    // Вниз
                    point = new Point(LastMousePos.X, LastMousePos.Y);
                    size = new Size(NewMousePos.X - LastMousePos.X, NewMousePos.Y - LastMousePos.Y);
                }
                else
                {
                    // Вверх
                    point = new Point(LastMousePos.X, NewMousePos.Y);
                    size = new Size(NewMousePos.X - LastMousePos.X, LastMousePos.Y - NewMousePos.Y);
                }
            }
            else
            {
                // Влево
                if (NewMousePos.Y > LastMousePos.Y)
                {
                    // Вниз
                    point = new Point(NewMousePos.X, LastMousePos.Y);
                    size = new Size(LastMousePos.X - NewMousePos.X, NewMousePos.Y - LastMousePos.Y);
                }
                else
                {
                    // Вверх
                    point = new Point(NewMousePos.X, NewMousePos.Y);
                    size = new Size(LastMousePos.X - NewMousePos.X, LastMousePos.Y - NewMousePos.Y);
                }
            }
            return new Rectangle(point, size);
        }

        private void DrawGrid(Graphics g, Color color, int Step, float Width)
        {
            Pen pen = new Pen(color, Width);
            // TODO нужно ли освобождать перья и кисти методом Dispose() или нет? 

            // Вертикальные линии
            for (int с = Step; с < Size.Width; с += Step)
            {
                g.DrawLine(
                    pen,
                    new Point(с, 0),
                    new Point(с, Size.Height));
            }

            // Горизонтальные линии
            for (int r = Step; r < Size.Height; r += Step)
            {
                g.DrawLine(
                    pen,
                    new Point(0, r),
                    new Point(Size.Width, r));
            }
        }

        private void NodeRender_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsLeftPressed = true;
                LastMousePos = new Point(e.X, e.Y);
                NewMousePos = LastMousePos;

                Node node = GetNodeUnderCursor(e.X, e.Y);
                if (node == null)
                    // Клик по рабочей области
                    Selected.Clear();
                else
                {
                    // Клик по элементу
                    if (Selected.Count > 1)
                    {
                        // Выбран как минимум 2 элемента -> множественный выбор
                        if (Selected.Contains(node) == false)
                        {
                            // Среди выбранный элементов текущего нет -> новый одиночный выбор
                            Selected.Clear();
                            Selected.Add(node);
                        }
                    }
                    else
                    {
                        // Выбран всего 1 элемент -> одиночный выбор
                        Selected.Clear();
                        Selected.Add(node);
                        //TopNode = node;
                        // TODO может лучше хранить ID нода? и искать его по его ID в сипике Nodes
                    }
                }

                Refresh();
            }
        }

        private void NodeRender_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsLeftPressed = false;

                Rectangle selectRect = GetSelectingRect();

                foreach (Node node in Nodes)
                {
                    Rectangle intersectRect = selectRect;
                    intersectRect.Intersect(new Rectangle(node.Location, node.Size));

                    if (intersectRect.Width == node.Size.Width &&
                        intersectRect.Height == node.Size.Height)
                    {
                        Selected.Add(node);
                    }
                }
            }

            Refresh();
        }

        private void NodeRender_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsLeftPressed)
            {
                if (Selected.Count > 0)
                {
                    // режим перемещения
                    foreach (Node node in Selected)
                    {
                        node.Location.X += e.X - LastMousePos.X;
                        node.Location.Y += e.Y - LastMousePos.Y;
                    }
                    LastMousePos = new Point(e.X, e.Y);
                    Refresh();
                }
                else
                {
                    // режим выделения 
                    NewMousePos = new Point(e.X, e.Y);
                    Refresh();
                }
            }
            else
            {
                int newHighlightID = -1;

                Node node = GetNodeUnderCursor(e.X, e.Y);
                if (node != null)
                    newHighlightID = node.NodeID;

                if (Highlight != newHighlightID)
                {
                    Highlight = newHighlightID;
                    Refresh();
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);

            // Цвет фона задется родительским свойством BackColor, а цвет сетки свойством GridColor
            if (Grid)
                DrawGrid(pevent.Graphics, GridColor, GridStep, GridWidth);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            deb_paint++;

            foreach (Node node in Nodes)
            {
                NodeDrawArgs args = new NodeDrawArgs()
                {
                    Graphics = e.Graphics,
                    HighlightID = Highlight,
                    Selected = Selected
                };
                node.Draw(args);
            }

            if (IsLeftPressed && Highlight == -1 & NewMousePos != LastMousePos)
            {
                Color color;

                if (NewMousePos.X > LastMousePos.X)
                    color = SelectingColorToRight; // Вправо
                else
                    color = SelectingColorToLeft; // Влево

                Rectangle selRect = GetSelectingRect();

                // Рисуем прямоугольник выделения
                e.Graphics.FillRectangle(
                            new SolidBrush(Color.FromArgb(SelectingAlpha, color)), selRect);
                e.Graphics.DrawRectangle(
                    new Pen(SelectingBorderColor, SelectingBorderWidth), selRect);
            }
        }
    }
}
