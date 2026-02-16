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
using System.Drawing;
using Data;
using Common_functions.Global_definitions;
using Common_functions.Form_tools;
using System.Data.SqlTypes;
using static System.Net.Mime.MediaTypeNames;
using Windows_forms;
using System.Threading;

namespace ClassLibrary1.SigData_userInterface
{
    class SigData_text_class
    {
        public static string Changes_significance_using_both_cutoffs_text { get { return "Adjust significance\nusing both cutoffs"; } }
        public static string Set_all_genes_to_significant_text { get { return "Define all genes\nas significant"; } }
        public static string Eg_pvalue_text { get { return "eg p-values"; } }
        public static string Eg_log2foldchange_text { get { return "eg log2(FCs)"; } }
        public static string Eg_minuslog10pvalue_text { get { return "eg -log10(p)"; } }

        public const string Value1st_string = "1.value";
        public const string Value2nd_string = "2.value";
        public const string Higher_string = "Higher"; 
        public const string Lower_string = "Lower";
        public const string Dateset_definition_mergedUpAndDown = "Name, timepoint";
        public const string Dateset_definition_separatedUpAndDown = "Name, timepoint, Up/Down status";
    }

    class SigData_userInterface_class
    {
        MyPanel Overall_panel { get; set; }
        MyPanel SigSelection_panel { get; set; }
        Label ValueDirection_headline_label { get; set; }
        OwnListBox DirectionValue1st_ownListBox { get; set; }
        Label DirectionValue1st_label { get; set; }
        OwnListBox DirectionValue2nd_ownListBox { get; set; }
        Label DirectionValue2nd_label { get; set; }

        Label First_sigCutoff_headline_label { get; set; }
        MyPanel_label Value1st_cutoff_myPanelLabel { get; set; }
        OwnTextBox Value1st_cutoff_ownTextBox { get; set; }
        MyPanel_label Value1st_cutoff_expl_myPanelLabel { get; set; }
        MyPanel_label Value2nd_cutoff_myPanelLabel { get; set; }
        OwnTextBox Value2nd_cutoff_ownTextBox { get; set; }
        MyPanel_label Value2nd_cutoff_expl_myPanelLabel { get; set; }

        Label Second_sigCutoff_headline_label { get; set; }
        Label RankByValue_left_label { get; set; }
        OwnListBox RankByValue_ownListBox { get; set; }
        MyPanel_label RankByTieBreaker_myPanelLabel { get; set; }
        Label DefineDataset_label { get; set; }
        OwnListBox DefineDataset_ownListBox { get; set; }
        MyPanel_label DefineDataset_expl_myPanelLabel { get; set; }

        OwnTextBox KeepTopRankedGenes_ownTextBox { get; set; }
        Label KeepTopRankedGenes_left_label { get; set; }
        Label KeepTopRankedGenes_right_label { get; set; }

        MyCheckBox_button DeleteNotSignGenes_cbButton { get; set; }
        Label DeleteNotSignGenes_cbLabel { get; set; }
        Label AllGenesSignificant_headline_label { get; set; }
        MyCheckBox_button AllGenesSignificant_cbButton { get; set; }
        Label AllGenesSignificant_cbLabel { get; set; }
        Button ResetSig_button { get; set; }
        Button ResetParameter_button { get; set; }
        Label SigSubject_explanation_label { get; set; }
        Button Tutorial_button { get; set; }
        Tutorial_interface_class UserInterface_tutorial { get; set; }
        User_data_options_class Visualized_options { get; set; }
        Form1_default_settings_class Form_default_settings { get; set; }


