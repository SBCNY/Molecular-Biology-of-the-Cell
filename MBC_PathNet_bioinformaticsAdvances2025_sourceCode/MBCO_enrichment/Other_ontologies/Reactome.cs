using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using Enrichment;
using Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows_forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Other_ontologies_and_databases
{
    class Reactome_pathway_annotation_line_class
    {
        public string Reactome_id { get; set; }
        public string Reactome_name { get; set; }
        public string Organism_string { get; set; }
        public Organism_enum Organism { get; set; }

        public Reactome_pathway_annotation_line_class Deep_copy()
        {
            Reactome_pathway_annotation_line_class copy = (Reactome_pathway_annotation_line_class)this.MemberwiseClone();
            copy.Reactome_name = (string)this.Reactome_name.Clone();
            copy.Reactome_id = (string)this.Reactome_id.Clone();
            copy.Organism_string = (string)this.Organism_string.Clone();
            return copy;
        }
    }

    class Reactome_download_pathway_annotation_readOptions_class : ReadWriteOptions_base
    {
        public Reactome_download_pathway_annotation_readOptions_class()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Ontology_inputDirectory_dict[Ontology_type_enum.Reactome] + gdf.Ontology_pathwayAnnotation_dict[Ontology_type_enum.Reactome];
            this.Key_propertyNames = new string[] { "Reactome_id", "Reactome_name", "Organism_string" };
            this.Key_columnIndexes = new int[] { 0, 1, 2 };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = false;
        }
    }

    class Reactome_pathway_annotation_class
    {
        public Reactome_pathway_annotation_line_class[] Pathway_annotations { get; set; }

        public void Generate_by_reading(ProgressReport_interface_class progressReport)
        {
            Read(progressReport);
        }
        public void Keep_only_pathways_with_selected_organism(Organism_enum organism)
        {
            List<Reactome_pathway_annotation_line_class> keep = new List<Reactome_pathway_annotation_line_class>();
            foreach (Reactome_pathway_annotation_line_class reactome_line in this.Pathway_annotations)
            {
                if (reactome_line.Organism_string.Equals(organism.ToString().Replace("_"," ")))
                {
                    keep.Add(reactome_line);
                }
                else if (  (organism.Equals(Organism_enum.Canis_lupus_familiaris))
                         &&(reactome_line.Organism_string.Equals("Canis familiaris")))
                {
                    keep.Add(reactome_line);
                }
            }
            if (keep.Count==0) { throw new Exception(); }
            this.Pathway_annotations = keep.ToArray();
        }
        public Dictionary<string,string> Get_pathwayID_pathwayName_dict()
        {
            Dictionary<string, string> pathwayID_pathwayName_dict = new Dictionary<string, string>();
            foreach (Reactome_pathway_annotation_line_class annotation_line in this.Pathway_annotations)
            {
                if (!pathwayID_pathwayName_dict.ContainsKey(annotation_line.Reactome_id))
                {
                    pathwayID_pathwayName_dict.Add(annotation_line.Reactome_id, annotation_line.Reactome_name);
                }
                else if (!pathwayID_pathwayName_dict[annotation_line.Reactome_id].Equals(annotation_line.Reactome_name))
                {
                    throw new Exception();
                }
            }
            return pathwayID_pathwayName_dict;
        }

        private void Read(ProgressReport_interface_class progressReport)
        {
            Reactome_download_pathway_annotation_readOptions_class readWriteOptions = new Reactome_download_pathway_annotation_readOptions_class();
            string shared_error_response = Ontology_classification_class.Get_pleaseDonwload_file_again_message(readWriteOptions.File);
            this.Pathway_annotations = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Reactome_pathway_annotation_line_class>(readWriteOptions, progressReport, shared_error_response);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    class Reactome_library_line_class
    {
        public string Entity_id { get; set; }
        public string Entity { get; set; }
        public string Reactome_pathway_id { get; set; }
        public string Pathway { get; set; }
        public string Validation_string { get; set; }
        public string Organism_string { get; set; }
        public Organism_enum Pathway_organism { get; set; }
        public Organism_enum Gene_organism { get; set; }

        public Reactome_library_line_class()
        { }
    }

    class Reactome_library_readWriteOptions : ReadWriteOptions_base
    {
        public Reactome_library_readWriteOptions(Ontology_type_enum ontology, Organism_enum organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Ontology_inputDirectory_dict[Ontology_type_enum.Reactome] + gdf.Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Reactome][organism];
            this.Key_propertyNames = new string[] { "Entity_id", "Reactome_pathway_id", "Pathway", "Validation_string", "Organism_string" };
            this.Key_columnIndexes = new int[] { 0, 1, 3, 4, 5 };//2 is missing
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
            this.File_has_headline = false;
        }
    }

    class Reactome_library_class
    {
        public Reactome_library_line_class[] Reactome { get; set; }

        public Reactome_library_class()
        {
            this.Reactome = new Reactome_library_line_class[0];
        }

        private void Add_entity_for_chembio()
        {
            foreach (Reactome_library_line_class reactome_library_line in this.Reactome)
            {
                reactome_library_line.Entity = "CHEBI:" + reactome_library_line.Entity_id;
            }
        }

        private void Remove_duplicated_scpID_scp_gene_associations()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, bool>>> scpID_scp_gene_dict = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
            List<Reactome_library_line_class> keep = new List<Reactome_library_line_class>();
            List<Reactome_library_line_class> remove = new List<Reactome_library_line_class>();
            foreach (Reactome_library_line_class library_line in this.Reactome)
            {
                if (!scpID_scp_gene_dict.ContainsKey(library_line.Reactome_pathway_id))
                {
                    scpID_scp_gene_dict.Add(library_line.Reactome_pathway_id, new Dictionary<string, Dictionary<string, bool>>());
                }
                if (!scpID_scp_gene_dict[library_line.Reactome_pathway_id].ContainsKey(library_line.Pathway))
                {
                    scpID_scp_gene_dict[library_line.Reactome_pathway_id].Add(library_line.Pathway, new Dictionary<string, bool>());
                }
                if (!scpID_scp_gene_dict[library_line.Reactome_pathway_id][library_line.Pathway].ContainsKey(library_line.Entity))
                {
                    scpID_scp_gene_dict[library_line.Reactome_pathway_id][library_line.Pathway].Add(library_line.Entity, true);
                    keep.Add(library_line);
                }
                else if (Global_class.Do_internal_checks)
                {
                    remove.Add(library_line);
                }
            }
            this.Reactome = keep.ToArray();
        }

        private void Add_entity_and_gene_organism_for_ncbi(Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            GeneInfo_class geneInfo = new GeneInfo_class(new Organism_enum[] { organism });
            geneInfo.Generate_deNovo_and_save_or_read_appGenerated_geneInfo(progressReport);
            if (geneInfo.Data.Length==0) { throw new Exception(); }
            Dictionary<int, string> geneId_geneSymbol_dict = geneInfo.Get_geneID_ncbiGeneSymbol_dict();
            Dictionary<int, Organism_enum> geneId_organism_dict = geneInfo.Get_geneID_organism_dict();
            int entity_id_number;
            foreach (Reactome_library_line_class reactome_line in this.Reactome)
            {
                if (int.TryParse(reactome_line.Entity_id, out entity_id_number))
                {
                    if (geneId_geneSymbol_dict.ContainsKey(entity_id_number))
                    {
                        reactome_line.Entity = (string)geneId_geneSymbol_dict[entity_id_number].Clone();
                        reactome_line.Gene_organism = geneId_organism_dict[entity_id_number];
                    }
                }
                else
                {
                    //throw new Exception();
                }
            }
        }

        private void Keep_only_indicated_organism_strings(params string[] keep_organism_strings)
        {
            Dictionary<string, bool> keep_organism_dict = new Dictionary<string, bool>();
            foreach (string keep_organism_string in keep_organism_strings)
            {
                keep_organism_dict.Add(keep_organism_string, true);
            }
            List<Reactome_library_line_class> keep = new List<Reactome_library_line_class>();
            foreach (Reactome_library_line_class reactome_line in this.Reactome)
            {
                if (keep_organism_dict.ContainsKey(reactome_line.Organism_string))
                {
                    keep.Add(reactome_line);
                }
            }
            this.Reactome = keep.ToArray();
        }

        private void Set_pathway_organism_from_organism_string()
        {
            Dictionary<string,Organism_enum> organismString_organismEnum_dict = new Dictionary<string,Organism_enum>();
            foreach (Reactome_library_line_class reactome_line in this.Reactome)
            {
                if (!organismString_organismEnum_dict.ContainsKey(reactome_line.Organism_string))
                {
                    switch (reactome_line.Organism_string)
                    {
                        case "Bos taurus":
                        case "Caenorhabditis elegans":
                        case "Danio rerio":
                        case "Dictyostelium discoideum":
                        case "Drosophila melanogaster":
                        case "Gallus gallus":
                        case "Homo sapiens":
                        case "Mus musculus":
                        case "Mycobacterium tuberculosis":
                        case "Plasmodium falciparum":
                        case "Rattus norvegicus":
                        case "Saccharomyces cerevisiae":
                        case "Schizosaccharomyces pombe":
                        case "Sus scrofa":
                        case "Xenopus tropicalis":
                            try
                            {
                                reactome_line.Pathway_organism = (Organism_enum)Enum.Parse(typeof(Organism_enum), reactome_line.Organism_string.Replace(" ", "_"));
                            }
                            catch
                            {
                                reactome_line.Pathway_organism = Organism_enum.E_m_p_t_y;
                            }
                           break;
                        case "Canis familiaris":
                            reactome_line.Pathway_organism = Organism_enum.Canis_lupus_familiaris;
                            break;
                        default:
                            if (Global_class.Do_internal_checks) { throw new Exception(); }
                            reactome_line.Pathway_organism = Organism_enum.E_m_p_t_y;
                            break;
                    }
                    organismString_organismEnum_dict.Add(reactome_line.Organism_string, reactome_line.Pathway_organism);
                }
                else
                {
                    reactome_line.Pathway_organism = organismString_organismEnum_dict[reactome_line.Organism_string];
                }
            }
        }

        private void Keep_only_lines_with_selected_pathway_organisms(params Organism_enum[] organisms)
        {
            Dictionary<Organism_enum, bool> organisms_dict = new Dictionary<Organism_enum, bool>();
            foreach (Organism_enum organism in organisms)
            {
                organisms_dict.Add(organism, true);
            }

            List<Reactome_library_line_class> keep = new List<Reactome_library_line_class>();
            foreach (Reactome_library_line_class reactome_line in this.Reactome)
            {
                if (organisms_dict.ContainsKey(reactome_line.Pathway_organism))
                {
                    keep.Add(reactome_line);
                }
            }
            this.Reactome = keep.ToArray();
        }

        private void Keep_only_lines_with_entity_that_is_non_empty()
        {
            List<Reactome_library_line_class> keep = new List<Reactome_library_line_class>();
            List<Reactome_library_line_class> remove = new List<Reactome_library_line_class>();
            foreach (Reactome_library_line_class library_line in this.Reactome)
            {
                if (!string.IsNullOrEmpty(library_line.Entity))
                {
                    keep.Add(library_line);
                }
                else if (Global_class.Do_internal_checks)
                {
                    remove.Add(library_line);
                }
            }
            this.Reactome = keep.ToArray();
        }

        private void Add_pathwayNames_from_hierarchy_based_on_pathwayId_keep_only_lines_with_pathwayIds_in_hierarchy_and_check_if_intial_names_identical(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            MBCO_obo_network_class parent_child_network = new MBCO_obo_network_class(ontology, SCP_hierarchy_interaction_type_enum.Parent_child, organism);
            parent_child_network.Generate_by_reading_safed_spreadsheet_file_or_obo_file_and_return_if_finalized(progressReport, out bool not_interrupted);

            Dictionary<string, string> pathwayId_pathway_dict = parent_child_network.Nodes.Get_pathwayId_pathway_dict();
            int missing_pathwayIds = 0;
            string pathwayName;
            List<Reactome_library_line_class> keep = new List<Reactome_library_line_class>();
            foreach (Reactome_library_line_class library_line in this.Reactome)
            {
                if (pathwayId_pathway_dict.ContainsKey(library_line.Reactome_pathway_id))
                {
                    pathwayName = pathwayId_pathway_dict[library_line.Reactome_pathway_id];
                    if (pathwayName.IndexOf(library_line.Pathway) != 0) { throw new Exception("Pathway name for same pathway id differs in Reactome gene annotation and Reactome hierarchy: " + library_line.Reactome_pathway_id + ": " + library_line.Pathway + " vs " + pathwayName); }
                    library_line.Pathway = pathwayId_pathway_dict[library_line.Reactome_pathway_id];
                    keep.Add(library_line);
                }
                else
                {
                    missing_pathwayIds++;
                }
            }
            this.Reactome = keep.ToArray();
        }

        private void Keep_only_lines_with_identical_gene_and_pathway_organism()
        {
            List<Reactome_library_line_class> keep = new List<Reactome_library_line_class>();
            List<Reactome_library_line_class> remove = new List<Reactome_library_line_class>();
            foreach (Reactome_library_line_class library_line in this.Reactome)
            {
                if (library_line.Gene_organism.Equals(library_line.Pathway_organism))
                {
                    keep.Add(library_line);
                }
                else if (Global_class.Do_internal_checks)
                {
                    remove.Add(library_line);
                }
            }
            if (Global_class.Do_internal_checks)
            {
            //    if (keep.Count!=this.Reactome.Length) { throw new Exception(); }
            }
            this.Reactome = keep.ToArray();
        }


        public void Generate_organism_selective_by_reading(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            Read(ontology,organism,progressReport);
            Set_pathway_organism_from_organism_string();
            Keep_only_lines_with_selected_pathway_organisms(organism);
            Add_pathwayNames_from_hierarchy_based_on_pathwayId_keep_only_lines_with_pathwayIds_in_hierarchy_and_check_if_intial_names_identical(ontology, organism, progressReport);
            Add_entity_and_gene_organism_for_ncbi(organism, progressReport);
            Keep_only_lines_with_entity_that_is_non_empty();
            Keep_only_lines_with_identical_gene_and_pathway_organism();
            Remove_duplicated_scpID_scp_gene_associations();
        }

        public Organism_enum[] Get_all_organisms()
        {
            Dictionary<Organism_enum, bool> organism_dict = new Dictionary<Organism_enum, bool>();
            foreach (Reactome_library_line_class library_line in this.Reactome)
            {
                if (!organism_dict.ContainsKey(library_line.Pathway_organism))
                {
                    organism_dict.Add(library_line.Pathway_organism, true);
                }
            }
            return organism_dict.Keys.ToArray();
        }

        private void Read(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            Reactome_library_readWriteOptions readWriteOptions = new Reactome_library_readWriteOptions(ontology, organism);
            string shared_error_response = Ontology_classification_class.Get_pleaseDonwload_file_again_message(readWriteOptions.File);
            this.Reactome = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Reactome_library_line_class>(readWriteOptions, progressReport, shared_error_response);
        }

    }

}
