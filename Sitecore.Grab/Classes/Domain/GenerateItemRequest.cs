using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Grab.Classes.Domain
{
    public class GenerateItemRequest
    {
        public string ItemId { get; set; }
        public string ItemPath { get; set; }
        public string Database { get; set; }
        public string GenerateSubItems { get; set; }
    }
}
