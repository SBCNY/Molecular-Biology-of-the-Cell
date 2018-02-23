#region Author information
/*
The code was written by Jens Hansen working for the Ravi Iyengar Lab
The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
Please acknowledge the MBC Ontology in your publications by citing the following reference:
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: 
A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes
Sci Rep. 2017 Dec18th
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using MBCO;
using Network;
using Leave_out;
using Common_functions.Array_own;
using Common_functions.Global_definitions;
using Common_functions.Report;
using yed_network;

namespace Enrichment
{
    enum Data_value_signs_of_interest_enum { E_m_p_t_y, Upregulated, Downregulated, Combined }

    class Mbc_add_inferred_parentSCP_line_class
    {
        public string Inferred_parentSCP { get; set; }
        public string[] Children_scps { get; set; }
        public int ParentScp_processLevel { get; set; }

        public Mbc_add_inferred_parentSCP_line_class Deep_copy()
        {
            Mbc_add_inferred_parentSCP_line_class copy = (Mbc_add_inferred_parentSCP_line_class)this.MemberwiseClone();
            copy.Inferred_parentSCP = (string)this.Inferred_parentSCP.Clone();
            copy.Children_scps = Array_class.Deep_copy_string_array(this.Children_scps);
            return copy;
        }
    }

    class Mbc_enrichment_pipeline_options_class
    {
        #region Fields for general Options
        public bool Report { get; set; }
        #endregion

        #region Fields for standard and dynamic enrichment analysis
        public Data_value_signs_of_interest_enum[] Data_value_signs_of_interest { get; set; }
        public float Maximum_pvalue_for_standardDynamicEnrichment { get; set; }
        #endregion

        #region Fields for standard enrichment analysis
        public int[] Kept_top_predictions_standardEnrichment_per_level { get; set; }
        #endregion

        #region Fields for dynamic enrichment analysis
        public float[] Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level { get; set; }
        public float[] Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level { get; set; }
        public int[] Scp_levels_for_dynamicEnrichment { get; set; }
        public int[][] Numbers_of_merged_scps_for_dynamicEnrichment_per_level { get; set; }
        public int[] Kept_top_predictions_dynamicEnrichment_per_level { get; set; }
        public int[] Kept_singleSCPs_dynamicEnrichment_per_level { get; set; }
        public bool Consider_interactions_between_signalingSCPs_for_dyanmicEnrichment { get; set; }
        #endregion

        public Mbc_enrichment_pipeline_options_class()
        {
            #region Parameter for general options
            Report = true;
            #endregion

            #region Parameter for standard and dynamic enrichment analysis
            Data_value_signs_of_interest = new Data_value_signs_of_interest_enum[] { Data_value_signs_of_interest_enum.Combined, Data_value_signs_of_interest_enum.Upregulated, Data_value_signs_of_interest_enum.Downregulated };
            Maximum_pvalue_for_standardDynamicEnrichment = 1;
            #endregion

            #region Fields for standard enrichment analysis
            Kept_top_predictions_standardEnrichment_per_level = new int[] { -1, 5, 5, 10, 5 };
            #endregion

            #region Fields for dynamic enrichment analysis
            Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = new float[] { -1, -1, 0.2F, 0.25F, -1 };
            Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level = new float[] { -1, -1, 1, 1, -1 };
            Scp_levels_for_dynamicEnrichment = new int[] { 3 };
            Numbers_of_merged_scps_for_dynamicEnrichment_per_level = new int[5][];
            Numbers_of_merged_scps_for_dynamicEnrichment_per_level[3] = new int[] { 2, 3 };
            Kept_top_predictions_dynamicEnrichment_per_level = new int[] { -1, -1, 3, 5 };
            Kept_singleSCPs_dynamicEnrichment_per_level = new int[] { -1, -1, 99, 99, -1 };
            Consider_interactions_between_signalingSCPs_for_dyanmicEnrichment = false;
            #endregion
        }

        public void Print_options()
        {
            Report_class.WriteLine("{0}: Print options", typeof(Mbc_enrichment_pipeline_options_class).Name);
            int right_shift = 4;


            #region Write general options
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("General:");
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("Report: {0}", Report);
            Report_class.WriteLine();
            #endregion

            #region Write parameter for both standard and dynamic enrichment analysis
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("Parameter for both standard and dynamic enrichment analysis:");
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Data value signs of interest:");
            int signs_of_interest_length = Data_value_signs_of_interest.Length;
            for (int indexS=0; indexS<signs_of_interest_length; indexS++)
            {
                if (indexS!=0) { Report_class.Write(","); }
                Report_class.Write(" {0}", Data_value_signs_of_interest[indexS]);
            }
            Report_class.WriteLine();
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("Maximum pvalue: {0}", Maximum_pvalue_for_standardDynamicEnrichment);
            Report_class.WriteLine();
            #endregion

            #region Write parameter for standard enrichment analysis
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("Parameter for standard enrichment analysis only:");
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Keep top predictions:");
            int keep_top_length = Kept_top_predictions_standardEnrichment_per_level.Length;
            for (int level=1; level<keep_top_length; level++)
            {
                if (level != 1) { Report_class.Write(", "); } 
                Report_class.Write(" Level {0}: {1}", level, Kept_top_predictions_standardEnrichment_per_level[level]);
            }
            Report_class.WriteLine();
            Report_class.WriteLine();
            #endregion

            #region Write parameter for dyanmic enrichment analysis
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("Parameter for dynamic enrichment analysis only:");
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Do dynamic enrichment for: ");
            int scp_levels_for_dynamicEnrichment_length = Scp_levels_for_dynamicEnrichment.Length;
            for (int indexLevel = 0; indexLevel < scp_levels_for_dynamicEnrichment_length; indexLevel++)
            {
                if ((indexLevel != 0) && (indexLevel==scp_levels_for_dynamicEnrichment_length-1)) { Report_class.Write(" and "); }
                else if (indexLevel != 0) { Report_class.Write(", "); }
                Report_class.Write(" Level {0}", Scp_levels_for_dynamicEnrichment[indexLevel]);
            }
            Report_class.WriteLine();
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.WriteLine("Number of neighboring SCPs that are merged to SCP unions:");
            for (int indexLevel = 0; indexLevel < scp_levels_for_dynamicEnrichment_length; indexLevel++)
            {
                int level = Scp_levels_for_dynamicEnrichment[indexLevel];
                int[] merge_scps_current_level = Numbers_of_merged_scps_for_dynamicEnrichment_per_level[level];
                for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
                Report_class.Write("To generate level-{0} SCP unions merge ", level);
                int merge_length = merge_scps_current_level.Length;
                for (int indexM = 0; indexM < merge_length; indexM++)
                {
                    if ((indexM != 0)&&(indexM==merge_length-1)) { Report_class.Write(" or "); }
                    else if (indexM != 0) { Report_class.Write(", "); }
                    Report_class.Write("{0}", merge_scps_current_level[indexM]);
                }
                Report_class.WriteLine(" SCPs");
            }

            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Top quantile of scp interactions that are considered to identify neighboring SCPs: ");
            int top_quantile_length = Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Length;
            for (int indexLevel = 0; indexLevel < scp_levels_for_dynamicEnrichment_length; indexLevel++)
            {
                int level = Scp_levels_for_dynamicEnrichment[indexLevel];
                if (indexLevel != 0) { Report_class.Write(", "); } 
                Report_class.WriteLine("Level {0}: {1}", level, Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[level]);
            }
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Top quantile of scp interactions that are considered to visualize SCP network:");
            top_quantile_length = Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level.Length;
            for (int indexLevel = 0; indexLevel < scp_levels_for_dynamicEnrichment_length; indexLevel++)
            {
                int level = Scp_levels_for_dynamicEnrichment[indexLevel];
                if (indexLevel != 1) { Report_class.Write(", "); }
                Report_class.Write("Level {0}: {1}", level, Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level[level]);
            }
            Report_class.WriteLine();
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Keep top predicted SCP unions or single SCPs: ");
            top_quantile_length = Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level.Length;
            for (int indexLevel = 0; indexLevel < scp_levels_for_dynamicEnrichment_length; indexLevel++)
            {
                int level = Scp_levels_for_dynamicEnrichment[indexLevel];
                if (indexLevel != 0) { Report_class.Write(", "); }
                Report_class.Write("Level {0}: {1}", level, Kept_top_predictions_dynamicEnrichment_per_level[level]);
            }
            Report_class.WriteLine();
            for (int i = 0; i < right_shift; i++) { Report_class.Write(" "); }
            Report_class.Write("Keep not more than x SCPs as part of a single SCP prediction or a predicted SCP-union: ");
            top_quantile_length = Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level.Length;
            for (int indexLevel = 0; indexLevel < scp_levels_for_dynamicEnrichment_length; indexLevel++)
            {
                int level = Scp_levels_for_dynamicEnrichment[indexLevel];
                if (indexLevel != 0) { Report_class.Write(", "); }
                Report_class.WriteLine("Level {0}: {1}", level, Kept_singleSCPs_dynamicEnrichment_per_level[level]);
            }
            Report_class.WriteLine();
            #endregion
        }
    }

    class Mbc_enrichment_pipeline_class
    {
        #region Fields
        public MBCO_association_class MBCO_association { get; set; }
        public Ontology_fisher_class MBCO_fisher_standard { get; set; }
        public Leave_out_scp_scp_network_class Leave_out_scp_network_for_dynamicEnrichment_analysis { get; set; }
        public Leave_out_scp_scp_network_class Leave_out_scp_network_for_dynamicEnrichment_visualization { get; set; }
        public Mbc_enrichment_pipeline_options_class Options { get; set; }
        public MBCO_obo_network_class Mbco_parentChild_nw { get; set; }
        public Ontology_enrichment_class Onto_enrich_combined { get; set; }
        public string[] Exp_bg_genes { get; set; }
        public string[] Final_bg_genes { get; set; }
        public string Base_file_name { get; set; }
        #endregion

        public Mbc_enrichment_pipeline_class()
        {
            Options = new Mbc_enrichment_pipeline_options_class();
            Mbco_parentChild_nw = new MBCO_obo_network_class();
            Leave_out_scp_network_for_dynamicEnrichment_analysis = new Leave_out_scp_scp_network_class();
            Leave_out_scp_network_for_dynamicEnrichment_visualization = new Leave_out_scp_scp_network_class();
        }

        #region Generate
        private void Generate_parent_child_network()
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Generate hierarchical parent child MBCO network", typeof(Mbc_enrichment_pipeline_class).Name); }
            Mbco_parentChild_nw.Generate_by_reading_safed_obo_file();
        }

        private void Generate_mbco_association(string[] bg_genes)
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Generate MBCO gene-SCP association networks", typeof(Mbc_enrichment_pipeline_class).Name); }
            MBCO_association = new MBCO_association_class();
            MBCO_association.Generate_by_reading_safed_file();
            string[] all_mbco_genes = MBCO_association.Get_all_distinct_ordered_symbols();
            if (bg_genes.Length > 0)
            {
                this.Exp_bg_genes = Array_class.Deep_copy_string_array(bg_genes);
                this.Final_bg_genes = Overlap_class.Get_intersection(this.Exp_bg_genes, all_mbco_genes);
            }
            else
            {
                this.Exp_bg_genes = new string[0];
                this.Final_bg_genes = Array_class.Deep_copy_string_array(all_mbco_genes);
            }
            this.MBCO_association.Keep_only_bg_symbols(this.Final_bg_genes);
            this.MBCO_association.Remove_background_genes_scp();
            this.MBCO_fisher_standard = new Ontology_fisher_class();
            this.MBCO_fisher_standard.Generate(this.MBCO_association, Ontology_type_enum.Molecular_biology_cell, this.Final_bg_genes);
        }

        private void Generate_leave_out_scp_network()
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Generate SCP networks", typeof(Mbc_enrichment_pipeline_class).Name); }
            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_after_setting_process_level();

            Leave_out_class leave_out = new Leave_out_class();
            leave_out.Generate_by_reading_safed_file();
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Options.Consider_scp_interactions_between_signaling_processes = this.Options.Consider_interactions_between_signalingSCPs_for_dyanmicEnrichment;
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Options.Top_quantile_of_considered_SCP_interactions_per_level = this.Options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level;
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Generate_scp_scp_network_from_leave_out(leave_out);
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);

            Leave_out_scp_network_for_dynamicEnrichment_visualization.Options.Consider_scp_interactions_between_signaling_processes = this.Options.Consider_interactions_between_signalingSCPs_for_dyanmicEnrichment;
            Leave_out_scp_network_for_dynamicEnrichment_visualization.Options.Top_quantile_of_considered_SCP_interactions_per_level = this.Options.Top_quantile_of_scp_interactions_for_visualization_of_dynamicEnrichment_per_level;
            Leave_out_scp_network_for_dynamicEnrichment_visualization.Generate_scp_scp_network_from_leave_out(leave_out);
            Leave_out_scp_network_for_dynamicEnrichment_visualization.Scp_nw.Transform_into_undirected_single_network_and_set_all_widths_to_one();
            Leave_out_scp_network_for_dynamicEnrichment_visualization.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
        }

        public void Generate(string[] bg_genes)
        {
            Generate_parent_child_network();
            Generate_mbco_association(bg_genes);
            Generate_leave_out_scp_network();
        }
        #endregion

        #region Standard enrichment analysis with MBCO and other ontologies
        private void Write_parent_child_network_for_results_of_standard_enrichment_analysis(Ontology_enrichment_class onto_enrich_mbc_for_nw_visualization)
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Visualize parent child relationships of standard enrichment results", typeof(Mbc_enrichment_pipeline_class).Name); }
            string mbc_results_subdirectory = Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum.Molecular_biology_cell);
            int enrich_length = onto_enrich_mbc_for_nw_visualization.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> sameLevel_enrich_list = new List<Ontology_enrichment_line_class>();
            onto_enrich_mbc_for_nw_visualization.Order_by_complete_sample_pvalue();
            MBCO_obo_network_class current_obo_mbc;
            string complete_sampleName;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = onto_enrich_mbc_for_nw_visualization.Enrich[indexE];
                if ((indexE == 0) || (!enrich_line.Complete_sample_name.Equals(onto_enrich_mbc_for_nw_visualization.Enrich[indexE - 1].Complete_sample_name)))
                {
                    sameLevel_enrich_list.Clear();
                }
                sameLevel_enrich_list.Add(enrich_line);
                if ((indexE == enrich_length - 1) || (!enrich_line.Complete_sample_name.Equals(onto_enrich_mbc_for_nw_visualization.Enrich[indexE + 1].Complete_sample_name)))
                {
                    complete_sampleName = enrich_line.Complete_sample_name;
                    current_obo_mbc = this.Mbco_parentChild_nw.Deep_copy_mbco_obo_nw();
                    current_obo_mbc.Add_significance_and_remove_unsignificant_nodes_but_keep_all_ancestors(sameLevel_enrich_list.ToArray());
                    current_obo_mbc.Write_yED_nw_in_results_directory_with_nodes_colored_by_minusLog10Pvalue_without_sameLevel_processes_grouped(mbc_results_subdirectory + complete_sampleName + "_parentChild_nw");
                }
            }
        }

        private Ontology_enrichment_class Do_standard_enrichment_analysis_and_write_results(Data_class data, out Ontology_enrichment_class onto_enrich)
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Do standard enrichment analysis and write results", typeof(Mbc_enrichment_pipeline_class).Name); }

            onto_enrich = this.MBCO_fisher_standard.Analyse_data_instance(data);

            Ontology_type_enum ontology = Ontology_type_enum.Molecular_biology_cell;
            string subdirectory = Get_results_subdirectory_for_indicated_ontology(ontology);
            onto_enrich.Write(subdirectory, "Standard_enrichment_results.txt");

            Ontology_enrichment_class onto_enrich_filtered = onto_enrich.Deep_copy();
            onto_enrich_filtered.Keep_enrichment_lines_below_pvalue_cutoff(Options.Maximum_pvalue_for_standardDynamicEnrichment);
            onto_enrich_filtered.Keep_top_x_predictions_per_level_for_each_sample(Options.Kept_top_predictions_standardEnrichment_per_level);
            onto_enrich_filtered.Write(subdirectory, "Standard_enrichment_results_filtered.txt");
            return onto_enrich_filtered;
        }
        #endregion

        #region Dynamic enrichment analysis
        private MBCO_association_line_class[] Generate_new_mbco_association_lines_by_merging_neighboring_scps_for_one_level(Ontology_enrichment_line_class[] sameSampleLevel_mbco_enrich_lines_standard)
        {
            int level = sameSampleLevel_mbco_enrich_lines_standard[0].ProcessLevel;
            int sameSample_length = sameSampleLevel_mbco_enrich_lines_standard.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            List<string> scpNames_list = new List<string>();
            for (int indexE = 0; indexE < sameSample_length; indexE++)
            {
                onto_enrich_line = sameSampleLevel_mbco_enrich_lines_standard[indexE];
                if (onto_enrich_line.ProcessLevel != level) { throw new System.Exception("more than one level"); }
                if (  (indexE!=0)
                    &&(!onto_enrich_line.Equal_complete_sample(sameSampleLevel_mbco_enrich_lines_standard[indexE-1])))
                {
                    throw new Exception("More than one sample");
                }
                scpNames_list.Add(onto_enrich_line.Scp_name);
            }
            if (scpNames_list.Distinct().ToArray().Length != scpNames_list.Count) { throw new Exception(); }
            Dictionary<string,string[]> scpNames_scpUnionNames_dict = Leave_out_scp_network_for_dynamicEnrichment_analysis.Generate_scpNames_scpUnionNames_dict_with_indicated_numbers_of_combined_scps(this.Options.Numbers_of_merged_scps_for_dynamicEnrichment_per_level[level], scpNames_list.ToArray());
            string[] scpNames = scpNames_scpUnionNames_dict.Keys.ToArray();
            string scpName;
            string[] scpUnionNames;
            string scpUnionName;
            int scpUnionNames_length;
            int scpNames_length = scpNames.Length;
            scpNames = scpNames.OrderBy(l => l).ToArray();
            this.MBCO_association.Order_by_processName_symbol();
            int mbco_length = this.MBCO_association.MBCO_associations.Length;
            int indexMBCO = 0;
            int stringCompare = -2;
            MBCO_association_line_class mbc_association_line;
            MBCO_association_line_class scpUnion_mbc_association_line;
            List<MBCO_association_line_class> scpUnion_mbc_association_list = new List<MBCO_association_line_class>();
            for (int indexScp=0; indexScp<scpNames_length; indexScp++)
            {
                scpName = scpNames[indexScp];
                stringCompare = -2;
                while ((indexMBCO<mbco_length)&&(stringCompare<=0))
                {
                    mbc_association_line = this.MBCO_association.MBCO_associations[indexMBCO];
                    stringCompare = mbc_association_line.ProcessName.CompareTo(scpName);
                    if (stringCompare<0)
                    {
                        indexMBCO++;
                    }
                    else if (stringCompare==0)
                    {
                        scpUnionNames = scpNames_scpUnionNames_dict[scpName];
                        scpUnionNames_length = scpUnionNames.Length;
                        for (int indexUnion = 0; indexUnion < scpUnionNames_length; indexUnion++)
                        {
                            scpUnionName = scpUnionNames[indexUnion];
                            scpUnion_mbc_association_line = new MBCO_association_line_class();
                            scpUnion_mbc_association_line.ProcessID = "SCP union";
                            scpUnion_mbc_association_line.ProcessName = (string)scpUnionName.Clone();
                            scpUnion_mbc_association_line.ProcessLevel = mbc_association_line.ProcessLevel;
                            scpUnion_mbc_association_line.Symbol = (string)mbc_association_line.Symbol.Clone();
                            scpUnion_mbc_association_line.Parent_processName = "SCP union";
                            scpUnion_mbc_association_list.Add(scpUnion_mbc_association_line);
                        }
                        indexMBCO++;
                    }
                }
            }

            List<MBCO_association_line_class> final_scpUnion_mbc_association_list = new List<MBCO_association_line_class>();
            scpUnion_mbc_association_list = scpUnion_mbc_association_list.OrderBy(l => l.ProcessName).ThenBy(l=>l.Symbol).ToList();
            int scpUnion_length = scpUnion_mbc_association_list.Count;
            for (int indexScpUnion=0; indexScpUnion < scpUnion_length; indexScpUnion++)
            {
                mbc_association_line = scpUnion_mbc_association_list[indexScpUnion];
                if (  (indexScpUnion==0)
                    || (!mbc_association_line.ProcessName.Equals(scpUnion_mbc_association_list[indexScpUnion - 1].ProcessName))
                    || (!mbc_association_line.Symbol.Equals(scpUnion_mbc_association_list[indexScpUnion - 1].Symbol)))
                {
                    final_scpUnion_mbc_association_list.Add(mbc_association_line);
                }
            }
            return final_scpUnion_mbc_association_list.ToArray();
        }

        private MBCO_association_line_class[] Generate_new_mbco_association_lines_by_merging_neighboring_scps(Ontology_enrichment_line_class[] sameSample_mbco_enrich_lines_standard)
        {
            sameSample_mbco_enrich_lines_standard = sameSample_mbco_enrich_lines_standard.OrderBy(l => l.ProcessLevel).ThenBy(l=>l.Complete_sample_name).ToArray();
            List<Ontology_enrichment_line_class> sameSampleLevel_mbco_enrich_lines_list = new List<Ontology_enrichment_line_class>();
            int sameSample_length = sameSample_mbco_enrich_lines_standard.Length;
            Ontology_enrichment_line_class enrich_line;
            MBCO_association_line_class[] new_mbco_association_lines;
            List<MBCO_association_line_class> new_mbco_association_list = new List<MBCO_association_line_class>();
            for (int indexE=0; indexE<sameSample_length; indexE++)
            {
                enrich_line = sameSample_mbco_enrich_lines_standard[indexE];
                if ((indexE!=0) && (!enrich_line.Complete_sample_name.Equals(sameSample_mbco_enrich_lines_standard[indexE-1].Complete_sample_name)))
                {
                    throw new Exception("multiple samples");
                }
                if (  (indexE==0)
                    || (!enrich_line.ProcessLevel.Equals(sameSample_mbco_enrich_lines_standard[indexE - 1].ProcessLevel)))
                {
                    sameSampleLevel_mbco_enrich_lines_list.Clear();
                }
                sameSampleLevel_mbco_enrich_lines_list.Add(enrich_line);
                if (  (indexE == sameSample_length-1)
                    || (!enrich_line.ProcessLevel.Equals(sameSample_mbco_enrich_lines_standard[indexE + 1].ProcessLevel)))
                {
                    if (Options.Scp_levels_for_dynamicEnrichment.Contains(enrich_line.ProcessLevel))
                    {
                        new_mbco_association_lines = Generate_new_mbco_association_lines_by_merging_neighboring_scps_for_one_level(sameSampleLevel_mbco_enrich_lines_list.ToArray());
                        new_mbco_association_list.AddRange(new_mbco_association_lines);
                    }
                }
            }
            return new_mbco_association_list.ToArray();
        }

        private void Write_scp_network_for_results_of_dynamic_enrichment_analsyis(Ontology_enrichment_class dynamic_onto_enrichment_filtered, Ontology_enrichment_class standard_onto_enrichment_filtered)
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Visualize SCP relationships of dynamic enrichment results", typeof(Mbc_enrichment_pipeline_class).Name); }
            string subdirectory = Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum.Molecular_biology_cell);

            dynamic_onto_enrichment_filtered.Order_by_complete_sample_pvalue();
            standard_onto_enrichment_filtered.Order_by_complete_sample_pvalue();

            Leave_out_class leave_out = new Leave_out_class();
            leave_out.Generate_by_reading_safed_file();

            List<Ontology_enrichment_line_class> sameSample_ontology_enrichment = new List<Ontology_enrichment_line_class>();
            int dynamic_onto_enrich_length = dynamic_onto_enrichment_filtered.Enrich.Length;
            Ontology_enrichment_line_class enrichment_line;

            Leave_out_scp_scp_network_class current_scp_network;
            List<string> current_scpNames = new List<string>();

            string complete_sampleName;
            string[] standard_enriched_scps;
            Dictionary<string, Shape_enum> nodeLable_shape_dict = new Dictionary<string, Shape_enum>();
            for (int indexE=0; indexE< dynamic_onto_enrich_length; indexE++)
            {
                enrichment_line = dynamic_onto_enrichment_filtered.Enrich[indexE];
                if ((indexE==0)||(!enrichment_line.Equal_complete_sample(dynamic_onto_enrichment_filtered.Enrich[indexE-1])))
                {
                    sameSample_ontology_enrichment.Clear();
                }
                sameSample_ontology_enrichment.Add(enrichment_line);
                if ((indexE == dynamic_onto_enrich_length - 1) || (!enrichment_line.Equal_complete_sample(dynamic_onto_enrichment_filtered.Enrich[indexE + 1])))
                {
                    current_scpNames.Clear();
                    foreach (Ontology_enrichment_line_class sameSample_enrichment_line in sameSample_ontology_enrichment)
                    {
                        current_scpNames.AddRange(sameSample_enrichment_line.Scp_name.Split(Global_class.Scp_delimiter));
                    }
                    complete_sampleName = enrichment_line.Complete_sample_name;
                    current_scp_network = Leave_out_scp_network_for_dynamicEnrichment_visualization.Deep_copy_scp_network();
                    current_scp_network.Scp_nw.Keep_only_input_nodeNames(current_scpNames.ToArray());
                    current_scp_network.Add_ancestors_of_missing_levels(this.Mbco_parentChild_nw);
                    standard_enriched_scps = standard_onto_enrichment_filtered.Get_all_scps_of_completeSample(complete_sampleName);
                    nodeLable_shape_dict.Clear();
                    foreach (string standard_enriched_scp in standard_enriched_scps)
                    {
                        nodeLable_shape_dict.Add(standard_enriched_scp, Shape_enum.Rectangle);
                    }
                    current_scp_network.Scp_nw.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_sameLevel_processes_grouped(subdirectory + complete_sampleName + "_dynamicEnrichment_nw", Shape_enum.Diamond, nodeLable_shape_dict);
                }
            }
        }

        private Ontology_enrichment_class Do_dynamic_enrichment_analysis_for_mbco_and_write(Ontology_enrichment_class mbco_onto_enrich_standard, Data_class data)
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Do dynamic enrichment analysis and write results", typeof(Mbc_enrichment_pipeline_class).Name); }
            string subdirectory = Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum.Molecular_biology_cell);

            mbco_onto_enrich_standard.Order_by_complete_sample_pvalue();
            int mbco_onto_enrich_length = mbco_onto_enrich_standard.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            MBCO_association_line_class[] new_mbco_association_lines;
            MBCO_association_class new_mbco_association;
            List<Ontology_enrichment_line_class> current_onto_enrich = new List<Ontology_enrichment_line_class>();
            Ontology_fisher_class mbco_fisher_dynamic;
            int col_length = data.ColChar.Columns.Length;
            Data_class current_data;
            Colchar_column_line_class current_column_line;
            Ontology_enrichment_class current_dynamic_onto_enrich;
            Ontology_enrichment_class complete_dynamic_onto_enrich = new Ontology_enrichment_class();
            List<Ontology_enrichment_line_class> dynamic_onto_enrich_list = new List<Ontology_enrichment_line_class>();
            int added_standard_results_count = 0;
            for (int indexCol = 0; indexCol < col_length; indexCol++)
            {
                current_data = data.Deep_copy();
                current_data.Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(indexCol);
                current_column_line = current_data.ColChar.Columns[0];
                current_onto_enrich.Clear();
                for (int indexMbco = 0; indexMbco < mbco_onto_enrich_length; indexMbco++)
                {
                    enrich_line = mbco_onto_enrich_standard.Enrich[indexMbco];
                    if ((enrich_line.EntryType.Equals(current_column_line.EntryType))
                        && (enrich_line.Timepoint.Equals(current_column_line.Timepoint))
                        && (enrich_line.Sample_name.Equals(current_column_line.SampleName)))
                    {
                        current_onto_enrich.Add(enrich_line);
                        added_standard_results_count++;
                    }
                }
                if (current_onto_enrich.Count==0) { throw new Exception(); }
                new_mbco_association_lines = Generate_new_mbco_association_lines_by_merging_neighboring_scps(current_onto_enrich.ToArray());
                new_mbco_association = new MBCO_association_class();
                new_mbco_association.Add_to_array(new_mbco_association_lines);
                mbco_fisher_dynamic = new Ontology_fisher_class();
                mbco_fisher_dynamic.Generate(new_mbco_association, Ontology_type_enum.Molecular_biology_cell, this.Final_bg_genes);
                current_dynamic_onto_enrich = mbco_fisher_dynamic.Analyse_data_instance(current_data);
                complete_dynamic_onto_enrich.Add_other(current_dynamic_onto_enrich);
            }
            if (added_standard_results_count != mbco_onto_enrich_standard.Enrich.Length) { throw new Exception(); }

            complete_dynamic_onto_enrich.Add_other(mbco_onto_enrich_standard);
            complete_dynamic_onto_enrich.Keep_only_enrichment_lines_of_indicated_levels(Options.Scp_levels_for_dynamicEnrichment);
            complete_dynamic_onto_enrich.Write(subdirectory, "Dynamic_enrichment_results.txt");

            Ontology_enrichment_class complete_dynamic_onto_enrich_filtered = complete_dynamic_onto_enrich.Deep_copy();
            complete_dynamic_onto_enrich_filtered.Keep_enrichment_lines_below_pvalue_cutoff(Options.Maximum_pvalue_for_standardDynamicEnrichment);
            complete_dynamic_onto_enrich_filtered.Keep_top_x_predictions_per_level_for_each_sample(Options.Kept_top_predictions_dynamicEnrichment_per_level);
            complete_dynamic_onto_enrich_filtered.Keep_top_x_predictedSCPs_as_part_of_SCPunit_or_singleSCPs(Options.Kept_singleSCPs_dynamicEnrichment_per_level);
            complete_dynamic_onto_enrich_filtered.Write(subdirectory, "Dynamic_enrichment_results_filtered.txt");
            return complete_dynamic_onto_enrich_filtered;
        }
        #endregion

        #region Prepare data
        private Data_class Generate_data_instance_with_separated_or_combined_entries(Data_class data)
        {
            if (Options.Report) { Report_class.WriteLine("{0}: Prepare data for enrichment analysis", typeof(Mbc_enrichment_pipeline_class).Name); }
            bool positive_entries_exist = false;
            bool negative_entries_exist = false;
            int data_length = data.Data_length;
            Data_line_class data_line;
            int col_length = data.ColChar.Columns_length;
            for (int indexData=0; indexData<data_length; indexData++)
            {
                data_line = data.Data[indexData];
                for (int indexCol = 0; indexCol<col_length; indexCol++)
                {
                    if (data_line.Columns[indexCol]>0)
                    {
                        positive_entries_exist = true;
                    }
                    else if (data_line.Columns[indexCol]<0)
                    {
                        negative_entries_exist = true;
                    }
                }
            }

            if ((positive_entries_exist) && (negative_entries_exist))
            {
                Data_class data_sep = new Data_class();
                if (Options.Data_value_signs_of_interest.Contains(Data_value_signs_of_interest_enum.Combined))
                {
                    data_sep.Add_other_data_instance(data);
                }
                if (Options.Data_value_signs_of_interest.Contains(Data_value_signs_of_interest_enum.Upregulated))
                {
                    data_sep.Add_other_data_instance(data.Get_data_instance_with_only_upregulated_entries());
                }
                if (Options.Data_value_signs_of_interest.Contains(Data_value_signs_of_interest_enum.Downregulated))
                {
                    data_sep.Add_other_data_instance(data.Get_data_instance_with_only_downregulated_entries());
                }
                return data_sep;
            }
            else
            {
                return data;
            }
        }
        #endregion

        #region Directories
        private string Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum ontology)
        {
            return this.Base_file_name + "_" + ontology + "//";
        }
        #endregion

        public void Analyze_de_instance(Data_class data, string base_file_name)
        {
            this.Base_file_name = (string)base_file_name.Clone();
            Data_class data_sep = Generate_data_instance_with_separated_or_combined_entries(data);
            data_sep.Set_all_ncbi_official_gene_symbols_to_upper_case();
            Ontology_enrichment_class standard_onto_enrich_standard;
            Ontology_enrichment_class standard_onto_enrich_filtered = Do_standard_enrichment_analysis_and_write_results(data_sep, out standard_onto_enrich_standard);
            Ontology_enrichment_class dynamic_onto_enrich_filtered = Do_dynamic_enrichment_analysis_for_mbco_and_write(standard_onto_enrich_standard, data_sep);

            Write_parent_child_network_for_results_of_standard_enrichment_analysis(standard_onto_enrich_filtered);
            Write_scp_network_for_results_of_dynamic_enrichment_analsyis(dynamic_onto_enrich_filtered, standard_onto_enrich_filtered);
            Write_legend_for_networks(base_file_name);
        }

        private void Write_legend_for_networks(string base_file_name)
        {
            string directory = Global_directory_and_file_class.Results_directory + Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum.Molecular_biology_cell);
            string complete_fileName = directory + "Legend_for_networks.txt";
            System.IO.StreamWriter writer = new System.IO.StreamWriter(complete_fileName);
            writer.WriteLine("Nodes in parent child SCP network that were identified based on standard enrichment:");
            writer.WriteLine("Colored SCP-nodes indicate SCPs that were identified based on standard enrichment analysis after filtering (see Standard_enrichment_results_filtered.txt).");
            writer.WriteLine("Colors correspond to minus log10(p-value).");
            writer.WriteLine("Gray SCP-nodes are offspring nodes of identified SCPs that did not survive the filtering by themselves.");
            writer.WriteLine("Arrows point from parent to children SCPs.");
            writer.WriteLine();
            writer.WriteLine("Nodes in SCP network based on dynamic enrichment:");
            writer.WriteLine("Squared SCP-nodes indicate SCPs that were also identified based on standard enrichment analysis after filtering (see Standard_enrichment_results_filtered.txt).");
            writer.WriteLine("Diamond SCP-nodes indicate SCPs that were only identified based on dynamic enrichment analysis.");
            writer.WriteLine("SCP-nodes are colored based on SCP-level: level 1: dark red, level 2: light red, level 3: blue.");
            writer.WriteLine("Arrows point from parent to children SCPs.");
            writer.WriteLine("Dashed lines connect SCPs that are related to each other.");
            writer.WriteLine("Blue level-3 SCP nodes were identified based on dynamic enrichment analysis.");
            writer.WriteLine("Darkred level-1 and lightred level-2 SCP nodes are grandparents and parents of the identified level-3 SCPs.");
            writer.WriteLine();
            writer.WriteLine("Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar:");
            writer.WriteLine("A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes");
            writer.WriteLine("bioRxiv  112201;  doi: https://doi.org/10.1101/112201");
            writer.Close();
        }
    }
}