        public SigData_userInterface_class(MyPanel overall_panel,
                                           MyPanel sigSelection_panel,
                                           Label valueDirection_headline_label,
                                           OwnListBox directionValue1st_ownListBox,
                                           Label directionValue1st_label,
                                           OwnListBox directionValue2nd_ownListBox,
                                           Label directionValue2nd_label,

                                           Label first_sigCutoff_headline_label,
                                           MyPanel_label value1st_cutoff_myPanelLabel,
                                           OwnTextBox value1st_cutoff_ownTextBox,
                                           MyPanel_label value1st_cutoff_expl_myPanelLabel,
                                           MyPanel_label value2nd_cutoff_myPanelLabel,
                                           OwnTextBox value2nd_cutoff_ownTextBox,
                                           MyPanel_label value2nd_cutoff_expl_myPanelLabel,

                                           Label second_sigCutoff_headline_label,
                                           Label rankByValue_left_label,
                                           OwnListBox rankByValue_ownListBox,
                                           MyPanel_label rankByTieBreaker_myPanelLabel,
                                           Label defineDataset_label,
                                           OwnListBox defineDataset_ownListBox,
                                           MyPanel_label defineDataset_expl_myPanelLabel,

                                           OwnTextBox keepTopRankedGenes_ownTextBox,
                                           Label keepTopRankedGenes_left_label,
                                           Label keepTopRankedGenes_right_label,

                                           MyCheckBox_button deleteNotSignGenes_cbButton,
                                           Label deleteNotSignGenes_cbLabel,
                                           Label allGenesSignificant_headline_label,
                                           MyCheckBox_button allGenesSignificant_cbButton,
                                           Label allGenesSignificant_cbLabel,
                                           Button resetSig_button,
                                           Button resetParameter_button,
                                           Label sigSubject_explanation_label,
                                           Form1_default_settings_class form_default_settings,
                                           
                                           Button tutorial_button,
                                           Tutorial_interface_class userInterface_tutorial,

                                           User_data_options_class custom_data_options)
        {
            Visualized_options = custom_data_options.Deep_copy();
            Form_default_settings = form_default_settings;

            Overall_panel = overall_panel;
            SigSelection_panel = sigSelection_panel;
            ValueDirection_headline_label = valueDirection_headline_label;
            DirectionValue1st_ownListBox = directionValue1st_ownListBox;
            DirectionValue1st_label = directionValue1st_label;
            DirectionValue2nd_ownListBox = directionValue2nd_ownListBox;
            DirectionValue2nd_label = directionValue2nd_label;

            First_sigCutoff_headline_label = first_sigCutoff_headline_label;
            Value1st_cutoff_myPanelLabel = value1st_cutoff_myPanelLabel;
            Value1st_cutoff_ownTextBox = value1st_cutoff_ownTextBox;
            Value1st_cutoff_expl_myPanelLabel = value1st_cutoff_expl_myPanelLabel;
            Value2nd_cutoff_myPanelLabel = value2nd_cutoff_myPanelLabel;
            Value2nd_cutoff_ownTextBox = value2nd_cutoff_ownTextBox;
            Value2nd_cutoff_expl_myPanelLabel = value2nd_cutoff_expl_myPanelLabel;

            Second_sigCutoff_headline_label = second_sigCutoff_headline_label;
            RankByValue_left_label = rankByValue_left_label;
            RankByValue_ownListBox = rankByValue_ownListBox;
            RankByTieBreaker_myPanelLabel = rankByTieBreaker_myPanelLabel;
            DefineDataset_label = defineDataset_label;
            DefineDataset_ownListBox = defineDataset_ownListBox;
            DefineDataset_expl_myPanelLabel = defineDataset_expl_myPanelLabel;

            KeepTopRankedGenes_ownTextBox = keepTopRankedGenes_ownTextBox;
            KeepTopRankedGenes_left_label = keepTopRankedGenes_left_label;
            KeepTopRankedGenes_right_label = keepTopRankedGenes_right_label;

            DeleteNotSignGenes_cbButton = deleteNotSignGenes_cbButton;
            DeleteNotSignGenes_cbLabel = deleteNotSignGenes_cbLabel;
            AllGenesSignificant_headline_label = allGenesSignificant_headline_label;
            AllGenesSignificant_cbButton = allGenesSignificant_cbButton;
            AllGenesSignificant_cbLabel = allGenesSignificant_cbLabel;
            ResetSig_button = resetSig_button;
            ResetParameter_button = resetParameter_button;
            SigSubject_explanation_label = sigSubject_explanation_label;
            Form_default_settings = form_default_settings;

            Tutorial_button = tutorial_button;
            UserInterface_tutorial = userInterface_tutorial;

            Fill_list_and_text_boxes();
            Update_all_graphic_elements(custom_data_options);
        }

        private void Fill_list_and_text_boxes()
        {
            DirectionValue1st_ownListBox.Items.Clear();
            DirectionValue1st_ownListBox.Items.Add(SigData_text_class.Lower_string);
            DirectionValue1st_ownListBox.Items.Add(SigData_text_class.Higher_string);
            DirectionValue2nd_ownListBox.Items.Clear();
            DirectionValue2nd_ownListBox.Items.Add(SigData_text_class.Lower_string);
            DirectionValue2nd_ownListBox.Items.Add(SigData_text_class.Higher_string);
            DefineDataset_ownListBox.Items.Clear();
            DefineDataset_ownListBox.Items.Add(SigData_text_class.Dateset_definition_mergedUpAndDown);
            DefineDataset_ownListBox.Items.Add(SigData_text_class.Dateset_definition_separatedUpAndDown);
            RankByValue_ownListBox.Items.Clear();
            RankByValue_ownListBox.Items.Add(SigData_text_class.Value1st_string);
            RankByValue_ownListBox.Items.Add(SigData_text_class.Value2nd_string);
        }

        private void Get_shared_graphical_parameters(out int space_for_headline, out int shared_row_height, out int shared_right_referenceBorder,out int shared_left_referenceBorder, out float fraction_of_rowHeight_between_paragraphs)
        {
            fraction_of_rowHeight_between_paragraphs = 0.35F;
            space_for_headline = (int)Math.Round(0.065F * SigSelection_panel.Height);
            shared_row_height = (int)Math.Round((float)(SigSelection_panel.Height - 4.2 * space_for_headline) / (10F + 3*fraction_of_rowHeight_between_paragraphs));
            shared_right_referenceBorder = (int)Math.Round(0.95F * Overall_panel.Width);
            shared_left_referenceBorder = (int)Math.Round(0.02F * Overall_panel.Width);
        }

        public void Update_all_graphic_elements(User_data_options_class custom_data_options)
        { 
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            MyPanel my_panel;
            Label my_label;
            MyCheckBox_button my_cbButton;
            OwnTextBox my_textBox;
            OwnListBox my_listBox;
            Button my_button;

            this.Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);
            int shared_distance_from_leftRightSide = (int)Math.Round(0.01 * this.Overall_panel.Width);


