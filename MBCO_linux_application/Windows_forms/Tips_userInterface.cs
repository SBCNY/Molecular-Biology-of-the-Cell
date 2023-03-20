using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_forms_customized_tools;
using Common_functions.Form_tools;


namespace ClassLibrary1.Tips_userInterface
{
    class Tips_userInterface_class
    {
        private MyPanel Overall_panel { get; set; }
        private Label OverallHeadline_label { get; set; }
        private Label Label1 { get; set; }
        private Label Label2 { get; set; }
        private Label Label3 { get; set; }
        private Label Label4 { get; set; }
        private Label Label5 { get; set; }
        private Label Label6 { get; set; }
        private Label Demonstration_headline_label { get; set; }
        private MyCheckBox_button Demonstration_cbButton { get; set; }
        private Label Demonstration_cbLabel { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }

        public Tips_userInterface_class(MyPanel overall_panel,
                                        Label overallHeadline_label,
                                        Label label1,
                                        Label label2,
                                        Label label3,
                                        Label label4,
                                        Label label5,
                                        Label label6,
                                        Label demonstration_headline_label,
                                        MyCheckBox_button demonstration_cbButton,
                                        Label demonstration_cbLabel,
                                        Form1_default_settings_class form_default_settings)
        {
            this.Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.OverallHeadline_label = overallHeadline_label;
            this.Label1 = label1;
            this.Label2 = label2;
            this.Label3 = label3;
            this.Label4 = label4;
            this.Label5 = label5;
            this.Label6 = label6;
            this.Demonstration_headline_label = demonstration_headline_label;
            this.Demonstration_cbButton = demonstration_cbButton;
            this.Demonstration_cbLabel = demonstration_cbLabel;
            Update_all_graphic_elements();
        }

        public void Update_all_graphic_elements()
        { 

            int left_reference_position;
            int right_reference_position;
            int top_reference_position;
            int bottom_reference_position;
            Label my_label;
            MyCheckBox_button my_cbButton;

            Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            left_reference_position = (int)Math.Round(0.01 * Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.99 * Overall_panel.Width);

            top_reference_position = 0;
            bottom_reference_position = (int)Math.Round(0.06 * Overall_panel.Height);
            my_label = OverallHeadline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            int shared_height_of_each_row = (int)Math.Round((float)(Overall_panel.Height - OverallHeadline_label.Location.Y - OverallHeadline_label.Height) / 31);
            int space_between_tips = (int)Math.Round(0.5 * shared_height_of_each_row);

            top_reference_position = bottom_reference_position;
            bottom_reference_position = top_reference_position + 3 * shared_height_of_each_row;
            my_label = Label1;
            Form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = bottom_reference_position + space_between_tips;
            bottom_reference_position = top_reference_position + 5 * shared_height_of_each_row;
            my_label = Label2;
            Form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = bottom_reference_position + space_between_tips;
            bottom_reference_position = top_reference_position + 2 * shared_height_of_each_row;
            my_label = Label3;
            Form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = bottom_reference_position + space_between_tips;
            bottom_reference_position = top_reference_position + 4 * shared_height_of_each_row;
            my_label = Label4;
            Form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = bottom_reference_position + space_between_tips;
            bottom_reference_position = top_reference_position + 5 * shared_height_of_each_row;
            my_label = Label5;
            Form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = bottom_reference_position + space_between_tips;
            bottom_reference_position = top_reference_position + 5 * shared_height_of_each_row;
            my_label = Label6;
            Form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            int sizeOfCbButton = (int)Math.Round(0.05F * Overall_panel.Height);

            top_reference_position = (int)Math.Round(0.93 * Overall_panel.Height);
            bottom_reference_position = top_reference_position + sizeOfCbButton;
            right_reference_position = left_reference_position + sizeOfCbButton;
            my_cbButton = this.Demonstration_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = Demonstration_cbButton.Location.Y - Demonstration_cbButton.Height;
            bottom_reference_position = Demonstration_cbButton.Location.Y;
            right_reference_position = (int)Math.Round(0.95F * this.Overall_panel.Width);
            my_label = this.Demonstration_headline_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);



            Update_demonstration_cbLabel();
        }

        public void Update_demonstration_cbLabel()
        {
            bool cbButton_pressed = this.Demonstration_cbButton.Checked;
            if (cbButton_pressed)
            {
                this.Demonstration_cbLabel.Text = "Item is selected";
            }
            else
            {
                this.Demonstration_cbLabel.Text = "Item is not selected";
            }
            int left_reference_position = Demonstration_cbButton.Location.X + Demonstration_cbButton.Width;
            int top_reference_position = Demonstration_cbButton.Location.Y;
            int bottom_reference_position = Demonstration_cbButton.Location.Y + Demonstration_cbButton.Height;
            int right_reference_position = (int)Math.Round(0.95F*this.Overall_panel.Width);
            Label my_cbLabel = this.Demonstration_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_cbLabel, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

        }

    }
}
