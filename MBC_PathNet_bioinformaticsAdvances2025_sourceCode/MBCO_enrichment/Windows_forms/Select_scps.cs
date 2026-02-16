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
using System.Drawing;
using Windows_forms_customized_tools;
using System.Windows.Forms;
using Common_functions.Array_own;
using Network;
using Enrichment;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using MBCO;
using Result_visualization;
using Windows_forms;
using System.Windows.Controls;
using PdfSharp.Pdf.Content.Objects;
using System.Windows.Documents;
using System.Windows;
using ZedGraph;
using yed_network;
using System.Net.NetworkInformation;

namespace ClassLibrary1.Select_scps_userInterface
{
    class Select_scps_userInterface_class
    {
        const string No_selected_scps_message = "No SCPs selected";
        private MyPanel Overall_panel { get; set; }
        private System.Windows.Forms.Label OverallHeadline_label { get; set; }
        private System.Windows.Forms.Label NewGroup_label { get; set; }
        private OwnTextBox NewGroup_ownTextBox { get; set; }
        private System.Windows.Forms.Button AddGroup_button { get; set; }
        private System.Windows.Forms.Button RemoveGroup_button { get; set; }
        private System.Windows.Forms.Label Groups_label {get;set;}
        private OwnListBox Groups_ownListBox { get; set; }
        private MyPanel Selection_panel { get; set; }
        private OwnListBox Mbco_scps_listBox { get; set; }
        private System.Windows.Forms.Label SelectedGroup_label { get; set; }
        private OwnListBox Selected_scps_listBox { get; set; }
        private System.Windows.Forms.Label SortSCPs_label { get; set; }
        private OwnListBox SortSCPs_listBox { get; set; }
        private System.Windows.Forms.Label IncludeHeadline_label { get; set; }
        private System.Windows.Forms.Label IncludeBracket_label { get; set; }
        private MyCheckBox_button IncludeOffspringSCPs_cbButton { get; set; }
        private System.Windows.Forms.Label IncludeOffspringSCPs_cbLabel { get; set; }
        private MyCheckBox_button IncludeAncestorSCPs_cbButton{ get; set; }
        private System.Windows.Forms.Label IncludeAncestorSCPs_cbLabel { get; set; }
        private bool Update_mbco_scps { get; set; }
        private bool Update_selected_scps { get; set; }
        private System.Windows.Forms.Button Add_button { get; set; }
        private System.Windows.Forms.Button Remove_button { get; set; }
        private System.Windows.Forms.Button Write_mbcoHierarchy_button { get; set; }
        private MyCheckBox_button Show_onlySelectedScps_cbButton { get; set; }
        private System.Windows.Forms.Label Show_onlySelectedScps_cbLabel { get; set; }
        private MyCheckBox_button AddGenes_cbButton { get; set; }
        private System.Windows.Forms.Label AddGenes_cbLabel { get; set; }
        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }
        private MBCO_obo_network_class Mbco_child_parent_network { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private ProgressReport_interface_class ProgressReport { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }
        private System.Windows.Forms.Button Tutorial_button { get; set; }



        public Select_scps_userInterface_class(MyPanel overall_panel,
                                               System.Windows.Forms.Label overallHeadline_label,
                                               System.Windows.Forms.Label newGroup_label,
                                               OwnTextBox newGroup_ownTextBox,
                                               System.Windows.Forms.Button addGroup_button,
                                               System.Windows.Forms.Button removeGroup_button,
                                               System.Windows.Forms.Label groups_label,
                                               OwnListBox groups_ownListBox,
                                               MyPanel selection_panel,
                                               OwnListBox mbco_scps_listBox,
                                               System.Windows.Forms.Label selectedGroup_label,
                                               OwnListBox selected_scps_listBox,
                                               System.Windows.Forms.Label sortSCPs_label,
                                               OwnListBox sortSCPs_ownListBox,
                                               System.Windows.Forms.Button add_button,
                                               System.Windows.Forms.Button remove_button,
                                               System.Windows.Forms.Label includeHeadline_label,
                                               System.Windows.Forms.Label includeBracket_label,
                                               MyCheckBox_button includeOffspringSCPs_cbButton,
                                               System.Windows.Forms.Label includeOffspringSCPs_cbLabel,
                                               MyCheckBox_button includeAncestorSCPs_cbButton,
                                               System.Windows.Forms.Label includeAncestorSCPs_cbLabel,
                                               System.Windows.Forms.Button write_mbcoHierarchy_button,
                                               MyCheckBox_button show_onlySelectedScps_cbButton,
                                               System.Windows.Forms.Label show_onlySelectedScps_cbLabel,
                                               MyCheckBox_button addGenes_cbButton,
                                               System.Windows.Forms.Label addGenes_cbLabel,
                                               ProgressReport_interface_class progressReport,
                                               Tutorial_interface_class userInterface_tutorial,
                                               System.Windows.Forms.Button tutorial_button,
                                               Form1_default_settings_class form_default_settings,
                                               MBCO_enrichment_pipeline_options_class mbco_options,
                                               MBCO_obo_network_class parent_child_nw
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
            this.ProgressReport = progressReport;
            this.UserInterface_tutorial = userInterface_tutorial;
            this.Tutorial_button = tutorial_button;

            this.IncludeOffspringSCPs_cbButton.Checked = false;
            this.IncludeAncestorSCPs_cbButton.Checked = false;

            Update_all_graphic_elements();
            Update_mbco_parent_child_and_child_parent_networks_and_set_to_default(parent_child_nw);
        }

        private void Set_to_default_under_consideration_of_selected_ontology()
        {
            Add_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Add_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Remove_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Remove_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            AddGroup_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            AddGroup_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            RemoveGroup_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            RemoveGroup_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;


            Add_button.Visible = false;
            Remove_button.Visible = false;
            IncludeOffspringSCPs_cbButton.SilentChecked = false;
            IncludeOffspringSCPs_cbButton.SilentChecked = false;
            SortSCPs_listBox.Items.Clear();
            SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_alphabetically_text);
            switch (Mbco_child_parent_network.Ontology)
            {
                case Ontology_type_enum.Mbco:
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevelParentScp_text);
                    SortSCPs_listBox.SilentSelectedIndex = 1;
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byDepth_text);
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevelParentScp_text);
                    SortSCPs_listBox.SilentSelectedIndex = 1;
                    break;
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byDepth_text);
                    SortSCPs_listBox.Items.Add(Form1_shared_text_class.Sort_byLevel_text);
                    SortSCPs_listBox.SilentSelectedIndex = 0;
                    break;
                default:
                    throw new Exception();
            }
            NewGroup_ownTextBox.Text = "Selected SCPs";
        }
        public void Update_mbco_parent_child_and_child_parent_networks_and_set_to_default(MBCO_obo_network_class parentChild_nw)
        {
            if (!parentChild_nw.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }
            if (!parentChild_nw.Scp_hierarchal_interactions.Equals(SCP_hierarchy_interaction_type_enum.Parent_child)) { throw new Exception(); }
            Mbco_parent_child_network = parentChild_nw.Deep_copy_mbco_obo_nw();
            if (Ontology_classification_class.Is_go_ontology(Mbco_parent_child_network.Ontology))
            {
                Mbco_parent_child_network.Keep_only_scps_of_selected_namespace_if_gene_ontology();
            }

            Mbco_child_parent_network = Mbco_parent_child_network.Deep_copy_mbco_obo_nw();
            Mbco_child_parent_network.Transform_into_child_parent_direction();

            Update_mbco_scps = true;
            Update_selected_scps = true;
            Set_to_default_under_consideration_of_selected_ontology();
        }

        public void Update_all_graphic_elements()
        { 
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            MyPanel my_panel;
            System.Windows.Forms.Label my_label;
            OwnTextBox my_textBox;
            MyCheckBox_button my_cbButton;
            OwnListBox my_listBox;
            System.Windows.Forms.Button my_button;

            Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            #region Overall panel textBoxes, listBoxes and buttons
            int shared_textBox_listBox_height_overallPanel = (int)Math.Round(0.05 * Overall_panel.Height);
            int shared_betweenDistanceHeight_textBox_listBox_height_overallPanel = (int)Math.Round(0.005 * Overall_panel.Height);
            int shared_distanceFrom_leftRightSides_overallPanel = (int)Math.Round(0.01F * Overall_panel.Width);

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
            bottom_referenceBorder = (int)Math.Round(0.955F * Overall_panel.Height);
            my_panel = this.Selection_panel;
            Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region MBCO SCPs and selected SCPs listBoxes
            left_referenceBorder = 0;
            right_referenceBorder = this.Selection_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = (int)Math.Round(0.39F * this.Selection_panel.Height);
            my_listBox = Mbco_scps_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.Selection_panel.Width;
            top_referenceBorder = (int)Math.Round(0.58F * this.Selection_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.965F * this.Selection_panel.Height);
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
            this.AddGroup_button.Text = "Add";
            my_button = this.Add_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + height_of_each_row_between_scpListBoxes;
            this.RemoveGroup_button.Text = "Remove";
            my_button = this.Remove_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            #region Include ancestors and offspring cbButtons and label
            int shared_cbButton_widthHeight = (int)Math.Round((float)2/(float)2.5F * (float)height_of_each_row_between_scpListBoxes);
            int shared_left_referenceBorder_ancestorOffspring = Add_button.Location.X + Add_button.Width;// + (int)Math.Round(0.00F * Overall_panel.Width);
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

            Update_selectedGroup_label_size();
            #endregion

            #region CheckBoxes and buttons below selectSCP listBox
            int height_of_each_row_below_selectedSCPs_listBox = (int)Math.Round(0.5F * (this.Selection_panel.Height - this.Selected_scps_listBox.Location.Y - this.Selected_scps_listBox.Height));

            left_referenceBorder = Selected_scps_listBox.Location.X;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            top_referenceBorder = Selected_scps_listBox.Location.Y + Selected_scps_listBox.Height;
            bottom_referenceBorder = Math.Min(Selection_panel.Height, top_referenceBorder + shared_cbButton_widthHeight);
            my_cbButton = Show_onlySelectedScps_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.7F * this.Selection_panel.Width);
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            my_cbButton = AddGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Show_onlySelectedScps_cbButton.Location.X + Show_onlySelectedScps_cbButton.Width;
            right_referenceBorder = AddGenes_cbButton.Location.X;
            my_label = Show_onlySelectedScps_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = AddGenes_cbButton.Location.X + AddGenes_cbButton.Width;
            right_referenceBorder = Selection_panel.Width;
            my_label = AddGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Tutorial and write MBCO hierarchy button
            int hierarchyTutorialButtons_shared_distanceFrom_topBottom_border = (int)Math.Round(0.001F * Overall_panel.Height);
            int hierarchyTutorialButtons_shared_distanceFrom_rightLeft_border = (int)Math.Round(0.2F * Overall_panel.Width);
            int hierarchyTutorialButtons_inBetween_halfDistance = (int)Math.Round(0.0025F * Overall_panel.Width);
            int hierarchyTutorialButtons_button_width = (int)Math.Round(0.3F * this.Overall_panel.Width);


            top_referenceBorder = this.Selection_panel.Location.Y + this.Selection_panel.Height + hierarchyTutorialButtons_shared_distanceFrom_topBottom_border;
            bottom_referenceBorder = Overall_panel.Height - hierarchyTutorialButtons_shared_distanceFrom_topBottom_border;
            right_referenceBorder = this.Overall_panel.Width - shared_distanceFrom_leftRightSides_overallPanel;
            left_referenceBorder = right_referenceBorder - hierarchyTutorialButtons_button_width;
            //if (Form_default_settings.Is_mono)
            //{
            //top_referenceBorder -= (int)Math.Round(0.015 * Selection_panel.Height);//0.01
            //bottom_referenceBorder -= (int)Math.Round(0.01 * Selection_panel.Height);
            //}
            my_button = Write_mbcoHierarchy_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            right_referenceBorder = left_referenceBorder - hierarchyTutorialButtons_inBetween_halfDistance * 2;
            left_referenceBorder = right_referenceBorder - hierarchyTutorialButtons_button_width;
            //right_referenceBorder = (int)Math.Round(0.5F * this.Selection_panel.Width) - hierarchyTutorialButtons_inBetween_halfDistance;
            //if (Form_default_settings.Is_mono)
            //{
            //top_referenceBorder -= (int)Math.Round(0.015 * Selection_panel.Height);//0.01
            //bottom_referenceBorder -= (int)Math.Round(0.01 * Selection_panel.Height);
            //}
            Tutorial_button.Text = "Tour";
            my_button = Tutorial_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            Update_mbco_scps = true;
            Update_selected_scps = true;
        }
        public bool Are_parentChild_hierarchy_networks_upToDate(MBCO_enrichment_pipeline_options_class options)
        {
            return (Mbco_parent_child_network.Ontology.Equals(options.Next_ontology))
                   && (Mbco_parent_child_network.Organism.Equals(options.Next_organism))
                   && (Mbco_child_parent_network.Ontology.Equals(options.Next_ontology))
                   && (Mbco_child_parent_network.Organism.Equals(options.Next_organism));
        }

        public void Set_to_visible(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, bool update_scp_windows)
        {
            Check_if_ontologies_match(options);
            if (update_scp_windows)
            {
                this.Update_mbco_scps = true;
                this.Update_selected_scps = true;
            }
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
            this.Overall_panel.Visible = true;
            this.Overall_panel.Refresh();
        }

        private void Update_selectedGroup_label_size()
        {
            int left_referenceBorder = 0;
            int right_referenceBorder = Selection_panel.Width;
            int top_referenceBorder = IncludeOffspringSCPs_cbButton.Location.Y + IncludeOffspringSCPs_cbButton.Height;
            int bottom_referenceBorder = Selected_scps_listBox.Location.Y;
            System.Windows.Forms.Label my_label = SelectedGroup_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
        }

        private void Update_visibilities_of_checkBoxes_and_buttons(MBCO_enrichment_pipeline_options_class options)
        {
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
                    if (Show_onlySelectedScps_cbButton.Checked)
                    {
                        AddGenes_cbButton.Visible = true;
                        AddGenes_cbLabel.Visible = true;
                    }
                    else
                    {
                        AddGenes_cbButton.Visible = false;
                        AddGenes_cbLabel.Visible = false;
                    }
                }
                else
                {
                    Show_onlySelectedScps_cbButton.Visible = false;
                    Show_onlySelectedScps_cbLabel.Visible = false;
                    AddGenes_cbButton.Visible = false;
                    AddGenes_cbLabel.Visible = false;
                }
                Show_onlySelectedScps_cbButton.Refresh();
                AddGenes_cbButton.Refresh();
                AddGenes_cbLabel.Refresh();
                Selection_panel.Refresh();
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
                IncludeOffspringSCPs_cbButton.Refresh();
                IncludeAncestorSCPs_cbButton.Refresh();
                Remove_button.Refresh();
                Add_button.Refresh();
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
            Groups_ownListBox.Refresh();
        }

        private void Check_if_ontologies_match(MBCO_enrichment_pipeline_options_class options)
        {
            if (!Mbco_parent_child_network.Ontology.Equals(options.Next_ontology)) { throw new Exception("Current ans next onlogy don't match in Parent child network"); }
            if (!Mbco_child_parent_network.Ontology.Equals(options.Next_ontology)) { throw new Exception("Current ans next onlogy don't match in Chil parent network"); }
        }

        private void Update_mbcoSCPs_and_selectedSCPs_listBox(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
        {
            Check_if_ontologies_match(options);

            Show_onlySelectedScps_cbButton.SilentChecked = options.Show_all_and_only_selected_scps;
            AddGenes_cbButton.SilentChecked = network_options.Add_genes_to_standard_networks;
            Update_scpGroupListBox_and_selected_group_label(options);



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
                switch (SortSCPs_listBox.SelectedItem)
                {
                    case Form1_shared_text_class.Sort_alphabetically_text:
                        string[] all_custom_scps = options.OwnScp_level_dict.Keys.ToArray();
                        add_mbco_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(all_scps, all_custom_scps);
                        add_mbco_scps = add_mbco_scps.OrderBy(l => l).ToArray();
                        break;
                    case Form1_shared_text_class.Sort_byLevel_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_with_level_announcing_headlines(false, options.OwnScp_level_dict, all_scps);
                        break;
                    case Form1_shared_text_class.Sort_byDepth_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_depth_with_depth_announcing_headlines(false, options.OwnScp_level_dict, all_scps);
                        break;
                    case Form1_shared_text_class.Sort_byLevelParentScp_text:
                        add_mbco_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(false, options.OwnScp_level_dict, all_scps);
                        break;
                    default:
                        throw new Exception(SortSCPs_listBox.SelectedItem + " is not considered in switch function");
                }
                Mbco_scps_listBox.Items.Clear();
                Mbco_scps_listBox.Items.AddRange(add_mbco_scps);
                Mbco_scps_listBox.Refresh();
                Update_mbco_scps = false;
                ProgressReport.Update_progressReport_text_and_visualization("");
            }
            if (Update_selected_scps)
            {
                ProgressReport.Update_progressReport_text_and_visualization("Updating SCP order in lower list box");
                string[] selected_scps = new string[0];
                if (Groups_ownListBox.Items.Count > 0)
                {
                    string current_scpGroup = Groups_ownListBox.SelectedItem.ToString();
                    selected_scps = options.Group_selectedScps_dict[current_scpGroup];
                    this.SelectedGroup_label.Text = current_scpGroup;
                    SelectedGroup_label.Refresh();
                    Update_selectedGroup_label_size();
                }
                string[] add_selected_scps = new string[0];
                if (selected_scps.Length > 0)
                {
                    string[] own_scps = options.OwnScp_level_dict.Keys.ToArray();
                    string[] selected_own_scps = Overlap_class.Get_ordered_intersection(selected_scps, own_scps);
                    Dictionary<string, int> selected_ownSCP_level_dict = new Dictionary<string, int>();
                    foreach (string selected_own_scp in selected_own_scps)
                    {
                        selected_ownSCP_level_dict.Add(selected_own_scp, options.OwnScp_level_dict[selected_own_scp]);
                    }
                    if (Ontology_classification_class.Is_go_ontology(options.Next_ontology))
                    {
                        int min_scp_size = options.Get_go_min_size(options.Next_ontology);
                        int max_scp_size = options.Get_go_max_size(options.Next_ontology);
                        selected_scps = Mbco_parent_child_network.Return_all_scps_meeting_minimum_and_maximum_size_criteria_if_specified(selected_scps, min_scp_size, max_scp_size);
                    }
                    switch (SortSCPs_listBox.SelectedItem)
                    {
                        case Form1_shared_text_class.Sort_alphabetically_text:
                            add_selected_scps = selected_scps.OrderBy(l => l).ToArray();
                            break;
                        case Form1_shared_text_class.Sort_byLevel_text:
                            add_selected_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_level_with_level_announcing_headlines(false, selected_ownSCP_level_dict, selected_scps);
                            break;
                        case Form1_shared_text_class.Sort_byLevelParentScp_text:
                            add_selected_scps = Mbco_child_parent_network.Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_child_parent(false, selected_ownSCP_level_dict, selected_scps);
                            break;
                        case Form1_shared_text_class.Sort_byDepth_text:
                            add_selected_scps = Mbco_parent_child_network.Get_input_scps_sorted_by_depth_with_depth_announcing_headlines(false, selected_ownSCP_level_dict, selected_scps);
                            break;
                        default:
                            throw new Exception();
                    }
                }
                Selected_scps_listBox.Items.Clear();
                Selected_scps_listBox.Items.AddRange(add_selected_scps.ToArray());
                Selected_scps_listBox.Refresh();
                Update_selected_scps = false;
                ProgressReport.Update_progressReport_text_and_visualization("");
            }
            Update_visibilities_of_checkBoxes_and_buttons(options);
        }
        public MBCO_enrichment_pipeline_options_class Add_groupButton_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
        {
            Check_if_ontologies_match(options);
            string newGroup = NewGroup_ownTextBox.Text.ToString().Replace("$","-");
            if (!options.Group_selectedScps_dict.ContainsKey(newGroup))
            {
                options.Group_selectedScps_dict_add(newGroup, new string[0]);
            }
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
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
        public MBCO_enrichment_pipeline_options_class Remove_groupButton_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, ref bool add_genes_to_standard_network)
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
                Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
                Update_pipeline_and_network_options(options, ref add_genes_to_standard_network);
            }
            return options;
        }
        public void Groups_ownListBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
        {
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
        }

        public MBCO_enrichment_pipeline_options_class Add_button_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
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
                Mbco_child_parent_network.Nodes.Order_by_nw_index();
                ancestor_scps = Mbco_child_parent_network.Get_all_ancestors_if_direction_is_child_parent_without_ordering_nodes_by_index(add_to_selected_scps);
            }
            if (IncludeOffspringSCPs_cbButton.Checked)
            {
                Mbco_parent_child_network.Nodes.Order_by_nw_index();
                offspring_scps = Mbco_parent_child_network.Get_all_descendents_if_direction_is_parent_child_without_ordering_nodes_by_index(add_to_selected_scps);
            }
            if (ancestor_scps.Length > 0) { add_to_selected_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(add_to_selected_scps, ancestor_scps); }
            if (offspring_scps.Length > 0) { add_to_selected_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(add_to_selected_scps, offspring_scps); }

            string[] defined_scps = options.OwnScp_mbcoSubScps_dict.Keys.ToArray();
            string[] all_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(mbco_scps, defined_scps);
            add_to_selected_scps = Overlap_class.Get_ordered_intersection(all_scps, add_to_selected_scps);
            options.Group_selectedScps_dict[current_scpGroup] = Overlap_class.Get_union_of_string_arrays_keeping_the_order(options.Group_selectedScps_dict[current_scpGroup], add_to_selected_scps);
            Mbco_scps_listBox.SelectedItems.Clear();
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
            Add_button.Visible = false;
            return options;
        }

        public MBCO_enrichment_pipeline_options_class Remove_button_pressed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options, ref bool add_genes_to_standard_network)
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
                Mbco_child_parent_network.Nodes.Order_by_nw_index();
                ancestor_scps = Mbco_child_parent_network.Get_all_ancestors_if_direction_is_child_parent_without_ordering_nodes_by_index(remove_from_selected_scps);
            }
            if (IncludeOffspringSCPs_cbButton.Checked)
            {
                Mbco_parent_child_network.Nodes.Order_by_nw_index();
                offspring_scps = Mbco_parent_child_network.Get_all_descendents_if_direction_is_parent_child_without_ordering_nodes_by_index(remove_from_selected_scps);
            }
            if (ancestor_scps.Length > 0) { remove_from_selected_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(remove_from_selected_scps, ancestor_scps); }
            if (offspring_scps.Length > 0) { remove_from_selected_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(remove_from_selected_scps, offspring_scps); }
            options.Group_selectedScps_dict[current_scpGroup] = Overlap_class.Get_part_of_list1_but_not_of_list2(options.Group_selectedScps_dict[current_scpGroup], remove_from_selected_scps);
            if (!options.Is_at_least_one_scp_selected_as_part_of_any_group()) 
            { 
                options.Show_all_and_only_selected_scps = false;
                network_options.Add_genes_to_standard_networks = false;
            }
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
            Update_pipeline_and_network_options(options, ref add_genes_to_standard_network);
            Selected_scps_listBox.SelectedItems.Clear();
            Remove_button.Visible = false;
            return options;
        }
        public void SortSCPs_listBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
        {
            Check_if_ontologies_match(options);
            Update_mbco_scps = true;
            Update_selected_scps = true;
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
        }
        public void MBCO_listBox_changed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
        {
            Check_if_ontologies_match(options);
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
        }
        public void Selected_listBox_changed(MBCO_enrichment_pipeline_options_class options, MBCO_network_based_integration_options_class network_options)
        {
            Check_if_ontologies_match(options);
            Update_mbcoSCPs_and_selectedSCPs_listBox(options, network_options);
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

        #region Tutorial
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
        public void Tutorial_button_pressed(MBCO_enrichment_pipeline_options_class enrich_options, MBCO_network_based_integration_options_class nw_options)
        {
            MBCO_enrichment_pipeline_options_class copy_enrich_options = enrich_options.Deep_copy();
            MBCO_network_based_integration_options_class copy_nw_options = nw_options.Deep_copy();
            copy_enrich_options.Clear_all_dictionaries_with_selected_scps_for_next_ontology();

            Update_mbcoSCPs_and_selectedSCPs_listBox(copy_enrich_options, copy_nw_options);

            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel = Overall_panel.Location.X - distance_from_overalMenueLabel;
            int mid_y_position;
            int right_x_position;
            string text;

            right_x_position = right_x_position_next_to_overall_panel;

            #region Extract current entries from text boxes and set buttons to defaults
            string newGroup_button_text = (string)NewGroup_ownTextBox.Text.Clone();
            bool show_onlySelectedScps_cbButton_checked = Show_onlySelectedScps_cbButton.Checked;
            bool addGenes_cbButton_checked = AddGenes_cbButton.Checked;
            bool add_ancestor_scps = IncludeAncestorSCPs_cbButton.Checked;
            bool add_descendent_scps = IncludeOffspringSCPs_cbButton.Checked;
            IncludeAncestorSCPs_cbButton.SilentChecked = false;
            IncludeOffspringSCPs_cbButton.SilentChecked = false;
            if (show_onlySelectedScps_cbButton_checked) { Show_onlySelectedScps_cbButton.SilentChecked = false; }
            if (addGenes_cbButton_checked) { AddGenes_cbButton.Checked = false; }
            int sortScps_listBox_selectedIndex = SortSCPs_listBox.SelectedIndex;
            string selected_scp_listBox_scp = "";
            if (Groups_ownListBox.Items.Count > 0)
            { selected_scp_listBox_scp = Groups_ownListBox.SelectedItem.ToString(); }
            #endregion



            bool is_mbco = Ontology_classification_class.Is_mbco_ontology(copy_enrich_options.Next_ontology);
            string pathway_term_name;
            string capital_pathway_term_name;
            string first_pathway_term_name;
            string my_scp_group;
            if (is_mbco)
            {
                pathway_term_name = "SCP";
                capital_pathway_term_name = "SCP";
                first_pathway_term_name = "subcellular processes (SCPs)";
                my_scp_group = "Mito energy ancestor/descendents";
            }
            else
            {
                pathway_term_name = "pathway";
                capital_pathway_term_name = "Pathway";
                first_pathway_term_name = "pathways";
                my_scp_group = "My pathway group";
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
                        text = "Grouping of " + first_pathway_term_name + " enables focused analysis of enrichment results limited to selected SCPs, independent of other predictions or significance cutoffs.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        copy_enrich_options.Group_selectedScps_dict_clear();
                        Update_visibilities_of_checkBoxes_and_buttons(copy_enrich_options);
                        Update_scpGroupListBox_and_selected_group_label(copy_enrich_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        mid_y_position = this.Overall_panel.Location.Y + (int)Math.Round((float)this.NewGroup_ownTextBox.Location.Y + 0.5F * this.NewGroup_ownTextBox.Height);
                        text = "Select a name for a new " + pathway_term_name + " group and press 'Add'.";
                        NewGroup_ownTextBox.SilentText_and_refresh = my_scp_group;
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        copy_enrich_options.Group_selectedScps_dict_clear();
                        copy_enrich_options = Add_groupButton_pressed(copy_enrich_options, copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.SortSCPs_listBox.Location.Y + 0.5F * this.SortSCPs_listBox.Height);
                        if (is_mbco) { text = "MBCO " + pathway_term_name + "s can be sorted alphabetically, by level or by level and parent " + pathway_term_name + "."; }
                        else { text = capital_pathway_term_name + " can be sorted alphabetically, by level (shortest distance from root " + pathway_term_name + ") or by depth (longest distance from root " + pathway_term_name + ")."; }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (is_mbco) { SortSCPs_listBox.SilentSelectedIndex = SortSCPs_listBox.Items.IndexOf(Form1_shared_text_class.Sort_byLevelParentScp_text); }
                        else { SortSCPs_listBox.SilentSelectedIndex = SortSCPs_listBox.Items.IndexOf(Form1_shared_text_class.Sort_byDepth_text); }
                        SortSCPs_listBox_SelectedIndexChanged(copy_enrich_options, copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 3:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Mbco_scps_listBox.Location.Y + 0.5F * this.Mbco_scps_listBox.Height);
                        if (is_mbco) { text = "To quickly jump to a level, enter the level number after selecting the " + pathway_term_name + " list box."; }
                        else { text = "To quickly jump to a level or depth, enter the level or depth number after selecting the " + pathway_term_name + " list box."; }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        Mbco_scps_listBox.SilentSelectedIndex = -1;
                        Update_visibilities_of_checkBoxes_and_buttons(copy_enrich_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 4:
                        if (IncludeAncestorSCPs_cbButton.Checked) { IncludeAncestorSCPs_cbButton.Button_pressed(); }
                        if (IncludeOffspringSCPs_cbButton.Checked) { IncludeOffspringSCPs_cbButton.Button_pressed(); }
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Mbco_scps_listBox.Location.Y + 0.5F * this.Mbco_scps_listBox.Height);
                        text = "Select one or multiple " + pathway_term_name + "s that shall become part of the group.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (is_mbco) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial energy production"); }
                        else if (Ontology_classification_class.Is_reactome_ontology(copy_enrich_options.Next_ontology)) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Autophagy"); }
                        else if (Ontology_classification_class.Is_go_ontology(copy_enrich_options.Next_ontology)) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("muscle filament sliding"); }
                        else if (Ontology_classification_class.Is_specialized_mbco_ontology(copy_enrich_options.Next_ontology)) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Sodium potassium chloride symporter"); }
                        if (Mbco_scps_listBox.SelectedItems.Count == 0) { Mbco_scps_listBox.SilentSelectedIndex = 1; }
                        Update_visibilities_of_checkBoxes_and_buttons(copy_enrich_options);
                        MBCO_listBox_changed(copy_enrich_options, copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 5:
                        if (IncludeAncestorSCPs_cbButton.Checked) { IncludeAncestorSCPs_cbButton.Button_pressed(); }
                        if (IncludeOffspringSCPs_cbButton.Checked) { IncludeOffspringSCPs_cbButton.Button_pressed(); }
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.IncludeAncestorSCPs_cbButton.Location.Y + 0.5F * (this.IncludeOffspringSCPs_cbButton.Location.Y + this.IncludeOffspringSCPs_cbButton.Height - this.IncludeAncestorSCPs_cbButton.Location.Y));
                        text = "The application allows automatic addition of all ancestor and/or descendent " + pathway_term_name + "(s) of the selected " + pathway_term_name + "(s).";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (!IncludeAncestorSCPs_cbButton.Checked) { IncludeAncestorSCPs_cbButton.Button_pressed(); }
                        if (!IncludeOffspringSCPs_cbButton.Checked) { IncludeOffspringSCPs_cbButton.Button_pressed(); }
                        if (is_mbco) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Mitochondrial energy production"); }
                        else if (Ontology_classification_class.Is_reactome_ontology(copy_enrich_options.Next_ontology)) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Autophagy"); }
                        else if (Ontology_classification_class.Is_go_ontology(copy_enrich_options.Next_ontology)) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("muscle filament sliding"); }
                        else if (Ontology_classification_class.Is_specialized_mbco_ontology(copy_enrich_options.Next_ontology)) { Mbco_scps_listBox.SilentSelectedIndex = Mbco_scps_listBox.Items.IndexOf("Sodium potassium chloride symporter"); }
                        if (Mbco_scps_listBox.SelectedItems.Count == 0) { Mbco_scps_listBox.SilentSelectedIndex = 1; }
                        Update_visibilities_of_checkBoxes_and_buttons(copy_enrich_options);
                        MBCO_listBox_changed(copy_enrich_options, copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 6:
                        if (Show_onlySelectedScps_cbButton.Checked)
                        { 
                            Show_onlySelectedScps_cbButton.SilentChecked = false;
                            Show_onlySelectedScps_cbButton.Refresh();
                            copy_enrich_options = ShowOnlySelectedScps_checkBox_CheckedChanged(copy_enrich_options, out bool add_genes_to_network);
                            copy_nw_options.Add_genes_to_standard_networks = add_genes_to_network;
                        }
                        if (AddGenes_cbButton.Checked)
                        { 
                            AddGenes_cbButton.SilentChecked = false;
                            AddGenes_cbButton.Refresh();
                        }
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.IncludeAncestorSCPs_cbButton.Location.Y + 0.5F * (this.IncludeOffspringSCPs_cbButton.Location.Y + this.IncludeOffspringSCPs_cbButton.Height));
                        text = "Pressing the 'Add'-button will move the selected " + pathway_term_name + "s into the group.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        copy_enrich_options = Add_button_pressed(copy_enrich_options, copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 7:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Show_onlySelectedScps_cbButton.Location.Y + 0.5F * (this.Show_onlySelectedScps_cbButton.Height));
                        text = "If selected, the application will generate separate result files for each group that only show the assigned " + pathway_term_name + "s.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (!Show_onlySelectedScps_cbButton.Checked)
                        { 
                            Show_onlySelectedScps_cbButton.SilentChecked = true;
                            Show_onlySelectedScps_cbButton.Refresh();
                            copy_enrich_options = ShowOnlySelectedScps_checkBox_CheckedChanged(copy_enrich_options, out bool add_genes_to_network);
                            copy_nw_options.Add_genes_to_standard_networks = add_genes_to_network;
                        }
                        if (AddGenes_cbButton.Checked)
                        { 
                            AddGenes_cbButton.SilentChecked = false;
                            AddGenes_cbButton.Refresh();
                        }
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 8:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Show_onlySelectedScps_cbButton.Location.Y + 0.5F * (this.Show_onlySelectedScps_cbButton.Height));
                        text = "If selected, the application will add the genes of each dataset as child nodes to the " + pathway_term_name + "s they map to.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (!Show_onlySelectedScps_cbButton.Checked)
                        {
                            Show_onlySelectedScps_cbButton.SilentChecked = true;
                            Show_onlySelectedScps_cbButton.Refresh();
                        }
                        if (!AddGenes_cbButton.Checked)
                        {
                            AddGenes_cbButton.SilentChecked = true;
                            AddGenes_cbButton.Refresh();
                        }
                        copy_nw_options = AddGenes_checkBox_CheckedChanged(copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 9:
                        mid_y_position = this.Overall_panel.Location.Y + Selection_panel.Location.Y + (int)Math.Round((float)this.Show_onlySelectedScps_cbButton.Location.Y + 0.5F * (this.Show_onlySelectedScps_cbButton.Height));
                        text = "If node sizes are scaled by " + pathway_term_name + " significance, the gene nodes may become very small. Selecting an alternative metric within the 'SCP networks'-menu to determine node sizes will produce gene nodes with more regular and readable sizes.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        if (!Show_onlySelectedScps_cbButton.Checked)
                        {
                            Show_onlySelectedScps_cbButton.SilentChecked = true;
                            Show_onlySelectedScps_cbButton.Refresh();
                        }
                        if (!AddGenes_cbButton.Checked)
                        {
                            AddGenes_cbButton.SilentChecked = true;
                            AddGenes_cbButton.Refresh();
                        }
                        copy_nw_options = AddGenes_checkBox_CheckedChanged(copy_nw_options);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    default:
                        end_tour = true;
                        break;
                }
                if (back_pressed) { tour_box_index = tour_box_index - 2; }
                if ((escape_pressed) || (tour_box_index == -2)) { end_tour = true; }
            }

            #region Reset buttons and selections
            NewGroup_ownTextBox.Text = (string)newGroup_button_text.Clone();
            Show_onlySelectedScps_cbButton.SilentChecked = show_onlySelectedScps_cbButton_checked;
            AddGenes_cbButton.SilentChecked = addGenes_cbButton_checked;
            IncludeAncestorSCPs_cbButton.Checked = add_ancestor_scps;
            IncludeOffspringSCPs_cbButton.Checked = add_descendent_scps;
            bool add_genes = true;
            Remove_groupButton_pressed(copy_enrich_options, copy_nw_options, ref add_genes);
            SortSCPs_listBox.SelectedIndex = sortScps_listBox_selectedIndex;
            if (Selected_scps_listBox.Items.Count > 0)
            {
                Selected_scps_listBox.SelectedIndex = sortScps_listBox_selectedIndex;
            }

            Update_mbcoSCPs_and_selectedSCPs_listBox(enrich_options, nw_options);
            #endregion

            UserInterface_tutorial.Set_to_invisible();
        }
        #endregion

        public void Write_mbco_yed_network(Graph_editor_enum graphEditor)
        {
            Common_functions.Global_definitions.Global_directory_and_file_class global_dirFile = new Common_functions.Global_definitions.Global_directory_and_file_class();
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string ontology_subdirectory = gdf.Ontology_hierarchy_subdirectory;
            string ontology_fileName = gdf.Get_name_for_ontology_hierarchy(Mbco_child_parent_network.Ontology);
            string[] legend_dataset_nodes = new string[0];
            Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(Mbco_parent_child_network.Ontology, Ontology_overview_network_enum.Parent_child_hierarchy, ontology_subdirectory, ontology_fileName, legend_dataset_nodes, graphEditor, ProgressReport);
            Write_mbcoHierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbcoHierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
    }
}
