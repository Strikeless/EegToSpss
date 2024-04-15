using EegToSpss.Formats;
using CsvHelper.Configuration;
using System.Globalization;
using EegToSpss.Formats.CsvHelper.Csv;
using EegToSpss.Formats.CsvHelper.Excel;

namespace EegToSpss
{
    internal class CLI
    {
        private ProgramArgs Args { get; }

        private CsvConfiguration CsvConfig { get; }

        public CLI(ProgramArgs args)
        {
            Args = args;

            CsvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = Args.CsvDelimiter
            };
        }

        public void Run()
        {
            var data = new GlobalData();
            string? fieldPrefix = null;

            if (!GetSpssWriterByPath(Args.Output, out var spssWriter) || spssWriter == null)
            {
                Console.Error.WriteLine($"Output '{Args.Output}' has an unrecognized extension. Supported extensions are csv, xlsx and xlsm.");
                return;
            }

            if (!Args.Paths.Any())
            {
                Console.Error.WriteLine("No paths to EEG content were given.");
                return;
            }

            foreach (string path in Args.Paths)
            {
                if (!GetFilesAtPath(path, out string[] filePaths))
                {
                    Console.Error.WriteLine($"File or directory '{path}' was not found or was not accessible.");
                    return;
                }

                foreach (var filePath in filePaths)
                {
                    var fileFieldPrefix = GetFieldPrefixByPath(filePath);
                    if (fieldPrefix == null)
                    {
                        fieldPrefix = fileFieldPrefix;
                    } else if (fileFieldPrefix != fieldPrefix)
                    {
                        Console.Error.WriteLine(
                            $"WARNING: Combining files with different field prefixes ({fieldPrefix} and {fileFieldPrefix}). "
                            + $"Only {fieldPrefix} will be used."
                        );
                    }

                    if (!GetEegReaderByPath(filePath, out var eegReader) || eegReader == null)
                    {
                        Console.Error.WriteLine($"File '{filePath}' has an unrecognized extension. Supported extensions are csv, xlsx and xlsm.");
                        return;
                    }

                    StreamReader? fileReader = null;
                    try
                    {
                        fileReader = new StreamReader(filePath);
                        eegReader.ReadEeg(ref data, fileReader, GetSubjectIdByPath(filePath));
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine($"Failed to read EEG from '{filePath}': {e.Message}");
                        return;
                    }
                    finally
                    {
                        fileReader?.Close();
                    }
                }
            }

            if (fieldPrefix == null)
            {
                Console.Error.WriteLine("Field prefix wasn't determined.");
                return;
            }

            data.Timepoints.RemoveAll(timepoint => timepoint < Args.MinTime || timepoint > Args.MaxTime);

            StreamWriter? fileWriter = null;
            try
            {
                fileWriter = new StreamWriter(Args.Output);
                spssWriter.WriteSpss(data, fileWriter, fieldPrefix);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to write SPSS to '{Args.Output}': {e.Message}");
                return;
            }
            finally
            {
                fileWriter?.Close();
            }
        }

        private bool GetFilesAtPath(string path, out string[] filePaths)
        {
            if (Directory.Exists(path))
            {
                filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                return true;
            }
            else if (File.Exists(path))
            {
                filePaths = [path];
                return true;
            }

            filePaths = [];
            return false;
        }

        private bool GetEegReaderByPath(string path, out IEegReader? reader)
        {
            string fileExtension = Path.GetExtension(path);
            switch (fileExtension.ToLowerInvariant())
            {
                case ".csv":
                    {
                        reader = new CsvEegReader(CsvConfig);
                        return true;
                    }
                case ".xlsx" or ".xslm":
                    {
                        reader = new ExcelEegReader(CsvConfig);
                        return true;
                    }
                default:
                    {
                        reader = null;
                        return false;
                    }
            }
        }

        private bool GetSpssWriterByPath(string path, out ISpssWriter? writer)
        {
            string fileExtension = Path.GetExtension(path);
            switch (fileExtension.ToLowerInvariant())
            {
                case ".csv":
                    {
                        writer = new CsvSpssWriter(CsvConfig);
                        return true;
                    }
                case ".xlsx" or ".xslm":
                    {
                        writer = new ExcelSpssWriter();
                        return true;
                    }
                default:
                    {
                        writer = null;
                        return false;
                    }
            }
        }

        private static string GetSubjectIdByPath(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            if (fileName.Contains('_'))
            {
                return fileName.Split('_').Last().Replace("ID", "");
            } else
            {
                return fileName;
            }
        }

        private static string GetFieldPrefixByPath(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var splitFileName = fileName.Split('_');

            if (splitFileName.Length >= 3)
            {
                return $"Eeg{splitFileName[2]}";
            } else
            {
                return "Eeg";
            }
        }
    }
}
