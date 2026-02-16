using Common_functions.Array_own;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using Enrichment;
using Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Windows_forms;
using Windows_forms_customized_tools;
using yed_network;

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
        private Button Tutorial_button { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }
        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }
        private MBCO_obo_network_class Mbco_child_parent_network { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private ProgressReport_interface_class ProgressReport { get; set; }

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
                                              ProgressReport_interface_class progressReport,
                                              Tutorial_interface_class userInterface_tutorial,
                                              Button tutorial_button,
                                              MBCO_enrichment_pipeline_options_class mbco_options,
                                              Form1_default_settings_class form_default_settings,
                                              MBCO_obo_network_class parent_child_nw
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
            this.ProgressReport = progressReport;
            this.UserInterface_tutorial = userInterface_tutorial;
            this.Tutorial_button = tutorial_button;
            Set_to_default();

            Update_mbco_parent_child_and_child_parent_obo_networks_and_adjust_sortByList(parent_child_nw);

            Update_all_graphic_elements();
        }

        public void Get_ontology_and_oranism_from_mbcp_parent_child_network(out Ontology_type_enum ontolgy, out Organism_enum organism)
        {
            ontolgy = Mbco_parent_child_network.Ontology;
            organism = Mbco_parent_child_network.Organism;
        }
        public bool Are_parentChild_hierarchy_networks_upToDate(MBCO_enrichment_pipeline_options_class options)
        {
            return (Mbco_parent_child_network.Ontology.Equals(options.Next_ontology))
                   && (Mbco_parent_child_network.Organism.Equals(options.Next_organism))
                   && (Mbco_child_parent_network.Ontology.Equals(options.Next_ontology))
                   && (Mbco_child_parent_network.Organism.Equals(options.Next_organism));
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
            bottom_referencePosition = (int)Math.Round(0.955F*Overall_panel.Height);
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
            int height_of_write_mbco_button =  (int)Math.Round(0.6F * (OwnSubScps_listBox.Location.Y - Mbco_scps_listBox.Location.Y - Mbco_scps_listBox.Height));
            int height_of_each_row_between_SCP_listBoxes = (int)Math.Round(1.0/3 * (OwnSubScps_listBox.Location.Y- Mbco_scps_listBox.Location.Y - Mbco_scps_listBox.Height));

            left_referencePosition = (int)Math.Round(0.7 * this.Selection_panel.Width);
            right_referencePosition = Selection_panel.Width;
            top_referencePosition = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height; ;
            bottom_referencePosition = top_referencePosition + height_of_write_mbco_button;
            if (Form_default_settings.Is_mono)
            {
                bottom_referencePosition -= (int)Math.Round(0.02 * Overall_panel.Height);
            }
            my_button = Write_mbcoHierarchy_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referencePosition, right_referencePosition, top_referencePosition, bottom_referencePosition);

            left_referencePosition = 0;
            right_referencePosition = (int)Math.Round(0.3F * this.Selection_panel.Width);

            top_referencePosition = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height;
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
            #endregion

            #region Tutorial button
            int hierarchyTutorialButtons_shared_distanceFrom_topBottom_border = (int)Math.Round(0.0001F * Overall_panel.Height);
            int hierarchyTutorialButtons_shared_distanceFrom_rightLeft_border = (int)Math.Round(0.2F * Overall_panel.Width);
            int hierarchyTutorialButtons_inBetween_halfDistance = (int)Math.Round(0.0025F * Overall_panel.Width);
            int hierarchyTutorialButtons_button_width = (int)Math.Round(0.3F * this.Overall_panel.Width);

            top_referencePosition= this.Selection_panel.Location.Y + this.Selection_panel.Height;
            bottom_referencePosition = Overall_panel.Height - hierarchyTutorialButtons_shared_distanceFrom_topBottom_border;
            right_referencePosition = this.Overall_panel.Width - shared_distances_from_leftRightSides;
            left_referencePosition = right_referencePosition - hierarchyTutorialButtons_button_width;
            Tutorial_button.Text = "Tour";
            my_button = Tutorial_button;
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

        public void Update_mbco_parent_child_and_child_parent_obo_networks_and_adjust_sortByList(MBCO_obo_network_class parentChild_nw)
        {
            if (!parentChild_nw.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }
            if (!parentChild_nw.Scp_hierarchal_interactions.Equals(SCP_hierarchy_interaction_type_enum.Parent_child)) { throw new Exception(); }
            Mbco_parent_child_network = parentChild_nw.Deep_copy_mbco_obo_nw();

            if (Ontology_classification_class.Is_go_ontology(parentChild_nw.Ontology))
            {
                Mbco_parent_child_network.Keep_only_scps_of_selected_namespace_if_gene_ontology();
            }
            Mbco_child_parent_network = Mbco_parent_child_network.Deep_copy_mbco_obo_nw();
            Mbco_child_parent_network.Transform_into_child_parent_direction();
            Set_sortBy_listBox_to_default_under_consideration_of_selected_ontology();
            Update_mbco_scps = true;
            Update_own_subScps = true;
        }

        private void Check_if_ontologies_match(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            if (!Mbco_parent_child_network.Ontology.Equals(mbco_options.Next_ontology)) { throw new Exception(); }
            if (!Mbco_parent_child_network.Organism.Equals(mbco_options.Next_organism)) { throw new Exception(); }
        }

        private void Set_sortBy_listBox_to_default_under_consideration_of_selected_ontology()
        {
            Sort_listBox.Items.Clear();
            Sort_listBox.Items.Add(Form1_shared_text_class.Sort_alphabetically_text);
            switch (this.Mbco_parent_child_network.Ontology)
            {
                case Ontology_type_enum.Mbco:
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevelParentScp_text);
                    Sort_listBox.SilentSelectedIndex = 1;
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byDepth_text);
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevelParentScp_text);
                    Sort_listBox.SilentSelectedIndex = 1;
                    break;
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byDepth_text);
                    Sort_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                    Sort_listBox.SilentSelectedIndex = 0;
                    break;
                default:
                    throw new Exception();
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
            Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Tutorial_button.Visible = true;
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
            this.SelectedOwnScp_listBox.Refresh();
            int new_index = this.SelectedOwnScp_listBox.Items.IndexOf(new_scp);
            if ((new_index==-1)&&(this.SelectedOwnScp_listBox.Items.Count>0)) { new_index = 0; }
            this.SelectedOwnScp_listBox.SilentSelectedIndex = new_index;
            if ((new_index!=-1)&&(Ontology_classification_class.Is_mbco_ontology(options.Next_ontology)))
            {
                string selected_scp = (string)this.SelectedOwnScp_listBox.SelectedItem;
                //this.Selected_ownScp_level_label.Visible = true;
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
                this.Selected_ownScp_level1_cbLabel.Refresh();
                this.Selected_ownScp_level2_cbLabel.Refresh();
                this.Selected_ownScp_level3_cbLabel.Refresh();
                this.Selected_ownScp_level4_cbLabel.Refresh();
                this.Selected_ownScp_level1_cbButton.Refresh();
                this.Selected_ownScp_level2_cbButton.Refresh();
                this.Selected_ownScp_level3_cbButton.Refresh();
                this.Selected_ownScp_level4_cbButton.Refresh();
            }
            else
            {

            }
        }

        private void Update_mbcoSCPs_and_selectedSCPs_listBox(MBCO_enrichment_pipeline_options_class options)
        {
            Check_if_ontologies_match(options);
            if (Update_mbco_scps)
            {
                ProgressReport.Update_progressReport_text_and_visualization("Updating SCP order in upper list box");
                string[] add_mbco_scps = new string[0];
                string[] all_scps = Mbco_parent_child_network.Get_all_scps();
                if (Ontology_classification_class.Is_go_ontology(options.Next_ontology))
                {
                    int min_scp_size = options.Get_go_min_size(options.Next_ontology);
                    int max_scp_size = options.Get_go_max_size(options.Next_ontology);
                    all_scps = Mbco_parent_child_network.Return_all_scps_meeting_minimum_and_maximum_size_criteria_if_specified(all_scps, min_scp_size, max_scp_size);
                }
                switch (Sort_listBox.SelectedItem)
                {
                    case Form1_shared_text_class.Sort_alphabetically_text:
                        add_mbco_scps = all_scps.OrderBy(l => l).ToArray();
                        break;
                    case Form1_shared_text_class.Sort_byLevel_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_with_level_announcing_headlines(false, new Dictionary<string, int>(), all_scps);
                        break;
                    case Form1_shared_text_class.Sort_byDepth_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_depth_with_depth_announcing_headlines(false, new Dictionary<string, int>(), all_scps);
                        break;
                    case Form1_shared_text_class.Sort_byLevelParentScp_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(false, new Dictionary<string, int>(), all_scps);
                        break;
                    default:
                        throw new Exception();
                }

                Mbco_scps_listBox.Items.Clear();
                Mbco_scps_listBox.Items.AddRange(add_mbco_scps);
                Mbco_scps_listBox.Refresh();
                Update_mbco_scps = false;
                ProgressReport.Update_progressReport_text_and_visualization("");
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
                    ProgressReport.Update_progressReport_text_and_visualization("Updating SCP order in lower list box");
                    string current_selected_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
                    this.OwnSubScps_label.Text = "Selected own SCP contains genes of";
                    Set_fontSize_and_size_for_ownSubScps_label();
                    string[] ownScp_subscps = options.OwnScp_mbcoSubScps_dict[current_selected_scp];
                    string[] add_ownScp_subscps = new string[0];
                    if (ownScp_subscps.Length > 0)
                    {
                        switch (Sort_listBox.SelectedItem)
                        {
                            case Form1_shared_text_class.Sort_alphabetically_text:
                                add_ownScp_subscps = ownScp_subscps.OrderBy(l => l).ToArray();
                                break;
                            case Form1_shared_text_class.Sort_byLevel_text:
                                add_ownScp_subscps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_with_level_announcing_headlines(false, new Dictionary<string, int>(), ownScp_subscps);
                                break;
                            case Form1_shared_text_class.Sort_byDepth_text:
                                add_ownScp_subscps = Mbco_parent_child_network.Get_input_scps_sorted_by_depth_with_depth_announcing_headlines(false, new Dictionary<string, int>(), ownScp_subscps);
                                break;
                            case Form1_shared_text_class.Sort_byLevelParentScp_text:
                                add_ownScp_subscps = Mbco_child_parent_network.Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_child_parent(false, new Dictionary<string, int>(), ownScp_subscps);
                                break;
                            default:
                                throw new Exception();
                        }
                        if (Ontology_classification_class.Is_go_ontology(options.Next_ontology))
                        {
                            int min_scp_size = options.Get_go_min_size(options.Next_ontology);
                            int max_scp_size = options.Get_go_max_size(options.Next_ontology);
                            ownScp_subscps = Mbco_parent_child_network.Return_all_scps_meeting_minimum_and_maximum_size_criteria_if_specified(ownScp_subscps, min_scp_size, max_scp_size);
                        }
                    }
                    OwnSubScps_listBox.Items.Clear();
                    OwnSubScps_listBox.Items.AddRange(add_ownScp_subscps);
                    if (OwnSubScps_listBox.SelectedItems.Count == 0) { Remove_ownScp_button.Visible = false; }
                    OwnSubScps_listBox.Refresh();
                    this.OwnSubScps_listBox.Visible = true;
                    this.OwnSubScps_label.Visible = true;
                }
                Selection_panel.Refresh();
                Update_own_subScps = false;
                ProgressReport.Update_progressReport_text_and_visualization("");
            }
        }

        public void Set_to_visible(MBCO_enrichment_pipeline_options_class options, bool update_scp_lists)
        {
            Check_if_ontologies_match(options);
            if (update_scp_lists)
            {
                Update_mbco_scps = true;
                Update_own_subScps = true;
            }
            Update_selectedSCPs_listBox_and_level_checkBoxes(options,"no new scp");
            Update_mbcoSCPs_and_selectedSCPs_listBox(options);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
            this.Overall_panel.Visible = true;
            this.Overall_panel.Refresh();
        }

        public void Sort_listBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options)
        {
            Check_if_ontologies_match(options);
            Update_mbco_scps = true;
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options);
        }
        public MBCO_enrichment_pipeline_options_class AddNewOwnSCP_button_Click(MBCO_enrichment_pipeline_options_class options)
        {
            Check_if_ontologies_match(options);
            string new_scp = (string)this.NewOwnScp_textBox.Text.Replace('$','-').Clone();
            string[] mbco_scps = Mbco_parent_child_network.Get_all_scps();
            string[] existing_own_scps = options.OwnScp_mbcoSubScps_dict.Keys.ToArray();
            if (mbco_scps.Contains(new_scp))
            {
                this.NewOwnScp_textBox.Text = "SCP name is already used in ontology";
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
                Update_mbcoSCPs_and_selectedSCPs_listBox(options);
                Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
                Selection_panel.Refresh();
            }
            return options;
        }
        public MBCO_enrichment_pipeline_options_class RemoveOwnSCP_button_Click(MBCO_enrichment_pipeline_options_class options)
        {
            Check_if_ontologies_match(options);
            string remove_scp = (string)this.SelectedOwnScp_listBox.SelectedItem.ToString();
            options.OwnScp_mbcoSubScps_dict.Remove(remove_scp);
            options.OwnScp_level_dict.Remove(remove_scp);
            Update_selectedSCPs_listBox_and_level_checkBoxes(options,"no new scp");
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
            return options;
        }
        public void SelectOwnScp_ownListBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options)
        {
            Check_if_ontologies_match(options);
            Update_own_subScps = true;
            string selected_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
            Update_selectedSCPs_listBox_and_level_checkBoxes(options, selected_scp);
            Update_mbcoSCPs_and_selectedSCPs_listBox(options);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
        }
        public void SelectedOwnScp_level_checkBox_changed(MBCO_enrichment_pipeline_options_class options, int level)
        {
            Check_if_ontologies_match(options);
            string selected_scp = this.SelectedOwnScp_listBox.Text;
            options.OwnScp_level_dict[selected_scp] = level;
            Update_selectedSCPs_listBox_and_level_checkBoxes(options, selected_scp);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
        }
        private void Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(MBCO_enrichment_pipeline_options_class options)
        {
            if (SelectedOwnScp_listBox.SelectedItems.Count > 0)
            {
                this.Remove_ownScp_button.Visible = true;
                this.Selection_panel.Visible = true;
            }
            else
            {
                this.Remove_ownScp_button.Visible = false;
                this.Selection_panel.Visible = false;
            }


            if ((SelectedOwnScp_listBox.SelectedItems.Count>0)&&(Ontology_classification_class.Is_mbco_ontology(options.Next_ontology)))
            {
                this.Selected_ownScp_level_label.Visible = true;
                this.Selected_ownScp_level1_cbButton.Visible = true;
                this.Selected_ownScp_level1_cbLabel.Visible = true;
                this.Selected_ownScp_level2_cbButton.Visible = true;
                this.Selected_ownScp_level2_cbLabel.Visible = true;
                this.Selected_ownScp_level3_cbButton.Visible = true;
                this.Selected_ownScp_level3_cbLabel.Visible = true;
                this.Selected_ownScp_level4_cbButton.Visible = true;
                this.Selected_ownScp_level4_cbLabel.Visible = true;
            }
            else
            {
                this.Selected_ownScp_level_label.Visible = false;
                this.Selected_ownScp_level1_cbButton.Visible = false;
                this.Selected_ownScp_level1_cbLabel.Visible = false;
                this.Selected_ownScp_level2_cbButton.Visible = false;
                this.Selected_ownScp_level2_cbLabel.Visible = false;
                this.Selected_ownScp_level3_cbButton.Visible = false;
                this.Selected_ownScp_level3_cbLabel.Visible = false;
                this.Selected_ownScp_level4_cbButton.Visible = false;
                this.Selected_ownScp_level4_cbLabel.Visible = false;
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
        public void MBCO_listBox_changed(MBCO_enrichment_pipeline_options_class options)
        {
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
        }
        public void OwnSubScps_listBox_changed(MBCO_enrichment_pipeline_options_class options)
        {
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
        }

        #region Feature tour
        public void Set_tutorial_button_to_inactive()
        {
            this.Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Tutorial_button.Refresh();
        }
        public void Set_tutorial_button_to_active(System.Windows.Forms.Button selected_button)
        {
            selected_button.BackColor = Form_default_settings.Color_button_pressed_back;
            selected_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            selected_button.Refresh();
        }
        public void Tutorial_button_pressed(MBCO_enrichment_pipeline_options_class enrich_options)
        {
            MBCO_enrichment_pipeline_options_class copy_enrich_options = enrich_options.Deep_copy();
            copy_enrich_options.Clear_all_dictionaries_with_selected_scps_for_next_ontology();

            Update_mbcoSCPs_and_selectedSCPs_listBox(copy_enrich_options);

            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel = Overall_panel.Location.X - distance_from_overalMenueLabel;
            int mid_y_position;
            int right_x_position;
            string text;

            right_x_position = right_x_position_next_to_overall_panel;

            #region Extract current entries from text boxes and set buttons to defaults
            string new_own_scp_text = (string)NewOwnScp_textBox.Text.Clone();
            bool is_level1_button_selected = Selected_ownScp_level1_cbButton.Checked;
            bool is_level2_button_selected = Selected_ownScp_level2_cbButton.Checked;
            bool is_level3_button_selected = Selected_ownScp_level3_cbButton.Checked;
            bool is_level4_button_selected = Selected_ownScp_level4_cbButton.Checked;
            string sort_scps_by = Sort_listBox.SelectedItem.ToString();
            #endregion

            bool is_mbco = Ontology_classification_class.Is_mbco_ontology(copy_enrich_options.Next_ontology);
            string pathway_term_name;
            string capitalized_pathway_term_name;
            string first_pathway_term_name;
            string new_scp;
            if (is_mbco)
            {
                new_scp = "Mitochondrial homeostasis";
                pathway_term_name = "SCP";
                capitalized_pathway_term_name = "SCP";
                first_pathway_term_name = "subcellular processes (SCPs)";
            }
            else
            {
                new_scp = "My pathway";
                pathway_term_name = "pathway";
                capitalized_pathway_term_name = "Pathway";
                first_pathway_term_name = "pathways";
            }

            bool end_tour = false;
            int tour_box_index = -1;
            bool escape_pressed = false;
            bool back_pressed = false;

            while (!end_tour)
            {
                tour_box_index++;
                switch (tour_box_index)
                {
                    case -1:
                        end_tour = true;
                        break;
                    case 0:
                        mid_y_position = this.Overall_panel.Location.Y + (int)Math.Round(this.Overall_panel.Location.Y + 0.5F * this.Overall_panel.Height);
                        text = "The menu allows definition of new " + first_pathway_term_name + " by merging the genes of existing " + pathway_term_name + "s.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        while (copy_enrich_options.OwnScp_level_dict.Keys.Count > 0)
                        {
                            RemoveOwnSCP_button_Click(copy_enrich_options);
                        }
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        mid_y_position = this.Overall_panel.Location.Y + (int)Math.Round(this.NewOwnScp_textBox.Location.Y + 0.5F * this.NewOwnScp_textBox.Height);
                        text = "Select a name for the new SCP and press the 'Add'-button.";
                        copy_enrich_options.OwnScp_mbcoSubScps_dict_clear();
                        copy_enrich_options.OwnScp_level_dict_clear();
                        NewOwnScp_textBox.SilentText_and_refresh = (string)new_scp.Clone();
                        NewOwnScp_textBox.Refresh();
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Update_mbco_scps = true;
                        Update_own_subScps = true;
                        copy_enrich_options = AddNewOwnSCP_button_Click(copy_enrich_options);
                        this.Selected_ownScp_level_label.Refresh();
                        this.Selected_ownScp_level1_cbLabel.Refresh();
                        this.Selected_ownScp_level2_cbLabel.Refresh();
                        this.Selected_ownScp_level3_cbLabel.Refresh();
                        this.Selected_ownScp_level4_cbLabel.Refresh();
                        this.Selected_ownScp_level1_cbButton.Refresh();
                        this.Selected_ownScp_level2_cbButton.Refresh();
                        this.Selected_ownScp_level3_cbButton.Refresh();
                        this.Selected_ownScp_level4_cbButton.Refresh();
                        this.OwnSubScps_listBox.SilentSelectedIndex = this.OwnSubScps_listBox.Items.IndexOf(NewOwnScp_textBox.Text);
                        this.OwnSubScps_listBox.Refresh();
                        this.Selection_panel.Refresh();
                        NewOwnScp_textBox.SilentText_and_refresh = (string)new_own_scp_text.Clone();
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        if (is_mbco)
                        {
                            mid_y_position = this.Overall_panel.Location.Y + (int)Math.Round(this.Selected_ownScp_level1_cbLabel.Location.Y + 0.5F * this.Selected_ownScp_level1_cbLabel.Height);
                            text = "Select a level for the new " + pathway_term_name + ".";
                            NewOwnScp_textBox.Refresh();
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 3:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Sort_listBox.Location.Y + 0.5F * this.Sort_listBox.Height);
                        if (is_mbco) { text = "MBCO " + pathway_term_name + "s can be sorted alphabetically, by level or by level and parent " + pathway_term_name + "."; }
                        else { text = capitalized_pathway_term_name + " can be sorted alphabetically, by level (shortest distance from root " + pathway_term_name + ") or by depth (longest distance from root " + pathway_term_name + ")."; }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (is_mbco) { Sort_listBox.SilentSelectedIndex = Sort_listBox.Items.IndexOf(Form1_shared_text_class.Sort_byLevelParentScp_text); }
                        else { Sort_listBox.SilentSelectedIndex = Sort_listBox.Items.IndexOf(Form1_shared_text_class.Sort_byDepth_text); }
                        Sort_listBox_SelectedIndexChanged(copy_enrich_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 4:
                        Mbco_scps_listBox.SilentSelectedIndex = 0;
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Mbco_scps_listBox.Location.Y + 0.5F * this.Mbco_scps_listBox.Height);
                        if (is_mbco) { text = "To quickly jump to a level, enter the level number after selecting the " + pathway_term_name + " list box."; }
                        else { text = "To quickly jump to a level or depth, enter the level or depth number after selecting the " + pathway_term_name + " list box."; }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 5:
                        Sort_listBox.SilentSelectedIndex = Sort_listBox.Items.IndexOf(Form1_shared_text_class.Sort_alphabetically_text);
                        Sort_listBox_SelectedIndexChanged(copy_enrich_options);

                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Mbco_scps_listBox.Location.Y + 0.5F * this.Mbco_scps_listBox.Height);
                        text = "Select " + pathway_term_name + "s whose merged genes will be the genes annoated to the new " + pathway_term_name + " and press the 'Add'-button.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (is_mbco)
                        {
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial gene expression");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial dynamics");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial energy production");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial protein import machinery");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Post-translational protein modification in Mitochondria");
                        }
                        if (Mbco_scps_listBox.SelectedItems.Count == 0)
                        {
                            Mbco_scps_listBox.SilentSelectedIndex = (int)Math.Round(0.5F * Mbco_scps_listBox.Items.Count);
                            Mbco_scps_listBox.SilentSelectedIndex = (int)Math.Round(0.6F * Mbco_scps_listBox.Items.Count);
                            Mbco_scps_listBox.SilentSelectedIndex = (int)Math.Round(0.8F * Mbco_scps_listBox.Items.Count);
                        }
                        MBCO_listBox_changed(copy_enrich_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 6:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Add_subScp_button.Location.Y + 0.5F * this.Add_subScp_button.Height);
                        text = "Since parent " + pathway_term_name + "s contain all genes of their children, there is not need for adding descendent pathways here.";
                        if (is_mbco)
                        {
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial gene expression");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial dynamics");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial energy production");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial protein import machinery");
                            Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Post-translational protein modification in Mitochondria");
                        }
                        if (Mbco_scps_listBox.SelectedItems.Count == 0)
                        {
                            Mbco_scps_listBox.SilentSelectedIndex = (int)Math.Round(0.5F * Mbco_scps_listBox.Items.Count);
                            Mbco_scps_listBox.SilentSelectedIndex = (int)Math.Round(0.6F * Mbco_scps_listBox.Items.Count);
                            Mbco_scps_listBox.SilentSelectedIndex = (int)Math.Round(0.8F * Mbco_scps_listBox.Items.Count);
                        }
                        MBCO_listBox_changed(copy_enrich_options);
                        if (copy_enrich_options.OwnScp_mbcoSubScps_dict.Keys.Count > 0)
                        {
                            copy_enrich_options.OwnScp_mbcoSubScps_dict_of_given_ownSCP_clear(new_scp);
                            Update_own_subScps = true;
                            Update_mbcoSCPs_and_selectedSCPs_listBox(copy_enrich_options);
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 7:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Add_subScp_button.Location.Y + 0.5F * this.Add_subScp_button.Height);
                        text = "After pressing the 'Add'-button the genes of the selected " + pathway_term_name + "s will be annotated to the new " + pathway_term_name + ".";
                        AddSubScp_button_Click(copy_enrich_options);
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

            #region Set buttons back to initial values
            NewOwnScp_textBox.SilentText_and_refresh = (string)new_own_scp_text.Clone();
            RemoveOwnSCP_button_Click(copy_enrich_options);

            Selected_ownScp_level1_cbButton.Checked = is_level1_button_selected;
            Selected_ownScp_level2_cbButton.Checked = is_level2_button_selected;
            Selected_ownScp_level3_cbButton.Checked = is_level3_button_selected;
            Selected_ownScp_level4_cbButton.Checked = is_level4_button_selected;

            if (Sort_listBox.Items.Count > 0) { Sort_listBox.SelectedIndex = Sort_listBox.Items.IndexOf(sort_scps_by); }
            Set_to_visible(enrich_options, true);
            #endregion



            UserInterface_tutorial.Set_to_invisible();
        }
        #endregion

        public MBCO_enrichment_pipeline_options_class AddSubScp_button_Click(MBCO_enrichment_pipeline_options_class options)
        {
            Check_if_ontologies_match(options);
            string own_scp = this.SelectedOwnScp_listBox.SelectedItem.ToString();
            int new_scps_length = this.Mbco_scps_listBox.SelectedItems.Count;
            string[] new_scps = new string[new_scps_length];
            for (int indexNew=0; indexNew<new_scps_length; indexNew++)
            {
                new_scps[indexNew] = this.Mbco_scps_listBox.SelectedItems[indexNew].ToString();
            }
            string[] all_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(new_scps, options.OwnScp_mbcoSubScps_dict[own_scp]);
            options.OwnScp_mbcoSubScps_dict[own_scp] = all_scps.OrderBy(l => l).ToArray();
            Update_own_subScps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
            this.Mbco_scps_listBox.SelectedItems.Clear();
            return options;
        }

        public MBCO_enrichment_pipeline_options_class RemoveSubScp_button_Click(MBCO_enrichment_pipeline_options_class options)
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
            Update_mbcoSCPs_and_selectedSCPs_listBox(options);
            Update_visibility_of_add_and_remove_buttons_and_level_checkBoxes(options);
            this.OwnSubScps_listBox.SelectedItems.Clear();
            return options;
        }

        public bool Write_mbco_yed_network_and_return_if_interrupted(Graph_editor_enum graphEditor)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Write_mbcoHierarchy_button.Refresh();
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string ontology_subdirectory = gdf.Ontology_hierarchy_subdirectory;
            string ontology_fileName = gdf.Get_name_for_ontology_hierarchy(Mbco_parent_child_network.Ontology);
            string[] legend_dataset_nodes = new string[0];
            bool nw_generation_interrupted = Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(Mbco_parent_child_network.Ontology, Ontology_overview_network_enum.Parent_child_hierarchy, ontology_subdirectory, ontology_fileName, legend_dataset_nodes, graphEditor, ProgressReport);
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Write_mbcoHierarchy_button.Refresh();
            return nw_generation_interrupted;
        }
    }
}
