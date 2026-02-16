//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

#region PDF sharp reference and liscense - empira Software GmbH, Troisdorf (Germany)
/*http://www.pdfsharp.net/
PDFsharp is published under the MIT License.

Copyright(c) 2005 - 2014 empira Software GmbH, Troisdorf (Germany)

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

PDFsharp is Open Source.
You can copy, modify and integrate the source code of PDFsharp in your application without restrictions at all.
This also applies to commercial products (both open source and closed source).
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enrichment;
//using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Common_functions.Options_base;
using Common_functions.Text;
using System.IO;
using ZedGraph;
using System.Web;
using System.Drawing.Imaging;
using Network;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;
using Windows_forms;
using ClassLibrary1.OrganizeData_userInterface;
using System.Windows.Controls;
using PdfSharp.Pdf.Content.Objects;

namespace Result_visualization
{
    enum Draw_geometrics_enum { E_m_p_t_y, Rectangle, Top_line, Left_line, None };
    enum Charts_per_page_enum { Low, Medium, High }

    abstract class Visualization_base_class
    {
        public bool PDF_generation_allowed { get; set; }
        public ProgressReport_interface_class ProgressReport { get; set; }
        private Dictionary<string, string> FullScpName_shortenedSCPName_dict { get; set; }

        public Visualization_base_class(ProgressReport_interface_class progressReport)
        {
            PDF_generation_allowed = true;
            this.ProgressReport = progressReport;
            FullScpName_shortenedSCPName_dict = new Dictionary<string, string>();
            Fill_fullScpName_shortenedSCPName_dict();
        }

        public abstract void Set_PDF_generation_allowed(bool pdf_generation_allowed);

        private void Write_memory_exception_and_set_pdf_generation_allowed_to_false(string directory)
        {
            string text_to_be_written = "PDF generation stopped, most likely due to insufficient memory, potentially caused by iterative generation of PDFs."
                                         + "\r\nTo generate PDFs, please close and restart the application and try again."
                                         + "\r\nData and parameter settings can be reloaded from the results folder 'Input_data' using the menu panel 'Read data'.";
            string complete_fileName = directory + "Generation_of_PDFs_aborted.txt";
            StreamWriter writer = new StreamWriter(complete_fileName);
            writer.WriteLine("Generation of PDF was stopped, most likely due to insufficient memory.");
            writer.WriteLine("This could be caused by iterative generation of multiple PDFs.");
            writer.WriteLine("To generate PDFs, please close and restart the application and try again.");
            writer.WriteLine("Data and parameter settings can be reloaded from the results folder 'Input_data' using the menu panel 'Read data'.");
            writer.WriteLine("Alternatively, select a different file type for the visualization of the reults in the menu panel 'Enrichment'.");
            writer.Close();
            ProgressReport.Write_temporary_announcement_and_restore_progressReport(text_to_be_written, 10);
            PDF_generation_allowed = false;
        }

        private string Get_clean_string(string input_string)
        {
            if (input_string.IndexOf("translational protein modification and quality control during biosynthetic")!=-1)
            {
                string lol = "";
            }
            string clean = (string)input_string.Clone();
            clean = clean.Normalize(NormalizationForm.FormKC);
            clean = clean.Replace('\u2013', ' ')  // en dash
                         .Replace('\u2014', ' ')  // em dash
                         .Replace('\u2212', ' ')  // minus sign
                         .Replace('\u002D', ' '); // hyphen-minus (already ASCII)
            return clean;
        }

        private void Fill_fullScpName_shortenedSCPName_dict()
        {
            FullScpName_shortenedSCPName_dict.Add("Post-translational protein modification and quality control during biosynthetic-secretory pathway", "PT protein modification & QC during biosynthetic-secretory pathway");
            FullScpName_shortenedSCPName_dict.Add("Endoplasmic reticulum and nuclear envelope organization", "ER & nuclear envelope organization");
            FullScpName_shortenedSCPName_dict.Add("Coagulation, fibrinolysis, complement system and blood protein dynamics", "Coag., fib., compl. system and blood protein dynamics");
            FullScpName_shortenedSCPName_dict.Add("CCN intercellular signaling protein family receptor signaling", "CCN family receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("mRNA degradation, storing and translational repression by cytoplasmic processing bodies", "mRNA deg., storing and translational repr. by cyt. processing bodies");
            FullScpName_shortenedSCPName_dict.Add("Neuronal membrane repolarization during action potential and hyperpolarization", "Neuronal membrane repolarization and hyperpolarization");
            FullScpName_shortenedSCPName_dict.Add("Extracellular matrix breakdown and membrane shedding by adamalysins", "ECM breakdown & membrane shedding by adamalysins");
            FullScpName_shortenedSCPName_dict.Add("Inhibition of amyloid aggregation, amyloid degradation and uptake", "Amyloid degradation & inhibition of amyloid uptake/aggregation");
            FullScpName_shortenedSCPName_dict.Add("Signaling pathways that control cell proliferation and differentiation", "Sig. pathways that control cell prol. and diff.");
            FullScpName_shortenedSCPName_dict.Add("Signaling pathways regulating steroid and sex hormone synthesis", "Sig. pathways regulating steroid and sex hormone synthesis");
            FullScpName_shortenedSCPName_dict.Add("Transmembrane water and ion transport not involved in membrane potential generatation", "TM H2O/ion transport not involved in membrane potential generation");
            FullScpName_shortenedSCPName_dict.Add("Cardiomyocyte repolarization during action potential and hyperpolarization", "Cardiomyocyte repolarization and hyperpolarization");
            FullScpName_shortenedSCPName_dict.Add("Clathrin-mediated vesicle traffic from TGN to endosomal lysosomal system", "Clathrin-mediated vesicle traffic from TGN to endolysosomal system");
            FullScpName_shortenedSCPName_dict.Add("Transmembrane ion transport involved in membrane potential generation", "TM ion transport involved in membrane potential generation");
            FullScpName_shortenedSCPName_dict.Add("Membrane transport of small molecules and electrical properties of membranes", "TM transport of small molecules & electrical properties of membranes");
            FullScpName_shortenedSCPName_dict.Add("Granulocyte macrophage colony-stimulating factor receptor signaling", "GM-CSF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Granulocyte-colony stimulating factor receptor signaling", "GCSF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Adrenocorticotropic hormone receptor signaling","ACTH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Platelet−derived growth factor receptor signaling", "PDGF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Antidiuretic hormone receptor signaling", "ADH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Epidermal growth factor receptor signaling","EGFR signaling");
            FullScpName_shortenedSCPName_dict.Add("Bone morphogenetic protein receptor signaling","BMP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Corticotropin-releasing hormone receptor signaling", "CRH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Fibroblast growth factor receptor signaling", "FGFR signaling");
            FullScpName_shortenedSCPName_dict.Add("Vascular endothelial growth factor receptor signaling", "VEGF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Brain derived neurotrophic factor receptor signaling","BDNF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Growth differentiation factor receptor signaling","GDF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Follicle stimulating hormone receptor signaling","FSH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Gastric inhibitory polypeptide receptor signaling","GIP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Gastrin-releasing peptide receptor signaling","GRP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Glucagon-like peptide-1 receptor signaling", "GLP-1 receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Growth hormone receptor signaling", "GH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Heparin-binding EGF-like growth factor receptor signaling","HB-EGF-like GF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Hepatocyte growth factor receptor signaling","HGF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Hyaluronan-mediated motility receptor signaling","RHAMM signaling");
            FullScpName_shortenedSCPName_dict.Add("Insulin-like growth factor receptor signaling","IGF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Interferon alpha receptor signaling", "IFN-alpha receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Interferon beta receptor signaling", "IFN-beta receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Interferon gamma receptor signaling", "IFN-gamma receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Post-translational protein modification in Mitochondria", "PT protein modification in mitochondria");
            FullScpName_shortenedSCPName_dict.Add("Phosphatidylcholine and phosphatidylethanolamine biosynthesis via Kennedy pathway", "PtdCho & PtdEth biosynthesis via Kennedy pathway");
            FullScpName_shortenedSCPName_dict.Add("Phosphatidylcholine, phoshpatidylethanolamine and phosphatidylserine interconversion","PtdCho, PtdEth & PtdSer interconversion");
            FullScpName_shortenedSCPName_dict.Add("Phosphatidylinositol biosynthesis","PtdIns biosynthesis");
            FullScpName_shortenedSCPName_dict.Add("Leukemia inhibitory factor receptor signaling","LIF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Luteinizing hormone hormone receptor signaling","LH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Macrophage migration inhibitory factor signaling","MIF signaling");
            FullScpName_shortenedSCPName_dict.Add("Mammalian target of rapamycin signaling pathway","mTOR signaling pathway");
            FullScpName_shortenedSCPName_dict.Add("Nerve growth factor receptor signaling","NGF receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Vesicle traffic between TGN and endosomal lysosomal system", "Vesicle traffic between TGN & endosomal lysosomal system");
            FullScpName_shortenedSCPName_dict.Add("Target RNA degradation/inhibition/destabilization by RICS or RITS", "");
            FullScpName_shortenedSCPName_dict.Add("Co-translational translocation membrane protein insertion and import", "Co-translational membrane protein insertion and import");
            FullScpName_shortenedSCPName_dict.Add("Cytosolic Dicer-mediated microRNA and endogenous siRNA processing", "Cytosolic Dicer-mediated microRNA & endogenous siRNA processing");
            FullScpName_shortenedSCPName_dict.Add("Metabolism and transport of cholesterol, steroids and bile acids", "Metabolism and transport of cholesterol, steroids & bile acids");
            //FullScpName_shortenedSCPName_dict.Add("Oncostatin-M receptor signaling","OSM receptor signaling");
            //FullScpName_shortenedSCPName_dict.Add("Osteonectin receptor signaling","ON receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Parathyroid hormone receptor signaling","PTH receptor signaling");
            //FullScpName_shortenedSCPName_dict.Add("Osteopontin receptor signaling","OPN receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Peroxisome proliferator-activated receptor alpha signaling", "PPAR alpha signaling");
            FullScpName_shortenedSCPName_dict.Add("Peroxisome proliferator-activated receptor beta signaling", "PPAR beta signaling");
            FullScpName_shortenedSCPName_dict.Add("Peroxisome proliferator-activated receptor gamma signaling", "PPAR gamma signaling");
            FullScpName_shortenedSCPName_dict.Add("Prostaglandin D2 receptor signaling", "PGD2 receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Prostaglandin E2 receptor signaling", "PGE2 receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Endoplasmic reticulum membrane protein insertion and import","ER membrane protein insertion and import");
            FullScpName_shortenedSCPName_dict.Add("Prostaglandin PGF2 alpha receptor signaling","PGF2 alpha receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Protein kinase C signaling pathway","PKC signaling pathway");
            FullScpName_shortenedSCPName_dict.Add("Recognition of lysosomal enzymes by mannose 6-phosphate receptor","Recognition of lysosomal enzymes by M6P receptor");
            FullScpName_shortenedSCPName_dict.Add("Thyroid-stimulating hormone receptor signaling","TSH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Thyrotropin-releasing hormone receptor signaling","TRH receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Transforming growth factor alpha receptor signaling", "TGF alpha receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Transforming growth factor beta receptor signaling", "TGF beta receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Tumor necrosis factor alpha receptor signaling", "TNF alpha receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Tumor necrosis factor beta receptor signaling", "TNF beta receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Vasoactive intestinal peptide receptor signaling","VIP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Atrial natriuretic peptide receptor signaling","ANP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Brain natriuretic peptide receptor signaling","BNP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("C-type natriuretic peptide receptor signaling","CNP receptor signaling");
            FullScpName_shortenedSCPName_dict.Add("Endoplasmic reticulum three way junctions stabalization","ER three way junctions stabilization");
            FullScpName_shortenedSCPName_dict.Add("Endoplasmic reticulum tubular membrane shaping", "ER tubular membrane shaping");
            FullScpName_shortenedSCPName_dict.Add("Endoplasmic reticulum tubulus maintenance", "ER tubulus maintenance");
            FullScpName_shortenedSCPName_dict.Add("Amination of alpha-keto acids and deamination of amino acids", "Amination of a-keto acids and deamination of amino acids");
            //FullScpName_shortenedSCPName_dict.Add("Nodal growth differentiation factor receptor signaling","Nodal growth differentiation factor receptor signaling");

            string[] fullScpNames = FullScpName_shortenedSCPName_dict.Keys.ToArray();
            string clean;
            foreach (string fullScpName in fullScpNames)
            {
                clean = Get_clean_string(fullScpName);
                if (!FullScpName_shortenedSCPName_dict.ContainsKey(clean))
                {
                    FullScpName_shortenedSCPName_dict.Add(clean, FullScpName_shortenedSCPName_dict[fullScpName]);
                }
            }


            MBCO_obo_network_class mbco_network_get_all_scps = new MBCO_obo_network_class(Ontology_type_enum.Mbco, Enrichment.SCP_hierarchy_interaction_type_enum.Parent_child, Organism_enum.Homo_sapiens);
            mbco_network_get_all_scps.Generate_by_reading_safed_spreadsheet_file_or_obo_file_add_missing_scps_if_custom_add_human_processSizes_and_return_if_not_interrupted(ProgressReport, out bool not_interrupted);
            string[] all_scps = mbco_network_get_all_scps.Get_all_scps();
            string scp;
            string shortened_scp_name;
            int all_scps_length = all_scps.Length;
            for (int indexS=0; indexS<all_scps_length; indexS++)
            {
                scp = all_scps[indexS];
                shortened_scp_name = (string)scp.Clone();
                shortened_scp_name = shortened_scp_name.Replace("Peroxisome proliferator-activated receptor", "PPAR");
                shortened_scp_name = shortened_scp_name.Replace("peroxisome proliferator-activated receptor", "PPAR");
                shortened_scp_name = shortened_scp_name.Replace("Epidermal growth factor receptor", "EGFR");
                shortened_scp_name = shortened_scp_name.Replace("epidermal growth factor receptor", "EGFR");
                shortened_scp_name = shortened_scp_name.Replace("Epidermal growth factor", "EGF");
                shortened_scp_name = shortened_scp_name.Replace("epidermal growth factor", "EGF");
                shortened_scp_name = shortened_scp_name.Replace("Transmembrane", "TM");
                shortened_scp_name = shortened_scp_name.Replace("transmembrane", "TM");
                shortened_scp_name = shortened_scp_name.Replace("Interferon", "IFN");
                shortened_scp_name = shortened_scp_name.Replace("interferon", "IFN");
                //shortened_scp_name = shortened_scp_name.Replace("Interleukin ", "IL ");
                shortened_scp_name = shortened_scp_name.Replace("Membrane transport", "TMT");
                shortened_scp_name = shortened_scp_name.Replace("membrane transport", "TMT");
                shortened_scp_name = shortened_scp_name.Replace("small molecules", "small mol.");
                shortened_scp_name = shortened_scp_name.Replace("Extracellular matrix", "ECM");
                shortened_scp_name = shortened_scp_name.Replace("extracellular matrix", "ECM");
                //shortened_scp_name = shortened_scp_name.Replace("Cardiomyocyte", "CM");
                //shortened_scp_name = shortened_scp_name.Replace("cardiomyocyte", "CM");
                shortened_scp_name = shortened_scp_name.Replace("action potential", "AP");
                shortened_scp_name = shortened_scp_name.Replace("Phosphatidylcholine", "PtdCho");
                shortened_scp_name = shortened_scp_name.Replace("phosphatidylcholine", "PtdCho");
                shortened_scp_name = shortened_scp_name.Replace("Phosphatidylethanolamine", "PtdEth");
                shortened_scp_name = shortened_scp_name.Replace("phosphatidylethanolamine", "PtdEth");
                shortened_scp_name = shortened_scp_name.Replace("Phoshpatidylethanolamine", "PtdEth");
                shortened_scp_name = shortened_scp_name.Replace("phosphatidylserine", "PtdSer");
                shortened_scp_name = shortened_scp_name.Replace("Phosphatidylserine", "PtdSer");
                if ((!scp.Equals(shortened_scp_name))&&(!FullScpName_shortenedSCPName_dict.ContainsKey(scp)))
                {
                    //FullScpName_shortenedSCPName_dict.Add(scp, shortened_scp_name);
                }
            }
        }

        public void Write_scp_abbreviations(string directory, string fileName)
        {
            string complete_fileName = directory + fileName;
            StreamWriter writer = new StreamWriter(complete_fileName);
            string[] scps = FullScpName_shortenedSCPName_dict.Keys.ToArray();
            char delimiter = Global_class.Tab;
            writer.WriteLine("{1}{0}{2}", delimiter, "SCP abbreviation", "SCP");
            foreach (string scp in scps)
            {
                writer.WriteLine("{1}{0}{2}", delimiter, FullScpName_shortenedSCPName_dict[scp], scp);
            }
            writer.Close();
        }

        protected double[] Get_potential_inner_tick_distances_for_minusLog10Pvalues()
        {
            return new double[] { 1, 2, 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000 };
        }
        protected double[] Get_potential_inner_tick_distances_for_time(bool is_log10_scale)
        {
            if (is_log10_scale)
            {
                return new double[] { 0.01, 0.05, 0.1, 0.5, 1, 5, 10, 50, 100, 500, 1000, 5000, 10000 };
            }
            else
            {
                return new double[] { 1, 2, 5, 10, 20, 50, 60, 100, 120, 200, 240, 250, 500, 1000, 2000, 2500, 5000, 10000 };
            }
        }

        protected double Get_inner_tick_distance(double max_value, double min_value, int inner_ticks_count, double[] potential_inner_tick_distances)
        {
            double calculated_inner_tick_distance = (max_value - min_value) / (float)inner_ticks_count;
            double potential_inner_tick_distance;
            int potential_inner_tick_distances_length = potential_inner_tick_distances.Length;
            int indexOfShortestDistance = -1;
            double minimum_deviation = -1;
            double current_deviation;
            for (int indexP = 0; indexP < potential_inner_tick_distances_length; indexP++)
            {
                potential_inner_tick_distance = potential_inner_tick_distances[indexP];
                current_deviation = Math.Abs(calculated_inner_tick_distance - potential_inner_tick_distance);
                if (  (minimum_deviation==-1)
                    ||(current_deviation < minimum_deviation))
                {
                    indexOfShortestDistance = indexP;
                    minimum_deviation = current_deviation;
                }
            }
            return potential_inner_tick_distances[indexOfShortestDistance];
        }

        protected double Get_inner_tick_distance(double max_value, int inner_ticks_count, double[] potential_inner_tick_distances)
        {
            return Get_inner_tick_distance(max_value, 0, inner_ticks_count, potential_inner_tick_distances);
        }

        protected double Get_inner_tick_distance_for_indicated_set_of_values(double[] timepoints, int inner_tick_counts)
        {
            timepoints = timepoints.OrderBy(l => l).ToArray();
            int timepoints_length = timepoints.Length;
            double current_timepoint = timepoints[1];
            double previous_timepoint = timepoints[0];
            double smallest_distance = -1;
            for (int indexT = 1; indexT < timepoints_length; indexT++)
            {
                current_timepoint = timepoints[indexT];
                previous_timepoint = timepoints[indexT - 1];
                if ((smallest_distance == -1) || (current_timepoint - previous_timepoint < smallest_distance))
                {
                    smallest_distance = current_timepoint - previous_timepoint;
                }
            }

            bool unique_distance = true;
            int factor = 1;
            for (int indexT = 1; indexT < timepoints_length; indexT++)
            {
                current_timepoint = timepoints[indexT];
                previous_timepoint = timepoints[indexT - 1];
                factor = 1;
                while ((current_timepoint - previous_timepoint > smallest_distance * factor))
                {
                    factor++;
                }
                if (current_timepoint - previous_timepoint != smallest_distance * factor)
                {
                    unique_distance = false;
                }
            }
            if (unique_distance)
            {
                factor = 1;
                double timerange = timepoints[timepoints_length - 1] - timepoints[0];
                while (timerange / (smallest_distance * (double)factor) > inner_tick_counts)
                {
                    factor++;
                }
                return factor * smallest_distance;
            }
            else
            {
                timepoints = timepoints.OrderByDescending(l => l).ToArray();
                double max_value = (double)timepoints[0];
                double min_value = (double)timepoints[timepoints.Length - 1];
                return Get_inner_tick_distance(max_value, min_value, inner_tick_counts, new double[] { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000 });
            }

        }

        protected string Get_shortened_scp_name(string scp)
        {
            string shortened_scp;
            string clean = Get_clean_string(scp);
            if (FullScpName_shortenedSCPName_dict.ContainsKey(clean))
            {
                shortened_scp = FullScpName_shortenedSCPName_dict[clean];
            }
            else { shortened_scp = scp; }
            return shortened_scp;
        }

        protected string[] Get_shortened_scp_names(string[] scps)
        {
            int scps_length = scps.Length;
            for (int indexScp = 0; indexScp < scps_length; indexScp++)
            {
                scps[indexScp] = Get_shortened_scp_name(scps[indexScp]);
            }
            return scps;
        }

        protected bool Analyze_if_dynamic_enrichment(Ontology_enrichment_line_class[] enrichment_lines)
        {
            bool is_dynamic_enrichment = false;
            foreach (Ontology_enrichment_line_class enrich_line in enrichment_lines)
            {
                if (enrich_line.Scp_name.IndexOf('$') != -1)
                {
                    is_dynamic_enrichment = true;
                    break;
                }
            }
            return is_dynamic_enrichment;
        }

        private MasterPane Set_masterPane_layout_and_size(MasterPane masterPane, int graphPanes_for_each_masterPane, float width_each_graphPane)
        {
            int graphPanes_count = masterPane.PaneList.Count;
            int rows_count = -1;
            int cols_count = -1;
            if (graphPanes_for_each_masterPane == 1)
            {
                rows_count = 1;
                cols_count = 1;
            }
            else if (graphPanes_for_each_masterPane == 2)
            {
                rows_count = 2;
                cols_count = 1;
            }
            else if (graphPanes_for_each_masterPane <= 4)
            {
                rows_count = 2;
                cols_count = 2;
            }
            else if (graphPanes_for_each_masterPane <= 6)
            {
                rows_count = 3;
                cols_count = 2;
            }
            else if (graphPanes_for_each_masterPane <= 8)
            {
                rows_count = 4;
                cols_count = 2;
            }
            else if (graphPanes_for_each_masterPane <= 12)
            {
                rows_count = 4;
                cols_count = 3;
            }
            else if (graphPanes_for_each_masterPane <= 15)
            {
                rows_count = 5;
                cols_count = 3;
            }
            else if (graphPanes_for_each_masterPane <= 20)
            {
                rows_count = 5;
                cols_count = 4;
            }
            else if (graphPanes_for_each_masterPane <= 30)
            {
                rows_count = 6;
                cols_count = 5;
            }
            else if (graphPanes_for_each_masterPane <= 36)
            {
                rows_count = 6;
                cols_count = 6;
            }
            else
            {
                throw new Exception();
            }
            System.Windows.Forms.Control control = new System.Windows.Forms.Control();
            float width_graphPane = width_each_graphPane * cols_count;
            float height_graphPane = width_graphPane * 1.414F;
            using (Graphics g = control.CreateGraphics())
            {
                masterPane.Rect = new RectangleF(0, 0, width_graphPane, height_graphPane);
                masterPane.SetLayout(g, rows_count, cols_count);
            }
            return masterPane;
        }

        protected GraphPane[] Get_all_graphPanes_from_masterPanes(MasterPane[] masterPanes)
        {
            List<GraphPane> graphPanes = new List<GraphPane>();
            foreach (MasterPane masterPane in masterPanes)
            {
                graphPanes.AddRange(masterPane.PaneList);
            }
            return graphPanes.ToArray();
        }
        protected void Add_masterPanes_to_pdf(ref PdfDocument pdf_document, List<MasterPane> masterPanes, int graphPanes_for_each_masterPane, float width_of_singelGraphPane)
        {
            int masterPanes_length = masterPanes.Count;
            if (masterPanes_length == 0) { throw new Exception(); }
            if (PDF_generation_allowed)
            {
                // Create an empty page
                MasterPane masterPane;
                for (int indexMP = 0; indexMP < masterPanes_length; indexMP++)
                {
                    masterPane = masterPanes[indexMP];
                    if (masterPane.PaneList.Count > 0)
                    {
                        masterPane = Set_masterPane_layout_and_size(masterPane, graphPanes_for_each_masterPane, width_of_singelGraphPane);
                        PdfPage page = pdf_document.AddPage();
                        page.Size = PdfSharp.PageSize.A4;
                        using (XGraphics xgr = XGraphics.FromPdfPage(page))
                        {
                            int width_int = (int)Math.Round(masterPane.Rect.Width);
                            int height_int = (int)Math.Round(masterPane.Rect.Height);
                            float dpi = 250;
                            bool isAntiAlias = true;

                            Bitmap bmp = masterPane.GetImage(width_int, height_int, dpi, isAntiAlias);
                            bmp.SetResolution(dpi, dpi);
                            XImage img = XImage.FromGdiPlusImage(bmp);
                            xgr.DrawImage(img, 0, 0, page.Width, page.Height);
                        }
                    }
                }
            }
        }

        protected void Write_and_dispose_pdf_document_if_allowed(ref PdfDocument pdf_document, string completeFileName)
        {
            if (PDF_generation_allowed)
            {
                bool pdf_saved = false;
                string warning_text;
                while (!pdf_saved)
                {
                    try
                    {
                        pdf_document.Save(completeFileName);
                        pdf_saved = true;
                    }
                    catch
                    {
                        warning_text = ReadWriteOptions_base.Save_file_in_use_warning_text_start + completeFileName;
                        ProgressReport.Write_warning_and_save_current_entry(warning_text);
                    }
                }
                ProgressReport.Restore_and_delete_lastEntry_if_not_emtpy();
            }
            pdf_document.Dispose();
            pdf_document = null;
        }

        protected void Write_masterPane(MasterPane masterPane, string completeFileName, System.Drawing.Imaging.ImageFormat chartImageFormat, int graphPanes_for_each_masterPane, float width_of_singelGraphPane)
        {
            masterPane = Set_masterPane_layout_and_size(masterPane, graphPanes_for_each_masterPane, width_of_singelGraphPane);

            bool image_saved = false;
            string warning_text;
            while (!image_saved)
            {
                try
                {
                    int width_int = (int)Math.Round(masterPane.Rect.Width);
                    int height_int = (int)Math.Round(masterPane.Rect.Height);
                    float dpi = 250;
                    bool isAntiAlias = true;

                    masterPane.GetImage(width_int, height_int, dpi, isAntiAlias).Save(completeFileName, chartImageFormat);
                    image_saved = true;
                }
                catch
                {
                    warning_text = ReadWriteOptions_base.Save_file_in_use_warning_text_start + completeFileName;
                    ProgressReport.Write_warning_and_save_current_entry(warning_text);
                }
            }
            ProgressReport.Restore_and_delete_lastEntry_if_not_emtpy();
        }
        protected GraphPane Generate_empty_graphPane()
        {
            GraphPane empty_graphPane = new GraphPane();
            empty_graphPane.XAxis.IsVisible = false;
            empty_graphPane.X2Axis.IsVisible = false;
            empty_graphPane.YAxis.IsVisible = false;
            empty_graphPane.Y2Axis.IsVisible = false;
            empty_graphPane.XAxis.MajorGrid.IsVisible = false;
            empty_graphPane.XAxis.MinorGrid.IsVisible = false;
            empty_graphPane.X2Axis.MajorGrid.IsVisible = false;
            empty_graphPane.X2Axis.MinorGrid.IsVisible = false;
            empty_graphPane.YAxis.MajorGrid.IsVisible = false;
            empty_graphPane.YAxis.MinorGrid.IsVisible = false;
            empty_graphPane.Y2Axis.MajorGrid.IsVisible = false;
            empty_graphPane.Y2Axis.MinorGrid.IsVisible = false;
            empty_graphPane.Border.IsVisible = false;
            empty_graphPane.Chart.Border.IsVisible = false;
            return empty_graphPane;
        }

    }

    class Bardiagram_options_class : Options_readWrite_base_class
    {
        public bool Generate_bardiagrams { get; set; }
        public bool Customized_colors { get; set; }
        public bool Use_scp_abbreviations { get; set; }
        public float Fraction_of_used_xspace_on_pdf_page { get; set; }
        public float Fraction_of_used_yspace_on_pdf_page { get; set; }
        public int Resolution_factor { get; set; }
        public int Figures_per_chart
        {
            get
            {
                switch (Charts_per_page)
                {
                    case Charts_per_page_enum.High:
                        return 8;
                    case Charts_per_page_enum.Medium:
                        return 4;
                    case Charts_per_page_enum.Low:
                        return 1;
                    default:
                        throw new Exception();
                }
            }
        }
        public string Bardiagram_subdirectory { get; set; }
        public float Width_of_singleGraphPane { get; set; }
        public Charts_per_page_enum Charts_per_page { get; set; }
        public System.Drawing.Imaging.ImageFormat ImageFormat { get; set; }
        public int Max_lines_per_bardiagram_for_standard_enrichment { get; set; }
        public int Max_lines_per_bardiagram_for_dynamic_enrichment { get; set; }
        public int Max_character_for_scp_in_bardigram { get; set; }
        public int Max_character_for_chartName_in_bardigram { get; set; }
        public int Max_lines_for_chartName_in_bardigram { get; set; }
        public string Image_fileName_extension
        {
            get
            {
                if (Write_pdf) { return ".pdf"; }
                else { return "." + ImageFormat.ToString().ToLower(); }
            }
        }
        public bool Write_pdf { get; set; }

        public int Get_max_lines_per_bardiagram(Enrichment_algorithm_enum enrichment_algorithm)
        {
            switch (enrichment_algorithm)
            {
                case Enrichment_algorithm_enum.Dynamic_enrichment:
                    return Max_lines_per_bardiagram_for_dynamic_enrichment;
                case Enrichment_algorithm_enum.Standard_enrichment:
                    return Max_lines_per_bardiagram_for_standard_enrichment;
                default:
                    throw new Exception();
            }
        }

        public override void Write_option_entries(StreamWriter writer)
        {
            base.Write_entries_excluding_dictionaries(writer, typeof(Bardiagram_options_class), "Fraction_of_used_xspace_on_pdf_page",
                                                                        "Fraction_of_used_yspace_on_pdf_page", "Resolution_factor",
                                                                        "Max_lines_per_bardiagram_for_standard_enrichment",
                                                                        "Width_of_singleGraphPane", "Bardiagram_subdirectory",
                                                                        "Use_scp_abbreviations",
                                                                        "Max_lines_per_bardiagram_for_dynamic_enrichment",
                                                                        "ImageFormat"
                                                                        );
        }
        public override bool Add_read_entry_to_options_and_return_if_successful(string readLine)
        {
            return base.Add_read_entry_and_return_if_succesful(readLine, typeof(Bardiagram_options_class));
        }


        public Bardiagram_options_class()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            Width_of_singleGraphPane = 2000;//3000
            Resolution_factor = 80;
            Max_character_for_scp_in_bardigram = 90;
            Max_lines_for_chartName_in_bardigram = 2;
            Max_character_for_chartName_in_bardigram = 70;
            Customized_colors = false;
            Use_scp_abbreviations = true;
            Max_lines_per_bardiagram_for_standard_enrichment = 28;
            Max_lines_per_bardiagram_for_dynamic_enrichment = 32;// 32;
            Generate_bardiagrams = true;
            Fraction_of_used_xspace_on_pdf_page = 0.9F;
            Fraction_of_used_yspace_on_pdf_page = 0.85F;
            Charts_per_page = Charts_per_page_enum.High;
            ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            Bardiagram_subdirectory = "Bardiagrams" + gdf.Delimiter;
        }

        public Bardiagram_options_class Deep_copy()
        {
            Bardiagram_options_class copy = (Bardiagram_options_class)this.MemberwiseClone();
            copy.Bardiagram_subdirectory = (string)this.Bardiagram_subdirectory.Clone();
            return copy;
        }
    }

    class Bardiagram_class : Visualization_base_class
    {
        public Bardiagram_options_class Options { get; set; }

        public Bardiagram_class(ProgressReport_interface_class progressReport) : base(progressReport)
        {
            this.Options = new Bardiagram_options_class();
        }
        private void Add_enrichment_results_to_graph_variables(ref Dictionary<int, List<double>> level_pointPairList, ref List<string> scp_labels, Ontology_type_enum ontology, int scp_level, string scp_name, float minusLog10Pvalue)
        {
            int level_for_bardiagram = -1;
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                    level_for_bardiagram = 1;
                    break;
                case Ontology_type_enum.Mbco:
                    level_for_bardiagram = scp_level;
                    break;
                case Ontology_type_enum.E_m_p_t_y:
                    level_for_bardiagram = scp_level;
                    break;
                default:
                    throw new Exception();
            }


            for (int level = 0; level <= 4; level++)
            {
                if (!level_pointPairList.ContainsKey(level))
                {
                    level_pointPairList.Add(level, new List<double>());
                }
                if (level == level_for_bardiagram) { level_pointPairList[level_for_bardiagram].Add(minusLog10Pvalue); }
                else { level_pointPairList[level].Add(0); }
            }
            scp_labels.Add(scp_name);
        }
        private void Add_blank_enrichment_results_to_graph_variables(ref Dictionary<int, List<double>> level_pointPairList, ref List<string> scp_labels)
        {
            Add_enrichment_results_to_graph_variables(ref level_pointPairList, ref scp_labels, Ontology_type_enum.E_m_p_t_y, 0, "", 0);
        }
        private GraphPane Generate_bardiagram_graphPane_for_sameDataset_enrich_lines(Ontology_enrichment_line_class[] sameDataset_enrich_lines, int current_lines_in_bardiagram, string add_to_uniqueDatasetName, float max_minusLog10Pvalue, bool is_dynamic_enrichment, int max_lines_per_bardiagram)
        {
            string uniqueDatasetName = sameDataset_enrich_lines[0].Unique_dataset_name;
            Ontology_type_enum ontology = sameDataset_enrich_lines[0].Ontology_type;
            int enrich_length = sameDataset_enrich_lines.Length;
            Ontology_enrichment_line_class enrich_line;

            Color dataset_color = sameDataset_enrich_lines[0].Sample_color;
            //sameDataset_enrich_lines = sameDataset_enrich_lines.OrderByDescending(l => l.ProcessLevel).ThenBy(l => l.Minus_log10_pvalue).ToArray();
            sameDataset_enrich_lines = Ontology_enrichment_line_class.Order_by_descendingProcessLevel_minusLog10Pvalue(sameDataset_enrich_lines);


            int numbers_in_max_minusLog10Pvalue = (int)Math.Round(max_minusLog10Pvalue).ToString().Length;

            int resolution_factor = Math.Max(1, Options.Resolution_factor / 2);

            Dictionary<int, Color> level_color_dict = Global_class.Get_level_scpColor_dict();
            Dictionary<Ontology_type_enum, Color> ontology_color_dict = Global_class.Get_ontology_scpColor_dict();

            string chart_name = uniqueDatasetName + add_to_uniqueDatasetName;
            if (chart_name.Length> Options.Max_character_for_chartName_in_bardigram)
            {
                chart_name = Text_class.Split_texts_over_multiple_lines(new string[] { chart_name }, Options.Max_character_for_chartName_in_bardigram, Options.Max_lines_for_chartName_in_bardigram)[0];
            }


            GraphPane bardiagram_graphPane = new GraphPane();
            bardiagram_graphPane.Margin.All = 1;
            bardiagram_graphPane.Border.Color = Color.White;
            bardiagram_graphPane.Title.Text = (string)chart_name.Clone();
            bardiagram_graphPane.Title.FontSpec = new FontSpec("Arial", 14, Color.Black, true, false, false);
            bardiagram_graphPane.Title.FontSpec.Border.IsVisible = false;
            float fontSize = -1;

            switch (Options.Charts_per_page)
            {
                case Charts_per_page_enum.Low:
                    fontSize = 16;
                    break;
                case Charts_per_page_enum.Medium:
                    fontSize = 15;
                    break;
                case Charts_per_page_enum.High:
                    fontSize = 10;
                    break;
            }

            if (is_dynamic_enrichment)
            {
                bardiagram_graphPane.BarSettings.MinClusterGap = 0F;
                fontSize = fontSize * 0.80F;
            }
            else
            {
                bardiagram_graphPane.BarSettings.MinClusterGap = 0.2F;
            }
            bardiagram_graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", fontSize, Color.Black, false, false, false);
            bardiagram_graphPane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Far;
            bardiagram_graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
            bardiagram_graphPane.YAxis.Scale.FontSpec.Angle = 90;
            bardiagram_graphPane.YAxis.Title.Text = "";
            bardiagram_graphPane.XAxis.Title.Text = "-log10(p)";
            bardiagram_graphPane.XAxis.Title.FontSpec = new FontSpec("Arial", 14, Color.Black, false, false, false);
            bardiagram_graphPane.XAxis.Title.FontSpec.Border.IsVisible = false;
                
            bardiagram_graphPane.Chart.Border.IsVisible = false;
            bardiagram_graphPane.BarSettings.ClusterScaleWidth = 0.5F;

            List<string> sameLevel_scps = new List<string>();
            List<float> sameLevel_minusLog10Pvalues = new List<float>();

            string[] scp_names;
            string scp_name;
            int scp_names_length;
            int max_scps_per_dynamic = 3;
            int empty_lines_count = max_lines_per_bardiagram - current_lines_in_bardiagram;
            Dictionary<int, List<double>> level_valuesList_dict = new Dictionary<int, List<double>>();
            List<string> scp_labels = new List<string>();
            Dictionary<int, List<double>> extra_blank_level_valuesList_dict = new Dictionary<int, List<double>>();
            List<string> extra_blank_scp_labels = new List<string>();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = sameDataset_enrich_lines[indexE];
                if (!enrich_line.Unique_dataset_name.Equals(uniqueDatasetName)) { throw new Exception(); }
                if ((indexE > 0) && (is_dynamic_enrichment))
                {
                    Add_blank_enrichment_results_to_graph_variables(ref level_valuesList_dict, ref scp_labels);
                }
                if ((indexE != 0) && (!enrich_line.ProcessLevel.Equals(sameDataset_enrich_lines[indexE - 1].ProcessLevel)))
                {
                    Add_blank_enrichment_results_to_graph_variables(ref level_valuesList_dict, ref scp_labels);
                }
                scp_names = enrich_line.Scp_name.Split('$');
                scp_names_length = scp_names.Length;
                if ((is_dynamic_enrichment) && (scp_names_length < 3))
                {
                    Add_enrichment_results_to_graph_variables(ref level_valuesList_dict, ref scp_labels, enrich_line.Ontology_type, enrich_line.ProcessLevel, "", enrich_line.Minus_log10_pvalue);
                }
                for (int indexScp = 0; indexScp < scp_names_length; indexScp++)
                {
                    scp_name = scp_names[indexScp];
                    Add_enrichment_results_to_graph_variables(ref level_valuesList_dict, ref scp_labels, enrich_line.Ontology_type, enrich_line.ProcessLevel, scp_name, enrich_line.Minus_log10_pvalue);
                }
                if ((is_dynamic_enrichment) && (max_scps_per_dynamic - scp_names_length > 1))
                {
                    Add_enrichment_results_to_graph_variables(ref level_valuesList_dict, ref scp_labels, enrich_line.Ontology_type, enrich_line.ProcessLevel, "", enrich_line.Minus_log10_pvalue);
                }
            }

            while (extra_blank_scp_labels.Count < max_lines_per_bardiagram - scp_labels.Count)
            {
                Add_blank_enrichment_results_to_graph_variables(ref extra_blank_level_valuesList_dict, ref extra_blank_scp_labels);
            }

            #region Stacked bardiagram
            int[] levels = level_valuesList_dict.Keys.ToArray();
            List<double> final_level_valuesList = new List<double>();
            foreach (int level in levels)
            {
                Color color = level_color_dict[0];
                if (Options.Customized_colors)
                {
                    color = dataset_color;
                }
                else if (Ontology_classification_class.Is_mbco_ontology(ontology))
                {
                    color = level_color_dict[level];
                }
                else if (ontology_color_dict.ContainsKey(ontology))
                {
                    color = ontology_color_dict[ontology];
                }
                else { throw new Exception(); }
                final_level_valuesList.Clear();
                if (extra_blank_level_valuesList_dict.ContainsKey(level))
                { final_level_valuesList.AddRange(extra_blank_level_valuesList_dict[level]); }
                final_level_valuesList.AddRange(level_valuesList_dict[level]);
                BarItem add_bar = bardiagram_graphPane.AddBar("Level " + level.ToString(), final_level_valuesList.ToArray(), null, color);
                add_bar.Label.IsVisible = false;
                add_bar.Bar.Fill = new Fill(color);
                add_bar.Bar.Border.Color = color;
                add_bar.Bar.Fill.Type = FillType.Solid;
            }
            #endregion

            List<string> final_scp_labels_list = new List<string>();
            final_scp_labels_list.AddRange(extra_blank_scp_labels);
            final_scp_labels_list.AddRange(scp_labels);



            bardiagram_graphPane.Y2Axis.MajorTic.IsBetweenLabels = true;
            bardiagram_graphPane.BarSettings.Type = BarType.Stack;

            bardiagram_graphPane.XAxis.Cross = extra_blank_scp_labels.Count;

            bardiagram_graphPane.BarSettings.Base = BarBase.Y;
            bardiagram_graphPane.X2Axis.IsVisible = false;
            bardiagram_graphPane.Y2Axis.IsVisible = false;
            bardiagram_graphPane.Y2Axis.Color = Color.White;
            bardiagram_graphPane.XAxis.MajorGrid.IsZeroLine = true;
            bardiagram_graphPane.XAxis.MajorGrid.IsVisible = false;
            bardiagram_graphPane.XAxis.MinorGrid.IsVisible = false;
            bardiagram_graphPane.XAxis.MajorTic.IsOpposite = false;
            bardiagram_graphPane.XAxis.MinorTic.IsOpposite = false;
            bardiagram_graphPane.YAxis.MajorTic.IsOpposite = false;
            bardiagram_graphPane.YAxis.MinorTic.IsOpposite = false;
            bardiagram_graphPane.XAxis.Color = Color.Black;
            bardiagram_graphPane.YAxis.Color = Color.White;
            bardiagram_graphPane.YAxis.MinSpace = 0;
            bardiagram_graphPane.YAxis.IsVisible = true;
            bardiagram_graphPane.YAxis.IsAxisSegmentVisible = false;
            bardiagram_graphPane.XAxis.MajorGrid.Color = Color.Black;
            bardiagram_graphPane.YAxis.MajorGrid.IsVisible = false;
            bardiagram_graphPane.YAxis.MajorTic.Color = Color.Transparent;
            bardiagram_graphPane.YAxis.MinorTic.Color = Color.Transparent;
            bardiagram_graphPane.YAxis.Type = AxisType.Text;
            string[] final_scp_labels;
            string final_scp_label;
            int final_scp_labels_length;
            if (Options.Use_scp_abbreviations)
            { final_scp_labels = Get_shortened_scp_names(final_scp_labels_list.ToArray()); }
            else { final_scp_labels = final_scp_labels_list.ToArray(); }
            final_scp_labels_length = final_scp_labels.Length;
            for (int indexFinal = 0; indexFinal < final_scp_labels_length; indexFinal++)
            {
                final_scp_label = final_scp_labels[indexFinal];
                if (final_scp_label.Length > Options.Max_character_for_scp_in_bardigram)
                {
                    final_scp_label = final_scp_label.Substring(0, Options.Max_character_for_scp_in_bardigram);
                    final_scp_labels[indexFinal] = final_scp_label;
                }
            }
            bardiagram_graphPane.YAxis.Scale.TextLabels = final_scp_labels;
            bardiagram_graphPane.YAxis.Scale.AlignH = AlignH.Center;
            bardiagram_graphPane.YAxis.Scale.Align = AlignP.Inside;
            bardiagram_graphPane.XAxis.Scale.Max = max_minusLog10Pvalue;
            bardiagram_graphPane.XAxis.Scale.Min = 0;
            bardiagram_graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
            bardiagram_graphPane.YAxis.Scale.IsPreventLabelOverlap = false;

            int max_ticks = 4;
            double[] potential_inner_tick_distances = Get_potential_inner_tick_distances_for_minusLog10Pvalues();
            double inner_ticks_distance = Get_inner_tick_distance(max_minusLog10Pvalue, max_ticks, potential_inner_tick_distances);
            bardiagram_graphPane.XAxis.Scale.MinorStep = inner_ticks_distance;
            bardiagram_graphPane.XAxis.Scale.MajorStep = inner_ticks_distance;
            bardiagram_graphPane.AxisChange();

            return bardiagram_graphPane;
        }
        private GraphPane Generate_legend_graphPane(int[] existing_levels, Ontology_type_enum ontology)
        {
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> enrich_list = new List<Ontology_enrichment_line_class>();

            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            enrich_line = new Ontology_enrichment_line_class();
            enrich_line.ProcessLevel = 5;
            enrich_line.Scp_name = gdf.SCP_abbreviations_fileName_reference;
            enrich_line.Minus_log10_pvalue = 0;
            enrich_line.Sample_color = Color.Black;
            enrich_line.Ontology_type = ontology;
            enrich_list.Add(enrich_line);

            bool is_mbco = Ontology_classification_class.Is_mbco_ontology(ontology);

            string[] bundle_enumerations = new string[] { "first", "second", "third", "fourth" };
            int existing_levels_length = existing_levels.Length;
            int existing_level;
            for (int indexL = 0; indexL < existing_levels_length; indexL++)
            {
                existing_level = existing_levels[indexL];
                enrich_line = new Ontology_enrichment_line_class();
                enrich_line.ProcessLevel = existing_level;
                if (is_mbco)
                {
                    enrich_line.Scp_name = "SCPs of level " + enrich_line.ProcessLevel + " (" + bundle_enumerations[indexL] + " bundle of SCPs)";
                }
                else
                {
                    enrich_line.Scp_name = "Top predicted " + Ontology_classification_class.Get_name_of_scps_for_progress_report(ontology);
                }
                enrich_line.Minus_log10_pvalue = 1;
                enrich_line.Sample_color = Color.Gray;
                enrich_line.Ontology_type = ontology;
                enrich_list.Add(enrich_line);
            }

            GraphPane legend_graphPane = new GraphPane();
            legend_graphPane = Generate_bardiagram_graphPane_for_sameDataset_enrich_lines(enrich_list.ToArray(), 0, "", 7, false, Options.Max_lines_per_bardiagram_for_standard_enrichment);


            legend_graphPane.Title.Text = "Legend";
            legend_graphPane.XAxis.Color = Color.Transparent;
            legend_graphPane.XAxis.IsAxisSegmentVisible = true;
            legend_graphPane.YAxis.Color = Color.Transparent;
            legend_graphPane.XAxis.Color = Color.Transparent;
            legend_graphPane.XAxis.Title.Text = "";
            legend_graphPane.YAxis.Title.Text = "";
            legend_graphPane.XAxis.MajorTic.Color = Color.Transparent;
            legend_graphPane.XAxis.MinorTic.Color = Color.Transparent;
            legend_graphPane.XAxis.IsVisible = false;
            legend_graphPane.XAxis.MajorGrid.IsVisible = false;
            legend_graphPane.XAxis.MinorGrid.IsVisible = false;
            legend_graphPane.XAxis.MajorTic.IsOpposite = false;
            legend_graphPane.XAxis.MinorTic.IsOpposite = false;
            legend_graphPane.YAxis.MajorTic.IsOpposite = false;
            legend_graphPane.YAxis.MinorTic.IsOpposite = false;
            legend_graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 1, Color.Transparent, false, false, false);
            return legend_graphPane;
        }
        public override void Set_PDF_generation_allowed(bool pdf_generation_allowed)
        {
            base.PDF_generation_allowed = pdf_generation_allowed;
        }

        private string Get_bardiagram_fileName_string_after_increasing_fileNo(ref int bardiagram_fileNo, string bardiagram_base_fileName)
        {
            StringBuilder bardiagram_fileName_sb = new StringBuilder();
            bardiagram_fileNo++;
            string bardiagram_fileNo_string = bardiagram_fileNo.ToString();
            while (bardiagram_fileNo_string.Length != 2) { bardiagram_fileNo_string = "0" + bardiagram_fileNo_string; }
            bardiagram_fileName_sb.AppendFormat((string)bardiagram_base_fileName);
            if (bardiagram_fileName_sb.Length > 0) { bardiagram_fileName_sb.AppendFormat("_bardiagram_no"); }
            else { bardiagram_fileName_sb.AppendFormat("Bardiagram_no"); }
            bardiagram_fileName_sb.AppendFormat("{0}", bardiagram_fileNo_string);
            bardiagram_fileName_sb.Replace('\\', '_');
            bardiagram_fileName_sb.Replace('/', '_');
            bardiagram_fileName_sb.Replace(';', '_');
            bardiagram_fileName_sb.Replace(',', '_');
            return bardiagram_fileName_sb.ToString();
        }

        public GraphPane[] Generate_bardiagrams_from_enrichment_results_save_as_images_and_return_graphPanes(Ontology_enrichment_class enrich_input, string results_directory, string bardiagram_base_fileName, Enrichment_algorithm_enum enrichment_algorithm, Form1_default_settings_class form_default_settings)
        {
            Ontology_type_enum ontology = enrich_input.Get_ontology_and_check_if_only_one_ontology();
            bool is_dynamic_enrichment = false;
            switch (enrichment_algorithm)
            {
                case Enrichment_algorithm_enum.Dynamic_enrichment:
                    is_dynamic_enrichment = true;
                    break;
                case Enrichment_algorithm_enum.Standard_enrichment:
                    is_dynamic_enrichment = false;
                    break;
                default:
                    throw new Exception();
            }
            string integrationGroup = enrich_input.Enrich[0].IntegrationGroup;
            string integrationGroup_string = ClassLibrary1.Dataset_userInterface.Default_textBox_texts.Get_integrationGroup_string(integrationGroup);
            int max_lines_per_bardiagram = Options.Get_max_lines_per_bardiagram(enrichment_algorithm);

            string bardiagram_results_directory = results_directory + Options.Bardiagram_subdirectory;
            if (!Options.Write_pdf) { ReadWriteClass.Create_directory_if_it_does_not_exist(bardiagram_results_directory); }
            Ontology_enrichment_class enrich = enrich_input.Deep_copy();
            if (!Ontology_classification_class.Is_mbco_ontology(ontology))
            {
                enrich.Set_all_processLevels_to_input_level(1);
            }
            int uniqueDatasets_length = enrich.Get_all_uniqueDatasetNames().Length;
            //enrich.Enrich = enrich.Enrich.OrderBy(l => l.Results_number).ToArray();
            enrich.Enrich = Ontology_enrichment_line_class.Order_by_resultsNo(enrich.Enrich);
            int enrich_length = enrich.Enrich.Length;
            Ontology_enrichment_line_class inner_enrich_line;
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> sameDataset_enrich = new List<Ontology_enrichment_line_class>();
            Color sampleName_color;

            GraphPane bardiagram_graphPane;


            int columns_count = 1;
            int rows_count = Options.Figures_per_chart;
            int total_images_count = columns_count * rows_count;
            int current_bars_in_diagram = 0;

            string current_bardiagram_fileName;
            int bardiagram_fileNo = 0;
            StringBuilder headlineSampleName_sb = new StringBuilder();

            PdfDocument pdf_document = new PdfDocument();
            int chart_no = 0;
            string add_to_uniqueDatasetName = "";
            float max_minusLog10Pvalue = -1;
            int empty_bars = 0;
            List<MasterPane> bardiagram_masterPanes = new List<MasterPane>();
            Dictionary<int, bool> processLevel_dict = new Dictionary<int, bool>();
            MasterPane bardiagram_masterPane = new MasterPane();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrich.Enrich[indexE];
                if ((indexE == 0)
                    || (!enrich_line.Unique_dataset_name.Equals(enrich.Enrich[indexE - 1].Unique_dataset_name))
                    || (current_bars_in_diagram - empty_bars > max_lines_per_bardiagram))
                {
                    processLevel_dict.Clear();
                    if ((indexE == 0)
                        || (!enrich_line.Unique_dataset_name.Equals(enrich.Enrich[indexE - 1].Unique_dataset_name)))
                    {
                        chart_no = 1;
                    }
                    else { chart_no++; }
                    if (chart_no == 1)
                    {
                        max_minusLog10Pvalue = -1;
                        for (int indexInner = indexE; indexInner < enrich_length; indexInner++)
                        {
                            inner_enrich_line = enrich.Enrich[indexInner];
                            if (inner_enrich_line.Unique_dataset_name.Equals(enrich_line.Unique_dataset_name))
                            {
                                if ((max_minusLog10Pvalue == -1)
                                    || (inner_enrich_line.Minus_log10_pvalue > max_minusLog10Pvalue))
                                {
                                    max_minusLog10Pvalue = inner_enrich_line.Minus_log10_pvalue;
                                }
                            }
                            else { break; }
                        }
                    }
                    sameDataset_enrich.Clear();
                    current_bars_in_diagram = 0;
                }
                sameDataset_enrich.Add(enrich_line);
                empty_bars = 0;
                if (!processLevel_dict.ContainsKey(enrich_line.ProcessLevel))
                {
                    processLevel_dict.Add(enrich_line.ProcessLevel,true);
                    empty_bars++;
                }
                if (is_dynamic_enrichment)
                {
                    current_bars_in_diagram = current_bars_in_diagram + 3;
                    empty_bars++;
                }
                else
                {
                    current_bars_in_diagram++;
                }
                current_bars_in_diagram = current_bars_in_diagram + empty_bars;
                if ((indexE == enrich_length - 1)
                    || (!enrich_line.Unique_dataset_name.Equals(enrich.Enrich[indexE + 1].Unique_dataset_name))
                    || (current_bars_in_diagram - empty_bars > max_lines_per_bardiagram))
                {
                    sampleName_color = enrich_line.Sample_color;
                    if (((indexE != enrich_length - 1) && (enrich_line.Unique_dataset_name.Equals(enrich.Enrich[indexE + 1].Unique_dataset_name)))
                        || (chart_no > 1))
                    {
                        add_to_uniqueDatasetName = " (no." + chart_no + ")";
                    }
                    else
                    {
                        add_to_uniqueDatasetName = "";
                    }
                    bardiagram_graphPane = Generate_bardiagram_graphPane_for_sameDataset_enrich_lines(sameDataset_enrich.ToArray(), current_bars_in_diagram, add_to_uniqueDatasetName, max_minusLog10Pvalue, is_dynamic_enrichment, max_lines_per_bardiagram);
                    bardiagram_masterPane.Add(bardiagram_graphPane);
                    if (bardiagram_masterPane.PaneList.Count == Options.Figures_per_chart)
                    {
                        bardiagram_masterPanes.Add(bardiagram_masterPane);
                        if (!Options.Write_pdf)
                        {
                            current_bardiagram_fileName = Get_bardiagram_fileName_string_after_increasing_fileNo(ref bardiagram_fileNo, bardiagram_base_fileName);
                            Write_masterPane(bardiagram_masterPane, bardiagram_results_directory + current_bardiagram_fileName + Options.Image_fileName_extension, Options.ImageFormat, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
                        }
                        bardiagram_masterPane = new MasterPane();
                    }
                }
            }

            int[] all_levels = enrich.Get_all_levels();

            bardiagram_graphPane = Generate_legend_graphPane(all_levels, ontology);
            bardiagram_masterPane.Add(bardiagram_graphPane);
            bardiagram_masterPanes.Add(bardiagram_masterPane);
            if (!Options.Write_pdf)
            {
                current_bardiagram_fileName = Get_bardiagram_fileName_string_after_increasing_fileNo(ref bardiagram_fileNo, bardiagram_base_fileName);
                Write_masterPane(bardiagram_masterPane, bardiagram_results_directory + current_bardiagram_fileName + Options.Image_fileName_extension, Options.ImageFormat, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
            }
            else
            {
                Add_masterPanes_to_pdf(ref pdf_document, bardiagram_masterPanes, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
                Write_and_dispose_pdf_document_if_allowed(ref pdf_document, results_directory + bardiagram_base_fileName + "_bardiagrams.pdf");
            }
            GraphPane[] graphPanes = base.Get_all_graphPanes_from_masterPanes(bardiagram_masterPanes.ToArray());
            return graphPanes.ToArray();
        }

    }

    class Timeline_options_class : Options_readWrite_base_class
    {
        public bool Generate_timeline { get; set; }
        public bool Customized_colors { get; set; }
        public bool Use_scp_abbreviations { get; set; }
        public int Resolution_factor { get; set; }
        public float Width_of_singleGraphPane { get; set; }
        public int LineStyle_maxOnOff { get; set; }

        public float Significance_pvalue_cutoff_copy { get; set; }
        public float Fraction_of_used_xspace_on_pdf_page { get; set; }
        public float Fraction_of_used_yspace_on_pdf_page { get; set; }
        public float Significance_minusLog10pvalue_cutoff
        {
            get
            {
                return -(float)Math.Log10(Significance_pvalue_cutoff_copy);
            }
        }
        public int Figures_per_chart
        {
            get
            {
                switch (Charts_per_page)
                {
                    case Charts_per_page_enum.High:
                        return 30;
                    case Charts_per_page_enum.Medium:
                        return 12;
                    case Charts_per_page_enum.Low:
                        return 4;
                    default:
                        throw new Exception();

                }
            }
        }
        public Charts_per_page_enum Charts_per_page { get; set; }
        public System.Drawing.Imaging.ImageFormat ImageFormat { get; set; }
        public bool Write_pdf { get; set; }
        public string Image_fileName_extension
        {
            get
            {
                if (Write_pdf) { return ".pdf"; }
                else { return "." + ImageFormat.ToString().ToLower(); }
            }
        }
        public string Timeline_subdirectory { get; set; }
        public bool Is_logarithmic_time_axis { get; set; }
        public bool Generate_timeline_in_log_scale { get { return Generate_timeline && Is_logarithmic_time_axis; } }

        public override void Write_option_entries(StreamWriter writer)
        {
            base.Write_entries_excluding_dictionaries(writer, typeof(Timeline_options_class), "Timeline_subdirectory", "Fraction_of_used_xspace_on_pdf_page",
                                                                       "Fraction_of_used_yspace_on_pdf_page", "Resolution_factor",
                                                                       "Width_of_singleGraphPane", "LineStyle_maxOnOff",
                                                                       "Significance_pvalue_cutoff_copy",
                                                                       "ImageFormat", "Charts_per_page", "Is_logarithmic_time_axis",
                                                                       "Write_pdf", "Use_scp_abbreviations",
                                                                       "Customized_colors");
        }
        public override bool Add_read_entry_to_options_and_return_if_successful(string readLine)
        {
            return base.Add_read_entry_and_return_if_succesful(readLine, typeof(Timeline_options_class));
        }


        public Timeline_options_class()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            LineStyle_maxOnOff = 10;
            Width_of_singleGraphPane = 750;//1200
            Resolution_factor = 100;
            Customized_colors = false;
            Generate_timeline = false;
            Fraction_of_used_xspace_on_pdf_page = 0.9F;
            Fraction_of_used_yspace_on_pdf_page = 0.85F;
            Charts_per_page = Charts_per_page_enum.High;
            ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            Timeline_subdirectory = "Timelines" + gdf.Delimiter;
            Is_logarithmic_time_axis = false;
            Use_scp_abbreviations = true;
        }
    }

    class Timeline_class : Visualization_base_class
    {
        public Timeline_options_class Options { get; set; }

        public Timeline_class(ProgressReport_interface_class progressReport) : base(progressReport)
        {
            Options = new Timeline_options_class();
        }
        public override void Set_PDF_generation_allowed(bool pdf_generation_allowed)
        {
            base.PDF_generation_allowed = pdf_generation_allowed;
        }

        private Ontology_enrichment_line_class[] Set_unique_timeunits(Ontology_enrichment_line_class[] enrich_lines)
        {
            int enrich_length = enrich_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            Dictionary<Timeunit_enum, bool> timeunit_dict = new Dictionary<Timeunit_enum, bool>();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrich_lines[indexE];
                if (enrich_line.Timepoint >= 1)
                {
                    if (!timeunit_dict.ContainsKey(enrich_line.Timeunit))
                    {
                        timeunit_dict.Add(enrich_line.Timeunit, true);
                    }
                }
            }
            Timeunit_enum smallest_timeunit;
            Timeunit_enum largest_timeunit;
            if (timeunit_dict.Keys.ToArray().Length >= 2)
            {
                Timeunit_conversion_class.Get_lowest_and_highest_timeunit(timeunit_dict.Keys.ToArray(), out smallest_timeunit, out largest_timeunit);
                if (!smallest_timeunit.Equals(largest_timeunit))
                {
                    for (int indexE = 0; indexE < enrich_length; indexE++)
                    {
                        enrich_line = enrich_lines[indexE];
                        enrich_line.Timepoint = Timeunit_conversion_class.Convert_timepoint_from_old_unit_to_new_unit(enrich_line.Timepoint, enrich_line.Timeunit, smallest_timeunit);
                        enrich_line.Timeunit = smallest_timeunit;
                    }
                }
            }
            return enrich_lines;
        }

        private GraphPane Generate_point_line_graphPane_for_sameSCP_enrich_lines(Ontology_enrichment_line_class[] sameSCP_enrich_lines, bool down_regulated_scps_exist, bool up_regulated_scps_exist, Dictionary<string, int> sampleName_lineOnDash_dict, bool is_mbco_ontology)
        {
            bool is_dynamic_enrichment = Analyze_if_dynamic_enrichment(sameSCP_enrich_lines);
            sameSCP_enrich_lines = Set_unique_timeunits(sameSCP_enrich_lines);
            Timeunit_enum timeunit = sameSCP_enrich_lines[0].Timeunit;
            string scp = sameSCP_enrich_lines[0].Scp_name;
            int level = sameSCP_enrich_lines[0].ProcessLevel;
            Ontology_type_enum ontology = sameSCP_enrich_lines[0].Ontology_type;
            if (is_dynamic_enrichment) { throw new Exception(); }
            int enrich_length = sameSCP_enrich_lines.Length;
            Ontology_enrichment_line_class enrich_line;

            Dictionary<int, Color> level_color_dict = Global_class.Get_level_scpColor_dict();
            Dictionary<Ontology_type_enum, Color> ontology_color_dict = Global_class.Get_ontology_scpColor_dict();

            string used_scpName = "";
            if (Options.Use_scp_abbreviations)
            { used_scpName = Get_shortened_scp_name(scp); }
            else { used_scpName = (string)scp.Clone(); }
            int max_character_count = 0;
            int current_character_count;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = sameSCP_enrich_lines[indexE];
                if (!enrich_line.Timeunit.Equals(timeunit)) { throw new Exception(); }
                current_character_count = enrich_line.Timepoint.ToString().Length;
                if (current_character_count > max_character_count)
                {
                    max_character_count = current_character_count;
                }
            }

            GraphPane timeline_graphPane = new GraphPane();
            timeline_graphPane.Margin.All = 1;
            timeline_graphPane.Border.Color = Color.White;
            if (is_mbco_ontology)
            {
                timeline_graphPane.Title.Text = "Level " + level + ":\n\r" + Text_class.Split_text_over_multiple_lines(used_scpName, 38, 5);
            }
            else
            {
                timeline_graphPane.Title.Text = Text_class.Split_text_over_multiple_lines(used_scpName, 38, 5);
            }
            timeline_graphPane.Title.FontSpec = new FontSpec("Arial", 30, Color.Black, true, false, false);
            timeline_graphPane.Title.FontSpec.Border.IsVisible = false;
            timeline_graphPane.X2Axis.IsVisible = false;
            timeline_graphPane.Y2Axis.IsVisible = false;
            timeline_graphPane.XAxis.MajorTic.IsOpposite = false;
            timeline_graphPane.XAxis.MinorTic.IsOpposite = false;
            timeline_graphPane.YAxis.MajorTic.IsOpposite = false;
            timeline_graphPane.YAxis.MinorTic.IsOpposite = false;
            timeline_graphPane.XAxis.Title.Text = "time [" + timeunit + "]";
            timeline_graphPane.YAxis.Title.Text = "-log10(p)";
            timeline_graphPane.XAxis.Title.FontSpec = new FontSpec("Arial", 40, Color.Black, false, false, false);
            timeline_graphPane.XAxis.Title.FontSpec.Border.IsVisible = false;
            timeline_graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 30, Color.Black, false, false, false);
            timeline_graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
            timeline_graphPane.YAxis.Title.FontSpec = new FontSpec("Arial", 40, Color.Black, false, false, false);
            timeline_graphPane.YAxis.Title.FontSpec.Border.IsVisible = false;
            timeline_graphPane.YAxis.Title.FontSpec.Angle = 180;
            timeline_graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 30, Color.Black, false, false, false);
            timeline_graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
            timeline_graphPane.YAxis.Scale.FontSpec.Angle = 90;
            timeline_graphPane.Chart.Border.IsVisible = false;

            float max_timepoint = -1;
            float min_timepoint = 9999999999;
            float min_abs_timepoint_not_zero = 9999999999;
            float max_minLog10pvalue = -1;
            float min_distance_between_adjacent_timepoints = -1;
            float current_distance_between_adjacent_timepoints;
            Dictionary<double, bool> all_timepoints_dict = new Dictionary<double, bool>();
            all_timepoints_dict.Add(0, true);

            //sameSCP_enrich_lines = sameSCP_enrich_lines.OrderBy(l => l.Timepoint).ToArray();
            sameSCP_enrich_lines = Ontology_enrichment_line_class.Order_by_timepoint(sameSCP_enrich_lines);
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = sameSCP_enrich_lines[indexE];
                if ((indexE != 0)
                    && (!enrich_line.Timepoint.Equals(sameSCP_enrich_lines[indexE - 1].Timepoint)))
                {
                    current_distance_between_adjacent_timepoints = enrich_line.Timepoint - sameSCP_enrich_lines[indexE - 1].Timepoint;
                    if ((min_distance_between_adjacent_timepoints == -1)
                        || (current_distance_between_adjacent_timepoints < min_distance_between_adjacent_timepoints))
                    {
                        min_distance_between_adjacent_timepoints = current_distance_between_adjacent_timepoints;
                    }
                }
                if (enrich_line.Minus_log10_pvalue > max_minLog10pvalue) { max_minLog10pvalue = enrich_line.Minus_log10_pvalue; }
                if (enrich_line.Timepoint > max_timepoint) { max_timepoint = enrich_line.Timepoint; }
                if (enrich_line.Timepoint < min_timepoint) { min_timepoint = enrich_line.Timepoint; }
                if ((Math.Abs(enrich_line.Timepoint) < min_abs_timepoint_not_zero)
                    && (enrich_line.Timepoint != 0)) { min_abs_timepoint_not_zero = Math.Abs(enrich_line.Timepoint); }
                if (!all_timepoints_dict.ContainsKey(enrich_line.Timepoint))
                {
                    all_timepoints_dict.Add(enrich_line.Timepoint, true);
                }
            }
            float min_x = 0;
            float max_x = 0;
            if (!Options.Is_logarithmic_time_axis)
            {
                if (min_timepoint >= 0) { min_x = 0F; }
                else { min_x = min_timepoint - min_distance_between_adjacent_timepoints; }
                max_x = max_timepoint + min_distance_between_adjacent_timepoints;
                if (!all_timepoints_dict.ContainsKey(min_x)) { all_timepoints_dict.Add(min_x, true); }
                if (!all_timepoints_dict.ContainsKey(max_x)) { all_timepoints_dict.Add(max_x, true); }
            }
            else
            {
                min_x = (float)Math.Pow(10, Math.Floor(Math.Log10(min_timepoint)));
                max_x = (float)Math.Pow(10, Math.Ceiling(Math.Log10(max_timepoint)));
            }
            float max_y = 0;
            float min_y = 0;
            LineItem new_line_in_graph;

            if (down_regulated_scps_exist) { min_y = -(float)Math.Ceiling(max_minLog10pvalue * 1.1); }
            if (up_regulated_scps_exist) { max_y = (float)Math.Ceiling(max_minLog10pvalue * 1.1); }


            #region Identify inner tick distances for x and y
            double y_distance;
            double y_major_tick_interval;
            double[] potential_distances = Get_potential_inner_tick_distances_for_minusLog10Pvalues();
            int number_of_ticks = 4;
            if ((up_regulated_scps_exist) && (down_regulated_scps_exist))
            {
                number_of_ticks = 4;
                if (Math.Abs(max_y) != Math.Abs(min_y)) { throw new Exception(); }
                y_distance = max_y;
                y_major_tick_interval = Get_inner_tick_distance(y_distance, number_of_ticks, potential_distances);
                max_y *= 1.1F;
                min_y = -max_y;
            }
            else
            {
                number_of_ticks = 4;
                y_distance = max_y - min_y;
                y_major_tick_interval = Get_inner_tick_distance(y_distance, number_of_ticks, potential_distances);
                max_y *= 1.1F;
            }

            int max_count_of_timepoints = all_timepoints_dict.Keys.Count;
            double x_distance;
            double[] potential_x_distances = Get_potential_inner_tick_distances_for_time(Options.Is_logarithmic_time_axis);
            double x_major_tick_interval;
            if ((max_x > 0) && (min_x < 0))
            {
                number_of_ticks = Math.Min(4, max_count_of_timepoints);
                x_distance = max_x - min_x;
                x_major_tick_interval = Get_inner_tick_distance(x_distance, number_of_ticks, potential_x_distances);
            }
            else if (max_x > 0)
            {
                number_of_ticks = Math.Min(7, max_count_of_timepoints);
                x_distance = max_x;
                x_major_tick_interval = Get_inner_tick_distance(x_distance, number_of_ticks, potential_x_distances);
            }
            else if (min_x < 0)
            {
                number_of_ticks = Math.Min(7, max_count_of_timepoints);
                x_distance = max_x;
                x_major_tick_interval = Get_inner_tick_distance(x_distance, number_of_ticks, potential_x_distances);
            }
            else { throw new Exception(); }
            #endregion

            timeline_graphPane.XAxis.IsVisible = true;
            if (Options.Is_logarithmic_time_axis)
            {
                timeline_graphPane.XAxis.Type = AxisType.Log;
                timeline_graphPane.AxisChange();
            }
            else
            {
                timeline_graphPane.XAxis.Scale.MajorStep = x_major_tick_interval;
                timeline_graphPane.XAxis.Scale.MinorStep = timeline_graphPane.XAxis.Scale.MajorStep;
            }
            timeline_graphPane.XAxis.Scale.Max = max_x;
            timeline_graphPane.XAxis.Scale.Min = min_x;
            timeline_graphPane.YAxis.Scale.Max = max_y;
            timeline_graphPane.YAxis.Scale.Min = min_y;
            timeline_graphPane.YAxis.Scale.MajorStep = y_major_tick_interval;
            timeline_graphPane.YAxis.Scale.MinorStep = timeline_graphPane.YAxis.Scale.MajorStep;
            timeline_graphPane.XAxis.MajorGrid.IsVisible = false;
            timeline_graphPane.XAxis.MinorGrid.IsVisible = false;
            timeline_graphPane.YAxis.MajorGrid.IsVisible = false;
            timeline_graphPane.YAxis.MinorGrid.IsVisible = false;
            timeline_graphPane.Legend.IsVisible = false;

            float direction_factor = 0;
            List<double> points_x_values = new List<double>();
            List<double> points_y_values = new List<double>();
            Color sample_color;

            //sameSCP_enrich_lines = sameSCP_enrich_lines.OrderBy(l => l.SampleName).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ToArray();
            sameSCP_enrich_lines = Ontology_enrichment_line_class.Order_sampleName_entryType_timepoint(sameSCP_enrich_lines);
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = sameSCP_enrich_lines[indexE];
                if ((indexE == 0)
                    || (!enrich_line.SampleName.Equals(sameSCP_enrich_lines[indexE - 1].SampleName))
                    || (!enrich_line.EntryType.Equals(sameSCP_enrich_lines[indexE - 1].EntryType)))
                {
                    switch (enrich_line.EntryType)
                    {
                        case Entry_type_enum.Up:
                            direction_factor = 1;
                            break;
                        case Entry_type_enum.Down:
                            direction_factor = -1;
                            break;
                    }
                }
                points_x_values.Add(enrich_line.Timepoint);
                points_y_values.Add(enrich_line.Minus_log10_pvalue * direction_factor);
                if ((indexE == enrich_length - 1)
                    || (!enrich_line.SampleName.Equals(sameSCP_enrich_lines[indexE + 1].SampleName))
                    || (!enrich_line.EntryType.Equals(sameSCP_enrich_lines[indexE + 1].EntryType)))
                {
                    if (Options.Customized_colors)
                    {
                        sample_color = enrich_line.Sample_color;
                    }
                    else if (Ontology_classification_class.Is_mbco_ontology(ontology))
                    {
                        sample_color = level_color_dict[enrich_line.ProcessLevel];
                    }
                    else if (ontology_color_dict.ContainsKey(ontology))
                    {
                        sample_color = ontology_color_dict[ontology];
                    }
                    else { throw new Exception(); }
                    new_line_in_graph = timeline_graphPane.AddCurve(enrich_line.SampleName + "_" + enrich_line.EntryType, points_x_values.ToArray(), points_y_values.ToArray(), sample_color, SymbolType.Circle);
                    new_line_in_graph.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                    new_line_in_graph.Symbol.Size = 15;
                    new_line_in_graph.Symbol.Fill.Color = sample_color;
                    new_line_in_graph.Symbol.Fill.Type = FillType.Solid;
                    new_line_in_graph.Line.DashOn = sampleName_lineOnDash_dict[enrich_line.SampleName];
                    new_line_in_graph.Line.DashOff = Options.LineStyle_maxOnOff - sampleName_lineOnDash_dict[enrich_line.SampleName];
                    new_line_in_graph.Line.Width = 10;
                    points_x_values.Clear();
                    points_y_values.Clear();
                }
            }

            #region Draw axises and cutoff lines
            Color axis_color = Color.Black;
            float axis_lineWidth = 3;

            float referenceLines_min_x = min_x - 0.1F * Math.Max(Math.Abs(max_x), Math.Abs(min_x));
            float referenceLines_max_x = max_x + 0.1F * Math.Max(Math.Abs(max_x), Math.Abs(min_x));

            //Draw x-axis
            new_line_in_graph = timeline_graphPane.AddCurve("X-axis", new double[] { referenceLines_min_x, referenceLines_max_x }, new double[] { 0, 0 }, axis_color, SymbolType.None);
            new_line_in_graph.Line.Width = axis_lineWidth;

            float pvalue_cutoff_dashOn_distance = 5;
            float pvalue_cutoff_dashOff_distance = 5;
            float significance_minusLog10pvalue_cutoff = Options.Significance_minusLog10pvalue_cutoff;
            if (up_regulated_scps_exist)
            {
                //Draw positive -log10(p-value) cutoff line
                new_line_in_graph = timeline_graphPane.AddCurve("Positive cutoff line", new double[] { referenceLines_min_x, referenceLines_max_x }, new double[] { significance_minusLog10pvalue_cutoff, significance_minusLog10pvalue_cutoff }, axis_color, SymbolType.None);
                new_line_in_graph.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                new_line_in_graph.Line.DashOn = pvalue_cutoff_dashOn_distance;
                new_line_in_graph.Line.DashOff = pvalue_cutoff_dashOff_distance;
                new_line_in_graph.Line.Width = axis_lineWidth;
            }
            if (down_regulated_scps_exist)
            {
                //Draw negative -log10(p-value) cutoff line
                new_line_in_graph = timeline_graphPane.AddCurve("Negative cutoff line", new double[] { referenceLines_min_x, referenceLines_max_x }, new double[] { -significance_minusLog10pvalue_cutoff, -significance_minusLog10pvalue_cutoff }, axis_color, SymbolType.None);
                new_line_in_graph.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                new_line_in_graph.Line.DashOn = pvalue_cutoff_dashOn_distance;
                new_line_in_graph.Line.DashOff = pvalue_cutoff_dashOff_distance;
                new_line_in_graph.Line.Width = axis_lineWidth;
            }
            #endregion


            return timeline_graphPane;
        }

        private GraphPane[] Generate_legend(Ontology_enrichment_line_class[] sameSCP_enrich_lines, Dictionary<string, int> sampleName_lineOnDash_dict)
        {
            //sameSCP_enrich_lines = sameSCP_enrich_lines.OrderBy(l => l.SampleName).ThenBy(l => l.ProcessLevel).ThenBy(l => l.TimepointInDays).ToArray();
            sameSCP_enrich_lines = Ontology_enrichment_line_class.Order_sampleName_processLevel_timepointInDays(sameSCP_enrich_lines);
            Ontology_type_enum ontology = sameSCP_enrich_lines[0].Ontology_type;
            int enrich_lines_length = sameSCP_enrich_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            GraphPane legend_graphPane = new GraphPane();
            List<GraphPane> legend_graphPanes = new List<GraphPane>();
            Color sample_color;
            Dictionary<int, Color> level_color_dict = Global_class.Get_level_scpColor_dict();
            Dictionary<Ontology_type_enum, Color> ontology_color_dict = Global_class.Get_ontology_scpColor_dict();
            List<string> sampleNames = new List<string>();
            int legendIndex = -1;
            int maxEntries_each_legend = 11;

            LineItem legend_line;
            TextObj text;
            string legend_entry;

            legendIndex++;
            text = new TextObj("Positive '-log10(p)' (above abscissa): Upregulated", 0.05, maxEntries_each_legend - legendIndex, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
            text.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill.Color = Color.Transparent;
            text.ZOrder = ZOrder.A_InFront;
            legend_graphPane.GraphObjList.Add(text);

            legendIndex++;
            text = new TextObj("Negative '-log10(p)' (below abscissa): Downregulated", 0.05, maxEntries_each_legend - legendIndex, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
            text.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill.Color = Color.Transparent;
            text.ZOrder = ZOrder.A_InFront;
            legend_graphPane.GraphObjList.Add(text);

            legendIndex++;
            legend_entry = "+/- '-log10(" + Options.Significance_pvalue_cutoff_copy + ")'";
            legend_line = legend_graphPane.AddCurve(legend_entry, new double[] { 0.05, 0.37 }, new double[] { maxEntries_each_legend - legendIndex, maxEntries_each_legend - legendIndex }, Color.Black, SymbolType.None);
            legend_line.Line.Width = 10;
            legend_line.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
            legend_line.Line.DashOn = 5;
            legend_line.Line.DashOff = 5;
            text = new TextObj(legend_entry, 0.40, maxEntries_each_legend - legendIndex, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
            text.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill.Color = Color.Transparent;
            text.ZOrder = ZOrder.A_InFront;
            legend_graphPane.GraphObjList.Add(text);

            for (int indexE = 0; indexE < enrich_lines_length; indexE++)
            {
                enrich_line = sameSCP_enrich_lines[indexE];
                if ((indexE == 0)
                    || (!enrich_line.SampleName.Equals(sameSCP_enrich_lines[indexE - 1].SampleName))
                    || (!enrich_line.ProcessLevel.Equals(sameSCP_enrich_lines[indexE - 1].ProcessLevel))
                    )
                {
                    legendIndex++;
                    sampleNames.Add(enrich_line.SampleName);
                    if (Options.Customized_colors)
                    {
                        sample_color = enrich_line.Sample_color;
                    }
                    else if (Ontology_classification_class.Is_mbco_ontology(ontology))
                    {
                        sample_color = level_color_dict[enrich_line.ProcessLevel];
                    }
                    else if (ontology_color_dict.ContainsKey(ontology))
                    {
                        sample_color = ontology_color_dict[(ontology)];
                    }
                    else { throw new Exception(); }
                    legend_entry = (string)enrich_line.SampleName.Clone() + " - SCP level " + enrich_line.ProcessLevel;
                    legend_line = legend_graphPane.AddCurve(legend_entry, new double[] { 0.05, 0.2 }, new double[] { maxEntries_each_legend - legendIndex, maxEntries_each_legend - legendIndex }, sample_color, SymbolType.None);
                    legend_line.Line.Width = 10;
                    legend_line.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                    legend_line.Line.DashOn = sampleName_lineOnDash_dict[enrich_line.SampleName];
                    legend_line.Line.DashOff = Options.LineStyle_maxOnOff - sampleName_lineOnDash_dict[enrich_line.SampleName];
                    text = new TextObj(legend_entry, 0.25, maxEntries_each_legend - legendIndex, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                    text.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                    text.FontSpec.Border.IsVisible = false;
                    text.FontSpec.Fill.Color = Color.Transparent;
                    text.ZOrder = ZOrder.A_InFront;
                    legend_graphPane.GraphObjList.Add(text);
                }
                if ((indexE == enrich_lines_length - 1)
                    || (legendIndex == maxEntries_each_legend - 1))
                {
                    //legendIndex = -1;
                    legend_graphPane.Title.Text = "Legend";
                    legend_graphPane.Title.FontSpec = new FontSpec("Arial", 25, Color.Black, true, false, false);
                    legend_graphPane.Title.FontSpec.Border.IsVisible = false;
                    legend_graphPane.XAxis.Scale.Min = 0;
                    legend_graphPane.XAxis.Scale.Max = 1;
                    legend_graphPane.XAxis.IsVisible = false;
                    legend_graphPane.YAxis.IsVisible = false;
                    legend_graphPane.XAxis.MajorGrid.IsVisible = false;
                    legend_graphPane.XAxis.MinorGrid.IsVisible = false;
                    legend_graphPane.YAxis.MajorGrid.IsVisible = false;
                    legend_graphPane.YAxis.MinorGrid.IsVisible = false;
                    legend_graphPane.XAxis.Color = Color.Transparent;
                    legend_graphPane.YAxis.Color = Color.Transparent;
                    legend_graphPane.XAxis.IsAxisSegmentVisible = false;
                    legend_graphPane.YAxis.IsAxisSegmentVisible = false;
                    legend_graphPane.BarSettings.Base = BarBase.Y;
                    legend_graphPane.YAxis.Scale.Min = -1;
                    legend_graphPane.YAxis.Scale.Max = maxEntries_each_legend + 1;
                    legend_graphPane.Border.IsVisible = false;
                    legend_graphPane.YAxis.Scale.MajorStep = 1;
                    legend_graphPane.YAxis.Scale.MinorStep = 1;
                    legend_graphPane.YAxis.Scale.IsVisible = false;
                    legend_graphPane.XAxis.Scale.IsVisible = false;
                    legend_graphPane.Legend.IsVisible = false;
                    legend_graphPane.Border.IsVisible = false;
                    legend_graphPane.Chart.Border.IsVisible = false;
                    legend_graphPane.Border.IsVisible = false;

                    legend_graphPanes.Add(legend_graphPane);
                }
            }
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            legendIndex++;
            text = new TextObj(gdf.SCP_abbreviations_fileName_reference, 0.05, maxEntries_each_legend - legendIndex, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
            text.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill.Color = Color.Transparent;
            text.ZOrder = ZOrder.A_InFront;
            legend_graphPane.GraphObjList.Add(text);
            
            return legend_graphPanes.ToArray();
        }

        private string Get_timeline_fileName_after_increase_of_figure_chart_count(ref int timeline_fileNo, string timeline_base_fileName)
        {
            timeline_fileNo++;
            string timeline_fileName;
            string timeline_fileNo_string = timeline_fileNo.ToString();
            while (timeline_fileNo_string.Length != 2) { timeline_fileNo_string = "0" + timeline_fileNo_string; }
            if (timeline_base_fileName.Length == 0) { timeline_fileName = "Timeline_no" + timeline_fileNo_string + Options.Image_fileName_extension; }
            else { timeline_fileName = timeline_base_fileName + "_" + "timeline_no" + timeline_fileNo_string + Options.Image_fileName_extension; }
            return timeline_fileName;
        }

        private Ontology_enrichment_class Adjust_sampleColors_for_same_sampleName_lines(Ontology_enrichment_class onto_enrich)
        {
            //onto_enrich.Enrich = onto_enrich.Enrich.OrderBy(l => l.SampleName).ThenBy(l => l.TimepointInDays).ThenByDescending(l => l.EntryType).ToArray();
            onto_enrich.Enrich = Ontology_enrichment_line_class.Order_by_sampleName(onto_enrich.Enrich);
            int enrich_length = onto_enrich.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            Color current_color = Color.Tan;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = onto_enrich.Enrich[indexE];
                if ((indexE == 0)
                    || (!enrich_line.SampleName.Equals(onto_enrich.Enrich[indexE - 1].SampleName)))
                {
                    current_color = enrich_line.Sample_color;
                }
                enrich_line.Sample_color = current_color;
            }
            return onto_enrich;
        }

        private Dictionary<string, int> Generate_sampleName_lineStyleOnDashDictionary(Ontology_enrichment_class onto_enrich)
        {
            string[] sampleNames = onto_enrich.Get_all_sampleNames();
            string sampleName;
            int sampleNames_length = sampleNames.Length;
            Dictionary<string, int> sampleName_onDash_dict = new Dictionary<string, int>();
            if (Options.Customized_colors)
            {
                for (int indexSample = 0; indexSample < sampleNames_length; indexSample++)
                {
                    sampleName = sampleNames[indexSample];
                    sampleName_onDash_dict.Add(sampleName, Options.LineStyle_maxOnOff);
                }
            }
            else
            {
                float onDash_stepSize = (float)Options.LineStyle_maxOnOff / sampleNames_length;
                for (int indexSample = 0; indexSample < sampleNames_length; indexSample++)
                {
                    sampleName = sampleNames[indexSample];
                    sampleName_onDash_dict.Add(sampleName, (int)Math.Round((indexSample + 1) * onDash_stepSize));
                }
            }
            return sampleName_onDash_dict;
        }

        public GraphPane[] Generate_timelines_from_enrichment_results_save_as_images_and_return_graphPanes(Ontology_enrichment_class enrich_input, string results_directory, string timeline_base_fileName)
        {
            string integrationGroup = enrich_input.Enrich[0].IntegrationGroup;
            Ontology_type_enum ontology = enrich_input.Get_ontology_and_check_if_only_one_ontology();
            bool is_mbco = Ontology_classification_class.Is_mbco_ontology(ontology);
            Ontology_enrichment_class enrich = enrich_input.Deep_copy();
            if (!is_mbco)
            {
                enrich.Set_all_processLevels_to_input_level(1);
            }
            enrich = Adjust_sampleColors_for_same_sampleName_lines(enrich);

            Dictionary<string, int> sampleName_lineDashOn_dict = Generate_sampleName_lineStyleOnDashDictionary(enrich);

            string timeline_results_directory = results_directory + Options.Timeline_subdirectory;
            int enrich_length = enrich.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            if (!Options.Write_pdf) { ReadWriteClass.Create_directory_if_it_does_not_exist(timeline_results_directory); }

            bool add_entryTypes = enrich.Get_all_entryTypes().Length > 1;
            bool add_timepoints = enrich.Get_all_timepointsInDays().Length > 1;
            List<Ontology_enrichment_line_class> sameSCPs_enrich_lines = new List<Ontology_enrichment_line_class>();

            Entry_type_enum[] entryTypes = enrich.Get_all_entryTypes();
            bool up_regulated_scps_exist = entryTypes.Contains(Entry_type_enum.Up);
            bool down_regulated_scps_exist = entryTypes.Contains(Entry_type_enum.Down);

            Chart timeline_chart = new Chart();

            string timeline_fileName;
            int total_scps_count = enrich.Get_all_scps().Length;
            //enrich.Enrich = enrich.Enrich.OrderBy(l => l.ProcessLevel).ThenBy(l => l.Scp_name).ToArray();
            enrich.Enrich = Ontology_enrichment_line_class.Order_by_processLevel_scpName(enrich.Enrich);
            PdfDocument document = new PdfSharp.Pdf.PdfDocument();
            int current_scp_no = 0;
            List<MasterPane> timeline_masterPanes = new List<MasterPane>();
            MasterPane timeline_masterPane = new MasterPane();
            GraphPane timeline_graphPane;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrich.Enrich[indexE];
                if (!enrich_line.IntegrationGroup.Equals(integrationGroup)) { throw new Exception(); }
                if ((indexE == 0)
                    || (!enrich_line.Scp_name.Equals(enrich.Enrich[indexE - 1].Scp_name)))
                {
                    sameSCPs_enrich_lines.Clear();
                    current_scp_no++;
                }
                sameSCPs_enrich_lines.Add(enrich_line);
                if ((indexE == enrich_length - 1)
                    || (!enrich_line.Scp_name.Equals(enrich.Enrich[indexE + 1].Scp_name)))
                {
                    timeline_graphPane = Generate_point_line_graphPane_for_sameSCP_enrich_lines(sameSCPs_enrich_lines.ToArray(), down_regulated_scps_exist, up_regulated_scps_exist, sampleName_lineDashOn_dict, is_mbco);
                    timeline_masterPane.PaneList.Add(timeline_graphPane);
                    if ((timeline_masterPane.PaneList.Count == Options.Figures_per_chart)
                        || (indexE == enrich_length - 1))
                    {
                        if (timeline_masterPane.PaneList.Count.Equals(Options.Figures_per_chart))
                        {
                            timeline_masterPanes.Add(timeline_masterPane);
                            timeline_masterPane = new MasterPane();
                        }
                    }
                }
            }
            GraphPane[] legend_graphPanes = Generate_legend(enrich.Enrich, sampleName_lineDashOn_dict);
            foreach (GraphPane legend_graphPane in legend_graphPanes)
            {
                timeline_masterPane.PaneList.Add(legend_graphPane);
                if (timeline_masterPane.PaneList.Count.Equals(Options.Figures_per_chart))
                {
                    timeline_masterPanes.Add(timeline_masterPane);
                    timeline_masterPane = new MasterPane();
                }
            }
            if (timeline_masterPane.PaneList.Count > 0) { timeline_masterPanes.Add(timeline_masterPane); }
            if (!Options.Write_pdf)
            {
                int timeline_fileNo = 0;
                foreach (MasterPane image_timeline_masterPane in timeline_masterPanes)
                {
                    timeline_fileName = Get_timeline_fileName_after_increase_of_figure_chart_count(ref timeline_fileNo, timeline_base_fileName);
                    Write_masterPane(image_timeline_masterPane, timeline_results_directory + timeline_fileName, Options.ImageFormat, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
                }
            }
            else
            {
                #region Adjust graphical parameter for PDF
                foreach (MasterPane pdf_masterPane in timeline_masterPanes)
                {
                    foreach (GraphPane pdf_graphPane in pdf_masterPane.PaneList)
                    {
                        pdf_graphPane.Title.FontSpec = new FontSpec("Arial", 28, Color.Black, true, false, false);
                        pdf_graphPane.Title.FontSpec.Border.IsVisible = false;
                        pdf_graphPane.XAxis.Title.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                        pdf_graphPane.XAxis.Title.FontSpec.Border.IsVisible = false;
                        pdf_graphPane.YAxis.Title.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                        pdf_graphPane.YAxis.Title.FontSpec.Border.IsVisible = false;
                        pdf_graphPane.YAxis.Title.FontSpec.Angle = 180;
                        pdf_graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 28, Color.Black, false, false, false);
                        pdf_graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
                        pdf_graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
                        pdf_graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 28, Color.Black, false, false, false);
                        pdf_graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
                        pdf_graphPane.YAxis.Scale.IsPreventLabelOverlap = false;
                        pdf_graphPane.YAxis.Scale.FontSpec.Angle = 90;
                        foreach (LineItem line in pdf_graphPane.CurveList)
                        {
                            if ((line.Label.Text.Equals("Positive cutoff line"))
                                || (line.Label.Text.Equals("Negative cutoff line"))
                                || (line.Label.Text.Equals("X-axis")))
                            {
                                line.Line.Width = 1;
                            }
                            else
                            {
                                line.Line.Width = 5;
                                line.Symbol.Size = 14;
                            }
                        }
                    }
                }
                #endregion

                Add_masterPanes_to_pdf(ref document, timeline_masterPanes, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
                Write_and_dispose_pdf_document_if_allowed(ref document, results_directory + timeline_base_fileName + "_timeline.pdf");
            }
            GraphPane[] graphPanes = base.Get_all_graphPanes_from_masterPanes(timeline_masterPanes.ToArray());
            return base.Get_all_graphPanes_from_masterPanes(timeline_masterPanes.ToArray());
        }
    }

    class Heatmap_options_class : Options_readWrite_base_class
    {
        public bool Generate_heatmap { get; set; }
        public System.Drawing.Imaging.ImageFormat ImageFormat { get; set; }
        public bool Write_pdf { get; set; }
        public bool Use_scp_abbreviations { get; set; }
        public int Min_scpRows_in_one_heatmap { get; set; }
        public int Max_scpRows_in_one_heatmap { get; set; }
        public int DatasetCols_in_one_heatmap { get; set; }
        public Enrichment_value_type_enum Value_type_selected_for_visualization { get; set; }
        public bool Show_significant_scps_over_all_conditions { get; set; }
        public float Width_of_singleGraphPane { get; set; }
        public string Image_fileName_extension
        {
            get
            {
                if (Write_pdf) { return ".pdf"; }
                else { return "." + ImageFormat.ToString().ToLower(); }
            }
        }
        public string Heatmap_subdirectory { get; set; }
        public Color[] Color_gradient_up { get; set; }
        public Color[] Color_gradient_down { get; set; }
        public Color[] Reverse_color_gradient_up
        {
            get
            {
                int up_length = Color_gradient_up.Length;
                Color[] up_color_gradient = new Color[up_length];
                for (int indexColor = 0; indexColor < up_length; indexColor++)
                {
                    up_color_gradient[up_length - indexColor - 1] = Color_gradient_up[indexColor];
                }
                return up_color_gradient;
            }
        }
        public Color[] Reverse_color_gradient_down
        {
            get
            {
                int down_length = Color_gradient_down.Length;
                Color[] up_color_gradient = new Color[down_length];
                for (int indexColor = 0; indexColor < down_length; indexColor++)
                {
                    up_color_gradient[down_length - indexColor - 1] = Color_gradient_down[indexColor];
                }
                return up_color_gradient;
            }
        }
        public Color Color_default { get; set; }
        public Color Color_border { get; set; }
        public Brush Brush_text { get; set; }
        public Charts_per_page_enum Charts_per_page { get; set; }
        public int Figures_per_chart
        {
            get
            {
                switch (Charts_per_page)
                {
                    case Charts_per_page_enum.High:
                        return 4;
                    case Charts_per_page_enum.Medium:
                        return 2;
                    case Charts_per_page_enum.Low:
                        return 1;
                    default:
                        throw new Exception();
                }
            }
        }


        public Heatmap_options_class()
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            Width_of_singleGraphPane = 2000;// 3000;
            Generate_heatmap = false;
            ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            Heatmap_subdirectory = "Heatmaps" + gdf.Delimiter;
            Show_significant_scps_over_all_conditions = false;
            Min_scpRows_in_one_heatmap = 10;
            Max_scpRows_in_one_heatmap = 10;
            Use_scp_abbreviations = true;
            Value_type_selected_for_visualization = Enrichment_value_type_enum.Fractional_rank;
            int max_colors_in_gradient = 1000;
            Color[] intermediate_color_up = new Color[] { Color.White, Color.Yellow, Color.Orange, Color.OrangeRed, Color.Red, Color.DarkRed };
            Color[] intermediate_color_down = new Color[] { Color.White, Color.LightSkyBlue, Color.SkyBlue, Color.DeepSkyBlue, Color.Navy };
            Color_gradient_up = Generate_color_gradient_after_interpolation_of_input_colors(intermediate_color_up, max_colors_in_gradient);
            Color_gradient_down = Generate_color_gradient_after_interpolation_of_input_colors(intermediate_color_down, max_colors_in_gradient);
            Color_default = Color.White;
            Color_border = Color.Black;
            Brush_text = Brushes.Black;
            DatasetCols_in_one_heatmap = 8;
            Charts_per_page = Charts_per_page_enum.High;
        }

        private Color[] Generate_color_gradient_after_interpolation_of_input_colors(Color[] intermediate_colors, int max_colors_in_gradient)
        {
            SortedDictionary<float, Color> gradient_sortedDict = new SortedDictionary<float, Color>();
            int intermediate_colors_length = intermediate_colors.Length;
            for (int indexIntermediate = 0; indexIntermediate < intermediate_colors_length; indexIntermediate++)
            {
                gradient_sortedDict.Add(1f * indexIntermediate / (intermediate_colors_length - 1), intermediate_colors[indexIntermediate]);
            }
            List<Color> color_list = new List<Color>();

            using (Bitmap bmp = new Bitmap(max_colors_in_gradient, 1))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle bmpCRect = new Rectangle(Point.Empty, bmp.Size);
                System.Drawing.Drawing2D.ColorBlend color_blend = new System.Drawing.Drawing2D.ColorBlend();
                color_blend.Positions = new float[gradient_sortedDict.Count];
                for (int indexSortedDict = 0; indexSortedDict < gradient_sortedDict.Count; indexSortedDict++)
                {
                    color_blend.Positions[indexSortedDict] = gradient_sortedDict.ElementAt(indexSortedDict).Key;
                }
                color_blend.Colors = gradient_sortedDict.Values.ToArray();
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush
                                        (bmpCRect, Color.Empty, Color.Empty, 0, false))
                {
                    brush.InterpolationColors = color_blend;
                    g.FillRectangle(brush, bmpCRect);
                    for (int i = 0; i < max_colors_in_gradient; i++) color_list.Add(bmp.GetPixel(i, 0));
                }
            }
            return color_list.ToArray();
        }

        public override void Write_option_entries(StreamWriter writer)
        {
            base.Write_entries_excluding_dictionaries(writer, typeof(Heatmap_options_class), "Color_default", "Color_border", "Brush_text",
                                                                      "Heatmap_subdirectory",
                                                                      "Min_scpRows_in_one_heatmap", "Max_scpRows_in_one_heatmap",
                                                                      "DatasetCols_in_one_heatmap",
                                                                      "Width_of_singleGraphPane",
                                                                      "ImageFormat", "Charts_per_page", 
                                                                      "Write_pdf", "Use_scp_abbreviations");
        }
        public override bool Add_read_entry_to_options_and_return_if_successful(string readLine)
        {
            return base.Add_read_entry_and_return_if_succesful(readLine, typeof(Heatmap_options_class));
        }
    }

    class Heatmap_class : Visualization_base_class
    {
        public Heatmap_options_class Options { get; set; }

        public Heatmap_class(ProgressReport_interface_class progressReport) : base(progressReport)
        {
            this.Options = new Heatmap_options_class();
        }
        public override void Set_PDF_generation_allowed(bool pdf_generation_allowed)
        {
            base.PDF_generation_allowed = pdf_generation_allowed;
        }

        private GraphPane Generate_heatmap_legend()
        {
            GraphPane legend_graphPane = new GraphPane();
            List<GraphPane> legend_graphPanes = new List<GraphPane>();
            Dictionary<int, Color> level_color_dict = Global_class.Get_level_scpColor_dict();
            List<string> sampleNames = new List<string>();
            BarItem legend_barItem;
            TextObj text;

            string numbers_meaning_string = "";
            switch (Options.Value_type_selected_for_visualization)
            {
                case Enrichment_value_type_enum.Minus_log10_pvalue:
                    numbers_meaning_string = "Numbers show\n\r-log10(p-values)";
                    break;
                case Enrichment_value_type_enum.Fractional_rank:
                    numbers_meaning_string = "Numbers show\n\rfractional significance ranks";
                    break;
                default:
                    throw new Exception();
            }

            Dictionary<string, Color[]> barLabel_color_dict = new Dictionary<string, Color[]>();
            barLabel_color_dict.Add(" ", new Color[] { Color.White, Color.White });
            barLabel_color_dict.Add("", new Color[] { Color.White, Color.White });
            barLabel_color_dict.Add("  ", new Color[] { Color.White, Color.White });
            barLabel_color_dict.Add("Not significant", new Color[] { Color.White, Color.White });
            barLabel_color_dict.Add("Downregulated", Options.Color_gradient_down);
            barLabel_color_dict.Add("Upregulated", Options.Color_gradient_up);

            Dictionary<string, Color> barLabel_borderColor_dict = new Dictionary<string, Color>();
            barLabel_borderColor_dict.Add(" ", Color.White);
            barLabel_borderColor_dict.Add("", Color.White);
            barLabel_borderColor_dict.Add("  ", Color.White);
            barLabel_borderColor_dict.Add("Upregulated", Color.Black);
            barLabel_borderColor_dict.Add("Downregulated", Color.Black);
            barLabel_borderColor_dict.Add("Not significant", Color.Black);

            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            Dictionary<string, string> barLabel_barText_dict = new Dictionary<string, string>();
            barLabel_barText_dict.Add(" ",gdf.SCP_abbreviations_fileName_reference);
            barLabel_barText_dict.Add("   ", "");
            barLabel_barText_dict.Add("", "-- Significance increases -->>");
            barLabel_barText_dict.Add("  ", numbers_meaning_string);
            barLabel_barText_dict.Add("Upregulated", "");
            barLabel_barText_dict.Add("Downregulated", "");
            barLabel_barText_dict.Add("Not significant", "");

            Dictionary<string, bool> barLabel_boldFont_dict = new Dictionary<string, bool>();
            barLabel_boldFont_dict.Add(" ", false);
            barLabel_boldFont_dict.Add("", true);
            barLabel_boldFont_dict.Add("  ", true);
            barLabel_boldFont_dict.Add("Upregulated", false);
            barLabel_boldFont_dict.Add("Downregulated", false);
            barLabel_boldFont_dict.Add("Not significant", false);

            string[] barLabels = barLabel_color_dict.Keys.ToArray();
            string barLabel;
            int barLabels_length = barLabels.Length;
            Color[] fill_colors;
            string[] y_labels = new string[barLabels_length];
            double[] bar_values;
            bool font_is_bold;
            for (int indexBL = 0; indexBL < barLabels_length; indexBL++)
            {
                barLabel = barLabels[indexBL];
                fill_colors = barLabel_color_dict[barLabel];
                font_is_bold = barLabel_boldFont_dict[barLabel];
                y_labels[indexBL] = barLabel;
                bar_values = new double[barLabels_length];
                bar_values[indexBL] = 1;
                legend_barItem = legend_graphPane.AddBar(barLabel, bar_values, null, Color.Red);
                legend_barItem.Bar.Fill = new Fill(fill_colors);
                legend_barItem.Bar.Border.Color = barLabel_borderColor_dict[barLabel];
                legend_barItem.Bar.Border.Width = 4;
                legend_barItem.Bar.Border.IsVisible = true;
                legend_barItem.Bar.Fill.AlignH = AlignH.Right;
                legend_barItem.Bar.Fill.AlignV = AlignV.Center;

                text = new TextObj(barLabel_barText_dict[barLabel] + indexBL, 0.5, indexBL + 1, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
                text.Text = (string)barLabel_barText_dict[barLabel].Clone();
                text.FontSpec = new FontSpec("Arial", 22, Color.Black, font_is_bold, false, false);
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.StringAlignment = StringAlignment.Center;
                text.FontSpec.Fill.Color = Color.Transparent;
                text.ZOrder = ZOrder.A_InFront;
                legend_graphPane.GraphObjList.Add(text);
            }

            legend_graphPane.Title.Text = "Legend";
            legend_graphPane.Title.FontSpec = new FontSpec("Arial", 30, Color.Black, true, false, false);
            legend_graphPane.Title.FontSpec.Border.IsVisible = false;
            legend_graphPane.Y2Axis.MajorTic.IsBetweenLabels = true;
            legend_graphPane.BarSettings.Type = BarType.Stack;
            legend_graphPane.BarSettings.Base = BarBase.Y;
            legend_graphPane.X2Axis.IsVisible = false;
            legend_graphPane.Y2Axis.IsVisible = false;
            legend_graphPane.XAxis.MajorGrid.IsZeroLine = true;
            legend_graphPane.XAxis.MajorGrid.IsVisible = false;
            legend_graphPane.XAxis.MinorGrid.IsVisible = false;
            legend_graphPane.XAxis.MajorTic.IsOpposite = false;
            legend_graphPane.XAxis.MinorTic.IsOpposite = false;
            legend_graphPane.YAxis.MajorTic.IsOpposite = false;
            legend_graphPane.YAxis.MinorTic.IsOpposite = false;
            legend_graphPane.XAxis.Color = Color.Black;
            legend_graphPane.YAxis.Color = Color.Transparent;
            legend_graphPane.YAxis.MinSpace = 0;
            legend_graphPane.YAxis.Scale.MinorStep = 1;
            legend_graphPane.YAxis.Scale.MajorStep = 1;
            legend_graphPane.YAxis.IsVisible = true;
            legend_graphPane.XAxis.IsVisible = false;
            legend_graphPane.XAxis.MajorGrid.Color = Color.Black;
            legend_graphPane.YAxis.MajorGrid.IsVisible = false;
            legend_graphPane.YAxis.MajorTic.Color = Color.Transparent;
            legend_graphPane.YAxis.MinorTic.Color = Color.Transparent;
            legend_graphPane.YAxis.Type = AxisType.Text;
            legend_graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 25, Color.Black, true, false, false);
            legend_graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
            legend_graphPane.YAxis.Scale.FontSpec.Angle = 90;
            legend_graphPane.YAxis.Scale.TextLabels = y_labels.ToArray();
            legend_graphPane.YAxis.Scale.AlignH = AlignH.Center;
            legend_graphPane.YAxis.Scale.Align = AlignP.Inside;
            legend_graphPane.YAxis.Scale.Max = barLabels_length + 1;
            legend_graphPane.YAxis.Scale.Min = -0.5;
            legend_graphPane.XAxis.Scale.Max = 1;
            legend_graphPane.XAxis.Scale.Min = 0;
            legend_graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
            legend_graphPane.YAxis.Scale.IsPreventLabelOverlap = false;
            legend_graphPane.Chart.Border.IsVisible = false;
            legend_graphPane.Border.IsVisible = false;
            legend_graphPane.Legend.IsVisible = false;


            return legend_graphPane;
        }
        private GraphPane Generate_heatmap_graphPane_for_same_level_and_integrationGroup_enrichment_results(Ontology_enrichment_line_class[] sameLevel_integrationGroup_enrich_lines, float to_gradient_factor, Enrichment_algorithm_enum enrichment_algorithm, string addToHeadline, float heatmap_innerplot_width_percent, float heatmap_innerplot_height_percent, int scpRows_in_heatmap, bool is_mbco_ontology)
        {
            //sameLevel_integrationGroup_enrich_lines = sameLevel_integrationGroup_enrich_lines.OrderBy(l => l.Scp_name).ThenBy(l => l.Results_number).ToArray();
            #region Get set row and col indexes for all scps and uniqueDatasetNames and check if only one integration group and level
            sameLevel_integrationGroup_enrich_lines = Ontology_enrichment_line_class.Order_by_resultsNo(sameLevel_integrationGroup_enrich_lines);
            Ontology_enrichment_line_class enrich_line;
            int sameLevel_integrationGroup_enrich_lines_length = sameLevel_integrationGroup_enrich_lines.Length;
            Dictionary<string, int> uniqueDatasetName_colIndex_dict = new Dictionary<string, int>();
            int colIndex = -1;
            string integrationGroup = sameLevel_integrationGroup_enrich_lines[0].IntegrationGroup;
            int level = sameLevel_integrationGroup_enrich_lines[0].ProcessLevel;
            for (int indexE=0; indexE<sameLevel_integrationGroup_enrich_lines_length; indexE++)
            {
                enrich_line = sameLevel_integrationGroup_enrich_lines[indexE];
                if (enrich_line.ProcessLevel != level) { throw new Exception(); }
                if (!enrich_line.IntegrationGroup.Equals(integrationGroup)) { throw new Exception(); }
                if (!uniqueDatasetName_colIndex_dict.ContainsKey(enrich_line.Unique_dataset_name))
                {
                    colIndex++;
                    uniqueDatasetName_colIndex_dict.Add(enrich_line.Unique_dataset_name, colIndex);
                }
            }
            sameLevel_integrationGroup_enrich_lines = Ontology_enrichment_line_class.Order_by_scpName(sameLevel_integrationGroup_enrich_lines);
            Dictionary<string, int> scpName_rowIndex_dict = new Dictionary<string, int>();
            int rowIndex = scpRows_in_heatmap;
            for (int indexE = 0; indexE < sameLevel_integrationGroup_enrich_lines_length; indexE++)
            {
                enrich_line = sameLevel_integrationGroup_enrich_lines[indexE];
                if (!scpName_rowIndex_dict.ContainsKey(enrich_line.Scp_name))
                {
                    rowIndex--;
                    scpName_rowIndex_dict.Add(enrich_line.Scp_name, rowIndex);
                }
            }
            int rowIndex_of_abscissa = rowIndex;
            #endregion

            string integrationGroup_level = integrationGroup + " SCP level " + level;

            //float text_factor = 1;
            //if (Options.Write_pdf) { text_factor = 1.25F; }

            string value_type_string = "";
            string enrichment_algorithm_string = "";
            switch (enrichment_algorithm)
            {
                case Enrichment_algorithm_enum.Dynamic_enrichment:
                    enrichment_algorithm_string = "Dynamic enrichment analysis";
                    break;
                case Enrichment_algorithm_enum.Standard_enrichment:
                    enrichment_algorithm_string = "Standard enrichment analysis";
                    break;
                default:
                    throw new Exception();
            }
            switch (Options.Value_type_selected_for_visualization)
            {
                case Enrichment_value_type_enum.Fractional_rank:
                    value_type_string = "Fractional ranks";
                    break;
                case Enrichment_value_type_enum.Minus_log10_pvalue:
                    value_type_string = "-log10(p-values)";
                    break;
                default:
                    throw new Exception();
            }
            string heatmap_chart_name = "";
            if (is_mbco_ontology)
            {
                heatmap_chart_name = integrationGroup_level + addToHeadline + "\r\n(" + enrichment_algorithm_string + ": " + value_type_string + ")";
            }
            else
            {
                heatmap_chart_name = addToHeadline + "\r\n(" + enrichment_algorithm_string + ": " + value_type_string + ")";
            }
            float textSize_scale_factor = -1;
            switch (Options.Charts_per_page)
            {
                case Charts_per_page_enum.Low:
                    textSize_scale_factor = 1.1F;
                    break;
                case Charts_per_page_enum.Medium:
                    textSize_scale_factor = 0.6F;
                    break;
                case Charts_per_page_enum.High:
                    textSize_scale_factor = 1.0F;
                    break;
                default:
                    throw new Exception();
            }
            GraphPane heatmap_graphPane = new GraphPane();
            heatmap_graphPane.IsAlignGrids = true;
            heatmap_graphPane.X2Axis.IsVisible = false;
            heatmap_graphPane.Y2Axis.IsVisible = false;
            heatmap_graphPane.XAxis.IsVisible = true;
            heatmap_graphPane.Margin.All = 1;
            heatmap_graphPane.Border.Color = Color.White;
            heatmap_graphPane.XAxis.MajorTic.IsOpposite = false;
            heatmap_graphPane.XAxis.MinorTic.IsOpposite = false;
            heatmap_graphPane.YAxis.MajorTic.IsOpposite = false;
            heatmap_graphPane.YAxis.MinorTic.IsOpposite = false;
            heatmap_graphPane.Title.Text = (string)heatmap_chart_name.Clone();
            heatmap_graphPane.Title.FontSpec = new FontSpec("Arial", (int)Math.Round(textSize_scale_factor * 18), Color.Black, true, false, false);
            heatmap_graphPane.Title.FontSpec.Border.IsVisible = false;
            heatmap_graphPane.YAxis.Title.Text = "";
            heatmap_graphPane.XAxis.Title.Text = "";
            heatmap_graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", (int)Math.Round(textSize_scale_factor * 13), Color.Black, false, false, false);
            heatmap_graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
            heatmap_graphPane.XAxis.Scale.FontSpec.StringAlignment = StringAlignment.Far;
            heatmap_graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", (int)Math.Round(textSize_scale_factor * 13), Color.Black, false, false, false);
            heatmap_graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
            heatmap_graphPane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Far;
            heatmap_graphPane.XAxis.Scale.FontSpec.Angle = 90;
            heatmap_graphPane.YAxis.Scale.FontSpec.Angle = 90;
            heatmap_graphPane.Chart.Border.IsVisible = false;
            heatmap_graphPane.BarSettings.ClusterScaleWidth = 0.5F;

            Color[] up_color_gradient = new Color[0];
            Color[] down_color_gradient = new Color[0];
            Color default_color = Options.Color_default;
            switch (Options.Value_type_selected_for_visualization)
            {
                case Enrichment_value_type_enum.Minus_log10_pvalue:
                    up_color_gradient = Options.Color_gradient_up;
                    down_color_gradient = Options.Color_gradient_down;
                    break;
                case Enrichment_value_type_enum.Fractional_rank:
                    up_color_gradient = Options.Reverse_color_gradient_up;
                    down_color_gradient = Options.Reverse_color_gradient_down;
                    break;
                default:
                    throw new Exception();
            }
            if (up_color_gradient.Length != down_color_gradient.Length) { throw new Exception(); }
            int current_colIndex = -1;
            int current_rowIndex = scpRows_in_heatmap;
            int indexColorGradient = -1;
            int enrich_lines_length = sameLevel_integrationGroup_enrich_lines.Length;
            Color image_color = Color.White;
            string image_value = "";
            Color border_color = Options.Color_border;
            float rounded_minusLog10Pvalue = -1;
            string used_scp_name;
            float column_width = 1F / Options.DatasetCols_in_one_heatmap;
            float row_height = 1F / scpRows_in_heatmap;

            #region Generate x and y labels
            string[] uniqueDatasetNames = uniqueDatasetName_colIndex_dict.Keys.ToArray();
            string uniqueDatasetName;
            int uniqueDatasetNames_length = uniqueDatasetNames.Length;
            string[] x_axis_labels = new string[uniqueDatasetNames_length];
            for (int indexUDN=0; indexUDN < uniqueDatasetNames_length; indexUDN++)
            {
                uniqueDatasetName = uniqueDatasetNames[indexUDN];
                x_axis_labels[uniqueDatasetName_colIndex_dict[uniqueDatasetName]] = (string)uniqueDatasetName.Clone();
            }
            string[] scpNames = scpName_rowIndex_dict.Keys.ToArray();
            string scpName;
            int scpNames_length = scpNames.Length;
            string[] y_axis_labels = new string[scpRows_in_heatmap];
            for (int indexY=0; indexY<scpRows_in_heatmap;indexY++) { y_axis_labels[indexY] = ""; }
            for (int indexScpName = 0; indexScpName < scpNames_length; indexScpName++)
            {
                scpName = scpNames[indexScpName];
                if (Options.Use_scp_abbreviations)
                { used_scp_name = Get_shortened_scp_name(scpName); }
                else { used_scp_name = scpName; }
                y_axis_labels[scpName_rowIndex_dict[scpName]] = (string)used_scp_name.Clone();
            }
            #endregion

            for (int indexE = 0; indexE < enrich_lines_length; indexE++)
            {
                enrich_line = sameLevel_integrationGroup_enrich_lines[indexE];
                current_rowIndex = scpName_rowIndex_dict[enrich_line.Scp_name];
                current_colIndex = uniqueDatasetName_colIndex_dict[enrich_line.Unique_dataset_name];
                switch (Options.Value_type_selected_for_visualization)
                {
                    case Enrichment_value_type_enum.Minus_log10_pvalue:
                        if (enrich_line.Significant)
                        {
                            indexColorGradient = (int)Math.Round(to_gradient_factor * enrich_line.Minus_log10_pvalue);
                        }
                        else { indexColorGradient = 0; }
                        rounded_minusLog10Pvalue = (float)Math.Round(enrich_line.Minus_log10_pvalue * 10) / 10F;
                        if (rounded_minusLog10Pvalue > 0)
                        { image_value = (Math.Round(enrich_line.Minus_log10_pvalue * 10) / 10).ToString(); }
                        else { image_value = ""; }
                        break;
                    case Enrichment_value_type_enum.Fractional_rank:
                        if (enrich_line.Significant)
                        {
                            indexColorGradient = (int)Math.Round(to_gradient_factor * enrich_line.Fractional_rank);
                        }
                        if (enrich_line.Fractional_rank < 100)
                        { image_value = (Math.Round(enrich_line.Fractional_rank * 10) / 10).ToString(); }
                        else { image_value = ""; }
                        break;
                    default:
                        throw new Exception();
                }
                if (enrich_line.Significant)
                {
                    switch (enrich_line.EntryType)
                    {
                        case Entry_type_enum.Up:
                            image_color = up_color_gradient[indexColorGradient];
                            break;
                        case Entry_type_enum.Down:
                            image_color = down_color_gradient[indexColorGradient];
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else { image_color = default_color; }

                int text_nchar = image_value.Length;
                float fontSize = 20;
                if (text_nchar == 1) { fontSize = 23F; }
                else if (text_nchar == 2) { fontSize = 20F; }
                else if (text_nchar == 3) { fontSize = 17; }
                else if (text_nchar == 4) { fontSize = 13F; }
                else { fontSize = 10F; }
                fontSize = (int)Math.Round(textSize_scale_factor * fontSize);

                TextObj text = new TextObj(image_value, current_colIndex * column_width + 0.5 * column_width, 1 - (current_rowIndex + 1) * row_height + 0.5 * row_height, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
                text.Location.CoordinateFrame = CoordType.ChartFraction;
                text.Text = image_value;
                text.FontSpec = new FontSpec("Arial", fontSize, Color.Black, false, false, false);
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.Color = image_color;
                text.ZOrder = ZOrder.A_InFront;

                heatmap_graphPane.GraphObjList.Add(text);
                BoxObj box = new BoxObj(current_colIndex * column_width, 1 - (current_rowIndex + 1) * row_height, column_width, row_height, border_color, image_color);
                box.Location.CoordinateFrame = CoordType.ChartFraction;
                box.Fill = new Fill(image_color);
                box.ZOrder = ZOrder.E_BehindCurves;
                heatmap_graphPane.GraphObjList.Add(box);
            }
            int y_axis_labels_length = y_axis_labels.Length;
            heatmap_graphPane.XAxis.Scale.Min = 0.5;
            heatmap_graphPane.YAxis.Scale.Min = 0.5;
            heatmap_graphPane.XAxis.Scale.Max = Options.DatasetCols_in_one_heatmap + 0.5;
            heatmap_graphPane.YAxis.Scale.Max = scpRows_in_heatmap + 0.5;
            heatmap_graphPane.XAxis.Scale.MajorStep = 1;
            heatmap_graphPane.YAxis.Scale.MajorStep = 1;
            heatmap_graphPane.XAxis.Scale.MinorStep = 1;
            heatmap_graphPane.YAxis.Scale.MinorStep = 1;
            heatmap_graphPane.XAxis.MajorTic.IsBetweenLabels = true;
            heatmap_graphPane.YAxis.MajorTic.IsBetweenLabels = true;
            heatmap_graphPane.XAxis.MajorGrid.IsVisible = true;
            heatmap_graphPane.YAxis.MajorGrid.IsVisible = false;
            heatmap_graphPane.XAxis.MinorGrid.IsVisible = true;
            heatmap_graphPane.YAxis.MinorGrid.IsVisible = false;
            heatmap_graphPane.XAxis.MajorTic.Color = Color.Black;
            heatmap_graphPane.YAxis.MajorTic.Color = Color.Transparent;
            heatmap_graphPane.XAxis.MinorTic.Color = Color.Black;
            heatmap_graphPane.YAxis.MinorTic.Color = Color.Transparent;
            heatmap_graphPane.XAxis.Scale.TextLabels = Text_class.Split_texts_over_multiple_lines(x_axis_labels.ToArray(), 1, 2);
            string[] textLabels = new string[] { };
            if (Options.Use_scp_abbreviations)
            { textLabels = Get_shortened_scp_names(y_axis_labels); }
            else { textLabels = y_axis_labels; }
            heatmap_graphPane.YAxis.Scale.TextLabels = Text_class.Split_texts_over_multiple_lines(textLabels, 1, 2);

            heatmap_graphPane.BarSettings.Base = BarBase.Y;

            heatmap_graphPane.XAxis.Cross = rowIndex_of_abscissa + 0.5;
            heatmap_graphPane.XAxis.Scale.AlignH = AlignH.Center;
            heatmap_graphPane.XAxis.Scale.Align = AlignP.Inside;

            heatmap_graphPane.YAxis.Scale.AlignH = AlignH.Center;
            heatmap_graphPane.YAxis.Scale.Align = AlignP.Inside;
            heatmap_graphPane.XAxis.Type = AxisType.Text;
            heatmap_graphPane.YAxis.Type = AxisType.Text;
            heatmap_graphPane.XAxis.MajorTic.Color = Color.Transparent;
            heatmap_graphPane.YAxis.MajorTic.Color = Color.Transparent;
            heatmap_graphPane.XAxis.MinorTic.Color = Color.Transparent;
            heatmap_graphPane.YAxis.MinorTic.Color = Color.Transparent;
            heatmap_graphPane.XAxis.MajorGrid.IsVisible = true;
            heatmap_graphPane.YAxis.MinorGrid.IsVisible = true;
            heatmap_graphPane.XAxis.Color = Color.Transparent;
            heatmap_graphPane.YAxis.Color = Color.Transparent;
            heatmap_graphPane.XAxis.MajorGrid.Color = Color.Transparent;
            heatmap_graphPane.YAxis.MajorGrid.Color = Color.Transparent;
            heatmap_graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
            heatmap_graphPane.YAxis.Scale.IsPreventLabelOverlap = false;

            return heatmap_graphPane;
        }

        private List<MasterPane> Generate_and_add_heatmap_masterPanes_for_current_datasetGroup(List<MasterPane> heatmap_masterPanes, Ontology_enrichment_line_class[] currentLevelDataset_enrichment_lines, Enrichment_algorithm_enum enrichment_algorithm, float gradient_factor, int sameLevel_rowBlock_no, int sameLevel_total_rowBlocks_count, ref int sameLevel_colBlock_no, int sameLevel_total_colBlocks_count, ref bool addGroupNo_to_headline, bool is_mbco_ontology)
        {
            int processLevel = currentLevelDataset_enrichment_lines[0].ProcessLevel;
            string integrationGroup = currentLevelDataset_enrichment_lines[0].IntegrationGroup;
            //currentLevelDataset_enrichment_lines = currentLevelDataset_enrichment_lines.OrderBy(l => l.Unique_dataset_name).ToArray();
            currentLevelDataset_enrichment_lines = Ontology_enrichment_line_class.Order_by_uniqueDatasetName(currentLevelDataset_enrichment_lines);

            Ontology_enrichment_line_class enrich_line;
            int enrich_length = currentLevelDataset_enrichment_lines.Length;

            List<Ontology_enrichment_line_class> sameLevel_scpName_enrich_lines = new List<Ontology_enrichment_line_class>();
            DataGridView dgv = new DataGridView();
            int scpCols_in_heatmap = Options.DatasetCols_in_one_heatmap;
            int heatmap_innerplot_width_pixel = 60 * scpCols_in_heatmap;
            int heatmap_width = 1500 + heatmap_innerplot_width_pixel;
            int heatmap_innerplot_width_percent = 100 * heatmap_innerplot_width_pixel / heatmap_width;
            int scpRows_in_heatmap = Options.Max_scpRows_in_one_heatmap;
            int heatmap_innerplot_pixel_height = 60 * scpRows_in_heatmap;
            int heatmap_height = 900 + heatmap_innerplot_pixel_height;
            int heatmap_innerplot_height_percent = 100 * heatmap_innerplot_pixel_height / heatmap_height;
            int total_images_count = Options.Figures_per_chart;
            int added_uniqueDatasets_count = 0;
            StringBuilder sb_fileName = new StringBuilder();
            GraphPane heatmap_graphPane = new GraphPane();
            MasterPane heatmap_masterPane = new MasterPane();
            if (heatmap_masterPanes.Count > 0)
            {
                heatmap_masterPane = heatmap_masterPanes[heatmap_masterPanes.Count - 1];
            }
            if ((heatmap_masterPanes.Count == 0)
                || (heatmap_masterPane.PaneList.Count == total_images_count))
            {
                heatmap_masterPane = new MasterPane();
                heatmap_masterPanes.Add(heatmap_masterPane);
            }
            int added_lines_count = 0;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = currentLevelDataset_enrichment_lines[indexE];
                if (!enrich_line.IntegrationGroup.Equals(integrationGroup)) { throw new Exception(); }
                if (!enrich_line.ProcessLevel.Equals(processLevel)) { throw new Exception(); }
                if (added_uniqueDatasets_count == scpCols_in_heatmap)
                {
                    sameLevel_scpName_enrich_lines.Clear();
                    added_uniqueDatasets_count = 0;
                }
                sameLevel_scpName_enrich_lines.Add(enrich_line);
                if ((indexE == enrich_length - 1)
                    || (!enrich_line.Unique_dataset_name.Equals(currentLevelDataset_enrichment_lines[indexE + 1].Unique_dataset_name)))
                {
                    added_uniqueDatasets_count++;
                }
                if ((indexE == enrich_length - 1)
                    || (added_uniqueDatasets_count == scpCols_in_heatmap))
                {
                    sameLevel_colBlock_no++;
                    string addToHeadline = "";
                    if (indexE != enrich_length - 1) { addGroupNo_to_headline = true; }
                    if (addGroupNo_to_headline) { addToHeadline = " (col: " + sameLevel_colBlock_no + "/" + sameLevel_total_colBlocks_count + ", row: " + sameLevel_rowBlock_no + "/" + sameLevel_total_rowBlocks_count + ")"; }
                    heatmap_graphPane = Generate_heatmap_graphPane_for_same_level_and_integrationGroup_enrichment_results(sameLevel_scpName_enrich_lines.ToArray(), gradient_factor, enrichment_algorithm, addToHeadline, heatmap_innerplot_width_percent, heatmap_innerplot_height_percent, scpRows_in_heatmap, is_mbco_ontology);
                    heatmap_masterPane.Add(heatmap_graphPane);
                    added_lines_count += sameLevel_scpName_enrich_lines.Count;
                    if (heatmap_masterPane.PaneList.Count == total_images_count)
                    {
                        heatmap_masterPane = new MasterPane();
                        heatmap_masterPanes.Add(heatmap_masterPane);
                    }
                }
            }
            if (added_lines_count!=enrich_length) { throw new Exception(); }
            return heatmap_masterPanes;
        }

        private List<MasterPane> Generate_and_add_heatmap_masterPanes_for_current_level(Ontology_enrichment_class sameLevel_enrich, Enrichment_algorithm_enum enrichment_algorithm, List<MasterPane> heatmap_masterPanes, Form1_default_settings_class form_default_settings)
        {
            string integrationGroup = sameLevel_enrich.Enrich[0].IntegrationGroup;
            int level = sameLevel_enrich.Enrich[0].ProcessLevel;

            int scps_length = sameLevel_enrich.Get_all_scps().Length;
            int uniqueDatasets_length = sameLevel_enrich.Get_all_uniqueDatasetNames().Length;
            int sameLevel_total_rowBlocks_count = (int)Math.Ceiling((float)scps_length / Options.Max_scpRows_in_one_heatmap);
            int sameLevel_total_colBlocks_count = (int)Math.Ceiling((float)uniqueDatasets_length / Options.DatasetCols_in_one_heatmap);

            Ontology_type_enum ontology = sameLevel_enrich.Get_ontology_and_check_if_only_one_ontology();
            bool is_mbco_ontology = Ontology_classification_class.Is_mbco_ontology(ontology);
            int enrich_length = sameLevel_enrich.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            float max_minusLog10_pvalue = -1;
            float max_fractionalRank = -1;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = sameLevel_enrich.Enrich[indexE];
                if (!enrich_line.ProcessLevel.Equals(level)) { throw new Exception(); }
                if (!enrich_line.IntegrationGroup.Equals(integrationGroup)) { throw new Exception(); }
                if (enrich_line.Significant)
                {
                    if ((max_minusLog10_pvalue == -1) || (enrich_line.Minus_log10_pvalue > max_minusLog10_pvalue)) { max_minusLog10_pvalue = enrich_line.Minus_log10_pvalue; }
                    if ((max_fractionalRank == -1) || (enrich_line.Fractional_rank > max_fractionalRank)) { max_fractionalRank = enrich_line.Fractional_rank; }
                }
            }
            float gradient_factor = -1F;
            if (Options.Color_gradient_up.Length != Options.Color_gradient_down.Length) { throw new Exception(); }
            switch (Options.Value_type_selected_for_visualization)
            {
                case Enrichment_value_type_enum.Minus_log10_pvalue:
                    gradient_factor = 1F / ((float)(max_minusLog10_pvalue + 1) / (float)(Options.Color_gradient_up.Length - 1));
                    break;
                case Enrichment_value_type_enum.Fractional_rank:
                    gradient_factor = 1F / ((float)(max_fractionalRank + 1) / (float)(Options.Color_gradient_up.Length - 1));
                    break;
                default:
                    throw new Exception();
            }
            List<Ontology_enrichment_line_class> currentMax_uniqueDatasetName_enrich_lines = new List<Ontology_enrichment_line_class>();
            //enrich.Order_by_level_results_no();
            sameLevel_enrich.Order_by_scpName();
            int scpName_count = 0;
            int sameLevel_rowBlock_no = 0;
            int sameLevel_colBlock_no = 0;
            bool add_groupNo_to_headline = false;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = sameLevel_enrich.Enrich[indexE];
                if ((indexE == enrich_length - 1)
                    || (!enrich_line.Scp_name.Equals(sameLevel_enrich.Enrich[indexE + 1].Scp_name)))
                {
                    scpName_count++;
                }
                currentMax_uniqueDatasetName_enrich_lines.Add(enrich_line);
                if ((indexE == enrich_length - 1)
                    || (scpName_count == Options.Max_scpRows_in_one_heatmap))
                {
                    sameLevel_rowBlock_no++;
                    sameLevel_colBlock_no = 0;
                    if (indexE != enrich_length - 1)
                    {
                        add_groupNo_to_headline = true;
                    }
                    heatmap_masterPanes = Generate_and_add_heatmap_masterPanes_for_current_datasetGroup(heatmap_masterPanes, currentMax_uniqueDatasetName_enrich_lines.ToArray(), enrichment_algorithm, gradient_factor, sameLevel_rowBlock_no, sameLevel_total_rowBlocks_count, ref sameLevel_colBlock_no, sameLevel_total_colBlocks_count, ref add_groupNo_to_headline, is_mbco_ontology);
                    currentMax_uniqueDatasetName_enrich_lines.Clear();
                    scpName_count = 0;
                }
            }
            return heatmap_masterPanes;
        }

        public GraphPane[] Generate_heatmaps_and_return_graphPanes(Ontology_enrichment_class enrich_input, string results_directory, string heatmap_baseFileName, Enrichment_algorithm_enum enrichment_algorithm, Form1_default_settings_class form_default_settings)
        {
            Ontology_enrichment_class enrich = enrich_input.Deep_copy();
            Ontology_type_enum ontology = enrich.Get_ontology_and_check_if_only_one_ontology();
            if (!Ontology_classification_class.Is_mbco_ontology(ontology))
            {
                enrich.Set_all_processLevels_to_input_level(1);
            }
            enrich.Order_by_level();
            MasterPane heatmap_masterPane;
            List<MasterPane> heatmap_masterPanes = new List<MasterPane>();
            Ontology_enrichment_class sameLevel_enrich = new Ontology_enrichment_class();
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> sameLevel_enrich_lines = new List<Ontology_enrichment_line_class>();
            int enrich_length = enrich.Enrich.Length;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrich.Enrich[indexE];
                if ((indexE == 0)
                    || (!enrich_line.ProcessLevel.Equals(enrich.Enrich[indexE - 1].ProcessLevel)))
                {
                    sameLevel_enrich_lines.Clear();
                    sameLevel_enrich = new Ontology_enrichment_class();
                }
                sameLevel_enrich_lines.Add(enrich_line);
                if ((indexE == enrich_length - 1)
                    || (!enrich_line.ProcessLevel.Equals(enrich.Enrich[indexE + 1].ProcessLevel)))
                {
                    sameLevel_enrich.Enrich = sameLevel_enrich_lines.ToArray();
                    heatmap_masterPanes = Generate_and_add_heatmap_masterPanes_for_current_level(sameLevel_enrich, enrichment_algorithm, heatmap_masterPanes, form_default_settings);
                }
            }

            PdfDocument pdf_document = new PdfDocument();
            StringBuilder sb_fileName = new StringBuilder();

            GraphPane legend_graphPane = Generate_heatmap_legend();
            if (heatmap_masterPanes[heatmap_masterPanes.Count - 1].PaneList.Count < Options.Figures_per_chart)
            {
                heatmap_masterPanes[heatmap_masterPanes.Count - 1].PaneList.Add(legend_graphPane);
            }
            else
            {
                heatmap_masterPane = new MasterPane();
                heatmap_masterPane.PaneList.Add(legend_graphPane);
                heatmap_masterPanes.Add(heatmap_masterPane);
            }
            if (!Options.Write_pdf)
            {
                int heatmaps_length = heatmap_masterPanes.Count;
                string heatmap_no_string;
                for (int indexHeat = 0; indexHeat < heatmaps_length; indexHeat++)
                {
                    heatmap_masterPane = heatmap_masterPanes[indexHeat];
                    if (heatmap_masterPane.PaneList.Count > 0)
                    {
                        heatmap_no_string = (indexHeat + 1).ToString();
                        while (heatmap_no_string.Length < heatmaps_length.ToString().Length)
                        {
                            heatmap_no_string = "0" + heatmap_no_string;
                        }
                        sb_fileName.Clear();
                        if (heatmap_baseFileName.Length > 0) { sb_fileName.AppendFormat("{0}_h", heatmap_baseFileName); }
                        else { sb_fileName.AppendFormat("H"); }
                        sb_fileName.AppendFormat("eatmap");
                        if (heatmaps_length > 1) { sb_fileName.AppendFormat("_no{0}", heatmap_no_string); }
                        sb_fileName.AppendFormat(Options.Image_fileName_extension);
                        string heatmap_results_directory = results_directory + Options.Heatmap_subdirectory;
                        ReadWriteClass.Create_directory_if_it_does_not_exist(heatmap_results_directory);
                        Write_masterPane(heatmap_masterPane, heatmap_results_directory + sb_fileName.ToString(), Options.ImageFormat, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
                    }
                }
            }
            else
            {
                base.Add_masterPanes_to_pdf(ref pdf_document, heatmap_masterPanes, Options.Figures_per_chart, Options.Width_of_singleGraphPane);
                base.Write_and_dispose_pdf_document_if_allowed(ref pdf_document, results_directory + heatmap_baseFileName + "_heatmaps.pdf");
            }
            GraphPane[] graphPanes = base.Get_all_graphPanes_from_masterPanes(heatmap_masterPanes.ToArray());
            return graphPanes;
        }
    }
}
