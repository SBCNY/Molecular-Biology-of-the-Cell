﻿//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections.Generic;
using System.Linq;
using Windows_forms_customized_tools;
using System.Windows.Forms;
using Common_functions.Global_definitions;
using Data;
using Common_functions.Form_tools;


namespace ClassLibrary1.OrganizeData_userInterface
{
    class OrganizeData_text_strings_class
    {
        public string AddFileNames_string { get { return "Data sources"; } }
        public string AddDatasetOrder_string { get { return "Order #"; } }
    }

    class OrganizeData_userInterface_class
    {
        private char Delimiter_separating_multiple_substrings = ',';

        private MyPanel Overall_panel { get; set; }
        private Label Show_headline_label { get; set; }
        private MyPanel Show_panel { get; set; }
        private MyCheckBox_button Show_name_cbButton { get; set; }
        private Label Show_name_cbLabel { get; set; }
        private MyCheckBox_button Show_color_cbButton { get; set; }
        private Label Show_color_cbLabel { get; set; }
        private MyCheckBox_button Show_timepoint_cbButton { get; set; }
        private Label Show_timepoint_cbLabel { get; set; }
        private MyCheckBox_button Show_entryType_cbButton { get; set; }
        private Label Show_entryType_cbLabel { get; set; }
        private MyCheckBox_button Show_integrationGroup_cbButton { get; set; }
        private Label Show_integrationGroup_cbLabel { get; set; }
        private MyCheckBox_button Show_sourceFile_cbButton { get; set; }
        private Label Show_sourceFile_cbLabel { get; set; }
        private Label Show_sourceFile_label { get; set; }
        private MyCheckBox_button Show_datasetOrderNo_cbButton { get; set; }
        private Label Show_datasetOrderNo_cbLabel { get; set; }
        private Button Show_attributesWithDifferentEntries_button { get; set; }
        private Label Show_attributesWithDifferentEntries_label { get; set; }
        private MyPanel AddFileName_panel { get; set; }
        private Label AddFileName_label { get; set; }
        public OwnListBox AddFileName_listBox { get; private set; }
        private Button AddFileName_before_button { get; set; }
        private Label AddFileName_before_label { get; set; }
        private Button AddFileName_after_button { get; set; }
        private Label AddFileName_after_label { get; set; }
        private Button AddFileName_remove_button { get; set; }
        private Label AddFileName_remove_label { get; set; }
        private MyPanel ConvertTimeunits_panel { get; set; }
        private Label ConvertTimeunits_label { get; set; }
        private OwnListBox ConvertTimeunits_ownListBox { get; set; }
        private Button ConvertTimeunits_button { get; set; }
        private MyPanel Modify_panel { get; set; }
        private Label Modify_headline_label { get; set; }
        private MyPanel Modify_substringOptions_panel { get; set; }
        private MyCheckBox_button Modify_name_cbButton { get; set; }
        private Label Modify_name_cbLabel { get; set; }
        private MyCheckBox_button Modify_timepoint_cbButton { get; set; }
        private Label Modify_timepoint_cbLabel { get; set; }
        private MyCheckBox_button Modify_entryType_cbButton { get; set; }
        private Label Modify_entryType_cbLabel { get; set; }
        private MyCheckBox_button Modify_substring_cbButton { get; set; }
        private Label Modify_substring_cbLabel { get; set; }
        private MyCheckBox_button Modify_sourceFileName_cbButton { get; set; }
        private Label Modify_sourceFileName_cbLabel { get; set; }
        private Label Modify_delimiter_label { get; set; }
        private OwnTextBox Modify_delimiter_ownTextBox { get; set; }
        private Label Modify_indexes_label { get; set; }
        private Label Modify_indexLeft_label { get; set; }
        private OwnTextBox Modify_indexLeft_ownTextBox { get; set; }
        private Label Modify_indexRight_label { get; set; }
        private OwnTextBox Modify_indexRight_ownTextBox { get; set; }
        private List<Dataset_attributes_enum> ModifyCheckBoxes_order_of_activation { get; set; }
        private Button Change_color_button { get; set; }
        private Button Change_delete_button { get; set; }
        private Button Change_integrationGroup_button { get; set; }
        private MyPanel Automatic_panel { get; set; }
        private Label Automatic_headline_label { get; set; }
        private Button Automatic_color_button { get; set; }
        private Button Automatic_integrationGroup_button { get; set; }
        private Button Automatic_datasetOrder_button { get; set; }
        private Label Error_reports_headline_label { get; set; }
        private Label Error_reports_maxErrorPerFile1_label { get; set; }
        private Label Error_reports_maxErrorPerFile2_label { get; set; }
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }

        private Button Explanation_button { get; set; }

        private Form1_default_settings_class Form_default_settings { get; set; }
        private OrganizeData_text_strings_class Organize_data_textStrings { get; set; }

