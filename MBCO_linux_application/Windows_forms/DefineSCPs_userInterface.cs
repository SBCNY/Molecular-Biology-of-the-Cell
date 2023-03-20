using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_forms_customized_tools;
using Network;
using Enrichment;
using Common_functions.Array_own;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using MBCO;

namespace ClassLibrary1.DefineSCPs_userInterface
{
    class DefineSCPs_userInterface_class
    {
        private MyPanel Overall_panel { get; set; }
        private Label Overall_headline_label { get; set; }
        private Label NewOwnScp_label { get; set; }
        private OwnTextBox NewOwnScp_textBox { get; set; }
        private Button Add_newOwnScp_button { get; set; }
        private Button Remove_ownScp_button { get; set; }
        private Label SelectedOwnScp_label { get; set; }
        private OwnListBox SelectedOwnScp_listBox { get; set; }
        private Label Selected_ownScp_level_label { get; set; }
        private MyPanel Selection_panel { get; set; }
        private MyCheckBox_button Selected_ownScp_level1_cbButton { get; set; }
        private Label Selected_ownScp_level1_cbLabel { get; set; }
        private MyCheckBox_button Selected_ownScp_level2_cbButton { get; set; }
        private Label Selected_ownScp_level2_cbLabel { get; set; }
        private MyCheckBox_button Selected_ownScp_level3_cbButton { get; set; }
        private Label Selected_ownScp_level3_cbLabel { get; set; }
        private MyCheckBox_button Selected_ownScp_level4_cbButton { get; set; }
        private Label Selected_ownScp_level4_cbLabel { get; set; }
        private Label Mbco_scps_label { get; set; }
        private OwnListBox Mbco_scps_listBox { get; set; }
        public Label OwnSubScps_label { get; set; }
        private OwnListBox OwnSubScps_listBox { get; set; }
        private Label SortHeadline_label { get; set; }
        private OwnListBox Sort_listBox { get; set; }
        private bool Update_mbco_scps { get; set; }
        private bool Update_own_subScps { get; set; }
        private Button Add_subScp_button { get; set; }
        private Button Remove_subScp_button { get; set; }
        private Button Write_mbcoHierarchy_button { get; set; }
        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }

