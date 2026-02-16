using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Windows_forms_customized_tools;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using Common_functions.Global_definitions;

namespace Common_functions.Form_tools
{
    class Form1_resolution_parameter_class
    {
        public float X_factor { get; set; }
        public float Y_factor { get; set; }
        public float Factor_for_max_fontSizes { get; set; }
        public float TextBox_heightFraction_for_fontSize { get; set; }
        public float ListBox_heightFraction_for_fontSize { get; set; }
        public float Button_heightFraction_for_fontSize { get; set; }
        public float Button_widthFraction_for_fontSize { get; set; }
        public int Panel_X_distance_from_referenceBorder { get; set; }
        public int Panel_Y_distance_from_referenceBorder { get; set; }
        public int Label_X_distance_from_referenceBorder { get; set; }
        public int Label_Y_distance_from_referenceBorder { get; set; }
        public int TextBox_X_distance_from_referenceBorder { get; set; }
        public int TextBox_Y_distance_from_referenceBorder { get; set; }
        public int ListBox_X_distance_from_referenceBorder { get; set; }
        public int ListBox_Y_distance_from_referenceBorder { get; set; }
        public int CheckBox_X_distance_from_referenceBorder { get; set; }
        public int CheckBox_Y_distance_from_referenceBorder { get; set; }
        public int CheckBoxButton_X_distance_from_referenceBorder { get; set; }
        public int CheckBoxButton_Y_distance_from_referenceBorder { get; set; }
        public int Button_X_distance_from_referenceBorder { get; set; }
        public int Button_Y_distance_from_referenceBorder { get; set; }
        public int ScrollBar_X_distance_from_referenceBorder { get; set; }
        public int ScrollBar_Y_distance_from_referenceBorder { get; set; }
        public int Max_lines_per_label { get; set; }
        public float Aspect_ratio_screen { get; set; }
        private int Reference_percent_height { get; set; }
        private int Reference_percent_width { get; set; }
        public int Reference_overall_panel_width { get; private set; }
        public int Reference_overall_panel_height { get; private set; }
        private Rectangle Resolution { get; set; }

        public Form1_resolution_parameter_class()
        {
            try { Resolution = Screen.PrimaryScreen.Bounds; }
            catch { Resolution = new Rectangle(20, 20, 3000, 1700); }
            Reference_overall_panel_width = 1270;
            Reference_overall_panel_height = 870;
            //Reference_overall_panel_width = 1920;
            //Reference_overall_panel_height = (int)Math.Round(0.95F * 1080);
            float anticipated_aspect_ratio = 1920F / 1080F;
            float observed_aspect_ratio = (float)Resolution.Width / (float)Resolution.Height;
            float ratio_of_aspectRatios = observed_aspect_ratio / anticipated_aspect_ratio;
            Reference_percent_width = (int)Math.Round(100F * (float)Reference_overall_panel_width / 1920F);
            Reference_percent_height = (int)Math.Round(100F * (float)Reference_overall_panel_height / (0.95F * 1080F));
            //To change the initial height and width, use function 'Get_initial_percentages_of_width_and_height'
        }

        public void Get_initial_percentages_of_width_and_height(out int percent_of_width, out int percent_of_height)
        {
            //percent_of_height = (int)Math.Round(95F * 1080 / Resolution.Height);
            //percent_of_width = (int)Math.Round(74F * 1920 / Resolution.Width);
            int max_percentage = 95;
            percent_of_height = 95;
            percent_of_width = (int)Math.Round(74F * (1920F/ 1080F) * ((float)Resolution.Height / (float)Resolution.Width));
            while (percent_of_height > max_percentage | percent_of_width > max_percentage)
            {
                if (percent_of_height > max_percentage)
                {
                    percent_of_width = (int)Math.Round((float)percent_of_width * (float)max_percentage / (float)percent_of_height);
                    percent_of_height = max_percentage;
                }
                if (percent_of_width > max_percentage)
                {
                    percent_of_height = (int)Math.Round((float)percent_of_height * (float)max_percentage / (float)percent_of_width);
                    percent_of_width = max_percentage;
                }
            }
        }

