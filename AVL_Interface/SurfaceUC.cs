using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class SurfaceUC : UserControl
    {
        public class SectionSelectEventArgs : EventArgs
        {
            public int Selected_Section = -1;
            public SectionSelectEventArgs(int Section)
            {
                Selected_Section = Section;
            }
        }

        public delegate void SelectionEventHandler(SurfaceUC sender, SectionSelectEventArgs e);
        public static event SelectionEventHandler OnUpdateSelection = null;

        private AVL_File.Surface m_surf;
        private int m_currentSection = 0;
        //private bool m_beenEdited = false;

        private bool m_allowUpdates = true;

        public AVL_File.Surface DispSurface
        {
            get { return m_surf; }
        }

        public int SelectedSection
        {
            get { return m_currentSection; }
        }

        public SurfaceUC()
        {
            InitializeComponent();
        }

        public SurfaceUC(AVL_File.Surface surf): this()
        {
            m_surf = surf;
            ribbonTab1.Text = "SURFACE: " + surf.Name;
            ydupTextBox.TextBoxText = surf.YDUPLICATE.ToString();
            componentTextBox.TextBoxText = surf.COMPONENT.ToString();
            scaleTextBox.TextBoxText = surf.SCALE[0].ToString() + "," + surf.SCALE[1].ToString() + "," + surf.SCALE[2].ToString();
            translateTextBox.TextBoxText = surf.TRANSLATE[0].ToString() + "," + surf.TRANSLATE[1].ToString() + "," + surf.TRANSLATE[2].ToString();
            angleTextBox.TextBoxText = surf.ANGLE.ToString();

            nowakeCheck.Checked = surf.NOWAKE;
            noableCheck.Checked = surf.NOALBE;
            noloadCheck.Checked = surf.NOLOAD;

            chordwiseUpDown.TextBoxText = surf.Nchordwise.ToString();
            spanwiseUpDown.TextBoxText = surf.Nspanwise.ToString();

            string[] ControlNames = ControlSurfNames();
            for (int i = 0; i < ControlNames.Length; i++)
            {
                var lab = new RibbonLabel();
                lab.Text = ControlNames[i];
                controlSurfDropDown.DropDownItems.Add(lab);
                if (i == 0)
                    controlSurfDropDown.SelectedItem = lab;
            }

            ChangeSection(0);
            RecalculateStats();
            OnChangedSelection(this, new SectionSelectEventArgs(-1));
        }

        private string[] ControlSurfNames()
        {
            List<string> names = new List<string>();
            foreach (AVL_File.Surface.Section sec in m_surf.Sections)
                foreach (AVL_File.Surface.Section.Control con in sec.control_surfaces)
                    if (!names.Contains(con.Name))
                        names.Add(con.Name);

            return names.ToArray();
        }

        private void RecalculateStats()
        {
            /*double span = m_surf.Sections[m_surf.Sections.Count - 1].Y_LeadingEdge;
            double area = 0;
            for (int i = 0; i < m_surf.Sections.Count - 1; i++)
            {
                double single_span = m_surf.Sections[i + 1].Y_LeadingEdge - m_surf.Sections[i].Y_LeadingEdge;
                area += m_surf.Sections[i].Chord * single_span + (m_surf.Sections[i + 1].Chord - m_surf.Sections[i].Chord) * (single_span) / 2;
            }
            span *= 2;
            area *= 2;
            double AR = (span * span) / area;*/

            areaLabel.Text = "Surface Area: " + m_surf.Area.ToString("0.##");
            spanLabel.Text = "Surface Span: " + m_surf.Span.ToString("0.##");
            ARLabel.Text = "Surface AR: " + m_surf.AspectRatio.ToString("0.##");
        }

        public void ChangeSection(int rootSection)
        {
            if (m_surf == null || m_surf.Sections.Count == 0)
                return;

            //turn off the updates that results from changing the textboxes
            m_allowUpdates = false;

            //if this int actually represents a section, display it
            if (m_surf.Sections.Count > rootSection)
            {
                rootLabel.Text = "Section " + rootSection.ToString();
                rootxleUpDown.TextBoxText = m_surf.Sections[rootSection].X_LeadingEdge.ToString();
                rootyleUpDown.TextBoxText = m_surf.Sections[rootSection].Y_LeadingEdge.ToString();
                rootzleUpDown.TextBoxText = m_surf.Sections[rootSection].Z_LeadingEdge.ToString();

                rootChordUpDown.TextBoxText = m_surf.Sections[rootSection].Chord.ToString();
                rootAIncUpDown.TextBoxText = m_surf.Sections[rootSection].Angle_Incidence.ToString();

                if (m_surf.Sections[rootSection].AirfoilFile != string.Empty)
                {
                    rootAirfoilButton.Text = m_surf.Sections[rootSection].AirfoilFile;
                    rootUseAirfoilCheck.Checked = true;
                }
                else if (m_surf.Sections[rootSection].NACA != string.Empty)
                {
                    rootAirfoilButton.Text = "NACA " + m_surf.Sections[rootSection].NACA;
                    rootUseNACACheck.Checked = true;
                }

                rootAirfoilFileLabel.Text = "      " + m_surf.Sections[rootSection].AirfoilFile;
                rootNACATextbox.Text = "      " + m_surf.Sections[rootSection].NACA;
            }
            //make sure there is a tip section before displaying
            if (m_surf.Sections.Count > rootSection + 1)
            {
                tipLabel.Text = "Section " + (rootSection + 1).ToString();
                tipxleUpDown.TextBoxText = m_surf.Sections[rootSection + 1].X_LeadingEdge.ToString();
                tipyleUpDown.TextBoxText = m_surf.Sections[rootSection + 1].Y_LeadingEdge.ToString();
                tipzleUpDown.TextBoxText = m_surf.Sections[rootSection + 1].Z_LeadingEdge.ToString();

                tipChordUpDown.TextBoxText = m_surf.Sections[rootSection + 1].Chord.ToString();
                tipAIncUpDown.TextBoxText = m_surf.Sections[rootSection + 1].Angle_Incidence.ToString();

                if (m_surf.Sections[rootSection+1].AirfoilFile != string.Empty)
                {
                    tipAirfoilButton.Text = m_surf.Sections[rootSection+1].AirfoilFile;
                    tipUseAirfoilCheck.Checked = true;
                }
                else if (m_surf.Sections[rootSection+1].NACA != string.Empty)
                {
                    tipAirfoilButton.Text = "NACA " + m_surf.Sections[rootSection+1].NACA;
                    tipUseNACACheck.Checked = true;
                }

                tipAirfoilFileLabel.Text = m_surf.Sections[rootSection+1].AirfoilFile;
                tipNACATextbox.Text = m_surf.Sections[rootSection+1].NACA; 
            }

            if (rootSection == 0)
            {
                prevButton.Enabled = false;
                deleteButton.Enabled = false;
            }
            else
            {
                prevButton.Enabled = true;
                deleteButton.Enabled = true;
            }

            if (rootSection == m_surf.Sections.Count - 2)
            {
                nextButton.Text = "New Section";
                nextButton.Image = imageList1.Images[1];
            }
            else
            {
                if (nextButton.Text != "Next Section")
                {
                    nextButton.Text = "Next Section";
                    nextButton.Image = imageList1.Images[0];
                }
            }

            m_currentSection = rootSection;
            OnChangedSelection(this, new SectionSelectEventArgs(m_currentSection));
            //reenable textbox changes
            m_allowUpdates = true;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (nextButton.Text == "Next Section")
                ChangeSection(m_currentSection + 1);
            else
            {
                if (MessageBox.Show("Add another section to this surface?", "Add Section", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    AVL_File.Surface.Section newsec = new AVL_File.Surface.Section(m_surf);
                    AVL_File.Surface.Section lastsec = m_surf.Sections[m_surf.Sections.Count - 1];
                    newsec.Chord = lastsec.Chord;
                    newsec.AirfoilFile = lastsec.AirfoilFile;
                    newsec.NACA = lastsec.NACA;
                    newsec.Angle_Incidence = lastsec.Angle_Incidence;
                    newsec.X_LeadingEdge = lastsec.X_LeadingEdge;
                    newsec.Y_LeadingEdge = lastsec.Y_LeadingEdge + 10;
                    newsec.Z_LeadingEdge = lastsec.Z_LeadingEdge;
                    m_surf.Sections.Add(newsec);
                    ChangeSection(m_surf.Sections.Count - 2);
                    RecalculateStats();
                }
            }
                
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            ChangeSection(m_currentSection - 1);
        }

        private static void OnChangedSelection(SurfaceUC inst, SectionSelectEventArgs e)
        {
            if (OnUpdateSelection != null)
                OnUpdateSelection(inst, e);
        }

        private void ribbonTab2_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeSection(m_currentSection);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this section? Note, this will not delete sections beyond this one.", "Section Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_surf.Sections.RemoveAt(m_currentSection + 1);
                RecalculateStats();
                ChangeSection(m_currentSection-1);
            }
        }
        private void deleteSurfButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this entire Surface?", "Surface Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_surf.Parent.Surfaces.Remove(m_surf);

                this.Dispose();
            }
        }
        private void setRefButton_Click(object sender, EventArgs e)
        {
            m_surf.Parent.Sref = m_surf.Area;
            m_surf.Parent.Bref = m_surf.Span;
            m_surf.Parent.Cref = m_surf.MeanChord;
            Control p = this.Parent;
            while(p.GetType() != typeof(Aircraft_Info) && p.Parent!= null)
                p = p.Parent;

            if (p != null)
                ((Aircraft_Info)p).RefreshValues();
        }

