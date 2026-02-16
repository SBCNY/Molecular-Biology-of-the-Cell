//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a Apache 2.0 license.

//Please acknowledge MBC PathNet in your publications by citing the following reference:
//MBC PathNet: integration and visualization of networks connecting functionally related pathways predicted from transcriptomic and proteomic datasets
//Jens Hansen, Ravi Iyengar. Bioinform Adv. 2025 Aug 18; 5(1):vbaf197. PMID: 40917650. PMCID: PMC12413227. DOI:10.1093/bioadv/vbaf197

//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

//Last update: 2026 February 12

using ClassLibrary1;
using ClassLibrary1.Prepare_datasets;
using System;
using System.Runtime.InteropServices;

namespace Pubmed_search
{
    class Main_class
    {
        public static void Main(string[] arguments)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Collections.Generic.List<string> non_empty_arguments = new System.Collections.Generic.List<string>();
            foreach (string argument in arguments)
            {
                if (!String.IsNullOrEmpty(argument))
                {
                    non_empty_arguments.Add(argument);
                }
            }
            arguments = non_empty_arguments.ToArray();
            if (arguments.Length > 0) { System.Windows.Forms.Application.Run(new Mbco_user_application_form1(arguments)); }
            else { System.Windows.Forms.Application.Run(new Mbco_user_application_form()); }
        }
    }
}