            #region SigSelection panel
            left_referenceBorder = shared_distance_from_leftRightSide;
            right_referenceBorder = this.Overall_panel.Width - shared_distance_from_leftRightSide;
            top_referenceBorder = shared_distance_from_leftRightSide;
            bottom_referenceBorder = (int)Math.Round(0.75 * this.Overall_panel.Height);
            my_panel = this.SigSelection_panel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            int space_for_headline;
            int shared_row_height;
            int shared_right_referenceBorder;
            int shared_left_referenceBorder;
            float fraction_of_rowHeight_between_paragraphs;
            Get_shared_graphical_parameters(out space_for_headline, out shared_row_height, out shared_right_referenceBorder, out shared_left_referenceBorder, out fraction_of_rowHeight_between_paragraphs);
            int shared_height_of_textBoxes = Math.Min(shared_row_height,(int)Math.Round(0.07F * SigSelection_panel.Height));
            int shared_heightWidth_of_checkBoxButtons = Math.Min(shared_row_height, (int)Math.Round(0.05F * SigSelection_panel.Height));
            int half_extra_height_for_cbLabels = (int)Math.Round(0.5F * (shared_row_height - shared_heightWidth_of_checkBoxButtons));
            int running_top_referenceBorder = 0;

            #region Significance directions - headline label
            left_referenceBorder = shared_left_referenceBorder;
            right_referenceBorder = shared_right_referenceBorder;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.ValueDirection_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += space_for_headline;

            #region Significance direction listBoxes, labels and example labels
            Dictionary<OwnListBox, Label> valueDirection_ownListBox_label_dict = new Dictionary<OwnListBox, Label>();
            valueDirection_ownListBox_label_dict.Add(DirectionValue1st_ownListBox, DirectionValue1st_label);
            valueDirection_ownListBox_label_dict.Add(DirectionValue2nd_ownListBox, DirectionValue2nd_label);

            int width_of_directionValue_listBoxes = (int)Math.Round(0.25 * SigSelection_panel.Width);

            OwnListBox[] valueDirection_ownListBoxes = valueDirection_ownListBox_label_dict.Keys.ToArray();
            OwnListBox current_valueDirection_ownListBox;
            int valueDirection_ownListBoxes_length = valueDirection_ownListBoxes.Length;
            Label current_valueDirection_label;