        public OrganizeData_userInterface_class(MyPanel overall_panel,
                                                MyPanel show_panel,
                                                Label show_headline_label,
                                                MyCheckBox_button show_name_cbButton,
                                                Label show_name_cbLabel,
                                                MyCheckBox_button show_color_cbButton,
                                                Label show_color_cbLabel,
                                                MyCheckBox_button show_timepoint_cbButton,
                                                Label show_timepoint_cbLabel,
                                                MyCheckBox_button show_entryType_cbButton,
                                                Label show_entryType_cbLabel,
                                                MyCheckBox_button show_integrationGroup_cbButton,
                                                Label show_integrationGroup_cbLabel,
                                                MyCheckBox_button show_sourceFile_cbButton,
                                                Label show_sourceFile_cbLabel,
                                                Label show_sourceFile_label,
                                                MyCheckBox_button show_datasetOrderNo_cbButton,
                                                Label show_datasetOrderNo_cbLabel,
                                                Button show_attributesWithDifferentEntries_button,
                                                Label show_attributesWithDifferentEntries_label,
                                                MyPanel addFileName_panel,
                                                Label addFileName_label,
                                                OwnListBox addFileName_listBox,
                                                Button addFileName_before_button,
                                                Label addFileName_before_label,
                                                Button addFileName_after_button,
                                                Label addFileName_after_label,
                                                Button addFileName_remove_button,
                                                Label addFileName_remove_label,
                                                MyPanel convertTimeunits_panel,
                                                Label convertTimeunits_label,
                                                OwnListBox convertTimeunits_ownListBox,
                                                Button convertTimeunits_button,
                                                MyPanel modify_panel,
                                                Label modify_headline_label,
                                                MyCheckBox_button modify_name_cbButton,
                                                Label modify_name_cbLabel,
                                                MyCheckBox_button modify_timepoint_cbButton,
                                                Label modify_timepoint_cbLabel,
                                                MyCheckBox_button modify_entryType_cbButton,
                                                Label modify_entryType_cbLabel,
                                                MyCheckBox_button modify_sourceFileName_cbButton,
                                                Label modify_sourceFileName_cbLabel,
                                                MyCheckBox_button modify_substring_cbButton,
                                                Label modify_substring_cbLabel,
                                                MyPanel modify_substringOptions_panel,
                                                Label modify_delimiter_label,
                                                OwnTextBox modify_delimiter_ownTextBox,
                                                Label modify_indexes_label,
                                                Label modify_indexLeft_label,
                                                OwnTextBox modify_indexLeft_ownTextBox,
                                                Label modify_indexRight_label,
                                                OwnTextBox modify_indexRight_ownTextBox,
                                                Button change_integrationGroup_button,
                                                Button change_color_button,
                                                Button change_delete_button,
                                                MyPanel automatic_panel,
                                                Label automatic_headline,
                                                Button integrationGroup_automatic_button,
                                                Button color_automatic_button,
                                                Button datasetOrder_automatic_button,

                                                Label error_reports_headline_label,
                                                Label error_reports_maxErrorPerFile1_label,
                                                Label error_reports_maxErrorPerFile2_label,
                                                OwnTextBox error_reports_ownTextBox,
                                                OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,

                                                Button explanation_button,

                                                Form1_default_settings_class form_default_settings
                                    )
        {
            this.Organize_data_textStrings = new OrganizeData_text_strings_class();
            Form_default_settings = form_default_settings;

            this.Overall_panel = overall_panel;
            this.Show_panel = show_panel;
            this.Show_name_cbButton = show_name_cbButton;
            this.Show_name_cbLabel = show_name_cbLabel;
            this.Show_entryType_cbButton = show_entryType_cbButton;
            this.Show_entryType_cbLabel = show_entryType_cbLabel;
            this.Show_timepoint_cbButton = show_timepoint_cbButton;
            this.Show_timepoint_cbLabel = show_timepoint_cbLabel;
            this.Show_integrationGroup_cbButton = show_integrationGroup_cbButton;
            this.Show_integrationGroup_cbLabel = show_integrationGroup_cbLabel;
            this.Show_color_cbButton = show_color_cbButton;
            this.Show_color_cbLabel = show_color_cbLabel;
            this.Show_sourceFile_cbButton = show_sourceFile_cbButton;
            this.Show_sourceFile_cbLabel = show_sourceFile_cbLabel;
            this.Show_attributesWithDifferentEntries_button = show_attributesWithDifferentEntries_button;
            this.Show_datasetOrderNo_cbButton = show_datasetOrderNo_cbButton;
            this.Show_datasetOrderNo_cbLabel = show_datasetOrderNo_cbLabel;
            this.Show_sourceFile_label = show_sourceFile_label;
            this.Show_attributesWithDifferentEntries_label = show_attributesWithDifferentEntries_label;
            this.Show_headline_label = show_headline_label;
            this.AddFileName_panel = addFileName_panel;
            this.AddFileName_listBox = addFileName_listBox;
            this.AddFileName_before_button = addFileName_before_button;
            this.AddFileName_before_label = addFileName_before_label;
            this.AddFileName_after_button = addFileName_after_button;
            this.AddFileName_after_label = addFileName_after_label;
            this.AddFileName_label = addFileName_label;
            this.AddFileName_remove_button = addFileName_remove_button;
            this.AddFileName_remove_label = addFileName_remove_label;
            this.ConvertTimeunits_panel = convertTimeunits_panel;
            this.ConvertTimeunits_button = convertTimeunits_button;
            this.ConvertTimeunits_ownListBox = convertTimeunits_ownListBox;
            this.ConvertTimeunits_label = convertTimeunits_label;
            this.Modify_panel = modify_panel;
            this.Change_integrationGroup_button = change_integrationGroup_button;
            this.Change_color_button = change_color_button;
            this.Change_delete_button = change_delete_button;
            this.Modify_name_cbButton = modify_name_cbButton;
            this.Modify_name_cbLabel = modify_name_cbLabel;
            this.Modify_entryType_cbButton = modify_entryType_cbButton;
            this.Modify_entryType_cbLabel = modify_entryType_cbLabel;
            this.Modify_timepoint_cbButton = modify_timepoint_cbButton;
            this.Modify_timepoint_cbLabel = modify_timepoint_cbLabel;
            this.Modify_sourceFileName_cbButton = modify_sourceFileName_cbButton;
            this.Modify_sourceFileName_cbLabel = modify_sourceFileName_cbLabel;
            this.Modify_substring_cbButton = modify_substring_cbButton;
            this.Modify_substring_cbLabel = modify_substring_cbLabel;
            this.Modify_substringOptions_panel = modify_substringOptions_panel;
            this.Modify_delimiter_ownTextBox = modify_delimiter_ownTextBox;
            this.Modify_indexRight_ownTextBox = modify_indexRight_ownTextBox;
            this.Modify_indexLeft_ownTextBox = modify_indexLeft_ownTextBox;
            this.Modify_delimiter_label = modify_delimiter_label;
            this.Modify_indexes_label = modify_indexes_label;
            this.Modify_indexLeft_label = modify_indexLeft_label;
            this.Modify_indexRight_label = modify_indexRight_label;
            this.Automatic_color_button = color_automatic_button;
            this.Automatic_integrationGroup_button = integrationGroup_automatic_button;
            this.Automatic_datasetOrder_button = datasetOrder_automatic_button;
            this.Modify_headline_label = modify_headline_label;
            this.Automatic_panel = automatic_panel;
            this.Automatic_headline_label = automatic_headline;
            this.AddFileName_panel = addFileName_panel;

            Error_reports_headline_label = error_reports_headline_label;
            Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;
            Error_reports_ownTextBox = error_reports_ownTextBox;
            Explanation_button = explanation_button;

            Initialize_and_set_to_default();

            Update_all_graphic_elements();
        }

