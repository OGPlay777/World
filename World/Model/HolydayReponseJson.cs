using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Model
{
    public class HolydayReponseJson
    {
        public List<Holyday>? AllHolydaysList { get; set; }
        public bool ResponseStatus { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
