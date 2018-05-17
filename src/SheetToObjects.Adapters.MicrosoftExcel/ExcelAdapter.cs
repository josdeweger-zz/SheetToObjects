using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    internal class ExcelAdapter : IConvertResponseToSheet<ExcelData>
    {
        public Sheet Convert(ExcelData excelData)
        {
            if(excelData.IsNull())
                throw new ArgumentException(nameof(excelData));

            if (!excelData.Values.Any())
                return new Sheet(new List<Row>());

            var cells = excelData.Values.ToRows();

            return new Sheet(cells);
        }


    }
}
