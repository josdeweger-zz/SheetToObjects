using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    public class SheetProvider : IProvideSheet
    {
        private readonly IConvertResponseToSheet<CsvData> _csvDataConverter;

        internal SheetProvider(IConvertResponseToSheet<CsvData> csvDataConverter)
        {
            _csvDataConverter = csvDataConverter;
        }

        public SheetProvider() : this(new CsvAdapter()) { }

        public Sheet GetFromBase64Encoded(string base64EncodedFile, char delimiter)
        {
            var fileBytes = Convert.FromBase64String(base64EncodedFile);
            var fileString = Encoding.UTF8.GetString(fileBytes);

            var data = fileString
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Split(delimiter).ToList())
                .ToList();

            var csvData = new CsvData { Values = data };

            return _csvDataConverter.Convert(csvData);
        }

        public Sheet GetFromPath(string csvPath, char delimiter)
        {
            var data = File.ReadAllLines(csvPath)
                .Select(line => line.Split(delimiter).ToList())
                .ToList();

            var csvData = new CsvData { Values = data };

            return _csvDataConverter.Convert(csvData);
        }

        public Sheet GetFromStream(Stream stream, char delimiter)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                while(!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var data = lines
                .Select(line => line.Split(delimiter).ToList())
                .ToList();

            var csvData = new CsvData { Values = data };

            return _csvDataConverter.Convert(csvData);
        }
    }
}
