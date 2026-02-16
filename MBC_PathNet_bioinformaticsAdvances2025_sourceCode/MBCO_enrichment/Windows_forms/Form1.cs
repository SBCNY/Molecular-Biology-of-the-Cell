//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a Apache 2.0 license.

//Please acknowledge MBC PathNet in your publications by citing the following reference:
//MBC PathNet: integration and visualization of networks connecting functionally related pathways predicted from transcriptomic and proteomic datasets
//Jens Hansen, Ravi Iyengar. Bioinform Adv. 2025 Aug 18; 5(1):vbaf197. PMID: 40917650. PMCID: PMC12413227. DOI:10.1093/bioadv/vbaf197

//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

//Last update: 2026 February 12


using ClassLibrary1.BgGenes_userInterface;
using ClassLibrary1.Dataset_userInterface;
using ClassLibrary1.DefineSCPs_userInterface;
using ClassLibrary1.EnrichmentOptions_userInterface;
using ClassLibrary1.LoadExamples_userInterface;
using ClassLibrary1.OrganizeData_userInterface;
using ClassLibrary1.Read_interface;
using ClassLibrary1.Results_userInterface;
using ClassLibrary1.ScpNetworks_userInterface;
using ClassLibrary1.Select_scps_userInterface;
using ClassLibrary1.SigData_userInterface;
using ClassLibrary1.Tips_userInterface;
using ClassLibrary1.Ontology_userInterface;
using Common_functions.Array_own;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using Common_functions.Text;
using Data;
using Enrichment;
using Network;
using Result_visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Windows_forms;
using Windows_forms_customized_tools;
using yed_network;
using ZedGraph;
using System.IO;
using Common_functions.ReadWrite;
using System.Data.SqlClient;
using System.Net;
using Leave_out;
using ClassLibrary1.Windows_forms;

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
        private Ontology_interface_class UserInterface_ontology { get; set; }
        private EnrichmentOptions_userInterface_class UserInterface_enrichmentOptions { get; set; }
        private BgGenes_userInterface_class UserInterface_bgGenes { get; set; }
        private DefineSCPs_userInterface_class UserInterface_defineSCPs { get; set; }
        private Select_scps_userInterface_class UserInterface_selectSCPs { get; set; }
        private LoadExamples_userInterface_class UserInterface_loadExamples { get; set; }
        private Tips_userInterface_class UserInterface_tips { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }
        private ProgressReport_interface_class ProgressReport { get; set; }
        private ExplanationErrorReport_userInterface UserInterface_explantionErrorReport { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private Key_mouseButton_inputWaiter_class Key_MouseButton_InputWaiter { get; set; }
        private readonly ManualResetEvent mre = new ManualResetEvent(false);

        public Mbco_user_application_form(bool visible)
        {
            this.Visible = false;
            InitializeComponent();
            Initialize_form_settings_object();

            Report_panel.Location = new Point(DatasetInterface_overall_panel.Location.X, DatasetInterface_overall_panel.Location.Y);

            //Report_ownTextBox.Enabled= false;

            Set_main_panel_visibilities_to_default();

            Custom_data = new Custom_data_class();
            Mbco_enrichment_pipeline = new Mbc_enrichment_fast_pipeline_class(Ontology_type_enum.Mbco, Organism_enum.Homo_sapiens);
            Mbco_network_integration = new Mbc_network_based_integration_class(Mbco_enrichment_pipeline.Options.Next_ontology, Mbco_enrichment_pipeline.Options.Next_organism);
            ProgressReport = new ProgressReport_interface_class(ProgressReport_myPanelTextBox, Form_default_settings);
            Bardiagram = new Bardiagram_class(ProgressReport);
            Timeline_diagram = new Timeline_class(ProgressReport);
            Heatmap = new Heatmap_class(ProgressReport);
            Key_MouseButton_InputWaiter = new Key_mouseButton_inputWaiter_class();
            UserInterface_tutorial = new Tutorial_interface_class(Tutorial_myPanelTextBox, ProgressReport, Key_MouseButton_InputWaiter, Form_default_settings);
            UserInterface_explantionErrorReport = new ExplanationErrorReport_userInterface(Read_error_reports_maxErrorsPerFile_ownTextBox,
                                                                                           ProgressReport);

            Initialize_and_reset_userInterface_enrichmentOptions();
            Initialize_and_reset_userInterface_dataSig();
            Initialize_and_reset_read_userInterface();
            Generate_results_directory_replace_and_refresh();

            MBCO_obo_network_class mbco_obo_parent_child_network_for_level_and_depth = new MBCO_obo_network_class(this.Mbco_enrichment_pipeline.Options.Next_ontology, SCP_hierarchy_interaction_type_enum.Parent_child, this.Mbco_enrichment_pipeline.Options.Next_organism);
            mbco_obo_parent_child_network_for_level_and_depth.Generate_by_reading_safed_spreadsheet_file_or_obo_file_add_missing_scps_if_custom_add_human_processSizes_and_return_if_not_interrupted(ProgressReport, out bool not_interrupted);

            Reset_gene_list_text_box();
            Initialize_and_reset_userInterface_ontology(mbco_obo_parent_child_network_for_level_and_depth);
            Initialize_and_reset_userInterface_scpNetworks();
            Initialize_and_reset_userInterface_organizeData();
            Initialize_and_reset_userInterface_bgGenes();
            Initialize_and_reset_userInterface_loadExamples();
            Initialize_and_reset_userInterface_selectSCPs(mbco_obo_parent_child_network_for_level_and_depth);
            Initialize_and_reset_userInterface_defineSCPs(mbco_obo_parent_child_network_for_level_and_depth);
            Initialize_and_reset_userInterface_tips(mbco_obo_parent_child_network_for_level_and_depth);
            Initialize_and_reset_userInterface_results();

            Initialize_and_reset_datasetSummary_userInterface();

            Update_graphic_parameter_overallApplicationSize_and_adjust_all_graphic_elements();

            //Update_all_option_menu_buttons();
            //Update_all_graphic_elements_of_shared_tools();

            Set_button_colors_to_unpressed();

            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            Set_tab_order();

            if (Global_class.Do_internal_checks)
            {
                ProgressReport_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh("Do internal checks is set to true", Form_default_settings);
            }
            else
            {
                ProgressReport_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh("For an introduction, use the 'Tour'- and 'Mini Tour'-buttons in the lower-right corner.", Form_default_settings);
            }
            this.Refresh();
            Form_default_settings.Higlight_button(Options_quickTour_button);
            Form_default_settings.Higlight_button(Options_tour_button);
            this.Visible = visible;
        }
        public Mbco_user_application_form() : this(true)
        { }

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
            Read_delimiter_ownListBox.TabIndex = current_index++;
            Read_setToCustom1_button.TabIndex = current_index++;
            Read_setToCustom2_button.TabIndex = current_index++;
            Read_setToSingleCell_button.TabIndex = current_index++;
            Read_setToMBCO_button.TabIndex = current_index++;
            Read_setToMinimum_button.TabIndex = current_index++;
            Read_setToOptimum_button.TabIndex = current_index++;
            Read_order_allFilesInDirectory_cbButton.TabIndex = current_index++;
            Read_order_onlySpecifiedFile_cbButton.TabIndex = current_index++;
            Read_error_reports_button.TabIndex = current_index++;


            SigData_directionValue1st_ownListBox.TabIndex = current_index++;
            SigData_directionValue2nd_ownListBox.TabIndex = current_index++;
            SigData_value1st_cutoff_ownTextBox.TabIndex = current_index++;
            SigData_value2nd_cutoff_ownTextBox.TabIndex = current_index++;
            SigData_rankByValue_ownListBox.TabIndex = current_index++;
            SigData_defineDataset_ownListBox.TabIndex = current_index++;
            SigData_keepTopRankedGenes_ownTextBox.TabIndex = current_index++;
            SigData_deleteNotSignGenes_cbButton.TabIndex = current_index++;
            SigData_allGenesSignificant_cbButton.TabIndex = current_index++;
            SigData_resetSig_button.TabIndex = current_index++;
            SigData_resetParameter_button.TabIndex = current_index++;

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
            EnrichmentOptions_GO_sizeMin_ownTextBox.TabIndex = current_index++;
            EnrichmentOptions_GO_sizeMax_ownTextBox.TabIndex = current_index++;
            EnrichmentOptions_default_button.TabIndex = current_index++;
            EnrichmentOptions_generateBardiagrams_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmaps_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmapShowRanks_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateTimeline_cbButton.TabIndex = current_index++;
            EnrichmentOptions_generateTimelinePvalue_textBox.TabIndex= current_index++;
            EnrichmentOptions_explanation_button.TabIndex = current_index++;

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

            ScpNetworks_nodeSizes_determinant_ownListBox.TabIndex = current_index++;
            ScpNetworks_nodeSizes_maxDiameter_ownTextBox.TabIndex = current_index++;
            ScpNetworks_nodeLabel_minSize_ownTextBox.TabIndex = current_index++;
            ScpNetworks_nodeLabel_maxSize_ownTextBox.TabIndex = current_index++;
            ScpNetworks_nodeLabel_uniqueSize_ownTextBox.TabIndex = current_index++;
            ScpNetworks_nodeSizes_scaling_ownListBox.TabIndex = current_index++;
            ScpNetworks_default_button.TabIndex = current_index++;
            ScpNetworks_explanation_button.TabIndex = current_index++;

            AddNewDataset_button.TabIndex = current_index++;
            AnalyzeData_button.TabIndex= current_index++;
            ClearCustomData_button.TabIndex = current_index++;
            ClearReadAnalyze_button.TabIndex = current_index++;

            ResultsDirectory_textBox.TabIndex = current_index++;
            Read_directoryOrFile_ownTextBox.TabIndex = current_index++;

            Options_readData_button.TabIndex = current_index++;
            Options_backgroundGenes_button.TabIndex = current_index++;
            Options_dataSignificance_button.TabIndex = current_index++;
            Options_organizeData_button.TabIndex = current_index++;
            Options_ontology_button.TabIndex = current_index++;
            Options_enrichment_button.TabIndex = current_index++;
            Options_scpNetworks_button.TabIndex = current_index++;
            Options_selectSCPs_button.TabIndex = current_index++;
            Options_defineSCPs_button.TabIndex = current_index++;
            Options_exampleData_button.TabIndex = current_index++;
            Options_results_button.TabIndex = current_index++;
            Options_tips_button.TabIndex = current_index++;
            Options_quickTour_button.TabIndex = current_index++;
            Options_tour_button.TabIndex = current_index++;

            AppSize_width_textBox.TabIndex = current_index++;
            AppSize_height_textBox.TabIndex = current_index++;
            AppSize_increase_button.TabIndex = current_index++;
            AppSize_decrease_button.TabIndex = current_index++;
            AppSize_colorTheme_listBox.TabIndex = current_index++;
            AppSize_resize_button.TabIndex = current_index++;

        }

        private void Update_acknowledgment_and_application_headline()
        {
            Reference_myPanelTextBox.TextColor = Form_default_settings.Color_label_foreColor;
            Reference_myPanelTextBox.Visible = false;
            Headline_myPanelLabel.Visible = false;

            Ontology_type_enum selected_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
            string mbcoPathNet_reference = "Hansen and Iyengar. MBC PathNet: Integration and visualization of networks connecting functionally related pathways predicted from transcriptomic and proteomic datasets. Bioinform Adv. 2025 Aug 18;5(1). PMID: 40917650";
            string acknowledgment_text = "error";
            switch (selected_ontology)
            {
                case Ontology_type_enum.Mbco:
                    //acknowledgment_text = "Hansen J, Meretzky D, Woldesenbet S, Stolovitzky G, Iyengar R. A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes. Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142\r\n" + mbcoPathNet_reference;
                    acknowledgment_text = "Hansen J, Meretzky D, Woldesenbet S, Stolovitzky G, Iyengar R. A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes. Sci Rep. 2017 Dec 18; 7(1). PMID: 29255142\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Molecular Biology of the Cell Ontology");
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    acknowledgment_text = "Hansen J, Sealfon R, Menon R et al for KPMP. A reference tissue atlas for the human kidney. Sci Adv. 2022 Jun 10;8(23). PMID: 35675394\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Special MBCO library: TM transport");
                    break;
                case Ontology_type_enum.Go_bp:
                    acknowledgment_text = "geneontology.org\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("MBC PathNet: GO Biological Process");
                    break;
                case Ontology_type_enum.Go_cc:
                    acknowledgment_text = "geneontology.org\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("MBC PathNet: GO Cellular Component");
                    break;
                case Ontology_type_enum.Go_mf:
                    acknowledgment_text = "geneontology.org\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("MBC PathNet: GO Molecular Function");
                    break;
                case Ontology_type_enum.Reactome:
                    acknowledgment_text = "reactome.org\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("MBC PathNet: Reactome");
                    break;
                case Ontology_type_enum.Custom_1:
                    acknowledgment_text = "User supplied ontology\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("MBC PathNet: Custom ontology 1");
                    break;
                case Ontology_type_enum.Custom_2:
                    acknowledgment_text = "User supplied ontology\r\n" + mbcoPathNet_reference;
                    Headline_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("MBC PathNet: Custom ontology 2");
                    break;
                default:
                    throw new Exception();
            }

            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;

            #region Headline panel and label
            left_referenceBorder = 0;
            right_referenceBorder = base.Width;
            top_referenceBorder = 0;
            bottom_referenceBorder = this.Options_readData_panel.Location.Y;
            Headline_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            Headline_myPanelLabel.Font_style = FontStyle.Bold;
            float max_labelDefault_size = Form_default_settings.Max_fontSize_defaultBold;
            Form_default_settings.Max_fontSize_defaultBold = 100;
            Headline_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            Form_default_settings.Max_fontSize_defaultBold = max_labelDefault_size;
            #endregion

            #region Reference panel and label
            left_referenceBorder = 0;
            right_referenceBorder = this.Options_readData_button.Location.X;
            top_referenceBorder = this.Read_directoryOrFile_ownTextBox.Location.Y + Read_directoryOrFile_ownTextBox.Height;
            bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.075 * this.Height);
            Reference_myPanelTextBox.Set_left_top_right_bottom_position(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            Reference_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh(acknowledgment_text, Form_default_settings);
            Reference_myPanelTextBox.Back_color = Form_default_settings.Color_overall_background;
            #endregion

            Reference_myPanelTextBox.Visible = true;
            Headline_myPanelLabel.Visible = true;
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
                                                      this.Options_ontology_button,
                                                      this.Options_scpNetworks_button,
                                                      this.Options_defineSCPs_button,
                                                      this.Options_results_button,
                                                      this.Options_quickTour_button,

                                                      this.Options_backgroundGenes_button,
                                                      this.Options_organizeData_button,
                                                      this.Options_enrichment_button,
                                                      this.Options_selectSCPs_button,
                                                      this.Options_exampleData_button,
                                                      this.Options_tips_button,
                                                      this.Options_tour_button
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
            ClearReadAnalyze_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            ClearReadAnalyze_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Changes_update_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Changes_update_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Changes_reset_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Changes_reset_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            AnalyzeData_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            AnalyzeData_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            LoadExamples_load_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            LoadExamples_load_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
        private void Set_main_panel_visibilities_to_default()
        {
            Tutorial_myPanelTextBox.Visible = false;
            DatasetInterface_overall_panel.Visible = true;
            DatasetInterface_overall_panel.Refresh();
            Results_visualization_panel.Visible = false;
            Report_panel.Visible = false;
            AppSize_panel.Visible = true;
        }
        private Dataset_attributes_enum[] Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes()
        {
            Set_main_panel_visibilities_to_default();
            ProgressReport.Update_progressReport_text_and_visualization("Press key to proceed");
            Dataset_attributes_enum[] visible_dataset_attributes_for_tutorial = new Dataset_attributes_enum[] { };// Dataset_attributes_enum.Delete, Dataset_attributes_enum.Name };
            Dataset_attributes_enum[] visible_dataset_attributes = DatasetSummary_userInterface.Get_dataset_attributes_defining_visible_panels();
            visible_dataset_attributes_for_tutorial = Overlap_class.Get_ordered_intersection(visible_dataset_attributes, visible_dataset_attributes_for_tutorial);
            if (!DatasetSummary_userInterface.Is_filter_mode) { DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(visible_dataset_attributes_for_tutorial); }
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            DatasetInterface_overall_panel.Refresh();
            return visible_dataset_attributes;
        }
        private void Restore_main_panel_visibilities_after_tutorial(Dataset_attributes_enum[] visible_dataset_attributes)
        {
            Set_main_panel_visibilities_to_default();
            if (!DatasetSummary_userInterface.Is_filter_mode) { DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(visible_dataset_attributes); }
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            ProgressReport.Clear_progressReport_text_and_last_entry();
        }
        private void Initialize_and_reset_userInterface_ontology(MBCO_obo_network_class parentChild_nw)
        {
            this.UserInterface_ontology = new Ontology_interface_class(Options_ontology_panel,
                                                                       Ontology_ontology_panel,
                                                                       Ontology_ontology_listBox,
                                                                       Ontology_ontology_label,
                                                                       Ontology_fileName_panelLabel,
                                                                       Ontology_organism_listBox,
                                                                       Ontology_organism_label,
                                                                       Ontology_topScpInteractions_panel,
                                                                       Ontology_topScpInteractions_left_label,
                                                                       Ontology_topScpInteractions_top_label,
                                                                       Ontology_topScpInteractions_level2_label,
                                                                       Ontology_topScpInteractions_level3_label,
                                                                       Ontology_topScpInteractions_level2_textBox,
                                                                       Ontology_topScpInteractions_level3_textBox,
                                                                       Ontology_write_scpInteractions_button,
                                                                       Ontology_tour_button,
                                                                       Ontology_writeHierarchy_button,
                                                                       UserInterface_tutorial,
                                                                       Mbco_enrichment_pipeline.Options,
                                                                       parentChild_nw,
                                                                       ProgressReport,
                                                                       Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_enrichmentOptions()
        {
            this.UserInterface_enrichmentOptions = new EnrichmentOptions_userInterface_class(Options_enrichment_panel,
                                                                                             EnrichmentOptions_ontology_panel,
                                                                                             EnrichmentOptions_ontology_label,
                                                                                             EnrichmentOptions_cutoffs_panel,
                                                                                             EnrichmentOptions_scpTopInteractions_panel,
                                                                                             EnrichmentOptions_keepTopSCPs_panel,
                                                                                             EnrichmentOptions_keepScpsScpLevel_label,
                                                                                             EnrichmentOptions_maxRanks_myPanelLabel,
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
                                                                                             EnrichmentOptions_cutoffsExplanation_myPanelLabel,
                                                                                             EnrichmentOptions_default_button,

                                                                                             EnrichmentOptions_GO_hyperparameter_panel,
                                                                                             EnrichmentOptions_GO_headline_label,
                                                                                             EnrichmentOptions_GO_size_label,
                                                                                             EnrichmentOptions_GO_size_min_label,
                                                                                             EnrichmentOptions_GO_size_max_label,
                                                                                             EnrichmentOptions_GO_sizeMin_ownTextBox,
                                                                                             EnrichmentOptions_GO_sizeMax_ownTextBox,
                                                                                             EnrichmentOptions_GO_explanation_label,

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
                                                                                             EnrichmentOptions_tutorial_button,
                                                                                             UserInterface_tutorial,

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
                                                                                  ClearReadAnalyze_button,
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
                                                                                  CompatibilityInfos_myPanelLabel,
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

            #region Results and input directories
            int height_of_textBox = (int)Math.Round(0.055 * DatasetInterface_overall_panel.Height);
            int height_of_textBoxHeadline = (int)Math.Round(0.04 * DatasetInterface_overall_panel.Height);

            int start_referenceBorder = this.ProgressReport_myPanelTextBox.Location.Y + ProgressReport_myPanelTextBox.Height + height_of_textBox;
            bottom_referenceBorder = start_referenceBorder;
            left_referenceBorder = this.DatasetInterface_overall_panel.Location.X;
            right_referenceBorder = this.Options_readData_button.Location.X - (int)Math.Round(0.001*this.Width);

            top_referenceBorder = bottom_referenceBorder + height_of_textBoxHeadline;
            bottom_referenceBorder = top_referenceBorder + height_of_textBox;
            my_textBox = ResultsDirectory_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = bottom_referenceBorder + height_of_textBoxHeadline;
            bottom_referenceBorder = top_referenceBorder + height_of_textBox;
            my_textBox = Read_directoryOrFile_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            bottom_referenceBorder = this.ResultsDirectory_textBox.Location.Y;
            top_referenceBorder = start_referenceBorder;
            my_label = this.ResultsDirectory_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            bottom_referenceBorder = this.Read_directoryOrFile_ownTextBox.Location.Y;
            top_referenceBorder = start_referenceBorder + height_of_textBox + height_of_textBoxHeadline;
            my_label = this.Read_directoryOrFile_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            Update_acknowledgment_and_application_headline();

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
                                                          Read_informationGroup_myPanelLabel,
                                                          Read_setToDefault_label,
                                                          Read_setToCustom1_button,
                                                          Read_setToCustom2_button,
                                                          Read_setToMBCO_button,
                                                          Read_setToSingleCell_button,
                                                          Read_setToMinimum_button,
                                                          Read_setToOptimum_button,
                                                          Read_readDataset_button,
                                                          Read_tutorial_button,
                                                          Report_headline_label,
                                                          Read_error_reports_button,
                                                          Read_error_reports_myPanelLabel,
                                                          Report_ownTextBox,
                                                          Report_maxErrorPerFile1_label,
                                                          Report_maxErrorPerFile2_label,
                                                          Read_error_reports_maxErrorsPerFile_ownTextBox,
                                                          ProgressReport,
                                                          UserInterface_tutorial,
                                                          Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_scpNetworks()
        {
            this.UserInterface_scp_networks = new ScpNetworks_userInterface_class(Options_scpNetworks_panel,
                                                                                  ScpNetworks_standard_panel,
                                                                                  ScpNetworks_standard_label,
                                                                                  ScpNetworks_dynamic_panel,
                                                                                  ScpNetworks_dynamic_label,
                                                                                  ScpNetworks_default_button,
                                                                                  ScpNetworks_standardParentChild_cbButton,
                                                                                  ScpNetworks_standardParentChild_cbLabel,
                                                                                  ScpNetworks_standardGroupSameLevelSCPs_cbButton,
                                                                                  ScpNetworks_standardGroupSameLevelSCPs_cbLabel,
                                                                                  ScpNetworks_standardAddGenes_cbButton,
                                                                                  ScpNetworks_standardAddGenes_cbLabel,
                                                                                  ScpNetworks_standardConnectRelated_cbButton,
                                                                                  ScpNetworks_standardConnectRelated_cbLabel,
                                                                                  ScpNetworks_hierarchicalScpInteractions_ownListBox,
                                                                                  ScpNetworks_parentChildSCPNetG_label,
                                                                                  ScpNetworks_parentChildSCPNetGeneration_ownListBox,
                                                                                  ScpNetworks_hierarchicalScpInteractions_label,
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
                                                                                  ScpNetworks_dynamicConnectAllScps_explantion_myPanelLabel,
                                                                                  ScpNetworks_generateNetworks_cbButton,
                                                                                  ScpNetworks_generateNetworks_cbLabel,

                                                                                  ScpNetworks_nodeSize_panel,
                                                                                  ScpNetworks_nodeSizes_headline_label,
                                                                                  ScpNetworks_nodeSizes_determinant_label,
                                                                                  ScpNetworks_nodeSizes_determinant_ownListBox,
                                                                                  ScpNetworks_adoptTextSize_cbButton,
                                                                                  ScpNetworks_adoptTextSize_label,
                                                                                  ScpNetworks_nodeSizes_maxDiameter_myPanelLabel,
                                                                                  ScpNetworks_nodeSizes_maxDiameter_ownTextBox,
                                                                                  ScpNetworks_nodeLabel_minSize_myPanelLabel,
                                                                                  ScpNetworks_nodeLabel_minSize_ownTextBox,
                                                                                  ScpNetworks_nodeLabel_maxSize_myPanelLabel,
                                                                                  ScpNetworks_nodeLabel_maxSize_ownTextBox,
                                                                                  ScpNetworks_nodeLabel_uniqueSize_ownTextBox,
                                                                                  ScpNetworks_nodeSizes_scaling_label,
                                                                                  ScpNetworks_nodeSizes_scaling_ownListBox,

                                                                                  ScpNetworks_graphEditor_panel,
                                                                                  ScpNetworks_graphEditor_ownListBox,
                                                                                  ScpNetworks_graphEditor_label,
                                                                                  ScpNetworks_graphFileExtension_myPanelLabel,

                                                                                  ScpNetworks_comments_panel,
                                                                                  ScpNetworks_comments_standardDynamicAddGenes_myPanelLabel,



                                                                                  Report_headline_label,
                                                                                  Report_maxErrorPerFile1_label,
                                                                                  Report_maxErrorPerFile2_label,
                                                                                  Report_ownTextBox,
                                                                                  Read_error_reports_maxErrorsPerFile_ownTextBox,

                                                                                  ScpNetworks_explanation_button,
                                                                                  ScpNetworks_tutorial_button,
                                                                                  UserInterface_tutorial,

                                                                                  Mbco_network_integration.Options,
                                                                                  Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_dataSig()
        {
            this.UserInterface_sigData = new SigData_userInterface_class(Options_dataSignificance_panel,
                                           SigData_sigSelection_panel,
                                           SigData_valueDirection_headline_label,
                                           SigData_directionValue1st_ownListBox,
                                           SigData_directionValue1st_label,
                                           SigData_directionValue2nd_ownListBox,
                                           SigData_directionValue2nd_label,

                                           SigData_first_sigCutoff_headline_label,
                                           SigData_value1st_cutoff_myPanelLabel,
                                           SigData_value1st_cutoff_ownTextBox,
                                           SigData_value1st_cutoff_expl_myPanelLabel,
                                           SigData_value2nd_cutoff_myPanelLabel,
                                           SigData_value2nd_cutoff_ownTextBox,
                                           SigData_value2nd_cutoff_expl_myPanelLabel,

                                           SigData_second_sigCutoff_headline_label,
                                           SigData_rankByValue_left_label,
                                           SigData_rankByValue_ownListBox,
                                           SigData_rankByTieBreaker_myPanelLabel,
                                           SigData_defineDataset_label,
                                           SigData_defineDataset_ownListBox,
                                           SigData_defineDataset_expl_myPanelLabel,

                                           SigData_keepTopRankedGenes_ownTextBox,
                                           SigData_keepTopRankedGenes_left_label,
                                           SigData_keepTopRankedGenes_right_label,

                                           SigData_deleteNotSignGenes_cbButton,
                                           SigData_deleteNotSignGenes_cbLabel,
                                           SigData_allGenesSignificant_headline_label,
                                           SigData_allGenesSignificant_cbButton,
                                           SigData_allGenesSignificant_cbLabel,
                                           SigData_resetSig_button,
                                           SigData_resetParameter_button,
                                           SigData_sigSubject_explanation_label,
                                           Form_default_settings,

                                           SigData_tutorial_button,
                                           UserInterface_tutorial,

                                           Custom_data.Options);
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
                                                                               OrganizeData_tutorial_button,
                                                                               UserInterface_tutorial,

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
                                                                         BgGenes_AddErrors_myPanelLabel,
                                                                         BgGenes_addReadExplainFile_myPanelLabel,
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
                                                                         UserInterface_tutorial,
                                                                         BgGenes_tutorial_button,
                                                                         ProgressReport,
                                                                         Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_selectSCPs(MBCO_obo_network_class parentChild_nw)
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
                                                                           ProgressReport,
                                                                           UserInterface_tutorial,
                                                                           SelectedScps_tutorial_button,
                                                                           Form_default_settings,
                                                                           Mbco_enrichment_pipeline.Options,
                                                                           parentChild_nw
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
                                                                                   LoadExamples_dtoxs_cbButton,
                                                                                   LoadExamples_dtoxs_cbLabel,
                                                                                   LoadExamples_dtoxs_reference,
                                                                                   LoadExamples_load_button,
                                                                                   LoadExamples_tutorial_button,
                                                                                   LoadExamples_copyright_label,
                                                                                   UserInterface_tutorial,
                                                                                   Form_default_settings);
        }
        private void Initialize_and_reset_userInterface_defineSCPs(MBCO_obo_network_class parentChild_nw)
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
                                                                          ProgressReport,
                                                                          UserInterface_tutorial,
                                                                          DefineSCPs_tutorial_button,
                                                                          Mbco_enrichment_pipeline.Options,
                                                                          Form_default_settings,
                                                                          parentChild_nw); 
        }
        private void Initialize_and_reset_userInterface_tips(MBCO_obo_network_class mbco_obo_parent_child_nw)
        {
            UserInterface_tips = new Tips_userInterface_class(Options_tips_panel,
                                                              Tips_overallHeadline_label,
                                                              Tips_tips_myPanelTextBox,
                                                              Tips_demonstration_headline_label,
                                                              Tips_demonstration_cbButton,
                                                              Tips_demonstration_cbMyPanelLabel,
                                                              Tips_forward_cbButton,
                                                              Tips_backward_cbButton,
                                                              Tips_write_mbco_hierarchy,
                                                              Mbco_enrichment_pipeline.Options,
                                                              ProgressReport,
                                                              Form_default_settings,
                                                              mbco_obo_parent_child_nw
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
                                                                    Results_directory_myPanelTextBox,
                                                                    Results_directory_expl_label,
                                                                    Results_addResultsToControl_cbButton,
                                                                    Results_addResultsToControl_cbLabel,
                                                                    Results_visualization_panel,
                                                                    Results_visualization_integrationGroup_myPanelLabel,
                                                                    Results_zegGraph_control,
                                                                    Results_previous_button,
                                                                    Results_next_button,
                                                                    Results_position_myPanelLabel,
                                                                    ProgressReport,
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
        private void Write_parameter_documentations_and_add_to_failed_writing_attemps_if_failed(string results_directory, ref List<string> fileNames_with_failed_writing_attempts)
        {
            string date_time_string = DateTime.Now.ToLongDateString().Replace(" ", "_").Replace(",", "") + "_at_" + DateTime.Now.ToShortTimeString().Replace(":", "_").Replace(" ", "_");
            string fileName = "Selected_parameter_" + date_time_string + ".txt";
            string complete_documentation_fileName = results_directory + fileName;
            System.IO.StreamWriter writer = Common_functions.ReadWrite.ReadWriteClass.Get_new_stream_writer_and_return_if_successful_or_write_warning_to_progress_report(complete_documentation_fileName, ProgressReport, out bool file_opened);
            if (file_opened)
            {
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
                            writer.WriteLine("    decreasing significance of 2nd value is used as as tiebreaker)");
                            break;
                        case Value_importance_order_enum.Value_2nd_1st:
                            writer.WriteLine("   (Genes within each {0} are ranked by decreasing significance of 2nd value,", dataset_string);
                            writer.WriteLine("    decreasing significance of 1st value is used as as tiebreaker)");
                            break;
                        default:
                            throw new Exception();
                    }
                    if (custom_data_options.Merge_upDown_before_ranking)
                    {
                        writer.WriteLine("   (*Datasets that only differ in Up/Down status are temporarily merged to rank genes of");
                        writer.WriteLine("    both datasets together as one set.)");
                    }
                }
                writer.WriteLine();
                writer.WriteLine("-------------------------------------------------------------------------------------------------------");
                writer.WriteLine();
                writer.WriteLine();
                MBCO_enrichment_pipeline_options_class enrich_options = Mbco_enrichment_pipeline.Options;
                writer.WriteLine("Ontology");
                writer.WriteLine("--------");
                writer.WriteLine(enrich_options.Next_ontology);
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
                if (Ontology_classification_class.Is_go_ontology(enrich_options.Next_ontology))
                {
                    writer.WriteLine("GO hyperparameters");
                    writer.WriteLine("------------------");
                    int goTerm_min_size = enrich_options.Next_GoOntology_hyperParameter_cutoff_dict[enrich_options.Next_ontology][GO_hyperParameter_enum.Min_size];
                    int goTerm_max_size = enrich_options.Next_GoOntology_hyperParameter_cutoff_dict[enrich_options.Next_ontology][GO_hyperParameter_enum.Max_size];
                    if ((goTerm_max_size >= 0) && (goTerm_min_size >= 0))
                    {
                        writer.WriteLine("Only GO terms with at least {0} and at max {1} genes will be considered for the enrichment", goTerm_min_size, goTerm_max_size);
                        writer.WriteLine("analysis and network generation.");
                    }
                    else if (goTerm_max_size >= 0)
                    {
                        writer.WriteLine("Only GO terms with at max {0} genes will be considered for the enrichment analysis and", goTerm_max_size);
                        writer.WriteLine("network generation.");
                    }
                    else if (goTerm_min_size >= 0)
                    {
                        writer.WriteLine("Only GO terms with at least {0} genes will be considered for the enrichment analysis", goTerm_min_size);
                        writer.WriteLine("and network generation.");
                    }
                    else
                    {
                        writer.WriteLine("Every GO term will be considered, independently of the number of annotated genes.");
                    }
                    writer.WriteLine();
                }
                if (!enrich_options.Show_all_and_only_selected_scps)
                {
                    if (Ontology_classification_class.Is_mbco_ontology(enrich_options.Next_ontology))
                    {
                        writer.WriteLine("Identification of significant SCPs:");
                        writer.WriteLine("-----------------------------------");
                        writer.WriteLine("For the identification of significant SCPs, two different cutoffs");
                        writer.WriteLine("that depend on the enrichment analysis type and SCP level are applied:");
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
                    }
                    else
                    {
                        writer.WriteLine("Identification of significant SCPs:");
                        writer.WriteLine("-----------------------------------");
                        writer.WriteLine("For the identification of significant SCPs, two different cutoffs");
                        writer.WriteLine("are applied:");
                        writer.WriteLine("1) Maximum p-value: {0}", enrich_options.Max_pvalue_for_standardEnrichment);
                        writer.WriteLine("2) Being among the top x most significant predictions: top {0}", enrich_options.Keep_top_predictions_standardEnrichment_per_level[1]);
                        writer.WriteLine();
                    }
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
                        for (int indexSelected = 0; indexSelected < selected_scps_length; indexSelected++)
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
                if (Ontology_classification_class.Is_mbco_ontology(Mbco_enrichment_pipeline.Options.Next_ontology) && (Mbco_network_integration.Options.Add_edges_that_connect_standard_scps))
                {
                    writer.WriteLine("SCP-network parameter for standard enrichment analysis");
                    writer.WriteLine("------------------------------------------------------");
                }
                else
                {
                    writer.WriteLine("SCP-network parameter");
                    writer.WriteLine("---------------------");
                }
                if (Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks)
                {
                    writer.WriteLine("SCPs that were predicted based on standard enrichment analysis are integrated into the ontology hierarchy.");
                    writer.WriteLine("Solid arrows point from parent to child SCPs.");
                    writer.WriteLine();
                }
                if (Ontology_classification_class.Is_mbco_ontology(Mbco_enrichment_pipeline.Options.Next_ontology) && (Mbco_network_integration.Options.Add_edges_that_connect_standard_scps))
                {
                    writer.WriteLine("The annotated MBCO hierarchy is enriched by a unique MBCO algorithm that infers weighted interactions between level-2");
                    writer.WriteLine("or level-3 SCPs from text mining results.");
                    writer.WriteLine("Level-2 or level-3 SCPs predicted by standard enrichment analysis are connected with each other by dashed lines without");
                    writer.WriteLine("arrow heads, if their interactions are among the top x inferred interactions.");
                    writer.WriteLine("      Interactions between level-2 SCPs: top {0}%", Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[2] * 100);
                    writer.WriteLine("      Interactions between level-2 SCPs: top {0}%", Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[3] * 100);
                }
                writer.WriteLine();
                if ((!Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps) && (Ontology_classification_class.Is_mbco_ontology(Mbco_enrichment_pipeline.Options.Next_ontology)))
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
                        writer.WriteLine("are the same as the ones used for dynamic enrichment analysis that are specified together with the other");
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
                if (Ontology_classification_class.Is_go_ontology(enrich_options.Next_ontology))
                {
                    writer.WriteLine("SCP-network parameter for Gene Ontology");
                    writer.WriteLine("-----------------------------------------------------");
                    switch (Mbco_network_integration.Options.Next_scp_hierachical_interactions)
                    {
                        case SCP_hierarchy_interaction_type_enum.Parent_child_regulatory:
                            writer.WriteLine("GO terms will be connected based on regulatory relationships and relationships between parent and child SCPs (i.e., all 3 'regulates', 'is_a' and 'part of').");
                            break;
                        case SCP_hierarchy_interaction_type_enum.Parent_child:
                            writer.WriteLine("GO terms will be connected based on relationships between parent and child SCPs (i.e., 'is_a' and 'part of').");
                            break;
                        default:
                            throw new Exception();
                    }
                    writer.WriteLine();
                }
                writer.WriteLine("SCP-node parameters");
                writer.WriteLine("-----------------------------");
                switch (Mbco_network_integration.Options.Node_size_determinant)
                {
                    case yed_network.Yed_network_node_size_determinant_enum.Uniform:
                        writer.WriteLine("SCP node areas are equal for all SCPs. Pie slices areas within the same SCP node are equal.");
                        writer.WriteLine("The standard node diameter is set to {0}.", Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant);
                        writer.WriteLine("The label size is set to {0}.", Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant);
                        break;
                    case yed_network.Yed_network_node_size_determinant_enum.No_of_different_colors:
                        writer.WriteLine("SCP node areas are proportional to the number of datasets with different colors that predicted an SCP of interest.");
                        writer.WriteLine("Pie slices are equal for all pies within the same SCP node.");
                        writer.WriteLine("The maximum node diameter is set to {0}.", Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant);
                        writer.WriteLine("The minimum label size is set to {0}.", Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant);
                        writer.WriteLine("The maximum label size is set to {0}.", Mbco_network_integration.Options.Label_maxSize_for_current_nodeSize_determinant);
                        break;
                    case yed_network.Yed_network_node_size_determinant_enum.No_of_sets:
                        writer.WriteLine("SCP node areas are proportional to the number of datasets that predicted an SCP of interest.");
                        writer.WriteLine("Pie slices are equal for all pies within the same SCP node.");
                        writer.WriteLine("The maximum node diameter is set to {0}.", Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant);
                        writer.WriteLine("The minimum label size is set to {0}.", Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant);
                        writer.WriteLine("The maximum label size is set to {0}.", Mbco_network_integration.Options.Label_maxSize_for_current_nodeSize_determinant);
                        break;
                    case yed_network.Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                        writer.WriteLine("SCP node areas are proportional to the sum of all -log10(p-values) obtained for any dataset that predicted an SCP");
                        writer.WriteLine("of interest. Pie slices are proportional to the -log10(p-values) obtained for the related datasets.");
                        if (Ontology_classification_class.Is_mbco_ontology(Mbco_enrichment_pipeline.Options.Next_ontology))
                        {
                            writer.WriteLine("In the case of dynamic enrichment analysis, the -log10(p-value) of each prediction is equally splitted among all");
                            writer.WriteLine("contributing SCPs. Since the same SCP can be part of multiple predicitons, all splitted -log10(p-values) are");
                            writer.WriteLine("summed up for each SCP.");
                        }
                        writer.WriteLine("The maximum node diameter is set to {0}.", Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant);
                        writer.WriteLine("The minimum label size is set to {0}.", Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant);
                        writer.WriteLine("The maximum label size is set to {0}.", Mbco_network_integration.Options.Label_maxSize_for_current_nodeSize_determinant);
                        break;
                    default:
                        throw new Exception();
                }
                writer.Close();
            }
            else
            {
                fileNames_with_failed_writing_attempts.Add(fileName);
            }
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

        private void Write_enrichment_results_text_files(Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, Ontology_enrichment_class combined_dynamic_not_filtered, string filtered_fileName_addition, string results_directory, ref List<string> fileNames_with_failed_writing_attempt)
        {
            ProgressReport.Update_progressReport_text_and_visualization("Writing results as tab-delimited files");
            bool file_opened_successful;
            string fileName;
            if ((combined_dynamic_filtered != null) && (combined_dynamic_filtered.Enrich.Length > 0))
            {
                string dynamic_resultName_addition = combined_dynamic_not_filtered.Get_results_fileName_addition_ontology_organism();
                fileName = dynamic_resultName_addition + "_dynamic_" + filtered_fileName_addition + ".txt";
                combined_dynamic_filtered.Write_and_return_fileOpen_success(results_directory, fileName, ProgressReport, out file_opened_successful);
                if (!file_opened_successful) { fileNames_with_failed_writing_attempt.Add(fileName); }
                fileName = dynamic_resultName_addition + "_dynamic_allPredictions.txt";
                combined_dynamic_not_filtered.Write_and_return_fileOpen_success(results_directory, fileName, ProgressReport, out file_opened_successful);
                if (!file_opened_successful) { fileNames_with_failed_writing_attempt.Add(fileName); }
            }

            string standard_resultName_addition = combined_standard_not_filtered.Get_results_fileName_addition_ontology_organism();
            fileName = standard_resultName_addition + "_standard_" + filtered_fileName_addition + ".txt";
            combined_standard_filtered.Write_and_return_fileOpen_success(results_directory, fileName, ProgressReport, out file_opened_successful);
            if (!file_opened_successful) { fileNames_with_failed_writing_attempt.Add(fileName); }
            fileName = standard_resultName_addition + "_standard_allPredictions.txt";
            combined_standard_not_filtered.Write_and_return_fileOpen_success(results_directory, fileName, ProgressReport, out file_opened_successful);
            if (!file_opened_successful) { fileNames_with_failed_writing_attempt.Add(fileName); }
            ProgressReport.Clear_progressReport_text_and_last_entry();
        }

        private void Update_progress_report(string integrationGroup_string, string task_string)
        {
            string progress_text;
            if (integrationGroup_string.Length > 0)
            { progress_text = integrationGroup_string + ": " + task_string; }
            else { progress_text = task_string; }
            ProgressReport.Update_progressReport_text_and_visualization(progress_text);
        }

        private void Generate_bardiagrams_and_return_standard_and_dynamic_graphPanes_for_eachIntegrationGroup(out Dictionary<string,GraphPane[]> integrationGroup_bardiagram_standard_graphPanes_dict, out Dictionary<string,GraphPane[]> integrationGroup_bardiagram_dynamic_graphPanes_dict, Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, string filteredFileName_addition, string results_directory)
        {
            integrationGroup_bardiagram_dynamic_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            integrationGroup_bardiagram_standard_graphPanes_dict = new Dictionary<string, GraphPane[]>();
            string[] integration_groups;
            string integration_group;
            string integration_group_add_to_file;
            int integration_groups_length;
            integration_groups = Common_functions.Array_own.Overlap_class.Get_union_of_string_arrays_keeping_the_order(combined_standard_filtered.Get_all_integrationGroups(), combined_dynamic_filtered.Get_all_integrationGroups());
            integration_groups_length = integration_groups.Length;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_filtered;
            Ontology_enrichment_class dynamic_currentIntegration_group_enrichment_filtered;
            string standard_resultsName_addition;
            string dynamic_resultsName_addition;
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
                    standard_resultsName_addition = standard_currentIntegration_group_enrichment_filtered.Get_results_fileName_addition_ontology_organism();
                    new_standard_graphPanes = Bardiagram.Generate_bardiagrams_from_enrichment_results_save_as_images_and_return_graphPanes(standard_currentIntegration_group_enrichment_filtered, results_directory, standard_resultsName_addition + standard_label + integration_group_add_to_file, Enrichment_algorithm_enum.Standard_enrichment, Form_default_settings);
                    standard_graphPanes_list.AddRange(new_standard_graphPanes);
                }
                if ((combined_dynamic_filtered!=null) && (combined_dynamic_filtered.Enrich.Length>0))
                {
                    task_string = "Generating and writing bardiagrams for dynamic enrichment analysis";
                    Update_progress_report(integrationGroup_string, task_string);
                    dynamic_currentIntegration_group_enrichment_filtered = combined_dynamic_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                    if (dynamic_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                    {
                        dynamic_resultsName_addition = standard_currentIntegration_group_enrichment_filtered.Get_results_fileName_addition_ontology_organism();
                        new_dynamic_graphPanes = Bardiagram.Generate_bardiagrams_from_enrichment_results_save_as_images_and_return_graphPanes(dynamic_currentIntegration_group_enrichment_filtered, results_directory, dynamic_resultsName_addition + "_dynamic" + integration_group_add_to_file, Enrichment_algorithm_enum.Dynamic_enrichment, Form_default_settings);
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
            integration_groups = Common_functions.Array_own.Overlap_class.Get_union_of_string_arrays_keeping_the_order(combined_standard_filtered.Get_all_integrationGroups(), combined_dynamic_filtered.Get_all_integrationGroups());
            integration_groups_length = integration_groups.Length;
            Ontology_enrichment_class standard_currentIntegration_group_enrichment_heatmap;
            Ontology_enrichment_class dynamic_currentIntegration_group_enrichment_heatmap;
            Ontology_enrichment_class integrationGroup_combined_standard_filtered;
            Ontology_enrichment_class integrationGroup_combined_dynamic_filtered;
            Ontology_enrichment_class integrationGroup_combined_standard_not_filtered;
            Ontology_enrichment_class integrationGroup_combined_dynamic_not_filtered;
            string standard_resultName_addtion;
            string dynamic_resultName_addtion;
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

                //integrationGroup_combined_dynamic_filtered.Separate_scp_unions_into_single_scps_and_add_up_splitted_shared_pvalues_and_rerank();
                //integrationGroup_combined_dynamic_not_filtered.Separate_scp_unions_into_single_scps_and_add_up_splitted_shared_pvalues_and_rerank();
                integrationGroup_combined_dynamic_filtered.Separate_scp_unions_into_single_scps_and_keep_line_defined_by_lowest_pvalue_for_each_scp_and_add_scp_specific_genes(integrationGroup_combined_standard_not_filtered);
                integrationGroup_combined_dynamic_not_filtered.Separate_scp_unions_into_single_scps_and_keep_line_defined_by_lowest_pvalue_for_each_scp_and_add_scp_specific_genes(integrationGroup_combined_standard_not_filtered);

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
                    standard_currentIntegration_group_enrichment_heatmap.Add_new_enrichment_lines_for_each_process_with_missing_integrationGroup_sampleName_entryType_timepointInDays_if_at_least_one();
                    standard_currentIntegration_group_enrichment_heatmap.Check_for_correctness();
                    standard_resultName_addtion = standard_currentIntegration_group_enrichment_heatmap.Get_results_fileName_addition_ontology_organism();

                    task_string = "Generating and writing heatmaps for standard enrichment analysis";
                    Update_progress_report(integrationGroup_string, task_string);
                    new_standard_graphPanes = Heatmap.Generate_heatmaps_and_return_graphPanes(standard_currentIntegration_group_enrichment_heatmap, results_directory, standard_resultName_addtion + standard_label + integration_group_add_to_file, Enrichment_algorithm_enum.Standard_enrichment, Form_default_settings);
                    standard_graphPanes_list.AddRange(new_standard_graphPanes);
                }
                if ((dynamic_currentIntegration_group_enrichment_heatmap!=null) && (dynamic_currentIntegration_group_enrichment_heatmap.Enrich.Length > 0))
                {
                    dynamic_currentIntegration_group_enrichment_heatmap.Add_new_enrichment_lines_for_each_process_with_missing_integrationGroup_sampleName_entryType_timepointInDays_if_at_least_one();
                    dynamic_currentIntegration_group_enrichment_heatmap.Check_for_correctness();
                    dynamic_resultName_addtion = dynamic_currentIntegration_group_enrichment_heatmap.Get_results_fileName_addition_ontology_organism();
                    task_string = "Generating and writing heatmaps for dynamic enrichment analysis";
                    Update_progress_report(integrationGroup_string, task_string);
                    new_dynamic_graphPanes = Heatmap.Generate_heatmaps_and_return_graphPanes(dynamic_currentIntegration_group_enrichment_heatmap, results_directory, dynamic_resultName_addtion + "_dynamic" + integration_group_add_to_file, Enrichment_algorithm_enum.Dynamic_enrichment, Form_default_settings);
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
            string standard_resultName_addition;

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
                    standard_currentIntegration_group_enrichment_timeline.Add_new_enrichment_lines_for_each_process_with_missing_integrationGroup_sampleName_entryType_timepointInDays_if_at_least_one();
                    standard_currentIntegration_group_enrichment_timeline.Set_equal_colors_for_all_entryTypes_and_timepoints_of_each_sampleName_to_first_timepoint_color();
                    standard_currentIntegration_group_enrichment_timeline.Check_for_correctness();
                    standard_resultName_addition = standard_currentIntegration_group_enrichment_timeline.Get_results_fileName_addition_ontology_organism();
                    new_timeline_graphPanes = Timeline_diagram.Generate_timelines_from_enrichment_results_save_as_images_and_return_graphPanes(standard_currentIntegration_group_enrichment_timeline, results_directory, standard_resultName_addition + standard_label + integration_group_add_to_file);
                    timeline_graphPanes_list.AddRange(new_timeline_graphPanes);
                }
                integrationGroup_timeline_standard_graphPanes_dict.Add(integration_group, timeline_graphPanes_list.ToArray());
            }
            Timeline_diagram.Options.Significance_pvalue_cutoff_copy = -1;
        }
        private void Generate_scp_networks(Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, string filteredFileName_addition, string results_directory, Dictionary<Enrichment_type_enum, float> enrichmentType_pvalueCutoff_dict)
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
            string standard_resultFile_addition = combined_standard_not_filtered.Get_results_fileName_addition_ontology_organism();
            string dynamic_resultFile_addition = combined_dynamic_filtered.Get_results_fileName_addition_ontology_organism();
            Enrichment_type_enum enrichment_type;
            bool network_generation_interrupted = false;
            combined_standard_filtered.Get_networkNodeSizeDeterminant_max_and_min_values_dictionaries(out Dictionary<Yed_network_node_size_determinant_enum, float> standard_networkNodeSizeDeterminant_maxValue_dict, out Dictionary<Yed_network_node_size_determinant_enum, float> standard_networkNodeSizeDeterminant_minValue_dict);
            combined_dynamic_filtered.Get_networkNodeSizeDeterminant_max_and_min_values_dictionaries(out Dictionary<Yed_network_node_size_determinant_enum, float> dynamic_networkNodeSizeDeterminant_maxValue_dict, out Dictionary<Yed_network_node_size_determinant_enum, float> dynamic_networkNodeSizeDeterminant_minValue_dict);
            for (int indexIG = 0; indexIG < integration_groups_length; indexIG++)
            {
                integration_group = integration_groups[indexIG];
                integrationGroup_string = Default_textBox_texts.Get_integrationGroup_string(integration_group);

                Set_integration_group_add_to_file_name_and_standard_label(integrationGroup_string, filteredFileName_addition, out integration_group_add_to_file, out standard_label, out dynamic_label);

                standard_currentIntegration_group_enrichment_filtered = combined_standard_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                standard_currentIntegration_group_enrichment_not_filtered = combined_standard_not_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);
                dynamic_currentIntegration_group_enrichment_filtered = combined_dynamic_filtered.Get_new_enrichment_instance_with_indicated_integrationGroup(integration_group);


                if (standard_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                {
                    string standard_nw_base_file_name = standard_resultFile_addition + integration_group_add_to_file + standard_label;
                    task_string = "Generating and writing SCP-networks for standard enrichment analysis";
                    #region Update progress report (copy paste)
                    if (integrationGroup_string.Length > 0)
                    { ProgressReport.Update_progressReport_text_and_visualization(integrationGroup_string + ":\r\n" + task_string); }
                    else { ProgressReport.Update_progressReport_text_and_visualization(task_string); }
                    #endregion
                    enrichment_type = Enrichment_type_enum.Standard;
                    network_generation_interrupted = Mbco_network_integration.Generate_and_write_integrative_network_for_indicated_enrichment_results_of_each_integrationGroupName_only_defined_sets_and_return_if_interrupted(standard_currentIntegration_group_enrichment_filtered, standard_currentIntegration_group_enrichment_not_filtered, results_directory, standard_nw_base_file_name, enrichment_type, standard_networkNodeSizeDeterminant_maxValue_dict, standard_networkNodeSizeDeterminant_minValue_dict, enrichmentType_pvalueCutoff_dict[enrichment_type], ProgressReport);
                    if (network_generation_interrupted)
                    {
                        break;
                    }
                    ProgressReport.Clear_progressReport_text_and_last_entry();
                }

                if ((combined_dynamic_filtered!=null)&&(combined_dynamic_filtered.Enrich.Length>0))
                {
                    string dynamic_nw_base_file_name = dynamic_resultFile_addition + integration_group_add_to_file + "_dynamic";
                    task_string = "Generating and writing SCP-networks for dynamic enrichment analysis";
                    #region Update progress report (copy paste)
                    if (integrationGroup_string.Length > 0)
                    { ProgressReport.Update_progressReport_text_and_visualization(integrationGroup_string + ":\r\n" + task_string); }
                    else { ProgressReport.Update_progressReport_text_and_visualization(task_string); }
                    #endregion
                    enrichment_type = Enrichment_type_enum.Dynamic;
                    if (dynamic_currentIntegration_group_enrichment_filtered.Enrich.Length > 0)
                    {
                        network_generation_interrupted = Mbco_network_integration.Generate_and_write_integrative_network_for_indicated_enrichment_results_of_each_integrationGroupName_only_defined_sets_and_return_if_interrupted(dynamic_currentIntegration_group_enrichment_filtered, standard_currentIntegration_group_enrichment_not_filtered, results_directory, dynamic_nw_base_file_name, enrichment_type, dynamic_networkNodeSizeDeterminant_maxValue_dict, dynamic_networkNodeSizeDeterminant_minValue_dict, enrichmentType_pvalueCutoff_dict[enrichment_type], ProgressReport);
                        if (network_generation_interrupted) { break; }
                    }
                    ProgressReport.Clear_progressReport_text_and_last_entry();
                }
            }
        }
        private void Write_parameter_spreadsheet_from_option_files_and_add_to_failed_writing_attempts(string results_directory, string input_data_subdirectory, ref List<string> fileNames_failed_writing)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            results_directory = gdf.Transform_into_compatible_directory_and_clean_up(results_directory);
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string complete_fileName = results_directory + input_data_subdirectory + global_dirFile.Mbco_parameter_settings_fileName;
            Common_functions.ReadWrite.ReadWriteClass.Create_directory_if_it_does_not_exist(results_directory + input_data_subdirectory);

            System.IO.StreamWriter writer = ReadWriteClass.Get_new_stream_writer_and_return_if_successful_or_write_warning_to_progress_report(complete_fileName, ProgressReport, out bool file_opened);
            if (file_opened)
            {
                writer.WriteLine(global_dirFile.FirstLine_of_mbco_parameter_setting_fileName);
                writer.WriteLine("Results directory{0}{1}", Global_class.Tab, results_directory);
                Custom_data.Options.Write_option_entries(writer);
                Mbco_enrichment_pipeline.Options.Write_option_entries(writer);
                Mbco_network_integration.Options.Write_option_entries(writer);
                Bardiagram.Options.Write_option_entries(writer);
                Heatmap.Options.Write_option_entries(writer);
                Timeline_diagram.Options.Write_option_entries(writer);
                writer.Close();
            }
            else
            {
                fileNames_failed_writing.Add(global_dirFile.Mbco_parameter_settings_fileName);
            }
        }

        private void Add_parameters_from_parameter_setting_lines_to_options_and_update_options_in_all_menu_panels(string[] parameter_setting_lines)
        {
            int parameter_settings_length = parameter_setting_lines.Length;
            string parameter_setting_line;
            string classTypeName;

            Ontology_type_enum current_next_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
            Organism_enum current_next_organism = Mbco_enrichment_pipeline.Options.Next_organism;
            Ontology_type_enum selected_next_ontology;
            Organism_enum selected_next_organism;
            Mbco_enrichment_pipeline.Options.Clear_all_deNovo_dictionaries();
            List<string> parameter_setting_lines_with_errors = new List<string>();

            bool successful;

            for (int indexP=0; indexP<parameter_settings_length;indexP++)
            {
                parameter_setting_line = parameter_setting_lines[indexP];
                successful = false;
                if (!Global_class.Do_internal_checks)
                {
                    try //always copy paste the block into the else condition
                    {
                        classTypeName = parameter_setting_line.Split(Global_class.Tab)[0];
                        if (classTypeName.Equals(typeof(User_data_options_class).Name))
                        { successful = Custom_data.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                        else if (classTypeName.Equals(typeof(MBCO_enrichment_pipeline_options_class).Name))
                        { successful = Mbco_enrichment_pipeline.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                        else if (classTypeName.Equals(typeof(MBCO_network_based_integration_options_class).Name))
                        { successful = Mbco_network_integration.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                        else if (classTypeName.Equals(typeof(Bardiagram_options_class).Name))
                        { successful = Bardiagram.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                        else if (classTypeName.Equals(typeof(Heatmap_options_class).Name))
                        { successful = Heatmap.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                        else if (classTypeName.Equals(typeof(Timeline_options_class).Name))
                        { successful = Timeline_diagram.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                        else if (classTypeName.Equals("Results directory"))
                        { 
                            ResultsDirectory_textBox.SilentText_and_refresh = (string)parameter_setting_line.Split(Global_class.Tab)[1].Clone();
                            successful = true;
                        }
                    }
                    catch
                    {
                        successful = false;
                    }
                }
                else //always copy paste the block above into the else condition
                {
                    classTypeName = parameter_setting_line.Split(Global_class.Tab)[0];
                    if (classTypeName.Equals(typeof(User_data_options_class).Name))
                    { successful = Custom_data.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                    else if (classTypeName.Equals(typeof(MBCO_enrichment_pipeline_options_class).Name))
                    { successful = Mbco_enrichment_pipeline.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                    else if (classTypeName.Equals(typeof(MBCO_network_based_integration_options_class).Name))
                    { successful = Mbco_network_integration.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                    else if (classTypeName.Equals(typeof(Bardiagram_options_class).Name))
                    { successful = Bardiagram.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                    else if (classTypeName.Equals(typeof(Heatmap_options_class).Name))
                    { successful = Heatmap.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                    else if (classTypeName.Equals(typeof(Timeline_options_class).Name))
                    { successful = Timeline_diagram.Options.Add_read_entry_to_options_and_return_if_successful(parameter_setting_line); }
                    else if (classTypeName.Equals("Results directory"))
                    {
                        ResultsDirectory_textBox.SilentText_and_refresh = (string)parameter_setting_line.Split(Global_class.Tab)[1].Clone();
                        successful = true;
                    }
                }
                if (!successful)
                {
                    parameter_setting_lines_with_errors.Add(parameter_setting_line);
                }
            }

            if (parameter_setting_lines_with_errors.Count>0)
            {
                Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                List<string> final_report_lines = new List<string>();
                final_report_lines.Add("The following parameter settings could not be imported:");
                final_report_lines.Add("");
                final_report_lines.AddRange(parameter_setting_lines_with_errors);
                final_report_lines.Add("");
                final_report_lines.Add("Recommendation:");
                final_report_lines.Add("Use the application to define anticipated parameter settings and analyze any dataset.");
                final_report_lines.Add("Copy-paste the generated '" + gdf.Mbco_parameter_settings_fileName + "'-file from the");
                final_report_lines.Add("'Input_data'-subdirectory within the results folder into your data directory.");
                string error_reports_label = "Some parameters could not be imported.";
                UserInterface_read.Add_to_error_label_and_error_box_and_switch_to_error_mode_if_not_already(error_reports_label, final_report_lines.ToArray());
            }

            selected_next_ontology = Mbco_enrichment_pipeline.Options.Next_ontology;
            selected_next_organism = Mbco_enrichment_pipeline.Options.Next_organism;
            if (  (!current_next_ontology.Equals(selected_next_ontology))
                ||(!current_next_organism.Equals(selected_next_organism)))
            {
                UserInterface_ontology.Update_organism_and_ontology_if_possible_and_add_comment_if_not(selected_next_ontology, selected_next_organism, Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
            }
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
        private void Generate_and_write_all_enrichment_results(Ontology_enrichment_class combined_standard_filtered, Ontology_enrichment_class combined_dynamic_filtered, Ontology_enrichment_class combined_standard_not_filtered, Ontology_enrichment_class combined_dynamic_not_filtered, string filtered_fileName_addition, string results_directory, ref bool results_added_to_results_menu, ref List<string> fileNames_with_failed_writting_attempts, Dictionary<Enrichment_type_enum, float> enrichmentType_pvalueCutoff_dict)
        {
            Write_enrichment_results_text_files(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory, ref fileNames_with_failed_writting_attempts);
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
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Bardiagram_standard, integrationGroup_standardGraphPanes, ref results_added_to_results_menu);
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Bardiagram_dynamic, integrationGroup_dynamicGraphPanes, ref results_added_to_results_menu);
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
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Heatmap_standard, integrationGroup_standardGraphPanes, ref results_added_to_results_menu);
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Heatmap_dynamic, integrationGroup_dynamicGraphPanes, ref results_added_to_results_menu);
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
                    UserInterface_results.Add_enrichmentResults_graphPanes(Enrichment_results_enum.Timeline_standard, integrationGroup_standardGraphPanes, ref results_added_to_results_menu);
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
            { Generate_scp_networks(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, filtered_fileName_addition, results_directory, enrichmentType_pvalueCutoff_dict); }
        }
        private bool AnalyzeData_after_button_click_and_return_true_if_success_or_no_data_lines()
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string results_directory = global_dirFile.Transform_into_compatible_directory_and_clean_up(ResultsDirectory_textBox.Text + global_dirFile.Delimiter);
            bool results_directory_contains_invalid_characters = global_dirFile.Does_path_contain_invalid_characters(results_directory, Form_default_settings);
            Delete_all_existing_analysis_finished_fileNames();
            Custom_data.Check_for_correctness();
            int datasets_length = Custom_data.Get_all_unique_ordered_fixed_datasetIdentifies().Length;
            string progress_warning_text;
            bool successful_analysis_or_zero_data_lines = false;
            ResultsDirectory_textBox.SilentText = results_directory;
            if (datasets_length == 0)
            {
                progress_warning_text = "Please add, read or load dataset(s).";
                ProgressReport.Write_temporary_warning_and_restore_progressReport(progress_warning_text, 3);
            }
            if (datasets_length > 0)
            {
                progress_warning_text = "Checking, if all " + datasets_length + " datasets can be submitted to enrichment analysis";
                ProgressReport.Update_progressReport_text_and_visualization(progress_warning_text);
            }
            if (results_directory_contains_invalid_characters)
            {
                progress_warning_text = "Results directory contains invalid characters.";
                ProgressReport.Write_temporary_warning_and_restore_progressReport(progress_warning_text, 3);
            }
            if (Custom_data.Custom_data.Length==0) { successful_analysis_or_zero_data_lines = true; }
            if ((Custom_data.Analyse_if_data_can_be_submitted_to_enrichment_analysis(Timeline_diagram.Options.Generate_timeline_in_log_scale))&&(!results_directory_contains_invalid_characters))
            {
                Custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
                progress_warning_text = "Preparing " + Custom_data.Get_all_unique_ordered_fixed_datasetIdentifies().Length + " datasets for enrichment analysis";
                ProgressReport.Update_progressReport_text_and_visualization(progress_warning_text);
                Custom_data.Set_unique_datasetName_within_whole_custom_data_ignoring_integrationGroups();
                string fileName_addition = "";
                Ontology_enrichment_class standard_enrichment_unfiltered;
                Ontology_enrichment_class dynamic_enrichment_unfiltered;
                bool is_necessary_to_update_pipeline_for_all_runs = Mbco_enrichment_pipeline.Is_necessary_to_update_pipeline_for_all_runs_and_out_give_update_network_integration(out bool update_network_integration);
                if (is_necessary_to_update_pipeline_for_all_runs)
                {
                    progress_warning_text = Ontology_classification_class.Get_loading_report_for_enrichment(Mbco_enrichment_pipeline.Options.Next_ontology);
                    ProgressReport.Update_progressReport_text_and_visualization(progress_warning_text);
                    Mbco_enrichment_pipeline.Generate_for_all_runs_after_setting_ontology_to_next_ontology(ProgressReport);
                }
                if (Mbco_enrichment_pipeline.Is_data_complete_for_analysis())
                {
                    List<string> fileNames_failed_to_be_written = new List<string>();
                    bool streamWriter_opened_successfully;
                    Mbco_enrichment_pipeline.Write_orthologue_identification_if_done(results_directory);
                    Mbco_enrichment_pipeline.Remove_all_existing_custom_scps_from_mbco_association_unmodified_and_add_new_ones();
                    if ((!Mbco_network_integration.Options.Ontology.Equals(Mbco_enrichment_pipeline.Options.Ontology))
                        || (!Mbco_network_integration.Options.Ontology.Equals(Mbco_network_integration.Options.Next_ontology))
                        || (!Mbco_network_integration.Options.Scp_hierachical_interactions.Equals(Mbco_network_integration.Options.Next_scp_hierachical_interactions))
                        || (update_network_integration)
                        || (!Mbco_network_integration.Generated_for_all_runs))
                    {
                        progress_warning_text = Ontology_classification_class.Get_loadAndPrepare_report_for_network(Mbco_enrichment_pipeline.Options.Next_ontology);
                        ProgressReport.Update_progressReport_text_and_visualization(progress_warning_text);
                        string[] keep_scps = Mbco_enrichment_pipeline.MBCO_association_unmodified.Get_all_distinct_ordered_scps();
                        keep_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(keep_scps, new string[] { Ontology_classification_class.No_annotated_parent_scp });
                        Mbco_network_integration.Generate_for_all_runs_after_setting_ontology_and_organism_to_next_ontology_and_organism(Mbco_enrichment_pipeline.Options.Ontology, Mbco_enrichment_pipeline.Options.Next_organism, keep_scps, ProgressReport);
                    }
                    if (!Mbco_network_integration.Options.Ontology.Equals(Mbco_enrichment_pipeline.Options.Ontology)) { throw new Exception(); }
                    if (!Mbco_network_integration.Options.Ontology.Equals(Mbco_network_integration.Options.Next_ontology)) { throw new Exception(); }
                    if (!Mbco_enrichment_pipeline.Options.Ontology.Equals(Mbco_enrichment_pipeline.Options.Next_ontology)) { throw new Exception(); }
                    Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = Mbco_enrichment_pipeline.Options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level;

                    string[] mbco_background_genes = Mbco_enrichment_pipeline.Get_all_symbols_of_any_SCPs_after_updating_instance_if_ontology_unequals_next_ontology(ProgressReport);

                    string input_subdirectory = "Input_data" + global_dirFile.Delimiter;
                    string results_input_directory = results_directory + global_dirFile.Delimiter + input_subdirectory;
                    Write_parameter_documentations_and_add_to_failed_writing_attemps_if_failed(results_directory, ref fileNames_failed_to_be_written);
                    Write_parameter_spreadsheet_from_option_files_and_add_to_failed_writing_attempts(results_directory, input_subdirectory, ref fileNames_failed_to_be_written);
                    Custom_data_summary_class data_summary = new Custom_data_summary_class();
                    data_summary.Generate_from_custom_data(Custom_data, mbco_background_genes);
                    string data_summary_fileName = "Summary_of_data_submitted_to_enrichment.txt";
                    data_summary.Write(results_directory, data_summary_fileName, ProgressReport, out streamWriter_opened_successfully);
                    if (!streamWriter_opened_successfully) { fileNames_failed_to_be_written.Add(data_summary_fileName); }
                    string submitted_data_fileName = "Data_submitted_to_enrichment.txt";
                    Custom_data.Write(results_input_directory, submitted_data_fileName, ProgressReport, out streamWriter_opened_successfully);
                    if (!streamWriter_opened_successfully) { fileNames_failed_to_be_written.Add(submitted_data_fileName); }
                    string[] datasets_with_missing_sign_genes_in_bgList = data_summary.Get_uniqueDatasetNames_plus_integrationGroup_with_no_signficant_genes_in_final_background_gene_list();
                    if (data_summary.Get_number_of_significant_genes_in_final_background_gene_list_of_all_datasets() == 0)
                    {
                        progress_warning_text = "No dataset contains significant genes in final background gene list. Enrichment analysis not possible.\r\nSee '.\\Input_data\\Summary_of_data_submitted_to_enrichment.txt' for details.";
                        ProgressReport.Write_temporary_warning_and_restore_progressReport(progress_warning_text, 7);
                    }
                    else
                    {
                        if (datasets_with_missing_sign_genes_in_bgList.Length > 0)
                        {
                            progress_warning_text = datasets_with_missing_sign_genes_in_bgList.Length + " datasets have no significant genes that are part of final background gene list.\r\nSee '.\\Input_data\\Summary_of_data_submitted_to_enrichment.txt' for details.";
                            ProgressReport.Write_temporary_warning_and_restore_progressReport(progress_warning_text, 7);
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
                        progress_warning_text = "Analyzing " + Custom_data.Get_all_unique_ordered_fixed_datasetIdentifies().Length + " datasets for enrichment of genes annotated to " + Ontology_classification_class.Get_name_of_scps_for_progress_report(Mbco_enrichment_pipeline.Options.Ontology);
                        ProgressReport.Update_progressReport_text_and_visualization(progress_warning_text);
                        for (int indexBG = 0; indexBG < bgGeneListNames_length; indexBG++)
                        {
                            bgGeneListName = bgGeneListNames[indexBG];
                            currentBgGeneListNames_custom_data = Custom_data.Get_custom_data_class_with_indicated_background_genes_list(bgGeneListName);
                            currentBgGeneListNames_custom_data.Keep_only_significant_lines();
                            current_bgGenes = currentBgGeneListNames_custom_data.ExpBgGenesList_bgGenes_dict[bgGeneListName];
                            Mbco_enrichment_pipeline.Reset_mbco_and_adjust_bg_genes_and_set_to_upperCase(current_bgGenes);
                            currentBgGenes_data = currentBgGeneListNames_custom_data.Generate_new_data_instance();
                            currentBgGenes_data.Set_all_ncbi_official_gene_symbols_to_upper_case();
                            Mbco_enrichment_pipeline.Analyse_data_instance_fast(currentBgGenes_data, results_directory, fileName_addition, out standard_enrichment_unfiltered, out dynamic_enrichment_unfiltered);
                            combined_standard_not_filtered.Add_other(standard_enrichment_unfiltered);
                            combined_dynamic_not_filtered.Add_other(dynamic_enrichment_unfiltered);
                        }

                        ProgressReport.Clear_progressReport_text_and_last_entry();
                        Custom_data.Reset_unique_datasetName();

                        combined_dynamic_not_filtered.Check_for_correctness();
                        combined_standard_not_filtered.Check_for_correctness();

                        combined_standard_not_filtered.Set_significance_based_on_ranks_and_pvalue_after_calculation_of_fractional_rank(Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level, Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment);
                        combined_dynamic_not_filtered.Set_significance_based_on_ranks_and_pvalue_after_calculation_of_fractional_rank(Mbco_enrichment_pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level, Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment);

                        Ontology_enrichment_class combined_standard_filtered;
                        Ontology_enrichment_class combined_dynamic_filtered;
                        string filtered_fileName_addition = "";

                        #region Prepare results visualization 
                        UserInterface_results.Clear_all_enrichmentResults_graphPanes();
                        bool results_added_to_results_menu = false;
                        #endregion

                        if (Mbco_enrichment_pipeline.MBCO_association.MBCO_associations.Length == 0)
                        {
                            progress_warning_text = "Selected ontology/SCPs do not contain genes, please change selection.";
                            ProgressReport.Write_temporary_warning_and_restore_progressReport(progress_warning_text, 7);
                        }
                        else
                        {
                            Dictionary<Enrichment_type_enum, float> enrichmentType_pvalueCutoff_dict = new Dictionary<Enrichment_type_enum, float>();
                            enrichmentType_pvalueCutoff_dict.Add(Enrichment_type_enum.Standard, Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment);
                            enrichmentType_pvalueCutoff_dict.Add(Enrichment_type_enum.Dynamic, Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment);
                            if (!Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps)
                            {
                                combined_standard_filtered = combined_standard_not_filtered.Deep_copy();
                                combined_dynamic_filtered = combined_dynamic_not_filtered.Deep_copy();
                                combined_standard_filtered.Keep_only_signficant_enrichment_lines_and_reset_uniqueDatasetNames();
                                combined_dynamic_filtered.Keep_only_signficant_enrichment_lines_and_reset_uniqueDatasetNames();
                                filtered_fileName_addition = "significantPredictions";
                                Generate_and_write_all_enrichment_results(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory, ref results_added_to_results_menu, ref fileNames_failed_to_be_written, enrichmentType_pvalueCutoff_dict);
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
                                    Generate_and_write_all_enrichment_results(combined_standard_filtered, combined_dynamic_filtered, combined_standard_not_filtered, combined_dynamic_not_filtered, filtered_fileName_addition, results_directory, ref results_added_to_results_menu, ref fileNames_failed_to_be_written, enrichmentType_pvalueCutoff_dict);
                                }
                            }
                            //if (light_up_results_button)
                            //{
                            //    Options_results_button.BackColor = Form_default_settings.Color_button_highlight_back;
                            //    Options_results_button.ForeColor = Form_default_settings.Color_button_highlight_fore;
                            //    Options_results_button.Refresh();
                            //    if (!Form_default_settings.Is_mono)
                            //    {
                            //        System.Threading.Thread.Sleep(1000);
                            //    }
                            //    Options_results_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                            //    Options_results_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
                            //    Options_results_button.Refresh();
                            //}
                            StringBuilder sb = new StringBuilder();
                            if (results_added_to_results_menu)
                            { sb.Append("Results can be found in the directory specified below or in the 'Results' menu."); }
                            else { sb.Append("Results can be found in the directory specified below."); }
                            if (Mbco_network_integration.Options.Generate_scp_networks)
                            {
                                switch (Mbco_network_integration.Options.Graph_editor)
                                {
                                    case Graph_editor_enum.Cytoscape:
                                        sb.Append("\r\nImport xgmml network files and the xml style into Cytoscape using the 'File'-menu. Open the 'Style'- tab on the left to select 'MBC PathNet'-style. When uploading multiple networks, ensure to unselect existing networks to prevent network merging.");
                                        break;
                                    case Graph_editor_enum.yED:
                                        sb.Append("\r\nOpen graphml SCP networks with 'yED - graph editor'. Select 'Layout', 'Tree' - 'Directed'. Within the pop-up window open the 'Directed' tab and activate 'Consider Node Labels'.");
                                        break;
                                    default:
                                        throw new Exception();
                                }
                            }
                            ProgressReport.Update_progressReport_text_and_visualization(sb.ToString());
                            successful_analysis_or_zero_data_lines = true;
                        }
                    }
                    if (fileNames_failed_to_be_written.Count>0)
                    {
                        StringBuilder error_sb = new StringBuilder();
                        foreach (string fileName in fileNames_failed_to_be_written)
                        {
                            if (error_sb.Length>0) { error_sb.Append(", "); }
                            error_sb.AppendFormat("'{0}'", fileName);
                        }
                        error_sb.Append(" could not be opened.");
                        if (fileNames_failed_to_be_written.Count==1)
                        {
                            error_sb.Append(" Is it in use ? Is file / directory name too long ? ");
                        }
                        else
                        {
                            error_sb.Append(" Are they in use ? Are file / directory names too long ? ");
                        }
                        ProgressReport.Write_warning_and_save_current_entry(error_sb.ToString());
                        successful_analysis_or_zero_data_lines = false;
                    }
                }
            }
            if (successful_analysis_or_zero_data_lines) { Write_all_analysis_finished_fileNames(out bool fileOpened_successful); }
            return successful_analysis_or_zero_data_lines;
        }
        private void AnalyzeData_button_Click(object sender, EventArgs e)
        {
            Set_tour_and_quick_tour_buttons_to_notPressed_if_highlighted();
            Set_datasetSummary_button_to_pressed(AnalyzeData_button);
            AnalyzeData_after_button_click_and_return_true_if_success_or_no_data_lines();
            Set_datasetSummary_button_to_notPressed(AnalyzeData_button);
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

        private void EnrichmentOptions_GO_sizeMin_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.GO_min0_max1_size_textBox_TextChanged(0, Mbco_enrichment_pipeline.Options);
        }

        private void EnrichmentOptions_GO_sizeMax_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_enrichmentOptions.GO_min0_max1_size_textBox_TextChanged(1, Mbco_enrichment_pipeline.Options);
        }
        private void EnrichmentOptions_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_enrichmentOptions.Set_explanation_tutorial_button_to_not_selected();
            UserInterface_enrichmentOptions.Set_selected_explanation_or_tutorial_button_to_activated(EnrichmentOptions_tutorial_button);
            UserInterface_enrichmentOptions.Tutorial_button_activated(Mbco_enrichment_pipeline.Options.Next_ontology);
            UserInterface_enrichmentOptions.Set_explanation_tutorial_button_to_not_selected();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
        }
        private void EnrichmentOptions_explanation_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (UserInterface_enrichmentOptions.Is_explanation_tutorial_button_active(EnrichmentOptions_explanation_button))
            {
                UserInterface_enrichmentOptions.Set_explanation_tutorial_button_to_not_selected();
                Reset_progressBar_after_explanationErrorReports_textBox_has_been_closed();
            }
            else
            {
                UserInterface_enrichmentOptions.Set_explanation_tutorial_button_to_not_selected();
                UserInterface_enrichmentOptions.Set_selected_explanation_or_tutorial_button_to_activated(EnrichmentOptions_explanation_button);
                UserInterface_enrichmentOptions.Explanation_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                Set_progressBar_to_describe_how_to_scroll_explanationErrorReports_textBox();
            }
        }

        #endregion
        private void Set_progressBar_to_describe_how_to_scroll_explanationErrorReports_textBox()
        {
            if (Form_default_settings.Is_mono)
            { ProgressReport.Update_progressReport_text_and_visualization("To scroll the text, click inside the text box and use the mouse wheel, arrow keys, or Page Up/Down keys."); }
            else { ProgressReport.Update_progressReport_text_and_visualization("To scroll the text, move the pointer over the text box and use the mouse wheel, or click inside the box and use the arrow keys or Page Up/Down."); }
        }
        private void Reset_progressBar_after_explanationErrorReports_textBox_has_been_closed()
        {
            ProgressReport.Clear_progressReport_text_and_last_entry();
        }
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
                string[] mbco_background_genes = Mbco_enrichment_pipeline.Get_all_symbols_of_any_SCPs_after_updating_instance_if_ontology_unequals_next_ontology(ProgressReport);
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
            Set_tour_and_quick_tour_buttons_to_notPressed_if_highlighted();
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

        private void ClearCustomData_after_button_is_pressed()
        {
            Set_tour_and_quick_tour_buttons_to_notPressed_if_highlighted();
            Custom_data.Clear_custom_data();
            UserInterface_bgGenes.Update_available_bgGeneListsName_listBox(Custom_data);
            Dataset_attributes_enum[] new_attributes = new Dataset_attributes_enum[] { Dataset_attributes_enum.Delete, Dataset_attributes_enum.Name, Dataset_attributes_enum.Color };
            DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(new Dataset_attributes_enum[0]);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            DatasetSummary_userInterface.Reset_empty_interface_lines_to_default(0);
            DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(new_attributes);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(new_attributes);
        }

        private void ClearCustomData_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();
            Custom_data.Clear_custom_data();
            ClearCustomData_after_button_is_pressed();
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            Custom_data = UserInterface_bgGenes.Reset_bgGenesLists_to_default(Custom_data);
        }
        protected bool ClearReadAnalyze_and_return_sucess_if_at_least_one_dataset_was_analyzed_successfully_or_no_data_was_analyzed_at_all(Ontology_type_enum selected_ontology, Organism_enum selected_organism, params string[] results_directory)
        {
            Set_tour_and_quick_tour_buttons_to_notPressed_if_highlighted();
            Set_datasetSummary_button_to_pressed(ClearReadAnalyze_button);
            ClearCustomData_after_button_is_pressed();
            UserInterface_read.Set_visibility(true);
            UserInterface_read.Read_allFilesInDirectory_ownCheckedBox_clicked();
            bool is_reading_succesful = ReadData_after_button_click_and_return_if_at_least_one_dataset_was_uploaded_successfully();
            if (  (!selected_ontology.Equals(Ontology_type_enum.E_m_p_t_y))
                ||(!selected_organism.Equals(Organism_enum.E_m_p_t_y)))
            {
                Update_nextOntology_and_nextOrganism_from_outside_ontologyOrganismUserInterface_if_not_empty_and_if_current_selections_are_valid_and_return_eventual_error_messages(selected_ontology, selected_organism, out string potential_error_messages);
            }
            if (results_directory.Length>1) { throw new Exception(); }
            else if (results_directory.Length==1)
            {
                ResultsDirectory_textBox.SilentText = (string)results_directory[0].Clone();
            }
            bool is_analysis_successful_or_no_data_lines = false;
            if (is_reading_succesful)
            {
                is_analysis_successful_or_no_data_lines = AnalyzeData_after_button_click_and_return_true_if_success_or_no_data_lines();
            }
            else
            {
                Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels(Options_menu_enum.Read_data);
                Set_optionsButton_to_pressed(Options_readData_button);
                UserInterface_read.Set_visibility(true);
                ProgressReport.Update_progressReport_text_and_visualization("Data could not be read, see 'Read data'-menu on the right for details.");
            }
            Set_datasetSummary_button_to_notPressed(ClearReadAnalyze_button);
            return is_analysis_successful_or_no_data_lines;
        }

        private bool ClearReadAnalyze_and_return_sucess_if_at_least_one_dataset_was_analyzed_successfully_or_no_data_was_analyzed_at_all()
        {
            return ClearReadAnalyze_and_return_sucess_if_at_least_one_dataset_was_analyzed_successfully_or_no_data_was_analyzed_at_all(Ontology_type_enum.E_m_p_t_y, Organism_enum.E_m_p_t_y, new string[0]);
        }
        private void ClearReadAnalyze_button_Click(object sender, EventArgs e)
        {
            ClearReadAnalyze_and_return_sucess_if_at_least_one_dataset_was_analyzed_successfully_or_no_data_was_analyzed_at_all();
        }

        private void Dataset_scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            DatasetSummary_userInterface.Dataset_scrollBar_Scroll();
        }

        #region Options buttons
        private void Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels(params Options_menu_enum[] ignore_panels)
        {
            Set_options_buttons_and_panel_visibilities_to_default_except_specified_panels(ignore_panels);
            ProgressReport.Clear_progressReport_text_and_last_entry();
        }
        private void Set_tour_and_quick_tour_buttons_to_notPressed_if_highlighted()
        {
            List<Button> buttons = new List<Button>();
            if (Options_tour_button.BackColor.Equals(Form_default_settings.Color_button_highlight_back)) { buttons.Add(Options_tour_button); }
            if (Options_quickTour_button.BackColor.Equals(Form_default_settings.Color_button_highlight_back)) { buttons.Add(Options_quickTour_button); }

            foreach (Button button in buttons)
            {
                Set_optionsButton_to_notPressed(button);
            }
        }
        private void Set_options_buttons_and_panel_visibilities_to_default_except_specified_panels(params Options_menu_enum[] ignore_panels)
        {
            Dictionary<Options_menu_enum, bool> ignorePanel_dict = new Dictionary<Options_menu_enum, bool>();
            foreach (Options_menu_enum ignore_panel in ignore_panels)
            {
                ignorePanel_dict.Add(ignore_panel, true);
            }

            List<Button> buttons = new List<Button>();
            buttons.Add(Options_readData_button);
            buttons.Add(Options_scpNetworks_button);
            buttons.Add(Options_ontology_button);
            buttons.Add(Options_enrichment_button);
            buttons.Add(Options_backgroundGenes_button);
            buttons.Add(Options_organizeData_button);
            buttons.Add(Options_dataSignificance_button);
            buttons.Add(Options_enrichment_button);
            buttons.Add(Options_exampleData_button);
            buttons.Add(Options_defineSCPs_button);
            buttons.Add(Options_selectSCPs_button);
            buttons.Add(Options_tips_button);
            buttons.Add(Options_results_button);
            Set_tour_and_quick_tour_buttons_to_notPressed_if_highlighted();

            foreach (Button button in buttons)
            {
                Set_optionsButton_to_notPressed(button);
            }

            EnrichmentOptions_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            EnrichmentOptions_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            OrganizeData_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            OrganizeData_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            ScpNetworks_explanation_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            ScpNetworks_explanation_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;

            Options_ontology_panel.Visible = false;
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
            if (!ignorePanel_dict.ContainsKey(Options_menu_enum.Read_data)) { UserInterface_read.Set_visibility(false); }
            UserInterface_scp_networks.Set_visibility(false, Mbco_network_integration.Options);
            UserInterface_enrichmentOptions.Set_visibility(false, Mbco_enrichment_pipeline.Options, Bardiagram.Options, Heatmap.Options, Timeline_diagram.Options);
            UserInterface_loadExamples.Set_visibility(false);
            Options_bgGenes_panel.Visible = false;
            Options_tips_panel.Visible = false;
            DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();

            ResultsDirectory_label.Visible = true;
            ResultsDirectory_textBox.Visible = true;
            Reference_myPanelTextBox.Visible = true;
            ProgressReport_myPanelTextBox.Visible = true;
        }
        private void Set_dataset_interface_to_selected_attributes_in_organizeData_panel()
        {
            Dataset_attributes_enum[] selected_attributes = UserInterface_organize_data.Get_dataset_attributes_from_showCheckBoxes();
            selected_attributes = DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(selected_attributes);
            DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            UserInterface_organize_data.Set_showCheckBoxes_based_on_dataset_attributes(selected_attributes);
        }
        private void Set_datasetSummary_button_to_pressed(Button button)
        {
            Set_optionsButton_to_pressed(button);
        }
        private void Set_datasetSummary_button_to_notPressed(Button button)
        {
            Set_optionsButton_to_notPressed(button);
        }
        private void Set_optionsButton_to_pressed(Button options_button)
        {
            options_button.BackColor = Form_default_settings.Color_button_pressed_back;
            options_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            options_button.Refresh();
        }
        private void Set_optionsButton_to_notPressed(Button options_button)
        {
            options_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            options_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            options_button.Refresh();
        }
        private void Options_ontology_button_Click_action()
        {
            Set_main_panel_visibilities_to_default();
            Color last_backColor = Options_ontology_button.BackColor;
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (last_backColor.Equals(Form_default_settings.Color_button_notPressed_back))
            {
                Set_optionsButton_to_pressed(Options_ontology_button);
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                UserInterface_ontology.Set_to_visible(Mbco_enrichment_pipeline.Options);
            }
        }
        private void Options_ontology_button_Click(object sender, EventArgs e)
        {
            Options_ontology_button_Click_action();
        }
        private bool Is_options_button_pressed(Button options_button)
        {
            return options_button.BackColor.Equals(Form_default_settings.Color_button_pressed_back);
        }
        private void Options_readData_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_readData_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_optionsButton_to_pressed(Options_readData_button);
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                UserInterface_read.Set_visibility(true);
            }
        }
        private void Options_enrichment_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_enrichment_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Set_optionsButton_to_pressed(Options_enrichment_button);
                UserInterface_enrichmentOptions.Set_visibility(true, Mbco_enrichment_pipeline.Options, Bardiagram.Options, Heatmap.Options, Timeline_diagram.Options);
            }
        }
        private void Update_parentChild_hierarchies_in_selectScps_defineScps_ontology_and_tips_if_necessary()
        {
            bool defineScps_upToData = UserInterface_defineSCPs.Are_parentChild_hierarchy_networks_upToDate(Mbco_enrichment_pipeline.Options);
            bool selectScps_upToData = UserInterface_selectSCPs.Are_parentChild_hierarchy_networks_upToDate(Mbco_enrichment_pipeline.Options);
            bool ontologyScps_upToData = UserInterface_ontology.Are_parentChild_hierarchy_networks_upToDate(Mbco_enrichment_pipeline.Options);
            bool tips_upToData = UserInterface_tips.Are_parentChild_hierarchy_networks_upToDate(Mbco_enrichment_pipeline.Options);
            if (Global_class.Do_internal_checks)
            {
                if (!defineScps_upToData.Equals(selectScps_upToData)) { throw new Exception(); }
                if (!defineScps_upToData.Equals(tips_upToData)) { throw new Exception(); }
                if (!defineScps_upToData.Equals(ontologyScps_upToData)) { throw new Exception(); }
            }
            if ((!ontologyScps_upToData)||(!defineScps_upToData)||(!selectScps_upToData)||(!tips_upToData))
            {
                ProgressReport.Update_progressReport_text_and_visualization("Updating ontology networks.");
                MBCO_obo_network_class mbco_parent_child_network = new MBCO_obo_network_class(Mbco_enrichment_pipeline.Options.Next_ontology, SCP_hierarchy_interaction_type_enum.Parent_child, Mbco_enrichment_pipeline.Options.Next_organism);
                mbco_parent_child_network.Generate_by_reading_safed_spreadsheet_file_or_obo_file_add_missing_scps_if_custom_add_human_processSizes_and_return_if_not_interrupted(ProgressReport, out bool not_interrupted);
                if (!not_interrupted)
                {
                    UserInterface_defineSCPs.Get_ontology_and_oranism_from_mbcp_parent_child_network(out Ontology_type_enum previous_ontology, out Organism_enum previous_organism);
                    Mbco_enrichment_pipeline.Options.Set_next_ontology_and_organism(previous_ontology, previous_organism);
                    Mbco_network_integration.Options.Set_next_ontology_and_organism(previous_ontology, previous_organism);
                    Update_acknowledgment_and_application_headline();
                    //mbco_parent_child_network = new MBCO_obo_network_class(Mbco_enrichment_pipeline.Options.Next_ontology, SCP_hierarchy_interaction_type_enum.Parent_child, Mbco_enrichment_pipeline.Options.Next_organism);
                    //mbco_parent_child_network.Generate_by_reading_safed_spreadsheet_file_or_obo_file_and_return_if_finalized(ProgressReport, out not_interrupted);
                }
                else
                {
                    if (!selectScps_upToData) { UserInterface_defineSCPs.Update_mbco_parent_child_and_child_parent_obo_networks_and_adjust_sortByList(mbco_parent_child_network); }
                    if (!defineScps_upToData) { UserInterface_selectSCPs.Update_mbco_parent_child_and_child_parent_networks_and_set_to_default(mbco_parent_child_network); }
                    if (!tips_upToData) { UserInterface_tips.Update_mbco_parent_child_and_child_parent_obo_networks(mbco_parent_child_network); }
                    if (!ontologyScps_upToData) {  UserInterface_ontology.Update_mbco_parent_child_and_child_parent_obo_networks(mbco_parent_child_network); }
                }
            }
        }
        private void Options_selectSCPs_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_selectSCPs_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_optionsButton_to_pressed(Options_selectSCPs_button);
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Update_parentChild_hierarchies_in_selectScps_defineScps_ontology_and_tips_if_necessary();
                UserInterface_selectSCPs.Set_to_visible(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, true);
            }
        }
        private void Options_scpNetworks_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_scpNetworks_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                UserInterface_scp_networks.Set_visibility(true, Mbco_network_integration.Options);
                Set_optionsButton_to_pressed(Options_scpNetworks_button);
            }
        }
        private void Options_backgroundGenes_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_backgroundGenes_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                UserInterface_bgGenes.Set_to_visible(Custom_data);
                Set_optionsButton_to_pressed(Options_backgroundGenes_button);
                Dataset_attributes_enum[] attributes_for_bgGenes_selection = UserInterface_bgGenes.BgGenesInterface_dataset_attributes;
                DatasetSummary_userInterface.Set_to_filter_mode(attributes_for_bgGenes_selection);
            }
        }
        private void Options_organizeData_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_organizeData_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Set_optionsButton_to_pressed(Options_organizeData_button);
                Dataset_attributes_enum[] visible_panels = DatasetSummary_userInterface.Get_dataset_attributes_defining_visible_panels();
                UserInterface_organize_data.Set_to_visible(visible_panels);
            }
        }
        private void Options_dataSignificance_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_dataSignificance_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_optionsButton_to_pressed(Options_dataSignificance_button);
                Custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
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
                DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
                DatasetSummary_userInterface.Set_attributes_with_visible_panel_if_space_and_return_final_selection(visible_panels);
                DatasetSummary_userInterface.Set_visibility_and_xlocations_of_dataset_panels_to_overall_visibility_if_among_seletected_attributes();
            }
        }
        private void Options_defineSCPs_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_defineSCPs_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_optionsButton_to_pressed(Options_defineSCPs_button);
                Update_parentChild_hierarchies_in_selectScps_defineScps_ontology_and_tips_if_necessary();
                UserInterface_defineSCPs.Set_to_visible(Mbco_enrichment_pipeline.Options, true);
            }
        }
        private void Options_exampleData_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_exampleData_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_dataset_interface_to_selected_attributes_in_organizeData_panel();
                Set_optionsButton_to_pressed(Options_exampleData_button);
                UserInterface_loadExamples.Set_visibility(true);
                Dataset_attributes_enum[] visible_panels = DatasetSummary_userInterface.Get_dataset_attributes_defining_visible_panels();
            }
        }
        private void Options_tips_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_tips_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_optionsButton_to_pressed(Options_tips_button);
                Update_parentChild_hierarchies_in_selectScps_defineScps_ontology_and_tips_if_necessary();
                Options_tips_panel.Visible = true;
            }
        }
        private void Options_results_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            bool button_pressed = Is_options_button_pressed(Options_results_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            if (!button_pressed)
            {
                Set_optionsButton_to_pressed(Options_results_button);
                UserInterface_results.Set_visibility(true);
                Results_visualization_panel.Visible = true;
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = false;
                ProgressReport_myPanelTextBox.Visible = false;
                ResultsDirectory_label.Visible = false;
                ResultsDirectory_textBox.Visible = false;
                Reference_myPanelTextBox.Visible = false;
            }
        }
        private void Highlight_tutorial_button_in_menu_panel(Button tutorial_button)
        {
            tutorial_button.BackColor = Form_default_settings.Color_button_highlight_back;
            tutorial_button.ForeColor = Form_default_settings.Color_button_highlight_fore;
            tutorial_button.Refresh();
        }
        private void Options_quickTour_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();
            Dataset_attributes_enum[] currently_visible_datset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            Set_main_panel_visibilities_to_default();
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            Set_optionsButton_to_pressed(Options_quickTour_button);

            int mid_y_position;
            int mid_x_position;
            int bottom_y_position;
            int right_x_position;
            int top_y_position;
            int left_x_position;
            string text;
            int distance_from_options_panel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;

            int explanation_index = -1;
            bool end_tour = false;
            bool escape_pressed = false;
            bool back_pressed = false;
            while (!end_tour)
            {
                explanation_index++;
                switch (explanation_index)
                {
                    case 0:
                        top_y_position = DatasetInterface_overall_panel.Location.Y + Input_geneList_textBox.Location.Y + (int)Math.Round(0.5F * Input_geneList_textBox.Height);
                        left_x_position = DatasetInterface_overall_panel.Location.X + AddNewDataset_button.Location.X;
                        text = "For data upload, copy genes into the 'Gene list'-text-box, name your dataset using the 'Name'-text-box and press the 'Add dataset'-button.";
                        if (Is_options_button_pressed(Options_readData_button)) { Options_readData_button_Click(this, EventArgs.Empty); }
                        Set_datasetSummary_button_to_pressed(AddNewDataset_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, left_x_position, top_y_position, ContentAlignment.TopLeft);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_datasetSummary_button_to_notPressed(AddNewDataset_button);
                        break;
                    case 1:
                        mid_y_position = Options_readData_panel.Location.Y + (int)Math.Round(0.5F * Options_readData_panel.Height);
                        right_x_position = Options_readData_panel.Location.X - distance_from_options_panel;
                        text = "Alternatively, upload multiple datasets simultaneously using the 'Read data'-menu.";
                        if (!Is_options_button_pressed(Options_readData_button)) { Options_readData_button_Click(this, EventArgs.Empty); }
                        Highlight_tutorial_button_in_menu_panel(Read_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
                        break;
                    case 2:
                        mid_y_position = Options_ontology_panel.Location.Y + (int)Math.Round(0.5F * Options_ontology_panel.Height);
                        right_x_position = Options_ontology_panel.Location.X - distance_from_options_panel;
                        text = "Select an ontology of interest and the species under investigation.";
                        if (!Is_options_button_pressed(Options_ontology_button)) { Options_ontology_button_Click(this, EventArgs.Empty); }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
                        break;
                    case 3:
                        mid_y_position = Options_scpNetworks_panel.Location.Y + ScpNetworks_graphEditor_panel.Location.Y + (int)Math.Round(0.5F * ScpNetworks_graphEditor_panel.Height);
                        right_x_position = Options_scpNetworks_panel.Location.X - distance_from_options_panel;
                        text = "Select if network files shall be generated for the yED graph editor or Cytoscape.";
                        if (!Is_options_button_pressed(Options_scpNetworks_button)) { Options_scpNetworks_button_Click(this, EventArgs.Empty); }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
                        break;
                    case 4:
                        mid_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y + AnalyzeData_button.Height
                                         + (int)Math.Round(0.5F * (ResultsDirectory_textBox.Location.Y - DatasetInterface_overall_panel.Location.Y - AnalyzeData_button.Location.Y - AnalyzeData_button.Height));
                        mid_x_position = DatasetInterface_overall_panel.Location.X + AnalyzeData_button.Location.X + (int)Math.Round(0.5F * AnalyzeData_button.Width);
                        text = "Specify a results directory and press the 'Analyze'-button.";
                        Set_datasetSummary_button_to_pressed(AnalyzeData_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, mid_y_position, ContentAlignment.MiddleCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_datasetSummary_button_to_notPressed(AnalyzeData_button);
                        break;
                    case 5:
                        int distance_above_analyze_buttons = (int)Math.Round(0.01F * DatasetInterface_overall_panel.Height);
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        Set_datasetSummary_button_to_pressed(ClearReadAnalyze_button);
                        text = "The 'Clear, read & analyze'-button reduces manual steps by automatically uploading data, background genes, and settings, and immediately starting the analysis.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_datasetSummary_button_to_notPressed(ClearReadAnalyze_button);
                        break;
                    default:
                        end_tour = true;
                        break;
                }
                if (escape_pressed) { end_tour = true; }
                if (back_pressed) { explanation_index = explanation_index - 2; }
                if (explanation_index==-2) { end_tour = true; }
            }
            Set_optionsButton_to_notPressed(Options_quickTour_button);
            Restore_main_panel_visibilities_after_tutorial(currently_visible_datset_attributes);
        }

        private void Options_tour_button_Click(object sender, EventArgs e)
        {
            DatasetSummary_userInterface.Remove_filter_mode_if_in_filter_mode();
            Dataset_attributes_enum[] currently_visible_datset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            Set_main_panel_visibilities_to_default();
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            Set_optionsButton_to_pressed(Options_tour_button);

            int mid_y_position;
            int mid_x_position;
            int right_x_position;
            //int top_y_position;
            int bottom_y_position;
            int left_x_position;
            string text;
            int distance_from_options_panel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;

            string first_pathway_name = "Pathways";
            string pathway_name = "pathway";
            if (Ontology_classification_class.Is_mbco_ontology(Mbco_enrichment_pipeline.Options.Next_ontology))
            {
                first_pathway_name = "Subcellular processes (SCPs)";
                pathway_name = "SCP";
            }

            int distance_above_analyze_buttons = (int)Math.Round(0.07F * DatasetInterface_overall_panel.Height);

            int explanation_index = -1;
            bool end_tour = false;
            bool escape_pressed = false;
            bool back_pressed = false;
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            while (!end_tour)
            {
                explanation_index++;
                switch (explanation_index)
                {
                    case 0:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        left_x_position = DatasetInterface_overall_panel.Location.X + AddNewDataset_button.Location.X;
                        text = "Genes copied into the 'Gene list'-text-box will be uploaded under the name specified in the 'Name'-text-box after pressing the 'Add dataset'-button.";
                        Set_datasetSummary_button_to_pressed(AddNewDataset_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, left_x_position, bottom_y_position, ContentAlignment.BottomLeft);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_datasetSummary_button_to_notPressed(AddNewDataset_button);
                        break;
                    case 1:
                        mid_y_position = Options_readData_panel.Location.Y + (int)Math.Round(0.5F * Options_readData_panel.Height);
                        right_x_position = Options_readData_panel.Location.X - distance_from_options_panel;
                        //text = first_pathway_name + " predicted for datasets within the same integration group will be displayed in the same charts and integrated into combined " + pathway_name + " networks.";
                        text = "As a central functionality, the application allows grouping of datasets into integration groups. " + first_pathway_name + " predicted for datasets within the same integration group will be displayed in the same charts and " + pathway_name + " networks.";
                        if (Is_options_button_pressed(Options_readData_button)) { Options_readData_button_Click(this, EventArgs.Empty); }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_organize_data.Set_explanation_and_tutorial_buttons_to_inactive();
                        break;
                    case 2:
                        mid_y_position = Options_readData_panel.Location.Y + (int)Math.Round(0.5F * Options_readData_panel.Height);
                        right_x_position = Options_readData_panel.Location.X - distance_from_options_panel;
                        text = "The 'Read data' menu enables automatic upload of multiple files containing datasets, background genes and parameter settings. Selected dataset attributes, including integration groups, are extracted from those files.";
                        if (!Is_options_button_pressed(Options_readData_button)) { Options_readData_button_Click(this, EventArgs.Empty); }
                        Highlight_tutorial_button_in_menu_panel(Read_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
                        break;
                    case 3:
                        mid_y_position = Options_dataSignificance_panel.Location.Y + (int)Math.Round(0.5F * Options_dataSignificance_panel.Height);
                        right_x_position = Options_dataSignificance_panel.Location.X - distance_from_options_panel;
                        text = "Background genes can additionally be uploaded using the 'Background genes'-menu.";
                        if (!Is_options_button_pressed(Options_backgroundGenes_button))
                        {
                            Options_backgroundGenes_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(BgGenes_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_bgGenes.Set_explanation_tutorial_buttons_to_inactive();
                        break;
                    case 4:
                        mid_y_position = Options_dataSignificance_panel.Location.Y + (int)Math.Round(0.5F * Options_dataSignificance_panel.Height);
                        right_x_position = Options_dataSignificance_panel.Location.X - distance_from_options_panel;
                        text = "Significance cutoffs for uploaded datasets can be defined using the 'Set data cutoffs'-menu.";
                        if (!Is_options_button_pressed(Options_dataSignificance_button))
                        {
                            Options_dataSignificance_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(SigData_tutorial_button);
                        bool sig_selected = SigData_allGenesSignificant_cbButton.Checked;
                        if (!sig_selected) { SigData_allGenesSignificant_cbButton_Click(this, EventArgs.Empty); }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_sigData.Set_tutorial_button_to_inactive();
                        if (!sig_selected) { SigData_allGenesSignificant_cbButton_Click(this, EventArgs.Empty); }
                        break;
                    case 5:
                        mid_y_position = Options_organizeData_panel.Location.Y + (int)Math.Round(0.5F * Options_organizeData_panel.Height);
                        right_x_position = Options_organizeData_panel.Location.X - distance_from_options_panel;
                        text = "The functionalities to organize the uploaded datasets include the quick assignment of integration groups and colors to multiple datasets at once.";
                        if (!Is_options_button_pressed(Options_organizeData_button))
                        {
                            Options_organizeData_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(OrganizeData_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_organize_data.Set_explanation_and_tutorial_buttons_to_inactive();
                        break;
                    case 6:
                        mid_y_position = Options_ontology_panel.Location.Y + (int)Math.Round(0.5F * Options_ontology_panel.Height);
                        right_x_position = Options_ontology_panel.Location.X - distance_from_options_panel;
                        text = "In the 'Ontology / Species' menu, users can choose the ontology for data analysis and specify the species under investigation.";
                        if (!Is_options_button_pressed(Options_ontology_button))
                        {
                            Options_ontology_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(Ontology_tour_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_ontology.Set_tour_button_to_not_selected();
                        break;
                    case 7:
                        mid_y_position = Options_enrichment_panel.Location.Y + (int)Math.Round(0.5F * Options_enrichment_panel.Height);
                        right_x_position = Options_enrichment_panel.Location.X - distance_from_options_panel;
                        text = "The 'Enrichment' menu allows setting significance cutoffs for predicted " + pathway_name + "s, selecting ontology-specific parameters, and choosing charts for result visualization.";
                        if (!Is_options_button_pressed(Options_enrichment_button))
                        {
                            Options_enrichment_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(EnrichmentOptions_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_enrichmentOptions.Set_explanation_tutorial_button_to_not_selected();
                        break;
                    case 8:
                        mid_y_position = Options_scpNetworks_panel.Location.Y + (int)Math.Round(0.5F * Options_scpNetworks_panel.Height);
                        right_x_position = Options_scpNetworks_panel.Location.X - distance_from_options_panel;
                        text = "In the 'SCP-networks'-menu the user can specify parameters for networks connecting predicted " + pathway_name + "s and select if network files shall be written for the yED graph editor or Cytoscape.";
                        if (!Is_options_button_pressed(Options_scpNetworks_button))
                        {
                            Options_scpNetworks_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(ScpNetworks_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_scp_networks.Set_explanation_and_tutorial_button_to_inactive();
                        break;
                    case 9:
                        mid_y_position = Options_selectSCPs_panel.Location.Y + (int)Math.Round(0.5F * Options_selectSCPs_panel.Height);
                        right_x_position = Options_selectSCPs_panel.Location.X - distance_from_options_panel;
                        text = "The 'Select SCPs' menu enables the definition of " + pathway_name + " groups for targeted analysis that can include visualization of associated dataset genes.";
                        if (!Is_options_button_pressed(Options_selectSCPs_button))
                        {
                            Options_selectSCPs_button_Click(this, EventArgs.Empty);
                            SelectSCPs_addGroup_button_Click(sender, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(SelectedScps_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_selectSCPs.Set_tutorial_button_to_inactive();
                        break;
                    case 10:
                        mid_y_position = Options_defineScps_panel.Location.Y + (int)Math.Round(0.5F * Options_defineScps_panel.Height);
                        right_x_position = Options_defineScps_panel.Location.X - distance_from_options_panel;
                        text = "To generate new " + pathway_name + "s by merging the genes of existing " + pathway_name + "s use the 'Define new SCPs'-menu.";
                        if (!Is_options_button_pressed(Options_defineSCPs_button))
                        {
                            Options_defineSCPs_button_Click(this, EventArgs.Empty);
                            DefineScps_addNewOwnSCP_button_Click(this, EventArgs.Empty);
                        }
                        Highlight_tutorial_button_in_menu_panel(DefineSCPs_tutorial_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        UserInterface_defineSCPs.Set_tutorial_button_to_inactive();
                        Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
                        break;
                    case 11:
                        distance_above_analyze_buttons = (int)Math.Round(0.01F * DatasetInterface_overall_panel.Height);
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + AnalyzeData_button.Location.X + (int)Math.Round(0.5F * AnalyzeData_button.Width);
                        text = "Pressing the 'Analyze'-button will subject uploaded datasets to enrichment analysis. Results will be saved into specified directory below.";
                        Set_datasetSummary_button_to_pressed(AnalyzeData_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_datasetSummary_button_to_notPressed(AnalyzeData_button);
                        break;
                    case 12:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "The 'Clear, read & analyze'-button reduces manual steps by automatically uploading data, background genes, and settings, and immediately starting the analysis.";
                        Set_datasetSummary_button_to_pressed(ClearReadAnalyze_button);
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 13:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "After uploading data via the 'Read data'-menu, the application saves selected parameters (e.g., data file or folder name, and any chosen 'Custom 1' or '2' column names) for reimport after restart.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 14:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "After data analysis, parameters of all menus are saved to ‘" + gdf.Mbco_parameter_settings_fileName + "’ in the ‘Input_data’ subfolder of the results directory.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 15:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "The application will always search for that parameter settings file in a given input directory and reimport the selections.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 16:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "Copy paste of the '" + gdf.Mbco_parameter_settings_fileName + "' into a given data directory allows automatic import and external modification of parameter settings.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 17:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "The described features enable immediate analysis of any data from the last input directory with preselected parameters upon application launch using the 'Clear, read & analyze'-button.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        Set_datasetSummary_button_to_notPressed(ClearReadAnalyze_button);
                        break;
                    case 18:
                        bottom_y_position = DatasetInterface_overall_panel.Location.Y + AnalyzeData_button.Location.Y - distance_above_analyze_buttons;
                        mid_x_position = DatasetInterface_overall_panel.Location.X + ClearReadAnalyze_button.Location.X + (int)Math.Round(0.5F * ClearReadAnalyze_button.Width);
                        text = "Launching the application from the command line under specification of at least one argument automatically triggers the 'Clear, read & analyze'-pipeline. For details, see the file 'CommandLine_Automation_Guide.txt'.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, mid_x_position, bottom_y_position, ContentAlignment.BottomCenter);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    default:
                        end_tour = true;
                        break;
                }
                if (escape_pressed) { end_tour = true; }
                if (back_pressed) { explanation_index = explanation_index - 2; }
                if (explanation_index == -2) { end_tour = true; }
            }
            Set_optionsButton_to_notPressed(Options_tour_button);
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_datset_attributes);
        }


        #endregion

        #region Read userInterface buttons
        protected bool Read_custom_columNames_and_update_in_userInterface_readMenu_and_return_if_successful(string directory, int custom_columnNames_no)
        {
            ReadTextBoxName_columnName_readWriteOptions readWriteOptions = new ReadTextBoxName_columnName_readWriteOptions(directory, custom_columnNames_no);
            bool successful = false;
            if (System.IO.File.Exists(readWriteOptions.File))
            {
                Read_error_message_line_class[] error_messages;
                DatasetAttribute_columnName_line_class[] datasetAttribute_columnNames_lines = ReadWriteClass.Read_data_fill_array_and_return_error_messages<DatasetAttribute_columnName_line_class>(out error_messages, readWriteOptions, ProgressReport);
                if (error_messages.Length==0)
                {
                    UserInterface_read.Fill_or_override_datasetAttribute_customColumnName_1_or_2_dict(datasetAttribute_columnNames_lines, custom_columnNames_no);
                    UserInterface_read.Fill_textBox_columNames_from_datasetAttribtute_columnName_dict(custom_columnNames_no);
                    successful = true;
                }
            }
            return successful;
        }
        protected bool Is_readDataMenu_in_error_report_mode()
        {
            return UserInterface_read.Is_error_mode();
        }
        protected string Get_deep_clone_of_entry_of_explanation_error_reports_panels()
        {
            return UserInterface_read.Get_deep_clone_of_entry_of_explanationErrorReport_panel();
        }
        protected string Get_deep_clone_of_entry_of_readData_errorReportsMyPanel()
        {
            return UserInterface_read.Get_deep_clone_of_entry_of_errorReportsMyPanel();
        }
        protected string Get_deep_clone_of_entry_of_progressBar()
        {
            return (string)ProgressReport.Get_deep_copy_of_progressReport_text();
        }
        protected void Set_input_data_directory(string new_directory)
        {
            Read_directoryOrFile_ownTextBox.SilentText_and_refresh = (string)new_directory.Clone();
        }
        protected void Read_setToCustom1_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_custom1_button_clicked();
        }
        protected void Read_setToCustom2_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_custom2_button_clicked();
        }
        protected void Read_setToMBCO_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_mbco_button_clicked();
        }
        protected void Read_setToSingleCell_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_singleCell_button_clicked();
        }
        protected void Read_setToMinimum_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.SetTo_minimum_button_clicked();
        }
        protected void Read_setToOptimum_button_Click(object sender, EventArgs e)
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
        protected void Read_order_allFilesInDirectory_cbButton_Click(object sender, EventArgs e)
        {
            UserInterface_read.Read_allFilesInDirectory_ownCheckedBox_clicked();
        }
        private void Read_order_onlySpecifiedFile_cbButton_Click(object sender, EventArgs e)
        {
            UserInterface_read.Read_onlySpecifiedFile_ownCheckedBox_clicked();
        }
        private void Read_error_reports_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (UserInterface_read.Is_given_explanation_tuturial_or_readData_button_active(Read_error_reports_button))
            {
                UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
                DatasetInterface_overall_panel.Visible = true;
                Report_panel.Visible = false;
                Reset_progressBar_after_explanationErrorReports_textBox_has_been_closed();
            }
            else
            {
                UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                UserInterface_read.Set_selected_explanation_tutorial_readData_button_to_active(Read_error_reports_button);
                Set_progressBar_to_describe_how_to_scroll_explanationErrorReports_textBox();
            }
        }
        private void Read_error_reports_maxErrorsPerFile_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_read.Error_reports_maxErrorsPerFile_ownTextBox_TextChanged();
        }
        private void Read_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
            UserInterface_read.Set_selected_explanation_tutorial_readData_button_to_active(Read_tutorial_button);
            UserInterface_read.Tutorial_button_pressed();
            UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
        }
        private bool ReadData_after_button_click_and_return_if_at_least_one_dataset_was_uploaded_successfully()
        {
            Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
            string defaultIntegrationGroup = DatasetSummary_userInterface.Get_default_integrationGroup_from_lastSaved_datasetSummaries();
            string[] parameter_setting_lines;
            int before_custom_data_length = this.Custom_data.Custom_data.Length;
            this.Custom_data = UserInterface_read.Read_button_click(this.Custom_data, defaultIntegrationGroup, selectable_colors, Override_used_selected_column_names_enum.No, out parameter_setting_lines);
            if (parameter_setting_lines.Length > 0)
            {
                Add_parameters_from_parameter_setting_lines_to_options_and_update_options_in_all_menu_panels(parameter_setting_lines);
                Check_if_result_figure_options_agree();
            }
            Analyze_if_custom_data_compatible_with_specified_options_and_adopt_options_if_not();
            DatasetSummary_userInterface.Copy_custom_data_into_all_interface_fields(Custom_data);
            DatasetSummary_userInterface.Update_graphical_interface_excluding_dataset_panel_visiblities_and_xlocations();
            Set_main_panel_visibilities_to_default();
            Set_visualization_in_datasetSummary_userInterface_and_organizeData_userInterface_to_optimum();
            return this.Custom_data.Custom_data.Length > before_custom_data_length;
        }
        private void ReadDataset_button_Click(object sender, EventArgs e)
        {
            UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
            UserInterface_read.Set_selected_explanation_tutorial_readData_button_to_active(Read_readDataset_button);
            ReadData_after_button_click_and_return_if_at_least_one_dataset_was_uploaded_successfully();
            UserInterface_read.Set_explanation_tutorial_readData_buttons_to_inactive();
        }
        #endregion

        #region Ontology user interface
        #endregion


        #region ScpNetworks interface buttons and boxes
        private void ScpNetworks_standardGroupSameLevelSCPs_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardGroupSameLevelSCPs_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardParentChild_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardParentChild_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardAddGenes_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardAddGenes_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardConnectRelated_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_standardConnectRelated_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_parentChildSCPNetG_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_hierarchicalScpInteractions_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicAddGenes_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicAddGenes_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicParentChild_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicParentChild_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicGroupSameLevelSCPs_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicConnectAllRelated_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_dynamicConnectAllRelated_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_generateNetworks_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_generateNetworks_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_default_button_Click(object sender, EventArgs e)
        {
            Mbco_network_integration.Replace_options_by_default_options_but_keep_nextOntology_nextOrganism_and_reset_generated_for_all_runs();
            UserInterface_scp_networks.Copy_options_into_interface_selections(Mbco_network_integration.Options);
        }
        private void ScpNetworks_standardTopLevel_2_interactions_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.StandardTopLevel_2_interactions_ownTextBox_TextChanged(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardTopLevel_3_interactions_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.StandardTopLevel_3_interactions_ownTextBox_TextChanged(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_dynamicGroupSameLevelSCPs_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_standardGroupSameLevelSCPs_ownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_nodeSizes_determinant_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_nodeSizes_maxDiameter_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSizes_maxDiameter_ownTextBox_TextChanged(Mbco_network_integration.Options);
        }
        private void ScpNetworks_nodeLabel_minSize_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSizes_label_minSize_ownTextBox_TextChanged(Mbco_network_integration.Options);
        }
        private void ScpNetworks_nodeLabel_maxSize_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSizes_label_maxSize_ownTextBox_TextChanged(Mbco_network_integration.Options);
        }
        private void ScpNetworks_nodeLabel_uniqueSize_ownTextBox_TextChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.NodeSizes_label_uniqueSize_ownTextBox_TextChanged(Mbco_network_integration.Options);
        }

        private void ScpNetworks_nodeSizes_scaling_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_adoptTextSize_cbButton_Click(object sender, EventArgs e)
        {
            ScpNetworks_adoptTextSize_cbButton.Button_pressed();
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }
        private void ScpNetworks_graphEditor_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mbco_network_integration.Options = UserInterface_scp_networks.Copy_interfaceSelections_excluding_textBoxes_into_options(Mbco_network_integration.Options, Mbco_enrichment_pipeline.Options);
        }

        private void ScpNetworks_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_scp_networks.Set_explanation_and_tutorial_button_to_inactive();
            UserInterface_scp_networks.Set_selected_explanation_or_tutorial_button_to_activated(ScpNetworks_tutorial_button);
            UserInterface_scp_networks.Tutorial_button_activated(Mbco_enrichment_pipeline.Options.Next_ontology);
            UserInterface_scp_networks.Set_explanation_and_tutorial_button_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
        }
        private void ScpNetworks_explanation_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (UserInterface_scp_networks.Is_activated_explanation_or_tutorial_button(ScpNetworks_explanation_button))
            {
                UserInterface_scp_networks.Set_explanation_and_tutorial_button_to_inactive();
                Reset_progressBar_after_explanationErrorReports_textBox_has_been_closed();
            }
            else
            {
                UserInterface_scp_networks.Set_explanation_and_tutorial_button_to_inactive();
                UserInterface_scp_networks.Set_selected_explanation_or_tutorial_button_to_activated(ScpNetworks_explanation_button);
                UserInterface_scp_networks.Explanation_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                Set_progressBar_to_describe_how_to_scroll_explanationErrorReports_textBox();
            }
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
            Custom_data = UserInterface_bgGenes.Add_read_button_pressed(Custom_data);
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
            if (UserInterface_bgGenes.Is_given_explanation_or_tuturial_button_active(BgGenes_warnings_button))
            {
                UserInterface_bgGenes.Set_explanation_tutorial_buttons_to_inactive();
                Reset_progressBar_after_explanationErrorReports_textBox_has_been_closed();
            }
            else
            {
                UserInterface_bgGenes.Set_explanation_tutorial_buttons_to_inactive();
                UserInterface_bgGenes.Set_selected_explanation_tutorial_button_to_active(BgGenes_warnings_button);
                UserInterface_bgGenes.Warnings_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                BgGenes_AddShowErrors_button.BackColor = Form_default_settings.Color_button_notPressed_back;
                BgGenes_AddShowErrors_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
                Set_progressBar_to_describe_how_to_scroll_explanationErrorReports_textBox();
            }
        }
        private void BgGenes_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_bgGenes.Set_explanation_tutorial_buttons_to_inactive();
            UserInterface_bgGenes.Set_selected_explanation_tutorial_button_to_active(BgGenes_tutorial_button);
            UserInterface_bgGenes.Tutorial_button_pressed(Mbco_enrichment_pipeline.Options.Next_ontology);
            UserInterface_bgGenes.Set_explanation_tutorial_buttons_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
        }
        #endregion

        private void OrganizeData_explanation_button_Click(object sender, EventArgs e)
        {
            Set_main_panel_visibilities_to_default();
            if (UserInterface_organize_data.Is_given_explanation_or_tutorial_button_active(OrganizeData_explanation_button))
            {
                UserInterface_organize_data.Set_explanation_and_tutorial_buttons_to_inactive();
                Reset_progressBar_after_explanationErrorReports_textBox_has_been_closed();
            }
            else
            {
                UserInterface_organize_data.Set_explanation_and_tutorial_buttons_to_inactive();
                UserInterface_organize_data.Set_explanation_or_tutorial_button_to_active(OrganizeData_explanation_button);
                UserInterface_organize_data.Explanation_button_activated();
                DatasetInterface_overall_panel.Visible = false;
                Report_panel.Visible = true;
                Set_progressBar_to_describe_how_to_scroll_explanationErrorReports_textBox();
            }
        }
        private void OrganizeData_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_organize_data.Set_explanation_and_tutorial_buttons_to_inactive();
            UserInterface_organize_data.Set_explanation_or_tutorial_button_to_active(OrganizeData_tutorial_button);
            UserInterface_organize_data.Tutorial_button_pressed(Mbco_enrichment_pipeline.Options.Next_ontology);
            UserInterface_organize_data.Set_explanation_and_tutorial_buttons_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
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
            Override_used_selected_column_names_enum override_user_selected_column_names = Override_used_selected_column_names_enum.No;
            if (UserInterface_loadExamples.Nog_cbButton.Checked)
            {
                this.Read_directoryOrFile_ownTextBox.SilentText = global_dirFile.Get_custom_data_nog_directory();
                this.ResultsDirectory_textBox.SilentText = global_dirFile.Get_custom_results_nog_directory();
                override_user_selected_column_names = Override_used_selected_column_names_enum.Default_custom_2_names;

                Custom_data.Options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Value_1st_cutoff = (float)Math.Log(1.3F, 2);
                Custom_data.Options.Value_2nd_cutoff = -(float)Math.Log10(0.05);
                Custom_data.Options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
                Custom_data.Options.Keep_top_ranks = 99999;
                Custom_data.Options.Merge_upDown_before_ranking = true;
                Custom_data.Options.All_genes_significant = true;

                Mbco_enrichment_pipeline.Options.Set_next_ontology_and_organism(Ontology_type_enum.Mbco, Organism_enum.Mus_musculus);
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
                Mbco_enrichment_pipeline.Options.Timeline_pvalue_cutoff = 0.05F;

                Timeline_diagram.Options.Generate_timeline = true;
                Timeline_diagram.Options.Customized_colors = false;
                Timeline_diagram.Options.Is_logarithmic_time_axis = false;

                Bardiagram.Options.Generate_bardiagrams = true;
                Bardiagram.Options.Customized_colors = Timeline_diagram.Options.Customized_colors;

                Heatmap.Options.Generate_heatmap = false;
                Heatmap.Options.Value_type_selected_for_visualization = Enrichment_value_type_enum.Fractional_rank;
                Heatmap.Options.Show_significant_scps_over_all_conditions = true;

                Mbco_network_integration.Options.Set_next_ontology_and_organism(Mbco_enrichment_pipeline.Options.Next_ontology, Mbco_enrichment_pipeline.Options.Next_organism);
                Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks = true;
                Mbco_network_integration.Options.Add_genes_to_standard_networks = false;
                Mbco_network_integration.Options.Add_edges_that_connect_standard_scps = false;
                Mbco_network_integration.Options.Add_additional_edges_that_connect_dynamic_scps = false;
                Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_network_integration.Options.Add_parent_child_relationships_to_dynamic_SCP_networks = false;
                Mbco_network_integration.Options.Add_genes_to_dynamic_networks = false;
                Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.Minus_log10_pvalue;
                Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant = 150;
                Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant = 50;
                Mbco_network_integration.Options.Label_maxSize_for_current_nodeSize_determinant = 70;
                Mbco_network_integration.Options.Label_uniqueSize_for_current_nodeSize_determinant = 50;
                //Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node
                //_size_determinant_enum.Uniform;
                //Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant = 50;
                Mbco_network_integration.Options.Adjust_labelSizes_to_nodeSizes = true;
                Mbco_network_integration.Options.Node_size_scaling_across_plots = Node_size_scaling_across_plots_enum.Unique;
                Mbco_network_integration.Options.Generate_scp_networks = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_dynamic_enrichment = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_standard_enrichment = false;

                read_data_and_update_dataset_interface = true;

            }
            else if (UserInterface_loadExamples.Kpmp_cbButton.Checked)
            {
                this.Read_directoryOrFile_ownTextBox.SilentText = global_dirFile.Get_custom_data_kpmp_directory();
                this.ResultsDirectory_textBox.SilentText = global_dirFile.Get_custom_results_kpmp_directory();
                override_user_selected_column_names = Override_used_selected_column_names_enum.Default_custom_1_names;

                Custom_data.Options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Value_1st_cutoff = 0;
                Custom_data.Options.Value_2nd_cutoff = -(float)Math.Log10(0.05);
                Custom_data.Options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
                Custom_data.Options.Keep_top_ranks = 300;
                Custom_data.Options.Merge_upDown_before_ranking = true;
                Custom_data.Options.All_genes_significant = true;

                Mbco_enrichment_pipeline.Options.Set_next_ontology_and_organism(Ontology_type_enum.Mbco, Organism_enum.Homo_sapiens);
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level = new int[] { -1, 5, 5, 10, 5 };
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level = new int[] { -1, -1, 5, 7, -1 };
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment = 0.05F;
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment = 0.05F;
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

                Mbco_network_integration.Options.Set_next_ontology_and_organism(Mbco_enrichment_pipeline.Options.Next_ontology, Mbco_enrichment_pipeline.Options.Next_organism);
                Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks = true;
                Mbco_network_integration.Options.Add_genes_to_standard_networks = false;
                Mbco_network_integration.Options.Add_edges_that_connect_standard_scps = false;
                Mbco_network_integration.Options.Add_additional_edges_that_connect_dynamic_scps = true;
                Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_network_integration.Options.Add_parent_child_relationships_to_dynamic_SCP_networks = false;
                Mbco_network_integration.Options.Add_genes_to_dynamic_networks = false;
                //Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.Minus_log10_pvalue;
                //Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant = 200;
                Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.No_of_different_colors;
                Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant = 150;
                Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant = 50;
                Mbco_network_integration.Options.Label_maxSize_for_current_nodeSize_determinant = 70;
                Mbco_network_integration.Options.Label_uniqueSize_for_current_nodeSize_determinant = 50;
                Mbco_network_integration.Options.Adjust_labelSizes_to_nodeSizes = true;
                Mbco_network_integration.Options.Node_size_scaling_across_plots = Node_size_scaling_across_plots_enum.Unique;
                Mbco_network_integration.Options.Generate_scp_networks = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_dynamic_enrichment = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_standard_enrichment = false;

                read_data_and_update_dataset_interface = true;
            }
            else if (UserInterface_loadExamples.Dtoxs_cbButton.Checked)
            {
                this.Read_directoryOrFile_ownTextBox.SilentText = global_dirFile.Get_custom_data_dtoxs_directory();
                this.ResultsDirectory_textBox.SilentText = global_dirFile.Get_custom_results_DToxS_directory();
                override_user_selected_column_names = Override_used_selected_column_names_enum.Example_dataset_no;
                UserInterface_read.SetTo_mbco_button_clicked();

                Custom_data.Options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                Custom_data.Options.Value_1st_cutoff = 0;
                Custom_data.Options.Value_2nd_cutoff = 0;
                Custom_data.Options.Value_importance_order = Value_importance_order_enum.Value_1st_2nd;
                Custom_data.Options.Keep_top_ranks = 600;
                Custom_data.Options.Merge_upDown_before_ranking = true;
                Custom_data.Options.All_genes_significant = true;

                Mbco_enrichment_pipeline.Options.Set_next_ontology_and_organism(Ontology_type_enum.Go_bp, Organism_enum.Homo_sapiens);
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level = new int[] { -1, 5, 5, 10, 5 };
                Mbco_enrichment_pipeline.Options.Set_go_min_size(Ontology_type_enum.Go_bp, 5);
                Mbco_enrichment_pipeline.Options.Set_go_max_size(Ontology_type_enum.Go_bp, 360);
                Mbco_enrichment_pipeline.Options.Set_next_ontology_and_organism(Ontology_type_enum.Mbco, Organism_enum.Homo_sapiens);
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_standardEnrichment_per_level = new int[] { -1, 5, 5, 10, 5 };
                Mbco_enrichment_pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level = new int[] { -1, -1, 3, 5, -1 };
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_standardEnrichment = 0.05F;
                Mbco_enrichment_pipeline.Options.Max_pvalue_for_dynamicEnrichment = 0.05F;
                Mbco_enrichment_pipeline.Options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_enrichment_pipeline.Options.Show_all_and_only_selected_scps = false;
                Mbco_enrichment_pipeline.Options.Group_selectedScps_dict = new Dictionary<string, string[]>();
                Mbco_enrichment_pipeline.Options.OwnScp_mbcoSubScps_dict = new Dictionary<string, string[]>();
                Mbco_enrichment_pipeline.Options.OwnScp_level_dict = new Dictionary<string, int>();
                Mbco_enrichment_pipeline.Options.Timeline_pvalue_cutoff = 0.05F;

                Timeline_diagram.Options.Generate_timeline = false;
                Timeline_diagram.Options.Customized_colors = true;

                Bardiagram.Options.Generate_bardiagrams = true;
                Bardiagram.Options.Customized_colors = Timeline_diagram.Options.Customized_colors;

                Heatmap.Options.Generate_heatmap = false;
                Heatmap.Options.Value_type_selected_for_visualization = Enrichment_value_type_enum.Fractional_rank;
                Heatmap.Options.Show_significant_scps_over_all_conditions = true;

                Mbco_network_integration.Options.Set_next_ontology_and_organism(Mbco_enrichment_pipeline.Options.Next_ontology, Mbco_enrichment_pipeline.Options.Next_organism);
                Mbco_network_integration.Options.Add_parent_child_relationships_to_standard_SCP_networks = true;
                Mbco_network_integration.Options.Add_genes_to_standard_networks = false;
                Mbco_network_integration.Options.Add_edges_that_connect_standard_scps = false;
                Mbco_network_integration.Options.Add_additional_edges_that_connect_dynamic_scps = true;
                Mbco_network_integration.Options.Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
                Mbco_network_integration.Options.Add_parent_child_relationships_to_dynamic_SCP_networks = false;
                Mbco_network_integration.Options.Add_genes_to_dynamic_networks = false;
                //Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.Minus_log10_pvalue;
                //Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant = 200;
                Mbco_network_integration.Options.Node_size_determinant = yed_network.Yed_network_node_size_determinant_enum.Minus_log10_pvalue;
                Mbco_network_integration.Options.Node_size_diameterMax_for_current_nodeSize_determinant = 150;
                Mbco_network_integration.Options.Label_minSize_for_current_nodeSize_determinant = 50;
                Mbco_network_integration.Options.Label_maxSize_for_current_nodeSize_determinant = 70;
                Mbco_network_integration.Options.Label_uniqueSize_for_current_nodeSize_determinant = 50;
                Mbco_network_integration.Options.Adjust_labelSizes_to_nodeSizes = true;
                Mbco_network_integration.Options.Node_size_scaling_across_plots = Node_size_scaling_across_plots_enum.Unique;
                Mbco_network_integration.Options.Generate_scp_networks = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_dynamic_enrichment = true;
                Mbco_network_integration.Options.Box_sameLevel_scps_for_standard_enrichment = false;

                read_data_and_update_dataset_interface = true;
            }
            if (read_data_and_update_dataset_interface)
            {
                Update_all_visualized_options_in_menu_panels();
                Update_acknowledgment_and_application_headline();

                Read_order_allFilesInDirectory_cbButton.Button_switch_to_positive();
                UserInterface_read.Read_allFilesInDirectory_ownCheckedBox_clicked();

                Color[] selectable_colors = Default_textBox_texts.Get_priority_and_remaining_colors();
                string defaultIntegrationGroup = DatasetSummary_userInterface.Get_default_integrationGroup_from_lastSaved_datasetSummaries();
                string[] parameter_settings;
                this.Custom_data = UserInterface_read.Read_button_click(this.Custom_data, defaultIntegrationGroup, selectable_colors, override_user_selected_column_names, out parameter_settings);
                BgGenes_addReadAllFilesInDirectory_cbButton.Checked = true;

                string[] mbco_background_genes = Mbco_enrichment_pipeline.Get_all_symbols_of_any_SCPs_after_updating_instance_if_ontology_unequals_next_ontology(ProgressReport);
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
        }
        private void LoadExamples_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] current_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_loadExamples.Set_explanation_and_tutorial_buttons_to_inactive();
            UserInterface_loadExamples.Set_explanation_or_tutorial_button_to_active(LoadExamples_tutorial_button);
            UserInterface_loadExamples.Tutorial_button_pressed();
            UserInterface_loadExamples.Set_explanation_and_tutorial_buttons_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(current_visible_dataset_attributes);

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
        private void LoadExamples_DToxS_cbButton_Click(object sender, EventArgs e)
        {
            LoadExamples_dtoxs_cbButton.Button_pressed();
            UserInterface_loadExamples.Dtoxs_checkBox_CheckedChanged();
        }

        #endregion

        #region SigData
        private void SigData_directionValue1st_ownTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.DirectionValue1st_ownListBox_SelectedIndexChanged(Custom_data.Options);
        }
        private void SigData_directionValue2nd_ownTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.DirectionValue2nd_ownListBox_SelectedIndexChanged(Custom_data.Options);
        }
        private void SigData_value1st_cutoff_textBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.Value1st_cutoff_textBox_TextChanged(Custom_data.Options);
        }
        private void SigData_value2nd_cutoff_textBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.Value2nd_cutoff_textBox_TextChanged(Custom_data.Options);
        }
        private void SigData_rankByValue_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.RankByValue_ownListBox_SelectedIndexChanged(Custom_data.Options);
        }
        private void SigData_defineDataset_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_sigData.DefineDataset_ownListBox_SelectedIndexChanged(Custom_data.Options);
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
        private void SigData_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_sigData.Set_selected_tutorial_button_to_active(SigData_tutorial_button);
            UserInterface_sigData.Tutorial_button_activated();
            UserInterface_sigData.Set_tutorial_button_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
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
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Add_groupButton_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
        }
        private void SelectSCPs_removeGroup_button_Click(object sender, EventArgs e)
        {
            bool addGenes_to_standard_network = Mbco_network_integration.Options.Add_genes_to_standard_networks;
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Remove_groupButton_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, ref addGenes_to_standard_network);
            Mbco_network_integration.Options.Add_genes_to_standard_networks = addGenes_to_standard_network;
        }
        private void SelectSCPs_groups_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.Groups_ownListBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
        }
        private void SelectSCPs_add_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Add_button_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
        }
        private void SelectSCPs_remove_button_Click(object sender, EventArgs e)
        {
            bool addGenes_to_standard_network = Mbco_network_integration.Options.Add_genes_to_standard_networks;
            Mbco_enrichment_pipeline.Options = UserInterface_selectSCPs.Remove_button_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options, ref addGenes_to_standard_network);
            Mbco_network_integration.Options.Add_genes_to_standard_networks = addGenes_to_standard_network;
        }
        private void SelectSCPs_sortSCPs_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.SortSCPs_listBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
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
            UserInterface_selectSCPs.MBCO_listBox_changed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
        }
        private void SelectScps_selectedSCPs_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_selectSCPs.Selected_listBox_changed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
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
            UserInterface_selectSCPs.Write_mbco_yed_network(Mbco_network_integration.Options.Graph_editor);
        }
        private void SelectedScps_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_selectSCPs.Set_tutorial_button_to_inactive();
            UserInterface_selectSCPs.Set_tutorial_button_to_active(SelectedScps_tutorial_button);
            UserInterface_selectSCPs.Tutorial_button_pressed(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
            UserInterface_selectSCPs.Set_tutorial_button_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
        }
        #endregion

        #region Define scps
        private void DefineScps_sort_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.Sort_listBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options);
        }
        private void DefineScps_addNewOwnSCP_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.AddNewOwnSCP_button_Click(Mbco_enrichment_pipeline.Options);
        }
        private void DefineScps_removeOwnSCP_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.RemoveOwnSCP_button_Click(Mbco_enrichment_pipeline.Options);
        }
        private void DefineScps_selectOwnScp_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.SelectOwnScp_ownListBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options);
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
            UserInterface_defineSCPs.MBCO_listBox_changed(Mbco_enrichment_pipeline.Options);
        }
        private void DefineScps_addSubScp_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.AddSubScp_button_Click(Mbco_enrichment_pipeline.Options);
        }
        private void DefineScps_removeSubScp_button_Click(object sender, EventArgs e)
        {
            Mbco_enrichment_pipeline.Options = UserInterface_defineSCPs.RemoveSubScp_button_Click(Mbco_enrichment_pipeline.Options);
        }
        private void DefineSCPs_writeMbcoHierarchy_button_Click(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.Write_mbco_yed_network_and_return_if_interrupted(Mbco_network_integration.Options.Graph_editor);
        }
        private void DefineScps_ownSubScps_ownListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInterface_defineSCPs.OwnSubScps_listBox_changed(Mbco_enrichment_pipeline.Options);
        }
        private void DefineSCPs_tutorial_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_defineSCPs.Set_tutorial_button_to_inactive();
            UserInterface_defineSCPs.Set_tutorial_button_to_active(DefineSCPs_tutorial_button);
            UserInterface_defineSCPs.Tutorial_button_pressed(Mbco_enrichment_pipeline.Options);
            UserInterface_defineSCPs.Set_tutorial_button_to_inactive();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
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
        private void Identify_suited_fontSize_for_tutorial_text_boxes()
        {
            int test_top_y_position = DatasetInterface_overall_panel.Location.Y + Input_geneList_textBox.Location.Y + (int)Math.Round(0.4F * Input_geneList_textBox.Height);
            int test_left_x_position = DatasetInterface_overall_panel.Location.X + Input_geneList_textBox.Location.X + Input_geneList_textBox.Width + (int)Math.Round(0.4F * Input_geneList_textBox.Width);
            int test_height = (int)Math.Round(0.07F * this.Height);
            int test_width = (int)Math.Round(0.4F * this.Width);

            UserInterface_tutorial.Identify_suited_tutorial_fontSize(test_left_x_position, test_top_y_position, test_width, test_height, Global_class.Do_internal_checks);
            if (Global_class.Do_internal_checks)
            {
                UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out bool escape_pressed, out bool back_arrow_pressed);
            }
            UserInterface_tutorial.Reset_after_tour_finished();
        }
        private void Update_graphic_parameter_overallApplicationSize_and_adjust_all_graphic_elements()
        {
            Set_options_buttons_progressReport_and_panel_visibilities_to_default_except_specified_panels();
            this.Form_default_settings.Update_parameter();
            this.Visible = false;
            this.Update_overallApplicationSize();
            this.ProgressReport.Update_all_graphic_elements();
            this.Form_default_settings.Update_all_graphic_elements_in_applicationSize_panel();
            this.Form_default_settings.Update_applicationSize_textBoxes_and_listBoxes();
            this.UserInterface_read.Update_all_graphic_elements();
            this.UserInterface_sigData.Update_all_graphic_elements(Custom_data.Options);
            this.UserInterface_organize_data.Update_all_graphic_elements();
            this.UserInterface_ontology.Update_all_graphic_elements();
            this.UserInterface_enrichmentOptions.Update_all_graphic_elements(Mbco_enrichment_pipeline.Options);
            this.UserInterface_bgGenes.Update_all_graphic_elements(Custom_data);
            this.UserInterface_defineSCPs.Update_all_graphic_elements();
            this.UserInterface_scp_networks.Update_graphic_elements(Mbco_network_integration.Options);
            this.UserInterface_selectSCPs.Update_all_graphic_elements();
            this.UserInterface_loadExamples.Update_all_graphics_elements();
            this.UserInterface_tips.Update_all_graphic_elements();
            this.DatasetSummary_userInterface.Update_all_graphic_elements(Custom_data);
            this.UserInterface_results.Update_all_graphic_elements();
            this.Update_all_option_menu_buttons();
            this.Update_all_graphic_elements_of_shared_tools();
            this.Visible = true;
            Identify_suited_fontSize_for_tutorial_text_boxes();
        }
        private void AppSize_resize_button_Click(object sender, EventArgs e)
        {
            //this.Form_default_settings.
            int current_height_percentage = Form_default_settings.Current_height_percentage;
            int current_width_percentage = Form_default_settings.Current_width_percentage;
            try
            {
                Update_graphic_parameter_overallApplicationSize_and_adjust_all_graphic_elements();
            }
            catch
            {
                Form_default_settings.Get_selected_heigth_and_width_percentage_from_textBoxes(out int failed_percentage_of_height, out int failed_percentage_of_width);
                Form_default_settings.Update_current_height_and_width_min_and_max_percentages(failed_percentage_of_height, failed_percentage_of_width);
                this.AppSize_height_textBox.SilentText = current_height_percentage.ToString();
                this.AppSize_width_textBox.SilentText = current_width_percentage.ToString();
                Update_graphic_parameter_overallApplicationSize_and_adjust_all_graphic_elements();
                ProgressReport.Update_progressReport_text_and_visualization("Size selections not possible, restored to previous values.");
            }
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

        #region Tips
        private void Tips_demonstration_cbButton_Click(object sender, EventArgs e)
        {
            Tips_demonstration_cbButton.Button_pressed();
            UserInterface_tips.Update_demonstration_cbLabel();
        }
        private void Tips_forward_cbButton_Click(object sender, EventArgs e)
        {
            UserInterface_tips.Forward_or_backward_button_pressed(1);
        }
        private void Tips_backward_cbButton_Click(object sender, EventArgs e)
        {
            UserInterface_tips.Forward_or_backward_button_pressed(-1);
        }
        private void Tips_write_mbco_hierarchy_Click(object sender, EventArgs e)
        {
            UserInterface_tips.Write_mbco_hierarchy_button_pressed(Mbco_network_integration.Options.Graph_editor);
        }

        #endregion

        #region Ontology userInterface
        protected bool Update_nextOntology_and_nextOrganism_from_outside_ontologyOrganismUserInterface_if_not_empty_and_if_current_selections_are_valid_and_return_eventual_error_messages(Ontology_type_enum selected_ontology, Organism_enum selected_organism, out string error_messages)
        {
            bool updated = false;
            bool no_update_necessary = true;
            string ontology_listBox_entry;
            string organism_listBox_entry;
            StringBuilder sb_progress_report = new StringBuilder();
            if ((!selected_ontology.Equals(Ontology_type_enum.E_m_p_t_y))
                && (selected_organism.Equals(Organism_enum.E_m_p_t_y)))
            {
                no_update_necessary = false;
                ontology_listBox_entry = UserInterface_ontology.Get_listBox_entry_for_ontology(selected_ontology);
                Ontology_ontology_listBox.SilentSelectedIndex = Ontology_ontology_listBox.Items.IndexOf(ontology_listBox_entry);
                Ontology_ontology_listBox_SelectedIndexChanged_action();
                if (Mbco_enrichment_pipeline.Options.Next_ontology.Equals(selected_ontology))
                { 
                    updated = true;
                }
                else
                {
                    if (sb_progress_report.Length>0) { sb_progress_report.Append("; "); }
                    sb_progress_report.AppendFormat(ProgressReport.Get_deep_copy_of_progressReport_text());
                }
            }
            else if ((!selected_organism.Equals(Organism_enum.E_m_p_t_y))
                     && (selected_ontology.Equals(Ontology_type_enum.E_m_p_t_y)))
            {
                no_update_necessary = false;
                organism_listBox_entry = UserInterface_ontology.Get_listBox_entry_for_organism(selected_organism);
                Ontology_organism_listBox.SilentSelectedIndex = Ontology_organism_listBox.Items.IndexOf(organism_listBox_entry);
                Ontology_organism_listBox_SelectedIndexChanged_action();
                if (Mbco_enrichment_pipeline.Options.Next_organism.Equals(selected_organism)) { updated = true; }
                else
                {
                    if (sb_progress_report.Length > 0) { sb_progress_report.Append("; "); }
                    sb_progress_report.AppendFormat(ProgressReport.Get_deep_copy_of_progressReport_text());
                }
            }
            else if ((!selected_ontology.Equals(Ontology_type_enum.E_m_p_t_y))
                     && (!selected_organism.Equals(Organism_enum.E_m_p_t_y)))
            {
                no_update_necessary = false;
                ontology_listBox_entry = UserInterface_ontology.Get_listBox_entry_for_ontology(selected_ontology);
                Ontology_ontology_listBox.SilentSelectedIndex = Ontology_ontology_listBox.Items.IndexOf(ontology_listBox_entry);
                organism_listBox_entry = UserInterface_ontology.Get_listBox_entry_for_organism(selected_organism);
                Ontology_organism_listBox.SilentSelectedIndex = Ontology_organism_listBox.Items.IndexOf(organism_listBox_entry);
                Ontology_organism_listBox_SelectedIndexChanged_action();
                if (  (Mbco_enrichment_pipeline.Options.Next_ontology.Equals(selected_ontology))
                    &&(Mbco_enrichment_pipeline.Options.Next_organism.Equals(selected_organism)))
                { 
                    updated = true;
                }
                else
                {
                    if (sb_progress_report.Length > 0) { sb_progress_report.Append("; "); }
                    sb_progress_report.AppendFormat(ProgressReport.Get_deep_copy_of_progressReport_text());
                }
            }
            if (updated)
            {
                UserInterface_ontology.Update_all_visualized_options_and_nw_toy_options(Mbco_enrichment_pipeline.Options);
            }
            error_messages = sb_progress_report.ToString();
            return updated || no_update_necessary;
        }
        private void Ontology_ontology_listBox_SelectedIndexChanged_action()
        {
            UserInterface_ontology.Ontology_or_organism_listBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
            Update_acknowledgment_and_application_headline();
        }
        private void Ontology_ontology_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ontology_ontology_listBox_SelectedIndexChanged_action();
        }
        private void Ontology_organism_listBox_SelectedIndexChanged_action()
        {
            UserInterface_ontology.Ontology_or_organism_listBox_SelectedIndexChanged(Mbco_enrichment_pipeline.Options, Mbco_network_integration.Options);
        }
        private void Ontology_organism_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ontology_organism_listBox_SelectedIndexChanged_action();
        }
        private void Ontology_tour_button_Click(object sender, EventArgs e)
        {
            Dataset_attributes_enum[] currently_visible_dataset_attributes = Set_main_panel_visibilities_to_tutorial_mode_and_return_currently_visible_dataset_attributes();
            UserInterface_ontology.Set_tour_button_to_not_selected();
            UserInterface_ontology.Set_selected_tour_button_to_activated(Ontology_tour_button);
            UserInterface_ontology.Tour_button_activated(Mbco_enrichment_pipeline.Options);
            UserInterface_ontology.Set_tour_button_to_not_selected();
            Restore_main_panel_visibilities_after_tutorial(currently_visible_dataset_attributes);
        }
        private void Ontology_writeHierarchy_button_Click(object sender, EventArgs e)
        {
            Update_parentChild_hierarchies_in_selectScps_defineScps_ontology_and_tips_if_necessary();
            UserInterface_ontology.Write_mbco_hierarchy_button_pressed(Mbco_network_integration.Options.Graph_editor);
        }
        private void Ontology_write_scpInteractions_button_Click(object sender, EventArgs e)
        {
            Update_parentChild_hierarchies_in_selectScps_defineScps_ontology_and_tips_if_necessary();
            UserInterface_ontology.Write_scp_interactions_button_pressed(Mbco_network_integration.Options.Graph_editor);
        }
        private void Ontology_topScpInteractions_level2_textBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_ontology.TopPercentScpsLevel_x_SCPs_textBox_TextChanged(2);
        }
        private void Ontology_topScpInteractions_level3_textBox_TextChanged(object sender, EventArgs e)
        {
            UserInterface_ontology.TopPercentScpsLevel_x_SCPs_textBox_TextChanged(3);
        }
        #endregion

        protected void Update_text_in_progressReport(string text)
        {
            this.ProgressReport.Update_progressReport_text_and_visualization(text);
        }
        protected void Delete_all_existing_analysis_finished_fileNames(params string[] additional_directories)
        {
            string results_directory = (string)this.ResultsDirectory_textBox.Text.Clone();
            additional_directories = Overlap_class.Get_ordered_union_of_string_arrays(additional_directories, new string[] { results_directory });
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string[] complete_analysis_finished_fileNames = gdf.Get_all_analysis_finished_complete_fileNames(additional_directories);
            foreach (string complete_analysis_finished_fileName in complete_analysis_finished_fileNames)
            {
                if (System.IO.File.Exists(complete_analysis_finished_fileName))
                {
                    System.IO.File.Delete(complete_analysis_finished_fileName);
                }
            }
        }
        protected void Write_all_analysis_finished_fileNames(out bool file_opened_successful, params string[] additional_directories)
        {
            string results_directory = (string)this.ResultsDirectory_textBox.Text.Clone();
            additional_directories = Overlap_class.Get_ordered_union_of_string_arrays(additional_directories, new string[] { results_directory });
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string[] complete_analysis_finished_fileNames = gdf.Get_all_analysis_finished_complete_fileNames(additional_directories);
            file_opened_successful = true;
            foreach (string complete_analysis_finished_fileName in complete_analysis_finished_fileNames)
            {
                ReadWriteClass.WriteArray<string>(new string[] { "Analysis finished." }, complete_analysis_finished_fileName, ProgressReport, out bool file_opened_successful_local);
                if (!file_opened_successful_local) { file_opened_successful = false; }
            }
        }
        protected void Delete_all_existing_commandLineErrorReport_fileNames(params string[] additional_directories)
        {
            string results_directory = (string)this.ResultsDirectory_textBox.Text.Clone();
            additional_directories = Overlap_class.Get_ordered_union_of_string_arrays(additional_directories, new string[] { results_directory });
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string[] complete_commandLineErrorReport_fileNames = gdf.Get_all_command_line_error_report_complete_fileNames(additional_directories);
            foreach (string complete_commandLineErrorReport_fileName in complete_commandLineErrorReport_fileNames)
            {
                if (System.IO.File.Exists(complete_commandLineErrorReport_fileName))
                {
                    System.IO.File.Delete(complete_commandLineErrorReport_fileName);
                }
            }
        }
        protected void Write_all_commandLineErrorReport_fileNames(string text, params string[] additional_directories)
        {
            string results_directory = (string)this.ResultsDirectory_textBox.Text.Clone();
            additional_directories = Overlap_class.Get_ordered_union_of_string_arrays(additional_directories, new string[] { results_directory });
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string[] complete_analysis_finished_fileNames = gdf.Get_all_command_line_error_report_complete_fileNames(additional_directories);
            string[] array = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            foreach (string complete_analysis_finished_fileName in complete_analysis_finished_fileNames)
            {
                ReadWriteClass.WriteArray<string>(array, complete_analysis_finished_fileName, ProgressReport, out bool fileNames_opened_successsful);
            }
        }

        protected void Update_results_directory(string new_results_directory)
        {
            this.ResultsDirectory_textBox.SilentText_and_refresh = (string)new_results_directory.Clone();
        }
        private void Results_addResultsToControl_cbButton_Click(object sender, EventArgs e)
        {
            Results_addResultsToControl_cbButton.Button_pressed();
            UserInterface_results.Set_visibility_of_checkBoxes_and_labels();
        }
    }

    public partial class Mbco_user_application_form1 : Mbco_user_application_form
    {
        public Mbco_user_application_form1() : base()
        { }

        private StringBuilder Add_text_to_sb_errors(StringBuilder sb_errors, string text, bool start_new_line)
        {
            if (sb_errors.Length > 0)
            {
                if (start_new_line) { sb_errors.Append("\n"); }
                else { sb_errors.Append(" "); }
            }
            else
            {
                sb_errors.Append("Command line error(s): ");
            }
            sb_errors.Append(text);
            return sb_errors;
        }

        private bool Is_string_a_command_argument_or_was_already_encountered(string read_string, List<string> all_arguments_or_ecountered_strings)
        {
            return (all_arguments_or_ecountered_strings.Contains(read_string)) | (read_string.IndexOf("--")==0);
        }

        public Mbco_user_application_form1(string[] arguments) : base(false)
        {
            this.Visible = false;
            //System.Diagnostics.Debugger.Launch();
            List<string> identified_arguments = new List<string>();
            int arguments_length = arguments.Length;
            string argument;
            bool column_name_instructions_specified = false;
            bool analysis_successful = false;
            bool input_dir_argument_specified = false;
            bool error_encountered = false;
            string input_directory = "error";
            string non_results_directory_specified_label = "none";
            string results_directory = (string)non_results_directory_specified_label.Clone();
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            StringBuilder sb_error_txtFile = new StringBuilder();
            StringBuilder sb_error_progressBar = new StringBuilder();
            bool error_textFile_startNewLineForEachError = true;
            bool error_progressBar_startNewLineForEachError = false;
            List<string> all_arguments_and_encountered_strings = new List<string>();
            string input_dir_argument = "--input-dir";
            all_arguments_and_encountered_strings.Add(input_directory);
            string results_dir_argument = "--results-dir";
            all_arguments_and_encountered_strings.Add(results_dir_argument);
            string species_argument = "--species";
            all_arguments_and_encountered_strings.Add(species_argument);
            string organism_argument = "--organism";
            all_arguments_and_encountered_strings.Add(organism_argument);
            string ontology_argument = "--ontology";
            all_arguments_and_encountered_strings.Add(ontology_argument);
            string mbco_columnNames_argument = "--mbco-column-names";
            all_arguments_and_encountered_strings.Add(mbco_columnNames_argument);
            string singleCell_columnNames_argument = "--single-cell-column-names";
            all_arguments_and_encountered_strings.Add(singleCell_columnNames_argument);
            string minimum_columnNames_argument = "--minimum-column-names";
            all_arguments_and_encountered_strings.Add(minimum_columnNames_argument);
            string optimum_columnNames_argument = "--optimum-column-names";
            all_arguments_and_encountered_strings.Add(optimum_columnNames_argument);
            string custom_colmumnNames_1_argument = "--custom-1-column-names";
            all_arguments_and_encountered_strings.Add(custom_colmumnNames_1_argument);
            string custom_colmumnNames_2_argument = "--custom-2-column-names";
            all_arguments_and_encountered_strings.Add(custom_colmumnNames_2_argument);

            string argument_inLowerCase;

            //System.Diagnostics.Debugger.Launch();

            for (int indexA = 0; indexA < arguments_length; indexA++)
            {
                argument = arguments[indexA];
                argument_inLowerCase = argument.ToLower();
                if (string.Equals(argument_inLowerCase, input_dir_argument))
                {
                    input_dir_argument_specified = true;
                    identified_arguments.Add(argument);
                    if ((indexA <= arguments_length - 2) && (!Is_string_a_command_argument_or_was_already_encountered(arguments[indexA + 1], all_arguments_and_encountered_strings)))
                    {
                        indexA++;
                        input_directory = arguments[indexA];
                        identified_arguments.Add(input_directory);
                        if (!Directory.Exists(input_directory))
                        {
                            Add_text_to_sb_errors(sb_error_progressBar, "Given '" + input_dir_argument + "' not found: " + input_directory + ".", false);
                            error_encountered = true;
                            all_arguments_and_encountered_strings.Add(input_directory);
                        }
                        else
                        {
                            input_directory = gdf.Transform_into_compatible_directory_and_clean_up(input_directory);
                            base.Set_input_data_directory(input_directory);
                        }
                    }
                    else
                    {
                        Add_text_to_sb_errors(sb_error_progressBar, "'" + input_dir_argument + "' is not followed by directory.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                }
                if (string.Equals(argument_inLowerCase, results_dir_argument))
                {
                    identified_arguments.Add(argument);
                    if ((indexA <= arguments_length - 2) && (!Is_string_a_command_argument_or_was_already_encountered(arguments[indexA + 1], all_arguments_and_encountered_strings)))
                    {
                        indexA++;
                        results_directory = arguments[indexA];
                        identified_arguments.Add(results_directory);
                        results_directory = gdf.Transform_into_compatible_directory_and_clean_up(results_directory);
                    }
                    else
                    {
                        Add_text_to_sb_errors(sb_error_progressBar, "'" + results_dir_argument + "' is not followed by directory.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                }
            }
            if (!input_dir_argument_specified)
            {
                Add_text_to_sb_errors(sb_error_progressBar, "'" + input_dir_argument + "' argument is missing.", error_progressBar_startNewLineForEachError);
                error_encountered = true;
            }

            #region Search for organism argument and organism
            string selected_organism_string;
            string selected_organism_string_in_lowerCase;
            Organism_enum[] available_organisms = (Organism_enum[])Enum.GetValues(typeof(Organism_enum));
            available_organisms = Overlap_class.Get_part_of_list1_but_not_of_list2(available_organisms, Organism_enum.E_m_p_t_y);
            Organism_enum selected_organism = Organism_enum.E_m_p_t_y;
            //System.Diagnostics.Debugger.Launch();
            Dictionary<string, Organism_enum> string_organismEnum_dict = Ontology_classification_class.Get_organismString_organismEnum_dict();
            for (int indexA = 0; indexA < arguments_length; indexA++)
            {
                argument = arguments[indexA];
                argument_inLowerCase = argument.ToLower();
                if (  (string.Equals(argument_inLowerCase, species_argument))
                    ||(string.Equals(argument_inLowerCase, organism_argument)))
                {
                    identified_arguments.Add(argument);
                    if ((indexA <= arguments_length - 2) && (!Is_string_a_command_argument_or_was_already_encountered(arguments[indexA + 1], all_arguments_and_encountered_strings)))
                    {
                        indexA++;
                        selected_organism_string = arguments[indexA];
                        selected_organism_string_in_lowerCase = selected_organism_string.ToLower();
                        identified_arguments.Add(selected_organism_string);
                        if (string_organismEnum_dict.ContainsKey(selected_organism_string_in_lowerCase))
                        {
                            selected_organism = string_organismEnum_dict[selected_organism_string_in_lowerCase];
                        }
                        else
                        {
                            Add_text_to_sb_errors(sb_error_progressBar, "Given '" + species_argument + "' is not selectable: '" + selected_organism_string+ "'.", error_progressBar_startNewLineForEachError);
                            error_encountered = true;
                            all_arguments_and_encountered_strings.Add(selected_organism_string);
                        }
                    }
                    else
                    {
                        Add_text_to_sb_errors(sb_error_progressBar, "No species specified after '" + species_argument + "'.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                }
            }
            #endregion

            #region Search for ontology argument and ontology
            string selected_ontology_string_in_lowerCase;
            string selected_ontology_string;
            Ontology_type_enum[] available_ontologies = (Ontology_type_enum[])Enum.GetValues(typeof(Ontology_type_enum));
            available_ontologies = Overlap_class.Get_part_of_list1_but_not_of_list2<Ontology_type_enum>(available_ontologies, Ontology_type_enum.E_m_p_t_y);
            Ontology_type_enum selected_ontology = Ontology_type_enum.E_m_p_t_y;
            Dictionary<string, Ontology_type_enum> ontologyName_ontology_dict = Ontology_classification_class.Get_ontologyName_ontology_dict();
            //System.Diagnostics.Debugger.Launch();
            for (int indexA = 0; indexA < arguments_length; indexA++)
            {
                argument = arguments[indexA];
                argument_inLowerCase = argument.ToLower();
                if (string.Equals(argument_inLowerCase, ontology_argument))
                {
                    identified_arguments.Add(argument);
                    if ((indexA <= arguments_length - 2) && (!Is_string_a_command_argument_or_was_already_encountered(arguments[indexA + 1], all_arguments_and_encountered_strings)))
                    {
                        indexA++;
                        selected_ontology_string = arguments[indexA];
                        selected_ontology_string_in_lowerCase = selected_ontology_string.ToLower();
                        identified_arguments.Add(selected_ontology_string);
                        if (ontologyName_ontology_dict.ContainsKey(selected_ontology_string_in_lowerCase))
                        {
                            selected_ontology = ontologyName_ontology_dict[selected_ontology_string_in_lowerCase];
                        }
                        else
                        {
                            Add_text_to_sb_errors(sb_error_progressBar, "Given '" + ontology_argument + "' is not selectable: '" + selected_ontology_string + "'.", error_progressBar_startNewLineForEachError);
                            error_encountered = true;
                            all_arguments_and_encountered_strings.Add(selected_ontology_string);
                        }
                    }
                    else
                    {
                        Add_text_to_sb_errors(sb_error_progressBar, "No ontology specified after '" + ontology_argument + "'.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                }
            }
            #endregion


            Delete_all_existing_analysis_finished_fileNames(input_directory);
            //System.Diagnostics.Debugger.Launch();
            for (int indexA = 0; indexA < arguments_length; indexA++)
            {
                argument = arguments[indexA];
                argument_inLowerCase = argument.ToLower();
                int[] custom_columnNames_nos = new int[] {1,2};
                int custom_columnNames_nos_length = custom_columnNames_nos.Length;
                int custom_columnNames_no;
                string reference_columName;
                for (int indexCC = 0; indexCC < custom_columnNames_nos_length; indexCC++)
                {
                    custom_columnNames_no = custom_columnNames_nos[indexCC];
                    if (custom_columnNames_no==1) { reference_columName = custom_colmumnNames_1_argument; }
                    else if (custom_columnNames_no==2) { reference_columName = custom_colmumnNames_2_argument; }
                    else { throw new Exception("indexCC referes to not existing custom column names argument"); }
                    if (argument_inLowerCase.Equals(reference_columName))
                    {
                        identified_arguments.Add(argument);
                        if (column_name_instructions_specified)
                        {
                            if (sb_error_progressBar.Length > 0) { sb_error_progressBar.Append(" "); }
                            Add_text_to_sb_errors(sb_error_progressBar, "Selection of both custom column names 1 & 2 is not allowed.", error_progressBar_startNewLineForEachError);
                            error_encountered = true;
                        }
                        column_name_instructions_specified = true;
                        string custom_fileName = gdf.Get_customColumName_fileName(custom_columnNames_no);
                        string complete_custom_fileName = input_directory + custom_fileName;
                        if (!System.IO.File.Exists(complete_custom_fileName))
                        {
                            //System.Diagnostics.Debugger.Launch();
                            Add_text_to_sb_errors(sb_error_progressBar, "'" + custom_fileName + "' not found in data input directory.", error_progressBar_startNewLineForEachError);
                            error_encountered = true;
                        }
                        else
                        {
                            bool sucessful_upload = Read_custom_columNames_and_update_in_userInterface_readMenu_and_return_if_successful(input_directory, custom_columnNames_no);
                            if (!sucessful_upload)
                            {
                                Add_text_to_sb_errors(sb_error_progressBar, "Unable to read '" + custom_fileName + "'. File appears to be corrupted.", error_progressBar_startNewLineForEachError);
                                error_encountered = true;
                            }
                            switch (custom_columnNames_no)
                            {
                                case 1:
                                    Read_setToCustom1_button_Click(this, EventArgs.Empty);
                                    break;
                                case 2:
                                    Read_setToCustom2_button_Click(this, EventArgs.Empty);
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                    }
                }
                if (argument_inLowerCase.Equals(mbco_columnNames_argument))
                {
                    identified_arguments.Add(argument);
                    if (column_name_instructions_specified)
                    {
                        if (sb_error_progressBar.Length > 0) { sb_error_progressBar.Append(" "); }
                        Add_text_to_sb_errors(sb_error_progressBar, "Selection of multiple column name sets is not allowed.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                    column_name_instructions_specified = true;
                    Read_setToMBCO_button_Click(this, EventArgs.Empty);
                }
                else if (argument_inLowerCase.Equals(singleCell_columnNames_argument))
                {
                    identified_arguments.Add(argument);
                    if (column_name_instructions_specified)
                    {
                        if (sb_error_progressBar.Length > 0) { sb_error_progressBar.Append(" "); }
                        Add_text_to_sb_errors(sb_error_progressBar, "Selection of multiple column name sets is not allowed.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                    column_name_instructions_specified = true;
                    Read_setToSingleCell_button_Click(this, EventArgs.Empty);
                }
                else if (argument_inLowerCase.Equals(minimum_columnNames_argument))
                {
                    identified_arguments.Add(argument);
                    if (column_name_instructions_specified)
                    {
                        if (sb_error_progressBar.Length > 0) { sb_error_progressBar.Append(" "); }
                        Add_text_to_sb_errors(sb_error_progressBar, "Selection of multiple column name sets is not allowed.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                    column_name_instructions_specified = true;
                    Read_setToMinimum_button_Click(this, EventArgs.Empty);
                }
                else if (argument_inLowerCase.Equals(optimum_columnNames_argument))
                {
                    identified_arguments.Add(argument);
                    if (column_name_instructions_specified)
                    {
                        if (sb_error_progressBar.Length > 0) { sb_error_progressBar.Append(" "); }
                        Add_text_to_sb_errors(sb_error_progressBar, "Selection of multiple column name sets is not allowed.", error_progressBar_startNewLineForEachError);
                        error_encountered = true;
                    }
                    column_name_instructions_specified = true;
                    Read_setToOptimum_button_Click(this, EventArgs.Empty);
                }
            }
            Read_order_allFilesInDirectory_cbButton_Click(this, EventArgs.Empty);
            //System.Diagnostics.Debugger.Launch();
            bool current_ontolgy_and_organism_are_valid = Update_nextOntology_and_nextOrganism_from_outside_ontologyOrganismUserInterface_if_not_empty_and_if_current_selections_are_valid_and_return_eventual_error_messages(selected_ontology, selected_organism, out string potential_error_messages);
            if (!current_ontolgy_and_organism_are_valid)
            {
                Add_text_to_sb_errors(sb_error_progressBar, potential_error_messages, error_progressBar_startNewLineForEachError);
            }
            if (identified_arguments.Count <= arguments_length - 1)
            {
                string[] unknown_arguments = Overlap_class.Get_part_of_list1_but_not_of_list2_while_keeping_the_order(arguments, identified_arguments.ToArray());
                StringBuilder sb = new StringBuilder();
                int unknown_arguments_length = unknown_arguments.Length;
                string unknown_argument;
                //System.Diagnostics.Debugger.Launch();
                for (int indexU = 0; indexU < unknown_arguments_length; indexU++)
                {
                    unknown_argument = unknown_arguments[indexU];
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append(unknown_argument);
                }
                Add_text_to_sb_errors(sb_error_progressBar, "Unrecognized argument(s): " + sb.ToString() + ".", error_progressBar_startNewLineForEachError);
                error_encountered = true;
            }
            sb_error_txtFile.Append(sb_error_progressBar);
            Delete_all_existing_commandLineErrorReport_fileNames(input_directory);
            analysis_successful = false;
            if ((!error_encountered)&&(current_ontolgy_and_organism_are_valid))
            {
                //System.Diagnostics.Debugger.Launch();
                base.Update_text_in_progressReport("Starting automatic data upload and analysis, application will close if no errors are encountered.");
                if (results_directory.Equals(non_results_directory_specified_label)) { analysis_successful = ClearReadAnalyze_and_return_sucess_if_at_least_one_dataset_was_analyzed_successfully_or_no_data_was_analyzed_at_all(selected_ontology, selected_organism); }
                else { analysis_successful = ClearReadAnalyze_and_return_sucess_if_at_least_one_dataset_was_analyzed_successfully_or_no_data_was_analyzed_at_all(selected_ontology, selected_organism, results_directory); }
                bool read_data_errors = Is_readDataMenu_in_error_report_mode();

                if (read_data_errors)
                {
                    string reportOwnTextBox = "Errors encountered during data upload.";
                    string readData_error_summary = Get_deep_clone_of_entry_of_readData_errorReportsMyPanel();
                    string readData_errors = Get_deep_clone_of_entry_of_explanation_error_reports_panels();
                    Add_text_to_sb_errors(sb_error_progressBar, reportOwnTextBox, error_progressBar_startNewLineForEachError);
                    Add_text_to_sb_errors(sb_error_txtFile, reportOwnTextBox, error_textFile_startNewLineForEachError);
                    Add_text_to_sb_errors(sb_error_txtFile, readData_error_summary, error_textFile_startNewLineForEachError);
                    Add_text_to_sb_errors(sb_error_txtFile, readData_errors, error_textFile_startNewLineForEachError);
                    error_encountered = true;
                }
                if (!analysis_successful)
                {
                    Add_text_to_sb_errors(sb_error_progressBar, Get_deep_clone_of_entry_of_progressBar(), error_progressBar_startNewLineForEachError);
                }
            }
            if ((analysis_successful)&&(!error_encountered))
            {
                Write_all_analysis_finished_fileNames(out bool files_opened_successful, input_directory);
                Environment.Exit(1);
            }
            else
            {
                base.Update_text_in_progressReport(sb_error_progressBar.ToString());
                if (Directory.Exists(input_directory))
                {
                    Write_all_commandLineErrorReport_fileNames(sb_error_txtFile.ToString(), input_directory);
                }
                else
                {
                    Write_all_commandLineErrorReport_fileNames(sb_error_txtFile.ToString());
                }
                this.Visible = true;
            }
        }
    }

    class Form1_shared_text_class
    {
        public const string Sort_alphabetically_text = "alphabetically";
        public const string Sort_byLevel_text = "by level";
        public const string Sort_byLevelParentScp_text = "by parent SCPs";
        public const string Sort_byDepth_text = "by depth";
        public const string Sort_byDepthParentScp_text = "by depth and parent SCPs";
    }

}