            bottom_referenceBorder = running_top_referenceBorder;
            for (int indexVD=0; indexVD<valueDirection_ownListBoxes_length;indexVD++)
            {
                current_valueDirection_ownListBox = valueDirection_ownListBoxes[indexVD];
                current_valueDirection_label = valueDirection_ownListBox_label_dict[current_valueDirection_ownListBox];

                left_referenceBorder = shared_left_referenceBorder + Form_default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
                right_referenceBorder = left_referenceBorder + width_of_directionValue_listBoxes;
                top_referenceBorder = bottom_referenceBorder;
                bottom_referenceBorder = top_referenceBorder + shared_row_height;
                my_listBox = current_valueDirection_ownListBox;
                Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                left_referenceBorder = current_valueDirection_ownListBox.Location.X + current_valueDirection_ownListBox.Width;
                right_referenceBorder = shared_right_referenceBorder;
                top_referenceBorder = current_valueDirection_ownListBox.Location.Y;
                bottom_referenceBorder = current_valueDirection_ownListBox.Location.Y + current_valueDirection_ownListBox.Height;
                my_label = current_valueDirection_label;
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
            #endregion

            running_top_referenceBorder += (int)Math.Round((valueDirection_ownListBoxes_length+ fraction_of_rowHeight_between_paragraphs) * shared_row_height);

            #region Significance cutoff headline
            left_referenceBorder = shared_left_referenceBorder;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.First_sigCutoff_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion
  
            running_top_referenceBorder += space_for_headline;

            #region Significance cutoff textBoxes and labels
            int shared_leftSide_for_cutoff_textBoxes = (int)Math.Round(0.52F * this.SigSelection_panel.Width);
            int shared_rightSide_for_cutoff_textBoxes = (int)Math.Round(0.72F * this.SigSelection_panel.Width);

            Dictionary<OwnTextBox, MyPanel_label> cutoff_textBox_label_dict = new Dictionary<OwnTextBox, MyPanel_label>();
            Dictionary<OwnTextBox, MyPanel_label> cutoff_textBox_exampleLabel_dict = new Dictionary<OwnTextBox, MyPanel_label>();
            cutoff_textBox_label_dict.Add(Value1st_cutoff_ownTextBox, Value1st_cutoff_myPanelLabel);
            cutoff_textBox_label_dict.Add(Value2nd_cutoff_ownTextBox, Value2nd_cutoff_myPanelLabel);

            OwnTextBox[] cutoff_ownTextBoxes = cutoff_textBox_label_dict.Keys.ToArray();
            OwnTextBox current_cutoff_ownTextBox;
            int cutoff_ownTextBoxes_length = cutoff_ownTextBoxes.Length;
            bottom_referenceBorder = running_top_referenceBorder;
            for (int indexCutoff=0; indexCutoff<cutoff_ownTextBoxes_length;indexCutoff++)
            {
                current_cutoff_ownTextBox = cutoff_ownTextBoxes[indexCutoff];

                left_referenceBorder = shared_leftSide_for_cutoff_textBoxes;
                right_referenceBorder = shared_rightSide_for_cutoff_textBoxes;
                top_referenceBorder = bottom_referenceBorder;
                bottom_referenceBorder = top_referenceBorder + shared_row_height;
                my_textBox = current_cutoff_ownTextBox;
                Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
            Set_cutoff_value1st_and_2nd_label_and_expl_label_fontSize_and_size();
            #endregion

            running_top_referenceBorder += (int)Math.Round((cutoff_ownTextBoxes_length+ fraction_of_rowHeight_between_paragraphs) * shared_row_height);

            #region Rank headline
            left_referenceBorder = shared_left_referenceBorder;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.Second_sigCutoff_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += space_for_headline;

            #region Rank by first or second value listBox, label and tie breaker label
            left_referenceBorder = (int)Math.Round(0.42F * (SigSelection_panel.Width + shared_left_referenceBorder));
            right_referenceBorder = (int)Math.Round(0.67F * (SigSelection_panel.Width + shared_left_referenceBorder));
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_row_height;
            my_listBox = this.RankByValue_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_left_referenceBorder;
            right_referenceBorder = RankByValue_ownListBox.Location.X - Form_default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
            top_referenceBorder = RankByValue_ownListBox.Location.Y;
            bottom_referenceBorder = RankByValue_ownListBox.Location.Y + RankByValue_ownListBox.Height;
            my_label = this.RankByValue_left_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Set_tie_breaker_explanation_label_fontSize_and_size();
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Rank by first or second value dataset definition
            left_referenceBorder = shared_left_referenceBorder;
            right_referenceBorder = (int)Math.Round(0.81 * (shared_right_referenceBorder));
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_row_height;
            my_label = this.DefineDataset_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_row_height;
            left_referenceBorder = shared_left_referenceBorder + Form_default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
            right_referenceBorder = (int)Math.Round(0.95F * (shared_right_referenceBorder));
            my_listBox = this.DefineDataset_ownListBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Set_dataset_definition_expl_label_size_and_font_size();
            #endregion

            running_top_referenceBorder += (int)Math.Round(3.5F * shared_row_height);

            #region Rank keep top ranks text box, left and right labels
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_height_of_textBoxes;
            left_referenceBorder = (int)Math.Round(0.35F * Overall_panel.Width);
            right_referenceBorder = (int)Math.Round(0.52F * Overall_panel.Width);
            my_textBox = this.KeepTopRankedGenes_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_left_referenceBorder;
            right_referenceBorder = this.KeepTopRankedGenes_ownTextBox.Location.X;
            top_referenceBorder = this.KeepTopRankedGenes_ownTextBox.Location.Y;
            bottom_referenceBorder = this.KeepTopRankedGenes_ownTextBox.Location.Y + this.KeepTopRankedGenes_ownTextBox.Height;
            my_label = this.KeepTopRankedGenes_left_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.KeepTopRankedGenes_ownTextBox.Location.X + this.KeepTopRankedGenes_ownTextBox.Width;
            right_referenceBorder = shared_right_referenceBorder;
            top_referenceBorder = this.KeepTopRankedGenes_ownTextBox.Location.Y;
            bottom_referenceBorder = this.KeepTopRankedGenes_ownTextBox.Location.Y + this.KeepTopRankedGenes_ownTextBox.Height;
            my_label = this.KeepTopRankedGenes_right_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += (int)Math.Round((1F+ fraction_of_rowHeight_between_paragraphs)*shared_row_height);

            #region Delete not significant check box
            left_referenceBorder = shared_left_referenceBorder + Form_default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = this.DeleteNotSignGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DeleteNotSignGenes_cbButton.Location.X + DeleteNotSignGenes_cbButton.Width;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = DeleteNotSignGenes_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = DeleteNotSignGenes_cbButton.Location.Y + DeleteNotSignGenes_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = this.DeleteNotSignGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += (int)Math.Round((1F + fraction_of_rowHeight_between_paragraphs) * shared_row_height);

            #region All genes are significant headline label
            left_referenceBorder = this.SigSelection_panel.Location.X;
            right_referenceBorder = this.SigSelection_panel.Width;
            top_referenceBorder = this.SigSelection_panel.Location.Y + this.SigSelection_panel.Height;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.AllGenesSignificant_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region All Genes are significant checkbox
            left_referenceBorder = this.SigSelection_panel.Location.X + Form_default_settings.Resolution_parameter.Label_X_distance_from_referenceBorder;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = (int)SigSelection_panel.Location.Y + SigSelection_panel.Height + space_for_headline + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton= this.AllGenesSignificant_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = AllGenesSignificant_cbButton.Location.X + AllGenesSignificant_cbButton.Width;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = AllGenesSignificant_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = AllGenesSignificant_cbButton.Location.Y + AllGenesSignificant_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = this.AllGenesSignificant_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Apply and reset significance criteria buttons
            if (!Form_default_settings.Is_mono)
            {
                top_referenceBorder = AllGenesSignificant_cbButton.Location.Y + AllGenesSignificant_cbButton.Height + half_extra_height_for_cbLabels;
                bottom_referenceBorder = (int)Math.Round(0.935 * this.Overall_panel.Height);
            }
            else
            {
                int remove_height = (int)Math.Round(0.01 * this.Overall_panel.Height);
                top_referenceBorder = AllGenesSignificant_cbButton.Location.Y + AllGenesSignificant_cbButton.Height + half_extra_height_for_cbLabels + remove_height;
                bottom_referenceBorder = (int)Math.Round(0.935 * this.Overall_panel.Height) - remove_height;
            }

            left_referenceBorder = shared_distance_from_leftRightSide;
            right_referenceBorder = (int)Math.Round(0.5 * this.Overall_panel.Width - 0.5F * shared_distance_from_leftRightSide);
            my_button = this.ResetSig_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.5 * this.Overall_panel.Width + 0.5F * shared_distance_from_leftRightSide);
            right_referenceBorder = this.Overall_panel.Width - shared_distance_from_leftRightSide;
            my_button = this.ResetParameter_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Tutorial button
            int distance_of_tutorial_button_to_top_and_bottom_border = (int)Math.Round(0.005F * this.Overall_panel.Height);
            top_referenceBorder = this.ResetParameter_button.Location.Y + this.ResetParameter_button.Height + distance_of_tutorial_button_to_top_and_bottom_border;
            bottom_referenceBorder = this.Overall_panel.Height - distance_of_tutorial_button_to_top_and_bottom_border;
            if (bottom_referenceBorder<top_referenceBorder) { throw new Exception(); }
            left_referenceBorder = (int)Math.Round(0.7 * this.Overall_panel.Width);
            right_referenceBorder = this.Overall_panel.Width - shared_distance_from_leftRightSide;
            my_button = this.Tutorial_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Subject only significant genes to enrichment explanation label
            left_referenceBorder = shared_distance_from_leftRightSide;
            right_referenceBorder = this.Tutorial_button.Location.X;
            top_referenceBorder = this.ResetParameter_button.Location.Y + this.ResetParameter_button.Height;
            bottom_referenceBorder = this.Overall_panel.Height;
            my_label = this.SigSubject_explanation_label;
            Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            Set_visualized_options_to_custom_data_options_and_update_boxes(custom_data_options);
        }