#region Textbox handlers
        /// <summary>
        /// One function to make sure all our TextUpDowns only allow numbers, decimals, and negative signs
        /// Todo: Add inline math?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUpDown_TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as RibbonTextBox).TextBoxText.IndexOf('.') > -1))
                e.Handled = true;
        }

        private void noptionsCheck_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            m_surf.NOWAKE = nowakeCheck.Checked;
            m_surf.NOALBE = noableCheck.Checked;
            m_surf.NOLOAD = noloadCheck.Checked;
        }

        private void ThreeNumb_TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != ','))
                e.Handled = true;
            
            // only allow 2 commas
            if ((e.KeyChar == ',') && ((sender as RibbonTextBox).TextBoxText.Split(',').Length > 2))
                e.Handled = true;
        }

        private void ydupTextBox_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (!m_allowUpdates)
                return;

            double temp = 0;
            if (double.TryParse(ydupTextBox.TextBoxText, out temp))
                m_surf.YDUPLICATE = temp;
        }

        private void componentTextBox_TextBoxTextChanged(object sender, EventArgs e)
        {
            int temp = 0;
            if (int.TryParse(componentTextBox.TextBoxText, out temp))
            {
                if (temp < 0)
                    temp = -1;
                m_surf.COMPONENT = temp;
            }
        }

        private void angleTextBox_TextBoxTextChanged(object sender, EventArgs e)
        {
            double temp = 0;
            if (double.TryParse(angleTextBox.TextBoxText, out temp))
                m_surf.ANGLE = temp;
        }

        private void chordwiseUpDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            double temp = 0;
            if (double.TryParse(chordwiseUpDown.TextBoxText, out temp))
            {
                m_surf.Nchordwise = (int)temp;
                ChangeSection(m_currentSection);
            }
        }
        private void spanwiseUpDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            double temp = 0;
            if (double.TryParse(spanwiseUpDown.TextBoxText, out temp))
            {
                m_surf.Nspanwise = (int)temp;
                ChangeSection(m_currentSection);
            }
        }

        private void scaleTextBox_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (!m_allowUpdates)
                return;

            string[] splits = scaleTextBox.TextBoxText.Split(',');
            if (splits.Length > 0 && splits.Length<=3)
            {
                for (int i = 0; i < splits.Length; i++)
                {
                    double temp = 0;
                    if (double.TryParse(splits[i], out temp))
                        m_surf.SCALE[i] = temp;
                }
            }
        }

        private void translateTextBox_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (!m_allowUpdates)
                return;

            string[] splits = translateTextBox.TextBoxText.Split(',');
            if (splits.Length > 0 && splits.Length <= 3)
            {
                for (int i = 0; i < splits.Length; i++)
                {
                    double temp = 0;
                    if (double.TryParse(splits[i], out temp))
                        m_surf.TRANSLATE[i] = temp;
                }
            }
        }

        private void rootSectUpDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (!m_allowUpdates)
                return;

            double temp = 0;
            if (double.TryParse((sender as RibbonUpDown).TextBoxText, out temp))
            {
                switch ((sender as RibbonUpDown).Text.Substring(0,1))
                {
                    case "X": m_surf.Sections[m_currentSection].X_LeadingEdge = temp; break;
                    case "Y": m_surf.Sections[m_currentSection].Y_LeadingEdge = temp; break;
                    case "Z": m_surf.Sections[m_currentSection].Z_LeadingEdge = temp; break;
                    case "C":
                        {
                            if (temp < 0)
                            {
                                (sender as RibbonUpDown).TextBoxText = "0";
                                return;
                            }
                            else
                                m_surf.Sections[m_currentSection].Chord = temp;
                        } break;
                    case "A": m_surf.Sections[m_currentSection].Angle_Incidence = temp; break;
                }
                OnChangedSelection(this, new SectionSelectEventArgs(m_currentSection));
                RecalculateStats();
            }
        }

        private void tipSectUpDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (!m_allowUpdates)
                return;

            double temp = 0;
            if (double.TryParse((sender as RibbonUpDown).TextBoxText, out temp))
            {
                switch ((sender as RibbonUpDown).Text.Substring(0, 1))
                {
                    case "X": m_surf.Sections[m_currentSection + 1].X_LeadingEdge = temp; break;
                    case "Y": m_surf.Sections[m_currentSection + 1].Y_LeadingEdge = temp; break;
                    case "Z": m_surf.Sections[m_currentSection + 1].Z_LeadingEdge = temp; break;
                    case "C": 
                        {
                            if (temp < 0)
                            {
                                (sender as RibbonUpDown).TextBoxText = "0";
                                return;
                            }
                            else
                                m_surf.Sections[m_currentSection + 1].Chord = temp;
                        } break;
                    case "A": m_surf.Sections[m_currentSection + 1].Angle_Incidence = temp; break;
                }
                OnChangedSelection(this, new SectionSelectEventArgs(m_currentSection));
                RecalculateStats();
            }
        }

        private void UpDown_UpButtonClicked(object sender, MouseEventArgs e)
        {
            RibbonUpDown rud = (sender as RibbonUpDown);
            if (rud != null)
            {
                double val = double.Parse(rud.TextBoxText);
                val++;
                rud.TextBoxText = val.ToString();
            }
        }

        private void UpDown_DownButtonClicked(object sender, MouseEventArgs e)
        {
            RibbonUpDown rud = (sender as RibbonUpDown);
            if (rud != null)
            {
                double val = double.Parse(rud.TextBoxText);
                val--;
                rud.TextBoxText = val.ToString();
            }
        }

        private void noNegUpDown_DownButtonClicked(object sender, MouseEventArgs e)
        {
            RibbonUpDown rud = (sender as RibbonUpDown);
            if (rud != null)
            {
                double val = double.Parse(rud.TextBoxText);
                if (val > 0)
                {
                    val--;
                    rud.TextBoxText = val.ToString();
                }
            }
        }
