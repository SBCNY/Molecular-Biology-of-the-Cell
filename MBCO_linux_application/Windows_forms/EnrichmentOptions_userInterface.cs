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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
//using ZedGraph;


namespace ClassLibrary1.EnrichmentOptions_userInterface
{
    class EnrichmentOptions_text_class
    {
        public static string Timeline_label_Log_only_allowed_if_all_values_larger_zero {  get { return "Log scale only\nallowed, if all\ntimepoints > 0"; } }
        public static string Timeline_label_standard_text { get { return "1 timeline chart for\r\neach integration\r\ngroup and SCP"; } }
        public static string Timeline_label_only_one_timepoint { get { return "Data contains only\r\none timepoint"; } }
    }

    class EnrichmentOptions_userInterface_class
    {
        MyPanel Overall_panel { get; set; }
        MyPanel Ontology_panel { get; set; }
        System.Windows.Forms.Label Ontology_label { get; set; }
        OwnListBox Ontology_ownListBox { get; set; }
        MyPanel Cutoffs_panel { get; set; }
        MyPanel ScpTopInteractions_panel { get; set; }
        MyPanel EnrichmentSettings_panel { get; set; }
        System.Windows.Forms.Label KeepScpsScpLevel_label { get; set; }
        System.Windows.Forms.Label MaxRanks_label { get; set; }
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
        System.Windows.Forms.Label CutoffsExplanation_label { get; set; }
        Button Default_button { get; set; }

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
        Form1_default_settings_class Form_default_settings { get; set; }

