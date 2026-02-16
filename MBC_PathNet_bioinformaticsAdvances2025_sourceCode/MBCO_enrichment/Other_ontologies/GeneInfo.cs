using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_forms;

namespace Other_ontologies_and_databases
{
    class GeneInfo_readOptions_class : ReadWriteOptions_base
    {
        public GeneInfo_readOptions_class(params Organism_enum[] organisms)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            File = gdf.Complete_geneInfo_download_fileName;
            Key_propertyNames = new string[] { "TaxID", "GeneID", "Symbol", "LocusTag", "Synonyms", "Description", "dbXrefs" };
            Key_columnNames = new string[] { "#tax_id", "GeneID", "Symbol", "LocusTag", "Synonyms", "description", "dbXrefs" };
            Key_columnIndexes = null;

            File_has_headline = true;
            RemoveFromHeadline = new string[0];// { "#", "Format:" };

            SaveOrAndCondition_columnNames = new string[organisms.Length][];
            SaveOrAndCondition_columnIndexes = null;
            SaveOrAndCondition_entries = new string[organisms.Length][];
            for (int i = 0; i < organisms.Length; i++)
            {
                SaveOrAndCondition_columnNames[i] = new string[] { "#tax_id" };
                SaveOrAndCondition_entries[i] = new string[] { ((int)organisms[i]).ToString() };
            }

            LineDelimiters = new char[] { Global_class.Tab };
            HeadlineDelimiters = new char[] { Global_class.Tab };

