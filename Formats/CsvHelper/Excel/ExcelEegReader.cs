using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Excel;

namespace EegToSpss.Formats.CsvHelper.Excel
{
    internal class ExcelEegReader(CsvConfiguration csvConfiguration) : CsvHelperEegReader
    {
        private readonly CsvConfiguration csvConfiguration = csvConfiguration;

        protected override CsvReader GetCsvHelperReader(StreamReader reader)
        {
            return new CsvReader(new ExcelParser(reader.BaseStream, "", csvConfiguration));
        }
    }
}
