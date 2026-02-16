using Common_functions.Form_tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_forms_customized_tools;

namespace Windows_forms
{
    //public class InputWaiter_ : Form
    //{
    //    public Key_mouse_input_waiter_()
    //    {
    //        this.StartPosition = FormStartPosition.CenterScreen;
    //        this.FormBorderStyle = FormBorderStyle.None;
    //        this.BackColor = Color.Black;
    //        this.Opacity = 0.01; // Almost invisible, but clickable
    //        this.ShowInTaskbar = false;
    //        this.TopMost = true;
    //        this.Width = Screen.PrimaryScreen.Bounds.Width;
    //        this.Height = Screen.PrimaryScreen.Bounds.Height;

    //        this.KeyPreview = true;
    //        this.KeyDown += (s, e) => this.Close();
    //        this.MouseDown += (s, e) => this.Close();
    //    }

    //    public static void WaitForKeyOrMouse()
    //    {
    //        using (InputWaiter_ waiter = new InputWaiter_())
    //        {
    //            waiter.ShowDialog();
    //        }
    //    }

    //    private void InitializeComponent()
    //    {
    //        this.SuspendLayout();
    //        // 
    //        // InputWaiter
    //        // 
    //        this.ClientSize = new System.Drawing.Size(278, 229);
    //        this.Name = "InputWaiter";
    //        this.ResumeLayout(false);

    //    }
    //}

