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

        //[WebGet(UriTemplate = "Create?keyName={keyName}&cubeValue={cubeValue}&parentKey={parentKey}&cubeType={cubeType}", ResponseFormat = WebMessageFormat.Json)]
        //public Stream Create(string keyName, string cubeValue, string parentKey, string cubeType) {
        //    var cube = new CubeItem {
        //        CubeType = cubeType,
        //        CubeValue = cubeValue,
        //        KeyName = keyName,
        //        ParentKey = parentKey
        //    };

        //    var newId = Dao.Create(cube);
        //    return ConvertToJsonStream(new { id = newId });
        //}

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

        //[WebInvoke(UriTemplate = "Create", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        //[ScriptMethod]
        //[OperationContract]
        //public Stream Create(CubeItem cubeItem) {
        //    //CubeItem cubeItem = new CubeItem();
        //    var newId = Dao.Create(cubeItem);
        //    return ConvertToJsonStream(new { id = newId });
        //}

        //[WebInvoke(UriTemplate = "", Method = "POST")]
        //public Stream Create(CubeItem cubeItem) {
        //    //CubeItem cubeItem = new CubeItem();
        //    var newId = Dao.Create(cubeItem);
        //    return ConvertToJsonStream(new { id = newId });
        //}

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

        private MemoryStream ConvertToJsonStream(object value) {
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";

            var json = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        }

    }
}
