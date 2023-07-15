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
         private readonly string apiUrl = "https://webapiappsfit-cliente.azurewebsites.net/api";
        //private readonly string apiUrl = "https://localhost:44386/api";

        public AppsFitService()
        {
            _httpClient = new HttpClient();

        }
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
                    resp.Success= false;
                }
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

     

    }
}
