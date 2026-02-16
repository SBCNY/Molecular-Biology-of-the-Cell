// Jackson Labs is missing

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using Windows_forms;
using Common_functions.Array_own;
using Other_ontologies_and_databases;
using Network;

namespace Gene_databases
{
    public enum SpeciesSwitch_enum { E_m_p_t_y, Ncbi, Jackson_labs, Simple_transfer }

    /// ///////////////////////////////////////////////////////////////////////////////////////

    class NCBI_ortholog_readOptions_class : ReadWriteOptions_base
    {
        public NCBI_ortholog_readOptions_class(Organism_enum source_organism, Organism_enum target_organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            File = gdf.Complete_ncbi_ortholgs_download_fileName;
            Key_propertyNames = new string[] { "TaxID", "GeneID", "Other_taxID", "Other_geneID"};
            Key_columnNames = new string[] { "#tax_id", "GeneID", "Other_tax_id", "Other_GeneID"};


            SaveOrAndCondition_columnNames = new string[2][];
            SaveOrAndCondition_entries = new string[2][];
            SaveOrAndCondition_columnNames[0] = new string[] { "#tax_id", "Other_tax_id" };
            SaveOrAndCondition_entries[0] = new string[] { ((int)source_organism).ToString(), ((int)target_organism).ToString()};
            SaveOrAndCondition_columnNames[1] = new string[] { "#tax_id", "Other_tax_id" };
            SaveOrAndCondition_entries[1] = new string[] { ((int)target_organism).ToString(), ((int)source_organism).ToString()};

            File_has_headline = true;
            RemoveFromHeadline = null;
            LineDelimiters = new char[] { Global_class.Tab };
            HeadlineDelimiters = new char[] { Global_class.Tab };

            Report = ReadWrite_report_enum.Report_everything;
        }
    }

    class NCBI_ortholog_line_class
    {
        public int TaxID { get; set; }
        public Organism_enum Organism { get; set; }
        public int GeneID { get; set; }
        public int Other_taxID { get; set; }
        public Organism_enum Other_organism { get; set; }
        public int Other_geneID { get; set; }
    }

    class NCBI_ortholog_class
    {
        public NCBI_ortholog_line_class[] NCBI_orthologs { get; set; }
        public Organism_enum Source_organism { get; private set; }
        public Organism_enum Target_organism { get; private set; }

        public NCBI_ortholog_class(Organism_enum source_organsim, Organism_enum target_organism)
        {
            Source_organism = source_organsim;
            Target_organism = target_organism;
        }

        private void Keep_only_selected_organisms_and_add_organisms()
        {
            List<NCBI_ortholog_line_class> keep = new List<NCBI_ortholog_line_class>();
            foreach (NCBI_ortholog_line_class ortholog_line in NCBI_orthologs)
            {
                if ((ortholog_line.TaxID.Equals((int)Source_organism))
                    && (ortholog_line.Other_taxID.Equals((int)Target_organism)))
                {
                    ortholog_line.Organism = Source_organism;
                    ortholog_line.Other_organism = Target_organism;
                    keep.Add(ortholog_line);
                }
                else if (  (ortholog_line.TaxID.Equals((int)Target_organism))
                         &&(ortholog_line.Other_taxID.Equals((int)Source_organism)))
                {
                    ortholog_line.Organism = Target_organism;
                    ortholog_line.Other_organism = Source_organism;
                    keep.Add(ortholog_line);
                }
            }
            if (!keep.Count.Equals(this.NCBI_orthologs.Length)) { throw new Exception("Assumptions about NBCI orthologs are wrong"); }
            this.NCBI_orthologs = keep.ToArray();
        }

        public void Generate_from_raw_data(ProgressReport_interface_class progress_report)
        {
            ReadRawData(progress_report);
            Keep_only_selected_organisms_and_add_organisms();
        }

