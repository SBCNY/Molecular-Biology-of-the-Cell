//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections.Generic;
using System.Linq;
using Common_functions.ReadWrite;
using Common_functions.Global_definitions;
using Common_functions.Array_own;
using Common_functions.Form_tools;
using Network;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.IO;

namespace MBCO
{
    class MBCO_association_line_class
    {
        #region Fields
        public int ProcessLevel { get; set; }
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string Parent_processName { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string[] References { get; set; }
        public Manual_validation_enum Manual_validation { get; set; }

        public string ReadWrite_references
        {
            get { return ReadWriteClass.Get_writeLine_from_array(this.References, MBCO_association_readOptions_class.Array_delimiter); }
            set { this.References = ReadWriteClass.Get_array_from_readLine<string>(value, MBCO_association_readOptions_class.Array_delimiter); }
        }
        #endregion

        public MBCO_association_line_class()
        {
            ProcessLevel = -1;
            ProcessID = Global_class.Empty_entry;
            ProcessName = Global_class.Empty_entry;
            Parent_processName = Global_class.Empty_entry;
            Description = "";
            References = new string[0];
        }

        public MBCO_association_line_class Deep_copy()
        {
            MBCO_association_line_class copy = (MBCO_association_line_class)this.MemberwiseClone();
            copy.ProcessID = (string)this.ProcessID.Clone();
            copy.ProcessName = (string)this.ProcessName.Clone();
            copy.Parent_processName = (string)this.Parent_processName.Clone();
            copy.Symbol = (string)this.Symbol.Clone();
            copy.Description = (string)this.Description.Clone();
            copy.References = Array_class.Deep_copy_string_array(this.References);
            return copy;
        }
    }

    class MBCO_association_readOptions_class : ReadWriteOptions_base
    {
        public static char Array_delimiter { get { return ';'; } }