            Report = ReadWrite_report_enum.Report_everything;
        }
    }

    class GeneInfo_appGenerated_readWriteOptions : ReadWriteOptions_base
    {
        public GeneInfo_appGenerated_readWriteOptions(Organism_enum organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            File = gdf.Get_appGenerated_complete_geneInfo_fileName(organism);
            Key_propertyNames = new string[] { "Species", "TaxID", "GeneID", "Symbol", "LocusTag", "Synonyms", "Description", "dbXrefs" };
            Key_columnNames = Key_propertyNames;
            Key_columnIndexes = null;

            File_has_headline = true;
            RemoveFromHeadline = new string[0];// { "#", "Format:" };

            LineDelimiters = new char[] { Global_class.Tab };
            HeadlineDelimiters = new char[] { Global_class.Tab };

            Report = ReadWrite_report_enum.Report_everything;
        }

    }

    class GeneInfo_line_class
    {
        public int TaxID { get; set; }
        public Organism_enum Species { get; set; }
        public int GeneID { get; set; }
        public string Synonyms { get; set; }
        public string Symbol { get; set; }
        public string LocusTag { get; set; }
        public string Description { get; set; }
        public string Ensemble { get; set; }
        public string dbXrefs { get; set; }

        public GeneInfo_line_class()
        {
        }

    }

    class GeneInfo_class
    {
        const char separator = '|';
        const string empty_sign = "-";
        public GeneInfo_line_class[] Data { get; set; }
        Organism_enum[] Organisms { get; set; }
        public char Separator { get { return separator; } }
        public string Empty_sign { get { return empty_sign; } }

        public GeneInfo_class(Organism_enum[] organisms)
        {
            Organisms = organisms;
            this.Data = new GeneInfo_line_class[0];
        }

        private void Add_to_array(GeneInfo_line_class[] add_data)
        {
            int this_length = this.Data.Length;
            int add_length = add_data.Length;
            int new_length = this_length + add_length;
            GeneInfo_line_class[] new_data = new GeneInfo_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_data[indexNew] = this.Data[indexThis];
            }
            for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_data[indexNew] = add_data[indexAdd];
            }
            this.Data = new_data;
        }

        #region Order
        protected void Order_by_locusTag()
        {
            Data = Data.OrderBy(l => l.LocusTag).ToArray();
        }
        #endregion

        public void Generate_deNovo_and_save_or_read_appGenerated_geneInfo(ProgressReport_interface_class progressReport)
        {
            List<Organism_enum> missing_organisms = new List<Organism_enum>();
            foreach (Organism_enum organism in Organisms)
            {
                GeneInfo_appGenerated_readWriteOptions readWriteOptions = new GeneInfo_appGenerated_readWriteOptions(organism);
                if (!System.IO.File.Exists(readWriteOptions.File))
                {
                    missing_organisms.Add(organism);
                }
                else
                {
                    Read_appGenerated_data_and_add(organism, progressReport);
                }
            }
            if (missing_organisms.Count>0)
            {
                ReadRawData_save_and_add(missing_organisms.ToArray(),progressReport);
            }
        }

        public void ReadRawData_save_and_add(Organism_enum[] missing_organisms, ProgressReport_interface_class progressReport)
        {
            GeneInfo_readOptions_class readOptions = new GeneInfo_readOptions_class(missing_organisms);
            string shared_error_response = Ontology_classification_class.Get_pleaseDonwload_file_again_message(readOptions.File);
            GeneInfo_line_class[] add_data = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<GeneInfo_line_class>(readOptions, progressReport, shared_error_response);
            add_data = add_data.OrderBy(l => l.TaxID).ToArray();
            GeneInfo_line_class add_data_line;
            List<GeneInfo_line_class> sameOrganism_add_data = new List<GeneInfo_line_class>();
            int add_data_length = add_data.Length;
            for (int indexAdd=0; indexAdd<add_data_length;indexAdd++)
            {
                add_data_line = add_data[indexAdd];
                add_data_line.Species = (Organism_enum)Enum.ToObject(typeof(Organism_enum), add_data_line.TaxID);
                if (  (indexAdd==0)
                    || (!add_data_line.TaxID.Equals(add_data[indexAdd-1].TaxID)))
                {
                    sameOrganism_add_data.Clear();
                }
                sameOrganism_add_data.Add(add_data_line);
                if ((indexAdd == add_data_length-1)
                    || (!add_data_line.TaxID.Equals(add_data[indexAdd + 1].TaxID)))
                {
                    Write_appGenerated_data(sameOrganism_add_data.ToArray(), (Organism_enum)Enum.ToObject(typeof(Organism_enum), add_data_line.TaxID), progressReport, out bool file_opened_successful);
                }
            }
            Add_to_array(add_data);
        }
        private void Read_appGenerated_data_and_add(Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            GeneInfo_appGenerated_readWriteOptions readWriteOptions = new GeneInfo_appGenerated_readWriteOptions(organism);
            string shared_error_response = Ontology_classification_class.Get_pleaseDelete_file_message(readWriteOptions.File);
            GeneInfo_line_class[] add_data = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<GeneInfo_line_class>(readWriteOptions, progressReport, shared_error_response);
            Add_to_array(add_data);
        }
        private void Write_appGenerated_data(GeneInfo_line_class[] save_geneInfo, Organism_enum organism, ProgressReport_interface_class progressReport, out bool file_opened_successful)
        {
            GeneInfo_appGenerated_readWriteOptions readWriteOptions = new GeneInfo_appGenerated_readWriteOptions(organism);
            string shared_error_response = Ontology_classification_class.Get_pleaseDelete_file_message(readWriteOptions.File);
            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(save_geneInfo, readWriteOptions, progressReport, out file_opened_successful);
        }

        public Dictionary<int, string> Get_geneID_ncbiGeneSymbol_dict()
        {
            Dictionary<int, string> geneID_ncbiGeneSymbol_dict = new Dictionary<int, string>();
            foreach (GeneInfo_line_class geneInfo_line in Data)
            {
                if (!geneID_ncbiGeneSymbol_dict.ContainsKey(geneInfo_line.GeneID))
                {
                    geneID_ncbiGeneSymbol_dict.Add(geneInfo_line.GeneID, geneInfo_line.Symbol);
                }
                else if (!geneID_ncbiGeneSymbol_dict[geneInfo_line.GeneID].Equals(geneInfo_line.Symbol)) { throw new Exception(); }
            }
            return geneID_ncbiGeneSymbol_dict;
        }
        public Dictionary<int, Organism_enum> Get_geneID_organism_dict()
        {
            Dictionary<int, Organism_enum> geneID_organism_dict = new Dictionary<int, Organism_enum>();
            foreach (GeneInfo_line_class geneInfo_line in Data)
            {
                if (!geneID_organism_dict.ContainsKey(geneInfo_line.GeneID))
                {
                    geneID_organism_dict.Add(geneInfo_line.GeneID, geneInfo_line.Species);
                }
                else if (!geneID_organism_dict[geneInfo_line.GeneID].Equals(geneInfo_line.Species)) { throw new Exception(); }
            }
            return geneID_organism_dict;
        }
    }
}
