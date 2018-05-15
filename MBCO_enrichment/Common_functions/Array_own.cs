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
using Common_functions.Report;

namespace Common_functions.Array_own
{
    class Array_class
    {
        public static bool Array_order_dependent_equal<T>(T[] array1, T[] array2)
        {
            bool equal = true;
            int array1_length = array1.Length;
            int array2_length = array2.Length;
            if (array1_length != array2_length)
            {
                equal = false;
            }
            else
            {
                for (int indexA = 0; indexA < array1_length; indexA++)
                {
                    if (!array1[indexA].Equals(array2[indexA]))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }

        public static bool Array_order_independent_equal<T>(T[] array1, T[] array2)
        {
            array1 = array1.OrderBy(l => l).ToArray();
            array2 = array1.OrderBy(l => l).ToArray();
            return Array_order_dependent_equal<T>(array1, array2);
        }

        public static T[] Get_shallow_copy_array_within_index_range<T>(T[] array, int indexStart, int indexEnd)
        {
            var TType = typeof(T);
            int new_array_length = indexEnd - indexStart + 1;
            T[] new_array = new T[new_array_length];
            for (int i = indexStart; i <= indexEnd; i++)
            {
                new_array[i - indexStart] = array[i];
            }
            return new_array;
        }

        public static string[] Deep_copy_string_array(string[] array)
        {
            int array_length = array.Length;
            string[] copy = new string[array_length];
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                copy[indexA] = (string)array[indexA].Clone();
            }
            return copy;
        }

        public static List<string> Deep_copy_string_list(List<string> list)
        {
            int list_count = list.Count;
            List<string> copy = new List<string>();
            for (int indexL = 0; indexL < list_count; indexL++)
            {
                copy.Add((string)list[indexL].Clone());
            }
            return copy;
        }

        public static T[] Deep_copy_array<T>(T[] array)
        {
            T[] copy = new T[0];
            if (typeof(T) == typeof(string))
            {
                Report_class.Write_error_line("{0}: Deep copy array, typeof(T)=={1} is not allowed", typeof(Array_class).Name, typeof(T));
            }
            else if (array != null)
            {
                int array_length = array.Length;
                copy = new T[array_length];
                for (int indexA = 0; indexA < array_length; indexA++)
                {
                    copy[indexA] = (T)array[indexA];
                }
            }
            else
            {
                copy = array;
            }
            return copy;
        }

        public static bool Equal_arrays<T>(T[] array1, T[] array2) where T : IComparable<T>
        {
            int array1_length = array1.Length;
            int array2_length = array2.Length;
            bool equal = true;
            if (array1_length != array2_length)
            {
                equal = false;
            }
            else
            {
                for (int indexA = 0; indexA < array1_length; indexA++)
                {
                    if (!array1[indexA].Equals(array2[indexA]))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }

        public static T[] Get_ordered_union<T>(T[] array1, params T[] array2) where T : IComparable<T>
        {
            int array1_length = array1.Length;
            int array2_length = array2.Length;
            int combined_array_length = array1_length + array2_length;
            int indexC = -1;
            T[] combined_array = new T[combined_array_length];
            for (int index1 = 0; index1 < array1_length; index1++)
            {
                indexC++;
                combined_array[indexC] = array1[index1];
            }
            for (int index2 = 0; index2 < array2_length; index2++)
            {
                indexC++;
                combined_array[indexC] = array2[index2];
            }
            return combined_array.Distinct().OrderBy(l => l).ToArray();

        }

        public static int Compare_two_arrays<T>(T[] array1, T[] array2) where T : IComparable<T>
        {
            int array1_length = array1.Length;
            int array2_length = array2.Length;
            int shorter_length = Math.Min(array1_length, array2_length);
            int tCompare = 0;
            var TType = typeof(T);
            for (int indexS = 0; indexS < shorter_length; indexS++)
            {
                tCompare = array1[indexS].CompareTo(array2[indexS]);
                if (tCompare != 0)
                {
                    break;
                }
            }
            if (tCompare == 0)
            {
                if (array1_length < array2_length)
                {
                    tCompare = -1;
                }
                else if (array1_length > array2_length)
                {
                    tCompare = 1;
                }
            }
            return tCompare;
        }

        public static T[] Combine_and_order_arrays_by_shallow_copy_of_elements<T>(T[][] arrays)
        {
            int number_of_arrays = arrays.Length;
            int new_array_length = 0;
            for (int indexN = 0; indexN < number_of_arrays; indexN++)
            {
                new_array_length += arrays[indexN].Length;
            }
            T[] new_array = new T[new_array_length];
            int indexNew = -1;
            T[] act_array;
            int act_length;
            for (int indexN = 0; indexN < number_of_arrays; indexN++)
            {
                act_array = arrays[indexN];
                act_length = act_array.Length;
                for (int indexAct = 0; indexAct < act_length; indexAct++)
                {
                    indexNew++;
                    new_array[indexNew] = act_array[indexAct];
                }
            }
            new_array = new_array.OrderBy(l => l).ToArray();
            return new_array;
        }

        public static T[][] Generate_jagged_array_with_all_possible_element_combinations<T>(T[] array1, T[] array2)
        {
            int array1_length = array1.Length;
            int array2_length = array2.Length;
            int combination_length = array1_length * array2_length;
            T[][] combination = new T[combination_length][];
            int indexC = -1;
            for (int index1 = 0; index1 < array1_length; index1++)
            {
                for (int index2 = 0; index2 < array2_length; index2++)
                {
                    indexC++;
                    combination[indexC] = new T[] { array1[index1], array2[index2] };
                }
            }
            return combination;
        }

        public static T[] Generate_array_instance<T>(int array_length) where T : class
        {
            T[] array = new T[array_length];
            var TType = typeof(T);
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                array[indexA] = (T)Activator.CreateInstance(TType);
            }
            return array;
        }
    }

    class Overlap_class
    {
        public static string[] Get_intersection(string[] list1, params string[] list2)
        {
            list1 = list1.Distinct().OrderBy(l => l).ToArray();
            list2 = list2.Distinct().OrderBy(l => l).ToArray();
            int list1_length = list1.Length;
            int list2_length = list2.Length;
            int index1 = 0;
            int index2 = 0;
            int stringCompare;
            List<string> intersection = new List<string>();
            while ((index1 < list1_length) && (index2 < list2_length))
            {
                stringCompare = list2[index2].CompareTo(list1[index1]);
                if (stringCompare < 0) { index2++; }
                else if (stringCompare > 0) { index1++; }
                else
                {
                    intersection.Add(list1[index1]);
                    index1++;
                    index2++;
                }
            }
            return intersection.ToArray();
        }

        public static string[] Get_union(string[] list1, params string[] list2)
        {
            string[] union = new string[0];
            if ((list1 != null) && (list2 != null))
            {
                list1 = list1.Distinct().OrderBy(l => l).ToArray();
                list2 = list2.Distinct().OrderBy(l => l).ToArray();
                int list1_length = list1.Length;
                int list2_length = list2.Length;
                int index1 = 0;
                int index2 = 0;
                int stringCompare;
                List<string> union_list = new List<string>();
                while ((index1 < list1_length) || (index2 < list2_length))
                {
                    if ((index1 < list1_length) && (index2 < list2_length))
                    {
                        stringCompare = list2[index2].CompareTo(list1[index1]);
                        if (stringCompare < 0)
                        {
                            union_list.Add(list2[index2]);
                            index2++;
                        }
                        else if (stringCompare > 0)
                        {
                            union_list.Add(list1[index1]);
                            index1++;
                        }
                        else
                        {
                            union_list.Add(list1[index1]);
                            index1++;
                            index2++;
                        }
                    }
                    else if (index1 < list1_length)
                    {
                        union_list.Add(list1[index1]);
                        index1++;
                    }
                    else if (index2 < list2_length)
                    {
                        union_list.Add(list2[index2]);
                        index2++;
                    }
                }
                union = union_list.ToArray();
            }
            else if (list1 != null)
            {
                union = list1;
            }
            else if (list2 != null)
            {
                union = list2;
            }
            return union;
        }

        public static string[] Get_part_of_list1_but_not_of_list2(string[] list1, params string[] list2)
        {
            list1 = list1.Distinct().OrderBy(l => l).ToArray();
            list2 = list2.Distinct().OrderBy(l => l).ToArray();
            List<string> not = new List<string>();
            int list1_length = list1.Length;
            int list2_length = list2.Length;
            int index2 = 0;
            int stringCompare;
            for (int index1 = 0; index1 < list1_length; index1++)
            {
                stringCompare = -2;
                while ((index2 < list2_length) && (stringCompare < 0))
                {
                    stringCompare = list2[index2].CompareTo(list1[index1]);
                    if (stringCompare < 0) { index2++; }
                }
                if ((stringCompare > 0) || (index2 == list2_length))
                {
                    not.Add(list1[index1]);
                }
            }
            return not.ToArray();
        }

        public static int[] Get_intersection(int[] list1, int[] list2)
        {
            list1 = list1.Distinct().OrderBy(l => l).ToArray();
            list2 = list2.Distinct().OrderBy(l => l).ToArray();
            int list1_length = list1.Length;
            int list2_length = list2.Length;
            int index1 = 0;
            int index2 = 0;
            int stringCompare;
            List<int> intersection = new List<int>();
            while ((index1 < list1_length) && (index2 < list2_length))
            {
                stringCompare = list2[index2].CompareTo(list1[index1]);
                if (stringCompare < 0) { index2++; }
                else if (stringCompare > 0) { index1++; }
                else
                {
                    intersection.Add(list1[index1]);
                    index1++;
                    index2++;
                }
            }
            return intersection.ToArray();
        }

        public static int[] Get_union(int[] list1, int[] list2)
        {
            int[] union = new int[0];
            if ((list1 != null) && (list2 != null))
            {
                list1 = list1.Distinct().OrderBy(l => l).ToArray();
                list2 = list2.Distinct().OrderBy(l => l).ToArray();
                int list1_length = list1.Length;
                int list2_length = list2.Length;
                int index1 = 0;
                int index2 = 0;
                int intCompare;
                List<int> union_list = new List<int>();
                while ((index1 < list1_length) || (index2 < list2_length))
                {
                    if ((index1 < list1_length) && (index2 < list2_length))
                    {
                        intCompare = list2[index2] - list1[index1];
                        if (intCompare < 0)
                        {
                            union_list.Add(list2[index2]);
                            index2++;
                        }
                        else if (intCompare > 0)
                        {
                            union_list.Add(list1[index1]);
                            index1++;
                        }
                        else
                        {
                            union_list.Add(list1[index1]);
                            index1++;
                            index2++;
                        }
                    }
                    else if (index1 < list1_length)
                    {
                        union_list.Add(list1[index1]);
                        index1++;
                    }
                    else if (index2 < list2_length)
                    {
                        union_list.Add(list2[index2]);
                        index2++;
                    }
                }
                union = union_list.ToArray();
            }
            else if (list1 != null)
            {
                union = list1;
            }
            else if (list2 != null)
            {
                union = list2;
            }
            return union;
        }

        public static T[] Get_intersection<T>(T[] list1, T[] list2) where T : IComparable
        {
            list1 = list1.Distinct().OrderBy(l => l).ToArray();
            list2 = list2.Distinct().OrderBy(l => l).ToArray();
            int list1_length = list1.Length;
            int list2_length = list2.Length;
            int index1 = 0;
            int index2 = 0;
            int stringCompare;
            List<T> intersection = new List<T>();
            while ((index1 < list1_length) && (index2 < list2_length))
            {
                stringCompare = list2[index2].CompareTo(list1[index1]);
                if (stringCompare < 0) { index2++; }
                else if (stringCompare > 0) { index1++; }
                else
                {
                    intersection.Add(list1[index1]);
                    index1++;
                    index2++;
                }
            }
            return intersection.ToArray();
        }

        public static T[] Get_part_of_list1_but_not_of_list2<T>(T[] list1, T[] list2) where T : IComparable
        {
            list1 = list1.Distinct().OrderBy(l => l).ToArray();
            list2 = list2.Distinct().OrderBy(l => l).ToArray();
            List<T> not = new List<T>();
            int list1_length = list1.Length;
            int list2_length = list2.Length;
            int index2 = 0;
            int stringCompare;
            for (int index1 = 0; index1 < list1_length; index1++)
            {
                stringCompare = -2;
                while ((index2 < list2_length) && (stringCompare < 0))
                {
                    stringCompare = list2[index2].CompareTo(list1[index1]);
                    if (stringCompare < 0) { index2++; }
                }
                if ((stringCompare > 0) || (index2 == list2_length))
                {
                    not.Add(list1[index1]);
                }
            }
            return not.ToArray();
        }
    }
}
