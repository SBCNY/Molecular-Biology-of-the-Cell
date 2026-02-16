//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
//using ZedGraph;


namespace ClassLibrary1.EnrichmentOptions_userInterface
{
    class EnrichmentOptions_text_class
    {
        public static string Timeline_label_Log_only_allowed_if_all_values_larger_zero {  get { return "Log scale only allowed, if all timepoints > 0"; } }
        public static string Timeline_label_standard_text { get { return "1 timeline chart for each integration group and SCP"; } }
        public static string Timeline_label_only_one_timepoint { get { return "Data contains only one timepoint"; } }
    }

    class EnrichmentOptions_userInterface_class
    {
        MyPanel Overall_panel { get; set; }
        MyPanel Ontology_panel { get; set; }
        System.Windows.Forms.Label Ontology_label { get; set; }
        MyPanel Cutoffs_panel { get; set; }
        MyPanel ScpTopInteractions_panel { get; set; }
        public MyPanel EnrichmentSettings_panel { get; set; }
        System.Windows.Forms.Label KeepScpsScpLevel_label { get; set; }
        MyPanel_label MaxRanks_myPanelLabel { get; set; }
        System.Windows.Forms.Label KeepScps_level_1_label { get; set; }
        System.Windows.Forms.Label KeepScps_level_2_label { get; set; }
        System.Windows.Forms.Label KeepScps_level_3_label { get; set; }
        System.Windows.Forms.Label KeepScps_level_4_label { get; set; }
        System.Windows.Forms.Label MaxPvalue_label { get; set; }
        System.Windows.Forms.Label StandardKeepTopScps_label { get; set; }
        OwnTextBox StandardKeepTopLevel_1_SCPs_textBox { get; set; }
        OwnTextBox StandardKeepTopLevel_2_SCPs_textBox { get; set; }
        OwnTextBox StandardKeepTopLevel_3_SCPs_textBox { get; set; }
        OwnTextBox StandardKeepTopLevel_4_SCPs_textBox { get; set; }
        System.Windows.Forms.Label DynamicKeepTopScps_label { get; set; }
        OwnTextBox DynamicKeepTopLevel_2_SCPs_textBox { get; set; }
        OwnTextBox DynamicKeepTopLevel_3_SCPs_textBox { get; set; }
        OwnTextBox StandardPvalue_textBox { get; set; }
        OwnTextBox DynamicPvalue_textBox { get; set; }
        System.Windows.Forms.Label ScpInteractionsLevel_label { get; set; }
        System.Windows.Forms.Label ScpInteractionsLevel_2_label { get; set; }
        System.Windows.Forms.Label ScpInteractionsLevel_3_label { get; set; }

        System.Windows.Forms.Label PercentDynamicTopSCPInteractions_label { get; set; }
        OwnTextBox DynamicTopPercentScpsLevel_2_SCPs_textBox { get; set; }
        OwnTextBox DynamicTopPercentScpsLevel_3_SCPs_textBox { get; set; }
        MyPanel_label CutoffsExplanation_myPanelLabel { get; set; }
        Button Default_button { get; set; }


        MyPanel GO_hyperparameter_panel { get; set; }
        System.Windows.Forms.Label GO_headline_label { get; set; }
        System.Windows.Forms.Label GO_size_label { get; set; }
        System.Windows.Forms.Label GO_size_min_label { get; set; }
        System.Windows.Forms.Label GO_size_max_label { get; set; }
        OwnTextBox GO_sizeMin_ownTextBox { get; set; }
        OwnTextBox GO_sizeMax_ownTextBox { get; set; }
        System.Windows.Forms.Label GO_explanation_label { get; set; }


        MyPanel DefineOutputs_panel { get; set; }
        MyCheckBox_button GenerateBardiagrams_cbButton { get; set; }
        System.Windows.Forms.Label GenerateBardiagrams_cbLabel { get; set; }
        System.Windows.Forms.Label GenerateBardiagramsExplanation_label { get; set; }
        MyCheckBox_button GenerateHeatmaps_cbButton { get; set; }
        System.Windows.Forms.Label GenerateHeatmaps_cbLabel { get; set; }
        System.Windows.Forms.Label GenerateHeatmapsExplanation_label { get; set; }
        MyCheckBox_button GenerateHeatmapShowRanks_cbButton { get; set; }
        System.Windows.Forms.Label GenerateHeatmapShowRanks_cbLabel { get; set; }
        MyCheckBox_button GenerateHeatmapShowMinusLog10Pvalues_cbButton { get; set; }
        System.Windows.Forms.Label GenerateHeatmapShowMinusLog10Pvalues_cbLabel { get; set; }
        MyCheckBox_button GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton { get; set; }
        System.Windows.Forms.Label GenerateHeatmapShowSignificantSCPsInAllDatasets_cbLabel { get; set; }
        MyCheckBox_button GenerateTimeline_cbButton { get; set; }
        System.Windows.Forms.Label GenerateTimeline_cbLabel { get; set; }
        System.Windows.Forms.Label GenerateTimelineExplanation_label { get; set; }
        System.Windows.Forms.Label GenerateTimelinePvalue_label { get; set; }
        OwnTextBox GenerateTimelinePvalue_textBox { get; set; }
        MyCheckBox_button GenerateTimelineLogScale_cbButton { get; set; }
        System.Windows.Forms.Label GenerateTimelineLogScale_cbLabel { get; set; }
        System.Windows.Forms.Label SafeFigures_label { get; set; }
        OwnListBox SaveFiguresAs_ownListBox { get; set; }
        System.Windows.Forms.Label SaveFiguresAsExplanation_label { get; set; }
        System.Windows.Forms.Label ChartsPerPage_label { get; set; }
        OwnListBox ChartsPerPage_ownListBox { get; set; }

        MyPanel Colors_panel { get; set; }
        System.Windows.Forms.Label ColorBarsTimelines_label { get; set; }
        MyCheckBox_button ColorByLevel_cbButton { get; set; }
        System.Windows.Forms.Label ColorByLevel_cbLabel{ get; set; }
        MyCheckBox_button ColorByDatasetColor_cbButton { get; set; }
        System.Windows.Forms.Label ColorByDatasetColor_cbLabel { get; set; }
        private System.Windows.Forms.Label Error_reports_headline_label { get; set; }
        private System.Windows.Forms.Label Error_reports_maxErrorPerFile1_label { get; set; }
        private System.Windows.Forms.Label Error_reports_maxErrorPerFile2_label { get; set; }
        private OwnTextBox Error_reports_ownTextBox { get; set; }
        private OwnTextBox Error_reports_maxErrorsPerFile_ownTextBox { get; set; }

        private Button Explanation_button { get; set; }
        private Button Tutorial_button { get; set; }
        private Tutorial_interface_class UserInterface_tutorial { get; set; }
        Form1_default_settings_class Form_default_settings { get; set; }

