using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Models;
using Newtonsoft.Json;

namespace BPCloud.VendorMasterService.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly MasterContext _dbContext;

        public LocationRepository(MasterContext dbContext)
        {
            _dbContext = dbContext;
        }
        public CBPLocation GetLocationByPincode(string Pincode)
        {
            try
            {
                
                return _dbContext.CBPLocations.FirstOrDefault(x => x.Pincode == Pincode);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MyArray> GetLocation(string Pincode)
        {
            try
            {

                Postal Response2 = new Postal();
                string uri = "https://api.postalpincode.in/pincode/" + Pincode;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Accept = "*";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var read = new StreamReader(response.GetResponseStream()))
                {
                    var result = read.ReadToEnd();
                    //var JTokendata = JToken.Parse(result).ToObject<Postal>();
                    var serialize = JsonConvert.DeserializeObject<List<MyArray>>(result);
                    return serialize;
                }
            }

            catch (Exception ex)

            {
                WriteLog.WriteToFile("Master/GetLocationByPincode", ex);
                throw ex;
            }
        }
        public List<StateDetails> GetStateDetails()
        {
            try
            {
                var result = (from tb in _dbContext.CBPLocations
                              where tb.State != null && tb.StateCode != null
                              orderby tb.StateCode
                              select new StateDetails
                              {
                                  State = tb.State,
                                  StateCode = tb.StateCode
                              }).ToList();
                var unique = result.GroupBy(x => new { x.State, x.StateCode }).Select(x => x.First()).ToList();
                var result1= unique.OrderBy(x => x.StateCode).Where(tb => tb.State != null && tb.StateCode != null).ToList();
                return result1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
