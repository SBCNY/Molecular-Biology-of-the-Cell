<h1>Molecular-Biology-of-the-Cell Ontology (MBCO)</h1>
<h1>MBC PathNet - Generation of pathway networks from multiomics datasets</h1>
<br>
<b><i><u>The last update of MBC PathNet was uploaded on June 02, 2025.</u></i></b><br>
<br>
To use the application, download the zip-folder by pressing the green 'Code'-button on the upper right side of this GitHub internet page. After unpacking of the downloaded folder, copy its subfolder "MBCO_windows_application" to your hard drive.<br>
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
cd your_directory\MBCO_windows_application<br>
mono MBC_PathNet.exe<br>
<br>
<h2>Network visualization</h2>
The application generates pathway networks that can be visualized using the yED Graph Editor or Cytoscape. See 'Download_all_datasets'-files for download information.<br>
<br>
<h2>Third-party datasets</h2>
Follow the instructions in the 'Download_all_datasets', and 'Prepare_custom_ontoloy'-files to download all datasets that the application will use, if related functionalites are selected.<br>
<br>
<h2>Command line mode</h2>
Follow the instructions in the 'CommandLine_Automation_Guide' to start the application from the command line without interaction with the user-interface.<br>
<br>
<h2>Additional datasets and code</h2>
The folder 'Additional MBCO datasets' contains two ontologies that map genes to metabolic energy pathways as well as to pathways involved in sodium and glucose transmembrane transport. They were both developed within the Kidney Precision Medicine Project (Hansen, Sealfon, Menon et al, Sci Adv. 2022).<br>
The folders "MBCO_enrichment" and "MBCO_dataset" contain a C#-script and datasets that were uploaded in 2017. All datasets and code with relevance for the 'MBC_PathNet' are within the 'MBCO_windows_application' folder, so that these directories can simply be ignored.