        public EnrichmentOptions_userInterface_class(MyPanel overall_panel,
                                                     MyPanel ontology_panel,
                                                     System.Windows.Forms.Label ontology_label,
                                                     MyPanel cutoffs_panel,
                                                     MyPanel scpTopInteractions_panel,
                                                     MyPanel enrichmentSettings_panel,
                                                     System.Windows.Forms.Label keepScpsScpLevel_label,
                                                     MyPanel_label maxRanks_myPanelLabel,
                                                     System.Windows.Forms.Label keepScps_level_1_label,
                                                     System.Windows.Forms.Label keepScps_level_2_label,
                                                     System.Windows.Forms.Label keepScps_level_3_label,
                                                     System.Windows.Forms.Label keepScps_level_4_label,
                                                     System.Windows.Forms.Label maxPvalue_label,
                                                     System.Windows.Forms.Label standardKeepTopScps_label,
                                                     OwnTextBox standardKeepTopLevel_1_SCPs_textBox,
                                                     OwnTextBox standardKeepTopLevel_2_SCPs_textBox,
                                                     OwnTextBox standardKeepTopLevel_3_SCPs_textBox,
                                                     OwnTextBox standardKeepTopLevel_4_SCPs_textBox,
                                                     System.Windows.Forms.Label dynamicKeepTopScps_label,
                                                     OwnTextBox dynamicKeepTopLevel_2_SCPs_textBox,
                                                     OwnTextBox dynamicKeepTopLevel_3_SCPs_textBox,
                                                     OwnTextBox standardPvalue_textBox,
                                                     OwnTextBox dynamicPvalue_textBox,
                                                     System.Windows.Forms.Label scpInteractionsLevel_label,
                                                     System.Windows.Forms.Label scpInteractionsLevel_2_label,
                                                     System.Windows.Forms.Label scpInteractionsLevel_3_label,
                                                     System.Windows.Forms.Label percentDynamicTopSCPInteractions_label,
                                                     OwnTextBox dynamicTopPercentScpsLevel_2_SCPs_textBox,
                                                     OwnTextBox dynamicTopPercentScpsLevel_3_SCPs_textBox,
                                                     MyPanel_label cutoffsExplanation_myPanelLabel,
                                                     Button default_button,

                                                     MyPanel go_hyperparameter_panel,
                                                     System.Windows.Forms.Label go_headline_label,
                                                     System.Windows.Forms.Label go_size_label,
                                                     System.Windows.Forms.Label go_size_min_label,
                                                     System.Windows.Forms.Label go_size_max_label,
                                                     OwnTextBox go_sizeMin_ownTextBox,
                                                     OwnTextBox go_sizeMax_ownTextBox,
                                                     System.Windows.Forms.Label go_explanation_label,

                                                     MyPanel defineOutputs_panel,
                                                     MyCheckBox_button generateBardiagrams_cbButton,
                                                     System.Windows.Forms.Label generateBardiagrams_cbLabel,
                                                     System.Windows.Forms.Label generateBardiagramsExplanation_label,
                                                     MyCheckBox_button generateHeatmaps_cbButton,
                                                     System.Windows.Forms.Label generateHeatmaps_cbLabel,
                                                     System.Windows.Forms.Label generateHeatmapsExplanation_label,
                                                     MyCheckBox_button generateHeatmapShowRanks_cbButton,
                                                     System.Windows.Forms.Label generateHeatmapShowRanks_cbLabel,
                                                     MyCheckBox_button generateHeatmapShowMinusLog10Pvalues_cbButton,
                                                     System.Windows.Forms.Label generateHeatmapShowMinusLog10Pvalues_cbLabel,
                                                     MyCheckBox_button generateHeatmapShowSignificantSCPsInAllDatasets_cbButton,
                                                     System.Windows.Forms.Label generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel,
                                                     MyCheckBox_button generateTimeline_cbButton,
                                                     System.Windows.Forms.Label generateTimeline_cbLabel,
                                                     System.Windows.Forms.Label generateTimelineExplanation_label,
                                                     System.Windows.Forms.Label generateTimelinePvalue_label,
                                                     OwnTextBox generateTimelinePvalue_textBox,
                                                     MyCheckBox_button generateTimelineLogScale_cbButton,
                                                     System.Windows.Forms.Label generateTimelineLogScale_cbLabel,
                                                     System.Windows.Forms.Label safeFigures_label,
                                                     OwnListBox saveFiguresAs_ownListBox,
                                                     System.Windows.Forms.Label saveFiguresAsExplanation_label,
                                                     System.Windows.Forms.Label chartsPerPage_label,
                                                     OwnListBox chartsPerPage_ownListBox,

                                                     MyPanel colors_panel,
                                                     System.Windows.Forms.Label colorBarsTimelines_label,
                                                     MyCheckBox_button colorByLevel_cbButton,
                                                     System.Windows.Forms.Label colorByLevel_cbLabel,
                                                     MyCheckBox_button colorByDatasetColor_cbButton,
                                                     System.Windows.Forms.Label colorByDatasetColor_cbLabel,

                                                     System.Windows.Forms.Label error_reports_headline_label,
                                                     System.Windows.Forms.Label error_reports_maxErrorPerFile1_label,
                                                     System.Windows.Forms.Label error_reports_maxErrorPerFile2_label,
                                                     OwnTextBox error_reports_ownTextBox,
                                                     OwnTextBox error_reports_maxErrorsPerFile_ownTextBox,

                                                     Button explanation_button,
                                                     Button tutorial_button,
                                                     Tutorial_interface_class userInterface_tutorial,

                                                     MBCO_enrichment_pipeline_options_class mbco_options,
                                                     Form1_default_settings_class form_default_settings
            )
        {
            this.Form_default_settings = form_default_settings;

            Overall_panel = overall_panel;
            Ontology_panel = ontology_panel;
            Ontology_label = ontology_label;
            EnrichmentSettings_panel = enrichmentSettings_panel;
            Cutoffs_panel = cutoffs_panel;
            ScpTopInteractions_panel = scpTopInteractions_panel;
            StandardKeepTopLevel_1_SCPs_textBox = standardKeepTopLevel_1_SCPs_textBox;
            StandardKeepTopLevel_2_SCPs_textBox = standardKeepTopLevel_2_SCPs_textBox;
            StandardKeepTopLevel_3_SCPs_textBox = standardKeepTopLevel_3_SCPs_textBox;
            StandardKeepTopLevel_4_SCPs_textBox = standardKeepTopLevel_4_SCPs_textBox;
            DynamicKeepTopLevel_2_SCPs_textBox = dynamicKeepTopLevel_2_SCPs_textBox;
            DynamicKeepTopLevel_3_SCPs_textBox = dynamicKeepTopLevel_3_SCPs_textBox;
            StandardPvalue_textBox = standardPvalue_textBox;
            DynamicPvalue_textBox = dynamicPvalue_textBox;
            DynamicTopPercentScpsLevel_2_SCPs_textBox = dynamicTopPercentScpsLevel_2_SCPs_textBox;
            DynamicTopPercentScpsLevel_3_SCPs_textBox = dynamicTopPercentScpsLevel_3_SCPs_textBox;

            KeepScpsScpLevel_label = keepScpsScpLevel_label;
            MaxRanks_myPanelLabel = maxRanks_myPanelLabel;
            MaxPvalue_label = maxPvalue_label;
            KeepScps_level_1_label = keepScps_level_1_label;
            KeepScps_level_2_label = keepScps_level_2_label;
            KeepScps_level_3_label = keepScps_level_3_label;
            KeepScps_level_4_label = keepScps_level_4_label;
            ScpInteractionsLevel_2_label = scpInteractionsLevel_2_label;
            ScpInteractionsLevel_3_label = scpInteractionsLevel_3_label;
            StandardKeepTopScps_label = standardKeepTopScps_label;
            DynamicKeepTopScps_label = dynamicKeepTopScps_label;
            PercentDynamicTopSCPInteractions_label = percentDynamicTopSCPInteractions_label;
            ScpInteractionsLevel_label = scpInteractionsLevel_label;
            CutoffsExplanation_myPanelLabel = cutoffsExplanation_myPanelLabel;
            Default_button = default_button;

            GO_hyperparameter_panel = go_hyperparameter_panel;
            GO_headline_label = go_headline_label;
            GO_size_label = go_size_label;
            GO_size_min_label = go_size_min_label;
            GO_size_max_label = go_size_max_label;
            GO_sizeMin_ownTextBox = go_sizeMin_ownTextBox;
            GO_sizeMax_ownTextBox = go_sizeMax_ownTextBox;
            GO_explanation_label = go_explanation_label;

            DefineOutputs_panel = defineOutputs_panel;
            GenerateTimelinePvalue_textBox = generateTimelinePvalue_textBox;
            GenerateBardiagrams_cbButton = generateBardiagrams_cbButton;
            GenerateBardiagrams_cbLabel = generateBardiagrams_cbLabel;
            GenerateHeatmaps_cbButton = generateHeatmaps_cbButton;
            GenerateHeatmaps_cbLabel = generateHeatmaps_cbLabel;
            GenerateTimeline_cbButton = generateTimeline_cbButton;
            GenerateTimeline_cbLabel = generateTimeline_cbLabel;
            GenerateHeatmapShowRanks_cbButton = generateHeatmapShowRanks_cbButton;
            GenerateHeatmapShowRanks_cbLabel = generateHeatmapShowRanks_cbLabel;
            GenerateHeatmapShowMinusLog10Pvalues_cbButton = generateHeatmapShowMinusLog10Pvalues_cbButton;
            GenerateHeatmapShowMinusLog10Pvalues_cbLabel = generateHeatmapShowMinusLog10Pvalues_cbLabel;
            GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton = generateHeatmapShowSignificantSCPsInAllDatasets_cbButton;
            GenerateHeatmapShowSignificantSCPsInAllDatasets_cbLabel = generateHeatmapShowSignificantSCPsInAllDatasets_cbLabel;
            GenerateTimelineLogScale_cbButton = generateTimelineLogScale_cbButton;
            GenerateTimelineLogScale_cbLabel = generateTimelineLogScale_cbLabel;
            SaveFiguresAs_ownListBox = saveFiguresAs_ownListBox;
            ChartsPerPage_ownListBox = chartsPerPage_ownListBox;
            GenerateTimelineExplanation_label = generateTimelineExplanation_label;
            GenerateBardiagramsExplanation_label = generateBardiagramsExplanation_label;
            GenerateHeatmapsExplanation_label = generateHeatmapsExplanation_label;
            GenerateTimelinePvalue_label = generateTimelinePvalue_label;
            SaveFiguresAsExplanation_label = saveFiguresAsExplanation_label;
            SafeFigures_label = safeFigures_label;
            ChartsPerPage_label = chartsPerPage_label;
            Colors_panel = colors_panel;
            ColorByLevel_cbButton = colorByLevel_cbButton;
            ColorByLevel_cbLabel = colorByLevel_cbLabel;
            ColorByDatasetColor_cbButton = colorByDatasetColor_cbButton;
            ColorByDatasetColor_cbLabel = colorByDatasetColor_cbLabel;
            ColorBarsTimelines_label = colorBarsTimelines_label;

            Error_reports_headline_label = error_reports_headline_label;
            Error_reports_maxErrorPerFile1_label = error_reports_maxErrorPerFile1_label;
            Error_reports_maxErrorPerFile2_label = error_reports_maxErrorPerFile2_label;
            Error_reports_maxErrorsPerFile_ownTextBox = error_reports_maxErrorsPerFile_ownTextBox;
            Error_reports_ownTextBox = error_reports_ownTextBox;

            Explanation_button = explanation_button;
            Tutorial_button = tutorial_button;
            UserInterface_tutorial = userInterface_tutorial;

            this.SaveFiguresAs_ownListBox.Items.Clear();
            this.SaveFiguresAs_ownListBox.Items.Add(System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            this.SaveFiguresAs_ownListBox.Items.Add(System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
            this.SaveFiguresAs_ownListBox.Items.Add(System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Tiff);
            this.SaveFiguresAs_ownListBox.Items.Add("Pdf");

            Update_all_graphic_elements(mbco_options);
        }

        public void Update_all_graphic_elements(MBCO_enrichment_pipeline_options_class mbco_options)
        { 
            int top_reference_border; int bottom_reference_border; int left_reference_border; int right_reference_border; 
            MyPanel my_panel; System.Windows.Forms.Label my_label; MyCheckBox_button my_cb_button; OwnTextBox my_textBox; Button my_button; OwnListBox my_listBox;
            int left_position; int right_position; int top_position; int bottom_position;
            int min_left_position = 0;

            Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            #region Ontology panel and listBox
            left_reference_border = 0;
            right_reference_border = Overall_panel.Width;
            top_reference_border = 0;
            bottom_reference_border = (int)Math.Round(0.07F*this.Overall_panel.Height);
            my_panel = Ontology_panel;
            Ontology_panel = Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_position = min_left_position;
            right_position = Ontology_panel.Width - min_left_position;
            top_position = 0;
            bottom_position = Ontology_panel.Height;
            Ontology_label = Ontology_label;
            Ontology_label = Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_y(Ontology_label, left_position, right_position, top_position, bottom_position);
            #endregion

            #region Panel and subpanels
            left_reference_border = 0;
            right_reference_border = Overall_panel.Width;
            top_reference_border = Ontology_panel.Location.Y + Ontology_panel.Height;
            bottom_reference_border = (int)Math.Round(0.46F*Overall_panel.Height);
            my_panel = EnrichmentSettings_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;
            right_reference_border = EnrichmentSettings_panel.Width;
            top_reference_border = 0;
            bottom_reference_border = (int)Math.Round(EnrichmentSettings_panel.Height * 0.6);
            my_panel = Cutoffs_panel;
            Cutoffs_panel = Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            top_reference_border = (int)Math.Round(EnrichmentSettings_panel.Height * 0.7);
            bottom_reference_border = EnrichmentSettings_panel.Height;

            right_reference_border = left_reference_border + EnrichmentSettings_panel.Width;
            my_panel = ScpTopInteractions_panel;
            ScpTopInteractions_panel = Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            my_panel = GO_hyperparameter_panel;
            GO_hyperparameter_panel = Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            int shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes = (int)Math.Round(0.13F * this.EnrichmentSettings_panel.Height);
            int shared_width_of_enrichmentSettingsKeep_scpInteractions_textBoxes = (int)Math.Round(0.08F * this.EnrichmentSettings_panel.Width);

            #region Enrichment settings Cutoff panel textboxes
            int shared_location_Y_of_top_enrichmentSetting_textBox_row = (int)Math.Round(0.35F * this.Cutoffs_panel.Height);
            int shared_height_of_enrichmentSetting_textBoxes = shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes;
            int shared_width_of_enrichmentSettingKeep_textBoxes = shared_width_of_enrichmentSettingsKeep_scpInteractions_textBoxes;
            int shared_right_referenceBorder_pvalue_textBoxes = (int)Math.Round(0.98F * this.Cutoffs_panel.Width);
            int shared_distance_between_keepAndPvalue_textBoxes = (int)Math.Round(0.5F * shared_width_of_enrichmentSettingKeep_textBoxes);

            OwnTextBox[] standardKeep_textBoxes = new OwnTextBox[] { StandardKeepTopLevel_1_SCPs_textBox, StandardKeepTopLevel_2_SCPs_textBox, StandardKeepTopLevel_3_SCPs_textBox, StandardKeepTopLevel_4_SCPs_textBox };
            OwnTextBox standardKeep_textBox;
            int standardKeep_textBoxes_length = standardKeep_textBoxes.Length;

            OwnTextBox[] dynamicKeep_textBoxes = new OwnTextBox[] { new OwnTextBox(), DynamicKeepTopLevel_2_SCPs_textBox, DynamicKeepTopLevel_3_SCPs_textBox };
            OwnTextBox dynamicKeep_textBox;
            int dynamicKeep_textBoxes_length = dynamicKeep_textBoxes.Length;

            right_reference_border = (int)Math.Round(0.48F * this.Cutoffs_panel.Width);
            top_reference_border = shared_location_Y_of_top_enrichmentSetting_textBox_row;
            int[] left_reference_borders = new int[standardKeep_textBoxes_length];
            int[] right_reference_borders = new int[standardKeep_textBoxes_length];
            bottom_reference_border = top_reference_border + shared_height_of_enrichmentSetting_textBoxes;
            for (int indexStandardKeep=0; indexStandardKeep<standardKeep_textBoxes_length; indexStandardKeep++)
            {
                standardKeep_textBox = standardKeep_textBoxes[indexStandardKeep];
                left_reference_border = right_reference_border;
                right_reference_border = left_reference_border + shared_width_of_enrichmentSettingKeep_textBoxes;
                left_reference_borders[indexStandardKeep] = left_reference_border;
                right_reference_borders[indexStandardKeep] = right_reference_border;
                my_textBox = standardKeep_textBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }

            left_reference_border = StandardKeepTopLevel_4_SCPs_textBox.Location.X + StandardKeepTopLevel_4_SCPs_textBox.Width + shared_distance_between_keepAndPvalue_textBoxes;
            right_reference_border = shared_right_referenceBorder_pvalue_textBoxes;
            my_textBox = StandardPvalue_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            int distance_between_standard_and_dynamic_keepLevels = (int)Math.Round(0.05F * this.Cutoffs_panel.Height);

            top_reference_border = this.StandardKeepTopLevel_2_SCPs_textBox.Location.Y + this.StandardKeepTopLevel_2_SCPs_textBox.Height + distance_between_standard_and_dynamic_keepLevels;
            bottom_reference_border = top_reference_border + shared_height_of_enrichmentSetting_textBoxes;
            for (int indexDynamicKeep=1; indexDynamicKeep<dynamicKeep_textBoxes_length; indexDynamicKeep++)
            {
                dynamicKeep_textBox = dynamicKeep_textBoxes[indexDynamicKeep];
                left_reference_border = left_reference_borders[indexDynamicKeep];
                right_reference_border = right_reference_borders[indexDynamicKeep];
                my_textBox = dynamicKeep_textBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }

            left_reference_border = StandardKeepTopLevel_4_SCPs_textBox.Location.X + StandardKeepTopLevel_4_SCPs_textBox.Width + shared_distance_between_keepAndPvalue_textBoxes;
            right_reference_border = shared_right_referenceBorder_pvalue_textBoxes;
            my_textBox = DynamicPvalue_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region EnrichmentSettings Cutoff label
            int halfdistanceHeight_between_enrichmetSettings_textBoxes = (int)Math.Round(0.5 * (DynamicKeepTopLevel_2_SCPs_textBox.Location.Y - StandardKeepTopLevel_2_SCPs_textBox.Location.Y - StandardKeepTopLevel_2_SCPs_textBox.Height));

            Dictionary<System.Windows.Forms.Label, OwnTextBox> headlineLabel_of_referenceTextBox_dict = new Dictionary<System.Windows.Forms.Label, OwnTextBox>();
            headlineLabel_of_referenceTextBox_dict.Add(KeepScps_level_1_label, StandardKeepTopLevel_1_SCPs_textBox);
            headlineLabel_of_referenceTextBox_dict.Add(KeepScps_level_2_label, StandardKeepTopLevel_2_SCPs_textBox);
            headlineLabel_of_referenceTextBox_dict.Add(KeepScps_level_3_label, StandardKeepTopLevel_3_SCPs_textBox);
            headlineLabel_of_referenceTextBox_dict.Add(KeepScps_level_4_label, StandardKeepTopLevel_4_SCPs_textBox);
            headlineLabel_of_referenceTextBox_dict.Add(ScpInteractionsLevel_2_label, DynamicTopPercentScpsLevel_2_SCPs_textBox);
            headlineLabel_of_referenceTextBox_dict.Add(ScpInteractionsLevel_3_label, DynamicTopPercentScpsLevel_3_SCPs_textBox);

            System.Windows.Forms.Label[] headlineLabels = headlineLabel_of_referenceTextBox_dict.Keys.ToArray();
            OwnTextBox referenceTextBox;
            int distance_to_left = StandardPvalue_textBox.Location.X - StandardKeepTopLevel_4_SCPs_textBox.Location.X - StandardKeepTopLevel_4_SCPs_textBox.Width;
            int distance_to_right = EnrichmentSettings_panel.Width - StandardPvalue_textBox.Location.X - StandardPvalue_textBox.Width;
            int distance = Math.Min(distance_to_left, distance_to_right);
            if (distance < 0) { throw new Exception(); }
            foreach (System.Windows.Forms.Label headlineLabel in headlineLabels)
            {
                referenceTextBox = headlineLabel_of_referenceTextBox_dict[headlineLabel];
                left_reference_border = referenceTextBox.Location.X - distance;
                right_reference_border = referenceTextBox.Location.X + referenceTextBox.Width + distance;
                top_reference_border = (int)Math.Round(0.5 * referenceTextBox.Location.Y);
                bottom_reference_border = referenceTextBox.Location.Y;
                my_label = headlineLabel;
                Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }

            referenceTextBox = StandardPvalue_textBox;
            left_reference_border = referenceTextBox.Location.X - distance;
            right_reference_border = referenceTextBox.Location.X + referenceTextBox.Width + distance;
            top_reference_border = 0;
            bottom_reference_border = referenceTextBox.Location.Y;
            my_label = MaxPvalue_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            Update_maxRank_label(mbco_options);

            int distance_to_bottom_cutoffPanel = Cutoffs_panel.Height - DynamicKeepTopLevel_2_SCPs_textBox.Location.Y - DynamicKeepTopLevel_2_SCPs_textBox.Height;
            int height_for_each_label = (int)Math.Round(0.5F * (float)(distance_to_bottom_cutoffPanel + DynamicKeepTopLevel_2_SCPs_textBox.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Height - StandardKeepTopLevel_1_SCPs_textBox.Location.Y));

            left_position = 0;
            right_position = StandardKeepTopLevel_1_SCPs_textBox.Location.X;
            top_position = StandardKeepTopLevel_1_SCPs_textBox.Location.Y - halfdistanceHeight_between_enrichmetSettings_textBoxes;
            bottom_position = StandardKeepTopLevel_1_SCPs_textBox.Location.Y + StandardKeepTopLevel_1_SCPs_textBox.Height + halfdistanceHeight_between_enrichmetSettings_textBoxes;
            my_label = StandardKeepTopScps_label;
            StandardKeepTopScps_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_position, right_position, top_position, bottom_position);

            left_position = 0;
            right_position = StandardKeepTopLevel_1_SCPs_textBox.Location.X;
            top_position = DynamicKeepTopLevel_2_SCPs_textBox.Location.Y - halfdistanceHeight_between_enrichmetSettings_textBoxes;
            bottom_position = DynamicKeepTopLevel_2_SCPs_textBox.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Height + halfdistanceHeight_between_enrichmetSettings_textBoxes;
            my_label = DynamicKeepTopScps_label;
            DynamicKeepTopScps_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_position, right_position, top_position, bottom_position);

            left_reference_border = 0;
            right_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X;
            top_reference_border = (int)Math.Round(0.2 * StandardKeepTopLevel_1_SCPs_textBox.Location.Y);
            bottom_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.Y;
            my_label = KeepScpsScpLevel_label; //currently no text
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_lowerYPostion(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region EnrichmentSettings Scp interactions textBox, label, default button
            OwnTextBox[] dynamicTopPercentScpsLevel_textBoxes = new OwnTextBox[] { DynamicTopPercentScpsLevel_2_SCPs_textBox, DynamicTopPercentScpsLevel_3_SCPs_textBox };

            right_reference_border = (int)Math.Round(0.6F * this.ScpTopInteractions_panel.Width);
            top_reference_border = (int)Math.Round(0.4F * this.ScpTopInteractions_panel.Height);
            bottom_reference_border = top_reference_border + shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes;
            int distance_defaultButton_from_topPercentTextBoxes = (int)Math.Round(0.02F*(float)this.ScpTopInteractions_panel.Width);

            foreach (OwnTextBox dynamicTopPercentScpsLevel_textBox in dynamicTopPercentScpsLevel_textBoxes)
            {
                left_reference_border = right_reference_border;
                right_reference_border = left_reference_border + shared_width_of_enrichmentSettingsKeep_scpInteractions_textBoxes;
                my_textBox = dynamicTopPercentScpsLevel_textBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }

            left_position = 0;
            right_position = DynamicTopPercentScpsLevel_2_SCPs_textBox.Location.X;
            top_position = (int)Math.Round(0.73*DynamicTopPercentScpsLevel_2_SCPs_textBox.Location.Y);
            bottom_position = ScpTopInteractions_panel.Height;
            my_label = PercentDynamicTopSCPInteractions_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_position, right_position, top_position, bottom_position);

            left_position = 0;
            right_position = DynamicTopPercentScpsLevel_2_SCPs_textBox.Location.X;
            top_position = 0;
            bottom_position = PercentDynamicTopSCPInteractions_label.Location.Y;
            my_label = ScpInteractionsLevel_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_lowerYPostion(my_label, left_position, right_position, top_position, bottom_position);

            referenceTextBox = DynamicTopPercentScpsLevel_2_SCPs_textBox;
            left_reference_border = referenceTextBox.Location.X - 11;
            right_reference_border = referenceTextBox.Location.X + referenceTextBox.Width + 11;
            top_reference_border = 0;
            bottom_reference_border = referenceTextBox.Location.Y;
            my_label = ScpInteractionsLevel_2_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            referenceTextBox = DynamicTopPercentScpsLevel_3_SCPs_textBox;
            left_reference_border = referenceTextBox.Location.X - 11;
            right_reference_border = referenceTextBox.Location.X + referenceTextBox.Width + 11;
            top_reference_border = 0;
            bottom_reference_border = referenceTextBox.Location.Y;
            my_label = ScpInteractionsLevel_3_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_attach_to_lower_reference_at_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = DynamicTopPercentScpsLevel_3_SCPs_textBox.Location.X + DynamicTopPercentScpsLevel_3_SCPs_textBox.Width + distance_defaultButton_from_topPercentTextBoxes;
            right_reference_border = ScpTopInteractions_panel.Width;
            top_reference_border = DynamicTopPercentScpsLevel_3_SCPs_textBox.Location.Y;
            bottom_reference_border = DynamicTopPercentScpsLevel_3_SCPs_textBox.Location.Y + DynamicTopPercentScpsLevel_3_SCPs_textBox.Height;
            my_button = Default_button;
            Form_default_settings.Button_2nd_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            Set_cutoff_explanation_label_fontSize_and_position(mbco_options.Show_all_and_only_selected_scps);
            #endregion

            #region EnrichmentSettings GO hyperparameter
            Dictionary<OwnTextBox, Label> go_size_ownTextBox_label_dict = new Dictionary<OwnTextBox, Label>();
            go_size_ownTextBox_label_dict.Add(GO_sizeMin_ownTextBox, GO_size_min_label);
            go_size_ownTextBox_label_dict.Add(GO_sizeMax_ownTextBox, GO_size_max_label);
            OwnTextBox[] go_size_textBoxes = go_size_ownTextBox_label_dict.Keys.ToArray();
            OwnTextBox go_size_textBox;
            int go_size_textBoxes_length = go_size_textBoxes.Length;
            int distance_between_go_textBoxes = (int)Math.Round(0.025 * GO_hyperparameter_panel.Width);
            int shared_width_of_enrichmentSettingsKeep_goHyperparameter_textBoxes = (int)Math.Round(0.12F * GO_hyperparameter_panel.Width);

            for (int indexGo=0; indexGo<go_size_textBoxes_length; indexGo++)
            {
                go_size_textBox = go_size_textBoxes[indexGo];
                left_reference_border = indexGo * (distance_between_go_textBoxes + shared_width_of_enrichmentSettingsKeep_goHyperparameter_textBoxes) + (int)Math.Round(0.7F * GO_hyperparameter_panel.Width);
                right_reference_border = left_reference_border + shared_width_of_enrichmentSettingsKeep_goHyperparameter_textBoxes;
                top_reference_border = (int)Math.Round(  (float)this.GO_hyperparameter_panel.Height
                                                       - (float)shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes
                                                       - 0.1F * this.GO_hyperparameter_panel.Height);
                bottom_reference_border = top_reference_border + shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes;
                my_textBox = go_size_textBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

                left_reference_border -= (int)Math.Round(1F * shared_distance_between_keepAndPvalue_textBoxes);
                right_reference_border += (int)Math.Round(1F * shared_distance_between_keepAndPvalue_textBoxes);
                top_reference_border = 0;
                bottom_reference_border = go_size_textBox.Location.Y;
                my_label = go_size_ownTextBox_label_dict[go_size_textBox];
                Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }

            left_reference_border = 0;
            right_reference_border = GO_sizeMin_ownTextBox.Location.X;
            top_reference_border = 0;
            bottom_reference_border = (int)Math.Round(0.5F * GO_hyperparameter_panel.Height);
            my_label = GO_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_attachTo_leftSide_and_topSide(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;
            right_reference_border = GO_sizeMin_ownTextBox.Location.X;
            top_reference_border = GO_sizeMax_ownTextBox.Location.Y;
            bottom_reference_border = top_reference_border + GO_sizeMax_ownTextBox.Height;
            my_label = GO_size_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;
            right_reference_border = GO_size_label.Location.X;
            top_reference_border = GO_headline_label.Location.Y + GO_headline_label.Height;
            bottom_reference_border = GO_hyperparameter_panel.Height;
            my_label = GO_explanation_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region Define outputs panel
            left_reference_border = 0;
            right_reference_border = Overall_panel.Width;
            top_reference_border = EnrichmentSettings_panel.Location.Y + EnrichmentSettings_panel.Height;
            bottom_reference_border = (int)Math.Round(0.9F * this.Overall_panel.Height);
            my_panel = DefineOutputs_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel,left_reference_border,right_reference_border,top_reference_border, bottom_reference_border);
            #endregion

            #region Define outputs: specify shared variables
            int bardiagram_defineOutputs_checkBox_location_y = 0;
            int heatmap_defineOutputs_checkBox_location_y = (int)Math.Round(0.1F * this.DefineOutputs_panel.Height);
            int timeline_defineOutputs_checkBox_location_y = (int)Math.Round(0.5F * this.DefineOutputs_panel.Height);
            int shared_max_right_referenceBorder_defineOutputs_checkBoxes = (int)Math.Round(0.55F * this.DefineOutputs_panel.Width);
            int shared_heightWidth_of_defineOutputs_checkBoxButtons = (int)Math.Round(0.1F * this.DefineOutputs_panel.Height);
            int shared_left_referenceBorders_for_mainCheck_boxes = (int)Math.Round(0.02F*this.DefineOutputs_panel.Width);
            int shared_left_referenceBorders_for_subCheck_boxes = (int)Math.Round(0.05F * this.DefineOutputs_panel.Width);
            int shared_left_referenceBorders_for_textBoxes = (int)Math.Round(0.35F * DefineOutputs_panel.Width);
            int shared_textBoxCheckBox_height_defineOutputs = shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes;
            #endregion

            #region Define outputs: Bardiagram checkBox button and label
            left_reference_border = shared_left_referenceBorders_for_mainCheck_boxes;
            top_reference_border = bardiagram_defineOutputs_checkBox_location_y;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateBardiagrams_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button,left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateBardiagrams_cbButton.Location.X + GenerateBardiagrams_cbButton.Width;
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            my_label = GenerateBardiagrams_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region Define outputs: Heatmap checkBox buttons and labels
            left_reference_border = shared_left_referenceBorders_for_mainCheck_boxes;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            top_reference_border = heatmap_defineOutputs_checkBox_location_y;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateHeatmaps_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateHeatmaps_cbButton.Location.X + GenerateHeatmaps_cbButton.Width;
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            my_label = GenerateHeatmaps_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = shared_left_referenceBorders_for_subCheck_boxes;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            top_reference_border = heatmap_defineOutputs_checkBox_location_y + 1 * shared_heightWidth_of_defineOutputs_checkBoxButtons;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateHeatmapShowRanks_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateHeatmapShowRanks_cbButton.Location.X + GenerateHeatmapShowRanks_cbButton.Width;
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            my_label = GenerateHeatmapShowRanks_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = shared_left_referenceBorders_for_subCheck_boxes;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            top_reference_border = heatmap_defineOutputs_checkBox_location_y + 2 * shared_heightWidth_of_defineOutputs_checkBoxButtons;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateHeatmapShowMinusLog10Pvalues_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateHeatmapShowMinusLog10Pvalues_cbButton.Location.X + GenerateHeatmapShowMinusLog10Pvalues_cbButton.Width;
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            my_label = GenerateHeatmapShowMinusLog10Pvalues_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = shared_left_referenceBorders_for_subCheck_boxes;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            top_reference_border = heatmap_defineOutputs_checkBox_location_y + 3 * shared_heightWidth_of_defineOutputs_checkBoxButtons;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateHeatmapShowMinusLog10Pvalues_cbButton.Location.X + GenerateHeatmapShowMinusLog10Pvalues_cbButton.Width;
            right_reference_border = this.DefineOutputs_panel.Width;
            bottom_reference_border = timeline_defineOutputs_checkBox_location_y;
            my_label = GenerateHeatmapShowSignificantSCPsInAllDatasets_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region Define outputs: Timeline checkboxes and textboxes
            left_reference_border = shared_left_referenceBorders_for_mainCheck_boxes;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            top_reference_border = timeline_defineOutputs_checkBox_location_y;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateTimeline_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateTimeline_cbButton.Location.X + GenerateTimeline_cbButton.Width;
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            my_label = GenerateTimeline_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = (int)Math.Round(0.3F * DefineOutputs_panel.Width);
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            top_reference_border = GenerateTimeline_cbButton.Location.Y + GenerateTimeline_cbButton.Height;
            bottom_reference_border = top_reference_border + shared_height_of_enrichmentSetting_textBoxes;
            my_textBox = GenerateTimelinePvalue_textBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = shared_left_referenceBorders_for_subCheck_boxes;
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            top_reference_border = GenerateTimelinePvalue_textBox.Location.Y + GenerateTimelinePvalue_textBox.Height;
            bottom_reference_border = top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = GenerateTimelineLogScale_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = GenerateTimelineLogScale_cbButton.Location.X + GenerateTimelineLogScale_cbButton.Width;
            right_reference_border = this.DefineOutputs_panel.Width;
            my_label = GenerateTimelineLogScale_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region Define output: SaveFiguresAs ownListBoxes
            left_reference_border = shared_left_referenceBorders_for_textBoxes;
            right_reference_border = shared_max_right_referenceBorder_defineOutputs_checkBoxes;
            int half_left_over_height = (int)Math.Round(0.5F * (this.DefineOutputs_panel.Height - GenerateTimelineLogScale_cbButton.Location.Y - GenerateTimelineLogScale_cbButton.Height));

            top_reference_border = GenerateTimelineLogScale_cbButton.Location.Y + GenerateTimelineLogScale_cbButton.Height;
            bottom_reference_border = top_reference_border + half_left_over_height;
            my_listBox = SaveFiguresAs_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            top_reference_border = SaveFiguresAs_ownListBox.Location.Y + SaveFiguresAs_ownListBox.Height;
            bottom_reference_border = top_reference_border + half_left_over_height;
            my_listBox = ChartsPerPage_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region DefineOutputs: labels
            int space_per_line;
            int shared_left_reference_border = Set_timeline_explanation_label_fontSize_and_position_and_return_sharedLeftBorder(out space_per_line);

            left_reference_border = shared_left_reference_border;
            right_reference_border = DefineOutputs_panel.Width;
            top_reference_border = 0;
            bottom_reference_border = top_reference_border + 2*space_per_line;
            my_label = GenerateBardiagramsExplanation_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = shared_left_reference_border;
            right_reference_border = DefineOutputs_panel.Width;
            top_reference_border = GenerateBardiagramsExplanation_label.Location.Y + GenerateBardiagramsExplanation_label.Height;
            bottom_reference_border = GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Location.Y;
            my_label = GenerateHeatmapsExplanation_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = GenerateTimelinePvalue_textBox.Location.X;
            top_reference_border = GenerateTimelinePvalue_textBox.Location.Y;
            bottom_reference_border = GenerateTimelinePvalue_textBox.Location.Y + GenerateTimelinePvalue_textBox.Height;
            my_label = GenerateTimelinePvalue_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = SaveFiguresAs_ownListBox.Location.X + SaveFiguresAs_ownListBox.Width;// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = DefineOutputs_panel.Width;
            top_reference_border = SaveFiguresAs_ownListBox.Location.Y;
            bottom_reference_border = ChartsPerPage_ownListBox.Location.Y + ChartsPerPage_ownListBox.Height;
            my_label = SaveFiguresAsExplanation_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            int explanation_y_distance = 0;
            left_reference_border = 0;// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = SaveFiguresAs_ownListBox.Location.X;
            top_reference_border = SaveFiguresAs_ownListBox.Location.Y - explanation_y_distance;
            bottom_reference_border = SaveFiguresAs_ownListBox.Location.Y + SaveFiguresAs_ownListBox.Height + explanation_y_distance;
            my_label = SafeFigures_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            explanation_y_distance = 0;
            left_reference_border = 0;// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = ChartsPerPage_ownListBox.Location.X;
            top_reference_border = ChartsPerPage_ownListBox.Location.Y - explanation_y_distance;
            bottom_reference_border = ChartsPerPage_ownListBox.Location.Y + ChartsPerPage_ownListBox.Height + explanation_y_distance;
            my_label = ChartsPerPage_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            #region Color panel, checkboxes and label          
            left_reference_border = 0;// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = (int)Math.Round(0.7F*Overall_panel.Width);
            top_reference_border = DefineOutputs_panel.Location.Y + DefineOutputs_panel.Height;
            bottom_reference_border = Overall_panel.Height;
            my_panel = Colors_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            top_reference_border = (int)Math.Round(0.4F * Colors_panel.Height);
            left_reference_border = (int)Math.Round(0.02F * Colors_panel.Width);
            bottom_reference_border = Math.Min(Colors_panel.Height, top_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons);
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons;
            my_cb_button = ColorByLevel_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = ColorByLevel_cbButton.Location.X + ColorByLevel_cbButton.Width;
            right_reference_border = (int)Math.Round(0.475F * Colors_panel.Width);
            my_label = ColorByLevel_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = (int)Math.Round(0.475F * Colors_panel.Width);
            right_reference_border = left_reference_border + shared_heightWidth_of_defineOutputs_checkBoxButtons; 
            my_cb_button = ColorByDatasetColor_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cb_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = ColorByDatasetColor_cbButton.Location.X + ColorByDatasetColor_cbButton.Width;
            right_reference_border = Colors_panel.Width - (int)Math.Round(0.02F * Colors_panel.Width);
            my_label = ColorByDatasetColor_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = Colors_panel.Width;
            top_reference_border = 0;
            bottom_reference_border = ColorByLevel_cbButton.Location.Y;
            my_label = ColorBarsTimelines_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            int distance_y_for_buttons = (int)Math.Round(0.005 * Overall_panel.Height);
            int y_space_for_buttons = Overall_panel.Height - DefineOutputs_panel.Location.Y - DefineOutputs_panel.Height - 3 * distance_y_for_buttons;
            int button_y_size = (int)Math.Round(0.5 * y_space_for_buttons);

            left_reference_border = Colors_panel.Width + (int)Math.Round(0.02F * Overall_panel.Width);// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = Overall_panel.Width - (int)Math.Round(0.02F * Overall_panel.Width);
            top_reference_border = DefineOutputs_panel.Location.Y + DefineOutputs_panel.Height + distance_y_for_buttons;
            bottom_reference_border = top_reference_border + button_y_size;
            my_button = Tutorial_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            top_reference_border = bottom_reference_border + distance_y_for_buttons;
            bottom_reference_border = top_reference_border + button_y_size;
            my_button = Explanation_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

        }
        private void Update_ontology_label(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            int left_position = 0;
            int right_position = Ontology_panel.Width;
            int top_position = 0;
            int bottom_position = Ontology_panel.Height;

            Ontology_label.TextAlign = ContentAlignment.MiddleCenter;
            Ontology_label.Text = Ontology_classification_class.Get_name_of_ontology_plus_organism_without_underlines(mbco_options.Next_ontology, mbco_options.Next_organism);
            Ontology_label = Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_y(Ontology_label, left_position, right_position, top_position, bottom_position);
        }
        private void Set_cutoff_explanation_label_fontSize_and_position(bool scpTopInteractions_panel_is_visible)
        {
            int left_reference_border;
            int right_reference_border;
            int top_reference_border;
            int bottom_reference_border;
            left_reference_border = 0;
            right_reference_border = EnrichmentSettings_panel.Width;
            top_reference_border = Cutoffs_panel.Location.Y + Cutoffs_panel.Height;
            CutoffsExplanation_myPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            CutoffsExplanation_myPanelLabel.Font_style = System.Drawing.FontStyle.Italic;

            if (scpTopInteractions_panel_is_visible)
            {
                bottom_reference_border = ScpTopInteractions_panel.Location.Y - (int)Math.Round(0.05 * Cutoffs_panel.Height);
            }
            else
            {
                bottom_reference_border = (int)Math.Round(0.9 * EnrichmentSettings_panel.Height);
            }
            CutoffsExplanation_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_reference_border, top_reference_border, right_reference_border, bottom_reference_border, Form_default_settings);
        }
        private int Set_timeline_explanation_label_fontSize_and_position_and_return_sharedLeftBorder(out int space_per_line)
        {
            space_per_line = (int)Math.Round(0.075F*this.DefineOutputs_panel.Height);
            int shared_left_reference_border = Math.Max(Math.Max(GenerateBardiagrams_cbButton.Location.X + GenerateBardiagrams_cbButton.Width,
                                                                 GenerateHeatmaps_cbButton.Location.X + GenerateHeatmaps_cbButton.Width),
                                                                 GenerateTimelinePvalue_textBox.Location.X + GenerateTimelinePvalue_textBox.Width);
            int left_reference_border = shared_left_reference_border;
            int right_reference_border = DefineOutputs_panel.Width;
            int top_reference_border = GenerateTimeline_cbButton.Location.Y;
            int bottom_reference_border = top_reference_border + 3 * space_per_line;
            System.Windows.Forms.Label my_label = GenerateTimelineExplanation_label;
            GenerateTimelineExplanation_label = Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            return shared_left_reference_border;
        }

