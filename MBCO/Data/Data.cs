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
using System.Text;
using System.IO;
using Common_functions.Global_definitions;

namespace Data
{
    interface IAdd_to_data
    {
        string NCBI_official_symbol_for_data { get; }
        Entry_type_enum EntryType_for_data { get; }
        int Timepoint_for_data { get; }
        string SampleName_for_data { get; }
        float Value_for_data { get; }
    }

    class IAdd_to_data_line_class : IAdd_to_data
    {
        public string NCBI_official_symbol_for_data { get; set; }
        public Entry_type_enum EntryType_for_data { get; set; }
        public int Timepoint_for_data { get; set; }
        public string SampleName_for_data { get; set; }
        public float Value_for_data { get; set; }
    }

    ///////////////////////////////////////////////////////////////

    class Colchar_column_line_class
    {
        #region Fields
        public Entry_type_enum EntryType { get; set; }
        public int Timepoint { get; set; }
        public string SampleName { get; set; }
        public string Full_column_name
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!EntryType.Equals(Entry_type_enum.E_m_p_t_y))
                {
                    if (sb.Length > 0) { sb.AppendFormat(Full_name_datalimiter.ToString()); }
                    sb.AppendFormat(EntryType.ToString());
                }
                if ((!String.IsNullOrEmpty(SampleName))
                    && (!SampleName.Equals(Global_class.Empty_entry)))
                {
                    if (sb.Length > 0) { sb.AppendFormat(Full_name_datalimiter.ToString()); }
                    sb.AppendFormat(SampleName);
                }
                return sb.ToString();
            }
        }
        public static char Full_name_datalimiter { get { return '-'; } }
        #endregion

        public Colchar_column_line_class()
        {
        }

        public Colchar_column_line_class(Entry_type_enum entryType, int timepoint, string sampleName)
        {
            this.EntryType = entryType;
            this.Timepoint = timepoint;
            SampleName = (string)sampleName.Clone();
        }

        #region Standard
        public static Colchar_column_line_class[] Order_in_standard_way(Colchar_column_line_class[] columns)
        {
            return columns.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ToArray();
        }

        public bool Equal_in_standard_way(IAdd_to_data other)
        {
            return ((this.EntryType.Equals(other.EntryType_for_data))
                    && (this.Timepoint.Equals(other.Timepoint_for_data))
                    && (this.SampleName.Equals(other.SampleName_for_data)));
        }

        public bool Equal_in_standard_way(Colchar_column_line_class other)
        {
            return ((this.EntryType.Equals(other.EntryType))
                    && (this.Timepoint.Equals(other.Timepoint))
                    && (this.SampleName.Equals(other.SampleName)));
        }

        public static bool Equal_in_standard_way(Colchar_column_line_class line1, Colchar_column_line_class line2)
        {
            return ((line1.EntryType.Equals(line2.EntryType))
                    && (line1.Timepoint.Equals(line2.Timepoint))
                    && (line1.SampleName.Equals(line2.SampleName)));
        }

        public static IAdd_to_data[] Order_in_standard_way(IAdd_to_data[] add_lines)
        {
            return add_lines.OrderBy(l => l.EntryType_for_data).ThenBy(l => l.Timepoint_for_data).ThenBy(l => l.SampleName_for_data).ToArray();
        }
        #endregion

        public Colchar_column_line_class Deep_copy()
        {
            Colchar_column_line_class copy = (Colchar_column_line_class)this.MemberwiseClone();
            copy.SampleName = (string)this.SampleName.Clone();
            return copy;
        }
    }

    class Colchar_class
    {
        #region Fields
        public Colchar_column_line_class[] Columns { get; set; }
        public int Columns_length { get { return Columns.Length; } }
        public bool Column_rearrangements_adopted { get; set; }
        #endregion

        public Colchar_class()
        {
            Column_rearrangements_adopted = true;
            Columns = new Colchar_column_line_class[0];
        }

        #region Check
        public void Correctness_check()
        {
            int col_length = Columns_length;
            Colchar_column_line_class[] columns_copy = Deep_copy_columns();
            columns_copy = Colchar_column_line_class.Order_in_standard_way(columns_copy);
            for (int indexC = 1; indexC < col_length; indexC++)
            {
                if (columns_copy[indexC].Equal_in_standard_way(columns_copy[indexC - 1]))
                {
                    throw new Exception("duplicated column charactarization");
                }
            }
            if (!Column_rearrangements_adopted)
            {
                throw new Exception("column rearrangements are not adopted");
            }
        }
        #endregion

        #region Order
        public int[] Order_columns_in_standard_way_and_get_old_new_index_array()
        {
            Correctness_check();
            Column_rearrangements_adopted = false;

            Colchar_column_line_class[] old_columns = Deep_copy_columns();
            Columns = Colchar_column_line_class.Order_in_standard_way(Columns);
            int columns_length = Columns.Length;

            #region Initialize index old new (and fill with -1 entries)
            int[] old_new_index_array = new int[columns_length];
            for (int indexOld = 0; indexOld < columns_length; indexOld++)
            {
                old_new_index_array[indexOld] = -1;
            }
            #endregion

            #region Fill index old new
            Colchar_column_line_class old_column;
            Colchar_column_line_class new_column;
            for (int indexOld = 0; indexOld < columns_length; indexOld++)
            {
                old_column = old_columns[indexOld];
                for (int indexNew = 0; indexNew < columns_length; indexNew++)
                {
                    new_column = Columns[indexNew];
                    if (old_column.Equal_in_standard_way(new_column))
                    {
                        old_new_index_array[indexOld] = indexNew;
                        break;
                    }
                }
            }
            #endregion

            return old_new_index_array;
        }
        #endregion

        #region Get
        public int Get_columnIndex(IAdd_to_data add_line)
        {
            int columns_length = Columns_length;
            Colchar_column_line_class column;
            int columnIndex = -1;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                column = Columns[indexC];
                if (column.Equal_in_standard_way(add_line))
                {
                    columnIndex = indexC;
                    break;
                }
            }
            if (columnIndex == -1) { throw new Exception("column does not exist"); }
            return columnIndex;
        }

        public int[] Get_indexes_of_input_column_characterization_lines(Colchar_column_line_class[] column_characterization_lines)
        {
            column_characterization_lines = column_characterization_lines.OrderBy(l => l.Timepoint).ThenBy(l => l.EntryType).ThenBy(l => l.SampleName).ToArray();
            int column_characterization_lines_length = column_characterization_lines.Length;
            //this.Columns = this.Columns.OrderBy(l => l.Timepoint).ThenBy(l => l.EntryType).ThenBy(l => l.Name).ToArray();
            int this_length = this.Columns_length;
            Colchar_column_line_class selected_col_line;
            Colchar_column_line_class this_col_line;

            List<int> colIndexes = new List<int>();

            int colCompare = -2;
            for (int indexColChar = 0; indexColChar < column_characterization_lines_length; indexColChar++)
            {
                selected_col_line = column_characterization_lines[indexColChar];
                colCompare = -2;
                for (int indexThis = 0; indexThis < this_length; indexThis++)
                {
                    this_col_line = this.Columns[indexThis];
                    colCompare = this_col_line.Timepoint.CompareTo(selected_col_line.Timepoint);
                    if (colCompare == 0)
                    {
                        colCompare = this_col_line.EntryType.CompareTo(selected_col_line.EntryType);
                    }
                    if (colCompare == 0)
                    {
                        colCompare = this_col_line.SampleName.CompareTo(selected_col_line.SampleName);
                    }
                    if (colCompare == 0)
                    {
                        colIndexes.Add(indexThis);
                        break;
                    }
                }
                if (colCompare != 0)
                {
                    throw new Exception("selecetd colChar does not exist");
                }
            }
            return colIndexes.ToArray();
        }

        public void Get_entryType_timepoint_and_name_of_indicated_column(out Entry_type_enum entryType, out float timepoint, out string name, int indexColumn)
        {
            entryType = Columns[indexColumn].EntryType;
            timepoint = Columns[indexColumn].Timepoint;
            name = (string)Columns[indexColumn].SampleName.Clone();
        }
        #endregion

        #region Switch entry type
        public void Switch_entryType_to_upregulated()
        {
            int columns_length = Columns_length;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                switch (Columns[indexC].EntryType)
                {
                    case Entry_type_enum.Diffrna:
                        Columns[indexC].EntryType = Entry_type_enum.Diffrna_up;
                        break;
                    case Entry_type_enum.Diffprot:
                        Columns[indexC].EntryType = Entry_type_enum.Diffprot_up;
                        break;
                    case Entry_type_enum.Various:
                    default:
                        Columns[indexC].EntryType = Entry_type_enum.Various_up;
                        break;
                }
            }
        }

        public void Switch_entryType_to_downregulated()
        {
            int columns_length = Columns_length;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                switch (Columns[indexC].EntryType)
                {
                    case Entry_type_enum.Diffrna:
                        Columns[indexC].EntryType = Entry_type_enum.Diffrna_down;
                        break;
                    case Entry_type_enum.Diffprot:
                        Columns[indexC].EntryType = Entry_type_enum.Diffprot_down;
                        break;
                    case Entry_type_enum.Various:
                    default:
                        Columns[indexC].EntryType = Entry_type_enum.Various_down;
                        break;
                }
            }
        }
        #endregion

        #region Keep
        public void Keep_only_input_columns(params int[] inputColumns)
        {
            Correctness_check();
            int inputColumns_length = inputColumns.Length;
            Colchar_column_line_class[] new_columns = new Colchar_column_line_class[inputColumns_length];
            for (int indexInput = 0; indexInput < inputColumns_length; indexInput++)
            {
                new_columns[indexInput] = this.Columns[inputColumns[indexInput]].Deep_copy();
            }
            Columns = new_columns;
            Column_rearrangements_adopted = false;
        }

        public void Merge_all_columns()
        {
            int columns_count = this.Columns_length;
            Colchar_column_line_class column_line;
            StringBuilder sb_new_name = new StringBuilder();
            sb_new_name.AppendFormat("Merged");
            Entry_type_enum entryType = this.Columns[0].EntryType;
            for (int indexC = 0; indexC < columns_count; indexC++)
            {
                column_line = this.Columns[indexC];
                if (!column_line.EntryType.Equals(entryType))
                {
                    throw new Exception();
                }
                sb_new_name.AppendFormat("-");
                sb_new_name.AppendFormat("{0}", column_line.Timepoint);
            }

            Colchar_column_line_class new_column_line = new Colchar_column_line_class(entryType, 0, sb_new_name.ToString());
            this.Columns = new Colchar_column_line_class[] { new_column_line };
        }
        #endregion

        #region Add
        public void Identify_new_columns_and_add_at_right_site(Colchar_column_line_class[] add_data)
        {
            Correctness_check();

            int old_columns_length = Columns_length;
            Colchar_column_line_class[] add_columns;

            #region Identify add columns
            List<Colchar_column_line_class> add_columns_list = new List<Colchar_column_line_class>();
            add_data = Colchar_column_line_class.Order_in_standard_way(add_data);
            int add_data_length = add_data.Length;
            Colchar_column_line_class add_line;
            bool old_columns_contain_new_combination = false;
            Colchar_column_line_class old_column_line;
            Colchar_column_line_class add_column_line;
            for (int indexAdd = 0; indexAdd < add_data_length; indexAdd++)
            {
                add_line = add_data[indexAdd];
                if ((indexAdd == 0) || (!Colchar_column_line_class.Equal_in_standard_way(add_line, add_data[indexAdd - 1])))
                {
                    old_columns_contain_new_combination = false;
                    for (int indexOld = 0; indexOld < old_columns_length; indexOld++)
                    {
                        old_column_line = Columns[indexOld];
                        if (old_column_line.Equal_in_standard_way(add_line))
                        {
                            old_columns_contain_new_combination = true;
                            break;
                        }
                    }
                    if (!old_columns_contain_new_combination)
                    {
                        add_column_line = new Colchar_column_line_class(add_line.EntryType, add_line.Timepoint, add_line.SampleName);
                        add_columns_list.Add(add_column_line);
                    }
                }
            }
            add_columns = add_columns_list.ToArray();
            #endregion

            #region Add add columns at right site
            int add_columns_length = add_columns.Length;
            int new_columns_length = add_columns_length + old_columns_length;
            Colchar_column_line_class[] new_columns = new Colchar_column_line_class[new_columns_length];
            int indexNew = -1;
            for (int indexOld = 0; indexOld < old_columns_length; indexOld++)
            {
                indexNew++;
                new_columns[indexNew] = this.Columns[indexOld].Deep_copy();
            }
            for (int indexAdd = 0; indexAdd < add_columns_length; indexAdd++)
            {
                indexNew++;
                new_columns[indexNew] = add_columns[indexAdd].Deep_copy();
            }
            Columns = new_columns;
            #endregion

            Correctness_check();
        }
        #endregion

        #region Copy
        private Colchar_column_line_class[] Deep_copy_columns()
        {
            int columns_length = Columns.Length;
            Colchar_column_line_class[] copy_columns = new Colchar_column_line_class[columns_length];
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                copy_columns[indexC] = this.Columns[indexC].Deep_copy();
            }
            return copy_columns;
        }

        public Colchar_class Deep_copy()
        {
            Colchar_class copy = (Colchar_class)this.MemberwiseClone();
            int columns_length = Columns.Length;
            copy.Columns = Deep_copy_columns();
            return copy;
        }
        #endregion
    }

    ///////////////////////////////////////////////////////////////

    class Data_line_class
    {
        public static float Empty_entry { get { return 0; } }
        public float[] Columns { get; set; }
        public int Columns_length { get { return Columns.Length; } }
        public string NCBI_official_symbol { get; set; }

        public Data_line_class(string ncbi_symbol, int columns_length)
        {
            NCBI_official_symbol = (string)ncbi_symbol.Clone();
            Columns = new float[columns_length];
            for (int indexCol = 0; indexCol < columns_length; indexCol++)
            {
                Columns[indexCol] = Empty_entry;
            }
        }

        public void Rearrange_columns(int[] old_new_index)
        {
            float[] old_columns = Deep_copy_columns();
            int columns_length = Columns.Length;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                Columns[indexC] = 0;
            }
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                Columns[old_new_index[indexC]] = old_columns[indexC];
            }
        }

        public void Add_to_this_line_after_checking_if_this_line_has_empty_entry(IAdd_to_data add_line, int indexCol)
        {
            if (!NCBI_official_symbol.Equals(add_line.NCBI_official_symbol_for_data))
            {
                throw new Exception("rowNames do not match");
            }
            if (Columns[indexCol].Equals(Empty_entry))
            {
                Columns[indexCol] = add_line.Value_for_data;
            }
            else
            {
                throw new Exception("Position already filled");
            }
        }

        public void Sum_values_to_this_in_consideration_of_empty_entry(Data_line_class other)
        {
            int columns_length = Columns_length;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                if ((this.Columns[indexC] != Empty_entry)
                    && (other.Columns[indexC] != Empty_entry))
                {
                    this.Columns[indexC] += other.Columns[indexC];
                }
                else if (other.Columns[indexC] != Empty_entry)
                {
                    this.Columns[indexC] = other.Columns[indexC];
                }
            }
        }

        public void Keep_columns(params int[] kept_columnIndexes)
        {
            List<float> keep_columns = new List<float>();
            int columns_length = this.Columns_length;
            for (int indexColumn = 0; indexColumn < columns_length; indexColumn++)
            {
                if (kept_columnIndexes.Contains(indexColumn))
                {
                    keep_columns.Add(this.Columns[indexColumn]);
                }
            }
            this.Columns = keep_columns.ToArray();
        }

        #region Copy
        private float[] Deep_copy_columns()
        {
            int columns_length = Columns.Length;
            float[] copy = new float[columns_length];
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                copy[indexC] = Columns[indexC];
            }
            return copy;
        }

        public Data_line_class Deep_copy()
        {
            Data_line_class copy = (Data_line_class)this.MemberwiseClone();
            copy.NCBI_official_symbol = (string)this.NCBI_official_symbol.Clone();
            copy.Columns = Deep_copy_columns();
            return copy;
        }
        #endregion
    }

    class Data_private_readWriteOptions_class
    {
        public string Complete_fileName { get; set; }
        public char LineDelimiter { get; set; }

        public Data_private_readWriteOptions_class(string subdirectory, string file_name)
        {
            Complete_fileName = Global_directory_and_file_class.Results_directory + subdirectory + file_name;
            LineDelimiter = Global_class.Tab;
        }
    }

    class Data_class
    {
        #region Fields
        public Data_line_class[] Data { get; set; }
        public int Data_length { get { return Data.Length; } }
        public Colchar_class ColChar { get; set; }
        #endregion

        public Data_class()
        {
            ColChar = new Colchar_class();
            Data = new Data_line_class[0];
        }

        #region Check
        public void Correctness_check()
        {
            ColChar.Correctness_check();
            int col_length = ColChar.Columns.Length;
            int data_length = Data.Length;
            Data_line_class data_line;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = Data[indexD];
                if (data_line.Columns.Length != col_length)
                {
                    throw new Exception("Column lengths do not match");
                }
            }
        }
        #endregion

        #region Order
        public void Order_by_ncbiOfficialSymbol()
        {
            Data = Data.OrderBy(l => l.NCBI_official_symbol).ToArray();
        }

        public void Order_by_datascending_values_in_indicated_column(int indexColumn)
        {
            Data = Data.OrderByDescending(l => l.Columns[indexColumn]).ToArray();
        }
        #endregion

        #region Fill de instance
        public void Add_to_data_instance(IAdd_to_data[] add_data)
        {
            int old_columns_length = ColChar.Columns_length;
            int add_data_length = add_data.Length;
            IAdd_to_data add_data_interface_line;

            #region Analyze, if new columns needed and add to colChar
            add_data = add_data.OrderBy(l => l.EntryType_for_data).ThenBy(l => l.Timepoint_for_data).ThenBy(l => l.SampleName_for_data).ToArray();
            List<Colchar_column_line_class> colChar_column_list = new List<Colchar_column_line_class>();
            Colchar_column_line_class colChar_column_line;
            for (int indexAdd=0; indexAdd<add_data_length; indexAdd++)
            {
                add_data_interface_line = add_data[indexAdd];
                if (  (indexAdd==0)
                    || (!add_data_interface_line.EntryType_for_data.Equals(add_data[indexAdd - 1].EntryType_for_data))
                    || (!add_data_interface_line.Timepoint_for_data.Equals(add_data[indexAdd - 1].Timepoint_for_data))
                    || (!add_data_interface_line.SampleName_for_data.Equals(add_data[indexAdd - 1].SampleName_for_data)))
                {
                    colChar_column_line = new Colchar_column_line_class();
                    colChar_column_line.EntryType = add_data_interface_line.EntryType_for_data;
                    colChar_column_line.Timepoint = add_data_interface_line.Timepoint_for_data;
                    colChar_column_line.SampleName = (string)add_data_interface_line.SampleName_for_data.Clone();
                    colChar_column_list.Add(colChar_column_line);
                }
            }
            ColChar.Identify_new_columns_and_add_at_right_site(colChar_column_list.ToArray());
            int new_columns_length = ColChar.Columns.Length;
            #endregion

            int data_length = this.Data_length;
            Data_line_class data_line;

            #region Extend data lines by adding zero values at right site
            float[] new_columns;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = this.Data[indexD];
                new_columns = new float[new_columns_length];
                for (int indexOld = 0; indexOld < old_columns_length; indexOld++)
                {
                    new_columns[indexOld] = data_line.Columns[indexOld];
                }
                data_line.Columns = new_columns.ToArray();
            }
            #endregion

            #region Add to data and generate ordered data add lines to data
            add_data = add_data.OrderBy(l => l.NCBI_official_symbol_for_data).ThenBy(l => l.EntryType_for_data).ThenBy(l => l.SampleName_for_data).ToArray();

            int indexColumn;
            int indexData = 0;
            int stringCompare;
            Data_line_class new_data_line;
            List<Data_line_class> add_lines_to_data_list = new List<Data_line_class>();
            int indexCol = -1;

            for (int indexAdd = 0; indexAdd < add_data_length; indexAdd++)
            {
                add_data_interface_line = add_data[indexAdd];
                indexColumn = ColChar.Get_columnIndex(add_data_interface_line);
                stringCompare = -2;
                while ((indexData < data_length) && (stringCompare < 0))
                {
                    data_line = Data[indexData];
                    stringCompare = data_line.NCBI_official_symbol.CompareTo(add_data_interface_line.NCBI_official_symbol_for_data);
                    if (stringCompare < 0)
                    {
                        indexData++;
                    }
                    else if (stringCompare == 0)
                    {
                        indexCol = ColChar.Get_columnIndex(add_data_interface_line);
                        data_line.Add_to_this_line_after_checking_if_this_line_has_empty_entry(add_data_interface_line, indexCol);
                    }
                }
                if (stringCompare != 0)
                {
                    new_data_line = new Data_line_class(add_data_interface_line.NCBI_official_symbol_for_data, new_columns_length);
                    new_data_line.Columns[indexColumn] = add_data_interface_line.Value_for_data;
                    add_lines_to_data_list.Add(new_data_line);
                }
            }
            Data_line_class[] add_lines_to_data = add_lines_to_data_list.OrderBy(l => l.NCBI_official_symbol).ToArray();
            #endregion

            #region Combine add_lines_to_data to generate final add_data_lines
            int add_lines_length = add_lines_to_data.Length;
            int firstIndex_same_rowName = -1;
            Data_line_class add_data_line;
            Data_line_class inner_add_data_line;
            List<Data_line_class> final_add_data_list = new List<Data_line_class>();
            for (int indexAL = 0; indexAL < add_lines_length; indexAL++)
            {
                add_data_line = add_lines_to_data[indexAL];
                if ((indexAL == 0) || (!add_data_line.NCBI_official_symbol.Equals(add_lines_to_data[indexAL - 1].NCBI_official_symbol)))
                {
                    firstIndex_same_rowName = indexAL;
                }
                if ((indexAL == add_lines_length - 1) || (!add_data_line.NCBI_official_symbol.Equals(add_lines_to_data[indexAL + 1].NCBI_official_symbol)))
                {
                    for (int indexInner = firstIndex_same_rowName; indexInner < indexAL; indexInner++)
                    {
                        inner_add_data_line = add_lines_to_data[indexInner];
                        add_data_line.Sum_values_to_this_in_consideration_of_empty_entry(inner_add_data_line);
                    }
                    final_add_data_list.Add(add_data_line);
                }
            }
            #endregion

            #region Add final data line to data
            int final_data_length = final_add_data_list.Count;
            int new_data_length = final_data_length + data_length;
            Data_line_class[] new_data = new Data_line_class[new_data_length];
            int indexNew = -1;
            for (int indexOld = 0; indexOld < data_length; indexOld++)
            {
                indexNew++;
                new_data[indexNew] = Data[indexOld];
            }
            for (int indexFinal = 0; indexFinal < final_data_length; indexFinal++)
            {
                indexNew++;
                new_data[indexNew] = final_add_data_list[indexFinal];
            }
            Data = new_data;
            #endregion

            Keep_only_lines_that_contain_at_least_one_non_zero_value();
        }
        #endregion

        #region Order columns
        public void Order_columns_in_standard_way()
        {
            int[] old_new_index = ColChar.Order_columns_in_standard_way_and_get_old_new_index_array();
            int data_length = Data.Length;
            int columns_length = ColChar.Columns_length;
            Data_line_class[] ordered_data = new Data_line_class[data_length];
            Data_line_class data_line;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = Data[indexD];
                data_line.Rearrange_columns(old_new_index);
            }
            ColChar.Column_rearrangements_adopted = true;
        }
        #endregion

        #region Keep
        public void Keep_only_input_rowNames(string[] input_rowNames)
        {
            List<Data_line_class> keep_data = new List<Data_line_class>();
            input_rowNames = input_rowNames.Distinct().OrderBy(l => l).ToArray();
            string input_rowName;
            int indexInput = 0;
            int input_rowNames_length = input_rowNames.Length;
            this.Order_by_ncbiOfficialSymbol();
            int data_length = Data_length;
            int stringCompare = -2;
            Data_line_class data_line;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                stringCompare = -2;
                while ((indexInput < input_rowNames_length) && (stringCompare < 0))
                {
                    input_rowName = input_rowNames[indexInput];
                    stringCompare = input_rowName.CompareTo(data_line.NCBI_official_symbol);
                    if (stringCompare < 0)
                    {
                        indexInput++;
                    }
                    else if (stringCompare == 0)
                    {
                        keep_data.Add(data_line);
                    }
                }
            }
            Data = keep_data.ToArray();
        }

        public void Remove_empty_rows_and_columns()
        {
            int column_length = this.ColChar.Columns_length;
            List<int> keepColumns = new List<int>();
            bool keep_row;
            List<Data_line_class> newDE = new List<Data_line_class>();
            foreach (Data_line_class line in Data)
            {
                keep_row = false;
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (line.Columns[indexCol] != 0)
                    {
                        keepColumns.Add(indexCol);
                        keep_row = true;
                    }
                }
                if (keep_row)
                {
                    newDE.Add(line);
                }
            }

            if (keepColumns.Count() < column_length)
            {
                ColChar.Keep_only_input_columns(keepColumns.ToArray());
                foreach (Data_line_class line in Data)
                {
                    line.Keep_columns(keepColumns.ToArray());
                }
                ColChar.Column_rearrangements_adopted = true;
            }
            Data = newDE.ToArray();
        }

        public void Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(params int[] inputColumns)
        {
            inputColumns = inputColumns.Distinct().OrderBy(l => l).ToArray();
            ColChar.Keep_only_input_columns(inputColumns);
            inputColumns = inputColumns.Distinct().OrderBy(l => l).ToArray();
            int inputColumns_length = inputColumns.Length;
            int data_length = Data.Length;
            Data_line_class data_line;
            float[] new_columns;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                new_columns = new float[inputColumns_length];
                for (int indexI = 0; indexI < inputColumns_length; indexI++)
                {
                    new_columns[indexI] = data_line.Columns[inputColumns[indexI]];
                }
                data_line.Columns = new_columns;
            }
            Remove_empty_rows_and_columns();
            ColChar.Column_rearrangements_adopted = true;
        }

        public void Keep_only_columns_with_indicated_column_characterization_lines(params Colchar_column_line_class[] keep_col_chars)
        {
            int[] colIndexes = this.ColChar.Get_indexes_of_input_column_characterization_lines(keep_col_chars);
            this.Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(colIndexes);
        }
        #endregion

        #region Set to upper case
        public void Set_all_ncbi_official_gene_symbols_to_upper_case()
        {
            foreach (Data_line_class data_line in Data)
            {
                data_line.NCBI_official_symbol = data_line.NCBI_official_symbol.ToUpper();
            }
        }
        #endregion

        #region Keep only above or below cutoff
        private void Keep_only_lines_that_contain_at_least_one_non_zero_value()
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;
            List<Data_line_class> kept_data_list = new List<Data_line_class>();
            bool keep_line;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                keep_line = false;
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] != 0) { keep_line = true; }
                }
                if (keep_line) { kept_data_list.Add(data_line); }
            }
            Data = kept_data_list.ToArray();
        }

        private void Keep_only_columns_that_contain_at_least_one_non_zero_entry()
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;
            bool[] keep_colunns = new bool[column_length];
            for (int indexK = 0; indexK < column_length; indexK++) { keep_colunns[indexK] = false; }
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] != 0) { keep_colunns[indexCol] = true; }
                }
            }

            List<int> keep_column_indexes = new List<int>();
            for (int indexCol = 0; indexCol < column_length; indexCol++)
            {
                keep_column_indexes.Add(indexCol);
            }

            Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(keep_column_indexes.ToArray());
        }

        public void Keep_only_values_equal_to_or_greater_than_cutoff(float cutoff)
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;

            #region Set all values 0 that below cutoff
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] < cutoff) { data_line.Columns[indexCol] = 0; }
                }
            }
            #endregion

            Keep_only_lines_that_contain_at_least_one_non_zero_value();
            Keep_only_columns_that_contain_at_least_one_non_zero_entry();
        }

        public void Keep_only_upregulated_entries_and_change_entryType()
        {
            this.Keep_only_values_equal_to_or_greater_than_cutoff(0);
            ColChar.Switch_entryType_to_upregulated();
        }

        public void Keep_only_downregulated_entries_and_change_entryType()
        {
            this.Keep_only_values_equal_to_or_smaller_than_cutoff(0);
            ColChar.Switch_entryType_to_downregulated();
        }

        public void Keep_only_values_equal_to_or_smaller_than_cutoff(float cutoff)
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;
            this.Data = this.Data.OrderBy(l => l.Columns[0]).ToArray();


            #region Set all values 0 that above cutoff
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] > cutoff) { data_line.Columns[indexCol] = 0; }
                }
            }
            #endregion

            Keep_only_lines_that_contain_at_least_one_non_zero_value();
            Keep_only_columns_that_contain_at_least_one_non_zero_entry();
        }
        #endregion

        #region Get data instances with separated up- and downregulated entries
        public Data_class Get_data_instance_with_only_upregulated_entries()
        {
            Data_class data_up = this.Deep_copy();
            data_up.Keep_only_values_equal_to_or_greater_than_cutoff(0F);
            data_up.ColChar.Switch_entryType_to_upregulated();
            return data_up;
        }

        public Data_class Get_data_instance_with_only_downregulated_entries()
        {
            Data_class data_down = this.Deep_copy();
            data_down.Keep_only_values_equal_to_or_smaller_than_cutoff(0F);
            data_down.ColChar.Switch_entryType_to_downregulated();
            return data_down;
        }

        public Data_class Get_data_instance_with_separated_up_and_down_regulated_entries()
        {
            Data_class data = Get_data_instance_with_only_upregulated_entries();
            Data_class data_down = Get_data_instance_with_only_downregulated_entries();
            data.Add_other_data_instance(data_down);
            return data;
        }
        #endregion

        #region Add other data instance
        public void Add_other_data_instance(Data_class other_data)
        {
            List<IAdd_to_data_line_class> add_list = new List<IAdd_to_data_line_class>();

            #region Generate add list of other data
            IAdd_to_data_line_class add_line;
            int other_length = other_data.Data_length;
            int other_column_length = other_data.ColChar.Columns_length;
            Data_line_class other_line;
            Colchar_column_line_class other_column;
            for (int indexOther = 0; indexOther < other_length; indexOther++)
            {
                other_line = other_data.Data[indexOther];
                for (int indexCol = 0; indexCol < other_column_length; indexCol++)
                {
                    other_column = other_data.ColChar.Columns[indexCol];
                    if (other_line.Columns[indexCol] != 0)
                    {
                        add_line = new IAdd_to_data_line_class();
                        add_line.SampleName_for_data = (string)other_column.SampleName.Clone();
                        add_line.EntryType_for_data = other_column.EntryType;
                        add_line.Timepoint_for_data = other_column.Timepoint;
                        add_line.NCBI_official_symbol_for_data = (string)other_line.NCBI_official_symbol.Clone();
                        add_line.Value_for_data = other_line.Columns[indexCol];
                        add_list.Add(add_line);
                    }
                }
            }
            #endregion

            Add_to_data_instance(add_list.ToArray());
        }
        #endregion
 
        #region Get NCBI official symbols
        public string[] Get_alphabetically_ordered_ncbi_official_symbols__with_non_empty_entries_in_indicated_column(int indexColumn)
        {
            List<string> ncbi_official_symbols_list = new List<string>();
            int data_length = Data.Length;
            Data_line_class data_line;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                if (data_line.Columns[indexColumn] != Data_line_class.Empty_entry)
                {
                    ncbi_official_symbols_list.Add(data_line.NCBI_official_symbol);
                }
            }
            return ncbi_official_symbols_list.OrderBy(l => l).ToArray();
        }

        public string[] Get_alphabetically_ordered_ncbi_official_symbols()
        {
            List<string> ncbi_official_symbols_list = new List<string>();
            int data_length = Data.Length;
            Data_line_class data_line;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                ncbi_official_symbols_list.Add(data_line.NCBI_official_symbol);
            }
            return ncbi_official_symbols_list.OrderBy(l => l).ToArray();
        }

        public string[] Get_rowNames_ordered_by_datascending_values_with_non_empty_entries_in_indicated_column(int indexColumn)
        {
            this.Order_by_datascending_values_in_indicated_column(indexColumn);
            List<string> rowNames = new List<string>();
            int data_length = Data.Length;
            Data_line_class data_line;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                if (data_line.Columns[indexColumn] != Data_line_class.Empty_entry)
                {
                    rowNames.Add(data_line.NCBI_official_symbol);
                }
            }
            return rowNames.ToArray();
        }
        #endregion

        #region Write read copy
        public Data_class Deep_copy()
        {
            Data_class copy = (Data_class)this.MemberwiseClone();
            int data_length = Data.Length;
            copy.Data = new Data_line_class[data_length];
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                copy.Data[indexD] = this.Data[indexD].Deep_copy();
            }
            copy.ColChar = this.ColChar.Deep_copy();
            return copy;
        }

        private void Write_simple_headline(StreamWriter writer, Data_private_readWriteOptions_class readWriteOptions)
        {
            int columns_length = ColChar.Columns.Length;
            char lineDelimiter = readWriteOptions.LineDelimiter;

            writer.Write("RowNames");
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                writer.Write("{0}{1}", readWriteOptions.LineDelimiter, ColChar.Columns[indexC].Full_column_name);
            }
            writer.Write("{0}RowNames2", lineDelimiter);
            writer.WriteLine("{0}ReadWrite_subcellular_processes", lineDelimiter);
        }

        private void Write_multiple_line_headline(StreamWriter writer, Data_private_readWriteOptions_class readWriteOptions)
        {
            char lineDelimiter = readWriteOptions.LineDelimiter;
            int columns_length = ColChar.Columns.Length;
            int max_stringSplit_length = -1;
            string[] splitStrings;
            string[][] splitStrings_array = new string[columns_length][];

            #region Fill splitStrings_array and get max stringSplit length
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                splitStrings = ColChar.Columns[indexC].Full_column_name.Split(Colchar_column_line_class.Full_name_datalimiter);
                splitStrings_array[indexC] = splitStrings;
                max_stringSplit_length = Math.Max(max_stringSplit_length, splitStrings.Length);
            }
            #endregion

            #region Write multiple headlines
            for (int indexSplit = 0; indexSplit < max_stringSplit_length; indexSplit++)
            {
                writer.Write("RowNames");
                for (int indexColumn = 0; indexColumn < columns_length; indexColumn++)
                {
                    splitStrings = splitStrings_array[indexColumn];
                    writer.Write("{0}{1}", readWriteOptions.LineDelimiter, splitStrings[indexSplit]);
                }
                writer.WriteLine("RowNames2", lineDelimiter);
                writer.WriteLine("{0}ReadWrite_subcellular_processes", lineDelimiter);
            }
            #endregion
        }

        public void Set_all_rowNames_to_upperCase()
        {
            foreach (Data_line_class data_line in this.Data)
            {
                data_line.NCBI_official_symbol = (string)data_line.NCBI_official_symbol.ToUpper();
            }
        }

        private void Write_data(StreamWriter writer, Data_private_readWriteOptions_class readWriteOptions)
        {
            int columns_length = ColChar.Columns.Length;
            char lineDelimiter = readWriteOptions.LineDelimiter;

            Data_line_class data_line;
            int data_length = Data.Length;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = Data[indexD];
                writer.Write("{0}", data_line.NCBI_official_symbol);
                for (int indexC = 0; indexC < columns_length; indexC++)
                {
                    writer.Write("{0}{1}", lineDelimiter, data_line.Columns[indexC]);
                }
            }
        }

        public void Write_with_simple_headline(string subdirectory, string file_name)
        {
            Data_private_readWriteOptions_class readWriteOptions = new Data_private_readWriteOptions_class(subdirectory, file_name);
            StreamWriter writer = new StreamWriter(readWriteOptions.Complete_fileName);
            Write_simple_headline(writer, readWriteOptions);
            Write_data(writer, readWriteOptions);
            writer.Close();
        }

        public void Write_with_multiple_line_headline(string subdirectory, string file_name)
        {
            Data_private_readWriteOptions_class readWriteOptions = new Data_private_readWriteOptions_class(subdirectory,file_name);
            StreamWriter writer = new StreamWriter(readWriteOptions.Complete_fileName);
            Write_multiple_line_headline(writer, readWriteOptions);
            Write_data(writer, readWriteOptions);
            writer.Close();
        }
        #endregion
    }
}
