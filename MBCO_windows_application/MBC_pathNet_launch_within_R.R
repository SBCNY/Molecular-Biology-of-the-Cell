#The following R script launches MBC PathNet and analyzes the data saved in the degs_directory.
#After launch the application performs the Clear, read & analyze function, and then
#exits upon successful completion. The application window will remain invisible or may
#briefly appear before hiding again. If any errors are encountered during execution,
#the window will remain visible to allow error inspection.

#If you run the application in a Linux environment, ensure that you have access permissions 
#to the folder of the application.

#Ensure that you have downloaded all necessary third-party files, e.g. the gene-pathway annotations
#and parent child relationships files of the selected ontologies. For download instructions and 
#command line code, see 'Download_all_datasets_windows.txt' or 'Download_all_datasets_linux.txt'.
#Upon first use, the application will prepare those datasets (e.g., populate GO parent terms with
#the genes of their children) and save the results for faster access the next time.
#
#The degs_directory can contain multiple data files, e.g.:
#SingleCell_marker_genes_analysis1.txt
#SingleCell_marker_genes_analysis2.txt
#SingleCell_marker_genes_analysis3.txt
#SingleCell_marker_genes_analysis4.txt
#For separate integration and results visualization of each analysis, add a column
#'Integration_group' to each dataset and add integration group names, e.g. 'Analysis 1',
#'Analysis 2', '...'. The column name 'Integration_group' needs to be specified in the
#R data.frame custom_1_columnNames below (as it is in this script).
#To ensure the application uses the same colors as those generated in other R figures,
#use the col2hex command from the gplots package to convert R colors into hexadecimal color codes.
#Then define a new column (e.g., 'Dataset_color') and add the same hexadecimal color code to all rows with
#the same dataset (e.g. cluster) and directionality of change (i.e. positive or negative avg_log2FC).
#The application will read the colors in hexadecimal code and map them to the closest C#
#color. The selected column name (e.g., 'Color') needs to be specified in the R data.frame
#custom_1_columnNames (as it is in this script).
#
#The degs_directory can contain multiple lists of background genes (1 gene each row, no headline)
#that will be automatically mapped to the related data files with the same name,
#except the '_bgGenes'-ending, e.g.:
#SingleCell_marker_genes_analysis1_bgGenes.txt
#SingleCell_marker_genes_analysis2_bgGenes.txt
#SingleCell_marker_genes_analysis3_bgGenes.txt
#SingleCell_marker_genes_analysis4_bgGenes.txt
#
#The R script below defines and writes the Custom_1 column names into the degs_directory for
#automatic import into the application. The defined column names have to match the
#column names in all imported data files (e.g. 'SingleCell_marker_genes_analysis1.txt').
#
#The R-script will instruct MBC PathNet to read the data in the specified degs_directory,
#subject it to enrichment analysis with the selected ontologies specified in the array ontologies
#and write the results into the specified pathway_directory. 
#Before each analysis the R-script will read the 'MBC_pathNet_parameter_settings.txt'-file from the
#specified mbc_pathNet_directory. It will set the parameter 'Customized_colors' to 'True'
#to allow visualization of results in the same colors that are used for figures generated
#in R (e.g. within UMAPs). The script will write the updated parameter settings file into the degs_directory
#from where it will be imported into MBC PathNet.

#Preparation of 'MBC_pathNet_parameter_settings.txt':
#The main MBCO_windows_appliation subdirectory contains an example parameter settings file.
#To geneate a new file, launch MBC PathNet in regular mode and add one gene into the
#'Gene list'-text box #(e.g., 'AAK1'). Press the 'Add dataset'-button. If needed, change
#the parameter settings in the menus 'Enrichment' and 'SCP networks' and -for more detailed settings-
#in the menus 'Set data cutoffs', 'Select SCPs' and 'Define new SCPs'. Press 'Analyze'.
#Copy the 'MBC_pathNet_parameter_settings.txt'-file from the 'Input_data'-directory
#within the specified results directory ('Save results in') into the mbc_pathNet_directory.
#
#Windows vs Linux
#Indicate if you are running the code in a Windows or Linux environment. When running the code in a
#Linux environment, you need the Mono package. See ReadMe in the main directory for download details.

