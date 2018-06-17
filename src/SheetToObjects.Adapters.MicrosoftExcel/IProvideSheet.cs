﻿using System.IO;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    public interface IProvideSheet
    {
        Sheet GetFromPath(string excelPath, string sheetName, ExcelRange range);
        Sheet GetFromStream(Stream fileStream, string sheetName, ExcelRange range);
    }
}