        public MBCO_association_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Get_complete_fileName_of_gene_association_parentsPopulatedWithChildGenes(ontology);
            Key_propertyNames = new string[] { "ProcessLevel", "Parent_processName", "ProcessID", "ProcessName", "Symbol" };
            Key_columnNames = Key_propertyNames;
            this.File_has_headline = true;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_association_minimum_readOptions_class : ReadWriteOptions_base
    {
        public static char Array_delimiter { get { return ';'; } }

        public MBCO_association_minimum_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Get_complete_fileName_of_minimum_gene_association_parentsPopulatedWithChildGenes(ontology);
            Key_propertyNames = new string[] { "ProcessName", "Symbol" };
            Key_columnNames = Key_propertyNames;
            this.File_has_headline = true;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_association_goDownloaded_readOptions_class : ReadWriteOptions_base
    {
        public static char Array_delimiter { get { return ';'; } }

        public MBCO_association_goDownloaded_readOptions_class(Ontology_type_enum ontology)
        {
            if (!Ontology_classification_class.Is_go_ontology(ontology)) { throw new Exception(); }
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Complete_human_go_association_2022_downloaded_fileName;
            Key_propertyNames = new string[] { "Symbol", "ProcessID" };
            Key_columnIndexes = new int[] { 2, 4 };
            this.File_has_headline = false;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_association_class
    {
        public MBCO_association_line_class[] MBCO_associations { get; set; }
        public Ontology_type_enum Ontology { get; private set; }

        public MBCO_association_class()
        {
            MBCO_associations = new MBCO_association_line_class[0];
        }

        public bool Check_for_dupplicated_scp_symbol_associations()
        {
            int mbco_length = this.MBCO_associations.Length;
            this.Order_by_processName_symbol();
            MBCO_association_line_class mbco_association_line;
            for (int indexMBCO = 0; indexMBCO < mbco_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if ((indexMBCO != 0)
                    && (mbco_association_line.ProcessName.Equals(this.MBCO_associations[indexMBCO - 1].ProcessName))
                    && (mbco_association_line.Symbol.Equals(this.MBCO_associations[indexMBCO - 1].Symbol)))
                {
                    throw new Exception();
                }
            }
            return true;
        }
        public void Add_to_array(MBCO_association_line_class[] add_MBCO_associations)
        {
            int this_length = this.MBCO_associations.Length;
            int add_length = add_MBCO_associations.Length;
            int new_length = this_length + add_length;
            int indexNew = -1;
            MBCO_association_line_class[] new_MBCO_associations = new MBCO_association_line_class[new_length];
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_MBCO_associations[indexNew] = this.MBCO_associations[indexThis];
            }
            for (int indexAdd= 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_MBCO_associations[indexNew] = add_MBCO_associations[indexAdd];
            }
            this.MBCO_associations = new_MBCO_associations;
        }
        public void Remove_dupplicated_scp_symbol_associations()
        {
            int mbco_length = this.MBCO_associations.Length;
            this.Order_by_processName_symbol();
            MBCO_association_line_class mbco_association_line;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            for (int indexMBCO = 0; indexMBCO < mbco_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if ((indexMBCO == 0)
                    || (!mbco_association_line.ProcessName.Equals(this.MBCO_associations[indexMBCO - 1].ProcessName))
                    || (!mbco_association_line.Symbol.Equals(this.MBCO_associations[indexMBCO - 1].Symbol)))
                {
                    keep.Add(mbco_association_line);
                }
                else if (!mbco_association_line.ProcessID.Equals(this.MBCO_associations[indexMBCO-1].ProcessID))
                {
                    throw new Exception();
                }
            }
            this.MBCO_associations = keep.ToArray();
        }
        private void Add_missing_processNames_from_parent_child_nw_or_check_if_equal(MBCO_obo_network_class mbco_parentChild_nw)
        {
            Dictionary<string, string> processID_processName_dict = mbco_parentChild_nw.Get_processID_processName_dictionary();
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                if ((String.IsNullOrEmpty(mbco_association_line.ProcessName))
                    || (mbco_association_line.ProcessName.Equals(Global_class.Empty_entry)))
                {
                    mbco_association_line.ProcessName = (string)processID_processName_dict[mbco_association_line.ProcessID].Clone();
                }
                else if (mbco_association_line.ProcessName.Equals(Global_class.Background_genes_scpName)) { }
                else if (!mbco_association_line.ProcessName.Equals(processID_processName_dict[mbco_association_line.ProcessID]))
                {
                    throw new Exception();
                }
            }
        }
        private void Set_level_for_nonMBCO_ontologies()
        {
            if (Ontology_classification_class.Is_mbco_ontology(this.Ontology)) { throw new Exception(); }
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                mbco_association_line.ProcessLevel = Global_class.ProcessLevel_for_all_non_MBCO_SCPs;
            }
        }
        private void Keep_only_scps_of_selected_go_namespace(MBCO_obo_network_class mbco_parentChild_nw)
        {
            Namespace_type_enum keep_namespace = Namespace_type_enum.E_m_p_t_y;
            switch (this.Ontology)
            {
                case Ontology_type_enum.Go_bp_human:
                    keep_namespace = Namespace_type_enum.biological_process;
                    break;
                case Ontology_type_enum.Go_mf_human:
                    keep_namespace = Namespace_type_enum.molecular_function;
                    break;
                case Ontology_type_enum.Go_cc_human:
                    keep_namespace = Namespace_type_enum.cellular_component;
                    break;
                default:
                    throw new Exception();
            }
            Dictionary<string,Namespace_type_enum> scp_namespace_dict = mbco_parentChild_nw.Get_processName_namespace_dictionary();

            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class association_line in this.MBCO_associations)
            {
                if (scp_namespace_dict[association_line.ProcessName].Equals(keep_namespace))
                {
                    keep.Add(association_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }
        private void Populate_parent_scps_with_genes_of_children_scps(MBCO_obo_network_class mbco_parent_child_nw)
        {
            Dictionary<string, string[]> parents_child_dict = mbco_parent_child_nw.Get_sourceNodeName_targetNodeNames_dict();
            Dictionary<string, string> scp_scpId_dict = mbco_parent_child_nw.Get_processName_processId_dictionary();
            Dictionary<string, int> parents_populatedByNumberOfChildren = new Dictionary<string, int>();
            string[] child_scps = mbco_parent_child_nw.Get_all_finalChildren_leaves_if_parent_child();
            string child_scp;
            int child_scps_length = child_scps.Length;
            MBCO_obo_network_class mbco_child_parent_nw = mbco_parent_child_nw.Deep_copy_mbco_obo_nw();
            mbco_child_parent_nw.Transform_into_child_parent_direction();
            Dictionary<string, string[]> child_parents_dict = mbco_child_parent_nw.Get_sourceNodeName_targetNodeNames_dict();
            Dictionary<string, Dictionary<string,bool>> scp_genes_dict = this.Get_scp_targetGene_dictionary();
            string[] parentScps;
            string parentScp;
            int parentScps_length;
            string[] childScpGenes;
            string childScpGene;
            int childScpGenes_length;
            List<string> nextChildren = new List<string>();
            while (child_scps_length > 0)
            {
                nextChildren.Clear();
                for (int indexChild=0; indexChild<child_scps_length; indexChild++)
                {
                    child_scp = child_scps[indexChild];

                    if (child_parents_dict.ContainsKey(child_scp))
                    {
                        parentScps = child_parents_dict[child_scp];
                        parentScps_length = parentScps.Length;
                        for (int indexParent = 0; indexParent < parentScps_length; indexParent++)
                        {
                            parentScp = parentScps[indexParent];
                            if (!parents_populatedByNumberOfChildren.ContainsKey(parentScp))
                            { parents_populatedByNumberOfChildren.Add(parentScp, 0); }
                            if (scp_genes_dict.ContainsKey(child_scp))
                            {
                                childScpGenes = scp_genes_dict[child_scp].Keys.ToArray();
                                childScpGenes_length = childScpGenes.Length;
                                if (!scp_genes_dict.ContainsKey(parentScp)) { scp_genes_dict.Add(parentScp, new Dictionary<string, bool>()); }
                                for (int indexChildScpGenes = 0; indexChildScpGenes < childScpGenes_length; indexChildScpGenes++)
                                {
                                    childScpGene = childScpGenes[indexChildScpGenes];
                                    if (!scp_genes_dict[parentScp].ContainsKey(childScpGene))
                                    {
                                        scp_genes_dict[parentScp].Add(childScpGene, true);
                                    }
                                }
                            }
                            parents_populatedByNumberOfChildren[parentScp]++;
                            if (parents_child_dict[parentScp].Length != parents_child_dict[parentScp].Distinct().ToArray().Length) { throw new Exception(); }
                            if (parents_populatedByNumberOfChildren[parentScp] == parents_child_dict[parentScp].Length)
                            {
                                nextChildren.Add(parentScp);
                            }
                        }
                    }
                }
                child_scps = nextChildren.ToArray();
                child_scps_length = child_scps.Length;
            }
            this.Order_by_processName_symbol();
            int mbco_association_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMBCO=0; indexMBCO<mbco_association_length;indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                scp_genes_dict[mbco_association_line.ProcessName][mbco_association_line.Symbol] = false;
            }
            string[] scps = scp_genes_dict.Keys.ToArray();
            string scp;
            int scps_length = scps.Length;
            Dictionary<string, bool> scpGenes_dict;
            string[] scp_genes;
            string scp_gene;
            int scps_gene_length;
            MBCO_association_line_class new_mbco_association_line;
            List<MBCO_association_line_class> add_mbco_association_lines = new List<MBCO_association_line_class>();

            for (int indexScp=0; indexScp<scps_length;indexScp++)
            {
                scp = scps[indexScp];

                scpGenes_dict = scp_genes_dict[scp];
                scp_genes = scpGenes_dict.Keys.OrderBy(l=>l).ToArray();
                scps_gene_length = scp_genes.Length;
                for (int indexScpGene=0; indexScpGene<scps_gene_length;indexScpGene++)
                {
                    scp_gene = scp_genes[indexScpGene];
                    if (scpGenes_dict[scp_gene].Equals(true))
                    {
                        new_mbco_association_line = new MBCO_association_line_class();
                        new_mbco_association_line.Symbol = (string)scp_gene.Clone();
                        new_mbco_association_line.ProcessName = (string)scp.Clone();
                        new_mbco_association_line.ProcessID = scp_scpId_dict[new_mbco_association_line.ProcessName];
                        add_mbco_association_lines.Add(new_mbco_association_line);
                    }
                }
            }
            this.Add_to_array(add_mbco_association_lines.ToArray());
            Check_for_dupplicated_scp_symbol_associations();
        }
        private void Test_population_of_parent_scps_with_children_genes(MBCO_obo_network_class mbco_parent_child_nw_input)
        {
            MBCO_obo_network_class mbco_parent_child_nw = mbco_parent_child_nw_input.Deep_copy_mbco_obo_nw();
            //mbco_parent_child_nw.Keep_only_scps_of_selected_namespace_if_gene_ontology();

            Dictionary<string, Dictionary<string, bool>> scp_scpGenes_dict = this.Get_scp_targetGene_dictionary();
            string[] scps = scp_scpGenes_dict.Keys.ToArray();
            string[] child_scps;
            string child_scp;
            int child_scps_length;
            string parent_scp;
            string[] childScp_genes;
            int scps_length = scps.Length;
            Random random = new Random();
            int indexScp;

            for (int indexTest=5268; indexTest<scps.Length; indexTest++)
            {
                //indexScp = random.Next(scps_length);
                indexScp = indexTest;
                parent_scp = scps[indexScp];
                child_scps = mbco_parent_child_nw.Get_all_children_if_direction_is_parent_child(parent_scp);
                child_scps_length = child_scps.Length;
                for (int indexChildScp=0; indexChildScp<child_scps_length;indexChildScp++)
                {
                    child_scp = child_scps[indexChildScp];
                    if (scp_scpGenes_dict.ContainsKey(child_scp))
                    {
                        childScp_genes = scp_scpGenes_dict[child_scp].Keys.ToArray();
                        foreach (string childScp_gene in childScp_genes)
                        {
                            if (!scp_scpGenes_dict[parent_scp].ContainsKey(childScp_gene)) { throw new Exception(); }
                        }
                    }
                }
            }
        }

        private void Keep_only_scps_with_minimum_and_maximum_genes_count()
        {
            int max_genes_each_scp = 250;
            int min_genes_each_scp = 5;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            List<MBCO_association_line_class> current_scp = new List<MBCO_association_line_class>();
            int mbco_associations_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMBCO=0;indexMBCO<mbco_associations_length;indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (   (indexMBCO==0)
                    || (!mbco_association_line.ProcessName.Equals(this.MBCO_associations[indexMBCO-1].ProcessName)))
                {
                    current_scp.Clear();
                }
                current_scp.Add(mbco_association_line);
                if ((indexMBCO == mbco_associations_length-1)
                    || (!mbco_association_line.ProcessName.Equals(this.MBCO_associations[indexMBCO + 1].ProcessName)))
                {
                    if ((current_scp.Count>=min_genes_each_scp)&&(current_scp.Count<=max_genes_each_scp))
                    {
                        keep.AddRange(current_scp);
                    }
                }
            }
            this.MBCO_associations = keep.ToArray();
        }

        private void Set_all_genesToUpperCase_and_remove_empty_geneSymbols()
        {
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class mbco_line in this.MBCO_associations)
            {
                if (!String.IsNullOrEmpty(mbco_line.Symbol))
                {
                    mbco_line.Symbol = mbco_line.Symbol.ToUpper();
                    keep.Add(mbco_line);
                }
                else
                {
                    //in these lines only protein ids exist;
                }
            }
            this.MBCO_associations = keep.ToArray();
        }
        public void Generate_after_reading_safed_file(Ontology_type_enum ontology, System.Windows.Forms.Label errorReport_label, Form1_default_settings_class form_default_settings)
        {
            this.Ontology = ontology;
            switch (Ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    Read_mbco_associations_with_parentsPopulatedByChildrenGenes(errorReport_label, form_default_settings);
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    Read_mbco_associations_with_parentsPopulatedByChildrenGenes(errorReport_label, form_default_settings);
                    Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                    Set_level_for_nonMBCO_ontologies();
                    break;
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Go_mf_human:
                    Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                    if (File.Exists(gdf.Get_complete_fileName_of_gene_association_parentsPopulatedWithChildGenes(ontology)))
                    {
                        Read_mbco_associations_with_parentsPopulatedByChildrenGenes(errorReport_label, form_default_settings);
                    }
                    else
                    {
                        Read_go_associations_downloaded(errorReport_label, form_default_settings);
                        Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                        MBCO_obo_network_class mbco_parentChild_nw = new MBCO_obo_network_class(this.Ontology);
                        mbco_parentChild_nw.Generate_by_reading_safed_obo_file(errorReport_label, form_default_settings);
                        Add_missing_processNames_from_parent_child_nw_or_check_if_equal(mbco_parentChild_nw);
                        Remove_dupplicated_scp_symbol_associations();
                        Populate_parent_scps_with_genes_of_children_scps(mbco_parentChild_nw);
                        Remove_dupplicated_scp_symbol_associations();
                        Keep_only_scps_with_minimum_and_maximum_genes_count();
                        //Test_population_of_parent_scps_with_children_genes(mbco_parentChild_nw);
                        Keep_only_scps_of_selected_go_namespace(mbco_parentChild_nw);
                        Set_level_for_nonMBCO_ontologies();
                        Write_mbco_go_associations_with_parentsPopulatedByChildrenGenes(errorReport_label, form_default_settings);
                    }
                    break;
                default:
                    throw new Exception();
            }
        }