#endregion
#region controlsurfaces
        int cont_start_sec = -1;
        int cont_end_sec = -1;
        AVL_File.Surface.Section.Control cont_start;
        AVL_File.Surface.Section.Control cont_end;
        bool switching = false;

        private void controlSurfDropDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            switching = true;

            cont_start_sec = -1;
            cont_end_sec = -1;
            var contsurfs = new List<AVL_File.Surface.Section.Control>();

            //get all the control surf classes that share this name
            for (int i = 0; i < m_surf.Sections.Count; i++)
            {
                for (int j = 0; j < m_surf.Sections[i].control_surfaces.Count; j++)
                {
                    if (m_surf.Sections[i].control_surfaces[j].Name == controlSurfDropDown.TextBoxText)
                    {
                        if (cont_start_sec == -1)
                            cont_start_sec = i;

                        if (cont_end_sec < i)
                            cont_end_sec = i;

                        contsurfs.Add(m_surf.Sections[i].control_surfaces[j]);
                    }
                }
            }

            if(contsurfs.Count < 2 )
                return;

            cont_start = contsurfs[0];
            cont_end = contsurfs[contsurfs.Count - 1];

            controlDefCombobox.SelectedItem = cont_start.SgnDup == 1 ? togetherLabel : oppositeLabel;
            contHingeTextbox.TextBoxText = cont_start.HVec[0].ToString() + "," + cont_start.HVec[1].ToString() + "," + cont_start.HVec[2].ToString();

            //section number labels
            cstartSecUpDown.TextBoxText = cont_start_sec.ToString();
            cendSecUpDown.TextBoxText = cont_end_sec.ToString();

            //% chord labels
            cstartChordTextBox.TextBoxText = ((1 - cont_start.Xhinge)*100).ToString();
            cendChordTextBox.TextBoxText = ((1 - cont_end.Xhinge) * 100).ToString();

            //Gain labels
            cstartGainTextBox.TextBoxText = cont_start.gain.ToString();
            cendGainTextBox.TextBoxText = cont_start.gain.ToString();

            switching = false;
        }

        private void controlDefCombobox_TextBoxTextChanged(object sender, EventArgs e)
        {
            int sgndup = 0;
            if (controlDefCombobox.SelectedItem == togetherLabel)
                sgndup = 1;
            else if (controlDefCombobox.SelectedItem == oppositeLabel)
                sgndup = -1;

            if (sgndup != 0)
                for (int i = cont_start_sec; i <= cont_end_sec && i < m_surf.Sections.Count; i++)
                    for (int j = 0; j < m_surf.Sections[i].control_surfaces.Count; j++)
                        if (m_surf.Sections[i].control_surfaces[j].Name == controlSurfDropDown.TextBoxText)
                            m_surf.Sections[i].control_surfaces[j].SgnDup = sgndup;
        }

        private void contHingeTextbox_TextBoxTextChanged(object sender, EventArgs e)
        {
            double[] HVec = new double[3];

            string[] splits = translateTextBox.TextBoxText.Split(',');
            if (splits.Length > 0 && splits.Length <= 3)
            {
                for (int i = 0; i < splits.Length; i++)
                {
                    double temp = 0;
                    if (double.TryParse(splits[i], out temp))
                        HVec[i] = temp;
                }
            }

            for (int i = cont_start_sec; i <= cont_end_sec && i < m_surf.Sections.Count; i++)
                for (int j = 0; j < m_surf.Sections[i].control_surfaces.Count; j++)
                    if (m_surf.Sections[i].control_surfaces[j].Name == controlSurfDropDown.TextBoxText)
                        m_surf.Sections[i].control_surfaces[j].HVec = HVec;
        }

        private void cstartChordTextBox_TextBoxTextChanged(object sender, EventArgs e)
        {
            UpdateControlSurfaces();
        }

        private void cstartSecUpDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            int.TryParse(cstartSecUpDown.TextBoxText, out cont_start_sec);
        }

        private void cendSecUpDown_TextBoxTextChanged(object sender, EventArgs e)
        {
            int.TryParse(cendSecUpDown.TextBoxText, out cont_end_sec);
        }

        private void UpdateControlSurfaces()
        {
            if (switching)
                return;

            double startChord = 0;
            double endChord = 0;
            double startGain = 1;
            double endGain = 1;

            if (double.TryParse(cstartChordTextBox.TextBoxText, out startChord))
                startChord = (100 - startChord) / 100;

            if (double.TryParse(cendChordTextBox.TextBoxText, out endChord))
                endChord = (100 - endChord) / 100;

            double.TryParse(cstartGainTextBox.TextBoxText, out startGain);
            double.TryParse(cendGainTextBox.TextBoxText, out endGain);

            for (int i = cont_start_sec; i <= cont_end_sec && i < m_surf.Sections.Count; i++)
            {
                for (int j = 0; j < m_surf.Sections[i].control_surfaces.Count; j++)
                {
                    if (m_surf.Sections[i].control_surfaces[j].Name == controlSurfDropDown.TextBoxText)
                    {
                        m_surf.Sections[i].control_surfaces[j].Xhinge = startChord + (endChord - startChord) / (cont_end_sec - cont_start_sec) * (i - cont_start_sec);
                        m_surf.Sections[i].control_surfaces[j].gain = startGain + (endGain - startGain) / (cont_end_sec - cont_start_sec) * (i - cont_start_sec);
                    }
                }
            }
            OnChangedSelection(this, new SectionSelectEventArgs(-1));
        }

        private void cstartSecUpDown_DownButtonClicked(object sender, MouseEventArgs e)
        {
            //reducing the section number of the start means adding a new section 
            if (sender == cstartSecUpDown)
            {
                int val = int.Parse(cstartSecUpDown.TextBoxText);
                if (val > 0)
                {
                    val--;
                    cstartSecUpDown.TextBoxText = val.ToString();
                    ExtendControlSurface(val);
                }
            }
            //reducing the endsection means removing that last control surf
            else if (sender == cendSecUpDown)
            {
                int valstart = int.Parse(cstartSecUpDown.TextBoxText);
                int valend = int.Parse(cendSecUpDown.TextBoxText);
                if (valend > valstart+1)
                {
                    valend--;
                    cendSecUpDown.TextBoxText = valend.ToString();
                    RemoveControlSurface(valend + 1);
                }
            }
        }

        private void cendSecUpDown_UpButtonClicked(object sender, MouseEventArgs e)
        {
            //increasing the end section means adding a new surf to the section
            if (sender == cendSecUpDown)
            {
                int val = int.Parse(cendSecUpDown.TextBoxText);
                if (val < m_surf.Sections.Count-1)
                {
                    val++;
                    cendSecUpDown.TextBoxText = val.ToString();
                    ExtendControlSurface(val);
                }
            }
            //increasing the start section means removing the first section's control
            else if (sender == cstartSecUpDown)
            {
                int valstart = int.Parse(cstartSecUpDown.TextBoxText);
                int valend = int.Parse(cendSecUpDown.TextBoxText);
                if (valstart < valend-1)
                {
                    valstart++;
                    cstartSecUpDown.TextBoxText = valstart.ToString();
                    RemoveControlSurface(valstart - 1);
                }
            }
        }

        private void ExtendControlSurface(int sectionNum)
        {
            int sgndup = 0;
            if (controlDefCombobox.SelectedItem == togetherLabel)
                sgndup = 1;
            else if (controlDefCombobox.SelectedItem == oppositeLabel)
                sgndup = -1; 
            
            double[] HVec = new double[3];

            string[] splits = translateTextBox.TextBoxText.Split(',');
            if (splits.Length > 0 && splits.Length <= 3)
            {
                for (int i = 0; i < splits.Length; i++)
                {
                    double temp = 0;
                    if (double.TryParse(splits[i], out temp))
                        HVec[i] = temp;
                }
            }

            AVL_File.Surface.Section.Control cont = new AVL_File.Surface.Section.Control(controlSurfDropDown.TextBoxText);
            cont.SgnDup = sgndup;
            cont.HVec = HVec;

            m_surf.Sections[sectionNum].control_surfaces.Add(cont);
            UpdateControlSurfaces();
        }

        private void RemoveControlSurface(int sectionNum)
        {
            for (int j = 0; j < m_surf.Sections[sectionNum].control_surfaces.Count; j++)
            {
                if (m_surf.Sections[sectionNum].control_surfaces[j].Name == controlSurfDropDown.TextBoxText)
                {
                    m_surf.Sections[sectionNum].control_surfaces.RemoveAt(j); 
                    UpdateControlSurfaces();
                    return;
                }
            }
        }

        private void deleteControlButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_surf.Sections.Count; i++)
            {
                for (int j = 0; j < m_surf.Sections[i].control_surfaces.Count; j++)
                {
                    if (m_surf.Sections[i].control_surfaces[j].Name == controlSurfDropDown.TextBoxText)
                    {
                        m_surf.Sections[i].control_surfaces.RemoveAt(j);
                    }
                }
            }
            controlSurfDropDown.DropDownItems.Remove(controlSurfDropDown.SelectedItem);
            if (controlSurfDropDown.DropDownItems.Count > 0)
                controlSurfDropDown.SelectedItem = controlSurfDropDown.DropDownItems[0];
            else
                controlSurfDropDown.TextBoxText = "";
            OnChangedSelection(this, new SectionSelectEventArgs(-1));
        }

        private void addControlButton_Click(object sender, EventArgs e)
        {
            InputBox ib = new InputBox("New Control Surface", "Enter name for this new control surface:");
            if (ib.ShowDialog() == DialogResult.OK)
            {
                string controlName = ib.InputText;
                if (m_surf.Sections.Count >= 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var consurf = new AVL_File.Surface.Section.Control(controlName);
                        m_surf.Sections[i].control_surfaces.Add(consurf);
                    }
                    var lab = new RibbonLabel();
                    lab.Text = controlName;
                    controlSurfDropDown.DropDownItems.Add(lab);
                    controlSurfDropDown.SelectedItem = lab;
                }
            }
        }
