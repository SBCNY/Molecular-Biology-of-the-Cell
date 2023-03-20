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
using System.Threading.Tasks;
using Windows_forms_customized_tools;
using System.Windows.Forms;
using Common_functions.Array_own;
using Network;
using Enrichment;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using MBCO;
using Result_visualization;

namespace ClassLibrary1.Select_scps_userInterface
{
    class Select_scps_userInterface_class
    {
        const string No_selected_scps_message = "No SCPs selected";
        private MyPanel Overall_panel { get; set; }
        private Label OverallHeadline_label { get; set; }
        private Label NewGroup_label { get; set; }
        private OwnTextBox NewGroup_ownTextBox { get; set; }
        private Button AddGroup_button { get; set; }
        private Button RemoveGroup_button { get; set; }
        private Label Groups_label {get;set;}
        private OwnListBox Groups_ownListBox { get; set; }
        private MyPanel Selection_panel { get; set; }
        private OwnListBox Mbco_scps_listBox { get; set; }
        private Label SelectedGroup_label { get; set; }
        private OwnListBox Selected_scps_listBox { get; set; }
        private Label SortSCPs_label { get; set; }
        private OwnListBox SortSCPs_listBox { get; set; }
        private Label IncludeHeadline_label { get; set; }
        private Label IncludeBracket_label { get; set; }
        private MyCheckBox_button IncludeOffspringSCPs_cbButton { get; set; }
        private Label IncludeOffspringSCPs_cbLabel { get; set; }
        private MyCheckBox_button IncludeAncestorSCPs_cbButton{ get; set; }
        private Label IncludeAncestorSCPs_cbLabel { get; set; }
        private bool Update_mbco_scps { get; set; }
        private bool Update_selected_scps { get; set; }
        private Button Add_button { get; set; }
        private Button Remove_button { get; set; }
        private Button Write_mbcoHierarchy_button { get; set; }
        private MyCheckBox_button Show_onlySelectedScps_cbButton { get; set; }
        private Label Show_onlySelectedScps_cbLabel { get; set; }
        private MyCheckBox_button AddGenes_cbButton { get; set; }
        private Label AddGenes_cbLabel { get; set; }
        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }
        private MBCO_obo_network_class Mbco_child_parent_network { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }


        public Select_scps_userInterface_class(MyPanel overall_panel,
                                               Label overallHeadline_label,
                                               Label newGroup_label,
                                               OwnTextBox newGroup_ownTextBox,
                                               Button addGroup_button,
                                               Button removeGroup_button,
                                               Label groups_label,
                                               OwnListBox groups_ownListBox,
                                               MyPanel selection_panel,
                                               OwnListBox mbco_scps_listBox,
                                               Label selectedGroup_label,
                                               OwnListBox selected_scps_listBox,
                                               Label sortSCPs_label,
                                               OwnListBox sortSCPs_ownListBox,
                                               Button add_button,
                                               Button remove_button,
                                               Label includeHeadline_label,
                                               Label includeBracket_label,
                                               MyCheckBox_button includeOffspringSCPs_cbButton,
                                               Label includeOffspringSCPs_cbLabel,
                                               MyCheckBox_button includeAncestorSCPs_cbButton,
                                               Label includeAncestorSCPs_cbLabel,
                                               Button write_mbcoHierarchy_button,
                                               MyCheckBox_button show_onlySelectedScps_cbButton,
                                               Label show_onlySelectedScps_cbLabel,
                                               MyCheckBox_button addGenes_cbButton,
                                               Label addGenes_cbLabel,
                                               MBCO_enrichment_pipeline_options_class mbco_options,
                                               Label error_report_label,
                                               Form1_default_settings_class form_default_settings
                                               )
        {
            Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.OverallHeadline_label = overallHeadline_label;
            this.NewGroup_label = newGroup_label;
            this.NewGroup_ownTextBox = newGroup_ownTextBox;
            this.AddGroup_button = addGroup_button;
            this.RemoveGroup_button = removeGroup_button;
            this.Groups_label = groups_label;
            this.Groups_ownListBox = groups_ownListBox;
            this.Selection_panel = selection_panel;
            this.Mbco_scps_listBox = mbco_scps_listBox;
            this.SelectedGroup_label = selectedGroup_label;
            this.Selected_scps_listBox = selected_scps_listBox;
            this.SortSCPs_label = sortSCPs_label;
            this.SortSCPs_listBox = sortSCPs_ownListBox;
            this.Add_button = add_button;
            this.Remove_button = remove_button;
            this.IncludeHeadline_label = includeHeadline_label;
            this.IncludeBracket_label = includeBracket_label;
            this.IncludeOffspringSCPs_cbButton = includeOffspringSCPs_cbButton;
            this.IncludeOffspringSCPs_cbLabel = includeOffspringSCPs_cbLabel;
            this.IncludeAncestorSCPs_cbButton = includeAncestorSCPs_cbButton;
            this.IncludeAncestorSCPs_cbLabel = includeAncestorSCPs_cbLabel;
            this.Write_mbcoHierarchy_button = write_mbcoHierarchy_button;
            this.Show_onlySelectedScps_cbButton = show_onlySelectedScps_cbButton;
            this.Show_onlySelectedScps_cbLabel = show_onlySelectedScps_cbLabel;
            this.AddGenes_cbButton = addGenes_cbButton;
            this.AddGenes_cbLabel = addGenes_cbLabel;


            this.IncludeOffspringSCPs_cbButton.Checked = false;
            this.IncludeAncestorSCPs_cbButton.Checked = false;

            Update_mbco_parent_child_and_child_parent_networks_and_set_to_default(mbco_options,error_report_label);

            Update_all_graphic_elements(error_report_label);
        }