        public void Set_visualized_options_to_custom_data_options_and_update_boxes(User_data_options_class custom_data_options)
        {
            this.Visualized_options = custom_data_options.Deep_copy();
            Copy_visualization_options_into_menu_and_update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }

        #region Set sizes and font sizes for dynamic labels
        private void Set_tie_breaker_explanation_label_fontSize_and_size()
        {
            int left_referenceBorder = RankByValue_ownListBox.Location.X + RankByValue_ownListBox.Width;
            int right_referenceBorder = this.SigSelection_panel.Width;
            int top_referenceBorder;
            int bottom_referenceBorder;

            top_referenceBorder = RankByValue_ownListBox.Location.Y;
            bottom_referenceBorder = RankByValue_ownListBox.Location.Y + RankByValue_ownListBox.Height;

            RankByTieBreaker_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }

        private void Set_cutoff_value1st_and_2nd_label_and_expl_label_fontSize_and_size()
        {
            int space_for_headline;
            int shared_row_height;
            int shared_right_referenceBorder;
            int shared_left_referenceBorder;
            float fraction_of_rowHeight_between_paragraphs;
            Get_shared_graphical_parameters(out space_for_headline, out shared_row_height, out shared_right_referenceBorder, out shared_left_referenceBorder, out fraction_of_rowHeight_between_paragraphs);

            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;

            Dictionary<OwnTextBox, MyPanel_label> textBox_defaultLabel_dict = new Dictionary<OwnTextBox, MyPanel_label>();
            textBox_defaultLabel_dict.Add(this.Value1st_cutoff_ownTextBox, this.Value1st_cutoff_myPanelLabel);
            textBox_defaultLabel_dict.Add(this.Value2nd_cutoff_ownTextBox, this.Value2nd_cutoff_myPanelLabel);

            Dictionary<OwnTextBox, MyPanel_label> textBox_explLabel_dict = new Dictionary<OwnTextBox, MyPanel_label>();
            textBox_explLabel_dict.Add(this.Value1st_cutoff_ownTextBox, this.Value1st_cutoff_expl_myPanelLabel);
            textBox_explLabel_dict.Add(this.Value2nd_cutoff_ownTextBox, this.Value2nd_cutoff_expl_myPanelLabel);

            OwnTextBox[] textBoxes = textBox_defaultLabel_dict.Keys.ToArray();
            MyPanel_label current_label;
            int add_height;
            foreach (OwnTextBox textBox in textBoxes)
            {
                current_label = textBox_defaultLabel_dict[textBox];
                left_referenceBorder = shared_left_referenceBorder;
                right_referenceBorder = textBox.Location.X;
                top_referenceBorder = textBox.Location.Y;
                bottom_referenceBorder = textBox.Location.Y + textBox.Height;
                if (Form_default_settings.Is_mono)
                {
                    add_height = (int)Math.Round(0.1 * textBox.Height); //0.15
                    top_referenceBorder -= add_height;
                    bottom_referenceBorder += add_height;
                }
                current_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                current_label.Font_style = System.Drawing.FontStyle.Bold;
                current_label.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            }

            foreach (OwnTextBox textBox in textBoxes)
            {
                current_label = textBox_explLabel_dict[textBox];
                left_referenceBorder = textBox.Location.X + textBox.Width;
                right_referenceBorder = this.SigSelection_panel.Width;
                top_referenceBorder = textBox.Location.Y;
                bottom_referenceBorder = textBox.Location.Y + textBox.Height;
                current_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                current_label.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
            }
        }