#endregion
#region airfoils
        private void rootUseAirfoilCheck_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            if(rootUseAirfoilCheck.Checked)
            {
                m_surf.Sections[m_currentSection].NACA = "";
                m_surf.Sections[m_currentSection].UseAirfoilFile = true;
                m_surf.Sections[m_currentSection].AirfoilFile = rootAirfoilFileLabel.Text;
            }
            else if(rootUseNACACheck.Checked)
            {
                m_surf.Sections[m_currentSection].UseAirfoilFile = false;
                m_surf.Sections[m_currentSection].NACA = rootNACATextbox.TextBoxText;
            }
        }

        private void rootselectAfoilButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select section root Airfoil file";
            ofd.Multiselect = false;
            ofd.Filter = "DAT Files (*.dat)|*.dat";
            ofd.InitialDirectory = Properties.Settings.Default.AVL_Location;
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.CheckFileExists)
                    return;

                m_surf.Sections[m_currentSection].AirfoilFile = ofd.SafeFileName;
                m_surf.Sections[m_currentSection].UseAirfoilFile = true;
                rootAirfoilFileLabel.Text = ofd.SafeFileName;
                rootAirfoilButton.Text = ofd.SafeFileName;
            }
        }

        private void tipselectAfoilButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select section tip Airfoil file";
            ofd.Multiselect = false;
            ofd.Filter = "DAT Files (*.dat)|*.dat";
            ofd.InitialDirectory = Properties.Settings.Default.AVL_Location;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.CheckFileExists)
                    return;

                m_surf.Sections[m_currentSection + 1].AirfoilFile = ofd.SafeFileName;
                m_surf.Sections[m_currentSection+1].UseAirfoilFile = true;
                tipAirfoilFileLabel.Text = ofd.SafeFileName;
                tipAirfoilButton.Text = ofd.SafeFileName;
            }
        }

        private void tipUseAirfoilCheck_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            if (tipUseAirfoilCheck.Checked)
            {
                m_surf.Sections[m_currentSection + 1].NACA = "";
                m_surf.Sections[m_currentSection + 1].UseAirfoilFile = true;
                m_surf.Sections[m_currentSection + 1].AirfoilFile = tipAirfoilFileLabel.Text;
            }
            else if (tipUseNACACheck.Checked)
            {
                m_surf.Sections[m_currentSection + 1].UseAirfoilFile = false;
                m_surf.Sections[m_currentSection + 1].NACA = tipNACATextbox.TextBoxText;
            }
        }

        private void rootNACATextbox_TextBoxTextChanged(object sender, EventArgs e)
        {
            m_surf.Sections[m_currentSection].NACA = rootNACATextbox.TextBoxText;
        }

        private void tipNACATextbox_TextBoxTextChanged(object sender, EventArgs e)
        {
            m_surf.Sections[m_currentSection + 1].NACA = tipNACATextbox.TextBoxText;
        }
#endregion
    }
}
