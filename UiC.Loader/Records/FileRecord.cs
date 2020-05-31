using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Loader.Records
{
    public class FileRecord
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Hash")]
        public string Hash { get; set; }
    }
}
