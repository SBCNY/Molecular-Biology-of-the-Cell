//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_functions.Global_definitions;
using Data;
using Enrichment;
using Result_visualization;
using Common_functions.Text;
using Windows_forms_customized_tools;
using ClassLibrary1.Dataset_userInterface;
using ClassLibrary1.Read_interface;
using ClassLibrary1.ScpNetworks_userInterface;
using ClassLibrary1.OrganizeData_userInterface;
using ClassLibrary1.SigData_userInterface;
using ClassLibrary1.BgGenes_userInterface;
using ClassLibrary1.EnrichmentOptions_userInterface;
using ClassLibrary1.LoadExamples_userInterface;
using ClassLibrary1.Select_scps_userInterface;
using ClassLibrary1.DefineSCPs_userInterface;
using ClassLibrary1.Tips_userInterface;
using ClassLibrary1.Results_userInterface;
using Common_functions.Form_tools;
using ZedGraph;

namespace ClassLibrary1
{

    enum Dataset_compatibility_enum {  E_m_p_t_y, Ok, Duplicated_dataset }

    public partial class Mbco_user_application_form : Form
    {
        private Mbc_enrichment_fast_pipeline_class Mbco_enrichment_pipeline { get; set; }
        private Mbc_network_based_integration_class Mbco_network_integration { get; set; }
        private Bardiagram_class Bardiagram { get; set; }
        private Timeline_class Timeline_diagram { get; set; }
        private Heatmap_class Heatmap { get; set; }
        private Custom_data_class Custom_data { get; set; }
        private DatasetSummary_userInterface_class DatasetSummary_userInterface { get; set; }
        private Results_userInterface_class UserInterface_results { get; set; }
        private Read_interface_class UserInterface_read { get; set; }
        private ScpNetworks_userInterface_class UserInterface_scp_networks { get; set; }
        private OrganizeData_userInterface_class UserInterface_organize_data { get; set; }
        private SigData_userInterface_class UserInterface_sigData { get; set; }
        private EnrichmentOptions_userInterface_class UserInterface_enrichmentOptions { get; set; }
        private BgGenes_userInterface_class UserInterface_bgGenes { get; set; }
        private DefineSCPs_userInterface_class UserInterface_defineSCPs { get; set; }
        private Select_scps_userInterface_class UserInterface_selectSCPs { get; set; }
        private LoadExamples_userInterface_class UserInterface_loadExamples { get; set; }
        private Tips_userInterface_class UserInterface_tips { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }

        public Mbco_user_application_form()
        {
            InitializeComponent();
            Initialize_form_settings_object();
            Update_overallApplicationSize();

            Report_panel.Location = new Point(DatasetInterface_overall_panel.Location.X, DatasetInterface_overall_panel.Location.Y);
            //Report_ownTextBox.Enabled= false;

            Set_main_panel_visibilities_to_default();

            Custom_data = new Custom_data_class();
            Mbco_enrichment_pipeline = new Mbc_enrichment_fast_pipeline_class(Ontology_type_enum.Mbco_human);
            Mbco_network_integration = new Mbc_network_based_integration_class(Mbco_enrichment_pipeline.Options.Next_ontology);
            Bardiagram = new Bardiagram_class(Progress_report_label, Abort_button, Form_default_settings);
            Timeline_diagram = new Timeline_class(Progress_report_label, Abort_button, Form_default_settings);
            Heatmap = new Heatmap_class(Progress_report_label, Abort_button, Form_default_settings);

            Update_overallApplicationSize();

            Initialize_and_reset_userInterface_enrichmentOptions();
            Initialize_and_reset_userInterface_dataSig();
            Initialize_and_reset_read_userInterface();
            Generate_results_directory_replace_and_refresh();

            Reset_gene_list_text_box();
            Initialize_and_reset_userInterface_scpNetworks();
            Initialize_and_reset_userInterface_organizeData();
            Initialize_and_reset_userInterface_bgGenes();
            Initialize_and_reset_userInterface_loadExamples();
            Initialize_and_reset_userInterface_selectSCPs();
            Initialize_and_reset_userInterface_defineSCPs();
            Initialize_and_reset_userInterface_tips();
            Initialize_and_reset_userInterface_results();

            Initialize_and_reset_datasetSummary_userInterface();

            Update_all_option_menu_buttons();
            Update_all_graphic_elements_of_shared_tools();

            Set_button_colors_to_unpressed();

            Set_options_buttons_and_panel_visibilities_to_default();
            Set_tab_order();
        }

        private void Set_tab_order()
        {
            int current_index = 0;

            //Dataset_0_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_1_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_2_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_3_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_4_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_5_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_6_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_7_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_8_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_9_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_10_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_11_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_12_sampleName_ownTextBox.TabIndex = current_index++;
            //Dataset_0_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_1_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_2_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_3_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_4_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_5_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_6_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_7_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_8_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_9_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_10_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_11_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_12_substring_ownTextBox.TabIndex = current_index++;
            //Dataset_0_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_1_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_2_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_3_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_4_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_5_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_6_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_7_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_8_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_9_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_10_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_11_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_12_timepoint_ownTextBox.TabIndex = current_index++;
            //Dataset_0_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_1_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_2_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_3_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_4_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_5_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_6_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_7_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_8_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_9_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_10_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_11_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_12_timeunit_ownCheckBox.TabIndex = current_index++;
            //Dataset_0_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_1_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_2_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_3_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_4_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_5_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_6_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_7_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_8_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_9_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_10_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_11_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_12_entryType_ownCheckBox.TabIndex = current_index++;
            //Dataset_0_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_1_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_2_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_3_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_4_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_5_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_6_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_7_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_8_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_9_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_10_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_11_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_12_integrationGroup_ownTextBox.TabIndex = current_index++;
            //Dataset_0_color_ownListBox.TabIndex = current_index++;
            //Dataset_1_color_ownListBox.TabIndex = current_index++;
            //Dataset_2_color_ownListBox.TabIndex = current_index++;
            //Dataset_3_color_ownListBox.TabIndex = current_index++;
            //Dataset_4_color_ownListBox.TabIndex = current_index++;
            //Dataset_5_color_ownListBox.TabIndex = current_index++;
            //Dataset_6_color_ownListBox.TabIndex = current_index++;
            //Dataset_7_color_ownListBox.TabIndex = current_index++;
            //Dataset_8_color_ownListBox.TabIndex = current_index++;
            //Dataset_9_color_ownListBox.TabIndex = current_index++;
            //Dataset_10_color_ownListBox.TabIndex = current_index++;
            //Dataset_11_color_ownListBox.TabIndex = current_index++;
            //Dataset_12_color_ownListBox.TabIndex = current_index++;
            //Dataset_0_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_1_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_2_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_3_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_4_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_5_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_6_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_7_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_8_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_9_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_10_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_11_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_12_sourceFile_ownTextBox.TabIndex = current_index++;
            //Dataset_0_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_1_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_2_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_3_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_4_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_5_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_6_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_7_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_8_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_9_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_10_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_11_bgGenes_ownListBox.TabIndex = current_index++;
            //Dataset_12_bgGenes_ownListBox.TabIndex = current_index++;
            Read_sampleNameColumn_ownTextBox.TabIndex = current_index++;
            Read_timepointColumn_ownTextBox.TabIndex = current_index++;
            Read_timeunitColumn_ownTextBox.TabIndex = current_index++;
            Read_integrationGroupColumn_ownTextBox.TabIndex = current_index++;
            Read_colorColumn_ownTextBox.TabIndex = current_index++;
            Read_geneSymbol_ownTextBox.TabIndex = current_index++;
            Read_value1stColumn_ownTextBox.TabIndex = current_index++;
            Read_value2ndColumn_ownTextBox.TabIndex = current_index++;
            SigData_value1st_cutoff_ownTextBox.TabIndex = current_index++;
            SigData_value2nd_cutoff_ownTextBox.TabIndex = current_index++;
            SigData_keepTopRanks_ownTextBox.TabIndex = current_index++;
            EnrichmentOptions_ontology_ownListBox.TabIndex = current_index++;
            EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_standardPvalue_textBox.TabIndex = current_index++;
            EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_dynamicPvalue_textBox.TabIndex = current_index++;
            EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox.TabIndex = current_index++;
            EnrichmentOptions_default_button.TabIndex = current_index++;
            EnrichmentOptions_generateBardiagrams_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmaps_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmapShowRanks_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateTimeline_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateTimelinePvalue_textBox.TabIndex= current_index++;

            ScpNetworks_standardParentChild_cbButton.TabIndex = current_index++;
            ScpNetworks_standardGroupSameLevelSCPs_cbButton.TabIndex = current_index++;
            ScpNetworks_standardAddGenes_cbButton.TabIndex = current_index++;
            ScpNetworks_standardConnectRelated_cbButton.TabIndex = current_index++;
            ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox.TabIndex = current_index++;
            ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox.TabIndex = current_index++;

            ScpNetworks_dynamicParentChild_cbButton.TabIndex = current_index++;
            ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.TabIndex = current_index++;
            ScpNetworks_dynamicAddGenes_cbButton.TabIndex = current_index++;
            ScpNetworks_dynamicConnectAllRelated_cbButton.TabIndex= current_index++;

            ScpNetworks_nodeSizeByColorCount_cbButton.TabIndex = current_index++;
            ScpNetworks_nodeSizeByDatasetCount_cbButton.TabIndex = current_index++;
            ScpNetworks_nodeSizeFixed_cbButton.TabIndex = current_index++;
            ScpNetworks_default_button.TabIndex = current_index++;

            AddNewDataset_button.TabIndex = current_index++;
            AnalyzeData_button.TabIndex= current_index++;
            ClearCustomData_button.TabIndex = current_index++;
            Options_readData_button.TabIndex = current_index++;
            Options_organizeData_button.TabIndex = current_index++;
            Options_dataSignificance_button.TabIndex = current_index++;
            Options_backgroundGenes_button.TabIndex = current_index++;
            Options_enrichment_button.TabIndex = current_index++;
            Options_selectSCPs_button.TabIndex = current_index++;
            Options_scpNetworks_button.TabIndex = current_index++;
            Options_exampleData_button.TabIndex = current_index++;
            Options_tips_button.TabIndex = current_index++;
        }

