using System;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    public class ExcelRange
    {
        public ExcelCell From { get; }
        public ExcelCell To { get; }

        public ExcelRange(ExcelCell @from, ExcelCell to)
        {
            if (@from.ColumnNumber > to.ColumnNumber)
                throw new ArgumentException("From column needs to be smaller or equal to To column");

            if (@from.RowNumber > to.RowNumber)
                throw new ArgumentException("From row needs to be smaller or equal to To row");

            From = @from;
            To = to;
        }
    }
}