        public void Update_all_graphic_elements()
        { 
            this.Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            MyPanel my_panel;
            Label my_label;
            OwnTextBox my_textBox;
            OwnListBox my_listBox;
            Button my_button;

            int shared_leftReferenceBorder_of_allCheckBoxes = (int)Math.Round(0.02F * Overall_panel.Width);

            #region Show panel
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = (int)Math.Round(0.31*this.Overall_panel.Height);
            my_panel = this.Show_panel;
            this.Show_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            int showPanel_headline_lowerBorder = (int)Math.Round(0.13F * this.Show_panel.Height);

            #region Show panel checkboxes, buttons, button- and explanation labels
            Dictionary<MyCheckBox_button, Label> cbButton_label_dict = new Dictionary<MyCheckBox_button, Label>();
            cbButton_label_dict.Add(Show_name_cbButton, Show_name_cbLabel);
            cbButton_label_dict.Add(Show_entryType_cbButton, Show_entryType_cbLabel);
            cbButton_label_dict.Add(Show_timepoint_cbButton, Show_timepoint_cbLabel);
            cbButton_label_dict.Add(Show_integrationGroup_cbButton, Show_integrationGroup_cbLabel);
            cbButton_label_dict.Add(Show_color_cbButton, Show_color_cbLabel);
            cbButton_label_dict.Add(Show_sourceFile_cbButton, Show_sourceFile_cbLabel);
            MyCheckBox_button[] my_left_checkBox_buttons = cbButton_label_dict.Keys.ToArray();
            MyCheckBox_button my_left_checkBox_button;
            int my_left_checkBox_buttons_length = my_left_checkBox_buttons.Length;
            int shared_heightWidth_of_all_show_checkBoxButtons = (int)Math.Round((0.8F/(float)my_left_checkBox_buttons_length) * (float)this.Show_panel.Height);
            int start_location_y_show_checkbox = showPanel_headline_lowerBorder;
            int shared_rightReferenceBorder_of_showLeftCheckBoxes = (int)Math.Round(0.5F * this.Show_panel.Width);
            top_referenceBorder = start_location_y_show_checkbox - shared_heightWidth_of_all_show_checkBoxButtons;
            for (int indexMYLeft=0; indexMYLeft<my_left_checkBox_buttons_length;indexMYLeft++)
            {
                left_referenceBorder = shared_leftReferenceBorder_of_allCheckBoxes;
                right_referenceBorder = left_referenceBorder + shared_heightWidth_of_all_show_checkBoxButtons;
                top_referenceBorder += shared_heightWidth_of_all_show_checkBoxButtons;
                bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_all_show_checkBoxButtons;
                my_left_checkBox_button = my_left_checkBox_buttons[indexMYLeft];
                Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_left_checkBox_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                left_referenceBorder = my_left_checkBox_button.Location.X + my_left_checkBox_button.Width;
                right_referenceBorder = shared_rightReferenceBorder_of_showLeftCheckBoxes;
                my_label = cbButton_label_dict[my_left_checkBox_button];
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            left_referenceBorder = shared_rightReferenceBorder_of_showLeftCheckBoxes;

            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_all_show_checkBoxButtons;
            top_referenceBorder = start_location_y_show_checkbox + shared_heightWidth_of_all_show_checkBoxButtons;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_all_show_checkBoxButtons;
            my_button = Show_attributesWithDifferentEntries_button;
            Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Show_attributesWithDifferentEntries_button.Location.X + Show_attributesWithDifferentEntries_button.Width;
            top_referenceBorder = Math.Max(start_location_y_show_checkbox, Show_attributesWithDifferentEntries_button.Location.Y - shared_heightWidth_of_all_show_checkBoxButtons);
            bottom_referenceBorder = Show_attributesWithDifferentEntries_button.Location.Y + Show_attributesWithDifferentEntries_button.Height + shared_heightWidth_of_all_show_checkBoxButtons;
            right_referenceBorder = this.Show_panel.Width;
            my_label = Show_attributesWithDifferentEntries_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_rightReferenceBorder_of_showLeftCheckBoxes;
            top_referenceBorder = start_location_y_show_checkbox + 4 * shared_heightWidth_of_all_show_checkBoxButtons;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_all_show_checkBoxButtons;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_all_show_checkBoxButtons;
            my_left_checkBox_button = Show_datasetOrderNo_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_left_checkBox_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Show_datasetOrderNo_cbButton.Location.X + Show_datasetOrderNo_cbButton.Width;
            top_referenceBorder = Math.Max(Show_attributesWithDifferentEntries_label.Location.Y+Show_attributesWithDifferentEntries_label.Height, Show_datasetOrderNo_cbButton.Location.Y - shared_heightWidth_of_all_show_checkBoxButtons);
            bottom_referenceBorder = Math.Min(Show_panel.Height,Show_datasetOrderNo_cbButton.Location.Y + Show_datasetOrderNo_cbButton.Height + shared_heightWidth_of_all_show_checkBoxButtons);
            right_referenceBorder = this.Show_panel.Width;
            my_label = Show_datasetOrderNo_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Show_sourceFile_cbLabel.Location.X + this.Show_sourceFile_cbLabel.Width;
            right_referenceBorder = this.Show_datasetOrderNo_cbButton.Location.X;
            top_referenceBorder = this.Show_sourceFile_cbLabel.Location.Y;
            bottom_referenceBorder = this.Show_sourceFile_cbLabel.Location.Y + this.Show_sourceFile_cbLabel.Height;
            my_label = this.Show_sourceFile_label;
            this.Show_sourceFile_label = Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Show panel headline label
            left_referenceBorder = 0;
            right_referenceBorder = this.Show_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = showPanel_headline_lowerBorder;
            my_label = this.Show_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_attachTo_leftSide_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Add file names panel, buttons and labels
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = this.Show_panel.Location.Y + this.Show_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.46 * this.Overall_panel.Height);
            my_panel = this.AddFileName_panel;
            this.AddFileName_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(AddFileName_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder); ;

            int size_of_each_row_in_addFileName_panel = (int)Math.Round(0.333F * this.AddFileName_panel.Height);
            int half_size_of_each_row_in_addFileName_panel = (int)Math.Round(0.5F * size_of_each_row_in_addFileName_panel);
            int relative_center_of_each_row_in_addFileName_panel = (int)Math.Round(0.5F * size_of_each_row_in_addFileName_panel);
            int half_button_size_in_addFileName_panel = (int)Math.Round(0.4F * size_of_each_row_in_addFileName_panel);

            int left_referenceBorder_addFileName_buttons = (int)Math.Round(0.45F * AddFileName_panel.Width);

            int current_rowCenter = 0 - half_size_of_each_row_in_addFileName_panel;
            current_rowCenter += size_of_each_row_in_addFileName_panel;
            top_referenceBorder = current_rowCenter - half_button_size_in_addFileName_panel;
            bottom_referenceBorder = current_rowCenter + half_button_size_in_addFileName_panel;
            left_referenceBorder = left_referenceBorder_addFileName_buttons;
            right_referenceBorder = left_referenceBorder + (bottom_referenceBorder - top_referenceBorder);
            my_button = this.AddFileName_before_button;
            Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.AddFileName_before_button.Location.X + this.AddFileName_before_button.Width;
            right_referenceBorder = this.AddFileName_panel.Width;
            top_referenceBorder = current_rowCenter - half_size_of_each_row_in_addFileName_panel;
            bottom_referenceBorder = current_rowCenter + half_size_of_each_row_in_addFileName_panel;
            my_label = this.AddFileName_before_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            current_rowCenter += size_of_each_row_in_addFileName_panel;
            top_referenceBorder = current_rowCenter - half_button_size_in_addFileName_panel;
            bottom_referenceBorder = current_rowCenter + half_button_size_in_addFileName_panel;
            left_referenceBorder = left_referenceBorder_addFileName_buttons;
            right_referenceBorder = left_referenceBorder + (bottom_referenceBorder - top_referenceBorder);
            my_button = this.AddFileName_after_button;
            Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.AddFileName_after_button.Location.X + this.AddFileName_after_button.Width;
            right_referenceBorder = this.AddFileName_panel.Width;
            top_referenceBorder = current_rowCenter - half_size_of_each_row_in_addFileName_panel;
            bottom_referenceBorder = current_rowCenter + half_size_of_each_row_in_addFileName_panel;
            my_label = this.AddFileName_after_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int middle_between_both_buttons = size_of_each_row_in_addFileName_panel;
            left_referenceBorder = (int)Math.Round(0.13F* this.AddFileName_panel.Width);
            right_referenceBorder = this.AddFileName_before_button.Location.X;
            top_referenceBorder = middle_between_both_buttons - half_size_of_each_row_in_addFileName_panel;
            bottom_referenceBorder = middle_between_both_buttons + half_size_of_each_row_in_addFileName_panel;
            my_listBox = this.AddFileName_listBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);


            left_referenceBorder = 0;
            right_referenceBorder = this.AddFileName_listBox.Location.X;
            top_referenceBorder = middle_between_both_buttons - half_size_of_each_row_in_addFileName_panel;
            bottom_referenceBorder = middle_between_both_buttons + half_size_of_each_row_in_addFileName_panel;
            my_label = this.AddFileName_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int distance_from_rightEdge = this.AddFileName_panel.Width - this.AddFileName_before_button.Location.X - this.AddFileName_before_button.Width;

