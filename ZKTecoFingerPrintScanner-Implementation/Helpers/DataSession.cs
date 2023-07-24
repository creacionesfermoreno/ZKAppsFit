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
        public static int Code { get; set; }
        public static int Unidad { get; set; }
        public static int Sede { get; set; }
        public static string Name { get; set; }
        public static string Logo { get; set; }
        public static string Rubro { get; set; }
    }

    public static class DataStatic
    {
        public static Membresia MembresiasSelected { get; set; }

        public static string MessageGeneric { get; set; }
        public static string MessageGenericD { get; set; }
        public static List<Membresia> Membresias { get; set; }
        public static List<Asistence> Asistences { get; set; }
        public static List<Pago> Pagos { get; set; }
        public static List<Cuota> Cuotas { get; set; }
        public static SocioModel Socio { get; set; }
        public static List<Incidencia> Incidencias { get; set; }
    }


    public sealed class DataSocioAll
    {
        private static readonly Lazy<DataSocioAll> lazyInstance = new Lazy<DataSocioAll>(() => new DataSocioAll());

        private DataSocioAll()
        {
            MembresiasSelected = new Membresia();
        }

        public static DataSocioAll Instance => lazyInstance.Value;

        public Membresia MembresiasSelected { get; set; }
        public string MessageGeneric { get; set; }
        public string MessageGenericD { get; set; }

        private List<Membresia> membresias = new List<Membresia>();
        public List<Membresia> Membresias
        {
            get => membresias;
            set => membresias = value ?? new List<Membresia>();
        }

        private List<Asistence> asistences = new List<Asistence>();
        public List<Asistence> Asistences
        {
            get => asistences;
            set => asistences = value ?? new List<Asistence>();
        }

        private List<Pago> pagos = new List<Pago>();
        public List<Pago> Pagos
        {
            get => pagos;
            set => pagos = value ?? new List<Pago>();
        }

        private List<Cuota> cuotas = new List<Cuota>();
        public List<Cuota> Cuotas
        {
            get => cuotas;
            set => cuotas = value ?? new List<Cuota>();
        }

        private SocioModel socio = new SocioModel();
        public SocioModel Socio
        {
            get => socio;
            set => socio = value ?? new SocioModel();
        }

        private List<Incidencia> incidencias = new List<Incidencia>();
        public List<Incidencia> Incidencias
        {
            get => incidencias;
            set => incidencias = value ?? new List<Incidencia>();
        }
    }



    public sealed class HorarioSigleton
    {
        private static readonly Lazy<HorarioSigleton> lazyInstance = new Lazy<HorarioSigleton>(() => new HorarioSigleton());

        private HorarioSigleton()
        {
            HorarioProfesional = new List<HorarioProfesional>();
            HorarioFijo = new HorarioFijo();
            Personal = new SocioModel();
            Personal = new SocioModel();
            SelectedPersonal = false;
        }

        public static HorarioSigleton Instance => lazyInstance.Value;

        public string Message { get; set; }
        public bool SelectedPersonal { get; set; }


        private SocioModel personal = new SocioModel();
        public SocioModel Personal
        {
            get => personal;
            set => personal = value ?? new SocioModel();
        }

        

        private HorarioFijo horario_fijo = new HorarioFijo();
        public HorarioFijo HorarioFijo
        {
            get => horario_fijo;
            set => horario_fijo = value ?? new HorarioFijo();
        }

        private List<HorarioProfesional> horario_pro = new List<HorarioProfesional>();
        public List<HorarioProfesional> HorarioProfesional
        {
            get => horario_pro;
            set => horario_pro = value ?? new List<HorarioProfesional>();
        }

    }

}

