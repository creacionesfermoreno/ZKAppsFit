using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTecoFingerPrintScanner_Implementation.Models;

namespace ZKTecoFingerPrintScanner_Implementation.Helpers
{
    public static class DataSession
    {
        public static string DKey { get; set; }
        public static string Filtre { get; set; }
        public static int Unidad { get; set; }
        public static int Sede { get; set; }
        public static string Name { get; set; }
        public static string Logo { get; set; }
        public static string Rubro { get; set; }
    }

    public static class DataStatic
    {
        public static Membresia MembresiasSelected { get; set; }
        public static List<Membresia> Membresias { get; set; }
        public static List<Asistence> Asistences { get; set; }
        public static List<Pago> Pagos { get; set; }
        public static List<Cuota> Cuotas { get; set; }
        public static SocioModel Socio { get; set; }
        public static List<Incidencia> Incidencias { get; set; }
    }


}

