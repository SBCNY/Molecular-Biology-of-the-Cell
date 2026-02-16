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
using Windows_forms;
using Common_functions.Array_own;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Network;
using System.Security.Cryptography;
using System.Reflection;

namespace ClassLibrary1.Read_interface
{
    enum Override_used_selected_column_names_enum { No, Example_dataset_no, Default_custom_1_names, Default_custom_2_names }
    enum ReadDataMenu_save_selection_type_enum { E_m_p_t_y, Read_data_directory, Default_column_names, Read_all_or_one_file }
    enum ReadDataMenu_default_columnName_enum { E_m_p_t_y, Custom_1, Custom_2, Mbco, Single_cell, Minimum, Optimum }
    enum ReadDataMenu_readAllOrOneFile_enum {  E_m_p_t_y, Read_all_files, Read_one_file }
    enum ReadDataMenu_datasetAttribute_enum { E_m_p_t_y, Dataset_name, Ncbi_official_gene_symbol, Time_point, Time_unit, Integration_group, Dataset_color, Value_1st, Value_2nd };
    class Read_default_settings_class
    {
        public static string Timepoint_ignored_text {  get { return "Timepoints will be ignored"; } }
        public static string Names_will_be_extended_text {  get { return "Names will be extended by file names"; } }
        public static string Backgrounod_genes_file_info {  get { return "Background genes in files ending with '_bgGenes' (one column, no headline) are mapped automatically to datasets supplied with the same file name minus the '_bgGenes' part."; } }
        public static string EntryType_by_value_text {  get { return "Genes with 1st values >0 or <0 are up- or downregulated, respectively. Genes with 1st values = 0 will be removed."; } }
        public static string MBCO_columnNames_info {  get {  return "The application will also search for the potential column name 'Dataset order #'.";} }
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

    class DatasetAttribute_columnName_line_class
    {
        public ReadDataMenu_datasetAttribute_enum Dataset_attribute { get; set; }
        public string ColumnName { get; set; }
        public DatasetAttribute_columnName_line_class Deep_copy()
        {
            DatasetAttribute_columnName_line_class copy = (DatasetAttribute_columnName_line_class)this.MemberwiseClone();
            copy.ColumnName = (string)this.ColumnName.Clone();
            return copy;
        }
    }

    class ReadTextBoxName_columnName_readWriteOptions : ReadWriteOptions_base
    {
        public ReadTextBoxName_columnName_readWriteOptions(string directory, int custom_columnNames_no)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            if ((custom_columnNames_no!=1)&&(custom_columnNames_no!=2)) { throw new Exception(); }
            this.File = directory + gdf.Get_customColumName_fileName(custom_columnNames_no);
            this.Key_propertyNames = new string[] { "Dataset_attribute", "ColumnName" };
            this.Key_columnNames = new string[] { "Attribute in the application", "Column name in user data" };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
            this.File_has_headline = true;
        }
    }

    class Save_readDataMenuSelections_line_class
    {
        public ReadDataMenu_save_selection_type_enum Save_selection_type { get; set; }
        public string Entry { get; set; }
        public Save_readDataMenuSelections_line_class Deep_copy()
        {
            Save_readDataMenuSelections_line_class copy = (Save_readDataMenuSelections_line_class)this.MemberwiseClone();
            copy.Entry = (string)this.Entry.Clone();
            return copy;
        }
    }
    class Save_readDataMenuSelections_readWriteOptions : ReadWriteOptions_base
    {
        public Save_readDataMenuSelections_readWriteOptions()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.App_generated_datasets_directory + gdf.ReadDataMenu_save_setting_fileName;
            this.Key_propertyNames = new string[] { "Save_selection_type", "Entry" };
            this.Key_columnNames = this.Key_propertyNames;
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
            this.File_has_headline = true;
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
        private MyPanel_label Read_information_myPanelLabel { get; set; }
        private Label Read_defaultColumnName_label { get; set; }
        private Button Read_setTo_singleCell_button { get; set; }
        private Button Read_setTo_custom1_button { get; set; }
        private Button Read_setTo_custom2_button { get; set; }
        private Button Read_setTo_MBCO_button { get; set; }
        private Button Read_setTo_minimum_button { get; set; }
        private Button Read_setTo_optimum_button { get; set; }
        private Button Read_data_button { get; set; }
        private Label Errors_reports_headline_label { get; set; }
        private Button Error_reports_button { get; set; }
        private MyPanel_label Error_reports_myPanelLabel {get;set;}
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }
        private Label Error_reports_maxErrorPerFile1_label { get; set; }
        private Label Error_reports_maxErrorPerFile2_label { get; set; }
        private Read_error_message_line_class[] Last_error_reports { get; set; }
        private Read_interface_options_class Options { get; set; }
        public int UploadedFileNo { get; set; }
        public Form1_default_settings_class Form_default_settings { get; set; }
        public ProgressReport_interface_class ProgressReport { get; set; }
        private Button Tutorial_button { get; set; }
        public Tutorial_interface_class UserInterface_tutorial { get; set; }
        private Global_directory_and_file_class Global_dirFile { get; set; }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, OwnTextBox> DatasetAttribute_textBox_dict { get; set; }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, string> DatasetAttribute_customColumnName_1_dict { get; set; }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, string> DatasetAttribute_customColumnName_2_dict { get; set; }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, string> DatasetAttribute_propertyName_dict { get; set; }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, string> DatasetAttribute_customColumnName_default_1_dict { get; set; }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, string> DatasetAttribute_customColumnName_default_2_dict { get; set; }
        
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
                                    MyPanel_label read_information_myPanelLabel,
                                    Label read_defaultColumnName_label,
                                    Button read_setTo_example1_button,
                                    Button read_setTo_example2_button,
                                    Button read_setTo_MBCO_button,
                                    Button read_setTo_seurat_button,
                                    Button read_setTo_minimum_button,
                                    Button read_setTo_optimum_button,
                                    Button read_data_button,
                                    Button tutorial_button,
                                    Label errors_reportsHeadline_label,
                                    Button error_reports_button,
                                    MyPanel_label error_reports_myPanelLabel,
                                    OwnTextBox error_reports_ownTextBox,
                                    Label error_reports_maxErrorPerFile1_label,
                                    Label error_reports_maxErrorPerFile2_label,
                                    OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,
                                    ProgressReport_interface_class progressReport,
                                    Tutorial_interface_class userInterface_tutorial,
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
            this.Tutorial_button = tutorial_button;
            this.Read_timepointUnit_ownListBox = read_timepointUnit_ownListBox;
            this.Read_directoryOrFile_outside_label = read_directoryOrFile_outside_label;
            this.Read_directoryOrFile_outside_ownTextBox = read_directoryOrFile_outside_ownTextBox;
            this.Read_information_myPanelLabel = read_information_myPanelLabel;
            this.Error_reports_button = error_reports_button;
            this.Error_reports_myPanelLabel = error_reports_myPanelLabel;
            this.Errors_reports_headline_label = errors_reportsHeadline_label;
            this.Error_reports_ownTextBox = error_reports_ownTextBox;
            this.Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            this.Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            this.ProgressReport = progressReport;
            this.UserInterface_tutorial = userInterface_tutorial;
            this.Read_setTo_custom1_button = read_setTo_example1_button;
            this.Read_setTo_custom2_button = read_setTo_example2_button;
            this.Read_setTo_singleCell_button = read_setTo_seurat_button;
            this.Read_setTo_MBCO_button = read_setTo_MBCO_button;
            this.Read_setTo_minimum_button = read_setTo_minimum_button;
            this.Read_setTo_optimum_button = read_setTo_optimum_button;
            this.Read_defaultColumnName_label = read_defaultColumnName_label;
            this.Read_value1st_explanation_label = read_value1st_explanation_label;
            this.Read_value2nd_explanation_label = read_value2nd_explanation_label;
            this.Read_delimiter_ownListBox = read_delimiter_ownListBox;
            this.Read_delimiter_label = read_delimiter_label;
            this.Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;

