using CsvHelper;

namespace EegToSpss.Formats.CsvHelper
{
    internal abstract class CsvHelperEegReader : IEegReader
    {
        protected abstract CsvReader GetCsvHelperReader(StreamReader reader);

        public void ReadEeg(ref GlobalData data, StreamReader reader, string subjectId)
        {
            using var csv = GetCsvHelperReader(reader);

            // Read the CSV header.
            csv.Read();
            csv.ReadHeader();

            var subjectData = new SubjectData(subjectId);

            // Read every row from the CSV.
            while (csv.Read())
            {
                // The first column should always be the point in time.
                var time = csv.GetField<int>(0);

                if (!data.Timepoints.Contains(time))
                {
                    data.Timepoints.Add(time);
                }

                // Loop over every column except for the already read time column.
#pragma warning disable 8604 // HeaderRecord may not be null if we got to this while loop.
                foreach (var sensorName in csv.HeaderRecord.Skip(1))
#pragma warning restore 8604
                {
                    if (!data.ElectrodeNames.Contains(sensorName))
                    {
                        data.ElectrodeNames.Add(sensorName);
                    }

                    // Read the value of the column in the current row.
                    // var electrodeReading = csv.GetField<float>(sensorName); // This comes up with crazy values
                    var electrodeReading = float.Parse(csv.GetField(sensorName));

                    // Add the reading to the subject's electrode readings.
                    var datapoint = new Datapoint(time, sensorName);
                    subjectData.ElectrodeReadings.Add(datapoint, electrodeReading);
                }
            }

            data.SubjectDatas.Add(subjectData);
        }
    }
}