        public void Set_all_sizes(int percent_of_width, int percent_of_height)
        {
            X_factor = (float)percent_of_width / Reference_percent_width * (float)Resolution.Width / 1920F;
            Y_factor = (float)percent_of_height / Reference_percent_height * (float)Resolution.Height / 1080F;
            //X_factor = ((float)percent_of_width / 100F);// * (float)Resolution.Width / 1920F;
            //Y_factor = ((float)percent_of_height / 100F);// * (float)Resolution.Height / 1080;
            Aspect_ratio_screen = ((float)Resolution.Width*X_factor) / ((float)Resolution.Height*Y_factor);
            Factor_for_max_fontSizes = (float)Y_factor;
            TextBox_heightFraction_for_fontSize = 18F / 25F;
            ListBox_heightFraction_for_fontSize = 0.76F;
            Button_heightFraction_for_fontSize = 0.6F;
            Button_widthFraction_for_fontSize = 0.8F;
            Panel_X_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)4 * X_factor));
            Panel_Y_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)4 * Y_factor));
            Label_X_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)2 * X_factor));
            Label_Y_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)2 * Y_factor));
            TextBox_X_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)2 * X_factor));
            TextBox_Y_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)2 * Y_factor));
            ListBox_X_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)2 * X_factor));
            ListBox_Y_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)2 * Y_factor));
            CheckBox_X_distance_from_referenceBorder = TextBox_X_distance_from_referenceBorder;
            CheckBox_Y_distance_from_referenceBorder = TextBox_X_distance_from_referenceBorder;
            CheckBoxButton_X_distance_from_referenceBorder = TextBox_X_distance_from_referenceBorder;
            CheckBoxButton_Y_distance_from_referenceBorder = TextBox_Y_distance_from_referenceBorder;
            Button_X_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)0 * X_factor));
            Button_Y_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)0 * Y_factor));
            ScrollBar_X_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)0 * X_factor));
            ScrollBar_Y_distance_from_referenceBorder = Math.Max(0, (int)Math.Round((double)0 * Y_factor));
            Max_lines_per_label = 6;
        }
    }

    class Form1_default_settings_class
    {
        const string Color_theme_bright1 = "Bright I";
        const string Color_theme_bright2 = "Bright II";
        const string Color_theme_dark1 = "Dark I";
        const string Color_theme_dark2 = "Dark II";

        public int DatasetInterface_variable_panel_height { get; private set; }
        public int DatasetInterface_max_number_of_shown_entries { get; private set; }

        public Color Color_overall_background { get; private set; }
        public Color Color_label_backColor { get; private set; }
        public Color Color_label_foreColor { get; private set; }
        public Color Color_tutorial_text_backColor { get; private set; }
        public Color Color_tutorial_text_foreColor { get; private set; }
        public Color Color_tutorialPanel_borderColor { get; private set; }
        public Color Color_textBox_foreColor { get; private set; }
        public Color Color_textBox_backColor { get; private set; }
        public Color Color_textBox_backColor_invalid_value { get; private set; }
        public Color Color_panel_borderColor { get; private set; }
        public Color Color_panel_backColor { get; private set; }
        public Color Color_checkBox_foreColor { get; private set; }
        public Color Color_checkBox_backColor { get; private set; }
        public Color Color_listBox_foreColor { get; private set; }
        public Color Color_listBox_backColor { get; private set; }
        public Color ExplanationText_color { get; private set; }
        public Color Incompatible_dataset_backColor { get; private set; }
        public Color Incompatible_dataset_foreColor { get; private set; }
        public Color Color_warning { get; private set; }
        public Color Color_dataset_backColor_incompatibilityDelete { get; private set; }
        public Color Color_dataset_foreColor_incompatibilityDelete { get; private set; }
        public Color Color_dataset_foreColor { get; private set; }
        public Color Color_dataset_backColor { get; private set; }
        public Color Color_dataset_foreColor_markedForDeletion { get; private set; }
        public Color Color_dataset_backColor_markedForDeletion { get; private set; }
        public Color Warnings_back_color { get; private set; }
        public Color Warnings_fore_color { get; private set; }
        public Color Color_button_pressed_back { get; private set; }
        public Color Color_button_pressed_fore { get; private set; }
        public Color Color_button_notPressed_back { get; private set; }
        public Color Color_button_notPressed_fore { get; private set; }
        public Color Color_button_highlight_back { get; private set; }
        public Color Color_button_highlight_fore { get; private set; }
        public Color Color_secondaryButton_notPressed_back { get; private set; }
        public Color Color_secondaryButton_notPressed_fore { get; private set; }
        public Color Color_secondaryButton_pressed_back { get; private set; }
        public Color Color_secondaryButton_pressed_fore { get; private set; }
        public Color Color_checkBox_button_pressed_back { get; set; }
        public Color Color_checkBox_button_notPressed_back { get; set; }
        public Color Color_checkBox_button_pressed_fore { get; set; }
        public Color Color_checkBox_button_notPressed_fore { get; set; }
        public FontStyle ExplanationText_fontStyle { get; private set; }
        public string ExplanationText_font { get; private set; }
        public float Max_fontSize_checkBox { get; private set; }
        public float Max_fontSize_explanationText { get; private set; }
        public float Max_fontSize_headline { get; private set; }
        public float Max_fontSize_headlineUnderline { get; private set; }
        public float Max_fontSize_defaultBold { get; set; }
        public float Max_fontSize_defaultRegular { get; private set; }
        public float Max_fontSize_button { get; set; }
        public float Max_fontSize_button2nd { get; set; }
        public float Tutorial_fontSize { get; set; }
        public string Tutorial_fontFamilyName { get; set; }
        public int Tutorial_char_count_each_line { get; set; }
        public int Tutorial_max_lines { get; set; }
        public FontStyle Tutorial_fontStyle { get; set; }
        public string My_dataset { get; private set; }
        public float CheckBoxToButton_size_conversion_factor { get; set; }
        public int CheckBox_box_csharp_width { get; set; }
        public int CheckBox_box_csharp_height { get; set; }
        public int ListBox_height_of_one_line { get; set; }
        public int TextBox_height_of_one_line { get; set; }

        private MyPanel Overall_panel { get; set; }
        private Label Headline_label { get; set; }
        private Label Width_label { get; set; }
        private OwnTextBox Width_textBox { get; set; }
        private Label WidthPercent_label { get; set; }
        private Label Height_label { get; set; }
        private OwnTextBox Height_textBox { get; set; }
        private Label HeightPercent_label { get; set; }
        private Label ColorTheme_label { get; set; }
        private OwnListBox ColorTheme_listBox { get; set; }
        private Button Up_button { get; set; }
        private Button Down_button { get; set; }
        private Button Resize_button { get; set; }
        public Form1_resolution_parameter_class Resolution_parameter { get; set; }
        private int Current_height_min_percentage { get; set; }
        private int Current_height_max_percentage { get; set; }
        public int Current_height_percentage { get; private set; }
        private int Current_width_min_percentage { get; set; }
        private int Current_width_max_percentage { get; set; }
        public int Current_width_percentage { get; private set; }
        private int Fixed_add_remove_width { get; set; }
        private int Fixed_add_remove_height { get; set; }
        private string Current_color_theme { get; set; }
        public float Correction_factor_for_application_height { get; set; }
        public bool Is_mono { get; private set; }
        private bool Correct_mono_label_sizes { get; set; }
        public int Distance_of_right_x_of_tutorial_panel_from_menue_panel { get; set; }
        public string Explanation_text_major_separator { get; set; }
        public Form1_default_settings_class()
        {
            Resolution_parameter = new Form1_resolution_parameter_class();
            Max_fontSize_defaultBold = 100;

            Explanation_text_major_separator =
                  "\r\n" +
                  "\r\n---------------------------------------------------------------------------------------" +
                  "\r\n" +
                  "\r\n";

        }
        public void Update_current_height_and_width_min_and_max_percentages(int failed_heigth_percentage, int failed_width_percentage)
        {
            int rounding_factor = 5;
            if (Current_height_percentage < failed_heigth_percentage)
            {
                Current_height_max_percentage = (int)Math.Floor(1F/(float)rounding_factor * (failed_heigth_percentage + 1)) * rounding_factor;
            }
            else if (Current_height_percentage > failed_heigth_percentage)
            {
                Current_height_min_percentage = (int)Math.Ceiling(1F / (float)rounding_factor * (failed_heigth_percentage + 1)) * rounding_factor;
            }
            if (Current_width_percentage < failed_width_percentage)
            {
                Current_width_max_percentage = (int)Math.Floor(1F / (float)rounding_factor * (failed_width_percentage + 1)) * rounding_factor;
            }
            else if (Current_width_percentage > failed_width_percentage)
            {
                Current_width_min_percentage = (int)Math.Ceiling(1F / (float)rounding_factor * (failed_width_percentage + 1)) * rounding_factor;
            }
        }
        public void Higlight_button(Button button)
        {
            Color back_color = button.BackColor;
            Color fore_color = button.ForeColor;
            button.BackColor = Color_button_highlight_back;
            button.ForeColor = Color_button_highlight_fore;
            button.Refresh();
        }

        public bool Is_button_pressed(Color button_back_color)
        {
            bool botton_is_pressed = button_back_color.Equals(Color_button_pressed_back) | button_back_color.Equals(Color_secondaryButton_pressed_back);
            if (Global_class.Do_internal_checks)
            {
                if (Color_button_pressed_back.Equals(Color_button_notPressed_back)) { throw new Exception(); }
                //if (Color_button_pressed_back.Equals(Color_secondaryButton_pressed_back)) { throw new Exception(); }
                if (Color_secondaryButton_notPressed_back.Equals(Color_secondaryButton_pressed_back)) { throw new Exception(); }
            }
            return botton_is_pressed;
        }


        public Form1_default_settings_class(MyPanel overall_panel,
                                            Label headline_label,
                                            Label width_label,
                                            OwnTextBox width_textBox,
                                            Label widthPercent_label,
                                            Label height_label,
                                            OwnTextBox height_textBox,
                                            Label heightPercent_label,
                                            Button up_button,
                                            Button down_button,
                                            Label colorTheme_label,
                                            OwnListBox colorTheme_listBox,
                                            Button resizeButton
                                            ) : this()
        {
            OwnCheckBox checBox = new OwnCheckBox();
            Size size = checBox.Size;

            Resolution_parameter = new Form1_resolution_parameter_class();
            this.Overall_panel = overall_panel;

            Is_mono = false;
            if (Type.GetType("Mono.Runtime")!=null)
            {
                Is_mono = true;
            }
            Correct_mono_label_sizes = false;

            this.Headline_label = headline_label;
            this.Width_label = width_label;
            this.Width_textBox = width_textBox;
            this.WidthPercent_label = widthPercent_label;
            this.Height_label = height_label;
            this.Height_textBox = height_textBox;
            this.HeightPercent_label = heightPercent_label;
            this.Up_button = up_button;
            this.Down_button = down_button;
            this.ColorTheme_label = colorTheme_label;
            this.ColorTheme_listBox = colorTheme_listBox;
            this.Resize_button = resizeButton;

            this.Current_color_theme = (string)Color_theme_bright1.Clone();

            int initial_percentage_of_height;
            int initial_percentage_of_width;
            Resolution_parameter.Get_initial_percentages_of_width_and_height(out initial_percentage_of_width, out initial_percentage_of_height);

            this.Current_height_percentage = initial_percentage_of_height;
            this.Current_width_percentage = initial_percentage_of_width;
            this.Fixed_add_remove_height = (int)Math.Round(0.1F * (float)initial_percentage_of_height);
            this.Fixed_add_remove_width = (int)Math.Round(0.1F * (float)initial_percentage_of_width);
            this.Current_height_min_percentage = 25;
            this.Current_height_max_percentage = 300;
            this.Current_width_min_percentage = (int)Math.Ceiling((float)this.Current_height_min_percentage * (float)initial_percentage_of_width / (float)initial_percentage_of_height);
            this.Current_width_max_percentage = (int)Math.Ceiling((float)this.Current_height_max_percentage * (float)initial_percentage_of_width / (float)initial_percentage_of_height);

            Correction_factor_for_application_height = 1F;

            Fill_color_theme_listBox_and_width_height_textBoxes();
            Update_parameter();
            Update_applicationSize_textBoxes_and_listBoxes();

            Get_overall_panel_width_and_height(out int overall_width, out int overall_height);
            Distance_of_right_x_of_tutorial_panel_from_menue_panel = (int)Math.Round(0.02F * overall_width);

        }

        private void Fill_color_theme_listBox_and_width_height_textBoxes()
        {
            this.ColorTheme_listBox.Items.Clear();
            this.ColorTheme_listBox.Items.Add(Color_theme_bright1);
            this.ColorTheme_listBox.Items.Add(Color_theme_dark1);
            this.ColorTheme_listBox.Items.Add(Color_theme_dark2);
            this.ColorTheme_listBox.Visible = !Is_mono;
            this.ColorTheme_label.Visible = !Is_mono;
            this.ColorTheme_listBox.SilentSelectedIndex = this.ColorTheme_listBox.Items.IndexOf(Current_color_theme);
            this.Width_textBox.SilentText = this.Current_width_percentage.ToString();
            this.Height_textBox.SilentText = this.Current_height_percentage.ToString();
        }

        public void Get_selected_heigth_and_width_percentage_from_textBoxes(out int selected_percentage_of_height, out int selected_percentage_of_width)
        {
            int percentage_of_height;
            if (   (int.TryParse(Height_textBox.Text, out percentage_of_height))
                && (percentage_of_height >= Current_height_min_percentage)
                && (percentage_of_height <= Current_height_max_percentage))
            {
            }
            else
            {
                percentage_of_height = -1;
            }
            int percentage_of_width;
            if ((int.TryParse(Width_textBox.Text, out percentage_of_width))
                && (percentage_of_width >= Current_width_min_percentage)
                && (percentage_of_width <= Current_width_max_percentage))
            {
            }
            else
            {
                percentage_of_width = -1;
            }
            selected_percentage_of_height = percentage_of_height;
            selected_percentage_of_width = percentage_of_width;

        }
        public void Update_parameter()
        {
            Get_selected_heigth_and_width_percentage_from_textBoxes(out int percentage_of_height, out int percentage_of_width);
            if (percentage_of_height>0)
            {
                this.Current_height_percentage = percentage_of_height;
            }
            if (percentage_of_width>0)
            {
                this.Current_width_percentage = percentage_of_width;
            }
            Current_color_theme = this.ColorTheme_listBox.SelectedItem.ToString();
            switch (Current_color_theme)
            {
                case Color_theme_bright1:
                    Set_colors_bright();
                    break;
                case Color_theme_dark1:
                    Set_colors_dark();
                    break;
                case Color_theme_dark2:
                    Set_colors_dark2();
                    break;
                default:
                    throw new Exception();
            }


            Resolution_parameter.Set_all_sizes(Current_width_percentage,Current_height_percentage);

            MyPanel test_panel = new MyPanel();
            MyPanelOverallDatsetInterface_add_default_parameters(test_panel);


            TextBox_height_of_one_line = (int)Math.Round(0.0505F* (float)test_panel.Height);
            ListBox_height_of_one_line = (int)Math.Round(18F * (float)TextBox_height_of_one_line/24F);
            DatasetInterface_variable_panel_height = (int)Math.Round(370.0F * Resolution_parameter.Y_factor);
            DatasetInterface_max_number_of_shown_entries = 13;
            ExplanationText_fontStyle = FontStyle.Italic;
            ExplanationText_font = "Arial";
            Max_fontSize_checkBox = 20 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_explanationText = 18 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_headline = 99 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_headlineUnderline = 99 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_defaultBold = 20 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_defaultRegular = 20 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_button = 99 * Resolution_parameter.Factor_for_max_fontSizes;
            Max_fontSize_button2nd = 99 * Resolution_parameter.Factor_for_max_fontSizes;
            
            if (Is_mono)
            {
                Tutorial_fontSize = 15 * Resolution_parameter.Factor_for_max_fontSizes;
            }
            else
            {
                Tutorial_fontSize = 10 * Resolution_parameter.Factor_for_max_fontSizes;
            }
            Tutorial_fontFamilyName = "Arial";
            Tutorial_fontStyle = FontStyle.Regular;
            Tutorial_char_count_each_line = 20;
            Tutorial_max_lines = 7;

            CheckBoxToButton_size_conversion_factor = 1.2F;
            My_dataset = "My dataset";
            CheckBox_box_csharp_width = 18;
            CheckBox_box_csharp_height = 17;

        }
        public void Increase_heightWidth_by_fixed_number()
        {
            int textBox_height;
            int textBox_width;
            if (  (int.TryParse(this.Width_textBox.Text, out textBox_width))
                && (int.TryParse(this.Height_textBox.Text, out textBox_height)))
            {
                textBox_height += this.Fixed_add_remove_height;
                textBox_width += this.Fixed_add_remove_width;
                this.Width_textBox.SilentText = textBox_width.ToString();
                this.Height_textBox.SilentText = textBox_height.ToString();
                Update_applicationSize_textBoxes_and_listBoxes();
            }
        }
        public void Decrease_heightWidth_by_fixed_number()
        {
            int textBox_height;
            int textBox_width;
            if ((int.TryParse(this.Width_textBox.Text, out textBox_width))
                && (int.TryParse(this.Height_textBox.Text, out textBox_height)))
            {
                textBox_height -= this.Fixed_add_remove_height;
                textBox_width -= this.Fixed_add_remove_width;
                this.Width_textBox.SilentText = textBox_width.ToString();
                this.Height_textBox.SilentText = textBox_height.ToString();
                Update_applicationSize_textBoxes_and_listBoxes();
            }
        }
        public void Update_applicationSize_textBoxes_and_listBoxes()
        {
            int suggested_width = -1;
            int suggested_height = -1;
            int.TryParse(this.Width_textBox.Text, out suggested_width);
            int.TryParse(this.Height_textBox.Text, out suggested_height);
            string suggested_colorTheme = this.ColorTheme_listBox.SelectedItem.ToString();
            bool different_values = false;
            if ((suggested_width < Current_width_min_percentage)
                || (suggested_width > Current_width_max_percentage))
            {
                this.Width_textBox.BackColor = this.Color_textBox_backColor_invalid_value;
            }
            else
            {
                this.Width_textBox.BackColor = this.Color_textBox_backColor;
                if (suggested_width!=Current_width_percentage) { different_values = true; }
            }
            if ((suggested_height < Current_height_min_percentage)
                || (suggested_height > Current_height_max_percentage))
            {
                this.Height_textBox.BackColor = this.Color_textBox_backColor_invalid_value;
            }
            else
            {
                this.Height_textBox.BackColor = this.Color_textBox_backColor;
                if (suggested_height != Current_height_percentage) { different_values = true; }
            }
            if (!suggested_colorTheme.Equals(this.Current_color_theme))
            {
                different_values = true;
            }
            if (different_values) { Resize_button.Visible = true; }
            else { Resize_button.Visible = false; }
        }

        public void Update_all_graphic_elements_in_applicationSize_panel()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            OwnTextBox my_textBox;
            Label my_label;
            Button my_button;
            OwnListBox my_listBox;

            this.Overall_panel = this.MyPanelOverallApplicationSize_add_default_parameters(this.Overall_panel);

            int mono_distance_between_buttons = (int)Math.Round(0.005F * Overall_panel.Width);
            top_referenceBorder = (int)Math.Round(0.4F * Overall_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.95F * Overall_panel.Height);
            int shared_textBoxlabel_width = (int)Math.Round(0.1F * Overall_panel.Width);
            int shared_textBoxLabelPercent_width = (int)Math.Round(0.04F * Overall_panel.Width);
            int textBox_width = (int)Math.Round(0.06F * Overall_panel.Width);
            int listBox_width = (int)Math.Round(0.14F * Overall_panel.Width);
            int button_width = (int)Math.Round(0.14F * Overall_panel.Width);
            int listBoxlabel_width = (int)Math.Round(0.18F * Overall_panel.Width);
            int sharedButton_width = (int)Math.Round(0.07F * Overall_panel.Width);
            right_referenceBorder = 0;

            left_referenceBorder = right_referenceBorder + shared_textBoxlabel_width;
            right_referenceBorder = left_referenceBorder + textBox_width;
            my_textBox = this.Width_textBox;
            my_textBox.TextAlign = HorizontalAlignment.Center;
            this.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder + shared_textBoxlabel_width + shared_textBoxLabelPercent_width;
            right_referenceBorder = left_referenceBorder + textBox_width;
            my_textBox = this.Height_textBox;
            my_textBox.TextAlign = HorizontalAlignment.Center;
            this.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Height_textBox.Location.X + this.Height_textBox.Width + shared_textBoxLabelPercent_width;
            right_referenceBorder = left_referenceBorder + sharedButton_width;
            my_button = this.Up_button;
            Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder;
            if (Is_mono)
            {
                left_referenceBorder += mono_distance_between_buttons;
            }
            right_referenceBorder = left_referenceBorder + sharedButton_width;
            my_button = this.Down_button;
            Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);


            bool allow_color_change = !Is_mono;
            if (allow_color_change)
            {
                left_referenceBorder = right_referenceBorder + listBoxlabel_width;
                right_referenceBorder = left_referenceBorder + listBox_width;
                my_listBox = this.ColorTheme_listBox;
                MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                left_referenceBorder = right_referenceBorder;
                if (Is_mono)
                {
                    left_referenceBorder += mono_distance_between_buttons;
                }
                right_referenceBorder = left_referenceBorder + button_width;
                my_button = this.Resize_button;
                Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
            if (!allow_color_change)
            {
                left_referenceBorder = right_referenceBorder;
                if (Is_mono)
                {
                    left_referenceBorder += mono_distance_between_buttons;
                }
                right_referenceBorder = left_referenceBorder + button_width;
                my_button = this.Resize_button;
                Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            left_referenceBorder = 0;
            right_referenceBorder = this.Width_textBox.Location.X;
            my_label = Width_label;
            LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Width_textBox.Location.X + this.Width_textBox.Width + shared_textBoxLabelPercent_width;
            right_referenceBorder = this.Height_textBox.Location.X;
            my_label = Height_label;
            LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Width_textBox.Location.X + this.Width_textBox.Width;
            right_referenceBorder = left_referenceBorder + shared_textBoxLabelPercent_width;
            my_label = WidthPercent_label;
            LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Height_textBox.Location.X + this.Height_textBox.Width;
            right_referenceBorder = left_referenceBorder + shared_textBoxLabelPercent_width;
            my_label = HeightPercent_label;
            LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Down_button.Location.X + this.Down_button.Width;
            right_referenceBorder = this.ColorTheme_listBox.Location.X;
            my_label = ColorTheme_label;
            LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = Width_textBox.Location.Y;
            my_label = Headline_label;
            LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
        }

        public int Adjust_locationX_to_current_resoltion(int location_X)
        {
            return (int)Math.Round(Resolution_parameter.X_factor * location_X);
        }

        public void Get_overall_panel_width_and_height(out int width, out int height)
        {
            width = (int)Math.Round(Resolution_parameter.X_factor * Resolution_parameter.Reference_overall_panel_width);
            height = (int)Math.Round(Resolution_parameter.Y_factor * Resolution_parameter.Reference_overall_panel_height);
        }

        private void Set_colors_bright()
        {
            Color_overall_background = Color.FromArgb(160, 160, 255);
            Color_panel_backColor = Color_overall_background;
            Color_panel_borderColor = Color.Black;

            Color_label_backColor = Color_panel_backColor;
            Color_label_foreColor = Color.Black;
            Color_textBox_foreColor = Color.Black;
            Color_textBox_backColor = Color.White;
            Color_textBox_backColor_invalid_value = Color.DarkOrchid;

            Color_tutorial_text_backColor = Color_textBox_backColor;
            Color_tutorial_text_foreColor = Color_textBox_foreColor;
            Color_tutorialPanel_borderColor = Color_panel_borderColor;

            Color_tutorial_text_backColor = Color.White;
            Color_tutorial_text_foreColor = Color.Black;
            Color_tutorialPanel_borderColor = Color_panel_borderColor;


            Color_listBox_backColor = Color_textBox_backColor;
            Color_listBox_foreColor = Color_textBox_foreColor;
            Color_checkBox_backColor = Color_label_backColor;
            Color_checkBox_foreColor = Color_label_foreColor;

            if (!Is_mono)
            {
                Color_button_notPressed_back = Color.MidnightBlue;
                Color_button_notPressed_fore = Color.LightGray;
                Color_button_pressed_back = Color.DodgerBlue;
                Color_button_pressed_fore = Color.Black;
            }
            else
            {
                //Color_button_notPressed_back = Color.FromArgb(25, 25, 170);
                Color_button_notPressed_back = Color.MidnightBlue;
                Color_button_notPressed_fore = Color.LightGray;
                Color_button_pressed_back = Color.DodgerBlue;
                Color_button_pressed_fore = Color.Black;
            }

            Color_button_highlight_fore = Color.Black;
            Color_button_highlight_back = Color.LightSalmon;

            if (!Is_mono)
            {
                Color_secondaryButton_notPressed_back = Color.MidnightBlue;
                Color_secondaryButton_notPressed_fore = Color.LightGray;
                Color_secondaryButton_pressed_back = Color.Black;
                Color_secondaryButton_pressed_fore = Color.LightGray;
            }
            else
            {
                Color_secondaryButton_notPressed_back = Color.MidnightBlue;
                //Color_secondaryButton_notPressed_back = Color.FromArgb(25, 25, 170);
                Color_secondaryButton_notPressed_fore = Color.LightGray;
                Color_secondaryButton_pressed_back = Color.DodgerBlue;
                Color_secondaryButton_pressed_fore = Color.LightGray;
            }

            if (!Is_mono)
            {
                Color_checkBox_button_pressed_back = Color.Black;
                Color_checkBox_button_notPressed_back = this.Color_overall_background;
                Color_checkBox_button_pressed_fore = Color_checkBox_button_pressed_back;
                Color_checkBox_button_notPressed_fore = Color_checkBox_button_notPressed_back;
            }
            else
            {
                //Color_checkBox_button_pressed_back = Color.FromArgb(20, 90, 160);
                Color_checkBox_button_pressed_back = Color.MidnightBlue;
                Color_checkBox_button_notPressed_back = this.Color_overall_background;
                Color_checkBox_button_pressed_fore = Color_checkBox_button_pressed_back;
                Color_checkBox_button_notPressed_fore = Color_checkBox_button_notPressed_back;
            }

            ExplanationText_color = Color.Black;
            Incompatible_dataset_backColor = Color.MediumPurple;
            Incompatible_dataset_foreColor = Color.Black;
            Color_warning = Color.LightSalmon;
            Color_dataset_backColor_markedForDeletion = Color.LightSalmon;
            Color_dataset_foreColor_markedForDeletion = Color.Black;
            Color_dataset_backColor_incompatibilityDelete = Color.Transparent;
            Color_dataset_foreColor_incompatibilityDelete = Color_label_foreColor;

            Color_dataset_backColor = Color_textBox_backColor;
            Color_dataset_foreColor = Color_textBox_foreColor;
            Warnings_back_color = Color_dataset_backColor_markedForDeletion;
            Warnings_fore_color = Color_dataset_foreColor_markedForDeletion;
        }

        private void Set_colors_dark()
        {
            Color_overall_background = Color.Black;
            Color_panel_backColor = Color_overall_background;
            Color_panel_borderColor = Color.LightSlateGray;

            Color_label_backColor = Color_panel_backColor;
            Color_label_foreColor = Color.LightSlateGray;
            Color_textBox_foreColor = Color.LightSlateGray;
            Color_textBox_backColor = Color.DarkBlue;
            Color_textBox_backColor_invalid_value = Color.Indigo;

            Color_tutorial_text_backColor = Color_textBox_backColor;
            Color_tutorial_text_foreColor = Color_textBox_foreColor;
            Color_tutorialPanel_borderColor = Color_panel_borderColor;

            Color_listBox_backColor = Color_textBox_backColor;
            Color_listBox_foreColor = Color_textBox_foreColor;
            Color_checkBox_backColor = Color_label_backColor;
            Color_checkBox_foreColor = Color_label_foreColor;

            Color_button_notPressed_back = Color.MidnightBlue;
            Color_button_notPressed_fore = Color.Gray;
            Color_button_pressed_back = Color.RoyalBlue;
            Color_button_pressed_fore = Color.Black;

            Color_button_highlight_fore = Color.Gray;
            Color_button_highlight_back = Color.DarkRed;

            Color_secondaryButton_notPressed_back = Color.SteelBlue;
            Color_secondaryButton_notPressed_fore = Color.Black;
            Color_secondaryButton_pressed_back = Color.RoyalBlue;
            Color_secondaryButton_pressed_fore = Color.Black;

            Color_checkBox_button_pressed_back = Color.LightGray;
            Color_checkBox_button_notPressed_back = this.Color_overall_background;
            Color_checkBox_button_pressed_fore = Color_checkBox_button_pressed_back;
            Color_checkBox_button_notPressed_fore = Color_checkBox_button_notPressed_back;

            ExplanationText_color = Color.LightSlateGray;
            Incompatible_dataset_backColor = Color.BlueViolet;
            Incompatible_dataset_foreColor = Color.Black;
            Color_warning = Color.DarkRed;
            Color_dataset_backColor_markedForDeletion = Color.IndianRed;
            Color_dataset_foreColor_markedForDeletion = Color.Black;
            Color_dataset_backColor_incompatibilityDelete = Color.Transparent;
            Color_dataset_foreColor_incompatibilityDelete = Color_label_foreColor;
            Color_dataset_backColor = Color_textBox_backColor;
            Color_dataset_foreColor = Color_textBox_foreColor;
            Warnings_back_color = Color_dataset_backColor_markedForDeletion;
            Warnings_fore_color = Color_dataset_foreColor_markedForDeletion;
        }

        private void Set_colors_dark2()
        {
            Set_colors_dark();
            Color_textBox_foreColor = Color.LightGray;
            Color_listBox_foreColor = Color_textBox_foreColor;
            Color_dataset_foreColor = Color_textBox_foreColor;
            Color_tutorial_text_backColor = Color_textBox_backColor;
            Color_tutorial_text_foreColor = Color_textBox_foreColor;

            //Color_label_foreColor = Color.LightGray;
        }
        private string Split_text_equally_over_number_of_indicated_lines(string text, int no_of_lines)
        {
            string[] splitStrings = text.Split(' ');
            string splitString;
            int splitStrings_length = splitStrings.Length;
            if (splitStrings_length == 1) { }
            else
            {
                StringBuilder sb = new StringBuilder();
                int text_length = text.Length;
                int real_number_of_lines = 99999;
                double factor_char_per_line = 1;
                while (real_number_of_lines > no_of_lines)
                {
                    real_number_of_lines = 0;
                    int char_per_line = (int)Math.Round((double)factor_char_per_line * (double)text_length / (double)no_of_lines);
                    sb.Clear();
                    sb.AppendFormat(splitStrings[0]);
                    real_number_of_lines++;
                    int current_line_character = sb.Length;
                    int splitString_nchar;
                    for (int indexSplit = 1; indexSplit < splitStrings_length; indexSplit++)
                    {
                        splitString = splitStrings[indexSplit];
                        splitString_nchar = splitString.Length;
                        if (current_line_character + splitString_nchar + 1 <= char_per_line)
                        {
                            sb.AppendFormat(" {0}", splitString);
                            current_line_character += splitString_nchar + 1;
                        }
                        else
                        {
                            sb.AppendFormat("\r\n{0}", splitString);
                            current_line_character = splitString_nchar;
                            real_number_of_lines++;
                        }
                    }
                    factor_char_per_line += 0.05;
                }
                text = sb.ToString();
            }
            return text;
        }
        public string Split_text_equally_over_number_of_indicated_lines_under_consideration_of_mandatory_line_breaks(string text, int noOfLines)
        {
            string lineBreakString = "\r\n";
            string[] mandatorySections = Regex.Split(text, Regex.Escape(lineBreakString), RegexOptions.IgnoreCase);
            string mandatorySection;
            int mandatorySections_length = mandatorySections.Length;
            int total_n_char = 0;
            int[] noOfLinesForEachSection = new int[mandatorySections_length];
            for (int indexM=0; indexM<mandatorySections_length; indexM++)
            {
                mandatorySection = mandatorySections[indexM];
                total_n_char += mandatorySection.Length;
            }
            int remaining_lines = noOfLines - mandatorySections_length;
            int assignedLines = 0;

            // Calculate lines for each section
            for (int indexM = 0; indexM < mandatorySections_length; indexM++)
            {
                double proportion = (double)mandatorySections[indexM].Length / total_n_char;
                noOfLinesForEachSection[indexM] = (int)Math.Round(proportion * remaining_lines);
                assignedLines += noOfLinesForEachSection[indexM];
            }

            // Adjust the distribution in case of rounding errors
            int lineAdjustment = remaining_lines - assignedLines;
            for (int i = 0; i < Math.Abs(lineAdjustment) && i < mandatorySections.Length; i++)
            {
                noOfLinesForEachSection[i] += lineAdjustment > 0 ? 1 : -1;
            }
            for (int indexM=0; indexM<mandatorySections_length;indexM++)
            {
                noOfLinesForEachSection[indexM] += 1;
            }
            StringBuilder result = new StringBuilder();
            string add_text;
            for (int indexMS=0; indexMS< mandatorySections_length;indexMS++)
            {
                mandatorySection = mandatorySections[indexMS];
                if (indexMS>0)
                {
                    result.Append(lineBreakString);
                }
                if (!string.IsNullOrEmpty(mandatorySection))
                {
                    add_text = Split_text_equally_over_number_of_indicated_lines(mandatorySection, noOfLinesForEachSection[indexMS]);
                    result.Append(@add_text);
                }
            }
            return result.ToString().TrimEnd();
        }

        private Font Adjust_font_size_to_height_of_one_line(Font myFont, int height_of_one_line)
        {
            float fontEmSize = myFont.FontFamily.GetEmHeight(myFont.Style);
            float fontLineSpacing = myFont.FontFamily.GetLineSpacing(myFont.Style);
            float emSize = Math.Max(0.05F,(height_of_one_line) * fontEmSize / fontLineSpacing);
            return new Font(myFont.FontFamily, emSize, myFont.Style, GraphicsUnit.Pixel);

        }
        private Font Adjust_font_size_to_area_size(string text, Font font, out Size text_size, int width, int height)
        {
            int text_lines_count = Regex.Split(text, Regex.Escape("\r\n"), RegexOptions.IgnoreCase).Count();
            int height_of_one_line = Math.Max(1,(int)Math.Floor((float)height / (float)text_lines_count));
            font = Adjust_font_size_to_height_of_one_line(font, height_of_one_line);

            bool text_fits = false;
            int new_fontSize;
            text_size = TextRenderer.MeasureText(text, font);
            float original_fontSize = font.Size;
            while (!text_fits)
            {
                if ((text_size.Width > width)&&(font.Size!=1))
                {
                    new_fontSize = (int)Math.Floor(font.Size * (float)width / (float)text_size.Width);
                    if (new_fontSize<1) { new_fontSize = 1; }
                    font = new Font(font.FontFamily, new_fontSize, font.Style, GraphicsUnit.Pixel);
                    text_size = TextRenderer.MeasureText(text, font);
                }
                else { text_fits = true; }
            }
            return font;
        }
        public Font Adjust_font_size_and_text_to_area_size(ref string text, Font font, out Size text_size, int width, int height, float max_fontSize, int fixed_number_of_lines)
        {
            text_size = new Size(0, 0);
            if (text.Length > 0)
            {
                int minimum_lines = Regex.Split(text, Regex.Escape("\r\n"), RegexOptions.IgnoreCase).Count();
                int maximum_lines = Math.Max(minimum_lines,text.Split(' ').Length);
                if (minimum_lines == 1)
                {
                    maximum_lines = Math.Max(maximum_lines, Resolution_parameter.Max_lines_per_label);
                }
                else
                {
                    maximum_lines = (int)Math.Max(minimum_lines*3,Math.Ceiling(maximum_lines * 0.2));
                }
                if (fixed_number_of_lines>0)
                {
                    minimum_lines = fixed_number_of_lines;
                    maximum_lines = fixed_number_of_lines;
                }

                string suggested_text;
                float largest_font_size = -1;
                string suggested_text_with_largest_font_size = "";
                int width_of_max_size_area = -1;
                int height_of_max_size_area = -1;
                Font potential_font;
                for (int no_of_lines = minimum_lines; no_of_lines <= maximum_lines; no_of_lines++)
                {
                    suggested_text = Split_text_equally_over_number_of_indicated_lines_under_consideration_of_mandatory_line_breaks(text, no_of_lines);
                    potential_font = (Font)font.Clone();
                   // potential_size = (Size)text_size.Clone();
                    potential_font = Adjust_font_size_to_area_size(suggested_text, potential_font, out text_size, width, height);
                    if (potential_font.Size > max_fontSize)
                    {
                        potential_font = new Font(potential_font.FontFamily, max_fontSize, potential_font.Style, GraphicsUnit.Pixel);
                    }
                    if (  (largest_font_size<=0)
                        ||(potential_font.Size > largest_font_size))
                    {
                        if (potential_font.Size < max_fontSize) { largest_font_size = potential_font.Size; }
                        else { largest_font_size = max_fontSize; }
                        height_of_max_size_area = text_size.Height;
                        width_of_max_size_area = text_size.Width;
                        suggested_text_with_largest_font_size = (string)suggested_text.Clone();
                    }
                }
                text = (string)suggested_text_with_largest_font_size.Clone();
                font = new Font(font.FontFamily, largest_font_size, font.Style, GraphicsUnit.Pixel);
                text_size = new Size(width_of_max_size_area, height_of_max_size_area);// TextRenderer.MeasureText(text, font); 
            }
            return font;
        }

        #region Buttons

        private Button Adjust_font_size_to_button_size(Button button)
        {
            button.TextAlign = ContentAlignment.MiddleCenter;
            int button_text_area_width = (int)Math.Round(button.Size.Width * Resolution_parameter.Button_widthFraction_for_fontSize);
            int button_text_area_height = (int)Math.Round(button.Size.Height * Resolution_parameter.Button_heightFraction_for_fontSize);
            Size size;
            string text = (string)button.Text.Clone();
            float max_fontSize = Max_fontSize_button;
            button.Font = Adjust_font_size_and_text_to_area_size(ref text, button.Font, out size, button_text_area_width, button_text_area_height, max_fontSize, -1);
            button.Text = (string)text.Clone();
            return button;
        }
        private void Correct_positions_and_sizes_of_standard_button_for_mono(ref int left_position, ref int top_position, ref int width, ref int height)
        {
            int width_change = (int)Math.Round(1E-3 * this.Overall_panel.Width);
            int height_change = (int)Math.Round(1E-3 * this.Overall_panel.Width);
            left_position = left_position + width_change;
            top_position = top_position + height_change;
            width = width - 2 * width_change;
            height = height - 2 * height_change;
        }

        public Button Button_standard_add_default_values_and_adjust_to_referenceBorders(Button button, int left_referenceBorder, int right_referenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_position = left_referenceBorder + Resolution_parameter.Button_X_distance_from_referenceBorder;
            int top_position = top_referenceBorder + Resolution_parameter.Button_Y_distance_from_referenceBorder;
            int width = right_referenceBorder - Resolution_parameter.Button_X_distance_from_referenceBorder - left_referenceBorder;
            int height = bottom_referenceBorder - Resolution_parameter.Button_Y_distance_from_referenceBorder - top_referenceBorder;

            if (Is_mono)
            {
                Correct_positions_and_sizes_of_standard_button_for_mono(ref left_position, ref top_position, ref width, ref height);
            }

            button.BackColor = Color_button_notPressed_back;
            button.ForeColor = Color_button_notPressed_fore;
            button.Location = new Point(left_position, top_position);
            button.Size = new Size(width, height);
            button = Adjust_font_size_to_button_size(button);
            return button;
        }

        public Button Button_2nd_add_default_values(Button button)
        {
            button.BackColor = Color_secondaryButton_notPressed_back;
            button.ForeColor = Color_secondaryButton_notPressed_fore;
            button.Location = new Point((int)Math.Round((double)button.Location.X * Resolution_parameter.X_factor), (int)Math.Round((double)button.Location.Y * Resolution_parameter.Y_factor));
            button.Size = new Size((int)Math.Round((double)75.0 * Resolution_parameter.X_factor), (int)Math.Round((double)25.0 * Resolution_parameter.Y_factor));
            button = Adjust_font_size_to_button_size(button);
            return button;
        }

        public Button Button_2nd_add_default_values_and_adjust_to_referenceBorders(Button button, int left_referenceBorder, int right_referenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_position = left_referenceBorder + Resolution_parameter.Button_X_distance_from_referenceBorder;
            int top_position = top_referenceBorder + Resolution_parameter.Button_Y_distance_from_referenceBorder;
            int width = right_referenceBorder - Resolution_parameter.Button_X_distance_from_referenceBorder - left_referenceBorder;
            int height = bottom_referenceBorder - Resolution_parameter.Button_Y_distance_from_referenceBorder - top_referenceBorder;

            if (Is_mono)
            {
                //Correct_positions_and_sizes_for_mono(ref left_position, ref top_position, ref width, ref height);
            }

            button.BackColor = Color_secondaryButton_notPressed_back;
            button.ForeColor = Color_secondaryButton_notPressed_fore;
            button.Location = new Point(left_position,top_position);
            button.Size = new Size(width, height);
            button = Adjust_font_size_to_button_size(button);
            return button;
        }

        public Button Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(Button button, int left_referenceBorder, int right_referenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_border = left_referenceBorder + Resolution_parameter.CheckBox_X_distance_from_referenceBorder;
            int right_border = right_referenceBorder - Resolution_parameter.CheckBox_X_distance_from_referenceBorder;
            int top_border = top_referenceBorder + Resolution_parameter.CheckBox_Y_distance_from_referenceBorder;
            int bottom_border = bottom_referenceBorder - Resolution_parameter.CheckBox_Y_distance_from_referenceBorder;

            int height = bottom_border - top_border;
            int width = right_border - left_border;
            int length_of_squareBorder = -1;

            if (width < height * Resolution_parameter.Aspect_ratio_screen)
            { length_of_squareBorder = width; }
            else { length_of_squareBorder = height; }

            left_border = right_border - length_of_squareBorder;
            top_border = bottom_border - length_of_squareBorder;

            if (Is_mono)
            {
                //Correct_positions_and_sizes_for_mono(ref left_border, ref top_border, ref width, ref height);
            }

            button.BackColor = Color_secondaryButton_notPressed_back;
            button.ForeColor = Color_secondaryButton_notPressed_fore;

            button.Location = new Point(left_border,top_border);
            button.Size = new Size(length_of_squareBorder, length_of_squareBorder);

            button = Adjust_font_size_to_button_size(button);

            return button;
        }

        #endregion

        #region CheckBox Button
        public MyCheckBox_button MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(MyCheckBox_button my_cb_button, int left_referenceBorder, int right_refrenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_position = left_referenceBorder + Resolution_parameter.CheckBoxButton_X_distance_from_referenceBorder;
            int width = right_refrenceBorder - Resolution_parameter.CheckBoxButton_X_distance_from_referenceBorder - left_position;
            int top_position = top_referenceBorder + Resolution_parameter.CheckBoxButton_Y_distance_from_referenceBorder;
            int height = bottom_referenceBorder - Resolution_parameter.CheckBoxButton_Y_distance_from_referenceBorder - top_position;

            if (Is_mono)
            {
                //Correct_positions_and_sizes_for_mono(ref left_position, ref top_position, ref width, ref height);
            }

            my_cb_button.Checked_backColor = Color_checkBox_button_pressed_back;
            my_cb_button.NotChecked_backColor = Color_checkBox_button_notPressed_back;
            my_cb_button.Checked_foreColor = Color_checkBox_button_pressed_fore;
            my_cb_button.NotChecked_foreColor = Color_checkBox_button_notPressed_fore;
            my_cb_button.AutoSize = false;
            my_cb_button.Size = new Size(width, height);
            height = my_cb_button.Size.Height;
            width = my_cb_button.Size.Width;
            left_position = left_referenceBorder + (int)Math.Round(0.5 * (right_refrenceBorder - left_referenceBorder - width));
            top_position = top_referenceBorder + (int)Math.Round(0.5 * (bottom_referenceBorder - top_referenceBorder - height));
            my_cb_button.Location = new Point(left_position, top_position);
            my_cb_button.Text = "";
            return my_cb_button;
        }
        #endregion

        public float Get_resolution_parameter_x_factor()
        {
            return Resolution_parameter.X_factor;
        }

        #region Panels and labels with fixed positions
        private MyPanel MyPanelDefault_add_default_parameters_relative_to_reference_borders(MyPanel my_panel, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {

            int left_border = -1;
            if (left_reference_border >= 0) { left_border = left_reference_border + Resolution_parameter.Panel_X_distance_from_referenceBorder; }
            else { throw new Exception(); }
            int right_border = -1;
            if (right_reference_border >= 0) { right_border = right_reference_border - Resolution_parameter.Panel_X_distance_from_referenceBorder; }
            else { throw new Exception(); }
            int top_border = -1;
            if (top_reference_border >= 0) { top_border = top_reference_border + Resolution_parameter.Panel_Y_distance_from_referenceBorder; }
            else { throw new Exception(); }
            int bottom_border = -1;
            if (bottom_reference_border >= 0) { bottom_border = bottom_reference_border - Resolution_parameter.Panel_Y_distance_from_referenceBorder; }
            else { throw new Exception(); }

            my_panel.Location = new Point(left_border, top_border);
            my_panel.Size = new Size(right_border - left_border, bottom_border - top_border);

            return my_panel;
        }
        public MyPanel MyPanelOverallDatsetInterface_add_default_parameters(MyPanel my_panel)
        {
            my_panel.BackColor = Color_panel_backColor;
            my_panel.Border_color = Color.Transparent;
            my_panel.Location = new Point((int)Math.Round((double)0 * Resolution_parameter.X_factor), (int)Math.Round((double)110.0 * Resolution_parameter.Y_factor));
            my_panel.Size = new Size((int)Math.Round((double)878.0 * Resolution_parameter.X_factor), (int)Math.Round((double)477.0 * Resolution_parameter.Y_factor));
            return my_panel;
        }
        public void Get_myPanelTextBoxProgressReport_default_parameters(out int left_position, out int top_position, out int right_position, out int bottom_position, out Color backColor, out Color foreColor)
        {
            backColor = Color_panel_backColor;
            foreColor = Color_label_foreColor;
            right_position = (int)Math.Round(878.0 * Resolution_parameter.X_factor);
            top_position = (int)Math.Round((110 + 477) * Resolution_parameter.Y_factor);
            left_position = 0;
            bottom_position = top_position + (int)Math.Round(60 * Resolution_parameter.Y_factor);
        }
        public MyPanel MyPanelResultsVisualization_add_default_parameters(MyPanel my_panel)
        {
            my_panel.BackColor = Color_panel_backColor;
            my_panel.Border_color = Color.Transparent;
            my_panel.Location = new Point((int)Math.Round((double)0 * Resolution_parameter.X_factor), (int)Math.Round((double)110 * Resolution_parameter.Y_factor));
            my_panel.Size = new Size((int)Math.Round((double)878.0 * Resolution_parameter.X_factor), (int)Math.Round((double)700 * Resolution_parameter.Y_factor));
            return my_panel;
        }
        public MyPanel MyPanelOverallApplicationSize_add_default_parameters(MyPanel my_panel)
        {
            my_panel.BackColor = Color_panel_backColor;
            my_panel.Border_color = Color.Transparent;
            my_panel.Location = new Point((int)Math.Round((double)7.0 * Resolution_parameter.X_factor), (int)Math.Round((double)60.0 * Resolution_parameter.Y_factor));
            my_panel.Size = new Size((int)Math.Round((double)700.0 * Resolution_parameter.X_factor), (int)Math.Round((double)50.0 * Resolution_parameter.Y_factor));
            return my_panel;
        }
        public MyPanel MyPanelOverallMenu_add_default_parameters(MyPanel my_panel)
        {
            my_panel.BackColor = Color_panel_backColor;
            my_panel.Border_color = Color_panel_borderColor;
            my_panel.Location = new Point((int) Math.Round((double)880.0 * Resolution_parameter.X_factor), (int) Math.Round((double)60.0 * Resolution_parameter.Y_factor));
            my_panel.Size = new Size((int) Math.Round((double)360.0 * Resolution_parameter.X_factor), (int) Math.Round((double)529.0 * Resolution_parameter.Y_factor));
            return my_panel;
        }
        public Label LabelProgressReport_set_sizes_and_fontSize(Label my_label, int left0_middle1)
        {
            my_label = LabelDefaultBold_set_font(my_label);
            int left_position = 0;
            int right_position = (int)Math.Round(878 * Resolution_parameter.X_factor);
            int top_position = (int)Math.Round((110 + 477)*Resolution_parameter.Y_factor);
            int bottom_position = top_position + (int)Math.Round(30*Resolution_parameter.Y_factor);
            int height = bottom_position - top_position;
            int width = right_position - left_position;

            switch (left0_middle1)
            {
                case 0: 
                    left_position = 0;
                    break;
                case 1:
                    left_position = 0;//167
                    break;
                default: 
                    throw new Exception();
            }
            my_label.Location = new Point(left_position, top_position);
            my_label.Size = new Size(width, height);
            Size size;
            string text = (string)my_label.Text.Clone();
            my_label.Font = Adjust_font_size_and_text_to_area_size(ref text, my_label.Font, out size, width, height, 9999, -1);
            //my_label.Size = size; Do not change given size, since it is just as a reference
            return my_label;
        }
        #endregion

        #region Panels
        public MyPanel MyPanelDefaultBlackFrame_add_default_parameters(MyPanel my_panel, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            my_panel = MyPanelDefault_add_default_parameters_relative_to_reference_borders(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            my_panel.BackColor = Color_panel_backColor;
            my_panel.Border_color = Color_panel_borderColor;
            return my_panel;
        }
        public MyPanel MyPanelDefaultTransparentFrame_add_default_parameters(MyPanel my_panel, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            my_panel = MyPanelDefault_add_default_parameters_relative_to_reference_borders(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            my_panel.BackColor = Color_panel_backColor;
            my_panel.Border_color = Color.Transparent;
            return my_panel;
        }
        public MyPanel MyPanelDefaultTransparentFrame_add_default_parameters_considering_only_width_and_top_bottom_reference_borders(MyPanel my_panel, int top_referenceBorder, int bottom_referenceBorder, int width)
        {
            int top_border = top_referenceBorder + Resolution_parameter.Panel_Y_distance_from_referenceBorder;
            int bottom_border = bottom_referenceBorder - Resolution_parameter.Panel_Y_distance_from_referenceBorder;

            int new_width = width;
            int new_height = bottom_border-top_referenceBorder;

            my_panel.BackColor = Color_overall_background;
            my_panel.Border_color = Color.Transparent;
            my_panel.Location = new Point(my_panel.Location.X,top_border);
            my_panel.Size = new Size(new_width, new_height);
            return my_panel;
        }
        #endregion

        #region Label
        private Label Label_adjust_fontSize_to_given_positions(Label label, int left_border_position, int right_border_position, int top_border_position, int bottom_border_position, float max_fontSize)
        {
            int height = Math.Max(1,bottom_border_position - top_border_position - 2* Resolution_parameter.Label_Y_distance_from_referenceBorder);
            int width = Math.Max(1, right_border_position - left_border_position - 2* Resolution_parameter.Label_X_distance_from_referenceBorder);
            if ((height<=0)||(width<=0)) { throw new Exception(); }
            Size size;
            string text = (string)label.Text.Clone();
            label.Font = Adjust_font_size_and_text_to_area_size(ref text, label.Font, out size, width, height, max_fontSize, -1);
            //label.Font = Adjust_font_size_to_area_size(text, label.Font, out size, width, height);
            label.Text = (string)text.Clone();
            label.Size = new Size(size.Width, size.Height);
            //label = Decrease_label_size_if_label_not_visible(label);
            return label;
        }

        private Label Default_set(Label label)
        {
            label.Left = 0;
            label.Top = 0;
            label.BackColor = Color_label_backColor;
            label.ForeColor = Color_label_foreColor;
            //label.Anchor = AnchorStyles.None;
            label.AutoSize = true;
            label.Dock = DockStyle.None;
            return label;
        }

        private Label LabelDefaultBold_set_font(Label label)
        {
            label = Default_set(label);
            label.Font = new Font("Arial",1, FontStyle.Bold, GraphicsUnit.Pixel);
            return label;
        }
        private Label LabelDefaultRegular_set_font(Label label)
        {
            label = Default_set(label);
            label.Font = new Font("Arial", 1, FontStyle.Regular, GraphicsUnit.Pixel);
            return label;
        }
        private Label LabelHeadline_set_font(Label label)
        {
            label = Default_set(label);
            label.Font = new Font("Arial", 1, FontStyle.Bold, GraphicsUnit.Pixel);
            return label;
        }
        private Label LabelUnderlinedHeadline_set_font(Label label)
        {
            label = Default_set(label);
            if (!Is_mono)
            {
                label.Font = new Font("Arial", 1, FontStyle.Underline, GraphicsUnit.Pixel);
            }
            else
            {
                label.Font = new Font("Arial", 1, FontStyle.Regular, GraphicsUnit.Pixel);
            }
            return label;
        }
        private Label LabelExplanation_set_font(Label label)
        {
            label = Default_set(label);
            label.Font = new Font("Arial", 1, FontStyle.Italic, GraphicsUnit.Pixel);
            return label;
        }
        private Label Label_attach_y_position_to_top(Label label, int top_position)
        {
            label.Location = new Point(label.Location.X, top_position);
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    label.TextAlign = ContentAlignment.TopLeft;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    label.TextAlign = ContentAlignment.TopCenter;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    label.TextAlign = ContentAlignment.TopRight;
                    break;
                default:
                    throw new Exception();
            }
            return label;
        }
        private Label Label_attach_y_position_to_bottom(Label label, int bottom_position)
        {
            label.Location = new Point(label.Location.X, bottom_position - label.Height);
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    label.TextAlign = ContentAlignment.BottomLeft;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    label.TextAlign = ContentAlignment.BottomCenter;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    label.TextAlign = ContentAlignment.BottomRight;
                    break;
                default:
                    throw new Exception();
            }
            return label;
        }
        private Label Label_center_y_position(Label label, int top_border_position, int bottom_border_position)
        {
            int top_position = top_border_position + Resolution_parameter.Label_Y_distance_from_referenceBorder;
            int bottom_position = bottom_border_position - Resolution_parameter.Label_Y_distance_from_referenceBorder;

            int center = top_position + (int)Math.Round(0.5F * (bottom_position - top_position));
            label.Location = new Point(label.Location.X, center - (int)Math.Round(0.5 * label.Height));
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;
                default:
                    throw new Exception();
            }
            return label;
        }
        private Label Label_center_x_position(Label label, int left_border_position, int right_border_position)
        {
            int left_position = left_border_position + Resolution_parameter.Label_X_distance_from_referenceBorder;
            int right_position = right_border_position - Resolution_parameter.Label_X_distance_from_referenceBorder;

            int center = left_position + (int)Math.Round(0.5F * (right_position - left_position));
            label.Location = new Point(center - (int)Math.Round(0.5 * label.Width), label.Location.Y);
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    label.TextAlign = ContentAlignment.TopCenter;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    label.TextAlign = ContentAlignment.BottomCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    break;
                default:
                    throw new Exception();
            }
            return label;
        }
        private Label Label_attach_to_right_x_position(Label label, int right_border_position)
        {
            int rightPosition = right_border_position - Resolution_parameter.Label_X_distance_from_referenceBorder;
            label.Location = new Point(rightPosition - label.Width, label.Location.Y);
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    label.TextAlign = ContentAlignment.TopRight;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    label.TextAlign = ContentAlignment.BottomRight;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;
                default:
                    throw new Exception();
            }
            return label;
        }
        private Label Label_attach_to_left_x_position(Label label, int left_border_position)
        {
            int leftPosition = left_border_position + Resolution_parameter.Label_X_distance_from_referenceBorder;

            label.Location = new Point(leftPosition, label.Location.Y);
            switch (label.TextAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    label.TextAlign = ContentAlignment.TopLeft;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    label.TextAlign = ContentAlignment.BottomLeft;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    break;
                default:
                    throw new Exception();
            }
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            label = Label_attach_to_right_x_position(label, rightPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            label = Label_attach_to_left_x_position(label, leftPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultRegular_set_font(label);
            float max_fontSize = Max_fontSize_defaultRegular;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            label = Label_attach_to_left_x_position(label, leftPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultRegular_set_font(label);
            float max_fontSize = Max_fontSize_defaultRegular;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_y_position_to_top(label, topPosition);
            label = Label_attach_to_left_x_position(label, leftPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultRegular_adjust_to_given_referenceBorders_and_center_x_and_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            label = LabelDefaultRegular_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_to_given_referenceBorders_and_center_x_and_y(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border, max_fontSize);
            label.Refresh();
            label.Update();
            return label;
        }

        public Label LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_lowerYPostion(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_y_position_to_bottom(label, bottomPosition);
            label = Label_attach_to_right_x_position(label, rightPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_upperYPostion(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_y_position_to_top(label, topPosition);
            label = Label_attach_to_right_x_position(label, rightPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_y_position_to_bottom(label, bottomPosition);
            label = Label_attach_to_left_x_position(label, leftPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(Label label, int leftPosition, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_fontSize_to_given_positions(label, leftPosition, rightPosition, topPosition, bottomPosition,max_fontSize);
            label = Label_attach_y_position_to_top(label,  topPosition);
            label = Label_attach_to_left_x_position(label, leftPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelHeadline_adjust_to_given_positions_and_center_x_and_y(Label label, int leftPostion, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelHeadline_set_font(label);
            float max_fontSize = Max_fontSize_headline;
            label = Label_adjust_fontSize_to_given_positions(label, leftPostion, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_center_x_position(label, leftPostion, rightPosition);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelHeadline_adjust_to_given_positions_and_attachTo_leftSide_and_center_y(Label label, int leftPostion, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelHeadline_set_font(label);
            float max_fontSize = Max_fontSize_headline;
            label = Label_adjust_fontSize_to_given_positions(label, leftPostion, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_to_left_x_position(label, leftPostion);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelHeadline_adjust_to_given_positions_and_attachTo_leftSide_and_topSide(Label label, int leftPostion, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelHeadline_set_font(label);
            float max_fontSize = Max_fontSize_headline;
            label = Label_adjust_fontSize_to_given_positions(label, leftPostion, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_to_left_x_position(label, leftPostion);
            label = Label_attach_y_position_to_top(label, topPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(Label label, int leftPostion, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelUnderlinedHeadline_set_font(label);
            float max_fontSize = Max_fontSize_headlineUnderline;
            label = Label_adjust_fontSize_to_given_positions(label, leftPostion, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_to_left_x_position(label, leftPostion);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(Label label, int leftPostion, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelExplanation_set_font(label);
            float max_fontSize = Max_fontSize_explanationText;
            label = Label_adjust_fontSize_to_given_positions(label, leftPostion, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_to_left_x_position(label, leftPostion);
            label = Label_center_y_position(label, topPosition, bottomPosition);
            return label;
        }
        public Label LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(Label label, int leftPostion, int rightPosition, int topPosition, int bottomPosition)
        {
            label = LabelExplanation_set_font(label);
            float max_fontSize = Max_fontSize_explanationText;
            label = Label_adjust_fontSize_to_given_positions(label, leftPostion, rightPosition, topPosition, bottomPosition, max_fontSize);
            label = Label_attach_to_left_x_position(label, leftPostion);
            label = Label_attach_y_position_to_top(label, topPosition);
            return label;
        }
        private Label Label_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border, float max_fontSize)
        {
            label = Label_adjust_fontSize_to_given_positions(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border, max_fontSize);
            label = Label_center_x_position(label, left_reference_border, right_reference_border);
            label = Label_attach_y_position_to_bottom(label, bottom_reference_border);
            return label;
        }
        private Label Label_adjust_to_given_referenceBorders_and_center_x_and_attach_to_upper_reference_at_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border, float max_fontSize)
        {
            label = Label_adjust_fontSize_to_given_positions(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border, max_fontSize);
            label = Label_center_x_position(label, left_reference_border, right_reference_border);
            label = Label_attach_y_position_to_top(label, top_reference_border);
            return label;
        }
        private Label Label_adjust_to_given_referenceBorders_and_center_x_and_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border, float max_fontSize)
        {
            label = Label_adjust_fontSize_to_given_positions(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border, max_fontSize);
            label = Label_center_x_position(label, left_reference_border, right_reference_border);
            label = Label_center_y_position(label, top_reference_border, bottom_reference_border);
            return label;
        }
        public Label LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            label = LabelExplanation_set_font(label);
            float max_fontSize = Max_fontSize_explanationText;
            label = Label_adjust_to_given_referenceBorders_and_center_x_and_y(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border,max_fontSize);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_to_given_referenceBorders_and_center_x_and_y(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border,max_fontSize);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            if (left_reference_border > right_reference_border) { throw new Exception(); }
            if (top_reference_border > bottom_reference_border) { throw new Exception(); }
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border, max_fontSize);
            label.Refresh();
            label.Update();
            return label;
        }
        public Label LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_upper_reference_at_y(Label label, int left_reference_border, int right_reference_border, int top_reference_border, int bottom_reference_border)
        {
            if (left_reference_border > right_reference_border) { throw new Exception(); }
            if (top_reference_border > bottom_reference_border) { throw new Exception(); }
            label = LabelDefaultBold_set_font(label);
            float max_fontSize = Max_fontSize_defaultBold;
            label = Label_adjust_to_given_referenceBorders_and_center_x_and_attach_to_upper_reference_at_y(label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border, max_fontSize);
            label.Refresh();
            label.Update();
            return label;
        }
        #endregion

        #region Set Textboxes
        private OwnTextBox MyTextBox_adjust_default_parameters_to_existing_coordinates(OwnTextBox myTextBox, int new_height, bool one_line_only)
        {
            int new_height_of_one_line = -1;
            if (one_line_only)
            {
                myTextBox.Multiline = false;
                new_height_of_one_line = new_height;
            }
            else
            {
                new_height_of_one_line = TextBox_height_of_one_line;
                myTextBox.Multiline = true;
            }
            new_height_of_one_line = (int)Math.Round(Resolution_parameter.TextBox_heightFraction_for_fontSize * new_height_of_one_line);
            myTextBox.Font = new Font("Arial", 1, FontStyle.Regular, GraphicsUnit.Pixel);
            myTextBox.Font = Adjust_font_size_to_height_of_one_line(myTextBox.Font, new_height_of_one_line);
            myTextBox.Dock = DockStyle.None;
            myTextBox.TextAlign = HorizontalAlignment.Left;
            myTextBox.BackColor = Color_textBox_backColor;
            myTextBox.ForeColor = Color_textBox_foreColor;
            myTextBox.Size = new Size(myTextBox.Size.Width, new_height);
            return myTextBox;
        }
        private OwnTextBox MyTextBox_add_default_parameter_and_update_coordinates(OwnTextBox myTextBox, bool one_line_only)
        {
            int height_of_one_line = 22;
            int number_of_lines = -1;
            if (one_line_only)
            { 
                number_of_lines = 1;
                myTextBox.Multiline = false;
            }
            else 
            { 
                number_of_lines = (int)Math.Floor((double)myTextBox.Height / height_of_one_line);
                myTextBox.Multiline = true;
            }
            int new_location_x = (int)Math.Round((float)myTextBox.Location.X * Resolution_parameter.X_factor);
            int new_location_y = (int)Math.Round((float)myTextBox.Location.Y * Resolution_parameter.Y_factor);
            myTextBox.Location = new Point(new_location_x, new_location_y);
            int new_height_of_one_line = (int)Math.Round((double)height_of_one_line * Resolution_parameter.Y_factor);
            int new_height = new_height_of_one_line * number_of_lines;
            int new_width = (int)Math.Round((double)myTextBox.Size.Width * Resolution_parameter.X_factor);
            myTextBox.Size = new Size(new_width, new_height);
            myTextBox = MyTextBox_adjust_default_parameters_to_existing_coordinates(myTextBox, new_height,one_line_only);
            return myTextBox;
        }
        public OwnTextBox MyTextBoxSingleLine_updateCoordinates_add_default_parameter(OwnTextBox textBox)
        {
            return MyTextBox_add_default_parameter_and_update_coordinates(textBox, true);
        }
        public OwnTextBox MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(OwnTextBox textBox, int left_referenceBorder, int right_referenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_position = left_referenceBorder + Resolution_parameter.TextBox_X_distance_from_referenceBorder;
            int right_position = right_referenceBorder - Resolution_parameter.TextBox_X_distance_from_referenceBorder;
            int top_position = top_referenceBorder + Resolution_parameter.TextBox_Y_distance_from_referenceBorder;
            int bottom_position = bottom_referenceBorder - Resolution_parameter.TextBox_Y_distance_from_referenceBorder;
            int width = right_position - left_position;
            int height = bottom_position - top_position;

            textBox.Location = new Point(left_position, top_position);
            textBox.Size = new Size(width, height);

            MyTextBox_adjust_default_parameters_to_existing_coordinates(textBox, height, true);
            return textBox;
        }

        public OwnTextBox MyTextBoxMultiLine_adjustCoordinatesToBorders_add_default_parameter(OwnTextBox textBox, int left_referenceBorder, int right_referenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_position = left_referenceBorder + Resolution_parameter.TextBox_X_distance_from_referenceBorder;
            int right_position = right_referenceBorder - Resolution_parameter.TextBox_X_distance_from_referenceBorder;
            int top_position = top_referenceBorder + Resolution_parameter.TextBox_Y_distance_from_referenceBorder;
            int bottom_position = bottom_referenceBorder - Resolution_parameter.TextBox_Y_distance_from_referenceBorder;
            int width = right_position - left_position;
            int height = bottom_position - top_position;

            textBox.Location = new Point(left_position, top_position);
            textBox.Size = new Size(width, height);

            MyTextBox_adjust_default_parameters_to_existing_coordinates(textBox, height, false);
            return textBox;
        }
        public OwnTextBox MyTextBoxMultiLine_adjustCoordinatesToExactPositions_add_default_parameter(OwnTextBox textBox, int left_position, int right_position, int top_position, int bottom_position)
        {
            int width = right_position - left_position;
            int height = bottom_position - top_position;

            textBox.Location = new Point(left_position, top_position);
            textBox.Size = new Size(width, height);

            MyTextBox_adjust_default_parameters_to_existing_coordinates(textBox, height, false);
            return textBox;
        }
        #endregion

        #region Set ListBoxes
        private OwnListBox MyListBox_add_default_parameter_without_changing_coordinates(OwnListBox myListBox, int height_of_one_line)
        {
            myListBox.IntegralHeight = true;
            myListBox.Dock = DockStyle.Fill;
            //myListBox.Anchor = AnchorStyles.None;
            myListBox.Silent_font = new Font("Arial", 1, FontStyle.Regular, GraphicsUnit.Pixel);
            myListBox.Dock = DockStyle.None;
            myListBox.Silent_font = Adjust_font_size_to_height_of_one_line(myListBox.Font, height_of_one_line);
            myListBox.ItemHeight = height_of_one_line;
            myListBox.BackColor = Color_listBox_backColor;
            myListBox.ForeColor = Color_listBox_foreColor;
           // myListBox.SilentSelectedIndex = myListBox.Items.IndexOf(myListBox.SelectedItem);
            return myListBox;
        }
        public OwnListBox MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(OwnListBox myListBox, int left_referenceBorder, int right_refrenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_postion = left_referenceBorder + Resolution_parameter.ListBox_X_distance_from_referenceBorder;
            int width = right_refrenceBorder - Resolution_parameter.ListBox_X_distance_from_referenceBorder - left_postion;
            int top_postion = top_referenceBorder + Resolution_parameter.ListBox_Y_distance_from_referenceBorder;
            int height = bottom_referenceBorder - Resolution_parameter.ListBox_Y_distance_from_referenceBorder - top_postion;
            int height_of_one_line = (int)Math.Round(Resolution_parameter.ListBox_heightFraction_for_fontSize * height);

            myListBox = MyListBox_add_default_parameter_without_changing_coordinates(myListBox, height_of_one_line);
            myListBox.Location = new Point(left_postion, top_postion);
            myListBox.Size = new Size(width, height);
            return myListBox;
        }
        public OwnListBox MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(OwnListBox myListBox, int left_referenceBorder, int right_refrenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_postion = left_referenceBorder + Resolution_parameter.ListBox_X_distance_from_referenceBorder;
            int width = right_refrenceBorder - Resolution_parameter.ListBox_X_distance_from_referenceBorder - left_postion;
            int top_postion = top_referenceBorder + Resolution_parameter.ListBox_Y_distance_from_referenceBorder;
            int height = bottom_referenceBorder - Resolution_parameter.ListBox_Y_distance_from_referenceBorder - top_postion;
            int height_of_one_line = ListBox_height_of_one_line;

            myListBox = MyListBox_add_default_parameter_without_changing_coordinates(myListBox, height_of_one_line);
            myListBox.Location = new Point(left_postion, top_postion);
            myListBox.Size = new Size(width, height);
            return myListBox;
        }
        #endregion

        #region Set scrollBars
        public ScrollBar ScrollBar_adjustCoordinatedToBorders_and_add_default_parameter(ScrollBar my_scrollBar, int left_referenceBorder, int right_referenceBorder, int top_referenceBorder, int bottom_referenceBorder)
        {
            int left_position = left_referenceBorder + Resolution_parameter.ScrollBar_X_distance_from_referenceBorder;
            int right_position = right_referenceBorder - Resolution_parameter.ScrollBar_X_distance_from_referenceBorder;
            int top_position = top_referenceBorder + Resolution_parameter.ScrollBar_Y_distance_from_referenceBorder;
            int bottom_position = bottom_referenceBorder - Resolution_parameter.ScrollBar_Y_distance_from_referenceBorder;
            int height = bottom_position - top_position;
            int width = right_position - left_position;

            my_scrollBar.Location = new Point(left_position, top_position);
            my_scrollBar.Size = new Size(width, height);
            return my_scrollBar;
        }
        #endregion
    }


}