        private void Set_dataset_definition_expl_label_size_and_font_size()
        {
            int space_for_headline;
            int shared_row_height;
            int shared_right_referenceBorder;
            int shared_left_referenceBorder;
            float fraction_of_rowHeight_between_paragraphs;
            Get_shared_graphical_parameters(out space_for_headline, out shared_row_height, out shared_right_referenceBorder, out shared_left_referenceBorder, out fraction_of_rowHeight_between_paragraphs);

            int top_referenceBorder = DefineDataset_ownListBox.Location.Y + DefineDataset_ownListBox.Height;
            int bottom_referenceBorder = top_referenceBorder + (int)Math.Round(1.5F * shared_row_height);
            int left_referenceBorder = shared_left_referenceBorder;
            int right_referenceBorder = shared_right_referenceBorder;
            DefineDataset_expl_myPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            DefineDataset_expl_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }
        #endregion

        public void Set_to_visible(User_data_options_class custom_data_options)
        {
            Copy_visualization_options_into_menu_and_update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
            this.Overall_panel.Visible = true;
            this.Overall_panel.Refresh();
        }

        private void Update_buttons_and_labels_to_selections_in_visualization_options(User_data_options_class custom_data_options)
        {
            SigSelection_panel.Visible = !Visualized_options.All_genes_significant;
            if (!Visualized_options.Equals_other(custom_data_options))
            {
                if (Visualized_options.All_genes_significant)
                {
                    ResetSig_button.Text = (string)SigData_text_class.Set_all_genes_to_significant_text.Clone();
                }
                else
                {
                    ResetSig_button.Text = (string)SigData_text_class.Changes_significance_using_both_cutoffs_text.Clone();
                }
                ResetSig_button.Visible = true;
                ResetParameter_button.Visible = true;
            }
            else
            {
                ResetSig_button.Visible = false;
                ResetParameter_button.Visible = false;
            }
            ResetSig_button.Refresh();
            ResetParameter_button.Refresh();
            switch (Visualized_options.Significance_definition_value_1st)
            {
                case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                    this.DirectionValue1st_ownListBox.SilentSelectedIndex = this.DirectionValue1st_ownListBox.Items.IndexOf(SigData_text_class.Higher_string);
                    this.Value1st_cutoff_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Min abs 1.value:");
                    this.Value1st_cutoff_expl_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(SigData_text_class.Eg_log2foldchange_text);
                    break;
                case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                    this.DirectionValue1st_ownListBox.SilentSelectedIndex = this.DirectionValue1st_ownListBox.Items.IndexOf(SigData_text_class.Lower_string);
                    this.Value1st_cutoff_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Max abs 1.value:");
                    this.Value1st_cutoff_expl_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(SigData_text_class.Eg_pvalue_text);
                    break;
                default:
                    throw new Exception();
            }
            switch (Visualized_options.Significance_definition_value_2nd)
            {
                case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                    this.DirectionValue2nd_ownListBox.SilentSelectedIndex = this.DirectionValue2nd_ownListBox.Items.IndexOf(SigData_text_class.Higher_string);
                    this.Value2nd_cutoff_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Min abs 2.value:");
                    this.Value2nd_cutoff_expl_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(SigData_text_class.Eg_minuslog10pvalue_text);
                    break;
                case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                    this.DirectionValue2nd_ownListBox.SilentSelectedIndex = this.DirectionValue2nd_ownListBox.Items.IndexOf(SigData_text_class.Lower_string);
                    this.Value2nd_cutoff_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Max abs 2.value:");
                    this.Value2nd_cutoff_expl_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(SigData_text_class.Eg_pvalue_text);
                    break;
                default:
                    throw new Exception();
            }
            Set_cutoff_value1st_and_2nd_label_and_expl_label_fontSize_and_size();
            switch (Visualized_options.Value_importance_order)
            {
                case Value_importance_order_enum.Value_1st_2nd:
                    this.RankByValue_ownListBox.SilentSelectedIndex_and_topIndex = this.RankByValue_ownListBox.Items.IndexOf(SigData_text_class.Value1st_string);
                    this.RankByTieBreaker_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("then by 2.value");
                    break;
                case Value_importance_order_enum.Value_2nd_1st:
                    this.RankByValue_ownListBox.SilentSelectedIndex_and_topIndex = this.RankByValue_ownListBox.Items.IndexOf(SigData_text_class.Value2nd_string);
                    this.RankByTieBreaker_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("then by 1.value");
                    break;
                default:
                    throw new Exception();
            }
            Set_tie_breaker_explanation_label_fontSize_and_size();
            if (Visualized_options.Merge_upDown_before_ranking)
            {
                this.DefineDataset_ownListBox.SilentSelectedIndex_and_topIndex = this.DefineDataset_ownListBox.Items.IndexOf(SigData_text_class.Dateset_definition_mergedUpAndDown);
                this.DefineDataset_expl_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Unfiltered up/downregulated genes are ranked together (for otherwise matching datasets in same int. group).");
            }
            else
            {
                this.DefineDataset_ownListBox.SilentSelectedIndex_and_topIndex = this.DefineDataset_ownListBox.Items.IndexOf(SigData_text_class.Dateset_definition_separatedUpAndDown);
                this.DefineDataset_expl_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Unfiltered up/downregulated genes are ranked separately (for each dataset in each integration group).");
            }
            Set_dataset_definition_expl_label_size_and_font_size();
            if (Visualized_options.Delete_all_not_significant_genes)
            {
                DeleteNotSignGenes_cbLabel.BackColor = Form_default_settings.Warnings_back_color;
                DeleteNotSignGenes_cbLabel.ForeColor = Form_default_settings.Warnings_fore_color;
            }
            else
            {
                DeleteNotSignGenes_cbLabel.BackColor = Form_default_settings.Color_checkBox_backColor;
                DeleteNotSignGenes_cbLabel.ForeColor = Form_default_settings.Color_checkBox_foreColor;
            }
            DeleteNotSignGenes_cbLabel.Refresh();
            SigSelection_panel.Refresh();
        }

