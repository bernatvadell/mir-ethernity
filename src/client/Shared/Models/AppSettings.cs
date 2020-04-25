using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Models
{
    public class ContentSettings
    {
        public string Path { get; set; }
    }

    public class AppSettings
    {
        public ContentSettings Content { get; set; }
    }
}
