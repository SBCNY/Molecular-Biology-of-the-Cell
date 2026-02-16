using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Common_functions.Form_tools;
using PdfSharp.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;

namespace Windows_forms_customized_tools
{

    class OwnTextBox : TextBox
    {
        private bool Silent { get; set; }

        public OwnTextBox()
        {
            base.Multiline = true;
            base.Dock = DockStyle.None;
        }

        public string SilentText
        {
            set
            {
                Silent = true;
                Text = value;
                Silent = false;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            base.BackColor = this.BackColor; // Triggers override
            base.ForeColor = this.ForeColor;
        }
        public BorderStyle BorderStyle_ownTextBox
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
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
            GraphicsPath graphics_path;
            graphics_path = MyGrahics_class.Create_graphics_path(0, 0, this.Width - 1, this.Height - 1, Corner_radius);
            graphics.FillPath(new SolidBrush(Fill_color), graphics_path);
            graphics.DrawPath(new Pen(Border_color), graphics_path);
            graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), border_rectangle);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (Corner_radius>0)
            {
                this.Draw_frame(e.Graphics);
            }
            else
            {
            }
        }
    }

    enum MyPanel_label_status_enum {  E_m_p_t_y, Regular, Red_warning, Purple_warning }

    class MyPanel_label : Panel
    {
        public Label FullSize_label { get; private set; }
        public int Initial_fontSize { get; set; }
        public FontStyle Font_style { get; set; }
        public FontFamily Font_family { get; set; }
        public ContentAlignment TextAlign
        {
            set { FullSize_label.TextAlign = value;  }
        }
        public MyPanel_label_status_enum Status { get; set; }

        public MyPanel_label()
        {
            FullSize_label = new Label();
            Controls.Add(FullSize_label);
            FullSize_label.Location = new Point(0, 0);
            FullSize_label.Height = Height;
            FullSize_label.Width = Width;
            FullSize_label.BorderStyle = BorderStyle.None;
            Font_style = FontStyle.Regular;
            Font_family = FontFamily.GenericSerif;
            Initial_fontSize = 10;
            Status = MyPanel_label_status_enum.Regular;
        }
        public void Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(int left_position, int top_position, int right_position, int bottom_position, Form1_default_settings_class default_settings)
        {
            left_position += 2 * default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
            right_position -= 2 * default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
            top_position += default_settings.Resolution_parameter.Label_Y_distance_from_referenceBorder;
            bottom_position -= default_settings.Resolution_parameter.Label_Y_distance_from_referenceBorder;

            if (right_position < left_position) { throw new Exception(); }
            if (bottom_position < top_position) { throw new Exception(); }

            if (default_settings.Is_mono)
            {
                int remove_width = (int)Math.Round(0.025 * (right_position - left_position));
                left_position += remove_width;
                right_position -= remove_width;
            }

            Location = new Point(left_position, top_position);
            Height = bottom_position - top_position;
            Width = right_position - left_position;
            FullSize_label.Height = Height;
            FullSize_label.Width = Width;

            base.BackColor = default_settings.Color_label_backColor;
            base.ForeColor = default_settings.Color_label_backColor;
            switch (Status)
            {
                case MyPanel_label_status_enum.Regular:
                    FullSize_label.BackColor = default_settings.Color_label_backColor; ;
                    FullSize_label.ForeColor = default_settings.ExplanationText_color;
                    break;
                case MyPanel_label_status_enum.Red_warning:
                    FullSize_label.BackColor = default_settings.Warnings_back_color;
                    FullSize_label.ForeColor = default_settings.Warnings_fore_color;
                    break;
                case MyPanel_label_status_enum.Purple_warning:
                    FullSize_label.BackColor = default_settings.Incompatible_dataset_backColor;
                    FullSize_label.ForeColor = default_settings.Incompatible_dataset_foreColor;
                    break;
                throw new Exception();
            }
            Adjust_fontSize_to_textBoxBorders(default_settings);
            FullSize_label.Refresh();
        }
        public void Set_rightX_midY_position_text_with_fixed_font_and_adjust_width_and_height(string text, Font font, int rightX, int midY)
        {
            FullSize_label.Text = (string)text.Clone();
            FullSize_label.Font = font;
            FullSize_label.TextAlign = ContentAlignment.MiddleRight;
            Size label_size = TextRenderer.MeasureText(FullSize_label.Text, FullSize_label.Font);
            int width = label_size.Width;
            int height = label_size.Height;
            int left_position = rightX - width;
            int top_position = midY - (int)Math.Round(0.5F * height);
            Location = new Point(left_position, top_position);
            Height = height;
            Width = width;
        }
        public void Set_leftX_midY_position_text_with_fixed_font_and_adjust_width_and_height(string text, Font font, int leftX, int midY)
        {
            FullSize_label.Text = (string)text.Clone();
            FullSize_label.Font = font;
            FullSize_label.TextAlign = ContentAlignment.MiddleRight;
            Size label_size = TextRenderer.MeasureText(FullSize_label.Text, FullSize_label.Font);
            int width = label_size.Width;
            int height = label_size.Height;
            int left_position = leftX;
            int top_position = midY - (int)Math.Round(0.5F * height);
            Location = new Point(left_position, top_position);
            Height = height;
            Width = width;
        }
        private void Adjust_fontSize_to_textBoxBorders(Form1_default_settings_class default_settings)
        {
            string text = FullSize_label.Text;
            Size textSize;
            FullSize_label.Font = default_settings.Adjust_font_size_and_text_to_area_size(ref text, FullSize_label.Font, out textSize, Width, Height, default_settings.Max_fontSize_defaultBold, -1);
            FullSize_label.Text = text;
            FullSize_label.Font = new Font(FullSize_label.Font, Font_style);

            Size label_size = TextRenderer.MeasureText(FullSize_label.Text, FullSize_label.Font);

            int new_x;
            int new_y;
            int new_width;
            int new_height;
            new_width = label_size.Width;
            new_height = label_size.Height;

            int left_side_x = 0;
            int center_x = 0 + (int)Math.Round(0.5F * this.Width - 0.5F * new_width);
            int right_side_x = 0 + this.Width - new_width;
            int top_side_y = 0;
            int center_y = 0 + (int)Math.Round(0.5F * this.Height - 0.5F * new_height);
            int bottom_side_y = 0 + this.Height - new_height;


            switch (FullSize_label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    new_y = top_side_y;
                    new_x = left_side_x;
                    break;
                case ContentAlignment.TopCenter:
                    new_y = top_side_y;
                    new_x = center_x;
                    break;
                case ContentAlignment.TopRight:
                    new_y = top_side_y;
                    new_x = right_side_x;
                    break;
                case ContentAlignment.MiddleLeft:
                    new_y = center_y;
                    new_x = left_side_x;
                    break;
                case ContentAlignment.MiddleCenter:
                    new_y = center_y;
                    new_x = center_x;
                    break;
                case ContentAlignment.MiddleRight:
                    new_y = center_y;
                    new_x = right_side_x;
                    break;
                case ContentAlignment.BottomLeft:
                    new_y = bottom_side_y;
                    new_x = left_side_x;
                    break;
                case ContentAlignment.BottomCenter:
                    new_y = bottom_side_y;
                    new_x = center_x;
                    break;
                case ContentAlignment.BottomRight:
                    new_y = bottom_side_y;
                    new_x = right_side_x;
                    break;
                default:
                    throw new Exception();
            }
            FullSize_label.Location = new Point(new_x, new_y);

            FullSize_label.Height = new_height;
            FullSize_label.Width = new_width;
        }

        public void Set_silent_text_adjustFontSize_and_refresh(string text, Form1_default_settings_class default_settings)
        {
            FullSize_label.Text = text;
            FullSize_label.Font = new Font(FullSize_label.Font, Font_style);
            Adjust_fontSize_to_textBoxBorders(default_settings);
            FullSize_label.Refresh();
        }
        public void Set_silent_text_without_adjustment_of_fontSize(string text)
        {
            FullSize_label.Text = text;
            FullSize_label.Font = new Font(FullSize_label.Font, Font_style);
        }
        public string Get_text()
        {
            return FullSize_label.Text;
        }
        public string Get_deep_copy_of_text()
        {
            return (string)FullSize_label.Text.Clone();
        }


    }
    class MyPanel_textBox : MyPanel
    {
        #region Fields
        public OwnTextBox FullSize_OwnTextBox { get; private set; }
        public FontStyle Font_style { get; set; }
        public FontFamily Font_family { get; set; }
        public int Initial_fontSize { get; set; }
        public Color Back_color
        {
            set
            {
                base.BackColor = value;
                base.ForeColor = value;
                FullSize_OwnTextBox.BackColor = value;
                FullSize_OwnTextBox.Refresh();
            }
            get { return FullSize_OwnTextBox.BackColor; }
        }
        public Color TextColor
        {
            set
            {
                FullSize_OwnTextBox.ForeColor = value;
                FullSize_OwnTextBox.Refresh();
            }
            get {  return FullSize_OwnTextBox.ForeColor; }
        }
        public override Color BackColor
        {
            set { this.Back_color = value; }
            get { return Back_color; }
        }
        public override Color ForeColor
        {
            set { this.TextColor = value; }
            get { return this.TextColor; }
        }
        #endregion



        public MyPanel_textBox()
        {
            base.Corner_radius = 99990;
            FullSize_OwnTextBox = new OwnTextBox();
            Controls.Add(FullSize_OwnTextBox);
            FullSize_OwnTextBox.Location = new Point(0, 0);
            FullSize_OwnTextBox.Height = Height;
            FullSize_OwnTextBox.Width = Width;
            FullSize_OwnTextBox.BorderStyle = BorderStyle.None;
            FullSize_OwnTextBox.BorderStyle_ownTextBox = BorderStyle.None;
            Font_style = FontStyle.Bold;
            Font_family = FontFamily.GenericSerif;
            Initial_fontSize = 10;
            FullSize_OwnTextBox.ReadOnly = true;
            FullSize_OwnTextBox.Multiline = true;
            FullSize_OwnTextBox.WordWrap = false;
        }
        public void Set_location_and_move_overall_left_border_to_left(int left_x_position, int top_y_position, int move_to_left)
        {
            this.Location = new Point(left_x_position - move_to_left, top_y_position);
            this.Width = this.Width + move_to_left;
            FullSize_OwnTextBox.Location = new Point(move_to_left, 0);
        }
        public void Set_left_top_right_bottom_position(int left_position, int top_position, int right_position, int bottom_position, Form1_default_settings_class default_settings)
        {
            Location = new Point(left_position, top_position);
            Height = bottom_position - top_position;
            Width = right_position - left_position;
            FullSize_OwnTextBox.Height = Height;
            FullSize_OwnTextBox.Width = Width;
            Adjust_fontSize_to_textBoxBorders(default_settings);
        }
        private void Adjust_fontSize_to_textBoxBorders(Form1_default_settings_class default_settings)
        {
            string text = FullSize_OwnTextBox.Text;
            Size textSize;
            FullSize_OwnTextBox.Font = new Font(FullSize_OwnTextBox.Font, Font_style);
            FullSize_OwnTextBox.Font = default_settings.Adjust_font_size_and_text_to_area_size(ref text, FullSize_OwnTextBox.Font, out textSize, Width, Height, default_settings.Max_fontSize_defaultBold, -1);
            FullSize_OwnTextBox.SilentText = (string)text.Clone();
        }
        private void Adjust_fontSize_to_textBoxBorders_with_fixed_number_of_lines(Form1_default_settings_class default_settings, int fixed_number_of_lines)
        {
            string text = FullSize_OwnTextBox.Text;
            Size textSize;
            FullSize_OwnTextBox.Font = new Font(FullSize_OwnTextBox.Font, Font_style);
            FullSize_OwnTextBox.Font = default_settings.Adjust_font_size_and_text_to_area_size(ref text, FullSize_OwnTextBox.Font, out textSize, Width, Height, default_settings.Max_fontSize_defaultBold, fixed_number_of_lines);
            FullSize_OwnTextBox.SilentText = (string)text.Clone();
        }

        public void Set_silent_text_adjustFontSize_and_refresh(string text, Form1_default_settings_class default_settings)
        {
            FullSize_OwnTextBox.SilentText_and_refresh = text;
            Adjust_fontSize_to_textBoxBorders(default_settings);
            FullSize_OwnTextBox.Refresh();
        }
        public void Set_silent_text_for_tutorial_with_given_max_char_each_line_use_crossComputer_fixed_textSize(string input_text, Form1_default_settings_class default_settings)
        {
            int char_count_each_line = default_settings.Tutorial_char_count_each_line;
            string[] splitStrings = input_text.Split(' ');
            bool round_to_ceiling = false;
            foreach (string splitString in splitStrings)
            {
                if (splitString.Length>char_count_each_line)
                {  
                    char_count_each_line = splitString.Length;
                    round_to_ceiling = true;
                }
            }
            int no_of_lines = -1;
            if (!round_to_ceiling) { no_of_lines = (int)Math.Round((float)input_text.Length / char_count_each_line); }
            else { no_of_lines = (int)Math.Ceiling((float)input_text.Length / char_count_each_line); }
            no_of_lines = Math.Min(no_of_lines, default_settings.Tutorial_max_lines);
            string split_text = default_settings.Split_text_equally_over_number_of_indicated_lines_under_consideration_of_mandatory_line_breaks(input_text, no_of_lines);
            FullSize_OwnTextBox.SilentText_and_refresh = (string)split_text.Clone();
            FullSize_OwnTextBox.Multiline = true;
            FullSize_OwnTextBox.Font = new Font(default_settings.Tutorial_fontFamilyName, default_settings.Tutorial_fontSize, default_settings.Tutorial_fontStyle, GraphicsUnit.Pixel);

            string[] lines = split_text.Split(new[] { "\r\n" }, StringSplitOptions.None);

            //int lineHeight = TextRenderer.MeasureText("A", FullSize_OwnTextBox.Font).Height;
            //int totalHeight = lineHeight * lines.Length;

            //using (Graphics g = FullSize_OwnTextBox.CreateGraphics())
            //{
            //    SizeF textSize = g.MeasureString(
            //        split_text,
            //        FullSize_OwnTextBox.Font,
            //        default_settings.Tutorial_char_count_each_line * 100, // rough max width in pixels
            //        StringFormat.GenericTypographic);

            //    int finalWidth = (int)Math.Ceiling(textSize.Width);
            //    int finalHeight = (int)Math.Ceiling(textSize.Height);

            //    FullSize_OwnTextBox.Size = new Size(finalWidth, finalHeight);
            //    this.Size = new Size(finalWidth, finalHeight);
            //}

            int maxWidth = 0;
            int lineHeight = TextRenderer.MeasureText("A", FullSize_OwnTextBox.Font).Height;
            int totalHeight = lineHeight * lines.Length;

            foreach (string line in lines)
            {
                Size lineSize = TextRenderer.MeasureText(line, FullSize_OwnTextBox.Font,
                                                         new Size(int.MaxValue, int.MaxValue),
                                                         TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
                if (lineSize.Width > maxWidth) { maxWidth = lineSize.Width; }
            }

            int textWidth = (int)Math.Round(maxWidth * 1.025F);
            if (default_settings.Is_mono)
            {
                Size = new Size(textWidth, totalHeight);
            }
            else
            {
                Size = new Size(textWidth, totalHeight);
            }
            FullSize_OwnTextBox.Size = new Size(textWidth, totalHeight);
            FullSize_OwnTextBox.SilentText_and_refresh = split_text;
            //Adjust_fontSize_to_textBoxBorders_with_fixed_number_of_lines(default_settings, lines.Length);
            this.Size = new Size(FullSize_OwnTextBox.Size.Width, FullSize_OwnTextBox.Size.Height);
            float fs = this.Font.Size;

            FullSize_OwnTextBox.Refresh();
        }
        public string Get_deep_copy_of_text()
        {
            return (string)FullSize_OwnTextBox.Text.Clone();
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
