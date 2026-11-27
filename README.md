<h1>Molecular-Biology-of-the-Cell Ontology (MBCO)</h1>
<h1>MBC PathNet - Generation of pathway networks from multiomics datasets</h1>
<br>
See www.iyengarlab.org/pathnet/ for a brief introduction of MBC PathNet.<br>
<br>
Please acknowledge the MBC Ontologies and MBC PathNet in your publications by citing the following references:<br>
<br>
MBCO v2:<br>
Functional cooperation, adaptation and synchronization of subcellular processes in cellular neighborhoods as inferred from spatial transcriptomics. Jens Hansen, Ravi Iyengar. bioRxiv 2026.11.24.768887. https://doi.org/10.64898/2026.11.24.768887<br>
<br>
MBCO v1:<br>
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes Sci Rep. 2017 Dec 18;7(1):17689. https://doi.org/10.1038/s41598-017-16627-4.<br>
<br>
MBC PathNet:<br>
Jens Hansen, Ravi Iyengar. MBC PathNet: Integration and visualization of networks connecting functionally related pathways predicted from transcriptomic and proteomic datasets. Bioinform Adv. 2025 Aug 18;5(1). PMID: 40917650. https://doi.org/10.1093/bioadv/vbaf197<br>
<br>
Additional MBCO datasets:<br>
Hansen, Sealfon, Menon et al. for the Kidney Precision Medicine Project. A reference tissue atlas for the human kidney. Sci Adv. 2022 Jun 10;8(23). PMID: 35675394. https://doi.org/10.1126/sciadv.abn4965<br>
<br>
<b><i><u>MBCO v2 has been released as a beta version, since it has not been peer-reviewed yet.</u></i></b><br>
<br>
<b><i><u>The last update of MBC PathNet was uploaded on November 27, 2026.</u></i></b><br>
<br>
To use the application, download the zip-folder by pressing the green 'Code'-button on the upper right side of this GitHub internet page. After unpacking of the downloaded folder, copy its subfolder "MBC_PathNet" to your hard drive. Avoid long directory names, as they can prevent the application from writing result files.<br>
<br>
<h2>Windows</h2>
The windows application can be started by opening "MBC_PathNet.exe" (file type "Application").<br>
<br>
<h2>Linux</h2>
To start the Linux application, download the 'mono-xsp4' or 'mono-devel' package from 'mono-project.com' ("an open source implementation of Microsoft's .NET Framework","sponsored by Microsoft"). Open the application from the terminal with the command "mono MBC_PathNet.exe".<br>
Briefly, the following commands can be used:<br>
<br>
sudo apt update<br>
sudo apt install mono-xsp4<br>
cd your_directory\MBC_PathNet<br>
mono MBC_PathNet.exe<br>
<br>
<h2>Network visualization</h2>
The application generates pathway networks that can be visualized using the yED Graph Editor or Cytoscape. See 'Download_all_datasets'-files for download information.<br>
<br>
<h2>Third-party datasets</h2>
Follow the instructions in the 'Download_all_datasets'-files to download all third-party datasets that the application will use, if related functionalites are selected. To import ontologies that cannot explicitly be selected in the application, follow the instructions in the 'Prepare_custom_ontoloy'-file.<br>
<br>
<h2>Command line/R mode</h2>
Follow the instructions in the 'CommandLine_Automation_Guide'-file to launch the application from the command line, skipping any interactions with the user-interface.<br>
The script 'MBC_pathNet_launch_within_R.R' contains code that enables launching the application directly from within R.
<br>
<h2>Operating systems</h2>
MBC_PathNet was tested on the following systems and operating systems:<br>
1)	High-performance computer, 40 cores, ~400 GB RAM: Windows 10 Pro 2009 & Ubuntu 24.02.2 LTS in test mode<br>
2)	2nd high-performance computer, 40 cores, ~400 GB RAM: Ubuntu debian 22.04 LTS<br>
3)	Laptop, 16 GB RAM: Windows 11 & Ubuntu 24.02.2 LTS in test mode<br>
<br>
<h2>Additional datasets and code</h2>
The folder 'Additional MBCO datasets' contains two ontologies that map genes to metabolic energy pathways as well as to pathways involved in sodium and glucose transmembrane transport. They were both developed within the Kidney Precision Medicine Project (Hansen, Sealfon, Menon et al, Sci Adv. 2022).<br>
The folders "MBCO_enrichment" and "MBCO_dataset" contain a C#-script and datasets that were uploaded in 2017. All datasets and code with relevance for the 'MBC_PathNet' are within the 'MBCO_windows_application' folder, so that these directories can simply be ignored.
