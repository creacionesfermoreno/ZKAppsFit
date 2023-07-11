using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZKTecoFingerPrintScanner_Implementation.Models
{
    public class Requests
    {
    }
    public class ResponseModel
    {
        
        public string Message1 { get; set; }
        public bool Success { get; set; }
        [JsonProperty("Date")]
        public DataItem Data { get; set; }
    }
    public class DataItem
    {
        public int code { get; set; }
        public string name { get; set; }
        public string surnames { get; set; }
        public string nro_document { get; set; }
        public string image { get; set; }
        public string huella { get; set; }
        public int unidad { get; set; }
        public int sede { get; set; }
        public string rubro { get; set; }
    }


    public class ResponseFinger
    {

        public string Message1 { get; set; }
        public bool Success { get; set; }
        [JsonProperty("Date")]
        public List<SocioModel> Data { get; set; }
    }

    public class ResponseGeneric
    {
        public string Message1 { get; set; }
        public bool Success { get; set; }
        [JsonProperty("Date")]
        public List<Membresia> Data { get; set; }
    }
    public class ResponseAsistence
    {
        public string Message1 { get; set; }
        public bool Success { get; set; }
        [JsonProperty("Date")]
        public List<Asistence> Data { get; set; }
    }

    public class ResponseHPC
    {
        public string Message1 { get; set; }
        public bool Success { get; set; }
        [JsonProperty("Date")]
        public Date Data { get; set; }
    }


    public class ResponseBase
    {
        public string Message1 { get; set; }
        public bool Success { get; set; }
        [JsonProperty("Date")]
        public List<Object> Data { get; set; }
    }

    public class Date
    {
        public List<Pago> Pagos { get; set; }
        public List<Cuota> Cuotas { get; set; }
        public List<Incidencia> Incidencias { get; set; }
    }
}
