//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows_forms_customized_tools;
using System.Windows.Forms;
using Data;
using Common_functions.Global_definitions;
using Common_functions.Text;
using Common_functions.ReadWrite;
using Common_functions.Form_tools;

namespace ClassLibrary1.BgGenes_userInterface
{
    class BgGenes_default_texts_class
    {
        public static string BgGenes_addGenes_textBox_default_text { get { return "Copy paste background gene list (one gene per row)"; } }
        public static string BgGenes_addName_textBox_default_text { get { return "Enter name of list"; } }
        public static string BgGenes_addName_textBox_duplicated_name_text {  get {  return "Name already exists"; } }
        public static string BgGenes_addGenes_textBox_noGenes_text {  get { return "No genes added"; } }
        public static string Explanation_button_label { get { return "Explanation"; } }
        public static string Warning_button_label { get { return "Warning"; } }
        public static bool Is_among_addGenes_textBox_texts(string input)
        {
            if (input.Equals(BgGenes_addGenes_textBox_noGenes_text)) { return true; }
            else if (input.Equals(BgGenes_addGenes_textBox_default_text)) { return true; }
            else { return false; }
        }
        public static bool Is_among_addNames_textBox_texts(string input)
        {
            if (input.Equals(BgGenes_addName_textBox_default_text)) { return true; }
            else if (input.Equals(BgGenes_addName_textBox_duplicated_name_text)) { return true; }
            else { return false; }
        }
    }

    class Read_bgGenes_interface_options_class
    {
        public int Max_documented_errors_per_file { get; set; }
        public Read_bgGenes_interface_options_class()
        {
            Max_documented_errors_per_file = 5;
        }
    }


    class BgGenes_userInterface_class
    {
        public MyPanel Overall_panel { get; set; }
        public Label Overall_headline_label { get; set; }
        public MyPanel Add_panel { get; set; }
        public Label Add_genes_label { get; set; }
        public OwnTextBox Add_genes_textBox { get; set; }
        public Label Add_name_label { get; set; }
        public OwnTextBox Add_name_textBox { get; set; }
        public Button Add_button { get; set; }
        public Label Add_readFileDir_label { get; set; }
        public MyCheckBox_button Add_readOnlyFile_cbButton { get; set; }
        public Label Add_readOnlyFile_cbLabel { get; set; }
        public MyCheckBox_button Add_readAllFilesInDirectory_cbButton { get; set; }
        public Label Add_readAllFilesInDirectory_cbLabel { get; set; }
        public Button Add_read_button { get; set; }
        public Button Add_showErrors_button { get; set; }
        public Label Add_read_error_reports_label { get; set; }
        public Label Add_readExplainFile_label { get; set; }
        public MyPanel Organize_panel { get; set; }
        public Label Organize_availableBgGeneLists_label { get; set; }
        public OwnListBox Organize_availableBgGeneLists_ownListBox { get; set; }
        public Button Organize_deleteSelection_button { get; set; }
        public Button Organize_deleteAll_button { get; set; }
        public MyPanel Assignment_panel { get; set; }
        public Button Assignments_automatic_button { get; set; }
        public Label Assignments_automatic_label { get; set; }
        public Button Assignments_reset_button { get; set; }
        public Label Assignments_reset_label { get; set; }
        public Label AssignmentsExplanation_label { get; set; }
        public Button Warnings_button { get; set; }
        public Label Warnings_label { get; set; }
        public MyPanel Warnings_panel { get; set; }
        public OwnTextBox Read_directoryOrFile_ownTextBox { get; set; }
        public Label Read_directoryOrFile_label { get; set; }
        private Read_error_message_line_class[] Last_error_reports { get; set; }
        private Custom_data_summary_class Custom_data_summary { get; set; }
        private Label Error_reports_maxErrorPerFile1_label { get; set; }
        private Label Error_reports_maxErrorPerFile2_label { get; set; }
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }
        public Label Error_reports_headline_label { get; set; }
        public Dataset_attributes_enum[] BgGenesInterface_dataset_attributes {  get { return new Dataset_attributes_enum[] { Dataset_attributes_enum.SourceFile, Dataset_attributes_enum.BgGenes }; } }
        private Read_bgGenes_interface_options_class Options { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }


