using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class create_report : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string apiurlreport = ConfigurationManager.AppSettings["ApiUrlReport"];
        string api_user = ConfigurationManager.AppSettings["ApiUser"];
        string api_password = ConfigurationManager.AppSettings["ApiPassword"];
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack)
            //{
            //    // Cek apakah postback berasal dari kontrol yang tepat
            //    if (Request["__EVENTTARGET"] == Select4.ClientID)
            //    {
            //        // Panggil fungsi yang diinginkan, misalnya ubahSite
            //        ubahSite();
            //    }
            //}
            if (!IsPostBack)
            {
                SelectMultiple.Items.Add(new ListItem("All", "all"));
                Select4.Items.Add(new ListItem("All", "all"));

                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_sites", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                var selectedIds = new List<string>();
                                while (dr.Read())
                                {
                                    string id = dr["id"].ToString();
                                    string siteLocation = dr["name"].ToString();
                                    ListItem item = new ListItem(siteLocation, id);

                                    Select4.Items.Add(item);
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id, c.name, c.location, c.camera_site_id, cs.name as site FROM cameras c JOIN camera_sites cs ON c.camera_site_id = cs.id", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    string id = dr["id"].ToString();
                                    string siteLocation = dr["site"].ToString() + " - " + dr["location"].ToString();
                                    string siteId = dr["camera_site_id"].ToString();

                                    ListItem item = new ListItem(siteLocation, id);
                                    item.Attributes.Add("data-siteid", siteId);
                                    SelectMultiple.Items.Add(item);
                                }
                            }
                        }
                    }

                }
                //Select1.Items.Add(new ListItem("", "none"));
                Select1.Items.Add(new ListItem("All", "all"));
                Select1.Items.Add(new ListItem("Face Recognition", "face recognition"));
                Select1.Items.Add(new ListItem("License Plate Recognition", "license plate recognition"));
                Select1.Items.Add(new ListItem("Traffic Detection", "traffic detection"));

                //Select2.Items.Add(new ListItem("", "none"));
                Select2.Items.Add(new ListItem("All", "all"));
                Select2.Items.Add(new ListItem("Comply", "0"));
                Select2.Items.Add(new ListItem("Not Comply or Badge Expired", "1"));
                Select2.Items.Add(new ListItem("FTW Rejected Medical / Expired", "2"));
                Select2.Items.Add(new ListItem("Daily Checkup Failed", "3"));
                Select2.Items.Add(new ListItem("Blacklist", "blacklist"));

                Select3.Items.Add(new ListItem("All", "all"));
                Select3.Items.Add(new ListItem("Comply", "0"));
                Select3.Items.Add(new ListItem("Not Approved", "1"));
                //Select3.Items.Add(new ListItem("", "2"));
                Select3.Items.Add(new ListItem("Unrecognized", "unregistered"));

                Select5.Items.Add(new ListItem("None", "none"));
                Select5.Items.Add(new ListItem("URL", "url"));
                Select5.Items.Add(new ListItem("Embed", "embed"));
            }
        }

        protected async void Button1_Click(object sender, EventArgs e)
        {
            string text1 = TextBox1.Text;
            string startDate = "";
            if (text1 == "")
            { }
            else
            {
                DateTime parsedDateTime = DateTime.Parse(text1);
                startDate += parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string text2 = TextBox2.Text;
            string endDate = "";
            if (text2 == "")
            { }
            else
            {
                DateTime parsedDateTime2 = DateTime.Parse(text2);
                endDate += parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string is_unique_valid_vehicle;
            string is_unique_invalid_vehicle;
            string is_unique_valid_person;
            string is_unique_invalid_person;
            string is_unique_blacklist_person;
            if (CheckBox1.Checked)
            {
                is_unique_valid_vehicle = "1";
            }
            else
            {
                is_unique_valid_vehicle = "0";
            }
            if (CheckBox5.Checked)
            {
                is_unique_invalid_vehicle = "1";
            }
            else
            {
                is_unique_invalid_vehicle = "0";
            }
            if (CheckBox2.Checked)
            {
                is_unique_valid_person = "1";
            }
            else
            {
                is_unique_valid_person = "0";
            }
            if (CheckBox3.Checked)
            {
                is_unique_invalid_person = "1";
            }
            else
            {
                is_unique_invalid_person = "0";
            }
            if (CheckBox4.Checked)
            {
                is_unique_blacklist_person = "1";
            }
            else
            {
                is_unique_blacklist_person = "0";
            }
            List<string> selectedCameras = new List<string>();
            List<string> selectedSnapshots = new List<string>();
            List<string> selectedPersonStatus = new List<string>();
            List<string> selectedVehicleStatus = new List<string>();
            List<string> selectedSites = new List<string>();
            //List<string> selectedImages = new List<string>();
            foreach (ListItem item in SelectMultiple.Items)
            {
                if (item.Selected)
                {
                    selectedCameras.Add(item.Value);
                }
            }
            foreach (ListItem item2 in Select1.Items)
            {
                if (item2.Selected)
                {
                    selectedSnapshots.Add(item2.Value);
                }
            }
            foreach (ListItem item3 in Select2.Items)
            {
                if (item3.Selected)
                {
                    selectedPersonStatus.Add(item3.Value);
                }
            }
            foreach (ListItem item4 in Select3.Items)
            {
                if (item4.Selected)
                {
                    selectedVehicleStatus.Add(item4.Value);
                }
            }
            foreach (ListItem item5 in Select4.Items)
            {
                if (item5.Selected)
                {
                    selectedSites.Add(item5.Value);
                }
            }
            //foreach (ListItem item6 in Select5.Items)
            //{
            //    if (item6.Selected)
            //    {
            //        selectedImages.Add(item6.Value);
            //    }
            //}
            var selectedImages = Select5.SelectedValue;
            var jsonObject = new
            {
                cameras = selectedCameras,
                snapshot_types = selectedSnapshots,
                person_status = selectedPersonStatus,
                vehicle_status = selectedVehicleStatus,
                start_date = startDate,
                end_date = endDate,
                sites = selectedSites,
                image_type = selectedImages,
                is_valid_vehicle_unique = is_unique_valid_vehicle == "0" ? false : true,
                is_invalid_vehicle_unique = is_unique_invalid_vehicle == "0" ? false : true,
                is_valid_person_unique = is_unique_valid_person == "0" ? false : true,
                is_invalid_person_unique = is_unique_invalid_person == "0" ? false : true,
                is_blacklist_person_unique = is_unique_blacklist_person == "0" ? false : true,
            };


            var json = new JavaScriptSerializer().Serialize(jsonObject);

            //Response.Write(json);

            using (var client = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes(api_user + ":" + api_password);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var content = new StringContent(json, encoding: Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiurlreport + "/v2/reports", content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Session["success"] = "On process to get report";
                    Response.Redirect("list_report.aspx");
                    //lblOutput.Text = $"Response: {responseData}";
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var jsonData = (JObject)JsonConvert.DeserializeObject(responseBody);
                    Session["fail"] = "Error: " + jsonData["message"];
                    Response.Redirect("list_report.aspx");
                    //lblOutput.Text = "Error: " + response.Content + ". json: " + json;
                }
            }
        }
    }

}