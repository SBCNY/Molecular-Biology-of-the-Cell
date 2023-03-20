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
using System.Windows.Forms;
using Windows_forms_customized_tools;
using System.Drawing;
using Common_functions.Global_definitions;
using Data;
using Common_functions.ReadWrite;
using Common_functions.Form_tools;

namespace ClassLibrary1.Read_interface
{
    class Read_default_settings_class
    {
        public static string Timepoint_ignored_text {  get { return "Timepoints will be ignored"; } }
        public static string Names_will_be_extended_text {  get { return "Names will be extended by file names"; } }
        public static string EntryType_by_value_text {  get { return "Genes with 1st values >0 or <0 are up- or downregulated, respectively. Genes with 1st values = 0 will be removed."; } }
        public static string EntryType_allowed_types_text {  get { return "Label each gene with 'Up' or 'Down'"; } }
        public static string Value_entryType_missing_text {  get { return "Please specify column name for value or up/down"; } }
        public static string Value_0_will_be_ignored {  get { return "Genes with value = 0 will be ignored"; } }
        public static string GeneSymbol_missing { get { return "Please specify column name for gene symbols"; } }
        public static string Missing_mandatory_columnNames {  get { return "Please define mandatory column name"; } }
        public static string Name_missing { get { return "Please specify column name for dataset name"; } }
        public static string Default_entryType {  get { return "1st values of all genes will be set to 1,\r\nmarking them as upregulated."; } }
        public static string Duplicated_columnNames {  get { return "Duplicated column names"; } }
        public static string ErrorReportsExplanationButtonText_errorText { get { return "Show errors"; } }
        public static string ErrorReportsExplanationButtonText_explanationText { get { return "Explanation"; } }

        public static string Get_delimiter_string(Read_file_delimiter_enum delimiter)
        {
            return delimiter + "-separated file";
        }

        public static Read_file_delimiter_enum Get_delimter_from_string(string delimiter_string)
        {
            return (Read_file_delimiter_enum)Enum.Parse(typeof(Read_file_delimiter_enum), delimiter_string.Replace("-separated file", ""));
        }
    }

    class Read_interface_options_class
    {
        public int Max_documented_errors_per_file { get; set; }
        public Read_interface_options_class()
        {
            Max_documented_errors_per_file = 5;
        }
    }

    class Read_interface_class
    {
        private MyPanel Read_overall_groupBox { get; set; }
        private Label Read_headline_label { get; set; }
        private Label Read_sampleNameColumn_label { get; set; }
        private OwnTextBox Read_sampleNameColumn_ownTextBox { get; set; }
        private Label Read_timepointColumn_label { get; set; }
        private OwnTextBox Read_timepointColumn_ownTextBox { get; set; }
        private Label Read_timeunitColumn_label { get; set; }
        private OwnTextBox Read_timeunitColumn_ownTextBox { get; set; }
        private Label Read_color_label { get; set; }
        private OwnTextBox Read_color_ownTextBox { get; set; }
        private Label Read_value1stColumn_label { get; set; }
        private OwnTextBox Read_value1stColumn_ownTextBox { get; set; }
        private Label Read_value2ndColumn_label { get; set; }
        private OwnTextBox Read_value2ndColumn_ownTextBox { get; set; }
        private Label Read_value1st_explanation_label { get; set; }
        private Label Read_value2nd_explanation_label { get; set; }
        private Label Read_geneSymbol_label { get; set; }
        private OwnTextBox Read_geneSymbol_ownTextBox { get; set; }
        private OwnListBox Read_timepointUnit_ownListBox { get; set; }
        private Label Read_integrationGroup_label { get; set; }
        private OwnTextBox Read_integrationGroup_ownTextBox { get; set; }
        private Label Read_order_label { get; set; }
        private MyCheckBox_button Read_order_allFilesInDirectory_cbButton { get; set; }
        private Label Read_order_allFilesInDirectory_cbLabel { get; set; }
        private MyCheckBox_button Read_order_onlySpecifiedFile_cbButton { get; set; }
        private Label Read_order_onlySpecifiedFile_cbLabel { get; set; }
        private Label Read_directoryOrFile_outside_label { get; set; }
        private OwnTextBox Read_directoryOrFile_outside_ownTextBox { get; set; }
        private Label Read_delimiter_label { get; set; }
        private OwnListBox Read_delimiter_ownListBox { get; set; }
        private Label Read_information_label { get; set; }
        private Label Read_defaultColumnName_label { get; set; }
        private Button Read_setTo_seurat_button { get; set; }
        private Button Read_setTo_example1_button { get; set; }
        private Button Read_setTo_example2_button { get; set; }
        private Button Read_setTo_MBCO_button { get; set; }
        private Button Read_setTo_minimum_button { get; set; }
        private Button Read_setTo_optimum_button { get; set; }
        private Button Read_data_button { get; set; }
        private Label Errors_reports_headline_label { get; set; }
        private Button Error_reports_button { get; set; }
        private Label Error_reports_label {get;set;}
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }
        private Label Error_reports_maxErrorPerFile1_label { get; set; }
        private Label Error_reports_maxErrorPerFile2_label { get; set; }
        private Label Progress_report_label { get; set; }
        private Read_error_message_line_class[] Last_error_reports { get; set; }
        private Read_interface_options_class Options { get; set; }
        public int UploadedFileNo { get; set; }
        public Form1_default_settings_class Form_default_settings { get; set; }
        private Global_directory_and_file_class Global_dirFile { get; set; }

        public Read_interface_class(MyPanel read_overall_panel,
                                    Label read_headline_label,
                                    Label read_sampleNameColumn_label,
                                    OwnTextBox read_sampleNameColumn_ownTextBox,
                                    Label read_timepointColumn_label,
                                    OwnTextBox read_timepointColumn_ownTextBox,
                                    Label read_timeunitColumn_label,
                                    OwnTextBox read_timeunitColumn_ownTextBox,
                                    Label read_color_label,
                                    OwnTextBox read_color_ownTextBox,
                                    Label read_geneSymbol_label,
                                    OwnTextBox read_geneSymbol_ownTextBox,
                                    Label read_value1stColumn_label,
                                    OwnTextBox read_value1stColumn_ownTextBox,
                                    Label read_value1st_explanation_label,
                                    Label read_value2ndColumn_label,
                                    OwnTextBox read_value2ndColumn_ownTextBox,
                                    Label read_value2nd_explanation_label,
                                    Label read_integrationGroup_label,
                                    OwnTextBox read_integrationGroup_ownTextBox,
                                    OwnListBox read_timepointUnit_ownListBox,
                                    Label read_delimiter_label,
                                    OwnListBox read_delimiter_ownListBox,
                                    Label read_order_label,
                                    MyCheckBox_button read_order_allFilesInDirectory_cbButton,
                                    Label read_order_allFilesInDirectory_cbLabel,
                                    MyCheckBox_button read_order_onlySpecifiedFile_cbButton,
                                    Label read_order_onlySpecifiedFile_cbLabel,
                                    Label read_directoryOrFile_outside_label,
                                    OwnTextBox read_directoryOrFile_outside_ownTextBox,
                                    Label read_information_label,
                                    Label read_defaultColumnName_label,
                                    Button read_setTo_example1_button,
                                    Button read_setTo_example2_button,
                                    Button read_setTo_MBCO_button,
                                    Button read_setTo_seurat_button,
                                    Button read_setTo_minimum_button,
                                    Button read_setTo_optimum_button,
                                    Button read_data_button,
                                    Label errors_reportsHeadline_label,
                                    Button error_reports_button,
                                    Label error_reports_label,
                                    OwnTextBox error_reports_ownTextBox,
                                    Label error_reports_maxErrorPerFile1_label,
                                    Label error_reports_maxErrorPerFile2_label,
                                    OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,
                                    Label progress_report_label,
                                    Form1_default_settings_class form_default_settings)