        public void Read_without_any_modifications(Ontology_type_enum ontology, System.Windows.Forms.Label errorReport_label, Form1_default_settings_class form_default_settings)
        {
            this.Ontology = ontology;
            switch (Ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    Read_mbco_associations_with_parentsPopulatedByChildrenGenes(errorReport_label, form_default_settings);
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    Read_mbco_associations_with_parentsPopulatedByChildrenGenes(errorReport_label, form_default_settings);
                    break;
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Go_mf_human:
                    Read_go_associations_downloaded(errorReport_label, form_default_settings);
                    break;
                default:
                    throw new Exception();
            }
        }

        #region Order
        public void Order_by_symbol_processName()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol).ThenBy(l => l.ProcessName).ToArray();
        }

        public void Order_by_symbol_descending_processLevel_processName()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol).ThenByDescending(l=>l.ProcessLevel).ThenBy(l => l.ProcessName).ToArray();
        }

        public void Order_by_lengthOfSymbol_symbol()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol.Length).ThenBy(l => l.Symbol).ToArray();
        }

        public void Order_by_process_id()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.ProcessID).ToArray();
        }

        public void Order_by_processName_symbol()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.ProcessName).ThenBy(l => l.Symbol).ToArray();
        }

        public void Order_by_level_processName_symbol_and_add_background_genes_to_the_end()
        {
            MBCO_associations = MBCO_associations.OrderBy(l=>l.ProcessLevel).ThenBy(l => l.ProcessName).ThenBy(l => l.Symbol).ToArray();
            List<MBCO_association_line_class> background = new List<MBCO_association_line_class>();
            List<MBCO_association_line_class> regular = new List<MBCO_association_line_class>();
            int mbco_associations_length = MBCO_associations.Length;
            MBCO_association_line_class mbco_line;
            for (int indexMBCO=0;indexMBCO<mbco_associations_length;indexMBCO++)
            {
                mbco_line = MBCO_associations[indexMBCO];
                if (mbco_line.ProcessName.Equals("Background genes"))
                {
                    background.Add(mbco_line);
                }
                else
                {
                    regular.Add(mbco_line);
                }
            }
            regular.AddRange(background);
            this.MBCO_associations = regular.ToArray();
        }

        public void Order_by_processLevel()
        {
            this.MBCO_associations = this.MBCO_associations.OrderBy(l => l.ProcessLevel).ToArray();
        }
        #endregion

        #region Get
        public int[] Get_all_levels()
        {
            List<int> all_levels = new List<int>();
            MBCO_association_line_class onto_line;
            int onto_length = MBCO_associations.Length;
            this.Order_by_processLevel();
            for (int indexOnto = 0; indexOnto < onto_length; indexOnto++)
            {
                onto_line = MBCO_associations[indexOnto];
                if ((indexOnto == 0)
                    || (!onto_line.ProcessLevel.Equals(MBCO_associations[indexOnto - 1].ProcessLevel)))
                {
                    all_levels.Add(onto_line.ProcessLevel);
                }
            }
            return all_levels.ToArray();
        }

        public string[] Get_all_distinct_ordered_symbols()
        {
            Dictionary<string, bool> symbols_dict = new Dictionary<string, bool>();
            int onto_length = MBCO_associations.Length;
            MBCO_association_line_class onto_line;
            for (int indexOnto = 0; indexOnto < onto_length; indexOnto++)
            {
                onto_line = MBCO_associations[indexOnto];
                if (!symbols_dict.ContainsKey(onto_line.Symbol))
                {
                    symbols_dict.Add(onto_line.Symbol,true);
                }
            }
            return symbols_dict.Keys.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_distinct_ordered_scps()
        {
            this.Order_by_processName_symbol();
            int onto_length = MBCO_associations.Length;
            MBCO_association_line_class onto_line;
            List<string> all_distinct_ordered_scps = new List<string>();
            for (int indexOnto = 0; indexOnto < onto_length; indexOnto++)
            {
                onto_line = MBCO_associations[indexOnto];
                if ((indexOnto == 0)
                    || (!onto_line.ProcessName.Equals(MBCO_associations[indexOnto - 1].ProcessName)))
                {
                    all_distinct_ordered_scps.Add(onto_line.ProcessName);
                }
            }
            return all_distinct_ordered_scps.OrderBy(l => l).ToArray();
        }

        public Dictionary<string, int> Get_scp_level_dictionary()
        {
            Dictionary<string, int> scp_level_dict = new Dictionary<string, int>();
            int mbco_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMbco = 0; indexMbco < mbco_length; indexMbco++)
            {
                mbco_association_line = this.MBCO_associations[indexMbco];
                if (!scp_level_dict.ContainsKey(mbco_association_line.ProcessName)) { scp_level_dict.Add(mbco_association_line.ProcessName, mbco_association_line.ProcessLevel); }
            }
            return scp_level_dict;
        }

        public Dictionary<string,Dictionary<string,bool>> Get_scp_targetGene_dictionary()
        {
            Dictionary<string, Dictionary<string, bool>> scp_targetGene_dict = new Dictionary<string, Dictionary<string, bool>>();
            int mbco_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMbco=0; indexMbco<mbco_length; indexMbco++)
            {
                mbco_association_line = this.MBCO_associations[indexMbco];
                if (!scp_targetGene_dict.ContainsKey(mbco_association_line.ProcessName)) { scp_targetGene_dict.Add(mbco_association_line.ProcessName, new Dictionary<string, bool>()); }
                scp_targetGene_dict[mbco_association_line.ProcessName].Add(mbco_association_line.Symbol, true);
            }
            return scp_targetGene_dict;
        }

        public string[] Get_all_symbols_of_process_names(params string[] process_names)
        {
            process_names = process_names.Distinct().OrderBy(l => l).ToArray();
            int process_names_length = process_names.Length;
            string process_name;

            int onto_length = MBCO_associations.Length;
            this.MBCO_associations = this.MBCO_associations.OrderBy(l => l.ProcessName).ToArray();
            MBCO_association_line_class onto_association_line;
            List<string> process_symbols_list = new List<string>();
            int indexOnto = 0;
            int stringCompare = -2;

            bool process_name_exists = false;
            for (int indexProcessName = 0; indexProcessName < process_names_length; indexProcessName++)
            {
                process_name = process_names[indexProcessName];
                process_name_exists = false;
                stringCompare = -2;
                while ((indexOnto < onto_length) && (stringCompare <= 0))
                {
                    onto_association_line = MBCO_associations[indexOnto];
                    stringCompare = onto_association_line.ProcessName.CompareTo(process_name);
                    if (stringCompare < 0)
                    {
                        indexOnto++;
                    }
                    else if (stringCompare == 0)
                    {
                        process_symbols_list.Add(onto_association_line.Symbol);
                        indexOnto++;
                        process_name_exists = true;
                    }
                }
                if (!process_name_exists) { throw new Exception("process name does not exist"); }
            }
            return process_symbols_list.ToArray();
        }
        #endregion

        #region Keep, Remove
        public void Keep_only_bg_symbols(string[] bg_symbols)
        {
            bg_symbols = bg_symbols.Distinct().OrderBy(l => l).ToArray();
            string bg_symbol;
            int bg_symbols_length = bg_symbols.Length;
            int indexSymbol = 0;

            this.Order_by_symbol_processName();
            int mbco_associations_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            int stringCompare = -2;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            for (int indexMBCO = 0; indexMBCO < mbco_associations_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                stringCompare = -2;
                while ((indexSymbol < bg_symbols_length) && (stringCompare < 0))
                {
                    bg_symbol = bg_symbols[indexSymbol];
                    stringCompare = bg_symbol.CompareTo(mbco_association_line.Symbol);
                    if (stringCompare < 0)
                    {
                        indexSymbol++;
                    }
                    else if (stringCompare == 0)
                    {
                        keep.Add(mbco_association_line);
                    }
                }
            }
            this.MBCO_associations = keep.ToArray();
        }

        public void Keep_only_indicated_scps(string[] keep_scps)
        {
            keep_scps = keep_scps.Distinct().OrderBy(l => l).ToArray();
            int keep_scps_length = keep_scps.Length;
            Dictionary<string, bool> keep_scps_dict = new Dictionary<string, bool>();
            for (int indexKeep =0; indexKeep< keep_scps_length; indexKeep++)
            {
                keep_scps_dict.Add(keep_scps[indexKeep], true);
            }
            int mbco_associations_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            for (int indexMBCO=0; indexMBCO<mbco_associations_length;indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (keep_scps_dict.ContainsKey(mbco_association_line.ProcessName))
                {
                    keep.Add(mbco_association_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }

        public void Remove_indicated_symbols(string[] remove_symbols)
        {
            remove_symbols = remove_symbols.Distinct().ToArray();
            Dictionary<string, bool> remove_symbols_dict = new Dictionary<string, bool>();
            foreach (string remove_symbol in remove_symbols)
            {
                remove_symbols_dict.Add(remove_symbol, true);
            }

            int mbco_associations_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            for (int indexMBCO = 0; indexMBCO < mbco_associations_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (!remove_symbols_dict.ContainsKey(mbco_association_line.Symbol))
                {
                    keep.Add(mbco_association_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }


        public void Keep_only_lines_with_indicated_levels(params int[] levels)
        {
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                if (levels.Contains(mbco_association_line.ProcessLevel))
                {
                    keep.Add(mbco_association_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }

        public void Remove_background_genes_scp()
        {
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                if (!mbco_association_line.ProcessName.Equals(Global_class.Background_genes_scpName))
                {
                    keep.Add(mbco_association_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }
        #endregion

        #region Custom SCPs
        public void Remove_all_custom_SCPs_from_mbco_association_unmodified()
        {
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class mbco_line in this.MBCO_associations)
            {
                if (!mbco_line.ProcessID.Equals(Global_class.CustomScp_id))
                {
                    keep.Add(mbco_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }

        public void Add_custom_scps_as_combination_of_selected_scps_to_mbco_association_unmodified(Dictionary<string, string[]> customSCP_mbcoScps_dict, Dictionary<string,int> customSCP_level_dict)
        {
            Dictionary<string, string[]> mbcoScps_custom_dict = Dictionary_class.Reverse_dictionary(customSCP_mbcoScps_dict);
            Dictionary<string, bool> mbcoScp_found_dict = new Dictionary<string, bool>();
            List<MBCO_association_line_class> new_association_lines = new List<MBCO_association_line_class>();
            MBCO_association_line_class new_mbco_association_line;
            MBCO_association_line_class mbco_association_line;
            int mbco_length = this.MBCO_associations.Length;
            string[] new_scps;
            string new_scp;
            int new_scps_length;
            Dictionary<string, Dictionary<string, bool>> newSCP_gene_exists_dict = new Dictionary<string, Dictionary<string, bool>>();
            for (int indexMBCO=0; indexMBCO<mbco_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (mbcoScps_custom_dict.ContainsKey(mbco_association_line.ProcessName))
                {
                    if (!mbcoScp_found_dict.ContainsKey(mbco_association_line.ProcessName)) { mbcoScp_found_dict.Add(mbco_association_line.ProcessName, true); }
                    new_scps = mbcoScps_custom_dict[mbco_association_line.ProcessName];
                    new_scps_length = new_scps.Length;
                    for (int indexNew=0; indexNew<new_scps_length;indexNew++)
                    {
                        new_scp = new_scps[indexNew];
                        if (!newSCP_gene_exists_dict.ContainsKey(new_scp))
                        {
                            newSCP_gene_exists_dict.Add(new_scp, new Dictionary<string, bool>());
                        }
                        if (!newSCP_gene_exists_dict[new_scp].ContainsKey(mbco_association_line.Symbol))
                        {
                            new_mbco_association_line = mbco_association_line.Deep_copy();
                            new_mbco_association_line.References = new string[] { Global_class.CustomScp_id };
                            new_mbco_association_line.ProcessID = Global_class.CustomScp_id;
                            new_mbco_association_line.ProcessLevel = customSCP_level_dict[new_scp];
                            new_mbco_association_line.Manual_validation = Manual_validation_enum.Custom_gene_scp_association;
                            new_mbco_association_line.Parent_processName = Global_class.CustomScp_id;
                            new_mbco_association_line.ProcessName = (string)new_scp.Clone();
                            new_association_lines.Add(new_mbco_association_line);
                            newSCP_gene_exists_dict[new_scp].Add(mbco_association_line.Symbol, true);
                        }
                    }
                }
            }
            //if (mbcoScp_found_dict.Keys.ToArray().Length!=mbcoScps_custom_dict.Keys.ToArray().Length) { throw new Exception(); }
            new_association_lines.AddRange(this.MBCO_associations);
            this.MBCO_associations = new_association_lines.ToArray();
            Check_for_dupplicated_scp_symbol_associations();
        }

        #endregion

        #region Read write copy
        private void Read_mbco_associations_with_parentsPopulatedByChildrenGenes(System.Windows.Forms.Label errorReport_label, Form1_default_settings_class form_default_settings)
        {
            MBCO_association_readOptions_class readOptions = new MBCO_association_readOptions_class(this.Ontology);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_if_error_message<MBCO_association_line_class>(readOptions, errorReport_label, form_default_settings);
        }
        private void Write_mbco_go_associations_with_parentsPopulatedByChildrenGenes(System.Windows.Forms.Label errorReport_label, Form1_default_settings_class form_default_settings)
        {
            if (!Ontology_classification_class.Is_go_ontology(this.Ontology)) { throw new Exception(); }
            MBCO_association_readOptions_class readOptions = new MBCO_association_readOptions_class(this.Ontology);
            ReadWriteClass.WriteData(this.MBCO_associations, readOptions, errorReport_label, form_default_settings);
        }
        private void Read_go_associations_downloaded(System.Windows.Forms.Label errorReport_label, Form1_default_settings_class form_default_settings)
        {
            MBCO_association_goDownloaded_readOptions_class readOptions = new MBCO_association_goDownloaded_readOptions_class(this.Ontology);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_if_error_message<MBCO_association_line_class>(readOptions, errorReport_label, form_default_settings);
        }
        public void Read_minimum_mbco_association(Ontology_type_enum ontology)
        {
            MBCO_association_minimum_readOptions_class readOptions = new MBCO_association_minimum_readOptions_class(ontology);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_if_error_message<MBCO_association_line_class>(readOptions, new System.Windows.Forms.Label(), new Form1_default_settings_class());
        }
        public void Write_mbco_association(Ontology_type_enum ontology)
        {
            MBCO_association_readOptions_class readOptions = new MBCO_association_readOptions_class(ontology);
            ReadWriteClass.WriteData(this.MBCO_associations ,readOptions, new System.Windows.Forms.Label(), new Form1_default_settings_class());
        }
        public MBCO_association_class Deep_copy()
        {
            MBCO_association_class copy = (MBCO_association_class)this.MemberwiseClone();
            int associations_length = MBCO_associations.Length;
            copy.MBCO_associations = new MBCO_association_line_class[associations_length];
            for (int indexA = 0; indexA < associations_length; indexA++)
            {
                copy.MBCO_associations[indexA] = this.MBCO_associations[indexA].Deep_copy();
            }
            return copy;
        }
        #endregion

    }
}