        private void Set_to_default_under_consideration_of_selected_ontology()
        {
            Add_button.Visible = false;
            Remove_button.Visible = false;
            IncludeOffspringSCPs_cbButton.SilentChecked = false;
            IncludeOffspringSCPs_cbButton.SilentChecked = false;
            SortSCPs_listBox.Items.Clear();
            SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_alphabetically_text);
            if (Ontology_classification_class.Is_mbco_ontology(Mbco_child_parent_network.Ontology))
            {
                SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevelParentScp_text);
                SortSCPs_listBox.SilentSelectedIndex = 1;
            }
            else
            {
                SortSCPs_listBox.SilentSelectedIndex = 0;
            }
            NewGroup_ownTextBox.Text = "Selected SCPs";
        }

        public void Update_mbco_parent_child_and_child_parent_networks_and_set_to_default(MBCO_enrichment_pipeline_options_class mbco_options, Label error_report_label)
        {
            Ontology_type_enum human_reference_ontology = Ontology_classification_class.Get_related_human_ontology(mbco_options.Next_ontology);

            Mbco_parent_child_network = new MBCO_obo_network_class(human_reference_ontology);
            Mbco_parent_child_network.Generate_by_reading_safed_obo_file(error_report_label, Form_default_settings);

            if (Ontology_classification_class.Is_go_ontology(Mbco_parent_child_network.Ontology))
            {
                Mbco_parent_child_network.Keep_only_scps_of_selected_namespace_if_gene_ontology();
            }

            //MBCO_association_class mbco_association = new MBCO_association_class();
            //mbco_association.Read_without_any_modifications(mbco_options.Next_ontology, error_report_label, Form_default_settings);
            //string[] all_scps = mbco_association.Get_all_distinct_ordered_scps();
            //Mbco_parent_child_network.Keep_only_input_nodeNames(all_scps);

            Mbco_child_parent_network = Mbco_parent_child_network.Deep_copy_mbco_obo_nw();
            Mbco_child_parent_network.Transform_into_child_parent_direction();

            Update_mbco_scps = true;
            Update_selected_scps = true;
            Set_to_default_under_consideration_of_selected_ontology();
        }

        public void Update_all_graphic_elements(Label error_report_label)
        { 
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            MyPanel my_panel;
            Label my_label;
            OwnTextBox my_textBox;
            MyCheckBox_button my_cbButton;
            OwnListBox my_listBox;
            Button my_button;

            Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            #region Overall panel textBoxes, listBoxes and buttons
            int shared_textBox_listBox_height_overallPanel = (int)Math.Round(0.05 * Overall_panel.Height);
            int shared_betweenDistanceHeight_textBox_listBox_height_overallPanel = (int)Math.Round(0.005 * Overall_panel.Height);
            int shared_distanceFrom_leftRightSides_overallPanel = (int)Math.Round(0.02F * Overall_panel.Width);

            top_referenceBorder = (int)Math.Round(0.05F * Overall_panel.Height);
            left_referenceBorder = (int)Math.Round(0.3F * Overall_panel.Width);
            right_referenceBorder = (int)Math.Round(0.8F * Overall_panel.Width);
            bottom_referenceBorder = top_referenceBorder + shared_textBox_listBox_height_overallPanel;
            my_textBox = this.NewGroup_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = NewGroup_ownTextBox.Location.X + NewGroup_ownTextBox.Width;
            right_referenceBorder = Overall_panel.Width - shared_distanceFrom_leftRightSides_overallPanel;
            top_referenceBorder = this.NewGroup_ownTextBox.Location.Y;
            bottom_referenceBorder = this.NewGroup_ownTextBox.Location.Y + NewGroup_ownTextBox.Height;
            my_button = AddGroup_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.4F * Overall_panel.Width);
            right_referenceBorder = Overall_panel.Width - shared_distanceFrom_leftRightSides_overallPanel;
            top_referenceBorder = this.NewGroup_ownTextBox.Location.Y + this.NewGroup_ownTextBox.Height + shared_betweenDistanceHeight_textBox_listBox_height_overallPanel;
            bottom_referenceBorder = top_referenceBorder + shared_textBox_listBox_height_overallPanel;
            my_button = RemoveGroup_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_distanceFrom_leftRightSides_overallPanel;
            right_referenceBorder = Overall_panel.Width - shared_distanceFrom_leftRightSides_overallPanel;
            top_referenceBorder = RemoveGroup_button.Location.Y + RemoveGroup_button.Height + shared_betweenDistanceHeight_textBox_listBox_height_overallPanel;
            bottom_referenceBorder = top_referenceBorder + shared_textBox_listBox_height_overallPanel;
            my_listBox = Groups_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Overall panel labels
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.NewGroup_ownTextBox.Location.Y;
            my_label = OverallHeadline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = NewGroup_ownTextBox.Location.X;
            top_referenceBorder = NewGroup_ownTextBox.Location.Y;
            bottom_referenceBorder = this.NewGroup_ownTextBox.Location.Y + NewGroup_ownTextBox.Height;
            my_label = NewGroup_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = RemoveGroup_button.Location.X;
            top_referenceBorder = RemoveGroup_button.Location.Y;
            bottom_referenceBorder = this.Groups_ownListBox.Location.Y;
            my_label = Groups_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Selection panel
            left_referenceBorder = shared_distanceFrom_leftRightSides_overallPanel;
            right_referenceBorder = Overall_panel.Width - shared_distanceFrom_leftRightSides_overallPanel;
            top_referenceBorder = Groups_ownListBox.Location.Y + Groups_ownListBox.Height;
            bottom_referenceBorder = Overall_panel.Height;
            my_panel = this.Selection_panel;
            Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region MBCO SCPs and selected SCPs listBoxes
            left_referenceBorder = 0;
            right_referenceBorder = this.Selection_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = (int)Math.Round(0.4F * this.Selection_panel.Height);
            my_listBox = Mbco_scps_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.Selection_panel.Width;
            top_referenceBorder = (int)Math.Round(0.6F * this.Selection_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.9F * this.Selection_panel.Height);
            my_listBox = Selected_scps_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Buttons, CheckBoxes, ListBoxes and labels between SCP listBoxes
            int height_of_each_row_between_scpListBoxes = (int)Math.Round(0.333F * (Selected_scps_listBox.Location.Y - Mbco_scps_listBox.Location.Y - Mbco_scps_listBox.Height));

            bottom_referenceBorder = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height;

            left_referenceBorder = Mbco_scps_listBox.Location.X;
            right_referenceBorder = (int)Math.Round(0.25F*this.Selection_panel.Width);

            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + height_of_each_row_between_scpListBoxes;
            my_button = this.Add_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + height_of_each_row_between_scpListBoxes;
            my_button = this.Remove_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            #region Include ancestors and offspring cbButtons and label
            int shared_cbButton_widthHeight = (int)Math.Round((float)2/(float)3 * (float)height_of_each_row_between_scpListBoxes);
            int shared_left_referenceBorder_ancestorOffspring = Add_button.Location.X + Add_button.Width + (int)Math.Round(0.05F * Overall_panel.Width);
            int shared_right_referenceBorder_ancestorOffspring = (int)Math.Round(0.7F * Selection_panel.Width);

            left_referenceBorder = shared_left_referenceBorder_ancestorOffspring;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            top_referenceBorder = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height + shared_cbButton_widthHeight;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight; 
            my_cbButton = IncludeAncestorSCPs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = IncludeAncestorSCPs_cbButton.Location.X + IncludeAncestorSCPs_cbButton.Width;
            right_referenceBorder = shared_right_referenceBorder_ancestorOffspring;
            my_label = IncludeAncestorSCPs_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_left_referenceBorder_ancestorOffspring;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight;
            my_cbButton = IncludeOffspringSCPs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = IncludeOffspringSCPs_cbButton.Location.X + IncludeOffspringSCPs_cbButton.Width;
            right_referenceBorder = shared_right_referenceBorder_ancestorOffspring;
            my_label = IncludeOffspringSCPs_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion


            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = this.Selection_panel.Width;
            top_referenceBorder = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height + height_of_each_row_between_scpListBoxes;
            bottom_referenceBorder = top_referenceBorder + height_of_each_row_between_scpListBoxes;
            my_listBox = SortSCPs_listBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Add_button.Location.X + Add_button.Width;
            right_referenceBorder = IncludeOffspringSCPs_cbButton.Location.X;
            top_referenceBorder = IncludeAncestorSCPs_cbButton.Location.Y;
            bottom_referenceBorder = IncludeOffspringSCPs_cbButton.Location.Y + IncludeOffspringSCPs_cbButton.Height;
            my_label = IncludeBracket_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Add_button.Location.X + Add_button.Width;
            right_referenceBorder = SortSCPs_listBox.Location.X;
            top_referenceBorder = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height;
            bottom_referenceBorder = IncludeAncestorSCPs_cbButton.Location.Y;
            my_label = IncludeHeadline_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = SortSCPs_listBox.Location.X;
            right_referenceBorder = Selection_panel.Width;
            top_referenceBorder = Mbco_scps_listBox.Location.Y + Mbco_scps_listBox.Height;
            bottom_referenceBorder = SortSCPs_listBox.Location.Y;
            my_label = SortSCPs_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = Selection_panel.Width;
            top_referenceBorder = IncludeOffspringSCPs_cbButton.Location.Y + IncludeOffspringSCPs_cbButton.Height;
            bottom_referenceBorder = Selected_scps_listBox.Location.Y;
            my_label = SelectedGroup_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region CheckBoxes and buttons below selectSCP listBox
            int height_of_each_row_below_selectedSCPs_listBox = (int)Math.Round(0.5F * (this.Selection_panel.Height - this.Selected_scps_listBox.Location.Y - this.Selected_scps_listBox.Height));

            left_referenceBorder = (int)Math.Round(0.6F * this.Selection_panel.Width);
            right_referenceBorder = Selected_scps_listBox.Location.X + Selected_scps_listBox.Width;
            top_referenceBorder = Selected_scps_listBox.Location.Y + Selected_scps_listBox.Height + height_of_each_row_below_selectedSCPs_listBox;
            bottom_referenceBorder = Selection_panel.Height;
            my_button = Write_mbcoHierarchy_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Selected_scps_listBox.Location.X;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            top_referenceBorder = Selected_scps_listBox.Location.Y + Selected_scps_listBox.Height;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight;
            my_cbButton = Show_onlySelectedScps_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Show_onlySelectedScps_cbButton.Location.X + Show_onlySelectedScps_cbButton.Width;
            right_referenceBorder = Selected_scps_listBox.Location.X + Selected_scps_listBox.Width;
            my_label = Show_onlySelectedScps_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Selected_scps_listBox.Location.X;
            top_referenceBorder = bottom_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight;
            my_cbButton = AddGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = AddGenes_cbButton.Location.X + AddGenes_cbButton.Width;
            right_referenceBorder = Write_mbcoHierarchy_button.Location.X;
            my_label = AddGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            Update_mbco_scps = true;
            Update_selected_scps = true;
        }

        public void Set_to_visible(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_repor_label, bool update_scp_windows)
        {
            if (update_scp_windows)
            {
                this.Update_mbco_scps = true;
                this.Update_selected_scps = true;
            }
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_repor_label);
            this.Overall_panel.Visible = true;
        }

        private void Update_visibilities_of_checkBoxes_and_buttons(MBCO_enrichment_pipeline_options_class options)
        {
            AddGenes_cbButton.Visible = Show_onlySelectedScps_cbButton.Checked;
            AddGenes_cbLabel.Visible = AddGenes_cbButton.Visible;
            string[] scpGroups = options.Group_selectedScps_dict.Keys.ToArray();
            if (scpGroups.Length==0)
            {
                Selection_panel.Visible=false;
            }
            else
            {
                Selection_panel.Visible = true;
                if (options.Is_at_least_one_scp_selected_as_part_of_any_group())
                {
                    Show_onlySelectedScps_cbButton.Visible = true;
                    Show_onlySelectedScps_cbLabel.Visible = true;
                }
                else
                {
                    Show_onlySelectedScps_cbButton.Visible = false;
                    Show_onlySelectedScps_cbLabel.Visible = false;
                }
                Show_onlySelectedScps_cbButton.Refresh();
                AddGenes_cbButton.Refresh();
            }
            bool scps_in_mbco_listBox_selected = false;
            bool scps_in_selected_listBox_selected = false;

            if (Mbco_scps_listBox.SelectedItems.Count > 0) { scps_in_mbco_listBox_selected = true; }
            if (Selected_scps_listBox.SelectedItems.Count > 0) { scps_in_selected_listBox_selected = true; }
            if (scps_in_mbco_listBox_selected) { Add_button.Visible = true; }
            else { Add_button.Visible = false; }
            if (scps_in_selected_listBox_selected) { Remove_button.Visible = true; }
            else { Remove_button.Visible= false; }
            if ((scps_in_mbco_listBox_selected)||(scps_in_selected_listBox_selected))
            {
                IncludeHeadline_label.Visible = true;
                IncludeBracket_label.Visible = true;
                IncludeAncestorSCPs_cbButton.Visible = true;
                IncludeAncestorSCPs_cbLabel.Visible = true;
                IncludeOffspringSCPs_cbButton.Visible = true;
                IncludeOffspringSCPs_cbLabel.Visible = true;
                IncludeOffspringSCPs_cbButton.BringToFront();
                IncludeAncestorSCPs_cbButton.BringToFront();
                Remove_button.BringToFront();
                Add_button.BringToFront();
            }
            else 
            {
                IncludeHeadline_label.Visible = false;
                IncludeBracket_label.Visible = false;
                IncludeAncestorSCPs_cbButton.Visible = false;
                IncludeAncestorSCPs_cbLabel.Visible = false;
                IncludeOffspringSCPs_cbButton.Visible = false;
                IncludeOffspringSCPs_cbLabel.Visible = false;
            }
        }

        private void Update_scpGroupListBox_and_selected_group_label(MBCO_enrichment_pipeline_options_class options)
        {
            int scpGroups_length = Groups_ownListBox.Items.Count;
            string[] scpGroups = new string[scpGroups_length];
            for (int indexScpGroup = 0; indexScpGroup < scpGroups_length; indexScpGroup++)
            {
                scpGroups[indexScpGroup] = Groups_ownListBox.Items[indexScpGroup].ToString();
            }
            string[] updated_scpGroups = options.Group_selectedScps_dict.Keys.ToArray();
            if (updated_scpGroups.Length != scpGroups_length)
            {
                updated_scpGroups = updated_scpGroups.OrderBy(l => l).ToArray();
                Groups_ownListBox.Items.Clear();
                Groups_ownListBox.Items.AddRange(updated_scpGroups);
                string[] new_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(updated_scpGroups, scpGroups);
                if (new_scps.Length>0)
                {
                    Groups_ownListBox.SilentSelectedIndex = Groups_ownListBox.Items.IndexOf(new_scps[0]);
                }
                else if (Groups_ownListBox.Items.Count > 0)
                {
                    Groups_ownListBox.SilentSelectedIndex = 0;
                }
            }
        }

        private void Check_if_ontologies_match(MBCO_enrichment_pipeline_options_class options)
        {
            Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(options.Next_ontology);
            if (!Mbco_parent_child_network.Ontology.Equals(human_ontology)) { throw new Exception(); }
            if (!Mbco_child_parent_network.Ontology.Equals(human_ontology)) { throw new Exception(); }
        }

        private void Update_mbcoSCPs_and_selectedSCPs_listBox(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_report_label)
        {
            Check_if_ontologies_match(options);

            Show_onlySelectedScps_cbButton.SilentChecked = options.Show_all_and_only_selected_scps;
            AddGenes_cbButton.SilentChecked = network_options.Add_genes_to_standard_networks;
            Update_scpGroupListBox_and_selected_group_label(options);

            if (Update_mbco_scps)
            {
                string[] add_mbco_scps = new string[0];
                switch (SortSCPs_listBox.SelectedItem)
                {
                    case Form1_shared_text_class.Sort_alphabetically_text:
                        string[] all_scps = Mbco_parent_child_network.Get_all_scps();
                        string[] all_custom_scps = options.OwnScp_level_dict.Keys.ToArray();
                        add_mbco_scps = Overlap_class.Get_union(all_scps, all_custom_scps);
                        add_mbco_scps = add_mbco_scps.OrderBy(l => l).ToArray();
                        break;
                    case Form1_shared_text_class.Sort_byLevel_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_all_scps_sorted_by_level_with_level_announcing_headlines(options.OwnScp_level_dict);
                        break;
                    case Form1_shared_text_class.Sort_byLevelParentScp_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_all_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(options.OwnScp_level_dict);
                        break;
                    default:
                        throw new Exception();
                }
                Mbco_scps_listBox.Items.Clear();
                Mbco_scps_listBox.Items.AddRange(add_mbco_scps);
                Update_mbco_scps = false;
            }
            if (Update_selected_scps)
            {
                string[] selected_scps = new string[0];
                if (Groups_ownListBox.Items.Count > 0)
                {
                    string current_scpGroup = Groups_ownListBox.SelectedItem.ToString();
                    selected_scps = options.Group_selectedScps_dict[current_scpGroup];
                    this.SelectedGroup_label.Text = current_scpGroup;
                    SelectedGroup_label.Refresh();
                }
                string[] own_scps = options.OwnScp_level_dict.Keys.ToArray();
                string[] selected_own_scps = Overlap_class.Get_intersection(selected_scps, own_scps);
                Dictionary<string, int> selected_ownSCP_level_dict = new Dictionary<string, int>();
                foreach (string selected_own_scp in selected_own_scps)
                {
                    selected_ownSCP_level_dict.Add(selected_own_scp, options.OwnScp_level_dict[selected_own_scp]);
                }
                string[] add_selected_scps = new string[0];
                switch (SortSCPs_listBox.SelectedItem)
                {
                    case Form1_shared_text_class.Sort_alphabetically_text:
                        add_selected_scps = selected_scps.OrderBy(l => l).ToArray();
                        break;
                    case Form1_shared_text_class.Sort_byLevel_text:
                        add_selected_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_with_level_announcing_headlines(false, selected_ownSCP_level_dict, selected_scps);
                        break;
                    case Form1_shared_text_class.Sort_byLevelParentScp_text:
                        add_selected_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(false, selected_ownSCP_level_dict, selected_scps);
                        break;
                    default:
                        throw new Exception();
                }
                Selected_scps_listBox.Items.Clear();
                Selected_scps_listBox.Items.AddRange(add_selected_scps.ToArray());
                Update_selected_scps = false;
            }
            Update_visibilities_of_checkBoxes_and_buttons(options);
        }
        public MBCO_enrichment_pipeline_options_class Add_groupButton_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            string newGroup = NewGroup_ownTextBox.Text.ToString().Replace("$","-");
            if (!options.Group_selectedScps_dict.ContainsKey(newGroup))
            {
                options.Group_selectedScps_dict_add(newGroup, new string[0]);
            }
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_report_label);
            return options;
        }
        private MBCO_enrichment_pipeline_options_class Update_pipeline_and_network_options(MBCO_enrichment_pipeline_options_class options, ref bool add_genes_to_standard_network)
        {
            if (!options.Is_at_least_one_scp_selected_as_part_of_any_group())
            {
                options.Show_all_and_only_selected_scps = false;
                add_genes_to_standard_network = false;
            }
            return options;
        }
        public MBCO_enrichment_pipeline_options_class Remove_groupButton_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, ref bool add_genes_to_standard_network, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            if (Groups_ownListBox.Items.Count > 0)
            {
                string removeGroup = Groups_ownListBox.SelectedItem.ToString();
                Dictionary<string, string[]> group_scps = new Dictionary<string, string[]>();
                string[] groups = options.Group_selectedScps_dict.Keys.ToArray();
                foreach (string group in groups)
                {
                    if (!group.Equals(removeGroup))
                    {
                        group_scps.Add(group,options.Group_selectedScps_dict[group]);
                    }
                }
                options.Group_selectedScps_dict = group_scps;
                Update_selected_scps = true;
                Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_report_label);
                Update_pipeline_and_network_options(options, ref add_genes_to_standard_network);
            }
            return options;
        }
        public void Groups_ownListBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_report_label)
        {
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_report_label);
        }

        public MBCO_enrichment_pipeline_options_class Add_button_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            string current_scpGroup = Groups_ownListBox.SelectedItem.ToString();
            int add_scps_length = Mbco_scps_listBox.SelectedItems.Count;
            string[] add_to_selected_scps = new string[add_scps_length];
            for (int indexS = 0; indexS < add_scps_length; indexS++)
            {
                add_to_selected_scps[indexS] = (string)Mbco_scps_listBox.SelectedItems[indexS];
            }
            string[] mbco_scps = Mbco_parent_child_network.Get_all_scps();
            string[] ancestor_scps = new string[0];
            string[] offspring_scps = new string[0];
            if (IncludeAncestorSCPs_cbButton.Checked)
            {
                ancestor_scps = Mbco_child_parent_network.Get_all_ancestors_if_direction_is_child_parent(add_to_selected_scps);
            }
            if (IncludeOffspringSCPs_cbButton.Checked)
            {
                offspring_scps = Mbco_parent_child_network.Get_all_offspring_if_direction_is_parent_child(add_to_selected_scps);
            }
            if (ancestor_scps.Length > 0) { add_to_selected_scps = Overlap_class.Get_union(add_to_selected_scps, ancestor_scps); }
            if (offspring_scps.Length > 0) { add_to_selected_scps = Overlap_class.Get_union(add_to_selected_scps, offspring_scps); }

            string[] defined_scps = options.OwnScp_mbcoSubScps_dict.Keys.ToArray();
            string[] all_scps = Overlap_class.Get_union(mbco_scps, defined_scps);
            add_to_selected_scps = Overlap_class.Get_intersection(all_scps, add_to_selected_scps);
            options.Group_selectedScps_dict[current_scpGroup] = Overlap_class.Get_union(options.Group_selectedScps_dict[current_scpGroup], add_to_selected_scps);
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_report_label);
            Mbco_scps_listBox.SelectedItems.Clear();
            Add_button.Visible = false;
            return options;
        }

        public MBCO_enrichment_pipeline_options_class Remove_button_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, ref bool add_genes_to_standard_network, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            string current_scpGroup = Groups_ownListBox.SelectedItem.ToString();
            int remove_scps_length = Selected_scps_listBox.SelectedItems.Count;
            string[] remove_from_selected_scps = new string[remove_scps_length];
            for (int indexS=0; indexS < remove_scps_length; indexS++)
            {
                remove_from_selected_scps[indexS] = (string)Selected_scps_listBox.SelectedItems[indexS];
            }
            string[] ancestor_scps = new string[0];
            string[] offspring_scps = new string[0];
            if (IncludeAncestorSCPs_cbButton.Checked)
            {
                ancestor_scps = Mbco_child_parent_network.Get_all_ancestors_if_direction_is_child_parent(remove_from_selected_scps);
            }
            if (IncludeOffspringSCPs_cbButton.Checked)
            {
                offspring_scps = Mbco_parent_child_network.Get_all_offspring_if_direction_is_parent_child(remove_from_selected_scps);
            }
            if (ancestor_scps.Length > 0) { remove_from_selected_scps = Overlap_class.Get_union(remove_from_selected_scps, ancestor_scps); }
            if (offspring_scps.Length > 0) { remove_from_selected_scps = Overlap_class.Get_union(remove_from_selected_scps, offspring_scps); }
            options.Group_selectedScps_dict[current_scpGroup] = Overlap_class.Get_part_of_list1_but_not_of_list2(options.Group_selectedScps_dict[current_scpGroup], remove_from_selected_scps);
            if (!options.Is_at_least_one_scp_selected_as_part_of_any_group()) 
            { 
                options.Show_all_and_only_selected_scps = false;
                network_options.Add_genes_to_standard_networks = false;
            }
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_report_label);
            Update_pipeline_and_network_options(options, ref add_genes_to_standard_network);
            Selected_scps_listBox.SelectedItems.Clear();
            Remove_button.Visible = false;
            return options;
        }
        public void SortSCPs_listBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_report_label)
        {
            Check_if_ontologies_match(options);
            Update_mbco_scps = true;
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_report_label);
        }
        public void MBCO_listBox_changed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_repor_label)
        {
            Check_if_ontologies_match(options);
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_repor_label);
        }
        public void Selected_listBox_changed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, Label error_repor_label)
        {
            Check_if_ontologies_match(options);
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options, error_repor_label);
        }
        public MBCO_enrichment_pipeline_options_class ShowOnlySelectedScps_checkBox_CheckedChanged(MBCO_enrichment_pipeline_options_class options, out bool add_genes_to_network)
        {
            if (options.Is_at_least_one_scp_selected_as_part_of_any_group())
            {
                options.Show_all_and_only_selected_scps = Show_onlySelectedScps_cbButton.Checked;
            }
            else if (Show_onlySelectedScps_cbButton.Checked)
            {
                Show_onlySelectedScps_cbButton.SilentChecked = false;
                Selected_scps_listBox.Items.Add(No_selected_scps_message);
                options.Show_all_and_only_selected_scps = Show_onlySelectedScps_cbButton.Checked;
            }
            add_genes_to_network = options.Show_all_and_only_selected_scps;
            AddGenes_cbButton.SilentChecked = add_genes_to_network;
            Update_visibilities_of_checkBoxes_and_buttons(options);
            return options;
        }

        public MBCO_network_based_integration_options_class AddGenes_checkBox_CheckedChanged(MBCO_network_based_integration_options_class network_options)
        {
            network_options.Add_genes_to_standard_networks = AddGenes_cbButton.Checked;
            return network_options;
        }

        public void Write_mbco_yed_network(Label progress_report)
        {
            Common_functions.Global_definitions.Global_directory_and_file_class global_dirFile = new Common_functions.Global_definitions.Global_directory_and_file_class();
            progress_report.Text = "Write MBCO hierarchy into " + global_dirFile.Results_directory + "MBCO_hierarchy" + global_dirFile.Delimiter + "\r\nOpen with graph editor (e.g. yED graph editor, here use layout circular)";
            progress_report.Visible = true;
            progress_report.Refresh();
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            string ontology_string = Ontology_classification_class.Get_name_of_scps_for_progress_report(Mbco_parent_child_network.Ontology);
            Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted("MBCO_hierarchy\\", "Hierarchy of " + ontology_string, yed_network.Shape_enum.Rectangle, progress_report, this.Form_default_settings);
            System.Threading.Thread.Sleep(8000);
            progress_report.Text = "";
            progress_report.Refresh();
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
    }
}
