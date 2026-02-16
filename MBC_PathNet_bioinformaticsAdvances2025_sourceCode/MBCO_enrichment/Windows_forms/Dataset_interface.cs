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
using System.Drawing;
using Common_functions.Global_definitions;
using Windows_forms_customized_tools;
using Data;
using Common_functions.Array_own;
using Common_functions.Text;
using Common_functions.Form_tools;


namespace ClassLibrary1.Dataset_userInterface
{
    enum Add_fileName_instructions_enum {  E_m_p_t_y, Before_dataset_name, After_dataset_name, Remove }

    class Default_textBox_texts
    {
        public static string Input_dataset_sampleName { get { return "Enter dataset name"; } }
        public static string InputGene_list_textBox_default { get { return "Copy paste gene list (one gene per row).\r\n\r\nEnter name of dataset in small box right to this box.\r\n\r\nPress 'Add dataset'-button below to upload data."; } }
        public static string InputGene_list_textBox_noGenes { get { return "No genes added.\r\n\r\nPlease copy paste gene list (one gene per row) into this box."; } }
        public static string Default_analysis_name { get { return "Analysis 1"; } }
        public static string[] InputGene_list_textBox_texts
        {
            get
            {
                return new string[] { InputGene_list_textBox_default, InputGene_list_textBox_noGenes };
            }
        }
        public static bool Is_among_inputGene_list_textBox_texts(string input_text)
        {
            bool is_among_texts = false;
            foreach (string text in InputGene_list_textBox_texts)
            {
                if (text.IndexOf(input_text)!=-1)
                {
                    is_among_texts = true;
                    break;
                }
            }
            return is_among_texts;
        }
        public static string Triangle_up_in_newCourier { get { return ((char)0x25B2).ToString(); } }
        public static string Triangle_down_in_newCourier { get { return ((char)0x25BC).ToString(); } }
        public static string IntegrationGroup_textBox_allInOne { get { return "All datasets in same group"; } }
        public static string IntegrationGroup_textBox_undefined { get { return "Undefinied group"; } }
        public static string[] IntegrationGroup_default_Groups
        {
            get
            {
                return new string[] { IntegrationGroup_textBox_allInOne, IntegrationGroup_textBox_undefined };
            }
        }

        public static string IntegrationGroup_automatic_baseName {  get { return "Group No "; } }

        public static string Get_integrationGroup_string(string integrationGroup)
        {
            if (integrationGroup.Equals(IntegrationGroup_textBox_allInOne))
            {
                return "";
            }
            else
            {
                return integrationGroup;
            }
        }

        public static Color[] Get_priority_colors()
        {
            Color[] priority_colors = new Color[]
            {
                Color.Orange,
                Color.DodgerBlue,
                Color.Purple,
                Color.OrangeRed,
                Color.Blue,
                Color.Pink,
                Color.Red,
                Color.LightSkyBlue,
                Color.Orchid,
                Color.DarkRed,
                Color.Navy,
                Color.Plum,
                Color.DarkGoldenrod,
                Color.SlateBlue,
                Color.DarkMagenta,
                Color.Yellow,
                Color.SteelBlue,
                Color.DarkOrchid 
            };
            return priority_colors;
        }
    

        public static Color[] All_selectable_colors
        {
            get
            {
                Dictionary<Color, bool> selectable_colors_dict = new Dictionary<Color, bool>();

                Color add_color;
                foreach (System.Reflection.PropertyInfo property in typeof(Color).GetProperties())
                {
                    if (property.PropertyType == typeof(Color))
                    {
                        add_color = (Color)property.GetValue(null);
                        if (  (!selectable_colors_dict.ContainsKey(add_color))
                            &&(!add_color.Equals(Color.White))
                            &&(!add_color.Equals(Color.Transparent)))
                        {
                            selectable_colors_dict.Add(add_color,false);
                        }
                    }
                }
                return selectable_colors_dict.Keys.ToArray();
            }
        }

        public static Color[] Get_priority_and_remaining_colors()
        {
            Color[] priority_colors = Get_priority_colors();
            Dictionary<Color, bool> selectable_colors_dict = new Dictionary<Color, bool>();
            foreach (Color priority_color in priority_colors)
            {
                selectable_colors_dict.Add(priority_color, true);
            }
            Color[] selectable_colors = All_selectable_colors;
            foreach (Color selectable_color in selectable_colors)
            {
                if (!selectable_colors_dict.ContainsKey(selectable_color)) { selectable_colors_dict.Add(selectable_color, true); }
            }
            return selectable_colors_dict.Keys.ToArray();
        }
    }

    class DatasetSummary_line_class
    {
        public int Results_no { get; set; }
        public int Current_temporary_order { get; set; }
        public bool Delete { get; set; }
        public bool Visualize { get; set; }
        public string SampleName { get; set; }
        public string Substring { get; set; }
        public float Timepoint { get; set; }
        public float Timepoint_in_days { get { return Timeunit_conversion_class.Get_timepoint_in_days(Timepoint, Timeunit); } }
        public Color SampleColor { get; set; }
        public Timeunit_enum Timeunit { get; set; }
        public Entry_type_enum EntryType { get; set; }
        public string IntegrationGroup { get; set; }
        public string Source_fileName { get; set; }
        public string Background_geneListName { get; set; }
        public int Datasets_count { get; set; }
        public string Genes_count_string { get; set; }
        public Dataset_compatibility_enum Dataset_compatibility { get; set; }
        public bool Row_contains_invalid_value { get; set; }
        public string Unique_fixed_dataset_identifier { get; set; }

        public DatasetSummary_line_class(DatasetSummary_userInterface_line_class userInterface_line, Form1_default_settings_class form1_default_settings)
        {
            this.SampleName = (string)userInterface_line.Dataset_sampleName_textBox.Text.Clone();
            float new_timepoint;
            Row_contains_invalid_value = false;

            if (  (userInterface_line.Dataset_time_textBox.Text.Length>0)
                &&(float.TryParse(userInterface_line.Dataset_time_textBox.Text, out new_timepoint)))
            { 
                this.Timepoint = new_timepoint;
            }
            else
            {
                Row_contains_invalid_value = true;
            }
            this.EntryType = (Entry_type_enum)Enum.Parse(typeof(Entry_type_enum), userInterface_line.Dataset_entryType_listBox.Text);
            this.SampleColor = Color.FromName(userInterface_line.Dataset_color_listBox.Text);
            this.Timeunit = (Timeunit_enum)Enum.Parse(typeof(Timeunit_enum), userInterface_line.Dataset_timeunit_listBox.Text);
            this.Delete = userInterface_line.Dataset_delete_cbButton.Checked;
            this.IntegrationGroup = (string)userInterface_line.Dataset_integrationGroup_textBox.Text.Clone();
            this.Substring = (string)userInterface_line.Dataset_substring_textBox.Text.Clone();
            this.Source_fileName = (string)userInterface_line.Dataset_sourceFile_textBox.Text.Clone();
            this.Background_geneListName = (string)userInterface_line.Dataset_bgGenes_listBox.Text.Clone();
            this.Datasets_count = (int)userInterface_line.Dataset_counts;
            int new_results_no = -1;
            if (  (userInterface_line.Dataset_orderNo_textBox.Text.Length > 0)
                &&(int.TryParse(userInterface_line.Dataset_orderNo_textBox.Text, out new_results_no)))
            { 
                this.Results_no = new_results_no;
            }
            else
            {
                Row_contains_invalid_value = true;
            }
            this.Visualize = true;
            this.Genes_count_string = (string)userInterface_line.Dataset_genesCount_label.Text.Clone();
            this.Unique_fixed_dataset_identifier = (string)userInterface_line.Unique_fixed_dataset_identifier.Clone();
            this.Current_temporary_order = -1;
        }

        public DatasetSummary_line_class(Custom_data_line_class custom_data_line, int total_genes_count, int sign_genes_count)
        {
            this.Substring = "";
            this.SampleName = (string)custom_data_line.SampleName.Clone();
            this.SampleColor = custom_data_line.SampleColor_for_data;
            this.IntegrationGroup = (string)custom_data_line.IntegrationGroup.Clone();
            this.EntryType = custom_data_line.EntryType;
            this.Timepoint = custom_data_line.Timepoint;
            this.Timeunit = custom_data_line.Timeunit;
            this.Dataset_compatibility = Dataset_compatibility_enum.Ok;
            this.Source_fileName = (string)custom_data_line.Source_fileName.Clone();
            this.Background_geneListName = (string)custom_data_line.BgGenes_listName.Clone();
            this.Row_contains_invalid_value = false;
            this.Datasets_count = 1;
            this.Results_no = custom_data_line.Results_number;
            this.Genes_count_string = sign_genes_count + " sig. genes (" + total_genes_count + " in total)";
            this.Unique_fixed_dataset_identifier = (string)custom_data_line.Unique_fixed_dataset_identifier.Clone();
        }

        public bool Equals_other(DatasetSummary_line_class other_line)
        {
            return ((this.SampleName.Equals(other_line.SampleName))
                    && (this.EntryType.Equals(other_line.EntryType))
                    && (this.SampleColor.Equals(other_line.SampleColor))
                    && (this.Timepoint_in_days.Equals(other_line.Timepoint_in_days))
                    && (this.IntegrationGroup.Equals(other_line.IntegrationGroup))
                    && (this.Source_fileName.Equals(other_line.Source_fileName))
                    && (this.Background_geneListName.Equals(other_line.Background_geneListName))
                    && (this.Results_no.Equals(other_line.Results_no))
                    && (this.Delete.Equals(other_line.Delete)));
        }

        public DatasetSummary_line_class Deep_copy()
        {
            DatasetSummary_line_class copy = (DatasetSummary_line_class)this.MemberwiseClone();
            copy.Substring = (string)this.Substring.Clone();
            copy.SampleName = (string)this.SampleName.Clone();
            copy.IntegrationGroup = (string)this.IntegrationGroup.Clone();
            copy.Background_geneListName = (string)this.Background_geneListName.Clone();
            copy.Source_fileName = (string)this.Source_fileName.Clone();
            copy.Unique_fixed_dataset_identifier = (string)this.Unique_fixed_dataset_identifier.Clone();
            copy.Genes_count_string = (string)this.Genes_count_string.Clone();
            return copy;
        }
    }

    class DatasetSummary_userInterface_line_class
    {
        #region Fields
        private Dictionary<Dataset_attributes_enum, OwnTextBox> DatasetAtrribute_textBox_dict { get; set; }
        private Dictionary<Dataset_attributes_enum, OwnListBox> DatasetAtrribute_listBox_dict { get; set; }
        private Dictionary<Dataset_attributes_enum, MyCheckBox_button> DatasetAtrribute_cbButton_dict { get; set; }
        private Dictionary<Dataset_attributes_enum, Label> DatasetAtrribute_label_dict { get; set; }

        public OwnTextBox Dataset_sampleName_textBox { get; set; }
        public OwnListBox Dataset_color_listBox { get; set; }
        public OwnTextBox Dataset_time_textBox { get; set; }
        public OwnListBox Dataset_timeunit_listBox { get; set; }
        public OwnListBox Dataset_entryType_listBox { get; set; }
        public OwnTextBox Dataset_integrationGroup_textBox { get; set; }
        public OwnTextBox Dataset_substring_textBox { get; set; }
        public OwnTextBox Dataset_sourceFile_textBox { get; set; }
        public OwnListBox Dataset_bgGenes_listBox { get; set; }
        public OwnTextBox Dataset_orderNo_textBox { get; set; }
        public MyCheckBox_button Dataset_delete_cbButton{ get; set; }
        public Label Dataset_datasetsCount_label { get; set; }
        public Label Dataset_genesCount_label { get; set; }
        public int Dataset_counts { get; set; }
        public Dataset_compatibility_enum Dataset_compatibility { get; set; }
        public bool Row_contains_at_least_one_invalid_value { get; set; }
        public string Unique_fixed_dataset_identifier { get; set; }
        #endregion

