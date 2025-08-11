using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace hr_cam
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        [WebMethod]
        public List<List<object>> HelloWorld()
        {
            List<List<object>> list = new List<List<object>>();
            using (MySqlConnection con1 = new MySqlConnection(strcon))
            {
                if (con1.State == ConnectionState.Closed)
                {
                    con1.Open();
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_events", con1))
                {
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            List<object> innerList = new List<object>();
                            innerList.Add(dr["id"]);
                            innerList.Add(dr["type"]);
                            innerList.Add(dr["occurred_at"]);
                            list.Add(innerList);
                        }
                    }
                }
            }
            return list;
        }
    }
}
