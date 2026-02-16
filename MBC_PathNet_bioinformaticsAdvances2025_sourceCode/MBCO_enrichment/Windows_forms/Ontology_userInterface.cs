using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Enrichment;
using System.Windows.Forms;
using Windows_forms_customized_tools;
using Result_visualization;
using Data;
using Common_functions.Global_definitions;
using Common_functions.Array_own;
using Common_functions.Form_tools;
using System.Drawing;
using Windows_forms;
using System.Net;
using Network;
using System.IO;
using yed_network;
using Leave_out;
using System.Net.NetworkInformation;
using System.Windows.Forms.VisualStyles;
using System.Runtime.Remoting.Metadata.W3cXsd2001;



namespace ClassLibrary1.Ontology_userInterface
{
    class Ontology_interface_class
    {
        private MyPanel Overall_panel { get; set; }
        private MyPanel Ontology_panel { get; set; }
        private OwnListBox Ontology_listBox { get; set; }
        private Label Ontology_label { get; set; }
        private MyPanel_label Ontology_fileName_label { get; set; }
        private OwnListBox Organism_listBox { get; set; }
        private Button Tour_button { get; set; }
        private Button Write_hierarchy_button { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }
        private MyPanel TopScpInteractions_panel { get; set; }
        private Label TopScpInteractions_left_label { get; set; }
        private Label TopScpInteractions_top_label { get; set; }
        private Label TopScpInteractions_level2_label { get; set; }
        private Label TopScpInteractions_level3_label { get; set; }
        private OwnTextBox TopScpInteractions_level2_textBox { get; set; }
        private OwnTextBox TopScpInteractions_level3_textBox { get; set; }
        private Button Write_scpInteractions_button { get; set; }
        private Label Organism_label { get; set; }
        private Dictionary<string, Ontology_type_enum> OntologyString_ontology_dict = new Dictionary<string, Ontology_type_enum>();
        private Dictionary<string, Organism_enum> OrganismString_organism_dict = new Dictionary<string, Organism_enum>();
        private Dictionary<Ontology_type_enum, string> Ontology_ontologyString_dict = new Dictionary<Ontology_type_enum, string>();
        private Dictionary<Organism_enum, string> Organism_organismString_dict = new Dictionary<Organism_enum, string>();
        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }
        private ProgressReport_interface_class Progress_report { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private MBCO_enrichment_pipeline_options_class Enrichment_toy_options { get; set; }

        public Ontology_interface_class(MyPanel overall_panel,
                                        MyPanel ontology_panel,
                                        OwnListBox ontology_listBox,
                                        Label ontology_label,
                                        MyPanel_label ontology_fileName_panelLabel,
                                        OwnListBox organism_listBox,
                                        Label organism_label,
                                        MyPanel topScpInteractions_panel,
                                        Label topScpInteractions_left_label,
                                        Label topScpInteractions_top_label,
                                        Label topScpInteractions_level2_label,
                                        Label topScpInteractions_level3_label,
                                        OwnTextBox topScpInteractions_level2_textBox,
                                        OwnTextBox topScpInteractions_level3_textBox,
                                        Button write_scpInteractions_button,
                                        Button tour_button,
                                        Button write_hierarchy_button,
                                        Tutorial_interface_class userInterface_tutorial,
                                        MBCO_enrichment_pipeline_options_class enrich_options,
                                        MBCO_obo_network_class mbco_parent_child_nw,
                                        ProgressReport_interface_class progress_report,
                                        Form1_default_settings_class form_default_settings)
        {
            Overall_panel = overall_panel;
            Ontology_panel = ontology_panel;
            Ontology_listBox = ontology_listBox;
            Ontology_label = ontology_label;
            Ontology_fileName_label = ontology_fileName_panelLabel;
            Organism_listBox = organism_listBox;
            Organism_label = organism_label;
            Progress_report = progress_report;
            TopScpInteractions_panel = topScpInteractions_panel;
            TopScpInteractions_left_label = topScpInteractions_left_label;
            TopScpInteractions_top_label = topScpInteractions_top_label;
            TopScpInteractions_level2_label = topScpInteractions_level2_label;
            TopScpInteractions_level3_label = topScpInteractions_level3_label;
            TopScpInteractions_level2_textBox = topScpInteractions_level2_textBox;
            TopScpInteractions_level3_textBox = topScpInteractions_level3_textBox;
            Write_scpInteractions_button = write_scpInteractions_button;
            Enrichment_toy_options = enrich_options.Deep_copy();


            Tour_button = tour_button;
            Write_hierarchy_button = write_hierarchy_button;
            UserInterface_tutorial = userInterface_tutorial;
            Form_default_settings = form_default_settings;

            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            Ontology_type_enum[] ontologies = gdf.Ontology_organism_geneAssociationInputFileName_dict.Keys.ToArray();
            string ontology_string;
            foreach (Ontology_type_enum ontology in ontologies)
            {
                ontology_string = Ontology_classification_class.Get_name_of_ontology(ontology);
                OntologyString_ontology_dict.Add(ontology_string, ontology);
                Ontology_ontologyString_dict.Add(ontology, ontology_string);
            }
            Organism_enum[] organisms = gdf.Organisms;
            string organism_string;
            string add_string;
            foreach (Organism_enum organism in organisms)
            {
                switch (organism)
                {
                    case Organism_enum.Caenorhabditis_elegans:
                        add_string = "";
                        break;
                    default:
                        add_string = " (" + Ontology_classification_class.Get_organismString_for_enum(organism) + ")";
                        break;
                }

                organism_string = organism.ToString().Replace("_"," ") + add_string;
                OrganismString_organism_dict.Add(organism_string, organism);
                Organism_organismString_dict.Add(organism, organism_string);
            }


            Update_all_graphic_elements();
            Update_all_visualized_options_and_nw_toy_options(enrich_options);
            Update_mbco_parent_child_and_child_parent_obo_networks(mbco_parent_child_nw);

            TopScpInteractions_level2_textBox.SilentText_and_refresh = Math.Round(enrich_options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[2] * 100).ToString();
            TopScpInteractions_level3_textBox.SilentText_and_refresh = Math.Round(enrich_options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[3] * 100).ToString();

        }
        public bool Are_parentChild_hierarchy_networks_upToDate(MBCO_enrichment_pipeline_options_class options)
        {
            return (Mbco_parent_child_network.Ontology.Equals(options.Next_ontology))
                   && (Mbco_parent_child_network.Organism.Equals(options.Next_organism));
        }

        public void Update_mbco_parent_child_and_child_parent_obo_networks(MBCO_obo_network_class parent_child_nw)
        {
            if (parent_child_nw.Nodes.Nodes_length==0) { throw new Exception("No nodes in parent child network"); }
            if (!parent_child_nw.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception("Parent child NW Nodes direction is " + parent_child_nw.Nodes.Direction.ToString() + ", but has to be " + Ontology_direction_enum.Parent_child.ToString()); }
            if (!parent_child_nw.Scp_hierarchal_interactions.Equals(SCP_hierarchy_interaction_type_enum.Parent_child)) { throw new Exception("Parent child NW SCP hierarchical interaction is " + parent_child_nw.Scp_hierarchal_interactions + ", but has to be " + SCP_hierarchy_interaction_type_enum.Parent_child.ToString()); }
            Mbco_parent_child_network = parent_child_nw.Deep_copy_mbco_obo_nw();
        }

        public void Update_all_graphic_elements()
        {
            MyPanel my_panel; System.Windows.Forms.Label my_label; OwnTextBox my_textBox; Button my_button; OwnListBox my_listBox;
            int left_position; int right_position; int top_position; int bottom_position;
            int y_distance_panel_overallPanel = (int)Math.Round(0.005F * this.Overall_panel.Height);
            int x_distance_panel_overallPanel = y_distance_panel_overallPanel;

            Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            left_position = x_distance_panel_overallPanel;
            right_position = this.Overall_panel.Width - x_distance_panel_overallPanel;
            top_position = y_distance_panel_overallPanel;
            bottom_position = top_position + (int)Math.Round(0.85F * Overall_panel.Height);
            my_panel = Ontology_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_position, right_position, top_position, bottom_position);

            int y_distance_from_border_within_panel = (int)Math.Round(0.01F * this.Ontology_panel.Height);
            int x_distance_from_border_within_panel = (int)Math.Round(0.01F * this.Ontology_panel.Width);
            int y_distance_between_listBoxes = (int)Math.Round(0.05F * Ontology_panel.Height);
            int listBox_left_position = x_distance_from_border_within_panel;
            int listBox_right_position = (int)Math.Round(0.5F*this.Ontology_panel.Width);
            int listBox_height = (int)Math.Round(0.4F * this.Ontology_panel.Height);

            left_position = listBox_left_position;
            right_position = this.Ontology_panel.Width - listBox_left_position;
            top_position = y_distance_between_listBoxes;
            bottom_position = top_position + listBox_height;
            my_listBox = Ontology_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_position, right_position, top_position, bottom_position);