        public EnrichmentOptions_userInterface_class(MyPanel overall_panel,
                                                     MyPanel ontology_panel,
                                                     System.Windows.Forms.Label ontology_label,
                                                     OwnListBox ontology_ownListBox,
                                                     MyPanel cutoffs_panel,
                                                     MyPanel scpTopInteractions_panel,
                                                     MyPanel enrichmentSettings_panel,
                                                     System.Windows.Forms.Label keepScpsScpLevel_label,
                                                     System.Windows.Forms.Label maxRanks_label,
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
                                                     System.Windows.Forms.Label cutoffsExplanation_label,
                                                     Button default_button,

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
            Ontology_ownListBox = ontology_ownListBox;
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
            MaxRanks_label = maxRanks_label;
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
            CutoffsExplanation_label = cutoffsExplanation_label;
            Default_button = default_button;
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

            this.Ontology_ownListBox.Items.Clear();
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Mbco_human);
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Mbco_mouse);
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Mbco_rat);
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport_human);
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Go_bp_human);
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Go_mf_human);
            this.Ontology_ownListBox.Items.Add(Ontology_type_enum.Go_cc_human);

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

            left_reference_border = (int)Math.Round(0.22F * Ontology_panel.Width);
            right_reference_border = (int)Math.Round(0.98F * Ontology_panel.Width); 
            top_reference_border = (int)Math.Round(0.05F * Ontology_panel.Height);
            bottom_reference_border = (int)Math.Round(0.95F * Ontology_panel.Height);
            my_listBox = Ontology_ownListBox;
            Ontology_ownListBox = Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border); ;

            left_position = min_left_position;
            right_position = Ontology_ownListBox.Location.X;
            top_position = 0;
            bottom_position = Ontology_panel.Height;
            Ontology_label = Ontology_label;
            Ontology_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(Ontology_label, left_position, right_position, top_position, bottom_position);
            #endregion

            #region Panel and subpanels
            left_reference_border = 0;
            right_reference_border = Overall_panel.Width;
            top_reference_border = Ontology_panel.Location.Y + Ontology_panel.Height;
            bottom_reference_border = (int)Math.Round(0.45F*Overall_panel.Height);
            my_panel = EnrichmentSettings_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;
            right_reference_border = EnrichmentSettings_panel.Width;
            top_reference_border = 0;
            bottom_reference_border = (int)Math.Round(EnrichmentSettings_panel.Height * 0.53);
            my_panel = Cutoffs_panel;
            Cutoffs_panel = Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

            left_reference_border = 0;
            right_reference_border = EnrichmentSettings_panel.Width;
            top_reference_border = (int)Math.Round(EnrichmentSettings_panel.Height * 0.65);
            bottom_reference_border = EnrichmentSettings_panel.Height;
            my_panel = ScpTopInteractions_panel;
            ScpTopInteractions_panel = Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            #endregion

            int shared_height_of_enrichmentSettings_scpInteractions_defineOutputs_textBoxes_checkBoxes = (int)Math.Round(0.14F * this.EnrichmentSettings_panel.Height);
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
                top_reference_border = (int)Math.Round(0.45 * referenceTextBox.Location.Y);
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
            top_position = StandardKeepTopLevel_1_SCPs_textBox.Location.Y - distance_to_bottom_cutoffPanel;
            bottom_position = StandardKeepTopLevel_1_SCPs_textBox.Location.Y + StandardKeepTopLevel_1_SCPs_textBox.Height + halfdistanceHeight_between_enrichmetSettings_textBoxes;
            my_label = StandardKeepTopScps_label;
            StandardKeepTopScps_label = Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_position, right_position, top_position, bottom_position);

            left_position = 0;
            right_position = StandardKeepTopLevel_1_SCPs_textBox.Location.X;
            top_position = DynamicKeepTopLevel_2_SCPs_textBox.Location.Y - halfdistanceHeight_between_enrichmetSettings_textBoxes;
            bottom_position = DynamicKeepTopLevel_2_SCPs_textBox.Location.Y + DynamicKeepTopLevel_2_SCPs_textBox.Height + distance_to_bottom_cutoffPanel;
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
            top_position = (int)Math.Round(0.65*DynamicTopPercentScpsLevel_2_SCPs_textBox.Location.Y);
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
            #endregion

            #region EnrichmentSettings cutoff information label
            Set_cutoff_explanation_label_fontSize_and_position();
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

            left_reference_border = Colors_panel.Width + (int)Math.Round(0.02F*Overall_panel.Width);// GenerateTimelinePvalue_textBox.Location.X + GenerateTimeline_ownCheckBox.Width;
            right_reference_border = Overall_panel.Width - (int)Math.Round(0.02F * Overall_panel.Width);
            top_reference_border = Colors_panel.Location.Y + (int)Math.Round(0.02F * Overall_panel.Height);
            bottom_reference_border = Colors_panel.Location.Y + Colors_panel.Height - (int)Math.Round(0.02F * Overall_panel.Height);
            my_button = Explanation_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);

        }
        private void Set_cutoff_explanation_label_fontSize_and_position()
        {
            int left_reference_border = 0;
            int right_reference_border = EnrichmentSettings_panel.Width;
            int top_reference_border = Cutoffs_panel.Location.Y + Cutoffs_panel.Height;
            int bottom_reference_border = ScpTopInteractions_panel.Location.Y;
            System.Windows.Forms.Label my_label = CutoffsExplanation_label;
            CutoffsExplanation_label = Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
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
            bool is_mbco_ontology = Ontology_classification_class.Get_related_human_ontology(mbco_options.Next_ontology).Equals(Ontology_type_enum.Mbco_human);
            int left_reference_border;
            int right_reference_border;
            int top_reference_border;
            int bottom_reference_border;
            if (is_mbco_ontology)
            {
                MaxRanks_label.Text = "Max ranks for indicated level";
                left_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X - (int)Math.Round(0.2F * Cutoffs_panel.Width);
                right_reference_border = StandardKeepTopLevel_4_SCPs_textBox.Location.X + StandardKeepTopLevel_4_SCPs_textBox.Width;
                top_reference_border = 0;
                bottom_reference_border = KeepScps_level_1_label.Location.Y;
                System.Windows.Forms.Label my_label = MaxRanks_label;
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }
            else
            {
                MaxRanks_label.Text = "Max rank";
                int half_extra_width = (int)Math.Round(0.5F * (MaxPvalue_label.Width - StandardKeepTopLevel_1_SCPs_textBox.Width));
                left_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X - half_extra_width;
                right_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.X + StandardKeepTopLevel_1_SCPs_textBox.Width + half_extra_width;
                top_reference_border = 0;
                bottom_reference_border = StandardKeepTopLevel_1_SCPs_textBox.Location.Y;
                System.Windows.Forms.Label my_label = MaxRanks_label;
                Form_default_settings.LabelDefaultBold_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_reference_border, right_reference_border, top_reference_border, bottom_reference_border);
            }
        }
        private void Update_enrichment_options(MBCO_enrichment_pipeline_options_class mbco_options)
        {
            Ontology_type_enum human_reference_ontology = Ontology_classification_class.Get_related_human_ontology(mbco_options.Next_ontology);
            bool visualize_mbco_specific_elements = human_reference_ontology.Equals(Ontology_type_enum.Mbco_human);
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
            Update_maxRank_label(mbco_options);
            if (mbco_options.Show_all_and_only_selected_scps)
            {
                CutoffsExplanation_label.Text = "All/only selected SCPs will be shown. Cutoffs only determine colors in heatmaps. Switch off under 'Select SCPs'";
                //Significance cutoffs will be ignored, only selected\r\nSCPs will be shown. Switch off under 'Select SCPs'
                Set_cutoff_explanation_label_fontSize_and_position();
                this.Cutoffs_panel.Visible = true;
                this.DynamicKeepTopLevel_2_SCPs_textBox.Visible = false;
                this.DynamicKeepTopLevel_3_SCPs_textBox.Visible = false;
                this.DynamicKeepTopScps_label.Visible = false;
                this.DynamicPvalue_textBox.Visible = false;
                this.ScpTopInteractions_panel.Visible = false;
            }
            else
            {
                CutoffsExplanation_label.Text = "SCPs among top ranked predictions AND\r\nwith a p-value below cutoff will be significant";
                Set_cutoff_explanation_label_fontSize_and_position();
                this.Cutoffs_panel.Visible = true;
                this.DynamicKeepTopLevel_2_SCPs_textBox.Visible = true;
                this.DynamicKeepTopLevel_3_SCPs_textBox.Visible = true;
                this.DynamicKeepTopScps_label.Visible = true;
                this.DynamicPvalue_textBox.Visible = true;
                this.ScpTopInteractions_panel.Visible = visualize_mbco_specific_elements;
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
            this.StandardPvalue_textBox.SilentText = mbco_options.Max_pvalue_for_standardEnrichment.ToString();
            this.StandardPvalue_textBox.TextAlign = HorizontalAlignment.Center;
            this.StandardPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.DynamicPvalue_textBox.SilentText = mbco_options.Max_pvalue_for_dynamicEnrichment.ToString();
            this.DynamicPvalue_textBox.TextAlign = HorizontalAlignment.Center;
            this.DynamicPvalue_textBox.BackColor = Form_default_settings.Color_textBox_backColor;

            this.GenerateTimelinePvalue_textBox.SilentText = mbco_options.Timeline_pvalue_cutoff.ToString();
            this.GenerateTimelinePvalue_textBox.Refresh();
            this.Ontology_ownListBox.SilentSelectedIndex_and_topIndex = this.Ontology_ownListBox.Items.IndexOf(mbco_options.Next_ontology);
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
        }

        public MBCO_enrichment_pipeline_options_class Ontology_ownListBox_SelectedIndexChanged(MBCO_enrichment_pipeline_options_class options)
        {
            options.Next_ontology = (Ontology_type_enum)Ontology_ownListBox.SelectedItem;
            Update_enrichment_options(options);
            return options;
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

        #region Explanation
        private void Write_explanation_into_error_reports_panel()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string go_association_complete_fileName = gdf.Complete_human_go_association_2022_downloaded_fileName;
            string go_obo_nw_complete_fileName = gdf.Complete_go_obo_fileName;
            string go_association_fileName = System.IO.Path.GetFileName(go_association_complete_fileName);
            string go_obo_nw_fileName = System.IO.Path.GetFileName(go_obo_nw_complete_fileName);

            Error_reports_headline_label.Text = "Enrichment analysis";
            Error_reports_headline_label.Refresh();
            Error_reports_maxErrorsPerFile_ownTextBox.Visible = false;
            Error_reports_maxErrorsPerFile_ownTextBox.Refresh();
            Error_reports_maxErrorPerFile1_label.Visible = false;
            Error_reports_maxErrorPerFile2_label.Visible = false;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Molecular Biology of the Cell Ontology (MBCO)");
            sb.AppendFormat("\r\nMBCO was developed based on standard cell biology and biochemistry text books (Alberts et al.,");
            sb.AppendFormat("\r\nBerg et al., Rosenthal and Glew) and reviews. It has a strict focus on cell biology. Its subcellular");
            sb.AppendFormat("\r\nprocesses (SCPs) were populated using a combination of PubMed abstract text mining and enrichment");
            sb.AppendFormat("\r\nanalysis, followed by computer-assisted manual validation of each gene-SCP association.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nHierarchical organization of MBCO");
            sb.AppendFormat("\r\nMBCO SPCs are hierarchically organized in parent-child relationships that typically span three to four");
            sb.AppendFormat("\r\nlevels. Higher levels (levels with lower numbers) contain SCPs that describe more general, lower levels");
            sb.AppendFormat("\r\nSCPs that describe more detailed cell biological functions. SCPs of the same level were designed to be");
            sb.AppendFormat("\r\nmutually exclusive, so that every predicted SCP of the same level offers additional information.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nInferred functional interactions between SCPs of level-2 and level-3");
            sb.AppendFormat("\r\nThe annotated hierarchy is enriched using a unique MBCO algorithm that predicts functional interactions");
            sb.AppendFormat("\r\nbetween SCPs of the same level. These interactions connect SCPs with the same or different parents and");
            sb.AppendFormat("\r\ngrandparents. The interactions are ranked by their interaction strength.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nStandard enrichment analysis");
            sb.AppendFormat("\r\nStandard or traditional enrichment analysis analyses, if a list of experimentally obtained genes");
            sb.AppendFormat("\r\nis enriched for genes annotated to a subcellular process (SCP). The used right-tailed Fisher's Exact");
            sb.AppendFormat("\r\ntest first calculates the overlap between the genes of the SCP and the experimentally obtained");
            sb.AppendFormat("\r\ngenes list. Afterwards, it calculates a p-value that gives the probability to observe the same");
            sb.AppendFormat("\r\nor a higher overlap, if it was by chance. All predicted SCPs of the same level and dataset are");
            sb.AppendFormat("\r\nranked by significance.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nDynamic enrichment analysis");
            sb.AppendFormat("\r\nStandard enrichment analysis analyses one SCP at a time for enrichment of experimentally");
            sb.AppendFormat("\r\nobtained gene lists. In contrast, dynamic enrichment analysis additionally analyses, if a set");
            sb.AppendFormat("\r\nof merged functionally related SCPs enriches for the user-supplied genes.");
            sb.AppendFormat("\r\nDynamic enrichment analysis first determines those SCPs that contain at least one experimentally ");
            sb.AppendFormat("\r\nobtained gene. Identified SCPs are merged, if they are functionally related based on the inferred");
            sb.AppendFormat("\r\nSCP interactions. Any possible combination of two to three functionally related SCPs is added as a");
            sb.AppendFormat("\r\ncontext-specific higher-level SCP to the MBC ontology. The algorithm is called dynamic enrichment");
            sb.AppendFormat("\r\nanalysis, since the decision which context-specific higher-level SCPs are added to the ontology");
            sb.AppendFormat("\r\ndepends on the analyzed gene list and is made during runtime.");
            sb.AppendFormat("\r\nAfter generation of the context-specific ontology dynamic enrichment analysis uses the same");
            sb.AppendFormat("\r\nalgorithms as standard enrichment analysis. SCPs and SCP combinations are tested for enrichment");
            sb.AppendFormat("\r\nof user-supplied gene lists using Fisher's Exact Test. Predictions (that are either single SCPs");
            sb.AppendFormat("\r\nor SCP combinations) are ranked by significance for each dataset and SCP level.");
            sb.AppendFormat("\r\nIn the result spreadsheets, SCPs of predicted SCP combinations are separated by dollar signs. In the");
            sb.AppendFormat("\r\nbardiagram figures, SCPs of one combination label one -log10(p-value) bar. Since one SCP can be part of");
            sb.AppendFormat("\r\nmultiple SCP combinations, the heatmaps will show the most significant -log10(p-value) or the highest rank");
            sb.AppendFormat("\r\n(lowest rank number) of the SCP combinations it belongs to.");
            sb.AppendFormat("\r\nAdditional details about the dynamic enrichment algorithm are described in our publication Hansen et al.");
            sb.AppendFormat("\r\nSci Rep. 2017.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nCutoff parameter");
            sb.AppendFormat("\r\nPredicted SCPs and/or SCP combinations of each dataset and SCP level will be ranked by significance.");
            sb.AppendFormat("\r\nThe user can specify a p-value cutoff for standard or dynamic enrichment analysis. In addition, the");
            sb.AppendFormat("\r\nuser can define a maximum rank cutoff for SCPs of each level that are predicted by standard or dynamic");
            sb.AppendFormat("\r\nenrichment analysis. Only SCPs and/or SCP combinations that fulfill both cutoff criteria, will be");
            sb.AppendFormat("\r\nlabeled as significant and visualized in bardiagrams, heatmaps, timelines or networks.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nTop % SCP interactions for dynamic enrichment analysis");
            sb.AppendFormat("\r\nInferred interactions between functionally related SCPs are ranked by their interaction strength.");
            sb.AppendFormat("\r\nThe user can specify how many ot the top inferred interactions of each level (in percent) will be");
            sb.AppendFormat("\r\nconsidered for dynamic enrichment analysis.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nSpecialized MBCO dataset: Sodium glucose transmembrane (TM) transport ontology");
            sb.AppendFormat("\r\nTo allow a representation of the transport activities of experimentally obtained sodium and glucose");
            sb.AppendFormat("\r\ntransporter genes, we developed the Sodium glucose TM transport ontology.");
            sb.AppendFormat("\r\nThe Sodium glucose TM transport ontology contains a nearly comprehensive representation of known sodium");
            sb.AppendFormat("\r\nglucose transmembrane transporters (Channels and facilitative transporters are excluded).");
            sb.AppendFormat("\r\nThey are mapped to SCPs that specify their functional involvement in the renal tubule of the kidney.");
            sb.AppendFormat("\r\nSCPs document the type of the transporter (symporter, antiporter, uniporter), the direction of transport");
            sb.AppendFormat("\r\n(lumen to blood, L2B, blood to lumen, B2L) and the involvment of all other transported molecules. If not");
            sb.AppendFormat("\r\nstated otherwise, the direction of transport is always L2B. SCPs describing B2L transport are labeled");
            sb.AppendFormat("\r\naccordingly. Molecules transported by antiporters in the different directions are separated by 'vs'.");
            sb.AppendFormat("\r\nMolecules in front of 'vs' are transported L2B, molecules after 'vs' B2L. Since many transporters have");
            sb.AppendFormat("\r\nown names (e.g., the furosemid-sensitive NKCC2) that are familiar to the research community, regularily");
            sb.AppendFormat("\r\nused and easier to remember than the NCBI official gene symbols (here SLC12A1), we added these names to the");
            sb.AppendFormat("\r\nontology as individual SCPs as well. SCPs are integrated into a hierarchy that finally converges on");
            sb.AppendFormat("\r\nsodium or glucose L2B and B2L transport. Along the hierarchy transporter SCPs with partially shared activities");
            sb.AppendFormat("\r\nconverge on more general parent SCPs. NKCC2, for example, is a child of 'Sodium potassium chloride");
            sb.AppendFormat("\r\nsymporter' that is a child of 'Sodium potassium symporter' and 'Sodium chloride symporter' that are both'");
            sb.AppendFormat("\r\nchildren of 'Sodium L2B transport by symporter'. The SCP 'Potassium chloride symporter' is not part of the ontology,");
            sb.AppendFormat("\r\nsince the ontology focusses on sodium and glucose TM transport.");
            sb.AppendFormat("\r\nThe main purpose of this ontology lies in the representation of the transport activities of experimentally");
            sb.AppendFormat("\r\nobtained transporter genes. For an ideal overview, set the p-value and rank cutoffs in the menu 'Enrichment'");
            sb.AppendFormat("\r\nto 1 and 9999, respectively, and select 'Add genes' in the menu 'SCP networks'. This will generate a");
            sb.AppendFormat("\r\nhierarchical SCP network where the function of all identified transporter genes can be easily investigated.");
            sb.AppendFormat("\r\nTo limit the generated network to selected SCPs and/or SCP branches, use the functionalities in the menu");
            sb.AppendFormat("\r\n'Select SCPs'.");
            sb.AppendFormat("\r\nAdditional details can be obtained from Hansen et al. Sci Adv. 2022.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nGene Ontology (GO)");
            sb.AppendFormat("\r\nTo generate pathway networks using GO, download the files '" + go_association_fileName + "' and");
            sb.AppendFormat("\r\n'" + go_obo_nw_fileName + "' from geneontology.org, unzip and copy paste them into the folder 'GO_datasets'.");
            sb.AppendFormat("\r\nBefore the first use all GO parent terms will be populated with the genes of their offspring terms (based on.");
            sb.AppendFormat("\r\nthe 'part_of' and 'is_a' relationships). To focus on pathways that describe relevant biology and are not too");
            sb.AppendFormat("\r\nsmall, our application removes all GO terms that contain less than 7 and more than 250 genes. Each GO namespace");
            sb.AppendFormat("\r\n(Biologial Process, Cellular Component, Molecular Function) can be selected as a separate 'ontology' for enrichment");
            sb.AppendFormat("\r\nanalysis. Populated and trimmed ontology namespaces will be saved in the directory 'GO datasets' for later uses.");
            sb.AppendFormat("\r\nFor more information about GO visit geneontology.org.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nEnrichment analysis using specialized MBCO datasets or Gene Ontology (GO)");
            sb.AppendFormat("\r\nDynamic enrichment analysis and level-specific rank cutoffs are only available for MBCO. In the case");
            sb.AppendFormat("\r\nof the other ontologies, user-supplied gene lists will be subjected to standard enrichment analysis.");
            sb.AppendFormat("\r\nPredicted pathways of each dataset will be ranked by significance. The user can define one rank cutoff");
            sb.AppendFormat("\r\nfor all pathways.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nGenerate Bardiagrams");
            sb.AppendFormat("\r\nIf selected, bardiagrams will be generated for one dataset at a time. Each bardiagram figure shows all");
            sb.AppendFormat("\r\nsignificant SCPs predicted for one dataset. SCPs of the same level are bundled within each diagram. The");
            sb.AppendFormat("\r\nbars visualize the -log10(p-values) of each prediction. Bars are either colored by level-specific colors");
            sb.AppendFormat("\r\nor by user-defined dataset colors, depending on the user selection in the panel 'Color bars and timelines'.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nGenerate Heatmaps");
            sb.AppendFormat("\r\nIf selected, heatmaps will be generated. The rows contain predicted SCPs, the columns all datasets of the same"); 
            sb.AppendFormat("\r\nintegration group. In case of many SCPs or datasets, the heatmap will be split into multiple heatmaps that are");
            sb.AppendFormat("\r\nlabeled with increasing numbers. The user can select, if the heatmaps show SCP significance (-log10(p-values))");
            sb.AppendFormat("\r\nor fractional ranks. Colors are adjusted to selected entry values. Orange and light blue label predictions");
            sb.AppendFormat("\r\nwith lower significance based on up- or downregulated genes, respectively. Red and dark blue label predictions");
            sb.AppendFormat("\r\nwith higher significance based on up- or downregulated genes, respectively.");
            sb.AppendFormat("\r\nHeatmaps generated based on the results of dynamic enrichment analysis will show the most significant -log10(p-value)");
            sb.AppendFormat("\r\nor the highest rank (lowest rank number) of the SCP combinations an SCP belongs to.");
            sb.AppendFormat("\r\nIn addition, the user can select if results will only be shown for those datasets that predicted an SCP with");
            sb.AppendFormat("\r\nuser-defined significance or for all datasets, as long at least one dataset predicted that SCP with significance.");
            sb.AppendFormat("\r\nIn the latter case, fields containing not significant predictions will be colored white.");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nGenerate timelines");
            sb.AppendFormat("\r\nTimelines visualize the progression of SCP -log10(p-values) over a given time period. One timeline connects"); 
            sb.AppendFormat("\r\npredictions for all datasets that only differ in the assigned timepoint. -log10(p-values) of predicted SCPs");
            sb.AppendFormat("\r\nfrom up- or downregulated genes are defined to be positive or negative and visualized above or below the abscissa,");
            sb.AppendFormat("\r\nrespectively. All datasets assigned to the same integration group will be visualized in the same timeline figure.");
            sb.AppendFormat("\r\nThe user can define a maximum p-value ('p-value cutoff' text box below 'Generate timelines'). Results will be shown");
            sb.AppendFormat("\r\nfor each SCP that is predicted at at least one timepoint for at least one dataset with a p-value equal or below");
            sb.AppendFormat("\r\nthe specified p-value. Two dashed horizontal lines will show the specified significance cutoff above and below");
            sb.AppendFormat("\r\nthe abscissa (as +/- -log10(p-values) for up- and downregulated genes). Timelines will be colored based on SCP");
            sb.AppendFormat("\r\nlevels or user-defined dataset colors, depending on the user selection in the panel 'Color bars and timelines'");
            sb.AppendFormat("\r\nIn the first case, timelines generated for different datasets will have different line styles (e.g. solid versus");
            sb.AppendFormat("\r\ndashed lines).");
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\r\nColor bars and timelines");
            sb.AppendFormat("\r\nThe user can select, if bars and timelines will be colored by level-specific colors or user-defined dataset");
            sb.AppendFormat("\r\ncolors. In the first case, level-1, -2, -3 and -4 SCPs will be colored dark red, light red, blue and green.");
            sb.AppendFormat("\r\nHeatmap entries will always be colored by significance. Network SCP nodes will always be colored by");
            sb.AppendFormat("\r\nuser-defined dataset colors.");

            Error_reports_ownTextBox.SilentText_and_refresh = sb.ToString();
        }

        public void Explanation_button_activated()
        {
            Write_explanation_into_error_reports_panel();
        }
        #endregion

    }
}
