using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKTecoFingerPrintScanner_Implementation.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Huella { get; set; }
    }
    public class UserModel
    {
        public string Name { get; set; }
        public string Huella { get; set; }
        
    }
    public class SocioModel
    {
        public int CodigoUnidadNegocio { get; set; }
        public int CodigoSede { get; set; }
        public int CodigoSocio { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Huella { get; set; }
        public string ImagenUrl { get; set; }
        public string MessageExtra { get; set; }

    }

    public class Membresia
    {
        public int Id { get; set; }
        public string AsesorComercial { get; set; }
        public string Descripcion { get; set; }
        public string desTipoPaquete { get; set; }
        public int CodigoPaquete { get; set; }
        public string NombrePaquete { get; set; }
        public string DesFechaInicio { get; set; }
        public string DesFechaFin { get; set; }
        public string FCrecionText { get; set; }
        public decimal Costo { get; set; }
        public decimal Pago { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal Debe { get; set; }
        public int NroIngreso { get; set; }
        public int CantidadFreezing { get; set; }
        public int CantidadFreezingTomados { get; set; }
        public int CantidadAsistencia { get; set; }
        public string NroContrato { get; set; }
        public int NroIngresoActual { get; set; }
        public int CodigoSede { get; set; }
        public int CodigoMenbresia { get; set; }
        public string colorEstado { get; set; }
        public int Estado { get; set; }
        public string ObtenerTiempoVencimiento { get; set; }
        public string ObtenerEstadoCitaNutrional { get; set; }
    }

    public class Asistence
    {
        public int CodigoAsistencia { get; set; }
        public string DiaSemana { get; set; }
        public string FCreacionText { get; set; }
        public string HourText { get; set; }
        public string UsuarioCreacion { get; set; }

    }

    public class Cuota
    {
        public decimal Monto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime Fecha { get; set; }
        

    }

    public class Incidencia
    {
        public string Ocurrencia { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }


    }

    public class Pago {
        public string NroComprobante { get; set; }
        public int Estado { get; set; }
        public decimal Monto { get; set; }
        public string DesFormaPago { get; set; }
        public string UsuarioCreacion { get; set; }
        public string desFechaPago { get; set; }
    }


    public static class SocioData
    {
        public static List<SocioModel> socios { get; private set; }
        public static void SetListaUsers(List<SocioModel> sociosL)
        {
            socios = sociosL;
        }
        public static void AddUser(SocioModel user)
        {
            socios.Add(user);
        }
    }

   

}