        #region Tutorial
        public void Set_tutorial_button_to_inactive()
        {
            Tutorial_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Tutorial_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
            Tutorial_button.Refresh();
        }
        public void Set_selected_tutorial_button_to_active(Button selected_button)
        {
            selected_button.BackColor = Form_default_settings.Color_button_pressed_back;
            selected_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            selected_button.Refresh();
        }

        public void Tutorial_button_activated()
        {
            int distance_from_overalMenueLabel = Form_default_settings.Distance_of_right_x_of_tutorial_panel_from_menue_panel;
            int right_x_position_next_to_overall_panel = Overall_panel.Location.X - distance_from_overalMenueLabel;
            int mid_y_position;
            int right_x_position;
            string text;

            bool panel_visible = this.SigSelection_panel.Visible;
            bool buttons_visible = this.ResetSig_button.Visible;
            if (!panel_visible)
            { 
                this.SigSelection_panel.Visible = true;
                this.SigSelection_panel.Refresh();
            }
            if (!buttons_visible)
            {
                this.ResetSig_button.Visible = true;
                this.ResetParameter_button.Visible = true;
                this.ResetSig_button.Refresh();
                this.ResetParameter_button.Refresh();
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
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Overall_panel.Location.Y + this.SigSelection_panel.Location.Y + DirectionValue1st_ownListBox.Location.Y + DirectionValue1st_ownListBox.Height;
                        text = "Define if higher (e.g. log2(fold changes)) or lower (e.g. p-values) absolute 1st and 2nd values are more significant.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 1:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Overall_panel.Location.Y + this.SigSelection_panel.Location.Y + Value1st_cutoff_ownTextBox.Location.Y + Value1st_cutoff_ownTextBox.Height;
                        text = "Define a cutoff for the 1st and/or 2nd value.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 2:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Overall_panel.Location.Y + this.SigSelection_panel.Location.Y + (int)Math.Round((float)RankByValue_ownListBox.Location.Y + 0.5F * RankByValue_ownListBox.Height);
                        text = "Define if genes should be ranked by 2nd or 1st value.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 3:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Overall_panel.Location.Y + this.SigSelection_panel.Location.Y + (int)Math.Round((float)this.DefineDataset_ownListBox.Location.Y + 0.5F * DefineDataset_ownListBox.Height);
                        text = "Select if up- and downregulated genes shall be ranked together.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 4:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Overall_panel.Location.Y + this.SigSelection_panel.Location.Y + (int)Math.Round((float)this.KeepTopRankedGenes_ownTextBox.Location.Y + 0.5F * KeepTopRankedGenes_ownTextBox.Height);
                        text = "Define a rank cutoff.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 5:
                        right_x_position = right_x_position_next_to_overall_panel;
                        mid_y_position = Overall_panel.Location.Y + (int)Math.Round((float)this.ResetSig_button.Location.Y + 0.5F * ResetSig_button.Height);
                        text = "Apply both cutoffs simultanously or reset selections to current selections.";
                        UserInterface_tutorial.Set_to_invisible_update_text_move_to_front_and_set_to_visible(text, right_x_position, mid_y_position, ContentAlignment.MiddleRight);
                        UserInterface_tutorial.Wait_until_key_pressed_and_return_key_pressed_information(out escape_pressed, out back_pressed);
                        break;
                    case 6:
                    default:
                        end_tour = true;
                        break;
                }
                if (back_pressed) { tour_box_index = tour_box_index - 2; }
                if ((escape_pressed) || (tour_box_index == -2)) { end_tour = true; }
            }
            UserInterface_tutorial.Set_to_invisible();
            this.SigSelection_panel.Visible = panel_visible;
            this.ResetSig_button.Visible = buttons_visible;
            this.ResetParameter_button.Visible = buttons_visible;
        }
        #endregion

