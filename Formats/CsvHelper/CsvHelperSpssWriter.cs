using CsvHelper;

namespace EegToSpss.Formats.CsvHelper
{
    internal abstract class CsvHelperSpssWriter : ISpssWriter
    {
        protected abstract CsvWriter GetCsvHelperWriter(StreamWriter writer);

        public void WriteSpss(GlobalData data, StreamWriter writer, string fieldPrefix)
        {
            using var csv = GetCsvHelperWriter(writer);

            // Write the CSV header.
            csv.WriteField("ID");
            foreach (var timepoint in data.Timepoints)
            {
                foreach (var electrodeName in data.ElectrodeNames)
                {
                    csv.WriteField($"{fieldPrefix}{timepoint}{electrodeName}");
                }
            }
            csv.NextRecord();

            // Write subject data in the same order as in the header.
            // Scary time complexity but guarantees correct order of electrode readings.
            foreach (var subjectData in data.SubjectDatas)
            {
                csv.WriteField(subjectData.Name);

                foreach (var timepoint in data.Timepoints)
                {
                    foreach (var electrodeName in data.ElectrodeNames)
                    {
                        // NOTE: Datapoint is a record, so comparison in ElectrodeReadings will match.
                        var datapoint = new Datapoint(timepoint, electrodeName);

                        // Get and write the subject's electrode reading for the datapoint,
                        // or default to "" if the subject has no reading for that datapoint.
                        if (subjectData.ElectrodeReadings.TryGetValue(datapoint, out var reading))
                        {
                            csv.WriteField(reading);
                        } else
                        {
                            csv.WriteField("");
                        }
                    }
                }

                csv.NextRecord();
            }
        }
    }
}
