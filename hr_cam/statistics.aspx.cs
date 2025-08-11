using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
//using iText.Layout.Element;


namespace hr_cam
{
    public partial class statistics : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    Response.Write("User is authenticated");
            //}
            //else
            //{
            //    Response.Write("User is not authenticated");
            //}

            TextBox1.TextMode = TextBoxMode.DateTimeLocal;
            TextBox2.TextMode = TextBoxMode.DateTimeLocal;
            //TextBox3.TextMode = TextBoxMode.Date;
            //TextBox4.TextMode = TextBoxMode.Date;
            if (IsPostBack && Request["__EVENTTARGET"] == "SetColumnSession")
            {
                string[] values = Request["__EVENTARGUMENT"].Split(',');

                // Simpan data dalam session sesuai kolom yang diklik
                string camera = values[0];
                string filter = values[1];
                Response.Write(camera);
                Response.Write(filter);

                // Simpan nilai kolom yang diklik ke session
                Session["lokasi"] = camera;
                Session["type_event"] = filter;

                // Lakukan sesuatu dengan data ini, misalnya redirect atau update UI
                Response.Redirect("statistics.aspx");
            }
            if (!IsPostBack)
            {
                //Response.Write("<script>alert('camera " + Session["lokasi"].ToString() + " kolomnya " + Session["type_event"].ToString() + "')</script>");

                //Response.Write("<script>alert('Coba masuk')</script>");
                if (Session["camera_site_event"] == null)
                {
                    string camera_site = "none";
                    Session["camera_site_event"] = camera_site;
                    Label1.Text = "0";
                    Label2.Text = "0";
                    Label3.Text = "0";
                    Label4.Text = "0";
                    Label5.Text = "0";
                    Label6.Text = "0";
                    Label7.Text = "0";
                    using (MySqlConnection con = new MySqlConnection(strcon))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_sites order by id ASC limit 1", con))
                        {
                            cmd.Prepare();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {

                                        Session["camera_site_event"] = dr["id"];
                                    }
                                }
                                else
                                {
                                }
                            }
                        }

                    }
                    //Response.Write("<script>alert('" + Session["camera_site_event"].ToString() + "')</script>");

                }
                if (Session["camera_site_event"].ToString() != "none")
                {
                    DropDownList2.Items.Add(new ListItem("", "none"));
                    string sitenya = Session["camera_site_event"].ToString();
                    using (MySqlConnection con = new MySqlConnection(strcon))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site", con))
                        {
                            cmd.Parameters.AddWithValue("@camera_site", sitenya);
                            cmd.Prepare();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        DropDownList2.Items.Add(new ListItem(dr["location"].ToString(), dr["name"].ToString()));
                                        //DropDownList2.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
                                    }
                                }
                                else
                                {
                                }
                            }
                        }

                    }
                }

                if (Session["camera_site2"] == null)
                {
                    string camera_site2 = "none";
                    Session["camera_site2"] = camera_site2;
                    //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

                }
                if (Session["camera"] == null)
                {
                    string camera = "none";
                    Session["camera"] = camera;
                    //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

                }
                if (Session["camera_site2"].ToString() == "none")
                {
                    //using (MySqlConnection con = new MySqlConnection(strcon))
                    //{
                    //    if (con.State == ConnectionState.Closed)
                    //    {
                    //        con.Open();
                    //    }
                    //    using (MySqlCommand cmd2 = new MySqlCommand("SELECT * from cameras", con))
                    //    {
                    //        cmd2.Prepare();
                    //        using (MySqlDataReader dr2 = cmd2.ExecuteReader())
                    //        {
                    //            if (dr2.HasRows)
                    //            {
                    //                DropDownList3.Items.Clear();
                    //                DropDownList3.Items.Add(new ListItem("", "none"));
                    //                while (dr2.Read())
                    //                {
                    //                    DropDownList3.Items.Add(new ListItem(dr2.GetValue(1).ToString(), dr2.GetValue(0).ToString()));
                    //                }
                    //            }
                    //            else
                    //            {
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    //string camera_site_id = Session["camera_site2"].ToString();
                    using (MySqlConnection con = new MySqlConnection(strcon))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        //using (MySqlCommand cmd2 = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site", con))
                        //{
                        //    cmd2.Parameters.AddWithValue("@camera_site", camera_site_id);
                        //    cmd2.Prepare();
                        //    using (MySqlDataReader dr2 = cmd2.ExecuteReader())
                        //    {
                        //        if (dr2.HasRows)
                        //        {
                        //            DropDownList3.Items.Clear();
                        //            DropDownList3.Items.Add(new ListItem("", "none"));
                        //            while (dr2.Read())
                        //            {
                        //                DropDownList3.Items.Add(new ListItem(dr2.GetValue(1).ToString(), dr2.GetValue(0).ToString()));
                        //            }
                        //        }
                        //        else
                        //        {
                        //        }
                        //    }
                        //}
                    }
                }

                if (Session["from_event_dashboard"] != null)
                {
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                    Session["from_event_dashboard"] = today;

                }
                if (Session["to_event_dashboard"] != null)
                {
                    TextBox2.Text = Session["to_event_dashboard"].ToString();
                }
                //if (Session["from_event_dashboard"] != null)
                //{
                //}
                //else
                //{
                //    string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //    Session["from_event_dashboard"] = today;

                //}
                //if (Session["to2"] != null)
                //{
                //    //TextBox4.Text = Session["to2"].ToString();
                //}
                TextBox1.Text = Session["from_event_dashboard"].ToString();
                //TextBox3.Text = Session["from2"].ToString();
                DropDownList1.Items.Add(new ListItem("", "none"));
                //DropDownList2.Items.Add(new ListItem("", "none"));
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
                                while (dr.Read())
                                {
                                    DropDownList1.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
                                    //DropDownList2.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                }
                DropDownList1.SelectedValue = Session["camera_site_event"].ToString();
                DropDownList3.Items.Add(new ListItem("", "none"));
                DropDownList3.Items.Add(new ListItem("Face Recognized", "Face Recognition Match"));
                DropDownList3.Items.Add(new ListItem("Face Not Registered", "Face Recognition Not Match"));
                DropDownList3.Items.Add(new ListItem("License Plate Recognized", "Plate Match"));
                DropDownList3.Items.Add(new ListItem("License Plate Not Registered", "Plate MisMatch"));
                DropDownList3.Items.Add(new ListItem("Traffic Detected", "Intrusion Detection"));
                //DropDownList2.SelectedValue = Session["camera_site2"].ToString();
                //DropDownList3.SelectedValue = Session["camera"].ToString();
                FillCard();
                if (Session["person_name"] != null && !string.IsNullOrEmpty(Session["person_name"].ToString()))
                {
                    TextBox3.Text = Session["person_name"].ToString();
                }
                if (Session["lokasi"] != null && !string.IsNullOrEmpty(Session["lokasi"].ToString()))
                {
                    // Kirimkan JavaScript ke halaman untuk melakukan scroll ke elemen dengan ID 'fokusEvent'
                    ClientScript.RegisterStartupScript(this.GetType(), "ScrollToElement", "scrollToElement();", true);
                    DropDownList2.SelectedValue = Session["lokasi"].ToString();
                    DropDownList3.SelectedValue = Session["type_event"].ToString();
                    Session["lokasi"] = Session["lokasi"].ToString(); ;
                    Session["type_event"] = Session["type_event"].ToString();
                    //Response.Write(Session["lokasi"].ToString());
                    //Response.Write(Session["type_event"].ToString());
                    //if (Session["type_event"] != null && !string.IsNullOrEmpty(Session["type_event"].ToString()))
                    //{
                    //    string kolom = Session["type_event"].ToString();
                    //    DropDownList3.SelectedValue = kolom;
                    //}
                }
                else
                {
                    DropDownList2.SelectedValue = "none";
                }
                FillEventHistoryDPO();
                FillEventHistory();
                //FillChart1();
                //FillChart2();
                FillChart3();

            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string camera_site = DropDownList1.SelectedValue;

            //string dari = TextBox1.Text;
            //DateTime fromDate;
            //if (DateTime.TryParse(dari.ToString(), out fromDate))
            //{
            //    //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            //}
            //else
            //{
            //    string today = DateTime.Now.ToString("yyyy-MM-dd");
            //    dari = today;

            //}
            string dari = TextBox1.Text;
            if (!DateTime.TryParse(dari, out _))  // Gunakan out _ untuk mengabaikan nilai keluaran
            {
                dari = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            Session["camera_site_event"] = camera_site;
            Session["from_event_dashboard"] = dari;
            Session["to_event_dashboard"] = ke;
            Response.Redirect("statistics.aspx");
        }


        private void FillCard()
        {
            DateTime fromDate = DateTime.Now;
            string from = fromDate.ToString("yyyy-MM-dd");
            string camera = Session["camera_site_event"].ToString();
            //int jml = 0;
            int jml2 = 0;
            int jml3 = 0;
            //int jml4 = 0;
            //int jml5 = 0;
            //int jml6 = 0;
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

                //using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(ce.person_identification_number) AS jumlah FROM camera_events ce JOIN cameras c ON ce.camera_id=c.name JOIN persons p ON ce.person_identification_number=p.identification_number WHERE c.camera_site_id=@camera AND DATE(ce.occurred_at)=@from GROUP BY p.identification_number", con))

                //using (MySqlCommand cmd2 = new MySqlCommand("SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.person_identification_number IS NOT NULL AND ph.entry_code!=@entry AND c.camera_id IN (SELECT name FROM cameras WHERE camera_site_id=@camera) AND DATE(c.occurred_at)=@from GROUP BY c.person_identification_number;", con))
                //{
                //    cmd2.Parameters.AddWithValue("@entry", "0");
                //    cmd2.Parameters.AddWithValue("@camera", camera);
                //    cmd2.Parameters.AddWithValue("@from", from);
                //    cmd2.Prepare();
                //    using (MySqlDataReader dr2 = cmd2.ExecuteReader())
                //    {
                //        if (dr2.HasRows)
                //        {
                //            while (dr2.Read())
                //            {
                //                jml2++;
                //            }
                //            //Response.Write("jumlah2" + jml2);
                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                //using (MySqlCommand cmd2 = new MySqlCommand("SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type FROM camera_events c JOIN persons p ON c.person_identification_number = p.identification_number WHERE c.person_identification_number IS NOT NULL AND p.type='blacklist' AND c.camera_id IN (SELECT name FROM cameras WHERE camera_site_id=@camera) AND DATE(c.occurred_at)=@from GROUP BY c.person_identification_number;", con))

                string query_c1 = @"SELECT COUNT(ce.person_identification_number) AS jumlah FROM camera_events ce JOIN cameras c ON ce.camera_id=c.name JOIN persons p ON ce.person_identification_number=p.identification_number WHERE c.camera_site_id=@camera";
                string query_c2 = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type FROM camera_events c JOIN persons p ON c.person_identification_number = p.identification_number WHERE c.person_identification_number IS NOT NULL AND p.type='blacklist' AND c.camera_id IN (SELECT name FROM cameras WHERE camera_site_id=@camera)";
                string query = @"SELECT * FROM camera_events WHERE (image_file != '' OR image_file != NULL) AND camera_id in (SELECT name from cameras where camera_site_id = @camera) AND type!='Face Recognition Match' AND type!='Plate Match'";
                //string query2 = @"SELECT * FROM camera_events WHERE (image_file != '' OR image_file != NULL) AND camera_id in (SELECT name from cameras where camera_site_id = @camera) AND type='Face Recognition Match'";
                //string query2 = @"SELECT count(*) as jumlah FROM camera_events WHERE (image_file != '' OR image_file != NULL) AND camera_id in (SELECT name from cameras where camera_site_id = @camera) AND type='Face Recognition Match'";
                string query2 = @"SELECT count(*) as jumlah, ce.person_identification_number, ph.entry_code, p.type as person_type FROM camera_events ce join persons p on ce.person_identification_number=p.identification_number LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) AND ce.camera_id in (SELECT name from cameras where camera_site_id =@camera) AND ce.type='Face Recognition Match' AND ((p.type='employee' AND ph.entry_code=0) or (p.type='contractor' AND ph.entry_code=0) or p.type='external')";
                string query3 = @"SELECT count(*) as jumlah FROM camera_events WHERE (image_file != '' OR image_file != NULL) AND type like '%plate%' AND plate_number in (SELECT plate_number from vehicles) AND camera_id in (SELECT name from cameras where camera_site_id = @camera) AND type='Plate Match'";
                string query4 = @"SELECT count(*) as jumlah, ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ce.camera_id in (SELECT name from cameras where camera_site_id = @camera) AND ph.entry_code!=0 and ce.type like '%face%'";
                string query5 = @"SELECT count(*) as jumlah FROM camera_events WHERE (image_file != '' OR image_file != NULL) AND type like '%plate%' AND (plate_number not in (SELECT plate_number from vehicles) or plate_number='' or plate_number=NULL or plate_number IS NULL) AND camera_id in (SELECT name from cameras where camera_site_id = @camera) AND type='Plate MisMatch'";
                string query6 = @"SELECT count(*) as jumlah FROM camera_events WHERE (image_file != '' OR image_file != NULL) AND type like '%Intrusion%' AND camera_id in (SELECT name from cameras where camera_site_id = @camera)";
                if (from_date != null)
                {
                    query += @" AND occurred_at >= @from";
                    //query2 += @" AND occurred_at >= @from";
                    query2 += @" AND ce.occurred_at >= @from";
                    query3 += @" AND occurred_at >= @from";
                    query5 += @" AND occurred_at >= @from";
                    query6 += @" AND occurred_at >= @from";
                    query4 += @" AND ce.occurred_at >= @from";
                    query_c1 += @" AND ce.occurred_at >= @from";
                    query_c2 += @" AND c.occurred_at >= @from";
                }
                if (to_date != null)
                {
                    query += @" AND occurred_at <= @to";
                    //query2 += @" AND occurred_at <= @to";
                    query2 += @" AND ce.occurred_at <= @to";
                    query3 += @" AND occurred_at <= @to";
                    query5 += @" AND occurred_at <= @to";
                    query6 += @" AND occurred_at <= @to";
                    query4 += @" AND ce.occurred_at <= @to";
                    query_c1 += @" AND ce.occurred_at <= @to";
                    query_c2 += @" AND c.occurred_at <= @to";
                }
                //query2 += @" GROUP BY person_identification_number";
                //query4 += @" GROUP BY ce.person_identification_number";
                //query3 += @" GROUP BY plate_number";
                //Response.Write(query2);
                using (MySqlCommand cmd = new MySqlCommand(query_c1, con))
                {
                    cmd.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //Response.Write("<script>alert('jumlah" + dr.GetValue(0).ToString() + "')</script>");
                                //jml++;
                                Label1.Text = dr["jumlah"].ToString();
                            }
                            //Response.Write("jumlah" + jml);

                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd2 = new MySqlCommand(query_c2, con))
                {
                    cmd2.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd2.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd2.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd2.Prepare();
                    using (MySqlDataReader dr2 = cmd2.ExecuteReader())
                    {
                        if (dr2.HasRows)
                        {
                            while (dr2.Read())
                            {
                                jml2++;
                            }
                            //Response.Write("jumlah2" + jml2);
                        }
                        else
                        {
                        }
                    }
                }
                Label2.Text = jml2.ToString();

                //using (MySqlCommand cmd4 = new MySqlCommand(query2, con))
                //{
                //    cmd4.Parameters.AddWithValue("@camera", camera);
                //    if (from_date != null)
                //    {
                //        cmd4.Parameters.AddWithValue("@from", from_date);
                //    }
                //    if (to_date != null)
                //    {
                //        cmd4.Parameters.AddWithValue("@to", to_date);

                //    }
                //    cmd4.Prepare();
                //    using (MySqlDataReader dr = cmd4.ExecuteReader())
                //    {
                //        if (dr.HasRows)
                //        {
                //            while (dr.Read())
                //            {
                //                string type = dr["type"].ToString();
                //                if (type == "Face Recognition Match")
                //                {
                //                    jml3++;
                //                }
                //            }
                //            //Response.Write("jumlah" + jml);

                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                using (MySqlCommand cmd4 = new MySqlCommand(query2, con))
                {
                    cmd4.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd4.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd4.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd4.Prepare();
                    using (MySqlDataReader dr = cmd4.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string jumlah = dr["jumlah"].ToString();
                                jml3 = Convert.ToInt32(jumlah);
                                Label3.Text = jml3.ToString();
                                //string type = dr["type"].ToString();
                                //if (type == "Face Recognition Match")
                                //{
                                //    jml3++;
                                //}
                            }
                            //Response.Write("jumlah" + jml);

                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd5 = new MySqlCommand(query3, con))
                {
                    cmd5.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd5.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd5.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd5.Prepare();
                    using (MySqlDataReader dr = cmd5.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //string type = dr["type"].ToString();
                                //if (type == "Plate Match")
                                //{
                                //    jml5++;
                                //}
                                string jumlah = dr["jumlah"].ToString();
                                Label5.Text = jumlah;
                            }
                            //Response.Write("jumlah" + jml);

                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd6 = new MySqlCommand(query4, con))
                {
                    cmd6.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd6.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd6.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd6.Prepare();
                    using (MySqlDataReader dr = cmd6.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //string type = dr["type"].ToString();
                                //if (type == "Face Recognition Match")
                                //{
                                //    jml4++;
                                //}
                                string jumlah = dr["jumlah"].ToString();
                                Label4.Text = jumlah;
                            }
                            //Response.Write("jumlah" + jml);

                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd7 = new MySqlCommand(query5, con))
                {
                    cmd7.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd7.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd7.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd7.Prepare();
                    using (MySqlDataReader dr = cmd7.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //string type = dr["type"].ToString();
                                //if (type == "Face Recognition Match")
                                //{
                                //    jml4++;
                                //}
                                string jumlah = dr["jumlah"].ToString();
                                Label6.Text = jumlah;
                            }
                            //Response.Write("jumlah" + jml);

                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd7 = new MySqlCommand(query6, con))
                {
                    cmd7.Parameters.AddWithValue("@camera", camera);
                    if (from_date != null)
                    {
                        cmd7.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd7.Parameters.AddWithValue("@to", to_date);

                    }
                    cmd7.Prepare();
                    using (MySqlDataReader dr = cmd7.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                //string type = dr["type"].ToString();
                                //if (type == "Face Recognition Match")
                                //{
                                //    jml4++;
                                //}
                                Label7.Text = dr["jumlah"].ToString();
                            }
                            //Response.Write("jumlah" + jml);

                        }
                        else
                        {
                        }
                    }
                }

                //Label4.Text = jml4.ToString();

            }
        }

        private void FillEventHistoryDPO()
        {
            string camera_site = Session["camera_site_event"].ToString();
            string from2 = Session["from_event_dashboard"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from2);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;

            //Response.Write("<script>alert('Isinya:" + Session["lokasi"] + "')</script>");
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

            //DateTime fromDate;
            //DateTime toDate;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (camera_site == "none") { }
                else
                {
                    string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE cs.id=@camera_site AND (ce.image_file != '' or ce.image_file != NULL) and p.type='blacklist'";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >=@from";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <=@to";
                    }
                    query += @" ORDER BY ce.occurred_at DESC";
                    //Response.Write(query);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        if (from_date != null)
                        {
                            cmd.Parameters.AddWithValue("@from", from_date);
                        }
                        if (to_date != null)
                        {
                            cmd.Parameters.AddWithValue("@to", to_date);
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
                                    string person = dr["person"].ToString();
                                    string location = dr["location"].ToString();
                                    string event_type = dr["event_type"].ToString();
                                    string site = dr["site"].ToString();
                                    string person_type = dr["person_type"].ToString();
                                    string person_image = dr["person_image"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
                                    if (person_type == "blacklist")
                                    {
                                        TableRow row = new TableRow();

                                        TableCell imageCell = new TableCell();
                                        TableCell imageCell2 = new TableCell();
                                        TableCell pers = new TableCell();
                                        int cek = event_type.IndexOf("Plate");

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
                                            imageCell.ColumnSpan = 2;
                                            row.Cells.Add(imageCell);
                                        }
                                        pers.Text = person;

                                        TableCell occurred = new TableCell() { Text = occurred_at };
                                        TableCell loc = new TableCell() { Text = location };
                                        TableCell sites = new TableCell() { Text = site };
                                        //TableCell status = new TableCell() { Text = event_type };
                                        row.Cells.Add(occurred);
                                        row.Cells.Add(loc);
                                        row.Cells.Add(sites);
                                        row.Cells.Add(pers);
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



                                        Tbody1.Controls.Add(row);
                                    }
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
        //private void FillEventHistory()
        //{
        //    string camera_site = Session["camera_site_event"].ToString();
        //    string from2 = Session["from_event_dashboard"].ToString();
        //    DateTime parsedDateTime = DateTime.Parse(from2);
        //    string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    string to = null;
        //    string to_date = null;

        //    string lokasi = DropDownList2.SelectedValue;
        //    string person_name = TextBox3.Text;
        //    string type_event = DropDownList3.SelectedValue;
        //    //Response.Write("<script>alert('Isinya:" + Session["lokasi"] + "')</script>");
        //    if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
        //    {
        //        //Response.Write("<script>alert('Ga kosong')</script>");
        //        to = Session["to_event_dashboard"].ToString();
        //        DateTime parsedDateTime2 = DateTime.Parse(to);
        //        to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
        //    }
        //    else
        //    {
        //        //Response.Write("<script>alert('kosong')</script>");

        //    }

        //    DateTime fromDate;
        //    DateTime toDate;

        //    using (MySqlConnection con = new MySqlConnection(strcon))
        //    {
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        if (camera_site == "none") { }
        //        else
        //        {
        //            string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE cs.id=@camera_site AND (ce.image_file != '' or ce.image_file != NULL)";
        //            if (from_date != null)
        //            {
        //                query += @" AND ce.occurred_at >=@from";
        //            }
        //            if (to_date != null)
        //            {
        //                query += @" AND ce.occurred_at <=@to";
        //            }
        //            if (lokasi != "none")
        //            {
        //                query += @" AND ce.camera_id=@lokasi";
        //            }
        //            if (person_name != "")
        //            {
        //                query += @" AND p.name like '" + person_name + "'";
        //            }
        //            if (type_event != "none")
        //            {
        //                query += @" AND ce.type=@type_event";
        //            }
        //            query += @" ORDER BY ce.occurred_at DESC";
        //            //Response.Write(query);
        //            using (MySqlCommand cmd = new MySqlCommand(query, con))
        //            {
        //                cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                if (from_date != null)
        //                {
        //                    cmd.Parameters.AddWithValue("@from", from_date);
        //                }
        //                if (to_date != null)
        //                {
        //                    cmd.Parameters.AddWithValue("@to", to_date);
        //                }
        //                if (lokasi != "none")
        //                {
        //                    cmd.Parameters.AddWithValue("@lokasi", lokasi);
        //                }
        //                if (type_event != "none")
        //                {
        //                    cmd.Parameters.AddWithValue("@type_event", type_event);
        //                }
        //                cmd.Prepare();
        //                using (MySqlDataReader dr = cmd.ExecuteReader())
        //                {
        //                    if (dr.HasRows)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                            string image_file = dr["image_file"].ToString();
        //                            string plate_number_file = dr["plate_number_file"].ToString();
        //                            string occurred_at = dr["occurred_at"].ToString();
        //                            string person = dr["person"].ToString();
        //                            string location = dr["location"].ToString();
        //                            string event_type = dr["event_type"].ToString();
        //                            string site = dr["site"].ToString();
        //                            string person_type = dr["person_type"].ToString();
        //                            string person_image = dr["person_image"].ToString();
        //                            string entry_code = dr["entry_code"].ToString();
        //                            if (person_type == "blacklist")
        //                            {
        //                                TableRow row = new TableRow();

        //                                TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        row.Cells.Add(imageCell);
        //                                        row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    if (!string.IsNullOrEmpty(person_image))
        //                                    {
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "person_image/" + person_image,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        row.Cells.Add(imageCell);
        //                                        row.Cells.Add(imageCell2);
        //                                    }
        //                                    else
        //                                    {
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                //TableCell status = new TableCell() { Text = event_type };
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                //row.Cells.Add(status);
        //                                if (event_type == "Face Recognition Match")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "Face Recognized" };
        //                                    row.Cells.Add(status);
        //                                }
        //                                else if (event_type == "Face Recognition Not Match")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "Face Not Registered" };
        //                                    row.Cells.Add(status);
        //                                }
        //                                else if (event_type == "Plate Match")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "License Plate Recognized" };
        //                                    row.Cells.Add(status);
        //                                }
        //                                else if (event_type == "Plate MisMatch")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
        //                                    row.Cells.Add(status);

        //                                }



        //                                Tbody1.Controls.Add(row);
        //                            }
        //                            else
        //                            {
        //                                TableRow row = new TableRow();

        //                                TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        row.Cells.Add(imageCell);
        //                                        row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        row.Cells.Add(imageCell);
        //                                        if (!string.IsNullOrEmpty(person_image))
        //                                        {
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "person_image/" + person_image,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        else
        //                                        {
        //                                            imageCell2.Text = "";
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                    }
        //                                    if (!string.IsNullOrEmpty(person_image))
        //                                    {
        //                                        string warna = "";
        //                                        if (person_type == "blacklist")
        //                                        {
        //                                            warna += "red";
        //                                        }
        //                                        else
        //                                        {
        //                                            if (entry_code == "0")
        //                                            {
        //                                                warna += "green";
        //                                            }
        //                                            else if (entry_code == "1")
        //                                            {
        //                                                warna += "orange";
        //                                            }
        //                                            else if (entry_code == "2")
        //                                            {
        //                                                warna += "red";
        //                                            }
        //                                        }
        //                                        pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry_code}</span>";
        //                                    }
        //                                    else
        //                                    {
        //                                        pers.Text = person;
        //                                    }
        //                                }

        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                //TableCell status = new TableCell() { Text = event_type };

        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                //row.Cells.Add(status);
        //                                if (event_type == "Face Recognition Match")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "Face Recognized" };
        //                                    row.Cells.Add(status);
        //                                }
        //                                else if (event_type == "Face Recognition Not Match")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "Face Not Registered" };
        //                                    row.Cells.Add(status);
        //                                }
        //                                else if (event_type == "Plate Match")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "License Plate Recognized" };
        //                                    row.Cells.Add(status);
        //                                }
        //                                else if (event_type == "Plate MisMatch")
        //                                {
        //                                    TableCell status = new TableCell() { Text = "License Plate Not Registered" };
        //                                    row.Cells.Add(status);

        //                                }



        //                                TableBody.Controls.Add(row);
        //                            }
        //                        }
        //                        dr.Close();
        //                    }
        //                    else
        //                    {
        //                    }
        //                }
        //            }

        //        }

        //    }
        //}
        private void FillEventHistory()
        {
            string camera_site = Session["camera_site_event"].ToString();
            string from2 = Session["from_event_dashboard"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from2);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;

            string lokasi = DropDownList2.SelectedValue;
            string person_name = TextBox3.Text;
            string type_event = DropDownList3.SelectedValue;
            //Response.Write("<script>alert('Isinya:" + TextBox3.Text + "')</script>");
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

            //DateTime fromDate;
            //DateTime toDate;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (camera_site == "none") { }
                else
                {
                    string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, ce.plate_number, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle, v.type as vehicle_type FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE cs.id=@camera_site AND (ce.image_file != '' or ce.image_file != NULL)";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >=@from";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <=@to";
                    }
                    if (lokasi != "none")
                    {
                        query += @" AND ce.camera_id=@lokasi";
                    }
                    if (person_name != "")
                    {
                        query += @" AND p.name like '%" + person_name + "%'";
                    }
                    if (type_event != "none")
                    {
                        query += @" AND ce.type=@type_event";
                    }
                    //query += @" AND (ce.person_identification_number IS NULL OR ce.person_identification_number = '' OR p.type IS NULL OR p.type != 'blacklist') ";
                    query += @" ORDER BY ce.occurred_at DESC";
                    //Response.Write(query);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        if (from_date != null)
                        {
                            cmd.Parameters.AddWithValue("@from", from_date);
                        }
                        if (to_date != null)
                        {
                            cmd.Parameters.AddWithValue("@to", to_date);
                        }
                        if (lokasi != "none")
                        {
                            cmd.Parameters.AddWithValue("@lokasi", lokasi);
                        }
                        if (type_event != "none")
                        {
                            cmd.Parameters.AddWithValue("@type_event", type_event);
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
                                    string person = dr["person"].ToString();
                                    string location = dr["location"].ToString();
                                    string event_type = dr["event_type"].ToString();
                                    string site = dr["site"].ToString();
                                    string person_type = dr["person_type"].ToString();
                                    string person_image = dr["person_image"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
                                    string plate_number = dr["plate_number"].ToString();
                                    string code_vehicle = dr["code_vehicle"].ToString();
                                    string vehicle_type = dr["vehicle_type"].ToString();

                                    TableRow row = new TableRow();

                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();
                                    TableCell plate = new TableCell();
                                    int cek = event_type.IndexOf("Plate");
                                    int cek2 = event_type.IndexOf("Intrusion");

                                    if (cek != -1)
                                    {
                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
                                        if (!string.IsNullOrEmpty(plate_number_file))
                                        {
                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                            {
                                                ImageUrl = "image_file/" + image_file,
                                                AlternateText = "icon title"
                                            };
                                            img.Style.Add("width", "150px");
                                            imageCell.Controls.Add(img);
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
                                        pers.Text = "";
                                        string warna = "";
                                        string entry = "";
                                        if (code_vehicle == "0")
                                        {
                                            warna += "green";
                                            entry += "Comply";
                                        }
                                        else if (code_vehicle == "1")
                                        {
                                            warna += "orange";
                                            entry += "Not Approved";
                                        }
                                        else if (code_vehicle == "2")
                                        {
                                            warna += "red";
                                            entry += "";
                                        }
                                        else
                                        {
                                            warna += "red";
                                            entry += "Unrecognized";
                                        }
                                        if (code_vehicle == null || code_vehicle == "")
                                        {
                                            plate.Text = plate_number;
                                        }
                                        else
                                        {
                                            plate.Text = $"<span style='color:{warna};'>[{char.ToUpper(vehicle_type[0]) + vehicle_type.Substring(1)}]</span> - {plate_number} - <span style='color:{warna};'>{entry}</span>";
                                        }
                                    }
                                    else
                                    {
                                        if (cek2 != -1)
                                        {
                                            //Console.WriteLine("Substring ditemukan di indeks: " + index);
                                            if (!string.IsNullOrEmpty(image_file))
                                            {
                                                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                                {
                                                    ImageUrl = "image_file/" + image_file,
                                                    AlternateText = "icon title"
                                                };
                                                img.Style.Add("width", "150px");
                                                imageCell.Controls.Add(img);
                                                imageCell2.Text = "";
                                                row.Cells.Add(imageCell);
                                                row.Cells.Add(imageCell2);
                                            }
                                            pers.Text = "";
                                            plate.Text = "";
                                        }
                                        else
                                        {
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
                                                if (!string.IsNullOrEmpty(person_image))
                                                {
                                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                                    {
                                                        ImageUrl = "person_image/" + person_image,
                                                        AlternateText = "icon title"
                                                    };
                                                    img2.Style.Add("width", "150px");
                                                    imageCell2.Controls.Add(img2);
                                                    row.Cells.Add(imageCell2);
                                                }
                                                else
                                                {
                                                    imageCell2.Text = "";
                                                    row.Cells.Add(imageCell2);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(person))
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
                                                plate.Text = "";
                                            }
                                            else
                                            {
                                                pers.Text = person;
                                                plate.Text = "";
                                            }
                                        }

                                    }

                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sites = new TableCell() { Text = site };
                                    //TableCell status = new TableCell() { Text = event_type };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sites);
                                    row.Cells.Add(pers);
                                    row.Cells.Add(plate);
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
                                    else if (event_type == "Intrusion Detection")
                                    {
                                        TableCell status = new TableCell() { Text = "Traffic Detected" };
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






        //private void FillEventHistory()
        //{
        //    string camera_site = Session["camera_site_event"].ToString();
        //    string from = Session["from"].ToString();
        //    string to = null;
        //    if (Session["to"] != null)
        //    {
        //        to = Session["to"].ToString();
        //    }

        //    DateTime fromDate;
        //    DateTime toDate;

        //    using (MySqlConnection con = new MySqlConnection(strcon))
        //    {
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        if (camera_site == "none") { }
        //        else
        //        {

        //            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //            {
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE cs.id=@camera_site AND (ce.image_file != '' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to ORDER BY ce.occurred_at DESC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@from", from);
        //                    cmd.Parameters.AddWithValue("@to", to);
        //                    cmd.Prepare();
        //                    using (MySqlDataReader dr = cmd.ExecuteReader())
        //                    {
        //                        if (dr.HasRows)
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                string image_file = dr["image_file"].ToString();
        //                                string plate_number_file = dr["plate_number_file"].ToString();
        //                                string occurred_at = dr["occurred_at"].ToString();
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
        //                                if (person_type == "blacklist")
        //                                {
        //                                    TableRow row = new TableRow();

        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell imageCell2 = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            // string imagePath2 = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                            // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + plate_number_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            imageCell.ColumnSpan = 2;
        //                                            row.Cells.Add(imageCell);
        //                                            //imageCell2.Text = "";
        //                                            //row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = person;
        //                                    }
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };
        //                                    //row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);



        //                                    Tbody1.Controls.Add(row);
        //                                }
        //                                else
        //                                {
        //                                    TableRow row = new TableRow();

        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell imageCell2 = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            // string imagePath2 = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                            // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + plate_number_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            //imageCell.ColumnSpan = 2;
        //                                            row.Cells.Add(imageCell);
        //                                            imageCell2.Text = "";
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = person;
        //                                    }

        //                                    //TableCell infoCell = new TableCell();
        //                                    //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                    //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                    //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };


        //                                    //TableCell infoCell = new TableCell();
        //                                    //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                    //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                    //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                    //row.Cells.Add(infoCell);
        //                                    //row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);



        //                                    TableBody.Controls.Add(row);
        //                                }
        //                            }
        //                            dr.Close();
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(from.ToString(), out fromDate))
        //            {
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE cs.id=@camera_site AND (ce.image_file != '' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from ORDER BY ce.occurred_at DESC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@from", from);
        //                    cmd.Prepare();
        //                    using (MySqlDataReader dr = cmd.ExecuteReader())
        //                    {
        //                        if (dr.HasRows)
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                string image_file = dr["image_file"].ToString();
        //                                string plate_number_file = dr["plate_number_file"].ToString();
        //                                string occurred_at = dr["occurred_at"].ToString();
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
        //                                if (person_type == "blacklist")
        //                                {
        //                                    TableRow row = new TableRow();

        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell imageCell2 = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            // string imagePath2 = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                            // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + plate_number_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            imageCell.ColumnSpan = 2;
        //                                            row.Cells.Add(imageCell);
        //                                            //imageCell2.Text = "";
        //                                            //row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = person;
        //                                    }
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };
        //                                    //row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);



        //                                    Tbody1.Controls.Add(row);
        //                                }
        //                                else
        //                                {
        //                                    TableRow row = new TableRow();

        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell imageCell2 = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            // string imagePath2 = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                            // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + plate_number_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            //imageCell.ColumnSpan = 2;
        //                                            row.Cells.Add(imageCell);
        //                                            imageCell2.Text = "";
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = person;
        //                                    }

        //                                    //TableCell infoCell = new TableCell();
        //                                    //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                    //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                    //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };


        //                                    //TableCell infoCell = new TableCell();
        //                                    //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                    //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                    //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                    //row.Cells.Add(infoCell);
        //                                    //row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);
        //                                    //row.Cells.Add(infoCell);

        //                                    TableBody.Controls.Add(row);
        //                                }
        //                            }
        //                            dr.Close();
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(to, out toDate))
        //            {
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE cs.id=@camera_site AND (ce.image_file != '' or ce.image_file != NULL) AND DATE(ce.occurred_at) <=@to ORDER BY ce.occurred_at DESC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@to", to);
        //                    cmd.Prepare();
        //                    using (MySqlDataReader dr = cmd.ExecuteReader())
        //                    {
        //                        if (dr.HasRows)
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                string image_file = dr["image_file"].ToString();
        //                                string plate_number_file = dr["plate_number_file"].ToString();
        //                                string occurred_at = dr["occurred_at"].ToString();
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
        //                                if (person_type == "blacklist")
        //                                {
        //                                    TableRow row = new TableRow();

        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell imageCell2 = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            // string imagePath2 = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                            // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + plate_number_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            imageCell.ColumnSpan = 2;
        //                                            row.Cells.Add(imageCell);
        //                                            //imageCell2.Text = "";
        //                                            //row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = person;
        //                                    }
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };
        //                                    //row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);



        //                                    Tbody1.Controls.Add(row);
        //                                }
        //                                else
        //                                {
        //                                    TableRow row = new TableRow();

        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell imageCell2 = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        //Console.WriteLine("Substring ditemukan di indeks: " + index);
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            // string imagePath2 = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                            // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + plate_number_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img2.Style.Add("width", "150px");
        //                                            imageCell2.Controls.Add(img2);
        //                                            row.Cells.Add(imageCell);
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                            //imageCell.ColumnSpan = 2;
        //                                            row.Cells.Add(imageCell);
        //                                            imageCell2.Text = "";
        //                                            row.Cells.Add(imageCell2);
        //                                        }
        //                                        pers.Text = person;
        //                                    }

        //                                    //TableCell infoCell = new TableCell();
        //                                    //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                    //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                    //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };


        //                                    //TableCell infoCell = new TableCell();
        //                                    //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                    //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                    //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                    //row.Cells.Add(infoCell);
        //                                    //row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);

        //                                    TableBody.Controls.Add(row);
        //                                }
        //                            }
        //                            dr.Close();
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}
        //private void FillChart1()
        //{
        //    List<string> labels = new List<string>();
        //    List<string> cameras = new List<string>();
        //    //List<int> face_valid = new List<int>();
        //    List<int> face_invalid = new List<int>();
        //    List<int> unregister = new List<int>();
        //    List<int> blacklist = new List<int>();
        //    List<int> jumlah = new List<int>();
        //    List<int> jumlah2 = new List<int>();
        //    List<int> plate_register = new List<int>();
        //    List<int> plate_unregister = new List<int>();
        //    string camera_site = Session["camera_site_event"].ToString();
        //    string from = Session["from"].ToString();
        //    string to = null;
        //    if (Session["to"] != null)
        //    {
        //        to = Session["to"].ToString();
        //    }
        //    if (camera_site == "none")
        //    {

        //    }
        //    else
        //    {
        //        DateTime tanggalDibandingkan = DateTime.Parse(from);
        //        using (MySqlConnection con1 = new MySqlConnection(strcon))
        //        {
        //            if (con1.State == ConnectionState.Closed)
        //            {
        //                con1.Open();
        //            }

        //            using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site", con1))
        //            {
        //                cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                cmd.Prepare();
        //                using (MySqlDataReader dr = cmd.ExecuteReader())
        //                {
        //                    while (dr.Read())
        //                    {
        //                        labels.Add(dr["location"].ToString());
        //                        cameras.Add(dr["name"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //        foreach (var camera in cameras)
        //        {
        //            int fv = 0;
        //            int fi = 0;
        //            int pr = 0;
        //            int pu = 0;
        //            int nm = 0;
        //            int bl = 0;
        //            DateTime fromDate;
        //            DateTime toDate;
        //            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //            {
        //                using (MySqlConnection con1 = new MySqlConnection(strcon))
        //                {
        //                    if (con1.State == ConnectionState.Closed)
        //                    {
        //                        con1.Open();
        //                    }

        //                    //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
        //                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.camera_id=@camera AND DATE(c.occurred_at)>=@from AND DATE(c.occurred_at)<=@to ORDER BY c.occurred_at;", con1))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera", camera);
        //                        cmd.Parameters.AddWithValue("@from", from);
        //                        cmd.Parameters.AddWithValue("@to", to);
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                string type = dr["type"].ToString();
        //                                int cek = type.IndexOf("Plate");
        //                                if (cek != -1)
        //                                {
        //                                    //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
        //                                    if (dr["entry_code"] != null)
        //                                    {
        //                                        int cek2 = type.IndexOf("MisMatch");
        //                                        if (cek2 != -1)
        //                                        {
        //                                            pu++;
        //                                        }
        //                                        else
        //                                        {
        //                                            pr++;
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                        //Response.Write("not match" + type);
        //                                    if (type == "Face Recognition Not Match")
        //                                    {
        //                                        nm++;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (dr["person_type"].ToString() == "blacklist")
        //                                        {
        //                                            bl++;
        //                                        }
        //                                        else
        //                                        {
        //                                            //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
        //                                            if (dr["entry_code"] != null || dr["entry_code"].ToString()!="")
        //                                            {
        //                                                //byte entryAllowedValue = Convert.ToByte(dr[" ode"]);

        //                                                // Membandingkan nilai dengan 1
        //                                                //if (entryAllowedValue == 1)
        //                                                if (dr["entry_code"].ToString() == "True" || dr["entry_code"].ToString()=="0")
        //                                                {
        //                                                    fv++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (dr["entry_code"].ToString() == "1" || dr["entry_code"].ToString() == "2")
        //                                                    {
        //                                                        //Response.Write("<script>alert('Apa " + dr["entry_code"].ToString() + "')</script>");
        //                                                        fi++;
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                            }
        //                                        }


        //                                    }
        //                                }
        //                            }
        //                            //int jum = fv + fi;
        //                            //int jum = nm + fi + bl;
        //                            int jum = fv;
        //                            int jum2 = pr + pu;
        //                            jumlah.Add(jum);
        //                            jumlah2.Add(jum2);
        //                            //face_valid.Add(fv);
        //                            unregister.Add(nm);
        //                            blacklist.Add(bl);
        //                            face_invalid.Add(fi);
        //                            plate_register.Add(pr);
        //                            plate_unregister.Add(pu);
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(from.ToString(), out fromDate))
        //            {
        //                using (MySqlConnection con1 = new MySqlConnection(strcon))
        //                {
        //                    if (con1.State == ConnectionState.Closed)
        //                    {
        //                        con1.Open();
        //                    }

        //                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.camera_id=@camera AND DATE(c.occurred_at)>=@from ORDER BY c.occurred_at;", con1))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera", camera);
        //                        cmd.Parameters.AddWithValue("@from", from);
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            while (dr.Read())
        //                            {

        //                                string type = dr["type"].ToString();
        //                                int cek = type.IndexOf("Plate");
        //                                if (cek != -1)
        //                                {
        //                                    if (dr["entry_code"] != null)
        //                                    {
        //                                        int cek2 = type.IndexOf("MisMatch");
        //                                        if (cek2 != -1)
        //                                        {
        //                                            pu++;
        //                                        }
        //                                        else
        //                                        {
        //                                            pr++;
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (type == "Face Recognition Not Match")
        //                                    {
        //                                        nm++;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (dr["person_type"].ToString() == "blacklist")
        //                                        {
        //                                            bl++;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (dr["entry_code"] != null)
        //                                            {
        //                                                //byte entryAllowedValue = Convert.ToByte(dr["entry_code"]);

        //                                                // Membandingkan nilai dengan 1
        //                                                //if (entryAllowedValue == 1)
        //                                                if (dr["entry_code"].ToString() == "True" || dr["entry_code"].ToString() == "0")
        //                                                {
        //                                                    fv++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (dr["entry_code"].ToString() == "1" || dr["entry_code"].ToString() == "2")
        //                                                    {
        //                                                        //Response.Write("<script>alert('Apa " + dr["entry_code"].ToString() + "')</script>");
        //                                                        fi++;
        //                                                    }
        //                                                }
        //                                            }
        //                                        }


        //                                    }
        //                                }
        //                            }
        //                            //int jum = fv + fi;
        //                            int jum = nm + fi + bl;
        //                            int jum2 = pr + pu;
        //                            jumlah.Add(jum);
        //                            jumlah2.Add(jum2);
        //                            //face_valid.Add(fv);
        //                            unregister.Add(nm);
        //                            blacklist.Add(bl);
        //                            face_invalid.Add(fi);
        //                            plate_register.Add(pr);
        //                            plate_unregister.Add(pu);
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(to, out toDate))
        //            {
        //                using (MySqlConnection con1 = new MySqlConnection(strcon))
        //                {
        //                    if (con1.State == ConnectionState.Closed)
        //                    {
        //                        con1.Open();
        //                    }

        //                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.camera_id=@camera AND DATE(c.occurred_at)<=@to ORDER BY c.occurred_at;", con1))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera", camera);
        //                        cmd.Parameters.AddWithValue("@to", to);
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            while (dr.Read())
        //                            {

        //                                string type = dr["type"].ToString();
        //                                int cek = type.IndexOf("Plate");
        //                                if (cek != -1)
        //                                {
        //                                    if (dr["entry_code"] != null)
        //                                    {
        //                                        int cek2 = type.IndexOf("MisMatch");
        //                                        if (cek2 != -1)
        //                                        {
        //                                            pu++;
        //                                        }
        //                                        else
        //                                        {
        //                                            pr++;
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (type == "Face Recognition Not Match")
        //                                    {
        //                                        nm++;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (dr["person_type"].ToString() == "blacklist")
        //                                        {
        //                                            bl++;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (dr["entry_code"] != null)
        //                                            {
        //                                                //byte entryAllowedValue = Convert.ToByte(dr["entry_code"]);

        //                                                // Membandingkan nilai dengan 1
        //                                                //if (entryAllowedValue == 1)
        //                                                if (dr["entry_code"].ToString() == "True" || dr["entry_code"].ToString() == "0")
        //                                                {
        //                                                    fv++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (dr["entry_code"].ToString() == "1" || dr["entry_code"].ToString() == "2")
        //                                                    {
        //                                                        //Response.Write("<script>alert('Apa " + dr["entry_code"].ToString() + "')</script>");
        //                                                        fi++;
        //                                                    }
        //                                                }
        //                                            }
        //                                        }


        //                                    }
        //                                }
        //                            }
        //                            //int jum = fv + fi;
        //                            int jum = nm + fi + bl;
        //                            int jum2 = pr + pu;
        //                            jumlah.Add(jum);
        //                            jumlah2.Add(jum2);
        //                            //face_valid.Add(fv);
        //                            unregister.Add(nm);
        //                            blacklist.Add(bl);
        //                            face_invalid.Add(fi);
        //                            plate_register.Add(pr);
        //                            plate_unregister.Add(pu);
        //                        }
        //                    }
        //                }
        //            }

        //            //foreach (var innerList in list2D)
        //            //{

        //            //    // Iterasi melalui list dalam list dua dimensi
        //            //    foreach (var item in innerList)
        //            //    {
        //            //        // Cek tipe data untuk mengetahui cara akses
        //            //        if (item is int)
        //            //        {
        //            //            //Response.Write("ID: " + item);
        //            //            var id = item;
        //            //        }
        //            //        else if (item is String)
        //            //        {
        //            //            //Response.Write("Occurred At: " + item);
        //            //            var occurred_at = item;
        //            //        }
        //            //    }
        //            //}

        //        }

        //        var serializer = new JavaScriptSerializer();
        //        var labelsJson = serializer.Serialize(labels);
        //        var unregisterJson = serializer.Serialize(unregister);
        //        var faceInvalidJson = serializer.Serialize(face_invalid);
        //        var blacklistJson = serializer.Serialize(blacklist);
        //        var jumlahJson = serializer.Serialize(jumlah);
        //        var labels2Json = serializer.Serialize(labels);
        //        var plateRegisterJson = serializer.Serialize(plate_register);
        //        var plateUnregisterJson = serializer.Serialize(plate_unregister);
        //        var jumlah2Json = serializer.Serialize(jumlah2);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript", $"fillChart2({labelsJson}, {unregisterJson}, {faceInvalidJson}, {blacklistJson}, {jumlahJson});", true);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript2", $"fillChart({labels2Json}, {plateRegisterJson}, {plateUnregisterJson},{jumlah2Json});", true);
        //    }


        //}


        //private void FillChart1()
        //{
        //    List<string> labels = new List<string>();
        //    List<string> cameras = new List<string>();
        //    //List<int> face_valid = new List<int>();
        //    List<int> face_invalid = new List<int>();
        //    List<int> unregister = new List<int>();
        //    List<int> blacklist = new List<int>();
        //    List<int> jumlah = new List<int>();
        //    string camera_site = Session["camera_site_event"].ToString();
        //    string from2 = Session["from_event_dashboard"].ToString();
        //    DateTime parsedDateTime = DateTime.Parse(from2);
        //    string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    string to = null;
        //    string to_date = null;
        //    //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
        //    if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
        //    {
        //        //Response.Write("<script>alert('Ga kosong')</script>");
        //        to = Session["to_event_dashboard"].ToString();
        //        DateTime parsedDateTime2 = DateTime.Parse(to);
        //        to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
        //    }
        //    else
        //    {
        //        //Response.Write("<script>alert('kosong')</script>");

        //    }
        //    if (camera_site == "none")
        //    {

        //    }
        //    else
        //    {
        //        using (MySqlConnection con1 = new MySqlConnection(strcon))
        //        {
        //            if (con1.State == ConnectionState.Closed)
        //            {
        //                con1.Open();
        //            }

        //            using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site and name not like 'LPR%'", con1))
        //            {
        //                cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                cmd.Prepare();
        //                using (MySqlDataReader dr = cmd.ExecuteReader())
        //                {
        //                    while (dr.Read())
        //                    {
        //                        labels.Add(dr["location"].ToString());
        //                        cameras.Add(dr["name"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //        foreach (var camera in cameras)
        //        {
        //            int fv = 0;
        //            int fi = 0;
        //            int nm = 0;
        //            int bl = 0;
        //            DateTime fromDate;
        //            DateTime toDate;

        //                using (MySqlConnection con1 = new MySqlConnection(strcon))
        //                {
        //                    if (con1.State == ConnectionState.Closed)
        //                    {
        //                        con1.Open();
        //                    }
        //                    string query = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE (c.image_file != '' or c.image_file != NULL) AND c.type like '%Face%' AND c.camera_id=@camera";
        //                    if (from_date != null)
        //                    {
        //                        query += @" AND c.occurred_at >= @from";
        //                    }
        //                    if (to_date!=null)
        //                    {
        //                        query += @" AND c.occurred_at <= @to";
        //                    }
        //                    query += @" ORDER BY c.occurred_at";
        //                        //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
        //                        using (MySqlCommand cmd = new MySqlCommand(query, con1))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera", camera);
        //                        if (from_date != null)
        //                        {
        //                            cmd.Parameters.AddWithValue("@from", from_date);
        //                        }
        //                        if (to_date != null)
        //                        {
        //                            cmd.Parameters.AddWithValue("@to", to_date);
        //                        }
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                string type = dr["type"].ToString();
        //                                int cek = type.IndexOf("Plate");

        //                                    //Response.Write("not match" + type);
        //                                    if (type == "Face Recognition Not Match")
        //                                    {
        //                                        nm++;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (dr["person_type"].ToString() == "blacklist")
        //                                        {
        //                                            bl++;
        //                                        }
        //                                        else
        //                                        {
        //                                            //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
        //                                            if (dr["entry_code"] != null || dr["entry_code"].ToString() != "")
        //                                            {
        //                                                //byte entryAllowedValue = Convert.ToByte(dr[" ode"]);

        //                                                // Membandingkan nilai dengan 1
        //                                                //if (entryAllowedValue == 1)
        //                                                if (dr["entry_code"].ToString() == "True" || dr["entry_code"].ToString() == "0")
        //                                                {
        //                                                    fv++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (dr["entry_code"].ToString() == "1" || dr["entry_code"].ToString() == "2")
        //                                                    {
        //                                                        //Response.Write("<script>alert('Apa " + dr["entry_code"].ToString() + "')</script>");
        //                                                        fi++;
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                            }
        //                                        }


        //                                    }

        //                            }
        //                            //int jum = fv + fi;
        //                            //int jum = nm + fi + bl;
        //                            int jum = fv;
        //                            jumlah.Add(jum);
        //                            //face_valid.Add(fv);
        //                            unregister.Add(nm);
        //                            blacklist.Add(bl);
        //                            face_invalid.Add(fi);
        //                        }
        //                    }
        //                }


        //            //foreach (var innerList in list2D)
        //            //{

        //            //    // Iterasi melalui list dalam list dua dimensi
        //            //    foreach (var item in innerList)
        //            //    {
        //            //        // Cek tipe data untuk mengetahui cara akses
        //            //        if (item is int)
        //            //        {
        //            //            //Response.Write("ID: " + item);
        //            //            var id = item;
        //            //        }
        //            //        else if (item is String)
        //            //        {
        //            //            //Response.Write("Occurred At: " + item);
        //            //            var occurred_at = item;
        //            //        }
        //            //    }
        //            //}

        //        }

        //        var serializer = new JavaScriptSerializer();
        //        var labelsJson = serializer.Serialize(labels);
        //        var unregisterJson = serializer.Serialize(unregister);
        //        var faceInvalidJson = serializer.Serialize(face_invalid);
        //        var blacklistJson = serializer.Serialize(blacklist);
        //        var jumlahJson = serializer.Serialize(jumlah);
        //        var labels2Json = serializer.Serialize(labels);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript", $"fillChart2({labelsJson}, {unregisterJson}, {faceInvalidJson}, {blacklistJson}, {jumlahJson});", true);
        //    }


        //}

        private void FillChart1()
        {
            List<string> labels = new List<string>();
            List<string> cameras = new List<string>();
            //List<int> face_valid = new List<int>();
            List<int> face_invalid = new List<int>();
            List<int> unregister = new List<int>();
            List<int> blacklist = new List<int>();
            List<int> jumlah = new List<int>();
            string camera_site = Session["camera_site_event"].ToString();
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
            if (camera_site == "none")
            {

            }
            else
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site and name not like 'LPR%'", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                labels.Add(dr["location"].ToString());
                                cameras.Add(dr["name"].ToString());
                            }
                        }
                    }
                }
                foreach (var camera in cameras)
                {
                    int fv = 0;
                    int fi = 0;
                    int nm = 0;
                    int bl = 0;
                    //DateTime fromDate;
                    //DateTime toDate;

                    using (MySqlConnection con1 = new MySqlConnection(strcon))
                    {
                        if (con1.State == ConnectionState.Closed)
                        {
                            con1.Open();
                        }
                        string query = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE (c.image_file != '' or c.image_file != NULL) AND c.type!='Face Recognition Match' AND c.camera_id=@camera";
                        string query2 = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE (c.image_file != '' or c.image_file != NULL) AND c.type='Face Recognition Match' AND c.camera_id=@camera";
                        if (from_date != null)
                        {
                            query += @" AND c.occurred_at >= @from";
                            query2 += @" AND c.occurred_at >= @from";
                        }
                        if (to_date != null)
                        {
                            query += @" AND c.occurred_at <= @to";
                            query2 += @" AND c.occurred_at <= @to";
                        }
                        query += @" ORDER BY c.occurred_at";
                        query2 += @" GROUP BY c.person_identification_number ORDER BY c.occurred_at DESC";
                        //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
                        using (MySqlCommand cmd = new MySqlCommand(query, con1))
                        {
                            cmd.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd.Prepare();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string type = dr["type"].ToString();
                                    int cek = type.IndexOf("Plate");

                                    //Response.Write("not match" + type);
                                    if (type == "Face Recognition Not Match")
                                    {
                                        nm++;
                                    }
                                    else
                                    {
                                        //if (dr["person_type"].ToString() == "blacklist")
                                        //{
                                        //    bl++;
                                        //}
                                        //else
                                        //{
                                        //    //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
                                        //    if (dr["entry_code"] != null || dr["entry_code"].ToString() != "")
                                        //    {
                                        //        //byte entryAllowedValue = Convert.ToByte(dr[" ode"]);

                                        //        // Membandingkan nilai dengan 1
                                        //        //if (entryAllowedValue == 1)
                                        //        if (dr["entry_code"].ToString() == "True" || dr["entry_code"].ToString() == "0")
                                        //        {
                                        //            fv++;
                                        //        }
                                        //        else
                                        //        {
                                        //            if (dr["entry_code"].ToString() == "1" || dr["entry_code"].ToString() == "2")
                                        //            {
                                        //                //Response.Write("<script>alert('Apa " + dr["entry_code"].ToString() + "')</script>");
                                        //                fi++;
                                        //            }
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //    }
                                        //}


                                    }

                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;
                                //int jum = fv;
                                //jumlah.Add(jum);
                                //face_valid.Add(fv);
                                unregister.Add(nm);
                                //blacklist.Add(bl);
                                //face_invalid.Add(fi);
                            }
                        }
                        using (MySqlCommand cmd2 = new MySqlCommand(query2, con1))
                        {
                            cmd2.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd2.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd2.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd2.Prepare();
                            using (MySqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string type = dr["type"].ToString();
                                    int cek = type.IndexOf("Plate");

                                    //Response.Write("not match" + type);
                                    if (dr["person_type"].ToString() == "blacklist")
                                    {
                                        bl++;
                                    }
                                    else
                                    {
                                        //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
                                        if (dr["entry_code"] != null || dr["entry_code"].ToString() != "")
                                        {
                                            //byte entryAllowedValue = Convert.ToByte(dr[" ode"]);

                                            // Membandingkan nilai dengan 1
                                            //if (entryAllowedValue == 1)
                                            if (dr["entry_code"].ToString() == "True" || dr["entry_code"].ToString() == "0")
                                            {
                                                fv++;
                                            }
                                            else
                                            {
                                                if (dr["entry_code"].ToString() == "1" || dr["entry_code"].ToString() == "2")
                                                {
                                                    //Response.Write("<script>alert('Apa " + dr["entry_code"].ToString() + "')</script>");
                                                    fi++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }

                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;
                                int jum = fv;
                                jumlah.Add(jum);
                                //face_valid.Add(fv);
                                //unregister.Add(nm);
                                blacklist.Add(bl);
                                face_invalid.Add(fi);
                            }
                        }
                    }


                    //foreach (var innerList in list2D)
                    //{

                    //    // Iterasi melalui list dalam list dua dimensi
                    //    foreach (var item in innerList)
                    //    {
                    //        // Cek tipe data untuk mengetahui cara akses
                    //        if (item is int)
                    //        {
                    //            //Response.Write("ID: " + item);
                    //            var id = item;
                    //        }
                    //        else if (item is String)
                    //        {
                    //            //Response.Write("Occurred At: " + item);
                    //            var occurred_at = item;
                    //        }
                    //    }
                    //}

                }

                var serializer = new JavaScriptSerializer();
                var labelsJson = serializer.Serialize(labels);
                var unregisterJson = serializer.Serialize(unregister);
                var faceInvalidJson = serializer.Serialize(face_invalid);
                var blacklistJson = serializer.Serialize(blacklist);
                var jumlahJson = serializer.Serialize(jumlah);
                var labels2Json = serializer.Serialize(labels);
                ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript", $"fillChart2({labelsJson}, {unregisterJson}, {faceInvalidJson}, {blacklistJson}, {jumlahJson});", true);
            }


        }
        //private void FillChart2()
        //{
        //    List<string> labels = new List<string>();
        //    List<string> cameras = new List<string>();
        //    //List<int> face_valid = new List<int>();
        //    List<int> jumlah2 = new List<int>();
        //    List<int> plate_register = new List<int>();
        //    List<int> plate_unregister = new List<int>();
        //    string camera_site = Session["camera_site_event"].ToString();
        //    string from2 = Session["from_event_dashboard"].ToString();
        //    DateTime parsedDateTime = DateTime.Parse(from2);
        //    string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    string to = null;
        //    string to_date = null;
        //    //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
        //    if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
        //    {
        //        //Response.Write("<script>alert('Ga kosong')</script>");
        //        to = Session["to_event_dashboard"].ToString();
        //        DateTime parsedDateTime2 = DateTime.Parse(to);
        //        to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
        //    }
        //    else
        //    {
        //        //Response.Write("<script>alert('kosong')</script>");

        //    }
        //    if (camera_site == "none")
        //    {

        //    }
        //    else
        //    {
        //        using (MySqlConnection con1 = new MySqlConnection(strcon))
        //        {
        //            if (con1.State == ConnectionState.Closed)
        //            {
        //                con1.Open();
        //            }

        //            using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where name like 'LPR%' AND camera_site_id=@camera_site", con1))
        //            {
        //                cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                cmd.Prepare();
        //                using (MySqlDataReader dr = cmd.ExecuteReader())
        //                {
        //                    while (dr.Read())
        //                    {
        //                        labels.Add(dr["location"].ToString());
        //                        cameras.Add(dr["name"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //        foreach (var camera in cameras)
        //        {
        //            int pr = 0;
        //            int pu = 0;
        //            DateTime fromDate;
        //            DateTime toDate;
        //                using (MySqlConnection con1 = new MySqlConnection(strcon))
        //                {
        //                    if (con1.State == ConnectionState.Closed)
        //                    {
        //                        con1.Open();
        //                    }

        //                //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
        //                string query = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.camera_id=@camera";
        //                if(from_date != null)
        //                {
        //                    query += @" AND c.occurred_at>=@from";
        //                }
        //                if(to_date != null)
        //                {
        //                    query += @" AND c.occurred_at<=@to";
        //                }
        //                query += @" ORDER BY c.occurred_at";
        //                    using (MySqlCommand cmd = new MySqlCommand(query, con1))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera", camera);
        //                    if (from_date != null)
        //                    {
        //                        cmd.Parameters.AddWithValue("@from", from_date);
        //                    }
        //                    if (to_date != null)
        //                    {
        //                        cmd.Parameters.AddWithValue("@to", to_date);
        //                    }
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                string type = dr["type"].ToString();
        //                                int cek = type.IndexOf("Plate");
        //                                if (cek != -1)
        //                                {
        //                                    //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
        //                                    if (dr["entry_code"] != null)
        //                                    {
        //                                        int cek2 = type.IndexOf("MisMatch");
        //                                        if (cek2 != -1)
        //                                        {
        //                                            pu++;
        //                                        }
        //                                        else
        //                                        {
        //                                            pr++;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            int jum2 = pr + pu;
        //                            jumlah2.Add(jum2);
        //                            plate_register.Add(pr);
        //                            plate_unregister.Add(pu);
        //                        }
        //                    }
        //                }

        //            //foreach (var innerList in list2D)
        //            //{

        //            //    // Iterasi melalui list dalam list dua dimensi
        //            //    foreach (var item in innerList)
        //            //    {
        //            //        // Cek tipe data untuk mengetahui cara akses
        //            //        if (item is int)
        //            //        {
        //            //            //Response.Write("ID: " + item);
        //            //            var id = item;
        //            //        }
        //            //        else if (item is String)
        //            //        {
        //            //            //Response.Write("Occurred At: " + item);
        //            //            var occurred_at = item;
        //            //        }
        //            //    }
        //            //}

        //        }

        //        var serializer = new JavaScriptSerializer();
        //        var labelsJson = serializer.Serialize(labels);
        //        var labels2Json = serializer.Serialize(labels);
        //        var plateRegisterJson = serializer.Serialize(plate_register);
        //        var plateUnregisterJson = serializer.Serialize(plate_unregister);
        //        var jumlah2Json = serializer.Serialize(jumlah2);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript2", $"fillChart({labels2Json}, {plateRegisterJson}, {plateUnregisterJson},{jumlah2Json});", true);
        //    }


        //}

        private void FillChart2()
        {
            List<string> labels = new List<string>();
            List<string> cameras = new List<string>();
            //List<int> face_valid = new List<int>();
            List<int> jumlah2 = new List<int>();
            List<int> plate_register = new List<int>();
            List<int> plate_unregister = new List<int>();
            string camera_site = Session["camera_site_event"].ToString();
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
            if (camera_site == "none")
            {

            }
            else
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where name like 'LPR%' AND camera_site_id=@camera_site", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                labels.Add(dr["location"].ToString());
                                cameras.Add(dr["name"].ToString());
                            }
                        }
                    }
                }
                foreach (var camera in cameras)
                {
                    int pr = 0;
                    int pu = 0;
                    //DateTime fromDate;
                    //DateTime toDate;
                    using (MySqlConnection con1 = new MySqlConnection(strcon))
                    {
                        if (con1.State == ConnectionState.Closed)
                        {
                            con1.Open();
                        }

                        //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
                        string query = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.type!='Plate Match' AND c.camera_id=@camera";
                        string query2 = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, p.name AS person_name,p.type as person_type, ph.entry_code FROM camera_events c LEFT JOIN persons p ON c.person_identification_number = p.identification_number LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= c.occurred_at) WHERE c.type='Plate Match' AND c.camera_id=@camera";
                        if (from_date != null)
                        {
                            query += @" AND c.occurred_at>=@from";
                            query2 += @" AND c.occurred_at>=@from";
                        }
                        if (to_date != null)
                        {
                            query += @" AND c.occurred_at<=@to";
                            query2 += @" AND c.occurred_at<=@to";
                        }
                        query += @" ORDER BY c.occurred_at";
                        query2 += @" GROUP BY c.plate_number ORDER BY c.occurred_at DESC";
                        using (MySqlCommand cmd = new MySqlCommand(query, con1))
                        {
                            cmd.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd.Prepare();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string type = dr["type"].ToString();
                                    int cek = type.IndexOf("Plate");
                                    if (cek != -1)
                                    {
                                        //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
                                        if (dr["entry_code"] != null)
                                        {
                                            int cek2 = type.IndexOf("MisMatch");
                                            if (cek2 != -1)
                                            {
                                                pu++;
                                            }
                                            else
                                            {
                                                //pr++;
                                            }
                                        }
                                    }
                                }
                                //int jum2 = pr + pu;
                                //jumlah2.Add(jum2);
                                //plate_register.Add(pr);
                                plate_unregister.Add(pu);
                            }
                        }
                        using (MySqlCommand cmd2 = new MySqlCommand(query2, con1))
                        {
                            cmd2.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd2.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd2.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd2.Prepare();
                            using (MySqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string type = dr["type"].ToString();
                                    int cek = type.IndexOf("Plate");
                                    if (cek != -1)
                                    {
                                        //Response.Write("<script>alert('" + dr["entry_code"].ToString() + "')</script>");
                                        if (dr["entry_code"] != null)
                                        {
                                            int cek2 = type.IndexOf("MisMatch");
                                            if (cek2 != -1)
                                            {
                                                //pu++;
                                            }
                                            else
                                            {
                                                pr++;
                                            }
                                        }
                                    }
                                }
                                //int jum2 = pr + pu;
                                //jumlah2.Add(jum2);
                                plate_register.Add(pr);
                                //plate_unregister.Add(pu);
                            }
                        }
                    }

                    //foreach (var innerList in list2D)
                    //{

                    //    // Iterasi melalui list dalam list dua dimensi
                    //    foreach (var item in innerList)
                    //    {
                    //        // Cek tipe data untuk mengetahui cara akses
                    //        if (item is int)
                    //        {
                    //            //Response.Write("ID: " + item);
                    //            var id = item;
                    //        }
                    //        else if (item is String)
                    //        {
                    //            //Response.Write("Occurred At: " + item);
                    //            var occurred_at = item;
                    //        }
                    //    }
                    //}

                }

                var serializer = new JavaScriptSerializer();
                var labelsJson = serializer.Serialize(labels);
                var labels2Json = serializer.Serialize(labels);
                var plateRegisterJson = serializer.Serialize(plate_register);
                var plateUnregisterJson = serializer.Serialize(plate_unregister);
                var jumlah2Json = serializer.Serialize(jumlah2);
                ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript2", $"fillChart({labels2Json}, {plateRegisterJson}, {plateUnregisterJson},{jumlah2Json});", true);
            }


        }
        private void FillChart3()
        {
            List<string> labels = new List<string>();
            List<string> cameras = new List<string>();
            //List<int> face_valid = new List<int>();
            List<int> face_invalid = new List<int>();
            List<int> unregister = new List<int>();
            List<int> blacklist = new List<int>();
            List<int> jumlah = new List<int>();
            string camera_site = Session["camera_site_event"].ToString();
            string from = Session["from_event_dashboard"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
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
            if (camera_site == "none")
            {

            }
            else
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    //DropDownList2.Items.Add(new ListItem("", "none"));
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                labels.Add(dr["location"].ToString());
                                cameras.Add(dr["name"].ToString());
                                //DropDownList2.Items.Add(new ListItem(dr["location"].ToString(), dr["name"].ToString()));
                            }
                        }
                    }
                }
                foreach (var camera in cameras)
                {
                    int satu = 0;
                    int dua = 0;
                    int tiga = 0;
                    int empat = 0;
                    int jum = 0;
                    string camera_name = "";
                    //DateTime fromDate;
                    //DateTime toDate;

                    using (MySqlConnection con1 = new MySqlConnection(strcon))
                    {
                        if (con1.State == ConnectionState.Closed)
                        {
                            con1.Open();
                        }
                        string query = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, cam.location, cam.name FROM camera_events c join cameras cam on c.camera_id=cam.name WHERE type='Face Recognition Not Match' AND (c.image_file != '' or c.image_file != NULL) AND c.camera_id=@camera AND c.type!='Face Recognition Match' AND c.type!='Plate Match'";
                        string query2 = @"SELECT count(*) as jumlah, person_identification_number from camera_events where type='Face Recognition Match' AND camera_id=@camera";
                        string query3 = @"SELECT count(*) as jumlah, plate_number from camera_events where type like '%Plate%' AND plate_number in (SELECT plate_number from vehicles) AND camera_id=@camera";
                        //string query4 = @"SELECT count(*) as jumlah, plate_number from camera_events where type like '%Plate%' AND (plate_number not in (SELECT plate_number from vehicles) AND plate_number!='' AND plate_number!=NULL) AND camera_id=@camera";
                        string query4 = @"SELECT count(*) as jumlah, camera_id, plate_number from camera_events where type like '%Plate%' AND (plate_number not in (SELECT plate_number from vehicles)) AND (plate_number!='' or plate_number!=null) AND camera_id=@camera";
                        string query5 = @"SELECT count(*) as jumlah, plate_number from camera_events where type like '%Plate%' AND (plate_number='' or plate_number=NULL or plate_number IS NULL) AND camera_id=@camera";
                        if (from_date != null)
                        {
                            query += @" AND c.occurred_at >=@from";
                            query2 += @" AND occurred_at >=@from";
                            query3 += @" AND occurred_at >=@from";
                            query4 += @" AND occurred_at >=@from";
                            query5 += @" AND occurred_at >=@from";
                        }
                        if (to_date != null)
                        {
                            query += @" AND c.occurred_at <=@to";
                            query2 += @" AND occurred_at <=@to";
                            query3 += @" AND occurred_at <=@to";
                            query4 += @" AND occurred_at <=@to";
                            query5 += @" AND occurred_at <=@to";
                        }
                        query += @" ORDER BY c.occurred_at";
                        query2 += @" GROUP by person_identification_number ORDER BY occurred_at";
                        query3 += @" GROUP by plate_number ORDER BY occurred_at";
                        query4 += @" GROUP by plate_number ORDER BY occurred_at";
                        query5 += @" ORDER BY occurred_at";
                        //Response.Write(query2);
                        //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
                        using (MySqlCommand cmd = new MySqlCommand(query, con1))
                        {
                            cmd.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd.Prepare();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string type = dr["type"].ToString();
                                    //camera_name+= dr["location"].ToString();
                                    if (type == "Face Recognition Match")
                                    {
                                        //satu++;
                                    }
                                    else if (type == "Face Recognition Not Match")
                                    {
                                        dua++;
                                    }
                                    else if (type == "Plate Match")
                                    {
                                        //tiga++;
                                    }
                                    else if (type == "Plate MisMatch")
                                    {
                                        //empat++;
                                    }

                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;
                                //jum += dua + empat;
                                //jumlah.Add(jum);

                            }
                        }
                        using (MySqlCommand cmd3 = new MySqlCommand(query2, con1))
                        {
                            cmd3.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd3.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd3.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd3.Prepare();
                            using (MySqlDataReader dr = cmd3.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    //camera_name+= dr["location"].ToString();

                                    satu++;


                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;

                            }
                            //jum += satu;
                        }
                        using (MySqlCommand cmd4 = new MySqlCommand(query3, con1))
                        {
                            cmd4.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd4.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd4.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd4.Prepare();
                            using (MySqlDataReader dr = cmd4.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    //camera_name+= dr["location"].ToString();

                                    tiga++;


                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;

                            }
                            //jum += tiga;
                        }
                        //Response.Write(query5);
                        using (MySqlCommand cmd5 = new MySqlCommand(query4, con1))
                        {
                            cmd5.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd5.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd5.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd5.Prepare();
                            using (MySqlDataReader dr = cmd5.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    //camera_name+= dr["location"].ToString();

                                    empat++;


                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;

                            }
                            //jum += tiga;
                        }
                        using (MySqlCommand cmd5 = new MySqlCommand(query5, con1))
                        {
                            cmd5.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd5.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd5.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd5.Prepare();
                            using (MySqlDataReader dr = cmd5.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    //camera_name+= dr["location"].ToString();
                                    string hitung = dr["jumlah"].ToString();
                                    empat += Convert.ToInt32(hitung);


                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;

                            }
                            //jum += tiga;
                        }
                        jum = satu + dua + tiga + empat;
                        jumlah.Add(jum);
                        using (MySqlCommand cmd2 = new MySqlCommand("SELECT * from cameras where name=@camera", con1))
                        {
                            cmd2.Parameters.AddWithValue("@camera", camera);
                            cmd2.Prepare();
                            using (MySqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    camera_name = dr["location"].ToString();
                                }
                            }
                        }
                        TableRow row = new TableRow();
                        TableCell cam = new TableCell() { Text = camera_name };
                        TableCell baris1 = new TableCell() { Text = satu.ToString() };
                        TableCell baris2 = new TableCell() { Text = dua.ToString() };
                        TableCell baris3 = new TableCell() { Text = tiga.ToString() };
                        TableCell baris4 = new TableCell() { Text = empat.ToString() };
                        //TableCell status = new TableCell() { Text = event_type };

                        //cam.Attributes.Add("onclick", $"setColumnSession('{camera}','none')");
                        //cam.Attributes.Add("class", "clickable");
                        //baris1.Attributes.Add("onclick", $"setColumnSession('{camera}', 'Face Recognition Match')");
                        //baris1.Attributes.Add("class", "clickable");
                        //baris2.Attributes.Add("onclick", $"setColumnSession('{camera}', 'Face Recognition Not Match')");
                        //baris2.Attributes.Add("class", "clickable");
                        //baris3.Attributes.Add("onclick", $"setColumnSession('{camera}', 'Plate Match')");
                        //baris3.Attributes.Add("class", "clickable");
                        //baris4.Attributes.Add("onclick", $"setColumnSession('{camera}', 'Plate MisMatch')");
                        //baris4.Attributes.Add("class", "clickable");
                        cam.Attributes.Add("onclick", $"window.open('statistic_detail.aspx?camera={camera}&type=none', '_blank');");
                        cam.Attributes.Add("class", "clickable");
                        baris1.Attributes.Add("onclick", $"window.open('statistic_detail.aspx?camera={camera}&type=FaceRecognitionMatch', '_blank');");
                        baris1.Attributes.Add("class", "clickable");
                        baris2.Attributes.Add("onclick", $"window.open('statistic_detail.aspx?camera={camera}&type=FaceRecognitionNotMatch', '_blank');");
                        baris2.Attributes.Add("class", "clickable");
                        baris3.Attributes.Add("onclick", $"window.open('statistic_detail.aspx?camera={camera}&type=PlateMatch', '_blank');");
                        baris3.Attributes.Add("class", "clickable");
                        baris4.Attributes.Add("onclick", $"window.open('statistic_detail.aspx?camera={camera}&type=PlateMisMatch', '_blank');");
                        baris4.Attributes.Add("class", "clickable");

                        row.Cells.Add(cam);
                        row.Cells.Add(baris1);
                        row.Cells.Add(baris2);
                        row.Cells.Add(baris3);
                        row.Cells.Add(baris4);




                        TbodyPie.Controls.Add(row);
                    }


                    //foreach (var innerList in list2D)
                    //{

                    //    // Iterasi melalui list dalam list dua dimensi
                    //    foreach (var item in innerList)
                    //    {
                    //        // Cek tipe data untuk mengetahui cara akses
                    //        if (item is int)
                    //        {
                    //            //Response.Write("ID: " + item);
                    //            var id = item;
                    //        }
                    //        else if (item is String)
                    //        {
                    //            //Response.Write("Occurred At: " + item);
                    //            var occurred_at = item;
                    //        }
                    //    }
                    //}

                }

                var serializer = new JavaScriptSerializer();
                var labelsJson = serializer.Serialize(labels);
                var jumlahJson = serializer.Serialize(jumlah);
                ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript3", $"fillChart3({labelsJson},{jumlahJson});", true);
            }


        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string lokasi = DropDownList2.SelectedValue;
            string person_name = TextBox3.Text;
            string type_event = DropDownList3.SelectedValue;
            Session["lokasi"] = lokasi;
            Session["person_name"] = person_name;
            Session["type_event"] = type_event;
            //FillEventHistory();
            //Session.Remove("lokasi");
            //Session.Remove("type_event");
            Response.Redirect("statistics.aspx");

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string selectedValue = DropDownList2.SelectedValue;
            //LoadDropdown2(selectedValue);
        }


    }
}