            Fill_datasetAttribute_textBox_dict();
            Fill_datasetAttribute_propertyName_dict();
            Fill_readTextBoxName_customColumnName_dicts();
            Initialize_checkBox_buttons_and_listBoxes();
            Update_all_graphic_elements();
        }
        private void Fill_datasetAttribute_textBox_dict()
        {
            DatasetAttribute_textBox_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, OwnTextBox>();
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_name, Read_sampleNameColumn_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Ncbi_official_gene_symbol, Read_geneSymbol_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_point, Read_timepointColumn_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_unit, Read_timeunitColumn_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Integration_group, Read_integrationGroup_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_1st, Read_value1stColumn_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_2nd, Read_value2ndColumn_ownTextBox);
            DatasetAttribute_textBox_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_color, Read_color_ownTextBox);
        }
        private void Fill_datasetAttribute_propertyName_dict()
        {
            DatasetAttribute_propertyName_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, string>();
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_name, "SampleName");
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Ncbi_official_gene_symbol, "NCBI_official_symbol");
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_point, "Timepoint");
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_unit, "Timeunit_string"); // has to be string
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Integration_group, "IntegrationGroup");
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_1st, "Value_1st");
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_2nd, "Value_2nd");
            DatasetAttribute_propertyName_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_color, "SampleColor_string"); // has to be string

            Type customDataLine_type = typeof(Custom_data_line_class);
            PropertyInfo[] properties = customDataLine_type.GetProperties();
            List<string> wrong_property_names = new List<string>();

            foreach (var dict_pair in DatasetAttribute_propertyName_dict)
            {
                string propertyName = dict_pair.Value;
                PropertyInfo prop = customDataLine_type.GetProperty(propertyName);
                if (prop == null)
                {
                    wrong_property_names.Add(propertyName);
                }
            }
            if (wrong_property_names.Count>0) { throw new Exception(); }
        }
        private void Set_readData_savable_menu_selections_to_default_or_to_saved_values_if_exist()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            Save_readDataMenuSelections_readWriteOptions directoryFile_readWriteOptions = new Save_readDataMenuSelections_readWriteOptions();
            this.Read_directoryOrFile_outside_ownTextBox.SilentText = gdf.Custom_data_directory + "Enter_data_subdirectory\\";
            SetTo_custom1_button_clicked();
            Read_allFilesInDirectory_ownCheckedBox_clicked();
            if (System.IO.File.Exists(directoryFile_readWriteOptions.File))
            {
                Save_readDataMenuSelections_line_class[] selection_lines = ReadWriteClass.Read_data_fill_array_and_return_error_messages<Save_readDataMenuSelections_line_class>(out Read_error_message_line_class[] error_messages, directoryFile_readWriteOptions, ProgressReport);
                if (error_messages.Length>0)
                {
                    System.IO.File.Delete(directoryFile_readWriteOptions.File);
                    this.Read_directoryOrFile_outside_ownTextBox.SilentText = gdf.Custom_data_directory + "Enter_data_subdirectory\\";
                }
                else
                {
                    foreach (Save_readDataMenuSelections_line_class selection_line in selection_lines)
                    {
                        switch (selection_line.Save_selection_type)
                        {
                            case ReadDataMenu_save_selection_type_enum.Default_column_names:
                                ReadDataMenu_default_columnName_enum selected_default_columnName = (ReadDataMenu_default_columnName_enum)Enum.Parse(typeof(ReadDataMenu_default_columnName_enum), selection_line.Entry);
                                switch (selected_default_columnName)
                                {
                                    case ReadDataMenu_default_columnName_enum.Optimum:
                                        SetTo_optimum_button_clicked();
                                        break;
                                    case ReadDataMenu_default_columnName_enum.Minimum:
                                        SetTo_minimum_button_clicked();
                                        break;
                                    case ReadDataMenu_default_columnName_enum.Single_cell:
                                        SetTo_singleCell_button_clicked();
                                        break;
                                    case ReadDataMenu_default_columnName_enum.Custom_1:
                                        SetTo_custom1_button_clicked();
                                        break;
                                    case ReadDataMenu_default_columnName_enum.Custom_2:
                                        SetTo_custom2_button_clicked();
                                        break;
                                    case ReadDataMenu_default_columnName_enum.Mbco:
                                        SetTo_mbco_button_clicked();
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                break;
                            case ReadDataMenu_save_selection_type_enum.Read_data_directory:
                                this.Read_directoryOrFile_outside_ownTextBox.SilentText_and_refresh = (string)selection_line.Entry.Clone();
                                break;
                            case ReadDataMenu_save_selection_type_enum.Read_all_or_one_file:
                                ReadDataMenu_readAllOrOneFile_enum selected_read_all_or_one = (ReadDataMenu_readAllOrOneFile_enum)Enum.Parse(typeof(ReadDataMenu_readAllOrOneFile_enum), selection_line.Entry);
                                switch (selected_read_all_or_one)
                                {
                                    case ReadDataMenu_readAllOrOneFile_enum.Read_one_file:
                                        Read_onlySpecifiedFile_ownCheckedBox_clicked();
                                        break;
                                    case ReadDataMenu_readAllOrOneFile_enum.Read_all_files:
                                        Read_allFilesInDirectory_ownCheckedBox_clicked();
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                break;
                            default:
                                throw new Exception();
                        }
                    }
                }
            }
        }
        public void Fill_or_override_datasetAttribute_customColumnName_1_or_2_dict(DatasetAttribute_columnName_line_class[] datasetAttribute_columnNames_lines, int customColumnName_no)
        {
            Dictionary<ReadDataMenu_datasetAttribute_enum, string> datasetAttribute_customColumnName_ref_dict;
            switch (customColumnName_no)
            {
                case 1:
                    datasetAttribute_customColumnName_ref_dict = DatasetAttribute_customColumnName_1_dict;
                    break;
                case 2:
                    datasetAttribute_customColumnName_ref_dict = DatasetAttribute_customColumnName_2_dict;
                    break;
                default:
                    throw new Exception();
            }
            foreach (DatasetAttribute_columnName_line_class datasetAttribute_columnNames_line in datasetAttribute_columnNames_lines)
            {
                if (datasetAttribute_customColumnName_ref_dict.ContainsKey(datasetAttribute_columnNames_line.Dataset_attribute))
                {
                    if (!String.IsNullOrEmpty(datasetAttribute_columnNames_line.ColumnName))
                    { datasetAttribute_customColumnName_ref_dict[datasetAttribute_columnNames_line.Dataset_attribute] = (string)datasetAttribute_columnNames_line.ColumnName.Clone(); }
                    else { datasetAttribute_customColumnName_ref_dict[datasetAttribute_columnNames_line.Dataset_attribute] = ""; }
                }
            }
            
        }
        private void Fill_readTextBoxName_customColumnName_dicts()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            DatasetAttribute_customColumnName_default_1_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, string>();
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_name, "Dataset_name");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Ncbi_official_gene_symbol, "NCBI_official_gene_symbol");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_point, "");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_unit, "");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Integration_group, "Integration_group");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_color, "Dataset_color");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_1st, "Log2_fold_change");
            DatasetAttribute_customColumnName_default_1_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_2nd, "Minus_log10_pval_or_adj_pval");
            DatasetAttribute_customColumnName_1_dict = Array_class.Deep_copy_dictionary(DatasetAttribute_customColumnName_default_1_dict);

            DatasetAttribute_customColumnName_default_2_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, string>();
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_name, "Dataset_name");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Ncbi_official_gene_symbol, "NCBI_official_gene_symbol");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_point, "Time point");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Time_unit, "Time unit");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Integration_group, "Integration_group");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Dataset_color, "Dataset_color");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_1st, "Log2_fold_change");
            DatasetAttribute_customColumnName_default_2_dict.Add(ReadDataMenu_datasetAttribute_enum.Value_2nd, "Minus_log10_pval_or_adj_pval");
            DatasetAttribute_customColumnName_2_dict = Array_class.Deep_copy_dictionary(DatasetAttribute_customColumnName_default_2_dict);

            int[] custom_columnNames_nos = new int[] { 1, 2 };
            int custom_columnNames_nos_length = custom_columnNames_nos.Length;
            int custom_columnNames_no;
            for (int indexCC = 0; indexCC < custom_columnNames_nos_length; indexCC++)
            {
                custom_columnNames_no = custom_columnNames_nos[indexCC];
                ReadTextBoxName_columnName_readWriteOptions readWriteOptions_1 = new ReadTextBoxName_columnName_readWriteOptions(gdf.App_generated_datasets_directory, custom_columnNames_no);
                if (System.IO.File.Exists(readWriteOptions_1.File))
                {
                    Read_error_message_line_class[] error_messages;
                    DatasetAttribute_columnName_line_class[] propertyNames_columnNames_lines = ReadWriteClass.Read_data_fill_array_and_return_error_messages<DatasetAttribute_columnName_line_class>(out error_messages, readWriteOptions_1, ProgressReport);
                    Fill_or_override_datasetAttribute_customColumnName_1_or_2_dict(propertyNames_columnNames_lines, custom_columnNames_no);
                }
            }
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
            my_button = this.Read_setTo_custom1_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = defaultColumnNames_button_left_position;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = this.Read_setTo_custom1_button.Location.Y + this.Read_setTo_custom1_button.Height;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_custom2_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_custom1_button.Location.X + this.Read_setTo_custom1_button.Width;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = defaultColumnNames_button_top_position;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_singleCell_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_custom2_button.Location.X + this.Read_setTo_custom2_button.Width;
            current_right_position = current_left_position + defaultColumnNames_button_width;
            current_top_position = this.Read_setTo_singleCell_button.Location.Y + this.Read_setTo_singleCell_button.Height;
            current_bottom_position = current_top_position + defaultColumnNames_button_height;
            my_button = this.Read_setTo_MBCO_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = this.Read_setTo_singleCell_button.Location.X + this.Read_setTo_singleCell_button.Width;
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
            current_right_position = this.Read_setTo_custom1_button.Location.X;
            current_top_position = this.Read_setTo_custom1_button.Location.Y + (int)Math.Round(0.075 * this.Read_setTo_custom1_button.Height); ;
            current_bottom_position = this.Read_setTo_custom2_button.Location.Y + (int)Math.Round(0.925 * this.Read_setTo_custom2_button.Height);
            my_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, current_left_position, current_right_position, current_top_position, current_bottom_position);
            #endregion

            #region Adjust read order checkboxes and label
            int widthHeight_of_one_checkBox = (int)Math.Round(0.05F * Read_overall_groupBox.Height);
            current_left_position = shared_delimiterListBox_defaultColumnNamesButton_readCheckBox_left_position;
            current_right_position = current_left_position + widthHeight_of_one_checkBox;
            current_top_position = Read_setTo_custom2_button.Location.Y + Read_setTo_custom2_button.Height;
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
            if (Form_default_settings.Is_mono)
            {
                current_top_position -= (int)Math.Round(0.015 * this.Read_overall_groupBox.Height); //0.01
                current_bottom_position -= (int)Math.Round(0.015 * this.Read_overall_groupBox.Height); //0.01
            }
            int button_outer_distance_from_leftRightSide = (int)Math.Round(0.05F * this.Read_overall_groupBox.Width);
            int button_size = (int)Math.Round(((double)this.Read_overall_groupBox.Width - 2 * button_outer_distance_from_leftRightSide)/3);
            int distance_between_buttons = (int)Math.Round(0.01F * this.Read_overall_groupBox.Width);
            button_size = (int)Math.Round((double)button_size - (double)distance_between_buttons / 2.0);
            int button_inner_distance_from_leftRightSide = (int)Math.Round(0.4F * this.Read_overall_groupBox.Width);

            current_left_position = button_outer_distance_from_leftRightSide;
            current_right_position = current_left_position + button_size;
            my_button = this.Error_reports_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = current_right_position + distance_between_buttons;
            current_right_position = current_left_position + button_size;
            my_button = this.Tutorial_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, current_left_position, current_right_position, current_top_position, current_bottom_position);

            current_left_position = current_right_position + distance_between_buttons;
            current_right_position = current_left_position + button_size;
            my_button = this.Read_data_button;
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
            this.Read_information_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Read_information_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(current_left_position, current_top_position, current_right_position, current_bottom_position, Form_default_settings);
        }

        private void Adjust_sizes_of_error_reports_label()
        {
            int current_left_position = 0;
            int current_right_position = Read_overall_groupBox.Width;
            int current_top_position = Read_order_onlySpecifiedFile_cbButton.Location.Y + Read_order_onlySpecifiedFile_cbButton.Height;
            int current_bottom_position = Error_reports_button.Location.Y;
            Error_reports_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(current_left_position, current_top_position, current_right_position, current_bottom_position, Form_default_settings);
        }

        private void Initialize_checkBox_buttons_and_listBoxes()
        {
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.sec);
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.min);
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.hrs);
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.days);
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.weeks);
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.months);
            Read_timepointUnit_ownListBox.Items.Add(Timeunit_enum.years);
            Read_timepointUnit_ownListBox.SilentSelectedIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);

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
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            this.Last_error_reports = new Read_error_message_line_class[0];

            Set_readData_savable_menu_selections_to_default_or_to_saved_values_if_exist();
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
            Read_overall_groupBox.Refresh();
            Read_directoryOrFile_outside_ownTextBox.Refresh();
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
            Read_information_myPanelLabel.ForeColor = Form_default_settings.ExplanationText_color;
            Error_reports_myPanelLabel.ForeColor = Form_default_settings.ExplanationText_color;
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
                Read_information_myPanelLabel.Status = MyPanel_label_status_enum.Red_warning;
                Read_information_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(Read_default_settings_class.Missing_mandatory_columnNames);
                proceedToReadIsPossible = false;
            }
            else
            {
                bool duplicated = Analyze_if_selected_column_names_are_duplicated_and_set_backcolors_accordingly();
                Read_information_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
                if (duplicated)
                {
                    Read_information_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(Read_default_settings_class.Duplicated_columnNames);
                    Read_information_myPanelLabel.Status = MyPanel_label_status_enum.Red_warning;
                    proceedToReadIsPossible = false;
                }
                else
                {
                    if (Read_setTo_MBCO_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
                    {
                        Read_information_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(Read_default_settings_class.MBCO_columnNames_info);
                        Read_information_myPanelLabel.Status = MyPanel_label_status_enum.Regular;
                    }
                    else if (!String.IsNullOrEmpty(this.Read_value1stColumn_ownTextBox.Text))
                    {
                        Read_information_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(Read_default_settings_class.EntryType_by_value_text);
                        Read_information_myPanelLabel.Status = MyPanel_label_status_enum.Regular;
                    }
                    else
                    {
                        Read_information_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(Read_default_settings_class.Default_entryType);
                        Read_information_myPanelLabel.Status = MyPanel_label_status_enum.Regular;
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
            this.Read_directoryOrFile_outside_label.Refresh();
        }
        public void Read_allFilesInDirectory_ownCheckedBox_clicked()
        {
            Read_order_allFilesInDirectory_cbButton.Button_switch_to_positive();
            Read_order_onlySpecifiedFile_cbButton.SilentChecked = !Read_order_allFilesInDirectory_cbButton.Checked;
            Update_directoryOrFile_label();
        }
        public void Read_onlySpecifiedFile_ownCheckedBox_clicked()
        {
            Read_order_onlySpecifiedFile_cbButton.Button_switch_to_positive();
            Read_order_allFilesInDirectory_cbButton.SilentChecked = !Read_order_onlySpecifiedFile_cbButton.Checked;
            Update_directoryOrFile_label();
        }
        public void ColumName_specification_changed()
        {
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        #endregion

        #region Set to default column names buttons
        private void Set_color_of_setTo_defaultColumnName_button_to_activated(Button defaultColnames_button)
        {
            Get_lastPressed_back_and_fore_colors_for_default_column_name_button(out Color activated_backColor, out Color activated_foreColor);
            defaultColnames_button.ForeColor = activated_foreColor;
            defaultColnames_button.BackColor = activated_backColor;
            defaultColnames_button.Refresh();
        }
        private void Set_colors_of_all_buttons_for_default_colum_names_to_inactive()
        {
            this.Read_setTo_custom1_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_custom1_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_custom1_button.Refresh();
            this.Read_setTo_custom2_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_custom2_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_custom2_button.Refresh();
            this.Read_setTo_MBCO_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_MBCO_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_MBCO_button.Refresh();
            this.Read_setTo_minimum_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_minimum_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_minimum_button.Refresh();
            this.Read_setTo_optimum_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_optimum_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_optimum_button.Refresh();
            this.Read_setTo_singleCell_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Read_setTo_singleCell_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Read_setTo_singleCell_button.Refresh();
        }
        private bool Is_setToButton_active(Button setTo_button)
        {
            return setTo_button.BackColor.Equals(Form_default_settings.Color_button_notPressed_back);
        }
        private void Get_lastPressed_back_and_fore_colors_for_default_column_name_button(out Color activated_backColor, out Color activated_foreColor)
        {
            activated_backColor = Form_default_settings.Color_button_pressed_back;
            activated_foreColor = Form_default_settings.Color_button_pressed_fore;
        }

        private void Fill_texBox_columNames_from_datasetAttribute_columnName_dict(Dictionary<ReadDataMenu_datasetAttribute_enum, string> datasetAttribute_columName_dict)
        {
            ReadDataMenu_datasetAttribute_enum[] datasetAttributes = datasetAttribute_columName_dict.Keys.ToArray();
            foreach (ReadDataMenu_datasetAttribute_enum datasetAttribute in datasetAttributes)
            {
                DatasetAttribute_textBox_dict[datasetAttribute].SilentText_and_refresh = (string)datasetAttribute_columName_dict[datasetAttribute].Clone();
            }
        }
        public void Fill_textBox_columNames_from_datasetAttribtute_columnName_dict(int custom_column_name_no)
        {
            switch (custom_column_name_no)
            {
                case 1:
                    Fill_texBox_columNames_from_datasetAttribute_columnName_dict(this.DatasetAttribute_customColumnName_1_dict);
                    break;
                case 2:
                    Fill_texBox_columNames_from_datasetAttribute_columnName_dict(this.DatasetAttribute_customColumnName_2_dict);
                    break;
                default:
                    throw new Exception("Custom column name no (" + custom_column_name_no + ")  is not accepted.");
            }
        }
        public void SetTo_custom1_button_clicked()
        {
            Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected();
            Fill_texBox_columNames_from_datasetAttribute_columnName_dict(DatasetAttribute_customColumnName_1_dict);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(Read_setTo_custom1_button);
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_custom2_button_clicked()
        {
            Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected();
            Fill_texBox_columNames_from_datasetAttribute_columnName_dict(DatasetAttribute_customColumnName_2_dict);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(Read_setTo_custom2_button);
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        private void Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected()
        {
            if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_custom1_button))
            {
                ReadDataMenu_datasetAttribute_enum[] datasetAttributes = this.DatasetAttribute_customColumnName_1_dict.Keys.ToArray();
                foreach (ReadDataMenu_datasetAttribute_enum datasetAttribute in datasetAttributes)
                {
                    DatasetAttribute_customColumnName_1_dict[datasetAttribute] = (string)DatasetAttribute_textBox_dict[datasetAttribute].Text.Clone();
                }

            }
            else if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_custom2_button))
            {
                ReadDataMenu_datasetAttribute_enum[] datasetAttributes = this.DatasetAttribute_customColumnName_2_dict.Keys.ToArray();
                foreach (ReadDataMenu_datasetAttribute_enum datasetAttribute in datasetAttributes)
                {
                    DatasetAttribute_customColumnName_2_dict[datasetAttribute] = (string)DatasetAttribute_textBox_dict[datasetAttribute].Text.Clone();
                }

            }
        }
        public void SetTo_mbco_button_clicked()
        {
            Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected();
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Dataset name";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "NCBI official gene symbol";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "Timepoint";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "Timeunit";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "Value_1st";
            Read_color_ownTextBox.SilentText_and_refresh = "Dataset color";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "Value_2nd";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "Integration group";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(Read_setTo_MBCO_button);
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_singleCell_button_clicked()
        {
            Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected();
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "cluster";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "gene";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "avg_log2FC";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "p_val_adj";
            Read_color_ownTextBox.SilentText_and_refresh = "";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(Read_setTo_singleCell_button);
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_minimum_button_clicked()
        {
            Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected();
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "Gene";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "";
            Read_color_ownTextBox.SilentText_and_refresh = "";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(Read_setTo_minimum_button);
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        public void SetTo_optimum_button_clicked()
        {
            Copy_paste_entries_into_datasetAttribute_customColumn_name_dict_if_selected();
            Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Name";
            Read_geneSymbol_ownTextBox.SilentText_and_refresh = "Gene";
            Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
            Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
            Read_value1stColumn_ownTextBox.SilentText_and_refresh = "Value";
            Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "";
            Read_color_ownTextBox.SilentText_and_refresh = "";
            Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
            Read_timepointUnit_ownListBox.SilentSelectedIndex_and_topIndex = Read_timepointUnit_ownListBox.Items.IndexOf(Timeunit_enum.min);
            Read_delimiter_ownListBox.SilentSelectedIndex = Read_delimiter_ownListBox.Items.IndexOf(Read_default_settings_class.Get_delimiter_string(Read_file_delimiter_enum.Tab));
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(Read_setTo_optimum_button);
            Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible();
        }
        #endregion

        public void Set_explanation_tutorial_readData_buttons_to_inactive()
        {
            Error_reports_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Error_reports_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Error_reports_button.Refresh();
            Read_data_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Read_data_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Read_data_button.Refresh();
            Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Tutorial_button.Refresh();
        }
        public void Set_selected_explanation_tutorial_readData_button_to_active(Button selected_button)
        {
            selected_button.BackColor = Form_default_settings.Color_button_pressed_back;
            selected_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            selected_button.Refresh();
        }
        public bool Is_given_explanation_tuturial_or_readData_button_active(Button given_button)
        {
            return given_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
        }


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
            string text = "The ‘Read data’-menu allows for the quick upload of many datasets, background genes and user defined parameter settings." +
                  Form_default_settings.Explanation_text_major_separator +
                  "Input data directory of file name" +
                  "\r\nThe text box at the bottom of the application that is either labeled ‘Read all files in directory’ " +
                  "or ‘Read data file’ (in dependence of selections within the ‘Read data’-menu) allows for specification of the input data directory or file names." +
                  Form_default_settings.Explanation_text_major_separator +
                  "Definitions of column field names for user-supplied data files" +
                  "\r\nTo upload data spreadsheets into the application, the user needs to " +
                  "specify the names of the columns that contain the indicated values listed below the headline 'Enter column names of fields in data files'." +
                  "\r\nFields with empty text boxes will be ignored. If a spreadsheet does not contain a specified column name, the application will stop reading " +
                  "that file and generate an error message that the user can read after pressing the button 'Show errors'. The ‘Show errors’-button will replace the button " +
                  "'Explanation'. The user can either update the column name or delete it from the corresponding text box. Column names matching the following attributes " +
                  "can be defined:" +
                  "\r\n" +
                  "\r\nName:" +
                  "\r\nName of the dataset" +
                  "\r\n" +
                  "\r\nTime point:" +
                  "\r\nValues in the column specified by this text box will be uploaded into the timepoint field." +
                  "\r\n" +
                  "\r\nTime unit:\r\n" +
                  "A column that allows specification of a time unit. If the time unit text box is empty, but the time point text box contains a column name, " +
                  "a list box will appear that allows selection of a default time unit. The list box also contains all time units that can be selected: sec, min, hrs, days, " +
                  "weeks, months, years." +
                  "\r\n" +
                  "\r\nIntegration group:\r\n" +
                  "Enrichment results obtained from datasets assigned to the same integration group will be integrated into the same " +
                  "networks of subcellular processes (SCPs) and be visualized in the same heatmaps and timeline diagrams." +
                  "\r\n" +
                  "\r\nColor:" +
                  "\r\nThe user can upload dataset-specific colors by " +
                  "submitting either C# color names or colors in hexadecimal format (e.g. '#FF0000' for red). Color maps showing C# colors can be found by searching the internet for " +
                  "'C# colors'. Definition of hexadecimal colors allows visualization of MBC PathNet results in the same colors used in figures generated with other languages, e.g. R or " +
                  "Python. For example, R colors can be converted to hexadecimal colors using the function 'col2hex' that is part of the library 'gplots'. Hexadecimal colors will be mapped " +
                  "to the closest C# color, so they are represented by a color name known to the user." +
                  "\r\nDataset colors will be used to visualize which SCPs in the SCP-networks were predicted " +
                  "for which dataset. Additionally, the user can select in the menu panel 'Enrichment' to color bardiagrams and timelines in the dataset-specific colors as well." +
                  "\r\nThe C# colors 'White' and 'Transparent' are not accepted, since not predicted intermediate SCPs will be colored white. For quick color assignment after " +
                  "data upload use the menu 'Organize data'." +
                  "\r\n" +
                  "\r\nGene symbol:\r\n" + 
                  "This text box specifies the name of the column that contains the official NCBI gene symbols. It is the only " +
                  "text box that always needs to be assigned to a column name when uploading data files." +
                  "\r\n" +
                  "\r\n1st Value:\r\n" +
                  "The column specified in this text box contains those values that " +
                  "define if a gene is up- or downregulated. In case of a typical transcriptomics or proteomics experiment, it should contain the log2(fold changes). Genes with positive or " +
                  "negative 1st values will automatically be labeled as up- or downregulated, respectively. Genes with 1st values of zero will be removed. Up- and downregulated genes will become " +
                  "part of different datasets that have the same names and timepoints, but differ in their Up/Down status. If this text box is left empty, the 1st value of all uploaded genes will be " +
                  "assigned as one and the genes consequently be labeled as upregulated." +
                  "\r\n" +
                  "\r\n2nd Value:\r\n" +
                  "The user can specify another column containing experimental values. In contrast to the 1st values " +
                  "the 2nd values have no influence on dataset assignment but can be used to define significant genes. The menu 'Set data cutoffs' allows definition of significance and rank cutoffs using " +
                  "the 1st and 2nd values. In a typical transcriptomic or proteomic experiment, the 2nd value column should contain p-values or adjusted p-values." +
                  "\r\n" +
                  "\r\nDataset order #:\r\n" +
                  "If MBCO column names are selected, the application will also search for the column name 'Dataset order #' and import those " +
                  "integer numbers, if the column is found. This allows reimport of annotated order #s from the 'Input_Data' subdirectory in the 'Results' folder. Dataset " +
                  "order numbers influence the order of datasets within the same integration group in bardiagrams, heatmaps and pie charts (starting at 3h). They can be specified " +
                  "in the menu 'Organize data'. Due to space limitations, we did not add an extra field to allow users to specify their own column name for the order numbers." +
                  Form_default_settings.Explanation_text_major_separator +
                  "Default column names" +
                  "\r\nThe user can select predefined column name entries. Pressing one of these buttons will change the text box entries above." +
                  "\r\nAny changes to the 'Custom 1' and 'Custom 2' default column names will be saved by the application, if a file was successfully uploaded using those names." +
                  "Saved custom column names will be reloaded into the application after its restart.\r\nThe 'MBCO' default column names allow re-upload of analyzed data into the " +
                  "application. Any data that was subjected to analysis by pressing the 'Analyze' button in the middle of the application will be saved in the results folder 'Input_data'. " +
                  "Select the check box 'Read all files in directory' and copy the full path of the 'Input_data' directory into the text box 'Read all data files in directory'. In windows, " +
                  "the full path can be easily accessed by pressing the path with a mouse button. Pressing the ‘Read’-button will upload the data, the experimental background genes and the " +
                  "saved parameter settings back into the application. If the MBCO button is highlighted, the saved and uploaded data will also contain the dataset order numbers, even though " +
                  "the column is not mentioned in the menu panel." +
                  Form_default_settings.Explanation_text_major_separator +
                  "Automatic import of parameter settings" +
                  "\r\nThe application will always search the specified data input directory for a " +
                  "parameter settings file named “MBC_pathNet_parameter_settings.txt”. This file is generated during each data analysis and saved in the ‘Input_data’-folder within the specified results " +
                  "directory. This not only allows the user to re-import input data and parameter settings of a previous analysis from that ‘Input_data’-folder but also upload predefined parameter settings " +
                  "with any new dataset. Simply, generate a parameter settings file after specification of desired parameters by analyzing any list of DEGs (e.g., manually upload the gene ‘AAK1’, using the " +
                  "‘Gene list’ text box on the left side of the application). The generated “MBC_pathNet_parameter_settings.txt” can be copied into any input directory. Though external modification is possible, " +
                  "we recommend using the application for parameter specifications to avoid potential errors during external modification of the file. If an externally modified parameter settings file contains an " +
                  "error, the application will interrupt data upload and allow investigation of the encountered error after pressing the ‘Show errors’-button that will replace the ‘Explanation’-button." +
                  Form_default_settings.Explanation_text_major_separator +
                  "Read all files/only specified file" +
                  "\r\nFor quick upload of multiple data files, the user can select that all files in a particular directory will be read." +
                  "\r\n" +
                  "\r\nIf ‘Read all files in directory’ is selected, this mode will upload all data files and files with background genes (as well as the parameter settings file, if existent)." +
                  "\r\n" +
                  "\r\nData files must contain the column names specified below 'Enter column names of fields in data files', as described above. The file name that contains each uploaded dataset " +
                  "will be saved in the 'Source' field within the application (see menu 'Organize data')." +
                  "\r\n" +
                  "\r\nFiles containing background genes must end with '_bgGenes' and should contain one column with genes and no headline. After uploading background genes, the application will " +
                  "automatically map them to the datasets that were uploaded from a file with the same name (as saved in the source field), except without the '_bgGenes' ending. The automatic upload " +
                  "and mapping of background genes mimics the manual upload and mapping in the menu 'Background genes', using the buttons 'Read' and 'Automatically assign background genes'. Please " +
                  "consider that the background genes should contain all genes that were tested for significance and -consequently- must contain all genes that are part of the related datasets. For " +
                  "more details, please read the explanation in the menu 'Background genes'." +
                  "\r\n" +
                  "\r\nFiles with missing column names, wrongly formatted background genes or wrongly formatted parameter setting " +
                  "files will not be uploaded. For each encountered error an error message will be generated that can be investigated after pressing the ‘Show error’-button that will replace the ‘Explanation’-button." +
                  "\r\n" +
                  "\r\nTo reupload datasets, background genes and parameter settings of a previous analysis, simply copy the directory path leading to the ‘Input data’-directory within the results folder into the ‘Read " +
                  "all files in directory’-text box, select ‘Read all files in directory’ and press the ‘Read’-button. In Windows, the pathname is easily accessible after clicking the path with the mouse." +
                  "\r\n" +
                  "\r\nIf ‘Read only specified file’ is selected, the application will only upload that data file and ignore all other files or files with background genes. However, the application will always " +
                  "search for a parameter settings file, independently of the order to read all or only the specified file. Encountered errors will be handled in the same way.";

            Error_reports_ownTextBox.SilentText_and_refresh = text;
            int left = Error_reports_ownTextBox.Location.X;
            int right = left + Error_reports_ownTextBox.Width;
            int top = Error_reports_ownTextBox.Location.Y;
            int bottom = top + Error_reports_ownTextBox.Height;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToExactPositions_add_default_parameter(Error_reports_ownTextBox, left, right, top, bottom);
        }

        private void Update_error_reports_or_explanations(params string[] additional_comments)
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
            bool directory_does_not_contain_user_files = false;
            bool only_main_drive_specified = false;
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
                    if (!String.IsNullOrEmpty(System.IO.Path.GetExtension(error_message_line.Complete_fileName)))
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
                    if (directory_does_not_contain_user_files) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    invalid_spelling_of_directory = true;
                    report_error_description = false;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Wrong_ending_of_directoryFileName))
                {
                    if (directory_does_not_exist) { throw new Exception(); }
                    if (directory_does_not_contain_user_files) { throw new Exception(); }
                    if (only_main_drive_specified) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    wrong_ending_of_directory_fileName = true;
                    report_error_description = false;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Directory_does_not_exist))
                {
                    if (directory_does_not_exist) { throw new Exception(); }
                    if (directory_does_not_contain_user_files) { throw new Exception(); }
                    if (only_main_drive_specified) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    directory_does_not_exist = true;
                    report_error_description = false;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Directory_contains_no_user_files))
                {
                    if (directory_does_not_exist) { throw new Exception(); }
                    if (directory_does_not_contain_user_files) { throw new Exception(); }
                    if (only_main_drive_specified) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    directory_does_not_contain_user_files = true;
                    report_error_description = false;
                }
                else if (error_message_line.Error_message.Equals(Read_error_message_enum.Main_drive_not_allowed_as_directory))
                {
                    if (directory_does_not_exist) { throw new Exception(); }
                    if (directory_does_not_contain_user_files) { throw new Exception(); }
                    if (only_main_drive_specified) { throw new Exception(); }
                    if (invalid_spelling_of_directory) { throw new Exception(); }
                    if (wrong_ending_of_directory_fileName) { throw new Exception(); }
                    only_main_drive_specified = true;
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
                                    error_text = "File does not exist in given directory.";
                                }
                                else
                                {
                                    error_text = "No file specified.";
                                }
                                break;
                            case Read_error_message_enum.File_cannot_be_opened:
                                error_text = "File cannot be opened. Is it open in another program?";
                                break;
                            case Read_error_message_enum.File_does_not_contain_text:
                                error_text = "No text found in file.";
                                break;
                            case Read_error_message_enum.Defined_columnNames_are_missing:
                                error_text = "Headline is missing column names: " + error_message_line.Value + ". Check for typos or delete missing column name(s) in 'Read data' menu.";
                                break;
                            case Read_error_message_enum.Length_column_entries_not_equal_length_column_names:
                                error_text = "Number of columns in line " + error_message_line.LineIndex + " differs from number of headline column names.";
                                break;
                            case Read_error_message_enum.Not_a_float_or_double:
                                error_text = "Value (" + error_message_line.Value + ") is not a number in line " + error_message_line.LineIndex + ", column '" + error_message_line.ColumnName + "'.";
                                break;
                            case Read_error_message_enum.Not_an_integer:
                                error_text = "Value (" + error_message_line.Value + ") is not an integer in line " + error_message_line.LineIndex + ", column '" + error_message_line.ColumnName + "'.";
                                break;
                            case Read_error_message_enum.Wrong_value_type:
                                error_text = "Value (" + error_message_line.Value + ") is not of requiered value type in line " + error_message_line.LineIndex + ", column '" + error_message_line.ColumnName + "'.";
                                break;
                            case Read_error_message_enum.Not_part_of_enum:
                                error_text = "Value (" + error_message_line.Value + ") is not convertable into available selections in line " + error_message_line.LineIndex + ", column '" + error_message_line.ColumnName + "'.";
                                break;
                            case Read_error_message_enum.Delimiter_seems_wrong:
                                Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                                error_text = "Selected column delimiter seems to be wrong. If parameter settings file, first line has to be '" + gdf.FirstLine_of_mbco_parameter_setting_fileName + "'";
                                break;
                            case Read_error_message_enum.File_name_already_uploaded:
                                error_text = "Uploaded data already contains datasets from this source file name.";
                                break;
                            case Read_error_message_enum.Maximum_number_of_datasets_exceeded:
                                error_text = "Maximum number of datasets exceeded.";
                                break;
                            case Read_error_message_enum.Duplicated_entry:
                                error_text = "Duplicated entry: " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.Timeunit_not_recognized:
                                error_text = "Timeunit in line " + error_message_line.LineIndex + " not recognized: " + error_message_line.Value;
                                break;
                            case Read_error_message_enum.All_values_in_column_specified_for_1st_value_are_zero:
                                error_text = "All values in column specified to contain 1st values are zero.";
                                break;
                            case Read_error_message_enum.Multiple_integration_group_assignments_for_dataset:
                                error_text = "Multiple integration group assignments to " + error_message_line.Value + ".";
                                break;
                            case Read_error_message_enum.Multiple_color_assignments_for_dataset:
                                error_text = "Multiple color assignments to " + error_message_line.Value + ".";
                                break;
                            case Read_error_message_enum.Multiple_resultNo_assignments_for_dataset:
                                error_text = "Multiple dataset order # assignments to " + error_message_line.Value + ".";
                                break;
                            case Read_error_message_enum.Duplicated_bggenes_dataset:
                                error_text = "Bg gene list with same name already exists.";
                                break;
                            case Read_error_message_enum.Not_an_accepted_color:
                                error_text = error_message_line.Value + " (e.g., in line " + error_message_line.LineIndex + ") is not an accepted color.";
                                break;
                            case Read_error_message_enum.Custom_data_array_too_long:
                                error_text = "New data cannot be added, since existing data size is close to maximum (" + error_message_line.Value + "), remove not significant  genes using menu panel 'Set data cutoffs'.";
                                break;
                            case Read_error_message_enum.Directory_does_not_exist:
                                error_text = "Directory does not exist (" + error_message_line.Value + ") (" + error_message_line.Complete_fileName + ").";
                                break;
                            case Read_error_message_enum.Directory_contains_no_user_files:
                                error_text = "Directory does not contain user-supplied files (" + error_message_line.Value + ") (" + error_message_line.Complete_fileName + ").";
                                break;
                            case Read_error_message_enum.Field_contains_no_entry:
                                error_text = "Field in line " + error_message_line.LineIndex + ", column '" + error_message_line.ColumnName + "' contains no entry.";
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
            Error_reports_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("");
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
            if (directory_does_not_contain_user_files)
            {
                error_reports_text_list.Add("Directory does not contain user-supplied files.");
            }
            if (only_main_drive_specified)
            {
                error_reports_text_list.Add("Selecting only the main drive is not allowed. Please choose a subfolder.");
            }
            if (invalid_spelling_of_directory)
            {
                error_reports_text_list.Add("Invalid spelling of directory or file name.");
            }
            if (wrong_ending_of_directory_fileName)
            {
                error_reports_text_list.Add("Please ensure file name ends with extension\r\n(e.g. \".txt\") and directory name with \"\\\"");
            }
            if (additional_comments.Length>0)
            {
                foreach (string additional_comment in additional_comments)
                {
                    error_reports_text_list.Add(additional_comment);
                }
            }
            if (error_reports_text_list.Count==0)
            {
                error_reports_text_list.Add(Read_default_settings_class.Backgrounod_genes_file_info);
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
                    error_reports_sb.AppendFormat("{0} {1}", error_reports_text_list[0], error_reports_text_list[1]);
                    int error_reports_length = error_reports_text_list.Count;
                    for (int indexER=2; indexER<error_reports_length;indexER++)
                    {
                        error_reports_sb.AppendFormat("\r\n{0}", error_reports_text_list[indexER]);
                    }
                }
                Error_reports_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(error_reports_sb.ToString());
                Adjust_sizes_of_error_reports_label();
            }
            if ((files_with_error_reports>0)||(bg_files_with_error_reports>0))
            {
                Switch_to_error_mode();
            }
            else
            {
                Switch_to_explanation_mode();
            }
        }
        public bool Is_error_mode()
        {
            return Error_reports_button.Text.Equals(Read_default_settings_class.ErrorReportsExplanationButtonText_errorText);
        }
        public string Get_deep_clone_of_entry_of_explanationErrorReport_panel()
        {
            return (string)Error_reports_ownTextBox.Text.Clone();
        }
        public string Get_deep_clone_of_entry_of_errorReportsMyPanel()
        {
            return (string)Error_reports_myPanelLabel.FullSize_label.Text.Clone();
        }

        private void Switch_to_error_mode()
        {
            Error_reports_button.Text = (string)Read_default_settings_class.ErrorReportsExplanationButtonText_errorText.Clone();
            Errors_reports_headline_label.Text = "Error messages";
        }
        private void Switch_to_explanation_mode()
        {
            Error_reports_button.Text = (string)Read_default_settings_class.ErrorReportsExplanationButtonText_explanationText.Clone();
            Add_explanations_to_error_reportBox();
        }

        public string Extract_error_reports_if_shown_from_error_reportBox()
        {
            string error_reports = "";
            if (Error_reports_button.Text.Equals(Read_default_settings_class.ErrorReportsExplanationButtonText_errorText))
            {
                error_reports = (string)Error_reports_ownTextBox.Text.Clone();
            }
            return error_reports;
        }

        public void Add_to_error_label_and_error_box_and_switch_to_error_mode_if_not_already(string error_label_text, string[] add_text_lines)
        {
            StringBuilder sb = new StringBuilder();
            int add_text_lines_length = add_text_lines.Length;
            string add_text_line;
            for (int indexAdd=0; indexAdd<add_text_lines_length; indexAdd++)
            {
                if (indexAdd==0) { sb.Append("\r\n"); }
                add_text_line = add_text_lines[indexAdd];
                sb.AppendLine(add_text_line);
            }

            if (!Is_error_mode())
            {
                Switch_to_error_mode();
                Error_reports_ownTextBox.SilentText = sb.ToString();
            }
            else
            {
                Error_reports_ownTextBox.SilentText = Error_reports_ownTextBox.Text + "\r\n\r\n" + sb.ToString();
            }
            Error_reports_myPanelLabel.Set_silent_text_adjustFontSize_and_refresh(Error_reports_myPanelLabel.Get_text() + "\r\n" + error_label_text, Form_default_settings);
        }
        #endregion

        #region Tutorial
        public void Tutorial_button_pressed()
        {
            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel;
            int mid_y_position;
            int right_x_position;
            string text;
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();


            right_x_position_next_to_overall_panel = this.Read_overall_groupBox.Location.X - distance_from_overalMenueLabel;
            Dictionary<ReadDataMenu_datasetAttribute_enum, string> datasetAttribute_columnName_dict = Get_current_datasetAttribute_columnName_dict_from_userEntries();

            Button active_setToDefault_button = Get_activated_setTo_default_columnNames_button();
            bool read_all_files_pressed = Read_order_allFilesInDirectory_cbButton.Checked;
            bool read_one_file_pressed = Read_order_onlySpecifiedFile_cbButton.Checked;
            if (read_all_files_pressed.Equals(read_one_file_pressed)) { throw new Exception(); }

            int defaultNamesButton_upper_position = this.Read_overall_groupBox.Location.Y + this.Read_setTo_custom1_button.Location.Y;
            int defaultNamesButton_lower_position = this.Read_overall_groupBox.Location.Y + this.Read_setTo_custom2_button.Location.Y + this.Read_setTo_custom2_button.Height;
            int defaultNamesButton_height = defaultNamesButton_lower_position - defaultNamesButton_upper_position;
            int mid_x_position;
            int bottom_y_position;

            bool end_tour = false;
            int explanation_index = -1;
            bool escape_pressed = false;
            bool back_pressed = false;

            string current_entry_readDirectoryOfFile_textBox = (string)Read_directoryOrFile_outside_ownTextBox.Text.Clone();


            while (!end_tour)
            {
                explanation_index++;
                switch (explanation_index)
                {
                    case -1:
                        end_tour = true;
                        break;
                    #region Explain column name boxes
                    case 0:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + (int)Math.Round(0.5 * (this.Read_value2ndColumn_ownTextBox.Location.Y + this.Read_value2ndColumn_ownTextBox.Height - this.Read_sampleNameColumn_ownTextBox.Location.Y));

                        text = "Define the column names in your file that map to the fields in the application.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "Define";
                        Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "column names";
                        Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
                        Read_color_ownTextBox.SilentText_and_refresh = "";
                        Read_geneSymbol_ownTextBox.SilentText_and_refresh = "in your file";
                        Read_value1stColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "here";
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + this.Read_integrationGroup_ownTextBox.Location.Y + (int)Math.Round(0.5 * (this.Read_integrationGroup_ownTextBox.Height));
                        text = "Integration groups define datasets for which the results will be integrated.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_integrationGroup_ownTextBox.SilentText_and_refresh = "Define integration groups";
                        Read_color_ownTextBox.SilentText_and_refresh = "";
                        Read_geneSymbol_ownTextBox.SilentText_and_refresh = "";
                        Read_value1stColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "";
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + this.Read_color_ownTextBox.Location.Y + (int)Math.Round(0.5 * (this.Read_color_ownTextBox.Height));
                        text = "Accepted colors are C# color names or hex color codes.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Read_sampleNameColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_timepointColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_timeunitColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_integrationGroup_ownTextBox.SilentText_and_refresh = "";
                        Read_geneSymbol_ownTextBox.SilentText_and_refresh = "";
                        Read_value1stColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_value2ndColumn_ownTextBox.SilentText_and_refresh = "";
                        Read_color_ownTextBox.SilentText_and_refresh = "e.g. hex color codes";
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    #endregion
                    #region Explain default column name buttons
                    case 3:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = (int)Math.Round(defaultNamesButton_upper_position + 0.5 * defaultNamesButton_height);

                        text = "Buttons allow selection of predefined column names.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
                        SetTo_singleCell_button_clicked();
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 4:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = (int)Math.Round(defaultNamesButton_upper_position + 0.5 * defaultNamesButton_height);
                        text = "Any names defined after selection of a custom button will be saved for reupload after restart, if they allowed successful data upload.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        SetTo_custom1_button_clicked();
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 5:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = (int)Math.Round(defaultNamesButton_upper_position + 0.5 * defaultNamesButton_height);
                        text = "The only mandatory column name is shown after pressing the 'Minimum'-button.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        SetTo_minimum_button_clicked();
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    #endregion
                    #region Explain read file or all files in directory
                    case 6:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + (int)Math.Round(Read_order_allFilesInDirectory_cbButton.Location.Y
                                                                                                 + (0.5 * (Read_order_onlySpecifiedFile_cbButton.Location.Y
                                                                                                           + Read_order_onlySpecifiedFile_cbButton.Height
                                                                                                           - Read_order_allFilesInDirectory_cbButton.Location.Y)));
                        text = "Select if the application should read all files in a given directory or only one given file.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Read_onlySpecifiedFile_ownCheckedBox_clicked();
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    case 7:
                        int upper_label_position = Read_order_onlySpecifiedFile_cbButton.Location.Y + Read_order_onlySpecifiedFile_cbButton.Height;
                        int lower_label_position = Tutorial_button.Location.Y;

                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + (int)Math.Round(Error_reports_myPanelLabel.Location.Y
                                                                                                 + (0.5 * (Error_reports_myPanelLabel.Height)));
                        text = "If instructed to read all files, the application will also upload files with background genes and map them to related datasets.";
                        Read_allFilesInDirectory_ownCheckedBox_clicked();
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    #endregion
                    #region Explain text box for read file or directory entry
                    case 8:
                        mid_x_position = Read_directoryOrFile_outside_ownTextBox.Location.X + (int)Math.Round(0.5F * Read_directoryOrFile_outside_ownTextBox.Width);

                        bottom_y_position = Read_directoryOrFile_outside_label.Location.Y;
                        text = "Specify the name of the file or directory in the text box below.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        Read_directoryOrFile_outside_ownTextBox.SilentText_and_refresh = "Enter file or directory name here.";
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    case 9:
                        if (!Form_default_settings.Is_mono)
                        {
                            mid_x_position = Read_directoryOrFile_outside_ownTextBox.Location.X + (int)Math.Round(0.5F * Read_directoryOrFile_outside_ownTextBox.Width);
                            bottom_y_position = Read_directoryOrFile_outside_label.Location.Y;
                            text = "Directory paths can be copied from the address bar at the top of the Windows File Explorer by clicking the bar to reveal the full path and selecting it with the mouse.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                            UserInterface_tutorial.Set_to_invisible();
                        }
                        Read_directoryOrFile_outside_ownTextBox.SilentText_and_refresh = (string)current_entry_readDirectoryOfFile_textBox.Clone();
                        break;
                    #endregion
                    #region Reupload of input data and parameter settings
                    case 10:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + (int)Math.Round(Read_setTo_MBCO_button.Location.Y
                                                                                                 + (0.5 * (Read_setTo_MBCO_button.Height)));
                        text = "Analyzed datasets, related background genes and user-selected parameters will be saved in a subfolder 'Input_data' within the results folder.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    case 11:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + (int)Math.Round(Read_setTo_MBCO_button.Location.Y
                                                                                                 + (0.5 * (Read_setTo_MBCO_button.Height)));
                        text = "Selecting 'MBCO'-default names and 'Read all files in directory' enables automatic re-import of data and parameters from the 'Input_data' directory.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Read_allFilesInDirectory_ownCheckedBox_clicked();
                        SetTo_mbco_button_clicked();
                        Read_directoryOrFile_outside_ownTextBox.SilentText_and_refresh = ".../Results/Input_data/";
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    case 12:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Read_overall_groupBox.Location.Y + (int)Math.Round(Error_reports_myPanelLabel.Location.Y
                                                                                                 + (0.5 * (Error_reports_myPanelLabel.Height)));
                        text = "The parameter settings file ('" + gdf.Mbco_parameter_settings_fileName + "' in the 'Input_data'-subfolder) can be copy-pasted into any input data directory to enable parameter import and serve as a template for external parameter definition.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Read_allFilesInDirectory_ownCheckedBox_clicked();
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    #endregion
                    #region Explain press read button
                    case 13:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Read_overall_groupBox.Location.Y + Read_data_button.Location.Y + (int)Math.Round(0.5F * Read_data_button.Height);
                        text = "Press the 'Read'-button to upload the data.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Set_explanation_tutorial_readData_buttons_to_inactive();
                        Set_selected_explanation_tutorial_readData_button_to_active(Read_data_button);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_explanation_tutorial_readData_buttons_to_inactive();
                        Set_selected_explanation_tutorial_readData_button_to_active(Tutorial_button);
                        UserInterface_tutorial.Set_to_invisible();
                        break;
                    #endregion
                    case 14:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Read_overall_groupBox.Location.Y + Read_data_button.Location.Y + (int)Math.Round(0.5F * Read_data_button.Height);
                        text = "In addition to the specified custom column names, the application saves the file or directory name, the selected predefined column names, and whether to upload one or all files, allowing reimport after restart.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Set_explanation_tutorial_readData_buttons_to_inactive();
                        Set_selected_explanation_tutorial_readData_button_to_active(Read_data_button);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_explanation_tutorial_readData_buttons_to_inactive();
                        Set_selected_explanation_tutorial_readData_button_to_active(Tutorial_button);
                        break;
                    case 15:
                    default:
                        end_tour = true;
                        break;
                }
                if (back_pressed) { explanation_index = explanation_index - 2; }
                if (escape_pressed || explanation_index == -2) { end_tour = true; }
            }

            UserInterface_tutorial.Set_to_invisible();
            Set_colors_of_all_buttons_for_default_colum_names_to_inactive();
            Set_color_of_setTo_defaultColumnName_button_to_activated(active_setToDefault_button);
            Fill_texBox_columNames_from_datasetAttribute_columnName_dict(datasetAttribute_columnName_dict);

            if (read_all_files_pressed) { Read_allFilesInDirectory_ownCheckedBox_clicked(); }
            else if (read_one_file_pressed) { Read_onlySpecifiedFile_ownCheckedBox_clicked(); }
            else { throw new Exception(); }

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

        private bool Is_readSetTo_defaultColumnNames_activated(Button setTo_defaultName_button)
        {
            return setTo_defaultName_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
        }

        private Button Get_activated_setTo_default_columnNames_button()
        {
            Button button = new Button();
            bool activated_button_found = false;
            Button current_defaultButton = Read_setTo_custom1_button;
            if (Is_readSetTo_defaultColumnNames_activated(current_defaultButton))
            {
                if (activated_button_found) { throw new Exception(); }
                button = current_defaultButton;
                activated_button_found = true;
            }
            current_defaultButton = Read_setTo_custom2_button;
            if (Is_readSetTo_defaultColumnNames_activated(current_defaultButton))
            {
                if (activated_button_found) { throw new Exception(); }
                button = current_defaultButton;
                activated_button_found = true;
            }
            current_defaultButton = Read_setTo_MBCO_button;
            if (Is_readSetTo_defaultColumnNames_activated(current_defaultButton))
            {
                if (activated_button_found) { throw new Exception(); }
                button = current_defaultButton;
                activated_button_found = true;
            }
            current_defaultButton = Read_setTo_singleCell_button;
            if (Is_readSetTo_defaultColumnNames_activated(current_defaultButton))
            {
                if (activated_button_found) { throw new Exception(); }
                button = current_defaultButton;
                activated_button_found = true;
            }
            current_defaultButton = Read_setTo_minimum_button;
            if (Is_readSetTo_defaultColumnNames_activated(current_defaultButton))
            {
                if (activated_button_found) { throw new Exception(); }
                button = current_defaultButton;
                activated_button_found = true;
            }
            current_defaultButton = Read_setTo_optimum_button;
            if (Is_readSetTo_defaultColumnNames_activated(current_defaultButton))
            {
                if (activated_button_found) { throw new Exception(); }
                button = current_defaultButton;
                activated_button_found = true;
            }
            return button;
        }
        private Dictionary<ReadDataMenu_datasetAttribute_enum, string> Get_current_datasetAttribute_columnName_dict_from_userEntries()
        {
            Dictionary<ReadDataMenu_datasetAttribute_enum, string> datasetAttribute_columnName_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, string>();
            ReadDataMenu_datasetAttribute_enum[] datasetAttributes = DatasetAttribute_textBox_dict.Keys.ToArray();
            foreach (ReadDataMenu_datasetAttribute_enum datasetAttribute in datasetAttributes)
            {
                datasetAttribute_columnName_dict.Add(datasetAttribute, (string)DatasetAttribute_textBox_dict[datasetAttribute].Text.Clone());
            }
            return datasetAttribute_columnName_dict;
        }
        public Custom_data_class Read_button_click(Custom_data_class custom_data, 
                                                   string defaultIntegrationGroup, 
                                                   Color[] selectable_colors,
                                                   Override_used_selected_column_names_enum override_user_selected_column_names,
                                                   out string[] read_parameter_settings
                                                   )
        {
            read_parameter_settings = new string[0];
            Set_explanation_tutorial_readData_buttons_to_inactive();
            Set_selected_explanation_tutorial_readData_button_to_active(Read_data_button);
            bool always_proceed = false;
            switch (override_user_selected_column_names)
            {
                case Override_used_selected_column_names_enum.Default_custom_1_names:
                case Override_used_selected_column_names_enum.Default_custom_2_names:
                case Override_used_selected_column_names_enum.Example_dataset_no:
                    always_proceed = true;
                    break;
                case Override_used_selected_column_names_enum.No:
                    break;
                default:
                    throw new Exception();
            }


            if ((always_proceed)||(Update_labels_and_backcolor_of_textBoxes_and_return_if_proceedToReadIsPossible()))
            {
                bool Continue = true;
                List<Read_error_message_line_class> all_error_messages_list = new List<Read_error_message_line_class>();
                List<Read_error_message_line_class> currentFile_error_messages_list = new List<Read_error_message_line_class>();
                string directory = "";
                Read_directoryOrFile_outside_ownTextBox.SilentText = Global_dirFile.Transform_into_compatible_directory_and_clean_up(Read_directoryOrFile_outside_ownTextBox.Text);
                string inputDirectoryFileName_that_will_also_be_saved = Read_directoryOrFile_outside_ownTextBox.Text;

                if (  (!inputDirectoryFileName_that_will_also_be_saved[inputDirectoryFileName_that_will_also_be_saved.Length - 1].Equals(Global_dirFile.Delimiter))
                    &&(System.IO.Path.GetExtension(inputDirectoryFileName_that_will_also_be_saved).Length==0))
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
                        if (System.IO.Directory.Exists(inputDirectoryFileName_that_will_also_be_saved))
                        {
                            directory = inputDirectoryFileName_that_will_also_be_saved + Global_dirFile.Delimiter;
                        }
                        else
                        {
                            directory = System.IO.Path.GetDirectoryName(inputDirectoryFileName_that_will_also_be_saved) + Global_dirFile.Delimiter;
                        }
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
                    else
                    {
                        string fullPath = System.IO.Path.GetFullPath(directory).TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
                        string root = System.IO.Path.GetPathRoot(fullPath)?.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
                        if (string.Equals(fullPath, root, StringComparison.OrdinalIgnoreCase))
                        {
                            Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                            new_error_message.File_type = Read_file_type.Data;
                            new_error_message.Complete_fileName = (string)directory.Clone();
                            new_error_message.Error_message = Read_error_message_enum.Main_drive_not_allowed_as_directory;
                            all_error_messages_list.Add(new_error_message);
                            Continue = false;
                        }
                    }

                }
                Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                if (Continue)
                {
                    complete_fileNames = System.IO.Directory.GetFiles(directory);
                    complete_fileNames = gdf.Remove_appGenerated_fileNames(complete_fileNames);
                    if (complete_fileNames.Length == 0)
                    {
                        Continue = false;
                        Read_error_message_line_class new_error_message = new Read_error_message_line_class();
                        new_error_message.File_type = Read_file_type.Data;
                        new_error_message.Complete_fileName = (string)directory.Clone();
                        new_error_message.Error_message = Read_error_message_enum.Directory_contains_no_user_files;
                        all_error_messages_list.Add(new_error_message);
                    }
                }
                string add_info_to_report_label = "";
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
                    List<string> potential_key_propertyNames_list = new List<string>();
                    List<string> potential_key_columnNames_list = new List<string>();
                    Dictionary<ReadDataMenu_datasetAttribute_enum, string> referenceDatasetAttribute_columnName_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, string>();
                    switch (override_user_selected_column_names)
                    {
                        case Override_used_selected_column_names_enum.No:
                        case Override_used_selected_column_names_enum.Example_dataset_no:
                            referenceDatasetAttribute_columnName_dict = Get_current_datasetAttribute_columnName_dict_from_userEntries();
                            Get_lastPressed_back_and_fore_colors_for_default_column_name_button(out Color activated_backColor, out Color activated_foreColor);
                            if (Read_setTo_MBCO_button.BackColor.Equals(activated_backColor))
                            {
                                potential_key_propertyNames_list.Add("Results_number");
                                potential_key_columnNames_list.Add(Custom_data_readWriteOptions_class.ColumName_results_order_no_for_data);
                            }
                            break;
                        case Override_used_selected_column_names_enum.Default_custom_1_names:
                            referenceDatasetAttribute_columnName_dict = DatasetAttribute_customColumnName_default_1_dict;
                            break;
                        case Override_used_selected_column_names_enum.Default_custom_2_names:
                            referenceDatasetAttribute_columnName_dict = DatasetAttribute_customColumnName_default_2_dict;
                            break;
                        default:
                            throw new Exception();
                    }
                    ReadDataMenu_datasetAttribute_enum[] datasetAttributes = referenceDatasetAttribute_columnName_dict.Keys.ToArray();
                    string columnName;
                    foreach (ReadDataMenu_datasetAttribute_enum datasetAttribute in datasetAttributes)
                    {
                        columnName = (string)referenceDatasetAttribute_columnName_dict[datasetAttribute].Clone();
                        if (!String.IsNullOrEmpty(columnName))
                        {
                            key_propertyNames_list.Add(DatasetAttribute_propertyName_dict[datasetAttribute]);
                            key_columnNames_list.Add(columnName);
                            if (datasetAttribute.Equals(ReadDataMenu_datasetAttribute_enum.Value_1st))
                            {
                                column_for_first_value_specified = true;
                            }
                        }
                    }
                    #endregion

                    Dictionary<string, bool> existing_sourceFileNames_dict = custom_data.Get_sourceFileNames_dict();

                    int custom_datasets_count = custom_data.Get_all_unique_ordered_sampleNames().Length;
                    int add_custom_datasets_count;
                    for (int indexC = 0; indexC < complete_fileNames_length; indexC++)
                    {
                        currentFile_error_messages_list.Clear();
                        complete_fileName = complete_fileNames[indexC];
                        complete_fileName = gdf.Transform_into_compatible_directory_and_clean_up(complete_fileName);
                        fileName = System.IO.Path.GetFileName(complete_fileName);
                        fileName_without_extension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        bool is_mbco_paremeter_settings_file = fileName.Equals(Global_dirFile.Mbco_parameter_settings_fileName);
                        if (is_mbco_paremeter_settings_file)
                        {
                            System.IO.StreamReader reader = new System.IO.StreamReader(complete_fileName);
                            string firstLine = reader.ReadLine();
                            reader.Close();
                            if ((firstLine==null)||(!firstLine.Equals(Global_dirFile.FirstLine_of_mbco_parameter_setting_fileName)))
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
                            Read_error_message_line_class[] new_error_messages = custom_data.Read_and_add_background_genes_and_return_error_messages(complete_fileName, ProgressReport);
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
                        else if (  (  (!Read_order_allFilesInDirectory_cbButton.Checked)
                                    &&(Read_order_onlySpecifiedFile_cbButton.Checked)
                                    &&(complete_fileName.Equals(inputDirectoryFileName_that_will_also_be_saved)))
                                 ||(  (Read_order_allFilesInDirectory_cbButton.Checked)
                                    &&(!Read_order_onlySpecifiedFile_cbButton.Checked)))
                        {
                            ProgressReport.Update_progressReport_text_and_visualization("Reading " + fileName);
                            existing_sourceFileNames_dict.Add(fileName, true);
                            Custom_data_class add_custom_data = new Custom_data_class();
                            Custom_data_readWriteOptions_class readWriteOptions = new Custom_data_readWriteOptions_class(directory,fileName);
                            readWriteOptions.HeadlineDelimiters = new char[] { delimiter_char };
                            readWriteOptions.LineDelimiters = new char[] { delimiter_char };
                            readWriteOptions.Key_propertyNames = key_propertyNames_list.ToArray();
                            readWriteOptions.Key_columnNames = key_columnNames_list.ToArray();
                            readWriteOptions.Key_propertyNames_potential = potential_key_propertyNames_list.ToArray();
                            readWriteOptions.Key_columnNames_potential = potential_key_columnNames_list.ToArray();

                            Read_error_message_line_class[] error_messages_read = add_custom_data.Generate_custom_data_instance_if_no_errors_and_return_error_messages(column_for_first_value_specified, readWriteOptions, ProgressReport);
                            
                            currentFile_error_messages_list.AddRange(error_messages_read);
                            if (add_custom_data.Custom_data.Length > 0)
                            {
                                Read_error_message_line_class[] error_messages_timeunit = add_custom_data.Set_timeunits_based_on_timeunit_strings_and_return_error_messages(readWriteOptions.File, Read_timeunitColumn_ownTextBox.Text, readWriteOptions.Max_error_messages - currentFile_error_messages_list.Count);
                                currentFile_error_messages_list.AddRange(error_messages_timeunit);
                                UploadedFileNo++;
                                add_custom_data.Set_unique_fixed_dataset_identifier_after_reading(UploadedFileNo);
                                Read_error_message_line_class[] error_messages_duplicated = add_custom_data.Analyze_if_any_duplicated_lines_based_on_uniqueFixedDatasetIdentifier_and_geneSymbol(complete_fileName, readWriteOptions.Max_error_messages - currentFile_error_messages_list.Count);
                                currentFile_error_messages_list.AddRange(error_messages_duplicated);
                                Read_error_message_line_class[] error_messages_duplicated2 = add_custom_data.Analyze_if_all_sampleColors_resultNumbers_integrationGroups_are_identical_for_each_uniqueFixedDatasetIdentifier_and_geneSymbol(complete_fileName, readWriteOptions.Max_error_messages - currentFile_error_messages_list.Count);
                                currentFile_error_messages_list.AddRange(error_messages_duplicated2);
                            }
                            if (currentFile_error_messages_list.Count == 0)
                            {
                                Timeunit_enum default_timeunit = (Timeunit_enum)Enum.Parse(typeof(Timeunit_enum), Read_timepointUnit_ownListBox.SelectedItem.ToString());
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

                                if (  (Form_default_settings.Is_button_pressed(Read_setTo_custom1_button.BackColor))
                                    ||(Form_default_settings.Is_button_pressed(Read_setTo_custom2_button.BackColor)))
                                {
                                    switch (override_user_selected_column_names)
                                    {
                                        case Override_used_selected_column_names_enum.No:
                                            int customColumnNames_no = -1;
                                            Dictionary<ReadDataMenu_datasetAttribute_enum, string> datasetAttribute_customColumnName_dict = new Dictionary<ReadDataMenu_datasetAttribute_enum, string>();
                                            if (Form_default_settings.Is_button_pressed(Read_setTo_custom1_button.BackColor)) { customColumnNames_no = 1; }
                                            else if (Form_default_settings.Is_button_pressed(Read_setTo_custom2_button.BackColor)) { customColumnNames_no = 2; }
                                            else { throw new Exception(); }
                                            ReadTextBoxName_columnName_readWriteOptions rc_readWriteOptions = new ReadTextBoxName_columnName_readWriteOptions(gdf.App_generated_datasets_directory, customColumnNames_no);
                                            datasetAttributes = referenceDatasetAttribute_columnName_dict.Keys.ToArray();
                                            ReadDataMenu_datasetAttribute_enum datasetAttribute;
                                            int datasetAttributes_length = datasetAttributes.Length;
                                            DatasetAttribute_columnName_line_class[] readTextBoxName_columnNames = new DatasetAttribute_columnName_line_class[datasetAttributes_length];
                                            for (int indexDA = 0; indexDA < datasetAttributes_length; indexDA++)
                                            {
                                                datasetAttribute = datasetAttributes[indexDA];
                                                readTextBoxName_columnNames[indexDA] = new DatasetAttribute_columnName_line_class();
                                                readTextBoxName_columnNames[indexDA].Dataset_attribute = datasetAttribute;
                                                readTextBoxName_columnNames[indexDA].ColumnName = (string)referenceDatasetAttribute_columnName_dict[datasetAttribute].Clone();
                                            }
                                            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(readTextBoxName_columnNames, rc_readWriteOptions, ProgressReport, out bool file_opened_successful);
                                            Get_lastPressed_back_and_fore_colors_for_default_column_name_button(out Color activated_backColor, out Color activated_foreColor);
                                            add_info_to_report_label = "Custom " + customColumnNames_no + " column names saved for reimport.";
                                            break;
                                        case Override_used_selected_column_names_enum.Example_dataset_no:
                                        case Override_used_selected_column_names_enum.Default_custom_1_names:
                                        case Override_used_selected_column_names_enum.Default_custom_2_names:
                                            //this is selected when load data button is pressed, then the user selected default names will be ignored
                                            break;
                                        default:
                                            throw new Exception();
                                    }
                                }

                                switch (override_user_selected_column_names)
                                {
                                    case Override_used_selected_column_names_enum.Example_dataset_no:
                                    case Override_used_selected_column_names_enum.Default_custom_1_names:
                                    case Override_used_selected_column_names_enum.Default_custom_2_names:
                                        break;
                                    case Override_used_selected_column_names_enum.No:
                                        List<Save_readDataMenuSelections_line_class> save_settings = new List<Save_readDataMenuSelections_line_class>();
                                        Save_readDataMenuSelections_line_class save_settings_line;
                                        save_settings_line = new Save_readDataMenuSelections_line_class();
                                        save_settings_line.Save_selection_type = ReadDataMenu_save_selection_type_enum.Read_data_directory;
                                        save_settings_line.Entry = (string)inputDirectoryFileName_that_will_also_be_saved.Clone();
                                        save_settings.Add(save_settings_line);
                                        save_settings_line = new Save_readDataMenuSelections_line_class();
                                        save_settings_line.Save_selection_type = ReadDataMenu_save_selection_type_enum.Default_column_names;
                                        if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_custom1_button)) { save_settings_line.Entry = ReadDataMenu_default_columnName_enum.Custom_1.ToString(); }
                                        else if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_custom2_button)) { save_settings_line.Entry = ReadDataMenu_default_columnName_enum.Custom_2.ToString(); }
                                        else if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_singleCell_button)) { save_settings_line.Entry = ReadDataMenu_default_columnName_enum.Single_cell.ToString(); }
                                        else if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_MBCO_button)) { save_settings_line.Entry = ReadDataMenu_default_columnName_enum.Mbco.ToString(); }
                                        else if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_optimum_button)) { save_settings_line.Entry = save_settings_line.Entry = ReadDataMenu_default_columnName_enum.Optimum.ToString(); }
                                        else if (Is_readSetTo_defaultColumnNames_activated(Read_setTo_minimum_button)) { save_settings_line.Entry = ReadDataMenu_default_columnName_enum.Minimum.ToString(); }
                                        else { throw new Exception("No default column name button selected."); }
                                        save_settings.Add(save_settings_line);
                                        save_settings_line = new Save_readDataMenuSelections_line_class();
                                        save_settings_line.Save_selection_type = ReadDataMenu_save_selection_type_enum.Read_all_or_one_file;
                                        if (Read_order_allFilesInDirectory_cbButton.Checked) { save_settings_line.Entry = ReadDataMenu_readAllOrOneFile_enum.Read_all_files.ToString(); }
                                        else if (Read_order_onlySpecifiedFile_cbButton.Checked) { save_settings_line.Entry = ReadDataMenu_readAllOrOneFile_enum.Read_one_file.ToString(); }
                                        else { throw new Exception("Neither read all nor read one file is selected."); }
                                        save_settings.Add(save_settings_line);

                                        Save_readDataMenuSelections_readWriteOptions directoryFile_readWriteOptions = new Save_readDataMenuSelections_readWriteOptions();
                                        ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(save_settings.ToArray(), directoryFile_readWriteOptions, ProgressReport, out bool file_opened_successful);
                                        break;
                                    default:
                                        throw new Exception();
                                }

                            }
                        }
                        all_error_messages_list.AddRange(currentFile_error_messages_list);
                    }
                    ProgressReport.Update_progressReport_text_and_visualization("Add missing colors");
                    custom_data.Automatically_override_bgGeneListNames_if_matching_names();
                    custom_data.Set_missing_colors(selectable_colors);
                    ProgressReport.Update_progressReport_text_and_visualization("Set missing result numbers");
                    custom_data.Set_missing_results_numbers_and_adjust_to_consecutive_numbers_within_each_integrationGroup();
                    ProgressReport.Update_progressReport_text_and_visualization("Label significant genes based on specified significance criteria");
                    custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
                }
                this.Last_error_reports = all_error_messages_list.ToArray();

                Update_error_reports_or_explanations(add_info_to_report_label);
                ProgressReport.Clear_progressReport_text_and_last_entry();
            }
            Read_data_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Read_data_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            return custom_data;
        }
        #endregion

    }
}
