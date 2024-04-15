using CsvHelper;
using CsvHelper.Configuration;

namespace EegToSpss.Formats.CsvHelper.Csv
{
    internal class CsvEegReader(CsvConfiguration csvConfiguration) : CsvHelperEegReader
    {
        private readonly CsvConfiguration csvConfiguration = csvConfiguration;

        protected override CsvReader GetCsvHelperReader(StreamReader reader)
        {
            return new CsvReader(reader, csvConfiguration);
        }
    }
}