        private void ReadRawData(ProgressReport_interface_class progress_report)
        {
            NCBI_ortholog_readOptions_class readOptions = new NCBI_ortholog_readOptions_class(Source_organism, Target_organism);
            string shared_response = System.IO.Path.GetFileName(readOptions.File) + " not found. Please see Download_all_datasets.txt for instructions.";
            NCBI_orthologs = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<NCBI_ortholog_line_class>(readOptions, progress_report, shared_response);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

    class Mgi_orthologue_line_class
    {
        public string DB_class_key { get; set; }
        public string Common_organism_name { get; set; }
        public int Ncbi_taxon_id { get; set; }
        public string Symbol { get; set; }

        public int EntrezGene_ID { get; set; }

        public Mgi_orthologue_line_class Deep_copy()
        {
            Mgi_orthologue_line_class copy = (Mgi_orthologue_line_class)this.MemberwiseClone();
            copy.DB_class_key = (string)this.DB_class_key.Clone();
            copy.Common_organism_name = (string)this.Common_organism_name.Clone();
            copy.Ncbi_taxon_id = this.Ncbi_taxon_id;
            copy.Symbol = (string)this.Symbol.Clone();
            return copy;
        }
    }

    class Mgi_orthologue_download_readWriteOptions_class : ReadWriteOptions_base
    {
        public Mgi_orthologue_download_readWriteOptions_class()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Complete_mgi_orthologs_download_fileName;
            this.Key_propertyNames = new string[] { "DB_class_key", "Common_organism_name", "Ncbi_taxon_id", "Symbol", "EntrezGene_ID" };
            this.Key_columnNames = new string[] { "DB Class Key", "Common Organism Name", "NCBI Taxon ID", "Symbol", "EntrezGene ID" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class Mgi_ortholog_class
    {
        public Mgi_orthologue_line_class[] Mgi_orthologs { get; set; }

        public void Generate(ProgressReport_interface_class progress_report)
        {
            Read(progress_report);
        }

        private void Read(ProgressReport_interface_class progress_report)
        {
            Mgi_orthologue_download_readWriteOptions_class readWriteOptions = new Mgi_orthologue_download_readWriteOptions_class();
            string shared_response = System.IO.Path.GetFileName(readWriteOptions.File) + " not found. Please see Download_all_datasets.txt for instructions.";
            this.Mgi_orthologs = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Mgi_orthologue_line_class>(readWriteOptions, progress_report, shared_response);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

    class Orthologue_appGenerated_readWriteOptions_class : ReadWriteOptions_base
    {
        public Orthologue_appGenerated_readWriteOptions_class(Organism_enum source_organism, Organism_enum target_organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            StringBuilder file = new StringBuilder();
            File = gdf.Get_appGenerated_complete_orthology_fileName(source_organism, target_organism);
            Key_propertyNames = new string[] { "Species_0", "TaxID_0", "Symbol_0", "GeneID_0", "Species_1", "TaxID_1", "Symbol_1", "GeneID_1", "Origin" };
            Key_columnNames = Key_propertyNames;
            Key_columnIndexes = null;

            File_has_headline = true;
            RemoveFromHeadline = new string[0];
            LineDelimiters = new char[] { Global_class.Tab };
            HeadlineDelimiters = new char[] { Global_class.Tab };

            Report = ReadWrite_report_enum.Report_everything;
        }
    }

    class Orthologue_line_class
    {
        public string Symbol_0 { get; set; }
        public int GeneID_0 { get; set; }
        public int TaxID_0 { get; set; }
        public Organism_enum Species_0 { get; set; }
        public string Symbol_1 { get; set; }
        public int GeneID_1 { get; set; }
        public int TaxID_1 { get; set; }
        public Organism_enum Species_1 { get; set; }
        public SpeciesSwitch_enum Origin { get; set; }
    }

    class Orthologue_class : IDisposable
    {
        public Orthologue_line_class[] Ortho { get; set; }
        public Organism_enum Source_organism { get; private set; }
        public Organism_enum Target_organism { get; private set; }

        public Orthologue_class(Organism_enum source_organism, Organism_enum target_organism)
        {
            Source_organism = source_organism;
            Target_organism = target_organism;
            Ortho = new Orthologue_line_class[0];
        }

        #region Generate
        private void Add_to_array(Orthologue_line_class[] add_orthologs)
        {
            int this_length = this.Ortho.Length;
            int add_length = add_orthologs.Length;
            int new_length = this_length + add_length;
            Orthologue_line_class[] new_ortho = new Orthologue_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_ortho[indexNew] = this.Ortho[indexThis];
            }
            for (int indexAdd= 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_ortho[indexNew] = add_orthologs[indexAdd];
            }
            this.Ortho = new_ortho;
        }
        private void Fill_with_NCBI_orthologs(ProgressReport_interface_class progress_report)
        {
            NCBI_ortholog_class ncbi_orthologs = new NCBI_ortholog_class(Source_organism, Target_organism);
            ncbi_orthologs.Generate_from_raw_data(progress_report);
            int ncbi_orthologs_length = ncbi_orthologs.NCBI_orthologs.Length;
            NCBI_ortholog_line_class ncbi_ortholog_line;
            Orthologue_line_class ortholog_line;
            List<Orthologue_line_class> add_orthologs = new List<Orthologue_line_class>();
            for (int indexNCBI = 0; indexNCBI < ncbi_orthologs_length; indexNCBI++)
            {
                ncbi_ortholog_line = ncbi_orthologs.NCBI_orthologs[indexNCBI];
                ortholog_line = new Orthologue_line_class();
                ortholog_line.Origin = SpeciesSwitch_enum.Ncbi;
                if (  (ncbi_ortholog_line.Organism.Equals(Source_organism))
                    &&(ncbi_ortholog_line.Other_organism.Equals(Target_organism)))
                {
                    ortholog_line.GeneID_0 = ncbi_ortholog_line.GeneID;
                    ortholog_line.Species_0 = ncbi_ortholog_line.Organism;
                    ortholog_line.TaxID_0 = ncbi_ortholog_line.TaxID;
                    ortholog_line.GeneID_1 = ncbi_ortholog_line.Other_geneID;
                    ortholog_line.Species_1 = ncbi_ortholog_line.Other_organism;
                    ortholog_line.TaxID_1 = ncbi_ortholog_line.Other_taxID;
                }
                else if (   (ncbi_ortholog_line.Organism.Equals(Target_organism))
                         && (ncbi_ortholog_line.Other_organism.Equals(Source_organism)))
                {
                    ortholog_line.GeneID_1 = ncbi_ortholog_line.GeneID;
                    ortholog_line.Species_1 = ncbi_ortholog_line.Organism;
                    ortholog_line.TaxID_1 = ncbi_ortholog_line.TaxID;
                    ortholog_line.GeneID_0 = ncbi_ortholog_line.Other_geneID;
                    ortholog_line.Species_0 = ncbi_ortholog_line.Other_organism;
                    ortholog_line.TaxID_0 = ncbi_ortholog_line.Other_taxID;
                }
                else { throw new Exception(); }
                add_orthologs.Add(ortholog_line);
            }
            Add_to_array(add_orthologs.ToArray());
        }
        private void Fill_with_jackson_orthology(ProgressReport_interface_class progress_report)
        {
            Mgi_ortholog_class mgi = new Mgi_ortholog_class();
            mgi.Generate(progress_report);
            mgi.Mgi_orthologs = mgi.Mgi_orthologs.OrderBy(l => l.DB_class_key).ToArray();
            int mgi_length = mgi.Mgi_orthologs.Length;
            Mgi_orthologue_line_class mgi_ortho_line;
            List<int> geneID_organism0 = new List<int>();
            List<int> geneID_organism1 = new List<int>();
            Dictionary<int, List<string>> geneID0_symbols0_dict = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> geneID1_symbols1_dict = new Dictionary<int, List<string>>();
            Dictionary<int, int> geneID0_taxID0_dict = new Dictionary<int, int>();
            Dictionary<int, int> geneID1_taxID1_dict = new Dictionary<int, int>();
            Orthologue_line_class newOrthoLine;
            int lines_added = 0;
            int in_both_organisms_multiple_orthologes_count = 0;
            List<Orthologue_line_class> add_orthologs = new List<Orthologue_line_class>();
            for (int indexMgi = 0; indexMgi < mgi_length; indexMgi++)
            {
                mgi_ortho_line = mgi.Mgi_orthologs[indexMgi];
                if (   (indexMgi == 0)
                    || (!mgi_ortho_line.DB_class_key.Equals(mgi.Mgi_orthologs[indexMgi - 1].DB_class_key)))
                {
                    geneID_organism0.Clear();
                    geneID_organism1.Clear();
                    geneID0_symbols0_dict.Clear();
                    geneID1_symbols1_dict.Clear();
                    geneID0_taxID0_dict.Clear();
                    geneID1_taxID1_dict.Clear();
                }
                if (mgi_ortho_line.Ncbi_taxon_id == (int)Source_organism)
                {
                    geneID_organism0.Add(mgi_ortho_line.EntrezGene_ID);
                    if (!geneID0_symbols0_dict.ContainsKey(mgi_ortho_line.EntrezGene_ID))
                    { geneID0_symbols0_dict.Add(mgi_ortho_line.EntrezGene_ID, new List<string>()); }
                    geneID0_symbols0_dict[mgi_ortho_line.EntrezGene_ID].Add(mgi_ortho_line.Symbol);
                    geneID0_symbols0_dict[mgi_ortho_line.EntrezGene_ID] = geneID0_symbols0_dict[mgi_ortho_line.EntrezGene_ID].Distinct().ToList();
                    if (!geneID0_taxID0_dict.ContainsKey(mgi_ortho_line.EntrezGene_ID))
                    { geneID0_taxID0_dict.Add(mgi_ortho_line.EntrezGene_ID, mgi_ortho_line.Ncbi_taxon_id); }
                    else if (!geneID0_taxID0_dict[mgi_ortho_line.EntrezGene_ID].Equals(mgi_ortho_line.Ncbi_taxon_id)) { throw new Exception("Multiple gene symbols found for same gene id in MGI orthologs."); }
                }
                if (mgi_ortho_line.Ncbi_taxon_id == (int)Target_organism)
                {
                    geneID_organism1.Add(mgi_ortho_line.EntrezGene_ID);
                    if (!geneID1_symbols1_dict.ContainsKey(mgi_ortho_line.EntrezGene_ID))
                    { geneID1_symbols1_dict.Add(mgi_ortho_line.EntrezGene_ID, new List<string>()); }
                    geneID1_symbols1_dict[mgi_ortho_line.EntrezGene_ID].Add(mgi_ortho_line.Symbol);
                    geneID1_symbols1_dict[mgi_ortho_line.EntrezGene_ID] = geneID1_symbols1_dict[mgi_ortho_line.EntrezGene_ID].Distinct().ToList();
                    if (!geneID1_taxID1_dict.ContainsKey(mgi_ortho_line.EntrezGene_ID))
                    { geneID1_taxID1_dict.Add(mgi_ortho_line.EntrezGene_ID, mgi_ortho_line.Ncbi_taxon_id); }
                    else if (!geneID1_taxID1_dict[mgi_ortho_line.EntrezGene_ID].Equals(mgi_ortho_line.Ncbi_taxon_id)) { throw new Exception("Multiple gene symbols found for same gene id in MGI orthologs."); }
                }
                if (  (indexMgi == mgi_length - 1)
                    ||(!mgi_ortho_line.DB_class_key.Equals(mgi.Mgi_orthologs[indexMgi + 1].DB_class_key)))
                {
                    int symbol_organism0_count = geneID_organism0.Count;
                    int symbol_organism1_count = geneID_organism1.Count;
                    if ((symbol_organism0_count > 0) && (symbol_organism1_count > 0))
                    {
                        if ((symbol_organism0_count > 1) && (symbol_organism1_count > 1))
                        {
                            in_both_organisms_multiple_orthologes_count++;
                        }
                        for (int indexS0 = 0; indexS0 < symbol_organism0_count; indexS0++)
                        {
                            for (int indexS1 = 0; indexS1 < symbol_organism1_count; indexS1++)
                            {
                                if (geneID0_symbols0_dict[geneID_organism0[indexS0]].Count > 1) { throw new Exception(); }
                                if (geneID1_symbols1_dict[geneID_organism1[indexS1]].Count > 1) { throw new Exception(); }

                                newOrthoLine = new Orthologue_line_class();
                                newOrthoLine.GeneID_0 = geneID_organism0[indexS0];
                                newOrthoLine.GeneID_1 = geneID_organism1[indexS1];
                                newOrthoLine.Symbol_0 = (string)geneID0_symbols0_dict[newOrthoLine.GeneID_0][0].Clone();
                                newOrthoLine.Symbol_1 = (string)geneID1_symbols1_dict[newOrthoLine.GeneID_1][0].Clone();
                                newOrthoLine.TaxID_0 = geneID0_taxID0_dict[newOrthoLine.GeneID_0];
                                newOrthoLine.TaxID_1 = geneID1_taxID1_dict[newOrthoLine.GeneID_1];
                                newOrthoLine.Species_0 = (Organism_enum)Enum.ToObject(typeof(Organism_enum), newOrthoLine.TaxID_0);
                                newOrthoLine.Species_1 = (Organism_enum)Enum.ToObject(typeof(Organism_enum), newOrthoLine.TaxID_1);
                                newOrthoLine.Origin = SpeciesSwitch_enum.Jackson_labs;
                                add_orthologs.Add(newOrthoLine);
                                lines_added++;
                            }
                        }
                    }
                }
            }
            Add_to_array(add_orthologs.ToArray());
        }

        private void Add_symbols_from_geneInfo(ProgressReport_interface_class progressReport)
        {
            GeneInfo_class geneInfo = new GeneInfo_class(new Organism_enum[] { Source_organism, Target_organism });
            geneInfo.Generate_deNovo_and_save_or_read_appGenerated_geneInfo(progressReport);
            Dictionary<int, string> geneID_geneSymbol_dict = geneInfo.Get_geneID_ncbiGeneSymbol_dict();
            foreach (Orthologue_line_class orthologue_line in this.Ortho)
            {
                if (geneID_geneSymbol_dict.ContainsKey(orthologue_line.GeneID_0))
                {
                    //if (String.IsNullOrEmpty(ortholog_line.Symbol_0))
                    //{
                        orthologue_line.Symbol_0 = (string)geneID_geneSymbol_dict[orthologue_line.GeneID_0].Clone();
                    //}
                    //else if (!ortholog_line.Symbol_0.Equals(geneID_geneSymbol_dict[ortholog_line.GeneID_0]))
                    //{
                      //  if (Global_class.Do_internal_checks) { throw new Exception(); }
                    //}
                }
                if (geneID_geneSymbol_dict.ContainsKey(orthologue_line.GeneID_1))
                {
                    //if (String.IsNullOrEmpty(ortholog_line.Symbol_1))
                    //{
                        orthologue_line.Symbol_1 = (string)geneID_geneSymbol_dict[orthologue_line.GeneID_1].Clone();
                    //}
                    //else if (!ortholog_line.Symbol_1.Equals(geneID_geneSymbol_dict[ortholog_line.GeneID_1]))
                    //{
                      //  if (Global_class.Do_internal_checks) { throw new Exception(); }
                    //}
                }
            }
        }
        private void Remove_duplicates()
        {
            SpeciesSwitch_enum[] switchPriority = new SpeciesSwitch_enum[] { SpeciesSwitch_enum.Jackson_labs, SpeciesSwitch_enum.Ncbi };

            Ortho = Ortho.OrderBy(l => l.Symbol_0).ThenBy(l => l.Origin).ToArray();
            int ortho_count = Ortho.Length;
            int indexFirstSymbolDup = -1;
            List<SpeciesSwitch_enum> origins = new List<SpeciesSwitch_enum>();
            SpeciesSwitch_enum keepOrigin = SpeciesSwitch_enum.E_m_p_t_y;
            int addedHomologene = 0;
            int addedJackson = 0;


            List<Orthologue_line_class> newOrtho = new List<Orthologue_line_class>();
            Orthologue_line_class line;
            for (int indexOrtho = 0; indexOrtho < ortho_count; indexOrtho++)
            {
                line = Ortho[indexOrtho];
                if ((indexOrtho == 0) || (!line.Symbol_0.Equals(Ortho[indexOrtho - 1].Symbol_0)))
                {
                    indexFirstSymbolDup = indexOrtho;
                    origins.Clear();
                }
                if ((indexOrtho == 0) || (!line.Symbol_0.Equals(Ortho[indexOrtho - 1].Symbol_0)) || (!line.Origin.Equals(Ortho[indexOrtho - 1].Origin)))
                {
                    origins.Add(line.Origin);
                }
                if ((indexOrtho == ortho_count - 1) || (!line.Symbol_0.Equals(Ortho[indexOrtho + 1].Symbol_0)))
                {
                    for (int indexP = 0; indexP < switchPriority.Length; indexP++)
                    {
                        if (origins.Contains(switchPriority[indexP]))
                        {
                            keepOrigin = switchPriority[indexP];
                            break;
                        }
                    }
                    for (int indexAdd = indexFirstSymbolDup; indexAdd <= indexOrtho; indexAdd++)
                    {
                        if (Ortho[indexAdd].Origin == keepOrigin)
                        {
                            newOrtho.Add(Ortho[indexAdd]);
                            switch (keepOrigin)
                            {
                                case SpeciesSwitch_enum.Ncbi:
                                    addedHomologene++;
                                    break;
                                case SpeciesSwitch_enum.Jackson_labs:
                                    addedJackson++;
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                    }
                }
            }
            Ortho = newOrtho.ToArray();
        }
        private void Remove_unfilled_symbols()
        {
            List<Orthologue_line_class> newOrtho = new List<Orthologue_line_class>();
            int emptyNCBI_count = 0;
            int emptyMGI_count = 0;
            int totalNCBI_count = 0;
            int totalMGI_count = 0;
            foreach (Orthologue_line_class line in Ortho)
            {
                switch (line.Origin)
                {
                    case SpeciesSwitch_enum.Ncbi:
                        totalNCBI_count++;
                        break;
                    case SpeciesSwitch_enum.Jackson_labs:
                        totalMGI_count++;
                        break;
                    default:
                        throw new Exception();
                }
                if ((!String.IsNullOrEmpty(line.Symbol_0)) && (!String.IsNullOrEmpty(line.Symbol_1)))
                {
                    newOrtho.Add(line);
                }
                else
                {
                    switch (line.Origin)
                    {
                        case SpeciesSwitch_enum.Ncbi:
                            emptyNCBI_count++;
                            break;
                        case SpeciesSwitch_enum.Jackson_labs:
                            emptyMGI_count++;
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            Ortho = newOrtho.ToArray();
        }
        private void Set_all_symbols_0_and_1_to_upper_case()
        {
            foreach (Orthologue_line_class homologene_line in this.Ortho)
            {
                homologene_line.Symbol_0 = homologene_line.Symbol_0.ToUpper();
                homologene_line.Symbol_1 = homologene_line.Symbol_1.ToUpper();
            }
        }
        private void Generate_new_and_write(ProgressReport_interface_class progress_report)
        {
            progress_report.Update_progressReport_text_and_visualization("Combining orthologues from MGI and NCBI and adding official NCBI gene symbols from gene info. Results will be saved for faster access next time.");
            Ortho = new Orthologue_line_class[0];

            Organism_enum[] jackson_organisms = new Organism_enum[] { Organism_enum.Homo_sapiens, Organism_enum.Mus_musculus, Organism_enum.Rattus_norvegicus };

            Fill_with_NCBI_orthologs(progress_report);
            Fill_with_jackson_orthology(progress_report);
            Add_symbols_from_geneInfo(progress_report);
            Remove_unfilled_symbols();
            Set_all_symbols_0_and_1_to_upper_case();
            Remove_duplicates();
            Write_appGenerated_file(progress_report, out bool file_opened_succesful);
            if (file_opened_succesful)
            { progress_report.Update_progressReport_text_and_visualization(""); }
            else
            { progress_report.Update_progressReport_text_and_visualization("App generated files could not be written. Are they in use? Is the directory name too long?"); }
        }
        public void Generate(ProgressReport_interface_class progress_report)
        {
            Orthologue_appGenerated_readWriteOptions_class readWrite_options = new Orthologue_appGenerated_readWriteOptions_class(Source_organism, Target_organism);
            if (System.IO.File.Exists(readWrite_options.File))
            {
                Read_appGenerated_file(progress_report);
            }
            else
            {
                Generate_new_and_write(progress_report);
            }
        }
        #endregion

        #region Order
        public void Order_by_symbol_0()
        {
            Ortho = Ortho.OrderBy(l => l.Symbol_0).ToArray();
        }
        #endregion

        #region Get
        public string[] Get_all_symbols_of_organism(Organism_enum organism)
        {
            int zero_or_first_symbol = -1;
            if (Source_organism == organism)
            {
                zero_or_first_symbol = 0;
            }
            else if (Target_organism == organism)
            {
                zero_or_first_symbol = 1;
            }
            else
            {
                throw new Exception();
            }

            int ortho_count = Ortho.Length;
            string[] all_symbols = new string[ortho_count];
            Orthologue_line_class line;
            for (int indexOrtho = 0; indexOrtho < ortho_count; indexOrtho++)
            {
                line = Ortho[indexOrtho];
                switch (zero_or_first_symbol)
                {
                    case 0:
                        all_symbols[indexOrtho] = (string)line.Symbol_0.Clone();
                        break;
                    case 1:
                        all_symbols[indexOrtho] = (string)line.Symbol_1.Clone();
                        break;
                    default:
                        break;
                }
            }
            return all_symbols.Distinct().ToArray();
        }
        #endregion

        public Dictionary<string, string[]> Get_humanGene_speciesOrthologs_dict()
        {
            Organism_enum organism_0 = Organism_enum.Homo_sapiens;
            Organism_enum organism_1 = this.Ortho[0].Species_1;
            Dictionary<string, string[]> humanGene_speciesOrthologs_dict = new Dictionary<string, string[]>();
            foreach (Orthologue_line_class ortho_line in this.Ortho)
            {
                if (!ortho_line.Species_0.Equals(organism_0)) { throw new Exception("First species is not human"); }
                if (!ortho_line.Species_1.Equals(organism_1)) { throw new Exception("Multiple second species exist"); }
                if (!humanGene_speciesOrthologs_dict.ContainsKey(ortho_line.Symbol_0))
                {
                    humanGene_speciesOrthologs_dict.Add(ortho_line.Symbol_0, new string[] { ortho_line.Symbol_1 });
                }
                else
                {
                    humanGene_speciesOrthologs_dict[ortho_line.Symbol_0] = Overlap_class.Get_ordered_union_of_string_arrays(humanGene_speciesOrthologs_dict[ortho_line.Symbol_0], ortho_line.Symbol_1);
                }
            }
            return humanGene_speciesOrthologs_dict;
        }

        #region Read write
        private void Write_appGenerated_file(ProgressReport_interface_class progressReport, out bool fileName_opened_successful)
        {
            Orthologue_appGenerated_readWriteOptions_class ReadWrite_options = new Orthologue_appGenerated_readWriteOptions_class(Source_organism, Target_organism);
            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed<Orthologue_line_class>(Ortho, ReadWrite_options, progressReport, out fileName_opened_successful);
        }
        private void Read_appGenerated_file(ProgressReport_interface_class progressReport)
        {
            Orthologue_appGenerated_readWriteOptions_class ReadWriteOptions = new Orthologue_appGenerated_readWriteOptions_class(Source_organism, Target_organism);
            Ortho = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Orthologue_line_class>(ReadWriteOptions, progressReport, "error").ToArray();
        }
        #endregion

        public void Dispose()
        {
            Ortho = new Orthologue_line_class[0];
            Ortho = null;
            Source_organism = Organism_enum.E_m_p_t_y;
            Target_organism = Organism_enum.E_m_p_t_y;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

}
