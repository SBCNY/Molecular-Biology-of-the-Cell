#region Author information
/*
The code was written by Jens Hansen working for the Ravi Iyengar Lab
The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
Please acknowledge the MBC Ontology in your publications by citing the following reference:
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: 
A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes
Sci Rep. 2017 Dec 18;7(1):17689. doi: 10.1038/s41598-017-16627-4.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using MBCO;
using Data;
using Common_functions.Array_own;
using Common_functions.Statistic;
using Common_functions.Global_definitions;

namespace Enrichment
{
    class Ontology_fisher_class
    {
        #region Fields
        MBCO_association_class MBCO_association { get; set; }
        Ontology_type_enum Ontology { get; set; }
        Fisher_exact_test_class Fisher_exact { get; set; }
        string[] Bg_genes { get; set; }
        #endregion

        #region Generate and modify MBCO associaton
        public void Generate(MBCO_association_class mbco_association, Ontology_type_enum ontology, params string[] bg_symbols)
        {
            this.Ontology = ontology;
            this.Bg_genes = Array_class.Deep_copy_string_array(bg_symbols);
            this.MBCO_association = mbco_association.Deep_copy();
            if (this.Bg_genes.Length > 0)
            {
                MBCO_association.Keep_only_bg_symbols(bg_symbols);
            }
        }

        public void Add_new_mbco_association_lines_after_filtering_for_bg_genes(MBCO_association_class new_mbco_association)
        {
            if (this.Bg_genes.Length > 0)
            {
                new_mbco_association.Keep_only_bg_symbols(this.Bg_genes);
            }
            this.MBCO_association.Add_array_of_other_MBCO_association(new_mbco_association);
        }

        public void Keep_only_indicated_level(int level)
        {
            MBCO_association.Keep_only_lines_with_indicated_level(level);
        }
        #endregion

        #region Analyse data
        private Ontology_enrichment_line_class[] Generate_enrichment_lines_and_calculate_pvalues(string[] experimental_symbols, Entry_type_enum entryType, int timepoint, string sample_name)
        {
            experimental_symbols = experimental_symbols.Distinct().OrderBy(l => l).ToArray();
            int experimental_symbols_length = experimental_symbols.Length;
            Dictionary<string, int> processName_symbol_count = new Dictionary<string, int>();
            Dictionary<string, List<string>> processName_symbol_overlap_symbols = new Dictionary<string, List<string>>();

            MBCO_association.Order_by_symbol_processName();

            #region Count overlap between process genes and experimental genes
            string experimental_symbol;
            int mbco_associations_length = MBCO_association.MBCO_associations.Length;
            int indexSymbol = 0;
            int stringCompare;
            MBCO_association_line_class mbco_association_line;
            for (int indexMBCO = 0; indexMBCO < mbco_associations_length; indexMBCO++)
            {
                mbco_association_line = MBCO_association.MBCO_associations[indexMBCO];
                stringCompare = -2;
                while ((indexSymbol < experimental_symbols_length) && (stringCompare < 0))
                {
                    experimental_symbol = experimental_symbols[indexSymbol];
                    stringCompare = experimental_symbol.CompareTo(mbco_association_line.Symbol);
                    if (stringCompare < 0)
                    {
                        indexSymbol++;
                    }
                    else if (stringCompare == 0)
                    {
                        #region Overlap symbols
                        if (!processName_symbol_overlap_symbols.ContainsKey(mbco_association_line.ProcessName))
                        {
                            processName_symbol_overlap_symbols.Add(mbco_association_line.ProcessName, new List<string>());
                        }
                        processName_symbol_overlap_symbols[mbco_association_line.ProcessName].Add(experimental_symbol);
                        #endregion
                    }
                }
                #region Process symbol count
                if ((indexMBCO != 0)
                    && (mbco_association_line.ProcessName.Equals(MBCO_association.MBCO_associations[indexMBCO - 1].ProcessName))
                    && (mbco_association_line.Symbol.Equals(MBCO_association.MBCO_associations[indexMBCO - 1].Symbol)))
                {
                    throw new Exception();
                }
                if (!processName_symbol_count.ContainsKey(mbco_association_line.ProcessName))
                {
                    processName_symbol_count.Add(mbco_association_line.ProcessName, 1);
                }
                else
                {
                    processName_symbol_count[mbco_association_line.ProcessName]++;
                }
                #endregion
            }
            #endregion

            #region Generate enrichment lines
            string[] processNames = processName_symbol_overlap_symbols.Keys.ToArray();
            string processName;
            int processNames_length = processNames.Length;
            Ontology_enrichment_line_class[] enrich_lines = new Ontology_enrichment_line_class[processNames_length];
            Ontology_enrichment_line_class new_enrich_line;
            for (int indexP = 0; indexP < processNames_length; indexP++)
            {
                processName = processNames[indexP];
                new_enrich_line = new Ontology_enrichment_line_class();
                new_enrich_line.Ontology_type = this.Ontology;
                new_enrich_line.Scp_name = (string)processName.Clone();
                new_enrich_line.Experimental_symbols_count = experimental_symbols_length;
                new_enrich_line.Process_symbols_count = processName_symbol_count[processName];
                new_enrich_line.Bg_symbol_count = this.Bg_genes.Length;
                new_enrich_line.Overlap_symbols = processName_symbol_overlap_symbols[processName].OrderBy(l => l).ToArray();
                new_enrich_line.Overlap_count = new_enrich_line.Overlap_symbols.Length;
                new_enrich_line.EntryType = entryType;
                new_enrich_line.Timepoint = timepoint;
                new_enrich_line.Sample_name = (string)sample_name.Clone();
                enrich_lines[indexP] = new_enrich_line;
            }
            #endregion

            #region Calculate pvalues
            int enrich_length = enrich_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            int a; int b; int c; int d;
            Fisher_exact_test_class fisher = new Fisher_exact_test_class(this.Bg_genes.Length, false);
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrich_lines[indexE];
                a = enrich_line.Overlap_count;
                b = enrich_line.Experimental_symbols_count - a;
                c = enrich_line.Process_symbols_count - a;
                d = enrich_line.Bg_symbol_count - a - b - c;
                if ((a < 0) || (b < 0) || (c < 0) || (d < 0))
                {
                    throw new Exception("negative values");
                }
                enrich_line.Pvalue = fisher.Get_rightTailed_p_value(a, b, c, d);
                enrich_line.Minus_log10_pvalue = -(float)Math.Log10(enrich_line.Pvalue);
            }
            #endregion

            #region Calculate qvalues and FDR
            enrich_lines = enrich_lines.OrderBy(l => l.Pvalue).ToArray();
            int enrich_lines_length = enrich_lines.Length;
            int rank = 0;
            for (int indexE = 0; indexE < enrich_lines_length; indexE++)
            {
                enrich_line = enrich_lines[indexE];
                rank++;

                enrich_line.Qvalue = enrich_line.Pvalue * ((double)processNames_length / (double)rank);
                if (enrich_line.Qvalue > 1) { enrich_line.Qvalue = 1; }
            }

            double lowest_qvalue = -1;
            for (int indexE = enrich_lines_length - 1; indexE >= 0; indexE--)
            {
                enrich_line = enrich_lines[indexE];
                if ((lowest_qvalue == -1)
                    || (lowest_qvalue > enrich_line.Qvalue))
                {
                    lowest_qvalue = enrich_line.Qvalue;
                }
                enrich_line.FDR = lowest_qvalue;
            }
            #endregion

            return enrich_lines;
        }

        private Ontology_enrichment_line_class[] Add_missing_process_information(Ontology_enrichment_line_class[] enrichment_lines)
        {
            enrichment_lines = enrichment_lines.OrderBy(l => l.Scp_name).ToArray();
            int enrich_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            MBCO_association.Order_by_processName_symbol();
            int onto_length = MBCO_association.MBCO_associations.Length;
            int indexOnto = 0;
            MBCO_association_line_class mbco_association_line;
            int stringCompare;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrichment_lines[indexE];
                stringCompare = -2;
                while ((indexOnto < onto_length) && (stringCompare < 0))
                {
                    mbco_association_line = MBCO_association.MBCO_associations[indexOnto];
                    stringCompare = mbco_association_line.ProcessName.CompareTo(enrich_line.Scp_name);
                    if (stringCompare < 0)
                    {
                        indexOnto++;
                    }
                    else if (stringCompare == 0)
                    {
                        if (!string.IsNullOrEmpty(mbco_association_line.ProcessID)) { enrich_line.Scp_id = (string)mbco_association_line.ProcessID.Clone(); }
                        enrich_line.ProcessLevel = mbco_association_line.ProcessLevel;
                        if (!string.IsNullOrEmpty(mbco_association_line.Parent_processName)) { enrich_line.Parent_scp_name = (string)mbco_association_line.Parent_processName.Clone(); }
                    }
                }
            }
            return enrichment_lines;
        }

        private Ontology_enrichment_line_class[] Add_upregulated_downregulated_symbol_information(Ontology_enrichment_line_class[] enrichment_lines, Data_class data, int indexCol)
        {
            int de_symbols_length = data.Data_length;
            Data_line_class data_line;
            Dictionary<string, float> symbol_value_dict = new Dictionary<string, float>();
            for (int indexS = 0; indexS < de_symbols_length; indexS++)
            {
                data_line = data.Data[indexS];
                if (data_line.Columns[indexCol] != 0)
                {
                    symbol_value_dict.Add(data_line.NCBI_official_symbol, data_line.Columns[indexCol]);
                }
            }

            enrichment_lines = enrichment_lines.OrderBy(l => l.Scp_name).ToArray();
            int enrich_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            string symbol;
            int overlap_symbols_length;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrichment_lines[indexE];
                overlap_symbols_length = enrich_line.Overlap_symbols.Length;
                for (int indexS = 0; indexS < overlap_symbols_length; indexS++)
                {
                    symbol = (string)enrich_line.Overlap_symbols[indexS].Clone();
                    enrich_line.Overlap_symbols[indexS] = symbol + " (" + symbol_value_dict[symbol] + ")";
                }
            }
            return enrichment_lines;
        }

        public Ontology_enrichment_class Analyse_data_instance(Data_class data_input)
        {
            Data_class data = data_input.Deep_copy();
            data.Keep_only_input_rowNames(this.Bg_genes);

            int data_length = data.Data_length;
            int col_length = data.ColChar.Columns_length;
            string[] symbols;
            List<Ontology_enrichment_line_class> enrich_list = new List<Ontology_enrichment_line_class>();
            Ontology_enrichment_line_class[] add_enrich;
            Entry_type_enum entryType;
            int timepoint;
            string sample_name;
            for (int indexCol = 0; indexCol < col_length; indexCol++)
            {
                entryType = data.ColChar.Columns[indexCol].EntryType;
                timepoint = data.ColChar.Columns[indexCol].Timepoint;
                sample_name = (string)data.ColChar.Columns[indexCol].SampleName.Clone();
                symbols = data.Get_alphabetically_ordered_ncbi_official_symbols__with_non_empty_entries_in_indicated_column(indexCol);
                add_enrich = Generate_enrichment_lines_and_calculate_pvalues(symbols,entryType,timepoint,sample_name);
                add_enrich = Add_missing_process_information(add_enrich);
                add_enrich = Add_upregulated_downregulated_symbol_information(add_enrich, data, indexCol);
                enrich_list.AddRange(add_enrich);
            }
            Ontology_enrichment_line_class[] enrich_array = Add_missing_process_information(enrich_list.ToArray());
            Ontology_enrichment_class enrich = new Ontology_enrichment_class();
            enrich.Add_other_lines(enrich_array);
            return enrich;
        }
        #endregion

        #region Copy
        public Ontology_fisher_class Deep_copy()
        {
            Ontology_fisher_class copy = (Ontology_fisher_class)this.MemberwiseClone();
            copy.MBCO_association = this.MBCO_association.Deep_copy();
            copy.Bg_genes = Array_class.Deep_copy_string_array(this.Bg_genes);
            return copy;
        }
        #endregion
    }
}