        public BgGenes_userInterface_class(MyPanel overall_panel,
                                           Label overall_headline_label,
                                           MyPanel add_panel,
                                           Label add_genes_label,
                                           OwnTextBox add_genes_textBox,
                                           Label add_name_label,
                                           OwnTextBox add_name_textBox,
                                           Button add_button,
                                           Label add_readFileDir_label,
                                           MyCheckBox_button add_readOnlyFile_cbButton,
                                           Label add_readOnlyFile_cbLabel,
                                           MyCheckBox_button add_readAllFilesInDirectory_cbButton,
                                           Label add_readAllFilesInDirectory_cbLabel,
                                           Button add_read_button,
                                           Button add_showErrors_button,
                                           Label add_read_error_reports_label,
                                           Label add_readExplainFile_label,
                                           MyPanel organize_panel,
                                           Label organize_availableBgGeneLists_label,
                                           OwnListBox organize_availableBgGeneLists_ownListBox,
                                           Button organize_deleteSelection_button,
                                           Button organize_deleteAll_button,
                                           MyPanel assignment_panel,
                                           Button assignments_automatic_button,
                                           Label assignments_automatic_label,
                                           Button assignments_reset_button,
                                           Label assignments_reset_label,
                                           Label assignmentsExplanation_label,
                                           MyPanel warnings_panel,
                                           Label warnings_label,
                                           Button warnings_button,
                                           Custom_data_class custom_data,
                                           OwnTextBox read_directoryOrFile_ownTextBox,
                                           Label read_directoryOrFile_label,
                                           Label error_reports_headline_label,
                                           Label error_reports_maxErrorPerFile1_label,
                                           Label error_reports_maxErrorPerFile2_label,
                                           OwnTextBox error_reports_ownTextBox,
                                           OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,
                                           Form1_default_settings_class form_default_settings)
        {
            Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.Overall_headline_label = overall_headline_label;
            this.Add_panel = add_panel;
            this.Add_genes_textBox = add_genes_textBox;
            this.Add_name_textBox = add_name_textBox;
            this.Add_genes_label = add_genes_label;
            this.Add_name_label = add_name_label;
            this.Add_button = add_button;
            this.Add_readOnlyFile_cbButton = add_readOnlyFile_cbButton;
            this.Add_readOnlyFile_cbLabel = add_readOnlyFile_cbLabel;
            this.Add_readAllFilesInDirectory_cbButton = add_readAllFilesInDirectory_cbButton;
            this.Add_readAllFilesInDirectory_cbLabel = add_readAllFilesInDirectory_cbLabel;
            this.Add_read_button = add_read_button;
            this.Add_showErrors_button = add_showErrors_button;
            this.Add_readFileDir_label = add_readFileDir_label;
            this.Add_read_error_reports_label = add_read_error_reports_label;
            this.Add_readExplainFile_label = add_readExplainFile_label;
            this.Warnings_panel = warnings_panel;
            this.Warnings_button = warnings_button;
            this.Warnings_label = warnings_label;
            this.Organize_panel = organize_panel;
            this.Organize_availableBgGeneLists_label = organize_availableBgGeneLists_label;
            this.Organize_availableBgGeneLists_ownListBox = organize_availableBgGeneLists_ownListBox;
            this.Organize_deleteSelection_button = organize_deleteSelection_button;
            this.Organize_deleteAll_button = organize_deleteAll_button;
            this.Assignment_panel = assignment_panel;
            this.Assignments_reset_button = assignments_reset_button;
            this.Assignments_automatic_button = assignments_automatic_button;
            this.Assignments_reset_label = assignments_reset_label;
            this.AssignmentsExplanation_label = assignmentsExplanation_label;
            this.Read_directoryOrFile_label = read_directoryOrFile_label;
            this.Read_directoryOrFile_ownTextBox = read_directoryOrFile_ownTextBox;
            this.Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            this.Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            this.Error_reports_headline_label = error_reports_headline_label;
            this.Error_reports_ownTextBox = error_reports_ownTextBox;
            this.Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;
            this.Assignments_automatic_label = assignments_automatic_label;
            Update_all_graphic_elements(custom_data);
        }

        public void Update_all_graphic_elements(Custom_data_class custom_data)
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            MyPanel my_panel;
            Label my_label;
            MyCheckBox_button my_cbButton;
            OwnTextBox my_textBox;
            Button my_button;
            OwnListBox my_listBox;

            Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            #region Add panel
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = (int)Math.Round(0.06 * this.Overall_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.63 * this.Overall_panel.Height);
            this.Add_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(Add_panel,left_referenceBorder,right_referenceBorder,top_referenceBorder,bottom_referenceBorder);

            int minimum_distance_from_x_panel_edges = (int)Math.Round(0.01F*(float)this.Add_panel.Width);
            int shared_button_height_addPanel = (int)Math.Round(0.12F * this.Add_panel.Height);
            int shared_button_width_addPanel = (int)Math.Round(0.22F * this.Add_panel.Width);

