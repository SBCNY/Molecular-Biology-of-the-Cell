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
        private MyPanel Comments_panel { get; set; }
        private Label Comments_headline_label { get; set; }
        private Label Comments_yED_label { get; set; }
        private Label Comments_standardDynamicAddGenes_label { get; set; }
        private MyCheckBox_button StandardParentChild_cbButton { get; set; }
        private Label StandardParentChild_cbLabel { get; set; }
        private MyCheckBox_button StandardGroupSameLevelSCPs_cbButton { get; set; }
        private Label StandardGroupSameLevelSCPs_cbLabel { get; set; }
        private MyCheckBox_button StandardAddGenes_cbButton { get; set; }
        private Label StandardAddGenes_cbLabel { get; set; }
        private MyCheckBox_button StandardConnectRelated_cbButton { get; set; }
        private Label StandardConnectRelated_cbLabel { get; set; }
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
        private Label DynamicGroupSameLevelSCPs_cbLabel { get; set; }
        private MyCheckBox_button DynamicAddGenes_cbButton { get; set; }
        private Label DynamicAddGenes_cbLabel { get; set; }
        private MyCheckBox_button DynamicConnectAllRelated_cbButton { get; set; }
        private Label DynamicConnectAllRelated_cbLabel{ get; set; }
        private MyCheckBox_button Generate_scp_networks_cbButton { get; set; }
        private Label Generate_scp_networks_cbLabel{ get; set; }
        private MyPanel NodeSize_panel { get; set; }
        private Label DynamicConnectAllScps_explantion_label { get; set; }
        private Label NodeSize_label { get; set; }
        private MyCheckBox_button NodeSize_byDatasetsCount_cbButton { get; set; }
        private Label NodeSize_byDatasetsCount_cbLabel { get; set; }
        private MyCheckBox_button NodeSize_byColorsCount_cbButton { get; set; }
        private Label NodeSize_byColorsCount_cbLabel { get; set; }
        private MyCheckBox_button NodeSize_fixed_cbButton { get; set; }
        private Label NodeSize_fixed_cbLabel { get; set; }
        private Label Error_reports_headline_label { get; set; }
        private Label Error_reports_maxErrorPerFile1_label { get; set; }
        private Label Error_reports_maxErrorPerFile2_label { get; set; }
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }

        private Button Explanation_button { get; set; }

        private Form1_default_settings_class Form_default_settings { get; set; }

        public ScpNetworks_userInterface_class(MyPanel overall_panel,
                                               MyPanel standard_panel,
                                               Label standard_label,
                                               MyPanel dynamic_panel,
                                               Label dynamic_label,
                                               MyPanel files_panel,
                                               Label comments_headline_label,
                                               Label comments_yED_label,
                                               Label comments_standardDynamicAddGenes_label,
                                               Button default_button,
                                               MyCheckBox_button standardParentChild_cbButton,
                                               Label standardParentChild_cbLabel,
                                               MyCheckBox_button standardGroupSameLevelScps_cbButton,
                                               Label standardGroupSameLevelScps_cbLabel,
                                               MyCheckBox_button standardAddGenes_cbButton,
                                               Label standardAddGenes_cbLabel,
                                               MyCheckBox_button standardConnectRelated_cbButton,
                                               Label standardConnectRelated_cbLabel,
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
                                               Label dynamicGroupSameLevelScps_cbLabel,
                                               MyCheckBox_button dynamicAddGenes_cbButton,
                                               Label dynamicAddGenes_cbLabel,
                                               MyCheckBox_button dynamicConnectAllRelated_cbButton,
                                               Label dynamicConnectAllRelated_cbLabel,
                                               Label dynamicConnectAllScps_explantion_label,
                                               MyCheckBox_button generate_scp_networks_cbButton,
                                               Label generate_scp_networks_cbLabel,
                                               MyPanel nodeSize_panel,
                                               Label nodeSize_label,
                                               MyCheckBox_button nodeSize_byDatasetsCount_cbButton,
                                               Label nodeSize_byDatasetsCount_cbLabel,
                                               MyCheckBox_button nodeSize_byColorsCount_cbButton,
                                               Label nodeSize_byColorsCount_cbLabel,
                                               MyCheckBox_button nodeSize_fixed_cbButton,
                                               Label nodeSize_fixed_cbLabel,

                                               Label error_reports_headline_label,
                                               Label error_reports_maxErrorPerFile1_label,
                                               Label error_reports_maxErrorPerFile2_label,
                                               OwnTextBox error_reports_ownTextBox,
                                               OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,

                                               Button explanation_button,

                                               MBCO_network_based_integration_options_class options,
                                               Form1_default_settings_class form_default_settings)

        {
            this.Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.Standard_panel = standard_panel;
            this.Standard_label = standard_label;
            this.Dynamic_panel = dynamic_panel;
            this.Dynamic_label = dynamic_label;
            this.Comments_panel = files_panel;
            this.Comments_headline_label  = comments_headline_label;
            this.Comments_yED_label = comments_yED_label;
            this.Comments_standardDynamicAddGenes_label = comments_standardDynamicAddGenes_label;
            this.Default_button = default_button;
            this.StandardParentChild_cbButton = standardParentChild_cbButton;
            this.StandardParentChild_cbLabel = standardParentChild_cbLabel;
            this.StandardGroupSameLevelSCPs_cbButton = standardGroupSameLevelScps_cbButton;
            this.StandardGroupSameLevelSCPs_cbLabel = standardGroupSameLevelScps_cbLabel;
            this.StandardAddGenes_cbButton = standardAddGenes_cbButton;
            this.StandardAddGenes_cbLabel = standardAddGenes_cbLabel;
            this.StandardConnectRelated_cbButton = standardConnectRelated_cbButton;
            this.StandardConnectRelated_cbLabel = standardConnectRelated_cbLabel;
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
            this.DynamicConnectAllScps_explantion_label = dynamicConnectAllScps_explantion_label;
            this.Generate_scp_networks_cbButton = generate_scp_networks_cbButton;
            this.Generate_scp_networks_cbLabel = generate_scp_networks_cbLabel;
            this.NodeSize_panel = nodeSize_panel;
            this.NodeSize_label = nodeSize_label;
            this.NodeSize_byDatasetsCount_cbButton = nodeSize_byDatasetsCount_cbButton;
            this.NodeSize_byDatasetsCount_cbLabel = nodeSize_byDatasetsCount_cbLabel;
            this.NodeSize_byColorsCount_cbButton = nodeSize_byColorsCount_cbButton;
            this.NodeSize_byColorsCount_cbLabel = nodeSize_byColorsCount_cbLabel;
            this.NodeSize_fixed_cbButton = nodeSize_fixed_cbButton;
            this.NodeSize_fixed_cbLabel = nodeSize_fixed_cbLabel;

            Error_reports_headline_label = error_reports_headline_label;
            Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;
            Error_reports_ownTextBox = error_reports_ownTextBox;

            Explanation_button = explanation_button;

            Update_graphic_elements(options);
        }

        public void Update_graphic_elements(MBCO_network_based_integration_options_class options)
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            Label my_label;
            MyPanel my_panel;
            OwnTextBox my_textBox;
            MyCheckBox_button my_cbButton;
            Button my_button;

            this.Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            #region Standard scp networks panel
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = (int)Math.Round(0.313*(double)Overall_panel.Height);
            this.Standard_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(Standard_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
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

            left_referenceBorder = StandardParentChild_cbButton.Location.X + StandardParentChild_cbButton.Width;
            right_referenceBorder = this.Standard_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            my_label = StandardParentChild_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_standardSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = StandardGroupSameLevelSCPs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = StandardGroupSameLevelSCPs_cbButton.Location.X + StandardGroupSameLevelSCPs_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.65F*Standard_panel.Width);
            my_label = StandardGroupSameLevelSCPs_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = StandardAddGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = StandardAddGenes_cbButton.Location.X + StandardAddGenes_cbButton.Width;
            right_referenceBorder = Standard_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            my_label = StandardAddGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

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

            #region Standard headline label
            this.Standard_label = Standard_label;
            left_referenceBorder = 0;
            right_referenceBorder = Standard_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.StandardParentChild_cbButton.Location.Y;
            my_label = this.Standard_label;
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

            top_referenceBorder = (int)Math.Round(0.55F * this.StandardConnectScpsTopInteractions_panel.Height);
            bottom_referenceBorder = top_referenceBorder + height_of_textBoxes_standard_connectSCPs;

            left_referenceBorder = (int)Math.Round(0.75F * this.StandardConnectScpsTopInteractions_panel.Width);
            right_referenceBorder = left_referenceBorder + width_of_textBoxes_standard_connectSCPs;
            my_textBox = this.StandardConnectScpsTopInteractions_level_2_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = left_referenceBorder + width_of_textBoxes_standard_connectSCPs;
            my_textBox = this.StandardConnectScpsTopInteractions_level_3_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

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
            top_referenceBorder = this.Standard_panel.Location.Y + Standard_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.56 * (double)Overall_panel.Height);
            my_panel = Dynamic_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_dynamicSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            top_referenceBorder = top_referenceBorder_checkBoxes_dynamicSCP;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicParentChild_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DynamicParentChild_cbButton.Location.X + DynamicParentChild_cbButton.Width;
            right_referenceBorder = this.Dynamic_panel.Width - shared_min_distance_X_from_panelSides_dynamicSCP;
            my_label = DynamicParentChild_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_min_distance_X_from_panelSides_dynamicSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicGroupSameLevelSCPs_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DynamicGroupSameLevelSCPs_cbButton.Location.X + DynamicGroupSameLevelSCPs_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.65F*this.Dynamic_panel.Width);
            my_label = DynamicGroupSameLevelSCPs_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_dynamicSCP;
            my_cbButton = DynamicAddGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DynamicAddGenes_cbButton.Location.X + DynamicAddGenes_cbButton.Width;
            right_referenceBorder = this.Dynamic_panel.Width - shared_min_distance_X_from_panelSides_dynamicSCP;
            my_label = DynamicAddGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

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

            #region Node size panel
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = this.Dynamic_panel.Location.Y + Dynamic_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.65F * (double)Overall_panel.Height);
            my_panel = this.NodeSize_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder); ;

            int shared_cbButton_widthHeight_nodeSizePanel = shared_cbButton_widthHeight_standardSCP;
            int shared_min_distance_X_from_panelSides_nodeSizes = shared_min_distance_X_from_panelSides_standardSCP;

            left_referenceBorder = shared_min_distance_X_from_panelSides_dynamicSCP;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_nodeSizePanel;
            top_referenceBorder = (int)Math.Round(0.5F * (double)NodeSize_panel.Height);
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_nodeSizePanel;
            my_cbButton = NodeSize_byColorsCount_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.NodeSize_byColorsCount_cbButton.Location.X + NodeSize_byColorsCount_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.333F * (double)NodeSize_panel.Width);
            my_label = NodeSize_byColorsCount_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_nodeSizePanel;
            my_cbButton = NodeSize_byDatasetsCount_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.NodeSize_byDatasetsCount_cbButton.Location.X + NodeSize_byDatasetsCount_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.666F * (double)NodeSize_panel.Width);
            my_label = NodeSize_byDatasetsCount_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = right_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_nodeSizePanel;
            my_cbButton = NodeSize_fixed_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.NodeSize_fixed_cbButton.Location.X + NodeSize_fixed_cbButton.Width;
            right_referenceBorder = NodeSize_panel.Width - shared_min_distance_X_from_panelSides_nodeSizes;
            my_label = NodeSize_fixed_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.NodeSize_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.NodeSize_byColorsCount_cbButton.Location.Y;
            this.NodeSize_label = Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(NodeSize_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Comments panels
            left_referenceBorder = 0;
            right_referenceBorder = Overall_panel.Width;
            top_referenceBorder = this.NodeSize_panel.Location.Y + NodeSize_panel.Height;
            bottom_referenceBorder = (int)Math.Round(0.9F*this.Overall_panel.Height);
            this.Comments_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(Comments_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            left_referenceBorder = 0;
            right_referenceBorder = Comments_panel.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = (int)Math.Round(0.25 * (double)Comments_panel.Height);
            my_label = this.Comments_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            left_referenceBorder = 0;
            right_referenceBorder = Comments_panel.Width;
            top_referenceBorder = Comments_headline_label.Location.Y + Comments_headline_label.Height;// (int)Math.Round(0.3 * (double)Comments_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.75 * (double)Comments_panel.Height);
            my_label = this.Comments_yED_label;
            this.Comments_yED_label = Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            left_referenceBorder = 0;
            right_referenceBorder = Comments_panel.Width;
            top_referenceBorder = Comments_yED_label.Location.Y + Comments_yED_label.Height;//(int)Math.Round(0.8 * (double)Comments_panel.Height);
            bottom_referenceBorder = Comments_panel.Height;
            my_label = this.Comments_standardDynamicAddGenes_label;
            Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            int defaultButton_explanationButton_height = (int)Math.Round(0.05F * this.Overall_panel.Height);

            #region Generate nw check box and default button
            right_referenceBorder = this.Overall_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            left_referenceBorder = right_referenceBorder - (int)Math.Round(0.3F * this.Overall_panel.Width);
            top_referenceBorder = (int)Math.Round(0.9F * this.Overall_panel.Height);
            bottom_referenceBorder = top_referenceBorder + defaultButton_explanationButton_height;
            my_button = Default_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            this.Generate_scp_networks_cbButton = Generate_scp_networks_cbButton;
            left_referenceBorder = Comments_panel.Location.X;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            top_referenceBorder = (int)Math.Round(0.90F * this.Overall_panel.Height);
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight_standardSCP;
            my_cbButton = Generate_scp_networks_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Generate_scp_networks_cbButton.Location.X + Generate_scp_networks_cbButton.Width;
            right_referenceBorder = Default_button.Location.X;
            top_referenceBorder = Generate_scp_networks_cbButton.Location.Y;
            bottom_referenceBorder = Generate_scp_networks_cbButton.Location.Y + Generate_scp_networks_cbButton.Height;
            my_label = Generate_scp_networks_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            right_referenceBorder = this.Overall_panel.Width - shared_min_distance_X_from_panelSides_standardSCP;
            left_referenceBorder = right_referenceBorder - (int)Math.Round(0.3F * this.Overall_panel.Width);
            top_referenceBorder = (int)Math.Round(0.95F * this.Overall_panel.Height);
            bottom_referenceBorder = top_referenceBorder + defaultButton_explanationButton_height;
            my_button = Explanation_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Set_default_colors();
            Copy_options_into_interface_selections(options);
            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
        }

        private void Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label()
        {
            int left_referenceBorder = 0;
            int right_referenceBorder = Dynamic_panel.Width;
            int top_referenceBorder = this.DynamicConnectAllRelated_cbButton.Location.Y + this.DynamicConnectAllRelated_cbButton.Height;
            int bottom_referenceBorder = this.Dynamic_panel.Height;
            Label my_label = this.DynamicConnectAllScps_explantion_label;
            Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
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
                Ontology_type_enum reference_ontology = Ontology_classification_class.Get_related_human_ontology(options.Next_ontology);
                bool visualize_mbco_selective_parameter = reference_ontology.Equals(Ontology_type_enum.Mbco_human);


                Dynamic_panel.Visible = visualize_mbco_selective_parameter;
                Standard_panel.Visible = true;
                StandardConnectRelated_cbButton.Visible = visualize_mbco_selective_parameter;
                StandardConnectRelated_cbLabel.Visible = visualize_mbco_selective_parameter;
                StandardGroupSameLevelSCPs_cbButton.Visible = visualize_mbco_selective_parameter;
                StandardGroupSameLevelSCPs_cbLabel.Visible = visualize_mbco_selective_parameter;
                StandardParentChild_cbButton.Visible = visualize_mbco_selective_parameter;
                StandardParentChild_cbLabel.Visible = visualize_mbco_selective_parameter;

                Default_button.Visible = true;
                Comments_panel.Visible = true;
                NodeSize_panel.Visible = true;
                if ((this.StandardConnectRelated_cbButton.Checked)&&(visualize_mbco_selective_parameter))
                {
                    is_visible = true;
                }
                else { is_visible = false; }
                this.StandardConnectScpsTopInteractions_panel.Visible = is_visible;
                if ((this.StandardAddGenes_cbButton.Checked)||(this.DynamicAddGenes_cbButton.Checked))
                {
                    this.Comments_standardDynamicAddGenes_label.Visible = true;
                }
                else 
                {
                    this.Comments_standardDynamicAddGenes_label.Visible = false;
                }
                if (this.DynamicConnectAllRelated_cbButton.Checked)
                {
                    DynamicConnectAllScps_explantion_label.Location = new Point(3, DynamicConnectAllScps_explantion_label.Location.Y);
                    DynamicConnectAllScps_explantion_label.Text = "(All related SCPs in the network will be connected\nbased on parameters specified in menu 'Enrichment'.)";
                    Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label();
                }
                else
                {
                    DynamicConnectAllScps_explantion_label.Location = new Point(30, DynamicConnectAllScps_explantion_label.Location.Y);
                    DynamicConnectAllScps_explantion_label.Text = "(Only combined SCPs forming a significant\nhigher-level SCP will be connected.)";
                    Set_fontSize_and_size_of_dynamicConnetAllScps_explanation_label();
                }
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

        public void Set_visibility(bool is_visible, MBCO_network_based_integration_options_class options)
        {
            Copy_options_into_interface_selections(options);
            Set_default_colors();
            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
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
            switch (options.Node_size_determinant)
            {
                case yed_network.Yed_network_node_size_determinant_enum.Standard:
                    NodeSize_fixed_cbButton.SilentChecked = true;
                    NodeSize_byDatasetsCount_cbButton.SilentChecked = false;
                    NodeSize_byColorsCount_cbButton.SilentChecked = false;
                    break;
                case yed_network.Yed_network_node_size_determinant_enum.No_of_different_colors:
                    NodeSize_fixed_cbButton.SilentChecked = false;
                    NodeSize_byDatasetsCount_cbButton.SilentChecked = false;
                    NodeSize_byColorsCount_cbButton.SilentChecked = true;
                    break;
                case yed_network.Yed_network_node_size_determinant_enum.No_of_sets:
                    NodeSize_fixed_cbButton.SilentChecked = false;
                    NodeSize_byDatasetsCount_cbButton.SilentChecked = true;
                    NodeSize_byColorsCount_cbButton.SilentChecked = false;
                    break;
                default:
                    throw new System.Exception();
            }

            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
        }
        public MBCO_network_based_integration_options_class Copy_interfaceSelections_into_options(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            options.Add_parent_child_relationships_to_standard_SCP_networks = StandardParentChild_cbButton.Checked;
            options.Add_parent_child_relationships_to_dynamic_SCP_networks = DynamicParentChild_cbButton.Checked;
            options.Add_genes_to_standard_networks = StandardAddGenes_cbButton.Checked;
            options.Add_genes_to_dynamic_networks = DynamicAddGenes_cbButton.Checked;
            options.Add_edges_that_connect_standard_scps = StandardConnectRelated_cbButton.Checked;
            options.Add_additional_edges_that_connect_dynamic_scps = DynamicConnectAllRelated_cbButton.Checked;
            options.Box_sameLevel_scps_for_standard_enrichment = StandardGroupSameLevelSCPs_cbButton.Checked;
            options.Box_sameLevel_scps_for_dynamic_enrichment = DynamicGroupSameLevelSCPs_cbButton.Checked;
            int percent;
            float[] top_quantiles;

            #region Update top quantiles and text box for level 2
            top_quantiles = Array_class.Deep_copy_array(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
            if (int.TryParse(StandardConnectScpsTopInteractions_level_2_ownTextBox.Text, out percent))
            { top_quantiles[2] = (float)percent / 100F; }
            else { top_quantiles[2] = -1; } // ensures mismatch with max min range
            options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level= top_quantiles;
            if (Array_class.Arrays_are_equal(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level, top_quantiles))
            { StandardConnectScpsTopInteractions_level_2_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { StandardConnectScpsTopInteractions_level_2_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            #endregion

            #region Update top quantiles and text box for level 3
            top_quantiles = Array_class.Deep_copy_array(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
            if (int.TryParse(StandardConnectScpsTopInteractions_level_3_ownTextBox.Text, out percent))
            { top_quantiles[3] = (float)percent / 100F; }
            else { top_quantiles[3] = -1; } // ensures mismatch with max min range
            options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = top_quantiles;
            if (Array_class.Arrays_are_equal(options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level, top_quantiles))
            { StandardConnectScpsTopInteractions_level_3_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { StandardConnectScpsTopInteractions_level_3_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            #endregion

            options.Generate_scp_networks = Generate_scp_networks_cbButton.Checked;
            if (NodeSize_fixed_cbButton.Checked)
            {
                options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.Standard;
            }
            else if (NodeSize_byDatasetsCount_cbButton.Checked)
            {
                options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.No_of_sets;
            }
            else if (NodeSize_byColorsCount_cbButton.Checked)
            {
                options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.No_of_different_colors;
            }
            else { throw new System.Exception(); }
            Update_visibility_of_topInteractions_labels_and_textBoxes(options);
            return options;
        }

        public MBCO_network_based_integration_options_class NodeSize_byColorsCount_changed(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            NodeSize_byDatasetsCount_cbButton.SilentChecked = !NodeSize_byColorsCount_cbButton.Checked;
            NodeSize_fixed_cbButton.SilentChecked = !NodeSize_byColorsCount_cbButton.Checked;
            return Copy_interfaceSelections_into_options(options, enrichment_options);
        }
        public MBCO_network_based_integration_options_class NodeSize_byDatasetsCount_changed(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            NodeSize_byColorsCount_cbButton.SilentChecked = !NodeSize_byDatasetsCount_cbButton.Checked;
            NodeSize_fixed_cbButton.SilentChecked = !NodeSize_byDatasetsCount_cbButton.Checked;
            return Copy_interfaceSelections_into_options(options, enrichment_options);
        }
        public MBCO_network_based_integration_options_class NodeSize_fixed_changed(MBCO_network_based_integration_options_class options, MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            NodeSize_byDatasetsCount_cbButton.SilentChecked = !NodeSize_fixed_cbButton.Checked;
            NodeSize_byColorsCount_cbButton.SilentChecked = !NodeSize_fixed_cbButton.Checked;
            return Copy_interfaceSelections_into_options(options, enrichment_options);
        }

        #region Explanation
        private void Write_explanation_into_error_reports_panel()
        {
            Error_reports_headline_label.Text = "SCP networks";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Standard enrichment analysis (using MBCO)");
            sb.AppendFormat("\r\nThe check boxes specify the following parameters:");
            sb.AppendFormat("\r\nConnect parent and child SCPs: SCPs in annotated parent-child relationships will be connected using solid");
            sb.AppendFormat("\r\n                               arrows pointing from parents to their children. If selected, all ancestors");
            sb.AppendFormat("\r\n                               of a predicted subcellular process (SCP) will always be shown. If they are");
            sb.AppendFormat("\r\n                               not among the significant predictions, they will be added as white intermediate");
            sb.AppendFormat("\r\n                               SCPs. In the case of MBCO, the oldest parent is always a level-1 SCP. With");
            sb.AppendFormat("\r\n                               each solid arrow the level number increases by 1, so that the level of an");
            sb.AppendFormat("\r\n                               SCP can be identified from its hierarchical position.");
            sb.AppendFormat("\r\nBox SCPs of the same level: SCPs of the same level will be boxed. If 'Add genes' is selected, all added");
            sb.AppendFormat("\r\n                            genes will be boxed in an independent box.");
            sb.AppendFormat("\r\nAdd genes: If selected, all genes that are part of a user-supplied dataset and a predicted SCP will be");
            sb.AppendFormat("\r\n           added as children to that SCP. If predicted SCPs are in parent child relationships, the genes");
            sb.AppendFormat("\r\n           will only be added to the youngest child (independently of added or missing parent-child connections).");
            sb.AppendFormat("\r\nConnect related SCPs: The annotated MBCO hierarchy is enriched using a unique MBCO algorithm that infers");
            sb.AppendFormat("\r\n                      interactions between functionally related SCPs of the levels-2 or -3. If this check");
            sb.AppendFormat("\r\n                      box is selected any predicted SCPs will be connected, if their interaction is among");
            sb.AppendFormat("\r\n                      the top % of SCP interactions, specified in the next parameter field.");
            sb.AppendFormat("\r\nConnect related SCPs in NW using top % SCP interactions: The inferred interactions between functionally");
            sb.AppendFormat("\r\n                      related SCPs are weighted and ranked by their interaction strength. The user can");
            sb.AppendFormat("\r\n                      specify how many (in percent) of the infered top ranked interactions will be considered");
            sb.AppendFormat("\r\n                      to connect predicted SCPs.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nDynamic enrichment analysis (using MBCO)");
            sb.AppendFormat("\r\nThe check boxes specify the following parameters:");
            sb.AppendFormat("\r\nConnect parent and child SCPs: Same functionality as described under Standard enrichment analysis.");
            sb.AppendFormat("\r\nBox SCPs of the same level: Same functionality as described under Standard enrichment analysis");
            sb.AppendFormat("\r\nAdd genes: Same functionality as described under Standard enrichment analysis.");
            sb.AppendFormat("\r\nConnect all related SCPs: Unchecked, only those SCPs will be connected that were combined to form");
            sb.AppendFormat("\r\n                   a context-specific higher level-SCP that meets the significance criteria, defined");
            sb.AppendFormat("\r\n                   in the menu panel ‘Enrichment’. These combined SCPs label the same bar in the");
            sb.AppendFormat("\r\n                   bardiagram charts for dynamic enrichment analysis. The results text file that");
            sb.AppendFormat("\r\n                   documents the significant predictions for dynamic enrichment analysis, lists these");
            sb.AppendFormat("\r\n                   SCPs as one entry in the column ‘SCP’, separated by dollar signs. Since an SCP can");
            sb.AppendFormat("\r\n                   be part of multiple higher-level SCPs, it can be connected to more than two other");
            sb.AppendFormat("\r\n                   SCPs. ");
            sb.AppendFormat("\r\n                   If the check box is checked, any SCPs will be connected, as long as their interaction");
            sb.AppendFormat("\r\n                   is among the top percentages of considered SCP interactions, no matter if they are");
            sb.AppendFormat("\r\n                   part of the same significant context-specific higher level-SCP or not. If checked,");
            sb.AppendFormat("\r\n                   SCPs will also be connected across different datasets. The top percentages of");
            sb.AppendFormat("\r\n                   considered SCP interactions are the same as those used for the dynamic enrichment");
            sb.AppendFormat("\r\n                   analysis algorithm and can be modified in the menu panel ‘Enrichment’.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nSpecialized MBCO datasets and Gene Ontology");
            sb.AppendFormat("\r\nIn case of specialized MBCO datasets or Gene Ontology the user can select to add genes to the networks");
            sb.AppendFormat("\r\ngenerated by standard enrichment analysis.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nSCP node sizes");
            sb.AppendFormat("\r\nThe user can specify, if the size of the SCP nodes should be proportional to the number of datasets or");
            sb.AppendFormat("\r\nnumber of datasets with different user-defined colors that predicted an SCP. Additionally, the user can");
            sb.AppendFormat("\r\nspecify that all SCP nodes should have the same size.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nGenerate SCP networks: The user can switch off the network generation.");

            Error_reports_ownTextBox.SilentText_and_refresh = sb.ToString();
        }

        public void Explanation_button_activated()
        {
            Write_explanation_into_error_reports_panel();
        }
        #endregion


    }
}
