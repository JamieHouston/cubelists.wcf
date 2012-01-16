using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntellAgent.CubeList.Wcf {
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CubeService {

        [WebGet(UriTemplate = "", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetCollection() {
            var value = Dao.GetAllCubes();
            return ConvertToJsonStream(value);
        }

        [WebGet(UriTemplate = "List/{keyName}", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetCubeRows(string keyName) {
            var value = Dao.GetCubeRows(keyName);
            return ConvertToJsonStream(value);
        }

        [WebGet(UriTemplate="create?keyName={keyName}&parentKey={parentKey}&cubeType={cubeType}&cubeValue={cubeValue}")]
        public Stream Test(string keyName, string parentKey, string cubeType, string cubeValue)
        {
            var cube = new CubeItem {
                CubeType = cubeType,
                CubeValue = cubeValue,
                KeyName = keyName,
                ParentKey = parentKey
            };

            var newId = Dao.Create(cube);
            cube.Id = newId;
            return ConvertToJsonStream(cube);
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

        [WebGet(UriTemplate = "delete/{keyName}")]
        public void Delete(string keyName)
        {
            Dao.Delete(keyName);
        }

        private MemoryStream ConvertToJsonStream(object value) {
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";

            var json = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        }

    }
}
