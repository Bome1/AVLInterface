using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;

namespace AVL_Interface
{
    /// <summary>
    /// This class loads and generates a Bitmap image of the *.ps file that AVL outputs.
    /// The number and type of implimented PS commands is only what is needed to read
    /// those AVL files, so dont expect anything to fancy.
    /// 
    /// TODO:
    /// Color replacement
    /// Set size of bitmap based on the Boarder comment at the end of the PS files
    /// </summary>
    public class PostScript_Interp: IDisposable
    {
        private string m_fileName;
        Stack<string> operand = new Stack<string>();
        Dictionary<string, string> CustomOps = new Dictionary<string, string>();
        List<string> data = new List<string>();

        private Image img = new Bitmap(800,500);
        private PointF currentPoint;
        private Pen currentPen = new Pen(Color.Black);

        public string FileName
        {
            get { return m_fileName; }
        }

        public Image Rendered_Image
        {
            get { return img; }
        }

        public PostScript_Interp()
        {
            using (var graphics = Graphics.FromImage(img))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            }
        }

        public void Dispose()
        {
            this.currentPen.Dispose();
            this.img.Dispose();
        }

        public Image Load(string file)
        {
            m_fileName = file;
            FileInfo finfo = new FileInfo(m_fileName);

            if( !finfo.Exists)
                return img;

            var fs = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    string Line = sr.ReadLine();
                    // split the line on spaces or tabs, remove empty entries
                    string[] splits = Line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (splits.Length == 0 || splits[0].Length == 0)
                        continue;

                    switch (splits[0].Substring(0, 1))
                    {
                        // defines a new operator
                        case "/":
                            {
                                string key = splits[0].Substring(1, splits[0].Length - 1);
                                string value = "";
                                if (splits[1].StartsWith("{"))
                                {
                                    //edge case to fix a bug, also why above is startswith and not ==
                                    if (splits[1].Length > 1)
                                        value += splits[1].Substring(1,splits[1].Length-1) + " ";

                                    int i = 2;
                                    while (i < splits.Length && splits[i] != "}")
                                    {
                                        //yes this only goes 1 deep, and techically it can be n-deep
                                        if (CustomOps.ContainsKey(splits[i]))
                                            value += CustomOps[splits[i]];
                                        else
                                            value += splits[i] + " ";
                                        i++;
                                    }
                                    //for the one case where the command is split across 2 lines...
                                    if (i == splits.Length && splits[i-1] != "}")
                                    {
                                        Line = sr.ReadLine();
                                        splits = Line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                        i = 0;
                                        while (i < splits.Length && splits[i] != "}")
                                        {
                                            if (CustomOps.ContainsKey(splits[i]))
                                                value += CustomOps[splits[i]];
                                            else
                                                value += splits[i] + " ";
                                            i++;
                                        }
                                    }
                                }
                                if (!CustomOps.ContainsKey(key))
                                    CustomOps.Add(key, value);
                            }; break;
                        // % (not in a command) is a comment, %% is a Document structuring convention (DSC),
                        // which communicates structure and printing requirements, 
                        // but does not affect language page descrition (final output)
                        case "%":
                            {
                                if (splits[0].Length > 1 && splits[0].Substring(0, 2) == "%%")
                                {
                                    //add code ehre to parse the boarder comment to size the image
                                    break;// fill more in here later?
                                }
                                else
                                    break;
                            }; //break;
                        default:
                            {
                                data.AddRange(splits);
                            }; break;
                    }
                }
            }

            // now that we gone and read the file it, render us the picture
            Execute();

            // Apparnetly it renders upside down? flip it
            img.RotateFlip(RotateFlipType.RotateNoneFlipY);

            finfo = null;
            return img;
        }

        private void Execute()
        {
            for (int i = 0; i < data.Count; i++)
            {
                //handle arrays
                if (data[i] == "[")
                {
                    string fullArray = string.Empty;

                    while (i < data.Count && data[i] != "]")
                    {
                        fullArray += data[i] + " ";
                        i++;
                    }
                    //do we need to do one last step to add the ]?
                    fullArray += data[i];

                    operand.Push(fullArray);
                }
                else if (data[i].Trim().Length == 0 || data[i].StartsWith("%"))
                    continue;
                else
                    Evaluate(data[i]);
            }
        }

        private void Evaluate(string msg)
        {
            if (CustomOps.ContainsKey(msg))
            {
                string[] splits = CustomOps[msg].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < splits.Length; i++)
                    Evaluate(splits[i]);
            }
            else
            {
                switch (msg.ToLower())
                {
                    case "add"://pop 2 and add, then push back to stack
                        {
                            double val2 = double.Parse(operand.Pop());
                            double val1 = double.Parse(operand.Pop());
                            double res = val1 + val2;
                            operand.Push(res.ToString());
                        }; break;
                    case "sub"://pop 2 and subtract, then push back to stack
                        {
                            double val2 = double.Parse(operand.Pop());
                            double val1 = double.Parse(operand.Pop());
                            double res = val1 - val2;
                            operand.Push(res.ToString());
                        }; break;
                    case "div"://pop 2 and divide, then push back to stack
                        {
                            double val2 = double.Parse(operand.Pop());
                            double val1 = double.Parse(operand.Pop());
                            double res = val1 / val2;
                            operand.Push(res.ToString());
                        }; break;
                    case "exch"://pop 2 and push back in exchanged order
                        {
                            string val1 = operand.Pop();
                            string val2 = operand.Pop();
                            operand.Push(val1);
                            operand.Push(val2);

                        }; break;
                    case "index"://push the current x and y to the stack
                        {
                            int place = int.Parse(operand.Pop());
                            string[] vales = new string[place+1];
                            for (int i = 0; i < vales.Length; i++)
                                vales[i] = operand.Pop();
                            for (int i = vales.Length - 1; i >= 0; i--)
                                operand.Push(vales[i]);
                            operand.Push(vales[place]);
                        }; break;
                    case "pop"://push the current x and y to the stack
                        {
                            operand.Pop();
                        }; break;
                    case "currentpoint"://push the current x and y to the stack
                        {
                            operand.Push(currentPoint.X.ToString());
                            operand.Push(currentPoint.Y.ToString());
                        }; break;
                    case "moveto"://pop 2 and set the new currentpoint as that location
                        {
                            float y = float.Parse(operand.Pop());
                            float x = float.Parse(operand.Pop());
                            currentPoint = new PointF(x, y);

                        }; break;
                    case "rmoveto"://pop 2 and set the new currentpoint using deltas
                        {
                            float dy = float.Parse(operand.Pop());
                            float dx = float.Parse(operand.Pop());
                            currentPoint = new PointF(currentPoint.X + dx, currentPoint.Y + dy);

                        }; break;
                    case "lineto"://pop 2 and draw line to that location
                        {
                            float y = float.Parse(operand.Pop());
                            float x = float.Parse(operand.Pop());
                            using (var graphics = Graphics.FromImage(img))
                                graphics.DrawLine(currentPen, currentPoint.X, currentPoint.Y, x, y);
                            currentPoint = new PointF(x, y);

                        }; break;
                    case "rlineto"://pop 2 and draw line to new location as delta of current location
                        {
                            float dy = float.Parse(operand.Pop());
                            float dx = float.Parse(operand.Pop());
                            using (var graphics = Graphics.FromImage(img))
                                graphics.DrawLine(currentPen, currentPoint.X, currentPoint.Y, currentPoint.X + dx, currentPoint.Y + dy);
                            currentPoint = new PointF(currentPoint.X + dx, currentPoint.Y + dy);

                        }; break;
                    case "setgray"://push the current x and y to the stack
                        {
                            float gray = float.Parse(operand.Pop());
                            gray *= 255;
                            currentPen.Color = Color.FromArgb((int)gray, (int)gray, (int)gray);
                        }; break;
                    case "setlinewidth"://push the current x and y to the stack
                        {
                            float width = float.Parse(operand.Pop());
                            currentPen.Width = width;
                        }; break;
                    case "setrgbcolor"://push the current x and y to the stack
                        {
                            float blue = float.Parse(operand.Pop()) * 255;
                            float green = float.Parse(operand.Pop()) * 255;
                            float red = float.Parse(operand.Pop()) * 255;

                            currentPen.Color = Color.FromArgb((int)red, (int)green, (int)blue);
                        }; break;
                    case "setdash"://push the current x and y to the stack
                        {
                            int offset = int.Parse(operand.Pop());
                            string array = operand.Pop();
                            string[] splits = array.Split(' ');
                            float[] arrayvals = new float[splits.Length - 2];//not the [ or ]
                            for (int i = 0; i < splits.Length - 2; i++)
                                arrayvals[i] = float.Parse(splits[i + 1]);

                            if (arrayvals.Length == 0)
                                currentPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            else
                            {
                                currentPen.DashPattern = arrayvals;
                                currentPen.DashOffset = offset;
                            }

                        }; break;
                    case "translate"://push the current x and y to the stack
                        {
                            float ty = float.Parse(operand.Pop());
                            float tx = float.Parse(operand.Pop());
                            using (var graphics = Graphics.FromImage(img))
                                graphics.TranslateTransform(tx, ty);
                        }; break;
                    case "rotate"://push the current x and y to the stack
                        {
                            float deg_angle = float.Parse(operand.Pop());
                            using (var graphics = Graphics.FromImage(img))
                                graphics.RotateTransform(deg_angle);

                        }; break;
                    case "setlinejoin"://push the current x and y to the stack
                        {
                            int type = int.Parse(operand.Pop());
                            switch (type)
                            {
                                case 0:
                                    currentPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter; break;
                                case 1:
                                    currentPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round; break;
                                case 2:
                                    currentPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel; break;
                            }
                        }; break;
                    // these are the words we are ignoring
                    case "gsave":
                    case "current":
                    case "context":
                    case "stroke":
                    case "closepath":
                    case "fill":
                    case "bind":
                    case "def": break;
                    default:
                        {
                            operand.Push(msg);
                        }; break;
                }
            }
        }
    }
}