        {
            Global_dirFile = new Global_directory_and_file_class();
            this.Form_default_settings = form_default_settings;
            this.Read_overall_groupBox = read_overall_panel;
            this.Read_headline_label = read_headline_label;
            this.Read_sampleNameColumn_label = read_sampleNameColumn_label;
            this.Read_sampleNameColumn_ownTextBox = read_sampleNameColumn_ownTextBox;
            this.Read_timepointColumn_label = read_timepointColumn_label;
            this.Read_timepointColumn_ownTextBox = read_timepointColumn_ownTextBox;
            this.Read_timeunitColumn_label = read_timeunitColumn_label;
            this.Read_timeunitColumn_ownTextBox = read_timeunitColumn_ownTextBox;
            this.Read_integrationGroup_label = read_integrationGroup_label;
            this.Read_integrationGroup_ownTextBox = read_integrationGroup_ownTextBox;
            this.Read_color_label = read_color_label;
            this.Read_color_ownTextBox = read_color_ownTextBox;
            this.Read_geneSymbol_label = read_geneSymbol_label;
            this.Read_geneSymbol_ownTextBox = read_geneSymbol_ownTextBox;
            this.Read_value1stColumn_label = read_value1stColumn_label;
            this.Read_value1stColumn_ownTextBox = read_value1stColumn_ownTextBox;
            this.Read_value2ndColumn_label = read_value2ndColumn_label;
            this.Read_value2ndColumn_ownTextBox = read_value2ndColumn_ownTextBox;
            this.Read_order_label = read_order_label;
            this.Read_order_allFilesInDirectory_cbButton = read_order_allFilesInDirectory_cbButton;
            this.Read_order_allFilesInDirectory_cbLabel = read_order_allFilesInDirectory_cbLabel;
            this.Read_order_onlySpecifiedFile_cbButton = read_order_onlySpecifiedFile_cbButton;
            this.Read_order_onlySpecifiedFile_cbLabel = read_order_onlySpecifiedFile_cbLabel;
            this.Read_data_button = read_data_button;
            this.Read_timepointUnit_ownListBox = read_timepointUnit_ownListBox;
            this.Read_directoryOrFile_outside_label = read_directoryOrFile_outside_label;
            this.Read_directoryOrFile_outside_ownTextBox = read_directoryOrFile_outside_ownTextBox;
            this.Read_information_label = read_information_label;
            this.Error_reports_button = error_reports_button;
            this.Error_reports_label = error_reports_label;
            this.Errors_reports_headline_label = errors_reportsHeadline_label;
            this.Error_reports_ownTextBox = error_reports_ownTextBox;
            this.Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            this.Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            this.Progress_report_label = progress_report_label;
            this.Read_setTo_example1_button = read_setTo_example1_button;
            this.Read_setTo_example2_button = read_setTo_example2_button;
            this.Read_setTo_seurat_button = read_setTo_seurat_button;
            this.Read_setTo_MBCO_button = read_setTo_MBCO_button;
            this.Read_setTo_minimum_button = read_setTo_minimum_button;
            this.Read_setTo_optimum_button = read_setTo_optimum_button;
            this.Read_defaultColumnName_label = read_defaultColumnName_label;
            this.Read_value1st_explanation_label = read_value1st_explanation_label;
            this.Read_value2nd_explanation_label = read_value2nd_explanation_label;
            this.Read_delimiter_ownListBox = read_delimiter_ownListBox;
            this.Read_delimiter_label = read_delimiter_label;
            this.Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;

            Initialize_checkBox_buttons_and_listBoxes();
            Update_all_graphic_elements();

        }

        public void Update_all_graphic_elements()
        {
            this.Read_overall_groupBox = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Read_overall_groupBox);
            int current_right_position;
            int current_left_position;
            int current_top_position;
            int current_bottom_position;
            int min_left_position = 0;
            OwnTextBox[] textBoxes;
            OwnTextBox my_textBox;
            Label my_label;
            OwnListBox my_listBox;
            Button my_button;
            MyCheckBox_button my_cbButton;
            int textBoxes_length;
            int max_right_position = this.Read_overall_groupBox.Size.Width;
            int min_top_position = 0;