            top_position = bottom_position + y_distance_between_listBoxes;
            bottom_position = top_position + listBox_height;
            my_listBox = Organism_listBox;
            Form_default_settings.MyListBoxMultipleLines_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_position, right_position, top_position, bottom_position);

            left_position = listBox_left_position;
            right_position = listBox_right_position;
            bottom_position = Ontology_listBox.Location.Y;
            top_position = bottom_position - y_distance_between_listBoxes;
            my_label = Ontology_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_position, right_position, top_position, bottom_position);

            left_position = listBox_left_position;
            right_position = listBox_right_position;
            bottom_position = Organism_listBox.Location.Y;
            top_position = bottom_position - y_distance_between_listBoxes;
            my_label = Organism_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_position, right_position, top_position, bottom_position);

            left_position = x_distance_from_border_within_panel;
            right_position = this.Ontology_panel.Width - x_distance_from_border_within_panel;
            top_position = Organism_listBox.Location.Y + Organism_listBox.Height;
            bottom_position = (int)Math.Round(1F * this.Ontology_panel.Height);
            Ontology_fileName_label.Set_silent_text_without_adjustment_of_fontSize("");
            Ontology_fileName_label.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            Ontology_fileName_label.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_position, top_position, right_position, bottom_position, Form_default_settings);

            #region Scp interactions textBox, label, default button
            left_position = x_distance_panel_overallPanel;
            right_position = this.Ontology_panel.Width - (int)Math.Round(0.3F * Overall_panel.Width);
            top_position = this.Ontology_panel.Location.Y + this.Ontology_panel.Height;
            bottom_position = (int)Math.Round(1F * this.Overall_panel.Height) - y_distance_panel_overallPanel;
            my_panel = this.TopScpInteractions_panel;
            Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_position, right_position, top_position, bottom_position);

            OwnTextBox[] topScpInteraction_textBoxes = new OwnTextBox[] { TopScpInteractions_level2_textBox, TopScpInteractions_level3_textBox};
            int textBox_width = (int)Math.Round(0.135F * this.TopScpInteractions_panel.Width);
            int textBox_height = (int)Math.Round(0.4F * this.TopScpInteractions_panel.Height);

            right_position = this.TopScpInteractions_panel.Width - x_distance_from_border_within_panel - 2* textBox_width;
            top_position = (int)Math.Round(0.4F * this.TopScpInteractions_panel.Height);
            bottom_position = top_position + textBox_height;
            int distance_defaultButton_from_topPercentTextBoxes = (int)Math.Round(0.02F * (float)this.TopScpInteractions_panel.Width);

            foreach (OwnTextBox topScpInteraction_textBox in topScpInteraction_textBoxes)
            {
                left_position = right_position;
                right_position = left_position + textBox_width;
                my_textBox = topScpInteraction_textBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_position, right_position, top_position, bottom_position);
            }

            left_position = 0;
            right_position = TopScpInteractions_level2_textBox.Location.X;
            top_position = TopScpInteractions_level2_textBox.Location.Y;
            bottom_position = TopScpInteractions_panel.Height;
            my_label = TopScpInteractions_left_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_upperYPostion(my_label, left_position, right_position, top_position, bottom_position);

            left_position = 0;
            right_position = TopScpInteractions_level2_textBox.Location.X;
            top_position = 0;
            bottom_position = TopScpInteractions_left_label.Location.Y;
            my_label = TopScpInteractions_top_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_lowerYPostion(my_label, left_position, right_position, top_position, bottom_position);

            OwnTextBox referenceTextBox;
            referenceTextBox = TopScpInteractions_level2_textBox;
            left_position = referenceTextBox.Location.X - 11;
            right_position = referenceTextBox.Location.X + referenceTextBox.Width + 11;
            top_position = 0;
            bottom_position = referenceTextBox.Location.Y;
            my_label = TopScpInteractions_level2_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_position, right_position, top_position, bottom_position);

            referenceTextBox = TopScpInteractions_level3_textBox;
            left_position = referenceTextBox.Location.X - 11;
            right_position = referenceTextBox.Location.X + referenceTextBox.Width + 11;
            top_position = 0;
            bottom_position = referenceTextBox.Location.Y;
            my_label = TopScpInteractions_level3_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_position, right_position, top_position, bottom_position);
            #endregion

            List<Button> buttons = new List<Button>();
            buttons.Add(Write_scpInteractions_button);
            buttons.Add(Write_hierarchy_button);
            buttons.Add(Tour_button);
            int distance_between_buttons = (int)Math.Round(0.00025F * Overall_panel.Height);
            int left_height = this.Overall_panel.Height - this.Ontology_panel.Location.Y - this.Ontology_panel.Height;
            int button_height = (int)Math.Round(0.33333F*(left_height - 3 * distance_between_buttons));

            bottom_position = this.Ontology_panel.Location.Y + this.Ontology_panel.Height;
            foreach (Button button in buttons)
            {
                left_position = TopScpInteractions_panel.Location.X + TopScpInteractions_panel.Width;
                right_position = Overall_panel.Width - x_distance_panel_overallPanel;
                top_position = bottom_position + distance_between_buttons;
                bottom_position = top_position + button_height;
                my_button = button;
                Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_position, right_position, top_position, bottom_position);
            }
        }

        public void Update_all_visualized_options_and_nw_toy_options(MBCO_enrichment_pipeline_options_class enrich_options)
        {
            Enrichment_toy_options = enrich_options.Deep_copy();

            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            Ontology_listBox.Items.Clear();
            Ontology_listBox.Items.AddRange(OntologyString_ontology_dict.Keys.ToArray());
            Organism_listBox.Items.Clear();
            Organism_listBox.Items.AddRange(OrganismString_organism_dict.Keys.ToArray());
            string next_ontology_string = Ontology_ontologyString_dict[enrich_options.Next_ontology];
            Ontology_listBox.SilentSelectedIndex = Ontology_listBox.Items.IndexOf(next_ontology_string);
            string next_organism_string = Organism_organismString_dict[enrich_options.Next_organism];
            Organism_listBox.SilentSelectedIndex = Organism_listBox.Items.IndexOf(next_organism_string);

            string info_text = "";
            switch (gdf.Ontology_species_selectionOrder_dict[enrich_options.Next_ontology][enrich_options.Next_organism])
            {
                case Species_selection_order_enum.Insist_on_file:
                    info_text = "Species-selective gene pathway annotations exist.";
                    break;
                case Species_selection_order_enum.Always_generate_orthologues:
                    info_text = "Species-specific gene-pathway annotations will be inferred from human annotations using MGI and NCBI Gene orthologue mappings.";
                    break;
                case Species_selection_order_enum.Generate_orthologues_if_missing:
                    string complete_fileName = gdf.Ontology_inputDirectory_dict[enrich_options.Next_ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[enrich_options.Next_ontology][enrich_options.Next_organism];
                    if (System.IO.File.Exists(complete_fileName))
                    {
                        info_text = "Species-selective gene pathway annotations exist.";
                    }
                    else
                    {
                        info_text = "Species-specific gene-pathway annotations will be inferred from human annotations using MGI and NCBI Gene orthologue mappings.";
                    }
                    break;
                default:
                    throw new Exception();
            }
            if (Ontology_classification_class.Is_mbco_ontology(enrich_options.Next_ontology))
            {
                TopScpInteractions_panel.Visible = true;
                Write_scpInteractions_button.Visible = true;
            }
            else
            {
                TopScpInteractions_panel.Visible = false;
                Write_scpInteractions_button.Visible = false;
            }
            Ontology_fileName_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Ontology_fileName_label.Set_silent_text_adjustFontSize_and_refresh(info_text, Form_default_settings);
        }
        public void Set_to_visible(MBCO_enrichment_pipeline_options_class enrich_options)
        {
            this.Overall_panel.Visible = true;
            Update_all_visualized_options_and_nw_toy_options(enrich_options);
            //Update_errorReport_panelLabel("");
            Overall_panel.Refresh();
        }

        private string[] Get_all_fileNames_for_selected_ontology_and_selected_organism(Ontology_type_enum selected_ontology, Organism_enum selected_organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            List<string> all_necessary_fileNames = new List<string>();
            string ontology_association_fileName = gdf.Ontology_organism_geneAssociationInputFileName_dict[selected_ontology][selected_organism];
            all_necessary_fileNames.Add(ontology_association_fileName);
            string ontology_hierarchical_nw_fileName = gdf.Ontology_hierarchyInputFileName_dict[selected_ontology];
            all_necessary_fileNames.Add(ontology_hierarchical_nw_fileName);
            string ontology_pathwayAnnotation_fileName = "";
            if (gdf.Ontology_pathwayAnnotation_dict.ContainsKey(selected_ontology))
            {
                ontology_pathwayAnnotation_fileName = gdf.Ontology_pathwayAnnotation_dict[selected_ontology];
                all_necessary_fileNames.Add(ontology_pathwayAnnotation_fileName);
            }
            string ontology_scpNetworkInputFilename = "";
            if (gdf.Ontology_scpNetworkInputFileName_dict.ContainsKey(selected_ontology))
            {
                ontology_scpNetworkInputFilename = gdf.Ontology_scpNetworkInputFileName_dict[selected_ontology];
                all_necessary_fileNames.Add(ontology_scpNetworkInputFilename);
            }
            return all_necessary_fileNames.ToArray();
        }
        private string Generate_string_for_missing_fileNames(string[] missing_fileNames, string directory)
        {
            int missing_ontology_fileNames_length = missing_fileNames.Length;
            if (missing_ontology_fileNames_length==0) { throw new Exception(); }
            string missing_fileName;
            StringBuilder sb = new StringBuilder();
            for (int indexM = 0; indexM < missing_ontology_fileNames_length; indexM++)
            {
                missing_fileName = missing_fileNames[indexM];
                if (sb.Length > 0)
                {
                    if (indexM == missing_ontology_fileNames_length-1) { sb.Append(" and "); }
                    else { sb.Append(", "); }
                }
                sb.AppendFormat("'{0}'", missing_fileName);
            }
            if (missing_ontology_fileNames_length > 1) { sb.AppendFormat(" are "); }
            else { sb.AppendFormat(" is "); }
            sb.AppendFormat(" missing in '{0}'.", Path.GetFileName(directory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)));
            return sb.ToString();
        }

        private string[] Get_all_mandatory_ontology_file_names_if_insist_on_file(Ontology_type_enum selected_ontology, Organism_enum selected_organism)
        {
            string[] all_mandatory_ontologyfileNames = Get_all_fileNames_for_selected_ontology_and_selected_organism(selected_ontology, selected_organism);
            return all_mandatory_ontologyfileNames;
        }
        private void Get_all_mandatory_ontology_and_orthologue_file_names_if_insist_on_file(Global_directory_and_file_class gdf, Ontology_type_enum selected_ontology, Organism_enum selected_organism, out string[] all_mandatory_ontologyfileNames, out string[] all_mandatory_orthologuefileNames)
        {
            all_mandatory_ontologyfileNames = Get_all_fileNames_for_selected_ontology_and_selected_organism(selected_ontology, Organism_enum.Homo_sapiens);
            all_mandatory_orthologuefileNames = new string[] { gdf.Mgi_orthologs_download_fileName, gdf.GeneInfo_download_fileName, gdf.Ncbi_orthologs_download_fileName };
        }
        private void Get_missing_ontology_and_orthologue_fileNames(string ontology_data_directory, string orthologue_data_directory, string[] mandatory_ontology_fileNames, string[] mandatory_orthologue_fileNames, out string[] missing_mandatory_ontology_fileNames, out string[] missing_mandatory_orthologue_fileNames)
        {
            List<string> missing_ontology_fileNames = new List<string>();
            List<string> missing_ortholog_fileNames = new List<string>();
            foreach (string necessary_fileName in mandatory_ontology_fileNames)
            {
                string complete_fileName = ontology_data_directory + necessary_fileName;
                if (!System.IO.File.Exists(complete_fileName))
                {
                    missing_ontology_fileNames.Add(necessary_fileName);
                }
            }
            foreach (string necessary_fileName in mandatory_orthologue_fileNames)
            {
                string complete_fileName = orthologue_data_directory + necessary_fileName;
                if (!System.IO.File.Exists(complete_fileName))
                {
                    missing_ortholog_fileNames.Add(necessary_fileName);
                }
            }
            missing_mandatory_ontology_fileNames = missing_ontology_fileNames.ToArray();
            missing_mandatory_orthologue_fileNames = missing_ortholog_fileNames.ToArray();

        }
        private StringBuilder Add_missing_files_to_stringBuilder(StringBuilder sb, string ontology_data_subdirectory, string orthologue_data_subdirectory, string[] missing_ontology_fileNames, string[] missing_orthologue_fileNames)
        {
            if ((missing_orthologue_fileNames.Length > 0) || (missing_ontology_fileNames.Length > 0))
            {
                if (ontology_data_subdirectory.Equals(orthologue_data_subdirectory))
                {
                    List<string> missing_combined_fileNames = new List<string>();
                    missing_combined_fileNames.AddRange(missing_ontology_fileNames);
                    missing_combined_fileNames.AddRange(missing_orthologue_fileNames);
                    sb.Append(Generate_string_for_missing_fileNames(missing_combined_fileNames.ToArray(), ontology_data_subdirectory));
                }
                else
                {
                    if (missing_ontology_fileNames.Length > 0)
                    {
                        sb.Append(Generate_string_for_missing_fileNames(missing_ontology_fileNames.ToArray(), ontology_data_subdirectory));
                    }
                    if (missing_orthologue_fileNames.Length > 0)
                    {
                        sb.Append(Generate_string_for_missing_fileNames(missing_orthologue_fileNames.ToArray(), orthologue_data_subdirectory));
                    }
                }
            }
            return sb;
        }
        private bool Analyze_if_change_to_ontology_allowed_and_visualize_comment_if_not_allowed(Ontology_type_enum selected_ontology, Organism_enum selected_organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string[] all_mandatory_ontologyfileNames = new string[0];
            string[] all_mandatory_orthologsfileNames = new string[0];
            string[] all_alternative_mandatory_ontologyfileNames = new string[0];
            string[] all_alternative_mandatory_orthologsfileNames = new string[0];
            string ontology_association_fileName = gdf.Ontology_organism_geneAssociationInputFileName_dict[selected_ontology][selected_organism];
            string ontology_data_subdirectory = "error";
            string ortholog_data_subdirectory = "error";
            string[] missing_ontology_fileNames;
            string[] missing_orthologue_fileNames;
            string[] missing_alternative_ontology_fileNames = new string[0];
            string[] missing_alternative_orthologue_fileNames = new string[0];
            ontology_data_subdirectory = gdf.Ontology_inputDirectory_dict[selected_ontology];
            ortholog_data_subdirectory = gdf.Other_ontologies_datasets_subdirectory;
            string download_all_datasets_fileName;
            if (Form_default_settings.Is_mono)
            {
                download_all_datasets_fileName = "Download_all_datasets_linux.txt";
            }
            else
            {
                download_all_datasets_fileName = "Download_all_datasets_windows.txt";
            }


            string otherDataset_download_response = "See '" + download_all_datasets_fileName + "' for download instructions. ";
            string customDataset_response = "See 'Prepare_custom_ontology.txt' for instructions.";
            string otherdatasets_and_custom_response = "See '" + download_all_datasets_fileName + "' and 'Prepare_custom_ontology.txt' for instructions.";
            string mbcoDataset_download_response = "Please download the app again. ";
            bool change_allowed = true;
            StringBuilder sb = new StringBuilder();
            switch (gdf.Ontology_species_selectionOrder_dict[selected_ontology][selected_organism])
            {
                case Species_selection_order_enum.Always_generate_orthologues:
                    Get_all_mandatory_ontology_and_orthologue_file_names_if_insist_on_file(gdf, selected_ontology, selected_organism, out all_mandatory_ontologyfileNames, out all_mandatory_orthologsfileNames);
                    Get_missing_ontology_and_orthologue_fileNames(ontology_data_subdirectory, ortholog_data_subdirectory, all_mandatory_ontologyfileNames, all_mandatory_orthologsfileNames, out missing_ontology_fileNames, out missing_orthologue_fileNames);
                    if (missing_ontology_fileNames.Length>0 || missing_orthologue_fileNames.Length>0 )
                    {
                        sb = Add_missing_files_to_stringBuilder(sb, ontology_data_subdirectory, ortholog_data_subdirectory, missing_ontology_fileNames, missing_orthologue_fileNames);
                        change_allowed = false;
                    }
                    break;
                case Species_selection_order_enum.Insist_on_file:
                    all_mandatory_ontologyfileNames = Get_all_mandatory_ontology_file_names_if_insist_on_file(selected_ontology, selected_organism);
                    Get_missing_ontology_and_orthologue_fileNames(ontology_data_subdirectory, ortholog_data_subdirectory, all_mandatory_ontologyfileNames, all_mandatory_orthologsfileNames, out missing_ontology_fileNames, out missing_orthologue_fileNames);
                    if (missing_ontology_fileNames.Length > 0)
                    {
                        sb = Add_missing_files_to_stringBuilder(sb, ontology_data_subdirectory, ortholog_data_subdirectory, missing_ontology_fileNames, missing_orthologue_fileNames);
                        change_allowed = false;
                    }
                    break;
                case Species_selection_order_enum.Generate_orthologues_if_missing:
                    all_mandatory_ontologyfileNames = Get_all_mandatory_ontology_file_names_if_insist_on_file(selected_ontology, selected_organism);
                    Get_missing_ontology_and_orthologue_fileNames(ontology_data_subdirectory, ortholog_data_subdirectory, all_mandatory_ontologyfileNames, all_mandatory_orthologsfileNames, out missing_ontology_fileNames, out missing_orthologue_fileNames);
                    Get_all_mandatory_ontology_and_orthologue_file_names_if_insist_on_file(gdf, selected_ontology, selected_organism, out all_alternative_mandatory_ontologyfileNames, out all_alternative_mandatory_orthologsfileNames);
                    Get_missing_ontology_and_orthologue_fileNames(ontology_data_subdirectory, ortholog_data_subdirectory, all_alternative_mandatory_ontologyfileNames, all_alternative_mandatory_orthologsfileNames, out missing_alternative_ontology_fileNames, out missing_alternative_orthologue_fileNames);
                    if ((missing_ontology_fileNames.Length > 0)
                        && (  (missing_alternative_ontology_fileNames.Length > 0)
                            ||(missing_alternative_orthologue_fileNames.Length > 0)))
                    {
                        sb = Add_missing_files_to_stringBuilder(sb, ontology_data_subdirectory, ortholog_data_subdirectory, missing_ontology_fileNames, missing_orthologue_fileNames);
                        sb.AppendFormat(" Alternatively, for mapping of human genes to {0} orthologues: ", Ontology_classification_class.Get_organismString_for_enum(selected_organism));
                        sb = Add_missing_files_to_stringBuilder(sb, ontology_data_subdirectory, ortholog_data_subdirectory, missing_alternative_ontology_fileNames, missing_alternative_orthologue_fileNames);
                        change_allowed = false;
                    }
                    break;
                default:
                    throw new Exception();
            }
            if (!change_allowed)
            {
                if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                {
                    if (missing_ontology_fileNames.Length>0)
                    {
                        sb.AppendFormat(" {0}", mbcoDataset_download_response);
                    }
                    else if (missing_orthologue_fileNames.Length>0)
                    {
                        sb.AppendFormat(" {0}", otherDataset_download_response);
                    }
                }
                else if (Ontology_classification_class.Is_custom_ontology(selected_ontology))
                {
                    if ((missing_orthologue_fileNames.Length>0)||(missing_alternative_orthologue_fileNames.Length>0))
                    {
                        sb.AppendFormat(" {0}", otherdatasets_and_custom_response);
                    }
                    else
                    {
                        sb.AppendFormat(" {0}", customDataset_response);
                    }
                }
                else
                {
                    sb.AppendFormat(" {0}", otherDataset_download_response);
                }

                string progressReport_text = "Selection of '" + Ontology_classification_class.Get_name_of_ontology(selected_ontology) + " - " + Ontology_classification_class.Get_organismString_for_enum(selected_organism) + "' is not possible, since " + sb.ToString();
                Progress_report.Update_progressReport_text_and_visualization(progressReport_text);
                Ontology_fileName_label.Set_silent_text_adjustFontSize_and_refresh("Selection is not possible, because of missing files.", Form_default_settings);
            }
            else
            {
                Progress_report.Update_progressReport_text_and_visualization("");
            }

            return change_allowed;
        }
        public void Update_organism_and_ontology_if_possible_and_add_comment_if_not(Ontology_type_enum selected_ontology, Organism_enum selected_organism, MBCO_enrichment_pipeline_options_class enrich_options, MBCO_network_based_integration_options_class nw_options)
        {
            if (Analyze_if_change_to_ontology_allowed_and_visualize_comment_if_not_allowed(selected_ontology, selected_organism))
            {
                enrich_options.Set_next_ontology_and_organism(selected_ontology, selected_organism);
                nw_options.Set_next_ontology_and_organism(selected_ontology, selected_organism);
                Update_all_visualized_options_and_nw_toy_options(enrich_options);
            }
        }
        public void Ontology_or_organism_listBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class enrich_options, MBCO_network_based_integration_options_class nw_options)
        {
            string selected_ontology_string = Ontology_listBox.SelectedItem.ToString();
            Ontology_type_enum selected_ontology = OntologyString_ontology_dict[selected_ontology_string];
            string selected_organism_string = Organism_listBox.SelectedItem.ToString();
            Organism_enum selected_organism = OrganismString_organism_dict[selected_organism_string];
            Update_organism_and_ontology_if_possible_and_add_comment_if_not(selected_ontology, selected_organism, enrich_options, nw_options);
        }
        public string Get_listBox_entry_for_ontology(Ontology_type_enum ontology)
        {
            return Ontology_ontologyString_dict[ontology];
        }
        public string Get_listBox_entry_for_organism(Organism_enum organism)
        {
            return Organism_organismString_dict[organism];
        }

        public bool Is_tour_button_active(Button selected_button)
        {
            return selected_button.BackColor == Form_default_settings.Color_button_pressed_back;
        }
        public void Set_tour_button_to_not_selected()
        {
            this.Tour_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Tour_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Tour_button.Refresh();
        }
        public void Set_selected_tour_button_to_activated(Button selected_button)
        {
            selected_button.BackColor = Form_default_settings.Color_button_pressed_back;
            selected_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            selected_button.Refresh();
        }
        public void Tour_button_activated(MBCO_enrichment_pipeline_options_class enrich_options)
        {
            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel;
            int mid_y_position;
            int right_x_position;
            string text;
            right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;

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
                        mid_y_position = Overall_panel.Location.Y + Ontology_panel.Location.Y + Ontology_listBox.Location.Y
                             + (int)Math.Round(0.5F * (Ontology_listBox.Height));
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Select the ontology of interest. Refer to 'Download_all_datasets.txt' for dataset download commands, and 'Prepare_custom_ontologies.txt' for instructions on preparing custom ontologies.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        mid_y_position = Overall_panel.Location.Y + Ontology_panel.Location.Y + Ontology_listBox.Location.Y
                                         + (int)Math.Round(0.5F * (Ontology_listBox.Height));
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "The selected ontology affects available parameter options and a few tour steps in this, the 'Enrichment'-, 'SCP Networks'-, 'Select SCPs'- and 'Define new SCPs'-menus.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        mid_y_position = Overall_panel.Location.Y + Ontology_panel.Location.Y + Organism_listBox.Location.Y
                                         + (int)Math.Round(0.5F * (Organism_listBox.Height));
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "If gene-pathway annotations are unavailable for the selected species and ontology, human genes will be mapped to orthologs of the selected species.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 3:
                        mid_y_position = Overall_panel.Location.Y + Write_hierarchy_button.Location.Y
                                         + (int)Math.Round(0.5F * (Write_hierarchy_button.Height));
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "The function to generate a network showing the parent-child hierarchy of the selected ontology is implemented here and additionally in the 'Tips'-, 'Select SCPs'- and 'Define SCPs'-menus. The graph editor for visualization can be selected in the 'SCP networks'-menu.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 4:
                        if (Ontology_classification_class.Is_mbco_ontology(enrich_options.Next_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + TopScpInteractions_panel.Location.Y
                                             + (int)Math.Round(0.5F * (TopScpInteractions_panel.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "In MBCO, annotated parent-child interactions span up to four levels of granularity, with subcellular processes (SCPs) at the same level exhibiting minimal functional redundancies.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 5:
                        if (Ontology_classification_class.Is_mbco_ontology(enrich_options.Next_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + TopScpInteractions_panel.Location.Y
                                             + (int)Math.Round(0.5F * (TopScpInteractions_panel.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "In addition, MBCO includes weighted interactions between level-2 and level-3 SCPs, inferred from text mining of PubMed abstracts.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 6:
                        if (Ontology_classification_class.Is_mbco_ontology(enrich_options.Next_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + TopScpInteractions_panel.Location.Y
                                 + (int)Math.Round(0.5F * (TopScpInteractions_panel.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "These interactions, which form the basis for dynamic enrichment analysis (see 'Enrichment' menu), can be explored after specifying the percentage of top level-2 and level-3 interactions to include.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, System.Drawing.ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    default:
                        end_tour = true;
                        break;
                }
                if (back_pressed) { tour_box_index = tour_box_index - 2; }
                if ((escape_pressed) || (tour_box_index == -2)) { end_tour = true; }
            }
            UserInterface_tutorial.Set_to_invisible();


            UserInterface_tutorial.Set_to_invisible();
        }

        public void Write_mbco_hierarchy_button_pressed(Graph_editor_enum graphEditor)
        {
            Write_hierarchy_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_hierarchy_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Write_hierarchy_button.Refresh();
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string ontology_subdirectory = gdf.Ontology_hierarchy_subdirectory;
            string ontology_fileName = gdf.Get_name_for_ontology_hierarchy(Mbco_parent_child_network.Ontology);
            string[] legend_dataset_nodes = new string[0];
            Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(Mbco_parent_child_network.Ontology, Ontology_overview_network_enum.Parent_child_hierarchy, ontology_subdirectory, ontology_fileName, legend_dataset_nodes, graphEditor, Progress_report);
            Write_hierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_hierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
        public void Write_scp_interactions_button_pressed(Graph_editor_enum graphEditor)
        {
            Ontology_type_enum ontology = Mbco_parent_child_network.Ontology;
            if (!Ontology_classification_class.Is_mbco_ontology(ontology)) { throw new Exception("Not MBCO ontology in parent child"); }

            Write_scpInteractions_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_scpInteractions_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Write_scpInteractions_button.Refresh();
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string ontology_subdirectory = gdf.Ontology_hierarchy_subdirectory;
            string scpInteractions_name = gdf.Get_name_for_ontology_scpInteractions(ontology);
            string[] legend_dataset_nodes = new string[0];

            float top_level2_interactions = Enrichment_toy_options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[2];
            float top_level3_interactions = Enrichment_toy_options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[3];


            Dictionary<string, int> processName_processLevel_dict = Mbco_parent_child_network.Get_processName_level_dictionary_without_setting_process_level();

            Leave_out_class leave_out = new Leave_out_class(Mbco_parent_child_network.Ontology);
            leave_out.Generate_by_reading_safed_file(Progress_report);

            if (leave_out.Leave_out_lines.Length > 0)
            {
                Leave_out_scp_scp_network_class leave_out_nw = new Leave_out_scp_scp_network_class(Mbco_parent_child_network.Ontology);
                leave_out_nw.Options.Top_quantile_of_considered_SCP_interactions_per_level = new float[] { -1F, -1F, top_level2_interactions, top_level3_interactions, -1F };
                leave_out_nw.Generate_scp_scp_network_from_leave_out(leave_out, Progress_report);
                leave_out_nw.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
                leave_out_nw.Scp_nw.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(ontology, Ontology_overview_network_enum.Scp_interactions, ontology_subdirectory, scpInteractions_name, new string[0], graphEditor, Progress_report);
                Write_scpInteractions_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                Write_scpInteractions_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
        }

        private OwnTextBox Consider_topPercentScpsLevel_changed(OwnTextBox text_box, int scp_level, ref float[] top_considered_interactions)
        {
            int top_percent = -1;
            if (int.TryParse(text_box.Text, out top_percent))
            {
            }
            else { top_percent = -1; }//value ensures mismatch
            top_considered_interactions[scp_level] = (float)top_percent / 100;
            return text_box;
        }
        public void TopPercentScpsLevel_x_SCPs_textBox_TextChanged(int scp_level)
        {
            float[] top_considered_quantiles = new float[0];
            switch (scp_level)
            {
                case 2:
                    top_considered_quantiles = Array_class.Deep_copy_array(Enrichment_toy_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
                    this.TopScpInteractions_level2_textBox = Consider_topPercentScpsLevel_changed(this.TopScpInteractions_level2_textBox, scp_level, ref top_considered_quantiles);
                    Enrichment_toy_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = top_considered_quantiles;
                    if (Array_class.Arrays_are_equal(Enrichment_toy_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level, top_considered_quantiles))
                    { this.TopScpInteractions_level2_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.TopScpInteractions_level2_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                case 3:
                    top_considered_quantiles = Array_class.Deep_copy_array(Enrichment_toy_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
                    this.TopScpInteractions_level3_textBox = Consider_topPercentScpsLevel_changed(this.TopScpInteractions_level3_textBox, scp_level, ref top_considered_quantiles);
                    Enrichment_toy_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = top_considered_quantiles;
                    if (Array_class.Arrays_are_equal(Enrichment_toy_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level, top_considered_quantiles))
                    { this.TopScpInteractions_level3_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.TopScpInteractions_level3_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                default:
                    throw new Exception();
            }
        }

    }
}
