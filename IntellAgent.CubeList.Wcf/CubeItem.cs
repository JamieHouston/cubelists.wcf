using System;
using System.Collections.Generic;

namespace IntellAgent.CubeList.Wcf {
    public class CubeItem {
        public CubeItem()
        {
            ModifiedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string KeyName { get; set; }
        public string CubeValue { get; set; }
        public string ParentKey { get; set; }
        public string CubeType { get; set; }
        public IList<CubeItem> Cubes { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}