mbc_pathNet_directory = "D:/MBCO_windows_application/" #ensure directory ending with '/'
degs_directory = "D:/MBCO_windows_application/Custom_data/Neurite_outgrowth/" #ensure directory ending with '/'
pathway_directory = "D:/MBCO_windows_application/Results/RTest/" #ensure directory ending with '/'
ontologies = c("MBCO")#,"GO_BP","GO_MF","GO_CC","Reactome","Custom_1","Custom_2" #case-insensitive
species = "human" #"human","mouse","rat","chicken","dog","pig","cow","zebrafish","frog","c_elegans","fly" #case-insensitive
is_windows = TRUE
is_linux = !is_windows

{#Begin - Calculate pathways using MBC PathNet
  current_working_directory = getwd()
  setwd(mbc_pathNet_directory)
  custom_1_columnNames <- data.frame(
    #Do not change, attributes have to match the attributes within MBC_pathNet
    `Attribute in the application` = c(
      "Dataset_name", #Index 1
      "Ncbi_official_gene_symbol", #Index 2
      "Time_point", #Index 3
      "Time_unit", #Index 4
      "Integration_group", #Index 5
      "Value_1st", #Index 6
      "Value_2nd", #Index 7
      "Dataset_color" ), #Index 8
    #Adapt to your data column names, each column name will be matched to the attribute at the same index,
    #indices with empty strings will be ignored
    `Column name in user data` = c(
      "Dataset_name", #Index 1 (e.g. "cluster", if single cell transcriptomics)
      "NCBI_official_gene_symbol", #Index 2 (e.g "gene")
      "Time point", #Index 3 ( e.g. "")
      "Time unit", #Index 4 (e.g. "")
      "Integration_group", #Index 5
      "Log2_fold_change", #Index 6 (e.g "avg_log2FC")
      "Minus_log10_pval_or_adj_pval", #Index 7 (e.g. "p_val_adj")
      "Dataset_color" )) #Index 8
  complete_custom_columnNames_fileName = paste(degs_directory,"Custom_1_columnNames_userData.txt",sep='')
  colnames(custom_1_columnNames) = gsub("[.]"," ",colnames(custom_1_columnNames))
  write.table(custom_1_columnNames,file=complete_custom_columnNames_fileName,quote=FALSE,row.names=FALSE,col.names = TRUE,sep='\t')
  
  complete_analysis_finished_fileName = paste(degs_directory,"Analysis_finished.txt",sep='')
  indexOntology=1
  for (indexOntology in 1:length(ontologies))
  {#Begin
    ontology = ontologies[indexOntology]
    print(paste("Analyzing the data in '",degs_directory,"' with MBC PathNet using ",ontology," and generating selected charts and networks.",sep=''))
    complete_input_parameterSettings_fileName = paste(mbc_pathNet_directory,"MBC_pathNet_parameter_settings.txt",sep='')
    complete_output_parameterSettings_fileName = paste(degs_directory,"MBC_pathNet_parameter_settings.txt",sep='')
    mbcPathNet_parameter_lines <- readLines(complete_input_parameterSettings_fileName)
    indexUseCustomizedColors = grep("Bardiagram_options_class\tCustomized_colors",mbcPathNet_parameter_lines)
    mbcPathNet_parameter_lines[indexUseCustomizedColors] = "Bardiagram_options_class\tCustomized_colors\tTrue"
    writeLines(mbcPathNet_parameter_lines,complete_output_parameterSettings_fileName)
    exe_path = file.path(mbc_pathNet_directory, "MBC_PathNet.exe")
    shared_arguments = paste0('" --input-dir ', '"', degs_directory, '"',' --results-dir ',pathway_directory, ' --custom-1-column-names', ' --ontology ',ontology,' --species ',species,sep='')
    if (is_windows) { cmd = paste0('"', exe_path, shared_arguments,sep='') }
    if (is_linux) { cmd = paste0('mono ',exe_path, shared_arguments,sep='') }
    system(cmd, wait=TRUE)
    if (file.exists(complete_analysis_finished_fileName))
    { print(paste("Data analysis with ",ontology," successful. Enrichment results saved in '",pathway_directory,"'",sep='')) }
    if (!file.exists(complete_analysis_finished_fileName))
    {#Begin
       print(paste("For loop interrupted, since MBC PathNet analysis was not finished successfully.",sep=''))
       break;
    }#End
  }#End
  setwd(current_working_directory)
}#End - Calculate pathways using MBC PathNet



