using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Lib;

namespace SheetToObjects.Specs.Builders
{
    public class SheetBuilder
    {
        private string[] _headers = new string[0];
        private readonly List<Row> _rows = new List<Row>();
        private readonly List<Func<RowBuilder, Row>> _rowBuilderFuncs = new List<Func<RowBuilder, Row>>();

        public SheetBuilder AddRow(Func<RowBuilder, Row> rowBuilderFuncs)
        {
            _rowBuilderFuncs.Add(rowBuilderFuncs);
            return this;
        }
        
        public SheetBuilder AddHeaders(params string[] headers)
        {
            _headers = headers;
            return this;
        }

        public Sheet Build()
        {
            if (_headers.Any())
            {
                var headerCells = _headers
                    .Select((headerValue, columnIndex) => new Cell(columnIndex, 0, headerValue))
                    .ToList();

                _rows.Add(new Row(headerCells,0));
            }

            foreach (var rowBuilderFunc in _rowBuilderFuncs)
            {
                var row = rowBuilderFunc(new RowBuilder());
                _rows.Add(row);
            }

            return new Sheet(_rows);
        }
    }
}
