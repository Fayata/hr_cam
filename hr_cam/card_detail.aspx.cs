using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class card_detail : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {


            string card = Request.QueryString["card"];
            string start_date = Request.QueryString["start_date"];
            string end_date = Request.QueryString["end_date"];
            string site = Request.QueryString["sites"];
            string location = Request.QueryString["location"];
            if (!IsPostBack)
            {
                if (start_date != "" && start_date != null)
                {
                    TextBox1.Text = start_date;
                }
                if (end_date != "" && end_date != null)
                {
                    TextBox2.Text = end_date;
                }
                if (location != "" && location != null)
                {
                    location = location.TrimEnd(',');
                    string[] locationArray = location.Split(',');
                    if (locationArray[0] == "all")
                    {
                        ListItem item2 = new ListItem("All Camera", "all");
                        item2.Selected = true;
                        SelectMultiple.Items.Add(item2);
                    }
                    else
                    {
                        ListItem item2 = new ListItem("All Camera", "all");
                        SelectMultiple.Items.Add(item2);
                    }
                }
                else
                {
                    ListItem item2 = new ListItem("All Camera", "all");
                    SelectMultiple.Items.Add(item2);
                }

                if (site != "" && site != null)
                {
                    site = site.TrimEnd(',');
                    string[] siteArray = site.Split(',');
                    if (siteArray[0] == "all")
                    {
                        ListItem item2 = new ListItem("All Site", "all");
                        item2.Selected = true;
                        Select1.Items.Add(item2);
                    }
                    else
                    {
                        ListItem item2 = new ListItem("All Site", "all");
                        Select1.Items.Add(item2);
                    }
                }
                else
                {
                    ListItem item2 = new ListItem("All Site", "all");
                    Select1.Items.Add(item2);
                }

                //SelectMultiple.Items.Add(new ListItem("All Camera", "all"));
                //Select1.Items.Add(new ListItem("All Site", "all"));
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id, c.name, c.location, cs.name as site, cs.id as camera_site_id from cameras c join camera_sites cs on c.camera_site_id=cs.id", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                //string site = Request.QueryString["site"];
                                var selectedIds = new List<string>();
                                if (location != "" && location != null)
                                {
                                    location = location.TrimEnd(',');
                                    string[] locationArray = location.Split(',');
                                    int y = 0;
                                    foreach (string location_id in locationArray)
                                    {
                                        string locationId = location_id;
                                        if (locationId == "all")
                                        {
                                            //ListItem item2 = new ListItem("All Camera", "all");
                                            //item2.Selected = true;
                                            //SelectMultiple.Items.Add(item2);
                                        }
                                        else
                                        {
                                            //ListItem item2 = new ListItem("All Camera", "all");
                                            //SelectMultiple.Items.Add(item2);
                                            selectedIds.Add(locationId);
                                        }
                                        y++;
                                    }
                                }
                                else
                                {
                                    //ListItem item2 = new ListItem("All Camera", "all");
                                    //SelectMultiple.Items.Add(item2);
                                }
                                //for (int i = 0; i < jumlahCamera; i++)
                                //{
                                //    var sessionValue = Session[$"camera_people{i}"];
                                //    if (sessionValue == null) break;
                                //    selectedIds.Add(sessionValue.ToString());
                                //}
                                while (dr.Read())
                                {
                                    //SelectMultiple.Items.Add(new ListItem(dr["site"].ToString() + " - " + dr["location"].ToString(), dr["id"].ToString()));
                                    string id = dr["id"].ToString();
                                    string siteLocation = dr["location"].ToString();
                                    string siteId = dr["camera_site_id"].ToString();

                                    ListItem item = new ListItem(siteLocation, id);
                                    item.Attributes.Add("data-siteid", siteId);
                                    // Tambahkan item ke dalam SelectMultiple
                                    //ListItem item = new ListItem(siteLocation, id);

                                    // Tandai sebagai terpilih jika ID ada dalam array selectedIds
                                    if (selectedIds.Contains(id))
                                    {
                                        item.Selected = true;
                                    }

                                    SelectMultiple.Items.Add(item);
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_sites", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                var selectedIds2 = new List<string>();
                                if (site != "" && site != null)
                                {
                                    site = site.TrimEnd(',');
                                    string[] siteArray = site.Split(',');
                                    int y = 0;
                                    foreach (string site_id in siteArray)
                                    {
                                        string siteId = site_id;
                                        if (siteId == "all")
                                        {
                                            //ListItem item2 = new ListItem("All Site", "all");
                                            //item2.Selected = true;
                                            //Select1.Items.Add(item2);
                                        }
                                        else
                                        {
                                            //ListItem item2 = new ListItem("All Site", "all");
                                            //Select1.Items.Add(item2);
                                            selectedIds2.Add(siteId);
                                        }
                                        y++;
                                    }
                                }
                                else
                                {
                                    //ListItem item2 = new ListItem("All Site", "all");
                                    //Select1.Items.Add(item2);
                                }
                                //Response.Write("<script>alert('" + selectedIds2 + "')</script>");
                                //Response.Write("<script>console.log('" + selectedIds2 + "')</script>");
                                //for (int i = 0; i < jumlahCamera; i++)
                                //{
                                //    var sessionValue = Session[$"camera_people{i}"];
                                //    if (sessionValue == null) break;
                                //    selectedIds2.Add(sessionValue.ToString());
                                //}
                                while (dr.Read())
                                {
                                    //SelectMultiple.Items.Add(new ListItem(dr["site"].ToString() + " - " + dr["location"].ToString(), dr["id"].ToString()));
                                    string id = dr["id"].ToString();
                                    string site_name = dr["name"].ToString();

                                    // Tambahkan item ke dalam SelectMultiple
                                    ListItem item2 = new ListItem(site_name, id);

                                    // Tandai sebagai terpilih jika ID ada dalam array selectedIds
                                    if (selectedIds2.Contains(id))
                                    {
                                        item2.Selected = true;
                                    }

                                    Select1.Items.Add(item2);
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
                if (card == "Card1" || card == "Card2" || card == "Card5" || card == "Card6" || card == "Card9" || card == "Card10" || card == "Card13" || card == "Card14" || card == "Card17" || card == "Card18")
                {
                    DateTime fromDate = DateTime.Now;
                    string today = fromDate.ToString("yyyy-MM-dd");
                    // Menetapkan rentang tanggal min dan max secara dinamis
                    TextBox1.Attributes["min"] = today + "T00:00";  // Tanggal minimal
                    TextBox1.Attributes["max"] = today + "T23:59";  // Tanggal maksimal

                    TextBox2.Attributes["min"] = today + "T00:00";  // Tanggal minimal
                    TextBox2.Attributes["max"] = today + "T23:59";  // Tanggal maksimal
                }
                if (card == "Card3" || card == "Card4" || card == "Card7" || card == "Card8" || card == "Card11" || card == "Card12" || card == "Card15" || card == "Card16" || card == "Card19" || card == "Card20")
                {
                    DateTime fromDate = DateTime.Now;
                    string today = fromDate.ToString("yyyy-MM-dd");
                    string first_date = today.Substring(0, 4) + "-" + today.Substring(5, 2) + "-" + "01";
                    // Menetapkan rentang tanggal min dan max secara dinamis
                    TextBox1.Attributes["min"] = first_date + "T00:00";  // Tanggal minimal
                    TextBox1.Attributes["max"] = today + "T23:59";  // Tanggal maksimal

                    TextBox2.Attributes["min"] = first_date + "T00:00";  // Tanggal minimal
                    TextBox2.Attributes["max"] = today + "T23:59";  // Tanggal maksimal
                }
            }
            if (card == "Card1")
            {
                Judul.Text = "Face Detected Today";
                GenerateTableHead();
                FillTable(card, start_date, end_date, site, location);
            }
            else if (card == "Card2")
            {
                Judul.Text = "Today's Blacklist Detection";
                GenerateTableHead();
                FillTable(card, start_date, end_date, site, location);
            }
            else if (card == "Card3")
            {
                Judul.Text = "Face Detected This Month";
                GenerateTableHead();
                FillTable2(card, start_date, end_date, site, location);
            }
            else if (card == "Card4")
            {
                Judul.Text = "Blacklist Detection This Month";
                GenerateTableHead();
                FillTable2(card, start_date, end_date, site, location);
            }
            else if (card == "Card5")
            {
                Judul.Text = "Today's Unique Fit To Work";
                GenerateTableHead();
                FillTable(card, start_date, end_date, site, location);
            }
            else if (card == "Card6")
            {
                Judul.Text = "Today's Invalid or Exception";
                GenerateTableHead();
                FillTable(card, start_date, end_date, site, location);
            }
            else if (card == "Card7")
            {
                Judul.Text = "Unique Fit To Work This Month";
                GenerateTableHead();
                FillTable2(card, start_date, end_date, site, location);
            }
            else if (card == "Card8")
            {
                Judul.Text = "Invalid or Exception This Month";
                GenerateTableHead();
                FillTable2(card, start_date, end_date, site, location);
            }
            else if (card == "Card9")
            {
                Judul.Text = "Today's Unique License Plate Detected";
                GenerateTableHead2();
                //FillTable3("Card13");
                //FillTable3("Card14");
                //FillTable3("Card19");
                FillTable3(card, start_date, end_date, site, location);
            }
            else if (card == "Card10")
            {
                Judul.Text = "Today's Traffic Detected";
                GenerateTableHead3();
                FillTable4(card, start_date, end_date, site, location);
            }
            else if (card == "Card11")
            {
                Judul.Text = "Unique License Plate Detected This Month";
                GenerateTableHead2();
                FillTable5(card, start_date, end_date, site, location);
            }
            else if (card == "Card12")
            {
                Judul.Text = "Traffic Detected This Month";
                GenerateTableHead3();
                FillTable6(card, start_date, end_date, site, location);
            }
            else if (card == "Card13")
            {
                Judul.Text = "Today's Unique License Plate Recognized";
                GenerateTableHead2();
                FillTable3(card, start_date, end_date, site, location);
            }
            else if (card == "Card14")
            {
                Judul.Text = "Today's Unique Expire License Plate";
                GenerateTableHead2();
                FillTable3(card, start_date, end_date, site, location);
            }
            else if (card == "Card15")
            {
                Judul.Text = "Unique License Plate Recognized This Month";
                GenerateTableHead2();
                FillTable5(card, start_date, end_date, site, location);
            }
            else if (card == "Card16")
            {
                Judul.Text = "Unique Expire License Plate This Month";
                GenerateTableHead2();
                FillTable5(card, start_date, end_date, site, location);
            }
            else if (card == "Card17")
            {
                Judul.Text = "Today's Unique Expiring License Plate";
                GenerateTableHead2();
                FillTable3(card, start_date, end_date, site, location);
            }
            else if (card == "Card18")
            {
                Judul.Text = "Today's Unique Unrecognized License Plate";
                GenerateTableHead2();
                FillTable7(card, start_date, end_date, site, location);
            }
            else if (card == "Card19")
            {
                Judul.Text = "Unique Expiring License Plate This Month";
                GenerateTableHead2();
                FillTable5(card, start_date, end_date, site, location);
            }
            else if (card == "Card20")
            {
                Judul.Text = "Unique Unrecognized License Plate This Month";
                GenerateTableHead2();
                FillTable7(card, start_date, end_date, site, location);
            }
            else if (card == "Card2.1")
            {
                Judul.Text = "People Detection";
                GenerateTableHead();
                FillTable8(card);
            }
            else if (card == "Card2.2")
            {
                Judul.Text = "DPO Detection";
                GenerateTableHead();
                FillTable8(card);
            }
            else if (card == "Card2.3")
            {
                Judul.Text = "License Plate Recognized";
                GenerateTableHead2();
                FillTable9(card);
            }
            else if (card == "Card2.4")
            {
                Judul.Text = "Person Valid Entry";
                GenerateTableHead();
                FillTable8(card);
            }
            else if (card == "Card2.5")
            {
                Judul.Text = "Invalid or Exception";
                GenerateTableHead();
                FillTable8(card);
            }
            else if (card == "Card2.6")
            {
                Judul.Text = "License Plate Unrecognized";
                GenerateTableHead2();
                FillTable9(card);
            }
            else if (card == "Card2.7")
            {
                Judul.Text = "Traffic Detected";
                GenerateTableHead3();
                FillTable10(card);
            }
            //Response.Write(card);

        }

        private void GenerateTableHead()
        {
            string tableHeadHtml = @"
                <tr>
                    <th>Image</th>
                    <th></th>
                    <th>Occurred At</th>
                    <th>Location</th>
                    <th>Site</th>
                    <th>Person</th>
                    <th>Status</th>
                </tr>";

            // Assign the generated HTML to the Literal control
            LiteralTableHead.Text = tableHeadHtml;
        }
        private void GenerateTableHead2()
        {
            string tableHeadHtml = @"
                <tr>
                    <th>Image</th>
                    <th></th>
                    <th>Occurred At</th>
                    <th>Location</th>
                    <th>Site</th>
                    <th>Plate Number</th>
                    <th>Status</th>
                </tr>";

            // Assign the generated HTML to the Literal control
            LiteralTableHead.Text = tableHeadHtml;
        }

        private void GenerateTableHead3()
        {
            string tableHeadHtml = @"
                <tr>
                    <th>Image</th>
                    <th>Occurred At</th>
                    <th>Location</th>
                    <th>Site</th>
                </tr>";

            // Assign the generated HTML to the Literal control
            LiteralTableHead.Text = tableHeadHtml;
        }
        private void FillTable(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                if (card == "Card1")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" ORDER BY ce.occurred_at ASC";
                    //Response.Write(query);
                }
                else if (card == "Card2")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND p.type like '%blacklist%'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" ORDER BY ce.occurred_at ASC";

                }
                else if (card == "Card5")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND ph.entry_code='0'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @"  GROUP BY p.identification_number ORDER BY ce.occurred_at ASC";
                }
                else if (card == "Card6")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND ph.entry_code!='0'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" ORDER BY ce.occurred_at ASC";
                }
                //Response.Write(query);
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string person_image = dr["person_image"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();

                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                if (!string.IsNullOrEmpty(person_image))
                                {
                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "person_image/" + person_image,
                                        AlternateText = "icon title"
                                    };
                                    img2.Style.Add("width", "150px");
                                    imageCell2.Controls.Add(img2);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                }
                                else
                                {
                                    row.Cells.Add(imageCell);
                                    imageCell2.Text = " ";
                                    row.Cells.Add(imageCell2);
                                }
                                //pers.Text = person;
                                if (!string.IsNullOrEmpty(person_image))
                                {
                                    string warna = "";
                                    string entry = "";
                                    if (person_type == "blacklist")
                                    {
                                        warna += "red";
                                    }
                                    else
                                    {
                                        if (entry_code == "0")
                                        {
                                            warna += "green";
                                            entry += "Comply";
                                        }
                                        else if (entry_code == "1")
                                        {
                                            warna += "orange";
                                            entry += "Not Comply or Badge Expired";
                                        }
                                        else if (entry_code == "2")
                                        {
                                            warna += "red";
                                            entry += "FTW Rejected Medical / Expired";
                                        }
                                        else if (entry_code == "3")
                                        {
                                            warna += "#9f1239";
                                            entry += "Daily Checkup Failed";
                                        }
                                    }
                                    pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry}</span>";
                                }
                                else
                                {
                                    pers.Text = person;
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitecell = new TableCell() { Text = site };
                                TableCell plate = new TableCell() { Text = "" };
                                //TableCell status = new TableCell() { Text = event_type };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitecell);
                                row.Cells.Add(pers);
                                //row.Cells.Add(plate);
                                //row.Cells.Add(status);
                                if (event_type == "Face Recognition Match")
                                {
                                    TableCell status = new TableCell() { Text = "Face Recognized" };
                                    row.Cells.Add(status);
                                }
                                else if (event_type == "Face Recognition Not Match")
                                {
                                    TableCell status = new TableCell() { Text = "Face Not Registered" };
                                    row.Cells.Add(status);
                                }
                                else if (event_type == "Plate Match")
                                {
                                    TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                    row.Cells.Add(status);
                                }
                                else if (event_type == "Plate MisMatch")
                                {
                                    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                    row.Cells.Add(status);

                                }



                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }


            }
        }

        private void FillTable2(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            string first_date = today.Substring(0, 4) + "-" + today.Substring(5, 2) + "-" + "01";
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                if (card == "Card3")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" ORDER BY ce.occurred_at ASC";
                }
                else if (card == "Card4")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND p.type='blacklist'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @"  ORDER BY ce.occurred_at ASC";

                }
                else if (card == "Card7")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' and ph.entry_code='0'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" GROUP BY p.identification_number ORDER BY ce.occurred_at ASC";
                }
                else if (card == "Card8")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' and ph.entry_code!='0'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" ORDER BY ce.occurred_at ASC";
                }
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tgl", first_date);
                    cmd.Parameters.AddWithValue("@today", today);
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string person_image = dr["person_image"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();

                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                if (!string.IsNullOrEmpty(person_image))
                                {
                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "person_image/" + person_image,
                                        AlternateText = "icon title"
                                    };
                                    img2.Style.Add("width", "150px");
                                    imageCell2.Controls.Add(img2);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                }
                                else
                                {
                                    row.Cells.Add(imageCell);
                                    imageCell2.Text = " ";
                                    row.Cells.Add(imageCell2);
                                }
                                //pers.Text = person;
                                if (!string.IsNullOrEmpty(person_image))
                                {
                                    string warna = "";
                                    string entry = "";
                                    if (person_type == "blacklist")
                                    {
                                        warna += "red";
                                    }
                                    else
                                    {
                                        if (entry_code == "0")
                                        {
                                            warna += "green";
                                            entry += "Comply";
                                        }
                                        else if (entry_code == "1")
                                        {
                                            warna += "orange";
                                            entry += "Not Comply or Badge Expired";
                                        }
                                        else if (entry_code == "2")
                                        {
                                            warna += "red";
                                            entry += "FTW Rejected Medical / Expired";
                                        }
                                        else if (entry_code == "3")
                                        {
                                            warna += "#9f1239";
                                            entry += "Daily Checkup Failed";
                                        }
                                    }
                                    pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry}</span>";
                                }
                                else
                                {
                                    pers.Text = person;
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitescell = new TableCell() { Text = site };
                                TableCell plate = new TableCell() { Text = "" };
                                //TableCell status = new TableCell() { Text = event_type };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitescell);
                                row.Cells.Add(pers);
                                //row.Cells.Add(plate);
                                //row.Cells.Add(status);
                                if (event_type == "Face Recognition Match")
                                {
                                    TableCell status = new TableCell() { Text = "Face Recognized" };
                                    row.Cells.Add(status);
                                }
                                else if (event_type == "Face Recognition Not Match")
                                {
                                    TableCell status = new TableCell() { Text = "Face Not Registered" };
                                    row.Cells.Add(status);
                                }
                                else if (event_type == "Plate Match")
                                {
                                    TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                    row.Cells.Add(status);
                                }
                                else if (event_type == "Plate MisMatch")
                                {
                                    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                    row.Cells.Add(status);

                                }



                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }


            }
        }

        private void FillTable3(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                string query2 = "";
                if (card == "Card9")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND ce.plate_number!=''";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += " GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";

                    query2 += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND (ce.plate_number='' OR ce.plate_number=NULL or ce.plate_number IS NULL)";
                    query2 += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query2 += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query2 += @" AND ce.occurred_at<=@end_date";

                    }
                    query2 += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query2 += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query2 += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query2 += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query2 += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query2 += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query2 += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query2 += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query2 += ")";
                        }
                    }
                    query2 += " ORDER BY ce.occurred_at ASC";
                }
                else if (card == "Card13")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND vh.entry_code='0'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += " GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";

                }
                else if (card == "Card14")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND vh.entry_code='1'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += " GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";

                }
                else if (card == "Card17")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND vh.entry_code='2'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += "  GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";
                }
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string plate_number = dr["plate_number"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                string statusnya = "";
                                if (entry_code == "0")
                                {
                                    statusnya = "Comply";
                                }
                                else if (entry_code == "1")
                                {
                                    statusnya = "Not Approved";
                                }
                                else if (entry_code == "2")
                                {
                                    statusnya = "";
                                }
                                else
                                {
                                    statusnya = "Unrecognized";
                                }
                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                if (!string.IsNullOrEmpty(plate_number_file))
                                {
                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + plate_number_file,
                                        AlternateText = "icon title"
                                    };
                                    img2.Style.Add("width", "150px");
                                    imageCell2.Controls.Add(img2);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                }
                                else
                                {
                                    row.Cells.Add(imageCell);
                                    imageCell2.Text = " ";
                                    row.Cells.Add(imageCell2);
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitecell = new TableCell() { Text = site };
                                TableCell plate = new TableCell() { Text = plate_number };
                                TableCell status = new TableCell() { Text = statusnya };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitecell);
                                //row.Cells.Add(pers);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);
                                //if (event_type == "Face Recognition Match")
                                //{
                                //    TableCell status = new TableCell() { Text = "Face Recognized" };
                                //    row.Cells.Add(status);
                                //}
                                //else if (event_type == "Face Recognition Not Match")
                                //{
                                //    TableCell status = new TableCell() { Text = "Face Not Registered" };
                                //    row.Cells.Add(status);
                                //}
                                //else if (event_type == "Plate Match")
                                //{
                                //    TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                //    row.Cells.Add(status);
                                //}
                                //else if (event_type == "Plate MisMatch")
                                //{
                                //    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                //    row.Cells.Add(status);

                                //}



                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }

                if (card == "Card9")
                {
                    using (MySqlCommand cmd = new MySqlCommand(query2, con))
                    {
                        cmd.Parameters.AddWithValue("@today", today);
                        if (start_date != "" && start_date != null)
                        {
                            cmd.Parameters.AddWithValue("@start_date", start_date);
                        }
                        if (end_date != "" && end_date != null)
                        {
                            cmd.Parameters.AddWithValue("@end_date", end_date);

                        }
                        if (sites != "" && sites != null)
                        {
                            sites = sites.TrimEnd(',');
                            string[] siteArray = sites.Split(',');
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                                }
                                y++;
                            }
                        }
                        if (cameras != "" && cameras != null)
                        {
                            cameras = cameras.TrimEnd(',');
                            string[] cameraArray = cameras.Split(',');
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                        }
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                    string image_file = dr["image_file"].ToString();
                                    string plate_number_file = dr["plate_number_file"].ToString();
                                    DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                    string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    string plate_number = dr["plate_number"].ToString();
                                    string location = dr["location"].ToString();
                                    string event_type = dr["event_type"].ToString();
                                    string site = dr["site"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
                                    TableRow row = new TableRow();

                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();
                                    string statusnya = "";
                                    if (entry_code == "0")
                                    {
                                        statusnya = "Comply";
                                    }
                                    else if (entry_code == "1")
                                    {
                                        statusnya = "Not Approved";
                                    }
                                    else if (entry_code == "2")
                                    {
                                        statusnya = "";
                                    }
                                    else
                                    {
                                        statusnya = "Unrecognized";
                                    }
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                    }
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + plate_number_file,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    else
                                    {
                                        row.Cells.Add(imageCell);
                                        imageCell2.Text = " ";
                                        row.Cells.Add(imageCell2);
                                    }

                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sitecell = new TableCell() { Text = site };
                                    TableCell plate = new TableCell() { Text = plate_number };
                                    TableCell status = new TableCell() { Text = statusnya };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sitecell);
                                    //row.Cells.Add(pers);
                                    row.Cells.Add(plate);
                                    row.Cells.Add(status);
                                    //if (event_type == "Face Recognition Match")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "Face Recognized" };
                                    //    row.Cells.Add(status);
                                    //}
                                    //else if (event_type == "Face Recognition Not Match")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "Face Not Registered" };
                                    //    row.Cells.Add(status);
                                    //}
                                    //else if (event_type == "Plate Match")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                    //    row.Cells.Add(status);
                                    //}
                                    //else if (event_type == "Plate MisMatch")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                    //    row.Cells.Add(status);

                                    //}



                                    TableBody.Controls.Add(row);

                                }
                                dr.Close();
                            }
                            else
                            {
                            }
                        }
                    }
                }


            }
        }

        private void FillTable4(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                if (card == "Card10")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Intrusion%'";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += " ORDER BY ce.occurred_at ASC";
                }
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string location = dr["location"].ToString();
                                string site = dr["site"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();

                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                    row.Cells.Add(imageCell);
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitecell = new TableCell() { Text = site };
                                //TableCell status = new TableCell() { Text = event_type };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitecell);




                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }


            }
        }
        private void FillTable5(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            string first_date = today.Substring(0, 4) + "-" + today.Substring(5, 2) + "-" + "01";
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                string query2 = "";
                if (card == "Card11")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND (ce.plate_number!='' OR ce.plate_number!=NULL)";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";

                    query2 += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND (ce.plate_number='' OR ce.plate_number=NULL or ce.plate_number IS NULL)";
                    query2 += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query2 += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query2 += @" AND ce.occurred_at<=@end_date";

                    }
                    query2 += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query2 += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query2 += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query2 += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query2 += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query2 += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query2 += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query2 += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query2 += ")";
                        }
                    }
                    query2 += @"  ORDER BY ce.occurred_at ASC";
                }
                else if (card == "Card15")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND vh.entry_code='0'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";

                }
                else if (card == "Card16")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND vh.entry_code='1'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";
                }
                else if (card == "Card19")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' AND vh.entry_code='2'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";

                }
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tgl", first_date);
                    cmd.Parameters.AddWithValue("@today", today);
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");

                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string plate_number = dr["plate_number"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                TableRow row = new TableRow();
                                string statusnya = "";
                                if (entry_code == "0")
                                {
                                    statusnya = "Comply";
                                }
                                else if (entry_code == "1")
                                {
                                    statusnya = "Not Approved";
                                }
                                else if (entry_code == "2")
                                {
                                    statusnya = "";
                                }
                                else
                                {
                                    statusnya = "Unrecognized";
                                }
                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();

                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                if (!string.IsNullOrEmpty(plate_number_file))
                                {
                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + plate_number_file,
                                        AlternateText = "icon title"
                                    };
                                    img2.Style.Add("width", "150px");
                                    imageCell2.Controls.Add(img2);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                }
                                else
                                {
                                    row.Cells.Add(imageCell);
                                    imageCell2.Text = " ";
                                    row.Cells.Add(imageCell2);
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitescell = new TableCell() { Text = site };
                                TableCell plate = new TableCell() { Text = plate_number };
                                TableCell status = new TableCell() { Text = statusnya };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitescell);
                                //row.Cells.Add(pers);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);
                                //if (event_type == "Face Recognition Match")
                                //{
                                //    TableCell status = new TableCell() { Text = "Face Recognized" };
                                //    row.Cells.Add(status);
                                //}
                                //else if (event_type == "Face Recognition Not Match")
                                //{
                                //    TableCell status = new TableCell() { Text = "Face Not Registered" };
                                //    row.Cells.Add(status);
                                //}
                                //else if (event_type == "Plate Match")
                                //{
                                //    TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                //    row.Cells.Add(status);
                                //}
                                //else if (event_type == "Plate MisMatch")
                                //{
                                //    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                //    row.Cells.Add(status);

                                //}



                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }

                if (card == "Card11")
                {
                    using (MySqlCommand cmd = new MySqlCommand(query2, con))
                    {
                        cmd.Parameters.AddWithValue("@tgl", first_date);
                        cmd.Parameters.AddWithValue("@today", today);
                        if (start_date != "" && start_date != null)
                        {
                            cmd.Parameters.AddWithValue("@start_date", start_date);
                        }
                        if (end_date != "" && end_date != null)
                        {
                            cmd.Parameters.AddWithValue("@end_date", end_date);

                        }
                        if (sites != "" && sites != null)
                        {
                            sites = sites.TrimEnd(',');
                            string[] siteArray = sites.Split(',');
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                                }
                                y++;
                            }
                        }
                        if (cameras != "" && cameras != null)
                        {
                            cameras = cameras.TrimEnd(',');
                            string[] cameraArray = cameras.Split(',');
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                        }
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");

                                    string image_file = dr["image_file"].ToString();
                                    string plate_number_file = dr["plate_number_file"].ToString();
                                    DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                    string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    string plate_number = dr["plate_number"].ToString();
                                    string location = dr["location"].ToString();
                                    string event_type = dr["event_type"].ToString();
                                    string site = dr["site"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
                                    TableRow row = new TableRow();
                                    string statusnya = "";
                                    if (entry_code == "0")
                                    {
                                        statusnya = "Comply";
                                    }
                                    else if (entry_code == "1")
                                    {
                                        statusnya = "Not Approved";
                                    }
                                    else if (entry_code == "2")
                                    {
                                        statusnya = "";
                                    }
                                    else
                                    {
                                        statusnya = "Unrecognized";
                                    }
                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();

                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                    }
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + plate_number_file,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    else
                                    {
                                        row.Cells.Add(imageCell);
                                        imageCell2.Text = " ";
                                        row.Cells.Add(imageCell2);
                                    }

                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sitescell = new TableCell() { Text = site };
                                    TableCell plate = new TableCell() { Text = plate_number };
                                    TableCell status = new TableCell() { Text = statusnya };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sitescell);
                                    //row.Cells.Add(pers);
                                    row.Cells.Add(plate);
                                    row.Cells.Add(status);
                                    //if (event_type == "Face Recognition Match")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "Face Recognized" };
                                    //    row.Cells.Add(status);
                                    //}
                                    //else if (event_type == "Face Recognition Not Match")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "Face Not Registered" };
                                    //    row.Cells.Add(status);
                                    //}
                                    //else if (event_type == "Plate Match")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                    //    row.Cells.Add(status);
                                    //}
                                    //else if (event_type == "Plate MisMatch")
                                    //{
                                    //    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                    //    row.Cells.Add(status);

                                    //}



                                    TableBody.Controls.Add(row);

                                }
                                dr.Close();
                            }
                            else
                            {
                            }
                        }
                    }
                }


            }
        }

        private void FillTable6(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            string first_date = today.Substring(0, 4) + "-" + today.Substring(5, 2) + "-" + "01";
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                if (card == "Card12")
                {
                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Intrusion%'";
                    query += @" AND (DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query += @" ORDER BY ce.occurred_at ASC";
                }
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tgl", first_date);
                    cmd.Parameters.AddWithValue("@today", today);
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string location = dr["location"].ToString();
                                string site = dr["site"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();

                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                    row.Cells.Add(imageCell);
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitescell = new TableCell() { Text = site };
                                //TableCell status = new TableCell() { Text = event_type };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitescell);




                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }


            }
        }

        private void FillTable7(string card, string start_date, string end_date, string sites, string cameras)
        {
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            string first_date = today.Substring(0, 4) + "-" + today.Substring(5, 2) + "-" + "01";
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                string query2 = "";
                query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' and ce.plate_number not in (SELECT plate_number from vehicles) and (ce.plate_number!='' or ce.plate_number!=null)";
                query2 += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Plate%' and (ce.plate_number='' or ce.plate_number=null or ce.plate_number IS NULL)";
                if (card == "Card18")
                {
                    //query += " AND DATE(ce.occurred_at)=@today";
                    query += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }

                    //query2 += " AND DATE(ce.occurred_at)=@today";
                    query2 += @" AND ( DATE(ce.occurred_at)=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query2 += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query2 += @" AND ce.occurred_at<=@end_date";

                    }
                    query2 += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query2 += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query2 += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query2 += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query2 += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query2 += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query2 += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query2 += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query2 += ")";
                        }
                    }
                }
                else if (card == "Card20")
                {
                    //query += " AND DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    query += @" AND ( DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                    if (start_date != "" && start_date != null)
                    {
                        query += @" AND ce.occurred_at>=@start_date";
                    }
                    if (end_date != "" && end_date != null)
                    {
                        query += @" AND ce.occurred_at<=@end_date";

                    }
                    query += ")";
                    if (sites != "" && sites != null && sites != "all,")
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        if (siteArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            //Response.Write(siteArray[0]);
                            int y = 0;
                            foreach (string site in siteArray)
                            {
                                string siteId = site;
                                if (siteId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"cs.id=@siteId{y}";
                                    }
                                    else
                                    {
                                        query += $" or cs.id=@siteId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    if (cameras != "" && cameras != null && sites != "all,")
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        if (cameraArray[0] == "all") { }
                        else
                        {
                            query += " AND (";
                            int y = 0;
                            foreach (string cam in cameraArray)
                            {
                                string cameraId = cam;
                                if (cameraId == "all")
                                {
                                }
                                else
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@cameraId{y}";
                                    }
                                    else
                                    {
                                        query += $" or c.id=@cameraId{y}";

                                    }
                                }
                                //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                                y++;
                            }
                            query += ")";
                        }
                    }
                    query2 += " AND DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                }
                query += " GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";
                //query2 += " ORDER BY ce.occurred_at ASC";
                query2 += @" AND ( DATE(ce.occurred_at)>=@tgl AND DATE(ce.occurred_at)<=@today";
                if (start_date != "" && start_date != null)
                {
                    query2 += @" AND ce.occurred_at>=@start_date";
                }
                if (end_date != "" && end_date != null)
                {
                    query2 += @" AND ce.occurred_at<=@end_date";

                }
                query2 += ")";
                if (sites != "" && sites != null && sites != "all,")
                {
                    sites = sites.TrimEnd(',');
                    string[] siteArray = sites.Split(',');
                    if (siteArray[0] == "all") { }
                    else
                    {
                        query2 += " AND (";
                        //Response.Write(siteArray[0]);
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                if (y == 0)
                                {
                                    query2 += $"cs.id=@siteId{y}";
                                }
                                else
                                {
                                    query2 += $" or cs.id=@siteId{y}";

                                }
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                        query2 += ")";
                    }
                }
                if (cameras != "" && cameras != null && sites != "all,")
                {
                    cameras = cameras.TrimEnd(',');
                    string[] cameraArray = cameras.Split(',');
                    if (cameraArray[0] == "all") { }
                    else
                    {
                        query2 += " AND (";
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                if (y == 0)
                                {
                                    query2 += $"c.id=@cameraId{y}";
                                }
                                else
                                {
                                    query2 += $" or c.id=@cameraId{y}";

                                }
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                        query2 += ")";
                    }
                }
                //Response.Write(query);
                //Response.Write(query2);
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    //if (card == "Card20")
                    //{
                    cmd.Parameters.AddWithValue("@tgl", first_date);
                    //}
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string plate_number = dr["plate_number"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                string statusnya = "";
                                statusnya = "Unrecognized";
                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                if (!string.IsNullOrEmpty(plate_number_file))
                                {
                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + plate_number_file,
                                        AlternateText = "icon title"
                                    };
                                    img2.Style.Add("width", "150px");
                                    imageCell2.Controls.Add(img2);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                }
                                else
                                {
                                    row.Cells.Add(imageCell);
                                    imageCell2.Text = " ";
                                    row.Cells.Add(imageCell2);
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitescell = new TableCell() { Text = site };
                                TableCell plate = new TableCell() { Text = plate_number };
                                TableCell status = new TableCell() { Text = statusnya };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitescell);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);
                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd = new MySqlCommand(query2, con))
                {
                    cmd.Parameters.AddWithValue("@today", today);
                    //if (card == "Card20")
                    //{
                    cmd.Parameters.AddWithValue("@tgl", first_date);
                    //}
                    if (start_date != "" && start_date != null)
                    {
                        cmd.Parameters.AddWithValue("@start_date", start_date);
                    }
                    if (end_date != "" && end_date != null)
                    {
                        cmd.Parameters.AddWithValue("@end_date", end_date);

                    }
                    if (sites != "" && sites != null)
                    {
                        sites = sites.TrimEnd(',');
                        string[] siteArray = sites.Split(',');
                        int y = 0;
                        foreach (string site in siteArray)
                        {
                            string siteId = site;
                            if (siteId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@siteId{y}", siteId);
                            }
                            y++;
                        }
                    }
                    if (cameras != "" && cameras != null)
                    {
                        cameras = cameras.TrimEnd(',');
                        string[] cameraArray = cameras.Split(',');
                        int y = 0;
                        foreach (string cam in cameraArray)
                        {
                            string cameraId = cam;
                            if (cameraId == "all")
                            {
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@cameraId{y}", cameraId);
                            }
                            //Response.Write("Site ID " + y + ": " + siteId + "<br>");
                            y++;
                        }
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string plate_number = dr["plate_number"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                string statusnya = "";
                                statusnya = "Unrecognized";
                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                if (!string.IsNullOrEmpty(plate_number_file))
                                {
                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = "image_file/" + plate_number_file,
                                        AlternateText = "icon title"
                                    };
                                    img2.Style.Add("width", "150px");
                                    imageCell2.Controls.Add(img2);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                }
                                else
                                {
                                    row.Cells.Add(imageCell);
                                    imageCell2.Text = " ";
                                    row.Cells.Add(imageCell2);
                                }

                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sitescell = new TableCell() { Text = site };
                                TableCell plate = new TableCell() { Text = plate_number };
                                TableCell status = new TableCell() { Text = statusnya };

                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sitescell);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);
                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }


            }
        }

        private void FillTable8(string card)
        {
            string camera_site = Session["camera_site_event"].ToString();
            if (camera_site == "none")
            {
                Response.Write("<script>alert('Choose Camera Site first');</script>");
            }
            else
            {
                string from2 = Session["from_event_dashboard"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from2);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
                if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_event_dashboard"].ToString();
                    DateTime parsedDateTime2 = DateTime.Parse(to);
                    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    //Response.Write("<script>alert('kosong')</script>");

                }
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = "";
                    if (card == "Card2.1")
                    {
                        query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND cs.id=@camera_site";
                    }
                    else if (card == "Card2.2")
                    {
                        query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND cs.id=@camera_site AND p.type='blacklist'";
                    }
                    else if (card == "Card2.4")
                    {
                        query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type='Face Recognition Match' AND ((p.type='employee' AND ph.entry_code=0) or (p.type='contractor' AND ph.entry_code=0) or p.type='external') AND cs.id=@camera_site";
                    }
                    else if (card == "Card2.5")
                    {
                        query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Face%' AND ph.entry_code!=0 AND cs.id=@camera_site";
                    }

                    if (from_date != null)
                    {
                        query += " AND ce.occurred_at>=@from_date";
                    }
                    if (to_date != null)
                    {
                        query += " AND ce.occurred_at<=@to_date";
                    }
                    query += "  ORDER BY ce.occurred_at ASC";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        if (from_date != null)
                        {
                            cmd.Parameters.AddWithValue("@from_date", from_date);
                        }
                        if (to_date != null)
                        {
                            cmd.Parameters.AddWithValue("@to_date", to_date);
                        }
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                    string image_file = dr["image_file"].ToString();
                                    string plate_number_file = dr["plate_number_file"].ToString();
                                    DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                    string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    string person = dr["person"].ToString();
                                    string location = dr["location"].ToString();
                                    string event_type = dr["event_type"].ToString();
                                    string site = dr["site"].ToString();
                                    string person_type = dr["person_type"].ToString();
                                    string person_image = dr["person_image"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
                                    TableRow row = new TableRow();

                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();

                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                    }
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "person_image/" + person_image,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    else
                                    {
                                        row.Cells.Add(imageCell);
                                        imageCell2.Text = " ";
                                        row.Cells.Add(imageCell2);
                                    }
                                    //pers.Text = person;
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        string warna = "";
                                        string entry = "";
                                        if (person_type == "blacklist")
                                        {
                                            warna += "red";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                warna += "green";
                                                entry += "Comply";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                warna += "orange";
                                                entry += "Not Comply or Badge Expired";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                warna += "red";
                                                entry += "FTW Rejected Medical / Expired";
                                            }
                                            else if (entry_code == "3")
                                            {
                                                warna += "#9f1239";
                                                entry += "Daily Checkup Failed";
                                            }
                                        }
                                        pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry}</span>";
                                    }
                                    else
                                    {
                                        pers.Text = person;
                                    }

                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sites = new TableCell() { Text = site };
                                    TableCell plate = new TableCell() { Text = "" };
                                    //TableCell status = new TableCell() { Text = event_type };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sites);
                                    row.Cells.Add(pers);
                                    //row.Cells.Add(plate);
                                    //row.Cells.Add(status);
                                    if (event_type == "Face Recognition Match")
                                    {
                                        TableCell status = new TableCell() { Text = "Face Recognized" };
                                        row.Cells.Add(status);
                                    }
                                    else if (event_type == "Face Recognition Not Match")
                                    {
                                        TableCell status = new TableCell() { Text = "Face Not Registered" };
                                        row.Cells.Add(status);
                                    }
                                    else if (event_type == "Plate Match")
                                    {
                                        TableCell status = new TableCell() { Text = "License Plate Recognized" };
                                        row.Cells.Add(status);
                                    }
                                    else if (event_type == "Plate MisMatch")
                                    {
                                        TableCell status = new TableCell() { Text = "License Plate Not Registered" };
                                        row.Cells.Add(status);

                                    }



                                    TableBody.Controls.Add(row);

                                }
                                dr.Close();
                            }
                            else
                            {
                            }
                        }
                    }


                }
            }
        }

        private void FillTable9(string card)
        {
            string camera_site = Session["camera_site_event"].ToString();
            if (camera_site == "none")
            {
                Response.Write("<script>alert('Choose Camera Site first');</script>");
            }
            else
            {
                string from2 = Session["from_event_dashboard"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from2);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
                if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_event_dashboard"].ToString();
                    DateTime parsedDateTime2 = DateTime.Parse(to);
                    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    //Response.Write("<script>alert('kosong')</script>");

                }
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = "";
                    if (card == "Card2.3")
                    {
                        query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id = cs.id LEFT JOIN vehicle_history vh ON v.id = vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE(ce.image_file != '' or ce.image_file != NULL) AND ce.type like '%Plate%' AND cs.id=@camera_site AND ce.plate_number in (SELECT plate_number from vehicles)";
                    }
                    else if (card == "Card2.6")
                    {
                        query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id = cs.id LEFT JOIN vehicle_history vh ON v.id = vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE(ce.image_file != '' or ce.image_file != NULL) AND ce.type like '%Plate%' AND cs.id=@camera_site AND (ce.plate_number not in (SELECT plate_number from vehicles) or ce.plate_number='' or ce.plate_number=NULL )";
                    }
                    if (from_date != null)
                    {
                        query += " AND ce.occurred_at>=@from_date";
                    }
                    if (to_date != null)
                    {
                        query += " AND ce.occurred_at<=@to_date";
                    }
                    query += " ORDER BY ce.occurred_at ASC";
                    //Response.Write(query);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        if (from_date != null)
                        {
                            cmd.Parameters.AddWithValue("@from_date", from_date);
                        }
                        if (to_date != null)
                        {
                            cmd.Parameters.AddWithValue("@to_date", to_date);
                        }
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");

                                    string image_file = dr["image_file"].ToString();
                                    string plate_number_file = dr["plate_number_file"].ToString();
                                    DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                    string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    string plate_number = dr["plate_number"].ToString();
                                    string location = dr["location"].ToString();
                                    string event_type = dr["event_type"].ToString();
                                    string site = dr["site"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
                                    TableRow row = new TableRow();
                                    string statusnya = "";
                                    if (entry_code == "0")
                                    {
                                        statusnya = "Comply";
                                    }
                                    else if (entry_code == "1")
                                    {
                                        statusnya = "Not Approved";
                                    }
                                    else if (entry_code == "2")
                                    {
                                        statusnya = "";
                                    }
                                    else
                                    {
                                        statusnya = "Unrecognized";
                                    }
                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                    }
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + plate_number_file,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    else
                                    {
                                        row.Cells.Add(imageCell);
                                        imageCell2.Text = " ";
                                        row.Cells.Add(imageCell2);
                                    }
                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sites = new TableCell() { Text = site };
                                    TableCell plate = new TableCell() { Text = plate_number };
                                    TableCell status = new TableCell() { Text = statusnya };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sites);
                                    row.Cells.Add(plate);
                                    row.Cells.Add(status);

                                    TableBody.Controls.Add(row);

                                }
                                dr.Close();
                            }
                            else
                            {
                            }
                        }
                    }


                }
            }
        }

        private void FillTable10(string card)
        {
            string camera_site = Session["camera_site_event"].ToString();
            if (camera_site == "none")
            {
                Response.Write("<script>alert('Choose Camera Site first');</script>");
            }
            else
            {
                string from2 = Session["from_event_dashboard"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from2);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
                if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_event_dashboard"].ToString();
                    DateTime parsedDateTime2 = DateTime.Parse(to);
                    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    //Response.Write("<script>alert('kosong')</script>");

                }
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = "";

                    query += @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, ce.plate_number, v.plate_number as plate, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN vehicles as v ON ce.plate_number = v.plate_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like '%Intrusion%' AND cs.id=@camera_site";
                    if (from_date != null)
                    {
                        query += " AND ce.occurred_at>=@from_date";
                    }
                    if (to_date != null)
                    {
                        query += " AND ce.occurred_at<=@to_date";

                    }
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        if (from_date != null)
                        {
                            cmd.Parameters.AddWithValue("@from_date", from_date);
                        }
                        if (to_date != null)
                        {
                            cmd.Parameters.AddWithValue("@to_date", to_date);
                        }
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                    string image_file = dr["image_file"].ToString();
                                    DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                    string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    string location = dr["location"].ToString();
                                    string site = dr["site"].ToString();
                                    TableRow row = new TableRow();

                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();

                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        row.Cells.Add(imageCell);
                                    }

                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sites = new TableCell() { Text = site };
                                    //TableCell status = new TableCell() { Text = event_type };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sites);




                                    TableBody.Controls.Add(row);

                                }
                                dr.Close();
                            }
                            else
                            {
                            }
                        }
                    }


                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string card = Request.QueryString["card"];
            HtmlSelect selectMultiple = SelectMultiple;
            var i = 0;
            string location = "";
            if (selectMultiple != null)
            {
                //List<string> selectedValues = new List<string>();

                foreach (ListItem item in selectMultiple.Items)
                {
                    if (item.Selected)
                    {
                        //selectedValues.Add(item.Value);
                        //Session["camera_people" + i] = item.Value;
                        location += item.Value + ",";
                        i++;
                        if (i == 1 && item.Value == "all")
                        {
                            break;
                        }
                    }
                }
            }
            HtmlSelect select1 = Select1;
            var x = 0;
            string sites = "";
            if (select1 != null)
            {
                //List<string> selectedValues = new List<string>();

                foreach (ListItem item2 in select1.Items)
                {
                    if (item2.Selected)
                    {
                        //selectedValues.Add(item.Value);
                        //Session["camera_people" + i] = item.Value;
                        sites += item2.Value + ",";
                        x++;
                        if (x == 1 && item2.Value == "all")
                        {
                            break;
                        }
                        else
                        {
                        }
                    }
                }
            }
            string from = TextBox1.Text;
            string to = TextBox2.Text;

            string currentUrl = Request.Url.AbsolutePath;
            string newUrl = currentUrl + "?card=" + card + "&start_date=" + from + "&end_date=" + to + "&sites=" + sites + "&location=" + location;
            Response.Redirect(newUrl);


            // Menghapus koma terakhir jika ada
            //if (sites != "")
            //{
            //    sites = sites.TrimEnd(',');
            //    string[] siteArray = sites.Split(',');
            //    int y = 0;
            //    foreach (string site in siteArray)
            //    {
            //        string siteId = site;
            //        //int siteId = int.Parse(site);
            //        Response.Write("Site ID " + y + ": " + siteId + "<br>");
            //        y++;
            //    }
            //}
            //else
            //{
            //    Response.Write("Ga ada");
            //}


            //string dari = TextBox1.Text;
            //DateTime fromDate;
            //if (DateTime.TryParse(dari.ToString(), out fromDate))
            //{
            //}
            //else
            //{
            //    string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //    dari = today;

            //}
            //string ke = TextBox2.Text;
            //Session["from_people"] = dari;
            //Session["to_people"] = ke;
            //Response.Redirect("report_people.aspx");
        }

    }
}