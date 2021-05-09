using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesPOJO
{
    public class Reporte: BaseEntity
    {
        public int Id { get; set; }

        public string Usuario { get; set; }
        
        public DateTime Fecha { get; set; }

        public string Frase_sin_traducir { get; set; }

        public string Traduccion { get; set; }
    }
}