        public DefineSCPs_userInterface_class(MyPanel overall_panel,
                                              Label overall_headline_label,
                                              Label newOwnScp_label,
                                              OwnTextBox newOwnScp_textBox,
                                              Button add_newOwnScp_button,
                                              Button remove_ownScp_button,
                                              Label selected_ownScp_label,
                                              OwnListBox selected_ownScp_listBox,
                                              Label selected_ownScp_level_label,
                                              MyCheckBox_button selected_ownScp_level1_cbButton,
                                              Label selected_ownScp_level1_cbLabel,
                                              MyCheckBox_button selected_ownScp_level2_cbButton,
                                              Label selected_ownScp_level2_cbLabel,
                                              MyCheckBox_button selected_ownScp_level3_cbButton,
                                              Label selected_ownScp_level3_cbLabel,
                                              MyCheckBox_button selected_ownScp_level4_cbButton,
                                              Label selected_ownScp_level4_cbLabel,
                                              MyPanel selection_panel,
                                              Label mbco_scps_label,
                                              OwnListBox mbco_scps_listBox,
                                              Label sortHeadline_label,
                                              OwnListBox sort_listBox,
                                              Button add_subScp_button,
                                              Button remove_subScp_button,
                                              Button write_mbcoHierarchy_button,
                                              Label ownSubScps_label,
                                              OwnListBox ownSubScps_listBox,
                                              Label error_report_label,
                                              MBCO_enrichment_pipeline_options_class mbco_options,
                                              Form1_default_settings_class form_default_settings
                                              )
        {
            Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.Overall_headline_label = overall_headline_label;
            this.NewOwnScp_label = newOwnScp_label;
            this.NewOwnScp_textBox = newOwnScp_textBox;
            this.Add_newOwnScp_button = add_newOwnScp_button;
            this.Remove_ownScp_button = remove_ownScp_button;
            this.SelectedOwnScp_label = selected_ownScp_label;
            this.SelectedOwnScp_listBox = selected_ownScp_listBox;
            this.Selected_ownScp_level_label = selected_ownScp_level_label;
            this.Selected_ownScp_level1_cbButton = selected_ownScp_level1_cbButton;
            this.Selected_ownScp_level1_cbLabel = selected_ownScp_level1_cbLabel;
            this.Selected_ownScp_level2_cbButton = selected_ownScp_level2_cbButton;
            this.Selected_ownScp_level2_cbLabel = selected_ownScp_level2_cbLabel;
            this.Selected_ownScp_level3_cbButton = selected_ownScp_level3_cbButton;
            this.Selected_ownScp_level3_cbLabel = selected_ownScp_level3_cbLabel;
            this.Selected_ownScp_level4_cbButton = selected_ownScp_level4_cbButton;
            this.Selected_ownScp_level4_cbLabel = selected_ownScp_level4_cbLabel;
            this.Selection_panel = selection_panel;
            this.Mbco_scps_label = mbco_scps_label;
            this.Mbco_scps_listBox = mbco_scps_listBox;
            this.SortHeadline_label = sortHeadline_label;
            this.Sort_listBox = sort_listBox;
            this.Add_subScp_button = add_subScp_button;
            this.Remove_subScp_button = remove_subScp_button;
            this.Write_mbcoHierarchy_button = write_mbcoHierarchy_button;
            this.OwnSubScps_label = ownSubScps_label;
            this.OwnSubScps_listBox = ownSubScps_listBox;
            Set_to_default();

            Update_mbco_parent_child_and_child_parent_obo_networks_and_adjust_sortByList(mbco_options, error_report_label);

            Update_all_graphic_elements();
        }

