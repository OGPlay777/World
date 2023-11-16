using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Model
{
    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public Country(string name, string code) 
        {
            this.Name = name;
            this.Code = code;
        }
    }
}
