using System;
using System.Collections.Generic;
using SheetToObjects.Lib;

namespace SheetToObjects.Specs.Builders
{
    public class RowBuilder
    {
        private readonly List<Cell> _cells = new List<Cell>();
        private readonly List<Func<CellBuilder, Cell>> _cellBuilderFuncs = new List<Func<CellBuilder, Cell>>();

        public RowBuilder AddCell(Func<CellBuilder, Cell> cellBuilderFunc)
        {
            _cellBuilderFuncs.Add(cellBuilderFunc);
            return this;
        }

        public Row Build(int index)
        {
            foreach (var cellBuilderFunc in _cellBuilderFuncs)
            {
                var cell = cellBuilderFunc(new CellBuilder());
                _cells.Add(cell);
            }

            return new Row(_cells, index);
        }
    }
}

