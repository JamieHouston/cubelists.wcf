using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntellAgent.CubeList.Wcf {
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CubeService {

        [WebGet(UriTemplate = "", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetCollection()
        {
            var value = Dao.GetAllCubes();
            return ConvertToJsonStream(value);
        }

        [WebInvoke(UriTemplate = "Create", Method = "Get", RequestFormat = WebMessageFormat.Json)]
        public Stream Create(CubeItem cubeItem)
        {
            //CubeItem cubeItem = new CubeItem();
            var newId = Dao.Create(cubeItem);
            return ConvertToJsonStream(new { id = newId });
        }

        [WebGet(UriTemplate = "{keyName}", ResponseFormat = WebMessageFormat.Json)]
        public Stream Get(string keyName) {
            var cube = Dao.GetCubeWithChildren(keyName);
            return ConvertToJsonStream(cube);
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public Stream Update(string id, CubeItem instance) {
            throw new NotImplementedException();
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public void Delete(string id) {
            throw new NotImplementedException();
        }

        private MemoryStream ConvertToJsonStream(object value)
        {
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";

                var json = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        }

    }
}