        public DatasetSummary_userInterface_line_class(Dictionary<Dataset_attributes_enum, MyPanel> datasetAttribute_panel_dict, int indexDataset, Form1_default_settings_class form_default_settings)
        {
            DatasetAtrribute_textBox_dict = new Dictionary<Dataset_attributes_enum, OwnTextBox>();
            Dataset_sampleName_textBox = new OwnTextBox();
            DatasetAtrribute_textBox_dict.Add(Dataset_attributes_enum.Name, Dataset_sampleName_textBox);
            Dataset_substring_textBox = new OwnTextBox();
            DatasetAtrribute_textBox_dict.Add(Dataset_attributes_enum.Substring, Dataset_substring_textBox);
            Dataset_orderNo_textBox = new OwnTextBox();
            DatasetAtrribute_textBox_dict.Add(Dataset_attributes_enum.Dataset_order_no, Dataset_orderNo_textBox);
            Dataset_time_textBox = new OwnTextBox();
            DatasetAtrribute_textBox_dict.Add(Dataset_attributes_enum.Timepoint, Dataset_time_textBox);
            Dataset_integrationGroup_textBox = new OwnTextBox();
            DatasetAtrribute_textBox_dict.Add(Dataset_attributes_enum.IntegrationGroup, Dataset_integrationGroup_textBox);
            Dataset_sourceFile_textBox = new OwnTextBox();
            DatasetAtrribute_textBox_dict.Add(Dataset_attributes_enum.SourceFile, Dataset_sourceFile_textBox);

            DatasetAtrribute_listBox_dict = new Dictionary<Dataset_attributes_enum, OwnListBox>();
            Dataset_color_listBox = new OwnListBox();
            DatasetAtrribute_listBox_dict.Add(Dataset_attributes_enum.Color, Dataset_color_listBox);
            Dataset_entryType_listBox = new OwnListBox();
            DatasetAtrribute_listBox_dict.Add(Dataset_attributes_enum.EntryType, Dataset_entryType_listBox);
            Dataset_timeunit_listBox = new OwnListBox();
            DatasetAtrribute_listBox_dict.Add(Dataset_attributes_enum.Timepoint, Dataset_timeunit_listBox);
            Dataset_bgGenes_listBox = new OwnListBox();
            DatasetAtrribute_listBox_dict.Add(Dataset_attributes_enum.BgGenes, Dataset_bgGenes_listBox);
            Fill_listBoxes_with_items();

            DatasetAtrribute_cbButton_dict = new Dictionary<Dataset_attributes_enum, MyCheckBox_button>();
            Dataset_delete_cbButton = new MyCheckBox_button();
            DatasetAtrribute_cbButton_dict.Add(Dataset_attributes_enum.Delete, Dataset_delete_cbButton);

            DatasetAtrribute_label_dict = new Dictionary<Dataset_attributes_enum, Label>();
            Dataset_datasetsCount_label = new Label();
            DatasetAtrribute_label_dict.Add(Dataset_attributes_enum.Datasets_count, Dataset_datasetsCount_label);
            Dataset_genesCount_label = new Label();
            DatasetAtrribute_label_dict.Add(Dataset_attributes_enum.Genes_count, Dataset_genesCount_label);

            Dictionary<Dataset_attributes_enum, string> datasetAttribute_baseName_dict = new Dictionary<Dataset_attributes_enum, string>();
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.BgGenes, "bgGenes");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Color, "color");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Datasets_count, "datasetsCount");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Dataset_order_no, "orderNo");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Delete, "delete");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.EntryType, "entryType");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Genes_count, "genesCount");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.IntegrationGroup, "integrationGroup");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Name, "name");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.SourceFile, "source");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Substring, "substring");
            datasetAttribute_baseName_dict.Add(Dataset_attributes_enum.Timepoint, "timeline");

            Dataset_attributes_enum[] attributes = datasetAttribute_panel_dict.Keys.ToArray();
            Dataset_attributes_enum attribute;
            int attributes_length = attributes.Length;
            MyPanel current_panel;
            OwnTextBox current_textBox;
            OwnListBox current_listBox;
            MyCheckBox_button current_cbButton;
            Label current_label;
            string current_label_string;
            for (int indexAtrr = 0; indexAtrr < attributes_length; indexAtrr++)
            {
                attribute = attributes[indexAtrr];
                current_panel = datasetAttribute_panel_dict[attribute];
                current_label_string = datasetAttribute_baseName_dict[attribute].ToString();
                if (DatasetAtrribute_textBox_dict.ContainsKey(attribute))
                {
                    current_textBox = DatasetAtrribute_textBox_dict[attribute];
                    current_panel.Controls.Add(current_textBox);
                    current_panel.Name = "Dataset_" + indexAtrr.ToString() + "_" + current_label_string + "_ownTextBox";
                }
                if (DatasetAtrribute_listBox_dict.ContainsKey(attribute))
                {
                    current_listBox = DatasetAtrribute_listBox_dict[attribute];
                    current_panel.Controls.Add(current_listBox);
                    current_panel.Name = "Dataset_" + indexAtrr.ToString() + "_" + current_label_string + "_ownListBox";
                }
                if (DatasetAtrribute_cbButton_dict.ContainsKey(attribute))
                {
                    current_cbButton = DatasetAtrribute_cbButton_dict[attribute];
                    current_panel.Controls.Add(current_cbButton);
                    current_panel.Name = "Dataset_" + indexAtrr.ToString() + "_" + current_label_string + "_ownCheckBox";
                }
                if (DatasetAtrribute_label_dict.ContainsKey(attribute))
                {
                    current_label = DatasetAtrribute_label_dict[attribute];
                    current_panel.Controls.Add(current_label);
                    current_panel.Name = "Dataset_" + indexAtrr.ToString() + "_" + current_label_string + "_label";
                }
            }
        }

        public void Update_graphic_elements(Dictionary<Dataset_attributes_enum, MyPanel> datasetAttribute_panel_dict, int indexDataset, Form1_default_settings_class form_default_settings, ref int size_of_deleteButton)
        { 
            int height_for_headlines = (int)Math.Round(0.05 * form_default_settings.DatasetInterface_variable_panel_height);
            int shared_height_of_all_panels = form_default_settings.DatasetInterface_variable_panel_height - height_for_headlines;
            int number_of_userInterface_lines = form_default_settings.DatasetInterface_max_number_of_shown_entries;
            float size_of_each_userInterface_line = (float)shared_height_of_all_panels / (float)(number_of_userInterface_lines + 1);
            
            int overall_top_referenceBorder = height_for_headlines + (int)Math.Round((indexDataset) * size_of_each_userInterface_line);
            int overall_bottom_referenceBorder = height_for_headlines + (int)Math.Round((indexDataset + 1) * size_of_each_userInterface_line);
            int left_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            int right_referenceBorder;

            Dataset_attributes_enum[] attributes = datasetAttribute_panel_dict.Keys.ToArray();
            Dataset_attributes_enum attribute;
            int attributes_length = attributes.Length;
            MyPanel current_panel;
            OwnTextBox current_textBox;
            OwnListBox current_listBox;
            MyCheckBox_button current_cbButton;
            Label current_label;
            string current_label_text;
            int half_cbButton_unused_height;
            int half_cbButton_unused_width;
            int cbButton_left_referenceBorder;
            int cbButton_right_referenceBorder;
            int cbButton_top_referenceBorder;
            int cbButton_bottom_referenceBorder;
            for (int indexAtrr=0; indexAtrr<attributes_length;indexAtrr++)
            {
                top_referenceBorder = overall_top_referenceBorder;
                bottom_referenceBorder = overall_bottom_referenceBorder;
                attribute = attributes[indexAtrr];
                current_panel = datasetAttribute_panel_dict[attribute];
                left_referenceBorder = 0;
                if (DatasetAtrribute_textBox_dict.ContainsKey(attribute))
                {
                    current_textBox = DatasetAtrribute_textBox_dict[attribute];
                    if (DatasetAtrribute_listBox_dict.ContainsKey(attribute))
                    { right_referenceBorder = (int)Math.Round(0.32 * current_panel.Width); }
                    else { right_referenceBorder = current_panel.Width; }
                    form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(current_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                    left_referenceBorder = current_textBox.Location.X + current_textBox.Width;
                }
                if (DatasetAtrribute_listBox_dict.ContainsKey(attribute))
                {
                    current_listBox = DatasetAtrribute_listBox_dict[attribute];
                    right_referenceBorder = current_panel.Width;
                    form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(current_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                    left_referenceBorder = current_listBox.Location.X + current_listBox.Width;
                }
                //top_referenceBorder = Dataset_sampleName_textBox.Location.Y;
                //bottom_referenceBorder = Dataset_sampleName_textBox.Location.Y + Dataset_sampleName_textBox.Height;
                if (DatasetAtrribute_cbButton_dict.ContainsKey(attribute))
                {
                    current_cbButton = DatasetAtrribute_cbButton_dict[attribute];
                    right_referenceBorder = current_panel.Width;

                    if (size_of_deleteButton==-1)
                    {
                        size_of_deleteButton = (int)Math.Round(0.9F*(int)Math.Min(right_referenceBorder - left_referenceBorder, bottom_referenceBorder - top_referenceBorder));
                    }
                    half_cbButton_unused_height = (int)Math.Round(0.5F * (bottom_referenceBorder - top_referenceBorder - size_of_deleteButton));
                    half_cbButton_unused_width = (int)Math.Round(0.5F * (right_referenceBorder - left_referenceBorder - size_of_deleteButton));

                    cbButton_left_referenceBorder = left_referenceBorder + half_cbButton_unused_width;
                    cbButton_right_referenceBorder = right_referenceBorder - half_cbButton_unused_width;
                    cbButton_top_referenceBorder = top_referenceBorder + half_cbButton_unused_height;
                    cbButton_bottom_referenceBorder = bottom_referenceBorder - half_cbButton_unused_height;
                    form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(current_cbButton, cbButton_left_referenceBorder, cbButton_right_referenceBorder, cbButton_top_referenceBorder, cbButton_bottom_referenceBorder);
                    //form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(current_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                    left_referenceBorder = current_cbButton.Location.X + current_cbButton.Width;
                }
                if (DatasetAtrribute_label_dict.ContainsKey(attribute))
                {
                    current_label = DatasetAtrribute_label_dict[attribute];
                    right_referenceBorder = current_panel.Width;
                    current_label_text = (string)current_label.Text.Clone();
                    current_label.Text = "123456789012345678901";
                    form_default_settings.LabelDefaultRegular_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(current_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                    current_label.Text = (string)current_label_text.Clone();
                    left_referenceBorder = current_label.Location.X + current_label.Width;
                }
            }
            Unique_fixed_dataset_identifier = "";
        }

        private void Current_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Fill_listBoxes_with_items()
        {
            Dataset_color_listBox.Items.Clear();
            Color[] priority_colors = Default_textBox_texts.Get_priority_colors();
            Color[] colors = Default_textBox_texts.All_selectable_colors;
            Color add_color;
            int priority_colors_length = priority_colors.Length;
            for (int indexPriorityColor = 0; indexPriorityColor < priority_colors_length; indexPriorityColor++)
            {
                add_color = priority_colors[indexPriorityColor];
                if (!add_color.Equals(Global_class.Intermediate_network_scps_color))
                {
                    Dataset_color_listBox.Items.Add(Color_conversion_class.Get_color_string(add_color));
                }
            }
            int colors_length = colors.Length;
            for (int indexColor = 0; indexColor < colors_length; indexColor++)
            {
                add_color = colors[indexColor];
                if (!add_color.Equals(Global_class.Intermediate_network_scps_color))
                {
                    Dataset_color_listBox.Items.Add(Color_conversion_class.Get_color_string(add_color));
                }
            }
            Dataset_color_listBox.SilentSelectedIndex_and_topIndex = 0;
            Dataset_color_listBox.SelectionMode = SelectionMode.One;
            Dataset_timeunit_listBox.Items.Clear();
            Dataset_timeunit_listBox.SelectionMode = SelectionMode.One;
            var all_timeunits = Enum.GetValues(typeof(Timeunit_enum));
            foreach (var timeunit in all_timeunits)
            {
                if (!timeunit.Equals(Timeunit_enum.E_m_p_t_y))
                {
                    Dataset_timeunit_listBox.Items.Add(timeunit);
                }
            }
            Dataset_timeunit_listBox.SilentSelectedIndex_and_topIndex = 1;
            Dataset_timeunit_listBox.SelectionMode = SelectionMode.One;
            Dataset_entryType_listBox.Items.Clear();
            Dataset_entryType_listBox.Items.Add(Entry_type_enum.Up);
            Dataset_entryType_listBox.Items.Add(Entry_type_enum.Down);
            Dataset_entryType_listBox.SelectionMode= SelectionMode.One;
            Dataset_bgGenes_listBox.SelectionMode = SelectionMode.One;
        }

        public int Get_location_y_of_tool_linked_indicated_attribute(Dataset_attributes_enum attribute)
        {
            int location_y = -1;
            if (DatasetAtrribute_textBox_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_textBox_dict[attribute].Location.Y; }
            else if (DatasetAtrribute_listBox_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_listBox_dict[attribute].Location.Y; }
            else if (DatasetAtrribute_cbButton_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_cbButton_dict[attribute].Location.Y; }
            else if (DatasetAtrribute_label_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_label_dict[attribute].Location.Y; }
            return location_y;
        }

        public int Get_location_y_plus_height_of_tool_linked_indicated_attribute(Dataset_attributes_enum attribute)
        {
            int location_y = -1;
            if (DatasetAtrribute_textBox_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_textBox_dict[attribute].Location.Y + DatasetAtrribute_textBox_dict[attribute].Height; }
            else if (DatasetAtrribute_listBox_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_listBox_dict[attribute].Location.Y + DatasetAtrribute_listBox_dict[attribute].Height; }
            else if (DatasetAtrribute_cbButton_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_cbButton_dict[attribute].Location.Y + DatasetAtrribute_cbButton_dict[attribute].Height; }
            else if (DatasetAtrribute_label_dict.ContainsKey(attribute)) { location_y = DatasetAtrribute_label_dict[attribute].Location.Y + DatasetAtrribute_label_dict[attribute].Height; }
            return location_y;
        }

        public void Set_dataset_from_datasetSummary_line(DatasetSummary_line_class datasetSummary_line, string[] available_bgGenesLists)
        {
            Unique_fixed_dataset_identifier = (string)datasetSummary_line.Unique_fixed_dataset_identifier.Clone();
            Dataset_sampleName_textBox.SilentText = (string)datasetSummary_line.SampleName.Clone();
            Dataset_time_textBox.SilentText = datasetSummary_line.Timepoint.ToString();
            Dataset_timeunit_listBox.SilentSelectedIndex_and_topIndex = Dataset_timeunit_listBox.Items.IndexOf(datasetSummary_line.Timeunit);
            Dataset_entryType_listBox.SilentSelectedIndex_and_topIndex = Dataset_entryType_listBox.Items.IndexOf(datasetSummary_line.EntryType);
            Dataset_color_listBox.SilentSelectedIndex_and_topIndex = Dataset_color_listBox.Items.IndexOf(Color_conversion_class.Get_color_string(datasetSummary_line.SampleColor));
            Dataset_integrationGroup_textBox.SilentText = (string)datasetSummary_line.IntegrationGroup.Clone();
            Dataset_delete_cbButton.SilentChecked = datasetSummary_line.Delete;
            Dataset_substring_textBox.SilentText = (string)datasetSummary_line.Substring.Clone();
            Dataset_sourceFile_textBox.SilentText = (string)datasetSummary_line.Source_fileName.Clone();
            Dataset_orderNo_textBox.SilentText = datasetSummary_line.Results_no.ToString();
            Dataset_counts = datasetSummary_line.Datasets_count;
            Dataset_datasetsCount_label.Text = Dataset_counts + " datasets";
            Dataset_bgGenes_listBox.Items.Clear();
            Dataset_genesCount_label.Text = (string)datasetSummary_line.Genes_count_string.Clone();
            foreach (string available_bgGeneListName in available_bgGenesLists)
            {
                Dataset_bgGenes_listBox.Items.Add(available_bgGeneListName);
            }
            Dataset_bgGenes_listBox.SilentSelectedIndex = Dataset_bgGenes_listBox.Items.IndexOf(datasetSummary_line.Background_geneListName);
            Dataset_substring_textBox.SilentText = (string)datasetSummary_line.Substring.Clone();
            if (Dataset_timeunit_listBox.SelectedIndex == -1) { throw new Exception(); }
            if (Dataset_entryType_listBox.SelectedIndex == -1) { throw new Exception(); }
            if (Dataset_color_listBox.SelectedIndex == -1) { throw new Exception(); }
            Dataset_delete_cbButton.Checked = datasetSummary_line.Delete;
            Dataset_compatibility = datasetSummary_line.Dataset_compatibility;
            Row_contains_at_least_one_invalid_value = datasetSummary_line.Row_contains_invalid_value;
            //Dataset_sourceFile_textBox.ReadOnly = true;
            Refresh();
        }

        private void Refresh()
        {
            Dataset_sampleName_textBox.Refresh();
            Dataset_substring_textBox.Refresh();
            Dataset_entryType_listBox.Refresh();
            Dataset_timeunit_listBox.Refresh();
            Dataset_color_listBox.Refresh();
            Dataset_sourceFile_textBox.Refresh();
            Dataset_bgGenes_listBox.Refresh();
            Dataset_datasetsCount_label.Refresh();
            Dataset_genesCount_label.Refresh();
        }

        public void Update_userInterface_bgGenes_listBox(string[] newBgGenesListNames, string new_default)
        {
            string current_bgGenesListName = (string)Dataset_bgGenes_listBox.Text.Clone();
            Dataset_bgGenes_listBox.Items.Clear();
            Dataset_bgGenes_listBox.Items.AddRange(newBgGenesListNames);
            int indexOfOldSelection = Dataset_bgGenes_listBox.Items.IndexOf(current_bgGenesListName);
            if (indexOfOldSelection!=-1)
            {
                Dataset_bgGenes_listBox.SilentSelectedIndex = indexOfOldSelection;
            }
            else
            {
                Dataset_bgGenes_listBox.SilentSelectedIndex = Dataset_bgGenes_listBox.Items.IndexOf(new_default);
            }
        }

        public void Update_userInterface_backcolor_and_refresh(Dataset_compatibility_enum described_incompatibility, Form1_default_settings_class form_default_settings)
        {
            if (Dataset_delete_cbButton.Checked) { Set_backcolor_to_flagged_as_marked_for_deletion(form_default_settings); }
            else if (Dataset_compatibility.Equals(Dataset_compatibility_enum.Ok)) { Set_backcolor_to_normal(form_default_settings); }
            else if (Dataset_compatibility.Equals(described_incompatibility))
            {
                Set_backcolor_to_flagged_as_incompatible(form_default_settings);
            }
            else if (!Dataset_compatibility.Equals(Dataset_compatibility_enum.Ok))
            {
                Set_backcolor_to_normal(form_default_settings);
            }
            else { Set_backcolor_to_normal(form_default_settings); }
            Refresh();
        }

        public void Reset_to_empty_invisible_and_refresh(Color default_color, Form1_default_settings_class form_default_settings)
        {
            Dataset_sampleName_textBox.SilentText = Default_textBox_texts.Input_dataset_sampleName;
            Dataset_color_listBox.SilentSelectedIndex_and_topIndex = Dataset_color_listBox.Items.IndexOf(Color_conversion_class.Get_color_string(default_color));
            Dataset_time_textBox.SilentText = "0";
            Dataset_orderNo_textBox.SilentText = "0";
            Dataset_entryType_listBox.SilentSelectedIndex_and_topIndex = 0;
            Dataset_integrationGroup_textBox.SilentText = Default_textBox_texts.IntegrationGroup_textBox_allInOne;
            Dataset_datasetsCount_label.Text = "";
            Dataset_genesCount_label.Text = "";
            //Set_visible_parameter_for_all_entities_excluding_delete_button(false);
            Set_backcolor_to_normal(form_default_settings);
            Refresh();
        }

        public void Set_readOnly_property_for_dataset_defining_attributes_and_delete_checkBox(bool readOnly)
        {
            Dataset_sampleName_textBox.ReadOnly = readOnly;
            Dataset_time_textBox.ReadOnly = readOnly;
            Dataset_timeunit_listBox.ReadOnly = readOnly;
            Dataset_entryType_listBox.ReadOnly = readOnly;
            Dataset_sourceFile_textBox.ReadOnly = readOnly;
            Dataset_substring_textBox.ReadOnly = readOnly;
        }

        private void Set_fore_and_backColors_and_label_invalid_values(Color new_foreColor, Color new_backColor, Form1_default_settings_class form_default_settings)
        {
            Dataset_sampleName_textBox.ForeColor = new_foreColor;
            Dataset_sampleName_textBox.BackColor = new_backColor;
            Dataset_sampleName_textBox.Refresh();

            float new_timepoint;
            if (float.TryParse(Dataset_time_textBox.Text, out new_timepoint))
            {
                Dataset_time_textBox.ForeColor = new_foreColor;
                Dataset_time_textBox.BackColor = new_backColor;
            }
            else
            {
                Dataset_time_textBox.ForeColor = form_default_settings.Color_textBox_foreColor;
                Dataset_time_textBox.BackColor = form_default_settings.Color_textBox_backColor_invalid_value;
            }
            Dataset_timeunit_listBox.ForeColor = new_foreColor;
            Dataset_timeunit_listBox.BackColor = new_backColor;
            Dataset_timeunit_listBox.Refresh();
            Dataset_entryType_listBox.ForeColor = new_foreColor;
            Dataset_entryType_listBox.BackColor = new_backColor;
            Dataset_entryType_listBox.Refresh();
            Dataset_color_listBox.ForeColor = new_foreColor;
            Dataset_color_listBox.BackColor = new_backColor;
            Dataset_color_listBox.Refresh();
            Dataset_integrationGroup_textBox.ForeColor = new_foreColor;
            Dataset_integrationGroup_textBox.BackColor = new_backColor;
            Dataset_integrationGroup_textBox.Refresh();
            Dataset_substring_textBox.ForeColor = new_foreColor;
            Dataset_substring_textBox.BackColor = new_backColor;
            //Dataset_delete_checkBox.ForeColor = new_foreColor;
            //Dataset_delete_checkBox.BackColor = new_backColor;
            Dataset_sourceFile_textBox.ForeColor = new_foreColor;
            Dataset_sourceFile_textBox.BackColor = new_backColor;
            Dataset_bgGenes_listBox.ForeColor = new_foreColor;
            Dataset_bgGenes_listBox.BackColor = new_backColor;
            int new_results_no;
            if (int.TryParse(Dataset_orderNo_textBox.Text, out new_results_no))
            {
                Dataset_orderNo_textBox.ForeColor = new_foreColor;
                Dataset_orderNo_textBox.BackColor = new_backColor;
            }
            else
            {
                Dataset_orderNo_textBox.ForeColor = new_foreColor;
                Dataset_orderNo_textBox.BackColor = form_default_settings.Color_textBox_backColor_invalid_value;
            }
            Dataset_delete_cbButton.Refresh();
        }

        public void Set_backcolor_to_flagged_as_incompatible(Form1_default_settings_class Form_default_settings)
        {
            Set_fore_and_backColors_and_label_invalid_values(Form_default_settings.Incompatible_dataset_foreColor, Form_default_settings.Incompatible_dataset_backColor, Form_default_settings);
        }

        public void Set_backcolor_to_flagged_as_marked_for_deletion(Form1_default_settings_class Form_default_settings)
        {
            Set_fore_and_backColors_and_label_invalid_values(Form_default_settings.Color_dataset_foreColor_markedForDeletion, Form_default_settings.Color_dataset_backColor_markedForDeletion, Form_default_settings);
        }

        public void Set_backcolor_to_normal(Form1_default_settings_class Form_default_settings)
        {
            Set_fore_and_backColors_and_label_invalid_values(Form_default_settings.Color_dataset_foreColor, Form_default_settings.Color_dataset_backColor, Form_default_settings);
        }
    }

    class DatasetSummary_userInterface_class
    {
        public DatasetSummary_userInterface_line_class[] UserInterface_lines { get; set; }
        private DatasetSummary_line_class[] UserInterface_dataSummaries { get; set; }
        private DatasetSummary_line_class[] Hidden_userInterface_dataSummaries { get; set; }
        private DatasetSummary_line_class[] LastSaved_datasetSummaries { get; set; }
        private bool LastSavedDataSummaries_identical_with_userInterface { get; set; }
        private bool Updates_are_allowed_changes_button_allowed_to_be_visible { get; set; }
        private Label Input_geneList_label { get; set; }
        private OwnTextBox Input_geneList_textBox { get; set; }
        private MyPanel_label Dataset_compatibility_myPanelLabel { get; set; }
        private ScrollBar Dataset_scrollBar { get; set; }
        private Button AddNewDataset_button { get; set; }
        private Button AnalyzeDataset_button { get; set; }
        private Button ClearDataset_button { get; set; }
        private Button ClearReadAnalyze_button { get; set; }
        private Button Changes_reset_button { get; set; }
        private Button Changes_update_button { get; set; }
        private MyPanel Overall_interface_panel { get; set; }
        private Dictionary<Dataset_attributes_enum, MyPanel> DatasetAttributes_panel_dict { get; set; }
        private Dictionary<Dataset_attributes_enum, Label> DatasetAttributes_label_dict { get; set; }
        public Dictionary<Dataset_attributes_enum, Button> DatasetAttributes_sortByButton_dict { get; set; }
        private Dictionary<Dataset_attributes_enum, MyCheckBox_button> DatasetAttributes_cbButton_dict { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }

        private string[] Available_background_gene_list_names { get; set; }
        private int First_shown_userInterface_dataSummaries_index { get; set; }
        private int UserInterfaceLines_available_for_userInterfaceDataSummaries { get { return UserInterface_lines.Length - 1; } }
        private int Last_filled_userInterfaceLines_index
        {
            get
            {
                return Math.Min(UserInterfaceLines_available_for_userInterfaceDataSummaries - 1,
                                Math.Max(-1, UserInterface_dataSummaries.Length - First_shown_userInterface_dataSummaries_index - 1));
            }
        }
        private int Next_userInterfaceLine_index 
        { 
            get 
            {
                return Last_filled_userInterfaceLines_index + 1;
            } 
        }
        public Dataset_attributes_enum[] dataset_attributes_with_visible_panels = new Dataset_attributes_enum[0];
        public Dataset_attributes_enum[] Dataset_attributes_with_visible_panels 
        { 
            get
            {
                if (Is_filter_mode) { return Dataset_attributes_defining_filter_mode; }
                else { return dataset_attributes_with_visible_panels; }
            }
            set
            {
                if (Is_filter_mode) { throw new Exception(); }
                else { dataset_attributes_with_visible_panels = value; }
            }
        }
        public bool Overall_visibility { get; set; }
        public Dataset_attributes_enum[] Dataset_attributes_defining_filter_mode { get; private set; }
        public Dataset_attributes_enum[] Dataset_attributes_only_allowed_in_filter_mode
        {
            get { return new Dataset_attributes_enum[] { Dataset_attributes_enum.Substring, Dataset_attributes_enum.Datasets_count }; }
        }
        public bool Is_filter_mode { get { return Dataset_attributes_defining_filter_mode.Length > 0; } }

        public DatasetSummary_userInterface_class(MyPanel overall_interface_panel,
                                                  Label input_geneList_label,
                                                  OwnTextBox input_geneList_textBox,
                                                  ScrollBar dataset_scrollBar,
                                                  Button addNewDataset_button,
                                                  Button analyzeDataset_button,
                                                  Button clearDataset_button,
                                                  Button clearReadAnalyze_button,
                                                  Button changes_update_button,
                                                  Button changes_reset_button,
                                                  MyPanel delete_panel,
                                                  MyCheckBox_button dataset_all_delete_cbButton,
                                                  MyPanel name_panel,
                                                  Label name_label,
                                                  Button name_sortBy_button,
                                                  MyPanel timeline_panel,
                                                  Label timeline_label,
                                                  Button timeline_sortBy_button,
                                                  MyPanel entryType_panel,
                                                  Label entryType_label,
                                                  Button entryType_sortBy_button,
                                                  MyPanel integrationGroup_panel,
                                                  Label integrationGroup_label,
                                                  Button integrationGroup_sortBy_button,
                                                  MyPanel color_panel,
                                                  Label color_label,
                                                  Button color_sortBy_button,
                                                  MyPanel substring_panel,
                                                  Label substring_label,
                                                  Button substring_sortBy_button,
                                                  MyPanel source_panel,
                                                  Label source_label,
                                                  Button source_sortBy_button,
                                                  MyPanel bgGenes_panel,
                                                  Label bgGenes_label,
                                                  Button bgGenes_sortBy_button,
                                                  MyPanel datasetOrderNo_panel,
                                                  Label datasetOrderNo_label,
                                                  Button datasetOrderNo_sortBy_button,
                                                  MyPanel datasetsCount_panel,
                                                  MyPanel genesCount_panel,
                                                  MyPanel_label dataset_compatibility_myPanelLabel,
                                                  Custom_data_class custom_data,
                                                  Form1_default_settings_class form_default_settings)
        {
            this.Form_default_settings = form_default_settings;
            this.Overall_interface_panel = overall_interface_panel;
            this.Input_geneList_label = input_geneList_label;
            this.Input_geneList_textBox = input_geneList_textBox;
            this.Dataset_scrollBar = dataset_scrollBar;
            this.AddNewDataset_button = addNewDataset_button;
            this.AnalyzeDataset_button = analyzeDataset_button;
            this.ClearDataset_button = clearDataset_button;
            this.ClearReadAnalyze_button = clearReadAnalyze_button;
            this.Changes_update_button = changes_update_button;
            this.Changes_reset_button = changes_reset_button;

            DatasetAttributes_sortByButton_dict = new Dictionary<Dataset_attributes_enum, Button>();
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.Name, name_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.Timepoint, timeline_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.EntryType, entryType_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.IntegrationGroup, integrationGroup_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.Color, color_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.Substring, substring_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.BgGenes, bgGenes_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.SourceFile, source_sortBy_button);
            DatasetAttributes_sortByButton_dict.Add(Dataset_attributes_enum.Dataset_order_no, datasetOrderNo_sortBy_button);

            DatasetAttributes_label_dict = new Dictionary<Dataset_attributes_enum, Label>();
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.Name, name_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.Timepoint, timeline_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.EntryType, entryType_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.IntegrationGroup, integrationGroup_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.Color, color_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.Substring, substring_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.BgGenes, bgGenes_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.SourceFile, source_label);
            DatasetAttributes_label_dict.Add(Dataset_attributes_enum.Dataset_order_no, datasetOrderNo_label);

            this.Overall_visibility = true;
            Overall_interface_panel.Visible = true;
            DatasetAttributes_panel_dict = new Dictionary<Dataset_attributes_enum, MyPanel>();
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Delete, delete_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Name, name_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Substring, substring_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.EntryType, entryType_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Timepoint, timeline_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.IntegrationGroup, integrationGroup_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Color, color_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.SourceFile, source_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.BgGenes, bgGenes_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Dataset_order_no, datasetOrderNo_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Datasets_count, datasetsCount_panel);
            DatasetAttributes_panel_dict.Add(Dataset_attributes_enum.Genes_count, genesCount_panel);

            DatasetAttributes_cbButton_dict = new Dictionary<Dataset_attributes_enum, MyCheckBox_button>();
            dataset_all_delete_cbButton.SilentChecked = false;
            DatasetAttributes_cbButton_dict.Add(Dataset_attributes_enum.Delete, dataset_all_delete_cbButton);

            this.Dataset_compatibility_myPanelLabel = dataset_compatibility_myPanelLabel;

            int userInterface_lines_length = Form_default_settings.DatasetInterface_max_number_of_shown_entries;
            this.UserInterface_lines = new DatasetSummary_userInterface_line_class[userInterface_lines_length];
            for (int indexUI=0; indexUI<userInterface_lines_length; indexUI++)
            {
                this.UserInterface_lines[indexUI] = new DatasetSummary_userInterface_line_class(DatasetAttributes_panel_dict, indexUI, Form_default_settings);
            }
            Update_all_graphic_elements(custom_data);
        }

        public void Update_all_graphic_elements(Custom_data_class custom_data)
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            Label my_label;
            OwnTextBox my_textBox;
            Button my_button;
            ScrollBar my_scrollBar;

            Overall_interface_panel = Form_default_settings.MyPanelOverallDatsetInterface_add_default_parameters(Overall_interface_panel);


            left_referenceBorder = (int)Math.Round(0.01 * this.Overall_interface_panel.Width);
            right_referenceBorder = (int)Math.Round(0.2 * this.Overall_interface_panel.Width);
            top_referenceBorder = (int)Math.Round(0.05 * this.Overall_interface_panel.Height); ;
            bottom_referenceBorder = this.Overall_interface_panel.Height;
            my_textBox = this.Input_geneList_textBox;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Input_geneList_textBox.Location.X;
            right_referenceBorder = this.Input_geneList_textBox.Location.X + this.Input_geneList_textBox.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Input_geneList_textBox.Location.Y;
            my_label = this.Input_geneList_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                
            //Dataset_compatibility_myPanelLabel.Visible = false;
            UserInterface_dataSummaries = new DatasetSummary_line_class[0];
            LastSaved_datasetSummaries = new DatasetSummary_line_class[0];



            int height_of_compatibility_label = (int)Math.Round(0.1F * this.Overall_interface_panel.Height);

            int height_of_buttons = (int)Math.Round(0.10F * this.Overall_interface_panel.Height);
            int width_of_buttons = (int)Math.Round(0.13F * this.Overall_interface_panel.Width);
            int interbutton_width_mono = (int)Math.Round(0.01F * this.Overall_interface_panel.Width);
            right_referenceBorder = this.Input_geneList_textBox.Location.X + this.Input_geneList_textBox.Width + (int)Math.Round(0.01F * this.Overall_interface_panel.Width);
            bottom_referenceBorder = this.Overall_interface_panel.Height;
            top_referenceBorder = bottom_referenceBorder - height_of_buttons;
            if (Form_default_settings.Is_mono)
            {
                int move_height = (int)Math.Round(0.022*this.Overall_interface_panel.Height);
                bottom_referenceBorder -= move_height;
                top_referenceBorder -= move_height;
            }
            Button[] buttons = new Button[] { this.AddNewDataset_button, this.AnalyzeDataset_button, this.ClearDataset_button, this.ClearReadAnalyze_button };// 
            foreach (Button button in buttons)
            {
                left_referenceBorder = right_referenceBorder;
                right_referenceBorder = left_referenceBorder + width_of_buttons;
                my_button = button;
                Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
            
            Dataset_attributes_enum[] attributes;


            Dataset_compatibility_myPanelLabel.Location = new Point(0, Changes_update_button.Location.Y - height_of_compatibility_label);
            Dataset_compatibility_myPanelLabel.Height = height_of_compatibility_label;
            //Location X and Width will be updated in 'Initialize_and_set_default_buttons_boxes_and_locations()'


            Dictionary<Dataset_attributes_enum, float> attribute_panel_fractionWidth_dict = new Dictionary<Dataset_attributes_enum, float>();
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Delete, 0.03F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Name, 0.20F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Substring, 0.2F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.EntryType, 0.1F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Timepoint, 0.12F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.IntegrationGroup, 0.17F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Color, 0.15F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.SourceFile, 0.2F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Datasets_count, 0.3F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Genes_count, 0.3F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.BgGenes, 0.3F);
            attribute_panel_fractionWidth_dict.Add(Dataset_attributes_enum.Dataset_order_no, 0.05F);



            attributes = DatasetAttributes_panel_dict.Keys.ToArray();
            MyPanel current_panel;
            int width;
            foreach (Dataset_attributes_enum attribute in attributes)
            {
                current_panel = DatasetAttributes_panel_dict[attribute];
                top_referenceBorder = 0;
                bottom_referenceBorder = Dataset_compatibility_myPanelLabel.Location.Y;
                width = (int)Math.Round(attribute_panel_fractionWidth_dict[attribute] * this.Overall_interface_panel.Width);
                Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters_considering_only_width_and_top_bottom_reference_borders(current_panel,top_referenceBorder,bottom_referenceBorder, width);
            }

            int userInterface_lines_length = Form_default_settings.DatasetInterface_max_number_of_shown_entries;
            int size_of_deleteButton = -1;
            for (int indexDI = 0; indexDI < userInterface_lines_length; indexDI++)
            {
                this.UserInterface_lines[indexDI].Update_graphic_elements(DatasetAttributes_panel_dict, indexDI, Form_default_settings, ref size_of_deleteButton);
            }
            this.Hidden_userInterface_dataSummaries = new DatasetSummary_line_class[0];
            this.Dataset_attributes_defining_filter_mode = new Dataset_attributes_enum[0];

            attributes = DatasetAttributes_sortByButton_dict.Keys.ToArray();
            Button current_button;
            Label current_label;
            int shared_sortByButton_width = (int)Math.Round(0.2F * DatasetAttributes_panel_dict[Dataset_attributes_enum.Timepoint].Width);
            foreach (Dataset_attributes_enum attribute in attributes)
            {
                current_button = DatasetAttributes_sortByButton_dict[attribute];
                current_panel = DatasetAttributes_panel_dict[attribute];
                current_label = DatasetAttributes_label_dict[attribute];

                right_referenceBorder = current_panel.Width;
                left_referenceBorder = right_referenceBorder - shared_sortByButton_width;
                top_referenceBorder = 0;
                bottom_referenceBorder = UserInterface_lines[0].Get_location_y_of_tool_linked_indicated_attribute(attribute);
                Form_default_settings.Button_miniSquare_add_default_values_and_adjust_to_lower_right_referenceBorder(current_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);


                left_referenceBorder = 0;
                right_referenceBorder = current_button.Location.X;
                top_referenceBorder = 0;
                bottom_referenceBorder = UserInterface_lines[0].Get_location_y_of_tool_linked_indicated_attribute(attribute);
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(current_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            attributes = DatasetAttributes_cbButton_dict.Keys.ToArray();
            MyCheckBox_button current_cbButton;


            int cbButton_widthHeight;
            int half_cbButton_unused_height;
            int half_cbButton_unused_width;

            int cbButton_left_referenceBorder;
            int cbButton_right_referenceBorder;
            int cbButton_top_referenceBorder;
            int cbButton_bottom_referenceBorder;
            foreach (Dataset_attributes_enum attribute in attributes)
            {
                current_cbButton = DatasetAttributes_cbButton_dict[attribute];
                current_panel = DatasetAttributes_panel_dict[attribute];
                left_referenceBorder = 0;
                right_referenceBorder = current_panel.Width;
                top_referenceBorder = 0;
                bottom_referenceBorder = UserInterface_lines[0].Get_location_y_of_tool_linked_indicated_attribute(attribute);

                cbButton_widthHeight = size_of_deleteButton;// (int)Math.Min(right_referenceBorder - left_referenceBorder, bottom_referenceBorder - top_referenceBorder);
                half_cbButton_unused_height = (int)Math.Round(0.5F * (bottom_referenceBorder - top_referenceBorder - cbButton_widthHeight));
                half_cbButton_unused_width = (int)Math.Round(0.5F * (right_referenceBorder - left_referenceBorder - cbButton_widthHeight));

                cbButton_left_referenceBorder = left_referenceBorder + half_cbButton_unused_width;
                cbButton_right_referenceBorder = right_referenceBorder - half_cbButton_unused_width;
                cbButton_top_referenceBorder = top_referenceBorder + half_cbButton_unused_height;
                cbButton_bottom_referenceBorder = bottom_referenceBorder - half_cbButton_unused_height;

                Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(current_cbButton, cbButton_left_referenceBorder, cbButton_right_referenceBorder, cbButton_top_referenceBorder, cbButton_bottom_referenceBorder);
            }

            Dataset_attributes_enum reference_attribute = Dataset_attributes_enum.Name;
            left_referenceBorder = this.Input_geneList_textBox.Location.X + this.Input_geneList_textBox.Width + (int)Math.Round(0.003*this.Overall_interface_panel.Width);
            right_referenceBorder = left_referenceBorder + (int)Math.Round(0.02F * this.Overall_interface_panel.Width);
            top_referenceBorder = this.DatasetAttributes_label_dict[reference_attribute].Location.Y + this.UserInterface_lines[0].Get_location_y_of_tool_linked_indicated_attribute(reference_attribute);
            bottom_referenceBorder = top_referenceBorder + this.UserInterface_lines[userInterface_lines_length - 2].Get_location_y_plus_height_of_tool_linked_indicated_attribute(reference_attribute);
            my_scrollBar = this.Dataset_scrollBar;
            Form_default_settings.ScrollBar_adjustCoordinatedToBorders_and_add_default_parameter(my_scrollBar, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Copy_custom_data_into_all_interface_fields(custom_data);
            Initialize_and_set_default_buttons_boxes_and_locations();
            Reset_empty_interface_lines_to_default(Next_userInterfaceLine_index);
        }

        private void Initialize_and_set_default_buttons_boxes_and_locations()
        {
            Dataset_attributes_enum[] visible_panels = new Dataset_attributes_enum[] { Dataset_attributes_enum.Delete, Dataset_attributes_enum.Name, Dataset_attributes_enum.Color };
            string initial_sortBy_button_text = Default_textBox_texts.Triangle_up_in_newCourier;
            Dataset_attributes_enum[] sortBy_button_attributes = DatasetAttributes_sortByButton_dict.Keys.ToArray();
            foreach (Dataset_attributes_enum sortBy_button_attribute in sortBy_button_attributes)
            {
                DatasetAttributes_sortByButton_dict[sortBy_button_attribute].Text = (string)initial_sortBy_button_text.Clone();
            }

            int start_x_location;
            int end_x_location;
            int x_distance_between_panels;
            Get_start_and_end_x_location_and_x_distance_between_panels(out start_x_location, out end_x_location, out x_distance_between_panels);
            int shared_y_location = (int)Math.Round(0.05*this.Overall_interface_panel.Height);
            Dataset_attributes_enum[] panel_attributes = DatasetAttributes_panel_dict.Keys.ToArray();
            Panel current_panel;
            foreach (Dataset_attributes_enum panel_attribute in panel_attributes)
            {
                current_panel = DatasetAttributes_panel_dict[panel_attribute];
                current_panel.Location = new Point(current_panel.Location.X , shared_y_location);
            }
            Dataset_compatibility_myPanelLabel.Location = new Point(start_x_location + x_distance_between_panels + DatasetAttributes_panel_dict[Dataset_attributes_enum.Delete].Width, Dataset_compatibility_myPanelLabel.Location.Y);
            Set_attributes_with_visible_panel_if_space_and_return_final_selection(visible_panels);
            Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }

        public string Get_default_integrationGroup(DatasetSummary_line_class[] dataSummary_lines)
        {
            bool allInOne_is_the_only_integrationGroup = true;
            int dataSummaries_length = dataSummary_lines.Length;
            DatasetSummary_line_class dataSummary_line;
            for (int indexDS = 0; indexDS < dataSummaries_length; indexDS++)
            {
                dataSummary_line = dataSummary_lines[indexDS];
                if (!dataSummary_line.IntegrationGroup.Equals(Default_textBox_texts.IntegrationGroup_textBox_allInOne))
                {
                    allInOne_is_the_only_integrationGroup = false;
                    break;
                }
            }
            string default_integrationGroup = "error";
            if (allInOne_is_the_only_integrationGroup) { default_integrationGroup = (string)Default_textBox_texts.IntegrationGroup_textBox_allInOne.Clone(); }
            else { default_integrationGroup = (string)Default_textBox_texts.IntegrationGroup_textBox_undefined.Clone(); }
            return default_integrationGroup;
        }

        public string Get_default_integrationGroup_from_lastSaved_datasetSummaries()
        {
            return Get_default_integrationGroup(this.LastSaved_datasetSummaries);
        }

        #region Add to arrays
        public void Add_to_userInterface_datasetSummaries(DatasetSummary_line_class[] add_userInterface_datasetSummaries)
        {
            int this_length = this.UserInterface_dataSummaries.Length;
            int add_length = add_userInterface_datasetSummaries.Length;
            int new_length = this_length + add_length;
            DatasetSummary_line_class[] new_userInterface_datasetSummaries = new DatasetSummary_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_userInterface_datasetSummaries[indexNew] = this.UserInterface_dataSummaries[indexNew];
            }
            for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_userInterface_datasetSummaries[indexNew] = add_userInterface_datasetSummaries[indexAdd];
            }
            this.UserInterface_dataSummaries = new_userInterface_datasetSummaries;
        }

        #endregion

        #region Indexes
        private int Get_maximum_value_for_first_shown_userInterface_dataSummaries_index()
        {
            return Math.Max(0, UserInterface_dataSummaries.Length - UserInterfaceLines_available_for_userInterfaceDataSummaries);
        }

        private void Set_first_shown_userInterface_dataSummaries_index_to_maximum_value()
        {
            First_shown_userInterface_dataSummaries_index = Get_maximum_value_for_first_shown_userInterface_dataSummaries_index();
        }

        private void Set_first_shown_userInterface_dataSummaries_index_to_maximum_value_if_larger()
        {
            int maximum_value = Get_maximum_value_for_first_shown_userInterface_dataSummaries_index();
            if (First_shown_userInterface_dataSummaries_index > maximum_value)
            {
                First_shown_userInterface_dataSummaries_index = maximum_value;
            }
        }

        #endregion

        #region Sort by sortBy buttons
        private Custom_data_class Sort_customData_by_datasetAttribute_and_sortButtonInstructions_and_update_sortButton(Custom_data_class custom_data, Dataset_attributes_enum dataset_attribute, ref string button_direction_text)
        {
            if (button_direction_text.Equals(Default_textBox_texts.Triangle_up_in_newCourier))
            {
                switch (dataset_attribute)
                {
                    case Dataset_attributes_enum.Name:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.SampleName).ToArray();
                        break;
                    case Dataset_attributes_enum.Timepoint:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.Timepoint_in_days).ToArray();
                        break;
                    case Dataset_attributes_enum.EntryType:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.EntryType).ToArray();
                        break;
                    case Dataset_attributes_enum.IntegrationGroup:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.IntegrationGroup).ToArray();
                        break;
                    case Dataset_attributes_enum.Color:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.SampleColor_for_data.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.SourceFile:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.Source_fileName.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.BgGenes:
                        custom_data.Custom_data = custom_data.Custom_data.OrderBy(l => l.BgGenes_listName.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.Substring:
                        throw new Exception();
                    default:
                        throw new Exception();
                }
                button_direction_text = Default_textBox_texts.Triangle_down_in_newCourier;
            }
            else
            {
                switch (dataset_attribute)
                {
                    case Dataset_attributes_enum.Name:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.SampleName).ToArray();
                        break;
                    case Dataset_attributes_enum.Timepoint:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.Timepoint_in_days).ToArray();
                        break;
                    case Dataset_attributes_enum.EntryType:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.EntryType).ToArray();
                        break;
                    case Dataset_attributes_enum.IntegrationGroup:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.IntegrationGroup).ToArray();
                        break;
                    case Dataset_attributes_enum.Color:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.SampleColor_for_data.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.SourceFile:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.Source_fileName.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.BgGenes:
                        custom_data.Custom_data = custom_data.Custom_data.OrderByDescending(l => l.BgGenes_listName.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.Substring:
                        throw new Exception();
                    default:
                        throw new Exception();
                }
                button_direction_text = Default_textBox_texts.Triangle_up_in_newCourier;
            }
            Copy_custom_data_into_lastSaved_dataSummaries_and_available_background_gene_list_names_and_preserve_substrings(custom_data);
            Copy_lastSafedDataSummaries_into_userInferfaceDataSummaries();
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            First_shown_userInterface_dataSummaries_index = 0;
            return custom_data;
        }
        private void Sort_lastSavedDataSummaries_dataSummaries_update_sortButton_and_copy_userInterfaceDataSummaries_into_userInterfaceLines_and_update_graphical_interface(Dataset_attributes_enum dataset_attribute, ref string button_direction_text)
        {
            Copy_current_userInferface_lines_into_related_userInterfaceDatasetSummaries_and_check_for_dataset_incompatibilities();
            if (button_direction_text.Equals(Default_textBox_texts.Triangle_up_in_newCourier))
            {
                switch (dataset_attribute)
                {
                    case Dataset_attributes_enum.Name:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.SampleName).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.SampleName).ToArray();
                        break;
                    case Dataset_attributes_enum.Timepoint:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.Timepoint).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.Timepoint).ToArray();
                        break;
                    case Dataset_attributes_enum.EntryType:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.EntryType).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.EntryType).ToArray();
                        break;
                    case Dataset_attributes_enum.IntegrationGroup:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.IntegrationGroup).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.IntegrationGroup).ToArray();
                        break;
                    case Dataset_attributes_enum.Color:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.SampleColor.ToString()).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.SampleColor.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.SourceFile:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.Source_fileName).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.Source_fileName).ToArray();
                        break;
                    case Dataset_attributes_enum.Substring:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.Substring).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.Substring).ToArray();
                        break;
                    case Dataset_attributes_enum.BgGenes:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.Background_geneListName).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.Background_geneListName).ToArray();
                        break;
                    case Dataset_attributes_enum.Dataset_order_no:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l=>l.IntegrationGroup).ThenBy(l => l.Results_no).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.Results_no).ToArray();
                        break;
                    default:
                        throw new Exception();
                }
                button_direction_text = Default_textBox_texts.Triangle_down_in_newCourier;
            }
            else
            {
                switch (dataset_attribute)
                {
                    case Dataset_attributes_enum.Name:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.SampleName).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.SampleName).ToArray();
                        break;
                    case Dataset_attributes_enum.Timepoint:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.Timepoint).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.Timepoint).ToArray();
                        break;
                    case Dataset_attributes_enum.EntryType:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.EntryType).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.EntryType).ToArray();
                        break;
                    case Dataset_attributes_enum.IntegrationGroup:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.IntegrationGroup).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.IntegrationGroup).ToArray();
                        break;
                    case Dataset_attributes_enum.Color:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.SampleColor.ToString()).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.SampleColor.ToString()).ToArray();
                        break;
                    case Dataset_attributes_enum.Substring:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.Substring).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.Substring).ToArray();
                        break;
                    case Dataset_attributes_enum.SourceFile:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.Source_fileName).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.Source_fileName).ToArray();
                        break;
                    case Dataset_attributes_enum.BgGenes:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.Background_geneListName).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.Background_geneListName).ToArray();
                        break;
                    case Dataset_attributes_enum.Dataset_order_no:
                        this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderByDescending(l => l.IntegrationGroup).ThenByDescending(l => l.Results_no).ToArray();
                        this.LastSaved_datasetSummaries = this.LastSaved_datasetSummaries.OrderByDescending(l => l.IntegrationGroup).ThenByDescending(l => l.Results_no).ToArray();
                        break;
                    default:
                        throw new Exception();
                }
                button_direction_text = Default_textBox_texts.Triangle_up_in_newCourier;
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            First_shown_userInterface_dataSummaries_index = 0;
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        public void All_delete_button_click()
        {
            bool all_checked = DatasetAttributes_cbButton_dict[Dataset_attributes_enum.Delete].Checked;
            foreach (DatasetSummary_line_class datasetSummary_line in this.UserInterface_dataSummaries)
            {
                datasetSummary_line.Delete = all_checked;
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        public void SortBy_button_click(Dataset_attributes_enum dataset_attribute)
        {
            Button sortBy_button = DatasetAttributes_sortByButton_dict[dataset_attribute];
            string directionality_buttonText = (string)sortBy_button.Text.Clone();
            Sort_lastSavedDataSummaries_dataSummaries_update_sortButton_and_copy_userInterfaceDataSummaries_into_userInterfaceLines_and_update_graphical_interface(dataset_attribute, ref directionality_buttonText);
            sortBy_button.Text = (string)directionality_buttonText.Clone();
        }
        #endregion

        #region Graphical interface excluding panel visibilities and xlocations
        private void Update_dataset_scrollBar()
        {
            int uI_dataSummaries_length = UserInterface_dataSummaries.Length;
            int uI_lines_length = UserInterface_lines.Length;
            if (uI_dataSummaries_length >= uI_lines_length - 1)
            {
                float fraction_of_dataSummaries_shown_in_interface_lines = (float)(Last_filled_userInterfaceLines_index + 1) / (float)uI_dataSummaries_length;
                Dataset_scrollBar.Visible = true;
                Dataset_scrollBar.Enabled = true;
                Dataset_scrollBar.Minimum = 0;
                Dataset_scrollBar.Maximum = uI_dataSummaries_length;
                Dataset_scrollBar.SmallChange = 1;
                Dataset_scrollBar.LargeChange = uI_lines_length;// Dataset_scrollBar.Maximum - (int)Math.Round(fraction_of_dataSummaries_shown_in_interface_lines * (float)Dataset_scrollBar.Maximum);
                Dataset_scrollBar.Value = First_shown_userInterface_dataSummaries_index;
                Dataset_scrollBar.Refresh();
            }
            else
            {
                Dataset_scrollBar.Visible = false;
                Dataset_scrollBar.Enabled = false;
            }
        }
        public void Reset_empty_interface_lines_to_default(int index_first_empty_line)
        {
            Dictionary<Color, bool> usedColors = new Dictionary<Color, bool>();
            int lastSaved_datasetSummaries_length = this.LastSaved_datasetSummaries.Length;
            Color current_color;
            for (int indexLE = 0; indexLE < lastSaved_datasetSummaries_length; indexLE++)
            {
                current_color = this.LastSaved_datasetSummaries[indexLE].SampleColor;
                if (!usedColors.ContainsKey(current_color))
                {
                    usedColors.Add(current_color, true);
                }
            }
            Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
            Color selectable_color;
            int indexSelectable_colors = -1;
            int userInterface_length = this.UserInterface_lines.Length;
            string integrationGroup = Get_default_integrationGroup(this.UserInterface_dataSummaries);
            for (int indexUI = index_first_empty_line; indexUI < userInterface_length; indexUI++)
            {
                do
                {
                    indexSelectable_colors++;
                    if (indexSelectable_colors==selectable_colors.Length) 
                    { 
                        indexSelectable_colors = 0;
                        usedColors.Clear();
                    }
                    selectable_color = selectable_colors[indexSelectable_colors];
                } while (usedColors.ContainsKey(selectable_color));
                this.UserInterface_lines[indexUI].Reset_to_empty_invisible_and_refresh(selectable_color, Form_default_settings);
                this.UserInterface_lines[indexUI].Dataset_integrationGroup_textBox.SilentText = (string)integrationGroup.Clone();
                this.UserInterface_lines[indexUI].Dataset_delete_cbButton.SilentChecked = false;
            }
           // this.UserInterface_lines[Next_userInterfaceLine_index].Set_visible_parameter_for_all_entities_excluding_delete_button(true);
        }
        private void Update_lower_positions_and_visibility_of_buttons_and_panels()
        {
            bool sorting_buttons_visible = Last_filled_userInterfaceLines_index >= 1;
            Dataset_attributes_enum[] sortByButton_attributes = DatasetAttributes_sortByButton_dict.Keys.ToArray();
            foreach (Dataset_attributes_enum sortByButton_attribute in sortByButton_attributes)
            {
                DatasetAttributes_sortByButton_dict[sortByButton_attribute].Visible = sorting_buttons_visible;
            }

            #region Set new y locations
            int indexOfLast_visible_interfaceLine = -1;
            if (Is_filter_mode)
            {
                indexOfLast_visible_interfaceLine = Last_filled_userInterfaceLines_index;
            }
            else
            {
                indexOfLast_visible_interfaceLine = Next_userInterfaceLine_index;
            }
            Panel representative_panel = DatasetAttributes_panel_dict[Dataset_attributes_enum.Name];
            int lower_y_location_of_last_userInterface_line = -1;
            int new_lower_y_location_of_delete_panel = -1;
            if (Last_filled_userInterfaceLines_index >= 0)
            {
                new_lower_y_location_of_delete_panel =   representative_panel.Location.Y
                                                       + UserInterface_lines[Last_filled_userInterfaceLines_index].Dataset_sampleName_textBox.Location.Y
                                                       + UserInterface_lines[Last_filled_userInterfaceLines_index].Dataset_sampleName_textBox.Size.Height;
            }
            else
            {
                new_lower_y_location_of_delete_panel = representative_panel.Location.Y;
            }
            if (indexOfLast_visible_interfaceLine >= 0)
            {
                lower_y_location_of_last_userInterface_line =   representative_panel.Location.Y
                                                              + UserInterface_lines[indexOfLast_visible_interfaceLine].Dataset_sampleName_textBox.Location.Y
                                                              + UserInterface_lines[indexOfLast_visible_interfaceLine].Dataset_sampleName_textBox.Size.Height;
            }
            else
            {
                lower_y_location_of_last_userInterface_line = representative_panel.Location.Y;
            }

            int new_lower_y_location_of_panels = lower_y_location_of_last_userInterface_line;
            int one_half_of_left_space = (int)Math.Round(0.5F * (float)(AnalyzeDataset_button.Location.Y - new_lower_y_location_of_panels));
            int new_y_location_for_uncompatible_label = new_lower_y_location_of_panels;
            int dataset_incompatibility_height = one_half_of_left_space;
            int new_y_location_for_changes_buttons = new_y_location_for_uncompatible_label + dataset_incompatibility_height;
            #endregion

            #region Set new lower y locations for all panels
            Dataset_attributes_enum[] panel_attributes = DatasetAttributes_panel_dict.Keys.ToArray();
            Panel current_panel;
            foreach (Dataset_attributes_enum panel_attribute in panel_attributes)
            {
                current_panel = DatasetAttributes_panel_dict[panel_attribute];
                if (panel_attribute.Equals(Dataset_attributes_enum.Delete))
                {
                    current_panel.Height = new_lower_y_location_of_delete_panel - current_panel.Location.Y;
                }
                else
                {
                    current_panel.Height = new_lower_y_location_of_panels - current_panel.Location.Y;
                }
            }
            #endregion

            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            Button my_button;

            int middle_of_remaining_panel_right_of_textBox = this.Input_geneList_textBox.Location.X + this.Input_geneList_textBox.Width + (int)Math.Round(0.4F*(this.Overall_interface_panel.Width - this.Input_geneList_textBox.Location.X - this.Input_geneList_textBox.Width));
            int button_width = (int)Math.Round(0.4F * middle_of_remaining_panel_right_of_textBox);

            top_referenceBorder = new_y_location_for_changes_buttons;
            bottom_referenceBorder = Math.Min(AddNewDataset_button.Location.Y,
                                              new_y_location_for_changes_buttons + (int)Math.Round(0.07F * this.Overall_interface_panel.Height));

            left_referenceBorder = middle_of_remaining_panel_right_of_textBox - button_width;
            right_referenceBorder = left_referenceBorder + button_width;
            my_button = Changes_reset_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = middle_of_remaining_panel_right_of_textBox;
            right_referenceBorder = left_referenceBorder + button_width;
            my_button = Changes_update_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.DatasetAttributes_panel_dict[Dataset_attributes_enum.Name].Location.X;
            right_referenceBorder = this.Overall_interface_panel.Width;
            top_referenceBorder = new_y_location_for_uncompatible_label;
            bottom_referenceBorder = Changes_reset_button.Location.Y;
            Dataset_compatibility_myPanelLabel.TextAlign = ContentAlignment.MiddleLeft;
            Dataset_compatibility_myPanelLabel.Font_style = FontStyle.Bold;
            Dataset_compatibility_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }
        private void Update_datasetCompatibilityDelete_labels_and_description_in_graphical_interface(DatasetSummary_line_class[] dataSummary_lines)
        {
            Dataset_compatibility_enum described_incompatibility = Dataset_compatibility_enum.Ok;
            int count_of_datasets_labeled_for_deletion = 0;
            bool datasetCompatibility_label_is_visible = false;
            bool at_least_one_invalid_value = false;
            foreach (DatasetSummary_line_class summary_line in dataSummary_lines)
            {
                if (summary_line.Delete) { count_of_datasets_labeled_for_deletion += summary_line.Datasets_count; }
                if (!summary_line.Dataset_compatibility.Equals(Dataset_compatibility_enum.Ok))
                {
                    switch (described_incompatibility)
                    {
                        case Dataset_compatibility_enum.Ok:
                            described_incompatibility = summary_line.Dataset_compatibility;
                            break;
                        case Dataset_compatibility_enum.Duplicated_dataset:
                            break;
                        default:
                            throw new Exception();
                    }
                    if (summary_line.Row_contains_invalid_value)
                    {
                        at_least_one_invalid_value = true;
                    }
                }
            }
            if (Is_filter_mode)
            {
                datasetCompatibility_label_is_visible = false;
            }
            else if ((count_of_datasets_labeled_for_deletion > 0)&&(!at_least_one_invalid_value))
            {
                datasetCompatibility_label_is_visible = true;
                Dataset_compatibility_myPanelLabel.Status = MyPanel_label_status_enum.Red_warning;
                if (count_of_datasets_labeled_for_deletion > 1)
                { Dataset_compatibility_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(count_of_datasets_labeled_for_deletion + " datasets marked for deletion"); }
                else { Dataset_compatibility_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(count_of_datasets_labeled_for_deletion + " dataset marked for deletion"); }
                described_incompatibility = Dataset_compatibility_enum.Ok;
                Updates_are_allowed_changes_button_allowed_to_be_visible = true;
            }
            else if (!described_incompatibility.Equals(Dataset_compatibility_enum.Ok))
            {
                switch (described_incompatibility)
                {
                    case Dataset_compatibility_enum.Duplicated_dataset:
                        Dataset_compatibility_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Duplicated datasets in same integration group (go to 'Organize data')");
                        break;
                    default:
                        throw new Exception();
                }
                Dataset_compatibility_myPanelLabel.Status = MyPanel_label_status_enum.Purple_warning;
                datasetCompatibility_label_is_visible = true;
                Updates_are_allowed_changes_button_allowed_to_be_visible = false;
            }
            else if (  (this.Hidden_userInterface_dataSummaries.Length + this.UserInterface_dataSummaries.Length > Math.Max(0, Last_filled_userInterfaceLines_index))
                     &&(!at_least_one_invalid_value))
            {
                int datasets_count = this.Hidden_userInterface_dataSummaries.Length + this.UserInterface_dataSummaries.Length;
                if (datasets_count==1)
                {
                    Dataset_compatibility_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(datasets_count + " dataset");
                }
                else
                {
                    Dataset_compatibility_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(datasets_count + " datasets");
                }
                Dataset_compatibility_myPanelLabel.Status = MyPanel_label_status_enum.Regular;
                datasetCompatibility_label_is_visible = true;
                Updates_are_allowed_changes_button_allowed_to_be_visible = true;
            }
            else if (at_least_one_invalid_value)
            {
                datasetCompatibility_label_is_visible = false;
                Updates_are_allowed_changes_button_allowed_to_be_visible = false;
            }
            else
            { 
                datasetCompatibility_label_is_visible = false;
                Updates_are_allowed_changes_button_allowed_to_be_visible = true;
            }
            Dataset_compatibility_myPanelLabel.Visible = datasetCompatibility_label_is_visible;
            int interface_lines_length = this.UserInterface_lines.Length;
            for (int indexIL = 0; indexIL < interface_lines_length; indexIL++)
            {
                this.UserInterface_lines[indexIL].Update_userInterface_backcolor_and_refresh(described_incompatibility,Form_default_settings);
            }
        }
        private void Update_visibility_of_change_buttons()
        {
            Changes_reset_button.Visible = !LastSavedDataSummaries_identical_with_userInterface;
            Changes_update_button.Visible = !LastSavedDataSummaries_identical_with_userInterface;
            if (!Updates_are_allowed_changes_button_allowed_to_be_visible) { Changes_update_button.Visible = false; }
        }
        public void Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations()
        {
            LastSavedDataSummaries_identical_with_userInterface = Are_lastSavedDataSummaries_equalTo_userInferfaceDataSummaries_and_hiddenUserInterfaceDataSummaries_ignoring_order();
            Update_datasetCompatibilityDelete_labels_and_description_in_graphical_interface(this.UserInterface_dataSummaries);
            Update_lower_positions_and_visibility_of_buttons_and_panels();
            Reset_empty_interface_lines_to_default(Next_userInterfaceLine_index+1);
            Update_dataset_scrollBar();
            Update_visibility_of_change_buttons();
        }
        #endregion

        #region Graphical interface only visibility and xlocations
        private void Get_start_and_end_x_location_and_x_distance_between_panels(out int start_x_location, out int end_x_location, out int x_distance_between_panels)
        {
            start_x_location = this.Dataset_scrollBar.Location.X + this.Dataset_scrollBar.Width + (int)Math.Round(0.001F*this.Overall_interface_panel.Width);
            end_x_location = Overall_interface_panel.Width;
            x_distance_between_panels = 0;
        }
        public Dataset_attributes_enum[] Set_attributes_with_visible_panel_if_space_and_return_final_selection(Dataset_attributes_enum[] set_attributes_of_visible_panels, params Dataset_attributes_enum[] mandatory_attributes)
        {
            int start_x_location;
            int end_x_location;
            int next_potential_x_location;
            int x_distance_between_panels;
            Get_start_and_end_x_location_and_x_distance_between_panels(out start_x_location, out end_x_location, out x_distance_between_panels);
            int next_x_location = start_x_location;
            List<Dataset_attributes_enum> new_visible_attributes = new List<Dataset_attributes_enum>();
            Panel current_panel;

            List<Dataset_attributes_enum> final_attributes = new List<Dataset_attributes_enum>();
            Dataset_attributes_enum[] high_priority_attributes = new Dataset_attributes_enum[] { Dataset_attributes_enum.Delete, Dataset_attributes_enum.Datasets_count, Dataset_attributes_enum.Genes_count };
            foreach (Dataset_attributes_enum high_priority_attribute in high_priority_attributes)
            {
                if (set_attributes_of_visible_panels.Contains(high_priority_attribute))
                {
                    final_attributes.Add(high_priority_attribute);
                }
            }
            mandatory_attributes = Overlap_class.Get_part_of_list1_but_not_of_list2(mandatory_attributes, final_attributes.ToArray());
            final_attributes.AddRange(mandatory_attributes);
            set_attributes_of_visible_panels = Overlap_class.Get_part_of_list1_but_not_of_list2(set_attributes_of_visible_panels, final_attributes.ToArray());
            final_attributes.AddRange(set_attributes_of_visible_panels);

            foreach (Dataset_attributes_enum set_attribute_of_visible_panels in final_attributes)
            {
                current_panel = DatasetAttributes_panel_dict[set_attribute_of_visible_panels];
                next_potential_x_location = next_x_location + current_panel.Width + x_distance_between_panels;
                if (next_potential_x_location <= end_x_location)
                {
                    next_x_location = next_potential_x_location;
                    new_visible_attributes.Add(set_attribute_of_visible_panels);
                }
            }
            this.Dataset_attributes_with_visible_panels = new_visible_attributes.ToArray();
            return new_visible_attributes.ToArray();
        }
        public void Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes()
        {
            int start_x_location;
            int end_x_location;
            int x_distance_between_panels;
            Get_start_and_end_x_location_and_x_distance_between_panels(out start_x_location, out end_x_location, out x_distance_between_panels);
            int next_x_location = start_x_location;

            Dataset_attributes_enum[] attributes_with_visible_panels = this.Dataset_attributes_with_visible_panels;
            Dataset_attributes_enum[] all_attributes = DatasetAttributes_panel_dict.Keys.ToArray();
            Panel current_panel;
            foreach (Dataset_attributes_enum attribute in all_attributes)
            {
                current_panel = DatasetAttributes_panel_dict[attribute];
                if (attributes_with_visible_panels.Contains(attribute))
                {
                    current_panel.Location = new Point(next_x_location, current_panel.Location.Y);
                    next_x_location += current_panel.Width + x_distance_between_panels;
                    current_panel.Visible = Overall_visibility;
                }
                else if (attribute.Equals(Dataset_attributes_enum.Delete))
                {
                    next_x_location += current_panel.Width + x_distance_between_panels;
                    current_panel.Visible = false;
                }
                else
                {
                    current_panel.Visible = false;
                }
            }
        }
        #endregion

        #region Visble data attributes
        public Dataset_attributes_enum[] Get_dataset_attributes_defining_visible_panels()
        {
            return Dataset_attributes_with_visible_panels;
        }
        #endregion

        #region Substring
        public void Set_substrings_in_lastSavedDataSummaries_and_copy_into_userInterface_dataSummaries(string delimiter, int[] indexesLeft_oneBased, int[] indexesRight_oneBased)
        {
            int lastSaved_dataSummaries_length = this.LastSaved_datasetSummaries.Length;
            DatasetSummary_line_class lastSaved_data_summary_line;
            string leftOverString;
            List<string> splitStrings_list = new List<string>();
            string[] splitStrings;
            int splitStrings_length;
            int firstIndex;
            List<int> finalIndexes_oneBasedIndexes_left = new List<int>();
            StringBuilder substring_sb = new StringBuilder();
            string add_substring;
            for (int indexLS = 0; indexLS < lastSaved_dataSummaries_length; indexLS++)
            {
                lastSaved_data_summary_line = this.LastSaved_datasetSummaries[indexLS];
                finalIndexes_oneBasedIndexes_left.Clear();
                leftOverString = (string)lastSaved_data_summary_line.SampleName.Clone();
                splitStrings_list.Clear();
                do
                {
                    firstIndex = leftOverString.IndexOf(delimiter);
                    if (firstIndex != -1)
                    {
                        add_substring = leftOverString.Substring(0, firstIndex);
                        splitStrings_list.Add(add_substring);
                        leftOverString = leftOverString.Substring(firstIndex + delimiter.Length, leftOverString.Length - firstIndex - delimiter.Length);
                    }
                    else
                    {
                        splitStrings_list.Add(leftOverString);
                    }
                } while (firstIndex != -1);
                substring_sb.Clear();
                splitStrings = splitStrings_list.ToArray();
                splitStrings_length = splitStrings.Length;
                finalIndexes_oneBasedIndexes_left.AddRange(indexesLeft_oneBased);
                foreach (int indexRight in indexesRight_oneBased)
                {
                    finalIndexes_oneBasedIndexes_left.Add(splitStrings_length - indexRight + 1);
                }
                finalIndexes_oneBasedIndexes_left = finalIndexes_oneBasedIndexes_left.Distinct().OrderBy(l => l).ToList();
                foreach (int indexOneBasedLeft in finalIndexes_oneBasedIndexes_left)
                {
                    if ((indexOneBasedLeft >= 1) && (indexOneBasedLeft <= splitStrings_length))
                    {
                        if (substring_sb.Length > 0) { substring_sb.AppendFormat("-"); }
                        substring_sb.AppendFormat(splitStrings[indexOneBasedLeft-1]);
                    }
                }
                if (substring_sb.Length>0)
                {
                    lastSaved_data_summary_line.Substring = substring_sb.ToString();
                }
                else
                {
                    lastSaved_data_summary_line.Substring = (string)lastSaved_data_summary_line.SampleName.Clone();
                }
            }
            Copy_lastSafedDataSummaries_into_userInferfaceDataSummaries();
        }
        #endregion

        #region Background genes
        public void Set_all_background_genes_in_userInterface_dataSummaries_to_input_and_update_interface_lines(string new_bgGeneListName)
        {
            foreach (DatasetSummary_line_class userInterface_dataSummary_line in this.UserInterface_dataSummaries)
            {
                userInterface_dataSummary_line.Background_geneListName = (string)new_bgGeneListName.Clone();
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }

        public void Set_all_background_genes_to_matching_lists_based_on_fileNames_in_userInterface_dataSummaries_to_input_and_update_interface_lines(string[] potential_bgGeneListNames)
        {
            this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.Source_fileName).ToArray();
            int userInterface_dataSummaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class dataSummary_line;
            potential_bgGeneListNames = potential_bgGeneListNames.Distinct().OrderBy(l => l).ToArray();
            string potential_bgGeneListName;
            int potential_bgGeneListNames_length = potential_bgGeneListNames.Length;
            int indexPotential = 0;
            int stringCompare = -2;
            string suggested_bgGeneListName = "error";
            for (int indexUI=0; indexUI<userInterface_dataSummaries_length; indexUI++)
            {
                dataSummary_line = this.UserInterface_dataSummaries[indexUI];
                if (  (indexUI==0)
                    ||(!dataSummary_line.Source_fileName.Equals(this.UserInterface_dataSummaries[indexUI-1].Source_fileName)))
                {
                    suggested_bgGeneListName = "error";
                    suggested_bgGeneListName = System.IO.Path.GetFileNameWithoutExtension(dataSummary_line.Source_fileName) + Global_class.Bg_genes_label;
                    stringCompare = -2;
                    while ((indexPotential<potential_bgGeneListNames_length)&&(stringCompare<0))
                    {
                        potential_bgGeneListName = potential_bgGeneListNames[indexPotential];
                        stringCompare = potential_bgGeneListName.CompareTo(suggested_bgGeneListName);
                        if (stringCompare<0) { indexPotential++; }
                    }
                }
                if (stringCompare==0)
                {
                    dataSummary_line.Background_geneListName = (string)suggested_bgGeneListName.Clone();
                }
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        #endregion

        #region Filter mode
        private void Move_duplicated_userInterfaceSummaries_to_hidden_summaries_and_count_how_many_datasets_per_main_summary_line()
        {
            Dataset_attributes_enum[] filter_dataset_attributes = this.Dataset_attributes_defining_filter_mode;
            if (Hidden_userInterface_dataSummaries.Length != 0) { throw new Exception(); }
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, int>>>>> name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, int>>>>>();
            int userInterface_dataSummaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class dataSummary_line;
            List<DatasetSummary_line_class> new_hidden_userInterface_dataSummaries = new List<DatasetSummary_line_class>();
            List<DatasetSummary_line_class> new_userInterface_dataSummaries = new List<DatasetSummary_line_class>();
            bool consider_name = filter_dataset_attributes.Contains(Dataset_attributes_enum.Name);
            bool consider_timepoint = filter_dataset_attributes.Contains(Dataset_attributes_enum.Timepoint);
            bool consider_entryType = filter_dataset_attributes.Contains(Dataset_attributes_enum.EntryType);
            bool consider_substring = filter_dataset_attributes.Contains(Dataset_attributes_enum.Substring);
            bool consider_sourceFile = filter_dataset_attributes.Contains(Dataset_attributes_enum.SourceFile);
            string current_name;
            float current_timepointInDays;
            Entry_type_enum current_entryType;
            string current_substring;
            string current_sourceFile;
            for (int indexDS = 0; indexDS < userInterface_dataSummaries_length; indexDS++)
            {
                dataSummary_line = this.UserInterface_dataSummaries[indexDS];
                if (consider_name) { current_name = (string)dataSummary_line.SampleName.Clone(); }
                else { current_name = "Not_considered"; }
                if (consider_timepoint) { current_timepointInDays = dataSummary_line.Timepoint_in_days; }
                else { current_timepointInDays = -1; }
                if (consider_entryType) { current_entryType = dataSummary_line.EntryType; }
                else { current_entryType = Entry_type_enum.E_m_p_t_y; }
                if (consider_sourceFile) { current_sourceFile = dataSummary_line.Source_fileName; }
                else { current_sourceFile = "Not_considered"; }
                if (consider_substring) { current_substring = dataSummary_line.Substring; }
                else { current_substring = "Not_considered"; }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict.ContainsKey(current_name))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict.Add(current_name, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, int>>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name].ContainsKey(current_timepointInDays))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name].Add(current_timepointInDays, new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, int>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays].ContainsKey(current_entryType))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays].Add(current_entryType, new Dictionary<string, Dictionary<string, int>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays][current_entryType].ContainsKey(current_sourceFile))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays][current_entryType].Add(current_sourceFile, new Dictionary<string, int>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile].ContainsKey(current_substring))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile].Add(current_substring, 1);
                    new_userInterface_dataSummaries.Add(dataSummary_line);
                }
                else
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile][current_substring]++;
                    new_hidden_userInterface_dataSummaries.Add(dataSummary_line);
                }
            }
            this.Hidden_userInterface_dataSummaries = new_hidden_userInterface_dataSummaries.ToArray();
            this.UserInterface_dataSummaries = new_userInterface_dataSummaries.ToArray();
            foreach (DatasetSummary_line_class userInterface_dataSummary_line in this.UserInterface_dataSummaries)
            {
                if (consider_name) { current_name = (string)userInterface_dataSummary_line.SampleName.Clone(); }
                else { current_name = "Not_considered"; }
                if (consider_timepoint) { current_timepointInDays = userInterface_dataSummary_line.Timepoint_in_days; }
                else { current_timepointInDays = -1; }
                if (consider_entryType) { current_entryType = userInterface_dataSummary_line.EntryType; }
                else { current_entryType = Entry_type_enum.E_m_p_t_y; }
                if (consider_sourceFile) { current_sourceFile = userInterface_dataSummary_line.Source_fileName; }
                else { current_sourceFile = "Not_considered"; }
                if (consider_substring) { current_substring = userInterface_dataSummary_line.Substring; }
                else { current_substring = "Not_considered"; }
                userInterface_dataSummary_line.Datasets_count = name_timepointInDays_entryType_sourceFile_substring_datasetsCount_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile][current_substring];
            }
        }

        private void Set_readOnly_of_all_dataset_defining_attributes_and_deleteCheckBox(bool readOnly)
        {
            int userInterface_lines_length = this.UserInterface_lines.Length;
            DatasetSummary_userInterface_line_class dataUserInterface_line;
            for (int indexUI=0; indexUI<userInterface_lines_length;indexUI++)
            {
                dataUserInterface_line = this.UserInterface_lines[indexUI];
                dataUserInterface_line.Set_readOnly_property_for_dataset_defining_attributes_and_delete_checkBox(readOnly);
            }
        }

        public void Set_to_filter_mode(Dataset_attributes_enum[] dataset_attributes_for_filter_mode)
        {
            if (Is_filter_mode) { throw new Exception(); }
            List<Dataset_attributes_enum> final_attributes = new List<Dataset_attributes_enum>();
            final_attributes.AddRange(dataset_attributes_for_filter_mode);
            final_attributes.Add(Dataset_attributes_enum.Datasets_count);
            this.Dataset_attributes_defining_filter_mode = final_attributes.ToArray();
            Set_readOnly_of_all_dataset_defining_attributes_and_deleteCheckBox(true);
            Move_duplicated_userInterfaceSummaries_to_hidden_summaries_and_count_how_many_datasets_per_main_summary_line();
            Update_hiddenUserInterfaceDataSummaries_based_on_userInterfaceDataSummaries();
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            First_shown_userInterface_dataSummaries_index = 0;
            this.Overall_visibility = false;
            Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            this.Overall_visibility = true;
            Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
        }

        private void Update_hiddenUserInterfaceDataSummaries_based_on_userInterfaceDataSummaries()
        {
            if (!Is_filter_mode) { throw new Exception(); }
            bool names_are_considered = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Name);
            bool timepoints_are_considered = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Timepoint);
            bool entryTypes_are_considered = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.EntryType);
            bool substrings_are_considered = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Substring);
            bool sourceFiles_are_considered = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.SourceFile);
            int userInterfaceDatasetSummaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class dataSummary_line;
            string current_name;
            Entry_type_enum current_entryType;
            float current_timepointInDays;
            string current_sourceFile;
            string current_substring;
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, DatasetSummary_line_class>>>>> name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, DatasetSummary_line_class>>>>>();
            for (int indexDS = 0; indexDS < userInterfaceDatasetSummaries_length; indexDS++)
            {
                dataSummary_line = this.UserInterface_dataSummaries[indexDS];
                if (names_are_considered) { current_name = (string)dataSummary_line.SampleName.Clone(); }
                else { current_name = "Not_considered"; }
                if (timepoints_are_considered) { current_timepointInDays = dataSummary_line.Timepoint_in_days; }
                else { current_timepointInDays = -1; }
                if (entryTypes_are_considered) { current_entryType = dataSummary_line.EntryType; }
                else { current_entryType = Entry_type_enum.E_m_p_t_y; }
                if (substrings_are_considered) { current_substring = dataSummary_line.Substring; }
                else { current_substring = "Not_considered"; }
                if (sourceFiles_are_considered) { current_sourceFile = dataSummary_line.Source_fileName; }
                else { current_sourceFile = "Not_considered"; }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict.ContainsKey(current_name))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict.Add(current_name, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, DatasetSummary_line_class>>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name].ContainsKey(current_timepointInDays))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name].Add(current_timepointInDays, new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, DatasetSummary_line_class>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays].ContainsKey(current_entryType))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays].Add(current_entryType, new Dictionary<string, Dictionary<string, DatasetSummary_line_class>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays][current_entryType].ContainsKey(current_sourceFile))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays][current_entryType].Add(current_sourceFile, new Dictionary<string, DatasetSummary_line_class>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile].ContainsKey(current_substring))
                {
                    name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile].Add(current_substring, dataSummary_line);
                }
            }

            bool set_colors = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Color);
            bool set_integration_groups = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.IntegrationGroup);
            bool set_delete = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Delete);
            bool set_bgGenes = this.Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.BgGenes);
            int set_true_count = 0;
            if (set_colors) { set_true_count++; }
            if (set_integration_groups) { set_true_count++; }
            if (set_delete) { set_true_count++; }
            if (set_bgGenes) { set_true_count++; }
            if (set_true_count!=1) { throw new Exception(); }
            int hidden_length = this.Hidden_userInterface_dataSummaries.Length;
            DatasetSummary_line_class hidden_dataSummary_line;
            DatasetSummary_line_class determining_dataSummary_line;
            for (int indexH=0; indexH<hidden_length;indexH++)
            {
                hidden_dataSummary_line = this.Hidden_userInterface_dataSummaries[indexH];
                if (names_are_considered) { current_name = (string)hidden_dataSummary_line.SampleName.Clone(); }
                else { current_name = "Not_considered"; }
                if (timepoints_are_considered) { current_timepointInDays = hidden_dataSummary_line.Timepoint_in_days; }
                else { current_timepointInDays = -1; }
                if (entryTypes_are_considered) { current_entryType = hidden_dataSummary_line.EntryType; }
                else { current_entryType = Entry_type_enum.E_m_p_t_y; }
                if (substrings_are_considered) { current_substring = hidden_dataSummary_line.Substring; }
                else { current_substring = "Not_considered"; }
                if (sourceFiles_are_considered) { current_sourceFile = hidden_dataSummary_line.Source_fileName; }
                else { current_sourceFile = "Not_considered"; }
                determining_dataSummary_line = name_timepointInDays_entryType_sourceFile_substring_datasetSummaryLine_dict[current_name][current_timepointInDays][current_entryType][current_sourceFile][current_substring];
                if (set_colors)
                { hidden_dataSummary_line.SampleColor = determining_dataSummary_line.SampleColor; }
                if (set_integration_groups)
                { hidden_dataSummary_line.IntegrationGroup = (string)determining_dataSummary_line.IntegrationGroup.Clone(); }
                if (set_delete)
                { hidden_dataSummary_line.Delete = determining_dataSummary_line.Delete; }
                if (set_bgGenes)
                { hidden_dataSummary_line.Background_geneListName = (string)determining_dataSummary_line.Background_geneListName.Clone(); }
            }
        }

        private Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>>> Add_datasetSummaries_to_filterRestore_dictionary(Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>>> name_timepointInDays_entryType_sourceFile_substring_lines_dict, DatasetSummary_line_class[] add_summary_lines)
        {
            int add_length = add_summary_lines.Length;
            DatasetSummary_line_class summary_line;
            bool consider_name = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Name);
            bool consider_timepoint = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Timepoint);
            bool consider_entryType = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.EntryType);
            bool consider_substring = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Substring);
            bool consider_sourceFile = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.SourceFile);

            string name;
            float timepointInDays;
            Entry_type_enum entryType;
            string substring;
            string sourceFileName;

            for (int indexAdd = 0; indexAdd<add_length; indexAdd++)
            {
                summary_line = add_summary_lines[indexAdd];
                if (consider_name) { name = summary_line.SampleName; }
                else { name = "Not Considered"; }
                if (consider_timepoint) { timepointInDays = summary_line.Timepoint_in_days; }
                else { timepointInDays = -1; }
                if (consider_entryType) { entryType = summary_line.EntryType; }
                else { entryType = Entry_type_enum.E_m_p_t_y; }
                if (consider_substring) { substring = summary_line.Substring; }
                else { substring = "Not Considered"; }
                if (consider_sourceFile) { sourceFileName = summary_line.Source_fileName; }
                else { sourceFileName = "Not Considered"; }

                if (!name_timepointInDays_entryType_sourceFile_substring_lines_dict.ContainsKey(name))
                {
                    name_timepointInDays_entryType_sourceFile_substring_lines_dict.Add(name, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_lines_dict[name].ContainsKey(timepointInDays))
                {
                    name_timepointInDays_entryType_sourceFile_substring_lines_dict[name].Add(timepointInDays, new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays].ContainsKey(entryType))
                {
                    name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays].Add(entryType, new Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays][entryType].ContainsKey(sourceFileName))
                {
                    name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays][entryType].Add(sourceFileName, new Dictionary<string, List<DatasetSummary_line_class>>());
                }
                if (!name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays][entryType][sourceFileName].ContainsKey(substring))
                {
                    name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays][entryType][sourceFileName].Add(substring, new List<DatasetSummary_line_class>());
                }
                name_timepointInDays_entryType_sourceFile_substring_lines_dict[name][timepointInDays][entryType][sourceFileName][substring].Add(summary_line);
            }
            return name_timepointInDays_entryType_sourceFile_substring_lines_dict;
        }

        private void Add_hiddenUserInterfaceDataSummaries_back_to_userInterfaceDataSummaries()
        {
            if (!Is_filter_mode) { throw new Exception(); }
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>>> name_timepointInDays_entryType_sourceFile_substring_lines_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>>>();
            Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>> timepointInDays_entryType_sourceFile_substring_lines_dict = new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>>();
            Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>> entryType_sourceFile_substring_lines_dict = new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>>();
            Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>> sourceFile_substring_lines_dict = new Dictionary<string, Dictionary<string, List<DatasetSummary_line_class>>>();
            Dictionary<string, List<DatasetSummary_line_class>> substring_lines_dict = new Dictionary<string, List<DatasetSummary_line_class>>();
            name_timepointInDays_entryType_sourceFile_substring_lines_dict = Add_datasetSummaries_to_filterRestore_dictionary(name_timepointInDays_entryType_sourceFile_substring_lines_dict, this.UserInterface_dataSummaries);
            name_timepointInDays_entryType_sourceFile_substring_lines_dict = Add_datasetSummaries_to_filterRestore_dictionary(name_timepointInDays_entryType_sourceFile_substring_lines_dict, this.Hidden_userInterface_dataSummaries);

            List<DatasetSummary_line_class> combined_dataset_summary_lines = new List<DatasetSummary_line_class>();
            string[] names;
            string name;
            int names_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            string[] sourceFiles;
            string sourceFile;
            int sourceFiles_length;
            string[] substrings;
            string substring;
            int substrings_length;

            names = name_timepointInDays_entryType_sourceFile_substring_lines_dict.Keys.ToArray();
            names_length = names.Length;
            for (int indexN = 0; indexN < names_length; indexN++)
            {
                name = names[indexN];
                timepointInDays_entryType_sourceFile_substring_lines_dict = name_timepointInDays_entryType_sourceFile_substring_lines_dict[name];
                timepointsInDays = timepointInDays_entryType_sourceFile_substring_lines_dict.Keys.ToArray();
                timepointsInDays_length = timepointsInDays.Length;
                for (int indexTimepoint = 0; indexTimepoint < timepointsInDays_length; indexTimepoint++)
                {
                    timepointInDay = timepointsInDays[indexTimepoint];
                    entryType_sourceFile_substring_lines_dict = timepointInDays_entryType_sourceFile_substring_lines_dict[timepointInDay];
                    entryTypes = entryType_sourceFile_substring_lines_dict.Keys.ToArray();
                    entryTypes_length = entryTypes.Length;
                    for (int indexEntry = 0; indexEntry < entryTypes_length; indexEntry++)
                    {
                        entryType = entryTypes[indexEntry];
                        sourceFile_substring_lines_dict = entryType_sourceFile_substring_lines_dict[entryType];
                        sourceFiles = sourceFile_substring_lines_dict.Keys.ToArray();
                        sourceFiles_length = sourceFiles.Length;
                        for (int indexSource = 0; indexSource < sourceFiles_length; indexSource++)
                        {
                            sourceFile = sourceFiles[indexSource];
                            substring_lines_dict = sourceFile_substring_lines_dict[sourceFile];
                            substrings = substring_lines_dict.Keys.ToArray();
                            substrings_length = substrings.Length;
                            for (int indexSubstring = 0; indexSubstring < substrings_length; indexSubstring++)
                            {
                                substring = substrings[indexSubstring];
                                combined_dataset_summary_lines.AddRange(substring_lines_dict[substring]);
                            }
                        }
                    }
                }
            }

            if (combined_dataset_summary_lines.Count != this.Hidden_userInterface_dataSummaries.Length + this.UserInterface_dataSummaries.Length)
            {
                throw new Exception();
            }
            this.UserInterface_dataSummaries = combined_dataset_summary_lines.ToArray();
            foreach (DatasetSummary_line_class summary_line in this.UserInterface_dataSummaries)
            {
                summary_line.Datasets_count = 1;
            }
            this.Hidden_userInterface_dataSummaries = new DatasetSummary_line_class[0];
        }

        private void Delete_filter_attributes()
        {
            this.Dataset_attributes_defining_filter_mode = new Dataset_attributes_enum[0];
        }

        private void Remove_filter_mode_if_in_filter_mode_silent()
        {
            if (Is_filter_mode)
            {
                Copy_current_userInferface_lines_into_related_userInterfaceDatasetSummaries_and_check_for_dataset_incompatibilities();
                Update_hiddenUserInterfaceDataSummaries_based_on_userInterfaceDataSummaries();
                Add_hiddenUserInterfaceDataSummaries_back_to_userInterfaceDataSummaries();
                Delete_filter_attributes();
                Set_readOnly_of_all_dataset_defining_attributes_and_deleteCheckBox(false);
                Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            }
        }

        public void Remove_filter_mode_if_in_filter_mode()
        {
            if (Is_filter_mode)
            {
                Dataset_attributes_enum[] mandatory_attributes = Overlap_class.Get_part_of_list1_but_not_of_list2<Dataset_attributes_enum>(this.Dataset_attributes_defining_filter_mode, this.Dataset_attributes_only_allowed_in_filter_mode);

                Remove_filter_mode_if_in_filter_mode_silent();
                Set_attributes_with_visible_panel_if_space_and_return_final_selection(this.Dataset_attributes_with_visible_panels, mandatory_attributes);
                Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
                this.UserInterface_dataSummaries = Set_dataset_compatibilities(this.UserInterface_dataSummaries);
                Copy_userInterfaceDataSummaries_into_userInterfaceLines();
                Update_datasetCompatibilityDelete_labels_and_description_in_graphical_interface(this.UserInterface_dataSummaries);
                Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            }
        }
        #endregion

        #region Automatic changes
        public void Automatically_set_colors_of_different_userInterfaceDatasetSummaries_to_different_colors()
        {
            int userInterfaceSummaries_length = this.UserInterface_dataSummaries.Length;
            Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
            DatasetSummary_line_class dataSummary_line;
            int indexColor = -1;
            for (int indexUI = 0; indexUI < userInterfaceSummaries_length; indexUI++)
            {
                dataSummary_line = this.UserInterface_dataSummaries[indexUI];
                indexColor++;
                if (indexColor == selectable_colors.Length) { indexColor = 0; }
                dataSummary_line.SampleColor = selectable_colors[indexColor];
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }

        public void Automatically_set_integrationGroups_of_different_userInterfaceDatasetSummaries_to_different_integrationGroups()
        {
            int userInterfaceSummaries_length = this.UserInterface_dataSummaries.Length;
            string automaticIntegrationGroup_baseName = Default_textBox_texts.IntegrationGroup_automatic_baseName;
            DatasetSummary_line_class dataSummary_line;
            StringBuilder sb = new StringBuilder();
            bool name_among_dataset_defining_attributes = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Name);
            bool timepointInDays_among_dataset_defining_attributes = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Timepoint);
            bool entryType_among_dataset_defining_attributes = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.EntryType);
            bool substring_among_dataset_defining_attributes = Dataset_attributes_defining_filter_mode.Contains(Dataset_attributes_enum.Substring);
            bool noIntegrationGroups_specified = Dataset_attributes_defining_filter_mode.Length == 0;
            for (int indexUI = 0; indexUI < userInterfaceSummaries_length; indexUI++)
            {
                dataSummary_line = this.UserInterface_dataSummaries[indexUI];
                sb.Clear();
                if ((noIntegrationGroups_specified) || (name_among_dataset_defining_attributes)) 
                { 
                    sb.AppendFormat(dataSummary_line.SampleName); 
                }
                if (substring_among_dataset_defining_attributes)
                {
                    if (sb.Length > 0) { sb.AppendFormat(" - "); }
                    sb.AppendFormat(dataSummary_line.Substring.ToString());
                }
                if ((noIntegrationGroups_specified) || (entryType_among_dataset_defining_attributes))
                {
                    if (sb.Length > 0) { sb.AppendFormat(" - "); }
                    sb.AppendFormat(dataSummary_line.EntryType.ToString());
                }
                if ((noIntegrationGroups_specified) || (timepointInDays_among_dataset_defining_attributes))
                {
                    if (sb.Length > 0) { sb.AppendFormat(" - "); }
                    sb.AppendFormat(dataSummary_line.Timepoint + " " + dataSummary_line.Timeunit); 
                }
                dataSummary_line.IntegrationGroup = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(sb.ToString());
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }

        public void Automatically_set_resultOrders_within_integrationGroups_following_current_order_of_userInterfaceDatasetSummaries()
        {
            int userInterfaceSummaries_length = this.UserInterface_dataSummaries.Length;
            string automaticIntegrationGroup_baseName = Default_textBox_texts.IntegrationGroup_automatic_baseName;
            DatasetSummary_line_class dataSummary_line;
            Dictionary<string, int> integrationGroup_currentResultsNo_dict = new Dictionary<string, int>();
            for (int indexUI = 0; indexUI < userInterfaceSummaries_length; indexUI++)
            {
                dataSummary_line = this.UserInterface_dataSummaries[indexUI];
                if (!integrationGroup_currentResultsNo_dict.ContainsKey(dataSummary_line.IntegrationGroup))
                {
                    integrationGroup_currentResultsNo_dict.Add(dataSummary_line.IntegrationGroup, 0);
                }
                integrationGroup_currentResultsNo_dict[dataSummary_line.IntegrationGroup]++;
                dataSummary_line.Results_no = integrationGroup_currentResultsNo_dict[dataSummary_line.IntegrationGroup];
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        
        private int Get_max_resultsNumber_in_userInterface_summaries()
        {
            int max_order_no = -1;
            foreach (DatasetSummary_line_class datasetSummary in this.UserInterface_dataSummaries)
            {
                if (datasetSummary.Results_no>max_order_no)
                {
                    max_order_no = datasetSummary.Results_no;
                }
            }
            return max_order_no;
        }

        public void Add_or_remove_fileName_to_or_from_dataset_name_in_userInterface_dataSummaries_and_update_userInterfaceLines(Add_fileName_instructions_enum addFileName_instruction, string selectedItem)
        {
            ClassLibrary1.OrganizeData_userInterface.OrganizeData_text_strings_class textStrings = new OrganizeData_userInterface.OrganizeData_text_strings_class();
            int userInterface_dataSummaries = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class userInterface_data_summary_line;
            string fileName_without_extension;
            string datasetOrderString;
            string addItem;

            int indexHyphen_after_insetItem;
            int indexHyphen_before_insetItem;
            int startIndexSourceInFileName;
            int endIndexSourceInFileName;
            int startIndexOrderNoInFileName = -1;
            int endIndexOrderNoInFileName = -1;
            int startIndexItemInFileName;
            int endIndexItemInFileName;
            int indexInsetItem;
            int indexSpaceAfterNumberSign;
            int nchar_of_potential_number_string;
            int nchar_of_max_resultsNo = Get_max_resultsNumber_in_userInterface_summaries().ToString().Length;
            string potential_number_substring;
            int existingDatasetNo;
            int sampleName_nchar;
            StringBuilder new_sampleName_sb = new StringBuilder();
            string new_sampleName;
            for (int indexD = 0; indexD < userInterface_dataSummaries; indexD++)
            {
                userInterface_data_summary_line = this.UserInterface_dataSummaries[indexD];
                sampleName_nchar = userInterface_data_summary_line.SampleName.Length;

                #region Get start and end indexes of source
                fileName_without_extension = System.IO.Path.GetFileNameWithoutExtension(userInterface_data_summary_line.Source_fileName);
                indexInsetItem = userInterface_data_summary_line.SampleName.IndexOf(fileName_without_extension);
                if (indexInsetItem != -1)
                {
                    indexHyphen_before_insetItem = userInterface_data_summary_line.SampleName.IndexOf(" - " + fileName_without_extension);
                    indexHyphen_after_insetItem = userInterface_data_summary_line.SampleName.IndexOf(" - ", indexInsetItem);
                    if (indexHyphen_before_insetItem != -1) { startIndexSourceInFileName = indexHyphen_before_insetItem; }
                    else { startIndexSourceInFileName = indexInsetItem; }
                    if (indexHyphen_after_insetItem != -1) { endIndexSourceInFileName = indexHyphen_after_insetItem + 2; }
                    else { endIndexSourceInFileName = sampleName_nchar - 1; }
                }
                else
                {
                    startIndexSourceInFileName = -1;
                    endIndexSourceInFileName = -1;
                }
                #endregion

                #region Get start and end indexes of dataset order numbers
                datasetOrderString = userInterface_data_summary_line.Results_no.ToString();
                while (datasetOrderString.Length != nchar_of_max_resultsNo)
                {
                    datasetOrderString = "0" + datasetOrderString;
                }
                datasetOrderString = "#" + datasetOrderString;
                indexInsetItem = userInterface_data_summary_line.SampleName.IndexOf("#");
                if (indexInsetItem != -1)
                {
                    indexSpaceAfterNumberSign = userInterface_data_summary_line.SampleName.IndexOf(" ", indexInsetItem);
                    if (indexSpaceAfterNumberSign == -1)
                    {
                        nchar_of_potential_number_string = sampleName_nchar - indexInsetItem - 1;
                    }
                    else
                    {
                        nchar_of_potential_number_string = indexSpaceAfterNumberSign - indexInsetItem - 1;
                    }
                    potential_number_substring = userInterface_data_summary_line.SampleName.Substring(indexInsetItem + 1, nchar_of_potential_number_string);
                    if (int.TryParse(potential_number_substring, out existingDatasetNo))
                    {
                        indexHyphen_before_insetItem = userInterface_data_summary_line.SampleName.IndexOf(" - #");
                        indexHyphen_after_insetItem = userInterface_data_summary_line.SampleName.IndexOf(" - ", indexInsetItem);
                        if (indexHyphen_before_insetItem != -1) { startIndexOrderNoInFileName = indexHyphen_before_insetItem; }
                        else { startIndexOrderNoInFileName = indexInsetItem; }
                        if (indexHyphen_after_insetItem != -1) { endIndexOrderNoInFileName = indexHyphen_after_insetItem + 2; }
                        else { endIndexOrderNoInFileName = sampleName_nchar - 1; }
                    }
                }
                else
                {
                    startIndexOrderNoInFileName = -1;
                    endIndexOrderNoInFileName = -1;
                }

                #endregion

                #region Set start and end indexes of item
                if (selectedItem.Equals(textStrings.AddFileNames_string))
                {
                    if (  (endIndexOrderNoInFileName >= startIndexSourceInFileName)
                        && (endIndexOrderNoInFileName <= endIndexSourceInFileName)
                        && (endIndexOrderNoInFileName != -1))
                    { startIndexItemInFileName = Math.Max(endIndexOrderNoInFileName + 1, startIndexSourceInFileName); }
                    else { startIndexItemInFileName = startIndexSourceInFileName; }
                    if (   (startIndexOrderNoInFileName >= startIndexSourceInFileName)
                        && (startIndexOrderNoInFileName <= endIndexSourceInFileName)
                        && (startIndexOrderNoInFileName != -1))
                    { endIndexItemInFileName = Math.Min(startIndexOrderNoInFileName - 1, endIndexSourceInFileName); }
                    else { endIndexItemInFileName = endIndexSourceInFileName; }
                    addItem = (string)fileName_without_extension.Clone();
                }
                else if (selectedItem.Equals(textStrings.AddDatasetOrder_string))
                {
                    if (   (endIndexSourceInFileName >= startIndexOrderNoInFileName)
                        && (endIndexSourceInFileName <= endIndexOrderNoInFileName)
                        && (endIndexSourceInFileName != -1))
                    { startIndexItemInFileName = Math.Max(endIndexSourceInFileName + 1, startIndexOrderNoInFileName); }
                    else { startIndexItemInFileName = startIndexOrderNoInFileName; }
                    if (   (startIndexSourceInFileName >= startIndexOrderNoInFileName)
                        && (startIndexSourceInFileName <= endIndexSourceInFileName)
                        && (startIndexSourceInFileName != -1))
                    { endIndexItemInFileName = Math.Min(startIndexSourceInFileName - 1, endIndexOrderNoInFileName); }
                    else { endIndexItemInFileName = endIndexOrderNoInFileName; }
                    addItem = (string)datasetOrderString.Clone();
                }
                else { throw new Exception(); }
                #endregion

                new_sampleName_sb.Clear();
                if (startIndexItemInFileName != -1)
                {
                    if ((startIndexItemInFileName > -1)&&(startIndexItemInFileName!=0))
                    {
                        new_sampleName_sb.AppendFormat(userInterface_data_summary_line.SampleName.Substring(0, startIndexItemInFileName));
                    }
                    if ((endIndexItemInFileName>-1)&&(endIndexItemInFileName!=sampleName_nchar-1))
                    {
                        new_sampleName_sb.AppendFormat(userInterface_data_summary_line.SampleName.Substring(endIndexItemInFileName+1, sampleName_nchar - endIndexItemInFileName-1));
                    }
                }
                else
                {
                    new_sampleName_sb.AppendFormat(userInterface_data_summary_line.SampleName);
                }
                new_sampleName = new_sampleName_sb.ToString();
                new_sampleName = Text_class.Remove_given_substring_from_beginning_and_end_of_text_if_exists(new_sampleName, " - ");

                if (addFileName_instruction.Equals(Add_fileName_instructions_enum.Before_dataset_name))
                {
                    userInterface_data_summary_line.SampleName = addItem + " - " + new_sampleName;
                }
                else if (addFileName_instruction.Equals(Add_fileName_instructions_enum.After_dataset_name))
                {
                    userInterface_data_summary_line.SampleName = new_sampleName + " - " + addItem;
                }
                else if (addFileName_instruction.Equals(Add_fileName_instructions_enum.Remove))
                {
                    userInterface_data_summary_line.SampleName = new_sampleName;
                }
                else { throw new Exception(); }
            }
            this.UserInterface_dataSummaries = Set_dataset_compatibilities(this.UserInterface_dataSummaries);
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }

        #endregion

        #region Dataset compatibilities
        private DatasetSummary_line_class[] Set_dataset_compatibilities(DatasetSummary_line_class[] datasetSummaries)
        {
            DatasetSummary_line_class this_datasetSummary_line;
            DatasetSummary_line_class firstDuplicated_datasetSummary_line;
            int datasetSummaries_length = datasetSummaries.Length;

            #region Set all compatibilities to default, i.e. ok
            for (int indexDS = 0; indexDS < datasetSummaries_length; indexDS++)
            {
                this_datasetSummary_line = datasetSummaries[indexDS];
                this_datasetSummary_line.Dataset_compatibility = Dataset_compatibility_enum.Ok;
                this_datasetSummary_line.Row_contains_invalid_value = false;
            }
            #endregion

            #region Label duplicated datasets as duplicated and all other as ok
            Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>>> integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict = new Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>>>();

            int firstIndexDuplicatedDataset;
            for (int indexDS = 0; indexDS < datasetSummaries_length; indexDS++)
            {
                this_datasetSummary_line = datasetSummaries[indexDS];
                if (!integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict.ContainsKey(this_datasetSummary_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict.Add(this_datasetSummary_line.IntegrationGroup, new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup].ContainsKey(this_datasetSummary_line.SampleName))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup].Add(this_datasetSummary_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, int>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup][this_datasetSummary_line.SampleName].ContainsKey(this_datasetSummary_line.Timepoint_in_days))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup][this_datasetSummary_line.SampleName].Add(this_datasetSummary_line.Timepoint_in_days, new Dictionary<Entry_type_enum, int>());
                }
                if (!integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup][this_datasetSummary_line.SampleName][this_datasetSummary_line.Timepoint_in_days].ContainsKey(this_datasetSummary_line.EntryType))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup][this_datasetSummary_line.SampleName][this_datasetSummary_line.Timepoint_in_days].Add(this_datasetSummary_line.EntryType, indexDS);
                }
                else
                {
                    this_datasetSummary_line.Dataset_compatibility = Dataset_compatibility_enum.Duplicated_dataset;
                    firstIndexDuplicatedDataset = integrationGroup_sampleName_timepointInDays_entryType_firstIndex_dict[this_datasetSummary_line.IntegrationGroup][this_datasetSummary_line.SampleName][this_datasetSummary_line.Timepoint_in_days][this_datasetSummary_line.EntryType];
                    firstDuplicated_datasetSummary_line = datasetSummaries[firstIndexDuplicatedDataset];
                    firstDuplicated_datasetSummary_line.Dataset_compatibility = Dataset_compatibility_enum.Duplicated_dataset;
                }
            }
            #endregion

            return datasetSummaries;
        }
        #endregion

        #region Results numbers
        private void Update_result_numbers_in_userInterface_dataSummaries_if_no_hidden_userInterface_dataSummaries()
        {
            if (this.Hidden_userInterface_dataSummaries.Length>0) { throw new Exception(); }
            int userInterface_dataSummaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class userInterface_dataSummaries_line;
            for (int indexUI = 0; indexUI < userInterface_dataSummaries_length; indexUI++)
            {
                userInterface_dataSummaries_line = this.UserInterface_dataSummaries[indexUI];
                userInterface_dataSummaries_line.Current_temporary_order = indexUI;
            }
            this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l=>l.IntegrationGroup).ThenBy(l => l.Results_no).ThenBy(l=>l.SampleName).ThenBy(l=>l.Timepoint_in_days).ThenBy(l=>l.EntryType).ToArray();
            Dictionary<string, int> integrationGroup_currentResultsNo_dict = new Dictionary<string, int>();
            for (int indexUI=0; indexUI<userInterface_dataSummaries_length;indexUI++)
            {
                userInterface_dataSummaries_line = this.UserInterface_dataSummaries[indexUI];
                if (!integrationGroup_currentResultsNo_dict.ContainsKey(userInterface_dataSummaries_line.IntegrationGroup))
                {
                    integrationGroup_currentResultsNo_dict.Add(userInterface_dataSummaries_line.IntegrationGroup, 0);
                }
                integrationGroup_currentResultsNo_dict[userInterface_dataSummaries_line.IntegrationGroup]++;
                userInterface_dataSummaries_line.Results_no = integrationGroup_currentResultsNo_dict[userInterface_dataSummaries_line.IntegrationGroup];
            }
            this.UserInterface_dataSummaries = this.UserInterface_dataSummaries.OrderBy(l => l.Current_temporary_order).ToArray();
        }
        #endregion

        #region Data transfer between custom data, userInterface and lastEntries
        private DatasetSummary_line_class[] Update_default_integrationGroups(DatasetSummary_line_class[] dataSummary_lines)
        {
            string default_integrationGroup = Get_default_integrationGroup(dataSummary_lines);
            foreach (DatasetSummary_line_class dataSummary_line in dataSummary_lines)
            {
                if (Default_textBox_texts.IntegrationGroup_default_Groups.Contains(dataSummary_line.IntegrationGroup))
                {
                    dataSummary_line.IntegrationGroup = (string)default_integrationGroup.Clone();
                }
            }
            return dataSummary_lines;
        }

        private void Copy_userInterfaceDataSummaries_into_userInterfaceLines()
        {
            int maximum_first_shown = Get_maximum_value_for_first_shown_userInterface_dataSummaries_index();
            if (First_shown_userInterface_dataSummaries_index > maximum_first_shown)
            {
                Set_first_shown_userInterface_dataSummaries_index_to_maximum_value();
            }
            DatasetSummary_line_class userInterface_dataSummary_line;
            DatasetSummary_userInterface_line_class current_userInterface_line;
            for (int indexIndex = 0; indexIndex < Last_filled_userInterfaceLines_index + 1; indexIndex++)
            {
                userInterface_dataSummary_line = UserInterface_dataSummaries[First_shown_userInterface_dataSummaries_index + indexIndex];
                current_userInterface_line = this.UserInterface_lines[indexIndex];
                current_userInterface_line.Set_dataset_from_datasetSummary_line(userInterface_dataSummary_line, this.Available_background_gene_list_names);
            }
            LastSavedDataSummaries_identical_with_userInterface = Are_lastSavedDataSummaries_equalTo_userInferfaceDataSummaries_and_hiddenUserInterfaceDataSummaries_ignoring_order();
        }

        private void Copy_lastSafedDataSummaries_into_userInferfaceDataSummaries()
        {
            DatasetSummary_line_class lastSaved_dataSummary;
            int lastSaved_dataSummaries_length = LastSaved_datasetSummaries.Length;
            this.UserInterface_dataSummaries = new DatasetSummary_line_class[lastSaved_dataSummaries_length];
            for (int indexLE = 0; indexLE < lastSaved_dataSummaries_length; indexLE++)
            {
                lastSaved_dataSummary = LastSaved_datasetSummaries[indexLE];
                this.UserInterface_dataSummaries[indexLE] = lastSaved_dataSummary.Deep_copy();
            }
        }

        private void Copy_custom_data_into_lastSaved_dataSummaries_and_available_background_gene_list_names_and_preserve_substrings(Custom_data_class custom_data)
        {
            Dictionary<string, string> uniqueFixed_datasetIdentifier_dict = new Dictionary<string, string>();
            int lastSaved_datasetSummaries_length = this.LastSaved_datasetSummaries.Length;
            DatasetSummary_line_class dataset_summary_line;
            for (int indexDS=0; indexDS<lastSaved_datasetSummaries_length;indexDS++)
            {
                dataset_summary_line = this.LastSaved_datasetSummaries[indexDS];
                uniqueFixed_datasetIdentifier_dict.Add(dataset_summary_line.Unique_fixed_dataset_identifier, dataset_summary_line.Substring);
            }

            int custom_dataset_length = custom_data.Custom_data.Length;
            custom_data.Order_by_uniqueFixedDatasetIdentifier();
            Custom_data_line_class custom_data_line;
            DatasetSummary_line_class lastAdded_entry;
            List<DatasetSummary_line_class> LastSaved_userEntries_list = new List<DatasetSummary_line_class>();
            int current_genes_count = 0;
            int current_sig_genes_count = 0;
            for (int indexCustom = 0; indexCustom < custom_dataset_length; indexCustom++)
            {
                custom_data_line = custom_data.Custom_data[indexCustom];
                if (  (indexCustom==0)
                    ||(!custom_data_line.Unique_fixed_dataset_identifier.Equals(custom_data.Custom_data[indexCustom-1].Unique_fixed_dataset_identifier)))
                {
                    current_genes_count = 0;
                    current_sig_genes_count = 0;
                }
                current_genes_count++;
                if (custom_data_line.Significance_status.Equals(Significance_status_enum.Yes)) 
                { 
                    current_sig_genes_count++; 
                }
                if ((indexCustom == custom_dataset_length-1)
                    || (!custom_data_line.Unique_fixed_dataset_identifier.Equals(custom_data.Custom_data[indexCustom + 1].Unique_fixed_dataset_identifier)))
                {
                    lastAdded_entry = new DatasetSummary_line_class(custom_data_line, current_genes_count, current_sig_genes_count);
                    if (uniqueFixed_datasetIdentifier_dict.ContainsKey(lastAdded_entry.Unique_fixed_dataset_identifier))
                    {
                        lastAdded_entry.Substring = (string)uniqueFixed_datasetIdentifier_dict[lastAdded_entry.Unique_fixed_dataset_identifier].Clone();
                    }
                    LastSaved_userEntries_list.Add(lastAdded_entry);
                }
            }
            LastSaved_datasetSummaries = LastSaved_userEntries_list.ToArray();
            this.Available_background_gene_list_names = custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray();
        }

        private void Copy_current_userInferface_lines_into_related_userInterfaceDatasetSummaries_and_check_for_dataset_incompatibilities()
        {
            DatasetSummary_line_class old_userInterface_datasetSummary_line;
            DatasetSummary_line_class new_userInterface_datasetSummary_line;
            //this.UserInterface_lines = UserInterface_lines.OrderBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            //this.UserInterface_dataSummaries = UserInterface_dataSummaries.OrderBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            int userInferface_summaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_userInterface_line_class userInterface_line;
            int noCompare = -2;
            int indexOld = 0;
            List<DatasetSummary_line_class> add_userInterface_lines = new List<DatasetSummary_line_class>();
            for (int indexUI = 0; indexUI <= Last_filled_userInterfaceLines_index; indexUI++)
            {
                userInterface_line = this.UserInterface_lines[indexUI];
                new_userInterface_datasetSummary_line = new DatasetSummary_line_class(userInterface_line, Form_default_settings);
                noCompare = -2;
                while ((noCompare != 0) && (indexOld < userInferface_summaries_length))
                {
                    old_userInterface_datasetSummary_line = this.UserInterface_dataSummaries[indexOld];
                    noCompare = old_userInterface_datasetSummary_line.Unique_fixed_dataset_identifier.CompareTo(userInterface_line.Unique_fixed_dataset_identifier);
                    if (noCompare != 0) { indexOld++; }
                }
                if ((noCompare != 0)) { throw new Exception(); }//
                else if (noCompare == 0)
                {
                    this.UserInterface_dataSummaries[indexOld] = new_userInterface_datasetSummary_line.Deep_copy();
                }
                else
                {
                    add_userInterface_lines.Add(new_userInterface_datasetSummary_line);
                }
            }
            Add_to_userInterface_datasetSummaries(add_userInterface_lines.ToArray());
            this.UserInterface_dataSummaries = Update_default_integrationGroups(this.UserInterface_dataSummaries);
            this.UserInterface_dataSummaries = Set_dataset_compatibilities(this.UserInterface_dataSummaries);

            #region Add dataset compatibilities to userInterface_lines
            DatasetSummary_line_class reference_datasetSummary_line;
            int indexRef = 0;
            for (int indexUI = 0; indexUI <= Last_filled_userInterfaceLines_index; indexUI++)
            {
                userInterface_line = this.UserInterface_lines[indexUI];
                noCompare = -2;
                while ((noCompare != 0) && (indexOld < userInferface_summaries_length))
                {
                    reference_datasetSummary_line = this.UserInterface_dataSummaries[indexRef];
                    noCompare = reference_datasetSummary_line.Unique_fixed_dataset_identifier.CompareTo(userInterface_line.Unique_fixed_dataset_identifier);
                    if (noCompare != 0) { indexRef++; }
                    else if (noCompare == 0)
                    {
                        userInterface_line.Dataset_compatibility = reference_datasetSummary_line.Dataset_compatibility;
                        userInterface_line.Row_contains_at_least_one_invalid_value = reference_datasetSummary_line.Row_contains_invalid_value;
                    }
                }
            }
            #endregion
        }

        private Custom_data_class Copy_compatible_userInterfaceDataSummaries_into_customData_and_lastSavedDataSummaries_and_preserve_substrings(Custom_data_class custom_data)
        {
            #region Map not duplicated userInterfaceDataSummaries to lastSavedDataSummaries
            int userInterfaceDataSummaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class new_dataSummary_line;
            Dictionary<string, DatasetSummary_line_class> uniqueFixedDatasetIdentifies_new_dict = new Dictionary<string, DatasetSummary_line_class>();
            Dictionary<string, bool> uniqueFixedDatasetIdentifies_delete_dict = new Dictionary<string, bool>();
            for (int indexUIDS = 0; indexUIDS < userInterfaceDataSummaries_length; indexUIDS++)
            {
                new_dataSummary_line = UserInterface_dataSummaries[indexUIDS];
                if (new_dataSummary_line.Delete) { throw new Exception(); }
                if (new_dataSummary_line.Dataset_compatibility.Equals(Dataset_compatibility_enum.Ok))
                {
                    uniqueFixedDatasetIdentifies_new_dict.Add(new_dataSummary_line.Unique_fixed_dataset_identifier, new_dataSummary_line);
                }
            }
            #endregion

            #region Update custom data dataset names, timepoints, entryTypes and colors
            int custom_data_length = custom_data.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            List<Custom_data_line_class> keep = new List<Custom_data_line_class>();
            for (int indexCustom = 0; indexCustom < custom_data_length; indexCustom++)
            {
                custom_data_line = custom_data.Custom_data[indexCustom];
                if (uniqueFixedDatasetIdentifies_new_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                {
                    new_dataSummary_line = uniqueFixedDatasetIdentifies_new_dict[custom_data_line.Unique_fixed_dataset_identifier];
                    if (new_dataSummary_line.Delete) { throw new Exception(); }
                    if (!new_dataSummary_line.Unique_fixed_dataset_identifier.Equals(custom_data_line.Unique_fixed_dataset_identifier)) { throw new Exception(); }
                    custom_data_line.SampleName = (string)new_dataSummary_line.SampleName.Clone();
                    custom_data_line.Timepoint = new_dataSummary_line.Timepoint;
                    custom_data_line.Timeunit = new_dataSummary_line.Timeunit;
                    custom_data_line.EntryType = new_dataSummary_line.EntryType;
                    custom_data_line.IntegrationGroup = (string)new_dataSummary_line.IntegrationGroup.Clone();
                    custom_data_line.SampleColor = new_dataSummary_line.SampleColor;
                    custom_data_line.BgGenes_listName = (string)new_dataSummary_line.Background_geneListName.Clone();
                    custom_data_line.Source_fileName = (string)new_dataSummary_line.Source_fileName.Clone();
                    custom_data_line.Results_number = new_dataSummary_line.Results_no;
                }
            }
            #endregion

            Copy_custom_data_into_lastSaved_dataSummaries_and_available_background_gene_list_names_and_preserve_substrings(custom_data);

            return custom_data;
        }

        public void Copy_custom_data_into_all_interface_fields(Custom_data_class custom_data)
        {
            if (Is_filter_mode) { throw new Exception(); }
            Copy_custom_data_into_lastSaved_dataSummaries_and_available_background_gene_list_names_and_preserve_substrings(custom_data);
            this.LastSaved_datasetSummaries = Set_dataset_compatibilities(this.LastSaved_datasetSummaries);
            Copy_lastSafedDataSummaries_into_userInferfaceDataSummaries();
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Set_first_shown_userInterface_dataSummaries_index_to_maximum_value();
            Update_available_bgGenesListNames_in_userInterfaceLines(custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray());
        }

        public void Update_available_bgGenesListNames_in_userInterfaceLines(string[] new_bgGenesListNames)
        {
            if (!new_bgGenesListNames.Contains(Global_class.Mbco_exp_background_gene_list_name)) { throw new Exception(); }
            this.Available_background_gene_list_names = Array_class.Deep_copy_string_array(new_bgGenesListNames);
            int userinterface_lines_length = this.UserInterface_lines.Length;
            DatasetSummary_userInterface_line_class userinterface_line;
            for (int indexUI=0; indexUI<userinterface_lines_length;indexUI++)
            {
                userinterface_line = this.UserInterface_lines[indexUI];
                userinterface_line.Update_userInterface_bgGenes_listBox(this.Available_background_gene_list_names, Global_class.Mbco_exp_background_gene_list_name);
            }
        }
        #endregion

        #region Buttons
        private bool Are_lastSavedDataSummaries_equalTo_userInferfaceDataSummaries_and_hiddenUserInterfaceDataSummaries_ignoring_order()
        {
            int hidden_userInterface_dataSummaries_length = this.Hidden_userInterface_dataSummaries.Length;
            int userInterface_dataSummaries_length = this.UserInterface_dataSummaries.Length;
            int lastSaved_dataSummaries_length = this.LastSaved_datasetSummaries.Length;
            bool identical = true;

            if (hidden_userInterface_dataSummaries_length + userInterface_dataSummaries_length != lastSaved_dataSummaries_length) { throw new Exception(); }
            else
            {
                Dictionary<string, DatasetSummary_line_class> uniqueFixedDatasetIdentifier_userInterfaceDataSummaryLine_dict = new Dictionary<string, DatasetSummary_line_class>();
                DatasetSummary_line_class userInterface_datasetSummary_line;
                for (int indexUIDS = 0; indexUIDS < userInterface_dataSummaries_length; indexUIDS++)
                {
                    userInterface_datasetSummary_line = this.UserInterface_dataSummaries[indexUIDS];
                    uniqueFixedDatasetIdentifier_userInterfaceDataSummaryLine_dict.Add(userInterface_datasetSummary_line.Unique_fixed_dataset_identifier, userInterface_datasetSummary_line);
                }

                for (int indexHUIDS = 0; indexHUIDS < hidden_userInterface_dataSummaries_length; indexHUIDS++)
                {
                    userInterface_datasetSummary_line = this.Hidden_userInterface_dataSummaries[indexHUIDS];
                    uniqueFixedDatasetIdentifier_userInterfaceDataSummaryLine_dict.Add(userInterface_datasetSummary_line.Unique_fixed_dataset_identifier, userInterface_datasetSummary_line);
                }

                DatasetSummary_line_class lastSaved_datasetSummary_line;
                for (int indexLSDS = 0; indexLSDS < lastSaved_dataSummaries_length; indexLSDS++)
                {
                    lastSaved_datasetSummary_line = this.LastSaved_datasetSummaries[indexLSDS];
                    userInterface_datasetSummary_line = uniqueFixedDatasetIdentifier_userInterfaceDataSummaryLine_dict[lastSaved_datasetSummary_line.Unique_fixed_dataset_identifier];
                    if (!lastSaved_datasetSummary_line.Equals_other(userInterface_datasetSummary_line))
                    {
                        identical = false;
                    }
                }
            }
            return identical;
        }

        public void Changes_reset_button_click()
        {
            Dataset_attributes_enum[] attributes_of_filter_mode = Array_class.Deep_copy_array(this.Dataset_attributes_defining_filter_mode);
            if (Is_filter_mode)
            {
                Update_hiddenUserInterfaceDataSummaries_based_on_userInterfaceDataSummaries();
                Add_hiddenUserInterfaceDataSummaries_back_to_userInterfaceDataSummaries();
            }
            Copy_lastSafedDataSummaries_into_userInferfaceDataSummaries();
            if (Is_filter_mode)
            {
                Move_duplicated_userInterfaceSummaries_to_hidden_summaries_and_count_how_many_datasets_per_main_summary_line();
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }

        private Custom_data_class Delete_datasets_marked_for_deletion_in_custom_data_and_dataSummaries(Custom_data_class custom_data)
        {
            Dictionary<string, bool> uniqueFixedDatasetIdentifier_delete_dict = new Dictionary<string, bool>();
            Dictionary<string, bool> uniqueFixedDatasetIdentifier_keep_dict = new Dictionary<string, bool>();

            #region Fill uniqueFixedDatasetIdentifier_delete_dict and remove userInterface_dataSummaries marked for delition
            int userInterfaceDataSummaries_length = this.UserInterface_dataSummaries.Length;
            DatasetSummary_line_class new_dataSummary_line;
            List<DatasetSummary_line_class> keep_interfaceDataSummaries = new List<DatasetSummary_line_class>();
            for (int indexUIDS = 0; indexUIDS < userInterfaceDataSummaries_length; indexUIDS++)
            {
                new_dataSummary_line = UserInterface_dataSummaries[indexUIDS];
                if (new_dataSummary_line.Delete)
                {
                    uniqueFixedDatasetIdentifier_delete_dict.Add(new_dataSummary_line.Unique_fixed_dataset_identifier,true);
                }
                else
                {
                    uniqueFixedDatasetIdentifier_keep_dict.Add(new_dataSummary_line.Unique_fixed_dataset_identifier, true);
                    keep_interfaceDataSummaries.Add(new_dataSummary_line);
                }
            }
            this.UserInterface_dataSummaries = keep_interfaceDataSummaries.ToArray();
            #endregion

            custom_data.Delete_indicated_datasets(uniqueFixedDatasetIdentifier_delete_dict);
            Copy_custom_data_into_lastSaved_dataSummaries_and_available_background_gene_list_names_and_preserve_substrings(custom_data);

            #region Check if unique fixed dataset identifiers are thes same in lastSaved_dataSummary as in userInterface_dataSummaries
            int lastSaved_datasetSummaries_length = this.LastSaved_datasetSummaries.Length;
            DatasetSummary_line_class lastSaved_dataSummary_line;
            List<DatasetSummary_line_class> keep_lastSaved_dataSummaries = new List<DatasetSummary_line_class>();
            for (int indexLSDS = 0; indexLSDS < lastSaved_datasetSummaries_length; indexLSDS++)
            {
                lastSaved_dataSummary_line = LastSaved_datasetSummaries[indexLSDS];
                if (uniqueFixedDatasetIdentifier_delete_dict.ContainsKey(lastSaved_dataSummary_line.Unique_fixed_dataset_identifier))
                {
                    throw new Exception();
                }
                else if (uniqueFixedDatasetIdentifier_keep_dict.ContainsKey(lastSaved_dataSummary_line.Unique_fixed_dataset_identifier))
                {
                }
                else { throw new Exception(); }
            }
            #endregion

            return custom_data;
        }

        public Custom_data_class Changes_update_button_click(Custom_data_class custom_data)
        {

            Copy_current_userInferface_lines_into_related_userInterfaceDatasetSummaries_and_check_for_dataset_incompatibilities();
            if (Is_filter_mode)
            {
                Set_readOnly_of_all_dataset_defining_attributes_and_deleteCheckBox(false);
                Update_hiddenUserInterfaceDataSummaries_based_on_userInterfaceDataSummaries();
                Add_hiddenUserInterfaceDataSummaries_back_to_userInterfaceDataSummaries();
            }
            custom_data = Delete_datasets_marked_for_deletion_in_custom_data_and_dataSummaries(custom_data);
            Update_result_numbers_in_userInterface_dataSummaries_if_no_hidden_userInterface_dataSummaries();
            this.UserInterface_dataSummaries = Update_default_integrationGroups(this.UserInterface_dataSummaries);
            this.UserInterface_dataSummaries = Set_dataset_compatibilities(this.UserInterface_dataSummaries);
            custom_data = Copy_compatible_userInterfaceDataSummaries_into_customData_and_lastSavedDataSummaries_and_preserve_substrings(custom_data);
            custom_data.Check_for_correctness();
            custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
            if (Is_filter_mode)
            {
                Move_duplicated_userInterfaceSummaries_to_hidden_summaries_and_count_how_many_datasets_per_main_summary_line();
                Set_readOnly_of_all_dataset_defining_attributes_and_deleteCheckBox(true);
            }
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            Reset_empty_interface_lines_to_default(Next_userInterfaceLine_index);
            custom_data.Check_for_correctness();
            return custom_data;
        }

        public Custom_data_class AddNewDataset_button_click_and_return_success(Custom_data_class custom_data, string[] inputGenes, out bool successful)
        {
            DatasetSummary_line_class userInterface_add_dataset = new DatasetSummary_line_class(this.UserInterface_lines[Next_userInterfaceLine_index], Form_default_settings);
            if (userInterface_add_dataset.Delete)
            {
                throw new Exception();
            }
            Custom_data_line_class new_custom_data_line;
            inputGenes = inputGenes.Distinct().ToArray();
            int inputGenes_length = inputGenes.Length;
            string inputGene;
            List<Custom_data_line_class> new_custom_data_lines = new List<Custom_data_line_class>();
            int genes_added = 0;
            string unique_fixed_dataset_identifier = custom_data.Get_available_uniqueFixedDatasetIdentifier_for_manual_addition();
            successful = false;
            for (int indexGene = 0; indexGene < inputGenes_length; indexGene++)
            {
                inputGene = inputGenes[indexGene].Replace("\r\n", "");
                if (!String.IsNullOrEmpty(inputGene))
                {
                    new_custom_data_line = new Custom_data_line_class();
                    new_custom_data_line.EntryType = Entry_type_enum.Up;
                    new_custom_data_line.Timepoint = -1;
                    new_custom_data_line.Value_1st = 1;
                    new_custom_data_line.Value_2nd = 0;
                    new_custom_data_line.Significance_status = Significance_status_enum.Yes;
                    new_custom_data_line.SampleName = (string)userInterface_add_dataset.SampleName.Clone();
                    new_custom_data_line.Timepoint = userInterface_add_dataset.Timepoint;
                    new_custom_data_line.Timeunit = userInterface_add_dataset.Timeunit;
                    new_custom_data_line.Timeunit_string = new_custom_data_line.Timeunit.ToString();
                    new_custom_data_line.EntryType = userInterface_add_dataset.EntryType;
                    new_custom_data_line.NCBI_official_symbol = (string)inputGene.ToUpper().Clone();
                    new_custom_data_line.SampleColor = userInterface_add_dataset.SampleColor;
                    new_custom_data_line.BgGenes_listName = (string)Global_class.Mbco_exp_background_gene_list_name.Clone();
                    new_custom_data_line.Source_fileName = "Manually added";
                    new_custom_data_line.IntegrationGroup = (string)userInterface_add_dataset.IntegrationGroup.Clone();
                    new_custom_data_line.Unique_fixed_dataset_identifier = (string)unique_fixed_dataset_identifier.Clone();
                    new_custom_data_lines.Add(new_custom_data_line);
                    genes_added++;
                }
            }
            if (genes_added == 0)
            {
            }
            else
            {
                custom_data.Add_to_array_and_return_error_message_if_not_possible(new_custom_data_lines.ToArray());
                Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
                custom_data.Set_missing_colors(selectable_colors);
                custom_data.Set_missing_results_numbers_and_adjust_to_consecutive_numbers_within_each_integrationGroup();


                Copy_custom_data_into_lastSaved_dataSummaries_and_available_background_gene_list_names_and_preserve_substrings(custom_data);
                Copy_lastSafedDataSummaries_into_userInferfaceDataSummaries();
                Copy_userInterfaceDataSummaries_into_userInterfaceLines();
                Set_first_shown_userInterface_dataSummaries_index_to_maximum_value();
                successful = true;
            }

            UserInterface_dataSummaries = Set_dataset_compatibilities(UserInterface_dataSummaries);
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_datasetCompatibilityDelete_labels_and_description_in_graphical_interface(UserInterface_dataSummaries);
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            return custom_data;
        }

        public void Dataset_scrollBar_Scroll()
        {
            First_shown_userInterface_dataSummaries_index = Dataset_scrollBar.Value;
            Copy_current_userInferface_lines_into_related_userInterfaceDatasetSummaries_and_check_for_dataset_incompatibilities();
            Copy_userInterfaceDataSummaries_into_userInterfaceLines();
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        #endregion

        public void Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations()
        {
            Copy_current_userInferface_lines_into_related_userInterfaceDatasetSummaries_and_check_for_dataset_incompatibilities();
            if (Is_filter_mode)
            {
                Update_hiddenUserInterfaceDataSummaries_based_on_userInterfaceDataSummaries();
                List<DatasetSummary_line_class> all_userInterface_dataSummaries = new List<DatasetSummary_line_class>();
                all_userInterface_dataSummaries.AddRange(this.UserInterface_dataSummaries);
                all_userInterface_dataSummaries.AddRange(this.Hidden_userInterface_dataSummaries);
            }
            Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
    }
}