            left_referenceBorder = minimum_distance_from_x_panel_edges;
            right_referenceBorder = (int)Math.Round(0.5F * this.Add_panel.Width);
            top_referenceBorder = (int)Math.Round(0.12 * this.Add_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.6 * this.Add_panel.Height);
            my_textBox = Add_genes_textBox;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox,left_referenceBorder,right_referenceBorder,top_referenceBorder,bottom_referenceBorder);

            left_referenceBorder = Add_genes_textBox.Location.X + Add_genes_textBox.Width +  2 * minimum_distance_from_x_panel_edges;
            right_referenceBorder = this.Add_panel.Width - minimum_distance_from_x_panel_edges;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.1 * this.Add_panel.Height);
            my_textBox = Add_name_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Add_genes_textBox.Location.X;
            right_referenceBorder = this.Add_genes_textBox.Location.X + this.Add_genes_textBox.Height;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Add_genes_textBox.Location.Y;
            my_label = this.Add_genes_label;
            this.Add_genes_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Add_name_textBox.Location.X;
            right_referenceBorder = this.Add_name_textBox.Location.X + Add_name_textBox.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Add_name_textBox.Location.Y;
            my_label = this.Add_name_label;
            this.Add_name_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            right_referenceBorder = this.Add_panel.Width - minimum_distance_from_x_panel_edges;
            left_referenceBorder = right_referenceBorder - shared_button_width_addPanel;
            top_referenceBorder = this.Add_name_textBox.Location.Y + 2 * this.Add_name_textBox.Height;
            bottom_referenceBorder = top_referenceBorder + shared_button_height_addPanel;
            my_button = this.Add_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button,left_referenceBorder,right_referenceBorder,top_referenceBorder,bottom_referenceBorder);

            right_referenceBorder = this.Add_panel.Width - minimum_distance_from_x_panel_edges;
            left_referenceBorder = right_referenceBorder - shared_button_width_addPanel;
            top_referenceBorder = this.Add_genes_textBox.Location.Y + this.Add_genes_textBox.Height;
            bottom_referenceBorder = top_referenceBorder + shared_button_height_addPanel;
            my_button = this.Add_read_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int heightWidth_of_one_read_checkBox = (int)Math.Round(0.07F * this.Add_panel.Height);
            int shared_location_x_of_read_cbButtons = (int)Math.Round(0.2F * this.Add_panel.Width);

            left_referenceBorder = shared_location_x_of_read_cbButtons;
            right_referenceBorder = left_referenceBorder + heightWidth_of_one_read_checkBox;
            top_referenceBorder = this.Add_genes_textBox.Location.Y + this.Add_genes_textBox.Height;
            bottom_referenceBorder = top_referenceBorder + heightWidth_of_one_read_checkBox;
            my_cbButton = this.Add_readOnlyFile_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Add_readOnlyFile_cbButton.Location.X + this.Add_readOnlyFile_cbButton.Width;
            right_referenceBorder = Add_read_button.Location.X;
            my_label = this.Add_readOnlyFile_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_location_x_of_read_cbButtons;
            right_referenceBorder = left_referenceBorder + heightWidth_of_one_read_checkBox;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + heightWidth_of_one_read_checkBox;
            my_cbButton = this.Add_readAllFilesInDirectory_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Add_readAllFilesInDirectory_cbButton.Location.X + this.Add_readAllFilesInDirectory_cbButton.Width;
            right_referenceBorder = Add_read_button.Location.X;
            my_label = this.Add_readAllFilesInDirectory_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = minimum_distance_from_x_panel_edges;
            right_referenceBorder = left_referenceBorder + (int)Math.Round(0.3F*this.Add_panel.Width);
            top_referenceBorder = (int)Math.Round(this.Add_panel.Height - 0.3F*(this.Add_panel.Height - this.Add_readOnlyFile_cbButton.Location.Y - this.Add_readOnlyFile_cbButton.Height));
            bottom_referenceBorder = (int)Math.Round(0.99F*this.Add_panel.Height);
            my_button = this.Add_showErrors_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int middle = this.Add_readAllFilesInDirectory_cbButton.Location.Y + (int)Math.Round(0.5 * (this.Add_readOnlyFile_cbButton.Location.Y + this.Add_readOnlyFile_cbButton.Height - Add_readAllFilesInDirectory_cbButton.Location.Y));
            int half_distance = (int)Math.Round(0.7*this.Add_readAllFilesInDirectory_cbButton.Height);
            left_referenceBorder = minimum_distance_from_x_panel_edges;
            right_referenceBorder = this.Add_readAllFilesInDirectory_cbButton.Location.X;
            top_referenceBorder = middle - half_distance;
            bottom_referenceBorder = middle + half_distance;
            my_label = this.Add_readFileDir_label;
            this.Add_readFileDir_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Update_readExplainFile_label();

            Set_add_panel_add_read_error_reports_label();
            #endregion

            #region Overall headline
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Add_panel.Location.Y;
            my_label = Overall_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Organize panel
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = this.Add_panel.Location.Y + Add_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.78 * this.Overall_panel.Height);
            my_panel = this.Organize_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.05F * Organize_panel.Width);
            right_referenceBorder = (int)Math.Round(0.95F * Organize_panel.Width);
            top_referenceBorder = (int)Math.Round(0.30F * Organize_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.70F * Organize_panel.Height);
            my_listBox = this.Organize_availableBgGeneLists_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = Organize_availableBgGeneLists_ownListBox.Location.Y + Organize_availableBgGeneLists_ownListBox.Height;
            bottom_referenceBorder = (int)Math.Round(0.98F * Organize_panel.Height);

            left_referenceBorder = (int)Math.Round(0.05F * Organize_panel.Width);
            right_referenceBorder = (int)Math.Round(0.475F * Organize_panel.Width);
            my_button = this.Organize_deleteSelection_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.525F * Organize_panel.Width);
            right_referenceBorder = (int)Math.Round(0.95F * Organize_panel.Width);
            my_button = this.Organize_deleteAll_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.Organize_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Organize_availableBgGeneLists_ownListBox.Location.Y;
            my_label = Organize_availableBgGeneLists_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Warnings panel
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = this.Organize_panel.Location.Y + Organize_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.84 * this.Overall_panel.Height);
            this.Warnings_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(Warnings_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Set_warnings_button_and_label_fontSize_size_and_visibility(false);

            left_referenceBorder = 0;
            right_referenceBorder = this.Warnings_button.Location.X;
            top_referenceBorder = 0;
            bottom_referenceBorder = Warnings_panel.Height;
            my_label = this.Warnings_label;
            this.Warnings_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            this.Warnings_label.BackColor = Form_default_settings.Warnings_back_color;
            #endregion

            #region Assignments panel
            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = this.Warnings_panel.Location.Y + Warnings_panel.Height;
            bottom_referenceBorder = (int)Math.Round(1.0 * this.Overall_panel.Height);
            my_panel = this.Assignment_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int automatic_asignment_button_widthHeight = (int)Math.Round(0.3F * this.Assignment_panel.Height);
            left_referenceBorder = (int)Math.Round(0.01F * this.Assignment_panel.Width);
            right_referenceBorder = left_referenceBorder + automatic_asignment_button_widthHeight;

            top_referenceBorder = (int)Math.Round(0.05F * this.Assignment_panel.Height);
            bottom_referenceBorder = top_referenceBorder + automatic_asignment_button_widthHeight;
            my_button = Assignments_reset_button;
            Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = Assignments_reset_button.Location.Y + Assignments_reset_button.Height;
            bottom_referenceBorder = top_referenceBorder + automatic_asignment_button_widthHeight;
            my_button = this.Assignments_automatic_button;
            Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Assignments_reset_button.Location.X + this.Assignments_reset_button.Width;
            right_referenceBorder = this.Assignment_panel.Width;
            top_referenceBorder = this.Assignments_reset_button.Location.Y;
            bottom_referenceBorder = this.Assignments_reset_button.Location.Y + this.Assignments_reset_button.Height; ;
            my_label = this.Assignments_reset_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Assignments_automatic_button.Location.X + this.Assignments_automatic_button.Width;
            right_referenceBorder = this.Assignment_panel.Width;
            top_referenceBorder = this.Assignments_automatic_button.Location.Y;
            bottom_referenceBorder = this.Assignments_automatic_button.Location.Y + this.Assignments_automatic_button.Height; ;
            my_label = this.Assignments_automatic_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Assignments_automatic_button.Location.X;
            right_referenceBorder = this.Assignment_panel.Width;
            top_referenceBorder = this.Assignments_automatic_button.Location.Y + this.Assignments_automatic_button.Height;
            bottom_referenceBorder = Assignment_panel.Height;
            my_label = this.AssignmentsExplanation_label;
            this.AssignmentsExplanation_label = Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_upperYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            this.Add_readAllFilesInDirectory_cbButton.SilentChecked = true;
            this.Add_readOnlyFile_cbButton.SilentChecked = !this.Add_readAllFilesInDirectory_cbButton.Checked;
            this.Options = new Read_bgGenes_interface_options_class();
            this.Last_error_reports = new Read_error_message_line_class[0];
            this.Organize_availableBgGeneLists_ownListBox.SelectionMode = SelectionMode.One;
            this.Custom_data_summary = new Custom_data_summary_class();
            Set_to_default(custom_data);
        }

        private void Set_add_panel_add_read_error_reports_label()
        {
            int left_referenceBorder = this.Add_showErrors_button.Location.X + this.Add_showErrors_button.Width;
            int right_referenceBorder = this.Add_panel.Width;
            int top_referenceBorder = this.Add_showErrors_button.Location.Y;
            int bottom_referenceBorder = this.Add_panel.Height;
            Label my_label = this.Add_read_error_reports_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
        }

        private void Set_warnings_button_and_label_fontSize_size_and_visibility(bool warnigs_activated)
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            if (warnigs_activated) { left_referenceBorder = (int)Math.Round(0.65F * this.Warnings_panel.Width); }
            else { left_referenceBorder = (int)Math.Round(0.35F * this.Warnings_panel.Width); }
            right_referenceBorder = left_referenceBorder + (int)Math.Round(0.3F * this.Warnings_panel.Width);
            top_referenceBorder = (int)Math.Round(0.05F * this.Warnings_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.95F * this.Warnings_panel.Height);
            Button my_button = this.Warnings_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            if (warnigs_activated) { this.Warnings_label.Visible = true; }
            else { this.Warnings_label.Visible = false; }
        }

        public void Set_to_default(Custom_data_class custom_data)
        {
            this.Options = new Read_bgGenes_interface_options_class();
            this.Last_error_reports = new Read_error_message_line_class[0];
            this.Custom_data_summary = new Custom_data_summary_class();
            Set_to_visible(custom_data);
        }

        public void Set_to_visible(Custom_data_class custom_data)
        {
            this.Add_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Add_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Add_read_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Add_read_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Add_showErrors_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Add_showErrors_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Add_showErrors_button.Visible = false;
            this.Organize_deleteAll_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Organize_deleteAll_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Organize_deleteSelection_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Organize_deleteSelection_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Assignments_automatic_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Assignments_automatic_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Assignments_reset_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Assignments_reset_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.AssignmentsExplanation_label.ForeColor = Form_default_settings.ExplanationText_color;
            this.Warnings_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Warnings_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Set_add_textBox_texts_to_default();
            Update_available_bgGeneListsName_listBox(custom_data);
            this.Read_directoryOrFile_ownTextBox.Visible = true;
            this.Read_directoryOrFile_label.Visible = true;
            Update_readExplainFile_label();
            Update_read_error_reports();
            Update_bgGenes_warnings();
        }

        private void Update_readExplainFile_label()
        {
            if ((Add_readAllFilesInDirectory_cbButton.Checked) && (!Add_readOnlyFile_cbButton.Checked))
            {
                this.Read_directoryOrFile_label.Text = "Read all background gene files in directory:";
                this.Add_readExplainFile_label.Text = "List genes in one column, one gene per row (no\r\nheadline). All files ending with '_bgGenes' will be read.";
            }
            else if ((!Add_readAllFilesInDirectory_cbButton.Checked) && (Add_readOnlyFile_cbButton.Checked))
            {
                this.Read_directoryOrFile_label.Text = "Read background gene file:";
                this.Add_readExplainFile_label.Text = "List genes in one column, one gene per row\r\n(no headline).";
            }
            else 
            {
                this.Read_directoryOrFile_label.Text = "";
                this.Add_readExplainFile_label.Text = "";
            }
            int left_referenceBorder = 0;
            int right_referenceBorder = this.Overall_panel.Width;
            int top_referenceBorder = this.Add_readAllFilesInDirectory_cbButton.Location.Y + Add_readAllFilesInDirectory_cbButton.Height;
            int bottom_referenceBorder = this.Add_showErrors_button.Location.Y;
            Label my_label = this.Add_readExplainFile_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

        }

        private void Set_add_textBox_texts_to_default()
        {
            Add_genes_textBox.Text = BgGenes_default_texts_class.BgGenes_addGenes_textBox_default_text;
            Add_name_textBox.Text = BgGenes_default_texts_class.BgGenes_addName_textBox_default_text;
        }

        private void Update_available_bgGeneListsName_listBox(Custom_data_class custom_data)
        {
            string old_selection = (string)Organize_availableBgGeneLists_ownListBox.Text.Clone();
            string[] bgGenes_lists = custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray();
            Organize_availableBgGeneLists_ownListBox.Items.Clear();
            Organize_availableBgGeneLists_ownListBox.Items.AddRange(bgGenes_lists);
            int indexOld = Organize_availableBgGeneLists_ownListBox.Items.IndexOf(old_selection);
            if (indexOld != -1)
            {
                Organize_availableBgGeneLists_ownListBox.SilentSelectedIndex = indexOld;
            }
            else
            {
                Organize_availableBgGeneLists_ownListBox.SilentSelectedIndex = Organize_availableBgGeneLists_ownListBox.Items.IndexOf(Global_class.Mbco_exp_background_gene_list_name);
            }
        }

        public Custom_data_class Add_background_genes(Custom_data_class custom_data)
        {
            string bgGenesListName = Add_name_textBox.Text;
            if (custom_data.ExpBgGenesList_bgGenes_dict.ContainsKey(bgGenesListName))
            {
                Add_name_textBox.Text = BgGenes_default_texts_class.BgGenes_addName_textBox_duplicated_name_text;
            }
            else if (  (BgGenes_default_texts_class.Is_among_addNames_textBox_texts(bgGenesListName))
                     ||(String.IsNullOrEmpty(bgGenesListName)))
            {
                Add_name_textBox.Text = BgGenes_default_texts_class.BgGenes_addName_textBox_default_text;
            }
            else if (BgGenes_default_texts_class.Is_among_addGenes_textBox_texts(Add_genes_textBox.Text))
            {
                Add_genes_textBox.Text = BgGenes_default_texts_class.BgGenes_addGenes_textBox_default_text;
            }
            else
            {
                string[] bgGenes = Add_genes_textBox.Text.Split('\r');
                bgGenes = bgGenes.Distinct().ToArray();
                string bgGene;
                List<string> add_bgGenes = new List<string>();
                int bgGenes_length = bgGenes.Length;
                for (int indexBg = 0; indexBg < bgGenes_length; indexBg++)
                {
                    bgGene = bgGenes[indexBg];
                    bgGene = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(bgGene);
                    bgGene = Text_class.Remove_enter_characters_from_beginning_of_text(bgGene);
                    if (!String.IsNullOrEmpty(bgGene))
                    //&& (!Default_textBox_texts.Is_among_inputGene_list_textBox_texts(inputGene)))
                    {
                        add_bgGenes.Add(bgGene);
                    }
                }
                if (add_bgGenes.Count > 0)
                {
                    bool succesful = custom_data.Add_new_experimental_background_genes_and_return_success(bgGenesListName, add_bgGenes.ToArray());
                    if (!succesful) { throw new Exception(); }
                    Set_add_textBox_texts_to_default();
                    Update_available_bgGeneListsName_listBox(custom_data);
                }
                else
                {
                    Add_genes_textBox.Text = BgGenes_default_texts_class.BgGenes_addGenes_textBox_noGenes_text;
                }
            }
            return custom_data;
        }

        #region Organize
        public Custom_data_class Remove_selected_bgGenesList(Custom_data_class custom_data)
        {
            string selected_bgGenes_list = (string)Organize_availableBgGeneLists_ownListBox.Text.Clone();
            custom_data.Remove_indicated_bgGenes_list(selected_bgGenes_list);
            Update_available_bgGeneListsName_listBox(custom_data);
            return custom_data;
        }
        public Custom_data_class Reset_bgGenesLists_to_default(Custom_data_class custom_data)
        {
            custom_data.Reset_expBgGenesList_to_default();
            Update_available_bgGeneListsName_listBox(custom_data);
            return custom_data;
        }
        #endregion

        #region Read error reports
        public void Error_reports_maxErrorsPerFile_ownTextBox_TextChanged()
        {
            int new_number;
            if (int.TryParse(Error_reports_maxErrorsPerFile_ownTextBox.Text, out new_number))
            {
                Options.Max_documented_errors_per_file = new_number;
                Update_read_error_reports();
            }
        }

        private void Write_read_error_reports_into_error_reports_panel()
        {
            Error_reports_headline_label.Text = "Error messages";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = true;
            Error_reports_maxErrorPerFile1_label.Refresh();
            Error_reports_maxErrorPerFile2_label.Visible = true;
            Error_reports_maxErrorPerFile2_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Text = Options.Max_documented_errors_per_file.ToString();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = true;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            this.Last_error_reports = this.Last_error_reports.OrderBy(l => l.Complete_fileName).ToArray();
            int last_error_reports_length = this.Last_error_reports.Length;
            Read_error_message_line_class error_message_line;
            int files_with_added_data = 0;
            int files_with_error_reports = 0;
            int error_reports_in_this_file = 0;
            int max_documented_error_reports = this.Options.Max_documented_errors_per_file;
            StringBuilder sb = new StringBuilder();
            string error_text;
            for (int indexLE = 0; indexLE < last_error_reports_length; indexLE++)
            {
                error_message_line = this.Last_error_reports[indexLE];
                if (error_message_line.Error_message.Equals(Read_error_message_enum.BgGenes_file_read))
                {
                    files_with_added_data++;
                }
                else
                {

                    if ((indexLE == 0)
                        || (!error_message_line.Complete_fileName.Equals(this.Last_error_reports[indexLE - 1].Complete_fileName)))
                    {
                        files_with_error_reports++;
                        error_reports_in_this_file = 0;
                    }
                    error_reports_in_this_file++;
                    if (error_reports_in_this_file <= max_documented_error_reports)
                    {
                        if (indexLE > 0) { sb.AppendFormat("\r\n"); }

                        error_text = "Coding error in C# script";
                        switch (error_message_line.Error_message)
                        {
                            case Read_error_message_enum.File_does_not_exist:
                                error_text = "File does not exist in given directory";
                                break;
                            case Read_error_message_enum.Duplicated_bggenes_dataset:
                                error_text = "Bg gene list with same name already exists";
                                break;
                            case Read_error_message_enum.BgGenes_file_read:
                                break;
                            default:
                                throw new Exception();
                        }
                        sb.AppendFormat(System.IO.Path.GetFileName(error_message_line.Complete_fileName) + ":   " + error_text);
                    }
                    if ((indexLE == last_error_reports_length - 1)
                        || (!error_message_line.Complete_fileName.Equals(this.Last_error_reports[indexLE + 1].Complete_fileName)))
                    {
                        if (error_reports_in_this_file > max_documented_error_reports)
                        {
                            if (indexLE > 0) { sb.AppendFormat("\r\n"); }
                            error_text = error_reports_in_this_file - max_documented_error_reports + " additional errors found";
                            sb.AppendFormat(System.IO.Path.GetFileName(error_message_line.Complete_fileName) + ":   " + error_text);
                        }
                    }
                }
            }
            Error_reports_ownTextBox.SilentText = sb.ToString();
        }

        public void Read_error_reports_button_activated()
        {
            Write_read_error_reports_into_error_reports_panel();
        }

        private void Update_read_error_reports()
        {
            int last_error_reports_length = Last_error_reports.Length;
            Read_error_message_line_class read_error_line;
            List<Read_error_message_line_class> only_error_reports = new List<Read_error_message_line_class>();
            List<string> fileNames_with_added_data = new List<string>();
            List<string> fileNames_with_not_added_data = new List<string>();
            for (int indexRead=0;indexRead<last_error_reports_length;indexRead++)
            {
                read_error_line = Last_error_reports[indexRead];
                if (read_error_line.Error_message.Equals(Read_error_message_enum.BgGenes_file_read))
                {
                    fileNames_with_added_data.Add(read_error_line.Complete_fileName);

                }
                else if (  (!read_error_line.Error_message.Equals(Read_error_message_enum.BgGenes_file_read))
                         &&(!read_error_line.Error_message.Equals(Read_error_message_enum.Directory_does_not_exist))
                         &&(!read_error_line.Error_message.Equals(Read_error_message_enum.Invalid_spelling_of_directory_or_file_name))
                         &&(!read_error_line.Error_message.Equals(Read_error_message_enum.File_does_not_exist)))
                {
                    only_error_reports.Add(read_error_line);
                    fileNames_with_not_added_data.Add(read_error_line.Complete_fileName);
                }
                else
                {
                    only_error_reports.Add(read_error_line);

                }
            }
            fileNames_with_not_added_data = fileNames_with_not_added_data.Distinct().OrderBy(l => l).ToList();
            fileNames_with_added_data = fileNames_with_added_data.Distinct().OrderBy(l => l).ToList();
            int added_datasets_count = fileNames_with_added_data.Count;
            int not_added_datasets_count = fileNames_with_not_added_data.Count;
            int error_label_line_breaks = 0;

            StringBuilder error_label_sb = new StringBuilder();

            Add_read_error_reports_label.Text = "";
            if (added_datasets_count == 1)
            {
                if (error_label_sb.Length > 0)
                {
                    error_label_sb.AppendFormat("\r\n");
                    error_label_line_breaks++;
                }
                error_label_sb.AppendFormat("Bg genes from {0} file added", added_datasets_count);
            }
            else if (added_datasets_count > 1)
            {
                if (error_label_sb.Length > 0)
                {
                    error_label_sb.AppendFormat("\r\n");
                    error_label_line_breaks++;
                }
                error_label_sb.AppendFormat("Bg genes from {0} files added", added_datasets_count);
            }

            if (not_added_datasets_count == 1)
            {
                if (error_label_sb.Length > 0)
                {
                    error_label_sb.AppendFormat("\r\n");
                    error_label_line_breaks++;
                }
                error_label_sb.AppendFormat("Bg genes from {0} file not added", not_added_datasets_count);
            }
            else if (not_added_datasets_count > 1)
            {
                if (error_label_sb.Length > 0)
                {
                    error_label_sb.AppendFormat("\r\n");
                    error_label_line_breaks++;
                }
                error_label_sb.AppendFormat("Bg genes from {0} files not added", not_added_datasets_count);
            }

            if (only_error_reports.Count == 0)
            {
                Add_showErrors_button.Visible = false;
                Add_read_error_reports_label.Visible = true;
            }
            else if ((only_error_reports.Count == 1) && (only_error_reports[0].Error_message.Equals(Read_error_message_enum.Directory_does_not_exist)))
            {
                Add_showErrors_button.Visible = false;
                if (error_label_sb.Length > 0)
                {
                    error_label_sb.AppendFormat("\r\n");
                    error_label_line_breaks++;
                }
                error_label_sb.AppendFormat("Directory does not exist", added_datasets_count);
            }
            else if ((only_error_reports.Count == 1) && (only_error_reports[0].Error_message.Equals(Read_error_message_enum.File_does_not_exist)))
            {
                Add_showErrors_button.Visible = false;
                if (error_label_sb.Length > 0)
                {
                    error_label_sb.AppendFormat("\r\n");
                    error_label_line_breaks++;
                }
                error_label_sb.AppendFormat("File does not exist", added_datasets_count);
            }
            else
            { 
                Add_showErrors_button.Visible = true;
                Add_read_error_reports_label.Visible = true;
            }
            Add_read_error_reports_label.Text = error_label_sb.ToString();
            Set_add_panel_add_read_error_reports_label();
        }
        #endregion

        #region Custom data summary - Warnings
        private void Write_warnings_into_error_reports_panel()
        {
            Error_reports_headline_label.Text = "Warnings";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            StringBuilder sb = new StringBuilder();
            Custom_data_summary_line_class custom_data_summary_line;
            int custom_data_summary_length = Custom_data_summary.Summaries.Length;
            Custom_data_summary.Summaries = Custom_data_summary.Summaries.OrderBy(l => l.Source_fileName).ToArray();
            int missing_genes_count = 0;
            bool new_file_announced = false;
            string dash_string = "_____________________________________________________________________________________________";
            for (int indexCD = 0; indexCD < custom_data_summary_length; indexCD++)
            {
                custom_data_summary_line = Custom_data_summary.Summaries[indexCD];
                if ((indexCD == 0) || (!custom_data_summary_line.Source_fileName.Equals(Custom_data_summary.Summaries[indexCD - 1].Source_fileName)))
                {
                    new_file_announced = false;
                }
                missing_genes_count = custom_data_summary_line.Total_genes_count - custom_data_summary_line.Total_genes_among_experimental_background_genes_count;
                if (missing_genes_count > 0)
                {
                    if (!new_file_announced)
                    {
                        new_file_announced = true;
                        if (sb.Length > 0) { sb.AppendFormat("\r\n"); }
                        sb.AppendFormat("'{0}': List of background genes is probably wrong", custom_data_summary_line.Source_fileName);
                    }
                }
            }

            sb.AppendFormat("\r\n\r\n{0}\r\n\r\n", dash_string);

            for (int indexCD = 0; indexCD < custom_data_summary_length; indexCD++)
            {
                custom_data_summary_line = Custom_data_summary.Summaries[indexCD];
                if ((indexCD==0)||(!custom_data_summary_line.Source_fileName.Equals(Custom_data_summary.Summaries[indexCD-1].Source_fileName)))
                {
                    new_file_announced = false;
                }
                missing_genes_count = custom_data_summary_line.Total_genes_count - custom_data_summary_line.Total_genes_among_experimental_background_genes_count;
                if (missing_genes_count > 0)
                {
                    if (!new_file_announced)
                    {
                        new_file_announced = true;
                        if (sb.Length > 0) { sb.AppendFormat("\r\n"); }
                        sb.AppendFormat("Summary for '{0}'", custom_data_summary_line.Source_fileName);
                    }
                    if (sb.Length > 0) { sb.AppendFormat("\r\n\r\n"); }
                    sb.AppendFormat("{0} of {1} genes of the dataset '{2}' uploaded from file '{3}'", missing_genes_count, custom_data_summary_line.Total_genes_count, custom_data_summary_line.UniqueDatasetName, custom_data_summary_line.Source_fileName);
                    sb.AppendFormat("\r\n");
                    sb.AppendFormat("are missing in background list '{0}'", custom_data_summary_line.Exp_bgGenes_list_name);
                }
                if ((indexCD == custom_data_summary_length-1) || (!custom_data_summary_line.Source_fileName.Equals(Custom_data_summary.Summaries[indexCD + 1].Source_fileName)))
                {
                    if (new_file_announced)
                    {
                        sb.AppendFormat("\r\n\r\n{0}\r\n\r\n", dash_string);
                    }
                }
            }
            if (sb.Length > 0) 
            {
                sb.AppendFormat("\r\nExperimental background genes are all genes that have a chance to be identified in a dataset.");
                sb.AppendFormat("\r\nAny missing genes (as shown above) document that the wrong background genes list has been selected for the datasets.");
                sb.AppendFormat("\r\nThe intersection of the experimental and MBCO background genes constitutes the final background genes.");
                sb.AppendFormat("\r\nAll dataset genes and SCP genes that are not part of final background genes will be removed.");
            }
            Error_reports_ownTextBox.SilentText_and_refresh = sb.ToString();
        }

        private void Write_explanation_into_error_reports_panel()
        {
            Error_reports_headline_label.Text = "Explanation of background genes";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Right-tailed Fisher's Exact test");
            sb.AppendFormat("\r\nMBCO uses the right tailed Fisher's Exact test to identify if an experimental gene list is");
            sb.AppendFormat("\r\nenriched for genes annotated to a particular subcellular process (SCP). Briefly, it determines");
            sb.AppendFormat("\r\nthe overlap between an experimental gene list and an SCP gene list and calculates the likelihood");
            sb.AppendFormat("\r\nto observe the same or a higher overlap, if it was by chance. For this, it needs to know the");
            sb.AppendFormat("\r\nshared background list of genes that contains all genes that do have a chance to become part");
            sb.AppendFormat("\r\nof either list.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nOntology background genes");
            sb.AppendFormat("\r\nMBCO background genes are all genes that were identified in at least one PubMed abstract during text");
            sb.AppendFormat("\r\nmining and population of MBCO SCPs. Any gene that is not among the MBCO background genes did not have");
            sb.AppendFormat("\r\na chance to be assigned to any MBCO SCP and should consequently not be considered for the Fisher's");
            sb.AppendFormat("\r\nExcat Test.");
            sb.AppendFormat("\r\nMBCO background genes were also added to the specialized MBCO dataset 'Sodium and glucose transmembrane");
            sb.AppendFormat("\r\ntransport'.");
            sb.AppendFormat("\r\nBackground genes for the three different Gene Ontology (GO) namesspaces are those genes that are annotated");
            sb.AppendFormat("\r\nto any GO biological process, cellular component or molecular function after preparation of each namespace");
            sb.AppendFormat("\r\nfor enrichment analysis (i.e., after removal of GO terms with less than 7 or more than 250 genes).");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nExperimental background genes");
            sb.AppendFormat("\r\nIt is very likely that some of the ontology background genes do not have a chance to be identified");
            sb.AppendFormat("\r\nin an experiment. To further increase the statistical accuracy, experimental background genes");
            sb.AppendFormat("\r\ncan be uploaded as well. Uploaded lists of experimental background genes should contain all genes");
            sb.AppendFormat("\r\nthat have a chance to be identified in a dataset. In an RNAseq experiment, for example, only genes");
            sb.AppendFormat("\r\nthat are annotated to the reference genome can be identified as differentially expressed.");
            sb.AppendFormat("\r\nExperimental background genes can be uploaded using the functionalities of this menu panel or those");
            sb.AppendFormat("\r\nof the menu 'Read data'.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nShared/final background genes");
            sb.AppendFormat("\r\nThe intersection of  experimental and ontology background genes constitutes the final background");
            sb.AppendFormat("\r\ngene list. All experimental and SCP genes that are not part of the final background gene list");
            sb.AppendFormat("\r\nand can consequently never become part of the overlap between experimental and SCP genes will");
            sb.AppendFormat("\r\nbe removed before Fisher's Exact Test.");
            sb.AppendFormat("\r\nIf experimental background genes are not supplied, the final background genes will be the");
            sb.AppendFormat("\r\nontology background genes.");

            Error_reports_ownTextBox.SilentText_and_refresh = sb.ToString();
        }

        private void Update_bgGenes_warnings()
        {
            Dictionary<string, int> uniqueDatasetName_missingGenesCount_dict = Custom_data_summary.Get_uniqueDatasetName_missingGenes_dictionary_that_containGenes_that_are_not_part_of_experimental_bgGenes();
            Form1_resolution_parameter_class rsp = new Form1_resolution_parameter_class();
            if (uniqueDatasetName_missingGenesCount_dict.Keys.ToArray().Length > 0)
            {
                Warnings_button.Text = BgGenes_default_texts_class.Warning_button_label;
                Set_warnings_button_and_label_fontSize_size_and_visibility(true);
            }
            else
            {
                Warnings_button.Text = BgGenes_default_texts_class.Explanation_button_label;
                Set_warnings_button_and_label_fontSize_size_and_visibility(false);
            }
        }

        public void Warnings_button_activated()
        {
            if (Warnings_button.Text.Equals(BgGenes_default_texts_class.Warning_button_label))
            {
                Write_warnings_into_error_reports_panel();
            }
            else if (Warnings_button.Text.Equals(BgGenes_default_texts_class.Explanation_button_label))
            {
                Write_explanation_into_error_reports_panel();
            }
            else { throw new Exception(); }
        }

        public void Analyze_if_all_genes_are_part_of_selected_background_gene_lists(Custom_data_class custom_data, string[] mbco_background_genes)
        {
            custom_data.Set_unique_datasetName_within_whole_custom_data_ignoring_integrationGroups();
            Custom_data_summary = new Custom_data_summary_class();
            Custom_data_summary.Generate_from_custom_data(custom_data, mbco_background_genes);
            Update_bgGenes_warnings();
        }
        #endregion

        #region Read
        public void Add_readAllFilesInDirectory_ownCheckBox_checked()
        {
            this.Add_readOnlyFile_cbButton.SilentChecked = !this.Add_readAllFilesInDirectory_cbButton.Checked;
            Update_readExplainFile_label();
        }
        public void Add_readOnlyFile_ownCheckBox_checked()
        {
            this.Add_readAllFilesInDirectory_cbButton.Checked = !this.Add_readOnlyFile_cbButton.Checked;
            Update_readExplainFile_label();
        }
        public Custom_data_class Add_read_button_pressed(Custom_data_class custom_data, System.Windows.Forms.Label error_report_label)
        {
            string[] completeBgFileNames = new string[0];
            List<Read_error_message_line_class> error_messages_list = new List<Read_error_message_line_class>();
            if (Add_readOnlyFile_cbButton.Checked)
            {
                completeBgFileNames = new string[] { Read_directoryOrFile_ownTextBox.Text };
            }
            else if (Add_readAllFilesInDirectory_cbButton.Checked)
            {
                string directory = Read_directoryOrFile_ownTextBox.Text;
                if (System.IO.Directory.Exists(directory))
                { completeBgFileNames = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(directory)); }
                else
                {
                    Read_error_message_line_class error_message_line = new Read_error_message_line_class();
                    error_message_line.File_type = Read_file_type.Background_genes;
                    error_message_line.Complete_fileName = (string)directory.Clone();
                    error_message_line.Error_message = Read_error_message_enum.Directory_does_not_exist;
                    error_messages_list.Add(error_message_line);
                }
            }
            int completeBgFileNames_length = completeBgFileNames.Length;
            string completeBgFileName;
            string fileName_withoutExtension;
            Read_error_message_line_class[] error_messages;
            for (int indexC=0; indexC<completeBgFileNames_length;indexC++)
            {
                completeBgFileName = completeBgFileNames[indexC];
                fileName_withoutExtension = System.IO.Path.GetFileNameWithoutExtension(completeBgFileName);
                if (fileName_withoutExtension.IndexOf(Global_class.Bg_genes_label) == fileName_withoutExtension.Length - Global_class.Bg_genes_label.Length)
                {
                    error_messages = custom_data.Read_and_add_background_genes_and_return_error_messages(completeBgFileName, error_report_label, Form_default_settings);
                    error_messages_list.AddRange(error_messages);
                }
            }
            Update_available_bgGeneListsName_listBox(custom_data);
            this.Last_error_reports = error_messages_list.ToArray();
            Update_read_error_reports();
            return custom_data;
        }
        #endregion

    }




}

