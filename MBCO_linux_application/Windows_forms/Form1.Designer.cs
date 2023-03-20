//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

namespace ClassLibrary1
{
    partial class Mbco_user_application_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mbco_user_application_form));
            this.MBCO_headline = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ResultsDirectory_label = new System.Windows.Forms.Label();
            this.MBCO_reference_label = new System.Windows.Forms.Label();
            this.Read_directoryOrFile_label = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.Options_scpNetworks_button = new System.Windows.Forms.Button();
            this.Options_enrichment_button = new System.Windows.Forms.Button();
            this.Options_readData_button = new System.Windows.Forms.Button();
            this.Options_backgroundGenes_button = new System.Windows.Forms.Button();
            this.Options_organizeData_button = new System.Windows.Forms.Button();
            this.Options_exampleData_button = new System.Windows.Forms.Button();
            this.Progress_report_label = new System.Windows.Forms.Label();
            this.Options_dataSignificance_button = new System.Windows.Forms.Button();
            this.Options_selectSCPs_button = new System.Windows.Forms.Button();
            this.Options_defineSCPs_button = new System.Windows.Forms.Button();
            this.Options_tips_button = new System.Windows.Forms.Button();
            this.Options_results_button = new System.Windows.Forms.Button();
            this.Options_loadExamples_panel = new Windows_forms_customized_tools.MyPanel();
            this.LoadExamples_KPMPreference_cbLabel = new System.Windows.Forms.Label();
            this.LoadExamples_NOG_cbLabel = new System.Windows.Forms.Label();
            this.LoadExamples_KPMPreference_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.LoadExamples_NOG_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.LoadExamples_copyright_label = new System.Windows.Forms.Label();
            this.LoadExamples_KPMP_reference = new System.Windows.Forms.Label();
            this.LoadExamples_NOG_reference = new System.Windows.Forms.Label();
            this.LoadExamples_overallHeadline_label = new System.Windows.Forms.Label();
            this.LoadExamples_load_button = new System.Windows.Forms.Button();
            this.Options_selectSCPs_panel = new Windows_forms_customized_tools.MyPanel();
            this.SelectSCPs_selection_panel = new Windows_forms_customized_tools.MyPanel();
            this.SelectSCPs_addGenes_cbLabel = new System.Windows.Forms.Label();
            this.SelectSCPs_showOnlySelectedScps_cbLabel = new System.Windows.Forms.Label();
            this.SelectSCPs_addGenes_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SelectSCPs_showOnlySelectedScps_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SelectSCPs_includeOffspringSCPs_cbLabel = new System.Windows.Forms.Label();
            this.SelectSCPs_includeAncestorSCPs_cbLabel = new System.Windows.Forms.Label();
            this.SelectSCPs_includeOffspringSCPs_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SelectSCPs_includeAncestorSCPs_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SelectedSCPs_writeMbcoHierarchy_button = new System.Windows.Forms.Button();
            this.SelectSCPs_sortSCPs_listBox = new Windows_forms_customized_tools.OwnListBox();
            this.SelectSCPs_remove_button = new System.Windows.Forms.Button();
            this.SelectSCPs_add_button = new System.Windows.Forms.Button();
            this.SelectSCPs_includeHeadline_label = new System.Windows.Forms.Label();
            this.SelectSCPs_includeBracket_label = new System.Windows.Forms.Label();
            this.SelectScps_mbcoSCPs_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.SelectSCPs_sortSCPs_label = new System.Windows.Forms.Label();
            this.SelectScps_selectedSCPs_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.SelectScps_selectedGroup_label = new System.Windows.Forms.Label();
            this.SelectSCPs_removeGroup_button = new System.Windows.Forms.Button();
            this.SelectSCPs_addGroup_button = new System.Windows.Forms.Button();
            this.SelectSCPs_groups_label = new System.Windows.Forms.Label();
            this.SelectSCPs_groups_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.SelectSCPs_newGroup_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.SelectSCPs_newGroup_label = new System.Windows.Forms.Label();
            this.SelectSCPs_overallHeadline_label = new System.Windows.Forms.Label();
            this.Options_scpNetworks_panel = new Windows_forms_customized_tools.MyPanel();
            this.ScpNetworks_explanation_button = new System.Windows.Forms.Button();
            this.ScpNetworks_generateNetworks_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_generateNetworks_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_nodeSize_panel = new Windows_forms_customized_tools.MyPanel();
            this.ScpNetworks_nodeSizeFixed_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_nodeSizeByColorCount_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_nodeSizeFixed_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_nodeSizeByColorCount_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_scpNodeSizes_label = new System.Windows.Forms.Label();
            this.ScpNetworks_default_button = new System.Windows.Forms.Button();
            this.ScpNetworks_standard_panel = new Windows_forms_customized_tools.MyPanel();
            this.ScpNetworks_standardConnectRelated_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_standardAddGenes_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_standardParentChild_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_standardConnectRelated_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_standardAddGenes_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_standardParentChild_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_standardConnectScpsTopInteractions_panel = new Windows_forms_customized_tools.MyPanel();
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label = new System.Windows.Forms.Label();
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label = new System.Windows.Forms.Label();
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label = new System.Windows.Forms.Label();
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label = new System.Windows.Forms.Label();
            this.ScpNetworks_standard_label = new System.Windows.Forms.Label();
            this.ScpNetworks_comments_panel = new Windows_forms_customized_tools.MyPanel();
            this.SCPNetworks_comments_yEDText_label = new System.Windows.Forms.Label();
            this.ScpNetworks_comments_standardDynamicAddGenes_label = new System.Windows.Forms.Label();
            this.ScpNetworks_commentsHeadline_label = new System.Windows.Forms.Label();
            this.ScpNetworks_dynamic_panel = new Windows_forms_customized_tools.MyPanel();
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_dynamicAddGenes_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_dynamicParentChild_cbLabel = new System.Windows.Forms.Label();
            this.ScpNetworks_dynamicConnectAllRelated_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_dynamicAddGenes_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_dynamicParentChild_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.ScpNetworks_dynamicConnectAllScps_explantion_label = new System.Windows.Forms.Label();
            this.ScpNetworks_dynamic_label = new System.Windows.Forms.Label();
            this.Options_enrichment_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_explanation_button = new System.Windows.Forms.Button();
            this.EnrichmentOptions_ontology_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_ontology_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.EnrichmentOptions_ontology_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_defineOutputs_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateTimeline_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateHeatmaps_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateBardiagrams_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateTimelineLogScale_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_generateTimeline_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_generateHeatmaps_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_generateBardiagrams_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_chartsPerPage_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_chartsPerPage_ownCheckBox = new Windows_forms_customized_tools.OwnListBox();
            this.EnrichmentOptions_generateTimelineExplanation_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateTimelinePvalue_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateTimelinePvalue_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_saveFiguresAs_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.EnrichmentOptions_saveFiguresAsExplanation_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_safeFigures_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateHeatmapsExplanation_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_generateBardiagramsExplanation_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_colors_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_colorByDatasetColor_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_colorByLevel_cbLabel = new System.Windows.Forms.Label();
            this.EnrichmentOptions_colorByDatasetColor_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_colorByLevel_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.EnrichmentOptions_colorBarsTimelines_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_keepTopSCPs_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_cutoffsExplanation_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_scpTopInteractions_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_ScpInteractionsLevel_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_default_button = new System.Windows.Forms.Button();
            this.EnrichmentOptions_scpInteractionsLevel_2_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_scpInteractionsLevel_3_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_cutoffs_panel = new Windows_forms_customized_tools.MyPanel();
            this.EnrichmentOptions_dynamicPvalue_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_maxPvalue_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_dynamicKeepTopScps_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_maxRanks_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_keepScps_level_4_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_keepScps_level_3_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_keepScps_level_2_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_standardPvalue_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_keepScpsScpLevel_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_keepScps_level_1_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_standardKeepTopScps_label = new System.Windows.Forms.Label();
            this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Options_dataSignificance_panel = new Windows_forms_customized_tools.MyPanel();
            this.SigData_allGenesSignificant_cbLabel = new System.Windows.Forms.Label();
            this.SigData_allGenesSignificant_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_allGenesSignificant_headline_label = new System.Windows.Forms.Label();
            this.SigData_resetParameter_button = new System.Windows.Forms.Button();
            this.SigData_sigSubject_explanation_label = new System.Windows.Forms.Label();
            this.SigData_resetSig_button = new System.Windows.Forms.Button();
            this.SigData_sigSelection_panel = new Windows_forms_customized_tools.MyPanel();
            this.SigData_deleteNotSignGenes_cbLabel = new System.Windows.Forms.Label();
            this.SigData_keep_mergeUpDown_cbLabel = new System.Windows.Forms.Label();
            this.SigData_keep_eachDataset_cbLabel = new System.Windows.Forms.Label();
            this.SigData_rankBy_2ndValue_cbLabel = new System.Windows.Forms.Label();
            this.SigData_rankBy_1stValue_cbLabel = new System.Windows.Forms.Label();
            this.SigData_value2nd_lowerSig_cbLabel = new System.Windows.Forms.Label();
            this.SigData_value2nd_higherSig_cbLabel = new System.Windows.Forms.Label();
            this.SigData_value1st_lowerSig_cbLabel = new System.Windows.Forms.Label();
            this.SigData_value1st_higherSig_cbLabel = new System.Windows.Forms.Label();
            this.SigData_deleteNotSignGenes_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_keep_eachDataset_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_keep_mergeUpDown_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_rankBy_2ndValue_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_rankBy_1stValue_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_value2nd_lowerSig_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_value2nd_higherSig_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_value1st_lowerSig_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_value1st_higherSig_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.SigData_value1st_cutoff_expl_label = new System.Windows.Forms.Label();
            this.SigData_value2nd_cutoff_expl_label = new System.Windows.Forms.Label();
            this.SigData_rankBy_left_label = new System.Windows.Forms.Label();
            this.SigData_tieBreaker_explanation_label = new System.Windows.Forms.Label();
            this.SigData_rankBy_top_label = new System.Windows.Forms.Label();
            this.SigData_value2nd_lowerSig_expl_label = new System.Windows.Forms.Label();
            this.SigData_value1st_higherSig_expl_label = new System.Windows.Forms.Label();
            this.SigData_2nd_sigCutoff_headline_label = new System.Windows.Forms.Label();
            this.SigData_value2nd_higherSig_expl_label = new System.Windows.Forms.Label();
            this.SigData_value1st_lowerSig_expl_label = new System.Windows.Forms.Label();
            this.SigData_keepTopRanks_right_label = new System.Windows.Forms.Label();
            this.SigData_keepTopRanks_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.SigData_keepTopRanks_left_label = new System.Windows.Forms.Label();
            this.SigData_1st_sigCutoff_headline_label = new System.Windows.Forms.Label();
            this.SigData_value2nd_cutoff_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.SigData_value2nd_cutoff_label = new System.Windows.Forms.Label();
            this.SigData_value1st_cutoff_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.SigData_value1st_cutoff_label = new System.Windows.Forms.Label();
            this.SigData_value2nd_headline_label = new System.Windows.Forms.Label();
            this.SigData_value1st_headline_label = new System.Windows.Forms.Label();
            this.Options_results_panel = new Windows_forms_customized_tools.MyPanel();
            this.Results_controlCommand_panel = new Windows_forms_customized_tools.MyPanel();
            this.Results_integrationGroup_label = new System.Windows.Forms.Label();
            this.Results_integrationGroup_listBox = new Windows_forms_customized_tools.OwnListBox();
            this.Results_bardiagram_show_label = new System.Windows.Forms.Label();
            this.Results_heatmap_dynamic_cbLabel = new System.Windows.Forms.Label();
            this.Results_bardiagram_standard_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Results_heatmap_dynamic_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Results_bardiagram_standard_cbLabel = new System.Windows.Forms.Label();
            this.Results_heatmap_show_label = new System.Windows.Forms.Label();
            this.Results_bardiagram_dynamic_cbLabel = new System.Windows.Forms.Label();
            this.Results_timeline_show_label = new System.Windows.Forms.Label();
            this.Results_bardiagram_dynamic_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Results_heatmap_standard_cbLabel = new System.Windows.Forms.Label();
            this.Results_timeline_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Results_heatmap_standard_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Results_timeline_cbLabel = new System.Windows.Forms.Label();
            this.Results_addResultsToControl_cbLabel = new System.Windows.Forms.Label();
            this.Results_addResultsToControl_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Results_directory_expl_label = new System.Windows.Forms.Label();
            this.Results_directory_headline_label = new System.Windows.Forms.Label();
            this.Results_directory_label = new System.Windows.Forms.Label();
            this.Results_overall_headline_label = new System.Windows.Forms.Label();
            this.Results_visualization_panel = new Windows_forms_customized_tools.MyPanel();
            this.Results_visualization_integrationGroup_label = new System.Windows.Forms.Label();
            this.Results_position_label = new System.Windows.Forms.Label();
            this.Results_previous_button = new System.Windows.Forms.Button();
            this.Results_next_button = new System.Windows.Forms.Button();
            this.Results_zegGraph_control = new ZedGraph.ZedGraphControl();
            this.Report_panel = new Windows_forms_customized_tools.MyPanel();
            this.Report_maxErrorPerFile2_label = new System.Windows.Forms.Label();
            this.Report_headline_label = new System.Windows.Forms.Label();
            this.Read_error_reports_maxErrorsPerFile_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Report_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Report_maxErrorPerFile1_label = new System.Windows.Forms.Label();
            this.Options_readData_panel = new Windows_forms_customized_tools.MyPanel();
            this.Read_order_allFilesInDirectory_label = new System.Windows.Forms.Label();
            this.Read_order_onlySpecifiedFile_label = new System.Windows.Forms.Label();
            this.Read_order_allFilesInDirectory_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Read_order_onlySpecifiedFile_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Read_setToOptimum_button = new System.Windows.Forms.Button();
            this.Read_setToMBCO_button = new System.Windows.Forms.Button();
            this.Read_colorColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_colorColumn_label = new System.Windows.Forms.Label();
            this.Read_headline_label = new System.Windows.Forms.Label();
            this.Read_value1st_explanation_label = new System.Windows.Forms.Label();
            this.Read_value2nd_explanation_label = new System.Windows.Forms.Label();
            this.Read_value2ndColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_value2ndColumn_label = new System.Windows.Forms.Label();
            this.Read_setToDefault_label = new System.Windows.Forms.Label();
            this.Read_timeunitColumn_label = new System.Windows.Forms.Label();
            this.Read_timeunitColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_error_reports_label = new System.Windows.Forms.Label();
            this.Read_readDataset_button = new System.Windows.Forms.Button();
            this.Read_error_reports_button = new System.Windows.Forms.Button();
            this.Read_sampleNameColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_delimiter_label = new System.Windows.Forms.Label();
            this.Read_timepointColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_delimiter_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.Read_value1stColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_timepointColumn_label = new System.Windows.Forms.Label();
            this.Read_setToMinimum_button = new System.Windows.Forms.Button();
            this.Read_sampleNameColumn_label = new System.Windows.Forms.Label();
            this.Read_integrationGroupColumn_label = new System.Windows.Forms.Label();
            this.Read_value1stColumn_label = new System.Windows.Forms.Label();
            this.Read_integrationGroupColumn_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_setToSeurat_button = new System.Windows.Forms.Button();
            this.Read_setToMBCO2_button = new System.Windows.Forms.Button();
            this.Read_geneSymbol_label = new System.Windows.Forms.Label();
            this.Read_informationGroup_label = new System.Windows.Forms.Label();
            this.Read_geneSymbol_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Read_setToMBCO1_button = new System.Windows.Forms.Button();
            this.Read_timeunit_ownCheckBox = new Windows_forms_customized_tools.OwnListBox();
            this.Read_order_allFilesDirectory_label = new System.Windows.Forms.Label();
            this.Options_defineScps_panel = new Windows_forms_customized_tools.MyPanel();
            this.DefineScps_level4_cbLabel = new System.Windows.Forms.Label();
            this.DefineScps_level3_cbLabel = new System.Windows.Forms.Label();
            this.DefineScps_level2_cbLabel = new System.Windows.Forms.Label();
            this.DefineScps_level1_cbLabel = new System.Windows.Forms.Label();
            this.DefineScps_level4_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.DefineScps_level3_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.DefineScps_level2_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.DefineScps_level1_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.DefineScps_selection_panel = new Windows_forms_customized_tools.MyPanel();
            this.DefineScps_sort_listBox = new Windows_forms_customized_tools.OwnListBox();
            this.DefineScps_ownSubScps_label = new System.Windows.Forms.Label();
            this.DefineSCPs_writeMbcoHierarchy_button = new System.Windows.Forms.Button();
            this.DefineScps_removeSubScp_button = new System.Windows.Forms.Button();
            this.DefineScps_mbcoSCP_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.DefineScps_sort_label = new System.Windows.Forms.Label();
            this.DefineScps_mbcoSCP_label = new System.Windows.Forms.Label();
            this.DefineScps_addSubScp_button = new System.Windows.Forms.Button();
            this.DefineScps_ownSubScps_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.DefineScps_level_label = new System.Windows.Forms.Label();
            this.DefineScps_removeOwnSCP_button = new System.Windows.Forms.Button();
            this.DefineScps_addNewOwnSCP_button = new System.Windows.Forms.Button();
            this.DefineScps_selectOwnScp_label = new System.Windows.Forms.Label();
            this.DefineScps_selectOwnScp_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.DefineScps_newOwnScpName_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.DefineScps_newOwnScpName_label = new System.Windows.Forms.Label();
            this.DefineScps_overall_headline_label = new System.Windows.Forms.Label();
            this.Options_tips_panel = new Windows_forms_customized_tools.MyPanel();
            this.Tips_demonstration_headline_label = new System.Windows.Forms.Label();
            this.Tips_demonstration_cbLabel = new System.Windows.Forms.Label();
            this.Tips_demonstration_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Tips_label6 = new System.Windows.Forms.Label();
            this.Tips_label5 = new System.Windows.Forms.Label();
            this.Tips_label4 = new System.Windows.Forms.Label();
            this.Tips_label3 = new System.Windows.Forms.Label();
            this.Tips_label2 = new System.Windows.Forms.Label();
            this.Tips_label1 = new System.Windows.Forms.Label();
            this.Tips_overallHeadline_label = new System.Windows.Forms.Label();
            this.Options_bgGenes_panel = new Windows_forms_customized_tools.MyPanel();
            this.BgGenes_warnings_panel = new Windows_forms_customized_tools.MyPanel();
            this.BgGenes_warnings_label = new System.Windows.Forms.Label();
            this.BgGenes_warnings_button = new System.Windows.Forms.Button();
            this.BgGenes_overall_headline_label = new System.Windows.Forms.Label();
            this.BgGenes_assignment_panel = new Windows_forms_customized_tools.MyPanel();
            this.BgGenes_assignmentsExplanation_label = new System.Windows.Forms.Label();
            this.BgGenes_assignmentsReset_label = new System.Windows.Forms.Label();
            this.BgGenes_assignmentsAutomatic_button = new System.Windows.Forms.Button();
            this.BgGenes_assignmentsAutomatic_label = new System.Windows.Forms.Label();
            this.BgGenes_assignmentsReset_button = new System.Windows.Forms.Button();
            this.BgGenes_organize_panel = new Windows_forms_customized_tools.MyPanel();
            this.BgGenes_OrganizeAvailableBgGeneLists_label = new System.Windows.Forms.Label();
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.BgGenes_OrganizeDeleteAll_button = new System.Windows.Forms.Button();
            this.BgGenes_OrganizeDeleteSelection_button = new System.Windows.Forms.Button();
            this.BgGenes_add_panel = new Windows_forms_customized_tools.MyPanel();
            this.BgGenes_addReadAllFilesInDirectory_cbLabel = new System.Windows.Forms.Label();
            this.BgGenes_addReadOnlyFile_cbLabel = new System.Windows.Forms.Label();
            this.BgGenes_addReadAllFilesInDirectory_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.BgGenes_addReadOnlyFile_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.BgGenes_AddErrors_label = new System.Windows.Forms.Label();
            this.BgGenes_AddShowErrors_button = new System.Windows.Forms.Button();
            this.BgGenes_addReadExplainFile_label = new System.Windows.Forms.Label();
            this.BgGenes_add_button = new System.Windows.Forms.Button();
            this.BgGenes_addGenes_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.BgGenes_AddRead_button = new System.Windows.Forms.Button();
            this.BgGenes_addName_label = new System.Windows.Forms.Label();
            this.BgGenes_addReadFileDir_label = new System.Windows.Forms.Label();
            this.BgGenes_addGenes_label = new System.Windows.Forms.Label();
            this.BgGenes_addName_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Options_organizeData_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_explanation_button = new System.Windows.Forms.Button();
            this.OrganizeData_convertTimeunits_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_convertTimeunites_convert_button = new System.Windows.Forms.Button();
            this.OrganizeData_convertTimeunits_unit_ownListBox = new Windows_forms_customized_tools.OwnListBox();
            this.OrganizeData_convertTimeunits_label = new System.Windows.Forms.Label();
            this.OrganizeData_automatically_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_automatically_headline_label = new System.Windows.Forms.Label();
            this.OrganizeData_automaticDatasetOrder_button = new System.Windows.Forms.Button();
            this.OrganizeData_automaticIntegrationGroups_button = new System.Windows.Forms.Button();
            this.OrganizeData_automaticColors_button = new System.Windows.Forms.Button();
            this.OrganizeData_addFileName_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_addFileNames_listBox = new Windows_forms_customized_tools.OwnListBox();
            this.OrganizeData_addFileNameAfter_label = new System.Windows.Forms.Label();
            this.OrganizeData_addFileNameBefore_label = new System.Windows.Forms.Label();
            this.OrganizeData_addFileNameRemove_label = new System.Windows.Forms.Label();
            this.OrganizeData_addFileNameRemove_button = new System.Windows.Forms.Button();
            this.OrganizeData_addFileNameAfter_button = new System.Windows.Forms.Button();
            this.OrganizeData_addFileNamesBefore_button = new System.Windows.Forms.Button();
            this.OrganizeData_addFileNames_label = new System.Windows.Forms.Label();
            this.OrganizeData_show_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_showDatasetOrderNo_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showSourceFile_label = new System.Windows.Forms.Label();
            this.OrganizeData_showSourceFile_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showColor_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showIntegrationGroup_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showTimepoint_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showEntryType_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showName_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_showDatasetOrderNo_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showSourceFile_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showColor_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showIntegrationGroup_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showTimepoint_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showEntryType_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showName_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_showDifferentEntries_label = new System.Windows.Forms.Label();
            this.OrganizeData_showDifferentEntries_button = new System.Windows.Forms.Button();
            this.OrganizeData_show_headline_label = new System.Windows.Forms.Label();
            this.OrganizeData_modify_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_modifySourceFileName_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_modifySubstring_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_modifyEntryType_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_modifyTimepoint_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_modifyName_cbLabel = new System.Windows.Forms.Label();
            this.OrganizeData_modifySubstring_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_modifySourceFileName_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_modifyEntryType_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_modifyTimepoint_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_modifyName_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.OrganizeData_modifySubstringOptions_panel = new Windows_forms_customized_tools.MyPanel();
            this.OrganizeData_modifyDelimiter_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.OrganizeData_modifyIndexLeft_label = new System.Windows.Forms.Label();
            this.OrganizeData_modifyDelimiter_label = new System.Windows.Forms.Label();
            this.OrganizeData_modifyIndexLeft_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.OrganizeData_modifyIndexRight_label = new System.Windows.Forms.Label();
            this.OrganizeData_modifyIndexes_label = new System.Windows.Forms.Label();
            this.OrganizeData_modifyIndexRight_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            this.OrganizeData_changeDelete_button = new System.Windows.Forms.Button();
            this.OrganizeData_changeIntegrationGroup_button = new System.Windows.Forms.Button();
            this.OrganizeData_changeColor_button = new System.Windows.Forms.Button();
            this.OrganizeData_modifyHeadline_label = new System.Windows.Forms.Label();
            this.AppSize_panel = new Windows_forms_customized_tools.MyPanel();
            this.AppSize_width_percent_label = new System.Windows.Forms.Label();
            this.AppSize_heightPercent_label = new System.Windows.Forms.Label();
            this.AppSize_decrease_button = new System.Windows.Forms.Button();
            this.AppSize_increase_button = new System.Windows.Forms.Button();
            this.AppSize_headline_label = new System.Windows.Forms.Label();
            this.AppSize_colorTheme_label = new System.Windows.Forms.Label();
            this.AppSize_colorTheme_listBox = new Windows_forms_customized_tools.OwnListBox();
            this.AppSize_resize_button = new System.Windows.Forms.Button();
            this.AppSize_width_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.AppSize_height_label = new System.Windows.Forms.Label();
            this.AppSize_height_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.AppSize_width_label = new System.Windows.Forms.Label();
            this.ResultsDirectory_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.DatasetInterface_overall_panel = new Windows_forms_customized_tools.MyPanel();
            this.DatasetsCount_panel = new Windows_forms_customized_tools.MyPanel();
            this.Source_panel = new Windows_forms_customized_tools.MyPanel();
            this.Source_label = new System.Windows.Forms.Label();
            this.Source_sortBy_button = new System.Windows.Forms.Button();
            this.AnalyzeData_button = new System.Windows.Forms.Button();
            this.EntryType_panel = new Windows_forms_customized_tools.MyPanel();
            this.EntryType_sortBy_button = new System.Windows.Forms.Button();
            this.EntryType_label = new System.Windows.Forms.Label();
            this.BgGenes_panel = new Windows_forms_customized_tools.MyPanel();
            this.BgGenes_sortBy_button = new System.Windows.Forms.Button();
            this.BgGenes_label = new System.Windows.Forms.Label();
            this.GeneCounts_panel = new Windows_forms_customized_tools.MyPanel();
            this.IntegrationGroup_panel = new Windows_forms_customized_tools.MyPanel();
            this.IntegrationGroup_sortBy_button = new System.Windows.Forms.Button();
            this.IntegrationGroup_label = new System.Windows.Forms.Label();
            this.Color_panel = new Windows_forms_customized_tools.MyPanel();
            this.Color_sortBy_button = new System.Windows.Forms.Button();
            this.Color_label = new System.Windows.Forms.Label();
            this.Substring_panel = new Windows_forms_customized_tools.MyPanel();
            this.Substring_label = new System.Windows.Forms.Label();
            this.Substring_sortBy_button = new System.Windows.Forms.Button();
            this.Delete_panel = new Windows_forms_customized_tools.MyPanel();
            this.Dataset_all_delete_cbButton = new Windows_forms_customized_tools.MyCheckBox_button();
            this.Timeline_panel = new Windows_forms_customized_tools.MyPanel();
            this.Timeline_label = new System.Windows.Forms.Label();
            this.Timeline_sortBy_button = new System.Windows.Forms.Button();
            this.Name_panel = new Windows_forms_customized_tools.MyPanel();
            this.Name_label = new System.Windows.Forms.Label();
            this.Name_sortBy_button = new System.Windows.Forms.Button();
            this.DatasetOrderNo_panel = new Windows_forms_customized_tools.MyPanel();
            this.DatasetOrderNo_label = new System.Windows.Forms.Label();
            this.DatasetOrderNo_sortBy_button = new System.Windows.Forms.Button();
            this.Input_geneList_textBox = new Windows_forms_customized_tools.OwnTextBox();
            this.Abort_button = new System.Windows.Forms.Button();
            this.Input_geneList_label = new System.Windows.Forms.Label();
            this.CompatibilityInfos_label = new System.Windows.Forms.Label();
            this.Dataset_scrollBar = new System.Windows.Forms.VScrollBar();
            this.Changes_reset_button = new System.Windows.Forms.Button();
            this.Changes_update_button = new System.Windows.Forms.Button();
            this.ClearCustomData_button = new System.Windows.Forms.Button();
            this.AddNewDataset_button = new System.Windows.Forms.Button();
            this.Read_directoryOrFile_ownTextBox = new Windows_forms_customized_tools.OwnTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.Options_loadExamples_panel.SuspendLayout();
            this.Options_selectSCPs_panel.SuspendLayout();
            this.SelectSCPs_selection_panel.SuspendLayout();
            this.Options_scpNetworks_panel.SuspendLayout();
            this.ScpNetworks_nodeSize_panel.SuspendLayout();
            this.ScpNetworks_standard_panel.SuspendLayout();
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.SuspendLayout();
            this.ScpNetworks_comments_panel.SuspendLayout();
            this.ScpNetworks_dynamic_panel.SuspendLayout();
            this.Options_enrichment_panel.SuspendLayout();
            this.EnrichmentOptions_ontology_panel.SuspendLayout();
            this.EnrichmentOptions_defineOutputs_panel.SuspendLayout();
            this.EnrichmentOptions_colors_panel.SuspendLayout();
            this.EnrichmentOptions_keepTopSCPs_panel.SuspendLayout();
            this.EnrichmentOptions_scpTopInteractions_panel.SuspendLayout();
            this.EnrichmentOptions_cutoffs_panel.SuspendLayout();
            this.Options_dataSignificance_panel.SuspendLayout();
            this.SigData_sigSelection_panel.SuspendLayout();
            this.Options_results_panel.SuspendLayout();
            this.Results_controlCommand_panel.SuspendLayout();
            this.Results_visualization_panel.SuspendLayout();
            this.Report_panel.SuspendLayout();
            this.Options_readData_panel.SuspendLayout();
            this.Options_defineScps_panel.SuspendLayout();
            this.DefineScps_selection_panel.SuspendLayout();
            this.Options_tips_panel.SuspendLayout();
            this.Options_bgGenes_panel.SuspendLayout();
            this.BgGenes_warnings_panel.SuspendLayout();
            this.BgGenes_assignment_panel.SuspendLayout();
            this.BgGenes_organize_panel.SuspendLayout();
            this.BgGenes_add_panel.SuspendLayout();
            this.Options_organizeData_panel.SuspendLayout();
            this.OrganizeData_convertTimeunits_panel.SuspendLayout();
            this.OrganizeData_automatically_panel.SuspendLayout();
            this.OrganizeData_addFileName_panel.SuspendLayout();
            this.OrganizeData_show_panel.SuspendLayout();
            this.OrganizeData_modify_panel.SuspendLayout();
            this.OrganizeData_modifySubstringOptions_panel.SuspendLayout();
            this.AppSize_panel.SuspendLayout();
            this.DatasetInterface_overall_panel.SuspendLayout();
            this.Source_panel.SuspendLayout();
            this.EntryType_panel.SuspendLayout();
            this.BgGenes_panel.SuspendLayout();
            this.IntegrationGroup_panel.SuspendLayout();
            this.Color_panel.SuspendLayout();
            this.Substring_panel.SuspendLayout();
            this.Delete_panel.SuspendLayout();
            this.Timeline_panel.SuspendLayout();
            this.Name_panel.SuspendLayout();
            this.DatasetOrderNo_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MBCO_headline
            // 
            this.MBCO_headline.AutoSize = true;
            this.MBCO_headline.Font = new System.Drawing.Font("Arial", 25F, System.Drawing.FontStyle.Bold);
            this.MBCO_headline.ForeColor = System.Drawing.Color.Navy;
            this.MBCO_headline.Location = new System.Drawing.Point(225, 3);
            this.MBCO_headline.Name = "MBCO_headline";
            this.MBCO_headline.Size = new System.Drawing.Size(804, 55);
            this.MBCO_headline.TabIndex = 8;
            this.MBCO_headline.Text = "Molecular Biology of the Cell Ontology";
            this.MBCO_headline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MBCO_headline.UseCompatibleTextRendering = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ResultsDirectory_label
            // 
            this.ResultsDirectory_label.AutoSize = true;
            this.ResultsDirectory_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ResultsDirectory_label.Location = new System.Drawing.Point(9, 635);
            this.ResultsDirectory_label.Name = "ResultsDirectory_label";
            this.ResultsDirectory_label.Size = new System.Drawing.Size(125, 19);
            this.ResultsDirectory_label.TabIndex = 10;
            this.ResultsDirectory_label.Text = "Save results in";
            // 
            // MBCO_reference_label
            // 
            this.MBCO_reference_label.AutoSize = true;
            this.MBCO_reference_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MBCO_reference_label.Location = new System.Drawing.Point(1, 678);
            this.MBCO_reference_label.Name = "MBCO_reference_label";
            this.MBCO_reference_label.Size = new System.Drawing.Size(0, 20);
            this.MBCO_reference_label.TabIndex = 32;
            // 
            // Read_directoryOrFile_label
            // 
            this.Read_directoryOrFile_label.AutoSize = true;
            this.Read_directoryOrFile_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_directoryOrFile_label.Location = new System.Drawing.Point(11, 684);
            this.Read_directoryOrFile_label.Name = "Read_directoryOrFile_label";
            this.Read_directoryOrFile_label.Size = new System.Drawing.Size(139, 19);
            this.Read_directoryOrFile_label.TabIndex = 77;
            this.Read_directoryOrFile_label.Text = "Read dataset file";
            // 
            // Options_scpNetworks_button
            // 
            this.Options_scpNetworks_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_scpNetworks_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_scpNetworks_button.ForeColor = System.Drawing.Color.White;
            this.Options_scpNetworks_button.Location = new System.Drawing.Point(1071, 731);
            this.Options_scpNetworks_button.Name = "Options_scpNetworks_button";
            this.Options_scpNetworks_button.Size = new System.Drawing.Size(170, 25);
            this.Options_scpNetworks_button.TabIndex = 163;
            this.Options_scpNetworks_button.Text = "SCP-networks";
            this.Options_scpNetworks_button.UseVisualStyleBackColor = false;
            this.Options_scpNetworks_button.Click += new System.EventHandler(this.Options_scpNetworks_button_Click);
            // 
            // Options_enrichment_button
            // 
            this.Options_enrichment_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_enrichment_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_enrichment_button.ForeColor = System.Drawing.Color.White;
            this.Options_enrichment_button.Location = new System.Drawing.Point(1071, 685);
            this.Options_enrichment_button.Name = "Options_enrichment_button";
            this.Options_enrichment_button.Size = new System.Drawing.Size(170, 25);
            this.Options_enrichment_button.TabIndex = 164;
            this.Options_enrichment_button.Text = "Enrichment";
            this.Options_enrichment_button.UseVisualStyleBackColor = false;
            this.Options_enrichment_button.Click += new System.EventHandler(this.Options_enrichment_button_Click);
            // 
            // Options_readData_button
            // 
            this.Options_readData_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_readData_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_readData_button.ForeColor = System.Drawing.Color.White;
            this.Options_readData_button.Location = new System.Drawing.Point(1071, 594);
            this.Options_readData_button.Name = "Options_readData_button";
            this.Options_readData_button.Size = new System.Drawing.Size(170, 25);
            this.Options_readData_button.TabIndex = 165;
            this.Options_readData_button.Text = "Read data";
            this.Options_readData_button.UseVisualStyleBackColor = false;
            this.Options_readData_button.Click += new System.EventHandler(this.Options_readData_button_Click);
            // 
            // Options_backgroundGenes_button
            // 
            this.Options_backgroundGenes_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_backgroundGenes_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_backgroundGenes_button.ForeColor = System.Drawing.Color.White;
            this.Options_backgroundGenes_button.Location = new System.Drawing.Point(1071, 662);
            this.Options_backgroundGenes_button.Name = "Options_backgroundGenes_button";
            this.Options_backgroundGenes_button.Size = new System.Drawing.Size(170, 25);
            this.Options_backgroundGenes_button.TabIndex = 168;
            this.Options_backgroundGenes_button.Text = "Background genes";
            this.Options_backgroundGenes_button.UseVisualStyleBackColor = false;
            this.Options_backgroundGenes_button.Click += new System.EventHandler(this.Options_backgroundGenes_button_Click);
            // 
            // Options_organizeData_button
            // 
            this.Options_organizeData_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_organizeData_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_organizeData_button.ForeColor = System.Drawing.Color.White;
            this.Options_organizeData_button.Location = new System.Drawing.Point(1071, 617);
            this.Options_organizeData_button.Name = "Options_organizeData_button";
            this.Options_organizeData_button.Size = new System.Drawing.Size(170, 25);
            this.Options_organizeData_button.TabIndex = 177;
            this.Options_organizeData_button.Text = "Organize data";
            this.Options_organizeData_button.UseVisualStyleBackColor = false;
            this.Options_organizeData_button.Click += new System.EventHandler(this.Options_organizeData_button_Click);
            // 
            // Options_exampleData_button
            // 
            this.Options_exampleData_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_exampleData_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_exampleData_button.ForeColor = System.Drawing.Color.White;
            this.Options_exampleData_button.Location = new System.Drawing.Point(1071, 772);
            this.Options_exampleData_button.Name = "Options_exampleData_button";
            this.Options_exampleData_button.Size = new System.Drawing.Size(170, 25);
            this.Options_exampleData_button.TabIndex = 217;
            this.Options_exampleData_button.Text = "Example data";
            this.Options_exampleData_button.UseVisualStyleBackColor = false;
            this.Options_exampleData_button.Click += new System.EventHandler(this.Options_exampleData_button_Click);
            // 
            // Progress_report_label
            // 
            this.Progress_report_label.AutoSize = true;
            this.Progress_report_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Progress_report_label.Location = new System.Drawing.Point(167, 587);
            this.Progress_report_label.Name = "Progress_report_label";
            this.Progress_report_label.Size = new System.Drawing.Size(135, 19);
            this.Progress_report_label.TabIndex = 179;
            this.Progress_report_label.Text = "Progress report";
            // 
            // Options_dataSignificance_button
            // 
            this.Options_dataSignificance_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_dataSignificance_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_dataSignificance_button.ForeColor = System.Drawing.Color.White;
            this.Options_dataSignificance_button.Location = new System.Drawing.Point(1071, 639);
            this.Options_dataSignificance_button.Name = "Options_dataSignificance_button";
            this.Options_dataSignificance_button.Size = new System.Drawing.Size(170, 25);
            this.Options_dataSignificance_button.TabIndex = 219;
            this.Options_dataSignificance_button.Text = "Set data cutoffs";
            this.Options_dataSignificance_button.UseVisualStyleBackColor = false;
            this.Options_dataSignificance_button.Click += new System.EventHandler(this.Options_dataSignificance_button_Click);
            // 
            // Options_selectSCPs_button
            // 
            this.Options_selectSCPs_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_selectSCPs_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_selectSCPs_button.ForeColor = System.Drawing.Color.White;
            this.Options_selectSCPs_button.Location = new System.Drawing.Point(1071, 708);
            this.Options_selectSCPs_button.Name = "Options_selectSCPs_button";
            this.Options_selectSCPs_button.Size = new System.Drawing.Size(170, 25);
            this.Options_selectSCPs_button.TabIndex = 220;
            this.Options_selectSCPs_button.Text = "Select SCPs";
            this.Options_selectSCPs_button.UseVisualStyleBackColor = false;
            this.Options_selectSCPs_button.Click += new System.EventHandler(this.Options_selectSCPs_button_Click);
            // 
            // Options_defineSCPs_button
            // 
            this.Options_defineSCPs_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_defineSCPs_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_defineSCPs_button.ForeColor = System.Drawing.Color.White;
            this.Options_defineSCPs_button.Location = new System.Drawing.Point(1071, 752);
            this.Options_defineSCPs_button.Name = "Options_defineSCPs_button";
            this.Options_defineSCPs_button.Size = new System.Drawing.Size(170, 25);
            this.Options_defineSCPs_button.TabIndex = 252;
            this.Options_defineSCPs_button.Text = "Define own SCPs";
            this.Options_defineSCPs_button.UseVisualStyleBackColor = false;
            this.Options_defineSCPs_button.Click += new System.EventHandler(this.Options_defineSCPs_button_Click);
            // 
            // Options_tips_button
            // 
            this.Options_tips_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_tips_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_tips_button.ForeColor = System.Drawing.Color.White;
            this.Options_tips_button.Location = new System.Drawing.Point(1072, 795);
            this.Options_tips_button.Name = "Options_tips_button";
            this.Options_tips_button.Size = new System.Drawing.Size(170, 25);
            this.Options_tips_button.TabIndex = 253;
            this.Options_tips_button.Text = "Tips";
            this.Options_tips_button.UseVisualStyleBackColor = false;
            this.Options_tips_button.Click += new System.EventHandler(this.Options_tips_button_Click);
            // 
            // Options_results_button
            // 
            this.Options_results_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Options_results_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Options_results_button.ForeColor = System.Drawing.Color.White;
            this.Options_results_button.Location = new System.Drawing.Point(1071, 825);
            this.Options_results_button.Name = "Options_results_button";
            this.Options_results_button.Size = new System.Drawing.Size(170, 25);
            this.Options_results_button.TabIndex = 262;
            this.Options_results_button.Text = "Results";
            this.Options_results_button.UseVisualStyleBackColor = false;
            this.Options_results_button.Click += new System.EventHandler(this.Options_results_button_Click);
            // 
            // Options_loadExamples_panel
            // 
            this.Options_loadExamples_panel.Border_color = System.Drawing.Color.Black;
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_KPMPreference_cbLabel);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_NOG_cbLabel);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_KPMPreference_cbButton);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_NOG_cbButton);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_copyright_label);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_KPMP_reference);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_NOG_reference);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_overallHeadline_label);
            this.Options_loadExamples_panel.Controls.Add(this.LoadExamples_load_button);
            this.Options_loadExamples_panel.Corner_radius = 10F;
            this.Options_loadExamples_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_loadExamples_panel.Location = new System.Drawing.Point(3, 468);
            this.Options_loadExamples_panel.Name = "Options_loadExamples_panel";
            this.Options_loadExamples_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_loadExamples_panel.TabIndex = 200;
            // 
            // LoadExamples_KPMPreference_cbLabel
            // 
            this.LoadExamples_KPMPreference_cbLabel.AutoSize = true;
            this.LoadExamples_KPMPreference_cbLabel.Location = new System.Drawing.Point(89, 167);
            this.LoadExamples_KPMPreference_cbLabel.Name = "LoadExamples_KPMPreference_cbLabel";
            this.LoadExamples_KPMPreference_cbLabel.Size = new System.Drawing.Size(173, 16);
            this.LoadExamples_KPMPreference_cbLabel.TabIndex = 212;
            this.LoadExamples_KPMPreference_cbLabel.Text = "KPMP reference tissue atlas";
            // 
            // LoadExamples_NOG_cbLabel
            // 
            this.LoadExamples_NOG_cbLabel.AutoSize = true;
            this.LoadExamples_NOG_cbLabel.Location = new System.Drawing.Point(94, 101);
            this.LoadExamples_NOG_cbLabel.Name = "LoadExamples_NOG_cbLabel";
            this.LoadExamples_NOG_cbLabel.Size = new System.Drawing.Size(108, 16);
            this.LoadExamples_NOG_cbLabel.TabIndex = 211;
            this.LoadExamples_NOG_cbLabel.Text = "Neurite outgrowth";
            // 
            // LoadExamples_KPMPreference_cbButton
            // 
            this.LoadExamples_KPMPreference_cbButton.Checked = false;
            this.LoadExamples_KPMPreference_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.LoadExamples_KPMPreference_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.LoadExamples_KPMPreference_cbButton.Location = new System.Drawing.Point(51, 166);
            this.LoadExamples_KPMPreference_cbButton.Name = "LoadExamples_KPMPreference_cbButton";
            this.LoadExamples_KPMPreference_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.LoadExamples_KPMPreference_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.LoadExamples_KPMPreference_cbButton.Size = new System.Drawing.Size(32, 23);
            this.LoadExamples_KPMPreference_cbButton.TabIndex = 209;
            this.LoadExamples_KPMPreference_cbButton.Text = "myCheckBox_button2";
            this.LoadExamples_KPMPreference_cbButton.UseVisualStyleBackColor = true;
            this.LoadExamples_KPMPreference_cbButton.Click += new System.EventHandler(this.LoadExamples_KPMPreference_cbButton_Click);
            // 
            // LoadExamples_NOG_cbButton
            // 
            this.LoadExamples_NOG_cbButton.Checked = false;
            this.LoadExamples_NOG_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.LoadExamples_NOG_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.LoadExamples_NOG_cbButton.Location = new System.Drawing.Point(56, 97);
            this.LoadExamples_NOG_cbButton.Name = "LoadExamples_NOG_cbButton";
            this.LoadExamples_NOG_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.LoadExamples_NOG_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.LoadExamples_NOG_cbButton.Size = new System.Drawing.Size(32, 23);
            this.LoadExamples_NOG_cbButton.TabIndex = 208;
            this.LoadExamples_NOG_cbButton.Text = "myCheckBox_button1";
            this.LoadExamples_NOG_cbButton.UseVisualStyleBackColor = true;
            this.LoadExamples_NOG_cbButton.Click += new System.EventHandler(this.LoadExamples_NOG_cbButton_Click);
            // 
            // LoadExamples_copyright_label
            // 
            this.LoadExamples_copyright_label.AutoSize = true;
            this.LoadExamples_copyright_label.Location = new System.Drawing.Point(43, 414);
            this.LoadExamples_copyright_label.Name = "LoadExamples_copyright_label";
            this.LoadExamples_copyright_label.Size = new System.Drawing.Size(234, 32);
            this.LoadExamples_copyright_label.TabIndex = 207;
            this.LoadExamples_copyright_label.Text = "Copyright for the datasets can be found\r\nat the referenced publications.";
            // 
            // LoadExamples_KPMP_reference
            // 
            this.LoadExamples_KPMP_reference.AutoSize = true;
            this.LoadExamples_KPMP_reference.Location = new System.Drawing.Point(39, 188);
            this.LoadExamples_KPMP_reference.Name = "LoadExamples_KPMP_reference";
            this.LoadExamples_KPMP_reference.Size = new System.Drawing.Size(285, 80);
            this.LoadExamples_KPMP_reference.TabIndex = 205;
            this.LoadExamples_KPMP_reference.Text = "Hansen, Sealfon, Menon et al. for the Kidney\r\nPrecision Medicine Project, 2022, S" +
    "ci Adv.\r\nLake, Chen, Hoshi, Plongthongkum et al. 2019, \r\nNat Commun.\r\nMenon et a" +
    "l. 2020, JCI Insight";
            // 
            // LoadExamples_NOG_reference
            // 
            this.LoadExamples_NOG_reference.AutoSize = true;
            this.LoadExamples_NOG_reference.Location = new System.Drawing.Point(39, 119);
            this.LoadExamples_NOG_reference.Name = "LoadExamples_NOG_reference";
            this.LoadExamples_NOG_reference.Size = new System.Drawing.Size(198, 16);
            this.LoadExamples_NOG_reference.TabIndex = 204;
            this.LoadExamples_NOG_reference.Text = "Hansen et al. 2022, J Biol Chem.";
            // 
            // LoadExamples_overallHeadline_label
            // 
            this.LoadExamples_overallHeadline_label.AutoSize = true;
            this.LoadExamples_overallHeadline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.errorProvider1.SetIconAlignment(this.LoadExamples_overallHeadline_label, System.Windows.Forms.ErrorIconAlignment.BottomRight);
            this.LoadExamples_overallHeadline_label.Location = new System.Drawing.Point(73, 10);
            this.LoadExamples_overallHeadline_label.Name = "LoadExamples_overallHeadline_label";
            this.LoadExamples_overallHeadline_label.Size = new System.Drawing.Size(218, 57);
            this.LoadExamples_overallHeadline_label.TabIndex = 200;
            this.LoadExamples_overallHeadline_label.Text = "Load example datasets\r\ngenerated and/or analyzed\r\nby the Iyengarlab";
            this.LoadExamples_overallHeadline_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadExamples_load_button
            // 
            this.LoadExamples_load_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LoadExamples_load_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadExamples_load_button.ForeColor = System.Drawing.Color.White;
            this.LoadExamples_load_button.Location = new System.Drawing.Point(205, 486);
            this.LoadExamples_load_button.Name = "LoadExamples_load_button";
            this.LoadExamples_load_button.Size = new System.Drawing.Size(140, 31);
            this.LoadExamples_load_button.TabIndex = 76;
            this.LoadExamples_load_button.Text = "Load";
            this.LoadExamples_load_button.UseVisualStyleBackColor = false;
            this.LoadExamples_load_button.Click += new System.EventHandler(this.LoadExamples_load_button_Click);
            // 
            // Options_selectSCPs_panel
            // 
            this.Options_selectSCPs_panel.Border_color = System.Drawing.Color.Black;
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_selection_panel);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_removeGroup_button);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_addGroup_button);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_groups_label);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_groups_ownListBox);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_newGroup_ownTextBox);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_newGroup_label);
            this.Options_selectSCPs_panel.Controls.Add(this.SelectSCPs_overallHeadline_label);
            this.Options_selectSCPs_panel.Corner_radius = 10F;
            this.Options_selectSCPs_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_selectSCPs_panel.Location = new System.Drawing.Point(1125, 22);
            this.Options_selectSCPs_panel.Name = "Options_selectSCPs_panel";
            this.Options_selectSCPs_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_selectSCPs_panel.TabIndex = 250;
            // 
            // SelectSCPs_selection_panel
            // 
            this.SelectSCPs_selection_panel.Border_color = System.Drawing.Color.Transparent;
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_addGenes_cbLabel);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_showOnlySelectedScps_cbLabel);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_addGenes_cbButton);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_showOnlySelectedScps_cbButton);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_includeOffspringSCPs_cbLabel);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_includeAncestorSCPs_cbLabel);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_includeOffspringSCPs_cbButton);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_includeAncestorSCPs_cbButton);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectedSCPs_writeMbcoHierarchy_button);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_sortSCPs_listBox);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_remove_button);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_add_button);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_includeHeadline_label);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_includeBracket_label);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectScps_mbcoSCPs_ownListBox);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectSCPs_sortSCPs_label);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectScps_selectedSCPs_ownListBox);
            this.SelectSCPs_selection_panel.Controls.Add(this.SelectScps_selectedGroup_label);
            this.SelectSCPs_selection_panel.Corner_radius = 10F;
            this.SelectSCPs_selection_panel.Fill_color = System.Drawing.Color.Transparent;
            this.SelectSCPs_selection_panel.Location = new System.Drawing.Point(4, 95);
            this.SelectSCPs_selection_panel.Name = "SelectSCPs_selection_panel";
            this.SelectSCPs_selection_panel.Size = new System.Drawing.Size(349, 427);
            this.SelectSCPs_selection_panel.TabIndex = 249;
            // 
            // SelectSCPs_addGenes_cbLabel
            // 
            this.SelectSCPs_addGenes_cbLabel.AutoSize = true;
            this.SelectSCPs_addGenes_cbLabel.Location = new System.Drawing.Point(57, 406);
            this.SelectSCPs_addGenes_cbLabel.Name = "SelectSCPs_addGenes_cbLabel";
            this.SelectSCPs_addGenes_cbLabel.Size = new System.Drawing.Size(69, 16);
            this.SelectSCPs_addGenes_cbLabel.TabIndex = 246;
            this.SelectSCPs_addGenes_cbLabel.Text = "Add genes";
            // 
            // SelectSCPs_showOnlySelectedScps_cbLabel
            // 
            this.SelectSCPs_showOnlySelectedScps_cbLabel.AutoSize = true;
            this.SelectSCPs_showOnlySelectedScps_cbLabel.Location = new System.Drawing.Point(57, 383);
            this.SelectSCPs_showOnlySelectedScps_cbLabel.Name = "SelectSCPs_showOnlySelectedScps_cbLabel";
            this.SelectSCPs_showOnlySelectedScps_cbLabel.Size = new System.Drawing.Size(262, 16);
            this.SelectSCPs_showOnlySelectedScps_cbLabel.TabIndex = 245;
            this.SelectSCPs_showOnlySelectedScps_cbLabel.Text = "Ignore sig. cutoffs/show only selected SCPs";
            // 
            // SelectSCPs_addGenes_cbButton
            // 
            this.SelectSCPs_addGenes_cbButton.Checked = false;
            this.SelectSCPs_addGenes_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_addGenes_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_addGenes_cbButton.Location = new System.Drawing.Point(30, 401);
            this.SelectSCPs_addGenes_cbButton.Name = "SelectSCPs_addGenes_cbButton";
            this.SelectSCPs_addGenes_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_addGenes_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_addGenes_cbButton.Size = new System.Drawing.Size(21, 23);
            this.SelectSCPs_addGenes_cbButton.TabIndex = 244;
            this.SelectSCPs_addGenes_cbButton.Text = "MyCheckBox_button1";
            this.SelectSCPs_addGenes_cbButton.UseVisualStyleBackColor = true;
            this.SelectSCPs_addGenes_cbButton.Click += new System.EventHandler(this.SelectSCPs_addGenes_cbButton_Click);
            // 
            // SelectSCPs_showOnlySelectedScps_cbButton
            // 
            this.SelectSCPs_showOnlySelectedScps_cbButton.Checked = false;
            this.SelectSCPs_showOnlySelectedScps_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_showOnlySelectedScps_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_showOnlySelectedScps_cbButton.Location = new System.Drawing.Point(30, 378);
            this.SelectSCPs_showOnlySelectedScps_cbButton.Name = "SelectSCPs_showOnlySelectedScps_cbButton";
            this.SelectSCPs_showOnlySelectedScps_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_showOnlySelectedScps_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_showOnlySelectedScps_cbButton.Size = new System.Drawing.Size(21, 23);
            this.SelectSCPs_showOnlySelectedScps_cbButton.TabIndex = 243;
            this.SelectSCPs_showOnlySelectedScps_cbButton.Text = "MyCheckBox_button1";
            this.SelectSCPs_showOnlySelectedScps_cbButton.UseVisualStyleBackColor = true;
            this.SelectSCPs_showOnlySelectedScps_cbButton.Click += new System.EventHandler(this.SelectSCPs_showOnlySelectedScps_cbButton_Click);
            // 
            // SelectSCPs_includeOffspringSCPs_cbLabel
            // 
            this.SelectSCPs_includeOffspringSCPs_cbLabel.AutoSize = true;
            this.SelectSCPs_includeOffspringSCPs_cbLabel.Location = new System.Drawing.Point(142, 213);
            this.SelectSCPs_includeOffspringSCPs_cbLabel.Name = "SelectSCPs_includeOffspringSCPs_cbLabel";
            this.SelectSCPs_includeOffspringSCPs_cbLabel.Size = new System.Drawing.Size(93, 16);
            this.SelectSCPs_includeOffspringSCPs_cbLabel.TabIndex = 242;
            this.SelectSCPs_includeOffspringSCPs_cbLabel.Text = "offspring SCPs";
            // 
            // SelectSCPs_includeAncestorSCPs_cbLabel
            // 
            this.SelectSCPs_includeAncestorSCPs_cbLabel.AutoSize = true;
            this.SelectSCPs_includeAncestorSCPs_cbLabel.Location = new System.Drawing.Point(188, 188);
            this.SelectSCPs_includeAncestorSCPs_cbLabel.Name = "SelectSCPs_includeAncestorSCPs_cbLabel";
            this.SelectSCPs_includeAncestorSCPs_cbLabel.Size = new System.Drawing.Size(95, 16);
            this.SelectSCPs_includeAncestorSCPs_cbLabel.TabIndex = 241;
            this.SelectSCPs_includeAncestorSCPs_cbLabel.Text = "ancestor SCPs";
            // 
            // SelectSCPs_includeOffspringSCPs_cbButton
            // 
            this.SelectSCPs_includeOffspringSCPs_cbButton.Checked = false;
            this.SelectSCPs_includeOffspringSCPs_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeOffspringSCPs_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeOffspringSCPs_cbButton.Location = new System.Drawing.Point(107, 211);
            this.SelectSCPs_includeOffspringSCPs_cbButton.Name = "SelectSCPs_includeOffspringSCPs_cbButton";
            this.SelectSCPs_includeOffspringSCPs_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeOffspringSCPs_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeOffspringSCPs_cbButton.Size = new System.Drawing.Size(21, 23);
            this.SelectSCPs_includeOffspringSCPs_cbButton.TabIndex = 240;
            this.SelectSCPs_includeOffspringSCPs_cbButton.Text = "MyCheckBox_button2";
            this.SelectSCPs_includeOffspringSCPs_cbButton.UseVisualStyleBackColor = true;
            this.SelectSCPs_includeOffspringSCPs_cbButton.Click += new System.EventHandler(this.SelectSCPs_includeOffspringSCPs_cbButton_Click);
            // 
            // SelectSCPs_includeAncestorSCPs_cbButton
            // 
            this.SelectSCPs_includeAncestorSCPs_cbButton.Checked = false;
            this.SelectSCPs_includeAncestorSCPs_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeAncestorSCPs_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeAncestorSCPs_cbButton.Location = new System.Drawing.Point(122, 182);
            this.SelectSCPs_includeAncestorSCPs_cbButton.Name = "SelectSCPs_includeAncestorSCPs_cbButton";
            this.SelectSCPs_includeAncestorSCPs_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeAncestorSCPs_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SelectSCPs_includeAncestorSCPs_cbButton.Size = new System.Drawing.Size(21, 23);
            this.SelectSCPs_includeAncestorSCPs_cbButton.TabIndex = 239;
            this.SelectSCPs_includeAncestorSCPs_cbButton.Text = "MyCheckBox_button1";
            this.SelectSCPs_includeAncestorSCPs_cbButton.UseVisualStyleBackColor = true;
            this.SelectSCPs_includeAncestorSCPs_cbButton.Click += new System.EventHandler(this.SelectSCPs_includeAncestorSCPs_cbButton_Click);
            // 
            // SelectedSCPs_writeMbcoHierarchy_button
            // 
            this.SelectedSCPs_writeMbcoHierarchy_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SelectedSCPs_writeMbcoHierarchy_button.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.SelectedSCPs_writeMbcoHierarchy_button.ForeColor = System.Drawing.Color.White;
            this.SelectedSCPs_writeMbcoHierarchy_button.Location = new System.Drawing.Point(169, 397);
            this.SelectedSCPs_writeMbcoHierarchy_button.Name = "SelectedSCPs_writeMbcoHierarchy_button";
            this.SelectedSCPs_writeMbcoHierarchy_button.Size = new System.Drawing.Size(176, 23);
            this.SelectedSCPs_writeMbcoHierarchy_button.TabIndex = 232;
            this.SelectedSCPs_writeMbcoHierarchy_button.Text = "Write MBCO hierarchy";
            this.SelectedSCPs_writeMbcoHierarchy_button.UseVisualStyleBackColor = false;
            this.SelectedSCPs_writeMbcoHierarchy_button.Click += new System.EventHandler(this.SelectedSCPs_writeMbcoHierarchy_button_Click);
            // 
            // SelectSCPs_sortSCPs_listBox
            // 
            this.SelectSCPs_sortSCPs_listBox.FormattingEnabled = true;
            this.SelectSCPs_sortSCPs_listBox.ItemHeight = 16;
            this.SelectSCPs_sortSCPs_listBox.Location = new System.Drawing.Point(228, 185);
            this.SelectSCPs_sortSCPs_listBox.Name = "SelectSCPs_sortSCPs_listBox";
            this.SelectSCPs_sortSCPs_listBox.ReadOnly = false;
            this.SelectSCPs_sortSCPs_listBox.Size = new System.Drawing.Size(121, 4);
            this.SelectSCPs_sortSCPs_listBox.TabIndex = 235;
            this.SelectSCPs_sortSCPs_listBox.SelectedIndexChanged += new System.EventHandler(this.SelectSCPs_sortSCPs_listBox_SelectedIndexChanged);
            // 
            // SelectSCPs_remove_button
            // 
            this.SelectSCPs_remove_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SelectSCPs_remove_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_remove_button.ForeColor = System.Drawing.Color.White;
            this.SelectSCPs_remove_button.Location = new System.Drawing.Point(2, 197);
            this.SelectSCPs_remove_button.Name = "SelectSCPs_remove_button";
            this.SelectSCPs_remove_button.Size = new System.Drawing.Size(75, 24);
            this.SelectSCPs_remove_button.TabIndex = 231;
            this.SelectSCPs_remove_button.Text = "Remove";
            this.SelectSCPs_remove_button.UseVisualStyleBackColor = false;
            this.SelectSCPs_remove_button.Click += new System.EventHandler(this.SelectSCPs_remove_button_Click);
            // 
            // SelectSCPs_add_button
            // 
            this.SelectSCPs_add_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SelectSCPs_add_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_add_button.ForeColor = System.Drawing.Color.White;
            this.SelectSCPs_add_button.Location = new System.Drawing.Point(2, 174);
            this.SelectSCPs_add_button.Name = "SelectSCPs_add_button";
            this.SelectSCPs_add_button.Size = new System.Drawing.Size(75, 24);
            this.SelectSCPs_add_button.TabIndex = 208;
            this.SelectSCPs_add_button.Text = "Add";
            this.SelectSCPs_add_button.UseVisualStyleBackColor = false;
            this.SelectSCPs_add_button.Click += new System.EventHandler(this.SelectSCPs_add_button_Click);
            // 
            // SelectSCPs_includeHeadline_label
            // 
            this.SelectSCPs_includeHeadline_label.AutoSize = true;
            this.SelectSCPs_includeHeadline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_includeHeadline_label.Location = new System.Drawing.Point(85, 166);
            this.SelectSCPs_includeHeadline_label.Name = "SelectSCPs_includeHeadline_label";
            this.SelectSCPs_includeHeadline_label.Size = new System.Drawing.Size(87, 19);
            this.SelectSCPs_includeHeadline_label.TabIndex = 237;
            this.SelectSCPs_includeHeadline_label.Text = "include all";
            // 
            // SelectSCPs_includeBracket_label
            // 
            this.SelectSCPs_includeBracket_label.AutoSize = true;
            this.SelectSCPs_includeBracket_label.Font = new System.Drawing.Font("Arial Narrow", 25F);
            this.SelectSCPs_includeBracket_label.Location = new System.Drawing.Point(67, 168);
            this.SelectSCPs_includeBracket_label.Name = "SelectSCPs_includeBracket_label";
            this.SelectSCPs_includeBracket_label.Size = new System.Drawing.Size(34, 49);
            this.SelectSCPs_includeBracket_label.TabIndex = 238;
            this.SelectSCPs_includeBracket_label.Text = "{";
            // 
            // SelectScps_mbcoSCPs_ownListBox
            // 
            this.SelectScps_mbcoSCPs_ownListBox.FormattingEnabled = true;
            this.SelectScps_mbcoSCPs_ownListBox.ItemHeight = 16;
            this.SelectScps_mbcoSCPs_ownListBox.Location = new System.Drawing.Point(4, 1);
            this.SelectScps_mbcoSCPs_ownListBox.Name = "SelectScps_mbcoSCPs_ownListBox";
            this.SelectScps_mbcoSCPs_ownListBox.ReadOnly = false;
            this.SelectScps_mbcoSCPs_ownListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.SelectScps_mbcoSCPs_ownListBox.Size = new System.Drawing.Size(345, 148);
            this.SelectScps_mbcoSCPs_ownListBox.TabIndex = 229;
            this.SelectScps_mbcoSCPs_ownListBox.SelectedIndexChanged += new System.EventHandler(this.SelectScps_mbcoSCPs_ownListBox_SelectedIndexChanged);
            // 
            // SelectSCPs_sortSCPs_label
            // 
            this.SelectSCPs_sortSCPs_label.AutoSize = true;
            this.SelectSCPs_sortSCPs_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_sortSCPs_label.Location = new System.Drawing.Point(256, 167);
            this.SelectSCPs_sortSCPs_label.Name = "SelectSCPs_sortSCPs_label";
            this.SelectSCPs_sortSCPs_label.Size = new System.Drawing.Size(91, 19);
            this.SelectSCPs_sortSCPs_label.TabIndex = 228;
            this.SelectSCPs_sortSCPs_label.Text = "Sort SCPs";
            // 
            // SelectScps_selectedSCPs_ownListBox
            // 
            this.SelectScps_selectedSCPs_ownListBox.FormattingEnabled = true;
            this.SelectScps_selectedSCPs_ownListBox.ItemHeight = 16;
            this.SelectScps_selectedSCPs_ownListBox.Location = new System.Drawing.Point(4, 248);
            this.SelectScps_selectedSCPs_ownListBox.Name = "SelectScps_selectedSCPs_ownListBox";
            this.SelectScps_selectedSCPs_ownListBox.ReadOnly = false;
            this.SelectScps_selectedSCPs_ownListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.SelectScps_selectedSCPs_ownListBox.Size = new System.Drawing.Size(345, 116);
            this.SelectScps_selectedSCPs_ownListBox.TabIndex = 230;
            this.SelectScps_selectedSCPs_ownListBox.SelectedIndexChanged += new System.EventHandler(this.SelectScps_selectedSCPs_ownListBox_SelectedIndexChanged);
            // 
            // SelectScps_selectedGroup_label
            // 
            this.SelectScps_selectedGroup_label.AutoSize = true;
            this.SelectScps_selectedGroup_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectScps_selectedGroup_label.Location = new System.Drawing.Point(0, 222);
            this.SelectScps_selectedGroup_label.Name = "SelectScps_selectedGroup_label";
            this.SelectScps_selectedGroup_label.Size = new System.Drawing.Size(124, 19);
            this.SelectScps_selectedGroup_label.TabIndex = 227;
            this.SelectScps_selectedGroup_label.Text = "Selected SCPs";
            // 
            // SelectSCPs_removeGroup_button
            // 
            this.SelectSCPs_removeGroup_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SelectSCPs_removeGroup_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_removeGroup_button.ForeColor = System.Drawing.Color.White;
            this.SelectSCPs_removeGroup_button.Location = new System.Drawing.Point(149, 49);
            this.SelectSCPs_removeGroup_button.Name = "SelectSCPs_removeGroup_button";
            this.SelectSCPs_removeGroup_button.Size = new System.Drawing.Size(205, 25);
            this.SelectSCPs_removeGroup_button.TabIndex = 248;
            this.SelectSCPs_removeGroup_button.Text = "Remove selected group";
            this.SelectSCPs_removeGroup_button.UseVisualStyleBackColor = false;
            this.SelectSCPs_removeGroup_button.Click += new System.EventHandler(this.SelectSCPs_removeGroup_button_Click);
            // 
            // SelectSCPs_addGroup_button
            // 
            this.SelectSCPs_addGroup_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SelectSCPs_addGroup_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_addGroup_button.ForeColor = System.Drawing.Color.White;
            this.SelectSCPs_addGroup_button.Location = new System.Drawing.Point(303, 25);
            this.SelectSCPs_addGroup_button.Name = "SelectSCPs_addGroup_button";
            this.SelectSCPs_addGroup_button.Size = new System.Drawing.Size(50, 25);
            this.SelectSCPs_addGroup_button.TabIndex = 247;
            this.SelectSCPs_addGroup_button.Text = "Add";
            this.SelectSCPs_addGroup_button.UseVisualStyleBackColor = false;
            this.SelectSCPs_addGroup_button.Click += new System.EventHandler(this.SelectSCPs_addGroup_button_Click);
            // 
            // SelectSCPs_groups_label
            // 
            this.SelectSCPs_groups_label.AutoSize = true;
            this.SelectSCPs_groups_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_groups_label.Location = new System.Drawing.Point(4, 55);
            this.SelectSCPs_groups_label.Name = "SelectSCPs_groups_label";
            this.SelectSCPs_groups_label.Size = new System.Drawing.Size(128, 19);
            this.SelectSCPs_groups_label.TabIndex = 246;
            this.SelectSCPs_groups_label.Text = "Selected group";
            // 
            // SelectSCPs_groups_ownListBox
            // 
            this.SelectSCPs_groups_ownListBox.FormattingEnabled = true;
            this.SelectSCPs_groups_ownListBox.ItemHeight = 16;
            this.SelectSCPs_groups_ownListBox.Location = new System.Drawing.Point(8, 75);
            this.SelectSCPs_groups_ownListBox.Name = "SelectSCPs_groups_ownListBox";
            this.SelectSCPs_groups_ownListBox.ReadOnly = false;
            this.SelectSCPs_groups_ownListBox.Size = new System.Drawing.Size(345, 4);
            this.SelectSCPs_groups_ownListBox.TabIndex = 245;
            this.SelectSCPs_groups_ownListBox.SelectedIndexChanged += new System.EventHandler(this.SelectSCPs_groups_ownListBox_SelectedIndexChanged);
            // 
            // SelectSCPs_newGroup_ownTextBox
            // 
            this.SelectSCPs_newGroup_ownTextBox.Location = new System.Drawing.Point(101, 26);
            this.SelectSCPs_newGroup_ownTextBox.Name = "SelectSCPs_newGroup_ownTextBox";
            this.SelectSCPs_newGroup_ownTextBox.Size = new System.Drawing.Size(200, 22);
            this.SelectSCPs_newGroup_ownTextBox.TabIndex = 243;
            // 
            // SelectSCPs_newGroup_label
            // 
            this.SelectSCPs_newGroup_label.AutoSize = true;
            this.SelectSCPs_newGroup_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SelectSCPs_newGroup_label.Location = new System.Drawing.Point(5, 28);
            this.SelectSCPs_newGroup_label.Name = "SelectSCPs_newGroup_label";
            this.SelectSCPs_newGroup_label.Size = new System.Drawing.Size(95, 19);
            this.SelectSCPs_newGroup_label.TabIndex = 244;
            this.SelectSCPs_newGroup_label.Text = "New group";
            // 
            // SelectSCPs_overallHeadline_label
            // 
            this.SelectSCPs_overallHeadline_label.AutoSize = true;
            this.SelectSCPs_overallHeadline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectSCPs_overallHeadline_label.ForeColor = System.Drawing.Color.Black;
            this.SelectSCPs_overallHeadline_label.Location = new System.Drawing.Point(44, 2);
            this.SelectSCPs_overallHeadline_label.Name = "SelectSCPs_overallHeadline_label";
            this.SelectSCPs_overallHeadline_label.Size = new System.Drawing.Size(283, 24);
            this.SelectSCPs_overallHeadline_label.TabIndex = 202;
            this.SelectSCPs_overallHeadline_label.Text = "Group SCPs for visualization";
            // 
            // Options_scpNetworks_panel
            // 
            this.Options_scpNetworks_panel.Border_color = System.Drawing.Color.Black;
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_explanation_button);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_generateNetworks_cbLabel);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_generateNetworks_cbButton);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_nodeSize_panel);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_default_button);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_standard_panel);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_comments_panel);
            this.Options_scpNetworks_panel.Controls.Add(this.ScpNetworks_dynamic_panel);
            this.Options_scpNetworks_panel.Corner_radius = 10F;
            this.Options_scpNetworks_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_scpNetworks_panel.Location = new System.Drawing.Point(379, 603);
            this.Options_scpNetworks_panel.Name = "Options_scpNetworks_panel";
            this.Options_scpNetworks_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_scpNetworks_panel.TabIndex = 174;
            // 
            // ScpNetworks_explanation_button
            // 
            this.ScpNetworks_explanation_button.BackColor = System.Drawing.Color.White;
            this.ScpNetworks_explanation_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ScpNetworks_explanation_button.ForeColor = System.Drawing.Color.Black;
            this.ScpNetworks_explanation_button.Location = new System.Drawing.Point(211, 501);
            this.ScpNetworks_explanation_button.Name = "ScpNetworks_explanation_button";
            this.ScpNetworks_explanation_button.Size = new System.Drawing.Size(72, 24);
            this.ScpNetworks_explanation_button.TabIndex = 253;
            this.ScpNetworks_explanation_button.Text = "Explanation";
            this.ScpNetworks_explanation_button.UseVisualStyleBackColor = false;
            this.ScpNetworks_explanation_button.Click += new System.EventHandler(this.ScpNetworks_explanation_button_Click);
            // 
            // ScpNetworks_generateNetworks_cbLabel
            // 
            this.ScpNetworks_generateNetworks_cbLabel.AutoSize = true;
            this.ScpNetworks_generateNetworks_cbLabel.Location = new System.Drawing.Point(70, 504);
            this.ScpNetworks_generateNetworks_cbLabel.Name = "ScpNetworks_generateNetworks_cbLabel";
            this.ScpNetworks_generateNetworks_cbLabel.Size = new System.Drawing.Size(147, 16);
            this.ScpNetworks_generateNetworks_cbLabel.TabIndex = 252;
            this.ScpNetworks_generateNetworks_cbLabel.Text = "Generate SCP networks";
            // 
            // ScpNetworks_generateNetworks_cbButton
            // 
            this.ScpNetworks_generateNetworks_cbButton.Checked = false;
            this.ScpNetworks_generateNetworks_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_generateNetworks_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_generateNetworks_cbButton.Location = new System.Drawing.Point(43, 501);
            this.ScpNetworks_generateNetworks_cbButton.Name = "ScpNetworks_generateNetworks_cbButton";
            this.ScpNetworks_generateNetworks_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_generateNetworks_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_generateNetworks_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_generateNetworks_cbButton.TabIndex = 248;
            this.ScpNetworks_generateNetworks_cbButton.Text = "MyCheckBox_button13";
            this.ScpNetworks_generateNetworks_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_generateNetworks_cbButton.Click += new System.EventHandler(this.ScpNetworks_generateNetworks_cbButton_Click);
            // 
            // ScpNetworks_nodeSize_panel
            // 
            this.ScpNetworks_nodeSize_panel.Border_color = System.Drawing.Color.Black;
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_nodeSizeFixed_cbLabel);
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_nodeSizeByDatasetCount_cbLabel);
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_nodeSizeByColorCount_cbLabel);
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_nodeSizeFixed_cbButton);
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_nodeSizeByDatasetCount_cbButton);
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_nodeSizeByColorCount_cbButton);
            this.ScpNetworks_nodeSize_panel.Controls.Add(this.ScpNetworks_scpNodeSizes_label);
            this.ScpNetworks_nodeSize_panel.Corner_radius = 10F;
            this.ScpNetworks_nodeSize_panel.Fill_color = System.Drawing.Color.Transparent;
            this.ScpNetworks_nodeSize_panel.Location = new System.Drawing.Point(5, 307);
            this.ScpNetworks_nodeSize_panel.Name = "ScpNetworks_nodeSize_panel";
            this.ScpNetworks_nodeSize_panel.Size = new System.Drawing.Size(350, 64);
            this.ScpNetworks_nodeSize_panel.TabIndex = 204;
            // 
            // ScpNetworks_nodeSizeFixed_cbLabel
            // 
            this.ScpNetworks_nodeSizeFixed_cbLabel.AutoSize = true;
            this.ScpNetworks_nodeSizeFixed_cbLabel.Location = new System.Drawing.Point(262, 38);
            this.ScpNetworks_nodeSizeFixed_cbLabel.Name = "ScpNetworks_nodeSizeFixed_cbLabel";
            this.ScpNetworks_nodeSizeFixed_cbLabel.Size = new System.Drawing.Size(67, 16);
            this.ScpNetworks_nodeSizeFixed_cbLabel.TabIndex = 251;
            this.ScpNetworks_nodeSizeFixed_cbLabel.Text = "same size";
            // 
            // ScpNetworks_nodeSizeByDatasetCount_cbLabel
            // 
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel.AutoSize = true;
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel.Location = new System.Drawing.Point(116, 49);
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel.Name = "ScpNetworks_nodeSizeByDatasetCount_cbLabel";
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel.Size = new System.Drawing.Size(80, 16);
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel.TabIndex = 250;
            this.ScpNetworks_nodeSizeByDatasetCount_cbLabel.Text = "~ # datasets";
            // 
            // ScpNetworks_nodeSizeByColorCount_cbLabel
            // 
            this.ScpNetworks_nodeSizeByColorCount_cbLabel.AutoSize = true;
            this.ScpNetworks_nodeSizeByColorCount_cbLabel.Location = new System.Drawing.Point(73, 26);
            this.ScpNetworks_nodeSizeByColorCount_cbLabel.Name = "ScpNetworks_nodeSizeByColorCount_cbLabel";
            this.ScpNetworks_nodeSizeByColorCount_cbLabel.Size = new System.Drawing.Size(65, 16);
            this.ScpNetworks_nodeSizeByColorCount_cbLabel.TabIndex = 249;
            this.ScpNetworks_nodeSizeByColorCount_cbLabel.Text = "~ # colors";
            // 
            // ScpNetworks_nodeSizeFixed_cbButton
            // 
            this.ScpNetworks_nodeSizeFixed_cbButton.Checked = false;
            this.ScpNetworks_nodeSizeFixed_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeFixed_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeFixed_cbButton.Location = new System.Drawing.Point(226, 34);
            this.ScpNetworks_nodeSizeFixed_cbButton.Name = "ScpNetworks_nodeSizeFixed_cbButton";
            this.ScpNetworks_nodeSizeFixed_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeFixed_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeFixed_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_nodeSizeFixed_cbButton.TabIndex = 247;
            this.ScpNetworks_nodeSizeFixed_cbButton.Text = "MyCheckBox_button12";
            this.ScpNetworks_nodeSizeFixed_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_nodeSizeFixed_cbButton.Click += new System.EventHandler(this.ScpNetworks_nodeSizeFixed_cbButton_Click);
            // 
            // ScpNetworks_nodeSizeByDatasetCount_cbButton
            // 
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Checked = false;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Location = new System.Drawing.Point(86, 43);
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Name = "ScpNetworks_nodeSizeByDatasetCount_cbButton";
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.TabIndex = 246;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Text = "MyCheckBox_button11";
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_nodeSizeByDatasetCount_cbButton.Click += new System.EventHandler(this.ScpNetworks_nodeSizeByDatasetCount_cbButton_Click);
            // 
            // ScpNetworks_nodeSizeByColorCount_cbButton
            // 
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Checked = false;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Location = new System.Drawing.Point(43, 25);
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Name = "ScpNetworks_nodeSizeByColorCount_cbButton";
            this.ScpNetworks_nodeSizeByColorCount_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_nodeSizeByColorCount_cbButton.TabIndex = 245;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Text = "MyCheckBox_button10";
            this.ScpNetworks_nodeSizeByColorCount_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_nodeSizeByColorCount_cbButton.Click += new System.EventHandler(this.ScpNetworks_nodeSizeByColorCount_cbButton_Click);
            // 
            // ScpNetworks_scpNodeSizes_label
            // 
            this.ScpNetworks_scpNodeSizes_label.AutoSize = true;
            this.ScpNetworks_scpNodeSizes_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScpNetworks_scpNodeSizes_label.ForeColor = System.Drawing.Color.Black;
            this.ScpNetworks_scpNodeSizes_label.Location = new System.Drawing.Point(94, 1);
            this.ScpNetworks_scpNodeSizes_label.Name = "ScpNetworks_scpNodeSizes_label";
            this.ScpNetworks_scpNodeSizes_label.Size = new System.Drawing.Size(157, 24);
            this.ScpNetworks_scpNodeSizes_label.TabIndex = 201;
            this.ScpNetworks_scpNodeSizes_label.Text = "SCP node sizes";
            // 
            // ScpNetworks_default_button
            // 
            this.ScpNetworks_default_button.BackColor = System.Drawing.Color.White;
            this.ScpNetworks_default_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ScpNetworks_default_button.ForeColor = System.Drawing.Color.Black;
            this.ScpNetworks_default_button.Location = new System.Drawing.Point(281, 501);
            this.ScpNetworks_default_button.Name = "ScpNetworks_default_button";
            this.ScpNetworks_default_button.Size = new System.Drawing.Size(72, 24);
            this.ScpNetworks_default_button.TabIndex = 38;
            this.ScpNetworks_default_button.Text = "Default";
            this.ScpNetworks_default_button.UseVisualStyleBackColor = false;
            this.ScpNetworks_default_button.Click += new System.EventHandler(this.ScpNetworks_default_button_Click);
            // 
            // ScpNetworks_standard_panel
            // 
            this.ScpNetworks_standard_panel.Border_color = System.Drawing.Color.Black;
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardConnectRelated_cbLabel);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardAddGenes_cbLabel);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardParentChild_cbLabel);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardConnectRelated_cbButton);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardAddGenes_cbButton);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardParentChild_cbButton);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardGroupSameLevelSCPs_cbButton);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_panel);
            this.ScpNetworks_standard_panel.Controls.Add(this.ScpNetworks_standard_label);
            this.ScpNetworks_standard_panel.Corner_radius = 10F;
            this.ScpNetworks_standard_panel.Fill_color = System.Drawing.Color.Transparent;
            this.ScpNetworks_standard_panel.Location = new System.Drawing.Point(5, 5);
            this.ScpNetworks_standard_panel.Name = "ScpNetworks_standard_panel";
            this.ScpNetworks_standard_panel.Size = new System.Drawing.Size(350, 165);
            this.ScpNetworks_standard_panel.TabIndex = 0;
            // 
            // ScpNetworks_standardConnectRelated_cbLabel
            // 
            this.ScpNetworks_standardConnectRelated_cbLabel.AutoSize = true;
            this.ScpNetworks_standardConnectRelated_cbLabel.Location = new System.Drawing.Point(121, 71);
            this.ScpNetworks_standardConnectRelated_cbLabel.Name = "ScpNetworks_standardConnectRelated_cbLabel";
            this.ScpNetworks_standardConnectRelated_cbLabel.Size = new System.Drawing.Size(136, 16);
            this.ScpNetworks_standardConnectRelated_cbLabel.TabIndex = 244;
            this.ScpNetworks_standardConnectRelated_cbLabel.Text = "Connect related SCPs";
            // 
            // ScpNetworks_standardGroupSameLevelSCPs_cbLabel
            // 
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel.AutoSize = true;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel.Location = new System.Drawing.Point(65, 63);
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel.Name = "ScpNetworks_standardGroupSameLevelSCPs_cbLabel";
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel.Size = new System.Drawing.Size(147, 16);
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel.TabIndex = 243;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbLabel.Text = "Box SCPs of same level";
            // 
            // ScpNetworks_standardAddGenes_cbLabel
            // 
            this.ScpNetworks_standardAddGenes_cbLabel.AutoSize = true;
            this.ScpNetworks_standardAddGenes_cbLabel.Location = new System.Drawing.Point(110, 38);
            this.ScpNetworks_standardAddGenes_cbLabel.Name = "ScpNetworks_standardAddGenes_cbLabel";
            this.ScpNetworks_standardAddGenes_cbLabel.Size = new System.Drawing.Size(69, 16);
            this.ScpNetworks_standardAddGenes_cbLabel.TabIndex = 242;
            this.ScpNetworks_standardAddGenes_cbLabel.Text = "Add genes";
            // 
            // ScpNetworks_standardParentChild_cbLabel
            // 
            this.ScpNetworks_standardParentChild_cbLabel.AutoSize = true;
            this.ScpNetworks_standardParentChild_cbLabel.Location = new System.Drawing.Point(79, 32);
            this.ScpNetworks_standardParentChild_cbLabel.Name = "ScpNetworks_standardParentChild_cbLabel";
            this.ScpNetworks_standardParentChild_cbLabel.Size = new System.Drawing.Size(189, 16);
            this.ScpNetworks_standardParentChild_cbLabel.TabIndex = 241;
            this.ScpNetworks_standardParentChild_cbLabel.Text = "Connect parent and child SCPs";
            // 
            // ScpNetworks_standardConnectRelated_cbButton
            // 
            this.ScpNetworks_standardConnectRelated_cbButton.Checked = false;
            this.ScpNetworks_standardConnectRelated_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardConnectRelated_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardConnectRelated_cbButton.Location = new System.Drawing.Point(281, 68);
            this.ScpNetworks_standardConnectRelated_cbButton.Name = "ScpNetworks_standardConnectRelated_cbButton";
            this.ScpNetworks_standardConnectRelated_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardConnectRelated_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardConnectRelated_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_standardConnectRelated_cbButton.TabIndex = 240;
            this.ScpNetworks_standardConnectRelated_cbButton.Text = "MyCheckBox_button5";
            this.ScpNetworks_standardConnectRelated_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_standardConnectRelated_cbButton.Click += new System.EventHandler(this.ScpNetworks_standardConnectRelated_cbButton_Click);
            // 
            // ScpNetworks_standardAddGenes_cbButton
            // 
            this.ScpNetworks_standardAddGenes_cbButton.Checked = false;
            this.ScpNetworks_standardAddGenes_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardAddGenes_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardAddGenes_cbButton.Location = new System.Drawing.Point(271, 45);
            this.ScpNetworks_standardAddGenes_cbButton.Name = "ScpNetworks_standardAddGenes_cbButton";
            this.ScpNetworks_standardAddGenes_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardAddGenes_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardAddGenes_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_standardAddGenes_cbButton.TabIndex = 239;
            this.ScpNetworks_standardAddGenes_cbButton.Text = "MyCheckBox_button4";
            this.ScpNetworks_standardAddGenes_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_standardAddGenes_cbButton.Click += new System.EventHandler(this.ScpNetworks_standardAddGenes_cbButton_Click);
            // 
            // ScpNetworks_standardParentChild_cbButton
            // 
            this.ScpNetworks_standardParentChild_cbButton.Checked = false;
            this.ScpNetworks_standardParentChild_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardParentChild_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardParentChild_cbButton.Location = new System.Drawing.Point(290, 21);
            this.ScpNetworks_standardParentChild_cbButton.Name = "ScpNetworks_standardParentChild_cbButton";
            this.ScpNetworks_standardParentChild_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardParentChild_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardParentChild_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_standardParentChild_cbButton.TabIndex = 238;
            this.ScpNetworks_standardParentChild_cbButton.Text = "MyCheckBox_button3";
            this.ScpNetworks_standardParentChild_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_standardParentChild_cbButton.Click += new System.EventHandler(this.ScpNetworks_standardParentChild_cbButton_Click);
            // 
            // ScpNetworks_standardGroupSameLevelSCPs_cbButton
            // 
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Checked = false;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Location = new System.Drawing.Point(176, 45);
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Name = "ScpNetworks_standardGroupSameLevelSCPs_cbButton";
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.TabIndex = 237;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Text = "MyCheckBox_button2";
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_standardGroupSameLevelSCPs_cbButton.Click += new System.EventHandler(this.ScpNetworks_standardGroupSameLevelSCPs_cbButton_Click);
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_panel
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Border_color = System.Drawing.Color.Black;
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_connect_label);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Controls.Add(this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Corner_radius = 10F;
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Fill_color = System.Drawing.Color.Transparent;
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Location = new System.Drawing.Point(8, 94);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Name = "ScpNetworks_standardConnectScpsTopInteractions_panel";
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.Size = new System.Drawing.Size(327, 67);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.TabIndex = 201;
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox.Location = new System.Drawing.Point(289, 37);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox.Name = "ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox";
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox.Size = new System.Drawing.Size(27, 22);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox.TabIndex = 67;
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox.TextChanged += new System.EventHandler(this.ScpNetworks_standardTopLevel_3_interactions_ownTextBox_TextChanged);
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox.Location = new System.Drawing.Point(259, 37);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox.Name = "ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox";
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox.Size = new System.Drawing.Size(27, 22);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox.TabIndex = 67;
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox.TextChanged += new System.EventHandler(this.ScpNetworks_standardTopLevel_2_interactions_ownTextBox_TextChanged);
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_level_2_label
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.AutoSize = true;
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.Location = new System.Drawing.Point(264, 20);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.Name = "ScpNetworks_standardConnectScpsTopInteractions_level_2_label";
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.Size = new System.Drawing.Size(18, 19);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.TabIndex = 83;
            this.ScpNetworks_standardConnectScpsTopInteractions_level_2_label.Text = "2";
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_level_3_label
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.AutoSize = true;
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.Location = new System.Drawing.Point(294, 20);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.Name = "ScpNetworks_standardConnectScpsTopInteractions_level_3_label";
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.Size = new System.Drawing.Size(18, 19);
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.TabIndex = 84;
            this.ScpNetworks_standardConnectScpsTopInteractions_level_3_label.Text = "3";
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_connect_label
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.AutoSize = true;
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.errorProvider1.SetIconAlignment(this.ScpNetworks_standardConnectScpsTopInteractions_connect_label, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.Location = new System.Drawing.Point(16, 27);
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.Name = "ScpNetworks_standardConnectScpsTopInteractions_connect_label";
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.Size = new System.Drawing.Size(241, 38);
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.TabIndex = 80;
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.Text = "Connect related SCPs in NW\r\nusing top % SCP interactions";
            this.ScpNetworks_standardConnectScpsTopInteractions_connect_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label
            // 
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.AutoSize = true;
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.Location = new System.Drawing.Point(243, 4);
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.Name = "ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label";
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.Size = new System.Drawing.Size(83, 19);
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.TabIndex = 85;
            this.ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label.Text = "SCP level";
            // 
            // ScpNetworks_standard_label
            // 
            this.ScpNetworks_standard_label.AutoSize = true;
            this.ScpNetworks_standard_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScpNetworks_standard_label.ForeColor = System.Drawing.Color.Black;
            this.ScpNetworks_standard_label.Location = new System.Drawing.Point(23, 3);
            this.ScpNetworks_standard_label.Name = "ScpNetworks_standard_label";
            this.ScpNetworks_standard_label.Size = new System.Drawing.Size(291, 24);
            this.ScpNetworks_standard_label.TabIndex = 200;
            this.ScpNetworks_standard_label.Text = "Standard enrichment analysis";
            // 
            // ScpNetworks_comments_panel
            // 
            this.ScpNetworks_comments_panel.Border_color = System.Drawing.Color.Black;
            this.ScpNetworks_comments_panel.Controls.Add(this.SCPNetworks_comments_yEDText_label);
            this.ScpNetworks_comments_panel.Controls.Add(this.ScpNetworks_comments_standardDynamicAddGenes_label);
            this.ScpNetworks_comments_panel.Controls.Add(this.ScpNetworks_commentsHeadline_label);
            this.ScpNetworks_comments_panel.Corner_radius = 10F;
            this.ScpNetworks_comments_panel.Fill_color = System.Drawing.Color.Transparent;
            this.ScpNetworks_comments_panel.Location = new System.Drawing.Point(5, 375);
            this.ScpNetworks_comments_panel.Name = "ScpNetworks_comments_panel";
            this.ScpNetworks_comments_panel.Size = new System.Drawing.Size(350, 126);
            this.ScpNetworks_comments_panel.TabIndex = 203;
            // 
            // SCPNetworks_comments_yEDText_label
            // 
            this.SCPNetworks_comments_yEDText_label.AutoSize = true;
            this.SCPNetworks_comments_yEDText_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
            this.SCPNetworks_comments_yEDText_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SCPNetworks_comments_yEDText_label.Location = new System.Drawing.Point(15, 20);
            this.SCPNetworks_comments_yEDText_label.Name = "SCPNetworks_comments_yEDText_label";
            this.SCPNetworks_comments_yEDText_label.Size = new System.Drawing.Size(323, 68);
            this.SCPNetworks_comments_yEDText_label.TabIndex = 201;
            this.SCPNetworks_comments_yEDText_label.Text = "graphml files can be opened with editors such as\r\nyED graph editor.\r\n(Tip: Select" +
    " layout \'tree - directed\' for standard\r\nand \'organic\' for dynamic enrichment ana" +
    "lysis.)";
            this.SCPNetworks_comments_yEDText_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScpNetworks_comments_standardDynamicAddGenes_label
            // 
            this.ScpNetworks_comments_standardDynamicAddGenes_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScpNetworks_comments_standardDynamicAddGenes_label.AutoSize = true;
            this.ScpNetworks_comments_standardDynamicAddGenes_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
            this.ScpNetworks_comments_standardDynamicAddGenes_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ScpNetworks_comments_standardDynamicAddGenes_label.Location = new System.Drawing.Point(15, 91);
            this.ScpNetworks_comments_standardDynamicAddGenes_label.Name = "ScpNetworks_comments_standardDynamicAddGenes_label";
            this.ScpNetworks_comments_standardDynamicAddGenes_label.Size = new System.Drawing.Size(313, 17);
            this.ScpNetworks_comments_standardDynamicAddGenes_label.TabIndex = 202;
            this.ScpNetworks_comments_standardDynamicAddGenes_label.Text = "Genes will be added to the youngest child SCP.";
            this.ScpNetworks_comments_standardDynamicAddGenes_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScpNetworks_commentsHeadline_label
            // 
            this.ScpNetworks_commentsHeadline_label.AutoSize = true;
            this.ScpNetworks_commentsHeadline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScpNetworks_commentsHeadline_label.ForeColor = System.Drawing.Color.Black;
            this.ScpNetworks_commentsHeadline_label.Location = new System.Drawing.Point(115, 1);
            this.ScpNetworks_commentsHeadline_label.Name = "ScpNetworks_commentsHeadline_label";
            this.ScpNetworks_commentsHeadline_label.Size = new System.Drawing.Size(111, 24);
            this.ScpNetworks_commentsHeadline_label.TabIndex = 201;
            this.ScpNetworks_commentsHeadline_label.Text = "Comments";
            // 
            // ScpNetworks_dynamic_panel
            // 
            this.ScpNetworks_dynamic_panel.Border_color = System.Drawing.Color.Black;
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicConnectAllRelated_cbLabel);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicAddGenes_cbLabel);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicParentChild_cbLabel);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicConnectAllRelated_cbButton);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicAddGenes_cbButton);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicParentChild_cbButton);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamicConnectAllScps_explantion_label);
            this.ScpNetworks_dynamic_panel.Controls.Add(this.ScpNetworks_dynamic_label);
            this.ScpNetworks_dynamic_panel.Corner_radius = 10F;
            this.ScpNetworks_dynamic_panel.Fill_color = System.Drawing.Color.Transparent;
            this.ScpNetworks_dynamic_panel.Location = new System.Drawing.Point(5, 175);
            this.ScpNetworks_dynamic_panel.Name = "ScpNetworks_dynamic_panel";
            this.ScpNetworks_dynamic_panel.Size = new System.Drawing.Size(350, 128);
            this.ScpNetworks_dynamic_panel.TabIndex = 1;
            // 
            // ScpNetworks_dynamicConnectAllRelated_cbLabel
            // 
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel.AutoSize = true;
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel.Location = new System.Drawing.Point(97, 73);
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel.Name = "ScpNetworks_dynamicConnectAllRelated_cbLabel";
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel.Size = new System.Drawing.Size(153, 16);
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel.TabIndex = 248;
            this.ScpNetworks_dynamicConnectAllRelated_cbLabel.Text = "Connect all related SCPs";
            // 
            // ScpNetworks_dynamicAddGenes_cbLabel
            // 
            this.ScpNetworks_dynamicAddGenes_cbLabel.AutoSize = true;
            this.ScpNetworks_dynamicAddGenes_cbLabel.Location = new System.Drawing.Point(237, 52);
            this.ScpNetworks_dynamicAddGenes_cbLabel.Name = "ScpNetworks_dynamicAddGenes_cbLabel";
            this.ScpNetworks_dynamicAddGenes_cbLabel.Size = new System.Drawing.Size(69, 16);
            this.ScpNetworks_dynamicAddGenes_cbLabel.TabIndex = 247;
            this.ScpNetworks_dynamicAddGenes_cbLabel.Text = "Add genes";
            // 
            // ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel
            // 
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel.AutoSize = true;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel.Location = new System.Drawing.Point(36, 88);
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel.Name = "ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel";
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel.Size = new System.Drawing.Size(147, 16);
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel.TabIndex = 246;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel.Text = "Box SCPs of same level";
            // 
            // ScpNetworks_dynamicParentChild_cbLabel
            // 
            this.ScpNetworks_dynamicParentChild_cbLabel.AutoSize = true;
            this.ScpNetworks_dynamicParentChild_cbLabel.Location = new System.Drawing.Point(82, 29);
            this.ScpNetworks_dynamicParentChild_cbLabel.Name = "ScpNetworks_dynamicParentChild_cbLabel";
            this.ScpNetworks_dynamicParentChild_cbLabel.Size = new System.Drawing.Size(189, 16);
            this.ScpNetworks_dynamicParentChild_cbLabel.TabIndex = 245;
            this.ScpNetworks_dynamicParentChild_cbLabel.Text = "Connect parent and child SCPs";
            // 
            // ScpNetworks_dynamicConnectAllRelated_cbButton
            // 
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Checked = false;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Location = new System.Drawing.Point(281, 71);
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Name = "ScpNetworks_dynamicConnectAllRelated_cbButton";
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.TabIndex = 244;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Text = "MyCheckBox_button9";
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_dynamicConnectAllRelated_cbButton.Click += new System.EventHandler(this.ScpNetworks_dynamicConnectAllRelated_cbButton_Click);
            // 
            // ScpNetworks_dynamicAddGenes_cbButton
            // 
            this.ScpNetworks_dynamicAddGenes_cbButton.Checked = false;
            this.ScpNetworks_dynamicAddGenes_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicAddGenes_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicAddGenes_cbButton.Location = new System.Drawing.Point(24, 49);
            this.ScpNetworks_dynamicAddGenes_cbButton.Name = "ScpNetworks_dynamicAddGenes_cbButton";
            this.ScpNetworks_dynamicAddGenes_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicAddGenes_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicAddGenes_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_dynamicAddGenes_cbButton.TabIndex = 243;
            this.ScpNetworks_dynamicAddGenes_cbButton.Text = "Add genes";
            this.ScpNetworks_dynamicAddGenes_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_dynamicAddGenes_cbButton.Click += new System.EventHandler(this.ScpNetworks_dynamicAddGenes_cbButton_Click);
            // 
            // ScpNetworks_dynamicGroupSameLevelSCPs_cbButton
            // 
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Checked = false;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Location = new System.Drawing.Point(176, 47);
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Name = "ScpNetworks_dynamicGroupSameLevelSCPs_cbButton";
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.TabIndex = 242;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Text = "MyCheckBox_button7";
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton.Click += new System.EventHandler(this.ScpNetworks_dynamicGroupSameLevelSCPs_cbButton_Click);
            // 
            // ScpNetworks_dynamicParentChild_cbButton
            // 
            this.ScpNetworks_dynamicParentChild_cbButton.Checked = false;
            this.ScpNetworks_dynamicParentChild_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicParentChild_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicParentChild_cbButton.Location = new System.Drawing.Point(286, 24);
            this.ScpNetworks_dynamicParentChild_cbButton.Name = "ScpNetworks_dynamicParentChild_cbButton";
            this.ScpNetworks_dynamicParentChild_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicParentChild_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.ScpNetworks_dynamicParentChild_cbButton.Size = new System.Drawing.Size(24, 23);
            this.ScpNetworks_dynamicParentChild_cbButton.TabIndex = 241;
            this.ScpNetworks_dynamicParentChild_cbButton.Text = "MyCheckBox_button6";
            this.ScpNetworks_dynamicParentChild_cbButton.UseVisualStyleBackColor = true;
            this.ScpNetworks_dynamicParentChild_cbButton.Click += new System.EventHandler(this.ScpNetworks_dynamicParentChild_cbButton_Click);
            // 
            // ScpNetworks_dynamicConnectAllScps_explantion_label
            // 
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.AutoSize = true;
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider1.SetIconAlignment(this.ScpNetworks_dynamicConnectAllScps_explantion_label, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.Location = new System.Drawing.Point(40, 88);
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.Name = "ScpNetworks_dynamicConnectAllScps_explantion_label";
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.Size = new System.Drawing.Size(282, 34);
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.TabIndex = 204;
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.Text = "(If not selected, only related SCPs that\r\nwere predicted together will be connect" +
    "ed.)";
            this.ScpNetworks_dynamicConnectAllScps_explantion_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScpNetworks_dynamic_label
            // 
            this.ScpNetworks_dynamic_label.AutoSize = true;
            this.ScpNetworks_dynamic_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScpNetworks_dynamic_label.ForeColor = System.Drawing.Color.Black;
            this.ScpNetworks_dynamic_label.Location = new System.Drawing.Point(24, 2);
            this.ScpNetworks_dynamic_label.Name = "ScpNetworks_dynamic_label";
            this.ScpNetworks_dynamic_label.Size = new System.Drawing.Size(286, 24);
            this.ScpNetworks_dynamic_label.TabIndex = 200;
            this.ScpNetworks_dynamic_label.Text = "Dynamic enrichment analysis";
            // 
            // Options_enrichment_panel
            // 
            this.Options_enrichment_panel.Border_color = System.Drawing.Color.Black;
            this.Options_enrichment_panel.Controls.Add(this.EnrichmentOptions_explanation_button);
            this.Options_enrichment_panel.Controls.Add(this.EnrichmentOptions_ontology_panel);
            this.Options_enrichment_panel.Controls.Add(this.EnrichmentOptions_defineOutputs_panel);
            this.Options_enrichment_panel.Controls.Add(this.EnrichmentOptions_colors_panel);
            this.Options_enrichment_panel.Controls.Add(this.EnrichmentOptions_keepTopSCPs_panel);
            this.Options_enrichment_panel.Corner_radius = 10F;
            this.Options_enrichment_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_enrichment_panel.Location = new System.Drawing.Point(919, 68);
            this.Options_enrichment_panel.Name = "Options_enrichment_panel";
            this.Options_enrichment_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_enrichment_panel.TabIndex = 176;
            // 
            // EnrichmentOptions_explanation_button
            // 
            this.EnrichmentOptions_explanation_button.Location = new System.Drawing.Point(314, 485);
            this.EnrichmentOptions_explanation_button.Name = "EnrichmentOptions_explanation_button";
            this.EnrichmentOptions_explanation_button.Size = new System.Drawing.Size(75, 23);
            this.EnrichmentOptions_explanation_button.TabIndex = 178;
            this.EnrichmentOptions_explanation_button.Text = "Explanation";
            this.EnrichmentOptions_explanation_button.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_explanation_button.Click += new System.EventHandler(this.EnrichmentOptions_explanation_button_Click);
            // 
            // EnrichmentOptions_ontology_panel
            // 
            this.EnrichmentOptions_ontology_panel.Border_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_ontology_panel.Controls.Add(this.EnrichmentOptions_ontology_ownListBox);
            this.EnrichmentOptions_ontology_panel.Controls.Add(this.EnrichmentOptions_ontology_label);
            this.EnrichmentOptions_ontology_panel.Corner_radius = 10F;
            this.EnrichmentOptions_ontology_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_ontology_panel.Location = new System.Drawing.Point(5, 5);
            this.EnrichmentOptions_ontology_panel.Name = "EnrichmentOptions_ontology_panel";
            this.EnrichmentOptions_ontology_panel.Size = new System.Drawing.Size(350, 30);
            this.EnrichmentOptions_ontology_panel.TabIndex = 175;
            // 
            // EnrichmentOptions_ontology_ownListBox
            // 
            this.EnrichmentOptions_ontology_ownListBox.FormattingEnabled = true;
            this.EnrichmentOptions_ontology_ownListBox.ItemHeight = 16;
            this.EnrichmentOptions_ontology_ownListBox.Location = new System.Drawing.Point(89, 6);
            this.EnrichmentOptions_ontology_ownListBox.Name = "EnrichmentOptions_ontology_ownListBox";
            this.EnrichmentOptions_ontology_ownListBox.ReadOnly = false;
            this.EnrichmentOptions_ontology_ownListBox.Size = new System.Drawing.Size(245, 4);
            this.EnrichmentOptions_ontology_ownListBox.TabIndex = 33;
            this.EnrichmentOptions_ontology_ownListBox.SelectedIndexChanged += new System.EventHandler(this.EnrichmentOptions_ontology_ownListBox_SelectedIndexChanged);
            // 
            // EnrichmentOptions_ontology_label
            // 
            this.EnrichmentOptions_ontology_label.AutoSize = true;
            this.EnrichmentOptions_ontology_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_ontology_label.Location = new System.Drawing.Point(7, 7);
            this.EnrichmentOptions_ontology_label.Name = "EnrichmentOptions_ontology_label";
            this.EnrichmentOptions_ontology_label.Size = new System.Drawing.Size(81, 19);
            this.EnrichmentOptions_ontology_label.TabIndex = 32;
            this.EnrichmentOptions_ontology_label.Text = "Ontology";
            // 
            // EnrichmentOptions_defineOutputs_panel
            // 
            this.EnrichmentOptions_defineOutputs_panel.Border_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimelineLogScale_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimeline_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmaps_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateBardiagrams_cbLabel);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimelineLogScale_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimeline_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapShowRanks_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmaps_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateBardiagrams_cbButton);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_chartsPerPage_label);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_chartsPerPage_ownCheckBox);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimelineExplanation_label);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimelinePvalue_label);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateTimelinePvalue_textBox);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_saveFiguresAs_ownListBox);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_saveFiguresAsExplanation_label);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_safeFigures_label);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateHeatmapsExplanation_label);
            this.EnrichmentOptions_defineOutputs_panel.Controls.Add(this.EnrichmentOptions_generateBardiagramsExplanation_label);
            this.EnrichmentOptions_defineOutputs_panel.Corner_radius = 10F;
            this.EnrichmentOptions_defineOutputs_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_defineOutputs_panel.Location = new System.Drawing.Point(5, 230);
            this.EnrichmentOptions_defineOutputs_panel.Name = "EnrichmentOptions_defineOutputs_panel";
            this.EnrichmentOptions_defineOutputs_panel.Size = new System.Drawing.Size(350, 240);
            this.EnrichmentOptions_defineOutputs_panel.TabIndex = 177;
            // 
            // EnrichmentOptions_generateTimelineLogScale_cbLabel
            // 
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel.Location = new System.Drawing.Point(86, 182);
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel.Name = "EnrichmentOptions_generateTimelineLogScale_cbLabel";
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel.Size = new System.Drawing.Size(71, 16);
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel.TabIndex = 75;
            this.EnrichmentOptions_generateTimelineLogScale_cbLabel.Text = "log10(time)";
            // 
            // EnrichmentOptions_generateTimeline_cbLabel
            // 
            this.EnrichmentOptions_generateTimeline_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateTimeline_cbLabel.Location = new System.Drawing.Point(54, 141);
            this.EnrichmentOptions_generateTimeline_cbLabel.Name = "EnrichmentOptions_generateTimeline_cbLabel";
            this.EnrichmentOptions_generateTimeline_cbLabel.Size = new System.Drawing.Size(119, 16);
            this.EnrichmentOptions_generateTimeline_cbLabel.TabIndex = 74;
            this.EnrichmentOptions_generateTimeline_cbLabel.Text = "Generate Timelines";
            // 
            // EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel
            // 
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.Location = new System.Drawing.Point(86, 111);
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.Name = "EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel";
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.Size = new System.Drawing.Size(314, 16);
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.TabIndex = 73;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.Text = "Show SCPs sig. for at least 1 dataset for all datasets";
            // 
            // EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel
            // 
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel.Location = new System.Drawing.Point(79, 83);
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel.Name = "EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel";
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel.Size = new System.Drawing.Size(93, 16);
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel.TabIndex = 72;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel.Text = "Show -log10(p)";
            // 
            // EnrichmentOptions_generateHeatmapShowRanks_cbLabel
            // 
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel.Location = new System.Drawing.Point(108, 65);
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel.Name = "EnrichmentOptions_generateHeatmapShowRanks_cbLabel";
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel.Size = new System.Drawing.Size(75, 16);
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel.TabIndex = 71;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbLabel.Text = "Show ranks";
            // 
            // EnrichmentOptions_generateHeatmaps_cbLabel
            // 
            this.EnrichmentOptions_generateHeatmaps_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateHeatmaps_cbLabel.Location = new System.Drawing.Point(67, 42);
            this.EnrichmentOptions_generateHeatmaps_cbLabel.Name = "EnrichmentOptions_generateHeatmaps_cbLabel";
            this.EnrichmentOptions_generateHeatmaps_cbLabel.Size = new System.Drawing.Size(123, 16);
            this.EnrichmentOptions_generateHeatmaps_cbLabel.TabIndex = 70;
            this.EnrichmentOptions_generateHeatmaps_cbLabel.Text = "Generate Heatmaps";
            // 
            // EnrichmentOptions_generateBardiagrams_cbLabel
            // 
            this.EnrichmentOptions_generateBardiagrams_cbLabel.AutoSize = true;
            this.EnrichmentOptions_generateBardiagrams_cbLabel.Location = new System.Drawing.Point(75, 6);
            this.EnrichmentOptions_generateBardiagrams_cbLabel.Name = "EnrichmentOptions_generateBardiagrams_cbLabel";
            this.EnrichmentOptions_generateBardiagrams_cbLabel.Size = new System.Drawing.Size(137, 16);
            this.EnrichmentOptions_generateBardiagrams_cbLabel.TabIndex = 69;
            this.EnrichmentOptions_generateBardiagrams_cbLabel.Text = "Generate Bardiagrams";
            // 
            // EnrichmentOptions_generateTimelineLogScale_cbButton
            // 
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Checked = false;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Location = new System.Drawing.Point(57, 176);
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Name = "EnrichmentOptions_generateTimelineLogScale_cbButton";
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.TabIndex = 68;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Text = "myCheckBox_button7";
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateTimelineLogScale_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateTimelineLogScale_cbButton_Click);
            // 
            // EnrichmentOptions_generateTimeline_cbButton
            // 
            this.EnrichmentOptions_generateTimeline_cbButton.Checked = false;
            this.EnrichmentOptions_generateTimeline_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimeline_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimeline_cbButton.Location = new System.Drawing.Point(29, 137);
            this.EnrichmentOptions_generateTimeline_cbButton.Name = "EnrichmentOptions_generateTimeline_cbButton";
            this.EnrichmentOptions_generateTimeline_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimeline_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateTimeline_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateTimeline_cbButton.TabIndex = 67;
            this.EnrichmentOptions_generateTimeline_cbButton.Text = "myCheckBox_button6";
            this.EnrichmentOptions_generateTimeline_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateTimeline_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateTimeline_cbButton_Click);
            // 
            // EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton
            // 
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Checked = false;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Location = new System.Drawing.Point(53, 109);
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Name = "EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton";
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.TabIndex = 66;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Text = "myCheckBox_button5";
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton_Click);
            // 
            // EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton
            // 
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Checked = false;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Location = new System.Drawing.Point(50, 80);
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Name = "EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton";
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.TabIndex = 65;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Text = "myCheckBox_button4";
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton_Click);
            // 
            // EnrichmentOptions_generateHeatmapShowRanks_cbButton
            // 
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Checked = false;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Location = new System.Drawing.Point(68, 60);
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Name = "EnrichmentOptions_generateHeatmapShowRanks_cbButton";
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.TabIndex = 64;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Text = "myCheckBox_button3";
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateHeatmapShowRanks_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateHeatmapShowRanks_cbButton_Click);
            // 
            // EnrichmentOptions_generateHeatmaps_cbButton
            // 
            this.EnrichmentOptions_generateHeatmaps_cbButton.Checked = false;
            this.EnrichmentOptions_generateHeatmaps_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmaps_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmaps_cbButton.Location = new System.Drawing.Point(35, 39);
            this.EnrichmentOptions_generateHeatmaps_cbButton.Name = "EnrichmentOptions_generateHeatmaps_cbButton";
            this.EnrichmentOptions_generateHeatmaps_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmaps_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateHeatmaps_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateHeatmaps_cbButton.TabIndex = 63;
            this.EnrichmentOptions_generateHeatmaps_cbButton.Text = "myCheckBox_button2";
            this.EnrichmentOptions_generateHeatmaps_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateHeatmaps_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateHeatmaps_cbButton_Click);
            // 
            // EnrichmentOptions_generateBardiagrams_cbButton
            // 
            this.EnrichmentOptions_generateBardiagrams_cbButton.Checked = false;
            this.EnrichmentOptions_generateBardiagrams_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateBardiagrams_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateBardiagrams_cbButton.Location = new System.Drawing.Point(39, 6);
            this.EnrichmentOptions_generateBardiagrams_cbButton.Name = "EnrichmentOptions_generateBardiagrams_cbButton";
            this.EnrichmentOptions_generateBardiagrams_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateBardiagrams_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_generateBardiagrams_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_generateBardiagrams_cbButton.TabIndex = 62;
            this.EnrichmentOptions_generateBardiagrams_cbButton.Text = "myCheckBox_button1";
            this.EnrichmentOptions_generateBardiagrams_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_generateBardiagrams_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_generateBardiagrams_cbButton_Click);
            // 
            // EnrichmentOptions_chartsPerPage_label
            // 
            this.EnrichmentOptions_chartsPerPage_label.AutoSize = true;
            this.EnrichmentOptions_chartsPerPage_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_chartsPerPage_label.Location = new System.Drawing.Point(6, 219);
            this.EnrichmentOptions_chartsPerPage_label.Name = "EnrichmentOptions_chartsPerPage_label";
            this.EnrichmentOptions_chartsPerPage_label.Size = new System.Drawing.Size(111, 18);
            this.EnrichmentOptions_chartsPerPage_label.TabIndex = 60;
            this.EnrichmentOptions_chartsPerPage_label.Text = "# charts / page";
            this.EnrichmentOptions_chartsPerPage_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EnrichmentOptions_chartsPerPage_ownCheckBox
            // 
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.FormattingEnabled = true;
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.ItemHeight = 16;
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.Location = new System.Drawing.Point(115, 218);
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.Name = "EnrichmentOptions_chartsPerPage_ownCheckBox";
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.ReadOnly = false;
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.Size = new System.Drawing.Size(68, 4);
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.TabIndex = 59;
            this.EnrichmentOptions_chartsPerPage_ownCheckBox.SelectedIndexChanged += new System.EventHandler(this.EnrichmentOptions_chartsPerPage_ownCheckBox_SelectedIndexChanged);
            // 
            // EnrichmentOptions_generateTimelineExplanation_label
            // 
            this.EnrichmentOptions_generateTimelineExplanation_label.AutoSize = true;
            this.EnrichmentOptions_generateTimelineExplanation_label.Location = new System.Drawing.Point(215, 140);
            this.EnrichmentOptions_generateTimelineExplanation_label.Name = "EnrichmentOptions_generateTimelineExplanation_label";
            this.EnrichmentOptions_generateTimelineExplanation_label.Size = new System.Drawing.Size(114, 48);
            this.EnrichmentOptions_generateTimelineExplanation_label.TabIndex = 50;
            this.EnrichmentOptions_generateTimelineExplanation_label.Text = "1 timeline chart for\r\neach integration\r\ngroup and SCP";
            // 
            // EnrichmentOptions_generateTimelinePvalue_label
            // 
            this.EnrichmentOptions_generateTimelinePvalue_label.AutoSize = true;
            this.EnrichmentOptions_generateTimelinePvalue_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_generateTimelinePvalue_label.Location = new System.Drawing.Point(28, 158);
            this.EnrichmentOptions_generateTimelinePvalue_label.Name = "EnrichmentOptions_generateTimelinePvalue_label";
            this.EnrichmentOptions_generateTimelinePvalue_label.Size = new System.Drawing.Size(111, 18);
            this.EnrichmentOptions_generateTimelinePvalue_label.TabIndex = 58;
            this.EnrichmentOptions_generateTimelinePvalue_label.Text = "P-value cutoff:";
            this.EnrichmentOptions_generateTimelinePvalue_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EnrichmentOptions_generateTimelinePvalue_textBox
            // 
            this.EnrichmentOptions_generateTimelinePvalue_textBox.Location = new System.Drawing.Point(139, 156);
            this.EnrichmentOptions_generateTimelinePvalue_textBox.Name = "EnrichmentOptions_generateTimelinePvalue_textBox";
            this.EnrichmentOptions_generateTimelinePvalue_textBox.Size = new System.Drawing.Size(40, 22);
            this.EnrichmentOptions_generateTimelinePvalue_textBox.TabIndex = 57;
            this.EnrichmentOptions_generateTimelinePvalue_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_generateTimelinePvalue_textBox_TextChanged);
            // 
            // EnrichmentOptions_saveFiguresAs_ownListBox
            // 
            this.EnrichmentOptions_saveFiguresAs_ownListBox.FormattingEnabled = true;
            this.EnrichmentOptions_saveFiguresAs_ownListBox.ItemHeight = 16;
            this.EnrichmentOptions_saveFiguresAs_ownListBox.Location = new System.Drawing.Point(114, 197);
            this.EnrichmentOptions_saveFiguresAs_ownListBox.Name = "EnrichmentOptions_saveFiguresAs_ownListBox";
            this.EnrichmentOptions_saveFiguresAs_ownListBox.ReadOnly = false;
            this.EnrichmentOptions_saveFiguresAs_ownListBox.Size = new System.Drawing.Size(68, 4);
            this.EnrichmentOptions_saveFiguresAs_ownListBox.TabIndex = 53;
            this.EnrichmentOptions_saveFiguresAs_ownListBox.SelectedIndexChanged += new System.EventHandler(this.EnrichmentOptions_saveFiguresAs_ownListBox_SelectedIndexChanged);
            // 
            // EnrichmentOptions_saveFiguresAsExplanation_label
            // 
            this.EnrichmentOptions_saveFiguresAsExplanation_label.AutoSize = true;
            this.EnrichmentOptions_saveFiguresAsExplanation_label.Location = new System.Drawing.Point(214, 201);
            this.EnrichmentOptions_saveFiguresAsExplanation_label.Name = "EnrichmentOptions_saveFiguresAsExplanation_label";
            this.EnrichmentOptions_saveFiguresAsExplanation_label.Size = new System.Drawing.Size(122, 32);
            this.EnrichmentOptions_saveFiguresAsExplanation_label.TabIndex = 52;
            this.EnrichmentOptions_saveFiguresAsExplanation_label.Text = "Generation of PDFs\r\nis time consumptive";
            // 
            // EnrichmentOptions_safeFigures_label
            // 
            this.EnrichmentOptions_safeFigures_label.AutoSize = true;
            this.EnrichmentOptions_safeFigures_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_safeFigures_label.Location = new System.Drawing.Point(3, 198);
            this.EnrichmentOptions_safeFigures_label.Name = "EnrichmentOptions_safeFigures_label";
            this.EnrichmentOptions_safeFigures_label.Size = new System.Drawing.Size(129, 19);
            this.EnrichmentOptions_safeFigures_label.TabIndex = 51;
            this.EnrichmentOptions_safeFigures_label.Text = "Save figures as";
            this.EnrichmentOptions_safeFigures_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EnrichmentOptions_generateHeatmapsExplanation_label
            // 
            this.EnrichmentOptions_generateHeatmapsExplanation_label.AutoSize = true;
            this.EnrichmentOptions_generateHeatmapsExplanation_label.Location = new System.Drawing.Point(216, 44);
            this.EnrichmentOptions_generateHeatmapsExplanation_label.Name = "EnrichmentOptions_generateHeatmapsExplanation_label";
            this.EnrichmentOptions_generateHeatmapsExplanation_label.Size = new System.Drawing.Size(118, 48);
            this.EnrichmentOptions_generateHeatmapsExplanation_label.TabIndex = 49;
            this.EnrichmentOptions_generateHeatmapsExplanation_label.Text = "1 heatmap for each\r\nintegration group\r\nand SCP level";
            // 
            // EnrichmentOptions_generateBardiagramsExplanation_label
            // 
            this.EnrichmentOptions_generateBardiagramsExplanation_label.AutoSize = true;
            this.EnrichmentOptions_generateBardiagramsExplanation_label.Location = new System.Drawing.Point(216, 2);
            this.EnrichmentOptions_generateBardiagramsExplanation_label.Name = "EnrichmentOptions_generateBardiagramsExplanation_label";
            this.EnrichmentOptions_generateBardiagramsExplanation_label.Size = new System.Drawing.Size(115, 32);
            this.EnrichmentOptions_generateBardiagramsExplanation_label.TabIndex = 48;
            this.EnrichmentOptions_generateBardiagramsExplanation_label.Text = "1 bardiagram chart\r\nfor each dataset";
            // 
            // EnrichmentOptions_colors_panel
            // 
            this.EnrichmentOptions_colors_panel.Border_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_colors_panel.Controls.Add(this.EnrichmentOptions_colorByDatasetColor_cbLabel);
            this.EnrichmentOptions_colors_panel.Controls.Add(this.EnrichmentOptions_colorByLevel_cbLabel);
            this.EnrichmentOptions_colors_panel.Controls.Add(this.EnrichmentOptions_colorByDatasetColor_cbButton);
            this.EnrichmentOptions_colors_panel.Controls.Add(this.EnrichmentOptions_colorByLevel_cbButton);
            this.EnrichmentOptions_colors_panel.Controls.Add(this.EnrichmentOptions_colorBarsTimelines_label);
            this.EnrichmentOptions_colors_panel.Corner_radius = 10F;
            this.EnrichmentOptions_colors_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_colors_panel.Location = new System.Drawing.Point(5, 475);
            this.EnrichmentOptions_colors_panel.Name = "EnrichmentOptions_colors_panel";
            this.EnrichmentOptions_colors_panel.Size = new System.Drawing.Size(300, 45);
            this.EnrichmentOptions_colors_panel.TabIndex = 176;
            // 
            // EnrichmentOptions_colorByDatasetColor_cbLabel
            // 
            this.EnrichmentOptions_colorByDatasetColor_cbLabel.AutoSize = true;
            this.EnrichmentOptions_colorByDatasetColor_cbLabel.Location = new System.Drawing.Point(158, 25);
            this.EnrichmentOptions_colorByDatasetColor_cbLabel.Name = "EnrichmentOptions_colorByDatasetColor_cbLabel";
            this.EnrichmentOptions_colorByDatasetColor_cbLabel.Size = new System.Drawing.Size(89, 16);
            this.EnrichmentOptions_colorByDatasetColor_cbLabel.TabIndex = 78;
            this.EnrichmentOptions_colorByDatasetColor_cbLabel.Text = "dataset colors";
            // 
            // EnrichmentOptions_colorByLevel_cbLabel
            // 
            this.EnrichmentOptions_colorByLevel_cbLabel.AutoSize = true;
            this.EnrichmentOptions_colorByLevel_cbLabel.Location = new System.Drawing.Point(22, 23);
            this.EnrichmentOptions_colorByLevel_cbLabel.Name = "EnrichmentOptions_colorByLevel_cbLabel";
            this.EnrichmentOptions_colorByLevel_cbLabel.Size = new System.Drawing.Size(70, 16);
            this.EnrichmentOptions_colorByLevel_cbLabel.TabIndex = 76;
            this.EnrichmentOptions_colorByLevel_cbLabel.Text = "SCP levels";
            // 
            // EnrichmentOptions_colorByDatasetColor_cbButton
            // 
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Checked = false;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Location = new System.Drawing.Point(254, 22);
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Name = "EnrichmentOptions_colorByDatasetColor_cbButton";
            this.EnrichmentOptions_colorByDatasetColor_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_colorByDatasetColor_cbButton.TabIndex = 77;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Text = "myCheckBox_button7";
            this.EnrichmentOptions_colorByDatasetColor_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_colorByDatasetColor_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_colorByDatasetColor_cbButton_Click);
            // 
            // EnrichmentOptions_colorByLevel_cbButton
            // 
            this.EnrichmentOptions_colorByLevel_cbButton.Checked = false;
            this.EnrichmentOptions_colorByLevel_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByLevel_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByLevel_cbButton.Location = new System.Drawing.Point(98, 24);
            this.EnrichmentOptions_colorByLevel_cbButton.Name = "EnrichmentOptions_colorByLevel_cbButton";
            this.EnrichmentOptions_colorByLevel_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByLevel_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.EnrichmentOptions_colorByLevel_cbButton.Size = new System.Drawing.Size(23, 23);
            this.EnrichmentOptions_colorByLevel_cbButton.TabIndex = 76;
            this.EnrichmentOptions_colorByLevel_cbButton.Text = "myCheckBox_button7";
            this.EnrichmentOptions_colorByLevel_cbButton.UseVisualStyleBackColor = true;
            this.EnrichmentOptions_colorByLevel_cbButton.Click += new System.EventHandler(this.EnrichmentOptions_colorByLevel_cbButton_Click);
            // 
            // EnrichmentOptions_colorBarsTimelines_label
            // 
            this.EnrichmentOptions_colorBarsTimelines_label.AutoSize = true;
            this.EnrichmentOptions_colorBarsTimelines_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_colorBarsTimelines_label.Location = new System.Drawing.Point(37, 1);
            this.EnrichmentOptions_colorBarsTimelines_label.Name = "EnrichmentOptions_colorBarsTimelines_label";
            this.EnrichmentOptions_colorBarsTimelines_label.Size = new System.Drawing.Size(243, 19);
            this.EnrichmentOptions_colorBarsTimelines_label.TabIndex = 68;
            this.EnrichmentOptions_colorBarsTimelines_label.Text = "Color bars\\timelines based on";
            // 
            // EnrichmentOptions_keepTopSCPs_panel
            // 
            this.EnrichmentOptions_keepTopSCPs_panel.Border_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_keepTopSCPs_panel.Controls.Add(this.EnrichmentOptions_cutoffsExplanation_label);
            this.EnrichmentOptions_keepTopSCPs_panel.Controls.Add(this.EnrichmentOptions_scpTopInteractions_panel);
            this.EnrichmentOptions_keepTopSCPs_panel.Controls.Add(this.EnrichmentOptions_cutoffs_panel);
            this.EnrichmentOptions_keepTopSCPs_panel.Corner_radius = 10F;
            this.EnrichmentOptions_keepTopSCPs_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_keepTopSCPs_panel.Location = new System.Drawing.Point(5, 40);
            this.EnrichmentOptions_keepTopSCPs_panel.Name = "EnrichmentOptions_keepTopSCPs_panel";
            this.EnrichmentOptions_keepTopSCPs_panel.Size = new System.Drawing.Size(350, 185);
            this.EnrichmentOptions_keepTopSCPs_panel.TabIndex = 174;
            // 
            // EnrichmentOptions_cutoffsExplanation_label
            // 
            this.EnrichmentOptions_cutoffsExplanation_label.Location = new System.Drawing.Point(42, 98);
            this.EnrichmentOptions_cutoffsExplanation_label.Name = "EnrichmentOptions_cutoffsExplanation_label";
            this.EnrichmentOptions_cutoffsExplanation_label.Size = new System.Drawing.Size(259, 21);
            this.EnrichmentOptions_cutoffsExplanation_label.TabIndex = 54;
            this.EnrichmentOptions_cutoffsExplanation_label.Text = "Cutoff expl. label";
            this.EnrichmentOptions_cutoffsExplanation_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EnrichmentOptions_scpTopInteractions_panel
            // 
            this.EnrichmentOptions_scpTopInteractions_panel.Border_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_percentDynamicTopSCPInteractions_label);
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_ScpInteractionsLevel_label);
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_default_button);
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_scpInteractionsLevel_2_label);
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox);
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_scpInteractionsLevel_3_label);
            this.EnrichmentOptions_scpTopInteractions_panel.Controls.Add(this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox);
            this.EnrichmentOptions_scpTopInteractions_panel.Corner_radius = 10F;
            this.EnrichmentOptions_scpTopInteractions_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_scpTopInteractions_panel.Location = new System.Drawing.Point(3, 123);
            this.EnrichmentOptions_scpTopInteractions_panel.Name = "EnrichmentOptions_scpTopInteractions_panel";
            this.EnrichmentOptions_scpTopInteractions_panel.Size = new System.Drawing.Size(343, 58);
            this.EnrichmentOptions_scpTopInteractions_panel.TabIndex = 222;
            // 
            // EnrichmentOptions_percentDynamicTopSCPInteractions_label
            // 
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.AutoSize = true;
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.errorProvider1.SetIconAlignment(this.EnrichmentOptions_percentDynamicTopSCPInteractions_label, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.Location = new System.Drawing.Point(3, 17);
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.Name = "EnrichmentOptions_percentDynamicTopSCPInteractions_label";
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.Size = new System.Drawing.Size(199, 38);
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.TabIndex = 30;
            this.EnrichmentOptions_percentDynamicTopSCPInteractions_label.Text = "Top % SCP interactions\r\n for dynamic enrichment";
            // 
            // EnrichmentOptions_ScpInteractionsLevel_label
            // 
            this.EnrichmentOptions_ScpInteractionsLevel_label.AutoSize = true;
            this.EnrichmentOptions_ScpInteractionsLevel_label.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnrichmentOptions_ScpInteractionsLevel_label.Location = new System.Drawing.Point(125, 1);
            this.EnrichmentOptions_ScpInteractionsLevel_label.Name = "EnrichmentOptions_ScpInteractionsLevel_label";
            this.EnrichmentOptions_ScpInteractionsLevel_label.Size = new System.Drawing.Size(82, 19);
            this.EnrichmentOptions_ScpInteractionsLevel_label.TabIndex = 56;
            this.EnrichmentOptions_ScpInteractionsLevel_label.Text = "SCP level";
            // 
            // EnrichmentOptions_default_button
            // 
            this.EnrichmentOptions_default_button.BackColor = System.Drawing.Color.White;
            this.EnrichmentOptions_default_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_default_button.ForeColor = System.Drawing.Color.Black;
            this.EnrichmentOptions_default_button.Location = new System.Drawing.Point(261, 25);
            this.EnrichmentOptions_default_button.Name = "EnrichmentOptions_default_button";
            this.EnrichmentOptions_default_button.Size = new System.Drawing.Size(72, 27);
            this.EnrichmentOptions_default_button.TabIndex = 13;
            this.EnrichmentOptions_default_button.Text = "Default";
            this.EnrichmentOptions_default_button.UseVisualStyleBackColor = false;
            this.EnrichmentOptions_default_button.Click += new System.EventHandler(this.ResetOptions_button_Click);
            // 
            // EnrichmentOptions_scpInteractionsLevel_2_label
            // 
            this.EnrichmentOptions_scpInteractionsLevel_2_label.AutoSize = true;
            this.EnrichmentOptions_scpInteractionsLevel_2_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_scpInteractionsLevel_2_label.Location = new System.Drawing.Point(208, 2);
            this.EnrichmentOptions_scpInteractionsLevel_2_label.Name = "EnrichmentOptions_scpInteractionsLevel_2_label";
            this.EnrichmentOptions_scpInteractionsLevel_2_label.Size = new System.Drawing.Size(18, 19);
            this.EnrichmentOptions_scpInteractionsLevel_2_label.TabIndex = 35;
            this.EnrichmentOptions_scpInteractionsLevel_2_label.Text = "2";
            // 
            // EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox
            // 
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox.Location = new System.Drawing.Point(229, 29);
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox.Name = "EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox";
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox.TabIndex = 41;
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_scpInteractionsLevel_3_label
            // 
            this.EnrichmentOptions_scpInteractionsLevel_3_label.AutoSize = true;
            this.EnrichmentOptions_scpInteractionsLevel_3_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_scpInteractionsLevel_3_label.Location = new System.Drawing.Point(232, 2);
            this.EnrichmentOptions_scpInteractionsLevel_3_label.Name = "EnrichmentOptions_scpInteractionsLevel_3_label";
            this.EnrichmentOptions_scpInteractionsLevel_3_label.Size = new System.Drawing.Size(18, 19);
            this.EnrichmentOptions_scpInteractionsLevel_3_label.TabIndex = 36;
            this.EnrichmentOptions_scpInteractionsLevel_3_label.Text = "3";
            // 
            // EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox
            // 
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox.Location = new System.Drawing.Point(205, 29);
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox.Name = "EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox";
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox.TabIndex = 40;
            this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_cutoffs_panel
            // 
            this.EnrichmentOptions_cutoffs_panel.Border_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_dynamicPvalue_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_maxPvalue_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_dynamicKeepTopScps_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_maxRanks_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_keepScps_level_4_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_keepScps_level_3_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_keepScps_level_2_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_standardPvalue_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_keepScpsScpLevel_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_keepScps_level_1_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_standardKeepTopScps_label);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox);
            this.EnrichmentOptions_cutoffs_panel.Controls.Add(this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox);
            this.EnrichmentOptions_cutoffs_panel.Corner_radius = 10F;
            this.EnrichmentOptions_cutoffs_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EnrichmentOptions_cutoffs_panel.Location = new System.Drawing.Point(4, 4);
            this.EnrichmentOptions_cutoffs_panel.Name = "EnrichmentOptions_cutoffs_panel";
            this.EnrichmentOptions_cutoffs_panel.Size = new System.Drawing.Size(343, 88);
            this.EnrichmentOptions_cutoffs_panel.TabIndex = 221;
            // 
            // EnrichmentOptions_dynamicPvalue_textBox
            // 
            this.EnrichmentOptions_dynamicPvalue_textBox.Location = new System.Drawing.Point(291, 63);
            this.EnrichmentOptions_dynamicPvalue_textBox.Name = "EnrichmentOptions_dynamicPvalue_textBox";
            this.EnrichmentOptions_dynamicPvalue_textBox.Size = new System.Drawing.Size(40, 22);
            this.EnrichmentOptions_dynamicPvalue_textBox.TabIndex = 47;
            this.EnrichmentOptions_dynamicPvalue_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_dynamicPvalue_textBox_TextChanged);
            // 
            // EnrichmentOptions_maxPvalue_label
            // 
            this.EnrichmentOptions_maxPvalue_label.AutoSize = true;
            this.EnrichmentOptions_maxPvalue_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_maxPvalue_label.Location = new System.Drawing.Point(279, 2);
            this.EnrichmentOptions_maxPvalue_label.Name = "EnrichmentOptions_maxPvalue_label";
            this.EnrichmentOptions_maxPvalue_label.Size = new System.Drawing.Size(66, 38);
            this.EnrichmentOptions_maxPvalue_label.TabIndex = 47;
            this.EnrichmentOptions_maxPvalue_label.Text = "Max\r\np-value";
            this.EnrichmentOptions_maxPvalue_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EnrichmentOptions_dynamicKeepTopScps_label
            // 
            this.EnrichmentOptions_dynamicKeepTopScps_label.AutoSize = true;
            this.EnrichmentOptions_dynamicKeepTopScps_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_dynamicKeepTopScps_label.Location = new System.Drawing.Point(4, 67);
            this.EnrichmentOptions_dynamicKeepTopScps_label.Name = "EnrichmentOptions_dynamicKeepTopScps_label";
            this.EnrichmentOptions_dynamicKeepTopScps_label.Size = new System.Drawing.Size(169, 19);
            this.EnrichmentOptions_dynamicKeepTopScps_label.TabIndex = 24;
            this.EnrichmentOptions_dynamicKeepTopScps_label.Text = "Dynamic enrichment";
            // 
            // EnrichmentOptions_maxRanks_label
            // 
            this.EnrichmentOptions_maxRanks_label.AutoSize = true;
            this.EnrichmentOptions_maxRanks_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_maxRanks_label.Location = new System.Drawing.Point(177, 3);
            this.EnrichmentOptions_maxRanks_label.Name = "EnrichmentOptions_maxRanks_label";
            this.EnrichmentOptions_maxRanks_label.Size = new System.Drawing.Size(232, 19);
            this.EnrichmentOptions_maxRanks_label.TabIndex = 55;
            this.EnrichmentOptions_maxRanks_label.Text = "Max ranks for indicated level";
            // 
            // EnrichmentOptions_keepScps_level_4_label
            // 
            this.EnrichmentOptions_keepScps_level_4_label.AutoSize = true;
            this.EnrichmentOptions_keepScps_level_4_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_keepScps_level_4_label.Location = new System.Drawing.Point(250, 21);
            this.EnrichmentOptions_keepScps_level_4_label.Name = "EnrichmentOptions_keepScps_level_4_label";
            this.EnrichmentOptions_keepScps_level_4_label.Size = new System.Drawing.Size(18, 19);
            this.EnrichmentOptions_keepScps_level_4_label.TabIndex = 23;
            this.EnrichmentOptions_keepScps_level_4_label.Text = "4";
            // 
            // EnrichmentOptions_keepScps_level_3_label
            // 
            this.EnrichmentOptions_keepScps_level_3_label.AutoSize = true;
            this.EnrichmentOptions_keepScps_level_3_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_keepScps_level_3_label.Location = new System.Drawing.Point(226, 21);
            this.EnrichmentOptions_keepScps_level_3_label.Name = "EnrichmentOptions_keepScps_level_3_label";
            this.EnrichmentOptions_keepScps_level_3_label.Size = new System.Drawing.Size(18, 19);
            this.EnrichmentOptions_keepScps_level_3_label.TabIndex = 22;
            this.EnrichmentOptions_keepScps_level_3_label.Text = "3";
            // 
            // EnrichmentOptions_keepScps_level_2_label
            // 
            this.EnrichmentOptions_keepScps_level_2_label.AutoSize = true;
            this.EnrichmentOptions_keepScps_level_2_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_keepScps_level_2_label.Location = new System.Drawing.Point(202, 21);
            this.EnrichmentOptions_keepScps_level_2_label.Name = "EnrichmentOptions_keepScps_level_2_label";
            this.EnrichmentOptions_keepScps_level_2_label.Size = new System.Drawing.Size(18, 19);
            this.EnrichmentOptions_keepScps_level_2_label.TabIndex = 21;
            this.EnrichmentOptions_keepScps_level_2_label.Text = "2";
            // 
            // EnrichmentOptions_standardPvalue_textBox
            // 
            this.EnrichmentOptions_standardPvalue_textBox.Location = new System.Drawing.Point(291, 40);
            this.EnrichmentOptions_standardPvalue_textBox.Name = "EnrichmentOptions_standardPvalue_textBox";
            this.EnrichmentOptions_standardPvalue_textBox.Size = new System.Drawing.Size(40, 22);
            this.EnrichmentOptions_standardPvalue_textBox.TabIndex = 46;
            this.EnrichmentOptions_standardPvalue_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_standardPvalue_textBox_TextChanged);
            // 
            // EnrichmentOptions_keepScpsScpLevel_label
            // 
            this.EnrichmentOptions_keepScpsScpLevel_label.AutoSize = true;
            this.EnrichmentOptions_keepScpsScpLevel_label.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnrichmentOptions_keepScpsScpLevel_label.Location = new System.Drawing.Point(91, 20);
            this.EnrichmentOptions_keepScpsScpLevel_label.Name = "EnrichmentOptions_keepScpsScpLevel_label";
            this.EnrichmentOptions_keepScpsScpLevel_label.Size = new System.Drawing.Size(0, 19);
            this.EnrichmentOptions_keepScpsScpLevel_label.TabIndex = 29;
            // 
            // EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox
            // 
            this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox.Location = new System.Drawing.Point(247, 40);
            this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox.Name = "EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox";
            this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox.TabIndex = 45;
            this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_keepScps_level_1_label
            // 
            this.EnrichmentOptions_keepScps_level_1_label.AutoSize = true;
            this.EnrichmentOptions_keepScps_level_1_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_keepScps_level_1_label.Location = new System.Drawing.Point(178, 21);
            this.EnrichmentOptions_keepScps_level_1_label.Name = "EnrichmentOptions_keepScps_level_1_label";
            this.EnrichmentOptions_keepScps_level_1_label.Size = new System.Drawing.Size(18, 19);
            this.EnrichmentOptions_keepScps_level_1_label.TabIndex = 20;
            this.EnrichmentOptions_keepScps_level_1_label.Text = "1";
            // 
            // EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox
            // 
            this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox.Location = new System.Drawing.Point(223, 40);
            this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox.Name = "EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox";
            this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox.TabIndex = 44;
            this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_standardKeepTopScps_label
            // 
            this.EnrichmentOptions_standardKeepTopScps_label.AutoSize = true;
            this.EnrichmentOptions_standardKeepTopScps_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EnrichmentOptions_standardKeepTopScps_label.Location = new System.Drawing.Point(-1, 43);
            this.EnrichmentOptions_standardKeepTopScps_label.Name = "EnrichmentOptions_standardKeepTopScps_label";
            this.EnrichmentOptions_standardKeepTopScps_label.Size = new System.Drawing.Size(174, 19);
            this.EnrichmentOptions_standardKeepTopScps_label.TabIndex = 15;
            this.EnrichmentOptions_standardKeepTopScps_label.Text = "Standard enrichment";
            // 
            // EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox
            // 
            this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox.Location = new System.Drawing.Point(223, 63);
            this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox.Name = "EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox";
            this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox.TabIndex = 43;
            this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox
            // 
            this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox.Location = new System.Drawing.Point(175, 40);
            this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox.Name = "EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox";
            this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox.TabIndex = 38;
            this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox
            // 
            this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox.Location = new System.Drawing.Point(199, 63);
            this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox.Name = "EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox";
            this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox.TabIndex = 42;
            this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox_TextChanged);
            // 
            // EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox
            // 
            this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox.Location = new System.Drawing.Point(199, 40);
            this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox.Name = "EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox";
            this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox.Size = new System.Drawing.Size(23, 22);
            this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox.TabIndex = 39;
            this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox.TextChanged += new System.EventHandler(this.EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox_TextChanged);
            // 
            // Options_dataSignificance_panel
            // 
            this.Options_dataSignificance_panel.Border_color = System.Drawing.Color.Black;
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_allGenesSignificant_cbLabel);
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_allGenesSignificant_cbButton);
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_allGenesSignificant_headline_label);
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_resetParameter_button);
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_sigSubject_explanation_label);
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_resetSig_button);
            this.Options_dataSignificance_panel.Controls.Add(this.SigData_sigSelection_panel);
            this.Options_dataSignificance_panel.Corner_radius = 10F;
            this.Options_dataSignificance_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_dataSignificance_panel.Location = new System.Drawing.Point(34, 595);
            this.Options_dataSignificance_panel.Name = "Options_dataSignificance_panel";
            this.Options_dataSignificance_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_dataSignificance_panel.TabIndex = 218;
            // 
            // SigData_allGenesSignificant_cbLabel
            // 
            this.SigData_allGenesSignificant_cbLabel.AutoSize = true;
            this.SigData_allGenesSignificant_cbLabel.Location = new System.Drawing.Point(55, 425);
            this.SigData_allGenesSignificant_cbLabel.Name = "SigData_allGenesSignificant_cbLabel";
            this.SigData_allGenesSignificant_cbLabel.Size = new System.Drawing.Size(145, 16);
            this.SigData_allGenesSignificant_cbLabel.TabIndex = 271;
            this.SigData_allGenesSignificant_cbLabel.Text = "All genes are significant";
            // 
            // SigData_allGenesSignificant_cbButton
            // 
            this.SigData_allGenesSignificant_cbButton.Checked = false;
            this.SigData_allGenesSignificant_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_allGenesSignificant_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_allGenesSignificant_cbButton.Location = new System.Drawing.Point(29, 419);
            this.SigData_allGenesSignificant_cbButton.Name = "SigData_allGenesSignificant_cbButton";
            this.SigData_allGenesSignificant_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_allGenesSignificant_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_allGenesSignificant_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_allGenesSignificant_cbButton.TabIndex = 262;
            this.SigData_allGenesSignificant_cbButton.Text = "MyCheckBox_button10";
            this.SigData_allGenesSignificant_cbButton.UseVisualStyleBackColor = true;
            this.SigData_allGenesSignificant_cbButton.Click += new System.EventHandler(this.SigData_allGenesSignificant_cbButton_Click);
            // 
            // SigData_allGenesSignificant_headline_label
            // 
            this.SigData_allGenesSignificant_headline_label.AutoSize = true;
            this.SigData_allGenesSignificant_headline_label.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SigData_allGenesSignificant_headline_label.Location = new System.Drawing.Point(4, 391);
            this.SigData_allGenesSignificant_headline_label.Name = "SigData_allGenesSignificant_headline_label";
            this.SigData_allGenesSignificant_headline_label.Size = new System.Drawing.Size(178, 19);
            this.SigData_allGenesSignificant_headline_label.TabIndex = 251;
            this.SigData_allGenesSignificant_headline_label.Text = "No significance cutoff";
            // 
            // SigData_resetParameter_button
            // 
            this.SigData_resetParameter_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SigData_resetParameter_button.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.SigData_resetParameter_button.ForeColor = System.Drawing.Color.White;
            this.SigData_resetParameter_button.Location = new System.Drawing.Point(180, 442);
            this.SigData_resetParameter_button.Name = "SigData_resetParameter_button";
            this.SigData_resetParameter_button.Size = new System.Drawing.Size(174, 48);
            this.SigData_resetParameter_button.TabIndex = 248;
            this.SigData_resetParameter_button.Text = "Reset to current cutoffs";
            this.SigData_resetParameter_button.UseVisualStyleBackColor = false;
            this.SigData_resetParameter_button.Click += new System.EventHandler(this.SigData_resetParameter_button_Click);
            // 
            // SigData_sigSubject_explanation_label
            // 
            this.SigData_sigSubject_explanation_label.AutoSize = true;
            this.SigData_sigSubject_explanation_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_sigSubject_explanation_label.Location = new System.Drawing.Point(35, 484);
            this.SigData_sigSubject_explanation_label.Name = "SigData_sigSubject_explanation_label";
            this.SigData_sigSubject_explanation_label.Size = new System.Drawing.Size(267, 38);
            this.SigData_sigSubject_explanation_label.TabIndex = 236;
            this.SigData_sigSubject_explanation_label.Text = "Only significant genes will be\r\nsubjected to enrichment analysis";
            // 
            // SigData_resetSig_button
            // 
            this.SigData_resetSig_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SigData_resetSig_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_resetSig_button.ForeColor = System.Drawing.Color.White;
            this.SigData_resetSig_button.Location = new System.Drawing.Point(5, 442);
            this.SigData_resetSig_button.Name = "SigData_resetSig_button";
            this.SigData_resetSig_button.Size = new System.Drawing.Size(174, 48);
            this.SigData_resetSig_button.TabIndex = 220;
            this.SigData_resetSig_button.Text = "Adjust significance";
            this.SigData_resetSig_button.UseVisualStyleBackColor = false;
            this.SigData_resetSig_button.Click += new System.EventHandler(this.SigData_resetSig_button_Click);
            // 
            // SigData_sigSelection_panel
            // 
            this.SigData_sigSelection_panel.Border_color = System.Drawing.Color.Transparent;
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_deleteNotSignGenes_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keep_mergeUpDown_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keep_eachDataset_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_rankBy_2ndValue_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_rankBy_1stValue_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_lowerSig_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_higherSig_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_lowerSig_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_higherSig_cbLabel);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_deleteNotSignGenes_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keep_eachDataset_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keep_mergeUpDown_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_rankBy_2ndValue_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_rankBy_1stValue_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_lowerSig_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_higherSig_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_lowerSig_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_higherSig_cbButton);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_cutoff_expl_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_cutoff_expl_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_rankBy_left_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_tieBreaker_explanation_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_rankBy_top_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_lowerSig_expl_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_higherSig_expl_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_2nd_sigCutoff_headline_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_higherSig_expl_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_lowerSig_expl_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keepTopRanks_right_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keepTopRanks_ownTextBox);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_keepTopRanks_left_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_1st_sigCutoff_headline_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_cutoff_ownTextBox);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_cutoff_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_cutoff_ownTextBox);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_cutoff_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value2nd_headline_label);
            this.SigData_sigSelection_panel.Controls.Add(this.SigData_value1st_headline_label);
            this.SigData_sigSelection_panel.Corner_radius = 10F;
            this.SigData_sigSelection_panel.Fill_color = System.Drawing.Color.Transparent;
            this.SigData_sigSelection_panel.Location = new System.Drawing.Point(5, 8);
            this.SigData_sigSelection_panel.Name = "SigData_sigSelection_panel";
            this.SigData_sigSelection_panel.Size = new System.Drawing.Size(353, 382);
            this.SigData_sigSelection_panel.TabIndex = 252;
            // 
            // SigData_deleteNotSignGenes_cbLabel
            // 
            this.SigData_deleteNotSignGenes_cbLabel.AutoSize = true;
            this.SigData_deleteNotSignGenes_cbLabel.Location = new System.Drawing.Point(50, 361);
            this.SigData_deleteNotSignGenes_cbLabel.Name = "SigData_deleteNotSignGenes_cbLabel";
            this.SigData_deleteNotSignGenes_cbLabel.Size = new System.Drawing.Size(242, 16);
            this.SigData_deleteNotSignGenes_cbLabel.TabIndex = 270;
            this.SigData_deleteNotSignGenes_cbLabel.Text = "Permanently delete not significant genes";
            // 
            // SigData_keep_mergeUpDown_cbLabel
            // 
            this.SigData_keep_mergeUpDown_cbLabel.AutoSize = true;
            this.SigData_keep_mergeUpDown_cbLabel.Location = new System.Drawing.Point(21, 331);
            this.SigData_keep_mergeUpDown_cbLabel.Name = "SigData_keep_mergeUpDown_cbLabel";
            this.SigData_keep_mergeUpDown_cbLabel.Size = new System.Drawing.Size(45, 16);
            this.SigData_keep_mergeUpDown_cbLabel.TabIndex = 269;
            this.SigData_keep_mergeUpDown_cbLabel.Text = "with or";
            // 
            // SigData_keep_eachDataset_cbLabel
            // 
            this.SigData_keep_eachDataset_cbLabel.AutoSize = true;
            this.SigData_keep_eachDataset_cbLabel.Location = new System.Drawing.Point(128, 306);
            this.SigData_keep_eachDataset_cbLabel.Name = "SigData_keep_eachDataset_cbLabel";
            this.SigData_keep_eachDataset_cbLabel.Size = new System.Drawing.Size(384, 16);
            this.SigData_keep_eachDataset_cbLabel.TabIndex = 268;
            this.SigData_keep_eachDataset_cbLabel.Text = "without temp. merging datasets that differ only in Up/Down status";
            // 
            // SigData_rankBy_2ndValue_cbLabel
            // 
            this.SigData_rankBy_2ndValue_cbLabel.AutoSize = true;
            this.SigData_rankBy_2ndValue_cbLabel.Location = new System.Drawing.Point(170, 277);
            this.SigData_rankBy_2ndValue_cbLabel.Name = "SigData_rankBy_2ndValue_cbLabel";
            this.SigData_rankBy_2ndValue_cbLabel.Size = new System.Drawing.Size(61, 16);
            this.SigData_rankBy_2ndValue_cbLabel.TabIndex = 267;
            this.SigData_rankBy_2ndValue_cbLabel.Text = "2nd value";
            // 
            // SigData_rankBy_1stValue_cbLabel
            // 
            this.SigData_rankBy_1stValue_cbLabel.AutoSize = true;
            this.SigData_rankBy_1stValue_cbLabel.Location = new System.Drawing.Point(162, 255);
            this.SigData_rankBy_1stValue_cbLabel.Name = "SigData_rankBy_1stValue_cbLabel";
            this.SigData_rankBy_1stValue_cbLabel.Size = new System.Drawing.Size(58, 16);
            this.SigData_rankBy_1stValue_cbLabel.TabIndex = 266;
            this.SigData_rankBy_1stValue_cbLabel.Text = "1st value";
            // 
            // SigData_value2nd_lowerSig_cbLabel
            // 
            this.SigData_value2nd_lowerSig_cbLabel.AutoSize = true;
            this.SigData_value2nd_lowerSig_cbLabel.Location = new System.Drawing.Point(76, 114);
            this.SigData_value2nd_lowerSig_cbLabel.Name = "SigData_value2nd_lowerSig_cbLabel";
            this.SigData_value2nd_lowerSig_cbLabel.Size = new System.Drawing.Size(155, 16);
            this.SigData_value2nd_lowerSig_cbLabel.TabIndex = 265;
            this.SigData_value2nd_lowerSig_cbLabel.Text = "lower absolute 2nd values";
            // 
            // SigData_value2nd_higherSig_cbLabel
            // 
            this.SigData_value2nd_higherSig_cbLabel.AutoSize = true;
            this.SigData_value2nd_higherSig_cbLabel.Location = new System.Drawing.Point(72, 91);
            this.SigData_value2nd_higherSig_cbLabel.Name = "SigData_value2nd_higherSig_cbLabel";
            this.SigData_value2nd_higherSig_cbLabel.Size = new System.Drawing.Size(160, 16);
            this.SigData_value2nd_higherSig_cbLabel.TabIndex = 264;
            this.SigData_value2nd_higherSig_cbLabel.Text = "higher absolute 2nd values";
            // 
            // SigData_value1st_lowerSig_cbLabel
            // 
            this.SigData_value1st_lowerSig_cbLabel.AutoSize = true;
            this.SigData_value1st_lowerSig_cbLabel.Location = new System.Drawing.Point(72, 48);
            this.SigData_value1st_lowerSig_cbLabel.Name = "SigData_value1st_lowerSig_cbLabel";
            this.SigData_value1st_lowerSig_cbLabel.Size = new System.Drawing.Size(152, 16);
            this.SigData_value1st_lowerSig_cbLabel.TabIndex = 263;
            this.SigData_value1st_lowerSig_cbLabel.Text = "lower absolute 1st values";
            // 
            // SigData_value1st_higherSig_cbLabel
            // 
            this.SigData_value1st_higherSig_cbLabel.AutoSize = true;
            this.SigData_value1st_higherSig_cbLabel.Location = new System.Drawing.Point(72, 26);
            this.SigData_value1st_higherSig_cbLabel.Name = "SigData_value1st_higherSig_cbLabel";
            this.SigData_value1st_higherSig_cbLabel.Size = new System.Drawing.Size(157, 16);
            this.SigData_value1st_higherSig_cbLabel.TabIndex = 262;
            this.SigData_value1st_higherSig_cbLabel.Text = "higher absolute 1st values";
            // 
            // SigData_deleteNotSignGenes_cbButton
            // 
            this.SigData_deleteNotSignGenes_cbButton.Checked = false;
            this.SigData_deleteNotSignGenes_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_deleteNotSignGenes_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_deleteNotSignGenes_cbButton.Location = new System.Drawing.Point(24, 358);
            this.SigData_deleteNotSignGenes_cbButton.Name = "SigData_deleteNotSignGenes_cbButton";
            this.SigData_deleteNotSignGenes_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_deleteNotSignGenes_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_deleteNotSignGenes_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_deleteNotSignGenes_cbButton.TabIndex = 261;
            this.SigData_deleteNotSignGenes_cbButton.Text = "MyCheckBox_button9";
            this.SigData_deleteNotSignGenes_cbButton.UseVisualStyleBackColor = true;
            this.SigData_deleteNotSignGenes_cbButton.Click += new System.EventHandler(this.SigData_deleteNotSignGenes_cbButton_Click);
            // 
            // SigData_keep_eachDataset_cbButton
            // 
            this.SigData_keep_eachDataset_cbButton.Checked = false;
            this.SigData_keep_eachDataset_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_keep_eachDataset_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_keep_eachDataset_cbButton.Location = new System.Drawing.Point(106, 303);
            this.SigData_keep_eachDataset_cbButton.Name = "SigData_keep_eachDataset_cbButton";
            this.SigData_keep_eachDataset_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_keep_eachDataset_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_keep_eachDataset_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_keep_eachDataset_cbButton.TabIndex = 260;
            this.SigData_keep_eachDataset_cbButton.Text = "MyCheckBox_button8";
            this.SigData_keep_eachDataset_cbButton.UseVisualStyleBackColor = true;
            this.SigData_keep_eachDataset_cbButton.Click += new System.EventHandler(this.SigData_keep_eachDataset_cbButton_Click);
            // 
            // SigData_keep_mergeUpDown_cbButton
            // 
            this.SigData_keep_mergeUpDown_cbButton.Checked = false;
            this.SigData_keep_mergeUpDown_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_keep_mergeUpDown_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_keep_mergeUpDown_cbButton.Location = new System.Drawing.Point(24, 304);
            this.SigData_keep_mergeUpDown_cbButton.Name = "SigData_keep_mergeUpDown_cbButton";
            this.SigData_keep_mergeUpDown_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_keep_mergeUpDown_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_keep_mergeUpDown_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_keep_mergeUpDown_cbButton.TabIndex = 259;
            this.SigData_keep_mergeUpDown_cbButton.Text = "MyCheckBox_button7";
            this.SigData_keep_mergeUpDown_cbButton.UseVisualStyleBackColor = true;
            this.SigData_keep_mergeUpDown_cbButton.Click += new System.EventHandler(this.SigData_keep_mergeUpDown_cbButton_Click);
            // 
            // SigData_rankBy_2ndValue_cbButton
            // 
            this.SigData_rankBy_2ndValue_cbButton.Checked = false;
            this.SigData_rankBy_2ndValue_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_2ndValue_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_2ndValue_cbButton.Location = new System.Drawing.Point(143, 272);
            this.SigData_rankBy_2ndValue_cbButton.Name = "SigData_rankBy_2ndValue_cbButton";
            this.SigData_rankBy_2ndValue_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_2ndValue_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_2ndValue_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_rankBy_2ndValue_cbButton.TabIndex = 258;
            this.SigData_rankBy_2ndValue_cbButton.Text = "MyCheckBox_button6";
            this.SigData_rankBy_2ndValue_cbButton.UseVisualStyleBackColor = true;
            this.SigData_rankBy_2ndValue_cbButton.Click += new System.EventHandler(this.SigData_rankBy_2ndValue_cbButton_Click);
            // 
            // SigData_rankBy_1stValue_cbButton
            // 
            this.SigData_rankBy_1stValue_cbButton.Checked = false;
            this.SigData_rankBy_1stValue_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_1stValue_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_1stValue_cbButton.Location = new System.Drawing.Point(136, 250);
            this.SigData_rankBy_1stValue_cbButton.Name = "SigData_rankBy_1stValue_cbButton";
            this.SigData_rankBy_1stValue_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_1stValue_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_rankBy_1stValue_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_rankBy_1stValue_cbButton.TabIndex = 257;
            this.SigData_rankBy_1stValue_cbButton.Text = "MyCheckBox_button5";
            this.SigData_rankBy_1stValue_cbButton.UseVisualStyleBackColor = true;
            this.SigData_rankBy_1stValue_cbButton.Click += new System.EventHandler(this.SigData_rankBy_1stValue_cbButton_Click);
            // 
            // SigData_value2nd_lowerSig_cbButton
            // 
            this.SigData_value2nd_lowerSig_cbButton.Checked = false;
            this.SigData_value2nd_lowerSig_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_lowerSig_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_lowerSig_cbButton.Location = new System.Drawing.Point(34, 111);
            this.SigData_value2nd_lowerSig_cbButton.Name = "SigData_value2nd_lowerSig_cbButton";
            this.SigData_value2nd_lowerSig_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_lowerSig_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_lowerSig_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_value2nd_lowerSig_cbButton.TabIndex = 256;
            this.SigData_value2nd_lowerSig_cbButton.Text = "MyCheckBox_button4";
            this.SigData_value2nd_lowerSig_cbButton.UseVisualStyleBackColor = true;
            this.SigData_value2nd_lowerSig_cbButton.Click += new System.EventHandler(this.SigData_value2nd_lowerSig_cbButton_Click);
            // 
            // SigData_value2nd_higherSig_cbButton
            // 
            this.SigData_value2nd_higherSig_cbButton.Checked = false;
            this.SigData_value2nd_higherSig_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_higherSig_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_higherSig_cbButton.Location = new System.Drawing.Point(34, 86);
            this.SigData_value2nd_higherSig_cbButton.Name = "SigData_value2nd_higherSig_cbButton";
            this.SigData_value2nd_higherSig_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_higherSig_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value2nd_higherSig_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_value2nd_higherSig_cbButton.TabIndex = 255;
            this.SigData_value2nd_higherSig_cbButton.Text = "MyCheckBox_button3";
            this.SigData_value2nd_higherSig_cbButton.UseVisualStyleBackColor = true;
            this.SigData_value2nd_higherSig_cbButton.Click += new System.EventHandler(this.SigData_value2nd_higherSig_cbButton_Click);
            // 
            // SigData_value1st_lowerSig_cbButton
            // 
            this.SigData_value1st_lowerSig_cbButton.Checked = false;
            this.SigData_value1st_lowerSig_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_value1st_lowerSig_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value1st_lowerSig_cbButton.Location = new System.Drawing.Point(34, 45);
            this.SigData_value1st_lowerSig_cbButton.Name = "SigData_value1st_lowerSig_cbButton";
            this.SigData_value1st_lowerSig_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_value1st_lowerSig_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value1st_lowerSig_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_value1st_lowerSig_cbButton.TabIndex = 254;
            this.SigData_value1st_lowerSig_cbButton.Text = "MyCheckBox_button2";
            this.SigData_value1st_lowerSig_cbButton.UseVisualStyleBackColor = true;
            this.SigData_value1st_lowerSig_cbButton.Click += new System.EventHandler(this.SigData_value1st_lowerSig_cbButton_Click);
            // 
            // SigData_value1st_higherSig_cbButton
            // 
            this.SigData_value1st_higherSig_cbButton.Checked = false;
            this.SigData_value1st_higherSig_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.SigData_value1st_higherSig_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value1st_higherSig_cbButton.Location = new System.Drawing.Point(34, 21);
            this.SigData_value1st_higherSig_cbButton.Name = "SigData_value1st_higherSig_cbButton";
            this.SigData_value1st_higherSig_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.SigData_value1st_higherSig_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.SigData_value1st_higherSig_cbButton.Size = new System.Drawing.Size(20, 23);
            this.SigData_value1st_higherSig_cbButton.TabIndex = 253;
            this.SigData_value1st_higherSig_cbButton.Text = "MyCheckBox_button1";
            this.SigData_value1st_higherSig_cbButton.UseVisualStyleBackColor = true;
            this.SigData_value1st_higherSig_cbButton.Click += new System.EventHandler(this.SigData_value1st_higherSig_cbButton_Click);
            // 
            // SigData_value1st_cutoff_expl_label
            // 
            this.SigData_value1st_cutoff_expl_label.AutoSize = true;
            this.SigData_value1st_cutoff_expl_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value1st_cutoff_expl_label.Location = new System.Drawing.Point(275, 159);
            this.SigData_value1st_cutoff_expl_label.Name = "SigData_value1st_cutoff_expl_label";
            this.SigData_value1st_cutoff_expl_label.Size = new System.Drawing.Size(141, 19);
            this.SigData_value1st_cutoff_expl_label.TabIndex = 252;
            this.SigData_value1st_cutoff_expl_label.Text = "(e.g. for log2(fc))";
            // 
            // SigData_value2nd_cutoff_expl_label
            // 
            this.SigData_value2nd_cutoff_expl_label.AutoSize = true;
            this.SigData_value2nd_cutoff_expl_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value2nd_cutoff_expl_label.Location = new System.Drawing.Point(278, 185);
            this.SigData_value2nd_cutoff_expl_label.Name = "SigData_value2nd_cutoff_expl_label";
            this.SigData_value2nd_cutoff_expl_label.Size = new System.Drawing.Size(141, 19);
            this.SigData_value2nd_cutoff_expl_label.TabIndex = 251;
            this.SigData_value2nd_cutoff_expl_label.Text = "(e.g. for log2(fc))";
            // 
            // SigData_rankBy_left_label
            // 
            this.SigData_rankBy_left_label.AutoSize = true;
            this.SigData_rankBy_left_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.SigData_rankBy_left_label.Location = new System.Drawing.Point(2, 264);
            this.SigData_rankBy_left_label.Name = "SigData_rankBy_left_label";
            this.SigData_rankBy_left_label.Size = new System.Drawing.Size(111, 18);
            this.SigData_rankBy_left_label.TabIndex = 243;
            this.SigData_rankBy_left_label.Text = "significance of";
            // 
            // SigData_tieBreaker_explanation_label
            // 
            this.SigData_tieBreaker_explanation_label.AutoSize = true;
            this.SigData_tieBreaker_explanation_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_tieBreaker_explanation_label.Location = new System.Drawing.Point(217, 256);
            this.SigData_tieBreaker_explanation_label.Name = "SigData_tieBreaker_explanation_label";
            this.SigData_tieBreaker_explanation_label.Size = new System.Drawing.Size(153, 38);
            this.SigData_tieBreaker_explanation_label.TabIndex = 249;
            this.SigData_tieBreaker_explanation_label.Text = "Not selected value\r\nwill be tie breaker";
            // 
            // SigData_rankBy_top_label
            // 
            this.SigData_rankBy_top_label.AutoSize = true;
            this.SigData_rankBy_top_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.SigData_rankBy_top_label.Location = new System.Drawing.Point(2, 234);
            this.SigData_rankBy_top_label.Name = "SigData_rankBy_top_label";
            this.SigData_rankBy_top_label.Size = new System.Drawing.Size(335, 18);
            this.SigData_rankBy_top_label.TabIndex = 241;
            this.SigData_rankBy_top_label.Text = "Rank genes within each dataset by decreasing";
            // 
            // SigData_value2nd_lowerSig_expl_label
            // 
            this.SigData_value2nd_lowerSig_expl_label.AutoSize = true;
            this.SigData_value2nd_lowerSig_expl_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value2nd_lowerSig_expl_label.Location = new System.Drawing.Point(221, 112);
            this.SigData_value2nd_lowerSig_expl_label.Name = "SigData_value2nd_lowerSig_expl_label";
            this.SigData_value2nd_lowerSig_expl_label.Size = new System.Drawing.Size(148, 19);
            this.SigData_value2nd_lowerSig_expl_label.TabIndex = 247;
            this.SigData_value2nd_lowerSig_expl_label.Text = "(e.g. for p-values)";
            // 
            // SigData_value1st_higherSig_expl_label
            // 
            this.SigData_value1st_higherSig_expl_label.AutoSize = true;
            this.SigData_value1st_higherSig_expl_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value1st_higherSig_expl_label.Location = new System.Drawing.Point(221, 27);
            this.SigData_value1st_higherSig_expl_label.Name = "SigData_value1st_higherSig_expl_label";
            this.SigData_value1st_higherSig_expl_label.Size = new System.Drawing.Size(141, 19);
            this.SigData_value1st_higherSig_expl_label.TabIndex = 244;
            this.SigData_value1st_higherSig_expl_label.Text = "(e.g. for log2(fc))";
            // 
            // SigData_2nd_sigCutoff_headline_label
            // 
            this.SigData_2nd_sigCutoff_headline_label.AutoSize = true;
            this.SigData_2nd_sigCutoff_headline_label.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SigData_2nd_sigCutoff_headline_label.Location = new System.Drawing.Point(3, 211);
            this.SigData_2nd_sigCutoff_headline_label.Name = "SigData_2nd_sigCutoff_headline_label";
            this.SigData_2nd_sigCutoff_headline_label.Size = new System.Drawing.Size(215, 19);
            this.SigData_2nd_sigCutoff_headline_label.TabIndex = 231;
            this.SigData_2nd_sigCutoff_headline_label.Text = "Second significance cutoff";
            // 
            // SigData_value2nd_higherSig_expl_label
            // 
            this.SigData_value2nd_higherSig_expl_label.AutoSize = true;
            this.SigData_value2nd_higherSig_expl_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value2nd_higherSig_expl_label.Location = new System.Drawing.Point(221, 91);
            this.SigData_value2nd_higherSig_expl_label.Name = "SigData_value2nd_higherSig_expl_label";
            this.SigData_value2nd_higherSig_expl_label.Size = new System.Drawing.Size(141, 19);
            this.SigData_value2nd_higherSig_expl_label.TabIndex = 246;
            this.SigData_value2nd_higherSig_expl_label.Text = "(e.g. for log2(fc))";
            // 
            // SigData_value1st_lowerSig_expl_label
            // 
            this.SigData_value1st_lowerSig_expl_label.AutoSize = true;
            this.SigData_value1st_lowerSig_expl_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value1st_lowerSig_expl_label.Location = new System.Drawing.Point(221, 48);
            this.SigData_value1st_lowerSig_expl_label.Name = "SigData_value1st_lowerSig_expl_label";
            this.SigData_value1st_lowerSig_expl_label.Size = new System.Drawing.Size(148, 19);
            this.SigData_value1st_lowerSig_expl_label.TabIndex = 245;
            this.SigData_value1st_lowerSig_expl_label.Text = "(e.g. for p-values)";
            // 
            // SigData_keepTopRanks_right_label
            // 
            this.SigData_keepTopRanks_right_label.AutoSize = true;
            this.SigData_keepTopRanks_right_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.SigData_keepTopRanks_right_label.Location = new System.Drawing.Point(127, 339);
            this.SigData_keepTopRanks_right_label.Name = "SigData_keepTopRanks_right_label";
            this.SigData_keepTopRanks_right_label.Size = new System.Drawing.Size(202, 18);
            this.SigData_keepTopRanks_right_label.TabIndex = 235;
            this.SigData_keepTopRanks_right_label.Text = "ranked genes as significant";
            // 
            // SigData_keepTopRanks_ownTextBox
            // 
            this.SigData_keepTopRanks_ownTextBox.Location = new System.Drawing.Point(85, 336);
            this.SigData_keepTopRanks_ownTextBox.Name = "SigData_keepTopRanks_ownTextBox";
            this.SigData_keepTopRanks_ownTextBox.Size = new System.Drawing.Size(41, 22);
            this.SigData_keepTopRanks_ownTextBox.TabIndex = 233;
            this.SigData_keepTopRanks_ownTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SigData_keepTopRanks_ownTextBox.TextChanged += new System.EventHandler(this.SigData_keepTopRanks_ownTextBox_TextChanged);
            // 
            // SigData_keepTopRanks_left_label
            // 
            this.SigData_keepTopRanks_left_label.AutoSize = true;
            this.SigData_keepTopRanks_left_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.SigData_keepTopRanks_left_label.Location = new System.Drawing.Point(0, 338);
            this.SigData_keepTopRanks_left_label.Name = "SigData_keepTopRanks_left_label";
            this.SigData_keepTopRanks_left_label.Size = new System.Drawing.Size(82, 18);
            this.SigData_keepTopRanks_left_label.TabIndex = 234;
            this.SigData_keepTopRanks_left_label.Text = "Define top";
            // 
            // SigData_1st_sigCutoff_headline_label
            // 
            this.SigData_1st_sigCutoff_headline_label.AutoSize = true;
            this.SigData_1st_sigCutoff_headline_label.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SigData_1st_sigCutoff_headline_label.Location = new System.Drawing.Point(3, 138);
            this.SigData_1st_sigCutoff_headline_label.Name = "SigData_1st_sigCutoff_headline_label";
            this.SigData_1st_sigCutoff_headline_label.Size = new System.Drawing.Size(192, 19);
            this.SigData_1st_sigCutoff_headline_label.TabIndex = 226;
            this.SigData_1st_sigCutoff_headline_label.Text = "First significance cutoff";
            // 
            // SigData_value2nd_cutoff_ownTextBox
            // 
            this.SigData_value2nd_cutoff_ownTextBox.Location = new System.Drawing.Point(182, 184);
            this.SigData_value2nd_cutoff_ownTextBox.Name = "SigData_value2nd_cutoff_ownTextBox";
            this.SigData_value2nd_cutoff_ownTextBox.Size = new System.Drawing.Size(58, 22);
            this.SigData_value2nd_cutoff_ownTextBox.TabIndex = 224;
            this.SigData_value2nd_cutoff_ownTextBox.TextChanged += new System.EventHandler(this.SigData_value2nd_cutoff_textBox_TextChanged);
            // 
            // SigData_value2nd_cutoff_label
            // 
            this.SigData_value2nd_cutoff_label.AutoSize = true;
            this.SigData_value2nd_cutoff_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SigData_value2nd_cutoff_label.Location = new System.Drawing.Point(2, 186);
            this.SigData_value2nd_cutoff_label.Name = "SigData_value2nd_cutoff_label";
            this.SigData_value2nd_cutoff_label.Size = new System.Drawing.Size(209, 18);
            this.SigData_value2nd_cutoff_label.TabIndex = 225;
            this.SigData_value2nd_cutoff_label.Text = "Minimum absolute 2nd value";
            // 
            // SigData_value1st_cutoff_ownTextBox
            // 
            this.SigData_value1st_cutoff_ownTextBox.Location = new System.Drawing.Point(182, 159);
            this.SigData_value1st_cutoff_ownTextBox.Name = "SigData_value1st_cutoff_ownTextBox";
            this.SigData_value1st_cutoff_ownTextBox.Size = new System.Drawing.Size(58, 22);
            this.SigData_value1st_cutoff_ownTextBox.TabIndex = 57;
            this.SigData_value1st_cutoff_ownTextBox.TextChanged += new System.EventHandler(this.SigData_value1st_cutoff_textBox_TextChanged);
            // 
            // SigData_value1st_cutoff_label
            // 
            this.SigData_value1st_cutoff_label.AutoSize = true;
            this.SigData_value1st_cutoff_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SigData_value1st_cutoff_label.Location = new System.Drawing.Point(2, 161);
            this.SigData_value1st_cutoff_label.Name = "SigData_value1st_cutoff_label";
            this.SigData_value1st_cutoff_label.Size = new System.Drawing.Size(167, 18);
            this.SigData_value1st_cutoff_label.TabIndex = 57;
            this.SigData_value1st_cutoff_label.Text = "Min absolute 1st value";
            // 
            // SigData_value2nd_headline_label
            // 
            this.SigData_value2nd_headline_label.AutoSize = true;
            this.SigData_value2nd_headline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value2nd_headline_label.Location = new System.Drawing.Point(0, 70);
            this.SigData_value2nd_headline_label.Name = "SigData_value2nd_headline_label";
            this.SigData_value2nd_headline_label.Size = new System.Drawing.Size(221, 19);
            this.SigData_value2nd_headline_label.TabIndex = 221;
            this.SigData_value2nd_headline_label.Text = "Significance increases with";
            // 
            // SigData_value1st_headline_label
            // 
            this.SigData_value1st_headline_label.AutoSize = true;
            this.SigData_value1st_headline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.SigData_value1st_headline_label.Location = new System.Drawing.Point(0, 6);
            this.SigData_value1st_headline_label.Name = "SigData_value1st_headline_label";
            this.SigData_value1st_headline_label.Size = new System.Drawing.Size(221, 19);
            this.SigData_value1st_headline_label.TabIndex = 220;
            this.SigData_value1st_headline_label.Text = "Significance increases with";
            // 
            // Options_results_panel
            // 
            this.Options_results_panel.Border_color = System.Drawing.Color.Black;
            this.Options_results_panel.Controls.Add(this.Results_controlCommand_panel);
            this.Options_results_panel.Controls.Add(this.Results_addResultsToControl_cbLabel);
            this.Options_results_panel.Controls.Add(this.Results_addResultsToControl_cbButton);
            this.Options_results_panel.Controls.Add(this.Results_directory_expl_label);
            this.Options_results_panel.Controls.Add(this.Results_directory_headline_label);
            this.Options_results_panel.Controls.Add(this.Results_directory_label);
            this.Options_results_panel.Controls.Add(this.Results_overall_headline_label);
            this.Options_results_panel.Corner_radius = 10F;
            this.Options_results_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_results_panel.Location = new System.Drawing.Point(232, 48);
            this.Options_results_panel.Name = "Options_results_panel";
            this.Options_results_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_results_panel.TabIndex = 263;
            // 
            // Results_controlCommand_panel
            // 
            this.Results_controlCommand_panel.Border_color = System.Drawing.Color.Transparent;
            this.Results_controlCommand_panel.Controls.Add(this.Results_integrationGroup_label);
            this.Results_controlCommand_panel.Controls.Add(this.Results_integrationGroup_listBox);
            this.Results_controlCommand_panel.Controls.Add(this.Results_bardiagram_show_label);
            this.Results_controlCommand_panel.Controls.Add(this.Results_heatmap_dynamic_cbLabel);
            this.Results_controlCommand_panel.Controls.Add(this.Results_bardiagram_standard_cbButton);
            this.Results_controlCommand_panel.Controls.Add(this.Results_heatmap_dynamic_cbButton);
            this.Results_controlCommand_panel.Controls.Add(this.Results_bardiagram_standard_cbLabel);
            this.Results_controlCommand_panel.Controls.Add(this.Results_heatmap_show_label);
            this.Results_controlCommand_panel.Controls.Add(this.Results_bardiagram_dynamic_cbLabel);
            this.Results_controlCommand_panel.Controls.Add(this.Results_timeline_show_label);
            this.Results_controlCommand_panel.Controls.Add(this.Results_bardiagram_dynamic_cbButton);
            this.Results_controlCommand_panel.Controls.Add(this.Results_heatmap_standard_cbLabel);
            this.Results_controlCommand_panel.Controls.Add(this.Results_timeline_cbButton);
            this.Results_controlCommand_panel.Controls.Add(this.Results_heatmap_standard_cbButton);
            this.Results_controlCommand_panel.Controls.Add(this.Results_timeline_cbLabel);
            this.Results_controlCommand_panel.Corner_radius = 10F;
            this.Results_controlCommand_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Results_controlCommand_panel.Location = new System.Drawing.Point(21, 44);
            this.Results_controlCommand_panel.Name = "Results_controlCommand_panel";
            this.Results_controlCommand_panel.Size = new System.Drawing.Size(334, 359);
            this.Results_controlCommand_panel.TabIndex = 274;
            // 
            // Results_integrationGroup_label
            // 
            this.Results_integrationGroup_label.AutoSize = true;
            this.Results_integrationGroup_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_integrationGroup_label.Location = new System.Drawing.Point(11, 10);
            this.Results_integrationGroup_label.Name = "Results_integrationGroup_label";
            this.Results_integrationGroup_label.Size = new System.Drawing.Size(174, 24);
            this.Results_integrationGroup_label.TabIndex = 271;
            this.Results_integrationGroup_label.Text = "Integration group";
            // 
            // Results_integrationGroup_listBox
            // 
            this.Results_integrationGroup_listBox.FormattingEnabled = true;
            this.Results_integrationGroup_listBox.ItemHeight = 16;
            this.Results_integrationGroup_listBox.Location = new System.Drawing.Point(14, 34);
            this.Results_integrationGroup_listBox.Name = "Results_integrationGroup_listBox";
            this.Results_integrationGroup_listBox.ReadOnly = false;
            this.Results_integrationGroup_listBox.Size = new System.Drawing.Size(120, 4);
            this.Results_integrationGroup_listBox.TabIndex = 270;
            this.Results_integrationGroup_listBox.SelectedIndexChanged += new System.EventHandler(this.Results_integrationGroup_listBox_SelectedIndexChanged);
            // 
            // Results_bardiagram_show_label
            // 
            this.Results_bardiagram_show_label.AutoSize = true;
            this.Results_bardiagram_show_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_bardiagram_show_label.Location = new System.Drawing.Point(11, 80);
            this.Results_bardiagram_show_label.Name = "Results_bardiagram_show_label";
            this.Results_bardiagram_show_label.Size = new System.Drawing.Size(130, 24);
            this.Results_bardiagram_show_label.TabIndex = 263;
            this.Results_bardiagram_show_label.Text = "Bardiagrams";
            // 
            // Results_heatmap_dynamic_cbLabel
            // 
            this.Results_heatmap_dynamic_cbLabel.AutoSize = true;
            this.Results_heatmap_dynamic_cbLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_heatmap_dynamic_cbLabel.Location = new System.Drawing.Point(101, 314);
            this.Results_heatmap_dynamic_cbLabel.Name = "Results_heatmap_dynamic_cbLabel";
            this.Results_heatmap_dynamic_cbLabel.Size = new System.Drawing.Size(203, 24);
            this.Results_heatmap_dynamic_cbLabel.TabIndex = 269;
            this.Results_heatmap_dynamic_cbLabel.Text = "Dynamic enrichment";
            // 
            // Results_bardiagram_standard_cbButton
            // 
            this.Results_bardiagram_standard_cbButton.Checked = false;
            this.Results_bardiagram_standard_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_standard_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_standard_cbButton.Location = new System.Drawing.Point(15, 112);
            this.Results_bardiagram_standard_cbButton.Name = "Results_bardiagram_standard_cbButton";
            this.Results_bardiagram_standard_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_standard_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_standard_cbButton.Size = new System.Drawing.Size(75, 23);
            this.Results_bardiagram_standard_cbButton.TabIndex = 254;
            this.Results_bardiagram_standard_cbButton.Text = "myCheckBox_button2";
            this.Results_bardiagram_standard_cbButton.UseVisualStyleBackColor = false;
            this.Results_bardiagram_standard_cbButton.Click += new System.EventHandler(this.Results_bardiagram_standard_cbButton_Click);
            // 
            // Results_heatmap_dynamic_cbButton
            // 
            this.Results_heatmap_dynamic_cbButton.Checked = false;
            this.Results_heatmap_dynamic_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Results_heatmap_dynamic_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Results_heatmap_dynamic_cbButton.Location = new System.Drawing.Point(20, 315);
            this.Results_heatmap_dynamic_cbButton.Name = "Results_heatmap_dynamic_cbButton";
            this.Results_heatmap_dynamic_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Results_heatmap_dynamic_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Results_heatmap_dynamic_cbButton.Size = new System.Drawing.Size(75, 23);
            this.Results_heatmap_dynamic_cbButton.TabIndex = 268;
            this.Results_heatmap_dynamic_cbButton.Text = "myCheckBox_button4";
            this.Results_heatmap_dynamic_cbButton.UseVisualStyleBackColor = false;
            this.Results_heatmap_dynamic_cbButton.Click += new System.EventHandler(this.Results_heatmap_dynamic_cbButton_Click);
            // 
            // Results_bardiagram_standard_cbLabel
            // 
            this.Results_bardiagram_standard_cbLabel.AutoSize = true;
            this.Results_bardiagram_standard_cbLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_bardiagram_standard_cbLabel.Location = new System.Drawing.Point(96, 109);
            this.Results_bardiagram_standard_cbLabel.Name = "Results_bardiagram_standard_cbLabel";
            this.Results_bardiagram_standard_cbLabel.Size = new System.Drawing.Size(208, 24);
            this.Results_bardiagram_standard_cbLabel.TabIndex = 257;
            this.Results_bardiagram_standard_cbLabel.Text = "Standard enrichment";
            // 
            // Results_heatmap_show_label
            // 
            this.Results_heatmap_show_label.AutoSize = true;
            this.Results_heatmap_show_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_heatmap_show_label.Location = new System.Drawing.Point(16, 261);
            this.Results_heatmap_show_label.Name = "Results_heatmap_show_label";
            this.Results_heatmap_show_label.Size = new System.Drawing.Size(104, 24);
            this.Results_heatmap_show_label.TabIndex = 265;
            this.Results_heatmap_show_label.Text = "Heatmaps";
            // 
            // Results_bardiagram_dynamic_cbLabel
            // 
            this.Results_bardiagram_dynamic_cbLabel.AutoSize = true;
            this.Results_bardiagram_dynamic_cbLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_bardiagram_dynamic_cbLabel.Location = new System.Drawing.Point(97, 140);
            this.Results_bardiagram_dynamic_cbLabel.Name = "Results_bardiagram_dynamic_cbLabel";
            this.Results_bardiagram_dynamic_cbLabel.Size = new System.Drawing.Size(203, 24);
            this.Results_bardiagram_dynamic_cbLabel.TabIndex = 267;
            this.Results_bardiagram_dynamic_cbLabel.Text = "Dynamic enrichment";
            // 
            // Results_timeline_show_label
            // 
            this.Results_timeline_show_label.AutoSize = true;
            this.Results_timeline_show_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_timeline_show_label.Location = new System.Drawing.Point(20, 186);
            this.Results_timeline_show_label.Name = "Results_timeline_show_label";
            this.Results_timeline_show_label.Size = new System.Drawing.Size(100, 24);
            this.Results_timeline_show_label.TabIndex = 264;
            this.Results_timeline_show_label.Text = "Timelines";
            // 
            // Results_bardiagram_dynamic_cbButton
            // 
            this.Results_bardiagram_dynamic_cbButton.Checked = false;
            this.Results_bardiagram_dynamic_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_dynamic_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_dynamic_cbButton.Location = new System.Drawing.Point(15, 137);
            this.Results_bardiagram_dynamic_cbButton.Name = "Results_bardiagram_dynamic_cbButton";
            this.Results_bardiagram_dynamic_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_dynamic_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Results_bardiagram_dynamic_cbButton.Size = new System.Drawing.Size(75, 23);
            this.Results_bardiagram_dynamic_cbButton.TabIndex = 266;
            this.Results_bardiagram_dynamic_cbButton.Text = "myCheckBox_button2";
            this.Results_bardiagram_dynamic_cbButton.UseVisualStyleBackColor = false;
            this.Results_bardiagram_dynamic_cbButton.Click += new System.EventHandler(this.Results_bardiagram_dynamic_cbButton_Click);
            // 
            // Results_heatmap_standard_cbLabel
            // 
            this.Results_heatmap_standard_cbLabel.AutoSize = true;
            this.Results_heatmap_standard_cbLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_heatmap_standard_cbLabel.Location = new System.Drawing.Point(98, 287);
            this.Results_heatmap_standard_cbLabel.Name = "Results_heatmap_standard_cbLabel";
            this.Results_heatmap_standard_cbLabel.Size = new System.Drawing.Size(208, 24);
            this.Results_heatmap_standard_cbLabel.TabIndex = 259;
            this.Results_heatmap_standard_cbLabel.Text = "Standard enrichment";
            // 
            // Results_timeline_cbButton
            // 
            this.Results_timeline_cbButton.Checked = false;
            this.Results_timeline_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Results_timeline_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Results_timeline_cbButton.Location = new System.Drawing.Point(21, 212);
            this.Results_timeline_cbButton.Name = "Results_timeline_cbButton";
            this.Results_timeline_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Results_timeline_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Results_timeline_cbButton.Size = new System.Drawing.Size(75, 23);
            this.Results_timeline_cbButton.TabIndex = 255;
            this.Results_timeline_cbButton.Text = "myCheckBox_button3";
            this.Results_timeline_cbButton.UseVisualStyleBackColor = false;
            this.Results_timeline_cbButton.Click += new System.EventHandler(this.Results_timeline_cbButton_Click);
            // 
            // Results_heatmap_standard_cbButton
            // 
            this.Results_heatmap_standard_cbButton.Checked = false;
            this.Results_heatmap_standard_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Results_heatmap_standard_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Results_heatmap_standard_cbButton.Location = new System.Drawing.Point(17, 288);
            this.Results_heatmap_standard_cbButton.Name = "Results_heatmap_standard_cbButton";
            this.Results_heatmap_standard_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Results_heatmap_standard_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Results_heatmap_standard_cbButton.Size = new System.Drawing.Size(75, 23);
            this.Results_heatmap_standard_cbButton.TabIndex = 256;
            this.Results_heatmap_standard_cbButton.Text = "myCheckBox_button4";
            this.Results_heatmap_standard_cbButton.UseVisualStyleBackColor = false;
            this.Results_heatmap_standard_cbButton.Click += new System.EventHandler(this.Results_heatmap_standard_cbButton_Click);
            // 
            // Results_timeline_cbLabel
            // 
            this.Results_timeline_cbLabel.AutoSize = true;
            this.Results_timeline_cbLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_timeline_cbLabel.Location = new System.Drawing.Point(102, 221);
            this.Results_timeline_cbLabel.Name = "Results_timeline_cbLabel";
            this.Results_timeline_cbLabel.Size = new System.Drawing.Size(208, 24);
            this.Results_timeline_cbLabel.TabIndex = 258;
            this.Results_timeline_cbLabel.Text = "Standard enrichment";
            // 
            // Results_addResultsToControl_cbLabel
            // 
            this.Results_addResultsToControl_cbLabel.AutoSize = true;
            this.Results_addResultsToControl_cbLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_addResultsToControl_cbLabel.Location = new System.Drawing.Point(98, 493);
            this.Results_addResultsToControl_cbLabel.Name = "Results_addResultsToControl_cbLabel";
            this.Results_addResultsToControl_cbLabel.Size = new System.Drawing.Size(208, 24);
            this.Results_addResultsToControl_cbLabel.TabIndex = 273;
            this.Results_addResultsToControl_cbLabel.Text = "Standard enrichment";
            // 
            // Results_addResultsToControl_cbButton
            // 
            this.Results_addResultsToControl_cbButton.Checked = false;
            this.Results_addResultsToControl_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Results_addResultsToControl_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Results_addResultsToControl_cbButton.Location = new System.Drawing.Point(14, 494);
            this.Results_addResultsToControl_cbButton.Name = "Results_addResultsToControl_cbButton";
            this.Results_addResultsToControl_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Results_addResultsToControl_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Results_addResultsToControl_cbButton.Size = new System.Drawing.Size(75, 23);
            this.Results_addResultsToControl_cbButton.TabIndex = 272;
            this.Results_addResultsToControl_cbButton.Text = "myCheckBox_button2";
            this.Results_addResultsToControl_cbButton.UseVisualStyleBackColor = false;
            this.Results_addResultsToControl_cbButton.Click += new System.EventHandler(this.Results_addResultsToControl_cbButton_Click);
            // 
            // Results_directory_expl_label
            // 
            this.Results_directory_expl_label.AutoSize = true;
            this.Results_directory_expl_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_directory_expl_label.Location = new System.Drawing.Point(84, 464);
            this.Results_directory_expl_label.Name = "Results_directory_expl_label";
            this.Results_directory_expl_label.Size = new System.Drawing.Size(644, 24);
            this.Results_directory_expl_label.TabIndex = 262;
            this.Results_directory_expl_label.Text = "(Use editors, e.g. yED graph editor, to open graphml network files.)";
            // 
            // Results_directory_headline_label
            // 
            this.Results_directory_headline_label.AutoSize = true;
            this.Results_directory_headline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_directory_headline_label.Location = new System.Drawing.Point(7, 403);
            this.Results_directory_headline_label.Name = "Results_directory_headline_label";
            this.Results_directory_headline_label.Size = new System.Drawing.Size(434, 24);
            this.Results_directory_headline_label.TabIndex = 261;
            this.Results_directory_headline_label.Text = "Figures and networks are saved in directory:";
            // 
            // Results_directory_label
            // 
            this.Results_directory_label.AutoSize = true;
            this.Results_directory_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_directory_label.Location = new System.Drawing.Point(58, 430);
            this.Results_directory_label.Name = "Results_directory_label";
            this.Results_directory_label.Size = new System.Drawing.Size(204, 24);
            this.Results_directory_label.TabIndex = 260;
            this.Results_directory_label.Text = "Results visualization";
            // 
            // Results_overall_headline_label
            // 
            this.Results_overall_headline_label.AutoSize = true;
            this.Results_overall_headline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results_overall_headline_label.Location = new System.Drawing.Point(21, 16);
            this.Results_overall_headline_label.Name = "Results_overall_headline_label";
            this.Results_overall_headline_label.Size = new System.Drawing.Size(204, 24);
            this.Results_overall_headline_label.TabIndex = 253;
            this.Results_overall_headline_label.Text = "Results visualization";
            // 
            // Results_visualization_panel
            // 
            this.Results_visualization_panel.Border_color = System.Drawing.Color.Transparent;
            this.Results_visualization_panel.Controls.Add(this.Results_visualization_integrationGroup_label);
            this.Results_visualization_panel.Controls.Add(this.Results_position_label);
            this.Results_visualization_panel.Controls.Add(this.Results_previous_button);
            this.Results_visualization_panel.Controls.Add(this.Results_next_button);
            this.Results_visualization_panel.Controls.Add(this.Results_zegGraph_control);
            this.Results_visualization_panel.Corner_radius = 10F;
            this.Results_visualization_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Results_visualization_panel.Location = new System.Drawing.Point(1132, 5);
            this.Results_visualization_panel.Name = "Results_visualization_panel";
            this.Results_visualization_panel.Size = new System.Drawing.Size(860, 480);
            this.Results_visualization_panel.TabIndex = 261;
            // 
            // Results_visualization_integrationGroup_label
            // 
            this.Results_visualization_integrationGroup_label.AutoSize = true;
            this.Results_visualization_integrationGroup_label.Location = new System.Drawing.Point(422, 8);
            this.Results_visualization_integrationGroup_label.Name = "Results_visualization_integrationGroup_label";
            this.Results_visualization_integrationGroup_label.Size = new System.Drawing.Size(103, 16);
            this.Results_visualization_integrationGroup_label.TabIndex = 81;
            this.Results_visualization_integrationGroup_label.Text = "Integration group";
            // 
            // Results_position_label
            // 
            this.Results_position_label.AutoSize = true;
            this.Results_position_label.Location = new System.Drawing.Point(261, 447);
            this.Results_position_label.Name = "Results_position_label";
            this.Results_position_label.Size = new System.Drawing.Size(41, 16);
            this.Results_position_label.TabIndex = 80;
            this.Results_position_label.Text = "label1";
            // 
            // Results_previous_button
            // 
            this.Results_previous_button.Location = new System.Drawing.Point(124, 440);
            this.Results_previous_button.Name = "Results_previous_button";
            this.Results_previous_button.Size = new System.Drawing.Size(75, 23);
            this.Results_previous_button.TabIndex = 79;
            this.Results_previous_button.Text = "Previous";
            this.Results_previous_button.UseVisualStyleBackColor = true;
            this.Results_previous_button.Click += new System.EventHandler(this.Results_previous_button_Click);
            // 
            // Results_next_button
            // 
            this.Results_next_button.Location = new System.Drawing.Point(39, 439);
            this.Results_next_button.Name = "Results_next_button";
            this.Results_next_button.Size = new System.Drawing.Size(75, 23);
            this.Results_next_button.TabIndex = 78;
            this.Results_next_button.Text = "Next";
            this.Results_next_button.UseVisualStyleBackColor = true;
            this.Results_next_button.Click += new System.EventHandler(this.Results_next_button_Click);
            // 
            // Results_zegGraph_control
            // 
            this.Results_zegGraph_control.AutoScroll = true;
            this.Results_zegGraph_control.Location = new System.Drawing.Point(23, 56);
            this.Results_zegGraph_control.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Results_zegGraph_control.Name = "Results_zegGraph_control";
            this.Results_zegGraph_control.ScrollGrace = 0D;
            this.Results_zegGraph_control.ScrollMaxX = 0D;
            this.Results_zegGraph_control.ScrollMaxY = 0D;
            this.Results_zegGraph_control.ScrollMaxY2 = 0D;
            this.Results_zegGraph_control.ScrollMinX = 0D;
            this.Results_zegGraph_control.ScrollMinY = 0D;
            this.Results_zegGraph_control.ScrollMinY2 = 0D;
            this.Results_zegGraph_control.Size = new System.Drawing.Size(605, 371);
            this.Results_zegGraph_control.TabIndex = 77;
            // 
            // Report_panel
            // 
            this.Report_panel.Border_color = System.Drawing.Color.Transparent;
            this.Report_panel.Controls.Add(this.Report_maxErrorPerFile2_label);
            this.Report_panel.Controls.Add(this.Report_headline_label);
            this.Report_panel.Controls.Add(this.Read_error_reports_maxErrorsPerFile_ownTextBox);
            this.Report_panel.Controls.Add(this.Report_ownTextBox);
            this.Report_panel.Controls.Add(this.Report_maxErrorPerFile1_label);
            this.Report_panel.Corner_radius = 10F;
            this.Report_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Report_panel.Location = new System.Drawing.Point(1135, 19);
            this.Report_panel.Name = "Report_panel";
            this.Report_panel.Size = new System.Drawing.Size(860, 480);
            this.Report_panel.TabIndex = 77;
            // 
            // Report_maxErrorPerFile2_label
            // 
            this.Report_maxErrorPerFile2_label.AutoSize = true;
            this.Report_maxErrorPerFile2_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Report_maxErrorPerFile2_label.Location = new System.Drawing.Point(138, 450);
            this.Report_maxErrorPerFile2_label.Name = "Report_maxErrorPerFile2_label";
            this.Report_maxErrorPerFile2_label.Size = new System.Drawing.Size(116, 19);
            this.Report_maxErrorPerFile2_label.TabIndex = 76;
            this.Report_maxErrorPerFile2_label.Text = "errors per file";
            // 
            // Report_headline_label
            // 
            this.Report_headline_label.AutoSize = true;
            this.Report_headline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Report_headline_label.Location = new System.Drawing.Point(3, 14);
            this.Report_headline_label.Name = "Report_headline_label";
            this.Report_headline_label.Size = new System.Drawing.Size(160, 24);
            this.Report_headline_label.TabIndex = 1;
            this.Report_headline_label.Text = "Error messages";
            // 
            // Read_error_reports_maxErrorsPerFile_ownTextBox
            // 
            this.Read_error_reports_maxErrorsPerFile_ownTextBox.Location = new System.Drawing.Point(115, 449);
            this.Read_error_reports_maxErrorsPerFile_ownTextBox.Name = "Read_error_reports_maxErrorsPerFile_ownTextBox";
            this.Read_error_reports_maxErrorsPerFile_ownTextBox.Size = new System.Drawing.Size(2, 22);
            this.Read_error_reports_maxErrorsPerFile_ownTextBox.TabIndex = 73;
            this.Read_error_reports_maxErrorsPerFile_ownTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Read_error_reports_maxErrorsPerFile_ownTextBox.TextChanged += new System.EventHandler(this.Read_error_reports_maxErrorsPerFile_ownTextBox_TextChanged);
            // 
            // Report_ownTextBox
            // 
            this.Report_ownTextBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Report_ownTextBox.Location = new System.Drawing.Point(8, 40);
            this.Report_ownTextBox.Multiline = true;
            this.Report_ownTextBox.Name = "Report_ownTextBox";
            this.Report_ownTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Report_ownTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Report_ownTextBox.Size = new System.Drawing.Size(836, 402);
            this.Report_ownTextBox.TabIndex = 0;
            // 
            // Report_maxErrorPerFile1_label
            // 
            this.Report_maxErrorPerFile1_label.AutoSize = true;
            this.Report_maxErrorPerFile1_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Report_maxErrorPerFile1_label.Location = new System.Drawing.Point(5, 450);
            this.Report_maxErrorPerFile1_label.Name = "Report_maxErrorPerFile1_label";
            this.Report_maxErrorPerFile1_label.Size = new System.Drawing.Size(110, 19);
            this.Report_maxErrorPerFile1_label.TabIndex = 72;
            this.Report_maxErrorPerFile1_label.Text = "Show at max";
            // 
            // Options_readData_panel
            // 
            this.Options_readData_panel.Border_color = System.Drawing.Color.Black;
            this.Options_readData_panel.Controls.Add(this.Read_order_allFilesInDirectory_label);
            this.Options_readData_panel.Controls.Add(this.Read_order_onlySpecifiedFile_label);
            this.Options_readData_panel.Controls.Add(this.Read_order_allFilesInDirectory_cbButton);
            this.Options_readData_panel.Controls.Add(this.Read_order_onlySpecifiedFile_cbButton);
            this.Options_readData_panel.Controls.Add(this.Read_setToOptimum_button);
            this.Options_readData_panel.Controls.Add(this.Read_setToMBCO_button);
            this.Options_readData_panel.Controls.Add(this.Read_colorColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_colorColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_headline_label);
            this.Options_readData_panel.Controls.Add(this.Read_value1st_explanation_label);
            this.Options_readData_panel.Controls.Add(this.Read_value2nd_explanation_label);
            this.Options_readData_panel.Controls.Add(this.Read_value2ndColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_value2ndColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_setToDefault_label);
            this.Options_readData_panel.Controls.Add(this.Read_timeunitColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_timeunitColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_error_reports_label);
            this.Options_readData_panel.Controls.Add(this.Read_readDataset_button);
            this.Options_readData_panel.Controls.Add(this.Read_error_reports_button);
            this.Options_readData_panel.Controls.Add(this.Read_sampleNameColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_delimiter_label);
            this.Options_readData_panel.Controls.Add(this.Read_timepointColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_delimiter_ownListBox);
            this.Options_readData_panel.Controls.Add(this.Read_value1stColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_timepointColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_setToMinimum_button);
            this.Options_readData_panel.Controls.Add(this.Read_sampleNameColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_integrationGroupColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_value1stColumn_label);
            this.Options_readData_panel.Controls.Add(this.Read_integrationGroupColumn_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_setToSeurat_button);
            this.Options_readData_panel.Controls.Add(this.Read_setToMBCO2_button);
            this.Options_readData_panel.Controls.Add(this.Read_geneSymbol_label);
            this.Options_readData_panel.Controls.Add(this.Read_informationGroup_label);
            this.Options_readData_panel.Controls.Add(this.Read_geneSymbol_ownTextBox);
            this.Options_readData_panel.Controls.Add(this.Read_setToMBCO1_button);
            this.Options_readData_panel.Controls.Add(this.Read_timeunit_ownCheckBox);
            this.Options_readData_panel.Controls.Add(this.Read_order_allFilesDirectory_label);
            this.Options_readData_panel.Corner_radius = 10F;
            this.Options_readData_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_readData_panel.Location = new System.Drawing.Point(998, 72);
            this.Options_readData_panel.Name = "Options_readData_panel";
            this.Options_readData_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_readData_panel.TabIndex = 175;
            // 
            // Read_order_allFilesInDirectory_label
            // 
            this.Read_order_allFilesInDirectory_label.AutoSize = true;
            this.Read_order_allFilesInDirectory_label.Location = new System.Drawing.Point(223, 440);
            this.Read_order_allFilesInDirectory_label.Name = "Read_order_allFilesInDirectory_label";
            this.Read_order_allFilesInDirectory_label.Size = new System.Drawing.Size(115, 16);
            this.Read_order_allFilesInDirectory_label.TabIndex = 212;
            this.Read_order_allFilesInDirectory_label.Text = "all files in directory";
            // 
            // Read_order_onlySpecifiedFile_label
            // 
            this.Read_order_onlySpecifiedFile_label.AutoSize = true;
            this.Read_order_onlySpecifiedFile_label.Location = new System.Drawing.Point(192, 408);
            this.Read_order_onlySpecifiedFile_label.Name = "Read_order_onlySpecifiedFile_label";
            this.Read_order_onlySpecifiedFile_label.Size = new System.Drawing.Size(106, 16);
            this.Read_order_onlySpecifiedFile_label.TabIndex = 211;
            this.Read_order_onlySpecifiedFile_label.Text = "only specified file";
            // 
            // Read_order_allFilesInDirectory_cbButton
            // 
            this.Read_order_allFilesInDirectory_cbButton.Checked = false;
            this.Read_order_allFilesInDirectory_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Read_order_allFilesInDirectory_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Read_order_allFilesInDirectory_cbButton.Location = new System.Drawing.Point(179, 432);
            this.Read_order_allFilesInDirectory_cbButton.Name = "Read_order_allFilesInDirectory_cbButton";
            this.Read_order_allFilesInDirectory_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Read_order_allFilesInDirectory_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Read_order_allFilesInDirectory_cbButton.Size = new System.Drawing.Size(19, 8);
            this.Read_order_allFilesInDirectory_cbButton.TabIndex = 210;
            this.Read_order_allFilesInDirectory_cbButton.Text = "X";
            this.Read_order_allFilesInDirectory_cbButton.UseVisualStyleBackColor = true;
            this.Read_order_allFilesInDirectory_cbButton.Click += new System.EventHandler(this.Read_order_allFilesInDirectory_cbButton_Click);
            // 
            // Read_order_onlySpecifiedFile_cbButton
            // 
            this.Read_order_onlySpecifiedFile_cbButton.Checked = false;
            this.Read_order_onlySpecifiedFile_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Read_order_onlySpecifiedFile_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Read_order_onlySpecifiedFile_cbButton.Location = new System.Drawing.Point(136, 408);
            this.Read_order_onlySpecifiedFile_cbButton.Name = "Read_order_onlySpecifiedFile_cbButton";
            this.Read_order_onlySpecifiedFile_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Read_order_onlySpecifiedFile_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Read_order_onlySpecifiedFile_cbButton.Size = new System.Drawing.Size(19, 8);
            this.Read_order_onlySpecifiedFile_cbButton.TabIndex = 209;
            this.Read_order_onlySpecifiedFile_cbButton.Text = "X";
            this.Read_order_onlySpecifiedFile_cbButton.UseVisualStyleBackColor = true;
            this.Read_order_onlySpecifiedFile_cbButton.Click += new System.EventHandler(this.Read_order_onlySpecifiedFile_cbButton_Click);
            // 
            // Read_setToOptimum_button
            // 
            this.Read_setToOptimum_button.BackColor = System.Drawing.Color.DimGray;
            this.Read_setToOptimum_button.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.Read_setToOptimum_button.ForeColor = System.Drawing.Color.White;
            this.Read_setToOptimum_button.Location = new System.Drawing.Point(283, 363);
            this.Read_setToOptimum_button.Name = "Read_setToOptimum_button";
            this.Read_setToOptimum_button.Size = new System.Drawing.Size(75, 25);
            this.Read_setToOptimum_button.TabIndex = 208;
            this.Read_setToOptimum_button.Text = "Optimum";
            this.Read_setToOptimum_button.UseVisualStyleBackColor = false;
            this.Read_setToOptimum_button.Click += new System.EventHandler(this.Read_setToOptimum_button_Click);
            // 
            // Read_setToMBCO_button
            // 
            this.Read_setToMBCO_button.BackColor = System.Drawing.Color.DimGray;
            this.Read_setToMBCO_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_setToMBCO_button.ForeColor = System.Drawing.Color.White;
            this.Read_setToMBCO_button.Location = new System.Drawing.Point(207, 362);
            this.Read_setToMBCO_button.Name = "Read_setToMBCO_button";
            this.Read_setToMBCO_button.Size = new System.Drawing.Size(75, 25);
            this.Read_setToMBCO_button.TabIndex = 207;
            this.Read_setToMBCO_button.Text = "MBCO";
            this.Read_setToMBCO_button.UseVisualStyleBackColor = false;
            this.Read_setToMBCO_button.Click += new System.EventHandler(this.Read_setToMBCO_button_Click);
            // 
            // Read_colorColumn_ownTextBox
            // 
            this.Read_colorColumn_ownTextBox.Location = new System.Drawing.Point(101, 147);
            this.Read_colorColumn_ownTextBox.Name = "Read_colorColumn_ownTextBox";
            this.Read_colorColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_colorColumn_ownTextBox.TabIndex = 205;
            // 
            // Read_colorColumn_label
            // 
            this.Read_colorColumn_label.AutoSize = true;
            this.Read_colorColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_colorColumn_label.Location = new System.Drawing.Point(54, 150);
            this.Read_colorColumn_label.Name = "Read_colorColumn_label";
            this.Read_colorColumn_label.Size = new System.Drawing.Size(47, 18);
            this.Read_colorColumn_label.TabIndex = 206;
            this.Read_colorColumn_label.Text = "Color";
            this.Read_colorColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_headline_label
            // 
            this.Read_headline_label.AutoSize = true;
            this.Read_headline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.errorProvider1.SetIconAlignment(this.Read_headline_label, System.Windows.Forms.ErrorIconAlignment.BottomRight);
            this.Read_headline_label.Location = new System.Drawing.Point(16, 4);
            this.Read_headline_label.Name = "Read_headline_label";
            this.Read_headline_label.Size = new System.Drawing.Size(330, 19);
            this.Read_headline_label.TabIndex = 204;
            this.Read_headline_label.Text = "Enter column names of fields in data files";
            this.Read_headline_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Read_value1st_explanation_label
            // 
            this.Read_value1st_explanation_label.AutoSize = true;
            this.Read_value1st_explanation_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
            this.Read_value1st_explanation_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Read_value1st_explanation_label.Location = new System.Drawing.Point(284, 209);
            this.Read_value1st_explanation_label.Name = "Read_value1st_explanation_label";
            this.Read_value1st_explanation_label.Size = new System.Drawing.Size(77, 17);
            this.Read_value1st_explanation_label.TabIndex = 203;
            this.Read_value1st_explanation_label.Text = "eg log2(fc)";
            // 
            // Read_value2nd_explanation_label
            // 
            this.Read_value2nd_explanation_label.AutoSize = true;
            this.Read_value2nd_explanation_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
            this.Read_value2nd_explanation_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Read_value2nd_explanation_label.Location = new System.Drawing.Point(284, 239);
            this.Read_value2nd_explanation_label.Name = "Read_value2nd_explanation_label";
            this.Read_value2nd_explanation_label.Size = new System.Drawing.Size(76, 17);
            this.Read_value2nd_explanation_label.TabIndex = 202;
            this.Read_value2nd_explanation_label.Text = "eg p-value";
            // 
            // Read_value2ndColumn_ownTextBox
            // 
            this.Read_value2ndColumn_ownTextBox.Location = new System.Drawing.Point(101, 237);
            this.Read_value2ndColumn_ownTextBox.Name = "Read_value2ndColumn_ownTextBox";
            this.Read_value2ndColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_value2ndColumn_ownTextBox.TabIndex = 200;
            this.Read_value2ndColumn_ownTextBox.TextChanged += new System.EventHandler(this.Read_value2ndColumn_ownTextBox_TextChanged);
            // 
            // Read_value2ndColumn_label
            // 
            this.Read_value2ndColumn_label.AutoSize = true;
            this.Read_value2ndColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_value2ndColumn_label.Location = new System.Drawing.Point(25, 239);
            this.Read_value2ndColumn_label.Name = "Read_value2ndColumn_label";
            this.Read_value2ndColumn_label.Size = new System.Drawing.Size(76, 18);
            this.Read_value2ndColumn_label.TabIndex = 201;
            this.Read_value2ndColumn_label.Text = "2nd Value";
            this.Read_value2ndColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_setToDefault_label
            // 
            this.Read_setToDefault_label.AutoSize = true;
            this.Read_setToDefault_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_setToDefault_label.Location = new System.Drawing.Point(7, 345);
            this.Read_setToDefault_label.Name = "Read_setToDefault_label";
            this.Read_setToDefault_label.Size = new System.Drawing.Size(122, 38);
            this.Read_setToDefault_label.TabIndex = 199;
            this.Read_setToDefault_label.Text = "Default\r\ncolumn names";
            // 
            // Read_timeunitColumn_label
            // 
            this.Read_timeunitColumn_label.AutoSize = true;
            this.Read_timeunitColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_timeunitColumn_label.Location = new System.Drawing.Point(27, 89);
            this.Read_timeunitColumn_label.Name = "Read_timeunitColumn_label";
            this.Read_timeunitColumn_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Read_timeunitColumn_label.Size = new System.Drawing.Size(74, 18);
            this.Read_timeunitColumn_label.TabIndex = 198;
            this.Read_timeunitColumn_label.Text = "Time unit";
            this.Read_timeunitColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_timeunitColumn_ownTextBox
            // 
            this.Read_timeunitColumn_ownTextBox.Location = new System.Drawing.Point(101, 87);
            this.Read_timeunitColumn_ownTextBox.Name = "Read_timeunitColumn_ownTextBox";
            this.Read_timeunitColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_timeunitColumn_ownTextBox.TabIndex = 197;
            this.Read_timeunitColumn_ownTextBox.TextChanged += new System.EventHandler(this.Read_timeunitColumn_ownTextBox_TextChanged);
            // 
            // Read_error_reports_label
            // 
            this.Read_error_reports_label.AutoSize = true;
            this.Read_error_reports_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_error_reports_label.Location = new System.Drawing.Point(11, 433);
            this.Read_error_reports_label.Name = "Read_error_reports_label";
            this.Read_error_reports_label.Size = new System.Drawing.Size(114, 19);
            this.Read_error_reports_label.TabIndex = 196;
            this.Read_error_reports_label.Text = "Add file name";
            // 
            // Read_readDataset_button
            // 
            this.Read_readDataset_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Read_readDataset_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Read_readDataset_button.ForeColor = System.Drawing.Color.White;
            this.Read_readDataset_button.Location = new System.Drawing.Point(209, 492);
            this.Read_readDataset_button.Name = "Read_readDataset_button";
            this.Read_readDataset_button.Size = new System.Drawing.Size(140, 31);
            this.Read_readDataset_button.TabIndex = 76;
            this.Read_readDataset_button.Text = "Read";
            this.Read_readDataset_button.UseVisualStyleBackColor = false;
            this.Read_readDataset_button.Click += new System.EventHandler(this.ReadDataset_button_Click);
            // 
            // Read_error_reports_button
            // 
            this.Read_error_reports_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Read_error_reports_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_error_reports_button.ForeColor = System.Drawing.Color.White;
            this.Read_error_reports_button.Location = new System.Drawing.Point(11, 492);
            this.Read_error_reports_button.Name = "Read_error_reports_button";
            this.Read_error_reports_button.Size = new System.Drawing.Size(140, 31);
            this.Read_error_reports_button.TabIndex = 195;
            this.Read_error_reports_button.Text = "Show errors";
            this.Read_error_reports_button.UseVisualStyleBackColor = false;
            this.Read_error_reports_button.Click += new System.EventHandler(this.Read_error_reports_button_Click);
            // 
            // Read_sampleNameColumn_ownTextBox
            // 
            this.Read_sampleNameColumn_ownTextBox.Location = new System.Drawing.Point(101, 27);
            this.Read_sampleNameColumn_ownTextBox.Name = "Read_sampleNameColumn_ownTextBox";
            this.Read_sampleNameColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_sampleNameColumn_ownTextBox.TabIndex = 98;
            this.Read_sampleNameColumn_ownTextBox.TextChanged += new System.EventHandler(this.Read_sampleNameColumn_ownTextBox_TextChanged);
            // 
            // Read_delimiter_label
            // 
            this.Read_delimiter_label.AutoSize = true;
            this.Read_delimiter_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_delimiter_label.Location = new System.Drawing.Point(46, 316);
            this.Read_delimiter_label.Name = "Read_delimiter_label";
            this.Read_delimiter_label.Size = new System.Drawing.Size(75, 19);
            this.Read_delimiter_label.TabIndex = 194;
            this.Read_delimiter_label.Text = "File type";
            this.Read_delimiter_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_timepointColumn_ownTextBox
            // 
            this.Read_timepointColumn_ownTextBox.Location = new System.Drawing.Point(101, 57);
            this.Read_timepointColumn_ownTextBox.Name = "Read_timepointColumn_ownTextBox";
            this.Read_timepointColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_timepointColumn_ownTextBox.TabIndex = 99;
            this.Read_timepointColumn_ownTextBox.TextChanged += new System.EventHandler(this.Read_timepointColumn_ownTextBox_TextChanged);
            // 
            // Read_delimiter_ownListBox
            // 
            this.Read_delimiter_ownListBox.FormattingEnabled = true;
            this.Read_delimiter_ownListBox.ItemHeight = 16;
            this.Read_delimiter_ownListBox.Location = new System.Drawing.Point(135, 316);
            this.Read_delimiter_ownListBox.Name = "Read_delimiter_ownListBox";
            this.Read_delimiter_ownListBox.ReadOnly = false;
            this.Read_delimiter_ownListBox.Size = new System.Drawing.Size(196, 4);
            this.Read_delimiter_ownListBox.TabIndex = 193;
            // 
            // Read_value1stColumn_ownTextBox
            // 
            this.Read_value1stColumn_ownTextBox.Location = new System.Drawing.Point(101, 207);
            this.Read_value1stColumn_ownTextBox.Name = "Read_value1stColumn_ownTextBox";
            this.Read_value1stColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_value1stColumn_ownTextBox.TabIndex = 101;
            this.Read_value1stColumn_ownTextBox.TextChanged += new System.EventHandler(this.Read_valueColumn_ownTextBox_TextChanged);
            // 
            // Read_timepointColumn_label
            // 
            this.Read_timepointColumn_label.AutoSize = true;
            this.Read_timepointColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_timepointColumn_label.Location = new System.Drawing.Point(22, 59);
            this.Read_timepointColumn_label.Name = "Read_timepointColumn_label";
            this.Read_timepointColumn_label.Size = new System.Drawing.Size(79, 18);
            this.Read_timepointColumn_label.TabIndex = 135;
            this.Read_timepointColumn_label.Text = "Timepoint";
            this.Read_timepointColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_setToMinimum_button
            // 
            this.Read_setToMinimum_button.BackColor = System.Drawing.Color.DimGray;
            this.Read_setToMinimum_button.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.Read_setToMinimum_button.ForeColor = System.Drawing.Color.White;
            this.Read_setToMinimum_button.Location = new System.Drawing.Point(283, 338);
            this.Read_setToMinimum_button.Name = "Read_setToMinimum_button";
            this.Read_setToMinimum_button.Size = new System.Drawing.Size(75, 25);
            this.Read_setToMinimum_button.TabIndex = 191;
            this.Read_setToMinimum_button.Text = "Minimum";
            this.Read_setToMinimum_button.UseVisualStyleBackColor = false;
            this.Read_setToMinimum_button.Click += new System.EventHandler(this.Read_setToMinimum_button_Click);
            // 
            // Read_sampleNameColumn_label
            // 
            this.Read_sampleNameColumn_label.AutoSize = true;
            this.Read_sampleNameColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_sampleNameColumn_label.Location = new System.Drawing.Point(53, 29);
            this.Read_sampleNameColumn_label.Name = "Read_sampleNameColumn_label";
            this.Read_sampleNameColumn_label.Size = new System.Drawing.Size(48, 18);
            this.Read_sampleNameColumn_label.TabIndex = 161;
            this.Read_sampleNameColumn_label.Text = "Name";
            this.Read_sampleNameColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_integrationGroupColumn_label
            // 
            this.Read_integrationGroupColumn_label.AutoSize = true;
            this.Read_integrationGroupColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_integrationGroupColumn_label.Location = new System.Drawing.Point(16, 111);
            this.Read_integrationGroupColumn_label.Name = "Read_integrationGroupColumn_label";
            this.Read_integrationGroupColumn_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Read_integrationGroupColumn_label.Size = new System.Drawing.Size(85, 36);
            this.Read_integrationGroupColumn_label.TabIndex = 190;
            this.Read_integrationGroupColumn_label.Text = "Integration\r\ngroup";
            this.Read_integrationGroupColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_value1stColumn_label
            // 
            this.Read_value1stColumn_label.AutoSize = true;
            this.Read_value1stColumn_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_value1stColumn_label.Location = new System.Drawing.Point(30, 209);
            this.Read_value1stColumn_label.Name = "Read_value1stColumn_label";
            this.Read_value1stColumn_label.Size = new System.Drawing.Size(71, 18);
            this.Read_value1stColumn_label.TabIndex = 163;
            this.Read_value1stColumn_label.Text = "1st Value";
            this.Read_value1stColumn_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_integrationGroupColumn_ownTextBox
            // 
            this.Read_integrationGroupColumn_ownTextBox.Location = new System.Drawing.Point(101, 117);
            this.Read_integrationGroupColumn_ownTextBox.Name = "Read_integrationGroupColumn_ownTextBox";
            this.Read_integrationGroupColumn_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_integrationGroupColumn_ownTextBox.TabIndex = 189;
            this.Read_integrationGroupColumn_ownTextBox.TextChanged += new System.EventHandler(this.Read_integrationGroupColumn_ownTextBox_TextChanged);
            // 
            // Read_setToSeurat_button
            // 
            this.Read_setToSeurat_button.BackColor = System.Drawing.Color.DimGray;
            this.Read_setToSeurat_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_setToSeurat_button.ForeColor = System.Drawing.Color.White;
            this.Read_setToSeurat_button.Location = new System.Drawing.Point(207, 338);
            this.Read_setToSeurat_button.Name = "Read_setToSeurat_button";
            this.Read_setToSeurat_button.Size = new System.Drawing.Size(75, 25);
            this.Read_setToSeurat_button.TabIndex = 186;
            this.Read_setToSeurat_button.Text = "Seurat";
            this.Read_setToSeurat_button.UseVisualStyleBackColor = false;
            this.Read_setToSeurat_button.Click += new System.EventHandler(this.Read_setToSeurat_button_Click);
            // 
            // Read_setToMBCO2_button
            // 
            this.Read_setToMBCO2_button.BackColor = System.Drawing.Color.DimGray;
            this.Read_setToMBCO2_button.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.Read_setToMBCO2_button.ForeColor = System.Drawing.Color.White;
            this.Read_setToMBCO2_button.Location = new System.Drawing.Point(131, 362);
            this.Read_setToMBCO2_button.Name = "Read_setToMBCO2_button";
            this.Read_setToMBCO2_button.Size = new System.Drawing.Size(75, 25);
            this.Read_setToMBCO2_button.TabIndex = 185;
            this.Read_setToMBCO2_button.Text = "Example 2";
            this.Read_setToMBCO2_button.UseVisualStyleBackColor = false;
            this.Read_setToMBCO2_button.Click += new System.EventHandler(this.Read_setToMBCO2_button_Click);
            // 
            // Read_geneSymbol_label
            // 
            this.Read_geneSymbol_label.AutoSize = true;
            this.Read_geneSymbol_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Read_geneSymbol_label.Location = new System.Drawing.Point(1, 179);
            this.Read_geneSymbol_label.Name = "Read_geneSymbol_label";
            this.Read_geneSymbol_label.Size = new System.Drawing.Size(100, 18);
            this.Read_geneSymbol_label.TabIndex = 183;
            this.Read_geneSymbol_label.Text = "Gene symbol";
            this.Read_geneSymbol_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_informationGroup_label
            // 
            this.Read_informationGroup_label.AutoSize = true;
            this.Read_informationGroup_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
            this.Read_informationGroup_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Read_informationGroup_label.Location = new System.Drawing.Point(51, 263);
            this.Read_informationGroup_label.Name = "Read_informationGroup_label";
            this.Read_informationGroup_label.Size = new System.Drawing.Size(161, 17);
            this.Read_informationGroup_label.TabIndex = 172;
            this.Read_informationGroup_label.Text = "Read_information_label";
            // 
            // Read_geneSymbol_ownTextBox
            // 
            this.Read_geneSymbol_ownTextBox.Location = new System.Drawing.Point(101, 177);
            this.Read_geneSymbol_ownTextBox.Name = "Read_geneSymbol_ownTextBox";
            this.Read_geneSymbol_ownTextBox.Size = new System.Drawing.Size(183, 22);
            this.Read_geneSymbol_ownTextBox.TabIndex = 182;
            this.Read_geneSymbol_ownTextBox.TextChanged += new System.EventHandler(this.Read_geneSymbol_ownTextBox_TextChanged);
            // 
            // Read_setToMBCO1_button
            // 
            this.Read_setToMBCO1_button.BackColor = System.Drawing.Color.DimGray;
            this.Read_setToMBCO1_button.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.Read_setToMBCO1_button.ForeColor = System.Drawing.Color.White;
            this.Read_setToMBCO1_button.Location = new System.Drawing.Point(131, 338);
            this.Read_setToMBCO1_button.Name = "Read_setToMBCO1_button";
            this.Read_setToMBCO1_button.Size = new System.Drawing.Size(75, 25);
            this.Read_setToMBCO1_button.TabIndex = 177;
            this.Read_setToMBCO1_button.Text = "Example 1";
            this.Read_setToMBCO1_button.UseVisualStyleBackColor = false;
            this.Read_setToMBCO1_button.Click += new System.EventHandler(this.Read_setToMBCO1_button_Click);
            // 
            // Read_timeunit_ownCheckBox
            // 
            this.Read_timeunit_ownCheckBox.FormattingEnabled = true;
            this.Read_timeunit_ownCheckBox.ItemHeight = 16;
            this.Read_timeunit_ownCheckBox.Location = new System.Drawing.Point(287, 58);
            this.Read_timeunit_ownCheckBox.Name = "Read_timeunit_ownCheckBox";
            this.Read_timeunit_ownCheckBox.ReadOnly = false;
            this.Read_timeunit_ownCheckBox.Size = new System.Drawing.Size(68, 4);
            this.Read_timeunit_ownCheckBox.TabIndex = 181;
            // 
            // Read_order_allFilesDirectory_label
            // 
            this.Read_order_allFilesDirectory_label.AutoSize = true;
            this.Read_order_allFilesDirectory_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Read_order_allFilesDirectory_label.Location = new System.Drawing.Point(11, 398);
            this.Read_order_allFilesDirectory_label.Name = "Read_order_allFilesDirectory_label";
            this.Read_order_allFilesDirectory_label.Size = new System.Drawing.Size(49, 19);
            this.Read_order_allFilesDirectory_label.TabIndex = 180;
            this.Read_order_allFilesDirectory_label.Text = "Read";
            // 
            // Options_defineScps_panel
            // 
            this.Options_defineScps_panel.Border_color = System.Drawing.Color.Black;
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level4_cbLabel);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level3_cbLabel);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level2_cbLabel);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level1_cbLabel);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level4_cbButton);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level3_cbButton);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level2_cbButton);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level1_cbButton);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_selection_panel);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_level_label);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_removeOwnSCP_button);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_addNewOwnSCP_button);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_selectOwnScp_label);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_selectOwnScp_ownListBox);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_newOwnScpName_ownTextBox);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_newOwnScpName_label);
            this.Options_defineScps_panel.Controls.Add(this.DefineScps_overall_headline_label);
            this.Options_defineScps_panel.Corner_radius = 10F;
            this.Options_defineScps_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_defineScps_panel.Location = new System.Drawing.Point(1048, 19);
            this.Options_defineScps_panel.Name = "Options_defineScps_panel";
            this.Options_defineScps_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_defineScps_panel.TabIndex = 251;
            // 
            // DefineScps_level4_cbLabel
            // 
            this.DefineScps_level4_cbLabel.AutoSize = true;
            this.DefineScps_level4_cbLabel.Location = new System.Drawing.Point(305, 77);
            this.DefineScps_level4_cbLabel.Name = "DefineScps_level4_cbLabel";
            this.DefineScps_level4_cbLabel.Size = new System.Drawing.Size(14, 16);
            this.DefineScps_level4_cbLabel.TabIndex = 252;
            this.DefineScps_level4_cbLabel.Text = "4";
            // 
            // DefineScps_level3_cbLabel
            // 
            this.DefineScps_level3_cbLabel.AutoSize = true;
            this.DefineScps_level3_cbLabel.Location = new System.Drawing.Point(264, 82);
            this.DefineScps_level3_cbLabel.Name = "DefineScps_level3_cbLabel";
            this.DefineScps_level3_cbLabel.Size = new System.Drawing.Size(14, 16);
            this.DefineScps_level3_cbLabel.TabIndex = 251;
            this.DefineScps_level3_cbLabel.Text = "3";
            // 
            // DefineScps_level2_cbLabel
            // 
            this.DefineScps_level2_cbLabel.AutoSize = true;
            this.DefineScps_level2_cbLabel.Location = new System.Drawing.Point(225, 82);
            this.DefineScps_level2_cbLabel.Name = "DefineScps_level2_cbLabel";
            this.DefineScps_level2_cbLabel.Size = new System.Drawing.Size(14, 16);
            this.DefineScps_level2_cbLabel.TabIndex = 250;
            this.DefineScps_level2_cbLabel.Text = "2";
            // 
            // DefineScps_level1_cbLabel
            // 
            this.DefineScps_level1_cbLabel.AutoSize = true;
            this.DefineScps_level1_cbLabel.Location = new System.Drawing.Point(168, 82);
            this.DefineScps_level1_cbLabel.Name = "DefineScps_level1_cbLabel";
            this.DefineScps_level1_cbLabel.Size = new System.Drawing.Size(14, 16);
            this.DefineScps_level1_cbLabel.TabIndex = 249;
            this.DefineScps_level1_cbLabel.Text = "1";
            // 
            // DefineScps_level4_cbButton
            // 
            this.DefineScps_level4_cbButton.Checked = false;
            this.DefineScps_level4_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level4_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level4_cbButton.Location = new System.Drawing.Point(330, 85);
            this.DefineScps_level4_cbButton.Name = "DefineScps_level4_cbButton";
            this.DefineScps_level4_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level4_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level4_cbButton.Size = new System.Drawing.Size(22, 23);
            this.DefineScps_level4_cbButton.TabIndex = 248;
            this.DefineScps_level4_cbButton.Text = "myCheckBox_button5";
            this.DefineScps_level4_cbButton.UseVisualStyleBackColor = true;
            this.DefineScps_level4_cbButton.Click += new System.EventHandler(this.DefineScps_level4_cbButton_Click);
            // 
            // DefineScps_level3_cbButton
            // 
            this.DefineScps_level3_cbButton.Checked = false;
            this.DefineScps_level3_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level3_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level3_cbButton.Location = new System.Drawing.Point(292, 85);
            this.DefineScps_level3_cbButton.Name = "DefineScps_level3_cbButton";
            this.DefineScps_level3_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level3_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level3_cbButton.Size = new System.Drawing.Size(22, 23);
            this.DefineScps_level3_cbButton.TabIndex = 247;
            this.DefineScps_level3_cbButton.Text = "myCheckBox_button4";
            this.DefineScps_level3_cbButton.UseVisualStyleBackColor = true;
            this.DefineScps_level3_cbButton.Click += new System.EventHandler(this.DefineScps_level3_cbButton_Click);
            // 
            // DefineScps_level2_cbButton
            // 
            this.DefineScps_level2_cbButton.Checked = false;
            this.DefineScps_level2_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level2_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level2_cbButton.Location = new System.Drawing.Point(253, 85);
            this.DefineScps_level2_cbButton.Name = "DefineScps_level2_cbButton";
            this.DefineScps_level2_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level2_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level2_cbButton.Size = new System.Drawing.Size(22, 23);
            this.DefineScps_level2_cbButton.TabIndex = 246;
            this.DefineScps_level2_cbButton.Text = "myCheckBox_button3";
            this.DefineScps_level2_cbButton.UseVisualStyleBackColor = true;
            this.DefineScps_level2_cbButton.Click += new System.EventHandler(this.DefineScps_level2_cbButton_Click);
            // 
            // DefineScps_level1_cbButton
            // 
            this.DefineScps_level1_cbButton.Checked = false;
            this.DefineScps_level1_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level1_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level1_cbButton.Location = new System.Drawing.Point(212, 85);
            this.DefineScps_level1_cbButton.Name = "DefineScps_level1_cbButton";
            this.DefineScps_level1_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.DefineScps_level1_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.DefineScps_level1_cbButton.Size = new System.Drawing.Size(22, 23);
            this.DefineScps_level1_cbButton.TabIndex = 245;
            this.DefineScps_level1_cbButton.Text = "myCheckBox_button2";
            this.DefineScps_level1_cbButton.UseVisualStyleBackColor = true;
            this.DefineScps_level1_cbButton.Click += new System.EventHandler(this.DefineScps_level1_cbButton_Click);
            // 
            // DefineScps_selection_panel
            // 
            this.DefineScps_selection_panel.Border_color = System.Drawing.Color.Transparent;
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_sort_listBox);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_ownSubScps_label);
            this.DefineScps_selection_panel.Controls.Add(this.DefineSCPs_writeMbcoHierarchy_button);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_removeSubScp_button);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_mbcoSCP_ownListBox);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_sort_label);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_mbcoSCP_label);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_addSubScp_button);
            this.DefineScps_selection_panel.Controls.Add(this.DefineScps_ownSubScps_ownListBox);
            this.DefineScps_selection_panel.Corner_radius = 10F;
            this.DefineScps_selection_panel.Fill_color = System.Drawing.Color.Transparent;
            this.DefineScps_selection_panel.Location = new System.Drawing.Point(3, 118);
            this.DefineScps_selection_panel.Name = "DefineScps_selection_panel";
            this.DefineScps_selection_panel.Size = new System.Drawing.Size(354, 403);
            this.DefineScps_selection_panel.TabIndex = 244;
            // 
            // DefineScps_sort_listBox
            // 
            this.DefineScps_sort_listBox.FormattingEnabled = true;
            this.DefineScps_sort_listBox.ItemHeight = 16;
            this.DefineScps_sort_listBox.Location = new System.Drawing.Point(113, 207);
            this.DefineScps_sort_listBox.Name = "DefineScps_sort_listBox";
            this.DefineScps_sort_listBox.ReadOnly = false;
            this.DefineScps_sort_listBox.Size = new System.Drawing.Size(121, 4);
            this.DefineScps_sort_listBox.TabIndex = 243;
            this.DefineScps_sort_listBox.SelectedIndexChanged += new System.EventHandler(this.DefineScps_sort_listBox_SelectedIndexChanged);
            // 
            // DefineScps_ownSubScps_label
            // 
            this.DefineScps_ownSubScps_label.AutoSize = true;
            this.DefineScps_ownSubScps_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.DefineScps_ownSubScps_label.Location = new System.Drawing.Point(1, 232);
            this.DefineScps_ownSubScps_label.Name = "DefineScps_ownSubScps_label";
            this.DefineScps_ownSubScps_label.Size = new System.Drawing.Size(252, 36);
            this.DefineScps_ownSubScps_label.TabIndex = 236;
            this.DefineScps_ownSubScps_label.Text = "SCPs whose genes\r\nwill be added to selected own SCP";
            // 
            // DefineSCPs_writeMbcoHierarchy_button
            // 
            this.DefineSCPs_writeMbcoHierarchy_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DefineSCPs_writeMbcoHierarchy_button.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.DefineSCPs_writeMbcoHierarchy_button.ForeColor = System.Drawing.Color.White;
            this.DefineSCPs_writeMbcoHierarchy_button.Location = new System.Drawing.Point(250, 184);
            this.DefineSCPs_writeMbcoHierarchy_button.Name = "DefineSCPs_writeMbcoHierarchy_button";
            this.DefineSCPs_writeMbcoHierarchy_button.Size = new System.Drawing.Size(100, 46);
            this.DefineSCPs_writeMbcoHierarchy_button.TabIndex = 232;
            this.DefineSCPs_writeMbcoHierarchy_button.Text = "Write MBCO\r\nhierarchy";
            this.DefineSCPs_writeMbcoHierarchy_button.UseVisualStyleBackColor = false;
            this.DefineSCPs_writeMbcoHierarchy_button.Click += new System.EventHandler(this.DefineSCPs_writeMbcoHierarchy_button_Click);
            // 
            // DefineScps_removeSubScp_button
            // 
            this.DefineScps_removeSubScp_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DefineScps_removeSubScp_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.DefineScps_removeSubScp_button.ForeColor = System.Drawing.Color.White;
            this.DefineScps_removeSubScp_button.Location = new System.Drawing.Point(3, 207);
            this.DefineScps_removeSubScp_button.Name = "DefineScps_removeSubScp_button";
            this.DefineScps_removeSubScp_button.Size = new System.Drawing.Size(75, 24);
            this.DefineScps_removeSubScp_button.TabIndex = 231;
            this.DefineScps_removeSubScp_button.Text = "Remove";
            this.DefineScps_removeSubScp_button.UseVisualStyleBackColor = false;
            this.DefineScps_removeSubScp_button.Click += new System.EventHandler(this.DefineScps_removeSubScp_button_Click);
            // 
            // DefineScps_mbcoSCP_ownListBox
            // 
            this.DefineScps_mbcoSCP_ownListBox.FormattingEnabled = true;
            this.DefineScps_mbcoSCP_ownListBox.ItemHeight = 16;
            this.DefineScps_mbcoSCP_ownListBox.Location = new System.Drawing.Point(5, 20);
            this.DefineScps_mbcoSCP_ownListBox.Name = "DefineScps_mbcoSCP_ownListBox";
            this.DefineScps_mbcoSCP_ownListBox.ReadOnly = false;
            this.DefineScps_mbcoSCP_ownListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.DefineScps_mbcoSCP_ownListBox.Size = new System.Drawing.Size(345, 148);
            this.DefineScps_mbcoSCP_ownListBox.TabIndex = 229;
            this.DefineScps_mbcoSCP_ownListBox.SelectedIndexChanged += new System.EventHandler(this.DefineScps_mbcoSCP_ownListBox_SelectedIndexChanged);
            // 
            // DefineScps_sort_label
            // 
            this.DefineScps_sort_label.AutoSize = true;
            this.DefineScps_sort_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DefineScps_sort_label.Location = new System.Drawing.Point(109, 188);
            this.DefineScps_sort_label.Name = "DefineScps_sort_label";
            this.DefineScps_sort_label.Size = new System.Drawing.Size(91, 19);
            this.DefineScps_sort_label.TabIndex = 228;
            this.DefineScps_sort_label.Text = "Sort SCPs";
            // 
            // DefineScps_mbcoSCP_label
            // 
            this.DefineScps_mbcoSCP_label.AutoSize = true;
            this.DefineScps_mbcoSCP_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DefineScps_mbcoSCP_label.Location = new System.Drawing.Point(2, 3);
            this.DefineScps_mbcoSCP_label.Name = "DefineScps_mbcoSCP_label";
            this.DefineScps_mbcoSCP_label.Size = new System.Drawing.Size(107, 19);
            this.DefineScps_mbcoSCP_label.TabIndex = 208;
            this.DefineScps_mbcoSCP_label.Text = "MBCO SCPs";
            // 
            // DefineScps_addSubScp_button
            // 
            this.DefineScps_addSubScp_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DefineScps_addSubScp_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.DefineScps_addSubScp_button.ForeColor = System.Drawing.Color.White;
            this.DefineScps_addSubScp_button.Location = new System.Drawing.Point(3, 184);
            this.DefineScps_addSubScp_button.Name = "DefineScps_addSubScp_button";
            this.DefineScps_addSubScp_button.Size = new System.Drawing.Size(75, 24);
            this.DefineScps_addSubScp_button.TabIndex = 208;
            this.DefineScps_addSubScp_button.Text = "Add";
            this.DefineScps_addSubScp_button.UseVisualStyleBackColor = false;
            this.DefineScps_addSubScp_button.Click += new System.EventHandler(this.DefineScps_addSubScp_button_Click);
            // 
            // DefineScps_ownSubScps_ownListBox
            // 
            this.DefineScps_ownSubScps_ownListBox.FormattingEnabled = true;
            this.DefineScps_ownSubScps_ownListBox.ItemHeight = 16;
            this.DefineScps_ownSubScps_ownListBox.Location = new System.Drawing.Point(5, 270);
            this.DefineScps_ownSubScps_ownListBox.Name = "DefineScps_ownSubScps_ownListBox";
            this.DefineScps_ownSubScps_ownListBox.ReadOnly = false;
            this.DefineScps_ownSubScps_ownListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.DefineScps_ownSubScps_ownListBox.Size = new System.Drawing.Size(345, 116);
            this.DefineScps_ownSubScps_ownListBox.TabIndex = 230;
            this.DefineScps_ownSubScps_ownListBox.SelectedIndexChanged += new System.EventHandler(this.DefineScps_ownSubScps_ownListBox_SelectedIndexChanged);
            // 
            // DefineScps_level_label
            // 
            this.DefineScps_level_label.AutoSize = true;
            this.DefineScps_level_label.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.DefineScps_level_label.Location = new System.Drawing.Point(6, 98);
            this.DefineScps_level_label.Name = "DefineScps_level_label";
            this.DefineScps_level_label.Size = new System.Drawing.Size(161, 16);
            this.DefineScps_level_label.TabIndex = 242;
            this.DefineScps_level_label.Text = "Level of selected SCP";
            // 
            // DefineScps_removeOwnSCP_button
            // 
            this.DefineScps_removeOwnSCP_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DefineScps_removeOwnSCP_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DefineScps_removeOwnSCP_button.ForeColor = System.Drawing.Color.White;
            this.DefineScps_removeOwnSCP_button.Location = new System.Drawing.Point(163, 49);
            this.DefineScps_removeOwnSCP_button.Name = "DefineScps_removeOwnSCP_button";
            this.DefineScps_removeOwnSCP_button.Size = new System.Drawing.Size(190, 25);
            this.DefineScps_removeOwnSCP_button.TabIndex = 237;
            this.DefineScps_removeOwnSCP_button.Text = "Remove selected SCP";
            this.DefineScps_removeOwnSCP_button.UseVisualStyleBackColor = false;
            this.DefineScps_removeOwnSCP_button.Click += new System.EventHandler(this.DefineScps_removeOwnSCP_button_Click);
            // 
            // DefineScps_addNewOwnSCP_button
            // 
            this.DefineScps_addNewOwnSCP_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.DefineScps_addNewOwnSCP_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DefineScps_addNewOwnSCP_button.ForeColor = System.Drawing.Color.White;
            this.DefineScps_addNewOwnSCP_button.Location = new System.Drawing.Point(302, 24);
            this.DefineScps_addNewOwnSCP_button.Name = "DefineScps_addNewOwnSCP_button";
            this.DefineScps_addNewOwnSCP_button.Size = new System.Drawing.Size(50, 25);
            this.DefineScps_addNewOwnSCP_button.TabIndex = 235;
            this.DefineScps_addNewOwnSCP_button.Text = "Add";
            this.DefineScps_addNewOwnSCP_button.UseVisualStyleBackColor = false;
            this.DefineScps_addNewOwnSCP_button.Click += new System.EventHandler(this.DefineScps_addNewOwnSCP_button_Click);
            // 
            // DefineScps_selectOwnScp_label
            // 
            this.DefineScps_selectOwnScp_label.AutoSize = true;
            this.DefineScps_selectOwnScp_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DefineScps_selectOwnScp_label.Location = new System.Drawing.Point(4, 54);
            this.DefineScps_selectOwnScp_label.Name = "DefineScps_selectOwnScp_label";
            this.DefineScps_selectOwnScp_label.Size = new System.Drawing.Size(153, 19);
            this.DefineScps_selectOwnScp_label.TabIndex = 234;
            this.DefineScps_selectOwnScp_label.Text = "Selected own SCP";
            // 
            // DefineScps_selectOwnScp_ownListBox
            // 
            this.DefineScps_selectOwnScp_ownListBox.FormattingEnabled = true;
            this.DefineScps_selectOwnScp_ownListBox.ItemHeight = 16;
            this.DefineScps_selectOwnScp_ownListBox.Location = new System.Drawing.Point(8, 75);
            this.DefineScps_selectOwnScp_ownListBox.Name = "DefineScps_selectOwnScp_ownListBox";
            this.DefineScps_selectOwnScp_ownListBox.ReadOnly = false;
            this.DefineScps_selectOwnScp_ownListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.DefineScps_selectOwnScp_ownListBox.Size = new System.Drawing.Size(345, 4);
            this.DefineScps_selectOwnScp_ownListBox.TabIndex = 233;
            this.DefineScps_selectOwnScp_ownListBox.SelectedIndexChanged += new System.EventHandler(this.DefineScps_selectOwnScp_ownListBox_SelectedIndexChanged);
            // 
            // DefineScps_newOwnScpName_ownTextBox
            // 
            this.DefineScps_newOwnScpName_ownTextBox.Location = new System.Drawing.Point(122, 25);
            this.DefineScps_newOwnScpName_ownTextBox.Name = "DefineScps_newOwnScpName_ownTextBox";
            this.DefineScps_newOwnScpName_ownTextBox.Size = new System.Drawing.Size(178, 22);
            this.DefineScps_newOwnScpName_ownTextBox.TabIndex = 209;
            // 
            // DefineScps_newOwnScpName_label
            // 
            this.DefineScps_newOwnScpName_label.AutoSize = true;
            this.DefineScps_newOwnScpName_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DefineScps_newOwnScpName_label.Location = new System.Drawing.Point(5, 27);
            this.DefineScps_newOwnScpName_label.Name = "DefineScps_newOwnScpName_label";
            this.DefineScps_newOwnScpName_label.Size = new System.Drawing.Size(120, 19);
            this.DefineScps_newOwnScpName_label.TabIndex = 227;
            this.DefineScps_newOwnScpName_label.Text = "New own SCP";
            // 
            // DefineScps_overall_headline_label
            // 
            this.DefineScps_overall_headline_label.AutoSize = true;
            this.DefineScps_overall_headline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DefineScps_overall_headline_label.ForeColor = System.Drawing.Color.Black;
            this.DefineScps_overall_headline_label.Location = new System.Drawing.Point(99, 1);
            this.DefineScps_overall_headline_label.Name = "DefineScps_overall_headline_label";
            this.DefineScps_overall_headline_label.Size = new System.Drawing.Size(172, 24);
            this.DefineScps_overall_headline_label.TabIndex = 202;
            this.DefineScps_overall_headline_label.Text = "Define own SCPs";
            // 
            // Options_tips_panel
            // 
            this.Options_tips_panel.Border_color = System.Drawing.Color.Black;
            this.Options_tips_panel.Controls.Add(this.Tips_demonstration_headline_label);
            this.Options_tips_panel.Controls.Add(this.Tips_demonstration_cbLabel);
            this.Options_tips_panel.Controls.Add(this.Tips_demonstration_cbButton);
            this.Options_tips_panel.Controls.Add(this.Tips_label6);
            this.Options_tips_panel.Controls.Add(this.Tips_label5);
            this.Options_tips_panel.Controls.Add(this.Tips_label4);
            this.Options_tips_panel.Controls.Add(this.Tips_label3);
            this.Options_tips_panel.Controls.Add(this.Tips_label2);
            this.Options_tips_panel.Controls.Add(this.Tips_label1);
            this.Options_tips_panel.Controls.Add(this.Tips_overallHeadline_label);
            this.Options_tips_panel.Corner_radius = 10F;
            this.Options_tips_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_tips_panel.Location = new System.Drawing.Point(1705, 552);
            this.Options_tips_panel.Name = "Options_tips_panel";
            this.Options_tips_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_tips_panel.TabIndex = 208;
            // 
            // Tips_demonstration_headline_label
            // 
            this.Tips_demonstration_headline_label.AutoSize = true;
            this.Tips_demonstration_headline_label.Location = new System.Drawing.Point(159, 471);
            this.Tips_demonstration_headline_label.Name = "Tips_demonstration_headline_label";
            this.Tips_demonstration_headline_label.Size = new System.Drawing.Size(135, 16);
            this.Tips_demonstration_headline_label.TabIndex = 214;
            this.Tips_demonstration_headline_label.Text = "Test check box button";
            // 
            // Tips_demonstration_cbLabel
            // 
            this.Tips_demonstration_cbLabel.AutoSize = true;
            this.Tips_demonstration_cbLabel.Location = new System.Drawing.Point(80, 490);
            this.Tips_demonstration_cbLabel.Name = "Tips_demonstration_cbLabel";
            this.Tips_demonstration_cbLabel.Size = new System.Drawing.Size(41, 16);
            this.Tips_demonstration_cbLabel.TabIndex = 213;
            this.Tips_demonstration_cbLabel.Text = "label1";
            // 
            // Tips_demonstration_cbButton
            // 
            this.Tips_demonstration_cbButton.Checked = false;
            this.Tips_demonstration_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Tips_demonstration_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Tips_demonstration_cbButton.Location = new System.Drawing.Point(29, 477);
            this.Tips_demonstration_cbButton.Name = "Tips_demonstration_cbButton";
            this.Tips_demonstration_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Tips_demonstration_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Tips_demonstration_cbButton.Size = new System.Drawing.Size(25, 23);
            this.Tips_demonstration_cbButton.TabIndex = 212;
            this.Tips_demonstration_cbButton.Text = "myCheckBox_button2";
            this.Tips_demonstration_cbButton.UseVisualStyleBackColor = false;
            this.Tips_demonstration_cbButton.Click += new System.EventHandler(this.Tips_demonstration_cbButton_Click);
            // 
            // Tips_label6
            // 
            this.Tips_label6.AutoSize = true;
            this.Tips_label6.Location = new System.Drawing.Point(3, 406);
            this.Tips_label6.Name = "Tips_label6";
            this.Tips_label6.Size = new System.Drawing.Size(521, 48);
            this.Tips_label6.TabIndex = 211;
            this.Tips_label6.Text = resources.GetString("Tips_label6.Text");
            // 
            // Tips_label5
            // 
            this.Tips_label5.AutoSize = true;
            this.Tips_label5.Location = new System.Drawing.Point(5, 315);
            this.Tips_label5.Name = "Tips_label5";
            this.Tips_label5.Size = new System.Drawing.Size(359, 80);
            this.Tips_label5.TabIndex = 210;
            this.Tips_label5.Text = resources.GetString("Tips_label5.Text");
            // 
            // Tips_label4
            // 
            this.Tips_label4.AutoSize = true;
            this.Tips_label4.Location = new System.Drawing.Point(5, 234);
            this.Tips_label4.Name = "Tips_label4";
            this.Tips_label4.Size = new System.Drawing.Size(346, 64);
            this.Tips_label4.TabIndex = 209;
            this.Tips_label4.Text = resources.GetString("Tips_label4.Text");
            // 
            // Tips_label3
            // 
            this.Tips_label3.AutoSize = true;
            this.Tips_label3.Location = new System.Drawing.Point(5, 185);
            this.Tips_label3.Name = "Tips_label3";
            this.Tips_label3.Size = new System.Drawing.Size(348, 32);
            this.Tips_label3.TabIndex = 208;
            this.Tips_label3.Text = "Color maps that link colors to color names can be found by\r\nsearching the interne" +
    "t for \'C# colors\'.";
            // 
            // Tips_label2
            // 
            this.Tips_label2.AutoSize = true;
            this.Tips_label2.Location = new System.Drawing.Point(5, 106);
            this.Tips_label2.Name = "Tips_label2";
            this.Tips_label2.Size = new System.Drawing.Size(555, 48);
            this.Tips_label2.TabIndex = 207;
            this.Tips_label2.Text = resources.GetString("Tips_label2.Text");
            // 
            // Tips_label1
            // 
            this.Tips_label1.AutoSize = true;
            this.Tips_label1.Location = new System.Drawing.Point(5, 43);
            this.Tips_label1.Name = "Tips_label1";
            this.Tips_label1.Size = new System.Drawing.Size(323, 48);
            this.Tips_label1.TabIndex = 206;
            this.Tips_label1.Text = "Pressing letter keys allows quick jump to entries of list\r\nboxes that start with " +
    "the selected letter (e.g., when\r\nsearching for colors).";
            // 
            // Tips_overallHeadline_label
            // 
            this.Tips_overallHeadline_label.AutoSize = true;
            this.Tips_overallHeadline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.errorProvider1.SetIconAlignment(this.Tips_overallHeadline_label, System.Windows.Forms.ErrorIconAlignment.BottomRight);
            this.Tips_overallHeadline_label.Location = new System.Drawing.Point(151, 8);
            this.Tips_overallHeadline_label.Name = "Tips_overallHeadline_label";
            this.Tips_overallHeadline_label.Size = new System.Drawing.Size(51, 24);
            this.Tips_overallHeadline_label.TabIndex = 200;
            this.Tips_overallHeadline_label.Text = "Tips";
            this.Tips_overallHeadline_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Options_bgGenes_panel
            // 
            this.Options_bgGenes_panel.Border_color = System.Drawing.Color.Black;
            this.Options_bgGenes_panel.Controls.Add(this.BgGenes_warnings_panel);
            this.Options_bgGenes_panel.Controls.Add(this.BgGenes_overall_headline_label);
            this.Options_bgGenes_panel.Controls.Add(this.BgGenes_assignment_panel);
            this.Options_bgGenes_panel.Controls.Add(this.BgGenes_organize_panel);
            this.Options_bgGenes_panel.Controls.Add(this.BgGenes_add_panel);
            this.Options_bgGenes_panel.Corner_radius = 10F;
            this.Options_bgGenes_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_bgGenes_panel.Location = new System.Drawing.Point(993, 762);
            this.Options_bgGenes_panel.Name = "Options_bgGenes_panel";
            this.Options_bgGenes_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_bgGenes_panel.TabIndex = 216;
            // 
            // BgGenes_warnings_panel
            // 
            this.BgGenes_warnings_panel.Border_color = System.Drawing.Color.Transparent;
            this.BgGenes_warnings_panel.Controls.Add(this.BgGenes_warnings_label);
            this.BgGenes_warnings_panel.Controls.Add(this.BgGenes_warnings_button);
            this.BgGenes_warnings_panel.Corner_radius = 10F;
            this.BgGenes_warnings_panel.Fill_color = System.Drawing.Color.Transparent;
            this.BgGenes_warnings_panel.Location = new System.Drawing.Point(5, 412);
            this.BgGenes_warnings_panel.Name = "BgGenes_warnings_panel";
            this.BgGenes_warnings_panel.Size = new System.Drawing.Size(350, 25);
            this.BgGenes_warnings_panel.TabIndex = 229;
            // 
            // BgGenes_warnings_label
            // 
            this.BgGenes_warnings_label.AutoSize = true;
            this.BgGenes_warnings_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.BgGenes_warnings_label.Location = new System.Drawing.Point(6, 4);
            this.BgGenes_warnings_label.Name = "BgGenes_warnings_label";
            this.BgGenes_warnings_label.Size = new System.Drawing.Size(210, 18);
            this.BgGenes_warnings_label.TabIndex = 227;
            this.BgGenes_warnings_label.Text = "Data and bg genes mismatch";
            // 
            // BgGenes_warnings_button
            // 
            this.BgGenes_warnings_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BgGenes_warnings_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_warnings_button.ForeColor = System.Drawing.Color.White;
            this.BgGenes_warnings_button.Location = new System.Drawing.Point(116, 1);
            this.BgGenes_warnings_button.Name = "BgGenes_warnings_button";
            this.BgGenes_warnings_button.Size = new System.Drawing.Size(120, 25);
            this.BgGenes_warnings_button.TabIndex = 226;
            this.BgGenes_warnings_button.Text = "Explanation";
            this.BgGenes_warnings_button.UseVisualStyleBackColor = false;
            this.BgGenes_warnings_button.Click += new System.EventHandler(this.BgGenes_warnings_button_Click);
            // 
            // BgGenes_overall_headline_label
            // 
            this.BgGenes_overall_headline_label.AutoSize = true;
            this.BgGenes_overall_headline_label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.BgGenes_overall_headline_label.Location = new System.Drawing.Point(8, 7);
            this.BgGenes_overall_headline_label.Name = "BgGenes_overall_headline_label";
            this.BgGenes_overall_headline_label.Size = new System.Drawing.Size(345, 24);
            this.BgGenes_overall_headline_label.TabIndex = 224;
            this.BgGenes_overall_headline_label.Text = "Optional: Define background genes";
            // 
            // BgGenes_assignment_panel
            // 
            this.BgGenes_assignment_panel.Border_color = System.Drawing.Color.Transparent;
            this.BgGenes_assignment_panel.Controls.Add(this.BgGenes_assignmentsExplanation_label);
            this.BgGenes_assignment_panel.Controls.Add(this.BgGenes_assignmentsReset_label);
            this.BgGenes_assignment_panel.Controls.Add(this.BgGenes_assignmentsAutomatic_button);
            this.BgGenes_assignment_panel.Controls.Add(this.BgGenes_assignmentsAutomatic_label);
            this.BgGenes_assignment_panel.Controls.Add(this.BgGenes_assignmentsReset_button);
            this.BgGenes_assignment_panel.Corner_radius = 10F;
            this.BgGenes_assignment_panel.Fill_color = System.Drawing.Color.Transparent;
            this.BgGenes_assignment_panel.Location = new System.Drawing.Point(5, 442);
            this.BgGenes_assignment_panel.Name = "BgGenes_assignment_panel";
            this.BgGenes_assignment_panel.Size = new System.Drawing.Size(350, 78);
            this.BgGenes_assignment_panel.TabIndex = 229;
            // 
            // BgGenes_assignmentsExplanation_label
            // 
            this.BgGenes_assignmentsExplanation_label.AutoSize = true;
            this.BgGenes_assignmentsExplanation_label.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.BgGenes_assignmentsExplanation_label.Location = new System.Drawing.Point(9, 43);
            this.BgGenes_assignmentsExplanation_label.Name = "BgGenes_assignmentsExplanation_label";
            this.BgGenes_assignmentsExplanation_label.Size = new System.Drawing.Size(366, 32);
            this.BgGenes_assignmentsExplanation_label.TabIndex = 224;
            this.BgGenes_assignmentsExplanation_label.Text = "Each source file will be mapped to that background\r\ngene list that has the same n" +
    "ame plus \'_bgGenes\'.";
            // 
            // BgGenes_assignmentsReset_label
            // 
            this.BgGenes_assignmentsReset_label.AutoSize = true;
            this.BgGenes_assignmentsReset_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_assignmentsReset_label.Location = new System.Drawing.Point(25, 5);
            this.BgGenes_assignmentsReset_label.Name = "BgGenes_assignmentsReset_label";
            this.BgGenes_assignmentsReset_label.Size = new System.Drawing.Size(309, 19);
            this.BgGenes_assignmentsReset_label.TabIndex = 217;
            this.BgGenes_assignmentsReset_label.Text = "Reset background genes assignments";
            // 
            // BgGenes_assignmentsAutomatic_button
            // 
            this.BgGenes_assignmentsAutomatic_button.Location = new System.Drawing.Point(6, 24);
            this.BgGenes_assignmentsAutomatic_button.Name = "BgGenes_assignmentsAutomatic_button";
            this.BgGenes_assignmentsAutomatic_button.Size = new System.Drawing.Size(20, 20);
            this.BgGenes_assignmentsAutomatic_button.TabIndex = 208;
            this.BgGenes_assignmentsAutomatic_button.UseVisualStyleBackColor = true;
            this.BgGenes_assignmentsAutomatic_button.Click += new System.EventHandler(this.BgGenes_assignmentsAutomatic_button_Click);
            // 
            // BgGenes_assignmentsAutomatic_label
            // 
            this.BgGenes_assignmentsAutomatic_label.AutoSize = true;
            this.BgGenes_assignmentsAutomatic_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_assignmentsAutomatic_label.Location = new System.Drawing.Point(25, 25);
            this.BgGenes_assignmentsAutomatic_label.Name = "BgGenes_assignmentsAutomatic_label";
            this.BgGenes_assignmentsAutomatic_label.Size = new System.Drawing.Size(321, 19);
            this.BgGenes_assignmentsAutomatic_label.TabIndex = 209;
            this.BgGenes_assignmentsAutomatic_label.Text = "Automatically assign background genes";
            // 
            // BgGenes_assignmentsReset_button
            // 
            this.BgGenes_assignmentsReset_button.Location = new System.Drawing.Point(6, 4);
            this.BgGenes_assignmentsReset_button.Name = "BgGenes_assignmentsReset_button";
            this.BgGenes_assignmentsReset_button.Size = new System.Drawing.Size(20, 20);
            this.BgGenes_assignmentsReset_button.TabIndex = 216;
            this.BgGenes_assignmentsReset_button.UseVisualStyleBackColor = true;
            this.BgGenes_assignmentsReset_button.Click += new System.EventHandler(this.BgGenes_assignmentsReset_button_Click);
            // 
            // BgGenes_organize_panel
            // 
            this.BgGenes_organize_panel.Border_color = System.Drawing.Color.Transparent;
            this.BgGenes_organize_panel.Controls.Add(this.BgGenes_OrganizeAvailableBgGeneLists_label);
            this.BgGenes_organize_panel.Controls.Add(this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox);
            this.BgGenes_organize_panel.Controls.Add(this.BgGenes_OrganizeDeleteAll_button);
            this.BgGenes_organize_panel.Controls.Add(this.BgGenes_OrganizeDeleteSelection_button);
            this.BgGenes_organize_panel.Corner_radius = 10F;
            this.BgGenes_organize_panel.Fill_color = System.Drawing.Color.Transparent;
            this.BgGenes_organize_panel.Location = new System.Drawing.Point(5, 335);
            this.BgGenes_organize_panel.Name = "BgGenes_organize_panel";
            this.BgGenes_organize_panel.Size = new System.Drawing.Size(350, 72);
            this.BgGenes_organize_panel.TabIndex = 228;
            // 
            // BgGenes_OrganizeAvailableBgGeneLists_label
            // 
            this.BgGenes_OrganizeAvailableBgGeneLists_label.AutoSize = true;
            this.BgGenes_OrganizeAvailableBgGeneLists_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_OrganizeAvailableBgGeneLists_label.Location = new System.Drawing.Point(3, 4);
            this.BgGenes_OrganizeAvailableBgGeneLists_label.Name = "BgGenes_OrganizeAvailableBgGeneLists_label";
            this.BgGenes_OrganizeAvailableBgGeneLists_label.Size = new System.Drawing.Size(256, 19);
            this.BgGenes_OrganizeAvailableBgGeneLists_label.TabIndex = 224;
            this.BgGenes_OrganizeAvailableBgGeneLists_label.Text = "Available background gene lists";
            // 
            // BgGenes_OrganizeAvailableBgGeneLists_ownListBox
            // 
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.FormattingEnabled = true;
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.ItemHeight = 16;
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.Location = new System.Drawing.Point(5, 23);
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.Name = "BgGenes_OrganizeAvailableBgGeneLists_ownListBox";
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.ReadOnly = false;
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.Size = new System.Drawing.Size(342, 4);
            this.BgGenes_OrganizeAvailableBgGeneLists_ownListBox.TabIndex = 223;
            // 
            // BgGenes_OrganizeDeleteAll_button
            // 
            this.BgGenes_OrganizeDeleteAll_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BgGenes_OrganizeDeleteAll_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_OrganizeDeleteAll_button.ForeColor = System.Drawing.Color.White;
            this.BgGenes_OrganizeDeleteAll_button.Location = new System.Drawing.Point(175, 46);
            this.BgGenes_OrganizeDeleteAll_button.Name = "BgGenes_OrganizeDeleteAll_button";
            this.BgGenes_OrganizeDeleteAll_button.Size = new System.Drawing.Size(150, 25);
            this.BgGenes_OrganizeDeleteAll_button.TabIndex = 226;
            this.BgGenes_OrganizeDeleteAll_button.Text = "Delete all";
            this.BgGenes_OrganizeDeleteAll_button.UseVisualStyleBackColor = false;
            this.BgGenes_OrganizeDeleteAll_button.Click += new System.EventHandler(this.BgGenes_OrganizeDeleteAll_button_Click);
            // 
            // BgGenes_OrganizeDeleteSelection_button
            // 
            this.BgGenes_OrganizeDeleteSelection_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BgGenes_OrganizeDeleteSelection_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_OrganizeDeleteSelection_button.ForeColor = System.Drawing.Color.White;
            this.BgGenes_OrganizeDeleteSelection_button.Location = new System.Drawing.Point(25, 46);
            this.BgGenes_OrganizeDeleteSelection_button.Name = "BgGenes_OrganizeDeleteSelection_button";
            this.BgGenes_OrganizeDeleteSelection_button.Size = new System.Drawing.Size(150, 25);
            this.BgGenes_OrganizeDeleteSelection_button.TabIndex = 225;
            this.BgGenes_OrganizeDeleteSelection_button.Text = "Delete selection";
            this.BgGenes_OrganizeDeleteSelection_button.UseVisualStyleBackColor = false;
            this.BgGenes_OrganizeDeleteSelection_button.Click += new System.EventHandler(this.BgGenes_OrganizeDeleteSelection_button_Click);
            // 
            // BgGenes_add_panel
            // 
            this.BgGenes_add_panel.Border_color = System.Drawing.Color.Transparent;
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addReadAllFilesInDirectory_cbLabel);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addReadOnlyFile_cbLabel);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addReadAllFilesInDirectory_cbButton);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addReadOnlyFile_cbButton);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_AddErrors_label);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_AddShowErrors_button);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addReadExplainFile_label);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_add_button);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addGenes_ownTextBox);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_AddRead_button);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addName_label);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addReadFileDir_label);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addGenes_label);
            this.BgGenes_add_panel.Controls.Add(this.BgGenes_addName_ownTextBox);
            this.BgGenes_add_panel.Corner_radius = 10F;
            this.BgGenes_add_panel.Fill_color = System.Drawing.Color.Transparent;
            this.BgGenes_add_panel.Location = new System.Drawing.Point(5, 34);
            this.BgGenes_add_panel.Name = "BgGenes_add_panel";
            this.BgGenes_add_panel.Size = new System.Drawing.Size(350, 295);
            this.BgGenes_add_panel.TabIndex = 227;
            // 
            // BgGenes_addReadAllFilesInDirectory_cbLabel
            // 
            this.BgGenes_addReadAllFilesInDirectory_cbLabel.AutoSize = true;
            this.BgGenes_addReadAllFilesInDirectory_cbLabel.Location = new System.Drawing.Point(102, 208);
            this.BgGenes_addReadAllFilesInDirectory_cbLabel.Name = "BgGenes_addReadAllFilesInDirectory_cbLabel";
            this.BgGenes_addReadAllFilesInDirectory_cbLabel.Size = new System.Drawing.Size(133, 16);
            this.BgGenes_addReadAllFilesInDirectory_cbLabel.TabIndex = 230;
            this.BgGenes_addReadAllFilesInDirectory_cbLabel.Text = "all bg files in directory";
            // 
            // BgGenes_addReadOnlyFile_cbLabel
            // 
            this.BgGenes_addReadOnlyFile_cbLabel.AutoSize = true;
            this.BgGenes_addReadOnlyFile_cbLabel.Location = new System.Drawing.Point(102, 183);
            this.BgGenes_addReadOnlyFile_cbLabel.Name = "BgGenes_addReadOnlyFile_cbLabel";
            this.BgGenes_addReadOnlyFile_cbLabel.Size = new System.Drawing.Size(106, 16);
            this.BgGenes_addReadOnlyFile_cbLabel.TabIndex = 229;
            this.BgGenes_addReadOnlyFile_cbLabel.Text = "only specified file";
            // 
            // BgGenes_addReadAllFilesInDirectory_cbButton
            // 
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Checked = false;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Location = new System.Drawing.Point(70, 204);
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Name = "BgGenes_addReadAllFilesInDirectory_cbButton";
            this.BgGenes_addReadAllFilesInDirectory_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Size = new System.Drawing.Size(26, 23);
            this.BgGenes_addReadAllFilesInDirectory_cbButton.TabIndex = 228;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Text = "myCheckBox_button2";
            this.BgGenes_addReadAllFilesInDirectory_cbButton.UseVisualStyleBackColor = true;
            this.BgGenes_addReadAllFilesInDirectory_cbButton.Click += new System.EventHandler(this.BgGenes_addReadAllFilesInDirectory_cbButton_Click);
            // 
            // BgGenes_addReadOnlyFile_cbButton
            // 
            this.BgGenes_addReadOnlyFile_cbButton.Checked = false;
            this.BgGenes_addReadOnlyFile_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadOnlyFile_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadOnlyFile_cbButton.Location = new System.Drawing.Point(70, 180);
            this.BgGenes_addReadOnlyFile_cbButton.Name = "BgGenes_addReadOnlyFile_cbButton";
            this.BgGenes_addReadOnlyFile_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadOnlyFile_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.BgGenes_addReadOnlyFile_cbButton.Size = new System.Drawing.Size(26, 23);
            this.BgGenes_addReadOnlyFile_cbButton.TabIndex = 227;
            this.BgGenes_addReadOnlyFile_cbButton.Text = "myCheckBox_button1";
            this.BgGenes_addReadOnlyFile_cbButton.UseVisualStyleBackColor = true;
            this.BgGenes_addReadOnlyFile_cbButton.Click += new System.EventHandler(this.BgGenes_addReadOnlyFile_cbButton_Click);
            // 
            // BgGenes_AddErrors_label
            // 
            this.BgGenes_AddErrors_label.AutoSize = true;
            this.BgGenes_AddErrors_label.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.BgGenes_AddErrors_label.Location = new System.Drawing.Point(116, 271);
            this.BgGenes_AddErrors_label.Name = "BgGenes_AddErrors_label";
            this.BgGenes_AddErrors_label.Size = new System.Drawing.Size(99, 16);
            this.BgGenes_AddErrors_label.TabIndex = 226;
            this.BgGenes_AddErrors_label.Text = "Error reports";
            // 
            // BgGenes_AddShowErrors_button
            // 
            this.BgGenes_AddShowErrors_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BgGenes_AddShowErrors_button.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.BgGenes_AddShowErrors_button.ForeColor = System.Drawing.Color.White;
            this.BgGenes_AddShowErrors_button.Location = new System.Drawing.Point(6, 263);
            this.BgGenes_AddShowErrors_button.Name = "BgGenes_AddShowErrors_button";
            this.BgGenes_AddShowErrors_button.Size = new System.Drawing.Size(105, 30);
            this.BgGenes_AddShowErrors_button.TabIndex = 225;
            this.BgGenes_AddShowErrors_button.Text = "Show errors";
            this.BgGenes_AddShowErrors_button.UseVisualStyleBackColor = false;
            this.BgGenes_AddShowErrors_button.Click += new System.EventHandler(this.BgGenes_AddShowErrors_button_Click);
            // 
            // BgGenes_addReadExplainFile_label
            // 
            this.BgGenes_addReadExplainFile_label.AutoSize = true;
            this.BgGenes_addReadExplainFile_label.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.BgGenes_addReadExplainFile_label.Location = new System.Drawing.Point(1, 222);
            this.BgGenes_addReadExplainFile_label.Name = "BgGenes_addReadExplainFile_label";
            this.BgGenes_addReadExplainFile_label.Size = new System.Drawing.Size(388, 32);
            this.BgGenes_addReadExplainFile_label.TabIndex = 224;
            this.BgGenes_addReadExplainFile_label.Text = "List genes in one column, one gene per row (no\r\nheadline). All files ending with " +
    "\'_bgGenes\' will be read.";
            // 
            // BgGenes_add_button
            // 
            this.BgGenes_add_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BgGenes_add_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.BgGenes_add_button.ForeColor = System.Drawing.Color.White;
            this.BgGenes_add_button.Location = new System.Drawing.Point(160, 66);
            this.BgGenes_add_button.Name = "BgGenes_add_button";
            this.BgGenes_add_button.Size = new System.Drawing.Size(70, 30);
            this.BgGenes_add_button.TabIndex = 223;
            this.BgGenes_add_button.Text = "Add";
            this.BgGenes_add_button.UseVisualStyleBackColor = false;
            this.BgGenes_add_button.Click += new System.EventHandler(this.BgGenes_add_button_Click);
            // 
            // BgGenes_addGenes_ownTextBox
            // 
            this.BgGenes_addGenes_ownTextBox.Location = new System.Drawing.Point(4, 40);
            this.BgGenes_addGenes_ownTextBox.Multiline = true;
            this.BgGenes_addGenes_ownTextBox.Name = "BgGenes_addGenes_ownTextBox";
            this.BgGenes_addGenes_ownTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.BgGenes_addGenes_ownTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.BgGenes_addGenes_ownTextBox.Size = new System.Drawing.Size(153, 141);
            this.BgGenes_addGenes_ownTextBox.TabIndex = 218;
            // 
            // BgGenes_AddRead_button
            // 
            this.BgGenes_AddRead_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BgGenes_AddRead_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.BgGenes_AddRead_button.ForeColor = System.Drawing.Color.White;
            this.BgGenes_AddRead_button.Location = new System.Drawing.Point(254, 189);
            this.BgGenes_AddRead_button.Name = "BgGenes_AddRead_button";
            this.BgGenes_AddRead_button.Size = new System.Drawing.Size(70, 30);
            this.BgGenes_AddRead_button.TabIndex = 197;
            this.BgGenes_AddRead_button.Text = "Read";
            this.BgGenes_AddRead_button.UseVisualStyleBackColor = false;
            this.BgGenes_AddRead_button.Click += new System.EventHandler(this.BgGenes_AddRead_button_Click);
            // 
            // BgGenes_addName_label
            // 
            this.BgGenes_addName_label.AutoSize = true;
            this.BgGenes_addName_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_addName_label.Location = new System.Drawing.Point(157, 2);
            this.BgGenes_addName_label.Name = "BgGenes_addName_label";
            this.BgGenes_addName_label.Size = new System.Drawing.Size(174, 38);
            this.BgGenes_addName_label.TabIndex = 222;
            this.BgGenes_addName_label.Text = "Name of\r\nbackground gene list";
            // 
            // BgGenes_addReadFileDir_label
            // 
            this.BgGenes_addReadFileDir_label.AutoSize = true;
            this.BgGenes_addReadFileDir_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_addReadFileDir_label.Location = new System.Drawing.Point(1, 195);
            this.BgGenes_addReadFileDir_label.Name = "BgGenes_addReadFileDir_label";
            this.BgGenes_addReadFileDir_label.Size = new System.Drawing.Size(49, 19);
            this.BgGenes_addReadFileDir_label.TabIndex = 215;
            this.BgGenes_addReadFileDir_label.Text = "Read";
            // 
            // BgGenes_addGenes_label
            // 
            this.BgGenes_addGenes_label.AutoSize = true;
            this.BgGenes_addGenes_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.BgGenes_addGenes_label.Location = new System.Drawing.Point(1, 20);
            this.BgGenes_addGenes_label.Name = "BgGenes_addGenes_label";
            this.BgGenes_addGenes_label.Size = new System.Drawing.Size(157, 19);
            this.BgGenes_addGenes_label.TabIndex = 221;
            this.BgGenes_addGenes_label.Text = "Background genes";
            // 
            // BgGenes_addName_ownTextBox
            // 
            this.BgGenes_addName_ownTextBox.Location = new System.Drawing.Point(161, 40);
            this.BgGenes_addName_ownTextBox.Name = "BgGenes_addName_ownTextBox";
            this.BgGenes_addName_ownTextBox.Size = new System.Drawing.Size(186, 22);
            this.BgGenes_addName_ownTextBox.TabIndex = 220;
            // 
            // Options_organizeData_panel
            // 
            this.Options_organizeData_panel.Border_color = System.Drawing.Color.Black;
            this.Options_organizeData_panel.Controls.Add(this.OrganizeData_explanation_button);
            this.Options_organizeData_panel.Controls.Add(this.OrganizeData_convertTimeunits_panel);
            this.Options_organizeData_panel.Controls.Add(this.OrganizeData_automatically_panel);
            this.Options_organizeData_panel.Controls.Add(this.OrganizeData_addFileName_panel);
            this.Options_organizeData_panel.Controls.Add(this.OrganizeData_show_panel);
            this.Options_organizeData_panel.Controls.Add(this.OrganizeData_modify_panel);
            this.Options_organizeData_panel.Corner_radius = 10F;
            this.Options_organizeData_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Options_organizeData_panel.Location = new System.Drawing.Point(1305, 541);
            this.Options_organizeData_panel.Name = "Options_organizeData_panel";
            this.Options_organizeData_panel.Size = new System.Drawing.Size(360, 525);
            this.Options_organizeData_panel.TabIndex = 177;
            // 
            // OrganizeData_explanation_button
            // 
            this.OrganizeData_explanation_button.Location = new System.Drawing.Point(289, 467);
            this.OrganizeData_explanation_button.Name = "OrganizeData_explanation_button";
            this.OrganizeData_explanation_button.Size = new System.Drawing.Size(75, 23);
            this.OrganizeData_explanation_button.TabIndex = 214;
            this.OrganizeData_explanation_button.Text = "Explanation";
            this.OrganizeData_explanation_button.UseVisualStyleBackColor = true;
            this.OrganizeData_explanation_button.Click += new System.EventHandler(this.OrganizeData_explanation_button_Click);
            // 
            // OrganizeData_convertTimeunits_panel
            // 
            this.OrganizeData_convertTimeunits_panel.Border_color = System.Drawing.Color.Transparent;
            this.OrganizeData_convertTimeunits_panel.Controls.Add(this.OrganizeData_convertTimeunites_convert_button);
            this.OrganizeData_convertTimeunits_panel.Controls.Add(this.OrganizeData_convertTimeunits_unit_ownListBox);
            this.OrganizeData_convertTimeunits_panel.Controls.Add(this.OrganizeData_convertTimeunits_label);
            this.OrganizeData_convertTimeunits_panel.Corner_radius = 10F;
            this.OrganizeData_convertTimeunits_panel.Fill_color = System.Drawing.Color.Transparent;
            this.OrganizeData_convertTimeunits_panel.Location = new System.Drawing.Point(5, 242);
            this.OrganizeData_convertTimeunits_panel.Name = "OrganizeData_convertTimeunits_panel";
            this.OrganizeData_convertTimeunits_panel.Size = new System.Drawing.Size(350, 28);
            this.OrganizeData_convertTimeunits_panel.TabIndex = 213;
            // 
            // OrganizeData_convertTimeunites_convert_button
            // 
            this.OrganizeData_convertTimeunites_convert_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_convertTimeunites_convert_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_convertTimeunites_convert_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_convertTimeunites_convert_button.Location = new System.Drawing.Point(259, 1);
            this.OrganizeData_convertTimeunites_convert_button.Name = "OrganizeData_convertTimeunites_convert_button";
            this.OrganizeData_convertTimeunites_convert_button.Size = new System.Drawing.Size(82, 25);
            this.OrganizeData_convertTimeunites_convert_button.TabIndex = 233;
            this.OrganizeData_convertTimeunites_convert_button.Text = "Convert";
            this.OrganizeData_convertTimeunites_convert_button.UseVisualStyleBackColor = false;
            this.OrganizeData_convertTimeunites_convert_button.Click += new System.EventHandler(this.OrganizeData_convertTimeunites_convert_button_Click);
            // 
            // OrganizeData_convertTimeunits_unit_ownListBox
            // 
            this.OrganizeData_convertTimeunits_unit_ownListBox.FormattingEnabled = true;
            this.OrganizeData_convertTimeunits_unit_ownListBox.ItemHeight = 16;
            this.OrganizeData_convertTimeunits_unit_ownListBox.Location = new System.Drawing.Point(186, 11);
            this.OrganizeData_convertTimeunits_unit_ownListBox.Name = "OrganizeData_convertTimeunits_unit_ownListBox";
            this.OrganizeData_convertTimeunits_unit_ownListBox.ReadOnly = false;
            this.OrganizeData_convertTimeunits_unit_ownListBox.Size = new System.Drawing.Size(70, 4);
            this.OrganizeData_convertTimeunits_unit_ownListBox.TabIndex = 120;
            // 
            // OrganizeData_convertTimeunits_label
            // 
            this.OrganizeData_convertTimeunits_label.AutoSize = true;
            this.OrganizeData_convertTimeunits_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_convertTimeunits_label.Location = new System.Drawing.Point(4, 5);
            this.OrganizeData_convertTimeunits_label.Name = "OrganizeData_convertTimeunits_label";
            this.OrganizeData_convertTimeunits_label.Size = new System.Drawing.Size(180, 19);
            this.OrganizeData_convertTimeunits_label.TabIndex = 200;
            this.OrganizeData_convertTimeunits_label.Text = "Convert timepoints to";
            // 
            // OrganizeData_automatically_panel
            // 
            this.OrganizeData_automatically_panel.Border_color = System.Drawing.Color.Transparent;
            this.OrganizeData_automatically_panel.Controls.Add(this.OrganizeData_automatically_headline_label);
            this.OrganizeData_automatically_panel.Controls.Add(this.OrganizeData_automaticDatasetOrder_button);
            this.OrganizeData_automatically_panel.Controls.Add(this.OrganizeData_automaticIntegrationGroups_button);
            this.OrganizeData_automatically_panel.Controls.Add(this.OrganizeData_automaticColors_button);
            this.OrganizeData_automatically_panel.Corner_radius = 10F;
            this.OrganizeData_automatically_panel.Fill_color = System.Drawing.Color.Transparent;
            this.OrganizeData_automatically_panel.Location = new System.Drawing.Point(5, 451);
            this.OrganizeData_automatically_panel.Name = "OrganizeData_automatically_panel";
            this.OrganizeData_automatically_panel.Size = new System.Drawing.Size(267, 69);
            this.OrganizeData_automatically_panel.TabIndex = 213;
            // 
            // OrganizeData_automatically_headline_label
            // 
            this.OrganizeData_automatically_headline_label.AutoSize = true;
            this.OrganizeData_automatically_headline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_automatically_headline_label.Location = new System.Drawing.Point(105, 2);
            this.OrganizeData_automatically_headline_label.Name = "OrganizeData_automatically_headline_label";
            this.OrganizeData_automatically_headline_label.Size = new System.Drawing.Size(143, 19);
            this.OrganizeData_automatically_headline_label.TabIndex = 220;
            this.OrganizeData_automatically_headline_label.Text = "Automatically set";
            // 
            // OrganizeData_automaticDatasetOrder_button
            // 
            this.OrganizeData_automaticDatasetOrder_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_automaticDatasetOrder_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_automaticDatasetOrder_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_automaticDatasetOrder_button.Location = new System.Drawing.Point(175, 14);
            this.OrganizeData_automaticDatasetOrder_button.Name = "OrganizeData_automaticDatasetOrder_button";
            this.OrganizeData_automaticDatasetOrder_button.Size = new System.Drawing.Size(80, 46);
            this.OrganizeData_automaticDatasetOrder_button.TabIndex = 216;
            this.OrganizeData_automaticDatasetOrder_button.Text = "Dataset\r\norder";
            this.OrganizeData_automaticDatasetOrder_button.UseVisualStyleBackColor = false;
            this.OrganizeData_automaticDatasetOrder_button.Click += new System.EventHandler(this.OrganizeData_automaticDatasetOrder_button_Click);
            // 
            // OrganizeData_automaticIntegrationGroups_button
            // 
            this.OrganizeData_automaticIntegrationGroups_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_automaticIntegrationGroups_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_automaticIntegrationGroups_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_automaticIntegrationGroups_button.Location = new System.Drawing.Point(10, 23);
            this.OrganizeData_automaticIntegrationGroups_button.Name = "OrganizeData_automaticIntegrationGroups_button";
            this.OrganizeData_automaticIntegrationGroups_button.Size = new System.Drawing.Size(80, 46);
            this.OrganizeData_automaticIntegrationGroups_button.TabIndex = 214;
            this.OrganizeData_automaticIntegrationGroups_button.Text = "Int.\r\ngroups";
            this.OrganizeData_automaticIntegrationGroups_button.UseVisualStyleBackColor = false;
            this.OrganizeData_automaticIntegrationGroups_button.Click += new System.EventHandler(this.OrganizeData_automaticIntegrationGroups_button_Click);
            // 
            // OrganizeData_automaticColors_button
            // 
            this.OrganizeData_automaticColors_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_automaticColors_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_automaticColors_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_automaticColors_button.Location = new System.Drawing.Point(99, 20);
            this.OrganizeData_automaticColors_button.Name = "OrganizeData_automaticColors_button";
            this.OrganizeData_automaticColors_button.Size = new System.Drawing.Size(80, 46);
            this.OrganizeData_automaticColors_button.TabIndex = 215;
            this.OrganizeData_automaticColors_button.Text = "Colors";
            this.OrganizeData_automaticColors_button.UseVisualStyleBackColor = false;
            this.OrganizeData_automaticColors_button.Click += new System.EventHandler(this.OrganizeData_automaticColors_button_Click);
            // 
            // OrganizeData_addFileName_panel
            // 
            this.OrganizeData_addFileName_panel.Border_color = System.Drawing.Color.Transparent;
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNames_listBox);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNameAfter_label);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNameBefore_label);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNameRemove_label);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNameRemove_button);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNameAfter_button);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNamesBefore_button);
            this.OrganizeData_addFileName_panel.Controls.Add(this.OrganizeData_addFileNames_label);
            this.OrganizeData_addFileName_panel.Corner_radius = 10F;
            this.OrganizeData_addFileName_panel.Fill_color = System.Drawing.Color.Transparent;
            this.OrganizeData_addFileName_panel.Location = new System.Drawing.Point(5, 170);
            this.OrganizeData_addFileName_panel.Name = "OrganizeData_addFileName_panel";
            this.OrganizeData_addFileName_panel.Size = new System.Drawing.Size(350, 69);
            this.OrganizeData_addFileName_panel.TabIndex = 212;
            // 
            // OrganizeData_addFileNames_listBox
            // 
            this.OrganizeData_addFileNames_listBox.FormattingEnabled = true;
            this.OrganizeData_addFileNames_listBox.ItemHeight = 16;
            this.OrganizeData_addFileNames_listBox.Location = new System.Drawing.Point(62, 19);
            this.OrganizeData_addFileNames_listBox.Name = "OrganizeData_addFileNames_listBox";
            this.OrganizeData_addFileNames_listBox.ReadOnly = false;
            this.OrganizeData_addFileNames_listBox.Size = new System.Drawing.Size(70, 4);
            this.OrganizeData_addFileNames_listBox.TabIndex = 234;
            this.OrganizeData_addFileNames_listBox.SelectedIndexChanged += new System.EventHandler(this.OrganizeData_addFileNames_listBox_SelectedIndexChanged);
            // 
            // OrganizeData_addFileNameAfter_label
            // 
            this.OrganizeData_addFileNameAfter_label.AutoSize = true;
            this.OrganizeData_addFileNameAfter_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_addFileNameAfter_label.Location = new System.Drawing.Point(173, 25);
            this.OrganizeData_addFileNameAfter_label.Name = "OrganizeData_addFileNameAfter_label";
            this.OrganizeData_addFileNameAfter_label.Size = new System.Drawing.Size(164, 19);
            this.OrganizeData_addFileNameAfter_label.TabIndex = 207;
            this.OrganizeData_addFileNameAfter_label.Text = "after dataset names";
            // 
            // OrganizeData_addFileNameBefore_label
            // 
            this.OrganizeData_addFileNameBefore_label.AutoSize = true;
            this.OrganizeData_addFileNameBefore_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_addFileNameBefore_label.Location = new System.Drawing.Point(171, 3);
            this.OrganizeData_addFileNameBefore_label.Name = "OrganizeData_addFileNameBefore_label";
            this.OrganizeData_addFileNameBefore_label.Size = new System.Drawing.Size(178, 19);
            this.OrganizeData_addFileNameBefore_label.TabIndex = 206;
            this.OrganizeData_addFileNameBefore_label.Text = "before dataset names";
            // 
            // OrganizeData_addFileNameRemove_label
            // 
            this.OrganizeData_addFileNameRemove_label.AutoSize = true;
            this.OrganizeData_addFileNameRemove_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_addFileNameRemove_label.Location = new System.Drawing.Point(27, 45);
            this.OrganizeData_addFileNameRemove_label.Name = "OrganizeData_addFileNameRemove_label";
            this.OrganizeData_addFileNameRemove_label.Size = new System.Drawing.Size(187, 19);
            this.OrganizeData_addFileNameRemove_label.TabIndex = 205;
            this.OrganizeData_addFileNameRemove_label.Text = "Remove source names";
            // 
            // OrganizeData_addFileNameRemove_button
            // 
            this.OrganizeData_addFileNameRemove_button.Location = new System.Drawing.Point(8, 44);
            this.OrganizeData_addFileNameRemove_button.Name = "OrganizeData_addFileNameRemove_button";
            this.OrganizeData_addFileNameRemove_button.Size = new System.Drawing.Size(20, 20);
            this.OrganizeData_addFileNameRemove_button.TabIndex = 204;
            this.OrganizeData_addFileNameRemove_button.UseVisualStyleBackColor = true;
            this.OrganizeData_addFileNameRemove_button.Click += new System.EventHandler(this.OrganizeData_addFileNameRemove_button_Click);
            // 
            // OrganizeData_addFileNameAfter_button
            // 
            this.OrganizeData_addFileNameAfter_button.Location = new System.Drawing.Point(153, 26);
            this.OrganizeData_addFileNameAfter_button.Name = "OrganizeData_addFileNameAfter_button";
            this.OrganizeData_addFileNameAfter_button.Size = new System.Drawing.Size(20, 20);
            this.OrganizeData_addFileNameAfter_button.TabIndex = 203;
            this.OrganizeData_addFileNameAfter_button.UseVisualStyleBackColor = true;
            this.OrganizeData_addFileNameAfter_button.Click += new System.EventHandler(this.OrganizeData_addFileNameAfter_button_Click);
            // 
            // OrganizeData_addFileNamesBefore_button
            // 
            this.OrganizeData_addFileNamesBefore_button.Location = new System.Drawing.Point(153, 3);
            this.OrganizeData_addFileNamesBefore_button.Name = "OrganizeData_addFileNamesBefore_button";
            this.OrganizeData_addFileNamesBefore_button.Size = new System.Drawing.Size(20, 20);
            this.OrganizeData_addFileNamesBefore_button.TabIndex = 202;
            this.OrganizeData_addFileNamesBefore_button.UseVisualStyleBackColor = true;
            this.OrganizeData_addFileNamesBefore_button.Click += new System.EventHandler(this.OrganizeData_addFileNamesbefore_button_Click);
            // 
            // OrganizeData_addFileNames_label
            // 
            this.OrganizeData_addFileNames_label.AutoSize = true;
            this.OrganizeData_addFileNames_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_addFileNames_label.Location = new System.Drawing.Point(2, 14);
            this.OrganizeData_addFileNames_label.Name = "OrganizeData_addFileNames_label";
            this.OrganizeData_addFileNames_label.Size = new System.Drawing.Size(40, 19);
            this.OrganizeData_addFileNames_label.TabIndex = 200;
            this.OrganizeData_addFileNames_label.Text = "Add";
            // 
            // OrganizeData_show_panel
            // 
            this.OrganizeData_show_panel.Border_color = System.Drawing.Color.Transparent;
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showDatasetOrderNo_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showSourceFile_label);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showSourceFile_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showColor_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showIntegrationGroup_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showTimepoint_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showEntryType_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showName_cbLabel);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showDatasetOrderNo_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showSourceFile_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showColor_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showIntegrationGroup_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showTimepoint_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showEntryType_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showName_cbButton);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showDifferentEntries_label);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_showDifferentEntries_button);
            this.OrganizeData_show_panel.Controls.Add(this.OrganizeData_show_headline_label);
            this.OrganizeData_show_panel.Corner_radius = 10F;
            this.OrganizeData_show_panel.Fill_color = System.Drawing.Color.Transparent;
            this.OrganizeData_show_panel.Location = new System.Drawing.Point(5, 5);
            this.OrganizeData_show_panel.Name = "OrganizeData_show_panel";
            this.OrganizeData_show_panel.Size = new System.Drawing.Size(350, 160);
            this.OrganizeData_show_panel.TabIndex = 210;
            // 
            // OrganizeData_showDatasetOrderNo_cbLabel
            // 
            this.OrganizeData_showDatasetOrderNo_cbLabel.AutoSize = true;
            this.OrganizeData_showDatasetOrderNo_cbLabel.Location = new System.Drawing.Point(155, 116);
            this.OrganizeData_showDatasetOrderNo_cbLabel.Name = "OrganizeData_showDatasetOrderNo_cbLabel";
            this.OrganizeData_showDatasetOrderNo_cbLabel.Size = new System.Drawing.Size(265, 16);
            this.OrganizeData_showDatasetOrderNo_cbLabel.TabIndex = 235;
            this.OrganizeData_showDatasetOrderNo_cbLabel.Text = "Dataset order # within each integration group";
            // 
            // OrganizeData_showSourceFile_label
            // 
            this.OrganizeData_showSourceFile_label.AutoSize = true;
            this.OrganizeData_showSourceFile_label.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Italic);
            this.OrganizeData_showSourceFile_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.OrganizeData_showSourceFile_label.Location = new System.Drawing.Point(31, 110);
            this.OrganizeData_showSourceFile_label.Name = "OrganizeData_showSourceFile_label";
            this.OrganizeData_showSourceFile_label.Size = new System.Drawing.Size(93, 16);
            this.OrganizeData_showSourceFile_label.TabIndex = 201;
            this.OrganizeData_showSourceFile_label.Text = "(-> bg genes)";
            this.OrganizeData_showSourceFile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OrganizeData_showSourceFile_cbLabel
            // 
            this.OrganizeData_showSourceFile_cbLabel.AutoSize = true;
            this.OrganizeData_showSourceFile_cbLabel.Location = new System.Drawing.Point(114, 59);
            this.OrganizeData_showSourceFile_cbLabel.Name = "OrganizeData_showSourceFile_cbLabel";
            this.OrganizeData_showSourceFile_cbLabel.Size = new System.Drawing.Size(48, 16);
            this.OrganizeData_showSourceFile_cbLabel.TabIndex = 234;
            this.OrganizeData_showSourceFile_cbLabel.Text = "Source";
            // 
            // OrganizeData_showColor_cbLabel
            // 
            this.OrganizeData_showColor_cbLabel.AutoSize = true;
            this.OrganizeData_showColor_cbLabel.Location = new System.Drawing.Point(100, 117);
            this.OrganizeData_showColor_cbLabel.Name = "OrganizeData_showColor_cbLabel";
            this.OrganizeData_showColor_cbLabel.Size = new System.Drawing.Size(37, 16);
            this.OrganizeData_showColor_cbLabel.TabIndex = 233;
            this.OrganizeData_showColor_cbLabel.Text = "Color";
            // 
            // OrganizeData_showIntegrationGroup_cbLabel
            // 
            this.OrganizeData_showIntegrationGroup_cbLabel.AutoSize = true;
            this.OrganizeData_showIntegrationGroup_cbLabel.Location = new System.Drawing.Point(127, 121);
            this.OrganizeData_showIntegrationGroup_cbLabel.Name = "OrganizeData_showIntegrationGroup_cbLabel";
            this.OrganizeData_showIntegrationGroup_cbLabel.Size = new System.Drawing.Size(103, 16);
            this.OrganizeData_showIntegrationGroup_cbLabel.TabIndex = 232;
            this.OrganizeData_showIntegrationGroup_cbLabel.Text = "Integration group";
            // 
            // OrganizeData_showTimepoint_cbLabel
            // 
            this.OrganizeData_showTimepoint_cbLabel.AutoSize = true;
            this.OrganizeData_showTimepoint_cbLabel.Location = new System.Drawing.Point(244, 81);
            this.OrganizeData_showTimepoint_cbLabel.Name = "OrganizeData_showTimepoint_cbLabel";
            this.OrganizeData_showTimepoint_cbLabel.Size = new System.Drawing.Size(63, 16);
            this.OrganizeData_showTimepoint_cbLabel.TabIndex = 231;
            this.OrganizeData_showTimepoint_cbLabel.Text = "Timepoint";
            // 
            // OrganizeData_showEntryType_cbLabel
            // 
            this.OrganizeData_showEntryType_cbLabel.AutoSize = true;
            this.OrganizeData_showEntryType_cbLabel.Location = new System.Drawing.Point(175, 77);
            this.OrganizeData_showEntryType_cbLabel.Name = "OrganizeData_showEntryType_cbLabel";
            this.OrganizeData_showEntryType_cbLabel.Size = new System.Drawing.Size(57, 16);
            this.OrganizeData_showEntryType_cbLabel.TabIndex = 230;
            this.OrganizeData_showEntryType_cbLabel.Text = "Up/down";
            // 
            // OrganizeData_showName_cbLabel
            // 
            this.OrganizeData_showName_cbLabel.AutoSize = true;
            this.OrganizeData_showName_cbLabel.Location = new System.Drawing.Point(134, 31);
            this.OrganizeData_showName_cbLabel.Name = "OrganizeData_showName_cbLabel";
            this.OrganizeData_showName_cbLabel.Size = new System.Drawing.Size(41, 16);
            this.OrganizeData_showName_cbLabel.TabIndex = 229;
            this.OrganizeData_showName_cbLabel.Text = "Name";
            // 
            // OrganizeData_showDatasetOrderNo_cbButton
            // 
            this.OrganizeData_showDatasetOrderNo_cbButton.Checked = false;
            this.OrganizeData_showDatasetOrderNo_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showDatasetOrderNo_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showDatasetOrderNo_cbButton.Location = new System.Drawing.Point(243, 88);
            this.OrganizeData_showDatasetOrderNo_cbButton.Name = "OrganizeData_showDatasetOrderNo_cbButton";
            this.OrganizeData_showDatasetOrderNo_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showDatasetOrderNo_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showDatasetOrderNo_cbButton.Size = new System.Drawing.Size(24, 23);
            this.OrganizeData_showDatasetOrderNo_cbButton.TabIndex = 228;
            this.OrganizeData_showDatasetOrderNo_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showDatasetOrderNo_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showDatasetOrderNo_cbButton.Click += new System.EventHandler(this.OrganizeData_showDatasetOrderNo_cbButton_Click);
            // 
            // OrganizeData_showSourceFile_cbButton
            // 
            this.OrganizeData_showSourceFile_cbButton.Checked = false;
            this.OrganizeData_showSourceFile_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showSourceFile_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showSourceFile_cbButton.Location = new System.Drawing.Point(142, 121);
            this.OrganizeData_showSourceFile_cbButton.Name = "OrganizeData_showSourceFile_cbButton";
            this.OrganizeData_showSourceFile_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showSourceFile_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showSourceFile_cbButton.Size = new System.Drawing.Size(21, 23);
            this.OrganizeData_showSourceFile_cbButton.TabIndex = 227;
            this.OrganizeData_showSourceFile_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showSourceFile_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showSourceFile_cbButton.Click += new System.EventHandler(this.OrganizeData_showSourceFile_cbButton_Click_1);
            // 
            // OrganizeData_showColor_cbButton
            // 
            this.OrganizeData_showColor_cbButton.Checked = false;
            this.OrganizeData_showColor_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showColor_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showColor_cbButton.Location = new System.Drawing.Point(184, 92);
            this.OrganizeData_showColor_cbButton.Name = "OrganizeData_showColor_cbButton";
            this.OrganizeData_showColor_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showColor_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showColor_cbButton.Size = new System.Drawing.Size(21, 23);
            this.OrganizeData_showColor_cbButton.TabIndex = 226;
            this.OrganizeData_showColor_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showColor_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showColor_cbButton.Click += new System.EventHandler(this.OrganizeData_showColor_cbButton_Click_1);
            // 
            // OrganizeData_showIntegrationGroup_cbButton
            // 
            this.OrganizeData_showIntegrationGroup_cbButton.Checked = false;
            this.OrganizeData_showIntegrationGroup_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showIntegrationGroup_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showIntegrationGroup_cbButton.Location = new System.Drawing.Point(27, 54);
            this.OrganizeData_showIntegrationGroup_cbButton.Name = "OrganizeData_showIntegrationGroup_cbButton";
            this.OrganizeData_showIntegrationGroup_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showIntegrationGroup_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showIntegrationGroup_cbButton.Size = new System.Drawing.Size(23, 23);
            this.OrganizeData_showIntegrationGroup_cbButton.TabIndex = 225;
            this.OrganizeData_showIntegrationGroup_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showIntegrationGroup_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showIntegrationGroup_cbButton.Click += new System.EventHandler(this.OrganizeData_showIntegrationGroup_cbButton_Click_1);
            // 
            // OrganizeData_showTimepoint_cbButton
            // 
            this.OrganizeData_showTimepoint_cbButton.Checked = false;
            this.OrganizeData_showTimepoint_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showTimepoint_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showTimepoint_cbButton.Location = new System.Drawing.Point(151, 62);
            this.OrganizeData_showTimepoint_cbButton.Name = "OrganizeData_showTimepoint_cbButton";
            this.OrganizeData_showTimepoint_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showTimepoint_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showTimepoint_cbButton.Size = new System.Drawing.Size(21, 23);
            this.OrganizeData_showTimepoint_cbButton.TabIndex = 224;
            this.OrganizeData_showTimepoint_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showTimepoint_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showTimepoint_cbButton.Click += new System.EventHandler(this.OrganizeData_showTimepoint_cbButton_Click_1);
            // 
            // OrganizeData_showEntryType_cbButton
            // 
            this.OrganizeData_showEntryType_cbButton.Checked = false;
            this.OrganizeData_showEntryType_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showEntryType_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showEntryType_cbButton.Location = new System.Drawing.Point(151, 35);
            this.OrganizeData_showEntryType_cbButton.Name = "OrganizeData_showEntryType_cbButton";
            this.OrganizeData_showEntryType_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showEntryType_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showEntryType_cbButton.Size = new System.Drawing.Size(21, 23);
            this.OrganizeData_showEntryType_cbButton.TabIndex = 223;
            this.OrganizeData_showEntryType_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showEntryType_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showEntryType_cbButton.Click += new System.EventHandler(this.OrganizeData_showEntryType_cbButton_Click_1);
            // 
            // OrganizeData_showName_cbButton
            // 
            this.OrganizeData_showName_cbButton.Checked = false;
            this.OrganizeData_showName_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showName_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showName_cbButton.Location = new System.Drawing.Point(37, 31);
            this.OrganizeData_showName_cbButton.Name = "OrganizeData_showName_cbButton";
            this.OrganizeData_showName_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_showName_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_showName_cbButton.Size = new System.Drawing.Size(21, 23);
            this.OrganizeData_showName_cbButton.TabIndex = 222;
            this.OrganizeData_showName_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_showName_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_showName_cbButton.Click += new System.EventHandler(this.OrganizeData_showName_cbButton_Click_1);
            // 
            // OrganizeData_showDifferentEntries_label
            // 
            this.OrganizeData_showDifferentEntries_label.AutoSize = true;
            this.OrganizeData_showDifferentEntries_label.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrganizeData_showDifferentEntries_label.Location = new System.Drawing.Point(198, 24);
            this.OrganizeData_showDifferentEntries_label.Name = "OrganizeData_showDifferentEntries_label";
            this.OrganizeData_showDifferentEntries_label.Size = new System.Drawing.Size(147, 57);
            this.OrganizeData_showDifferentEntries_label.TabIndex = 218;
            this.OrganizeData_showDifferentEntries_label.Text = "Select attributes\r\nthat differ\r\nbetween datasets";
            // 
            // OrganizeData_showDifferentEntries_button
            // 
            this.OrganizeData_showDifferentEntries_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_showDifferentEntries_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_showDifferentEntries_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_showDifferentEntries_button.Location = new System.Drawing.Point(178, 42);
            this.OrganizeData_showDifferentEntries_button.Name = "OrganizeData_showDifferentEntries_button";
            this.OrganizeData_showDifferentEntries_button.Size = new System.Drawing.Size(20, 20);
            this.OrganizeData_showDifferentEntries_button.TabIndex = 221;
            this.OrganizeData_showDifferentEntries_button.UseVisualStyleBackColor = false;
            this.OrganizeData_showDifferentEntries_button.Click += new System.EventHandler(this.OrganizeData_showDifferentEntries_button_Click);
            // 
            // OrganizeData_show_headline_label
            // 
            this.OrganizeData_show_headline_label.AutoSize = true;
            this.OrganizeData_show_headline_label.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrganizeData_show_headline_label.Location = new System.Drawing.Point(2, 6);
            this.OrganizeData_show_headline_label.Name = "OrganizeData_show_headline_label";
            this.OrganizeData_show_headline_label.Size = new System.Drawing.Size(197, 19);
            this.OrganizeData_show_headline_label.TabIndex = 207;
            this.OrganizeData_show_headline_label.Text = "Show dataset attributes";
            // 
            // OrganizeData_modify_panel
            // 
            this.OrganizeData_modify_panel.Border_color = System.Drawing.Color.Transparent;
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifySourceFileName_cbLabel);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifySubstring_cbLabel);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyEntryType_cbLabel);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyTimepoint_cbLabel);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyName_cbLabel);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifySubstring_cbButton);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifySourceFileName_cbButton);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyEntryType_cbButton);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyTimepoint_cbButton);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyName_cbButton);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifySubstringOptions_panel);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_changeDelete_button);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_changeIntegrationGroup_button);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_changeColor_button);
            this.OrganizeData_modify_panel.Controls.Add(this.OrganizeData_modifyHeadline_label);
            this.OrganizeData_modify_panel.Corner_radius = 10F;
            this.OrganizeData_modify_panel.Fill_color = System.Drawing.Color.Transparent;
            this.OrganizeData_modify_panel.Location = new System.Drawing.Point(5, 273);
            this.OrganizeData_modify_panel.Name = "OrganizeData_modify_panel";
            this.OrganizeData_modify_panel.Size = new System.Drawing.Size(350, 172);
            this.OrganizeData_modify_panel.TabIndex = 208;
            // 
            // OrganizeData_modifySourceFileName_cbLabel
            // 
            this.OrganizeData_modifySourceFileName_cbLabel.AutoSize = true;
            this.OrganizeData_modifySourceFileName_cbLabel.Location = new System.Drawing.Point(34, 80);
            this.OrganizeData_modifySourceFileName_cbLabel.Name = "OrganizeData_modifySourceFileName_cbLabel";
            this.OrganizeData_modifySourceFileName_cbLabel.Size = new System.Drawing.Size(48, 16);
            this.OrganizeData_modifySourceFileName_cbLabel.TabIndex = 230;
            this.OrganizeData_modifySourceFileName_cbLabel.Text = "Source";
            // 
            // OrganizeData_modifySubstring_cbLabel
            // 
            this.OrganizeData_modifySubstring_cbLabel.AutoSize = true;
            this.OrganizeData_modifySubstring_cbLabel.Location = new System.Drawing.Point(50, 114);
            this.OrganizeData_modifySubstring_cbLabel.Name = "OrganizeData_modifySubstring_cbLabel";
            this.OrganizeData_modifySubstring_cbLabel.Size = new System.Drawing.Size(129, 16);
            this.OrganizeData_modifySubstring_cbLabel.TabIndex = 229;
            this.OrganizeData_modifySubstring_cbLabel.Text = "Substring(s) in Name";
            // 
            // OrganizeData_modifyEntryType_cbLabel
            // 
            this.OrganizeData_modifyEntryType_cbLabel.AutoSize = true;
            this.OrganizeData_modifyEntryType_cbLabel.Location = new System.Drawing.Point(20, 42);
            this.OrganizeData_modifyEntryType_cbLabel.Name = "OrganizeData_modifyEntryType_cbLabel";
            this.OrganizeData_modifyEntryType_cbLabel.Size = new System.Drawing.Size(59, 16);
            this.OrganizeData_modifyEntryType_cbLabel.TabIndex = 228;
            this.OrganizeData_modifyEntryType_cbLabel.Text = "Up/Down";
            // 
            // OrganizeData_modifyTimepoint_cbLabel
            // 
            this.OrganizeData_modifyTimepoint_cbLabel.AutoSize = true;
            this.OrganizeData_modifyTimepoint_cbLabel.Location = new System.Drawing.Point(49, 58);
            this.OrganizeData_modifyTimepoint_cbLabel.Name = "OrganizeData_modifyTimepoint_cbLabel";
            this.OrganizeData_modifyTimepoint_cbLabel.Size = new System.Drawing.Size(63, 16);
            this.OrganizeData_modifyTimepoint_cbLabel.TabIndex = 227;
            this.OrganizeData_modifyTimepoint_cbLabel.Text = "Timepoint";
            // 
            // OrganizeData_modifyName_cbLabel
            // 
            this.OrganizeData_modifyName_cbLabel.AutoSize = true;
            this.OrganizeData_modifyName_cbLabel.Location = new System.Drawing.Point(100, 24);
            this.OrganizeData_modifyName_cbLabel.Name = "OrganizeData_modifyName_cbLabel";
            this.OrganizeData_modifyName_cbLabel.Size = new System.Drawing.Size(41, 16);
            this.OrganizeData_modifyName_cbLabel.TabIndex = 226;
            this.OrganizeData_modifyName_cbLabel.Text = "Name";
            // 
            // OrganizeData_modifySubstring_cbButton
            // 
            this.OrganizeData_modifySubstring_cbButton.Checked = false;
            this.OrganizeData_modifySubstring_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySubstring_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySubstring_cbButton.Location = new System.Drawing.Point(89, 78);
            this.OrganizeData_modifySubstring_cbButton.Name = "OrganizeData_modifySubstring_cbButton";
            this.OrganizeData_modifySubstring_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySubstring_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySubstring_cbButton.Size = new System.Drawing.Size(27, 23);
            this.OrganizeData_modifySubstring_cbButton.TabIndex = 225;
            this.OrganizeData_modifySubstring_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_modifySubstring_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_modifySubstring_cbButton.Click += new System.EventHandler(this.OrganizeData_modifySubstring_cbButton_Click);
            // 
            // OrganizeData_modifySourceFileName_cbButton
            // 
            this.OrganizeData_modifySourceFileName_cbButton.Checked = false;
            this.OrganizeData_modifySourceFileName_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySourceFileName_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySourceFileName_cbButton.Location = new System.Drawing.Point(123, 32);
            this.OrganizeData_modifySourceFileName_cbButton.Name = "OrganizeData_modifySourceFileName_cbButton";
            this.OrganizeData_modifySourceFileName_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySourceFileName_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifySourceFileName_cbButton.Size = new System.Drawing.Size(23, 23);
            this.OrganizeData_modifySourceFileName_cbButton.TabIndex = 224;
            this.OrganizeData_modifySourceFileName_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_modifySourceFileName_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_modifySourceFileName_cbButton.Click += new System.EventHandler(this.OrganizeData_modifySourceFileName_cbButton_Click);
            // 
            // OrganizeData_modifyEntryType_cbButton
            // 
            this.OrganizeData_modifyEntryType_cbButton.Checked = false;
            this.OrganizeData_modifyEntryType_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyEntryType_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyEntryType_cbButton.Location = new System.Drawing.Point(237, 25);
            this.OrganizeData_modifyEntryType_cbButton.Name = "OrganizeData_modifyEntryType_cbButton";
            this.OrganizeData_modifyEntryType_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyEntryType_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyEntryType_cbButton.Size = new System.Drawing.Size(75, 23);
            this.OrganizeData_modifyEntryType_cbButton.TabIndex = 223;
            this.OrganizeData_modifyEntryType_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_modifyEntryType_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_modifyEntryType_cbButton.Click += new System.EventHandler(this.OrganizeData_modifyEntryType_cbButton_Click);
            // 
            // OrganizeData_modifyTimepoint_cbButton
            // 
            this.OrganizeData_modifyTimepoint_cbButton.Checked = false;
            this.OrganizeData_modifyTimepoint_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyTimepoint_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyTimepoint_cbButton.Location = new System.Drawing.Point(124, 84);
            this.OrganizeData_modifyTimepoint_cbButton.Name = "OrganizeData_modifyTimepoint_cbButton";
            this.OrganizeData_modifyTimepoint_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyTimepoint_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyTimepoint_cbButton.Size = new System.Drawing.Size(21, 23);
            this.OrganizeData_modifyTimepoint_cbButton.TabIndex = 222;
            this.OrganizeData_modifyTimepoint_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_modifyTimepoint_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_modifyTimepoint_cbButton.Click += new System.EventHandler(this.OrganizeData_modifyTimepoint_cbButton_Click);
            // 
            // OrganizeData_modifyName_cbButton
            // 
            this.OrganizeData_modifyName_cbButton.Checked = false;
            this.OrganizeData_modifyName_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyName_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyName_cbButton.Location = new System.Drawing.Point(108, 38);
            this.OrganizeData_modifyName_cbButton.Name = "OrganizeData_modifyName_cbButton";
            this.OrganizeData_modifyName_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyName_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.OrganizeData_modifyName_cbButton.Size = new System.Drawing.Size(27, 23);
            this.OrganizeData_modifyName_cbButton.TabIndex = 221;
            this.OrganizeData_modifyName_cbButton.Text = "MyCheckBox_button1";
            this.OrganizeData_modifyName_cbButton.UseVisualStyleBackColor = true;
            this.OrganizeData_modifyName_cbButton.Click += new System.EventHandler(this.OrganizeData_modifyName_cbButton_Click);
            // 
            // OrganizeData_modifySubstringOptions_panel
            // 
            this.OrganizeData_modifySubstringOptions_panel.Border_color = System.Drawing.Color.Transparent;
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyDelimiter_ownTextBox);
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyIndexLeft_label);
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyDelimiter_label);
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyIndexLeft_ownTextBox);
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyIndexRight_label);
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyIndexes_label);
            this.OrganizeData_modifySubstringOptions_panel.Controls.Add(this.OrganizeData_modifyIndexRight_ownTextBox);
            this.OrganizeData_modifySubstringOptions_panel.Corner_radius = 10F;
            this.OrganizeData_modifySubstringOptions_panel.Fill_color = System.Drawing.Color.Transparent;
            this.OrganizeData_modifySubstringOptions_panel.Location = new System.Drawing.Point(162, 46);
            this.OrganizeData_modifySubstringOptions_panel.Name = "OrganizeData_modifySubstringOptions_panel";
            this.OrganizeData_modifySubstringOptions_panel.Size = new System.Drawing.Size(181, 76);
            this.OrganizeData_modifySubstringOptions_panel.TabIndex = 220;
            // 
            // OrganizeData_modifyDelimiter_ownTextBox
            // 
            this.OrganizeData_modifyDelimiter_ownTextBox.Location = new System.Drawing.Point(156, 2);
            this.OrganizeData_modifyDelimiter_ownTextBox.Name = "OrganizeData_modifyDelimiter_ownTextBox";
            this.OrganizeData_modifyDelimiter_ownTextBox.Size = new System.Drawing.Size(22, 22);
            this.OrganizeData_modifyDelimiter_ownTextBox.TabIndex = 211;
            // 
            // OrganizeData_modifyIndexLeft_label
            // 
            this.OrganizeData_modifyIndexLeft_label.AutoSize = true;
            this.OrganizeData_modifyIndexLeft_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_modifyIndexLeft_label.Location = new System.Drawing.Point(10, 53);
            this.OrganizeData_modifyIndexLeft_label.Name = "OrganizeData_modifyIndexLeft_label";
            this.OrganizeData_modifyIndexLeft_label.Size = new System.Drawing.Size(39, 19);
            this.OrganizeData_modifyIndexLeft_label.TabIndex = 218;
            this.OrganizeData_modifyIndexLeft_label.Text = "left:";
            // 
            // OrganizeData_modifyDelimiter_label
            // 
            this.OrganizeData_modifyDelimiter_label.AutoSize = true;
            this.OrganizeData_modifyDelimiter_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_modifyDelimiter_label.Location = new System.Drawing.Point(-6, 7);
            this.OrganizeData_modifyDelimiter_label.Name = "OrganizeData_modifyDelimiter_label";
            this.OrganizeData_modifyDelimiter_label.Size = new System.Drawing.Size(164, 19);
            this.OrganizeData_modifyDelimiter_label.TabIndex = 212;
            this.OrganizeData_modifyDelimiter_label.Text = "Substring delimiter:";
            // 
            // OrganizeData_modifyIndexLeft_ownTextBox
            // 
            this.OrganizeData_modifyIndexLeft_ownTextBox.Location = new System.Drawing.Point(50, 43);
            this.OrganizeData_modifyIndexLeft_ownTextBox.Name = "OrganizeData_modifyIndexLeft_ownTextBox";
            this.OrganizeData_modifyIndexLeft_ownTextBox.Size = new System.Drawing.Size(27, 22);
            this.OrganizeData_modifyIndexLeft_ownTextBox.TabIndex = 213;
            this.OrganizeData_modifyIndexLeft_ownTextBox.TextChanged += new System.EventHandler(this.OrganizeData_modifyIndexLeft_ownTextBox_TextChanged);
            // 
            // OrganizeData_modifyIndexRight_label
            // 
            this.OrganizeData_modifyIndexRight_label.AutoSize = true;
            this.OrganizeData_modifyIndexRight_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_modifyIndexRight_label.Location = new System.Drawing.Point(77, 53);
            this.OrganizeData_modifyIndexRight_label.Name = "OrganizeData_modifyIndexRight_label";
            this.OrganizeData_modifyIndexRight_label.Size = new System.Drawing.Size(74, 19);
            this.OrganizeData_modifyIndexRight_label.TabIndex = 216;
            this.OrganizeData_modifyIndexRight_label.Text = "or right:";
            // 
            // OrganizeData_modifyIndexes_label
            // 
            this.OrganizeData_modifyIndexes_label.AutoSize = true;
            this.OrganizeData_modifyIndexes_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_modifyIndexes_label.Location = new System.Drawing.Point(9, 32);
            this.OrganizeData_modifyIndexes_label.Name = "OrganizeData_modifyIndexes_label";
            this.OrganizeData_modifyIndexes_label.Size = new System.Drawing.Size(173, 19);
            this.OrganizeData_modifyIndexes_label.TabIndex = 214;
            this.OrganizeData_modifyIndexes_label.Text = "Substring no(s) from";
            // 
            // OrganizeData_modifyIndexRight_ownTextBox
            // 
            this.OrganizeData_modifyIndexRight_ownTextBox.Location = new System.Drawing.Point(151, 43);
            this.OrganizeData_modifyIndexRight_ownTextBox.Name = "OrganizeData_modifyIndexRight_ownTextBox";
            this.OrganizeData_modifyIndexRight_ownTextBox.Size = new System.Drawing.Size(27, 22);
            this.OrganizeData_modifyIndexRight_ownTextBox.TabIndex = 215;
            this.OrganizeData_modifyIndexRight_ownTextBox.TextChanged += new System.EventHandler(this.OrganizeData_modifyIndexRight_ownTextBox_TextChanged);
            // 
            // OrganizeData_changeDelete_button
            // 
            this.OrganizeData_changeDelete_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_changeDelete_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_changeDelete_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_changeDelete_button.Location = new System.Drawing.Point(231, 118);
            this.OrganizeData_changeDelete_button.Name = "OrganizeData_changeDelete_button";
            this.OrganizeData_changeDelete_button.Size = new System.Drawing.Size(110, 50);
            this.OrganizeData_changeDelete_button.TabIndex = 218;
            this.OrganizeData_changeDelete_button.Text = "Mark for\r\nDeletion";
            this.OrganizeData_changeDelete_button.UseVisualStyleBackColor = false;
            this.OrganizeData_changeDelete_button.Click += new System.EventHandler(this.OrganizeData_changeDelete_button_Click);
            // 
            // OrganizeData_changeIntegrationGroup_button
            // 
            this.OrganizeData_changeIntegrationGroup_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_changeIntegrationGroup_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_changeIntegrationGroup_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_changeIntegrationGroup_button.Location = new System.Drawing.Point(8, 118);
            this.OrganizeData_changeIntegrationGroup_button.Name = "OrganizeData_changeIntegrationGroup_button";
            this.OrganizeData_changeIntegrationGroup_button.Size = new System.Drawing.Size(110, 50);
            this.OrganizeData_changeIntegrationGroup_button.TabIndex = 202;
            this.OrganizeData_changeIntegrationGroup_button.Text = "Modify\r\nInt. Groups";
            this.OrganizeData_changeIntegrationGroup_button.UseVisualStyleBackColor = false;
            this.OrganizeData_changeIntegrationGroup_button.Click += new System.EventHandler(this.OrganizeData_changeIntegrationGroup_button_Click);
            // 
            // OrganizeData_changeColor_button
            // 
            this.OrganizeData_changeColor_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.OrganizeData_changeColor_button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_changeColor_button.ForeColor = System.Drawing.Color.White;
            this.OrganizeData_changeColor_button.Location = new System.Drawing.Point(120, 118);
            this.OrganizeData_changeColor_button.Name = "OrganizeData_changeColor_button";
            this.OrganizeData_changeColor_button.Size = new System.Drawing.Size(110, 50);
            this.OrganizeData_changeColor_button.TabIndex = 198;
            this.OrganizeData_changeColor_button.Text = "Modify Colors";
            this.OrganizeData_changeColor_button.UseVisualStyleBackColor = false;
            this.OrganizeData_changeColor_button.Click += new System.EventHandler(this.OrganizeData_changeColor_button_Click);
            // 
            // OrganizeData_modifyHeadline_label
            // 
            this.OrganizeData_modifyHeadline_label.AutoSize = true;
            this.OrganizeData_modifyHeadline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.OrganizeData_modifyHeadline_label.Location = new System.Drawing.Point(3, 3);
            this.OrganizeData_modifyHeadline_label.Name = "OrganizeData_modifyHeadline_label";
            this.OrganizeData_modifyHeadline_label.Size = new System.Drawing.Size(268, 19);
            this.OrganizeData_modifyHeadline_label.TabIndex = 0;
            this.OrganizeData_modifyHeadline_label.Text = "Modify all datasets with the same";
            // 
            // AppSize_panel
            // 
            this.AppSize_panel.Border_color = System.Drawing.Color.Transparent;
            this.AppSize_panel.Controls.Add(this.AppSize_width_percent_label);
            this.AppSize_panel.Controls.Add(this.AppSize_heightPercent_label);
            this.AppSize_panel.Controls.Add(this.AppSize_decrease_button);
            this.AppSize_panel.Controls.Add(this.AppSize_increase_button);
            this.AppSize_panel.Controls.Add(this.AppSize_headline_label);
            this.AppSize_panel.Controls.Add(this.AppSize_colorTheme_label);
            this.AppSize_panel.Controls.Add(this.AppSize_colorTheme_listBox);
            this.AppSize_panel.Controls.Add(this.AppSize_resize_button);
            this.AppSize_panel.Controls.Add(this.AppSize_width_textBox);
            this.AppSize_panel.Controls.Add(this.AppSize_height_label);
            this.AppSize_panel.Controls.Add(this.AppSize_height_textBox);
            this.AppSize_panel.Controls.Add(this.AppSize_width_label);
            this.AppSize_panel.Corner_radius = 10F;
            this.AppSize_panel.Fill_color = System.Drawing.Color.Transparent;
            this.AppSize_panel.Location = new System.Drawing.Point(15, 45);
            this.AppSize_panel.Name = "AppSize_panel";
            this.AppSize_panel.Size = new System.Drawing.Size(200, 54);
            this.AppSize_panel.TabIndex = 260;
            // 
            // AppSize_width_percent_label
            // 
            this.AppSize_width_percent_label.AutoSize = true;
            this.AppSize_width_percent_label.Location = new System.Drawing.Point(94, 9);
            this.AppSize_width_percent_label.Name = "AppSize_width_percent_label";
            this.AppSize_width_percent_label.Size = new System.Drawing.Size(19, 16);
            this.AppSize_width_percent_label.TabIndex = 275;
            this.AppSize_width_percent_label.Text = "%";
            // 
            // AppSize_heightPercent_label
            // 
            this.AppSize_heightPercent_label.AutoSize = true;
            this.AppSize_heightPercent_label.Location = new System.Drawing.Point(78, 19);
            this.AppSize_heightPercent_label.Name = "AppSize_heightPercent_label";
            this.AppSize_heightPercent_label.Size = new System.Drawing.Size(19, 16);
            this.AppSize_heightPercent_label.TabIndex = 274;
            this.AppSize_heightPercent_label.Text = "%";
            // 
            // AppSize_decrease_button
            // 
            this.AppSize_decrease_button.Location = new System.Drawing.Point(84, 16);
            this.AppSize_decrease_button.Name = "AppSize_decrease_button";
            this.AppSize_decrease_button.Size = new System.Drawing.Size(32, 23);
            this.AppSize_decrease_button.TabIndex = 273;
            this.AppSize_decrease_button.Text = "Down";
            this.AppSize_decrease_button.UseVisualStyleBackColor = true;
            this.AppSize_decrease_button.Click += new System.EventHandler(this.AppSize_decrease_button_Click);
            // 
            // AppSize_increase_button
            // 
            this.AppSize_increase_button.Location = new System.Drawing.Point(30, 12);
            this.AppSize_increase_button.Name = "AppSize_increase_button";
            this.AppSize_increase_button.Size = new System.Drawing.Size(32, 23);
            this.AppSize_increase_button.TabIndex = 272;
            this.AppSize_increase_button.Text = "Up";
            this.AppSize_increase_button.UseVisualStyleBackColor = true;
            this.AppSize_increase_button.Click += new System.EventHandler(this.AppSize_increase_button_Click);
            // 
            // AppSize_headline_label
            // 
            this.AppSize_headline_label.AutoSize = true;
            this.AppSize_headline_label.Location = new System.Drawing.Point(6, 34);
            this.AppSize_headline_label.Name = "AppSize_headline_label";
            this.AppSize_headline_label.Size = new System.Drawing.Size(155, 16);
            this.AppSize_headline_label.TabIndex = 261;
            this.AppSize_headline_label.Text = "Optimize window settings";
            // 
            // AppSize_colorTheme_label
            // 
            this.AppSize_colorTheme_label.AutoSize = true;
            this.AppSize_colorTheme_label.Location = new System.Drawing.Point(80, 19);
            this.AppSize_colorTheme_label.Name = "AppSize_colorTheme_label";
            this.AppSize_colorTheme_label.Size = new System.Drawing.Size(77, 16);
            this.AppSize_colorTheme_label.TabIndex = 262;
            this.AppSize_colorTheme_label.Text = "Color theme";
            // 
            // AppSize_colorTheme_listBox
            // 
            this.AppSize_colorTheme_listBox.FormattingEnabled = true;
            this.AppSize_colorTheme_listBox.ItemHeight = 16;
            this.AppSize_colorTheme_listBox.Location = new System.Drawing.Point(68, 10);
            this.AppSize_colorTheme_listBox.Name = "AppSize_colorTheme_listBox";
            this.AppSize_colorTheme_listBox.ReadOnly = false;
            this.AppSize_colorTheme_listBox.Size = new System.Drawing.Size(34, 4);
            this.AppSize_colorTheme_listBox.TabIndex = 261;
            this.AppSize_colorTheme_listBox.SelectedIndexChanged += new System.EventHandler(this.AppSize_colorTheme_listBox_SelectedIndexChanged);
            // 
            // AppSize_resize_button
            // 
            this.AppSize_resize_button.Location = new System.Drawing.Point(134, 5);
            this.AppSize_resize_button.Name = "AppSize_resize_button";
            this.AppSize_resize_button.Size = new System.Drawing.Size(60, 36);
            this.AppSize_resize_button.TabIndex = 257;
            this.AppSize_resize_button.Text = "Update";
            this.AppSize_resize_button.UseVisualStyleBackColor = true;
            this.AppSize_resize_button.Click += new System.EventHandler(this.AppSize_resize_button_Click);
            // 
            // AppSize_width_textBox
            // 
            this.AppSize_width_textBox.Location = new System.Drawing.Point(61, 20);
            this.AppSize_width_textBox.Name = "AppSize_width_textBox";
            this.AppSize_width_textBox.Size = new System.Drawing.Size(41, 22);
            this.AppSize_width_textBox.TabIndex = 255;
            this.AppSize_width_textBox.TextChanged += new System.EventHandler(this.AppSize_width_textBox_TextChanged);
            // 
            // AppSize_height_label
            // 
            this.AppSize_height_label.AutoSize = true;
            this.AppSize_height_label.Location = new System.Drawing.Point(3, 0);
            this.AppSize_height_label.Name = "AppSize_height_label";
            this.AppSize_height_label.Size = new System.Drawing.Size(44, 16);
            this.AppSize_height_label.TabIndex = 259;
            this.AppSize_height_label.Text = "Height";
            // 
            // AppSize_height_textBox
            // 
            this.AppSize_height_textBox.Location = new System.Drawing.Point(108, 20);
            this.AppSize_height_textBox.Name = "AppSize_height_textBox";
            this.AppSize_height_textBox.Size = new System.Drawing.Size(16, 22);
            this.AppSize_height_textBox.TabIndex = 256;
            this.AppSize_height_textBox.TextChanged += new System.EventHandler(this.AppSize_height_textBox_TextChanged);
            // 
            // AppSize_width_label
            // 
            this.AppSize_width_label.AutoSize = true;
            this.AppSize_width_label.Location = new System.Drawing.Point(8, 23);
            this.AppSize_width_label.Name = "AppSize_width_label";
            this.AppSize_width_label.Size = new System.Drawing.Size(41, 16);
            this.AppSize_width_label.TabIndex = 258;
            this.AppSize_width_label.Text = "Width";
            // 
            // ResultsDirectory_textBox
            // 
            this.ResultsDirectory_textBox.Location = new System.Drawing.Point(15, 659);
            this.ResultsDirectory_textBox.Name = "ResultsDirectory_textBox";
            this.ResultsDirectory_textBox.Size = new System.Drawing.Size(1051, 22);
            this.ResultsDirectory_textBox.TabIndex = 254;
            // 
            // DatasetInterface_overall_panel
            // 
            this.DatasetInterface_overall_panel.Border_color = System.Drawing.Color.Transparent;
            this.DatasetInterface_overall_panel.Controls.Add(this.DatasetsCount_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Source_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.AnalyzeData_button);
            this.DatasetInterface_overall_panel.Controls.Add(this.EntryType_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.BgGenes_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.GeneCounts_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.IntegrationGroup_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Color_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Substring_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Delete_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Timeline_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Name_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.DatasetOrderNo_panel);
            this.DatasetInterface_overall_panel.Controls.Add(this.Input_geneList_textBox);
            this.DatasetInterface_overall_panel.Controls.Add(this.Abort_button);
            this.DatasetInterface_overall_panel.Controls.Add(this.Input_geneList_label);
            this.DatasetInterface_overall_panel.Controls.Add(this.CompatibilityInfos_label);
            this.DatasetInterface_overall_panel.Controls.Add(this.Dataset_scrollBar);
            this.DatasetInterface_overall_panel.Controls.Add(this.Changes_reset_button);
            this.DatasetInterface_overall_panel.Controls.Add(this.Changes_update_button);
            this.DatasetInterface_overall_panel.Controls.Add(this.ClearCustomData_button);
            this.DatasetInterface_overall_panel.Controls.Add(this.AddNewDataset_button);
            this.DatasetInterface_overall_panel.Corner_radius = 10F;
            this.DatasetInterface_overall_panel.Fill_color = System.Drawing.Color.Transparent;
            this.DatasetInterface_overall_panel.Location = new System.Drawing.Point(15, 104);
            this.DatasetInterface_overall_panel.Name = "DatasetInterface_overall_panel";
            this.DatasetInterface_overall_panel.Size = new System.Drawing.Size(889, 476);
            this.DatasetInterface_overall_panel.TabIndex = 0;
            // 
            // DatasetsCount_panel
            // 
            this.DatasetsCount_panel.Border_color = System.Drawing.Color.Transparent;
            this.DatasetsCount_panel.Corner_radius = 10F;
            this.DatasetsCount_panel.Fill_color = System.Drawing.Color.Transparent;
            this.DatasetsCount_panel.Location = new System.Drawing.Point(742, 23);
            this.DatasetsCount_panel.Name = "DatasetsCount_panel";
            this.DatasetsCount_panel.Size = new System.Drawing.Size(110, 370);
            this.DatasetsCount_panel.TabIndex = 222;
            // 
            // Source_panel
            // 
            this.Source_panel.Border_color = System.Drawing.Color.Transparent;
            this.Source_panel.Controls.Add(this.Source_label);
            this.Source_panel.Controls.Add(this.Source_sortBy_button);
            this.Source_panel.Corner_radius = 10F;
            this.Source_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Source_panel.Location = new System.Drawing.Point(837, 14);
            this.Source_panel.Name = "Source_panel";
            this.Source_panel.Size = new System.Drawing.Size(230, 370);
            this.Source_panel.TabIndex = 222;
            // 
            // Source_label
            // 
            this.Source_label.AutoSize = true;
            this.Source_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Source_label.Location = new System.Drawing.Point(2, 25);
            this.Source_label.Name = "Source_label";
            this.Source_label.Size = new System.Drawing.Size(65, 19);
            this.Source_label.TabIndex = 7;
            this.Source_label.Text = "Source";
            // 
            // Source_sortBy_button
            // 
            this.Source_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.Source_sortBy_button.Location = new System.Drawing.Point(210, 23);
            this.Source_sortBy_button.Name = "Source_sortBy_button";
            this.Source_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.Source_sortBy_button.TabIndex = 160;
            this.Source_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // AnalyzeData_button
            // 
            this.AnalyzeData_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.AnalyzeData_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.AnalyzeData_button.ForeColor = System.Drawing.Color.White;
            this.AnalyzeData_button.Location = new System.Drawing.Point(601, 421);
            this.AnalyzeData_button.Name = "AnalyzeData_button";
            this.AnalyzeData_button.Size = new System.Drawing.Size(140, 30);
            this.AnalyzeData_button.TabIndex = 9;
            this.AnalyzeData_button.Text = "Analyze";
            this.AnalyzeData_button.UseVisualStyleBackColor = false;
            this.AnalyzeData_button.Click += new System.EventHandler(this.AnalyzeData_button_Click);
            // 
            // EntryType_panel
            // 
            this.EntryType_panel.Border_color = System.Drawing.Color.Transparent;
            this.EntryType_panel.Controls.Add(this.EntryType_sortBy_button);
            this.EntryType_panel.Controls.Add(this.EntryType_label);
            this.EntryType_panel.Corner_radius = 10F;
            this.EntryType_panel.Fill_color = System.Drawing.Color.Transparent;
            this.EntryType_panel.Location = new System.Drawing.Point(330, 37);
            this.EntryType_panel.Name = "EntryType_panel";
            this.EntryType_panel.Size = new System.Drawing.Size(76, 370);
            this.EntryType_panel.TabIndex = 147;
            // 
            // EntryType_sortBy_button
            // 
            this.EntryType_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.EntryType_sortBy_button.Location = new System.Drawing.Point(56, 29);
            this.EntryType_sortBy_button.Name = "EntryType_sortBy_button";
            this.EntryType_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.EntryType_sortBy_button.TabIndex = 133;
            this.EntryType_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // EntryType_label
            // 
            this.EntryType_label.AutoSize = true;
            this.EntryType_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.EntryType_label.Location = new System.Drawing.Point(3, 11);
            this.EntryType_label.Name = "EntryType_label";
            this.EntryType_label.Size = new System.Drawing.Size(79, 19);
            this.EntryType_label.TabIndex = 62;
            this.EntryType_label.Text = "Up/down";
            // 
            // BgGenes_panel
            // 
            this.BgGenes_panel.Border_color = System.Drawing.Color.Transparent;
            this.BgGenes_panel.Controls.Add(this.BgGenes_sortBy_button);
            this.BgGenes_panel.Controls.Add(this.BgGenes_label);
            this.BgGenes_panel.Corner_radius = 10F;
            this.BgGenes_panel.Fill_color = System.Drawing.Color.Transparent;
            this.BgGenes_panel.Location = new System.Drawing.Point(742, 98);
            this.BgGenes_panel.Name = "BgGenes_panel";
            this.BgGenes_panel.Size = new System.Drawing.Size(300, 370);
            this.BgGenes_panel.TabIndex = 223;
            // 
            // BgGenes_sortBy_button
            // 
            this.BgGenes_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.BgGenes_sortBy_button.Location = new System.Drawing.Point(278, 15);
            this.BgGenes_sortBy_button.Name = "BgGenes_sortBy_button";
            this.BgGenes_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.BgGenes_sortBy_button.TabIndex = 120;
            this.BgGenes_sortBy_button.UseVisualStyleBackColor = true;
            this.BgGenes_sortBy_button.Click += new System.EventHandler(this.SortBy_bgGenesListName_button_Click);
            // 
            // BgGenes_label
            // 
            this.BgGenes_label.AutoSize = true;
            this.BgGenes_label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.BgGenes_label.Location = new System.Drawing.Point(4, 17);
            this.BgGenes_label.Name = "BgGenes_label";
            this.BgGenes_label.Size = new System.Drawing.Size(142, 18);
            this.BgGenes_label.TabIndex = 49;
            this.BgGenes_label.Text = "Background genes";
            // 
            // GeneCounts_panel
            // 
            this.GeneCounts_panel.Border_color = System.Drawing.Color.Transparent;
            this.GeneCounts_panel.Corner_radius = 10F;
            this.GeneCounts_panel.Fill_color = System.Drawing.Color.Transparent;
            this.GeneCounts_panel.Location = new System.Drawing.Point(770, 26);
            this.GeneCounts_panel.Name = "GeneCounts_panel";
            this.GeneCounts_panel.Size = new System.Drawing.Size(166, 370);
            this.GeneCounts_panel.TabIndex = 224;
            // 
            // IntegrationGroup_panel
            // 
            this.IntegrationGroup_panel.Border_color = System.Drawing.Color.Transparent;
            this.IntegrationGroup_panel.Controls.Add(this.IntegrationGroup_sortBy_button);
            this.IntegrationGroup_panel.Controls.Add(this.IntegrationGroup_label);
            this.IntegrationGroup_panel.Corner_radius = 10F;
            this.IntegrationGroup_panel.Fill_color = System.Drawing.Color.Transparent;
            this.IntegrationGroup_panel.Location = new System.Drawing.Point(199, 14);
            this.IntegrationGroup_panel.Name = "IntegrationGroup_panel";
            this.IntegrationGroup_panel.Size = new System.Drawing.Size(170, 370);
            this.IntegrationGroup_panel.TabIndex = 222;
            // 
            // IntegrationGroup_sortBy_button
            // 
            this.IntegrationGroup_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.IntegrationGroup_sortBy_button.Location = new System.Drawing.Point(147, 20);
            this.IntegrationGroup_sortBy_button.Name = "IntegrationGroup_sortBy_button";
            this.IntegrationGroup_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.IntegrationGroup_sortBy_button.TabIndex = 134;
            this.IntegrationGroup_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // IntegrationGroup_label
            // 
            this.IntegrationGroup_label.AutoSize = true;
            this.IntegrationGroup_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.IntegrationGroup_label.Location = new System.Drawing.Point(3, 22);
            this.IntegrationGroup_label.Name = "IntegrationGroup_label";
            this.IntegrationGroup_label.Size = new System.Drawing.Size(146, 19);
            this.IntegrationGroup_label.TabIndex = 133;
            this.IntegrationGroup_label.Text = "Integration group";
            // 
            // Color_panel
            // 
            this.Color_panel.Border_color = System.Drawing.Color.Transparent;
            this.Color_panel.Controls.Add(this.Color_sortBy_button);
            this.Color_panel.Controls.Add(this.Color_label);
            this.Color_panel.Corner_radius = 10F;
            this.Color_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Color_panel.Location = new System.Drawing.Point(143, 11);
            this.Color_panel.Name = "Color_panel";
            this.Color_panel.Size = new System.Drawing.Size(126, 370);
            this.Color_panel.TabIndex = 227;
            // 
            // Color_sortBy_button
            // 
            this.Color_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.Color_sortBy_button.Location = new System.Drawing.Point(104, 21);
            this.Color_sortBy_button.Name = "Color_sortBy_button";
            this.Color_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.Color_sortBy_button.TabIndex = 120;
            this.Color_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // Color_label
            // 
            this.Color_label.AutoSize = true;
            this.Color_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Color_label.Location = new System.Drawing.Point(3, 22);
            this.Color_label.Name = "Color_label";
            this.Color_label.Size = new System.Drawing.Size(52, 19);
            this.Color_label.TabIndex = 49;
            this.Color_label.Text = "Color";
            // 
            // Substring_panel
            // 
            this.Substring_panel.Border_color = System.Drawing.Color.Transparent;
            this.Substring_panel.Controls.Add(this.Substring_label);
            this.Substring_panel.Controls.Add(this.Substring_sortBy_button);
            this.Substring_panel.Corner_radius = 10F;
            this.Substring_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Substring_panel.Location = new System.Drawing.Point(570, 3);
            this.Substring_panel.Name = "Substring_panel";
            this.Substring_panel.Size = new System.Drawing.Size(166, 370);
            this.Substring_panel.TabIndex = 225;
            // 
            // Substring_label
            // 
            this.Substring_label.AutoSize = true;
            this.Substring_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Substring_label.Location = new System.Drawing.Point(0, 23);
            this.Substring_label.Name = "Substring_label";
            this.Substring_label.Size = new System.Drawing.Size(86, 19);
            this.Substring_label.TabIndex = 7;
            this.Substring_label.Text = "Substring";
            // 
            // Substring_sortBy_button
            // 
            this.Substring_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.Substring_sortBy_button.Location = new System.Drawing.Point(144, 21);
            this.Substring_sortBy_button.Name = "Substring_sortBy_button";
            this.Substring_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.Substring_sortBy_button.TabIndex = 160;
            this.Substring_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // Delete_panel
            // 
            this.Delete_panel.Border_color = System.Drawing.Color.Transparent;
            this.Delete_panel.Controls.Add(this.Dataset_all_delete_cbButton);
            this.Delete_panel.Corner_radius = 10F;
            this.Delete_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Delete_panel.Location = new System.Drawing.Point(486, 14);
            this.Delete_panel.Name = "Delete_panel";
            this.Delete_panel.Size = new System.Drawing.Size(26, 370);
            this.Delete_panel.TabIndex = 223;
            // 
            // Dataset_all_delete_cbButton
            // 
            this.Dataset_all_delete_cbButton.Checked = false;
            this.Dataset_all_delete_cbButton.Checked_backColor = System.Drawing.Color.Empty;
            this.Dataset_all_delete_cbButton.Checked_foreColor = System.Drawing.Color.Empty;
            this.Dataset_all_delete_cbButton.Location = new System.Drawing.Point(10, 46);
            this.Dataset_all_delete_cbButton.Name = "Dataset_all_delete_cbButton";
            this.Dataset_all_delete_cbButton.NotChecked_backColor = System.Drawing.Color.Empty;
            this.Dataset_all_delete_cbButton.NotChecked_foreColor = System.Drawing.Color.Empty;
            this.Dataset_all_delete_cbButton.Size = new System.Drawing.Size(13, 23);
            this.Dataset_all_delete_cbButton.TabIndex = 161;
            this.Dataset_all_delete_cbButton.Text = "myCheckBox_button1";
            this.Dataset_all_delete_cbButton.UseVisualStyleBackColor = true;
            this.Dataset_all_delete_cbButton.Click += new System.EventHandler(this.Dataset_all_delete_cbButton_Click);
            // 
            // Timeline_panel
            // 
            this.Timeline_panel.Border_color = System.Drawing.Color.Transparent;
            this.Timeline_panel.Controls.Add(this.Timeline_label);
            this.Timeline_panel.Controls.Add(this.Timeline_sortBy_button);
            this.Timeline_panel.Corner_radius = 10F;
            this.Timeline_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Timeline_panel.Location = new System.Drawing.Point(255, 66);
            this.Timeline_panel.Name = "Timeline_panel";
            this.Timeline_panel.Size = new System.Drawing.Size(103, 370);
            this.Timeline_panel.TabIndex = 222;
            // 
            // Timeline_label
            // 
            this.Timeline_label.AutoSize = true;
            this.Timeline_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Timeline_label.Location = new System.Drawing.Point(-2, 20);
            this.Timeline_label.Name = "Timeline_label";
            this.Timeline_label.Size = new System.Drawing.Size(86, 19);
            this.Timeline_label.TabIndex = 50;
            this.Timeline_label.Text = "Timepoint";
            // 
            // Timeline_sortBy_button
            // 
            this.Timeline_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.Timeline_sortBy_button.Location = new System.Drawing.Point(81, 20);
            this.Timeline_sortBy_button.Name = "Timeline_sortBy_button";
            this.Timeline_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.Timeline_sortBy_button.TabIndex = 82;
            this.Timeline_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // Name_panel
            // 
            this.Name_panel.Border_color = System.Drawing.Color.Transparent;
            this.Name_panel.Controls.Add(this.Name_label);
            this.Name_panel.Controls.Add(this.Name_sortBy_button);
            this.Name_panel.Corner_radius = 10F;
            this.Name_panel.Fill_color = System.Drawing.Color.Transparent;
            this.Name_panel.Location = new System.Drawing.Point(210, 115);
            this.Name_panel.Name = "Name_panel";
            this.Name_panel.Size = new System.Drawing.Size(166, 370);
            this.Name_panel.TabIndex = 222;
            // 
            // Name_label
            // 
            this.Name_label.AutoSize = true;
            this.Name_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Name_label.Location = new System.Drawing.Point(3, 17);
            this.Name_label.Name = "Name_label";
            this.Name_label.Size = new System.Drawing.Size(53, 19);
            this.Name_label.TabIndex = 7;
            this.Name_label.Text = "Name";
            // 
            // Name_sortBy_button
            // 
            this.Name_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.Name_sortBy_button.Location = new System.Drawing.Point(146, 15);
            this.Name_sortBy_button.Name = "Name_sortBy_button";
            this.Name_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.Name_sortBy_button.TabIndex = 160;
            this.Name_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // DatasetOrderNo_panel
            // 
            this.DatasetOrderNo_panel.Border_color = System.Drawing.Color.Transparent;
            this.DatasetOrderNo_panel.Controls.Add(this.DatasetOrderNo_label);
            this.DatasetOrderNo_panel.Controls.Add(this.DatasetOrderNo_sortBy_button);
            this.DatasetOrderNo_panel.Corner_radius = 10F;
            this.DatasetOrderNo_panel.Fill_color = System.Drawing.Color.Transparent;
            this.DatasetOrderNo_panel.Location = new System.Drawing.Point(537, 20);
            this.DatasetOrderNo_panel.Name = "DatasetOrderNo_panel";
            this.DatasetOrderNo_panel.Size = new System.Drawing.Size(53, 370);
            this.DatasetOrderNo_panel.TabIndex = 222;
            // 
            // DatasetOrderNo_label
            // 
            this.DatasetOrderNo_label.AutoSize = true;
            this.DatasetOrderNo_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.DatasetOrderNo_label.Location = new System.Drawing.Point(3, 23);
            this.DatasetOrderNo_label.Name = "DatasetOrderNo_label";
            this.DatasetOrderNo_label.Size = new System.Drawing.Size(31, 19);
            this.DatasetOrderNo_label.TabIndex = 7;
            this.DatasetOrderNo_label.Text = "No";
            // 
            // DatasetOrderNo_sortBy_button
            // 
            this.DatasetOrderNo_sortBy_button.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold);
            this.DatasetOrderNo_sortBy_button.Location = new System.Drawing.Point(33, 21);
            this.DatasetOrderNo_sortBy_button.Name = "DatasetOrderNo_sortBy_button";
            this.DatasetOrderNo_sortBy_button.Size = new System.Drawing.Size(20, 20);
            this.DatasetOrderNo_sortBy_button.TabIndex = 160;
            this.DatasetOrderNo_sortBy_button.UseVisualStyleBackColor = true;
            // 
            // Input_geneList_textBox
            // 
            this.Input_geneList_textBox.Location = new System.Drawing.Point(12, 45);
            this.Input_geneList_textBox.Multiline = true;
            this.Input_geneList_textBox.Name = "Input_geneList_textBox";
            this.Input_geneList_textBox.Size = new System.Drawing.Size(142, 431);
            this.Input_geneList_textBox.TabIndex = 221;
            // 
            // Abort_button
            // 
            this.Abort_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Abort_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Abort_button.ForeColor = System.Drawing.Color.White;
            this.Abort_button.Location = new System.Drawing.Point(611, 446);
            this.Abort_button.Name = "Abort_button";
            this.Abort_button.Size = new System.Drawing.Size(140, 30);
            this.Abort_button.TabIndex = 220;
            this.Abort_button.Text = "Abort";
            this.Abort_button.UseVisualStyleBackColor = false;
            // 
            // Input_geneList_label
            // 
            this.Input_geneList_label.AutoSize = true;
            this.Input_geneList_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.Input_geneList_label.Location = new System.Drawing.Point(8, 23);
            this.Input_geneList_label.Name = "Input_geneList_label";
            this.Input_geneList_label.Size = new System.Drawing.Size(78, 19);
            this.Input_geneList_label.TabIndex = 5;
            this.Input_geneList_label.Text = "Gene list";
            // 
            // CompatibilityInfos_label
            // 
            this.CompatibilityInfos_label.AutoSize = true;
            this.CompatibilityInfos_label.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.CompatibilityInfos_label.Location = new System.Drawing.Point(215, 377);
            this.CompatibilityInfos_label.Name = "CompatibilityInfos_label";
            this.CompatibilityInfos_label.Size = new System.Drawing.Size(154, 19);
            this.CompatibilityInfos_label.TabIndex = 96;
            this.CompatibilityInfos_label.Text = "Compatibility infos";
            // 
            // Dataset_scrollBar
            // 
            this.Dataset_scrollBar.Location = new System.Drawing.Point(182, 42);
            this.Dataset_scrollBar.Name = "Dataset_scrollBar";
            this.Dataset_scrollBar.Size = new System.Drawing.Size(16, 297);
            this.Dataset_scrollBar.TabIndex = 94;
            this.Dataset_scrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Dataset_scrollBar_Scroll);
            // 
            // Changes_reset_button
            // 
            this.Changes_reset_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Changes_reset_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Changes_reset_button.ForeColor = System.Drawing.Color.White;
            this.Changes_reset_button.Location = new System.Drawing.Point(455, 398);
            this.Changes_reset_button.Name = "Changes_reset_button";
            this.Changes_reset_button.Size = new System.Drawing.Size(181, 31);
            this.Changes_reset_button.TabIndex = 80;
            this.Changes_reset_button.Text = "Reset changes";
            this.Changes_reset_button.UseVisualStyleBackColor = false;
            this.Changes_reset_button.Visible = false;
            this.Changes_reset_button.Click += new System.EventHandler(this.Changes_reset_button_Click);
            // 
            // Changes_update_button
            // 
            this.Changes_update_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Changes_update_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Changes_update_button.ForeColor = System.Drawing.Color.White;
            this.Changes_update_button.Location = new System.Drawing.Point(273, 398);
            this.Changes_update_button.Name = "Changes_update_button";
            this.Changes_update_button.Size = new System.Drawing.Size(176, 31);
            this.Changes_update_button.TabIndex = 79;
            this.Changes_update_button.Text = "Update changes";
            this.Changes_update_button.UseVisualStyleBackColor = false;
            this.Changes_update_button.Click += new System.EventHandler(this.Changes_update_button_Click);
            // 
            // ClearCustomData_button
            // 
            this.ClearCustomData_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.ClearCustomData_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.ClearCustomData_button.ForeColor = System.Drawing.Color.White;
            this.ClearCustomData_button.Location = new System.Drawing.Point(455, 430);
            this.ClearCustomData_button.Name = "ClearCustomData_button";
            this.ClearCustomData_button.Size = new System.Drawing.Size(140, 30);
            this.ClearCustomData_button.TabIndex = 3;
            this.ClearCustomData_button.Text = "Clear";
            this.ClearCustomData_button.UseVisualStyleBackColor = false;
            this.ClearCustomData_button.Click += new System.EventHandler(this.ClearCustomData_button_Click);
            // 
            // AddNewDataset_button
            // 
            this.AddNewDataset_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.AddNewDataset_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddNewDataset_button.ForeColor = System.Drawing.Color.White;
            this.AddNewDataset_button.Location = new System.Drawing.Point(163, 446);
            this.AddNewDataset_button.Name = "AddNewDataset_button";
            this.AddNewDataset_button.Size = new System.Drawing.Size(140, 30);
            this.AddNewDataset_button.TabIndex = 2;
            this.AddNewDataset_button.Text = "Add dataset";
            this.AddNewDataset_button.UseVisualStyleBackColor = false;
            this.AddNewDataset_button.Click += new System.EventHandler(this.AddNewDataset_button_Click);
            // 
            // Read_directoryOrFile_ownTextBox
            // 
            this.Read_directoryOrFile_ownTextBox.Location = new System.Drawing.Point(14, 702);
            this.Read_directoryOrFile_ownTextBox.Name = "Read_directoryOrFile_ownTextBox";
            this.Read_directoryOrFile_ownTextBox.Size = new System.Drawing.Size(1051, 22);
            this.Read_directoryOrFile_ownTextBox.TabIndex = 167;
            // 
            // Mbco_user_application_form
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1924, 1055);
            this.Controls.Add(this.Options_loadExamples_panel);
            this.Controls.Add(this.Options_selectSCPs_panel);
            this.Controls.Add(this.Options_scpNetworks_panel);
            this.Controls.Add(this.Options_enrichment_panel);
            this.Controls.Add(this.Options_dataSignificance_panel);
            this.Controls.Add(this.Options_results_panel);
            this.Controls.Add(this.Options_results_button);
            this.Controls.Add(this.Results_visualization_panel);
            this.Controls.Add(this.Report_panel);
            this.Controls.Add(this.Options_readData_panel);
            this.Controls.Add(this.Options_defineScps_panel);
            this.Controls.Add(this.Options_tips_panel);
            this.Controls.Add(this.Options_bgGenes_panel);
            this.Controls.Add(this.Options_organizeData_panel);
            this.Controls.Add(this.AppSize_panel);
            this.Controls.Add(this.ResultsDirectory_textBox);
            this.Controls.Add(this.DatasetInterface_overall_panel);
            this.Controls.Add(this.Options_tips_button);
            this.Controls.Add(this.Options_defineSCPs_button);
            this.Controls.Add(this.Options_selectSCPs_button);
            this.Controls.Add(this.Options_dataSignificance_button);
            this.Controls.Add(this.Progress_report_label);
            this.Controls.Add(this.Options_exampleData_button);
            this.Controls.Add(this.Options_organizeData_button);
            this.Controls.Add(this.Options_backgroundGenes_button);
            this.Controls.Add(this.Read_directoryOrFile_ownTextBox);
            this.Controls.Add(this.Options_readData_button);
            this.Controls.Add(this.Options_enrichment_button);
            this.Controls.Add(this.Options_scpNetworks_button);
            this.Controls.Add(this.Read_directoryOrFile_label);
            this.Controls.Add(this.MBCO_reference_label);
            this.Controls.Add(this.ResultsDirectory_label);
            this.Controls.Add(this.MBCO_headline);
            this.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "Mbco_user_application_form";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.Options_loadExamples_panel.ResumeLayout(false);
            this.Options_loadExamples_panel.PerformLayout();
            this.Options_selectSCPs_panel.ResumeLayout(false);
            this.Options_selectSCPs_panel.PerformLayout();
            this.SelectSCPs_selection_panel.ResumeLayout(false);
            this.SelectSCPs_selection_panel.PerformLayout();
            this.Options_scpNetworks_panel.ResumeLayout(false);
            this.Options_scpNetworks_panel.PerformLayout();
            this.ScpNetworks_nodeSize_panel.ResumeLayout(false);
            this.ScpNetworks_nodeSize_panel.PerformLayout();
            this.ScpNetworks_standard_panel.ResumeLayout(false);
            this.ScpNetworks_standard_panel.PerformLayout();
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.ResumeLayout(false);
            this.ScpNetworks_standardConnectScpsTopInteractions_panel.PerformLayout();
            this.ScpNetworks_comments_panel.ResumeLayout(false);
            this.ScpNetworks_comments_panel.PerformLayout();
            this.ScpNetworks_dynamic_panel.ResumeLayout(false);
            this.ScpNetworks_dynamic_panel.PerformLayout();
            this.Options_enrichment_panel.ResumeLayout(false);
            this.EnrichmentOptions_ontology_panel.ResumeLayout(false);
            this.EnrichmentOptions_ontology_panel.PerformLayout();
            this.EnrichmentOptions_defineOutputs_panel.ResumeLayout(false);
            this.EnrichmentOptions_defineOutputs_panel.PerformLayout();
            this.EnrichmentOptions_colors_panel.ResumeLayout(false);
            this.EnrichmentOptions_colors_panel.PerformLayout();
            this.EnrichmentOptions_keepTopSCPs_panel.ResumeLayout(false);
            this.EnrichmentOptions_scpTopInteractions_panel.ResumeLayout(false);
            this.EnrichmentOptions_scpTopInteractions_panel.PerformLayout();
            this.EnrichmentOptions_cutoffs_panel.ResumeLayout(false);
            this.EnrichmentOptions_cutoffs_panel.PerformLayout();
            this.Options_dataSignificance_panel.ResumeLayout(false);
            this.Options_dataSignificance_panel.PerformLayout();
            this.SigData_sigSelection_panel.ResumeLayout(false);
            this.SigData_sigSelection_panel.PerformLayout();
            this.Options_results_panel.ResumeLayout(false);
            this.Options_results_panel.PerformLayout();
            this.Results_controlCommand_panel.ResumeLayout(false);
            this.Results_controlCommand_panel.PerformLayout();
            this.Results_visualization_panel.ResumeLayout(false);
            this.Results_visualization_panel.PerformLayout();
            this.Report_panel.ResumeLayout(false);
            this.Report_panel.PerformLayout();
            this.Options_readData_panel.ResumeLayout(false);
            this.Options_readData_panel.PerformLayout();
            this.Options_defineScps_panel.ResumeLayout(false);
            this.Options_defineScps_panel.PerformLayout();
            this.DefineScps_selection_panel.ResumeLayout(false);
            this.DefineScps_selection_panel.PerformLayout();
            this.Options_tips_panel.ResumeLayout(false);
            this.Options_tips_panel.PerformLayout();
            this.Options_bgGenes_panel.ResumeLayout(false);
            this.Options_bgGenes_panel.PerformLayout();
            this.BgGenes_warnings_panel.ResumeLayout(false);
            this.BgGenes_warnings_panel.PerformLayout();
            this.BgGenes_assignment_panel.ResumeLayout(false);
            this.BgGenes_assignment_panel.PerformLayout();
            this.BgGenes_organize_panel.ResumeLayout(false);
            this.BgGenes_organize_panel.PerformLayout();
            this.BgGenes_add_panel.ResumeLayout(false);
            this.BgGenes_add_panel.PerformLayout();
            this.Options_organizeData_panel.ResumeLayout(false);
            this.OrganizeData_convertTimeunits_panel.ResumeLayout(false);
            this.OrganizeData_convertTimeunits_panel.PerformLayout();
            this.OrganizeData_automatically_panel.ResumeLayout(false);
            this.OrganizeData_automatically_panel.PerformLayout();
            this.OrganizeData_addFileName_panel.ResumeLayout(false);
            this.OrganizeData_addFileName_panel.PerformLayout();
            this.OrganizeData_show_panel.ResumeLayout(false);
            this.OrganizeData_show_panel.PerformLayout();
            this.OrganizeData_modify_panel.ResumeLayout(false);
            this.OrganizeData_modify_panel.PerformLayout();
            this.OrganizeData_modifySubstringOptions_panel.ResumeLayout(false);
            this.OrganizeData_modifySubstringOptions_panel.PerformLayout();
            this.AppSize_panel.ResumeLayout(false);
            this.AppSize_panel.PerformLayout();
            this.DatasetInterface_overall_panel.ResumeLayout(false);
            this.DatasetInterface_overall_panel.PerformLayout();
            this.Source_panel.ResumeLayout(false);
            this.Source_panel.PerformLayout();
            this.EntryType_panel.ResumeLayout(false);
            this.EntryType_panel.PerformLayout();
            this.BgGenes_panel.ResumeLayout(false);
            this.BgGenes_panel.PerformLayout();
            this.IntegrationGroup_panel.ResumeLayout(false);
            this.IntegrationGroup_panel.PerformLayout();
            this.Color_panel.ResumeLayout(false);
            this.Color_panel.PerformLayout();
            this.Substring_panel.ResumeLayout(false);
            this.Substring_panel.PerformLayout();
            this.Delete_panel.ResumeLayout(false);
            this.Timeline_panel.ResumeLayout(false);
            this.Timeline_panel.PerformLayout();
            this.Name_panel.ResumeLayout(false);
            this.Name_panel.PerformLayout();
            this.DatasetOrderNo_panel.ResumeLayout(false);
            this.DatasetOrderNo_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AddNewDataset_button;
        private System.Windows.Forms.Button ClearCustomData_button;
        private System.Windows.Forms.Label Input_geneList_label;
        private System.Windows.Forms.Label Name_label;
        private System.Windows.Forms.Label MBCO_headline;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button AnalyzeData_button;
        private System.Windows.Forms.Label ResultsDirectory_label;
        private System.Windows.Forms.Button EnrichmentOptions_default_button;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label EnrichmentOptions_keepScpsScpLevel_label;
        private System.Windows.Forms.Label EnrichmentOptions_dynamicKeepTopScps_label;
        private System.Windows.Forms.Label EnrichmentOptions_keepScps_level_4_label;
        private System.Windows.Forms.Label EnrichmentOptions_keepScps_level_3_label;
        private System.Windows.Forms.Label EnrichmentOptions_keepScps_level_2_label;
        private System.Windows.Forms.Label EnrichmentOptions_keepScps_level_1_label;
        private System.Windows.Forms.Label EnrichmentOptions_standardKeepTopScps_label;
        private System.Windows.Forms.Label EnrichmentOptions_ontology_label;
        private System.Windows.Forms.Label MBCO_reference_label;
        private System.Windows.Forms.Label Color_label;
        private System.Windows.Forms.Label EnrichmentOptions_colorBarsTimelines_label;
        private System.Windows.Forms.Label EnrichmentOptions_percentDynamicTopSCPInteractions_label;
        private System.Windows.Forms.Label EnrichmentOptions_scpInteractionsLevel_3_label;
        private System.Windows.Forms.Label EnrichmentOptions_scpInteractionsLevel_2_label;
        private System.Windows.Forms.Button Read_readDataset_button;
        private System.Windows.Forms.Label Read_directoryOrFile_label;
        private System.Windows.Forms.Button Changes_reset_button;
        private System.Windows.Forms.Button Changes_update_button;
        private System.Windows.Forms.VScrollBar Dataset_scrollBar;
        private System.Windows.Forms.Label IntegrationGroup_label;
        private System.Windows.Forms.Button Timeline_sortBy_button;
        private System.Windows.Forms.Label EntryType_label;
        private System.Windows.Forms.Label Timeline_label;
        private System.Windows.Forms.Label CompatibilityInfos_label;
        private System.Windows.Forms.Button Name_sortBy_button;
        private System.Windows.Forms.Button IntegrationGroup_sortBy_button;
        private System.Windows.Forms.Button EntryType_sortBy_button;
        private System.Windows.Forms.Label Read_sampleNameColumn_label;
        private System.Windows.Forms.Label Read_timepointColumn_label;
        private Windows_forms_customized_tools.OwnTextBox Read_value1stColumn_ownTextBox;
        private Windows_forms_customized_tools.OwnTextBox Read_timepointColumn_ownTextBox;
        private Windows_forms_customized_tools.OwnTextBox Read_sampleNameColumn_ownTextBox;
        private System.Windows.Forms.Label Read_value1stColumn_label;
        private System.Windows.Forms.Button Options_readData_button;
        private System.Windows.Forms.Button Options_enrichment_button;
        private System.Windows.Forms.Button Options_scpNetworks_button;
        private System.Windows.Forms.Label Read_informationGroup_label;
        private System.Windows.Forms.Button Read_setToMBCO1_button;
        private System.Windows.Forms.Label Read_order_allFilesDirectory_label;
        private Windows_forms_customized_tools.OwnListBox Read_timeunit_ownCheckBox;
        private System.Windows.Forms.Label Read_geneSymbol_label;
        private Windows_forms_customized_tools.OwnTextBox Read_geneSymbol_ownTextBox;
        private System.Windows.Forms.Button Read_setToSeurat_button;
        private System.Windows.Forms.Button Read_setToMBCO2_button;
        private Windows_forms_customized_tools.OwnTextBox Read_directoryOrFile_ownTextBox;
        private System.Windows.Forms.Button Options_backgroundGenes_button;
        private System.Windows.Forms.Label Read_integrationGroupColumn_label;
        private Windows_forms_customized_tools.OwnTextBox Read_integrationGroupColumn_ownTextBox;
        private System.Windows.Forms.Button Read_setToMinimum_button;
        private System.Windows.Forms.Label Read_delimiter_label;
        private Windows_forms_customized_tools.OwnListBox Read_delimiter_ownListBox;
        private System.Windows.Forms.Button Read_error_reports_button;
        private System.Windows.Forms.Label Read_error_reports_label;
        private Windows_forms_customized_tools.OwnTextBox Report_ownTextBox;
        private System.Windows.Forms.Label Report_headline_label;
        private System.Windows.Forms.Button ScpNetworks_default_button;
        private System.Windows.Forms.Label ScpNetworks_standardConnectScpsTopInteractions_scpLevel_label;
        private System.Windows.Forms.Label ScpNetworks_standardConnectScpsTopInteractions_connect_label;
        private System.Windows.Forms.Label ScpNetworks_standardConnectScpsTopInteractions_level_3_label;
        private System.Windows.Forms.Label ScpNetworks_standardConnectScpsTopInteractions_level_2_label;
        private System.Windows.Forms.Label ScpNetworks_standard_label;
        private System.Windows.Forms.Label ScpNetworks_dynamic_label;
        private Windows_forms_customized_tools.MyPanel ScpNetworks_dynamic_panel;
        private Windows_forms_customized_tools.MyPanel ScpNetworks_standard_panel;
        private Windows_forms_customized_tools.MyPanel ScpNetworks_comments_panel;
        private System.Windows.Forms.Label ScpNetworks_commentsHeadline_label;
        private Windows_forms_customized_tools.MyPanel Options_scpNetworks_panel;
        private Windows_forms_customized_tools.MyPanel Options_readData_panel;
        private Windows_forms_customized_tools.OwnTextBox ScpNetworks_standardConnectScpsTopInteractions_level_3_textBox;
        private Windows_forms_customized_tools.OwnTextBox ScpNetworks_standardConnectScpsTopInteractions_level_2_textBox;
        private Windows_forms_customized_tools.MyPanel ScpNetworks_standardConnectScpsTopInteractions_panel;
        private Windows_forms_customized_tools.MyPanel Options_enrichment_panel;
        private Windows_forms_customized_tools.MyPanel Options_organizeData_panel;
        private System.Windows.Forms.Button Options_organizeData_button;
        private System.Windows.Forms.Button OrganizeData_changeIntegrationGroup_button;
        private Windows_forms_customized_tools.MyPanel OrganizeData_modify_panel;
        private System.Windows.Forms.Button OrganizeData_changeColor_button;
        private System.Windows.Forms.Label OrganizeData_modifyHeadline_label;
        private Windows_forms_customized_tools.MyPanel OrganizeData_show_panel;
        private System.Windows.Forms.Label OrganizeData_show_headline_label;
        private System.Windows.Forms.Button OrganizeData_automaticIntegrationGroups_button;
        private System.Windows.Forms.Button OrganizeData_automaticColors_button;
        private System.Windows.Forms.Label Report_maxErrorPerFile2_label;
        private Windows_forms_customized_tools.OwnTextBox Read_error_reports_maxErrorsPerFile_ownTextBox;
        private System.Windows.Forms.Label Report_maxErrorPerFile1_label;
        private System.Windows.Forms.Button Color_sortBy_button;
        private System.Windows.Forms.Label OrganizeData_modifyIndexRight_label;
        private Windows_forms_customized_tools.OwnTextBox OrganizeData_modifyIndexRight_ownTextBox;
        private System.Windows.Forms.Label OrganizeData_modifyIndexes_label;
        private Windows_forms_customized_tools.OwnTextBox OrganizeData_modifyIndexLeft_ownTextBox;
        private System.Windows.Forms.Label OrganizeData_modifyDelimiter_label;
        private Windows_forms_customized_tools.OwnTextBox OrganizeData_modifyDelimiter_ownTextBox;
        private System.Windows.Forms.Label Substring_label;
        private System.Windows.Forms.Button Substring_sortBy_button;
        private System.Windows.Forms.Label Source_label;
        private System.Windows.Forms.Button Source_sortBy_button;
        private System.Windows.Forms.Button BgGenes_sortBy_button;
        private System.Windows.Forms.Label BgGenes_label;
        private System.Windows.Forms.Button OrganizeData_changeDelete_button;
        private Windows_forms_customized_tools.MyPanel OrganizeData_addFileName_panel;
        private System.Windows.Forms.Label OrganizeData_addFileNameAfter_label;
        private System.Windows.Forms.Label OrganizeData_addFileNameBefore_label;
        private System.Windows.Forms.Label OrganizeData_addFileNameRemove_label;
        private System.Windows.Forms.Button OrganizeData_addFileNameRemove_button;
        private System.Windows.Forms.Button OrganizeData_addFileNameAfter_button;
        private System.Windows.Forms.Button OrganizeData_addFileNamesBefore_button;
        private System.Windows.Forms.Label OrganizeData_addFileNames_label;
        private Windows_forms_customized_tools.MyPanel Options_bgGenes_panel;
        private System.Windows.Forms.Label BgGenes_assignmentsReset_label;
        private System.Windows.Forms.Button BgGenes_assignmentsReset_button;
        private System.Windows.Forms.Label BgGenes_assignmentsAutomatic_label;
        private System.Windows.Forms.Button BgGenes_assignmentsAutomatic_button;
        private Windows_forms_customized_tools.MyPanel BgGenes_assignment_panel;
        private Windows_forms_customized_tools.MyPanel BgGenes_organize_panel;
        private System.Windows.Forms.Label BgGenes_OrganizeAvailableBgGeneLists_label;
        private Windows_forms_customized_tools.OwnListBox BgGenes_OrganizeAvailableBgGeneLists_ownListBox;
        private System.Windows.Forms.Button BgGenes_OrganizeDeleteAll_button;
        private System.Windows.Forms.Button BgGenes_OrganizeDeleteSelection_button;
        private System.Windows.Forms.Label BgGenes_overall_headline_label;
        private System.Windows.Forms.Label BgGenes_assignmentsExplanation_label;
        private System.Windows.Forms.Label Read_setToDefault_label;
        private System.Windows.Forms.Label Read_timeunitColumn_label;
        private Windows_forms_customized_tools.OwnTextBox Read_timeunitColumn_ownTextBox;
        private Windows_forms_customized_tools.MyPanel EnrichmentOptions_defineOutputs_panel;
        private Windows_forms_customized_tools.MyPanel EnrichmentOptions_colors_panel;
        private Windows_forms_customized_tools.MyPanel EnrichmentOptions_ontology_panel;
        private Windows_forms_customized_tools.MyPanel EnrichmentOptions_keepTopSCPs_panel;
        private Windows_forms_customized_tools.OwnListBox EnrichmentOptions_ontology_ownListBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_standardKeepTopLevel_4_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_standardKeepTopLevel_3_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_dynamicKeepTopLevel_3_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_dynamicKeepTopLevel_2_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_dynamicTopPercentScpsLevel_3_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_dynamicTopPercentScpsLevel_2_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_standardKeepTopLevel_2_SCPs_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_standardKeepTopLevel_1_SCPs_textBox;
        private Windows_forms_customized_tools.MyPanel Options_loadExamples_panel;
        private System.Windows.Forms.Label LoadExamples_overallHeadline_label;
        private System.Windows.Forms.Button LoadExamples_load_button;
        private System.Windows.Forms.Button Options_exampleData_button;
        private System.Windows.Forms.Button OrganizeData_automaticDatasetOrder_button;
        private Windows_forms_customized_tools.MyPanel OrganizeData_automatically_panel;
        private System.Windows.Forms.Label OrganizeData_automatically_headline_label;
        private System.Windows.Forms.Label DatasetOrderNo_label;
        private System.Windows.Forms.Button DatasetOrderNo_sortBy_button;
        private System.Windows.Forms.Label Progress_report_label;
        private System.Windows.Forms.Label EnrichmentOptions_generateTimelineExplanation_label;
        private System.Windows.Forms.Label EnrichmentOptions_generateHeatmapsExplanation_label;
        private System.Windows.Forms.Label EnrichmentOptions_generateBardiagramsExplanation_label;
        private System.Windows.Forms.Label EnrichmentOptions_saveFiguresAsExplanation_label;
        private System.Windows.Forms.Label EnrichmentOptions_safeFigures_label;
        private Windows_forms_customized_tools.OwnListBox EnrichmentOptions_saveFiguresAs_ownListBox;
        private System.Windows.Forms.Label EnrichmentOptions_maxPvalue_label;
        private System.Windows.Forms.Label EnrichmentOptions_maxRanks_label;
        private System.Windows.Forms.Label EnrichmentOptions_cutoffsExplanation_label;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_dynamicPvalue_textBox;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_standardPvalue_textBox;
        private System.Windows.Forms.Label EnrichmentOptions_ScpInteractionsLevel_label;
        private System.Windows.Forms.Label EnrichmentOptions_generateTimelinePvalue_label;
        private Windows_forms_customized_tools.OwnTextBox EnrichmentOptions_generateTimelinePvalue_textBox;
        private System.Windows.Forms.Label OrganizeData_showDifferentEntries_label;
        private System.Windows.Forms.Button OrganizeData_showDifferentEntries_button;
        private Windows_forms_customized_tools.MyPanel BgGenes_warnings_panel;
        private System.Windows.Forms.Button BgGenes_warnings_button;
        private Windows_forms_customized_tools.MyPanel Options_dataSignificance_panel;
        private System.Windows.Forms.Button SigData_resetParameter_button;
        private System.Windows.Forms.Label SigData_value1st_higherSig_expl_label;
        private System.Windows.Forms.Label SigData_value2nd_lowerSig_expl_label;
        private System.Windows.Forms.Label SigData_value2nd_higherSig_expl_label;
        private System.Windows.Forms.Label SigData_value1st_lowerSig_expl_label;
        private System.Windows.Forms.Label SigData_rankBy_left_label;
        private System.Windows.Forms.Label SigData_rankBy_top_label;
        private System.Windows.Forms.Label SigData_sigSubject_explanation_label;
        private System.Windows.Forms.Button SigData_resetSig_button;
        private System.Windows.Forms.Label SigData_keepTopRanks_right_label;
        private Windows_forms_customized_tools.OwnTextBox SigData_keepTopRanks_ownTextBox;
        private System.Windows.Forms.Label SigData_keepTopRanks_left_label;
        private System.Windows.Forms.Label SigData_2nd_sigCutoff_headline_label;
        private System.Windows.Forms.Label SigData_1st_sigCutoff_headline_label;
        private Windows_forms_customized_tools.OwnTextBox SigData_value2nd_cutoff_ownTextBox;
        private System.Windows.Forms.Label SigData_value2nd_cutoff_label;
        private Windows_forms_customized_tools.OwnTextBox SigData_value1st_cutoff_ownTextBox;
        private System.Windows.Forms.Label SigData_value1st_cutoff_label;
        private System.Windows.Forms.Label SigData_value2nd_headline_label;
        private System.Windows.Forms.Label SigData_value1st_headline_label;
        private System.Windows.Forms.Button Options_dataSignificance_button;
        private System.Windows.Forms.Label SigData_tieBreaker_explanation_label;
        private System.Windows.Forms.Label Read_value1st_explanation_label;
        private System.Windows.Forms.Label Read_value2nd_explanation_label;
        private Windows_forms_customized_tools.OwnTextBox Read_value2ndColumn_ownTextBox;
        private System.Windows.Forms.Label Read_value2ndColumn_label;
        private System.Windows.Forms.Label Read_headline_label;
        private System.Windows.Forms.Label BgGenes_warnings_label;
        private System.Windows.Forms.Label LoadExamples_KPMP_reference;
        private System.Windows.Forms.Label LoadExamples_NOG_reference;
        private Windows_forms_customized_tools.OwnTextBox Read_colorColumn_ownTextBox;
        private System.Windows.Forms.Label Read_colorColumn_label;
        private System.Windows.Forms.Label SCPNetworks_comments_yEDText_label;
        private System.Windows.Forms.Label OrganizeData_showSourceFile_label;
        private System.Windows.Forms.Button Read_setToMBCO_button;
        private System.Windows.Forms.Button Abort_button;
        private Windows_forms_customized_tools.MyPanel Options_selectSCPs_panel;
        private System.Windows.Forms.Label SelectSCPs_sortSCPs_label;
        private System.Windows.Forms.Label SelectScps_selectedGroup_label;
        private System.Windows.Forms.Button SelectSCPs_add_button;
        private System.Windows.Forms.Label SelectSCPs_overallHeadline_label;
        private System.Windows.Forms.Button Options_selectSCPs_button;
        private Windows_forms_customized_tools.OwnListBox SelectScps_selectedSCPs_ownListBox;
        private Windows_forms_customized_tools.OwnListBox SelectScps_mbcoSCPs_ownListBox;
        private System.Windows.Forms.Button SelectSCPs_remove_button;
        private System.Windows.Forms.Button SelectedSCPs_writeMbcoHierarchy_button;
        private Windows_forms_customized_tools.MyPanel EnrichmentOptions_scpTopInteractions_panel;
        private Windows_forms_customized_tools.MyPanel EnrichmentOptions_cutoffs_panel;
        private Windows_forms_customized_tools.MyPanel ScpNetworks_nodeSize_panel;
        private System.Windows.Forms.Label ScpNetworks_scpNodeSizes_label;
        private System.Windows.Forms.Button Read_setToOptimum_button;
        private System.Windows.Forms.Label LoadExamples_copyright_label;
        private Windows_forms_customized_tools.MyPanel Options_defineScps_panel;
        private System.Windows.Forms.Label DefineScps_ownSubScps_label;
        private System.Windows.Forms.Button DefineScps_addNewOwnSCP_button;
        private System.Windows.Forms.Label DefineScps_selectOwnScp_label;
        private Windows_forms_customized_tools.OwnListBox DefineScps_selectOwnScp_ownListBox;
        private Windows_forms_customized_tools.OwnTextBox DefineScps_newOwnScpName_ownTextBox;
        private System.Windows.Forms.Button DefineSCPs_writeMbcoHierarchy_button;
        private System.Windows.Forms.Button DefineScps_removeSubScp_button;
        private Windows_forms_customized_tools.OwnListBox DefineScps_mbcoSCP_ownListBox;
        private System.Windows.Forms.Label DefineScps_sort_label;
        private System.Windows.Forms.Label DefineScps_newOwnScpName_label;
        private System.Windows.Forms.Label DefineScps_mbcoSCP_label;
        private System.Windows.Forms.Button DefineScps_addSubScp_button;
        private System.Windows.Forms.Label DefineScps_overall_headline_label;
        private Windows_forms_customized_tools.OwnListBox DefineScps_ownSubScps_ownListBox;
        private System.Windows.Forms.Button Options_defineSCPs_button;
        private System.Windows.Forms.Button DefineScps_removeOwnSCP_button;
        private System.Windows.Forms.Label DefineScps_level_label;
        private Windows_forms_customized_tools.MyPanel OrganizeData_convertTimeunits_panel;
        private System.Windows.Forms.Button OrganizeData_convertTimeunites_convert_button;
        private Windows_forms_customized_tools.OwnListBox OrganizeData_convertTimeunits_unit_ownListBox;
        private System.Windows.Forms.Label OrganizeData_convertTimeunits_label;
        private System.Windows.Forms.Label SigData_allGenesSignificant_headline_label;
        private Windows_forms_customized_tools.MyPanel SigData_sigSelection_panel;
        private System.Windows.Forms.Label EnrichmentOptions_chartsPerPage_label;
        private Windows_forms_customized_tools.OwnListBox EnrichmentOptions_chartsPerPage_ownCheckBox;
        private System.Windows.Forms.Label ScpNetworks_comments_standardDynamicAddGenes_label;
        public System.Windows.Forms.Label ScpNetworks_dynamicConnectAllScps_explantion_label;
        private System.Windows.Forms.Button Options_tips_button;
        private Windows_forms_customized_tools.MyPanel Options_tips_panel;
        private System.Windows.Forms.Label Tips_label2;
        private System.Windows.Forms.Label Tips_label1;
        private System.Windows.Forms.Label Tips_overallHeadline_label;
        private System.Windows.Forms.Label Tips_label3;
        private System.Windows.Forms.Label Tips_label4;
        private System.Windows.Forms.Button SelectSCPs_removeGroup_button;
        private System.Windows.Forms.Button SelectSCPs_addGroup_button;
        private System.Windows.Forms.Label SelectSCPs_groups_label;
        private Windows_forms_customized_tools.OwnListBox SelectSCPs_groups_ownListBox;
        private Windows_forms_customized_tools.OwnTextBox SelectSCPs_newGroup_ownTextBox;
        private System.Windows.Forms.Label SelectSCPs_newGroup_label;
        private Windows_forms_customized_tools.MyPanel SelectSCPs_selection_panel;
        private System.Windows.Forms.Label SelectSCPs_includeHeadline_label;
        private Windows_forms_customized_tools.OwnListBox SelectSCPs_sortSCPs_listBox;
        private System.Windows.Forms.Label SelectSCPs_includeBracket_label;
        private Windows_forms_customized_tools.OwnListBox DefineScps_sort_listBox;
        private Windows_forms_customized_tools.MyPanel DefineScps_selection_panel;
        private System.Windows.Forms.Label Tips_label5;
        private Windows_forms_customized_tools.MyPanel OrganizeData_modifySubstringOptions_panel;
        private System.Windows.Forms.Label OrganizeData_modifyIndexLeft_label;
        private System.Windows.Forms.Label SigData_value1st_cutoff_expl_label;
        private System.Windows.Forms.Label SigData_value2nd_cutoff_expl_label;
        private Windows_forms_customized_tools.MyPanel BgGenes_add_panel;
        private System.Windows.Forms.Label BgGenes_AddErrors_label;
        private System.Windows.Forms.Button BgGenes_AddShowErrors_button;
        private System.Windows.Forms.Label BgGenes_addReadExplainFile_label;
        private System.Windows.Forms.Button BgGenes_add_button;
        private Windows_forms_customized_tools.OwnTextBox BgGenes_addGenes_ownTextBox;
        private System.Windows.Forms.Button BgGenes_AddRead_button;
        private System.Windows.Forms.Label BgGenes_addName_label;
        private System.Windows.Forms.Label BgGenes_addReadFileDir_label;
        private System.Windows.Forms.Label BgGenes_addGenes_label;
        private Windows_forms_customized_tools.OwnTextBox BgGenes_addName_ownTextBox;
        private Windows_forms_customized_tools.MyPanel DatasetInterface_overall_panel;
        private Windows_forms_customized_tools.OwnTextBox Input_geneList_textBox;
        private Windows_forms_customized_tools.MyPanel BgGenes_panel;
        private Windows_forms_customized_tools.MyPanel DatasetOrderNo_panel;
        private Windows_forms_customized_tools.MyPanel Source_panel;
        private Windows_forms_customized_tools.MyPanel Timeline_panel;
        private Windows_forms_customized_tools.MyPanel IntegrationGroup_panel;
        private Windows_forms_customized_tools.MyPanel Delete_panel;
        private Windows_forms_customized_tools.MyPanel GeneCounts_panel;
        private Windows_forms_customized_tools.MyPanel DatasetsCount_panel;
        private Windows_forms_customized_tools.MyPanel Name_panel;
        private Windows_forms_customized_tools.MyPanel Color_panel;
        private Windows_forms_customized_tools.MyPanel Substring_panel;
        private Windows_forms_customized_tools.MyPanel EntryType_panel;
        private Windows_forms_customized_tools.MyPanel Report_panel;
        private Windows_forms_customized_tools.OwnTextBox ResultsDirectory_textBox;
        private Windows_forms_customized_tools.OwnTextBox AppSize_width_textBox;
        private System.Windows.Forms.Label AppSize_height_label;
        private System.Windows.Forms.Label AppSize_width_label;
        private System.Windows.Forms.Button AppSize_resize_button;
        private Windows_forms_customized_tools.OwnTextBox AppSize_height_textBox;
        private Windows_forms_customized_tools.MyPanel AppSize_panel;
        private System.Windows.Forms.Label AppSize_colorTheme_label;
        private Windows_forms_customized_tools.OwnListBox AppSize_colorTheme_listBox;
        private System.Windows.Forms.Label AppSize_headline_label;
        private Windows_forms_customized_tools.MyCheckBox_button Read_order_onlySpecifiedFile_cbButton;
        private System.Windows.Forms.Label Read_order_allFilesInDirectory_label;
        private System.Windows.Forms.Label Read_order_onlySpecifiedFile_label;
        private Windows_forms_customized_tools.MyCheckBox_button Read_order_allFilesInDirectory_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showSourceFile_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showColor_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showIntegrationGroup_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showTimepoint_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showEntryType_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showName_cbButton;
        private System.Windows.Forms.Label OrganizeData_showDatasetOrderNo_cbLabel;
        private System.Windows.Forms.Label OrganizeData_showSourceFile_cbLabel;
        private System.Windows.Forms.Label OrganizeData_showColor_cbLabel;
        private System.Windows.Forms.Label OrganizeData_showIntegrationGroup_cbLabel;
        private System.Windows.Forms.Label OrganizeData_showTimepoint_cbLabel;
        private System.Windows.Forms.Label OrganizeData_showEntryType_cbLabel;
        private System.Windows.Forms.Label OrganizeData_showName_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_showDatasetOrderNo_cbButton;
        private System.Windows.Forms.Label OrganizeData_modifySubstring_cbLabel;
        private System.Windows.Forms.Label OrganizeData_modifyEntryType_cbLabel;
        private System.Windows.Forms.Label OrganizeData_modifyTimepoint_cbLabel;
        private System.Windows.Forms.Label OrganizeData_modifyName_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_modifySubstring_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_modifySourceFileName_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_modifyEntryType_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_modifyTimepoint_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button OrganizeData_modifyName_cbButton;
        private System.Windows.Forms.Label OrganizeData_modifySourceFileName_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_generateNetworks_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_generateNetworks_cbButton;
        private System.Windows.Forms.Label ScpNetworks_nodeSizeFixed_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_nodeSizeByDatasetCount_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_nodeSizeByColorCount_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_nodeSizeFixed_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_nodeSizeByDatasetCount_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_nodeSizeByColorCount_cbButton;
        private System.Windows.Forms.Label ScpNetworks_standardConnectRelated_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_standardGroupSameLevelSCPs_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_standardAddGenes_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_standardParentChild_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_standardConnectRelated_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_standardAddGenes_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_standardParentChild_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_standardGroupSameLevelSCPs_cbButton;
        private System.Windows.Forms.Label ScpNetworks_dynamicConnectAllRelated_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_dynamicAddGenes_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_dynamicGroupSameLevelSCPs_cbLabel;
        private System.Windows.Forms.Label ScpNetworks_dynamicParentChild_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_dynamicConnectAllRelated_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_dynamicAddGenes_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_dynamicGroupSameLevelSCPs_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button ScpNetworks_dynamicParentChild_cbButton;
        private System.Windows.Forms.Label SigData_allGenesSignificant_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_allGenesSignificant_cbButton;
        private System.Windows.Forms.Label SigData_deleteNotSignGenes_cbLabel;
        private System.Windows.Forms.Label SigData_keep_mergeUpDown_cbLabel;
        private System.Windows.Forms.Label SigData_keep_eachDataset_cbLabel;
        private System.Windows.Forms.Label SigData_rankBy_2ndValue_cbLabel;
        private System.Windows.Forms.Label SigData_rankBy_1stValue_cbLabel;
        private System.Windows.Forms.Label SigData_value2nd_lowerSig_cbLabel;
        private System.Windows.Forms.Label SigData_value2nd_higherSig_cbLabel;
        private System.Windows.Forms.Label SigData_value1st_lowerSig_cbLabel;
        private System.Windows.Forms.Label SigData_value1st_higherSig_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_deleteNotSignGenes_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_keep_eachDataset_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_keep_mergeUpDown_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_rankBy_2ndValue_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_rankBy_1stValue_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_value2nd_lowerSig_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_value2nd_higherSig_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_value1st_lowerSig_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SigData_value1st_higherSig_cbButton;
        private System.Windows.Forms.Label SelectSCPs_includeOffspringSCPs_cbLabel;
        private System.Windows.Forms.Label SelectSCPs_includeAncestorSCPs_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button SelectSCPs_includeOffspringSCPs_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SelectSCPs_includeAncestorSCPs_cbButton;
        private System.Windows.Forms.Label SelectSCPs_addGenes_cbLabel;
        private System.Windows.Forms.Label SelectSCPs_showOnlySelectedScps_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button SelectSCPs_addGenes_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button SelectSCPs_showOnlySelectedScps_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button DefineScps_level4_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button DefineScps_level3_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button DefineScps_level2_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button DefineScps_level1_cbButton;
        private System.Windows.Forms.Label DefineScps_level4_cbLabel;
        private System.Windows.Forms.Label DefineScps_level3_cbLabel;
        private System.Windows.Forms.Label DefineScps_level2_cbLabel;
        private System.Windows.Forms.Label DefineScps_level1_cbLabel;
        private System.Windows.Forms.Label BgGenes_addReadAllFilesInDirectory_cbLabel;
        private System.Windows.Forms.Label BgGenes_addReadOnlyFile_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button BgGenes_addReadAllFilesInDirectory_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button BgGenes_addReadOnlyFile_cbButton;
        private System.Windows.Forms.Label EnrichmentOptions_generateTimelineLogScale_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_generateTimeline_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_generateHeatmapShowRanks_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_generateHeatmaps_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_generateBardiagrams_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateTimelineLogScale_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateTimeline_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateHeatmapShowSignificantSCPsInAllDatasets_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateHeatmapShowMinuLog10Pvalues_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateHeatmapShowRanks_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateHeatmaps_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_generateBardiagrams_cbButton;
        private System.Windows.Forms.Label EnrichmentOptions_colorByDatasetColor_cbLabel;
        private System.Windows.Forms.Label EnrichmentOptions_colorByLevel_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_colorByDatasetColor_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button EnrichmentOptions_colorByLevel_cbButton;
        private System.Windows.Forms.Label LoadExamples_KPMPreference_cbLabel;
        private System.Windows.Forms.Label LoadExamples_NOG_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button LoadExamples_KPMPreference_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button LoadExamples_NOG_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button Dataset_all_delete_cbButton;
        private Windows_forms_customized_tools.OwnListBox OrganizeData_addFileNames_listBox;
        private System.Windows.Forms.Label Tips_label6;
        private System.Windows.Forms.Label Tips_demonstration_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button Tips_demonstration_cbButton;
        private System.Windows.Forms.Label Tips_demonstration_headline_label;
        private System.Windows.Forms.Button AppSize_decrease_button;
        private System.Windows.Forms.Button AppSize_increase_button;
        private System.Windows.Forms.Label AppSize_width_percent_label;
        private System.Windows.Forms.Label AppSize_heightPercent_label;
        private Windows_forms_customized_tools.MyPanel Results_visualization_panel;
        private System.Windows.Forms.Button Options_results_button;
        private Windows_forms_customized_tools.MyPanel Options_results_panel;
        private System.Windows.Forms.Label Results_directory_expl_label;
        private System.Windows.Forms.Label Results_directory_headline_label;
        private System.Windows.Forms.Label Results_directory_label;
        private System.Windows.Forms.Label Results_heatmap_standard_cbLabel;
        private System.Windows.Forms.Label Results_timeline_cbLabel;
        private System.Windows.Forms.Label Results_bardiagram_standard_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button Results_heatmap_standard_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button Results_timeline_cbButton;
        private Windows_forms_customized_tools.MyCheckBox_button Results_bardiagram_standard_cbButton;
        private System.Windows.Forms.Label Results_overall_headline_label;
        private System.Windows.Forms.Label Results_bardiagram_show_label;
        private Windows_forms_customized_tools.MyCheckBox_button Results_bardiagram_dynamic_cbButton;
        private System.Windows.Forms.Label Results_heatmap_show_label;
        private System.Windows.Forms.Label Results_timeline_show_label;
        private System.Windows.Forms.Label Results_bardiagram_dynamic_cbLabel;
        private System.Windows.Forms.Label Results_heatmap_dynamic_cbLabel;
        private Windows_forms_customized_tools.MyCheckBox_button Results_heatmap_dynamic_cbButton;
        private System.Windows.Forms.Label Results_position_label;
        private System.Windows.Forms.Button Results_previous_button;
        private System.Windows.Forms.Button Results_next_button;
        private Windows_forms_customized_tools.OwnListBox Results_integrationGroup_listBox;
        private System.Windows.Forms.Label Results_integrationGroup_label;
        private Windows_forms_customized_tools.MyCheckBox_button Results_addResultsToControl_cbButton;
        private System.Windows.Forms.Label Results_addResultsToControl_cbLabel;
        private Windows_forms_customized_tools.MyPanel Results_controlCommand_panel;
        private System.Windows.Forms.Label Results_visualization_integrationGroup_label;
        private ZedGraph.ZedGraphControl Results_zegGraph_control;
        private System.Windows.Forms.Button EnrichmentOptions_explanation_button;
        private System.Windows.Forms.Button OrganizeData_explanation_button;
        private System.Windows.Forms.Button ScpNetworks_explanation_button;
    }
}