        private void Copy_visualization_options_into_menu_and_update_boxes_based_on_custom_data_and_visualized_options(User_data_options_class custom_data_options)
        {
            this.Value1st_cutoff_ownTextBox.SilentText = Visualized_options.Value_1st_cutoff.ToString();
            this.Value1st_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.Value2nd_cutoff_ownTextBox.SilentText = Visualized_options.Value_2nd_cutoff.ToString();
            this.Value2nd_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.KeepTopRankedGenes_ownTextBox.SilentText = Visualized_options.Keep_top_ranks.ToString();
            this.KeepTopRankedGenes_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            AllGenesSignificant_cbButton.SilentChecked = Visualized_options.All_genes_significant;
            DeleteNotSignGenes_cbButton.SilentChecked = Visualized_options.Delete_all_not_significant_genes;
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
            this.Overall_panel.Refresh();
        }

        public void DirectionValue1st_ownListBox_SelectedIndexChanged(User_data_options_class custom_data_options)
        {
            switch (DirectionValue1st_ownListBox.Text)
            {
                case SigData_text_class.Higher_string:
                    Visualized_options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                    break;
                case SigData_text_class.Lower_string:
                    Visualized_options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
                    break;
                default:
                    throw new Exception();
            }
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
        }
        public void DirectionValue2nd_ownListBox_SelectedIndexChanged(User_data_options_class custom_data_options)
        {
            switch (DirectionValue2nd_ownListBox.Text)
            {
                case SigData_text_class.Higher_string:
                    Visualized_options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
                    break;
                case SigData_text_class.Lower_string:
                    Visualized_options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
                    break;
                default:
                    throw new Exception();
            }
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
        }
        public void Value1st_cutoff_textBox_TextChanged(User_data_options_class custom_data_options)
        {
            float cutoff;
            if  (  (Value1st_cutoff_ownTextBox.Text.Length>0)
                 &&(!Value1st_cutoff_ownTextBox.Text[Value1st_cutoff_ownTextBox.Text.Length-1].Equals('.'))
                 &&(float.TryParse(Value1st_cutoff_ownTextBox.Text, out cutoff)) && ((cutoff >= 0)))
            {
                Visualized_options.Value_1st_cutoff = cutoff;
                Value1st_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
                Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
            }
            else
            {
                Value1st_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }
        public void Value2nd_cutoff_textBox_TextChanged(User_data_options_class custom_data_options)
        {
            float cutoff;
            if (  (Value2nd_cutoff_ownTextBox.Text.Length>0)
                &&(!Value2nd_cutoff_ownTextBox.Text[Value2nd_cutoff_ownTextBox.Text.Length - 1].Equals('.'))
                &&(float.TryParse(Value2nd_cutoff_ownTextBox.Text, out cutoff)) && ((cutoff >= 0)))
            {
                Visualized_options.Value_2nd_cutoff = cutoff;
                Value2nd_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
                Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
            }
            else
            {
                Value2nd_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }

        public void RankByValue_ownListBox_SelectedIndexChanged(User_data_options_class custom_data_options)
        {
            switch (RankByValue_ownListBox.Text)
            {
                case SigData_text_class.Value1st_string:
                    Visualized_options.Value_importance_order = Value_importance_order_enum.Value_1st_2nd;
                    break;
                case SigData_text_class.Value2nd_string:
                    Visualized_options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
                    break;
                default:
                    throw new Exception();

            }
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
        }
        public void DefineDataset_ownListBox_SelectedIndexChanged(User_data_options_class custom_data_options)
        {
            switch (DefineDataset_ownListBox.Text)
            {
                case SigData_text_class.Dateset_definition_mergedUpAndDown:
                    Visualized_options.Merge_upDown_before_ranking = true;
                    break;
                case SigData_text_class.Dateset_definition_separatedUpAndDown:
                    Visualized_options.Merge_upDown_before_ranking = false;
                    break;
                default:
                    throw new Exception();
            }
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
        }
        public void KeepTopRanks_ownTextBox_TextChanged(User_data_options_class custom_data_options)
        {
            int top_ranks;
            if (int.TryParse(KeepTopRankedGenes_ownTextBox.Text, out top_ranks) && (top_ranks > 0))
            {
                Visualized_options.Keep_top_ranks = top_ranks;
                //Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
                KeepTopRankedGenes_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
                Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
            }
            else
            {
                KeepTopRankedGenes_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }
        public void DeleteNotSignGenes_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            Visualized_options.Delete_all_not_significant_genes = DeleteNotSignGenes_cbButton.Checked;
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
        }

        public void AllGeneSignificant_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            Visualized_options.All_genes_significant = AllGenesSignificant_cbButton.Checked;
            Update_buttons_and_labels_to_selections_in_visualization_options(custom_data_options);
        }
        public Custom_data_class ResetSig_button_Click(Custom_data_class custom_data)
        {
            custom_data.Options = Visualized_options.Deep_copy();
            custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
            Copy_visualization_options_into_menu_and_update_boxes_based_on_custom_data_and_visualized_options(custom_data.Options);
            return custom_data;
        }
        public void ResetParameter_button_Click(User_data_options_class custom_data_options)
        {
            this.Visualized_options = custom_data_options.Deep_copy();
            Copy_visualization_options_into_menu_and_update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }

    }
}