        public void Update_all_graphic_elements()
        {
            int left_referencePosition;
            int right_referencePosition;
            int top_referencePosition;
            int bottom_referencePosition;
            Label my_label;
            MyCheckBox_button my_cbButton;
            OwnTextBox my_textBox;
            OwnListBox my_listBox;
            MyPanel my_panel;
            Button my_button;

            #region Overall and selection panel
            Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);
            left_referencePosition = 0;
            right_referencePosition = Overall_panel.Width;
            top_referencePosition = (int)Math.Round(0.25* Overall_panel.Height);
            bottom_referencePosition = Overall_panel.Height;
            my_panel = Selection_panel;
            Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition); ;
            #endregion

            #region TextBoxes, CheckBoxes and listBoxes at the top
            int height_of_headline = (int)Math.Round(0.05F * Overall_panel.Height);
            int height_of_one_row_at_top = (int)Math.Round(0.25F * (this.Selection_panel.Location.Y - height_of_headline));
            int shared_distances_from_leftRightSides = (int)Math.Round(0.01 * this.Overall_panel.Width);

            bottom_referencePosition = height_of_headline;

            top_referencePosition = bottom_referencePosition;
            bottom_referencePosition = top_referencePosition + height_of_one_row_at_top;

            left_referencePosition = (int)Math.Round(0.36F*this.Overall_panel.Width);
            right_referencePosition = (int)Math.Round(0.85F* Overall_panel.Width);
            my_textBox = NewOwnScp_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = right_referencePosition;
            right_referencePosition = Overall_panel.Width - shared_distances_from_leftRightSides;
            my_button = Add_newOwnScp_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            top_referencePosition = bottom_referencePosition;
            bottom_referencePosition = top_referencePosition + height_of_one_row_at_top;
            left_referencePosition = (int)Math.Round(0.5F * Overall_panel.Width);
            right_referencePosition = Overall_panel.Width - shared_distances_from_leftRightSides;
            my_button = Remove_ownScp_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            top_referencePosition = bottom_referencePosition;
            bottom_referencePosition = top_referencePosition + height_of_one_row_at_top;
            left_referencePosition = shared_distances_from_leftRightSides;
            right_referencePosition = Overall_panel.Width - shared_distances_from_leftRightSides;
            my_listBox = SelectedOwnScp_listBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            right_referencePosition = (int)Math.Round(0.52F * Overall_panel.Width);
            int total_width_for_level_checkboxes = this.Overall_panel.Width - shared_distances_from_leftRightSides - right_referencePosition;
            int width_of_each_level_checkBox = (int)Math.Round((float)total_width_for_level_checkboxes / 4F);
            int heightWidth_of_level_checkBoxButtons = (int)Math.Round(0.7F * height_of_one_row_at_top);
            int half_extraHeight_checkBoxLabels = (int)Math.Round(0.5F * (height_of_one_row_at_top - heightWidth_of_level_checkBoxButtons));
            int level_cbButton_location_y = SelectedOwnScp_listBox.Location.Y + SelectedOwnScp_listBox.Height;
            Dictionary<MyCheckBox_button, Label> cbButton_label_dict = new Dictionary<MyCheckBox_button, Label>();
            cbButton_label_dict.Add(Selected_ownScp_level1_cbButton, Selected_ownScp_level1_cbLabel);
            cbButton_label_dict.Add(Selected_ownScp_level2_cbButton, Selected_ownScp_level2_cbLabel);
            cbButton_label_dict.Add(Selected_ownScp_level3_cbButton, Selected_ownScp_level3_cbLabel);
            cbButton_label_dict.Add(Selected_ownScp_level4_cbButton, Selected_ownScp_level4_cbLabel);
            MyCheckBox_button[] cbButtons = cbButton_label_dict.Keys.ToArray();
      
            foreach (MyCheckBox_button level_checkBoxButton in cbButtons)
            {
                top_referencePosition = level_cbButton_location_y;
                bottom_referencePosition = top_referencePosition + heightWidth_of_level_checkBoxButtons;
                left_referencePosition = right_referencePosition;
                right_referencePosition = left_referencePosition + heightWidth_of_level_checkBoxButtons;
                my_cbButton = level_checkBoxButton;
                Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

                top_referencePosition = level_cbButton_location_y - half_extraHeight_checkBoxLabels;
                bottom_referencePosition = top_referencePosition + height_of_one_row_at_top;
                left_referencePosition = level_checkBoxButton.Location.X + level_checkBoxButton.Width;
                right_referencePosition = left_referencePosition + width_of_each_level_checkBox - heightWidth_of_level_checkBoxButtons;
                my_label = cbButton_label_dict[level_checkBoxButton];
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
            }
            #endregion

            #region Overall headline label
            left_referencePosition = shared_distances_from_leftRightSides;
            right_referencePosition = Overall_panel.Width - shared_distances_from_leftRightSides;
            top_referencePosition = 0;
            bottom_referencePosition = NewOwnScp_textBox.Location.Y;
            my_label = Overall_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
            #endregion

            #region Labels at the top
            left_referencePosition = shared_distances_from_leftRightSides;
            right_referencePosition = NewOwnScp_textBox.Location.X;
            top_referencePosition = NewOwnScp_textBox.Location.Y;
            bottom_referencePosition = NewOwnScp_textBox.Location.Y + NewOwnScp_textBox.Height;
            my_label = NewOwnScp_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = shared_distances_from_leftRightSides;
            right_referencePosition = Remove_ownScp_button.Location.X;
            top_referencePosition = Remove_ownScp_button.Location.Y;
            bottom_referencePosition = Remove_ownScp_button.Location.Y + Remove_ownScp_button.Height;
            my_label = SelectedOwnScp_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = shared_distances_from_leftRightSides;
            right_referencePosition = Selected_ownScp_level1_cbButton.Location.X;
            top_referencePosition = SelectedOwnScp_listBox.Location.Y + SelectedOwnScp_listBox.Height;
            bottom_referencePosition = Selected_ownScp_level1_cbButton.Location.Y + Selected_ownScp_level1_cbButton.Height + half_extraHeight_checkBoxLabels;
            my_label = Selected_ownScp_level_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
            #endregion

            #region SCP listBoxes
            left_referencePosition = 0;
            right_referencePosition = Selection_panel.Width;
            top_referencePosition = (int)Math.Round(0.05F*Selection_panel.Height);
            bottom_referencePosition = (int)Math.Round(0.5F*Selection_panel.Height);
            my_listBox = Mbco_scps_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = 0;
            right_referencePosition = Selection_panel.Width;
            top_referencePosition = (int)Math.Round(0.7F * Selection_panel.Height);
            bottom_referencePosition = Selection_panel.Height;
            my_listBox = OwnSubScps_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
            #endregion

            #region MBCO SCP listBox label
            left_referencePosition = 0;
            right_referencePosition = Selection_panel.Width;
            top_referencePosition = 0;
            bottom_referencePosition = Mbco_scps_listBox.Location.Y;
            my_label = Mbco_scps_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
            #endregion

            #region Buttons, listBoxes and labels between SCP listBoxes
            int height_of_each_row_between_SCP_listBoxes = (int)Math.Round(0.3F * (OwnSubScps_listBox.Location.Y - Mbco_scps_listBox.Location.Y - Mbco_scps_listBox.Height));

            bottom_referencePosition = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height;

            left_referencePosition = 0;
            right_referencePosition = (int)Math.Round(0.3F * this.Selection_panel.Width);

            top_referencePosition = bottom_referencePosition;
            bottom_referencePosition = top_referencePosition + height_of_each_row_between_SCP_listBoxes;
            my_button = Add_subScp_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            top_referencePosition = bottom_referencePosition;
            bottom_referencePosition = top_referencePosition + height_of_each_row_between_SCP_listBoxes;
            my_button = Remove_subScp_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = (int)Math.Round(0.35 * this.Selection_panel.Width);
            right_referencePosition = (int)Math.Round(0.7 * this.Selection_panel.Width); 
            my_listBox = Sort_listBox;
            Sort_listBox = Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = Sort_listBox.Location.X;
            right_referencePosition = Selection_panel.Width;
            top_referencePosition = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height;
            bottom_referencePosition = Sort_listBox.Location.Y;
            my_label = SortHeadline_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            Set_fontSize_and_size_for_ownSubScps_label();

            left_referencePosition = (int)Math.Round(0.7 * this.Selection_panel.Width);
            right_referencePosition = Selection_panel.Width;
            top_referencePosition = Add_subScp_button.Location.Y;
            bottom_referencePosition = Remove_subScp_button.Location.Y + Remove_subScp_button.Height;
            my_button = Write_mbcoHierarchy_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
            #endregion


            Update_mbco_scps = true;
            Update_own_subScps = false;
        }

        private void Set_fontSize_and_size_for_ownSubScps_label()
        {
            int left_referencePosition = 0;
            int right_referencePosition = Selection_panel.Width;
            int top_referencePosition = Remove_subScp_button.Location.Y + Remove_subScp_button.Height;
            int bottom_referencePosition = OwnSubScps_listBox.Location.Y;
            Label my_label = this.OwnSubScps_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);
        }

        public void Update_mbco_parent_child_and_child_parent_obo_networks_and_adjust_sortByList(MBCO_enrichment_pipeline_options_class mbco_options, Label error_report_label)
        {
            Ontology_type_enum reference_ontology = Ontology_classification_class.Get_related_human_ontology(mbco_options.Next_ontology);

            Mbco_parent_child_network = new MBCO_obo_network_class(reference_ontology);
            Mbco_parent_child_network.Generate_by_reading_safed_obo_file(error_report_label, Form_default_settings);

            //MBCO_association_class mbco_association = new MBCO_association_class();
            //mbco_association.Read_without_any_modifications(mbco_options.Next_ontology, error_report_label, Form_default_settings);
            //string[] all_scps = mbco_association.Get_all_distinct_ordered_scps();
            //Mbco_parent_child_network.Keep_only_input_nodeNames(all_scps);

            if (Ontology_classification_class.Is_go_ontology(reference_ontology))
            {
                Mbco_parent_child_network.Keep_only_scps_of_selected_namespace_if_gene_ontology();
            }
            Set_sortBy_listBox_to_default_under_consideration_of_selected_ontology();
            Update_mbco_scps = true;
            Update_own_subScps = true;
        }

        private void Check_if_ontologies_match(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(mbco_options.Next_ontology);
            if (!Mbco_parent_child_network.Ontology.Equals(human_ontology)) { throw new Exception(); }
        }

        private void Set_sortBy_listBox_to_default_under_consideration_of_selected_ontology()
        {
            Sort_listBox.Items.Clear();
            Sort_listBox.Items.Add(Form1_shared_text_class.Sort_alphabetically_text);
            if (Ontology_classification_class.Is_mbco_ontology(this.Mbco_parent_child_network.Ontology))
            {
                Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevelParentScp_text);
                Sort_listBox.SilentSelectedIndex = 1;
            }
            else
            {
                Sort_listBox.SilentSelectedIndex = 0;
            }
        }

        private void Set_to_default()
        {
            Add_subScp_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Add_subScp_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Add_subScp_button.Visible = false;
            Remove_subScp_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Remove_subScp_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Remove_subScp_button.Visible = false;
            NewOwnScp_textBox.Text = "My SCP";
            Add_newOwnScp_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Add_newOwnScp_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Add_newOwnScp_button.Visible = true;
            Remove_ownScp_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Remove_ownScp_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Remove_ownScp_button.Visible = false;
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Sort_listBox.SelectionMode = SelectionMode.One;

            SelectedOwnScp_listBox.SelectionMode = SelectionMode.One;
            Selected_ownScp_level_label.Visible = false;
            Selected_ownScp_level1_cbButton.Visible = false;
            Selected_ownScp_level1_cbLabel.Visible = false;
            Selected_ownScp_level2_cbButton.Visible = false;
            Selected_ownScp_level2_cbLabel.Visible = false;
            Selected_ownScp_level3_cbButton.Visible = false;
            Selected_ownScp_level3_cbLabel.Visible = false;
            Selected_ownScp_level4_cbButton.Visible = false;
            Selected_ownScp_level4_cbLabel.Visible = false;
            Selection_panel.Visible = false;
        }

        private void Update_selectedSCPs_listBox_and_level_checkBoxes(MBCO_enrichment_pipeline_options_class options, string new_scp)
        {
            Check_if_ontologies_match(options);
            string[] all_own_scps = options.OwnScp_mbcoSubScps_dict.Keys.OrderBy(l => l).ToArray();
            this.SelectedOwnScp_listBox.Items.Clear();
            this.SelectedOwnScp_listBox.Items.AddRange(all_own_scps);
            int new_index = this.SelectedOwnScp_listBox.Items.IndexOf(new_scp);
            if ((new_index==-1)&&(this.SelectedOwnScp_listBox.Items.Count>0)) { new_index = 0; }
            this.SelectedOwnScp_listBox.SilentSelectedIndex = new_index;
            if (new_index!=-1)
            {
                string selected_scp = (string)this.SelectedOwnScp_listBox.SelectedItem;
                this.Selected_ownScp_level_label.Visible = true;
                switch (options.OwnScp_level_dict[selected_scp])
                {
                    case 1:
                        this.Selected_ownScp_level1_cbButton.SilentChecked = true;
                        this.Selected_ownScp_level2_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level3_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level4_cbButton.SilentChecked = false;
                        break;
                    case 2:
                        this.Selected_ownScp_level1_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level2_cbButton.SilentChecked = true;
                        this.Selected_ownScp_level3_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level4_cbButton.SilentChecked = false;
                        break;
                    case 3:
                        this.Selected_ownScp_level1_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level2_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level3_cbButton.SilentChecked = true;
                        this.Selected_ownScp_level4_cbButton.SilentChecked = false;
                        break;
                    case 4:
                        this.Selected_ownScp_level1_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level2_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level3_cbButton.SilentChecked = false;
                        this.Selected_ownScp_level4_cbButton.SilentChecked = true;
                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        private void Update_mbcoSCPs_and_selectedSCPs_listBox(MBCO_enrichment_pipeline_options_class options, Label error_repor_label)
        {
            Check_if_ontologies_match(options);
            if (Update_mbco_scps)
            {
                string[] add_mbco_scps = new string[0];
                switch (Sort_listBox.SelectedItem)
                {
                    case Form1_shared_text_class.Sort_alphabetically_text:
                        string[] all_scps = Mbco_parent_child_network.Get_all_scps();
                        add_mbco_scps = all_scps.OrderBy(l => l).ToArray();
                        break;
                    case Form1_shared_text_class.Sort_byLevel_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_all_scps_sorted_by_level_with_level_announcing_headlines(new Dictionary<string, int>());
                        break;
                    case Form1_shared_text_class.Sort_byLevelParentScp_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_all_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(new Dictionary<string, int>());
                        break;
                    default:
                        throw new Exception();
                }
                Mbco_scps_listBox.Items.Clear();
                Mbco_scps_listBox.Items.AddRange(add_mbco_scps);
                Update_mbco_scps = false;
            }
            if (Update_own_subScps)
            {
                if (this.SelectedOwnScp_listBox.SelectedItems.Count == 0)
                {
                    this.OwnSubScps_listBox.Visible = false;
                    this.OwnSubScps_label.Visible = false;
                }
                else
                {
                    string current_selected_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
                    this.OwnSubScps_label.Text = "Genes of following SCPs will be added to SCP \n" + current_selected_scp;
                    Set_fontSize_and_size_for_ownSubScps_label();
                    string[] ownScp_subscps = options.OwnScp_mbcoSubScps_dict[current_selected_scp];
                    string[] add_ownScp_subscps = new string[0];
                    switch (Sort_listBox.SelectedItem)
                    {
                        case Form1_shared_text_class.Sort_alphabetically_text:
                            add_ownScp_subscps = ownScp_subscps.OrderBy(l => l).ToArray();
                            break;
                        case Form1_shared_text_class.Sort_byLevel_text:
                            add_ownScp_subscps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_with_level_announcing_headlines(false, new Dictionary<string, int>(), ownScp_subscps);
                            break;
                        case Form1_shared_text_class.Sort_byLevelParentScp_text:
                            add_ownScp_subscps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(false, new Dictionary<string, int>(), ownScp_subscps);
                            break;
                        default:
                            throw new Exception();
                    }
                    OwnSubScps_listBox.Items.Clear();
                    OwnSubScps_listBox.Items.AddRange(add_ownScp_subscps);
                    if (OwnSubScps_listBox.SelectedItems.Count == 0) { Remove_ownScp_button.Visible = false; }
                    this.OwnSubScps_listBox.Visible = true;
                    this.OwnSubScps_label.Visible = true;
                }
                Update_own_subScps = false;
            }
        }

        public void Set_to_visible(MBCO_enrichment_pipeline_options_class options, Label error_repor_label)
        {
            Check_if_ontologies_match(options);
            Update_own_subScps = true;
            Update_selectedSCPs_listBox_and_level_checkBoxes(options,"no new scp");
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_repor_label);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
            this.Overall_panel.Visible = true;
        }

        public void Sort_listBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            Update_mbco_scps = true;
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label);
        }
        public MBCO_enrichment_pipeline_options_class AddNewOwnSCP_button_Click(MBCO_enrichment_pipeline_options_class options, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            string new_scp = (string)this.NewOwnScp_textBox.Text.Clone();
            string[] mbco_scps = Mbco_parent_child_network.Get_all_scps();
            string[] existing_own_scps = options.OwnScp_mbcoSubScps_dict.Keys.ToArray();
            if (mbco_scps.Contains(new_scp))
            {
                this.NewOwnScp_textBox.Text = "SCP name is already used in MBCO";
                this.NewOwnScp_textBox.Refresh();
            }
            else if (existing_own_scps.Contains(new_scp))
            {
                this.NewOwnScp_textBox.Text = "SCP name is already defined as own SCP";
                this.NewOwnScp_textBox.Refresh();
            }
            else
            {
                options.OwnScp_mbcoSubScps_dict_add(new_scp, new string[0]);
                options.OwnScp_level_dict_add(new_scp, 1);
                Update_selectedSCPs_listBox_and_level_checkBoxes(options,new_scp);
                Update_own_subScps = true;
                Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label);
                Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
                options.Update_scps_in_select_SCPs_interface = true;
            }
            return options;
        }
        public MBCO_enrichment_pipeline_options_class RemoveOwnSCP_button_Click(MBCO_enrichment_pipeline_options_class options, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            string remove_scp = (string)this.SelectedOwnScp_listBox.SelectedItem.ToString();
            options.OwnScp_mbcoSubScps_dict.Remove(remove_scp);
            options.OwnScp_level_dict.Remove(remove_scp);
            Update_selectedSCPs_listBox_and_level_checkBoxes(options,"no new scp");
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
            options.Update_scps_in_select_SCPs_interface = true;
            return options;
        }
        public void SelectOwnScp_ownListBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options, Label error_repor_label)
        {
            Check_if_ontologies_match(options);
            Update_own_subScps = true;
            string selected_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
            Update_selectedSCPs_listBox_and_level_checkBoxes(options, selected_scp);
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_repor_label);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
        }
        public void SelectedOwnScp_level_checkBox_changed(MBCO_enrichment_pipeline_options_class options, int level)
        {
            Check_if_ontologies_match(options);
            string selected_scp = this.SelectedOwnScp_listBox.Text;
            options.OwnScp_level_dict[selected_scp] = level;
            Update_selectedSCPs_listBox_and_level_checkBoxes(options, selected_scp);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
        }
        private void Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes()
        {
            if (SelectedOwnScp_listBox.SelectedItems.Count>0)
            {
                this.Remove_ownScp_button.Visible = true;
                this.Selected_ownScp_level_label.Visible = true;
                this.Selected_ownScp_level1_cbButton.Visible = true;
                this.Selected_ownScp_level1_cbLabel.Visible = true;
                this.Selected_ownScp_level2_cbButton.Visible = true;
                this.Selected_ownScp_level2_cbLabel.Visible = true;
                this.Selected_ownScp_level3_cbButton.Visible = true;
                this.Selected_ownScp_level3_cbLabel.Visible = true;
                this.Selected_ownScp_level4_cbButton.Visible = true;
                this.Selected_ownScp_level4_cbLabel.Visible = true;
                this.Selection_panel.Visible = true;
            }
            else
            {
                this.Remove_ownScp_button.Visible = false;
                this.Selected_ownScp_level_label.Visible = false;
                this.Selected_ownScp_level1_cbButton.Visible = false;
                this.Selected_ownScp_level1_cbLabel.Visible = false;
                this.Selected_ownScp_level2_cbButton.Visible = false;
                this.Selected_ownScp_level2_cbLabel.Visible = false;
                this.Selected_ownScp_level3_cbButton.Visible = false;
                this.Selected_ownScp_level3_cbLabel.Visible = false;
                this.Selected_ownScp_level4_cbButton.Visible = false;
                this.Selected_ownScp_level4_cbLabel.Visible = false;
                this.Selection_panel.Visible = false;
            }
            this.Remove_ownScp_button.Refresh();
            if ((Mbco_scps_listBox.SelectedItems.Count > 0) && (SelectedOwnScp_listBox.SelectedItems.Count > 0))
            {
                Add_subScp_button.Visible = true;
            }
            else
            {
                Add_subScp_button.Visible = false;
            }
            Add_subScp_button.Refresh();
            if (OwnSubScps_listBox.SelectedItems.Count > 0)
            {
                Remove_subScp_button.Visible = true;
            }
            else
            {
                Remove_subScp_button.Visible = false;
            }
            Remove_subScp_button.Refresh();
        }
        public void MBCO_listBox_changed()
        {
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
        }
        public void OwnSubScps_listBox_changed()
        {
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
        }
        public MBCO_enrichment_pipeline_options_class AddSubScp_button_Click(MBCO_enrichment_pipeline_options_class options, Label error_repor_label)
        {
            Check_if_ontologies_match(options);
            string own_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
            int new_scps_length = this.Mbco_scps_listBox.SelectedItems.Count;
            string[] new_scps = new string[new_scps_length];
            for (int indexNew=0; indexNew<new_scps_length; indexNew++)
            {
                new_scps[indexNew] = this.Mbco_scps_listBox.SelectedItems[indexNew].ToString();
            }
            string[] all_scps = Overlap_class.Get_union(new_scps, options.OwnScp_mbcoSubScps_dict[own_scp]);
            options.OwnScp_mbcoSubScps_dict[own_scp] = all_scps.OrderBy(l => l).ToArray();
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_repor_label);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
            this.Mbco_scps_listBox.SelectedItems.Clear();
            return options;
        }

        public MBCO_enrichment_pipeline_options_class RemoveSubScp_button_Click(MBCO_enrichment_pipeline_options_class options, Label error_repor_label)
        {
            string own_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
            int remove_scps_length = this.OwnSubScps_listBox.SelectedItems.Count;
            string[] remove_scps = new string[remove_scps_length];
            for (int indexRemove = 0; indexRemove < remove_scps_length; indexRemove++)
            {
                remove_scps[indexRemove] = this.OwnSubScps_listBox.SelectedItems[indexRemove].ToString();
            }
            string[] all_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(options.OwnScp_mbcoSubScps_dict[own_scp],remove_scps);
            options.OwnScp_mbcoSubScps_dict[own_scp] = all_scps.OrderBy(l => l).ToArray();
            options.Remove_scps_from_group_selectedScps_dictionary(remove_scps);
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_repor_label);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes();
            this.OwnSubScps_listBox.SelectedItems.Clear();
            return options;
        }

        public bool Write_mbco_yed_network_and_return_if_interrupted(Label progress_report)
        {
            Common_functions.Global_definitions.Global_directory_and_file_class global_dirFile = new Common_functions.Global_definitions.Global_directory_and_file_class();
            progress_report.Text = "Write MBCO hierarchy into " + global_dirFile.Results_directory + "MBCO_hierarchy" + global_dirFile.Delimiter + "\r\nOpen with graph editor (e.g. yED graph editor, here use layout circular)";
            progress_report.Visible = true;
            progress_report.Refresh();
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            bool nw_generation_interrupted = Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted("MBCO_hierarchy" + global_dirFile.Delimiter, "MBCO_parent_child", yed_network.Shape_enum.Rectangle, progress_report, Form_default_settings);
            System.Threading.Thread.Sleep(8000);
            progress_report.Text = "";
            progress_report.Refresh();
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_fore;
            return nw_generation_interrupted;
        }
    }
}
