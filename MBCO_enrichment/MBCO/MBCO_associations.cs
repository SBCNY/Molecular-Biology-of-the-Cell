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
using Common_functions.ReadWrite;
using Common_functions.Global_definitions;
using Common_functions.Report;

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
        #endregion

        public MBCO_association_line_class()
        {
            ProcessLevel = -1;
            ProcessID = Global_class.Empty_entry;
            ProcessName = Global_class.Empty_entry;
            Parent_processName = Global_class.Empty_entry;
        }

        public MBCO_association_line_class Deep_copy()
        {
            MBCO_association_line_class copy = (MBCO_association_line_class)this.MemberwiseClone();
            copy.ProcessID = (string)this.ProcessID.Clone();
            copy.ProcessName = (string)this.ProcessName.Clone();
            copy.Parent_processName = (string)this.Parent_processName.Clone();
            copy.Symbol = (string)this.Symbol.Clone();
            return copy;
        }
    }

    class MBCO_association_readOptions_class : ReadWriteOptions_base
    {
        public MBCO_association_readOptions_class()
        {
            this.File = Global_directory_and_file_class.Complete_mbco_association_fileName;
            Key_propertyNames = new string[] { "ProcessLevel", "Parent_processName", "ProcessID", "ProcessName", "Symbol" };
            Key_columnNames = Key_propertyNames;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_association_class
    {
        public MBCO_association_line_class[] MBCO_associations { get; set; }

        public MBCO_association_class()
        {
            MBCO_associations = new MBCO_association_line_class[0];
        }

        #region Generate
        public void Generate_by_reading_safed_file()
        {
            Read_mbco_associations();
        }

        public void Generate_de_novo_from_input_mbco_association_lines(MBCO_association_line_class[] input_mbco_association_lines)
        {
            int input_length = input_mbco_association_lines.Length;
            this.MBCO_associations = new MBCO_association_line_class[input_length];
            for (int indexMBCO = 0; indexMBCO < input_length; indexMBCO++)
            {
                this.MBCO_associations[indexMBCO] = input_mbco_association_lines[indexMBCO].Deep_copy();
            }
        }
        #endregion

        public void Add_to_array(MBCO_association_line_class[] add_mbco_associations)
        {
            int this_length = this.MBCO_associations.Length;
            int add_length = add_mbco_associations.Length;
            int new_length = this_length + add_length;
            MBCO_association_line_class[] new_mbco_assoiations = new MBCO_association_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_mbco_assoiations[indexNew] = this.MBCO_associations[indexThis];
            }
            for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_mbco_assoiations[indexNew] = add_mbco_associations[indexAdd];
            }
            this.MBCO_associations = new_mbco_assoiations;
        }

        #region Check
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
        #endregion

        #region Order
        public void Order_by_symbol_processName()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol).ThenBy(l=>l.ProcessName).ToArray();
        }

        public void Order_by_process_id()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.ProcessID).ToArray();
        }

        public void Order_by_processName_symbol()
        {
            MBCO_associations = MBCO_associations.OrderBy(l => l.ProcessName).ThenBy(l => l.Symbol).ToArray();
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
            this.Order_by_symbol_processName();
            int onto_length = MBCO_associations.Length;
            MBCO_association_line_class onto_line;
            List<string> all_distinct_ordered_symbols = new List<string>();
            for (int indexOnto = 0; indexOnto < onto_length; indexOnto++)
            {
                onto_line = MBCO_associations[indexOnto];
                if ((indexOnto == 0)
                    || (!onto_line.Symbol.Equals(MBCO_associations[indexOnto - 1].Symbol)))
                {
                    all_distinct_ordered_symbols.Add(onto_line.Symbol);
                }
            }
            return all_distinct_ordered_symbols.OrderBy(l => l).ToArray();
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

        #region Keep
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
        }

        public void Keep_only_lines_with_indicated_level(int level)
        {
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                if (mbco_association_line.ProcessLevel == level)
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

        public void Add_array_of_other_MBCO_association(MBCO_association_class other_mbco)
        {
            Add_to_array(other_mbco.MBCO_associations);
            Check_for_dupplicated_scp_symbol_associations();
        }

        #region Read write copy
        private void Read_mbco_associations()
        {
            MBCO_association_readOptions_class readOptions = new MBCO_association_readOptions_class();
            this.MBCO_associations = ReadWriteClass.ReadRawData_and_FillArray<MBCO_association_line_class>(readOptions);
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

