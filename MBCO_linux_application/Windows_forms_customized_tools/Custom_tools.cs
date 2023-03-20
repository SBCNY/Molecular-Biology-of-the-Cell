using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Windows_forms_customized_tools
{

    class OwnTextBox : TextBox
    {
        private bool Silent { get; set; }

        public string SilentText
        {
            set
            {
                Silent = true;
                Text = value;
                Silent = false;
            }
        }

        public string SilentText_and_refresh
        {
            set
            {
                SilentText = value;
                base.Refresh();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!Silent)
            {
                base.OnTextChanged(e);
            }
        }
     }

    class OwnListBox : ListBox
    {
        private bool Silent { get; set; }
        public bool ReadOnly { get; set; }
        private int SelectedIndex_copy { get; set; }

        public string SilentText
        {
            set
            {
                Silent = true;
                Text = value;
                Silent = false;
            }
        }

        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                if (!ReadOnly)
                {
                    base.SelectedIndex = value;
                    SelectedIndex_copy = base.SelectedIndex;
                }
            }

        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (!ReadOnly)
                {
                    base.Font = value;
                }
            }

        }

        public int SilentSelectedIndex
        {
            set
            {
                Silent = true;
                bool readOnly = ReadOnly;
                ReadOnly = false;
                base.SelectedIndex = value;
                SelectedIndex_copy = base.SelectedIndex;
                Silent = false;
                ReadOnly = readOnly;
            }
        }

        public Font Silent_font
        {
            set
            {
                Silent = true;
                bool readOnly = ReadOnly;
                ReadOnly = false;
                base.Font = value;
                Silent = false;
                ReadOnly = readOnly;
            }
        }

        public int SilentSelectedIndex_and_topIndex
        {
            set
            {
                Silent = true;
                bool readOnly = ReadOnly;
                ReadOnly = false;
                base.SelectedIndex = value;
                TopIndex = value;
                SelectedIndex_copy = base.SelectedIndex;
                Silent = false;
                ReadOnly = readOnly;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!Silent)
            {
                base.OnTextChanged(e);
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if ((!Silent)&&(!ReadOnly))
            {
                base.OnSelectedIndexChanged(e);
            }
            else if (ReadOnly)
            {
                SilentSelectedIndex = SelectedIndex_copy;
            }
        }
    }

    class OwnCheckBox : CheckBox
    {
        private bool Silent { get; set; }
        public bool ReadOnly { get; set; }

        public bool SilentChecked
        {
            set
            {
                Silent = true;
                Checked = value;
                Silent = false;
                Refresh();
            }
        }

        

        protected override void OnCheckedChanged(EventArgs e)
        {
            if ((!Silent)&&(!ReadOnly))
            {
                base.OnCheckedChanged(e);
            }
        }
    }

    class MyCheckBox_button : Button
    {
        private bool is_checked { get; set; }
        public Color Checked_backColor { get; set; }
        public Color NotChecked_backColor { get; set; }
        public Color Checked_foreColor { get; set; }
        public Color NotChecked_foreColor { get; set; }

        public bool Checked
        {
            get { return is_checked; }
            set
            {
                is_checked = value;
                if (is_checked) 
                {  
                    this.BackColor = Checked_backColor; 
                    this.ForeColor = Checked_foreColor;
                }
                else 
                {
                    this.BackColor = NotChecked_backColor;
                    this.ForeColor = NotChecked_foreColor;
                }
            }
        }

        public MyCheckBox_button()
        {
            this.Text = "";
            this.Checked = false;
        }
        public void Button_pressed()
        {
            this.SilentChecked = !this.Checked;
        }

        public void Button_switch_to_positive()
        {
            this.SilentChecked = true;
        }

        public bool SilentChecked
        {
            set 
            { 
                this.Checked = value;
                this.Refresh();
            }
        }
    }
    class MyPanel : Panel
    {
        public Color Fill_color { get; set; }
        public Color Border_color { get; set; }
        public float Corner_radius { get; set; }

        public MyPanel()
        {
            Fill_color = Color.Transparent;
            Border_color = Color.Transparent;
            Corner_radius = 10F;
            base.Paint += OnPaint;
        }

        public void Draw_frame(Graphics graphics)
        {
            Rectangle border_rectangle = new Rectangle(base.Location, base.Size);

            GraphicsPath graphics_path = MyGrahics_class.Create_graphics_path(0, 0, this.Width - 1, this.Height - 1, Corner_radius);
            graphics.FillPath(new SolidBrush(Fill_color), graphics_path);
            graphics.DrawPath(new Pen(Border_color), graphics_path);
            graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), border_rectangle);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            this.Draw_frame(e.Graphics);
        }
    }

    class MyGrahics_class
    {
        public static GraphicsPath Create_graphics_path(float x, float y, float width, float height,
                                                        float radius)
        {
            float x_plus_width = x + width;
            float y_plus_height = y + height;
            float x_plus_width_minus_radius = x_plus_width - radius;
            float y_plus_height_minus_radius = y_plus_height - radius;
            float x_plus_radius = x + radius;
            float y_plus_radius = y + radius;
            float radius2 = radius * 2;
            float x_plus_width_minus_radius2 = x_plus_width - radius2;
            float y_plus_height_minus_radius2 = y_plus_height - radius2;

            GraphicsPath graphics_path = new GraphicsPath();
            graphics_path.StartFigure();

            //Top left corner
            graphics_path.AddArc(x, y, radius2, radius2, 180, 90);
            //Top right corner
            graphics_path.AddArc(x_plus_width_minus_radius2, y, radius2, radius2, 270, 90);
            //Bottom right corner
            graphics_path.AddArc(x_plus_width_minus_radius2, y_plus_height_minus_radius2, radius2, radius2, 0, 90);
            //Bottom left corner           
            graphics_path.AddArc(x, y_plus_height_minus_radius2, radius2, radius2, 90, 90);

            graphics_path.CloseFigure();
            return graphics_path;
        }
    }

}
