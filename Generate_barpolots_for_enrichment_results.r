rm(list = ls());
library(tools);

mbco_base_directory = "E://MBCO_enrichment//Results//";
specific_study_name = "erlotinib"
specific_study_name = "Secretory_pathway"
specific_study_name = "d508CFTR_interactome"
specific_study_name = "Custom dataset"
specific_study_directory = paste(specific_study_name,"_Molecular_biology_cell//",sep='');
complete_directory = paste(mbco_base_directory,specific_study_directory,sep='');
standard_enrichment_fileName = "Standard_enrichment_results_filtered";
dynamic_enrichment_fileName = "Dynamic_enrichment_results_filtered";
enrichment_file_extension = ".txt";
pdf_file_extension = ".pdf";

#################################################################################################

complete_standard_enrichment_file = paste(complete_directory,standard_enrichment_fileName,enrichment_file_extension,sep='');
complete_dynamic_enrichment_file = paste(complete_directory,dynamic_enrichment_fileName,enrichment_file_extension,sep='');
complete_standard_pdf_file = paste(complete_directory,standard_enrichment_fileName,pdf_file_extension,sep='');
complete_dynamic_pdf_file = paste(complete_directory,dynamic_enrichment_fileName,pdf_file_extension,sep='');

#################################################################################################

Standard = read.table(file=complete_standard_enrichment_file,header=TRUE,stringsAsFactors=FALSE,quote="",sep='\t');
Standard$Complete_sampleName = paste(Standard$EntryType,Standard$Timepoint,Standard$Sample_name,sep=' ');
Standard$Timepoint = as.numeric(Standard$Timepoint)

Standard = Standard[order(Standard$Timepoint),]

complete_sampleNames = unique(Standard$Complete_sampleName);
sampleNames_length = length(complete_sampleNames);

pdf(file = complete_standard_pdf_file);

for (indexS in 1:sampleNames_length)
{#Begin
    complete_sampleName = complete_sampleNames[indexS];
    indexCurrent = which(Standard$Complete_sampleName==complete_sampleName);
    current_standard = Standard[indexCurrent,];
    current_standard = current_standard[order(current_standard$Minus_log10_pvalue,decreasing=FALSE),]    
        
    if (length(current_standard[,1]))
    {#Begin
      process_levels = unique(current_standard$ProcessLevel);
      process_levels = process_levels[order(process_levels)]
      process_levels_length = length(process_levels);
      
      empty_line = current_standard[1,];
      empty_line$ProcessLevel = -1;
      empty_line$Scp_name = "";
      empty_line$Minus_log10_pvalue = 0;
      
      Sorted_data = c();
      Colors = c();
      Border_colors = c();

      for (indexLevel in length(process_levels):1)
      {#Begin
          process_level = process_levels[indexLevel];
          if (indexLevel!=length(process_levels))
          {#Begin
              Sorted_data = rbind(Sorted_data,empty_line);
              Colors = c(Colors,"white");
              Border_colors = c(Border_colors,"white");
          }#End
          Add_data = current_standard[current_standard$ProcessLevel==process_level,];
          Sorted_data =  rbind(Sorted_data,Add_data);
          if (process_level==1) { Level_color = "darkred"; }
          if (process_level==2) { Level_color = "firebrick1"; }
          if (process_level==3) { Level_color = "dodgerblue"; }
          if (process_level==4) { Level_color = "limegreen"; }
          Colors = c(Colors,replicate(length(Add_data[,1]),Level_color));
          Border_colors = c(Border_colors,replicate(length(Add_data[,1]),Level_color));
      }#End
      
      par( mar=c(5, 27, 1, 1));
      max_xvalue = ceiling(max(Sorted_data$Minus_log10_pvalue));
      if (max_xvalue<5)
      {#Begin
         max_xvalue = ceiling(max(Sorted_data$Minus_log10_pvalue) * 2) / 2;
      }#End
       
      Title = complete_sampleName; 
      Cex_names = 0.9;
      Cex_main = 0.6;
      barplot(Sorted_data$Minus_log10_pvalue, xlim = c(0,max_xvalue), border = Border_colors, col=Colors, names.arg=Sorted_data$Scp_name,horiz=TRUE,las=1,xlab="-log10(p)", main=Title, cex.names = Cex_names,cex.main=Cex_main)
    }#End
}#End

dev.off();

############################################################################################################################################

Dynamic = read.table(file=complete_dynamic_enrichment_file,header=TRUE,stringsAsFactors=FALSE,quote="",sep='\t');
Dynamic$Timepoint = as.numeric(Dynamic$Timepoint)
Dynamic$Complete_sampleName = paste(Dynamic$EntryType,Dynamic$Timepoint,Dynamic$Sample_name,sep=' ');
Dynamic$Scp_name = gsub("\\$","\n",Dynamic$Scp_name);

Dynamic = Dynamic[order(Dynamic$Timepoint),]

complete_sampleNames = unique(Dynamic$Complete_sampleName);
sampleNames_length = length(complete_sampleNames);

pdf(file = complete_dynamic_pdf_file);

for (indexS in 1:sampleNames_length)
{#Begin
    complete_sampleName = complete_sampleNames[indexS];
    indexCurrent = which(Dynamic$Complete_sampleName==complete_sampleName);
    current_dynamic = Dynamic[indexCurrent,];
    current_dynamic = current_dynamic[order(current_dynamic$Minus_log10_pvalue,decreasing=FALSE),]    
        
    if (length(current_standard[,1]))
    {#Begin
      process_levels = unique(current_dynamic$ProcessLevel);
      process_levels = process_levels[order(process_levels)]
      process_levels_length = length(process_levels);
      
      empty_line = current_dynamic[1,];
      empty_line$ProcessLevel = -1;
      empty_line$Scp_name = "";
      empty_line$Minus_log10_pvalue = 0;
      
      Sorted_data = c();
      Colors = c();
      Border_colors = c();

      for (indexLevel in length(process_levels):1)
      {#Begin
          process_level = process_levels[indexLevel];
          if (indexLevel!=length(process_levels))
          {#Begin
              Sorted_data = rbind(Sorted_data,empty_line);
              Colors = c(Colors,"white");
              Border_colors = c(Border_colors,"white");
          }#End
          Add_data = current_dynamic[current_dynamic$ProcessLevel==process_level,];
          Sorted_data =  rbind(Sorted_data,Add_data);
          if (process_level==1) { Level_color = "darkred"; }
          if (process_level==2) { Level_color = "firebrick1"; }
          if (process_level==3) { Level_color = "dodgerblue"; }
          if (process_level==4) { Level_color = "limegreen"; }
          Colors = c(Colors,replicate(length(Add_data[,1]),Level_color));
          Border_colors = c(Border_colors,replicate(length(Add_data[,1]),Level_color));
      }#End
      
      par( mar=c(5, 27, 1, 1));
      max_xvalue = ceiling(max(Sorted_data$Minus_log10_pvalue));
      if (max_xvalue<5)
      {#Begin
         max_xvalue = ceiling(max(Sorted_data$Minus_log10_pvalue) * 2) / 2;
      }#End
       
      Title = complete_sampleName; 
      Cex_names = 0.9;
      Cex_main = 0.6;
      barplot(Sorted_data$Minus_log10_pvalue, xlim = c(0,max_xvalue), border = Border_colors, col=Colors, names.arg=Sorted_data$Scp_name,horiz=TRUE,las=1,xlab="-log10(p)", main=Title, cex.names = Cex_names,cex.main=Cex_main)
    }#End
}#End

dev.off();