            Dictionary<OwnTextBox, Label> ownTextBox_labelLeftOfTextBox_dict = new Dictionary<OwnTextBox, Label>();
            Dictionary<OwnTextBox, Label> ownTextBox_labelRightOfTextBox_dict = new Dictionary<OwnTextBox, Label>();

            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_sampleNameColumn_ownTextBox, this.Read_sampleNameColumn_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_timepointColumn_ownTextBox, this.Read_timepointColumn_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_timeunitColumn_ownTextBox, this.Read_timeunitColumn_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_integrationGroup_ownTextBox, this.Read_integrationGroup_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_color_ownTextBox, this.Read_color_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_geneSymbol_ownTextBox, this.Read_geneSymbol_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_value1stColumn_ownTextBox, this.Read_value1stColumn_label);
            ownTextBox_labelLeftOfTextBox_dict.Add(this.Read_value2ndColumn_ownTextBox, this.Read_value2ndColumn_label);

            #region Adjust all text boxes
            textBoxes = ownTextBox_labelLeftOfTextBox_dict.Keys.ToArray();
            textBoxes_length = textBoxes.Length;
            current_left_position = (int)Math.Round(0.27F * Read_overall_groupBox.Width);
            current_right_position = (int)Math.Round(0.8F * Read_overall_groupBox.Width);
            int height_per_textBox = (int)Math.Round(0.45F * Read_overall_groupBox.Height / 8);
            current_bottom_position = (int)Math.Round(0.05F * Read_overall_groupBox.Height);
            for (int indexTB = 0; indexTB < textBoxes_length; indexTB++)
            {
                my_textBox = textBoxes[indexTB];
                current_top_position = current_bottom_position;
                current_bottom_position = current_top_position + height_per_textBox;
                my_textBox = Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, current_left_position,current_right_position,current_top_position,current_bottom_position);
            }
            int half_distance_between_boxes = (int)Math.Round(0.5 * (  this.Read_timepointColumn_ownTextBox.Location.Y
                                                                     - this.Read_sampleNameColumn_ownTextBox.Height
                                                                     - this.Read_sampleNameColumn_ownTextBox.Location.Y));
            #endregion

            #region Adjust headline label
            my_label = Read_headline_label;
            my_textBox = Read_sampleNameColumn_ownTextBox;
            current_left_position = min_left_position;
            current_right_position = max_right_position;
            current_top_position = min_top_position;
            current_bottom_position = my_textBox.Location.Y - half_distance_between_boxes;
            my_label = Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Adjust all default labels left of text boxes
            textBoxes = ownTextBox_labelLeftOfTextBox_dict.Keys.ToArray();
            textBoxes_length = textBoxes.Length;
            for (int indexTB = 0; indexTB < textBoxes_length; indexTB++)
            {
                my_textBox = textBoxes[indexTB];
                my_label = ownTextBox_labelLeftOfTextBox_dict[my_textBox];
                current_left_position = min_left_position;
                current_right_position = my_textBox.Location.X;
                current_top_position = my_textBox.Location.Y - half_distance_between_boxes;
                current_bottom_position = my_textBox.Location.Y + my_textBox.Height + half_distance_between_boxes;
                my_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            }
            #endregion

            #region Adjust all listBoxes and explanatory labels right of text boxes
            ownTextBox_labelRightOfTextBox_dict.Add(this.Read_value1stColumn_ownTextBox, this.Read_value1st_explanation_label);
            ownTextBox_labelRightOfTextBox_dict.Add(this.Read_value2ndColumn_ownTextBox, this.Read_value2nd_explanation_label);

            textBoxes = ownTextBox_labelRightOfTextBox_dict.Keys.ToArray();
            textBoxes_length = textBoxes.Length;
            for (int indexTB = 0; indexTB < textBoxes_length; indexTB++)
            {
                my_textBox = textBoxes[indexTB];
                my_label = ownTextBox_labelRightOfTextBox_dict[my_textBox];
                current_left_position = my_textBox.Location.X + my_textBox.Width;
                current_right_position = max_right_position;
                current_top_position = my_textBox.Location.Y;// - (int)Math.Round(0.5*half_distance_between_boxes);
                current_bottom_position = my_textBox.Location.Y + my_textBox.Height;// + (int)Math.Round(0.5*half_distance_between_boxes);
                my_label = Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            }

            my_textBox = this.Read_timepointColumn_ownTextBox;
            current_left_position = my_textBox.Location.X + my_textBox.Width;
            current_right_position = max_right_position;
            current_top_position = my_textBox.Location.Y;// - (int)Math.Round(0.5*half_distance_between_boxes);
            current_bottom_position = my_textBox.Location.Y + my_textBox.Height;// + (int)Math.Round(0.5*half_distance_between_boxes);
            my_listBox = Read_timepointUnit_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            int shared_delimiterListBox_defaultColumnNamesButton_readCheckBox_left_position = (int)Math.Round(0.35F * Read_overall_groupBox.Width);
            int topPostion_for_readDelimiter_checkBox_and_attached_defaultColumnNames_boxes = (int)Math.Round(0.57F * Read_overall_groupBox.Height);

            #region Adjust read delimiter listBox and label
            current_left_position = shared_delimiterListBox_defaultColumnNamesButton_readCheckBox_left_position;
            current_right_position = (int)Math.Round(0.85F * Read_overall_groupBox.Width);
            current_top_position = topPostion_for_readDelimiter_checkBox_and_attached_defaultColumnNames_boxes;
            current_bottom_position = (int)Math.Round(0.62F * Read_overall_groupBox.Height);
            my_listBox = this.Read_delimiter_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, current_left_position, current_right_position, current_top_position, current_bottom_position);
            current_left_position = min_left_position;
            current_right_position = my_listBox.Location.X;
            current_top_position = my_listBox.Location.Y;
            current_bottom_position = my_listBox.Location.Y + my_listBox.Height + half_distance_between_boxes;
            my_label = this.Read_delimiter_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Default column names buttons
            int defaultColumnNames_button_left_position = shared_delimiterListBox_defaultColumnNamesButton_readCheckBox_left_position;
            int defaultColumnNames_button_top_position = this.Read_delimiter_ownListBox.Location.Y + Read_delimiter_ownListBox.Height + half_distance_between_boxes;
            int defaultColumnNames_button_width = (int)Math.Round(0.21F * Read_overall_groupBox.Width);
            int defaultColumnNames_button_height = (int)Math.Round(0.05F * Read_overall_groupBox.Height);

            current_left_position = defaultColumnNames_button_left_position;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = defaultColumnNames_button_top_position;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_example1_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = defaultColumnNames_button_left_position;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = this.Read_setTo_example1_button.Location.Y + this.Read_setTo_example1_button.Height;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_example2_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_example1_button.Location.X + this.Read_setTo_example1_button.Width;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = defaultColumnNames_button_top_position;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_seurat_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_example2_button.Location.X + this.Read_setTo_example2_button.Width;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = this.Read_setTo_seurat_button.Location.Y + this.Read_setTo_seurat_button.Height;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_MBCO_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_seurat_button.Location.X + this.Read_setTo_seurat_button.Width;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = defaultColumnNames_button_top_position;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_minimum_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_MBCO_button.Location.X + this.Read_setTo_MBCO_button.Width;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = this.Read_setTo_minimum_button.Location.Y + this.Read_setTo_minimum_button.Height;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_optimum_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Adjust read default column name label
            my_label = this.Read_defaultColumnName_label;
            current_left_position = min_left_position;
            current_right_position = this.Read_setTo_example1_button.Location.X;
            current_top_position = this.Read_setTo_example1_button.Location.Y + (int)Math.Round(0.075 * this.Read_setTo_example1_button.Height); ;
            current_bottom_position = this.Read_setTo_example2_button.Location.Y + (int)Math.Round(0.925 * this.Read_setTo_example2_button.Height);
            my_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Adjust read order checkboxes and label
            int widthHeight_of_one_checkBox = (int)Math.Round(0.05F * Read_overall_groupBox.Height);
            current_left_position = shared_delimiterListBox_defaultColumnNamesButton_readCheckBox_left_position;
            current_right_position = current_left_position + widthHeight_of_one_checkBox;
            current_top_position = Read_setTo_example2_button.Location.Y + Read_setTo_example2_button.Height;
            current_bottom_position = current_top_position + widthHeight_of_one_checkBox;
            my_cbButton = this.Read_order_allFilesInDirectory_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = shared_delimiterListBox_defaultColumnNamesButton_readCheckBox_left_position;
            current_right_position = current_left_position + widthHeight_of_one_checkBox;
            current_top_position = Read_order_allFilesInDirectory_cbButton.Location.Y + Read_order_allFilesInDirectory_cbButton.Height;
            current_bottom_position = current_top_position + widthHeight_of_one_checkBox;
            my_cbButton = this.Read_order_onlySpecifiedFile_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = Read_order_allFilesInDirectory_cbButton.Location.X + Read_order_allFilesInDirectory_cbButton.Width;
            current_right_position = this.Read_overall_groupBox.Width;
            current_top_position = Read_order_allFilesInDirectory_cbButton.Location.Y;
            current_bottom_position = Read_order_allFilesInDirectory_cbButton.Location.Y + Read_order_allFilesInDirectory_cbButton.Height;
            my_label = this.Read_order_allFilesInDirectory_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = Read_order_onlySpecifiedFile_cbButton.Location.X + Read_order_onlySpecifiedFile_cbButton.Width;
            current_right_position = this.Read_overall_groupBox.Width;
            current_top_position = Read_order_onlySpecifiedFile_cbButton.Location.Y;
            current_bottom_position = Read_order_onlySpecifiedFile_cbButton.Location.Y + Read_order_onlySpecifiedFile_cbButton.Height;
            my_label = this.Read_order_onlySpecifiedFile_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = min_left_position;
            current_right_position = this.Read_order_allFilesInDirectory_cbButton.Location.X;
            current_top_position = this.Read_order_allFilesInDirectory_cbButton.Location.Y + (int)Math.Round(0.5F * widthHeight_of_one_checkBox);
            current_bottom_position = this.Read_order_onlySpecifiedFile_cbButton.Location.Y + this.Read_order_onlySpecifiedFile_cbButton.Height - (int)Math.Round(0.5F * widthHeight_of_one_checkBox);
            my_label = this.Read_order_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Adjust Read data and error reports buttons
            current_top_position = (int)Math.Round(0.93F * this.Read_overall_groupBox.Height);
            current_bottom_position = (int)Math.Round(0.99F * this.Read_overall_groupBox.Height);
            int button_inner_distance_from_leftRightSide = (int)Math.Round(0.4F * this.Read_overall_groupBox.Width);
            int button_outer_distance_from_leftRightSide = (int)Math.Round(0.05F * this.Read_overall_groupBox.Width);

            current_left_position = this.Read_overall_groupBox.Width - button_inner_distance_from_leftRightSide;
            current_right_position = this.Read_overall_groupBox.Width - button_outer_distance_from_leftRightSide;
            my_button = this.Read_data_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = button_outer_distance_from_leftRightSide;
            current_right_position = button_inner_distance_from_leftRightSide;
            my_button = this.Error_reports_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Read information label
            Adjust_sizes_of_readInformation_label();
            #endregion

            #region Error report label
            Adjust_sizes_of_error_reports_label();
            #endregion

            this.Error_reports_maxErrorsPerFile_ownTextBox = Form_default_settings.MyTextBoxSingleLine_updateCoordinates_add_default_parameter(Error_reports_maxErrorsPerFile_ownTextBox);
            
            this.Options = new Read_interface_options_class();
            //Read_delimiter_ownListBox.Height = 20;
            //Read_timepointUnit_ownListBox.Height = 20;
            Set_to_default();
            Set_visibility(false);
        }

        private void Adjust_sizes_of_readInformation_label()
        {
            int distance = (int)Math.Round(0.5F*(float)(Read_timeunitColumn_ownTextBox.Location.Y - Read_timepointColumn_ownTextBox.Location.Y - Read_timepointColumn_ownTextBox.Height));
            int current_left_position = 0;
            int current_right_position = Read_overall_groupBox.Width;
            int current_top_position = Read_value2ndColumn_ownTextBox.Location.Y + Read_value2ndColumn_ownTextBox.Height + distance;
            int current_bottom_position = Read_delimiter_ownListBox.Location.Y - distance;
            this.Read_information_label = Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(Read_information_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
        }

        private void Adjust_sizes_of_error_reports_label()
        {
            Label label = this.Error_reports_label;
            int current_left_position = 0;
            int current_right_position = Read_overall_groupBox.Width;
            int current_top_position = Read_order_onlySpecifiedFile_cbButton.Location.Y + Read_order_onlySpecifiedFile_cbButton.Height;
            int current_bottom_position = Error_reports_button.Location.Y;
            label = Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(label, current_left_position, current_right_position, current_top_position, current_bottom_position);
        }

        private void Initialize_checkBox_buttons_and_listBoxes()
        {
            this.Read_order_allFilesInDirectory_cbButton.Checked = true;
            this.Read_order_onlySpecifiedFile_cbButton.Checked = !this.Read_order_allFilesInDirectory_cbButton.Checked;

            var all_timeunits = Enum.GetValues(typeof(Timeunit_enum));
            foreach (var timeunit in all_timeunits)
            {
                if (!timeunit.Equals(Timeunit_enum.E_m_p_t_y))
                {
                    Read_timepointUnit_ownListBox.Items.Add(timeunit);
                }
            }
            Read_timepointUnit_ownListBox.SelectionMode = SelectionMode.One;
            var all_delimiters = Enum.GetValues(typeof(Read_file_delimiter_enum));
            Read_delimiter_ownListBox.Items.Clear();
            foreach (var delimiter in all_delimiters)
            {
                if (!delimiter.Equals(Read_file_delimiter_enum.E_m_p_t_y))
                {
                    Read_delimiter_ownListBox.Items.Add(Read_default_settings_class.Get_delimiter_string((Read_file_delimiter_enum)delimiter));
                }
            }
            Read_delimiter_ownListBox.SelectionMode = SelectionMode.One;

        }

        public void Set_to_default()
        {
            this.Read_data_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_data_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Error_reports_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Error_reports_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_example1_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_example1_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_example2_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_example2_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_MBCO_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_MBCO_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_minimum_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_minimum_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_optimum_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_optimum_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_seurat_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_seurat_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Last_error_reports = new Read_error_message_line_class[0];

            Global_directory_and_file_class global_directoryFile = new Global_directory_and_file_class();
            this.Read_directoryOrFile_outside_ownTextBox.SilentText = global_directoryFile.Custom_data_directory + "Enter_data_subdirectory\\";
            SetTo_exampleData1_button_clicked();
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
            Update_error_reports_or_explanations();
        }

        public void Set_visibility(bool is_visibile)
        {
            Read_overall_groupBox.Visible = false;
            Error_reports_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Error_reports_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Read_directoryOrFile_outside_label.Visible = is_visibile;
            Read_directoryOrFile_outside_ownTextBox.Visible = is_visibile;
            Errors_reports_headline_label.Text = "Error messages";
            Errors_reports_headline_label.Refresh();

            Update_error_reports_or_explanations();
            Update_directoryOrFile_label();
            Read_overall_groupBox.Visible = is_visibile;
        }

        #region Update labels
        private void Set_all_textBox_and_label_backcolors_to_regular()
        {
            Read_geneSymbol_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_sampleNameColumn_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_timepointColumn_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_timepointUnit_ownListBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_value1stColumn_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_value2ndColumn_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_integrationGroup_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            Read_information_label.ForeColor = Form_default_settings.ExplanationText_color;
            Error_reports_label.ForeColor = Form_default_settings.ExplanationText_color;
        }
        private bool Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref Dictionary<string, OwnTextBox> columnName_ownTextBox_dict, OwnTextBox ownTextBox)
        {
            bool duplicated = false;
            if (String.IsNullOrEmpty(ownTextBox.Text))
            {
                ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else if (!columnName_ownTextBox_dict.ContainsKey(ownTextBox.Text))
            {
                columnName_ownTextBox_dict.Add(ownTextBox.Text, ownTextBox);
                ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else
            {
                ownTextBox.BackColor = Form_default_settings.Color_warning;
                columnName_ownTextBox_dict[ownTextBox.Text].BackColor = Form_default_settings.Color_warning;
                duplicated = true;
            }
            return duplicated;
        }
        private bool Analyze_if_selected_column_names_are_duplicated_and_set_backcolors_accordingly()
        {
            bool at_least_one_duplicated = false;
            bool duplicated = false;
            Dictionary<string, OwnTextBox> columnName_ownTextBox_dict = new Dictionary<string, OwnTextBox>();
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_geneSymbol_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_sampleNameColumn_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_timepointColumn_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_timeunitColumn_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_value1stColumn_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_integrationGroup_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            duplicated = Add_ownTextBox_to_dict_analyze_if_duplicated_and_adopt_backcolor(ref columnName_ownTextBox_dict, Read_value2ndColumn_ownTextBox);
            at_least_one_duplicated = at_least_one_duplicated || duplicated;
            return at_least_one_duplicated;
        }
        private bool Analyze_if_mandatory_columnNames_defined_and_set_backcolors_accordingly()
        {
            bool mandatory_columnNames_defined = true;
            //if (String.IsNullOrEmpty(Read_sampleNameColumn_ownTextBox.Text))
            //{
            //    mandatory_columnNames_defined = false;
            //}
            if (String.IsNullOrEmpty(Read_geneSymbol_ownTextBox.Text))
            {
                mandatory_columnNames_defined = false;
            }
            return mandatory_columnNames_defined;
        }
        private bool Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible()
        {
            bool proceedToReadIsPossible = true;
            Set_all_textBox_and_label_backcolors_to_regular();
            bool mandatory_columnNames_defined = Analyze_if_mandatory_columnNames_defined_and_set_backcolors_accordingly();
            if (!mandatory_columnNames_defined)
            {
                Read_geneSymbol_ownTextBox.BackColor = Form_default_settings.Color_warning;
                //Read_sampleNameColumn_ownTextBox.BackColor = Read_default_settings_class.Warning_color;
                Read_information_label.BackColor = Form_default_settings.Color_warning;
                Read_information_label.Text = Read_default_settings_class.Missing_mandatory_columnNames;
                proceedToReadIsPossible = false;
            }
            else
            {
                bool duplicated = Analyze_if_selected_column_names_are_duplicated_and_set_backcolors_accordingly();
                if (duplicated)
                {
                    Read_information_label.Text = Read_default_settings_class.Duplicated_columnNames;
                    Read_information_label.BackColor = Form_default_settings.Color_warning;
                    proceedToReadIsPossible = false;
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.Read_value1stColumn_ownTextBox.Text))
                    {
                        Read_information_label.Text = Read_default_settings_class.EntryType_by_value_text;
                        Read_information_label.BackColor = Form_default_settings.Color_label_backColor;
                    }
                    else
                    {
                        Read_information_label.Text = Read_default_settings_class.Default_entryType;
                        Read_information_label.BackColor = Form_default_settings.Color_label_backColor;
                    }
                }
            }
            Adjust_sizes_of_readInformation_label();
            if (  (!String.IsNullOrEmpty(Read_timepointColumn_ownTextBox.Text))
                &&(String.IsNullOrEmpty(Read_timeunitColumn_ownTextBox.Text)))
            {
                this.Read_timepointUnit_ownListBox.Visible = true;
            }
            else
            {
                this.Read_timepointUnit_ownListBox.Visible = false;
            }
            return proceedToReadIsPossible;
        }
        #endregion

        #region Column names changed
        private void Update_directoryOrFile_label()
        {
            if ((!Read_order_onlySpecifiedFile_cbButton.Checked) && (Read_order_allFilesInDirectory_cbButton.Checked))
            {
                this.Read_directoryOrFile_outside_label.Text = "Read all data files in directory:";
            }
            else if ((Read_order_onlySpecifiedFile_cbButton.Checked) && (!Read_order_allFilesInDirectory_cbButton.Checked))
            {
                this.Read_directoryOrFile_outside_label.Text = "Read data file:";
            }
            else { throw new Exception(); }
        }
        public void Read_allFilesInDirectory_ownCheckedBox_clicked()
        {
            Read_order_onlySpecifiedFile_cbButton.SilentChecked = !Read_order_allFilesInDirectory_cbButton.Checked;
            Update_directoryOrFile_label();
        }
        public void Read_onlySpecifiedFile_ownCheckedBox_clicked()
        {
            Read_order_allFilesInDirectory_cbButton.SilentChecked = !Read_order_onlySpecifiedFile_cbButton.Checked;
            Update_directoryOrFile_label();
        }
        public void ColumName_specification_changed()
        {
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        #endregion

        #region Set to default column names buttons
        public void SetTo_exampleData1_button_clicked()
        {
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Dataset_name";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "NCBI_official_gene_symbol";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_color_ownTextBox.SilentText = "Dataset_color";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "Log2_fold_change";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "Minus_log10_pval_or_adj_pval";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "Integration_group";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_exampleData2_button_clicked()
        {
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Dataset_name";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "NCBI_official_gene_symbol";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "Timepoint";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "Timeunit";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "Log2_fold_change";
            Read_color_ownTextBox.SilentText = "Dataset_color";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "Minus_log10_pval_or_adj_pval";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "Integration_group";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_mbco_button_clicked()
        {
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Dataset name";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "NCBI official gene symbol";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "Timepoint";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "Timeunit";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "Value_1st";
            Read_color_ownTextBox.SilentText = "Dataset color";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "Value_2nd";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "Integration group";

            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_Seurat_button_clicked()
        {
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "cluster";
            Read_color_ownTextBox.SilentText = "";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "gene";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "avg_log2FC";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "p_val_adj";
            Read_color_ownTextBox.SilentText = "";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_minimum_button_clicked()
        {
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "Gene";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "";
            Read_color_ownTextBox.SilentText = "";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_optimum_button_clicked()
        {
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Name";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "Gene";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "Value";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "";
            Read_color_ownTextBox.SilentText = "";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        #endregion

        #region Error reports, Explanation
        public void Error_reports_maxErrorsPerFile_ownTextBox_TextChanged()
        {
            int new_number; 
            if (int.TryParse(Error_reports_maxErrorsPerFile_ownTextBox.Text, out new_number))
            {
                Options.Max_documented_errors_per_file = new_number;
                Update_error_reports_or_explanations();
            }
        }

        private void Add_explanations_to_error_reportBox()
        {
            Errors_reports_headline_label.Text = "Read data";
            Errors_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Definition of column field names for user-supplied data files");
            sb.AppendFormat("\r\nTo upload data spreadsheets into the application the user needs to specify the names of the columns");
            sb.AppendFormat("\r\nthat contain the indicated values listed below the headline 'Enter column names of fields in data files'.");
            sb.AppendFormat("\r\nFields with empty text boxes will be ignored. If a spreadsheet does not contain a specified column name");
            sb.AppendFormat("\r\nthe application will stop reading that file and generate an error message that the user can read after");
            sb.AppendFormat("\r\npressing the button 'Show errors' (that will replace the button 'Explanation'). The user can either update");
            sb.AppendFormat("\r\nthe column name or delete it from the corresponding text box.");
            sb.AppendFormat("\r\nName: Name of the dataset");
            sb.AppendFormat("\r\nTimepoint: Values in the column specified by this text box will be uploaded into the timepoint field.");
            sb.AppendFormat("\r\nTime unit: A column that allows specification of a time unit. If the time unit text box is empty, but the");
            sb.AppendFormat("\r\n           timepoint text box contains a column name, a list box will appear that allows selection of a");
            sb.AppendFormat("\r\n           default time unit. The list box also contains all timeunits that can be selected: sec, min, hrs,");
            sb.AppendFormat("\r\n           days, weeks, months, years.");
            sb.AppendFormat("\r\nIntegration group: Datasets can be assigned to integration groups. Generated heatmaps will allow comparison");
            sb.AppendFormat("\r\n                   of those datasets that are assigned to the same integration group. Timeline figues will");
            sb.AppendFormat("\r\n                   contain the enrichment results for one subcellular process (SCP) of interest for all");
            sb.AppendFormat("\r\n                   datasets of the same integration group. SCP networks of all datasets assigned to the");
            sb.AppendFormat("\r\n                   same integration group will be merged.");
            sb.AppendFormat("\r\nColor: The user can specify dataset-specific colors. If selected in the menu 'Enrichment', bardiagrams and");
            sb.AppendFormat("\r\n       timelines will be colored by the user-selected colors. The SCP networks show all significant SCPs");
            sb.AppendFormat("\r\n       predicted for each dataset of the same integration group. SCPs will be visualized as circles that");
            sb.AppendFormat("\r\n       are colored in the user-selected colors. If an SCP is predicted by multiple datasets, each dataset");
            sb.AppendFormat("\r\n       will be visualized as a pie-slice colored in the dataset-specific colors. The user-selected colors");
            sb.AppendFormat("\r\n       can either be uploaded or specified within the application. For quick color assignment to multiple");
            sb.AppendFormat("\r\n       datasets use the menu 'Organize data'. Color maps that link colors to color names can be found by");
            sb.AppendFormat("\r\n       searching the internet for 'C# colors'.");
            sb.AppendFormat("\r\nGene symbol: This text box specifies the name of the column that contains the official NCBI gene symbols.");
            sb.AppendFormat("\r\n             It is the only text box that always needs to be assigned to a column name when uploading");
            sb.AppendFormat("\r\n             data files.");
            sb.AppendFormat("\r\n1st Value: The column specified in this text box, contains those values that define if a gene is up- or");
            sb.AppendFormat("\r\n           downregulated. In case of a typical transcriptomics or proteomics experiment it should contain");
            sb.AppendFormat("\r\n           the log2(fold changes). Genes with positive or negative 1st values will automatically be labeled");
            sb.AppendFormat("\r\n           as up- or downregulated, respectively. Genes with 1st values of zero will be removed. Up- and");
            sb.AppendFormat("\r\n           downregulated genes will become part of different datasets. These two datasets have the same names");
            sb.AppendFormat("\r\n           and timepoints, but differ in their Up/Down status. If this text box is left empty, the 1st value of");
            sb.AppendFormat("\r\n           all uploaded genes will be assigned as one. The genes will consequently be labeled as upregulated.");
            sb.AppendFormat("\r\n2nd Value: The user can specify another column containing experimental values. In contrast to the 1st value");
            sb.AppendFormat("\r\n           the 2nd value has no influence on the dataset assignment, but can be used to define significant");
            sb.AppendFormat("\r\n           genes. The menu 'Set data cutoffs' allows definition of significance and rank cutoffs using the 1st");
            sb.AppendFormat("\r\n           and 2nd values. In a typical transcriptomic or proteomic experiment, the 2nd value column should");
            sb.AppendFormat("\r\n           contain p-values or adjusted p-values.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nDefault column names");
            sb.AppendFormat("\r\nThe user can select default column name entries. Pressing one of these buttons will change the text box entries");
            sb.AppendFormat("\r\nabove.");
            sb.AppendFormat("\r\nThe 'MBCO' default column names allow re-upload of analyzed data into the application. Any data that was subjected");
            sb.AppendFormat("\r\nto analysis by pressing the 'Analyze' button in the middle of the application will be saved in the results folder");
            sb.AppendFormat("\r\n'Input_data'. The user can reupload this data into the application after selecting the 'MBCO' default column names.");
            sb.AppendFormat("\r\nIf the analyzed data does not contain different timepoints, timepoints will not be saved in the results folder.");
            sb.AppendFormat("\r\nwIn this case, pressing the read button will generate an error stating that the columns assigned to 'Timepoint'");
            sb.AppendFormat("\r\nand 'Time unit' do not exist. Simply delete the entries in the 'Timepoint' and 'Time unit' text boxes and press");
            sb.AppendFormat("\r\nthe 'Read' button again.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nRead all files/only specified file");
            sb.AppendFormat("\r\nFor quick upload of multiple data files the user can select that all files in a particular directory will be read.");
            sb.AppendFormat("\r\nIf selected, this mode will search for data files, for files with background genes and for files that were written");
            sb.AppendFormat("\r\nby the application and contain user-selected parameter settings.");
            sb.AppendFormat("\r\nData files have to contain the column names specified below 'Enter column names of fields in data files'. The file name");
            sb.AppendFormat("\r\nthat contained each uploaded dataset will be saved in the 'Source' field (see menu 'Organize data').");
            sb.AppendFormat("\r\nFiles containing background genes have to end with 'bgGenes' and should contain one column with genes and no headline.");
            sb.AppendFormat("\r\nAfter upload of background genes, the application will search for uploaded data files with the same name (as saved in the");
            sb.AppendFormat("\r\nsource field), except the 'bgGenes' ending. It will automatically assign all background genes to those datasets that");
            sb.AppendFormat("\r\nwere part of the matching data file. The automatic upload and mapping of background genes mimics the manual upload and");
            sb.AppendFormat("\r\nmapping in the menu 'Background genes', using the buttons 'Read' and 'Automatically assign background genes'. Please");
            sb.AppendFormat("\r\nconsider that the background genes should contain all genes that did have a chance of being identified. Consequently,");
            sb.AppendFormat("\r\nthey have to contain all genes that are part of the data file. For more details, please read the explanation in the");
            sb.AppendFormat("\r\nmenu 'Background genes'.");
            sb.AppendFormat("\r\nFiles containing user-selected parameter settings are generated by the application after pressing of the 'Analyze'-button");
            sb.AppendFormat("\r\nin the middle of the application window. Before enrichment analysis the application will save all datasets, background");
            sb.AppendFormat("\r\ngenes and parameter settings in the results folder 'Input_data'. To re-import those files into the application, the user");
            sb.AppendFormat("\r\ncan select 'Read all files' and copy-paste the complete name of the 'Input_data' directory into the text box 'Read all");
            sb.AppendFormat("\r\ndata files in directory'. Selecting the 'MBCO'-default column names button will set the correct column names. Pressing");
            sb.AppendFormat("\r\nthe 'Read'-button will automatically reimport the analyzed data, background genes and parameter settings into the");
            sb.AppendFormat("\r\napplication. This allows quick reproduction of results and recapitulation of parameter settings.");
            
            Error_reports_ownTextBox.SilentText_and_refresh = sb.ToString();
        }

        private void Update_error_reports_or_explanations()
        {
            Error_reports_maxErrorPerFile1_label.Visible = true;
            Error_reports_maxErrorPerFile2_label.Visible = true;
            Error_reports_maxErrorsPerFile_ownTextBox.SilentText = Options.Max_documented_errors_per_file.ToString();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = true;
            this.Last_error_reports = this.Last_error_reports.OrderBy(l => l.Complete_fileName).ToArray();
            int last_error_reports_length = this.Last_error_reports.Length;
            Read_error_message_line_class error_message_line;
            int files_with_error_reports = 0;
            int files_with_parameter_settings = 0;
            int error_reports_in_this_file = 0;
            int files_with_bgGenes_label = 0;
            int bg_files_with_error_reports = 0;
            int files_with_no_error_reports = 0;
            int max_documented_error_reports = this.Options.Max_documented_errors_per_file;
            StringBuilder sb = new StringBuilder();
            string error_text;
            bool report_error_description = true;
            bool directory_does_not_exist = false;
            bool invalid_spelling_of_directory = false;
            bool wrong_ending_of_directory_fileName = false;
            for (int indexLE = 0; indexLE < last_error_reports_length; indexLE++)
            {
                error_message_line = this.Last_error_reports[indexLE];
                report_error_description = true;
                error_text = "";
                if ((indexLE == 0)
                    || (!error_message_line.Complete_fileName.Equals(this.Last_error_reports[indexLE - 1].Complete_fileName)))
                {
                    switch (error_message_line.File_type)
                    {
                        case Read_file_type.Parameter_settings:

                            break;
                        case Read_file_type.Background_genes:
                            if (!error_message_line.Error_message.Equals(Read_error_message_enum.BgGenes_file_read))
                            {
                                bg_files_with_error_reports++;
                            }
                            break;
                        case Read_file_type.Data:
                        default:
                            if ((!error_message_line.Error_message.Equals(Read_error_message_enum.No_error))
                                && (!error_message_line.Error_message.Equals(Read_error_message_enum.Duplicated_bggenes_dataset))
                                && (!error_message_line.Error_message.Equals(Read_error_message_enum.Directory_does_not_exist))
                                && (!error_message_line.Error_message.Equals(Read_error_message_enum.Wrong_ending_of_directoryFileName))
                                && (!error_message_line.Error_message.Equals(Read_error_message_enum.Invalid_spelling_of_directory_or_file_name)))
                            {
                                files_with_error_reports++;
                            }
                            break;
                    }
                    error_reports_in_this_file = 0;
                }
                if (error_message_line.Error_message.Equals(Read_error_message_enum.No_error))
                {
                    files_with_no_error_reports++;
                    report_error_description = false;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.BgGenes_file_read))
                {
                    report_error_description = false;
                    files_with_bgGenes_label++;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Parameter_file_read))
                {
                    files_with_parameter_settings++;
                }
                //else if (error_message_line.Error_message.Equals(Read_error_message_enum.Directory_does_not_exist))
                //{
                //    if (directory_does_not_exist) { throw new Exception(); }
                //    if (invalid_spelling_of_directory) { throw new Exception(); }
                //    directory_does_not_exist = true;
                //    report_error_description = false;
                //}
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Invalid_spelling_of_directory_or_file_name))
                {
                    if (directory_does_not_exist) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    invalid_spelling_of_directory = true;
                    report_error_description = false;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Wrong_ending_of_directoryFileName))
                {
                    if (directory_does_not_exist) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    wrong_ending_of_directory_fileName = true;
                    report_error_description = false;
                }
                else
                {
                    error_reports_in_this_file++;
                    string fileName = System.IO.Path.GetFileName(error_message_line.Complete_fileName);
                    if (error_reports_in_this_file <= max_documented_error_reports)
                    {
                        error_text = "Coding error in MBCO C# script";
                        switch (error_message_line.Error_message)
                        {
                            case Read_error_message_enum.File_does_not_exist:
                                if (fileName.Length > 0)
                                {
                                    error_text = "File does not exist in given directory";
                                }
                                else
                                {
                                    error_text = "No file specified";
                                }
                                break;
                            case Read_error_message_enum.File_does_not_contain_text:
                                error_text = "File seems to be no text file";
                                break;
                            case Read_error_message_enum.Defined_columnNames_are_missing:
                                error_text = "Headline is missing column names: " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.Length_column_entries_not_equal_length_column_names:
                                error_text = "Number of columns in line " + error_message_line.LineIndex + " differs from number of headline column names";
                                break;
                            case Read_error_message_enum.Not_a_float_or_double:
                                error_text = "Value (" + error_message_line.Value + ") is not a number in line " + error_message_line.LineIndex + " column '" + error_message_line.ColumnName + "'";
                                break;
                            case Read_error_message_enum.Not_an_integer:
                                error_text = "Value (" + error_message_line.Value + ") is not an integer in line " + error_message_line.LineIndex + " column '" + error_message_line.ColumnName + "'";
                                break;
                            case Read_error_message_enum.Not_part_of_enum:
                                error_text = "Value (" + error_message_line.Value + ") is not convertable into available selections in line " + error_message_line.LineIndex + " column '" + error_message_line.ColumnName + "'";
                                break;
                            case Read_error_message_enum.Delimiter_seems_wrong:
                                error_text = "Delimiter seems to be wrong";
                                break;
                            case Read_error_message_enum.File_name_already_uploaded:
                                error_text = "Uploaded data already contains datasets from this source file name";
                                break;
                            case Read_error_message_enum.Maximum_number_of_datasets_exceeded:
                                error_text = "Maximum number of datasets exceeded";
                                break;
                            case Read_error_message_enum.Duplicated_entry:
                                error_text = "Duplicated entry: " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.Timeunit_not_recognized:
                                error_text = "Timeunit in line " + error_message_line.LineIndex + " not recognized: " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.All_values_in_column_specified_for_1st_value_are_zero:
                                error_text = "All values in column specified to contain 1st values are zero";
                                break;
                            case Read_error_message_enum.Multiple_integration_group_assignments_for_dataset:
                                error_text = "Multiple integration group assignments to " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.Multiple_color_assignments_for_dataset:
                                error_text = "Multiple color assignments to " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.Duplicated_bggenes_dataset:
                                error_text = "Bg gene list with same name already exists";
                                break;
                            case Read_error_message_enum.Not_an_accepted_color:
                                error_text = error_message_line.Value + " (e.g., in line " + error_message_line.LineIndex + ") is not an accepted color";
                                break;
                            case Read_error_message_enum.Custom_data_array_too_long:
                                error_text = "New data cannot be added, since existing data size is close to maximum (" + error_message_line.Value + "), remove not significant  genes using menu panel 'Set data cutoffs'";
                                break;
                            case Read_error_message_enum.Directory_does_not_exist:
                                error_text = "Directory does not exist (" + error_message_line.Value + ") (" + error_message_line.Complete_fileName + ")";
                                break;
                            default:
                                throw new Exception();
                        }
                        if (report_error_description)
                        {
                            if (sb.Length > 0) { sb.AppendFormat("\r\n"); }
                            if (fileName.Length > 0)
                            {
                                sb.AppendFormat(fileName + ":   " + error_text);
                            }
                            else
                            {
                                sb.AppendFormat(error_text);
                            }
                        }
                    }
                }
                if (  (indexLE == last_error_reports_length - 1)
                    ||(!error_message_line.Complete_fileName.Equals(this.Last_error_reports[indexLE+1].Complete_fileName)))
                {
                    if ((error_reports_in_this_file > max_documented_error_reports)&&(report_error_description))
                    {
                        if (indexLE > 0) { sb.AppendFormat("\r\n"); }
                        error_text = "There are at least " + (error_reports_in_this_file - max_documented_error_reports) + " additional errors.";
                        sb.AppendFormat(System.IO.Path.GetFileName(error_message_line.Complete_fileName) + ":   " + error_text);
                    }
                }
            }
            Error_reports_ownTextBox.SilentText = sb.ToString();
            Error_reports_label.Text = "";
            List<string> error_reports_text_list = new List<string>();
            if (files_with_no_error_reports == 1)
            {
                error_reports_text_list.Add("Data from " + files_with_no_error_reports + " file added.");
            }
            else if (files_with_no_error_reports > 1)
            {
                error_reports_text_list.Add("Data from " + files_with_no_error_reports + " files added.");
            }
            if (files_with_error_reports == 1)
            {
                error_reports_text_list.Add("Data from " + files_with_error_reports + " file not added.");
            }
            else if (files_with_error_reports > 1)
            {
                error_reports_text_list.Add("Data from " + files_with_error_reports + " files not added.");
            }
            if (files_with_bgGenes_label == 1)
            {
                error_reports_text_list.Add("Background genes from " + files_with_bgGenes_label + " file added.");
            }
            else if (files_with_bgGenes_label > 1)
            {
                error_reports_text_list.Add("Background genes from " + files_with_bgGenes_label + " files added.");
            }
            if (bg_files_with_error_reports == 1)
            {
                error_reports_text_list.Add("Background genes from " + bg_files_with_error_reports + " file not added.");
            }
            else if (bg_files_with_error_reports > 1)
            {
                error_reports_text_list.Add("Background genes from " + bg_files_with_error_reports + " files not added.");
            }
            if (files_with_parameter_settings >= 1)
            {
                error_reports_text_list.Add("Parameters imported from file.");
            }
            if (directory_does_not_exist)
            {
                error_reports_text_list.Add("Directory does not exist.");
            }
            if (invalid_spelling_of_directory)
            {
                error_reports_text_list.Add("Invalid spelling of directory or file name.");
            }
            if (wrong_ending_of_directory_fileName)
            {
                error_reports_text_list.Add("Please ensure file name ends with extension\r\n(e.g. '.txt') and directory name with '\\'");
            }
            if (error_reports_text_list.Count > 0)
            {
                StringBuilder error_reports_sb = new StringBuilder();
                if (error_reports_text_list.Count<=3)
                {
                    foreach (string error_report in error_reports_text_list)
                    {
                        if (error_reports_sb.Length>0) { error_reports_sb.AppendFormat("\r\n"); }
                        error_reports_sb.AppendFormat(error_report);
                    }
                }
                else
                {
                    error_reports_sb.AppendFormat("{0}, {1}", error_reports_text_list[0], error_reports_text_list[1]);
                    int error_reports_length = error_reports_text_list.Count;
                    for (int indexER=2; indexER<error_reports_length;indexER++)
                    {
                        error_reports_sb.AppendFormat("\r\n{0}", error_reports_text_list[indexER]);
                    }
                }
                Error_reports_label.Text = error_reports_sb.ToString();
                Adjust_sizes_of_error_reports_label();
            }
            if ((files_with_error_reports>0)||(bg_files_with_error_reports>0))
            {
                Error_reports_button.Text = (string)Read_default_settings_class.ErrorReportsExplanationButtonText_errorText.Clone();
                Errors_reports_headline_label.Text = "Error messages";
            }
            else
            {
                Error_reports_button.Text = (string)Read_default_settings_class.ErrorReportsExplanationButtonText_explanationText.Clone();
                Add_explanations_to_error_reportBox();
            }
        }
        #endregion

        #region Read and add to custom data
        private string[] Read_parameter_setting_lines_if_exists(string complete_directory)
        {
            string complete_fileName = complete_directory + Global_dirFile.Mbco_parameter_settings_fileName;
            List<string> inputLines = new List<string>();
            string inputLine;
            if (System.IO.File.Exists(complete_fileName))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(complete_fileName);
                long baseStream_length = reader.BaseStream.Length;
                while ((inputLine = reader.ReadLine())!= null)
                {
                    if (!inputLine.Equals(Global_dirFile.FirstLine_of_mbco_parameter_setting_fileName))
                    {
                        inputLines.Add(inputLine);
                    }
                }
            }
            return inputLines.ToArray();
        }

        public Custom_data_class Read_button_click(Custom_data_class custom_data, 
                                                   string defaultIntegrationGroup, 
                                                   Color[] selectable_colors,
                                                   out string[] read_parameter_settings,
                                                   System.Windows.Forms.Label errorReport_label
                                                   )
        {
            read_parameter_settings = new string[0];
            Read_data_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Read_data_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            if (Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible())
            {
                bool Continue = true;
                List<Read_error_message_line_class> all_error_messages_list = new List<Read_error_message_line_class>();
                List<Read_error_message_line_class> currentFile_error_messages_list = new List<Read_error_message_line_class>();
                string directory = "";
                Read_directoryOrFile_outside_ownTextBox.SilentText = Global_dirFile.Transform_into_compatible_directory(Read_directoryOrFile_outside_ownTextBox.Text);
                string inputDirectoryFileName = Read_directoryOrFile_outside_ownTextBox.Text;

                if (  (!inputDirectoryFileName[inputDirectoryFileName.Length - 1].Equals(Global_dirFile.Delimiter))
                    &&(System.IO.Path.GetExtension(inputDirectoryFileName).Length==0))
                {
                    Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                    new_error_message.File_type = Read_file_type.Data;
                    new_error_message.Complete_fileName = (string)directory.Clone();
                    new_error_message.Error_message = Read_error_message_enum.Wrong_ending_of_directoryFileName;
                    all_error_messages_list.Add(new_error_message);
                    Continue = false;
                }
                if (Continue)
                {
                    try
                    {
                        directory = System.IO.Path.GetDirectoryName(inputDirectoryFileName) + Global_dirFile.Delimiter;
                    }
                    catch
                    {
                        Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                        new_error_message.File_type = Read_file_type.Data;
                        new_error_message.Complete_fileName = (string)directory.Clone();
                        new_error_message.Error_message = Read_error_message_enum.Invalid_spelling_of_directory_or_file_name;
                        all_error_messages_list.Add(new_error_message);
                        Continue = false;
                    }
                }
                string[] complete_fileNames = new string[0];
                if (Continue)
                {
                    if (!System.IO.Directory.Exists(directory))
                    {
                        complete_fileNames = new string[0];
                        Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                        new_error_message.File_type = Read_file_type.Data;
                        new_error_message.Complete_fileName = (string)directory.Clone();
                        new_error_message.Error_message = Read_error_message_enum.Directory_does_not_exist;
                        all_error_messages_list.Add(new_error_message);
                        Continue = false;
                    }
                }
                if (Continue)
                { 
                    if ((!Read_order_allFilesInDirectory_cbButton.Checked) && (Read_order_onlySpecifiedFile_cbButton.Checked))
                    { complete_fileNames = new string[] { inputDirectoryFileName }; }
                    else if ((Read_order_allFilesInDirectory_cbButton.Checked) && (!Read_order_onlySpecifiedFile_cbButton.Checked))
                    { complete_fileNames = System.IO.Directory.GetFiles(directory); }
                    else { throw new Exception(); }
                }
                if (Continue)
                {
                    int complete_fileNames_length = complete_fileNames.Length;
                    string complete_fileName;
                    string fileName;
                    string fileName_without_extension;
                    bool column_for_first_value_specified = false;

                    #region Set readOptions parameter
                    Read_file_delimiter_enum delimiter = Read_default_settings_class.Get_delimter_from_string(Read_delimiter_ownListBox.SelectedItem.ToString());
                    char delimiter_char = ReadWrite_settings_class.Get_delimiter_char_from_enum(delimiter);
                    List<string> key_propertyNames_list = new List<string>();
                    List<string> key_columnNames_list = new List<string>();
                    if (!String.IsNullOrEmpty(Read_sampleNameColumn_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("SampleName");
                        key_columnNames_list.Add(Read_sampleNameColumn_ownTextBox.Text);
                    }
                    if (!String.IsNullOrEmpty(Read_timepointColumn_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("Timepoint");
                        key_columnNames_list.Add(Read_timepointColumn_ownTextBox.Text);
                    }
                    if (!String.IsNullOrEmpty(Read_timeunitColumn_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("Timeunit_string");
                        key_columnNames_list.Add(Read_timeunitColumn_ownTextBox.Text);
                    }
                    if (!String.IsNullOrEmpty(Read_color_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("SampleColor_string");
                        key_columnNames_list.Add(Read_color_ownTextBox.Text);
                    }
                    if (!String.IsNullOrEmpty(Read_geneSymbol_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("NCBI_official_symbol");
                        key_columnNames_list.Add(Read_geneSymbol_ownTextBox.Text);
                    }
                    if (!String.IsNullOrEmpty(Read_integrationGroup_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("IntegrationGroup");
                        key_columnNames_list.Add(Read_integrationGroup_ownTextBox.Text);
                    }
                    if (!String.IsNullOrEmpty(Read_value1stColumn_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("Value_1st");
                        key_columnNames_list.Add(Read_value1stColumn_ownTextBox.Text);
                        column_for_first_value_specified = true;
                    }
                    if (!String.IsNullOrEmpty(Read_value2ndColumn_ownTextBox.Text))
                    {
                        key_propertyNames_list.Add("Value_2nd");
                        key_columnNames_list.Add(Read_value2ndColumn_ownTextBox.Text);
                    }
                    #endregion

                    Dictionary<string, bool> existing_sourceFileNames_dict = custom_data.Get_sourceFileNames_dict();

                    int custom_datasets_count = custom_data.Get_all_unique_ordered_sampleNames().Length;
                    int add_custom_datasets_count;
                    for (int indexC = 0; indexC < complete_fileNames_length; indexC++)
                    {
                        currentFile_error_messages_list.Clear();
                        complete_fileName = complete_fileNames[indexC];
                        fileName = System.IO.Path.GetFileName(complete_fileName);
                        fileName_without_extension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        bool is_mbco_paremeter_settings_file = fileName.Equals(Global_dirFile.Mbco_parameter_settings_fileName);
                        if (is_mbco_paremeter_settings_file)
                        {
                            System.IO.StreamReader reader = new System.IO.StreamReader(complete_fileName);
                            string firstLine = reader.ReadLine();
                            reader.Close();
                            if (!firstLine.Equals(Global_dirFile.FirstLine_of_mbco_parameter_setting_fileName))
                            {
                                is_mbco_paremeter_settings_file = false;
                            }
                        }
                        if (is_mbco_paremeter_settings_file)
                        {
                            read_parameter_settings = Read_parameter_setting_lines_if_exists(directory);
                            Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                            new_error_message.Error_message = Read_error_message_enum.Parameter_file_read;
                            new_error_message.File_type = Read_file_type.Parameter_settings;
                            new_error_message.Complete_fileName = directory + fileName;
                            currentFile_error_messages_list.Add(new_error_message);
                        }
                        else if (fileName_without_extension.IndexOf(Global_class.Bg_genes_label) == fileName_without_extension.Length - Global_class.Bg_genes_label.Length)
                        {
                            Read_error_message_line_class[] new_error_messages = custom_data.Read_and_add_background_genes_and_return_error_messages(complete_fileName, errorReport_label, Form_default_settings);
                            currentFile_error_messages_list.AddRange(new_error_messages);
                        }
                        else if (existing_sourceFileNames_dict.ContainsKey(fileName))
                        {
                            Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                            new_error_message.File_type = Read_file_type.Data;
                            new_error_message.Complete_fileName = (string)complete_fileName.Clone();
                            new_error_message.Error_message = Read_error_message_enum.File_name_already_uploaded;
                            currentFile_error_messages_list.Add(new_error_message); ;
                        }
                        else
                        {
                            Progress_report_label.Text = "Reading " + fileName;
                            Progress_report_label.Visible = true;
                            Progress_report_label.Refresh();
                            existing_sourceFileNames_dict.Add(fileName, true);
                            Custom_data_class add_custom_data = new Custom_data_class();
                            Custom_data_readWriteOptions_class readWriteOptions = new Custom_data_readWriteOptions_class(directory,fileName);
                            readWriteOptions.HeadlineDelimiters = new char[] { delimiter_char };
                            readWriteOptions.LineDelimiters = new char[] { delimiter_char };
                            readWriteOptions.Key_propertyNames = key_propertyNames_list.ToArray();
                            readWriteOptions.Key_columnNames = key_columnNames_list.ToArray();

                            Read_error_message_line_class[] error_messages_read = add_custom_data.Generate_custom_data_instance_if_no_errors_and_return_error_messages(column_for_first_value_specified, readWriteOptions, errorReport_label, Form_default_settings);
                            
                            currentFile_error_messages_list.AddRange(error_messages_read);
                            if (add_custom_data.Custom_data.Length > 0)
                            {
                                Read_error_message_line_class[] error_messages_timeunit = add_custom_data.Set_timeunits_based_on_timeunit_strings_and_return_error_messages(readWriteOptions.File, Read_timeunitColumn_ownTextBox.Text, readWriteOptions.Max_error_messages - currentFile_error_messages_list.Count);
                                currentFile_error_messages_list.AddRange(error_messages_timeunit);
                                UploadedFileNo++;
                                add_custom_data.Set_unique_fixed_dataset_identifier_after_reading(UploadedFileNo);
                                Read_error_message_line_class[] error_messages_duplicated = add_custom_data.Analyze_if_any_duplicated_lines_based_on_uniqueFixedDatasetIdentifier_and_geneSymbol(complete_fileName, readWriteOptions.Max_error_messages - currentFile_error_messages_list.Count);
                                currentFile_error_messages_list.AddRange(error_messages_duplicated);
                                Read_error_message_line_class[] error_messages_duplicated2 = add_custom_data.Analyze_if_all_sampleColors_integrationGroups_are_identical_for_each_uniqueFixedDatasetIdentifier_and_geneSymbol(complete_fileName, readWriteOptions.Max_error_messages - currentFile_error_messages_list.Count);
                                currentFile_error_messages_list.AddRange(error_messages_duplicated2);
                            }
                            if (currentFile_error_messages_list.Count == 0)
                            {
                                Timeunit_enum default_timeunit = (Timeunit_enum)Enum.Parse(typeof(Timeunit_enum), Read_timepointUnit_ownListBox.Text);
                                add_custom_data.Set_missing_dataset_names(Form_default_settings.My_dataset);
                                add_custom_data.Set_empty_timeunits_to_input_timeunit_and_check_if_all_or_no_timeunits_are_empty(default_timeunit);
                                add_custom_data.Set_missing_integrationGroups(defaultIntegrationGroup);
                                add_custom_data.Order_by_integrationGroup_sampleName_timepointInDays_entryType();

                                add_custom_datasets_count = add_custom_data.Get_all_unique_ordered_sampleNames().Length;
                                Read_error_message_line_class[] error_messages_custom_data_array_too_long = custom_data.Add_other_and_return_error_messages_if_not_possible(add_custom_data);
                                foreach (Read_error_message_line_class error_message_line_array_oversized in error_messages_custom_data_array_too_long)
                                {
                                    error_message_line_array_oversized.Complete_fileName = (string)readWriteOptions.File.Clone();
                                    error_message_line_array_oversized.Value = add_custom_data.Custom_data.Length.ToString() + " existing lines, " + custom_data.Custom_data.Length.ToString() + " new lines";
                                }
                                currentFile_error_messages_list.AddRange(error_messages_custom_data_array_too_long);
                                custom_data.Check_for_correctness();
                                custom_datasets_count += add_custom_datasets_count;
                            }
                            if (currentFile_error_messages_list.Count==0)
                            {
                                Read_error_message_line_class error_message_line = new Read_error_message_line_class();
                                error_message_line.File_type = Read_file_type.Data;
                                error_message_line.Complete_fileName = (string)readWriteOptions.File.Clone();
                                error_message_line.Error_message = Read_error_message_enum.No_error;
                                currentFile_error_messages_list.Add(error_message_line);
                            }
                            
                        }
                        all_error_messages_list.AddRange(currentFile_error_messages_list);
                    }
                    Progress_report_label.Text = "Add missing colors";
                    custom_data.Automatically_override_bgGeneListNames_if_matching_names();
                    custom_data.Set_missing_colors(selectable_colors);
                    Progress_report_label.Text = "Set missing ";
                    custom_data.Set_missing_results_numbers();
                    Progress_report_label.Text = "Label significant genes based on specified significance criteria";
                    custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
                }
                this.Last_error_reports = all_error_messages_list.ToArray();

                Update_error_reports_or_explanations();
                Progress_report_label.Text = "";
                Progress_report_label.Visible = false;
                Progress_report_label.Refresh();
            }
            Read_data_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Read_data_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            return custom_data;
        }
        #endregion

    }
}
