using CsvHelper;
using CsvHelper.Excel;
using System.Globalization;

namespace EegToSpss.Formats.CsvHelper.Excel
{
    internal class ExcelSpssWriter : CsvHelperSpssWriter
    {
        protected override CsvWriter GetCsvHelperWriter(StreamWriter writer)
        {
            // Must leave open for CLI to be able to close the stream later
            return new ExcelWriter(writer.BaseStream, "SPSS", CultureInfo.InvariantCulture, true);
        }
    }
}