            current_rowCenter += size_of_each_row_in_addFileName_panel;
            top_referenceBorder = current_rowCenter - half_button_size_in_addFileName_panel;
            bottom_referenceBorder = current_rowCenter + half_button_size_in_addFileName_panel;
            left_referenceBorder = AddFileName_label.Location.X;
            right_referenceBorder = left_referenceBorder + (bottom_referenceBorder - top_referenceBorder);
            my_button = this.AddFileName_remove_button;
            Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Update_removeFileName_label();
            #endregion

            #region Convert timeunits panel, button, listbox and label
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = this.AddFileName_panel.Location.Y + this.AddFileName_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.52 * this.Overall_panel.Height);
            my_panel = this.ConvertTimeunits_panel;
            this.ConvertTimeunits_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = (int)Math.Round(0.05F*this.ConvertTimeunits_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.95F * this.ConvertTimeunits_panel.Height);

            left_referenceBorder = (int)Math.Round(0.76F*this.ConvertTimeunits_panel.Width);
            right_referenceBorder = this.ConvertTimeunits_panel.Width - shared_leftReferenceBorder_of_allCheckBoxes;
            my_button = ConvertTimeunits_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.43F * this.ConvertTimeunits_panel.Width);
            right_referenceBorder = this.ConvertTimeunits_button.Location.X;
            my_listBox = ConvertTimeunits_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder,right_referenceBorder,top_referenceBorder,bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.ConvertTimeunits_ownListBox.Location.X;
            top_referenceBorder = ConvertTimeunits_ownListBox.Location.Y;
            bottom_referenceBorder = ConvertTimeunits_ownListBox.Location.Y + ConvertTimeunits_ownListBox.Height;
            my_label = this.ConvertTimeunits_label;
            this.ConvertTimeunits_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Modify panel
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = this.ConvertTimeunits_panel.Location.Y + this.ConvertTimeunits_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.86 * this.Overall_panel.Height);
            my_panel = Modify_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Modify panel buttons
            Button[] modifyPanel_buttons = new Button[] { this.Change_integrationGroup_button, this.Change_color_button, this.Change_delete_button };
            Button modifyPanel_button;
            int modifyPanel_buttons_length = modifyPanel_buttons.Length;
            int distance_for_buttons_from_leftRightSide_of_modifyPanel = (int)Math.Round(0.05F * this.Modify_panel.Width);
            int shared_width_of_all_buttons = (int)Math.Round((float)(this.Modify_panel.Width-2* distance_for_buttons_from_leftRightSide_of_modifyPanel) /(float)modifyPanel_buttons_length);
            top_referenceBorder = (int)Math.Round(0.7F * this.Modify_panel.Height);
            bottom_referenceBorder = this.Modify_panel.Height;
            right_referenceBorder = distance_for_buttons_from_leftRightSide_of_modifyPanel;
            for (int indexModifyPanelButton=0; indexModifyPanelButton < modifyPanel_buttons_length; indexModifyPanelButton++)
            {
                modifyPanel_button = modifyPanel_buttons[indexModifyPanelButton];
                left_referenceBorder = right_referenceBorder;
                right_referenceBorder = left_referenceBorder + shared_width_of_all_buttons;
                my_button = modifyPanel_button;
                Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
            #endregion

            int modifyPanel_headline_lowerBorder = (int)Math.Round(0.12F * Modify_panel.Height);

            #region Modify panel check boxes

            int shared_heightWidth_of_all_modify_checkBoxButtons = shared_heightWidth_of_all_show_checkBoxButtons;

            int shared_leftReferenceBorder_of_leftCheckBoxes = shared_leftReferenceBorder_of_allCheckBoxes;
            int shared_rightReferenceBorder_of_rightCheckBoxes = (int)Math.Round(0.45F * Modify_panel.Width);
            int shared_locationY_of_top_checkBoxes = modifyPanel_headline_lowerBorder;
            bottom_referenceBorder = shared_locationY_of_top_checkBoxes;
            Dictionary<MyCheckBox_button, Label> modify_lef_cbButtons_label_dict = new Dictionary<MyCheckBox_button, Label>();
            modify_lef_cbButtons_label_dict.Add(Modify_name_cbButton, Modify_name_cbLabel);
            modify_lef_cbButtons_label_dict.Add(Modify_entryType_cbButton, Modify_entryType_cbLabel);
            modify_lef_cbButtons_label_dict.Add(Modify_timepoint_cbButton, Modify_timepoint_cbLabel);
            modify_lef_cbButtons_label_dict.Add(Modify_sourceFileName_cbButton, Modify_sourceFileName_cbLabel);
            MyCheckBox_button[] left_modify_buttons = modify_lef_cbButtons_label_dict.Keys.ToArray();
            MyCheckBox_button modify_button;
            int left_modify_buttons_length = left_modify_buttons.Length;
            for (int indexLeft=0; indexLeft<left_modify_buttons_length; indexLeft++)
            {
                left_referenceBorder = shared_leftReferenceBorder_of_leftCheckBoxes;
                right_referenceBorder = left_referenceBorder + shared_heightWidth_of_all_modify_checkBoxButtons;
                top_referenceBorder = bottom_referenceBorder;
                bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_all_modify_checkBoxButtons;
                my_left_checkBox_button = left_modify_buttons[indexLeft];
                Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_left_checkBox_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                left_referenceBorder = my_left_checkBox_button.Location.X + my_left_checkBox_button.Width;
                right_referenceBorder = shared_rightReferenceBorder_of_rightCheckBoxes;
                my_label = modify_lef_cbButtons_label_dict[my_left_checkBox_button];
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            left_referenceBorder = shared_rightReferenceBorder_of_rightCheckBoxes;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_all_modify_checkBoxButtons;
            top_referenceBorder = shared_locationY_of_top_checkBoxes;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_all_modify_checkBoxButtons;
            modify_button = this.Modify_substring_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(modify_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Modify_substring_cbButton.Location.X + Modify_substring_cbButton.Width;
            right_referenceBorder = this.Modify_panel.Width;
            my_label = this.Modify_substring_cbLabel; ;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Modify substringOptions panel, textBoxes and labels

            left_referenceBorder = shared_rightReferenceBorder_of_rightCheckBoxes;
            right_referenceBorder = this.Modify_panel.Width;
            top_referenceBorder = this.Modify_substring_cbButton.Location.Y + this.Modify_substring_cbButton.Height;
            bottom_referenceBorder = Change_integrationGroup_button.Location.Y;
            my_panel = Modify_substringOptions_panel;
            this.Modify_substringOptions_panel = Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int shared_rightSide_substring_textBox_reference_border = this.Modify_substringOptions_panel.Width;
            int shared_size_substringPanel_textBoxes = (int)Math.Round(0.15F * this.Modify_substringOptions_panel.Width);
            int height_of_each_row_in_substringPanel = (int)Math.Round(0.3333F * this.Modify_substringOptions_panel.Height);

            bottom_referenceBorder = 0;

            right_referenceBorder = shared_rightSide_substring_textBox_reference_border;
            left_referenceBorder = right_referenceBorder - shared_size_substringPanel_textBoxes;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + height_of_each_row_in_substringPanel;
            my_textBox = this.Modify_delimiter_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            bottom_referenceBorder += height_of_each_row_in_substringPanel; //Substring no(s) label

            right_referenceBorder = shared_rightSide_substring_textBox_reference_border;
            left_referenceBorder = right_referenceBorder - shared_size_substringPanel_textBoxes;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + height_of_each_row_in_substringPanel;
            my_textBox = this.Modify_indexRight_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.3F*this.Modify_substringOptions_panel.Width);
            right_referenceBorder = left_referenceBorder + shared_size_substringPanel_textBoxes;
            my_textBox = this.Modify_indexLeft_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = Modify_delimiter_ownTextBox.Location.X;
            top_referenceBorder = this.Modify_delimiter_ownTextBox.Location.Y;
            bottom_referenceBorder = this.Modify_delimiter_ownTextBox.Location.Y + Modify_delimiter_ownTextBox.Height;
            my_label = this.Modify_delimiter_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = Modify_substringOptions_panel.Width;
            top_referenceBorder = this.Modify_delimiter_ownTextBox.Location.Y + Modify_delimiter_ownTextBox.Height;
            bottom_referenceBorder = this.Modify_indexLeft_ownTextBox.Location.Y;
            my_label = this.Modify_indexes_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.Modify_indexLeft_ownTextBox.Location.X;
            top_referenceBorder = this.Modify_indexLeft_ownTextBox.Location.Y;
            bottom_referenceBorder = this.Modify_indexLeft_ownTextBox.Location.Y + this.Modify_indexLeft_ownTextBox.Height;
            my_label = this.Modify_indexLeft_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Modify_indexLeft_ownTextBox.Location.X + this.Modify_indexLeft_ownTextBox.Width;
            right_referenceBorder = this.Modify_indexRight_ownTextBox.Location.X;
            top_referenceBorder = this.Modify_indexRight_ownTextBox.Location.Y;
            bottom_referenceBorder = this.Modify_indexRight_ownTextBox.Location.Y + this.Modify_indexLeft_ownTextBox.Height;
            my_label = this.Modify_indexRight_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Modify panel headline
            left_referenceBorder = 0;
            right_referenceBorder = this.Modify_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = modifyPanel_headline_lowerBorder;
            my_label = this.Modify_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_attachTo_leftSide_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Automatic panel, buttons and headline
            left_referenceBorder = 0;
            right_referenceBorder = (int)Math.Round(0.7F*this.Overall_panel.Width);
            top_referenceBorder = this.Modify_panel.Location.Y + this.Modify_panel.Height;
            bottom_referenceBorder = this.Overall_panel.Height;
            my_panel = Automatic_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            shared_width_of_all_buttons = (int)Math.Round(0.333F * (this.Automatic_panel.Width - 0.04F*this.Automatic_panel.Width)); ;

            Button[] automatic_buttons = new Button[] { this.Automatic_integrationGroup_button, this.Automatic_color_button, this.Automatic_datasetOrder_button };
            Button automatic_button;
            top_referenceBorder = (int)Math.Round(0.15F * this.Modify_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.98F*(float)this.Automatic_panel.Height);
            right_referenceBorder = (int)Math.Round(0.02F * this.Automatic_panel.Width);
            for (int indexAutomaticPanelButton = 0; indexAutomaticPanelButton < modifyPanel_buttons_length; indexAutomaticPanelButton++)
            {
                automatic_button = automatic_buttons[indexAutomaticPanelButton];
                left_referenceBorder = right_referenceBorder;
                right_referenceBorder = left_referenceBorder + shared_width_of_all_buttons;
                my_button = automatic_button;
                Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            left_referenceBorder = 0;
            right_referenceBorder = this.Automatic_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Automatic_datasetOrder_button.Location.Y;
            my_label = this.Automatic_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            left_referenceBorder = Automatic_panel.Location.X + Automatic_panel.Width + (int)Math.Round(0.02F * this.Overall_panel.Width);
            right_referenceBorder = this.Overall_panel.Width - (int)Math.Round(0.02F * this.Overall_panel.Width);
            top_referenceBorder = Automatic_panel.Location.Y + (int)Math.Round(0.2F * Automatic_panel.Height);
            bottom_referenceBorder = Automatic_panel.Location.Y + (int)Math.Round(0.8F * Automatic_panel.Height);
            my_button = Explanation_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

        }

        public void Update_removeFileName_label()
        {
            int one_third_of_height = (int)Math.Round(0.3333F * this.AddFileName_panel.Height);
            string curent_selection = this.AddFileName_listBox.SelectedItem.ToString();
            int left_referenceBorder = this.AddFileName_remove_button.Location.X + this.AddFileName_remove_button.Width; ;
            int right_referenceBorder = Math.Max(this.AddFileName_after_label.Location.X+this.AddFileName_after_label.Width, this.AddFileName_before_label.Location.X + this.AddFileName_before_label.Width);
            int top_referenceBorder = this.AddFileName_panel.Height - one_third_of_height;
            int bottom_referenceBorder = this.AddFileName_panel.Height;
            Label my_label = this.AddFileName_remove_label;
            my_label.Text = "Remove " + curent_selection + " from dataset names";
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
        }

        private void Initialize_and_set_to_default()
        {
            this.Show_color_cbButton.SilentChecked = true;
            this.Show_name_cbButton.SilentChecked = true;

            ConvertTimeunits_ownListBox.ClearSelected();
            var all_timeunits = Enum.GetValues(typeof(Timeunit_enum));
            foreach (var timeunit in all_timeunits)
            {
                if (!timeunit.Equals(Timeunit_enum.E_m_p_t_y))
                {
                    ConvertTimeunits_ownListBox.Items.Add(timeunit);
                }
            }
            ConvertTimeunits_ownListBox.SilentSelectedIndex = ConvertTimeunits_ownListBox.Items.IndexOf(Timeunit_enum.min);

            this.ModifyCheckBoxes_order_of_activation = new List<Dataset_attributes_enum>();

            this.AddFileName_listBox.Items.Clear();
            this.AddFileName_listBox.Items.Add(Organize_data_textStrings.AddFileNames_string);
            this.AddFileName_listBox.Items.Add(Organize_data_textStrings.AddDatasetOrder_string);
            this.AddFileName_listBox.SilentSelectedIndex = this.AddFileName_listBox.Items.IndexOf(Organize_data_textStrings.AddFileNames_string);
            Update_modify_panel();
        }

        public void Set_to_visible(Dataset_attributes_enum[] visible_dataset_panels)
        {
            Set_showCheckBoxes_based_on_dataset_attributes(visible_dataset_panels);
            ConvertTimeunits_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            ConvertTimeunits_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Change_color_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Change_color_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Change_integrationGroup_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Change_integrationGroup_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Change_delete_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Change_delete_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Automatic_color_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Automatic_color_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Automatic_integrationGroup_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Automatic_integrationGroup_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Automatic_datasetOrder_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Automatic_datasetOrder_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Show_attributesWithDifferentEntries_button.BackColor = Form_default_settings.Color_secondaryButton_notPressed_back;
            Show_attributesWithDifferentEntries_button.BackColor = Form_default_settings.Color_secondaryButton_notPressed_back;
            AddFileName_after_button.BackColor = Form_default_settings.Color_secondaryButton_notPressed_back;
            AddFileName_after_button.ForeColor = Form_default_settings.Color_secondaryButton_notPressed_fore;
            AddFileName_before_button.BackColor = Form_default_settings.Color_secondaryButton_notPressed_back;
            AddFileName_before_button.ForeColor = Form_default_settings.Color_secondaryButton_notPressed_fore;
            AddFileName_remove_button.BackColor = Form_default_settings.Color_secondaryButton_notPressed_back;
            AddFileName_remove_button.ForeColor = Form_default_settings.Color_secondaryButton_notPressed_fore;
            //AddFileName_after_button.FlatAppearance.BorderColor = Form_default_settings.Color_secondaryButton_notPressed_fore;
            //AddFileName_before_button.FlatAppearance.BorderColor = Form_default_settings.Color_secondaryButton_notPressed_fore; 
        }
        #region Show data
        public Dataset_attributes_enum[] Get_dataset_attributes_from_showCheckBoxes()
        {
            List<Dataset_attributes_enum> set_to_visible = new List<Dataset_attributes_enum>();
            if (Show_name_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.Name); }
            if (Show_color_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.Color); }
            if (Show_timepoint_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.Timepoint); }
            if (Show_entryType_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.EntryType); }
            if (Show_integrationGroup_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.IntegrationGroup); }
            if (Show_sourceFile_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.SourceFile); }
            if (Show_datasetOrderNo_cbButton.Checked) { set_to_visible.Add(Dataset_attributes_enum.Dataset_order_no); }
            set_to_visible.Add(Dataset_attributes_enum.Delete);
            return set_to_visible.ToArray();
        }

        public void Set_showCheckBoxes_based_on_dataset_attributes(Dataset_attributes_enum[] visible_dataset_attributes)
        {
            Show_name_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.Name);
            Show_timepoint_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.Timepoint);
            Show_color_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.Color);
            Show_entryType_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.EntryType);
            Show_integrationGroup_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.IntegrationGroup);
            Show_sourceFile_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.SourceFile);
            Show_datasetOrderNo_cbButton.SilentChecked = visible_dataset_attributes.Contains(Dataset_attributes_enum.Dataset_order_no);
        }

        public Dataset_attributes_enum[] Show_name_ownCheckBox_CheckedChanged()
        {
            if (!Changes_allowed()) //If in filter mode, reverse user action
            {
                Show_name_cbButton.SilentChecked = !Show_name_cbButton.Checked;
            }
            return Get_dataset_attributes_from_showCheckBoxes();
        }

        public Dataset_attributes_enum[] Show_timepoint_ownCheckBox_CheckedChanged()
        {
            if (!Changes_allowed()) //If in filter mode, reverse user action
            {
                Show_timepoint_cbButton.SilentChecked = !Show_timepoint_cbButton.Checked;
            }
            return Get_dataset_attributes_from_showCheckBoxes();
        }

        public Dataset_attributes_enum[] Show_entryType_ownCheckBox_CheckedChanged()
        {
            if (!Changes_allowed()) //If in filter mode, reverse user action
            {
                Show_entryType_cbButton.SilentChecked = !Show_entryType_cbButton.Checked;
            }
            return Get_dataset_attributes_from_showCheckBoxes();
        }

        public Dataset_attributes_enum[] Show_color_ownCheckBox_CheckedChanged()
        {
            if (!Changes_allowed()) //If in filter mode, reverse user action
            {
                Show_color_cbButton.SilentChecked = !Show_color_cbButton.Checked;
            }
            return Get_dataset_attributes_from_showCheckBoxes();
        }

        public Dataset_attributes_enum[] Show_integrationGroup_ownCheckBox_CheckedChanged()
        {
            if (!Changes_allowed()) //If in filter mode, reverse user action
            {
                Show_integrationGroup_cbButton.SilentChecked = !Show_integrationGroup_cbButton.Checked;
            }
            return Get_dataset_attributes_from_showCheckBoxes();
        }

        public Dataset_attributes_enum[] Show_sourceFile_ownCheckBox_CheckedChanged()
        {
            if (!Changes_allowed()) //If in filter mode, reverse user action
            {
                Show_sourceFile_cbButton.SilentChecked = !Show_sourceFile_cbButton.Checked;
            }
            return Get_dataset_attributes_from_showCheckBoxes();
        }
        #endregion

        public bool ShowDifferingEntities_in_dataset_button_pressed_and_return_if_changes_allowed(Custom_data_class custom_data, out Dataset_attributes_enum[] differing_attributes)
        {
            bool changes_allowed = Changes_allowed();
            differing_attributes = new Dataset_attributes_enum[0];
            if (changes_allowed)
            {
                differing_attributes = custom_data.Get_all_attributes_with_different_entries();
                this.Show_name_cbButton.SilentChecked = differing_attributes.Contains(Dataset_attributes_enum.Name);
                this.Show_entryType_cbButton.SilentChecked = differing_attributes.Contains(Dataset_attributes_enum.EntryType);
                this.Show_timepoint_cbButton.SilentChecked = differing_attributes.Contains(Dataset_attributes_enum.Timepoint);
                this.Show_integrationGroup_cbButton.SilentChecked = differing_attributes.Contains(Dataset_attributes_enum.IntegrationGroup);
                this.Show_color_cbButton.SilentChecked = differing_attributes.Contains(Dataset_attributes_enum.Color);
                this.Show_datasetOrderNo_cbButton.SilentChecked = differing_attributes.Contains(Dataset_attributes_enum.Dataset_order_no);
            }
            return changes_allowed;
        }

        public Custom_data_class ConvertTimeunites_convert_button_Click(Custom_data_class custom_data)
        {
            Timeunit_enum new_timepoint = (Timeunit_enum)this.ConvertTimeunits_ownListBox.SelectedItem;
            custom_data.Convert_all_timeunits_to_input_unit(new_timepoint);
            return custom_data;
        }

        private List<Dataset_attributes_enum> Update_checkBoxes_order_of_activation(List<Dataset_attributes_enum> checkBoxes_order_of_activation, Dataset_attributes_enum checkBox_pressed)
        {
            Dataset_attributes_enum remove_attribute = Dataset_attributes_enum.E_m_p_t_y;
            if (checkBox_pressed.Equals(Dataset_attributes_enum.Substring)) { remove_attribute = Dataset_attributes_enum.Name; }
            if (checkBox_pressed.Equals(Dataset_attributes_enum.Name)) { remove_attribute = Dataset_attributes_enum.Substring; }

            Dataset_attributes_enum current_datasetCheckBox;
            List<Dataset_attributes_enum> new_checkBoxes_order = new List<Dataset_attributes_enum>();
            int checkedBoxes_order_count;
            if (!checkBoxes_order_of_activation.Contains(checkBox_pressed))
            {
                #region Remove remove_attribute
                List<Dataset_attributes_enum> checkBoxes_after_removal = new List<Dataset_attributes_enum>();
                checkedBoxes_order_count = checkBoxes_order_of_activation.Count;
                for (int indexCB = 0; indexCB < checkedBoxes_order_count; indexCB++)
                {
                    current_datasetCheckBox = checkBoxes_order_of_activation[indexCB];
                    if (!current_datasetCheckBox.Equals(remove_attribute))
                    {
                        checkBoxes_after_removal.Add(current_datasetCheckBox);
                    }
                }
                checkBoxes_order_of_activation = checkBoxes_after_removal;
                #endregion

                #region Remove first attribute, if more than 5 attributes
                checkBoxes_order_of_activation.Add(checkBox_pressed);
                checkedBoxes_order_count = checkBoxes_order_of_activation.Count;
                if (checkBoxes_order_of_activation.Count>5) { throw new Exception(); }
                else if (checkedBoxes_order_count==5)
                {
                    for (int indexCB=1;indexCB<checkedBoxes_order_count;indexCB++)
                    {
                        current_datasetCheckBox = checkBoxes_order_of_activation[indexCB];
                        new_checkBoxes_order.Add(current_datasetCheckBox);
                    }
                }
                else { new_checkBoxes_order = checkBoxes_order_of_activation; }
                #endregion
            }
            else
            {
                #region Remove checkBox pressed, if part of attributes
                checkedBoxes_order_count = checkBoxes_order_of_activation.Count;
                for (int indexCB=0; indexCB<checkedBoxes_order_count;indexCB++)
                {
                    current_datasetCheckBox = checkBoxes_order_of_activation[indexCB];
                    if (!current_datasetCheckBox.Equals(checkBox_pressed))
                    {
                        new_checkBoxes_order.Add(current_datasetCheckBox);
                    }
                }
                #endregion
            }
            return new_checkBoxes_order;
        }

        public bool Changes_allowed(params Dataset_attributes_enum[] attributes_to_be_ignored)
        {
            bool changes_allowed = true;
            if (((changes_allowed))&&(!attributes_to_be_ignored.Contains(Dataset_attributes_enum.Color)))
            { 
                changes_allowed = !Change_color_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back); 
            }
            if (((changes_allowed)) && (!attributes_to_be_ignored.Contains(Dataset_attributes_enum.IntegrationGroup)))
            {
                changes_allowed = !Change_integrationGroup_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
            }
            if (((changes_allowed)) && (!attributes_to_be_ignored.Contains(Dataset_attributes_enum.Delete)))
            {
                changes_allowed = !Change_delete_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
            }
            return changes_allowed;
        }

        private void Update_modify_panel()
        {
            Modify_name_cbButton.SilentChecked = ModifyCheckBoxes_order_of_activation.Contains(Dataset_attributes_enum.Name);
            Modify_timepoint_cbButton.SilentChecked = ModifyCheckBoxes_order_of_activation.Contains(Dataset_attributes_enum.Timepoint);
            Modify_entryType_cbButton.SilentChecked = ModifyCheckBoxes_order_of_activation.Contains(Dataset_attributes_enum.EntryType);
            bool attributes_contain_substring = ModifyCheckBoxes_order_of_activation.Contains(Dataset_attributes_enum.Substring);
            Modify_substring_cbButton.SilentChecked = attributes_contain_substring;
            Modify_sourceFileName_cbButton.SilentChecked = ModifyCheckBoxes_order_of_activation.Contains(Dataset_attributes_enum.SourceFile);
            Modify_substringOptions_panel.Visible = attributes_contain_substring;
        }

        public void Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum checkBox_pressed)
        {
            if (Changes_allowed())
            {
                ModifyCheckBoxes_order_of_activation = Update_checkBoxes_order_of_activation(ModifyCheckBoxes_order_of_activation, checkBox_pressed);
            }
            Update_modify_panel();
        }

        public void Modify_indexLeft_ownTextBox_TextChanged()
        {
            int[] positive_indexes_oneBased;
            if (Try_get_positive_indexes_oneBased_from_textBox(Modify_indexLeft_ownTextBox.Text, out positive_indexes_oneBased))
            {
                Modify_indexLeft_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else if (String.IsNullOrEmpty(Modify_indexLeft_ownTextBox.Text))
            {
                Modify_indexLeft_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else
            {
                Modify_indexLeft_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }

        public void Modify_indexRight_ownTextBox_TextChanged()
        {
            int[] positive_indexes_oneBased;
            if (Try_get_positive_indexes_oneBased_from_textBox(Modify_indexRight_ownTextBox.Text, out positive_indexes_oneBased))
            {
                Modify_indexRight_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else if (String.IsNullOrEmpty(Modify_indexRight_ownTextBox.Text))
            {
                Modify_indexRight_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else
            {
                Modify_indexRight_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }

        private bool Try_get_positive_indexes_oneBased_from_textBox(string textBox_entry, out int[] positive_indexes_oneBased)
        {
            string[] preliminary_splitStrings = textBox_entry.Split(this.Delimiter_separating_multiple_substrings);
            List<string> splitStrings_list = new List<string>();
            foreach (string preliminary_splitString in preliminary_splitStrings)
            {
                if (!String.IsNullOrEmpty(preliminary_splitString))
                {
                    splitStrings_list.Add(preliminary_splitString);
                }
            }
            string[] splitStrings = splitStrings_list.ToArray();
            string splitString;
            int splitStrings_length = splitStrings.Length;
            positive_indexes_oneBased = new int[splitStrings_length];
            int potential_integer;
            bool all_substring_are_integers_or_empty = true;
            for (int indexSplit=0; indexSplit<splitStrings_length; indexSplit++)
            {
                splitString = splitStrings[indexSplit];
                if ((int.TryParse(splitString, out potential_integer)) && (potential_integer > 0))
                {
                    positive_indexes_oneBased[indexSplit] = potential_integer;
                }
                else if (String.IsNullOrEmpty(splitString)) { }
                else
                {
                    all_substring_are_integers_or_empty = false;
                }
            }
            return all_substring_are_integers_or_empty;
        }

        private Dataset_attributes_enum[] Get_attributes_after_changeButton_click(Button modifyChange_button, out string delimiter_string, out int[] indexesLeft_oneBased, out int[] indexesRight_oneBased)
        {
            Dataset_attributes_enum[] dataset_characterizations = new Dataset_attributes_enum[0];
            indexesLeft_oneBased = new int[0];
            indexesRight_oneBased = new int[0];
            delimiter_string = "error";
            bool all_necessary_information_available;
            if (modifyChange_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                modifyChange_button.BackColor = Form_default_settings.Color_button_pressed_back;
                modifyChange_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            }
            else if (Changes_allowed())
            {
                dataset_characterizations = this.ModifyCheckBoxes_order_of_activation.ToArray();
                dataset_characterizations = this.ModifyCheckBoxes_order_of_activation.ToArray();
                all_necessary_information_available = true;
                if (dataset_characterizations.Contains(Dataset_attributes_enum.Substring))
                {
                    if (Try_get_positive_indexes_oneBased_from_textBox(Modify_indexLeft_ownTextBox.Text, out indexesLeft_oneBased)) { }
                    else { indexesLeft_oneBased = new int[0]; }
                    if (Try_get_positive_indexes_oneBased_from_textBox(Modify_indexRight_ownTextBox.Text, out indexesRight_oneBased)) { }
                    else { indexesRight_oneBased = new int[0]; }
                    delimiter_string = (string)Modify_delimiter_ownTextBox.Text.Clone();
                    if (  ((indexesLeft_oneBased.Length>0) || (indexesRight_oneBased.Length > 0))
                        && (Modify_delimiter_ownTextBox.Text.Length > 0))
                    { all_necessary_information_available = true; }
                    else { all_necessary_information_available = false; }
                }
                if ((dataset_characterizations.Length > 0)&&(all_necessary_information_available))
                {
                    modifyChange_button.BackColor = Form_default_settings.Color_button_pressed_back;
                    modifyChange_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                }
                else
                {
                    dataset_characterizations = new Dataset_attributes_enum[0];
                }
            }
            return dataset_characterizations;
        }

        public Dataset_attributes_enum[] ChangeColor_button_Click(out string delimiter_string, out int[] indexesLeft_oneBased, out int[] indexesRight_oneBased)
        {
            Dataset_attributes_enum[] dataset_characterizations = Get_attributes_after_changeButton_click(Change_color_button, out delimiter_string, out indexesLeft_oneBased, out indexesRight_oneBased);
            if (dataset_characterizations.Length > 0)
            {
                List<Dataset_attributes_enum> new_dataset_attributes = new List<Dataset_attributes_enum>();
                new_dataset_attributes.AddRange(dataset_characterizations);
                new_dataset_attributes.Add(Dataset_attributes_enum.Color);
                dataset_characterizations = new_dataset_attributes.Distinct().OrderBy(l => l).ToArray();
            }
            return dataset_characterizations;
        }

        public Dataset_attributes_enum[] ChangeIntegrationGroup_button_Click(out string delimiter_string, out int[] indexesLeft_oneBased, out int[] indexesRight_oneBased)
        {
            Dataset_attributes_enum[] dataset_characterizations = Get_attributes_after_changeButton_click(Change_integrationGroup_button, out delimiter_string, out indexesLeft_oneBased, out indexesRight_oneBased);
            if (dataset_characterizations.Length > 0)
            {
                List<Dataset_attributes_enum> new_dataset_attributes = new List<Dataset_attributes_enum>();
                new_dataset_attributes.AddRange(dataset_characterizations);
                new_dataset_attributes.Add(Dataset_attributes_enum.IntegrationGroup);
                dataset_characterizations = new_dataset_attributes.Distinct().OrderBy(l => l).ToArray();
            }
            return dataset_characterizations;
        }

        public Dataset_attributes_enum[] ChangeDelete_button_Click(out string delimiter_string, out int[] indexesLeft_oneBased, out int[] indexesRight_oneBased)
        {
            Dataset_attributes_enum[] dataset_characterizations = Get_attributes_after_changeButton_click(Change_delete_button, out delimiter_string, out indexesLeft_oneBased, out indexesRight_oneBased);
            if (dataset_characterizations.Length > 0)
            {
                List<Dataset_attributes_enum> new_dataset_attributes = new List<Dataset_attributes_enum>();
                new_dataset_attributes.AddRange(dataset_characterizations);
                new_dataset_attributes.Add(Dataset_attributes_enum.Delete);
                dataset_characterizations = new_dataset_attributes.Distinct().OrderBy(l => l).ToArray();
            }
            return dataset_characterizations;
        }

        public void Unswitch_change_buttons()
        {
            Change_color_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Change_color_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Change_integrationGroup_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Change_integrationGroup_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Change_delete_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Change_delete_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }

        #region Explanation
        private void Write_explanation_into_error_reports_panel()
        {
            Error_reports_headline_label.Text = "Organize data";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Unique combination of dataset attributes");
            sb.AppendFormat("\r\nEach dataset must be assigned to a unique combination of Name, Up/Down-status, Timepoint and");
            sb.AppendFormat("\r\nIntegration group. If two different datasets are assigned to the same combination, the script will warn");
            sb.AppendFormat("\r\nthe user and ask to change at least one of indicated attributes.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nShow dataset attributes");
            sb.AppendFormat("\r\nSelection of check boxes in this panel specifies which dataset attributes are shown in the dataset window.");
            sb.AppendFormat("\r\nName: The given name of a dataset");
            sb.AppendFormat("\r\nUp/Down: The direction of expression change (i.e. up- or downregulated)");
            sb.AppendFormat("\r\nTimepoint: The timepoint of a dataset (if available)");
            sb.AppendFormat("\r\nIntegration group: Datasets can be grouped into integration groups. Results of all datasets of the same");
            sb.AppendFormat("\r\n                   integration group will be visualized together in the same figure sets or networks. See");
            sb.AppendFormat("\r\n                   menu 'Enrichment' for details.");
            sb.AppendFormat("\r\nSource: The source of a dataset is either a file name or a label indicating its manual upload via the MBCO");
            sb.AppendFormat("\r\n        PathNet user interface. The source is the reference for the assignment of background genes to the");
            sb.AppendFormat("\r\n        different datasets. See menu 'Background genes' for details.");
            sb.AppendFormat("\r\nColor: User-defined dataset colors will be used to color SCPs predicted for each dataset in the SCP networks.");
            sb.AppendFormat("\r\n       In addition, the user can specify to color all bars and timelines in user-defined colors in the menu");
            sb.AppendFormat("\r\n       'Enrichment'.");
            sb.AppendFormat("\r\nDataset order #: This entry specifies the order of the results for the datasets of the same integration group");
            sb.AppendFormat("\r\n                 in bardiagrams and heatmaps.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nAdd data sources or dataset order # to dataset names");
            sb.AppendFormat("\r\nIf different source files contain the same dataset names, the user can quicky add the source of each dataset");
            sb.AppendFormat("\r\nto its name, giving them unique dataset names. Similarly, the dataset order # can be added as well. The latter");
            sb.AppendFormat("\r\ncan allow easier interpretation of the results and saving those order numbers for their re-import into the");
            sb.AppendFormat("\r\napplication. Before each enrichment analysis, all user supplied dataset files are saved in the Results folder");
            sb.AppendFormat("\r\n'Input_Data'. From this folder they can be re-imported into the application using the functionalities of the");
            sb.AppendFormat("\r\nmenu 'Read data'. Adding the order # to the front of the dataset names will allow to save and restore them");
            sb.AppendFormat("\r\nafter re-import. Simply sort the re-uploaded data by dataset name, and press 'Dataset order' in the"); 
            sb.AppendFormat("\r\n'Automatically set'-panel.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nConvert timepoints");
            sb.AppendFormat("\r\nThis menu panel allows quick conversion of all timepoints to the selected time unit.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nModify all datasets");
            sb.AppendFormat("\r\nThe user can select given attributes to define datasets that will be temporarily grouped for modifications.");
            sb.AppendFormat("\r\nAll datasets that contain the same values at the given attributes will be grouped together. The selection");
            sb.AppendFormat("\r\n'Substring(s) in Name' allows definition of a delimiter and one or more index positions. All dataset names");
            sb.AppendFormat("\r\nwill be be split by the delimiter into multiple substrings and all datasets with the same substrings at the");
            sb.AppendFormat("\r\nselected indexes from the left and/or right will be temporarily grouped together. Multiple indexes from the same");
            sb.AppendFormat("\r\nside can be separated by comma. After selection of the characteristics pressing one of the three buttons below");
            sb.AppendFormat("\r\nwill collapse all datasets into the specified groups. Depending on the pressed button, the user can change the");
            sb.AppendFormat("\r\ncolor or integration group of all datasets of the same temporarily group at once or mark them for deletion.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nAutomatically set");
            sb.AppendFormat("\r\nPressing one of these three buttons will automatically set the selected value under consideration of the");
            sb.AppendFormat("\r\ncurrent order of the datasets in the dataset panel and temporarily groupings as specified in the panel above.");

            Error_reports_ownTextBox.SilentText_and_refresh = sb.ToString();
        }

        public void Explanation_button_activated()
        {
            Write_explanation_into_error_reports_panel();
        }
        #endregion

    }
}
