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
using System.IO;
using Common_functions.Global_definitions;

namespace Common_functions.Report
{
    class Report_class
    {
        static string File_name = Global_directory_and_file_class.Results_directory + "Report_output.jns";
        static string Error_file_name = Global_directory_and_file_class.Results_directory + "Report_output_errors.jns";
        static string Imperfect_code_file_name = Global_directory_and_file_class.Results_directory + "Report_output_imperfect_code.jns";

        public static bool Report_file_in_use()
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(File_name);
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static void Shift_text_right(string name)
        {
            int name_length = name.Length;
            for (int i = 0; i < name_length + 2; i++) { Write(" "); }
        }

        public static void Write_analysis_start()
        {
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("Start analysis, {0}, {1}", DateTime.Now.ToString("yyyy.MM.dd"), string.Format("{0:HH:mm:ss tt}", DateTime.Now));
        }

        public static void Write_analysis_end()
        {
            Report_class.WriteLine("Analysis finished, {0}, {1}", DateTime.Now.ToString("yyyy.MM.dd"), string.Format("{0:HH:mm:ss tt}", DateTime.Now));
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("-------------------------------------------------------------------------------");
        }

        public static void Write_analysis_start_announcement(string analysisName)
        {
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("Analysis start of {0}", analysisName);
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine();
        }

        public static void Write_analysis_end_announcement(string analysisName)
        {
            Report_class.WriteLine();
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("Analysis end of {0}", analysisName);
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine("-------------------------------------------------------------------------------");
            Report_class.WriteLine();
        }

        public static void Write(string text, params object[] add)
        {
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.Write(text, add);
            Console.Write(text, add);
            writer.Close();
        }

        public static void Write(params object[] add)
        {
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.Write(add);
            Console.Write(add);
            writer.Close();
        }

        public static void Write_major_line(string text, params object[] add)
        {
            Report_class.WriteLine();
            Report_class.WriteLine("*******************************************************************************");
            Report_class.WriteLine(text, add);
            int object_length = 0;
            foreach (object add_object in add)
            {
                object_length = +add_object.ToString().Length;
            }
            for (int i = 0; i < text.Length + object_length; i++) { Report_class.Write("*"); }
            Report_class.WriteLine();
        }

        public static void Write_major_line(string text)
        {
            Report_class.WriteLine();
            Report_class.WriteLine("*******************************************************************************");
            Report_class.WriteLine(text);
            for (int i = 0; i < text.Length; i++) { Report_class.Write("*"); }
            Report_class.WriteLine();
        }

        public static void Write_error_line(string text, params object[] add)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Report_class.WriteLine(text, add);
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.ResetColor();
            StreamWriter writer = new StreamWriter(Error_file_name, true);
            writer.WriteLine(text, add);
            writer.Close();
        }

        public static void Write_error_line(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Report_class.WriteLine(text);
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.ResetColor();
            StreamWriter writer = new StreamWriter(Error_file_name, true);
            writer.WriteLine(text);
            writer.Close();
        }

        public static void Write_code_imperfect_line(string text, params object[] add)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Report_class.WriteLine(text, add);
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.ResetColor();
            StreamWriter writer = new StreamWriter(Imperfect_code_file_name, true);
            writer.WriteLine(text, add);
            writer.Close();
        }

        public static void Write_code_imperfect_line(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Report_class.WriteLine(text);
            Report_class.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.ResetColor();
            StreamWriter writer = new StreamWriter(Imperfect_code_file_name, true);
            writer.WriteLine(text);
            writer.Close();
        }

        public static void Write_notation_line(string text, params object[] add)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Report_class.WriteLine(text, add);
            Console.ResetColor();
            StreamWriter writer = new StreamWriter(Error_file_name, true);
            writer.WriteLine(text, add);
            writer.Close();
        }

        public static void Write_notation_line(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Report_class.WriteLine(text);
            Console.ResetColor();
            StreamWriter writer = new StreamWriter(Error_file_name, true);
            writer.WriteLine(text);
            writer.Close();
        }

        public static void Write_notation(string text, params object[] add)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.Write(text, add);
            Console.Write(text, add);
            writer.Close();
            Console.ResetColor();
        }

        public static void Write_notation(params object[] add)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.Write(add);
            Console.Write(add);
            writer.Close();
            Console.ResetColor();
        }

        public static void Write_notation_line()
        {
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.WriteLine();
            Console.WriteLine();
            writer.Close();
        }

        public static void WriteLine()
        {
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.WriteLine();
            Console.WriteLine();
            writer.Close();
        }

        public static void WriteLine(string text, params object[] add)
        {
            StreamWriter writer = new StreamWriter(File_name, true);
            if ((add != null) && (add.Length > 0))
            {
                writer.WriteLine(text, add);
                Console.WriteLine(text, add);
            }
            else
            {
                writer.WriteLine(text);
                Console.WriteLine(text);
            }
            writer.Close();
        }

        public static void WriteLine(params object[] add)
        {
            StreamWriter writer = new StreamWriter(File_name, true);
            writer.WriteLine(add);
            Console.WriteLine(add);
            writer.Close();
        }

        public static void Write_minor_separation_line()
        {
            WriteLine("---------------------------------------------");
        }

        public static void Write_major_separation_line()
        {
            WriteLine("-------------------------------------------------------------------------------");
        }
    }
}
