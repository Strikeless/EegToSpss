> [!NOTE]
> This project is not being actively maintained, and has a lack of documentation. I cannot say how generic this is and whether or not it'll work for whatever data you're processing    .

# EegToSpss
A program to combine any amount of EEG files to one SPSS file. Supports CSV and Excel XLSX and XLSM formats.

# Usage examples
## Simply take EEG data from Evoked_aud_amp_ID1.xlsx and Evoked_aud_amp_ID2.csv, and write the output SPSS data to SPSS_output.xlsx
```EegToSpss -o SPSS_output.xlsx Evoked_aud_Amp_ID1.xlsx Evoked_aud_Amp_ID2.csv```

## Same as above, but only take data from a timespan of 500-600ms (A minimum time of 0ms is the default and does not need to be specified)
```EegToSpss -o SPSS_output.xlsx --min-time 500 --max-time 600 Evoked_aud_Amp_ID1.xlsx Evoked_aud_Amp_ID2.csv```
