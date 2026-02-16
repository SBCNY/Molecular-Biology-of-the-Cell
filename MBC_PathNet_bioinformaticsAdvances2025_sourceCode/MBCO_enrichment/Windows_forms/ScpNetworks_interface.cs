//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using Windows_forms_customized_tools;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System;
using Common_functions.Array_own;
using Enrichment;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Runtime.Remoting.Contexts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using System.Windows.Controls.Primitives;
using yed_network;
using System.Security.Policy;
using Windows_forms;
using PdfSharp.Pdf.Content.Objects;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace ClassLibrary1.ScpNetworks_userInterface
{
    class ScpNetworks_userInterface_class
    {
        private MyPanel Overall_panel { get; set; }
        private MyPanel Standard_panel { get; set; }
        private MyPanel Dynamic_panel { get; set; }
        private Label Standard_label { get; set; }
        private Label Dynamic_label { get; set; }
        private Button Default_button { get; set; }
        private MyCheckBox_button StandardParentChild_cbButton { get; set; }
        private Label StandardParentChild_cbLabel { get; set; }
        private MyCheckBox_button StandardGroupSameLevelSCPs_cbButton { get; set; }
        private MyPanel_label StandardGroupSameLevelSCPs_cbLabel { get; set; }
        private MyCheckBox_button StandardAddGenes_cbButton { get; set; }
        private Label StandardAddGenes_cbLabel { get; set; }
        private MyCheckBox_button StandardConnectRelated_cbButton { get; set; }
        private Label StandardConnectRelated_cbLabel { get; set; }
        private OwnListBox ParentChildSCPNetGeneration_ownListBox { get; set; }
        private Label ParentChildSCPNetGeneration_label{ get; set; }
        private OwnListBox HierarchicalScpInteractions_ownListBox { get; set; }
        private Label HierarchicalScpInteractions_label { get; set; }
        private MyPanel StandardConnectScpsTopInteractions_panel { get; set; }
        private Label StandardConnectScpsTopInteractions_scpLevel_label { get; set; }
        private Label StandardConnectScpsTopInteractions_level_2_label { get; set; }
        private Label StandardConnectScpsTopInteractions_level_3_label { get; set; }
        private Label StandardConnectScpsTopInteractions_connect_label { get; set; }
        private OwnTextBox StandardConnectScpsTopInteractions_level_2_ownTextBox { get; set; }
        private OwnTextBox StandardConnectScpsTopInteractions_level_3_ownTextBox { get; set; }
        private MyCheckBox_button DynamicParentChild_cbButton { get; set; }
        private Label DynamicParentChild_cbLabel { get; set; }
        private MyCheckBox_button DynamicGroupSameLevelSCPs_cbButton { get; set; }
        private MyPanel_label DynamicGroupSameLevelSCPs_cbLabel { get; set; }
        private MyCheckBox_button DynamicAddGenes_cbButton { get; set; }
        private Label DynamicAddGenes_cbLabel { get; set; }
        private MyCheckBox_button DynamicConnectAllRelated_cbButton { get; set; }
        private Label DynamicConnectAllRelated_cbLabel{ get; set; }
        private MyCheckBox_button Generate_scp_networks_cbButton { get; set; }
        private Label Generate_scp_networks_cbLabel{ get; set; }
        private MyPanel NodeSize_panel { get; set; }
        private MyPanel_label DynamicConnectAllScps_explanation_myPanelLabel { get; set; }
        private Label NodeSize_headline_label { get; set; }
        private Label NodeSize_determinant_label { get; set; }
        private OwnListBox NodeSize_determinant_ownListBox { get; set; }
        private MyCheckBox_button NodeSize_adoptTextSize_cbButton { get; set; }
        private Label NodeSize_adoptTextSize_label { get; set; }
        private Label NodeSize_scaling_label { get; set; }
        private OwnListBox NodeSize_scaling_ownListBox { get; set; }
        private MyPanel_label NodeSize_diameterMax_myPanelLabel { get; set; }
        private OwnTextBox NodeSize_diameterMax_ownTextBox { get; set; }
        private MyPanel_label NodeLabel_minSize_myPanelLabel { get; set; }
        private OwnTextBox NodeLabel_minSize_ownTextBox { get; set; }
        private MyPanel_label NodeLabel_maxSize_myPanelLabel { get; set; }
        private OwnTextBox NodeLabel_maxSize_ownTextBox { get; set; }
        private OwnTextBox NodeLabel_uniqueSize_ownTextBox { get; set; }
        private MyPanel GraphEditor_panel { get; set; }
        private OwnListBox GraphEditor_ownListBox { get; set; }
        private Label GraphEditor_label { get; set; }
        private MyPanel_label GraphFileExtension_myPanelLabel { get; set; }
        private MyPanel Comments_panel { get; set; }
        private MyPanel_label Comments_standardDynamicAddGenes_myPanelLabel { get; set; }

        private Label Error_reports_headline_label { get; set; }
        private Label Error_reports_maxErrorPerFile1_label { get; set; }
        private Label Error_reports_maxErrorPerFile2_label { get; set; }
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }

        private Button Explanation_button { get; set; }
        private Button Tutorial_button { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }

        private Form1_default_settings_class Form_default_settings { get; set; }

        private Dictionary<string, Yed_network_node_size_determinant_enum> ListBoxString_networkDeterminantSize_dict { get; set; }
        private Dictionary<Yed_network_node_size_determinant_enum, string> NetworkDeterminantSize_listBoxString_dict { get; set; }
        private Dictionary<string, Graph_editor_enum> ListBoxString_graphEditor_dict { get; set; }
        private Dictionary<Graph_editor_enum, string> GraphEditor_listBoxString_dict { get; set; }
        private Dictionary<string, Node_size_scaling_across_plots_enum> ListBoxString_nodeSizeScaling_dict { get; set; }
        private Dictionary<Node_size_scaling_across_plots_enum, string> NodeSizeScaling_listBoxString_dict { get; set; }
        private Dictionary<string, Predicted_scp_hierarchy_integration_strategy_enum> ListBoxString_parentChildScpNetGen_dict { get; set; }
        private Dictionary<Predicted_scp_hierarchy_integration_strategy_enum, string> ParentChildScpNetGen_listBoxString_dict { get; set; }
        private Dictionary<string, SCP_hierarchy_interaction_type_enum> ListBoxString_hierarchicalScpInteraction_dict { get; set; }
        private Dictionary<SCP_hierarchy_interaction_type_enum, string> HierarchicalScpInteraction_listBoxString_dict { get; set; }

        public ScpNetworks_userInterface_class(MyPanel overall_panel,
                                               MyPanel standard_panel,
                                               Label standard_label,
                                               MyPanel dynamic_panel,
                                               Label dynamic_label,
                                               Button default_button,
                                               MyCheckBox_button standardParentChild_cbButton,
                                               Label standardParentChild_cbLabel,
                                               MyCheckBox_button standardGroupSameLevelScps_cbButton,
                                               MyPanel_label standardGroupSameLevelScps_cbLabel,
                                               MyCheckBox_button standardAddGenes_cbButton,
                                               Label standardAddGenes_cbLabel,
                                               MyCheckBox_button standardConnectRelated_cbButton,
                                               Label standardConnectRelated_cbLabel,
                                               OwnListBox parentChildSCPNetGeneration_ownListBox,
                                               Label parentChildSCPNetGeneration_label,
                                               OwnListBox hierarchicalScpInteractions_ownListBox,
                                               Label hierarchicalScpInteractions_label,
                                               MyPanel standardConnectScpsTopInteractions_panel,
                                               Label standardConnectScpsTopInteractions_scpLevel_label,
                                               Label standardConnectScpsTopInteractions_level_2_label,
                                               Label standardConnectScpsTopInteractions_level_3_label,
                                               Label standardConnectScpsTopInteractions_connect_label,
                                               OwnTextBox standardConnectScpsTopInteractions_level_2_ownTextBox,
                                               OwnTextBox standardConnectScpsTopInteractions_level_3_ownTextBox,
                                               MyCheckBox_button dynamicParentChild_cbButton,
                                               Label dynamicParentChild_cbLabel,
                                               MyCheckBox_button dynamicGroupSameLevelScps_cbButton,
                                               MyPanel_label dynamicGroupSameLevelScps_cbLabel,
                                               MyCheckBox_button dynamicAddGenes_cbButton,
                                               Label dynamicAddGenes_cbLabel,
                                               MyCheckBox_button dynamicConnectAllRelated_cbButton,
                                               Label dynamicConnectAllRelated_cbLabel,
                                               MyPanel_label dynamicConnectAllScps_explantion_myPanelLabel,
                                               MyCheckBox_button generate_scp_networks_cbButton,
                                               Label generate_scp_networks_cbLabel,
                                               MyPanel nodeSize_panel,
                                               Label nodeSize_headline_label,
                                               Label nodeSize_determinant_label,
                                               OwnListBox nodeSize_determinant_ownListBox,
                                               MyCheckBox_button nodeSize_adoptTextSize_cbButton,
                                               Label nodeSize_adoptTextSize_label,
                                               MyPanel_label nodeSize_diameterMax_myPanelLabel,
                                               OwnTextBox nodeSize_diameterMax_ownTextBox,
                                               MyPanel_label nodeLabel_minSize_myPanelLabel,
                                               OwnTextBox nodeLabel_minSize_ownTextBox,
                                               MyPanel_label nodeLabel_maxSize_myPanelLabel,
                                               OwnTextBox nodeLabel_maxSize_ownTextBox,
                                               OwnTextBox nodeLabel_uniqueSize_ownTextBox,
                                               Label nodeSize_scaling_label,
                                               OwnListBox nodeSize_scaling_ownListBox,

                                               MyPanel graphEditor_panel,
                                               OwnListBox graphEditor_ownListBox,
                                               Label graphEditor_label,
                                               MyPanel_label graphFileExtension_myPanelLabel,

                                               MyPanel comments_panel,
                                               MyPanel_label comments_graphEditor_myPanelLabel,

                                               Label error_reports_headline_label,
                                               Label error_reports_maxErrorPerFile1_label,
                                               Label error_reports_maxErrorPerFile2_label,
                                               OwnTextBox error_reports_ownTextBox,
                                               OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,

                                               Button explanation_button,
                                               Button tutorial_button,
                                               Tutorial_interface_class userInterface_tutorial,

                                               MBCO_network_based_integration_options_class options,
                                               Form1_default_settings_class form_default_settings)

        {
            this.Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.Standard_panel = standard_panel;
            this.Standard_label = standard_label;
            this.Dynamic_panel = dynamic_panel;
            this.Dynamic_label = dynamic_label;
            this.Default_button = default_button;
            this.StandardParentChild_cbButton = standardParentChild_cbButton;
            this.StandardParentChild_cbLabel = standardParentChild_cbLabel;
            this.StandardGroupSameLevelSCPs_cbButton = standardGroupSameLevelScps_cbButton;
            this.StandardGroupSameLevelSCPs_cbLabel = standardGroupSameLevelScps_cbLabel;
            this.StandardAddGenes_cbButton = standardAddGenes_cbButton;
            this.StandardAddGenes_cbLabel = standardAddGenes_cbLabel;
            this.StandardConnectRelated_cbButton = standardConnectRelated_cbButton;
            this.StandardConnectRelated_cbLabel = standardConnectRelated_cbLabel;
            this.ParentChildSCPNetGeneration_ownListBox = parentChildSCPNetGeneration_ownListBox;
            this.ParentChildSCPNetGeneration_label = parentChildSCPNetGeneration_label;
            this.HierarchicalScpInteractions_ownListBox = hierarchicalScpInteractions_ownListBox;
            this.HierarchicalScpInteractions_label = hierarchicalScpInteractions_label;
            this.StandardConnectScpsTopInteractions_panel = standardConnectScpsTopInteractions_panel;
            this.StandardConnectScpsTopInteractions_scpLevel_label = standardConnectScpsTopInteractions_scpLevel_label;
            this.StandardConnectScpsTopInteractions_level_2_label = standardConnectScpsTopInteractions_level_2_label;
            this.StandardConnectScpsTopInteractions_level_3_label = standardConnectScpsTopInteractions_level_3_label;
            this.StandardConnectScpsTopInteractions_connect_label = standardConnectScpsTopInteractions_connect_label;
            this.StandardConnectScpsTopInteractions_level_2_ownTextBox = standardConnectScpsTopInteractions_level_2_ownTextBox;
            this.StandardConnectScpsTopInteractions_level_3_ownTextBox = standardConnectScpsTopInteractions_level_3_ownTextBox;
            this.DynamicParentChild_cbButton = dynamicParentChild_cbButton;
            this.DynamicParentChild_cbLabel = dynamicParentChild_cbLabel;
            this.DynamicGroupSameLevelSCPs_cbButton = dynamicGroupSameLevelScps_cbButton;
            this.DynamicGroupSameLevelSCPs_cbLabel = dynamicGroupSameLevelScps_cbLabel;
            this.DynamicAddGenes_cbButton = dynamicAddGenes_cbButton;
            this.DynamicAddGenes_cbLabel = dynamicAddGenes_cbLabel;
            this.DynamicConnectAllRelated_cbButton = dynamicConnectAllRelated_cbButton;
            this.DynamicConnectAllRelated_cbLabel = dynamicConnectAllRelated_cbLabel;
            this.DynamicConnectAllScps_explanation_myPanelLabel = dynamicConnectAllScps_explantion_myPanelLabel;
            this.Generate_scp_networks_cbButton = generate_scp_networks_cbButton;
            this.Generate_scp_networks_cbLabel = generate_scp_networks_cbLabel;
            this.NodeSize_panel = nodeSize_panel;
            this.NodeSize_headline_label = nodeSize_headline_label;
            this.NodeSize_determinant_label = nodeSize_determinant_label;
            this.NodeSize_determinant_ownListBox = nodeSize_determinant_ownListBox;
            this.NodeSize_adoptTextSize_label = nodeSize_adoptTextSize_label;
            this.NodeSize_adoptTextSize_cbButton = nodeSize_adoptTextSize_cbButton;
            this.NodeSize_diameterMax_ownTextBox = nodeSize_diameterMax_ownTextBox;
            this.NodeSize_diameterMax_myPanelLabel = nodeSize_diameterMax_myPanelLabel;
            this.NodeLabel_minSize_ownTextBox = nodeLabel_minSize_ownTextBox;
            this.NodeLabel_minSize_myPanelLabel = nodeLabel_minSize_myPanelLabel;
            this.NodeLabel_maxSize_ownTextBox = nodeLabel_maxSize_ownTextBox;
            this.NodeLabel_maxSize_myPanelLabel = nodeLabel_maxSize_myPanelLabel;
            this.NodeLabel_uniqueSize_ownTextBox = nodeLabel_uniqueSize_ownTextBox;
            this.NodeSize_scaling_ownListBox = nodeSize_scaling_ownListBox;
            this.NodeSize_scaling_label = nodeSize_scaling_label;
            this.GraphEditor_panel = graphEditor_panel;
            this.GraphEditor_ownListBox = graphEditor_ownListBox;
            this.GraphEditor_label = graphEditor_label;
            this.GraphFileExtension_myPanelLabel = graphFileExtension_myPanelLabel;
            this.Comments_panel = comments_panel;
            this.Comments_standardDynamicAddGenes_myPanelLabel = comments_graphEditor_myPanelLabel;




            Error_reports_headline_label = error_reports_headline_label;
            Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;
            Error_reports_ownTextBox = error_reports_ownTextBox;
            Explanation_button = explanation_button;
            Tutorial_button = tutorial_button;
            UserInterface_tutorial = userInterface_tutorial;


            string[] listBoxStrings;

            ListBoxString_graphEditor_dict = new Dictionary<string, Graph_editor_enum>();
            ListBoxString_graphEditor_dict.Add("yED Graph Editor", Graph_editor_enum.yED);
            ListBoxString_graphEditor_dict.Add("Cytoscape", Graph_editor_enum.Cytoscape);
            this.GraphEditor_ownListBox.Items.Clear();
            listBoxStrings = ListBoxString_graphEditor_dict.Keys.ToArray();
            GraphEditor_ownListBox.Items.AddRange(listBoxStrings);
            GraphEditor_listBoxString_dict = new Dictionary<Graph_editor_enum, string>();
            foreach (string listBoxString in listBoxStrings)
            {
                GraphEditor_listBoxString_dict.Add(ListBoxString_graphEditor_dict[listBoxString], listBoxString);
            }

            ListBoxString_networkDeterminantSize_dict = new Dictionary<string, Yed_network_node_size_determinant_enum>();
            ListBoxString_networkDeterminantSize_dict.Add("~ -log10(p)", Yed_network_node_size_determinant_enum.Minus_log10_pvalue);
            ListBoxString_networkDeterminantSize_dict.Add("~ # datasets", Yed_network_node_size_determinant_enum.No_of_sets);
            ListBoxString_networkDeterminantSize_dict.Add("~ # colors", Yed_network_node_size_determinant_enum.No_of_different_colors);
            ListBoxString_networkDeterminantSize_dict.Add("uniform", Yed_network_node_size_determinant_enum.Uniform);
            this.NodeSize_determinant_ownListBox.Items.Clear();
            listBoxStrings = ListBoxString_networkDeterminantSize_dict.Keys.ToArray();
            this.NodeSize_determinant_ownListBox.Items.AddRange(listBoxStrings);
            NetworkDeterminantSize_listBoxString_dict = new Dictionary<Yed_network_node_size_determinant_enum, string>();
            foreach (string listBoxString in listBoxStrings)
            {
                NetworkDeterminantSize_listBoxString_dict.Add(ListBoxString_networkDeterminantSize_dict[listBoxString], listBoxString);
            }

            ListBoxString_nodeSizeScaling_dict = new Dictionary<string, Node_size_scaling_across_plots_enum>();
            ListBoxString_nodeSizeScaling_dict.Add("independent", Node_size_scaling_across_plots_enum.Unique);
            ListBoxString_nodeSizeScaling_dict.Add("uniform", Node_size_scaling_across_plots_enum.Consitent);
            this.NodeSize_scaling_ownListBox.Items.Clear();
            listBoxStrings = ListBoxString_nodeSizeScaling_dict.Keys.ToArray();
            this.NodeSize_scaling_ownListBox.Items.AddRange(listBoxStrings);
            NodeSizeScaling_listBoxString_dict = new Dictionary<Node_size_scaling_across_plots_enum, string>();
            foreach (string listBoxString in listBoxStrings)
            {
                NodeSizeScaling_listBoxString_dict.Add(ListBoxString_nodeSizeScaling_dict[listBoxString], listBoxString);
            }

            ListBoxString_parentChildScpNetGen_dict = new Dictionary<string, Predicted_scp_hierarchy_integration_strategy_enum>();
            ListBoxString_parentChildScpNetGen_dict.Add("All ancestors", Predicted_scp_hierarchy_integration_strategy_enum.All_ancestors);
            //ListBoxString_parentChildScpNetGen_dict.Add("Intersecting branches", Predicted_scp_hierarchy_integration_strategy_enum.First_shared_parent);
            ListBoxString_parentChildScpNetGen_dict.Add("Intermediate nodes", Predicted_scp_hierarchy_integration_strategy_enum.Intermediate_nodes);
            this.ParentChildSCPNetGeneration_ownListBox.Items.Clear();
            listBoxStrings = ListBoxString_parentChildScpNetGen_dict.Keys.ToArray();
            this.ParentChildSCPNetGeneration_ownListBox.Items.AddRange(listBoxStrings);
            ParentChildScpNetGen_listBoxString_dict = new Dictionary<Predicted_scp_hierarchy_integration_strategy_enum, string>();
            foreach (string listBoxString in listBoxStrings)
            {
                ParentChildScpNetGen_listBoxString_dict.Add(ListBoxString_parentChildScpNetGen_dict[listBoxString], listBoxString);
            }

            ListBoxString_hierarchicalScpInteraction_dict = new Dictionary<string, SCP_hierarchy_interaction_type_enum>();
            ListBoxString_hierarchicalScpInteraction_dict.Add("Parent child", SCP_hierarchy_interaction_type_enum.Parent_child);
            ListBoxString_hierarchicalScpInteraction_dict.Add("Parent child regulatory", SCP_hierarchy_interaction_type_enum.Parent_child_regulatory);
            this.HierarchicalScpInteractions_ownListBox.Items.Clear();
            listBoxStrings = ListBoxString_hierarchicalScpInteraction_dict.Keys.ToArray();
            this.HierarchicalScpInteractions_ownListBox.Items.AddRange(listBoxStrings);
            HierarchicalScpInteraction_listBoxString_dict = new Dictionary<SCP_hierarchy_interaction_type_enum, string>();
            foreach (string listBoxString in listBoxStrings)
            {
                HierarchicalScpInteraction_listBoxString_dict.Add(ListBoxString_hierarchicalScpInteraction_dict[listBoxString], listBoxString);
            }
            Update_graphic_elements(options);
        }

        private void Get_shared_borders_for_nodeSizePanel(out int shared_nodeSize_listTextBox_topReferenceBorder, out int shared_nodeSize_listTextBox_bottomReferenceBorder, out int shared_nodeSize_headline_topReferenceBorder)
        {
            shared_nodeSize_listTextBox_topReferenceBorder = (int)Math.Round(0.50 * (double)NodeSize_panel.Height);
            shared_nodeSize_listTextBox_bottomReferenceBorder = (int)Math.Round(0.98 * (double)NodeSize_panel.Height);
            shared_nodeSize_headline_topReferenceBorder = (int)Math.Round(0.05 * NodeSize_panel.Height);
        }

        private Dictionary<string, int> Get_nodeSizePanelParameter_textBoxVerticalPosition_dict_nodeSizePanelStartListBoxesPosition_nodeSizePanelStartListBoxesPosition_leftTextBoxBorder_and_rightTextBoxBorder(out int nodeSizePanel_start_listBoxes_position, out int oneFourth_nodeSize_panel_height, out int left_textBoxBorder, out int right_textBoxBorder)
        {
            nodeSizePanel_start_listBoxes_position = (int)Math.Round(0.21F * NodeSize_panel.Height);
            oneFourth_nodeSize_panel_height = (int)Math.Round(0.25F * (NodeSize_panel.Height - nodeSizePanel_start_listBoxes_position));
            Dictionary<string, int> nodeSizePanelParameter_textBoxVerticalPosition_dict = new Dictionary<string, int>();
            int verticalPosition = 1;
            nodeSizePanelParameter_textBoxVerticalPosition_dict.Add("Determinant", verticalPosition);
            verticalPosition++;
            nodeSizePanelParameter_textBoxVerticalPosition_dict.Add("Node diameter", verticalPosition);
            verticalPosition++;
            nodeSizePanelParameter_textBoxVerticalPosition_dict.Add("Label size", verticalPosition);
            verticalPosition++;
            nodeSizePanelParameter_textBoxVerticalPosition_dict.Add("Scaling", verticalPosition);
            left_textBoxBorder = (int)Math.Round(0.5F * NodeSize_panel.Width);
            right_textBoxBorder = (int)Math.Round(0.85F * NodeSize_panel.Width);
            return nodeSizePanelParameter_textBoxVerticalPosition_dict;
        }
        public void Update_graphic_elements(MBCO_network_based_integration_options_class options)
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            int distance_between_panels = (int)Math.Round(0.002 * Overall_panel.Height);
            int bottom_of_previous_panel = - distance_between_panels;
            Label my_label;
            MyPanel my_panel;
            OwnTextBox my_textBox;
            OwnListBox my_listBox;
            MyCheckBox_button my_cbButton;
            Button my_button;

            this.Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            #region Standard scp networks panel
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = bottom_of_previous_panel + distance_between_panels;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.28*(double)Overall_panel.Height);
            this.Standard_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(Standard_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            bottom_of_previous_panel = this.Standard_panel.Location.Y + this.Standard_panel.Height;
            #endregion

            #region Standard scp checkboxes
            int shared_cbButton_widthHeight_standardSCP = (int)Math.Round(0.14F * this.Standard_panel.Height);
            int shared_min_distance_X_from_panelSides_standardSCP = (int)Math.Round(0.02F * this.Standard_panel.Width);
            int top_referenceBorder_checkBoxes_standardSCP = (int)Math.Round(0.15F * this.Standard_panel.Height);

            left_referenceBorder = shared_min_distance_X_from_panelSides_standardSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = top_referenceBorder_checkBoxes_standardSCP;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = StandardParentChild_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.70F * Standard_panel.Width);
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = StandardAddGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = StandardParentChild_cbButton.Location.X + StandardParentChild_cbButton.Width;
            right_referenceBorder = StandardAddGenes_cbButton.Location.X;
            my_label = StandardParentChild_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = StandardAddGenes_cbButton.Location.X + StandardAddGenes_cbButton.Width;
            right_referenceBorder = Standard_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            my_label = StandardAddGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_standardSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = StandardGroupSameLevelSCPs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            //my_label = StandardGroupSameLevelSCPs_cbLabel;
            //Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_standardSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = bottom_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = StandardConnectRelated_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = StandardConnectRelated_cbButton.Location.X + StandardConnectRelated_cbButton.Width;
            right_referenceBorder = Standard_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            my_label = StandardConnectRelated_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Standard scp parentChildConnect and hierarchical SCP interactions
            left_referenceBorder = (int)Math.Round(0.45 * Standard_panel.Width);
            right_referenceBorder = (int)Math.Round(0.99 * Standard_panel.Width);
            top_referenceBorder = (int)Math.Round(0.45 * Standard_panel.Height);
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.2 * Standard_panel.Height);
            my_listBox = ParentChildSCPNetGeneration_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = ParentChildSCPNetGeneration_ownListBox.Location.Y + ParentChildSCPNetGeneration_ownListBox.Height + (int)Math.Round(0.1 * Standard_panel.Height);
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.2 * Standard_panel.Height);
            my_listBox = HierarchicalScpInteractions_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_standardSCP;
            right_referenceBorder = ParentChildSCPNetGeneration_ownListBox.Location.X;
            top_referenceBorder = ParentChildSCPNetGeneration_ownListBox.Location.Y - (int)Math.Round(0.05 * Standard_panel.Height);
            bottom_referenceBorder = ParentChildSCPNetGeneration_ownListBox.Location.Y + ParentChildSCPNetGeneration_ownListBox.Height + (int)Math.Round(0.05 * Standard_panel.Height);
            my_label = ParentChildSCPNetGeneration_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            right_referenceBorder = HierarchicalScpInteractions_ownListBox.Location.X;
            top_referenceBorder = HierarchicalScpInteractions_ownListBox.Location.Y - (int)Math.Round(0.05 * Standard_panel.Height);
            bottom_referenceBorder = HierarchicalScpInteractions_ownListBox.Location.Y + ParentChildSCPNetGeneration_ownListBox.Height + (int)Math.Round(0.05 * Standard_panel.Height);
            my_label = HierarchicalScpInteractions_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Standard label
            left_referenceBorder = 0;
            right_referenceBorder = this.Standard_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = StandardParentChild_cbButton.Location.Y;
            my_label = Standard_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Standard connect related scps panel, textboxes and labels
            left_referenceBorder = 0;
            right_referenceBorder = this.Standard_panel.Width;
            top_referenceBorder = this.StandardConnectRelated_cbButton.Location.Y + this.StandardConnectRelated_cbButton.Height;
            bottom_referenceBorder = this.Standard_panel.Height;
            my_panel = this.StandardConnectScpsTopInteractions_panel;
            Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int height_of_textBoxes_standard_connectSCPs = (int)Math.Round(0.45F * StandardConnectScpsTopInteractions_panel.Height);
            int width_of_textBoxes_standard_connectSCPs = (int)Math.Round(0.1F * StandardConnectScpsTopInteractions_panel.Width);

            top_referenceBorder = (int)Math.Round(0.54F * this.StandardConnectScpsTopInteractions_panel.Height);
            bottom_referenceBorder = top_referenceBorder + height_of_textBoxes_standard_connectSCPs;

            left_referenceBorder = (int)Math.Round(0.75F * this.StandardConnectScpsTopInteractions_panel.Width);
            right_referenceBorder = left_referenceBorder + width_of_textBoxes_standard_connectSCPs;
            my_textBox = this.StandardConnectScpsTopInteractions_level_2_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            my_textBox.TextAlign = HorizontalAlignment.Center;

            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = left_referenceBorder + width_of_textBoxes_standard_connectSCPs;
            my_textBox = this.StandardConnectScpsTopInteractions_level_3_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            my_textBox.TextAlign = HorizontalAlignment.Center;

            Dictionary<OwnTextBox, Label> textBox_label_dict = new Dictionary<OwnTextBox, Label>();
            textBox_label_dict.Add(this.StandardConnectScpsTopInteractions_level_2_ownTextBox, this.StandardConnectScpsTopInteractions_level_2_label);
            textBox_label_dict.Add(this.StandardConnectScpsTopInteractions_level_3_ownTextBox, this.StandardConnectScpsTopInteractions_level_3_label);
            OwnTextBox[] textBoxes = textBox_label_dict.Keys.ToArray();
            Label current_label;
            OwnTextBox current_textBox;
            int textBoxes_length = textBoxes.Length;
            for (int indexTB=0; indexTB<textBoxes_length;indexTB++)
            {
                current_textBox = textBoxes[indexTB];
                current_label = textBox_label_dict[current_textBox];
                left_referenceBorder = current_textBox.Location.X;
                right_referenceBorder = current_textBox.Location.X + current_textBox.Width;
                top_referenceBorder = (int)Math.Round(0.21 * this.StandardConnectScpsTopInteractions_panel.Height);
                bottom_referenceBorder = current_textBox.Location.Y;
                my_label = current_label;
                Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            left_referenceBorder = StandardConnectScpsTopInteractions_level_2_ownTextBox.Location.X;
            right_referenceBorder = StandardConnectScpsTopInteractions_level_3_ownTextBox.Location.X + StandardConnectScpsTopInteractions_level_3_ownTextBox.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.StandardConnectScpsTopInteractions_level_2_label.Location.Y;
            my_label = this.StandardConnectScpsTopInteractions_scpLevel_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = StandardConnectScpsTopInteractions_level_2_ownTextBox.Location.X;
            top_referenceBorder = StandardConnectScpsTopInteractions_level_2_label.Location.Y;
            bottom_referenceBorder = StandardConnectScpsTopInteractions_panel.Height;
            my_label = this.StandardConnectScpsTopInteractions_connect_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Dynamic scp networks panel
            int shared_cbButton_widthHeight_dynamicSCP = shared_cbButton_widthHeight_standardSCP;
            int shared_min_distance_X_from_panelSides_dynamicSCP = shared_min_distance_X_from_panelSides_standardSCP;
            int top_referenceBorder_checkBoxes_dynamicSCP = top_referenceBorder_checkBoxes_standardSCP;

            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = bottom_of_previous_panel + distance_between_panels;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.24 * (double)Overall_panel.Height);
            my_panel = Dynamic_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            bottom_of_previous_panel = Dynamic_panel.Location.Y + Dynamic_panel.Height;

            left_referenceBorder = shared_min_distance_X_from_panelSides_dynamicSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            top_referenceBorder = top_referenceBorder_checkBoxes_dynamicSCP;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicParentChild_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.7F * Dynamic_panel.Width);
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicAddGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DynamicParentChild_cbButton.Location.X + DynamicParentChild_cbButton.Width;
            right_referenceBorder = this.DynamicAddGenes_cbButton.Location.X;
            my_label = DynamicParentChild_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DynamicAddGenes_cbButton.Location.X + DynamicAddGenes_cbButton.Width;
            right_referenceBorder = this.Dynamic_panel.Width - shared_min_distance_X_from_panelSides_dynamicSCP;
            my_label = DynamicAddGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_dynamicSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicGroupSameLevelSCPs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            //left_referenceBorder = DynamicGroupSameLevelSCPs_cbButton.Location.X + DynamicGroupSameLevelSCPs_cbButton.Width;
            //right_referenceBorder = (int)Math.Round(0.65F*this.Dynamic_panel.Width);
            //my_label = DynamicGroupSameLevelSCPs_cbLabel;
            //Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_dynamicSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = bottom_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicConnectAllRelated_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DynamicConnectAllRelated_cbButton.Location.X + DynamicConnectAllRelated_cbButton.Width;
            right_referenceBorder = this.Dynamic_panel.Width - shared_min_distance_X_from_panelSides_dynamicSCP;
            my_label = DynamicConnectAllRelated_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);            Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label();

            left_referenceBorder = 0;
            right_referenceBorder = Dynamic_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = DynamicParentChild_cbButton.Location.Y;
            my_label = this.Dynamic_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Comments panel
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = bottom_of_previous_panel + distance_between_panels;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.08F * this.Overall_panel.Height);
            this.Comments_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(Comments_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            bottom_of_previous_panel = Comments_panel.Location.Y + Comments_panel.Height;
            Update_standardDynamicAddGenes_myPanelLabel();
            #endregion

            #region Node size panel
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = bottom_of_previous_panel + distance_between_panels;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.25F * (double)Overall_panel.Height);
            my_panel = this.NodeSize_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder); ;
            bottom_of_previous_panel = NodeSize_panel.Location.Y + NodeSize_panel.Height;

            Dictionary<string, int> nodeSizePanelParameter_textBoxVerticalPosition_dict =  Get_nodeSizePanelParameter_textBoxVerticalPosition_dict_nodeSizePanelStartListBoxesPosition_nodeSizePanelStartListBoxesPosition_leftTextBoxBorder_and_rightTextBoxBorder(out int nodeSizePanel_start_listBoxes_position, out int oneFourth_nodeSize_panel_height, out int left_textBoxBorder, out int right_textBoxBorder);
            int half_size_nodeSize_panel = (int)Math.Round(0.5F * (NodeSize_panel.Height - nodeSizePanel_start_listBoxes_position));

            Get_shared_borders_for_nodeSizePanel(out int shared_nodeSize_listTextBox_topReferenceBorder, out int shared_nodeSize_listTextBox_bottomReferenceBorder, out int shared_nodeSize_headline_topReferenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = NodeSize_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = nodeSizePanel_start_listBoxes_position;
            my_label = NodeSize_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = left_textBoxBorder;
            right_referenceBorder = right_textBoxBorder;
            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Determinant"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            my_listBox = NodeSize_determinant_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Node diameter"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            my_textBox = NodeSize_diameterMax_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Scaling"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            my_listBox = NodeSize_scaling_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            right_referenceBorder = left_referenceBorder;
            left_referenceBorder = 0;
            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Determinant"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            my_label = NodeSize_determinant_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Node diameter"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            this.NodeSize_diameterMax_myPanelLabel.TextAlign = ContentAlignment.MiddleRight;
            NodeSize_diameterMax_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);

            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Scaling"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            my_label = NodeSize_scaling_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            //left_referenceBorder = NodeSize_determinant_ownListBox.Location.X + NodeSize_determinant_ownListBox.Width;
            //right_referenceBorder = left_referenceBorder + (int)Math.Round(0.1 * NodeSize_panel.Width);
            //top_referenceBorder = NodeSize_diameterMin_ownTextBox.Location.Y;
            //bottom_referenceBorder = NodeSize_diameterMin_ownTextBox.Location.Y + NodeSize_diameterMin_ownTextBox.Height;
            //my_label = NodeSize_diameterMin_label;
            //Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Update_nodeSize_maxDiamter_label_and_visibility_of_adoptTextSize_cbButton(options);

            int center_of_cbButton = nodeSizePanel_start_listBoxes_position + half_size_nodeSize_panel;

            //left_referenceBorder = NodeSize_determinant_ownListBox.Location.X + NodeSize_determinant_ownListBox.Width + (int)Math.Round(0.02F * NodeSize_panel.Width);
            //right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            //top_referenceBorder = center_of_cbButton - (int)Math.Round(0.5F * shared_cbButton_widthHeight_standardSCP);
            //bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            //my_cbButton = NodeSize_adoptTextSize_cbButton;
            //Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            //left_referenceBorder = NodeSize_adoptTextSize_cbButton.Location.X + NodeSize_adoptTextSize_cbButton.Width;
            //right_referenceBorder = NodeSize_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            //top_referenceBorder = nodeSizePanel_start_listBoxes_position + (int)Math.Round(0F * NodeSize_panel.Height);
            //bottom_referenceBorder = NodeSize_panel.Height - (int)Math.Round(0F * NodeSize_panel.Height);
            //my_label = NodeSize_adoptTextSize_label;
            //Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int remaining_right_space = NodeSize_panel.Width - NodeSize_determinant_ownListBox.Location.X - NodeSize_determinant_ownListBox.Width;
            int middle_remaining_right_space = NodeSize_panel.Width - (int)Math.Round(0.5 * remaining_right_space);

            left_referenceBorder = middle_remaining_right_space - (int)Math.Round(0.5 * shared_cbButton_widthHeight_dynamicSCP);
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = nodeSizePanel_start_listBoxes_position;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = NodeSize_adoptTextSize_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = NodeSize_determinant_ownListBox.Location.X + NodeSize_determinant_ownListBox.Width;// + shared_min_distance_X_from_panelSides_standardSCP;
            right_referenceBorder = NodeSize_panel.Width;// - shared_min_distance_X_from_panelSides_standardSCP;
            top_referenceBorder = NodeSize_adoptTextSize_cbButton.Location.Y + NodeSize_adoptTextSize_cbButton.Height;
            bottom_referenceBorder = NodeSize_panel.Height - (int)Math.Round(0.05F * NodeSize_panel.Height);
            my_label = NodeSize_adoptTextSize_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_upper_reference_at_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Graph editor panel
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = bottom_of_previous_panel + distance_between_panels;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.095F * Overall_panel.Height);
            my_panel = this.GraphEditor_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder); ;
            bottom_of_previous_panel = GraphEditor_panel.Location.Y + GraphEditor_panel.Height;

            left_referenceBorder = left_textBoxBorder;
            right_referenceBorder = (int)Math.Round(0.98F*GraphEditor_panel.Width);
            top_referenceBorder = (int)Math.Round(0.015F * GraphEditor_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.65F * GraphEditor_panel.Height);
            my_listBox = GraphEditor_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = GraphEditor_ownListBox.Location.X;
            top_referenceBorder = GraphEditor_ownListBox.Location.Y;
            bottom_referenceBorder = top_referenceBorder + GraphEditor_ownListBox.Height;
            my_label = GraphEditor_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            #endregion

            int defaultButton_explanationButton_height = (int)Math.Round(0.05F * this.Overall_panel.Height);

            #region Generate nw check box and default button
            int start_remaining_bottom_area = bottom_of_previous_panel + shared_min_distance_X_from_panelSides_standardSCP;
            int end_remaining_bottom_area = Overall_panel.Height - shared_min_distance_X_from_panelSides_standardSCP;
            //if (Form_default_settings.Is_mono)
            //{
               // start_remaining_bottom_area -= shared_min_distance_X_from_panelSides_standardSCP;
               // end_remaining_bottom_area -= shared_min_distance_X_from_panelSides_standardSCP;
            //}
            int half_position_remaining_bottom_area = start_remaining_bottom_area + (int)Math.Round(0.5 * (Overall_panel.Height - start_remaining_bottom_area));

            int start_y_for_buttons = (int)Math.Round(0.35F * this.Overall_panel.Width);
            int size_y_for_buttons = Overall_panel.Width - shared_min_distance_X_from_panelSides_standardSCP - start_y_for_buttons;
            int distance_y_between_buttons = (int)Math.Round(0.0025F * this.Overall_panel.Width);
            int size_y_for_each_button = (int)Math.Round(0.3333F * (size_y_for_buttons - 2 * distance_y_between_buttons));

            left_referenceBorder = start_y_for_buttons;
            right_referenceBorder = left_referenceBorder + size_y_for_each_button;
            top_referenceBorder = start_remaining_bottom_area;
            bottom_referenceBorder = end_remaining_bottom_area;
            my_button = Default_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder + distance_y_between_buttons;
            right_referenceBorder = left_referenceBorder + size_y_for_each_button;
            top_referenceBorder = start_remaining_bottom_area;
            bottom_referenceBorder = end_remaining_bottom_area;
            my_button = Tutorial_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder + distance_y_between_buttons;
            right_referenceBorder = left_referenceBorder + size_y_for_each_button;
            top_referenceBorder = start_remaining_bottom_area;
            bottom_referenceBorder = end_remaining_bottom_area;
            my_button = Explanation_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);


            this.Generate_scp_networks_cbButton = Generate_scp_networks_cbButton;
            left_referenceBorder = Comments_panel.Location.X;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = half_position_remaining_bottom_area - (int)Math.Round(0.5F * shared_cbButton_widthHeight_standardSCP);
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = Generate_scp_networks_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Generate_scp_networks_cbButton.Location.X + Generate_scp_networks_cbButton.Width;
            right_referenceBorder = Default_button.Location.X;
            top_referenceBorder = half_position_remaining_bottom_area - (int)Math.Round(1F * (half_position_remaining_bottom_area - start_remaining_bottom_area));
            bottom_referenceBorder = half_position_remaining_bottom_area + (int)Math.Round(1F * (half_position_remaining_bottom_area - start_remaining_bottom_area));
            my_label = Generate_scp_networks_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            Set_default_colors();
            Copy_options_into_interface_selections(options);
        }
        private void Update_standard_and_dynamic_groupSameLevelSCPs_cb_and_labels(MBCO_network_based_integration_options_class options)
        {
            int extra_space = (int)Math.Round(0.025 * Standard_panel.Height);
            int top_referenceBorder = StandardGroupSameLevelSCPs_cbButton.Location.Y - extra_space;
            int bottom_referenceBorder = top_referenceBorder + StandardGroupSameLevelSCPs_cbButton.Height + extra_space;
            int left_referenceBorder;
            int right_referenceBorder = Standard_panel.Width;
            switch (options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    if (StandardGroupSameLevelSCPs_cbButton.Visible) { StandardGroupSameLevelSCPs_cbButton.Visible = false; }
                    StandardGroupSameLevelSCPs_cbLabel.Set_silent_text_without_adjustment_of_fontSize("SCPs of the same level can be grouped within Cytoscape");
                    left_referenceBorder = 0;
                    StandardGroupSameLevelSCPs_cbLabel.TextAlign = ContentAlignment.MiddleCenter;
                    break;
                case Graph_editor_enum.yED:
                    //StandardGroupSameLevelSCPs_cbButton.Visible = true;
                    StandardGroupSameLevelSCPs_cbLabel.Set_silent_text_without_adjustment_of_fontSize("Box SCPs of same level");
                    left_referenceBorder = StandardGroupSameLevelSCPs_cbButton.Location.X + StandardGroupSameLevelSCPs_cbButton.Width;
                    StandardGroupSameLevelSCPs_cbLabel.TextAlign = ContentAlignment.MiddleLeft;
                    break;
                default:
                    throw new Exception();
            }
            StandardGroupSameLevelSCPs_cbLabel.Font_style = FontStyle.Bold;
            StandardGroupSameLevelSCPs_cbLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            //StandardGroupSameLevelSCPs_cbLabel.Visible = true;

            extra_space = (int)Math.Round(0.025 * Dynamic_panel.Height);
            top_referenceBorder = DynamicGroupSameLevelSCPs_cbButton.Location.Y - extra_space;
            bottom_referenceBorder = top_referenceBorder + DynamicGroupSameLevelSCPs_cbButton.Height + extra_space;
            right_referenceBorder = Dynamic_panel.Width;
            switch (options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    DynamicGroupSameLevelSCPs_cbButton.Visible = false;
                    DynamicGroupSameLevelSCPs_cbLabel.Set_silent_text_without_adjustment_of_fontSize("SCPs of the same level can be grouped within Cytoscape");
                    left_referenceBorder = 0;
                    DynamicGroupSameLevelSCPs_cbLabel.TextAlign = ContentAlignment.MiddleCenter;
                    break;
                case Graph_editor_enum.yED:
                    DynamicGroupSameLevelSCPs_cbButton.Visible = true;
                    DynamicGroupSameLevelSCPs_cbLabel.Set_silent_text_without_adjustment_of_fontSize("Box SCPs of same level");
                    left_referenceBorder = DynamicGroupSameLevelSCPs_cbButton.Location.X + DynamicGroupSameLevelSCPs_cbButton.Width;
                    DynamicGroupSameLevelSCPs_cbLabel.TextAlign = ContentAlignment.MiddleLeft;
                    break;
                default:
                    throw new Exception();
            }
            DynamicGroupSameLevelSCPs_cbLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            DynamicGroupSameLevelSCPs_cbLabel.Font_style = FontStyle.Bold;
            DynamicGroupSameLevelSCPs_cbLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            DynamicGroupSameLevelSCPs_cbLabel.Visible = true;
        }
        private void Update_nodeSize_maxDiamter_label_and_visibility_of_adoptTextSize_cbButton(MBCO_network_based_integration_options_class options)
        {
            OwnTextBox my_textBox;
            int left_referenceBorder;
            int right_referenceBorder;
            int bottom_referenceBorder;
            int top_referenceBorder;
            Dictionary<string, int> nodeSizePanelParameter_textBoxVerticalPosition_dict = Get_nodeSizePanelParameter_textBoxVerticalPosition_dict_nodeSizePanelStartListBoxesPosition_nodeSizePanelStartListBoxesPosition_leftTextBoxBorder_and_rightTextBoxBorder(out int nodeSizePanel_start_listBoxes_position, out int oneFourth_nodeSize_panel_height, out int left_textBoxBorder, out int right_textBoxBorder);
            top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Label size"] * oneFourth_nodeSize_panel_height;
            bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            switch (options.Node_size_determinant)
            {
                case Yed_network_node_size_determinant_enum.Uniform:
                    NodeSize_diameterMax_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Diameter");
                    NodeLabel_maxSize_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Label size");
                    NodeSize_adoptTextSize_cbButton.Visible = false;
                    NodeSize_adoptTextSize_label.Visible = false;
                    NodeSize_scaling_label.Visible = false;
                    NodeSize_scaling_ownListBox.Visible = false;
                    NodeLabel_minSize_myPanelLabel.Visible = false;
                    NodeLabel_minSize_ownTextBox.Visible = false;
                    NodeLabel_maxSize_ownTextBox.Visible = false;
                    NodeLabel_uniqueSize_ownTextBox.Visible = true;
                    break;
                case Yed_network_node_size_determinant_enum.No_of_different_colors:
                case Yed_network_node_size_determinant_enum.No_of_sets:
                case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                    NodeSize_diameterMax_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Max diameter");
                    if (options.Adjust_labelSizes_to_nodeSizes)
                    {
                        NodeLabel_minSize_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Label size: Min");
                        NodeLabel_maxSize_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Max");
                        NodeLabel_minSize_myPanelLabel.Visible = true;
                        NodeLabel_minSize_ownTextBox.Visible = true;
                        NodeLabel_maxSize_ownTextBox.Visible = true;
                        NodeLabel_uniqueSize_ownTextBox.Visible = false;
                    }
                    else
                    {
                        NodeLabel_maxSize_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Label size");
                        NodeLabel_minSize_myPanelLabel.Visible = false;
                        NodeLabel_minSize_ownTextBox.Visible = false;
                        NodeLabel_maxSize_ownTextBox.Visible = false;
                        NodeLabel_uniqueSize_ownTextBox.Visible = true;
                    }
                    NodeSize_adoptTextSize_cbButton.Visible = true;
                    NodeSize_adoptTextSize_label.Visible = true;
                    NodeSize_scaling_label.Visible = true;
                    NodeSize_scaling_ownListBox.Visible = true;
                    break;
                default:
                    throw new Exception();
            }
            if (NodeLabel_minSize_ownTextBox.Visible)
            {
                int textBox_width = (int)Math.Round(0.32F * (right_textBoxBorder-left_textBoxBorder));
                left_referenceBorder = left_textBoxBorder;
                right_referenceBorder = left_referenceBorder + textBox_width;
                my_textBox = NodeLabel_minSize_ownTextBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                right_referenceBorder = left_referenceBorder;
                left_referenceBorder = 0;
                NodeLabel_minSize_myPanelLabel.TextAlign = ContentAlignment.MiddleRight;
                NodeLabel_minSize_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);

                right_referenceBorder = right_textBoxBorder;
                left_referenceBorder = right_referenceBorder - textBox_width;
                my_textBox = NodeLabel_maxSize_ownTextBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                right_referenceBorder = left_referenceBorder;
                left_referenceBorder = NodeLabel_minSize_ownTextBox.Location.X + NodeLabel_minSize_ownTextBox.Width;
                NodeLabel_maxSize_myPanelLabel.TextAlign = ContentAlignment.MiddleRight;
                NodeLabel_maxSize_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);

            }
            else
            {
                left_referenceBorder = (int)Math.Round(0.5F * NodeSize_panel.Width);
                right_referenceBorder = (int)Math.Round(0.85F * NodeSize_panel.Width);
                my_textBox = NodeLabel_uniqueSize_ownTextBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                right_referenceBorder = left_referenceBorder;
                left_referenceBorder = 0;
                top_referenceBorder = nodeSizePanelParameter_textBoxVerticalPosition_dict["Label size"] * oneFourth_nodeSize_panel_height;
                bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
                NodeLabel_maxSize_myPanelLabel.TextAlign = ContentAlignment.MiddleRight;
                NodeLabel_maxSize_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            }

            //top_referenceBorder = bottom_referenceBorder;
            //bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            //this.NodeLabel_minSize_myPanelLabel.TextAlign = ContentAlignment.MiddleRight;
            //NodeLabel_minSize_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);



            //left_referenceBorder = 0;
            //right_referenceBorder = NodeSize_panel.Width;


            //top_referenceBorder = bottom_referenceBorder;
            //bottom_referenceBorder = top_referenceBorder + oneFourth_nodeSize_panel_height;
            //my_textBox = NodeLabel_minSize_ownTextBox;
            //Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);


            left_referenceBorder = 0;
            right_referenceBorder = NodeSize_diameterMax_ownTextBox.Location.X;
            top_referenceBorder = NodeSize_diameterMax_ownTextBox.Location.Y;
            bottom_referenceBorder = NodeSize_diameterMax_ownTextBox.Location.Y + NodeSize_diameterMax_ownTextBox.Height;
            this.NodeSize_diameterMax_myPanelLabel.TextAlign = ContentAlignment.MiddleRight;
            NodeSize_diameterMax_myPanelLabel.Font_style = FontStyle.Bold;
            NodeSize_diameterMax_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }
        private void Update_graphFileExtension_myPanelLabel()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            this.GraphFileExtension_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            left_referenceBorder = 0;
            right_referenceBorder = GraphEditor_panel.Width;
            top_referenceBorder = GraphEditor_ownListBox.Location.Y + GraphEditor_ownListBox.Height;
            bottom_referenceBorder = GraphEditor_panel.Height;
            string graphEditor_string = this.GraphEditor_ownListBox.SelectedItem.ToString();
            string file_extension = "error";
            if (!String.IsNullOrEmpty(graphEditor_string))
            {
                Graph_editor_enum graphEditor = ListBoxString_graphEditor_dict[graphEditor_string];
                switch (graphEditor)
                {
                    case Graph_editor_enum.Cytoscape:
                        file_extension = "Network / style file extensions: 'xgmml' / 'xml'";
                        break;
                    case Graph_editor_enum.yED:
                        file_extension = "Network file extension: 'graphml'";
                        break;
                    default:
                        throw new Exception();
                }
                GraphFileExtension_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(file_extension);
                GraphFileExtension_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            }
        }
        private void Update_standardDynamicAddGenes_myPanelLabel()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            this.Comments_standardDynamicAddGenes_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            left_referenceBorder = 0;
            right_referenceBorder = Comments_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = Comments_panel.Height;
            Comments_standardDynamicAddGenes_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }
        private void Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label()
        {
            int left_referenceBorder = 0;
            int right_referenceBorder = Dynamic_panel.Width;
            int top_referenceBorder = this.DynamicConnectAllRelated_cbButton.Location.Y + this.DynamicConnectAllRelated_cbButton.Height;
            int bottom_referenceBorder = this.Dynamic_panel.Height - (int)Math.Round(0.02 * this.Dynamic_panel.Height);
            this.DynamicConnectAllScps_explanation_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.DynamicConnectAllScps_explanation_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }

        public void Set_default_colors()
        {
            Color panel_color = Color.Black;
            Color headline_color = Color.DimGray;
            StandardConnectScpsTopInteractions_level_2_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            StandardConnectScpsTopInteractions_level_3_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
        }

        private void Update_visibility_of_topInteractions_labels_and_textBoxes(MBCO_network_based_integration_options_class options)
        {
            bool is_visible = false;
            if (Generate_scp_networks_cbButton.Checked)
            {
                Ontology_type_enum reference_ontology = options.Next_ontology;
                bool visualize_mbco_selective_parameter = Ontology_classification_class.Is_mbco_ontology(reference_ontology);
                bool is_GO = Ontology_classification_class.Is_go_ontology(reference_ontology);

                Dynamic_panel.Visible = visualize_mbco_selective_parameter;
                Standard_panel.Visible = true;
                StandardConnectRelated_cbButton.Visible = visualize_mbco_selective_parameter;
                StandardConnectRelated_cbLabel.Visible = visualize_mbco_selective_parameter;
                StandardGroupSameLevelSCPs_cbButton.Visible = visualize_mbco_selective_parameter;
                StandardGroupSameLevelSCPs_cbLabel.Visible = visualize_mbco_selective_parameter;
                StandardParentChild_cbButton.Visible = visualize_mbco_selective_parameter;
                StandardParentChild_cbLabel.Visible = visualize_mbco_selective_parameter;
                ParentChildSCPNetGeneration_label.Visible = !visualize_mbco_selective_parameter;
                ParentChildSCPNetGeneration_ownListBox.Visible = !visualize_mbco_selective_parameter;
                HierarchicalScpInteractions_ownListBox.Visible = is_GO;
                HierarchicalScpInteractions_label.Visible = HierarchicalScpInteractions_ownListBox.Visible;

                Default_button.Visible = true;
                Comments_panel.Visible = true;
                NodeSize_panel.Visible = true;
                if ((this.StandardConnectRelated_cbButton.Checked)&&(visualize_mbco_selective_parameter))
                {
                    is_visible = true;
                }
                else { is_visible = false; }
                string genes_to_youngest_child_text = "Genes will be added to the youngest child SCPs";
                string genes_to_every_scp_text = "Genes will be added to every SCP";
                string gene_addition_rules_differ = "Gene addition rules differ for standard and dynamic enrichment analysis";
                string predicted_by_dynamic_text = " predicted by dynamic enrichment analysis";
                string predicted_by_standard_text = " predicted by standard enrichment analysis";
                string predicted_by_both_text = " predicted by both enrichment analyses";
                this.StandardConnectScpsTopInteractions_panel.Visible = is_visible;
                if ((options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                    && (options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                    && (options.Add_parent_child_relationships(Enrichment_type_enum.Standard))
                    && (options.Add_parent_child_relationships(Enrichment_type_enum.Dynamic))
                    )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(genes_to_youngest_child_text + predicted_by_both_text);
                }
                else if ((options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                    && (options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                    && (!options.Add_parent_child_relationships(Enrichment_type_enum.Standard))
                    && (!options.Add_parent_child_relationships(Enrichment_type_enum.Dynamic))
                    )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(genes_to_every_scp_text + predicted_by_both_text);
                }
                else if ((options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                    && (options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                    && (options.Add_parent_child_relationships(Enrichment_type_enum.Standard)
                        !=options.Add_parent_child_relationships(Enrichment_type_enum.Dynamic))
                    )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(gene_addition_rules_differ);
                }
                else if ((options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                            && (!options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                            && (options.Add_parent_child_relationships(Enrichment_type_enum.Standard))
                        )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(genes_to_youngest_child_text + predicted_by_standard_text);
                }
                else if ((options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                            && (!options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                            && (!options.Add_parent_child_relationships(Enrichment_type_enum.Standard))
                        )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(genes_to_every_scp_text + predicted_by_standard_text);
                }
                else if ((options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                            && (!options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                            && (options.Add_parent_child_relationships(Enrichment_type_enum.Dynamic))
                        )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(genes_to_youngest_child_text + predicted_by_dynamic_text);
                }
                else if ((options.Add_genes_to_networks(Enrichment_type_enum.Dynamic))
                            && (!options.Add_genes_to_networks(Enrichment_type_enum.Standard))
                            && (!options.Add_parent_child_relationships(Enrichment_type_enum.Dynamic))
                        )
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(genes_to_every_scp_text + predicted_by_dynamic_text);
                }
                else
                {
                    this.Comments_standardDynamicAddGenes_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("");
                }
                if (this.DynamicConnectAllRelated_cbButton.Checked)
                {
                    DynamicConnectAllScps_explanation_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("(All related SCPs in the network will be connected based on parameters specified in menu 'Enrichment'.)");
                    Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label();
                }
                else
                {
                    DynamicConnectAllScps_explanation_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("(Only combined SCPs forming a significant higher-level SCP will be connected.)");
                    Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label();
                }
                Update_standardDynamicAddGenes_myPanelLabel();
                Update_standard_and_dynamic_groupSameLevelSCPs_cb_and_labels(options);
                Update_nodeSize_maxDiamter_label_and_visibility_of_adoptTextSize_cbButton(options);
            }
            else
            {
                Dynamic_panel.Visible = false;
                Standard_panel.Visible = false;
                Default_button.Visible = false;
                Comments_panel.Visible = false;
                NodeSize_panel.Visible = false;
            }
        }

        public void Set_visibility(bool visible, MBCO_network_based_integration_options_class options)
        {
            Copy_options_into_interface_selections(options);
            Set_default_colors();
            this.Overall_panel.Visible = visible;
            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
            this.Overall_panel.Refresh();
        }

        public void Copy_options_into_interface_selections(MBCO_network_based_integration_options_class options)
        {
            StandardParentChild_cbButton.SilentChecked = options.Add_parent_child_relationships_to_standard_SCP_networks;
            DynamicParentChild_cbButton.SilentChecked = options.Add_parent_child_relationships_to_dynamic_SCP_networks;
            StandardAddGenes_cbButton.SilentChecked = options.Add_genes_to_standard_networks;
            DynamicAddGenes_cbButton.SilentChecked = options.Add_genes_to_dynamic_networks;
            StandardConnectRelated_cbButton.SilentChecked = options.Add_edges_that_connect_standard_scps;
            StandardConnectScpsTopInteractions_level_2_ownTextBox.SilentText = (options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[2] * 100).ToString();
            StandardConnectScpsTopInteractions_level_3_ownTextBox.SilentText = (options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[3] * 100).ToString();
            DynamicConnectAllRelated_cbButton.SilentChecked = options.Add_additional_edges_that_connect_dynamic_scps;
            Generate_scp_networks_cbButton.SilentChecked = options.Generate_scp_networks;
            DynamicGroupSameLevelSCPs_cbButton.SilentChecked = options.Box_sameLevel_scps_for_dynamic_enrichment;
            StandardGroupSameLevelSCPs_cbButton.SilentChecked = options.Box_sameLevel_scps_for_standard_enrichment;
            NodeSize_determinant_ownListBox.SilentSelectedIndex_and_topIndex = NodeSize_determinant_ownListBox.Items.IndexOf(NetworkDeterminantSize_listBoxString_dict[options.Node_size_determinant]);
            NodeSize_diameterMax_ownTextBox.SilentText = options.Node_size_diameterMax_for_current_nodeSize_determinant.ToString();
            NodeLabel_minSize_ownTextBox.SilentText = options.Label_minSize_for_current_nodeSize_determinant.ToString();
            NodeLabel_maxSize_ownTextBox.SilentText = options.Label_maxSize_for_current_nodeSize_determinant.ToString();
            NodeLabel_uniqueSize_ownTextBox.SilentText = options.Label_uniqueSize_for_current_nodeSize_determinant.ToString();
            NodeSize_adoptTextSize_cbButton.SilentChecked = options.Adjust_labelSizes_to_nodeSizes;
            NodeSize_scaling_ownListBox.SilentSelectedIndex_and_topIndex = NodeSize_scaling_ownListBox.Items.IndexOf(NodeSizeScaling_listBoxString_dict[options.Node_size_scaling_across_plots]);
            ParentChildSCPNetGeneration_ownListBox.SilentSelectedIndex_and_topIndex = ParentChildSCPNetGeneration_ownListBox.Items.IndexOf(ParentChildScpNetGen_listBoxString_dict[options.Predicted_scp_hierarchy_integration_strategy]);
            HierarchicalScpInteractions_ownListBox.SilentSelectedIndex_and_topIndex = HierarchicalScpInteractions_ownListBox.Items.IndexOf(HierarchicalScpInteraction_listBoxString_dict[options.Next_scp_hierachical_interactions]);

            GraphEditor_ownListBox.SilentSelectedIndex_and_topIndex = GraphEditor_ownListBox.Items.IndexOf(GraphEditor_listBoxString_dict[options.Graph_editor]);

            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
            Update_graphFileExtension_myPanelLabel();
        }
        public MBCO_network_based_integration_options_class Copy_interfaceSelections_excluding_textBoxes_into_options(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            options.Add_parent_child_relationships_to_standard_SCP_networks = StandardParentChild_cbButton.Checked;
            options.Add_parent_child_relationships_to_dynamic_SCP_networks = DynamicParentChild_cbButton.Checked;
            options.Add_genes_to_standard_networks = StandardAddGenes_cbButton.Checked;
            options.Add_genes_to_dynamic_networks = DynamicAddGenes_cbButton.Checked;
            options.Add_edges_that_connect_standard_scps = StandardConnectRelated_cbButton.Checked;
            options.Add_additional_edges_that_connect_dynamic_scps = DynamicConnectAllRelated_cbButton.Checked;
            options.Box_sameLevel_scps_for_standard_enrichment = StandardGroupSameLevelSCPs_cbButton.Checked;
            options.Box_sameLevel_scps_for_dynamic_enrichment = DynamicGroupSameLevelSCPs_cbButton.Checked;

            options.Generate_scp_networks = Generate_scp_networks_cbButton.Checked;
            options.Node_size_determinant = ListBoxString_networkDeterminantSize_dict[NodeSize_determinant_ownListBox.SelectedItem.ToString()];
            options.Adjust_labelSizes_to_nodeSizes = NodeSize_adoptTextSize_cbButton.Checked;

            //Update min a second time, since min has to be lower than max
            options.Generate_scp_networks = Generate_scp_networks_cbButton.Checked;
            options.Node_size_determinant = ListBoxString_networkDeterminantSize_dict[NodeSize_determinant_ownListBox.SelectedItem.ToString()];
            options.Node_size_scaling_across_plots = ListBoxString_nodeSizeScaling_dict[NodeSize_scaling_ownListBox.SelectedItem.ToString()];
            options.Predicted_scp_hierarchy_integration_strategy = ListBoxString_parentChildScpNetGen_dict[ParentChildSCPNetGeneration_ownListBox.SelectedItem.ToString()];
            options.Next_scp_hierachical_interactions = ListBoxString_hierarchicalScpInteraction_dict[HierarchicalScpInteractions_ownListBox.SelectedItem.ToString()];   

            options.Graph_editor = ListBoxString_graphEditor_dict[GraphEditor_ownListBox.SelectedItem.ToString()];

            Copy_options_into_interface_selections(options);
            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
            Update_graphFileExtension_myPanelLabel();
            return options;
        }
        public MBCO_network_based_integration_options_class StandardTopLevel_2_interactions_ownTextBox_TextChanged(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            #region Update top quantiles and text box for level 2
            float[] top_quantiles = Array_class.Deep_copy_array(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
            int percent;
            if (int.TryParse(StandardConnectScpsTopInteractions_level_2_ownTextBox.Text, out percent))
            { top_quantiles[2] = (float)percent / 100F; }
            else { top_quantiles[2] = -1; } // ensures mismatch with max min range
            options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = top_quantiles;
            if (Array_class.Arrays_are_equal(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level, top_quantiles))
            { StandardConnectScpsTopInteractions_level_2_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { StandardConnectScpsTopInteractions_level_2_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            #endregion

            return options;
        }

        public MBCO_network_based_integration_options_class StandardTopLevel_3_interactions_ownTextBox_TextChanged(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            #region Update top quantiles and text box for level 3
            float[]top_quantiles = Array_class.Deep_copy_array(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
            int percent;
            if (int.TryParse(StandardConnectScpsTopInteractions_level_3_ownTextBox.Text, out percent))
            { top_quantiles[3] = (float)percent / 100F; }
            else { top_quantiles[3] = -1; } // ensures mismatch with max min range
            options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = top_quantiles;
            if (Array_class.Arrays_are_equal(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level, top_quantiles))
            { StandardConnectScpsTopInteractions_level_3_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { StandardConnectScpsTopInteractions_level_3_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            #endregion

            return options;
        }

        public MBCO_network_based_integration_options_class NodeSizes_maxDiameter_ownTextBox_TextChanged(MBCO_network_based_integration_options_class options)
        {
            int potential_max_diamter_value;
            if (int.TryParse(NodeSize_diameterMax_ownTextBox.Text, out potential_max_diamter_value))
            {
                options.Node_size_diameterMax_for_current_nodeSize_determinant = potential_max_diamter_value;
            }
            if (options.Node_size_diameterMax_for_current_nodeSize_determinant.Equals(potential_max_diamter_value)) { NodeSize_diameterMax_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { NodeSize_diameterMax_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return options;
        }

        public MBCO_network_based_integration_options_class NodeSizes_label_minSize_ownTextBox_TextChanged(MBCO_network_based_integration_options_class options)
        {
            int potential_label_size_value;
            if (int.TryParse(NodeLabel_minSize_ownTextBox.Text, out potential_label_size_value))
            {
                options.Label_minSize_for_current_nodeSize_determinant = potential_label_size_value;
            }
            if (options.Label_minSize_for_current_nodeSize_determinant.Equals(potential_label_size_value)) { NodeLabel_minSize_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { NodeLabel_minSize_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return options;
        }
        public MBCO_network_based_integration_options_class NodeSizes_label_maxSize_ownTextBox_TextChanged(MBCO_network_based_integration_options_class options)
        {
            int potential_label_size_value;
            if (int.TryParse(NodeLabel_maxSize_ownTextBox.Text, out potential_label_size_value))
            {
                options.Label_maxSize_for_current_nodeSize_determinant = potential_label_size_value;
            }
            if (options.Label_maxSize_for_current_nodeSize_determinant.Equals(potential_label_size_value)) { NodeLabel_maxSize_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { NodeLabel_maxSize_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return options;
        }
        public MBCO_network_based_integration_options_class NodeSizes_label_uniqueSize_ownTextBox_TextChanged(MBCO_network_based_integration_options_class options)
        {
            int potential_label_size_value;
            if (int.TryParse(NodeLabel_uniqueSize_ownTextBox.Text, out potential_label_size_value))
            {
                options.Label_uniqueSize_for_current_nodeSize_determinant = potential_label_size_value;
            }
            if (options.Label_uniqueSize_for_current_nodeSize_determinant.Equals(potential_label_size_value)) { NodeLabel_uniqueSize_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { NodeLabel_uniqueSize_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return options;
        }

        #region Explanation, Tutorial
        public bool Is_activated_explanation_or_tutorial_button(Button given_button)
        {
            return given_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
        }
        public void Set_explanation_and_tutorial_button_to_inactive()
        {
            this.Explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Explanation_button.Refresh();
            this.Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            this.Tutorial_button.Refresh();
        }
        public void Set_selected_explanation_or_tutorial_button_to_activated(Button selected_button)
        {
            selected_button.BackColor = Form_default_settings.Color_button_pressed_back;
            selected_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            selected_button.Refresh();
        }

        private void Write_explanation_into_error_reports_panel()
        {
            Error_reports_headline_label.Text = "SCP networks";
            string text = "Pathways or SCPs meeting the significance criteria defined in the ‘Enrichment’-menu are visualized in pathway/SCP networks. The application merges all networks generated for " +
                "all datasets annotated to the same integration group. Pathways are visualized as pie charts where each slice represents one dataset that predicted that pathway with significance." +
                "\r\n" +
                "\r\nIf MBCO is selected, the application will generate separate SCP-networks for standard and dynamic enrichment analysis. Since datasets are only be subjected to dynamic enrichment " +
                "analysis, if MBCO is selected, pathway networks will only be generated from standard enrichment results, if any other ontology is selected. For details about dynamic enrichment analysis " +
                "or the inferred networks of functional interactions between level-2 or -3 SCPs that are used for dynamic enrichment analysis, see the explanation texts in the ‘Enrichment’-menu or Hansen et al. Sci Rep. 2017." +
                Form_default_settings.Explanation_text_major_separator +
                "Standard enrichment analysis (using MBCO)" +
                "\r\n" +
                "\r\nConnecting parent and child SCPs:" +
                "\r\nSCPs in annotated parent-child relationships are connected using solid arrows pointing from parents to their children. " +
                "In the case of MBCO and if selected when using other ontologies (see below), all ancestors of a predicted subcellular process (SCP) or pathway will always be shown. If they are not among the significant " +
                "predictions, they will be added as white intermediate SCPs/pathways. In the case of MBCO, the oldest parent is always a level-1 SCP. With each solid arrow the level number increases by 1, so that the level " +
                "of an MBCO SCP can be identified from its hierarchical position." +
                "\r\n" +
                "\r\nBox SCPs of the same level:" +
                "\r\nThis option is only available if MBCO is selected for enrichment analysis. If selected, SCPs of the same " +
                "level will be boxed. If 'Add genes' is selected, all added genes will be boxed in an independent box." +
                "\r\n" +
                "\r\nAdding genes:" +
                "\r\nIf selected, all genes that are part of a user-supplied dataset and a predicted " +
                "SCP or pathway will be added as children to that SCP/pathway. " +
                "\r\nIf the option to connect parent and child SCPs/pathways is selected, the genes will only be added to the youngest child predicted by any dataset, " +
                "even if that child SCP/pathway was not predicted by the dataset the gene belongs to. Since only genes that map to significant SCPs/pathways will be added, an existing gene of a dataset of interest will not be " +
                "added to any SCP/pathway predicted by any other dataset, if neither that SCP/pathway nor any of its parent SCPs/pathways were predicted for the dataset of interest." +
                "\r\nTo identify the significant SCP/pathway of a " +
                "dataset a gene of the same dataset belongs to, simply follow the parent-child hierarchy upwards until encountering an SCP/pathway of the same dataset." +
                "\r\nPlease note that the hierarchical organization of many " +
                "ontologies including GO and Reactome is a directed acyclic graph, so that a gene mapping to a child SCP/pathway can be part of multiple predicted parent SCP/pathways. In contrast, the hierarchical organization " +
                "of MBCO represents a tree, so that there is only one possible ancestor SCPs within each level the gene maps to." +
                "\r\nDepending on the data, gene nodes may appear very small when node size is set to be proportional to the sum of all –log10(p-values). In such cases, selecting a different metric in the SCP node areas list " +
                "will increase the gene node sizes." +
                "\r\n" +
                "\r\nConnecting related SCPs:" +
                "\r\nThe annotated MBCO hierarchy is enriched using a unique MBCO " +
                "algorithm that infers interactions between functionally related SCPs of the levels-2 or -3. If this check box is selected any predicted SCPs will be connected, if their interaction is among the top % of SCP " +
                "interactions, specified in the showing-up parameter field. This option is only available when MBCO is used for enrichment analysis." +
                "\r\n" +
                "\r\nConnecting related SCPs in NW define abbreviation using top % SCP interactions:" +
                "\r\nThe inferred interactions between functionally related level-2 or -3 MBCO SCPs are weighted and ranked by their interaction strength. The user can specify how many (in percent) of the inferred " +
                "top ranked interactions are considered to connect predicted SCPs. This option is only available when MBCO is used for enrichment analysis." +
                Form_default_settings.Explanation_text_major_separator +
                "Predicted SCP networks using other ontologies" +
                "\r\n" +
                "\r\nComplement predicted SCP networks with:" +
                "\r\nSince ontologies such as GO can contain many pathways that are often organized in an extensive hierarchy, the user can select if predicted pathways shall be integrated into the hierarchy by including all their " +
                "ancestors or only intermediate pathways that lie between predicted pathways. This option is not available if MBCO was selected. Since MBCO SCPs are annotated to only four SCP levels, the application will always add " +
                "all ancestor pathways to allow identification of SCP levels by counting the distance from the oldest (level-1) ancestor." +
                "\r\n" +
                "\r\nGO term relationships:" +
                "\r\nSince Gene Ontology also contains regulatory relationships " +
                "between its terms, the parent child hierarchy used for pathway integration and visualization can be extended with these interactions. The selection has no influence on the population of parent terms with the genes " +
                "of their child terms. This option is only available when using one of the three GO namespaces for enrichment analysis." +
                Form_default_settings.Explanation_text_major_separator +
                "Dynamic enrichment analysis (using MBCO)" +
                "\r\nUser-supplied datasets are only subjected to " +
                "dynamic enrichment analysis if MBCO is selected. The following options for the generation of SCP networks generated from dynamic enrichment analysis are consequently only available if MBCO is selected." +
                "\r\n" +
                "\r\nConnecting parent and child SCPs:" +
                "\r\nSame functionality as described under Standard enrichment analysis. Since dynamic enrichment analysis can generate networks that are already highly connected, " +
                "we recommend reducing the number of datasets annotated to the same integration group (using the ‘Organize data’-menu), if parent and child SCPs shall additionally be connected." +
                "\r\n" +
                "\r\nBox SCPs of the same level:" +
                "\r\nSame functionality as described under Standard enrichment analysis" +
                "\r\n" +
                "\r\nAdding genes:" +
                "\r\nSame functionality as described under Standard enrichment analysis. Since dynamic enrichment analysis can generate " +
                "networks that are already highly connected without addition of SCP genes, we recommend reducing the number of datasets annotated to the same integration group, switching off the ‘Connect all related SCPs’ and " +
                "‘Connect parent and child SCPs’ check boxes, if genes shall be added." +
                "\r\n" +
                "\r\nConnecting all related SCPs:" +
                "\r\nUnchecked, only those SCPs will be connected that were combined to form a dataset-specific SCP " +
                "combination that meets the significance criteria that are defined in the menu panel ‘Enrichment’. The combined SCPs label the same bar in the bar diagram charts for dynamic enrichment analysis. The results " +
                "text files that document the significant predictions for dynamic enrichment analysis, lists these SCPs as one entry in the column ‘SCP’, separated by dollar signs. Since an SCP can be part of multiple higher-level " +
                "SCPs, it can be connected to more than two other SCPs." +
                "\r\nIf the check box is checked, any SCPs will be connected, if their interaction is among the top percentages of considered SCP interactions, no matter if they " +
                "are part of the same significant dataset-selective SCP combination or not. If checked, SCPs will also be connected across different datasets. The top percentages of considered SCP interactions are the same as those " +
                "used for the dynamic enrichment analysis algorithm and can be modified in the menu panel ‘Enrichment’." +
                Form_default_settings.Explanation_text_major_separator +
                "SCP node size and SCP node areas:" +
                "\r\nAny SCP that is significantly predicted for at least one dataset can be " +
                "visualized as a circle in the network. SCPs predicted for multiple datasets will contain multiple pie slices, one slice for each dataset. Here it can be specified what determines the area of the circle and of each pie " +
                "slice." +
                "\r\n" +
                "\r\n'~ -log10(p-values)':" +
                "\r\nThis selection will lead to the generation of nodes and pie slices whose areas are proportional to the sum of all -log10(p-values) obtained for any visualized dataset and to the " +
                "-log10(p-value) of the related dataset, respectively." +
                "\r\nThe dynamic enrichment algorithm generates dataset-selective SCP combinations by merging two or three functionally related SCPs that contain at least one expressed " +
                "gene and adds them to the ontology before enrichment analysis (See menu 'Enrichment' for details). In consequence, each -log10(p-value) can be associated with one to three SCPs and each SCP can be part of multiple " +
                "predictions. Our algorithm equally splits the  log10(p-value) of each prediction over all associated SCPs. For each SCP it will calculate the sum of all split -log10(p-values) it is associated with. This ensures that " +
                "network visualization documents higher importance for those SCPs that are part of multiple predictions and/or are predicted with high significance." +
                "\r\nSince pie charts are generated within the C# script and circles are " +
                "regular yED or Cytoscape elements, the legend will show the areas for both circles and pie charts for quality control. Both are equally sized, so simply delete on set of those legend nodes." +
                "\r\n" +
                "\r\n'~ # of datasets'/'~ # of colors':" +
                "\r\nThese selections will lead to the generation of nodes whose areas will be proportional to the number of datasets or number of different colors of the datasets that significantly predicted an SCP of interest, " +
                "respectively. The latter selection can, for example, be used if one assay, e.g., single cell RNAseq, generates multiple different datasets, e.g. subtypes of the same cell type, while another assay, e.g., spatial proteomics, " +
                "only generates one related dataset, e.g. the related subsegement. Under both selections, all pie slices of the same circle will be equally sized." +
                "\r\n" +
                "\r\n'uniform':" +
                "\r\nThis selection will generate circles of the same size, " +
                "independently of significance values or number of datasets or colors. Pie slices of the same circle will be equally sized." +
                "\r\n" +
                "\r\nIf Add genes is selected, each gene is assigned the smallest SCP slice-area value from the analyzed datasets, multiplied by 0.75. Gene node areas are " +
                "then calculated using the same algorithm applied to SCP nodes. When SCP slice areas are scaled according to –log10(p-values), the resulting gene nodes may become very small. " +
                "In such cases, choosing a different node-size metric will produce gene nodes with more regular, visually interpretable sizes." +
                "\r\n" +
                "\r\nMax diameter/Diameter:" +
                "\r\nDepending on the selection under 'SCP node " +
                "areas' the user can define the maximal or uniform diameter of any SCP node. The selection also influences the size of the eventually added gene nodes, as described above." +
                "\r\n" +
                "\r\nLabel size:" +
                "\r\nDepending on the selection under " +
                "'SCP node areas' and 'Label sizes ~ node sizes' the user can either define the uniform font size or the range of font sizes for all node labels." +
                "\r\n" +
                "\r\nScaling in networks:" +
                "\r\nHere, the user can select if the mapping of node sizes " +
                "to the selected data points should be unique for each network or uniform across all networks of the same enrichment type (i.e., standard or dynamic)." +
                "\r\n" +
                "\r\nLabel sizes ~ node sizes:" +
                "\r\nThe user can specify if the node label font " +
                "size should increase in size with increased SCP/pathways node area by activating the related button." +
                "\r\n" +
                "\r\nComment on the order of pie slices:" +
                "\r\nThe clockwise order of dataset pie slices (starting at 3h) follows the dataset order " +
                "that can be specified in the menu panel 'Organize Data'." +
                "\r\n" +
                "\r\nComment on network visualization software:" +
                "\r\nWithin yED Graph Editor pie charts canl be visualized as gray rectangles and node labels disappear after zooming out " +
                "5x from the 1:1 level. Decreasing node diameter and label sizes allows visualization of more nodes within the same zoom level, which can be helpful on smaller screens." +
                Form_default_settings.Explanation_text_major_separator +
                "Graph editor"+
                "\r\nThe user can instruct the application to generate network files for the yED Graph Editor or Cytoscape. " + 
                "If yED Graph Editor is selected, the application will write graphml-files that can directly be opened with yED via (double) click. If Cytoscape is selected the user can import the network- and " +
                "style-files after opening Cytoscape. In both cases the application will write a 'ReadMe'-file with further instructions." +
                "\r\nFor download links of either editor see 'Download_all_datasets_windows.txt' or 'Download_all_datasets_linux.txt'." +
                Form_default_settings.Explanation_text_major_separator +
                "Generate SCP networks" +
                "\r\nThe user can switch off the network generation.";


            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            Error_reports_ownTextBox.SilentText_and_refresh = text;
            int left = Error_reports_ownTextBox.Location.X;
            int right = left + Error_reports_ownTextBox.Width;
            int top = Error_reports_ownTextBox.Location.Y;
            int bottom = top + Error_reports_ownTextBox.Height;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToExactPositions_add_default_parameter(Error_reports_ownTextBox, left, right, top, bottom);
        }

        public void Explanation_button_activated()
        {
            Write_explanation_into_error_reports_panel();
        }
        public void Tutorial_button_activated(Ontology_type_enum selected_ontology)
        {
            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel;
            int mid_y_position;
            int right_x_position;
            string text;

            string reference_term;
            string capitalized_reference_term;
            string first_reference_term;
            if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
            {
                reference_term = Ontology_classification_class.Get_name_of_ontology_pathway(selected_ontology);
                capitalized_reference_term = "SCP";
                first_reference_term = "subcellular processes (SCPs)";
            }
            else
            {
                reference_term = Ontology_classification_class.Get_name_of_ontology_pathway(selected_ontology);
                capitalized_reference_term = "Pathway";
                first_reference_term = "pathways";
            }
            string ontology_string = Ontology_classification_class.Get_name_of_ontology(selected_ontology);


            right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;

            int height_of_selectable_chButtons = (this.StandardConnectRelated_cbButton.Location.Y + this.StandardGroupSameLevelSCPs_cbButton.Height - this.StandardGroupSameLevelSCPs_cbButton.Location.Y);
            if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
            {
                height_of_selectable_chButtons = (this.StandardConnectRelated_cbButton.Location.Y + this.StandardConnectRelated_cbButton.Height - this.StandardParentChild_cbButton.Location.Y);
            }

            bool is_connect_standard_scps_selected = this.StandardConnectRelated_cbButton.Checked;

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
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y
                                         + (int)Math.Round(0.5 * (Overall_panel.Height));
                        text = "The application generates networks of " + first_reference_term + ". Each " + reference_term + " is visualized as a pie chart, with slices representing the datasets in which the " + reference_term + " was predicted with significance.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y
                                         + (int)Math.Round(0.5 * (Overall_panel.Height));
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            text = ontology_string + " networks show the results obtained by dynamic or standard enrichment analysis for all datasets assigned to the same integration group.";
                        }
                        else
                        {
                            text = ontology_string + " networks show the results obtained by standard enrichment analysis for all datasets assigned to the same integration group.";
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + Standard_panel.Location.Y
                                         + (int)Math.Round(0.5 * (Standard_panel.Height));
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            text = reference_term + "s predicted by standard (and dynamic) enrichment analysis can be connected across all datasets through parent child relationships.";
                        }
                        else
                        {
                            text = "Predicted " + reference_term + "s are connected across all datasets through parent child relationships.";
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 3:
                        this.StandardConnectRelated_cbButton.SilentChecked = is_connect_standard_scps_selected;
                        this.StandardConnectScpsTopInteractions_panel.Visible = is_connect_standard_scps_selected;
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Standard_panel.Location.Y
                                             + (int)Math.Round(0.5 * (Standard_panel.Height));
                            text = "For " + ontology_string + ", all ancestor SCPs of each predicted SCP are included, allowing determination of the level of an SCP by counting its ancestors.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 4:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            if (!is_connect_standard_scps_selected)
                            {
                                this.StandardConnectRelated_cbButton.SilentChecked = true;
                                this.StandardConnectScpsTopInteractions_panel.Visible = true;
                                this.StandardConnectScpsTopInteractions_panel.Refresh();
                            }
                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Standard_panel.Location.Y + this.StandardConnectScpsTopInteractions_panel.Location.Y
                                             + (int)Math.Round(0.5 * (StandardConnectScpsTopInteractions_panel.Height));
                            text = ontology_string + " SCPs can also be connected across all datasets based on the specified top MBCO inferred functional interactions.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 5:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            this.StandardConnectRelated_cbButton.SilentChecked = is_connect_standard_scps_selected;
                            this.StandardConnectScpsTopInteractions_panel.Visible = is_connect_standard_scps_selected;

                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Dynamic_panel.Location.Y
                                             + (int)Math.Round(0.5 * (Dynamic_panel.Height));
                            text = "Dynamic enrichment analysis predicts either single SCPs or SCP combinations of up to 3 functionally related MBCO SCPs.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 6:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Dynamic_panel.Location.Y + this.DynamicConnectAllRelated_cbButton.Location.Y
                                         + (int)Math.Round(0.5 * (DynamicConnectAllRelated_cbButton.Height));
                            text = "The application will either connect all functionally related SCPs predicted by dynamic enrichment analysis across all datasets or only those SCPs that are part of the same prediction.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 7:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Dynamic_panel.Location.Y + this.DynamicConnectAllRelated_cbButton.Location.Y
                                         + (int)Math.Round(0.5 * (DynamicConnectAllRelated_cbButton.Height));
                            text = "Here, functional relationships are defined based on the top inferred interactions that were also considered for dynamic enrichment analysis. They can be specified in the 'Enrichment'-menu.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 8:
                        if (!Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Except for MBCO, where SCPs span at most four levels, users can choose to connect predicted pathways exclusively via intermediate pathway nodes, rather than including all ancestor GO pathways that meet the selected size criteria.";
                            mid_y_position = this.Overall_panel.Location.Y + Standard_panel.Location.Y + this.ParentChildSCPNetGeneration_ownListBox.Location.Y
                                             + (int)Math.Round(0.5 * (ParentChildSCPNetGeneration_ownListBox.Height));
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 9:
                        if (Ontology_classification_class.Is_go_ontology(selected_ontology))
                        {
                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Standard_panel.Location.Y + this.HierarchicalScpInteractions_ownListBox.Location.Y
                                             + (int)Math.Round(0.5 * (HierarchicalScpInteractions_ownListBox.Height));
                            text = "For " + ontology_string + ", regulatory pathway relationships can also be considered, alongside the standard 'is_a' and 'part_of' parent-child relationships.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 10:
                        {
                            right_x_position = right_x_position_next_to_overall_panel;
                            mid_y_position = this.Overall_panel.Location.Y + Standard_panel.Location.Y + this.HierarchicalScpInteractions_ownListBox.Location.Y
                                             + (int)Math.Round(0.5 * (HierarchicalScpInteractions_ownListBox.Height));
                            text = "Genes that are part of the analyzed datasets and map to the predicted SCPs can be added as child nodes to the predicted SCPs.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 11:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + NodeSize_panel.Location.Y + NodeSize_determinant_ownListBox.Location.Y
                                         + (int)Math.Round(0.5 * (NodeSize_determinant_ownListBox.Height));
                        text = capitalized_reference_term + " node sizes can be set to be proportional to SCP significance, the number of datasets, the number of datasets with the same color, or made uniform.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 12:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + NodeSize_panel.Location.Y + NodeSize_determinant_ownListBox.Location.Y
                                         + (int)Math.Round(0.5 * (NodeSize_determinant_ownListBox.Height));
                        text = "If genes are added to SCP nodes and node sizes are scaled by SCP significance, the gene nodes may become very small. Using an alternative metric to determine node sizes will produce gene nodes with more regular and readable sizes.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 13:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + NodeSize_panel.Location.Y + NodeSize_diameterMax_ownTextBox.Location.Y
                                         + (int)Math.Round(0.5 * (NodeLabel_maxSize_ownTextBox.Location.Y + NodeLabel_maxSize_ownTextBox.Height - NodeSize_diameterMax_ownTextBox.Location.Y));
                        text = "The application allows for the options to specify node diameters, label sizes and to adjust node label sizes relative to node sizes.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 14:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + NodeSize_panel.Location.Y + NodeSize_scaling_ownListBox.Location.Y
                                         + (int)Math.Round(0.5 * (NodeSize_scaling_ownListBox.Height));
                        text = "Node size scaling can be applied independently to each network or uniformly across all networks predicted by standard or dynamic enrichment analysis.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 15:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = this.Overall_panel.Location.Y + GraphEditor_panel.Location.Y
                                         + (int)Math.Round(0.5 * (GraphEditor_panel.Height));
                        text = "Network files can either be generated for the yED Graph Editor or Cytoscape.";
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
            UserInterface_tutorial.Set_to_invisible();
        }
        #endregion


    }
}