        private void Update_acknoledgment_and_application_headline()
        {
            Ontology_type_enum selected_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
            switch (selected_ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    MBCO_reference_label.Text = "Please acknowledge MBCO in your publications by citing our manuscript: Hansen J, Meretzky D, Woldesenbet S, Stolovitzky G, Iyengar R. A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes. Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142";
                    MBCO_headline.Text = "Molecular Biology of the Cell Ontology";
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    MBCO_reference_label.Text = "Please acknowledge our work in your publications by cinting our manuscript: Hansen J, Sealfon R, Menon R et al for KPMP. A reference tissue atlas for the human kidney. Sci Adv. 2022 Jun 10;8(23). PMID: 35675394. Please acknowledge MBCO in your publications: Hansen et al. Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142";
                    MBCO_headline.Text = "Specialized MBCO datasets";
                    break;
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Go_mf_human:
                    MBCO_reference_label.Text = "Please acknowledge Gene Ontology in your publications as described on geneontology.org. Please acknowledge MBCO in your publications: Hansen et al. A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes. Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142";
                    MBCO_headline.Text = "MBCO PathNet: Gene Ontology";
                    break;
                default:
                    throw new Exception();
            }

            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            System.Windows.Forms.Label my_label;

            #region MBCO headline
            left_referenceBorder = 0;
            right_referenceBorder = base.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Options_readData_panel.Location.Y;
            this.MBCO_headline = Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(this.MBCO_headline, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            left_referenceBorder = this.DatasetInterface_overall_panel.Location.X;
            right_referenceBorder = this.Options_readData_button.Location.X - (int)Math.Round(0.001 * this.Width);
            top_referenceBorder = this.Read_directoryOrFile_ownTextBox.Location.Y + this.Read_directoryOrFile_ownTextBox.Height;
            bottom_referenceBorder = Math.Min((int)Math.Round(Form_default_settings.Correction_factor_for_application_height * this.Height), top_referenceBorder + (int)Math.Round(3.3 * this.Read_directoryOrFile_ownTextBox.Height));
            my_label = this.MBCO_reference_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            MBCO_reference_label.Refresh();
            MBCO_headline.Refresh();
        }


        #region Initialize form settings
        private void Update_all_option_menu_buttons()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            int shared_height_of_all_buttons;
            int shared_width_of_all_buttons;

            Button[] options_buttons = new Button[] { this.Options_readData_button,
                                                      this.Options_dataSignificance_button,
                                                      this.Options_enrichment_button,
                                                      this.Options_selectSCPs_button,
                                                      this.Options_results_button,
                                                      this.Options_tips_button,

                                                      this.Options_backgroundGenes_button,
                                                      this.Options_organizeData_button,
                                                      this.Options_scpNetworks_button,
                                                      this.Options_defineSCPs_button,
                                                      this.Options_exampleData_button
                                                    };
            Button button;
            int buttons_length = options_buttons.Length;
            int location_y_of_all_buttons = this.Options_readData_panel.Location.Y + this.Options_readData_panel.Height + (int)Math.Round(0.02F * this.Height);
            shared_width_of_all_buttons = (int)Math.Round(0.5F*Options_readData_panel.Width);
            right_referenceBorder = this.Options_readData_panel.Location.X + this.Options_readData_panel.Width - 2 * shared_width_of_all_buttons;
            left_referenceBorder = right_referenceBorder - shared_width_of_all_buttons;
            bottom_referenceBorder = location_y_of_all_buttons;
            shared_height_of_all_buttons = (int)Math.Floor(Form_default_settings.Correction_factor_for_application_height*(float)(this.Height - bottom_referenceBorder)/((float)options_buttons.Length*0.8F));


            for (int indexB=0; indexB<buttons_length;indexB++)
            {
                if ((indexB==0)||(indexB==(int)Math.Round(0.5F*buttons_length)))
                {
                    bottom_referenceBorder = location_y_of_all_buttons;
                    right_referenceBorder += shared_width_of_all_buttons;
                    left_referenceBorder += shared_width_of_all_buttons;
                }

                button = options_buttons[indexB];
                top_referenceBorder = bottom_referenceBorder;
                bottom_referenceBorder += shared_height_of_all_buttons;
                Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
        }
        private void Initialize_form_settings_object()
        {
            Form_default_settings = new Form1_default_settings_class(this.AppSize_panel,
                                                                     this.AppSize_headline_label,
                                                                     this.AppSize_width_label,
                                                                     this.AppSize_width_textBox,
                                                                     this.AppSize_width_percent_label,
                                                                     this.AppSize_height_label,
                                                                     this.AppSize_height_textBox,
                                                                     this.AppSize_heightPercent_label,
                                                                     this.AppSize_increase_button,
                                                                     this.AppSize_decrease_button,
                                                                     this.AppSize_colorTheme_label,
                                                                     this.AppSize_colorTheme_listBox,
                                                                     this.AppSize_resize_button
                                                                     );
            Form_default_settings.Update_all_graphic_elements_in_applicationSize_panel();
        }
        #endregion

        #region Initialize interfaces

        private void Set_button_colors_to_unpressed()
        {
            AddNewDataset_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            AddNewDataset_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            ClearCustomData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            ClearCustomData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Changes_update_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Changes_update_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Changes_reset_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Changes_reset_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            AnalyzeData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            AnalyzeData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            LoadExamples_load_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            LoadExamples_load_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Abort_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Abort_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Abort_button.Visible = false;
        }
        private void Set_main_panel_visibilities_to_default()
        {
            DatasetInterface_overall_panel.Visible = true;
            Results_visualization_panel.Visible = false;
            Report_panel.Visible = false;
        }
        private void Initialize_and_reset_userInterface_enrichmentOptions()
        {
            this.UserInterface_enrichmentOptions = new EnrichmentOptions_userInterface_class(Options_enrichment_panel,
                                                                                             EnrichmentOptions_ontology_panel,
                                                                                             EnrichmentOptions_ontology_label,
                                                                                             EnrichmentOptions_ontology_ownListBox,
                                                                                             EnrichmentOptions_cutoffs_panel,
                                                                                             EnrichmentOptions_scpTopInteractions_panel,
                                                                                             EnrichmentOptions_keepTopSCPs_panel,
                                                                                             EnrichmentOptions_keepScpsScpLevel_label,
                                                                                             EnrichmentOptions_maxRanks_label,
                                                                                             EnrichmentOptions_keepScps_level_1_label,
                                                                                             EnrichmentOptions_keepScps_level_2_label,
                                                                                             EnrichmentOptions_keepScps_level_3_label,
                                                                                             EnrichmentOptions_keepScps_level_4_label,
                                                                                             EnrichmentOptions_maxPvalue_label,
                                                                                             EnrichmentOptions_standardKeepTopScps_label,
                                                                                             EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox,
                                                                                             EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox,
                                                                                             EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox,
                                                                                             EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox,
                                                                                             EnrichmentOptions_dynamicKeepTopScps_label,
                                                                                             EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox,
                                                                                             EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox,
                                                                                             EnrichmentOptions_standardPvalue_textBox,
                                                                                             EnrichmentOptions_dynamicPvalue_textBox,
                                                                                             EnrichmentOptions_ScpInteractionsLevel_label,
                                                                                             EnrichmentOptions_scpInteractionsLevel_2_label,
                                                                                             EnrichmentOptions_scpInteractionsLevel_3_label,
                                                                                             EnrichmentOptions_percentDynamicTopSCPInteractions_label,
                                                                                             EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox,
                                                                                             EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox,
                                                                                             EnrichmentOptions_cutoffsExplanation_label,
                                                                                             EnrichmentOptions_default_button,

                                                                                             EnrichmentOptions_defineOutputs_panel,
                                                                                             EnrichmentOptions_generateBardiagrams_cbButton,
                                                                                             EnrichmentOptions_generateBardiagrams_cbLabel,
                                                                                             EnrichmentOptions_generateBardiagramsExplanation_label,
                                                                                             EnrichmentOptions_generateHeatmaps_cbButton,
                                                                                             EnrichmentOptions_generateHeatmaps_cbLabel,
                                                                                             EnrichmentOptions_generateHeatmapsExplanation_label,
                                                                                             EnrichmentOptions_generateHeatmapShowRanks_cbButton,
                                                                                             EnrichmentOptions_generateHeatmapShowRanks_cbLabel,
                                                                                             EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton,
                                                                                             EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel,
                                                                                             EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton,
                                                                                             EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel,
                                                                                             EnrichmentOptions_generateTimeline_cbButton,
                                                                                             EnrichmentOptions_generateTimeline_cbLabel,
                                                                                             EnrichmentOptions_generateTimelineExplanation_label,
                                                                                             EnrichmentOptions_generateTimelinePvalue_label,
                                                                                             EnrichmentOptions_generateTimelinePvalue_textBox,
                                                                                             EnrichmentOptions_generateTimelineLogScale_cbButton,
                                                                                             EnrichmentOptions_generateTimelineLogScale_cbLabel,
                                                                                             EnrichmentOptions_safeFigures_label,
                                                                                             EnrichmentOptions_saveFiguresAs_ownListBox,
                                                                                             EnrichmentOptions_saveFiguresAsExplanation_label,
                                                                                             EnrichmentOptions_chartsPerPage_label,
                                                                                             EnrichmentOptions_chartsPerPage_ownCheckBox,

                                                                                             EnrichmentOptions_colors_panel,
                                                                                             EnrichmentOptions_colorBarsTimelines_label,
                                                                                             EnrichmentOptions_colorByLevel_cbButton,
                                                                                             EnrichmentOptions_colorByLevel_cbLabel,
                                                                                             EnrichmentOptions_colorByDatasetColor_cbButton,
                                                                                             EnrichmentOptions_colorByDatasetColor_cbLabel,

                                                                                             Report_headline_label,
                                                                                             Report_maxErrorPerFile1_label,
                                                                                             Report_maxErrorPerFile2_label,
                                                                                             Report_ownTextBox,
                                                                                             Read_error_reports_maxErrorsPerFile_ownTextBox,

                                                                                             EnrichmentOptions_explanation_button,

                                                                                             Mbco_enrichment_pipeline.Options,
                                                                                             Form_default_settings);
        }
        private void Initialize_and_reset_datasetSummary_userInterface()
        {
            DatasetSummary_userInterface = new DatasetSummary_userInterface_class(DatasetInterface_overall_panel,
                                                                                  Input_geneList_label,
                                                                                  Input_geneList_textBox,
                                                                                  Dataset_scrollBar,
                                                                                  AddNewDataset_button,
                                                                                  AnalyzeData_button,
                                                                                  ClearCustomData_button,
                                                                                  Changes_update_button,
                                                                                  Changes_reset_button,
                                                                                  Delete_panel,
                                                                                  Dataset_all_delete_cbButton,
                                                                                  Name_panel,
                                                                                  Name_label,
                                                                                  Name_sortBy_button,
                                                                                  Timeline_panel,
                                                                                  Timeline_label,
                                                                                  Timeline_sortBy_button,
                                                                                  EntryType_panel,
                                                                                  EntryType_label,
                                                                                  EntryType_sortBy_button,
                                                                                  IntegrationGroup_panel,
                                                                                  IntegrationGroup_label,
                                                                                  IntegrationGroup_sortBy_button,
                                                                                  Color_panel,
                                                                                  Color_label,
                                                                                  Color_sortBy_button,
                                                                                  Substring_panel,
                                                                                  Substring_label,
                                                                                  Substring_sortBy_button,
                                                                                  Source_panel,
                                                                                  Source_label,
                                                                                  Source_sortBy_button,
                                                                                  BgGenes_panel,
                                                                                  BgGenes_label,
                                                                                  BgGenes_sortBy_button,
                                                                                  DatasetOrderNo_panel,
                                                                                  DatasetOrderNo_label,
                                                                                  DatasetOrderNo_sortBy_button,
                                                                                  DatasetsCount_panel,
                                                                                  GeneCounts_panel,
                                                                                  CompatibilityInfos_label,
                                                                                  Custom_data,
                                                                                  Form_default_settings);

            int ui_length = DatasetSummary_userInterface.UserInterface_lines.Length;
            for (int indexUI = 0; indexUI < ui_length; indexUI++)
            {
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_sampleName_textBox.TextChanged += Dataset_sampleName_ownTextBox_TextChanged;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_bgGenes_listBox.SelectedIndexChanged += Dataset_bgGenes_ownListBox_SelectedIndexChanged;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_color_listBox.SelectedIndexChanged += Dataset_color_ownListBox_SelectedIndexChanged; ;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_entryType_listBox.SelectedIndexChanged += Dataset_entryType_ownCheckBox_SelectedIndexChanged;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_integrationGroup_textBox.TextChanged += Dataset_integrationGroup_ownTextBox_TextChanged; ;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_orderNo_textBox.TextChanged += Dataset_datsetOrderNo_ownTextBox_TextChanged;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_sourceFile_textBox.TextChanged += Dataset_sourceFile_ownTextBox_TextChanged;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_timeunit_listBox.SelectedIndexChanged += Dataset_timeline_ownListBox_SelectedIndexChanged;
                DatasetSummary_userInterface.UserInterface_lines[indexUI].Dataset_time_textBox.TextChanged += Dataset_timeline_ownTextBox_TextChanged;
            }

            if (0 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[0].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_00_clicked; }
            if (1 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[1].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_01_clicked; }
            if (2 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[2].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_02_clicked; }
            if (3 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[3].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_03_clicked; }
            if (4 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[4].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_04_clicked; }
            if (5 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[5].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_05_clicked; }
            if (6 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[6].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_06_clicked; }
            if (7 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[7].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_07_clicked; }
            if (8 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[8].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_08_clicked; }
            if (9 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[9].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_09_clicked; }
            if (10 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[10].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_10_clicked; }
            if (11 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[11].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_11_clicked; }
            if (12 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[12].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_12_clicked; }
            if (13 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[13].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_13_clicked; }
            if (14 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[14].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_14_clicked; }
            if (15 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[15].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_15_clicked; }
            if (16 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[16].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_16_clicked; }
            if (17 < ui_length)
            { DatasetSummary_userInterface.UserInterface_lines[17].Dataset_delete_cbButton.Click += Dataset_delete_myCheckBoxButton_17_clicked; }

            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.Name].Click += SortBy_sampleName_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.Substring].Click += SortBy_substring_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.EntryType].Click += SortBy_entryType_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.Timepoint].Click += SortBy_timepoint_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.IntegrationGroup].Click += SortBy_integrationGroup_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.Color].Click += SortBy_sampleColor_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.Dataset_order_no].Click += SortBy_datasetOrderNo_button_Click;
            DatasetSummary_userInterface.DatasetAttributes_sortByButton_dict[Dataset_attributes_enum.SourceFile].Click += SortBy_sourceFileName_button_Click;

            Dataset_attributes_enum[] selected_attributes = UserInterface_organize_data.Get_dataset_attributes_from_showCheckBoxes();
            DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(selected_attributes);
        }

        private void Update_overallApplicationSize()
        {
            //this.Location = new Point(50, 50);
            this.Dock = DockStyle.Fill;
            int width;
            int height;
            Form_default_settings.Get_overall_panel_width_and_height(out width, out height);
            this.Width = width;
            this.Height = height;
            this.BackColor = Form_default_settings.Color_overall_background;
        }

        private void Update_all_graphic_elements_of_shared_tools()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            OwnTextBox my_textBox;
            System.Windows.Forms.Label my_label;

            #region Error reports, warnings and explanation labels and textBox
            Form_default_settings.MyPanelOverallDatsetInterface_add_default_parameters(Report_panel);

            left_referenceBorder = (int)Math.Round(0.02 * this.Report_panel.Width);
            right_referenceBorder = this.Report_panel.Width;
            top_referenceBorder = (int)Math.Round(0.05 * this.Report_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.9F*this.Report_panel.Height);
            my_textBox = this.Report_ownTextBox;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.2F*this.Report_panel.Width);
            right_referenceBorder = left_referenceBorder + (int)Math.Round(0.04F * this.Report_panel.Width);
            top_referenceBorder = Report_ownTextBox.Location.Y + Report_ownTextBox.Height + (int)Math.Round(0.01 * this.Report_panel.Height);
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.06F * this.Report_panel.Height);
            my_textBox = this.Read_error_reports_maxErrorsPerFile_ownTextBox;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.Read_error_reports_maxErrorsPerFile_ownTextBox.Location.X;
            top_referenceBorder = 0;
            bottom_referenceBorder = Report_ownTextBox.Location.Y;
            my_label = this.Report_headline_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int distance_aboveBelow_button = (int)Math.Round(0.01 * this.Report_panel.Height);
            top_referenceBorder = this.Read_error_reports_maxErrorsPerFile_ownTextBox.Location.Y - distance_aboveBelow_button;
            bottom_referenceBorder = this.Read_error_reports_maxErrorsPerFile_ownTextBox.Location.Y + this.Read_error_reports_maxErrorsPerFile_ownTextBox.Height + distance_aboveBelow_button;

            left_referenceBorder = 0;
            right_referenceBorder = this.Read_error_reports_maxErrorsPerFile_ownTextBox.Location.X;
            my_label = this.Report_maxErrorPerFile1_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.Read_error_reports_maxErrorsPerFile_ownTextBox.Location.X + this.Read_error_reports_maxErrorsPerFile_ownTextBox.Width;
            right_referenceBorder = this.Report_panel.Width;
            my_label = this.Report_maxErrorPerFile2_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Progress/Error report panel
            Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 0);
            #endregion

            #region Results and input directories
            int height_of_row = (int)Math.Round(0.055 * DatasetInterface_overall_panel.Height);


            bottom_referenceBorder = this.Progress_report_label.Location.Y + Progress_report_label.Height + height_of_row;
            left_referenceBorder = this.DatasetInterface_overall_panel.Location.X;
            right_referenceBorder = this.Options_readData_button.Location.X - (int)Math.Round(0.001*this.Width);

            top_referenceBorder = bottom_referenceBorder + height_of_row;
            bottom_referenceBorder = top_referenceBorder + height_of_row;
            my_textBox = ResultsDirectory_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = bottom_referenceBorder + height_of_row;
            bottom_referenceBorder = top_referenceBorder + height_of_row;
            my_textBox = Read_directoryOrFile_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            bottom_referenceBorder = this.ResultsDirectory_textBox.Location.Y;
            top_referenceBorder = bottom_referenceBorder - height_of_row;
            my_label = this.ResultsDirectory_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            bottom_referenceBorder = this.Read_directoryOrFile_ownTextBox.Location.Y;
            top_referenceBorder = this.ResultsDirectory_textBox.Location.Y + this.ResultsDirectory_textBox.Height;
            my_label = this.Read_directoryOrFile_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            Update_acknoledgment_and_application_headline();

        }
        private void Initialize_and_reset_read_userInterface()
        {
            UserInterface_read = new Read_interface_class(Options_readData_panel,
                                                          Read_headline_label,
                                                          Read_sampleNameColumn_label,
                                                          Read_sampleNameColumn_ownTextBox,
                                                          Read_timepointColumn_label,
                                                          Read_timepointColumn_ownTextBox,
                                                          Read_timeunitColumn_label,
                                                          Read_timeunitColumn_ownTextBox,
                                                          Read_colorColumn_label,
                                                          Read_colorColumn_ownTextBox,
                                                          Read_geneSymbol_label,
                                                          Read_geneSymbol_ownTextBox,
                                                          Read_value1stColumn_label,
                                                          Read_value1stColumn_ownTextBox,
                                                          Read_value1st_explanation_label,
                                                          Read_value2ndColumn_label,
                                                          Read_value2ndColumn_ownTextBox,
                                                          Read_value2nd_explanation_label,
                                                          Read_integrationGroupColumn_label,
                                                          Read_integrationGroupColumn_ownTextBox,
                                                          Read_timeunit_ownCheckBox,
                                                          Read_delimiter_label,
                                                          Read_delimiter_ownListBox,
                                                          Read_order_allFilesDirectory_label,
                                                          Read_order_allFilesInDirectory_cbButton,
                                                          Read_order_allFilesInDirectory_label,
                                                          Read_order_onlySpecifiedFile_cbButton,
                                                          Read_order_onlySpecifiedFile_label,
                                                          Read_directoryOrFile_label,
                                                          Read_directoryOrFile_ownTextBox,
                                                          Read_informationGroup_label,
                                                          Read_setToDefault_label,
                                                          Read_setToMBCO1_button,
                                                          Read_setToMBCO2_button,
                                                          Read_setToMBCO_button,
                                                          Read_setToSeurat_button,
                                                          Read_setToMinimum_button,
                                                          Read_setToOptimum_button,
                                                          Read_readDataset_button,
                                                          Report_headline_label,
                                                          Read_error_reports_button,
                                                          Read_error_reports_label,
                                                          Report_ownTextBox,
                                                          Report_maxErrorPerFile1_label,
                                                          Report_maxErrorPerFile2_label,
                                                          Read_error_reports_maxErrorsPerFile_ownTextBox,
                                                          Progress_report_label,
                                                          Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_scpNetworks()
        {
            this.UserInterface_scp_networks = new ScpNetworks_userInterface_class(Options_scpNetworks_panel,
                                                                                  ScpNetworks_standard_panel,
                                                                                  ScpNetworks_standard_label,
                                                                                  ScpNetworks_dynamic_panel,
                                                                                  ScpNetworks_dynamic_label,
                                                                                  ScpNetworks_comments_panel,
                                                                                  ScpNetworks_commentsHeadline_label,
                                                                                  SCPNetworks_comments_yEDText_label,
                                                                                  ScpNetworks_comments_standardDynamicAddGenes_label,
                                                                                  ScpNetworks_default_button,
                                                                                  ScpNetworks_standardParentChild_cbButton,
                                                                                  ScpNetworks_standardParentChild_cbLabel,
                                                                                  ScpNetworks_standardGroupSameLevelSCPs_cbButton,
                                                                                  ScpNetworks_standardGroupSameLevelSCPs_cbLabel,
                                                                                  ScpNetworks_standardAddGenes_cbButton,
                                                                                  ScpNetworks_standardAddGenes_cbLabel,
                                                                                  ScpNetworks_standardConnectRelated_cbButton,
                                                                                  ScpNetworks_standardConnectRelated_cbLabel,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_panel,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_level_2_label,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_level_3_label,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_connect_label,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox,
                                                                                  ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox,
                                                                                  ScpNetworks_dynamicParentChild_cbButton,
                                                                                  ScpNetworks_dynamicParentChild_cbLabel,
                                                                                  ScpNetworks_dynamicGroupSameLevelSCPs_cbButton,
                                                                                  ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel,
                                                                                  ScpNetworks_dynamicAddGenes_cbButton,
                                                                                  ScpNetworks_dynamicAddGenes_cbLabel,
                                                                                  ScpNetworks_dynamicConnectAllRelated_cbButton,
                                                                                  ScpNetworks_dynamicConnectAllRelated_cbLabel,
                                                                                  ScpNetworks_dynamicConnectAllScps_explantion_label,
                                                                                  ScpNetworks_generateNetworks_cbButton,
                                                                                  ScpNetworks_generateNetworks_cbLabel,
                                                                                  ScpNetworks_nodeSize_panel,
                                                                                  ScpNetworks_scpNodeSizes_label,
                                                                                  ScpNetworks_nodeSizeByDatasetCount_cbButton,
                                                                                  ScpNetworks_nodeSizeByDatasetCount_cbLabel,
                                                                                  ScpNetworks_nodeSizeByColorCount_cbButton,
                                                                                  ScpNetworks_nodeSizeByColorCount_cbLabel,
                                                                                  ScpNetworks_nodeSizeFixed_cbButton,
                                                                                  ScpNetworks_nodeSizeFixed_cbLabel,

                                                                                  Report_headline_label,
                                                                                  Report_maxErrorPerFile1_label,
                                                                                  Report_maxErrorPerFile2_label,
                                                                                  Report_ownTextBox,
                                                                                  Read_error_reports_maxErrorsPerFile_ownTextBox,

                                                                                  ScpNetworks_explanation_button,

                                                                                  Mbco_network_integration.Options,
                                                                                  Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_dataSig()
        {
            this.UserInterface_sigData = new SigData_userInterface_class(Options_dataSignificance_panel,
                                           SigData_sigSelection_panel,
                                           SigData_value1st_headline_label,
                                           SigData_value1st_higherSig_cbButton,
                                           SigData_value1st_higherSig_cbLabel,
                                           SigData_value1st_higherSig_expl_label,
                                           SigData_value1st_lowerSig_cbButton,
                                           SigData_value1st_lowerSig_cbLabel,
                                           SigData_value1st_lowerSig_expl_label,
                                           SigData_value2nd_headline_label,
                                           SigData_value2nd_higherSig_cbButton,
                                           SigData_value2nd_higherSig_cbLabel,
                                           SigData_value2nd_higherSig_expl_label,
                                           SigData_value2nd_lowerSig_cbButton,
                                           SigData_value2nd_lowerSig_cbLabel,
                                           SigData_value2nd_lowerSig_expl_label,
                                           SigData_1st_sigCutoff_headline_label,
                                           SigData_value1st_cutoff_label,
                                           SigData_value1st_cutoff_ownTextBox,
                                           SigData_value1st_cutoff_expl_label,
                                           SigData_value2nd_cutoff_label,
                                           SigData_value2nd_cutoff_ownTextBox,
                                           SigData_value2nd_cutoff_expl_label,
                                           SigData_2nd_sigCutoff_headline_label,
                                           SigData_rankBy_top_label,
                                           SigData_rankBy_left_label,
                                           SigData_rankBy_1stValue_cbButton,
                                           SigData_rankBy_1stValue_cbLabel,
                                           SigData_rankBy_2ndValue_cbButton,
                                           SigData_rankBy_2ndValue_cbLabel,
                                           SigData_tieBreaker_explanation_label,
                                           SigData_keepTopRanks_left_label,
                                           SigData_keepTopRanks_ownTextBox,
                                           SigData_keepTopRanks_right_label,
                                           SigData_keep_eachDataset_cbButton,
                                           SigData_keep_eachDataset_cbLabel,
                                           SigData_keep_mergeUpDown_cbButton,
                                           SigData_keep_mergeUpDown_cbLabel,
                                           SigData_deleteNotSignGenes_cbButton,
                                           SigData_deleteNotSignGenes_cbLabel,
                                           SigData_allGenesSignificant_headline_label,
                                           SigData_allGenesSignificant_cbButton,
                                           SigData_allGenesSignificant_cbLabel,
                                           SigData_resetSig_button,
                                           SigData_resetParameter_button,
                                           SigData_sigSubject_explanation_label,
                                           Custom_data.Options,
                                           Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_organizeData()
        {
            UserInterface_organize_data = new OrganizeData_userInterface_class(Options_organizeData_panel,
                                                                               OrganizeData_show_panel,
                                                                               OrganizeData_show_headline_label,
                                                                               OrganizeData_showName_cbButton,
                                                                               OrganizeData_showName_cbLabel,
                                                                               OrganizeData_showColor_cbButton,
                                                                               OrganizeData_showColor_cbLabel,
                                                                               OrganizeData_showTimepoint_cbButton,
                                                                               OrganizeData_showTimepoint_cbLabel,
                                                                               OrganizeData_showEntryType_cbButton,
                                                                               OrganizeData_showEntryType_cbLabel,
                                                                               OrganizeData_showIntegrationGroup_cbButton,
                                                                               OrganizeData_showIntegrationGroup_cbLabel,
                                                                               OrganizeData_showSourceFile_cbButton,
                                                                               OrganizeData_showSourceFile_cbLabel,
                                                                               OrganizeData_showSourceFile_label,
                                                                               OrganizeData_showDatasetOrderNo_cbButton,
                                                                               OrganizeData_showDatasetOrderNo_cbLabel,
                                                                               OrganizeData_showDifferentEntries_button,
                                                                               OrganizeData_showDifferentEntries_label,
                                                                               OrganizeData_addFileName_panel,
                                                                               OrganizeData_addFileNames_label,
                                                                               OrganizeData_addFileNames_listBox,
                                                                               OrganizeData_addFileNamesBefore_button,
                                                                               OrganizeData_addFileNameBefore_label,
                                                                               OrganizeData_addFileNameAfter_button,
                                                                               OrganizeData_addFileNameAfter_label,
                                                                               OrganizeData_addFileNameRemove_button,
                                                                               OrganizeData_addFileNameRemove_label,
                                                                               OrganizeData_convertTimeunits_panel,
                                                                               OrganizeData_convertTimeunits_label,
                                                                               OrganizeData_convertTimeunits_unit_ownListBox,
                                                                               OrganizeData_convertTimeunites_convert_button,
                                                                               OrganizeData_modify_panel,
                                                                               OrganizeData_modifyHeadline_label,
                                                                               OrganizeData_modifyName_cbButton,
                                                                               OrganizeData_modifyName_cbLabel,
                                                                               OrganizeData_modifyTimepoint_cbButton,
                                                                               OrganizeData_modifyTimepoint_cbLabel,
                                                                               OrganizeData_modifyEntryType_cbButton,
                                                                               OrganizeData_modifyEntryType_cbLabel,
                                                                               OrganizeData_modifySourceFileName_cbButton,
                                                                               OrganizeData_modifySourceFileName_cbLabel,
                                                                               OrganizeData_modifySubstring_cbButton,
                                                                               OrganizeData_modifySubstring_cbLabel,
                                                                               OrganizeData_modifySubstringOptions_panel,
                                                                               OrganizeData_modifyDelimiter_label,
                                                                               OrganizeData_modifyDelimiter_ownTextBox,
                                                                               OrganizeData_modifyIndexes_label,
                                                                               OrganizeData_modifyIndexLeft_label,
                                                                               OrganizeData_modifyIndexLeft_ownTextBox,
                                                                               OrganizeData_modifyIndexRight_label,
                                                                               OrganizeData_modifyIndexRight_ownTextBox,
                                                                               OrganizeData_changeIntegrationGroup_button,
                                                                               OrganizeData_changeColor_button,
                                                                               OrganizeData_changeDelete_button,
                                                                               OrganizeData_automatically_panel,
                                                                               OrganizeData_automatically_headline_label,
                                                                               OrganizeData_automaticIntegrationGroups_button,
                                                                               OrganizeData_automaticColors_button,
                                                                               OrganizeData_automaticDatasetOrder_button,

                                                                                                                                                        Report_headline_label,
                                                                               Report_maxErrorPerFile1_label,
                                                                               Report_maxErrorPerFile2_label,
                                                                               Report_ownTextBox,
                                                                               Read_error_reports_maxErrorsPerFile_ownTextBox,
                                                                               OrganizeData_explanation_button,


                                                                               Form_default_settings
                                                                               ); ;
        }
        private void Initialize_and_reset_userInterface_bgGenes()
        {
            this.UserInterface_bgGenes = new BgGenes_userInterface_class(Options_bgGenes_panel,
                                                                         BgGenes_overall_headline_label,
                                                                         BgGenes_add_panel,
                                                                         BgGenes_addGenes_label,
                                                                         BgGenes_addGenes_ownTextBox,
                                                                         BgGenes_addName_label,
                                                                         BgGenes_addName_ownTextBox,
                                                                         BgGenes_add_button,
                                                                         BgGenes_addReadFileDir_label,
                                                                         BgGenes_addReadOnlyFile_cbButton,
                                                                         BgGenes_addReadOnlyFile_cbLabel,
                                                                         BgGenes_addReadAllFilesInDirectory_cbButton,
                                                                         BgGenes_addReadAllFilesInDirectory_cbLabel,
                                                                         BgGenes_AddRead_button,
                                                                         BgGenes_AddShowErrors_button,
                                                                         BgGenes_AddErrors_label,
                                                                         BgGenes_addReadExplainFile_label,
                                                                         BgGenes_organize_panel,
                                                                         BgGenes_OrganizeAvailableBgGeneLists_label,
                                                                         BgGenes_OrganizeAvailableBgGeneLists_ownListBox,
                                                                         BgGenes_OrganizeDeleteSelection_button,
                                                                         BgGenes_OrganizeDeleteAll_button,
                                                                         BgGenes_assignment_panel,
                                                                         BgGenes_assignmentsAutomatic_button,
                                                                         BgGenes_assignmentsAutomatic_label,
                                                                         BgGenes_assignmentsReset_button,
                                                                         BgGenes_assignmentsReset_label,
                                                                         BgGenes_assignmentsExplanation_label,
                                                                         BgGenes_warnings_panel,
                                                                         BgGenes_warnings_label,
                                                                         BgGenes_warnings_button,
                                                                         Custom_data,
                                                                         Read_directoryOrFile_ownTextBox,
                                                                         Read_directoryOrFile_label,
                                                                         Report_headline_label,
                                                                         Report_maxErrorPerFile1_label,
                                                                         Report_maxErrorPerFile2_label,
                                                                         Report_ownTextBox,
                                                                         Read_error_reports_maxErrorsPerFile_ownTextBox,
                                                                         Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_selectSCPs()
        {
            UserInterface_selectSCPs = new Select_scps_userInterface_class(Options_selectSCPs_panel,
                                                                           SelectSCPs_overallHeadline_label,
                                                                           SelectSCPs_newGroup_label,
                                                                           SelectSCPs_newGroup_ownTextBox,
                                                                           SelectSCPs_addGroup_button,
                                                                           SelectSCPs_removeGroup_button,
                                                                           SelectSCPs_groups_label,
                                                                           SelectSCPs_groups_ownListBox,
                                                                           SelectSCPs_selection_panel,
                                                                           SelectScps_mbcoSCPs_ownListBox,
                                                                           SelectScps_selectedGroup_label,
                                                                           SelectScps_selectedSCPs_ownListBox,
                                                                           SelectSCPs_sortSCPs_label,
                                                                           SelectSCPs_sortSCPs_listBox,
                                                                           SelectSCPs_add_button,
                                                                           SelectSCPs_remove_button,
                                                                           SelectSCPs_includeHeadline_label,
                                                                           SelectSCPs_includeBracket_label,
                                                                           SelectSCPs_includeOffspringSCPs_cbButton,
                                                                           SelectSCPs_includeOffspringSCPs_cbLabel,
                                                                           SelectSCPs_includeAncestorSCPs_cbButton,
                                                                           SelectSCPs_includeAncestorSCPs_cbLabel,
                                                                           SelectedSCPs_writeMbcoHierarchy_button,
                                                                           SelectSCPs_showOnlySelectedScps_cbButton,
                                                                           SelectSCPs_showOnlySelectedScps_cbLabel,
                                                                           SelectSCPs_addGenes_cbButton,
                                                                           SelectSCPs_addGenes_cbLabel,
                                                                           Mbco_enrichment_pipeline.Options,
                                                                           Progress_report_label,
                                                                           Form_default_settings
                                                                           );
        }
        private void Initialize_and_reset_userInterface_loadExamples()
        {
            this.UserInterface_loadExamples = new LoadExamples_userInterface_class(Options_loadExamples_panel,
                                                                                   LoadExamples_overallHeadline_label,
                                                                                   LoadExamples_NOG_cbButton,
                                                                                   LoadExamples_NOG_cbLabel,
                                                                                   LoadExamples_NOG_reference,
                                                                                   LoadExamples_KPMPreference_cbButton,
                                                                                   LoadExamples_KPMPreference_cbLabel,
                                                                                   LoadExamples_KPMP_reference,
                                                                                   LoadExamples_load_button,
                                                                                   LoadExamples_copyright_label,
                                                                                   Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_defineSCPs()
        {
            UserInterface_defineSCPs = new DefineSCPs_userInterface_class(Options_defineScps_panel,
                                                                          DefineScps_overall_headline_label,
                                                                          DefineScps_newOwnScpName_label,
                                                                          DefineScps_newOwnScpName_ownTextBox,
                                                                          DefineScps_addNewOwnSCP_button,
                                                                          DefineScps_removeOwnSCP_button,
                                                                          DefineScps_selectOwnScp_label,
                                                                          DefineScps_selectOwnScp_ownListBox,
                                                                          DefineScps_level_label,
                                                                          DefineScps_level1_cbButton,
                                                                          DefineScps_level1_cbLabel,
                                                                          DefineScps_level2_cbButton,
                                                                          DefineScps_level2_cbLabel,
                                                                          DefineScps_level3_cbButton,
                                                                          DefineScps_level3_cbLabel,
                                                                          DefineScps_level4_cbButton,
                                                                          DefineScps_level4_cbLabel,
                                                                          DefineScps_selection_panel,
                                                                          DefineScps_mbcoSCP_label,
                                                                          DefineScps_mbcoSCP_ownListBox,
                                                                          DefineScps_sort_label,
                                                                          DefineScps_sort_listBox,
                                                                          DefineScps_addSubScp_button,
                                                                          DefineScps_removeSubScp_button,
                                                                          DefineSCPs_writeMbcoHierarchy_button,
                                                                          DefineScps_ownSubScps_label,
                                                                          DefineScps_ownSubScps_ownListBox,
                                                                          Progress_report_label,
                                                                          Mbco_enrichment_pipeline.Options,
                                                                          Form_default_settings); 
        }
        private void Initialize_and_reset_userInterface_tips()
        {
            UserInterface_tips = new Tips_userInterface_class(Options_tips_panel,
                                                              Tips_overallHeadline_label,
                                                              Tips_label1,
                                                              Tips_label2,
                                                              Tips_label3,
                                                              Tips_label4,
                                                              Tips_label5,
                                                              Tips_label6,
                                                              Tips_demonstration_headline_label,
                                                              Tips_demonstration_cbButton,
                                                              Tips_demonstration_cbLabel,
                                                              Form_default_settings
                                                             );
        }
        #endregion

        private void Initialize_and_reset_userInterface_results()
        {
            UserInterface_results = new Results_userInterface_class(Options_results_panel,
                                                                    Results_overall_headline_label,
                                                                    Results_controlCommand_panel,
                                                                    Results_integrationGroup_label,
                                                                    Results_integrationGroup_listBox,
                                                                    Results_bardiagram_show_label,
                                                                    Results_bardiagram_standard_cbButton,
                                                                    Results_bardiagram_standard_cbLabel,
                                                                    Results_bardiagram_dynamic_cbButton,
                                                                    Results_bardiagram_dynamic_cbLabel,
                                                                    Results_heatmap_show_label,
                                                                    Results_heatmap_standard_cbButton,
                                                                    Results_heatmap_standard_cbLabel,
                                                                    Results_heatmap_dynamic_cbButton,
                                                                    Results_heatmap_dynamic_cbLabel,
                                                                    Results_timeline_show_label,
                                                                    Results_timeline_cbButton,
                                                                    Results_timeline_cbLabel,
                                                                    Results_directory_headline_label,
                                                                    Results_directory_label,
                                                                    Results_directory_expl_label,
                                                                    Results_addResultsToControl_cbButton,
                                                                    Results_addResultsToControl_cbLabel,
                                                                    Results_visualization_panel,
                                                                    Results_visualization_integrationGroup_label,
                                                                    Results_zegGraph_control,
                                                                    Results_previous_button,
                                                                    Results_next_button,
                                                                    Results_position_label,
                                                                    Progress_report_label,
                                                                    Form_default_settings);
        }
        private void Generate_results_directory_replace_and_refresh()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string working_path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            ResultsDirectory_textBox.Text = working_path + gdf.Delimiter + "Results" + gdf.Delimiter + "Analysis1" + gdf.Delimiter;
            //ResultsDirectory_textBox.Text = ResultsDirectory_textBox.Text.Replace(" ", "_");
            ResultsDirectory_textBox.Refresh();
        }
        private void Reset_gene_list_text_box()
        {
            Input_geneList_textBox.Text = Default_textBox_texts.InputGene_list_textBox_default;
        }

        #region Main menue buttons
        private void Write_parameter_documentations(string results_directory)
        {
            string date_time_string = DateTime.Now.ToLongDateString().Replace(" ", "_").Replace(",", "") + "_at_" + DateTime.Now.ToShortTimeString().Replace(":", "_").Replace(" ", "_");
            string complete_documentation_fileName = results_directory + "Selected_parameter_" + date_time_string + ".txt";
            System.IO.StreamWriter writer = Common_functions.ReadWrite.ReadWriteClass.Get_new_stream_writer_and_sent_notice_if_file_in_use(complete_documentation_fileName, Progress_report_label, Form_default_settings);
            User_data_options_class custom_data_options = Custom_data.Options;
            writer.WriteLine("Identification of significant genes:");
            writer.WriteLine("------------------------------------");
            if (custom_data_options.All_genes_significant)
            { writer.WriteLine("No significance cutoff applied. All genes are significant."); }
            else
            {
                string significance_maxMin_value_1st = "";
                string significance_maxMin_value_2nd = "";
                switch (custom_data_options.Significance_definition_value_1st)
                {
                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                        writer.WriteLine("Significance decreases with decreasing absolute 1st value");
                        writer.WriteLine("(Highest absolute 1st value is the most significant)");
                        significance_maxMin_value_1st = "Minimum";
                        break;
                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                        writer.WriteLine("Significance decreases with increasing absolute 1st value");
                        writer.WriteLine("(Lowest absolute 1st value is the most significant)");
                        significance_maxMin_value_1st = "Maximum";
                        break;
                    default:
                        throw new Exception();
                }
                switch (custom_data_options.Significance_definition_value_2nd)
                {
                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                        writer.WriteLine("Significance decreases with decreasing absolute 2nd value");
                        writer.WriteLine("(Highest absolute 2nd value is the most significant)");
                        significance_maxMin_value_2nd = "Minimum";
                        break;
                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                        writer.WriteLine("Significance decreases with increasing absolute 2nd value");
                        writer.WriteLine("(Lowest absolute 2nd value is the most significant)");
                        significance_maxMin_value_2nd = "Maximum";
                        break;
                    default:
                        throw new Exception();
                }
                writer.WriteLine();
                writer.WriteLine("To identify significant genes, 3 different cutoffs are applied:");
                writer.Write("1) " + significance_maxMin_value_1st + " absolute 1st value = ");
                writer.WriteLine("{0}", custom_data_options.Value_1st_cutoff);
                writer.Write("2) " + significance_maxMin_value_2nd + " absolute 2nd value = ");
                writer.WriteLine("{0}", custom_data_options.Value_2nd_cutoff);
                writer.WriteLine("3) Gene is among the top {0} ranked genes", custom_data_options.Keep_top_ranks);
                string dataset_string = "dataset";
                if (custom_data_options.Merge_upDown_before_ranking)
                {
                    dataset_string = "dataset*";
                }
                switch (custom_data_options.Value_importance_order)
                {
                    case Value_importance_order_enum.Value_1st_2nd:
                        writer.WriteLine("   (Genes within each {0} are ranked by decreasing significance of 1st value,", dataset_string);
                        writer.WriteLine("    decreasing 2nd value is used as as tiebreaker)");
                        break;
                    case Value_importance_order_enum.Value_2nd_1st:
                        writer.WriteLine("   (Genes within each {0} are ranked by decreasing significance of 2nd value,", dataset_string);
                        writer.WriteLine("    decreasing 1st value is used as as tiebreaker)");
                        break;
                    default:
                        throw new Exception();
                }
                if (custom_data_options.Merge_upDown_before_ranking)
                {
                    writer.WriteLine("    *Datasets that only differ in Up/Down status are temporarily merged to rank genes of");
                    writer.WriteLine("     both datasets together as one set.");
                }
            }
            writer.WriteLine();
            writer.WriteLine("-------------------------------------------------------------------------------------------------------");
            writer.WriteLine();
            writer.WriteLine();
            MBCO_enrichment_pipeline_options_class enrich_options = Mbco_enrichment_pipeline.Options;
            writer.WriteLine("Ontology");
            writer.WriteLine("--------");
            writer.WriteLine(enrich_options.Ontology);
            writer.WriteLine();
            string[] defined_scps = enrich_options.OwnScp_mbcoSubScps_dict.Keys.ToArray();
            int defined_scps_length = defined_scps.Length;
            if (defined_scps_length > 0)
            {
                writer.WriteLine("User defined SCPs:");
                string defined_scp;
                string[] mbco_scps;
                string mbco_scp;
                int mbco_scps_length;
                for (int indexDefined = 0; indexDefined < defined_scps_length; indexDefined++)
                {
                    defined_scp = defined_scps[indexDefined];
                    writer.Write("{0} (level {1}): ", defined_scp, enrich_options.OwnScp_level_dict[defined_scp]);
                    mbco_scps = enrich_options.OwnScp_mbcoSubScps_dict[defined_scp].OrderBy(l => l).ToArray();
                    mbco_scps_length = mbco_scps.Length;
                    for (int indexSub = 0; indexSub < mbco_scps_length; indexSub++)
                    {
                        mbco_scp = mbco_scps[indexSub];
                        if (indexSub > 0)
                        {
                            writer.WriteLine(",");
                            for (int i = 0; i < defined_scp.Length + 12; i++) { writer.Write(" "); }
                        }
                        writer.Write("{0}", mbco_scp);
                    }
                    writer.WriteLine();
                }
                writer.WriteLine();
            }
            writer.WriteLine("-------------------------------------------------------------------------------------------------------");
            writer.WriteLine();
            if (!enrich_options.Show_all_and_only_selected_scps)
            {
                writer.WriteLine("Identification of significant SCPs:");
                writer.WriteLine("-----------------------------------");
                writer.WriteLine("For the identification of significant SCPs, two different cutoffs");
                writer.WriteLine("that depend on the enrichment analysis type and SCP level are applied");
                writer.WriteLine("1) Maximum p-value:");
                writer.WriteLine("      Standard enrichment analysis: {0}", enrich_options.Max_pvalue_for_standardEnrichment);
                writer.WriteLine("      Dynamic enrichment analysis: {0}", enrich_options.Max_pvalue_for_dynamicEnrichment);
                writer.WriteLine("2) Being among the top x most significant predictions:");
                writer.WriteLine("   Standard enrichment analysis:");
                writer.WriteLine("      Level-1 SCPs: top {0}", enrich_options.Keep_top_predictions_standardEnrichment_per_level[1]);
                writer.WriteLine("      Level-2 SCPs: top {0}", enrich_options.Keep_top_predictions_standardEnrichment_per_level[2]);
                writer.WriteLine("      Level-3 SCPs: top {0}", enrich_options.Keep_top_predictions_standardEnrichment_per_level[3]);
                writer.WriteLine("      Level-4 SCPs: top {0}", enrich_options.Keep_top_predictions_standardEnrichment_per_level[4]);
                writer.WriteLine("   Dynamic enrichment analysis:");
                writer.WriteLine("      Level-2 SCPs/SCP-combinations: top {0}", enrich_options.Keep_top_predictions_dynamicEnrichment_per_level[2]);
                writer.WriteLine("      Level-3 SCPs/SCP-combinations: top {0}", enrich_options.Keep_top_predictions_dynamicEnrichment_per_level[3]);
                writer.WriteLine();
                writer.WriteLine("The annotated MBCO hierarchy is enriched by a unique MBCO algorithm that infers weighted interactions between level-2");
                writer.WriteLine("or level-3 SCPs from text mining results.");
                writer.WriteLine("The top x percent of these interactions are used as the basis for dynamic enrichment analysis. Dynamic enrichment analysis");
                writer.WriteLine("combines 2 to 3 functionally related SCPs that are connected by the top x percent of inferred interactions to generate");
                writer.WriteLine("context-specific higher-level SCPs that are added to the ontology. It is called dynamic enrichment analysis, because the");
                writer.WriteLine("context-specific SCPs depend on the analyzed dataset and are consequently generated during runtime.");
                writer.WriteLine("The following top x percent of interactions will be used for the generation of context-specific SCPs:");
                writer.WriteLine("      Interactions between level-2 SCPs: top {0}%", enrich_options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[2] * 100);
                writer.WriteLine("      Interactions between level-3 SCPs: top {0}%", enrich_options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[3] * 100);
                if (Timeline_diagram.Options.Generate_timeline)
                {
                    writer.WriteLine();
                    writer.WriteLine("-------------------------------------------------------------------------------------------------------");
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine("Identification of significant SCPs for timeline");
                    writer.WriteLine("-----------------------------------------------");
                    writer.WriteLine("The timeline shows the predicted -log10(p-values) of all SCPs at any timepoint,");
                    writer.WriteLine("if they were predicted at at least one timepoint with the maximum p-value shown below.");
                    writer.WriteLine("      Maximum p-value: {0}", Timeline_diagram.Options.Significance_pvalue_cutoff_copy);
                }
            }
            else
            {
                writer.WriteLine("Show only selected SCPs:");
                writer.WriteLine("------------------------");
                writer.WriteLine("Only selected SCPs will be shown, independently of any significance cutoffs.");
                string[] scpGroups = enrich_options.Group_selectedScps_dict.Keys.ToArray();
                string scpGroup;
                int scpGroups_length = scpGroups.Length;
                string group_annotation;
                string[] selected_scps;
                string selected_scp;
                int selected_scps_length;
                for (int indexGroup = 0; indexGroup < scpGroups_length; indexGroup++)
                {
                    scpGroup = scpGroups[indexGroup];
                    group_annotation = "Selected SCPs of group " + scpGroup + ": ";
                    writer.Write(group_annotation);
                    selected_scps = enrich_options.Group_selectedScps_dict[scpGroup];
                    selected_scps_length = selected_scps.Length;
                    for (int indexSelected=0; indexSelected<selected_scps_length; indexSelected++)
                    {
                        selected_scp = selected_scps[indexSelected];
                        if (indexSelected != 0) { for (int i = 0; i < group_annotation.Length; i++) { writer.Write(" "); } }
                        writer.WriteLine("{0}", selected_scp);
                    }
                    writer.WriteLine();
                }
            }
            writer.WriteLine();
            writer.WriteLine("-------------------------------------------------------------------------------------------------------");
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("SCP-network parameter for standard enrichment analysis");
            writer.WriteLine("------------------------------------------------------");
            if (Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks)
            {
                writer.WriteLine("SCPs that were predicted based on standard enrichment analysis are integrated into the MBCO hierarchy.");
                writer.WriteLine("Solid arrows point from parent to child SCPs.");
                writer.WriteLine();
            }
            if (Mbco_network_integration.Options.Add_edges_that_connect_standard_scps)
            {
                writer.WriteLine("The annotated MBCO hierarchy is enriched by a unique MBCO algorithm that infers weighted interactions between level-2");
                writer.WriteLine("or level-3 SCPs from text mining results.");
                writer.WriteLine("Level-2 or level-3 SCPs predicted by standard enrichment analysis are connected with each other by dashed lines without");
                writer.WriteLine("arrow heads, if their interactions are among the top x inferred interactions.");
                writer.WriteLine("      Interactions between level-2 SCPs: top {0}%", Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[2] * 100);
                writer.WriteLine("      Interactions between level-2 SCPs: top {0}%", Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[3] * 100);
            }
            writer.WriteLine();
            if (!Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps)
            {
                writer.WriteLine("SCP-network parameter for dynamic enrichment analysis");
                writer.WriteLine("-----------------------------------------------------");
                if (Mbco_network_integration.Options.Add_parent_child_relationships_to_dynamic_SCP_networks)
                {
                    writer.WriteLine("SCPs that were predicted based on dynamic enrichment analysis are integrated into the MBCO hierarchy.");
                    writer.WriteLine("Solid arrows point from parent to child SCPs.");
                    writer.WriteLine();
                }
                if (Mbco_network_integration.Options.Add_additional_edges_that_connect_dynamic_scps)
                {
                    writer.WriteLine("All SCPs that are predicted by dynamic enrichment analysis either as single SCPs or as part of any");
                    writer.WriteLine("context-specific higher-level SCPs are connected with each other based on the top infered interactions.");
                    writer.WriteLine("Consequently, the generated networks connect SCPs that are part of the same or different context-specific");
                    writer.WriteLine("higher-level SCPs. SCPs predicted for different datasets are also connected. These horizontal connections are");
                    writer.WriteLine("visualized by dashed lines without arrow heads. The top percentages of considered inferred SCP interactions");
                    writer.WriteLine("are the same as the one used for dynamic enrichment analysis that are specified together with the other"); 
                    writer.WriteLine("enrichment parameters (See \"Identification of significant SCPs\" above).");
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine("Level-2 or level-3 SCPs predicted by dynamic enrichment analysis are only connected, if they were predicted");
                    writer.WriteLine("as part of the same context-specific higher-level SCP (that is a combination of two or three related SCPs).");
                    writer.WriteLine("These horizontal interactions are visualized as dashed lines without arrow heads.");
                    writer.WriteLine("The SCPs that were combined to form a context-specific higher-level SCP label the same bardiagram in the");
                    writer.WriteLine("bardiagram plots. They are also listed as one entry in the column SCP (separated by dollar signs) of the");
                    writer.WriteLine("text file that shows the significant results for dynamic enrichment analysis.");
                    writer.WriteLine("(Since an SCP can be part of multiple context-specific higher-level SCPs, more than 3 SCPs can be");
                    writer.WriteLine(" connected in the generated networks.)");
                    writer.WriteLine();
                }
            }
            writer.Close();
        }

        private void Set_integration_group_add_to_file_name_and_standard_label(string integrationGroup_string, string filteredFileName_addition, out string integration_group_add_to_file, out string standard_label, out string dynamic_label)
        {
            if (!integrationGroup_string.Equals(DatasetSummary_userInterface.Get_default_integrationGroup_from_lastSaved_datasetSummaries()))
            {
                if (integrationGroup_string.Length > 0) { integration_group_add_to_file = "_" + integrationGroup_string; }
                else { integration_group_add_to_file = ""; }
                integration_group_add_to_file = Text_class.Replace_characters_that_are_incompatible_with_fileNames_by_underline(integration_group_add_to_file);
            }
            else
            {
                integration_group_add_to_file = "";
            }
            if (Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps)
            {
                standard_label = "_" + filteredFileName_addition;
            }
            else
            {
                standard_label = "_standard";
            }
            dynamic_label = "_dynamic";
        }

        private void Write_enrichment_results_text_files(Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, Ontology_enrichment_class combined_dynamic_not_filtered, string filtered_fileName_addition, string results_directory)
        {
            Progress_report_label.Text = "Writing results as tab-delimited files";
            Progress_report_label.Visible = true;
            Progress_report_label.Refresh();
            if ((combined_dynamic_filtered != null) && (combined_dynamic_filtered.Enrich.Length > 0))
            {
                Ontology_type_enum dynamic_ontology = combined_dynamic_not_filtered.Get_ontology_and_check_if_only_one_ontology();
                combined_dynamic_filtered.Write_and_continue_trying_until_file_free(results_directory, dynamic_ontology + "_dynamic_" + filtered_fileName_addition + ".txt", Progress_report_label, Form_default_settings);
                combined_dynamic_not_filtered.Write_and_continue_trying_until_file_free(results_directory, dynamic_ontology + "_dynamic_allPredictions.txt", Progress_report_label, Form_default_settings);
            }
            Ontology_type_enum standard_ontology = combined_standard_not_filtered.Get_ontology_and_check_if_only_one_ontology();
            combined_standard_filtered.Write_and_continue_trying_until_file_free(results_directory, standard_ontology + "_standard_" + filtered_fileName_addition + ".txt", Progress_report_label, Form_default_settings);
            combined_standard_not_filtered.Write_and_continue_trying_until_file_free(results_directory, standard_ontology + "_standard_allPredictions.txt", Progress_report_label, Form_default_settings);
            Progress_report_label.Visible = false;
        }

        private void Update_progress_report(string integrationGroup_string, string task_string)
        {
            Progress_report_label.Visible = true;
            if (integrationGroup_string.Length > 0)
            { Progress_report_label.Text = integrationGroup_string + ":\r\n" + task_string; }
            else { Progress_report_label.Text = task_string; }
            Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 0);
            Progress_report_label.Refresh();
        }

        private void Generate_bardiagrams_and_return_standard_and_dynamic_graphPanes_for_eachIntegrationGroup(out Dictionary<string,GraphPane[]> integrationGroup_bardiagram_standard_graphPanes_dict, out Dictionary<string,GraphPane[]> integrationGroup_bardiagram_dynamic_graphPanes_dict, Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, string filteredFileName_addition, string results_directory)
        {
            integrationGroup_bardiagram_dynamic_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            integrationGroup_bardiagram_standard_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            string[] integration_groups;
            string integration_group;
            string integration_group_add_to_file;
            int integration_groups_length;
            integration_groups = Common_functions.Array_own.Overlap_class.Get_union(combined_standard_filtered.Get_all_integrationGroups(), combined_dynamic_filtered.Get_all_integrationGroups());
            integration_groups_length = integration_groups.Length;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_filtered;
            Ontology_enrichment_class dynamic_currentIntegration_group_enrichment_filtered;
            Ontology_type_enum standard_ontology;
            Ontology_type_enum dynamic_ontology;
            string integrationGroup_string;
            string task_string;
            string standard_label;
            string dynamic_label;
            GraphPane[] new_dynamic_graphPanes;
            GraphPane[] new_standard_graphPanes;
            List<GraphPane> dynamic_graphPanes_list = new List<GraphPane>();
            List<GraphPane> standard_graphPanes_list = new List<GraphPane>();
            for (int indexIG = 0; indexIG < integration_groups_length; indexIG++)
            {
                integration_group = integration_groups[indexIG];
                integrationGroup_string = Default_textBox_texts.Get_integrationGroup_string(integration_group);

                standard_graphPanes_list.Clear();
                dynamic_graphPanes_list.Clear();

                Set_integration_group_add_to_file_name_and_standard_label(integrationGroup_string, filteredFileName_addition, out integration_group_add_to_file, out standard_label, out dynamic_label);
                task_string = "Generating and writing bardiagrams for standard enrichment analysis";
                Update_progress_report(integrationGroup_string, task_string);

                standard_currentIntegration_group_enrichment_filtered = combined_standard_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                if (standard_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                {
                    standard_ontology = standard_currentIntegration_group_enrichment_filtered.Get_ontology_and_check_if_only_one_ontology();
                    new_standard_graphPanes = Bardiagram.Generate_bardiagrams_from_enrichment_results_save_as_images_and_return_graphPanes(standard_currentIntegration_group_enrichment_filtered, results_directory, standard_ontology + standard_label + integration_group_add_to_file, Enrichment_algorithm_enum.Standard_enrichment, Form_default_settings);
                    standard_graphPanes_list.AddRange(new_standard_graphPanes);
                }
                if ((combined_dynamic_filtered!=null) && (combined_dynamic_filtered.Enrich.Length>0))
                {
                    task_string = "Generating and writing bardiagrams for dynamic enrichment analysis";
                    Update_progress_report(integrationGroup_string, task_string);
                    dynamic_currentIntegration_group_enrichment_filtered = combined_dynamic_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                    if (dynamic_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                    {
                        dynamic_ontology = standard_currentIntegration_group_enrichment_filtered.Get_ontology_and_check_if_only_one_ontology();
                        new_dynamic_graphPanes = Bardiagram.Generate_bardiagrams_from_enrichment_results_save_as_images_and_return_graphPanes(dynamic_currentIntegration_group_enrichment_filtered, results_directory, dynamic_ontology + "_dynamic" + integration_group_add_to_file, Enrichment_algorithm_enum.Dynamic_enrichment, Form_default_settings);
                        dynamic_graphPanes_list.AddRange(new_dynamic_graphPanes);
                    }
                }
                integrationGroup_bardiagram_standard_graphPanes_dict.Add(integration_group,standard_graphPanes_list.ToArray());
                integrationGroup_bardiagram_dynamic_graphPanes_dict.Add(integration_group, dynamic_graphPanes_list.ToArray());
            }
        }

        private void Generate_heatmaps_and_return_standard_and_dynamic_graphPanes_for_eachIntegrationGroup(out Dictionary<string,GraphPane[]> integrationGroup_heatmap_standard_graphPanes_dict, out Dictionary<string, GraphPane[]> integrationGroup_heatmap_dynamic_graphPanes_dict, Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, Ontology_enrichment_class combined_dynamic_not_filtered, string filteredFileName_addition, string results_directory)
        {
            integrationGroup_heatmap_standard_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            integrationGroup_heatmap_dynamic_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            string[] integration_groups;
            string integration_group;
            string integration_group_add_to_file;
            int integration_groups_length;
            integration_groups = Common_functions.Array_own.Overlap_class.Get_union(combined_standard_filtered.Get_all_integrationGroups(), combined_dynamic_filtered.Get_all_integrationGroups());
            integration_groups_length = integration_groups.Length;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_heatmap;
            Ontology_enrichment_class dynamic_currentIntegration_group_enrichment_heatmap;
            Ontology_enrichment_class integrationGroup_combined_standard_filtered;
            Ontology_enrichment_class integrationGroup_combined_dynamic_filtered;
            Ontology_enrichment_class integrationGroup_combined_standard_not_filtered;
            Ontology_enrichment_class integrationGroup_combined_dynamic_not_filtered;
            Ontology_type_enum standard_ontology;
            Ontology_type_enum dynamic_ontology;
            string integrationGroup_string;
            string task_string;
            string standard_label;
            string dynamic_label;
            GraphPane[] new_dynamic_graphPanes;
            GraphPane[] new_standard_graphPanes;
            List<GraphPane> dynamic_graphPanes_list = new List<GraphPane>();
            List<GraphPane> standard_graphPanes_list = new List<GraphPane>();
            for (int indexIG = 0; indexIG < integration_groups_length; indexIG++)
            {
                integration_group = integration_groups[indexIG];
                integrationGroup_string = Default_textBox_texts.Get_integrationGroup_string(integration_group);

                dynamic_graphPanes_list.Clear();
                standard_graphPanes_list.Clear();

                Set_integration_group_add_to_file_name_and_standard_label(integrationGroup_string, filteredFileName_addition, out integration_group_add_to_file, out standard_label, out dynamic_label);

                integrationGroup_combined_standard_not_filtered = combined_standard_not_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                integrationGroup_combined_dynamic_not_filtered = combined_dynamic_not_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                integrationGroup_combined_standard_filtered = combined_standard_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                integrationGroup_combined_dynamic_filtered = combined_dynamic_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);

                integrationGroup_combined_dynamic_filtered.Separate_scp_unions_into_single_scps_and_keep_line_defined_by_lowest_pvalue_for_each_scp_and_add_scp_specific_genes(combined_standard_not_filtered);
                integrationGroup_combined_dynamic_not_filtered.Separate_scp_unions_into_single_scps_and_keep_line_defined_by_lowest_pvalue_for_each_scp_and_add_scp_specific_genes(combined_standard_not_filtered);
                if ((Heatmap.Options.Show_significant_scps_over_all_conditions)||(Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps))
                {
                    string[] standard_scps = integrationGroup_combined_standard_filtered.Get_all_scps();
                    standard_currentIntegration_group_enrichment_heatmap = integrationGroup_combined_standard_not_filtered;
                    standard_currentIntegration_group_enrichment_heatmap.Keep_only_input_scpNames(standard_scps);

                    string[] dynamic_scps = integrationGroup_combined_dynamic_filtered.Get_all_scps();
                    dynamic_currentIntegration_group_enrichment_heatmap = integrationGroup_combined_dynamic_not_filtered;
                    dynamic_currentIntegration_group_enrichment_heatmap.Keep_only_input_scpNames(dynamic_scps);
                }
                else
                {
                    standard_currentIntegration_group_enrichment_heatmap = integrationGroup_combined_standard_filtered;
                    dynamic_currentIntegration_group_enrichment_heatmap = integrationGroup_combined_dynamic_filtered;
                }

                if (standard_currentIntegration_group_enrichment_heatmap.Enrich.Length > 0)
                {
                    standard_currentIntegration_group_enrichment_heatmap.Add_new_enrichment_lines_for_each_process_with_missing_completeSampleNames();
                    standard_currentIntegration_group_enrichment_heatmap.Check_for_correctness();
                    standard_ontology = standard_currentIntegration_group_enrichment_heatmap.Get_ontology_and_check_if_only_one_ontology();

                    task_string = "Generating and writing heatmaps for standard enrichment analysis";
                    Update_progress_report(integrationGroup_string, task_string);
                    new_standard_graphPanes = Heatmap.Generate_heatmaps_and_return_graphPanes(standard_currentIntegration_group_enrichment_heatmap, results_directory, standard_ontology + standard_label + integration_group_add_to_file, Enrichment_algorithm_enum.Standard_enrichment, Form_default_settings);
                    standard_graphPanes_list.AddRange(new_standard_graphPanes);
                }
                if ((dynamic_currentIntegration_group_enrichment_heatmap!=null) && (dynamic_currentIntegration_group_enrichment_heatmap.Enrich.Length > 0))
                {
                    dynamic_currentIntegration_group_enrichment_heatmap.Add_new_enrichment_lines_for_each_process_with_missing_completeSampleNames();
                    dynamic_currentIntegration_group_enrichment_heatmap.Check_for_correctness();
                    dynamic_ontology = dynamic_currentIntegration_group_enrichment_heatmap.Get_ontology_and_check_if_only_one_ontology();
                    task_string = "Generating and writing heatmaps for dynamic enrichment analysis";
                    Update_progress_report(integrationGroup_string, task_string);
                    new_dynamic_graphPanes = Heatmap.Generate_heatmaps_and_return_graphPanes(dynamic_currentIntegration_group_enrichment_heatmap, results_directory, dynamic_ontology + "_dynamic" + integration_group_add_to_file, Enrichment_algorithm_enum.Dynamic_enrichment, Form_default_settings);
                    dynamic_graphPanes_list.AddRange(new_dynamic_graphPanes);
                }
                integrationGroup_heatmap_standard_graphPanes_dict.Add(integration_group, standard_graphPanes_list.ToArray());
                integrationGroup_heatmap_dynamic_graphPanes_dict.Add(integration_group, dynamic_graphPanes_list.ToArray());
            }
        }

        private void Generate_timelines_and_return_standard_graphPanes_for_each_integrationGroup(out Dictionary<string,GraphPane[]> integrationGroup_timeline_standard_graphPanes_dict, Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_standard_not_filtered, string filteredFileName_addition, string results_directory)
        {
            Timeline_diagram.Options.Significance_pvalue_cutoff_copy = Mbco_enrichment_pipeline.Options.Timeline_pvalue_cutoff;
            integrationGroup_timeline_standard_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            string task_string = "Generating and writing timelines";
            string[] integration_groups = combined_standard_not_filtered.Get_all_integrationGroups();
            string integration_group;
            string integrationGroup_string;
            int integration_groups_length = integration_groups.Length;
            string integration_group_add_to_file;
            string standard_label;
            string dynamic_label;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_timeline;
            Ontology_type_enum standard_ontology;

            GraphPane[] new_timeline_graphPanes;
            List<GraphPane> timeline_graphPanes_list = new List<GraphPane>();

            for (int indexIG = 0; indexIG < integration_groups_length; indexIG++)
            {
                integration_group = integration_groups[indexIG];
                integrationGroup_string = Default_textBox_texts.Get_integrationGroup_string(integration_group);
                timeline_graphPanes_list.Clear();
                Set_integration_group_add_to_file_name_and_standard_label(integrationGroup_string, filteredFileName_addition, out integration_group_add_to_file, out standard_label, out dynamic_label);

                Update_progress_report(integrationGroup_string, task_string);

                standard_currentIntegration_group_enrichment_timeline = combined_standard_not_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                if (!Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps)
                {
                    standard_currentIntegration_group_enrichment_timeline.Set_significance_based_on_ranks_and_pvalue_after_calculation_of_fractional_rank(new int[] { 99999, 99999, 99999, 99999, 99999 }, Timeline_diagram.Options.Significance_pvalue_cutoff_copy);
                    standard_currentIntegration_group_enrichment_timeline.Identify_significant_predictions_and_keep_all_lines_with_these_SCPs_for_each_sample();
                }
                else
                {
                    string[] scps = combined_standard_filtered.Get_all_scps();
                    standard_currentIntegration_group_enrichment_timeline.Keep_only_input_scpNames(scps);
                }
                if ((standard_currentIntegration_group_enrichment_timeline.Enrich.Length > 0)
                    && (standard_currentIntegration_group_enrichment_timeline.Get_all_timepointsInDays().Length > 1))
                {
                    standard_currentIntegration_group_enrichment_timeline.Add_new_enrichment_lines_for_each_process_with_missing_completeSampleNames();
                    standard_currentIntegration_group_enrichment_timeline.Check_for_correctness();
                    standard_ontology = standard_currentIntegration_group_enrichment_timeline.Get_ontology_and_check_if_only_one_ontology();
                    new_timeline_graphPanes = Timeline_diagram.Generate_timelines_from_enrichment_results_save_as_images_and_return_graphPanes(standard_currentIntegration_group_enrichment_timeline, results_directory, standard_ontology + standard_label + integration_group_add_to_file, Form_default_settings);
                    timeline_graphPanes_list.AddRange(new_timeline_graphPanes);
                }
                integrationGroup_timeline_standard_graphPanes_dict.Add(integration_group, timeline_graphPanes_list.ToArray());
            }
            Timeline_diagram.Options.Significance_pvalue_cutoff_copy = -1;
        }

        private void Generate_scp_networks(Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, string filteredFileName_addition, string results_directory)
        {
            string task_string;
            string[] integration_groups = combined_standard_not_filtered.Get_all_integrationGroups();
            string integration_group;
            string integrationGroup_string;
            int integration_groups_length = integration_groups.Length;
            string integration_group_add_to_file;
            string standard_label;
            string dynamic_label;
            Ontology_enrichment_class dynamic_currentIntegration_group_enrichment_filtered;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_filtered;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_not_filtered;
            Ontology_type_enum standard_ontology = combined_standard_not_filtered.Get_ontology_and_check_if_only_one_ontology();
            Ontology_type_enum dynamic_ontology = combined_dynamic_filtered.Get_ontology_and_check_if_only_one_ontology();
            Enrichment_type_enum enrichment_type;
            bool network_generation_interrupted = false;
            for (int indexIG = 0; indexIG < integration_groups_length; indexIG++)
            {
                integration_group = integration_groups[indexIG];
                integrationGroup_string = Default_textBox_texts.Get_integrationGroup_string(integration_group);

                Set_integration_group_add_to_file_name_and_standard_label(integrationGroup_string, filteredFileName_addition, out integration_group_add_to_file, out standard_label, out dynamic_label);

                standard_currentIntegration_group_enrichment_filtered = combined_standard_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                standard_currentIntegration_group_enrichment_not_filtered = combined_standard_not_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                dynamic_currentIntegration_group_enrichment_filtered = combined_dynamic_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);

                string standard_nw_base_file_name = standard_ontology + integration_group_add_to_file + standard_label;
                task_string = "Generating and writing SCP-networks for standard enrichment analysis";
                Update_progress_report(integrationGroup_string, task_string);

                if (standard_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                {
                    enrichment_type = Enrichment_type_enum.Standard;
                    network_generation_interrupted = Mbco_network_integration.Generate_and_write_integrative_network_for_indicated_enrichment_results_of_each_integrationGroupName_only_defined_sets_and_return_if_interrupted(standard_currentIntegration_group_enrichment_filtered, standard_currentIntegration_group_enrichment_not_filtered, results_directory, standard_nw_base_file_name, enrichment_type, Progress_report_label, Form_default_settings);
                    if (network_generation_interrupted)
                    {
                        break;
                    }
                }

                string dynamic_nw_base_file_name = standard_ontology + integration_group_add_to_file + "_dynamic";
                task_string = "Generating and writing SCP-networks for dynamic enrichment analysis";
                #region Update progress report (copy paste)
                if (integrationGroup_string.Length > 0)
                { Progress_report_label.Text = integrationGroup_string + ":\r\n" + task_string; }
                else { Progress_report_label.Text = task_string; }
                Progress_report_label.Visible = true;
                Progress_report_label.Refresh();
                #endregion
                if ((combined_dynamic_filtered!=null)&&(combined_dynamic_filtered.Enrich.Length>0))
                {
                    enrichment_type = Enrichment_type_enum.Dynamic;
                    if (dynamic_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                    {
                        network_generation_interrupted = Mbco_network_integration.Generate_and_write_integrative_network_for_indicated_enrichment_results_of_each_integrationGroupName_only_defined_sets_and_return_if_interrupted(dynamic_currentIntegration_group_enrichment_filtered, standard_currentIntegration_group_enrichment_not_filtered, results_directory, dynamic_nw_base_file_name, enrichment_type, Progress_report_label, Form_default_settings);
                        if (network_generation_interrupted) { break; }
                    }
                }
            }
        }

        private void Write_parameter_spreadsheet_from_option_files(string results_input_directory)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string complete_fileName = results_input_directory + global_dirFile.Mbco_parameter_settings_fileName;
            Common_functions.ReadWrite.ReadWriteClass.Create_directory_if_it_does_not_exist(results_input_directory);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(complete_fileName);
            writer.WriteLine(global_dirFile.FirstLine_of_mbco_parameter_setting_fileName);
            Custom_data.Options.Write_option_entries(writer);
            Mbco_enrichment_pipeline.Options.Write_option_entries(writer);
            Mbco_network_integration.Options.Write_option_entries(writer);
            Bardiagram.Options.Write_option_entries(writer);
            Heatmap.Options.Write_option_entries(writer);
            Timeline_diagram.Options.Write_option_entries(writer);
            writer.Close();
        }
        private void Add_parameters_from_parameter_setting_lines_to_options_and_update_options_in_all_menu_panels(string[] parameter_setting_lines)
        {
            int parameter_settings_length = parameter_setting_lines.Length;
            string parameter_setting_line;
            string classTypeName;

            Mbco_enrichment_pipeline.Options.Clear_all_deNovo_dictionaries();

            for (int indexP=0; indexP<parameter_settings_length;indexP++)
            {
                parameter_setting_line = parameter_setting_lines[indexP];
                classTypeName = parameter_setting_line.Split(Global_class.Tab)[0];
                if (classTypeName.Equals(typeof(User_data_options_class).Name))
                { Custom_data.Options.Add_read_entry_to_options(parameter_setting_line); }
                else if (classTypeName.Equals(typeof(MBCO_enrichment_pipeline_options_class).Name))
                { Mbco_enrichment_pipeline.Options.Add_read_entry_to_options(parameter_setting_line); }
                else if (classTypeName.Equals(typeof(MBCO_network_based_integration_options_class).Name))
                { Mbco_network_integration.Options.Add_read_entry_to_options(parameter_setting_line); }
                else if (classTypeName.Equals(typeof(Bardiagram_options_class).Name))
                { Bardiagram.Options.Add_read_entry_to_options(parameter_setting_line); }
                else if (classTypeName.Equals(typeof(Heatmap_options_class).Name))
                { Heatmap.Options.Add_read_entry_to_options(parameter_setting_line); }
                else if (classTypeName.Equals(typeof(Timeline_options_class).Name))
                { Timeline_diagram.Options.Add_read_entry_to_options(parameter_setting_line); }
            }
            Mbco_enrichment_pipeline.Options.Update_scps_in_select_SCPs_interface = true;
            Custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
            Timeline_diagram.Options.Write_pdf = Bardiagram.Options.Write_pdf;
            Timeline_diagram.Options.Charts_per_page = Bardiagram.Options.Charts_per_page;
            Timeline_diagram.Options.ImageFormat = Bardiagram.Options.ImageFormat;
            Timeline_diagram.Options.Customized_colors = Bardiagram.Options.Customized_colors;
            Timeline_diagram.Options.Use_scp_abbreviations = Bardiagram.Options.Use_scp_abbreviations;
            Heatmap.Options.Write_pdf = Bardiagram.Options.Write_pdf;
            Heatmap.Options.Charts_per_page = Bardiagram.Options.Charts_per_page;
            Heatmap.Options.ImageFormat = Bardiagram.Options.ImageFormat;
            Heatmap.Options.Use_scp_abbreviations = Bardiagram.Options.Use_scp_abbreviations;
            Update_all_visualized_options_in_menu_panels();
        }

        private void Check_if_result_figure_options_agree()
        {
            if (!Bardiagram.Options.Write_pdf.Equals(Heatmap.Options.Write_pdf)) { throw new Exception(); }
            if (!Bardiagram.Options.Write_pdf.Equals(Timeline_diagram.Options.Write_pdf)) { throw new Exception(); }
            if (!Bardiagram.Options.Use_scp_abbreviations.Equals(Heatmap.Options.Use_scp_abbreviations)) { throw new Exception(); }
            if (!Bardiagram.Options.Use_scp_abbreviations.Equals(Timeline_diagram.Options.Use_scp_abbreviations)) { throw new Exception(); }
            if (!Bardiagram.Options.ImageFormat.Equals(Heatmap.Options.ImageFormat)) { throw new Exception(); }
            if (!Bardiagram.Options.ImageFormat.Equals(Timeline_diagram.Options.ImageFormat)) { throw new Exception(); }
            if (!Bardiagram.Options.Customized_colors.Equals(Timeline_diagram.Options.Customized_colors)) { throw new Exception(); }
            if (!Bardiagram.Options.Charts_per_page.Equals(Timeline_diagram.Options.Charts_per_page)) { throw new Exception(); }
            if (!Bardiagram.Options.Charts_per_page.Equals(Heatmap.Options.Charts_per_page)) { throw new Exception(); }
        }

        private void Generate_and_write_all_enrichment_results(Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, Ontology_enrichment_class combined_dynamic_not_filtered, string filtered_fileName_addition, string results_directory, ref bool light_up_results_button)
        {
            Write_enrichment_results_text_files(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory);
            Bardiagram.PDF_generation_allowed = true;
            Heatmap.PDF_generation_allowed = true;
            Timeline_diagram.PDF_generation_allowed = true;
            bool results_written = false;
            Dictionary<string, GraphPane[]> integrationGroup_standardGraphPanes;
            Dictionary<string, GraphPane[]> integrationGroup_dynamicGraphPanes;
            UserInterface_results.Set_results_directory_label(results_directory);
            Check_if_result_figure_options_agree();
            if (Bardiagram.Options.Generate_bardiagrams)
            {
                Generate_bardiagrams_and_return_standard_and_dynamic_graphPanes_for_eachIntegrationGroup(out integrationGroup_standardGraphPanes, out integrationGroup_dynamicGraphPanes, combined_standard_filtered, combined_dynamic_filtered, filtered_fileName_addition, results_directory);
                results_written = true;
                if (Results_addResultsToControl_cbButton.Checked)
                {
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Bardiagram_standard, integrationGroup_standardGraphPanes, ref light_up_results_button);
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Bardiagram_dynamic, integrationGroup_dynamicGraphPanes, ref light_up_results_button);
                }
                if (!Bardiagram.PDF_generation_allowed)
                {
                    Heatmap.PDF_generation_allowed = false;
                    Timeline_diagram.PDF_generation_allowed = false;
                }
            }
            if (Heatmap.Options.Generate_heatmap)
            { 
                Generate_heatmaps_and_return_standard_and_dynamic_graphPanes_for_eachIntegrationGroup(out integrationGroup_standardGraphPanes, out integrationGroup_dynamicGraphPanes, combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory);
                results_written = true;
                if (Results_addResultsToControl_cbButton.Checked)
                {
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Heatmap_standard, integrationGroup_standardGraphPanes, ref light_up_results_button);
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Heatmap_dynamic, integrationGroup_dynamicGraphPanes, ref light_up_results_button);
                }
                if (!Heatmap.PDF_generation_allowed)
                {
                    Bardiagram.PDF_generation_allowed = false;
                    Timeline_diagram.PDF_generation_allowed = false;
                }
            }
            if (Timeline_diagram.Options.Generate_timeline)
            { 
                Generate_timelines_and_return_standard_graphPanes_for_each_integrationGroup(out integrationGroup_standardGraphPanes, combined_standard_filtered, combined_standard_not_filtered, filtered_fileName_addition, results_directory);
                results_written = true;
                if (Results_addResultsToControl_cbButton.Checked)
                {
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Timeline_standard, integrationGroup_standardGraphPanes, ref light_up_results_button);
                }
                if (!Timeline_diagram.PDF_generation_allowed)
                {
                    Bardiagram.PDF_generation_allowed = false;
                    Heatmap.PDF_generation_allowed = false;
                }
            }
            if (results_written) 
            {
                Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                Bardiagram.Write_scp_abbreviations(results_directory, gdf.SCP_abbreviations_fileName); 
            }
            if (Mbco_network_integration.Options.Generate_scp_networks)
            { Generate_scp_networks(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, filtered_fileName_addition, results_directory); }
        }

        private void AnalyzeData_button_Click(object sender, EventArgs e)
        {
            AnalyzeData_button.BackColor = Form_default_settings.Color_button_pressed_back;
            AnalyzeData_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            AnalyzeData_button.Refresh();
            Custom_data.Check_for_correctness();
            int datasets_length = Custom_data.Get_all_unique_ordered_fixed_datasetIdentifies().Length;
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();

            ResultsDirectory_textBox.SilentText = global_dirFile.Transform_into_compatible_directory(ResultsDirectory_textBox.Text);
            if (datasets_length==0)
            {
                Progress_report_label.Visible = true;
                this.Progress_report_label.Text = "Please add, read or load dataset(s)";
                Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                this.Progress_report_label.Refresh();
                System.Threading.Thread.Sleep(1500);
                this.Progress_report_label.Text = "";
                Progress_report_label.Visible = false;
            }
            if (datasets_length > 0)
            {
                Progress_report_label.Visible = true;
                Progress_report_label.Text = "Checking, if all " + datasets_length + " datasets can be submitted to enrichment analysis";
                Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label,1);
                Progress_report_label.Refresh();
            }
            if (Custom_data.Analyse_if_data_can_be_submitted_to_enrichment_analysis(Timeline_diagram.Options.Generate_timeline_in_log_scale))
            {
                Progress_report_label.Visible = true;
                Progress_report_label.Text = "Preparing " + Custom_data.Get_all_unique_ordered_fixed_datasetIdentifies().Length + " datasets for enrichment analysis";
                Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                Progress_report_label.Refresh();
                Custom_data.Set_unique_datasetName_within_whole_custom_data_ignoring_integrationGroups();
                string results_directory = ResultsDirectory_textBox.Text;
                string fileName_addition = "";
                Ontology_enrichment_class standard_enrichment_unfiltered;
                Ontology_enrichment_class dynamic_enrichment_unfiltered;
                bool is_necessary_to_update_pipeline_for_all_runs = Mbco_enrichment_pipeline.Is_necessary_to_update_pipeline_for_all_runs();
                if (is_necessary_to_update_pipeline_for_all_runs)
                {
                    Progress_report_label.Visible = true;
                    Progress_report_label.Text = Ontology_classification_class.Get_loadAndPrepare_report_for_enrichment(Mbco_enrichment_pipeline.Options.Next_ontology);
                    Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                    Progress_report_label.Refresh();
                    Mbco_enrichment_pipeline.Generate_for_all_runs_after_setting_ontology_to_next_ontology(Progress_report_label, Form_default_settings);
                }
                Mbco_enrichment_pipeline.Remove_all_existing_custom_scps_from_mbco_association_unmodified_and_add_new_ones();
                if (   (!Mbco_network_integration.Options.Ontology.Equals(Mbco_enrichment_pipeline.Options.Ontology))
                    || (!Mbco_network_integration.Options.Ontology.Equals(Mbco_network_integration.Options.Next_ontology))
                    || (!Mbco_network_integration.Generated_for_all_runs))
                {
                    Progress_report_label.Visible = true;
                    Progress_report_label.Text = Ontology_classification_class.Get_loadAndPrepare_report_for_network(Mbco_enrichment_pipeline.Options.Next_ontology);
                    Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                    Progress_report_label.Refresh();
                    Mbco_network_integration.Generate_for_all_runs_after_resetting_ontology(Mbco_enrichment_pipeline.Options.Ontology, Progress_report_label, Form_default_settings);
                }
                if (!Mbco_network_integration.Options.Ontology.Equals(Mbco_enrichment_pipeline.Options.Ontology)) { throw new Exception(); }
                if (!Mbco_network_integration.Options.Ontology.Equals(Mbco_network_integration.Options.Next_ontology)) { throw new Exception(); }
                if (!Mbco_enrichment_pipeline.Options.Ontology.Equals(Mbco_enrichment_pipeline.Options.Next_ontology)) { throw new Exception(); }
                Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = Mbco_enrichment_pipeline.Options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level;

                string[] mbco_background_genes = Mbco_enrichment_pipeline.Get_all_symbols_of_any_SCPs_after_updating_instance_if_ontology_unequals_next_ontology(Progress_report_label, Form_default_settings,Global_class.Background_genes_scpName);

                string results_input_directory = results_directory + global_dirFile.Delimiter + "Input_data" + global_dirFile.Delimiter;
                Write_parameter_documentations(results_directory);
                Write_parameter_spreadsheet_from_option_files(results_input_directory);
                Custom_data_summary_class data_summary = new Custom_data_summary_class();
                data_summary.Generate_from_custom_data(Custom_data, mbco_background_genes);
                data_summary.Write(results_directory, "Summary_of_data_submitted_to_enrichment.txt", Progress_report_label, Form_default_settings);
                Custom_data.Write(results_input_directory, "Data_submitted_to_enrichment.txt", Progress_report_label, Form_default_settings);
                string[] datasets_with_missing_sign_genes_in_bgList = data_summary.Get_uniqueDatasetNames_plus_integrationGroup_with_no_signficant_genes_in_final_background_gene_list();
                if (data_summary.Get_number_of_significant_genes_in_final_background_gene_list_of_all_datasets() == 0)
                {
                    Progress_report_label.Visible = true;
                    Progress_report_label.Text = "No dataset contains significant genes in final background gene list. Enrichment analysis not possible.\r\nSee '.\\Input_data\\Summary_of_data_submitted_to_enrichment.txt' for details.";
                    Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                    Progress_report_label.Refresh();
                    System.Threading.Thread.Sleep(7000);
                }
                else
                {
                    if (datasets_with_missing_sign_genes_in_bgList.Length > 0)
                    {
                        Progress_report_label.Visible = true;
                        Progress_report_label.Text = datasets_with_missing_sign_genes_in_bgList.Length + " datasets have no significant genes that are part of final background gene list.\r\nSee '.\\Input_data\\Summary_of_data_submitted_to_enrichment.txt' for details.";
                        Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                        Progress_report_label.Refresh();
                        System.Threading.Thread.Sleep(7000);
                    }
                    Custom_data_class currentBgGeneListNames_custom_data;
                    string[] bgGeneListNames = Custom_data.Get_all_unique_ordered_bgGeneListNames();
                    string bgGeneListName;
                    int bgGeneListNames_length = bgGeneListNames.Length;
                    string[] current_bgGenes;
                    Data_class currentBgGenes_data;
                    Ontology_enrichment_class combined_standard_not_filtered = new Ontology_enrichment_class();
                    Ontology_enrichment_class combined_dynamic_not_filtered = new Ontology_enrichment_class();
                    Mbco_enrichment_pipeline.Options.Write_results = false;
                    Progress_report_label.Visible = true;
                    Progress_report_label.Text = "Analyzing " + Custom_data.Get_all_unique_ordered_fixed_datasetIdentifies().Length + " datasets for enrichment of " + Ontology_classification_class.Get_name_of_scps_for_progress_report(Mbco_enrichment_pipeline.Options.Ontology) + " genes";
                    Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 1);
                    Progress_report_label.Refresh();
                    for (int indexBG = 0; indexBG < bgGeneListNames_length; indexBG++)
                    {
                        bgGeneListName = bgGeneListNames[indexBG];
                        currentBgGeneListNames_custom_data = Custom_data.Get_custom_data_class_with_indicated_background_genes_list(bgGeneListName);
                        currentBgGeneListNames_custom_data.Keep_only_significant_lines();
                        current_bgGenes = currentBgGeneListNames_custom_data.ExpBgGenesList_bgGenes_dict[bgGeneListName];
                        Mbco_enrichment_pipeline.Reset_mbco_and_adjust_bg_genes_and_set_to_upperCase(current_bgGenes);
                        currentBgGenes_data = currentBgGeneListNames_custom_data.Generate_new_data_instance();
                        currentBgGenes_data.Set_all_ncbi_official_gene_symbols_to_upper_case();
                        Mbco_enrichment_pipeline.Analyse_data_instance_fast(currentBgGenes_data, results_directory, fileName_addition, out standard_enrichment_unfiltered, out dynamic_enrichment_unfiltered, Progress_report_label, Form_default_settings);
                        combined_standard_not_filtered.Add_other(standard_enrichment_unfiltered);
                        combined_dynamic_not_filtered.Add_other(dynamic_enrichment_unfiltered);
                    }
                    Custom_data.Reset_unique_datasetName();
                    Progress_report_label.Visible = false;

                    combined_dynamic_not_filtered.Check_for_correctness();
                    combined_standard_not_filtered.Check_for_correctness();

                    combined_standard_not_filtered.Set_significance_based_on_ranks_and_pvalue_after_calculation_of_fractional_rank(Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level, Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment);
                    combined_dynamic_not_filtered.Set_significance_based_on_ranks_and_pvalue_after_calculation_of_fractional_rank(Mbco_enrichment_pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level, Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment);

                    Ontology_enrichment_class combined_standard_filtered;
                    Ontology_enrichment_class combined_dynamic_filtered;
                    string filtered_fileName_addition = "";

                    #region Prepare results visualization 
                    UserInterface_results.Clear_all_enrichmentResults_graphPanes();
                    bool light_up_results_button = false;
                    #endregion

                    if (Mbco_enrichment_pipeline.MBCO_association.MBCO_associations.Length == 0)
                    {
                        Progress_report_label.Visible = true;
                        Progress_report_label.Text = "Selected ontology/SCPs do not contain genes, please change selection.";
                        Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 0);
                        Progress_report_label.Refresh();
                        System.Threading.Thread.Sleep(7000);
                    }
                    else
                    {
                        if (!Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps)
                        {
                            combined_standard_filtered = combined_standard_not_filtered.Deep_copy();
                            combined_dynamic_filtered = combined_dynamic_not_filtered.Deep_copy();
                            combined_standard_filtered.Keep_only_signficant_enrichment_lines_and_reset_uniqueDatasetNames();
                            combined_dynamic_filtered.Keep_only_signficant_enrichment_lines_and_reset_uniqueDatasetNames();
                            filtered_fileName_addition = "significantPredictions";
                            Generate_and_write_all_enrichment_results(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory, ref light_up_results_button);
                        }
                        else
                        {
                            string[] scpGroups = Mbco_enrichment_pipeline.Options.Group_selectedScps_dict.Keys.ToArray();
                            string scpGroup;
                            int scpGroups_length = scpGroups.Length;
                            string[] selected_scps;
                            for (int indexScpGroup = 0; indexScpGroup < scpGroups_length; indexScpGroup++)
                            {
                                scpGroup = scpGroups[indexScpGroup];
                                selected_scps = Mbco_enrichment_pipeline.Options.Group_selectedScps_dict[scpGroup];
                                combined_standard_filtered = combined_standard_not_filtered.Deep_copy();
                                combined_standard_filtered.Keep_only_input_scpNames(selected_scps);
                                combined_dynamic_filtered = new Ontology_enrichment_class();
                                filtered_fileName_addition = scpGroup;
                                Generate_and_write_all_enrichment_results(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory, ref light_up_results_button);
                            }
                        }
                        if (light_up_results_button)
                        {
                            Options_results_button.BackColor = Form_default_settings.Color_button_highlight_back;
                            Options_results_button.ForeColor = Form_default_settings.Color_button_highlight_fore;
                            Options_results_button.Refresh();
                            System.Threading.Thread.Sleep(1000);
                            Options_results_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                            Options_results_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
                            Options_results_button.Refresh();
                        }
                    }
                }
            }
            Progress_report_label.Text = "";
            Progress_report_label.Visible = false;
            Progress_report_label.Refresh();
            AnalyzeData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            AnalyzeData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            AnalyzeData_button.Refresh();
        }
        #endregion

        #region Enrichment options
        private void EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Keep_top_scps_standard_of_indicated_level(1, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Keep_top_scps_standard_of_indicated_level(2, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Keep_top_scps_standard_of_indicated_level(3, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Keep_top_scps_standard_of_indicated_level(4, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Keep_top_scps_dynamic_of_indicated_level(2, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Keep_top_scps_dynamic_of_indicated_level(3, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.DynamicTopPercentScpsLevel_x_SCPs_textBox_TextChanged(2, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.DynamicTopPercentScpsLevel_x_SCPs_textBox_TextChanged(3, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_standardPvalue_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Standard_pvalue(Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_dynamicPvalue_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Dynamic_pvalue(Mbco_enrichment_pipeline.Options);
        }
        private void ResetOptions_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline = UserInterface_enrichmentOptions.Reset_enrichment_options(Mbco_enrichment_pipeline, Bardiagram.Options, Heatmap.Options, Timeline_diagram.Options);
        }
        private void EnrichmentOptions_generateBardiagrams_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateBardiagrams_cbButton.Button_pressed();
            Bardiagram.Options = UserInterface_enrichmentOptions.GenerateBardiagram_ownCheckBox_CheckedChanged(Bardiagram.Options);
        }
        private void EnrichmentOptions_generateHeatmaps_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateHeatmaps_cbButton.Button_pressed();
            Heatmap.Options = UserInterface_enrichmentOptions.GenerateHeatmaps_ownCheckBox_CheckedChanged(Heatmap.Options);
        }
        private void EnrichmentOptions_generateHeatmapShowRanks_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateHeatmapShowRanks_cbButton.Button_pressed();
            Heatmap.Options = UserInterface_enrichmentOptions.GenerateHeatmapShowRanks_ownCheckBox_CheckedChanged(Heatmap.Options);
        }
        private void EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Button_pressed();
            Heatmap.Options = UserInterface_enrichmentOptions.GenerateHeatmapShowMinusLog10Pvalues_ownCheckBox_CheckedChanged(Heatmap.Options);
        }
        private void EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Button_pressed();
            Heatmap.Options = UserInterface_enrichmentOptions.GenerateHeatmapShowSignificantSCPsInAllDatasets_ownCheckBox_CheckedChanged(Heatmap.Options, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_generateTimeline_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateTimeline_cbButton.Button_pressed();
            int number_of_different_timepoints = Custom_data.Get_all_unique_ordered_timepointsInDays().Length;
            Timeline_diagram.Options = UserInterface_enrichmentOptions.GenerateTimeline_ownCheckBox_CheckedChanged(Timeline_diagram.Options, number_of_different_timepoints);
        }
        private void EnrichmentOptions_generateTimelinePvalue_textBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.GenerateTimeline_pvalue_textBox_TextChanged(Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_generateTimelineLogScale_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_generateTimelineLogScale_cbButton.Button_pressed();
            Timeline_diagram.Options = UserInterface_enrichmentOptions.GenerateTimelineLogScale_checkBox_CheckedChanged(Custom_data, Timeline_diagram.Options);
        }
        private void EnrichmentOptions_chartsPerPage_ownCheckBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bardiagram_options_class bardiagram_options = Bardiagram.Options;
            Heatmap_options_class heatmap_options = Heatmap.Options;
            Timeline_options_class timeline_options = Timeline_diagram.Options;
            UserInterface_enrichmentOptions.ChartsPerPage_ownCheckBox_SelectedIndexChanged(ref bardiagram_options, ref heatmap_options, ref timeline_options);
            Bardiagram.Options = bardiagram_options;
            Heatmap.Options = heatmap_options;
            Timeline_diagram.Options = timeline_options;
        }
        private void EnrichmentOptions_colorByLevel_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_colorByLevel_cbButton.Button_pressed();
            bool color_by_dataset = UserInterface_enrichmentOptions.ColorByLevel_ownCheckBox_CheckedChanged_and_return_if_colored_by_dataset();
            Bardiagram.Options.Customized_colors = color_by_dataset;
            Timeline_diagram.Options.Customized_colors = color_by_dataset;
        }
        private void EnrichmentOptions_colorByDatasetColor_cbButton_Click(object sender, EventArgs e)
        {
            EnrichmentOptions_colorByDatasetColor_cbButton.Button_pressed();
            bool color_by_dataset = UserInterface_enrichmentOptions.ColorByDataset_ownCheckBox_CheckedChanged_and_return_if_colored_by_dataset();
            Bardiagram.Options.Customized_colors = color_by_dataset;
            Timeline_diagram.Options.Customized_colors = color_by_dataset;
        }
        private void EnrichmentOptions_saveFiguresAs_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bardiagram_options_class bardiagram_options = Bardiagram.Options;
            Timeline_options_class timeline_options = Timeline_diagram.Options;
            Heatmap_options_class heatmap_options = Heatmap.Options;
            UserInterface_enrichmentOptions.SaveFiguresAs_ownListBox_SelectedIndexChanged(ref bardiagram_options, ref heatmap_options, ref timeline_options);
            Bardiagram.Options = bardiagram_options;
            Timeline_diagram.Options = timeline_options;
            Heatmap.Options = heatmap_options;
        }
        private void EnrichmentOptions_ontology_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Progress_report_label.Visible = true;
            Progress_report_label.Text = "Updating menu panels";
            Progress_report_label.Refresh();
            Ontology_type_enum selected_ontology = (Ontology_type_enum)EnrichmentOptions_ontology_ownListBox.SelectedItem;
            bool change_allowed = true;
            if (Ontology_classification_class.Is_go_ontology(selected_ontology))
            {
                Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                string go_association_fileName = gdf.Complete_human_go_association_2022_downloaded_fileName;
                string go_obo_nw_fileName = gdf.Complete_go_obo_fileName;
                if (!System.IO.File.Exists(go_association_fileName))
                {
                    change_allowed = false;
                    Progress_report_label.Visible = true;
                    Progress_report_label.Text = "Please download " + System.IO.Path.GetFileName(go_association_fileName) + @" from geneontology.org, unzip and copy into folder 'GO_datasets'";
                    Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 0);
                    Progress_report_label.Refresh();
                    System.Threading.Thread.Sleep(5000);
                }
                if (!System.IO.File.Exists(go_obo_nw_fileName))
                {
                    change_allowed = false;
                    Progress_report_label.Visible = true;
                    Progress_report_label.Text = "Please download " + System.IO.Path.GetFileName(go_obo_nw_fileName) + @" from geneontology.org and copy into folder 'GO_datasets'";
                    Form_default_settings.LabelProgressReport_set_sizes_and_fontSize(Progress_report_label, 0);
                    Progress_report_label.Refresh();
                    System.Threading.Thread.Sleep(5000);
                }
                Progress_report_label.Text = "";
            }
            if (change_allowed)
            {
                Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.Ontology_ownListBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options);
                Mbco_network_integration.Options.Next_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
                Update_acknoledgment_and_application_headline();
                UserInterface_selectSCPs.Update_mbco_parent_child_and_child_parent_networks_and_set_to_default(Mbco_enrichment_pipeline.Options, Progress_report_label);
                UserInterface_defineSCPs.Update_mbco_parent_child_and_child_parent_obo_networks_and_adjust_sortByList(Mbco_enrichment_pipeline.Options, Progress_report_label);
                Progress_report_label.Visible = false;
                Progress_report_label.Text = "";
            }
            else
            {
                EnrichmentOptions_ontology_ownListBox.SelectedIndex = EnrichmentOptions_ontology_ownListBox.Items.IndexOf(Mbco_network_integration.Options.Next_ontology);
            }
        }
        #endregion
        private void Set_visualization_in_datasetSummary_userInterface_and_organizeData_userInterface_to_optimum()
        {
            Dataset_attributes_enum[] attributes_with_different_entries = Custom_data.Get_all_attributes_with_different_entries();
            if (attributes_with_different_entries.Length == 0)
            {
                attributes_with_different_entries = new Dataset_attributes_enum[] { Dataset_attributes_enum.Name, Dataset_attributes_enum.Color };
            }
            List<Dataset_attributes_enum> attributes = new List<Dataset_attributes_enum>();
            attributes.Add(Dataset_attributes_enum.Delete);
            attributes.AddRange(attributes_with_different_entries);
            attributes_with_different_entries = attributes.Distinct().ToArray();
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(attributes_with_different_entries);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            UserInterface_organize_data.Unswitch_change_buttons();
        }

        #region DatasetSummary sort by 
        private void SortBy_sampleName_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.Name);
        }
        private void SortBy_timepoint_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.Timepoint);
        }
        private void SortBy_entryType_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.EntryType);
        }
        private void SortBy_integrationGroup_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.IntegrationGroup);
        }
        private void SortBy_sampleColor_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.Color);
        }
        private void SortBy_substring_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.Substring);
        }
        private void SortBy_bgGenesListName_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.BgGenes);
        }
        private void SortBy_sourceFileName_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.SourceFile);
        }
        private void SortBy_datasetOrderNo_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.SortBy_button_click(Dataset_attributes_enum.Dataset_order_no);
        }
        #endregion

        #region Update and reset changes
        private void React_to_changes_button_clicks()
        {
            if (Options_backgroundGenes_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                string[] mbco_background_genes = Mbco_enrichment_pipeline.Get_all_symbols_of_scp_names_after_updating_instance_if_ontology_unequals_next_ontology(Progress_report_label, Form_default_settings, Global_class.Background_genes_scpName);
                UserInterface_bgGenes.Analyze_if_all_genes_are_part_of_selected_background_gene_lists(Custom_data, mbco_background_genes);
            }
        }
        private void Changes_update_button_Click(object sender, EventArgs e)
        {
            Changes_update_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Changes_update_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Changes_update_button.Refresh();
            Custom_data = DatasetSummary_userInterface.Changes_update_button_click(Custom_data);
            Analyze_if_custom_data_compatible_with_specified_options_and_adopt_options_if_not();
            React_to_changes_button_clicks();
            Changes_update_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Changes_update_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Changes_update_button.Refresh();
        }

        private void Changes_reset_button_Click(object sender, EventArgs e)
        {
            Changes_reset_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Changes_reset_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Changes_reset_button.Refresh();
            DatasetSummary_userInterface.Changes_reset_button_click();
            React_to_changes_button_clicks();
            Changes_reset_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Changes_reset_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Changes_reset_button.Refresh();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }

        private void Analyze_if_custom_data_compatible_with_specified_options_and_adopt_options_if_not()
        {
            if (!Custom_data.Analyse_if_all_timepoints_larger_zero())
            {
                Timeline_diagram.Options = UserInterface_enrichmentOptions.Set_timeline_log_scale_to_false_and_update_corresponding_checkbox(Timeline_diagram.Options);
            }
        }

        private void AddNewDataset_button_Click(object sender, EventArgs e)
        {
            char[] delimiters = new[] { '\r', '\n' }; 
            string[] inputGenes = Input_geneList_textBox.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string inputGene;
            List<string> add_inputGenes = new List<string>();
            int inputGenes_length = inputGenes.Length;
            for (int indexInput = 0; indexInput < inputGenes_length; indexInput++)
            {
                inputGene = inputGenes[indexInput];
                inputGene = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(inputGene);
                if ((!String.IsNullOrEmpty(inputGene))
                    && (!Default_textBox_texts.Is_among_inputGene_list_textBox_texts(inputGene)))
                {
                    add_inputGenes.Add(inputGene.ToUpper());
                }
            }
            if (add_inputGenes.Count == 0)
            {
                Input_geneList_textBox.Text = Default_textBox_texts.InputGene_list_textBox_noGenes;
            }
            else
            {
                bool successful;
                Custom_data = DatasetSummary_userInterface.AddNewDataset_button_click_and_return_success(Custom_data, add_inputGenes.Distinct().ToArray(), out successful);
                if (successful) 
                { 
                    Input_geneList_textBox.Text = Default_textBox_texts.InputGene_list_textBox_default;
                    Analyze_if_custom_data_compatible_with_specified_options_and_adopt_options_if_not();
                }
            }
        }
        #endregion

        #region Dataset event callers
        private void Dataset_sampleName_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_color_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_timeline_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_timeline_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_entryType_ownCheckBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_integrationGroup_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_bgGenes_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_datsetOrderNo_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_sourceFile_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        #endregion

        #region Delete checkBoxes checked
        private void Dataset_all_delete_cbButton_Click(object sender, EventArgs e)
        {
            Dataset_all_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.All_delete_button_click();
        }
        private void Dataset_delete_myCheckBoxButton_00_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[0].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_01_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[1].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_02_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[2].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_03_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[3].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_04_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[4].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_05_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[5].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_06_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[6].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_07_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[7].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_08_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[8].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_09_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[9].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_10_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[10].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_11_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[11].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_12_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[12].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_13_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[13].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_14_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[14].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_15_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[15].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_16_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[16].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        private void Dataset_delete_myCheckBoxButton_17_clicked(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.UserInterface_lines[17].Dataset_delete_cbButton.Button_pressed();
            DatasetSummary_userInterface.Copy_userInterfaceLines_to_userInterfaceDatasetSummaries_and_update_graphical_interface_exclduing_dataset_visibilities_and_xlocations();
        }
        #endregion

        private void ClearCustomData_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();
            Custom_data.Clear_custom_data();
            UserInterface_read.Set_to_default();
            UserInterface_bgGenes.Set_to_default(Custom_data);
            Dataset_attributes_enum[] new_attributes = new Dataset_attributes_enum[] { Dataset_attributes_enum.Delete, Dataset_attributes_enum.Name, Dataset_attributes_enum.Color };
            DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(new Dataset_attributes_enum[0]);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            DatasetSummary_userInterface.Reset_empty_interface_lines_to_default(0);
            DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(new_attributes);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(new_attributes);
            Set_options_buttons_and_panel_visibilities_to_default();
            Custom_data = UserInterface_bgGenes.Reset_bgGenesLists_to_default(Custom_data);
        }
        private void Dataset_scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            DatasetSummary_userInterface.Dataset_scrollBar_Scroll();
        }

        #region Options buttons
        private void Set_options_buttons_and_panel_visibilities_to_default()
        {
            Options_readData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_readData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_scpNetworks_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_scpNetworks_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_enrichment_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_enrichment_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_backgroundGenes_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_backgroundGenes_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_organizeData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_organizeData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_dataSignificance_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_dataSignificance_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_enrichment_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_enrichment_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_exampleData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_exampleData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_defineSCPs_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_defineSCPs_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_selectSCPs_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_selectSCPs_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_tips_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_tips_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Options_results_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Options_results_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;

            EnrichmentOptions_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            EnrichmentOptions_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            OrganizeData_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            OrganizeData_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            ScpNetworks_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            ScpNetworks_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;

            Options_scpNetworks_panel.Visible = false;
            Options_scpNetworks_panel.Visible = false;
            Options_enrichment_panel.Visible = false;
            Options_organizeData_panel.Visible = false;
            Options_dataSignificance_panel.Visible = false;
            Options_selectSCPs_panel.Visible = false;
            Options_defineScps_panel.Visible = false;
            Options_results_panel.Visible = false;
            Read_directoryOrFile_ownTextBox.Visible = false;
            Read_directoryOrFile_label.Visible = false;
            UserInterface_read.Set_visibility(false);
            UserInterface_scp_networks.Set_visibility(false, Mbco_network_integration.Options);
            UserInterface_enrichmentOptions.Set_visibility(false, Mbco_enrichment_pipeline.Options, Bardiagram.Options, Heatmap.Options, Timeline_diagram.Options);
            UserInterface_loadExamples.Set_visibility(false);
            Options_bgGenes_panel.Visible = false;
            Options_tips_panel.Visible = false;
            DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();

            ResultsDirectory_label.Visible = true;
            ResultsDirectory_textBox.Visible = true;
            MBCO_reference_label.Visible = true;
        }
        private void Set_dataset_interface_to_selected_attributes_in_organizeData_panel()
        {
            Dataset_attributes_enum[] selected_attributes = UserInterface_organize_data.Get_dataset_attributes_from_showCheckBoxes();
            selected_attributes = DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(selected_attributes);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(selected_attributes);
        }
        private void Options_readData_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_readData_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Options_readData_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_readData_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                UserInterface_read.Set_visibility(true);
            }
        }
        private void Options_enrichment_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_enrichment_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Options_enrichment_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_enrichment_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                UserInterface_enrichmentOptions.Set_visibility(true, Mbco_enrichment_pipeline.Options, Bardiagram.Options, Heatmap.Options, Timeline_diagram.Options);
            }
        }
        private void Options_selectSCPs_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_selectSCPs_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Options_selectSCPs_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_selectSCPs_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                bool update_scp_window = Mbco_enrichment_pipeline.Options.Update_scps_in_select_SCPs_interface;
                UserInterface_selectSCPs.Set_to_visible(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label, update_scp_window);
                Mbco_enrichment_pipeline.Options.Update_scps_in_select_SCPs_interface = false;
            }
        }
        private void Options_scpNetworks_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_scpNetworks_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Options_scpNetworks_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_scpNetworks_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                Options_scpNetworks_panel.Visible = true;
                UserInterface_scp_networks.Set_visibility(true, Mbco_network_integration.Options);
            }
        }
        private void Options_backgroundGenes_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_backgroundGenes_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Options_backgroundGenes_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_backgroundGenes_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                Options_bgGenes_panel.Visible = true;
                UserInterface_bgGenes.Set_to_visible(Custom_data);
                Dataset_attributes_enum[] attributes_for_bgGenes_selection = UserInterface_bgGenes.BgGenesInterface_dataset_attributes;
                DatasetSummary_userInterface.Set_to_filter_mode(attributes_for_bgGenes_selection);
            }
        }
        private void Options_organizeData_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_organizeData_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Options_organizeData_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_organizeData_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                Options_organizeData_panel.Visible = true;
                Dataset_attributes_enum[] visible_panels = DatasetSummary_userInterface.Get_dataset_attributes_defining_visible_panels();
                UserInterface_organize_data.Set_to_visible(visible_panels);
            }
        }
        private void Options_dataSignificance_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_dataSignificance_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Options_dataSignificance_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_dataSignificance_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                Options_dataSignificance_panel.Visible = true;
                UserInterface_sigData.Set_to_visible(Custom_data.Options);
                Dataset_attributes_enum[] visible_panels = Custom_data.Get_all_attributes_with_different_entries();
                List<Dataset_attributes_enum> visible_panels_list = new List<Dataset_attributes_enum>();
                visible_panels_list.Add(Dataset_attributes_enum.Name);
                if (visible_panels.Contains(Dataset_attributes_enum.Timepoint))
                { visible_panels_list.Add(Dataset_attributes_enum.Timepoint); }
                if (visible_panels.Contains(Dataset_attributes_enum.EntryType))
                { visible_panels_list.Add(Dataset_attributes_enum.EntryType); }
                visible_panels_list.Add(Dataset_attributes_enum.Genes_count);
                visible_panels = visible_panels_list.Distinct().ToArray();
                DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(visible_panels);
                DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            }
        }
        private void Options_defineSCPs_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_defineSCPs_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Options_defineSCPs_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_defineSCPs_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                UserInterface_defineSCPs.Set_to_visible(Mbco_enrichment_pipeline.Options, Progress_report_label);
            }
        }
        private void Options_exampleData_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_exampleData_button .BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Options_exampleData_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_exampleData_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                UserInterface_loadExamples.Set_visibility(true);
                Dataset_attributes_enum[] visible_panels = DatasetSummary_userInterface.Get_dataset_attributes_defining_visible_panels();
                UserInterface_organize_data.Set_to_visible(visible_panels);
            }
        }
        private void Options_tips_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_tips_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Options_tips_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_tips_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                Options_tips_panel.Visible = true;
            }
        }
        private void Options_results_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_results_button.BackColor;
            Set_options_buttons_and_panel_visibilities_to_default();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Options_results_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Options_results_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                UserInterface_results.Set_visibility(true);
                Results_visualization_panel.Visible = true;
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = false;
                ResultsDirectory_label.Visible = false;
                ResultsDirectory_textBox.Visible = false;
                MBCO_reference_label.Visible = false;
            }
        }
        #endregion

        #region Read userInterface buttons
        private void Read_setToMBCO1_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_exampleData1_button_clicked();
        }
        private void Read_setToMBCO2_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_exampleData2_button_clicked();
        }
        private void Read_setToMBCO_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_mbco_button_clicked();
        }
        private void Read_setToSeurat_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_Seurat_button_clicked();
        }
        private void Read_setToMinimum_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_minimum_button_clicked();
        }
        private void Read_setToOptimum_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_optimum_button_clicked();
        }
        private void Read_sampleNameColumn_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_timepointColumn_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_timeunitColumn_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_geneSymbol_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_valueColumn_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_value2ndColumn_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_entryType_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_integrationGroupColumn_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.ColumName_specification_changed();
        }
        private void Read_order_allFilesInDirectory_cbButton_Click(object sender, EventArgs e)
        {
            Read_order_allFilesInDirectory_cbButton.Button_switch_to_positive();
            UserInterface_read.Read_allFilesInDirectory_ownCheckedBox_clicked();
        }
        private void Read_order_onlySpecifiedFile_cbButton_Click(object sender, EventArgs e)
        {
            Read_order_onlySpecifiedFile_cbButton.Button_switch_to_positive();
            UserInterface_read.Read_onlySpecifiedFile_ownCheckedBox_clicked();
        }
        private void Read_error_reports_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (Read_error_reports_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                Read_error_reports_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                Read_error_reports_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
            else
            {
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                Read_error_reports_button.BackColor = Form_default_settings.Color_button_pressed_back;
                Read_error_reports_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            }
        }
        private void Read_error_reports_maxErrorsPerFile_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.Error_reports_maxErrorsPerFile_ownTextBox_TextChanged();
        }
        private void ReadDataset_button_Click(object sender, EventArgs e)
        {
            Read_error_reports_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Read_error_reports_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Read_readDataset_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Read_readDataset_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Read_readDataset_button.Refresh();
            Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
            string defaultIntegrationGroup = DatasetSummary_userInterface.Get_default_integrationGroup_from_lastSaved_datasetSummaries();
            string[] parameter_setting_lines;
            this.Custom_data = UserInterface_read.Read_button_click(this.Custom_data, defaultIntegrationGroup, selectable_colors, out parameter_setting_lines, Progress_report_label);
            if (parameter_setting_lines.Length>0) 
            { 
                Add_parameters_from_parameter_setting_lines_to_options_and_update_options_in_all_menu_panels(parameter_setting_lines);
                Check_if_result_figure_options_agree();
            }
            Analyze_if_custom_data_compatible_with_specified_options_and_adopt_options_if_not();
            DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            Set_main_panel_visibilities_to_default();
            Set_visualization_in_datasetSummary_userInterface_and_organizeData_userInterface_to_optimum();
            Read_readDataset_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Read_readDataset_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Read_readDataset_button.Refresh();
        }
        #endregion

        #region ScpNetworks interface buttons and boxes
        private void ScpNetworks_standardGroupSameLevelSCPs_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardGroupSameLevelSCPs_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardParentChild_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardParentChild_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardAddGenes_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardAddGenes_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardConnectRelated_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardConnectRelated_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicAddGenes_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicAddGenes_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicParentChild_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicParentChild_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicGroupSameLevelSCPs_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicConnectAllRelated_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicConnectAllRelated_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_nodeSizeByColorCount_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_nodeSizeByColorCount_cbButton.Button_switch_to_positive();
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSize_byColorsCount_changed(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_nodeSizeByDatasetCount_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_nodeSizeByDatasetCount_cbButton.Button_switch_to_positive();
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSize_byDatasetsCount_changed(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_nodeSizeFixed_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_nodeSizeFixed_cbButton.Button_switch_to_positive();
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSize_fixed_changed(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_generateNetworks_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_generateNetworks_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_default_button_Click(object sender, EventArgs e)
        {
            Mbco_network_integration.Replace_options_by_default_options_and_reset_generated_for_all_runs();
            UserInterface_scp_networks.Copy_options_into_interface_selections(Mbco_network_integration.Options);
        }
        private void ScpNetworks_standardTopLevel_2_interactions_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardTopLevel_3_interactions_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicGroupSameLevelSCPs_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardGroupSameLevelSCPs_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        #endregion

        #region OrganizeData show panel
        private void OrganizeData_showAnyCheckBox_CheckedChanged()
        {
            Dataset_attributes_enum[] dataset_attributes = UserInterface_organize_data.Show_name_ownCheckBox_CheckedChanged();
            Dataset_attributes_enum[] dataset_attributes_shown = DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(dataset_attributes);
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(dataset_attributes_shown);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
        }
        private void OrganizeData_showName_cbButton_Click_1(object sender, EventArgs e)
        {
            this.OrganizeData_showName_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showIntegrationGroup_cbButton_Click_1(object sender, EventArgs e)
        {
            this.OrganizeData_showIntegrationGroup_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showColor_cbButton_Click_1(object sender, EventArgs e)
        {
            this.OrganizeData_showColor_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showTimepoint_cbButton_Click_1(object sender, EventArgs e)
        {
            this.OrganizeData_showTimepoint_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showEntryType_cbButton_Click_1(object sender, EventArgs e)
        {
            this.OrganizeData_showEntryType_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showDatasetOrderNo_cbButton_Click(object sender, EventArgs e)
        {
            OrganizeData_showDatasetOrderNo_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showSourceFile_cbButton_Click_1(object sender, EventArgs e)
        {
            OrganizeData_showSourceFile_cbButton.Button_pressed();
            OrganizeData_showAnyCheckBox_CheckedChanged();
        }
        private void OrganizeData_showDifferentEntries_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] attributes = Custom_data.Get_all_attributes_with_different_entries();
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(attributes);
            List<Dataset_attributes_enum> attributes_list = new List<Dataset_attributes_enum>();
            attributes_list.Add(Dataset_attributes_enum.Delete);
            attributes_list.AddRange(attributes);
            attributes = attributes_list.ToArray();
            DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(attributes);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
        }
        private void OrganizeData_modifySourceFileName_cbButton_Click(object sender, EventArgs e)
        {
            OrganizeData_modifySourceFileName_cbButton.Button_pressed();
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.SourceFile);
        }
        private void OrganizeData_modifyName_cbButton_Click(object sender, EventArgs e)
        {
            OrganizeData_modifyName_cbButton.Button_pressed();
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.Name);
        }
        private void OrganizeData_modifySubstring_cbButton_Click(object sender, EventArgs e)
        {
            OrganizeData_modifySubstring_cbButton.Button_pressed();
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.Substring);
        }
        private void OrganizeData_modifyTimepoint_cbButton_Click(object sender, EventArgs e)
        {
            OrganizeData_modifyTimepoint_cbButton.Button_pressed();
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.Timepoint);
        }
        private void OrganizeData_modifyEntryType_cbButton_Click(object sender, EventArgs e)
        {
            OrganizeData_modifyEntryType_cbButton.Button_pressed();
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.EntryType);
        }
        #endregion

        #region OrganizeData addFileName to dataset name
        private void OrganizeData_addFileNames_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Update_removeFileName_label();
        }

        private void OrganizeData_addFileNamesbefore_button_Click(object sender, EventArgs e)
        {
            if (UserInterface_organize_data.Changes_allowed())
            {
                string selectedAddItem = UserInterface_organize_data.AddFileName_listBox.SelectedItem.ToString();
                DatasetSummary_userInterface.Add_or_remove_fileName_to_or_from_dataset_name_in_userInterface_dataSummaries_and_update_userInterfaceLines(Add_fileName_instructions_enum.Before_dataset_name, selectedAddItem);
            }
        }
        private void OrganizeData_addFileNameAfter_button_Click(object sender, EventArgs e)
        {
            if (UserInterface_organize_data.Changes_allowed())
            {
                string selectedAddItem = UserInterface_organize_data.AddFileName_listBox.SelectedItem.ToString();
                DatasetSummary_userInterface.Add_or_remove_fileName_to_or_from_dataset_name_in_userInterface_dataSummaries_and_update_userInterfaceLines(Add_fileName_instructions_enum.After_dataset_name, selectedAddItem);
            }
        }
        private void OrganizeData_addFileNameRemove_button_Click(object sender, EventArgs e)
        {
            if (UserInterface_organize_data.Changes_allowed())
            {
                string selectedAddItem = UserInterface_organize_data.AddFileName_listBox.SelectedItem.ToString();
                DatasetSummary_userInterface.Add_or_remove_fileName_to_or_from_dataset_name_in_userInterface_dataSummaries_and_update_userInterfaceLines(Add_fileName_instructions_enum.Remove, selectedAddItem);
            }
        }
        #endregion

        #region OrganizeData modify
        private void OrganizeData_modifyName_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.Name);
        }
        private void OrganizeData_modifyTimepoint_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.Timepoint);
        }
        private void OrganizeData_modifyEntryType_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.EntryType);
        }
        private void OrganizeData_modifySourceFileName_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.SourceFile);
        }
        private void OrganizeData_modifySubstring_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.Substring);
        }
        private void OrganizeData_modifyIndexLeft_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_indexLeft_ownTextBox_TextChanged();
        }
        private void OrganizeData_modifyIndexRight_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_indexRight_ownTextBox_TextChanged();
        }
        private void OrganizeData_modifyIntegrationGroup_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_organize_data.Modify_checkedBoxes_checkedChanged(Dataset_attributes_enum.IntegrationGroup);
        }
        private void OrganizeData_respond_to_any_change_button_click(Dataset_attributes_enum[] attributes_to_filter_data, string delimiter_string, int[] indexesLeft_oneBased, int[] indexesRight_oneBased)
        {
            if (attributes_to_filter_data.Length > 0)
            {
                if (attributes_to_filter_data.Contains(Dataset_attributes_enum.Substring))
                {
                    DatasetSummary_userInterface.Set_substrings_in_lastSavedDataSummaries_and_copy_into_userInterface_dataSummaries(delimiter_string, indexesLeft_oneBased, indexesRight_oneBased);
                }
                DatasetSummary_userInterface.Set_to_filter_mode(attributes_to_filter_data);
            }
            else
            {
                DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();
                Set_visualization_in_datasetSummary_userInterface_and_organizeData_userInterface_to_optimum();
            }
        }
        private void OrganizeData_changeIntegrationGroup_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum current_attribute = Dataset_attributes_enum.IntegrationGroup;
            if (UserInterface_organize_data.Changes_allowed(current_attribute))
            {
                string delimiter_string;
                int[] indexesLeft_oneBased;
                int[] indexesRight_oneBased;
                Dataset_attributes_enum[] attributes_to_filter_data = UserInterface_organize_data.ChangeIntegrationGroup_button_Click(out delimiter_string, out indexesLeft_oneBased, out indexesRight_oneBased);
                OrganizeData_respond_to_any_change_button_click(attributes_to_filter_data, delimiter_string, indexesLeft_oneBased, indexesRight_oneBased);
            }
        }
        private void OrganizeData_changeColor_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum current_attribute = Dataset_attributes_enum.Color;
            if (UserInterface_organize_data.Changes_allowed(current_attribute))
            {
                string delimiter_string;
                int[] indexesLeft_oneBased;
                int[] indexesRight_oneBased;
                Dataset_attributes_enum[] attributes_to_filter_data = UserInterface_organize_data.ChangeColor_button_Click(out delimiter_string, out indexesLeft_oneBased, out indexesRight_oneBased);
                OrganizeData_respond_to_any_change_button_click(attributes_to_filter_data, delimiter_string, indexesLeft_oneBased, indexesRight_oneBased);
            }
        }
        private void OrganizeData_changeDelete_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum current_attribute = Dataset_attributes_enum.Delete;
            if (UserInterface_organize_data.Changes_allowed(current_attribute))
            {
                string delimiter_string;
                int[] indexesLeft_oneBased;
                int[] indexesRight_oneBased;
                Dataset_attributes_enum[] attributes_to_filter_data = UserInterface_organize_data.ChangeDelete_button_Click(out delimiter_string, out indexesLeft_oneBased, out indexesRight_oneBased);
                OrganizeData_respond_to_any_change_button_click(attributes_to_filter_data, delimiter_string, indexesLeft_oneBased, indexesRight_oneBased);
            }
        }
        #endregion

        #region OrganizeData convert timepoints
        private void OrganizeData_convertTimeunites_convert_button_Click(object sender, EventArgs e)
        {
            this.Custom_data = UserInterface_organize_data.ConvertTimeunites_convert_button_Click(this.Custom_data);
            DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(this.Custom_data);
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        #endregion

        #region OrganizeData automatic
        private void OrganizeData_automaticIntegrationGroups_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum attributes_to_be_ignored = Dataset_attributes_enum.IntegrationGroup;
            if (UserInterface_organize_data.Changes_allowed(attributes_to_be_ignored))
            {
                DatasetSummary_userInterface.Automatically_set_integrationGroups_of_different_userInterfaceDatasetSummaries_to_different_integrationGroups();
            }
        }
        private void OrganizeData_automaticColors_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum attributes_to_be_ignored = Dataset_attributes_enum.Color;
            if (UserInterface_organize_data.Changes_allowed(attributes_to_be_ignored))
            {
                DatasetSummary_userInterface.Automatically_set_colors_of_different_userInterfaceDatasetSummaries_to_different_colors();
            }
        }
        private void OrganizeData_automaticDatasetOrder_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum attributes_to_be_ignored = Dataset_attributes_enum.Dataset_order_no;
            if (UserInterface_organize_data.Changes_allowed(attributes_to_be_ignored))
            {
                DatasetSummary_userInterface.Automatically_set_resultOrders_within_integrationGroups_following_current_order_of_userInterfaceDatasetSummaries();
            }
        }
        #endregion

        #region Background genes
        private void BgGenes_add_button_Click(object sender, EventArgs e)
        {
            UserInterface_bgGenes.Add_background_genes(this.Custom_data);
            DatasetSummary_userInterface.Update_available_bgGenesListNames_in_userInterfaceLines(this.Custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray());
        }
        private void BgGenes_OrganizeDeleteSelection_button_Click(object sender, EventArgs e)
        {
            UserInterface_bgGenes.Remove_selected_bgGenesList(Custom_data);
            DatasetSummary_userInterface.Update_available_bgGenesListNames_in_userInterfaceLines(this.Custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray());
        }
        private void BgGenes_OrganizeDeleteAll_button_Click(object sender, EventArgs e)
        {
            UserInterface_bgGenes.Reset_bgGenesLists_to_default(Custom_data);
            DatasetSummary_userInterface.Update_available_bgGenesListNames_in_userInterfaceLines(this.Custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray());
        }
        private void BgGenes_assignmentsReset_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Set_all_background_genes_in_userInterface_dataSummaries_to_input_and_update_interface_lines(Global_class.Mbco_exp_background_gene_list_name);
        }
        private void BgGenes_assignmentsAutomatic_button_Click(object sender, EventArgs e)
        {
            string[] potential_background_geneLists = Custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray();
            DatasetSummary_userInterface.Set_all_background_genes_to_matching_lists_based_on_fileNames_in_userInterface_dataSummaries_to_input_and_update_interface_lines(potential_background_geneLists);
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
        }
        private void BgGenes_addReadOnlyFile_cbButton_Click(object sender, EventArgs e)
        {
            BgGenes_addReadOnlyFile_cbButton.Button_switch_to_positive();
            UserInterface_bgGenes.Add_readOnlyFile_ownCheckBox_checked();
        }
        private void BgGenes_addReadAllFilesInDirectory_cbButton_Click(object sender, EventArgs e)
        {
            BgGenes_addReadAllFilesInDirectory_cbButton.Button_switch_to_positive();
            UserInterface_bgGenes.Add_readAllFilesInDirectory_ownCheckBox_checked();
        }
        private void BgGenes_addRead_button_internal()
        {
            Custom_data = UserInterface_bgGenes.Add_read_button_pressed(Custom_data, Progress_report_label);
            DatasetSummary_userInterface.Update_available_bgGenesListNames_in_userInterfaceLines(this.Custom_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray());
        }
        private void BgGenes_AddRead_button_Click(object sender, EventArgs e)
        {
            BgGenes_addRead_button_internal();
        }
        private void BgGenes_AddShowErrors_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (BgGenes_AddShowErrors_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                BgGenes_AddShowErrors_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                BgGenes_AddShowErrors_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
            else
            {
                UserInterface_bgGenes.Read_error_reports_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                BgGenes_AddShowErrors_button.BackColor = Form_default_settings.Color_button_pressed_back;
                BgGenes_AddShowErrors_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                BgGenes_warnings_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                BgGenes_warnings_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
        }
        private void BgGenes_warnings_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (BgGenes_warnings_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                BgGenes_warnings_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                BgGenes_warnings_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
            else
            {
                UserInterface_bgGenes.Warnings_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                BgGenes_warnings_button.BackColor = Form_default_settings.Color_button_pressed_back;
                BgGenes_warnings_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
                BgGenes_AddShowErrors_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                BgGenes_AddShowErrors_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
        }
        #endregion

        private void EnrichmentOptions_explanation_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (EnrichmentOptions_explanation_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                EnrichmentOptions_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                EnrichmentOptions_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
            else
            {
                UserInterface_enrichmentOptions.Explanation_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                EnrichmentOptions_explanation_button.BackColor = Form_default_settings.Color_button_pressed_back;
                EnrichmentOptions_explanation_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            }
        }

        private void OrganizeData_explanation_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (OrganizeData_explanation_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                OrganizeData_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                OrganizeData_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
            else
            {
                UserInterface_organize_data.Explanation_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                OrganizeData_explanation_button.BackColor = Form_default_settings.Color_button_pressed_back;
                OrganizeData_explanation_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            }
        }

        private void ScpNetworks_explanation_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (OrganizeData_explanation_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back))
            {
                ScpNetworks_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                ScpNetworks_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            }
            else
            {
                UserInterface_scp_networks.Explanation_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                ScpNetworks_explanation_button.BackColor = Form_default_settings.Color_button_pressed_back;
                ScpNetworks_explanation_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            }
        }

        private void Update_all_visualized_options_in_menu_panels()
        {
            UserInterface_enrichmentOptions.Update_allOptions(Mbco_enrichment_pipeline.Options, Bardiagram.Options, Heatmap.Options, Timeline_diagram.Options);
            UserInterface_sigData.Set_visualized_options_to_custom_data_options_and_update_boxes(Custom_data.Options);
            UserInterface_scp_networks.Copy_options_into_interface_selections(Mbco_network_integration.Options);
        }

        #region LoadExamples
        private void LoadExamples_load_button_Click(object sender, EventArgs e)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            LoadExamples_load_button.BackColor = Form_default_settings.Color_button_pressed_back;
            LoadExamples_load_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            LoadExamples_load_button.Refresh();
            bool read_data_and_update_dataset_interface = false;
            if (UserInterface_loadExamples.Nog_cbButton.Checked)
            {
                this.Read_directoryOrFile_ownTextBox.SilentText = global_dirFile.Get_custom_data_nog_directory();
                this.ResultsDirectory_textBox.SilentText = global_dirFile.Get_custom_results_nog_directory();
                UserInterface_read.SetTo_exampleData2_button_clicked();

                Custom_data.Options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Value_1st_cutoff = (float)Math.Log(1.3F, 2);
                Custom_data.Options.Value_2nd_cutoff = -(float)Math.Log10(0.05);
                Custom_data.Options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
                Custom_data.Options.Keep_top_ranks = 99999;
                Custom_data.Options.Merge_upDown_before_ranking = false;
                Custom_data.Options.All_genes_significant = true;

                Mbco_enrichment_pipeline.Options.Next_ontology = Ontology_type_enum.Mbco_mouse;
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level = new int[] { -1, 5, 5, 10, 5 };
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level = new int[] { -1, -1, 3, 5, -1 };
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment = 0.05F;
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment = 0.05F;
                Mbco_enrichment_pipeline.Options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps = false;
                Mbco_enrichment_pipeline.Options.Group_selectedScps_dict = new Dictionary<string, string[]>();
                Mbco_enrichment_pipeline.Options.OwnScp_mbcoSubScps_dict = new Dictionary<string, string[]>();
                Mbco_enrichment_pipeline.Options.OwnScp_level_dict = new Dictionary<string, int>();
                Mbco_enrichment_pipeline.Options.OwnScp_mbcoSubScps_dict.Add("Structural organization of mitochondria", new string[] { "Mitochondrial gene expression", "Mitochondrial dynamics", "Mitochondrial energy production", "Mitochondrial protein import machinery", "Post-translational protein modification in Mitochondria" });
                Mbco_enrichment_pipeline.Options.OwnScp_level_dict.Add("Structural organization of mitochondria", 1);
                Mbco_enrichment_pipeline.Options.Update_scps_in_select_SCPs_interface = true;
                Mbco_enrichment_pipeline.Options.Timeline_pvalue_cutoff = 0.05F;

                Timeline_diagram.Options.Generate_timeline = true;
                Timeline_diagram.Options.Customized_colors = false;
                Timeline_diagram.Options.Is_logarithmic_time_axis = false;

                Bardiagram.Options.Generate_bardiagrams = true;
                Bardiagram.Options.Customized_colors = Timeline_diagram.Options.Customized_colors;

                Heatmap.Options.Generate_heatmap = false;
                Heatmap.Options.Value_type_selected_for_visualization = Enrichment_value_type_enum.Fractional_rank;
                Heatmap.Options.Show_significant_scps_over_all_conditions = true;

                Mbco_network_integration.Options.Next_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
                Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks = true;
                Mbco_network_integration.Options.Add_genes_to_standard_networks = false;
                Mbco_network_integration.Options.Add_edges_that_connect_standard_scps = false;
                Mbco_network_integration.Options.Add_additional_edges_that_connect_dynamic_scps = false;
                Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_network_integration.Options.Add_parent_child_relationships_to_dynamic_SCP_networks = false;
                Mbco_network_integration.Options.Add_genes_to_dynamic_networks = false;
                Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.Standard;
                Mbco_network_integration.Options.Generate_scp_networks = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_dynamic_enrichment = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_standard_enrichment = false;

                read_data_and_update_dataset_interface = true;

            }
            else if (UserInterface_loadExamples.Kpmp_cbButton.Checked)
            {
                this.Read_directoryOrFile_ownTextBox.SilentText = global_dirFile.Get_custom_data_kpmp_directory();
                this.ResultsDirectory_textBox.SilentText = global_dirFile.Get_custom_results_kpmp_directory();
                UserInterface_read.SetTo_exampleData1_button_clicked();

                Custom_data.Options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Value_1st_cutoff = 0;
                Custom_data.Options.Value_2nd_cutoff = -(float)Math.Log10(0.05);
                Custom_data.Options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
                Custom_data.Options.Keep_top_ranks = 300;
                Custom_data.Options.Merge_upDown_before_ranking = false;
                Custom_data.Options.All_genes_significant = true;

                Mbco_enrichment_pipeline.Options.Next_ontology = Ontology_type_enum.Mbco_human;
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level = new int[] { -1, 5, 5, 10, 5 };
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level = new int[] { -1, -1, 5, 7, -1 };
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment = 1F;
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment = 1F;
                Mbco_enrichment_pipeline.Options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps = false;
                Mbco_enrichment_pipeline.Options.Group_selectedScps_dict = new Dictionary<string, string[]>();
                Mbco_enrichment_pipeline.Options.OwnScp_mbcoSubScps_dict = new Dictionary<string, string[]>();
                Mbco_enrichment_pipeline.Options.OwnScp_level_dict = new Dictionary<string, int>();
                Mbco_enrichment_pipeline.Options.Timeline_pvalue_cutoff = 0.05F;

                Timeline_diagram.Options.Generate_timeline = false;
                Timeline_diagram.Options.Customized_colors = true;

                Bardiagram.Options.Generate_bardiagrams = false;
                Bardiagram.Options.Customized_colors = Timeline_diagram.Options.Customized_colors;

                Heatmap.Options.Generate_heatmap = false;
                Heatmap.Options.Value_type_selected_for_visualization = Enrichment_value_type_enum.Fractional_rank;
                Heatmap.Options.Show_significant_scps_over_all_conditions = true;

                Mbco_network_integration.Options.Next_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
                Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks = true;
                Mbco_network_integration.Options.Add_genes_to_standard_networks = false;
                Mbco_network_integration.Options.Add_edges_that_connect_standard_scps = false;
                Mbco_network_integration.Options.Add_additional_edges_that_connect_dynamic_scps = true;
                Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_network_integration.Options.Add_parent_child_relationships_to_dynamic_SCP_networks = false;
                Mbco_network_integration.Options.Add_genes_to_dynamic_networks = false;
                Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.No_of_different_colors;
                Mbco_network_integration.Options.Generate_scp_networks = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_dynamic_enrichment = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_standard_enrichment = false;

                read_data_and_update_dataset_interface = true;
            }
            if (read_data_and_update_dataset_interface)
            {
                Update_all_visualized_options_in_menu_panels();
                Update_acknoledgment_and_application_headline();

                Read_order_allFilesInDirectory_cbButton.Checked = true;
                Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
                string defaultIntegrationGroup = DatasetSummary_userInterface.Get_default_integrationGroup_from_lastSaved_datasetSummaries();
                string[] parameter_settings;
                this.Custom_data = UserInterface_read.Read_button_click(this.Custom_data, defaultIntegrationGroup, selectable_colors, out parameter_settings, Progress_report_label);
                BgGenes_addReadAllFilesInDirectory_cbButton.Checked = true;
                //this.Custom_data = UserInterface_bgGenes.Add_read_button_pressed(Custom_data, Progress_report_label, Default_textBox_texts.Get_progress_report_x_position(0));
                //this.Custom_data.Automatically_override_bgGeneListNames_if_matching_names();

                string[] mbco_background_genes = Mbco_enrichment_pipeline.Get_all_symbols_of_scp_names_after_updating_instance_if_ontology_unequals_next_ontology(Progress_report_label, Form_default_settings, Global_class.Background_genes_scpName);
                UserInterface_bgGenes.Analyze_if_all_genes_are_part_of_selected_background_gene_lists(Custom_data, mbco_background_genes);

                DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(new Dataset_attributes_enum[] { });
                DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
                DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
                DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
                Dataset_attributes_enum[] shown_attributes = Custom_data.Get_all_attributes_with_different_entries();
                List<Dataset_attributes_enum> shown_attributes_list = new List<Dataset_attributes_enum>();
                shown_attributes_list.AddRange(shown_attributes);
                shown_attributes_list.Add(Dataset_attributes_enum.Delete);
                shown_attributes_list.Add(Dataset_attributes_enum.Name);
                Dataset_attributes_enum[] final_selection = DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(shown_attributes_list.Distinct().ToArray());
                DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
                UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(final_selection);
                Analyze_if_custom_data_compatible_with_specified_options_and_adopt_options_if_not();
                Check_if_result_figure_options_agree();
            }
            LoadExamples_load_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            LoadExamples_load_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            LoadExamples_load_button.Refresh();

            //this.Dataset_all_delete_cbButton.Checked = true;
            //this.Dataset_delete_myCheckBoxButton_00_clicked(new object(), new EventArgs());
            //this.Changes_update_button_Click(new object(), new EventArgs());
            //this.Bardiagram.Options.Generate_bardiagrams = false;
            //this.Timeline_diagram.Options.Generate_timeline = false;
            //this.Heatmap.Options.Generate_heatmap = true;
            //this.AnalyzeData_button_Click(new object(), new EventArgs());

        }
        private void LoadExamples_NOG_cbButton_Click(object sender, EventArgs e)
        {
            LoadExamples_NOG_cbButton.Button_pressed();
            UserInterface_loadExamples.NOG_checkBox_CheckedChanged();
        }
        private void LoadExamples_KPMPreference_cbButton_Click(object sender, EventArgs e)
        {
            LoadExamples_KPMPreference_cbButton.Button_pressed();
            UserInterface_loadExamples.KPMPreference_checkBox_CheckedChanged();
        }
        #endregion

        #region SigData
        private void SigData_value1st_higherSig_cbButton_Click(object sender, EventArgs e)
        {
            SigData_value1st_higherSig_cbButton.Button_switch_to_positive();
            UserInterface_sigData.Value1st_higherSig_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_value1st_lowerSig_cbButton_Click(object sender, EventArgs e)
        {
            SigData_value1st_lowerSig_cbButton.Button_switch_to_positive();
            UserInterface_sigData.Value1st_lowerSig_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_value2nd_higherSig_cbButton_Click(object sender, EventArgs e)
        {
            SigData_value2nd_higherSig_cbButton.Button_switch_to_positive();
            UserInterface_sigData.Value2nd_higherSig_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_value2nd_lowerSig_cbButton_Click(object sender, EventArgs e)
        {
            SigData_value2nd_lowerSig_cbButton.Button_switch_to_positive();
            UserInterface_sigData.Value2nd_lowerSig_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_value1st_cutoff_textBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.Value1st_cutoff_textBox_TextChanged(Custom_data.Options);
        }
        private void SigData_value2nd_cutoff_textBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.Value2nd_cutoff_textBox_TextChanged(Custom_data.Options);
        }

        private void SigData_rankBy_1stValue_cbButton_Click(object sender, EventArgs e)
        {
            SigData_rankBy_1stValue_cbButton.Button_switch_to_positive();
            UserInterface_sigData.RankBy_1stValue_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_rankBy_2ndValue_cbButton_Click(object sender, EventArgs e)
        {
            SigData_rankBy_2ndValue_cbButton.Button_switch_to_positive();
            UserInterface_sigData.RankBy_2ndValue_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_keep_mergeUpDown_cbButton_Click(object sender, EventArgs e)
        {
            SigData_keep_mergeUpDown_cbButton.Button_switch_to_positive();
            UserInterface_sigData.Keep_mergeUpDown_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_keep_eachDataset_cbButton_Click(object sender, EventArgs e)
        {
            SigData_keep_eachDataset_cbButton.Button_switch_to_positive();
            UserInterface_sigData.Keep_eachDataset_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_keepTopRanks_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.KeepTopRanks_ownTextBox_TextChanged(Custom_data.Options);
        }
        private void SigData_deleteNotSignGenes_cbButton_Click(object sender, EventArgs e)
        {
            SigData_deleteNotSignGenes_cbButton.Button_pressed();
            UserInterface_sigData.DeleteNotSignGenes_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_deleteNotSignGenes_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.DeleteNotSignGenes_ownCheckBox_CheckedChanged(Custom_data.Options);
        }
        private void SigData_resetSig_button_Click(object sender, EventArgs e)
        {
            SigData_resetSig_button.BackColor = Form_default_settings.Color_button_pressed_back;
            SigData_resetSig_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            SigData_resetSig_button.Refresh();
            if (Custom_data.Analyse_if_data_can_be_submitted_to_enrichment_analysis(Timeline_diagram.Options.Generate_timeline_in_log_scale))
            {
                Custom_data = UserInterface_sigData.ResetSig_button_Click(Custom_data);
                DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
                UserInterface_sigData.Set_visualized_options_to_custom_data_options_and_update_boxes(Custom_data.Options);
            }
            SigData_resetSig_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            SigData_resetSig_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            SigData_resetSig_button.Refresh();
        }
        private void SigData_resetParameter_button_Click(object sender, EventArgs e)
        {
            SigData_resetParameter_button.BackColor = Form_default_settings.Color_button_pressed_back;
            SigData_resetParameter_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            SigData_resetParameter_button.Refresh();
            UserInterface_sigData.ResetParameter_button_Click(Custom_data.Options);
            SigData_resetParameter_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            SigData_resetParameter_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            SigData_resetParameter_button.Refresh();
        }
        private void SigData_allGenesSignificant_cbButton_Click(object sender, EventArgs e)
        {
            SigData_allGenesSignificant_cbButton.Button_pressed();
            UserInterface_sigData.AllGeneSignificant_ownCheckBox_CheckedChanged(Custom_data.Options);
        }

        #endregion

        #region Select SCPs
        private void SelectSCPs_addGroup_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Add_groupButton_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label);
        }
        private void SelectSCPs_removeGroup_button_Click(object sender, EventArgs e)
        {
            bool addGenes_to_standard_network = Mbco_network_integration.Options.Add_genes_to_standard_networks;
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Remove_groupButton_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, ref addGenes_to_standard_network, Progress_report_label);
            Mbco_network_integration.Options.Add_genes_to_standard_networks = addGenes_to_standard_network;
        }
        private void SelectSCPs_groups_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.Groups_ownListBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label);
        }
        private void SelectSCPs_add_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Add_button_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label);
        }
        private void SelectSCPs_remove_button_Click(object sender, EventArgs e)
        {
            bool addGenes_to_standard_network = Mbco_network_integration.Options.Add_genes_to_standard_networks;
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Remove_button_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, ref addGenes_to_standard_network, Progress_report_label);
            Mbco_network_integration.Options.Add_genes_to_standard_networks = addGenes_to_standard_network;
        }
        private void SelectSCPs_sortSCPs_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.SortSCPs_listBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label);
        }
        private void SelectSCPs_includeAncestorSCPs_cbButton_Click(object sender, EventArgs e)
        {
            SelectSCPs_includeAncestorSCPs_cbButton.Button_pressed();
        }
        private void SelectSCPs_includeOffspringSCPs_cbButton_Click(object sender, EventArgs e)
        {
            SelectSCPs_includeOffspringSCPs_cbButton.Button_pressed();
        }
        private void SelectScps_mbcoSCPs_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.MBCO_listBox_changed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label);
        }
        private void SelectScps_selectedSCPs_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.Selected_listBox_changed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, Progress_report_label);
        }
        private void SelectSCPs_showOnlySelectedScps_cbButton_Click(object sender, EventArgs e)
        {
            SelectSCPs_showOnlySelectedScps_cbButton.Button_pressed();
            bool add_genes_to_network;
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.ShowOnlySelectedScps_checkBox_CheckedChanged(Mbco_enrichment_pipeline.Options, out add_genes_to_network);
            if (Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps)
            {
                Heatmap.Options.Show_significant_scps_over_all_conditions = true;
            }
            Mbco_network_integration.Options.Add_genes_to_standard_networks = add_genes_to_network;
        }
        private void SelectSCPs_addGenes_cbButton_Click(object sender, EventArgs e)
        {
            SelectSCPs_addGenes_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_selectSCPs.AddGenes_checkBox_CheckedChanged(Mbco_network_integration.Options);
        }
        private void SelectedSCPs_writeMbcoHierarchy_button_Click(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.Write_mbco_yed_network(Progress_report_label);
        }
        #endregion

        #region Define scps
        private void DefineScps_sort_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.Sort_listBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Progress_report_label);
        }
        private void DefineScps_addNewOwnSCP_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.AddNewOwnSCP_button_Click(Mbco_enrichment_pipeline.Options, Progress_report_label);
        }
        private void DefineScps_removeOwnSCP_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.RemoveOwnSCP_button_Click(Mbco_enrichment_pipeline.Options, Progress_report_label);
        }
        private void DefineScps_selectOwnScp_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.SelectOwnScp_ownListBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Progress_report_label);
        }
        private void DefineScps_level1_cbButton_Click(object sender, EventArgs e)
        {
            DefineScps_level1_cbButton.Button_switch_to_positive();
            UserInterface_defineSCPs.SelectedOwnScp_level_checkBox_changed(Mbco_enrichment_pipeline.Options, 1);
        }
        private void DefineScps_level2_cbButton_Click(object sender, EventArgs e)
        {
            DefineScps_level2_cbButton.Button_switch_to_positive();
            UserInterface_defineSCPs.SelectedOwnScp_level_checkBox_changed(Mbco_enrichment_pipeline.Options, 2);
        }
        private void DefineScps_level3_cbButton_Click(object sender, EventArgs e)
        {
            DefineScps_level3_cbButton.Button_switch_to_positive();
            UserInterface_defineSCPs.SelectedOwnScp_level_checkBox_changed(Mbco_enrichment_pipeline.Options, 3);
        }
        private void DefineScps_level4_cbButton_Click(object sender, EventArgs e)
        {
            DefineScps_level4_cbButton.Button_switch_to_positive();
            UserInterface_defineSCPs.SelectedOwnScp_level_checkBox_changed(Mbco_enrichment_pipeline.Options, 4);
        }
        private void DefineScps_mbcoSCP_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.MBCO_listBox_changed();
        }
        private void DefineScps_addSubScp_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.AddSubScp_button_Click(Mbco_enrichment_pipeline.Options, Progress_report_label);
        }
        private void DefineScps_removeSubScp_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.RemoveSubScp_button_Click(Mbco_enrichment_pipeline.Options, Progress_report_label);
        }
        private void DefineSCPs_writeMbcoHierarchy_button_Click(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.Write_mbco_yed_network_and_return_if_interrupted(Progress_report_label);
        }
        private void DefineScps_ownSubScps_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.OwnSubScps_listBox_changed();
        }
        #endregion

        #region Results
        private void Results_integrationGroup_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_results.IntegrationGroup_listBox_selection_changed();

        }
        private void Results_bardiagram_standard_cbButton_Click(object sender, EventArgs e)
        {
            Results_bardiagram_standard_cbButton.Button_switch_to_positive();
            UserInterface_results.Enrichment_results_checkBox_button_pressed(Enrichment_results_enum.Bardiagram_standard);
        }
        private void Results_bardiagram_dynamic_cbButton_Click(object sender, EventArgs e)
        {
            Results_bardiagram_dynamic_cbButton.Button_switch_to_positive();
            UserInterface_results.Enrichment_results_checkBox_button_pressed(Enrichment_results_enum.Bardiagram_dynamic);
        }
        private void Results_timeline_cbButton_Click(object sender, EventArgs e)
        {
            Results_timeline_cbButton.Button_switch_to_positive();
            UserInterface_results.Enrichment_results_checkBox_button_pressed(Enrichment_results_enum.Timeline_standard);
        }
        private void Results_heatmap_standard_cbButton_Click(object sender, EventArgs e)
        {
            Results_heatmap_standard_cbButton.Button_switch_to_positive();
            UserInterface_results.Enrichment_results_checkBox_button_pressed(Enrichment_results_enum.Heatmap_standard);
        }
        private void Results_heatmap_dynamic_cbButton_Click(object sender, EventArgs e)
        {
            Results_heatmap_dynamic_cbButton.Button_switch_to_positive();
            UserInterface_results.Enrichment_results_checkBox_button_pressed(Enrichment_results_enum.Heatmap_dynamic);
        }
        private void Results_next_button_Click(object sender, EventArgs e)
        {
            UserInterface_results.Next_button_pressed();
        }
        private void Results_previous_button_Click(object sender, EventArgs e)
        {
            UserInterface_results.Previous_button_pressed();
        }

        #endregion

        #region Application size
        private void AppSize_resize_button_Click(object sender, EventArgs e)
        {
            //this.Form_default_settings.
            this.Form_default_settings.Update_parameter();
            //this.AppSize_panel.Visible = false;
            //this.Options_bgGenes_panel.Visible = false;
            //this.Options_defineScps_panel.Visible = false;
            //this.Options_enrichment_panel.Visible = false;
            //this.Options_loadExamples_panel.Visible = false;
            //this.Options_organizeData_panel.Visible = false;
            //this.Options_readData_panel.Visible = false;
            //this.Options_scpNetworks_panel.Visible = false;
            //this.Options_selectSCPs_panel.Visible = false;
            //this.Options_tips_panel.Visible=false;
            //this.Options_loadExamples_panel.Visible = false;
            //this.DatasetInterface_overall_panel.Visible = false;

            this.Visible = false;
            this.Update_overallApplicationSize();
            this.Form_default_settings.Update_all_graphic_elements_in_applicationSize_panel();
            this.Form_default_settings.Update_applicationSize_textBoxes_and_listBoxes();
            this.UserInterface_read.Update_all_graphic_elements();
            this.UserInterface_sigData.Update_all_graphic_elements(Custom_data.Options);
            this.UserInterface_organize_data.Update_all_graphic_elements();
            this.UserInterface_enrichmentOptions.Update_all_graphic_elements(Mbco_enrichment_pipeline.Options);
            this.UserInterface_bgGenes.Update_all_graphic_elements(Custom_data);
            this.UserInterface_defineSCPs.Update_all_graphic_elements();
            this.UserInterface_scp_networks.Update_graphic_elements(Mbco_network_integration.Options);
            this.UserInterface_selectSCPs.Update_all_graphic_elements(Progress_report_label);
            this.UserInterface_loadExamples.Update_all_graphics_elements();
            this.UserInterface_tips.Update_all_graphic_elements();
            this.DatasetSummary_userInterface.Update_all_graphic_elements(Custom_data);
            this.UserInterface_results.Update_all_graphic_elements();
            this.Update_all_option_menu_buttons();
            this.Update_all_graphic_elements_of_shared_tools();
            this.Visible = true;
        }
        private void AppSize_width_textBox_TextChanged(object sender, EventArgs e)
        {
            Form_default_settings.Update_applicationSize_textBoxes_and_listBoxes();
        }
        private void AppSize_colorTheme_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form_default_settings.Update_applicationSize_textBoxes_and_listBoxes();
        }
        private void AppSize_height_textBox_TextChanged(object sender, EventArgs e)
        {
            Form_default_settings.Update_applicationSize_textBoxes_and_listBoxes();
        }
        private void AppSize_increase_button_Click(object sender, EventArgs e)
        {
            Form_default_settings.Increase_heightWidth_by_fixed_number();
        }
        private void AppSize_decrease_button_Click(object sender, EventArgs e)
        {
            Form_default_settings.Decrease_heightWidth_by_fixed_number();
        }
        #endregion

        private void Tips_demonstration_cbButton_Click(object sender, EventArgs e)
        {
            Tips_demonstration_cbButton.Button_pressed();
            UserInterface_tips.Update_demonstration_cbLabel();
        }

        private void Results_addResultsToControl_cbButton_Click(object sender, EventArgs e)
        {
            Results_addResultsToControl_cbButton.Button_pressed();
            UserInterface_results.Set_visibility_of_checkBoxes_and_labels();
        }

    }

    class Form1_shared_text_class
    {
        public const string Sort_alphabetically_text = "alphabetically";
        public const string Sort_byLevel_text = "by level";
        public const string Sort_byLevelParentScp_text = "by parent SCPs";
    }

}