        private void Update_maxRank_label(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            bool is_mbco_ontology = Ontology_classification_class.Is_mbco_ontology(mbco_options.Next_ontology);
            int left_reference_border;
            int right_reference_border;
            int top_reference_border;
            int bottom_reference_border;
            if (is_mbco_ontology)
            {
                MaxRanks_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Max ranks for levels");
                left_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X - (int)Math.Round(0.2F * Cutoffs_panel.Width);
                right_reference_border = StandardKeepTopLevel_4_SCPs_textBox.Location.X + StandardKeepTopLevel_4_SCPs_textBox.Width;
                top_reference_border = 0;
                bottom_reference_border = KeepScps_level_1_label.Location.Y;
                MaxRanks_myPanelLabel.Font_style = System.Drawing.FontStyle.Bold;
                MaxRanks_myPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                MaxRanks_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_reference_border, top_reference_border, right_reference_border, bottom_reference_border, Form_default_settings);
            }
            else
            {
                MaxRanks_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Max rank");
                int half_extra_width = (int)Math.Round(0.5F * (MaxPvalue_label.Width - StandardKeepTopLevel_1_SCPs_textBox.Width));
                left_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X - half_extra_width;
                right_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X + StandardKeepTopLevel_1_SCPs_textBox.Width + half_extra_width;
                top_reference_border = 0;
                bottom_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.Y;
                MaxRanks_myPanelLabel.Font_style = System.Drawing.FontStyle.Bold;
                MaxRanks_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_reference_border, top_reference_border, right_reference_border, bottom_reference_border, Form_default_settings);
            }
        }
        public void Update_enrichment_options(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            Update_ontology_label(mbco_options);
            Ontology_type_enum ontology = mbco_options.Next_ontology;
            bool visualize_mbco_specific_elements = ontology.Equals(Ontology_type_enum.Mbco);
            bool visualize_go_specific_elements = Ontology_classification_class.Is_go_ontology(ontology);
            if (Global_class.ProcessLevel_for_all_non_MBCO_SCPs!=1) { throw new Exception(); }
            this.DynamicKeepTopLevel_2_SCPs_textBox.Visible = visualize_mbco_specific_elements;
            this.DynamicKeepTopLevel_3_SCPs_textBox.Visible = visualize_mbco_specific_elements;
            this.DynamicPvalue_textBox.Visible = visualize_mbco_specific_elements;
            this.DynamicKeepTopScps_label.Visible = visualize_mbco_specific_elements;
            this.StandardKeepTopLevel_2_SCPs_textBox.Visible = visualize_mbco_specific_elements;
            this.StandardKeepTopLevel_3_SCPs_textBox.Visible = visualize_mbco_specific_elements;
            this.StandardKeepTopLevel_4_SCPs_textBox.Visible = visualize_mbco_specific_elements;
            this.KeepScps_level_1_label.Visible = visualize_mbco_specific_elements;
            this.KeepScps_level_2_label.Visible = visualize_mbco_specific_elements;
            this.KeepScps_level_3_label.Visible = visualize_mbco_specific_elements;
            this.KeepScps_level_4_label.Visible = visualize_mbco_specific_elements;

            this.GO_hyperparameter_panel.Visible = visualize_go_specific_elements;


            Update_maxRank_label(mbco_options);
            if (mbco_options.Show_all_and_only_selected_scps)
            {
                this.Cutoffs_panel.Visible = true;
                this.DynamicKeepTopLevel_2_SCPs_textBox.Visible = false;
                this.DynamicKeepTopLevel_3_SCPs_textBox.Visible = false;
                this.DynamicKeepTopScps_label.Visible = false;
                this.DynamicPvalue_textBox.Visible = false;
                this.ScpTopInteractions_panel.Visible = false;
                CutoffsExplanation_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("All/only selected SCPs will be shown. Cutoffs only determine colors in heatmaps. Switch off under 'Select SCPs'.");
                //Significance cutoffs will be ignored, only selected SCPs will be shown. Switch off under 'Select SCPs'
                Set_cutoff_explanation_label_fontSize_and_position(visualize_go_specific_elements);
            }
            else
            {
                this.Cutoffs_panel.Visible = true;
                this.DynamicKeepTopLevel_2_SCPs_textBox.Visible = visualize_mbco_specific_elements;
                this.DynamicKeepTopLevel_3_SCPs_textBox.Visible = visualize_mbco_specific_elements;
                this.DynamicKeepTopScps_label.Visible = visualize_mbco_specific_elements;
                this.DynamicPvalue_textBox.Visible = visualize_mbco_specific_elements;
                this.ScpTopInteractions_panel.Visible = visualize_mbco_specific_elements;
                CutoffsExplanation_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Significant SCPs: rank && p-value <= cutoffs");
                Set_cutoff_explanation_label_fontSize_and_position(visualize_mbco_specific_elements|visualize_go_specific_elements);
            }
            this.DynamicKeepTopLevel_2_SCPs_textBox.SilentText = mbco_options.Keep_top_predictions_dynamicEnrichment_per_level[2].ToString();
            this.DynamicKeepTopLevel_2_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.DynamicKeepTopLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.DynamicKeepTopLevel_3_SCPs_textBox.SilentText = mbco_options.Keep_top_predictions_dynamicEnrichment_per_level[3].ToString();
            this.DynamicKeepTopLevel_3_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.DynamicKeepTopLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.StandardKeepTopLevel_1_SCPs_textBox.SilentText = mbco_options.Keep_top_predictions_standardEnrichment_per_level[1].ToString();
            this.StandardKeepTopLevel_1_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.StandardKeepTopLevel_2_SCPs_textBox.SilentText = mbco_options.Keep_top_predictions_standardEnrichment_per_level[2].ToString();
            this.StandardKeepTopLevel_2_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.StandardKeepTopLevel_3_SCPs_textBox.SilentText = mbco_options.Keep_top_predictions_standardEnrichment_per_level[3].ToString();
            this.StandardKeepTopLevel_3_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.StandardKeepTopLevel_4_SCPs_textBox.SilentText = mbco_options.Keep_top_predictions_standardEnrichment_per_level[4].ToString();
            this.StandardKeepTopLevel_4_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            float top_quantil_level_2 = mbco_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[2];
            this.DynamicTopPercentScpsLevel_2_SCPs_textBox.SilentText = (top_quantil_level_2 * 100).ToString();
            this.DynamicTopPercentScpsLevel_2_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.DynamicTopPercentScpsLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.DynamicTopPercentScpsLevel_3_SCPs_textBox.SilentText = (mbco_options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[3] * 100).ToString();
            this.DynamicTopPercentScpsLevel_3_SCPs_textBox.TextAlign = HorizontalAlignment.Center;
            this.DynamicTopPercentScpsLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor;

            if (Ontology_classification_class.Is_go_ontology(mbco_options.Next_ontology))
            {
                this.GO_sizeMin_ownTextBox.SilentText = mbco_options.Get_go_min_size_string(mbco_options.Next_ontology);
                this.GO_sizeMin_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
                this.GO_sizeMin_ownTextBox.TextAlign = HorizontalAlignment.Center;
                this.GO_sizeMax_ownTextBox.SilentText = mbco_options.Get_go_max_size_string(mbco_options.Next_ontology);
                this.GO_sizeMax_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
                this.GO_sizeMax_ownTextBox.TextAlign = HorizontalAlignment.Center;
            }

            this.StandardPvalue_textBox.SilentText = mbco_options.Max_pvalue_for_standardEnrichment.ToString();
            this.StandardPvalue_textBox.TextAlign = HorizontalAlignment.Center;
            this.StandardPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.DynamicPvalue_textBox.SilentText = mbco_options.Max_pvalue_for_dynamicEnrichment.ToString();
            this.DynamicPvalue_textBox.TextAlign = HorizontalAlignment.Center;
            this.DynamicPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor;

            this.GenerateTimelinePvalue_textBox.SilentText = mbco_options.Timeline_pvalue_cutoff.ToString();
            this.GenerateTimelinePvalue_textBox.Refresh();
        }
        public void Update_allOptions(MBCO_enrichment_pipeline_options_class mbco_options, Bardiagram_options_class bar_options, Heatmap_options_class heatmap_options, Timeline_options_class time_options)
        {
            Update_enrichment_options(mbco_options);
            this.GenerateBardiagrams_cbButton.SilentChecked = bar_options.Generate_bardiagrams;
            this.GenerateTimeline_cbButton.SilentChecked = time_options.Generate_timeline;
            this.GenerateTimelinePvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.GenerateTimelineLogScale_cbButton.SilentChecked = time_options.Is_logarithmic_time_axis;
            this.ColorByDatasetColor_cbButton.SilentChecked = bar_options.Customized_colors;
            if (time_options.Customized_colors != bar_options.Customized_colors) { throw new Exception(); }
            this.GenerateHeatmaps_cbButton.SilentChecked = heatmap_options.Generate_heatmap;
            this.GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton.SilentChecked = heatmap_options.Show_significant_scps_over_all_conditions;
            switch (heatmap_options.Value_type_selected_for_visualization)
            {
                case Enrichment_value_type_enum.Minus_log10_pvalue:
                    this.GenerateHeatmapShowMinusLog10Pvalues_cbButton.SilentChecked = true;
                    this.GenerateHeatmapShowRanks_cbButton.SilentChecked = false;
                    break;
                case Enrichment_value_type_enum.Fractional_rank:
                    this.GenerateHeatmapShowMinusLog10Pvalues_cbButton.SilentChecked = false;
                    this.GenerateHeatmapShowRanks_cbButton.SilentChecked = true;
                    break;
                default:
                    throw new Exception();
            }
            this.ColorByLevel_cbButton.SilentChecked = !this.ColorByDatasetColor_cbButton.Checked;

            if (bar_options.Write_pdf != time_options.Write_pdf) { throw new Exception(); }
            if (bar_options.Write_pdf != heatmap_options.Write_pdf) { throw new Exception(); }
            if (!bar_options.ImageFormat.Equals(time_options.ImageFormat)) { throw new Exception(); }
            if (!bar_options.ImageFormat.ToString().Equals(heatmap_options.ImageFormat.ToString())) { throw new Exception(); }
            if (bar_options.Write_pdf)
            {
                this.SaveFiguresAs_ownListBox.SilentSelectedIndex = SaveFiguresAs_ownListBox.Items.IndexOf("Pdf");
            }
            else
            {
                this.SaveFiguresAs_ownListBox.SilentSelectedIndex = SaveFiguresAs_ownListBox.Items.IndexOf(bar_options.ImageFormat);
            }
            var all_charts_per_page = Enum.GetValues(typeof(Charts_per_page_enum));
            if (!bar_options.Charts_per_page.Equals(time_options.Charts_per_page)) { throw new Exception(); }
            if (!bar_options.Charts_per_page.Equals(heatmap_options.Charts_per_page)) { throw new Exception(); }
            ChartsPerPage_ownListBox.Items.Clear();
            foreach (var charts_per_page in all_charts_per_page)
            {
                ChartsPerPage_ownListBox.Items.Add(charts_per_page);
            }
            this.ChartsPerPage_ownListBox.SilentSelectedIndex = this.ChartsPerPage_ownListBox.Items.IndexOf(bar_options.Charts_per_page);
        }

        public void Set_visibility(bool visible, MBCO_enrichment_pipeline_options_class mbco_options, Bardiagram_options_class bar_options, Heatmap_options_class heatmap_options, Timeline_options_class time_options)
        {
            this.Overall_panel.Visible = false;
            Update_allOptions(mbco_options, bar_options, heatmap_options, time_options);
            this.Overall_panel.Visible = visible;
            Update_panel();
        }
        private void Update_panel()
        {
            this.GenerateBardiagramsExplanation_label.Visible = this.GenerateBardiagrams_cbButton.Checked;
            this.GenerateBardiagramsExplanation_label.Refresh();
            this.GenerateTimelineExplanation_label.Text = EnrichmentOptions_text_class.Timeline_label_standard_text;
            int space_per_line;
            Set_timeline_explanation_label_fontSize_and_position_and_return_sharedLeftBorder(out space_per_line);
            this.GenerateTimelineExplanation_label.Visible = this.GenerateTimeline_cbButton.Checked;
            this.GenerateTimelineExplanation_label.Refresh();
            this.GenerateTimelinePvalue_textBox.Visible = this.GenerateTimeline_cbButton.Checked;
            this.GenerateTimelinePvalue_label.Visible = this.GenerateTimeline_cbButton.Checked;
            this.GenerateTimelineLogScale_cbButton.Visible = this.GenerateTimeline_cbButton.Checked;
            this.GenerateTimelineLogScale_cbLabel.Visible = this.GenerateTimelineLogScale_cbButton.Visible;
            this.GenerateHeatmapsExplanation_label.Visible = this.GenerateHeatmaps_cbButton.Checked;
            this.GenerateHeatmapsExplanation_label.Refresh();
            this.GenerateHeatmapShowMinusLog10Pvalues_cbButton.Visible = this.GenerateHeatmaps_cbButton.Checked;
            this.GenerateHeatmapShowMinusLog10Pvalues_cbLabel.Visible = this.GenerateHeatmapShowMinusLog10Pvalues_cbButton.Visible;
            this.GenerateHeatmapShowRanks_cbButton.Visible = this.GenerateHeatmaps_cbButton.Checked;
            this.GenerateHeatmapShowRanks_cbLabel.Visible = this.GenerateHeatmapShowRanks_cbButton.Visible;
            this.GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Visible = this.GenerateHeatmaps_cbButton.Checked;
            this.GenerateHeatmapShowSignificantSCPsInAllDatasets_cbLabel.Visible = this.GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Visible;
            this.ChartsPerPage_label.Visible = true;
            if (SaveFiguresAs_ownListBox.SelectedIndex.Equals(SaveFiguresAs_ownListBox.Items.IndexOf("Pdf"))) 
            { 
                this.SaveFiguresAsExplanation_label.Visible = true; 
            }
            else 
            { 
                this.SaveFiguresAsExplanation_label.Visible = false;
            }
            this.Overall_panel.Refresh();
        }

        #region Keep top SCPs
        private void Keep_top_scps_textBox_changed(OwnTextBox keep_top_textBox, int level, ref int[] keep_top_scps_per_level)
        {
            int new_cutoff = -1;
            if ((int.TryParse(keep_top_textBox.Text, out new_cutoff)) && (new_cutoff >= 0))
            {
                keep_top_scps_per_level[level] = new_cutoff;
            }
            else
            {
                keep_top_scps_per_level[level] = -1; //value lies outside of values that can be selected, so a mismatch is reported
            }
        }
        public MBCO_enrichment_pipeline_options_class Keep_top_scps_standard_of_indicated_level(int scp_level, MBCO_enrichment_pipeline_options_class options)
        {
            int[] keep_top_SCPs_per_level = new int[0];
            switch (scp_level)
            {
                case 1:
                    keep_top_SCPs_per_level = Array_class.Deep_copy_array(options.Keep_top_predictions_standardEnrichment_per_level);
                    Keep_top_scps_textBox_changed(this.StandardKeepTopLevel_1_SCPs_textBox, scp_level, ref keep_top_SCPs_per_level);
                    options.Keep_top_predictions_standardEnrichment_per_level = keep_top_SCPs_per_level;
                    if (Array_class.Arrays_are_equal(options.Keep_top_predictions_standardEnrichment_per_level, keep_top_SCPs_per_level))
                    { this.StandardKeepTopLevel_1_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.StandardKeepTopLevel_1_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                case 2:
                    keep_top_SCPs_per_level = Array_class.Deep_copy_array(options.Keep_top_predictions_standardEnrichment_per_level);
                    Keep_top_scps_textBox_changed(this.StandardKeepTopLevel_2_SCPs_textBox, scp_level, ref keep_top_SCPs_per_level);
                    options.Keep_top_predictions_standardEnrichment_per_level = keep_top_SCPs_per_level;
                    if (Array_class.Arrays_are_equal(options.Keep_top_predictions_standardEnrichment_per_level, keep_top_SCPs_per_level))
                    { this.StandardKeepTopLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.StandardKeepTopLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                case 3:
                    keep_top_SCPs_per_level = Array_class.Deep_copy_array(options.Keep_top_predictions_standardEnrichment_per_level);
                    Keep_top_scps_textBox_changed(this.StandardKeepTopLevel_3_SCPs_textBox, scp_level, ref keep_top_SCPs_per_level);
                    options.Keep_top_predictions_standardEnrichment_per_level = keep_top_SCPs_per_level;
                    if (Array_class.Arrays_are_equal(options.Keep_top_predictions_standardEnrichment_per_level, keep_top_SCPs_per_level))
                    { this.StandardKeepTopLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.StandardKeepTopLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                case 4:
                    keep_top_SCPs_per_level = Array_class.Deep_copy_array(options.Keep_top_predictions_standardEnrichment_per_level);
                    Keep_top_scps_textBox_changed(this.StandardKeepTopLevel_4_SCPs_textBox, scp_level, ref keep_top_SCPs_per_level);
                    options.Keep_top_predictions_standardEnrichment_per_level = keep_top_SCPs_per_level;
                    if (Array_class.Arrays_are_equal(options.Keep_top_predictions_standardEnrichment_per_level, keep_top_SCPs_per_level))
                    { this.StandardKeepTopLevel_4_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.StandardKeepTopLevel_4_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                default:
                    throw new Exception();
            }
            return options;
        }
        public MBCO_enrichment_pipeline_options_class Keep_top_scps_dynamic_of_indicated_level(int scp_level, MBCO_enrichment_pipeline_options_class options)
        {
            int[] keep_top_SCPs_per_level = new int[0];
            switch (scp_level)
            {
                case 2:
                    keep_top_SCPs_per_level = Array_class.Deep_copy_array(options.Keep_top_predictions_dynamicEnrichment_per_level);
                    Keep_top_scps_textBox_changed(this.DynamicKeepTopLevel_2_SCPs_textBox, scp_level, ref keep_top_SCPs_per_level);
                    options.Keep_top_predictions_dynamicEnrichment_per_level = keep_top_SCPs_per_level;
                    if (Array_class.Arrays_are_equal(options.Keep_top_predictions_dynamicEnrichment_per_level, keep_top_SCPs_per_level))
                    { this.DynamicKeepTopLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.DynamicKeepTopLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                case 3:
                    keep_top_SCPs_per_level = Array_class.Deep_copy_array(options.Keep_top_predictions_dynamicEnrichment_per_level);
                    Keep_top_scps_textBox_changed(this.DynamicKeepTopLevel_3_SCPs_textBox, scp_level, ref keep_top_SCPs_per_level);
                    options.Keep_top_predictions_dynamicEnrichment_per_level = keep_top_SCPs_per_level;
                    if (Array_class.Arrays_are_equal(options.Keep_top_predictions_dynamicEnrichment_per_level, keep_top_SCPs_per_level))
                    { this.DynamicKeepTopLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.DynamicKeepTopLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                default:
                    throw new Exception();
            }
            return options;
        }
        public MBCO_enrichment_pipeline_options_class Standard_pvalue(MBCO_enrichment_pipeline_options_class options)
        {
            float new_pvalue;
            if ((StandardPvalue_textBox.Text.Length > 0)
                && (!StandardPvalue_textBox.Text[StandardPvalue_textBox.Text.Length - 1].Equals('.'))
                && (float.TryParse(StandardPvalue_textBox.Text, out new_pvalue)))
            { }
            else { new_pvalue = -1; }//value lies outside of selectable values, so mismatch is reported 
            options.Max_pvalue_for_standardEnrichment = new_pvalue;
            if (options.Max_pvalue_for_standardEnrichment.Equals(new_pvalue))
            { StandardPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else
            { StandardPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return options;
        }
        public MBCO_enrichment_pipeline_options_class Dynamic_pvalue(MBCO_enrichment_pipeline_options_class options)
        {
            float new_pvalue = -1;
            if ((DynamicPvalue_textBox.Text.Length > 0)
                && (!DynamicPvalue_textBox.Text[DynamicPvalue_textBox.Text.Length - 1].Equals('.'))
                && (float.TryParse(DynamicPvalue_textBox.Text, out new_pvalue)))
            { }
            else { new_pvalue = -1; }//value lies outside of selectable values, so mismatch is reported
           options.Max_pvalue_for_dynamicEnrichment = new_pvalue;
           if (options.Max_pvalue_for_dynamicEnrichment.Equals(new_pvalue))
           { DynamicPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
           else { DynamicPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return options;
        }
        public Mbc_enrichment_fast_pipeline_class Reset_enrichment_options(Mbc_enrichment_fast_pipeline_class pipeline, Bardiagram_options_class bar_options, Heatmap_options_class heatmap_options, Timeline_options_class timeline_options)
        {
            pipeline.Options.Keep_top_predictions_dynamicEnrichment_per_level = Array_class.Deep_copy_array(pipeline.Options_default.Keep_top_predictions_dynamicEnrichment_per_level);
            pipeline.Options.Keep_top_predictions_standardEnrichment_per_level = Array_class.Deep_copy_array(pipeline.Options_default.Keep_top_predictions_standardEnrichment_per_level);
            pipeline.Options.Max_pvalue_for_standardEnrichment = pipeline.Options_default.Max_pvalue_for_standardEnrichment;
            pipeline.Options.Max_pvalue_for_dynamicEnrichment = pipeline.Options_default.Max_pvalue_for_dynamicEnrichment;
            pipeline.Options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = Array_class.Deep_copy_array(pipeline.Options_default.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
            Update_allOptions(pipeline.Options, bar_options, heatmap_options, timeline_options);
            return pipeline;
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
        public MBCO_enrichment_pipeline_options_class DynamicTopPercentScpsLevel_x_SCPs_textBox_TextChanged(int scp_level, MBCO_enrichment_pipeline_options_class options)
        {
            float[] dynamic_top_considered_quantile = new float[0];
            switch (scp_level)
            {
                case 2:
                    dynamic_top_considered_quantile = Array_class.Deep_copy_array(options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
                    this.DynamicTopPercentScpsLevel_2_SCPs_textBox = Consider_topPercentScpsLevel_changed(this.DynamicTopPercentScpsLevel_2_SCPs_textBox, scp_level, ref dynamic_top_considered_quantile);
                    options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = dynamic_top_considered_quantile;
                    if (Array_class.Arrays_are_equal(options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level, dynamic_top_considered_quantile))
                    { this.DynamicTopPercentScpsLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.DynamicTopPercentScpsLevel_2_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                case 3:
                    dynamic_top_considered_quantile = Array_class.Deep_copy_array(options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
                    this.DynamicTopPercentScpsLevel_3_SCPs_textBox = Consider_topPercentScpsLevel_changed(this.DynamicTopPercentScpsLevel_3_SCPs_textBox, scp_level, ref dynamic_top_considered_quantile);
                    options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = dynamic_top_considered_quantile;
                    if (Array_class.Arrays_are_equal(options.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level, dynamic_top_considered_quantile))
                    { this.DynamicTopPercentScpsLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                    else { this.DynamicTopPercentScpsLevel_3_SCPs_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                    break;
                default:
                    throw new Exception();
            }
            return options;
        }

        public MBCO_enrichment_pipeline_options_class GO_min0_max1_size_textBox_TextChanged(int min0_max1, MBCO_enrichment_pipeline_options_class options)
        {
            int value;
            bool invalid_value;
            for (int indexIteration = 0; indexIteration < 2; indexIteration++)
            {
                switch (min0_max1)
                {
                    case 0:
                        if (GO_sizeMin_ownTextBox.Text.Length == 0)
                        {
                            value = -1;
                            options.Set_go_min_size(options.Next_ontology, value);
                            if (options.Get_go_min_size(options.Next_ontology) != value) { invalid_value = true; }
                            else { invalid_value = false; }
                        }
                        else if (int.TryParse(GO_sizeMin_ownTextBox.Text, out value))
                        {
                            options.Set_go_min_size(options.Next_ontology, value);
                            if (options.Get_go_min_size(options.Next_ontology) != value) { invalid_value = true; }
                            else { invalid_value = false; }
                        }
                        else { invalid_value = true; }
                        if (!invalid_value)
                        { this.GO_sizeMin_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                        else { this.GO_sizeMin_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                        min0_max1 = 1;
                        break;
                    case 1:
                        if (GO_sizeMax_ownTextBox.Text.Length == 0)
                        {
                            value = -1;
                            options.Set_go_max_size(options.Next_ontology, value);
                            if (options.Get_go_max_size(options.Next_ontology) != value) { invalid_value = true; }
                            else { invalid_value = false; }
                        }
                        else if (int.TryParse(GO_sizeMax_ownTextBox.Text, out value))
                        {
                            options.Set_go_max_size(options.Next_ontology, value);
                            if (options.Get_go_max_size(options.Next_ontology) != value) { invalid_value = true; }
                            else { invalid_value = false; }
                        }
                        else { invalid_value = true; }
                        if (!invalid_value)
                        { this.GO_sizeMax_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor; }
                        else { this.GO_sizeMax_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
                        min0_max1 = 0;
                        break;
                    default:
                        throw new Exception();
                }
                if (!invalid_value)
                {
                    options.Clear_all_dictionaries_with_selected_scps_for_next_ontology();
                }
            }
            return options;
        }

        #endregion

        #region Color
        public bool ColorByLevel_ownCheckBox_CheckedChanged_and_return_if_colored_by_dataset()
        {
            ColorByDatasetColor_cbButton.SilentChecked = !ColorByLevel_cbButton.Checked;
            return ColorByDatasetColor_cbButton.Checked;
        }
        public bool ColorByDataset_ownCheckBox_CheckedChanged_and_return_if_colored_by_dataset()
        {
            ColorByLevel_cbButton.SilentChecked = !ColorByDatasetColor_cbButton.Checked;
            return ColorByDatasetColor_cbButton.Checked;
        }
        #endregion

        #region Heatmap
        public Heatmap_options_class GenerateHeatmaps_ownCheckBox_CheckedChanged(Heatmap_options_class heatmap_options)
        {
            heatmap_options.Generate_heatmap = GenerateHeatmaps_cbButton.Checked;
            Update_panel();
            return heatmap_options;
        }
        private Heatmap_options_class Set_value_type_for_visualization(Heatmap_options_class options)
        {
            if ((GenerateHeatmapShowMinusLog10Pvalues_cbButton.Checked) && (!GenerateHeatmapShowRanks_cbButton.Checked))
            {
                options.Value_type_selected_for_visualization = Enrichment_value_type_enum.Minus_log10_pvalue;
            }
            else if ((!GenerateHeatmapShowMinusLog10Pvalues_cbButton.Checked) && (GenerateHeatmapShowRanks_cbButton.Checked))
            {
                options.Value_type_selected_for_visualization = Enrichment_value_type_enum.Fractional_rank;
            }
            else { throw new Exception(); }
            return options;
        }
        public Heatmap_options_class GenerateHeatmapShowRanks_ownCheckBox_CheckedChanged(Heatmap_options_class options)
        {
            GenerateHeatmapShowMinusLog10Pvalues_cbButton.SilentChecked = !GenerateHeatmapShowRanks_cbButton.Checked;
            return Set_value_type_for_visualization(options);
        }
        public Heatmap_options_class GenerateHeatmapShowMinusLog10Pvalues_ownCheckBox_CheckedChanged(Heatmap_options_class options)
        {
            GenerateHeatmapShowRanks_cbButton.SilentChecked = !GenerateHeatmapShowMinusLog10Pvalues_cbButton.Checked;
            return Set_value_type_for_visualization(options);
        }
        public Heatmap_options_class GenerateHeatmapShowSignificantSCPsInAllDatasets_ownCheckBox_CheckedChanged(Heatmap_options_class options,MBCO_enrichment_pipeline_options_class enrichment_options)
        {
            options.Show_significant_scps_over_all_conditions = GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Checked;
            if (enrichment_options.Show_all_and_only_selected_scps)
            {
                options.Show_significant_scps_over_all_conditions = true;
                GenerateHeatmapShowSignificantSCPsInAllDatasets_cbButton.Checked = options.Show_significant_scps_over_all_conditions;
            }
            return options;
        }
        #endregion

        #region Timeline
        public Timeline_options_class GenerateTimeline_ownCheckBox_CheckedChanged(Timeline_options_class timeline_options, int number_of_different_timepoints)
        {
            if (number_of_different_timepoints<=1) 
            {
                GenerateTimeline_cbButton.SilentChecked = false;
                Update_panel();
                GenerateTimelineExplanation_label.Text = EnrichmentOptions_text_class.Timeline_label_only_one_timepoint;
                int space_per_line;
                Set_timeline_explanation_label_fontSize_and_position_and_return_sharedLeftBorder(out space_per_line);
                GenerateTimelineExplanation_label.Visible = true;
                GenerateTimelineExplanation_label.Refresh();
            }
            else
            {
                Update_panel();
            }
            timeline_options.Generate_timeline = GenerateTimeline_cbButton.Checked;
            return timeline_options;
        }
        public MBCO_enrichment_pipeline_options_class GenerateTimeline_pvalue_textBox_TextChanged(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            float new_pvalue = -1F;
            if (float.TryParse(GenerateTimelinePvalue_textBox.Text, out new_pvalue))
            { }
            else { new_pvalue = -1F; }
            mbco_options.Timeline_pvalue_cutoff = new_pvalue;
            if (mbco_options.Timeline_pvalue_cutoff.Equals(new_pvalue))
            { GenerateTimelinePvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor; }
            else { GenerateTimelinePvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value; }
            return mbco_options;
        }

        public Timeline_options_class Set_timeline_log_scale_to_false_and_update_corresponding_checkbox(Timeline_options_class options)
        {
            options.Is_logarithmic_time_axis = false;
            GenerateTimelineLogScale_cbButton.SilentChecked = options.Is_logarithmic_time_axis;
            GenerateTimelineExplanation_label.Text = EnrichmentOptions_text_class.Timeline_label_Log_only_allowed_if_all_values_larger_zero;
            int space_per_line;
            Set_timeline_explanation_label_fontSize_and_position_and_return_sharedLeftBorder(out space_per_line);
            return options;
        }

        public Timeline_options_class GenerateTimelineLogScale_checkBox_CheckedChanged(Custom_data_class custom_data, Timeline_options_class timeline_options)
        {
            if (GenerateTimelineLogScale_cbButton.Checked)
            {
                if (custom_data.Analyse_if_all_timepoints_larger_zero())
                {
                    timeline_options.Is_logarithmic_time_axis = GenerateTimelineLogScale_cbButton.Checked;
                }
                else
                {
                    timeline_options = Set_timeline_log_scale_to_false_and_update_corresponding_checkbox(timeline_options);
                }
            }
            else { timeline_options.Is_logarithmic_time_axis = GenerateTimelineLogScale_cbButton.Checked; }
            return timeline_options;
        }
        #endregion

        #region Bardiagram
        public Bardiagram_options_class GenerateBardiagram_ownCheckBox_CheckedChanged(Bardiagram_options_class bardiagram_options)
        {
            Update_panel();
            bardiagram_options.Generate_bardiagrams = GenerateBardiagrams_cbButton.Checked;
            return bardiagram_options;
        }
        #endregion

        #region Save figures
        public void SaveFiguresAs_ownListBox_SelectedIndexChanged(ref Bardiagram_options_class bardiagram_options, ref Heatmap_options_class heatmap_options, ref Timeline_options_class timeline_options)
        {
            if (SaveFiguresAs_ownListBox.SelectedItem.Equals("Pdf"))
            {
                bardiagram_options.Write_pdf = true;
                timeline_options.Write_pdf = true;
                heatmap_options.Write_pdf = true;
            }
            else
            {
                System.Drawing.Imaging.ImageFormat chartImageFormat = System.Drawing.Imaging.ImageFormat.Png;
                switch (SaveFiguresAs_ownListBox.SelectedItem.ToString().ToLower())
                {
                    case "png":
                        chartImageFormat = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case "jpeg":
                        chartImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case "bmp":
                        chartImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case "tiff":
                        chartImageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                    default:
                        throw new Exception();
                }

                bardiagram_options.Write_pdf = false;
                bardiagram_options.ImageFormat = chartImageFormat;
                timeline_options.Write_pdf = false;
                timeline_options.ImageFormat = chartImageFormat;
                heatmap_options.Write_pdf = false;
                heatmap_options.ImageFormat = chartImageFormat;
            }
            Update_panel();
        }

        public void ChartsPerPage_ownCheckBox_SelectedIndexChanged(ref Bardiagram_options_class bardiagram_options, ref Heatmap_options_class heatmap_options, ref Timeline_options_class timeline_options)
        {
            bardiagram_options.Charts_per_page = (Charts_per_page_enum)this.ChartsPerPage_ownListBox.SelectedItem;
            timeline_options.Charts_per_page = (Charts_per_page_enum)this.ChartsPerPage_ownListBox.SelectedItem;
            heatmap_options.Charts_per_page = (Charts_per_page_enum)this.ChartsPerPage_ownListBox.SelectedItem;
        }
        #endregion

        #region Explanation, tutorial
        public bool Is_explanation_tutorial_button_active(Button selected_button)
        {
            return selected_button.BackColor == Form_default_settings.Color_button_pressed_back;
        }
        public void Set_explanation_tutorial_button_to_not_selected()
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
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            Error_reports_headline_label.Text = "Enrichment analysis";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            string text = "Molecular Biology of the Cell Ontology (MBCO)" +
                "\r\nMBCO was developed based on standard cell biology and biochemistry textbooks (Alberts et al., Berg et al., Rosenthal and Glew) and reviews. It has a strict focus on cell biology. " +
                "Its subcellular processes (SCPs) were populated using a combination of PubMed abstract text mining and enrichment analysis, followed by computer-assisted manual validation of each gene-SCP " +
                "annotation." +
                Form_default_settings.Explanation_text_major_separator +
                "Hierarchical organization of MBCO" +
                "\r\nMBCO SPCs are hierarchically organized in parent-child relationships that typically span three to four levels. Higher levels (levels with lower numbers) " +
                "and lower levels contain SCPs that describe more general and more detailed cell biological functions, respectively. SCPs of the same level exhibit minimal functional redundancies, so that every " +
                "predicted SCP of the same level offers additional information." +
                Form_default_settings.Explanation_text_major_separator +
                "Inferred functional interactions between MBCO level-2 or -3 SCPs" +
                "\r\nThe annotated parent-child hierarchy of MBCO is enriched using " +
                "a unique MBCO algorithm that predicts weighted functional interactions between the SCPs of levels 2 or 3. Inferred from text mining results these interactions can connect SCPs with or without shared " +
                "annotated genes and with or without overlapping parent and grandparent SCPs. While hierarchical interactions only connect SCPs of different levels, the functional interactions only connect SCPs of the " +
                "same level. Within each level the interactions are ranked by their interaction strength. For details of the algorithm, please see Hansen et al. Sci Rep. 2017." +
                "\r\nTo explore the interactions, use the functionalities of the 'Ontologies / Species'-menu." +
                Form_default_settings.Explanation_text_major_separator +
                "Standard enrichment analysis" +
                "\r\nStandard enrichment analysis analyses, if list of experimentally obtained genes is enriched for genes annotated to a subcellular process (SCP) or pathway. It tests " +
                "one SCP/pathway at a time. The used right-tailed Fisher's exact test first calculates the overlap between the genes of the SCP and the experimentally obtained genes list. Afterwards, it calculates a p-value " +
                "that gives the probability to observe the same or a higher overlap, if it was by chance. All predicted SCPs of the same level and dataset will be ranked by significance, if MBCO is used for enrichment analysis. " +
                "If any other ontology is used, all predicted pathways will be ranked by significance, independent of the pathway level or depth." +
                Form_default_settings.Explanation_text_major_separator +
                "Dynamic enrichment analysis (only when using MBCO)" +
                "\r\nStandard enrichment " +
                "analysis analyses one SCP at a time for enrichment of experimentally obtained gene lists. In contrast, dynamic enrichment analysis determines if a set of merged functionally related SCPs enriches for the user-supplied genes." +
                "\r\nDynamic enrichment analysis depends on the availability of networks connecting functionally interacting SCPs and is currently implemented for MBCO level-2 and -3 SCPs. Here, it uses the inferred functional SCP " +
                "interactions described above." +
                "\r\n" + 
                "\r\nDynamic enrichment analysis first identifies all level-2 or -3 SCPs that contain at least one experimentally obtained gene. Identified SCPs are merged, if they are functionally related " +
                "based on the inferred SCP interactions. Any possible combination of two to three functionally related SCPs is added as a dataset-selective SCP combination to the MBC ontology to generate a dataset-selective ontology. " +
                "The added SCP combination contains all genes of the combined SCPs and is annotated to the same level. Its name is generated by concatenating the names of the combined SCPs." +
                "\r\n" +
                "\r\nAfter generation of the dataset-selective ontology dynamic enrichment analysis uses the same algorithms as standard enrichment analysis. SCPs and SCP combinations are tested for enrichment of user-supplied gene " +
                "lists using Fisher's Exact Test. Predictions (that are either single SCPs or SCP combinations) are ranked by significance for each dataset and SCP level." +
                "\r\n" +
                "\r\nThe algorithm is called dynamic enrichment analysis, because " +
                "the decision which dataset-selective SCP combinations are added to the ontology depends on the analyzed gene list and is made during runtime." +
                "\r\n" +
                "\r\nIn the result spreadsheets, SCPs of predicted SCP combinations are separated " +
                "by dollar signs. In the bar diagram figures, SCPs of one combination label one -log10(p-value) bar. Since one SCP can be part of multiple SCP combinations, heatmaps will show the most significant -log10(p-value) or the " +
                "highest rank (lowest rank number) of the SCP combinations it belongs to. The SCP networks will show all SCPs of a significant SCP combination and connect them. For more details about the networks, see the explanation text of the ‘SCP networks’-menu." +
                "\r\n" +
                "\r\nAdditional details about the dynamic enrichment algorithm are described in our publication Hansen et al. Sci Rep. 2017." +
                Form_default_settings.Explanation_text_major_separator +
                "Cutoff parameter" +
                "\r\nPathway or SCP predictions generated for each dataset are ranked by significance. " +
                "The user can specify a p-value and a significance rank cutoff. SCPs or pathways must meet both cutoffs to be defined as significant and visualized in all graphs or integrated in the same SCP/pathway networks." +
                "\r\n" +
                "\r\nMBCO SCPs of the same level exhibit minimized functional redundancies, while SCPs of lower levels (higher level numbers) describe subfunctions of their parent and grandparent SCPs annotated to higher levels. " +
                "The application will calculate significance ranks for all SCPs of the same level and dataset. Consequently, significance rank cutoffs can be specified for each level independently. Since most MBCO SCPs are annotated " +
                "to level 3, the default selection contains a higher significance rank cutoff for level-3 SCPs than for SCPs of the other levels.\r\nIn the case of MBCO, p-value and significance rank cutoffs can be further specified " +
                "separately for standard and dynamic enrichment analysis." +
                Form_default_settings.Explanation_text_major_separator +
                "Top % SCP interactions for dynamic enrichment analysis  when using MBCO" +
                "\r\nInferred interactions between functionally related level-2 or -3 SCPs are " +
                "ranked by their interaction strength, as described above. The user can specify how many of the top inferred interactions of each level (in percent) will be" +
                "considered for dynamic enrichment analysis." +
                Form_default_settings.Explanation_text_major_separator +
                "Specialized MBCO dataset: Sodium glucose transmembrane (TM) transport ontology" +
                "\r\nTo allow a representation of the transport activities of experimentally obtained sodium and glucose transporter genes, we developed " +
                "the Sodium glucose TM transport ontology. Please see Hansen et al. Sci Adv. 2022 for details. The main purpose of this ontology is the representation of the transport activities of experimentally obtained transporter genes. " +
                "For an ideal overview, set the p-value and rank cutoffs in this menu to 1 and 9999, respectively. Select ‘Connect parent and child SCPs’ and 'Add genes' in the menu 'SCP networks'. This will generate a hierarchical " +
                "SCP network where the function of all identified transporter genes can be easily investigated. To limit the generated network to selected SCPs and/or SCP branches, use the functionalities in the menu 'Select SCPs'." +
                Form_default_settings.Explanation_text_major_separator +
                "Gene Ontology (GO)" +
                "\r\nBefore the first use, all GO parent terms will be populated with the genes of their offspring terms (based on the 'part_of' and 'is_a' relationships) across all three namespaces. Results " +
                "will be saved. For each namespace (biological process, molecular function, cellular component) the user can select size cutoffs specifying a minimum and maximum number of genes annotated to a GO term. These size selections " +
                "refer to the populated GO terms. Only terms meeting those criteria will be used for enrichment analysis, pathway network integration and can be selected within the menus 'Select SCPs' and 'Define own SCPs'." +
                "\r\n" +
                "\r\nIndependently of the user-selected size cutoffs, we will always keep all genes annotated to any term with any size within the same namespace as background genes, to ensure consistent enrichment results for the " +
                "same GO term, independently of size selections." +
                "\r\n" +
                "\r\nHowever, GO is consistently updated, so that size selections and enrichment results might not be reproducible with GO annotations downloaded at different dates. For " +
                "more information about GO visit geneontology.org." +
                Form_default_settings.Explanation_text_major_separator +
                "Reactome" +
                "\r\nFirst time selection of any Reactome ontology will prepare the downloaded Reactome files for the application, including the mapping of NCBI gene symbols " +
                "to Reactome pathways based on the NCBI gene IDs. For more information about Reactome visit reactome.org." +
                Form_default_settings.Explanation_text_major_separator +
                "Custom 1 and Custom 2" +
                "\r\nThe application allows the upload of two custom ontologies. See " +
                "‘Prepare_custom_ontology.txt’ for details." +
                Form_default_settings.Explanation_text_major_separator +
                "Level and depth" +
                "\r\nSince the hierarchical organization of many ontologies represents a directed acyclic graph, there are often multiple ways to connect any " +
                "pathway with the ontology root pathway. The lengths of the shortest and longest pathways define the level and the depth, respectively. The menus 'Select SCPs' and 'Define own SCP' allow sorting of the pathways " +
                "under consideration of either variable. If a GO namespace is selected, both menus will only display pathways meeting the minimum and maximum size requirements, defined here. Levels and depths of those ontologies " +
                "have no influence on enrichment analysis, significance cutoff specifications, network integration and results visualization in bar diagrams, heatmaps or timelines." +
                Form_default_settings.Explanation_text_major_separator +
                "Generate Bar diagrams" +
                "\r\nIf selected, bar diagrams will be generated for one dataset at a time. Bar diagrams generated for all datasets annotated to the same integration group will first fill up all available spots on the same page(s) " +
                "in the generated pdf or picture files, before being added to the next page. Each bar diagram shows all significant SCPs predicted for one dataset. In the case of MBCO, SCPs of the same level are bundled within each " +
                "diagram. The bars visualize the -log10(p-values) of each prediction. Bars are either colored by level-specific colors (in the case of MBCO) or with the same color (any other ontology) or by user-defined dataset colors, " +
                "depending on the user selection in the panel 'Color bars and timelines'." +
                "\r\n" +
                "\r\nBars in diagrams visualizing the results of dynamic enrichment analysis are labeled with all SCPs of the predicted SCP combination or with a single " +
                "SCP, if the prediction is based on only one SCP." +
                "\r\n" +
                "\r\nThe last generated figure for each integration group shows a legend explaining the meaning of the colors and -in the case of MBCO- the meaning of the bundling of enrichment " +
                "results." +
                Form_default_settings.Explanation_text_major_separator +
                "Generate Heatmaps" +
                "\r\nIf selected, pathway prediction for all datasets assigned to the same integration groups can be visualized in the same heatmap. In the case of MBCO, enrichment results will be additionally " +
                "separated by MBCO SCP levels, i.e. the application will generate one heatmap for each level and integration group" +
                "\r\n" +
                "\r\nThe rows contain predicted SCPs, the columns all datasets of the same integration group. If the numbers of " +
                "SCPs or datasets are exceeding an upper threshold, the heatmap will be split into multiple heatmap blocks that are labeled with increasing block row and column indices." +
                "\r\n" +
                "\r\nThe user can select if the heatmaps show SCP " +
                "significance (-log10(p-values)) or significance ranks. Colors are adjusted to selected entry values. Fields colored in orange and light blue label predictions with lower significance based on up- or downregulated genes, " +
                "respectively. Fields colored in red and dark blue label predictions with higher significance based on up- or downregulated genes, respectively. The last generated figure shows the legend. Rounded  log10(p-values) or " +
                "significance ranks are written into the fields. In addition, the user can also select if only significant results should be visualized or if the results for each SCP that was predicted with significance for at least one " +
                "dataset should be shown for all datasets. In the latter case, fields of not significant predictions are colored white. Since the UpDown status is one of the attributes that define a different dataset, the directionality of " +
                "change visualized by fields colored in white can always be inferred from the color of the other fields annotated to the same dataset." +
                "\r\n" +
                "\r\nHeatmaps generated based on the results of dynamic enrichment analysis will show the " +
                "most significant -log10(p-value) or the highest rank (lowest rank number) of the SCP combinations an SCP belongs to" +
                Form_default_settings.Explanation_text_major_separator +
                "Generate timelines" +
                "\r\nTimelines visualize the progression of SCP -log10(p-values) over a given time " +
                "period. One timeline connects predictions for all datasets that only differ in the assigned timepoint and are assigned to the same integration group. -log10(p-values) of predicted SCPs from up- or downregulated genes are " +
                "defined to be positive or negative and visualized above or below the abscissa, respectively." +
                "\r\n" +
                "\r\nThe user can define a maximum p-value ('p-value cutoff' text box below 'Generate timelines'). Results will be shown for each " +
                "SCP that is predicted at at least one timepoint for at least one dataset with a p-value meeting the p-value cutoff. Two dashed horizontal lines will show the specified significance cutoff above and below the abscissa " +
                "(as +/- -log10(p-values) for up- and downregulated genes). Timelines will be colored based on SCP levels, if MBCO is selected, otherwise with the same color, or with user-defined dataset colors, depending on the user " +
                "selection in the panel 'Color bars and timelines'. If timelines are colored based on SCP levels or with the same color, timelines generated for different datasets will have different line styles (e.g. solid versus dashed " +
                "lines)." +
                "\r\n" +
                "\r\nThe last figure shows a legend, explaining the meaning of colors and line styles." +
                Form_default_settings.Explanation_text_major_separator +
                "Color bars and timelines" +
                "\r\nBars and timelines can be colored with level-specific colors (in the case of MBCO), with the same " +
                "color (in case of any other ontology) or with user-defined dataset colors. In the first case level-1, -2, -3 and -4 SCPs will be colored dark red, light red, blue and green." +
                "\r\n" +
                "\r\nHeatmap entries will always be " +
                "colored by significance. Network SCP nodes will always be colored by user-defined dataset colors.";

            Error_reports_ownTextBox.SilentText_and_refresh = text;
            int left = Error_reports_ownTextBox.Location.X;
            int right = left + Error_reports_ownTextBox.Width;
            int top = Error_reports_ownTextBox.Location.Y;
            int bottom = top + Error_reports_ownTextBox.Height;
            Form_default_settings.MyTextBoxMultiLine_adjustCoordinatesToBorders_add_default_parameter(Error_reports_ownTextBox, left, right, top, bottom);
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
            right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;

            bool generate_bardiagrams = GenerateBardiagrams_cbButton.Checked;
            bool generate_heatmaps = GenerateHeatmaps_cbButton.Checked;
            bool generate_timelines = GenerateTimelineLogScale_cbButton.Checked;
            GenerateBardiagrams_cbButton.SilentChecked = false;
            GenerateHeatmaps_cbButton.SilentChecked = false;
            GenerateTimeline_cbButton.SilentChecked = false;
            Update_panel();


            string reference_term = "pathway";
            string capitalized_reference_term = "Pathway";
            string first_reference_term = reference_term;
            if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
            { 
                reference_term = "SCP";
                capitalized_reference_term = "SCP";
                first_reference_term = "subcellular process (SCP)";
            }
            string selected_ontology_string = Ontology_classification_class.Get_name_of_ontology(selected_ontology);

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
                    #region Cutoffs
                    case 0:
                        mid_y_position = Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + StandardKeepTopLevel_1_SCPs_textBox.Location.Y
                                         + (int)Math.Round(0.5F * (StandardKeepTopLevel_1_SCPs_textBox.Height));
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Standard enrichment analysis that tests each " + first_reference_term + " individually for enrichment of each user-supplied gene list is used with any selected ontology.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;
                        mid_y_position = Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + CutoffsExplanation_myPanelLabel.Location.Y
                                         + (int)Math.Round(0.5F * (CutoffsExplanation_myPanelLabel.Height));
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = capitalized_reference_term + "s meeting both, the defined p-value and significance rank cutoffs, are defined significant and will be visualized in all outputs.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + StandardKeepTopLevel_1_SCPs_textBox.Location.Y
                             + (int)Math.Round(0.5F * (DynamicKeepTopLevel_2_SCPs_textBox.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Height - StandardKeepTopLevel_1_SCPs_textBox.Location.Y));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "MBCO SCPs are assigned to 4 granularity levels. SCPs of adjacent levels are in parent-child relationships, SCPs of the same level display minimized functional redundancies.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 3:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + StandardKeepTopLevel_1_SCPs_textBox.Location.Y
                                         + (int)Math.Round(0.5F * (DynamicKeepTopLevel_2_SCPs_textBox.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Height - StandardKeepTopLevel_1_SCPs_textBox.Location.Y));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Cutoff ranks can be specified separately for each MBCO level. Our default cutoff ranks consider that most SCPs map to level 3. For other ontologies a single cutoff rank will be applied to all pathways combined.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 4:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position =   Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Location.Y
                                             + (int)Math.Round(0.5F * (DynamicKeepTopLevel_2_SCPs_textBox.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Dynamic enrichment analysis merges and tests up to three functionally related MBCO SCPs for enrichment, if they contain genes of the dataset analyzed.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 5:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position =   Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + ScpTopInteractions_panel.Location.Y
                                             + (int)Math.Round(0.5F * (DynamicTopPercentScpsLevel_2_SCPs_textBox.Location.Y + DynamicTopPercentScpsLevel_2_SCPs_textBox.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Dynamic enrichment analysis uses functional interactions between level-2 or -3 MBCO SCPs that were predicted from text mining results.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 6:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position =   Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + ScpTopInteractions_panel.Location.Y
                                             + (int)Math.Round(0.5F * (DynamicTopPercentScpsLevel_2_SCPs_textBox.Location.Y + DynamicTopPercentScpsLevel_2_SCPs_textBox.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Since the functional interactions are weighted, the user can specify the top percentage of those interactions that will be used.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 7:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;
                            mid_y_position = Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Location.Y
                                             + (int)Math.Round(0.5F * (DynamicKeepTopLevel_2_SCPs_textBox.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Cutoff ranks and p-values will be applied to the predictions of dynamic enrichment analysis in the same way as for standard enrichment analysis.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 8:
                        if (GenerateBardiagrams_cbButton.Checked || GenerateHeatmaps_cbButton.Checked || GenerateTimeline_cbButton.Checked)
                        {
                            GenerateBardiagrams_cbButton.SilentChecked = false;
                            GenerateHeatmaps_cbButton.SilentChecked = false;
                            GenerateTimeline_cbButton.SilentChecked = false;
                            Update_panel();
                        }
                        if (Ontology_classification_class.Is_go_ontology(selected_ontology))
                        {
                            right_x_position_next_to_overall_panel = this.Overall_panel.Location.X - distance_from_overalMenueLabel;
                            mid_y_position = Overall_panel.Location.Y + EnrichmentSettings_panel.Location.Y + GO_hyperparameter_panel.Location.Y
                                             + (int)Math.Round(0.5F * (GO_hyperparameter_panel.Height));
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Since " + selected_ontology_string + " includes pathways with very few genes as well as pathways with many genes that may lack specificity, the user can define pathway size limits.";
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    #endregion
                    #region Output
                    case 9:
                        mid_y_position = Overall_panel.Location.Y + DefineOutputs_panel.Location.Y + (int)Math.Round((float)GenerateBardiagrams_cbButton.Location.Y + 0.5F * GenerateBardiagrams_cbButton.Height);
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Bardiagrams allow comparison of the significance -log10(p-values) of all " + reference_term + "s that were predicted for the same dataset.";
                        if (GenerateHeatmaps_cbButton.Checked || GenerateTimeline_cbButton.Checked)
                        {
                            GenerateHeatmaps_cbButton.SilentChecked = false;
                            GenerateTimeline_cbButton.SilentChecked = false;
                            Update_panel();
                        }
                        if (!GenerateBardiagrams_cbButton.Checked)
                        {
                            GenerateBardiagrams_cbButton.SilentChecked = true;
                            Update_panel();
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 10:
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + DefineOutputs_panel.Location.Y + (int)Math.Round((float)GenerateBardiagrams_cbButton.Location.Y + 0.5F * GenerateBardiagrams_cbButton.Height);
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Bars showing results for MBCO SCPs of the same level are visualized in one block.";
                            if (GenerateHeatmaps_cbButton.Checked || GenerateTimeline_cbButton.Checked)
                            {
                                GenerateHeatmaps_cbButton.SilentChecked = false;
                                GenerateTimeline_cbButton.SilentChecked = false;
                                Update_panel();
                            }
                            if (!GenerateBardiagrams_cbButton.Checked)
                            {
                                GenerateBardiagrams_cbButton.SilentChecked = true;
                                Update_panel();
                            }
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 11:
                        mid_y_position = Overall_panel.Location.Y + DefineOutputs_panel.Location.Y + (int)Math.Round((float)GenerateHeatmaps_cbButton.Location.Y + 0.5F * GenerateHeatmaps_cbButton.Height);
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Heatmaps show significance -log10(p-values) or ranks for all predicted " + reference_term + "s and each dataset within the same integration group.";
                        if (GenerateBardiagrams_cbButton.Checked  || GenerateTimeline_cbButton.Checked)
                        {
                            GenerateBardiagrams_cbButton.SilentChecked = false;
                            GenerateTimeline_cbButton.SilentChecked = false;
                            Update_panel();
                        }
                        if (!GenerateHeatmaps_cbButton.Checked)
                        {
                            GenerateHeatmaps_cbButton.SilentChecked = true;
                            Update_panel();
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 12:
                        if (GenerateBardiagrams_cbButton.Checked || GenerateTimeline_cbButton.Checked)
                        {
                            GenerateBardiagrams_cbButton.SilentChecked = false;
                            GenerateTimeline_cbButton.SilentChecked = false;
                            Update_panel();
                        }
                        if (Ontology_classification_class.Is_mbco_ontology(selected_ontology))
                        {
                            mid_y_position = Overall_panel.Location.Y + DefineOutputs_panel.Location.Y + (int)Math.Round((float)GenerateHeatmaps_cbButton.Location.Y + 0.5F * GenerateHeatmaps_cbButton.Height);
                            right_x_position = right_x_position_next_to_overall_panel;
                            text = "Results for MBCO " + reference_term + "s of each level are visualized in separate heatmaps.";
                            if (!GenerateHeatmaps_cbButton.Checked)
                            {
                                GenerateHeatmaps_cbButton.SilentChecked = true;
                                Update_panel();
                            }
                            UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                            UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        }
                        break;
                    case 13:
                        mid_y_position = Overall_panel.Location.Y + DefineOutputs_panel.Location.Y + (int)Math.Round((float)GenerateTimeline_cbButton.Location.Y + 0.5F * GenerateTimeline_cbButton.Height);
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Timelines show significance -log10(p-values) for one " + reference_term + " and all datasets within the same integration group over all timepoints as line charts.";
                        if (GenerateBardiagrams_cbButton.Checked || GenerateHeatmaps_cbButton.Checked)
                        {
                            GenerateBardiagrams_cbButton.SilentChecked = false;
                            GenerateHeatmaps_cbButton.SilentChecked = false;
                            Update_panel();
                        }
                        if (!GenerateTimeline_cbButton.Checked)
                        {
                            GenerateTimeline_cbButton.SilentChecked = true;
                            Update_panel();
                        }
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    #endregion
                    #region Color
                    case 14:
                        if (GenerateBardiagrams_cbButton.Checked || GenerateHeatmaps_cbButton.Checked || GenerateTimeline_cbButton.Checked)
                        {
                            GenerateBardiagrams_cbButton.SilentChecked = false;
                            GenerateHeatmaps_cbButton.SilentChecked = false;
                            GenerateTimeline_cbButton.SilentChecked = false;
                            Update_panel();
                        }
                        mid_y_position = Overall_panel.Location.Y + Colors_panel.Location.Y + (int)Math.Round(0.5F * Colors_panel.Height);
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Bars or timelines can be colored in ontology/level-selective or user-defined dataset colors.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    #endregion
                    #region Networks
                    case 15:
                        mid_y_position = Overall_panel.Location.Y + (int)Math.Round(0.5F * Overall_panel.Height);
                        right_x_position = right_x_position_next_to_overall_panel;
                        text = "Parameters for generated " + reference_term + " networks can be defined in menu 'SCP networks'.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    #endregion
                    default:
                        end_tour = true;
                        break;
                }
                if (back_pressed) { tour_box_index = tour_box_index - 2; }
                if ((escape_pressed) || (tour_box_index == -2)) { end_tour = true; }
            }
            UserInterface_tutorial.Set_to_invisible();

            GenerateBardiagrams_cbButton.SilentChecked = generate_bardiagrams;
            GenerateHeatmaps_cbButton.SilentChecked = generate_heatmaps;
            GenerateTimelineLogScale_cbButton.SilentChecked = generate_timelines;
        }
        #endregion


    }
}
