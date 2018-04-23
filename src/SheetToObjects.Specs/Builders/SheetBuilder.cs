using System;
using System.Collections.Generic;
using SheetToObjects.Lib;

namespace SheetToObjects.Specs.Builders
{
    public class SheetBuilder
    {
        private readonly List<Row> _rows = new List<Row>();
        private readonly List<Func<RowBuilder, Row>> _rowBuilderFuncs = new List<Func<RowBuilder, Row>>();

        public SheetBuilder AddRow(Row row)
        {
            _rows.Add(row);
            return this;
        }

        public SheetBuilder AddRow(Func<RowBuilder, Row> rowBuilderFuncs)
        {
            _rowBuilderFuncs.Add(rowBuilderFuncs);
            return this;
        }

        public Sheet Build()
        {
            foreach (var rowBuilderFunc in _rowBuilderFuncs)
            {
                var row = rowBuilderFunc(new RowBuilder());
                _rows.Add(row);
            }

            return new Sheet(_rows);
        }
    }
}
