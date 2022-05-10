# Molecular-Biology-of-the-Cell Ontology (MBCO) - Executable Windows application
The folder "MBCO_windows_application" contains an excecutable windows application that allows scientists to analyze their data.
Simply copy the folder "MBCO_windows_application" to any location on your hard drive and start the program by opening "Molecular_Biology_of_the_Cell_Ontology.exe".

This folder also contains MBCO version 1.1.

<i>Note: If your screen resolution is too high (e.g. 3840 x 2160) the current windows form will look compressed. In this case, lower your screen resolution (e.g. to 2560x1600) before opening the application. You can reset your screen resolution to the original value again, once the application started.</i>



# Molecular-Biology-of-the-Cell Ontology (MBCO)
<b>The following instructions describe how to use the C# script that was uploaded in 2017. They have no relevance for the windows application. </b><br>
<br>
<b>Before the first use</b><br>
Open the MBCO_standard_dynamic_enrichment.sln with a suited editor such as Visual Studio.
Select Solution 'MBCO_standard_dynamic_enrichment'. In the window properties set the path to the path that contains the downloaded files.
In the Solution Explorer open the folder Common_functions and the file "Global_definitions.sc".
Go to the class Global_directory_and_file_class
Specify the const mbco_major_directory (including hard drive letter). This is the directory from with all files will be loaded and all results will be safed at.
Run the function create all directories.
Copy paste the MBCO files "Supplementary Table S1B.txt","Supplementary Table S32 - gene-SCP associations" and "Supplementary Table S35 - inferred SCP relationships" into the directory MBCO_datasets.

<b>Specify data for enrichment analysis</b><br>
The script will generate a data instance ("data") that contains experimental data that will be subjected to the enrichment analysis instance ("mbco_enrichment_pipeline"). This data instance can either be filled with data from three published example studies (see Hansen et al. 2017) or with custom data. The latter can either be added via copy paste or loaded from a file that needs to be in the "Custom_data_sets" folder.<br>
<br>
<i>Analyze example data</i><br>
   Uncomment the commands within the regions Example study 1, Example study 2 or Example study 3 and comment all other commands within
   the other regions and the regions labeled with Custom data.<br>
   <br>
<i>Analyze own data - copy paste the gene list into the source code</i><br>
   Go to the function "Get_custom_data_by_copy_paste". Copy paste the gene list into the quotation marks after
   ncbi_official_gene_symbol (one gene per row as indicated). Uncomment the command line within the region Custom data - reading custom
   data spreadsheet. Comment all other regions that assign data to the data instance (Example study 1, Example study 2 and Example study
   3, Custom data - reading custom data spreadsheet).<br>
   <br>
   <i>Analyze own data - read a spreadsheet that contains the gene list</i><br>
   Go to the function "Get_custom_data_by_reading_custom_data_file". Specify the file name (including the file extension) that
   contains the data (custom_data_spreadsheet_name = "Custom_data_file_name.txt"). The tab-delimited file needs to be in the directory
   "Custom_data_sets" and needs to contain at least three columns that are specified under "custom_data_readWriteOptions.Key_columnNames", i.e. "SampleName", "NCBI_official_symbol", "Value". Additional columns will be ignored. All genes that belong to one sampleName will be analyzed as one group. Values will be used to distinguish between up-regulated (positive value) and down-regulated (negative value) genes. Any genes with a value of 0 will be removed. Any duplicates of the same gene/sampleName combination  will cause the throwing of an exception.<br>
   Uncomment the command within region "Custom data - reading custom data spreadsheet" in the Main function and comment the commands within all other regions.<br>
  
<b>Optional: Modify enrichment options</b><br>
   After construction of the mbco_enrichment_pipeline instance, several options can be specified in the corresponding options instance. This needs to be done before the function Generate of the instance is called.<br>
   <br>
   <i>General options (for standard and dynamic enrichment analysis)</i><br>
   Data_value_signs_of_interest:<br>
   Combined: All symbols of each sampleName that have a non zero value will be analyzed as one group.<br>
   Upregulated: All symbols of each sampleName that have a positive value will be analyzed as one group.<br>
   Downregulated: All symbols of each sampleName that have a negative value will be analyzed as one group.<br>
   <br>
   Maximum_pvalue_for_standardDynamicEnrichment:<br>
   All predicted SCPs or SCP-unions with a p-value above the indicated value will be removed. Default is 1.<br>
   <br>                                                 
   <i>Options for standard enrichment analysis only</i><br>
   Kept_top_predictions_standardEnrichment_per_level:<br>
   All SCPs that were predicted based on standard enrichment analysis will be ranked                                                       for each level. The indicated top predictions will finally be kept. Default is: Level 1: 5, Level2: 5, Level 3: 10, Level 4: 5.<br>
   <br>
   <i>Options for dynamic enrichment analysis only</i><br>
    Consider_interactions_between_signalingSCPs_for_dyanmicEnrichment: Specifies, if SCP-SCP interactions between 2 signaling SCPs should be considered. Default is false.<br>
    <br>
   Kept_top_predictions_dynamicEnrichment_per_level:<br>
All level-3 SCPs or SCP-unions that were predicted based on dynamic enrichment analysis will be ranked by significance. The indicated top predictions will be kept. Default is 5.<br>
<br>
   Kept_singleSCPs_dynamicEnrichment_per_level:<br>
   All level-3 SCPs or SCP-unions that were predicted based on dynamic enrichment analysis will be ranked by significance. All top predictions will be kept until the total number of SCPs that were predicted as single SCPs or as part of SCP-unions exceeds the given number. Default is: 99.<br>
   <br>
   Numbers_of_merged_scps_for_dynamicEnrichment_per_level:<br>
   The dynamic enrichment algorithm generates new SCP-unions by merging SCPs that contain at least one experimental gene and are related to each other as indicated in supplementary table S35. The values in this array specify the exact numbers of SCPs that will be merged with each other to generate new SCP-unions for the dynamic enrichment analysis. Default is 2 and 3.<br>
   <br>
   Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level:<br>
   The 2507 infered SCP-SCP interactions (supplementary table S35) will be ranked by the strength of the interaction. This value specifies which fraction of all SCP-SCP interactions will be considered to generate new SCP unions, starting with those SCP-SCP interactions with the strongest interaction. Default top 0.25.<br>
<br>
<b>Optional: Specify experimental background genes</b><br>
   Experimental background genes are those genes that do have a chance to be identified via the experimental methods (e.g. only genes
   that are annotated to a reference genome can be identified via RNASeq). These genes can be specified within the array
   bg_genes. The final background genes is the intersection between the MBCO background genes (i.e. all genes that our textmining algorithm identified in at least one
   abstract during the population of the ontology) and the experimental background genes. If the experimental background genes array has
   the length 0, the script will set the MBCO background genes as the final background
   genes. All MBCO genes and all experimental genes that are not part of the final background genes will be removed before Fisher's Exact test.
   <br>               
<b>Run the script</b><br>
   The script will generate a study specific results directory within the Results directory. The name of the directory will start with
   the name specified in base_file_name.<br>
<br>
<b>Analyze the results</b><br>
   The graphml networks can be opend with the network visualization software yED. The file Legend_for_networks contains general
   information about the networks.<br>
   The R-script "Generate_barplots_for_enrichment_results" will generate bar diagrams for the results of the standard and dynamic
   enrichment analysis. The mbco_base_directory has to contain the path of the results directory. Specific_study_name contains the name
   that is given by base_file_name in the C# script. This R-script will write two PDF files into the study specific results.
   
