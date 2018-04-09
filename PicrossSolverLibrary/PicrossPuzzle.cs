using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PicrossSolverLibrary
{
    public class PicrossPuzzle
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("column-count")]
        public int ColumnCount { get; set; }

        [JsonProperty("row-count")]
        public int RowCount { get; set; }

        [JsonProperty("column-rules")]
        public IList<IList<int>> ColumnRules { get; set; }

        [JsonProperty("row-rules")]
        public IList<IList<int>> RowRules { get; set; }
    }
}
