//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using Windows_forms_customized_tools;
using System.Windows.Forms;
using Common_functions.Form_tools;
using System;


namespace ClassLibrary1.LoadExamples_userInterface
{
    class LoadExamples_userInterface_class
    {
        private MyPanel Overall_panel { get; set; }
        private Label Overall_headline_label { get; set; }
        public MyCheckBox_button Nog_cbButton { get; set; }
        public Label Nog_cbLabel { get; set; }
        private Label Nog_reference_label { get; set; }
        public MyCheckBox_button Kpmp_cbButton { get; set; }
        public Label Kpmp_cbLabel { get; set; }
        private Label Kpmp_reference_label { get; set; }
        private Button Load_data_button { get; set; }
        private Label Copyright_label { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }

        public LoadExamples_userInterface_class(MyPanel overall_panel,
                                                Label overall_headline_label,
                                                MyCheckBox_button nog_cbButton,
                                                Label nog_cbLabel,
                                                Label nog_reference_label,
                                                MyCheckBox_button kpmp_cbButton,
                                                Label kpmp_cbLabel,
                                                Label kpmp_reference_label,
                                                Button load_data_button,
                                                Label copyright_label,
                                                Form1_default_settings_class form_default_settings)
        {
            this.Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.Overall_headline_label = overall_headline_label;
            this.Nog_cbButton = nog_cbButton;
            this.Nog_cbLabel = nog_cbLabel;
            this.Nog_reference_label = nog_reference_label;
            this.Kpmp_cbButton = kpmp_cbButton;
            this.Kpmp_cbLabel = kpmp_cbLabel;
            this.Kpmp_reference_label = kpmp_reference_label;
            this.Load_data_button = load_data_button;
            this.Copyright_label = copyright_label;
            Update_all_graphics_elements();
            Reset_all_checkBoxes();
        }

        public void Update_all_graphics_elements()
        { 

            int left_reference_position;
            int right_reference_position;
            int top_reference_position;
            int bottom_reference_position;
            Label my_label;
            MyCheckBox_button my_cbButton;
            Button my_button;


            this.Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            int distance_from_leftRightSides = (int)Math.Round(0.02 * this.Overall_panel.Width);
            int shared_heightWidth_of_all_checkBoxes = (int)Math.Round(0.045F* this.Overall_panel.Height);
            int height_of_each_explanation_row = (int)Math.Round(0.05F * this.Overall_panel.Height);

            left_reference_position = distance_from_leftRightSides;
            top_reference_position = (int)Math.Round(0.2F*this.Overall_panel.Height);
            right_reference_position = left_reference_position + shared_heightWidth_of_all_checkBoxes;
            bottom_reference_position = top_reference_position + shared_heightWidth_of_all_checkBoxes;
            my_cbButton = this.Nog_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = this.Nog_cbButton.Location.X + this.Nog_cbButton.Width;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            my_label = this.Nog_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = distance_from_leftRightSides;
            top_reference_position = (int)Math.Round(0.4F * this.Overall_panel.Height);
            right_reference_position = left_reference_position + shared_heightWidth_of_all_checkBoxes;
            bottom_reference_position = top_reference_position + shared_heightWidth_of_all_checkBoxes;
            my_cbButton = this.Kpmp_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = this.Kpmp_cbButton.Location.X + this.Kpmp_cbButton.Width;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            my_label = this.Kpmp_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = (int)Math.Round(0.7F * Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.9F * Overall_panel.Width);
            top_reference_position = (int)Math.Round(0.88F * Overall_panel.Height);
            bottom_reference_position = (int)Math.Round(0.95F * Overall_panel.Height);
            my_button = this.Load_data_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = 0;
            right_reference_position = Overall_panel.Width- distance_from_leftRightSides;
            top_reference_position = 0;
            bottom_reference_position = this.Nog_cbButton.Location.Y;
            my_label = this.Overall_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = this.Nog_cbButton.Location.X + this.Nog_cbButton.Width;
            right_reference_position = Overall_panel.Width- distance_from_leftRightSides;
            top_reference_position = this.Nog_cbLabel.Location.Y + this.Nog_cbLabel.Height;
            bottom_reference_position = top_reference_position + 1 * height_of_each_explanation_row;
            my_label = this.Nog_reference_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = this.Kpmp_cbButton.Location.X + this.Kpmp_cbButton.Width;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            top_reference_position = this.Kpmp_cbLabel.Location.Y + this.Kpmp_cbLabel.Height;
            bottom_reference_position = top_reference_position + 3 * height_of_each_explanation_row;
            my_label = this.Kpmp_reference_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = distance_from_leftRightSides;
            right_reference_position = Load_data_button.Location.X;
            top_reference_position = (int)Math.Round(0.85F * Overall_panel.Height);
            bottom_reference_position = top_reference_position + 2 * height_of_each_explanation_row;
            my_label = this.Copyright_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);
        }

        public void Set_visibility(bool visible)
        {
            this.Overall_panel.Visible = visible;
        }

        private void Reset_all_checkBoxes()
        {
            this.Nog_cbButton.SilentChecked = false;
            this.Kpmp_cbButton.SilentChecked = false;
        }

        public void NOG_checkBox_CheckedChanged()
        {
            bool selected = this.Nog_cbButton.Checked;
            Reset_all_checkBoxes();
            this.Nog_cbButton.SilentChecked = selected;
        }
        public void KPMPreference_checkBox_CheckedChanged()
        {
            bool selected = this.Kpmp_cbButton.Checked;
            Reset_all_checkBoxes();
            this.Kpmp_cbButton.SilentChecked = selected;
        }
    }
}
