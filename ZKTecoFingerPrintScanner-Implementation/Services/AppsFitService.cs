using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZKTecoFingerPrintScanner_Implementation.Models;

namespace ZKTecoFingerPrintScanner_Implementation.Services
{
    public class AppsFitService
    {
        private readonly HttpClient _httpClient;
        //private readonly string apiUrl = "https://webapiappsfit-cliente.azurewebsites.net/api";
        private readonly string apiUrl = "https://localhost:44386/api";

        public AppsFitService()
        {
            _httpClient = new HttpClient();

        }
        
        public async Task<ResponseModel> SearchSocioByHuella(object body)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/socio/huella", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        

        public async Task<ResponseGeneric> MembresiasList(object body)
        {
            ResponseGeneric resp = new ResponseGeneric();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/socio/membresias", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseGeneric>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //api list asistences historial
        public async Task<ResponseAsistence> AsistencesList(object body)
        {
            ResponseAsistence resp = new ResponseAsistence();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/socio/membresias/historial-asistences", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseAsistence>(responseContent);
                    resp.Data = resp.Data ?? new List<Asistence>();
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }


        public async Task<ResponseHPC> HistorialPC(object body)
        {
            ResponseHPC resp = new ResponseHPC();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/socio/hpagos-cuotas", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseHPC>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }


        public async Task<ResponseBase> MarkAsistence(object body)
        {
            ResponseBase resp = new ResponseBase();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/socio/mark-asistence", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseBase>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }



        public async Task<ResponseModel> VerifyDkey(object body)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/bussiness", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //************************************************************ +search+ ***************************************
        //socio
        public async Task<ResponseModel> SearchSocio(object body)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/socio", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //fijo

        public async Task<ResponseModel> SearchPF(string dni)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(new { });
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/search/pf/" + dni, httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //event
        public async Task<ResponseModel> SearchPEvent(string dni)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(new { });
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/search/pevent/" + dni, httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }
        //************************************************************ -search- ***************************************


        //************************************************************ +register huella+ ***************************************
        //socio
        public async Task<ResponseModel> RegHuellaAPI(object body)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/huella-register", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }
        
        //fijo
        public async Task<ResponseModel> RegHuellaAPIFijo(object body)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/pf/huella-register", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //event
        public async Task<ResponseModel> RegHuellaAPIFEvent(object body)
        {
            ResponseModel resp = new ResponseModel();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/pevent/huella-register", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //************************************************************ -register huella- ***************************************



        //************************************************************ +lst huella+ ***************************************
        //socios
        public async Task<ResponseFinger> FingerPrintsList(object body)
        {
            ResponseFinger resp = new ResponseFinger();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/huellas", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseFinger>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //fijos
        public async Task<ResponseFinger> FingerPrintsListFijo(object body)
        {
            ResponseFinger resp = new ResponseFinger();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/huellas/pfijo", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseFinger>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }


        //event
        public async Task<ResponseFinger> FingerPrintsListEvent(object body)
        {
            ResponseFinger resp = new ResponseFinger();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/huellas/pevents", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseFinger>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }


        //************************************************************ -lst huella- ***************************************



        //************************************************************ horarios ***************************************
        //Fijos
        public async Task<ResponseHFijo> HorarioFijo(object body)
        {
            ResponseHFijo resp = new ResponseHFijo();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/horarios/fijo", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseHFijo>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }


        //profesional
        public async Task<ResponseHProfesional> HorarioProfesional(object body)
        {
            ResponseHProfesional resp = new ResponseHProfesional();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/horarios/profesionales", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseHProfesional>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //************************************************************ -horarios- ***************************************


        //*************************************************** MARK ASISTENCE PERSONAL ***************************************************

        //fijos
        public async Task<ResponseObject> MarcarPersonalFijo(object body)
        {
            ResponseObject resp = new ResponseObject();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/asistences/mark/fijo", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseObject>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }


        //profesionales
        public async Task<ResponseObject> MarcarPersonalProfesional(object body)
        {
            ResponseObject resp = new ResponseObject();
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl + "/home/bios/asistences/profesional/mark", httpContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ResponseObject>(responseContent);
                }
                else
                {
                    resp.Success = false;
                }
            }
            catch (HttpRequestException ex)
            {
                resp.Message1 = ex.Message;
            }
            return resp;
        }

        //********************************************************************************************************************
    }
}
