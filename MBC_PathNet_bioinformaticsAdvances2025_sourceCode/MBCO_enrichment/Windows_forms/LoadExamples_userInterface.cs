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
using Windows_forms;
using Common_functions.Global_definitions;
using System.Collections.Generic;
using System.Drawing;


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
        public MyCheckBox_button Dtoxs_cbButton { get; set; }
        public Label Dtoxs_cbLabel { get; set; }
        private Label Dtoxs_reference_label { get; set; }
        private Button Load_data_button { get; set; }
        private Button Tutorial_button { get; set; }
        private Label Copyright_label { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }

        public LoadExamples_userInterface_class(MyPanel overall_panel,
                                                Label overall_headline_label,
                                                MyCheckBox_button nog_cbButton,
                                                Label nog_cbLabel,
                                                Label nog_reference_label,
                                                MyCheckBox_button kpmp_cbButton,
                                                Label kpmp_cbLabel,
                                                Label kpmp_reference_label,
                                                MyCheckBox_button dtoxs_cbButton,
                                                Label dtoxs_cbLabel,
                                                Label dtoxs_reference_label,
                                                Button load_data_button,
                                                Button tutorial_button,
                                                Label copyright_label,
                                                Tutorial_interface_class userInterface_tutorial,
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
            this.Dtoxs_cbButton = dtoxs_cbButton;
            this.Dtoxs_cbLabel = dtoxs_cbLabel;
            this.Dtoxs_reference_label = dtoxs_reference_label;
            this.Load_data_button = load_data_button;
            this.Tutorial_button = tutorial_button;
            this.Copyright_label = copyright_label;
            this.UserInterface_tutorial = userInterface_tutorial;
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

            left_reference_position = distance_from_leftRightSides;
            top_reference_position = (int)Math.Round(0.6F * this.Overall_panel.Height);
            right_reference_position = left_reference_position + shared_heightWidth_of_all_checkBoxes;
            bottom_reference_position = top_reference_position + shared_heightWidth_of_all_checkBoxes;
            my_cbButton = this.Dtoxs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = this.Dtoxs_cbButton.Location.X + this.Dtoxs_cbButton.Width;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            my_label = this.Dtoxs_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = 0;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            top_reference_position = 0;
            bottom_reference_position = this.Nog_cbButton.Location.Y;
            my_label = this.Overall_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = this.Nog_cbButton.Location.X + this.Nog_cbButton.Width;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
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

            left_reference_position = this.Dtoxs_cbButton.Location.X + this.Dtoxs_cbButton.Width;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            top_reference_position = this.Dtoxs_cbLabel.Location.Y + this.Dtoxs_cbLabel.Height;
            bottom_reference_position = top_reference_position + 3 * height_of_each_explanation_row;
            my_label = this.Dtoxs_reference_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = (int)Math.Round(0.02F * Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.3F * Overall_panel.Width);
            //top_reference_position = (int)Math.Round(0.85F * Overall_panel.Height);
            //bottom_reference_position = (int)Math.Round(0.91F * Overall_panel.Height);
            top_reference_position = (int)Math.Round(0.92F * Overall_panel.Height);
            bottom_reference_position = (int)Math.Round(0.98F * Overall_panel.Height);
            my_button = this.Tutorial_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = (int)Math.Round(0.7F * Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.98F * Overall_panel.Width);
            //top_reference_position = (int)Math.Round(0.92F * Overall_panel.Height);
            //bottom_reference_position = (int)Math.Round(0.98F * Overall_panel.Height);
            my_button = this.Load_data_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            left_reference_position = distance_from_leftRightSides;
            right_reference_position = Overall_panel.Width - distance_from_leftRightSides;
            top_reference_position = (int)Math.Round(0.80F * Overall_panel.Height);
            bottom_reference_position = top_reference_position + (int)Math.Round(1.5F * height_of_each_explanation_row);
            my_label = this.Copyright_label;
            Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);
        }

        public void Set_visibility(bool visible)
        {
            this.Overall_panel.Visible = visible;
        }

        private void Reset_all_checkBoxes()
        {
            this.Nog_cbButton.SilentChecked = false;
            this.Kpmp_cbButton.SilentChecked = false;
            this.Dtoxs_cbButton.SilentChecked = false;
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
        public void Dtoxs_checkBox_CheckedChanged()
        {
            bool selected = this.Dtoxs_cbButton.Checked;
            Reset_all_checkBoxes();
            this.Dtoxs_cbButton.SilentChecked = selected;
        }
        #region Tutorial
        public void Set_explanation_or_tutorial_button_to_active(Button selected_button)
        {
            selected_button.BackColor = Form_default_settings.Color_button_pressed_back;
            selected_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            selected_button.Refresh();
        }
        public void Set_explanation_and_tutorial_buttons_to_inactive()
        {
            Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Tutorial_button.Refresh();
        }
        public bool Is_given_explanation_or_tutorial_button_active(Button given_button)
        {
            return given_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
        }

        public void Tutorial_button_pressed()
        {
            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel;
            int mid_y_position;
            int right_x_position;
            string text;
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            bool nog_selected = Nog_cbButton.Checked;
            bool kpmp_selected = Kpmp_cbButton.Checked;
            bool dtoxs_selected = Dtoxs_cbButton.Checked;

            bool end_tour = false;
            int tour_box_index = -1;
            bool escape_pressed = false;
            bool back_pressed = false;

            right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;
            while (!end_tour)
            {
                tour_box_index++;
                switch (tour_box_index)
                {
                    case -1:
                        end_tour = true;
                        break;
                    case 0:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + this.Nog_cbButton.Location.Y + (int)Math.Round(0.5 * (this.Nog_cbButton.Height));
                        text = "Using a cannaboid receptor agonist neurite outgrowth was induced in the neuronal cell line N2A, followed by identification of differentially expressed genes (DEGs) and subcellular processes (SCPs) at 4 different time points.";
                        if (!Nog_cbButton.Checked)
                        {
                            Nog_cbButton.SilentChecked = true;
                            NOG_checkBox_CheckedChanged();
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        right_x_position = right_x_position_next_to_overall_panel;
                        if (!Kpmp_cbButton.Checked)
                        {
                            Kpmp_cbButton.SilentChecked = true;
                            KPMPreference_checkBox_CheckedChanged();
                        }
                        mid_y_position = this.Overall_panel.Location.Y + this.Kpmp_cbButton.Location.Y + (int)Math.Round(0.5 * (this.Kpmp_cbButton.Height));
                        text = "Human kidney tissue from unaffected parts of tumor and deceased donor nephrectomies, living donor, normal surveillance transplant and native kidney biopsies was analyzed using single-cell, single-nucleus, and spatial transcriptomics, as well as spatial and near single-cell proteomics.  Cell (sub)types, tissue subsegments and their marker genes, proteins and SCPs were identified to describe kidney physiological functions.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        if (!Dtoxs_cbButton.Checked)
                        {
                            Dtoxs_cbButton.SilentChecked = true;
                            Dtoxs_checkBox_CheckedChanged();
                        }
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + this.Dtoxs_cbButton.Location.Y + (int)Math.Round(0.5 * (this.Dtoxs_cbButton.Height));
                        text = "Healthy iPSC-derived human cardiomyocytes were stimulated with cancer drugs to identify potential DEG and SCP/pathway signatures associated with drug-induced heart failure.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    default:
                        end_tour = true;
                        break;
                }
                if (back_pressed) { tour_box_index = tour_box_index - 2; }
                if ((escape_pressed) || (tour_box_index == -2)) { end_tour = true; }
            }

            if (!Nog_cbButton.Checked != nog_selected)
            {
                Nog_cbButton.SilentChecked = nog_selected;
                NOG_checkBox_CheckedChanged();
            }
            if (Kpmp_cbButton.Checked != kpmp_selected)
            {
                Kpmp_cbButton.SilentChecked = kpmp_selected;
                KPMPreference_checkBox_CheckedChanged();
            }
            if (Dtoxs_cbButton.Checked != dtoxs_selected)
            {
                Dtoxs_cbButton.SilentChecked = dtoxs_selected;
                Dtoxs_checkBox_CheckedChanged();
            }
            UserInterface_tutorial.Set_to_invisible();
        }
        #endregion

    }
}
