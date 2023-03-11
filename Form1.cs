using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BezierCurve
{
    public partial class Form1 : Form
    {
        private List<PointF> controlPoints = new List<PointF>(); // List to hold control points
        private PointF[] bezierPoints; // Array to hold points on Bezier curve
        private int numControlPoints = 0; // Counter for number of control points

        public Form1()
        {
            InitializeComponent();
        }

        // Handler for MouseClick event on panel
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Add clicked point to list of control points
            controlPoints.Add(e.Location);
            numControlPoints++;

            // If two or more control points have been added, draw Bezier curve
            if (numControlPoints >= 2)
            {
                DrawBezierCurve();
            }

            // Redraw panel
            panel1.Invalidate();
        }

        // Method to draw Bezier curve using De Casteljau algorithm
        private void DrawBezierCurve()
        {
            int numPoints = 100; // Number of points to draw on Bezier curve
            bezierPoints = new PointF[numPoints];

            // Use De Casteljau algorithm to calculate points on Bezier curve
            for (int i = 0; i < numPoints; i++)
            {
                float t = (float)i / (float)(numPoints - 1);
                PointF[] points = new PointF[numControlPoints];
                controlPoints.CopyTo(points);
                for (int j = numControlPoints - 1; j > 0; j--)
                {
                    for (int k = 0; k < j; k++)
                    {
                        points[k] = new PointF(
                            (1 - t) * points[k].X + t * points[k + 1].X,
                            (1 - t) * points[k].Y + t * points[k + 1].Y);
                    }
                }
                bezierPoints[i] = points[0];
            }
        }

        // Handler for Paint event on panel
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw control points
            foreach (PointF point in controlPoints)
            {
                g.FillEllipse(Brushes.Blue, point.X - 5, point.Y - 5, 10, 10);
            }

            // Draw Bezier curve
            if (numControlPoints >= 2)
            {
                Pen bezierPen = new Pen(Color.Red, 2);
                g.DrawCurve(bezierPen, bezierPoints);
                bezierPen.Dispose();
            }
        }

        // Handler for Clear button click event
        private void clearButton_Click(object sender, EventArgs e)
        {
            controlPoints.Clear();
            numControlPoints = 0;
            panel1.Invalidate();
        }
    }
}
