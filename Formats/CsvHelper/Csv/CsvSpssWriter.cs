using CsvHelper;
using CsvHelper.Configuration;

namespace EegToSpss.Formats.CsvHelper.Csv
{
    internal class CsvSpssWriter(CsvConfiguration csvConfiguration) : CsvHelperSpssWriter
    {
        private readonly CsvConfiguration csvConfiguration = csvConfiguration;

        protected override CsvWriter GetCsvHelperWriter(StreamWriter writer)
        {
            return new CsvWriter(writer, csvConfiguration);
        }
    }
}