    public static class Keyboard_class
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }
    }
    public class InputSuppressor_class : IMessageFilter
    {
        public delegate void InputDetectedHandler(bool keyPressed, bool mousePressed);
        public InputDetectedHandler OnInputDetected;
        public InputSuppressor_class()
        { }
        public bool PreFilterMessage(ref Message m)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_MBUTTONDOWN = 0x0207;

            bool isKey = (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN);
            bool isMouse = (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN);

            if ((isKey || isMouse) && OnInputDetected != null)
            {
                OnInputDetected(isKey, isMouse);
                return true; // Suppress input
            }

            return false; // Let other messages through
        }
    }

    public class LinuxInputFilter : IMessageFilter
    {
        private readonly Action<Keys> onKeyDown;
        private readonly Action onMouseDown;

        public LinuxInputFilter(Action<Keys> onKeyDown, Action onMouseDown)
        {
            this.onKeyDown = onKeyDown;
            this.onMouseDown = onMouseDown;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;

            if (m.Msg == WM_KEYDOWN)
            {
                Keys key = (Keys)(int)m.WParam;
                onKeyDown?.Invoke(key);
            }
            else if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN)
            {
                onMouseDown?.Invoke();
            }

            return false; // Allow normal processing
        }
    }

    public class Key_mouseButton_inputWaiter_class
    {
        public Key_mouseButton_inputWaiter_class()
        {
        }
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);


        public bool Wait_for_user_input_and_return_if_escape_was_pressed_linux(out bool escape_is_pressed_return, out bool back_arrow_pressed_return)
        {
            bool inputDetected = false;
            bool escape_pressed = false;
            bool back_arrow_pressed = false;

            Application.AddMessageFilter(new LinuxInputFilter(
                (key) =>
                {
                    inputDetected = true;
                    if (key == Keys.Escape) { escape_pressed = true; }
                    else if (key == Keys.Back || key == Keys.Left) { back_arrow_pressed = true; }
                },
                () =>
                {
                    inputDetected = true;
                }));

            while (!inputDetected)
            {
                Application.DoEvents();
                Thread.Sleep(30);
            }
            escape_is_pressed_return = escape_pressed;
            back_arrow_pressed_return = back_arrow_pressed;
            return true;
        }

        public bool Wait_for_user_input_and_return_if_escape_was_pressed_windows(out bool escape_is_pressed_return, out bool back_arrow_pressed_return)
        {
            escape_is_pressed_return = false;
            bool inputDetected = false;
            bool escapePressed = false;
            bool back_arrow_pressed = false;

            InputSuppressor_class filter = new InputSuppressor_class();
            filter.OnInputDetected = new InputSuppressor_class.InputDetectedHandler(delegate (bool keyPressed, bool mousePressed)
            {
                inputDetected = keyPressed || mousePressed;
                if (keyPressed)
                if (Keyboard_class.IsKeyDown(Keys.Escape))
                {
                    escapePressed = true;
                }
                else if (  (Keyboard_class.IsKeyDown(Keys.Back))
                         ||(Keyboard_class.IsKeyDown(Keys.Left)))
                    {
                        back_arrow_pressed = true;
                    }
            });

            Application.AddMessageFilter(filter);

            Thread.Sleep(200); // Prevent early triggers

            while (!inputDetected)
            {
                Application.DoEvents();
                Thread.Sleep(30);
            }

            Application.RemoveMessageFilter(filter);
            escape_is_pressed_return = escapePressed;
            back_arrow_pressed_return = back_arrow_pressed;
            return true;
        }
    }

    class Tutorial_interface_class
    {
        private MyPanel_textBox Tutorial_myPanelTextBox { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private ProgressReport_interface_class ProgressReport { get; set; }
        private Key_mouseButton_inputWaiter_class Key_mouseButton_inputWaiter { get; set; }
        private float Tutorial_fontSize { get; set; }

        public Tutorial_interface_class(MyPanel_textBox tutorial_myPanelTextBox,
                                        ProgressReport_interface_class progressReport,
                                        Key_mouseButton_inputWaiter_class key_mouseButton_inputWaiter,
                                        Form1_default_settings_class form_default_settings)
        {
            Tutorial_myPanelTextBox = tutorial_myPanelTextBox;
            ProgressReport = progressReport;
            Form_default_settings = form_default_settings;
            Tutorial_myPanelTextBox.Corner_radius = 0;
            Tutorial_myPanelTextBox.Border_color = Color.Black;
            Tutorial_fontSize = -1;
            Tutorial_myPanelTextBox.BorderStyle = BorderStyle.FixedSingle;
            Tutorial_myPanelTextBox.Back_color = Form_default_settings.Color_tutorial_text_backColor;
            Tutorial_myPanelTextBox.TextColor = Form_default_settings.Color_tutorial_text_foreColor;
            Tutorial_myPanelTextBox.Border_color = Form_default_settings.Color_tutorialPanel_borderColor;
            Key_mouseButton_inputWaiter = key_mouseButton_inputWaiter;
        }
        public void Identify_suited_tutorial_fontSize(int left_x_position, int top_y_position, int width, int height, bool visible)
        {
            this.Set_to_invisible();
            int right_x_position = left_x_position + width;
            int bottom_y_position = top_y_position + height;
            string text = "This is test text to identify a suited font size for the tutorial text boxes. It is shown because the application runs in developer mode. Press button to continue";


            Form_default_settings.Tutorial_fontSize = 10;
            Tutorial_myPanelTextBox.FullSize_OwnTextBox.Font = new Font(Form_default_settings.Tutorial_fontFamilyName, Form_default_settings.Tutorial_fontSize, Form_default_settings.Tutorial_fontStyle, GraphicsUnit.Pixel);
            Tutorial_myPanelTextBox.Set_left_top_right_bottom_position(left_x_position, top_y_position, right_x_position, bottom_y_position, Form_default_settings);
            Tutorial_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh(text, Form_default_settings);
            Tutorial_fontSize = Tutorial_myPanelTextBox.FullSize_OwnTextBox.Font.Size;
            Tutorial_myPanelTextBox.BringToFront();
            Tutorial_myPanelTextBox.Visible = visible;
            Tutorial_myPanelTextBox.Refresh();
        }
        public void Set_to_invisible_update_text_move_to_front_and_set_to_visible(string text, int x_position, int y_position, ContentAlignment postion_references)
        {
            Set_to_invisible();
            Form_default_settings.Tutorial_fontSize = Tutorial_fontSize;
            Tutorial_myPanelTextBox.Set_silent_text_for_tutorial_with_given_max_char_each_line_use_crossComputer_fixed_textSize(text, Form_default_settings);
            int left_x_position = -1;
            int top_y_position = -1;
            switch (postion_references)
            {
                case ContentAlignment.TopLeft:
                    left_x_position = x_position;
                    top_y_position = y_position;
                    break;
                case ContentAlignment.TopCenter:
                    left_x_position = x_position - (int)Math.Round(0.5F * Tutorial_myPanelTextBox.Width);
                    top_y_position = y_position;
                    break;
                case ContentAlignment.MiddleRight:
                    left_x_position = x_position - Tutorial_myPanelTextBox.Size.Width;
                    top_y_position = y_position - (int)Math.Round(0.5F * Tutorial_myPanelTextBox.Height);
                    break;
                case ContentAlignment.MiddleCenter:
                    left_x_position = x_position - (int)Math.Round(0.5F * Tutorial_myPanelTextBox.Size.Width);
                    top_y_position = y_position - (int)Math.Round(0.5F * Tutorial_myPanelTextBox.Height);
                    break;
                case ContentAlignment.BottomCenter:
                    left_x_position = x_position - (int)Math.Round(0.5F * Tutorial_myPanelTextBox.Size.Width);
                    top_y_position = y_position - Tutorial_myPanelTextBox.Height;
                    break;
                case ContentAlignment.BottomLeft:
                    left_x_position = x_position;
                    top_y_position = y_position - Tutorial_myPanelTextBox.Height;
                    break;
                default:
                    throw new Exception();
            }
            if (Form_default_settings.Is_mono)
            {
                int distance_from_left = (int)Math.Round(Form_default_settings.Get_resolution_parameter_x_factor() * 5);
                Tutorial_myPanelTextBox.Set_location_and_move_overall_left_border_to_left(left_x_position, top_y_position, distance_from_left);
            }
            else
            {
                Tutorial_myPanelTextBox.Location = new Point(left_x_position, top_y_position);
            }

            Tutorial_myPanelTextBox.Back_color = Form_default_settings.Color_tutorial_text_backColor;
            Tutorial_myPanelTextBox.TextColor = Form_default_settings.Color_tutorial_text_foreColor;
            Tutorial_myPanelTextBox.Border_color = Form_default_settings.Color_tutorialPanel_borderColor;
            Tutorial_myPanelTextBox.BringToFront();
            Tutorial_myPanelTextBox.Visible = true;
            Tutorial_myPanelTextBox.Refresh();
        }
        public void Set_to_invisible()
        {
            Tutorial_myPanelTextBox.Visible = false;
            Tutorial_myPanelTextBox.Refresh();
        }
        public void Wait_until_key_pressed_and_return_key_pressed_information(out bool escape_was_pressed, out bool back_arrow_was_pressed)
        {
            ProgressReport.Update_progressReport_text_and_visualization("Press key or mouse button to continue. Press <- to go back and ESC to abort.");
            if (Form_default_settings.Is_mono) { Key_mouseButton_inputWaiter.Wait_for_user_input_and_return_if_escape_was_pressed_linux(out escape_was_pressed, out back_arrow_was_pressed); }
            else { Key_mouseButton_inputWaiter.Wait_for_user_input_and_return_if_escape_was_pressed_windows(out escape_was_pressed, out back_arrow_was_pressed); }
        }
        public void Reset_after_tour_finished()
        {
            ProgressReport.Clear_progressReport_text_and_last_entry();
            Set_to_invisible();
        }



    }
}
