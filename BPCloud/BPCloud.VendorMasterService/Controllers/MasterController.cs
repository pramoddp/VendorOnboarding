using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using BPCloud.VendorMasterService.Models;
using BPCloud.VendorMasterService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BPCloud.VendorMasterService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ITypeRepository _typeRepository;
        private readonly IPostalRepository _PostalRepository;
        private readonly IIdentityRepository _IdentityRepository;
        private readonly IBankRepository _BankRepository;
        private readonly ITitleRepository _TitleRepository;
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly IAppRepository _AppRepository;
        private readonly ILocationRepository _LocationRepository;
        private readonly IFieldMasterRepository _FieldMasterRepository;
        private IConfiguration _configuration;
        public static string concatenatedGspAuthToken;

        public MasterController(ITypeRepository typeRepository, IPostalRepository PostalRepository, IIdentityRepository IdentityRepository, IBankRepository BankRepository,
            ITitleRepository TitleRepository, IDepartmentRepository DepartmentRepository, IAppRepository AppRepository, ILocationRepository LocationRepository,
            IFieldMasterRepository FieldMasterRepository,IConfiguration configuration)
        {
            _typeRepository = typeRepository;
            _PostalRepository = PostalRepository;
            _IdentityRepository = IdentityRepository;
            _BankRepository = BankRepository;
            _TitleRepository = TitleRepository;
            _DepartmentRepository = DepartmentRepository;
            _AppRepository = AppRepository;
            _LocationRepository = LocationRepository;
            _FieldMasterRepository = FieldMasterRepository;
            _configuration = configuration;
        }
        public GspAuthResponse GetGspAuthToken()
        {
            GspAuthResponse gspauthResponse = new GspAuthResponse();
            try
            {
                //string errorCodeDescription = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://gsp.adaequare.com/gsp/authenticate?grant_type=token");
                request.Method = "POST";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Accept = "*";
                request.ContentType = "application/json";
                request.Headers.Add("gspappid", "7C64ED016B324DBF9B9D811AE9AC3A7A");
                request.Headers.Add("gspappsecret", "4768421CG01A3G4AE0GA251GB2F999FD28C5");
                string str3 = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
                gspauthResponse = JsonConvert.DeserializeObject<GspAuthResponse>(str3);
                gspauthResponse.errorStatus = false;
                gspauthResponse.errorMessage = "null";

            }
            catch (Exception exception)
            {
                gspauthResponse.errorStatus = true;
                gspauthResponse.errorMessage = exception.Message;
                //Log.WriteProcessLog("GSTReturns/GetGspAuthToken/Exception:- StackTrace: " + exception.StackTrace + " Message: " + exception.Message);

            }
            return gspauthResponse;
        }
        [HttpGet]
        public object SearchTaxPayer(string GSTNumber)
        {
            try
            {
                GspAuthResponse gspAuthResponse = GetGspAuthToken();
                GSTRResponse gstresponse = new GSTRResponse();
                TaxPayer payer;
                if (gspAuthResponse != null)
                {
                    concatenatedGspAuthToken = gspAuthResponse.token_type + " " + gspAuthResponse.access_token;
                }
                string uri = "https://gsp.adaequare.com/gstn/commonapi/search?action=TP&gstin=" + GSTNumber;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                request.ContentType = "application/json";
                request.Method = "GET";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Accept = "*";
                request.Headers.Add("Authorization", concatenatedGspAuthToken);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var read = new StreamReader(response.GetResponseStream()))
                {
                    var result = read.ReadToEnd();
                    if (result.Contains("error"))
                    {
                        return false;
                        //return JsonConvert.DeserializeObject<ErrorResponse>(result);
                    }
                    gstresponse = JsonConvert.DeserializeObject<GSTRResponse>(result);
                }
                gstresponse.data = Encoding.UTF8.GetString(System.Convert.FromBase64String(gstresponse.data));
                gstresponse.errorStatus = false;
                gstresponse.errorMessage = "Error";
                payer = JsonConvert.DeserializeObject<TaxPayer>(gstresponse.data);
                if(payer != null)
                {
                    TaxPayerDetails Details = new TaxPayerDetails();

                    Details.gstin = payer.gstin;
                    Details.tradeName = payer.tradeNam;

                    Details.legalName = payer.lgnm;

                    Details.address1 = payer.pradr.addr.bno + " , "+ payer.pradr.addr.st + " , " + payer.pradr.addr.loc;
                    Details.address2 = payer.pradr.addr.stcd;
                    Details.pinCode = payer.pradr.addr.pncd;
                    Details.stateCode = payer.gstin.Substring(0,2);
                    return Details;
                    //Details.txpType = payer.gstin;
                    //Details.status = payer.sts;
                    //Details.blkStatus = payer.gstin;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        #region CBPType

        [HttpGet]
        public List<CBPType> GetAllTypes()
        {
            try
            {
                var types = _typeRepository.GetAllTypes();
                return types;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllTypes", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateType(CBPType type)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _typeRepository.CreateType(type);
                return CreatedAtAction(nameof(GetAllTypes), new { Type = type.Type }, type);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateType", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateType(CBPType type)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _typeRepository.UpdateType(type);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateType", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteType(CBPType type)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _typeRepository.DeleteType(type);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteType", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPPostal

        [HttpGet]
        public List<CBPPostal> GetAllPostals()
        {
            try
            {
                var Postals = _PostalRepository.GetAllPostals();
                return Postals;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllPostals", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostal(CBPPostal Postal)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PostalRepository.CreatePostal(Postal);
                return CreatedAtAction(nameof(GetAllPostals), new { PostalCode = Postal.PostalCode }, Postal);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreatePostal", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostal(CBPPostal Postal)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PostalRepository.UpdatePostal(Postal);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdatePostal", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePostal(CBPPostal Postal)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PostalRepository.DeletePostal(Postal);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeletePostal", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPIdentity

        [HttpGet]
        public List<CBPIdentity> GetAllIdentities()
        {
            try
            {
                var Identitys = _IdentityRepository.GetAllIdentities();
                return Identitys;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllIdentitys", ex);
                return null;
            }
        }

        [HttpGet]
        public List<string> GetAllIdentityTypes()
        {
            try
            {
                var IdentityTypes = _IdentityRepository.GetAllIdentityTypes();
                return IdentityTypes;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllIdentityTypes", ex);
                return null;
            }
        }

        [HttpGet]
        public CBPIdentity GetIdentityByType(string Type)
        {
            try
            {
                var Identity = _IdentityRepository.GetIdentityByType(Type);
                return Identity;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetIdentityByType", ex);
                return null;
            }
        }
     
        [HttpGet]
        public CBPIdentity ValidateIdentityByType(string IdentityType, string ID)
        {
            try
            {
                string status = "";
                var Identity = _IdentityRepository.ValidateIdentityByType(IdentityType);
                if (Identity != null)
                {
                    if (Identity.Text.ToLower() == "pancard")
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(Identity.RegexFormat);
                        //If you want both upper case and lower case alphabets.  "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$"
                        Match match = regex.Match(ID); // here add your textbox name  
                        if (match.Success)
                        {
                            status = "Matched";
                            return Identity;
                        }
                        else
                        {
                            status = "NotMatched";
                            return null;
                        }
                    }
                    else if (Identity.Text.ToLower() == "gstin")
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(Identity.RegexFormat);
                        //If you want both upper case and lower case alphabets.  "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$"
                        Match match = regex.Match(ID); // here add your textbox name  
                        if (match.Success)
                        {
                            status = "Matched";
                            return Identity;
                        }
                        else
                        {
                            status = "NotMatched";
                            return null;
                        }
                    }
                }
                //else
                //{
                //    return BadRequest();
                //}
                //return status;
                return null;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/ValidateIdentityByType", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateIdentity(CBPIdentity Identity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IdentityRepository.CreateIdentity(Identity);
                return CreatedAtAction(nameof(GetAllIdentities), new { ID = Identity.ID }, Identity);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateIdentity", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIdentity(CBPIdentity Identity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result=await _IdentityRepository.UpdateIdentity(Identity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateIdentity", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdentity(CBPIdentity Identity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IdentityRepository.DeleteIdentity(Identity);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteIdentity", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPBank

        [HttpGet]
        public List<CBPBank> GetAllBanks()
        {
            try
            {
                var Banks = _BankRepository.GetAllBanks();
                return Banks;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllBanks", ex);
                return null;
            }
        }

        [HttpGet]
        public CBPBank GetBankByIFSC(string IFSC)
        {
            try
            {
                var Bank = _BankRepository.GetBankByIFSC(IFSC);
                return Bank;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetBankByIFSC", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(CBPBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.CreateBank(Bank);
                return CreatedAtAction(nameof(GetAllBanks), new { BankCode = Bank.BankCode }, Bank);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateBank", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBank(CBPBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.UpdateBank(Bank);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateBank", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBank(CBPBank Bank)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _BankRepository.DeleteBank(Bank);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteBank", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPTitle

        [HttpGet]
        public List<CBPTitle> GetAllTitles()
        {
            try
            {
                var Titles = _TitleRepository.GetAllTitles();
                return Titles;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllTitles", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitle(CBPTitle Title)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _TitleRepository.CreateTitle(Title);
                return CreatedAtAction(nameof(GetAllTitles), new { Title = Title.Title }, Title);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateTitle", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTitle(CBPTitle Title)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _TitleRepository.UpdateTitle(Title);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateTitle", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTitle(CBPTitle Title)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _TitleRepository.DeleteTitle(Title);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteTitle", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPDepartment

        [HttpGet]
        public List<CBPDepartment> GetAllDepartments()
        {
            try
            {
                var Departments = _DepartmentRepository.GetAllDepartments();
                return Departments;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllDepartments", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CBPDepartment Department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _DepartmentRepository.CreateDepartment(Department);
                return CreatedAtAction(nameof(GetAllDepartments), new { Department = Department.Department }, Department);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateDepartment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(CBPDepartment Department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _DepartmentRepository.UpdateDepartment(Department);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateDepartment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(CBPDepartment Department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _DepartmentRepository.DeleteDepartment(Department);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteDepartment", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPApp

        [HttpGet]
        public List<CBPApp> GetAllApps()
        {
            try
            {
                var Apps = _AppRepository.GetAllApps();
                return Apps;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllApps", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApp(CBPApp App)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AppRepository.CreateApp(App);
                return CreatedAtAction(nameof(GetAllApps), new { ID = App.ID }, App);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateApp", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApp(CBPApp App)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AppRepository.UpdateApp(App);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateApp", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApp(CBPApp App)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AppRepository.DeleteApp(App);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteApp", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CBPLocation

      [HttpGet]
      public CBPLocation GetLocationByPincode(string Pincode)
        {
            try
            {
                var Location = _LocationRepository.GetLocationByPincode(Pincode);
                return Location;
            }

            catch (Exception ex)
            {   
                WriteLog.WriteToFile("Master/GetLocationByPincode", ex);
                throw ex;
            }
        }
        [HttpGet]
        public List<MyArray> GetLocation(string Pincode)
        {
            try
            {
                var Location = _LocationRepository.GetLocation(Pincode);
                return Location;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetLocation", ex);
                return null;
            }
        }
        [HttpGet]
        public List<StateDetails> GetStateDetails()
        {
            try
            {
                var Location = _LocationRepository.GetStateDetails();
                return Location;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetLocationByPincode", ex);
                return null;
            }
        }
        #endregion

        #region Adopt from Gstin 
        public IActionResult GetTaxPayerDetails(string Gstin)
        {
            try
            {
                //05AAACG2115R1ZN
                string EwaybillAPIBaseAddress = _configuration.GetValue<string>("EwaybillAPIBaseAddress");
                string taxPayerOrTransGstin = Gstin;
                var HostURI = "http://192.168.0.28:7000/vendormasterapi/Master/GetTaxPayerDetails?Gstin=" + taxPayerOrTransGstin;
                var test = GetRequest(HostURI);
                if (test.Contains("errorMessage"))
                {
                    var TaxPayerDetailsError = JsonConvert.DeserializeObject<TaxPayerDetailsError>(test);
                    return Ok(TaxPayerDetailsError);
                }
                else
                {
                    var TaxPayerDetails = JsonConvert.DeserializeObject<TaxPayerDetails>(test);
                    return Ok(TaxPayerDetails);
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetTaxPayerDetails", ex);
                return BadRequest();
            }
        }

        public string GetRequest(string HostURI)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(HostURI);
                request.Method = "GET";
                string test = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        test = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                return test;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetRequest", ex);
                return null;
            }
        }

        //public async Task<IHttpActionResult> AsyncPostRequest(string HostURI, string SerializedObject)
        //{
        //    var uri = new Uri(HostURI);
        //    var request = (HttpWebRequest)WebRequest.Create(uri);
        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);
        //    using (var postStream = await request.GetRequestStreamAsync())
        //    {
        //        await postStream.WriteAsync(requestBody, 0, requestBody.Length);
        //    }
        //    try
        //    {
        //        using (var response = (HttpWebResponse)await request.GetResponseAsync())
        //        {
        //            if (response != null && response.StatusCode == HttpStatusCode.OK)
        //            {
        //                return Ok();
        //            }
        //            else
        //            {
        //                var reader = new StreamReader(response.GetResponseStream());
        //                string responseString = await reader.ReadToEndAsync();
        //                reader.Close();
        //                return Content(response.StatusCode, responseString);
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        using (var stream = ex.Response.GetResponseStream())
        //        using (var reader = new StreamReader(stream))
        //        {
        //            var errorMessage = reader.ReadToEnd();
        //            return Content(HttpStatusCode.InternalServerError, errorMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("AsyncPostRequest : - " + ex.Message);
        //        return Content(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        #endregion

        #region CBPFieldMaster
        [HttpGet]
        public List<CBPFieldMaster> GetAllOnBoardingFieldMaster()
        {
            try
            {
                var BoardingFieldMasters = _FieldMasterRepository.GetAllOnBoardingFieldMaster();
                return BoardingFieldMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllOnBoardingFieldMaster", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOnBoardingFieldMaster(CBPFieldMaster OnBoardingFieldMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _FieldMasterRepository.UpdateOnBoardingFieldMaster(OnBoardingFieldMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/UpdateOnBoardingFieldMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        [HttpGet]
        public List<CBPIdentity> GetAllIdentityFields()
        {
            try
            {
                var BoardingFieldMasters = _FieldMasterRepository.GetAllIdentityFields();
                return BoardingFieldMasters;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllIdentityFields", ex);
                return null;
            }
        }
    }
}