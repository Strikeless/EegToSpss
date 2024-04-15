using CommandLine;

namespace EegToSpss
{
    internal class ProgramArgs
    {
        [Option('i', "min-time", Required = false, Default = 0, HelpText = "Minimum time to be included in the output")]
        public int? MinTime { get; set; }

        [Option('a', "max-time", Required = false, HelpText = "Maximum time to be included in the output")]
        public int? MaxTime { get; set; }

        [Option("csv-delimiter", Required = false, Default = ";", HelpText = "The field delimiter to be used for CSV files")]
        public required string CsvDelimiter { get; set; }

        [Option('o', "output", Required = true, HelpText = "A filepath to where the output SPSS-file should be written to")]
        public required string Output { get; set; }

        [Value(0, MetaName = "paths", Required = true, HelpText = "All paths to files or directories to be compiled to the output")]
        public required IEnumerable<string> Paths { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ProgramArgs>(args)
                .WithParsed(parsedArgs => new CLI(parsedArgs).Run())
                .WithNotParsed(_errors => PrintUsage());
        }

        static void PrintUsage()
        {
            Console.WriteLine(
                "Usage examples:\n"
                + "Simply take EEG data from Evoked_aud_amp_ID1.xlsx and Evoked_aud_amp_ID2.csv, and\n"
                + "write the output SPSS data to SPSS_output.xlsx:\n"
                + "    EegToSpss -o SPSS_output.xlsx Evoked_aud_Amp_ID1.xlsx Evoked_aud_Amp_ID2.csv\n"
                + "\n"
                + "Same as above, but only take data from a timespan of 500-600ms\n"
                + "(A minimum time of 0ms is the default and does not need to be specified):\n"
                + "    EegToSpss -o SPSS_output.xlsx --min-time 500 --max-time 600 Evoked_aud_Amp_ID1.xlsx Evoked_aud_Amp_ID2.csv\n"
            );
        }
    }
}
