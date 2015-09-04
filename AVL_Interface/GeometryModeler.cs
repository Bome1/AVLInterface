using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AVL_Interface
{
    public static class GeometryModeler
    {
        public static Color BorderColor = Color.Black;
        public static Color LatticeColor = Color.Fuchsia;
        public static Color ControlSurfColor = Color.Blue;
        public static Color SelectedColor = Color.Gold;

        public enum Views
        {
            Isomeric,
            Planform,
            Side,
            Front
        }

        class Line3D
        {
            public Point3D[] Points;
            public Color LineColor = Color.Blue;

            public int Count
            {
                get { return Points.Length; }
            }

            public Line3D(Point3D[] points)
            {
                Points = points;
            }

            public Line3D(Point3D[] points, Color lineColor)
            {
                Points = points;
                LineColor = lineColor;
            }

            public void AddPoint(Point3D p)
            {
                Point3D[] temp = new Point3D[Points.Length + 1];
                Points.CopyTo(temp, 0);
                temp[Points.Length] = p;
                Points = temp;
            }
        }

        class Point3D
        {
            public double X;
            public double Y;
            public double Z;

            public Point3D(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public Point3D InvertY(double mirror_xzplane)
            {
                return new Point3D(X, -(Y - mirror_xzplane), Z);
            }

            public static Point3D operator +(Point3D p1, Point3D p2)
            {
                return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
            }

            public static Point3D operator -(Point3D p1, Point3D p2)
            {
                return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
            }

            public static Point3D operator /(Point3D p1, double val)
            {
                return new Point3D(p1.X / val, p1.Y / val, p1.Z / val);
            }

            public static Point3D operator *(Point3D p1, double val)
            {
                return new Point3D(p1.X * val, p1.Y * val, p1.Z * val);
            }

            /// <summary>
            /// Rotate the point
            /// </summary>
            /// <param name="point3D">point</param>
            /// <param name="x_degrees">Yaw</param>
            /// <param name="y_degrees">Pitch</param>
            /// <param name="z_degrees">Roll</param>
            /// <returns></returns>
            public static Point3D Rotate(Point3D point3D, double x_degrees, double y_degrees, double z_degrees)
            {
                //if no rotation, dont bother doing any sort of maths
                if (x_degrees == 0 && y_degrees == 0 && z_degrees == 0)
                    return point3D;

                double RRX = x_degrees * 0.01745329251f;
                double RRY = y_degrees * 0.01745329251f;
                double RRZ = z_degrees * 0.01745329251f;

                double cPsi = System.Math.Cos(RRX);//yaw
                double sPsi = System.Math.Sin(RRX);

                double cTheta = System.Math.Cos(RRY);//pitch
                double sTheta = System.Math.Sin(RRY);

                double cPhi = System.Math.Cos(RRZ);//roll
                double sPhi = System.Math.Sin(RRZ);

                var newPoint = new Point3D(0,0,0);
                newPoint.X = cTheta * cPsi * point3D.X + cTheta * sPsi * point3D.Y - sTheta * point3D.Z;
                newPoint.Y = (sPhi * sTheta * cPsi - cPhi * sPsi) * point3D.X + (sPhi * sTheta * sPsi + cPhi * cPsi) * point3D.Y + (sPhi * cTheta) * point3D.Z;
                newPoint.Z = (cPhi * sTheta * cPsi + sPhi * sPsi) * point3D.X + (cPhi * sTheta * sPsi - sPhi * cPsi) * point3D.Y + cPhi * cTheta * point3D.Z;

                return newPoint;
            }

            public static Point3D Translate(Point3D oldPoint, double dX, double dY, double dZ)
            {
                return new Point3D(oldPoint.X + dX, oldPoint.Y + dY, oldPoint.Z + dZ);
            }

            public static Point3D Translate(Point3D points3D, Point3D oldOrigin, Point3D newOrigin)
            {
                //Moves a 3D point based on a moved reference point
                Point3D difference = new Point3D(newOrigin.X - oldOrigin.X, newOrigin.Y - oldOrigin.Y, newOrigin.Z - oldOrigin.Z);
                points3D.X += difference.X;
                points3D.Y += difference.Y;
                points3D.Z += difference.Z;
                return points3D;
            }
            
            //These are to make the above functions workable with arrays of 3D points
            public static Point3D[] Rotate(Point3D[] points3D, double x_degrees, double y_degrees, double z_degrees)
            {
                for (int i = 0; i < points3D.Length; i++)
                {
                    points3D[i] = Rotate(points3D[i], x_degrees, y_degrees, z_degrees);
                }
                return points3D;
            }

            public static Point3D[] Translate(Point3D[] points3D, Point3D oldOrigin, Point3D newOrigin)
            {
                for (int i = 0; i < points3D.Length; i++)
                {
                    points3D[i] = Translate(points3D[i], oldOrigin, newOrigin);
                }
                return points3D;
            }
        }

        public static Image DrawAirplane(Aircraft ac, Views view)
        {
            return DrawAirplane(ac, view, string.Empty, -1);
        }

        public static Image DrawAirplane(Aircraft ac, Views view, string selected_SurfName, int secHighlight)
        {
            switch (view)
            {
                case Views.Isomeric:
                    return DrawAirplane(ac, 35, 0, -45, selected_SurfName, secHighlight);
                case Views.Planform:
                    return DrawAirplane(ac, 0, 0, 0, selected_SurfName, secHighlight);
                case Views.Side:
                    return DrawAirplane(ac, 0, 0, -90, selected_SurfName, secHighlight);
                case Views.Front:
                    return DrawAirplane(ac, 90, 0, -90, selected_SurfName, secHighlight);
                default: goto case 0;
            }
        }

        public static Image DrawAirplane(Aircraft ac, double Yaw_Angle, double Pitch_Angle, double Roll_Angle)
        {
            return DrawAirplane(ac, Yaw_Angle, Pitch_Angle, Roll_Angle, string.Empty, -1);
        }
            
        public static Image DrawAirplane(Aircraft ac, double Yaw_Angle, double Pitch_Angle, double Roll_Angle, string selected_SurfName, int secHighlight)
        {
            Bitmap bm = new Bitmap(500, 500);

            double xmin = double.MaxValue;
            double ymin = double.MaxValue;
            double xmax = double.MinValue;
            double ymax = double.MinValue;

            //We gather a list of all the lines that will make up the aircraft in 3D coordinates (as doubles!)
            List<Line3D> AllLines = new List<Line3D>();
            foreach (AVL_File.Surface surf in ac.Initial_AVL_File.Surfaces)
            {
                if (!string.IsNullOrEmpty(selected_SurfName) && surf.Name == selected_SurfName && secHighlight >= 0)
                    AllLines.AddRange(DrawSurface(surf, secHighlight));
                else
                    AllLines.AddRange(DrawSurface(surf));
            }
            //draw the axis
            //AllLines.AddRange(DrawAxis());

            //rotate points, and get bounding box
            foreach (Line3D l3d in AllLines)
            {
                for (int i = 0; i < l3d.Count; i++)
                {
                    Point3D translated = Point3D.Rotate(l3d.Points[i], Yaw_Angle, Pitch_Angle, Roll_Angle);
                    //bounding x check
                    if (translated.X < xmin)
                        xmin = translated.X;
                    else if (translated.X > xmax)
                        xmax = translated.X;
                    //bounding y check
                    if (translated.Y < ymin)
                        ymin = translated.Y;
                    else if (translated.Y > ymax)
                        ymax = translated.Y;

                    l3d.Points[i] = translated;
                }
            }

            double scaleFactor = xmax-xmin > ymax-ymin ? (double)bm.Width / (xmax-xmin) : (double)bm.Height / (ymax-ymin);

            //now to paint the picture
            using (var graphics = Graphics.FromImage(bm))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //center the image
                float yshift = (float)(Math.Abs(ymin) * scaleFactor + (bm.Height - (ymax - ymin) * scaleFactor) / 2);
                float xshift = (float)(Math.Abs(xmin) * scaleFactor + (bm.Width - (xmax - xmin) * scaleFactor) / 2);
                graphics.TranslateTransform(xshift, yshift);

                //draw all the lines, we project to the XY-plane simply by ignoring the Z dimension, since this is post-rotation
                for (int i = 0; i < AllLines.Count; i++)
                {
                    if (AllLines[i].Count == 0)
                        continue;

                    PointF[] pts2D = new PointF[AllLines[i].Count];
                    for (int j = 0; j < AllLines[i].Count; j++)
                        pts2D[j] = new PointF((float)(AllLines[i].Points[j].X * scaleFactor), (float)(AllLines[i].Points[j].Y * scaleFactor));

                    graphics.DrawLines(new Pen(AllLines[i].LineColor), pts2D);
                }
            }

            return bm;
        }

        private static List<Line3D> DrawSurface(AVL_File.Surface surf)
        {
            return DrawSurface(surf, -1);
        }

        private static List<Line3D> DrawSurface(AVL_File.Surface surf, int SecHighlight)
        {
            List<Line3D> lines = new List<Line3D>();
            for (int i = 0; i < surf.Sections.Count - 1; i++)
                lines.AddRange(DrawSection(surf.Sections[i], surf.Sections[i + 1], surf.Nchordwise, surf.Cspace, surf.Nspanwise, surf.Sspace, surf.YDUPLICATE, i == SecHighlight));

            lines.Add(DrawBorder(surf));
            return lines;
        }

        private static List<Line3D> DrawSection(AVL_File.Surface.Section sec1, AVL_File.Surface.Section sec2, int Nchord, int Cspace, int Nspan, int Sspace, double ydup, bool highlight)
        {
            //these could be defined for the entire surface, or by the section, so check
            if (Nspan == 0)
                Nspan = sec1.Nspanwise;
            if (Sspace == 0)
                Sspace = sec1.Sspace;

            Point3D LE_Sec1 = new Point3D(sec1.X_LeadingEdge, sec1.Y_LeadingEdge, sec1.Z_LeadingEdge);
            Point3D LE_Sec2 = new Point3D(sec2.X_LeadingEdge, sec2.Y_LeadingEdge, sec2.Z_LeadingEdge);

            Point3D TE_Sec1 = new Point3D(sec1.X_LeadingEdge + sec1.Chord, sec1.Y_LeadingEdge, sec1.Z_LeadingEdge);
            Point3D TE_Sec2 = new Point3D(sec2.X_LeadingEdge + sec2.Chord, sec2.Y_LeadingEdge, sec2.Z_LeadingEdge);

            List<Line3D> lines = new List<Line3D>();

            //chord-length lines stepping along the span, has to be Nspan-1 since for loop starts at 0, otherwise there will be one extra line
            Point3D d_le = (LE_Sec2 - LE_Sec1) / Nspan; 
            Point3D d_te = (TE_Sec2 - TE_Sec1) / Nspan;
            for (int i = 0; i < Nspan; i++)
            {
                double space_mod = i;
                if (Sspace == 1 || Sspace == -1)//cosine distribution of lines
                    space_mod = (double)Nspan / 2 * (1 - Math.Cos(Math.PI * i / (Nspan-1)));
                else if (Sspace == -2)//negative sine distribution of lines
                    space_mod = (double)Nspan * Math.Sin((Math.PI / 2) * i / (Nspan-1));
                else if (Sspace == 2)//positive sine distribution of lines
                    space_mod = (double)Nspan + ((double)Nspan * Math.Sin((-Math.PI / 2) * i / (Nspan-1)));

                Point3D le_point = LE_Sec1 + (d_le * space_mod);
                Point3D te_point = TE_Sec1 + (d_te * space_mod);

                lines.Add(new Line3D(new Point3D[] { le_point, te_point }, highlight ? SelectedColor : LatticeColor));
                if (!double.IsNaN(ydup))
                    lines.Add(new Line3D(new Point3D[] { le_point.InvertY(ydup), te_point.InvertY(ydup) }, highlight ? SelectedColor : LatticeColor));
            }

            //span-length lines stepping down the chord
            double dx_root = sec1.Chord / (Nchord - 1);
            double dx_tip = sec2.Chord / (Nchord - 1);
            for (int i = 0; i < Nchord; i++)
            {
                double space_mod = i;
                if (Cspace == 1 || Cspace == -1)//cosine distribution of lines
                    space_mod = Nchord / 2 * (1 - Math.Cos(Math.PI * i / Nchord));
                else if (Cspace == -2)//negative sine distribution of lines
                    space_mod = Nchord * Math.Sin((Math.PI / 2) * i / Nchord);
                else if (Cspace == 2)//positive sine distribution of lines
                    space_mod = Nchord + (Nchord * Math.Sin((-Math.PI / 2) * i / Nchord));
                Point3D root_point = new Point3D(LE_Sec1.X + dx_root * i, LE_Sec1.Y, LE_Sec1.Z);
                Point3D tip_point = new Point3D(LE_Sec2.X + dx_tip * i, LE_Sec2.Y, LE_Sec2.Z);

                double controlFractionRoot = 1;
                double controlFractionTip = 1;
                foreach (AVL_File.Surface.Section.Control csurfroot in sec1.control_surfaces)
                {
                    foreach (AVL_File.Surface.Section.Control csurftip in sec2.control_surfaces)
                    {
                        if (csurfroot.Name == csurftip.Name)
                        {
                            if (csurfroot.Xhinge < controlFractionRoot)
                                controlFractionRoot = csurfroot.Xhinge;
                            if (csurftip.Xhinge < controlFractionTip)
                                controlFractionTip = csurftip.Xhinge;
                        }
                    }
                }

                Color c = LatticeColor;
                if (controlFractionTip < 1 && (root_point.X - sec1.X_LeadingEdge) / sec1.Chord > controlFractionRoot)
                    c = ControlSurfColor;
                if (controlFractionRoot < 1 && (tip_point.X - sec2.X_LeadingEdge) / sec2.Chord > controlFractionTip)
                    c = ControlSurfColor;
                if (highlight)
                    c = SelectedColor;

                lines.Add(new Line3D(new Point3D[] { root_point, tip_point }, c));//highlight ? SelectedColor : LatticeColor));
                if (!double.IsNaN(ydup))
                    lines.Add(new Line3D(new Point3D[] { root_point.InvertY(ydup), tip_point.InvertY(ydup) }, c));//highlight ? SelectedColor : LatticeColor));
            }

            return lines;
        }

        /// <summary>
        /// Draws the border around the surface
        /// Still not quite right, if you duplicate off the y-axis then it doesnt quite outline each surface on its own
        /// Maybe code for the edges, skipping the last return point if ydup = 0, then mirror all points
        /// </summary>
        /// <param name="surf">Surface to draw border around</param>
        /// <returns>Points representing the border</returns>
        private static Line3D DrawBorder(AVL_File.Surface surf)
        {
            Point3D[] pts;
            if (!double.IsNaN(surf.YDUPLICATE))
                pts = new Point3D[surf.Sections.Count * 4];
            else
                pts = new Point3D[surf.Sections.Count * 2 + 1];

            int i = 0;
            foreach (AVL_File.Surface.Section sec in surf.Sections)
            {
                pts[i] = new Point3D(sec.X_LeadingEdge, sec.Y_LeadingEdge, sec.Z_LeadingEdge);

                if (double.IsNaN(surf.YDUPLICATE))
                    pts[(pts.Length - 2) - i] = new Point3D(sec.X_LeadingEdge + sec.Chord, sec.Y_LeadingEdge, sec.Z_LeadingEdge);
                else
                {
                    pts[(pts.Length-1) - i] = pts[i].InvertY(surf.YDUPLICATE);//mirrored leading edge

                    pts[((pts.Length)/2) -1 - i] = new Point3D(sec.X_LeadingEdge + sec.Chord, sec.Y_LeadingEdge, sec.Z_LeadingEdge);//trailing edge
                    pts[((pts.Length) / 2) + i] = Point3D.Translate(pts[(pts.Length-1) - i], sec.Chord, 0, 0);
                }
                i++;
            }

            if (double.IsNaN(surf.YDUPLICATE))
                pts[pts.Length - 1] = pts[0];

            return new Line3D(pts, BorderColor);
        }

        private static List<Line3D> DrawAxis()
        {
            var origin = new Point3D(0, 0, 0);
            var Xend = new Point3D(10, 0, 0);
            var Yend = new Point3D(0, -10, 0);
            var Zend = new Point3D(0, 0, 10);

            List<Line3D> axis = new List<Line3D>();
            axis.Add(new Line3D(new Point3D[] { origin, Xend }, Color.Black));
            axis.Add(new Line3D(new Point3D[] { origin, Yend }, Color.Black));
            axis.Add(new Line3D(new Point3D[] { origin, Zend }, Color.Black));

            return axis;
        }
    }
}
