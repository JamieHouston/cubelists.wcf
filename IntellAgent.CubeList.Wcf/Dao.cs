using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace IntellAgent.CubeList.Wcf {
    public class Dao {
        static string _connectionString =
                "Server=localhost;Initial Catalog=CubeLists;User Id=CubeList;Password=CubeList";

        public static IList<CubeItem> GetAllCubes() {
            return GetCubes("GetCubes");
        }

        public static CubeItem GetCube(string keyName) {
            KeyValuePair<string, object> parameter = new KeyValuePair<string, object>("KeyName", keyName);
            return GetCubes("GetCubeByKey", new List<KeyValuePair<string, object>> { parameter }).FirstOrDefault();
        }

        public static int Create(CubeItem cubeItem)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                using (SqlCommand command = new SqlCommand("CreateCube", connection)) {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("KeyName", cubeItem.KeyName);
                    command.Parameters.AddWithValue("CubeType", cubeItem.CubeType);
                    command.Parameters.AddWithValue("ParentKey", cubeItem.ParentKey);
                    command.Parameters.AddWithValue("CubeValue", cubeItem.CubeValue);

                    connection.Open();
                    var result = command.ExecuteScalar();
                    int.TryParse(result.ToString(), out id);

                    connection.Close();
                }
            }
            return id;
        }

        public static dynamic GetCubeWithChildren(string parentKey) {
            KeyValuePair<string, object> parameter = new KeyValuePair<string, object>("ParentKey", parentKey);
            CubeItem cube = GetCubes("GetCubeByKey",
                new List<KeyValuePair<string, object>>{
                                    new KeyValuePair<string, object>("KeyName", parentKey)}).FirstOrDefault();

            IList<CubeItem> children = GetCubes("GetCubesByParentKey",
                new List<KeyValuePair<string, object>>{
                                    new KeyValuePair<string, object>("ParentKey", parentKey)});

            cube.Cubes = children;
            return cube;
        }

        private static IList<CubeItem> GetCubes(string procedure, List<KeyValuePair<string, object>> parameters = null) {
            IList<CubeItem> cubes = new List<CubeItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString)) {

                using (SqlCommand cmd = new SqlCommand(procedure, connection)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null) {
                        parameters.ForEach(parameter =>
                                           cmd.Parameters.AddWithValue(parameter.Key, parameter.Value));
                    }

                    connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) {
                        CubeItem cubeItem = new CubeItem {
                            Id = (int)dr["Id"],
                            CubeType = dr["CubeType"].ToString(),
                            KeyName = dr["KeyName"].ToString(),
                            ParentKey = dr["ParentKey"].ToString(),
                            //ModifiedDate = (DateTime)dr["ModifiedDate"],
                            CubeValue = dr["CubeValue"].ToString()
                        };

                        cubes.Add(cubeItem);
                    }
                    connection.Close();
                    return cubes;
                }
            }
        }
    }
}