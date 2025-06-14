﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnPizzeria
{
    public class PlatilloDto
    {
        public int platillo_id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int tiempo_preparacion { get; set; }
        public string imagen_url { get; set; }
        public string categoria { get; set; }
        public bool estado { get; set; }
    }
}
