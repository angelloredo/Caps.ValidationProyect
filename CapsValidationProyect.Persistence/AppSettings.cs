using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapsValidationProyect.Persistence
{
    public class AppSettings
    {
        public bool UseCustomizationData { get; set; }
        public string? ConnectionString { get; set; }
    }
}
