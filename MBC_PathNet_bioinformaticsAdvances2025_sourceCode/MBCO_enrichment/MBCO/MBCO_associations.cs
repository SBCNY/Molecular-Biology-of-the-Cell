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
using ZedGraph;
using Windows_forms;
using Enrichment;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Drawing;
using Other_ontologies_and_databases;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using Gene_databases;

namespace MBCO
{
    class MBCO_association_line_class
    {
        #region Fields
        public int SCP_level { get; set; }
        public int SCP_depth { get; set; }
        public string SCP_id { get; set; }
        public string SCP_name { get; set; }
        public string Parent_scpName { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string[] References { get; set; }
        public Manual_validation_enum Manual_validation { get; set; }
        #endregion

        #region Order
        public static MBCO_association_line_class[] Order_by_symbol_processName(MBCO_association_line_class[] association_lines)
        {
            Dictionary<string, Dictionary<string, List<MBCO_association_line_class>>> symbol_processName_dict = new Dictionary<string, Dictionary<string, List<MBCO_association_line_class>>>();
            Dictionary<string, List<MBCO_association_line_class>> processName_dict = new Dictionary<string, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            for (int indexL=0; indexL < lines_length;indexL++)
            {
                association_line = association_lines[indexL];
                if (!symbol_processName_dict.ContainsKey(association_line.Symbol))
                {
                    symbol_processName_dict.Add(association_line.Symbol, new Dictionary<string, List<MBCO_association_line_class>>());
                }
                if (!symbol_processName_dict[association_line.Symbol].ContainsKey(association_line.SCP_name))
                {
                    symbol_processName_dict[association_line.Symbol].Add(association_line.SCP_name, new List<MBCO_association_line_class>());
                }
                symbol_processName_dict[association_line.Symbol][association_line.SCP_name].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            string[] symbols = symbol_processName_dict.Keys.ToArray();
            string symbol;
            int symbols_length = symbols.Length;
            string[] processNames;
            string processName;
            int processNames_length;
            symbols = symbols.OrderBy(l => l).ToArray();
            symbols_length = symbols.Length;
            for (int indexS=0; indexS<symbols_length;indexS++)
            {
                symbol = symbols[indexS];
                processName_dict = symbol_processName_dict[symbol];
                processNames = processName_dict.Keys.ToArray();
                processNames = processNames.OrderBy(l=>l).ToArray();
                processNames_length = processNames.Length;
                for (int indexPN=0; indexPN<processNames_length;indexPN++)
                {
                    processName = processNames[indexPN];
                    ordered_lines.AddRange(processName_dict[processName]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                for (int indexO=1; indexO<lines_length;indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Symbol.CompareTo(previous_line.Symbol)<0) { throw new Exception(); }
                    else if (  (this_line.Symbol.Equals(previous_line.Symbol))
                             &&(this_line.SCP_name.CompareTo(previous_line.SCP_name)<0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static MBCO_association_line_class[] Order_by_symbol_descendingProcessLevel_processName(MBCO_association_line_class[] association_lines)
        {
            Dictionary<string, Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>>> symbol_processLevel_processName_dict = new Dictionary<string, Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>>>();
            Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>> processLevel_processName_dict = new Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>>();
            Dictionary<string, List<MBCO_association_line_class>> processName_dict = new Dictionary<string, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            for (int indexL = 0; indexL < lines_length; indexL++)
            {
                association_line = association_lines[indexL];
                if (!symbol_processLevel_processName_dict.ContainsKey(association_line.Symbol))
                {
                    symbol_processLevel_processName_dict.Add(association_line.Symbol, new Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>>());
                }
                if (!symbol_processLevel_processName_dict[association_line.Symbol].ContainsKey(association_line.SCP_level))
                {
                    symbol_processLevel_processName_dict[association_line.Symbol].Add(association_line.SCP_level, new Dictionary<string, List<MBCO_association_line_class>>());
                }
                if (!symbol_processLevel_processName_dict[association_line.Symbol][association_line.SCP_level].ContainsKey(association_line.SCP_name))
                {
                    symbol_processLevel_processName_dict[association_line.Symbol][association_line.SCP_level].Add(association_line.SCP_name, new List<MBCO_association_line_class>());
                }
                symbol_processLevel_processName_dict[association_line.Symbol][association_line.SCP_level][association_line.SCP_name].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            string[] symbols = symbol_processLevel_processName_dict.Keys.ToArray();
            string symbol;
            int symbols_length = symbols.Length;
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            string[] processNames;
            string processName;
            int processNames_length;
            symbols = symbols.OrderBy(l => l).ToArray();
            symbols_length = symbols.Length;
            for (int indexS = 0; indexS < symbols_length; indexS++)
            {
                symbol = symbols[indexS];
                processLevel_processName_dict = symbol_processLevel_processName_dict[symbol];
                processLevels = processLevel_processName_dict.Keys.ToArray();
                processLevels = processLevels.OrderByDescending(l => l).ToArray();
                processLevels_length = processLevels.Length;
                for (int indexPL=0;indexPL < processLevels_length;indexPL++)
                {
                    processLevel = processLevels[indexPL];
                    processName_dict = processLevel_processName_dict[processLevel];
                    processNames = processName_dict.Keys.ToArray();
                    processNames = processNames.OrderBy(l => l).ToArray();
                    processNames_length = processNames.Length;
                    for (int indexPN = 0; indexPN < processNames_length; indexPN++)
                    {
                        processName = processNames[indexPN];
                        ordered_lines.AddRange(processName_dict[processName]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                //symbol_descendingProcessLevel_processName
                for (int indexO = 1; indexO < lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Symbol.CompareTo(previous_line.Symbol) < 0) { throw new Exception(); }
                    else if ((this_line.Symbol.Equals(previous_line.Symbol))
                             && (this_line.SCP_level.CompareTo(previous_line.SCP_level) > 0)) { throw new Exception(); }
                    else if ((this_line.Symbol.Equals(previous_line.Symbol))
                             && (this_line.SCP_level.Equals(previous_line.SCP_level))
                             && (this_line.SCP_name.CompareTo(previous_line.SCP_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static MBCO_association_line_class[] Order_by_lengthOfSymbol_symbol(MBCO_association_line_class[] association_lines)
        {
            Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>> lengthOfSymbol_symbol_dict = new Dictionary<int, Dictionary<string, List<MBCO_association_line_class>>>();
            Dictionary<string, List<MBCO_association_line_class>> symbol_dict = new Dictionary<string, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            int lengthOfSymbol;
            for (int indexL = 0; indexL < lines_length; indexL++)
            {
                association_line = association_lines[indexL];
                lengthOfSymbol = association_line.Symbol.Length;
                association_line = association_lines[indexL];
                if (!lengthOfSymbol_symbol_dict.ContainsKey(lengthOfSymbol))
                {
                    lengthOfSymbol_symbol_dict.Add(lengthOfSymbol, new Dictionary<string, List<MBCO_association_line_class>>());
                }
                if (!lengthOfSymbol_symbol_dict[lengthOfSymbol].ContainsKey(association_line.Symbol))
                {
                    lengthOfSymbol_symbol_dict[lengthOfSymbol].Add(association_line.Symbol, new List<MBCO_association_line_class>());
                }
                lengthOfSymbol_symbol_dict[lengthOfSymbol][association_line.Symbol].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            int[] lengthOfSymbol_array = lengthOfSymbol_symbol_dict.Keys.ToArray();
            int lengthOfSymbol_array_length;
            string[] symbols;
            string symbol;
            int symbols_length;
            lengthOfSymbol_array = lengthOfSymbol_array.OrderBy(l=>l).ToArray();
            lengthOfSymbol_array_length = lengthOfSymbol_array.Length;
            for (int indexLS = 0; indexLS < lengthOfSymbol_array_length; indexLS++)
            {
                lengthOfSymbol = lengthOfSymbol_array[indexLS];
                symbol_dict = lengthOfSymbol_symbol_dict[lengthOfSymbol];
                symbols = symbol_dict.Keys.ToArray();
                symbols = symbols.OrderBy(l => l).ToArray();
                symbols_length = symbols.Length;
                for (int indexS = 0; indexS < symbols_length; indexS++)
                {
                    symbol = symbols[indexS];
                    ordered_lines.AddRange(symbol_dict[symbol]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                int this_lengthOfSymbol;
                int previous_lengthOfSymbol;
                for (int indexO = 1; indexO < lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_lengthOfSymbol = this_line.Symbol.Length;
                    previous_lengthOfSymbol = previous_line.Symbol.Length;
                    if (this_lengthOfSymbol.CompareTo(previous_lengthOfSymbol) < 0) { throw new Exception(); }
                    else if ((this_lengthOfSymbol.Equals(previous_lengthOfSymbol))
                             && (this_line.Symbol.CompareTo(previous_line.Symbol) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static MBCO_association_line_class[] Order_by_processId(MBCO_association_line_class[] association_lines)
        {
            Dictionary<string, List<MBCO_association_line_class>> processID_dict = new Dictionary<string, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            for (int indexL = 0; indexL < lines_length; indexL++)
            {
                association_line = association_lines[indexL];
                if (!processID_dict.ContainsKey(association_line.SCP_id))
                {
                    processID_dict.Add(association_line.SCP_id, new List<MBCO_association_line_class>());
                }
                processID_dict[association_line.SCP_id].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            string[] processIDs = processID_dict.Keys.ToArray();
            string processID;
            int processIDs_length = processIDs.Length;
            processIDs = processIDs.OrderBy(l => l).ToArray();
            for (int indexPID = 0; indexPID < processIDs_length; indexPID++)
            {
                processID = processIDs[indexPID];
                ordered_lines.AddRange(processID_dict[processID]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                for (int indexO = 1; indexO < lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.SCP_id.CompareTo(previous_line.SCP_id) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static MBCO_association_line_class[] Order_by_processName(MBCO_association_line_class[] association_lines)
        {
            Dictionary<string, List<MBCO_association_line_class>> processName_dict = new Dictionary<string, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            for (int indexL = 0; indexL < lines_length; indexL++)
            {
                association_line = association_lines[indexL];
                if (!processName_dict.ContainsKey(association_line.SCP_name))
                {
                    processName_dict.Add(association_line.SCP_name, new List<MBCO_association_line_class>());
                }
                processName_dict[association_line.SCP_name].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            string[] processNames = processName_dict.Keys.ToArray();
            string processName;
            int processNames_length = processNames.Length;
            processNames = processNames.OrderBy(l => l).ToArray();
            for (int indexPN = 0; indexPN < processNames_length; indexPN++)
            {
                processName = processNames[indexPN];
                ordered_lines.AddRange(processName_dict[processName]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                for (int indexO = 1; indexO < lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.SCP_name.CompareTo(previous_line.SCP_name) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static MBCO_association_line_class[] Order_by_processLevel(MBCO_association_line_class[] association_lines)
        {
            Dictionary<int, List<MBCO_association_line_class>> processLevel_dict = new Dictionary<int, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            for (int indexL = 0; indexL < lines_length; indexL++)
            {
                association_line = association_lines[indexL];
                if (!processLevel_dict.ContainsKey(association_line.SCP_level))
                {
                    processLevel_dict.Add(association_line.SCP_level, new List<MBCO_association_line_class>());
                }
                processLevel_dict[association_line.SCP_level].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            int[] processLevels = processLevel_dict.Keys.ToArray();
            int processLevel;
            int processLevels_length = processLevels.Length;
            processLevels = processLevels.OrderBy(l => l).ToArray();
            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
            {
                processLevel = processLevels[indexPL];
                ordered_lines.AddRange(processLevel_dict[processLevel]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                for (int indexO = 1; indexO < lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.SCP_level.CompareTo(previous_line.SCP_level) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static MBCO_association_line_class[] Order_by_processName_symbol(MBCO_association_line_class[] association_lines)
        {
            Dictionary<string, Dictionary<string, List<MBCO_association_line_class>>> processName_symbol_dict = new Dictionary<string, Dictionary<string, List<MBCO_association_line_class>>>();
            Dictionary<string, List<MBCO_association_line_class>> symbol_dict = new Dictionary<string, List<MBCO_association_line_class>>();
            MBCO_association_line_class association_line;
            int lines_length = association_lines.Length;
            for (int indexL = 0; indexL < lines_length; indexL++)
            {
                association_line = association_lines[indexL];
                if (!processName_symbol_dict.ContainsKey(association_line.SCP_name))
                {
                    processName_symbol_dict.Add(association_line.SCP_name, new Dictionary<string, List<MBCO_association_line_class>>());
                }
                if (!processName_symbol_dict[association_line.SCP_name].ContainsKey(association_line.Symbol))
                {
                    processName_symbol_dict[association_line.SCP_name].Add(association_line.Symbol, new List<MBCO_association_line_class>());
                }
                processName_symbol_dict[association_line.SCP_name][association_line.Symbol].Add(association_line);
            }
            association_lines = null;
            List<MBCO_association_line_class> ordered_lines = new List<MBCO_association_line_class>();
            string[] processNames = processName_symbol_dict.Keys.ToArray();
            string processName;
            int processNames_length = processNames.Length;
            string[] symbols;
            string symbol;
            int symbols_length;
            processNames = processNames.OrderBy(l => l).ToArray();
            processNames_length = processNames.Length;
            for (int indexPN = 0; indexPN < processNames_length; indexPN++)
            {
                processName = processNames[indexPN];
                symbol_dict = processName_symbol_dict[processName];
                symbols = symbol_dict.Keys.ToArray();
                symbols = symbols.OrderBy(l => l).ToArray();
                symbols_length = symbols.Length;
                for (int indexS = 0; indexS < symbols_length; indexS++)
                {
                    symbol = symbols[indexS];
                    ordered_lines.AddRange(symbol_dict[symbol]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != lines_length) { throw new Exception(); }
                MBCO_association_line_class this_line;
                MBCO_association_line_class previous_line;
                for (int indexO = 1; indexO < lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.SCP_name.CompareTo(previous_line.SCP_name) < 0) { throw new Exception(); }
                    else if ((this_line.SCP_name.Equals(previous_line.SCP_name))
                             && (this_line.Symbol.CompareTo(previous_line.Symbol) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        #endregion



        public MBCO_association_line_class()
        {
            SCP_level = -1;
            SCP_depth = -1;
            SCP_id = Global_class.Empty_entry;
            SCP_name = Global_class.Empty_entry;
            Parent_scpName = Global_class.Empty_entry;
            Description = "";
            References = new string[0];
        }

        public MBCO_association_line_class Deep_copy()
        {
            MBCO_association_line_class copy = (MBCO_association_line_class)this.MemberwiseClone();
            copy.SCP_id = (string)this.SCP_id.Clone();
            copy.SCP_name = (string)this.SCP_name.Clone();
            copy.Parent_scpName = (string)this.Parent_scpName.Clone();
            copy.Symbol = (string)this.Symbol.Clone();
            copy.Description = (string)this.Description.Clone();
            copy.References = Array_class.Deep_copy_string_array(this.References);
            return copy;
        }
    }

    class MBCO_appGenerated_association_readOptions_class : ReadWriteOptions_base
    {
        public static char Array_delimiter { get { return ';'; } }

        public MBCO_appGenerated_association_readOptions_class(string complete_fileName)
        {
            this.File = complete_fileName;
            Key_propertyNames = new string[] { "SCP_level", "SCP_depth","SCP_id", "SCP_name", "Symbol" };
            Key_columnNames = Key_propertyNames;
            this.File_has_headline = true;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_download_association_mbco_readOptions_class : ReadWriteOptions_base
    {
        public static char Array_delimiter { get { return ';'; } }

        public MBCO_download_association_mbco_readOptions_class(Ontology_type_enum ontology, Organism_enum organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Ontology_inputDirectory_dict[ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[ontology][organism];
            Key_propertyNames = new string[] { "SCP_level", "SCP_id", "SCP_name", "Symbol" };
            Key_columnNames = new string[] { "ProcessLevel", "ProcessID", "ProcessName", "Symbol" }; ;
            this.File_has_headline = true;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_download_association_go_readOptions_class : ReadWriteOptions_base
    {
        public static char Array_delimiter { get { return ';'; } }

        public MBCO_download_association_go_readOptions_class(Ontology_type_enum ontology, Organism_enum organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Ontology_inputDirectory_dict[ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[ontology][organism];
            Key_propertyNames = new string[] { "Symbol", "SCP_id" };
            Key_columnIndexes = new int[] { 2, 4 };
            this.File_has_headline = false;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_download_association_standardInput_readOptions_class : ReadWriteOptions_base
    {
        public MBCO_download_association_standardInput_readOptions_class(Ontology_type_enum ontology, Organism_enum organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Ontology_inputDirectory_dict[ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[ontology][organism];
            this.Key_propertyNames = new string[] { "SCP_name", "Symbol" };
            this.Key_columnNames = new string[] { "Scp", "Symbol" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_download_association_specialMbcoDatasetInput_readOptions_class : ReadWriteOptions_base
    {
        public MBCO_download_association_specialMbcoDatasetInput_readOptions_class(Ontology_type_enum ontology, Organism_enum organism)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            this.File = gdf.Ontology_inputDirectory_dict[ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[ontology][organism];
            this.Key_propertyNames = new string[] { "SCP_level","Parent_scpName","SCP_id","SCP_name","Symbol" };
            this.Key_columnNames = new string[] { "ProcessLevel", "Parent_processName", "ProcessID", "ProcessName", "Symbol" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_main;
        }
    }

    class MBCO_association_class
    {
        public MBCO_association_line_class[] MBCO_associations { get; set; }
        public Ontology_type_enum Ontology { get; private set; }
        public Organism_enum Organism { get; private set; }

        public MBCO_association_class()
        {
            Clear_data();
        }

        public void Check_for_dupplicated_scp_symbol_associations()
        {
            int mbco_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            Dictionary<string, Dictionary<string, bool>> scp_symbol_dict = new Dictionary<string, Dictionary<string, bool>>();
            for (int indexMBCO = 0; indexMBCO < mbco_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (!scp_symbol_dict.ContainsKey(mbco_association_line.SCP_name))
                {
                    scp_symbol_dict.Add(mbco_association_line.SCP_name, new Dictionary<string, bool>());
                }
                scp_symbol_dict[mbco_association_line.SCP_name].Add(mbco_association_line.Symbol, true);
            }
        }
        public void Clear_data()
        {
            this.MBCO_associations = new MBCO_association_line_class[0];
            Organism = Organism_enum.E_m_p_t_y;
            Ontology = Ontology_type_enum.E_m_p_t_y;
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
            MBCO_association_line_class mbco_association_line;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            Dictionary<string, Dictionary<string, bool>> scp_symbol_dict = new Dictionary<string, Dictionary<string, bool>>();
            Dictionary<string, string> scp_scpId_dict = new Dictionary<string, string>();
            for (int indexMBCO = 0; indexMBCO < mbco_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (!scp_symbol_dict.ContainsKey(mbco_association_line.SCP_name))
                {
                    scp_symbol_dict.Add(mbco_association_line.SCP_name, new Dictionary<string, bool>());
                    scp_scpId_dict.Add(mbco_association_line.SCP_name, mbco_association_line.SCP_id);
                }
                else if (!scp_scpId_dict[mbco_association_line.SCP_name].Equals(mbco_association_line.SCP_id)) { throw new Exception(); }
                if (!scp_symbol_dict[mbco_association_line.SCP_name].ContainsKey(mbco_association_line.Symbol))
                {
                    scp_symbol_dict[mbco_association_line.SCP_name].Add(mbco_association_line.Symbol, true);
                    keep.Add(mbco_association_line);
                }
            }
            this.MBCO_associations = keep.ToArray();
        }
        private void Add_missing_processNames_from_parent_child_nw_or_check_if_equal(MBCO_obo_network_class mbco_parentChild_nw)
        {
            Dictionary<string, string> processID_processName_dict = mbco_parentChild_nw.Get_processID_processName_dictionary();
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                if ((String.IsNullOrEmpty(mbco_association_line.SCP_name))
                    || (mbco_association_line.SCP_name.Equals(Global_class.Empty_entry)))
                {
                    mbco_association_line.SCP_name = (string)processID_processName_dict[mbco_association_line.SCP_id].Clone();
                }
                else if (mbco_association_line.SCP_name.Equals(Ontology_classification_class.Background_genes_scp)) { }
                else if (!mbco_association_line.SCP_name.Equals(processID_processName_dict[mbco_association_line.SCP_id]))
                {
                    throw new Exception();
                }
            }
        }
        private void Set_level_and_depth_for_nonMBCO_ontologies(ProgressReport_interface_class progressReport)
        {
            MBCO_obo_network_class parent_child_network = new MBCO_obo_network_class(Ontology, SCP_hierarchy_interaction_type_enum.Parent_child, Organism);
            parent_child_network.Generate_by_reading_safed_spreadsheet_file_or_obo_file_and_return_if_finalized(progressReport, out bool not_interrupted);
            if ((Global_class.Do_internal_checks) && (!not_interrupted)) { throw new Exception(); }
            Dictionary<string, int> pathway_level_dict = parent_child_network.Get_processName_level_dictionary_without_setting_process_level();
            Dictionary<string, int> pathway_depth_dict = parent_child_network.Get_processName_depth_dictionary_without_setting_process_level();
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                if (!mbco_association_line.SCP_name.Equals("Background genes"))
                {
                    mbco_association_line.SCP_level = pathway_level_dict[mbco_association_line.SCP_name];
                    mbco_association_line.SCP_depth = pathway_depth_dict[mbco_association_line.SCP_name];
                }
                else
                {
                    mbco_association_line.SCP_level = -1;
                    mbco_association_line.SCP_depth = -1;
                }
            }
        }
        private void Set_depth_for_MBCO()
        {
            foreach (MBCO_association_line_class mbco_association_line in this.MBCO_associations)
            {
                mbco_association_line.SCP_depth = mbco_association_line.SCP_level;
            }
        }
        private void Keep_only_scps_of_correct_go_namespace_before_application_of_size_cutoffs(MBCO_obo_network_class mbco_parentChild_nw_for_population)
        {
            if (!mbco_parentChild_nw_for_population.Scp_hierarchal_interactions.Equals(SCP_hierarchy_interaction_type_enum.Parent_child)) { throw new Exception(); }
            if (!mbco_parentChild_nw_for_population.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }
            Dictionary<string, Namespace_type_enum> scp_namespace_dict = mbco_parentChild_nw_for_population.Get_processName_namespace_dictionary();
            Namespace_type_enum keep_namespace = Namespace_type_enum.E_m_p_t_y;
            switch (this.Ontology)
            {
                case Ontology_type_enum.Go_bp:
                    keep_namespace = Namespace_type_enum.Biological_process;
                    break;
                case Ontology_type_enum.Go_mf:
                    keep_namespace = Namespace_type_enum.Molecular_function;
                    break;
                case Ontology_type_enum.Go_cc:
                    keep_namespace = Namespace_type_enum.Cellular_component;
                    break;
                default:
                    throw new Exception();
            }

            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            foreach (MBCO_association_line_class association_line in this.MBCO_associations)
            {
                if (scp_namespace_dict[association_line.SCP_name].Equals(keep_namespace))
                {
                    keep.Add(association_line);
                }
                else if (association_line.SCP_name.Equals(Ontology_classification_class.Background_genes_scp))
                {
                    throw new Exception();
                }
            }
            this.MBCO_associations = keep.ToArray();
        }
        public void Populate_parent_scps_with_genes_of_children_scps_for_all_three_namespaces(MBCO_obo_network_class mbco_parent_child_nw)
        {
            if (!mbco_parent_child_nw.Scp_hierarchal_interactions.Equals(SCP_hierarchy_interaction_type_enum.Parent_child)) { throw new Exception(); }
            if (!mbco_parent_child_nw.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }

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
                scp_genes_dict[mbco_association_line.SCP_name][mbco_association_line.Symbol] = false;
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
                        new_mbco_association_line.SCP_name = (string)scp.Clone();
                        new_mbco_association_line.SCP_id = scp_scpId_dict[new_mbco_association_line.SCP_name];
                        add_mbco_association_lines.Add(new_mbco_association_line);
                    }
                }
            }
            this.Add_to_array(add_mbco_association_lines.ToArray());
            Check_for_dupplicated_scp_symbol_associations();
        }
        private void Test_population_of_parent_scps_with_children_genes(MBCO_obo_network_class mbco_parent_child_nw_input, ProgressReport_interface_class progressReport)
        {
            progressReport.Update_progressReport_text_and_visualization("Test population of parent pathways with genes of their children");
            MBCO_obo_network_class mbco_parent_child_nw = mbco_parent_child_nw_input.Deep_copy_mbco_obo_nw();
            mbco_parent_child_nw.Nodes.Order_by_nw_index();
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

            for (int indexTest=0; indexTest<scps.Length; indexTest++)
            {
                //indexScp = random.Next(scps_length);
                indexScp = indexTest;
                parent_scp = scps[indexScp];
                child_scps = mbco_parent_child_nw.Get_all_children_if_direction_is_parent_child_without_ordering_nodes_by_index(parent_scp);
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
            progressReport.Update_progressReport_text_and_visualization("Test population of parent pathways with genes of their children.\r\nNo errors found");
        }

        private void Generate_from_reactome_download_write_and_assign_correct_organism(Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            progressReport.Update_progressReport_text_and_visualization(Ontology_classification_class.Get_loading_and_preparing_report_for_enrichment(this.Ontology));

            Reactome_library_class reactome = new Reactome_library_class();
            reactome.Generate_organism_selective_by_reading(this.Ontology, organism, progressReport);

            MBCO_association_class mbco_association = new MBCO_association_class();
            mbco_association.Ontology = Ontology_type_enum.Reactome;
            mbco_association.Organism = organism;
            int reactome_length = reactome.Reactome.Length;
            mbco_association.MBCO_associations = new MBCO_association_line_class[reactome_length];
            Reactome_library_line_class reactome_line;
            MBCO_association_line_class mbc_association_line;
            for (int indexR = 0; indexR < reactome_length; indexR++)
            {
                reactome_line = reactome.Reactome[indexR];
                mbc_association_line = new MBCO_association_line_class();
                mbc_association_line.SCP_id = (string)reactome_line.Reactome_pathway_id.Clone();
                mbc_association_line.SCP_name = (string)reactome_line.Pathway.Clone();
                mbc_association_line.Symbol = (string)reactome_line.Entity.Clone();
                mbco_association.MBCO_associations[indexR] = mbc_association_line;
            }
            mbco_association.Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
            mbco_association.Set_level_and_depth_for_nonMBCO_ontologies(progressReport);
            if (mbco_association.Ontology.Equals(this.Ontology))
            {
                int mbco_associations_length = mbco_association.MBCO_associations.Length;
                this.MBCO_associations = new MBCO_association_line_class[mbco_associations_length];
                for (int indexMBCO=0; indexMBCO<mbco_associations_length; indexMBCO++)
                {
                    this.MBCO_associations[indexMBCO] = mbco_association.MBCO_associations[indexMBCO].Deep_copy();
                }
            }
        }


        private void Keep_only_scps_meeting_goOntologyHyperparameter_and_add_removed_genes_as_bgGenes(Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum,int>> ontology_goHyperparameter_cutoff_dict, MBCO_obo_network_class go_hierarchy)
        {
            if (ontology_goHyperparameter_cutoff_dict.ContainsKey(Ontology))
            {
                int max_genes_each_scp = ontology_goHyperparameter_cutoff_dict[Ontology][GO_hyperParameter_enum.Max_size];//250
                int min_genes_each_scp = ontology_goHyperparameter_cutoff_dict[Ontology][GO_hyperParameter_enum.Min_size];//5
                int max_level = -1;// ontology_goHyperparameter_cutoff_dict[Ontology][GO_hyperParameter_enum.Max_level];//5
                int min_level = -1;//ontology_goHyperparameter_cutoff_dict[Ontology][GO_hyperParameter_enum.Min_level];//5
                int max_depth = -1;//ontology_goHyperparameter_cutoff_dict[Ontology][GO_hyperParameter_enum.Max_depth];//5
                int min_depth = -1;//ontology_goHyperparameter_cutoff_dict[Ontology][GO_hyperParameter_enum.Min_depth];//5

                Dictionary<string, int> name_depth_dict = go_hierarchy.Nodes.Get_name_depth_dictionary();
                Dictionary<string, int> name_level_dict = go_hierarchy.Nodes.Get_name_level_dictionary();

                Dictionary<string, Namespace_type_enum> processName_namespace_dict = go_hierarchy.Get_processName_namespace_dictionary();


                List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
                List<MBCO_association_line_class> current_scp = new List<MBCO_association_line_class>();
                int mbco_associations_length = this.MBCO_associations.Length;
                //this.MBCO_associations = this.MBCO_associations.OrderBy(l => l.ProcessName).ToArray();
                this.MBCO_associations = MBCO_association_line_class.Order_by_processName(this.MBCO_associations);
                MBCO_association_line_class mbco_association_line;
                Dictionary<string, bool> bgGene_dict = new Dictionary<string, bool>();
                Namespace_type_enum ontology_namespace = processName_namespace_dict[this.MBCO_associations[0].SCP_name];
                for (int indexMBCO = 0; indexMBCO < mbco_associations_length; indexMBCO++)
                {
                    mbco_association_line = this.MBCO_associations[indexMBCO];
                    if (!processName_namespace_dict[mbco_association_line.SCP_name].Equals(ontology_namespace)) { throw new Exception(); }
                    if (!bgGene_dict.ContainsKey(mbco_association_line.Symbol))
                    {
                        bgGene_dict.Add(mbco_association_line.Symbol, true);
                    }
                    if ((indexMBCO == 0)
                        || (!mbco_association_line.SCP_name.Equals(this.MBCO_associations[indexMBCO - 1].SCP_name)))
                    {
                        current_scp.Clear();
                    }
                    current_scp.Add(mbco_association_line);
                    if ((indexMBCO == mbco_associations_length - 1)
                        || (!mbco_association_line.SCP_name.Equals(this.MBCO_associations[indexMBCO + 1].SCP_name)))
                    {
                        if (((min_genes_each_scp <= 0) || (current_scp.Count >= min_genes_each_scp))
                            && ((max_genes_each_scp <= 0) || (current_scp.Count <= max_genes_each_scp))
                            && ((max_depth <= 0) || (name_depth_dict[mbco_association_line.SCP_name] <= max_depth))
                            && ((min_depth <= 0) || (name_depth_dict[mbco_association_line.SCP_name] >= min_depth))
                            && ((max_level <= 0) || (name_level_dict[mbco_association_line.SCP_name] <= max_level))
                            && ((min_level <= 0) || (name_level_dict[mbco_association_line.SCP_name] >= min_level)))
                        {
                            keep.AddRange(current_scp);
                        }
                    }
                }
                int keep_count = keep.Count;
                for (int indexKeep = 0; indexKeep < keep_count; indexKeep++)
                {
                    mbco_association_line = keep[indexKeep];
                    bgGene_dict[mbco_association_line.Symbol] = false;
                }

                string[] bgGenes = bgGene_dict.Keys.ToArray();
                int readded_bgGenes_count = 0;
                foreach (string bgGene in bgGenes)
                {
                    if (bgGene_dict[bgGene])
                    {
                        mbco_association_line = new MBCO_association_line_class();
                        mbco_association_line.Symbol = (string)bgGene.Clone();
                        mbco_association_line.SCP_name = Ontology_classification_class.Background_genes_scp;
                        mbco_association_line.SCP_id = "GO:Background (not part of any kept process)";
                        keep.Add(mbco_association_line);
                        readded_bgGenes_count++;
                    }
                }
                this.MBCO_associations = keep.ToArray();
            }
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

        private void Replace_human_symbols_by_species_selective_symbols(ProgressReport_interface_class progressReport)
        {
            Orthologue_class orthologs = new Orthologue_class(Organism_enum.Homo_sapiens, this.Organism);
            orthologs.Generate(progressReport);
            Dictionary<string, string[]> humanGene_speciesOrthologs_dict = orthologs.Get_humanGene_speciesOrthologs_dict();
            List<MBCO_association_line_class> species_association_lines = new List<MBCO_association_line_class>();
            MBCO_association_line_class species_association_line;
            string[] ortholog_symbols;
            foreach (MBCO_association_line_class association_line in this.MBCO_associations)
            {
                if (humanGene_speciesOrthologs_dict.ContainsKey(association_line.Symbol))
                {
                    ortholog_symbols = humanGene_speciesOrthologs_dict[association_line.Symbol];
                    foreach (string ortholog_symbol in ortholog_symbols)
                    {
                        species_association_line = association_line.Deep_copy();
                        species_association_line.Symbol = (string)ortholog_symbol.Clone();
                        species_association_lines.Add(species_association_line);
                    }
                }
            }
            this.MBCO_associations = species_association_lines.ToArray();
            Remove_dupplicated_scp_symbol_associations();
        }


        public void Generate_after_reading_safed_file_or_de_novo_and_save(Ontology_type_enum ontology, Organism_enum organism, 
                                                                          Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum,int>> ontology_goHyperparameter_cutoff_dict,
                                                                          ProgressReport_interface_class progressReport)
        {
            this.Ontology = ontology;
            this.Organism = organism;
            Organism_enum download_organism = organism;
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            switch (gdf.Ontology_species_selectionOrder_dict[this.Ontology][this.Organism])
            {
                case Species_selection_order_enum.Insist_on_file:
                    download_organism = this.Organism;
                    break;
                case Species_selection_order_enum.Always_generate_orthologues:
                    download_organism = Organism_enum.Homo_sapiens;
                    break;
                case Species_selection_order_enum.Generate_orthologues_if_missing:
                    string complete_fileName = gdf.Ontology_inputDirectory_dict[this.Ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[this.Ontology][this.Organism];
                    if (File.Exists(complete_fileName))
                    {
                        download_organism = this.Organism;
                    }
                    else
                    {
                        download_organism = Organism_enum.Homo_sapiens;
                    }
                    break;
                default:
                    throw new Exception();
            }
            string complete_ontologyParentsPopulatedCutoffApplied_fileName = gdf.Get_appGenerated_complete_ontologyAssociationPopulatedParentsAppliedCutoff_fileName(ontology, this.Organism, ontology_goHyperparameter_cutoff_dict);
            string complete_ontologyParentsPopulatedNoCutoff_fileName = gdf.Get_appGenerated_complete_ontologyAssociationPopulatedParentsWithoutCutoff_fileName(ontology, this.Organism);
            bool stop_preparations = false;
            bool fileName_opened_successfully = true;
            if ((!File.Exists(complete_ontologyParentsPopulatedNoCutoff_fileName))&&(!stop_preparations))
            {
                MBCO_obo_network_class mbco_parentChild_nw_for_population;
                mbco_parentChild_nw_for_population = new MBCO_obo_network_class(this.Ontology, SCP_hierarchy_interaction_type_enum.Parent_child, this.Organism);
                mbco_parentChild_nw_for_population.Generate_by_reading_safed_spreadsheet_file_or_obo_file_and_return_if_finalized(progressReport, out bool not_interrupted);
                if (!not_interrupted) {  stop_preparations=true; }
                else
                {
                    switch (Ontology)
                    {
                        case Ontology_type_enum.Mbco:
                            Read_downnload_mbco_associations(ontology, download_organism, progressReport);
                            if (!download_organism.Equals(this.Organism)) { Replace_human_symbols_by_species_selective_symbols(progressReport); }
                            Set_depth_for_MBCO();
                            Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                            Write_appGenerated_ontology_associations(complete_ontologyParentsPopulatedNoCutoff_fileName, progressReport, out fileName_opened_successfully);
                            break;
                        case Ontology_type_enum.Reactome:
                            Generate_from_reactome_download_write_and_assign_correct_organism(download_organism, progressReport);
                            if (!download_organism.Equals(this.Organism)) { Replace_human_symbols_by_species_selective_symbols(progressReport); }
                            Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                            Set_level_and_depth_for_nonMBCO_ontologies(progressReport);
                            Write_appGenerated_ontology_associations(complete_ontologyParentsPopulatedNoCutoff_fileName, progressReport, out fileName_opened_successfully);
                            break;
                        case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                            Read_download_specialMbcoDatasetInput_gene_scp_association(ontology, download_organism, progressReport);
                            if (!download_organism.Equals(this.Organism)) { Replace_human_symbols_by_species_selective_symbols(progressReport); }
                            Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                            Set_level_and_depth_for_nonMBCO_ontologies(progressReport);
                            Write_appGenerated_ontology_associations(complete_ontologyParentsPopulatedNoCutoff_fileName, progressReport, out fileName_opened_successfully);
                            break;
                        case Ontology_type_enum.Custom_1:
                        case Ontology_type_enum.Custom_2:
                            Read_download_standardInput_gene_scp_association_and_fill_ids_with_names(ontology, download_organism, progressReport);
                            if (!download_organism.Equals(this.Organism)) { Replace_human_symbols_by_species_selective_symbols(progressReport); }
                            Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                            Set_level_and_depth_for_nonMBCO_ontologies(progressReport);
                            Write_appGenerated_ontology_associations(complete_ontologyParentsPopulatedNoCutoff_fileName, progressReport, out fileName_opened_successfully);
                            break;
                        case Ontology_type_enum.Go_bp:
                        case Ontology_type_enum.Go_cc:
                        case Ontology_type_enum.Go_mf:
                            progressReport.Update_progressReport_text_and_visualization(Ontology_classification_class.Get_loading_and_preparing_report_for_enrichment(ontology));
                            Read_go_associations_downloaded(ontology, download_organism, progressReport);
                            if (!download_organism.Equals(this.Organism)) { Replace_human_symbols_by_species_selective_symbols(progressReport); }
                            Set_all_genesToUpperCase_and_remove_empty_geneSymbols();
                            Add_missing_processNames_from_parent_child_nw_or_check_if_equal(mbco_parentChild_nw_for_population);
                            Remove_dupplicated_scp_symbol_associations();
                            Populate_parent_scps_with_genes_of_children_scps_for_all_three_namespaces(mbco_parentChild_nw_for_population);
                            Remove_dupplicated_scp_symbol_associations();
                            if (Global_class.Check_population_of_parents_with_children_genes) { Test_population_of_parent_scps_with_children_genes(mbco_parentChild_nw_for_population, progressReport); }
                            Set_level_and_depth_for_nonMBCO_ontologies(progressReport);
                            Namespace_type_enum[] namespace_types = mbco_parentChild_nw_for_population.Nodes.Get_all_ordered_namespaces();
                            Ontology_type_enum namespace_ontology;
                            string[] go_terms;
                            string local_complete_finalOntologyParentsPopulatedNoCutoff_fileName;
                            foreach (Namespace_type_enum namespace_type in namespace_types)
                            {
                                if (!namespace_type.Equals(Namespace_type_enum.Go_overall_parent))
                                {
                                    go_terms = mbco_parentChild_nw_for_population.Nodes.Get_all_ordered_nodeNames_of_indicated_namespace(namespace_type);
                                    MBCO_association_class namespace_mbco = this.Deep_copy();
                                    namespace_mbco.Keep_only_indicated_scps(go_terms);
                                    namespace_ontology = Ontology_classification_class.Get_ontology_of_namespace(namespace_type);
                                    local_complete_finalOntologyParentsPopulatedNoCutoff_fileName = gdf.Get_appGenerated_complete_ontologyAssociationPopulatedParentsWithoutCutoff_fileName(namespace_ontology, this.Organism);
                                    namespace_mbco.Write_appGenerated_ontology_associations(local_complete_finalOntologyParentsPopulatedNoCutoff_fileName, progressReport, out fileName_opened_successfully);
                                    //Write without applied filter
                                }
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                    if ((this.MBCO_associations.Length == 0)||(!fileName_opened_successfully)) { stop_preparations = true; }
                    Clear_data();
                }
            }
            this.Ontology = ontology; //reset after clear data
            this.Organism = organism;
            if ((!File.Exists(complete_ontologyParentsPopulatedCutoffApplied_fileName))&&(!stop_preparations))
            {
                Read_appGenerated_ontology_associations(complete_ontologyParentsPopulatedNoCutoff_fileName, progressReport);
                if (ontology_goHyperparameter_cutoff_dict.ContainsKey(ontology))
                {
                    MBCO_obo_network_class mbco_parentChild_nw_for_population;
                    mbco_parentChild_nw_for_population = new MBCO_obo_network_class(this.Ontology, SCP_hierarchy_interaction_type_enum.Parent_child, this.Organism);
                    mbco_parentChild_nw_for_population.Generate_by_reading_safed_spreadsheet_file_or_obo_file_and_return_if_finalized(progressReport, out bool not_interrupted);
                    Keep_only_scps_meeting_goOntologyHyperparameter_and_add_removed_genes_as_bgGenes(ontology_goHyperparameter_cutoff_dict, mbco_parentChild_nw_for_population);
                }
                Write_appGenerated_ontology_associations(complete_ontologyParentsPopulatedCutoffApplied_fileName, progressReport, out fileName_opened_successfully);
                Clear_data();
            }
            this.Ontology = ontology; //reset after clear data
            this.Organism = organism;
            if (!stop_preparations) { Read_appGenerated_ontology_associations(complete_ontologyParentsPopulatedCutoffApplied_fileName, progressReport); }
            if (!fileName_opened_successfully)
            {
                progressReport.Update_progressReport_text_and_visualization("App generated file could not be written. Is it in use? Is directory name too long?");
            }
        }

        #region Order
        public void Order_by_symbol_processName()
        {
            //MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol).ThenBy(l => l.ProcessName).ToArray();
            MBCO_associations = MBCO_association_line_class.Order_by_symbol_processName(MBCO_associations);
        }

        public void Order_by_symbol_descendingProcessLevel_processName()
        {
            //MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol).ThenByDescending(l=>l.ProcessLevel).ThenBy(l => l.ProcessName).ToArray();
            MBCO_associations = MBCO_association_line_class.Order_by_symbol_descendingProcessLevel_processName(MBCO_associations);
        }

        public void Order_by_lengthOfSymbol_symbol()
        {
            //MBCO_associations = MBCO_associations.OrderBy(l => l.Symbol.Length).ThenBy(l => l.Symbol).ToArray();
            MBCO_associations = MBCO_association_line_class.Order_by_lengthOfSymbol_symbol(MBCO_associations);
        }

        public void Order_by_processId()
        {
            //MBCO_associations = MBCO_associations.OrderBy(l => l.ProcessID).ToArray();
            MBCO_associations = MBCO_association_line_class.Order_by_processId(MBCO_associations);
        }

        public void Order_by_processName_symbol()
        {
            //MBCO_associations = MBCO_associations.OrderBy(l => l.ProcessName).ThenBy(l => l.Symbol).ToArray();
            MBCO_associations = MBCO_association_line_class.Order_by_processName_symbol(MBCO_associations);
        }

        public void Order_by_processLevel()
        {
            //this.MBCO_associations = this.MBCO_assbociations.OrderBy(l => l.ProcessLevel).ToArray();
            MBCO_associations = MBCO_association_line_class.Order_by_processLevel(MBCO_associations);
        }
        #endregion

        #region Get
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
            Dictionary<string, bool> scp_dict = new Dictionary<string, bool>();
            int onto_length = MBCO_associations.Length;
            MBCO_association_line_class onto_line;
            for (int indexOnto = 0; indexOnto < onto_length; indexOnto++)
            {
                onto_line = MBCO_associations[indexOnto];
                if (!scp_dict.ContainsKey(onto_line.SCP_name))
                {
                    scp_dict.Add(onto_line.SCP_name, true);
                }
            }
            return scp_dict.Keys.OrderBy(l => l).ToArray();
        }

        public Dictionary<string, int> Get_scp_level_dictionary()
        {
            Dictionary<string, int> scp_level_dict = new Dictionary<string, int>();
            int mbco_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMbco = 0; indexMbco < mbco_length; indexMbco++)
            {
                mbco_association_line = this.MBCO_associations[indexMbco];
                if (!scp_level_dict.ContainsKey(mbco_association_line.SCP_name)) { scp_level_dict.Add(mbco_association_line.SCP_name, mbco_association_line.SCP_level); }
            }
            return scp_level_dict;
        }

        public Dictionary<string, Dictionary<string, bool>> Get_scp_targetGene_dictionary()
        {
            Dictionary<string, Dictionary<string, bool>> scp_targetGene_dict = new Dictionary<string, Dictionary<string, bool>>();
            int mbco_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMbco = 0; indexMbco < mbco_length; indexMbco++)
            {
                mbco_association_line = this.MBCO_associations[indexMbco];
                if (!scp_targetGene_dict.ContainsKey(mbco_association_line.SCP_name)) { scp_targetGene_dict.Add(mbco_association_line.SCP_name, new Dictionary<string, bool>()); }
                scp_targetGene_dict[mbco_association_line.SCP_name].Add(mbco_association_line.Symbol, true);
            }
            return scp_targetGene_dict;
        }

        public Dictionary<string, int> Get_scp_scpSize_dictionary()
        {
            Dictionary<string, int> scp_scpSize_dict = new Dictionary<string, int>();
            int mbco_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            for (int indexMbco = 0; indexMbco < mbco_length; indexMbco++)
            {
                mbco_association_line = this.MBCO_associations[indexMbco];
                if (!scp_scpSize_dict.ContainsKey(mbco_association_line.SCP_name))
                {
                    scp_scpSize_dict.Add(mbco_association_line.SCP_name, 0);
                }
                scp_scpSize_dict[mbco_association_line.SCP_name]++;
            }
            return scp_scpSize_dict;
        }
        #endregion

        #region Keep, Remove
        public void Keep_only_bg_symbols(string[] bg_symbols)
        {
            Dictionary<string, bool> bgSymbol_dict = new Dictionary<string, bool>();
            foreach (string bgSymbol in bg_symbols)
            {
                if (!bgSymbol_dict.ContainsKey(bgSymbol))
                {
                    bgSymbol_dict.Add(bgSymbol, true);
                }
            }
            int mbco_associations_length = this.MBCO_associations.Length;
            MBCO_association_line_class mbco_association_line;
            List<MBCO_association_line_class> keep = new List<MBCO_association_line_class>();
            for (int indexMBCO = 0; indexMBCO < mbco_associations_length; indexMBCO++)
            {
                mbco_association_line = this.MBCO_associations[indexMBCO];
                if (bgSymbol_dict.ContainsKey(mbco_association_line.Symbol))
                {
                    keep.Add(mbco_association_line);
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
                if (keep_scps_dict.ContainsKey(mbco_association_line.SCP_name))
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
                if (levels.Contains(mbco_association_line.SCP_level))
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
                if (!mbco_association_line.SCP_name.Equals(Ontology_classification_class.Background_genes_scp))
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
                if (!mbco_line.SCP_id.Equals(Global_class.CustomScp_id))
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
                if (mbcoScps_custom_dict.ContainsKey(mbco_association_line.SCP_name))
                {
                    if (!mbcoScp_found_dict.ContainsKey(mbco_association_line.SCP_name)) { mbcoScp_found_dict.Add(mbco_association_line.SCP_name, true); }
                    new_scps = mbcoScps_custom_dict[mbco_association_line.SCP_name];
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
                            new_mbco_association_line.SCP_id = Global_class.CustomScp_id;
                            new_mbco_association_line.SCP_level = customSCP_level_dict[new_scp];
                            new_mbco_association_line.SCP_depth = new_mbco_association_line.SCP_level;
                            new_mbco_association_line.Manual_validation = Manual_validation_enum.Custom_gene_scp_association;
                            new_mbco_association_line.Parent_scpName = Global_class.CustomScp_id;
                            new_mbco_association_line.SCP_name = (string)new_scp.Clone();
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
        public void Read_download_standardInput_gene_scp_association_and_fill_ids_with_names(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            MBCO_download_association_standardInput_readOptions_class readOptions = new MBCO_download_association_standardInput_readOptions_class(ontology, organism);
            string shared_error_response = Ontology_classification_class.Get_pleaseDoubleCheck_file_messge(readOptions.File);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<MBCO_association_line_class>(readOptions, progressReport, shared_error_response);
            foreach (MBCO_association_line_class association_line in this.MBCO_associations)
            {
                association_line.SCP_id = (string)association_line.SCP_name.Clone();
            }
        }
        public void Read_download_specialMbcoDatasetInput_gene_scp_association(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            string shared_error_response = Ontology_classification_class.Get_pleaseDownloadMbcoPathNet_again_message();
            MBCO_download_association_specialMbcoDatasetInput_readOptions_class readOptions = new MBCO_download_association_specialMbcoDatasetInput_readOptions_class(ontology, organism);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<MBCO_association_line_class>(readOptions, progressReport, shared_error_response);
        }
        private void Read_downnload_mbco_associations(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            MBCO_download_association_mbco_readOptions_class readOptions = new MBCO_download_association_mbco_readOptions_class(ontology, organism);
            string shared_error_response = Ontology_classification_class.Get_pleaseDownloadMbcoPathNet_again_message();
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<MBCO_association_line_class>(readOptions, progressReport, shared_error_response);
        }
        private void Read_appGenerated_ontology_associations(string complete_fileName, ProgressReport_interface_class progressReport)
        {
            MBCO_appGenerated_association_readOptions_class readOptions = new MBCO_appGenerated_association_readOptions_class(complete_fileName);
            string shared_error_response = Ontology_classification_class.Get_pleaseDelete_file_message(readOptions.File);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<MBCO_association_line_class>(readOptions, progressReport, shared_error_response);
        }
        private void Write_appGenerated_ontology_associations(string complete_fileName, ProgressReport_interface_class progressReport, out bool fileName_written_successfully)
        {
            MBCO_appGenerated_association_readOptions_class readOptions = new MBCO_appGenerated_association_readOptions_class(complete_fileName);
            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(this.MBCO_associations, readOptions, progressReport, out fileName_written_successfully);
        }
        private void Read_go_associations_downloaded(Ontology_type_enum ontology, Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            MBCO_download_association_go_readOptions_class readOptions = new MBCO_download_association_go_readOptions_class(ontology, organism);
            string shared_error_message = Ontology_classification_class.Get_pleaseDonwload_file_again_message(readOptions.File);
            this.MBCO_associations = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<MBCO_association_line_class>(readOptions, progressReport, shared_error_message);
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

