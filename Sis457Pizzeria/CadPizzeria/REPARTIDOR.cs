//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CadPizzeria
{
    using System;
    using System.Collections.Generic;
    
    public partial class REPARTIDOR
    {
        public int usuario_id { get; set; }
        public string nro_licencia { get; set; }
        public System.DateTime fecha_ingreso { get; set; }
        public bool estado_registro { get; set; }
    
        public virtual USUARIO USUARIO { get; set; }
    }
}
