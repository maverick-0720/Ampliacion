using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesPOJO
{
    public class Idioma : BaseEntity
    {
        public int Id { get; set; }

        public string IdiomaNuevo { get; set; }

        public int Popularidad { get; set; }
    }
}
