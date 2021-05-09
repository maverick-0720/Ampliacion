using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesPOJO
{
    public class Traductor : BaseEntity
    {
        public int Id { get; set; }

        public int Cod_Idioma { get; set; }

        public string PO { get; set; }

        public string PT { get; set; }

        public int Popularidad { get; set; }
    }
}
